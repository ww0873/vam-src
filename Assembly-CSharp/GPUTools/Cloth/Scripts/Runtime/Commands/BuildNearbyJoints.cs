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
	// Token: 0x0200099A RID: 2458
	public class BuildNearbyJoints : BuildChainCommand
	{
		// Token: 0x06003D6A RID: 15722 RVA: 0x0012A130 File Offset: 0x00128530
		public BuildNearbyJoints(ClothSettings settings)
		{
			this.settings = settings;
		}

		// Token: 0x06003D6B RID: 15723 RVA: 0x0012A140 File Offset: 0x00128540
		protected override void OnBuild()
		{
			this.CreateNearbyJoints();
			this.settings.Runtime.NearbyJoints = this.nearbyJoints;
			if (this.nearbyJoints.Data.Length > 0)
			{
				this.nearbyJointsBuffer = new GpuBuffer<GPDistanceJoint>(this.nearbyJoints.Data, GPDistanceJoint.Size());
				this.settings.Runtime.NearbyJointsBuffer = this.nearbyJointsBuffer;
			}
			else
			{
				this.settings.Runtime.NearbyJointsBuffer = null;
			}
		}

		// Token: 0x06003D6C RID: 15724 RVA: 0x0012A1C3 File Offset: 0x001285C3
		protected override void OnUpdateSettings()
		{
			this.CreateNearbyJoints();
			this.settings.Runtime.NearbyJoints = this.nearbyJoints;
			this.nearbyJointsBuffer.Data = this.nearbyJoints.Data;
			this.nearbyJointsBuffer.PushData();
		}

		// Token: 0x06003D6D RID: 15725 RVA: 0x0012A204 File Offset: 0x00128604
		private void CreateNearbyJoints()
		{
			this.nearbyJoints = new GroupedData<GPDistanceJoint>();
			List<Int2ListContainer> nearbyJointGroups = this.settings.GeometryData.NearbyJointGroups;
			GPParticle[] data = this.settings.Runtime.Particles.Data;
			foreach (Int2ListContainer int2ListContainer in nearbyJointGroups)
			{
				List<GPDistanceJoint> list = new List<GPDistanceJoint>();
				foreach (Int2 @int in int2ListContainer.List)
				{
					GPParticle gpparticle = data[@int.X];
					GPParticle gpparticle2 = data[@int.Y];
					float distance = Vector3.Distance(gpparticle.Position, gpparticle2.Position) / this.settings.transform.lossyScale.x;
					GPDistanceJoint item = new GPDistanceJoint(@int.X, @int.Y, distance, this.settings.Stiffness);
					list.Add(item);
				}
				this.nearbyJoints.AddGroup(list);
			}
		}

		// Token: 0x06003D6E RID: 15726 RVA: 0x0012A364 File Offset: 0x00128764
		protected override void OnDispose()
		{
			if (this.nearbyJoints != null)
			{
				this.nearbyJoints.Dispose();
			}
			if (this.nearbyJointsBuffer != null)
			{
				this.nearbyJointsBuffer.Dispose();
			}
		}

		// Token: 0x04002F2E RID: 12078
		private readonly ClothSettings settings;

		// Token: 0x04002F2F RID: 12079
		private GroupedData<GPDistanceJoint> nearbyJoints;

		// Token: 0x04002F30 RID: 12080
		private GpuBuffer<GPDistanceJoint> nearbyJointsBuffer;
	}
}
