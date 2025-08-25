using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000E6E RID: 3694
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Color Adjustments/Color Correction (Ramp)")]
	public class ColorCorrectionRamp : ImageEffectBase
	{
		// Token: 0x060070BE RID: 28862 RVA: 0x002AB314 File Offset: 0x002A9714
		public ColorCorrectionRamp()
		{
		}

		// Token: 0x060070BF RID: 28863 RVA: 0x002AB31C File Offset: 0x002A971C
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			base.material.SetTexture("_RampTex", this.textureRamp);
			Graphics.Blit(source, destination, base.material);
		}

		// Token: 0x040063B9 RID: 25529
		public Texture textureRamp;
	}
}
