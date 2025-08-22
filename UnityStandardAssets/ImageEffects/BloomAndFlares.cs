using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000E62 RID: 3682
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Bloom and Glow/BloomAndFlares (3.5, Deprecated)")]
	public class BloomAndFlares : PostEffectsBase
	{
		// Token: 0x0600708C RID: 28812 RVA: 0x002A882C File Offset: 0x002A6C2C
		public BloomAndFlares()
		{
		}

		// Token: 0x0600708D RID: 28813 RVA: 0x002A8924 File Offset: 0x002A6D24
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.screenBlend = base.CheckShaderAndCreateMaterial(this.screenBlendShader, this.screenBlend);
			this.lensFlareMaterial = base.CheckShaderAndCreateMaterial(this.lensFlareShader, this.lensFlareMaterial);
			this.vignetteMaterial = base.CheckShaderAndCreateMaterial(this.vignetteShader, this.vignetteMaterial);
			this.separableBlurMaterial = base.CheckShaderAndCreateMaterial(this.separableBlurShader, this.separableBlurMaterial);
			this.addBrightStuffBlendOneOneMaterial = base.CheckShaderAndCreateMaterial(this.addBrightStuffOneOneShader, this.addBrightStuffBlendOneOneMaterial);
			this.hollywoodFlaresMaterial = base.CheckShaderAndCreateMaterial(this.hollywoodFlaresShader, this.hollywoodFlaresMaterial);
			this.brightPassFilterMaterial = base.CheckShaderAndCreateMaterial(this.brightPassFilterShader, this.brightPassFilterMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x0600708E RID: 28814 RVA: 0x002A89F8 File Offset: 0x002A6DF8
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			this.doHdr = false;
			if (this.hdr == HDRBloomMode.Auto)
			{
				this.doHdr = (source.format == RenderTextureFormat.ARGBHalf && base.GetComponent<Camera>().allowHDR);
			}
			else
			{
				this.doHdr = (this.hdr == HDRBloomMode.On);
			}
			this.doHdr = (this.doHdr && this.supportHDRTextures);
			BloomScreenBlendMode pass = this.screenBlendMode;
			if (this.doHdr)
			{
				pass = BloomScreenBlendMode.Add;
			}
			RenderTextureFormat format = (!this.doHdr) ? RenderTextureFormat.Default : RenderTextureFormat.ARGBHalf;
			RenderTexture temporary = RenderTexture.GetTemporary(source.width / 2, source.height / 2, 0, format);
			RenderTexture temporary2 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, format);
			RenderTexture temporary3 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, format);
			RenderTexture temporary4 = RenderTexture.GetTemporary(source.width / 4, source.height / 4, 0, format);
			float num = 1f * (float)source.width / (1f * (float)source.height);
			float num2 = 0.001953125f;
			Graphics.Blit(source, temporary, this.screenBlend, 2);
			Graphics.Blit(temporary, temporary2, this.screenBlend, 2);
			RenderTexture.ReleaseTemporary(temporary);
			this.BrightFilter(this.bloomThreshold, this.useSrcAlphaAsMask, temporary2, temporary3);
			temporary2.DiscardContents();
			if (this.bloomBlurIterations < 1)
			{
				this.bloomBlurIterations = 1;
			}
			for (int i = 0; i < this.bloomBlurIterations; i++)
			{
				float num3 = (1f + (float)i * 0.5f) * this.sepBlurSpread;
				this.separableBlurMaterial.SetVector("offsets", new Vector4(0f, num3 * num2, 0f, 0f));
				RenderTexture renderTexture = (i != 0) ? temporary2 : temporary3;
				Graphics.Blit(renderTexture, temporary4, this.separableBlurMaterial);
				renderTexture.DiscardContents();
				this.separableBlurMaterial.SetVector("offsets", new Vector4(num3 / num * num2, 0f, 0f, 0f));
				Graphics.Blit(temporary4, temporary2, this.separableBlurMaterial);
				temporary4.DiscardContents();
			}
			if (this.lensflares)
			{
				if (this.lensflareMode == LensflareStyle34.Ghosting)
				{
					this.BrightFilter(this.lensflareThreshold, 0f, temporary2, temporary4);
					temporary2.DiscardContents();
					this.Vignette(0.975f, temporary4, temporary3);
					temporary4.DiscardContents();
					this.BlendFlares(temporary3, temporary2);
					temporary3.DiscardContents();
				}
				else
				{
					this.hollywoodFlaresMaterial.SetVector("_threshold", new Vector4(this.lensflareThreshold, 1f / (1f - this.lensflareThreshold), 0f, 0f));
					this.hollywoodFlaresMaterial.SetVector("tintColor", new Vector4(this.flareColorA.r, this.flareColorA.g, this.flareColorA.b, this.flareColorA.a) * this.flareColorA.a * this.lensflareIntensity);
					Graphics.Blit(temporary4, temporary3, this.hollywoodFlaresMaterial, 2);
					temporary4.DiscardContents();
					Graphics.Blit(temporary3, temporary4, this.hollywoodFlaresMaterial, 3);
					temporary3.DiscardContents();
					this.hollywoodFlaresMaterial.SetVector("offsets", new Vector4(this.sepBlurSpread * 1f / num * num2, 0f, 0f, 0f));
					this.hollywoodFlaresMaterial.SetFloat("stretchWidth", this.hollyStretchWidth);
					Graphics.Blit(temporary4, temporary3, this.hollywoodFlaresMaterial, 1);
					temporary4.DiscardContents();
					this.hollywoodFlaresMaterial.SetFloat("stretchWidth", this.hollyStretchWidth * 2f);
					Graphics.Blit(temporary3, temporary4, this.hollywoodFlaresMaterial, 1);
					temporary3.DiscardContents();
					this.hollywoodFlaresMaterial.SetFloat("stretchWidth", this.hollyStretchWidth * 4f);
					Graphics.Blit(temporary4, temporary3, this.hollywoodFlaresMaterial, 1);
					temporary4.DiscardContents();
					if (this.lensflareMode == LensflareStyle34.Anamorphic)
					{
						for (int j = 0; j < this.hollywoodFlareBlurIterations; j++)
						{
							this.separableBlurMaterial.SetVector("offsets", new Vector4(this.hollyStretchWidth * 2f / num * num2, 0f, 0f, 0f));
							Graphics.Blit(temporary3, temporary4, this.separableBlurMaterial);
							temporary3.DiscardContents();
							this.separableBlurMaterial.SetVector("offsets", new Vector4(this.hollyStretchWidth * 2f / num * num2, 0f, 0f, 0f));
							Graphics.Blit(temporary4, temporary3, this.separableBlurMaterial);
							temporary4.DiscardContents();
						}
						this.AddTo(1f, temporary3, temporary2);
						temporary3.DiscardContents();
					}
					else
					{
						for (int k = 0; k < this.hollywoodFlareBlurIterations; k++)
						{
							this.separableBlurMaterial.SetVector("offsets", new Vector4(this.hollyStretchWidth * 2f / num * num2, 0f, 0f, 0f));
							Graphics.Blit(temporary3, temporary4, this.separableBlurMaterial);
							temporary3.DiscardContents();
							this.separableBlurMaterial.SetVector("offsets", new Vector4(this.hollyStretchWidth * 2f / num * num2, 0f, 0f, 0f));
							Graphics.Blit(temporary4, temporary3, this.separableBlurMaterial);
							temporary4.DiscardContents();
						}
						this.Vignette(1f, temporary3, temporary4);
						temporary3.DiscardContents();
						this.BlendFlares(temporary4, temporary3);
						temporary4.DiscardContents();
						this.AddTo(1f, temporary3, temporary2);
						temporary3.DiscardContents();
					}
				}
			}
			this.screenBlend.SetFloat("_Intensity", this.bloomIntensity);
			this.screenBlend.SetTexture("_ColorBuffer", source);
			Graphics.Blit(temporary2, destination, this.screenBlend, (int)pass);
			RenderTexture.ReleaseTemporary(temporary2);
			RenderTexture.ReleaseTemporary(temporary3);
			RenderTexture.ReleaseTemporary(temporary4);
		}

		// Token: 0x0600708F RID: 28815 RVA: 0x002A902F File Offset: 0x002A742F
		private void AddTo(float intensity_, RenderTexture from, RenderTexture to)
		{
			this.addBrightStuffBlendOneOneMaterial.SetFloat("_Intensity", intensity_);
			Graphics.Blit(from, to, this.addBrightStuffBlendOneOneMaterial);
		}

		// Token: 0x06007090 RID: 28816 RVA: 0x002A9050 File Offset: 0x002A7450
		private void BlendFlares(RenderTexture from, RenderTexture to)
		{
			this.lensFlareMaterial.SetVector("colorA", new Vector4(this.flareColorA.r, this.flareColorA.g, this.flareColorA.b, this.flareColorA.a) * this.lensflareIntensity);
			this.lensFlareMaterial.SetVector("colorB", new Vector4(this.flareColorB.r, this.flareColorB.g, this.flareColorB.b, this.flareColorB.a) * this.lensflareIntensity);
			this.lensFlareMaterial.SetVector("colorC", new Vector4(this.flareColorC.r, this.flareColorC.g, this.flareColorC.b, this.flareColorC.a) * this.lensflareIntensity);
			this.lensFlareMaterial.SetVector("colorD", new Vector4(this.flareColorD.r, this.flareColorD.g, this.flareColorD.b, this.flareColorD.a) * this.lensflareIntensity);
			Graphics.Blit(from, to, this.lensFlareMaterial);
		}

		// Token: 0x06007091 RID: 28817 RVA: 0x002A919C File Offset: 0x002A759C
		private void BrightFilter(float thresh, float useAlphaAsMask, RenderTexture from, RenderTexture to)
		{
			if (this.doHdr)
			{
				this.brightPassFilterMaterial.SetVector("threshold", new Vector4(thresh, 1f, 0f, 0f));
			}
			else
			{
				this.brightPassFilterMaterial.SetVector("threshold", new Vector4(thresh, 1f / (1f - thresh), 0f, 0f));
			}
			this.brightPassFilterMaterial.SetFloat("useSrcAlphaAsMask", useAlphaAsMask);
			Graphics.Blit(from, to, this.brightPassFilterMaterial);
		}

		// Token: 0x06007092 RID: 28818 RVA: 0x002A922C File Offset: 0x002A762C
		private void Vignette(float amount, RenderTexture from, RenderTexture to)
		{
			if (this.lensFlareVignetteMask)
			{
				this.screenBlend.SetTexture("_ColorBuffer", this.lensFlareVignetteMask);
				Graphics.Blit(from, to, this.screenBlend, 3);
			}
			else
			{
				this.vignetteMaterial.SetFloat("vignetteIntensity", amount);
				Graphics.Blit(from, to, this.vignetteMaterial);
			}
		}

		// Token: 0x04006337 RID: 25399
		public TweakMode34 tweakMode;

		// Token: 0x04006338 RID: 25400
		public BloomScreenBlendMode screenBlendMode = BloomScreenBlendMode.Add;

		// Token: 0x04006339 RID: 25401
		public HDRBloomMode hdr;

		// Token: 0x0400633A RID: 25402
		private bool doHdr;

		// Token: 0x0400633B RID: 25403
		public float sepBlurSpread = 1.5f;

		// Token: 0x0400633C RID: 25404
		public float useSrcAlphaAsMask = 0.5f;

		// Token: 0x0400633D RID: 25405
		public float bloomIntensity = 1f;

		// Token: 0x0400633E RID: 25406
		public float bloomThreshold = 0.5f;

		// Token: 0x0400633F RID: 25407
		public int bloomBlurIterations = 2;

		// Token: 0x04006340 RID: 25408
		public bool lensflares;

		// Token: 0x04006341 RID: 25409
		public int hollywoodFlareBlurIterations = 2;

		// Token: 0x04006342 RID: 25410
		public LensflareStyle34 lensflareMode = LensflareStyle34.Anamorphic;

		// Token: 0x04006343 RID: 25411
		public float hollyStretchWidth = 3.5f;

		// Token: 0x04006344 RID: 25412
		public float lensflareIntensity = 1f;

		// Token: 0x04006345 RID: 25413
		public float lensflareThreshold = 0.3f;

		// Token: 0x04006346 RID: 25414
		public Color flareColorA = new Color(0.4f, 0.4f, 0.8f, 0.75f);

		// Token: 0x04006347 RID: 25415
		public Color flareColorB = new Color(0.4f, 0.8f, 0.8f, 0.75f);

		// Token: 0x04006348 RID: 25416
		public Color flareColorC = new Color(0.8f, 0.4f, 0.8f, 0.75f);

		// Token: 0x04006349 RID: 25417
		public Color flareColorD = new Color(0.8f, 0.4f, 0f, 0.75f);

		// Token: 0x0400634A RID: 25418
		public Texture2D lensFlareVignetteMask;

		// Token: 0x0400634B RID: 25419
		public Shader lensFlareShader;

		// Token: 0x0400634C RID: 25420
		private Material lensFlareMaterial;

		// Token: 0x0400634D RID: 25421
		public Shader vignetteShader;

		// Token: 0x0400634E RID: 25422
		private Material vignetteMaterial;

		// Token: 0x0400634F RID: 25423
		public Shader separableBlurShader;

		// Token: 0x04006350 RID: 25424
		private Material separableBlurMaterial;

		// Token: 0x04006351 RID: 25425
		public Shader addBrightStuffOneOneShader;

		// Token: 0x04006352 RID: 25426
		private Material addBrightStuffBlendOneOneMaterial;

		// Token: 0x04006353 RID: 25427
		public Shader screenBlendShader;

		// Token: 0x04006354 RID: 25428
		private Material screenBlend;

		// Token: 0x04006355 RID: 25429
		public Shader hollywoodFlaresShader;

		// Token: 0x04006356 RID: 25430
		private Material hollywoodFlaresMaterial;

		// Token: 0x04006357 RID: 25431
		public Shader brightPassFilterShader;

		// Token: 0x04006358 RID: 25432
		private Material brightPassFilterMaterial;
	}
}
