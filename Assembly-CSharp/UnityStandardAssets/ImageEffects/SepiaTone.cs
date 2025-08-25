using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000E8C RID: 3724
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Color Adjustments/Sepia Tone")]
	public class SepiaTone : ImageEffectBase
	{
		// Token: 0x06007139 RID: 28985 RVA: 0x002B00FE File Offset: 0x002AE4FE
		public SepiaTone()
		{
		}

		// Token: 0x0600713A RID: 28986 RVA: 0x002B0106 File Offset: 0x002AE506
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			Graphics.Blit(source, destination, base.material);
		}
	}
}
