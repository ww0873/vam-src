using System;

namespace Leap.Unity
{
	// Token: 0x020006EF RID: 1775
	public class SkeletalFinger : FingerModel
	{
		// Token: 0x06002B05 RID: 11013 RVA: 0x000E7FE7 File Offset: 0x000E63E7
		public SkeletalFinger()
		{
		}

		// Token: 0x06002B06 RID: 11014 RVA: 0x000E7FEF File Offset: 0x000E63EF
		public override void InitFinger()
		{
			this.SetPositions();
		}

		// Token: 0x06002B07 RID: 11015 RVA: 0x000E7FF7 File Offset: 0x000E63F7
		public override void UpdateFinger()
		{
			this.SetPositions();
		}

		// Token: 0x06002B08 RID: 11016 RVA: 0x000E8000 File Offset: 0x000E6400
		protected void SetPositions()
		{
			for (int i = 0; i < this.bones.Length; i++)
			{
				if (this.bones[i] != null)
				{
					this.bones[i].transform.position = base.GetBoneCenter(i);
					this.bones[i].transform.rotation = base.GetBoneRotation(i);
				}
			}
			for (int j = 0; j < this.joints.Length; j++)
			{
				if (this.joints[j] != null)
				{
					this.joints[j].transform.position = base.GetJointPosition(j + 1);
					this.joints[j].transform.rotation = base.GetBoneRotation(j + 1);
				}
			}
		}
	}
}
