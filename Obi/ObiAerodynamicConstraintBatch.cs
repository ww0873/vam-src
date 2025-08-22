using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003A8 RID: 936
	[Serializable]
	public class ObiAerodynamicConstraintBatch : ObiConstraintBatch
	{
		// Token: 0x060017A6 RID: 6054 RVA: 0x00087232 File Offset: 0x00085632
		public ObiAerodynamicConstraintBatch(bool cooked, bool sharesParticles) : base(cooked, sharesParticles)
		{
		}

		// Token: 0x060017A7 RID: 6055 RVA: 0x0008725E File Offset: 0x0008565E
		public override Oni.ConstraintType GetConstraintType()
		{
			return Oni.ConstraintType.Aerodynamics;
		}

		// Token: 0x060017A8 RID: 6056 RVA: 0x00087262 File Offset: 0x00085662
		public override void Clear()
		{
			this.activeConstraints.Clear();
			this.aerodynamicIndices.Clear();
			this.aerodynamicCoeffs.Clear();
			this.constraintCount = 0;
		}

		// Token: 0x060017A9 RID: 6057 RVA: 0x0008728C File Offset: 0x0008568C
		public void AddConstraint(int index, float area, float drag, float lift)
		{
			this.activeConstraints.Add(this.constraintCount);
			this.aerodynamicIndices.Add(index);
			this.aerodynamicCoeffs.Add(area);
			this.aerodynamicCoeffs.Add(drag);
			this.aerodynamicCoeffs.Add(lift);
			this.constraintCount++;
		}

		// Token: 0x060017AA RID: 6058 RVA: 0x000872EC File Offset: 0x000856EC
		public void RemoveConstraint(int index)
		{
			if (index < 0 || index >= base.ConstraintCount)
			{
				return;
			}
			this.activeConstraints.Remove(index);
			for (int i = 0; i < this.activeConstraints.Count; i++)
			{
				if (this.activeConstraints[i] > index)
				{
					List<int> activeConstraints;
					int index2;
					(activeConstraints = this.activeConstraints)[index2 = i] = activeConstraints[index2] - 1;
				}
			}
			this.aerodynamicIndices.RemoveAt(index);
			this.aerodynamicCoeffs.RemoveRange(index * 3, 3);
			this.constraintCount--;
		}

		// Token: 0x060017AB RID: 6059 RVA: 0x0008738C File Offset: 0x0008578C
		public override List<int> GetConstraintsInvolvingParticle(int particleIndex)
		{
			List<int> list = new List<int>(1);
			for (int i = 0; i < base.ConstraintCount; i++)
			{
				if (this.aerodynamicIndices[i] == particleIndex)
				{
					list.Add(i);
				}
			}
			return list;
		}

		// Token: 0x060017AC RID: 6060 RVA: 0x000873D4 File Offset: 0x000857D4
		protected override void OnAddToSolver(ObiBatchedConstraints constraints)
		{
			this.solverIndices = new int[this.aerodynamicIndices.Count];
			for (int i = 0; i < this.aerodynamicIndices.Count; i++)
			{
				this.solverIndices[i] = constraints.Actor.particleIndices[this.aerodynamicIndices[i]];
			}
		}

		// Token: 0x060017AD RID: 6061 RVA: 0x00087433 File Offset: 0x00085833
		protected override void OnRemoveFromSolver(ObiBatchedConstraints constraints)
		{
		}

		// Token: 0x060017AE RID: 6062 RVA: 0x00087438 File Offset: 0x00085838
		public override void PushDataToSolver(ObiBatchedConstraints constraints)
		{
			if (constraints == null || constraints.Actor == null || !constraints.Actor.InSolver)
			{
				return;
			}
			ObiAerodynamicConstraints obiAerodynamicConstraints = (ObiAerodynamicConstraints)constraints;
			for (int i = 0; i < this.aerodynamicCoeffs.Count; i += 3)
			{
				this.aerodynamicCoeffs[i + 1] = obiAerodynamicConstraints.dragCoefficient * obiAerodynamicConstraints.airDensity;
				this.aerodynamicCoeffs[i + 2] = obiAerodynamicConstraints.liftCoefficient * obiAerodynamicConstraints.airDensity;
			}
			Oni.SetAerodynamicConstraints(this.batch, this.solverIndices, this.aerodynamicCoeffs.ToArray(), base.ConstraintCount);
		}

		// Token: 0x060017AF RID: 6063 RVA: 0x000874EE File Offset: 0x000858EE
		public override void PullDataFromSolver(ObiBatchedConstraints constraints)
		{
		}

		// Token: 0x04001379 RID: 4985
		[HideInInspector]
		public List<int> aerodynamicIndices = new List<int>();

		// Token: 0x0400137A RID: 4986
		[HideInInspector]
		public List<float> aerodynamicCoeffs = new List<float>();

		// Token: 0x0400137B RID: 4987
		private int[] solverIndices = new int[0];
	}
}
