using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Wind
{
	// Token: 0x02000A85 RID: 2693
	public class Perlin2D
	{
		// Token: 0x060045C4 RID: 17860 RVA: 0x0013F898 File Offset: 0x0013DC98
		public Perlin2D(int seed = 0)
		{
			System.Random random = new System.Random(seed);
			this.permutationTable = new byte[1024];
			random.NextBytes(this.permutationTable);
		}

		// Token: 0x060045C5 RID: 17861 RVA: 0x0013F8CE File Offset: 0x0013DCCE
		private static float QunticCurve(float t)
		{
			return t * t * t * (t * (t * 6f - 15f) + 10f);
		}

		// Token: 0x060045C6 RID: 17862 RVA: 0x0013F8EC File Offset: 0x0013DCEC
		private Vector2 GetPseudoRandomGradientVector(int x, int y)
		{
			int num = (int)(((long)(x * 1836311903) ^ (long)y * (long)((ulong)-1323752223) + 4807526976L) & 1023L);
			switch (this.permutationTable[num] & 3)
			{
			case 0:
				return new Vector2(1f, 0f);
			case 1:
				return new Vector2(-1f, 0f);
			case 2:
				return new Vector2(0f, 1f);
			default:
				return new Vector2(0f, -1f);
			}
		}

		// Token: 0x060045C7 RID: 17863 RVA: 0x0013F980 File Offset: 0x0013DD80
		public float Noise(Vector2 fp)
		{
			int num = (int)Math.Floor((double)fp.x);
			int num2 = (int)Math.Floor((double)fp.y);
			float num3 = fp.x - (float)num;
			float num4 = fp.y - (float)num2;
			Vector2 pseudoRandomGradientVector = this.GetPseudoRandomGradientVector(num, num2);
			Vector2 pseudoRandomGradientVector2 = this.GetPseudoRandomGradientVector(num + 1, num2);
			Vector2 pseudoRandomGradientVector3 = this.GetPseudoRandomGradientVector(num, num2 + 1);
			Vector2 pseudoRandomGradientVector4 = this.GetPseudoRandomGradientVector(num + 1, num2 + 1);
			Vector2 v = new Vector2(num3, num4);
			Vector2 v2 = new Vector2(num3 - 1f, num4);
			Vector2 v3 = new Vector2(num3, num4 - 1f);
			Vector2 v4 = new Vector2(num3 - 1f, num4 - 1f);
			float a = Vector3.Dot(v, pseudoRandomGradientVector);
			float b = Vector3.Dot(v2, pseudoRandomGradientVector2);
			float a2 = Vector3.Dot(v3, pseudoRandomGradientVector3);
			float b2 = Vector3.Dot(v4, pseudoRandomGradientVector4);
			num3 = Perlin2D.QunticCurve(num3);
			num4 = Perlin2D.QunticCurve(num4);
			float a3 = Mathf.Lerp(a, b, num3);
			float b3 = Mathf.Lerp(a2, b2, num3);
			return Mathf.Lerp(a3, b3, num4);
		}

		// Token: 0x060045C8 RID: 17864 RVA: 0x0013FAB8 File Offset: 0x0013DEB8
		public float Noise(Vector2 fp, int octaves, float persistence = 0.5f)
		{
			float num = 1f;
			float num2 = 0f;
			float num3 = 0f;
			while (octaves-- > 0)
			{
				num2 += num;
				num3 += this.Noise(fp) * num;
				num *= persistence;
				fp.x *= 2f;
				fp.y *= 2f;
			}
			return num3 / num2;
		}

		// Token: 0x060045C9 RID: 17865 RVA: 0x0013FB28 File Offset: 0x0013DF28
		public float Noise(Vector2 fp, List<NoiseOctave> octaves)
		{
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < octaves.Count; i++)
			{
				NoiseOctave noiseOctave = octaves[i];
				num2 += noiseOctave.Amplitude;
				num += this.Noise(fp * noiseOctave.Scale) * noiseOctave.Amplitude;
			}
			return num / num2;
		}

		// Token: 0x04003383 RID: 13187
		private readonly byte[] permutationTable;
	}
}
