using System;
using UnityEngine;

// Token: 0x0200030E RID: 782
public class TextureScale
{
	// Token: 0x0600126E RID: 4718 RVA: 0x00066DB9 File Offset: 0x000651B9
	public TextureScale()
	{
	}

	// Token: 0x0600126F RID: 4719 RVA: 0x00066DC1 File Offset: 0x000651C1
	public static void Point(Texture2D tex, int newWidth, int newHeight)
	{
		TextureScale.ThreadedScale(tex, newWidth, newHeight, false);
	}

	// Token: 0x06001270 RID: 4720 RVA: 0x00066DCC File Offset: 0x000651CC
	public static void Bilinear(Texture2D tex, int newWidth, int newHeight)
	{
		TextureScale.ThreadedScale(tex, newWidth, newHeight, true);
	}

	// Token: 0x06001271 RID: 4721 RVA: 0x00066DD8 File Offset: 0x000651D8
	private static void ThreadedScale(Texture2D tex, int newWidth, int newHeight, bool useBilinear)
	{
		TextureScale.texColors = tex.GetPixels();
		TextureScale.newColors = new Color[newWidth * newHeight];
		if (useBilinear)
		{
			TextureScale.ratioX = 1f / ((float)newWidth / (float)(tex.width - 1));
			TextureScale.ratioY = 1f / ((float)newHeight / (float)(tex.height - 1));
		}
		else
		{
			TextureScale.ratioX = (float)tex.width / (float)newWidth;
			TextureScale.ratioY = (float)tex.height / (float)newHeight;
		}
		TextureScale.w = tex.width;
		TextureScale.w2 = newWidth;
		TextureScale.ThreadData obj = new TextureScale.ThreadData(0, newHeight);
		if (useBilinear)
		{
			TextureScale.BilinearScale(obj);
		}
		else
		{
			TextureScale.PointScale(obj);
		}
		tex.Resize(newWidth, newHeight);
		tex.SetPixels(TextureScale.newColors);
		tex.Apply();
	}

	// Token: 0x06001272 RID: 4722 RVA: 0x00066EA0 File Offset: 0x000652A0
	public static void BilinearScale(object obj)
	{
		TextureScale.ThreadData threadData = (TextureScale.ThreadData)obj;
		for (int i = threadData.start; i < threadData.end; i++)
		{
			int num = (int)Mathf.Floor((float)i * TextureScale.ratioY);
			int num2 = num * TextureScale.w;
			int num3 = (num + 1) * TextureScale.w;
			int num4 = i * TextureScale.w2;
			for (int j = 0; j < TextureScale.w2; j++)
			{
				int num5 = (int)Mathf.Floor((float)j * TextureScale.ratioX);
				float value = (float)j * TextureScale.ratioX - (float)num5;
				TextureScale.newColors[num4 + j] = TextureScale.ColorLerpUnclamped(TextureScale.ColorLerpUnclamped(TextureScale.texColors[num2 + num5], TextureScale.texColors[num2 + num5 + 1], value), TextureScale.ColorLerpUnclamped(TextureScale.texColors[num3 + num5], TextureScale.texColors[num3 + num5 + 1], value), (float)i * TextureScale.ratioY - (float)num);
			}
		}
		TextureScale.finishCount++;
	}

	// Token: 0x06001273 RID: 4723 RVA: 0x00066FC8 File Offset: 0x000653C8
	public static void PointScale(object obj)
	{
		TextureScale.ThreadData threadData = (TextureScale.ThreadData)obj;
		for (int i = threadData.start; i < threadData.end; i++)
		{
			int num = (int)(TextureScale.ratioY * (float)i) * TextureScale.w;
			int num2 = i * TextureScale.w2;
			for (int j = 0; j < TextureScale.w2; j++)
			{
				TextureScale.newColors[num2 + j] = TextureScale.texColors[(int)((float)num + TextureScale.ratioX * (float)j)];
			}
		}
		TextureScale.finishCount++;
	}

	// Token: 0x06001274 RID: 4724 RVA: 0x00067068 File Offset: 0x00065468
	private static Color ColorLerpUnclamped(Color c1, Color c2, float value)
	{
		return new Color(c1.r + (c2.r - c1.r) * value, c1.g + (c2.g - c1.g) * value, c1.b + (c2.b - c1.b) * value, c1.a + (c2.a - c1.a) * value);
	}

	// Token: 0x04000FDA RID: 4058
	private static Color[] texColors;

	// Token: 0x04000FDB RID: 4059
	private static Color[] newColors;

	// Token: 0x04000FDC RID: 4060
	private static int w;

	// Token: 0x04000FDD RID: 4061
	private static float ratioX;

	// Token: 0x04000FDE RID: 4062
	private static float ratioY;

	// Token: 0x04000FDF RID: 4063
	private static int w2;

	// Token: 0x04000FE0 RID: 4064
	private static int finishCount;

	// Token: 0x0200030F RID: 783
	public class ThreadData
	{
		// Token: 0x06001275 RID: 4725 RVA: 0x000670DE File Offset: 0x000654DE
		public ThreadData(int s, int e)
		{
			this.start = s;
			this.end = e;
		}

		// Token: 0x04000FE1 RID: 4065
		public int start;

		// Token: 0x04000FE2 RID: 4066
		public int end;
	}
}
