using System;
using System.Collections.Generic;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Common.Scripts.Utils;
using GPUTools.Hair.Scripts.Geometry.MayaImport.Data;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.MayaImport.Commands
{
	// Token: 0x020009FE RID: 2558
	public class AssignHairLinesToTriangles : ICacheCommand
	{
		// Token: 0x060040C9 RID: 16585 RVA: 0x001344A6 File Offset: 0x001328A6
		public AssignHairLinesToTriangles(MayaHairGeometryImporter importer)
		{
			this.importer = importer;
			this.data = importer.Data;
		}

		// Token: 0x060040CA RID: 16586 RVA: 0x001344C1 File Offset: 0x001328C1
		public void Cache()
		{
			this.data.TringlesCenters = this.ComputeTringlesCenters();
			this.data.Lines = this.Assign(this.data.TringlesCenters);
		}

		// Token: 0x060040CB RID: 16587 RVA: 0x001344F0 File Offset: 0x001328F0
		private List<Vector3> Assign(List<Vector3> scalpTringlesCenters)
		{
			List<Vector3> lines = this.data.Lines;
			List<Vector3> list = new List<Vector3>();
			List<Vector3> list2 = new List<Vector3>();
			for (int i = 0; i < lines.Count; i += this.data.Segments)
			{
				Vector3 vector = lines[i];
				Vector3 vector2 = MathSearchUtils.FindCloseVertex(scalpTringlesCenters, vector, null);
				Vector3 offset = vector2 - vector;
				if (!list2.Contains(vector2))
				{
					List<Vector3> collection = this.CreateStandWithOffsetForRegion(lines, offset, i, i + this.data.Segments);
					list.AddRange(collection);
					list2.Add(vector2);
				}
			}
			return list;
		}

		// Token: 0x060040CC RID: 16588 RVA: 0x0013458C File Offset: 0x0013298C
		private List<Vector3> ComputeTringlesCenters()
		{
			int[] indices = this.importer.ScalpProvider.Mesh.GetIndices(0);
			Vector3[] vertices = this.importer.ScalpProvider.Mesh.vertices;
			List<Vector3> list = new List<Vector3>();
			for (int i = 0; i < indices.Length; i += 3)
			{
				Vector3 a = vertices[indices[i]];
				Vector3 b = vertices[indices[i + 1]];
				Vector3 b2 = vertices[indices[i + 2]];
				list.Add((a + b + b2) / 3f);
			}
			return list;
		}

		// Token: 0x060040CD RID: 16589 RVA: 0x00134638 File Offset: 0x00132A38
		private List<Vector3> CreateStandWithOffsetForRegion(List<Vector3> vertices, Vector3 offset, int start, int end)
		{
			List<Vector3> list = new List<Vector3>();
			for (int i = start; i < end; i++)
			{
				list.Add(vertices[i] + offset);
			}
			return list;
		}

		// Token: 0x040030C9 RID: 12489
		private readonly MayaHairGeometryImporter importer;

		// Token: 0x040030CA RID: 12490
		private readonly MayaHairData data;
	}
}
