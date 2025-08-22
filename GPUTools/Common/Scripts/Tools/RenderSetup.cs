using System;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools
{
	// Token: 0x020009D5 RID: 2517
	[RequireComponent(typeof(Camera))]
	public class RenderSetup : MonoBehaviour
	{
		// Token: 0x06003F83 RID: 16259 RVA: 0x0012F21A File Offset: 0x0012D61A
		public RenderSetup()
		{
		}

		// Token: 0x06003F84 RID: 16260 RVA: 0x0012F222 File Offset: 0x0012D622
		private void Start()
		{
			Application.targetFrameRate = this.targetFrameRate;
			base.GetComponent<Camera>().depthTextureMode = this.mode;
		}

		// Token: 0x04003014 RID: 12308
		[SerializeField]
		private DepthTextureMode mode;

		// Token: 0x04003015 RID: 12309
		[SerializeField]
		private int targetFrameRate;
	}
}
