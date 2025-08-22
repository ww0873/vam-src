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
	// Token: 0x02000A6F RID: 2671
	public class PointJointsPreCalculatedFinalKernel : KernelBase
	{
		// Token: 0x06004526 RID: 17702 RVA: 0x0013EABC File Offset: 0x0013CEBC
		public PointJointsPreCalculatedFinalKernel() : base("Compute/PointJointsPreCalculatedFinal", "CSPointJointsPreCalculatedFinal")
		{
		}

		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x06004528 RID: 17704 RVA: 0x0013EAD7 File Offset: 0x0013CED7
		// (set) Token: 0x06004527 RID: 17703 RVA: 0x0013EACE File Offset: 0x0013CECE
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

		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x0600452A RID: 17706 RVA: 0x0013EAE8 File Offset: 0x0013CEE8
		// (set) Token: 0x06004529 RID: 17705 RVA: 0x0013EADF File Offset: 0x0013CEDF
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

		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x0600452C RID: 17708 RVA: 0x0013EAF9 File Offset: 0x0013CEF9
		// (set) Token: 0x0600452B RID: 17707 RVA: 0x0013EAF0 File Offset: 0x0013CEF0
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

		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x0600452E RID: 17710 RVA: 0x0013EB0A File Offset: 0x0013CF0A
		// (set) Token: 0x0600452D RID: 17709 RVA: 0x0013EB01 File Offset: 0x0013CF01
		[GpuData("positions")]
		public GpuBuffer<Vector3> Positions
		{
			[CompilerGenerated]
			get
			{
				return this.<Positions>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Positions>k__BackingField = value;
			}
		}

		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x06004530 RID: 17712 RVA: 0x0013EB1B File Offset: 0x0013CF1B
		// (set) Token: 0x0600452F RID: 17711 RVA: 0x0013EB12 File Offset: 0x0013CF12
		[GpuData("jointStrength")]
		public GpuValue<float> JointStrength
		{
			[CompilerGenerated]
			get
			{
				return this.<JointStrength>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<JointStrength>k__BackingField = value;
			}
		}

		// Token: 0x06004531 RID: 17713 RVA: 0x0013EB23 File Offset: 0x0013CF23
		public override int GetGroupsNumX()
		{
			if (this.PointJoints != null)
			{
				return Mathf.CeilToInt((float)this.PointJoints.ComputeBuffer.count / 256f);
			}
			return 0;
		}

		// Token: 0x04003314 RID: 13076
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <IsFixed>k__BackingField;

		// Token: 0x04003315 RID: 13077
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPPointJoint> <PointJoints>k__BackingField;

		// Token: 0x04003316 RID: 13078
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x04003317 RID: 13079
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector3> <Positions>k__BackingField;

		// Token: 0x04003318 RID: 13080
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <JointStrength>k__BackingField;
	}
}
