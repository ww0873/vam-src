using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using GPUTools.Common.Scripts.PL.Abstract;
using GPUTools.Common.Scripts.PL.Attributes;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Joints;
using UnityEngine;

namespace GPUTools.Physics.Scripts.Kernels
{
	// Token: 0x02000A73 RID: 2675
	public class ResetToPointJointsPreCalculatedKernel : KernelBase
	{
		// Token: 0x06004552 RID: 17746 RVA: 0x0013ECE2 File Offset: 0x0013D0E2
		public ResetToPointJointsPreCalculatedKernel() : base("Compute/ResetToPointJointsPreCalculated", "CSResetToPointJointsPreCalculated")
		{
		}

		// Token: 0x1700094D RID: 2381
		// (get) Token: 0x06004554 RID: 17748 RVA: 0x0013ECFD File Offset: 0x0013D0FD
		// (set) Token: 0x06004553 RID: 17747 RVA: 0x0013ECF4 File Offset: 0x0013D0F4
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

		// Token: 0x1700094E RID: 2382
		// (get) Token: 0x06004556 RID: 17750 RVA: 0x0013ED0E File Offset: 0x0013D10E
		// (set) Token: 0x06004555 RID: 17749 RVA: 0x0013ED05 File Offset: 0x0013D105
		[GpuData("allPointJoints")]
		public GpuBuffer<GPPointJoint> AllPointJoints
		{
			[CompilerGenerated]
			get
			{
				return this.<AllPointJoints>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<AllPointJoints>k__BackingField = value;
			}
		}

		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x06004558 RID: 17752 RVA: 0x0013ED1F File Offset: 0x0013D11F
		// (set) Token: 0x06004557 RID: 17751 RVA: 0x0013ED16 File Offset: 0x0013D116
		[GpuData("positions")]
		public GpuBuffer<Vector3> Positions
		{
			[CompilerGenerated]
			get
			{
				return this.<Positions>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Positions>k__BackingField = value;
			}
		}

		// Token: 0x06004559 RID: 17753 RVA: 0x0013ED27 File Offset: 0x0013D127
		public override int GetGroupsNumX()
		{
			if (this.AllPointJoints != null)
			{
				return Mathf.CeilToInt((float)this.AllPointJoints.ComputeBuffer.count / 256f);
			}
			return 0;
		}

		// Token: 0x04003326 RID: 13094
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x04003327 RID: 13095
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPPointJoint> <AllPointJoints>k__BackingField;

		// Token: 0x04003328 RID: 13096
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<Vector3> <Positions>k__BackingField;
	}
}
