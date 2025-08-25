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
	// Token: 0x02000A57 RID: 2647
	public class ParticleCollisionResetKernel : KernelBase
	{
		// Token: 0x0600444E RID: 17486 RVA: 0x0013DFA9 File Offset: 0x0013C3A9
		public ParticleCollisionResetKernel() : base("Compute/ParticleCollisionReset", "CSParticleCollisionReset")
		{
		}

		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x06004450 RID: 17488 RVA: 0x0013DFC4 File Offset: 0x0013C3C4
		// (set) Token: 0x0600444F RID: 17487 RVA: 0x0013DFBB File Offset: 0x0013C3BB
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

		// Token: 0x06004451 RID: 17489 RVA: 0x0013DFCC File Offset: 0x0013C3CC
		public override int GetGroupsNumX()
		{
			if (this.Particles != null)
			{
				return Mathf.CeilToInt((float)this.Particles.Count / 256f);
			}
			return 0;
		}

		// Token: 0x040032C0 RID: 12992
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;
	}
}
