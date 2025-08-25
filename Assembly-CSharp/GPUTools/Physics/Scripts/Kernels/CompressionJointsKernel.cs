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
	// Token: 0x02000A49 RID: 2633
	public class CompressionJointsKernel : KernelBase
	{
		// Token: 0x060043C0 RID: 17344 RVA: 0x0013D54D File Offset: 0x0013B94D
		public CompressionJointsKernel(GroupedData<GPDistanceJoint> groupedData, GpuBuffer<GPDistanceJoint> compressionJointsBuffer) : base("Compute/CompressionJoints", "CSCompressionJoints")
		{
			this.CompressionJoints = groupedData;
			this.CompressionJointsBuffer = compressionJointsBuffer;
		}

		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x060043C2 RID: 17346 RVA: 0x0013D576 File Offset: 0x0013B976
		// (set) Token: 0x060043C1 RID: 17345 RVA: 0x0013D56D File Offset: 0x0013B96D
		[GpuData("compressionDistanceScale")]
		public GpuValue<float> CompressionDistanceScale
		{
			[CompilerGenerated]
			get
			{
				return this.<CompressionDistanceScale>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CompressionDistanceScale>k__BackingField = value;
			}
		}

		// Token: 0x170008AF RID: 2223
		// (get) Token: 0x060043C4 RID: 17348 RVA: 0x0013D587 File Offset: 0x0013B987
		// (set) Token: 0x060043C3 RID: 17347 RVA: 0x0013D57E File Offset: 0x0013B97E
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

		// Token: 0x170008B0 RID: 2224
		// (get) Token: 0x060043C6 RID: 17350 RVA: 0x0013D598 File Offset: 0x0013B998
		// (set) Token: 0x060043C5 RID: 17349 RVA: 0x0013D58F File Offset: 0x0013B98F
		[GpuData("compressionJoints")]
		public GpuBuffer<GPDistanceJoint> CompressionJointsBuffer
		{
			[CompilerGenerated]
			get
			{
				return this.<CompressionJointsBuffer>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CompressionJointsBuffer>k__BackingField = value;
			}
		}

		// Token: 0x170008B1 RID: 2225
		// (get) Token: 0x060043C8 RID: 17352 RVA: 0x0013D5A9 File Offset: 0x0013B9A9
		// (set) Token: 0x060043C7 RID: 17351 RVA: 0x0013D5A0 File Offset: 0x0013B9A0
		[GpuData("compressionJointPower")]
		public GpuValue<float> CompressionJointPower
		{
			[CompilerGenerated]
			get
			{
				return this.<CompressionJointPower>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CompressionJointPower>k__BackingField = value;
			}
		}

		// Token: 0x170008B2 RID: 2226
		// (get) Token: 0x060043CA RID: 17354 RVA: 0x0013D5BA File Offset: 0x0013B9BA
		// (set) Token: 0x060043C9 RID: 17353 RVA: 0x0013D5B1 File Offset: 0x0013B9B1
		public GroupedData<GPDistanceJoint> CompressionJoints
		{
			[CompilerGenerated]
			get
			{
				return this.<CompressionJoints>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<CompressionJoints>k__BackingField = value;
			}
		}

		// Token: 0x060043CB RID: 17355 RVA: 0x0013D5C4 File Offset: 0x0013B9C4
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
			for (int i = 0; i < this.CompressionJoints.GroupsData.Count; i++)
			{
				GroupData groupData = this.CompressionJoints.GroupsData[i];
				base.Shader.SetInt("startGroup", groupData.Start);
				base.Shader.SetInt("sizeGroup", groupData.Num);
				if (groupData.Num > 0)
				{
					int threadGroupsX = Mathf.CeilToInt((float)groupData.Num / 256f);
					base.Shader.Dispatch(this.KernelId, threadGroupsX, 1, 1);
				}
			}
		}

		// Token: 0x04003287 RID: 12935
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <CompressionDistanceScale>k__BackingField;

		// Token: 0x04003288 RID: 12936
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x04003289 RID: 12937
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPDistanceJoint> <CompressionJointsBuffer>k__BackingField;

		// Token: 0x0400328A RID: 12938
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <CompressionJointPower>k__BackingField;

		// Token: 0x0400328B RID: 12939
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GroupedData<GPDistanceJoint> <CompressionJoints>k__BackingField;
	}
}
