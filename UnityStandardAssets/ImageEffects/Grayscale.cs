using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000E7E RID: 3710
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Color Adjustments/Grayscale")]
	public class Grayscale : ImageEffectBase
	{
		// Token: 0x060070F6 RID: 28918 RVA: 0x002AE1E2 File Offset: 0x002AC5E2
		public Grayscale()
		{
		}

		// Token: 0x060070F7 RID: 28919 RVA: 0x002AE1EA File Offset: 0x002AC5EA
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			base.material.SetTexture("_RampTex", this.textureRamp);
			base.material.SetFloat("_RampOffset", this.rampOffset);
			Graphics.Blit(source, destination, base.material);
		}

		// Token: 0x0400644F RID: 25679
		public Texture textureRamp;

		// Token: 0x04006450 RID: 25680
		[Range(-1f, 1f)]
		public float rampOffset;
	}
}
