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
	// Token: 0x02000A51 RID: 2641
	public class IntegrateIterWithParticleHoldKernel : KernelBase
	{
		// Token: 0x06004414 RID: 17428 RVA: 0x0013DC09 File Offset: 0x0013C009
		public IntegrateIterWithParticleHoldKernel() : base("Compute/IntegrateIterWithParticleHold", "CSIntegrateIterWithParticleHold")
		{
		}

		// Token: 0x170008D0 RID: 2256
		// (get) Token: 0x06004416 RID: 17430 RVA: 0x0013DC24 File Offset: 0x0013C024
		// (set) Token: 0x06004415 RID: 17429 RVA: 0x0013DC1B File Offset: 0x0013C01B
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

		// Token: 0x170008D1 RID: 2257
		// (get) Token: 0x06004418 RID: 17432 RVA: 0x0013DC35 File Offset: 0x0013C035
		// (set) Token: 0x06004417 RID: 17431 RVA: 0x0013DC2C File Offset: 0x0013C02C
		[GpuData("dt")]
		public GpuValue<float> DT
		{
			[CompilerGenerated]
			get
			{
				return this.<DT>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DT>k__BackingField = value;
			}
		}

		// Token: 0x170008D2 RID: 2258
		// (get) Token: 0x0600441A RID: 17434 RVA: 0x0013DC46 File Offset: 0x0013C046
		// (set) Token: 0x06004419 RID: 17433 RVA: 0x0013DC3D File Offset: 0x0013C03D
		[GpuData("invDrag")]
		public GpuValue<float> InvDrag
		{
			[CompilerGenerated]
			get
			{
				return this.<InvDrag>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<InvDrag>k__BackingField = value;
			}
		}

		// Token: 0x170008D3 RID: 2259
		// (get) Token: 0x0600441C RID: 17436 RVA: 0x0013DC57 File Offset: 0x0013C057
		// (set) Token: 0x0600441B RID: 17435 RVA: 0x0013DC4E File Offset: 0x0013C04E
		[GpuData("accelDT2")]
		public GpuValue<Vector3> AccelDT2
		{
			[CompilerGenerated]
			get
			{
				return this.<AccelDT2>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<AccelDT2>k__BackingField = value;
			}
		}

		// Token: 0x0600441D RID: 17437 RVA: 0x0013DC5F File Offset: 0x0013C05F
		public override int GetGroupsNumX()
		{
			if (this.Particles != null)
			{
				return Mathf.CeilToInt((float)this.Particles.ComputeBuffer.count / 256f);
			}
			return 0;
		}

		// Token: 0x040032A9 RID: 12969
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x040032AA RID: 12970
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <DT>k__BackingField;

		// Token: 0x040032AB RID: 12971
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <InvDrag>k__BackingField;

		// Token: 0x040032AC RID: 12972
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<Vector3> <AccelDT2>k__BackingField;
	}
}
