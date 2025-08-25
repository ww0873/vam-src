using System;
using System.Runtime.Serialization;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad.UnityEngineNS
{
	// Token: 0x020001EA RID: 490
	[ProtoContract(ImplicitFields = ImplicitFields.AllFields)]
	public class Matrix4x4Surrogate : ISerializationSurrogate
	{
		// Token: 0x060009D8 RID: 2520 RVA: 0x0003CB92 File Offset: 0x0003AF92
		public Matrix4x4Surrogate()
		{
		}

		// Token: 0x060009D9 RID: 2521 RVA: 0x0003CB9C File Offset: 0x0003AF9C
		public static implicit operator Matrix4x4(Matrix4x4Surrogate v)
		{
			return new Matrix4x4
			{
				m00 = v.m00,
				m10 = v.m10,
				m20 = v.m20,
				m30 = v.m30,
				m01 = v.m01,
				m11 = v.m11,
				m21 = v.m21,
				m31 = v.m31,
				m02 = v.m02,
				m12 = v.m12,
				m22 = v.m22,
				m32 = v.m32,
				m03 = v.m03,
				m13 = v.m13,
				m23 = v.m23,
				m33 = v.m33
			};
		}

		// Token: 0x060009DA RID: 2522 RVA: 0x0003CC84 File Offset: 0x0003B084
		public static implicit operator Matrix4x4Surrogate(Matrix4x4 v)
		{
			return new Matrix4x4Surrogate
			{
				m00 = v.m00,
				m10 = v.m10,
				m20 = v.m20,
				m30 = v.m30,
				m01 = v.m01,
				m11 = v.m11,
				m21 = v.m21,
				m31 = v.m31,
				m02 = v.m02,
				m12 = v.m12,
				m22 = v.m22,
				m32 = v.m32,
				m03 = v.m03,
				m13 = v.m13,
				m23 = v.m23,
				m33 = v.m33
			};
		}

		// Token: 0x060009DB RID: 2523 RVA: 0x0003CD68 File Offset: 0x0003B168
		public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			Matrix4x4 matrix4x = (Matrix4x4)obj;
			info.AddValue("m00", matrix4x.m00);
			info.AddValue("m10", matrix4x.m10);
			info.AddValue("m20", matrix4x.m20);
			info.AddValue("m30", matrix4x.m30);
			info.AddValue("m01", matrix4x.m01);
			info.AddValue("m11", matrix4x.m11);
			info.AddValue("m21", matrix4x.m21);
			info.AddValue("m31", matrix4x.m31);
			info.AddValue("m02", matrix4x.m02);
			info.AddValue("m12", matrix4x.m12);
			info.AddValue("m22", matrix4x.m22);
			info.AddValue("m32", matrix4x.m32);
			info.AddValue("m03", matrix4x.m03);
			info.AddValue("m13", matrix4x.m13);
			info.AddValue("m23", matrix4x.m23);
			info.AddValue("m33", matrix4x.m33);
		}

		// Token: 0x060009DC RID: 2524 RVA: 0x0003CE9C File Offset: 0x0003B29C
		public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			Matrix4x4 matrix4x = (Matrix4x4)obj;
			matrix4x.m00 = (float)info.GetValue("m00", typeof(float));
			matrix4x.m10 = (float)info.GetValue("m10", typeof(float));
			matrix4x.m20 = (float)info.GetValue("m20", typeof(float));
			matrix4x.m30 = (float)info.GetValue("m30", typeof(float));
			matrix4x.m01 = (float)info.GetValue("m01", typeof(float));
			matrix4x.m11 = (float)info.GetValue("m11", typeof(float));
			matrix4x.m21 = (float)info.GetValue("m21", typeof(float));
			matrix4x.m31 = (float)info.GetValue("m31", typeof(float));
			matrix4x.m02 = (float)info.GetValue("m02", typeof(float));
			matrix4x.m12 = (float)info.GetValue("m12", typeof(float));
			matrix4x.m22 = (float)info.GetValue("m22", typeof(float));
			matrix4x.m32 = (float)info.GetValue("m32", typeof(float));
			matrix4x.m03 = (float)info.GetValue("m03", typeof(float));
			matrix4x.m13 = (float)info.GetValue("m13", typeof(float));
			matrix4x.m23 = (float)info.GetValue("m23", typeof(float));
			matrix4x.m33 = (float)info.GetValue("m33", typeof(float));
			return matrix4x;
		}

		// Token: 0x04000B10 RID: 2832
		public float m00;

		// Token: 0x04000B11 RID: 2833
		public float m10;

		// Token: 0x04000B12 RID: 2834
		public float m20;

		// Token: 0x04000B13 RID: 2835
		public float m30;

		// Token: 0x04000B14 RID: 2836
		public float m01;

		// Token: 0x04000B15 RID: 2837
		public float m11;

		// Token: 0x04000B16 RID: 2838
		public float m21;

		// Token: 0x04000B17 RID: 2839
		public float m31;

		// Token: 0x04000B18 RID: 2840
		public float m02;

		// Token: 0x04000B19 RID: 2841
		public float m12;

		// Token: 0x04000B1A RID: 2842
		public float m22;

		// Token: 0x04000B1B RID: 2843
		public float m32;

		// Token: 0x04000B1C RID: 2844
		public float m03;

		// Token: 0x04000B1D RID: 2845
		public float m13;

		// Token: 0x04000B1E RID: 2846
		public float m23;

		// Token: 0x04000B1F RID: 2847
		public float m33;
	}
}
