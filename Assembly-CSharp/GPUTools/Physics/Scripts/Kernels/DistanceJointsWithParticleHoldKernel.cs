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
	// Token: 0x02000A4E RID: 2638
	public class DistanceJointsWithParticleHoldKernel : KernelBase
	{
		// Token: 0x060043F8 RID: 17400 RVA: 0x0013D9ED File Offset: 0x0013BDED
		public DistanceJointsWithParticleHoldKernel(GroupedData<GPDistanceJoint> groupedData, GpuBuffer<GPDistanceJoint> distanceJointsBuffer) : base("Compute/DistanceJointsWithParticleHold", "CSDistanceJointsWithParticleHold")
		{
			this.DistanceJoints = groupedData;
			this.DistanceJointsBuffer = distanceJointsBuffer;
		}

		// Token: 0x170008C5 RID: 2245
		// (get) Token: 0x060043FA RID: 17402 RVA: 0x0013DA16 File Offset: 0x0013BE16
		// (set) Token: 0x060043F9 RID: 17401 RVA: 0x0013DA0D File Offset: 0x0013BE0D
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

		// Token: 0x170008C6 RID: 2246
		// (get) Token: 0x060043FC RID: 17404 RVA: 0x0013DA27 File Offset: 0x0013BE27
		// (set) Token: 0x060043FB RID: 17403 RVA: 0x0013DA1E File Offset: 0x0013BE1E
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

		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x060043FE RID: 17406 RVA: 0x0013DA38 File Offset: 0x0013BE38
		// (set) Token: 0x060043FD RID: 17405 RVA: 0x0013DA2F File Offset: 0x0013BE2F
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

		// Token: 0x170008C8 RID: 2248
		// (get) Token: 0x06004400 RID: 17408 RVA: 0x0013DA49 File Offset: 0x0013BE49
		// (set) Token: 0x060043FF RID: 17407 RVA: 0x0013DA40 File Offset: 0x0013BE40
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

		// Token: 0x170008C9 RID: 2249
		// (get) Token: 0x06004402 RID: 17410 RVA: 0x0013DA5A File Offset: 0x0013BE5A
		// (set) Token: 0x06004401 RID: 17409 RVA: 0x0013DA51 File Offset: 0x0013BE51
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

		// Token: 0x06004403 RID: 17411 RVA: 0x0013DA64 File Offset: 0x0013BE64
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

		// Token: 0x0400329E RID: 12958
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <DistanceScale>k__BackingField;

		// Token: 0x0400329F RID: 12959
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x040032A0 RID: 12960
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPDistanceJoint> <DistanceJointsBuffer>k__BackingField;

		// Token: 0x040032A1 RID: 12961
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <DistanceJointPower>k__BackingField;

		// Token: 0x040032A2 RID: 12962
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GroupedData<GPDistanceJoint> <DistanceJoints>k__BackingField;
	}
}
