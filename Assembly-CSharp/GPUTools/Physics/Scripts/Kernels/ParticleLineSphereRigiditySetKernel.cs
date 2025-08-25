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
	// Token: 0x02000A66 RID: 2662
	public class ParticleLineSphereRigiditySetKernel : KernelBase
	{
		// Token: 0x060044CA RID: 17610 RVA: 0x0013E640 File Offset: 0x0013CA40
		public ParticleLineSphereRigiditySetKernel() : base("Compute/ParticleLineSphereRigiditySet", "CSParticleLineSphereRigiditySet")
		{
		}

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x060044CC RID: 17612 RVA: 0x0013E65B File Offset: 0x0013CA5B
		// (set) Token: 0x060044CB RID: 17611 RVA: 0x0013E652 File Offset: 0x0013CA52
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

		// Token: 0x17000917 RID: 2327
		// (get) Token: 0x060044CE RID: 17614 RVA: 0x0013E66C File Offset: 0x0013CA6C
		// (set) Token: 0x060044CD RID: 17613 RVA: 0x0013E663 File Offset: 0x0013CA63
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

		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x060044D0 RID: 17616 RVA: 0x0013E67D File Offset: 0x0013CA7D
		// (set) Token: 0x060044CF RID: 17615 RVA: 0x0013E674 File Offset: 0x0013CA74
		[GpuData("rigiditySetLineSpheres")]
		public GpuBuffer<GPLineSphere> RigiditySetLineSpheres
		{
			[CompilerGenerated]
			get
			{
				return this.<RigiditySetLineSpheres>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<RigiditySetLineSpheres>k__BackingField = value;
			}
		}

		// Token: 0x060044D1 RID: 17617 RVA: 0x0013E685 File Offset: 0x0013CA85
		public override int GetGroupsNumX()
		{
			if (this.Particles != null)
			{
				return Mathf.CeilToInt((float)this.Particles.Count / 256f);
			}
			return 0;
		}

		// Token: 0x040032EF RID: 13039
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x040032F0 RID: 13040
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPPointJoint> <PointJoints>k__BackingField;

		// Token: 0x040032F1 RID: 13041
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphere> <RigiditySetLineSpheres>k__BackingField;
	}
}
