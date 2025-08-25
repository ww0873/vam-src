using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Shapes;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Kernels
{
	// Token: 0x02000A61 RID: 2657
	public class ParticleLineSphereProcessKernel : KernelBase
	{
		// Token: 0x060044A0 RID: 17568 RVA: 0x0013E418 File Offset: 0x0013C818
		public ParticleLineSphereProcessKernel() : base("Compute/ParticleLineSphereProcess", "CSParticleLineSphereProcess")
		{
		}

		// Token: 0x17000906 RID: 2310
		// (get) Token: 0x060044A2 RID: 17570 RVA: 0x0013E433 File Offset: 0x0013C833
		// (set) Token: 0x060044A1 RID: 17569 RVA: 0x0013E42A File Offset: 0x0013C82A
		[GpuData("collisionPrediction")]
		public GpuValue<float> CollisionPrediction
		{
			[CompilerGenerated]
			get
			{
				return this.<CollisionPrediction>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CollisionPrediction>k__BackingField = value;
			}
		}

		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x060044A4 RID: 17572 RVA: 0x0013E444 File Offset: 0x0013C844
		// (set) Token: 0x060044A3 RID: 17571 RVA: 0x0013E43B File Offset: 0x0013C83B
		[GpuData("lineSpheres")]
		public GpuBuffer<GPLineSphere> LineSpheres
		{
			[CompilerGenerated]
			get
			{
				return this.<LineSpheres>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<LineSpheres>k__BackingField = value;
			}
		}

		// Token: 0x17000908 RID: 2312
		// (get) Token: 0x060044A6 RID: 17574 RVA: 0x0013E455 File Offset: 0x0013C855
		// (set) Token: 0x060044A5 RID: 17573 RVA: 0x0013E44C File Offset: 0x0013C84C
		[GpuData("oldLineSpheres")]
		public GpuBuffer<GPLineSphere> OldLineSpheres
		{
			[CompilerGenerated]
			get
			{
				return this.<OldLineSpheres>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<OldLineSpheres>k__BackingField = value;
			}
		}

		// Token: 0x17000909 RID: 2313
		// (get) Token: 0x060044A8 RID: 17576 RVA: 0x0013E466 File Offset: 0x0013C866
		// (set) Token: 0x060044A7 RID: 17575 RVA: 0x0013E45D File Offset: 0x0013C85D
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

		// Token: 0x060044A9 RID: 17577 RVA: 0x0013E46E File Offset: 0x0013C86E
		public override int GetGroupsNumX()
		{
			if (this.LineSpheres != null)
			{
				return Mathf.CeilToInt((float)this.LineSpheres.Count / 256f);
			}
			return 0;
		}

		// Token: 0x040032DF RID: 13023
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <CollisionPrediction>k__BackingField;

		// Token: 0x040032E0 RID: 13024
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphere> <LineSpheres>k__BackingField;

		// Token: 0x040032E1 RID: 13025
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphere> <OldLineSpheres>k__BackingField;

		// Token: 0x040032E2 RID: 13026
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphereWithDelta> <ProcessedLineSpheres>k__BackingField;
	}
}
