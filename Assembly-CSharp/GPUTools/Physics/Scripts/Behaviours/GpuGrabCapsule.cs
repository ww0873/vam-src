using System;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Behaviours
{
	// Token: 0x02000A3B RID: 2619
	[ExecuteInEditMode]
	public class GpuGrabCapsule : GpuEditCapsule
	{
		// Token: 0x06004382 RID: 17282 RVA: 0x0013CD6F File Offset: 0x0013B16F
		public GpuGrabCapsule()
		{
		}

		// Token: 0x06004383 RID: 17283 RVA: 0x0013CD77 File Offset: 0x0013B177
		private void OnEnable()
		{
			if (this.capsuleCollider == null)
			{
				this.capsuleCollider = base.GetComponent<CapsuleCollider>();
			}
			if (Application.isPlaying)
			{
				GPUCollidersManager.RegisterGrabCapsule(this);
			}
			this.UpdateData();
		}

		// Token: 0x06004384 RID: 17284 RVA: 0x0013CDAC File Offset: 0x0013B1AC
		private void OnDisable()
		{
			if (Application.isPlaying)
			{
				GPUCollidersManager.DeregisterGrabCapsule(this);
			}
		}

		// Token: 0x170008A1 RID: 2209
		// (get) Token: 0x06004385 RID: 17285 RVA: 0x0013CDBE File Offset: 0x0013B1BE
		public Matrix4x4 changeMatrix
		{
			get
			{
				return this._changeMatrix;
			}
		}

		// Token: 0x06004386 RID: 17286 RVA: 0x0013CDC6 File Offset: 0x0013B1C6
		public override void UpdateData()
		{
			base.UpdateData();
			this._changeMatrix = base.transform.localToWorldMatrix * this._lastWorldToLocalMatrix;
			this._lastWorldToLocalMatrix = base.transform.worldToLocalMatrix;
		}

		// Token: 0x04003277 RID: 12919
		protected Matrix4x4 _lastWorldToLocalMatrix;

		// Token: 0x04003278 RID: 12920
		protected Matrix4x4 _changeMatrix;
	}
}
