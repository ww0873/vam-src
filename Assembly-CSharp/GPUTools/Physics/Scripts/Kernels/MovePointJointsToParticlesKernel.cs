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
	// Token: 0x02000A55 RID: 2645
	public class MovePointJointsToParticlesKernel : KernelBase
	{
		// Token: 0x06004436 RID: 17462 RVA: 0x0013DDDA File Offset: 0x0013C1DA
		public MovePointJointsToParticlesKernel() : base("Compute/MovePointJointsToParticles", "CSMovePointJointsToParticles")
		{
		}

		// Token: 0x170008DD RID: 2269
		// (get) Token: 0x06004438 RID: 17464 RVA: 0x0013DDF5 File Offset: 0x0013C1F5
		// (set) Token: 0x06004437 RID: 17463 RVA: 0x0013DDEC File Offset: 0x0013C1EC
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

		// Token: 0x170008DE RID: 2270
		// (get) Token: 0x0600443A RID: 17466 RVA: 0x0013DE06 File Offset: 0x0013C206
		// (set) Token: 0x06004439 RID: 17465 RVA: 0x0013DDFD File Offset: 0x0013C1FD
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

		// Token: 0x170008DF RID: 2271
		// (get) Token: 0x0600443C RID: 17468 RVA: 0x0013DE17 File Offset: 0x0013C217
		// (set) Token: 0x0600443B RID: 17467 RVA: 0x0013DE0E File Offset: 0x0013C20E
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

		// Token: 0x170008E0 RID: 2272
		// (get) Token: 0x0600443E RID: 17470 RVA: 0x0013DE28 File Offset: 0x0013C228
		// (set) Token: 0x0600443D RID: 17469 RVA: 0x0013DE1F File Offset: 0x0013C21F
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

		// Token: 0x0600443F RID: 17471 RVA: 0x0013DE30 File Offset: 0x0013C230
		public override int GetGroupsNumX()
		{
			if (this.PointJoints != null)
			{
				return Mathf.CeilToInt((float)this.PointJoints.ComputeBuffer.count / 256f);
			}
			return 0;
		}

		// Token: 0x040032B6 RID: 12982
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <IsFixed>k__BackingField;

		// Token: 0x040032B7 RID: 12983
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPPointJoint> <PointJoints>k__BackingField;

		// Token: 0x040032B8 RID: 12984
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x040032B9 RID: 12985
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Matrix4x4> <Transforms>k__BackingField;
	}
}
