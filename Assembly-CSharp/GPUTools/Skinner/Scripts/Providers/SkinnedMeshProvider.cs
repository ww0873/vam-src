using System;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Skinner.Scripts.Abstract;
using GPUTools.Skinner.Scripts.Kernels;
using GPUTools.Skinner.Scripts.Utils;
using UnityEngine;

namespace GPUTools.Skinner.Scripts.Providers
{
	// Token: 0x02000A92 RID: 2706
	[Serializable]
	public class SkinnedMeshProvider : IMeshProvider
	{
		// Token: 0x0600463B RID: 17979 RVA: 0x00140974 File Offset: 0x0013ED74
		public SkinnedMeshProvider()
		{
		}

		// Token: 0x0600463C RID: 17980 RVA: 0x0014097C File Offset: 0x0013ED7C
		public bool Validate(bool log)
		{
			if (log)
			{
			}
			return this.SkinnedMeshRenderer != null && this.SkinnedMeshRenderer.sharedMesh != null;
		}

		// Token: 0x0600463D RID: 17981 RVA: 0x001409A9 File Offset: 0x0013EDA9
		private void UpdateToWorldMatricesBufferGPU()
		{
			if (this.gpuSkinner == null)
			{
				this.gpuSkinner = new GPUSkinnerPro(this.SkinnedMeshRenderer);
			}
		}

		// Token: 0x1700099D RID: 2461
		// (get) Token: 0x0600463E RID: 17982 RVA: 0x001409C7 File Offset: 0x0013EDC7
		public GpuBuffer<Matrix4x4> ToWorldMatricesBuffer
		{
			get
			{
				this.UpdateToWorldMatricesBufferGPU();
				return this.gpuSkinner.TransformMatricesBuffer;
			}
		}

		// Token: 0x1700099E RID: 2462
		// (get) Token: 0x0600463F RID: 17983 RVA: 0x001409DA File Offset: 0x0013EDDA
		public GpuBuffer<Vector3> NormalsBuffer
		{
			get
			{
				return null;
			}
		}

		// Token: 0x1700099F RID: 2463
		// (get) Token: 0x06004640 RID: 17984 RVA: 0x001409DD File Offset: 0x0013EDDD
		public Matrix4x4 ToWorldMatrix
		{
			get
			{
				return MeshSkinUtils.CreateToWorldMatrix(this.SkinnedMeshRenderer);
			}
		}

		// Token: 0x170009A0 RID: 2464
		// (get) Token: 0x06004641 RID: 17985 RVA: 0x001409EA File Offset: 0x0013EDEA
		public GpuBuffer<Vector3> PreCalculatedVerticesBuffer
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170009A1 RID: 2465
		// (get) Token: 0x06004642 RID: 17986 RVA: 0x001409ED File Offset: 0x0013EDED
		public Mesh Mesh
		{
			get
			{
				return this.SkinnedMeshRenderer.sharedMesh;
			}
		}

		// Token: 0x170009A2 RID: 2466
		// (get) Token: 0x06004643 RID: 17987 RVA: 0x001409FA File Offset: 0x0013EDFA
		public Mesh MeshForImport
		{
			get
			{
				return this.SkinnedMeshRenderer.sharedMesh;
			}
		}

		// Token: 0x06004644 RID: 17988 RVA: 0x00140A07 File Offset: 0x0013EE07
		public void Dispatch()
		{
			if (this.gpuSkinner != null)
			{
				this.gpuSkinner.Dispatch();
			}
		}

		// Token: 0x06004645 RID: 17989 RVA: 0x00140A1F File Offset: 0x0013EE1F
		public void Dispose()
		{
			if (this.gpuSkinner != null)
			{
				this.gpuSkinner.Dispose();
			}
		}

		// Token: 0x040033BC RID: 13244
		[SerializeField]
		public SkinnedMeshRenderer SkinnedMeshRenderer;

		// Token: 0x040033BD RID: 13245
		private GPUSkinnerPro gpuSkinner;
	}
}
