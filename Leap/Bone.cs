using System;

namespace Leap
{
	// Token: 0x020005B3 RID: 1459
	[Serializable]
	public class Bone : IEquatable<Bone>
	{
		// Token: 0x06002497 RID: 9367 RVA: 0x000D3A80 File Offset: 0x000D1E80
		public Bone()
		{
			this.Type = Bone.BoneType.TYPE_INVALID;
		}

		// Token: 0x06002498 RID: 9368 RVA: 0x000D3A90 File Offset: 0x000D1E90
		public Bone(Vector prevJoint, Vector nextJoint, Vector center, Vector direction, float length, float width, Bone.BoneType type, LeapQuaternion rotation)
		{
			this.PrevJoint = prevJoint;
			this.NextJoint = nextJoint;
			this.Center = center;
			this.Direction = direction;
			this.Rotation = rotation;
			this.Length = length;
			this.Width = width;
			this.Type = type;
		}

		// Token: 0x06002499 RID: 9369 RVA: 0x000D3AE0 File Offset: 0x000D1EE0
		public bool Equals(Bone other)
		{
			return this.Center == other.Center && this.Direction == other.Direction && this.Length == other.Length;
		}

		// Token: 0x0600249A RID: 9370 RVA: 0x000D3B1F File Offset: 0x000D1F1F
		public override string ToString()
		{
			return Enum.GetName(typeof(Bone.BoneType), this.Type) + " bone";
		}

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x0600249B RID: 9371 RVA: 0x000D3B45 File Offset: 0x000D1F45
		public LeapTransform Basis
		{
			get
			{
				return new LeapTransform(this.PrevJoint, this.Rotation);
			}
		}

		// Token: 0x04001EC3 RID: 7875
		public Vector PrevJoint;

		// Token: 0x04001EC4 RID: 7876
		public Vector NextJoint;

		// Token: 0x04001EC5 RID: 7877
		public Vector Center;

		// Token: 0x04001EC6 RID: 7878
		public Vector Direction;

		// Token: 0x04001EC7 RID: 7879
		public float Length;

		// Token: 0x04001EC8 RID: 7880
		public float Width;

		// Token: 0x04001EC9 RID: 7881
		public Bone.BoneType Type;

		// Token: 0x04001ECA RID: 7882
		public LeapQuaternion Rotation;

		// Token: 0x020005B4 RID: 1460
		public enum BoneType
		{
			// Token: 0x04001ECC RID: 7884
			TYPE_INVALID = -1,
			// Token: 0x04001ECD RID: 7885
			TYPE_METACARPAL,
			// Token: 0x04001ECE RID: 7886
			TYPE_PROXIMAL,
			// Token: 0x04001ECF RID: 7887
			TYPE_INTERMEDIATE,
			// Token: 0x04001ED0 RID: 7888
			TYPE_DISTAL
		}
	}
}
