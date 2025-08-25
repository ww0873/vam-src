using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000E63 RID: 3683
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Bloom and Glow/Bloom (Optimized)")]
	public class BloomOptimized : PostEffectsBase
	{
		// Token: 0x06007093 RID: 28819 RVA: 0x002A9290 File Offset: 0x002A7690
		public BloomOptimized()
		{
		}

		// Token: 0x06007094 RID: 28820 RVA: 0x002A92C0 File Offset: 0x002A76C0
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.fastBloomMaterial = base.CheckShaderAndCreateMaterial(this.fastBloomShader, this.fastBloomMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06007095 RID: 28821 RVA: 0x002A92F9 File Offset: 0x002A76F9
		private void OnDisable()
		{
			if (this.fastBloomMaterial)
			{
				UnityEngine.Object.DestroyImmediate(this.fastBloomMaterial);
			}
		}

		// Token: 0x06007096 RID: 28822 RVA: 0x002A9318 File Offset: 0x002A7718
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			int num = (this.resolution != BloomOptimized.Resolution.Low) ? 2 : 4;
			float num2 = (this.resolution != BloomOptimized.Resolution.Low) ? 1f : 0.5f;
			this.fastBloomMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num2, 0f, this.threshold, this.intensity));
			source.filterMode = FilterMode.Bilinear;
			int width = source.width / num;
			int height = source.height / num;
			RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 0, source.format);
			renderTexture.filterMode = FilterMode.Bilinear;
			Graphics.Blit(source, renderTexture, this.fastBloomMaterial, 1);
			int num3 = (this.blurType != BloomOptimized.BlurType.Standard) ? 2 : 0;
			for (int i = 0; i < this.blurIterations; i++)
			{
				this.fastBloomMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num2 + (float)i * 1f, 0f, this.threshold, this.intensity));
				RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, source.format);
				temporary.filterMode = FilterMode.Bilinear;
				Graphics.Blit(renderTexture, temporary, this.fastBloomMaterial, 2 + num3);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary;
				temporary = RenderTexture.GetTemporary(width, height, 0, source.format);
				temporary.filterMode = FilterMode.Bilinear;
				Graphics.Blit(renderTexture, temporary, this.fastBloomMaterial, 3 + num3);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary;
			}
			this.fastBloomMaterial.SetTexture("_Bloom", renderTexture);
			Graphics.Blit(source, destination, this.fastBloomMaterial, 0);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x04006359 RID: 25433
		[Range(0f, 1.5f)]
		public float threshold = 0.25f;

		// Token: 0x0400635A RID: 25434
		[Range(0f, 2.5f)]
		public float intensity = 0.75f;

		// Token: 0x0400635B RID: 25435
		[Range(0.25f, 5.5f)]
		public float blurSize = 1f;

		// Token: 0x0400635C RID: 25436
		private BloomOptimized.Resolution resolution;

		// Token: 0x0400635D RID: 25437
		[Range(1f, 4f)]
		public int blurIterations = 1;

		// Token: 0x0400635E RID: 25438
		public BloomOptimized.BlurType blurType;

		// Token: 0x0400635F RID: 25439
		public Shader fastBloomShader;

		// Token: 0x04006360 RID: 25440
		private Material fastBloomMaterial;

		// Token: 0x02000E64 RID: 3684
		public enum Resolution
		{
			// Token: 0x04006362 RID: 25442
			Low,
			// Token: 0x04006363 RID: 25443
			High
		}

		// Token: 0x02000E65 RID: 3685
		public enum BlurType
		{
			// Token: 0x04006365 RID: 25445
			Standard,
			// Token: 0x04006366 RID: 25446
			Sgx
		}
	}
}
