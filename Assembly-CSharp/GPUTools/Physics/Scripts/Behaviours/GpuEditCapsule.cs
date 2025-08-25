using System;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Behaviours
{
	// Token: 0x02000A3A RID: 2618
	[ExecuteInEditMode]
	public class GpuEditCapsule : LineSphereCollider
	{
		// Token: 0x06004380 RID: 17280 RVA: 0x0013CAC4 File Offset: 0x0013AEC4
		public GpuEditCapsule()
		{
		}

		// Token: 0x06004381 RID: 17281 RVA: 0x0013CAD8 File Offset: 0x0013AED8
		public virtual void UpdateData()
		{
			if (this.capsuleCollider != null)
			{
				float num = this.capsuleCollider.radius + this.oversizeRadius;
				float num2 = (this.capsuleCollider.height + this.oversizeHeight) * 0.5f;
				if (num2 < num)
				{
					num2 = num;
				}
				this.RadiusA = num;
				this.RadiusB = num;
				Vector3 center = this.capsuleCollider.center;
				float num3 = num2 - num;
				int direction = this.capsuleCollider.direction;
				if (direction != 0)
				{
					if (direction != 1)
					{
						if (direction == 2)
						{
							this.A.x = center.x;
							this.A.y = center.y;
							this.A.z = center.z + num3;
							this.B.x = center.x;
							this.B.y = center.y;
							this.B.z = center.z - num3;
						}
					}
					else
					{
						this.A.x = center.x;
						this.A.y = center.y + num3;
						this.A.z = center.z;
						this.B.x = center.x;
						this.B.y = center.y - num3;
						this.B.z = center.z;
					}
				}
				else
				{
					this.A.x = center.x + num3;
					this.A.y = center.y;
					this.A.z = center.z;
					this.B.x = center.x - num3;
					this.B.y = center.y;
					this.B.z = center.z;
				}
			}
		}

		// Token: 0x04003273 RID: 12915
		public CapsuleCollider capsuleCollider;

		// Token: 0x04003274 RID: 12916
		public float oversizeRadius;

		// Token: 0x04003275 RID: 12917
		public float oversizeHeight;

		// Token: 0x04003276 RID: 12918
		public float strength = 1f;
	}
}
