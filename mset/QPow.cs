using System;
using System.Runtime.CompilerServices;

namespace mset
{
	// Token: 0x0200032B RID: 811
	public class QPow
	{
		// Token: 0x06001322 RID: 4898 RVA: 0x0006E73B File Offset: 0x0006CB3B
		public QPow()
		{
		}

		// Token: 0x06001323 RID: 4899 RVA: 0x0006E743 File Offset: 0x0006CB43
		public static float Pow1(float f)
		{
			return f;
		}

		// Token: 0x06001324 RID: 4900 RVA: 0x0006E746 File Offset: 0x0006CB46
		public static float Pow2(float f)
		{
			return f * f;
		}

		// Token: 0x06001325 RID: 4901 RVA: 0x0006E74B File Offset: 0x0006CB4B
		public static float Pow4(float f)
		{
			f *= f;
			return f * f;
		}

		// Token: 0x06001326 RID: 4902 RVA: 0x0006E755 File Offset: 0x0006CB55
		public static float Pow8(float f)
		{
			f *= f;
			f *= f;
			return f * f;
		}

		// Token: 0x06001327 RID: 4903 RVA: 0x0006E764 File Offset: 0x0006CB64
		public static float Pow16(float f)
		{
			f *= f;
			f *= f;
			f *= f;
			return f * f;
		}

		// Token: 0x06001328 RID: 4904 RVA: 0x0006E778 File Offset: 0x0006CB78
		public static float Pow32(float f)
		{
			f *= f;
			f *= f;
			f *= f;
			f *= f;
			return f * f;
		}

		// Token: 0x06001329 RID: 4905 RVA: 0x0006E791 File Offset: 0x0006CB91
		public static float Pow64(float f)
		{
			f *= f;
			f *= f;
			f *= f;
			f *= f;
			f *= f;
			return f * f;
		}

		// Token: 0x0600132A RID: 4906 RVA: 0x0006E7AF File Offset: 0x0006CBAF
		public static float Pow128(float f)
		{
			f *= f;
			f *= f;
			f *= f;
			f *= f;
			f *= f;
			f *= f;
			return f * f;
		}

		// Token: 0x0600132B RID: 4907 RVA: 0x0006E7D2 File Offset: 0x0006CBD2
		public static float Pow256(float f)
		{
			f *= f;
			f *= f;
			f *= f;
			f *= f;
			f *= f;
			f *= f;
			f *= f;
			return f * f;
		}

		// Token: 0x0600132C RID: 4908 RVA: 0x0006E7FA File Offset: 0x0006CBFA
		public static float Pow512(float f)
		{
			f *= f;
			f *= f;
			f *= f;
			f *= f;
			f *= f;
			f *= f;
			f *= f;
			f *= f;
			return f * f;
		}

		// Token: 0x0600132D RID: 4909 RVA: 0x0006E828 File Offset: 0x0006CC28
		public static QPow.PowFunc closestPowFunc(int exp)
		{
			if (exp + 128 >= 512)
			{
				if (QPow.<>f__mg$cache0 == null)
				{
					QPow.<>f__mg$cache0 = new QPow.PowFunc(QPow.Pow512);
				}
				return QPow.<>f__mg$cache0;
			}
			if (exp + 64 >= 256)
			{
				if (QPow.<>f__mg$cache1 == null)
				{
					QPow.<>f__mg$cache1 = new QPow.PowFunc(QPow.Pow256);
				}
				return QPow.<>f__mg$cache1;
			}
			if (exp + 32 >= 128)
			{
				if (QPow.<>f__mg$cache2 == null)
				{
					QPow.<>f__mg$cache2 = new QPow.PowFunc(QPow.Pow128);
				}
				return QPow.<>f__mg$cache2;
			}
			if (exp + 16 >= 64)
			{
				if (QPow.<>f__mg$cache3 == null)
				{
					QPow.<>f__mg$cache3 = new QPow.PowFunc(QPow.Pow64);
				}
				return QPow.<>f__mg$cache3;
			}
			if (exp + 8 >= 32)
			{
				if (QPow.<>f__mg$cache4 == null)
				{
					QPow.<>f__mg$cache4 = new QPow.PowFunc(QPow.Pow32);
				}
				return QPow.<>f__mg$cache4;
			}
			if (exp + 4 >= 16)
			{
				if (QPow.<>f__mg$cache5 == null)
				{
					QPow.<>f__mg$cache5 = new QPow.PowFunc(QPow.Pow16);
				}
				return QPow.<>f__mg$cache5;
			}
			if (exp + 2 >= 8)
			{
				if (QPow.<>f__mg$cache6 == null)
				{
					QPow.<>f__mg$cache6 = new QPow.PowFunc(QPow.Pow8);
				}
				return QPow.<>f__mg$cache6;
			}
			if (exp + 1 >= 4)
			{
				if (QPow.<>f__mg$cache7 == null)
				{
					QPow.<>f__mg$cache7 = new QPow.PowFunc(QPow.Pow4);
				}
				return QPow.<>f__mg$cache7;
			}
			if (exp >= 2)
			{
				if (QPow.<>f__mg$cache8 == null)
				{
					QPow.<>f__mg$cache8 = new QPow.PowFunc(QPow.Pow2);
				}
				return QPow.<>f__mg$cache8;
			}
			if (QPow.<>f__mg$cache9 == null)
			{
				QPow.<>f__mg$cache9 = new QPow.PowFunc(QPow.Pow1);
			}
			return QPow.<>f__mg$cache9;
		}

		// Token: 0x0600132E RID: 4910 RVA: 0x0006E9C8 File Offset: 0x0006CDC8
		public static int Log2i(int val)
		{
			int num = 0;
			while (val > 0)
			{
				num++;
				val >>= 1;
			}
			return num;
		}

		// Token: 0x0600132F RID: 4911 RVA: 0x0006E9F0 File Offset: 0x0006CDF0
		public static int Log2i(ulong val)
		{
			int num = 0;
			while (val > 0UL)
			{
				num++;
				val >>= 1;
			}
			return num;
		}

		// Token: 0x06001330 RID: 4912 RVA: 0x0006EA16 File Offset: 0x0006CE16
		public static int clampedDownShift(int val, int shift)
		{
			while (val > 0 && shift > 0)
			{
				val >>= 1;
				shift--;
			}
			return val;
		}

		// Token: 0x06001331 RID: 4913 RVA: 0x0006EA36 File Offset: 0x0006CE36
		public static int clampedDownShift(int val, int shift, int minVal)
		{
			while (val > minVal && shift > 0)
			{
				val >>= 1;
				shift--;
			}
			return val;
		}

		// Token: 0x040010C7 RID: 4295
		[CompilerGenerated]
		private static QPow.PowFunc <>f__mg$cache0;

		// Token: 0x040010C8 RID: 4296
		[CompilerGenerated]
		private static QPow.PowFunc <>f__mg$cache1;

		// Token: 0x040010C9 RID: 4297
		[CompilerGenerated]
		private static QPow.PowFunc <>f__mg$cache2;

		// Token: 0x040010CA RID: 4298
		[CompilerGenerated]
		private static QPow.PowFunc <>f__mg$cache3;

		// Token: 0x040010CB RID: 4299
		[CompilerGenerated]
		private static QPow.PowFunc <>f__mg$cache4;

		// Token: 0x040010CC RID: 4300
		[CompilerGenerated]
		private static QPow.PowFunc <>f__mg$cache5;

		// Token: 0x040010CD RID: 4301
		[CompilerGenerated]
		private static QPow.PowFunc <>f__mg$cache6;

		// Token: 0x040010CE RID: 4302
		[CompilerGenerated]
		private static QPow.PowFunc <>f__mg$cache7;

		// Token: 0x040010CF RID: 4303
		[CompilerGenerated]
		private static QPow.PowFunc <>f__mg$cache8;

		// Token: 0x040010D0 RID: 4304
		[CompilerGenerated]
		private static QPow.PowFunc <>f__mg$cache9;

		// Token: 0x0200032C RID: 812
		// (Invoke) Token: 0x06001333 RID: 4915
		public delegate float PowFunc(float f);
	}
}
