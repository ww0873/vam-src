using System;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Behaviours
{
	// Token: 0x02000A38 RID: 2616
	[ExecuteInEditMode]
	public class GpuBrushCapsule : GpuEditCapsule
	{
		// Token: 0x0600437A RID: 17274 RVA: 0x0013CCD1 File Offset: 0x0013B0D1
		public GpuBrushCapsule()
		{
		}

		// Token: 0x0600437B RID: 17275 RVA: 0x0013CCD9 File Offset: 0x0013B0D9
		private void OnEnable()
		{
			if (this.capsuleCollider == null)
			{
				this.capsuleCollider = base.GetComponent<CapsuleCollider>();
			}
			if (Application.isPlaying)
			{
				GPUCollidersManager.RegisterBrushCapsule(this);
			}
			this.UpdateData();
		}

		// Token: 0x0600437C RID: 17276 RVA: 0x0013CD0E File Offset: 0x0013B10E
		private void OnDisable()
		{
			if (Application.isPlaying)
			{
				GPUCollidersManager.DeregisterBrushCapsule(this);
			}
		}
	}
}
