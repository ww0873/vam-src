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
	// Token: 0x02000A50 RID: 2640
	public class IntegrateIterKernel : KernelBase
	{
		// Token: 0x0600440A RID: 17418 RVA: 0x0013DB88 File Offset: 0x0013BF88
		public IntegrateIterKernel() : base("Compute/IntegrateIter", "CSIntegrateIter")
		{
		}

		// Token: 0x170008CC RID: 2252
		// (get) Token: 0x0600440C RID: 17420 RVA: 0x0013DBA3 File Offset: 0x0013BFA3
		// (set) Token: 0x0600440B RID: 17419 RVA: 0x0013DB9A File Offset: 0x0013BF9A
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

		// Token: 0x170008CD RID: 2253
		// (get) Token: 0x0600440E RID: 17422 RVA: 0x0013DBB4 File Offset: 0x0013BFB4
		// (set) Token: 0x0600440D RID: 17421 RVA: 0x0013DBAB File Offset: 0x0013BFAB
		[GpuData("dt")]
		public GpuValue<float> DT
		{
			[CompilerGenerated]
			get
			{
				return this.<DT>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DT>k__BackingField = value;
			}
		}

		// Token: 0x170008CE RID: 2254
		// (get) Token: 0x06004410 RID: 17424 RVA: 0x0013DBC5 File Offset: 0x0013BFC5
		// (set) Token: 0x0600440F RID: 17423 RVA: 0x0013DBBC File Offset: 0x0013BFBC
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

		// Token: 0x170008CF RID: 2255
		// (get) Token: 0x06004412 RID: 17426 RVA: 0x0013DBD6 File Offset: 0x0013BFD6
		// (set) Token: 0x06004411 RID: 17425 RVA: 0x0013DBCD File Offset: 0x0013BFCD
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

		// Token: 0x06004413 RID: 17427 RVA: 0x0013DBDE File Offset: 0x0013BFDE
		public override int GetGroupsNumX()
		{
			if (this.Particles != null)
			{
				return Mathf.CeilToInt((float)this.Particles.ComputeBuffer.count / 256f);
			}
			return 0;
		}

		// Token: 0x040032A5 RID: 12965
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x040032A6 RID: 12966
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <DT>k__BackingField;

		// Token: 0x040032A7 RID: 12967
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <InvDrag>k__BackingField;

		// Token: 0x040032A8 RID: 12968
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<Vector3> <AccelDT2>k__BackingField;
	}
}
