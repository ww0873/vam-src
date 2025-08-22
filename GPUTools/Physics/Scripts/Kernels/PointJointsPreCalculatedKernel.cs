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
	// Token: 0x02000A70 RID: 2672
	public class PointJointsPreCalculatedKernel : KernelBase
	{
		// Token: 0x06004532 RID: 17714 RVA: 0x0013EB4E File Offset: 0x0013CF4E
		public PointJointsPreCalculatedKernel() : base("Compute/PointJointsPreCalculated", "CSPointJointsPreCalculated")
		{
		}

		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x06004534 RID: 17716 RVA: 0x0013EB69 File Offset: 0x0013CF69
		// (set) Token: 0x06004533 RID: 17715 RVA: 0x0013EB60 File Offset: 0x0013CF60
		[GpuData("t")]
		public GpuValue<float> T
		{
			[CompilerGenerated]
			get
			{
				return this.<T>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<T>k__BackingField = value;
			}
		}

		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x06004536 RID: 17718 RVA: 0x0013EB7A File Offset: 0x0013CF7A
		// (set) Token: 0x06004535 RID: 17717 RVA: 0x0013EB71 File Offset: 0x0013CF71
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

		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x06004538 RID: 17720 RVA: 0x0013EB8B File Offset: 0x0013CF8B
		// (set) Token: 0x06004537 RID: 17719 RVA: 0x0013EB82 File Offset: 0x0013CF82
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

		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x0600453A RID: 17722 RVA: 0x0013EB9C File Offset: 0x0013CF9C
		// (set) Token: 0x06004539 RID: 17721 RVA: 0x0013EB93 File Offset: 0x0013CF93
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

		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x0600453C RID: 17724 RVA: 0x0013EBAD File Offset: 0x0013CFAD
		// (set) Token: 0x0600453B RID: 17723 RVA: 0x0013EBA4 File Offset: 0x0013CFA4
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

		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x0600453E RID: 17726 RVA: 0x0013EBBE File Offset: 0x0013CFBE
		// (set) Token: 0x0600453D RID: 17725 RVA: 0x0013EBB5 File Offset: 0x0013CFB5
		[GpuData("oldPositions")]
		public GpuBuffer<Vector3> OldPositions
		{
			[CompilerGenerated]
			get
			{
				return this.<OldPositions>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<OldPositions>k__BackingField = value;
			}
		}

		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x06004540 RID: 17728 RVA: 0x0013EBCF File Offset: 0x0013CFCF
		// (set) Token: 0x0600453F RID: 17727 RVA: 0x0013EBC6 File Offset: 0x0013CFC6
		[GpuData("breakThreshold")]
		public GpuValue<float> BreakThreshold
		{
			[CompilerGenerated]
			get
			{
				return this.<BreakThreshold>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<BreakThreshold>k__BackingField = value;
			}
		}

		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x06004542 RID: 17730 RVA: 0x0013EBE0 File Offset: 0x0013CFE0
		// (set) Token: 0x06004541 RID: 17729 RVA: 0x0013EBD7 File Offset: 0x0013CFD7
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

		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x06004544 RID: 17732 RVA: 0x0013EBF1 File Offset: 0x0013CFF1
		// (set) Token: 0x06004543 RID: 17731 RVA: 0x0013EBE8 File Offset: 0x0013CFE8
		[GpuData("jointPrediction")]
		public GpuValue<float> JointPrediction
		{
			[CompilerGenerated]
			get
			{
				return this.<JointPrediction>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<JointPrediction>k__BackingField = value;
			}
		}

		// Token: 0x06004545 RID: 17733 RVA: 0x0013EBF9 File Offset: 0x0013CFF9
		public override int GetGroupsNumX()
		{
			if (this.PointJoints != null)
			{
				return Mathf.CeilToInt((float)this.PointJoints.ComputeBuffer.count / 256f);
			}
			return 0;
		}

		// Token: 0x04003319 RID: 13081
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <T>k__BackingField;

		// Token: 0x0400331A RID: 13082
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <IsFixed>k__BackingField;

		// Token: 0x0400331B RID: 13083
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPPointJoint> <PointJoints>k__BackingField;

		// Token: 0x0400331C RID: 13084
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x0400331D RID: 13085
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector3> <Positions>k__BackingField;

		// Token: 0x0400331E RID: 13086
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector3> <OldPositions>k__BackingField;

		// Token: 0x0400331F RID: 13087
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <BreakThreshold>k__BackingField;

		// Token: 0x04003320 RID: 13088
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <JointStrength>k__BackingField;

		// Token: 0x04003321 RID: 13089
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <JointPrediction>k__BackingField;
	}
}
