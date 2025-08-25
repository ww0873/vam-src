using System;
using UnityEngine;

namespace mset
{
	// Token: 0x0200032E RID: 814
	[Serializable]
	public class SHEncoding
	{
		// Token: 0x0600133A RID: 4922 RVA: 0x0006EBE1 File Offset: 0x0006CFE1
		public SHEncoding()
		{
			this.clearToBlack();
		}

		// Token: 0x0600133B RID: 4923 RVA: 0x0006EC0C File Offset: 0x0006D00C
		public void clearToBlack()
		{
			for (int i = 0; i < 27; i++)
			{
				this.c[i] = 0f;
			}
			for (int j = 0; j < 9; j++)
			{
				this.cBuffer[j] = Vector4.zero;
			}
		}

		// Token: 0x0600133C RID: 4924 RVA: 0x0006EC64 File Offset: 0x0006D064
		public bool equals(SHEncoding other)
		{
			for (int i = 0; i < 27; i++)
			{
				if (this.c[i] != other.c[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600133D RID: 4925 RVA: 0x0006EC9C File Offset: 0x0006D09C
		public void copyFrom(SHEncoding src)
		{
			for (int i = 0; i < 27; i++)
			{
				this.c[i] = src.c[i];
			}
			this.copyToBuffer();
		}

		// Token: 0x0600133E RID: 4926 RVA: 0x0006ECD4 File Offset: 0x0006D0D4
		public void copyToBuffer()
		{
			for (int i = 0; i < 9; i++)
			{
				float num = SHEncoding.sEquationConstants[i];
				this.cBuffer[i].x = this.c[i * 3] * num;
				this.cBuffer[i].y = this.c[i * 3 + 1] * num;
				this.cBuffer[i].z = this.c[i * 3 + 2] * num;
			}
		}

		// Token: 0x0600133F RID: 4927 RVA: 0x0006ED57 File Offset: 0x0006D157
		// Note: this type is marked as 'beforefieldinit'.
		static SHEncoding()
		{
		}

		// Token: 0x040010DE RID: 4318
		public float[] c = new float[27];

		// Token: 0x040010DF RID: 4319
		public Vector4[] cBuffer = new Vector4[9];

		// Token: 0x040010E0 RID: 4320
		public static float[] sEquationConstants = new float[]
		{
			0.28209478f,
			0.4886025f,
			0.4886025f,
			0.4886025f,
			1.0925485f,
			1.0925485f,
			0.31539157f,
			1.0925485f,
			0.54627424f
		};
	}
}
