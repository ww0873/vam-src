using System;
using UnityEngine;

// Token: 0x02000CA1 RID: 3233
public class MapVerticesGPU
{
	// Token: 0x0600614C RID: 24908 RVA: 0x002528B4 File Offset: 0x00250CB4
	public MapVerticesGPU(ComputeShader cs, VertexMap[] vertexMap)
	{
		this.mappingKernel = cs.FindKernel("MapVertices");
		if (this.mappingKernel != -1)
		{
			this.computeShader = cs;
			int num = vertexMap.Length;
			if (num == 0)
			{
				this.numMappingGroups = 0;
			}
			else
			{
				this.numMappingGroups = num / this.mappingGroupSize;
				int num2 = num % this.mappingGroupSize;
				if (num2 != 0)
				{
					this.numMappingGroups++;
				}
				int num3 = this.numMappingGroups * this.mappingGroupSize;
				this.mappingBuffer = new ComputeBuffer(num3, 8);
				this.vertexMapping = new MapVerticesGPU.VertexMapGPU[num3];
				for (int i = 0; i < num3; i++)
				{
					if (i < num)
					{
						VertexMap vertexMap2 = vertexMap[i];
						this.vertexMapping[i].fromvert = vertexMap2.fromvert;
						this.vertexMapping[i].tovert = vertexMap2.tovert;
					}
					else
					{
						this.vertexMapping[i].fromvert = 0;
						this.vertexMapping[i].tovert = 0;
					}
				}
				this.mappingBuffer.SetData(this.vertexMapping);
			}
		}
		else
		{
			Debug.LogError("Compute Shader does not have MapVertices kernel");
		}
	}

	// Token: 0x0600614D RID: 24909 RVA: 0x002529F8 File Offset: 0x00250DF8
	public void Map(ComputeBuffer vertsBuffer)
	{
		if (this.computeShader != null && this.numMappingGroups != 0)
		{
			this.computeShader.SetBuffer(this.mappingKernel, "vertexMapping", this.mappingBuffer);
			this.computeShader.SetBuffer(this.mappingKernel, "outVerts", vertsBuffer);
			this.computeShader.Dispatch(this.mappingKernel, this.numMappingGroups, 1, 1);
		}
	}

	// Token: 0x0600614E RID: 24910 RVA: 0x00252A6D File Offset: 0x00250E6D
	public void Release()
	{
		if (this.mappingBuffer != null)
		{
			this.mappingBuffer.Release();
		}
	}

	// Token: 0x04005138 RID: 20792
	protected int mappingGroupSize = 256;

	// Token: 0x04005139 RID: 20793
	protected int numMappingGroups;

	// Token: 0x0400513A RID: 20794
	protected ComputeShader computeShader;

	// Token: 0x0400513B RID: 20795
	protected int mappingKernel;

	// Token: 0x0400513C RID: 20796
	protected ComputeBuffer mappingBuffer;

	// Token: 0x0400513D RID: 20797
	protected string _mappingBufferName;

	// Token: 0x0400513E RID: 20798
	protected MapVerticesGPU.VertexMapGPU[] vertexMapping;

	// Token: 0x02000CA2 RID: 3234
	protected struct VertexMapGPU
	{
		// Token: 0x0400513F RID: 20799
		public int fromvert;

		// Token: 0x04005140 RID: 20800
		public int tovert;
	}
}
