using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000E7C RID: 3708
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Displacement/Fisheye")]
	public class Fisheye : PostEffectsBase
	{
		// Token: 0x060070F0 RID: 28912 RVA: 0x002ADD74 File Offset: 0x002AC174
		public Fisheye()
		{
		}

		// Token: 0x060070F1 RID: 28913 RVA: 0x002ADD92 File Offset: 0x002AC192
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.fisheyeMaterial = base.CheckShaderAndCreateMaterial(this.fishEyeShader, this.fisheyeMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060070F2 RID: 28914 RVA: 0x002ADDCC File Offset: 0x002AC1CC
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			float num = 0.15625f;
			float num2 = (float)source.width * 1f / ((float)source.height * 1f);
			this.fisheyeMaterial.SetVector("intensity", new Vector4(this.strengthX * num2 * num, this.strengthY * num, this.strengthX * num2 * num, this.strengthY * num));
			Graphics.Blit(source, destination, this.fisheyeMaterial);
		}

		// Token: 0x04006442 RID: 25666
		[Range(0f, 1.5f)]
		public float strengthX = 0.05f;

		// Token: 0x04006443 RID: 25667
		[Range(0f, 1.5f)]
		public float strengthY = 0.05f;

		// Token: 0x04006444 RID: 25668
		public Shader fishEyeShader;

		// Token: 0x04006445 RID: 25669
		private Material fisheyeMaterial;
	}
}
