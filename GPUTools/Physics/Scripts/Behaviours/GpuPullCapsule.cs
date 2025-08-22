using System;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Behaviours
{
	// Token: 0x02000A3F RID: 2623
	[ExecuteInEditMode]
	public class GpuPullCapsule : GpuEditCapsule
	{
		// Token: 0x06004391 RID: 17297 RVA: 0x0013CEF9 File Offset: 0x0013B2F9
		public GpuPullCapsule()
		{
		}

		// Token: 0x06004392 RID: 17298 RVA: 0x0013CF01 File Offset: 0x0013B301
		private void OnEnable()
		{
			if (this.capsuleCollider == null)
			{
				this.capsuleCollider = base.GetComponent<CapsuleCollider>();
			}
			if (Application.isPlaying)
			{
				GPUCollidersManager.RegisterPullCapsule(this);
			}
			this.UpdateData();
		}

		// Token: 0x06004393 RID: 17299 RVA: 0x0013CF36 File Offset: 0x0013B336
		private void OnDisable()
		{
			if (Application.isPlaying)
			{
				GPUCollidersManager.DeregisterPullCapsule(this);
			}
		}
	}
}
