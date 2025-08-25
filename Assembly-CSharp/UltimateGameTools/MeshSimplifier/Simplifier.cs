using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace UltimateGameTools.MeshSimplifier
{
	// Token: 0x0200047E RID: 1150
	public class Simplifier : MonoBehaviour
	{
		// Token: 0x06001D3A RID: 7482 RVA: 0x000A74E4 File Offset: 0x000A58E4
		public Simplifier()
		{
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06001D3B RID: 7483 RVA: 0x000A751A File Offset: 0x000A591A
		// (set) Token: 0x06001D3C RID: 7484 RVA: 0x000A7521 File Offset: 0x000A5921
		public static bool Cancelled
		{
			[CompilerGenerated]
			get
			{
				return Simplifier.<Cancelled>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				Simplifier.<Cancelled>k__BackingField = value;
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06001D3D RID: 7485 RVA: 0x000A7529 File Offset: 0x000A5929
		// (set) Token: 0x06001D3E RID: 7486 RVA: 0x000A7530 File Offset: 0x000A5930
		public static int CoroutineFrameMiliseconds
		{
			get
			{
				return Simplifier.m_nCoroutineFrameMiliseconds;
			}
			set
			{
				Simplifier.m_nCoroutineFrameMiliseconds = value;
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06001D3F RID: 7487 RVA: 0x000A7538 File Offset: 0x000A5938
		// (set) Token: 0x06001D40 RID: 7488 RVA: 0x000A7540 File Offset: 0x000A5940
		public bool CoroutineEnded
		{
			[CompilerGenerated]
			get
			{
				return this.<CoroutineEnded>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CoroutineEnded>k__BackingField = value;
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06001D41 RID: 7489 RVA: 0x000A7549 File Offset: 0x000A5949
		// (set) Token: 0x06001D42 RID: 7490 RVA: 0x000A7551 File Offset: 0x000A5951
		public bool UseEdgeLength
		{
			get
			{
				return this.m_bUseEdgeLength;
			}
			set
			{
				this.m_bUseEdgeLength = value;
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06001D43 RID: 7491 RVA: 0x000A755A File Offset: 0x000A595A
		// (set) Token: 0x06001D44 RID: 7492 RVA: 0x000A7562 File Offset: 0x000A5962
		public bool UseCurvature
		{
			get
			{
				return this.m_bUseCurvature;
			}
			set
			{
				this.m_bUseCurvature = value;
			}
		}

		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06001D45 RID: 7493 RVA: 0x000A756B File Offset: 0x000A596B
		// (set) Token: 0x06001D46 RID: 7494 RVA: 0x000A7573 File Offset: 0x000A5973
		public bool ProtectTexture
		{
			get
			{
				return this.m_bProtectTexture;
			}
			set
			{
				this.m_bProtectTexture = value;
			}
		}

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06001D47 RID: 7495 RVA: 0x000A757C File Offset: 0x000A597C
		// (set) Token: 0x06001D48 RID: 7496 RVA: 0x000A7584 File Offset: 0x000A5984
		public bool LockBorder
		{
			get
			{
				return this.m_bLockBorder;
			}
			set
			{
				this.m_bLockBorder = value;
			}
		}

		// Token: 0x06001D49 RID: 7497 RVA: 0x000A7590 File Offset: 0x000A5990
		public IEnumerator ProgressiveMesh(GameObject gameObject, Mesh sourceMesh, RelevanceSphere[] aRelevanceSpheres, string strProgressDisplayObjectName = "", Simplifier.ProgressDelegate progress = null)
		{
			this.m_meshOriginal = sourceMesh;
			Vector3[] aVerticesWorld = Simplifier.GetWorldVertices(gameObject);
			if (aVerticesWorld == null)
			{
				this.CoroutineEnded = true;
				yield break;
			}
			this.m_listVertexMap = new List<int>();
			this.m_listVertexPermutationBack = new List<int>();
			this.m_listVertices = new List<Simplifier.Vertex>();
			this.m_aListTriangles = new Simplifier.TriangleList[this.m_meshOriginal.subMeshCount];
			if (progress != null)
			{
				progress("Preprocessing mesh: " + strProgressDisplayObjectName, "Building unique vertex data", 1f);
				if (Simplifier.Cancelled)
				{
					this.CoroutineEnded = true;
					yield break;
				}
			}
			this.m_meshUniqueVertices = new MeshUniqueVertices();
			this.m_meshUniqueVertices.BuildData(this.m_meshOriginal, aVerticesWorld);
			this.m_nOriginalMeshVertexCount = this.m_meshUniqueVertices.ListVertices.Count;
			this.m_fOriginalMeshSize = Mathf.Max(new float[]
			{
				this.m_meshOriginal.bounds.size.x,
				this.m_meshOriginal.bounds.size.y,
				this.m_meshOriginal.bounds.size.z
			});
			this.m_listHeap = new List<Simplifier.Vertex>(this.m_meshUniqueVertices.ListVertices.Count);
			for (int i = 0; i < this.m_meshUniqueVertices.ListVertices.Count; i++)
			{
				this.m_listVertexMap.Add(-1);
				this.m_listVertexPermutationBack.Add(-1);
			}
			Vector2[] av2Mapping = this.m_meshOriginal.uv;
			this.AddVertices(this.m_meshUniqueVertices.ListVertices, this.m_meshUniqueVertices.ListVerticesWorld, this.m_meshUniqueVertices.ListBoneWeights);
			for (int j = 0; j < this.m_meshOriginal.subMeshCount; j++)
			{
				int[] triangles = this.m_meshOriginal.GetTriangles(j);
				this.m_aListTriangles[j] = new Simplifier.TriangleList();
				this.AddFaceListSubMesh(j, this.m_meshUniqueVertices.SubmeshesFaceList[j].m_listIndices, triangles, av2Mapping);
			}
			if (Application.isEditor && !Application.isPlaying)
			{
				IEnumerator enumerator = this.ComputeAllEdgeCollapseCosts(strProgressDisplayObjectName, gameObject.transform, aRelevanceSpheres, progress);
				while (enumerator.MoveNext())
				{
					if (Simplifier.Cancelled)
					{
						this.CoroutineEnded = true;
						yield break;
					}
				}
			}
			else
			{
				yield return base.StartCoroutine(this.ComputeAllEdgeCollapseCosts(strProgressDisplayObjectName, gameObject.transform, aRelevanceSpheres, progress));
			}
			int nVertices = this.m_listVertices.Count;
			Stopwatch sw = Stopwatch.StartNew();
			while (this.m_listVertices.Count > 0)
			{
				if (progress != null && (this.m_listVertices.Count & 255) == 0)
				{
					progress("Preprocessing mesh: " + strProgressDisplayObjectName, "Collapsing edges", 1f - (float)this.m_listVertices.Count / (float)nVertices);
					if (Simplifier.Cancelled)
					{
						this.CoroutineEnded = true;
						yield break;
					}
				}
				if (sw.ElapsedMilliseconds > (long)Simplifier.CoroutineFrameMiliseconds && Simplifier.CoroutineFrameMiliseconds > 0)
				{
					yield return null;
					sw = Stopwatch.StartNew();
				}
				Simplifier.Vertex mn = this.MinimumCostEdge();
				this.m_listVertexPermutationBack[this.m_listVertices.Count - 1] = mn.m_nID;
				this.m_listVertexMap[mn.m_nID] = ((mn.m_collapse == null) ? -1 : mn.m_collapse.m_nID);
				this.Collapse(mn, mn.m_collapse, true, gameObject.transform, aRelevanceSpheres);
			}
			this.m_listHeap.Clear();
			this.CoroutineEnded = true;
			yield break;
		}

		// Token: 0x06001D4A RID: 7498 RVA: 0x000A75D0 File Offset: 0x000A59D0
		public IEnumerator ComputeMeshWithVertexCount(GameObject gameObject, Mesh meshOut, int nVertices, string strProgressDisplayObjectName = "", Simplifier.ProgressDelegate progress = null)
		{
			if (this.GetOriginalMeshUniqueVertexCount() == -1)
			{
				this.CoroutineEnded = true;
				yield break;
			}
			if (nVertices < 3)
			{
				this.CoroutineEnded = true;
				yield break;
			}
			if (nVertices >= this.GetOriginalMeshUniqueVertexCount())
			{
				meshOut.triangles = new int[0];
				meshOut.subMeshCount = this.m_meshOriginal.subMeshCount;
				meshOut.vertices = this.m_meshOriginal.vertices;
				meshOut.normals = this.m_meshOriginal.normals;
				meshOut.tangents = this.m_meshOriginal.tangents;
				meshOut.uv = this.m_meshOriginal.uv;
				meshOut.uv2 = this.m_meshOriginal.uv2;
				meshOut.colors32 = this.m_meshOriginal.colors32;
				meshOut.boneWeights = this.m_meshOriginal.boneWeights;
				meshOut.bindposes = this.m_meshOriginal.bindposes;
				meshOut.triangles = this.m_meshOriginal.triangles;
				meshOut.subMeshCount = this.m_meshOriginal.subMeshCount;
				for (int i = 0; i < this.m_meshOriginal.subMeshCount; i++)
				{
					meshOut.SetTriangles(this.m_meshOriginal.GetTriangles(i), i);
				}
				meshOut.name = gameObject.name + " simplified mesh";
				this.CoroutineEnded = true;
				yield break;
			}
			this.m_listVertices = new List<Simplifier.Vertex>();
			this.m_aListTriangles = new Simplifier.TriangleList[this.m_meshOriginal.subMeshCount];
			List<Simplifier.Vertex> listVertices = new List<Simplifier.Vertex>();
			this.AddVertices(this.m_meshUniqueVertices.ListVertices, this.m_meshUniqueVertices.ListVerticesWorld, this.m_meshUniqueVertices.ListBoneWeights);
			for (int j = 0; j < this.m_listVertices.Count; j++)
			{
				this.m_listVertices[j].m_collapse = ((this.m_listVertexMap[j] != -1) ? this.m_listVertices[this.m_listVertexMap[j]] : null);
				listVertices.Add(this.m_listVertices[this.m_listVertexPermutationBack[j]]);
			}
			Vector2[] av2Mapping = this.m_meshOriginal.uv;
			for (int k = 0; k < this.m_meshOriginal.subMeshCount; k++)
			{
				int[] triangles = this.m_meshOriginal.GetTriangles(k);
				this.m_aListTriangles[k] = new Simplifier.TriangleList();
				this.AddFaceListSubMesh(k, this.m_meshUniqueVertices.SubmeshesFaceList[k].m_listIndices, triangles, av2Mapping);
			}
			int nTotalVertices = listVertices.Count;
			Stopwatch sw = Stopwatch.StartNew();
			while (listVertices.Count > nVertices)
			{
				if (progress != null && nTotalVertices != nVertices && (listVertices.Count & 255) == 0)
				{
					float fT = 1f - (float)(listVertices.Count - nVertices) / (float)(nTotalVertices - nVertices);
					progress("Simplifying mesh: " + strProgressDisplayObjectName, "Collapsing edges", fT);
					if (Simplifier.Cancelled)
					{
						this.CoroutineEnded = true;
						yield break;
					}
				}
				Simplifier.Vertex mn = listVertices[listVertices.Count - 1];
				listVertices.RemoveAt(listVertices.Count - 1);
				this.Collapse(mn, mn.m_collapse, false, null, null);
				if (sw.ElapsedMilliseconds > (long)Simplifier.CoroutineFrameMiliseconds && Simplifier.CoroutineFrameMiliseconds > 0)
				{
					yield return null;
					sw = Stopwatch.StartNew();
				}
			}
			Vector3[] av3Vertices = new Vector3[this.m_listVertices.Count];
			for (int l = 0; l < this.m_listVertices.Count; l++)
			{
				this.m_listVertices[l].m_nID = l;
				av3Vertices[l] = this.m_listVertices[l].m_v3Position;
			}
			if (Application.isEditor && !Application.isPlaying)
			{
				IEnumerator enumerator = this.ConsolidateMesh(gameObject, this.m_meshOriginal, meshOut, this.m_aListTriangles, av3Vertices, strProgressDisplayObjectName, progress);
				while (enumerator.MoveNext())
				{
					if (Simplifier.Cancelled)
					{
						this.CoroutineEnded = true;
						yield break;
					}
				}
			}
			else
			{
				yield return base.StartCoroutine(this.ConsolidateMesh(gameObject, this.m_meshOriginal, meshOut, this.m_aListTriangles, av3Vertices, strProgressDisplayObjectName, progress));
			}
			this.CoroutineEnded = true;
			yield break;
		}

		// Token: 0x06001D4B RID: 7499 RVA: 0x000A7610 File Offset: 0x000A5A10
		public int GetOriginalMeshUniqueVertexCount()
		{
			return this.m_nOriginalMeshVertexCount;
		}

		// Token: 0x06001D4C RID: 7500 RVA: 0x000A7618 File Offset: 0x000A5A18
		public int GetOriginalMeshTriangleCount()
		{
			return this.m_meshOriginal.triangles.Length / 3;
		}

		// Token: 0x06001D4D RID: 7501 RVA: 0x000A762C File Offset: 0x000A5A2C
		public static Vector3[] GetWorldVertices(GameObject gameObject)
		{
			Vector3[] array = null;
			SkinnedMeshRenderer component = gameObject.GetComponent<SkinnedMeshRenderer>();
			MeshFilter component2 = gameObject.GetComponent<MeshFilter>();
			if (component != null)
			{
				if (component.sharedMesh == null)
				{
					return null;
				}
				array = component.sharedMesh.vertices;
				BoneWeight[] boneWeights = component.sharedMesh.boneWeights;
				Matrix4x4[] bindposes = component.sharedMesh.bindposes;
				Transform[] bones = component.bones;
				if (array == null || boneWeights == null || bindposes == null || bones == null)
				{
					return null;
				}
				if (boneWeights.Length == 0 || bindposes.Length == 0 || bones.Length == 0)
				{
					return null;
				}
				for (int i = 0; i < array.Length; i++)
				{
					BoneWeight boneWeight = boneWeights[i];
					Vector3 vector = Vector3.zero;
					if (Math.Abs(boneWeight.weight0) > 1E-05f)
					{
						Vector3 point = bindposes[boneWeight.boneIndex0].MultiplyPoint3x4(array[i]);
						vector += bones[boneWeight.boneIndex0].transform.localToWorldMatrix.MultiplyPoint3x4(point) * boneWeight.weight0;
					}
					if (Math.Abs(boneWeight.weight1) > 1E-05f)
					{
						Vector3 point = bindposes[boneWeight.boneIndex1].MultiplyPoint3x4(array[i]);
						vector += bones[boneWeight.boneIndex1].transform.localToWorldMatrix.MultiplyPoint3x4(point) * boneWeight.weight1;
					}
					if (Math.Abs(boneWeight.weight2) > 1E-05f)
					{
						Vector3 point = bindposes[boneWeight.boneIndex2].MultiplyPoint3x4(array[i]);
						vector += bones[boneWeight.boneIndex2].transform.localToWorldMatrix.MultiplyPoint3x4(point) * boneWeight.weight2;
					}
					if (Math.Abs(boneWeight.weight3) > 1E-05f)
					{
						Vector3 point = bindposes[boneWeight.boneIndex3].MultiplyPoint3x4(array[i]);
						vector += bones[boneWeight.boneIndex3].transform.localToWorldMatrix.MultiplyPoint3x4(point) * boneWeight.weight3;
					}
					array[i] = vector;
				}
			}
			else if (component2 != null)
			{
				if (component2.sharedMesh == null)
				{
					return null;
				}
				array = component2.sharedMesh.vertices;
				if (array == null)
				{
					return null;
				}
				for (int j = 0; j < array.Length; j++)
				{
					array[j] = gameObject.transform.TransformPoint(array[j]);
				}
			}
			return array;
		}

		// Token: 0x06001D4E RID: 7502 RVA: 0x000A7930 File Offset: 0x000A5D30
		private IEnumerator ConsolidateMesh(GameObject gameObject, Mesh meshIn, Mesh meshOut, Simplifier.TriangleList[] aListTriangles, Vector3[] av3Vertices, string strProgressDisplayObjectName = "", Simplifier.ProgressDelegate progress = null)
		{
			Vector3[] av3NormalsIn = meshIn.normals;
			Vector4[] av4TangentsIn = meshIn.tangents;
			Vector2[] av2Mapping1In = meshIn.uv;
			Vector2[] av2Mapping2In = meshIn.uv2;
			Color[] acolColorsIn = meshIn.colors;
			Color32[] aColors32In = meshIn.colors32;
			List<List<int>> listlistIndicesOut = new List<List<int>>();
			List<Vector3> listVerticesOut = new List<Vector3>();
			List<Vector3> listNormalsOut = new List<Vector3>();
			List<Vector4> listTangentsOut = new List<Vector4>();
			List<Vector2> listMapping1Out = new List<Vector2>();
			List<Vector2> listMapping2Out = new List<Vector2>();
			List<Color32> listColors32Out = new List<Color32>();
			List<BoneWeight> listBoneWeightsOut = new List<BoneWeight>();
			Dictionary<Simplifier.VertexDataHash, int> dicVertexDataHash2Index = new Dictionary<Simplifier.VertexDataHash, int>(new Simplifier.VertexDataHashComparer());
			bool bUV = av2Mapping1In != null && av2Mapping1In.Length > 0;
			bool bUV2 = av2Mapping2In != null && av2Mapping2In.Length > 0;
			bool bNormal = av3NormalsIn != null && av3NormalsIn.Length > 0;
			bool bTangent = av4TangentsIn != null && av4TangentsIn.Length > 0;
			Stopwatch sw = Stopwatch.StartNew();
			for (int nSubMesh = 0; nSubMesh < aListTriangles.Length; nSubMesh++)
			{
				List<int> listIndicesOut = new List<int>();
				string strMesh = (aListTriangles.Length <= 1) ? "Consolidating mesh" : ("Consolidating submesh " + (nSubMesh + 1));
				for (int i = 0; i < aListTriangles[nSubMesh].m_listTriangles.Count; i++)
				{
					if (progress != null && (i & 255) == 0)
					{
						float fT = (aListTriangles[nSubMesh].m_listTriangles.Count != 1) ? ((float)i / (float)(aListTriangles[nSubMesh].m_listTriangles.Count - 1)) : 1f;
						progress("Simplifying mesh: " + strProgressDisplayObjectName, strMesh, fT);
						if (Simplifier.Cancelled)
						{
							yield break;
						}
					}
					if (sw.ElapsedMilliseconds > (long)Simplifier.CoroutineFrameMiliseconds && Simplifier.CoroutineFrameMiliseconds > 0)
					{
						yield return null;
						sw = Stopwatch.StartNew();
					}
					for (int j = 0; j < 3; j++)
					{
						int num = aListTriangles[nSubMesh].m_listTriangles[i].IndicesUV[j];
						int num2 = aListTriangles[nSubMesh].m_listTriangles[i].Indices[j];
						bool flag = false;
						Vector3 v3Position = aListTriangles[nSubMesh].m_listTriangles[i].Vertices[j].m_v3Position;
						Vector3 vector = (!bNormal) ? Vector3.zero : av3NormalsIn[num2];
						Vector4 item = (!bTangent) ? Vector4.zero : av4TangentsIn[num2];
						Vector2 vector2 = (!bUV) ? Vector2.zero : av2Mapping1In[num];
						Vector2 vector3 = (!bUV2) ? Vector2.zero : av2Mapping2In[num2];
						Color32 color = new Color32(0, 0, 0, 0);
						if (acolColorsIn != null && acolColorsIn.Length > 0)
						{
							color = acolColorsIn[num2];
							flag = true;
						}
						else if (aColors32In != null && aColors32In.Length > 0)
						{
							color = aColors32In[num2];
							flag = true;
						}
						Simplifier.VertexDataHash vertexDataHash = new Simplifier.VertexDataHash(v3Position, vector, vector2, vector3, color);
						if (dicVertexDataHash2Index.ContainsKey(vertexDataHash))
						{
							listIndicesOut.Add(dicVertexDataHash2Index[vertexDataHash]);
						}
						else
						{
							dicVertexDataHash2Index.Add(vertexDataHash, listVerticesOut.Count);
							listVerticesOut.Add(vertexDataHash.Vertex);
							if (bNormal)
							{
								listNormalsOut.Add(vector);
							}
							if (bUV)
							{
								listMapping1Out.Add(vector2);
							}
							if (bUV2)
							{
								listMapping2Out.Add(vector3);
							}
							if (bTangent)
							{
								listTangentsOut.Add(item);
							}
							if (flag)
							{
								listColors32Out.Add(color);
							}
							if (aListTriangles[nSubMesh].m_listTriangles[i].Vertices[j].m_bHasBoneWeight)
							{
								listBoneWeightsOut.Add(aListTriangles[nSubMesh].m_listTriangles[i].Vertices[j].m_boneWeight);
							}
							listIndicesOut.Add(listVerticesOut.Count - 1);
						}
					}
				}
				listlistIndicesOut.Add(listIndicesOut);
			}
			meshOut.triangles = new int[0];
			meshOut.vertices = listVerticesOut.ToArray();
			meshOut.normals = ((listNormalsOut.Count <= 0) ? null : listNormalsOut.ToArray());
			meshOut.tangents = ((listTangentsOut.Count <= 0) ? null : listTangentsOut.ToArray());
			meshOut.uv = ((listMapping1Out.Count <= 0) ? null : listMapping1Out.ToArray());
			meshOut.uv2 = ((listMapping2Out.Count <= 0) ? null : listMapping2Out.ToArray());
			meshOut.colors32 = ((listColors32Out.Count <= 0) ? null : listColors32Out.ToArray());
			meshOut.boneWeights = ((listBoneWeightsOut.Count <= 0) ? null : listBoneWeightsOut.ToArray());
			meshOut.bindposes = meshIn.bindposes;
			meshOut.subMeshCount = listlistIndicesOut.Count;
			for (int k = 0; k < listlistIndicesOut.Count; k++)
			{
				meshOut.SetTriangles(listlistIndicesOut[k].ToArray(), k);
			}
			meshOut.name = gameObject.name + " simplified mesh";
			progress("Simplifying mesh: " + strProgressDisplayObjectName, "Mesh consolidation done", 1f);
			yield break;
		}

		// Token: 0x06001D4F RID: 7503 RVA: 0x000A7971 File Offset: 0x000A5D71
		private int MapVertex(int nVertex, int nMax)
		{
			if (nMax <= 0)
			{
				return 0;
			}
			while (nVertex >= nMax)
			{
				nVertex = this.m_listVertexMap[nVertex];
			}
			return nVertex;
		}

		// Token: 0x06001D50 RID: 7504 RVA: 0x000A7998 File Offset: 0x000A5D98
		private float ComputeEdgeCollapseCost(Simplifier.Vertex u, Simplifier.Vertex v, float fRelevanceBias)
		{
			bool bUseEdgeLength = this.m_bUseEdgeLength;
			bool bUseCurvature = this.m_bUseCurvature;
			bool bProtectTexture = this.m_bProtectTexture;
			bool bLockBorder = this.m_bLockBorder;
			float num = (!bUseEdgeLength) ? 1f : (Vector3.Magnitude(v.m_v3Position - u.m_v3Position) / this.m_fOriginalMeshSize);
			float num2 = 0.001f;
			List<Simplifier.Triangle> list = new List<Simplifier.Triangle>();
			for (int i = 0; i < u.m_listFaces.Count; i++)
			{
				if (u.m_listFaces[i].HasVertex(v))
				{
					list.Add(u.m_listFaces[i]);
				}
			}
			if (bUseCurvature)
			{
				for (int i = 0; i < u.m_listFaces.Count; i++)
				{
					float num3 = 1f;
					for (int j = 0; j < list.Count; j++)
					{
						float num4 = Vector3.Dot(u.m_listFaces[i].Normal, list[j].Normal);
						num3 = Mathf.Min(num3, (1f - num4) / 2f);
					}
					num2 = Mathf.Max(num2, num3);
				}
			}
			if (u.IsBorder() && list.Count > 1)
			{
				num2 = 1f;
			}
			if (bProtectTexture)
			{
				bool flag = true;
				for (int i = 0; i < u.m_listFaces.Count; i++)
				{
					for (int k = 0; k < list.Count; k++)
					{
						if (!u.m_listFaces[i].HasUVData)
						{
							flag = false;
							break;
						}
						if (u.m_listFaces[i].TexAt(u) == list[k].TexAt(u))
						{
							flag = false;
						}
					}
				}
				if (flag)
				{
					num2 = 1f;
				}
			}
			if (bLockBorder && u.IsBorder())
			{
				num2 = 10000000f;
			}
			num2 += fRelevanceBias;
			return num * num2;
		}

		// Token: 0x06001D51 RID: 7505 RVA: 0x000A7BBC File Offset: 0x000A5FBC
		private void ComputeEdgeCostAtVertex(Simplifier.Vertex v, Transform transform, RelevanceSphere[] aRelevanceSpheres)
		{
			if (v.m_listNeighbors.Count == 0)
			{
				v.m_collapse = null;
				v.m_fObjDist = -0.01f;
				return;
			}
			v.m_fObjDist = 10000000f;
			v.m_collapse = null;
			float fRelevanceBias = 0f;
			if (aRelevanceSpheres != null)
			{
				for (int i = 0; i < aRelevanceSpheres.Length; i++)
				{
					Matrix4x4 matrix4x = Matrix4x4.TRS(aRelevanceSpheres[i].m_v3Position, Quaternion.Euler(aRelevanceSpheres[i].m_v3Rotation), aRelevanceSpheres[i].m_v3Scale);
					Vector3 v3PositionWorld = v.m_v3PositionWorld;
					if (matrix4x.inverse.MultiplyPoint(v3PositionWorld).magnitude <= 0.5f)
					{
						fRelevanceBias = aRelevanceSpheres[i].m_fRelevance;
					}
				}
			}
			for (int j = 0; j < v.m_listNeighbors.Count; j++)
			{
				float num = this.ComputeEdgeCollapseCost(v, v.m_listNeighbors[j], fRelevanceBias);
				if (v.m_collapse == null || num < v.m_fObjDist)
				{
					v.m_collapse = v.m_listNeighbors[j];
					v.m_fObjDist = num;
				}
			}
		}

		// Token: 0x06001D52 RID: 7506 RVA: 0x000A7CE0 File Offset: 0x000A60E0
		private IEnumerator ComputeAllEdgeCollapseCosts(string strProgressDisplayObjectName, Transform transform, RelevanceSphere[] aRelevanceSpheres, Simplifier.ProgressDelegate progress = null)
		{
			Stopwatch sw = Stopwatch.StartNew();
			for (int i = 0; i < this.m_listVertices.Count; i++)
			{
				if (progress != null && (i & 255) == 0)
				{
					progress("Preprocessing mesh: " + strProgressDisplayObjectName, "Computing edge collapse cost", (this.m_listVertices.Count != 1) ? ((float)i / ((float)this.m_listVertices.Count - 1f)) : 1f);
					if (Simplifier.Cancelled)
					{
						yield break;
					}
				}
				if (sw.ElapsedMilliseconds > (long)Simplifier.CoroutineFrameMiliseconds && Simplifier.CoroutineFrameMiliseconds > 0)
				{
					yield return null;
					sw = Stopwatch.StartNew();
				}
				this.ComputeEdgeCostAtVertex(this.m_listVertices[i], transform, aRelevanceSpheres);
				this.HeapAdd(this.m_listVertices[i]);
			}
			yield break;
		}

		// Token: 0x06001D53 RID: 7507 RVA: 0x000A7D18 File Offset: 0x000A6118
		private void Collapse(Simplifier.Vertex u, Simplifier.Vertex v, bool bRecompute, Transform transform, RelevanceSphere[] aRelevanceSpheres)
		{
			if (v == null)
			{
				u.Destructor(this);
				return;
			}
			List<Simplifier.Vertex> list = new List<Simplifier.Vertex>();
			for (int i = 0; i < u.m_listNeighbors.Count; i++)
			{
				list.Add(u.m_listNeighbors[i]);
			}
			List<Simplifier.Triangle> list2 = new List<Simplifier.Triangle>();
			for (int i = 0; i < u.m_listFaces.Count; i++)
			{
				if (u.m_listFaces[i].HasVertex(v))
				{
					list2.Add(u.m_listFaces[i]);
				}
			}
			for (int i = 0; i < u.m_listFaces.Count; i++)
			{
				if (!u.m_listFaces[i].HasVertex(v))
				{
					if (u.m_listFaces[i].HasUVData)
					{
						for (int j = 0; j < list2.Count; j++)
						{
							if (u.m_listFaces[i].TexAt(u) == list2[j].TexAt(u))
							{
								u.m_listFaces[i].SetTexAt(u, list2[j].TexAt(v));
								break;
							}
						}
					}
				}
			}
			for (int i = u.m_listFaces.Count - 1; i >= 0; i--)
			{
				if (i < u.m_listFaces.Count && i >= 0 && u.m_listFaces[i].HasVertex(v))
				{
					u.m_listFaces[i].Destructor(this);
				}
			}
			for (int i = u.m_listFaces.Count - 1; i >= 0; i--)
			{
				u.m_listFaces[i].ReplaceVertex(u, v);
			}
			u.Destructor(this);
			if (bRecompute)
			{
				for (int i = 0; i < list.Count; i++)
				{
					this.ComputeEdgeCostAtVertex(list[i], transform, aRelevanceSpheres);
					this.HeapSortUp(list[i].m_nHeapSpot);
					this.HeapSortDown(list[i].m_nHeapSpot);
				}
			}
		}

		// Token: 0x06001D54 RID: 7508 RVA: 0x000A7F48 File Offset: 0x000A6348
		private void AddVertices(List<Vector3> listVertices, List<Vector3> listVerticesWorld, List<MeshUniqueVertices.SerializableBoneWeight> listBoneWeights)
		{
			bool flag = listBoneWeights != null && listBoneWeights.Count > 0;
			for (int i = 0; i < listVertices.Count; i++)
			{
				new Simplifier.Vertex(this, listVertices[i], listVerticesWorld[i], flag, (!flag) ? default(BoneWeight) : listBoneWeights[i].ToBoneWeight(), i);
			}
		}

		// Token: 0x06001D55 RID: 7509 RVA: 0x000A7FB8 File Offset: 0x000A63B8
		private void AddFaceListSubMesh(int nSubMesh, List<int> listTriangles, int[] anIndices, Vector2[] v2Mapping)
		{
			bool bUVData = false;
			if (v2Mapping != null && v2Mapping.Length > 0)
			{
				bUVData = true;
			}
			for (int i = 0; i < listTriangles.Count / 3; i++)
			{
				Simplifier.Triangle t = new Simplifier.Triangle(this, nSubMesh, this.m_listVertices[listTriangles[i * 3]], this.m_listVertices[listTriangles[i * 3 + 1]], this.m_listVertices[listTriangles[i * 3 + 2]], bUVData, anIndices[i * 3], anIndices[i * 3 + 1], anIndices[i * 3 + 2]);
				this.ShareUV(v2Mapping, t);
			}
		}

		// Token: 0x06001D56 RID: 7510 RVA: 0x000A8058 File Offset: 0x000A6458
		private void ShareUV(Vector2[] aMapping, Simplifier.Triangle t)
		{
			if (!t.HasUVData)
			{
				return;
			}
			if (aMapping == null || aMapping.Length == 0)
			{
				return;
			}
			for (int i = 0; i < 3; i++)
			{
				int num = i;
				for (int j = 0; j < t.Vertices[num].m_listFaces.Count; j++)
				{
					Simplifier.Triangle triangle = t.Vertices[num].m_listFaces[j];
					if (t != triangle)
					{
						int num2 = t.TexAt(t.Vertices[num]);
						int num3 = triangle.TexAt(t.Vertices[num]);
						if (num2 != num3)
						{
							Vector2 lhs = aMapping[num2];
							Vector2 rhs = aMapping[num3];
							if (lhs == rhs)
							{
								t.SetTexAt(t.Vertices[num], num3);
							}
						}
					}
				}
			}
		}

		// Token: 0x06001D57 RID: 7511 RVA: 0x000A8144 File Offset: 0x000A6544
		private Simplifier.Vertex MinimumCostEdge()
		{
			return this.HeapPop();
		}

		// Token: 0x06001D58 RID: 7512 RVA: 0x000A815C File Offset: 0x000A655C
		private float HeapValue(int i)
		{
			if (i < 0 || i >= this.m_listHeap.Count)
			{
				return 1E+13f;
			}
			if (this.m_listHeap[i] == null)
			{
				return 1E+13f;
			}
			return this.m_listHeap[i].m_fObjDist;
		}

		// Token: 0x06001D59 RID: 7513 RVA: 0x000A81B0 File Offset: 0x000A65B0
		private void HeapSortUp(int k)
		{
			int num;
			while (this.HeapValue(k) < this.HeapValue(num = (k - 1) / 2))
			{
				Simplifier.Vertex value = this.m_listHeap[k];
				this.m_listHeap[k] = this.m_listHeap[num];
				this.m_listHeap[k].m_nHeapSpot = k;
				this.m_listHeap[num] = value;
				this.m_listHeap[num].m_nHeapSpot = num;
				k = num;
			}
		}

		// Token: 0x06001D5A RID: 7514 RVA: 0x000A8234 File Offset: 0x000A6634
		private void HeapSortDown(int k)
		{
			if (k == -1)
			{
				return;
			}
			int num;
			while (this.HeapValue(k) > this.HeapValue(num = (k + 1) * 2) || this.HeapValue(k) > this.HeapValue(num - 1))
			{
				num = ((this.HeapValue(num) >= this.HeapValue(num - 1)) ? (num - 1) : num);
				Simplifier.Vertex vertex = this.m_listHeap[k];
				this.m_listHeap[k] = this.m_listHeap[num];
				this.m_listHeap[k].m_nHeapSpot = k;
				this.m_listHeap[num] = vertex;
				if (vertex != null)
				{
					this.m_listHeap[num].m_nHeapSpot = num;
				}
				k = num;
			}
		}

		// Token: 0x06001D5B RID: 7515 RVA: 0x000A82FC File Offset: 0x000A66FC
		private void HeapAdd(Simplifier.Vertex v)
		{
			int count = this.m_listHeap.Count;
			this.m_listHeap.Add(v);
			v.m_nHeapSpot = count;
			this.HeapSortUp(count);
		}

		// Token: 0x06001D5C RID: 7516 RVA: 0x000A8330 File Offset: 0x000A6730
		private Simplifier.Vertex HeapPop()
		{
			Simplifier.Vertex vertex = this.m_listHeap[0];
			vertex.m_nHeapSpot = -1;
			this.m_listHeap[0] = null;
			this.HeapSortDown(0);
			return vertex;
		}

		// Token: 0x06001D5D RID: 7517 RVA: 0x000A8366 File Offset: 0x000A6766
		// Note: this type is marked as 'beforefieldinit'.
		static Simplifier()
		{
		}

		// Token: 0x040018D8 RID: 6360
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static bool <Cancelled>k__BackingField;

		// Token: 0x040018D9 RID: 6361
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool <CoroutineEnded>k__BackingField;

		// Token: 0x040018DA RID: 6362
		private static int m_nCoroutineFrameMiliseconds;

		// Token: 0x040018DB RID: 6363
		private const float MAX_VERTEX_COLLAPSE_COST = 10000000f;

		// Token: 0x040018DC RID: 6364
		private List<Simplifier.Vertex> m_listVertices;

		// Token: 0x040018DD RID: 6365
		private List<Simplifier.Vertex> m_listHeap;

		// Token: 0x040018DE RID: 6366
		private Simplifier.TriangleList[] m_aListTriangles;

		// Token: 0x040018DF RID: 6367
		[SerializeField]
		[HideInInspector]
		private int m_nOriginalMeshVertexCount = -1;

		// Token: 0x040018E0 RID: 6368
		[SerializeField]
		[HideInInspector]
		private float m_fOriginalMeshSize = 1f;

		// Token: 0x040018E1 RID: 6369
		[SerializeField]
		[HideInInspector]
		private List<int> m_listVertexMap;

		// Token: 0x040018E2 RID: 6370
		[SerializeField]
		[HideInInspector]
		private List<int> m_listVertexPermutationBack;

		// Token: 0x040018E3 RID: 6371
		[SerializeField]
		[HideInInspector]
		private MeshUniqueVertices m_meshUniqueVertices;

		// Token: 0x040018E4 RID: 6372
		[SerializeField]
		[HideInInspector]
		private Mesh m_meshOriginal;

		// Token: 0x040018E5 RID: 6373
		[SerializeField]
		[HideInInspector]
		private bool m_bUseEdgeLength = true;

		// Token: 0x040018E6 RID: 6374
		[SerializeField]
		[HideInInspector]
		private bool m_bUseCurvature = true;

		// Token: 0x040018E7 RID: 6375
		[SerializeField]
		[HideInInspector]
		private bool m_bProtectTexture = true;

		// Token: 0x040018E8 RID: 6376
		[SerializeField]
		[HideInInspector]
		private bool m_bLockBorder = true;

		// Token: 0x0200047F RID: 1151
		// (Invoke) Token: 0x06001D5F RID: 7519
		public delegate void ProgressDelegate(string strTitle, string strProgressMessage, float fT);

		// Token: 0x02000480 RID: 1152
		private class Triangle
		{
			// Token: 0x06001D62 RID: 7522 RVA: 0x000A8368 File Offset: 0x000A6768
			public Triangle(Simplifier simplifier, int nSubMesh, Simplifier.Vertex v0, Simplifier.Vertex v1, Simplifier.Vertex v2, bool bUVData, int nIndex1, int nIndex2, int nIndex3)
			{
				this.m_aVertices = new Simplifier.Vertex[3];
				this.m_aUV = new int[3];
				this.m_aIndices = new int[3];
				this.m_aVertices[0] = v0;
				this.m_aVertices[1] = v1;
				this.m_aVertices[2] = v2;
				this.m_nSubMesh = nSubMesh;
				this.m_bUVData = bUVData;
				if (this.m_bUVData)
				{
					this.m_aUV[0] = nIndex1;
					this.m_aUV[1] = nIndex2;
					this.m_aUV[2] = nIndex3;
				}
				this.m_aIndices[0] = nIndex1;
				this.m_aIndices[1] = nIndex2;
				this.m_aIndices[2] = nIndex3;
				this.ComputeNormal();
				simplifier.m_aListTriangles[nSubMesh].m_listTriangles.Add(this);
				for (int i = 0; i < 3; i++)
				{
					this.m_aVertices[i].m_listFaces.Add(this);
					for (int j = 0; j < 3; j++)
					{
						if (i != j && !this.m_aVertices[i].m_listNeighbors.Contains(this.m_aVertices[j]))
						{
							this.m_aVertices[i].m_listNeighbors.Add(this.m_aVertices[j]);
						}
					}
				}
			}

			// Token: 0x1700033A RID: 826
			// (get) Token: 0x06001D63 RID: 7523 RVA: 0x000A84A2 File Offset: 0x000A68A2
			public Simplifier.Vertex[] Vertices
			{
				get
				{
					return this.m_aVertices;
				}
			}

			// Token: 0x1700033B RID: 827
			// (get) Token: 0x06001D64 RID: 7524 RVA: 0x000A84AA File Offset: 0x000A68AA
			public bool HasUVData
			{
				get
				{
					return this.m_bUVData;
				}
			}

			// Token: 0x1700033C RID: 828
			// (get) Token: 0x06001D65 RID: 7525 RVA: 0x000A84B2 File Offset: 0x000A68B2
			public int[] IndicesUV
			{
				get
				{
					return this.m_aUV;
				}
			}

			// Token: 0x1700033D RID: 829
			// (get) Token: 0x06001D66 RID: 7526 RVA: 0x000A84BA File Offset: 0x000A68BA
			public Vector3 Normal
			{
				get
				{
					return this.m_v3Normal;
				}
			}

			// Token: 0x1700033E RID: 830
			// (get) Token: 0x06001D67 RID: 7527 RVA: 0x000A84C2 File Offset: 0x000A68C2
			public int[] Indices
			{
				get
				{
					return this.m_aIndices;
				}
			}

			// Token: 0x06001D68 RID: 7528 RVA: 0x000A84CC File Offset: 0x000A68CC
			public void Destructor(Simplifier simplifier)
			{
				simplifier.m_aListTriangles[this.m_nSubMesh].m_listTriangles.Remove(this);
				for (int i = 0; i < 3; i++)
				{
					if (this.m_aVertices[i] != null)
					{
						this.m_aVertices[i].m_listFaces.Remove(this);
					}
				}
				for (int i = 0; i < 3; i++)
				{
					int num = (i + 1) % 3;
					if (this.m_aVertices[i] != null && this.m_aVertices[num] != null)
					{
						this.m_aVertices[i].RemoveIfNonNeighbor(this.m_aVertices[num]);
						this.m_aVertices[num].RemoveIfNonNeighbor(this.m_aVertices[i]);
					}
				}
			}

			// Token: 0x06001D69 RID: 7529 RVA: 0x000A8586 File Offset: 0x000A6986
			public bool HasVertex(Simplifier.Vertex v)
			{
				return v == this.m_aVertices[0] || v == this.m_aVertices[1] || v == this.m_aVertices[2];
			}

			// Token: 0x06001D6A RID: 7530 RVA: 0x000A85B4 File Offset: 0x000A69B4
			public void ComputeNormal()
			{
				Vector3 v3Position = this.m_aVertices[0].m_v3Position;
				Vector3 v3Position2 = this.m_aVertices[1].m_v3Position;
				Vector3 v3Position3 = this.m_aVertices[2].m_v3Position;
				this.m_v3Normal = Vector3.Cross(v3Position2 - v3Position, v3Position3 - v3Position2);
				if (this.m_v3Normal.magnitude == 0f)
				{
					return;
				}
				this.m_v3Normal = this.m_v3Normal.normalized;
			}

			// Token: 0x06001D6B RID: 7531 RVA: 0x000A862C File Offset: 0x000A6A2C
			public int TexAt(Simplifier.Vertex vertex)
			{
				for (int i = 0; i < 3; i++)
				{
					if (this.m_aVertices[i] == vertex)
					{
						return this.m_aUV[i];
					}
				}
				UnityEngine.Debug.LogError("TexAt(): Vertex not found");
				return 0;
			}

			// Token: 0x06001D6C RID: 7532 RVA: 0x000A866D File Offset: 0x000A6A6D
			public int TexAt(int i)
			{
				return this.m_aUV[i];
			}

			// Token: 0x06001D6D RID: 7533 RVA: 0x000A8678 File Offset: 0x000A6A78
			public void SetTexAt(Simplifier.Vertex vertex, int uv)
			{
				for (int i = 0; i < 3; i++)
				{
					if (this.m_aVertices[i] == vertex)
					{
						this.m_aUV[i] = uv;
						return;
					}
				}
				UnityEngine.Debug.LogError("SetTexAt(): Vertex not found");
			}

			// Token: 0x06001D6E RID: 7534 RVA: 0x000A86B9 File Offset: 0x000A6AB9
			public void SetTexAt(int i, int uv)
			{
				this.m_aUV[i] = uv;
			}

			// Token: 0x06001D6F RID: 7535 RVA: 0x000A86C4 File Offset: 0x000A6AC4
			public void ReplaceVertex(Simplifier.Vertex vold, Simplifier.Vertex vnew)
			{
				if (vold == this.m_aVertices[0])
				{
					this.m_aVertices[0] = vnew;
				}
				else if (vold == this.m_aVertices[1])
				{
					this.m_aVertices[1] = vnew;
				}
				else
				{
					this.m_aVertices[2] = vnew;
				}
				vold.m_listFaces.Remove(this);
				vnew.m_listFaces.Add(this);
				for (int i = 0; i < 3; i++)
				{
					vold.RemoveIfNonNeighbor(this.m_aVertices[i]);
					this.m_aVertices[i].RemoveIfNonNeighbor(vold);
				}
				for (int i = 0; i < 3; i++)
				{
					for (int j = 0; j < 3; j++)
					{
						if (i != j && !this.m_aVertices[i].m_listNeighbors.Contains(this.m_aVertices[j]))
						{
							this.m_aVertices[i].m_listNeighbors.Add(this.m_aVertices[j]);
						}
					}
				}
				this.ComputeNormal();
			}

			// Token: 0x040018E9 RID: 6377
			private Simplifier.Vertex[] m_aVertices;

			// Token: 0x040018EA RID: 6378
			private bool m_bUVData;

			// Token: 0x040018EB RID: 6379
			private int[] m_aUV;

			// Token: 0x040018EC RID: 6380
			private int[] m_aIndices;

			// Token: 0x040018ED RID: 6381
			private Vector3 m_v3Normal;

			// Token: 0x040018EE RID: 6382
			private int m_nSubMesh;
		}

		// Token: 0x02000481 RID: 1153
		private class TriangleList
		{
			// Token: 0x06001D70 RID: 7536 RVA: 0x000A87C3 File Offset: 0x000A6BC3
			public TriangleList()
			{
				this.m_listTriangles = new List<Simplifier.Triangle>();
			}

			// Token: 0x040018EF RID: 6383
			public List<Simplifier.Triangle> m_listTriangles;
		}

		// Token: 0x02000482 RID: 1154
		private class Vertex
		{
			// Token: 0x06001D71 RID: 7537 RVA: 0x000A87D8 File Offset: 0x000A6BD8
			public Vertex(Simplifier simplifier, Vector3 v, Vector3 v3World, bool bHasBoneWeight, BoneWeight boneWeight, int nID)
			{
				this.m_v3Position = v;
				this.m_v3PositionWorld = v3World;
				this.m_bHasBoneWeight = bHasBoneWeight;
				this.m_boneWeight = boneWeight;
				this.m_nID = nID;
				this.m_listNeighbors = new List<Simplifier.Vertex>();
				this.m_listFaces = new List<Simplifier.Triangle>();
				simplifier.m_listVertices.Add(this);
			}

			// Token: 0x06001D72 RID: 7538 RVA: 0x000A8834 File Offset: 0x000A6C34
			public void Destructor(Simplifier simplifier)
			{
				while (this.m_listNeighbors.Count > 0)
				{
					this.m_listNeighbors[0].m_listNeighbors.Remove(this);
					if (this.m_listNeighbors.Count > 0)
					{
						this.m_listNeighbors.RemoveAt(0);
					}
				}
				simplifier.m_listVertices.Remove(this);
			}

			// Token: 0x06001D73 RID: 7539 RVA: 0x000A889C File Offset: 0x000A6C9C
			public void RemoveIfNonNeighbor(Simplifier.Vertex n)
			{
				if (!this.m_listNeighbors.Contains(n))
				{
					return;
				}
				for (int i = 0; i < this.m_listFaces.Count; i++)
				{
					if (this.m_listFaces[i].HasVertex(n))
					{
						return;
					}
				}
				this.m_listNeighbors.Remove(n);
			}

			// Token: 0x06001D74 RID: 7540 RVA: 0x000A88FC File Offset: 0x000A6CFC
			public bool IsBorder()
			{
				for (int i = 0; i < this.m_listNeighbors.Count; i++)
				{
					int num = 0;
					for (int j = 0; j < this.m_listFaces.Count; j++)
					{
						if (this.m_listFaces[j].HasVertex(this.m_listNeighbors[i]))
						{
							num++;
						}
					}
					if (num == 1)
					{
						return true;
					}
				}
				return false;
			}

			// Token: 0x040018F0 RID: 6384
			public Vector3 m_v3Position;

			// Token: 0x040018F1 RID: 6385
			public Vector3 m_v3PositionWorld;

			// Token: 0x040018F2 RID: 6386
			public bool m_bHasBoneWeight;

			// Token: 0x040018F3 RID: 6387
			public BoneWeight m_boneWeight;

			// Token: 0x040018F4 RID: 6388
			public int m_nID;

			// Token: 0x040018F5 RID: 6389
			public List<Simplifier.Vertex> m_listNeighbors;

			// Token: 0x040018F6 RID: 6390
			public List<Simplifier.Triangle> m_listFaces;

			// Token: 0x040018F7 RID: 6391
			public float m_fObjDist;

			// Token: 0x040018F8 RID: 6392
			public Simplifier.Vertex m_collapse;

			// Token: 0x040018F9 RID: 6393
			public int m_nHeapSpot;
		}

		// Token: 0x02000483 RID: 1155
		private class VertexDataHashComparer : IEqualityComparer<Simplifier.VertexDataHash>
		{
			// Token: 0x06001D75 RID: 7541 RVA: 0x000A8973 File Offset: 0x000A6D73
			public VertexDataHashComparer()
			{
			}

			// Token: 0x06001D76 RID: 7542 RVA: 0x000A897C File Offset: 0x000A6D7C
			public bool Equals(Simplifier.VertexDataHash a, Simplifier.VertexDataHash b)
			{
				return a.UV1 == b.UV1 && a.UV2 == b.UV2 && a.Vertex == b.Vertex && a.Color.r == b.Color.r && a.Color.g == b.Color.g && a.Color.b == b.Color.b && a.Color.a == b.Color.a;
			}

			// Token: 0x06001D77 RID: 7543 RVA: 0x000A8A53 File Offset: 0x000A6E53
			public int GetHashCode(Simplifier.VertexDataHash vdata)
			{
				return vdata.GetHashCode();
			}
		}

		// Token: 0x02000484 RID: 1156
		private class VertexDataHash
		{
			// Token: 0x06001D78 RID: 7544 RVA: 0x000A8A5B File Offset: 0x000A6E5B
			public VertexDataHash(Vector3 v3Vertex, Vector3 v3Normal, Vector2 v2Mapping1, Vector2 v2Mapping2, Color32 color)
			{
				this._v3Vertex = v3Vertex;
				this._v3Normal = v3Normal;
				this._v2Mapping1 = v2Mapping1;
				this._v2Mapping2 = v2Mapping2;
				this._color = color;
				this._uniqueVertex = new MeshUniqueVertices.UniqueVertex(v3Vertex);
			}

			// Token: 0x1700033F RID: 831
			// (get) Token: 0x06001D79 RID: 7545 RVA: 0x000A8A94 File Offset: 0x000A6E94
			public Vector3 Vertex
			{
				get
				{
					return this._v3Vertex;
				}
			}

			// Token: 0x17000340 RID: 832
			// (get) Token: 0x06001D7A RID: 7546 RVA: 0x000A8A9C File Offset: 0x000A6E9C
			public Vector3 Normal
			{
				get
				{
					return this._v3Normal;
				}
			}

			// Token: 0x17000341 RID: 833
			// (get) Token: 0x06001D7B RID: 7547 RVA: 0x000A8AA4 File Offset: 0x000A6EA4
			public Vector2 UV1
			{
				get
				{
					return this._v2Mapping1;
				}
			}

			// Token: 0x17000342 RID: 834
			// (get) Token: 0x06001D7C RID: 7548 RVA: 0x000A8AAC File Offset: 0x000A6EAC
			public Vector2 UV2
			{
				get
				{
					return this._v2Mapping2;
				}
			}

			// Token: 0x17000343 RID: 835
			// (get) Token: 0x06001D7D RID: 7549 RVA: 0x000A8AB4 File Offset: 0x000A6EB4
			public Color32 Color
			{
				get
				{
					return this._color;
				}
			}

			// Token: 0x06001D7E RID: 7550 RVA: 0x000A8ABC File Offset: 0x000A6EBC
			public override bool Equals(object obj)
			{
				Simplifier.VertexDataHash vertexDataHash = obj as Simplifier.VertexDataHash;
				return vertexDataHash._v2Mapping1 == this._v2Mapping1 && vertexDataHash._v2Mapping2 == this._v2Mapping2 && vertexDataHash._v3Vertex == this._v3Vertex && vertexDataHash._color.r == this._color.r && vertexDataHash._color.g == this._color.g && vertexDataHash._color.b == this._color.b && vertexDataHash._color.a == this._color.a;
			}

			// Token: 0x06001D7F RID: 7551 RVA: 0x000A8B7E File Offset: 0x000A6F7E
			public override int GetHashCode()
			{
				return this._uniqueVertex.GetHashCode();
			}

			// Token: 0x06001D80 RID: 7552 RVA: 0x000A8B8B File Offset: 0x000A6F8B
			public static bool operator ==(Simplifier.VertexDataHash a, Simplifier.VertexDataHash b)
			{
				return a.Equals(b);
			}

			// Token: 0x06001D81 RID: 7553 RVA: 0x000A8B94 File Offset: 0x000A6F94
			public static bool operator !=(Simplifier.VertexDataHash a, Simplifier.VertexDataHash b)
			{
				return !a.Equals(b);
			}

			// Token: 0x040018FA RID: 6394
			private Vector3 _v3Vertex;

			// Token: 0x040018FB RID: 6395
			private Vector3 _v3Normal;

			// Token: 0x040018FC RID: 6396
			private Vector2 _v2Mapping1;

			// Token: 0x040018FD RID: 6397
			private Vector2 _v2Mapping2;

			// Token: 0x040018FE RID: 6398
			private Color32 _color;

			// Token: 0x040018FF RID: 6399
			private MeshUniqueVertices.UniqueVertex _uniqueVertex;
		}

		// Token: 0x02000F6A RID: 3946
		[CompilerGenerated]
		private sealed class <ProgressiveMesh>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x060073BC RID: 29628 RVA: 0x000A8BA0 File Offset: 0x000A6FA0
			[DebuggerHidden]
			public <ProgressiveMesh>c__Iterator0()
			{
			}

			// Token: 0x060073BD RID: 29629 RVA: 0x000A8BA8 File Offset: 0x000A6FA8
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
				{
					this.m_meshOriginal = sourceMesh;
					aVerticesWorld = Simplifier.GetWorldVertices(gameObject);
					if (aVerticesWorld == null)
					{
						base.CoroutineEnded = true;
						return false;
					}
					this.m_listVertexMap = new List<int>();
					this.m_listVertexPermutationBack = new List<int>();
					this.m_listVertices = new List<Simplifier.Vertex>();
					this.m_aListTriangles = new Simplifier.TriangleList[this.m_meshOriginal.subMeshCount];
					if (progress != null)
					{
						progress("Preprocessing mesh: " + strProgressDisplayObjectName, "Building unique vertex data", 1f);
						if (Simplifier.Cancelled)
						{
							base.CoroutineEnded = true;
							return false;
						}
					}
					this.m_meshUniqueVertices = new MeshUniqueVertices();
					this.m_meshUniqueVertices.BuildData(this.m_meshOriginal, aVerticesWorld);
					this.m_nOriginalMeshVertexCount = this.m_meshUniqueVertices.ListVertices.Count;
					this.m_fOriginalMeshSize = Mathf.Max(new float[]
					{
						this.m_meshOriginal.bounds.size.x,
						this.m_meshOriginal.bounds.size.y,
						this.m_meshOriginal.bounds.size.z
					});
					this.m_listHeap = new List<Simplifier.Vertex>(this.m_meshUniqueVertices.ListVertices.Count);
					for (int i = 0; i < this.m_meshUniqueVertices.ListVertices.Count; i++)
					{
						this.m_listVertexMap.Add(-1);
						this.m_listVertexPermutationBack.Add(-1);
					}
					av2Mapping = this.m_meshOriginal.uv;
					base.AddVertices(this.m_meshUniqueVertices.ListVertices, this.m_meshUniqueVertices.ListVerticesWorld, this.m_meshUniqueVertices.ListBoneWeights);
					for (int j = 0; j < this.m_meshOriginal.subMeshCount; j++)
					{
						int[] triangles = this.m_meshOriginal.GetTriangles(j);
						this.m_aListTriangles[j] = new Simplifier.TriangleList();
						base.AddFaceListSubMesh(j, this.m_meshUniqueVertices.SubmeshesFaceList[j].m_listIndices, triangles, av2Mapping);
					}
					if (!Application.isEditor || Application.isPlaying)
					{
						this.$current = base.StartCoroutine(base.ComputeAllEdgeCollapseCosts(strProgressDisplayObjectName, gameObject.transform, aRelevanceSpheres, progress));
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					IEnumerator enumerator = base.ComputeAllEdgeCollapseCosts(strProgressDisplayObjectName, gameObject.transform, aRelevanceSpheres, progress);
					while (enumerator.MoveNext())
					{
						if (Simplifier.Cancelled)
						{
							base.CoroutineEnded = true;
							return false;
						}
					}
					break;
				}
				case 1U:
					break;
				case 2U:
					sw = Stopwatch.StartNew();
					goto IL_4B5;
				default:
					return false;
				}
				nVertices = this.m_listVertices.Count;
				sw = Stopwatch.StartNew();
				goto IL_562;
				IL_4B5:
				mn = base.MinimumCostEdge();
				this.m_listVertexPermutationBack[this.m_listVertices.Count - 1] = mn.m_nID;
				this.m_listVertexMap[mn.m_nID] = ((mn.m_collapse == null) ? -1 : mn.m_collapse.m_nID);
				base.Collapse(mn, mn.m_collapse, true, gameObject.transform, aRelevanceSpheres);
				IL_562:
				if (this.m_listVertices.Count <= 0)
				{
					this.m_listHeap.Clear();
					base.CoroutineEnded = true;
					this.$PC = -1;
				}
				else
				{
					if (progress != null && (this.m_listVertices.Count & 255) == 0)
					{
						progress("Preprocessing mesh: " + strProgressDisplayObjectName, "Collapsing edges", 1f - (float)this.m_listVertices.Count / (float)nVertices);
						if (Simplifier.Cancelled)
						{
							base.CoroutineEnded = true;
							return false;
						}
					}
					if (sw.ElapsedMilliseconds > (long)Simplifier.CoroutineFrameMiliseconds && Simplifier.CoroutineFrameMiliseconds > 0)
					{
						this.$current = null;
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						return true;
					}
					goto IL_4B5;
				}
				return false;
			}

			// Token: 0x170010F9 RID: 4345
			// (get) Token: 0x060073BE RID: 29630 RVA: 0x000A9153 File Offset: 0x000A7553
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010FA RID: 4346
			// (get) Token: 0x060073BF RID: 29631 RVA: 0x000A915B File Offset: 0x000A755B
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x060073C0 RID: 29632 RVA: 0x000A9163 File Offset: 0x000A7563
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x060073C1 RID: 29633 RVA: 0x000A9173 File Offset: 0x000A7573
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x040067A9 RID: 26537
			internal Mesh sourceMesh;

			// Token: 0x040067AA RID: 26538
			internal GameObject gameObject;

			// Token: 0x040067AB RID: 26539
			internal Vector3[] <aVerticesWorld>__0;

			// Token: 0x040067AC RID: 26540
			internal Simplifier.ProgressDelegate progress;

			// Token: 0x040067AD RID: 26541
			internal string strProgressDisplayObjectName;

			// Token: 0x040067AE RID: 26542
			internal Vector2[] <av2Mapping>__0;

			// Token: 0x040067AF RID: 26543
			internal RelevanceSphere[] aRelevanceSpheres;

			// Token: 0x040067B0 RID: 26544
			internal int <nVertices>__0;

			// Token: 0x040067B1 RID: 26545
			internal Stopwatch <sw>__0;

			// Token: 0x040067B2 RID: 26546
			internal Simplifier.Vertex <mn>__1;

			// Token: 0x040067B3 RID: 26547
			internal Simplifier $this;

			// Token: 0x040067B4 RID: 26548
			internal object $current;

			// Token: 0x040067B5 RID: 26549
			internal bool $disposing;

			// Token: 0x040067B6 RID: 26550
			internal int $PC;
		}

		// Token: 0x02000F6B RID: 3947
		[CompilerGenerated]
		private sealed class <ComputeMeshWithVertexCount>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x060073C2 RID: 29634 RVA: 0x000A917A File Offset: 0x000A757A
			[DebuggerHidden]
			public <ComputeMeshWithVertexCount>c__Iterator1()
			{
			}

			// Token: 0x060073C3 RID: 29635 RVA: 0x000A9184 File Offset: 0x000A7584
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					if (base.GetOriginalMeshUniqueVertexCount() == -1)
					{
						base.CoroutineEnded = true;
						return false;
					}
					if (nVertices < 3)
					{
						base.CoroutineEnded = true;
						return false;
					}
					if (nVertices >= base.GetOriginalMeshUniqueVertexCount())
					{
						meshOut.triangles = new int[0];
						meshOut.subMeshCount = this.m_meshOriginal.subMeshCount;
						meshOut.vertices = this.m_meshOriginal.vertices;
						meshOut.normals = this.m_meshOriginal.normals;
						meshOut.tangents = this.m_meshOriginal.tangents;
						meshOut.uv = this.m_meshOriginal.uv;
						meshOut.uv2 = this.m_meshOriginal.uv2;
						meshOut.colors32 = this.m_meshOriginal.colors32;
						meshOut.boneWeights = this.m_meshOriginal.boneWeights;
						meshOut.bindposes = this.m_meshOriginal.bindposes;
						meshOut.triangles = this.m_meshOriginal.triangles;
						meshOut.subMeshCount = this.m_meshOriginal.subMeshCount;
						for (int i = 0; i < this.m_meshOriginal.subMeshCount; i++)
						{
							meshOut.SetTriangles(this.m_meshOriginal.GetTriangles(i), i);
						}
						meshOut.name = gameObject.name + " simplified mesh";
						base.CoroutineEnded = true;
						return false;
					}
					this.m_listVertices = new List<Simplifier.Vertex>();
					this.m_aListTriangles = new Simplifier.TriangleList[this.m_meshOriginal.subMeshCount];
					listVertices = new List<Simplifier.Vertex>();
					base.AddVertices(this.m_meshUniqueVertices.ListVertices, this.m_meshUniqueVertices.ListVerticesWorld, this.m_meshUniqueVertices.ListBoneWeights);
					for (int j = 0; j < this.m_listVertices.Count; j++)
					{
						this.m_listVertices[j].m_collapse = ((this.m_listVertexMap[j] != -1) ? this.m_listVertices[this.m_listVertexMap[j]] : null);
						listVertices.Add(this.m_listVertices[this.m_listVertexPermutationBack[j]]);
					}
					av2Mapping = this.m_meshOriginal.uv;
					for (int k = 0; k < this.m_meshOriginal.subMeshCount; k++)
					{
						int[] triangles = this.m_meshOriginal.GetTriangles(k);
						this.m_aListTriangles[k] = new Simplifier.TriangleList();
						base.AddFaceListSubMesh(k, this.m_meshUniqueVertices.SubmeshesFaceList[k].m_listIndices, triangles, av2Mapping);
					}
					nTotalVertices = listVertices.Count;
					sw = Stopwatch.StartNew();
					break;
				case 1U:
					sw = Stopwatch.StartNew();
					break;
				case 2U:
					IL_6A0:
					base.CoroutineEnded = true;
					this.$PC = -1;
					return false;
				default:
					return false;
				}
				while (listVertices.Count > nVertices)
				{
					if (progress != null && nTotalVertices != nVertices && (listVertices.Count & 255) == 0)
					{
						float fT = 1f - (float)(listVertices.Count - nVertices) / (float)(nTotalVertices - nVertices);
						progress("Simplifying mesh: " + strProgressDisplayObjectName, "Collapsing edges", fT);
						if (Simplifier.Cancelled)
						{
							base.CoroutineEnded = true;
							return false;
						}
					}
					mn = listVertices[listVertices.Count - 1];
					listVertices.RemoveAt(listVertices.Count - 1);
					base.Collapse(mn, mn.m_collapse, false, null, null);
					if (sw.ElapsedMilliseconds > (long)Simplifier.CoroutineFrameMiliseconds && Simplifier.CoroutineFrameMiliseconds > 0)
					{
						this.$current = null;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
				}
				av3Vertices = new Vector3[this.m_listVertices.Count];
				for (int l = 0; l < this.m_listVertices.Count; l++)
				{
					this.m_listVertices[l].m_nID = l;
					av3Vertices[l] = this.m_listVertices[l].m_v3Position;
				}
				if (Application.isEditor && !Application.isPlaying)
				{
					IEnumerator enumerator = base.ConsolidateMesh(gameObject, this.m_meshOriginal, meshOut, this.m_aListTriangles, av3Vertices, strProgressDisplayObjectName, progress);
					while (enumerator.MoveNext())
					{
						if (Simplifier.Cancelled)
						{
							base.CoroutineEnded = true;
							return false;
						}
					}
					goto IL_6A0;
				}
				this.$current = base.StartCoroutine(base.ConsolidateMesh(gameObject, this.m_meshOriginal, meshOut, this.m_aListTriangles, av3Vertices, strProgressDisplayObjectName, progress));
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
			}

			// Token: 0x170010FB RID: 4347
			// (get) Token: 0x060073C4 RID: 29636 RVA: 0x000A9847 File Offset: 0x000A7C47
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010FC RID: 4348
			// (get) Token: 0x060073C5 RID: 29637 RVA: 0x000A984F File Offset: 0x000A7C4F
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x060073C6 RID: 29638 RVA: 0x000A9857 File Offset: 0x000A7C57
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x060073C7 RID: 29639 RVA: 0x000A9867 File Offset: 0x000A7C67
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x040067B7 RID: 26551
			internal int nVertices;

			// Token: 0x040067B8 RID: 26552
			internal Mesh meshOut;

			// Token: 0x040067B9 RID: 26553
			internal GameObject gameObject;

			// Token: 0x040067BA RID: 26554
			internal List<Simplifier.Vertex> <listVertices>__0;

			// Token: 0x040067BB RID: 26555
			internal Vector2[] <av2Mapping>__0;

			// Token: 0x040067BC RID: 26556
			internal int <nTotalVertices>__0;

			// Token: 0x040067BD RID: 26557
			internal Stopwatch <sw>__0;

			// Token: 0x040067BE RID: 26558
			internal Simplifier.ProgressDelegate progress;

			// Token: 0x040067BF RID: 26559
			internal string strProgressDisplayObjectName;

			// Token: 0x040067C0 RID: 26560
			internal Simplifier.Vertex <mn>__1;

			// Token: 0x040067C1 RID: 26561
			internal Vector3[] <av3Vertices>__0;

			// Token: 0x040067C2 RID: 26562
			internal Simplifier $this;

			// Token: 0x040067C3 RID: 26563
			internal object $current;

			// Token: 0x040067C4 RID: 26564
			internal bool $disposing;

			// Token: 0x040067C5 RID: 26565
			internal int $PC;
		}

		// Token: 0x02000F6C RID: 3948
		[CompilerGenerated]
		private sealed class <ConsolidateMesh>c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x060073C8 RID: 29640 RVA: 0x000A986E File Offset: 0x000A7C6E
			[DebuggerHidden]
			public <ConsolidateMesh>c__Iterator2()
			{
			}

			// Token: 0x060073C9 RID: 29641 RVA: 0x000A9878 File Offset: 0x000A7C78
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					av3NormalsIn = meshIn.normals;
					av4TangentsIn = meshIn.tangents;
					av2Mapping1In = meshIn.uv;
					av2Mapping2In = meshIn.uv2;
					acolColorsIn = meshIn.colors;
					aColors32In = meshIn.colors32;
					listlistIndicesOut = new List<List<int>>();
					listVerticesOut = new List<Vector3>();
					listNormalsOut = new List<Vector3>();
					listTangentsOut = new List<Vector4>();
					listMapping1Out = new List<Vector2>();
					listMapping2Out = new List<Vector2>();
					listColors32Out = new List<Color32>();
					listBoneWeightsOut = new List<BoneWeight>();
					dicVertexDataHash2Index = new Dictionary<Simplifier.VertexDataHash, int>(new Simplifier.VertexDataHashComparer());
					bUV = (av2Mapping1In != null && av2Mapping1In.Length > 0);
					bUV2 = (av2Mapping2In != null && av2Mapping2In.Length > 0);
					bNormal = (av3NormalsIn != null && av3NormalsIn.Length > 0);
					bTangent = (av4TangentsIn != null && av4TangentsIn.Length > 0);
					sw = Stopwatch.StartNew();
					nSubMesh = 0;
					goto IL_5F6;
				case 1U:
					sw = Stopwatch.StartNew();
					break;
				default:
					return false;
				}
				IL_2AC:
				for (int j = 0; j < 3; j++)
				{
					int num2 = aListTriangles[nSubMesh].m_listTriangles[i].IndicesUV[j];
					int num3 = aListTriangles[nSubMesh].m_listTriangles[i].Indices[j];
					bool flag = false;
					Vector3 v3Position = aListTriangles[nSubMesh].m_listTriangles[i].Vertices[j].m_v3Position;
					Vector3 vector = (!bNormal) ? Vector3.zero : av3NormalsIn[num3];
					Vector4 item = (!bTangent) ? Vector4.zero : av4TangentsIn[num3];
					Vector2 vector2 = (!bUV) ? Vector2.zero : av2Mapping1In[num2];
					Vector2 vector3 = (!bUV2) ? Vector2.zero : av2Mapping2In[num3];
					Color32 color = new Color32(0, 0, 0, 0);
					if (acolColorsIn != null && acolColorsIn.Length > 0)
					{
						color = acolColorsIn[num3];
						flag = true;
					}
					else if (aColors32In != null && aColors32In.Length > 0)
					{
						color = aColors32In[num3];
						flag = true;
					}
					Simplifier.VertexDataHash vertexDataHash = new Simplifier.VertexDataHash(v3Position, vector, vector2, vector3, color);
					if (dicVertexDataHash2Index.ContainsKey(vertexDataHash))
					{
						listIndicesOut.Add(dicVertexDataHash2Index[vertexDataHash]);
					}
					else
					{
						dicVertexDataHash2Index.Add(vertexDataHash, listVerticesOut.Count);
						listVerticesOut.Add(vertexDataHash.Vertex);
						if (bNormal)
						{
							listNormalsOut.Add(vector);
						}
						if (bUV)
						{
							listMapping1Out.Add(vector2);
						}
						if (bUV2)
						{
							listMapping2Out.Add(vector3);
						}
						if (bTangent)
						{
							listTangentsOut.Add(item);
						}
						if (flag)
						{
							listColors32Out.Add(color);
						}
						if (aListTriangles[nSubMesh].m_listTriangles[i].Vertices[j].m_bHasBoneWeight)
						{
							listBoneWeightsOut.Add(aListTriangles[nSubMesh].m_listTriangles[i].Vertices[j].m_boneWeight);
						}
						listIndicesOut.Add(listVerticesOut.Count - 1);
					}
				}
				i++;
				IL_5B5:
				if (i >= aListTriangles[nSubMesh].m_listTriangles.Count)
				{
					listlistIndicesOut.Add(listIndicesOut);
					nSubMesh++;
				}
				else
				{
					if (progress != null && (i & 255) == 0)
					{
						float fT = (aListTriangles[nSubMesh].m_listTriangles.Count != 1) ? ((float)i / (float)(aListTriangles[nSubMesh].m_listTriangles.Count - 1)) : 1f;
						progress("Simplifying mesh: " + strProgressDisplayObjectName, strMesh, fT);
						if (Simplifier.Cancelled)
						{
							return false;
						}
					}
					if (sw.ElapsedMilliseconds > (long)Simplifier.CoroutineFrameMiliseconds && Simplifier.CoroutineFrameMiliseconds > 0)
					{
						this.$current = null;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_2AC;
				}
				IL_5F6:
				if (nSubMesh < aListTriangles.Length)
				{
					listIndicesOut = new List<int>();
					strMesh = ((aListTriangles.Length <= 1) ? "Consolidating mesh" : ("Consolidating submesh " + (nSubMesh + 1)));
					i = 0;
					goto IL_5B5;
				}
				meshOut.triangles = new int[0];
				meshOut.vertices = listVerticesOut.ToArray();
				meshOut.normals = ((listNormalsOut.Count <= 0) ? null : listNormalsOut.ToArray());
				meshOut.tangents = ((listTangentsOut.Count <= 0) ? null : listTangentsOut.ToArray());
				meshOut.uv = ((listMapping1Out.Count <= 0) ? null : listMapping1Out.ToArray());
				meshOut.uv2 = ((listMapping2Out.Count <= 0) ? null : listMapping2Out.ToArray());
				meshOut.colors32 = ((listColors32Out.Count <= 0) ? null : listColors32Out.ToArray());
				meshOut.boneWeights = ((listBoneWeightsOut.Count <= 0) ? null : listBoneWeightsOut.ToArray());
				meshOut.bindposes = meshIn.bindposes;
				meshOut.subMeshCount = listlistIndicesOut.Count;
				for (int k = 0; k < listlistIndicesOut.Count; k++)
				{
					meshOut.SetTriangles(listlistIndicesOut[k].ToArray(), k);
				}
				meshOut.name = gameObject.name + " simplified mesh";
				progress("Simplifying mesh: " + strProgressDisplayObjectName, "Mesh consolidation done", 1f);
				this.$PC = -1;
				return false;
			}

			// Token: 0x170010FD RID: 4349
			// (get) Token: 0x060073CA RID: 29642 RVA: 0x000AA07D File Offset: 0x000A847D
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010FE RID: 4350
			// (get) Token: 0x060073CB RID: 29643 RVA: 0x000AA085 File Offset: 0x000A8485
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x060073CC RID: 29644 RVA: 0x000AA08D File Offset: 0x000A848D
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x060073CD RID: 29645 RVA: 0x000AA09D File Offset: 0x000A849D
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x040067C6 RID: 26566
			internal Mesh meshIn;

			// Token: 0x040067C7 RID: 26567
			internal Vector3[] <av3NormalsIn>__0;

			// Token: 0x040067C8 RID: 26568
			internal Vector4[] <av4TangentsIn>__0;

			// Token: 0x040067C9 RID: 26569
			internal Vector2[] <av2Mapping1In>__0;

			// Token: 0x040067CA RID: 26570
			internal Vector2[] <av2Mapping2In>__0;

			// Token: 0x040067CB RID: 26571
			internal Color[] <acolColorsIn>__0;

			// Token: 0x040067CC RID: 26572
			internal Color32[] <aColors32In>__0;

			// Token: 0x040067CD RID: 26573
			internal List<List<int>> <listlistIndicesOut>__0;

			// Token: 0x040067CE RID: 26574
			internal List<Vector3> <listVerticesOut>__0;

			// Token: 0x040067CF RID: 26575
			internal List<Vector3> <listNormalsOut>__0;

			// Token: 0x040067D0 RID: 26576
			internal List<Vector4> <listTangentsOut>__0;

			// Token: 0x040067D1 RID: 26577
			internal List<Vector2> <listMapping1Out>__0;

			// Token: 0x040067D2 RID: 26578
			internal List<Vector2> <listMapping2Out>__0;

			// Token: 0x040067D3 RID: 26579
			internal List<Color32> <listColors32Out>__0;

			// Token: 0x040067D4 RID: 26580
			internal List<BoneWeight> <listBoneWeightsOut>__0;

			// Token: 0x040067D5 RID: 26581
			internal Dictionary<Simplifier.VertexDataHash, int> <dicVertexDataHash2Index>__0;

			// Token: 0x040067D6 RID: 26582
			internal bool <bUV1>__0;

			// Token: 0x040067D7 RID: 26583
			internal bool <bUV2>__0;

			// Token: 0x040067D8 RID: 26584
			internal bool <bNormal>__0;

			// Token: 0x040067D9 RID: 26585
			internal bool <bTangent>__0;

			// Token: 0x040067DA RID: 26586
			internal Stopwatch <sw>__0;

			// Token: 0x040067DB RID: 26587
			internal int <nSubMesh>__1;

			// Token: 0x040067DC RID: 26588
			internal Simplifier.TriangleList[] aListTriangles;

			// Token: 0x040067DD RID: 26589
			internal List<int> <listIndicesOut>__2;

			// Token: 0x040067DE RID: 26590
			internal string <strMesh>__2;

			// Token: 0x040067DF RID: 26591
			internal int <i>__3;

			// Token: 0x040067E0 RID: 26592
			internal Simplifier.ProgressDelegate progress;

			// Token: 0x040067E1 RID: 26593
			internal string strProgressDisplayObjectName;

			// Token: 0x040067E2 RID: 26594
			internal Mesh meshOut;

			// Token: 0x040067E3 RID: 26595
			internal GameObject gameObject;

			// Token: 0x040067E4 RID: 26596
			internal object $current;

			// Token: 0x040067E5 RID: 26597
			internal bool $disposing;

			// Token: 0x040067E6 RID: 26598
			internal int $PC;
		}

		// Token: 0x02000F6D RID: 3949
		[CompilerGenerated]
		private sealed class <ComputeAllEdgeCollapseCosts>c__Iterator3 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x060073CE RID: 29646 RVA: 0x000AA0A4 File Offset: 0x000A84A4
			[DebuggerHidden]
			public <ComputeAllEdgeCollapseCosts>c__Iterator3()
			{
			}

			// Token: 0x060073CF RID: 29647 RVA: 0x000AA0AC File Offset: 0x000A84AC
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					sw = Stopwatch.StartNew();
					i = 0;
					goto IL_165;
				case 1U:
					sw = Stopwatch.StartNew();
					break;
				default:
					return false;
				}
				IL_109:
				base.ComputeEdgeCostAtVertex(this.m_listVertices[i], transform, aRelevanceSpheres);
				base.HeapAdd(this.m_listVertices[i]);
				i++;
				IL_165:
				if (i >= this.m_listVertices.Count)
				{
					this.$PC = -1;
				}
				else
				{
					if (progress != null && (i & 255) == 0)
					{
						progress("Preprocessing mesh: " + strProgressDisplayObjectName, "Computing edge collapse cost", (this.m_listVertices.Count != 1) ? ((float)i / ((float)this.m_listVertices.Count - 1f)) : 1f);
						if (Simplifier.Cancelled)
						{
							return false;
						}
					}
					if (sw.ElapsedMilliseconds > (long)Simplifier.CoroutineFrameMiliseconds && Simplifier.CoroutineFrameMiliseconds > 0)
					{
						this.$current = null;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_109;
				}
				return false;
			}

			// Token: 0x170010FF RID: 4351
			// (get) Token: 0x060073D0 RID: 29648 RVA: 0x000AA243 File Offset: 0x000A8643
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x17001100 RID: 4352
			// (get) Token: 0x060073D1 RID: 29649 RVA: 0x000AA24B File Offset: 0x000A864B
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x060073D2 RID: 29650 RVA: 0x000AA253 File Offset: 0x000A8653
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x060073D3 RID: 29651 RVA: 0x000AA263 File Offset: 0x000A8663
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x040067E7 RID: 26599
			internal Stopwatch <sw>__0;

			// Token: 0x040067E8 RID: 26600
			internal int <i>__1;

			// Token: 0x040067E9 RID: 26601
			internal Simplifier.ProgressDelegate progress;

			// Token: 0x040067EA RID: 26602
			internal string strProgressDisplayObjectName;

			// Token: 0x040067EB RID: 26603
			internal Transform transform;

			// Token: 0x040067EC RID: 26604
			internal RelevanceSphere[] aRelevanceSpheres;

			// Token: 0x040067ED RID: 26605
			internal Simplifier $this;

			// Token: 0x040067EE RID: 26606
			internal object $current;

			// Token: 0x040067EF RID: 26607
			internal bool $disposing;

			// Token: 0x040067F0 RID: 26608
			internal int $PC;
		}
	}
}
