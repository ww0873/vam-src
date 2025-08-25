using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Shapes;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Kernels
{
	// Token: 0x02000A62 RID: 2658
	public class ParticleLineSpherePullKernel : KernelBase
	{
		// Token: 0x060044AA RID: 17578 RVA: 0x0013E494 File Offset: 0x0013C894
		public ParticleLineSpherePullKernel() : base("Compute/ParticleLineSpherePull", "CSParticleLineSpherePull")
		{
		}

		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x060044AC RID: 17580 RVA: 0x0013E4AF File Offset: 0x0013C8AF
		// (set) Token: 0x060044AB RID: 17579 RVA: 0x0013E4A6 File Offset: 0x0013C8A6
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

		// Token: 0x1700090B RID: 2315
		// (get) Token: 0x060044AE RID: 17582 RVA: 0x0013E4C0 File Offset: 0x0013C8C0
		// (set) Token: 0x060044AD RID: 17581 RVA: 0x0013E4B7 File Offset: 0x0013C8B7
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

		// Token: 0x1700090C RID: 2316
		// (get) Token: 0x060044B0 RID: 17584 RVA: 0x0013E4D1 File Offset: 0x0013C8D1
		// (set) Token: 0x060044AF RID: 17583 RVA: 0x0013E4C8 File Offset: 0x0013C8C8
		[GpuData("pullLineSpheres")]
		public GpuBuffer<GPLineSphere> PullLineSpheres
		{
			[CompilerGenerated]
			get
			{
				return this.<PullLineSpheres>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<PullLineSpheres>k__BackingField = value;
			}
		}

		// Token: 0x060044B1 RID: 17585 RVA: 0x0013E4D9 File Offset: 0x0013C8D9
		public override int GetGroupsNumX()
		{
			if (this.Particles != null)
			{
				return Mathf.CeilToInt((float)this.Particles.Count / 256f);
			}
			return 0;
		}

		// Token: 0x040032E3 RID: 13027
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <Segments>k__BackingField;

		// Token: 0x040032E4 RID: 13028
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x040032E5 RID: 13029
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphere> <PullLineSpheres>k__BackingField;
	}
}
