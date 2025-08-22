using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000E87 RID: 3719
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Other/Screen Overlay")]
	public class ScreenOverlay : PostEffectsBase
	{
		// Token: 0x0600712A RID: 28970 RVA: 0x002AF7AA File Offset: 0x002ADBAA
		public ScreenOverlay()
		{
		}

		// Token: 0x0600712B RID: 28971 RVA: 0x002AF7C4 File Offset: 0x002ADBC4
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.overlayMaterial = base.CheckShaderAndCreateMaterial(this.overlayShader, this.overlayMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x0600712C RID: 28972 RVA: 0x002AF800 File Offset: 0x002ADC00
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			Vector4 value = new Vector4(1f, 0f, 0f, 1f);
			this.overlayMaterial.SetVector("_UV_Transform", value);
			this.overlayMaterial.SetFloat("_Intensity", this.intensity);
			this.overlayMaterial.SetTexture("_Overlay", this.texture);
			Graphics.Blit(source, destination, this.overlayMaterial, (int)this.blendMode);
		}

		// Token: 0x04006480 RID: 25728
		public ScreenOverlay.OverlayBlendMode blendMode = ScreenOverlay.OverlayBlendMode.Overlay;

		// Token: 0x04006481 RID: 25729
		public float intensity = 1f;

		// Token: 0x04006482 RID: 25730
		public Texture2D texture;

		// Token: 0x04006483 RID: 25731
		public Shader overlayShader;

		// Token: 0x04006484 RID: 25732
		private Material overlayMaterial;

		// Token: 0x02000E88 RID: 3720
		public enum OverlayBlendMode
		{
			// Token: 0x04006486 RID: 25734
			Additive,
			// Token: 0x04006487 RID: 25735
			ScreenBlend,
			// Token: 0x04006488 RID: 25736
			Multiply,
			// Token: 0x04006489 RID: 25737
			Overlay,
			// Token: 0x0400648A RID: 25738
			AlphaBlend
		}
	}
}
