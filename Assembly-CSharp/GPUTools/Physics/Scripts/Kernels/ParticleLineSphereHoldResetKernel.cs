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
	// Token: 0x02000A60 RID: 2656
	public class ParticleLineSphereHoldResetKernel : KernelBase
	{
		// Token: 0x0600449C RID: 17564 RVA: 0x0013E3CF File Offset: 0x0013C7CF
		public ParticleLineSphereHoldResetKernel() : base("Compute/ParticleLineSphereHoldReset", "CSParticleLineSphereHoldReset")
		{
		}

		// Token: 0x17000905 RID: 2309
		// (get) Token: 0x0600449E RID: 17566 RVA: 0x0013E3EA File Offset: 0x0013C7EA
		// (set) Token: 0x0600449D RID: 17565 RVA: 0x0013E3E1 File Offset: 0x0013C7E1
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

		// Token: 0x0600449F RID: 17567 RVA: 0x0013E3F2 File Offset: 0x0013C7F2
		public override int GetGroupsNumX()
		{
			if (this.Particles != null)
			{
				return Mathf.CeilToInt((float)this.Particles.Count / 256f);
			}
			return 0;
		}

		// Token: 0x040032DE RID: 13022
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;
	}
}
