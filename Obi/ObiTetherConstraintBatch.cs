using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003B0 RID: 944
	[Serializable]
	public class ObiTetherConstraintBatch : ObiConstraintBatch
	{
		// Token: 0x060017FD RID: 6141 RVA: 0x00088BA5 File Offset: 0x00086FA5
		public ObiTetherConstraintBatch(bool cooked, bool sharesParticles) : base(cooked, sharesParticles)
		{
		}

		// Token: 0x060017FE RID: 6142 RVA: 0x00088BDC File Offset: 0x00086FDC
		public ObiTetherConstraintBatch(bool cooked, bool sharesParticles, float minYoungModulus, float maxYoungModulus) : base(cooked, sharesParticles, minYoungModulus, maxYoungModulus)
		{
		}

		// Token: 0x060017FF RID: 6143 RVA: 0x00088C16 File Offset: 0x00087016
		public override Oni.ConstraintType GetConstraintType()
		{
			return Oni.ConstraintType.Tether;
		}

		// Token: 0x06001800 RID: 6144 RVA: 0x00088C19 File Offset: 0x00087019
		public override void Clear()
		{
			this.activeConstraints.Clear();
			this.tetherIndices.Clear();
			this.maxLengthsScales.Clear();
			this.stiffnesses.Clear();
			this.constraintCount = 0;
		}

		// Token: 0x06001801 RID: 6145 RVA: 0x00088C50 File Offset: 0x00087050
		public void AddConstraint(int index1, int index2, float maxLength, float scale, float stiffness)
		{
			this.activeConstraints.Add(this.constraintCount);
			this.tetherIndices.Add(index1);
			this.tetherIndices.Add(index2);
			this.maxLengthsScales.Add(new Vector2(maxLength, scale));
			this.stiffnesses.Add(stiffness);
			this.constraintCount++;
		}

		// Token: 0x06001802 RID: 6146 RVA: 0x00088CB4 File Offset: 0x000870B4
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
			this.tetherIndices.RemoveRange(index * 2, 2);
			this.maxLengthsScales.RemoveAt(index);
			this.stiffnesses.RemoveAt(index);
			this.constraintCount--;
		}

		// Token: 0x06001803 RID: 6147 RVA: 0x00088D60 File Offset: 0x00087160
		public override List<int> GetConstraintsInvolvingParticle(int particleIndex)
		{
			List<int> list = new List<int>(4);
			for (int i = 0; i < base.ConstraintCount; i++)
			{
				if (this.tetherIndices[i * 2] == particleIndex || this.tetherIndices[i * 2 + 1] == particleIndex)
				{
					list.Add(i);
				}
			}
			return list;
		}

		// Token: 0x06001804 RID: 6148 RVA: 0x00088DC0 File Offset: 0x000871C0
		public override void Cook()
		{
			this.batch = Oni.CreateBatch(0, true);
			Oni.SetTetherConstraints(this.batch, this.tetherIndices.ToArray(), this.maxLengthsScales.ToArray(), this.stiffnesses.ToArray(), base.ConstraintCount);
			if (Oni.CookBatch(this.batch))
			{
				this.constraintCount = Oni.GetBatchConstraintCount(this.batch);
				this.activeConstraints = Enumerable.Range(0, this.constraintCount).ToList<int>();
				int[] array = new int[this.constraintCount * 2];
				Vector2[] array2 = new Vector2[this.constraintCount];
				float[] collection = new float[this.constraintCount];
				Oni.GetTetherConstraints(this.batch, array, array2, collection);
				this.tetherIndices = new List<int>(array);
				this.maxLengthsScales = new List<Vector2>(array2);
				this.stiffnesses = new List<float>(collection);
				int batchPhaseCount = Oni.GetBatchPhaseCount(this.batch);
				int[] array3 = new int[batchPhaseCount];
				Oni.GetBatchPhaseSizes(this.batch, array3);
				this.phaseSizes = new List<int>(array3);
			}
			Oni.DestroyBatch(this.batch);
			this.batch = IntPtr.Zero;
		}

		// Token: 0x06001805 RID: 6149 RVA: 0x00088EE0 File Offset: 0x000872E0
		protected override void OnAddToSolver(ObiBatchedConstraints constraints)
		{
			this.solverIndices = new int[this.tetherIndices.Count];
			for (int i = 0; i < base.ConstraintCount; i++)
			{
				this.solverIndices[i * 2] = constraints.Actor.particleIndices[this.tetherIndices[i * 2]];
				this.solverIndices[i * 2 + 1] = constraints.Actor.particleIndices[this.tetherIndices[i * 2 + 1]];
			}
		}

		// Token: 0x06001806 RID: 6150 RVA: 0x00088F66 File Offset: 0x00087366
		protected override void OnRemoveFromSolver(ObiBatchedConstraints constraints)
		{
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x00088F68 File Offset: 0x00087368
		public override void PushDataToSolver(ObiBatchedConstraints constraints)
		{
			if (constraints == null || constraints.Actor == null || !constraints.Actor.InSolver)
			{
				return;
			}
			ObiTetherConstraints obiTetherConstraints = (ObiTetherConstraints)constraints;
			for (int i = 0; i < base.ConstraintCount; i++)
			{
				this.maxLengthsScales[i] = new Vector2(this.maxLengthsScales[i].x, obiTetherConstraints.tetherScale);
				this.stiffnesses[i] = base.StiffnessToCompliance(obiTetherConstraints.stiffness);
			}
			Oni.SetTetherConstraints(this.batch, this.solverIndices, this.maxLengthsScales.ToArray(), this.stiffnesses.ToArray(), base.ConstraintCount);
			Oni.SetBatchPhaseSizes(this.batch, this.phaseSizes.ToArray(), this.phaseSizes.Count);
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x00089052 File Offset: 0x00087452
		public override void PullDataFromSolver(ObiBatchedConstraints constraints)
		{
		}

		// Token: 0x040013A0 RID: 5024
		[HideInInspector]
		public List<int> tetherIndices = new List<int>();

		// Token: 0x040013A1 RID: 5025
		[HideInInspector]
		public List<Vector2> maxLengthsScales = new List<Vector2>();

		// Token: 0x040013A2 RID: 5026
		[HideInInspector]
		public List<float> stiffnesses = new List<float>();

		// Token: 0x040013A3 RID: 5027
		private int[] solverIndices = new int[0];
	}
}
