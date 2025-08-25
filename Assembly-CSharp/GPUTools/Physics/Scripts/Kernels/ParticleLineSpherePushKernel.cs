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
	// Token: 0x02000A63 RID: 2659
	public class ParticleLineSpherePushKernel : KernelBase
	{
		// Token: 0x060044B2 RID: 17586 RVA: 0x0013E4FF File Offset: 0x0013C8FF
		public ParticleLineSpherePushKernel() : base("Compute/ParticleLineSpherePush", "CSParticleLineSpherePush")
		{
		}

		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x060044B4 RID: 17588 RVA: 0x0013E51A File Offset: 0x0013C91A
		// (set) Token: 0x060044B3 RID: 17587 RVA: 0x0013E511 File Offset: 0x0013C911
		[GpuData("segments")]
		public GpuValue<int> Segments
		{
			[CompilerGenerated]
			get
			{
				return this.<Segments>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Segments>k__BackingField = value;
			}
		}

		// Token: 0x1700090E RID: 2318
		// (get) Token: 0x060044B6 RID: 17590 RVA: 0x0013E52B File Offset: 0x0013C92B
		// (set) Token: 0x060044B5 RID: 17589 RVA: 0x0013E522 File Offset: 0x0013C922
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

		// Token: 0x1700090F RID: 2319
		// (get) Token: 0x060044B8 RID: 17592 RVA: 0x0013E53C File Offset: 0x0013C93C
		// (set) Token: 0x060044B7 RID: 17591 RVA: 0x0013E533 File Offset: 0x0013C933
		[GpuData("pushLineSpheres")]
		public GpuBuffer<GPLineSphere> PushLineSpheres
		{
			[CompilerGenerated]
			get
			{
				return this.<PushLineSpheres>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<PushLineSpheres>k__BackingField = value;
			}
		}

		// Token: 0x060044B9 RID: 17593 RVA: 0x0013E544 File Offset: 0x0013C944
		public override int GetGroupsNumX()
		{
			if (this.Particles != null)
			{
				return Mathf.CeilToInt((float)this.Particles.Count / 256f);
			}
			return 0;
		}

		// Token: 0x040032E6 RID: 13030
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <Segments>k__BackingField;

		// Token: 0x040032E7 RID: 13031
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x040032E8 RID: 13032
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphere> <PushLineSpheres>k__BackingField;
	}
}
