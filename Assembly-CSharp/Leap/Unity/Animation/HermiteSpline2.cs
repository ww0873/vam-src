using System;
using UnityEngine;

namespace Leap.Unity.Animation
{
	// Token: 0x02000633 RID: 1587
	[Serializable]
	public struct HermiteSpline2
	{
		// Token: 0x060026ED RID: 9965 RVA: 0x000DA270 File Offset: 0x000D8670
		public HermiteSpline2(Vector2 pos0, Vector2 pos1)
		{
			this.t0 = 0f;
			this.t1 = 1f;
			this.vel0 = default(Vector2);
			this.vel1 = default(Vector2);
			this.pos0 = pos0;
			this.pos1 = pos1;
		}

		// Token: 0x060026EE RID: 9966 RVA: 0x000DA2BF File Offset: 0x000D86BF
		public HermiteSpline2(Vector2 pos0, Vector2 pos1, Vector2 vel0, Vector2 vel1)
		{
			this.t0 = 0f;
			this.t1 = 1f;
			this.vel0 = vel0;
			this.vel1 = vel1;
			this.pos0 = pos0;
			this.pos1 = pos1;
		}

		// Token: 0x060026EF RID: 9967 RVA: 0x000DA2F4 File Offset: 0x000D86F4
		public HermiteSpline2(Vector2 pos0, Vector2 pos1, Vector2 vel0, Vector2 vel1, float length)
		{
			this.t0 = 0f;
			this.t1 = length;
			this.vel0 = vel0;
			this.vel1 = vel1;
			this.pos0 = pos0;
			this.pos1 = pos1;
		}

		// Token: 0x060026F0 RID: 9968 RVA: 0x000DA326 File Offset: 0x000D8726
		public HermiteSpline2(float t0, float t1, Vector2 pos0, Vector2 pos1, Vector2 vel0, Vector2 vel1)
		{
			this.t0 = t0;
			this.t1 = t1;
			this.vel0 = vel0;
			this.vel1 = vel1;
			this.pos0 = pos0;
			this.pos1 = pos1;
		}

		// Token: 0x060026F1 RID: 9969 RVA: 0x000DA358 File Offset: 0x000D8758
		public Vector2 PositionAt(float t)
		{
			float num = Mathf.Clamp01((t - this.t0) / (this.t1 - this.t0));
			float num2 = num * num;
			float num3 = num2 * num;
			Vector2 a = (2f * num3 - 3f * num2 + 1f) * this.pos0;
			Vector2 b = (num3 - 2f * num2 + num) * (this.t1 - this.t0) * this.vel0;
			Vector2 b2 = (-2f * num3 + 3f * num2) * this.pos1;
			Vector2 b3 = (num3 - num2) * (this.t1 - this.t0) * this.vel1;
			return a + b + b2 + b3;
		}

		// Token: 0x060026F2 RID: 9970 RVA: 0x000DA420 File Offset: 0x000D8820
		public Vector2 VelocityAt(float t)
		{
			float num = this.t1 - this.t0;
			float num2 = 1f / num;
			float num3 = Mathf.Clamp01((t - this.t0) * num2);
			float num4 = num2;
			float num5 = num3 * num3;
			float num6 = 2f * num3 * num4;
			float num7 = num6 * num3 + num4 * num5;
			Vector2 a = (num7 * 2f - num6 * 3f) * this.pos0;
			Vector2 b = (num7 - 2f * num6 + num4) * num * this.vel0;
			Vector2 b2 = (num6 * 3f - 2f * num7) * this.pos1;
			Vector2 b3 = (num7 - num6) * num * this.vel1;
			return a + b2 + b + b3;
		}

		// Token: 0x060026F3 RID: 9971 RVA: 0x000DA4F8 File Offset: 0x000D88F8
		public void PositionAndVelAt(float t, out Vector2 position, out Vector2 velocity)
		{
			float num = this.t1 - this.t0;
			float num2 = 1f / num;
			float num3 = Mathf.Clamp01((t - this.t0) * num2);
			float num4 = num2;
			float num5 = num3 * num3;
			float num6 = 2f * num3 * num4;
			float num7 = num5 * num3;
			float num8 = num6 * num3 + num4 * num5;
			Vector2 a = (2f * num7 - 3f * num5 + 1f) * this.pos0;
			Vector2 a2 = (num8 * 2f - num6 * 3f) * this.pos0;
			Vector2 b = (num7 - 2f * num5 + num3) * num * this.vel0;
			Vector2 b2 = (num8 - 2f * num6 + num4) * num * this.vel0;
			Vector2 b3 = (3f * num5 - 2f * num7) * this.pos1;
			Vector2 b4 = (num6 * 3f - 2f * num8) * this.pos1;
			Vector2 b5 = (num7 - num5) * num * this.vel1;
			Vector2 b6 = (num8 - num6) * num * this.vel1;
			position = a + b3 + b + b5;
			velocity = a2 + b4 + b2 + b6;
		}

		// Token: 0x040020FE RID: 8446
		public float t0;

		// Token: 0x040020FF RID: 8447
		public float t1;

		// Token: 0x04002100 RID: 8448
		public Vector2 pos0;

		// Token: 0x04002101 RID: 8449
		public Vector2 pos1;

		// Token: 0x04002102 RID: 8450
		public Vector2 vel0;

		// Token: 0x04002103 RID: 8451
		public Vector2 vel1;
	}
}
