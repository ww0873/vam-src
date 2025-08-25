using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000E67 RID: 3687
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Blur/Blur (Optimized)")]
	public class BlurOptimized : PostEffectsBase
	{
		// Token: 0x0600709F RID: 28831 RVA: 0x002A96EF File Offset: 0x002A7AEF
		public BlurOptimized()
		{
		}

		// Token: 0x060070A0 RID: 28832 RVA: 0x002A9710 File Offset: 0x002A7B10
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.blurMaterial = base.CheckShaderAndCreateMaterial(this.blurShader, this.blurMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060070A1 RID: 28833 RVA: 0x002A9749 File Offset: 0x002A7B49
		public void OnDisable()
		{
			if (this.blurMaterial)
			{
				UnityEngine.Object.DestroyImmediate(this.blurMaterial);
			}
		}

		// Token: 0x060070A2 RID: 28834 RVA: 0x002A9768 File Offset: 0x002A7B68
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			float num = 1f / (1f * (float)(1 << this.downsample));
			this.blurMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num, -this.blurSize * num, 0f, 0f));
			source.filterMode = FilterMode.Bilinear;
			int width = source.width >> this.downsample;
			int height = source.height >> this.downsample;
			RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 0, source.format);
			renderTexture.filterMode = FilterMode.Bilinear;
			Graphics.Blit(source, renderTexture, this.blurMaterial, 0);
			int num2 = (this.blurType != BlurOptimized.BlurType.StandardGauss) ? 2 : 0;
			for (int i = 0; i < this.blurIterations; i++)
			{
				float num3 = (float)i * 1f;
				this.blurMaterial.SetVector("_Parameter", new Vector4(this.blurSize * num + num3, -this.blurSize * num - num3, 0f, 0f));
				RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, source.format);
				temporary.filterMode = FilterMode.Bilinear;
				Graphics.Blit(renderTexture, temporary, this.blurMaterial, 1 + num2);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary;
				temporary = RenderTexture.GetTemporary(width, height, 0, source.format);
				temporary.filterMode = FilterMode.Bilinear;
				Graphics.Blit(renderTexture, temporary, this.blurMaterial, 2 + num2);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary;
			}
			Graphics.Blit(renderTexture, destination);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x0400636B RID: 25451
		[Range(0f, 2f)]
		public int downsample = 1;

		// Token: 0x0400636C RID: 25452
		[Range(0f, 10f)]
		public float blurSize = 3f;

		// Token: 0x0400636D RID: 25453
		[Range(1f, 4f)]
		public int blurIterations = 2;

		// Token: 0x0400636E RID: 25454
		public BlurOptimized.BlurType blurType;

		// Token: 0x0400636F RID: 25455
		public Shader blurShader;

		// Token: 0x04006370 RID: 25456
		private Material blurMaterial;

		// Token: 0x02000E68 RID: 3688
		public enum BlurType
		{
			// Token: 0x04006372 RID: 25458
			StandardGauss,
			// Token: 0x04006373 RID: 25459
			SgxGauss
		}
	}
}
