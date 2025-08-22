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
	// Token: 0x02000A68 RID: 2664
	public class ParticleSphereCollisionHoldKernel : KernelBase
	{
		// Token: 0x060044DA RID: 17626 RVA: 0x0013E716 File Offset: 0x0013CB16
		public ParticleSphereCollisionHoldKernel() : base("Compute/ParticleSphereCollisionHold", "CSParticleSphereCollisionHold")
		{
		}

		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x060044DC RID: 17628 RVA: 0x0013E731 File Offset: 0x0013CB31
		// (set) Token: 0x060044DB RID: 17627 RVA: 0x0013E728 File Offset: 0x0013CB28
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

		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x060044DE RID: 17630 RVA: 0x0013E742 File Offset: 0x0013CB42
		// (set) Token: 0x060044DD RID: 17629 RVA: 0x0013E739 File Offset: 0x0013CB39
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

		// Token: 0x1700091E RID: 2334
		// (get) Token: 0x060044E0 RID: 17632 RVA: 0x0013E753 File Offset: 0x0013CB53
		// (set) Token: 0x060044DF RID: 17631 RVA: 0x0013E74A File Offset: 0x0013CB4A
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

		// Token: 0x060044E1 RID: 17633 RVA: 0x0013E75B File Offset: 0x0013CB5B
		public override int GetGroupsNumX()
		{
			if (this.Particles != null)
			{
				return Mathf.CeilToInt((float)this.Particles.Count / 256f);
			}
			return 0;
		}

		// Token: 0x040032F5 RID: 13045
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <Step>k__BackingField;

		// Token: 0x040032F6 RID: 13046
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x040032F7 RID: 13047
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPSphereWithDelta> <ProcessedSpheres>k__BackingField;
	}
}
