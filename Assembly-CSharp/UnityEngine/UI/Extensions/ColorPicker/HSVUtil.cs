﻿using System;

namespace UnityEngine.UI.Extensions.ColorPicker
{
	// Token: 0x020004BA RID: 1210
	public static class HSVUtil
	{
		// Token: 0x06001E8C RID: 7820 RVA: 0x000ADDF2 File Offset: 0x000AC1F2
		public static HsvColor ConvertRgbToHsv(Color color)
		{
			return HSVUtil.ConvertRgbToHsv((double)((int)(color.r * 255f)), (double)((int)(color.g * 255f)), (double)((int)(color.b * 255f)));
		}

		// Token: 0x06001E8D RID: 7821 RVA: 0x000ADE28 File Offset: 0x000AC228
		public static HsvColor ConvertRgbToHsv(double r, double b, double g)
		{
			double num = 0.0;
			double num2 = Math.Min(Math.Min(r, g), b);
			double num3 = Math.Max(Math.Max(r, g), b);
			double num4 = num3 - num2;
			double num5;
			if (num3 == 0.0)
			{
				num5 = 0.0;
			}
			else
			{
				num5 = num4 / num3;
			}
			if (num5 == 0.0)
			{
				num = 360.0;
			}
			else
			{
				if (r == num3)
				{
					num = (g - b) / num4;
				}
				else if (g == num3)
				{
					num = 2.0 + (b - r) / num4;
				}
				else if (b == num3)
				{
					num = 4.0 + (r - g) / num4;
				}
				num *= 60.0;
				if (num <= 0.0)
				{
					num += 360.0;
				}
			}
			return new HsvColor
			{
				H = 360.0 - num,
				S = num5,
				V = num3 / 255.0
			};
		}

		// Token: 0x06001E8E RID: 7822 RVA: 0x000ADF50 File Offset: 0x000AC350
		public static Color ConvertHsvToRgb(double h, double s, double v, float alpha)
		{
			double num;
			double num2;
			double num3;
			if (s == 0.0)
			{
				num = v;
				num2 = v;
				num3 = v;
			}
			else
			{
				if (h == 360.0)
				{
					h = 0.0;
				}
				else
				{
					h /= 60.0;
				}
				int num4 = (int)h;
				double num5 = h - (double)num4;
				double num6 = v * (1.0 - s);
				double num7 = v * (1.0 - s * num5);
				double num8 = v * (1.0 - s * (1.0 - num5));
				switch (num4)
				{
				case 0:
					num = v;
					num2 = num8;
					num3 = num6;
					break;
				case 1:
					num = num7;
					num2 = v;
					num3 = num6;
					break;
				case 2:
					num = num6;
					num2 = v;
					num3 = num8;
					break;
				case 3:
					num = num6;
					num2 = num7;
					num3 = v;
					break;
				case 4:
					num = num8;
					num2 = num6;
					num3 = v;
					break;
				default:
					num = v;
					num2 = num6;
					num3 = num7;
					break;
				}
			}
			return new Color((float)num, (float)num2, (float)num3, alpha);
		}
	}
}
