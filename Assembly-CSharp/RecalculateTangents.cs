using System;
using System.Threading;
using UnityEngine;

// Token: 0x02000C9D RID: 3229
public class RecalculateTangents
{
	// Token: 0x0600613C RID: 24892 RVA: 0x002515B8 File Offset: 0x0024F9B8
	public RecalculateTangents(int[] tris, Vector3[] verts, Vector3[] norms, Vector2[] uv, Vector4[] tangents, bool useSleep = false)
	{
		this._triangles = tris;
		this._vertices = verts;
		this._normals = norms;
		this._uv = uv;
		this._tangents = tangents;
		this._thisMarkerArray = new bool[this._vertices.Length];
		this._useSleep = useSleep;
	}

	// Token: 0x0600613D RID: 24893 RVA: 0x0025160B File Offset: 0x0024FA0B
	public void recalculateTangents(bool[] vertexDirty = null)
	{
		RecalculateTangents.recalculateTangentsFast(this._triangles, this._vertices, this._normals, this._uv, this._tangents, vertexDirty, this._thisMarkerArray, this._useSleep);
	}

	// Token: 0x0600613E RID: 24894 RVA: 0x0025163D File Offset: 0x0024FA3D
	public void recalculateTangentsAccurate()
	{
		RecalculateTangents.recalculateTangentsAccurate(this._triangles, this._vertices, this._normals, this._uv, ref this._tan1, ref this._tan2, this._tangents);
	}

	// Token: 0x0600613F RID: 24895 RVA: 0x0025166E File Offset: 0x0024FA6E
	public void recalculateTangents(int[] trianglesToUse, int[] verticesToUse)
	{
		RecalculateTangents.recalculateTangentsFast(this._triangles, this._vertices, this._normals, this._uv, this._tangents, trianglesToUse, verticesToUse, this._thisMarkerArray);
	}

	// Token: 0x06006140 RID: 24896 RVA: 0x0025169C File Offset: 0x0024FA9C
	public static void recalculateTangentsAccurate(int[] triangles, Vector3[] vertices, Vector3[] normals, Vector2[] uv, ref Vector3[] tan1, ref Vector3[] tan2, Vector4[] tangents)
	{
		int num = triangles.Length;
		int num2 = vertices.Length;
		if (tan1 == null)
		{
			tan1 = new Vector3[num2];
		}
		if (tan2 == null)
		{
			tan2 = new Vector3[num2];
		}
		for (int i = 0; i < num2; i++)
		{
			tan1[i].x = 0f;
			tan1[i].y = 0f;
			tan1[i].z = 0f;
			tan2[i].x = 0f;
			tan2[i].y = 0f;
			tan2[i].z = 0f;
		}
		for (int j = 0; j < num; j += 3)
		{
			int num3 = triangles[j];
			int num4 = triangles[j + 1];
			int num5 = triangles[j + 2];
			float num6 = vertices[num4].x - vertices[num3].x;
			float num7 = vertices[num4].y - vertices[num3].y;
			float num8 = vertices[num4].z - vertices[num3].z;
			float num9 = vertices[num5].x - vertices[num3].x;
			float num10 = vertices[num5].y - vertices[num3].y;
			float num11 = vertices[num5].z - vertices[num3].z;
			float num12 = uv[num4].x - uv[num3].x;
			float num13 = uv[num5].x - uv[num3].x;
			float num14 = uv[num4].y - uv[num3].y;
			float num15 = uv[num5].y - uv[num3].y;
			float num16 = num12 * num15 - num13 * num14;
			if (num16 == 0f)
			{
				num16 = 0.0001f;
			}
			float num17 = 1f / num16;
			float num18 = (num15 * num6 - num14 * num9) * num17;
			float num19 = (num15 * num7 - num14 * num10) * num17;
			float num20 = (num15 * num8 - num14 * num11) * num17;
			float num21 = (num12 * num9 - num13 * num6) * num17;
			float num22 = (num12 * num10 - num13 * num7) * num17;
			float num23 = (num12 * num11 - num13 * num8) * num17;
			Vector3[] array = tan1;
			int num24 = num3;
			array[num24].x = array[num24].x + num18;
			Vector3[] array2 = tan1;
			int num25 = num3;
			array2[num25].y = array2[num25].y + num19;
			Vector3[] array3 = tan1;
			int num26 = num3;
			array3[num26].z = array3[num26].z + num20;
			Vector3[] array4 = tan1;
			int num27 = num4;
			array4[num27].x = array4[num27].x + num18;
			Vector3[] array5 = tan1;
			int num28 = num4;
			array5[num28].y = array5[num28].y + num19;
			Vector3[] array6 = tan1;
			int num29 = num4;
			array6[num29].z = array6[num29].z + num20;
			Vector3[] array7 = tan1;
			int num30 = num5;
			array7[num30].x = array7[num30].x + num18;
			Vector3[] array8 = tan1;
			int num31 = num5;
			array8[num31].y = array8[num31].y + num19;
			Vector3[] array9 = tan1;
			int num32 = num5;
			array9[num32].z = array9[num32].z + num20;
			Vector3[] array10 = tan2;
			int num33 = num3;
			array10[num33].x = array10[num33].x + num21;
			Vector3[] array11 = tan2;
			int num34 = num3;
			array11[num34].y = array11[num34].y + num22;
			Vector3[] array12 = tan2;
			int num35 = num3;
			array12[num35].z = array12[num35].z + num23;
			Vector3[] array13 = tan2;
			int num36 = num4;
			array13[num36].x = array13[num36].x + num21;
			Vector3[] array14 = tan2;
			int num37 = num4;
			array14[num37].y = array14[num37].y + num22;
			Vector3[] array15 = tan2;
			int num38 = num4;
			array15[num38].z = array15[num38].z + num23;
			Vector3[] array16 = tan2;
			int num39 = num5;
			array16[num39].x = array16[num39].x + num21;
			Vector3[] array17 = tan2;
			int num40 = num5;
			array17[num40].y = array17[num40].y + num22;
			Vector3[] array18 = tan2;
			int num41 = num5;
			array18[num41].z = array18[num41].z + num23;
		}
		for (int k = 0; k < num2; k++)
		{
			Vector3 vector = normals[k];
			Vector3 vector2 = tan1[k];
			Vector3 normalized = (vector2 - vector * Vector3.Dot(vector, vector2)).normalized;
			tangents[k].x = normalized.x;
			tangents[k].y = normalized.y;
			tangents[k].z = normalized.z;
			tangents[k].w = ((Vector3.Dot(Vector3.Cross(vector, vector2), tan2[k]) >= 0f) ? 1f : -1f);
		}
	}

	// Token: 0x06006141 RID: 24897 RVA: 0x00251BB4 File Offset: 0x0024FFB4
	public static void recalculateTangentsFast(int[] triangles, Vector3[] vertices, Vector3[] normals, Vector2[] uv, Vector4[] tangents, bool[] vertexDirty = null, bool[] markerArray = null, bool useSleep = false)
	{
		int num = triangles.Length;
		int num2 = vertices.Length;
		if (markerArray == null)
		{
			if (RecalculateTangents.reusableMarkerArray == null || RecalculateTangents.reusableMarkerArray.Length < num2)
			{
				RecalculateTangents.reusableMarkerArray = new bool[num2];
			}
			markerArray = RecalculateTangents.reusableMarkerArray;
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
		for (int j = 0; j < num; j += 3)
		{
			num6++;
			if (useSleep && num6 > 5000)
			{
				num6 = 0;
				Thread.Sleep(0);
			}
			int num7 = triangles[j];
			int num8 = triangles[j + 1];
			int num9 = triangles[j + 2];
			if (vertexDirty == null || vertexDirty[num7] || vertexDirty[num8] || vertexDirty[num9])
			{
				float num10 = vertices[num8].x - vertices[num7].x;
				float num11 = vertices[num8].y - vertices[num7].y;
				float num12 = vertices[num8].z - vertices[num7].z;
				float num13 = vertices[num9].x - vertices[num7].x;
				float num14 = vertices[num9].y - vertices[num7].y;
				float num15 = vertices[num9].z - vertices[num7].z;
				float num16 = uv[num8].x - uv[num7].x;
				float num17 = uv[num9].x - uv[num7].x;
				float num18 = uv[num8].y - uv[num7].y;
				float num19 = uv[num9].y - uv[num7].y;
				float num20 = num16 * num19 - num17 * num18;
				if (num20 == 0f)
				{
					num20 = 0.0001f;
				}
				float num21 = 1f / num20;
				float num22 = (num19 * num10 - num18 * num13) * num21;
				float num23 = (num19 * num11 - num18 * num14) * num21;
				float num24 = (num19 * num12 - num18 * num15) * num21;
				if (vertexDirty == null || vertexDirty[num7])
				{
					if (!markerArray[num7])
					{
						markerArray[num7] = true;
						tangents[num7].x = num22;
						tangents[num7].y = num23;
						tangents[num7].z = num24;
					}
					else
					{
						int num25 = num7;
						tangents[num25].x = tangents[num25].x + num22;
						int num26 = num7;
						tangents[num26].y = tangents[num26].y + num23;
						int num27 = num7;
						tangents[num27].z = tangents[num27].z + num24;
					}
				}
				if (vertexDirty == null || vertexDirty[num8])
				{
					if (!markerArray[num8])
					{
						markerArray[num8] = true;
						tangents[num8].x = num22;
						tangents[num8].y = num23;
						tangents[num8].z = num24;
					}
					else
					{
						int num28 = num8;
						tangents[num28].x = tangents[num28].x + num22;
						int num29 = num8;
						tangents[num29].y = tangents[num29].y + num23;
						int num30 = num8;
						tangents[num30].z = tangents[num30].z + num24;
					}
				}
				if (vertexDirty == null || vertexDirty[num9])
				{
					if (!markerArray[num9])
					{
						markerArray[num9] = true;
						tangents[num9].x = num22;
						tangents[num9].y = num23;
						tangents[num9].z = num24;
					}
					else
					{
						int num31 = num9;
						tangents[num31].x = tangents[num31].x + num22;
						int num32 = num9;
						tangents[num32].y = tangents[num32].y + num23;
						int num33 = num9;
						tangents[num33].z = tangents[num33].z + num24;
					}
				}
			}
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
				float num34 = normals[k].x * tangents[k].x + normals[k].y * tangents[k].y + normals[k].z * tangents[k].z;
				Vector3 vector;
				vector.x = tangents[k].x - normals[k].x * num34;
				vector.y = tangents[k].y - normals[k].y * num34;
				vector.z = tangents[k].z - normals[k].z * num34;
				float num35 = Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
				float num36 = 1f / num35;
				tangents[k].x = vector.x * num36;
				tangents[k].y = vector.y * num36;
				tangents[k].z = vector.z * num36;
				tangents[k].w = -1f;
				markerArray[k] = false;
				if (vertexDirty != null)
				{
					vertexDirty[k] = false;
				}
			}
		}
	}

	// Token: 0x06006142 RID: 24898 RVA: 0x002521D8 File Offset: 0x002505D8
	public static void recalculateTangentsFast(int[] triangles, Vector3[] vertices, Vector3[] normals, Vector2[] uv, Vector4[] tangents, int[] trianglesToUse, int[] verticesToUse, bool[] markerArray = null)
	{
		int num = vertices.Length;
		if (markerArray == null)
		{
			if (RecalculateTangents.reusableMarkerArray == null || RecalculateTangents.reusableMarkerArray.Length < num)
			{
				RecalculateTangents.reusableMarkerArray = new bool[num];
			}
			markerArray = RecalculateTangents.reusableMarkerArray;
		}
		foreach (int num2 in trianglesToUse)
		{
			int num3 = triangles[num2];
			int num4 = triangles[num2 + 1];
			int num5 = triangles[num2 + 2];
			float num6 = vertices[num4].x - vertices[num3].x;
			float num7 = vertices[num4].y - vertices[num3].y;
			float num8 = vertices[num4].z - vertices[num3].z;
			float num9 = vertices[num5].x - vertices[num3].x;
			float num10 = vertices[num5].y - vertices[num3].y;
			float num11 = vertices[num5].z - vertices[num3].z;
			float num12 = uv[num4].x - uv[num3].x;
			float num13 = uv[num5].x - uv[num3].x;
			float num14 = uv[num4].y - uv[num3].y;
			float num15 = uv[num5].y - uv[num3].y;
			float num16 = 1f / (num12 * num15 - num13 * num14);
			float num17 = (num15 * num6 - num14 * num9) * num16;
			float num18 = (num15 * num7 - num14 * num10) * num16;
			float num19 = (num15 * num8 - num14 * num11) * num16;
			if (!markerArray[num3])
			{
				markerArray[num3] = true;
				tangents[num3].x = num17;
				tangents[num3].y = num18;
				tangents[num3].z = num19;
			}
			else
			{
				int num20 = num3;
				tangents[num20].x = tangents[num20].x + num17;
				int num21 = num3;
				tangents[num21].y = tangents[num21].y + num18;
				int num22 = num3;
				tangents[num22].z = tangents[num22].z + num19;
			}
			if (!markerArray[num4])
			{
				markerArray[num4] = true;
				tangents[num4].x = num17;
				tangents[num4].y = num18;
				tangents[num4].z = num19;
			}
			else
			{
				int num23 = num4;
				tangents[num23].x = tangents[num23].x + num17;
				int num24 = num4;
				tangents[num24].y = tangents[num24].y + num18;
				int num25 = num4;
				tangents[num25].z = tangents[num25].z + num19;
			}
			if (!markerArray[num5])
			{
				markerArray[num5] = true;
				tangents[num5].x = num17;
				tangents[num5].y = num18;
				tangents[num5].z = num19;
			}
			else
			{
				int num26 = num5;
				tangents[num26].x = tangents[num26].x + num17;
				int num27 = num5;
				tangents[num27].y = tangents[num27].y + num18;
				int num28 = num5;
				tangents[num28].z = tangents[num28].z + num19;
			}
		}
		foreach (int num29 in verticesToUse)
		{
			float num30 = normals[num29].x * tangents[num29].x + normals[num29].y * tangents[num29].y + normals[num29].z * tangents[num29].z;
			Vector3 vector;
			vector.x = tangents[num29].x - normals[num29].x * num30;
			vector.y = tangents[num29].y - normals[num29].y * num30;
			vector.z = tangents[num29].z - normals[num29].z * num30;
			float num31 = Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y + vector.z * vector.z);
			float num32 = 1f / num31;
			tangents[num29].x = vector.x * num32;
			tangents[num29].y = vector.y * num32;
			tangents[num29].z = vector.z * num32;
			tangents[num29].w = -1f;
			markerArray[num29] = false;
		}
	}

	// Token: 0x06006143 RID: 24899 RVA: 0x002526C4 File Offset: 0x00250AC4
	public static void recalculateTangents(Mesh mesh)
	{
		int[] triangles = mesh.triangles;
		Vector3[] vertices = mesh.vertices;
		Vector2[] uv = mesh.uv;
		Vector3[] normals = mesh.normals;
		int num = vertices.Length;
		Vector4[] tangents = new Vector4[num];
		Vector3[] array = null;
		Vector3[] array2 = null;
		RecalculateTangents.recalculateTangentsAccurate(triangles, vertices, normals, uv, ref array, ref array2, tangents);
		mesh.tangents = tangents;
	}

	// Token: 0x04005125 RID: 20773
	private int[] _triangles;

	// Token: 0x04005126 RID: 20774
	private Vector3[] _vertices;

	// Token: 0x04005127 RID: 20775
	private Vector3[] _normals;

	// Token: 0x04005128 RID: 20776
	private Vector2[] _uv;

	// Token: 0x04005129 RID: 20777
	private Vector4[] _tangents;

	// Token: 0x0400512A RID: 20778
	private bool[] _thisMarkerArray;

	// Token: 0x0400512B RID: 20779
	private bool _useSleep;

	// Token: 0x0400512C RID: 20780
	private Vector3[] _tan1;

	// Token: 0x0400512D RID: 20781
	private Vector3[] _tan2;

	// Token: 0x0400512E RID: 20782
	private static bool[] reusableMarkerArray;
}
