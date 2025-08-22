using System;
using System.Collections.Generic;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Hair.Scripts.Geometry.MayaImport.Data;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.MayaImport.Commands
{
	// Token: 0x02000A00 RID: 2560
	public class MoveHairLinesToScalpVertices : ICacheCommand
	{
		// Token: 0x060040D1 RID: 16593 RVA: 0x00134736 File Offset: 0x00132B36
		public MoveHairLinesToScalpVertices(MayaHairGeometryImporter importer)
		{
			this.importer = importer;
			this.data = importer.Data;
		}

		// Token: 0x060040D2 RID: 16594 RVA: 0x00134754 File Offset: 0x00132B54
		public void Cache()
		{
			int[] indices = this.importer.ScalpProvider.Mesh.GetIndices(0);
			Vector3[] vertices = this.importer.ScalpProvider.Mesh.vertices;
			List<Vector3> list = new List<Vector3>();
			List<int> list2 = new List<int>();
			for (int i = 0; i < indices.Length; i += 3)
			{
				Vector3 vector = this.data.TringlesCenters[i / 3];
				int num = this.FindStandIndex(this.data.Lines, vector, this.data.Segments);
				if (num != -1)
				{
					for (int j = 0; j < 3; j++)
					{
						Vector3 vector2 = vertices[indices[i + j]];
						Vector3 offset = vector2 - vector;
						int num2 = this.FindStandIndex(list, vector2, this.data.Segments);
						if (num2 == -1 || !this.CompareStands(list, num2, this.data.Lines, num, this.data.Segments))
						{
							list2.Add(list.Count / this.data.Segments);
							list.AddRange(this.CreateStandWithOffsetForRegion(this.data.Lines, offset, num, this.data.Segments));
						}
						else
						{
							list2.Add(num2 / this.data.Segments);
						}
					}
				}
			}
			this.data.Vertices = list;
			this.data.Indices = list2.ToArray();
		}

		// Token: 0x060040D3 RID: 16595 RVA: 0x001348E4 File Offset: 0x00132CE4
		private bool CompareStands(List<Vector3> hairStands1, int stand1, List<Vector3> hairStands2, int stand2, int segments)
		{
			float num = this.importer.RegionThresholdDistance * this.importer.RegionThresholdDistance;
			for (int i = 0; i < segments; i++)
			{
				Vector3 a = hairStands1[stand1 + i];
				Vector3 b = hairStands2[stand2 + i];
				if ((a - b).sqrMagnitude > num)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060040D4 RID: 16596 RVA: 0x0013494C File Offset: 0x00132D4C
		private int FindStandIndex(List<Vector3> hairVertices, Vector3 vertex, int segments)
		{
			for (int i = 0; i < hairVertices.Count; i += segments)
			{
				if (hairVertices[i] == vertex)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x060040D5 RID: 16597 RVA: 0x00134988 File Offset: 0x00132D88
		private List<Vector3> CreateStandWithOffsetForRegion(List<Vector3> vertices, Vector3 offset, int start, int segments)
		{
			List<Vector3> list = new List<Vector3>();
			int num = start + segments;
			for (int i = start; i < num; i++)
			{
				list.Add(vertices[i] + offset);
			}
			return list;
		}

		// Token: 0x040030CC RID: 12492
		private readonly MayaHairGeometryImporter importer;

		// Token: 0x040030CD RID: 12493
		private readonly MayaHairData data;
	}
}
