using System;
using UnityEngine;

namespace MK.Glow
{
	// Token: 0x02000002 RID: 2
	[ExecuteInEditMode]
	[ImageEffectAllowedInSceneView]
	[RequireComponent(typeof(Camera))]
	public class MKGlow : MonoBehaviour
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000450
		public MKGlow()
		{
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000002 RID: 2 RVA: 0x000020E8 File Offset: 0x000004E8
		[SerializeField]
		private Camera Cam
		{
			get
			{
				return base.GetComponent<Camera>();
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000003 RID: 3 RVA: 0x000020F0 File Offset: 0x000004F0
		// (set) Token: 0x06000004 RID: 4 RVA: 0x000020F8 File Offset: 0x000004F8
		public bool UseLens
		{
			get
			{
				return this.useLens;
			}
			set
			{
				this.useLens = value;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x06000005 RID: 5 RVA: 0x00002101 File Offset: 0x00000501
		// (set) Token: 0x06000006 RID: 6 RVA: 0x00002109 File Offset: 0x00000509
		public Texture LensTex
		{
			get
			{
				return this.lensTex;
			}
			set
			{
				this.lensTex = value;
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000007 RID: 7 RVA: 0x00002112 File Offset: 0x00000512
		// (set) Token: 0x06000008 RID: 8 RVA: 0x0000211A File Offset: 0x0000051A
		public float LensIntensity
		{
			get
			{
				return this.lensIntensity;
			}
			set
			{
				this.lensIntensity = value;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000009 RID: 9 RVA: 0x00002123 File Offset: 0x00000523
		// (set) Token: 0x0600000A RID: 10 RVA: 0x0000212B File Offset: 0x0000052B
		public LayerMask GlowLayer
		{
			get
			{
				return this.glowLayer;
			}
			set
			{
				this.glowLayer = value;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x0600000B RID: 11 RVA: 0x00002134 File Offset: 0x00000534
		// (set) Token: 0x0600000C RID: 12 RVA: 0x0000213C File Offset: 0x0000053C
		public GlowType GlowType
		{
			get
			{
				return this.glowType;
			}
			set
			{
				this.glowType = value;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600000D RID: 13 RVA: 0x00002145 File Offset: 0x00000545
		// (set) Token: 0x0600000E RID: 14 RVA: 0x0000214D File Offset: 0x0000054D
		public Color GlowTint
		{
			get
			{
				return this.glowTint;
			}
			set
			{
				this.glowTint = value;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600000F RID: 15 RVA: 0x00002156 File Offset: 0x00000556
		// (set) Token: 0x06000010 RID: 16 RVA: 0x0000215E File Offset: 0x0000055E
		public float Samples
		{
			get
			{
				return this.samples;
			}
			set
			{
				this.samples = value;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000011 RID: 17 RVA: 0x00002167 File Offset: 0x00000567
		// (set) Token: 0x06000012 RID: 18 RVA: 0x0000216F File Offset: 0x0000056F
		public int BlurIterations
		{
			get
			{
				return this.blurIterations;
			}
			set
			{
				this.blurIterations = Mathf.Clamp(value, 0, 10);
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000013 RID: 19 RVA: 0x00002180 File Offset: 0x00000580
		// (set) Token: 0x06000014 RID: 20 RVA: 0x00002188 File Offset: 0x00000588
		public float GlowIntensityInner
		{
			get
			{
				return this.glowIntensityInner;
			}
			set
			{
				this.glowIntensityInner = value;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000015 RID: 21 RVA: 0x00002191 File Offset: 0x00000591
		// (set) Token: 0x06000016 RID: 22 RVA: 0x00002199 File Offset: 0x00000599
		public float GlowIntensityOuter
		{
			get
			{
				return this.glowIntensityOuter;
			}
			set
			{
				this.glowIntensityOuter = value;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x06000017 RID: 23 RVA: 0x000021A2 File Offset: 0x000005A2
		// (set) Token: 0x06000018 RID: 24 RVA: 0x000021AA File Offset: 0x000005AA
		public float BlurSpreadInner
		{
			get
			{
				return this.blurSpreadInner;
			}
			set
			{
				this.blurSpreadInner = value;
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000019 RID: 25 RVA: 0x000021B3 File Offset: 0x000005B3
		// (set) Token: 0x0600001A RID: 26 RVA: 0x000021BB File Offset: 0x000005BB
		public float BlurSpreadOuter
		{
			get
			{
				return this.blurSpreadOuter;
			}
			set
			{
				this.blurSpreadOuter = value;
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600001B RID: 27 RVA: 0x000021C4 File Offset: 0x000005C4
		// (set) Token: 0x0600001C RID: 28 RVA: 0x000021CC File Offset: 0x000005CC
		public float Threshold
		{
			get
			{
				return this.threshold;
			}
			set
			{
				this.threshold = value;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600001D RID: 29 RVA: 0x000021D5 File Offset: 0x000005D5
		// (set) Token: 0x0600001E RID: 30 RVA: 0x000021DD File Offset: 0x000005DD
		public int AntiAliasing
		{
			get
			{
				return this.antiAliasing;
			}
			set
			{
				this.antiAliasing = value;
			}
		}

		// Token: 0x0600001F RID: 31 RVA: 0x000021E6 File Offset: 0x000005E6
		private void Awake()
		{
			this.GlowInitialize();
		}

		// Token: 0x06000020 RID: 32 RVA: 0x000021EE File Offset: 0x000005EE
		private void Reset()
		{
			this.GlowInitialize();
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000021F6 File Offset: 0x000005F6
		private void GlowInitialize()
		{
			this.Cleanup();
			this.SetupShaders();
			this.SetupMaterials();
		}

		// Token: 0x06000022 RID: 34 RVA: 0x0000220C File Offset: 0x0000060C
		private void SetupShaders()
		{
			if (!this.blurShader)
			{
				this.blurShader = Shader.Find("Hidden/MK/Glow/Blur");
			}
			if (!this.compositeShader)
			{
				this.compositeShader = Shader.Find("Hidden/MK/Glow/Composite");
			}
			if (!this.selectiveRenderShader)
			{
				this.selectiveRenderShader = Shader.Find("Hidden/MK/Glow/SelectiveRender");
			}
			if (!this.extractLuminanceShader)
			{
				this.extractLuminanceShader = Shader.Find("Hidden/MK/Glow/ExtractLuminance");
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002299 File Offset: 0x00000699
		private void Cleanup()
		{
			UnityEngine.Object.DestroyImmediate(this.selectiveGlowCamera);
			UnityEngine.Object.DestroyImmediate(this.SelectiveGlowCameraObject);
		}

		// Token: 0x06000024 RID: 36 RVA: 0x000022B1 File Offset: 0x000006B1
		private void OnEnable()
		{
			this.GlowInitialize();
		}

		// Token: 0x06000025 RID: 37 RVA: 0x000022B9 File Offset: 0x000006B9
		private void OnDisable()
		{
			this.Cleanup();
		}

		// Token: 0x06000026 RID: 38 RVA: 0x000022C1 File Offset: 0x000006C1
		private void OnDestroy()
		{
			this.Cleanup();
		}

		// Token: 0x06000027 RID: 39 RVA: 0x000022C9 File Offset: 0x000006C9
		private RenderTexture GetTemporaryRT(int width, int height, VRTextureUsage vrUsage)
		{
			return RenderTexture.GetTemporary(width, height, 0, this.rtFormat, RenderTextureReadWrite.Default, 1, RenderTextureMemoryless.None, vrUsage);
		}

		// Token: 0x06000028 RID: 40 RVA: 0x000022E0 File Offset: 0x000006E0
		private void Blur(RenderTexture main, RenderTexture tmpMain, RenderTexture sec, RenderTexture tmpSec)
		{
			for (int i = 1; i <= this.blurIterations; i++)
			{
				float num = (float)i * (this.blurSpreadInner * 10f) / (float)this.blurIterations / this.samples;
				num *= MKGlow.gaussFilter[i];
				this.blurMaterial.SetFloat("_Offset", num);
				Graphics.Blit(main, tmpMain, this.blurMaterial);
				this.blurMaterial.SetFloat("_Offset", num);
				Graphics.Blit(tmpMain, main, this.blurMaterial);
				if (this.blurSpreadOuter > 0f || this.glowIntensityOuter > 0f)
				{
					float num2 = (float)i * (this.blurSpreadOuter * 50f) / (float)this.blurIterations / this.samples;
					num2 *= MKGlow.gaussFilter[i];
					this.blurMaterial.SetFloat("_Offset", num2);
					Graphics.Blit(sec, tmpSec, this.blurMaterial);
					this.blurMaterial.SetFloat("_Offset", num2);
					Graphics.Blit(tmpSec, sec, this.blurMaterial);
				}
			}
		}

		// Token: 0x06000029 RID: 41 RVA: 0x000023F0 File Offset: 0x000007F0
		private void Blur(RenderTexture main, RenderTexture tmpMain)
		{
			for (int i = 1; i <= this.blurIterations; i++)
			{
				float num = (float)i * (this.blurSpreadInner * 10f) / (float)this.blurIterations / this.samples;
				num *= MKGlow.gaussFilter[i];
				this.blurMaterial.SetFloat("_Offset", num);
				Graphics.Blit(main, tmpMain, this.blurMaterial);
				this.blurMaterial.SetFloat("_Offset", num);
				Graphics.Blit(tmpMain, main, this.blurMaterial);
			}
		}

		// Token: 0x0600002A RID: 42 RVA: 0x0000247C File Offset: 0x0000087C
		private void SetupMaterials()
		{
			if (this.blurMaterial == null)
			{
				this.blurMaterial = new Material(this.blurShader);
				this.blurMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			if (this.extractLuminanceMaterial == null)
			{
				this.extractLuminanceMaterial = new Material(this.extractLuminanceShader);
				this.extractLuminanceMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
			if (this.compositeMaterial == null)
			{
				this.compositeMaterial = new Material(this.compositeShader);
				this.compositeMaterial.hideFlags = HideFlags.HideAndDontSave;
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00002518 File Offset: 0x00000918
		private GameObject SelectiveGlowCameraObject
		{
			get
			{
				if (!this.selectiveGlowCameraObject)
				{
					this.selectiveGlowCameraObject = new GameObject("selectiveGlowCameraObject");
					this.selectiveGlowCameraObject.AddComponent<Camera>();
					this.selectiveGlowCameraObject.hideFlags = HideFlags.HideAndDontSave;
					this.SelectiveGlowCamera.orthographic = false;
					this.SelectiveGlowCamera.enabled = false;
					this.SelectiveGlowCamera.renderingPath = RenderingPath.VertexLit;
					this.SelectiveGlowCamera.hideFlags = HideFlags.HideAndDontSave;
				}
				return this.selectiveGlowCameraObject;
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x0600002C RID: 44 RVA: 0x00002595 File Offset: 0x00000995
		private Camera SelectiveGlowCamera
		{
			get
			{
				if (this.selectiveGlowCamera == null)
				{
					this.selectiveGlowCamera = this.SelectiveGlowCameraObject.GetComponent<Camera>();
				}
				return this.selectiveGlowCamera;
			}
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000025C0 File Offset: 0x000009C0
		private void SetupGlowCamera()
		{
			this.SelectiveGlowCamera.CopyFrom(this.Cam);
			this.SelectiveGlowCamera.depthTextureMode = DepthTextureMode.None;
			this.SelectiveGlowCamera.targetTexture = this.glowTexRaw;
			this.SelectiveGlowCamera.clearFlags = CameraClearFlags.Color;
			this.SelectiveGlowCamera.rect = new Rect(0f, 0f, 1f, 1f);
			this.SelectiveGlowCamera.backgroundColor = new Color(0f, 0f, 0f, 0f);
			this.SelectiveGlowCamera.cullingMask = this.glowLayer;
			this.SelectiveGlowCamera.renderingPath = RenderingPath.VertexLit;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002674 File Offset: 0x00000A74
		private void LuminanceGlow(RenderTexture src, RenderTexture dest, RenderTexture glowTexInner, RenderTexture tmpGlowTexInner, RenderTexture glowTexOuter, RenderTexture tmpGlowTexOuter)
		{
			Graphics.Blit(src, glowTexInner, this.extractLuminanceMaterial);
			Graphics.Blit(glowTexInner, glowTexOuter);
			this.Blur(glowTexInner, tmpGlowTexInner, glowTexOuter, tmpGlowTexOuter);
			this.compositeMaterial.SetTexture("_MKGlowTexInner", glowTexInner);
			this.compositeMaterial.SetTexture("_MKGlowTexOuter", glowTexOuter);
			Graphics.Blit(src, dest, this.compositeMaterial, 0);
		}

		// Token: 0x0600002F RID: 47 RVA: 0x000026D4 File Offset: 0x00000AD4
		private void FullScreenGlow(RenderTexture src, RenderTexture dest, RenderTexture glowTexInner, RenderTexture tmpGlowTexInner)
		{
			Graphics.Blit(src, glowTexInner);
			this.Blur(glowTexInner, tmpGlowTexInner);
			this.compositeMaterial.SetTexture("_MKGlowTexInner", glowTexInner);
			Graphics.Blit(src, dest, this.compositeMaterial, 1);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002708 File Offset: 0x00000B08
		private void SelectiveGlow(RenderTexture src, RenderTexture dest, RenderTexture glowTexInner, RenderTexture tmpGlowTexInner, RenderTexture glowTexOuter, RenderTexture tmpGlowTexOuter)
		{
			Graphics.Blit(this.glowTexRaw, glowTexInner);
			Graphics.Blit(this.glowTexRaw, glowTexOuter);
			this.Blur(glowTexInner, tmpGlowTexInner, glowTexOuter, tmpGlowTexOuter);
			this.compositeMaterial.SetTexture("_MKGlowTexInner", glowTexInner);
			this.compositeMaterial.SetTexture("_MKGlowTexOuter", glowTexOuter);
			Graphics.Blit(src, dest, this.compositeMaterial, 0);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x0000276C File Offset: 0x00000B6C
		private void SelectiveGlowFast(RenderTexture src, RenderTexture dest, RenderTexture glowTexInner, RenderTexture tmpGlowTexInner)
		{
			Graphics.Blit(this.glowTexRaw, glowTexInner);
			this.Blur(glowTexInner, tmpGlowTexInner, null, null);
			this.compositeMaterial.SetTexture("_MKGlowTexInner", glowTexInner);
			Graphics.Blit(src, dest, this.compositeMaterial, 1);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x000027A4 File Offset: 0x00000BA4
		private void OnPostRender()
		{
			switch (this.glowType)
			{
			case GlowType.Selective:
			case GlowType.SelectiveFast:
				RenderTexture.ReleaseTemporary(this.glowTexRaw);
				this.glowTexRaw = RenderTexture.GetTemporary((int)((float)this.Cam.pixelWidth / this.samples), (int)((float)this.Cam.pixelHeight / this.samples), 24, this.rtFormat, RenderTextureReadWrite.Default, this.antiAliasing, RenderTextureMemoryless.None, this.srcVRUsage);
				this.SetupGlowCamera();
				this.SelectiveGlowCamera.RenderWithShader(this.selectiveRenderShader, "RenderType");
				break;
			case GlowType.Luminance:
				this.extractLuminanceMaterial.SetFloat("_Threshold", this.threshold);
				break;
			}
			this.blurMaterial.SetFloat("_VRMult", (!this.Cam.stereoEnabled) ? 1f : 0.5f);
			this.compositeMaterial.SetFloat("_GlowIntensityInner", this.glowIntensityInner * ((this.glowType == GlowType.Fullscreen) ? 10f : (25f * this.blurSpreadInner)));
			this.compositeMaterial.SetFloat("_GlowIntensityOuter", this.glowIntensityOuter * this.BlurSpreadOuter * 50f);
			this.compositeMaterial.SetColor("_GlowTint", this.glowTint);
			if (this.glowType == GlowType.SelectiveFast)
			{
				MKGlow.SetKeyword(false, "_MK_Outer", this.compositeMaterial);
			}
			else
			{
				MKGlow.SetKeyword(true, "_MK_Outer", this.compositeMaterial);
			}
			this.UpdateLensUsage();
			if (this.useLens)
			{
				this.compositeMaterial.SetTexture("_LensTex", this.lensTex);
				this.compositeMaterial.SetFloat("_LensIntensity", this.lensIntensity);
			}
		}

		// Token: 0x06000033 RID: 51 RVA: 0x0000297C File Offset: 0x00000D7C
		private void OnRenderImage(RenderTexture src, RenderTexture dest)
		{
			this.rtFormat = src.format;
			float num = (float)src.width;
			float num2 = (float)src.height;
			this.srcWidth = (int)(num / this.samples);
			this.srcHeight = (int)(num2 / this.samples);
			this.srcVRUsage = src.vrUsage;
			RenderTexture temporaryRT = this.GetTemporaryRT(this.srcWidth, this.srcHeight, src.vrUsage);
			RenderTexture temporaryRT2 = this.GetTemporaryRT(this.srcWidth, this.srcHeight, src.vrUsage);
			RenderTexture renderTexture = null;
			RenderTexture renderTexture2 = null;
			if (this.glowType == GlowType.Luminance || this.glowType == GlowType.Selective)
			{
				renderTexture = this.GetTemporaryRT(this.srcWidth / 2, this.srcHeight / 2, src.vrUsage);
				renderTexture2 = this.GetTemporaryRT(this.srcWidth / 2, this.srcHeight / 2, src.vrUsage);
			}
			switch (this.glowType)
			{
			case GlowType.Selective:
				this.SelectiveGlow(src, dest, temporaryRT, temporaryRT2, renderTexture, renderTexture2);
				break;
			case GlowType.Fullscreen:
				this.FullScreenGlow(src, dest, temporaryRT, temporaryRT2);
				break;
			case GlowType.Luminance:
				this.LuminanceGlow(src, dest, temporaryRT, temporaryRT2, renderTexture, renderTexture2);
				break;
			case GlowType.SelectiveFast:
				this.SelectiveGlowFast(src, dest, temporaryRT, temporaryRT2);
				break;
			}
			if (this.glowType == GlowType.Luminance || this.glowType == GlowType.Selective)
			{
				RenderTexture.ReleaseTemporary(renderTexture);
				RenderTexture.ReleaseTemporary(renderTexture2);
			}
			RenderTexture.ReleaseTemporary(temporaryRT);
			RenderTexture.ReleaseTemporary(temporaryRT2);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00002AF3 File Offset: 0x00000EF3
		private void UpdateLensUsage()
		{
			if (this.useLens && this.glowType != GlowType.Fullscreen)
			{
				this.EnableLens(true);
			}
			else
			{
				this.EnableLens(false);
			}
		}

		// Token: 0x06000035 RID: 53 RVA: 0x00002B1F File Offset: 0x00000F1F
		private void EnableLens(bool use)
		{
			if (use)
			{
				MKGlow.SetKeyword(true, "_MK_LENS", this.compositeMaterial);
			}
			else
			{
				MKGlow.SetKeyword(false, "_MK_LENS", this.compositeMaterial);
			}
		}

		// Token: 0x06000036 RID: 54 RVA: 0x00002B4E File Offset: 0x00000F4E
		private static void SetKeyword(bool enable, string keyword, Material mat)
		{
			if (enable)
			{
				if (!mat.IsKeywordEnabled(keyword))
				{
					mat.EnableKeyword(keyword);
				}
			}
			else if (mat.IsKeywordEnabled(keyword))
			{
				mat.DisableKeyword(keyword);
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x00002B81 File Offset: 0x00000F81
		// Note: this type is marked as 'beforefieldinit'.
		static MKGlow()
		{
		}

		// Token: 0x04000001 RID: 1
		private static float[] gaussFilter = new float[]
		{
			0.5f,
			0.5398f,
			0.5793f,
			0.6179f,
			0.6554f,
			0.6915f,
			0.7257f,
			0.758f,
			0.7881f,
			0.8159f,
			0.8413f
		};

		// Token: 0x04000002 RID: 2
		private const float GLOW_INTENSITY_INNER_MULT = 25f;

		// Token: 0x04000003 RID: 3
		private const float GLOW_INTENSITY_OUTER_MULT = 50f;

		// Token: 0x04000004 RID: 4
		private const float BLUR_SPREAD_INNTER_MULT = 10f;

		// Token: 0x04000005 RID: 5
		private const float BLUR_SPREAD_OUTER_MULT = 50f;

		// Token: 0x04000006 RID: 6
		private const string LENS_KEYWORD = "_MK_LENS";

		// Token: 0x04000007 RID: 7
		private RenderTextureFormat rtFormat = RenderTextureFormat.Default;

		// Token: 0x04000008 RID: 8
		[SerializeField]
		private Shader blurShader;

		// Token: 0x04000009 RID: 9
		[SerializeField]
		private Shader compositeShader;

		// Token: 0x0400000A RID: 10
		[SerializeField]
		private Shader selectiveRenderShader;

		// Token: 0x0400000B RID: 11
		[SerializeField]
		private Shader extractLuminanceShader;

		// Token: 0x0400000C RID: 12
		private Material compositeMaterial;

		// Token: 0x0400000D RID: 13
		private Material blurMaterial;

		// Token: 0x0400000E RID: 14
		private Material extractLuminanceMaterial;

		// Token: 0x0400000F RID: 15
		[SerializeField]
		[Tooltip("Lens Texture")]
		private Texture lensTex;

		// Token: 0x04000010 RID: 16
		[SerializeField]
		[Tooltip("Lens Intensity")]
		private float lensIntensity;

		// Token: 0x04000011 RID: 17
		[SerializeField]
		[Tooltip("recommend: -1")]
		private LayerMask glowLayer = -1;

		// Token: 0x04000012 RID: 18
		[SerializeField]
		[Tooltip("Selective = to specifically bring objects to glow, Fullscreen = complete screen glows")]
		private GlowType glowType = GlowType.Luminance;

		// Token: 0x04000013 RID: 19
		[SerializeField]
		[Tooltip("The glows coloration")]
		private Color glowTint = new Color(1f, 1f, 1f, 0f);

		// Token: 0x04000014 RID: 20
		[SerializeField]
		[Tooltip("Inner width of the glow effect")]
		private float blurSpreadInner = 0.6f;

		// Token: 0x04000015 RID: 21
		[SerializeField]
		[Tooltip("Outer width of the glow effect")]
		private float blurSpreadOuter = 0.7f;

		// Token: 0x04000016 RID: 22
		[SerializeField]
		[Tooltip("Number of used blurs. Lower iterations = better performance")]
		private int blurIterations = 5;

		// Token: 0x04000017 RID: 23
		[SerializeField]
		[Tooltip("The global inner luminous intensity")]
		private float glowIntensityInner = 0.4f;

		// Token: 0x04000018 RID: 24
		[SerializeField]
		[Tooltip("The global outer luminous intensity")]
		private float glowIntensityOuter;

		// Token: 0x04000019 RID: 25
		[SerializeField]
		[Tooltip("Downsampling steps of the blur. Higher samples = better performance, but gains more flickering")]
		private float samples = 2f;

		// Token: 0x0400001A RID: 26
		[SerializeField]
		[Tooltip("Threshold for glow")]
		private float threshold = 1f;

		// Token: 0x0400001B RID: 27
		[SerializeField]
		[Tooltip("Enable and disable the lens")]
		private bool useLens;

		// Token: 0x0400001C RID: 28
		[SerializeField]
		private int antiAliasing = 1;

		// Token: 0x0400001D RID: 29
		private Camera selectiveGlowCamera;

		// Token: 0x0400001E RID: 30
		private GameObject selectiveGlowCameraObject;

		// Token: 0x0400001F RID: 31
		private RenderTexture glowTexRaw;

		// Token: 0x04000020 RID: 32
		private int srcWidth;

		// Token: 0x04000021 RID: 33
		private int srcHeight;

		// Token: 0x04000022 RID: 34
		private VRTextureUsage srcVRUsage = VRTextureUsage.TwoEyes;
	}
}
