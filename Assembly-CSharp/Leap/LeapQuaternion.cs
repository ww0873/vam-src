using System;
using LeapInternal;

namespace Leap
{
	// Token: 0x0200061D RID: 1565
	[Serializable]
	public struct LeapQuaternion : IEquatable<LeapQuaternion>
	{
		// Token: 0x0600265C RID: 9820 RVA: 0x000D7A21 File Offset: 0x000D5E21
		public LeapQuaternion(float x, float y, float z, float w)
		{
			this = default(LeapQuaternion);
			this.x = x;
			this.y = y;
			this.z = z;
			this.w = w;
		}

		// Token: 0x0600265D RID: 9821 RVA: 0x000D7A47 File Offset: 0x000D5E47
		public LeapQuaternion(LeapQuaternion quaternion)
		{
			this = default(LeapQuaternion);
			this.x = quaternion.x;
			this.y = quaternion.y;
			this.z = quaternion.z;
			this.w = quaternion.w;
		}

		// Token: 0x0600265E RID: 9822 RVA: 0x000D7A84 File Offset: 0x000D5E84
		public LeapQuaternion(LEAP_QUATERNION quaternion)
		{
			this = default(LeapQuaternion);
			this.x = quaternion.x;
			this.y = quaternion.y;
			this.z = quaternion.z;
			this.w = quaternion.w;
		}

		// Token: 0x0600265F RID: 9823 RVA: 0x000D7AC4 File Offset: 0x000D5EC4
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.x,
				", ",
				this.y,
				", ",
				this.z,
				", ",
				this.w,
				")"
			});
		}

		// Token: 0x06002660 RID: 9824 RVA: 0x000D7B40 File Offset: 0x000D5F40
		public bool Equals(LeapQuaternion v)
		{
			return this.x.NearlyEquals(v.x, 1.1920929E-07f) && this.y.NearlyEquals(v.y, 1.1920929E-07f) && this.z.NearlyEquals(v.z, 1.1920929E-07f) && this.w.NearlyEquals(v.w, 1.1920929E-07f);
		}

		// Token: 0x06002661 RID: 9825 RVA: 0x000D7BBB File Offset: 0x000D5FBB
		public override bool Equals(object obj)
		{
			return obj is LeapQuaternion && this.Equals((LeapQuaternion)obj);
		}

		// Token: 0x06002662 RID: 9826 RVA: 0x000D7BD8 File Offset: 0x000D5FD8
		public bool IsValid()
		{
			return !float.IsNaN(this.x) && !float.IsInfinity(this.x) && !float.IsNaN(this.y) && !float.IsInfinity(this.y) && !float.IsNaN(this.z) && !float.IsInfinity(this.z) && !float.IsNaN(this.w) && !float.IsInfinity(this.w);
		}

		// Token: 0x170004B0 RID: 1200
		// (get) Token: 0x06002663 RID: 9827 RVA: 0x000D7C66 File Offset: 0x000D6066
		public float Magnitude
		{
			get
			{
				return (float)Math.Sqrt((double)(this.x * this.x + this.y * this.y + this.z * this.z + this.w * this.w));
			}
		}

		// Token: 0x170004B1 RID: 1201
		// (get) Token: 0x06002664 RID: 9828 RVA: 0x000D7CA6 File Offset: 0x000D60A6
		public float MagnitudeSquared
		{
			get
			{
				return this.x * this.x + this.y * this.y + this.z * this.z + this.w * this.w;
			}
		}

		// Token: 0x170004B2 RID: 1202
		// (get) Token: 0x06002665 RID: 9829 RVA: 0x000D7CE0 File Offset: 0x000D60E0
		public LeapQuaternion Normalized
		{
			get
			{
				float num = this.MagnitudeSquared;
				if (num <= 1.1920929E-07f)
				{
					return LeapQuaternion.Identity;
				}
				num = 1f / (float)Math.Sqrt((double)num);
				return new LeapQuaternion(this.x * num, this.y * num, this.z * num, this.w * num);
			}
		}

		// Token: 0x06002666 RID: 9830 RVA: 0x000D7D3C File Offset: 0x000D613C
		public LeapQuaternion Multiply(LeapQuaternion rhs)
		{
			return new LeapQuaternion(this.w * rhs.x + this.x * rhs.w + this.y * rhs.z - this.z * rhs.y, this.w * rhs.y + this.y * rhs.w + this.z * rhs.x - this.x * rhs.z, this.w * rhs.z + this.z * rhs.w + this.x * rhs.y - this.y * rhs.x, this.w * rhs.w - this.x * rhs.x - this.y * rhs.y - this.z * rhs.z);
		}

		// Token: 0x06002667 RID: 9831 RVA: 0x000D7E3C File Offset: 0x000D623C
		public override int GetHashCode()
		{
			int num = 17;
			num = num * 23 + this.x.GetHashCode();
			num = num * 23 + this.y.GetHashCode();
			num = num * 23 + this.z.GetHashCode();
			return num * 23 + this.w.GetHashCode();
		}

		// Token: 0x06002668 RID: 9832 RVA: 0x000D7EA9 File Offset: 0x000D62A9
		// Note: this type is marked as 'beforefieldinit'.
		static LeapQuaternion()
		{
		}

		// Token: 0x040020AD RID: 8365
		public float x;

		// Token: 0x040020AE RID: 8366
		public float y;

		// Token: 0x040020AF RID: 8367
		public float z;

		// Token: 0x040020B0 RID: 8368
		public float w;

		// Token: 0x040020B1 RID: 8369
		public static readonly LeapQuaternion Identity = new LeapQuaternion(0f, 0f, 0f, 1f);
	}
}
