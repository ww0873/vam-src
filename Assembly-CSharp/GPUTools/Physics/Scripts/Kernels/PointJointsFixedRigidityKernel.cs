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
	// Token: 0x02000A6D RID: 2669
	public class PointJointsFixedRigidityKernel : KernelBase
	{
		// Token: 0x0600450C RID: 17676 RVA: 0x0013E987 File Offset: 0x0013CD87
		public PointJointsFixedRigidityKernel() : base("Compute/PointJointsFixedRigidity", "CSPointJointsFixedRigidity")
		{
		}

		// Token: 0x17000930 RID: 2352
		// (get) Token: 0x0600450E RID: 17678 RVA: 0x0013E9A2 File Offset: 0x0013CDA2
		// (set) Token: 0x0600450D RID: 17677 RVA: 0x0013E999 File Offset: 0x0013CD99
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

		// Token: 0x17000931 RID: 2353
		// (get) Token: 0x06004510 RID: 17680 RVA: 0x0013E9B3 File Offset: 0x0013CDB3
		// (set) Token: 0x0600450F RID: 17679 RVA: 0x0013E9AA File Offset: 0x0013CDAA
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

		// Token: 0x17000932 RID: 2354
		// (get) Token: 0x06004512 RID: 17682 RVA: 0x0013E9C4 File Offset: 0x0013CDC4
		// (set) Token: 0x06004511 RID: 17681 RVA: 0x0013E9BB File Offset: 0x0013CDBB
		[GpuData("fixedRigidity")]
		public GpuValue<float> FixedRigidity
		{
			[CompilerGenerated]
			get
			{
				return this.<FixedRigidity>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<FixedRigidity>k__BackingField = value;
			}
		}

		// Token: 0x17000933 RID: 2355
		// (get) Token: 0x06004514 RID: 17684 RVA: 0x0013E9D5 File Offset: 0x0013CDD5
		// (set) Token: 0x06004513 RID: 17683 RVA: 0x0013E9CC File Offset: 0x0013CDCC
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

		// Token: 0x17000934 RID: 2356
		// (get) Token: 0x06004516 RID: 17686 RVA: 0x0013E9E6 File Offset: 0x0013CDE6
		// (set) Token: 0x06004515 RID: 17685 RVA: 0x0013E9DD File Offset: 0x0013CDDD
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

		// Token: 0x17000935 RID: 2357
		// (get) Token: 0x06004518 RID: 17688 RVA: 0x0013E9F7 File Offset: 0x0013CDF7
		// (set) Token: 0x06004517 RID: 17687 RVA: 0x0013E9EE File Offset: 0x0013CDEE
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

		// Token: 0x06004519 RID: 17689 RVA: 0x0013E9FF File Offset: 0x0013CDFF
		public override int GetGroupsNumX()
		{
			if (this.PointJoints != null)
			{
				return Mathf.CeilToInt((float)this.PointJoints.ComputeBuffer.count / 256f);
			}
			return 0;
		}

		// Token: 0x04003309 RID: 13065
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <Weight>k__BackingField;

		// Token: 0x0400330A RID: 13066
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <IsFixed>k__BackingField;

		// Token: 0x0400330B RID: 13067
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <FixedRigidity>k__BackingField;

		// Token: 0x0400330C RID: 13068
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPPointJoint> <PointJoints>k__BackingField;

		// Token: 0x0400330D RID: 13069
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x0400330E RID: 13070
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Matrix4x4> <Transforms>k__BackingField;
	}
}
