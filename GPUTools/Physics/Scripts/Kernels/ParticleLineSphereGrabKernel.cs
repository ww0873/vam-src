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
	// Token: 0x02000A5D RID: 2653
	public class ParticleLineSphereGrabKernel : KernelBase
	{
		// Token: 0x06004482 RID: 17538 RVA: 0x0013E265 File Offset: 0x0013C665
		public ParticleLineSphereGrabKernel() : base("Compute/ParticleLineSphereGrab", "CSParticleLineSphereGrab")
		{
		}

		// Token: 0x170008FB RID: 2299
		// (get) Token: 0x06004484 RID: 17540 RVA: 0x0013E280 File Offset: 0x0013C680
		// (set) Token: 0x06004483 RID: 17539 RVA: 0x0013E277 File Offset: 0x0013C677
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

		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x06004486 RID: 17542 RVA: 0x0013E291 File Offset: 0x0013C691
		// (set) Token: 0x06004485 RID: 17541 RVA: 0x0013E288 File Offset: 0x0013C688
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

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x06004488 RID: 17544 RVA: 0x0013E2A2 File Offset: 0x0013C6A2
		// (set) Token: 0x06004487 RID: 17543 RVA: 0x0013E299 File Offset: 0x0013C699
		[GpuData("grabLineSpheres")]
		public GpuBuffer<GPLineSphereWithMatrixDelta> GrabLineSpheres
		{
			[CompilerGenerated]
			get
			{
				return this.<GrabLineSpheres>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<GrabLineSpheres>k__BackingField = value;
			}
		}

		// Token: 0x06004489 RID: 17545 RVA: 0x0013E2AA File Offset: 0x0013C6AA
		public override int GetGroupsNumX()
		{
			if (this.Particles != null)
			{
				return Mathf.CeilToInt((float)(this.Particles.Count / this.Segments.Value) / 256f);
			}
			return 0;
		}

		// Token: 0x040032D4 RID: 13012
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<int> <Segments>k__BackingField;

		// Token: 0x040032D5 RID: 13013
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x040032D6 RID: 13014
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphereWithMatrixDelta> <GrabLineSpheres>k__BackingField;
	}
}
