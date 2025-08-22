using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003FB RID: 1019
	public static class ObiUtils
	{
		// Token: 0x060019E5 RID: 6629 RVA: 0x0009086C File Offset: 0x0008EC6C
		public static void DrawArrowGizmo(float bodyLenght, float bodyWidth, float headLenght, float headWidth)
		{
			float num = bodyLenght * 0.5f;
			float num2 = bodyWidth * 0.5f;
			Gizmos.DrawLine(new Vector3(num2, 0f, -num), new Vector3(num2, 0f, num));
			Gizmos.DrawLine(new Vector3(-num2, 0f, -num), new Vector3(-num2, 0f, num));
			Gizmos.DrawLine(new Vector3(-num2, 0f, -num), new Vector3(num2, 0f, -num));
			Gizmos.DrawLine(new Vector3(num2, 0f, num), new Vector3(headWidth, 0f, num));
			Gizmos.DrawLine(new Vector3(-num2, 0f, num), new Vector3(-headWidth, 0f, num));
			Gizmos.DrawLine(new Vector3(0f, 0f, num + headLenght), new Vector3(headWidth, 0f, num));
			Gizmos.DrawLine(new Vector3(0f, 0f, num + headLenght), new Vector3(-headWidth, 0f, num));
		}

		// Token: 0x060019E6 RID: 6630 RVA: 0x0009096C File Offset: 0x0008ED6C
		public static void ArrayFill<T>(T[] arrayToFill, T[] fillValue)
		{
			if (fillValue.Length <= arrayToFill.Length)
			{
				Array.Copy(fillValue, arrayToFill, fillValue.Length);
				int num = arrayToFill.Length / 2;
				for (int i = fillValue.Length; i < arrayToFill.Length; i *= 2)
				{
					int length = i;
					if (i > num)
					{
						length = arrayToFill.Length - i;
					}
					Array.Copy(arrayToFill, 0, arrayToFill, i, length);
				}
			}
		}

		// Token: 0x060019E7 RID: 6631 RVA: 0x000909C4 File Offset: 0x0008EDC4
		public static IList<T> Swap<T>(this IList<T> list, int indexA, int indexB)
		{
			if (indexA != indexB && indexB > -1 && indexB < list.Count && indexA > -1 && indexA < list.Count)
			{
				T value = list[indexA];
				list[indexA] = list[indexB];
				list[indexB] = value;
			}
			return list;
		}

		// Token: 0x060019E8 RID: 6632 RVA: 0x00090A20 File Offset: 0x0008EE20
		public static void AddRange<T>(ref T[] array, T[] other)
		{
			if (array == null || other == null)
			{
				return;
			}
			int index = array.Length;
			Array.Resize<T>(ref array, array.Length + other.Length);
			other.CopyTo(array, index);
		}

		// Token: 0x060019E9 RID: 6633 RVA: 0x00090A58 File Offset: 0x0008EE58
		public static void RemoveRange<T>(ref T[] array, int index, int count)
		{
			if (array == null)
			{
				return;
			}
			if (index < 0 || count < 0)
			{
				throw new ArgumentOutOfRangeException("Index and/or count are < 0.");
			}
			if (index + count > array.Length)
			{
				throw new ArgumentException("Index and count do not denote a valid range of elements.");
			}
			for (int i = index; i < array.Length - count; i++)
			{
				array.SetValue(array.GetValue(i + count), i);
			}
			Array.Resize<T>(ref array, array.Length - count);
		}

		// Token: 0x060019EA RID: 6634 RVA: 0x00090AD4 File Offset: 0x0008EED4
		public static Bounds Transform(this Bounds b, Matrix4x4 m)
		{
			Vector4 v = m.GetColumn(0) * b.min.x;
			Vector4 v2 = m.GetColumn(0) * b.max.x;
			Vector4 v3 = m.GetColumn(1) * b.min.y;
			Vector4 v4 = m.GetColumn(1) * b.max.y;
			Vector4 v5 = m.GetColumn(2) * b.min.z;
			Vector4 v6 = m.GetColumn(2) * b.max.z;
			Bounds result = default(Bounds);
			Vector3 b2 = m.GetColumn(3);
			result.SetMinMax(Vector3.Min(v, v2) + Vector3.Min(v3, v4) + Vector3.Min(v5, v6) + b2, Vector3.Max(v, v2) + Vector3.Max(v3, v4) + Vector3.Max(v5, v6) + b2);
			return result;
		}

		// Token: 0x060019EB RID: 6635 RVA: 0x00090C47 File Offset: 0x0008F047
		public static float Remap(this float value, float from1, float to1, float from2, float to2)
		{
			return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
		}

		// Token: 0x060019EC RID: 6636 RVA: 0x00090C57 File Offset: 0x0008F057
		public static float Mod(float a, float b)
		{
			return a - b * Mathf.Floor(a / b);
		}

		// Token: 0x060019ED RID: 6637 RVA: 0x00090C68 File Offset: 0x0008F068
		public static float TriangleArea(Vector3 p1, Vector3 p2, Vector3 p3)
		{
			return Mathf.Sqrt(Vector3.Cross(p2 - p1, p3 - p1).sqrMagnitude) / 2f;
		}
	}
}
