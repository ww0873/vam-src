using System;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003C5 RID: 965
	public class ObiEmitterMaterialGranular : ObiEmitterMaterial
	{
		// Token: 0x0600188E RID: 6286 RVA: 0x0008AEDE File Offset: 0x000892DE
		public ObiEmitterMaterialGranular()
		{
		}

		// Token: 0x0600188F RID: 6287 RVA: 0x0008AEE8 File Offset: 0x000892E8
		public void OnValidate()
		{
			this.resolution = Mathf.Max(0.001f, this.resolution);
			this.restDensity = Mathf.Max(0.001f, this.restDensity);
			this.randomness = Mathf.Max(0f, this.randomness);
		}

		// Token: 0x06001890 RID: 6288 RVA: 0x0008AF38 File Offset: 0x00089338
		public override Oni.FluidMaterial GetEquivalentOniMaterial(Oni.SolverParameters.Mode mode)
		{
			return new Oni.FluidMaterial
			{
				smoothingRadius = base.GetParticleSize(mode),
				restDensity = this.restDensity,
				viscosity = 0f,
				surfaceTension = 0f,
				buoyancy = -1f,
				atmosphericDrag = 0f,
				atmosphericPressure = 0f,
				vorticity = 0f
			};
		}

		// Token: 0x040013EF RID: 5103
		public float randomness;
	}
}
