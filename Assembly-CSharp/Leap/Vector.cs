using System;

namespace Leap
{
	// Token: 0x0200062C RID: 1580
	[Serializable]
	public struct Vector : IEquatable<Vector>
	{
		// Token: 0x060026BB RID: 9915 RVA: 0x000D94F0 File Offset: 0x000D78F0
		public Vector(float x, float y, float z)
		{
			this = default(Vector);
			this.x = x;
			this.y = y;
			this.z = z;
		}

		// Token: 0x060026BC RID: 9916 RVA: 0x000D950E File Offset: 0x000D790E
		public Vector(Vector vector)
		{
			this = default(Vector);
			this.x = vector.x;
			this.y = vector.y;
			this.z = vector.z;
		}

		// Token: 0x060026BD RID: 9917 RVA: 0x000D953E File Offset: 0x000D793E
		public static Vector operator +(Vector v1, Vector v2)
		{
			return new Vector(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
		}

		// Token: 0x060026BE RID: 9918 RVA: 0x000D9572 File Offset: 0x000D7972
		public static Vector operator -(Vector v1, Vector v2)
		{
			return new Vector(v1.x - v2.x, v1.y - v2.y, v1.z - v2.z);
		}

		// Token: 0x060026BF RID: 9919 RVA: 0x000D95A6 File Offset: 0x000D79A6
		public static Vector operator *(Vector v1, float scalar)
		{
			return new Vector(v1.x * scalar, v1.y * scalar, v1.z * scalar);
		}

		// Token: 0x060026C0 RID: 9920 RVA: 0x000D95C8 File Offset: 0x000D79C8
		public static Vector operator *(float scalar, Vector v1)
		{
			return new Vector(v1.x * scalar, v1.y * scalar, v1.z * scalar);
		}

		// Token: 0x060026C1 RID: 9921 RVA: 0x000D95EA File Offset: 0x000D79EA
		public static Vector operator /(Vector v1, float scalar)
		{
			return new Vector(v1.x / scalar, v1.y / scalar, v1.z / scalar);
		}

		// Token: 0x060026C2 RID: 9922 RVA: 0x000D960C File Offset: 0x000D7A0C
		public static Vector operator -(Vector v1)
		{
			return new Vector(-v1.x, -v1.y, -v1.z);
		}

		// Token: 0x060026C3 RID: 9923 RVA: 0x000D962B File Offset: 0x000D7A2B
		public static bool operator ==(Vector v1, Vector v2)
		{
			return v1.Equals(v2);
		}

		// Token: 0x060026C4 RID: 9924 RVA: 0x000D9635 File Offset: 0x000D7A35
		public static bool operator !=(Vector v1, Vector v2)
		{
			return !v1.Equals(v2);
		}

		// Token: 0x060026C5 RID: 9925 RVA: 0x000D9642 File Offset: 0x000D7A42
		public float[] ToFloatArray()
		{
			return new float[]
			{
				this.x,
				this.y,
				this.z
			};
		}

		// Token: 0x060026C6 RID: 9926 RVA: 0x000D9668 File Offset: 0x000D7A68
		public float DistanceTo(Vector other)
		{
			return (float)Math.Sqrt((double)((this.x - other.x) * (this.x - other.x) + (this.y - other.y) * (this.y - other.y) + (this.z - other.z) * (this.z - other.z)));
		}

		// Token: 0x060026C7 RID: 9927 RVA: 0x000D96D8 File Offset: 0x000D7AD8
		public float AngleTo(Vector other)
		{
			float num = this.MagnitudeSquared * other.MagnitudeSquared;
			if (num <= 1.1920929E-07f)
			{
				return 0f;
			}
			float num2 = this.Dot(other) / (float)Math.Sqrt((double)num);
			if (num2 >= 1f)
			{
				return 0f;
			}
			if (num2 <= -1f)
			{
				return 3.1415927f;
			}
			return (float)Math.Acos((double)num2);
		}

		// Token: 0x060026C8 RID: 9928 RVA: 0x000D9740 File Offset: 0x000D7B40
		public float Dot(Vector other)
		{
			return this.x * other.x + this.y * other.y + this.z * other.z;
		}

		// Token: 0x060026C9 RID: 9929 RVA: 0x000D9770 File Offset: 0x000D7B70
		public Vector Cross(Vector other)
		{
			return new Vector(this.y * other.z - this.z * other.y, this.z * other.x - this.x * other.z, this.x * other.y - this.y * other.x);
		}

		// Token: 0x060026CA RID: 9930 RVA: 0x000D97DC File Offset: 0x000D7BDC
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
				")"
			});
		}

		// Token: 0x060026CB RID: 9931 RVA: 0x000D9840 File Offset: 0x000D7C40
		public bool Equals(Vector v)
		{
			return this.x.NearlyEquals(v.x, 1.1920929E-07f) && this.y.NearlyEquals(v.y, 1.1920929E-07f) && this.z.NearlyEquals(v.z, 1.1920929E-07f);
		}

		// Token: 0x060026CC RID: 9932 RVA: 0x000D989F File Offset: 0x000D7C9F
		public override bool Equals(object obj)
		{
			return obj is Vector && this.Equals((Vector)obj);
		}

		// Token: 0x060026CD RID: 9933 RVA: 0x000D98BC File Offset: 0x000D7CBC
		public bool IsValid()
		{
			return !float.IsNaN(this.x) && !float.IsInfinity(this.x) && !float.IsNaN(this.y) && !float.IsInfinity(this.y) && !float.IsNaN(this.z) && !float.IsInfinity(this.z);
		}

		// Token: 0x170004BE RID: 1214
		public float this[uint index]
		{
			get
			{
				if (index == 0U)
				{
					return this.x;
				}
				if (index == 1U)
				{
					return this.y;
				}
				if (index == 2U)
				{
					return this.z;
				}
				throw new IndexOutOfRangeException();
			}
			set
			{
				if (index == 0U)
				{
					this.x = value;
				}
				if (index == 1U)
				{
					this.y = value;
				}
				if (index == 2U)
				{
					this.z = value;
				}
				throw new IndexOutOfRangeException();
			}
		}

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x060026D0 RID: 9936 RVA: 0x000D998A File Offset: 0x000D7D8A
		public float Magnitude
		{
			get
			{
				return (float)Math.Sqrt((double)(this.x * this.x + this.y * this.y + this.z * this.z));
			}
		}

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x060026D1 RID: 9937 RVA: 0x000D99BC File Offset: 0x000D7DBC
		public float MagnitudeSquared
		{
			get
			{
				return this.x * this.x + this.y * this.y + this.z * this.z;
			}
		}

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x060026D2 RID: 9938 RVA: 0x000D99E7 File Offset: 0x000D7DE7
		public float Pitch
		{
			get
			{
				return (float)Math.Atan2((double)this.y, (double)(-(double)this.z));
			}
		}

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x060026D3 RID: 9939 RVA: 0x000D99FE File Offset: 0x000D7DFE
		public float Roll
		{
			get
			{
				return (float)Math.Atan2((double)this.x, (double)(-(double)this.y));
			}
		}

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x060026D4 RID: 9940 RVA: 0x000D9A15 File Offset: 0x000D7E15
		public float Yaw
		{
			get
			{
				return (float)Math.Atan2((double)this.x, (double)(-(double)this.z));
			}
		}

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x060026D5 RID: 9941 RVA: 0x000D9A2C File Offset: 0x000D7E2C
		public Vector Normalized
		{
			get
			{
				float num = this.MagnitudeSquared;
				if (num <= 1.1920929E-07f)
				{
					return Vector.Zero;
				}
				num = 1f / (float)Math.Sqrt((double)num);
				return new Vector(this.x * num, this.y * num, this.z * num);
			}
		}

		// Token: 0x060026D6 RID: 9942 RVA: 0x000D9A80 File Offset: 0x000D7E80
		public static Vector Lerp(Vector a, Vector b, float t)
		{
			return new Vector(a.x + t * (b.x - a.x), a.y + t * (b.y - a.y), a.z + t * (b.z - a.z));
		}

		// Token: 0x060026D7 RID: 9943 RVA: 0x000D9AE0 File Offset: 0x000D7EE0
		public override int GetHashCode()
		{
			int num = 17;
			num = num * 23 + this.x.GetHashCode();
			num = num * 23 + this.y.GetHashCode();
			return num * 23 + this.z.GetHashCode();
		}

		// Token: 0x060026D8 RID: 9944 RVA: 0x000D9B38 File Offset: 0x000D7F38
		// Note: this type is marked as 'beforefieldinit'.
		static Vector()
		{
		}

		// Token: 0x040020DC RID: 8412
		public float x;

		// Token: 0x040020DD RID: 8413
		public float y;

		// Token: 0x040020DE RID: 8414
		public float z;

		// Token: 0x040020DF RID: 8415
		public static readonly Vector Zero = new Vector(0f, 0f, 0f);

		// Token: 0x040020E0 RID: 8416
		public static readonly Vector Ones = new Vector(1f, 1f, 1f);

		// Token: 0x040020E1 RID: 8417
		public static readonly Vector XAxis = new Vector(1f, 0f, 0f);

		// Token: 0x040020E2 RID: 8418
		public static readonly Vector YAxis = new Vector(0f, 1f, 0f);

		// Token: 0x040020E3 RID: 8419
		public static readonly Vector ZAxis = new Vector(0f, 0f, 1f);

		// Token: 0x040020E4 RID: 8420
		public static readonly Vector Forward = new Vector(0f, 0f, -1f);

		// Token: 0x040020E5 RID: 8421
		public static readonly Vector Backward = new Vector(0f, 0f, 1f);

		// Token: 0x040020E6 RID: 8422
		public static readonly Vector Left = new Vector(-1f, 0f, 0f);

		// Token: 0x040020E7 RID: 8423
		public static readonly Vector Right = new Vector(1f, 0f, 0f);

		// Token: 0x040020E8 RID: 8424
		public static readonly Vector Up = new Vector(0f, 1f, 0f);

		// Token: 0x040020E9 RID: 8425
		public static readonly Vector Down = new Vector(0f, -1f, 0f);
	}
}
