using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Leap
{
	// Token: 0x02000620 RID: 1568
	public struct Matrix
	{
		// Token: 0x06002680 RID: 9856 RVA: 0x000D853C File Offset: 0x000D693C
		public Matrix(Matrix other)
		{
			this = default(Matrix);
			this.xBasis = other.xBasis;
			this.yBasis = other.yBasis;
			this.zBasis = other.zBasis;
			this.origin = other.origin;
		}

		// Token: 0x06002681 RID: 9857 RVA: 0x000D8579 File Offset: 0x000D6979
		public Matrix(Vector xBasis, Vector yBasis, Vector zBasis)
		{
			this = default(Matrix);
			this.xBasis = xBasis;
			this.yBasis = yBasis;
			this.zBasis = zBasis;
			this.origin = Vector.Zero;
		}

		// Token: 0x06002682 RID: 9858 RVA: 0x000D85A2 File Offset: 0x000D69A2
		public Matrix(Vector xBasis, Vector yBasis, Vector zBasis, Vector origin)
		{
			this = default(Matrix);
			this.xBasis = xBasis;
			this.yBasis = yBasis;
			this.zBasis = zBasis;
			this.origin = origin;
		}

		// Token: 0x06002683 RID: 9859 RVA: 0x000D85C8 File Offset: 0x000D69C8
		public Matrix(Vector axis, float angleRadians)
		{
			this = default(Matrix);
			this.xBasis = Vector.XAxis;
			this.yBasis = Vector.YAxis;
			this.zBasis = Vector.ZAxis;
			this.origin = Vector.Zero;
			this.SetRotation(axis, angleRadians);
		}

		// Token: 0x06002684 RID: 9860 RVA: 0x000D8605 File Offset: 0x000D6A05
		public Matrix(Vector axis, float angleRadians, Vector translation)
		{
			this = default(Matrix);
			this.xBasis = Vector.XAxis;
			this.yBasis = Vector.YAxis;
			this.zBasis = Vector.ZAxis;
			this.origin = translation;
			this.SetRotation(axis, angleRadians);
		}

		// Token: 0x06002685 RID: 9861 RVA: 0x000D8640 File Offset: 0x000D6A40
		public Matrix(float m00, float m01, float m02, float m10, float m11, float m12, float m20, float m21, float m22)
		{
			this = default(Matrix);
			this.xBasis = new Vector(m00, m01, m02);
			this.yBasis = new Vector(m10, m11, m12);
			this.zBasis = new Vector(m20, m21, m22);
			this.origin = Vector.Zero;
		}

		// Token: 0x06002686 RID: 9862 RVA: 0x000D8690 File Offset: 0x000D6A90
		public Matrix(float m00, float m01, float m02, float m10, float m11, float m12, float m20, float m21, float m22, float m30, float m31, float m32)
		{
			this = default(Matrix);
			this.xBasis = new Vector(m00, m01, m02);
			this.yBasis = new Vector(m10, m11, m12);
			this.zBasis = new Vector(m20, m21, m22);
			this.origin = new Vector(m30, m31, m32);
		}

		// Token: 0x06002687 RID: 9863 RVA: 0x000D86E5 File Offset: 0x000D6AE5
		public static Matrix operator *(Matrix m1, Matrix m2)
		{
			return m1._operator_mul(m2);
		}

		// Token: 0x06002688 RID: 9864 RVA: 0x000D86F0 File Offset: 0x000D6AF0
		public float[] ToArray3x3(float[] output)
		{
			output[0] = this.xBasis.x;
			output[1] = this.xBasis.y;
			output[2] = this.xBasis.z;
			output[3] = this.yBasis.x;
			output[4] = this.yBasis.y;
			output[5] = this.yBasis.z;
			output[6] = this.zBasis.x;
			output[7] = this.zBasis.y;
			output[8] = this.zBasis.z;
			return output;
		}

		// Token: 0x06002689 RID: 9865 RVA: 0x000D879C File Offset: 0x000D6B9C
		public double[] ToArray3x3(double[] output)
		{
			output[0] = (double)this.xBasis.x;
			output[1] = (double)this.xBasis.y;
			output[2] = (double)this.xBasis.z;
			output[3] = (double)this.yBasis.x;
			output[4] = (double)this.yBasis.y;
			output[5] = (double)this.yBasis.z;
			output[6] = (double)this.zBasis.x;
			output[7] = (double)this.zBasis.y;
			output[8] = (double)this.zBasis.z;
			return output;
		}

		// Token: 0x0600268A RID: 9866 RVA: 0x000D8851 File Offset: 0x000D6C51
		public float[] ToArray3x3()
		{
			return this.ToArray3x3(new float[9]);
		}

		// Token: 0x0600268B RID: 9867 RVA: 0x000D8860 File Offset: 0x000D6C60
		public float[] ToArray4x4(float[] output)
		{
			output[0] = this.xBasis.x;
			output[1] = this.xBasis.y;
			output[2] = this.xBasis.z;
			output[3] = 0f;
			output[4] = this.yBasis.x;
			output[5] = this.yBasis.y;
			output[6] = this.yBasis.z;
			output[7] = 0f;
			output[8] = this.zBasis.x;
			output[9] = this.zBasis.y;
			output[10] = this.zBasis.z;
			output[11] = 0f;
			output[12] = this.origin.x;
			output[13] = this.origin.y;
			output[14] = this.origin.z;
			output[15] = 1f;
			return output;
		}

		// Token: 0x0600268C RID: 9868 RVA: 0x000D896C File Offset: 0x000D6D6C
		public double[] ToArray4x4(double[] output)
		{
			output[0] = (double)this.xBasis.x;
			output[1] = (double)this.xBasis.y;
			output[2] = (double)this.xBasis.z;
			output[3] = 0.0;
			output[4] = (double)this.yBasis.x;
			output[5] = (double)this.yBasis.y;
			output[6] = (double)this.yBasis.z;
			output[7] = 0.0;
			output[8] = (double)this.zBasis.x;
			output[9] = (double)this.zBasis.y;
			output[10] = (double)this.zBasis.z;
			output[11] = 0.0;
			output[12] = (double)this.origin.x;
			output[13] = (double)this.origin.y;
			output[14] = (double)this.origin.z;
			output[15] = 1.0;
			return output;
		}

		// Token: 0x0600268D RID: 9869 RVA: 0x000D8A91 File Offset: 0x000D6E91
		public float[] ToArray4x4()
		{
			return this.ToArray4x4(new float[16]);
		}

		// Token: 0x0600268E RID: 9870 RVA: 0x000D8AA0 File Offset: 0x000D6EA0
		public void SetRotation(Vector axis, float angleRadians)
		{
			Vector normalized = axis.Normalized;
			float num = (float)Math.Sin((double)angleRadians);
			float num2 = (float)Math.Cos((double)angleRadians);
			float num3 = 1f - num2;
			this.xBasis = new Vector(normalized[0U] * normalized[0U] * num3 + num2, normalized[0U] * normalized[1U] * num3 - normalized[2U] * num, normalized[0U] * normalized[2U] * num3 + normalized[1U] * num);
			this.yBasis = new Vector(normalized[1U] * normalized[0U] * num3 + normalized[2U] * num, normalized[1U] * normalized[1U] * num3 + num2, normalized[1U] * normalized[2U] * num3 - normalized[0U] * num);
			this.zBasis = new Vector(normalized[2U] * normalized[0U] * num3 - normalized[1U] * num, normalized[2U] * normalized[1U] * num3 + normalized[0U] * num, normalized[2U] * normalized[2U] * num3 + num2);
		}

		// Token: 0x0600268F RID: 9871 RVA: 0x000D8BE4 File Offset: 0x000D6FE4
		public Vector TransformPoint(Vector point)
		{
			return this.xBasis * point.x + this.yBasis * point.y + this.zBasis * point.z + this.origin;
		}

		// Token: 0x06002690 RID: 9872 RVA: 0x000D8C3C File Offset: 0x000D703C
		public Vector TransformDirection(Vector direction)
		{
			return this.xBasis * direction.x + this.yBasis * direction.y + this.zBasis * direction.z;
		}

		// Token: 0x06002691 RID: 9873 RVA: 0x000D8C8C File Offset: 0x000D708C
		public Matrix RigidInverse()
		{
			Matrix result = new Matrix(new Vector(this.xBasis[0U], this.yBasis[0U], this.zBasis[0U]), new Vector(this.xBasis[1U], this.yBasis[1U], this.zBasis[1U]), new Vector(this.xBasis[2U], this.yBasis[2U], this.zBasis[2U]));
			result.origin = result.TransformDirection(-this.origin);
			return result;
		}

		// Token: 0x06002692 RID: 9874 RVA: 0x000D8D56 File Offset: 0x000D7156
		private Matrix _operator_mul(Matrix other)
		{
			return new Matrix(this.TransformDirection(other.xBasis), this.TransformDirection(other.yBasis), this.TransformDirection(other.zBasis), this.TransformPoint(other.origin));
		}

		// Token: 0x06002693 RID: 9875 RVA: 0x000D8D94 File Offset: 0x000D7194
		public bool Equals(Matrix other)
		{
			return this.xBasis == other.xBasis && this.yBasis == other.yBasis && this.zBasis == other.zBasis && this.origin == other.origin;
		}

		// Token: 0x06002694 RID: 9876 RVA: 0x000D8DFC File Offset: 0x000D71FC
		public override string ToString()
		{
			return string.Format("xBasis: {0} yBasis: {1} zBasis: {2} origin: {3}", new object[]
			{
				this.xBasis,
				this.yBasis,
				this.zBasis,
				this.origin
			});
		}

		// Token: 0x170004B9 RID: 1209
		// (get) Token: 0x06002695 RID: 9877 RVA: 0x000D8E51 File Offset: 0x000D7251
		// (set) Token: 0x06002696 RID: 9878 RVA: 0x000D8E59 File Offset: 0x000D7259
		public Vector xBasis
		{
			[CompilerGenerated]
			get
			{
				return this.<xBasis>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<xBasis>k__BackingField = value;
			}
		}

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x06002697 RID: 9879 RVA: 0x000D8E62 File Offset: 0x000D7262
		// (set) Token: 0x06002698 RID: 9880 RVA: 0x000D8E6A File Offset: 0x000D726A
		public Vector yBasis
		{
			[CompilerGenerated]
			get
			{
				return this.<yBasis>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<yBasis>k__BackingField = value;
			}
		}

		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x06002699 RID: 9881 RVA: 0x000D8E73 File Offset: 0x000D7273
		// (set) Token: 0x0600269A RID: 9882 RVA: 0x000D8E7B File Offset: 0x000D727B
		public Vector zBasis
		{
			[CompilerGenerated]
			get
			{
				return this.<zBasis>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<zBasis>k__BackingField = value;
			}
		}

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x0600269B RID: 9883 RVA: 0x000D8E84 File Offset: 0x000D7284
		// (set) Token: 0x0600269C RID: 9884 RVA: 0x000D8E8C File Offset: 0x000D728C
		public Vector origin
		{
			[CompilerGenerated]
			get
			{
				return this.<origin>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<origin>k__BackingField = value;
			}
		}

		// Token: 0x0600269D RID: 9885 RVA: 0x000D8E95 File Offset: 0x000D7295
		// Note: this type is marked as 'beforefieldinit'.
		static Matrix()
		{
		}

		// Token: 0x040020BF RID: 8383
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Vector <xBasis>k__BackingField;

		// Token: 0x040020C0 RID: 8384
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Vector <yBasis>k__BackingField;

		// Token: 0x040020C1 RID: 8385
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Vector <zBasis>k__BackingField;

		// Token: 0x040020C2 RID: 8386
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Vector <origin>k__BackingField;

		// Token: 0x040020C3 RID: 8387
		public static readonly Matrix Identity = new Matrix(Vector.XAxis, Vector.YAxis, Vector.ZAxis, Vector.Zero);
	}
}
