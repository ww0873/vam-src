using System;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003C1 RID: 961
	public abstract class ObiEmitterMaterial : ScriptableObject
	{
		// Token: 0x06001883 RID: 6275 RVA: 0x0008ACBC File Offset: 0x000890BC
		protected ObiEmitterMaterial()
		{
		}

		// Token: 0x14000080 RID: 128
		// (add) Token: 0x06001884 RID: 6276 RVA: 0x0008ACDA File Offset: 0x000890DA
		// (remove) Token: 0x06001885 RID: 6277 RVA: 0x0008AD0A File Offset: 0x0008910A
		public event EventHandler<ObiEmitterMaterial.MaterialChangeEventArgs> OnChangesMade
		{
			add
			{
				this.onChangesMade = (EventHandler<ObiEmitterMaterial.MaterialChangeEventArgs>)Delegate.Remove(this.onChangesMade, value);
				this.onChangesMade = (EventHandler<ObiEmitterMaterial.MaterialChangeEventArgs>)Delegate.Combine(this.onChangesMade, value);
			}
			remove
			{
				this.onChangesMade = (EventHandler<ObiEmitterMaterial.MaterialChangeEventArgs>)Delegate.Remove(this.onChangesMade, value);
			}
		}

		// Token: 0x06001886 RID: 6278 RVA: 0x0008AD23 File Offset: 0x00089123
		public void CommitChanges(ObiEmitterMaterial.MaterialChanges changes)
		{
			if (this.onChangesMade != null)
			{
				this.onChangesMade(this, new ObiEmitterMaterial.MaterialChangeEventArgs(changes));
			}
		}

		// Token: 0x06001887 RID: 6279 RVA: 0x0008AD42 File Offset: 0x00089142
		public float GetParticleSize(Oni.SolverParameters.Mode mode)
		{
			return 1f / (10f * Mathf.Pow(this.resolution, 1f / ((mode != Oni.SolverParameters.Mode.Mode3D) ? 2f : 3f)));
		}

		// Token: 0x06001888 RID: 6280 RVA: 0x0008AD76 File Offset: 0x00089176
		public float GetParticleMass(Oni.SolverParameters.Mode mode)
		{
			return this.restDensity * Mathf.Pow(this.GetParticleSize(mode), (float)((mode != Oni.SolverParameters.Mode.Mode3D) ? 2 : 3));
		}

		// Token: 0x06001889 RID: 6281
		public abstract Oni.FluidMaterial GetEquivalentOniMaterial(Oni.SolverParameters.Mode mode);

		// Token: 0x040013E1 RID: 5089
		public float resolution = 1f;

		// Token: 0x040013E2 RID: 5090
		public float restDensity = 1000f;

		// Token: 0x040013E3 RID: 5091
		private EventHandler<ObiEmitterMaterial.MaterialChangeEventArgs> onChangesMade;

		// Token: 0x020003C2 RID: 962
		public class MaterialChangeEventArgs : EventArgs
		{
			// Token: 0x0600188A RID: 6282 RVA: 0x0008AD99 File Offset: 0x00089199
			public MaterialChangeEventArgs(ObiEmitterMaterial.MaterialChanges changes)
			{
				this.changes = changes;
			}

			// Token: 0x040013E4 RID: 5092
			public ObiEmitterMaterial.MaterialChanges changes;
		}

		// Token: 0x020003C3 RID: 963
		[Flags]
		public enum MaterialChanges
		{
			// Token: 0x040013E6 RID: 5094
			PER_MATERIAL_DATA = 0,
			// Token: 0x040013E7 RID: 5095
			PER_PARTICLE_DATA = 1
		}
	}
}
