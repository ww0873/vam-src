using System;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003C4 RID: 964
	public class ObiEmitterMaterialFluid : ObiEmitterMaterial
	{
		// Token: 0x0600188B RID: 6283 RVA: 0x0008ADA8 File Offset: 0x000891A8
		public ObiEmitterMaterialFluid()
		{
		}

		// Token: 0x0600188C RID: 6284 RVA: 0x0008ADDC File Offset: 0x000891DC
		public void OnValidate()
		{
			this.resolution = Mathf.Max(0.001f, this.resolution);
			this.restDensity = Mathf.Max(0.001f, this.restDensity);
			this.smoothing = Mathf.Max(1f, this.smoothing);
			this.viscosity = Mathf.Max(0f, this.viscosity);
			this.atmosphericDrag = Mathf.Max(0f, this.atmosphericDrag);
		}

		// Token: 0x0600188D RID: 6285 RVA: 0x0008AE58 File Offset: 0x00089258
		public override Oni.FluidMaterial GetEquivalentOniMaterial(Oni.SolverParameters.Mode mode)
		{
			return new Oni.FluidMaterial
			{
				smoothingRadius = base.GetParticleSize(mode) * this.smoothing,
				restDensity = this.restDensity,
				viscosity = this.viscosity,
				surfaceTension = this.surfaceTension,
				buoyancy = this.buoyancy,
				atmosphericDrag = this.atmosphericDrag,
				atmosphericPressure = this.atmosphericPressure,
				vorticity = this.vorticity
			};
		}

		// Token: 0x040013E8 RID: 5096
		public float smoothing = 1.5f;

		// Token: 0x040013E9 RID: 5097
		public float viscosity = 0.05f;

		// Token: 0x040013EA RID: 5098
		public float surfaceTension = 0.1f;

		// Token: 0x040013EB RID: 5099
		public float buoyancy = -1f;

		// Token: 0x040013EC RID: 5100
		public float atmosphericDrag;

		// Token: 0x040013ED RID: 5101
		public float atmosphericPressure;

		// Token: 0x040013EE RID: 5102
		public float vorticity;
	}
}
