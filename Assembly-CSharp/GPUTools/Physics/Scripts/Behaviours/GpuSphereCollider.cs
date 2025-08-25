using System;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Behaviours
{
	// Token: 0x02000A44 RID: 2628
	[ExecuteInEditMode]
	public class GpuSphereCollider : MonoBehaviour
	{
		// Token: 0x060043A0 RID: 17312 RVA: 0x0013D084 File Offset: 0x0013B484
		public GpuSphereCollider()
		{
		}

		// Token: 0x170008A3 RID: 2211
		// (get) Token: 0x060043A1 RID: 17313 RVA: 0x0013D097 File Offset: 0x0013B497
		// (set) Token: 0x060043A2 RID: 17314 RVA: 0x0013D0BB File Offset: 0x0013B4BB
		public Vector3 center
		{
			get
			{
				if (this.sphereCollider != null)
				{
					return this.sphereCollider.center;
				}
				return Vector3.zero;
			}
			set
			{
				if (this.sphereCollider != null && this.sphereCollider.center != value)
				{
					this.sphereCollider.center = value;
				}
			}
		}

		// Token: 0x170008A4 RID: 2212
		// (get) Token: 0x060043A4 RID: 17316 RVA: 0x0013D104 File Offset: 0x0013B504
		// (set) Token: 0x060043A3 RID: 17315 RVA: 0x0013D0F0 File Offset: 0x0013B4F0
		public Vector3 worldCenter
		{
			get
			{
				return base.transform.TransformPoint(this.center);
			}
			set
			{
				this.center = base.transform.InverseTransformPoint(value);
			}
		}

		// Token: 0x170008A5 RID: 2213
		// (get) Token: 0x060043A5 RID: 17317 RVA: 0x0013D117 File Offset: 0x0013B517
		// (set) Token: 0x060043A6 RID: 17318 RVA: 0x0013D142 File Offset: 0x0013B542
		public float radius
		{
			get
			{
				if (this.sphereCollider != null)
				{
					return this.sphereCollider.radius + this.oversizeRadius;
				}
				return 0f;
			}
			set
			{
				if (this.sphereCollider != null && this.sphereCollider.radius != value)
				{
					this.sphereCollider.radius = value;
				}
			}
		}

		// Token: 0x170008A6 RID: 2214
		// (get) Token: 0x060043A7 RID: 17319 RVA: 0x0013D174 File Offset: 0x0013B574
		public float worldRadius
		{
			get
			{
				return this.radius * base.transform.lossyScale.x;
			}
		}

		// Token: 0x170008A7 RID: 2215
		// (get) Token: 0x060043A8 RID: 17320 RVA: 0x0013D19C File Offset: 0x0013B59C
		private float Scale
		{
			get
			{
				return Mathf.Max(Mathf.Max(base.transform.lossyScale.x, base.transform.lossyScale.y), base.transform.lossyScale.z);
			}
		}

		// Token: 0x060043A9 RID: 17321 RVA: 0x0013D1EC File Offset: 0x0013B5EC
		private void OnEnable()
		{
			if (this.sphereCollider == null)
			{
				this.sphereCollider = base.GetComponent<SphereCollider>();
			}
			if (Application.isPlaying)
			{
				GPUCollidersManager.RegisterSphereCollider(this);
			}
		}

		// Token: 0x060043AA RID: 17322 RVA: 0x0013D21B File Offset: 0x0013B61B
		private void OnDisable()
		{
			if (Application.isPlaying)
			{
				GPUCollidersManager.DeregisterSphereCollider(this);
			}
		}

		// Token: 0x060043AB RID: 17323 RVA: 0x0013D22D File Offset: 0x0013B62D
		private void OnDrawGizmos()
		{
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(this.worldCenter, this.worldRadius);
		}

		// Token: 0x0400327E RID: 12926
		public SphereCollider sphereCollider;

		// Token: 0x0400327F RID: 12927
		public float oversizeRadius;

		// Token: 0x04003280 RID: 12928
		public float friction = 1f;
	}
}
