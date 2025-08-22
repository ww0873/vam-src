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
	// Token: 0x02000A5F RID: 2655
	public class ParticleLineSphereHoldKernel : KernelBase
	{
		// Token: 0x06004494 RID: 17556 RVA: 0x0013E364 File Offset: 0x0013C764
		public ParticleLineSphereHoldKernel() : base("Compute/ParticleLineSphereHold", "CSParticleLineSphereHold")
		{
		}

		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x06004496 RID: 17558 RVA: 0x0013E37F File Offset: 0x0013C77F
		// (set) Token: 0x06004495 RID: 17557 RVA: 0x0013E376 File Offset: 0x0013C776
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

		// Token: 0x17000903 RID: 2307
		// (get) Token: 0x06004498 RID: 17560 RVA: 0x0013E390 File Offset: 0x0013C790
		// (set) Token: 0x06004497 RID: 17559 RVA: 0x0013E387 File Offset: 0x0013C787
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

		// Token: 0x17000904 RID: 2308
		// (get) Token: 0x0600449A RID: 17562 RVA: 0x0013E3A1 File Offset: 0x0013C7A1
		// (set) Token: 0x06004499 RID: 17561 RVA: 0x0013E398 File Offset: 0x0013C798
		[GpuData("holdLineSpheres")]
		public GpuBuffer<GPLineSphere> HoldLineSpheres
		{
			[CompilerGenerated]
			get
			{
				return this.<HoldLineSpheres>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<HoldLineSpheres>k__BackingField = value;
			}
		}

		// Token: 0x0600449B RID: 17563 RVA: 0x0013E3A9 File Offset: 0x0013C7A9
		public override int GetGroupsNumX()
		{
			if (this.Particles != null)
			{
				return Mathf.CeilToInt((float)this.Particles.Count / 256f);
			}
			return 0;
		}

		// Token: 0x040032DB RID: 13019
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <Segments>k__BackingField;

		// Token: 0x040032DC RID: 13020
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x040032DD RID: 13021
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphere> <HoldLineSpheres>k__BackingField;
	}
}
