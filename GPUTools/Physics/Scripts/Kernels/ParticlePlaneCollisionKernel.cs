using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Dynamic;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Kernels
{
	// Token: 0x02000A67 RID: 2663
	public class ParticlePlaneCollisionKernel : KernelBase
	{
		// Token: 0x060044D2 RID: 17618 RVA: 0x0013E6AB File Offset: 0x0013CAAB
		public ParticlePlaneCollisionKernel() : base("Compute/ParticlePlaneCollision", "CSParticlePlaneCollision")
		{
		}

		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x060044D4 RID: 17620 RVA: 0x0013E6C6 File Offset: 0x0013CAC6
		// (set) Token: 0x060044D3 RID: 17619 RVA: 0x0013E6BD File Offset: 0x0013CABD
		[GpuData("step")]
		public GpuValue<float> Step
		{
			[CompilerGenerated]
			get
			{
				return this.<Step>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Step>k__BackingField = value;
			}
		}

		// Token: 0x1700091A RID: 2330
		// (get) Token: 0x060044D6 RID: 17622 RVA: 0x0013E6D7 File Offset: 0x0013CAD7
		// (set) Token: 0x060044D5 RID: 17621 RVA: 0x0013E6CE File Offset: 0x0013CACE
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

		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x060044D8 RID: 17624 RVA: 0x0013E6E8 File Offset: 0x0013CAE8
		// (set) Token: 0x060044D7 RID: 17623 RVA: 0x0013E6DF File Offset: 0x0013CADF
		[GpuData("planes")]
		public GpuBuffer<Vector4> Planes
		{
			[CompilerGenerated]
			get
			{
				return this.<Planes>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Planes>k__BackingField = value;
			}
		}

		// Token: 0x060044D9 RID: 17625 RVA: 0x0013E6F0 File Offset: 0x0013CAF0
		public override int GetGroupsNumX()
		{
			if (this.Particles != null)
			{
				return Mathf.CeilToInt((float)this.Particles.Count / 256f);
			}
			return 0;
		}

		// Token: 0x040032F2 RID: 13042
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <Step>k__BackingField;

		// Token: 0x040032F3 RID: 13043
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x040032F4 RID: 13044
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector4> <Planes>k__BackingField;
	}
}
