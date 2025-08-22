using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Dynamic;

namespace GPUTools.Physics.Scripts.Kernels
{
	// Token: 0x02000A4A RID: 2634
	public class CopySpecificParticlesKernel : KernelBase
	{
		// Token: 0x060043CC RID: 17356 RVA: 0x0013D68F File Offset: 0x0013BA8F
		public CopySpecificParticlesKernel() : base("Compute/CopySpecificParticles", "CSCopySpecificParticles")
		{
		}

		// Token: 0x170008B3 RID: 2227
		// (get) Token: 0x060043CE RID: 17358 RVA: 0x0013D6AA File Offset: 0x0013BAAA
		// (set) Token: 0x060043CD RID: 17357 RVA: 0x0013D6A1 File Offset: 0x0013BAA1
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

		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x060043D0 RID: 17360 RVA: 0x0013D6BB File Offset: 0x0013BABB
		// (set) Token: 0x060043CF RID: 17359 RVA: 0x0013D6B2 File Offset: 0x0013BAB2
		[GpuData("outParticles")]
		public GpuBuffer<GPParticle> OutParticles
		{
			[CompilerGenerated]
			get
			{
				return this.<OutParticles>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<OutParticles>k__BackingField = value;
			}
		}

		// Token: 0x170008B5 RID: 2229
		// (get) Token: 0x060043D2 RID: 17362 RVA: 0x0013D6CC File Offset: 0x0013BACC
		// (set) Token: 0x060043D1 RID: 17361 RVA: 0x0013D6C3 File Offset: 0x0013BAC3
		[GpuData("outParticlesMap")]
		public GpuBuffer<float> OutParticlesMap
		{
			[CompilerGenerated]
			get
			{
				return this.<OutParticlesMap>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<OutParticlesMap>k__BackingField = value;
			}
		}

		// Token: 0x060043D3 RID: 17363 RVA: 0x0013D6D4 File Offset: 0x0013BAD4
		public override int GetGroupsNumX()
		{
			return 1;
		}

		// Token: 0x0400328C RID: 12940
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x0400328D RID: 12941
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <OutParticles>k__BackingField;

		// Token: 0x0400328E RID: 12942
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<float> <OutParticlesMap>k__BackingField;
	}
}
