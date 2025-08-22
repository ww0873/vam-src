using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000E57 RID: 3671
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Other/Antialiasing")]
	public class Antialiasing : PostEffectsBase
	{
		// Token: 0x06007080 RID: 28800 RVA: 0x002A7888 File Offset: 0x002A5C88
		public Antialiasing()
		{
		}

		// Token: 0x06007081 RID: 28801 RVA: 0x002A78DC File Offset: 0x002A5CDC
		public Material CurrentAAMaterial()
		{
			Material result;
			switch (this.mode)
			{
			case AAMode.FXAA2:
				result = this.materialFXAAII;
				break;
			case AAMode.FXAA3Console:
				result = this.materialFXAAIII;
				break;
			case AAMode.FXAA1PresetA:
				result = this.materialFXAAPreset2;
				break;
			case AAMode.FXAA1PresetB:
				result = this.materialFXAAPreset3;
				break;
			case AAMode.NFAA:
				result = this.nfaa;
				break;
			case AAMode.SSAA:
				result = this.ssaa;
				break;
			case AAMode.DLAA:
				result = this.dlaa;
				break;
			default:
				result = null;
				break;
			}
			return result;
		}

		// Token: 0x06007082 RID: 28802 RVA: 0x002A7978 File Offset: 0x002A5D78
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.materialFXAAPreset2 = base.CreateMaterial(this.shaderFXAAPreset2, this.materialFXAAPreset2);
			this.materialFXAAPreset3 = base.CreateMaterial(this.shaderFXAAPreset3, this.materialFXAAPreset3);
			this.materialFXAAII = base.CreateMaterial(this.shaderFXAAII, this.materialFXAAII);
			this.materialFXAAIII = base.CreateMaterial(this.shaderFXAAIII, this.materialFXAAIII);
			this.nfaa = base.CreateMaterial(this.nfaaShader, this.nfaa);
			this.ssaa = base.CreateMaterial(this.ssaaShader, this.ssaa);
			this.dlaa = base.CreateMaterial(this.dlaaShader, this.dlaa);
			if (!this.ssaaShader.isSupported)
			{
				base.NotSupported();
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06007083 RID: 28803 RVA: 0x002A7A58 File Offset: 0x002A5E58
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (this.mode == AAMode.FXAA3Console && this.materialFXAAIII != null)
			{
				this.materialFXAAIII.SetFloat("_EdgeThresholdMin", this.edgeThresholdMin);
				this.materialFXAAIII.SetFloat("_EdgeThreshold", this.edgeThreshold);
				this.materialFXAAIII.SetFloat("_EdgeSharpness", this.edgeSharpness);
				Graphics.Blit(source, destination, this.materialFXAAIII);
			}
			else if (this.mode == AAMode.FXAA1PresetB && this.materialFXAAPreset3 != null)
			{
				Graphics.Blit(source, destination, this.materialFXAAPreset3);
			}
			else if (this.mode == AAMode.FXAA1PresetA && this.materialFXAAPreset2 != null)
			{
				source.anisoLevel = 4;
				Graphics.Blit(source, destination, this.materialFXAAPreset2);
				source.anisoLevel = 0;
			}
			else if (this.mode == AAMode.FXAA2 && this.materialFXAAII != null)
			{
				Graphics.Blit(source, destination, this.materialFXAAII);
			}
			else if (this.mode == AAMode.SSAA && this.ssaa != null)
			{
				Graphics.Blit(source, destination, this.ssaa);
			}
			else if (this.mode == AAMode.DLAA && this.dlaa != null)
			{
				source.anisoLevel = 0;
				RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height);
				Graphics.Blit(source, temporary, this.dlaa, 0);
				Graphics.Blit(temporary, destination, this.dlaa, (!this.dlaaSharp) ? 1 : 2);
				RenderTexture.ReleaseTemporary(temporary);
			}
			else if (this.mode == AAMode.NFAA && this.nfaa != null)
			{
				source.anisoLevel = 0;
				this.nfaa.SetFloat("_OffsetScale", this.offsetScale);
				this.nfaa.SetFloat("_BlurRadius", this.blurRadius);
				Graphics.Blit(source, destination, this.nfaa, (!this.showGeneratedNormals) ? 0 : 1);
			}
			else
			{
				Graphics.Blit(source, destination);
			}
		}

		// Token: 0x040062E4 RID: 25316
		public AAMode mode = AAMode.FXAA3Console;

		// Token: 0x040062E5 RID: 25317
		public bool showGeneratedNormals;

		// Token: 0x040062E6 RID: 25318
		public float offsetScale = 0.2f;

		// Token: 0x040062E7 RID: 25319
		public float blurRadius = 18f;

		// Token: 0x040062E8 RID: 25320
		public float edgeThresholdMin = 0.05f;

		// Token: 0x040062E9 RID: 25321
		public float edgeThreshold = 0.2f;

		// Token: 0x040062EA RID: 25322
		public float edgeSharpness = 4f;

		// Token: 0x040062EB RID: 25323
		public bool dlaaSharp;

		// Token: 0x040062EC RID: 25324
		public Shader ssaaShader;

		// Token: 0x040062ED RID: 25325
		private Material ssaa;

		// Token: 0x040062EE RID: 25326
		public Shader dlaaShader;

		// Token: 0x040062EF RID: 25327
		private Material dlaa;

		// Token: 0x040062F0 RID: 25328
		public Shader nfaaShader;

		// Token: 0x040062F1 RID: 25329
		private Material nfaa;

		// Token: 0x040062F2 RID: 25330
		public Shader shaderFXAAPreset2;

		// Token: 0x040062F3 RID: 25331
		private Material materialFXAAPreset2;

		// Token: 0x040062F4 RID: 25332
		public Shader shaderFXAAPreset3;

		// Token: 0x040062F5 RID: 25333
		private Material materialFXAAPreset3;

		// Token: 0x040062F6 RID: 25334
		public Shader shaderFXAAII;

		// Token: 0x040062F7 RID: 25335
		private Material materialFXAAII;

		// Token: 0x040062F8 RID: 25336
		public Shader shaderFXAAIII;

		// Token: 0x040062F9 RID: 25337
		private Material materialFXAAIII;
	}
}
