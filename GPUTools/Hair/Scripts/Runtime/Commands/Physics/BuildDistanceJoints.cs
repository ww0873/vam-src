using System;
using System.Collections.Generic;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Joints;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Commands.Physics
{
	// Token: 0x02000A0D RID: 2573
	public class BuildDistanceJoints : BuildChainCommand
	{
		// Token: 0x06004145 RID: 16709 RVA: 0x00136093 File Offset: 0x00134493
		public BuildDistanceJoints(HairSettings settings)
		{
			this.settings = settings;
		}

		// Token: 0x06004146 RID: 16710 RVA: 0x001360A4 File Offset: 0x001344A4
		protected override void OnBuild()
		{
			if (this.settings.RuntimeData.Particles != null)
			{
				this.pointToPreviousPointDistances = new float[this.settings.RuntimeData.Particles.Count];
			}
			else
			{
				this.pointToPreviousPointDistances = new float[0];
			}
			this.CreateDistanceJoints();
			if (this.distanceJointsBuffer != null)
			{
				this.distanceJointsBuffer.Dispose();
			}
			if (this.distanceJoints.Data.Length > 0)
			{
				this.distanceJointsBuffer = new GpuBuffer<GPDistanceJoint>(this.distanceJoints.Data, GPDistanceJoint.Size());
			}
			else
			{
				this.distanceJointsBuffer = null;
			}
			this.settings.RuntimeData.DistanceJoints = this.distanceJoints;
			this.settings.RuntimeData.DistanceJointsBuffer = this.distanceJointsBuffer;
			if (this.settings.RuntimeData.PointToPreviousPointDistances != null)
			{
				this.settings.RuntimeData.PointToPreviousPointDistances.Dispose();
			}
			if (this.pointToPreviousPointDistances.Length > 0)
			{
				this.settings.RuntimeData.PointToPreviousPointDistances = new GpuBuffer<float>(this.pointToPreviousPointDistances, 4);
			}
			else
			{
				this.settings.RuntimeData.PointToPreviousPointDistances = null;
			}
		}

		// Token: 0x06004147 RID: 16711 RVA: 0x001361E4 File Offset: 0x001345E4
		protected override void OnUpdateSettings()
		{
			this.CreateDistanceJoints();
			this.settings.RuntimeData.DistanceJoints = this.distanceJoints;
			if (this.distanceJointsBuffer != null)
			{
				this.distanceJointsBuffer.Data = this.distanceJoints.Data;
				this.distanceJointsBuffer.PushData();
			}
			if (this.settings.RuntimeData.PointToPreviousPointDistances != null)
			{
				this.settings.RuntimeData.PointToPreviousPointDistances.PushData();
			}
		}

		// Token: 0x06004148 RID: 16712 RVA: 0x00136264 File Offset: 0x00134664
		public void RebuildFromGPUData()
		{
			this.settings.RuntimeData.DistanceJointsBuffer.PullData();
			GPDistanceJoint[] data = this.settings.RuntimeData.DistanceJointsBuffer.Data;
		}

		// Token: 0x06004149 RID: 16713 RVA: 0x0013629C File Offset: 0x0013469C
		private void CreateDistanceJoints()
		{
			int segments = this.settings.StandsSettings.Segments;
			if (this.distanceJoints != null)
			{
				this.distanceJoints.Dispose();
			}
			this.distanceJoints = new GroupedData<GPDistanceJoint>();
			List<GPDistanceJoint> list = new List<GPDistanceJoint>();
			List<GPDistanceJoint> list2 = new List<GPDistanceJoint>();
			if (this.settings.RuntimeData.Particles != null)
			{
				for (int i = 0; i < this.settings.RuntimeData.Particles.Count; i++)
				{
					if (i % segments != 0)
					{
						GPParticle gpparticle = this.settings.RuntimeData.Particles.Data[i - 1];
						GPParticle gpparticle2 = this.settings.RuntimeData.Particles.Data[i];
						float num = Vector3.Distance(gpparticle.Position, gpparticle2.Position);
						this.pointToPreviousPointDistances[i] = num;
						List<GPDistanceJoint> list3 = (i % 2 != 0) ? list : list2;
						GPDistanceJoint item = new GPDistanceJoint(i - 1, i, num, 1f);
						list3.Add(item);
					}
				}
			}
			this.distanceJoints.AddGroup(list);
			this.distanceJoints.AddGroup(list2);
		}

		// Token: 0x0600414A RID: 16714 RVA: 0x001363D8 File Offset: 0x001347D8
		protected override void OnDispose()
		{
			if (this.distanceJoints != null)
			{
				this.distanceJoints.Dispose();
			}
			if (this.distanceJointsBuffer != null)
			{
				this.distanceJointsBuffer.Dispose();
			}
			if (this.settings.RuntimeData.PointToPreviousPointDistances != null)
			{
				this.settings.RuntimeData.PointToPreviousPointDistances.Dispose();
			}
		}

		// Token: 0x04003100 RID: 12544
		private readonly HairSettings settings;

		// Token: 0x04003101 RID: 12545
		private GroupedData<GPDistanceJoint> distanceJoints;

		// Token: 0x04003102 RID: 12546
		private GpuBuffer<GPDistanceJoint> distanceJointsBuffer;

		// Token: 0x04003103 RID: 12547
		private float[] pointToPreviousPointDistances;
	}
}
