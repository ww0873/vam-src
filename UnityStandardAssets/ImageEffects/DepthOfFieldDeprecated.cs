using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000E75 RID: 3701
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Camera/Depth of Field (deprecated)")]
	public class DepthOfFieldDeprecated : PostEffectsBase
	{
		// Token: 0x060070D9 RID: 28889 RVA: 0x002ACA30 File Offset: 0x002AAE30
		public DepthOfFieldDeprecated()
		{
		}

		// Token: 0x060070DA RID: 28890 RVA: 0x002ACB1C File Offset: 0x002AAF1C
		private void CreateMaterials()
		{
			this.dofBlurMaterial = base.CheckShaderAndCreateMaterial(this.dofBlurShader, this.dofBlurMaterial);
			this.dofMaterial = base.CheckShaderAndCreateMaterial(this.dofShader, this.dofMaterial);
			this.bokehSupport = this.bokehShader.isSupported;
			if (this.bokeh && this.bokehSupport && this.bokehShader)
			{
				this.bokehMaterial = base.CheckShaderAndCreateMaterial(this.bokehShader, this.bokehMaterial);
			}
		}

		// Token: 0x060070DB RID: 28891 RVA: 0x002ACBA8 File Offset: 0x002AAFA8
		public override bool CheckResources()
		{
			base.CheckSupport(true);
			this.dofBlurMaterial = base.CheckShaderAndCreateMaterial(this.dofBlurShader, this.dofBlurMaterial);
			this.dofMaterial = base.CheckShaderAndCreateMaterial(this.dofShader, this.dofMaterial);
			this.bokehSupport = this.bokehShader.isSupported;
			if (this.bokeh && this.bokehSupport && this.bokehShader)
			{
				this.bokehMaterial = base.CheckShaderAndCreateMaterial(this.bokehShader, this.bokehMaterial);
			}
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060070DC RID: 28892 RVA: 0x002ACC53 File Offset: 0x002AB053
		private void OnDisable()
		{
			Quads.Cleanup();
		}

		// Token: 0x060070DD RID: 28893 RVA: 0x002ACC5A File Offset: 0x002AB05A
		private void OnEnable()
		{
			this._camera = base.GetComponent<Camera>();
			this._camera.depthTextureMode |= DepthTextureMode.Depth;
		}

		// Token: 0x060070DE RID: 28894 RVA: 0x002ACC7C File Offset: 0x002AB07C
		private float FocalDistance01(float worldDist)
		{
			return this._camera.WorldToViewportPoint((worldDist - this._camera.nearClipPlane) * this._camera.transform.forward + this._camera.transform.position).z / (this._camera.farClipPlane - this._camera.nearClipPlane);
		}

		// Token: 0x060070DF RID: 28895 RVA: 0x002ACCEC File Offset: 0x002AB0EC
		private int GetDividerBasedOnQuality()
		{
			int result = 1;
			if (this.resolution == DepthOfFieldDeprecated.DofResolution.Medium)
			{
				result = 2;
			}
			else if (this.resolution == DepthOfFieldDeprecated.DofResolution.Low)
			{
				result = 2;
			}
			return result;
		}

		// Token: 0x060070E0 RID: 28896 RVA: 0x002ACD20 File Offset: 0x002AB120
		private int GetLowResolutionDividerBasedOnQuality(int baseDivider)
		{
			int num = baseDivider;
			if (this.resolution == DepthOfFieldDeprecated.DofResolution.High)
			{
				num *= 2;
			}
			if (this.resolution == DepthOfFieldDeprecated.DofResolution.Low)
			{
				num *= 2;
			}
			return num;
		}

		// Token: 0x060070E1 RID: 28897 RVA: 0x002ACD50 File Offset: 0x002AB150
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (this.smoothness < 0.1f)
			{
				this.smoothness = 0.1f;
			}
			this.bokeh = (this.bokeh && this.bokehSupport);
			float num = (!this.bokeh) ? 1f : DepthOfFieldDeprecated.BOKEH_EXTRA_BLUR;
			bool flag = this.quality > DepthOfFieldDeprecated.Dof34QualitySetting.OnlyBackground;
			float num2 = this.focalSize / (this._camera.farClipPlane - this._camera.nearClipPlane);
			if (this.simpleTweakMode)
			{
				this.focalDistance01 = ((!this.objectFocus) ? this.FocalDistance01(this.focalPoint) : (this._camera.WorldToViewportPoint(this.objectFocus.position).z / this._camera.farClipPlane));
				this.focalStartCurve = this.focalDistance01 * this.smoothness;
				this.focalEndCurve = this.focalStartCurve;
				flag = (flag && this.focalPoint > this._camera.nearClipPlane + Mathf.Epsilon);
			}
			else
			{
				if (this.objectFocus)
				{
					Vector3 vector = this._camera.WorldToViewportPoint(this.objectFocus.position);
					vector.z /= this._camera.farClipPlane;
					this.focalDistance01 = vector.z;
				}
				else
				{
					this.focalDistance01 = this.FocalDistance01(this.focalZDistance);
				}
				this.focalStartCurve = this.focalZStartCurve;
				this.focalEndCurve = this.focalZEndCurve;
				flag = (flag && this.focalPoint > this._camera.nearClipPlane + Mathf.Epsilon);
			}
			this.widthOverHeight = 1f * (float)source.width / (1f * (float)source.height);
			this.oneOverBaseSize = 0.001953125f;
			this.dofMaterial.SetFloat("_ForegroundBlurExtrude", this.foregroundBlurExtrude);
			this.dofMaterial.SetVector("_CurveParams", new Vector4((!this.simpleTweakMode) ? this.focalStartCurve : (1f / this.focalStartCurve), (!this.simpleTweakMode) ? this.focalEndCurve : (1f / this.focalEndCurve), num2 * 0.5f, this.focalDistance01));
			this.dofMaterial.SetVector("_InvRenderTargetSize", new Vector4(1f / (1f * (float)source.width), 1f / (1f * (float)source.height), 0f, 0f));
			int dividerBasedOnQuality = this.GetDividerBasedOnQuality();
			int lowResolutionDividerBasedOnQuality = this.GetLowResolutionDividerBasedOnQuality(dividerBasedOnQuality);
			this.AllocateTextures(flag, source, dividerBasedOnQuality, lowResolutionDividerBasedOnQuality);
			Graphics.Blit(source, source, this.dofMaterial, 3);
			this.Downsample(source, this.mediumRezWorkTexture);
			this.Blur(this.mediumRezWorkTexture, this.mediumRezWorkTexture, DepthOfFieldDeprecated.DofBlurriness.Low, 4, this.maxBlurSpread);
			if (this.bokeh && (DepthOfFieldDeprecated.BokehDestination.Foreground & this.bokehDestination) != (DepthOfFieldDeprecated.BokehDestination)0)
			{
				this.dofMaterial.SetVector("_Threshhold", new Vector4(this.bokehThresholdContrast, this.bokehThresholdLuminance, 0.95f, 0f));
				Graphics.Blit(this.mediumRezWorkTexture, this.bokehSource2, this.dofMaterial, 11);
				Graphics.Blit(this.mediumRezWorkTexture, this.lowRezWorkTexture);
				this.Blur(this.lowRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 0, this.maxBlurSpread * num);
			}
			else
			{
				this.Downsample(this.mediumRezWorkTexture, this.lowRezWorkTexture);
				this.Blur(this.lowRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 0, this.maxBlurSpread);
			}
			this.dofBlurMaterial.SetTexture("_TapLow", this.lowRezWorkTexture);
			this.dofBlurMaterial.SetTexture("_TapMedium", this.mediumRezWorkTexture);
			Graphics.Blit(null, this.finalDefocus, this.dofBlurMaterial, 3);
			if (this.bokeh && (DepthOfFieldDeprecated.BokehDestination.Foreground & this.bokehDestination) != (DepthOfFieldDeprecated.BokehDestination)0)
			{
				this.AddBokeh(this.bokehSource2, this.bokehSource, this.finalDefocus);
			}
			this.dofMaterial.SetTexture("_TapLowBackground", this.finalDefocus);
			this.dofMaterial.SetTexture("_TapMedium", this.mediumRezWorkTexture);
			Graphics.Blit(source, (!flag) ? destination : this.foregroundTexture, this.dofMaterial, (!this.visualize) ? 0 : 2);
			if (flag)
			{
				Graphics.Blit(this.foregroundTexture, source, this.dofMaterial, 5);
				this.Downsample(source, this.mediumRezWorkTexture);
				this.BlurFg(this.mediumRezWorkTexture, this.mediumRezWorkTexture, DepthOfFieldDeprecated.DofBlurriness.Low, 2, this.maxBlurSpread);
				if (this.bokeh && (DepthOfFieldDeprecated.BokehDestination.Foreground & this.bokehDestination) != (DepthOfFieldDeprecated.BokehDestination)0)
				{
					this.dofMaterial.SetVector("_Threshhold", new Vector4(this.bokehThresholdContrast * 0.5f, this.bokehThresholdLuminance, 0f, 0f));
					Graphics.Blit(this.mediumRezWorkTexture, this.bokehSource2, this.dofMaterial, 11);
					Graphics.Blit(this.mediumRezWorkTexture, this.lowRezWorkTexture);
					this.BlurFg(this.lowRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 1, this.maxBlurSpread * num);
				}
				else
				{
					this.BlurFg(this.mediumRezWorkTexture, this.lowRezWorkTexture, this.bluriness, 1, this.maxBlurSpread);
				}
				Graphics.Blit(this.lowRezWorkTexture, this.finalDefocus);
				this.dofMaterial.SetTexture("_TapLowForeground", this.finalDefocus);
				Graphics.Blit(source, destination, this.dofMaterial, (!this.visualize) ? 4 : 1);
				if (this.bokeh && (DepthOfFieldDeprecated.BokehDestination.Foreground & this.bokehDestination) != (DepthOfFieldDeprecated.BokehDestination)0)
				{
					this.AddBokeh(this.bokehSource2, this.bokehSource, destination);
				}
			}
			this.ReleaseTextures();
		}

		// Token: 0x060070E2 RID: 28898 RVA: 0x002AD36C File Offset: 0x002AB76C
		private void Blur(RenderTexture from, RenderTexture to, DepthOfFieldDeprecated.DofBlurriness iterations, int blurPass, float spread)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(to.width, to.height);
			if (iterations > DepthOfFieldDeprecated.DofBlurriness.Low)
			{
				this.BlurHex(from, to, blurPass, spread, temporary);
				if (iterations > DepthOfFieldDeprecated.DofBlurriness.High)
				{
					this.dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * this.oneOverBaseSize, 0f, 0f));
					Graphics.Blit(to, temporary, this.dofBlurMaterial, blurPass);
					this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0f, 0f, 0f));
					Graphics.Blit(temporary, to, this.dofBlurMaterial, blurPass);
				}
			}
			else
			{
				this.dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * this.oneOverBaseSize, 0f, 0f));
				Graphics.Blit(from, temporary, this.dofBlurMaterial, blurPass);
				this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0f, 0f, 0f));
				Graphics.Blit(temporary, to, this.dofBlurMaterial, blurPass);
			}
			RenderTexture.ReleaseTemporary(temporary);
		}

		// Token: 0x060070E3 RID: 28899 RVA: 0x002AD4B0 File Offset: 0x002AB8B0
		private void BlurFg(RenderTexture from, RenderTexture to, DepthOfFieldDeprecated.DofBlurriness iterations, int blurPass, float spread)
		{
			this.dofBlurMaterial.SetTexture("_TapHigh", from);
			RenderTexture temporary = RenderTexture.GetTemporary(to.width, to.height);
			if (iterations > DepthOfFieldDeprecated.DofBlurriness.Low)
			{
				this.BlurHex(from, to, blurPass, spread, temporary);
				if (iterations > DepthOfFieldDeprecated.DofBlurriness.High)
				{
					this.dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * this.oneOverBaseSize, 0f, 0f));
					Graphics.Blit(to, temporary, this.dofBlurMaterial, blurPass);
					this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0f, 0f, 0f));
					Graphics.Blit(temporary, to, this.dofBlurMaterial, blurPass);
				}
			}
			else
			{
				this.dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * this.oneOverBaseSize, 0f, 0f));
				Graphics.Blit(from, temporary, this.dofBlurMaterial, blurPass);
				this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0f, 0f, 0f));
				Graphics.Blit(temporary, to, this.dofBlurMaterial, blurPass);
			}
			RenderTexture.ReleaseTemporary(temporary);
		}

		// Token: 0x060070E4 RID: 28900 RVA: 0x002AD604 File Offset: 0x002ABA04
		private void BlurHex(RenderTexture from, RenderTexture to, int blurPass, float spread, RenderTexture tmp)
		{
			this.dofBlurMaterial.SetVector("offsets", new Vector4(0f, spread * this.oneOverBaseSize, 0f, 0f));
			Graphics.Blit(from, tmp, this.dofBlurMaterial, blurPass);
			this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, 0f, 0f, 0f));
			Graphics.Blit(tmp, to, this.dofBlurMaterial, blurPass);
			this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, spread * this.oneOverBaseSize, 0f, 0f));
			Graphics.Blit(to, tmp, this.dofBlurMaterial, blurPass);
			this.dofBlurMaterial.SetVector("offsets", new Vector4(spread / this.widthOverHeight * this.oneOverBaseSize, -spread * this.oneOverBaseSize, 0f, 0f));
			Graphics.Blit(tmp, to, this.dofBlurMaterial, blurPass);
		}

		// Token: 0x060070E5 RID: 28901 RVA: 0x002AD720 File Offset: 0x002ABB20
		private void Downsample(RenderTexture from, RenderTexture to)
		{
			this.dofMaterial.SetVector("_InvRenderTargetSize", new Vector4(1f / (1f * (float)to.width), 1f / (1f * (float)to.height), 0f, 0f));
			Graphics.Blit(from, to, this.dofMaterial, DepthOfFieldDeprecated.SMOOTH_DOWNSAMPLE_PASS);
		}

		// Token: 0x060070E6 RID: 28902 RVA: 0x002AD784 File Offset: 0x002ABB84
		private void AddBokeh(RenderTexture bokehInfo, RenderTexture tempTex, RenderTexture finalTarget)
		{
			if (this.bokehMaterial)
			{
				Mesh[] meshes = Quads.GetMeshes(tempTex.width, tempTex.height);
				RenderTexture.active = tempTex;
				GL.Clear(false, true, new Color(0f, 0f, 0f, 0f));
				GL.PushMatrix();
				GL.LoadIdentity();
				bokehInfo.filterMode = FilterMode.Point;
				float num = (float)bokehInfo.width * 1f / ((float)bokehInfo.height * 1f);
				float num2 = 2f / (1f * (float)bokehInfo.width);
				num2 += this.bokehScale * this.maxBlurSpread * DepthOfFieldDeprecated.BOKEH_EXTRA_BLUR * this.oneOverBaseSize;
				this.bokehMaterial.SetTexture("_Source", bokehInfo);
				this.bokehMaterial.SetTexture("_MainTex", this.bokehTexture);
				this.bokehMaterial.SetVector("_ArScale", new Vector4(num2, num2 * num, 0.5f, 0.5f * num));
				this.bokehMaterial.SetFloat("_Intensity", this.bokehIntensity);
				this.bokehMaterial.SetPass(0);
				foreach (Mesh mesh in meshes)
				{
					if (mesh)
					{
						Graphics.DrawMeshNow(mesh, Matrix4x4.identity);
					}
				}
				GL.PopMatrix();
				Graphics.Blit(tempTex, finalTarget, this.dofMaterial, 8);
				bokehInfo.filterMode = FilterMode.Bilinear;
			}
		}

		// Token: 0x060070E7 RID: 28903 RVA: 0x002AD8F8 File Offset: 0x002ABCF8
		private void ReleaseTextures()
		{
			if (this.foregroundTexture)
			{
				RenderTexture.ReleaseTemporary(this.foregroundTexture);
			}
			if (this.finalDefocus)
			{
				RenderTexture.ReleaseTemporary(this.finalDefocus);
			}
			if (this.mediumRezWorkTexture)
			{
				RenderTexture.ReleaseTemporary(this.mediumRezWorkTexture);
			}
			if (this.lowRezWorkTexture)
			{
				RenderTexture.ReleaseTemporary(this.lowRezWorkTexture);
			}
			if (this.bokehSource)
			{
				RenderTexture.ReleaseTemporary(this.bokehSource);
			}
			if (this.bokehSource2)
			{
				RenderTexture.ReleaseTemporary(this.bokehSource2);
			}
		}

		// Token: 0x060070E8 RID: 28904 RVA: 0x002AD9A8 File Offset: 0x002ABDA8
		private void AllocateTextures(bool blurForeground, RenderTexture source, int divider, int lowTexDivider)
		{
			this.foregroundTexture = null;
			if (blurForeground)
			{
				this.foregroundTexture = RenderTexture.GetTemporary(source.width, source.height, 0);
			}
			this.mediumRezWorkTexture = RenderTexture.GetTemporary(source.width / divider, source.height / divider, 0);
			this.finalDefocus = RenderTexture.GetTemporary(source.width / divider, source.height / divider, 0);
			this.lowRezWorkTexture = RenderTexture.GetTemporary(source.width / lowTexDivider, source.height / lowTexDivider, 0);
			this.bokehSource = null;
			this.bokehSource2 = null;
			if (this.bokeh)
			{
				this.bokehSource = RenderTexture.GetTemporary(source.width / (lowTexDivider * this.bokehDownsample), source.height / (lowTexDivider * this.bokehDownsample), 0, RenderTextureFormat.ARGBHalf);
				this.bokehSource2 = RenderTexture.GetTemporary(source.width / (lowTexDivider * this.bokehDownsample), source.height / (lowTexDivider * this.bokehDownsample), 0, RenderTextureFormat.ARGBHalf);
				this.bokehSource.filterMode = FilterMode.Bilinear;
				this.bokehSource2.filterMode = FilterMode.Bilinear;
				RenderTexture.active = this.bokehSource2;
				GL.Clear(false, true, new Color(0f, 0f, 0f, 0f));
			}
			source.filterMode = FilterMode.Bilinear;
			this.finalDefocus.filterMode = FilterMode.Bilinear;
			this.mediumRezWorkTexture.filterMode = FilterMode.Bilinear;
			this.lowRezWorkTexture.filterMode = FilterMode.Bilinear;
			if (this.foregroundTexture)
			{
				this.foregroundTexture.filterMode = FilterMode.Bilinear;
			}
		}

		// Token: 0x060070E9 RID: 28905 RVA: 0x002ADB2D File Offset: 0x002ABF2D
		// Note: this type is marked as 'beforefieldinit'.
		static DepthOfFieldDeprecated()
		{
		}

		// Token: 0x040063F7 RID: 25591
		private static int SMOOTH_DOWNSAMPLE_PASS = 6;

		// Token: 0x040063F8 RID: 25592
		private static float BOKEH_EXTRA_BLUR = 2f;

		// Token: 0x040063F9 RID: 25593
		public DepthOfFieldDeprecated.Dof34QualitySetting quality = DepthOfFieldDeprecated.Dof34QualitySetting.OnlyBackground;

		// Token: 0x040063FA RID: 25594
		public DepthOfFieldDeprecated.DofResolution resolution = DepthOfFieldDeprecated.DofResolution.Low;

		// Token: 0x040063FB RID: 25595
		public bool simpleTweakMode = true;

		// Token: 0x040063FC RID: 25596
		public float focalPoint = 1f;

		// Token: 0x040063FD RID: 25597
		public float smoothness = 0.5f;

		// Token: 0x040063FE RID: 25598
		public float focalZDistance;

		// Token: 0x040063FF RID: 25599
		public float focalZStartCurve = 1f;

		// Token: 0x04006400 RID: 25600
		public float focalZEndCurve = 1f;

		// Token: 0x04006401 RID: 25601
		private float focalStartCurve = 2f;

		// Token: 0x04006402 RID: 25602
		private float focalEndCurve = 2f;

		// Token: 0x04006403 RID: 25603
		private float focalDistance01 = 0.1f;

		// Token: 0x04006404 RID: 25604
		public Transform objectFocus;

		// Token: 0x04006405 RID: 25605
		public float focalSize;

		// Token: 0x04006406 RID: 25606
		public DepthOfFieldDeprecated.DofBlurriness bluriness = DepthOfFieldDeprecated.DofBlurriness.High;

		// Token: 0x04006407 RID: 25607
		public float maxBlurSpread = 1.75f;

		// Token: 0x04006408 RID: 25608
		public float foregroundBlurExtrude = 1.15f;

		// Token: 0x04006409 RID: 25609
		public Shader dofBlurShader;

		// Token: 0x0400640A RID: 25610
		private Material dofBlurMaterial;

		// Token: 0x0400640B RID: 25611
		public Shader dofShader;

		// Token: 0x0400640C RID: 25612
		private Material dofMaterial;

		// Token: 0x0400640D RID: 25613
		public bool visualize;

		// Token: 0x0400640E RID: 25614
		public DepthOfFieldDeprecated.BokehDestination bokehDestination = DepthOfFieldDeprecated.BokehDestination.Background;

		// Token: 0x0400640F RID: 25615
		private float widthOverHeight = 1.25f;

		// Token: 0x04006410 RID: 25616
		private float oneOverBaseSize = 0.001953125f;

		// Token: 0x04006411 RID: 25617
		public bool bokeh;

		// Token: 0x04006412 RID: 25618
		public bool bokehSupport = true;

		// Token: 0x04006413 RID: 25619
		public Shader bokehShader;

		// Token: 0x04006414 RID: 25620
		public Texture2D bokehTexture;

		// Token: 0x04006415 RID: 25621
		public float bokehScale = 2.4f;

		// Token: 0x04006416 RID: 25622
		public float bokehIntensity = 0.15f;

		// Token: 0x04006417 RID: 25623
		public float bokehThresholdContrast = 0.1f;

		// Token: 0x04006418 RID: 25624
		public float bokehThresholdLuminance = 0.55f;

		// Token: 0x04006419 RID: 25625
		public int bokehDownsample = 1;

		// Token: 0x0400641A RID: 25626
		private Material bokehMaterial;

		// Token: 0x0400641B RID: 25627
		private Camera _camera;

		// Token: 0x0400641C RID: 25628
		private RenderTexture foregroundTexture;

		// Token: 0x0400641D RID: 25629
		private RenderTexture mediumRezWorkTexture;

		// Token: 0x0400641E RID: 25630
		private RenderTexture finalDefocus;

		// Token: 0x0400641F RID: 25631
		private RenderTexture lowRezWorkTexture;

		// Token: 0x04006420 RID: 25632
		private RenderTexture bokehSource;

		// Token: 0x04006421 RID: 25633
		private RenderTexture bokehSource2;

		// Token: 0x02000E76 RID: 3702
		public enum Dof34QualitySetting
		{
			// Token: 0x04006423 RID: 25635
			OnlyBackground = 1,
			// Token: 0x04006424 RID: 25636
			BackgroundAndForeground
		}

		// Token: 0x02000E77 RID: 3703
		public enum DofResolution
		{
			// Token: 0x04006426 RID: 25638
			High = 2,
			// Token: 0x04006427 RID: 25639
			Medium,
			// Token: 0x04006428 RID: 25640
			Low
		}

		// Token: 0x02000E78 RID: 3704
		public enum DofBlurriness
		{
			// Token: 0x0400642A RID: 25642
			Low = 1,
			// Token: 0x0400642B RID: 25643
			High,
			// Token: 0x0400642C RID: 25644
			VeryHigh = 4
		}

		// Token: 0x02000E79 RID: 3705
		public enum BokehDestination
		{
			// Token: 0x0400642E RID: 25646
			Background = 1,
			// Token: 0x0400642F RID: 25647
			Foreground,
			// Token: 0x04006430 RID: 25648
			BackgroundAndForeground
		}
	}
}
