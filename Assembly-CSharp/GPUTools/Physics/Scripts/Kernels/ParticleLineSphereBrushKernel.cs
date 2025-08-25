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
	// Token: 0x02000A58 RID: 2648
	public class ParticleLineSphereBrushKernel : KernelBase
	{
		// Token: 0x06004452 RID: 17490 RVA: 0x0013DFF2 File Offset: 0x0013C3F2
		public ParticleLineSphereBrushKernel() : base("Compute/ParticleLineSphereBrush", "CSParticleLineSphereBrush")
		{
		}

		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x06004454 RID: 17492 RVA: 0x0013E00D File Offset: 0x0013C40D
		// (set) Token: 0x06004453 RID: 17491 RVA: 0x0013E004 File Offset: 0x0013C404
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

		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x06004456 RID: 17494 RVA: 0x0013E01E File Offset: 0x0013C41E
		// (set) Token: 0x06004455 RID: 17493 RVA: 0x0013E015 File Offset: 0x0013C415
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

		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x06004458 RID: 17496 RVA: 0x0013E02F File Offset: 0x0013C42F
		// (set) Token: 0x06004457 RID: 17495 RVA: 0x0013E026 File Offset: 0x0013C426
		[GpuData("brushLineSpheres")]
		public GpuBuffer<GPLineSphereWithDelta> BrushLineSpheres
		{
			[CompilerGenerated]
			get
			{
				return this.<BrushLineSpheres>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<BrushLineSpheres>k__BackingField = value;
			}
		}

		// Token: 0x06004459 RID: 17497 RVA: 0x0013E037 File Offset: 0x0013C437
		public override int GetGroupsNumX()
		{
			if (this.Particles != null)
			{
				return Mathf.CeilToInt((float)(this.Particles.Count / this.Segments.Value) / 256f);
			}
			return 0;
		}

		// Token: 0x040032C1 RID: 12993
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <Segments>k__BackingField;

		// Token: 0x040032C2 RID: 12994
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x040032C3 RID: 12995
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphereWithDelta> <BrushLineSpheres>k__BackingField;
	}
}
