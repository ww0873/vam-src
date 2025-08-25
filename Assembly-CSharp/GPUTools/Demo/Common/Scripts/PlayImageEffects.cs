using System;
using UnityEngine;

namespace GPUTools.Demo.Common.Scripts
{
	// Token: 0x020009E3 RID: 2531
	public class PlayImageEffects : MonoBehaviour
	{
		// Token: 0x06003FC6 RID: 16326 RVA: 0x001303AA File Offset: 0x0012E7AA
		public PlayImageEffects()
		{
		}

		// Token: 0x06003FC7 RID: 16327 RVA: 0x001303B2 File Offset: 0x0012E7B2
		private void Start()
		{
			this.material = new Material(this.shader);
		}

		// Token: 0x06003FC8 RID: 16328 RVA: 0x001303C5 File Offset: 0x0012E7C5
		private void OnRenderImage(RenderTexture src, RenderTexture dest)
		{
			Graphics.Blit(src, dest, this.material);
		}

		// Token: 0x04003032 RID: 12338
		[SerializeField]
		private Shader shader;

		// Token: 0x04003033 RID: 12339
		private Material material;
	}
}
