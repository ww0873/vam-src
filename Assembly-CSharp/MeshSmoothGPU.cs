using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000CA6 RID: 3238
public class MeshSmoothGPU
{
	// Token: 0x06006157 RID: 24919 RVA: 0x00252CFC File Offset: 0x002510FC
	public MeshSmoothGPU(ComputeShader cs, Vector3[] vertices, MeshPoly[] polys)
	{
		int num = vertices.Length;
		this._smoothKernel = cs.FindKernel("LaplacianSmooth");
		this._springSmoothKernel = cs.FindKernel("SpringSmooth");
		this._springSmooth2Kernel = cs.FindKernel("SpringSmooth2");
		this._hc1Kernel = cs.FindKernel("HCCorrectionP1");
		this._hc2Kernel = cs.FindKernel("HCCorrectionP2");
		if (this._smoothKernel != -1 && this._hc1Kernel != -1 && this._hc2Kernel != -1)
		{
			this.computeShader = cs;
			this.diffs = new Vector3[num];
			this.vertexNeighbors = new int[num][];
			Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
			for (int i = 0; i < polys.Length; i++)
			{
				for (int j = 0; j < polys[i].vertices.Length; j++)
				{
					int key = polys[i].vertices[j];
					List<int> list;
					if (!dictionary.TryGetValue(key, out list))
					{
						list = new List<int>();
						dictionary.Add(key, list);
					}
					list.Add(i);
				}
			}
			this.numVertThreadGroups = num / 256;
			int num2 = num % 256;
			if (num2 != 0)
			{
				this.numVertThreadGroups++;
			}
			int num3 = this.numVertThreadGroups * 256;
			this.vertexNeighborsStruct = new MeshSmoothGPU.VertNeighbors[num3];
			for (int k = 0; k < num; k++)
			{
				Dictionary<int, bool> dictionary2 = new Dictionary<int, bool>();
				List<int> list2;
				if (dictionary.TryGetValue(k, out list2))
				{
					foreach (int num4 in list2)
					{
						for (int l = 0; l < polys[num4].vertices.Length; l++)
						{
							int num5 = polys[num4].vertices[l];
							if (num5 != k && !dictionary2.ContainsKey(num5))
							{
								dictionary2.Add(num5, true);
							}
						}
					}
				}
				this.vertexNeighbors[k] = new int[dictionary2.Count];
				this.vertexNeighborsStruct[k] = default(MeshSmoothGPU.VertNeighbors);
				int count = dictionary2.Keys.Count;
				float num6;
				if (count > 16)
				{
					Debug.LogWarning(string.Concat(new object[]
					{
						"Vertex ",
						k,
						" has more neighbors ",
						count,
						" than the maximum ",
						16
					}));
					num6 = 0.0625f;
					this.vertexNeighborsStruct[k].numNeighbors = 16;
				}
				else
				{
					num6 = 1f / (float)count;
					this.vertexNeighborsStruct[k].numNeighbors = count;
				}
				int num7 = 0;
				this.vertexNeighborsStruct[k].neighbor0factor = 0f;
				this.vertexNeighborsStruct[k].neighbor1factor = 0f;
				this.vertexNeighborsStruct[k].neighbor2factor = 0f;
				this.vertexNeighborsStruct[k].neighbor3factor = 0f;
				this.vertexNeighborsStruct[k].neighbor4factor = 0f;
				this.vertexNeighborsStruct[k].neighbor5factor = 0f;
				this.vertexNeighborsStruct[k].neighbor6factor = 0f;
				this.vertexNeighborsStruct[k].neighbor7factor = 0f;
				this.vertexNeighborsStruct[k].neighbor8factor = 0f;
				this.vertexNeighborsStruct[k].neighbor9factor = 0f;
				this.vertexNeighborsStruct[k].neighbor10factor = 0f;
				this.vertexNeighborsStruct[k].neighbor11factor = 0f;
				this.vertexNeighborsStruct[k].neighbor12factor = 0f;
				this.vertexNeighborsStruct[k].neighbor13factor = 0f;
				this.vertexNeighborsStruct[k].neighbor14factor = 0f;
				this.vertexNeighborsStruct[k].neighbor15factor = 0f;
				this.vertexNeighborsStruct[k].neighbor0distance = 1f;
				this.vertexNeighborsStruct[k].neighbor1distance = 1f;
				this.vertexNeighborsStruct[k].neighbor2distance = 1f;
				this.vertexNeighborsStruct[k].neighbor3distance = 1f;
				this.vertexNeighborsStruct[k].neighbor4distance = 1f;
				this.vertexNeighborsStruct[k].neighbor5distance = 1f;
				this.vertexNeighborsStruct[k].neighbor6distance = 1f;
				this.vertexNeighborsStruct[k].neighbor7distance = 1f;
				this.vertexNeighborsStruct[k].neighbor8distance = 1f;
				this.vertexNeighborsStruct[k].neighbor9distance = 1f;
				this.vertexNeighborsStruct[k].neighbor10distance = 1f;
				this.vertexNeighborsStruct[k].neighbor11distance = 1f;
				this.vertexNeighborsStruct[k].neighbor12distance = 1f;
				this.vertexNeighborsStruct[k].neighbor13distance = 1f;
				this.vertexNeighborsStruct[k].neighbor14distance = 1f;
				this.vertexNeighborsStruct[k].neighbor15distance = 1f;
				foreach (int num8 in dictionary2.Keys)
				{
					this.vertexNeighbors[k][num7] = num8;
					switch (num7)
					{
					case 0:
						this.vertexNeighborsStruct[k].neighbor0 = num8;
						this.vertexNeighborsStruct[k].neighbor0distance = (vertices[num8] - vertices[k]).magnitude;
						this.vertexNeighborsStruct[k].neighbor0factor = num6;
						break;
					case 1:
						this.vertexNeighborsStruct[k].neighbor1 = num8;
						this.vertexNeighborsStruct[k].neighbor1distance = (vertices[num8] - vertices[k]).magnitude;
						this.vertexNeighborsStruct[k].neighbor1factor = num6;
						break;
					case 2:
						this.vertexNeighborsStruct[k].neighbor2 = num8;
						this.vertexNeighborsStruct[k].neighbor2distance = (vertices[num8] - vertices[k]).magnitude;
						this.vertexNeighborsStruct[k].neighbor2factor = num6;
						break;
					case 3:
						this.vertexNeighborsStruct[k].neighbor3 = num8;
						this.vertexNeighborsStruct[k].neighbor3distance = (vertices[num8] - vertices[k]).magnitude;
						this.vertexNeighborsStruct[k].neighbor3factor = num6;
						break;
					case 4:
						this.vertexNeighborsStruct[k].neighbor4 = num8;
						this.vertexNeighborsStruct[k].neighbor4distance = (vertices[num8] - vertices[k]).magnitude;
						this.vertexNeighborsStruct[k].neighbor4factor = num6;
						break;
					case 5:
						this.vertexNeighborsStruct[k].neighbor5 = num8;
						this.vertexNeighborsStruct[k].neighbor5distance = (vertices[num8] - vertices[k]).magnitude;
						this.vertexNeighborsStruct[k].neighbor5factor = num6;
						break;
					case 6:
						this.vertexNeighborsStruct[k].neighbor6 = num8;
						this.vertexNeighborsStruct[k].neighbor6distance = (vertices[num8] - vertices[k]).magnitude;
						this.vertexNeighborsStruct[k].neighbor6factor = num6;
						break;
					case 7:
						this.vertexNeighborsStruct[k].neighbor7 = num8;
						this.vertexNeighborsStruct[k].neighbor7distance = (vertices[num8] - vertices[k]).magnitude;
						this.vertexNeighborsStruct[k].neighbor7factor = num6;
						break;
					case 8:
						this.vertexNeighborsStruct[k].neighbor8 = num8;
						this.vertexNeighborsStruct[k].neighbor8distance = (vertices[num8] - vertices[k]).magnitude;
						this.vertexNeighborsStruct[k].neighbor8factor = num6;
						break;
					case 9:
						this.vertexNeighborsStruct[k].neighbor9 = num8;
						this.vertexNeighborsStruct[k].neighbor9distance = (vertices[num8] - vertices[k]).magnitude;
						this.vertexNeighborsStruct[k].neighbor9factor = num6;
						break;
					case 10:
						this.vertexNeighborsStruct[k].neighbor10 = num8;
						this.vertexNeighborsStruct[k].neighbor10distance = (vertices[num8] - vertices[k]).magnitude;
						this.vertexNeighborsStruct[k].neighbor10factor = num6;
						break;
					case 11:
						this.vertexNeighborsStruct[k].neighbor11 = num8;
						this.vertexNeighborsStruct[k].neighbor11distance = (vertices[num8] - vertices[k]).magnitude;
						this.vertexNeighborsStruct[k].neighbor11factor = num6;
						break;
					case 12:
						this.vertexNeighborsStruct[k].neighbor12 = num8;
						this.vertexNeighborsStruct[k].neighbor12distance = (vertices[num8] - vertices[k]).magnitude;
						this.vertexNeighborsStruct[k].neighbor12factor = num6;
						break;
					case 13:
						this.vertexNeighborsStruct[k].neighbor13 = num8;
						this.vertexNeighborsStruct[k].neighbor13distance = (vertices[num8] - vertices[k]).magnitude;
						this.vertexNeighborsStruct[k].neighbor13factor = num6;
						break;
					case 14:
						this.vertexNeighborsStruct[k].neighbor14 = num8;
						this.vertexNeighborsStruct[k].neighbor14distance = (vertices[num8] - vertices[k]).magnitude;
						this.vertexNeighborsStruct[k].neighbor14factor = num6;
						break;
					case 15:
						this.vertexNeighborsStruct[k].neighbor15 = num8;
						this.vertexNeighborsStruct[k].neighbor15distance = (vertices[num8] - vertices[k]).magnitude;
						this.vertexNeighborsStruct[k].neighbor15factor = num6;
						break;
					}
					num7++;
				}
			}
			for (int m = num; m < num3; m++)
			{
				this.vertexNeighborsStruct[m] = default(MeshSmoothGPU.VertNeighbors);
				this.vertexNeighborsStruct[m].neighbor0 = m;
				this.vertexNeighborsStruct[m].neighbor0factor = 1f;
				this.vertexNeighborsStruct[m].neighbor1factor = 0f;
				this.vertexNeighborsStruct[m].neighbor2factor = 0f;
				this.vertexNeighborsStruct[m].neighbor3factor = 0f;
				this.vertexNeighborsStruct[m].neighbor4factor = 0f;
				this.vertexNeighborsStruct[m].neighbor5factor = 0f;
				this.vertexNeighborsStruct[m].neighbor6factor = 0f;
				this.vertexNeighborsStruct[m].neighbor7factor = 0f;
				this.vertexNeighborsStruct[m].neighbor8factor = 0f;
				this.vertexNeighborsStruct[m].neighbor9factor = 0f;
				this.vertexNeighborsStruct[m].neighbor10factor = 0f;
				this.vertexNeighborsStruct[m].neighbor11factor = 0f;
				this.vertexNeighborsStruct[m].neighbor12factor = 0f;
				this.vertexNeighborsStruct[m].neighbor13factor = 0f;
				this.vertexNeighborsStruct[m].neighbor14factor = 0f;
				this.vertexNeighborsStruct[m].neighbor15factor = 0f;
			}
			this.vertNeighborBuffer = new ComputeBuffer(num3, 196);
			this.vertNeighborBuffer.SetData(this.vertexNeighborsStruct);
			this.vertDiffBuffer = new ComputeBuffer(num3, 12);
		}
		else
		{
			Debug.LogError("Compute Shader does not have LaplacianSmooth or HCCorrectionP1 or HCCorrectionP2 kernel");
		}
	}

	// Token: 0x06006158 RID: 24920 RVA: 0x00253BF4 File Offset: 0x00251FF4
	public void LaplacianSmoothGPU(ComputeBuffer inVertBuffer, ComputeBuffer outVertBuffer)
	{
		if (this.computeShader != null)
		{
			this.computeShader.SetBuffer(this._smoothKernel, "vertNeighbors", this.vertNeighborBuffer);
			this.computeShader.SetBuffer(this._smoothKernel, "inVerts", inVertBuffer);
			this.computeShader.SetBuffer(this._smoothKernel, "outVerts", outVertBuffer);
			this.computeShader.Dispatch(this._smoothKernel, this.numVertThreadGroups, 1, 1);
		}
	}

	// Token: 0x06006159 RID: 24921 RVA: 0x00253C78 File Offset: 0x00252078
	public void SpringSmoothGPU(ComputeBuffer inVertBuffer, ComputeBuffer outVertBuffer, float springFactor, float scale = 1f)
	{
		if (this.computeShader != null)
		{
			this.computeShader.SetBuffer(this._springSmoothKernel, "vertNeighbors", this.vertNeighborBuffer);
			this.computeShader.SetBuffer(this._springSmoothKernel, "inVerts", inVertBuffer);
			this.computeShader.SetBuffer(this._springSmoothKernel, "outVerts", outVertBuffer);
			this.computeShader.SetFloat("springFactor", springFactor);
			this.computeShader.SetFloat("smoothScale", scale);
			this.computeShader.Dispatch(this._springSmoothKernel, this.numVertThreadGroups, 1, 1);
		}
	}

	// Token: 0x0600615A RID: 24922 RVA: 0x00253D1C File Offset: 0x0025211C
	public void SpringSmooth2GPU(ComputeBuffer inVertBuffer, ComputeBuffer outVertBuffer, float springFactor, float scale = 1f)
	{
		if (this.computeShader != null)
		{
			this.computeShader.SetBuffer(this._springSmooth2Kernel, "vertNeighbors", this.vertNeighborBuffer);
			this.computeShader.SetBuffer(this._springSmooth2Kernel, "inVerts", inVertBuffer);
			this.computeShader.SetBuffer(this._springSmooth2Kernel, "outVerts", outVertBuffer);
			this.computeShader.SetFloat("springFactor", springFactor);
			this.computeShader.SetFloat("smoothScale", scale);
			this.computeShader.Dispatch(this._springSmooth2Kernel, this.numVertThreadGroups, 1, 1);
		}
	}

	// Token: 0x0600615B RID: 24923 RVA: 0x00253DC0 File Offset: 0x002521C0
	public void HCCorrectionGPU(ComputeBuffer inVertBuffer, ComputeBuffer outVertBuffer, float hcCorrectionBeta = 0.5f)
	{
		if (this.computeShader != null)
		{
			this.computeShader.SetBuffer(this._hc1Kernel, "inVerts", inVertBuffer);
			this.computeShader.SetBuffer(this._hc1Kernel, "outVerts", outVertBuffer);
			this.computeShader.SetBuffer(this._hc1Kernel, "smoothDiffs", this.vertDiffBuffer);
			this.computeShader.Dispatch(this._hc1Kernel, this.numVertThreadGroups, 1, 1);
			this.computeShader.SetFloat("HCCorrectionBeta", hcCorrectionBeta);
			this.computeShader.SetBuffer(this._hc2Kernel, "vertNeighbors", this.vertNeighborBuffer);
			this.computeShader.SetBuffer(this._hc2Kernel, "smoothDiffs", this.vertDiffBuffer);
			this.computeShader.SetBuffer(this._hc2Kernel, "outVerts", outVertBuffer);
			this.computeShader.Dispatch(this._hc2Kernel, this.numVertThreadGroups, 1, 1);
		}
	}

	// Token: 0x0600615C RID: 24924 RVA: 0x00253EBA File Offset: 0x002522BA
	public void Release()
	{
		if (this.vertNeighborBuffer != null)
		{
			this.vertNeighborBuffer.Release();
			this.vertNeighborBuffer = null;
		}
		if (this.vertDiffBuffer != null)
		{
			this.vertDiffBuffer.Release();
			this.vertDiffBuffer = null;
		}
	}

	// Token: 0x04005169 RID: 20841
	protected const int maxNumNeighbors = 16;

	// Token: 0x0400516A RID: 20842
	protected const int vertGroupSize = 256;

	// Token: 0x0400516B RID: 20843
	protected int numVertThreadGroups;

	// Token: 0x0400516C RID: 20844
	protected int[][] vertexNeighbors;

	// Token: 0x0400516D RID: 20845
	protected MeshSmoothGPU.VertNeighbors[] vertexNeighborsStruct;

	// Token: 0x0400516E RID: 20846
	protected ComputeShader computeShader;

	// Token: 0x0400516F RID: 20847
	protected int _smoothKernel;

	// Token: 0x04005170 RID: 20848
	protected int _springSmoothKernel;

	// Token: 0x04005171 RID: 20849
	protected int _springSmooth2Kernel;

	// Token: 0x04005172 RID: 20850
	protected int _hc1Kernel;

	// Token: 0x04005173 RID: 20851
	protected int _hc2Kernel;

	// Token: 0x04005174 RID: 20852
	protected ComputeBuffer vertNeighborBuffer;

	// Token: 0x04005175 RID: 20853
	protected ComputeBuffer vertDiffBuffer;

	// Token: 0x04005176 RID: 20854
	protected Vector3[] diffs;

	// Token: 0x02000CA7 RID: 3239
	protected struct VertNeighbors
	{
		// Token: 0x04005177 RID: 20855
		public int numNeighbors;

		// Token: 0x04005178 RID: 20856
		public int neighbor0;

		// Token: 0x04005179 RID: 20857
		public int neighbor1;

		// Token: 0x0400517A RID: 20858
		public int neighbor2;

		// Token: 0x0400517B RID: 20859
		public int neighbor3;

		// Token: 0x0400517C RID: 20860
		public int neighbor4;

		// Token: 0x0400517D RID: 20861
		public int neighbor5;

		// Token: 0x0400517E RID: 20862
		public int neighbor6;

		// Token: 0x0400517F RID: 20863
		public int neighbor7;

		// Token: 0x04005180 RID: 20864
		public int neighbor8;

		// Token: 0x04005181 RID: 20865
		public int neighbor9;

		// Token: 0x04005182 RID: 20866
		public int neighbor10;

		// Token: 0x04005183 RID: 20867
		public int neighbor11;

		// Token: 0x04005184 RID: 20868
		public int neighbor12;

		// Token: 0x04005185 RID: 20869
		public int neighbor13;

		// Token: 0x04005186 RID: 20870
		public int neighbor14;

		// Token: 0x04005187 RID: 20871
		public int neighbor15;

		// Token: 0x04005188 RID: 20872
		public float neighbor0factor;

		// Token: 0x04005189 RID: 20873
		public float neighbor1factor;

		// Token: 0x0400518A RID: 20874
		public float neighbor2factor;

		// Token: 0x0400518B RID: 20875
		public float neighbor3factor;

		// Token: 0x0400518C RID: 20876
		public float neighbor4factor;

		// Token: 0x0400518D RID: 20877
		public float neighbor5factor;

		// Token: 0x0400518E RID: 20878
		public float neighbor6factor;

		// Token: 0x0400518F RID: 20879
		public float neighbor7factor;

		// Token: 0x04005190 RID: 20880
		public float neighbor8factor;

		// Token: 0x04005191 RID: 20881
		public float neighbor9factor;

		// Token: 0x04005192 RID: 20882
		public float neighbor10factor;

		// Token: 0x04005193 RID: 20883
		public float neighbor11factor;

		// Token: 0x04005194 RID: 20884
		public float neighbor12factor;

		// Token: 0x04005195 RID: 20885
		public float neighbor13factor;

		// Token: 0x04005196 RID: 20886
		public float neighbor14factor;

		// Token: 0x04005197 RID: 20887
		public float neighbor15factor;

		// Token: 0x04005198 RID: 20888
		public float neighbor0distance;

		// Token: 0x04005199 RID: 20889
		public float neighbor1distance;

		// Token: 0x0400519A RID: 20890
		public float neighbor2distance;

		// Token: 0x0400519B RID: 20891
		public float neighbor3distance;

		// Token: 0x0400519C RID: 20892
		public float neighbor4distance;

		// Token: 0x0400519D RID: 20893
		public float neighbor5distance;

		// Token: 0x0400519E RID: 20894
		public float neighbor6distance;

		// Token: 0x0400519F RID: 20895
		public float neighbor7distance;

		// Token: 0x040051A0 RID: 20896
		public float neighbor8distance;

		// Token: 0x040051A1 RID: 20897
		public float neighbor9distance;

		// Token: 0x040051A2 RID: 20898
		public float neighbor10distance;

		// Token: 0x040051A3 RID: 20899
		public float neighbor11distance;

		// Token: 0x040051A4 RID: 20900
		public float neighbor12distance;

		// Token: 0x040051A5 RID: 20901
		public float neighbor13distance;

		// Token: 0x040051A6 RID: 20902
		public float neighbor14distance;

		// Token: 0x040051A7 RID: 20903
		public float neighbor15distance;
	}
}
