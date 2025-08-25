using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Tools
{
	// Token: 0x02000A07 RID: 2567
	public class MeshUtils
	{
		// Token: 0x060040FE RID: 16638 RVA: 0x001351EC File Offset: 0x001335EC
		public MeshUtils()
		{
		}

		// Token: 0x060040FF RID: 16639 RVA: 0x001351F4 File Offset: 0x001335F4
		public static List<Vector3> GetWorldVertices(MeshFilter fiter)
		{
			List<Vector3> list = new List<Vector3>();
			Vector3[] vertices = fiter.sharedMesh.vertices;
			foreach (Vector3 position in vertices)
			{
				list.Add(fiter.transform.TransformPoint(position));
			}
			return list;
		}

		// Token: 0x06004100 RID: 16640 RVA: 0x00135250 File Offset: 0x00133650
		public static List<Vector3> GetVerticesInSpace(Mesh mesh, Matrix4x4 toWorld, Matrix4x4 toTransform)
		{
			List<Vector3> list = new List<Vector3>();
			for (int i = 0; i < mesh.vertexCount; i++)
			{
				Vector3 point = mesh.vertices[i];
				Vector3 point2 = toWorld.MultiplyPoint3x4(point);
				Vector3 item = toTransform.MultiplyPoint3x4(point2);
				list.Add(item);
			}
			return list;
		}
	}
}
