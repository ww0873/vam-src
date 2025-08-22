using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MeshVR;
using MVR;
using MVR.FileManagement;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000AE4 RID: 2788
[ExecuteInEditMode]
public class DAZMesh : ObjectAllocator, IBinaryStorable, RenderSuspend
{
	// Token: 0x06004A1C RID: 18972 RVA: 0x0017D738 File Offset: 0x0017BB38
	public DAZMesh()
	{
	}

	// Token: 0x17000A62 RID: 2658
	// (get) Token: 0x06004A1D RID: 18973 RVA: 0x0017D7E8 File Offset: 0x0017BBE8
	// (set) Token: 0x06004A1E RID: 18974 RVA: 0x0017D7F0 File Offset: 0x0017BBF0
	public bool drawMorphedBaseMesh
	{
		get
		{
			return this._drawMorphedBaseMesh;
		}
		set
		{
			if (this._drawMorphedBaseMesh != value)
			{
				this._drawMorphedBaseMesh = value;
				if (value)
				{
					this.ApplyMorphs(true);
				}
			}
		}
	}

	// Token: 0x17000A63 RID: 2659
	// (get) Token: 0x06004A1F RID: 18975 RVA: 0x0017D812 File Offset: 0x0017BC12
	// (set) Token: 0x06004A20 RID: 18976 RVA: 0x0017D81A File Offset: 0x0017BC1A
	public bool drawMorphedUVMappedMesh
	{
		get
		{
			return this._drawMorphedUVMappedMesh;
		}
		set
		{
			if (this._drawMorphedUVMappedMesh != value)
			{
				this._drawMorphedUVMappedMesh = value;
				if (value)
				{
					this.ApplyMorphs(true);
				}
			}
		}
	}

	// Token: 0x17000A64 RID: 2660
	// (get) Token: 0x06004A21 RID: 18977 RVA: 0x0017D83C File Offset: 0x0017BC3C
	// (set) Token: 0x06004A22 RID: 18978 RVA: 0x0017D844 File Offset: 0x0017BC44
	public bool renderSuspend
	{
		get
		{
			return this._renderSuspend;
		}
		set
		{
			this._renderSuspend = value;
		}
	}

	// Token: 0x17000A65 RID: 2661
	// (get) Token: 0x06004A23 RID: 18979 RVA: 0x0017D84D File Offset: 0x0017BC4D
	// (set) Token: 0x06004A24 RID: 18980 RVA: 0x0017D855 File Offset: 0x0017BC55
	public bool useSmoothing
	{
		get
		{
			return this._useSmoothing;
		}
		set
		{
			if (this._useSmoothing != value)
			{
				this._useSmoothing = value;
				this.ApplyMorphs(true);
			}
		}
	}

	// Token: 0x06004A25 RID: 18981 RVA: 0x0017D874 File Offset: 0x0017BC74
	public void CopyMaterials()
	{
		if (this.copyMaterialsFrom != null)
		{
			this.materials = new Material[this.copyMaterialsFrom.materials.Length];
			this.materialsPass1 = new Material[this.copyMaterialsFrom.materialsPass1.Length];
			this.materialsPass2 = new Material[this.copyMaterialsFrom.materialsPass2.Length];
			this.materialsEnabled = new bool[this.copyMaterialsFrom.materials.Length];
			this.materialsPass1Enabled = new bool[this.copyMaterialsFrom.materialsPass1.Length];
			this.materialsPass2Enabled = new bool[this.copyMaterialsFrom.materialsPass2.Length];
			this.materialsShadowCastEnabled = new bool[this.copyMaterialsFrom.materials.Length];
			for (int i = 0; i < this.copyMaterialsFrom.materials.Length; i++)
			{
				this.materials[i] = this.copyMaterialsFrom.materials[i];
				this.materialsEnabled[i] = this.copyMaterialsFrom.materialsEnabled[i];
				this.materialsPass2Enabled[i] = this.copyMaterialsFrom.materialsPass2Enabled[i];
				this.materialsShadowCastEnabled[i] = this.copyMaterialsFrom.materialsShadowCastEnabled[i];
			}
			for (int j = 0; j < this.copyMaterialsFrom.materialsPass1.Length; j++)
			{
				this.materialsPass1[j] = this.copyMaterialsFrom.materialsPass1[j];
			}
			for (int k = 0; k < this.copyMaterialsFrom.materialsPass2.Length; k++)
			{
				this.materialsPass2[k] = this.copyMaterialsFrom.materialsPass2[k];
			}
		}
	}

	// Token: 0x17000A66 RID: 2662
	// (get) Token: 0x06004A26 RID: 18982 RVA: 0x0017DA12 File Offset: 0x0017BE12
	public int numBaseVertices
	{
		get
		{
			return this._numBaseVertices;
		}
	}

	// Token: 0x17000A67 RID: 2663
	// (get) Token: 0x06004A27 RID: 18983 RVA: 0x0017DA1A File Offset: 0x0017BE1A
	public int numBasePolygons
	{
		get
		{
			return this._numBasePolygons;
		}
	}

	// Token: 0x17000A68 RID: 2664
	// (get) Token: 0x06004A28 RID: 18984 RVA: 0x0017DA22 File Offset: 0x0017BE22
	public int numUVVertices
	{
		get
		{
			return this._numUVVertices;
		}
	}

	// Token: 0x17000A69 RID: 2665
	// (get) Token: 0x06004A29 RID: 18985 RVA: 0x0017DA2A File Offset: 0x0017BE2A
	public int numMaterials
	{
		get
		{
			return this._numMaterials;
		}
	}

	// Token: 0x17000A6A RID: 2666
	// (get) Token: 0x06004A2A RID: 18986 RVA: 0x0017DA32 File Offset: 0x0017BE32
	public string[] materialNames
	{
		get
		{
			return this._materialNames;
		}
	}

	// Token: 0x17000A6B RID: 2667
	// (get) Token: 0x06004A2B RID: 18987 RVA: 0x0017DA3A File Offset: 0x0017BE3A
	public bool verticesChangedLastFrame
	{
		get
		{
			return this._verticesChangedLastFrame;
		}
	}

	// Token: 0x17000A6C RID: 2668
	// (get) Token: 0x06004A2C RID: 18988 RVA: 0x0017DA42 File Offset: 0x0017BE42
	public bool visibleNonPoseVerticesChangedLastFrame
	{
		get
		{
			return this._visibleNonPoseVerticesChangedLastFrame;
		}
	}

	// Token: 0x17000A6D RID: 2669
	// (get) Token: 0x06004A2D RID: 18989 RVA: 0x0017DA4A File Offset: 0x0017BE4A
	public bool verticesChangedThisFrame
	{
		get
		{
			return this._verticesChangedThisFrame;
		}
	}

	// Token: 0x17000A6E RID: 2670
	// (get) Token: 0x06004A2E RID: 18990 RVA: 0x0017DA52 File Offset: 0x0017BE52
	public bool visibleNonPoseVerticesChangedThisFrame
	{
		get
		{
			return this._visibleNonPoseVerticesChangedThisFrame;
		}
	}

	// Token: 0x17000A6F RID: 2671
	// (get) Token: 0x06004A2F RID: 18991 RVA: 0x0017DA5A File Offset: 0x0017BE5A
	public bool normalsDirtyThisFrame
	{
		get
		{
			return this._normalsDirtyThisFrame;
		}
	}

	// Token: 0x17000A70 RID: 2672
	// (get) Token: 0x06004A30 RID: 18992 RVA: 0x0017DA62 File Offset: 0x0017BE62
	public bool tangentsDirtyThisFrame
	{
		get
		{
			return this._tangentsDirtyThisFrame;
		}
	}

	// Token: 0x17000A71 RID: 2673
	// (get) Token: 0x06004A31 RID: 18993 RVA: 0x0017DA6A File Offset: 0x0017BE6A
	public Mesh baseMesh
	{
		get
		{
			if (this._baseMesh == null)
			{
				this.Init();
			}
			return this._baseMesh;
		}
	}

	// Token: 0x17000A72 RID: 2674
	// (get) Token: 0x06004A32 RID: 18994 RVA: 0x0017DA89 File Offset: 0x0017BE89
	// (set) Token: 0x06004A33 RID: 18995 RVA: 0x0017DA94 File Offset: 0x0017BE94
	public DAZMeshData meshData
	{
		get
		{
			return this._meshData;
		}
		set
		{
			if (this._meshData != value)
			{
				if (value == null)
				{
					this._baseVertices = (Vector3[])this._meshData.baseVertices.Clone();
					this._basePolyList = (MeshPoly[])this._meshData.basePolyList.Clone();
					this._baseVerticesToUVVertices = (DAZVertexMap[])this._meshData.baseVerticesToUVVertices.Clone();
					this._UVVertices = (Vector3[])this._meshData.UVVertices.Clone();
					this._UVPolyList = (MeshPoly[])this._meshData.UVPolyList.Clone();
					this._OrigUV = (Vector2[])this._meshData.OrigUV.Clone();
					this._meshData = null;
				}
				else
				{
					this._meshData = value;
					this._baseVertices = null;
					this._basePolyList = null;
					this._baseVerticesToUVVertices = null;
					this._UVVertices = null;
					this._UVPolyList = null;
					this._OrigUV = null;
				}
			}
		}
	}

	// Token: 0x17000A73 RID: 2675
	// (get) Token: 0x06004A34 RID: 18996 RVA: 0x0017DB9D File Offset: 0x0017BF9D
	public Vector3[] baseVertices
	{
		get
		{
			if (this._meshData != null)
			{
				return this._meshData.baseVertices;
			}
			return this._baseVertices;
		}
	}

	// Token: 0x17000A74 RID: 2676
	// (get) Token: 0x06004A35 RID: 18997 RVA: 0x0017DBC2 File Offset: 0x0017BFC2
	public Vector3[] baseNormals
	{
		get
		{
			return this._baseNormals;
		}
	}

	// Token: 0x17000A75 RID: 2677
	// (get) Token: 0x06004A36 RID: 18998 RVA: 0x0017DBCA File Offset: 0x0017BFCA
	public Vector3[] baseSurfaceNormals
	{
		get
		{
			return this._baseSurfaceNormals;
		}
	}

	// Token: 0x17000A76 RID: 2678
	// (get) Token: 0x06004A37 RID: 18999 RVA: 0x0017DBD2 File Offset: 0x0017BFD2
	public MeshPoly[] basePolyList
	{
		get
		{
			if (this._meshData != null)
			{
				return this._meshData.basePolyList;
			}
			return this._basePolyList;
		}
	}

	// Token: 0x17000A77 RID: 2679
	// (get) Token: 0x06004A38 RID: 19000 RVA: 0x0017DBF7 File Offset: 0x0017BFF7
	public int[] baseTriangles
	{
		get
		{
			return this._baseTriangles;
		}
	}

	// Token: 0x17000A78 RID: 2680
	// (get) Token: 0x06004A39 RID: 19001 RVA: 0x0017DBFF File Offset: 0x0017BFFF
	public int[][] baseMaterialVertices
	{
		get
		{
			return this._baseMaterialVertices;
		}
	}

	// Token: 0x17000A79 RID: 2681
	// (get) Token: 0x06004A3A RID: 19002 RVA: 0x0017DC07 File Offset: 0x0017C007
	public Vector3[] morphedBaseVertices
	{
		get
		{
			return this._morphedBaseVertices;
		}
	}

	// Token: 0x17000A7A RID: 2682
	// (get) Token: 0x06004A3B RID: 19003 RVA: 0x0017DC0F File Offset: 0x0017C00F
	public DAZVertexMap[] baseVerticesToUVVertices
	{
		get
		{
			if (this._meshData != null)
			{
				return this._meshData.baseVerticesToUVVertices;
			}
			return this._baseVerticesToUVVertices;
		}
	}

	// Token: 0x17000A7B RID: 2683
	// (get) Token: 0x06004A3C RID: 19004 RVA: 0x0017DC34 File Offset: 0x0017C034
	public Dictionary<int, List<int>> baseVertToUVVertFullMap
	{
		get
		{
			Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
			for (int i = 0; i < this._numBaseVertices; i++)
			{
				dictionary.Add(i, new List<int>
				{
					i
				});
			}
			DAZVertexMap[] baseVerticesToUVVertices = this.baseVerticesToUVVertices;
			for (int j = 0; j < baseVerticesToUVVertices.Length; j++)
			{
				int fromvert = baseVerticesToUVVertices[j].fromvert;
				int tovert = baseVerticesToUVVertices[j].tovert;
				List<int> list;
				if (dictionary.TryGetValue(fromvert, out list))
				{
					list.Add(tovert);
				}
			}
			return dictionary;
		}
	}

	// Token: 0x17000A7C RID: 2684
	// (get) Token: 0x06004A3D RID: 19005 RVA: 0x0017DCC4 File Offset: 0x0017C0C4
	public Dictionary<int, int> uvVertToBaseVert
	{
		get
		{
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			DAZVertexMap[] baseVerticesToUVVertices = this.baseVerticesToUVVertices;
			for (int i = 0; i < baseVerticesToUVVertices.Length; i++)
			{
				int fromvert = baseVerticesToUVVertices[i].fromvert;
				int tovert = baseVerticesToUVVertices[i].tovert;
				if (!dictionary.ContainsKey(tovert))
				{
					dictionary.Add(tovert, fromvert);
				}
			}
			return dictionary;
		}
	}

	// Token: 0x17000A7D RID: 2685
	// (get) Token: 0x06004A3E RID: 19006 RVA: 0x0017DD1C File Offset: 0x0017C11C
	public Mesh uvMappedMesh
	{
		get
		{
			return this._uvMappedMesh;
		}
	}

	// Token: 0x17000A7E RID: 2686
	// (get) Token: 0x06004A3F RID: 19007 RVA: 0x0017DD24 File Offset: 0x0017C124
	public Vector3[] UVVertices
	{
		get
		{
			if (this._meshData != null)
			{
				return this._meshData.UVVertices;
			}
			return this._UVVertices;
		}
	}

	// Token: 0x17000A7F RID: 2687
	// (get) Token: 0x06004A40 RID: 19008 RVA: 0x0017DD49 File Offset: 0x0017C149
	public Vector3[] UVNormals
	{
		get
		{
			return this._UVNormals;
		}
	}

	// Token: 0x17000A80 RID: 2688
	// (get) Token: 0x06004A41 RID: 19009 RVA: 0x0017DD51 File Offset: 0x0017C151
	public Vector4[] UVTangents
	{
		get
		{
			return this._UVTangents;
		}
	}

	// Token: 0x17000A81 RID: 2689
	// (get) Token: 0x06004A42 RID: 19010 RVA: 0x0017DD59 File Offset: 0x0017C159
	public MeshPoly[] UVPolyList
	{
		get
		{
			if (this._meshData != null)
			{
				return this._meshData.UVPolyList;
			}
			return this._UVPolyList;
		}
	}

	// Token: 0x17000A82 RID: 2690
	// (get) Token: 0x06004A43 RID: 19011 RVA: 0x0017DD7E File Offset: 0x0017C17E
	public int[] UVTriangles
	{
		get
		{
			return this._UVTriangles;
		}
	}

	// Token: 0x17000A83 RID: 2691
	// (get) Token: 0x06004A44 RID: 19012 RVA: 0x0017DD86 File Offset: 0x0017C186
	public Vector2[] UV
	{
		get
		{
			return this._UV;
		}
	}

	// Token: 0x17000A84 RID: 2692
	// (get) Token: 0x06004A45 RID: 19013 RVA: 0x0017DD8E File Offset: 0x0017C18E
	public Vector2[] OrigUV
	{
		get
		{
			if (this._meshData != null)
			{
				return this._meshData.OrigUV;
			}
			return this._OrigUV;
		}
	}

	// Token: 0x17000A85 RID: 2693
	// (get) Token: 0x06004A46 RID: 19014 RVA: 0x0017DDB3 File Offset: 0x0017C1B3
	// (set) Token: 0x06004A47 RID: 19015 RVA: 0x0017DDBB File Offset: 0x0017C1BB
	public bool usePatches
	{
		get
		{
			return this._usePatches;
		}
		set
		{
			if (this._usePatches != value)
			{
				this._usePatches = value;
				this.ApplyUVPatches();
				this.RecalculateMorphedMeshTangents(true);
			}
		}
	}

	// Token: 0x06004A48 RID: 19016 RVA: 0x0017DDE0 File Offset: 0x0017C1E0
	public void ApplyUVPatches()
	{
		this._UV = new Vector2[this._numUVVertices];
		Vector2[] origUV = this.OrigUV;
		for (int i = 0; i < this._numUVVertices; i++)
		{
			this._UV[i] = origUV[i];
		}
		if (this._usePatches)
		{
			for (int j = 0; j < this.numUVPatches; j++)
			{
				int vertexNum = this.UVPatches[j].vertexNum;
				if (vertexNum >= 0 && vertexNum < this._numUVVertices)
				{
					this._UV[vertexNum] = this.UVPatches[j].uv;
				}
			}
		}
		this._uvMappedMesh.uv = this._UV;
		this._morphedUVMappedMesh.uv = this._UV;
	}

	// Token: 0x06004A49 RID: 19017 RVA: 0x0017DEBC File Offset: 0x0017C2BC
	protected void InitMeshSmooth()
	{
		if (this.meshSmooth == null && this.baseVertices != null && this.basePolyList != null && this.baseVerticesToUVVertices != null)
		{
			this.meshSmooth = new MeshSmooth(this.baseVertices, this.basePolyList);
		}
	}

	// Token: 0x17000A86 RID: 2694
	// (get) Token: 0x06004A4A RID: 19018 RVA: 0x0017DF0C File Offset: 0x0017C30C
	public Mesh morphedUVMappedMesh
	{
		get
		{
			return this._morphedUVMappedMesh;
		}
	}

	// Token: 0x17000A87 RID: 2695
	// (get) Token: 0x06004A4B RID: 19019 RVA: 0x0017DF14 File Offset: 0x0017C314
	// (set) Token: 0x06004A4C RID: 19020 RVA: 0x0017DF1C File Offset: 0x0017C31C
	public bool drawVisibleMorphedUVMappedMesh
	{
		get
		{
			return this._drawVisibleMorphedUVMappedMesh;
		}
		set
		{
			if (this._drawVisibleMorphedUVMappedMesh != value)
			{
				this._drawVisibleMorphedUVMappedMesh = value;
				if (value)
				{
					this.ApplyMorphs(true);
				}
			}
		}
	}

	// Token: 0x17000A88 RID: 2696
	// (get) Token: 0x06004A4D RID: 19021 RVA: 0x0017DF3E File Offset: 0x0017C33E
	public Vector3[] visibleMorphedUVVertices
	{
		get
		{
			return this._visibleMorphedUVVertices;
		}
	}

	// Token: 0x17000A89 RID: 2697
	// (get) Token: 0x06004A4E RID: 19022 RVA: 0x0017DF46 File Offset: 0x0017C346
	public Vector3[] rawMorphedUVVertices
	{
		get
		{
			return this._morphedUVVertices;
		}
	}

	// Token: 0x17000A8A RID: 2698
	// (get) Token: 0x06004A4F RID: 19023 RVA: 0x0017DF4E File Offset: 0x0017C34E
	public Vector3[] morphedUVVertices
	{
		get
		{
			if (this.useSmoothing)
			{
				return this._smoothedMorphedUVVertices;
			}
			return this._morphedUVVertices;
		}
	}

	// Token: 0x17000A8B RID: 2699
	// (get) Token: 0x06004A50 RID: 19024 RVA: 0x0017DF68 File Offset: 0x0017C368
	// (set) Token: 0x06004A51 RID: 19025 RVA: 0x0017DF70 File Offset: 0x0017C370
	public bool[] morphedBaseDirtyVertices
	{
		get
		{
			return this._morphedBaseDirtyVertices;
		}
		set
		{
			this._morphedBaseDirtyVertices = value;
		}
	}

	// Token: 0x17000A8C RID: 2700
	// (get) Token: 0x06004A52 RID: 19026 RVA: 0x0017DF79 File Offset: 0x0017C379
	// (set) Token: 0x06004A53 RID: 19027 RVA: 0x0017DF81 File Offset: 0x0017C381
	public bool[] morphedUVDirtyVertices
	{
		get
		{
			return this._morphedUVDirtyVertices;
		}
		set
		{
			this._morphedUVDirtyVertices = value;
		}
	}

	// Token: 0x17000A8D RID: 2701
	// (get) Token: 0x06004A54 RID: 19028 RVA: 0x0017DF8A File Offset: 0x0017C38A
	public Vector3[] morphedUVNormals
	{
		get
		{
			return this._morphedUVNormals;
		}
	}

	// Token: 0x17000A8E RID: 2702
	// (get) Token: 0x06004A55 RID: 19029 RVA: 0x0017DF92 File Offset: 0x0017C392
	public Vector4[] morphedUVTangents
	{
		get
		{
			return this._morphedUVTangents;
		}
	}

	// Token: 0x17000A8F RID: 2703
	// (get) Token: 0x06004A56 RID: 19030 RVA: 0x0017DF9A File Offset: 0x0017C39A
	// (set) Token: 0x06004A57 RID: 19031 RVA: 0x0017DFA2 File Offset: 0x0017C3A2
	public bool createMeshFilterAndRenderer
	{
		get
		{
			return this._createMeshFilterAndRenderer;
		}
		set
		{
			if (this._createMeshFilterAndRenderer != value)
			{
				this._createMeshFilterAndRenderer = value;
				this.InitMeshFilterAndRenderer();
			}
		}
	}

	// Token: 0x17000A90 RID: 2704
	// (get) Token: 0x06004A58 RID: 19032 RVA: 0x0017DFBD File Offset: 0x0017C3BD
	// (set) Token: 0x06004A59 RID: 19033 RVA: 0x0017DFC5 File Offset: 0x0017C3C5
	public bool createMeshCollider
	{
		get
		{
			return this._createMeshCollider;
		}
		set
		{
			if (this._createMeshCollider != value)
			{
				this._createMeshCollider = value;
				this.InitCollider();
			}
		}
	}

	// Token: 0x17000A91 RID: 2705
	// (get) Token: 0x06004A5A RID: 19034 RVA: 0x0017DFE0 File Offset: 0x0017C3E0
	// (set) Token: 0x06004A5B RID: 19035 RVA: 0x0017DFE8 File Offset: 0x0017C3E8
	public bool useConvexCollider
	{
		get
		{
			return this._useConvexCollider;
		}
		set
		{
			if (this._useConvexCollider != value)
			{
				this._useConvexCollider = value;
				this.InitCollider();
			}
		}
	}

	// Token: 0x06004A5C RID: 19036 RVA: 0x0017E004 File Offset: 0x0017C404
	private void InitCollider()
	{
		MeshCollider meshCollider = base.gameObject.GetComponent<MeshCollider>();
		if (this._createMeshCollider)
		{
			if (meshCollider == null)
			{
				meshCollider = base.gameObject.AddComponent<MeshCollider>();
			}
			meshCollider.sharedMesh = this.morphedUVMappedMesh;
			meshCollider.convex = this.useConvexCollider;
			meshCollider.cookingOptions = (MeshColliderCookingOptions.InflateConvexMesh | MeshColliderCookingOptions.CookForFasterSimulation | MeshColliderCookingOptions.EnableMeshCleaning | MeshColliderCookingOptions.WeldColocatedVertices);
			meshCollider.skinWidth = 0.0001f;
		}
	}

	// Token: 0x06004A5D RID: 19037 RVA: 0x0017E06C File Offset: 0x0017C46C
	protected void PolyListToTriangleIndexes(MeshPoly[] polylist, List<List<int>> indexes, List<HashSet<int>> materialVertices = null)
	{
		Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
		int num = -1;
		foreach (MeshPoly meshPoly in polylist)
		{
			int materialNum = meshPoly.materialNum;
			if (this.useUnityMaterialOrdering)
			{
				bool flag;
				if (!dictionary.TryGetValue(materialNum, out flag))
				{
					num++;
					dictionary.Add(materialNum, true);
				}
			}
			else
			{
				num = materialNum;
			}
			int item = meshPoly.vertices[0];
			int item2 = meshPoly.vertices[1];
			int item3 = meshPoly.vertices[2];
			indexes[num].Add(item3);
			indexes[num].Add(item2);
			indexes[num].Add(item);
			if (materialVertices != null)
			{
				materialVertices[num].Add(item);
				materialVertices[num].Add(item2);
				materialVertices[num].Add(item3);
			}
			if (meshPoly.vertices.Length == 4)
			{
				int item4 = meshPoly.vertices[3];
				indexes[num].Add(item);
				indexes[num].Add(item4);
				indexes[num].Add(item3);
				if (materialVertices != null)
				{
					materialVertices[num].Add(item4);
				}
			}
		}
	}

	// Token: 0x06004A5E RID: 19038 RVA: 0x0017E1AC File Offset: 0x0017C5AC
	public bool LoadFromBinaryReader(BinaryReader binReader)
	{
		try
		{
			string a = binReader.ReadString();
			if (a != "DAZMesh")
			{
				SuperController.LogError("Binary file corrupted. Tried to read DAZMesh in wrong section");
				return false;
			}
			string text = binReader.ReadString();
			if (text != "1.0")
			{
				SuperController.LogError("DAZMesh schema " + text + " is not compatible with this version of software");
				return false;
			}
			this.nodeId = binReader.ReadString();
			this.sceneNodeId = binReader.ReadString();
			this.geometryId = binReader.ReadString();
			this.sceneGeometryId = binReader.ReadString();
			Shader shader = Shader.Find(this.shaderNameForDynamicLoad);
			if (shader == null)
			{
				SuperController.LogError("Could not find shader " + this.shaderNameForDynamicLoad + ". Can't load DAZMesh from binary file");
				return false;
			}
			this.meshSmooth = null;
			this._meshData = null;
			this._numBaseVertices = binReader.ReadInt32();
			this._baseVertices = new Vector3[this._numBaseVertices];
			for (int i = 0; i < this._numBaseVertices; i++)
			{
				Vector3 vector;
				vector.x = binReader.ReadSingle();
				vector.y = binReader.ReadSingle();
				vector.z = binReader.ReadSingle();
				this._baseVertices[i] = vector;
			}
			this._numMaterials = binReader.ReadInt32();
			this._materialNames = new string[this._numMaterials];
			this.materials = new Material[this._numMaterials];
			this.materialsEnabled = new bool[this._numMaterials];
			this.materialsShadowCastEnabled = new bool[this._numMaterials];
			for (int j = 0; j < this._numMaterials; j++)
			{
				this._materialNames[j] = binReader.ReadString();
				this.materialsEnabled[j] = true;
				this.materialsShadowCastEnabled[j] = true;
				if (shader != null)
				{
					this.materials[j] = new Material(shader);
					base.RegisterAllocatedObject(this.materials[j]);
					this.materials[j].name = this._materialNames[j];
				}
			}
			this._numBasePolygons = binReader.ReadInt32();
			this._basePolyList = new MeshPoly[this._numBasePolygons];
			this._UVPolyList = new MeshPoly[this._numBasePolygons];
			for (int k = 0; k < this._numBasePolygons; k++)
			{
				MeshPoly meshPoly = new MeshPoly();
				meshPoly.materialNum = binReader.ReadInt32();
				int num = binReader.ReadInt32();
				meshPoly.vertices = new int[num];
				for (int l = 0; l < num; l++)
				{
					meshPoly.vertices[l] = binReader.ReadInt32();
				}
				this._basePolyList[k] = meshPoly;
			}
			for (int m = 0; m < this._numBasePolygons; m++)
			{
				MeshPoly meshPoly2 = new MeshPoly();
				meshPoly2.materialNum = binReader.ReadInt32();
				int num2 = binReader.ReadInt32();
				meshPoly2.vertices = new int[num2];
				for (int n = 0; n < num2; n++)
				{
					meshPoly2.vertices[n] = binReader.ReadInt32();
				}
				this._UVPolyList[m] = meshPoly2;
			}
			this._numUVVertices = binReader.ReadInt32();
			this._UVVertices = new Vector3[this._numUVVertices];
			for (int num3 = 0; num3 < this._baseVertices.Length; num3++)
			{
				this._UVVertices[num3] = this._baseVertices[num3];
			}
			this._OrigUV = new Vector2[this._numUVVertices];
			for (int num4 = 0; num4 < this._numUVVertices; num4++)
			{
				Vector2 vector2;
				vector2.x = binReader.ReadSingle();
				vector2.y = binReader.ReadSingle();
				this._OrigUV[num4] = vector2;
			}
			int num5 = binReader.ReadInt32();
			this._baseVerticesToUVVertices = new DAZVertexMap[num5];
			for (int num6 = 0; num6 < num5; num6++)
			{
				DAZVertexMap dazvertexMap = new DAZVertexMap();
				dazvertexMap.fromvert = binReader.ReadInt32();
				dazvertexMap.tovert = binReader.ReadInt32();
				dazvertexMap.polyindex = binReader.ReadInt32();
				this._baseVerticesToUVVertices[num6] = dazvertexMap;
			}
			foreach (DAZVertexMap dazvertexMap2 in this._baseVerticesToUVVertices)
			{
				this._UVVertices[dazvertexMap2.tovert] = this._UVVertices[dazvertexMap2.fromvert];
			}
		}
		catch (Exception arg)
		{
			SuperController.LogError("Error while loading DAZMesh from binary reader " + arg);
			return false;
		}
		this.DeriveMeshes();
		return true;
	}

	// Token: 0x06004A5F RID: 19039 RVA: 0x0017E6A0 File Offset: 0x0017CAA0
	public bool LoadFromBinaryFile(string path)
	{
		bool result = false;
		try
		{
			using (FileEntryStream fileEntryStream = FileManager.OpenStream(path, true))
			{
				using (BinaryReader binaryReader = new BinaryReader(fileEntryStream.Stream))
				{
					result = this.LoadFromBinaryReader(binaryReader);
				}
			}
		}
		catch (Exception ex)
		{
			SuperController.LogError(string.Concat(new object[]
			{
				"Error while loading DAZMesh from binary file ",
				path,
				" ",
				ex
			}));
		}
		return result;
	}

	// Token: 0x06004A60 RID: 19040 RVA: 0x0017E74C File Offset: 0x0017CB4C
	public bool StoreToBinaryWriter(BinaryWriter binWriter)
	{
		try
		{
			binWriter.Write("DAZMesh");
			binWriter.Write("1.0");
			binWriter.Write(this.nodeId);
			binWriter.Write(this.sceneNodeId);
			binWriter.Write(this.geometryId);
			binWriter.Write(this.sceneGeometryId);
			binWriter.Write(this._numBaseVertices);
			foreach (Vector3 vector in this.baseVertices)
			{
				binWriter.Write(vector.x);
				binWriter.Write(vector.y);
				binWriter.Write(vector.z);
			}
			binWriter.Write(this._numMaterials);
			foreach (string value in this._materialNames)
			{
				binWriter.Write(value);
			}
			binWriter.Write(this._numBasePolygons);
			foreach (MeshPoly meshPoly in this.basePolyList)
			{
				binWriter.Write(meshPoly.materialNum);
				binWriter.Write(meshPoly.vertices.Length);
				foreach (int value2 in meshPoly.vertices)
				{
					binWriter.Write(value2);
				}
			}
			foreach (MeshPoly meshPoly2 in this.UVPolyList)
			{
				binWriter.Write(meshPoly2.materialNum);
				binWriter.Write(meshPoly2.vertices.Length);
				foreach (int value3 in meshPoly2.vertices)
				{
					binWriter.Write(value3);
				}
			}
			binWriter.Write(this._numUVVertices);
			foreach (Vector2 vector2 in this._OrigUV)
			{
				binWriter.Write(vector2.x);
				binWriter.Write(vector2.y);
			}
			binWriter.Write(this._baseVerticesToUVVertices.Length);
			foreach (DAZVertexMap dazvertexMap in this._baseVerticesToUVVertices)
			{
				binWriter.Write(dazvertexMap.fromvert);
				binWriter.Write(dazvertexMap.tovert);
				binWriter.Write(dazvertexMap.polyindex);
			}
		}
		catch (Exception arg)
		{
			SuperController.LogError("Error while storing DAZMesh to binary reader " + arg);
			return false;
		}
		return true;
	}

	// Token: 0x06004A61 RID: 19041 RVA: 0x0017EA20 File Offset: 0x0017CE20
	public bool StoreToBinaryFile(string path)
	{
		bool result = false;
		try
		{
			FileManager.AssertNotCalledFromPlugin();
			using (FileStream fileStream = FileManager.OpenStreamForCreate(path))
			{
				using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
				{
					result = this.StoreToBinaryWriter(binaryWriter);
				}
			}
		}
		catch (Exception ex)
		{
			SuperController.LogError(string.Concat(new object[]
			{
				"Error while storing DAZMesh to binary file ",
				path,
				" ",
				ex
			}));
		}
		return result;
	}

	// Token: 0x06004A62 RID: 19042 RVA: 0x0017EACC File Offset: 0x0017CECC
	public virtual void DeriveMeshes()
	{
		bool flag = false;
		if (this.baseVertices.Length > 65000 || this.UVVertices.Length > 65000)
		{
			flag = true;
		}
		this._baseMesh = new Mesh();
		base.RegisterAllocatedObject(this._baseMesh);
		this._morphedBaseMesh = new Mesh();
		base.RegisterAllocatedObject(this._morphedBaseMesh);
		this._uvMappedMesh = new Mesh();
		base.RegisterAllocatedObject(this._uvMappedMesh);
		if (this._morphedUVMappedMesh == null || !Application.isPlaying)
		{
			this._morphedUVMappedMesh = new Mesh();
			base.RegisterAllocatedObject(this._morphedUVMappedMesh);
		}
		this._visibleMorphedUVMappedMesh = new Mesh();
		base.RegisterAllocatedObject(this._visibleMorphedUVMappedMesh);
		if (flag)
		{
			this._baseMesh.indexFormat = IndexFormat.UInt32;
			this._morphedBaseMesh.indexFormat = IndexFormat.UInt32;
			this._uvMappedMesh.indexFormat = IndexFormat.UInt32;
			this._morphedUVMappedMesh.indexFormat = IndexFormat.UInt32;
			this._visibleMorphedUVMappedMesh.indexFormat = IndexFormat.UInt32;
		}
		this._morphedBaseVertices = (Vector3[])this.baseVertices.Clone();
		this._morphedUVVertices = (Vector3[])this.UVVertices.Clone();
		this._visibleMorphedUVVertices = (Vector3[])this.UVVertices.Clone();
		this._smoothedMorphedUVVertices = (Vector3[])this._morphedUVVertices.Clone();
		this._baseMesh.vertices = this.baseVertices;
		this._morphedBaseMesh.vertices = this.baseVertices;
		this._uvMappedMesh.vertices = this.UVVertices;
		this._morphedUVMappedMesh.vertices = this._morphedUVVertices;
		this._visibleMorphedUVMappedMesh.vertices = this._visibleMorphedUVVertices;
		List<List<int>> list = new List<List<int>>();
		List<HashSet<int>> list2 = new List<HashSet<int>>();
		List<List<int>> list3 = new List<List<int>>();
		list.Capacity = this._numMaterials;
		list2.Capacity = this._numMaterials;
		list3.Capacity = this._numMaterials;
		for (int i = 0; i < this._numMaterials; i++)
		{
			list.Add(new List<int>());
			list2.Add(new HashSet<int>());
			list3.Add(new List<int>());
		}
		this._baseMesh.subMeshCount = this._numMaterials;
		this._morphedBaseMesh.subMeshCount = this._numMaterials;
		this._uvMappedMesh.subMeshCount = this._numMaterials;
		this._morphedUVMappedMesh.subMeshCount = this._numMaterials;
		this._visibleMorphedUVMappedMesh.subMeshCount = this._numMaterials;
		this.PolyListToTriangleIndexes(this.basePolyList, list, list2);
		this.PolyListToTriangleIndexes(this.UVPolyList, list3, null);
		this._baseMaterialVertices = new int[this._numMaterials][];
		for (int j = 0; j < this._numMaterials; j++)
		{
			this._baseMaterialVertices[j] = new int[list2[j].Count];
			list2[j].CopyTo(this._baseMaterialVertices[j]);
			int[] indices = list[j].ToArray();
			this._baseMesh.SetIndices(indices, MeshTopology.Triangles, j);
			this._morphedBaseMesh.SetIndices(indices, MeshTopology.Triangles, j);
			int[] indices2 = list3[j].ToArray();
			this._uvMappedMesh.SetIndices(indices2, MeshTopology.Triangles, j);
			this._morphedUVMappedMesh.SetIndices(indices2, MeshTopology.Triangles, j);
			this._visibleMorphedUVMappedMesh.SetIndices(indices2, MeshTopology.Triangles, j);
		}
		this._baseTriangles = this._baseMesh.triangles;
		this._UVTriangles = this._uvMappedMesh.triangles;
		this._baseMesh.RecalculateBounds();
		this._morphedBaseMesh.bounds = this._baseMesh.bounds;
		this._uvMappedMesh.bounds = this._baseMesh.bounds;
		this._morphedUVMappedMesh.bounds = this._baseMesh.bounds;
		this._visibleMorphedUVMappedMesh.bounds = this._baseMesh.bounds;
		this.ApplyUVPatches();
		this._baseNormals = new Vector3[this._numBaseVertices];
		this._baseSurfaceNormals = new Vector3[this._baseTriangles.Length / 3];
		RecalculateNormals.recalculateNormals(this._baseTriangles, this.baseVertices, this._baseNormals, this._baseSurfaceNormals, null, null, false, true);
		this._morphedBaseSurfaceNormals = (Vector3[])this._baseSurfaceNormals.Clone();
		this._morphedBaseNormals = (Vector3[])this._baseNormals.Clone();
		this._morphedUVNormals = new Vector3[this._numUVVertices];
		for (int k = 0; k < this._morphedBaseNormals.Length; k++)
		{
			this._morphedUVNormals[k] = this._morphedBaseNormals[k];
		}
		this._updateDuplicateMorphedUVNormals();
		this._UVNormals = (Vector3[])this._morphedUVNormals.Clone();
		this._baseMesh.normals = this._baseNormals;
		this._morphedBaseMesh.normals = this._morphedBaseNormals;
		this._uvMappedMesh.normals = this._UVNormals;
		this._morphedUVMappedMesh.normals = this._morphedUVNormals;
		this._visibleMorphedUVMappedMesh.normals = this._morphedUVNormals;
		this.ResetMorphedVerticesToOffset();
		this._UVTangents = new Vector4[this._numUVVertices];
		RecalculateTangents.recalculateTangentsFast(this._UVTriangles, this.UVVertices, this._UVNormals, this.UV, this._UVTangents, null, null, false);
		this._morphedUVTangents = (Vector4[])this._UVTangents.Clone();
		this._uvMappedMesh.tangents = this._UVTangents;
		this._morphedUVMappedMesh.tangents = this._morphedUVTangents;
		this._visibleMorphedUVMappedMesh.tangents = this._morphedUVTangents;
		this._morphedBaseDirtyVertices = new bool[this._morphedUVNormals.Length];
		this._morphedUVDirtyVertices = new bool[this._morphedUVNormals.Length];
	}

	// Token: 0x06004A63 RID: 19043 RVA: 0x0017F09C File Offset: 0x0017D49C
	public void RecalculateMorphedMeshNormals(bool forceAll = false)
	{
		if (this.useUnityRecalcNormals)
		{
			this._morphedBaseMesh.vertices = this._morphedBaseVertices;
			this._morphedBaseMesh.RecalculateNormals();
			this._morphedBaseNormals = this._morphedBaseMesh.normals;
		}
		else if (this._baseTriangles != null)
		{
			if (forceAll)
			{
				RecalculateNormals.recalculateNormals(this._baseTriangles, this.morphedUVVertices, this._morphedBaseNormals, this._morphedBaseSurfaceNormals, null, null, false, true);
			}
			else
			{
				RecalculateNormals.recalculateNormals(this._baseTriangles, this.morphedUVVertices, this._morphedBaseNormals, this._morphedBaseSurfaceNormals, this.morphedBaseDirtyVertices, null, false, true);
			}
			if (this._drawMorphedBaseMesh)
			{
				this._morphedBaseMesh.normals = this._morphedBaseNormals;
			}
		}
		for (int i = 0; i < this._morphedBaseNormals.Length; i++)
		{
			this._morphedUVNormals[i] = this._morphedBaseNormals[i];
		}
		this._updateDuplicateMorphedUVNormals();
		if (this._drawMorphedUVMappedMesh || !Application.isPlaying)
		{
			this._morphedUVMappedMesh.normals = this._morphedUVNormals;
		}
		if (this._drawMorphedUVMappedMesh || !Application.isPlaying)
		{
			this._visibleMorphedUVMappedMesh.normals = this._morphedUVNormals;
		}
	}

	// Token: 0x06004A64 RID: 19044 RVA: 0x0017F1EC File Offset: 0x0017D5EC
	public void RecalculateMorphedMeshTangentsAccurate()
	{
		Vector3[] array = null;
		Vector3[] array2 = null;
		RecalculateTangents.recalculateTangentsAccurate(this._UVTriangles, this.morphedUVVertices, this._morphedUVNormals, this.UV, ref array, ref array2, this._morphedUVTangents);
		if (this._drawMorphedUVMappedMesh || !Application.isPlaying)
		{
			this._morphedUVMappedMesh.tangents = this._morphedUVTangents;
		}
		if (this._drawVisibleMorphedUVMappedMesh || !Application.isPlaying)
		{
			this._visibleMorphedUVMappedMesh.tangents = this._morphedUVTangents;
		}
	}

	// Token: 0x06004A65 RID: 19045 RVA: 0x0017F270 File Offset: 0x0017D670
	public void RecalculateMorphedMeshTangents(bool forceAll = false)
	{
		if (forceAll)
		{
			RecalculateTangents.recalculateTangentsFast(this._UVTriangles, this.morphedUVVertices, this._morphedUVNormals, this.UV, this._morphedUVTangents, null, null, false);
		}
		else
		{
			RecalculateTangents.recalculateTangentsFast(this._UVTriangles, this.morphedUVVertices, this._morphedUVNormals, this.UV, this._morphedUVTangents, this.morphedUVDirtyVertices, null, false);
		}
		if (this._drawMorphedUVMappedMesh || !Application.isPlaying)
		{
			this._morphedUVMappedMesh.tangents = this._morphedUVTangents;
		}
		if (this._drawVisibleMorphedUVMappedMesh || !Application.isPlaying)
		{
			this._visibleMorphedUVMappedMesh.tangents = this._morphedUVTangents;
		}
	}

	// Token: 0x06004A66 RID: 19046 RVA: 0x0017F328 File Offset: 0x0017D728
	protected void _triggerNormalAndTangentRecalc()
	{
		if (this.recalcNormalsOnMorph && this.morphedNormalsDirty)
		{
			this.RecalculateMorphedMeshNormals(false);
			this.morphedNormalsDirty = false;
			this._normalsDirtyThisFrame = true;
		}
		if (this.recalcTangentsOnMorph && this.morphedTangentsDirty)
		{
			foreach (DAZVertexMap dazvertexMap in this.baseVerticesToUVVertices)
			{
				this.morphedUVDirtyVertices[dazvertexMap.tovert] = this.morphedUVDirtyVertices[dazvertexMap.fromvert];
			}
			this.RecalculateMorphedMeshTangents(false);
			this.morphedTangentsDirty = false;
			this._tangentsDirtyThisFrame = true;
		}
	}

	// Token: 0x06004A67 RID: 19047 RVA: 0x0017F3C4 File Offset: 0x0017D7C4
	public void Import(JSONNode jsonGeometry, DAZUVMap uvmap, Dictionary<string, Material> materialMap, bool inverseTransform = false)
	{
		this.meshSmooth = null;
		this._meshData = null;
		JSONNode jsonnode = jsonGeometry["vertices"];
		this._numBaseVertices = jsonnode["count"].AsInt;
		this._baseVertices = new Vector3[this._numBaseVertices];
		int num = 0;
		IEnumerator enumerator = jsonnode["values"].AsArray.GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				JSONNode jsonnode2 = (JSONNode)obj;
				float asFloat = jsonnode2[0].AsFloat;
				float asFloat2 = jsonnode2[1].AsFloat;
				float asFloat3 = jsonnode2[2].AsFloat;
				this._baseVertices[num].x = -asFloat * 0.01f;
				this._baseVertices[num].y = asFloat2 * 0.01f;
				this._baseVertices[num].z = asFloat3 * 0.01f;
				if (inverseTransform)
				{
					this._baseVertices[num] = base.transform.InverseTransformPoint(this._baseVertices[num]);
				}
				num++;
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		this._numMaterials = jsonGeometry["polygon_material_groups"]["count"].AsInt;
		if (this.materials == null || this.materials.Length != this._numMaterials)
		{
			this.materials = new Material[this._numMaterials];
			this._materialNames = new string[this._numMaterials];
		}
		if (this.materialsPass1 == null || this.materialsPass1.Length != this._numMaterials)
		{
			this.materialsPass1 = new Material[this._numMaterials];
		}
		if (this.materialsPass2 == null || this.materialsPass2.Length != this._numMaterials)
		{
			this.materialsPass2 = new Material[this._numMaterials];
		}
		if (this.materialsEnabled == null || this.materialsEnabled.Length != this._numMaterials)
		{
			this.materialsEnabled = new bool[this._numMaterials];
		}
		if (this.materialsPass1Enabled == null || this.materialsPass1Enabled.Length != this._numMaterials)
		{
			this.materialsPass1Enabled = new bool[this._numMaterials];
		}
		if (this.materialsPass2Enabled == null || this.materialsPass2Enabled.Length != this._numMaterials)
		{
			this.materialsPass2Enabled = new bool[this._numMaterials];
		}
		if (this.materialsShadowCastEnabled == null || this.materialsShadowCastEnabled.Length != this._numMaterials)
		{
			this.materialsShadowCastEnabled = new bool[this._numMaterials];
		}
		num = 0;
		IEnumerator enumerator2 = jsonGeometry["polygon_material_groups"]["values"].AsArray.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				object obj2 = enumerator2.Current;
				JSONNode d = (JSONNode)obj2;
				this._materialNames[num] = d;
				this.materialsEnabled[num] = true;
				this.materialsShadowCastEnabled[num] = true;
				Material material;
				if (materialMap.TryGetValue(d, out material))
				{
					this.materials[num] = material;
				}
				num++;
			}
		}
		finally
		{
			IDisposable disposable2;
			if ((disposable2 = (enumerator2 as IDisposable)) != null)
			{
				disposable2.Dispose();
			}
		}
		JSONNode jsonnode3 = jsonGeometry["polylist"];
		this._numBasePolygons = jsonnode3["count"].AsInt;
		this._basePolyList = new MeshPoly[this._numBasePolygons];
		this._UVPolyList = new MeshPoly[this._numBasePolygons];
		num = 0;
		IEnumerator enumerator3 = jsonnode3["values"].AsArray.GetEnumerator();
		try
		{
			while (enumerator3.MoveNext())
			{
				object obj3 = enumerator3.Current;
				JSONNode jsonnode4 = (JSONNode)obj3;
				int asInt = jsonnode4[1].AsInt;
				int asInt2 = jsonnode4[2].AsInt;
				int asInt3 = jsonnode4[3].AsInt;
				int asInt4 = jsonnode4[4].AsInt;
				MeshPoly meshPoly = new MeshPoly();
				if (jsonnode4.Count == 6)
				{
					int asInt5 = jsonnode4[5].AsInt;
					meshPoly.vertices = new int[4];
					meshPoly.vertices[3] = asInt5;
				}
				else
				{
					meshPoly.vertices = new int[3];
				}
				meshPoly.materialNum = asInt;
				meshPoly.vertices[0] = asInt2;
				meshPoly.vertices[1] = asInt3;
				meshPoly.vertices[2] = asInt4;
				this._basePolyList[num] = meshPoly;
				MeshPoly meshPoly2 = new MeshPoly();
				meshPoly2.materialNum = meshPoly.materialNum;
				meshPoly2.vertices = new int[meshPoly.vertices.Length];
				for (int i = 0; i < meshPoly.vertices.Length; i++)
				{
					meshPoly2.vertices[i] = meshPoly.vertices[i];
				}
				this._UVPolyList[num] = meshPoly2;
				num++;
			}
		}
		finally
		{
			IDisposable disposable3;
			if ((disposable3 = (enumerator3 as IDisposable)) != null)
			{
				disposable3.Dispose();
			}
		}
		if (jsonGeometry["graft"] != null)
		{
			DAZMeshGraft dazmeshGraft = new DAZMeshGraft();
			JSONNode jsonnode5 = jsonGeometry["graft"]["vertex_pairs"];
			if (jsonnode5 != null)
			{
				int asInt6 = jsonnode5["count"].AsInt;
				dazmeshGraft.vertexPairs = new DAZMeshGraftVertexPair[asInt6];
				num = 0;
				IEnumerator enumerator4 = jsonnode5["values"].AsArray.GetEnumerator();
				try
				{
					while (enumerator4.MoveNext())
					{
						object obj4 = enumerator4.Current;
						JSONNode jsonnode6 = (JSONNode)obj4;
						DAZMeshGraftVertexPair dazmeshGraftVertexPair = new DAZMeshGraftVertexPair();
						dazmeshGraftVertexPair.vertexNum = jsonnode6[0].AsInt;
						dazmeshGraftVertexPair.graftToVertexNum = jsonnode6[1].AsInt;
						dazmeshGraft.vertexPairs[num] = dazmeshGraftVertexPair;
						num++;
					}
				}
				finally
				{
					IDisposable disposable4;
					if ((disposable4 = (enumerator4 as IDisposable)) != null)
					{
						disposable4.Dispose();
					}
				}
				JSONNode jsonnode7 = jsonGeometry["graft"]["hidden_polys"];
				int asInt7 = jsonnode7["count"].AsInt;
				dazmeshGraft.hiddenPolys = new int[asInt7];
				num = 0;
				IEnumerator enumerator5 = jsonnode7["values"].AsArray.GetEnumerator();
				try
				{
					while (enumerator5.MoveNext())
					{
						object obj5 = enumerator5.Current;
						JSONNode jsonnode8 = (JSONNode)obj5;
						dazmeshGraft.hiddenPolys[num] = jsonnode8.AsInt;
						num++;
					}
				}
				finally
				{
					IDisposable disposable5;
					if ((disposable5 = (enumerator5 as IDisposable)) != null)
					{
						disposable5.Dispose();
					}
				}
				this.meshGraft = dazmeshGraft;
			}
		}
		if (uvmap.uvs == null)
		{
			this._OrigUV = new Vector2[this._numBaseVertices];
			this._UV = new Vector2[this._numBaseVertices];
			this._numUVVertices = this._numBaseVertices;
			this._UVVertices = new Vector3[this._numUVVertices];
			this._morphedUVVertices = new Vector3[this._numUVVertices];
			this._baseVerticesToUVVertices = new DAZVertexMap[0];
			for (int j = 0; j < this._baseVertices.Length; j++)
			{
				this._UVVertices[j] = this._baseVertices[j];
				this._morphedUVVertices[j] = this._baseVertices[j];
			}
		}
		else
		{
			this._OrigUV = uvmap.uvs;
			this._UV = (Vector2[])uvmap.uvs.Clone();
			this._numUVVertices = uvmap.uvs.Length;
			this._UVVertices = new Vector3[this._numUVVertices];
			this._morphedUVVertices = new Vector3[this._numUVVertices];
			this._baseVerticesToUVVertices = new DAZVertexMap[this._numUVVertices - this._numBaseVertices];
			for (int k = 0; k < this._baseVertices.Length; k++)
			{
				this._UVVertices[k] = this._baseVertices[k];
				this._morphedUVVertices[k] = this._baseVertices[k];
			}
			Dictionary<int, bool> dictionary = new Dictionary<int, bool>();
			int num2 = 0;
			foreach (DAZVertexMap dazvertexMap in uvmap.vertexMap)
			{
				this._UVVertices[dazvertexMap.tovert] = this._baseVertices[dazvertexMap.fromvert];
				this._morphedUVVertices[dazvertexMap.tovert] = this._baseVertices[dazvertexMap.fromvert];
				bool flag;
				if (!dictionary.TryGetValue(dazvertexMap.tovert, out flag))
				{
					this._baseVerticesToUVVertices[num2] = dazvertexMap;
					dictionary.Add(dazvertexMap.tovert, true);
					num2++;
				}
				MeshPoly meshPoly3 = this._UVPolyList[dazvertexMap.polyindex];
				for (int m = 0; m < meshPoly3.vertices.Length; m++)
				{
					if (meshPoly3.vertices[m] == dazvertexMap.fromvert)
					{
						meshPoly3.vertices[m] = dazvertexMap.tovert;
					}
				}
			}
		}
		this.DeriveMeshes();
		this.InitMeshFilterAndRenderer();
	}

	// Token: 0x06004A68 RID: 19048 RVA: 0x0017FDCC File Offset: 0x0017E1CC
	public void ReduceMeshToSingleMaterial(int materialNum)
	{
		if (materialNum >= 0 && materialNum < this._numMaterials)
		{
			this.DeriveMeshes();
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
			int num = this._baseMaterialVertices[materialNum].Length;
			Vector3[] array = new Vector3[num];
			Vector3[] baseVertices = this.baseVertices;
			for (int i = 0; i < num; i++)
			{
				int num2 = this._baseMaterialVertices[materialNum][i];
				dictionary.Add(num2, i);
				dictionary2.Add(i, num2);
				array[i] = baseVertices[num2];
			}
			int num3 = 1;
			Material[] array2 = new Material[num3];
			array2[0] = this.materials[materialNum];
			string[] array3 = new string[num3];
			array3[0] = this._materialNames[materialNum];
			Material[] array4 = new Material[num3];
			if (this.materialsPass1 != null && materialNum < this.materialsPass1.Length)
			{
				array4[0] = this.materialsPass1[materialNum];
			}
			Material[] array5 = new Material[num3];
			if (this.materialsPass2 != null && materialNum < this.materialsPass2.Length)
			{
				array5[0] = this.materialsPass2[materialNum];
			}
			bool[] array6 = new bool[num3];
			array6[0] = this.materialsEnabled[materialNum];
			bool[] array7 = new bool[num3];
			if (this.materialsPass1Enabled != null && materialNum < this.materialsPass1Enabled.Length)
			{
				array7[0] = this.materialsPass1Enabled[materialNum];
			}
			bool[] array8 = new bool[num3];
			if (this.materialsPass2Enabled != null && materialNum < this.materialsPass2Enabled.Length)
			{
				array8[0] = this.materialsPass2Enabled[materialNum];
			}
			bool[] array9 = new bool[num3];
			array9[0] = this.materialsShadowCastEnabled[materialNum];
			List<MeshPoly> list = new List<MeshPoly>();
			List<MeshPoly> list2 = new List<MeshPoly>();
			List<DAZVertexMap> list3 = new List<DAZVertexMap>();
			int num4 = num;
			int num5 = 0;
			MeshPoly[] basePolyList = this.basePolyList;
			MeshPoly[] uvpolyList = this.UVPolyList;
			for (int j = 0; j < this._numBasePolygons; j++)
			{
				MeshPoly meshPoly = basePolyList[j];
				MeshPoly meshPoly2 = uvpolyList[j];
				if (meshPoly.materialNum == materialNum)
				{
					MeshPoly meshPoly3 = new MeshPoly();
					MeshPoly meshPoly4 = new MeshPoly();
					meshPoly3.vertices = new int[meshPoly.vertices.Length];
					meshPoly4.vertices = new int[meshPoly.vertices.Length];
					for (int k = 0; k < meshPoly.vertices.Length; k++)
					{
						int num6;
						if (dictionary.TryGetValue(meshPoly.vertices[k], out num6))
						{
							meshPoly3.vertices[k] = num6;
							int num7;
							if (dictionary.TryGetValue(meshPoly2.vertices[k], out num7))
							{
								meshPoly4.vertices[k] = num7;
							}
							else
							{
								meshPoly4.vertices[k] = num4;
								dictionary.Add(meshPoly2.vertices[k], num4);
								dictionary2.Add(num4, meshPoly2.vertices[k]);
								list3.Add(new DAZVertexMap
								{
									polyindex = num5,
									fromvert = num6,
									tovert = num4
								});
								num4++;
							}
						}
						else
						{
							Debug.LogError("Could not find vert index " + meshPoly.vertices[k] + " in old vert to new vert map, but it should be there");
						}
					}
					meshPoly3.materialNum = 0;
					list.Add(meshPoly3);
					meshPoly4.materialNum = 0;
					list2.Add(meshPoly4);
					num5++;
				}
			}
			int count = list.Count;
			MeshPoly[] basePolyList2 = list.ToArray();
			MeshPoly[] uvpolyList2 = list2.ToArray();
			int num8 = num4;
			Vector3[] array10 = new Vector3[num8];
			Vector3[] array11 = new Vector3[num8];
			for (int l = 0; l < num; l++)
			{
				array10[l] = array[l];
				array11[l] = array[l];
			}
			foreach (DAZVertexMap dazvertexMap in list3)
			{
				array10[dazvertexMap.tovert] = array[dazvertexMap.fromvert];
				array11[dazvertexMap.tovert] = array[dazvertexMap.fromvert];
			}
			Vector2[] array12 = new Vector2[num8];
			Vector2[] origUV = this.OrigUV;
			for (int m = 0; m < num8; m++)
			{
				int num9;
				if (dictionary2.TryGetValue(m, out num9))
				{
					array12[m] = origUV[num9];
				}
				else
				{
					Debug.LogError("Could not find new vert index " + m + " in newVertToOldVert map, but it should be there");
				}
			}
			this._numBaseVertices = num;
			this._baseVertices = array;
			this._numMaterials = num3;
			this._materialNames = array3;
			this.materials = array2;
			this.materialsEnabled = array6;
			this.materialsPass1 = array4;
			this.materialsPass2 = array5;
			this.materialsPass1Enabled = array7;
			this.materialsPass2Enabled = array8;
			this.materialsShadowCastEnabled = array9;
			this._numBasePolygons = count;
			this._basePolyList = basePolyList2;
			this._UVPolyList = uvpolyList2;
			this._numUVVertices = num8;
			this._UVVertices = array10;
			this._morphedUVVertices = array11;
			this._baseVerticesToUVVertices = list3.ToArray();
			this._OrigUV = array12;
			this.DeriveMeshes();
			this.InitMeshFilterAndRenderer();
		}
	}

	// Token: 0x06004A69 RID: 19049 RVA: 0x00180354 File Offset: 0x0017E754
	public void ReduceMeshToMaterials(List<int> materialNums)
	{
		if (materialNums.Count > 0 && materialNums[0] >= 0 && materialNums[0] < this._numMaterials)
		{
			this.DeriveMeshes();
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
			int num = 0;
			for (int i = 0; i < this._numMaterials; i++)
			{
				if (materialNums.Contains(i))
				{
					for (int j = 0; j < this._baseMaterialVertices[i].Length; j++)
					{
						int num2 = this._baseMaterialVertices[i][j];
						if (!dictionary.ContainsKey(num2))
						{
							dictionary.Add(num2, num);
							dictionary2.Add(num, num2);
							num++;
						}
					}
				}
			}
			Vector3[] array = new Vector3[num];
			Vector3[] baseVertices = this.baseVertices;
			foreach (int num3 in dictionary.Keys)
			{
				int num4;
				if (dictionary.TryGetValue(num3, out num4))
				{
					array[num4] = baseVertices[num3];
				}
			}
			int count = materialNums.Count;
			Material[] array2 = new Material[count];
			Material[] array3 = new Material[count];
			Material[] array4 = new Material[count];
			bool[] array5 = new bool[count];
			bool[] array6 = new bool[count];
			bool[] array7 = new bool[count];
			bool[] array8 = new bool[count];
			string[] array9 = new string[count];
			int num5 = 0;
			Dictionary<int, int> dictionary3 = new Dictionary<int, int>();
			for (int k = 0; k < this._numMaterials; k++)
			{
				if (materialNums.Contains(k))
				{
					dictionary3.Add(k, num5);
					array2[num5] = this.materials[k];
					array9[num5] = this._materialNames[k];
					if (this.materialsPass1 != null && k < this.materialsPass1.Length)
					{
						array3[num5] = this.materialsPass1[k];
					}
					if (this.materialsPass2 != null && k < this.materialsPass2.Length)
					{
						array4[num5] = this.materialsPass2[k];
					}
					array5[num5] = this.materialsEnabled[k];
					if (this.materialsPass1Enabled != null && k < this.materialsPass1Enabled.Length)
					{
						array6[num5] = this.materialsPass1Enabled[k];
					}
					if (this.materialsPass2Enabled != null && k < this.materialsPass2Enabled.Length)
					{
						array7[num5] = this.materialsPass2Enabled[k];
					}
					array8[num5] = this.materialsShadowCastEnabled[k];
					num5++;
				}
			}
			List<MeshPoly> list = new List<MeshPoly>();
			List<MeshPoly> list2 = new List<MeshPoly>();
			List<DAZVertexMap> list3 = new List<DAZVertexMap>();
			int num6 = num;
			int num7 = 0;
			MeshPoly[] basePolyList = this.basePolyList;
			MeshPoly[] uvpolyList = this.UVPolyList;
			for (int l = 0; l < this._numBasePolygons; l++)
			{
				MeshPoly meshPoly = basePolyList[l];
				MeshPoly meshPoly2 = uvpolyList[l];
				if (materialNums.Contains(meshPoly.materialNum))
				{
					MeshPoly meshPoly3 = new MeshPoly();
					MeshPoly meshPoly4 = new MeshPoly();
					meshPoly3.vertices = new int[meshPoly.vertices.Length];
					meshPoly4.vertices = new int[meshPoly.vertices.Length];
					for (int m = 0; m < meshPoly.vertices.Length; m++)
					{
						int num8;
						if (dictionary.TryGetValue(meshPoly.vertices[m], out num8))
						{
							meshPoly3.vertices[m] = num8;
							int num9;
							if (dictionary.TryGetValue(meshPoly2.vertices[m], out num9))
							{
								meshPoly4.vertices[m] = num9;
							}
							else
							{
								meshPoly4.vertices[m] = num6;
								dictionary.Add(meshPoly2.vertices[m], num6);
								dictionary2.Add(num6, meshPoly2.vertices[m]);
								list3.Add(new DAZVertexMap
								{
									polyindex = num7,
									fromvert = num8,
									tovert = num6
								});
								num6++;
							}
						}
						else
						{
							Debug.LogError("Could not find vert index " + meshPoly.vertices[m] + " in old vert to new vert map, but it should be there");
						}
					}
					int materialNum;
					if (dictionary3.TryGetValue(meshPoly.materialNum, out materialNum))
					{
						meshPoly3.materialNum = materialNum;
						list.Add(meshPoly3);
						meshPoly4.materialNum = materialNum;
						list2.Add(meshPoly4);
						num7++;
					}
				}
			}
			int count2 = list.Count;
			MeshPoly[] basePolyList2 = list.ToArray();
			MeshPoly[] uvpolyList2 = list2.ToArray();
			int num10 = num6;
			Vector3[] array10 = new Vector3[num10];
			Vector3[] array11 = new Vector3[num10];
			for (int n = 0; n < num; n++)
			{
				array10[n] = array[n];
				array11[n] = array[n];
			}
			foreach (DAZVertexMap dazvertexMap in list3)
			{
				array10[dazvertexMap.tovert] = array[dazvertexMap.fromvert];
				array11[dazvertexMap.tovert] = array[dazvertexMap.fromvert];
			}
			Vector2[] array12 = new Vector2[num10];
			Vector2[] origUV = this.OrigUV;
			for (int num11 = 0; num11 < num10; num11++)
			{
				int num12;
				if (dictionary2.TryGetValue(num11, out num12))
				{
					array12[num11] = origUV[num12];
				}
				else
				{
					Debug.LogError("Could not find new vert index " + num11 + " in newVertToOldVert map, but it should be there");
				}
			}
			this._numBaseVertices = num;
			this._baseVertices = array;
			this._numMaterials = count;
			this._materialNames = array9;
			this.materials = array2;
			this.materialsEnabled = array5;
			this.materialsPass1 = array3;
			this.materialsPass2 = array4;
			this.materialsPass1Enabled = array6;
			this.materialsPass2Enabled = array7;
			this.materialsShadowCastEnabled = array8;
			this._numBasePolygons = count2;
			this._basePolyList = basePolyList2;
			this._UVPolyList = uvpolyList2;
			this._numUVVertices = num10;
			this._UVVertices = array10;
			this._morphedUVVertices = array11;
			this._baseVerticesToUVVertices = list3.ToArray();
			this._OrigUV = array12;
			this.DeriveMeshes();
			this.InitMeshFilterAndRenderer();
		}
	}

	// Token: 0x06004A6A RID: 19050 RVA: 0x001809FC File Offset: 0x0017EDFC
	public void ReduceMeshBelowPlane()
	{
		this.DeriveMeshes();
		Dictionary<int, int> dictionary = new Dictionary<int, int>();
		Dictionary<int, int> dictionary2 = new Dictionary<int, int>();
		int num = 0;
		List<Vector3> list = new List<Vector3>();
		List<Vector3> list2 = new List<Vector3>();
		Vector3[] baseVertices = this.baseVertices;
		for (int i = 0; i < this._numBaseVertices; i++)
		{
			Vector3 vector = baseVertices[i];
			float num2 = Vector3.Dot(this.reduceUp, vector - this.reducePoint);
			if (num2 >= 0f)
			{
				dictionary.Add(i, num);
				dictionary2.Add(num, i);
				list.Add(vector);
				list2.Add(vector);
				num++;
			}
		}
		int numBaseVertices = num;
		Vector3[] uvvertices = this.UVVertices;
		for (int j = this._numBaseVertices; j < this._numUVVertices; j++)
		{
			Vector3 vector2 = uvvertices[j];
			float num3 = Vector3.Dot(this.reduceUp, vector2 - this.reducePoint);
			if (num3 >= 0f)
			{
				dictionary.Add(j, num);
				dictionary2.Add(num, j);
				list2.Add(vector2);
				num++;
			}
		}
		Vector3[] baseVertices2 = list.ToArray();
		Vector3[] array = list2.ToArray();
		Vector3[] morphedUVVertices = (Vector3[])array.Clone();
		List<MeshPoly> list3 = new List<MeshPoly>();
		List<MeshPoly> list4 = new List<MeshPoly>();
		int num4 = 0;
		List<DAZVertexMap> list5 = new List<DAZVertexMap>();
		MeshPoly[] basePolyList = this.basePolyList;
		MeshPoly[] uvpolyList = this.UVPolyList;
		for (int k = 0; k < this._numBasePolygons; k++)
		{
			MeshPoly meshPoly = basePolyList[k];
			MeshPoly meshPoly2 = uvpolyList[k];
			bool flag = true;
			MeshPoly meshPoly3 = new MeshPoly();
			MeshPoly meshPoly4 = new MeshPoly();
			meshPoly3.materialNum = meshPoly.materialNum;
			meshPoly4.materialNum = meshPoly2.materialNum;
			meshPoly3.vertices = new int[meshPoly.vertices.Length];
			meshPoly4.vertices = new int[meshPoly.vertices.Length];
			for (int l = 0; l < meshPoly.vertices.Length; l++)
			{
				int num5;
				if (!dictionary.TryGetValue(meshPoly.vertices[l], out num5))
				{
					flag = false;
					break;
				}
				meshPoly3.vertices[l] = num5;
				int num6;
				if (dictionary.TryGetValue(meshPoly2.vertices[l], out num6))
				{
					meshPoly4.vertices[l] = num6;
				}
			}
			if (flag)
			{
				list3.Add(meshPoly3);
				list4.Add(meshPoly4);
				for (int m = 0; m < meshPoly3.vertices.Length; m++)
				{
					int num7 = meshPoly3.vertices[m];
					int num8 = meshPoly4.vertices[m];
					if (num7 != num8)
					{
						list5.Add(new DAZVertexMap
						{
							fromvert = num7,
							tovert = num8,
							polyindex = num4
						});
					}
				}
				num4++;
			}
		}
		int count = list3.Count;
		MeshPoly[] basePolyList2 = list3.ToArray();
		MeshPoly[] uvpolyList2 = list4.ToArray();
		Vector2[] array2 = new Vector2[num];
		Vector2[] origUV = this.OrigUV;
		for (int n = 0; n < num; n++)
		{
			int num9;
			if (dictionary2.TryGetValue(n, out num9))
			{
				array2[n] = origUV[num9];
			}
			else
			{
				Debug.LogError("Could not find new vert index " + n + " in newVertToOldVert map, but it should be there");
			}
		}
		this._numBaseVertices = numBaseVertices;
		this._baseVertices = baseVertices2;
		this._numBasePolygons = count;
		this._basePolyList = basePolyList2;
		this._UVPolyList = uvpolyList2;
		this._numUVVertices = num;
		this._UVVertices = array;
		this._morphedUVVertices = morphedUVVertices;
		this._baseVerticesToUVVertices = list5.ToArray();
		this._OrigUV = array2;
		this._meshData = null;
		this.DeriveMeshes();
		this.InitMeshFilterAndRenderer();
	}

	// Token: 0x06004A6B RID: 19051 RVA: 0x00180DE0 File Offset: 0x0017F1E0
	protected void _updateDuplicateMorphedUVVertices()
	{
		if (this.baseVerticesToUVVertices != null)
		{
			foreach (DAZVertexMap dazvertexMap in this.baseVerticesToUVVertices)
			{
				this._morphedUVVertices[dazvertexMap.tovert] = this._morphedUVVertices[dazvertexMap.fromvert];
				this._visibleMorphedUVVertices[dazvertexMap.tovert] = this._visibleMorphedUVVertices[dazvertexMap.fromvert];
			}
		}
	}

	// Token: 0x06004A6C RID: 19052 RVA: 0x00180E70 File Offset: 0x0017F270
	protected void _updateDuplicateSmoothedMorphedUVVertices()
	{
		if (this.baseVerticesToUVVertices != null)
		{
			foreach (DAZVertexMap dazvertexMap in this.baseVerticesToUVVertices)
			{
				this._smoothedMorphedUVVertices[dazvertexMap.tovert] = this._smoothedMorphedUVVertices[dazvertexMap.fromvert];
				this._visibleMorphedUVVertices[dazvertexMap.tovert] = this._visibleMorphedUVVertices[dazvertexMap.fromvert];
			}
		}
	}

	// Token: 0x06004A6D RID: 19053 RVA: 0x00180F00 File Offset: 0x0017F300
	protected void _updateDuplicateMorphedUVNormals()
	{
		if (this.baseVerticesToUVVertices != null)
		{
			foreach (DAZVertexMap dazvertexMap in this.baseVerticesToUVVertices)
			{
				this._morphedUVNormals[dazvertexMap.tovert] = this._morphedUVNormals[dazvertexMap.fromvert];
			}
		}
	}

	// Token: 0x06004A6E RID: 19054 RVA: 0x00180F63 File Offset: 0x0017F363
	public void StartMorph()
	{
		this._verticesChangedLastFrame = this._verticesChangedThisFrame;
		this._visibleNonPoseVerticesChangedLastFrame = this._visibleNonPoseVerticesChangedThisFrame;
		this._verticesChangedThisFrame = false;
		this._visibleNonPoseVerticesChangedThisFrame = false;
	}

	// Token: 0x06004A6F RID: 19055 RVA: 0x00180F8C File Offset: 0x0017F38C
	public void ApplyMorphVertices(bool visibleNonPoseVerticesChanged)
	{
		this._verticesChangedThisFrame = true;
		this._visibleNonPoseVerticesChangedThisFrame = visibleNonPoseVerticesChanged;
		if (this.useSmoothing)
		{
			this.InitMeshSmooth();
			this.meshSmooth.LaplacianSmooth(this._morphedUVVertices, this._smoothedMorphedUVVertices, 0, 100000000);
			this.meshSmooth.HCCorrection(this._morphedUVVertices, this._smoothedMorphedUVVertices, 0.5f, 0, 1000000000);
			this._updateDuplicateSmoothedMorphedUVVertices();
			if (this._drawMorphedUVMappedMesh || !Application.isPlaying)
			{
				this._morphedUVMappedMesh.vertices = this._smoothedMorphedUVVertices;
			}
			this._triggerNormalAndTangentRecalc();
		}
		else
		{
			this._updateDuplicateMorphedUVVertices();
			if (this._drawMorphedUVMappedMesh || !Application.isPlaying)
			{
				this._morphedUVMappedMesh.vertices = this._morphedUVVertices;
			}
			this._triggerNormalAndTangentRecalc();
		}
		if (this._drawMorphedBaseMesh)
		{
			this._morphedBaseMesh.vertices = this._morphedBaseVertices;
		}
		if (this._drawVisibleMorphedUVMappedMesh || !Application.isPlaying)
		{
			this._visibleMorphedUVMappedMesh.vertices = this._visibleMorphedUVVertices;
		}
	}

	// Token: 0x06004A70 RID: 19056 RVA: 0x001810A2 File Offset: 0x0017F4A2
	public void ReInitMorphs()
	{
		if (this.morphBank != null)
		{
			this.Init();
			this.morphBank.ReInit();
		}
	}

	// Token: 0x06004A71 RID: 19057 RVA: 0x001810C6 File Offset: 0x0017F4C6
	public void ResetMorphs()
	{
		if (this.morphBank != null)
		{
			this.morphBank.ResetMorphs();
		}
	}

	// Token: 0x06004A72 RID: 19058 RVA: 0x001810E4 File Offset: 0x0017F4E4
	public void ApplyMorphs(bool force = false)
	{
		if (this.morphBank != null)
		{
			this.morphBank.ApplyMorphs(force);
		}
	}

	// Token: 0x06004A73 RID: 19059 RVA: 0x00181104 File Offset: 0x0017F504
	public void ResetMorphedVerticesToOffset()
	{
		Vector3[] baseVertices = this.baseVertices;
		Vector3[] uvvertices = this.UVVertices;
		if (this.vertexNormalOffset != 0f)
		{
			for (int i = 0; i < this._morphedBaseVertices.Length; i++)
			{
				this._morphedBaseVertices[i] = baseVertices[i] + this._morphedBaseNormals[i] * this.vertexNormalOffset + this.vertexOffset;
			}
			for (int j = 0; j < this._morphedUVVertices.Length; j++)
			{
				this._morphedUVVertices[j] = uvvertices[j] + this._morphedUVNormals[j] * this.vertexNormalOffset + this.vertexOffset;
				this._visibleMorphedUVVertices[j] = this._morphedUVVertices[j];
			}
		}
		else
		{
			for (int k = 0; k < this._morphedBaseVertices.Length; k++)
			{
				this._morphedBaseVertices[k] = baseVertices[k] + this.vertexOffset;
			}
			for (int l = 0; l < this._morphedUVVertices.Length; l++)
			{
				this._morphedUVVertices[l] = uvvertices[l] + this.vertexOffset;
				this._visibleMorphedUVVertices[l] = this._morphedUVVertices[l];
			}
		}
		if (this._morphedBaseMesh != null)
		{
			this._morphedBaseMesh.vertices = this._morphedBaseVertices;
		}
		if (this._morphedUVMappedMesh != null)
		{
			this._morphedUVMappedMesh.vertices = this._morphedUVVertices;
		}
		if (this._visibleMorphedUVMappedMesh != null)
		{
			this._visibleMorphedUVMappedMesh.vertices = this._visibleMorphedUVVertices;
		}
	}

	// Token: 0x06004A74 RID: 19060 RVA: 0x00181334 File Offset: 0x0017F734
	public void ResetMorphedVertices()
	{
		if (this._wasInit)
		{
			this._verticesChangedThisFrame = true;
			this._visibleNonPoseVerticesChangedThisFrame = true;
			Vector3[] uvvertices = this.UVVertices;
			if (this.vertexNormalOffset == 0f)
			{
				for (int i = 0; i < this._numUVVertices; i++)
				{
					this._morphedUVVertices[i] = uvvertices[i] + this.vertexOffset;
					this._visibleMorphedUVVertices[i] = this._morphedUVVertices[i];
				}
			}
			else
			{
				for (int j = 0; j < this._numUVVertices; j++)
				{
					this._morphedUVVertices[j] = uvvertices[j] + this._UVNormals[j] * this.vertexNormalOffset + this.vertexOffset;
					this._visibleMorphedUVVertices[j] = this._morphedUVVertices[j];
				}
			}
			if (this._morphedUVMappedMesh != null && this._morphedUVVertices != null)
			{
				this._morphedUVMappedMesh.vertices = this._morphedUVVertices;
			}
			if (this._visibleMorphedUVMappedMesh != null && this._visibleMorphedUVVertices != null)
			{
				this._visibleMorphedUVMappedMesh.vertices = this._visibleMorphedUVVertices;
			}
		}
	}

	// Token: 0x06004A75 RID: 19061 RVA: 0x001814B4 File Offset: 0x0017F8B4
	public void CreateFolderIfNeeded(string filename)
	{
		string directoryName = Path.GetDirectoryName(filename);
		if (!Directory.Exists(directoryName))
		{
			Directory.CreateDirectory(directoryName);
		}
	}

	// Token: 0x06004A76 RID: 19062 RVA: 0x001814DA File Offset: 0x0017F8DA
	public void SaveMeshAsset(bool overwrite = false)
	{
	}

	// Token: 0x06004A77 RID: 19063 RVA: 0x001814DC File Offset: 0x0017F8DC
	public void SyncMeshRendererMaterials()
	{
		if (this._createMeshFilterAndRenderer && (!this._meshRendererMaterialsWasInit || !Application.isPlaying))
		{
			MeshRenderer component = base.GetComponent<MeshRenderer>();
			if (component != null && this.materials != null)
			{
				this._meshRendererMaterialsWasInit = true;
				Material[] array = component.sharedMaterials;
				if (array.Length != this.materials.Length)
				{
					array = new Material[this.materials.Length];
				}
				for (int i = 0; i < this.materials.Length; i++)
				{
					if (this.materialsEnabled[i])
					{
						if (this.useSimpleMaterial)
						{
							array[i] = this.simpleMaterial;
						}
						else
						{
							array[i] = this.materials[i];
						}
					}
					else
					{
						array[i] = this.discardMaterial;
					}
				}
				component.sharedMaterials = array;
			}
		}
	}

	// Token: 0x06004A78 RID: 19064 RVA: 0x001815B4 File Offset: 0x0017F9B4
	protected void InitMeshFilterAndRenderer()
	{
		MeshFilter meshFilter = base.GetComponent<MeshFilter>();
		MeshRenderer meshRenderer = base.GetComponent<MeshRenderer>();
		if (meshFilter == null && this._createMeshFilterAndRenderer)
		{
			meshFilter = base.gameObject.AddComponent<MeshFilter>();
		}
		if (meshRenderer == null && this._createMeshFilterAndRenderer)
		{
			meshRenderer = base.gameObject.AddComponent<MeshRenderer>();
		}
		if (meshFilter != null && meshRenderer != null && meshRenderer.enabled)
		{
			if (meshFilter.sharedMesh != this.morphedUVMappedMesh)
			{
				meshFilter.sharedMesh = this.morphedUVMappedMesh;
			}
			this.SyncMeshRendererMaterials();
		}
	}

	// Token: 0x06004A79 RID: 19065 RVA: 0x00181660 File Offset: 0x0017FA60
	public void DrawMorphedUVMappedMesh(Matrix4x4 m)
	{
		MeshFilter component = base.GetComponent<MeshFilter>();
		MeshRenderer component2 = base.GetComponent<MeshRenderer>();
		if (component != null && component2 != null && component2.enabled)
		{
			if (component.sharedMesh != this.morphedUVMappedMesh)
			{
				component.sharedMesh = this.morphedUVMappedMesh;
			}
		}
		else if (this.use2PassMaterials)
		{
			for (int i = 0; i < this.morphedUVMappedMesh.subMeshCount; i++)
			{
				Material material = null;
				if (this.materialsPass1 != null)
				{
					material = this.materialsPass1[i];
				}
				if (material == null || this.useSimpleMaterial)
				{
					material = this.simpleMaterial;
				}
				if (material != null && this.materialsPass1Enabled[i])
				{
					Graphics.DrawMesh(this.morphedUVMappedMesh, m, material, 0, null, i, null, this.castShadows && this.materialsShadowCastEnabled[i], this.receiveShadows);
				}
			}
			for (int j = 0; j < this.morphedUVMappedMesh.subMeshCount; j++)
			{
				Material material2 = null;
				if (this.materialsPass2 != null)
				{
					material2 = this.materialsPass2[j];
				}
				if (material2 == null || this.useSimpleMaterial)
				{
					material2 = this.simpleMaterial;
				}
				if (material2 != null && this.materialsPass2Enabled[j])
				{
					Graphics.DrawMesh(this.morphedUVMappedMesh, m, material2, 0, null, j, null, this.castShadows && this.materialsShadowCastEnabled[j], this.receiveShadows);
				}
			}
		}
		else
		{
			for (int k = 0; k < this.morphedUVMappedMesh.subMeshCount; k++)
			{
				Material material3 = null;
				if (this.materials != null)
				{
					material3 = this.materials[k];
				}
				if (material3 == null || this.useSimpleMaterial)
				{
					material3 = this.simpleMaterial;
				}
				if (material3 != null && this.materialsEnabled[k])
				{
					Graphics.DrawMesh(this.morphedUVMappedMesh, m, material3, 0, null, k, null, this.castShadows && this.materialsShadowCastEnabled[k], this.receiveShadows);
				}
			}
		}
	}

	// Token: 0x06004A7A RID: 19066 RVA: 0x001818B4 File Offset: 0x0017FCB4
	public void Draw()
	{
		if (!this._renderSuspend && (this.drawBaseMesh || this.drawMorphedBaseMesh || this.drawUVMappedMesh || this._drawMorphedUVMappedMesh || this._drawVisibleMorphedUVMappedMesh || this.debugGrafting || (this.drawInEditorWhenNotPlaying && !Application.isPlaying)))
		{
			Matrix4x4 matrix4x;
			if (Application.isPlaying && this.drawFromBone != null)
			{
				if (this.useDrawOffset)
				{
					Quaternion q = Quaternion.Euler(this.drawOffsetRotation);
					this.drawOffsetMatrix.SetTRS(this.drawOffset, q, this.drawOffsetScale * this.drawOffsetOverallScale);
					this.drawOffsetOriginMatrix1.SetTRS(this.drawOffsetOrigin, this.identityQuaternion, this.oneVector);
					this.drawOffsetOriginMatrix2.SetTRS(-this.drawOffsetOrigin, this.identityQuaternion, this.oneVector);
					matrix4x = this.drawFromBone.transform.localToWorldMatrix * this.drawFromBone.nonMorphedWorldToLocalMatrix * this.drawOffsetOriginMatrix1 * this.drawOffsetMatrix * this.drawOffsetOriginMatrix2;
				}
				else
				{
					matrix4x = this.drawFromBone.transform.localToWorldMatrix * this.drawFromBone.nonMorphedWorldToLocalMatrix;
				}
			}
			else if (this.useDrawOffset)
			{
				Quaternion q2 = Quaternion.Euler(this.drawOffsetRotation);
				Vector3 position = base.transform.TransformPoint(this.drawOffsetOrigin);
				MyDebug.DrawWireCube(position, 0.01f, Color.blue);
				this.drawOffsetMatrix.SetTRS(this.drawOffset, q2, this.drawOffsetScale * this.drawOffsetOverallScale);
				this.drawOffsetOriginMatrix1.SetTRS(this.drawOffsetOrigin, this.identityQuaternion, this.oneVector);
				this.drawOffsetOriginMatrix2.SetTRS(-this.drawOffsetOrigin, this.identityQuaternion, this.oneVector);
				matrix4x = base.transform.localToWorldMatrix * this.drawOffsetOriginMatrix1 * this.drawOffsetMatrix * this.drawOffsetOriginMatrix2;
			}
			else
			{
				matrix4x = base.transform.localToWorldMatrix;
			}
			if (this.delayDisplayFrameCount == 2)
			{
				Matrix4x4 matrix4x2 = this.lastMatrix2;
				this.lastMatrix2 = this.lastMatrix1;
				this.lastMatrix1 = matrix4x;
				matrix4x = matrix4x2;
			}
			else if (this.delayDisplayFrameCount == 1)
			{
				Matrix4x4 matrix4x3 = this.lastMatrix1;
				this.lastMatrix1 = matrix4x;
				matrix4x = matrix4x3;
			}
			if (this.drawBaseMesh && this.simpleMaterial != null)
			{
				for (int i = 0; i < this.baseMesh.subMeshCount; i++)
				{
					Graphics.DrawMesh(this.baseMesh, matrix4x, this.simpleMaterial, 0, null, i, null, this.castShadows, this.receiveShadows);
				}
			}
			if (this.drawMorphedBaseMesh && this.simpleMaterial != null)
			{
				for (int j = 0; j < this._morphedBaseMesh.subMeshCount; j++)
				{
					Graphics.DrawMesh(this._morphedBaseMesh, matrix4x, this.simpleMaterial, 0, null, j, null, this.castShadows, this.receiveShadows);
				}
			}
			if (this.debugGrafting && this.meshGraft != null && this.graftTo)
			{
				Vector3[] normals = this.baseMesh.normals;
				foreach (DAZMeshGraftVertexPair dazmeshGraftVertexPair in this.meshGraft.vertexPairs)
				{
					Vector3 point = this.graftTo.morphedUVVertices[dazmeshGraftVertexPair.graftToVertexNum];
					Vector3 vector = matrix4x.MultiplyPoint(point);
					Vector3 end = vector + normals[dazmeshGraftVertexPair.vertexNum] * 0.01f;
					Debug.DrawLine(vector, end, Color.red);
				}
			}
			if (this.drawUVMappedMesh)
			{
				for (int l = 0; l < this.uvMappedMesh.subMeshCount; l++)
				{
					Material material = null;
					if (this.materials != null)
					{
						material = this.materials[l];
					}
					if (material == null || this.useSimpleMaterial)
					{
						material = this.simpleMaterial;
					}
					if (material != null && this.materialsEnabled[l])
					{
						Graphics.DrawMesh(this.uvMappedMesh, matrix4x, material, 0, null, l, null, this.castShadows && this.materialsShadowCastEnabled[l], this.receiveShadows);
					}
				}
			}
			if (this._drawMorphedUVMappedMesh || (this.drawInEditorWhenNotPlaying && !Application.isPlaying))
			{
				this.DrawMorphedUVMappedMesh(matrix4x);
			}
			if (this._drawVisibleMorphedUVMappedMesh)
			{
				for (int m = 0; m < this._visibleMorphedUVMappedMesh.subMeshCount; m++)
				{
					Material material2 = null;
					if (this.materials != null)
					{
						material2 = this.materials[m];
					}
					if (material2 == null || this.useSimpleMaterial)
					{
						material2 = this.simpleMaterial;
					}
					if (material2 != null && this.materialsEnabled[m])
					{
						Graphics.DrawMesh(this._visibleMorphedUVMappedMesh, matrix4x, material2, 0, null, m, null, this.castShadows && this.materialsShadowCastEnabled[m], this.receiveShadows);
					}
				}
			}
		}
	}

	// Token: 0x06004A7B RID: 19067 RVA: 0x00181E4C File Offset: 0x0018024C
	public virtual void InitMaterials()
	{
		if (Application.isPlaying && !this._materialsWereInit)
		{
			this._materialsWereInit = true;
			if (this.materials != null)
			{
				for (int i = 0; i < this.materials.Length; i++)
				{
					if (this.materials[i] != null)
					{
						Material material = new Material(this.materials[i]);
						base.RegisterAllocatedObject(material);
						this.materials[i] = material;
					}
				}
				if (this.materialsShadowCastEnabled == null || this.materialsShadowCastEnabled.Length != this.materials.Length)
				{
					this.materialsShadowCastEnabled = new bool[this.materials.Length];
					for (int j = 0; j < this.materials.Length; j++)
					{
						this.materialsShadowCastEnabled[j] = true;
					}
				}
			}
		}
	}

	// Token: 0x17000A92 RID: 2706
	// (get) Token: 0x06004A7C RID: 19068 RVA: 0x00181F1F File Offset: 0x0018031F
	public bool wasInit
	{
		get
		{
			return this._wasInit;
		}
	}

	// Token: 0x06004A7D RID: 19069 RVA: 0x00181F27 File Offset: 0x00180327
	public virtual void Init()
	{
		if (!this._wasInit && this.baseVertices != null)
		{
			this._wasInit = true;
			this.DeriveMeshes();
			this.InitMeshFilterAndRenderer();
			this.InitCollider();
		}
	}

	// Token: 0x06004A7E RID: 19070 RVA: 0x00181F58 File Offset: 0x00180358
	public void SetMorphedUVMeshVertices(Vector3[] verts)
	{
		this._morphedUVMappedMesh.vertices = verts;
	}

	// Token: 0x06004A7F RID: 19071 RVA: 0x00181F66 File Offset: 0x00180366
	public void ConnectMorphBank()
	{
		if (this.morphBank != null)
		{
			this.morphBank.connectedMesh = this;
		}
	}

	// Token: 0x06004A80 RID: 19072 RVA: 0x00181F88 File Offset: 0x00180388
	private void LateUpdate()
	{
		if (this.debug && this.debugVertex < this._numUVVertices)
		{
			MyDebug.DrawWireCube(this._morphedUVVertices[this.debugVertex], 0.01f, Color.blue);
		}
		this.Draw();
	}

	// Token: 0x06004A81 RID: 19073 RVA: 0x00181FDC File Offset: 0x001803DC
	private void OnDisable()
	{
		if (!Application.isPlaying)
		{
			this._wasInit = false;
		}
	}

	// Token: 0x06004A82 RID: 19074 RVA: 0x00181FEF File Offset: 0x001803EF
	private void OnEnable()
	{
		this.Init();
		this.ConnectMorphBank();
	}

	// Token: 0x06004A83 RID: 19075 RVA: 0x00181FFD File Offset: 0x001803FD
	private void Awake()
	{
		this.InitMaterials();
	}

	// Token: 0x06004A84 RID: 19076 RVA: 0x00182005 File Offset: 0x00180405
	private void Start()
	{
		this.Init();
	}

	// Token: 0x040038DD RID: 14557
	protected const float geoScale = 0.01f;

	// Token: 0x040038DE RID: 14558
	public bool drawBaseMesh;

	// Token: 0x040038DF RID: 14559
	[SerializeField]
	protected bool _drawMorphedBaseMesh;

	// Token: 0x040038E0 RID: 14560
	public bool drawUVMappedMesh;

	// Token: 0x040038E1 RID: 14561
	[SerializeField]
	protected bool _drawMorphedUVMappedMesh;

	// Token: 0x040038E2 RID: 14562
	public bool drawInEditorWhenNotPlaying;

	// Token: 0x040038E3 RID: 14563
	protected bool _renderSuspend;

	// Token: 0x040038E4 RID: 14564
	public bool recalcNormalsOnMorph;

	// Token: 0x040038E5 RID: 14565
	public bool recalcTangentsOnMorph;

	// Token: 0x040038E6 RID: 14566
	public bool useUnityRecalcNormals;

	// Token: 0x040038E7 RID: 14567
	[SerializeField]
	protected bool _useSmoothing;

	// Token: 0x040038E8 RID: 14568
	public bool useSimpleMaterial;

	// Token: 0x040038E9 RID: 14569
	public Material simpleMaterial;

	// Token: 0x040038EA RID: 14570
	public Material discardMaterial;

	// Token: 0x040038EB RID: 14571
	public Material[] materials;

	// Token: 0x040038EC RID: 14572
	public bool use2PassMaterials;

	// Token: 0x040038ED RID: 14573
	public Material[] materialsPass1;

	// Token: 0x040038EE RID: 14574
	public Material[] materialsPass2;

	// Token: 0x040038EF RID: 14575
	public bool[] materialsEnabled;

	// Token: 0x040038F0 RID: 14576
	public bool[] materialsPass1Enabled;

	// Token: 0x040038F1 RID: 14577
	public bool[] materialsPass2Enabled;

	// Token: 0x040038F2 RID: 14578
	public bool[] materialsShadowCastEnabled;

	// Token: 0x040038F3 RID: 14579
	public DAZMesh copyMaterialsFrom;

	// Token: 0x040038F4 RID: 14580
	public bool castShadows = true;

	// Token: 0x040038F5 RID: 14581
	public bool receiveShadows = true;

	// Token: 0x040038F6 RID: 14582
	public string nodeId;

	// Token: 0x040038F7 RID: 14583
	public string sceneNodeId;

	// Token: 0x040038F8 RID: 14584
	public string geometryId;

	// Token: 0x040038F9 RID: 14585
	public string overrideGeometryId;

	// Token: 0x040038FA RID: 14586
	public string sceneGeometryId;

	// Token: 0x040038FB RID: 14587
	[SerializeField]
	protected int _numBaseVertices;

	// Token: 0x040038FC RID: 14588
	[SerializeField]
	protected int _numBasePolygons;

	// Token: 0x040038FD RID: 14589
	[SerializeField]
	protected int _numUVVertices;

	// Token: 0x040038FE RID: 14590
	[SerializeField]
	protected int _numMaterials;

	// Token: 0x040038FF RID: 14591
	[SerializeField]
	protected string[] _materialNames;

	// Token: 0x04003900 RID: 14592
	public DAZMorphBank morphBank;

	// Token: 0x04003901 RID: 14593
	public float vertexNormalOffset;

	// Token: 0x04003902 RID: 14594
	public Vector3 vertexOffset = Vector3.zero;

	// Token: 0x04003903 RID: 14595
	protected bool _verticesChangedLastFrame;

	// Token: 0x04003904 RID: 14596
	protected bool _visibleNonPoseVerticesChangedLastFrame;

	// Token: 0x04003905 RID: 14597
	protected bool _verticesChangedThisFrame;

	// Token: 0x04003906 RID: 14598
	protected bool _visibleNonPoseVerticesChangedThisFrame;

	// Token: 0x04003907 RID: 14599
	private bool _normalsDirtyThisFrame;

	// Token: 0x04003908 RID: 14600
	private bool _tangentsDirtyThisFrame;

	// Token: 0x04003909 RID: 14601
	protected Mesh _baseMesh;

	// Token: 0x0400390A RID: 14602
	[SerializeField]
	protected DAZMeshData _meshData;

	// Token: 0x0400390B RID: 14603
	[SerializeField]
	protected Vector3[] _baseVertices;

	// Token: 0x0400390C RID: 14604
	protected Vector3[] _baseNormals;

	// Token: 0x0400390D RID: 14605
	protected Vector3[] _baseSurfaceNormals;

	// Token: 0x0400390E RID: 14606
	[SerializeField]
	protected MeshPoly[] _basePolyList;

	// Token: 0x0400390F RID: 14607
	protected int[] _baseTriangles;

	// Token: 0x04003910 RID: 14608
	protected int[][] _baseMaterialVertices;

	// Token: 0x04003911 RID: 14609
	protected Mesh _morphedBaseMesh;

	// Token: 0x04003912 RID: 14610
	protected Vector3[] _morphedBaseVertices;

	// Token: 0x04003913 RID: 14611
	protected Vector3[] _morphedBaseNormals;

	// Token: 0x04003914 RID: 14612
	protected Vector3[] _morphedBaseSurfaceNormals;

	// Token: 0x04003915 RID: 14613
	public bool debugGrafting;

	// Token: 0x04003916 RID: 14614
	public DAZMeshGraft meshGraft;

	// Token: 0x04003917 RID: 14615
	public DAZMesh graftTo;

	// Token: 0x04003918 RID: 14616
	public bool debug;

	// Token: 0x04003919 RID: 14617
	public int debugVertex;

	// Token: 0x0400391A RID: 14618
	[SerializeField]
	protected DAZVertexMap[] _baseVerticesToUVVertices;

	// Token: 0x0400391B RID: 14619
	protected Mesh _uvMappedMesh;

	// Token: 0x0400391C RID: 14620
	[SerializeField]
	protected Vector3[] _UVVertices;

	// Token: 0x0400391D RID: 14621
	protected Vector3[] _UVNormals;

	// Token: 0x0400391E RID: 14622
	protected Vector4[] _UVTangents;

	// Token: 0x0400391F RID: 14623
	[SerializeField]
	protected MeshPoly[] _UVPolyList;

	// Token: 0x04003920 RID: 14624
	protected int[] _UVTriangles;

	// Token: 0x04003921 RID: 14625
	protected Vector2[] _UV;

	// Token: 0x04003922 RID: 14626
	[SerializeField]
	protected Vector2[] _OrigUV;

	// Token: 0x04003923 RID: 14627
	[SerializeField]
	protected bool _usePatches;

	// Token: 0x04003924 RID: 14628
	public int numUVPatches;

	// Token: 0x04003925 RID: 14629
	public DAZUVPatch[] UVPatches;

	// Token: 0x04003926 RID: 14630
	protected MeshSmooth meshSmooth;

	// Token: 0x04003927 RID: 14631
	protected Mesh _morphedUVMappedMesh;

	// Token: 0x04003928 RID: 14632
	protected Mesh _visibleMorphedUVMappedMesh;

	// Token: 0x04003929 RID: 14633
	[SerializeField]
	protected bool _drawVisibleMorphedUVMappedMesh;

	// Token: 0x0400392A RID: 14634
	protected Vector3[] _visibleMorphedUVVertices;

	// Token: 0x0400392B RID: 14635
	protected Vector3[] _morphedUVVertices;

	// Token: 0x0400392C RID: 14636
	protected Vector3[] _smoothedMorphedUVVertices;

	// Token: 0x0400392D RID: 14637
	protected bool[] _morphedBaseDirtyVertices;

	// Token: 0x0400392E RID: 14638
	protected bool[] _morphedUVDirtyVertices;

	// Token: 0x0400392F RID: 14639
	public bool morphedNormalsDirty;

	// Token: 0x04003930 RID: 14640
	public bool morphedTangentsDirty;

	// Token: 0x04003931 RID: 14641
	protected Vector3[] _morphedUVNormals;

	// Token: 0x04003932 RID: 14642
	protected Vector4[] _morphedUVTangents;

	// Token: 0x04003933 RID: 14643
	[SerializeField]
	protected bool _createMeshFilterAndRenderer;

	// Token: 0x04003934 RID: 14644
	[SerializeField]
	protected bool _createMeshCollider;

	// Token: 0x04003935 RID: 14645
	[SerializeField]
	protected bool _useConvexCollider;

	// Token: 0x04003936 RID: 14646
	protected bool useUnityMaterialOrdering;

	// Token: 0x04003937 RID: 14647
	public string shaderNameForDynamicLoad = "Custom/Subsurface/TransparentGlossNMNoCullSeparateAlpha";

	// Token: 0x04003938 RID: 14648
	public Vector3 reducePoint;

	// Token: 0x04003939 RID: 14649
	public Vector3 reduceUp;

	// Token: 0x0400393A RID: 14650
	public string assetSavePath = "Assets/VaMAssets/Generated/";

	// Token: 0x0400393B RID: 14651
	protected bool _meshRendererMaterialsWasInit;

	// Token: 0x0400393C RID: 14652
	public bool useDrawOffset;

	// Token: 0x0400393D RID: 14653
	public Vector3 drawOffset;

	// Token: 0x0400393E RID: 14654
	public Vector3 drawOffsetOrigin;

	// Token: 0x0400393F RID: 14655
	public Vector3 drawOffsetRotation;

	// Token: 0x04003940 RID: 14656
	public float drawOffsetOverallScale = 1f;

	// Token: 0x04003941 RID: 14657
	public Vector3 drawOffsetScale = Vector3.one;

	// Token: 0x04003942 RID: 14658
	protected Matrix4x4 lastMatrix1 = Matrix4x4.identity;

	// Token: 0x04003943 RID: 14659
	protected Matrix4x4 lastMatrix2 = Matrix4x4.identity;

	// Token: 0x04003944 RID: 14660
	protected Matrix4x4 identityMatrix = Matrix4x4.identity;

	// Token: 0x04003945 RID: 14661
	protected Matrix4x4 drawOffsetMatrix = Matrix4x4.identity;

	// Token: 0x04003946 RID: 14662
	protected Matrix4x4 drawOffsetOriginMatrix1 = Matrix4x4.identity;

	// Token: 0x04003947 RID: 14663
	protected Matrix4x4 drawOffsetOriginMatrix2 = Matrix4x4.identity;

	// Token: 0x04003948 RID: 14664
	protected Quaternion identityQuaternion = Quaternion.identity;

	// Token: 0x04003949 RID: 14665
	protected Vector3 oneVector = Vector3.one;

	// Token: 0x0400394A RID: 14666
	public DAZBone drawFromBone;

	// Token: 0x0400394B RID: 14667
	public DAZMesh.BoneSide boneSide;

	// Token: 0x0400394C RID: 14668
	public int delayDisplayFrameCount;

	// Token: 0x0400394D RID: 14669
	protected bool _materialsWereInit;

	// Token: 0x0400394E RID: 14670
	protected bool _wasInit;

	// Token: 0x02000AE5 RID: 2789
	public enum BoneSide
	{
		// Token: 0x04003950 RID: 14672
		Both,
		// Token: 0x04003951 RID: 14673
		Right,
		// Token: 0x04003952 RID: 14674
		Left,
		// Token: 0x04003953 RID: 14675
		None
	}
}
