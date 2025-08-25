using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000E90 RID: 3728
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Camera/Tilt Shift (Lens Blur)")]
	internal class TiltShift : PostEffectsBase
	{
		// Token: 0x0600713E RID: 28990 RVA: 0x002B05E0 File Offset: 0x002AE9E0
		public TiltShift()
		{
		}

		// Token: 0x0600713F RID: 28991 RVA: 0x002B0605 File Offset: 0x002AEA05
		public override bool CheckResources()
		{
			base.CheckSupport(false);
			this.tiltShiftMaterial = base.CheckShaderAndCreateMaterial(this.tiltShiftShader, this.tiltShiftMaterial);
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06007140 RID: 28992 RVA: 0x002B0640 File Offset: 0x002AEA40
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			this.tiltShiftMaterial.SetFloat("_BlurSize", (this.maxBlurSize >= 0f) ? this.maxBlurSize : 0f);
			this.tiltShiftMaterial.SetFloat("_BlurArea", this.blurArea);
			source.filterMode = FilterMode.Bilinear;
			RenderTexture renderTexture = destination;
			if ((float)this.downsample > 0f)
			{
				renderTexture = RenderTexture.GetTemporary(source.width >> this.downsample, source.height >> this.downsample, 0, source.format);
				renderTexture.filterMode = FilterMode.Bilinear;
			}
			int num = (int)this.quality;
			num *= 2;
			Graphics.Blit(source, renderTexture, this.tiltShiftMaterial, (this.mode != TiltShift.TiltShiftMode.TiltShiftMode) ? (num + 1) : num);
			if (this.downsample > 0)
			{
				this.tiltShiftMaterial.SetTexture("_Blurred", renderTexture);
				Graphics.Blit(source, destination, this.tiltShiftMaterial, 8);
			}
			if (renderTexture != destination)
			{
				RenderTexture.ReleaseTemporary(renderTexture);
			}
		}

		// Token: 0x040064B7 RID: 25783
		public TiltShift.TiltShiftMode mode;

		// Token: 0x040064B8 RID: 25784
		public TiltShift.TiltShiftQuality quality = TiltShift.TiltShiftQuality.Normal;

		// Token: 0x040064B9 RID: 25785
		[Range(0f, 15f)]
		public float blurArea = 1f;

		// Token: 0x040064BA RID: 25786
		[Range(0f, 25f)]
		public float maxBlurSize = 5f;

		// Token: 0x040064BB RID: 25787
		[Range(0f, 1f)]
		public int downsample;

		// Token: 0x040064BC RID: 25788
		public Shader tiltShiftShader;

		// Token: 0x040064BD RID: 25789
		private Material tiltShiftMaterial;

		// Token: 0x02000E91 RID: 3729
		public enum TiltShiftMode
		{
			// Token: 0x040064BF RID: 25791
			TiltShiftMode,
			// Token: 0x040064C0 RID: 25792
			IrisMode
		}

		// Token: 0x02000E92 RID: 3730
		public enum TiltShiftQuality
		{
			// Token: 0x040064C2 RID: 25794
			Preview,
			// Token: 0x040064C3 RID: 25795
			Low,
			// Token: 0x040064C4 RID: 25796
			Normal,
			// Token: 0x040064C5 RID: 25797
			High
		}
	}
}
