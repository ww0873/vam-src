using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200003D RID: 61
	public sealed class GrainComponent : PostProcessingComponentRenderTexture<GrainModel>
	{
		// Token: 0x0600013B RID: 315 RVA: 0x0000B835 File Offset: 0x00009C35
		public GrainComponent()
		{
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600013C RID: 316 RVA: 0x0000B840 File Offset: 0x00009C40
		public override bool active
		{
			get
			{
				return base.model != null && base.model.enabled && base.model.settings.intensity > 0f && SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf) && this.context != null && !this.context.interrupted;
			}
		}

		// Token: 0x0600013D RID: 317 RVA: 0x0000B8AC File Offset: 0x00009CAC
		public override void OnDisable()
		{
			GraphicsUtils.Destroy(this.m_GrainLookupRT);
			this.m_GrainLookupRT = null;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x0000B8C0 File Offset: 0x00009CC0
		public override void Prepare(Material uberMaterial)
		{
			GrainModel.Settings settings = base.model.settings;
			uberMaterial.EnableKeyword("GRAIN");
			float realtimeSinceStartup = Time.realtimeSinceStartup;
			float value = Random.value;
			float value2 = Random.value;
			if (this.m_GrainLookupRT == null || !this.m_GrainLookupRT.IsCreated())
			{
				GraphicsUtils.Destroy(this.m_GrainLookupRT);
				this.m_GrainLookupRT = new RenderTexture(192, 192, 0, RenderTextureFormat.ARGBHalf)
				{
					filterMode = FilterMode.Bilinear,
					wrapMode = TextureWrapMode.Repeat,
					anisoLevel = 0,
					name = "Grain Lookup Texture"
				};
				this.m_GrainLookupRT.Create();
			}
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Grain Generator");
			material.SetFloat(GrainComponent.Uniforms._Phase, realtimeSinceStartup / 20f);
			Graphics.Blit(null, this.m_GrainLookupRT, material, (!settings.colored) ? 0 : 1);
			uberMaterial.SetTexture(GrainComponent.Uniforms._GrainTex, this.m_GrainLookupRT);
			uberMaterial.SetVector(GrainComponent.Uniforms._Grain_Params1, new Vector2(settings.luminanceContribution, settings.intensity * 20f));
			uberMaterial.SetVector(GrainComponent.Uniforms._Grain_Params2, new Vector4((float)this.context.width / (float)this.m_GrainLookupRT.width / settings.size, (float)this.context.height / (float)this.m_GrainLookupRT.height / settings.size, value, value2));
		}

		// Token: 0x0400018E RID: 398
		private RenderTexture m_GrainLookupRT;

		// Token: 0x0200003E RID: 62
		private static class Uniforms
		{
			// Token: 0x0600013F RID: 319 RVA: 0x0000BA46 File Offset: 0x00009E46
			// Note: this type is marked as 'beforefieldinit'.
			static Uniforms()
			{
			}

			// Token: 0x0400018F RID: 399
			internal static readonly int _Grain_Params1 = Shader.PropertyToID("_Grain_Params1");

			// Token: 0x04000190 RID: 400
			internal static readonly int _Grain_Params2 = Shader.PropertyToID("_Grain_Params2");

			// Token: 0x04000191 RID: 401
			internal static readonly int _GrainTex = Shader.PropertyToID("_GrainTex");

			// Token: 0x04000192 RID: 402
			internal static readonly int _Phase = Shader.PropertyToID("_Phase");
		}
	}
}
