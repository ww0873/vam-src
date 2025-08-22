using System;
using UnityEngine;

// Token: 0x02000C84 RID: 3204
[AddComponentMenu("My Scripts/Geometry/BakedSkinMesh")]
public class BakedSkinMesh : MonoBehaviour
{
	// Token: 0x0600609D RID: 24733 RVA: 0x0024853D File Offset: 0x0024693D
	public BakedSkinMesh()
	{
	}

	// Token: 0x17000E3F RID: 3647
	// (get) Token: 0x0600609E RID: 24734 RVA: 0x00248568 File Offset: 0x00246968
	public Mesh BakedMesh
	{
		get
		{
			return this._mesh;
		}
	}

	// Token: 0x17000E40 RID: 3648
	// (get) Token: 0x0600609F RID: 24735 RVA: 0x00248570 File Offset: 0x00246970
	public Mesh OriginalMesh
	{
		get
		{
			return this._originalMesh;
		}
	}

	// Token: 0x060060A0 RID: 24736 RVA: 0x00248578 File Offset: 0x00246978
	private void BakeMesh()
	{
		if (this.smr)
		{
			if (this._alignType != this.alignType)
			{
				this._alignType = this.alignType;
				this.InitSkinnedMeshVars();
			}
			if (this.useUnityBake)
			{
				this.smr.BakeMesh(this._mesh);
			}
			else
			{
				this.SelfBake1();
			}
		}
	}

	// Token: 0x060060A1 RID: 24737 RVA: 0x002485E0 File Offset: 0x002469E0
	private void DrawMesh()
	{
		if (this.draw)
		{
			if (this.smr)
			{
				Matrix4x4 localToWorldMatrix = base.transform.localToWorldMatrix;
				Graphics.DrawMesh(this._mesh, localToWorldMatrix, this.smr.sharedMaterial, 0, null, 0, null, this.smr.shadowCastingMode, this.smr.receiveShadows);
			}
			else if (this.mf && this.mr)
			{
				Matrix4x4 localToWorldMatrix2 = base.transform.localToWorldMatrix;
				Graphics.DrawMesh(this._mesh, localToWorldMatrix2, this.mr.sharedMaterial, 0, null, 0, null, this.mr.shadowCastingMode, this.mr.receiveShadows);
			}
		}
	}

	// Token: 0x060060A2 RID: 24738 RVA: 0x002486A8 File Offset: 0x00246AA8
	private void DrawDebug(Matrix4x4 m, Material mat)
	{
		if (this.debugMesh != null && mat != null)
		{
			Graphics.DrawMesh(this.debugMesh, m, mat, 0, null, 0, null, false, false);
		}
	}

	// Token: 0x060060A3 RID: 24739 RVA: 0x002486E8 File Offset: 0x00246AE8
	private void SelfBake1()
	{
		bool flag = true;
		Vector3 zero = Vector3.zero;
		Vector3 zero2 = Vector3.zero;
		for (int i = 0; i < this.numBones; i++)
		{
			this.boneMatrices[i] = this.boneTransforms[i].localToWorldMatrix * this.bindposes[i];
			if (this.debugBone != null && this.debugBone != string.Empty && this.boneTransforms[i].name == this.debugBone)
			{
				this.DrawDebug(this.boneMatrices[i], this.debugMaterial1);
				this.DrawDebug(this.boneTransforms[i].localToWorldMatrix, this.debugMaterial2);
			}
		}
		for (int j = 0; j < this.numVertices; j++)
		{
			int boneIndex = this.boneWeights[j].boneIndex0;
			int boneIndex2 = this.boneWeights[j].boneIndex1;
			int boneIndex3 = this.boneWeights[j].boneIndex2;
			int boneIndex4 = this.boneWeights[j].boneIndex3;
			float weight = this.boneWeights[j].weight0;
			float weight2 = this.boneWeights[j].weight1;
			float weight3 = this.boneWeights[j].weight2;
			float weight4 = this.boneWeights[j].weight3;
			for (int k = 0; k < 16; k++)
			{
				this.vertexMatrix[k] = this.boneMatrices[boneIndex][k] * weight + this.boneMatrices[boneIndex2][k] * weight2 + this.boneMatrices[boneIndex3][k] * weight3 + this.boneMatrices[boneIndex4][k] * weight4;
			}
			this.bakedVertices[j] = this.vertexMatrix.MultiplyPoint3x4(this.originalVertices[j]);
			if (flag)
			{
				flag = false;
				zero.x = this.bakedVertices[j].x;
				zero2.x = this.bakedVertices[j].x;
				zero.y = this.bakedVertices[j].y;
				zero2.y = this.bakedVertices[j].y;
				zero.z = this.bakedVertices[j].z;
				zero2.z = this.bakedVertices[j].z;
			}
			else
			{
				if (this.bakedVertices[j].x < zero.x)
				{
					zero.x = this.bakedVertices[j].x;
				}
				else if (this.bakedVertices[j].x > zero2.x)
				{
					zero2.x = this.bakedVertices[j].x;
				}
				if (this.bakedVertices[j].y < zero.y)
				{
					zero.y = this.bakedVertices[j].y;
				}
				else if (this.bakedVertices[j].y > zero2.y)
				{
					zero2.y = this.bakedVertices[j].y;
				}
				if (this.bakedVertices[j].z < zero.z)
				{
					zero.z = this.bakedVertices[j].z;
				}
				else if (this.bakedVertices[j].z > zero2.z)
				{
					zero2.z = this.bakedVertices[j].z;
				}
			}
			Vector3 vector = this.vertexMatrix.MultiplyVector(this.originalNormals[j]);
			float num = 1f / vector.magnitude;
			vector.x *= num;
			vector.y *= num;
			vector.z *= num;
			this.bakedNormals[j] = vector;
			Vector3 vector2;
			vector2.x = this.originalTangents[j].x;
			vector2.y = this.originalTangents[j].y;
			vector2.z = this.originalTangents[j].z;
			Vector3 vector3 = this.vertexMatrix.MultiplyVector(vector2);
			num = 1f / vector3.magnitude;
			this.bakedTangents[j].x = vector3.x * num;
			this.bakedTangents[j].y = vector3.y * num;
			this.bakedTangents[j].z = vector3.z * num;
			this.bakedTangents[j].w = this.originalTangents[j].w;
		}
		this._mesh.vertices = this.bakedVertices;
		this._mesh.normals = this.bakedNormals;
		this._mesh.tangents = this.bakedTangents;
		Bounds bounds = default(Bounds);
		bounds.min = zero;
		bounds.max = zero2;
		this._mesh.bounds = bounds;
	}

	// Token: 0x060060A4 RID: 24740 RVA: 0x00248CE0 File Offset: 0x002470E0
	private void SelfBake2()
	{
		for (int i = 0; i < this.numBones; i++)
		{
			this.boneMatrices[i] = this.boneTransforms[i].localToWorldMatrix * this.bindposes[i];
		}
		for (int j = 0; j < this.numVertices; j++)
		{
			this.bakedVertices[j] = Vector3.zero;
		}
		for (int k = 0; k < this.numVertices; k++)
		{
			BoneWeight boneWeight = this.boneWeights[k];
			if (boneWeight.weight0 != 0f)
			{
				this.bakedVertices[k] += this.boneMatrices[boneWeight.boneIndex0].MultiplyPoint3x4(this.originalVertices[k]) * boneWeight.weight0;
			}
			if (boneWeight.weight1 != 0f)
			{
				this.bakedVertices[k] += this.boneMatrices[boneWeight.boneIndex1].MultiplyPoint3x4(this.originalVertices[k]) * boneWeight.weight1;
			}
			if (boneWeight.weight2 != 0f)
			{
				this.bakedVertices[k] += this.boneMatrices[boneWeight.boneIndex2].MultiplyPoint3x4(this.originalVertices[k]) * boneWeight.weight2;
			}
			if (boneWeight.weight3 != 0f)
			{
				this.bakedVertices[k] += this.boneMatrices[boneWeight.boneIndex3].MultiplyPoint3x4(this.originalVertices[k]) * boneWeight.weight3;
			}
		}
		this._mesh.vertices = this.bakedVertices;
	}

	// Token: 0x060060A5 RID: 24741 RVA: 0x00248F24 File Offset: 0x00247324
	private void InitSkinnedMeshVars()
	{
		this.numBones = this.smr.bones.Length;
		this.bindposes = this._originalMesh.bindposes;
		this.boneWeights = this._originalMesh.boneWeights;
		this.boneTransforms = new Transform[this.numBones];
		this.originalVertices = this._originalMesh.vertices;
		this.numVertices = this.originalVertices.Length;
		this.originalNormals = this._originalMesh.normals;
		this.originalTangents = this._originalMesh.tangents;
		this.bakedVertices = new Vector3[this.numVertices];
		this.bakedNormals = new Vector3[this.numVertices];
		this.bakedTangents = new Vector4[this.numVertices];
		this.boneMatrices = new Matrix4x4[this.numBones];
		this.vertexMatrix = default(Matrix4x4);
		for (int i = 0; i < this.numBones; i++)
		{
			this.boneTransforms[i] = this.smr.bones[i].transform;
			this.boneMatrices[i] = this.boneTransforms[i].localToWorldMatrix;
		}
		if (this.alignTangents)
		{
			Mesh mesh = this.CloneMesh(this._originalMesh);
			Vector4 vector;
			vector.w = 1f;
			Vector3 vector2 = Vector3.zero;
			Vector3 vector3 = Vector3.zero;
			if (this._alignType == BakedSkinMesh.AlignType.forward)
			{
				vector2 = Vector3.forward;
				vector3 = Vector3.right;
			}
			else if (this._alignType == BakedSkinMesh.AlignType.right)
			{
				vector2 = Vector3.right;
				vector3 = Vector3.up;
			}
			else if (this._alignType == BakedSkinMesh.AlignType.up)
			{
				vector2 = Vector3.up;
				vector3 = Vector3.forward;
			}
			for (int j = 0; j < this.numVertices; j++)
			{
				vector.x = this.originalNormals[j].y * vector2.z - this.originalNormals[j].z * vector2.y;
				vector.y = this.originalNormals[j].z * vector2.x - this.originalNormals[j].x * vector2.z;
				vector.z = this.originalNormals[j].x * vector2.y - this.originalNormals[j].y * vector2.x;
				float num = 1f / vector.magnitude;
				vector.x *= num;
				vector.y *= num;
				vector.z *= num;
				if (vector.sqrMagnitude <= 0.5f)
				{
					vector.x = this.originalNormals[j].y * vector3.z - this.originalNormals[j].z * vector3.y;
					vector.y = this.originalNormals[j].z * vector3.x - this.originalNormals[j].x * vector3.z;
					vector.z = this.originalNormals[j].x * vector3.y - this.originalNormals[j].y * vector3.x;
					num = 1f / vector.magnitude;
					vector.x *= num;
					vector.y *= num;
					vector.z *= num;
				}
				this.originalTangents[j] = vector;
			}
			mesh.tangents = this.originalTangents;
			this.smr.sharedMesh = mesh;
		}
	}

	// Token: 0x060060A6 RID: 24742 RVA: 0x00249320 File Offset: 0x00247720
	private Mesh CloneMesh(Mesh inMesh)
	{
		return UnityEngine.Object.Instantiate<Mesh>(inMesh);
	}

	// Token: 0x060060A7 RID: 24743 RVA: 0x00249338 File Offset: 0x00247738
	private void Start()
	{
		this.smr = base.GetComponent<SkinnedMeshRenderer>();
		this._alignType = this.alignType;
		if (this.smr)
		{
			this._originalMesh = this.smr.sharedMesh;
			this._mesh = this.CloneMesh(this._originalMesh);
			this._mesh.MarkDynamic();
			this.InitSkinnedMeshVars();
		}
		else
		{
			this.mr = base.GetComponent<MeshRenderer>();
			this.mf = base.GetComponent<MeshFilter>();
			if (this.mf)
			{
				this._mesh = this.mf.sharedMesh;
				this._originalMesh = this._mesh;
			}
		}
		this.BakeMesh();
	}

	// Token: 0x060060A8 RID: 24744 RVA: 0x002493F1 File Offset: 0x002477F1
	private void LateUpdate()
	{
		if (this.on)
		{
			base.transform.position = Vector3.zero;
			base.transform.rotation = Quaternion.identity;
			this.BakeMesh();
			this.DrawMesh();
		}
	}

	// Token: 0x0400502E RID: 20526
	public bool on = true;

	// Token: 0x0400502F RID: 20527
	public bool draw = true;

	// Token: 0x04005030 RID: 20528
	public bool useUnityBake = true;

	// Token: 0x04005031 RID: 20529
	public bool alignTangents = true;

	// Token: 0x04005032 RID: 20530
	public BakedSkinMesh.AlignType alignType = BakedSkinMesh.AlignType.forward;

	// Token: 0x04005033 RID: 20531
	private BakedSkinMesh.AlignType _alignType;

	// Token: 0x04005034 RID: 20532
	private Mesh _mesh;

	// Token: 0x04005035 RID: 20533
	private Mesh _originalMesh;

	// Token: 0x04005036 RID: 20534
	public string debugBone;

	// Token: 0x04005037 RID: 20535
	public Mesh debugMesh;

	// Token: 0x04005038 RID: 20536
	public Material debugMaterial1;

	// Token: 0x04005039 RID: 20537
	public Material debugMaterial2;

	// Token: 0x0400503A RID: 20538
	private SkinnedMeshRenderer smr;

	// Token: 0x0400503B RID: 20539
	private MeshFilter mf;

	// Token: 0x0400503C RID: 20540
	private MeshRenderer mr;

	// Token: 0x0400503D RID: 20541
	private int numBones;

	// Token: 0x0400503E RID: 20542
	private Matrix4x4[] bindposes;

	// Token: 0x0400503F RID: 20543
	private BoneWeight[] boneWeights;

	// Token: 0x04005040 RID: 20544
	private Transform[] boneTransforms;

	// Token: 0x04005041 RID: 20545
	private Matrix4x4[] boneMatrices;

	// Token: 0x04005042 RID: 20546
	private Vector3[] originalVertices;

	// Token: 0x04005043 RID: 20547
	private Vector3[] originalNormals;

	// Token: 0x04005044 RID: 20548
	private Vector4[] originalTangents;

	// Token: 0x04005045 RID: 20549
	private int numVertices;

	// Token: 0x04005046 RID: 20550
	private Vector3[] bakedVertices;

	// Token: 0x04005047 RID: 20551
	private Vector3[] bakedNormals;

	// Token: 0x04005048 RID: 20552
	private Vector4[] bakedTangents;

	// Token: 0x04005049 RID: 20553
	private Matrix4x4 vertexMatrix;

	// Token: 0x02000C85 RID: 3205
	public enum AlignType
	{
		// Token: 0x0400504B RID: 20555
		up,
		// Token: 0x0400504C RID: 20556
		right,
		// Token: 0x0400504D RID: 20557
		forward
	}
}
