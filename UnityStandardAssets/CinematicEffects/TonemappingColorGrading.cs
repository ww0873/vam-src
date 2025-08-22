using System;
using UnityEngine;

namespace UnityStandardAssets.CinematicEffects
{
	// Token: 0x02000E4D RID: 3661
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Color Adjustments/Tonemapping and Color Grading")]
	public class TonemappingColorGrading : MonoBehaviour
	{
		// Token: 0x06007054 RID: 28756 RVA: 0x002A5ED7 File Offset: 0x002A42D7
		public TonemappingColorGrading()
		{
		}

		// Token: 0x06007055 RID: 28757 RVA: 0x002A5F0B File Offset: 0x002A430B
		public void SetDirty()
		{
			this.m_Dirty = true;
		}

		// Token: 0x17001095 RID: 4245
		// (get) Token: 0x06007056 RID: 28758 RVA: 0x002A5F14 File Offset: 0x002A4314
		// (set) Token: 0x06007057 RID: 28759 RVA: 0x002A5F1C File Offset: 0x002A431C
		public TonemappingColorGrading.FilmicCurve filmicCurve
		{
			get
			{
				return this.m_FilmicCurve;
			}
			set
			{
				this.m_FilmicCurve = value;
				this.SetDirty();
			}
		}

		// Token: 0x17001096 RID: 4246
		// (get) Token: 0x06007058 RID: 28760 RVA: 0x002A5F2B File Offset: 0x002A432B
		// (set) Token: 0x06007059 RID: 28761 RVA: 0x002A5F33 File Offset: 0x002A4333
		public TonemappingColorGrading.ColorGrading colorGrading
		{
			get
			{
				return this.m_ColorGrading;
			}
			set
			{
				this.m_ColorGrading = value;
				this.SetDirty();
			}
		}

		// Token: 0x0600705A RID: 28762 RVA: 0x002A5F42 File Offset: 0x002A4342
		private void OnValidate()
		{
			this.SetDirty();
		}

		// Token: 0x17001097 RID: 4247
		// (get) Token: 0x0600705B RID: 28763 RVA: 0x002A5F4A File Offset: 0x002A434A
		private bool isLinearColorSpace
		{
			get
			{
				return QualitySettings.activeColorSpace == ColorSpace.Linear;
			}
		}

		// Token: 0x17001098 RID: 4248
		// (get) Token: 0x0600705C RID: 28764 RVA: 0x002A5F54 File Offset: 0x002A4354
		// (set) Token: 0x0600705D RID: 28765 RVA: 0x002A5F5C File Offset: 0x002A435C
		public Texture2D userLutTexture
		{
			get
			{
				return this.m_UserLutTexture;
			}
			set
			{
				this.m_UserLutTexture = value;
				this.SetDirty();
			}
		}

		// Token: 0x17001099 RID: 4249
		// (get) Token: 0x0600705E RID: 28766 RVA: 0x002A5F6B File Offset: 0x002A436B
		public Material tonemapMaterial
		{
			get
			{
				if (this.m_TonemapMaterial == null)
				{
					this.m_TonemapMaterial = ImageEffectHelper.CheckShaderAndCreateMaterial(this.tonemapShader);
				}
				return this.m_TonemapMaterial;
			}
		}

		// Token: 0x0600705F RID: 28767 RVA: 0x002A5F98 File Offset: 0x002A4398
		protected void OnEnable()
		{
			if (this.tonemapShader == null)
			{
				this.tonemapShader = Shader.Find("Hidden/TonemappingColorGrading");
			}
			if (ImageEffectHelper.IsSupported(this.tonemapShader, false, true, this))
			{
				return;
			}
			base.enabled = false;
			Debug.LogWarning("The image effect " + this.ToString() + " has been disabled as it's not supported on the current platform.");
		}

		// Token: 0x06007060 RID: 28768 RVA: 0x002A5FFB File Offset: 0x002A43FB
		private float GetHighlightRecovery()
		{
			return Mathf.Max(0f, this.m_FilmicCurve.lutShoulder * 3f);
		}

		// Token: 0x06007061 RID: 28769 RVA: 0x002A6018 File Offset: 0x002A4418
		public float GetWhitePoint()
		{
			return Mathf.Pow(2f, Mathf.Max(0f, this.GetHighlightRecovery()));
		}

		// Token: 0x06007062 RID: 28770 RVA: 0x002A6034 File Offset: 0x002A4434
		private static float LutToLin(float x, float lutA)
		{
			x = ((x < 1f) ? x : 1f);
			float num = x / lutA;
			return num / (1f - num);
		}

		// Token: 0x06007063 RID: 28771 RVA: 0x002A6066 File Offset: 0x002A4466
		private static float LinToLut(float x, float lutA)
		{
			return Mathf.Sqrt(x / (x + lutA));
		}

		// Token: 0x06007064 RID: 28772 RVA: 0x002A6074 File Offset: 0x002A4474
		private static float LiftGammaGain(float x, float lift, float invGamma, float gain)
		{
			float num = Mathf.Sqrt(x);
			float num2 = gain * (lift * (1f - num) + Mathf.Pow(num, invGamma));
			return num2 * num2;
		}

		// Token: 0x06007065 RID: 28773 RVA: 0x002A60A0 File Offset: 0x002A44A0
		private static float LogContrast(float x, float linRef, float contrast)
		{
			x = Mathf.Max(x, 1E-05f);
			float num = Mathf.Log(linRef);
			float num2 = Mathf.Log(x);
			float power = num + (num2 - num) * contrast;
			return Mathf.Exp(power);
		}

		// Token: 0x06007066 RID: 28774 RVA: 0x002A60D8 File Offset: 0x002A44D8
		private static Color NormalizeColor(Color c)
		{
			float num = (c.r + c.g + c.b) / 3f;
			if (num == 0f)
			{
				return new Color(1f, 1f, 1f, 1f);
			}
			return new Color
			{
				r = c.r / num,
				g = c.g / num,
				b = c.b / num,
				a = 1f
			};
		}

		// Token: 0x06007067 RID: 28775 RVA: 0x002A616D File Offset: 0x002A456D
		public static float GetLutA()
		{
			return 1.05f;
		}

		// Token: 0x06007068 RID: 28776 RVA: 0x002A6174 File Offset: 0x002A4574
		private void SetIdentityLut()
		{
			int num = 16;
			Color[] array = new Color[num * num * num];
			float num2 = 1f / (1f * (float)num - 1f);
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num; j++)
				{
					for (int k = 0; k < num; k++)
					{
						array[i + j * num + k * num * num] = new Color((float)i * 1f * num2, (float)j * 1f * num2, (float)k * 1f * num2, 1f);
					}
				}
			}
			this.m_UserLutData = array;
			this.m_UserLutDim = num;
		}

		// Token: 0x06007069 RID: 28777 RVA: 0x002A6232 File Offset: 0x002A4632
		private int ClampLutDim(int src)
		{
			return Mathf.Clamp(src, 0, this.m_UserLutDim - 1);
		}

		// Token: 0x0600706A RID: 28778 RVA: 0x002A6244 File Offset: 0x002A4644
		private Color SampleLutNearest(int r, int g, int b)
		{
			r = this.ClampLutDim(r);
			g = this.ClampLutDim(g);
			g = this.ClampLutDim(b);
			return this.m_UserLutData[r + g * this.m_UserLutDim + b * this.m_UserLutDim * this.m_UserLutDim];
		}

		// Token: 0x0600706B RID: 28779 RVA: 0x002A6296 File Offset: 0x002A4696
		private Color SampleLutNearestUnsafe(int r, int g, int b)
		{
			return this.m_UserLutData[r + g * this.m_UserLutDim + b * this.m_UserLutDim * this.m_UserLutDim];
		}

		// Token: 0x0600706C RID: 28780 RVA: 0x002A62C4 File Offset: 0x002A46C4
		private Color SampleLutLinear(float srcR, float srcG, float srcB)
		{
			float num = 0f;
			float num2 = (float)(this.m_UserLutDim - 1);
			float num3 = srcR * num2 + num;
			float num4 = srcG * num2 + num;
			float num5 = srcB * num2 + num;
			int num6 = Mathf.FloorToInt(num3);
			int num7 = Mathf.FloorToInt(num4);
			int num8 = Mathf.FloorToInt(num5);
			num6 = this.ClampLutDim(num6);
			num7 = this.ClampLutDim(num7);
			num8 = this.ClampLutDim(num8);
			int r = this.ClampLutDim(num6 + 1);
			int g = this.ClampLutDim(num7 + 1);
			int b = this.ClampLutDim(num8 + 1);
			float t = num3 - (float)num6;
			float t2 = num4 - (float)num7;
			float t3 = num5 - (float)num8;
			Color a = this.SampleLutNearestUnsafe(num6, num7, num8);
			Color b2 = this.SampleLutNearestUnsafe(num6, num7, b);
			Color a2 = this.SampleLutNearestUnsafe(num6, g, num8);
			Color b3 = this.SampleLutNearestUnsafe(num6, g, b);
			Color a3 = this.SampleLutNearestUnsafe(r, num7, num8);
			Color b4 = this.SampleLutNearestUnsafe(r, num7, b);
			Color a4 = this.SampleLutNearestUnsafe(r, g, num8);
			Color b5 = this.SampleLutNearestUnsafe(r, g, b);
			Color a5 = Color.Lerp(a, b2, t3);
			Color b6 = Color.Lerp(a2, b3, t3);
			Color a6 = Color.Lerp(a3, b4, t3);
			Color b7 = Color.Lerp(a4, b5, t3);
			Color a7 = Color.Lerp(a5, b6, t2);
			Color b8 = Color.Lerp(a6, b7, t2);
			return Color.Lerp(a7, b8, t);
		}

		// Token: 0x0600706D RID: 28781 RVA: 0x002A6434 File Offset: 0x002A4834
		private void UpdateUserLut()
		{
			if (this.userLutTexture == null)
			{
				this.SetIdentityLut();
				return;
			}
			if (!this.ValidDimensions(this.userLutTexture))
			{
				Debug.LogWarning("The given 2D texture " + this.userLutTexture.name + " cannot be used as a 3D LUT. Reverting to identity.");
				this.SetIdentityLut();
				return;
			}
			int height = this.userLutTexture.height;
			Color[] pixels = this.userLutTexture.GetPixels();
			Color[] array = new Color[pixels.Length];
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < height; j++)
				{
					for (int k = 0; k < height; k++)
					{
						int num = height - j - 1;
						Color color = pixels[k * height + i + num * height * height];
						array[i + j * height + k * height * height] = color;
					}
				}
			}
			this.m_UserLutDim = height;
			this.m_UserLutData = array;
		}

		// Token: 0x0600706E RID: 28782 RVA: 0x002A653C File Offset: 0x002A493C
		public float EvalFilmicHelper(float src, float lutA, TonemappingColorGrading.SimplePolyFunc polyToe, TonemappingColorGrading.SimplePolyFunc polyLinear, TonemappingColorGrading.SimplePolyFunc polyShoulder, float x0, float x1, float linearW)
		{
			float num = TonemappingColorGrading.LutToLin(src, lutA);
			if (this.m_FilmicCurve.enabled)
			{
				float linRef = 0.18f;
				num = TonemappingColorGrading.LogContrast(num, linRef, this.m_FilmicCurve.contrast);
				TonemappingColorGrading.SimplePolyFunc simplePolyFunc = polyToe;
				if (num >= x0)
				{
					simplePolyFunc = polyLinear;
				}
				if (num >= x1)
				{
					simplePolyFunc = polyShoulder;
				}
				num = Mathf.Min(num, linearW);
				num = simplePolyFunc.Eval(num);
			}
			return num;
		}

		// Token: 0x0600706F RID: 28783 RVA: 0x002A65A8 File Offset: 0x002A49A8
		private float EvalCurveGradingHelper(float src, float lift, float invGamma, float gain)
		{
			float num = src;
			if (this.m_ColorGrading.enabled)
			{
				num = TonemappingColorGrading.LiftGammaGain(num, lift, invGamma, gain);
			}
			num = Mathf.Max(num, 0f);
			if (this.m_ColorGrading.enabled)
			{
				num = Mathf.Pow(num, this.m_ColorGrading.gamma);
			}
			return num;
		}

		// Token: 0x06007070 RID: 28784 RVA: 0x002A6604 File Offset: 0x002A4A04
		private void Create3DLut(float lutA, TonemappingColorGrading.SimplePolyFunc polyToe, TonemappingColorGrading.SimplePolyFunc polyLinear, TonemappingColorGrading.SimplePolyFunc polyShoulder, float x0, float x1, float linearW, float liftR, float invGammaR, float gainR, float liftG, float invGammaG, float gainG, float liftB, float invGammaB, float gainB)
		{
			int num = 32;
			Color[] array = new Color[num * num * num];
			float num2 = 1f / (1f * (float)num - 1f);
			for (int i = 0; i < num; i++)
			{
				for (int j = 0; j < num; j++)
				{
					for (int k = 0; k < num; k++)
					{
						float src = (float)i * 1f * num2;
						float src2 = (float)j * 1f * num2;
						float src3 = (float)k * 1f * num2;
						float num3 = this.EvalFilmicHelper(src, lutA, polyToe, polyLinear, polyShoulder, x0, x1, linearW);
						float num4 = this.EvalFilmicHelper(src2, lutA, polyToe, polyLinear, polyShoulder, x0, x1, linearW);
						float num5 = this.EvalFilmicHelper(src3, lutA, polyToe, polyLinear, polyShoulder, x0, x1, linearW);
						Color color = this.SampleLutLinear(num3, num4, num5);
						num3 = color.r;
						num4 = color.g;
						num5 = color.b;
						num3 = this.EvalCurveGradingHelper(num3, liftR, invGammaR, gainR);
						num4 = this.EvalCurveGradingHelper(num4, liftG, invGammaG, gainG);
						num5 = this.EvalCurveGradingHelper(num5, liftB, invGammaB, gainB);
						if (this.m_ColorGrading.enabled)
						{
							float num6 = num3 * 0.2125f + num4 * 0.7154f + num5 * 0.0721f;
							num3 = num6 + (num3 - num6) * this.m_ColorGrading.saturation;
							num4 = num6 + (num4 - num6) * this.m_ColorGrading.saturation;
							num5 = num6 + (num5 - num6) * this.m_ColorGrading.saturation;
						}
						array[i + j * num + k * num * num] = new Color(num3, num4, num5, 1f);
					}
				}
			}
			if (this.m_LutTex == null)
			{
				this.m_LutTex = new Texture3D(num, num, num, TextureFormat.RGB24, false);
				this.m_LutTex.filterMode = FilterMode.Bilinear;
				this.m_LutTex.wrapMode = TextureWrapMode.Clamp;
				this.m_LutTex.hideFlags = HideFlags.DontSave;
			}
			this.m_LutTex.SetPixels(array);
			this.m_LutTex.Apply();
		}

		// Token: 0x06007071 RID: 28785 RVA: 0x002A6824 File Offset: 0x002A4C24
		private void Create1DLut(float lutA, TonemappingColorGrading.SimplePolyFunc polyToe, TonemappingColorGrading.SimplePolyFunc polyLinear, TonemappingColorGrading.SimplePolyFunc polyShoulder, float x0, float x1, float linearW, float liftR, float invGammaR, float gainR, float liftG, float invGammaG, float gainG, float liftB, float invGammaB, float gainB)
		{
			int num = 128;
			Color[] array = new Color[num * 2];
			float num2 = 1f / (1f * (float)num - 1f);
			for (int i = 0; i < num; i++)
			{
				float src = (float)i * 1f * num2;
				float src2 = (float)i * 1f * num2;
				float src3 = (float)i * 1f * num2;
				float num3 = this.EvalFilmicHelper(src, lutA, polyToe, polyLinear, polyShoulder, x0, x1, linearW);
				float num4 = this.EvalFilmicHelper(src2, lutA, polyToe, polyLinear, polyShoulder, x0, x1, linearW);
				float num5 = this.EvalFilmicHelper(src3, lutA, polyToe, polyLinear, polyShoulder, x0, x1, linearW);
				Color color = this.SampleLutLinear(num3, num4, num5);
				num3 = color.r;
				num4 = color.g;
				num5 = color.b;
				num3 = this.EvalCurveGradingHelper(num3, liftR, invGammaR, gainR);
				num4 = this.EvalCurveGradingHelper(num4, liftG, invGammaG, gainG);
				num5 = this.EvalCurveGradingHelper(num5, liftB, invGammaB, gainB);
				if (this.isLinearColorSpace)
				{
					num3 = Mathf.LinearToGammaSpace(num3);
					num4 = Mathf.LinearToGammaSpace(num4);
					num5 = Mathf.LinearToGammaSpace(num5);
				}
				array[i + 0 * num] = new Color(num3, num4, num5, 1f);
				array[i + num] = new Color(num3, num4, num5, 1f);
			}
			if (this.m_LutCurveTex1D == null)
			{
				this.m_LutCurveTex1D = new Texture2D(num, 2, TextureFormat.RGB24, false);
				this.m_LutCurveTex1D.filterMode = FilterMode.Bilinear;
				this.m_LutCurveTex1D.wrapMode = TextureWrapMode.Clamp;
				this.m_LutCurveTex1D.hideFlags = HideFlags.DontSave;
			}
			this.m_LutCurveTex1D.SetPixels(array);
			this.m_LutCurveTex1D.Apply();
		}

		// Token: 0x06007072 RID: 28786 RVA: 0x002A69E4 File Offset: 0x002A4DE4
		private void UpdateLut()
		{
			this.UpdateUserLut();
			float lutA = TonemappingColorGrading.GetLutA();
			float p = 2.2f;
			float num = Mathf.Pow(0.33333334f, p);
			float f = 0.7f;
			float num2 = Mathf.Pow(f, p);
			float f2 = Mathf.Pow(f, 1f + this.m_FilmicCurve.lutShoulder * 1f);
			float num3 = Mathf.Pow(f2, p);
			float num4 = num / num2;
			float num5 = num4 * num3;
			float num6 = num5 * (1f - this.m_FilmicCurve.toe * 0.5f);
			float num7 = num6;
			float num8 = num2 - num;
			float num9 = num3 - num7;
			float num10 = 0f;
			if (num8 > 0f && num9 > 0f)
			{
				num10 = num9 / num8;
			}
			TonemappingColorGrading.SimplePolyFunc simplePolyFunc;
			simplePolyFunc.x0 = num;
			simplePolyFunc.y0 = num7;
			simplePolyFunc.A = num10;
			simplePolyFunc.B = 1f;
			simplePolyFunc.signX = 1f;
			simplePolyFunc.signY = 1f;
			simplePolyFunc.logA = Mathf.Log(num10);
			TonemappingColorGrading.SimplePolyFunc polyToe = simplePolyFunc;
			polyToe.Initialize(num, num7, num10);
			float whitePoint = this.GetWhitePoint();
			float x_end = whitePoint - num2;
			float y_end = 1f - num3;
			TonemappingColorGrading.SimplePolyFunc polyShoulder = simplePolyFunc;
			polyShoulder.Initialize(x_end, y_end, num10);
			polyShoulder.signX = -1f;
			polyShoulder.x0 = -whitePoint;
			polyShoulder.signY = -1f;
			polyShoulder.y0 = 1f;
			Color color = TonemappingColorGrading.NormalizeColor(this.m_ColorGrading.lutColors.shadows);
			Color color2 = TonemappingColorGrading.NormalizeColor(this.m_ColorGrading.lutColors.midtones);
			Color color3 = TonemappingColorGrading.NormalizeColor(this.m_ColorGrading.lutColors.highlights);
			float num11 = (color.r + color.g + color.b) / 3f;
			float num12 = (color2.r + color2.g + color2.b) / 3f;
			float num13 = (color3.r + color3.g + color3.b) / 3f;
			float num14 = 0.1f;
			float num15 = 0.5f;
			float num16 = 0.5f;
			float liftR = (color.r - num11) * num14;
			float liftG = (color.g - num11) * num14;
			float liftB = (color.b - num11) * num14;
			float b = Mathf.Pow(2f, (color2.r - num12) * num15);
			float b2 = Mathf.Pow(2f, (color2.g - num12) * num15);
			float b3 = Mathf.Pow(2f, (color2.b - num12) * num15);
			float gainR = Mathf.Pow(2f, (color3.r - num13) * num16);
			float gainG = Mathf.Pow(2f, (color3.g - num13) * num16);
			float gainB = Mathf.Pow(2f, (color3.b - num13) * num16);
			float a = 0.01f;
			float invGammaR = 1f / Mathf.Max(a, b);
			float invGammaG = 1f / Mathf.Max(a, b2);
			float invGammaB = 1f / Mathf.Max(a, b3);
			if (!this.fastMode)
			{
				this.Create3DLut(lutA, polyToe, simplePolyFunc, polyShoulder, num, num2, whitePoint, liftR, invGammaR, gainR, liftG, invGammaG, gainG, liftB, invGammaB, gainB);
			}
			else
			{
				this.Create1DLut(lutA, polyToe, simplePolyFunc, polyShoulder, num, num2, whitePoint, liftR, invGammaR, gainR, liftG, invGammaG, gainG, liftB, invGammaB, gainB);
			}
		}

		// Token: 0x06007073 RID: 28787 RVA: 0x002A6D6C File Offset: 0x002A516C
		public bool ValidDimensions(Texture2D tex2d)
		{
			if (!tex2d)
			{
				return false;
			}
			int height = tex2d.height;
			return height == Mathf.FloorToInt(Mathf.Sqrt((float)tex2d.width));
		}

		// Token: 0x06007074 RID: 28788 RVA: 0x002A6DA7 File Offset: 0x002A51A7
		public void Convert(Texture2D temp2DTex)
		{
		}

		// Token: 0x06007075 RID: 28789 RVA: 0x002A6DAC File Offset: 0x002A51AC
		private void OnDisable()
		{
			if (this.m_TonemapMaterial)
			{
				UnityEngine.Object.DestroyImmediate(this.m_TonemapMaterial);
				this.m_TonemapMaterial = null;
			}
			if (this.m_LutTex)
			{
				UnityEngine.Object.DestroyImmediate(this.m_LutTex);
				this.m_LutTex = null;
			}
			if (this.m_LutCurveTex1D)
			{
				UnityEngine.Object.DestroyImmediate(this.m_LutCurveTex1D);
				this.m_LutCurveTex1D = null;
			}
		}

		// Token: 0x06007076 RID: 28790 RVA: 0x002A6E20 File Offset: 0x002A5220
		[ImageEffectTransformsToLDR]
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (this.tonemapMaterial == null)
			{
				Graphics.Blit(source, destination);
				return;
			}
			if (this.m_LutTex == null || this.m_Dirty)
			{
				this.UpdateLut();
				this.m_Dirty = false;
			}
			if (this.fastMode)
			{
				this.tonemapMaterial.SetTexture("_LutTex1D", this.m_LutCurveTex1D);
			}
			else
			{
				this.tonemapMaterial.SetTexture("_LutTex", this.m_LutTex);
			}
			float lutA = TonemappingColorGrading.GetLutA();
			float num = Mathf.Pow(2f, (!this.m_FilmicCurve.enabled) ? 0f : this.m_FilmicCurve.exposureBias);
			Vector4 value = new Vector4(num, num, num, 1f);
			Color c = new Color(1f, 1f, 1f, 1f);
			if (this.m_ColorGrading.enabled)
			{
				c.r = Mathf.Pow(this.m_ColorGrading.whiteBalance.r, 2.2f);
				c.g = Mathf.Pow(this.m_ColorGrading.whiteBalance.g, 2.2f);
				c.b = Mathf.Pow(this.m_ColorGrading.whiteBalance.b, 2.2f);
				Color color = TonemappingColorGrading.NormalizeColor(c);
				value.x *= color.r;
				value.y *= color.g;
				value.z *= color.b;
			}
			this.tonemapMaterial.SetFloat("_LutA", lutA);
			this.tonemapMaterial.SetVector("_LutExposureMult", value);
			this.tonemapMaterial.SetFloat("_Vibrance", (!this.m_ColorGrading.enabled) ? 1f : this.m_ColorGrading.saturation);
			int pass;
			if (this.debugClamp)
			{
				pass = ((!this.fastMode) ? 2 : 3);
			}
			else
			{
				pass = ((!this.fastMode) ? 0 : 1);
			}
			Graphics.Blit(source, destination, this.tonemapMaterial, pass);
		}

		// Token: 0x040062B1 RID: 25265
		[NonSerialized]
		public bool fastMode;

		// Token: 0x040062B2 RID: 25266
		public bool debugClamp;

		// Token: 0x040062B3 RID: 25267
		[NonSerialized]
		private bool m_Dirty = true;

		// Token: 0x040062B4 RID: 25268
		[SerializeField]
		[TonemappingColorGrading.SettingsGroup]
		[TonemappingColorGrading.DrawFilmicCurveAttribute]
		private TonemappingColorGrading.FilmicCurve m_FilmicCurve = TonemappingColorGrading.FilmicCurve.defaultFilmicCurve;

		// Token: 0x040062B5 RID: 25269
		[SerializeField]
		[TonemappingColorGrading.SettingsGroup]
		private TonemappingColorGrading.ColorGrading m_ColorGrading = TonemappingColorGrading.ColorGrading.defaultColorGrading;

		// Token: 0x040062B6 RID: 25270
		private Texture3D m_LutTex;

		// Token: 0x040062B7 RID: 25271
		private Texture2D m_LutCurveTex1D;

		// Token: 0x040062B8 RID: 25272
		[SerializeField]
		[Tooltip("Lookup Texture|Custom lookup texture")]
		private Texture2D m_UserLutTexture;

		// Token: 0x040062B9 RID: 25273
		public Shader tonemapShader;

		// Token: 0x040062BA RID: 25274
		public bool validRenderTextureFormat = true;

		// Token: 0x040062BB RID: 25275
		private Material m_TonemapMaterial;

		// Token: 0x040062BC RID: 25276
		private int m_UserLutDim = 16;

		// Token: 0x040062BD RID: 25277
		private Color[] m_UserLutData;

		// Token: 0x02000E4E RID: 3662
		[AttributeUsage(AttributeTargets.Field)]
		public class SettingsGroup : Attribute
		{
			// Token: 0x06007077 RID: 28791 RVA: 0x002A7063 File Offset: 0x002A5463
			public SettingsGroup()
			{
			}
		}

		// Token: 0x02000E4F RID: 3663
		public class DrawFilmicCurveAttribute : Attribute
		{
			// Token: 0x06007078 RID: 28792 RVA: 0x002A706B File Offset: 0x002A546B
			public DrawFilmicCurveAttribute()
			{
			}
		}

		// Token: 0x02000E50 RID: 3664
		public enum Passes
		{
			// Token: 0x040062BF RID: 25279
			ThreeD,
			// Token: 0x040062C0 RID: 25280
			OneD,
			// Token: 0x040062C1 RID: 25281
			ThreeDDebug,
			// Token: 0x040062C2 RID: 25282
			OneDDebug
		}

		// Token: 0x02000E51 RID: 3665
		[Serializable]
		public struct FilmicCurve
		{
			// Token: 0x06007079 RID: 28793 RVA: 0x002A7074 File Offset: 0x002A5474
			// Note: this type is marked as 'beforefieldinit'.
			static FilmicCurve()
			{
			}

			// Token: 0x040062C3 RID: 25283
			public bool enabled;

			// Token: 0x040062C4 RID: 25284
			[Range(-4f, 4f)]
			[Tooltip("Exposure Bias|Adjusts the overall exposure of the scene")]
			public float exposureBias;

			// Token: 0x040062C5 RID: 25285
			[Range(0f, 2f)]
			[Tooltip("Contrast|Contrast adjustment (log-space)")]
			public float contrast;

			// Token: 0x040062C6 RID: 25286
			[Range(0f, 1f)]
			[Tooltip("Toe|Toe of the filmic curve; affects the darker areas of the scene")]
			public float toe;

			// Token: 0x040062C7 RID: 25287
			[Range(0f, 1f)]
			[Tooltip("Shoulder|Shoulder of the filmic curve; brings overexposed highlights back into range")]
			public float lutShoulder;

			// Token: 0x040062C8 RID: 25288
			public static TonemappingColorGrading.FilmicCurve defaultFilmicCurve = new TonemappingColorGrading.FilmicCurve
			{
				enabled = false,
				exposureBias = 0f,
				contrast = 1f,
				toe = 0f,
				lutShoulder = 0f
			};
		}

		// Token: 0x02000E52 RID: 3666
		public class ColorWheelGroup : PropertyAttribute
		{
			// Token: 0x0600707A RID: 28794 RVA: 0x002A70C7 File Offset: 0x002A54C7
			public ColorWheelGroup()
			{
			}

			// Token: 0x0600707B RID: 28795 RVA: 0x002A70E2 File Offset: 0x002A54E2
			public ColorWheelGroup(int minSizePerWheel, int maxSizePerWheel)
			{
				this.minSizePerWheel = minSizePerWheel;
				this.maxSizePerWheel = maxSizePerWheel;
			}

			// Token: 0x040062C9 RID: 25289
			public int minSizePerWheel = 60;

			// Token: 0x040062CA RID: 25290
			public int maxSizePerWheel = 150;
		}

		// Token: 0x02000E53 RID: 3667
		[Serializable]
		public struct ColorGradingColors
		{
			// Token: 0x0600707C RID: 28796 RVA: 0x002A710C File Offset: 0x002A550C
			// Note: this type is marked as 'beforefieldinit'.
			static ColorGradingColors()
			{
			}

			// Token: 0x040062CB RID: 25291
			[Tooltip("Shadows|Shadows color")]
			public Color shadows;

			// Token: 0x040062CC RID: 25292
			[Tooltip("Midtones|Midtones color")]
			public Color midtones;

			// Token: 0x040062CD RID: 25293
			[Tooltip("Highlights|Highlights color")]
			public Color highlights;

			// Token: 0x040062CE RID: 25294
			public static TonemappingColorGrading.ColorGradingColors defaultGradingColors = new TonemappingColorGrading.ColorGradingColors
			{
				shadows = new Color(1f, 1f, 1f),
				midtones = new Color(1f, 1f, 1f),
				highlights = new Color(1f, 1f, 1f)
			};
		}

		// Token: 0x02000E54 RID: 3668
		[Serializable]
		public struct ColorGrading
		{
			// Token: 0x0600707D RID: 28797 RVA: 0x002A7178 File Offset: 0x002A5578
			// Note: this type is marked as 'beforefieldinit'.
			static ColorGrading()
			{
			}

			// Token: 0x040062CF RID: 25295
			public bool enabled;

			// Token: 0x040062D0 RID: 25296
			[ColorUsage(false)]
			[Tooltip("White Balance|Adjusts the white color before tonemapping")]
			public Color whiteBalance;

			// Token: 0x040062D1 RID: 25297
			[Range(0f, 2f)]
			[Tooltip("Vibrance|Pushes the intensity of all colors")]
			public float saturation;

			// Token: 0x040062D2 RID: 25298
			[Range(0f, 5f)]
			[Tooltip("Gamma|Adjusts the gamma")]
			public float gamma;

			// Token: 0x040062D3 RID: 25299
			[TonemappingColorGrading.ColorWheelGroup]
			public TonemappingColorGrading.ColorGradingColors lutColors;

			// Token: 0x040062D4 RID: 25300
			public static TonemappingColorGrading.ColorGrading defaultColorGrading = new TonemappingColorGrading.ColorGrading
			{
				whiteBalance = Color.white,
				enabled = false,
				saturation = 1f,
				gamma = 1f,
				lutColors = TonemappingColorGrading.ColorGradingColors.defaultGradingColors
			};
		}

		// Token: 0x02000E55 RID: 3669
		public struct SimplePolyFunc
		{
			// Token: 0x0600707E RID: 28798 RVA: 0x002A71CB File Offset: 0x002A55CB
			public float Eval(float x)
			{
				return this.signY * Mathf.Exp(this.logA + this.B * Mathf.Log(this.signX * x - this.x0)) + this.y0;
			}

			// Token: 0x0600707F RID: 28799 RVA: 0x002A7204 File Offset: 0x002A5604
			public void Initialize(float x_end, float y_end, float m)
			{
				this.A = 0f;
				this.B = 1f;
				this.x0 = 0f;
				this.y0 = 0f;
				this.signX = 1f;
				this.signY = 1f;
				if (m <= 0f || y_end <= 0f)
				{
					return;
				}
				if (x_end <= 0f)
				{
					return;
				}
				this.B = m * x_end / y_end;
				float num = Mathf.Pow(x_end, this.B);
				this.A = y_end / num;
				this.logA = Mathf.Log(y_end) - this.B * Mathf.Log(x_end);
			}

			// Token: 0x040062D5 RID: 25301
			public float A;

			// Token: 0x040062D6 RID: 25302
			public float B;

			// Token: 0x040062D7 RID: 25303
			public float x0;

			// Token: 0x040062D8 RID: 25304
			public float y0;

			// Token: 0x040062D9 RID: 25305
			public float signX;

			// Token: 0x040062DA RID: 25306
			public float signY;

			// Token: 0x040062DB RID: 25307
			public float logA;
		}
	}
}
