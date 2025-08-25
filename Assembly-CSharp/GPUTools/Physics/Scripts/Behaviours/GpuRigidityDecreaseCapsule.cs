using System;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Behaviours
{
	// Token: 0x02000A41 RID: 2625
	[ExecuteInEditMode]
	public class GpuRigidityDecreaseCapsule : GpuEditCapsule
	{
		// Token: 0x06004397 RID: 17303 RVA: 0x0013CF97 File Offset: 0x0013B397
		public GpuRigidityDecreaseCapsule()
		{
		}

		// Token: 0x06004398 RID: 17304 RVA: 0x0013CF9F File Offset: 0x0013B39F
		private void OnEnable()
		{
			if (this.capsuleCollider == null)
			{
				this.capsuleCollider = base.GetComponent<CapsuleCollider>();
			}
			if (Application.isPlaying)
			{
				GPUCollidersManager.RegisterRigidityDecreaseCapsule(this);
			}
			this.UpdateData();
		}

		// Token: 0x06004399 RID: 17305 RVA: 0x0013CFD4 File Offset: 0x0013B3D4
		private void OnDisable()
		{
			if (Application.isPlaying)
			{
				GPUCollidersManager.DeregisterRigidityDecreaseCapsule(this);
			}
		}
	}
}
