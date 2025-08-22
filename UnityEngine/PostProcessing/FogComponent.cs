using System;
using UnityEngine.Rendering;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000039 RID: 57
	public sealed class FogComponent : PostProcessingComponentCommandBuffer<FogModel>
	{
		// Token: 0x06000130 RID: 304 RVA: 0x0000B4AB File Offset: 0x000098AB
		public FogComponent()
		{
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000131 RID: 305 RVA: 0x0000B4B4 File Offset: 0x000098B4
		public override bool active
		{
			get
			{
				return base.model != null && base.model.enabled && this.context != null && this.context.isGBufferAvailable && RenderSettings.fog && !this.context.interrupted;
			}
		}

		// Token: 0x06000132 RID: 306 RVA: 0x0000B512 File Offset: 0x00009912
		public override string GetName()
		{
			return "Fog";
		}

		// Token: 0x06000133 RID: 307 RVA: 0x0000B519 File Offset: 0x00009919
		public override DepthTextureMode GetCameraFlags()
		{
			return DepthTextureMode.Depth;
		}

		// Token: 0x06000134 RID: 308 RVA: 0x0000B51C File Offset: 0x0000991C
		public override CameraEvent GetCameraEvent()
		{
			return CameraEvent.AfterImageEffectsOpaque;
		}

		// Token: 0x06000135 RID: 309 RVA: 0x0000B520 File Offset: 0x00009920
		public override void PopulateCommandBuffer(CommandBuffer cb)
		{
			FogModel.Settings settings = base.model.settings;
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Fog");
			material.shaderKeywords = null;
			Color value = (!GraphicsUtils.isLinearColorSpace) ? RenderSettings.fogColor : RenderSettings.fogColor.linear;
			material.SetColor(FogComponent.Uniforms._FogColor, value);
			material.SetFloat(FogComponent.Uniforms._Density, RenderSettings.fogDensity);
			material.SetFloat(FogComponent.Uniforms._Start, RenderSettings.fogStartDistance);
			material.SetFloat(FogComponent.Uniforms._End, RenderSettings.fogEndDistance);
			FogMode fogMode = RenderSettings.fogMode;
			if (fogMode != FogMode.Linear)
			{
				if (fogMode != FogMode.Exponential)
				{
					if (fogMode == FogMode.ExponentialSquared)
					{
						material.EnableKeyword("FOG_EXP2");
					}
				}
				else
				{
					material.EnableKeyword("FOG_EXP");
				}
			}
			else
			{
				material.EnableKeyword("FOG_LINEAR");
			}
			RenderTextureFormat format = (!this.context.isHdr) ? RenderTextureFormat.Default : RenderTextureFormat.DefaultHDR;
			cb.GetTemporaryRT(FogComponent.Uniforms._TempRT, this.context.width, this.context.height, 24, FilterMode.Bilinear, format);
			cb.Blit(BuiltinRenderTextureType.CameraTarget, FogComponent.Uniforms._TempRT);
			cb.Blit(FogComponent.Uniforms._TempRT, BuiltinRenderTextureType.CameraTarget, material, (!settings.excludeSkybox) ? 0 : 1);
			cb.ReleaseTemporaryRT(FogComponent.Uniforms._TempRT);
		}

		// Token: 0x04000186 RID: 390
		private const string k_ShaderString = "Hidden/Post FX/Fog";

		// Token: 0x0200003A RID: 58
		private static class Uniforms
		{
			// Token: 0x06000136 RID: 310 RVA: 0x0000B694 File Offset: 0x00009A94
			// Note: this type is marked as 'beforefieldinit'.
			static Uniforms()
			{
			}

			// Token: 0x04000187 RID: 391
			internal static readonly int _FogColor = Shader.PropertyToID("_FogColor");

			// Token: 0x04000188 RID: 392
			internal static readonly int _Density = Shader.PropertyToID("_Density");

			// Token: 0x04000189 RID: 393
			internal static readonly int _Start = Shader.PropertyToID("_Start");

			// Token: 0x0400018A RID: 394
			internal static readonly int _End = Shader.PropertyToID("_End");

			// Token: 0x0400018B RID: 395
			internal static readonly int _TempRT = Shader.PropertyToID("_TempRT");
		}
	}
}
