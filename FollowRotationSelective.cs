using System;
using UnityEngine;

// Token: 0x02000BD4 RID: 3028
public class FollowRotationSelective : MonoBehaviour
{
	// Token: 0x060055EE RID: 21998 RVA: 0x001F6BD2 File Offset: 0x001F4FD2
	public FollowRotationSelective()
	{
	}

	// Token: 0x060055EF RID: 21999 RVA: 0x001F6BDC File Offset: 0x001F4FDC
	private void Start()
	{
		if (this.mesh != null)
		{
			this.bindpose = this.follow.localToWorldMatrix;
			this.drawMesh = new Mesh();
			this.meshVerts = this.mesh.vertices;
			this.numVertices = this.meshVerts.Length;
			this.vertices = new Vector3[this.numVertices];
			for (int i = 0; i < this.numVertices; i++)
			{
				this.vertices[i] = this.bindpose.MultiplyPoint3x4(this.meshVerts[i]);
			}
			this.drawMesh.vertices = this.vertices;
			this.drawMesh.normals = this.mesh.normals;
			this.drawMesh.uv = this.mesh.uv;
			this.drawMesh.triangles = this.mesh.triangles;
		}
	}

	// Token: 0x060055F0 RID: 22000 RVA: 0x001F6CE0 File Offset: 0x001F50E0
	private void SkinMesh()
	{
		Matrix4x4 localToWorldMatrix = this.follow.localToWorldMatrix;
		for (int i = 0; i < this.numVertices; i++)
		{
			this.vertices[i] = localToWorldMatrix.MultiplyPoint3x4(this.meshVerts[i]);
		}
		this.drawMesh.vertices = this.vertices;
	}

	// Token: 0x060055F1 RID: 22001 RVA: 0x001F6D4C File Offset: 0x001F514C
	private void DrawMesh()
	{
		if (this.mesh != null && this.material != null)
		{
			Matrix4x4 identity = Matrix4x4.identity;
			for (int i = 0; i < this.mesh.subMeshCount; i++)
			{
				Graphics.DrawMesh(this.drawMesh, identity, this.material, 0, null, i, null, false, false);
			}
		}
	}

	// Token: 0x060055F2 RID: 22002 RVA: 0x001F6DB8 File Offset: 0x001F51B8
	private void Update()
	{
		Vector3 angles = Quaternion2Angles.GetAngles(this.follow.localRotation, this.rotationOrder);
		this.xrot = angles.x * 57.29578f;
		this.yrot = angles.y * 57.29578f;
		this.zrot = angles.z * 57.29578f;
		this.SkinMesh();
		this.DrawMesh();
	}

	// Token: 0x04004720 RID: 18208
	public Transform follow;

	// Token: 0x04004721 RID: 18209
	public Mesh mesh;

	// Token: 0x04004722 RID: 18210
	public Material material;

	// Token: 0x04004723 RID: 18211
	public Quaternion2Angles.RotationOrder rotationOrder;

	// Token: 0x04004724 RID: 18212
	public float xrot;

	// Token: 0x04004725 RID: 18213
	public float yrot;

	// Token: 0x04004726 RID: 18214
	public float zrot;

	// Token: 0x04004727 RID: 18215
	private Vector3[] meshVerts;

	// Token: 0x04004728 RID: 18216
	private Matrix4x4 bindpose;

	// Token: 0x04004729 RID: 18217
	private int numVertices;

	// Token: 0x0400472A RID: 18218
	private Vector3[] vertices;

	// Token: 0x0400472B RID: 18219
	private Mesh drawMesh;
}
