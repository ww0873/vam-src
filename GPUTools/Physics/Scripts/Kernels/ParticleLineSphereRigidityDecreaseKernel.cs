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
	// Token: 0x02000A64 RID: 2660
	public class ParticleLineSphereRigidityDecreaseKernel : KernelBase
	{
		// Token: 0x060044BA RID: 17594 RVA: 0x0013E56A File Offset: 0x0013C96A
		public ParticleLineSphereRigidityDecreaseKernel() : base("Compute/ParticleLineSphereRigidityDecrease", "CSParticleLineSphereRigidityDecrease")
		{
		}

		// Token: 0x17000910 RID: 2320
		// (get) Token: 0x060044BC RID: 17596 RVA: 0x0013E585 File Offset: 0x0013C985
		// (set) Token: 0x060044BB RID: 17595 RVA: 0x0013E57C File Offset: 0x0013C97C
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

		// Token: 0x17000911 RID: 2321
		// (get) Token: 0x060044BE RID: 17598 RVA: 0x0013E596 File Offset: 0x0013C996
		// (set) Token: 0x060044BD RID: 17597 RVA: 0x0013E58D File Offset: 0x0013C98D
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

		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x060044C0 RID: 17600 RVA: 0x0013E5A7 File Offset: 0x0013C9A7
		// (set) Token: 0x060044BF RID: 17599 RVA: 0x0013E59E File Offset: 0x0013C99E
		[GpuData("rigidityDecreaseLineSpheres")]
		public GpuBuffer<GPLineSphere> RigidityDecreaseLineSpheres
		{
			[CompilerGenerated]
			get
			{
				return this.<RigidityDecreaseLineSpheres>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<RigidityDecreaseLineSpheres>k__BackingField = value;
			}
		}

		// Token: 0x060044C1 RID: 17601 RVA: 0x0013E5AF File Offset: 0x0013C9AF
		public override int GetGroupsNumX()
		{
			if (this.Particles != null)
			{
				return Mathf.CeilToInt((float)this.Particles.Count / 256f);
			}
			return 0;
		}

		// Token: 0x040032E9 RID: 13033
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x040032EA RID: 13034
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPPointJoint> <PointJoints>k__BackingField;

		// Token: 0x040032EB RID: 13035
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphere> <RigidityDecreaseLineSpheres>k__BackingField;
	}
}
