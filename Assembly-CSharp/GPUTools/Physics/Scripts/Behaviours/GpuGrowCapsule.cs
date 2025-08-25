using System;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Behaviours
{
	// Token: 0x02000A3D RID: 2621
	[ExecuteInEditMode]
	public class GpuGrowCapsule : GpuEditCapsule
	{
		// Token: 0x0600438B RID: 17291 RVA: 0x0013CE5B File Offset: 0x0013B25B
		public GpuGrowCapsule()
		{
		}

		// Token: 0x0600438C RID: 17292 RVA: 0x0013CE63 File Offset: 0x0013B263
		private void OnEnable()
		{
			if (this.capsuleCollider == null)
			{
				this.capsuleCollider = base.GetComponent<CapsuleCollider>();
			}
			if (Application.isPlaying)
			{
				GPUCollidersManager.RegisterGrowCapsule(this);
			}
			this.UpdateData();
		}

		// Token: 0x0600438D RID: 17293 RVA: 0x0013CE98 File Offset: 0x0013B298
		private void OnDisable()
		{
			if (Application.isPlaying)
			{
				GPUCollidersManager.DeregisterGrowCapsule(this);
			}
		}
	}
}
