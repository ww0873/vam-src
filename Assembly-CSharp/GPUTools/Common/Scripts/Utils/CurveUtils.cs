using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Common.Scripts.Utils
{
	// Token: 0x020009DC RID: 2524
	public class CurveUtils
	{
		// Token: 0x06003F9D RID: 16285 RVA: 0x0012F9B0 File Offset: 0x0012DDB0
		public CurveUtils()
		{
		}

		// Token: 0x06003F9E RID: 16286 RVA: 0x0012F9B8 File Offset: 0x0012DDB8
		public static Vector3 GetSplinePoint(List<Vector3> points, float t)
		{
			int b = points.Count - 1;
			int num = (int)(t * (float)points.Count);
			float num2 = 1f / (float)points.Count;
			float t2 = t % num2 * (float)points.Count;
			int index = Mathf.Max(0, num - 1);
			int index2 = Mathf.Min(num, b);
			int index3 = Mathf.Min(num + 1, b);
			Vector3 a = points[index];
			Vector3 vector = points[index2];
			Vector3 b2 = points[index3];
			Vector3 p = (a + vector) * 0.5f;
			Vector3 p2 = (vector + b2) * 0.5f;
			return CurveUtils.GetBezierPoint(p, vector, p2, t2);
		}

		// Token: 0x06003F9F RID: 16287 RVA: 0x0012FA68 File Offset: 0x0012DE68
		public static Vector3 GetBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
		{
			float num = 1f - t;
			return num * num * p0 + 2f * num * t * p1 + t * t * p2;
		}
	}
}
