using System;

namespace Technie.PhysicsCreator.QHull
{
	// Token: 0x0200045F RID: 1119
	public class Vector3d
	{
		// Token: 0x06001BF3 RID: 7155 RVA: 0x0009C721 File Offset: 0x0009AB21
		public Vector3d()
		{
		}

		// Token: 0x06001BF4 RID: 7156 RVA: 0x0009C729 File Offset: 0x0009AB29
		public Vector3d(Vector3d v)
		{
			this.set(v);
		}

		// Token: 0x06001BF5 RID: 7157 RVA: 0x0009C738 File Offset: 0x0009AB38
		public Vector3d(double x, double y, double z)
		{
			this.set(x, y, z);
		}

		// Token: 0x06001BF6 RID: 7158 RVA: 0x0009C74C File Offset: 0x0009AB4C
		public double get(int i)
		{
			switch (i)
			{
			case 0:
				return this.x;
			case 1:
				return this.y;
			case 2:
				return this.z;
			default:
				throw new IndexOutOfRangeException(string.Empty + i);
			}
		}

		// Token: 0x06001BF7 RID: 7159 RVA: 0x0009C79C File Offset: 0x0009AB9C
		public void set(int i, double value)
		{
			switch (i)
			{
			case 0:
				this.x = value;
				break;
			case 1:
				this.y = value;
				break;
			case 2:
				this.z = value;
				break;
			default:
				throw new IndexOutOfRangeException(string.Empty + i);
			}
		}

		// Token: 0x06001BF8 RID: 7160 RVA: 0x0009C7FA File Offset: 0x0009ABFA
		public void set(Vector3d v1)
		{
			this.x = v1.x;
			this.y = v1.y;
			this.z = v1.z;
		}

		// Token: 0x06001BF9 RID: 7161 RVA: 0x0009C820 File Offset: 0x0009AC20
		public void add(Vector3d v1, Vector3d v2)
		{
			this.x = v1.x + v2.x;
			this.y = v1.y + v2.y;
			this.z = v1.z + v2.z;
		}

		// Token: 0x06001BFA RID: 7162 RVA: 0x0009C85B File Offset: 0x0009AC5B
		public void add(Vector3d v1)
		{
			this.x += v1.x;
			this.y += v1.y;
			this.z += v1.z;
		}

		// Token: 0x06001BFB RID: 7163 RVA: 0x0009C896 File Offset: 0x0009AC96
		public void sub(Vector3d v1, Vector3d v2)
		{
			this.x = v1.x - v2.x;
			this.y = v1.y - v2.y;
			this.z = v1.z - v2.z;
		}

		// Token: 0x06001BFC RID: 7164 RVA: 0x0009C8D1 File Offset: 0x0009ACD1
		public void sub(Vector3d v1)
		{
			this.x -= v1.x;
			this.y -= v1.y;
			this.z -= v1.z;
		}

		// Token: 0x06001BFD RID: 7165 RVA: 0x0009C90C File Offset: 0x0009AD0C
		public void scale(double s)
		{
			this.x = s * this.x;
			this.y = s * this.y;
			this.z = s * this.z;
		}

		// Token: 0x06001BFE RID: 7166 RVA: 0x0009C938 File Offset: 0x0009AD38
		public void scale(double s, Vector3d v1)
		{
			this.x = s * v1.x;
			this.y = s * v1.y;
			this.z = s * v1.z;
		}

		// Token: 0x06001BFF RID: 7167 RVA: 0x0009C964 File Offset: 0x0009AD64
		public double norm()
		{
			return Math.Sqrt(this.x * this.x + this.y * this.y + this.z * this.z);
		}

		// Token: 0x06001C00 RID: 7168 RVA: 0x0009C994 File Offset: 0x0009AD94
		public double normSquared()
		{
			return this.x * this.x + this.y * this.y + this.z * this.z;
		}

		// Token: 0x06001C01 RID: 7169 RVA: 0x0009C9C0 File Offset: 0x0009ADC0
		public double distance(Vector3d v)
		{
			double num = this.x - v.x;
			double num2 = this.y - v.y;
			double num3 = this.z - v.z;
			return Math.Sqrt(num * num + num2 * num2 + num3 * num3);
		}

		// Token: 0x06001C02 RID: 7170 RVA: 0x0009CA08 File Offset: 0x0009AE08
		public double distanceSquared(Vector3d v)
		{
			double num = this.x - v.x;
			double num2 = this.y - v.y;
			double num3 = this.z - v.z;
			return num * num + num2 * num2 + num3 * num3;
		}

		// Token: 0x06001C03 RID: 7171 RVA: 0x0009CA4A File Offset: 0x0009AE4A
		public double dot(Vector3d v1)
		{
			return this.x * v1.x + this.y * v1.y + this.z * v1.z;
		}

		// Token: 0x06001C04 RID: 7172 RVA: 0x0009CA78 File Offset: 0x0009AE78
		public void normalize()
		{
			double num = this.x * this.x + this.y * this.y + this.z * this.z;
			double num2 = num - 1.0;
			if (num2 > 4.440892098500626E-16 || num2 < -4.440892098500626E-16)
			{
				double num3 = Math.Sqrt(num);
				this.x /= num3;
				this.y /= num3;
				this.z /= num3;
			}
		}

		// Token: 0x06001C05 RID: 7173 RVA: 0x0009CB0A File Offset: 0x0009AF0A
		public void setZero()
		{
			this.x = 0.0;
			this.y = 0.0;
			this.z = 0.0;
		}

		// Token: 0x06001C06 RID: 7174 RVA: 0x0009CB39 File Offset: 0x0009AF39
		public void set(double x, double y, double z)
		{
			this.x = x;
			this.y = y;
			this.z = z;
		}

		// Token: 0x06001C07 RID: 7175 RVA: 0x0009CB50 File Offset: 0x0009AF50
		public void cross(Vector3d v1, Vector3d v2)
		{
			double num = v1.y * v2.z - v1.z * v2.y;
			double num2 = v1.z * v2.x - v1.x * v2.z;
			double num3 = v1.x * v2.y - v1.y * v2.x;
			this.x = num;
			this.y = num2;
			this.z = num3;
		}

		// Token: 0x06001C08 RID: 7176 RVA: 0x0009CBC8 File Offset: 0x0009AFC8
		protected void setRandom(double lower, double upper, Random generator)
		{
			double num = upper - lower;
			this.x = generator.NextDouble() * num + lower;
			this.y = generator.NextDouble() * num + lower;
			this.z = generator.NextDouble() * num + lower;
		}

		// Token: 0x06001C09 RID: 7177 RVA: 0x0009CC0C File Offset: 0x0009B00C
		public string toString()
		{
			return string.Concat(new object[]
			{
				this.x,
				" ",
				this.y,
				" ",
				this.z
			});
		}

		// Token: 0x040017C2 RID: 6082
		private const double DOUBLE_PREC = 2.220446049250313E-16;

		// Token: 0x040017C3 RID: 6083
		public double x;

		// Token: 0x040017C4 RID: 6084
		public double y;

		// Token: 0x040017C5 RID: 6085
		public double z;
	}
}
