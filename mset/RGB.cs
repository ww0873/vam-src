﻿using System;
using UnityEngine;

namespace mset
{
	// Token: 0x02000320 RID: 800
	public class RGB
	{
		// Token: 0x060012BF RID: 4799 RVA: 0x0006AE0E File Offset: 0x0006920E
		public RGB()
		{
		}

		// Token: 0x060012C0 RID: 4800 RVA: 0x0006AE18 File Offset: 0x00069218
		public static void toRGBM(ref Color32 rgbm, Color color, bool useGamma)
		{
			if (useGamma)
			{
				color.r = Mathf.Pow(color.r, Gamma.toSRGB);
				color.g = Mathf.Pow(color.g, Gamma.toSRGB);
				color.b = Mathf.Pow(color.b, Gamma.toSRGB);
			}
			color *= 0.16666667f;
			float num = Mathf.Max(Mathf.Max(color.r, color.g), color.b);
			num = Mathf.Clamp01(num);
			num = Mathf.Ceil(num * 255f) / 255f;
			if (num > 0f)
			{
				float num2 = 1f / num;
				color.r = Mathf.Clamp01(color.r * num2);
				color.g = Mathf.Clamp01(color.g * num2);
				color.b = Mathf.Clamp01(color.b * num2);
				rgbm.r = (byte)(color.r * 255f);
				rgbm.g = (byte)(color.g * 255f);
				rgbm.b = (byte)(color.b * 255f);
				rgbm.a = (byte)(num * 255f);
			}
			else
			{
				rgbm.r = (rgbm.g = (rgbm.b = (rgbm.a = 0)));
			}
		}

		// Token: 0x060012C1 RID: 4801 RVA: 0x0006AF80 File Offset: 0x00069380
		public static void toRGBM(ref Color rgbm, Color color, bool useGamma)
		{
			if (useGamma)
			{
				color.r = Mathf.Pow(color.r, Gamma.toSRGB);
				color.g = Mathf.Pow(color.g, Gamma.toSRGB);
				color.b = Mathf.Pow(color.b, Gamma.toSRGB);
			}
			color *= 0.16666667f;
			float num = Mathf.Max(Mathf.Max(color.r, color.g), color.b);
			num = Mathf.Clamp01(num);
			num = Mathf.Ceil(num * 255f) / 255f;
			if (num > 0f)
			{
				float num2 = 1f / num;
				rgbm.r = Mathf.Clamp01(color.r * num2);
				rgbm.g = Mathf.Clamp01(color.g * num2);
				rgbm.b = Mathf.Clamp01(color.b * num2);
				rgbm.a = num;
			}
			else
			{
				rgbm.r = (rgbm.g = (rgbm.b = (rgbm.a = 0f)));
			}
		}

		// Token: 0x060012C2 RID: 4802 RVA: 0x0006B0A4 File Offset: 0x000694A4
		public static void fromRGBM(ref Color color, Color32 rgbm, bool useGamma)
		{
			float num = 0.003921569f;
			float b = (float)rgbm.a * num;
			color.r = (float)rgbm.r * num;
			color.g = (float)rgbm.g * num;
			color.b = (float)rgbm.b * num;
			color *= b;
			color *= 6f;
			if (useGamma)
			{
				color.r = Mathf.Pow(color.r, Gamma.toLinear);
				color.g = Mathf.Pow(color.g, Gamma.toLinear);
				color.b = Mathf.Pow(color.b, Gamma.toLinear);
			}
			color.a = 1f;
		}

		// Token: 0x060012C3 RID: 4803 RVA: 0x0006B170 File Offset: 0x00069570
		public static void fromRGBM(ref Color color, Color rgbm, bool useGamma)
		{
			float a = rgbm.a;
			color = rgbm;
			color *= a;
			color *= 6f;
			if (useGamma)
			{
				color.r = Mathf.Pow(color.r, Gamma.toLinear);
				color.g = Mathf.Pow(color.g, Gamma.toLinear);
				color.b = Mathf.Pow(color.b, Gamma.toLinear);
			}
			color.a = 1f;
		}

		// Token: 0x060012C4 RID: 4804 RVA: 0x0006B208 File Offset: 0x00069608
		public static void fromXYZ(ref Color rgb, Color xyz)
		{
			rgb.r = 3.2404542f * xyz.r - 1.5371385f * xyz.g - 0.4985314f * xyz.b;
			rgb.g = -0.969266f * xyz.r + 1.8760108f * xyz.g + 0.041556f * xyz.b;
			rgb.b = 0.0556434f * xyz.r - 0.2040259f * xyz.g + 1.0572252f * xyz.b;
		}

		// Token: 0x060012C5 RID: 4805 RVA: 0x0006B2A4 File Offset: 0x000696A4
		public static void toXYZ(ref Color xyz, Color rgb)
		{
			xyz.r = 0.4124564f * rgb.r + 0.3575761f * rgb.g + 0.1804375f * rgb.b;
			xyz.g = 0.2126729f * rgb.r + 0.7151522f * rgb.g + 0.072175f * rgb.b;
			xyz.b = 0.0193339f * rgb.r + 0.119192f * rgb.g + 0.9503041f * rgb.b;
		}

		// Token: 0x060012C6 RID: 4806 RVA: 0x0006B340 File Offset: 0x00069740
		public static void toRGBE(ref Color32 rgbe, Color color)
		{
			float num = Mathf.Max(Mathf.Max(color.r, color.g), color.b);
			int num2 = Mathf.CeilToInt(Mathf.Log(num, 2f));
			num2 = Mathf.Clamp(num2, -128, 127);
			num = Mathf.Pow(2f, (float)num2);
			float num3 = 255f / num;
			rgbe.r = (byte)Mathf.Clamp(color.r * num3, 0f, 255f);
			rgbe.g = (byte)Mathf.Clamp(color.g * num3, 0f, 255f);
			rgbe.b = (byte)Mathf.Clamp(color.b * num3, 0f, 255f);
			rgbe.a = (byte)(num2 + 128);
		}
	}
}
