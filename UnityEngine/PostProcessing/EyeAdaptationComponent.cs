using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000037 RID: 55
	public sealed class EyeAdaptationComponent : PostProcessingComponentRenderTexture<EyeAdaptationModel>
	{
		// Token: 0x06000127 RID: 295 RVA: 0x0000AE5C File Offset: 0x0000925C
		public EyeAdaptationComponent()
		{
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x06000128 RID: 296 RVA: 0x0000AE78 File Offset: 0x00009278
		public override bool active
		{
			get
			{
				return base.model != null && base.model.enabled && SystemInfo.supportsComputeShaders && this.context != null && !this.context.interrupted;
			}
		}

		// Token: 0x06000129 RID: 297 RVA: 0x0000AEC6 File Offset: 0x000092C6
		public void ResetHistory()
		{
			this.m_FirstFrame = true;
		}

		// Token: 0x0600012A RID: 298 RVA: 0x0000AECF File Offset: 0x000092CF
		public override void OnEnable()
		{
			this.m_FirstFrame = true;
		}

		// Token: 0x0600012B RID: 299 RVA: 0x0000AED8 File Offset: 0x000092D8
		public override void OnDisable()
		{
			foreach (RenderTexture obj in this.m_AutoExposurePool)
			{
				GraphicsUtils.Destroy(obj);
			}
			if (this.m_HistogramBuffer != null)
			{
				this.m_HistogramBuffer.Release();
			}
			this.m_HistogramBuffer = null;
			if (this.m_DebugHistogram != null)
			{
				this.m_DebugHistogram.Release();
			}
			this.m_DebugHistogram = null;
		}

		// Token: 0x0600012C RID: 300 RVA: 0x0000AF4C File Offset: 0x0000934C
		private Vector4 GetHistogramScaleOffsetRes()
		{
			EyeAdaptationModel.Settings settings = base.model.settings;
			float num = (float)(settings.logMax - settings.logMin);
			float num2 = 1f / num;
			float y = (float)(-(float)settings.logMin) * num2;
			return new Vector4(num2, y, Mathf.Floor((float)this.context.width / 2f), Mathf.Floor((float)this.context.height / 2f));
		}

		// Token: 0x0600012D RID: 301 RVA: 0x0000AFC0 File Offset: 0x000093C0
		public Texture Prepare(RenderTexture source, Material uberMaterial)
		{
			EyeAdaptationModel.Settings settings = base.model.settings;
			if (this.m_EyeCompute == null)
			{
				this.m_EyeCompute = Resources.Load<ComputeShader>("Shaders/EyeHistogram");
			}
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Eye Adaptation");
			material.shaderKeywords = null;
			if (this.m_HistogramBuffer == null)
			{
				this.m_HistogramBuffer = new ComputeBuffer(64, 4);
			}
			if (EyeAdaptationComponent.s_EmptyHistogramBuffer == null)
			{
				EyeAdaptationComponent.s_EmptyHistogramBuffer = new uint[64];
			}
			Vector4 histogramScaleOffsetRes = this.GetHistogramScaleOffsetRes();
			RenderTexture renderTexture = this.context.renderTextureFactory.Get((int)histogramScaleOffsetRes.z, (int)histogramScaleOffsetRes.w, 0, source.format, RenderTextureReadWrite.Default, FilterMode.Bilinear, TextureWrapMode.Clamp, "FactoryTempTexture");
			Graphics.Blit(source, renderTexture);
			if (this.m_AutoExposurePool[0] == null || !this.m_AutoExposurePool[0].IsCreated())
			{
				this.m_AutoExposurePool[0] = new RenderTexture(1, 1, 0, RenderTextureFormat.RFloat);
			}
			if (this.m_AutoExposurePool[1] == null || !this.m_AutoExposurePool[1].IsCreated())
			{
				this.m_AutoExposurePool[1] = new RenderTexture(1, 1, 0, RenderTextureFormat.RFloat);
			}
			this.m_HistogramBuffer.SetData(EyeAdaptationComponent.s_EmptyHistogramBuffer);
			int kernelIndex = this.m_EyeCompute.FindKernel("KEyeHistogram");
			this.m_EyeCompute.SetBuffer(kernelIndex, "_Histogram", this.m_HistogramBuffer);
			this.m_EyeCompute.SetTexture(kernelIndex, "_Source", renderTexture);
			this.m_EyeCompute.SetVector("_ScaleOffsetRes", histogramScaleOffsetRes);
			this.m_EyeCompute.Dispatch(kernelIndex, Mathf.CeilToInt((float)renderTexture.width / 16f), Mathf.CeilToInt((float)renderTexture.height / 16f), 1);
			this.context.renderTextureFactory.Release(renderTexture);
			settings.highPercent = Mathf.Clamp(settings.highPercent, 1.01f, 99f);
			settings.lowPercent = Mathf.Clamp(settings.lowPercent, 1f, settings.highPercent - 0.01f);
			material.SetBuffer("_Histogram", this.m_HistogramBuffer);
			material.SetVector(EyeAdaptationComponent.Uniforms._Params, new Vector4(settings.lowPercent * 0.01f, settings.highPercent * 0.01f, Mathf.Exp(settings.minLuminance * 0.6931472f), Mathf.Exp(settings.maxLuminance * 0.6931472f)));
			material.SetVector(EyeAdaptationComponent.Uniforms._Speed, new Vector2(settings.speedDown, settings.speedUp));
			material.SetVector(EyeAdaptationComponent.Uniforms._ScaleOffsetRes, histogramScaleOffsetRes);
			material.SetFloat(EyeAdaptationComponent.Uniforms._ExposureCompensation, settings.keyValue);
			if (settings.dynamicKeyValue)
			{
				material.EnableKeyword("AUTO_KEY_VALUE");
			}
			if (this.m_FirstFrame || !Application.isPlaying)
			{
				this.m_CurrentAutoExposure = this.m_AutoExposurePool[0];
				Graphics.Blit(null, this.m_CurrentAutoExposure, material, 1);
				Graphics.Blit(this.m_AutoExposurePool[0], this.m_AutoExposurePool[1]);
			}
			else
			{
				int num = this.m_AutoExposurePingPing;
				RenderTexture source2 = this.m_AutoExposurePool[++num % 2];
				RenderTexture renderTexture2 = this.m_AutoExposurePool[++num % 2];
				Graphics.Blit(source2, renderTexture2, material, (int)settings.adaptationType);
				this.m_AutoExposurePingPing = (num + 1) % 2;
				this.m_CurrentAutoExposure = renderTexture2;
			}
			if (this.context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.EyeAdaptation))
			{
				if (this.m_DebugHistogram == null || !this.m_DebugHistogram.IsCreated())
				{
					this.m_DebugHistogram = new RenderTexture(256, 128, 0, RenderTextureFormat.ARGB32)
					{
						filterMode = FilterMode.Point,
						wrapMode = TextureWrapMode.Clamp
					};
				}
				material.SetFloat(EyeAdaptationComponent.Uniforms._DebugWidth, (float)this.m_DebugHistogram.width);
				Graphics.Blit(null, this.m_DebugHistogram, material, 2);
			}
			this.m_FirstFrame = false;
			return this.m_CurrentAutoExposure;
		}

		// Token: 0x0600012E RID: 302 RVA: 0x0000B3C4 File Offset: 0x000097C4
		public void OnGUI()
		{
			if (this.m_DebugHistogram == null || !this.m_DebugHistogram.IsCreated())
			{
				return;
			}
			Rect position = new Rect(this.context.viewport.x * (float)Screen.width + 8f, 8f, (float)this.m_DebugHistogram.width, (float)this.m_DebugHistogram.height);
			GUI.DrawTexture(position, this.m_DebugHistogram);
		}

		// Token: 0x04000175 RID: 373
		private ComputeShader m_EyeCompute;

		// Token: 0x04000176 RID: 374
		private ComputeBuffer m_HistogramBuffer;

		// Token: 0x04000177 RID: 375
		private readonly RenderTexture[] m_AutoExposurePool = new RenderTexture[2];

		// Token: 0x04000178 RID: 376
		private int m_AutoExposurePingPing;

		// Token: 0x04000179 RID: 377
		private RenderTexture m_CurrentAutoExposure;

		// Token: 0x0400017A RID: 378
		private RenderTexture m_DebugHistogram;

		// Token: 0x0400017B RID: 379
		private static uint[] s_EmptyHistogramBuffer;

		// Token: 0x0400017C RID: 380
		private bool m_FirstFrame = true;

		// Token: 0x0400017D RID: 381
		private const int k_HistogramBins = 64;

		// Token: 0x0400017E RID: 382
		private const int k_HistogramThreadX = 16;

		// Token: 0x0400017F RID: 383
		private const int k_HistogramThreadY = 16;

		// Token: 0x02000038 RID: 56
		private static class Uniforms
		{
			// Token: 0x0600012F RID: 303 RVA: 0x0000B444 File Offset: 0x00009844
			// Note: this type is marked as 'beforefieldinit'.
			static Uniforms()
			{
			}

			// Token: 0x04000180 RID: 384
			internal static readonly int _Params = Shader.PropertyToID("_Params");

			// Token: 0x04000181 RID: 385
			internal static readonly int _Speed = Shader.PropertyToID("_Speed");

			// Token: 0x04000182 RID: 386
			internal static readonly int _ScaleOffsetRes = Shader.PropertyToID("_ScaleOffsetRes");

			// Token: 0x04000183 RID: 387
			internal static readonly int _ExposureCompensation = Shader.PropertyToID("_ExposureCompensation");

			// Token: 0x04000184 RID: 388
			internal static readonly int _AutoExposure = Shader.PropertyToID("_AutoExposure");

			// Token: 0x04000185 RID: 389
			internal static readonly int _DebugWidth = Shader.PropertyToID("_DebugWidth");
		}
	}
}
