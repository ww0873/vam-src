using System;

namespace Leap
{
	// Token: 0x020005B2 RID: 1458
	[Serializable]
	public class Arm : Bone, IEquatable<Arm>
	{
		// Token: 0x06002491 RID: 9361 RVA: 0x000D3B58 File Offset: 0x000D1F58
		public Arm()
		{
		}

		// Token: 0x06002492 RID: 9362 RVA: 0x000D3B60 File Offset: 0x000D1F60
		public Arm(Vector elbow, Vector wrist, Vector center, Vector direction, float length, float width, LeapQuaternion rotation) : base(elbow, wrist, center, direction, length, width, Bone.BoneType.TYPE_METACARPAL, rotation)
		{
		}

		// Token: 0x06002493 RID: 9363 RVA: 0x000D3B7F File Offset: 0x000D1F7F
		public bool Equals(Arm other)
		{
			return base.Equals(other);
		}

		// Token: 0x06002494 RID: 9364 RVA: 0x000D3B88 File Offset: 0x000D1F88
		public override string ToString()
		{
			return "Arm";
		}

		// Token: 0x1700044D RID: 1101
		// (get) Token: 0x06002495 RID: 9365 RVA: 0x000D3B8F File Offset: 0x000D1F8F
		public Vector ElbowPosition
		{
			get
			{
				return this.PrevJoint;
			}
		}

		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x06002496 RID: 9366 RVA: 0x000D3B97 File Offset: 0x000D1F97
		public Vector WristPosition
		{
			get
			{
				return this.NextJoint;
			}
		}
	}
}
