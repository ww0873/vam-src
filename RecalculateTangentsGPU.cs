using System;
using UnityEngine;

// Token: 0x02000CAB RID: 3243
public class RecalculateTangentsGPU
{
	// Token: 0x06006164 RID: 24932 RVA: 0x00254560 File Offset: 0x00252960
	public RecalculateTangentsGPU(ComputeShader cs, int[] tris, Vector2[] uvs, int numVertices)
	{
		this._recalcVertexTangentsKernel = cs.FindKernel("RecalcVertexTangents");
		this._recalcTriangleTangentDirsKernel = cs.FindKernel("RecalcTriangleTangentDirs");
		if (this._recalcVertexTangentsKernel != -1 && this._recalcTriangleTangentDirsKernel != -1)
		{
			this.computeShader = cs;
			this.numVertThreadGroups = numVertices / 256;
			int num = numVertices % 256;
			if (num != 0)
			{
				this.numVertThreadGroups++;
			}
			int num2 = this.numVertThreadGroups * 256;
			int num3 = tris.Length / 3;
			this.numTriangleThreadGroups = num3 / 256;
			num = num3 % 256;
			if (num != 0)
			{
				this.numTriangleThreadGroups++;
			}
			int num4 = this.numTriangleThreadGroups * 256;
			this.vertTrianglesStruct = new RecalculateTangentsGPU.VertTriangles[num2];
			for (int i = 0; i < num2; i++)
			{
				this.vertTrianglesStruct[i] = default(RecalculateTangentsGPU.VertTriangles);
			}
			this.trianglesStruct = new RecalculateTangentsGPU.Triangle[num4];
			int[] array = new int[numVertices];
			for (int j = 0; j < num4; j++)
			{
				if (j < num3)
				{
					int num5 = j * 3;
					this.trianglesStruct[j] = default(RecalculateTangentsGPU.Triangle);
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
					this.trianglesStruct[j] = default(RecalculateTangentsGPU.Triangle);
					this.trianglesStruct[j].vert1 = numVertices;
					this.trianglesStruct[j].vert2 = numVertices;
					this.trianglesStruct[j].vert3 = numVertices;
				}
			}
			this.trianglesBuffer = new ComputeBuffer(num4, 12);
			this.trianglesBuffer.SetData(this.trianglesStruct);
			this.vertTrianglesBuffer = new ComputeBuffer(num2, 48);
			this.vertTrianglesBuffer.SetData(this.vertTrianglesStruct);
			this._uvBuffer = new ComputeBuffer(num2, 8);
			this._uvBuffer.SetData(uvs);
			this._tangentsBuffer = new ComputeBuffer(num2, 16);
			this._triangleTangentDirsBuffer = new ComputeBuffer(num4, 24);
		}
		else
		{
			Debug.LogError("Compute Shader does not have RecalcTangents* kernels");
		}
	}

	// Token: 0x17000E56 RID: 3670
	// (get) Token: 0x06006165 RID: 24933 RVA: 0x00254843 File Offset: 0x00252C43
	public ComputeBuffer tangentsBuffer
	{
		get
		{
			return this._tangentsBuffer;
		}
	}

	// Token: 0x06006166 RID: 24934 RVA: 0x0025484C File Offset: 0x00252C4C
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

	// Token: 0x06006167 RID: 24935 RVA: 0x00254998 File Offset: 0x00252D98
	public void RecalculateTangents(ComputeBuffer inVertBuffer, ComputeBuffer inNormalBuffer)
	{
		if (this.computeShader != null)
		{
			this.computeShader.SetBuffer(this._recalcTriangleTangentDirsKernel, "inVerts", inVertBuffer);
			this.computeShader.SetBuffer(this._recalcTriangleTangentDirsKernel, "triangles", this.trianglesBuffer);
			this.computeShader.SetBuffer(this._recalcTriangleTangentDirsKernel, "inUV", this._uvBuffer);
			this.computeShader.SetBuffer(this._recalcTriangleTangentDirsKernel, "triangleTangentDirs", this._triangleTangentDirsBuffer);
			this.computeShader.Dispatch(this._recalcTriangleTangentDirsKernel, this.numTriangleThreadGroups, 1, 1);
			this.computeShader.SetBuffer(this._recalcVertexTangentsKernel, "vertTriangles", this.vertTrianglesBuffer);
			this.computeShader.SetBuffer(this._recalcVertexTangentsKernel, "normals", inNormalBuffer);
			this.computeShader.SetBuffer(this._recalcVertexTangentsKernel, "triangleTangentDirs", this._triangleTangentDirsBuffer);
			this.computeShader.SetBuffer(this._recalcVertexTangentsKernel, "tangents", this._tangentsBuffer);
			this.computeShader.Dispatch(this._recalcVertexTangentsKernel, this.numVertThreadGroups, 1, 1);
		}
	}

	// Token: 0x06006168 RID: 24936 RVA: 0x00254AC0 File Offset: 0x00252EC0
	public void Release()
	{
		if (this.trianglesBuffer != null)
		{
			this.trianglesBuffer.Release();
			this.trianglesBuffer = null;
		}
		if (this._tangentsBuffer != null)
		{
			this._tangentsBuffer.Release();
			this._tangentsBuffer = null;
		}
		if (this._triangleTangentDirsBuffer != null)
		{
			this._triangleTangentDirsBuffer.Release();
			this._triangleTangentDirsBuffer = null;
		}
		if (this._uvBuffer != null)
		{
			this._uvBuffer.Release();
			this._uvBuffer = null;
		}
		if (this.vertTrianglesBuffer != null)
		{
			this.vertTrianglesBuffer.Release();
			this.vertTrianglesBuffer = null;
		}
	}

	// Token: 0x040051C6 RID: 20934
	protected const int maxNumTriangles = 6;

	// Token: 0x040051C7 RID: 20935
	protected const int triangleGroupSize = 256;

	// Token: 0x040051C8 RID: 20936
	protected const int vertGroupSize = 256;

	// Token: 0x040051C9 RID: 20937
	protected RecalculateTangentsGPU.Triangle[] trianglesStruct;

	// Token: 0x040051CA RID: 20938
	protected RecalculateTangentsGPU.VertTriangles[] vertTrianglesStruct;

	// Token: 0x040051CB RID: 20939
	protected ComputeShader computeShader;

	// Token: 0x040051CC RID: 20940
	protected int _recalcTriangleTangentDirsKernel;

	// Token: 0x040051CD RID: 20941
	protected int _recalcVertexTangentsKernel;

	// Token: 0x040051CE RID: 20942
	protected ComputeBuffer _tangentsBuffer;

	// Token: 0x040051CF RID: 20943
	protected ComputeBuffer _uvBuffer;

	// Token: 0x040051D0 RID: 20944
	protected ComputeBuffer _triangleTangentDirsBuffer;

	// Token: 0x040051D1 RID: 20945
	protected ComputeBuffer trianglesBuffer;

	// Token: 0x040051D2 RID: 20946
	protected ComputeBuffer vertTrianglesBuffer;

	// Token: 0x040051D3 RID: 20947
	protected int numTriangleThreadGroups;

	// Token: 0x040051D4 RID: 20948
	protected int numVertThreadGroups;

	// Token: 0x02000CAC RID: 3244
	protected struct Triangle
	{
		// Token: 0x040051D5 RID: 20949
		public int vert1;

		// Token: 0x040051D6 RID: 20950
		public int vert2;

		// Token: 0x040051D7 RID: 20951
		public int vert3;
	}

	// Token: 0x02000CAD RID: 3245
	protected struct VertTriangles
	{
		// Token: 0x040051D8 RID: 20952
		public int triangle0;

		// Token: 0x040051D9 RID: 20953
		public int triangle1;

		// Token: 0x040051DA RID: 20954
		public int triangle2;

		// Token: 0x040051DB RID: 20955
		public int triangle3;

		// Token: 0x040051DC RID: 20956
		public int triangle4;

		// Token: 0x040051DD RID: 20957
		public int triangle5;

		// Token: 0x040051DE RID: 20958
		public float triangle0factor;

		// Token: 0x040051DF RID: 20959
		public float triangle1factor;

		// Token: 0x040051E0 RID: 20960
		public float triangle2factor;

		// Token: 0x040051E1 RID: 20961
		public float triangle3factor;

		// Token: 0x040051E2 RID: 20962
		public float triangle4factor;

		// Token: 0x040051E3 RID: 20963
		public float triangle5factor;
	}
}
