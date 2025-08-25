using System;
using System.Collections.Generic;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Physics.Scripts.Types.Joints;
using GPUTools.Skinner.Scripts.Providers;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Runtime.Commands
{
	// Token: 0x0200099D RID: 2461
	public class BuildPointJoints : BuildChainCommand
	{
		// Token: 0x06003D77 RID: 15735 RVA: 0x0012A589 File Offset: 0x00128989
		public BuildPointJoints(ClothSettings settings)
		{
			this.settings = settings;
		}

		// Token: 0x06003D78 RID: 15736 RVA: 0x0012A598 File Offset: 0x00128998
		protected override void OnBuild()
		{
			this.CreatePointJoints();
			if (this.pointJoints.Count > 0)
			{
				this.pointJointsBuffer = new GpuBuffer<GPPointJoint>(this.pointJoints.ToArray(), GPPointJoint.Size());
				this.settings.Runtime.PointJoints = this.pointJointsBuffer;
			}
			if (this.allPointJoints.Count > 0)
			{
				this.allPointJointsBuffer = new GpuBuffer<GPPointJoint>(this.allPointJoints.ToArray(), GPPointJoint.Size());
				this.settings.Runtime.AllPointJoints = this.allPointJointsBuffer;
			}
		}

		// Token: 0x06003D79 RID: 15737 RVA: 0x0012A630 File Offset: 0x00128A30
		protected override void OnUpdateSettings()
		{
			this.CreatePointJoints();
			if (this.pointJointsBuffer != null)
			{
				this.pointJointsBuffer.Data = this.pointJoints.ToArray();
				this.pointJointsBuffer.PushData();
			}
			if (this.allPointJointsBuffer != null)
			{
				this.allPointJointsBuffer.Data = this.allPointJoints.ToArray();
				this.allPointJointsBuffer.PushData();
			}
		}

		// Token: 0x06003D7A RID: 15738 RVA: 0x0012A69C File Offset: 0x00128A9C
		private void CreatePointJoints()
		{
			Vector3[] particles = this.settings.GeometryData.Particles;
			int[] physicsToMeshVerticesMap = this.settings.GeometryData.PhysicsToMeshVerticesMap;
			float[] particlesBlend = this.settings.GeometryData.ParticlesBlend;
			this.pointJoints = new List<GPPointJoint>();
			this.allPointJoints = new List<GPPointJoint>();
			for (int i = 0; i < particles.Length; i++)
			{
				Vector3 point = particles[i];
				int num = physicsToMeshVerticesMap[i];
				float f = particlesBlend[num];
				int matrixId = (this.settings.MeshProvider.Type != ScalpMeshType.Static) ? num : 0;
				float rigidity = Mathf.Pow(f, 4f);
				this.pointJoints.Add(new GPPointJoint(i, matrixId, point, rigidity));
				this.allPointJoints.Add(new GPPointJoint(i, matrixId, point, rigidity));
			}
		}

		// Token: 0x06003D7B RID: 15739 RVA: 0x0012A77C File Offset: 0x00128B7C
		protected override void OnDispose()
		{
			if (this.settings.Runtime.PointJoints != null)
			{
				this.settings.Runtime.PointJoints.Dispose();
			}
			if (this.settings.Runtime.AllPointJoints != null)
			{
				this.settings.Runtime.AllPointJoints.Dispose();
			}
		}

		// Token: 0x04002F33 RID: 12083
		private readonly ClothSettings settings;

		// Token: 0x04002F34 RID: 12084
		private List<GPPointJoint> pointJoints;

		// Token: 0x04002F35 RID: 12085
		private List<GPPointJoint> allPointJoints;

		// Token: 0x04002F36 RID: 12086
		private GpuBuffer<GPPointJoint> pointJointsBuffer;

		// Token: 0x04002F37 RID: 12087
		private GpuBuffer<GPPointJoint> allPointJointsBuffer;
	}
}
