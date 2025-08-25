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
	// Token: 0x02000A74 RID: 2676
	public class SplineJointsAdjustKernel : KernelBase
	{
		// Token: 0x0600455A RID: 17754 RVA: 0x0013ED52 File Offset: 0x0013D152
		public SplineJointsAdjustKernel() : base("Compute/SplineJointsAdjust", "CSSplineJointsAdjust")
		{
		}

		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x0600455C RID: 17756 RVA: 0x0013ED6D File Offset: 0x0013D16D
		// (set) Token: 0x0600455B RID: 17755 RVA: 0x0013ED64 File Offset: 0x0013D164
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

		// Token: 0x17000951 RID: 2385
		// (get) Token: 0x0600455E RID: 17758 RVA: 0x0013ED7E File Offset: 0x0013D17E
		// (set) Token: 0x0600455D RID: 17757 RVA: 0x0013ED75 File Offset: 0x0013D175
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

		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x06004560 RID: 17760 RVA: 0x0013ED8F File Offset: 0x0013D18F
		// (set) Token: 0x0600455F RID: 17759 RVA: 0x0013ED86 File Offset: 0x0013D186
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

		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x06004562 RID: 17762 RVA: 0x0013EDA0 File Offset: 0x0013D1A0
		// (set) Token: 0x06004561 RID: 17761 RVA: 0x0013ED97 File Offset: 0x0013D197
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

		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x06004564 RID: 17764 RVA: 0x0013EDB1 File Offset: 0x0013D1B1
		// (set) Token: 0x06004563 RID: 17763 RVA: 0x0013EDA8 File Offset: 0x0013D1A8
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

		// Token: 0x06004565 RID: 17765 RVA: 0x0013EDB9 File Offset: 0x0013D1B9
		public override int GetGroupsNumX()
		{
			if (this.Particles != null)
			{
				return Mathf.CeilToInt((float)(this.Particles.Count / this.Segments.Value) / 256f);
			}
			return 0;
		}

		// Token: 0x04003329 RID: 13097
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <Segments>k__BackingField;

		// Token: 0x0400332A RID: 13098
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x0400332B RID: 13099
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPPointJoint> <PointJoints>k__BackingField;

		// Token: 0x0400332C RID: 13100
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<float> <PointToPreviousPointDistances>k__BackingField;

		// Token: 0x0400332D RID: 13101
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <SplineJointPower>k__BackingField;
	}
}
