using System;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Physics.Scripts.Types.Dynamic;
using GPUTools.Physics.Scripts.Types.Joints;
using UnityEngine;

namespace GPUTools.Physics.Scripts.DebugDraw
{
	// Token: 0x02000A48 RID: 2632
	public class GPDebugDraw
	{
		// Token: 0x060043BE RID: 17342 RVA: 0x0013D3CC File Offset: 0x0013B7CC
		public GPDebugDraw()
		{
		}

		// Token: 0x060043BF RID: 17343 RVA: 0x0013D3D4 File Offset: 0x0013B7D4
		public static void Draw(GpuBuffer<GPDistanceJoint> joints, GpuBuffer<GPDistanceJoint> joints2, GpuBuffer<GPParticle> particles, bool drawParticles, bool drawJoints, bool drawJoints2)
		{
			particles.PullData();
			Gizmos.color = Color.green;
			if (drawParticles)
			{
				foreach (GPParticle gpparticle in particles.Data)
				{
					Gizmos.DrawWireSphere(gpparticle.Position, gpparticle.Radius);
				}
			}
			if (drawJoints && joints != null)
			{
				joints.PullData();
				foreach (GPDistanceJoint gpdistanceJoint in joints.Data)
				{
					GPParticle gpparticle2 = particles.Data[gpdistanceJoint.Body1Id];
					GPParticle gpparticle3 = particles.Data[gpdistanceJoint.Body2Id];
					Gizmos.DrawLine(gpparticle2.Position, gpparticle3.Position);
				}
			}
			Gizmos.color = Color.yellow;
			if (drawJoints2 && joints2 != null)
			{
				joints2.PullData();
				foreach (GPDistanceJoint gpdistanceJoint2 in joints2.Data)
				{
					GPParticle gpparticle4 = particles.Data[gpdistanceJoint2.Body1Id];
					GPParticle gpparticle5 = particles.Data[gpdistanceJoint2.Body2Id];
					Gizmos.DrawLine(gpparticle4.Position, gpparticle5.Position);
				}
			}
		}
	}
}
