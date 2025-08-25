using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Hair.Scripts.Runtime.Render;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Shapes;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Kernels
{
	// Token: 0x02000A5C RID: 2652
	public class ParticleLineSphereCutKernel : KernelBase
	{
		// Token: 0x06004476 RID: 17526 RVA: 0x0013E1CC File Offset: 0x0013C5CC
		public ParticleLineSphereCutKernel() : base("Compute/ParticleLineSphereCut", "CSParticleLineSphereCut")
		{
		}

		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x06004478 RID: 17528 RVA: 0x0013E1E7 File Offset: 0x0013C5E7
		// (set) Token: 0x06004477 RID: 17527 RVA: 0x0013E1DE File Offset: 0x0013C5DE
		[GpuData("segments")]
		public GpuValue<int> Segments
		{
			[CompilerGenerated]
			get
			{
				return this.<Segments>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Segments>k__BackingField = value;
			}
		}

		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x0600447A RID: 17530 RVA: 0x0013E1F8 File Offset: 0x0013C5F8
		// (set) Token: 0x06004479 RID: 17529 RVA: 0x0013E1EF File Offset: 0x0013C5EF
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

		// Token: 0x170008F8 RID: 2296
		// (get) Token: 0x0600447C RID: 17532 RVA: 0x0013E209 File Offset: 0x0013C609
		// (set) Token: 0x0600447B RID: 17531 RVA: 0x0013E200 File Offset: 0x0013C600
		[GpuData("pointToPreviousPointDistances")]
		public GpuBuffer<float> PointToPreviousPointDistances
		{
			[CompilerGenerated]
			get
			{
				return this.<PointToPreviousPointDistances>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<PointToPreviousPointDistances>k__BackingField = value;
			}
		}

		// Token: 0x170008F9 RID: 2297
		// (get) Token: 0x0600447E RID: 17534 RVA: 0x0013E21A File Offset: 0x0013C61A
		// (set) Token: 0x0600447D RID: 17533 RVA: 0x0013E211 File Offset: 0x0013C611
		[GpuData("renderParticles")]
		public GpuBuffer<RenderParticle> RenderParticles
		{
			[CompilerGenerated]
			get
			{
				return this.<RenderParticles>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<RenderParticles>k__BackingField = value;
			}
		}

		// Token: 0x170008FA RID: 2298
		// (get) Token: 0x06004480 RID: 17536 RVA: 0x0013E22B File Offset: 0x0013C62B
		// (set) Token: 0x0600447F RID: 17535 RVA: 0x0013E222 File Offset: 0x0013C622
		[GpuData("cutLineSpheres")]
		public GpuBuffer<GPLineSphere> CutLineSpheres
		{
			[CompilerGenerated]
			get
			{
				return this.<CutLineSpheres>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CutLineSpheres>k__BackingField = value;
			}
		}

		// Token: 0x06004481 RID: 17537 RVA: 0x0013E233 File Offset: 0x0013C633
		public override int GetGroupsNumX()
		{
			if (this.Particles != null)
			{
				return Mathf.CeilToInt((float)(this.Particles.Count / this.Segments.Value) / 256f);
			}
			return 0;
		}

		// Token: 0x040032CF RID: 13007
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <Segments>k__BackingField;

		// Token: 0x040032D0 RID: 13008
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x040032D1 RID: 13009
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<float> <PointToPreviousPointDistances>k__BackingField;

		// Token: 0x040032D2 RID: 13010
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<RenderParticle> <RenderParticles>k__BackingField;

		// Token: 0x040032D3 RID: 13011
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphere> <CutLineSpheres>k__BackingField;
	}
}
