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
	// Token: 0x02000A79 RID: 2681
	public class StiffnessJointsKernel : KernelBase
	{
		// Token: 0x06004594 RID: 17812 RVA: 0x0013F03E File Offset: 0x0013D43E
		public StiffnessJointsKernel(GroupedData<GPDistanceJoint> groupedData, GpuBuffer<GPDistanceJoint> stiffnessJointsBuffer) : base("Compute/StiffnessJoints", "CSStiffnessJoints")
		{
			this.StiffnessJoints = groupedData;
			this.StiffnessJointsBuffer = stiffnessJointsBuffer;
		}

		// Token: 0x17000968 RID: 2408
		// (get) Token: 0x06004596 RID: 17814 RVA: 0x0013F067 File Offset: 0x0013D467
		// (set) Token: 0x06004595 RID: 17813 RVA: 0x0013F05E File Offset: 0x0013D45E
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

		// Token: 0x17000969 RID: 2409
		// (get) Token: 0x06004598 RID: 17816 RVA: 0x0013F078 File Offset: 0x0013D478
		// (set) Token: 0x06004597 RID: 17815 RVA: 0x0013F06F File Offset: 0x0013D46F
		[GpuData("stiffness")]
		public GpuValue<float> Stiffness
		{
			[CompilerGenerated]
			get
			{
				return this.<Stiffness>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Stiffness>k__BackingField = value;
			}
		}

		// Token: 0x1700096A RID: 2410
		// (get) Token: 0x0600459A RID: 17818 RVA: 0x0013F089 File Offset: 0x0013D489
		// (set) Token: 0x06004599 RID: 17817 RVA: 0x0013F080 File Offset: 0x0013D480
		[GpuData("compressionResistance")]
		public GpuValue<float> CompressionResistance
		{
			[CompilerGenerated]
			get
			{
				return this.<CompressionResistance>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CompressionResistance>k__BackingField = value;
			}
		}

		// Token: 0x1700096B RID: 2411
		// (get) Token: 0x0600459C RID: 17820 RVA: 0x0013F09A File Offset: 0x0013D49A
		// (set) Token: 0x0600459B RID: 17819 RVA: 0x0013F091 File Offset: 0x0013D491
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

		// Token: 0x1700096C RID: 2412
		// (get) Token: 0x0600459E RID: 17822 RVA: 0x0013F0AB File Offset: 0x0013D4AB
		// (set) Token: 0x0600459D RID: 17821 RVA: 0x0013F0A2 File Offset: 0x0013D4A2
		[GpuData("stiffnessJoints")]
		public GpuBuffer<GPDistanceJoint> StiffnessJointsBuffer
		{
			[CompilerGenerated]
			get
			{
				return this.<StiffnessJointsBuffer>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<StiffnessJointsBuffer>k__BackingField = value;
			}
		}

		// Token: 0x1700096D RID: 2413
		// (get) Token: 0x060045A0 RID: 17824 RVA: 0x0013F0BC File Offset: 0x0013D4BC
		// (set) Token: 0x0600459F RID: 17823 RVA: 0x0013F0B3 File Offset: 0x0013D4B3
		private GroupedData<GPDistanceJoint> StiffnessJoints
		{
			[CompilerGenerated]
			get
			{
				return this.<StiffnessJoints>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<StiffnessJoints>k__BackingField = value;
			}
		}

		// Token: 0x060045A1 RID: 17825 RVA: 0x0013F0C4 File Offset: 0x0013D4C4
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
			for (int i = 0; i < this.StiffnessJoints.GroupsData.Count; i++)
			{
				GroupData groupData = this.StiffnessJoints.GroupsData[i];
				base.Shader.SetInt("startGroup", groupData.Start);
				base.Shader.SetInt("sizeGroup", groupData.Num);
				int num = Mathf.CeilToInt((float)groupData.Num / 256f);
				if (num > 0)
				{
					base.Shader.Dispatch(this.KernelId, num, 1, 1);
				}
			}
		}

		// Token: 0x04003341 RID: 13121
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <DistanceScale>k__BackingField;

		// Token: 0x04003342 RID: 13122
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <Stiffness>k__BackingField;

		// Token: 0x04003343 RID: 13123
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <CompressionResistance>k__BackingField;

		// Token: 0x04003344 RID: 13124
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x04003345 RID: 13125
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPDistanceJoint> <StiffnessJointsBuffer>k__BackingField;

		// Token: 0x04003346 RID: 13126
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GroupedData<GPDistanceJoint> <StiffnessJoints>k__BackingField;
	}
}
