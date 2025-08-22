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
	// Token: 0x02000A78 RID: 2680
	public class SplineJointsWithParticleHoldKernel : KernelBase
	{
		// Token: 0x0600458A RID: 17802 RVA: 0x0013EFB6 File Offset: 0x0013D3B6
		public SplineJointsWithParticleHoldKernel() : base("Compute/SplineJointsWithParticleHold", "CSSplineJointsWithParticleHold")
		{
		}

		// Token: 0x17000964 RID: 2404
		// (get) Token: 0x0600458C RID: 17804 RVA: 0x0013EFD1 File Offset: 0x0013D3D1
		// (set) Token: 0x0600458B RID: 17803 RVA: 0x0013EFC8 File Offset: 0x0013D3C8
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

		// Token: 0x17000965 RID: 2405
		// (get) Token: 0x0600458E RID: 17806 RVA: 0x0013EFE2 File Offset: 0x0013D3E2
		// (set) Token: 0x0600458D RID: 17805 RVA: 0x0013EFD9 File Offset: 0x0013D3D9
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

		// Token: 0x17000966 RID: 2406
		// (get) Token: 0x06004590 RID: 17808 RVA: 0x0013EFF3 File Offset: 0x0013D3F3
		// (set) Token: 0x0600458F RID: 17807 RVA: 0x0013EFEA File Offset: 0x0013D3EA
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

		// Token: 0x17000967 RID: 2407
		// (get) Token: 0x06004592 RID: 17810 RVA: 0x0013F004 File Offset: 0x0013D404
		// (set) Token: 0x06004591 RID: 17809 RVA: 0x0013EFFB File Offset: 0x0013D3FB
		[GpuData("splineJointPower")]
		public GpuValue<float> SplineJointPower
		{
			[CompilerGenerated]
			get
			{
				return this.<SplineJointPower>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<SplineJointPower>k__BackingField = value;
			}
		}

		// Token: 0x06004593 RID: 17811 RVA: 0x0013F00C File Offset: 0x0013D40C
		public override int GetGroupsNumX()
		{
			if (this.Particles != null)
			{
				return Mathf.CeilToInt((float)(this.Particles.Count / this.Segments.Value) / 256f);
			}
			return 0;
		}

		// Token: 0x0400333D RID: 13117
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <Segments>k__BackingField;

		// Token: 0x0400333E RID: 13118
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x0400333F RID: 13119
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<float> <PointToPreviousPointDistances>k__BackingField;

		// Token: 0x04003340 RID: 13120
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <SplineJointPower>k__BackingField;
	}
}
