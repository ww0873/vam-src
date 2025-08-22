using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.MayaImport.Debug
{
	// Token: 0x02000A03 RID: 2563
	public class MayaImporterDebugDraw
	{
		// Token: 0x060040DC RID: 16604 RVA: 0x00134B20 File Offset: 0x00132F20
		public MayaImporterDebugDraw()
		{
		}

		// Token: 0x060040DD RID: 16605 RVA: 0x00134B28 File Offset: 0x00132F28
		public static void Draw(MayaHairGeometryImporter importer)
		{
			Gizmos.color = Color.green;
			List<Vector3> vertices = importer.Data.Vertices;
			Matrix4x4 toWorldMatrix = importer.ScalpProvider.ToWorldMatrix;
			for (int i = 1; i < vertices.Count; i++)
			{
				if (i % importer.Data.Segments != 0)
				{
					Vector3 from = toWorldMatrix.MultiplyPoint3x4(vertices[i - 1]);
					Vector3 to = toWorldMatrix.MultiplyPoint3x4(vertices[i]);
					Gizmos.DrawLine(from, to);
				}
			}
			Bounds bounds = importer.GetBounds();
			Gizmos.DrawWireCube(bounds.center, bounds.size);
		}
	}
}
