using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200004C RID: 76
	public sealed class VignetteComponent : PostProcessingComponentRenderTexture<VignetteModel>
	{
		// Token: 0x06000176 RID: 374 RVA: 0x0000DB3C File Offset: 0x0000BF3C
		public VignetteComponent()
		{
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000177 RID: 375 RVA: 0x0000DB44 File Offset: 0x0000BF44
		public override bool active
		{
			get
			{
				return base.model != null && base.model.enabled && this.context != null && !this.context.interrupted;
			}
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000DB80 File Offset: 0x0000BF80
		public override void Prepare(Material uberMaterial)
		{
			VignetteModel.Settings settings = base.model.settings;
			uberMaterial.SetColor(VignetteComponent.Uniforms._Vignette_Color, settings.color);
			if (settings.mode == VignetteModel.Mode.Classic)
			{
				uberMaterial.SetVector(VignetteComponent.Uniforms._Vignette_Center, settings.center);
				uberMaterial.EnableKeyword("VIGNETTE_CLASSIC");
				float z = (1f - settings.roundness) * 6f + settings.roundness;
				uberMaterial.SetVector(VignetteComponent.Uniforms._Vignette_Settings, new Vector4(settings.intensity * 3f, settings.smoothness * 5f, z, (!settings.rounded) ? 0f : 1f));
			}
			else if (settings.mode == VignetteModel.Mode.Masked && settings.mask != null && settings.opacity > 0f)
			{
				uberMaterial.EnableKeyword("VIGNETTE_MASKED");
				uberMaterial.SetTexture(VignetteComponent.Uniforms._Vignette_Mask, settings.mask);
				uberMaterial.SetFloat(VignetteComponent.Uniforms._Vignette_Opacity, settings.opacity);
			}
		}

		// Token: 0x0200004D RID: 77
		private static class Uniforms
		{
			// Token: 0x06000179 RID: 377 RVA: 0x0000DCA0 File Offset: 0x0000C0A0
			// Note: this type is marked as 'beforefieldinit'.
			static Uniforms()
			{
			}

			// Token: 0x04000204 RID: 516
			internal static readonly int _Vignette_Color = Shader.PropertyToID("_Vignette_Color");

			// Token: 0x04000205 RID: 517
			internal static readonly int _Vignette_Center = Shader.PropertyToID("_Vignette_Center");

			// Token: 0x04000206 RID: 518
			internal static readonly int _Vignette_Settings = Shader.PropertyToID("_Vignette_Settings");

			// Token: 0x04000207 RID: 519
			internal static readonly int _Vignette_Mask = Shader.PropertyToID("_Vignette_Mask");

			// Token: 0x04000208 RID: 520
			internal static readonly int _Vignette_Opacity = Shader.PropertyToID("_Vignette_Opacity");
		}
	}
}
