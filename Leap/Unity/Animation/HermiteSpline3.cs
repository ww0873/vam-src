using System;
using UnityEngine;

namespace Leap.Unity.Animation
{
	// Token: 0x02000634 RID: 1588
	[Serializable]
	public struct HermiteSpline3
	{
		// Token: 0x060026F4 RID: 9972 RVA: 0x000DA664 File Offset: 0x000D8A64
		public HermiteSpline3(Vector3 pos0, Vector3 pos1)
		{
			this.t0 = 0f;
			this.t1 = 1f;
			this.vel0 = default(Vector3);
			this.vel1 = default(Vector3);
			this.pos0 = pos0;
			this.pos1 = pos1;
		}

		// Token: 0x060026F5 RID: 9973 RVA: 0x000DA6B3 File Offset: 0x000D8AB3
		public HermiteSpline3(Vector3 pos0, Vector3 pos1, Vector3 vel0, Vector3 vel1)
		{
			this.t0 = 0f;
			this.t1 = 1f;
			this.vel0 = vel0;
			this.vel1 = vel1;
			this.pos0 = pos0;
			this.pos1 = pos1;
		}

		// Token: 0x060026F6 RID: 9974 RVA: 0x000DA6E8 File Offset: 0x000D8AE8
		public HermiteSpline3(Vector3 pos0, Vector3 pos1, Vector3 vel0, Vector3 vel1, float length)
		{
			this.t0 = 0f;
			this.t1 = length;
			this.vel0 = vel0;
			this.vel1 = vel1;
			this.pos0 = pos0;
			this.pos1 = pos1;
		}

		// Token: 0x060026F7 RID: 9975 RVA: 0x000DA71A File Offset: 0x000D8B1A
		public HermiteSpline3(float t0, float t1, Vector3 pos0, Vector3 pos1, Vector3 vel0, Vector3 vel1)
		{
			this.t0 = t0;
			this.t1 = t1;
			this.vel0 = vel0;
			this.vel1 = vel1;
			this.pos0 = pos0;
			this.pos1 = pos1;
		}

		// Token: 0x060026F8 RID: 9976 RVA: 0x000DA74C File Offset: 0x000D8B4C
		public Vector3 PositionAt(float t)
		{
			float num = Mathf.Clamp01((t - this.t0) / (this.t1 - this.t0));
			float num2 = num * num;
			float num3 = num2 * num;
			Vector3 a = (2f * num3 - 3f * num2 + 1f) * this.pos0;
			Vector3 b = (num3 - 2f * num2 + num) * (this.t1 - this.t0) * this.vel0;
			Vector3 b2 = (-2f * num3 + 3f * num2) * this.pos1;
			Vector3 b3 = (num3 - num2) * (this.t1 - this.t0) * this.vel1;
			return a + b + b2 + b3;
		}

		// Token: 0x060026F9 RID: 9977 RVA: 0x000DA814 File Offset: 0x000D8C14
		public Vector3 VelocityAt(float t)
		{
			float num = this.t1 - this.t0;
			float num2 = 1f / num;
			float num3 = Mathf.Clamp01((t - this.t0) * num2);
			float num4 = num2;
			float num5 = num3 * num3;
			float num6 = 2f * num3 * num4;
			float num7 = num6 * num3 + num4 * num5;
			Vector3 a = (num7 * 2f - num6 * 3f) * this.pos0;
			Vector3 b = (num7 - 2f * num6 + num4) * num * this.vel0;
			Vector3 b2 = (num6 * 3f - 2f * num7) * this.pos1;
			Vector3 b3 = (num7 - num6) * num * this.vel1;
			return a + b2 + b + b3;
		}

		// Token: 0x060026FA RID: 9978 RVA: 0x000DA8EC File Offset: 0x000D8CEC
		public void PositionAndVelAt(float t, out Vector3 position, out Vector3 velocity)
		{
			float num = this.t1 - this.t0;
			float num2 = 1f / num;
			float num3 = Mathf.Clamp01((t - this.t0) * num2);
			float num4 = num2;
			float num5 = num3 * num3;
			float num6 = 2f * num3 * num4;
			float num7 = num5 * num3;
			float num8 = num6 * num3 + num4 * num5;
			Vector3 a = (2f * num7 - 3f * num5 + 1f) * this.pos0;
			Vector3 a2 = (num8 * 2f - num6 * 3f) * this.pos0;
			Vector3 b = (num7 - 2f * num5 + num3) * num * this.vel0;
			Vector3 b2 = (num8 - 2f * num6 + num4) * num * this.vel0;
			Vector3 b3 = (3f * num5 - 2f * num7) * this.pos1;
			Vector3 b4 = (num6 * 3f - 2f * num8) * this.pos1;
			Vector3 b5 = (num7 - num5) * num * this.vel1;
			Vector3 b6 = (num8 - num6) * num * this.vel1;
			position = a + b3 + b + b5;
			velocity = a2 + b4 + b2 + b6;
		}

		// Token: 0x04002104 RID: 8452
		public float t0;

		// Token: 0x04002105 RID: 8453
		public float t1;

		// Token: 0x04002106 RID: 8454
		public Vector3 pos0;

		// Token: 0x04002107 RID: 8455
		public Vector3 pos1;

		// Token: 0x04002108 RID: 8456
		public Vector3 vel0;

		// Token: 0x04002109 RID: 8457
		public Vector3 vel1;
	}
}
