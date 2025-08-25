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
	// Token: 0x02000A59 RID: 2649
	public class ParticleLineSphereCollisionHoldKernel : KernelBase
	{
		// Token: 0x0600445A RID: 17498 RVA: 0x0013E069 File Offset: 0x0013C469
		public ParticleLineSphereCollisionHoldKernel() : base("Compute/ParticleLineSphereCollisionHold", "CSParticleLineSphereCollisionHold")
		{
		}

		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x0600445C RID: 17500 RVA: 0x0013E084 File Offset: 0x0013C484
		// (set) Token: 0x0600445B RID: 17499 RVA: 0x0013E07B File Offset: 0x0013C47B
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

		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x0600445E RID: 17502 RVA: 0x0013E095 File Offset: 0x0013C495
		// (set) Token: 0x0600445D RID: 17501 RVA: 0x0013E08C File Offset: 0x0013C48C
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

		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x06004460 RID: 17504 RVA: 0x0013E0A6 File Offset: 0x0013C4A6
		// (set) Token: 0x0600445F RID: 17503 RVA: 0x0013E09D File Offset: 0x0013C49D
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

		// Token: 0x06004461 RID: 17505 RVA: 0x0013E0AE File Offset: 0x0013C4AE
		public override int GetGroupsNumX()
		{
			if (this.Particles != null)
			{
				return Mathf.CeilToInt((float)this.Particles.Count / 256f);
			}
			return 0;
		}

		// Token: 0x040032C4 RID: 12996
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <Step>k__BackingField;

		// Token: 0x040032C5 RID: 12997
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x040032C6 RID: 12998
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphereWithDelta> <ProcessedLineSpheres>k__BackingField;
	}
}
