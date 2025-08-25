using System;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Behaviours
{
	// Token: 0x02000A3E RID: 2622
	[ExecuteInEditMode]
	public class GpuHoldCapsule : GpuEditCapsule
	{
		// Token: 0x0600438E RID: 17294 RVA: 0x0013CEAA File Offset: 0x0013B2AA
		public GpuHoldCapsule()
		{
		}

		// Token: 0x0600438F RID: 17295 RVA: 0x0013CEB2 File Offset: 0x0013B2B2
		private void OnEnable()
		{
			if (this.capsuleCollider == null)
			{
				this.capsuleCollider = base.GetComponent<CapsuleCollider>();
			}
			if (Application.isPlaying)
			{
				GPUCollidersManager.RegisterHoldCapsule(this);
			}
			this.UpdateData();
		}

		// Token: 0x06004390 RID: 17296 RVA: 0x0013CEE7 File Offset: 0x0013B2E7
		private void OnDisable()
		{
			if (Application.isPlaying)
			{
				GPUCollidersManager.DeregisterHoldCapsule(this);
			}
		}
	}
}
