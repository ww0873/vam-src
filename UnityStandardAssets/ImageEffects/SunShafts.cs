using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000E8D RID: 3725
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Rendering/Sun Shafts")]
	public class SunShafts : PostEffectsBase
	{
		// Token: 0x0600713B RID: 28987 RVA: 0x002B0118 File Offset: 0x002AE518
		public SunShafts()
		{
		}

		// Token: 0x0600713C RID: 28988 RVA: 0x002B0188 File Offset: 0x002AE588
		public override bool CheckResources()
		{
			base.CheckSupport(this.useDepthTexture);
			this.sunShaftsMaterial = base.CheckShaderAndCreateMaterial(this.sunShaftsShader, this.sunShaftsMaterial);
			this.simpleClearMaterial = base.CheckShaderAndCreateMaterial(this.simpleClearShader, this.simpleClearMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x0600713D RID: 28989 RVA: 0x002B01EC File Offset: 0x002AE5EC
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (this.useDepthTexture)
			{
				base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
			}
			int num = 4;
			if (this.resolution == SunShafts.SunShaftsResolution.Normal)
			{
				num = 2;
			}
			else if (this.resolution == SunShafts.SunShaftsResolution.High)
			{
				num = 1;
			}
			Vector3 vector = Vector3.one * 0.5f;
			if (this.sunTransform)
			{
				vector = base.GetComponent<Camera>().WorldToViewportPoint(this.sunTransform.position);
			}
			else
			{
				vector = new Vector3(0.5f, 0.5f, 0f);
			}
			int width = source.width / num;
			int height = source.height / num;
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0);
			this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(1f, 1f, 0f, 0f) * this.sunShaftBlurRadius);
			this.sunShaftsMaterial.SetVector("_SunPosition", new Vector4(vector.x, vector.y, vector.z, this.maxRadius));
			this.sunShaftsMaterial.SetVector("_SunThreshold", this.sunThreshold);
			if (!this.useDepthTexture)
			{
				RenderTextureFormat format = (!base.GetComponent<Camera>().allowHDR) ? RenderTextureFormat.Default : RenderTextureFormat.DefaultHDR;
				RenderTexture temporary2 = RenderTexture.GetTemporary(source.width, source.height, 0, format);
				RenderTexture.active = temporary2;
				GL.ClearWithSkybox(false, base.GetComponent<Camera>());
				this.sunShaftsMaterial.SetTexture("_Skybox", temporary2);
				Graphics.Blit(source, temporary, this.sunShaftsMaterial, 3);
				RenderTexture.ReleaseTemporary(temporary2);
			}
			else
			{
				Graphics.Blit(source, temporary, this.sunShaftsMaterial, 2);
			}
			base.DrawBorder(temporary, this.simpleClearMaterial);
			this.radialBlurIterations = Mathf.Clamp(this.radialBlurIterations, 1, 4);
			float num2 = this.sunShaftBlurRadius * 0.0013020834f;
			this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(num2, num2, 0f, 0f));
			this.sunShaftsMaterial.SetVector("_SunPosition", new Vector4(vector.x, vector.y, vector.z, this.maxRadius));
			for (int i = 0; i < this.radialBlurIterations; i++)
			{
				RenderTexture temporary3 = RenderTexture.GetTemporary(width, height, 0);
				Graphics.Blit(temporary, temporary3, this.sunShaftsMaterial, 1);
				RenderTexture.ReleaseTemporary(temporary);
				num2 = this.sunShaftBlurRadius * (((float)i * 2f + 1f) * 6f) / 768f;
				this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(num2, num2, 0f, 0f));
				temporary = RenderTexture.GetTemporary(width, height, 0);
				Graphics.Blit(temporary3, temporary, this.sunShaftsMaterial, 1);
				RenderTexture.ReleaseTemporary(temporary3);
				num2 = this.sunShaftBlurRadius * (((float)i * 2f + 2f) * 6f) / 768f;
				this.sunShaftsMaterial.SetVector("_BlurRadius4", new Vector4(num2, num2, 0f, 0f));
			}
			if (vector.z >= 0f)
			{
				this.sunShaftsMaterial.SetVector("_SunColor", new Vector4(this.sunColor.r, this.sunColor.g, this.sunColor.b, this.sunColor.a) * this.sunShaftIntensity);
			}
			else
			{
				this.sunShaftsMaterial.SetVector("_SunColor", Vector4.zero);
			}
			this.sunShaftsMaterial.SetTexture("_ColorBuffer", temporary);
			Graphics.Blit(source, destination, this.sunShaftsMaterial, (this.screenBlendMode != SunShafts.ShaftsScreenBlendMode.Screen) ? 4 : 0);
			RenderTexture.ReleaseTemporary(temporary);
		}

		// Token: 0x040064A2 RID: 25762
		public SunShafts.SunShaftsResolution resolution = SunShafts.SunShaftsResolution.Normal;

		// Token: 0x040064A3 RID: 25763
		public SunShafts.ShaftsScreenBlendMode screenBlendMode;

		// Token: 0x040064A4 RID: 25764
		public Transform sunTransform;

		// Token: 0x040064A5 RID: 25765
		public int radialBlurIterations = 2;

		// Token: 0x040064A6 RID: 25766
		public Color sunColor = Color.white;

		// Token: 0x040064A7 RID: 25767
		public Color sunThreshold = new Color(0.87f, 0.74f, 0.65f);

		// Token: 0x040064A8 RID: 25768
		public float sunShaftBlurRadius = 2.5f;

		// Token: 0x040064A9 RID: 25769
		public float sunShaftIntensity = 1.15f;

		// Token: 0x040064AA RID: 25770
		public float maxRadius = 0.75f;

		// Token: 0x040064AB RID: 25771
		public bool useDepthTexture = true;

		// Token: 0x040064AC RID: 25772
		public Shader sunShaftsShader;

		// Token: 0x040064AD RID: 25773
		private Material sunShaftsMaterial;

		// Token: 0x040064AE RID: 25774
		public Shader simpleClearShader;

		// Token: 0x040064AF RID: 25775
		private Material simpleClearMaterial;

		// Token: 0x02000E8E RID: 3726
		public enum SunShaftsResolution
		{
			// Token: 0x040064B1 RID: 25777
			Low,
			// Token: 0x040064B2 RID: 25778
			Normal,
			// Token: 0x040064B3 RID: 25779
			High
		}

		// Token: 0x02000E8F RID: 3727
		public enum ShaftsScreenBlendMode
		{
			// Token: 0x040064B5 RID: 25781
			Screen,
			// Token: 0x040064B6 RID: 25782
			Add
		}
	}
}
