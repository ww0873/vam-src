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
	// Token: 0x02000A6C RID: 2668
	public class PointJointsFinalKernel : KernelBase
	{
		// Token: 0x06004500 RID: 17664 RVA: 0x0013E8F5 File Offset: 0x0013CCF5
		public PointJointsFinalKernel() : base("Compute/PointJointsFinal", "CSPointJointsFinal")
		{
		}

		// Token: 0x1700092B RID: 2347
		// (get) Token: 0x06004502 RID: 17666 RVA: 0x0013E910 File Offset: 0x0013CD10
		// (set) Token: 0x06004501 RID: 17665 RVA: 0x0013E907 File Offset: 0x0013CD07
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

		// Token: 0x1700092C RID: 2348
		// (get) Token: 0x06004504 RID: 17668 RVA: 0x0013E921 File Offset: 0x0013CD21
		// (set) Token: 0x06004503 RID: 17667 RVA: 0x0013E918 File Offset: 0x0013CD18
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

		// Token: 0x1700092D RID: 2349
		// (get) Token: 0x06004506 RID: 17670 RVA: 0x0013E932 File Offset: 0x0013CD32
		// (set) Token: 0x06004505 RID: 17669 RVA: 0x0013E929 File Offset: 0x0013CD29
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

		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x06004508 RID: 17672 RVA: 0x0013E943 File Offset: 0x0013CD43
		// (set) Token: 0x06004507 RID: 17671 RVA: 0x0013E93A File Offset: 0x0013CD3A
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

		// Token: 0x1700092F RID: 2351
		// (get) Token: 0x0600450A RID: 17674 RVA: 0x0013E954 File Offset: 0x0013CD54
		// (set) Token: 0x06004509 RID: 17673 RVA: 0x0013E94B File Offset: 0x0013CD4B
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

		// Token: 0x0600450B RID: 17675 RVA: 0x0013E95C File Offset: 0x0013CD5C
		public override int GetGroupsNumX()
		{
			if (this.PointJoints != null)
			{
				return Mathf.CeilToInt((float)this.PointJoints.ComputeBuffer.count / 256f);
			}
			return 0;
		}

		// Token: 0x04003304 RID: 13060
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <Weight>k__BackingField;

		// Token: 0x04003305 RID: 13061
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <IsFixed>k__BackingField;

		// Token: 0x04003306 RID: 13062
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPPointJoint> <PointJoints>k__BackingField;

		// Token: 0x04003307 RID: 13063
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x04003308 RID: 13064
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Matrix4x4> <Transforms>k__BackingField;
	}
}
