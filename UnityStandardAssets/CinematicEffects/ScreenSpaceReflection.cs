using System;
using UnityEngine;

namespace UnityStandardAssets.CinematicEffects
{
	// Token: 0x02000E43 RID: 3651
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Rendering/Screen Space Reflection")]
	public class ScreenSpaceReflection : MonoBehaviour
	{
		// Token: 0x06007049 RID: 28745 RVA: 0x002A4A08 File Offset: 0x002A2E08
		public ScreenSpaceReflection()
		{
		}

		// Token: 0x17001091 RID: 4241
		// (get) Token: 0x0600704A RID: 28746 RVA: 0x002A4A2D File Offset: 0x002A2E2D
		public Material ssrMaterial
		{
			get
			{
				if (this.m_SSRMaterial == null)
				{
					this.m_SSRMaterial = ImageEffectHelper.CheckShaderAndCreateMaterial(this.ssrShader);
				}
				return this.m_SSRMaterial;
			}
		}

		// Token: 0x0600704B RID: 28747 RVA: 0x002A4A58 File Offset: 0x002A2E58
		protected void OnEnable()
		{
			if (this.ssrShader == null)
			{
				this.ssrShader = Shader.Find("Hidden/ScreenSpaceReflection");
			}
			if (!ImageEffectHelper.IsSupported(this.ssrShader, true, true, this))
			{
				base.enabled = false;
				Debug.LogWarning("The image effect " + this.ToString() + " has been disabled as it's not supported on the current platform.");
				return;
			}
			base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
		}

		// Token: 0x0600704C RID: 28748 RVA: 0x002A4AD0 File Offset: 0x002A2ED0
		private void OnDisable()
		{
			if (this.m_SSRMaterial)
			{
				UnityEngine.Object.DestroyImmediate(this.m_SSRMaterial);
			}
			if (this.m_PreviousDepthBuffer)
			{
				UnityEngine.Object.DestroyImmediate(this.m_PreviousDepthBuffer);
			}
			if (this.m_PreviousHitBuffer)
			{
				UnityEngine.Object.DestroyImmediate(this.m_PreviousHitBuffer);
			}
			if (this.m_PreviousReflectionBuffer)
			{
				UnityEngine.Object.DestroyImmediate(this.m_PreviousReflectionBuffer);
			}
			this.m_SSRMaterial = null;
			this.m_PreviousDepthBuffer = null;
			this.m_PreviousHitBuffer = null;
			this.m_PreviousReflectionBuffer = null;
		}

		// Token: 0x0600704D RID: 28749 RVA: 0x002A4B68 File Offset: 0x002A2F68
		private void PreparePreviousBuffers(int w, int h)
		{
			if (this.m_PreviousDepthBuffer != null && (this.m_PreviousDepthBuffer.width != w || this.m_PreviousDepthBuffer.height != h))
			{
				UnityEngine.Object.DestroyImmediate(this.m_PreviousDepthBuffer);
				UnityEngine.Object.DestroyImmediate(this.m_PreviousHitBuffer);
				UnityEngine.Object.DestroyImmediate(this.m_PreviousReflectionBuffer);
				this.m_PreviousDepthBuffer = null;
				this.m_PreviousHitBuffer = null;
				this.m_PreviousReflectionBuffer = null;
			}
			if (this.m_PreviousDepthBuffer == null)
			{
				this.m_PreviousDepthBuffer = new RenderTexture(w, h, 0, RenderTextureFormat.RFloat);
				this.m_PreviousHitBuffer = new RenderTexture(w, h, 0, RenderTextureFormat.ARGBHalf);
				this.m_PreviousReflectionBuffer = new RenderTexture(w, h, 0, RenderTextureFormat.ARGBHalf);
			}
		}

		// Token: 0x0600704E RID: 28750 RVA: 0x002A4C20 File Offset: 0x002A3020
		[ImageEffectOpaque]
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.ssrMaterial == null)
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (this.m_HasInformationFromPreviousFrame)
			{
				this.m_HasInformationFromPreviousFrame = (this.m_PreviousDepthBuffer != null && source.width == this.m_PreviousDepthBuffer.width && source.height == this.m_PreviousDepthBuffer.height);
			}
			bool flag = this.m_HasInformationFromPreviousFrame && (double)this.settings.advancedSettings.temporalFilterStrength > 0.0;
			this.m_HasInformationFromPreviousFrame = false;
			if (Camera.current.actualRenderingPath != RenderingPath.DeferredShading)
			{
				Graphics.Blit(source, destination);
				return;
			}
			int num = source.width;
			int num2 = source.height;
			RenderTexture temporaryRenderTexture = this.m_RTU.GetTemporaryRenderTexture(num, num2, 0, RenderTextureFormat.ARGB32, FilterMode.Bilinear);
			temporaryRenderTexture.filterMode = FilterMode.Point;
			Graphics.Blit(source, temporaryRenderTexture, this.ssrMaterial, 12);
			this.ssrMaterial.SetTexture("_NormalAndRoughnessTexture", temporaryRenderTexture);
			float num3 = (float)source.width;
			float num4 = (float)source.height;
			Vector2 vector = new Vector2(num3 / (float)num, num4 / (float)num2);
			int num5 = (this.settings.advancedSettings.resolution != ScreenSpaceReflection.SSRResolution.FullResolution) ? 2 : 1;
			num /= num5;
			num2 /= num5;
			this.ssrMaterial.SetVector("_SourceToTempUV", new Vector4(vector.x, vector.y, 1f / vector.x, 1f / vector.y));
			Matrix4x4 projectionMatrix = base.GetComponent<Camera>().projectionMatrix;
			Vector4 value = new Vector4(-2f / (num3 * projectionMatrix[0]), -2f / (num4 * projectionMatrix[5]), (1f - projectionMatrix[2]) / projectionMatrix[0], (1f + projectionMatrix[6]) / projectionMatrix[5]);
			float value2 = num3 / (-2f * (float)Math.Tan((double)base.GetComponent<Camera>().fieldOfView / 180.0 * 3.141592653589793 * 0.5));
			this.ssrMaterial.SetFloat("_PixelsPerMeterAtOneMeter", value2);
			float num6 = num3 / 2f;
			float num7 = num4 / 2f;
			Matrix4x4 lhs = default(Matrix4x4);
			lhs.SetRow(0, new Vector4(num6, 0f, 0f, num6));
			lhs.SetRow(1, new Vector4(0f, num7, 0f, num7));
			lhs.SetRow(2, new Vector4(0f, 0f, 1f, 0f));
			lhs.SetRow(3, new Vector4(0f, 0f, 0f, 1f));
			Matrix4x4 value3 = lhs * projectionMatrix;
			this.ssrMaterial.SetVector("_ScreenSize", new Vector2(num3, num4));
			this.ssrMaterial.SetVector("_ReflectionBufferSize", new Vector2((float)num, (float)num2));
			Vector2 v = new Vector2((float)(1.0 / (double)num3), (float)(1.0 / (double)num4));
			Matrix4x4 worldToCameraMatrix = base.GetComponent<Camera>().worldToCameraMatrix;
			Matrix4x4 inverse = base.GetComponent<Camera>().worldToCameraMatrix.inverse;
			this.ssrMaterial.SetVector("_InvScreenSize", v);
			this.ssrMaterial.SetVector("_ProjInfo", value);
			this.ssrMaterial.SetMatrix("_ProjectToPixelMatrix", value3);
			this.ssrMaterial.SetMatrix("_WorldToCameraMatrix", worldToCameraMatrix);
			this.ssrMaterial.SetMatrix("_CameraToWorldMatrix", inverse);
			this.ssrMaterial.SetInt("_EnableRefine", (!this.settings.advancedSettings.reduceBanding) ? 0 : 1);
			this.ssrMaterial.SetInt("_AdditiveReflection", (!this.settings.basicSettings.additiveReflection) ? 0 : 1);
			this.ssrMaterial.SetInt("_ImproveCorners", (!this.settings.advancedSettings.improveCorners) ? 0 : 1);
			this.ssrMaterial.SetFloat("_ScreenEdgeFading", this.settings.basicSettings.screenEdgeFading);
			this.ssrMaterial.SetFloat("_MipBias", this.mipBias);
			this.ssrMaterial.SetInt("_UseOcclusion", (!this.useOcclusion) ? 0 : 1);
			this.ssrMaterial.SetInt("_BilateralUpsampling", (!this.settings.advancedSettings.bilateralUpsample) ? 0 : 1);
			this.ssrMaterial.SetInt("_FallbackToSky", (!this.fallbackToSky) ? 0 : 1);
			this.ssrMaterial.SetInt("_TreatBackfaceHitAsMiss", (!this.settings.advancedSettings.treatBackfaceHitAsMiss) ? 0 : 1);
			this.ssrMaterial.SetInt("_AllowBackwardsRays", (!this.settings.advancedSettings.allowBackwardsRays) ? 0 : 1);
			this.ssrMaterial.SetInt("_TraceEverywhere", (!this.settings.advancedSettings.traceEverywhere) ? 0 : 1);
			float farClipPlane = base.GetComponent<Camera>().farClipPlane;
			float nearClipPlane = base.GetComponent<Camera>().nearClipPlane;
			Vector3 v2 = (!float.IsPositiveInfinity(farClipPlane)) ? new Vector3(nearClipPlane * farClipPlane, nearClipPlane - farClipPlane, farClipPlane) : new Vector3(nearClipPlane, -1f, 1f);
			this.ssrMaterial.SetVector("_CameraClipInfo", v2);
			this.ssrMaterial.SetFloat("_MaxRayTraceDistance", this.settings.basicSettings.maxDistance);
			this.ssrMaterial.SetFloat("_FadeDistance", this.settings.basicSettings.fadeDistance);
			this.ssrMaterial.SetFloat("_LayerThickness", this.settings.reflectionSettings.widthModifier);
			RenderTextureFormat format = (!this.settings.basicSettings.enableHDR) ? RenderTextureFormat.ARGB32 : RenderTextureFormat.ARGBHalf;
			RenderTexture[] array = new RenderTexture[5];
			for (int i = 0; i < 5; i++)
			{
				if (this.fullResolutionFiltering)
				{
					array[i] = this.m_RTU.GetTemporaryRenderTexture(num, num2, 0, format, FilterMode.Bilinear);
				}
				else
				{
					array[i] = this.m_RTU.GetTemporaryRenderTexture(num >> i, num2 >> i, 0, format, FilterMode.Bilinear);
				}
				array[i].filterMode = ((!this.settings.advancedSettings.bilateralUpsample) ? FilterMode.Bilinear : FilterMode.Point);
			}
			this.ssrMaterial.SetInt("_EnableSSR", 1);
			this.ssrMaterial.SetInt("_DebugMode", (int)this.settings.debugSettings.debugMode);
			this.ssrMaterial.SetInt("_TraceBehindObjects", (!this.settings.advancedSettings.traceBehindObjects) ? 0 : 1);
			this.ssrMaterial.SetInt("_MaxSteps", this.settings.reflectionSettings.maxSteps);
			RenderTexture temporaryRenderTexture2 = this.m_RTU.GetTemporaryRenderTexture(num, num2, 0, RenderTextureFormat.ARGBHalf, FilterMode.Bilinear);
			int pass = Mathf.Clamp(this.settings.reflectionSettings.rayStepSize, 0, 4);
			Graphics.Blit(source, temporaryRenderTexture2, this.ssrMaterial, pass);
			this.ssrMaterial.SetTexture("_HitPointTexture", temporaryRenderTexture2);
			Graphics.Blit(source, array[0], this.ssrMaterial, 11);
			this.ssrMaterial.SetTexture("_ReflectionTexture0", array[0]);
			this.ssrMaterial.SetInt("_FullResolutionFiltering", (!this.fullResolutionFiltering) ? 0 : 1);
			this.ssrMaterial.SetFloat("_MaxRoughness", 1f - this.settings.reflectionSettings.smoothFallbackThreshold);
			this.ssrMaterial.SetFloat("_RoughnessFalloffRange", this.settings.reflectionSettings.smoothFallbackDistance);
			this.ssrMaterial.SetFloat("_SSRMultiplier", this.settings.basicSettings.reflectionMultiplier);
			RenderTexture[] array2 = new RenderTexture[5];
			if (this.settings.advancedSettings.bilateralUpsample && this.useEdgeDetector)
			{
				array2[0] = this.m_RTU.GetTemporaryRenderTexture(num, num2, 0, RenderTextureFormat.ARGBHalf, FilterMode.Bilinear);
				Graphics.Blit(source, array2[0], this.ssrMaterial, 9);
				for (int j = 1; j < 5; j++)
				{
					array2[j] = this.m_RTU.GetTemporaryRenderTexture(num >> j, num2 >> j, 0, RenderTextureFormat.ARGBHalf, FilterMode.Bilinear);
					this.ssrMaterial.SetInt("_LastMip", j - 1);
					Graphics.Blit(array2[j - 1], array2[j], this.ssrMaterial, 10);
				}
			}
			if (this.settings.advancedSettings.highQualitySharpReflections)
			{
				RenderTexture temporaryRenderTexture3 = this.m_RTU.GetTemporaryRenderTexture(array[0].width, array[0].height, 0, array[0].format, FilterMode.Bilinear);
				temporaryRenderTexture3.filterMode = array[0].filterMode;
				array[0].filterMode = FilterMode.Bilinear;
				Graphics.Blit(array[0], temporaryRenderTexture3, this.ssrMaterial, 16);
				this.m_RTU.ReleaseTemporaryRenderTexture(array[0]);
				array[0] = temporaryRenderTexture3;
				this.ssrMaterial.SetTexture("_ReflectionTexture0", array[0]);
			}
			for (int k = 1; k < 5; k++)
			{
				RenderTexture renderTexture = array[k - 1];
				RenderTexture temporaryRenderTexture4;
				if (this.fullResolutionFiltering)
				{
					temporaryRenderTexture4 = this.m_RTU.GetTemporaryRenderTexture(num, num2, 0, format, FilterMode.Bilinear);
				}
				else
				{
					int num8 = k;
					temporaryRenderTexture4 = this.m_RTU.GetTemporaryRenderTexture(num >> num8, num2 >> k - 1, 0, format, FilterMode.Bilinear);
				}
				for (int l = 0; l < ((!this.fullResolutionFiltering) ? 1 : (k * k)); l++)
				{
					this.ssrMaterial.SetVector("_Axis", new Vector4(1f, 0f, 0f, 0f));
					this.ssrMaterial.SetFloat("_CurrentMipLevel", (float)k - 1f);
					Graphics.Blit(renderTexture, temporaryRenderTexture4, this.ssrMaterial, 6);
					this.ssrMaterial.SetVector("_Axis", new Vector4(0f, 1f, 0f, 0f));
					renderTexture = array[k];
					Graphics.Blit(temporaryRenderTexture4, renderTexture, this.ssrMaterial, 6);
				}
				this.ssrMaterial.SetTexture("_ReflectionTexture" + k, array[k]);
				this.m_RTU.ReleaseTemporaryRenderTexture(temporaryRenderTexture4);
			}
			if (this.settings.advancedSettings.bilateralUpsample && this.useEdgeDetector)
			{
				for (int m = 0; m < 5; m++)
				{
					this.ssrMaterial.SetTexture("_EdgeTexture" + m, array2[m]);
				}
			}
			this.ssrMaterial.SetInt("_UseEdgeDetector", (!this.useEdgeDetector) ? 0 : 1);
			RenderTexture temporaryRenderTexture5 = this.m_RTU.GetTemporaryRenderTexture(source.width, source.height, 0, RenderTextureFormat.RHalf, FilterMode.Bilinear);
			if (this.computeAverageRayDistance)
			{
				Graphics.Blit(source, temporaryRenderTexture5, this.ssrMaterial, 15);
			}
			this.ssrMaterial.SetInt("_UseAverageRayDistance", (!this.computeAverageRayDistance) ? 0 : 1);
			this.ssrMaterial.SetTexture("_AverageRayDistanceBuffer", temporaryRenderTexture5);
			bool flag2 = this.settings.advancedSettings.resolution == ScreenSpaceReflection.SSRResolution.HalfTraceFullResolve;
			RenderTexture temporaryRenderTexture6 = this.m_RTU.GetTemporaryRenderTexture((!flag2) ? num : source.width, (!flag2) ? num2 : source.height, 0, format, FilterMode.Bilinear);
			this.ssrMaterial.SetFloat("_FresnelFade", this.settings.reflectionSettings.fresnelFade);
			this.ssrMaterial.SetFloat("_FresnelFadePower", this.settings.reflectionSettings.fresnelFadePower);
			this.ssrMaterial.SetFloat("_DistanceBlur", this.settings.reflectionSettings.distanceBlur);
			this.ssrMaterial.SetInt("_HalfResolution", (this.settings.advancedSettings.resolution == ScreenSpaceReflection.SSRResolution.FullResolution) ? 0 : 1);
			this.ssrMaterial.SetInt("_HighlightSuppression", (!this.settings.advancedSettings.highlightSuppression) ? 0 : 1);
			Graphics.Blit(array[0], temporaryRenderTexture6, this.ssrMaterial, 7);
			this.ssrMaterial.SetTexture("_FinalReflectionTexture", temporaryRenderTexture6);
			RenderTexture temporaryRenderTexture7 = this.m_RTU.GetTemporaryRenderTexture((!flag2) ? num : source.width, (!flag2) ? num2 : source.height, 0, format, FilterMode.Bilinear);
			if (flag)
			{
				this.ssrMaterial.SetInt("_UseTemporalConfidence", (!this.settings.advancedSettings.useTemporalConfidence) ? 0 : 1);
				this.ssrMaterial.SetFloat("_TemporalAlpha", this.settings.advancedSettings.temporalFilterStrength);
				this.ssrMaterial.SetMatrix("_CurrentCameraToPreviousCamera", this.m_PreviousWorldToCameraMatrix * inverse);
				this.ssrMaterial.SetTexture("_PreviousReflectionTexture", this.m_PreviousReflectionBuffer);
				this.ssrMaterial.SetTexture("_PreviousCSZBuffer", this.m_PreviousDepthBuffer);
				Graphics.Blit(source, temporaryRenderTexture7, this.ssrMaterial, 14);
				this.ssrMaterial.SetTexture("_FinalReflectionTexture", temporaryRenderTexture7);
			}
			if ((double)this.settings.advancedSettings.temporalFilterStrength > 0.0)
			{
				this.m_PreviousWorldToCameraMatrix = worldToCameraMatrix;
				this.PreparePreviousBuffers(source.width, source.height);
				Graphics.Blit(source, this.m_PreviousDepthBuffer, this.ssrMaterial, 13);
				Graphics.Blit(temporaryRenderTexture2, this.m_PreviousHitBuffer);
				Graphics.Blit((!flag) ? temporaryRenderTexture6 : temporaryRenderTexture7, this.m_PreviousReflectionBuffer);
				this.m_HasInformationFromPreviousFrame = true;
			}
			Graphics.Blit(source, destination, this.ssrMaterial, 5);
			this.m_RTU.ReleaseAllTemporyRenderTexutres();
		}

		// Token: 0x0400625A RID: 25178
		[SerializeField]
		public ScreenSpaceReflection.SSRSettings settings = ScreenSpaceReflection.SSRSettings.defaultSettings;

		// Token: 0x0400625B RID: 25179
		[Tooltip("Enable to try and bypass expensive bilateral upsampling away from edges. There is a slight performance hit for generating the edge buffers, but a potentially high performance savings from bypassing bilateral upsampling where it is unneeded. Test on your target platforms to see if performance improves.")]
		private bool useEdgeDetector;

		// Token: 0x0400625C RID: 25180
		[Range(-4f, 4f)]
		private float mipBias;

		// Token: 0x0400625D RID: 25181
		private bool useOcclusion = true;

		// Token: 0x0400625E RID: 25182
		private bool fullResolutionFiltering;

		// Token: 0x0400625F RID: 25183
		private bool fallbackToSky;

		// Token: 0x04006260 RID: 25184
		private bool computeAverageRayDistance;

		// Token: 0x04006261 RID: 25185
		private bool m_HasInformationFromPreviousFrame;

		// Token: 0x04006262 RID: 25186
		private Matrix4x4 m_PreviousWorldToCameraMatrix;

		// Token: 0x04006263 RID: 25187
		private RenderTexture m_PreviousDepthBuffer;

		// Token: 0x04006264 RID: 25188
		private RenderTexture m_PreviousHitBuffer;

		// Token: 0x04006265 RID: 25189
		private RenderTexture m_PreviousReflectionBuffer;

		// Token: 0x04006266 RID: 25190
		public Shader ssrShader;

		// Token: 0x04006267 RID: 25191
		private Material m_SSRMaterial;

		// Token: 0x04006268 RID: 25192
		[NonSerialized]
		private RenderTexureUtility m_RTU = new RenderTexureUtility();

		// Token: 0x02000E44 RID: 3652
		public enum SSRDebugMode
		{
			// Token: 0x0400626A RID: 25194
			None,
			// Token: 0x0400626B RID: 25195
			IncomingRadiance,
			// Token: 0x0400626C RID: 25196
			SSRResult,
			// Token: 0x0400626D RID: 25197
			FinalGlossyTerm,
			// Token: 0x0400626E RID: 25198
			SSRMask,
			// Token: 0x0400626F RID: 25199
			Roughness,
			// Token: 0x04006270 RID: 25200
			BaseColor,
			// Token: 0x04006271 RID: 25201
			SpecColor,
			// Token: 0x04006272 RID: 25202
			Reflectivity,
			// Token: 0x04006273 RID: 25203
			ReflectionProbeOnly,
			// Token: 0x04006274 RID: 25204
			ReflectionProbeMinusSSR,
			// Token: 0x04006275 RID: 25205
			SSRMinusReflectionProbe,
			// Token: 0x04006276 RID: 25206
			NoGlossy,
			// Token: 0x04006277 RID: 25207
			NegativeNoGlossy,
			// Token: 0x04006278 RID: 25208
			MipLevel
		}

		// Token: 0x02000E45 RID: 3653
		public enum SSRResolution
		{
			// Token: 0x0400627A RID: 25210
			FullResolution,
			// Token: 0x0400627B RID: 25211
			HalfTraceFullResolve,
			// Token: 0x0400627C RID: 25212
			HalfResolution
		}

		// Token: 0x02000E46 RID: 3654
		[Serializable]
		public struct SSRSettings
		{
			// Token: 0x17001092 RID: 4242
			// (get) Token: 0x0600704F RID: 28751 RVA: 0x002A5AA1 File Offset: 0x002A3EA1
			public static ScreenSpaceReflection.SSRSettings performanceSettings
			{
				get
				{
					return ScreenSpaceReflection.SSRSettings.s_Performance;
				}
			}

			// Token: 0x17001093 RID: 4243
			// (get) Token: 0x06007050 RID: 28752 RVA: 0x002A5AA8 File Offset: 0x002A3EA8
			public static ScreenSpaceReflection.SSRSettings defaultSettings
			{
				get
				{
					return ScreenSpaceReflection.SSRSettings.s_Default;
				}
			}

			// Token: 0x17001094 RID: 4244
			// (get) Token: 0x06007051 RID: 28753 RVA: 0x002A5AAF File Offset: 0x002A3EAF
			public static ScreenSpaceReflection.SSRSettings highQualitySettings
			{
				get
				{
					return ScreenSpaceReflection.SSRSettings.s_HighQuality;
				}
			}

			// Token: 0x06007052 RID: 28754 RVA: 0x002A5AB8 File Offset: 0x002A3EB8
			// Note: this type is marked as 'beforefieldinit'.
			static SSRSettings()
			{
			}

			// Token: 0x0400627D RID: 25213
			[ScreenSpaceReflection.SSRSettings.LayoutAttribute]
			public ScreenSpaceReflection.BasicSettings basicSettings;

			// Token: 0x0400627E RID: 25214
			[ScreenSpaceReflection.SSRSettings.LayoutAttribute]
			public ScreenSpaceReflection.ReflectionSettings reflectionSettings;

			// Token: 0x0400627F RID: 25215
			[ScreenSpaceReflection.SSRSettings.LayoutAttribute]
			public ScreenSpaceReflection.AdvancedSettings advancedSettings;

			// Token: 0x04006280 RID: 25216
			[ScreenSpaceReflection.SSRSettings.LayoutAttribute]
			public ScreenSpaceReflection.DebugSettings debugSettings;

			// Token: 0x04006281 RID: 25217
			private static readonly ScreenSpaceReflection.SSRSettings s_Performance = new ScreenSpaceReflection.SSRSettings
			{
				basicSettings = new ScreenSpaceReflection.BasicSettings
				{
					screenEdgeFading = 0f,
					maxDistance = 10f,
					fadeDistance = 10f,
					reflectionMultiplier = 1f,
					enableHDR = false,
					additiveReflection = false
				},
				reflectionSettings = new ScreenSpaceReflection.ReflectionSettings
				{
					maxSteps = 64,
					rayStepSize = 4,
					widthModifier = 0.5f,
					smoothFallbackThreshold = 0.4f,
					distanceBlur = 1f,
					fresnelFade = 0.2f,
					fresnelFadePower = 2f,
					smoothFallbackDistance = 0.05f
				},
				advancedSettings = new ScreenSpaceReflection.AdvancedSettings
				{
					useTemporalConfidence = false,
					temporalFilterStrength = 0f,
					treatBackfaceHitAsMiss = false,
					allowBackwardsRays = false,
					traceBehindObjects = true,
					highQualitySharpReflections = false,
					traceEverywhere = false,
					resolution = ScreenSpaceReflection.SSRResolution.HalfResolution,
					bilateralUpsample = false,
					improveCorners = false,
					reduceBanding = false,
					highlightSuppression = false
				},
				debugSettings = new ScreenSpaceReflection.DebugSettings
				{
					debugMode = ScreenSpaceReflection.SSRDebugMode.None
				}
			};

			// Token: 0x04006282 RID: 25218
			private static readonly ScreenSpaceReflection.SSRSettings s_Default = new ScreenSpaceReflection.SSRSettings
			{
				basicSettings = new ScreenSpaceReflection.BasicSettings
				{
					screenEdgeFading = 0.03f,
					maxDistance = 100f,
					fadeDistance = 100f,
					reflectionMultiplier = 1f,
					enableHDR = true,
					additiveReflection = false
				},
				reflectionSettings = new ScreenSpaceReflection.ReflectionSettings
				{
					maxSteps = 128,
					rayStepSize = 3,
					widthModifier = 0.5f,
					smoothFallbackThreshold = 0.2f,
					distanceBlur = 1f,
					fresnelFade = 0.2f,
					fresnelFadePower = 2f,
					smoothFallbackDistance = 0.05f
				},
				advancedSettings = new ScreenSpaceReflection.AdvancedSettings
				{
					useTemporalConfidence = true,
					temporalFilterStrength = 0.7f,
					treatBackfaceHitAsMiss = false,
					allowBackwardsRays = false,
					traceBehindObjects = true,
					highQualitySharpReflections = true,
					traceEverywhere = true,
					resolution = ScreenSpaceReflection.SSRResolution.HalfTraceFullResolve,
					bilateralUpsample = true,
					improveCorners = true,
					reduceBanding = true,
					highlightSuppression = false
				},
				debugSettings = new ScreenSpaceReflection.DebugSettings
				{
					debugMode = ScreenSpaceReflection.SSRDebugMode.None
				}
			};

			// Token: 0x04006283 RID: 25219
			private static readonly ScreenSpaceReflection.SSRSettings s_HighQuality = new ScreenSpaceReflection.SSRSettings
			{
				basicSettings = new ScreenSpaceReflection.BasicSettings
				{
					screenEdgeFading = 0.03f,
					maxDistance = 100f,
					fadeDistance = 100f,
					reflectionMultiplier = 1f,
					enableHDR = true,
					additiveReflection = false
				},
				reflectionSettings = new ScreenSpaceReflection.ReflectionSettings
				{
					maxSteps = 512,
					rayStepSize = 1,
					widthModifier = 0.5f,
					smoothFallbackThreshold = 0.2f,
					distanceBlur = 1f,
					fresnelFade = 0.2f,
					fresnelFadePower = 2f,
					smoothFallbackDistance = 0.05f
				},
				advancedSettings = new ScreenSpaceReflection.AdvancedSettings
				{
					useTemporalConfidence = true,
					temporalFilterStrength = 0.7f,
					treatBackfaceHitAsMiss = false,
					allowBackwardsRays = false,
					traceBehindObjects = true,
					highQualitySharpReflections = true,
					traceEverywhere = true,
					resolution = ScreenSpaceReflection.SSRResolution.HalfTraceFullResolve,
					bilateralUpsample = true,
					improveCorners = true,
					reduceBanding = true,
					highlightSuppression = false
				},
				debugSettings = new ScreenSpaceReflection.DebugSettings
				{
					debugMode = ScreenSpaceReflection.SSRDebugMode.None
				}
			};

			// Token: 0x02000E47 RID: 3655
			[AttributeUsage(AttributeTargets.Field)]
			public class LayoutAttribute : PropertyAttribute
			{
				// Token: 0x06007053 RID: 28755 RVA: 0x002A5ECF File Offset: 0x002A42CF
				public LayoutAttribute()
				{
				}
			}
		}

		// Token: 0x02000E48 RID: 3656
		[Serializable]
		public struct BasicSettings
		{
			// Token: 0x04006284 RID: 25220
			[Tooltip("Nonphysical multiplier for the SSR reflections. 1.0 is physically based.")]
			[Range(0f, 2f)]
			public float reflectionMultiplier;

			// Token: 0x04006285 RID: 25221
			[Tooltip("Maximum reflection distance in world units.")]
			[Range(0.5f, 1000f)]
			public float maxDistance;

			// Token: 0x04006286 RID: 25222
			[Tooltip("How far away from the maxDistance to begin fading SSR.")]
			[Range(0f, 1000f)]
			public float fadeDistance;

			// Token: 0x04006287 RID: 25223
			[Tooltip("Higher = fade out SSRR near the edge of the screen so that reflections don't pop under camera motion.")]
			[Range(0f, 1f)]
			public float screenEdgeFading;

			// Token: 0x04006288 RID: 25224
			[Tooltip("Enable for better reflections of very bright objects at a performance cost")]
			public bool enableHDR;

			// Token: 0x04006289 RID: 25225
			[Tooltip("Add reflections on top of existing ones. Not physically correct.")]
			public bool additiveReflection;
		}

		// Token: 0x02000E49 RID: 3657
		[Serializable]
		public struct ReflectionSettings
		{
			// Token: 0x0400628A RID: 25226
			[Tooltip("Max raytracing length.")]
			[Range(16f, 2048f)]
			public int maxSteps;

			// Token: 0x0400628B RID: 25227
			[Tooltip("Log base 2 of ray tracing coarse step size. Higher traces farther, lower gives better quality silhouettes.")]
			[Range(0f, 4f)]
			public int rayStepSize;

			// Token: 0x0400628C RID: 25228
			[Tooltip("Typical thickness of columns, walls, furniture, and other objects that reflection rays might pass behind.")]
			[Range(0.01f, 10f)]
			public float widthModifier;

			// Token: 0x0400628D RID: 25229
			[Tooltip("Increase if reflections flicker on very rough surfaces.")]
			[Range(0f, 1f)]
			public float smoothFallbackThreshold;

			// Token: 0x0400628E RID: 25230
			[Tooltip("Start falling back to non-SSR value solution at smoothFallbackThreshold - smoothFallbackDistance, with full fallback occuring at smoothFallbackThreshold.")]
			[Range(0f, 0.2f)]
			public float smoothFallbackDistance;

			// Token: 0x0400628F RID: 25231
			[Tooltip("Amplify Fresnel fade out. Increase if floor reflections look good close to the surface and bad farther 'under' the floor.")]
			[Range(0f, 1f)]
			public float fresnelFade;

			// Token: 0x04006290 RID: 25232
			[Tooltip("Higher values correspond to a faster Fresnel fade as the reflection changes from the grazing angle.")]
			[Range(0.1f, 10f)]
			public float fresnelFadePower;

			// Token: 0x04006291 RID: 25233
			[Tooltip("Controls how blurry reflections get as objects are further from the camera. 0 is constant blur no matter trace distance or distance from camera. 1 fully takes into account both factors.")]
			[Range(0f, 1f)]
			public float distanceBlur;
		}

		// Token: 0x02000E4A RID: 3658
		[Serializable]
		public struct AdvancedSettings
		{
			// Token: 0x04006292 RID: 25234
			[Range(0f, 0.99f)]
			[Tooltip("Increase to decrease flicker in scenes; decrease to prevent ghosting (especially in dynamic scenes). 0 gives maximum performance.")]
			public float temporalFilterStrength;

			// Token: 0x04006293 RID: 25235
			[Tooltip("Enable to limit ghosting from applying the temporal filter.")]
			public bool useTemporalConfidence;

			// Token: 0x04006294 RID: 25236
			[Tooltip("Enable to allow rays to pass behind objects. This can lead to more screen-space reflections, but the reflections are more likely to be wrong.")]
			public bool traceBehindObjects;

			// Token: 0x04006295 RID: 25237
			[Tooltip("Enable to increase quality of the sharpest reflections (through filtering), at a performance cost.")]
			public bool highQualitySharpReflections;

			// Token: 0x04006296 RID: 25238
			[Tooltip("Improves quality in scenes with varying smoothness, at a potential performance cost.")]
			public bool traceEverywhere;

			// Token: 0x04006297 RID: 25239
			[Tooltip("Enable to force more surfaces to use reflection probes if you see streaks on the sides of objects or bad reflections of their backs.")]
			public bool treatBackfaceHitAsMiss;

			// Token: 0x04006298 RID: 25240
			[Tooltip("Enable for a performance gain in scenes where most glossy objects are horizontal, like floors, water, and tables. Leave on for scenes with glossy vertical objects.")]
			public bool allowBackwardsRays;

			// Token: 0x04006299 RID: 25241
			[Tooltip("Improve visual fidelity of reflections on rough surfaces near corners in the scene, at the cost of a small amount of performance.")]
			public bool improveCorners;

			// Token: 0x0400629A RID: 25242
			[Tooltip("Half resolution SSRR is much faster, but less accurate. Quality can be reclaimed for some performance by doing the resolve at full resolution.")]
			public ScreenSpaceReflection.SSRResolution resolution;

			// Token: 0x0400629B RID: 25243
			[Tooltip("Drastically improves reflection reconstruction quality at the expense of some performance.")]
			public bool bilateralUpsample;

			// Token: 0x0400629C RID: 25244
			[Tooltip("Improve visual fidelity of mirror reflections at the cost of a small amount of performance.")]
			public bool reduceBanding;

			// Token: 0x0400629D RID: 25245
			[Tooltip("Enable to limit the effect a few bright pixels can have on rougher surfaces")]
			public bool highlightSuppression;
		}

		// Token: 0x02000E4B RID: 3659
		[Serializable]
		public struct DebugSettings
		{
			// Token: 0x0400629E RID: 25246
			[Tooltip("Various Debug Visualizations")]
			public ScreenSpaceReflection.SSRDebugMode debugMode;
		}

		// Token: 0x02000E4C RID: 3660
		private enum PassIndex
		{
			// Token: 0x040062A0 RID: 25248
			RayTraceStep1,
			// Token: 0x040062A1 RID: 25249
			RayTraceStep2,
			// Token: 0x040062A2 RID: 25250
			RayTraceStep4,
			// Token: 0x040062A3 RID: 25251
			RayTraceStep8,
			// Token: 0x040062A4 RID: 25252
			RayTraceStep16,
			// Token: 0x040062A5 RID: 25253
			CompositeFinal,
			// Token: 0x040062A6 RID: 25254
			Blur,
			// Token: 0x040062A7 RID: 25255
			CompositeSSR,
			// Token: 0x040062A8 RID: 25256
			Blit,
			// Token: 0x040062A9 RID: 25257
			EdgeGeneration,
			// Token: 0x040062AA RID: 25258
			MinMipGeneration,
			// Token: 0x040062AB RID: 25259
			HitPointToReflections,
			// Token: 0x040062AC RID: 25260
			BilateralKeyPack,
			// Token: 0x040062AD RID: 25261
			BlitDepthAsCSZ,
			// Token: 0x040062AE RID: 25262
			TemporalFilter,
			// Token: 0x040062AF RID: 25263
			AverageRayDistanceGeneration,
			// Token: 0x040062B0 RID: 25264
			PoissonBlur
		}
	}
}
