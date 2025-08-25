using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000E98 RID: 3736
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Camera/Vignette and Chromatic Aberration")]
	public class VignetteAndChromaticAberration : PostEffectsBase
	{
		// Token: 0x0600714F RID: 29007 RVA: 0x002B1174 File Offset: 0x002AF574
		public VignetteAndChromaticAberration()
		{
		}

		// Token: 0x06007150 RID: 29008 RVA: 0x002B11CC File Offset: 0x002AF5CC
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.m_VignetteMaterial = base.CheckShaderAndCreateMaterial(this.vignetteShader, this.m_VignetteMaterial);
			this.m_SeparableBlurMaterial = base.CheckShaderAndCreateMaterial(this.separableBlurShader, this.m_SeparableBlurMaterial);
			this.m_ChromAberrationMaterial = base.CheckShaderAndCreateMaterial(this.chromAberrationShader, this.m_ChromAberrationMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06007151 RID: 29009 RVA: 0x002B1240 File Offset: 0x002AF640
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			int width = source.width;
			int height = source.height;
			bool flag = Mathf.Abs(this.blur) > 0f || Mathf.Abs(this.intensity) > 0f;
			float num = 1f * (float)width / (1f * (float)height);
			RenderTexture renderTexture = null;
			RenderTexture renderTexture2 = null;
			if (flag)
			{
				renderTexture = RenderTexture.GetTemporary(width, height, 0, source.format);
				if (Mathf.Abs(this.blur) > 0f)
				{
					renderTexture2 = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format);
					Graphics.Blit(source, renderTexture2, this.m_ChromAberrationMaterial, 0);
					for (int i = 0; i < 2; i++)
					{
						this.m_SeparableBlurMaterial.SetVector("offsets", new Vector4(0f, this.blurSpread * 0.001953125f, 0f, 0f));
						RenderTexture temporary = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format);
						Graphics.Blit(renderTexture2, temporary, this.m_SeparableBlurMaterial);
						RenderTexture.ReleaseTemporary(renderTexture2);
						this.m_SeparableBlurMaterial.SetVector("offsets", new Vector4(this.blurSpread * 0.001953125f / num, 0f, 0f, 0f));
						renderTexture2 = RenderTexture.GetTemporary(width / 2, height / 2, 0, source.format);
						Graphics.Blit(temporary, renderTexture2, this.m_SeparableBlurMaterial);
						RenderTexture.ReleaseTemporary(temporary);
					}
				}
				this.m_VignetteMaterial.SetFloat("_Intensity", 1f / (1f - this.intensity) - 1f);
				this.m_VignetteMaterial.SetFloat("_Blur", 1f / (1f - this.blur) - 1f);
				this.m_VignetteMaterial.SetTexture("_VignetteTex", renderTexture2);
				Graphics.Blit(source, renderTexture, this.m_VignetteMaterial, 0);
			}
			this.m_ChromAberrationMaterial.SetFloat("_ChromaticAberration", this.chromaticAberration);
			this.m_ChromAberrationMaterial.SetFloat("_AxialAberration", this.axialAberration);
			this.m_ChromAberrationMaterial.SetVector("_BlurDistance", new Vector2(-this.blurDistance, this.blurDistance));
			this.m_ChromAberrationMaterial.SetFloat("_Luminance", 1f / Mathf.Max(Mathf.Epsilon, this.luminanceDependency));
			if (flag)
			{
				renderTexture.wrapMode = TextureWrapMode.Clamp;
			}
			else
			{
				source.wrapMode = TextureWrapMode.Clamp;
			}
			Graphics.Blit((!flag) ? source : renderTexture, destination, this.m_ChromAberrationMaterial, (this.mode != VignetteAndChromaticAberration.AberrationMode.Advanced) ? 1 : 2);
			RenderTexture.ReleaseTemporary(renderTexture);
			RenderTexture.ReleaseTemporary(renderTexture2);
		}

		// Token: 0x040064E8 RID: 25832
		public VignetteAndChromaticAberration.AberrationMode mode;

		// Token: 0x040064E9 RID: 25833
		public float intensity = 0.036f;

		// Token: 0x040064EA RID: 25834
		public float chromaticAberration = 0.2f;

		// Token: 0x040064EB RID: 25835
		public float axialAberration = 0.5f;

		// Token: 0x040064EC RID: 25836
		public float blur;

		// Token: 0x040064ED RID: 25837
		public float blurSpread = 0.75f;

		// Token: 0x040064EE RID: 25838
		public float luminanceDependency = 0.25f;

		// Token: 0x040064EF RID: 25839
		public float blurDistance = 2.5f;

		// Token: 0x040064F0 RID: 25840
		public Shader vignetteShader;

		// Token: 0x040064F1 RID: 25841
		public Shader separableBlurShader;

		// Token: 0x040064F2 RID: 25842
		public Shader chromAberrationShader;

		// Token: 0x040064F3 RID: 25843
		private Material m_VignetteMaterial;

		// Token: 0x040064F4 RID: 25844
		private Material m_SeparableBlurMaterial;

		// Token: 0x040064F5 RID: 25845
		private Material m_ChromAberrationMaterial;

		// Token: 0x02000E99 RID: 3737
		public enum AberrationMode
		{
			// Token: 0x040064F7 RID: 25847
			Simple,
			// Token: 0x040064F8 RID: 25848
			Advanced
		}
	}
}
