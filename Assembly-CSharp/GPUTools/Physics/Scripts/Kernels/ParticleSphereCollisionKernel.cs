using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Shapes;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Kernels
{
	// Token: 0x02000A69 RID: 2665
	public class ParticleSphereCollisionKernel : KernelBase
	{
		// Token: 0x060044E2 RID: 17634 RVA: 0x0013E781 File Offset: 0x0013CB81
		public ParticleSphereCollisionKernel() : base("Compute/ParticleSphereCollision", "CSParticleSphereCollision")
		{
		}

		// Token: 0x1700091F RID: 2335
		// (get) Token: 0x060044E4 RID: 17636 RVA: 0x0013E79C File Offset: 0x0013CB9C
		// (set) Token: 0x060044E3 RID: 17635 RVA: 0x0013E793 File Offset: 0x0013CB93
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

		// Token: 0x17000920 RID: 2336
		// (get) Token: 0x060044E6 RID: 17638 RVA: 0x0013E7AD File Offset: 0x0013CBAD
		// (set) Token: 0x060044E5 RID: 17637 RVA: 0x0013E7A4 File Offset: 0x0013CBA4
		[GpuData("friction")]
		public GpuValue<float> Friction
		{
			[CompilerGenerated]
			get
			{
				return this.<Friction>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Friction>k__BackingField = value;
			}
		}

		// Token: 0x17000921 RID: 2337
		// (get) Token: 0x060044E8 RID: 17640 RVA: 0x0013E7BE File Offset: 0x0013CBBE
		// (set) Token: 0x060044E7 RID: 17639 RVA: 0x0013E7B5 File Offset: 0x0013CBB5
		[GpuData("staticFriction")]
		public GpuValue<float> StaticFriction
		{
			[CompilerGenerated]
			get
			{
				return this.<StaticFriction>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<StaticFriction>k__BackingField = value;
			}
		}

		// Token: 0x17000922 RID: 2338
		// (get) Token: 0x060044EA RID: 17642 RVA: 0x0013E7CF File Offset: 0x0013CBCF
		// (set) Token: 0x060044E9 RID: 17641 RVA: 0x0013E7C6 File Offset: 0x0013CBC6
		[GpuData("collisionPower")]
		public GpuValue<float> CollisionPower
		{
			[CompilerGenerated]
			get
			{
				return this.<CollisionPower>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CollisionPower>k__BackingField = value;
			}
		}

		// Token: 0x17000923 RID: 2339
		// (get) Token: 0x060044EC RID: 17644 RVA: 0x0013E7E0 File Offset: 0x0013CBE0
		// (set) Token: 0x060044EB RID: 17643 RVA: 0x0013E7D7 File Offset: 0x0013CBD7
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

		// Token: 0x17000924 RID: 2340
		// (get) Token: 0x060044EE RID: 17646 RVA: 0x0013E7F1 File Offset: 0x0013CBF1
		// (set) Token: 0x060044ED RID: 17645 RVA: 0x0013E7E8 File Offset: 0x0013CBE8
		[GpuData("processedSpheres")]
		public GpuBuffer<GPSphereWithDelta> ProcessedSpheres
		{
			[CompilerGenerated]
			get
			{
				return this.<ProcessedSpheres>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ProcessedSpheres>k__BackingField = value;
			}
		}

		// Token: 0x060044EF RID: 17647 RVA: 0x0013E7F9 File Offset: 0x0013CBF9
		public override int GetGroupsNumX()
		{
			if (this.Particles != null)
			{
				return Mathf.CeilToInt((float)this.Particles.Count / 256f);
			}
			return 0;
		}

		// Token: 0x040032F8 RID: 13048
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <T>k__BackingField;

		// Token: 0x040032F9 RID: 13049
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <Friction>k__BackingField;

		// Token: 0x040032FA RID: 13050
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <StaticFriction>k__BackingField;

		// Token: 0x040032FB RID: 13051
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <CollisionPower>k__BackingField;

		// Token: 0x040032FC RID: 13052
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x040032FD RID: 13053
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPSphereWithDelta> <ProcessedSpheres>k__BackingField;
	}
}
