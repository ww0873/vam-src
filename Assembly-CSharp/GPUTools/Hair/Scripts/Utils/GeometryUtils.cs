using System;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Utils
{
	// Token: 0x02000A30 RID: 2608
	public static class GeometryUtils
	{
		// Token: 0x0600433F RID: 17215 RVA: 0x0013BCF4 File Offset: 0x0013A0F4
		public static Vector2 To2D(int i, int sizeY)
		{
			int num = i / sizeY;
			int num2 = i % sizeY;
			return new Vector2((float)num, (float)num2);
		}

		// Token: 0x06004340 RID: 17216 RVA: 0x0013BD14 File Offset: 0x0013A114
		public static Bounds InverseTransformBounds(this Transform transform, Bounds worldBounds)
		{
			Vector3 center = transform.InverseTransformPoint(worldBounds.center);
			Vector3 extents = worldBounds.extents;
			Vector3 vector = transform.InverseTransformVector(extents.x, 0f, 0f);
			Vector3 vector2 = transform.InverseTransformVector(0f, extents.y, 0f);
			Vector3 vector3 = transform.InverseTransformVector(0f, 0f, extents.z);
			extents.x = Mathf.Abs(vector.x) + Mathf.Abs(vector2.x) + Mathf.Abs(vector3.x);
			extents.y = Mathf.Abs(vector.y) + Mathf.Abs(vector2.y) + Mathf.Abs(vector3.y);
			extents.z = Mathf.Abs(vector.z) + Mathf.Abs(vector2.z) + Mathf.Abs(vector3.z);
			return new Bounds
			{
				center = center,
				extents = extents
			};
		}

		// Token: 0x06004341 RID: 17217 RVA: 0x0013BE24 File Offset: 0x0013A224
		public static Bounds TransformBounds(this Transform transform, Bounds worldBounds)
		{
			Vector3 center = transform.TransformPoint(worldBounds.center);
			Vector3 extents = worldBounds.extents;
			Vector3 vector = transform.TransformVector(extents.x, 0f, 0f);
			Vector3 vector2 = transform.TransformVector(0f, extents.y, 0f);
			Vector3 vector3 = transform.TransformVector(0f, 0f, extents.z);
			extents.x = Mathf.Abs(vector.x) + Mathf.Abs(vector2.x) + Mathf.Abs(vector3.x);
			extents.y = Mathf.Abs(vector.y) + Mathf.Abs(vector2.y) + Mathf.Abs(vector3.y);
			extents.z = Mathf.Abs(vector.z) + Mathf.Abs(vector2.z) + Mathf.Abs(vector3.z);
			return new Bounds
			{
				center = center,
				extents = extents
			};
		}
	}
}
