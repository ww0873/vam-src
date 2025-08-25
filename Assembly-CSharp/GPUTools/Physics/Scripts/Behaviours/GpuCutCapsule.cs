using System;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Behaviours
{
	// Token: 0x02000A39 RID: 2617
	[ExecuteInEditMode]
	public class GpuCutCapsule : GpuEditCapsule
	{
		// Token: 0x0600437D RID: 17277 RVA: 0x0013CD20 File Offset: 0x0013B120
		public GpuCutCapsule()
		{
		}

		// Token: 0x0600437E RID: 17278 RVA: 0x0013CD28 File Offset: 0x0013B128
		private void OnEnable()
		{
			if (this.capsuleCollider == null)
			{
				this.capsuleCollider = base.GetComponent<CapsuleCollider>();
			}
			if (Application.isPlaying)
			{
				GPUCollidersManager.RegisterCutCapsule(this);
			}
			this.UpdateData();
		}

		// Token: 0x0600437F RID: 17279 RVA: 0x0013CD5D File Offset: 0x0013B15D
		private void OnDisable()
		{
			if (Application.isPlaying)
			{
				GPUCollidersManager.DeregisterCutCapsule(this);
			}
		}
	}
}
