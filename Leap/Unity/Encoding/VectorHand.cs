using System;
using UnityEngine;

namespace Leap.Unity.Encoding
{
	// Token: 0x020006DC RID: 1756
	[Serializable]
	public class VectorHand
	{
		// Token: 0x06002A37 RID: 10807 RVA: 0x000E3EB7 File Offset: 0x000E22B7
		public VectorHand()
		{
		}

		// Token: 0x06002A38 RID: 10808 RVA: 0x000E3EBF File Offset: 0x000E22BF
		public VectorHand(Hand hand) : this()
		{
			this.Encode(hand);
		}

		// Token: 0x17000554 RID: 1364
		// (get) Token: 0x06002A39 RID: 10809 RVA: 0x000E3ECE File Offset: 0x000E22CE
		public Vector3[] jointPositions
		{
			get
			{
				if (this._backingJointPositions == null)
				{
					this._backingJointPositions = new Vector3[25];
				}
				return this._backingJointPositions;
			}
		}

		// Token: 0x06002A3A RID: 10810 RVA: 0x000E3EF0 File Offset: 0x000E22F0
		public void Encode(Hand fromHand)
		{
			this.isLeft = fromHand.IsLeft;
			this.palmPos = fromHand.PalmPosition.ToVector3();
			this.palmRot = fromHand.Rotation.ToQuaternion();
			int num = 0;
			for (int i = 0; i < 5; i++)
			{
				Vector3 vector = VectorHand.ToLocal(fromHand.Fingers[i].bones[0].PrevJoint.ToVector3(), this.palmPos, this.palmRot);
				this.jointPositions[num++] = vector;
				for (int j = 0; j < 4; j++)
				{
					Vector3 vector2 = VectorHand.ToLocal(fromHand.Fingers[i].bones[j].NextJoint.ToVector3(), this.palmPos, this.palmRot);
					this.jointPositions[num++] = vector2;
				}
			}
		}

		// Token: 0x06002A3B RID: 10811 RVA: 0x000E3FE0 File Offset: 0x000E23E0
		public void Decode(Hand intoHand)
		{
			Vector3 vector = Vector3.zero;
			Vector3 vector2 = Vector3.zero;
			Quaternion quaternion = Quaternion.identity;
			for (int i = 0; i < 5; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					int boneIdx = i * 4 + j;
					vector = this.jointPositions[i * 5 + j];
					vector2 = this.jointPositions[i * 5 + j + 1];
					if ((vector2 - vector).normalized == Vector3.zero)
					{
						quaternion = Quaternion.identity;
					}
					else
					{
						quaternion = Quaternion.LookRotation((vector2 - vector).normalized, Vector3.Cross((vector2 - vector).normalized, (i != 0) ? Vector3.right : ((!this.isLeft) ? Vector3.up : (-Vector3.up))));
					}
					vector2 = VectorHand.ToWorld(vector2, this.palmPos, this.palmRot);
					vector = VectorHand.ToWorld(vector, this.palmPos, this.palmRot);
					quaternion = this.palmRot * quaternion;
					intoHand.GetBone(boneIdx).Fill(vector.ToVector(), vector2.ToVector(), ((vector2 + vector) / 2f).ToVector(), (this.palmRot * Vector3.forward).ToVector(), (vector - vector2).magnitude, 0.01f, (Bone.BoneType)j, quaternion.ToLeapQuaternion());
				}
				intoHand.Fingers[i].Fill(-1L, (!this.isLeft) ? 1 : 0, i, Time.time, vector2.ToVector(), (quaternion * Vector3.forward).ToVector(), 1f, 1f, true, (Finger.FingerType)i, null, null, null, null);
			}
			intoHand.Arm.Fill(VectorHand.ToWorld(new Vector3(0f, 0f, -0.3f), this.palmPos, this.palmRot).ToVector(), VectorHand.ToWorld(new Vector3(0f, 0f, -0.055f), this.palmPos, this.palmRot).ToVector(), VectorHand.ToWorld(new Vector3(0f, 0f, -0.125f), this.palmPos, this.palmRot).ToVector(), Vector.Zero, 0.3f, 0.05f, this.palmRot.ToLeapQuaternion());
			intoHand.Fill(-1L, (!this.isLeft) ? 1 : 0, 1f, 0.5f, 100f, 0.5f, 50f, 0.085f, this.isLeft, 1f, null, this.palmPos.ToVector(), this.palmPos.ToVector(), Vector3.zero.ToVector(), (this.palmRot * Vector3.down).ToVector(), this.palmRot.ToLeapQuaternion(), (this.palmRot * Vector3.forward).ToVector(), VectorHand.ToWorld(new Vector3(0f, 0f, -0.055f), this.palmPos, this.palmRot).ToVector());
		}

		// Token: 0x17000555 RID: 1365
		// (get) Token: 0x06002A3C RID: 10812 RVA: 0x000E4339 File Offset: 0x000E2739
		public int numBytesRequired
		{
			get
			{
				return 86;
			}
		}

		// Token: 0x06002A3D RID: 10813 RVA: 0x000E4340 File Offset: 0x000E2740
		public void ReadBytes(byte[] bytes, ref int offset)
		{
			if (bytes.Length - offset < this.numBytesRequired)
			{
				throw new IndexOutOfRangeException(string.Concat(new object[]
				{
					"Not enough room to read bytes for VectorHand encoding starting at offset ",
					offset,
					" for array of size ",
					bytes,
					"; need at least ",
					this.numBytesRequired,
					" bytes from the offset position."
				}));
			}
			this.isLeft = (bytes[offset++] == 0);
			for (int i = 0; i < 3; i++)
			{
				this.palmPos[i] = Convert.ToSingle(BitConverterNonAlloc.ToInt16(bytes, ref offset)) / 4096f;
			}
			this.palmRot = Utils.DecompressBytesToQuat(bytes, ref offset);
			for (int j = 0; j < 25; j++)
			{
				for (int k = 0; k < 3; k++)
				{
					this.jointPositions[j][k] = VectorHandExtensions.ByteToFloat(bytes[offset++], 0.3f);
				}
			}
		}

		// Token: 0x06002A3E RID: 10814 RVA: 0x000E4448 File Offset: 0x000E2848
		public void FillBytes(byte[] bytesToFill, ref int offset)
		{
			if (this._backingJointPositions == null)
			{
				throw new InvalidOperationException("Joint positions array is null. You must fill a VectorHand with data before you can use it to fill byte representations.");
			}
			if (bytesToFill.Length - offset < this.numBytesRequired)
			{
				throw new IndexOutOfRangeException(string.Concat(new object[]
				{
					"Not enough room to fill bytes for VectorHand encoding starting at offset ",
					offset,
					" for array of size ",
					bytesToFill.Length,
					"; need at least ",
					this.numBytesRequired,
					" bytes from the offset position."
				}));
			}
			bytesToFill[offset++] = ((!this.isLeft) ? 1 : 0);
			for (int i = 0; i < 3; i++)
			{
				BitConverterNonAlloc.GetBytes(Convert.ToInt16(this.palmPos[i] * 4096f), bytesToFill, ref offset);
			}
			Utils.CompressQuatToBytes(this.palmRot, bytesToFill, ref offset);
			for (int j = 0; j < 25; j++)
			{
				for (int k = 0; k < 3; k++)
				{
					bytesToFill[offset++] = VectorHandExtensions.FloatToByte(this.jointPositions[j][k], 0.3f);
				}
			}
		}

		// Token: 0x06002A3F RID: 10815 RVA: 0x000E4578 File Offset: 0x000E2978
		public void FillBytes(byte[] bytesToFill)
		{
			int num = 0;
			this.FillBytes(bytesToFill, ref num);
		}

		// Token: 0x06002A40 RID: 10816 RVA: 0x000E4590 File Offset: 0x000E2990
		public void ReadBytes(byte[] bytes, ref int offset, Hand intoHand)
		{
			this.ReadBytes(bytes, ref offset);
			this.Decode(intoHand);
		}

		// Token: 0x06002A41 RID: 10817 RVA: 0x000E45A1 File Offset: 0x000E29A1
		public void FillBytes(byte[] bytes, ref int offset, Hand fromHand)
		{
			this.Encode(fromHand);
			this.FillBytes(bytes, ref offset);
		}

		// Token: 0x06002A42 RID: 10818 RVA: 0x000E45B2 File Offset: 0x000E29B2
		public static Vector3 ToWorld(Vector3 localPoint, Vector3 localOrigin, Quaternion localRot)
		{
			return localRot * localPoint + localOrigin;
		}

		// Token: 0x06002A43 RID: 10819 RVA: 0x000E45C1 File Offset: 0x000E29C1
		public static Vector3 ToLocal(Vector3 worldPoint, Vector3 localOrigin, Quaternion localRot)
		{
			return Quaternion.Inverse(localRot) * (worldPoint - localOrigin);
		}

		// Token: 0x0400226F RID: 8815
		public const int NUM_JOINT_POSITIONS = 25;

		// Token: 0x04002270 RID: 8816
		public bool isLeft;

		// Token: 0x04002271 RID: 8817
		public Vector3 palmPos;

		// Token: 0x04002272 RID: 8818
		public Quaternion palmRot;

		// Token: 0x04002273 RID: 8819
		[SerializeField]
		private Vector3[] _backingJointPositions;
	}
}
