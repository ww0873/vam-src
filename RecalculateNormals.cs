using System;
using System.Threading;
using UnityEngine;

// Token: 0x02000C9C RID: 3228
public class RecalculateNormals
{
	// Token: 0x06006135 RID: 24885 RVA: 0x00250814 File Offset: 0x0024EC14
	public RecalculateNormals(int[] tris, Vector3[] verts, Vector3[] norms, Vector3[] surfaceNorms = null, bool useSleep = false)
	{
		this.triangles = tris;
		this.vertices = verts;
		this.normals = norms;
		if (surfaceNorms == null)
		{
			surfaceNorms = new Vector3[this.triangles.Length / 3];
		}
		this.surfaceNormals = surfaceNorms;
		this.thisMarkerArray = new bool[this.vertices.Length];
		this._useSleep = useSleep;
	}

	// Token: 0x06006136 RID: 24886 RVA: 0x00250877 File Offset: 0x0024EC77
	public void recalculateNormals(bool[] vertexDirty = null)
	{
		RecalculateNormals.recalculateNormals(this.triangles, this.vertices, this.normals, this.surfaceNormals, vertexDirty, this.thisMarkerArray, this._useSleep, true);
	}

	// Token: 0x06006137 RID: 24887 RVA: 0x002508A4 File Offset: 0x0024ECA4
	public void recalculateNormals(Vector3[] inputVertices)
	{
		RecalculateNormals.recalculateNormals(this.triangles, inputVertices, this.normals, this.surfaceNormals, null, this.thisMarkerArray, this._useSleep, true);
	}

	// Token: 0x06006138 RID: 24888 RVA: 0x002508CC File Offset: 0x0024ECCC
	public static void recalculateNormals(int[] triangles, Vector3[] vertices, Vector3[] normals, Vector3[] surfaceNormals, bool[] vertexDirty = null, bool[] markerArray = null, bool useSleep = false, bool normalizeSurfaceNormals = true)
	{
		int num = triangles.Length;
		int num2 = vertices.Length;
		if (markerArray == null)
		{
			if (RecalculateNormals.reusableVertexMarkerArray == null || RecalculateNormals.reusableVertexMarkerArray.Length < num2)
			{
				RecalculateNormals.reusableVertexMarkerArray = new bool[num2];
			}
			markerArray = RecalculateNormals.reusableVertexMarkerArray;
		}
		if (vertexDirty != null)
		{
			for (int i = 0; i < num; i += 3)
			{
				int num3 = triangles[i];
				int num4 = triangles[i + 1];
				int num5 = triangles[i + 2];
				if (vertexDirty[num3] || vertexDirty[num4] || vertexDirty[num5])
				{
					vertexDirty[num3] = (vertexDirty[num4] = (vertexDirty[num5] = true));
				}
			}
		}
		int num6 = 0;
		int num7 = 0;
		for (int j = 0; j < num; j += 3)
		{
			num6++;
			if (useSleep && num6 > 5000)
			{
				num6 = 0;
				Thread.Sleep(0);
			}
			int num8 = triangles[j];
			int num9 = triangles[j + 1];
			int num10 = triangles[j + 2];
			if (vertexDirty == null || vertexDirty[num8] || vertexDirty[num9] || vertexDirty[num10])
			{
				float num11 = vertices[num9].x - vertices[num8].x;
				float num12 = vertices[num9].y - vertices[num8].y;
				float num13 = vertices[num9].z - vertices[num8].z;
				float num14 = vertices[num10].x - vertices[num8].x;
				float num15 = vertices[num10].y - vertices[num8].y;
				float num16 = vertices[num10].z - vertices[num8].z;
				surfaceNormals[num7].x = num12 * num16 - num13 * num15;
				surfaceNormals[num7].y = num13 * num14 - num11 * num16;
				surfaceNormals[num7].z = num11 * num15 - num12 * num14;
				if (normalizeSurfaceNormals)
				{
					if (surfaceNormals[num7].x == 0f && surfaceNormals[num7].y == 0f && surfaceNormals[num7].z == 0f)
					{
						surfaceNormals[num7].z = 0.001f;
					}
					float num17 = Mathf.Sqrt(surfaceNormals[num7].x * surfaceNormals[num7].x + surfaceNormals[num7].y * surfaceNormals[num7].y + surfaceNormals[num7].z * surfaceNormals[num7].z);
					float num18 = 1f / num17;
					int num19 = num7;
					surfaceNormals[num19].x = surfaceNormals[num19].x * num18;
					int num20 = num7;
					surfaceNormals[num20].y = surfaceNormals[num20].y * num18;
					int num21 = num7;
					surfaceNormals[num21].z = surfaceNormals[num21].z * num18;
				}
				Vector3 vector = surfaceNormals[num7];
				if (vertexDirty == null || vertexDirty[num8])
				{
					if (!markerArray[num8])
					{
						markerArray[num8] = true;
						normals[num8].x = vector.x;
						normals[num8].y = vector.y;
						normals[num8].z = vector.z;
					}
					else
					{
						int num22 = num8;
						normals[num22].x = normals[num22].x + vector.x;
						int num23 = num8;
						normals[num23].y = normals[num23].y + vector.y;
						int num24 = num8;
						normals[num24].z = normals[num24].z + vector.z;
					}
				}
				if (vertexDirty == null || vertexDirty[num9])
				{
					if (!markerArray[num9])
					{
						markerArray[num9] = true;
						normals[num9].x = vector.x;
						normals[num9].y = vector.y;
						normals[num9].z = vector.z;
					}
					else
					{
						int num25 = num9;
						normals[num25].x = normals[num25].x + vector.x;
						int num26 = num9;
						normals[num26].y = normals[num26].y + vector.y;
						int num27 = num9;
						normals[num27].z = normals[num27].z + vector.z;
					}
				}
				if (vertexDirty == null || vertexDirty[num10])
				{
					if (!markerArray[num10])
					{
						markerArray[num10] = true;
						normals[num10].x = vector.x;
						normals[num10].y = vector.y;
						normals[num10].z = vector.z;
					}
					else
					{
						int num28 = num10;
						normals[num28].x = normals[num28].x + vector.x;
						int num29 = num10;
						normals[num29].y = normals[num29].y + vector.y;
						int num30 = num10;
						normals[num30].z = normals[num30].z + vector.z;
					}
				}
			}
			num7++;
		}
		for (int k = 0; k < num2; k++)
		{
			num6++;
			if (useSleep && num6 > 5000)
			{
				num6 = 0;
				Thread.Sleep(0);
			}
			if (markerArray[k])
			{
				if (normals[k].x == 0f && normals[k].y == 0f && normals[k].z == 0f)
				{
					if (k > 0)
					{
						normals[k] = normals[k - 1];
					}
					else
					{
						normals[k].z = 1f;
					}
				}
				float num31 = Mathf.Sqrt(normals[k].x * normals[k].x + normals[k].y * normals[k].y + normals[k].z * normals[k].z);
				float num32 = 1f / num31;
				int num33 = k;
				normals[num33].x = normals[num33].x * num32;
				int num34 = k;
				normals[num34].y = normals[num34].y * num32;
				int num35 = k;
				normals[num35].z = normals[num35].z * num32;
				markerArray[k] = false;
				if (vertexDirty != null)
				{
					vertexDirty[k] = false;
				}
			}
		}
	}

	// Token: 0x06006139 RID: 24889 RVA: 0x00250F8E File Offset: 0x0024F38E
	public void recalculateNormals(int[] trianglesToUse, int[] verticesToUse)
	{
		RecalculateNormals.recalculateNormals(this.triangles, this.vertices, this.normals, this.surfaceNormals, trianglesToUse, verticesToUse, this.thisMarkerArray, true);
	}

	// Token: 0x0600613A RID: 24890 RVA: 0x00250FB8 File Offset: 0x0024F3B8
	public static void recalculateNormals(int[] triangles, Vector3[] vertices, Vector3[] normals, Vector3[] surfaceNormals, int[] trianglesToUse, int[] verticesToUse, bool[] markerArray = null, bool normalizeSurfaceNormals = true)
	{
		int num = vertices.Length;
		if (markerArray == null)
		{
			if (RecalculateNormals.reusableVertexMarkerArray == null || RecalculateNormals.reusableVertexMarkerArray.Length < num)
			{
				RecalculateNormals.reusableVertexMarkerArray = new bool[num];
			}
			markerArray = RecalculateNormals.reusableVertexMarkerArray;
		}
		foreach (int num2 in trianglesToUse)
		{
			if (num2 < triangles.Length)
			{
				int num3 = num2 / 3;
				int num4 = triangles[num2];
				int num5 = triangles[num2 + 1];
				int num6 = triangles[num2 + 2];
				float num7 = vertices[num5].x - vertices[num4].x;
				float num8 = vertices[num5].y - vertices[num4].y;
				float num9 = vertices[num5].z - vertices[num4].z;
				float num10 = vertices[num6].x - vertices[num4].x;
				float num11 = vertices[num6].y - vertices[num4].y;
				float num12 = vertices[num6].z - vertices[num4].z;
				surfaceNormals[num3].x = num8 * num12 - num9 * num11;
				surfaceNormals[num3].y = num9 * num10 - num7 * num12;
				surfaceNormals[num3].z = num7 * num11 - num8 * num10;
				if (surfaceNormals[num3].x == 0f && surfaceNormals[num3].y == 0f && surfaceNormals[num3].z == 0f)
				{
					surfaceNormals[num3].z = 0.001f;
				}
				if (normalizeSurfaceNormals)
				{
					float num13 = Mathf.Sqrt(surfaceNormals[num3].x * surfaceNormals[num3].x + surfaceNormals[num3].y * surfaceNormals[num3].y + surfaceNormals[num3].z * surfaceNormals[num3].z);
					float num14 = 1f / num13;
					int num15 = num3;
					surfaceNormals[num15].x = surfaceNormals[num15].x * num14;
					int num16 = num3;
					surfaceNormals[num16].y = surfaceNormals[num16].y * num14;
					int num17 = num3;
					surfaceNormals[num17].z = surfaceNormals[num17].z * num14;
				}
				Vector3 vector = surfaceNormals[num3];
				if (!markerArray[num4])
				{
					markerArray[num4] = true;
					normals[num4].x = vector.x;
					normals[num4].y = vector.y;
					normals[num4].z = vector.z;
				}
				else
				{
					int num18 = num4;
					normals[num18].x = normals[num18].x + vector.x;
					int num19 = num4;
					normals[num19].y = normals[num19].y + vector.y;
					int num20 = num4;
					normals[num20].z = normals[num20].z + vector.z;
				}
				if (!markerArray[num5])
				{
					markerArray[num5] = true;
					normals[num5].x = vector.x;
					normals[num5].y = vector.y;
					normals[num5].z = vector.z;
				}
				else
				{
					int num21 = num5;
					normals[num21].x = normals[num21].x + vector.x;
					int num22 = num5;
					normals[num22].y = normals[num22].y + vector.y;
					int num23 = num5;
					normals[num23].z = normals[num23].z + vector.z;
				}
				if (!markerArray[num6])
				{
					markerArray[num6] = true;
					normals[num6].x = vector.x;
					normals[num6].y = vector.y;
					normals[num6].z = vector.z;
				}
				else
				{
					int num24 = num6;
					normals[num24].x = normals[num24].x + vector.x;
					int num25 = num6;
					normals[num25].y = normals[num25].y + vector.y;
					int num26 = num6;
					normals[num26].z = normals[num26].z + vector.z;
				}
			}
		}
		foreach (int num27 in verticesToUse)
		{
			if (num27 < markerArray.Length && markerArray[num27])
			{
				if (normals[num27].x == 0f && normals[num27].y == 0f && normals[num27].z == 0f)
				{
					if (num27 > 0)
					{
						normals[num27] = normals[num27 - 1];
					}
					else
					{
						normals[num27].z = 1f;
					}
				}
				float num28 = Mathf.Sqrt(normals[num27].x * normals[num27].x + normals[num27].y * normals[num27].y + normals[num27].z * normals[num27].z);
				float num29 = 1f / num28;
				int num30 = num27;
				normals[num30].x = normals[num30].x * num29;
				int num31 = num27;
				normals[num31].y = normals[num31].y * num29;
				int num32 = num27;
				normals[num32].z = normals[num32].z * num29;
				markerArray[num27] = false;
			}
		}
	}

	// Token: 0x0600613B RID: 24891 RVA: 0x00251574 File Offset: 0x0024F974
	public static void recalculateNormals(Mesh mesh)
	{
		int[] array = mesh.triangles;
		Vector3[] array2 = mesh.vertices;
		Vector3[] array3 = mesh.normals;
		Vector3[] array4 = new Vector3[array.Length / 3];
		RecalculateNormals.recalculateNormals(array, array2, array3, array4, null, null, false, true);
		mesh.normals = array3;
	}

	// Token: 0x0400511E RID: 20766
	private static bool[] reusableVertexMarkerArray;

	// Token: 0x0400511F RID: 20767
	private int[] triangles;

	// Token: 0x04005120 RID: 20768
	private Vector3[] vertices;

	// Token: 0x04005121 RID: 20769
	private Vector3[] normals;

	// Token: 0x04005122 RID: 20770
	private Vector3[] surfaceNormals;

	// Token: 0x04005123 RID: 20771
	private bool[] thisMarkerArray;

	// Token: 0x04005124 RID: 20772
	private bool _useSleep;
}
