using System;
using UnityEngine;

namespace mset
{
	// Token: 0x02000330 RID: 816
	public class SHUtil
	{
		// Token: 0x06001341 RID: 4929 RVA: 0x0006ED78 File Offset: 0x0006D178
		public SHUtil()
		{
		}

		// Token: 0x06001342 RID: 4930 RVA: 0x0006ED80 File Offset: 0x0006D180
		private static float project_l0_m0(Vector3 u)
		{
			return SHEncoding.sEquationConstants[0];
		}

		// Token: 0x06001343 RID: 4931 RVA: 0x0006ED89 File Offset: 0x0006D189
		private static float project_l1_mneg1(Vector3 u)
		{
			return SHEncoding.sEquationConstants[1] * u.y;
		}

		// Token: 0x06001344 RID: 4932 RVA: 0x0006ED9A File Offset: 0x0006D19A
		private static float project_l1_m0(Vector3 u)
		{
			return SHEncoding.sEquationConstants[2] * u.z;
		}

		// Token: 0x06001345 RID: 4933 RVA: 0x0006EDAB File Offset: 0x0006D1AB
		private static float project_l1_m1(Vector3 u)
		{
			return SHEncoding.sEquationConstants[3] * u.x;
		}

		// Token: 0x06001346 RID: 4934 RVA: 0x0006EDBC File Offset: 0x0006D1BC
		private static float project_l2_mneg2(Vector3 u)
		{
			return SHEncoding.sEquationConstants[4] * u.y * u.x;
		}

		// Token: 0x06001347 RID: 4935 RVA: 0x0006EDD5 File Offset: 0x0006D1D5
		private static float project_l2_mneg1(Vector3 u)
		{
			return SHEncoding.sEquationConstants[5] * u.y * u.z;
		}

		// Token: 0x06001348 RID: 4936 RVA: 0x0006EDEE File Offset: 0x0006D1EE
		private static float project_l2_m0(Vector3 u)
		{
			return SHEncoding.sEquationConstants[6] * (3f * u.z * u.z - 1f);
		}

		// Token: 0x06001349 RID: 4937 RVA: 0x0006EE13 File Offset: 0x0006D213
		private static float project_l2_m1(Vector3 u)
		{
			return SHEncoding.sEquationConstants[7] * u.z * u.x;
		}

		// Token: 0x0600134A RID: 4938 RVA: 0x0006EE2C File Offset: 0x0006D22C
		private static float project_l2_m2(Vector3 u)
		{
			return SHEncoding.sEquationConstants[8] * (u.x * u.x - u.y * u.y);
		}

		// Token: 0x0600134B RID: 4939 RVA: 0x0006EE58 File Offset: 0x0006D258
		private static void scale(ref SHEncoding sh, float s)
		{
			for (int i = 0; i < 27; i++)
			{
				sh.c[i] *= s;
			}
		}

		// Token: 0x0600134C RID: 4940 RVA: 0x0006EE8C File Offset: 0x0006D28C
		public static void projectCubeBuffer(ref SHEncoding sh, CubeBuffer cube)
		{
			sh.clearToBlack();
			float num = 0f;
			ulong num2 = (ulong)((long)cube.faceSize);
			float[] array = new float[9];
			Vector3 zero = Vector3.zero;
			for (ulong num3 = 0UL; num3 < 6UL; num3 += 1UL)
			{
				for (ulong num4 = 0UL; num4 < num2; num4 += 1UL)
				{
					for (ulong num5 = 0UL; num5 < num2; num5 += 1UL)
					{
						float num6 = 1f;
						Util.invCubeLookup(ref zero, ref num6, num3, num5, num4, num2);
						float num7 = 1.3333334f;
						ulong num8 = num3 * num2 * num2 + num4 * num2 + num5;
						Color color = cube.pixels[(int)(checked((IntPtr)num8))];
						array[0] = SHUtil.project_l0_m0(zero);
						array[1] = SHUtil.project_l1_mneg1(zero);
						array[2] = SHUtil.project_l1_m0(zero);
						array[3] = SHUtil.project_l1_m1(zero);
						array[4] = SHUtil.project_l2_mneg2(zero);
						array[5] = SHUtil.project_l2_mneg1(zero);
						array[6] = SHUtil.project_l2_m0(zero);
						array[7] = SHUtil.project_l2_m1(zero);
						array[8] = SHUtil.project_l2_m2(zero);
						for (int i = 0; i < 9; i++)
						{
							sh.c[3 * i] += num7 * num6 * color[0] * array[i];
							sh.c[3 * i + 1] += num7 * num6 * color[1] * array[i];
							sh.c[3 * i + 2] += num7 * num6 * color[2] * array[i];
						}
						num += num6;
					}
				}
			}
			SHUtil.scale(ref sh, 16f / num);
		}

		// Token: 0x0600134D RID: 4941 RVA: 0x0006F03C File Offset: 0x0006D43C
		public static void projectCube(ref SHEncoding sh, Cubemap cube, int mip, bool hdr)
		{
			sh.clearToBlack();
			float num = 0f;
			ulong num2 = (ulong)((long)cube.width);
			mip = Mathf.Min(QPow.Log2i(num2) + 1, mip);
			num2 >>= mip;
			float[] array = new float[9];
			Vector3 zero = Vector3.zero;
			for (ulong num3 = 0UL; num3 < 6UL; num3 += 1UL)
			{
				Color color = Color.black;
				Color[] pixels = cube.GetPixels((CubemapFace)num3, mip);
				for (ulong num4 = 0UL; num4 < num2; num4 += 1UL)
				{
					for (ulong num5 = 0UL; num5 < num2; num5 += 1UL)
					{
						float num6 = 1f;
						Util.invCubeLookup(ref zero, ref num6, num3, num5, num4, num2);
						float num7 = 1.3333334f;
						ulong num8 = num4 * num2 + num5;
						checked
						{
							if (hdr)
							{
								RGB.fromRGBM(ref color, pixels[(int)((IntPtr)num8)], true);
							}
							else
							{
								color = pixels[(int)((IntPtr)num8)];
							}
							array[0] = SHUtil.project_l0_m0(zero);
							array[1] = SHUtil.project_l1_mneg1(zero);
							array[2] = SHUtil.project_l1_m0(zero);
							array[3] = SHUtil.project_l1_m1(zero);
							array[4] = SHUtil.project_l2_mneg2(zero);
							array[5] = SHUtil.project_l2_mneg1(zero);
							array[6] = SHUtil.project_l2_m0(zero);
							array[7] = SHUtil.project_l2_m1(zero);
							array[8] = SHUtil.project_l2_m2(zero);
						}
						for (int i = 0; i < 9; i++)
						{
							sh.c[3 * i] += num7 * num6 * color[0] * array[i];
							sh.c[3 * i + 1] += num7 * num6 * color[1] * array[i];
							sh.c[3 * i + 2] += num7 * num6 * color[2] * array[i];
						}
						num += num6;
					}
				}
			}
			SHUtil.scale(ref sh, 16f / num);
		}

		// Token: 0x0600134E RID: 4942 RVA: 0x0006F22C File Offset: 0x0006D62C
		public static void convolve(ref SHEncoding sh)
		{
			SHUtil.convolve(ref sh, 1f, 0.6666667f, 0.25f);
		}

		// Token: 0x0600134F RID: 4943 RVA: 0x0006F244 File Offset: 0x0006D644
		public static void convolve(ref SHEncoding sh, float conv0, float conv1, float conv2)
		{
			for (int i = 0; i < 27; i++)
			{
				if (i < 3)
				{
					sh.c[i] *= conv0;
				}
				else if (i < 12)
				{
					sh.c[i] *= conv1;
				}
				else
				{
					sh.c[i] *= conv2;
				}
			}
		}
	}
}
