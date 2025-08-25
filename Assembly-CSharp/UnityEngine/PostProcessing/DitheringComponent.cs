using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000035 RID: 53
	public sealed class DitheringComponent : PostProcessingComponentRenderTexture<DitheringModel>
	{
		// Token: 0x06000121 RID: 289 RVA: 0x0000ACFE File Offset: 0x000090FE
		public DitheringComponent()
		{
		}

		// Token: 0x17000027 RID: 39
		// (get) Token: 0x06000122 RID: 290 RVA: 0x0000AD06 File Offset: 0x00009106
		public override bool active
		{
			get
			{
				return base.model != null && base.model.enabled && this.context != null && !this.context.interrupted;
			}
		}

		// Token: 0x06000123 RID: 291 RVA: 0x0000AD3F File Offset: 0x0000913F
		public override void OnDisable()
		{
			this.noiseTextures = null;
		}

		// Token: 0x06000124 RID: 292 RVA: 0x0000AD48 File Offset: 0x00009148
		private void LoadNoiseTextures()
		{
			this.noiseTextures = new Texture2D[64];
			for (int i = 0; i < 64; i++)
			{
				this.noiseTextures[i] = Resources.Load<Texture2D>("Bluenoise64/LDR_LLL1_" + i);
			}
		}

		// Token: 0x06000125 RID: 293 RVA: 0x0000AD94 File Offset: 0x00009194
		public override void Prepare(Material uberMaterial)
		{
			if (++this.textureIndex >= 64)
			{
				this.textureIndex = 0;
			}
			float value = Random.value;
			float value2 = Random.value;
			if (this.noiseTextures == null)
			{
				this.LoadNoiseTextures();
			}
			Texture2D texture2D = this.noiseTextures[this.textureIndex];
			uberMaterial.EnableKeyword("DITHERING");
			uberMaterial.SetTexture(DitheringComponent.Uniforms._DitheringTex, texture2D);
			uberMaterial.SetVector(DitheringComponent.Uniforms._DitheringCoords, new Vector4((float)this.context.width / (float)texture2D.width, (float)this.context.height / (float)texture2D.height, value, value2));
		}

		// Token: 0x04000170 RID: 368
		private Texture2D[] noiseTextures;

		// Token: 0x04000171 RID: 369
		private int textureIndex;

		// Token: 0x04000172 RID: 370
		private const int k_TextureCount = 64;

		// Token: 0x02000036 RID: 54
		private static class Uniforms
		{
			// Token: 0x06000126 RID: 294 RVA: 0x0000AE3C File Offset: 0x0000923C
			// Note: this type is marked as 'beforefieldinit'.
			static Uniforms()
			{
			}

			// Token: 0x04000173 RID: 371
			internal static readonly int _DitheringTex = Shader.PropertyToID("_DitheringTex");

			// Token: 0x04000174 RID: 372
			internal static readonly int _DitheringCoords = Shader.PropertyToID("_DitheringCoords");
		}
	}
}
