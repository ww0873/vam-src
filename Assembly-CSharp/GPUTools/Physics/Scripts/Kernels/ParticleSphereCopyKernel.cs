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
	// Token: 0x02000A6A RID: 2666
	public class ParticleSphereCopyKernel : KernelBase
	{
		// Token: 0x060044F0 RID: 17648 RVA: 0x0013E81F File Offset: 0x0013CC1F
		public ParticleSphereCopyKernel() : base("Compute/ParticleSphereCopy", "CSParticleSphereCopy")
		{
		}

		// Token: 0x17000925 RID: 2341
		// (get) Token: 0x060044F2 RID: 17650 RVA: 0x0013E83A File Offset: 0x0013CC3A
		// (set) Token: 0x060044F1 RID: 17649 RVA: 0x0013E831 File Offset: 0x0013CC31
		[GpuData("spheres")]
		public GpuBuffer<GPSphere> Spheres
		{
			[CompilerGenerated]
			get
			{
				return this.<Spheres>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Spheres>k__BackingField = value;
			}
		}

		// Token: 0x17000926 RID: 2342
		// (get) Token: 0x060044F4 RID: 17652 RVA: 0x0013E84B File Offset: 0x0013CC4B
		// (set) Token: 0x060044F3 RID: 17651 RVA: 0x0013E842 File Offset: 0x0013CC42
		[GpuData("oldSpheres")]
		public GpuBuffer<GPSphere> OldSpheres
		{
			[CompilerGenerated]
			get
			{
				return this.<OldSpheres>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<OldSpheres>k__BackingField = value;
			}
		}

		// Token: 0x060044F5 RID: 17653 RVA: 0x0013E853 File Offset: 0x0013CC53
		public override int GetGroupsNumX()
		{
			if (this.Spheres != null)
			{
				return Mathf.CeilToInt((float)this.Spheres.Count / 256f);
			}
			return 0;
		}

		// Token: 0x040032FE RID: 13054
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPSphere> <Spheres>k__BackingField;

		// Token: 0x040032FF RID: 13055
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPSphere> <OldSpheres>k__BackingField;
	}
}
