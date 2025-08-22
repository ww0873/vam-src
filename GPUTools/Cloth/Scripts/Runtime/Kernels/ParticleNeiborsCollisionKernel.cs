using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Dynamic;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Runtime.Kernels
{
	// Token: 0x020009A6 RID: 2470
	public class ParticleNeiborsCollisionKernel : KernelBase
	{
		// Token: 0x06003DFC RID: 15868 RVA: 0x0012B603 File Offset: 0x00129A03
		public ParticleNeiborsCollisionKernel() : base("Compute/ParticleNeiborsCollision", "CSParticleNeiborsCollision")
		{
		}

		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x06003DFE RID: 15870 RVA: 0x0012B61E File Offset: 0x00129A1E
		// (set) Token: 0x06003DFD RID: 15869 RVA: 0x0012B615 File Offset: 0x00129A15
		[GpuData("step")]
		public GpuValue<float> Step
		{
			[CompilerGenerated]
			get
			{
				return this.<Step>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Step>k__BackingField = value;
			}
		}

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x06003E00 RID: 15872 RVA: 0x0012B62F File Offset: 0x00129A2F
		// (set) Token: 0x06003DFF RID: 15871 RVA: 0x0012B626 File Offset: 0x00129A26
		[GpuData("t")]
		public GpuValue<float> T
		{
			[CompilerGenerated]
			get
			{
				return this.<T>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<T>k__BackingField = value;
			}
		}

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x06003E02 RID: 15874 RVA: 0x0012B640 File Offset: 0x00129A40
		// (set) Token: 0x06003E01 RID: 15873 RVA: 0x0012B637 File Offset: 0x00129A37
		[GpuData("facesForNormalNum")]
		public GpuValue<int> FacesForNormalNum
		{
			[CompilerGenerated]
			get
			{
				return this.<FacesForNormalNum>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<FacesForNormalNum>k__BackingField = value;
			}
		}

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x06003E04 RID: 15876 RVA: 0x0012B651 File Offset: 0x00129A51
		// (set) Token: 0x06003E03 RID: 15875 RVA: 0x0012B648 File Offset: 0x00129A48
		[GpuData("particles")]
		public GpuBuffer<GPParticle> Particles
		{
			[CompilerGenerated]
			get
			{
				return this.<Particles>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Particles>k__BackingField = value;
			}
		}

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x06003E05 RID: 15877 RVA: 0x0012B659 File Offset: 0x00129A59
		// (set) Token: 0x06003E06 RID: 15878 RVA: 0x0012B661 File Offset: 0x00129A61
		[GpuData("meshVertexToNeiborsMap")]
		public GpuBuffer<int> MeshVertexToNeiborsMap
		{
			[CompilerGenerated]
			get
			{
				return this.<MeshVertexToNeiborsMap>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<MeshVertexToNeiborsMap>k__BackingField = value;
			}
		}

		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x06003E07 RID: 15879 RVA: 0x0012B66A File Offset: 0x00129A6A
		// (set) Token: 0x06003E08 RID: 15880 RVA: 0x0012B672 File Offset: 0x00129A72
		[GpuData("meshVertexToNeiborsMapCounts")]
		public GpuBuffer<int> MeshVertexToNeiborsMapCounts
		{
			[CompilerGenerated]
			get
			{
				return this.<MeshVertexToNeiborsMapCounts>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<MeshVertexToNeiborsMapCounts>k__BackingField = value;
			}
		}

		// Token: 0x06003E09 RID: 15881 RVA: 0x0012B67B File Offset: 0x00129A7B
		public override int GetGroupsNumX()
		{
			if (this.Particles != null)
			{
				return Mathf.CeilToInt((float)this.Particles.Count / 256f);
			}
			return 0;
		}

		// Token: 0x04002F5E RID: 12126
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <Step>k__BackingField;

		// Token: 0x04002F5F RID: 12127
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <T>k__BackingField;

		// Token: 0x04002F60 RID: 12128
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <FacesForNormalNum>k__BackingField;

		// Token: 0x04002F61 RID: 12129
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x04002F62 RID: 12130
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<int> <MeshVertexToNeiborsMap>k__BackingField;

		// Token: 0x04002F63 RID: 12131
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<int> <MeshVertexToNeiborsMapCounts>k__BackingField;
	}
}
