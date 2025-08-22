using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003B2 RID: 946
	[DisallowMultipleComponent]
	public class ObiAerodynamicConstraints : ObiBatchedConstraints
	{
		// Token: 0x06001813 RID: 6163 RVA: 0x0008978C File Offset: 0x00087B8C
		public ObiAerodynamicConstraints()
		{
		}

		// Token: 0x06001814 RID: 6164 RVA: 0x000897C0 File Offset: 0x00087BC0
		public override Oni.ConstraintType GetConstraintType()
		{
			return Oni.ConstraintType.Aerodynamics;
		}

		// Token: 0x06001815 RID: 6165 RVA: 0x000897C4 File Offset: 0x00087BC4
		public override List<ObiConstraintBatch> GetBatches()
		{
			List<ObiAerodynamicConstraintBatch> list = this.batches;
			if (ObiAerodynamicConstraints.<>f__am$cache0 == null)
			{
				ObiAerodynamicConstraints.<>f__am$cache0 = new Converter<ObiAerodynamicConstraintBatch, ObiConstraintBatch>(ObiAerodynamicConstraints.<GetBatches>m__0);
			}
			return list.ConvertAll<ObiConstraintBatch>(ObiAerodynamicConstraints.<>f__am$cache0);
		}

		// Token: 0x06001816 RID: 6166 RVA: 0x000897F0 File Offset: 0x00087BF0
		public void OnValidate()
		{
			this.airDensity = Mathf.Max(0f, this.airDensity);
			this.dragCoefficient = Mathf.Max(0f, this.dragCoefficient);
			this.liftCoefficient = Mathf.Max(0f, this.liftCoefficient);
		}

		// Token: 0x06001817 RID: 6167 RVA: 0x0008983F File Offset: 0x00087C3F
		public override void Clear()
		{
			base.RemoveFromSolver(null);
			this.batches.Clear();
		}

		// Token: 0x06001818 RID: 6168 RVA: 0x00089854 File Offset: 0x00087C54
		public void AddBatch(ObiAerodynamicConstraintBatch batch)
		{
			if (batch != null && batch.GetConstraintType() == this.GetConstraintType())
			{
				this.batches.Add(batch);
			}
		}

		// Token: 0x06001819 RID: 6169 RVA: 0x00089879 File Offset: 0x00087C79
		public void RemoveBatch(ObiAerodynamicConstraintBatch batch)
		{
			this.batches.Remove(batch);
		}

		// Token: 0x0600181A RID: 6170 RVA: 0x00089888 File Offset: 0x00087C88
		public void OnDrawGizmosSelected()
		{
			if (!this.visualize)
			{
				return;
			}
			Gizmos.color = Color.blue;
			foreach (ObiAerodynamicConstraintBatch obiAerodynamicConstraintBatch in this.batches)
			{
				foreach (int index in obiAerodynamicConstraintBatch.ActiveConstraints)
				{
					Gizmos.DrawWireSphere(this.actor.GetParticlePosition(obiAerodynamicConstraintBatch.aerodynamicIndices[index]), 0.01f);
				}
			}
		}

		// Token: 0x0600181B RID: 6171 RVA: 0x00089958 File Offset: 0x00087D58
		[CompilerGenerated]
		private static ObiConstraintBatch <GetBatches>m__0(ObiAerodynamicConstraintBatch x)
		{
			return x;
		}

		// Token: 0x040013AA RID: 5034
		[Tooltip("Air density in kg/m3. Higher densities will make both drag and lift forces stronger.")]
		public float airDensity = 1.225f;

		// Token: 0x040013AB RID: 5035
		[Tooltip("How much is the cloth affected by drag forces. Extreme values can cause the cloth to behave unrealistically, so use with care.")]
		public float dragCoefficient = 0.05f;

		// Token: 0x040013AC RID: 5036
		[Tooltip("How much is the cloth affected by lift forces. Extreme values can cause the cloth to behave unrealistically, so use with care.")]
		public float liftCoefficient = 0.05f;

		// Token: 0x040013AD RID: 5037
		[SerializeField]
		[HideInInspector]
		private List<ObiAerodynamicConstraintBatch> batches = new List<ObiAerodynamicConstraintBatch>();

		// Token: 0x040013AE RID: 5038
		[CompilerGenerated]
		private static Converter<ObiAerodynamicConstraintBatch, ObiConstraintBatch> <>f__am$cache0;
	}
}
