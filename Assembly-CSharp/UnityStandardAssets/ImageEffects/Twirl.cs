using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000E97 RID: 3735
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Displacement/Twirl")]
	public class Twirl : ImageEffectBase
	{
		// Token: 0x0600714D RID: 29005 RVA: 0x002B1114 File Offset: 0x002AF514
		public Twirl()
		{
		}

		// Token: 0x0600714E RID: 29006 RVA: 0x002B1151 File Offset: 0x002AF551
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			ImageEffects.RenderDistortion(base.material, source, destination, this.angle, this.center, this.radius);
		}

		// Token: 0x040064E5 RID: 25829
		public Vector2 radius = new Vector2(0.3f, 0.3f);

		// Token: 0x040064E6 RID: 25830
		[Range(0f, 360f)]
		public float angle = 50f;

		// Token: 0x040064E7 RID: 25831
		public Vector2 center = new Vector2(0.5f, 0.5f);
	}
}
