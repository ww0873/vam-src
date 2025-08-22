using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000048 RID: 72
	public sealed class TaaComponent : PostProcessingComponentRenderTexture<AntialiasingModel>
	{
		// Token: 0x06000163 RID: 355 RVA: 0x0000D14E File Offset: 0x0000B54E
		public TaaComponent()
		{
		}

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000164 RID: 356 RVA: 0x0000D16C File Offset: 0x0000B56C
		public override bool active
		{
			get
			{
				return base.model != null && base.model.enabled && base.model.settings.method == AntialiasingModel.Method.Taa && SystemInfo.supportsMotionVectors && SystemInfo.supportedRenderTargetCount >= 2 && this.context != null && !this.context.interrupted;
			}
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000D1DE File Offset: 0x0000B5DE
		public override DepthTextureMode GetCameraFlags()
		{
			return DepthTextureMode.Depth | DepthTextureMode.MotionVectors;
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000166 RID: 358 RVA: 0x0000D1E1 File Offset: 0x0000B5E1
		// (set) Token: 0x06000167 RID: 359 RVA: 0x0000D1E9 File Offset: 0x0000B5E9
		public Vector2 jitterVector
		{
			[CompilerGenerated]
			get
			{
				return this.<jitterVector>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<jitterVector>k__BackingField = value;
			}
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0000D1F2 File Offset: 0x0000B5F2
		public void ResetHistory()
		{
			this.m_ResetHistory = true;
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000D1FC File Offset: 0x0000B5FC
		public void SetProjectionMatrix(Func<Vector2, Matrix4x4> jitteredFunc)
		{
			AntialiasingModel.TaaSettings taaSettings = base.model.settings.taaSettings;
			Vector2 vector = this.GenerateRandomOffset();
			vector *= taaSettings.jitterSpread;
			this.context.camera.nonJitteredProjectionMatrix = this.context.camera.projectionMatrix;
			if (jitteredFunc != null)
			{
				this.context.camera.projectionMatrix = jitteredFunc(vector);
			}
			else
			{
				this.context.camera.projectionMatrix = ((!this.context.camera.orthographic) ? this.GetPerspectiveProjectionMatrix(vector) : this.GetOrthographicProjectionMatrix(vector));
			}
			this.context.camera.useJitteredProjectionMatrixForTransparentRendering = false;
			vector.x /= (float)this.context.width;
			vector.y /= (float)this.context.height;
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Temporal Anti-aliasing");
			material.SetVector(TaaComponent.Uniforms._Jitter, vector);
			this.jitterVector = vector;
		}

		// Token: 0x0600016A RID: 362 RVA: 0x0000D320 File Offset: 0x0000B720
		public void Render(RenderTexture source, RenderTexture destination)
		{
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Temporal Anti-aliasing");
			material.shaderKeywords = null;
			AntialiasingModel.TaaSettings taaSettings = base.model.settings.taaSettings;
			if (this.m_ResetHistory || this.m_HistoryTexture == null || this.m_HistoryTexture.width != source.width || this.m_HistoryTexture.height != source.height)
			{
				if (this.m_HistoryTexture)
				{
					RenderTexture.ReleaseTemporary(this.m_HistoryTexture);
				}
				this.m_HistoryTexture = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
				this.m_HistoryTexture.name = "TAA History";
				Graphics.Blit(source, this.m_HistoryTexture, material, 2);
			}
			material.SetVector(TaaComponent.Uniforms._SharpenParameters, new Vector4(taaSettings.sharpen, 0f, 0f, 0f));
			material.SetVector(TaaComponent.Uniforms._FinalBlendParameters, new Vector4(taaSettings.stationaryBlending, taaSettings.motionBlending, 6000f, 0f));
			material.SetTexture(TaaComponent.Uniforms._MainTex, source);
			material.SetTexture(TaaComponent.Uniforms._HistoryTex, this.m_HistoryTexture);
			RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0, source.format);
			temporary.name = "TAA History";
			this.m_MRT[0] = destination.colorBuffer;
			this.m_MRT[1] = temporary.colorBuffer;
			Graphics.SetRenderTarget(this.m_MRT, source.depthBuffer);
			GraphicsUtils.Blit(material, (!this.context.camera.orthographic) ? 0 : 1);
			RenderTexture.ReleaseTemporary(this.m_HistoryTexture);
			this.m_HistoryTexture = temporary;
			this.m_ResetHistory = false;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0000D508 File Offset: 0x0000B908
		private float GetHaltonValue(int index, int radix)
		{
			float num = 0f;
			float num2 = 1f / (float)radix;
			while (index > 0)
			{
				num += (float)(index % radix) * num2;
				index /= radix;
				num2 /= (float)radix;
			}
			return num;
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000D544 File Offset: 0x0000B944
		private Vector2 GenerateRandomOffset()
		{
			Vector2 result = new Vector2(this.GetHaltonValue(this.m_SampleIndex & 1023, 2), this.GetHaltonValue(this.m_SampleIndex & 1023, 3));
			if (++this.m_SampleIndex >= 8)
			{
				this.m_SampleIndex = 0;
			}
			return result;
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000D5A0 File Offset: 0x0000B9A0
		private Matrix4x4 GetPerspectiveProjectionMatrix(Vector2 offset)
		{
			float num = Mathf.Tan(0.008726646f * this.context.camera.fieldOfView);
			float num2 = num * this.context.camera.aspect;
			offset.x *= num2 / (0.5f * (float)this.context.width);
			offset.y *= num / (0.5f * (float)this.context.height);
			float num3 = (offset.x - num2) * this.context.camera.nearClipPlane;
			float num4 = (offset.x + num2) * this.context.camera.nearClipPlane;
			float num5 = (offset.y + num) * this.context.camera.nearClipPlane;
			float num6 = (offset.y - num) * this.context.camera.nearClipPlane;
			Matrix4x4 result = default(Matrix4x4);
			result[0, 0] = 2f * this.context.camera.nearClipPlane / (num4 - num3);
			result[0, 1] = 0f;
			result[0, 2] = (num4 + num3) / (num4 - num3);
			result[0, 3] = 0f;
			result[1, 0] = 0f;
			result[1, 1] = 2f * this.context.camera.nearClipPlane / (num5 - num6);
			result[1, 2] = (num5 + num6) / (num5 - num6);
			result[1, 3] = 0f;
			result[2, 0] = 0f;
			result[2, 1] = 0f;
			result[2, 2] = -(this.context.camera.farClipPlane + this.context.camera.nearClipPlane) / (this.context.camera.farClipPlane - this.context.camera.nearClipPlane);
			result[2, 3] = -(2f * this.context.camera.farClipPlane * this.context.camera.nearClipPlane) / (this.context.camera.farClipPlane - this.context.camera.nearClipPlane);
			result[3, 0] = 0f;
			result[3, 1] = 0f;
			result[3, 2] = -1f;
			result[3, 3] = 0f;
			return result;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0000D830 File Offset: 0x0000BC30
		private Matrix4x4 GetOrthographicProjectionMatrix(Vector2 offset)
		{
			float orthographicSize = this.context.camera.orthographicSize;
			float num = orthographicSize * this.context.camera.aspect;
			offset.x *= num / (0.5f * (float)this.context.width);
			offset.y *= orthographicSize / (0.5f * (float)this.context.height);
			float left = offset.x - num;
			float right = offset.x + num;
			float top = offset.y + orthographicSize;
			float bottom = offset.y - orthographicSize;
			return Matrix4x4.Ortho(left, right, bottom, top, this.context.camera.nearClipPlane, this.context.camera.farClipPlane);
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000D8FA File Offset: 0x0000BCFA
		public override void OnDisable()
		{
			if (this.m_HistoryTexture != null)
			{
				RenderTexture.ReleaseTemporary(this.m_HistoryTexture);
			}
			this.m_HistoryTexture = null;
			this.m_SampleIndex = 0;
			this.ResetHistory();
		}

		// Token: 0x040001F6 RID: 502
		private const string k_ShaderString = "Hidden/Post FX/Temporal Anti-aliasing";

		// Token: 0x040001F7 RID: 503
		private const int k_SampleCount = 8;

		// Token: 0x040001F8 RID: 504
		private readonly RenderBuffer[] m_MRT = new RenderBuffer[2];

		// Token: 0x040001F9 RID: 505
		private int m_SampleIndex;

		// Token: 0x040001FA RID: 506
		private bool m_ResetHistory = true;

		// Token: 0x040001FB RID: 507
		private RenderTexture m_HistoryTexture;

		// Token: 0x040001FC RID: 508
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Vector2 <jitterVector>k__BackingField;

		// Token: 0x02000049 RID: 73
		private static class Uniforms
		{
			// Token: 0x06000170 RID: 368 RVA: 0x0000D92C File Offset: 0x0000BD2C
			// Note: this type is marked as 'beforefieldinit'.
			static Uniforms()
			{
			}

			// Token: 0x040001FD RID: 509
			internal static int _Jitter = Shader.PropertyToID("_Jitter");

			// Token: 0x040001FE RID: 510
			internal static int _SharpenParameters = Shader.PropertyToID("_SharpenParameters");

			// Token: 0x040001FF RID: 511
			internal static int _FinalBlendParameters = Shader.PropertyToID("_FinalBlendParameters");

			// Token: 0x04000200 RID: 512
			internal static int _HistoryTex = Shader.PropertyToID("_HistoryTex");

			// Token: 0x04000201 RID: 513
			internal static int _MainTex = Shader.PropertyToID("_MainTex");
		}
	}
}
