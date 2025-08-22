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
	// Token: 0x02000A54 RID: 2644
	public class IntegrateVelocityKernel : KernelBase
	{
		// Token: 0x0600442E RID: 17454 RVA: 0x0013DD6A File Offset: 0x0013C16A
		public IntegrateVelocityKernel() : base("Compute/IntegrateVelocity", "CSIntegrateVelocity")
		{
		}

		// Token: 0x170008DA RID: 2266
		// (get) Token: 0x06004430 RID: 17456 RVA: 0x0013DD85 File Offset: 0x0013C185
		// (set) Token: 0x0600442F RID: 17455 RVA: 0x0013DD7C File Offset: 0x0013C17C
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

		// Token: 0x170008DB RID: 2267
		// (get) Token: 0x06004432 RID: 17458 RVA: 0x0013DD96 File Offset: 0x0013C196
		// (set) Token: 0x06004431 RID: 17457 RVA: 0x0013DD8D File Offset: 0x0013C18D
		[GpuData("dtrecip")]
		public GpuValue<float> DTRecip
		{
			[CompilerGenerated]
			get
			{
				return this.<DTRecip>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DTRecip>k__BackingField = value;
			}
		}

		// Token: 0x170008DC RID: 2268
		// (get) Token: 0x06004434 RID: 17460 RVA: 0x0013DDA7 File Offset: 0x0013C1A7
		// (set) Token: 0x06004433 RID: 17459 RVA: 0x0013DD9E File Offset: 0x0013C19E
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

		// Token: 0x06004435 RID: 17461 RVA: 0x0013DDAF File Offset: 0x0013C1AF
		public override int GetGroupsNumX()
		{
			if (this.Particles != null)
			{
				return Mathf.CeilToInt((float)this.Particles.ComputeBuffer.count / 256f);
			}
			return 0;
		}

		// Token: 0x040032B3 RID: 12979
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x040032B4 RID: 12980
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <DTRecip>k__BackingField;

		// Token: 0x040032B5 RID: 12981
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <Step>k__BackingField;
	}
}
