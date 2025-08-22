using System;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Behaviours
{
	// Token: 0x02000A40 RID: 2624
	[ExecuteInEditMode]
	public class GpuPushCapsule : GpuEditCapsule
	{
		// Token: 0x06004394 RID: 17300 RVA: 0x0013CF48 File Offset: 0x0013B348
		public GpuPushCapsule()
		{
		}

		// Token: 0x06004395 RID: 17301 RVA: 0x0013CF50 File Offset: 0x0013B350
		private void OnEnable()
		{
			if (this.capsuleCollider == null)
			{
				this.capsuleCollider = base.GetComponent<CapsuleCollider>();
			}
			if (Application.isPlaying)
			{
				GPUCollidersManager.RegisterPushCapsule(this);
			}
			this.UpdateData();
		}

		// Token: 0x06004396 RID: 17302 RVA: 0x0013CF85 File Offset: 0x0013B385
		private void OnDisable()
		{
			if (Application.isPlaying)
			{
				GPUCollidersManager.DeregisterPushCapsule(this);
			}
		}
	}
}
