using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Joints;
using GPUTools.Physics.Scripts.Types.Shapes;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Kernels
{
	// Token: 0x02000A65 RID: 2661
	public class ParticleLineSphereRigidityIncreaseKernel : KernelBase
	{
		// Token: 0x060044C2 RID: 17602 RVA: 0x0013E5D5 File Offset: 0x0013C9D5
		public ParticleLineSphereRigidityIncreaseKernel() : base("Compute/ParticleLineSphereRigidityIncrease", "CSParticleLineSphereRigidityIncrease")
		{
		}

		// Token: 0x17000913 RID: 2323
		// (get) Token: 0x060044C4 RID: 17604 RVA: 0x0013E5F0 File Offset: 0x0013C9F0
		// (set) Token: 0x060044C3 RID: 17603 RVA: 0x0013E5E7 File Offset: 0x0013C9E7
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

		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x060044C6 RID: 17606 RVA: 0x0013E601 File Offset: 0x0013CA01
		// (set) Token: 0x060044C5 RID: 17605 RVA: 0x0013E5F8 File Offset: 0x0013C9F8
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

		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x060044C8 RID: 17608 RVA: 0x0013E612 File Offset: 0x0013CA12
		// (set) Token: 0x060044C7 RID: 17607 RVA: 0x0013E609 File Offset: 0x0013CA09
		[GpuData("rigidityIncreaseLineSpheres")]
		public GpuBuffer<GPLineSphere> RigidityIncreaseLineSpheres
		{
			[CompilerGenerated]
			get
			{
				return this.<RigidityIncreaseLineSpheres>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<RigidityIncreaseLineSpheres>k__BackingField = value;
			}
		}

		// Token: 0x060044C9 RID: 17609 RVA: 0x0013E61A File Offset: 0x0013CA1A
		public override int GetGroupsNumX()
		{
			if (this.Particles != null)
			{
				return Mathf.CeilToInt((float)this.Particles.Count / 256f);
			}
			return 0;
		}

		// Token: 0x040032EC RID: 13036
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x040032ED RID: 13037
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPPointJoint> <PointJoints>k__BackingField;

		// Token: 0x040032EE RID: 13038
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphere> <RigidityIncreaseLineSpheres>k__BackingField;
	}
}
