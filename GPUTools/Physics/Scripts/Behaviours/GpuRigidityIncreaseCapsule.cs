using System;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Behaviours
{
	// Token: 0x02000A42 RID: 2626
	[ExecuteInEditMode]
	public class GpuRigidityIncreaseCapsule : GpuEditCapsule
	{
		// Token: 0x0600439A RID: 17306 RVA: 0x0013CFE6 File Offset: 0x0013B3E6
		public GpuRigidityIncreaseCapsule()
		{
		}

		// Token: 0x0600439B RID: 17307 RVA: 0x0013CFEE File Offset: 0x0013B3EE
		private void OnEnable()
		{
			if (this.capsuleCollider == null)
			{
				this.capsuleCollider = base.GetComponent<CapsuleCollider>();
			}
			if (Application.isPlaying)
			{
				GPUCollidersManager.RegisterRigidityIncreaseCapsule(this);
			}
			this.UpdateData();
		}

		// Token: 0x0600439C RID: 17308 RVA: 0x0013D023 File Offset: 0x0013B423
		private void OnDisable()
		{
			if (Application.isPlaying)
			{
				GPUCollidersManager.DeregisterRigidityIncreaseCapsule(this);
			}
		}
	}
}
