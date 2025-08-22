using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000ADB RID: 2779
[ExecuteInEditMode]
public class DAZMergedMesh : DAZMesh
{
	// Token: 0x060049FA RID: 18938 RVA: 0x00182010 File Offset: 0x00180410
	public DAZMergedMesh()
	{
	}

	// Token: 0x17000A5D RID: 2653
	// (get) Token: 0x060049FB RID: 18939 RVA: 0x00182061 File Offset: 0x00180461
	public bool has2ndGraft
	{
		get
		{
			return this.hasGraft2;
		}
	}

	// Token: 0x17000A5E RID: 2654
	// (get) Token: 0x060049FC RID: 18940 RVA: 0x00182069 File Offset: 0x00180469
	// (set) Token: 0x060049FD RID: 18941 RVA: 0x00182071 File Offset: 0x00180471
	public float graftXFactor
	{
		get
		{
			return this._graftXFactor;
		}
		set
		{
			if (this._graftXFactor != value)
			{
				this._graftXFactor = value;
				this.graftFactorsDirty = true;
			}
		}
	}

	// Token: 0x17000A5F RID: 2655
	// (get) Token: 0x060049FE RID: 18942 RVA: 0x0018208D File Offset: 0x0018048D
	// (set) Token: 0x060049FF RID: 18943 RVA: 0x00182095 File Offset: 0x00180495
	public float graftYFactor
	{
		get
		{
			return this._graftYFactor;
		}
		set
		{
			if (this._graftYFactor != value)
			{
				this._graftYFactor = value;
				this.graftFactorsDirty = true;
			}
		}
	}

	// Token: 0x17000A60 RID: 2656
	// (get) Token: 0x06004A00 RID: 18944 RVA: 0x001820B1 File Offset: 0x001804B1
	// (set) Token: 0x06004A01 RID: 18945 RVA: 0x001820B9 File Offset: 0x001804B9
	public float graftZFactor
	{
		get
		{
			return this._graftZFactor;
		}
		set
		{
			if (this._graftZFactor != value)
			{
				this._graftZFactor = value;
				this.graftFactorsDirty = true;
			}
		}
	}

	// Token: 0x06004A02 RID: 18946 RVA: 0x001820D8 File Offset: 0x001804D8
	public void CopyGraftOptions()
	{
		if (this.copyGraftOptionsFromMesh != null)
		{
			this.graftMethod = this.copyGraftOptionsFromMesh.graftMethod;
			this.graftSymmetryAxis = this.copyGraftOptionsFromMesh.graftSymmetryAxis;
			this.useGraftSymmetry = this.copyGraftOptionsFromMesh.useGraftSymmetry;
			this.graftSymmetryDistance = this.copyGraftOptionsFromMesh.graftSymmetryDistance;
			this.graftToCenterlineVerts = this.copyGraftOptionsFromMesh.graftToCenterlineVerts;
			this.freeVertexDistance = this.copyGraftOptionsFromMesh.freeVertexDistance;
			this.graftXFactor = this.copyGraftOptionsFromMesh.graftXFactor;
			this.graftYFactor = this.copyGraftOptionsFromMesh.graftYFactor;
			this.graftZFactor = this.copyGraftOptionsFromMesh.graftZFactor;
			this.graftMeshMorphNamesForGrafting = new string[this.copyGraftOptionsFromMesh.graftMeshMorphNamesForGrafting.Length];
			for (int i = 0; i < this.copyGraftOptionsFromMesh.graftMeshMorphNamesForGrafting.Length; i++)
			{
				this.graftMeshMorphNamesForGrafting[i] = this.copyGraftOptionsFromMesh.graftMeshMorphNamesForGrafting[i];
			}
			this.graftMeshMorphValuesForGrafting = new float[this.copyGraftOptionsFromMesh.graftMeshMorphValuesForGrafting.Length];
			for (int j = 0; j < this.copyGraftOptionsFromMesh.graftMeshMorphValuesForGrafting.Length; j++)
			{
				this.graftMeshMorphValuesForGrafting[j] = this.copyGraftOptionsFromMesh.graftMeshMorphValuesForGrafting[j];
			}
		}
	}

	// Token: 0x06004A03 RID: 18947 RVA: 0x00182228 File Offset: 0x00180628
	public override void DeriveMeshes()
	{
		base.DeriveMeshes();
		this._threadedMorphedBaseVertices = (Vector3[])base.baseVertices.Clone();
		this._threadedMorphedUVVertices = (Vector3[])base.UVVertices.Clone();
		this._threadedVisibleMorphedUVVertices = (Vector3[])base.UVVertices.Clone();
		this._graftMovements = new Vector3[this.numGraftBaseVertices];
		this._graftMovements2 = new Vector3[this.numGraftBaseVertices];
		if (this.hasGraft2)
		{
			this._graft2Movements = new Vector3[this.numGraft2BaseVertices];
			this._graft2Movements2 = new Vector3[this.numGraft2BaseVertices];
		}
		this.UpdateVertices(true);
		if (this.graftMesh != null)
		{
			Vector3[] baseVertices = this.graftMesh.baseVertices;
			this._graftMorphedMeshVertices = new Vector3[baseVertices.Length];
			for (int i = 0; i < baseVertices.Length; i++)
			{
				this._graftMorphedMeshVertices[i] = baseVertices[i];
			}
			Mesh baseMesh = this.graftMesh.baseMesh;
			this._graftMorphedMesh = new Mesh();
			base.RegisterAllocatedObject(this._graftMorphedMesh);
			this._graftMorphedMesh.vertices = this._graftMorphedMeshVertices;
			this._graftMorphedMesh.subMeshCount = this.graftMesh.numMaterials;
			for (int j = 0; j < this.graftMesh.numMaterials; j++)
			{
				this._graftMorphedMesh.SetIndices(baseMesh.GetIndices(j), MeshTopology.Triangles, j);
			}
			this._graftMorphedMesh.RecalculateBounds();
			this._graftMorphedMesh.normals = baseMesh.normals;
		}
	}

	// Token: 0x06004A04 RID: 18948 RVA: 0x001823C8 File Offset: 0x001807C8
	public void CalculateFreeVertGraftWeightsViaClosest()
	{
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		foreach (DAZMeshGraftVertexPair dazmeshGraftVertexPair in this.graftMesh.meshGraft.vertexPairs)
		{
			dictionary.Add(dazmeshGraftVertexPair.vertexNum, dazmeshGraftVertexPair.graftToVertexNum);
		}
		MeshPoly[] basePolyList = this.targetMesh.basePolyList;
		Vector3[] baseVertices = this.targetMesh.baseVertices;
		Vector3[] baseVertices2 = this.graftMesh.baseVertices;
		Vector3[] array = new Vector3[baseVertices2.Length];
		Vector3[] array2 = new Vector3[basePolyList.Length];
		List<int> list = new List<int>();
		foreach (int num in this.graftMesh.meshGraft.hiddenPolys)
		{
			foreach (int item in basePolyList[num].vertices)
			{
				list.Add(item);
			}
		}
		for (int l = 0; l < baseVertices2.Length; l++)
		{
			array[l] = baseVertices2[l];
		}
		if (this.graftMeshMorphNamesForGrafting != null && this.graftMesh.morphBank != null)
		{
			if (this.graftMeshMorphNamesForGrafting.Length == this.graftMeshMorphValuesForGrafting.Length)
			{
				for (int m = 0; m < this.graftMeshMorphNamesForGrafting.Length; m++)
				{
					float d = this.graftMeshMorphValuesForGrafting[m];
					DAZMorph builtInMorph = this.graftMesh.morphBank.GetBuiltInMorph(this.graftMeshMorphNamesForGrafting[m]);
					if (builtInMorph != null && builtInMorph.deltas.Length > 0)
					{
						foreach (DAZMorphVertex dazmorphVertex in builtInMorph.deltas)
						{
							Vector3 b = dazmorphVertex.delta * d;
							array[dazmorphVertex.vertex] += b;
						}
					}
					else
					{
						Debug.LogError("Could not find graft morph " + this.graftMeshMorphNamesForGrafting[m]);
					}
				}
			}
			else
			{
				Debug.LogError("Graft mesh morph names and morph values are not same length");
			}
		}
		foreach (int num3 in this.graftMesh.meshGraft.hiddenPolys)
		{
			int[] vertices2 = basePolyList[num3].vertices;
			float num4 = 0f;
			float num5 = 0f;
			float num6 = 0f;
			int num7 = vertices2.Length;
			float num8 = 1f / (float)num7;
			for (int num9 = 0; num9 < num7; num9++)
			{
				int num10 = vertices2[num9];
				num4 += baseVertices[num10].x * num8;
				num5 += baseVertices[num10].y * num8;
				num6 += baseVertices[num10].z * num8;
			}
			array2[num3].x = num4;
			array2[num3].y = num5;
			array2[num3].z = num6;
		}
		this._freeVertGraftWeights = new DAZMergedMesh.FreeVertGraftWeight[this.numGraftBaseVertices];
		for (int num11 = 0; num11 < this.numGraftBaseVertices; num11++)
		{
			float num12 = 1000000f;
			float num13 = 1000000f;
			int graftVert = -1;
			int graftPoly = -1;
			foreach (int num15 in this.graftMesh.meshGraft.hiddenPolys)
			{
				Vector3 a = array2[num15];
				float magnitude = (a - array[num11]).magnitude;
				if (magnitude < num13)
				{
					graftPoly = num15;
					num13 = magnitude;
				}
			}
			float num16 = 0f;
			DAZMergedMesh.GraftSymmetryAxis graftSymmetryAxis = this.graftSymmetryAxis;
			if (graftSymmetryAxis != DAZMergedMesh.GraftSymmetryAxis.X)
			{
				if (graftSymmetryAxis != DAZMergedMesh.GraftSymmetryAxis.Y)
				{
					if (graftSymmetryAxis == DAZMergedMesh.GraftSymmetryAxis.Z)
					{
						num16 = Mathf.Abs(array[num11].z);
					}
				}
				else
				{
					num16 = Mathf.Abs(array[num11].y);
				}
			}
			else
			{
				num16 = Mathf.Abs(array[num11].x);
			}
			if (this.useGraftSymmetry)
			{
				if (num16 < this.graftSymmetryDistance)
				{
					foreach (int num17 in list)
					{
						Vector3 a2 = baseVertices[num17];
						float num18 = 0f;
						DAZMergedMesh.GraftSymmetryAxis graftSymmetryAxis2 = this.graftSymmetryAxis;
						if (graftSymmetryAxis2 != DAZMergedMesh.GraftSymmetryAxis.X)
						{
							if (graftSymmetryAxis2 != DAZMergedMesh.GraftSymmetryAxis.Y)
							{
								if (graftSymmetryAxis2 == DAZMergedMesh.GraftSymmetryAxis.Z)
								{
									num18 = Mathf.Abs(a2.z);
								}
							}
							else
							{
								num18 = Mathf.Abs(a2.y);
							}
						}
						else
						{
							num18 = Mathf.Abs(a2.x);
						}
						if (num18 < this.graftSymmetryDistance)
						{
							float magnitude2 = (a2 - array[num11]).magnitude;
							if (magnitude2 < num12)
							{
								graftVert = num17;
								num12 = magnitude2;
							}
						}
					}
				}
				else
				{
					foreach (int num19 in list)
					{
						Vector3 a3 = baseVertices[num19];
						float num20 = 0f;
						DAZMergedMesh.GraftSymmetryAxis graftSymmetryAxis3 = this.graftSymmetryAxis;
						if (graftSymmetryAxis3 != DAZMergedMesh.GraftSymmetryAxis.X)
						{
							if (graftSymmetryAxis3 != DAZMergedMesh.GraftSymmetryAxis.Y)
							{
								if (graftSymmetryAxis3 == DAZMergedMesh.GraftSymmetryAxis.Z)
								{
									num20 = Mathf.Abs(a3.z);
								}
							}
							else
							{
								num20 = Mathf.Abs(a3.y);
							}
						}
						else
						{
							num20 = Mathf.Abs(a3.x);
						}
						if (this.graftToCenterlineVerts || (!this.graftToCenterlineVerts && num20 > this.graftSymmetryDistance))
						{
							float magnitude3 = (a3 - array[num11]).magnitude;
							if (magnitude3 < num12)
							{
								graftVert = num19;
								num12 = magnitude3;
							}
						}
					}
				}
			}
			else
			{
				foreach (int num21 in list)
				{
					Vector3 a4 = baseVertices[num21];
					float magnitude4 = (a4 - array[num11]).magnitude;
					if (magnitude4 < num12)
					{
						graftVert = num21;
						num12 = magnitude4;
					}
				}
			}
			DAZMergedMesh.FreeVertGraftWeight freeVertGraftWeight = new DAZMergedMesh.FreeVertGraftWeight();
			freeVertGraftWeight.freeVert = num11;
			if (this.freeVertexDistance == 0f)
			{
				freeVertGraftWeight.weight = 0f;
			}
			else
			{
				float weight = Mathf.Clamp01(1f - num12 / this.freeVertexDistance);
				freeVertGraftWeight.weight = weight;
			}
			freeVertGraftWeight.graftVert = graftVert;
			freeVertGraftWeight.graftPoly = graftPoly;
			if (this.useGraftSymmetry)
			{
				DAZMergedMesh.GraftSymmetryAxis graftSymmetryAxis4 = this.graftSymmetryAxis;
				if (graftSymmetryAxis4 != DAZMergedMesh.GraftSymmetryAxis.X)
				{
					if (graftSymmetryAxis4 != DAZMergedMesh.GraftSymmetryAxis.Y)
					{
						if (graftSymmetryAxis4 == DAZMergedMesh.GraftSymmetryAxis.Z)
						{
							num16 = Mathf.Abs(array[num11].z);
						}
					}
					else
					{
						num16 = Mathf.Abs(array[num11].y);
					}
				}
				else
				{
					num16 = Mathf.Abs(array[num11].x);
				}
				if (num16 < this.graftSymmetryDistance)
				{
					freeVertGraftWeight.graftVertToPolyRatio = 0f;
				}
				else
				{
					freeVertGraftWeight.graftVertToPolyRatio = num12 / (num12 + num13);
				}
			}
			else
			{
				freeVertGraftWeight.graftVertToPolyRatio = num12 / (num12 + num13);
			}
			this._freeVertGraftWeights[num11] = freeVertGraftWeight;
		}
	}

	// Token: 0x06004A05 RID: 18949 RVA: 0x00182BFC File Offset: 0x00180FFC
	public void Merge()
	{
		base.meshData = null;
		this.meshSmooth = null;
		DAZMesh[] components = base.GetComponents<DAZMesh>();
		bool flag = false;
		this.hasGraft2 = false;
		foreach (DAZMesh dazmesh in components)
		{
			if (dazmesh.meshGraft != null && dazmesh.graftTo != null && dazmesh != this)
			{
				if (flag)
				{
					this.hasGraft2 = true;
					this.graft2Mesh = dazmesh;
					if (dazmesh.graftTo != this.targetMesh)
					{
						Debug.LogError(string.Concat(new string[]
						{
							"2nd graft mesh ",
							dazmesh.geometryId,
							" uses a different target mesh ",
							dazmesh.graftTo.geometryId,
							" than 1st graft mesh ",
							this.targetMesh.geometryId
						}));
						Debug.LogError("Merge aborted");
						return;
					}
					this.geometryId = this.geometryId + ":" + this.graft2Mesh.geometryId;
				}
				else
				{
					flag = true;
					this.graftMesh = dazmesh;
					this.targetMesh = dazmesh.graftTo;
					this.geometryId = this.targetMesh.geometryId + ":" + this.graftMesh.geometryId;
				}
			}
		}
		if (this.targetMesh != null && this.graftMesh != null)
		{
			Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
			foreach (int key in this.graftMesh.meshGraft.hiddenPolys)
			{
				dictionary.Add(key, true);
			}
			if (this.hasGraft2)
			{
				foreach (int key2 in this.graft2Mesh.meshGraft.hiddenPolys)
				{
					dictionary.Add(key2, true);
				}
			}
			Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
			foreach (DAZMeshGraftVertexPair dazmeshGraftVertexPair in this.graftMesh.meshGraft.vertexPairs)
			{
				dictionary2.Add(dazmeshGraftVertexPair.vertexNum, dazmeshGraftVertexPair.graftToVertexNum);
			}
			Dictionary<int, int> dictionary3 = new Dictionary<int, int>();
			if (this.hasGraft2)
			{
				foreach (DAZMeshGraftVertexPair dazmeshGraftVertexPair2 in this.graft2Mesh.meshGraft.vertexPairs)
				{
					dictionary3.Add(dazmeshGraftVertexPair2.vertexNum, dazmeshGraftVertexPair2.graftToVertexNum);
				}
			}
			Dictionary<int, int> dictionary4 = new Dictionary<int, int>();
			List<DAZVertexMap> list = new List<DAZVertexMap>();
			this.numTargetBaseVertices = this.targetMesh.numBaseVertices;
			this.numTargetUVVertices = this.targetMesh.numUVVertices;
			this.numGraftBaseVertices = this.graftMesh.numBaseVertices;
			if (this.hasGraft2)
			{
				this.numGraft2BaseVertices = this.graft2Mesh.numBaseVertices;
			}
			else
			{
				this.numGraft2BaseVertices = 0;
			}
			this.numGraftUVVertices = this.graftMesh.numUVVertices;
			this.startGraftVertIndex = this.numTargetBaseVertices;
			if (this.hasGraft2)
			{
				this.startGraft2VertIndex = this.numTargetBaseVertices + this.numGraftBaseVertices;
			}
			if (this.hasGraft2)
			{
				this._numBaseVertices = this.numTargetBaseVertices + this.numGraftBaseVertices + this.numGraft2BaseVertices;
			}
			else
			{
				this._numBaseVertices = this.numTargetBaseVertices + this.numGraftBaseVertices;
			}
			this._baseVertices = new Vector3[this._numBaseVertices];
			Vector3[] baseVertices = this.targetMesh.baseVertices;
			for (int n = 0; n < this.numTargetBaseVertices; n++)
			{
				this._baseVertices[n] = baseVertices[n];
			}
			Vector3[] baseVertices2 = this.graftMesh.baseVertices;
			DAZMeshGraftVertexPair[] vertexPairs3 = this.graftMesh.meshGraft.vertexPairs;
			int num = vertexPairs3.Length;
			this._graftWeights = new float[this.numGraftBaseVertices * num];
			this._graftIsFreeVert = new bool[this.numGraftBaseVertices];
			this.CalculateFreeVertGraftWeightsViaClosest();
			for (int num2 = 0; num2 < this.numGraftBaseVertices; num2++)
			{
				int num3;
				if (dictionary2.TryGetValue(num2, out num3))
				{
					this._graftIsFreeVert[num2] = false;
					DAZVertexMap dazvertexMap = new DAZVertexMap();
					dazvertexMap.fromvert = num3;
					dazvertexMap.tovert = this.startGraftVertIndex + num2;
					list.Add(dazvertexMap);
					dictionary4.Add(dazvertexMap.tovert, dazvertexMap.fromvert);
					this._baseVertices[this.startGraftVertIndex + num2] = baseVertices[num3];
				}
				else
				{
					this._graftIsFreeVert[num2] = true;
					this._baseVertices[this.startGraftVertIndex + num2] = baseVertices2[num2];
					float num4 = 0f;
					for (int num5 = 0; num5 < num; num5++)
					{
						int vertexNum = vertexPairs3[num5].vertexNum;
						float num6 = baseVertices2[num2].x - baseVertices2[vertexNum].x;
						float num7 = baseVertices2[num2].y - baseVertices2[vertexNum].y;
						float num8 = baseVertices2[num2].z - baseVertices2[vertexNum].z;
						float num9 = num6 * num6 + num7 * num7 + num8 * num8;
						float num10 = num9 * num9;
						num4 += num10;
						int num11 = num5 * this.numGraftBaseVertices + num2;
						this._graftWeights[num11] = num10;
					}
					float num12 = 0f;
					for (int num13 = 0; num13 < num; num13++)
					{
						int num14 = num13 * this.numGraftBaseVertices + num2;
						float num15 = num4 / this._graftWeights[num14];
						this._graftWeights[num14] = num15;
						num12 += num15;
					}
					float num16 = 1f / num12;
					for (int num17 = 0; num17 < num; num17++)
					{
						int num18 = num17 * this.numGraftBaseVertices + num2;
						this._graftWeights[num18] *= num16;
					}
				}
			}
			if (this.hasGraft2)
			{
				Vector3[] baseVertices3 = this.graft2Mesh.baseVertices;
				DAZMeshGraftVertexPair[] vertexPairs4 = this.graft2Mesh.meshGraft.vertexPairs;
				int num19 = vertexPairs4.Length;
				this._graft2Weights = new float[this.numGraft2BaseVertices * num19];
				this._graft2IsFreeVert = new bool[this.numGraft2BaseVertices];
				for (int num20 = 0; num20 < this.numGraft2BaseVertices; num20++)
				{
					int num21;
					if (dictionary3.TryGetValue(num20, out num21))
					{
						this._graft2IsFreeVert[num20] = false;
						DAZVertexMap dazvertexMap2 = new DAZVertexMap();
						dazvertexMap2.fromvert = num21;
						dazvertexMap2.tovert = this.startGraft2VertIndex + num20;
						list.Add(dazvertexMap2);
						dictionary4.Add(dazvertexMap2.tovert, dazvertexMap2.fromvert);
						this._baseVertices[this.startGraft2VertIndex + num20] = baseVertices[num21];
					}
					else
					{
						this._graft2IsFreeVert[num20] = true;
						this._baseVertices[this.startGraft2VertIndex + num20] = baseVertices3[num20];
						float num22 = 0f;
						for (int num23 = 0; num23 < num19; num23++)
						{
							int vertexNum2 = vertexPairs4[num23].vertexNum;
							float num24 = baseVertices3[num20].x - baseVertices3[vertexNum2].x;
							float num25 = baseVertices3[num20].y - baseVertices3[vertexNum2].y;
							float num26 = baseVertices3[num20].z - baseVertices3[vertexNum2].z;
							float num27 = num24 * num24 + num25 * num25 + num26 * num26;
							float num28 = num27 * num27;
							num22 += num28;
							int num29 = num23 * this.numGraft2BaseVertices + num20;
							this._graft2Weights[num29] = num28;
						}
						float num30 = 0f;
						for (int num31 = 0; num31 < num19; num31++)
						{
							int num32 = num31 * this.numGraft2BaseVertices + num20;
							float num33 = num22 / this._graft2Weights[num32];
							this._graft2Weights[num32] = num33;
							num30 += num33;
						}
						float num34 = 1f / num30;
						for (int num35 = 0; num35 < num19; num35++)
						{
							int num36 = num35 * this.numGraft2BaseVertices + num20;
							this._graft2Weights[num36] *= num34;
						}
					}
				}
			}
			int numMaterials = this.targetMesh.numMaterials;
			this._numMaterials = numMaterials + this.graftMesh.numMaterials;
			if (this.hasGraft2)
			{
				this._numMaterials += this.graft2Mesh.numMaterials;
			}
			this._numMaterials++;
			this.materials = new Material[this._numMaterials];
			this.materialsEnabled = new bool[this._numMaterials];
			this.materialsShadowCastEnabled = new bool[this._numMaterials];
			this._materialNames = new string[this._numMaterials];
			string[] materialNames = this.targetMesh.materialNames;
			string[] materialNames2 = this.graftMesh.materialNames;
			string[] array2 = null;
			if (this.hasGraft2)
			{
				array2 = this.graft2Mesh.materialNames;
			}
			for (int num37 = 0; num37 < this.targetMesh.numMaterials; num37++)
			{
				this.materials[num37] = this.targetMesh.materials[num37];
				this.materialsEnabled[num37] = true;
				this.materialsShadowCastEnabled[num37] = true;
				this._materialNames[num37] = materialNames[num37];
			}
			int numMaterials2 = this.graftMesh.numMaterials;
			for (int num38 = 0; num38 < numMaterials2; num38++)
			{
				this.materials[numMaterials + num38] = this.graftMesh.materials[num38];
				this.materialsEnabled[numMaterials + num38] = true;
				this.materialsShadowCastEnabled[numMaterials + num38] = true;
				this._materialNames[numMaterials + num38] = materialNames2[num38];
			}
			if (this.hasGraft2)
			{
				for (int num39 = 0; num39 < this.graft2Mesh.numMaterials; num39++)
				{
					this.materials[numMaterials + numMaterials2 + num39] = this.graft2Mesh.materials[num39];
					this.materialsEnabled[numMaterials + numMaterials2 + num39] = true;
					this.materialsShadowCastEnabled[numMaterials + numMaterials2 + num39] = true;
					this._materialNames[numMaterials + numMaterials2 + num39] = array2[num39];
				}
			}
			this.materialsEnabled[this._numMaterials - 1] = true;
			this.materialsShadowCastEnabled[this._numMaterials - 1] = true;
			this._materialNames[this._numMaterials - 1] = "Hidden";
			MeshPoly[] basePolyList = this.targetMesh.basePolyList;
			MeshPoly[] basePolyList2 = this.graftMesh.basePolyList;
			MeshPoly[] array3 = null;
			if (this.hasGraft2)
			{
				array3 = this.graft2Mesh.basePolyList;
			}
			int num40 = basePolyList.Length;
			int num41 = basePolyList2.Length;
			int num42 = 0;
			if (this.hasGraft2)
			{
				num42 = array3.Length;
			}
			this._numBasePolygons = num40 + num41;
			if (this.hasGraft2)
			{
				this._numBasePolygons += num42;
			}
			this._basePolyList = new MeshPoly[this._numBasePolygons];
			int num43 = 0;
			for (int num44 = 0; num44 < num40; num44++)
			{
				bool flag2;
				if (!dictionary.TryGetValue(num44, out flag2))
				{
					MeshPoly meshPoly = basePolyList[num44];
					this._basePolyList[num43] = meshPoly;
					num43++;
				}
			}
			for (int num45 = 0; num45 < num41; num45++)
			{
				MeshPoly meshPoly2 = basePolyList2[num45];
				MeshPoly meshPoly3 = new MeshPoly();
				meshPoly3.materialNum = meshPoly2.materialNum + numMaterials;
				meshPoly3.vertices = new int[meshPoly2.vertices.Length];
				for (int num46 = 0; num46 < meshPoly2.vertices.Length; num46++)
				{
					int num47;
					if (dictionary2.TryGetValue(meshPoly2.vertices[num46], out num47))
					{
						meshPoly3.vertices[num46] = num47;
					}
					else
					{
						meshPoly3.vertices[num46] = meshPoly2.vertices[num46] + this.startGraftVertIndex;
					}
				}
				this._basePolyList[num43] = meshPoly3;
				num43++;
			}
			if (this.hasGraft2)
			{
				for (int num48 = 0; num48 < num42; num48++)
				{
					MeshPoly meshPoly4 = array3[num48];
					MeshPoly meshPoly5 = new MeshPoly();
					meshPoly5.materialNum = meshPoly4.materialNum + numMaterials + numMaterials2;
					meshPoly5.vertices = new int[meshPoly4.vertices.Length];
					for (int num49 = 0; num49 < meshPoly4.vertices.Length; num49++)
					{
						int num50;
						if (dictionary3.TryGetValue(meshPoly4.vertices[num49], out num50))
						{
							meshPoly5.vertices[num49] = num50;
						}
						else
						{
							meshPoly5.vertices[num49] = meshPoly4.vertices[num49] + this.startGraft2VertIndex;
						}
					}
					this._basePolyList[num43] = meshPoly5;
					num43++;
				}
			}
			for (int num51 = 0; num51 < num40; num51++)
			{
				bool flag3;
				if (dictionary.TryGetValue(num51, out flag3))
				{
					MeshPoly meshPoly6 = basePolyList[num51];
					MeshPoly meshPoly7 = new MeshPoly();
					meshPoly7.vertices = (int[])meshPoly6.vertices.Clone();
					meshPoly7.materialNum = this._numMaterials - 1;
					this._basePolyList[num43] = meshPoly7;
					num43++;
				}
			}
			this._numUVVertices = this.numTargetUVVertices + this.numGraftUVVertices;
			if (this.hasGraft2)
			{
				this._numUVVertices += this.graft2Mesh.numUVVertices;
			}
			this._UV = new Vector2[this._numUVVertices];
			this._OrigUV = new Vector2[this._numUVVertices];
			this._UVVertices = new Vector3[this._numUVVertices];
			this._UVPolyList = new MeshPoly[this._numBasePolygons];
			this._morphedUVVertices = new Vector3[this._numUVVertices];
			Vector2[] uv = this.targetMesh.UV;
			Vector2[] uv2 = this.graftMesh.UV;
			Vector2[] array4 = null;
			if (this.hasGraft2)
			{
				array4 = this.graft2Mesh.UV;
			}
			Vector3[] uvvertices = this.targetMesh.UVVertices;
			Vector3[] uvvertices2 = this.graftMesh.UVVertices;
			Vector3[] array5 = null;
			if (this.hasGraft2)
			{
				array5 = this.graft2Mesh.UVVertices;
			}
			Vector3[] morphedUVVertices = this.targetMesh.morphedUVVertices;
			Vector3[] morphedUVVertices2 = this.graftMesh.morphedUVVertices;
			Vector3[] array6 = null;
			if (this.hasGraft2)
			{
				array6 = this.graft2Mesh.morphedUVVertices;
			}
			DAZVertexMap[] baseVerticesToUVVertices = this.targetMesh.baseVerticesToUVVertices;
			DAZVertexMap[] baseVerticesToUVVertices2 = this.graftMesh.baseVerticesToUVVertices;
			DAZVertexMap[] array7 = null;
			if (this.hasGraft2)
			{
				array7 = this.graft2Mesh.baseVerticesToUVVertices;
			}
			int num52 = baseVerticesToUVVertices.Length;
			for (int num53 = 0; num53 < num52; num53++)
			{
				DAZVertexMap dazvertexMap3 = baseVerticesToUVVertices[num53];
				DAZVertexMap dazvertexMap4 = new DAZVertexMap();
				dazvertexMap4.tovert = dazvertexMap3.tovert + this.numGraftBaseVertices + this.numGraft2BaseVertices;
				int fromvert;
				if (dictionary4.TryGetValue(dazvertexMap3.fromvert, out fromvert))
				{
					dazvertexMap4.fromvert = fromvert;
				}
				else
				{
					dazvertexMap4.fromvert = dazvertexMap3.fromvert;
				}
				dictionary4.Add(dazvertexMap4.tovert, dazvertexMap4.fromvert);
				list.Add(dazvertexMap4);
			}
			foreach (DAZVertexMap dazvertexMap5 in baseVerticesToUVVertices2)
			{
				DAZVertexMap dazvertexMap6 = new DAZVertexMap();
				dazvertexMap6.fromvert = dazvertexMap5.fromvert + this.numTargetBaseVertices;
				dazvertexMap6.tovert = dazvertexMap5.tovert + this.numTargetUVVertices + this.numGraft2BaseVertices;
				int fromvert2;
				if (dictionary4.TryGetValue(dazvertexMap6.fromvert, out fromvert2))
				{
					dazvertexMap6.fromvert = fromvert2;
				}
				dictionary4.Add(dazvertexMap6.tovert, dazvertexMap6.fromvert);
				list.Add(dazvertexMap6);
			}
			if (this.hasGraft2)
			{
				foreach (DAZVertexMap dazvertexMap7 in array7)
				{
					DAZVertexMap dazvertexMap8 = new DAZVertexMap();
					dazvertexMap8.fromvert = dazvertexMap7.fromvert + this.numTargetBaseVertices + this.numGraftBaseVertices;
					dazvertexMap8.tovert = dazvertexMap7.tovert + this.numTargetUVVertices + this.numGraftUVVertices;
					int fromvert3;
					if (dictionary4.TryGetValue(dazvertexMap8.fromvert, out fromvert3))
					{
						dazvertexMap8.fromvert = fromvert3;
					}
					dictionary4.Add(dazvertexMap8.tovert, dazvertexMap8.fromvert);
					list.Add(dazvertexMap8);
				}
			}
			for (int num56 = 0; num56 < this.numTargetUVVertices; num56++)
			{
				int num57 = num56;
				if (num56 >= this.numTargetBaseVertices)
				{
					num57 += this.numGraftBaseVertices + this.numGraft2BaseVertices;
				}
				this._OrigUV[num57] = uv[num56];
				this._UV[num57] = uv[num56];
				this._UVVertices[num57] = uvvertices[num56];
				this._morphedUVVertices[num57] = morphedUVVertices[num56];
			}
			for (int num58 = 0; num58 < this.graftMesh.numUVVertices; num58++)
			{
				int num59 = num58 + this.numTargetBaseVertices;
				if (num58 >= this.numGraftBaseVertices)
				{
					num59 = num58 + this.numTargetUVVertices + this.numGraft2BaseVertices;
				}
				this._OrigUV[num59] = uv2[num58];
				this._UV[num59] = uv2[num58];
				this._UVVertices[num59] = uvvertices2[num58];
				this._morphedUVVertices[num59] = morphedUVVertices2[num58];
			}
			if (this.hasGraft2)
			{
				for (int num60 = 0; num60 < this.graft2Mesh.numUVVertices; num60++)
				{
					int num61 = num60 + this.numTargetBaseVertices + this.numGraftBaseVertices;
					if (num60 >= this.numGraft2BaseVertices)
					{
						num61 = num60 + this.numTargetUVVertices + this.numGraftUVVertices;
					}
					this._OrigUV[num61] = array4[num60];
					this._UV[num61] = array4[num60];
					this._UVVertices[num61] = array5[num60];
					this._morphedUVVertices[num61] = array6[num60];
				}
			}
			MeshPoly[] uvpolyList = this.targetMesh.UVPolyList;
			MeshPoly[] uvpolyList2 = this.graftMesh.UVPolyList;
			MeshPoly[] array8 = null;
			if (this.hasGraft2)
			{
				array8 = this.graft2Mesh.UVPolyList;
			}
			num43 = 0;
			for (int num62 = 0; num62 < num40; num62++)
			{
				bool flag4;
				if (!dictionary.TryGetValue(num62, out flag4))
				{
					MeshPoly meshPoly8 = uvpolyList[num62];
					MeshPoly meshPoly9 = new MeshPoly();
					meshPoly9.materialNum = meshPoly8.materialNum;
					meshPoly9.vertices = new int[meshPoly8.vertices.Length];
					for (int num63 = 0; num63 < meshPoly8.vertices.Length; num63++)
					{
						if (meshPoly8.vertices[num63] >= this.numTargetBaseVertices)
						{
							meshPoly9.vertices[num63] = meshPoly8.vertices[num63] + this.numGraftBaseVertices + this.numGraft2BaseVertices;
						}
						else
						{
							meshPoly9.vertices[num63] = meshPoly8.vertices[num63];
						}
					}
					this._UVPolyList[num43] = meshPoly9;
					num43++;
				}
			}
			for (int num64 = 0; num64 < num41; num64++)
			{
				MeshPoly meshPoly10 = uvpolyList2[num64];
				MeshPoly meshPoly11 = new MeshPoly();
				meshPoly11.materialNum = meshPoly10.materialNum + numMaterials;
				meshPoly11.vertices = new int[meshPoly10.vertices.Length];
				for (int num65 = 0; num65 < meshPoly10.vertices.Length; num65++)
				{
					if (meshPoly10.vertices[num65] >= this.numGraftBaseVertices)
					{
						meshPoly11.vertices[num65] = meshPoly10.vertices[num65] + this.numTargetUVVertices + this.numGraft2BaseVertices;
					}
					else
					{
						meshPoly11.vertices[num65] = meshPoly10.vertices[num65] + this.numTargetBaseVertices;
					}
				}
				this._UVPolyList[num43] = meshPoly11;
				num43++;
			}
			if (this.hasGraft2)
			{
				for (int num66 = 0; num66 < num42; num66++)
				{
					MeshPoly meshPoly12 = array8[num66];
					MeshPoly meshPoly13 = new MeshPoly();
					meshPoly13.materialNum = meshPoly12.materialNum + numMaterials + numMaterials2;
					meshPoly13.vertices = new int[meshPoly12.vertices.Length];
					for (int num67 = 0; num67 < meshPoly12.vertices.Length; num67++)
					{
						if (meshPoly12.vertices[num67] >= this.numGraft2BaseVertices)
						{
							meshPoly13.vertices[num67] = meshPoly12.vertices[num67] + this.numTargetUVVertices + this.numGraftUVVertices;
						}
						else
						{
							meshPoly13.vertices[num67] = meshPoly12.vertices[num67] + this.numTargetBaseVertices + this.numGraftBaseVertices;
						}
					}
					this._UVPolyList[num43] = meshPoly13;
					num43++;
				}
			}
			for (int num68 = 0; num68 < num40; num68++)
			{
				bool flag5;
				if (dictionary.TryGetValue(num68, out flag5))
				{
					MeshPoly meshPoly14 = uvpolyList[num68];
					MeshPoly meshPoly15 = new MeshPoly();
					meshPoly15.materialNum = this._numMaterials - 1;
					meshPoly15.vertices = new int[meshPoly14.vertices.Length];
					for (int num69 = 0; num69 < meshPoly14.vertices.Length; num69++)
					{
						if (meshPoly14.vertices[num69] >= this.numTargetBaseVertices)
						{
							meshPoly15.vertices[num69] = meshPoly14.vertices[num69] + this.numGraftBaseVertices + this.numGraft2BaseVertices;
						}
						else
						{
							meshPoly15.vertices[num69] = meshPoly14.vertices[num69];
						}
					}
					this._UVPolyList[num43] = meshPoly15;
					num43++;
				}
			}
			this._baseVerticesToUVVertices = list.ToArray();
			this.DeriveMeshes();
		}
	}

	// Token: 0x06004A06 RID: 18950 RVA: 0x00184308 File Offset: 0x00182708
	public new void RecalculateMorphedMeshTangentsAccurate()
	{
		base.RecalculateMorphedMeshTangentsAccurate();
		DAZMeshGraftVertexPair[] vertexPairs = this.graftMesh.meshGraft.vertexPairs;
		int num = vertexPairs.Length;
		for (int i = 0; i < num; i++)
		{
			int graftToVertexNum = vertexPairs[i].graftToVertexNum;
			int vertexNum = vertexPairs[i].vertexNum;
			this._morphedUVTangents[graftToVertexNum] = this._morphedUVTangents[this.startGraftVertIndex + vertexNum];
		}
	}

	// Token: 0x06004A07 RID: 18951 RVA: 0x00184380 File Offset: 0x00182780
	public new void RecalculateMorphedMeshTangents(bool forceAll = false)
	{
		base.RecalculateMorphedMeshTangents(forceAll);
		DAZMeshGraftVertexPair[] vertexPairs = this.graftMesh.meshGraft.vertexPairs;
		int num = vertexPairs.Length;
		for (int i = 0; i < num; i++)
		{
			int graftToVertexNum = vertexPairs[i].graftToVertexNum;
			int vertexNum = vertexPairs[i].vertexNum;
			this._morphedUVTangents[graftToVertexNum] = this._morphedUVTangents[this.startGraftVertIndex + vertexNum];
		}
	}

	// Token: 0x06004A08 RID: 18952 RVA: 0x001843F8 File Offset: 0x001827F8
	public void UpdateVertices(bool force = false)
	{
		this.UpdateVerticesPre(false);
		this.UpdateVerticesThreaded(force);
		this.UpdateVerticesPost(true);
	}

	// Token: 0x17000A61 RID: 2657
	// (get) Token: 0x06004A09 RID: 18953 RVA: 0x0018440F File Offset: 0x0018280F
	public Vector3[] threadedMorphedUVVertices
	{
		get
		{
			return this._threadedMorphedUVVertices;
		}
	}

	// Token: 0x06004A0A RID: 18954 RVA: 0x00184418 File Offset: 0x00182818
	public void UpdateVerticesPre(bool forceChange = false)
	{
		if (this.targetMesh != null)
		{
			this._targetMeshVerticesChangedThisFrame = (this.targetMesh.verticesChangedThisFrame || forceChange);
			this._targetMeshVisibleNonPoseVerticesChangedThisFrame = this.targetMesh.visibleNonPoseVerticesChangedThisFrame;
			this._targetMeshVisibleNonPoseVerticesChangedLastFrame = this.targetMesh.visibleNonPoseVerticesChangedLastFrame;
		}
		if (this.graftMesh != null)
		{
			this._graftMeshVerticesChangedThisFrame = (this.graftMesh.verticesChangedThisFrame || forceChange);
			this._graftMeshVisibleNonPoseVerticesChangedThisFrame = this.graftMesh.visibleNonPoseVerticesChangedThisFrame;
			this._graftMeshVisibleNonPoseVerticesChangedLastFrame = this.graftMesh.visibleNonPoseVerticesChangedLastFrame;
		}
		if (this.graft2Mesh != null)
		{
			this._graft2MeshVerticesChangedThisFrame = (this.graft2Mesh.verticesChangedThisFrame || forceChange);
			this._graft2MeshVisibleNonPoseVerticesChangedThisFrame = this.graft2Mesh.visibleNonPoseVerticesChangedThisFrame;
			this._graft2MeshVisibleNonPoseVerticesChangedLastFrame = this.graft2Mesh.visibleNonPoseVerticesChangedLastFrame;
		}
		this._verticesChangedLastFrame = this._verticesChangedThisFrame;
		this._threadedVerticesChangedThisFrame = false;
		this._verticesChangedThisFrame = false;
		this._visibleNonPoseVerticesChangedLastFrame = this._visibleNonPoseVerticesChangedThisFrame;
		this._threadedVisibleNonPoseVerticesChangedThisFrame = false;
		this._visibleNonPoseVerticesChangedThisFrame = false;
	}

	// Token: 0x06004A0B RID: 18955 RVA: 0x00184540 File Offset: 0x00182940
	public void UpdateVerticesThreaded(bool force = false)
	{
		if (this.targetMesh == null || this.graftMesh == null)
		{
			return;
		}
		if (this.graftFactorsDirty)
		{
			force = true;
			this.graftFactorsDirty = false;
		}
		bool flag = false;
		Vector3[] morphedUVVertices = this.targetMesh.morphedUVVertices;
		Vector3[] visibleMorphedUVVertices = this.targetMesh.visibleMorphedUVVertices;
		Vector3[] morphedBaseVertices = this.targetMesh.morphedBaseVertices;
		int numUVVertices = this.graftMesh.numUVVertices;
		if (this._targetMeshVerticesChangedThisFrame || force)
		{
			Vector3[] morphedUVNormals = this.targetMesh.morphedUVNormals;
			for (int i = 0; i < this.numTargetBaseVertices; i++)
			{
				this._threadedMorphedUVVertices[i] = morphedUVVertices[i];
				this._threadedVisibleMorphedUVVertices[i] = visibleMorphedUVVertices[i];
				this._threadedMorphedBaseVertices[i] = morphedBaseVertices[i];
				if ((this.targetMesh.recalcNormalsOnMorph && this.targetMesh.normalsDirtyThisFrame) || force)
				{
					flag = true;
					this._morphedUVNormals[i] = morphedUVNormals[i];
				}
			}
			if ((this.targetMesh.recalcTangentsOnMorph && this.targetMesh.tangentsDirtyThisFrame) || force)
			{
				Vector4[] morphedUVTangents = this.targetMesh.morphedUVTangents;
				for (int j = 0; j < this.numTargetUVVertices; j++)
				{
					int num = j;
					if (j >= this.numTargetBaseVertices)
					{
						num += this.numGraftBaseVertices + this.numGraft2BaseVertices;
					}
					this._morphedUVTangents[num] = morphedUVTangents[j];
				}
			}
		}
		if ((this.graftMesh.normalsDirtyThisFrame && this.graftMesh.recalcNormalsOnMorph) || force)
		{
			flag = true;
			Vector3[] morphedUVNormals2 = this.graftMesh.morphedUVNormals;
			for (int k = 0; k < this.numGraftBaseVertices; k++)
			{
				this._morphedUVNormals[this.startGraftVertIndex + k] = morphedUVNormals2[k];
			}
		}
		if ((this.graftMesh.tangentsDirtyThisFrame && this.graftMesh.recalcTangentsOnMorph) || force)
		{
			Vector4[] morphedUVTangents2 = this.graftMesh.morphedUVTangents;
			for (int l = 0; l < this.graftMesh.numUVVertices; l++)
			{
				int num2;
				if (l >= this.numGraftBaseVertices)
				{
					num2 = l + this.numTargetUVVertices + this.numGraft2BaseVertices;
				}
				else
				{
					num2 = l + this.startGraftVertIndex;
				}
				this._morphedUVTangents[num2] = morphedUVTangents2[l];
			}
		}
		if (this.hasGraft2 && ((this.graft2Mesh.normalsDirtyThisFrame && this.graftMesh.recalcNormalsOnMorph) || force))
		{
			flag = true;
			Vector3[] morphedUVNormals3 = this.graft2Mesh.morphedUVNormals;
			for (int m = 0; m < this.numGraft2BaseVertices; m++)
			{
				this._morphedUVNormals[this.startGraft2VertIndex + m] = morphedUVNormals3[m];
			}
		}
		if (this.hasGraft2 && ((this.graft2Mesh.tangentsDirtyThisFrame && this.graft2Mesh.recalcTangentsOnMorph) || force))
		{
			Vector4[] morphedUVTangents3 = this.graft2Mesh.morphedUVTangents;
			for (int n = 0; n < this.graft2Mesh.numUVVertices; n++)
			{
				int num3;
				if (n >= this.numGraft2BaseVertices)
				{
					num3 = n + this.numTargetUVVertices + numUVVertices;
				}
				else
				{
					num3 = n + this.startGraft2VertIndex;
				}
				this._morphedUVTangents[num3] = morphedUVTangents3[n];
			}
		}
		if (flag || force)
		{
			base._updateDuplicateMorphedUVNormals();
		}
		bool flag2 = false;
		Vector3[] morphedUVVertices2 = this.graftMesh.morphedUVVertices;
		Vector3[] visibleMorphedUVVertices2 = this.graftMesh.visibleMorphedUVVertices;
		Vector3[] baseVertices = this.targetMesh.baseVertices;
		DAZMeshGraftVertexPair[] vertexPairs = this.graftMesh.meshGraft.vertexPairs;
		int num4 = vertexPairs.Length;
		float num5 = 0f;
		float num6 = 0f;
		float num7 = 0f;
		float num8 = 0f;
		float num9 = 0f;
		float num10 = 0f;
		if (this._targetMeshVerticesChangedThisFrame || force)
		{
			for (int num11 = 0; num11 < num4; num11++)
			{
				int graftToVertexNum = vertexPairs[num11].graftToVertexNum;
				int vertexNum = vertexPairs[num11].vertexNum;
				int num12 = this.startGraftVertIndex + vertexNum;
				float num13 = morphedUVVertices[graftToVertexNum].x - baseVertices[graftToVertexNum].x;
				float num14 = morphedUVVertices[graftToVertexNum].y - baseVertices[graftToVertexNum].y;
				float num15 = morphedUVVertices[graftToVertexNum].z - baseVertices[graftToVertexNum].z;
				num5 += num13;
				num6 += num14;
				num7 += num15;
				if (this._graftMovements[vertexNum].x != num13)
				{
					flag2 = true;
					this._graftMovements[vertexNum].x = num13;
				}
				if (this._graftMovements[vertexNum].y != num14)
				{
					flag2 = true;
					this._graftMovements[vertexNum].y = num14;
				}
				if (this._graftMovements[vertexNum].z != num15)
				{
					flag2 = true;
					this._graftMovements[vertexNum].z = num15;
				}
				float num16 = visibleMorphedUVVertices[graftToVertexNum].x - baseVertices[graftToVertexNum].x;
				float num17 = visibleMorphedUVVertices[graftToVertexNum].y - baseVertices[graftToVertexNum].y;
				float num18 = visibleMorphedUVVertices[graftToVertexNum].z - baseVertices[graftToVertexNum].z;
				num8 += num16;
				num9 += num17;
				num10 += num18;
				if (this._graftMovements2[vertexNum].x != num16)
				{
					this._graftMovements2[vertexNum].x = num16;
				}
				if (this._graftMovements2[vertexNum].y != num17)
				{
					this._graftMovements2[vertexNum].y = num17;
				}
				if (this._graftMovements2[vertexNum].z != num18)
				{
					this._graftMovements2[vertexNum].z = num18;
				}
				this._threadedMorphedUVVertices[num12] = morphedUVVertices[graftToVertexNum];
				this._threadedMorphedBaseVertices[num12] = morphedBaseVertices[graftToVertexNum];
				this._threadedVisibleMorphedUVVertices[num12] = visibleMorphedUVVertices[graftToVertexNum];
			}
		}
		float num19 = num5 / (float)num4;
		float num20 = num6 / (float)num4;
		float num21 = num7 / (float)num4;
		float num22 = num8 / (float)num4;
		float num23 = num9 / (float)num4;
		float num24 = num10 / (float)num4;
		this.morphedNormalsDirty = false;
		this._threadedVisibleNonPoseVerticesChangedThisFrame = (this._targetMeshVisibleNonPoseVerticesChangedThisFrame || this._graftMeshVisibleNonPoseVerticesChangedThisFrame || this._targetMeshVisibleNonPoseVerticesChangedLastFrame || this._graftMeshVisibleNonPoseVerticesChangedLastFrame);
		if (this._targetMeshVerticesChangedThisFrame || this._graftMeshVerticesChangedThisFrame || force)
		{
			this._threadedVerticesChangedThisFrame = true;
			MeshPoly[] basePolyList = this.targetMesh.basePolyList;
			switch (this.graftMethod)
			{
			case DAZMergedMesh.GraftMethod.Closest:
				for (int num25 = 0; num25 < this.numGraftBaseVertices; num25++)
				{
					if (this._graftIsFreeVert[num25])
					{
						int num26 = this.startGraftVertIndex + num25;
						DAZMergedMesh.FreeVertGraftWeight freeVertGraftWeight = this._freeVertGraftWeights[num25];
						int graftVert = freeVertGraftWeight.graftVert;
						float weight = freeVertGraftWeight.weight;
						if (weight > 0f)
						{
							float num27 = 1f - weight;
							float num28 = morphedUVVertices[graftVert].x - baseVertices[graftVert].x;
							float num29 = morphedUVVertices[graftVert].y - baseVertices[graftVert].y;
							float num30 = morphedUVVertices[graftVert].z - baseVertices[graftVert].z;
							this._graftMovements[num25].x = num19 * num27 + num28 * weight;
							this._graftMovements[num25].y = num20 * num27 + num29 * weight;
							this._graftMovements[num25].z = num21 * num27 + num30 * weight;
							float num31 = visibleMorphedUVVertices[graftVert].x - baseVertices[graftVert].x;
							float num32 = visibleMorphedUVVertices[graftVert].y - baseVertices[graftVert].y;
							float num33 = visibleMorphedUVVertices[graftVert].z - baseVertices[graftVert].z;
							this._graftMovements2[num25].x = num22 * num27 + num31 * weight;
							this._graftMovements2[num25].y = num23 * num27 + num32 * weight;
							this._graftMovements2[num25].z = num24 * num27 + num33 * weight;
						}
						else
						{
							this._graftMovements[num25].x = num19;
							this._graftMovements[num25].y = num20;
							this._graftMovements[num25].z = num21;
							this._graftMovements2[num25].x = num22;
							this._graftMovements2[num25].y = num23;
							this._graftMovements2[num25].z = num24;
						}
						this._threadedMorphedUVVertices[num26].x = morphedUVVertices2[num25].x + this._graftMovements[num25].x;
						this._threadedMorphedUVVertices[num26].y = morphedUVVertices2[num25].y + this._graftMovements[num25].y;
						this._threadedMorphedUVVertices[num26].z = morphedUVVertices2[num25].z + this._graftMovements[num25].z;
						this._threadedMorphedBaseVertices[num26] = this._morphedUVVertices[num26];
						this._threadedVisibleMorphedUVVertices[num26].x = morphedUVVertices2[num25].x + this._graftMovements2[num25].x;
						this._threadedVisibleMorphedUVVertices[num26].y = morphedUVVertices2[num25].y + this._graftMovements2[num25].y;
						this._threadedVisibleMorphedUVVertices[num26].z = morphedUVVertices2[num25].z + this._graftMovements2[num25].z;
					}
				}
				break;
			case DAZMergedMesh.GraftMethod.Boundary:
				for (int num34 = 0; num34 < this.numGraftBaseVertices; num34++)
				{
					if (this._graftIsFreeVert[num34])
					{
						int num35 = this.startGraftVertIndex + num34;
						if (flag2 || force)
						{
							Vector3 vector;
							vector.x = 0f;
							vector.y = 0f;
							vector.z = 0f;
							Vector3 vector2;
							vector2.x = 0f;
							vector2.y = 0f;
							vector2.z = 0f;
							float num36 = this._graftXFactor;
							float num37 = this._graftYFactor;
							float num38 = this._graftZFactor;
							if (this.useGraftSymmetry)
							{
								DAZMergedMesh.GraftSymmetryAxis graftSymmetryAxis = this.graftSymmetryAxis;
								if (graftSymmetryAxis != DAZMergedMesh.GraftSymmetryAxis.X)
								{
									if (graftSymmetryAxis != DAZMergedMesh.GraftSymmetryAxis.Y)
									{
										if (graftSymmetryAxis == DAZMergedMesh.GraftSymmetryAxis.Z)
										{
											float num39 = Mathf.Abs(morphedUVVertices2[num34].z);
											float num40 = Mathf.Clamp01(num39 / this.graftSymmetryDistance);
											num38 *= num40;
										}
									}
									else
									{
										float num39 = Mathf.Abs(morphedUVVertices2[num34].y);
										float num40 = Mathf.Clamp01(num39 / this.graftSymmetryDistance);
										num37 *= num40;
									}
								}
								else
								{
									float num39 = Mathf.Abs(morphedUVVertices2[num34].x);
									float num40 = Mathf.Clamp01(num39 / this.graftSymmetryDistance);
									num36 *= num40;
								}
							}
							for (int num41 = 0; num41 < num4; num41++)
							{
								int vertexNum2 = vertexPairs[num41].vertexNum;
								int num42 = num41 * this.numGraftBaseVertices + num34;
								float num43 = this._graftWeights[num42];
								vector.x += this._graftMovements[vertexNum2].x * num43 * num36;
								vector.y += this._graftMovements[vertexNum2].y * num43 * num37;
								vector.z += this._graftMovements[vertexNum2].z * num43 * num38;
								vector2.x += this._graftMovements2[vertexNum2].x * num43 * num36;
								vector2.y += this._graftMovements2[vertexNum2].y * num43 * num37;
								vector2.z += this._graftMovements2[vertexNum2].z * num43 * num38;
							}
							this._graftMovements[num34] = vector;
							this._graftMovements2[num34] = vector2;
						}
						this._threadedMorphedUVVertices[num35].x = morphedUVVertices2[num34].x + this._graftMovements[num34].x;
						this._threadedMorphedUVVertices[num35].y = morphedUVVertices2[num34].y + this._graftMovements[num34].y;
						this._threadedMorphedUVVertices[num35].z = morphedUVVertices2[num34].z + this._graftMovements[num34].z;
						this._threadedMorphedBaseVertices[num35] = this._morphedUVVertices[num35];
						this._threadedVisibleMorphedUVVertices[num35].x = morphedUVVertices2[num34].x + this._graftMovements2[num34].x;
						this._threadedVisibleMorphedUVVertices[num35].y = morphedUVVertices2[num34].y + this._graftMovements2[num34].y;
						this._threadedVisibleMorphedUVVertices[num35].z = morphedUVVertices2[num34].z + this._graftMovements2[num34].z;
					}
				}
				break;
			case DAZMergedMesh.GraftMethod.ClosestPoly:
				for (int num44 = 0; num44 < this.numGraftBaseVertices; num44++)
				{
					if (this._graftIsFreeVert[num44])
					{
						int num45 = this.startGraftVertIndex + num44;
						DAZMergedMesh.FreeVertGraftWeight freeVertGraftWeight2 = this._freeVertGraftWeights[num44];
						int graftPoly = freeVertGraftWeight2.graftPoly;
						float weight2 = freeVertGraftWeight2.weight;
						if (weight2 > 0f)
						{
							float num46 = 1f - weight2;
							float num47 = 0f;
							float num48 = 0f;
							float num49 = 0f;
							float num50 = 0f;
							float num51 = 0f;
							float num52 = 0f;
							int[] vertices = basePolyList[graftPoly].vertices;
							int num53 = vertices.Length;
							float num54 = 1f / (float)num53;
							for (int num55 = 0; num55 < num53; num55++)
							{
								int num56 = vertices[num55];
								num47 += (morphedUVVertices[num56].x - baseVertices[num56].x) * num54;
								num48 += (morphedUVVertices[num56].y - baseVertices[num56].y) * num54;
								num49 += (morphedUVVertices[num56].z - baseVertices[num56].z) * num54;
								num50 += (visibleMorphedUVVertices[num56].x - baseVertices[num56].x) * num54;
								num51 += (visibleMorphedUVVertices[num56].y - baseVertices[num56].y) * num54;
								num52 += (visibleMorphedUVVertices[num56].z - baseVertices[num56].z) * num54;
							}
							this._graftMovements[num44].x = num19 * num46 + num47 * weight2;
							this._graftMovements[num44].y = num20 * num46 + num48 * weight2;
							this._graftMovements[num44].z = num21 * num46 + num49 * weight2;
							this._graftMovements2[num44].x = num22 * num46 + num50 * weight2;
							this._graftMovements2[num44].y = num23 * num46 + num51 * weight2;
							this._graftMovements2[num44].z = num24 * num46 + num52 * weight2;
						}
						else
						{
							this._graftMovements[num44].x = num19;
							this._graftMovements[num44].y = num20;
							this._graftMovements[num44].z = num21;
							this._graftMovements2[num44].x = num22;
							this._graftMovements2[num44].y = num23;
							this._graftMovements2[num44].z = num24;
						}
						this._threadedMorphedUVVertices[num45].x = morphedUVVertices2[num44].x + this._graftMovements[num44].x;
						this._threadedMorphedUVVertices[num45].y = morphedUVVertices2[num44].y + this._graftMovements[num44].y;
						this._threadedMorphedUVVertices[num45].z = morphedUVVertices2[num44].z + this._graftMovements[num44].z;
						this._threadedMorphedBaseVertices[num45] = this._morphedUVVertices[num45];
						this._threadedVisibleMorphedUVVertices[num45].x = morphedUVVertices2[num44].x + this._graftMovements2[num44].x;
						this._threadedVisibleMorphedUVVertices[num45].y = morphedUVVertices2[num44].y + this._graftMovements2[num44].y;
						this._threadedVisibleMorphedUVVertices[num45].z = morphedUVVertices2[num44].z + this._graftMovements2[num44].z;
					}
				}
				break;
			case DAZMergedMesh.GraftMethod.ClosestVertAndPoly:
				for (int num57 = 0; num57 < this.numGraftBaseVertices; num57++)
				{
					if (this._graftIsFreeVert[num57])
					{
						int num58 = this.startGraftVertIndex + num57;
						DAZMergedMesh.FreeVertGraftWeight freeVertGraftWeight3 = this._freeVertGraftWeights[num57];
						int graftPoly2 = freeVertGraftWeight3.graftPoly;
						int graftVert2 = freeVertGraftWeight3.graftVert;
						float weight3 = freeVertGraftWeight3.weight;
						if (weight3 > 0f)
						{
							float num59 = 1f - weight3;
							float num60 = 1f - freeVertGraftWeight3.graftVertToPolyRatio;
							float num61 = (morphedUVVertices[graftVert2].x - baseVertices[graftVert2].x) * num60;
							float num62 = (morphedUVVertices[graftVert2].y - baseVertices[graftVert2].y) * num60;
							float num63 = (morphedUVVertices[graftVert2].z - baseVertices[graftVert2].z) * num60;
							float num64 = (visibleMorphedUVVertices[graftVert2].x - baseVertices[graftVert2].x) * num60;
							float num65 = (visibleMorphedUVVertices[graftVert2].y - baseVertices[graftVert2].y) * num60;
							float num66 = (visibleMorphedUVVertices[graftVert2].z - baseVertices[graftVert2].z) * num60;
							if (freeVertGraftWeight3.graftVertToPolyRatio > 0f && graftPoly2 != -1)
							{
								int[] vertices2 = basePolyList[graftPoly2].vertices;
								int num67 = vertices2.Length;
								float graftVertToPolyRatio = freeVertGraftWeight3.graftVertToPolyRatio;
								float num68 = 1f / (float)num67 * graftVertToPolyRatio;
								for (int num69 = 0; num69 < num67; num69++)
								{
									int num70 = vertices2[num69];
									num61 += (morphedUVVertices[num70].x - baseVertices[num70].x) * num68;
									num62 += (morphedUVVertices[num70].y - baseVertices[num70].y) * num68;
									num63 += (morphedUVVertices[num70].z - baseVertices[num70].z) * num68;
									num64 += (visibleMorphedUVVertices[num70].x - baseVertices[num70].x) * num68;
									num65 += (visibleMorphedUVVertices[num70].y - baseVertices[num70].y) * num68;
									num66 += (visibleMorphedUVVertices[num70].z - baseVertices[num70].z) * num68;
								}
							}
							this._graftMovements[num57].x = num19 * num59 + num61 * weight3;
							this._graftMovements[num57].y = num20 * num59 + num62 * weight3;
							this._graftMovements[num57].z = num21 * num59 + num63 * weight3;
							this._graftMovements2[num57].x = num22 * num59 + num64 * weight3;
							this._graftMovements2[num57].y = num23 * num59 + num65 * weight3;
							this._graftMovements2[num57].z = num24 * num59 + num66 * weight3;
						}
						else
						{
							this._graftMovements[num57].x = num19;
							this._graftMovements[num57].y = num20;
							this._graftMovements[num57].z = num21;
							this._graftMovements2[num57].x = num22;
							this._graftMovements2[num57].y = num23;
							this._graftMovements2[num57].z = num24;
						}
						this._threadedMorphedUVVertices[num58].x = morphedUVVertices2[num57].x + this._graftMovements[num57].x;
						this._threadedMorphedUVVertices[num58].y = morphedUVVertices2[num57].y + this._graftMovements[num57].y;
						this._threadedMorphedUVVertices[num58].z = morphedUVVertices2[num57].z + this._graftMovements[num57].z;
						this._threadedMorphedBaseVertices[num58] = this._morphedUVVertices[num58];
						this._threadedVisibleMorphedUVVertices[num58].x = morphedUVVertices2[num57].x + this._graftMovements2[num57].x;
						this._threadedVisibleMorphedUVVertices[num58].y = morphedUVVertices2[num57].y + this._graftMovements2[num57].y;
						this._threadedVisibleMorphedUVVertices[num58].z = morphedUVVertices2[num57].z + this._graftMovements2[num57].z;
					}
				}
				break;
			}
			if ((flag2 || force) && this.recalcNormalsOnMorph)
			{
				for (int num71 = 0; num71 < this.numGraftBaseVertices; num71++)
				{
					base.morphedBaseDirtyVertices[this.startGraftVertIndex + num71] = true;
				}
				for (int num72 = 0; num72 < num4; num72++)
				{
					int graftToVertexNum2 = vertexPairs[num72].graftToVertexNum;
					base.morphedBaseDirtyVertices[graftToVertexNum2] = true;
				}
				this.morphedNormalsDirty = true;
			}
			if ((flag2 || force) && this.recalcTangentsOnMorph)
			{
				for (int num73 = 0; num73 < this.numGraftBaseVertices; num73++)
				{
					base.morphedUVDirtyVertices[this.startGraftVertIndex + num73] = true;
				}
				for (int num74 = 0; num74 < num4; num74++)
				{
					int graftToVertexNum3 = vertexPairs[num74].graftToVertexNum;
					base.morphedUVDirtyVertices[graftToVertexNum3] = true;
				}
				this.morphedTangentsDirty = true;
			}
		}
		if (this.hasGraft2)
		{
			Vector3[] morphedUVVertices3 = this.graft2Mesh.morphedUVVertices;
			Vector3[] visibleMorphedUVVertices3 = this.graft2Mesh.visibleMorphedUVVertices;
			DAZMeshGraftVertexPair[] vertexPairs2 = this.graft2Mesh.meshGraft.vertexPairs;
			int num75 = vertexPairs2.Length;
			bool flag3 = false;
			if (this._targetMeshVerticesChangedThisFrame || force)
			{
				for (int num76 = 0; num76 < num75; num76++)
				{
					int graftToVertexNum4 = vertexPairs2[num76].graftToVertexNum;
					int vertexNum3 = vertexPairs2[num76].vertexNum;
					int num77 = this.startGraft2VertIndex + vertexNum3;
					float num78 = morphedUVVertices[graftToVertexNum4].x - baseVertices[graftToVertexNum4].x;
					float num79 = morphedUVVertices[graftToVertexNum4].y - baseVertices[graftToVertexNum4].y;
					float num80 = morphedUVVertices[graftToVertexNum4].z - baseVertices[graftToVertexNum4].z;
					float num81 = visibleMorphedUVVertices[graftToVertexNum4].x - baseVertices[graftToVertexNum4].x;
					float num82 = visibleMorphedUVVertices[graftToVertexNum4].y - baseVertices[graftToVertexNum4].y;
					float num83 = visibleMorphedUVVertices[graftToVertexNum4].z - baseVertices[graftToVertexNum4].z;
					if (this._graft2Movements[vertexNum3].x != num78)
					{
						flag3 = true;
						this._graft2Movements[vertexNum3].x = num78;
					}
					if (this._graft2Movements[vertexNum3].y != num79)
					{
						flag3 = true;
						this._graft2Movements[vertexNum3].y = num79;
					}
					if (this._graft2Movements[vertexNum3].z != num80)
					{
						flag3 = true;
						this._graft2Movements[vertexNum3].z = num80;
					}
					if (this._graft2Movements2[vertexNum3].x != num81)
					{
						this._graft2Movements2[vertexNum3].x = num81;
					}
					if (this._graft2Movements2[vertexNum3].y != num82)
					{
						this._graft2Movements2[vertexNum3].y = num82;
					}
					if (this._graft2Movements2[vertexNum3].z != num83)
					{
						this._graft2Movements2[vertexNum3].z = num83;
					}
					this._threadedMorphedUVVertices[num77] = morphedUVVertices[graftToVertexNum4];
					this._threadedMorphedBaseVertices[num77] = morphedUVVertices[graftToVertexNum4];
					this._threadedVisibleMorphedUVVertices[num77] = visibleMorphedUVVertices[graftToVertexNum4];
				}
			}
			if (this._targetMeshVerticesChangedThisFrame || this._graft2MeshVerticesChangedThisFrame || force)
			{
				this._threadedVerticesChangedThisFrame = true;
				this._threadedVisibleNonPoseVerticesChangedThisFrame = (this._visibleNonPoseVerticesChangedThisFrame || this._graft2MeshVisibleNonPoseVerticesChangedThisFrame);
				for (int num84 = 0; num84 < this.numGraft2BaseVertices; num84++)
				{
					if (this._graft2IsFreeVert[num84])
					{
						int num85 = this.startGraft2VertIndex + num84;
						if (flag3 || force)
						{
							Vector3 vector3;
							vector3.x = 0f;
							vector3.y = 0f;
							vector3.z = 0f;
							Vector3 vector4;
							vector4.x = 0f;
							vector4.y = 0f;
							vector4.z = 0f;
							for (int num86 = 0; num86 < num75; num86++)
							{
								int vertexNum4 = vertexPairs2[num86].vertexNum;
								int num87 = num86 * this.numGraft2BaseVertices + num84;
								float num88 = this._graft2Weights[num87];
								vector3.x += this._graft2Movements[vertexNum4].x * num88 * this._graftXFactor;
								vector3.y += this._graft2Movements[vertexNum4].y * num88 * this._graftYFactor;
								vector3.z += this._graft2Movements[vertexNum4].z * num88 * this._graftZFactor;
								vector4.x += this._graft2Movements2[vertexNum4].x * num88 * this._graftXFactor;
								vector4.y += this._graft2Movements2[vertexNum4].y * num88 * this._graftYFactor;
								vector4.z += this._graft2Movements2[vertexNum4].z * num88 * this._graftZFactor;
							}
							this._graft2Movements[num84] = vector3;
							this._graft2Movements2[num84] = vector4;
						}
						this._threadedMorphedUVVertices[num85].x = morphedUVVertices3[num84].x + this._graft2Movements[num84].x;
						this._threadedMorphedUVVertices[num85].y = morphedUVVertices3[num84].y + this._graft2Movements[num84].y;
						this._threadedMorphedUVVertices[num85].z = morphedUVVertices3[num84].z + this._graft2Movements[num84].z;
						this._threadedMorphedBaseVertices[num85] = this._morphedUVVertices[num85];
						this._threadedVisibleMorphedUVVertices[num85].x = visibleMorphedUVVertices3[num84].x + this._graft2Movements2[num84].x;
						this._threadedVisibleMorphedUVVertices[num85].y = visibleMorphedUVVertices3[num84].y + this._graft2Movements2[num84].y;
						this._threadedVisibleMorphedUVVertices[num85].z = visibleMorphedUVVertices3[num84].z + this._graft2Movements2[num84].z;
					}
				}
			}
			if ((flag3 || force) && this.recalcNormalsOnMorph)
			{
				for (int num89 = 0; num89 < this.numGraft2BaseVertices; num89++)
				{
					base.morphedBaseDirtyVertices[this.startGraft2VertIndex + num89] = true;
				}
				for (int num90 = 0; num90 < num75; num90++)
				{
					int graftToVertexNum5 = vertexPairs2[num90].graftToVertexNum;
					base.morphedBaseDirtyVertices[graftToVertexNum5] = true;
				}
				this.morphedNormalsDirty = true;
			}
			if ((flag3 || force) && this.recalcTangentsOnMorph)
			{
				for (int num91 = 0; num91 < this.numGraft2BaseVertices; num91++)
				{
					base.morphedUVDirtyVertices[this.startGraft2VertIndex + num91] = true;
				}
				for (int num92 = 0; num92 < num75; num92++)
				{
					int graftToVertexNum6 = vertexPairs2[num92].graftToVertexNum;
					base.morphedUVDirtyVertices[graftToVertexNum6] = true;
				}
				this.morphedTangentsDirty = true;
			}
		}
		base._triggerNormalAndTangentRecalc();
	}

	// Token: 0x06004A0C RID: 18956 RVA: 0x0018662C File Offset: 0x00184A2C
	public void UpdateVerticesPrepThreadedFast(Vector3[] mesh1MorphedVerts, Vector3[] mergedUVVerts, bool useAltMovementArray = false)
	{
		Vector3[] array;
		if (useAltMovementArray)
		{
			array = this._graftMovements2;
		}
		else
		{
			array = this._graftMovements;
		}
		Vector3[] baseVertices = this.targetMesh.baseVertices;
		for (int i = 0; i < baseVertices.Length; i++)
		{
			mergedUVVerts[i] = mesh1MorphedVerts[i];
		}
		DAZMeshGraftVertexPair[] vertexPairs = this.graftMesh.meshGraft.vertexPairs;
		int num = vertexPairs.Length;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		for (int j = 0; j < num; j++)
		{
			int graftToVertexNum = vertexPairs[j].graftToVertexNum;
			int vertexNum = vertexPairs[j].vertexNum;
			float num5 = mesh1MorphedVerts[graftToVertexNum].x - baseVertices[graftToVertexNum].x;
			float num6 = mesh1MorphedVerts[graftToVertexNum].y - baseVertices[graftToVertexNum].y;
			float num7 = mesh1MorphedVerts[graftToVertexNum].z - baseVertices[graftToVertexNum].z;
			num2 += num5;
			num3 += num6;
			num4 += num7;
			array[vertexNum].x = num5;
			array[vertexNum].y = num6;
			array[vertexNum].z = num7;
		}
		if (useAltMovementArray)
		{
			this.avgxdiff2 = num2 / (float)num;
			this.avgydiff2 = num3 / (float)num;
			this.avgzdiff2 = num4 / (float)num;
		}
		else
		{
			this.avgxdiff1 = num2 / (float)num;
			this.avgydiff1 = num3 / (float)num;
			this.avgzdiff1 = num4 / (float)num;
		}
	}

	// Token: 0x06004A0D RID: 18957 RVA: 0x001867D0 File Offset: 0x00184BD0
	public void UpdateVerticesThreadedFast(Vector3[] mesh1MorphedVerts, Vector3[] mesh2MorphedVerts, Vector3[] mesh3MorphedVerts, Vector3[] mergedUVVerts, int mini, int maxi, bool useAltMovementArray = false)
	{
		Vector3[] array;
		float num;
		float num2;
		float num3;
		if (useAltMovementArray)
		{
			array = this._graftMovements2;
			num = this.avgxdiff2;
			num2 = this.avgydiff2;
			num3 = this.avgzdiff2;
		}
		else
		{
			array = this._graftMovements;
			num = this.avgxdiff1;
			num2 = this.avgydiff1;
			num3 = this.avgzdiff1;
		}
		Vector3[] baseVertices = this.targetMesh.baseVertices;
		DAZMeshGraftVertexPair[] vertexPairs = this.graftMesh.meshGraft.vertexPairs;
		int num4 = vertexPairs.Length;
		MeshPoly[] basePolyList = this.targetMesh.basePolyList;
		switch (this.graftMethod)
		{
		case DAZMergedMesh.GraftMethod.Closest:
			for (int i = mini; i < maxi; i++)
			{
				if (this._graftIsFreeVert[i])
				{
					int num5 = this.startGraftVertIndex + i;
					DAZMergedMesh.FreeVertGraftWeight freeVertGraftWeight = this._freeVertGraftWeights[i];
					int graftVert = freeVertGraftWeight.graftVert;
					float weight = freeVertGraftWeight.weight;
					if (weight > 0f)
					{
						float num6 = 1f - weight;
						float num7 = mesh1MorphedVerts[graftVert].x - baseVertices[graftVert].x;
						float num8 = mesh1MorphedVerts[graftVert].y - baseVertices[graftVert].y;
						float num9 = mesh1MorphedVerts[graftVert].z - baseVertices[graftVert].z;
						array[i].x = num * num6 + num7 * weight;
						array[i].y = num2 * num6 + num8 * weight;
						array[i].z = num3 * num6 + num9 * weight;
					}
					else
					{
						array[i].x = num;
						array[i].y = num2;
						array[i].z = num3;
					}
					mergedUVVerts[num5].x = mesh2MorphedVerts[i].x + array[i].x;
					mergedUVVerts[num5].y = mesh2MorphedVerts[i].y + array[i].y;
					mergedUVVerts[num5].z = mesh2MorphedVerts[i].z + array[i].z;
				}
			}
			break;
		case DAZMergedMesh.GraftMethod.Boundary:
			for (int j = mini; j < maxi; j++)
			{
				if (this._graftIsFreeVert[j])
				{
					int num10 = this.startGraftVertIndex + j;
					Vector3 vector;
					vector.x = 0f;
					vector.y = 0f;
					vector.z = 0f;
					float num11 = this._graftXFactor;
					float num12 = this._graftYFactor;
					float num13 = this._graftZFactor;
					if (this.useGraftSymmetry)
					{
						DAZMergedMesh.GraftSymmetryAxis graftSymmetryAxis = this.graftSymmetryAxis;
						if (graftSymmetryAxis != DAZMergedMesh.GraftSymmetryAxis.X)
						{
							if (graftSymmetryAxis != DAZMergedMesh.GraftSymmetryAxis.Y)
							{
								if (graftSymmetryAxis == DAZMergedMesh.GraftSymmetryAxis.Z)
								{
									float num14 = Mathf.Abs(mesh2MorphedVerts[j].z);
									float num15 = Mathf.Clamp01(num14 / this.graftSymmetryDistance);
									num13 *= num15;
								}
							}
							else
							{
								float num14 = Mathf.Abs(mesh2MorphedVerts[j].y);
								float num15 = Mathf.Clamp01(num14 / this.graftSymmetryDistance);
								num12 *= num15;
							}
						}
						else
						{
							float num14 = Mathf.Abs(mesh2MorphedVerts[j].x);
							float num15 = Mathf.Clamp01(num14 / this.graftSymmetryDistance);
							num11 *= num15;
						}
					}
					for (int k = 0; k < num4; k++)
					{
						int vertexNum = vertexPairs[k].vertexNum;
						int num16 = k * this.numGraftBaseVertices + j;
						float num17 = this._graftWeights[num16];
						vector.x += array[vertexNum].x * num17 * num11;
						vector.y += array[vertexNum].y * num17 * num12;
						vector.z += array[vertexNum].z * num17 * num13;
					}
					array[j] = vector;
					mergedUVVerts[num10].x = mesh2MorphedVerts[j].x + array[j].x;
					mergedUVVerts[num10].y = mesh2MorphedVerts[j].y + array[j].y;
					mergedUVVerts[num10].z = mesh2MorphedVerts[j].z + array[j].z;
				}
			}
			break;
		case DAZMergedMesh.GraftMethod.ClosestPoly:
			for (int l = mini; l < maxi; l++)
			{
				if (this._graftIsFreeVert[l])
				{
					int num18 = this.startGraftVertIndex + l;
					DAZMergedMesh.FreeVertGraftWeight freeVertGraftWeight2 = this._freeVertGraftWeights[l];
					int graftPoly = freeVertGraftWeight2.graftPoly;
					float weight2 = freeVertGraftWeight2.weight;
					if (weight2 > 0f)
					{
						float num19 = 1f - weight2;
						float num20 = 0f;
						float num21 = 0f;
						float num22 = 0f;
						int[] vertices = basePolyList[graftPoly].vertices;
						int num23 = vertices.Length;
						float num24 = 1f / (float)num23;
						for (int m = 0; m < num23; m++)
						{
							int num25 = vertices[m];
							num20 += (mesh1MorphedVerts[num25].x - baseVertices[num25].x) * num24;
							num21 += (mesh1MorphedVerts[num25].y - baseVertices[num25].y) * num24;
							num22 += (mesh1MorphedVerts[num25].z - baseVertices[num25].z) * num24;
						}
						array[l].x = num * num19 + num20 * weight2;
						array[l].y = num2 * num19 + num21 * weight2;
						array[l].z = num3 * num19 + num22 * weight2;
					}
					else
					{
						array[l].x = num;
						array[l].y = num2;
						array[l].z = num3;
					}
					mergedUVVerts[num18].x = mesh2MorphedVerts[l].x + array[l].x;
					mergedUVVerts[num18].y = mesh2MorphedVerts[l].y + array[l].y;
					mergedUVVerts[num18].z = mesh2MorphedVerts[l].z + array[l].z;
				}
			}
			break;
		case DAZMergedMesh.GraftMethod.ClosestVertAndPoly:
			for (int n = mini; n < maxi; n++)
			{
				if (this._graftIsFreeVert[n])
				{
					int num26 = this.startGraftVertIndex + n;
					DAZMergedMesh.FreeVertGraftWeight freeVertGraftWeight3 = this._freeVertGraftWeights[n];
					int graftPoly2 = freeVertGraftWeight3.graftPoly;
					int graftVert2 = freeVertGraftWeight3.graftVert;
					float weight3 = freeVertGraftWeight3.weight;
					if (weight3 > 0f)
					{
						float num27 = 1f - weight3;
						float num28 = 1f - freeVertGraftWeight3.graftVertToPolyRatio;
						float num29 = (mesh1MorphedVerts[graftVert2].x - baseVertices[graftVert2].x) * num28;
						float num30 = (mesh1MorphedVerts[graftVert2].y - baseVertices[graftVert2].y) * num28;
						float num31 = (mesh1MorphedVerts[graftVert2].z - baseVertices[graftVert2].z) * num28;
						if (freeVertGraftWeight3.graftVertToPolyRatio > 0f && graftPoly2 != -1)
						{
							int[] vertices2 = basePolyList[graftPoly2].vertices;
							int num32 = vertices2.Length;
							float graftVertToPolyRatio = freeVertGraftWeight3.graftVertToPolyRatio;
							float num33 = 1f / (float)num32 * graftVertToPolyRatio;
							for (int num34 = 0; num34 < num32; num34++)
							{
								int num35 = vertices2[num34];
								num29 += (mesh1MorphedVerts[num35].x - baseVertices[num35].x) * num33;
								num30 += (mesh1MorphedVerts[num35].y - baseVertices[num35].y) * num33;
								num31 += (mesh1MorphedVerts[num35].z - baseVertices[num35].z) * num33;
							}
						}
						array[n].x = num * num27 + num29 * weight3;
						array[n].y = num2 * num27 + num30 * weight3;
						array[n].z = num3 * num27 + num31 * weight3;
					}
					else
					{
						array[n].x = num;
						array[n].y = num2;
						array[n].z = num3;
					}
					mergedUVVerts[num26].x = mesh2MorphedVerts[n].x + array[n].x;
					mergedUVVerts[num26].y = mesh2MorphedVerts[n].y + array[n].y;
					mergedUVVerts[num26].z = mesh2MorphedVerts[n].z + array[n].z;
				}
			}
			break;
		}
	}

	// Token: 0x06004A0E RID: 18958 RVA: 0x00187138 File Offset: 0x00185538
	public void UpdateVerticesFinishThreadedFast(Vector3[] mesh1MorphedVerts, Vector3[] mesh2MorphedVerts, Vector3[] mesh3MorphedVerts, Vector3[] mergedUVVerts, bool useAltMovementArray = false)
	{
		Vector3[] array;
		if (this.hasGraft2)
		{
			if (useAltMovementArray)
			{
				array = this._graft2Movements2;
			}
			else
			{
				array = this._graft2Movements;
			}
		}
		else
		{
			array = null;
		}
		Vector3[] baseVertices = this.targetMesh.baseVertices;
		if (this.hasGraft2)
		{
			DAZMeshGraftVertexPair[] vertexPairs = this.graft2Mesh.meshGraft.vertexPairs;
			int num = vertexPairs.Length;
			for (int i = 0; i < num; i++)
			{
				int graftToVertexNum = vertexPairs[i].graftToVertexNum;
				int vertexNum = vertexPairs[i].vertexNum;
				float x = mesh1MorphedVerts[graftToVertexNum].x - baseVertices[graftToVertexNum].x;
				float y = mesh1MorphedVerts[graftToVertexNum].y - baseVertices[graftToVertexNum].y;
				float z = mesh1MorphedVerts[graftToVertexNum].z - baseVertices[graftToVertexNum].z;
				array[vertexNum].x = x;
				array[vertexNum].y = y;
				array[vertexNum].z = z;
			}
			for (int j = 0; j < this.numGraft2BaseVertices; j++)
			{
				if (this._graft2IsFreeVert[j])
				{
					int num2 = this.startGraft2VertIndex + j;
					Vector3 vector;
					vector.x = 0f;
					vector.y = 0f;
					vector.z = 0f;
					float num3 = this._graftXFactor;
					float num4 = this._graftYFactor;
					float num5 = this._graftZFactor;
					if (this.useGraftSymmetry)
					{
						DAZMergedMesh.GraftSymmetryAxis graftSymmetryAxis = this.graftSymmetryAxis;
						if (graftSymmetryAxis != DAZMergedMesh.GraftSymmetryAxis.X)
						{
							if (graftSymmetryAxis != DAZMergedMesh.GraftSymmetryAxis.Y)
							{
								if (graftSymmetryAxis == DAZMergedMesh.GraftSymmetryAxis.Z)
								{
									float num6 = Mathf.Abs(mesh3MorphedVerts[j].z);
									float num7 = Mathf.Clamp01(num6 / this.graftSymmetryDistance);
									num5 *= num7;
								}
							}
							else
							{
								float num6 = Mathf.Abs(mesh3MorphedVerts[j].y);
								float num7 = Mathf.Clamp01(num6 / this.graftSymmetryDistance);
								num4 *= num7;
							}
						}
						else
						{
							float num6 = Mathf.Abs(mesh3MorphedVerts[j].x);
							float num7 = Mathf.Clamp01(num6 / this.graftSymmetryDistance);
							num3 *= num7;
						}
					}
					for (int k = 0; k < num; k++)
					{
						int vertexNum2 = vertexPairs[k].vertexNum;
						int num8 = k * this.numGraft2BaseVertices + j;
						float num9 = this._graft2Weights[num8];
						vector.x += array[vertexNum2].x * num9 * num3;
						vector.y += array[vertexNum2].y * num9 * num4;
						vector.z += array[vertexNum2].z * num9 * num5;
					}
					array[j] = vector;
					mergedUVVerts[num2].x = mesh3MorphedVerts[j].x + array[j].x;
					mergedUVVerts[num2].y = mesh3MorphedVerts[j].y + array[j].y;
					mergedUVVerts[num2].z = mesh3MorphedVerts[j].z + array[j].z;
				}
			}
		}
		if (base.baseVerticesToUVVertices != null)
		{
			foreach (DAZVertexMap dazvertexMap in base.baseVerticesToUVVertices)
			{
				mergedUVVerts[dazvertexMap.tovert] = mergedUVVerts[dazvertexMap.fromvert];
			}
		}
	}

	// Token: 0x06004A0F RID: 18959 RVA: 0x001874FC File Offset: 0x001858FC
	public void UpdateVerticesPost(bool updateBaseVertices = false)
	{
		this._verticesChangedThisFrame = this._threadedVerticesChangedThisFrame;
		this._visibleNonPoseVerticesChangedThisFrame = this._threadedVisibleNonPoseVerticesChangedThisFrame;
		if (this._verticesChangedThisFrame || this._visibleNonPoseVerticesChangedThisFrame)
		{
			if (updateBaseVertices)
			{
				for (int i = 0; i < this._numBaseVertices; i++)
				{
					this._morphedBaseVertices[i] = this._threadedMorphedBaseVertices[i];
					this._morphedUVVertices[i] = this._threadedMorphedUVVertices[i];
					this._visibleMorphedUVVertices[i] = this._threadedVisibleMorphedUVVertices[i];
				}
				for (int j = this._numBaseVertices; j < this._numUVVertices; j++)
				{
					this._morphedUVVertices[j] = this._threadedMorphedUVVertices[j];
					this._visibleMorphedUVVertices[j] = this._threadedVisibleMorphedUVVertices[j];
				}
			}
			else
			{
				for (int k = 0; k < this._numUVVertices; k++)
				{
					this._morphedUVVertices[k] = this._threadedMorphedUVVertices[k];
					this._visibleMorphedUVVertices[k] = this._threadedVisibleMorphedUVVertices[k];
				}
			}
		}
		if (this._useSmoothing)
		{
			base.InitMeshSmooth();
			this.meshSmooth.LaplacianSmooth(this._morphedUVVertices, this._smoothedMorphedUVVertices, 0, 100000000);
			this.meshSmooth.HCCorrection(this._morphedUVVertices, this._smoothedMorphedUVVertices, 0.5f, 0, 1000000000);
			base._updateDuplicateSmoothedMorphedUVVertices();
			if (this._drawMorphedUVMappedMesh || !Application.isPlaying)
			{
				this._morphedUVMappedMesh.vertices = this._smoothedMorphedUVVertices;
				this._morphedUVMappedMesh.normals = this._morphedUVNormals;
				this._morphedUVMappedMesh.tangents = this._morphedUVTangents;
			}
		}
		else
		{
			base._updateDuplicateMorphedUVVertices();
			if (this._drawMorphedUVMappedMesh || !Application.isPlaying)
			{
				this._morphedUVMappedMesh.vertices = this._morphedUVVertices;
				this._morphedUVMappedMesh.normals = this._morphedUVNormals;
				this._morphedUVMappedMesh.tangents = this._morphedUVTangents;
			}
		}
		if (base.drawMorphedBaseMesh)
		{
			this._morphedBaseMesh.vertices = this._morphedBaseVertices;
			this._morphedBaseMesh.normals = this._morphedBaseNormals;
		}
	}

	// Token: 0x06004A10 RID: 18960 RVA: 0x00187798 File Offset: 0x00185B98
	public new void Draw()
	{
		base.Draw();
		if (this.drawGraftMorphedMesh && this.graftMesh != null && this._graftMorphedMesh != null)
		{
			Vector3[] baseVertices = this.graftMesh.baseVertices;
			for (int i = 0; i < baseVertices.Length; i++)
			{
				this._graftMorphedMeshVertices[i] = baseVertices[i];
			}
			if (this.graftMeshMorphNamesForGrafting != null && this.graftMesh.morphBank != null)
			{
				if (this.graftMeshMorphNamesForGrafting.Length == this.graftMeshMorphValuesForGrafting.Length)
				{
					for (int j = 0; j < this.graftMeshMorphNamesForGrafting.Length; j++)
					{
						float d = this.graftMeshMorphValuesForGrafting[j];
						DAZMorph builtInMorph = this.graftMesh.morphBank.GetBuiltInMorph(this.graftMeshMorphNamesForGrafting[j]);
						if (builtInMorph != null && builtInMorph.deltas.Length > 0)
						{
							foreach (DAZMorphVertex dazmorphVertex in builtInMorph.deltas)
							{
								Vector3 b = dazmorphVertex.delta * d;
								this._graftMorphedMeshVertices[dazmorphVertex.vertex] += b;
							}
						}
						else
						{
							Debug.LogError("Could not find graft morph " + this.graftMeshMorphNamesForGrafting[j]);
						}
					}
					this._graftMorphedMesh.vertices = this._graftMorphedMeshVertices;
				}
				else
				{
					Debug.LogError("Graft mesh morph names and morph values are not same length");
				}
			}
			Matrix4x4 matrix4x = base.transform.localToWorldMatrix;
			if (Application.isPlaying && this.drawFromBone != null)
			{
				matrix4x *= this.drawFromBone.changeFromOriginalMatrix;
			}
			if (this.simpleMaterial != null)
			{
				for (int l = 0; l < this._graftMorphedMesh.subMeshCount; l++)
				{
					Graphics.DrawMesh(this._graftMorphedMesh, matrix4x, this.simpleMaterial, 0, null, l, null, false, false);
				}
			}
			else
			{
				Debug.LogWarning("Draw Graft Morphed Mesh is enabled but simple material is not set");
			}
		}
	}

	// Token: 0x06004A11 RID: 18961 RVA: 0x001879CB File Offset: 0x00185DCB
	public void ManualUpdate()
	{
		this.Update();
	}

	// Token: 0x06004A12 RID: 18962 RVA: 0x001879D3 File Offset: 0x00185DD3
	private void Update()
	{
		if (!this.staticMesh)
		{
			this.UpdateVertices(!Application.isPlaying);
		}
		this.Draw();
	}

	// Token: 0x06004A13 RID: 18963 RVA: 0x001879F4 File Offset: 0x00185DF4
	public override void Init()
	{
		if (!this._wasInit)
		{
			if (this.targetMesh != null)
			{
				this.targetMesh.Init();
			}
			if (this.graftMesh != null)
			{
				this.graftMesh.Init();
			}
			if (this.graft2Mesh != null)
			{
				this.graft2Mesh.Init();
			}
			base.Init();
		}
	}

	// Token: 0x0400388F RID: 14479
	public DAZMesh targetMesh;

	// Token: 0x04003890 RID: 14480
	public DAZMesh graftMesh;

	// Token: 0x04003891 RID: 14481
	public DAZMesh graft2Mesh;

	// Token: 0x04003892 RID: 14482
	[SerializeField]
	private bool hasGraft2;

	// Token: 0x04003893 RID: 14483
	public bool staticMesh;

	// Token: 0x04003894 RID: 14484
	public DAZMergedMesh copyGraftOptionsFromMesh;

	// Token: 0x04003895 RID: 14485
	public DAZMergedMesh.GraftSymmetryAxis graftSymmetryAxis;

	// Token: 0x04003896 RID: 14486
	public bool useGraftSymmetry;

	// Token: 0x04003897 RID: 14487
	public float graftSymmetryDistance = 0.001f;

	// Token: 0x04003898 RID: 14488
	public bool graftToCenterlineVerts;

	// Token: 0x04003899 RID: 14489
	public DAZMergedMesh.GraftMethod graftMethod = DAZMergedMesh.GraftMethod.Boundary;

	// Token: 0x0400389A RID: 14490
	private bool graftFactorsDirty;

	// Token: 0x0400389B RID: 14491
	[SerializeField]
	private float _graftXFactor = 1f;

	// Token: 0x0400389C RID: 14492
	[SerializeField]
	private float _graftYFactor = 1f;

	// Token: 0x0400389D RID: 14493
	[SerializeField]
	private float _graftZFactor = 1f;

	// Token: 0x0400389E RID: 14494
	public string[] graftMeshMorphNamesForGrafting;

	// Token: 0x0400389F RID: 14495
	public float[] graftMeshMorphValuesForGrafting;

	// Token: 0x040038A0 RID: 14496
	public bool drawGraftMorphedMesh;

	// Token: 0x040038A1 RID: 14497
	[SerializeField]
	private float[] _graftWeights;

	// Token: 0x040038A2 RID: 14498
	[SerializeField]
	private float[] _graft2Weights;

	// Token: 0x040038A3 RID: 14499
	[SerializeField]
	private bool[] _graftIsFreeVert;

	// Token: 0x040038A4 RID: 14500
	[SerializeField]
	private bool[] _graft2IsFreeVert;

	// Token: 0x040038A5 RID: 14501
	[SerializeField]
	private DAZMergedMesh.FreeVertGraftWeight[] _freeVertGraftWeights;

	// Token: 0x040038A6 RID: 14502
	public float freeVertexDistance = 0.1f;

	// Token: 0x040038A7 RID: 14503
	public int numTargetBaseVertices;

	// Token: 0x040038A8 RID: 14504
	public int numGraftBaseVertices;

	// Token: 0x040038A9 RID: 14505
	public int numGraftUVVertices;

	// Token: 0x040038AA RID: 14506
	public int numGraft2BaseVertices;

	// Token: 0x040038AB RID: 14507
	public int numTargetUVVertices;

	// Token: 0x040038AC RID: 14508
	public int startGraftVertIndex;

	// Token: 0x040038AD RID: 14509
	public int startGraft2VertIndex;

	// Token: 0x040038AE RID: 14510
	private Vector3[] _graftMovements;

	// Token: 0x040038AF RID: 14511
	private Vector3[] _graft2Movements;

	// Token: 0x040038B0 RID: 14512
	private Vector3[] _graftMovements2;

	// Token: 0x040038B1 RID: 14513
	private Vector3[] _graft2Movements2;

	// Token: 0x040038B2 RID: 14514
	private Mesh _graftMorphedMesh;

	// Token: 0x040038B3 RID: 14515
	private Vector3[] _graftMorphedMeshVertices;

	// Token: 0x040038B4 RID: 14516
	protected bool isPlaying;

	// Token: 0x040038B5 RID: 14517
	protected Vector3[] _threadedMorphedUVVertices;

	// Token: 0x040038B6 RID: 14518
	protected Vector3[] _threadedVisibleMorphedUVVertices;

	// Token: 0x040038B7 RID: 14519
	protected Vector3[] _threadedMorphedBaseVertices;

	// Token: 0x040038B8 RID: 14520
	protected bool _threadedVerticesChangedThisFrame;

	// Token: 0x040038B9 RID: 14521
	protected bool _threadedVisibleNonPoseVerticesChangedThisFrame;

	// Token: 0x040038BA RID: 14522
	protected bool _targetMeshVerticesChangedThisFrame;

	// Token: 0x040038BB RID: 14523
	protected bool _targetMeshVisibleNonPoseVerticesChangedThisFrame;

	// Token: 0x040038BC RID: 14524
	protected bool _targetMeshVisibleNonPoseVerticesChangedLastFrame;

	// Token: 0x040038BD RID: 14525
	protected bool _graftMeshVerticesChangedThisFrame;

	// Token: 0x040038BE RID: 14526
	protected bool _graftMeshVisibleNonPoseVerticesChangedThisFrame;

	// Token: 0x040038BF RID: 14527
	protected bool _graftMeshVisibleNonPoseVerticesChangedLastFrame;

	// Token: 0x040038C0 RID: 14528
	protected bool _graft2MeshVerticesChangedThisFrame;

	// Token: 0x040038C1 RID: 14529
	protected bool _graft2MeshVisibleNonPoseVerticesChangedThisFrame;

	// Token: 0x040038C2 RID: 14530
	protected bool _graft2MeshVisibleNonPoseVerticesChangedLastFrame;

	// Token: 0x040038C3 RID: 14531
	private float avgxdiff1;

	// Token: 0x040038C4 RID: 14532
	private float avgydiff1;

	// Token: 0x040038C5 RID: 14533
	private float avgzdiff1;

	// Token: 0x040038C6 RID: 14534
	private float avgxdiff2;

	// Token: 0x040038C7 RID: 14535
	private float avgydiff2;

	// Token: 0x040038C8 RID: 14536
	private float avgzdiff2;

	// Token: 0x02000ADC RID: 2780
	public enum GraftMethod
	{
		// Token: 0x040038CA RID: 14538
		Closest,
		// Token: 0x040038CB RID: 14539
		Boundary,
		// Token: 0x040038CC RID: 14540
		ClosestPoly,
		// Token: 0x040038CD RID: 14541
		ClosestVertAndPoly
	}

	// Token: 0x02000ADD RID: 2781
	public enum GraftSymmetryAxis
	{
		// Token: 0x040038CF RID: 14543
		X,
		// Token: 0x040038D0 RID: 14544
		Y,
		// Token: 0x040038D1 RID: 14545
		Z
	}

	// Token: 0x02000ADE RID: 2782
	[Serializable]
	protected class FreeVertGraftWeight
	{
		// Token: 0x06004A14 RID: 18964 RVA: 0x00187A66 File Offset: 0x00185E66
		public FreeVertGraftWeight()
		{
		}

		// Token: 0x040038D2 RID: 14546
		public int freeVert;

		// Token: 0x040038D3 RID: 14547
		public int graftVert;

		// Token: 0x040038D4 RID: 14548
		public float weight;

		// Token: 0x040038D5 RID: 14549
		public int graftPoly;

		// Token: 0x040038D6 RID: 14550
		public float graftVertToPolyRatio;
	}
}
