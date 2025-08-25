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
	// Token: 0x02000A76 RID: 2678
	public class SplineJointsReverseKernel : KernelBase
	{
		// Token: 0x06004572 RID: 17778 RVA: 0x0013EE84 File Offset: 0x0013D284
		public SplineJointsReverseKernel() : base("Compute/SplineJointsReverse", "CSSplineJointsReverse")
		{
		}

		// Token: 0x1700095A RID: 2394
		// (get) Token: 0x06004574 RID: 17780 RVA: 0x0013EE9F File Offset: 0x0013D29F
		// (set) Token: 0x06004573 RID: 17779 RVA: 0x0013EE96 File Offset: 0x0013D296
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

		// Token: 0x1700095B RID: 2395
		// (get) Token: 0x06004576 RID: 17782 RVA: 0x0013EEB0 File Offset: 0x0013D2B0
		// (set) Token: 0x06004575 RID: 17781 RVA: 0x0013EEA7 File Offset: 0x0013D2A7
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

		// Token: 0x1700095C RID: 2396
		// (get) Token: 0x06004578 RID: 17784 RVA: 0x0013EEC1 File Offset: 0x0013D2C1
		// (set) Token: 0x06004577 RID: 17783 RVA: 0x0013EEB8 File Offset: 0x0013D2B8
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

		// Token: 0x1700095D RID: 2397
		// (get) Token: 0x0600457A RID: 17786 RVA: 0x0013EED2 File Offset: 0x0013D2D2
		// (set) Token: 0x06004579 RID: 17785 RVA: 0x0013EEC9 File Offset: 0x0013D2C9
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

		// Token: 0x1700095E RID: 2398
		// (get) Token: 0x0600457C RID: 17788 RVA: 0x0013EEE3 File Offset: 0x0013D2E3
		// (set) Token: 0x0600457B RID: 17787 RVA: 0x0013EEDA File Offset: 0x0013D2DA
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

		// Token: 0x0600457D RID: 17789 RVA: 0x0013EEEB File Offset: 0x0013D2EB
		public override int GetGroupsNumX()
		{
			if (this.Particles != null)
			{
				return Mathf.CeilToInt((float)(this.Particles.Count / this.Segments.Value) / 256f);
			}
			return 0;
		}

		// Token: 0x04003333 RID: 13107
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <Segments>k__BackingField;

		// Token: 0x04003334 RID: 13108
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x04003335 RID: 13109
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPPointJoint> <PointJoints>k__BackingField;

		// Token: 0x04003336 RID: 13110
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<float> <PointToPreviousPointDistances>k__BackingField;

		// Token: 0x04003337 RID: 13111
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <SplineJointPower>k__BackingField;
	}
}
