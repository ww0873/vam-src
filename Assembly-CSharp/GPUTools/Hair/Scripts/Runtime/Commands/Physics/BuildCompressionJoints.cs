using System;
using System.Collections.Generic;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Joints;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Commands.Physics
{
	// Token: 0x02000A0C RID: 2572
	public class BuildCompressionJoints : BuildChainCommand
	{
		// Token: 0x06004140 RID: 16704 RVA: 0x00135E39 File Offset: 0x00134239
		public BuildCompressionJoints(HairSettings settings)
		{
			this.settings = settings;
		}

		// Token: 0x06004141 RID: 16705 RVA: 0x00135E48 File Offset: 0x00134248
		protected override void OnBuild()
		{
			this.CreateCompressionJoints();
			if (this.compressionJointsBuffer != null)
			{
				this.compressionJointsBuffer.Dispose();
			}
			if (this.compressionJoints.Data.Length > 0)
			{
				this.compressionJointsBuffer = new GpuBuffer<GPDistanceJoint>(this.compressionJoints.Data, GPDistanceJoint.Size());
			}
			else
			{
				this.compressionJointsBuffer = null;
			}
			this.settings.RuntimeData.CompressionJoints = this.compressionJoints;
			this.settings.RuntimeData.CompressionJointsBuffer = this.compressionJointsBuffer;
		}

		// Token: 0x06004142 RID: 16706 RVA: 0x00135ED8 File Offset: 0x001342D8
		protected override void OnUpdateSettings()
		{
			this.CreateCompressionJoints();
			this.settings.RuntimeData.CompressionJoints = this.compressionJoints;
			if (this.compressionJointsBuffer != null)
			{
				this.compressionJointsBuffer.Data = this.compressionJoints.Data;
				this.compressionJointsBuffer.PushData();
			}
		}

		// Token: 0x06004143 RID: 16707 RVA: 0x00135F30 File Offset: 0x00134330
		private void CreateCompressionJoints()
		{
			int segments = this.settings.StandsSettings.Segments;
			this.compressionJoints = new GroupedData<GPDistanceJoint>();
			List<GPDistanceJoint> list = new List<GPDistanceJoint>();
			List<GPDistanceJoint> list2 = new List<GPDistanceJoint>();
			if (this.settings.RuntimeData.Particles != null)
			{
				for (int i = 0; i < this.settings.RuntimeData.Particles.Count; i++)
				{
					if (i % segments != 0 && i % segments != 1)
					{
						GPParticle gpparticle = this.settings.RuntimeData.Particles.Data[i - 2];
						GPParticle gpparticle2 = this.settings.RuntimeData.Particles.Data[i];
						float distance = Vector3.Distance(gpparticle.Position, gpparticle2.Position) * 0.95f;
						List<GPDistanceJoint> list3 = (i % 4 != 2 && i % 4 != 3) ? list2 : list;
						GPDistanceJoint item = new GPDistanceJoint(i - 2, i, distance, 1f);
						list3.Add(item);
					}
				}
			}
			this.compressionJoints.AddGroup(list);
			this.compressionJoints.AddGroup(list2);
		}

		// Token: 0x06004144 RID: 16708 RVA: 0x00136065 File Offset: 0x00134465
		protected override void OnDispose()
		{
			if (this.compressionJoints != null)
			{
				this.compressionJoints.Dispose();
			}
			if (this.compressionJointsBuffer != null)
			{
				this.compressionJointsBuffer.Dispose();
			}
		}

		// Token: 0x040030FD RID: 12541
		private readonly HairSettings settings;

		// Token: 0x040030FE RID: 12542
		private GroupedData<GPDistanceJoint> compressionJoints;

		// Token: 0x040030FF RID: 12543
		private GpuBuffer<GPDistanceJoint> compressionJointsBuffer;
	}
}
