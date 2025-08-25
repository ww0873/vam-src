using System;
using UnityEngine;

// Token: 0x02000C95 RID: 3221
public class LineDrawer
{
	// Token: 0x06006121 RID: 24865 RVA: 0x0024F949 File Offset: 0x0024DD49
	public LineDrawer(Material m)
	{
		this.material = m;
		this._numLines = 1;
		this._LineDrawer();
	}

	// Token: 0x06006122 RID: 24866 RVA: 0x0024F96C File Offset: 0x0024DD6C
	public LineDrawer(int numberOfLines, Material m)
	{
		this.material = m;
		this._numLines = numberOfLines;
		this._LineDrawer();
	}

	// Token: 0x17000E4E RID: 3662
	// (get) Token: 0x06006123 RID: 24867 RVA: 0x0024F98F File Offset: 0x0024DD8F
	public int numLines
	{
		get
		{
			return this._numLines;
		}
	}

	// Token: 0x06006124 RID: 24868 RVA: 0x0024F998 File Offset: 0x0024DD98
	private void _LineDrawer()
	{
		this.mesh = new Mesh();
		this.numVertices = this._numLines * 2;
		this.drawMatrix = Matrix4x4.identity;
		this.meshVerts = new Vector3[this.numVertices];
		int[] array = new int[this.numVertices];
		Vector2[] array2 = new Vector2[this.numVertices];
		Vector3[] normals = new Vector3[this.numVertices];
		for (int i = 0; i < this.numVertices; i++)
		{
			array[i] = i;
			array2[i].x = 0f;
			array2[i].y = 0f;
		}
		this.mesh.vertices = this.meshVerts;
		this.mesh.uv = array2;
		this.mesh.normals = normals;
		this.mesh.SetIndices(array, MeshTopology.Lines, 0);
	}

	// Token: 0x06006125 RID: 24869 RVA: 0x0024FA74 File Offset: 0x0024DE74
	public void SetLinePoints(int lineIndex, Vector3 point1, Vector3 point2)
	{
		int num = lineIndex * 2;
		this.meshVerts[num] = point1;
		this.meshVerts[num + 1] = point2;
	}

	// Token: 0x06006126 RID: 24870 RVA: 0x0024FAAB File Offset: 0x0024DEAB
	public void SetLinePoints(Vector3 point1, Vector3 point2)
	{
		this.SetLinePoints(0, point1, point2);
	}

	// Token: 0x06006127 RID: 24871 RVA: 0x0024FAB8 File Offset: 0x0024DEB8
	public void Draw(int layer = 0)
	{
		this.mesh.vertices = this.meshVerts;
		this.mesh.RecalculateBounds();
		if (this.material)
		{
			Graphics.DrawMesh(this.mesh, this.drawMatrix, this.material, layer, null, 0, null, false, false);
		}
	}

	// Token: 0x06006128 RID: 24872 RVA: 0x0024FB0E File Offset: 0x0024DF0E
	public void Destroy()
	{
		if (this.mesh != null)
		{
			UnityEngine.Object.Destroy(this.mesh);
		}
	}

	// Token: 0x040050FD RID: 20733
	public Material material;

	// Token: 0x040050FE RID: 20734
	private Mesh mesh;

	// Token: 0x040050FF RID: 20735
	private Vector3[] meshVerts;

	// Token: 0x04005100 RID: 20736
	private int numVertices;

	// Token: 0x04005101 RID: 20737
	private int _numLines = 1;

	// Token: 0x04005102 RID: 20738
	private Matrix4x4 drawMatrix;
}
