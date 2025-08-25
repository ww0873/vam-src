using System;
using System.Collections.Generic;
using GPUTools.Common.Scripts.Tools.Commands;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.MayaImport.Commands
{
	// Token: 0x02000A01 RID: 2561
	public class StripsToLinesMayaPass : ICacheCommand
	{
		// Token: 0x060040D6 RID: 16598 RVA: 0x001349C6 File Offset: 0x00132DC6
		public StripsToLinesMayaPass(MayaHairGeometryImporter importer)
		{
			this.importer = importer;
		}

		// Token: 0x060040D7 RID: 16599 RVA: 0x001349D8 File Offset: 0x00132DD8
		public void Cache()
		{
			MeshFilter[] componentsInChildren = this.importer.HairContainer.GetComponentsInChildren<MeshFilter>();
			List<Vector3> list = new List<Vector3>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				List<Vector3> list2 = this.CacheStand(componentsInChildren[i]);
				list.AddRange(list2);
				this.importer.Data.Segments = list2.Count;
			}
			this.importer.Data.Lines = list;
		}

		// Token: 0x060040D8 RID: 16600 RVA: 0x00134A48 File Offset: 0x00132E48
		private List<Vector3> CacheStand(MeshFilter meshFilter)
		{
			Vector3[] vertices = meshFilter.sharedMesh.vertices;
			int[] triangles = meshFilter.sharedMesh.triangles;
			List<Vector3> list = new List<Vector3>();
			for (int i = 0; i < triangles.Length; i += 6)
			{
				int num = triangles[i + 2];
				Vector3 item = this.ToScalpSpace(meshFilter, vertices[num]);
				list.Add(item);
			}
			return list;
		}

		// Token: 0x060040D9 RID: 16601 RVA: 0x00134AB0 File Offset: 0x00132EB0
		private Vector3 ToScalpSpace(MeshFilter filter, Vector3 point)
		{
			Matrix4x4 inverse = this.importer.ScalpProvider.ToWorldMatrix.inverse;
			Vector3 point2 = filter.transform.TransformPoint(point);
			return inverse.MultiplyPoint3x4(point2);
		}

		// Token: 0x040030CE RID: 12494
		private readonly MayaHairGeometryImporter importer;
	}
}
