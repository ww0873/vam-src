using System;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006ED RID: 1773
	public class RigidFinger : SkeletalFinger
	{
		// Token: 0x06002AFD RID: 11005 RVA: 0x000E80CD File Offset: 0x000E64CD
		public RigidFinger()
		{
		}

		// Token: 0x06002AFE RID: 11006 RVA: 0x000E80E0 File Offset: 0x000E64E0
		private void Start()
		{
			for (int i = 0; i < this.bones.Length; i++)
			{
				if (this.bones[i] != null)
				{
					this.bones[i].GetComponent<Rigidbody>().maxAngularVelocity = float.PositiveInfinity;
				}
			}
		}

		// Token: 0x06002AFF RID: 11007 RVA: 0x000E8130 File Offset: 0x000E6530
		public override void UpdateFinger()
		{
			for (int i = 0; i < this.bones.Length; i++)
			{
				if (this.bones[i] != null)
				{
					CapsuleCollider component = this.bones[i].GetComponent<CapsuleCollider>();
					if (component != null)
					{
						component.direction = 2;
						this.bones[i].localScale = new Vector3(1f / base.transform.lossyScale.x, 1f / base.transform.lossyScale.y, 1f / base.transform.lossyScale.z);
						component.radius = base.GetBoneWidth(i) / 2f;
						component.height = base.GetBoneLength(i) + base.GetBoneWidth(i);
					}
					Rigidbody component2 = this.bones[i].GetComponent<Rigidbody>();
					if (component2)
					{
						component2.MovePosition(base.GetBoneCenter(i));
						component2.MoveRotation(base.GetBoneRotation(i));
					}
					else
					{
						this.bones[i].position = base.GetBoneCenter(i);
						this.bones[i].rotation = base.GetBoneRotation(i);
					}
				}
			}
		}

		// Token: 0x040022D7 RID: 8919
		public float filtering = 0.5f;
	}
}
