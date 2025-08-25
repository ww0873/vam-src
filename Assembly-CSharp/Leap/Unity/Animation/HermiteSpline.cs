using System;
using UnityEngine;

namespace Leap.Unity.Animation
{
	// Token: 0x02000632 RID: 1586
	[Serializable]
	public struct HermiteSpline
	{
		// Token: 0x060026E6 RID: 9958 RVA: 0x000D9F08 File Offset: 0x000D8308
		public HermiteSpline(float pos0, float pos1)
		{
			this.t0 = 0f;
			this.t1 = 1f;
			this.vel0 = 0f;
			this.vel1 = 0f;
			this.pos0 = pos0;
			this.pos1 = pos1;
		}

		// Token: 0x060026E7 RID: 9959 RVA: 0x000D9F44 File Offset: 0x000D8344
		public HermiteSpline(float pos0, float pos1, float vel0, float vel1)
		{
			this.t0 = 0f;
			this.t1 = 1f;
			this.vel0 = vel0;
			this.vel1 = vel1;
			this.pos0 = pos0;
			this.pos1 = pos1;
		}

		// Token: 0x060026E8 RID: 9960 RVA: 0x000D9F79 File Offset: 0x000D8379
		public HermiteSpline(float pos0, float pos1, float vel0, float vel1, float length)
		{
			this.t0 = 0f;
			this.t1 = length;
			this.vel0 = vel0;
			this.vel1 = vel1;
			this.pos0 = pos0;
			this.pos1 = pos1;
		}

		// Token: 0x060026E9 RID: 9961 RVA: 0x000D9FAB File Offset: 0x000D83AB
		public HermiteSpline(float t0, float t1, float pos0, float pos1, float vel0, float vel1)
		{
			this.t0 = t0;
			this.t1 = t1;
			this.vel0 = vel0;
			this.vel1 = vel1;
			this.pos0 = pos0;
			this.pos1 = pos1;
		}

		// Token: 0x060026EA RID: 9962 RVA: 0x000D9FDC File Offset: 0x000D83DC
		public float PositionAt(float t)
		{
			float num = Mathf.Clamp01((t - this.t0) / (this.t1 - this.t0));
			float num2 = num * num;
			float num3 = num2 * num;
			float num4 = (2f * num3 - 3f * num2 + 1f) * this.pos0;
			float num5 = (num3 - 2f * num2 + num) * (this.t1 - this.t0) * this.vel0;
			float num6 = (-2f * num3 + 3f * num2) * this.pos1;
			float num7 = (num3 - num2) * (this.t1 - this.t0) * this.vel1;
			return num4 + num5 + num6 + num7;
		}

		// Token: 0x060026EB RID: 9963 RVA: 0x000DA088 File Offset: 0x000D8488
		public float VelocityAt(float t)
		{
			float num = this.t1 - this.t0;
			float num2 = 1f / num;
			float num3 = Mathf.Clamp01((t - this.t0) * num2);
			float num4 = num2;
			float num5 = num3 * num3;
			float num6 = 2f * num3 * num4;
			float num7 = num6 * num3 + num4 * num5;
			float num8 = (num7 * 2f - num6 * 3f) * this.pos0;
			float num9 = (num7 - 2f * num6 + num4) * num * this.vel0;
			float num10 = (num6 * 3f - 2f * num7) * this.pos1;
			float num11 = (num7 - num6) * num * this.vel1;
			return num8 + num10 + num9 + num11;
		}

		// Token: 0x060026EC RID: 9964 RVA: 0x000DA144 File Offset: 0x000D8544
		public void PositionAndVelAt(float t, out float position, out float velocity)
		{
			float num = this.t1 - this.t0;
			float num2 = 1f / num;
			float num3 = Mathf.Clamp01((t - this.t0) * num2);
			float num4 = num2;
			float num5 = num3 * num3;
			float num6 = 2f * num3 * num4;
			float num7 = num5 * num3;
			float num8 = num6 * num3 + num4 * num5;
			float num9 = (2f * num7 - 3f * num5 + 1f) * this.pos0;
			float num10 = (num8 * 2f - num6 * 3f) * this.pos0;
			float num11 = (num7 - 2f * num5 + num3) * num * this.vel0;
			float num12 = (num8 - 2f * num6 + num4) * num * this.vel0;
			float num13 = (3f * num5 - 2f * num7) * this.pos1;
			float num14 = (num6 * 3f - 2f * num8) * this.pos1;
			float num15 = (num7 - num5) * num * this.vel1;
			float num16 = (num8 - num6) * num * this.vel1;
			position = num9 + num13 + num11 + num15;
			velocity = num10 + num14 + num12 + num16;
		}

		// Token: 0x040020F8 RID: 8440
		public float t0;

		// Token: 0x040020F9 RID: 8441
		public float t1;

		// Token: 0x040020FA RID: 8442
		public float pos0;

		// Token: 0x040020FB RID: 8443
		public float pos1;

		// Token: 0x040020FC RID: 8444
		public float vel0;

		// Token: 0x040020FD RID: 8445
		public float vel1;
	}
}
