using System;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003EF RID: 1007
	public class ObiSphericalForceZone : ObiExternalForce
	{
		// Token: 0x06001987 RID: 6535 RVA: 0x0008D2C8 File Offset: 0x0008B6C8
		public ObiSphericalForceZone()
		{
		}

		// Token: 0x06001988 RID: 6536 RVA: 0x0008D2E4 File Offset: 0x0008B6E4
		public void OnEnable()
		{
			foreach (ObiSolver obiSolver in this.affectedSolvers)
			{
				obiSolver.RequireRenderablePositions();
			}
		}

		// Token: 0x06001989 RID: 6537 RVA: 0x0008D318 File Offset: 0x0008B718
		public void OnDisable()
		{
			foreach (ObiSolver obiSolver in this.affectedSolvers)
			{
				obiSolver.RelinquishRenderablePositions();
			}
		}

		// Token: 0x0600198A RID: 6538 RVA: 0x0008D34C File Offset: 0x0008B74C
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
			Vector4 a = matrix4x.MultiplyVector(Vector3.forward * (this.intensity + base.GetTurbulence(this.turbulence)));
			float num = this.radius * this.radius;
			Vector4[] array = new Vector4[actor.particleIndices.Length];
			Vector4 b = new Vector4(base.transform.position.x, base.transform.position.y, base.transform.position.z);
			for (int i = 0; i < array.Length; i++)
			{
				Vector4 a2 = actor.Solver.renderablePositions[actor.particleIndices[i]] - b;
				float sqrMagnitude = a2.sqrMagnitude;
				float d = Mathf.Clamp01((num - sqrMagnitude) / num);
				if (this.radial)
				{
					array[i] = a2 / (Mathf.Sqrt(sqrMagnitude) + float.Epsilon) * d * this.intensity;
				}
				else
				{
					array[i] = a * d;
				}
				array[i][3] = (float)((!actor.UsesCustomExternalForces) ? 0 : 1);
			}
			Oni.AddParticleExternalForces(actor.Solver.OniSolver, array, actor.particleIndices, actor.particleIndices.Length);
		}

		// Token: 0x0600198B RID: 6539 RVA: 0x0008D514 File Offset: 0x0008B914
		public void OnDrawGizmosSelected()
		{
			Gizmos.matrix = base.transform.localToWorldMatrix;
			Gizmos.color = new Color(0f, 0.7f, 1f, 1f);
			Gizmos.DrawWireSphere(Vector3.zero, this.radius);
			float turbulence = base.GetTurbulence(1f);
			if (!this.radial)
			{
				ObiUtils.DrawArrowGizmo(this.radius + turbulence, this.radius * 0.2f, this.radius * 0.3f, this.radius * 0.2f);
			}
			else
			{
				Gizmos.DrawLine(new Vector3(0f, 0f, -this.radius * 0.5f) * turbulence, new Vector3(0f, 0f, this.radius * 0.5f) * turbulence);
				Gizmos.DrawLine(new Vector3(0f, -this.radius * 0.5f, 0f) * turbulence, new Vector3(0f, this.radius * 0.5f, 0f) * turbulence);
				Gizmos.DrawLine(new Vector3(-this.radius * 0.5f, 0f, 0f) * turbulence, new Vector3(this.radius * 0.5f, 0f, 0f) * turbulence);
			}
		}

		// Token: 0x040014DA RID: 5338
		public float radius = 5f;

		// Token: 0x040014DB RID: 5339
		public bool radial = true;
	}
}
