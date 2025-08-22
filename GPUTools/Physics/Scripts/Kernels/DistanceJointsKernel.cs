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
	// Token: 0x02000A4C RID: 2636
	public class DistanceJointsKernel : KernelBase
	{
		// Token: 0x060043DE RID: 17374 RVA: 0x0013D766 File Offset: 0x0013BB66
		public DistanceJointsKernel(GroupedData<GPDistanceJoint> groupedData, GpuBuffer<GPDistanceJoint> distanceJointsBuffer) : base("Compute/DistanceJoints", "CSDistanceJoints")
		{
			this.DistanceJoints = groupedData;
			this.DistanceJointsBuffer = distanceJointsBuffer;
		}

		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x060043E0 RID: 17376 RVA: 0x0013D78F File Offset: 0x0013BB8F
		// (set) Token: 0x060043DF RID: 17375 RVA: 0x0013D786 File Offset: 0x0013BB86
		[GpuData("distanceScale")]
		public GpuValue<float> DistanceScale
		{
			[CompilerGenerated]
			get
			{
				return this.<DistanceScale>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DistanceScale>k__BackingField = value;
			}
		}

		// Token: 0x170008BB RID: 2235
		// (get) Token: 0x060043E2 RID: 17378 RVA: 0x0013D7A0 File Offset: 0x0013BBA0
		// (set) Token: 0x060043E1 RID: 17377 RVA: 0x0013D797 File Offset: 0x0013BB97
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

		// Token: 0x170008BC RID: 2236
		// (get) Token: 0x060043E4 RID: 17380 RVA: 0x0013D7B1 File Offset: 0x0013BBB1
		// (set) Token: 0x060043E3 RID: 17379 RVA: 0x0013D7A8 File Offset: 0x0013BBA8
		[GpuData("distanceJoints")]
		public GpuBuffer<GPDistanceJoint> DistanceJointsBuffer
		{
			[CompilerGenerated]
			get
			{
				return this.<DistanceJointsBuffer>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DistanceJointsBuffer>k__BackingField = value;
			}
		}

		// Token: 0x170008BD RID: 2237
		// (get) Token: 0x060043E6 RID: 17382 RVA: 0x0013D7C2 File Offset: 0x0013BBC2
		// (set) Token: 0x060043E5 RID: 17381 RVA: 0x0013D7B9 File Offset: 0x0013BBB9
		[GpuData("distanceJointPower")]
		public GpuValue<float> DistanceJointPower
		{
			[CompilerGenerated]
			get
			{
				return this.<DistanceJointPower>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DistanceJointPower>k__BackingField = value;
			}
		}

		// Token: 0x170008BE RID: 2238
		// (get) Token: 0x060043E8 RID: 17384 RVA: 0x0013D7D3 File Offset: 0x0013BBD3
		// (set) Token: 0x060043E7 RID: 17383 RVA: 0x0013D7CA File Offset: 0x0013BBCA
		public GroupedData<GPDistanceJoint> DistanceJoints
		{
			[CompilerGenerated]
			get
			{
				return this.<DistanceJoints>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DistanceJoints>k__BackingField = value;
			}
		}

		// Token: 0x060043E9 RID: 17385 RVA: 0x0013D7DC File Offset: 0x0013BBDC
		public override void Dispatch()
		{
			if (!base.IsEnabled)
			{
				return;
			}
			if (this.Props.Count == 0)
			{
				this.CacheAttributes();
			}
			base.BindAttributes();
			for (int i = 0; i < this.DistanceJoints.GroupsData.Count; i++)
			{
				GroupData groupData = this.DistanceJoints.GroupsData[i];
				base.Shader.SetInt("startGroup", groupData.Start);
				base.Shader.SetInt("sizeGroup", groupData.Num);
				int num = Mathf.CeilToInt((float)groupData.Num / 256f);
				if (num > 0)
				{
					base.Shader.Dispatch(this.KernelId, num, 1, 1);
				}
			}
		}

		// Token: 0x04003293 RID: 12947
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <DistanceScale>k__BackingField;

		// Token: 0x04003294 RID: 12948
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x04003295 RID: 12949
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPDistanceJoint> <DistanceJointsBuffer>k__BackingField;

		// Token: 0x04003296 RID: 12950
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <DistanceJointPower>k__BackingField;

		// Token: 0x04003297 RID: 12951
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GroupedData<GPDistanceJoint> <DistanceJoints>k__BackingField;
	}
}
