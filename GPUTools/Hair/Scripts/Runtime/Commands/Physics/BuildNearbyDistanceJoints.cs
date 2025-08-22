using System;
using System.Collections.Generic;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Hair.Scripts.Types;
using GPUTools.Physics.Scripts.Types.Joints;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Commands.Physics
{
	// Token: 0x02000A10 RID: 2576
	public class BuildNearbyDistanceJoints : BuildChainCommand
	{
		// Token: 0x06004151 RID: 16721 RVA: 0x00136648 File Offset: 0x00134A48
		public BuildNearbyDistanceJoints(HairSettings settings)
		{
			this.settings = settings;
		}

		// Token: 0x06004152 RID: 16722 RVA: 0x00136658 File Offset: 0x00134A58
		protected override void OnBuild()
		{
			this.CreateNearbyDistanceJoints();
			if (this.nearbyDistanceJointsBuffer != null)
			{
				this.nearbyDistanceJointsBuffer.Dispose();
			}
			if (this.nearbyDistanceJoints.Data.Length > 0)
			{
				this.nearbyDistanceJointsBuffer = new GpuBuffer<GPDistanceJoint>(this.nearbyDistanceJoints.Data, GPDistanceJoint.Size());
			}
			else
			{
				this.nearbyDistanceJointsBuffer = null;
			}
			this.settings.RuntimeData.NearbyDistanceJointsBuffer = this.nearbyDistanceJointsBuffer;
			this.settings.RuntimeData.NearbyDistanceJoints = this.nearbyDistanceJoints;
		}

		// Token: 0x06004153 RID: 16723 RVA: 0x001366E7 File Offset: 0x00134AE7
		private void AddToHashSet(HashSet<Vector3> set, int i1, int i2, float distance)
		{
			if (i1 == -1 || i2 == -1)
			{
				return;
			}
			set.Add((i1 <= i2) ? new Vector3((float)i2, (float)i1, distance) : new Vector3((float)i1, (float)i2, distance));
		}

		// Token: 0x06004154 RID: 16724 RVA: 0x00136724 File Offset: 0x00134B24
		private void CreateNearbyDistanceJoints()
		{
			this.nearbyDistanceJoints = new GroupedData<GPDistanceJoint>();
			int segments = this.settings.StandsSettings.Segments;
			float num = (float)(segments - 1);
			int num2 = 0;
			List<Vector4ListContainer> nearbyVertexGroups = this.settings.StandsSettings.Provider.GetNearbyVertexGroups();
			if (nearbyVertexGroups != null)
			{
				foreach (Vector4ListContainer vector4ListContainer in nearbyVertexGroups)
				{
					List<GPDistanceJoint> list = new List<GPDistanceJoint>();
					this.nearbyDistanceJoints.AddGroup(list);
					foreach (Vector4 vector in vector4ListContainer.List)
					{
						int num3 = (int)vector.x;
						float num4 = 1f - (float)(num3 % segments) / num;
						int num5 = (int)vector.y;
						float num6 = 1f - (float)(num5 % segments) / num;
						float elasticity = (num4 + num6) * 0.5f;
						GPDistanceJoint item = new GPDistanceJoint(num3, num5, vector.z, elasticity);
						list.Add(item);
						num2++;
					}
				}
			}
		}

		// Token: 0x06004155 RID: 16725 RVA: 0x00136870 File Offset: 0x00134C70
		protected override void OnDispose()
		{
			if (this.nearbyDistanceJoints != null)
			{
				this.nearbyDistanceJoints.Dispose();
			}
			if (this.nearbyDistanceJointsBuffer != null)
			{
				this.nearbyDistanceJointsBuffer.Dispose();
			}
		}

		// Token: 0x04003106 RID: 12550
		private readonly HairSettings settings;

		// Token: 0x04003107 RID: 12551
		private GroupedData<GPDistanceJoint> nearbyDistanceJoints;

		// Token: 0x04003108 RID: 12552
		private GpuBuffer<GPDistanceJoint> nearbyDistanceJointsBuffer;
	}
}
