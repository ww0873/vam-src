using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000E9A RID: 3738
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Displacement/Vortex")]
	public class Vortex : ImageEffectBase
	{
		// Token: 0x06007152 RID: 29010 RVA: 0x002B150A File Offset: 0x002AF90A
		public Vortex()
		{
		}

		// Token: 0x06007153 RID: 29011 RVA: 0x002B1547 File Offset: 0x002AF947
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			ImageEffects.RenderDistortion(base.material, source, destination, this.angle, this.center, this.radius);
		}

		// Token: 0x040064F9 RID: 25849
		public Vector2 radius = new Vector2(0.4f, 0.4f);

		// Token: 0x040064FA RID: 25850
		public float angle = 50f;

		// Token: 0x040064FB RID: 25851
		public Vector2 center = new Vector2(0.5f, 0.5f);
	}
}
