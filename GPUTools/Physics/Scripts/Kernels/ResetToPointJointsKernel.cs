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
	// Token: 0x02000A72 RID: 2674
	public class ResetToPointJointsKernel : KernelBase
	{
		// Token: 0x0600454A RID: 17738 RVA: 0x0013EC72 File Offset: 0x0013D072
		public ResetToPointJointsKernel() : base("Compute/ResetToPointJoints", "CSResetToPointJoints")
		{
		}

		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x0600454C RID: 17740 RVA: 0x0013EC8D File Offset: 0x0013D08D
		// (set) Token: 0x0600454B RID: 17739 RVA: 0x0013EC84 File Offset: 0x0013D084
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

		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x0600454E RID: 17742 RVA: 0x0013EC9E File Offset: 0x0013D09E
		// (set) Token: 0x0600454D RID: 17741 RVA: 0x0013EC95 File Offset: 0x0013D095
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

		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x06004550 RID: 17744 RVA: 0x0013ECAF File Offset: 0x0013D0AF
		// (set) Token: 0x0600454F RID: 17743 RVA: 0x0013ECA6 File Offset: 0x0013D0A6
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

		// Token: 0x06004551 RID: 17745 RVA: 0x0013ECB7 File Offset: 0x0013D0B7
		public override int GetGroupsNumX()
		{
			if (this.PointJoints != null)
			{
				return Mathf.CeilToInt((float)this.PointJoints.ComputeBuffer.count / 256f);
			}
			return 0;
		}

		// Token: 0x04003323 RID: 13091
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x04003324 RID: 13092
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPPointJoint> <PointJoints>k__BackingField;

		// Token: 0x04003325 RID: 13093
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Matrix4x4> <Transforms>k__BackingField;
	}
}
