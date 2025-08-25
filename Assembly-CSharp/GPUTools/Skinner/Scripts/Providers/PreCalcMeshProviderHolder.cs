using System;
using GPUTools.Common.Scripts.PL.Tools;
using UnityEngine;

namespace GPUTools.Skinner.Scripts.Providers
{
	// Token: 0x02000A91 RID: 2705
	public class PreCalcMeshProviderHolder : PreCalcMeshProvider
	{
		// Token: 0x0600462B RID: 17963 RVA: 0x0014079B File Offset: 0x0013EB9B
		public PreCalcMeshProviderHolder()
		{
		}

		// Token: 0x17000994 RID: 2452
		// (get) Token: 0x0600462C RID: 17964 RVA: 0x001407A3 File Offset: 0x0013EBA3
		// (set) Token: 0x0600462D RID: 17965 RVA: 0x001407AB File Offset: 0x0013EBAB
		public PreCalcMeshProvider provider
		{
			get
			{
				return this._provider;
			}
			set
			{
				if (this._provider != value)
				{
					this._provider = value;
				}
			}
		}

		// Token: 0x0600462E RID: 17966 RVA: 0x001407C5 File Offset: 0x0013EBC5
		public override bool Validate(bool log)
		{
			return this.provider != null && this.provider.Validate(log);
		}

		// Token: 0x17000995 RID: 2453
		// (get) Token: 0x0600462F RID: 17967 RVA: 0x001407E6 File Offset: 0x0013EBE6
		public override Matrix4x4 ToWorldMatrix
		{
			get
			{
				if (this.provider != null)
				{
					return this.provider.ToWorldMatrix;
				}
				return Matrix4x4.identity;
			}
		}

		// Token: 0x17000996 RID: 2454
		// (get) Token: 0x06004630 RID: 17968 RVA: 0x0014080A File Offset: 0x0013EC0A
		public override GpuBuffer<Matrix4x4> ToWorldMatricesBuffer
		{
			get
			{
				if (this.provider != null)
				{
					return this.provider.ToWorldMatricesBuffer;
				}
				return null;
			}
		}

		// Token: 0x17000997 RID: 2455
		// (get) Token: 0x06004631 RID: 17969 RVA: 0x0014082A File Offset: 0x0013EC2A
		public override GpuBuffer<Vector3> PreCalculatedVerticesBuffer
		{
			get
			{
				if (this.provider != null)
				{
					return this.provider.PreCalculatedVerticesBuffer;
				}
				return null;
			}
		}

		// Token: 0x17000998 RID: 2456
		// (get) Token: 0x06004632 RID: 17970 RVA: 0x0014084A File Offset: 0x0013EC4A
		public override GpuBuffer<Vector3> NormalsBuffer
		{
			get
			{
				if (this.provider != null)
				{
					return this.provider.NormalsBuffer;
				}
				return null;
			}
		}

		// Token: 0x17000999 RID: 2457
		// (get) Token: 0x06004633 RID: 17971 RVA: 0x0014086A File Offset: 0x0013EC6A
		public override Mesh Mesh
		{
			get
			{
				if (this.provider != null)
				{
					return this.provider.Mesh;
				}
				return null;
			}
		}

		// Token: 0x1700099A RID: 2458
		// (get) Token: 0x06004634 RID: 17972 RVA: 0x0014088A File Offset: 0x0013EC8A
		public override Mesh BaseMesh
		{
			get
			{
				if (this.provider != null)
				{
					return this.provider.BaseMesh;
				}
				return null;
			}
		}

		// Token: 0x1700099B RID: 2459
		// (get) Token: 0x06004635 RID: 17973 RVA: 0x001408AA File Offset: 0x0013ECAA
		public override Mesh MeshForImport
		{
			get
			{
				if (this.provider != null)
				{
					return this.provider.MeshForImport;
				}
				return null;
			}
		}

		// Token: 0x1700099C RID: 2460
		// (get) Token: 0x06004636 RID: 17974 RVA: 0x001408CA File Offset: 0x0013ECCA
		public override Color[] VertexSimColors
		{
			get
			{
				if (this.provider != null)
				{
					return this.provider.VertexSimColors;
				}
				return null;
			}
		}

		// Token: 0x06004637 RID: 17975 RVA: 0x001408EA File Offset: 0x0013ECEA
		public override void Stop()
		{
			if (this.provider != null)
			{
				this.provider.Stop();
			}
		}

		// Token: 0x06004638 RID: 17976 RVA: 0x00140908 File Offset: 0x0013ED08
		public override void Dispatch()
		{
			if (this.provider != null)
			{
				this.provider.provideToWorldMatrices = this.provideToWorldMatrices;
				this.provider.Dispatch();
			}
		}

		// Token: 0x06004639 RID: 17977 RVA: 0x00140937 File Offset: 0x0013ED37
		public override void PostProcessDispatch(ComputeBuffer finalVerts)
		{
			if (this.provider != null)
			{
				this.provider.PostProcessDispatch(finalVerts);
			}
		}

		// Token: 0x0600463A RID: 17978 RVA: 0x00140956 File Offset: 0x0013ED56
		public override void Dispose()
		{
			if (this.provider != null)
			{
				this.provider.Dispose();
			}
		}

		// Token: 0x040033BB RID: 13243
		protected PreCalcMeshProvider _provider;
	}
}
