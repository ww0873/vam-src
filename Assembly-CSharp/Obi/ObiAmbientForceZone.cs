using System;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003ED RID: 1005
	public class ObiAmbientForceZone : ObiExternalForce
	{
		// Token: 0x06001980 RID: 6528 RVA: 0x0008D1AB File Offset: 0x0008B5AB
		public ObiAmbientForceZone()
		{
		}

		// Token: 0x06001981 RID: 6529 RVA: 0x0008D1B4 File Offset: 0x0008B5B4
		public override void ApplyForcesToActor(ObiActor actor)
		{
			Matrix4x4 matrix4x;
			if (actor.Solver.simulateInLocalSpace)
			{
				matrix4x = actor.Solver.transform.worldToLocalMatrix * base.transform.localToWorldMatrix;
			}
			else
			{
				matrix4x = base.transform.localToWorldMatrix;
			}
			Vector4 vector = matrix4x.MultiplyVector(Vector3.forward * (this.intensity + base.GetTurbulence(this.turbulence)));
			vector[3] = (float)((!actor.UsesCustomExternalForces) ? 0 : 1);
			Oni.AddParticleExternalForce(actor.Solver.OniSolver, ref vector, actor.particleIndices, actor.particleIndices.Length);
		}

		// Token: 0x06001982 RID: 6530 RVA: 0x0008D268 File Offset: 0x0008B668
		public void OnDrawGizmosSelected()
		{
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.color = new Color(0f, 0.7f, 1f, 1f);
			ObiUtils.DrawArrowGizmo(0.5f + base.GetTurbulence(1f), 0.2f, 0.3f, 0.2f);
		}
	}
}
