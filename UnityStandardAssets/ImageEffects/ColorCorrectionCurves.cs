using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000E6B RID: 3691
	[ExecuteInEditMode]
	[AddComponentMenu("Image Effects/Color Adjustments/Color Correction (Curves, Saturation)")]
	public class ColorCorrectionCurves : PostEffectsBase
	{
		// Token: 0x060070AF RID: 28847 RVA: 0x002AA77C File Offset: 0x002A8B7C
		public ColorCorrectionCurves()
		{
		}

		// Token: 0x060070B0 RID: 28848 RVA: 0x002AA9AF File Offset: 0x002A8DAF
		private new void Start()
		{
			base.Start();
			this.updateTexturesOnStartup = true;
		}

		// Token: 0x060070B1 RID: 28849 RVA: 0x002AA9BE File Offset: 0x002A8DBE
		private void Awake()
		{
		}

		// Token: 0x060070B2 RID: 28850 RVA: 0x002AA9C0 File Offset: 0x002A8DC0
		public override bool CheckResources()
		{
			base.CheckSupport(this.mode == ColorCorrectionCurves.ColorCorrectionMode.Advanced);
			this.ccMaterial = base.CheckShaderAndCreateMaterial(this.simpleColorCorrectionCurvesShader, this.ccMaterial);
			this.ccDepthMaterial = base.CheckShaderAndCreateMaterial(this.colorCorrectionCurvesShader, this.ccDepthMaterial);
			this.selectiveCcMaterial = base.CheckShaderAndCreateMaterial(this.colorCorrectionSelectiveShader, this.selectiveCcMaterial);
			if (!this.rgbChannelTex)
			{
				this.rgbChannelTex = new Texture2D(256, 4, TextureFormat.ARGB32, false, true);
			}
			if (!this.rgbDepthChannelTex)
			{
				this.rgbDepthChannelTex = new Texture2D(256, 4, TextureFormat.ARGB32, false, true);
			}
			if (!this.zCurveTex)
			{
				this.zCurveTex = new Texture2D(256, 1, TextureFormat.ARGB32, false, true);
			}
			this.rgbChannelTex.hideFlags = HideFlags.DontSave;
			this.rgbDepthChannelTex.hideFlags = HideFlags.DontSave;
			this.zCurveTex.hideFlags = HideFlags.DontSave;
			this.rgbChannelTex.wrapMode = TextureWrapMode.Clamp;
			this.rgbDepthChannelTex.wrapMode = TextureWrapMode.Clamp;
			this.zCurveTex.wrapMode = TextureWrapMode.Clamp;
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x060070B3 RID: 28851 RVA: 0x002AAAF4 File Offset: 0x002A8EF4
		public void UpdateParameters()
		{
			this.CheckResources();
			if (this.redChannel != null && this.greenChannel != null && this.blueChannel != null)
			{
				for (float num = 0f; num <= 1f; num += 0.003921569f)
				{
					float num2 = Mathf.Clamp(this.redChannel.Evaluate(num), 0f, 1f);
					float num3 = Mathf.Clamp(this.greenChannel.Evaluate(num), 0f, 1f);
					float num4 = Mathf.Clamp(this.blueChannel.Evaluate(num), 0f, 1f);
					this.rgbChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 0, new Color(num2, num2, num2));
					this.rgbChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 1, new Color(num3, num3, num3));
					this.rgbChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 2, new Color(num4, num4, num4));
					float num5 = Mathf.Clamp(this.zCurve.Evaluate(num), 0f, 1f);
					this.zCurveTex.SetPixel((int)Mathf.Floor(num * 255f), 0, new Color(num5, num5, num5));
					num2 = Mathf.Clamp(this.depthRedChannel.Evaluate(num), 0f, 1f);
					num3 = Mathf.Clamp(this.depthGreenChannel.Evaluate(num), 0f, 1f);
					num4 = Mathf.Clamp(this.depthBlueChannel.Evaluate(num), 0f, 1f);
					this.rgbDepthChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 0, new Color(num2, num2, num2));
					this.rgbDepthChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 1, new Color(num3, num3, num3));
					this.rgbDepthChannelTex.SetPixel((int)Mathf.Floor(num * 255f), 2, new Color(num4, num4, num4));
				}
				this.rgbChannelTex.Apply();
				this.rgbDepthChannelTex.Apply();
				this.zCurveTex.Apply();
			}
		}

		// Token: 0x060070B4 RID: 28852 RVA: 0x002AAD17 File Offset: 0x002A9117
		private void UpdateTextures()
		{
			this.UpdateParameters();
		}

		// Token: 0x060070B5 RID: 28853 RVA: 0x002AAD20 File Offset: 0x002A9120
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (this.updateTexturesOnStartup)
			{
				this.UpdateParameters();
				this.updateTexturesOnStartup = false;
			}
			if (this.useDepthCorrection)
			{
				base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
			}
			RenderTexture renderTexture = destination;
			if (this.selectiveCc)
			{
				renderTexture = RenderTexture.GetTemporary(source.width, source.height);
			}
			if (this.useDepthCorrection)
			{
				this.ccDepthMaterial.SetTexture("_RgbTex", this.rgbChannelTex);
				this.ccDepthMaterial.SetTexture("_ZCurve", this.zCurveTex);
				this.ccDepthMaterial.SetTexture("_RgbDepthTex", this.rgbDepthChannelTex);
				this.ccDepthMaterial.SetFloat("_Saturation", this.saturation);
				Graphics.Blit(source, renderTexture, this.ccDepthMaterial);
			}
			else
			{
				this.ccMaterial.SetTexture("_RgbTex", this.rgbChannelTex);
				this.ccMaterial.SetFloat("_Saturation", this.saturation);
				Graphics.Blit(source, renderTexture, this.ccMaterial);
			}
			if (this.selectiveCc)
			{
				this.selectiveCcMaterial.SetColor("selColor", this.selectiveFromColor);
				this.selectiveCcMaterial.SetColor("targetColor", this.selectiveToColor);
				Graphics.Blit(renderTexture, destination, this.selectiveCcMaterial);
				RenderTexture.ReleaseTemporary(renderTexture);
			}
		}

		// Token: 0x0400639A RID: 25498
		public AnimationCurve redChannel = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x0400639B RID: 25499
		public AnimationCurve greenChannel = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x0400639C RID: 25500
		public AnimationCurve blueChannel = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x0400639D RID: 25501
		public bool useDepthCorrection;

		// Token: 0x0400639E RID: 25502
		public AnimationCurve zCurve = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x0400639F RID: 25503
		public AnimationCurve depthRedChannel = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x040063A0 RID: 25504
		public AnimationCurve depthGreenChannel = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x040063A1 RID: 25505
		public AnimationCurve depthBlueChannel = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f),
			new Keyframe(1f, 1f)
		});

		// Token: 0x040063A2 RID: 25506
		private Material ccMaterial;

		// Token: 0x040063A3 RID: 25507
		private Material ccDepthMaterial;

		// Token: 0x040063A4 RID: 25508
		private Material selectiveCcMaterial;

		// Token: 0x040063A5 RID: 25509
		private Texture2D rgbChannelTex;

		// Token: 0x040063A6 RID: 25510
		private Texture2D rgbDepthChannelTex;

		// Token: 0x040063A7 RID: 25511
		private Texture2D zCurveTex;

		// Token: 0x040063A8 RID: 25512
		public float saturation = 1f;

		// Token: 0x040063A9 RID: 25513
		public bool selectiveCc;

		// Token: 0x040063AA RID: 25514
		public Color selectiveFromColor = Color.white;

		// Token: 0x040063AB RID: 25515
		public Color selectiveToColor = Color.white;

		// Token: 0x040063AC RID: 25516
		public ColorCorrectionCurves.ColorCorrectionMode mode;

		// Token: 0x040063AD RID: 25517
		public bool updateTextures = true;

		// Token: 0x040063AE RID: 25518
		public Shader colorCorrectionCurvesShader;

		// Token: 0x040063AF RID: 25519
		public Shader simpleColorCorrectionCurvesShader;

		// Token: 0x040063B0 RID: 25520
		public Shader colorCorrectionSelectiveShader;

		// Token: 0x040063B1 RID: 25521
		private bool updateTexturesOnStartup = true;

		// Token: 0x02000E6C RID: 3692
		public enum ColorCorrectionMode
		{
			// Token: 0x040063B3 RID: 25523
			Simple,
			// Token: 0x040063B4 RID: 25524
			Advanced
		}
	}
}
