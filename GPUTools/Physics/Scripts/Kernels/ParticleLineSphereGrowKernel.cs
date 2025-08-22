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
	// Token: 0x02000A5E RID: 2654
	public class ParticleLineSphereGrowKernel : KernelBase
	{
		// Token: 0x0600448A RID: 17546 RVA: 0x0013E2DC File Offset: 0x0013C6DC
		public ParticleLineSphereGrowKernel() : base("Compute/ParticleLineSphereGrow", "CSParticleLineSphereGrow")
		{
		}

		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x0600448C RID: 17548 RVA: 0x0013E2F7 File Offset: 0x0013C6F7
		// (set) Token: 0x0600448B RID: 17547 RVA: 0x0013E2EE File Offset: 0x0013C6EE
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

		// Token: 0x170008FF RID: 2303
		// (get) Token: 0x0600448E RID: 17550 RVA: 0x0013E308 File Offset: 0x0013C708
		// (set) Token: 0x0600448D RID: 17549 RVA: 0x0013E2FF File Offset: 0x0013C6FF
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

		// Token: 0x17000900 RID: 2304
		// (get) Token: 0x06004490 RID: 17552 RVA: 0x0013E319 File Offset: 0x0013C719
		// (set) Token: 0x0600448F RID: 17551 RVA: 0x0013E310 File Offset: 0x0013C710
		[GpuData("pointToPreviousPointDistances")]
		public GpuBuffer<float> PointToPreviousPointDistances
		{
			[CompilerGenerated]
			get
			{
				return this.<PointToPreviousPointDistances>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<PointToPreviousPointDistances>k__BackingField = value;
			}
		}

		// Token: 0x17000901 RID: 2305
		// (get) Token: 0x06004492 RID: 17554 RVA: 0x0013E32A File Offset: 0x0013C72A
		// (set) Token: 0x06004491 RID: 17553 RVA: 0x0013E321 File Offset: 0x0013C721
		[GpuData("growLineSpheres")]
		public GpuBuffer<GPLineSphere> GrowLineSpheres
		{
			[CompilerGenerated]
			get
			{
				return this.<GrowLineSpheres>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<GrowLineSpheres>k__BackingField = value;
			}
		}

		// Token: 0x06004493 RID: 17555 RVA: 0x0013E332 File Offset: 0x0013C732
		public override int GetGroupsNumX()
		{
			if (this.Particles != null)
			{
				return Mathf.CeilToInt((float)(this.Particles.Count / this.Segments.Value) / 256f);
			}
			return 0;
		}

		// Token: 0x040032D7 RID: 13015
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <Segments>k__BackingField;

		// Token: 0x040032D8 RID: 13016
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x040032D9 RID: 13017
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<float> <PointToPreviousPointDistances>k__BackingField;

		// Token: 0x040032DA RID: 13018
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphere> <GrowLineSpheres>k__BackingField;
	}
}
