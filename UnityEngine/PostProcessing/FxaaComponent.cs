using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200003B RID: 59
	public sealed class FxaaComponent : PostProcessingComponentRenderTexture<AntialiasingModel>
	{
		// Token: 0x06000137 RID: 311 RVA: 0x0000B6EC File Offset: 0x00009AEC
		public FxaaComponent()
		{
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000138 RID: 312 RVA: 0x0000B6F4 File Offset: 0x00009AF4
		public override bool active
		{
			get
			{
				return base.model != null && base.model.enabled && base.model.settings.method == AntialiasingModel.Method.Fxaa && this.context != null && !this.context.interrupted;
			}
		}

		// Token: 0x06000139 RID: 313 RVA: 0x0000B750 File Offset: 0x00009B50
		public void Render(RenderTexture source, RenderTexture destination)
		{
			AntialiasingModel.FxaaSettings fxaaSettings = base.model.settings.fxaaSettings;
			Material material = this.context.materialFactory.Get("Hidden/Post FX/FXAA");
			AntialiasingModel.FxaaQualitySettings fxaaQualitySettings = AntialiasingModel.FxaaQualitySettings.presets[(int)fxaaSettings.preset];
			AntialiasingModel.FxaaConsoleSettings fxaaConsoleSettings = AntialiasingModel.FxaaConsoleSettings.presets[(int)fxaaSettings.preset];
			material.SetVector(FxaaComponent.Uniforms._QualitySettings, new Vector3(fxaaQualitySettings.subpixelAliasingRemovalAmount, fxaaQualitySettings.edgeDetectionThreshold, fxaaQualitySettings.minimumRequiredLuminance));
			material.SetVector(FxaaComponent.Uniforms._ConsoleSettings, new Vector4(fxaaConsoleSettings.subpixelSpreadAmount, fxaaConsoleSettings.edgeSharpnessAmount, fxaaConsoleSettings.edgeDetectionThreshold, fxaaConsoleSettings.minimumRequiredLuminance));
			Graphics.Blit(source, destination, material, 0);
		}

		// Token: 0x0200003C RID: 60
		private static class Uniforms
		{
			// Token: 0x0600013A RID: 314 RVA: 0x0000B815 File Offset: 0x00009C15
			// Note: this type is marked as 'beforefieldinit'.
			static Uniforms()
			{
			}

			// Token: 0x0400018C RID: 396
			internal static readonly int _QualitySettings = Shader.PropertyToID("_QualitySettings");

			// Token: 0x0400018D RID: 397
			internal static readonly int _ConsoleSettings = Shader.PropertyToID("_ConsoleSettings");
		}
	}
}
