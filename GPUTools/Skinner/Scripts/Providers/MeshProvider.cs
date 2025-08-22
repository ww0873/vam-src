using System;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Skinner.Scripts.Abstract;
using UnityEngine;

namespace GPUTools.Skinner.Scripts.Providers
{
	// Token: 0x02000A8F RID: 2703
	[Serializable]
	public class MeshProvider : IMeshProvider
	{
		// Token: 0x0600460E RID: 17934 RVA: 0x00140559 File Offset: 0x0013E959
		public MeshProvider()
		{
		}

		// Token: 0x0600460F RID: 17935 RVA: 0x00140578 File Offset: 0x0013E978
		public bool Validate(bool log)
		{
			IMeshProvider currentProvider = this.GetCurrentProvider();
			return currentProvider != null && currentProvider.Validate(log);
		}

		// Token: 0x06004610 RID: 17936 RVA: 0x0014059B File Offset: 0x0013E99B
		public void Stop()
		{
			if (this.PreCalcProvider != null)
			{
				this.PreCalcProvider.Stop();
			}
		}

		// Token: 0x06004611 RID: 17937 RVA: 0x001405BC File Offset: 0x0013E9BC
		public void Dispatch()
		{
			IMeshProvider currentProvider = this.GetCurrentProvider();
			if (currentProvider != null)
			{
				currentProvider.Dispatch();
			}
		}

		// Token: 0x06004612 RID: 17938 RVA: 0x001405DC File Offset: 0x0013E9DC
		public void Dispose()
		{
			IMeshProvider currentProvider = this.GetCurrentProvider();
			if (currentProvider != null)
			{
				currentProvider.Dispose();
			}
		}

		// Token: 0x06004613 RID: 17939 RVA: 0x001405FC File Offset: 0x0013E9FC
		private IMeshProvider GetCurrentProvider()
		{
			if (this.Type == ScalpMeshType.Static)
			{
				return this.StaticProvider;
			}
			if (this.Type == ScalpMeshType.PreCalc)
			{
				return this.PreCalcProvider;
			}
			return this.SkinnedProvider;
		}

		// Token: 0x17000985 RID: 2437
		// (get) Token: 0x06004614 RID: 17940 RVA: 0x0014062C File Offset: 0x0013EA2C
		public Matrix4x4 ToWorldMatrix
		{
			get
			{
				IMeshProvider currentProvider = this.GetCurrentProvider();
				if (currentProvider != null)
				{
					return currentProvider.ToWorldMatrix;
				}
				return Matrix4x4.identity;
			}
		}

		// Token: 0x17000986 RID: 2438
		// (get) Token: 0x06004615 RID: 17941 RVA: 0x00140654 File Offset: 0x0013EA54
		public GpuBuffer<Matrix4x4> ToWorldMatricesBuffer
		{
			get
			{
				IMeshProvider currentProvider = this.GetCurrentProvider();
				if (currentProvider != null)
				{
					return currentProvider.ToWorldMatricesBuffer;
				}
				return null;
			}
		}

		// Token: 0x17000987 RID: 2439
		// (get) Token: 0x06004616 RID: 17942 RVA: 0x00140678 File Offset: 0x0013EA78
		public GpuBuffer<Vector3> PreCalculatedVerticesBuffer
		{
			get
			{
				IMeshProvider currentProvider = this.GetCurrentProvider();
				if (currentProvider != null)
				{
					return currentProvider.PreCalculatedVerticesBuffer;
				}
				return null;
			}
		}

		// Token: 0x17000988 RID: 2440
		// (get) Token: 0x06004617 RID: 17943 RVA: 0x0014069C File Offset: 0x0013EA9C
		public GpuBuffer<Vector3> NormalsBuffer
		{
			get
			{
				IMeshProvider currentProvider = this.GetCurrentProvider();
				if (currentProvider != null)
				{
					return currentProvider.NormalsBuffer;
				}
				return null;
			}
		}

		// Token: 0x17000989 RID: 2441
		// (get) Token: 0x06004618 RID: 17944 RVA: 0x001406C0 File Offset: 0x0013EAC0
		public Mesh Mesh
		{
			get
			{
				IMeshProvider currentProvider = this.GetCurrentProvider();
				if (currentProvider != null)
				{
					return currentProvider.Mesh;
				}
				return null;
			}
		}

		// Token: 0x1700098A RID: 2442
		// (get) Token: 0x06004619 RID: 17945 RVA: 0x001406E2 File Offset: 0x0013EAE2
		public Color[] SimColors
		{
			get
			{
				if (this.Type == ScalpMeshType.PreCalc && this.PreCalcProvider != null)
				{
					return this.PreCalcProvider.VertexSimColors;
				}
				return null;
			}
		}

		// Token: 0x1700098B RID: 2443
		// (get) Token: 0x0600461A RID: 17946 RVA: 0x00140710 File Offset: 0x0013EB10
		public Mesh MeshForImport
		{
			get
			{
				IMeshProvider currentProvider = this.GetCurrentProvider();
				if (currentProvider != null)
				{
					return currentProvider.MeshForImport;
				}
				return null;
			}
		}

		// Token: 0x040033B1 RID: 13233
		public ScalpMeshType Type;

		// Token: 0x040033B2 RID: 13234
		[SerializeField]
		public SkinnedMeshProvider SkinnedProvider = new SkinnedMeshProvider();

		// Token: 0x040033B3 RID: 13235
		[SerializeField]
		public StaticMeshProvider StaticProvider = new StaticMeshProvider();

		// Token: 0x040033B4 RID: 13236
		[SerializeField]
		public PreCalcMeshProvider PreCalcProvider;
	}
}
