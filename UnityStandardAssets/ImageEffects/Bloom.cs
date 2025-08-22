using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000E58 RID: 3672
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Bloom and Glow/Bloom")]
	public class Bloom : PostEffectsBase
	{
		// Token: 0x06007084 RID: 28804 RVA: 0x002A7C9C File Offset: 0x002A609C
		public Bloom()
		{
		}

		// Token: 0x06007085 RID: 28805 RVA: 0x002A7D9C File Offset: 0x002A619C
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.screenBlend = base.CheckShaderAndCreateMaterial(this.screenBlendShader, this.screenBlend);
			this.lensFlareMaterial = base.CheckShaderAndCreateMaterial(this.lensFlareShader, this.lensFlareMaterial);
			this.blurAndFlaresMaterial = base.CheckShaderAndCreateMaterial(this.blurAndFlaresShader, this.blurAndFlaresMaterial);
			this.brightPassFilterMaterial = base.CheckShaderAndCreateMaterial(this.brightPassFilterShader, this.brightPassFilterMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06007086 RID: 28806 RVA: 0x002A7E28 File Offset: 0x002A6228
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			this.doHdr = false;
			if (this.hdr == Bloom.HDRBloomMode.Auto)
			{
				this.doHdr = (source.format == RenderTextureFormat.ARGBHalf && base.GetComponent<Camera>().allowHDR);
			}
			else
			{
				this.doHdr = (this.hdr == Bloom.HDRBloomMode.On);
			}
			this.doHdr = (this.doHdr && this.supportHDRTextures);
			Bloom.BloomScreenBlendMode bloomScreenBlendMode = this.screenBlendMode;
			if (this.doHdr)
			{
				bloomScreenBlendMode = Bloom.BloomScreenBlendMode.Add;
			}
			RenderTextureFormat format = (!this.doHdr) ? RenderTextureFormat.Default : RenderTextureFormat.ARGBHalf;
			int width = source.width / 2;
			int height = source.height / 2;
			int width2 = source.width / 4;
			int height2 = source.height / 4;
			float num = 1f * (float)source.width / (1f * (float)source.height);
			float num2 = 0.001953125f;
			RenderTexture temporary = RenderTexture.GetTemporary(width2, height2, 0, format);
			RenderTexture temporary2 = RenderTexture.GetTemporary(width, height, 0, format);
			if (this.quality > Bloom.BloomQuality.Cheap)
			{
				Graphics.Blit(source, temporary2, this.screenBlend, 2);
				RenderTexture temporary3 = RenderTexture.GetTemporary(width2, height2, 0, format);
				Graphics.Blit(temporary2, temporary3, this.screenBlend, 2);
				Graphics.Blit(temporary3, temporary, this.screenBlend, 6);
				RenderTexture.ReleaseTemporary(temporary3);
			}
			else
			{
				Graphics.Blit(source, temporary2);
				Graphics.Blit(temporary2, temporary, this.screenBlend, 6);
			}
			RenderTexture.ReleaseTemporary(temporary2);
			RenderTexture renderTexture = RenderTexture.GetTemporary(width2, height2, 0, format);
			this.BrightFilter(this.bloomThreshold * this.bloomThresholdColor, temporary, renderTexture);
			if (this.bloomBlurIterations < 1)
			{
				this.bloomBlurIterations = 1;
			}
			else if (this.bloomBlurIterations > 10)
			{
				this.bloomBlurIterations = 10;
			}
			for (int i = 0; i < this.bloomBlurIterations; i++)
			{
				float num3 = (1f + (float)i * 0.25f) * this.sepBlurSpread;
				RenderTexture temporary4 = RenderTexture.GetTemporary(width2, height2, 0, format);
				this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(0f, num3 * num2, 0f, 0f));
				Graphics.Blit(renderTexture, temporary4, this.blurAndFlaresMaterial, 4);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary4;
				temporary4 = RenderTexture.GetTemporary(width2, height2, 0, format);
				this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(num3 / num * num2, 0f, 0f, 0f));
				Graphics.Blit(renderTexture, temporary4, this.blurAndFlaresMaterial, 4);
				RenderTexture.ReleaseTemporary(renderTexture);
				renderTexture = temporary4;
				if (this.quality > Bloom.BloomQuality.Cheap)
				{
					if (i == 0)
					{
						Graphics.SetRenderTarget(temporary);
						GL.Clear(false, true, Color.black);
						Graphics.Blit(renderTexture, temporary);
					}
					else
					{
						temporary.MarkRestoreExpected();
						Graphics.Blit(renderTexture, temporary, this.screenBlend, 10);
					}
				}
			}
			if (this.quality > Bloom.BloomQuality.Cheap)
			{
				Graphics.SetRenderTarget(renderTexture);
				GL.Clear(false, true, Color.black);
				Graphics.Blit(temporary, renderTexture, this.screenBlend, 6);
			}
			if (this.lensflareIntensity > Mathf.Epsilon)
			{
				RenderTexture temporary5 = RenderTexture.GetTemporary(width2, height2, 0, format);
				if (this.lensflareMode == Bloom.LensFlareStyle.Ghosting)
				{
					this.BrightFilter(this.lensflareThreshold, renderTexture, temporary5);
					if (this.quality > Bloom.BloomQuality.Cheap)
					{
						this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(0f, 1.5f / (1f * (float)temporary.height), 0f, 0f));
						Graphics.SetRenderTarget(temporary);
						GL.Clear(false, true, Color.black);
						Graphics.Blit(temporary5, temporary, this.blurAndFlaresMaterial, 4);
						this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(1.5f / (1f * (float)temporary.width), 0f, 0f, 0f));
						Graphics.SetRenderTarget(temporary5);
						GL.Clear(false, true, Color.black);
						Graphics.Blit(temporary, temporary5, this.blurAndFlaresMaterial, 4);
					}
					this.Vignette(0.975f, temporary5, temporary5);
					this.BlendFlares(temporary5, renderTexture);
				}
				else
				{
					float num4 = 1f * Mathf.Cos(this.flareRotation);
					float num5 = 1f * Mathf.Sin(this.flareRotation);
					float num6 = this.hollyStretchWidth * 1f / num * num2;
					this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(num4, num5, 0f, 0f));
					this.blurAndFlaresMaterial.SetVector("_Threshhold", new Vector4(this.lensflareThreshold, 1f, 0f, 0f));
					this.blurAndFlaresMaterial.SetVector("_TintColor", new Vector4(this.flareColorA.r, this.flareColorA.g, this.flareColorA.b, this.flareColorA.a) * this.flareColorA.a * this.lensflareIntensity);
					this.blurAndFlaresMaterial.SetFloat("_Saturation", this.lensFlareSaturation);
					temporary.DiscardContents();
					Graphics.Blit(temporary5, temporary, this.blurAndFlaresMaterial, 2);
					temporary5.DiscardContents();
					Graphics.Blit(temporary, temporary5, this.blurAndFlaresMaterial, 3);
					this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(num4 * num6, num5 * num6, 0f, 0f));
					this.blurAndFlaresMaterial.SetFloat("_StretchWidth", this.hollyStretchWidth);
					temporary.DiscardContents();
					Graphics.Blit(temporary5, temporary, this.blurAndFlaresMaterial, 1);
					this.blurAndFlaresMaterial.SetFloat("_StretchWidth", this.hollyStretchWidth * 2f);
					temporary5.DiscardContents();
					Graphics.Blit(temporary, temporary5, this.blurAndFlaresMaterial, 1);
					this.blurAndFlaresMaterial.SetFloat("_StretchWidth", this.hollyStretchWidth * 4f);
					temporary.DiscardContents();
					Graphics.Blit(temporary5, temporary, this.blurAndFlaresMaterial, 1);
					for (int j = 0; j < this.hollywoodFlareBlurIterations; j++)
					{
						num6 = this.hollyStretchWidth * 2f / num * num2;
						this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(num6 * num4, num6 * num5, 0f, 0f));
						temporary5.DiscardContents();
						Graphics.Blit(temporary, temporary5, this.blurAndFlaresMaterial, 4);
						this.blurAndFlaresMaterial.SetVector("_Offsets", new Vector4(num6 * num4, num6 * num5, 0f, 0f));
						temporary.DiscardContents();
						Graphics.Blit(temporary5, temporary, this.blurAndFlaresMaterial, 4);
					}
					if (this.lensflareMode == Bloom.LensFlareStyle.Anamorphic)
					{
						this.AddTo(1f, temporary, renderTexture);
					}
					else
					{
						this.Vignette(1f, temporary, temporary5);
						this.BlendFlares(temporary5, temporary);
						this.AddTo(1f, temporary, renderTexture);
					}
				}
				RenderTexture.ReleaseTemporary(temporary5);
			}
			int pass = (int)bloomScreenBlendMode;
			this.screenBlend.SetFloat("_Intensity", this.bloomIntensity);
			this.screenBlend.SetTexture("_ColorBuffer", source);
			if (this.quality > Bloom.BloomQuality.Cheap)
			{
				RenderTexture temporary6 = RenderTexture.GetTemporary(width, height, 0, format);
				Graphics.Blit(renderTexture, temporary6);
				Graphics.Blit(temporary6, destination, this.screenBlend, pass);
				RenderTexture.ReleaseTemporary(temporary6);
			}
			else
			{
				Graphics.Blit(renderTexture, destination, this.screenBlend, pass);
			}
			RenderTexture.ReleaseTemporary(temporary);
			RenderTexture.ReleaseTemporary(renderTexture);
		}

		// Token: 0x06007087 RID: 28807 RVA: 0x002A85C9 File Offset: 0x002A69C9
		private void AddTo(float intensity_, RenderTexture from, RenderTexture to)
		{
			this.screenBlend.SetFloat("_Intensity", intensity_);
			to.MarkRestoreExpected();
			Graphics.Blit(from, to, this.screenBlend, 9);
		}

		// Token: 0x06007088 RID: 28808 RVA: 0x002A85F4 File Offset: 0x002A69F4
		private void BlendFlares(RenderTexture from, RenderTexture to)
		{
			this.lensFlareMaterial.SetVector("colorA", new Vector4(this.flareColorA.r, this.flareColorA.g, this.flareColorA.b, this.flareColorA.a) * this.lensflareIntensity);
			this.lensFlareMaterial.SetVector("colorB", new Vector4(this.flareColorB.r, this.flareColorB.g, this.flareColorB.b, this.flareColorB.a) * this.lensflareIntensity);
			this.lensFlareMaterial.SetVector("colorC", new Vector4(this.flareColorC.r, this.flareColorC.g, this.flareColorC.b, this.flareColorC.a) * this.lensflareIntensity);
			this.lensFlareMaterial.SetVector("colorD", new Vector4(this.flareColorD.r, this.flareColorD.g, this.flareColorD.b, this.flareColorD.a) * this.lensflareIntensity);
			to.MarkRestoreExpected();
			Graphics.Blit(from, to, this.lensFlareMaterial);
		}

		// Token: 0x06007089 RID: 28809 RVA: 0x002A8744 File Offset: 0x002A6B44
		private void BrightFilter(float thresh, RenderTexture from, RenderTexture to)
		{
			this.brightPassFilterMaterial.SetVector("_Threshhold", new Vector4(thresh, thresh, thresh, thresh));
			Graphics.Blit(from, to, this.brightPassFilterMaterial, 0);
		}

		// Token: 0x0600708A RID: 28810 RVA: 0x002A876D File Offset: 0x002A6B6D
		private void BrightFilter(Color threshColor, RenderTexture from, RenderTexture to)
		{
			this.brightPassFilterMaterial.SetVector("_Threshhold", threshColor);
			Graphics.Blit(from, to, this.brightPassFilterMaterial, 1);
		}

		// Token: 0x0600708B RID: 28811 RVA: 0x002A8794 File Offset: 0x002A6B94
		private void Vignette(float amount, RenderTexture from, RenderTexture to)
		{
			if (this.lensFlareVignetteMask)
			{
				this.screenBlend.SetTexture("_ColorBuffer", this.lensFlareVignetteMask);
				to.MarkRestoreExpected();
				Graphics.Blit((!(from == to)) ? from : null, to, this.screenBlend, (!(from == to)) ? 3 : 7);
			}
			else if (from != to)
			{
				Graphics.SetRenderTarget(to);
				GL.Clear(false, true, Color.black);
				Graphics.Blit(from, to);
			}
		}

		// Token: 0x040062FA RID: 25338
		public Bloom.TweakMode tweakMode;

		// Token: 0x040062FB RID: 25339
		public Bloom.BloomScreenBlendMode screenBlendMode = Bloom.BloomScreenBlendMode.Add;

		// Token: 0x040062FC RID: 25340
		public Bloom.HDRBloomMode hdr;

		// Token: 0x040062FD RID: 25341
		private bool doHdr;

		// Token: 0x040062FE RID: 25342
		public float sepBlurSpread = 2.5f;

		// Token: 0x040062FF RID: 25343
		public Bloom.BloomQuality quality = Bloom.BloomQuality.High;

		// Token: 0x04006300 RID: 25344
		public float bloomIntensity = 0.5f;

		// Token: 0x04006301 RID: 25345
		public float bloomThreshold = 0.5f;

		// Token: 0x04006302 RID: 25346
		public Color bloomThresholdColor = Color.white;

		// Token: 0x04006303 RID: 25347
		public int bloomBlurIterations = 2;

		// Token: 0x04006304 RID: 25348
		public int hollywoodFlareBlurIterations = 2;

		// Token: 0x04006305 RID: 25349
		public float flareRotation;

		// Token: 0x04006306 RID: 25350
		public Bloom.LensFlareStyle lensflareMode = Bloom.LensFlareStyle.Anamorphic;

		// Token: 0x04006307 RID: 25351
		public float hollyStretchWidth = 2.5f;

		// Token: 0x04006308 RID: 25352
		public float lensflareIntensity;

		// Token: 0x04006309 RID: 25353
		public float lensflareThreshold = 0.3f;

		// Token: 0x0400630A RID: 25354
		public float lensFlareSaturation = 0.75f;

		// Token: 0x0400630B RID: 25355
		public Color flareColorA = new Color(0.4f, 0.4f, 0.8f, 0.75f);

		// Token: 0x0400630C RID: 25356
		public Color flareColorB = new Color(0.4f, 0.8f, 0.8f, 0.75f);

		// Token: 0x0400630D RID: 25357
		public Color flareColorC = new Color(0.8f, 0.4f, 0.8f, 0.75f);

		// Token: 0x0400630E RID: 25358
		public Color flareColorD = new Color(0.8f, 0.4f, 0f, 0.75f);

		// Token: 0x0400630F RID: 25359
		public Texture2D lensFlareVignetteMask;

		// Token: 0x04006310 RID: 25360
		public Shader lensFlareShader;

		// Token: 0x04006311 RID: 25361
		private Material lensFlareMaterial;

		// Token: 0x04006312 RID: 25362
		public Shader screenBlendShader;

		// Token: 0x04006313 RID: 25363
		private Material screenBlend;

		// Token: 0x04006314 RID: 25364
		public Shader blurAndFlaresShader;

		// Token: 0x04006315 RID: 25365
		private Material blurAndFlaresMaterial;

		// Token: 0x04006316 RID: 25366
		public Shader brightPassFilterShader;

		// Token: 0x04006317 RID: 25367
		private Material brightPassFilterMaterial;

		// Token: 0x02000E59 RID: 3673
		public enum LensFlareStyle
		{
			// Token: 0x04006319 RID: 25369
			Ghosting,
			// Token: 0x0400631A RID: 25370
			Anamorphic,
			// Token: 0x0400631B RID: 25371
			Combined
		}

		// Token: 0x02000E5A RID: 3674
		public enum TweakMode
		{
			// Token: 0x0400631D RID: 25373
			Basic,
			// Token: 0x0400631E RID: 25374
			Complex
		}

		// Token: 0x02000E5B RID: 3675
		public enum HDRBloomMode
		{
			// Token: 0x04006320 RID: 25376
			Auto,
			// Token: 0x04006321 RID: 25377
			On,
			// Token: 0x04006322 RID: 25378
			Off
		}

		// Token: 0x02000E5C RID: 3676
		public enum BloomScreenBlendMode
		{
			// Token: 0x04006324 RID: 25380
			Screen,
			// Token: 0x04006325 RID: 25381
			Add
		}

		// Token: 0x02000E5D RID: 3677
		public enum BloomQuality
		{
			// Token: 0x04006327 RID: 25383
			Cheap,
			// Token: 0x04006328 RID: 25384
			High
		}
	}
}
