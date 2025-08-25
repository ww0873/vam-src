using System;
using System.Collections.Generic;
using GPUTools.Cloth.Scripts.Types;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Joints;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Runtime.Commands
{
	// Token: 0x02000997 RID: 2455
	public class BuildDistanceJoints : BuildChainCommand
	{
		// Token: 0x06003D5F RID: 15711 RVA: 0x00129E79 File Offset: 0x00128279
		public BuildDistanceJoints(ClothSettings settings)
		{
			this.settings = settings;
		}

		// Token: 0x06003D60 RID: 15712 RVA: 0x00129E88 File Offset: 0x00128288
		protected override void OnBuild()
		{
			this.CreateDistanceJoints();
			this.distanceJointsBuffer = new GpuBuffer<GPDistanceJoint>(this.distanceJoints.Data, GPDistanceJoint.Size());
			this.settings.Runtime.DistanceJoints = this.distanceJoints;
			this.settings.Runtime.DistanceJointsBuffer = this.distanceJointsBuffer;
		}

		// Token: 0x06003D61 RID: 15713 RVA: 0x00129EE2 File Offset: 0x001282E2
		protected override void OnUpdateSettings()
		{
			this.CreateDistanceJoints();
			this.settings.Runtime.DistanceJoints = this.distanceJoints;
			this.distanceJointsBuffer.Data = this.distanceJoints.Data;
			this.distanceJointsBuffer.PushData();
		}

		// Token: 0x06003D62 RID: 15714 RVA: 0x00129F24 File Offset: 0x00128324
		private void CreateDistanceJoints()
		{
			this.distanceJoints = new GroupedData<GPDistanceJoint>();
			List<Int2ListContainer> jointGroups = this.settings.GeometryData.JointGroups;
			GPParticle[] data = this.settings.Runtime.Particles.Data;
			foreach (Int2ListContainer int2ListContainer in jointGroups)
			{
				List<GPDistanceJoint> list = new List<GPDistanceJoint>();
				foreach (Int2 @int in int2ListContainer.List)
				{
					GPParticle gpparticle = data[@int.X];
					GPParticle gpparticle2 = data[@int.Y];
					float distance = Vector3.Distance(gpparticle.Position, gpparticle2.Position) / this.settings.transform.lossyScale.x;
					GPDistanceJoint item = new GPDistanceJoint(@int.X, @int.Y, distance, 1f - this.settings.Stretchability);
					list.Add(item);
				}
				this.distanceJoints.AddGroup(list);
			}
		}

		// Token: 0x06003D63 RID: 15715 RVA: 0x0012A088 File Offset: 0x00128488
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
		}

		// Token: 0x04002F29 RID: 12073
		private readonly ClothSettings settings;

		// Token: 0x04002F2A RID: 12074
		private GroupedData<GPDistanceJoint> distanceJoints;

		// Token: 0x04002F2B RID: 12075
		private GpuBuffer<GPDistanceJoint> distanceJointsBuffer;
	}
}
