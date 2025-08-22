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
	// Token: 0x02000A4D RID: 2637
	public class DistanceJointsUniformRigidityKernel : KernelBase
	{
		// Token: 0x060043EA RID: 17386 RVA: 0x0013D8A1 File Offset: 0x0013BCA1
		public DistanceJointsUniformRigidityKernel(GroupedData<GPDistanceJoint> groupedData, GpuBuffer<GPDistanceJoint> distanceJointsBuffer) : base("Compute/DistanceJointsUniformRigidity", "CSDistanceJointsUniformRigidity")
		{
			this.DistanceJoints = groupedData;
			this.DistanceJointsBuffer = distanceJointsBuffer;
		}

		// Token: 0x170008BF RID: 2239
		// (get) Token: 0x060043EC RID: 17388 RVA: 0x0013D8CA File Offset: 0x0013BCCA
		// (set) Token: 0x060043EB RID: 17387 RVA: 0x0013D8C1 File Offset: 0x0013BCC1
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

		// Token: 0x170008C0 RID: 2240
		// (get) Token: 0x060043EE RID: 17390 RVA: 0x0013D8DB File Offset: 0x0013BCDB
		// (set) Token: 0x060043ED RID: 17389 RVA: 0x0013D8D2 File Offset: 0x0013BCD2
		[GpuData("rigidity")]
		public GpuValue<float> Rigidity
		{
			[CompilerGenerated]
			get
			{
				return this.<Rigidity>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Rigidity>k__BackingField = value;
			}
		}

		// Token: 0x170008C1 RID: 2241
		// (get) Token: 0x060043F0 RID: 17392 RVA: 0x0013D8EC File Offset: 0x0013BCEC
		// (set) Token: 0x060043EF RID: 17391 RVA: 0x0013D8E3 File Offset: 0x0013BCE3
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

		// Token: 0x170008C2 RID: 2242
		// (get) Token: 0x060043F2 RID: 17394 RVA: 0x0013D8FD File Offset: 0x0013BCFD
		// (set) Token: 0x060043F1 RID: 17393 RVA: 0x0013D8F4 File Offset: 0x0013BCF4
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

		// Token: 0x170008C3 RID: 2243
		// (get) Token: 0x060043F4 RID: 17396 RVA: 0x0013D90E File Offset: 0x0013BD0E
		// (set) Token: 0x060043F3 RID: 17395 RVA: 0x0013D905 File Offset: 0x0013BD05
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

		// Token: 0x170008C4 RID: 2244
		// (get) Token: 0x060043F6 RID: 17398 RVA: 0x0013D91F File Offset: 0x0013BD1F
		// (set) Token: 0x060043F5 RID: 17397 RVA: 0x0013D916 File Offset: 0x0013BD16
		private GroupedData<GPDistanceJoint> DistanceJoints
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

		// Token: 0x060043F7 RID: 17399 RVA: 0x0013D928 File Offset: 0x0013BD28
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

		// Token: 0x04003298 RID: 12952
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <DistanceScale>k__BackingField;

		// Token: 0x04003299 RID: 12953
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <Rigidity>k__BackingField;

		// Token: 0x0400329A RID: 12954
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuValue<float> <CompressionResistance>k__BackingField;

		// Token: 0x0400329B RID: 12955
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPParticle> <Particles>k__BackingField;

		// Token: 0x0400329C RID: 12956
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GpuBuffer<GPDistanceJoint> <DistanceJointsBuffer>k__BackingField;

		// Token: 0x0400329D RID: 12957
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private GroupedData<GPDistanceJoint> <DistanceJoints>k__BackingField;
	}
}
