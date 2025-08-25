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
	// Token: 0x02000A52 RID: 2642
	public class IntegrateKernel : KernelBase
	{
		// Token: 0x0600441E RID: 17438 RVA: 0x0013DC8A File Offset: 0x0013C08A
		public IntegrateKernel() : base("Compute/Integrate", "CSIntegrate")
		{
		}

		// Token: 0x170008D4 RID: 2260
		// (get) Token: 0x06004420 RID: 17440 RVA: 0x0013DCA5 File Offset: 0x0013C0A5
		// (set) Token: 0x0600441F RID: 17439 RVA: 0x0013DC9C File Offset: 0x0013C09C
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

		// Token: 0x170008D5 RID: 2261
		// (get) Token: 0x06004422 RID: 17442 RVA: 0x0013DCB6 File Offset: 0x0013C0B6
		// (set) Token: 0x06004421 RID: 17441 RVA: 0x0013DCAD File Offset: 0x0013C0AD
		[GpuData("invDrag")]
		public GpuValue<float> InvDrag
		{
			[CompilerGenerated]
			get
			{
				return this.<InvDrag>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<InvDrag>k__BackingField = value;
			}
		}

		// Token: 0x170008D6 RID: 2262
		// (get) Token: 0x06004424 RID: 17444 RVA: 0x0013DCC7 File Offset: 0x0013C0C7
		// (set) Token: 0x06004423 RID: 17443 RVA: 0x0013DCBE File Offset: 0x0013C0BE
		[GpuData("accelDT2")]
		public GpuValue<Vector3> AccelDT2
		{
			[CompilerGenerated]
			get
			{
				return this.<AccelDT2>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<AccelDT2>k__BackingField = value;
			}
		}

		// Token: 0x06004425 RID: 17445 RVA: 0x0013DCCF File Offset: 0x0013C0CF
		public override int GetGroupsNumX()
		{
			if (this.Particles != null)
			{
				return Mathf.CeilToInt((float)this.Particles.ComputeBuffer.count / 256f);
			}
			return 0;
		}

		// Token: 0x040032AD RID: 12973
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x040032AE RID: 12974
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <InvDrag>k__BackingField;

		// Token: 0x040032AF RID: 12975
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<Vector3> <AccelDT2>k__BackingField;
	}
}
