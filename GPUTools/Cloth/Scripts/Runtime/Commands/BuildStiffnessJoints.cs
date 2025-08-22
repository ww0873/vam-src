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
	// Token: 0x0200099F RID: 2463
	public class BuildStiffnessJoints : BuildChainCommand
	{
		// Token: 0x06003D7F RID: 15743 RVA: 0x0012A81A File Offset: 0x00128C1A
		public BuildStiffnessJoints(ClothSettings settings)
		{
			this.settings = settings;
		}

		// Token: 0x06003D80 RID: 15744 RVA: 0x0012A82C File Offset: 0x00128C2C
		protected override void OnBuild()
		{
			this.CreateStiffnessJoints();
			this.stiffnessJointsBuffer = new GpuBuffer<GPDistanceJoint>(this.stiffnessJoints.Data, GPDistanceJoint.Size());
			this.settings.Runtime.StiffnessJoints = this.stiffnessJoints;
			this.settings.Runtime.StiffnessJointsBuffer = this.stiffnessJointsBuffer;
		}

		// Token: 0x06003D81 RID: 15745 RVA: 0x0012A886 File Offset: 0x00128C86
		protected override void OnUpdateSettings()
		{
			this.CreateStiffnessJoints();
			this.settings.Runtime.StiffnessJoints = this.stiffnessJoints;
			this.stiffnessJointsBuffer.Data = this.stiffnessJoints.Data;
			this.stiffnessJointsBuffer.PushData();
		}

		// Token: 0x06003D82 RID: 15746 RVA: 0x0012A8C8 File Offset: 0x00128CC8
		private void CreateStiffnessJoints()
		{
			this.stiffnessJoints = new GroupedData<GPDistanceJoint>();
			List<Int2ListContainer> stiffnessJointGroups = this.settings.GeometryData.StiffnessJointGroups;
			GPParticle[] data = this.settings.Runtime.Particles.Data;
			foreach (Int2ListContainer int2ListContainer in stiffnessJointGroups)
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
				this.stiffnessJoints.AddGroup(list);
			}
		}

		// Token: 0x06003D83 RID: 15747 RVA: 0x0012AA28 File Offset: 0x00128E28
		protected override void OnDispose()
		{
			if (this.stiffnessJoints != null)
			{
				this.stiffnessJoints.Dispose();
			}
			if (this.stiffnessJointsBuffer != null)
			{
				this.stiffnessJointsBuffer.Dispose();
			}
		}

		// Token: 0x04002F39 RID: 12089
		private readonly ClothSettings settings;

		// Token: 0x04002F3A RID: 12090
		private GroupedData<GPDistanceJoint> stiffnessJoints;

		// Token: 0x04002F3B RID: 12091
		private GpuBuffer<GPDistanceJoint> stiffnessJointsBuffer;
	}
}
