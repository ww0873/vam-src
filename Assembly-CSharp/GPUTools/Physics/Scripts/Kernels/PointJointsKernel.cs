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
	// Token: 0x02000A6E RID: 2670
	public class PointJointsKernel : KernelBase
	{
		// Token: 0x0600451A RID: 17690 RVA: 0x0013EA2A File Offset: 0x0013CE2A
		public PointJointsKernel() : base("Compute/PointJoints", "CSPointJoints")
		{
		}

		// Token: 0x17000936 RID: 2358
		// (get) Token: 0x0600451C RID: 17692 RVA: 0x0013EA45 File Offset: 0x0013CE45
		// (set) Token: 0x0600451B RID: 17691 RVA: 0x0013EA3C File Offset: 0x0013CE3C
		[GpuData("weight")]
		public GpuValue<float> Weight
		{
			[CompilerGenerated]
			get
			{
				return this.<Weight>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Weight>k__BackingField = value;
			}
		}

		// Token: 0x17000937 RID: 2359
		// (get) Token: 0x0600451E RID: 17694 RVA: 0x0013EA56 File Offset: 0x0013CE56
		// (set) Token: 0x0600451D RID: 17693 RVA: 0x0013EA4D File Offset: 0x0013CE4D
		[GpuData("isFixed")]
		public GpuValue<int> IsFixed
		{
			[CompilerGenerated]
			get
			{
				return this.<IsFixed>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<IsFixed>k__BackingField = value;
			}
		}

		// Token: 0x17000938 RID: 2360
		// (get) Token: 0x06004520 RID: 17696 RVA: 0x0013EA67 File Offset: 0x0013CE67
		// (set) Token: 0x0600451F RID: 17695 RVA: 0x0013EA5E File Offset: 0x0013CE5E
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

		// Token: 0x17000939 RID: 2361
		// (get) Token: 0x06004522 RID: 17698 RVA: 0x0013EA78 File Offset: 0x0013CE78
		// (set) Token: 0x06004521 RID: 17697 RVA: 0x0013EA6F File Offset: 0x0013CE6F
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

		// Token: 0x1700093A RID: 2362
		// (get) Token: 0x06004524 RID: 17700 RVA: 0x0013EA89 File Offset: 0x0013CE89
		// (set) Token: 0x06004523 RID: 17699 RVA: 0x0013EA80 File Offset: 0x0013CE80
		[GpuData("transforms")]
		public GpuBuffer<Matrix4x4> Transforms
		{
			[CompilerGenerated]
			get
			{
				return this.<Transforms>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Transforms>k__BackingField = value;
			}
		}

		// Token: 0x06004525 RID: 17701 RVA: 0x0013EA91 File Offset: 0x0013CE91
		public override int GetGroupsNumX()
		{
			if (this.PointJoints != null)
			{
				return Mathf.CeilToInt((float)this.PointJoints.ComputeBuffer.count / 256f);
			}
			return 0;
		}

		// Token: 0x0400330F RID: 13071
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <Weight>k__BackingField;

		// Token: 0x04003310 RID: 13072
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <IsFixed>k__BackingField;

		// Token: 0x04003311 RID: 13073
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPPointJoint> <PointJoints>k__BackingField;

		// Token: 0x04003312 RID: 13074
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x04003313 RID: 13075
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Matrix4x4> <Transforms>k__BackingField;
	}
}
