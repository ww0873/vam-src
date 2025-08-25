using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000C98 RID: 3224
public class MeshSmooth
{
	// Token: 0x0600612B RID: 24875 RVA: 0x0024FB60 File Offset: 0x0024DF60
	public MeshSmooth(Vector3[] baseVerts, MeshPoly[] basePolys)
	{
		this.numVertices = baseVerts.Length;
		this.diffs = new Vector3[this.numVertices];
		this.vertexNeighbors = new int[this.numVertices][];
		this.vertexNeighborDistances = new float[this.numVertices][];
		Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
		for (int i = 0; i < basePolys.Length; i++)
		{
			for (int j = 0; j < basePolys[i].vertices.Length; j++)
			{
				int key = basePolys[i].vertices[j];
				List<int> list;
				if (!dictionary.TryGetValue(key, out list))
				{
					list = new List<int>();
					dictionary.Add(key, list);
				}
				list.Add(i);
			}
		}
		for (int k = 0; k < this.numVertices; k++)
		{
			Dictionary<int, bool> dictionary2 = new Dictionary<int, bool>();
			List<int> list2;
			if (dictionary.TryGetValue(k, out list2))
			{
				foreach (int num in list2)
				{
					for (int l = 0; l < basePolys[num].vertices.Length; l++)
					{
						int num2 = basePolys[num].vertices[l];
						if (num2 != k && !dictionary2.ContainsKey(num2))
						{
							dictionary2.Add(num2, true);
						}
					}
				}
			}
			this.vertexNeighbors[k] = new int[dictionary2.Count];
			this.vertexNeighborDistances[k] = new float[dictionary2.Count];
			int num3 = 0;
			foreach (int num4 in dictionary2.Keys)
			{
				this.vertexNeighbors[k][num3] = num4;
				Vector3 vector = baseVerts[k] - baseVerts[num4];
				float num5 = Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
				this.vertexNeighborDistances[k][num3] = num5;
				num3++;
			}
		}
	}

	// Token: 0x0600612C RID: 24876 RVA: 0x0024FDC8 File Offset: 0x0024E1C8
	public MeshSmooth(Vector3[] baseVerts, int[] baseTriangles)
	{
		this.vertexNeighbors = new int[baseVerts.Length][];
		for (int i = 0; i < baseVerts.Length; i++)
		{
			Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
			for (int j = 0; j < baseTriangles.Length; j += 3)
			{
				if (baseTriangles[j] == i || baseTriangles[j + 1] == i || baseTriangles[j + 2] == i)
				{
					if (baseTriangles[j] != i && !dictionary.ContainsKey(baseTriangles[j]))
					{
						dictionary.Add(baseTriangles[j], true);
					}
					if (baseTriangles[j + 1] != i && !dictionary.ContainsKey(baseTriangles[j + 1]))
					{
						dictionary.Add(baseTriangles[j + 1], true);
					}
					if (baseTriangles[j + 2] != i && !dictionary.ContainsKey(baseTriangles[j + 2]))
					{
						dictionary.Add(baseTriangles[j + 2], true);
					}
				}
			}
			this.vertexNeighbors[i] = new int[dictionary.Count];
			int num = 0;
			foreach (int num2 in dictionary.Keys)
			{
				this.vertexNeighbors[i][num] = num2;
				num++;
			}
		}
	}

	// Token: 0x0600612D RID: 24877 RVA: 0x0024FF14 File Offset: 0x0024E314
	public void LaplacianSmooth(Vector3[] inVerts, Vector3[] outVerts, int startIndex = 0, int stopIndex = 100000000)
	{
		int num = this.numVertices - 1;
		if (num > stopIndex)
		{
			num = stopIndex;
		}
		for (int i = startIndex; i <= num; i++)
		{
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			int[] array = this.vertexNeighbors[i];
			int num5 = array.Length;
			if (num5 > 0)
			{
				for (int j = 0; j < num5; j++)
				{
					int num6 = array[j];
					num2 += inVerts[num6].x;
					num3 += inVerts[num6].y;
					num4 += inVerts[num6].z;
				}
				float num7 = 1f / (float)num5;
				outVerts[i].x = num2 * num7;
				outVerts[i].y = num3 * num7;
				outVerts[i].z = num4 * num7;
			}
		}
	}

	// Token: 0x0600612E RID: 24878 RVA: 0x00250000 File Offset: 0x0024E400
	public void HCCorrection(Vector3[] inVerts, Vector3[] outVerts, float beta, int startIndex = 0, int stopIndex = 1000000000)
	{
		int num = this.numVertices - 1;
		if (num > stopIndex)
		{
			num = stopIndex;
		}
		for (int i = startIndex; i <= num; i++)
		{
			this.diffs[i].x = outVerts[i].x - inVerts[i].x;
			this.diffs[i].y = outVerts[i].y - inVerts[i].y;
			this.diffs[i].z = outVerts[i].z - inVerts[i].z;
		}
		for (int j = startIndex; j <= num; j++)
		{
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			int[] array = this.vertexNeighbors[j];
			int num5 = array.Length;
			if (num5 > 0)
			{
				for (int k = 0; k < num5; k++)
				{
					int num6 = array[k];
					num2 += this.diffs[num6].x;
					num3 += this.diffs[num6].y;
					num4 += this.diffs[num6].z;
				}
				float num7 = (1f - beta) / (float)num5;
				int num8 = j;
				outVerts[num8].x = outVerts[num8].x - (beta * this.diffs[j].x + num2 * num7);
				int num9 = j;
				outVerts[num9].y = outVerts[num9].y - (beta * this.diffs[j].y + num3 * num7);
				int num10 = j;
				outVerts[num10].z = outVerts[num10].z - (beta * this.diffs[j].z + num4 * num7);
			}
		}
	}

	// Token: 0x0600612F RID: 24879 RVA: 0x002501E4 File Offset: 0x0024E5E4
	public void SpringSmooth(Vector3[] inVerts, Vector3[] outVerts, float springFactor, float scale = 1f, int startIndex = 0, int stopIndex = 100000000)
	{
		int num = this.numVertices - 1;
		if (num > stopIndex)
		{
			num = stopIndex;
		}
		for (int i = startIndex; i <= num; i++)
		{
			Vector3 vector;
			vector.x = 0f;
			vector.y = 0f;
			vector.z = 0f;
			int[] array = this.vertexNeighbors[i];
			float[] array2 = this.vertexNeighborDistances[i];
			int num2 = array.Length;
			if (num2 > 0)
			{
				for (int j = 0; j < num2; j++)
				{
					int num3 = array[j];
					float num4 = array2[j] * scale;
					Vector3 vector2 = inVerts[num3] - inVerts[i];
					float num5 = Mathf.Sqrt(vector2.x * vector2.x + vector2.y * vector2.y + vector2.z * vector2.z);
					if (num5 != 0f)
					{
						float num6 = 1f / num5;
						Vector3 a;
						a.x = vector2.x * num6;
						a.y = vector2.y * num6;
						a.z = vector2.z * num6;
						vector += a * (num5 - num4) * springFactor;
					}
				}
			}
			outVerts[i] = inVerts[i] + vector;
		}
	}

	// Token: 0x04005111 RID: 20753
	private int numVertices;

	// Token: 0x04005112 RID: 20754
	private int[][] vertexNeighbors;

	// Token: 0x04005113 RID: 20755
	private float[][] vertexNeighborDistances;

	// Token: 0x04005114 RID: 20756
	private Vector3[] diffs;
}
