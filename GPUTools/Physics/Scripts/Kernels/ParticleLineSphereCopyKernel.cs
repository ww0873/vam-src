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
	// Token: 0x02000A5B RID: 2651
	public class ParticleLineSphereCopyKernel : KernelBase
	{
		// Token: 0x06004470 RID: 17520 RVA: 0x0013E172 File Offset: 0x0013C572
		public ParticleLineSphereCopyKernel() : base("Compute/ParticleLineSphereCopy", "CSParticleLineSphereCopy")
		{
		}

		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x06004472 RID: 17522 RVA: 0x0013E18D File Offset: 0x0013C58D
		// (set) Token: 0x06004471 RID: 17521 RVA: 0x0013E184 File Offset: 0x0013C584
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

		// Token: 0x170008F5 RID: 2293
		// (get) Token: 0x06004474 RID: 17524 RVA: 0x0013E19E File Offset: 0x0013C59E
		// (set) Token: 0x06004473 RID: 17523 RVA: 0x0013E195 File Offset: 0x0013C595
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

		// Token: 0x06004475 RID: 17525 RVA: 0x0013E1A6 File Offset: 0x0013C5A6
		public override int GetGroupsNumX()
		{
			if (this.LineSpheres != null)
			{
				return Mathf.CeilToInt((float)this.LineSpheres.Count / 256f);
			}
			return 0;
		}

		// Token: 0x040032CD RID: 13005
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphere> <LineSpheres>k__BackingField;

		// Token: 0x040032CE RID: 13006
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPLineSphere> <OldLineSpheres>k__BackingField;
	}
}
