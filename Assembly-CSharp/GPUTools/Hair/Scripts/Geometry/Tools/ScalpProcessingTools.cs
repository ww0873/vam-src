using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Geometry.Tools
{
	// Token: 0x02000A08 RID: 2568
	public class ScalpProcessingTools
	{
		// Token: 0x06004101 RID: 16641 RVA: 0x001352A8 File Offset: 0x001336A8
		public ScalpProcessingTools()
		{
		}

		// Token: 0x06004102 RID: 16642 RVA: 0x001352B0 File Offset: 0x001336B0
		public static List<int> HairRootToScalpIndices(List<Vector3> scalpVertices, List<Vector3> hairVertices, int segments, float accuracy = 1E-05f)
		{
			List<int> list = new List<int>();
			for (int i = 0; i < hairVertices.Count; i += segments)
			{
				for (int j = 0; j < scalpVertices.Count; j++)
				{
					if ((hairVertices[i] - scalpVertices[j]).sqrMagnitude < accuracy * accuracy)
					{
						list.Add(j);
						break;
					}
				}
			}
			return list;
		}

		// Token: 0x06004103 RID: 16643 RVA: 0x00135324 File Offset: 0x00133724
		public static List<int> ProcessIndices(List<int> scalpIndices, List<Vector3> scalpVertices, List<List<Vector3>> hairVerticesGroups, int segments, float accuracy = 1E-05f)
		{
			List<int> list = new List<int>();
			int num = 0;
			foreach (List<Vector3> list2 in hairVerticesGroups)
			{
				List<int> collection = ScalpProcessingTools.ProcessIndicesForMesh(num, scalpVertices, scalpIndices, list2, segments, accuracy);
				list.AddRange(collection);
				num += list2.Count;
			}
			for (int i = 0; i < list.Count; i++)
			{
				list[i] /= segments;
			}
			return list;
		}

		// Token: 0x06004104 RID: 16644 RVA: 0x001353CC File Offset: 0x001337CC
		private static List<int> ProcessIndicesForMesh(int startIndex, List<Vector3> scalpVertices, List<int> scalpIndices, List<Vector3> hairVertices, int segments, float accuracy = 1E-05f)
		{
			List<int> list = new List<int>();
			for (int i = 0; i < scalpIndices.Count; i++)
			{
				int index = scalpIndices[i];
				Vector3 b = scalpVertices[index];
				if (i % 3 == 0)
				{
					ScalpProcessingTools.FixNotCompletedPolygon(list);
				}
				for (int j = 0; j < hairVertices.Count; j += segments)
				{
					Vector3 a = hairVertices[j];
					if ((a - b).sqrMagnitude < accuracy * accuracy)
					{
						list.Add(startIndex + j);
						break;
					}
				}
			}
			ScalpProcessingTools.FixNotCompletedPolygon(list);
			return list;
		}

		// Token: 0x06004105 RID: 16645 RVA: 0x0013546C File Offset: 0x0013386C
		private static void FixNotCompletedPolygon(List<int> hairIndices)
		{
			int num = hairIndices.Count % 3;
			if (num > 0)
			{
				hairIndices.RemoveRange(hairIndices.Count - num, num);
			}
		}

		// Token: 0x06004106 RID: 16646 RVA: 0x00135498 File Offset: 0x00133898
		public static float MiddleDistanceBetweenPoints(Mesh mesh)
		{
			Vector3[] vertices = mesh.vertices;
			int[] indices = mesh.GetIndices(0);
			float num = 0f;
			int num2 = 0;
			for (int i = 0; i < Mathf.Min(500, indices.Length); i += 3)
			{
				Vector3 a = vertices[indices[i]];
				Vector3 b = vertices[indices[i + 1]];
				num += Vector3.Distance(a, b);
				num2++;
			}
			return num / (float)num2;
		}

		// Token: 0x06004107 RID: 16647 RVA: 0x00135518 File Offset: 0x00133918
		public static List<Vector3> ShiftToScalpRoot(List<Vector3> scalpVertices, List<Vector3> hairVertices, int segments)
		{
			for (int i = 0; i < hairVertices.Count; i += segments)
			{
				int index = 0;
				float num = float.MaxValue;
				for (int j = 0; j < scalpVertices.Count; j++)
				{
					float sqrMagnitude = (hairVertices[i] - scalpVertices[j]).sqrMagnitude;
					if (sqrMagnitude < num)
					{
						index = j;
						num = sqrMagnitude;
					}
				}
				hairVertices[i] = scalpVertices[index];
			}
			return hairVertices;
		}

		// Token: 0x040030E2 RID: 12514
		public const float Accuracy = 1E-05f;
	}
}
