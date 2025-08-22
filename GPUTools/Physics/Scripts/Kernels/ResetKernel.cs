using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Dynamic;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Kernels
{
	// Token: 0x02000A71 RID: 2673
	public class ResetKernel : KernelBase
	{
		// Token: 0x06004546 RID: 17734 RVA: 0x0013EC24 File Offset: 0x0013D024
		public ResetKernel() : base("Compute/Reset", "CSReset")
		{
		}

		// Token: 0x17000949 RID: 2377
		// (get) Token: 0x06004548 RID: 17736 RVA: 0x0013EC3F File Offset: 0x0013D03F
		// (set) Token: 0x06004547 RID: 17735 RVA: 0x0013EC36 File Offset: 0x0013D036
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

		// Token: 0x06004549 RID: 17737 RVA: 0x0013EC47 File Offset: 0x0013D047
		public override int GetGroupsNumX()
		{
			if (this.Particles != null)
			{
				return Mathf.CeilToInt((float)this.Particles.ComputeBuffer.count / 256f);
			}
			return 0;
		}

		// Token: 0x04003322 RID: 13090
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;
	}
}
