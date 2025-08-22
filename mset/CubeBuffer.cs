using System;
using UnityEngine;

namespace mset
{
	// Token: 0x02000321 RID: 801
	public class CubeBuffer
	{
		// Token: 0x060012C7 RID: 4807 RVA: 0x0006B409 File Offset: 0x00069809
		public CubeBuffer()
		{
			this.filterMode = CubeBuffer.FilterMode.BILINEAR;
			this.clear();
		}

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x060012C9 RID: 4809 RVA: 0x0006B499 File Offset: 0x00069899
		// (set) Token: 0x060012C8 RID: 4808 RVA: 0x0006B420 File Offset: 0x00069820
		public CubeBuffer.FilterMode filterMode
		{
			get
			{
				return this._filterMode;
			}
			set
			{
				this._filterMode = value;
				CubeBuffer.FilterMode filterMode = this._filterMode;
				if (filterMode != CubeBuffer.FilterMode.NEAREST)
				{
					if (filterMode != CubeBuffer.FilterMode.BILINEAR)
					{
						if (filterMode == CubeBuffer.FilterMode.BICUBIC)
						{
							this.sample = new CubeBuffer.SampleFunc(this.sampleBicubic);
						}
					}
					else
					{
						this.sample = new CubeBuffer.SampleFunc(this.sampleBilinear);
					}
				}
				else
				{
					this.sample = new CubeBuffer.SampleFunc(this.sampleNearest);
				}
			}
		}

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x060012CA RID: 4810 RVA: 0x0006B4A1 File Offset: 0x000698A1
		public int width
		{
			get
			{
				return this.faceSize;
			}
		}

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x060012CB RID: 4811 RVA: 0x0006B4A9 File Offset: 0x000698A9
		public int height
		{
			get
			{
				return this.faceSize * 6;
			}
		}

		// Token: 0x060012CC RID: 4812 RVA: 0x0006B4B4 File Offset: 0x000698B4
		~CubeBuffer()
		{
		}

		// Token: 0x060012CD RID: 4813 RVA: 0x0006B4E0 File Offset: 0x000698E0
		public void clear()
		{
			this.pixels = null;
			this.faceSize = 0;
		}

		// Token: 0x060012CE RID: 4814 RVA: 0x0006B4F0 File Offset: 0x000698F0
		public bool empty()
		{
			return this.pixels == null || this.pixels.Length == 0;
		}

		// Token: 0x060012CF RID: 4815 RVA: 0x0006B510 File Offset: 0x00069910
		public static void pixelCopy(ref Color[] dst, int dst_offset, Color[] src, int src_offset, int count)
		{
			for (int i = 0; i < count; i++)
			{
				dst[dst_offset + i] = src[src_offset + i];
			}
		}

		// Token: 0x060012D0 RID: 4816 RVA: 0x0006B550 File Offset: 0x00069950
		public static void pixelCopy(ref Color[] dst, int dst_offset, Color32[] src, int src_offset, int count)
		{
			float num = 0.003921569f;
			for (int i = 0; i < count; i++)
			{
				dst[dst_offset + i].r = (float)src[src_offset + i].r * num;
				dst[dst_offset + i].g = (float)src[src_offset + i].g * num;
				dst[dst_offset + i].b = (float)src[src_offset + i].b * num;
				dst[dst_offset + i].a = (float)src[src_offset + i].a * num;
			}
		}

		// Token: 0x060012D1 RID: 4817 RVA: 0x0006B5F8 File Offset: 0x000699F8
		public static void pixelCopy(ref Color32[] dst, int dst_offset, Color[] src, int src_offset, int count)
		{
			for (int i = 0; i < count; i++)
			{
				dst[dst_offset + i].r = (byte)Mathf.Clamp(src[src_offset + i].r * 255f, 0f, 255f);
				dst[dst_offset + i].g = (byte)Mathf.Clamp(src[src_offset + i].g * 255f, 0f, 255f);
				dst[dst_offset + i].b = (byte)Mathf.Clamp(src[src_offset + i].b * 255f, 0f, 255f);
				dst[dst_offset + i].a = (byte)Mathf.Clamp(src[src_offset + i].a * 255f, 0f, 255f);
			}
		}

		// Token: 0x060012D2 RID: 4818 RVA: 0x0006B6E4 File Offset: 0x00069AE4
		public static void pixelCopyBlock<T>(ref T[] dst, int dst_x, int dst_y, int dst_w, T[] src, int src_x, int src_y, int src_w, int block_w, int block_h, bool flip)
		{
			if (flip)
			{
				for (int i = 0; i < block_w; i++)
				{
					for (int j = 0; j < block_h; j++)
					{
						int num = (dst_y + j) * dst_w + dst_x + i;
						int num2 = (src_y + (block_h - j - 1)) * src_w + src_x + i;
						dst[num] = src[num2];
					}
				}
			}
			else
			{
				for (int k = 0; k < block_w; k++)
				{
					for (int l = 0; l < block_h; l++)
					{
						int num3 = (dst_y + l) * dst_w + dst_x + k;
						int num4 = (src_y + l) * src_w + src_x + k;
						dst[num3] = src[num4];
					}
				}
			}
		}

		// Token: 0x060012D3 RID: 4819 RVA: 0x0006B7AC File Offset: 0x00069BAC
		public static void encode(ref Color[] dst, Color[] src, ColorMode outMode, bool useGamma)
		{
			if (outMode == ColorMode.RGBM8)
			{
				for (int i = 0; i < src.Length; i++)
				{
					RGB.toRGBM(ref dst[i], src[i], useGamma);
				}
			}
			else if (useGamma)
			{
				Util.applyGamma(ref dst, src, Gamma.toSRGB);
			}
			else
			{
				CubeBuffer.pixelCopy(ref dst, 0, src, 0, src.Length);
			}
		}

		// Token: 0x060012D4 RID: 4820 RVA: 0x0006B818 File Offset: 0x00069C18
		public static void encode(ref Color32[] dst, Color[] src, ColorMode outMode, bool useGamma)
		{
			if (outMode == ColorMode.RGBM8)
			{
				for (int i = 0; i < src.Length; i++)
				{
					RGB.toRGBM(ref dst[i], src[i], useGamma);
				}
			}
			else
			{
				if (useGamma)
				{
					Util.applyGamma(ref src, src, Gamma.toSRGB);
				}
				CubeBuffer.pixelCopy(ref dst, 0, src, 0, src.Length);
			}
		}

		// Token: 0x060012D5 RID: 4821 RVA: 0x0006B880 File Offset: 0x00069C80
		public static void decode(ref Color[] dst, Color[] src, ColorMode inMode, bool useGamma)
		{
			if (inMode == ColorMode.RGBM8)
			{
				for (int i = 0; i < src.Length; i++)
				{
					RGB.fromRGBM(ref dst[i], src[i], useGamma);
				}
			}
			else
			{
				if (useGamma)
				{
					Util.applyGamma(ref dst, src, Gamma.toLinear);
				}
				else
				{
					CubeBuffer.pixelCopy(ref dst, 0, src, 0, src.Length);
				}
				CubeBuffer.clearAlpha(ref dst);
			}
		}

		// Token: 0x060012D6 RID: 4822 RVA: 0x0006B8F0 File Offset: 0x00069CF0
		public static void decode(ref Color[] dst, Color32[] src, ColorMode inMode, bool useGamma)
		{
			if (inMode == ColorMode.RGBM8)
			{
				for (int i = 0; i < src.Length; i++)
				{
					RGB.fromRGBM(ref dst[i], src[i], useGamma);
				}
			}
			else
			{
				CubeBuffer.pixelCopy(ref dst, 0, src, 0, src.Length);
				if (useGamma)
				{
					Util.applyGamma(ref dst, Gamma.toLinear);
				}
				CubeBuffer.clearAlpha(ref dst);
			}
		}

		// Token: 0x060012D7 RID: 4823 RVA: 0x0006B95C File Offset: 0x00069D5C
		public static void decode(ref Color[] dst, int dst_offset, Color[] src, int src_offset, int count, ColorMode inMode, bool useGamma)
		{
			if (inMode == ColorMode.RGBM8)
			{
				for (int i = 0; i < count; i++)
				{
					RGB.fromRGBM(ref dst[i + dst_offset], src[i + src_offset], useGamma);
				}
			}
			else
			{
				if (useGamma)
				{
					Util.applyGamma(ref dst, dst_offset, src, src_offset, count, Gamma.toLinear);
				}
				else
				{
					CubeBuffer.pixelCopy(ref dst, dst_offset, src, src_offset, count);
				}
				CubeBuffer.clearAlpha(ref dst, dst_offset, count);
			}
		}

		// Token: 0x060012D8 RID: 4824 RVA: 0x0006B9D8 File Offset: 0x00069DD8
		public static void decode(ref Color[] dst, int dst_offset, Color32[] src, int src_offset, int count, ColorMode inMode, bool useGamma)
		{
			if (inMode == ColorMode.RGBM8)
			{
				for (int i = 0; i < count; i++)
				{
					RGB.fromRGBM(ref dst[i + dst_offset], src[i + src_offset], useGamma);
				}
			}
			else
			{
				CubeBuffer.pixelCopy(ref dst, dst_offset, src, src_offset, count);
				if (useGamma)
				{
					Util.applyGamma(ref dst, dst_offset, dst, dst_offset, count, Gamma.toLinear);
				}
				CubeBuffer.clearAlpha(ref dst, dst_offset, count);
			}
		}

		// Token: 0x060012D9 RID: 4825 RVA: 0x0006BA50 File Offset: 0x00069E50
		public static void clearAlpha(ref Color[] dst)
		{
			CubeBuffer.clearAlpha(ref dst, 0, dst.Length);
		}

		// Token: 0x060012DA RID: 4826 RVA: 0x0006BA60 File Offset: 0x00069E60
		public static void clearAlpha(ref Color[] dst, int offset, int count)
		{
			for (int i = offset; i < offset + count; i++)
			{
				dst[i].a = 1f;
			}
		}

		// Token: 0x060012DB RID: 4827 RVA: 0x0006BA93 File Offset: 0x00069E93
		public static void clearAlpha(ref Color32[] dst)
		{
			CubeBuffer.clearAlpha(ref dst, 0, dst.Length);
		}

		// Token: 0x060012DC RID: 4828 RVA: 0x0006BAA0 File Offset: 0x00069EA0
		public static void clearAlpha(ref Color32[] dst, int offset, int count)
		{
			for (int i = offset; i < offset + count; i++)
			{
				dst[i].a = byte.MaxValue;
			}
		}

		// Token: 0x060012DD RID: 4829 RVA: 0x0006BAD4 File Offset: 0x00069ED4
		public static void applyExposure(ref Color[] pixels, float mult)
		{
			for (int i = 0; i < pixels.Length; i++)
			{
				Color[] array = pixels;
				int num = i;
				array[num].r = array[num].r * mult;
				Color[] array2 = pixels;
				int num2 = i;
				array2[num2].g = array2[num2].g * mult;
				Color[] array3 = pixels;
				int num3 = i;
				array3[num3].b = array3[num3].b * mult;
			}
		}

		// Token: 0x060012DE RID: 4830 RVA: 0x0006BB38 File Offset: 0x00069F38
		public void applyExposure(float mult)
		{
			for (int i = 0; i < this.pixels.Length; i++)
			{
				Color[] array = this.pixels;
				int num = i;
				array[num].r = array[num].r * mult;
				Color[] array2 = this.pixels;
				int num2 = i;
				array2[num2].g = array2[num2].g * mult;
				Color[] array3 = this.pixels;
				int num3 = i;
				array3[num3].b = array3[num3].b * mult;
			}
		}

		// Token: 0x060012DF RID: 4831 RVA: 0x0006BBA9 File Offset: 0x00069FA9
		public int toIndex(int face, int x, int y)
		{
			x = Mathf.Clamp(x, 0, this.faceSize - 1);
			y = Mathf.Clamp(y, 0, this.faceSize - 1);
			return this.faceSize * this.faceSize * face + this.faceSize * y + x;
		}

		// Token: 0x060012E0 RID: 4832 RVA: 0x0006BBE7 File Offset: 0x00069FE7
		public int toIndex(CubemapFace face, int x, int y)
		{
			x = Mathf.Clamp(x, 0, this.faceSize - 1);
			y = Mathf.Clamp(y, 0, this.faceSize - 1);
			return this.faceSize * this.faceSize * (int)face + this.faceSize * y + x;
		}

		// Token: 0x060012E1 RID: 4833 RVA: 0x0006BC28 File Offset: 0x0006A028
		private static void linkEdges()
		{
			if (CubeBuffer._leftEdges == null)
			{
				CubeBuffer._leftEdges = new CubeBuffer.CubeEdge[6];
				CubeBuffer._leftEdges[1] = new CubeBuffer.CubeEdge(5, false, false);
				CubeBuffer._leftEdges[0] = new CubeBuffer.CubeEdge(4, false, false);
				CubeBuffer._leftEdges[3] = new CubeBuffer.CubeEdge(1, true, true);
				CubeBuffer._leftEdges[2] = new CubeBuffer.CubeEdge(1, false, true, true);
				CubeBuffer._leftEdges[5] = new CubeBuffer.CubeEdge(0, false, false);
				CubeBuffer._leftEdges[4] = new CubeBuffer.CubeEdge(1, false, false);
				CubeBuffer._rightEdges = new CubeBuffer.CubeEdge[6];
				CubeBuffer._rightEdges[1] = new CubeBuffer.CubeEdge(4, false, false);
				CubeBuffer._rightEdges[0] = new CubeBuffer.CubeEdge(5, false, false);
				CubeBuffer._rightEdges[3] = new CubeBuffer.CubeEdge(0, false, true, true);
				CubeBuffer._rightEdges[2] = new CubeBuffer.CubeEdge(0, true, true);
				CubeBuffer._rightEdges[5] = new CubeBuffer.CubeEdge(1, false, false);
				CubeBuffer._rightEdges[4] = new CubeBuffer.CubeEdge(0, false, false);
				CubeBuffer._upEdges = new CubeBuffer.CubeEdge[6];
				CubeBuffer._upEdges[1] = new CubeBuffer.CubeEdge(2, false, true, true);
				CubeBuffer._upEdges[0] = new CubeBuffer.CubeEdge(2, true, true);
				CubeBuffer._upEdges[3] = new CubeBuffer.CubeEdge(4, false, false);
				CubeBuffer._upEdges[2] = new CubeBuffer.CubeEdge(5, true, false, true);
				CubeBuffer._upEdges[5] = new CubeBuffer.CubeEdge(2, true, false, true);
				CubeBuffer._upEdges[4] = new CubeBuffer.CubeEdge(2, false, false);
				CubeBuffer._downEdges = new CubeBuffer.CubeEdge[6];
				CubeBuffer._downEdges[1] = new CubeBuffer.CubeEdge(3, true, true);
				CubeBuffer._downEdges[0] = new CubeBuffer.CubeEdge(3, false, true, true);
				CubeBuffer._downEdges[3] = new CubeBuffer.CubeEdge(5, true, false, true);
				CubeBuffer._downEdges[2] = new CubeBuffer.CubeEdge(4, false, false);
				CubeBuffer._downEdges[5] = new CubeBuffer.CubeEdge(3, true, false, true);
				CubeBuffer._downEdges[4] = new CubeBuffer.CubeEdge(3, false, false);
				for (int i = 0; i < 6; i++)
				{
					CubeBuffer._leftEdges[i].minEdge = (CubeBuffer._upEdges[i].minEdge = true);
					CubeBuffer._rightEdges[i].minEdge = (CubeBuffer._downEdges[i].minEdge = false);
				}
			}
		}

		// Token: 0x060012E2 RID: 4834 RVA: 0x0006BE28 File Offset: 0x0006A228
		public int toIndexLinked(int face, int u, int v)
		{
			CubeBuffer.linkEdges();
			int num = face;
			CubeBuffer._leftEdges[num].transmogrify(ref u, ref v, ref num, this.faceSize);
			CubeBuffer._upEdges[num].transmogrify(ref v, ref u, ref num, this.faceSize);
			CubeBuffer._rightEdges[num].transmogrify(ref u, ref v, ref num, this.faceSize);
			CubeBuffer._downEdges[num].transmogrify(ref v, ref u, ref num, this.faceSize);
			u = Mathf.Clamp(u, 0, this.faceSize - 1);
			v = Mathf.Clamp(v, 0, this.faceSize - 1);
			return this.toIndex(num, u, v);
		}

		// Token: 0x060012E3 RID: 4835 RVA: 0x0006BEC8 File Offset: 0x0006A2C8
		public void sampleNearest(ref Color dst, float u, float v, int face)
		{
			int num = Mathf.FloorToInt((float)this.faceSize * u);
			int num2 = Mathf.FloorToInt((float)this.faceSize * v);
			dst = this.pixels[this.faceSize * this.faceSize * face + this.faceSize * num2 + num];
		}

		// Token: 0x060012E4 RID: 4836 RVA: 0x0006BF24 File Offset: 0x0006A324
		public void sampleBilinear(ref Color dst, float u, float v, int face)
		{
			u = (float)this.faceSize * u + 0.5f;
			int num = Mathf.FloorToInt(u) - 1;
			u = Mathf.Repeat(u, 1f);
			v = (float)this.faceSize * v + 0.5f;
			int num2 = Mathf.FloorToInt(v) - 1;
			v = Mathf.Repeat(v, 1f);
			int num3 = this.toIndexLinked(face, num, num2);
			int num4 = this.toIndexLinked(face, num + 1, num2);
			int num5 = this.toIndexLinked(face, num + 1, num2 + 1);
			int num6 = this.toIndexLinked(face, num, num2 + 1);
			Color a = Color.Lerp(this.pixels[num3], this.pixels[num4], u);
			Color b = Color.Lerp(this.pixels[num6], this.pixels[num5], u);
			dst = Color.Lerp(a, b, v);
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x0006C01C File Offset: 0x0006A41C
		public void sampleBicubic(ref Color dst, float u, float v, int face)
		{
			u = (float)this.faceSize * u + 0.5f;
			int num = Mathf.FloorToInt(u) - 1;
			u = Mathf.Repeat(u, 1f);
			v = (float)this.faceSize * v + 0.5f;
			int num2 = Mathf.FloorToInt(v) - 1;
			v = Mathf.Repeat(v, 1f);
			for (int i = 0; i < 4; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					int num3 = this.toIndexLinked(face, num - 1 + i, num2 - 1 + j);
					CubeBuffer.cubicKernel[i, j] = this.pixels[num3];
				}
			}
			float t = 0.85f;
			float b = 0.333f;
			Color color;
			Color a;
			Color color2;
			Color color3;
			Color a2;
			Color color4;
			Color b2;
			for (int k = 0; k < 4; k++)
			{
				color = CubeBuffer.cubicKernel[0, k];
				a = CubeBuffer.cubicKernel[1, k];
				color2 = CubeBuffer.cubicKernel[2, k];
				color3 = CubeBuffer.cubicKernel[3, k];
				color = Color.Lerp(a, color, t);
				color3 = Color.Lerp(color2, color3, t);
				color = a + b * (a - color);
				color3 = color2 + b * (color2 - color3);
				a2 = Color.Lerp(a, color, u);
				color4 = Color.Lerp(color, color3, u);
				b2 = Color.Lerp(color3, color2, u);
				a2 = Color.Lerp(a2, color4, u);
				color4 = Color.Lerp(color4, b2, u);
				CubeBuffer.cubicKernel[0, k] = Color.Lerp(a2, color4, u);
			}
			color = CubeBuffer.cubicKernel[0, 0];
			a = CubeBuffer.cubicKernel[0, 1];
			color2 = CubeBuffer.cubicKernel[0, 2];
			color3 = CubeBuffer.cubicKernel[0, 3];
			color = Color.Lerp(a, color, t);
			color3 = Color.Lerp(color2, color3, t);
			color = a + b * (a - color);
			color3 = color2 + b * (color2 - color3);
			a2 = Color.Lerp(a, color, v);
			color4 = Color.Lerp(color, color3, v);
			b2 = Color.Lerp(color3, color2, v);
			a2 = Color.Lerp(a2, color4, v);
			color4 = Color.Lerp(color4, b2, v);
			dst = Color.Lerp(a2, color4, v);
		}

		// Token: 0x060012E6 RID: 4838 RVA: 0x0006C2A8 File Offset: 0x0006A6A8
		public void resize(int newFaceSize)
		{
			if (newFaceSize == this.faceSize)
			{
				return;
			}
			this.faceSize = newFaceSize;
			this.pixels = null;
			this.pixels = new Color[this.faceSize * this.faceSize * 6];
			Util.clearTo(ref this.pixels, Color.black);
		}

		// Token: 0x060012E7 RID: 4839 RVA: 0x0006C2FA File Offset: 0x0006A6FA
		public void resize(int newFaceSize, Color clearColor)
		{
			this.resize(newFaceSize);
			Util.clearTo(ref this.pixels, clearColor);
		}

		// Token: 0x060012E8 RID: 4840 RVA: 0x0006C310 File Offset: 0x0006A710
		public void resample(int newSize)
		{
			if (newSize == this.faceSize)
			{
				return;
			}
			Color[] array = new Color[newSize * newSize * 6];
			this.resample(ref array, newSize);
			this.pixels = array;
			this.faceSize = newSize;
		}

		// Token: 0x060012E9 RID: 4841 RVA: 0x0006C34C File Offset: 0x0006A74C
		public void resample(ref Color[] dst, int newSize)
		{
			int num = newSize * newSize;
			float num2 = 1f / (float)newSize;
			for (int i = 0; i < 6; i++)
			{
				for (int j = 0; j < newSize; j++)
				{
					float v = ((float)j + 0.5f) * num2;
					for (int k = 0; k < newSize; k++)
					{
						float u = ((float)k + 0.5f) * num2;
						int num3 = num * i + j * newSize + k;
						this.sample(ref dst[num3], u, v, i);
					}
				}
			}
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x0006C3DE File Offset: 0x0006A7DE
		public void resampleFace(ref Color[] dst, int face, int newSize, bool flipY)
		{
			this.resampleFace(ref dst, 0, face, newSize, flipY);
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x0006C3EC File Offset: 0x0006A7EC
		public void resampleFace(ref Color[] dst, int dstOffset, int face, int newSize, bool flipY)
		{
			if (newSize == this.faceSize)
			{
				CubeBuffer.pixelCopy(ref dst, dstOffset, this.pixels, face * this.faceSize * this.faceSize, this.faceSize * this.faceSize);
				return;
			}
			float num = 1f / (float)newSize;
			if (flipY)
			{
				for (int i = 0; i < newSize; i++)
				{
					float v = 1f - ((float)i + 0.5f) * num;
					for (int j = 0; j < newSize; j++)
					{
						float u = ((float)j + 0.5f) * num;
						int num2 = i * newSize + j + dstOffset;
						this.sample(ref dst[num2], u, v, face);
					}
				}
			}
			else
			{
				for (int k = 0; k < newSize; k++)
				{
					float v2 = ((float)k + 0.5f) * num;
					for (int l = 0; l < newSize; l++)
					{
						float u2 = ((float)l + 0.5f) * num;
						int num3 = k * newSize + l + dstOffset;
						this.sample(ref dst[num3], u2, v2, face);
					}
				}
			}
		}

		// Token: 0x060012EC RID: 4844 RVA: 0x0006C518 File Offset: 0x0006A918
		public void fromCube(Cubemap cube, int mip, ColorMode cubeColorMode, bool useGamma)
		{
			int num = cube.width >> mip;
			if (this.pixels == null || this.faceSize != num)
			{
				this.resize(num);
			}
			for (int i = 0; i < 6; i++)
			{
				Color[] array = cube.GetPixels((CubemapFace)i, mip);
				CubeBuffer.pixelCopy(ref this.pixels, i * this.faceSize * this.faceSize, array, 0, array.Length);
			}
			CubeBuffer.decode(ref this.pixels, this.pixels, cubeColorMode, useGamma);
		}

		// Token: 0x060012ED RID: 4845 RVA: 0x0006C5A0 File Offset: 0x0006A9A0
		public void toCube(ref Cubemap cube, int mip, ColorMode cubeColorMode, bool useGamma)
		{
			int num = this.faceSize * this.faceSize;
			Color[] array = new Color[num];
			for (int i = 0; i < 6; i++)
			{
				CubeBuffer.pixelCopy(ref array, 0, this.pixels, i * num, num);
				CubeBuffer.encode(ref array, array, cubeColorMode, useGamma);
				cube.SetPixels(array, (CubemapFace)i, mip);
			}
			cube.Apply(false);
		}

		// Token: 0x060012EE RID: 4846 RVA: 0x0006C604 File Offset: 0x0006AA04
		public void resampleToCube(ref Cubemap cube, int mip, ColorMode cubeColorMode, bool useGamma, float exposureMult)
		{
			int num = cube.width >> mip;
			int num2 = num * num * 6;
			Color[] array = new Color[num2];
			for (int i = 0; i < 6; i++)
			{
				this.resampleFace(ref array, i, num, false);
				if (exposureMult != 1f)
				{
					CubeBuffer.applyExposure(ref array, exposureMult);
				}
				CubeBuffer.encode(ref array, array, cubeColorMode, useGamma);
				cube.SetPixels(array, (CubemapFace)i, mip);
			}
			cube.Apply(false);
		}

		// Token: 0x060012EF RID: 4847 RVA: 0x0006C67C File Offset: 0x0006AA7C
		public void resampleToBuffer(ref CubeBuffer dst, float exposureMult)
		{
			int num = dst.faceSize * dst.faceSize;
			for (int i = 0; i < 6; i++)
			{
				this.resampleFace(ref dst.pixels, i * num, i, dst.faceSize, false);
				dst.applyExposure(exposureMult);
			}
		}

		// Token: 0x060012F0 RID: 4848 RVA: 0x0006C6CC File Offset: 0x0006AACC
		public void fromBuffer(CubeBuffer src)
		{
			this.clear();
			this.faceSize = src.faceSize;
			this.pixels = new Color[src.pixels.Length];
			CubeBuffer.pixelCopy(ref this.pixels, 0, src.pixels, 0, this.pixels.Length);
		}

		// Token: 0x060012F1 RID: 4849 RVA: 0x0006C71C File Offset: 0x0006AB1C
		public void fromPanoTexture(Texture2D tex, int _faceSize, ColorMode texColorMode, bool useGamma)
		{
			this.resize(_faceSize);
			ulong num = (ulong)((long)this.faceSize);
			for (ulong num2 = 0UL; num2 < 6UL; num2 += 1UL)
			{
				for (ulong num3 = 0UL; num3 < num; num3 += 1UL)
				{
					for (ulong num4 = 0UL; num4 < num; num4 += 1UL)
					{
						float x = 0f;
						float num5 = 0f;
						Util.cubeToLatLongLookup(ref x, ref num5, num2, num4, num3, num);
						float num6 = 1f / (float)this.faceSize;
						num5 = Mathf.Clamp(num5, num6, 1f - num6);
						this.pixels[(int)(checked((IntPtr)(unchecked(num2 * num * num + num3 * num + num4))))] = tex.GetPixelBilinear(x, num5);
					}
				}
			}
			CubeBuffer.decode(ref this.pixels, this.pixels, texColorMode, useGamma);
		}

		// Token: 0x060012F2 RID: 4850 RVA: 0x0006C7EC File Offset: 0x0006ABEC
		public void fromColTexture(Texture2D tex, ColorMode texColorMode, bool useGamma)
		{
			this.fromColTexture(tex, 0, texColorMode, useGamma);
		}

		// Token: 0x060012F3 RID: 4851 RVA: 0x0006C7F8 File Offset: 0x0006ABF8
		public void fromColTexture(Texture2D tex, int mip, ColorMode texColorMode, bool useGamma)
		{
			if (tex.width * 6 != tex.height)
			{
				Debug.LogError("CubeBuffer.fromColTexture takes textures of a 1x6 aspect ratio");
				return;
			}
			int num = tex.width >> mip;
			if (this.pixels == null || this.faceSize != num)
			{
				this.resize(num);
			}
			Color32[] pixels = tex.GetPixels32(mip);
			if ((float)pixels[0].a != 1f)
			{
				CubeBuffer.clearAlpha(ref pixels);
			}
			CubeBuffer.decode(ref this.pixels, pixels, texColorMode, useGamma);
		}

		// Token: 0x060012F4 RID: 4852 RVA: 0x0006C882 File Offset: 0x0006AC82
		public void fromHorizCrossTexture(Texture2D tex, ColorMode texColorMode, bool useGamma)
		{
			this.fromHorizCrossTexture(tex, 0, texColorMode, useGamma);
		}

		// Token: 0x060012F5 RID: 4853 RVA: 0x0006C890 File Offset: 0x0006AC90
		public void fromHorizCrossTexture(Texture2D tex, int mip, ColorMode texColorMode, bool useGamma)
		{
			if (tex.width * 3 != tex.height * 4)
			{
				Debug.LogError("CubeBuffer.fromHorizCrossTexture takes textures of a 4x3 aspect ratio");
				return;
			}
			int num = tex.width / 4 >> mip;
			if (this.pixels == null || this.faceSize != num)
			{
				this.resize(num);
			}
			Color32[] pixels = tex.GetPixels32(mip);
			if ((float)pixels[0].a != 1f)
			{
				CubeBuffer.clearAlpha(ref pixels);
			}
			Color32[] src = new Color32[this.faceSize * this.faceSize];
			for (int i = 0; i < 6; i++)
			{
				CubemapFace cubemapFace = (CubemapFace)i;
				int src_x = 0;
				int src_y = 0;
				int dst_offset = i * this.faceSize * this.faceSize;
				switch (cubemapFace)
				{
				case CubemapFace.PositiveX:
					src_x = this.faceSize * 2;
					src_y = this.faceSize;
					break;
				case CubemapFace.NegativeX:
					src_x = 0;
					src_y = this.faceSize;
					break;
				case CubemapFace.PositiveY:
					src_x = this.faceSize;
					src_y = this.faceSize * 2;
					break;
				case CubemapFace.NegativeY:
					src_x = this.faceSize;
					src_y = 0;
					break;
				case CubemapFace.PositiveZ:
					src_x = this.faceSize;
					src_y = this.faceSize;
					break;
				case CubemapFace.NegativeZ:
					src_x = this.faceSize * 3;
					src_y = this.faceSize;
					break;
				}
				CubeBuffer.pixelCopyBlock<Color32>(ref src, 0, 0, this.faceSize, pixels, src_x, src_y, this.faceSize * 4, this.faceSize, this.faceSize, true);
				CubeBuffer.decode(ref this.pixels, dst_offset, src, 0, this.faceSize * this.faceSize, texColorMode, useGamma);
			}
		}

		// Token: 0x060012F6 RID: 4854 RVA: 0x0006CA34 File Offset: 0x0006AE34
		public void toColTexture(ref Texture2D tex, ColorMode texColorMode, bool useGamma)
		{
			if (tex.width != this.faceSize || tex.height != this.faceSize * 6)
			{
				tex.Resize(this.faceSize, 6 * this.faceSize);
			}
			Color32[] pixels = tex.GetPixels32();
			CubeBuffer.encode(ref pixels, this.pixels, texColorMode, useGamma);
			tex.SetPixels32(pixels);
			tex.Apply(false);
		}

		// Token: 0x060012F7 RID: 4855 RVA: 0x0006CAA4 File Offset: 0x0006AEA4
		public void toPanoTexture(ref Texture2D tex, ColorMode texColorMode, bool useGamma)
		{
			ulong num = (ulong)((long)tex.width);
			ulong num2 = (ulong)((long)tex.height);
			Color[] array = tex.GetPixels();
			for (ulong num3 = 0UL; num3 < num; num3 += 1UL)
			{
				for (ulong num4 = 0UL; num4 < num2; num4 += 1UL)
				{
					float u = 0f;
					float v = 0f;
					ulong num5 = 0UL;
					Util.latLongToCubeLookup(ref u, ref v, ref num5, num3, num4, num, num2);
					this.sample(ref array[(int)(checked((IntPtr)(unchecked(num4 * num + num3))))], u, v, (int)num5);
				}
			}
			CubeBuffer.encode(ref array, array, texColorMode, useGamma);
			tex.SetPixels(array);
			tex.Apply(tex.mipmapCount > 1);
		}

		// Token: 0x060012F8 RID: 4856 RVA: 0x0006CB5C File Offset: 0x0006AF5C
		public void toPanoBuffer(ref Color[] buffer, int width, int height)
		{
			ulong num = (ulong)((long)width);
			ulong num2 = (ulong)((long)height);
			for (ulong num3 = 0UL; num3 < num; num3 += 1UL)
			{
				for (ulong num4 = 0UL; num4 < num2; num4 += 1UL)
				{
					float u = 0f;
					float v = 0f;
					ulong num5 = 0UL;
					Util.latLongToCubeLookup(ref u, ref v, ref num5, num3, num4, num, num2);
					this.sample(ref buffer[(int)(checked((IntPtr)(unchecked(num4 * num + num3))))], u, v, (int)num5);
				}
			}
		}

		// Token: 0x060012F9 RID: 4857 RVA: 0x0006CBD7 File Offset: 0x0006AFD7
		// Note: this type is marked as 'beforefieldinit'.
		static CubeBuffer()
		{
		}

		// Token: 0x04001082 RID: 4226
		public CubeBuffer.SampleFunc sample;

		// Token: 0x04001083 RID: 4227
		private CubeBuffer.FilterMode _filterMode;

		// Token: 0x04001084 RID: 4228
		public int faceSize;

		// Token: 0x04001085 RID: 4229
		public Color[] pixels;

		// Token: 0x04001086 RID: 4230
		private static CubeBuffer.CubeEdge[] _leftEdges = null;

		// Token: 0x04001087 RID: 4231
		private static CubeBuffer.CubeEdge[] _rightEdges = null;

		// Token: 0x04001088 RID: 4232
		private static CubeBuffer.CubeEdge[] _upEdges = null;

		// Token: 0x04001089 RID: 4233
		private static CubeBuffer.CubeEdge[] _downEdges = null;

		// Token: 0x0400108A RID: 4234
		private static Color[,] cubicKernel = new Color[4, 4];

		// Token: 0x02000322 RID: 802
		public enum FilterMode
		{
			// Token: 0x0400108C RID: 4236
			NEAREST,
			// Token: 0x0400108D RID: 4237
			BILINEAR,
			// Token: 0x0400108E RID: 4238
			BICUBIC
		}

		// Token: 0x02000323 RID: 803
		// (Invoke) Token: 0x060012FB RID: 4859
		public delegate void SampleFunc(ref Color dst, float u, float v, int face);

		// Token: 0x02000324 RID: 804
		private class CubeEdge
		{
			// Token: 0x060012FE RID: 4862 RVA: 0x0006CBFD File Offset: 0x0006AFFD
			public CubeEdge(int Other, bool flip, bool swizzle)
			{
				this.other = Other;
				this.flipped = flip;
				this.swizzled = swizzle;
				this.mirrored = false;
				this.minEdge = false;
			}

			// Token: 0x060012FF RID: 4863 RVA: 0x0006CC28 File Offset: 0x0006B028
			public CubeEdge(int Other, bool flip, bool swizzle, bool mirror)
			{
				this.other = Other;
				this.flipped = flip;
				this.swizzled = swizzle;
				this.mirrored = mirror;
				this.minEdge = false;
			}

			// Token: 0x06001300 RID: 4864 RVA: 0x0006CC54 File Offset: 0x0006B054
			public void transmogrify(ref int primary, ref int secondary, ref int face, int faceSize)
			{
				bool flag = false;
				if (this.minEdge && primary < 0)
				{
					primary = faceSize + primary;
					flag = true;
				}
				else if (!this.minEdge && primary >= faceSize)
				{
					primary %= faceSize;
					flag = true;
				}
				if (flag)
				{
					if (this.mirrored)
					{
						primary = faceSize - primary - 1;
					}
					if (this.flipped)
					{
						secondary = faceSize - secondary - 1;
					}
					if (this.swizzled)
					{
						int num = secondary;
						secondary = primary;
						primary = num;
					}
					face = this.other;
				}
			}

			// Token: 0x06001301 RID: 4865 RVA: 0x0006CCEC File Offset: 0x0006B0EC
			public void transmogrify(ref int primary_i, ref int primary_j, ref int secondary_i, ref int secondary_j, ref int face_i, ref int face_j, int faceSize)
			{
				if (primary_i < 0)
				{
					primary_i = (primary_j = faceSize - 1);
				}
				else
				{
					primary_i = (primary_j = 0);
				}
				if (this.mirrored)
				{
					primary_i = faceSize - primary_i - 1;
					primary_j = faceSize - primary_j - 1;
				}
				if (this.flipped)
				{
					secondary_i = faceSize - secondary_i - 1;
					secondary_j = faceSize - secondary_j - 1;
				}
				if (this.swizzled)
				{
					int num = secondary_i;
					secondary_i = primary_i;
					primary_i = num;
					num = secondary_j;
					secondary_j = primary_j;
					primary_j = num;
				}
				face_i = (face_j = this.other);
			}

			// Token: 0x0400108F RID: 4239
			public int other;

			// Token: 0x04001090 RID: 4240
			public bool flipped;

			// Token: 0x04001091 RID: 4241
			public bool swizzled;

			// Token: 0x04001092 RID: 4242
			public bool mirrored;

			// Token: 0x04001093 RID: 4243
			public bool minEdge;
		}
	}
}
