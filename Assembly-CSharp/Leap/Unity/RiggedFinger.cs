using System;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006EB RID: 1771
	public class RiggedFinger : FingerModel
	{
		// Token: 0x06002ADB RID: 10971 RVA: 0x000E70B8 File Offset: 0x000E54B8
		static RiggedFinger()
		{
			Hand hand = TestHandFactory.MakeTestHand(true, 0, 0, TestHandFactory.UnitType.UnityUnits);
			for (int i = 0; i < 5; i++)
			{
				Bone bone = hand.Fingers[i].bones[3];
				RiggedFinger.s_standardFingertipLengths[i] = bone.Length;
			}
		}

		// Token: 0x06002ADC RID: 10972 RVA: 0x000E710D File Offset: 0x000E550D
		public RiggedFinger()
		{
		}

		// Token: 0x06002ADD RID: 10973 RVA: 0x000E7130 File Offset: 0x000E5530
		public Quaternion Reorientation()
		{
			return Quaternion.Inverse(Quaternion.LookRotation(this.modelFingerPointing, -this.modelPalmFacing));
		}

		// Token: 0x06002ADE RID: 10974 RVA: 0x000E7150 File Offset: 0x000E5550
		public override void UpdateFinger()
		{
			for (int i = 0; i < this.bones.Length; i++)
			{
				if (this.bones[i] != null)
				{
					this.bones[i].rotation = base.GetBoneRotation(i) * this.Reorientation();
					if (this.deformPosition)
					{
						Vector3 jointPosition = base.GetJointPosition(i);
						this.bones[i].position = jointPosition;
						if (i == 3 && this.scaleLastFingerBone)
						{
							Vector3 jointPosition2 = base.GetJointPosition(i + 1);
							float magnitude = (jointPosition2 - jointPosition).magnitude;
							float num = RiggedFinger.s_standardFingertipLengths[(int)this.fingerType];
							Vector3 localScale = this.bones[i].transform.localScale;
							int largestComponentIndex = this.getLargestComponentIndex(this.modelFingerPointing);
							localScale[largestComponentIndex] = magnitude / num;
							this.bones[i].transform.localScale = localScale;
						}
					}
				}
			}
		}

		// Token: 0x06002ADF RID: 10975 RVA: 0x000E7248 File Offset: 0x000E5648
		private int getLargestComponentIndex(Vector3 pointingVector)
		{
			float num = 0f;
			int result = 0;
			for (int i = 0; i < 3; i++)
			{
				float f = pointingVector[i];
				if (Mathf.Abs(f) > num)
				{
					result = i;
					num = Mathf.Abs(f);
				}
			}
			return result;
		}

		// Token: 0x06002AE0 RID: 10976 RVA: 0x000E728E File Offset: 0x000E568E
		public void SetupRiggedFinger(bool useMetaCarpals)
		{
			this.findBoneTransforms(useMetaCarpals);
			this.modelFingerPointing = this.calulateModelFingerPointing();
		}

		// Token: 0x06002AE1 RID: 10977 RVA: 0x000E72A4 File Offset: 0x000E56A4
		private void findBoneTransforms(bool useMetaCarpals)
		{
			if (!useMetaCarpals || this.fingerType == Finger.FingerType.TYPE_THUMB)
			{
				this.bones[1] = base.transform;
				this.bones[2] = base.transform.GetChild(0).transform;
				this.bones[3] = base.transform.GetChild(0).transform.GetChild(0).transform;
			}
			else
			{
				this.bones[0] = base.transform;
				this.bones[1] = base.transform.GetChild(0).transform;
				this.bones[2] = base.transform.GetChild(0).transform.GetChild(0).transform;
				this.bones[3] = base.transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).transform;
			}
		}

		// Token: 0x06002AE2 RID: 10978 RVA: 0x000E738C File Offset: 0x000E578C
		private Vector3 calulateModelFingerPointing()
		{
			Vector3 vectorToZero = base.transform.InverseTransformPoint(base.transform.position) - base.transform.InverseTransformPoint(base.transform.GetChild(0).transform.position);
			return RiggedHand.CalculateZeroedVector(vectorToZero);
		}

		// Token: 0x040022C7 RID: 8903
		[HideInInspector]
		public bool deformPosition;

		// Token: 0x040022C8 RID: 8904
		[HideInInspector]
		public bool scaleLastFingerBone;

		// Token: 0x040022C9 RID: 8905
		public Vector3 modelFingerPointing = Vector3.forward;

		// Token: 0x040022CA RID: 8906
		public Vector3 modelPalmFacing = -Vector3.up;

		// Token: 0x040022CB RID: 8907
		private static float[] s_standardFingertipLengths = new float[5];
	}
}
