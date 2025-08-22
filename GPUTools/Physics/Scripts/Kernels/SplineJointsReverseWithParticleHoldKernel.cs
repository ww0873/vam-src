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
	// Token: 0x02000A77 RID: 2679
	public class SplineJointsReverseWithParticleHoldKernel : KernelBase
	{
		// Token: 0x0600457E RID: 17790 RVA: 0x0013EF1D File Offset: 0x0013D31D
		public SplineJointsReverseWithParticleHoldKernel() : base("Compute/SplineJointsReverseWithParticleHold", "CSSplineJointsReverseWithParticleHold")
		{
		}

		// Token: 0x1700095F RID: 2399
		// (get) Token: 0x06004580 RID: 17792 RVA: 0x0013EF38 File Offset: 0x0013D338
		// (set) Token: 0x0600457F RID: 17791 RVA: 0x0013EF2F File Offset: 0x0013D32F
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

		// Token: 0x17000960 RID: 2400
		// (get) Token: 0x06004582 RID: 17794 RVA: 0x0013EF49 File Offset: 0x0013D349
		// (set) Token: 0x06004581 RID: 17793 RVA: 0x0013EF40 File Offset: 0x0013D340
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

		// Token: 0x17000961 RID: 2401
		// (get) Token: 0x06004584 RID: 17796 RVA: 0x0013EF5A File Offset: 0x0013D35A
		// (set) Token: 0x06004583 RID: 17795 RVA: 0x0013EF51 File Offset: 0x0013D351
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

		// Token: 0x17000962 RID: 2402
		// (get) Token: 0x06004586 RID: 17798 RVA: 0x0013EF6B File Offset: 0x0013D36B
		// (set) Token: 0x06004585 RID: 17797 RVA: 0x0013EF62 File Offset: 0x0013D362
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

		// Token: 0x17000963 RID: 2403
		// (get) Token: 0x06004588 RID: 17800 RVA: 0x0013EF7C File Offset: 0x0013D37C
		// (set) Token: 0x06004587 RID: 17799 RVA: 0x0013EF73 File Offset: 0x0013D373
		[GpuData("reverseSplineJointPower")]
		public GpuValue<float> ReverseSplineJointPower
		{
			[CompilerGenerated]
			get
			{
				return this.<ReverseSplineJointPower>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ReverseSplineJointPower>k__BackingField = value;
			}
		}

		// Token: 0x06004589 RID: 17801 RVA: 0x0013EF84 File Offset: 0x0013D384
		public override int GetGroupsNumX()
		{
			if (this.Particles != null)
			{
				return Mathf.CeilToInt((float)(this.Particles.Count / this.Segments.Value) / 256f);
			}
			return 0;
		}

		// Token: 0x04003338 RID: 13112
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <Segments>k__BackingField;

		// Token: 0x04003339 RID: 13113
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x0400333A RID: 13114
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<float> <PointToPreviousPointDistances>k__BackingField;

		// Token: 0x0400333B RID: 13115
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <SplineJointPower>k__BackingField;

		// Token: 0x0400333C RID: 13116
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <ReverseSplineJointPower>k__BackingField;
	}
}
