using System;
using UnityEngine;

namespace GPUTools.Common.Scripts.Utils
{
	// Token: 0x020009DE RID: 2526
	public static class TransformUtils
	{
		// Token: 0x06003FA4 RID: 16292 RVA: 0x0012FC00 File Offset: 0x0012E000
		public static Vector3[] TransformPoints(this Transform transform, Vector3[] points)
		{
			Vector3[] array = new Vector3[points.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = transform.TransformPoint(points[i]);
			}
			return array;
		}

		// Token: 0x06003FA5 RID: 16293 RVA: 0x0012FC4C File Offset: 0x0012E04C
		public static Vector3[] InverseTransformPoints(this Transform transform, Vector3[] points)
		{
			Vector3[] array = new Vector3[points.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = transform.InverseTransformPoint(points[i]);
			}
			return array;
		}

		// Token: 0x06003FA6 RID: 16294 RVA: 0x0012FC98 File Offset: 0x0012E098
		public static void TransformPoints(this Transform transform, ref Vector3[] points)
		{
			for (int i = 0; i < points.Length; i++)
			{
				points[i] = transform.TransformPoint(points[i]);
			}
		}

		// Token: 0x06003FA7 RID: 16295 RVA: 0x0012FCDC File Offset: 0x0012E0DC
		public static Vector3[] TransformVectors(this Transform transform, Vector3[] vectors)
		{
			Vector3[] array = new Vector3[vectors.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = transform.TransformVector(vectors[i]);
			}
			return array;
		}

		// Token: 0x06003FA8 RID: 16296 RVA: 0x0012FD28 File Offset: 0x0012E128
		public static void TransformVectors(this Transform transform, ref Vector3[] vectors)
		{
			for (int i = 0; i < vectors.Length; i++)
			{
				vectors[i] = transform.TransformVector(vectors[i]);
			}
		}

		// Token: 0x06003FA9 RID: 16297 RVA: 0x0012FD6C File Offset: 0x0012E16C
		public static Vector3[] TransformDirrections(this Transform transform, Vector3[] dirrections)
		{
			Vector3[] array = new Vector3[dirrections.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = transform.TransformDirection(dirrections[i]);
			}
			return array;
		}

		// Token: 0x06003FAA RID: 16298 RVA: 0x0012FDB8 File Offset: 0x0012E1B8
		public static void TransformDirrections(this Transform transform, ref Vector3[] dirrections)
		{
			for (int i = 0; i < dirrections.Length; i++)
			{
				dirrections[i] = transform.TransformDirection(dirrections[i]);
			}
		}

		// Token: 0x06003FAB RID: 16299 RVA: 0x0012FDFC File Offset: 0x0012E1FC
		public static Vector3[] TransformPoints(this Matrix4x4 matrix, Vector3[] points)
		{
			Vector3[] array = new Vector3[points.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = matrix.MultiplyPoint3x4(points[i]);
			}
			return array;
		}

		// Token: 0x06003FAC RID: 16300 RVA: 0x0012FE48 File Offset: 0x0012E248
		public static Vector3[] InverseTransformPoints(this Matrix4x4 matrix, Vector3[] points)
		{
			Vector3[] array = new Vector3[points.Length];
			Matrix4x4 inverse = matrix.inverse;
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = inverse.MultiplyPoint3x4(points[i]);
			}
			return array;
		}
	}
}
