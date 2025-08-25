using System;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Behaviours
{
	// Token: 0x02000A37 RID: 2615
	[ExecuteInEditMode]
	public class CapsuleLineSphereCollider : LineSphereCollider
	{
		// Token: 0x06004375 RID: 17269 RVA: 0x0013C6B6 File Offset: 0x0013AAB6
		public CapsuleLineSphereCollider()
		{
		}

		// Token: 0x06004376 RID: 17270 RVA: 0x0013C6CC File Offset: 0x0013AACC
		public void UpdateData(float radius, float height, int direction, Vector3 center)
		{
			float num = radius + this.oversizeRadius;
			float num2 = (height + this.oversizeHeight) * 0.5f;
			if (num2 < num)
			{
				num2 = num;
			}
			this.RadiusA = num;
			this.RadiusB = num;
			float num3 = num2 - num;
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

		// Token: 0x06004377 RID: 17271 RVA: 0x0013C884 File Offset: 0x0013AC84
		public void UpdateData()
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

		// Token: 0x06004378 RID: 17272 RVA: 0x0013CA7D File Offset: 0x0013AE7D
		private void OnEnable()
		{
			if (this.capsuleCollider == null)
			{
				this.capsuleCollider = base.GetComponent<CapsuleCollider>();
			}
			if (Application.isPlaying)
			{
				GPUCollidersManager.RegisterLineSphereCollider(this);
			}
			this.UpdateData();
		}

		// Token: 0x06004379 RID: 17273 RVA: 0x0013CAB2 File Offset: 0x0013AEB2
		private void OnDisable()
		{
			if (Application.isPlaying)
			{
				GPUCollidersManager.DeregisterLineSphereCollider(this);
			}
		}

		// Token: 0x0400326F RID: 12911
		public CapsuleCollider capsuleCollider;

		// Token: 0x04003270 RID: 12912
		public float oversizeRadius;

		// Token: 0x04003271 RID: 12913
		public float oversizeHeight;

		// Token: 0x04003272 RID: 12914
		public float friction = 1f;
	}
}
