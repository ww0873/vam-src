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
	// Token: 0x02000A4B RID: 2635
	public class DistanceJointsAdjustKernel : KernelBase
	{
		// Token: 0x060043D4 RID: 17364 RVA: 0x0013D6D7 File Offset: 0x0013BAD7
		public DistanceJointsAdjustKernel(GroupedData<GPDistanceJoint> groupedData, GpuBuffer<GPDistanceJoint> distanceJointsBuffer) : base("Compute/DistanceJointsAdjust", "CSDistanceJointsAdjust")
		{
			this.DistanceJoints = groupedData;
			this.DistanceJointsBuffer = distanceJointsBuffer;
		}

		// Token: 0x170008B6 RID: 2230
		// (get) Token: 0x060043D6 RID: 17366 RVA: 0x0013D700 File Offset: 0x0013BB00
		// (set) Token: 0x060043D5 RID: 17365 RVA: 0x0013D6F7 File Offset: 0x0013BAF7
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

		// Token: 0x170008B7 RID: 2231
		// (get) Token: 0x060043D8 RID: 17368 RVA: 0x0013D711 File Offset: 0x0013BB11
		// (set) Token: 0x060043D7 RID: 17367 RVA: 0x0013D708 File Offset: 0x0013BB08
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

		// Token: 0x170008B8 RID: 2232
		// (get) Token: 0x060043DA RID: 17370 RVA: 0x0013D722 File Offset: 0x0013BB22
		// (set) Token: 0x060043D9 RID: 17369 RVA: 0x0013D719 File Offset: 0x0013BB19
		[GpuData("distanceJoints")]
		public GpuBuffer<GPDistanceJoint> DistanceJointsBuffer
		{
			[CompilerGenerated]
			get
			{
				return this.<DistanceJointsBuffer>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DistanceJointsBuffer>k__BackingField = value;
			}
		}

		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x060043DC RID: 17372 RVA: 0x0013D733 File Offset: 0x0013BB33
		// (set) Token: 0x060043DB RID: 17371 RVA: 0x0013D72A File Offset: 0x0013BB2A
		public GroupedData<GPDistanceJoint> DistanceJoints
		{
			[CompilerGenerated]
			get
			{
				return this.<DistanceJoints>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DistanceJoints>k__BackingField = value;
			}
		}

		// Token: 0x060043DD RID: 17373 RVA: 0x0013D73B File Offset: 0x0013BB3B
		public override int GetGroupsNumX()
		{
			if (this.DistanceJointsBuffer != null)
			{
				return Mathf.CeilToInt((float)this.DistanceJointsBuffer.ComputeBuffer.count / 256f);
			}
			return 0;
		}

		// Token: 0x0400328F RID: 12943
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <Segments>k__BackingField;

		// Token: 0x04003290 RID: 12944
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x04003291 RID: 12945
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPDistanceJoint> <DistanceJointsBuffer>k__BackingField;

		// Token: 0x04003292 RID: 12946
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GroupedData<GPDistanceJoint> <DistanceJoints>k__BackingField;
	}
}
