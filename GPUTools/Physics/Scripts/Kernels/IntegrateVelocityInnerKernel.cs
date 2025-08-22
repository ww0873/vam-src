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
	// Token: 0x02000A53 RID: 2643
	public class IntegrateVelocityInnerKernel : KernelBase
	{
		// Token: 0x06004426 RID: 17446 RVA: 0x0013DCFA File Offset: 0x0013C0FA
		public IntegrateVelocityInnerKernel() : base("Compute/IntegrateVelocityInner", "CSIntegrateVelocityInner")
		{
		}

		// Token: 0x170008D7 RID: 2263
		// (get) Token: 0x06004428 RID: 17448 RVA: 0x0013DD15 File Offset: 0x0013C115
		// (set) Token: 0x06004427 RID: 17447 RVA: 0x0013DD0C File Offset: 0x0013C10C
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

		// Token: 0x170008D8 RID: 2264
		// (get) Token: 0x0600442A RID: 17450 RVA: 0x0013DD26 File Offset: 0x0013C126
		// (set) Token: 0x06004429 RID: 17449 RVA: 0x0013DD1D File Offset: 0x0013C11D
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

		// Token: 0x170008D9 RID: 2265
		// (get) Token: 0x0600442C RID: 17452 RVA: 0x0013DD37 File Offset: 0x0013C137
		// (set) Token: 0x0600442B RID: 17451 RVA: 0x0013DD2E File Offset: 0x0013C12E
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

		// Token: 0x0600442D RID: 17453 RVA: 0x0013DD3F File Offset: 0x0013C13F
		public override int GetGroupsNumX()
		{
			if (this.Particles != null)
			{
				return Mathf.CeilToInt((float)this.Particles.ComputeBuffer.count / 256f);
			}
			return 0;
		}

		// Token: 0x040032B0 RID: 12976
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x040032B1 RID: 12977
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <DTRecip>k__BackingField;

		// Token: 0x040032B2 RID: 12978
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <Step>k__BackingField;
	}
}
