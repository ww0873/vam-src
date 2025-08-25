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
	// Token: 0x02000A5A RID: 2650
	public class ParticleLineSphereCollisionKernel : KernelBase
	{
		// Token: 0x06004462 RID: 17506 RVA: 0x0013E0D4 File Offset: 0x0013C4D4
		public ParticleLineSphereCollisionKernel() : base("Compute/ParticleLineSphereCollision", "CSParticleLineSphereCollision")
		{
		}

		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x06004464 RID: 17508 RVA: 0x0013E0EF File Offset: 0x0013C4EF
		// (set) Token: 0x06004463 RID: 17507 RVA: 0x0013E0E6 File Offset: 0x0013C4E6
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

		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x06004466 RID: 17510 RVA: 0x0013E100 File Offset: 0x0013C500
		// (set) Token: 0x06004465 RID: 17509 RVA: 0x0013E0F7 File Offset: 0x0013C4F7
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

		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x06004468 RID: 17512 RVA: 0x0013E111 File Offset: 0x0013C511
		// (set) Token: 0x06004467 RID: 17511 RVA: 0x0013E108 File Offset: 0x0013C508
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

		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x0600446A RID: 17514 RVA: 0x0013E122 File Offset: 0x0013C522
		// (set) Token: 0x06004469 RID: 17513 RVA: 0x0013E119 File Offset: 0x0013C519
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

		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x0600446C RID: 17516 RVA: 0x0013E133 File Offset: 0x0013C533
		// (set) Token: 0x0600446B RID: 17515 RVA: 0x0013E12A File Offset: 0x0013C52A
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

		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x0600446E RID: 17518 RVA: 0x0013E144 File Offset: 0x0013C544
		// (set) Token: 0x0600446D RID: 17517 RVA: 0x0013E13B File Offset: 0x0013C53B
		[GpuData("processedLineSpheres")]
		public GpuBuffer<GPLineSphereWithDelta> ProcessedLineSpheres
		{
			[CompilerGenerated]
			get
			{
				return this.<ProcessedLineSpheres>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ProcessedLineSpheres>k__BackingField = value;
			}
		}

		// Token: 0x0600446F RID: 17519 RVA: 0x0013E14C File Offset: 0x0013C54C
		public override int GetGroupsNumX()
		{
			if (this.Particles != null)
			{
				return Mathf.CeilToInt((float)this.Particles.Count / 256f);
			}
			return 0;
		}

		// Token: 0x040032C7 RID: 12999
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <T>k__BackingField;

		// Token: 0x040032C8 RID: 13000
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <Friction>k__BackingField;

		// Token: 0x040032C9 RID: 13001
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <StaticFriction>k__BackingField;

		// Token: 0x040032CA RID: 13002
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <CollisionPower>k__BackingField;

		// Token: 0x040032CB RID: 13003
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x040032CC RID: 13004
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphereWithDelta> <ProcessedLineSpheres>k__BackingField;
	}
}
