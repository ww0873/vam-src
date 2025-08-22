using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Common.Scripts.Utils
{
	// Token: 0x020009DD RID: 2525
	public class MathSearchUtils
	{
		// Token: 0x06003FA0 RID: 16288 RVA: 0x0012FAA8 File Offset: 0x0012DEA8
		public MathSearchUtils()
		{
		}

		// Token: 0x06003FA1 RID: 16289 RVA: 0x0012FAB0 File Offset: 0x0012DEB0
		public static List<Vector3> FindCloseVertices(Vector3[] vertices, Vector3 testVertex, int count)
		{
			List<Vector3> list = new List<Vector3>();
			for (int i = 0; i < count; i++)
			{
				Vector3 item = MathSearchUtils.FindCloseVertex(vertices, testVertex, list);
				list.Add(item);
			}
			return list;
		}

		// Token: 0x06003FA2 RID: 16290 RVA: 0x0012FAE8 File Offset: 0x0012DEE8
		public static Vector3 FindCloseVertex(Vector3[] vertices, Vector3 testVertex, List<Vector3> ignoreList = null)
		{
			float num = float.PositiveInfinity;
			Vector3 result = vertices[0];
			foreach (Vector3 vector in vertices)
			{
				if (ignoreList == null || !ignoreList.Contains(vector))
				{
					float sqrMagnitude = (vector - testVertex).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						num = sqrMagnitude;
						result = vector;
					}
				}
			}
			return result;
		}

		// Token: 0x06003FA3 RID: 16291 RVA: 0x0012FB68 File Offset: 0x0012DF68
		public static Vector3 FindCloseVertex(List<Vector3> vertices, Vector3 testVertex, List<Vector3> ignoreList = null)
		{
			float num = float.PositiveInfinity;
			Vector3 result = vertices[0];
			foreach (Vector3 vector in vertices)
			{
				if (ignoreList == null || !ignoreList.Contains(vector))
				{
					float sqrMagnitude = (vector - testVertex).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						num = sqrMagnitude;
						result = vector;
					}
				}
			}
			return result;
		}
	}
}
