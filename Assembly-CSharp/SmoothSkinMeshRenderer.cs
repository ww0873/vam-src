using System;
using MVR;
using UnityEngine;

// Token: 0x02000C9F RID: 3231
[ExecuteInEditMode]
public class SmoothSkinMeshRenderer : ObjectAllocator
{
	// Token: 0x06006146 RID: 24902 RVA: 0x00252718 File Offset: 0x00250B18
	public SmoothSkinMeshRenderer()
	{
	}

	// Token: 0x06006147 RID: 24903 RVA: 0x00252720 File Offset: 0x00250B20
	private void Update()
	{
		this.Init();
		if (this.skinnedMeshRenderer != null)
		{
			this.skinnedMeshRenderer.BakeMesh(this.mesh);
			this.inputVerts = this.mesh.vertices;
			this.meshSmooth.LaplacianSmooth(this.inputVerts, this.smoothedVerts, 0, 100000000);
			this.mesh.vertices = this.smoothedVerts;
			for (int i = 0; i < this.mesh.subMeshCount; i++)
			{
				Material material = this.skinnedMeshRenderer.sharedMaterials[i];
				if (material != null)
				{
					Graphics.DrawMesh(this.mesh, base.transform.localToWorldMatrix, material, base.gameObject.layer, null, i, null, this.skinnedMeshRenderer.shadowCastingMode, this.skinnedMeshRenderer.receiveShadows);
				}
			}
		}
	}

	// Token: 0x06006148 RID: 24904 RVA: 0x00252808 File Offset: 0x00250C08
	private void Init()
	{
		if (!this.wasInit && this.skinnedMeshRenderer != null)
		{
			this.wasInit = true;
			this.mesh = UnityEngine.Object.Instantiate<Mesh>(this.skinnedMeshRenderer.sharedMesh);
			base.RegisterAllocatedObject(this.mesh);
			this.smoothedVerts = new Vector3[this.mesh.vertices.Length];
			this.meshSmooth = new MeshSmooth(this.mesh.vertices, this.mesh.triangles);
		}
	}

	// Token: 0x06006149 RID: 24905 RVA: 0x00252893 File Offset: 0x00250C93
	private void OnEnable()
	{
		this.wasInit = false;
		this.Init();
	}

	// Token: 0x0600614A RID: 24906 RVA: 0x002528A2 File Offset: 0x00250CA2
	private void Start()
	{
		this.wasInit = false;
		this.Init();
	}

	// Token: 0x0400512F RID: 20783
	public SkinnedMeshRenderer skinnedMeshRenderer;

	// Token: 0x04005130 RID: 20784
	private Mesh mesh;

	// Token: 0x04005131 RID: 20785
	private MeshSmooth meshSmooth;

	// Token: 0x04005132 RID: 20786
	private MeshRenderer mr;

	// Token: 0x04005133 RID: 20787
	public Vector3[] inputVerts;

	// Token: 0x04005134 RID: 20788
	public Vector3[] smoothedVerts;

	// Token: 0x04005135 RID: 20789
	private bool wasInit;
}
