using System;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003EE RID: 1006
	public abstract class ObiExternalForce : MonoBehaviour
	{
		// Token: 0x06001983 RID: 6531 RVA: 0x0008D0E9 File Offset: 0x0008B4E9
		protected ObiExternalForce()
		{
		}

		// Token: 0x06001984 RID: 6532 RVA: 0x0008D0FC File Offset: 0x0008B4FC
		public void LateUpdate()
		{
			foreach (ObiSolver obiSolver in this.affectedSolvers)
			{
				if (obiSolver != null)
				{
					foreach (ObiActor obiActor in obiSolver.actors)
					{
						if (obiActor != null)
						{
							this.ApplyForcesToActor(obiActor);
						}
					}
				}
			}
		}

		// Token: 0x06001985 RID: 6533 RVA: 0x0008D190 File Offset: 0x0008B590
		protected float GetTurbulence(float turbulenceIntensity)
		{
			return Mathf.PerlinNoise(Time.fixedTime * this.turbulenceFrequency, this.turbulenceSeed) * turbulenceIntensity;
		}

		// Token: 0x06001986 RID: 6534
		public abstract void ApplyForcesToActor(ObiActor actor);

		// Token: 0x040014D5 RID: 5333
		public float intensity;

		// Token: 0x040014D6 RID: 5334
		public float turbulence;

		// Token: 0x040014D7 RID: 5335
		public float turbulenceFrequency = 1f;

		// Token: 0x040014D8 RID: 5336
		public float turbulenceSeed;

		// Token: 0x040014D9 RID: 5337
		public ObiSolver[] affectedSolvers;
	}
}
