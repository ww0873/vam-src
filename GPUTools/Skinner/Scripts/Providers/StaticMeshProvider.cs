using System;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Skinner.Scripts.Abstract;
using UnityEngine;

namespace GPUTools.Skinner.Scripts.Providers
{
	// Token: 0x02000A93 RID: 2707
	[Serializable]
	public class StaticMeshProvider : IMeshProvider
	{
		// Token: 0x06004646 RID: 17990 RVA: 0x00140A37 File Offset: 0x0013EE37
		public StaticMeshProvider()
		{
		}

		// Token: 0x06004647 RID: 17991 RVA: 0x00140A3F File Offset: 0x0013EE3F
		public bool Validate(bool log)
		{
			if (log)
			{
			}
			return this.MeshFilter != null;
		}

		// Token: 0x06004648 RID: 17992 RVA: 0x00140A54 File Offset: 0x0013EE54
		private void UpdateToWorldMatrices()
		{
			if (this.toWorldMatricesBuffer == null)
			{
				this.toWorldMatricesBuffer = new GpuBuffer<Matrix4x4>(1, 64);
			}
			this.toWorldMatricesBuffer.Data[0] = this.MeshFilter.transform.localToWorldMatrix;
			this.toWorldMatricesBuffer.PushData();
		}

		// Token: 0x170009A3 RID: 2467
		// (get) Token: 0x06004649 RID: 17993 RVA: 0x00140AAB File Offset: 0x0013EEAB
		public Matrix4x4 ToWorldMatrix
		{
			get
			{
				return this.MeshFilter.transform.localToWorldMatrix;
			}
		}

		// Token: 0x170009A4 RID: 2468
		// (get) Token: 0x0600464A RID: 17994 RVA: 0x00140ABD File Offset: 0x0013EEBD
		public GpuBuffer<Matrix4x4> ToWorldMatricesBuffer
		{
			get
			{
				this.UpdateToWorldMatrices();
				return this.toWorldMatricesBuffer;
			}
		}

		// Token: 0x170009A5 RID: 2469
		// (get) Token: 0x0600464B RID: 17995 RVA: 0x00140ACB File Offset: 0x0013EECB
		public GpuBuffer<Vector3> NormalsBuffer
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170009A6 RID: 2470
		// (get) Token: 0x0600464C RID: 17996 RVA: 0x00140ACE File Offset: 0x0013EECE
		public GpuBuffer<Vector3> PreCalculatedVerticesBuffer
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170009A7 RID: 2471
		// (get) Token: 0x0600464D RID: 17997 RVA: 0x00140AD1 File Offset: 0x0013EED1
		public Mesh Mesh
		{
			get
			{
				if (!(this.MeshFilter != null))
				{
					return null;
				}
				if (Application.isPlaying)
				{
					return this.MeshFilter.mesh;
				}
				return this.MeshFilter.sharedMesh;
			}
		}

		// Token: 0x170009A8 RID: 2472
		// (get) Token: 0x0600464E RID: 17998 RVA: 0x00140B07 File Offset: 0x0013EF07
		public Mesh MeshForImport
		{
			get
			{
				if (!(this.MeshFilter != null))
				{
					return null;
				}
				if (Application.isPlaying)
				{
					return this.MeshFilter.mesh;
				}
				return this.MeshFilter.sharedMesh;
			}
		}

		// Token: 0x0600464F RID: 17999 RVA: 0x00140B3D File Offset: 0x0013EF3D
		public void Dispatch()
		{
			this.UpdateToWorldMatrices();
		}

		// Token: 0x06004650 RID: 18000 RVA: 0x00140B45 File Offset: 0x0013EF45
		public void Dispose()
		{
			if (this.toWorldMatricesBuffer != null)
			{
				this.toWorldMatricesBuffer.Dispose();
			}
		}

		// Token: 0x040033BE RID: 13246
		[SerializeField]
		public MeshFilter MeshFilter;

		// Token: 0x040033BF RID: 13247
		private GpuBuffer<Matrix4x4> toWorldMatricesBuffer;

		// Token: 0x040033C0 RID: 13248
		private GpuBuffer<Matrix4x4> oldToWorldMatricesBuffer;
	}
}
