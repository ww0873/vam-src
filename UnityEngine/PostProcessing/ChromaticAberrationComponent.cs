using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200002F RID: 47
	public sealed class ChromaticAberrationComponent : PostProcessingComponentRenderTexture<ChromaticAberrationModel>
	{
		// Token: 0x060000FA RID: 250 RVA: 0x000095E3 File Offset: 0x000079E3
		public ChromaticAberrationComponent()
		{
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x060000FB RID: 251 RVA: 0x000095EC File Offset: 0x000079EC
		public override bool active
		{
			get
			{
				return base.model != null && base.model.enabled && base.model.settings.intensity > 0f && this.context != null && !this.context.interrupted;
			}
		}

		// Token: 0x060000FC RID: 252 RVA: 0x0000964D File Offset: 0x00007A4D
		public override void OnDisable()
		{
			GraphicsUtils.Destroy(this.m_SpectrumLut);
			this.m_SpectrumLut = null;
		}

		// Token: 0x060000FD RID: 253 RVA: 0x00009664 File Offset: 0x00007A64
		public override void Prepare(Material uberMaterial)
		{
			ChromaticAberrationModel.Settings settings = base.model.settings;
			Texture2D texture2D = settings.spectralTexture;
			if (texture2D == null)
			{
				if (this.m_SpectrumLut == null)
				{
					this.m_SpectrumLut = new Texture2D(3, 1, TextureFormat.RGB24, false)
					{
						name = "Chromatic Aberration Spectrum Lookup",
						filterMode = FilterMode.Bilinear,
						wrapMode = TextureWrapMode.Clamp,
						anisoLevel = 0,
						hideFlags = HideFlags.DontSave
					};
					Color[] pixels = new Color[]
					{
						new Color(1f, 0f, 0f),
						new Color(0f, 1f, 0f),
						new Color(0f, 0f, 1f)
					};
					this.m_SpectrumLut.SetPixels(pixels);
					this.m_SpectrumLut.Apply();
				}
				texture2D = this.m_SpectrumLut;
			}
			uberMaterial.EnableKeyword("CHROMATIC_ABERRATION");
			uberMaterial.SetFloat(ChromaticAberrationComponent.Uniforms._ChromaticAberration_Amount, settings.intensity * 0.03f);
			uberMaterial.SetTexture(ChromaticAberrationComponent.Uniforms._ChromaticAberration_Spectrum, texture2D);
		}

		// Token: 0x04000146 RID: 326
		private Texture2D m_SpectrumLut;

		// Token: 0x02000030 RID: 48
		private static class Uniforms
		{
			// Token: 0x060000FE RID: 254 RVA: 0x0000978F File Offset: 0x00007B8F
			// Note: this type is marked as 'beforefieldinit'.
			static Uniforms()
			{
			}

			// Token: 0x04000147 RID: 327
			internal static readonly int _ChromaticAberration_Amount = Shader.PropertyToID("_ChromaticAberration_Amount");

			// Token: 0x04000148 RID: 328
			internal static readonly int _ChromaticAberration_Spectrum = Shader.PropertyToID("_ChromaticAberration_Spectrum");
		}
	}
}
