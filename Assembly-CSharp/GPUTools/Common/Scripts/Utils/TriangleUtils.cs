using System;
using UnityEngine;

namespace GPUTools.Common.Scripts.Utils
{
	// Token: 0x020009DF RID: 2527
	public class TriangleUtils
	{
		// Token: 0x06003FAD RID: 16301 RVA: 0x0012FE9A File Offset: 0x0012E29A
		public TriangleUtils()
		{
		}

		// Token: 0x06003FAE RID: 16302 RVA: 0x0012FEA4 File Offset: 0x0012E2A4
		public static Rect FindBoundRect(Vector2[] points)
		{
			Rect result = new Rect
			{
				min = points[0],
				max = points[0]
			};
			for (int i = 1; i < points.Length; i++)
			{
				if (points[i].x < result.min.x)
				{
					result.min = new Vector2(points[i].x, result.min.y);
				}
				if (points[i].y < result.min.y)
				{
					result.min = new Vector2(result.min.x, points[i].y);
				}
				if (points[i].x > result.max.x)
				{
					result.max = new Vector2(points[i].x, result.max.y);
				}
				if (points[i].y > result.max.y)
				{
					result.max = new Vector2(result.max.x, points[i].y);
				}
			}
			return result;
		}

		// Token: 0x06003FAF RID: 16303 RVA: 0x0013001C File Offset: 0x0012E41C
		public static bool IsPointInsideTriangle(Vector2 p, Vector2 a, Vector2 b, Vector2 c)
		{
			Vector2 barycentricInsideTriangle = TriangleUtils.GetBarycentricInsideTriangle(p, a, b, c);
			return barycentricInsideTriangle.x >= 0f && barycentricInsideTriangle.y >= 0f && barycentricInsideTriangle.x + barycentricInsideTriangle.y <= 1f;
		}

		// Token: 0x06003FB0 RID: 16304 RVA: 0x00130071 File Offset: 0x0012E471
		public static bool IsPointInsideTriangle(Vector2 barycentric)
		{
			return barycentric.x >= 0f && barycentric.y >= 0f && barycentric.x + barycentric.y <= 1f;
		}

		// Token: 0x06003FB1 RID: 16305 RVA: 0x001300B4 File Offset: 0x0012E4B4
		public static Vector3 GetPointInsideTriangle(Vector3 a, Vector3 b, Vector3 c, Vector2 barycentric)
		{
			return a * (1f - (barycentric.x + barycentric.y)) + b * barycentric.y + c * barycentric.x;
		}

		// Token: 0x06003FB2 RID: 16306 RVA: 0x00130100 File Offset: 0x0012E500
		public static Vector2 GetBarycentricInsideTriangle(Vector2 p, Vector2 a, Vector2 b, Vector2 c)
		{
			Vector2 vector = c - a;
			Vector2 vector2 = b - a;
			Vector2 rhs = p - a;
			float num = Vector2.Dot(vector, vector);
			float num2 = Vector2.Dot(vector, vector2);
			float num3 = Vector2.Dot(vector, rhs);
			float num4 = Vector2.Dot(vector2, vector2);
			float num5 = Vector2.Dot(vector2, rhs);
			float num6 = 1f / (num * num4 - num2 * num2);
			float x = (num4 * num3 - num2 * num5) * num6;
			float y = (num * num5 - num2 * num3) * num6;
			return new Vector2(x, y);
		}
	}
}
