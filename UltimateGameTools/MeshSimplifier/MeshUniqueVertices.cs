using System;
using System.Collections.Generic;
using UnityEngine;

namespace UltimateGameTools.MeshSimplifier
{
	// Token: 0x02000476 RID: 1142
	[Serializable]
	public class MeshUniqueVertices
	{
		// Token: 0x06001D16 RID: 7446 RVA: 0x000A69A3 File Offset: 0x000A4DA3
		public MeshUniqueVertices()
		{
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06001D17 RID: 7447 RVA: 0x000A69AB File Offset: 0x000A4DAB
		public MeshUniqueVertices.ListIndices[] SubmeshesFaceList
		{
			get
			{
				return this.m_aFaceList;
			}
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06001D18 RID: 7448 RVA: 0x000A69B3 File Offset: 0x000A4DB3
		public List<Vector3> ListVertices
		{
			get
			{
				return this.m_listVertices;
			}
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06001D19 RID: 7449 RVA: 0x000A69BB File Offset: 0x000A4DBB
		public List<Vector3> ListVerticesWorld
		{
			get
			{
				return this.m_listVerticesWorld;
			}
		}

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06001D1A RID: 7450 RVA: 0x000A69C3 File Offset: 0x000A4DC3
		public List<MeshUniqueVertices.SerializableBoneWeight> ListBoneWeights
		{
			get
			{
				return this.m_listBoneWeights;
			}
		}

		// Token: 0x06001D1B RID: 7451 RVA: 0x000A69CC File Offset: 0x000A4DCC
		public void BuildData(Mesh sourceMesh, Vector3[] av3VerticesWorld)
		{
			Vector3[] vertices = sourceMesh.vertices;
			BoneWeight[] boneWeights = sourceMesh.boneWeights;
			Dictionary<MeshUniqueVertices.UniqueVertex, MeshUniqueVertices.RepeatedVertexList> dictionary = new Dictionary<MeshUniqueVertices.UniqueVertex, MeshUniqueVertices.RepeatedVertexList>();
			this.m_listVertices = new List<Vector3>();
			this.m_listVerticesWorld = new List<Vector3>();
			this.m_listBoneWeights = new List<MeshUniqueVertices.SerializableBoneWeight>();
			this.m_aFaceList = new MeshUniqueVertices.ListIndices[sourceMesh.subMeshCount];
			for (int i = 0; i < sourceMesh.subMeshCount; i++)
			{
				this.m_aFaceList[i] = new MeshUniqueVertices.ListIndices();
				int[] triangles = sourceMesh.GetTriangles(i);
				for (int j = 0; j < triangles.Length; j++)
				{
					MeshUniqueVertices.UniqueVertex key = new MeshUniqueVertices.UniqueVertex(vertices[triangles[j]]);
					if (dictionary.ContainsKey(key))
					{
						dictionary[key].Add(new MeshUniqueVertices.RepeatedVertex(j / 3, triangles[j]));
						this.m_aFaceList[i].m_listIndices.Add(dictionary[key].UniqueIndex);
					}
					else
					{
						int count = this.m_listVertices.Count;
						dictionary.Add(key, new MeshUniqueVertices.RepeatedVertexList(count, new MeshUniqueVertices.RepeatedVertex(j / 3, triangles[j])));
						this.m_listVertices.Add(vertices[triangles[j]]);
						this.m_listVerticesWorld.Add(av3VerticesWorld[triangles[j]]);
						this.m_aFaceList[i].m_listIndices.Add(count);
						if (boneWeights != null && boneWeights.Length > 0)
						{
							this.m_listBoneWeights.Add(new MeshUniqueVertices.SerializableBoneWeight(boneWeights[triangles[j]]));
						}
					}
				}
			}
		}

		// Token: 0x040018B7 RID: 6327
		[SerializeField]
		private List<Vector3> m_listVertices;

		// Token: 0x040018B8 RID: 6328
		[SerializeField]
		private List<Vector3> m_listVerticesWorld;

		// Token: 0x040018B9 RID: 6329
		[SerializeField]
		private List<MeshUniqueVertices.SerializableBoneWeight> m_listBoneWeights;

		// Token: 0x040018BA RID: 6330
		[SerializeField]
		private MeshUniqueVertices.ListIndices[] m_aFaceList;

		// Token: 0x02000477 RID: 1143
		[Serializable]
		public class ListIndices
		{
			// Token: 0x06001D1C RID: 7452 RVA: 0x000A6B6B File Offset: 0x000A4F6B
			public ListIndices()
			{
				this.m_listIndices = new List<int>();
			}

			// Token: 0x040018BB RID: 6331
			public List<int> m_listIndices;
		}

		// Token: 0x02000478 RID: 1144
		[Serializable]
		public class SerializableBoneWeight
		{
			// Token: 0x06001D1D RID: 7453 RVA: 0x000A6B80 File Offset: 0x000A4F80
			public SerializableBoneWeight(BoneWeight boneWeight)
			{
				this._boneIndex0 = boneWeight.boneIndex0;
				this._boneIndex1 = boneWeight.boneIndex1;
				this._boneIndex2 = boneWeight.boneIndex2;
				this._boneIndex3 = boneWeight.boneIndex3;
				this._boneWeight0 = boneWeight.weight0;
				this._boneWeight1 = boneWeight.weight1;
				this._boneWeight2 = boneWeight.weight2;
				this._boneWeight3 = boneWeight.weight3;
			}

			// Token: 0x06001D1E RID: 7454 RVA: 0x000A6BFC File Offset: 0x000A4FFC
			public BoneWeight ToBoneWeight()
			{
				return new BoneWeight
				{
					boneIndex0 = this._boneIndex0,
					boneIndex1 = this._boneIndex1,
					boneIndex2 = this._boneIndex2,
					boneIndex3 = this._boneIndex3,
					weight0 = this._boneWeight0,
					weight1 = this._boneWeight1,
					weight2 = this._boneWeight2,
					weight3 = this._boneWeight3
				};
			}

			// Token: 0x040018BC RID: 6332
			public int _boneIndex0;

			// Token: 0x040018BD RID: 6333
			public int _boneIndex1;

			// Token: 0x040018BE RID: 6334
			public int _boneIndex2;

			// Token: 0x040018BF RID: 6335
			public int _boneIndex3;

			// Token: 0x040018C0 RID: 6336
			public float _boneWeight0;

			// Token: 0x040018C1 RID: 6337
			public float _boneWeight1;

			// Token: 0x040018C2 RID: 6338
			public float _boneWeight2;

			// Token: 0x040018C3 RID: 6339
			public float _boneWeight3;
		}

		// Token: 0x02000479 RID: 1145
		public class UniqueVertex
		{
			// Token: 0x06001D1F RID: 7455 RVA: 0x000A6C7A File Offset: 0x000A507A
			public UniqueVertex(Vector3 v3Vertex)
			{
				this.FromVertex(v3Vertex);
			}

			// Token: 0x06001D20 RID: 7456 RVA: 0x000A6C8C File Offset: 0x000A508C
			public override bool Equals(object obj)
			{
				MeshUniqueVertices.UniqueVertex uniqueVertex = obj as MeshUniqueVertices.UniqueVertex;
				return uniqueVertex.m_nFixedX == this.m_nFixedX && uniqueVertex.m_nFixedY == this.m_nFixedY && uniqueVertex.m_nFixedZ == this.m_nFixedZ;
			}

			// Token: 0x06001D21 RID: 7457 RVA: 0x000A6CD3 File Offset: 0x000A50D3
			public override int GetHashCode()
			{
				return this.m_nFixedX + (this.m_nFixedY << 2) + (this.m_nFixedZ << 4);
			}

			// Token: 0x06001D22 RID: 7458 RVA: 0x000A6CED File Offset: 0x000A50ED
			public Vector3 ToVertex()
			{
				return new Vector3(this.FixedToCoord(this.m_nFixedX), this.FixedToCoord(this.m_nFixedY), this.FixedToCoord(this.m_nFixedZ));
			}

			// Token: 0x06001D23 RID: 7459 RVA: 0x000A6D18 File Offset: 0x000A5118
			public static bool operator ==(MeshUniqueVertices.UniqueVertex a, MeshUniqueVertices.UniqueVertex b)
			{
				return a.Equals(b);
			}

			// Token: 0x06001D24 RID: 7460 RVA: 0x000A6D21 File Offset: 0x000A5121
			public static bool operator !=(MeshUniqueVertices.UniqueVertex a, MeshUniqueVertices.UniqueVertex b)
			{
				return !a.Equals(b);
			}

			// Token: 0x06001D25 RID: 7461 RVA: 0x000A6D2D File Offset: 0x000A512D
			private void FromVertex(Vector3 vertex)
			{
				this.m_nFixedX = this.CoordToFixed(vertex.x);
				this.m_nFixedY = this.CoordToFixed(vertex.y);
				this.m_nFixedZ = this.CoordToFixed(vertex.z);
			}

			// Token: 0x06001D26 RID: 7462 RVA: 0x000A6D68 File Offset: 0x000A5168
			private int CoordToFixed(float fCoord)
			{
				int num = Mathf.FloorToInt(fCoord);
				int num2 = Mathf.FloorToInt((fCoord - (float)num) * 100000f);
				return num << 16 | num2;
			}

			// Token: 0x06001D27 RID: 7463 RVA: 0x000A6D94 File Offset: 0x000A5194
			private float FixedToCoord(int nFixed)
			{
				float num = (float)(nFixed & 65535) / 100000f;
				float num2 = (float)(nFixed >> 16);
				return num2 + num;
			}

			// Token: 0x040018C4 RID: 6340
			private int m_nFixedX;

			// Token: 0x040018C5 RID: 6341
			private int m_nFixedY;

			// Token: 0x040018C6 RID: 6342
			private int m_nFixedZ;

			// Token: 0x040018C7 RID: 6343
			private const float fDecimalMultiplier = 100000f;
		}

		// Token: 0x0200047A RID: 1146
		private class RepeatedVertex
		{
			// Token: 0x06001D28 RID: 7464 RVA: 0x000A6DB9 File Offset: 0x000A51B9
			public RepeatedVertex(int nFaceIndex, int nOriginalVertexIndex)
			{
				this._nFaceIndex = nFaceIndex;
				this._nOriginalVertexIndex = nOriginalVertexIndex;
			}

			// Token: 0x1700032C RID: 812
			// (get) Token: 0x06001D29 RID: 7465 RVA: 0x000A6DCF File Offset: 0x000A51CF
			public int FaceIndex
			{
				get
				{
					return this._nFaceIndex;
				}
			}

			// Token: 0x1700032D RID: 813
			// (get) Token: 0x06001D2A RID: 7466 RVA: 0x000A6DD7 File Offset: 0x000A51D7
			public int OriginalVertexIndex
			{
				get
				{
					return this._nOriginalVertexIndex;
				}
			}

			// Token: 0x040018C8 RID: 6344
			private int _nFaceIndex;

			// Token: 0x040018C9 RID: 6345
			private int _nOriginalVertexIndex;
		}

		// Token: 0x0200047B RID: 1147
		private class RepeatedVertexList
		{
			// Token: 0x06001D2B RID: 7467 RVA: 0x000A6DDF File Offset: 0x000A51DF
			public RepeatedVertexList(int nUniqueIndex, MeshUniqueVertices.RepeatedVertex repeatedVertex)
			{
				this.m_nUniqueIndex = nUniqueIndex;
				this.m_listRepeatedVertices = new List<MeshUniqueVertices.RepeatedVertex>();
				this.m_listRepeatedVertices.Add(repeatedVertex);
			}

			// Token: 0x1700032E RID: 814
			// (get) Token: 0x06001D2C RID: 7468 RVA: 0x000A6E05 File Offset: 0x000A5205
			public int UniqueIndex
			{
				get
				{
					return this.m_nUniqueIndex;
				}
			}

			// Token: 0x06001D2D RID: 7469 RVA: 0x000A6E0D File Offset: 0x000A520D
			public void Add(MeshUniqueVertices.RepeatedVertex repeatedVertex)
			{
				this.m_listRepeatedVertices.Add(repeatedVertex);
			}

			// Token: 0x040018CA RID: 6346
			private int m_nUniqueIndex;

			// Token: 0x040018CB RID: 6347
			private List<MeshUniqueVertices.RepeatedVertex> m_listRepeatedVertices;
		}
	}
}
