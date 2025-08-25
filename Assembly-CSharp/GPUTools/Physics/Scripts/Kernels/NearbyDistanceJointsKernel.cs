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
	// Token: 0x02000A56 RID: 2646
	public class NearbyDistanceJointsKernel : KernelBase
	{
		// Token: 0x06004440 RID: 17472 RVA: 0x0013DE5B File Offset: 0x0013C25B
		public NearbyDistanceJointsKernel(GroupedData<GPDistanceJoint> groupedData, GpuBuffer<GPDistanceJoint> nearbyDistanceJointsBuffer) : base("Compute/NearbyDistanceJoints", "CSNearbyDistanceJoints")
		{
			this.NearbyDistanceJoints = groupedData;
			this.NearbyDistanceJointsBuffer = nearbyDistanceJointsBuffer;
		}

		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x06004442 RID: 17474 RVA: 0x0013DE84 File Offset: 0x0013C284
		// (set) Token: 0x06004441 RID: 17473 RVA: 0x0013DE7B File Offset: 0x0013C27B
		[GpuData("nearbyDistanceScale")]
		public GpuValue<float> NearbyDistanceScale
		{
			[CompilerGenerated]
			get
			{
				return this.<NearbyDistanceScale>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<NearbyDistanceScale>k__BackingField = value;
			}
		}

		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x06004444 RID: 17476 RVA: 0x0013DE95 File Offset: 0x0013C295
		// (set) Token: 0x06004443 RID: 17475 RVA: 0x0013DE8C File Offset: 0x0013C28C
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

		// Token: 0x170008E3 RID: 2275
		// (get) Token: 0x06004446 RID: 17478 RVA: 0x0013DEA6 File Offset: 0x0013C2A6
		// (set) Token: 0x06004445 RID: 17477 RVA: 0x0013DE9D File Offset: 0x0013C29D
		[GpuData("nearbyDistanceJoints")]
		public GpuBuffer<GPDistanceJoint> NearbyDistanceJointsBuffer
		{
			[CompilerGenerated]
			get
			{
				return this.<NearbyDistanceJointsBuffer>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<NearbyDistanceJointsBuffer>k__BackingField = value;
			}
		}

		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x06004448 RID: 17480 RVA: 0x0013DEB7 File Offset: 0x0013C2B7
		// (set) Token: 0x06004447 RID: 17479 RVA: 0x0013DEAE File Offset: 0x0013C2AE
		[GpuData("nearbyJointPower")]
		public GpuValue<float> NearbyJointPower
		{
			[CompilerGenerated]
			get
			{
				return this.<NearbyJointPower>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<NearbyJointPower>k__BackingField = value;
			}
		}

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x0600444A RID: 17482 RVA: 0x0013DEC8 File Offset: 0x0013C2C8
		// (set) Token: 0x06004449 RID: 17481 RVA: 0x0013DEBF File Offset: 0x0013C2BF
		[GpuData("nearbyJointPowerRolloff")]
		public GpuValue<float> NearbyJointPowerRolloff
		{
			[CompilerGenerated]
			get
			{
				return this.<NearbyJointPowerRolloff>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<NearbyJointPowerRolloff>k__BackingField = value;
			}
		}

		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x0600444C RID: 17484 RVA: 0x0013DED9 File Offset: 0x0013C2D9
		// (set) Token: 0x0600444B RID: 17483 RVA: 0x0013DED0 File Offset: 0x0013C2D0
		public GroupedData<GPDistanceJoint> NearbyDistanceJoints
		{
			[CompilerGenerated]
			get
			{
				return this.<NearbyDistanceJoints>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<NearbyDistanceJoints>k__BackingField = value;
			}
		}

		// Token: 0x0600444D RID: 17485 RVA: 0x0013DEE4 File Offset: 0x0013C2E4
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
			for (int i = 0; i < this.NearbyDistanceJoints.GroupsData.Count; i++)
			{
				GroupData groupData = this.NearbyDistanceJoints.GroupsData[i];
				base.Shader.SetInt("startGroup", groupData.Start);
				base.Shader.SetInt("sizeGroup", groupData.Num);
				int num = Mathf.CeilToInt((float)groupData.Num / 256f);
				if (num > 0)
				{
					base.Shader.Dispatch(this.KernelId, num, 1, 1);
				}
			}
		}

		// Token: 0x040032BA RID: 12986
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <NearbyDistanceScale>k__BackingField;

		// Token: 0x040032BB RID: 12987
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x040032BC RID: 12988
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPDistanceJoint> <NearbyDistanceJointsBuffer>k__BackingField;

		// Token: 0x040032BD RID: 12989
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <NearbyJointPower>k__BackingField;

		// Token: 0x040032BE RID: 12990
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <NearbyJointPowerRolloff>k__BackingField;

		// Token: 0x040032BF RID: 12991
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GroupedData<GPDistanceJoint> <NearbyDistanceJoints>k__BackingField;
	}
}
