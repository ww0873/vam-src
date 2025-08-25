using System;
using UnityEngine;

// Token: 0x02000CA8 RID: 3240
public class RecalculateNormalsGPU
{
	// Token: 0x0600615D RID: 24925 RVA: 0x00253EF8 File Offset: 0x002522F8
	public RecalculateNormalsGPU(ComputeShader cs, int[] tris, int numVertices, VertexMap[] vertexMap = null)
	{
		this._recalcSurfaceNormalsKernel = cs.FindKernel("RecalcSurfaceNormals");
		this._recalcVertexNormalsKernel = cs.FindKernel("RecalcVertexNormals");
		if (this._recalcVertexNormalsKernel != -1 && this._recalcSurfaceNormalsKernel != -1)
		{
			this.computeShader = cs;
			this.numVertThreadGroups = numVertices / 256;
			int num = numVertices % 256;
			if (num != 0)
			{
				this.numVertThreadGroups++;
			}
			int num2 = this.numVertThreadGroups * 256;
			this.vertTrianglesStruct = new RecalculateNormalsGPU.VertTriangles[num2];
			for (int i = 0; i < num2; i++)
			{
				this.vertTrianglesStruct[i] = default(RecalculateNormalsGPU.VertTriangles);
			}
			int num3 = tris.Length / 3;
			this.numTriangleThreadGroups = num3 / 256;
			num = num3 % 256;
			if (num != 0)
			{
				this.numTriangleThreadGroups++;
			}
			int num4 = this.numTriangleThreadGroups * 256;
			this.trianglesStruct = new RecalculateNormalsGPU.Triangle[num4];
			int[] array = new int[numVertices];
			for (int j = 0; j < num4; j++)
			{
				if (j < num3)
				{
					int num5 = j * 3;
					this.trianglesStruct[j] = default(RecalculateNormalsGPU.Triangle);
					int num6 = tris[num5];
					this.SetVertexTriangle(array[num6], num6, j);
					array[num6]++;
					this.trianglesStruct[j].vert1 = num6;
					int num7 = tris[num5 + 1];
					this.SetVertexTriangle(array[num7], num7, j);
					array[num7]++;
					this.trianglesStruct[j].vert2 = num7;
					int num8 = tris[num5 + 2];
					this.SetVertexTriangle(array[num8], num8, j);
					array[num8]++;
					this.trianglesStruct[j].vert3 = num8;
				}
				else
				{
					this.trianglesStruct[j] = default(RecalculateNormalsGPU.Triangle);
					this.trianglesStruct[j].vert1 = numVertices;
					this.trianglesStruct[j].vert2 = numVertices;
					this.trianglesStruct[j].vert3 = numVertices;
				}
			}
			this.trianglesBuffer = new ComputeBuffer(num4, 12);
			this.trianglesBuffer.SetData(this.trianglesStruct);
			this._normalsBuffer = new ComputeBuffer(num2, 12);
			this._surfaceNormalsBuffer = new ComputeBuffer(num4, 12);
			this.vertTrianglesBuffer = new ComputeBuffer(num2, 48);
			this.vertTrianglesBuffer.SetData(this.vertTrianglesStruct);
			if (vertexMap != null)
			{
				this.mapVerticesGPU = new MapVerticesGPU(this.computeShader, vertexMap);
			}
		}
		else
		{
			Debug.LogError("Compute Shader does not have RecalcSurfaceNormals or RecalcVertexNormals kernel");
		}
	}

	// Token: 0x17000E54 RID: 3668
	// (get) Token: 0x0600615E RID: 24926 RVA: 0x002541D9 File Offset: 0x002525D9
	public ComputeBuffer normalsBuffer
	{
		get
		{
			return this._normalsBuffer;
		}
	}

	// Token: 0x17000E55 RID: 3669
	// (get) Token: 0x0600615F RID: 24927 RVA: 0x002541E1 File Offset: 0x002525E1
	public ComputeBuffer surfaceNormalsBuffer
	{
		get
		{
			return this._surfaceNormalsBuffer;
		}
	}

	// Token: 0x06006160 RID: 24928 RVA: 0x002541EC File Offset: 0x002525EC
	protected void SetVertexTriangle(int count, int vert, int triangle)
	{
		if (count < 6)
		{
			switch (count)
			{
			case 0:
				this.vertTrianglesStruct[vert].triangle0 = triangle;
				this.vertTrianglesStruct[vert].triangle0factor = 1f;
				break;
			case 1:
				this.vertTrianglesStruct[vert].triangle1 = triangle;
				this.vertTrianglesStruct[vert].triangle1factor = 1f;
				break;
			case 2:
				this.vertTrianglesStruct[vert].triangle2 = triangle;
				this.vertTrianglesStruct[vert].triangle2factor = 1f;
				break;
			case 3:
				this.vertTrianglesStruct[vert].triangle3 = triangle;
				this.vertTrianglesStruct[vert].triangle3factor = 1f;
				break;
			case 4:
				this.vertTrianglesStruct[vert].triangle4 = triangle;
				this.vertTrianglesStruct[vert].triangle4factor = 1f;
				break;
			case 5:
				this.vertTrianglesStruct[vert].triangle5 = triangle;
				this.vertTrianglesStruct[vert].triangle5factor = 1f;
				break;
			}
		}
	}

	// Token: 0x06006161 RID: 24929 RVA: 0x00254338 File Offset: 0x00252738
	public void RecalculateSurfaceNormals(ComputeBuffer inVertBuffer)
	{
		this.computeShader.SetBuffer(this._recalcSurfaceNormalsKernel, "inVerts", inVertBuffer);
		this.computeShader.SetBuffer(this._recalcSurfaceNormalsKernel, "triangles", this.trianglesBuffer);
		this.computeShader.SetBuffer(this._recalcSurfaceNormalsKernel, "surfaceNormals", this._surfaceNormalsBuffer);
		this.computeShader.Dispatch(this._recalcSurfaceNormalsKernel, this.numTriangleThreadGroups, 1, 1);
	}

	// Token: 0x06006162 RID: 24930 RVA: 0x002543B0 File Offset: 0x002527B0
	public void RecalculateNormals(ComputeBuffer inVertBuffer)
	{
		if (this.computeShader != null)
		{
			this.computeShader.SetBuffer(this._recalcSurfaceNormalsKernel, "inVerts", inVertBuffer);
			this.computeShader.SetBuffer(this._recalcSurfaceNormalsKernel, "triangles", this.trianglesBuffer);
			this.computeShader.SetBuffer(this._recalcSurfaceNormalsKernel, "surfaceNormals", this._surfaceNormalsBuffer);
			this.computeShader.Dispatch(this._recalcSurfaceNormalsKernel, this.numTriangleThreadGroups, 1, 1);
			this.computeShader.SetBuffer(this._recalcVertexNormalsKernel, "vertTriangles", this.vertTrianglesBuffer);
			this.computeShader.SetBuffer(this._recalcVertexNormalsKernel, "surfaceNormals", this._surfaceNormalsBuffer);
			this.computeShader.SetBuffer(this._recalcVertexNormalsKernel, "normals", this._normalsBuffer);
			this.computeShader.Dispatch(this._recalcVertexNormalsKernel, this.numVertThreadGroups, 1, 1);
			if (this.mapVerticesGPU != null)
			{
				this.mapVerticesGPU.Map(this._normalsBuffer);
			}
		}
	}

	// Token: 0x06006163 RID: 24931 RVA: 0x002544C0 File Offset: 0x002528C0
	public void Release()
	{
		if (this.mapVerticesGPU != null)
		{
			this.mapVerticesGPU.Release();
			this.mapVerticesGPU = null;
		}
		if (this.trianglesBuffer != null)
		{
			this.trianglesBuffer.Release();
			this.trianglesBuffer = null;
		}
		if (this._normalsBuffer != null)
		{
			this._normalsBuffer.Release();
			this._normalsBuffer = null;
		}
		if (this._surfaceNormalsBuffer != null)
		{
			this._surfaceNormalsBuffer.Release();
			this._surfaceNormalsBuffer = null;
		}
		if (this.vertTrianglesBuffer != null)
		{
			this.vertTrianglesBuffer.Release();
			this.vertTrianglesBuffer = null;
		}
	}

	// Token: 0x040051A8 RID: 20904
	protected const int maxNumTriangles = 6;

	// Token: 0x040051A9 RID: 20905
	protected const int triangleGroupSize = 256;

	// Token: 0x040051AA RID: 20906
	protected const int vertGroupSize = 256;

	// Token: 0x040051AB RID: 20907
	protected RecalculateNormalsGPU.Triangle[] trianglesStruct;

	// Token: 0x040051AC RID: 20908
	protected RecalculateNormalsGPU.VertTriangles[] vertTrianglesStruct;

	// Token: 0x040051AD RID: 20909
	protected ComputeShader computeShader;

	// Token: 0x040051AE RID: 20910
	protected int _recalcSurfaceNormalsKernel;

	// Token: 0x040051AF RID: 20911
	protected int _recalcVertexNormalsKernel;

	// Token: 0x040051B0 RID: 20912
	protected ComputeBuffer _normalsBuffer;

	// Token: 0x040051B1 RID: 20913
	protected ComputeBuffer trianglesBuffer;

	// Token: 0x040051B2 RID: 20914
	protected ComputeBuffer _surfaceNormalsBuffer;

	// Token: 0x040051B3 RID: 20915
	protected ComputeBuffer vertTrianglesBuffer;

	// Token: 0x040051B4 RID: 20916
	protected int numTriangleThreadGroups;

	// Token: 0x040051B5 RID: 20917
	protected int numVertThreadGroups;

	// Token: 0x040051B6 RID: 20918
	protected MapVerticesGPU mapVerticesGPU;

	// Token: 0x02000CA9 RID: 3241
	protected struct Triangle
	{
		// Token: 0x040051B7 RID: 20919
		public int vert1;

		// Token: 0x040051B8 RID: 20920
		public int vert2;

		// Token: 0x040051B9 RID: 20921
		public int vert3;
	}

	// Token: 0x02000CAA RID: 3242
	protected struct VertTriangles
	{
		// Token: 0x040051BA RID: 20922
		public int triangle0;

		// Token: 0x040051BB RID: 20923
		public int triangle1;

		// Token: 0x040051BC RID: 20924
		public int triangle2;

		// Token: 0x040051BD RID: 20925
		public int triangle3;

		// Token: 0x040051BE RID: 20926
		public int triangle4;

		// Token: 0x040051BF RID: 20927
		public int triangle5;

		// Token: 0x040051C0 RID: 20928
		public float triangle0factor;

		// Token: 0x040051C1 RID: 20929
		public float triangle1factor;

		// Token: 0x040051C2 RID: 20930
		public float triangle2factor;

		// Token: 0x040051C3 RID: 20931
		public float triangle3factor;

		// Token: 0x040051C4 RID: 20932
		public float triangle4factor;

		// Token: 0x040051C5 RID: 20933
		public float triangle5factor;
	}
}
