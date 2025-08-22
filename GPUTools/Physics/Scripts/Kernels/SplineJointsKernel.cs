using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Joints;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Kernels
{
	// Token: 0x02000A75 RID: 2677
	public class SplineJointsKernel : KernelBase
	{
		// Token: 0x06004566 RID: 17766 RVA: 0x0013EDEB File Offset: 0x0013D1EB
		public SplineJointsKernel() : base("Compute/SplineJoints", "CSSplineJoints")
		{
		}

		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x06004568 RID: 17768 RVA: 0x0013EE06 File Offset: 0x0013D206
		// (set) Token: 0x06004567 RID: 17767 RVA: 0x0013EDFD File Offset: 0x0013D1FD
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

		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x0600456A RID: 17770 RVA: 0x0013EE17 File Offset: 0x0013D217
		// (set) Token: 0x06004569 RID: 17769 RVA: 0x0013EE0E File Offset: 0x0013D20E
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

		// Token: 0x17000957 RID: 2391
		// (get) Token: 0x0600456C RID: 17772 RVA: 0x0013EE28 File Offset: 0x0013D228
		// (set) Token: 0x0600456B RID: 17771 RVA: 0x0013EE1F File Offset: 0x0013D21F
		[GpuData("pointJoints")]
		public GpuBuffer<GPPointJoint> PointJoints
		{
			[CompilerGenerated]
			get
			{
				return this.<PointJoints>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<PointJoints>k__BackingField = value;
			}
		}

		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x0600456E RID: 17774 RVA: 0x0013EE39 File Offset: 0x0013D239
		// (set) Token: 0x0600456D RID: 17773 RVA: 0x0013EE30 File Offset: 0x0013D230
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

		// Token: 0x17000959 RID: 2393
		// (get) Token: 0x06004570 RID: 17776 RVA: 0x0013EE4A File Offset: 0x0013D24A
		// (set) Token: 0x0600456F RID: 17775 RVA: 0x0013EE41 File Offset: 0x0013D241
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

		// Token: 0x06004571 RID: 17777 RVA: 0x0013EE52 File Offset: 0x0013D252
		public override int GetGroupsNumX()
		{
			if (this.Particles != null)
			{
				return Mathf.CeilToInt((float)(this.Particles.Count / this.Segments.Value) / 256f);
			}
			return 0;
		}

		// Token: 0x0400332E RID: 13102
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <Segments>k__BackingField;

		// Token: 0x0400332F RID: 13103
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x04003330 RID: 13104
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPPointJoint> <PointJoints>k__BackingField;

		// Token: 0x04003331 RID: 13105
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<float> <PointToPreviousPointDistances>k__BackingField;

		// Token: 0x04003332 RID: 13106
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <SplineJointPower>k__BackingField;
	}
}
