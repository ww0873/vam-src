using System;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Behaviours
{
	// Token: 0x02000A43 RID: 2627
	[ExecuteInEditMode]
	public class GpuRigiditySetCapsule : GpuEditCapsule
	{
		// Token: 0x0600439D RID: 17309 RVA: 0x0013D035 File Offset: 0x0013B435
		public GpuRigiditySetCapsule()
		{
		}

		// Token: 0x0600439E RID: 17310 RVA: 0x0013D03D File Offset: 0x0013B43D
		private void OnEnable()
		{
			if (this.capsuleCollider == null)
			{
				this.capsuleCollider = base.GetComponent<CapsuleCollider>();
			}
			if (Application.isPlaying)
			{
				GPUCollidersManager.RegisterRigiditySetCapsule(this);
			}
			this.UpdateData();
		}

		// Token: 0x0600439F RID: 17311 RVA: 0x0013D072 File Offset: 0x0013B472
		private void OnDisable()
		{
			if (Application.isPlaying)
			{
				GPUCollidersManager.DeregisterRigiditySetCapsule(this);
			}
		}
	}
}
