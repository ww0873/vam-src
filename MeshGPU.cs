using System;
using UnityEngine;

// Token: 0x02000CA3 RID: 3235
public class MeshGPU
{
	// Token: 0x0600614F RID: 24911 RVA: 0x00252A88 File Offset: 0x00250E88
	public MeshGPU(ComputeShader cs, Mesh m, Vector3[] vertices, int[] triangles, VertexMap[] vertMap, MeshPoly[] polys = null)
	{
		this.mesh = m;
		this.meshSmooth = new MeshSmoothGPU(cs, vertices, polys);
		this.recalcNormals = new RecalculateNormalsGPU(cs, triangles, vertices.Length, vertMap);
		this._surfaceNormalsBuffer = this.recalcNormals.surfaceNormalsBuffer;
		this._normalsBuffer = this.recalcNormals.normalsBuffer;
		this.numVertThreadGroups = vertices.Length / 256;
		int num = vertices.Length % 256;
		if (num != 0)
		{
			this.numVertThreadGroups++;
		}
	}

	// Token: 0x17000E50 RID: 3664
	// (get) Token: 0x06006150 RID: 24912 RVA: 0x00252B21 File Offset: 0x00250F21
	public int numMaterials
	{
		get
		{
			return this._numMaterials;
		}
	}

	// Token: 0x17000E51 RID: 3665
	// (get) Token: 0x06006151 RID: 24913 RVA: 0x00252B29 File Offset: 0x00250F29
	public string[] materialNames
	{
		get
		{
			return this._materialNames;
		}
	}

	// Token: 0x06006152 RID: 24914 RVA: 0x00252B34 File Offset: 0x00250F34
	public void CopyMaterials(Material[] materials, string[] matNames = null, Material simpleMaterial = null, bool[] enabled = null)
	{
		this._numMaterials = materials.Length;
		if (simpleMaterial != null)
		{
			this.GPUsimpleMaterial = simpleMaterial;
		}
		this.GPUmaterials = new Material[this._numMaterials];
		this.materialsEnabled = new bool[this._numMaterials];
		this._materialNames = new string[this._numMaterials];
		for (int i = 0; i < this._numMaterials; i++)
		{
			this.GPUmaterials[i] = materials[i];
			if (enabled != null)
			{
				this.materialsEnabled[i] = enabled[i];
			}
			if (matNames != null)
			{
				this._materialNames[i] = matNames[i];
			}
		}
	}

	// Token: 0x17000E52 RID: 3666
	// (get) Token: 0x06006153 RID: 24915 RVA: 0x00252BD6 File Offset: 0x00250FD6
	public ComputeBuffer normalsBuffer
	{
		get
		{
			return this._normalsBuffer;
		}
	}

	// Token: 0x17000E53 RID: 3667
	// (get) Token: 0x06006154 RID: 24916 RVA: 0x00252BDE File Offset: 0x00250FDE
	public ComputeBuffer surfaceNormalsBuffer
	{
		get
		{
			return this._surfaceNormalsBuffer;
		}
	}

	// Token: 0x06006155 RID: 24917 RVA: 0x00252BE6 File Offset: 0x00250FE6
	public void VerticesUpdated(ComputeBuffer vertsBuffer)
	{
		this.recalcNormals.RecalculateNormals(vertsBuffer);
	}

	// Token: 0x06006156 RID: 24918 RVA: 0x00252BF4 File Offset: 0x00250FF4
	public void Draw(ComputeBuffer vertsBuffer)
	{
		Matrix4x4 identity = Matrix4x4.identity;
		for (int i = 0; i < this._numMaterials; i++)
		{
			if (this.GPUuseSimpleMaterial && this.GPUsimpleMaterial)
			{
				this.GPUsimpleMaterial.SetBuffer("verts", vertsBuffer);
				this.GPUsimpleMaterial.SetBuffer("normals", this.normalsBuffer);
				Graphics.DrawMesh(this.mesh, identity, this.GPUsimpleMaterial, 0, null, i, null, this.castShadows, this.receiveShadows);
			}
			else if (this.materialsEnabled[i] && this.GPUmaterials[i] != null)
			{
				this.GPUmaterials[i].SetBuffer("verts", this.smoothedVertsBuffer);
				this.GPUmaterials[i].SetBuffer("normals", this.normalsBuffer);
				Graphics.DrawMesh(this.mesh, identity, this.GPUmaterials[i], 0, null, i, null, this.castShadows, this.receiveShadows);
			}
		}
	}

	// Token: 0x04005141 RID: 20801
	public ComputeShader GPUMeshCompute;

	// Token: 0x04005142 RID: 20802
	protected Mesh mesh;

	// Token: 0x04005143 RID: 20803
	protected Vector3[] _vertices;

	// Token: 0x04005144 RID: 20804
	protected int[] _triangles;

	// Token: 0x04005145 RID: 20805
	public bool castShadows = true;

	// Token: 0x04005146 RID: 20806
	public bool receiveShadows = true;

	// Token: 0x04005147 RID: 20807
	protected const int vertGroupSize = 256;

	// Token: 0x04005148 RID: 20808
	protected const int baseVertToUVVertGroupSize = 256;

	// Token: 0x04005149 RID: 20809
	protected int numVertThreadGroups;

	// Token: 0x0400514A RID: 20810
	protected int numBaseVertToUVVertThreadGroups;

	// Token: 0x0400514B RID: 20811
	protected ComputeBuffer _wrapVerticesBuffer;

	// Token: 0x0400514C RID: 20812
	protected MeshGPU.Triangle[] _skinToTrianglesStruct;

	// Token: 0x0400514D RID: 20813
	protected ComputeBuffer _skinToTrianglesBuffer;

	// Token: 0x0400514E RID: 20814
	protected MeshGPU.BaseVertToUVVert[] baseVertToUVVerts;

	// Token: 0x0400514F RID: 20815
	protected ComputeBuffer baseVertToUVVertsBuffer;

	// Token: 0x04005150 RID: 20816
	protected int _smoothKernel;

	// Token: 0x04005151 RID: 20817
	protected int _hcp1Kernel;

	// Token: 0x04005152 RID: 20818
	protected int _hcp2Kernel;

	// Token: 0x04005153 RID: 20819
	protected int _baseVertsToUVVertsKernel;

	// Token: 0x04005154 RID: 20820
	protected int _recalcSurfaceNormalsKernel;

	// Token: 0x04005155 RID: 20821
	protected int _recalcVertexNormalsKernel;

	// Token: 0x04005156 RID: 20822
	protected int _baseNormalsToUVNormalsKernel;

	// Token: 0x04005157 RID: 20823
	public bool showMaterials;

	// Token: 0x04005158 RID: 20824
	public bool GPUuseSimpleMaterial;

	// Token: 0x04005159 RID: 20825
	public Material GPUsimpleMaterial;

	// Token: 0x0400515A RID: 20826
	public Material[] GPUmaterials;

	// Token: 0x0400515B RID: 20827
	public bool[] materialsEnabled;

	// Token: 0x0400515C RID: 20828
	[SerializeField]
	protected int _numMaterials;

	// Token: 0x0400515D RID: 20829
	[SerializeField]
	protected string[] _materialNames;

	// Token: 0x0400515E RID: 20830
	protected MeshSmoothGPU meshSmooth;

	// Token: 0x0400515F RID: 20831
	protected RecalculateNormalsGPU recalcNormals;

	// Token: 0x04005160 RID: 20832
	public ComputeBuffer smoothedVertsBuffer;

	// Token: 0x04005161 RID: 20833
	protected ComputeBuffer _normalsBuffer;

	// Token: 0x04005162 RID: 20834
	protected ComputeBuffer _surfaceNormalsBuffer;

	// Token: 0x04005163 RID: 20835
	protected int _nullVertexIndex;

	// Token: 0x02000CA4 RID: 3236
	protected struct BaseVertToUVVert
	{
		// Token: 0x04005164 RID: 20836
		public int baseVertex;

		// Token: 0x04005165 RID: 20837
		public int UVVertex;
	}

	// Token: 0x02000CA5 RID: 3237
	protected struct Triangle
	{
		// Token: 0x04005166 RID: 20838
		public int vert1;

		// Token: 0x04005167 RID: 20839
		public int vert2;

		// Token: 0x04005168 RID: 20840
		public int vert3;
	}
}
