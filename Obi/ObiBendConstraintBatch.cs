using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003A9 RID: 937
	[Serializable]
	public class ObiBendConstraintBatch : ObiConstraintBatch
	{
		// Token: 0x060017B0 RID: 6064 RVA: 0x000874F0 File Offset: 0x000858F0
		public ObiBendConstraintBatch(bool cooked, bool sharesParticles) : base(cooked, sharesParticles)
		{
		}

		// Token: 0x060017B1 RID: 6065 RVA: 0x00087527 File Offset: 0x00085927
		public ObiBendConstraintBatch(bool cooked, bool sharesParticles, float minYoungModulus, float maxYoungModulus) : base(cooked, sharesParticles, minYoungModulus, maxYoungModulus)
		{
		}

		// Token: 0x060017B2 RID: 6066 RVA: 0x00087561 File Offset: 0x00085961
		public override Oni.ConstraintType GetConstraintType()
		{
			return Oni.ConstraintType.Bending;
		}

		// Token: 0x060017B3 RID: 6067 RVA: 0x00087564 File Offset: 0x00085964
		public override void Clear()
		{
			this.activeConstraints.Clear();
			this.bendingIndices.Clear();
			this.restBends.Clear();
			this.bendingStiffnesses.Clear();
			this.constraintCount = 0;
		}

		// Token: 0x060017B4 RID: 6068 RVA: 0x0008759C File Offset: 0x0008599C
		public void AddConstraint(int index1, int index2, int index3, float restBend, float bending, float stiffness)
		{
			this.activeConstraints.Add(this.constraintCount);
			this.bendingIndices.Add(index1);
			this.bendingIndices.Add(index2);
			this.bendingIndices.Add(index3);
			this.restBends.Add(restBend);
			this.bendingStiffnesses.Add(new Vector2(bending, stiffness));
			this.constraintCount++;
		}

		// Token: 0x060017B5 RID: 6069 RVA: 0x00087610 File Offset: 0x00085A10
		public void InsertConstraint(int constraintIndex, int index1, int index2, int index3, float restBend, float bending, float stiffness)
		{
			if (constraintIndex < 0 || constraintIndex > base.ConstraintCount)
			{
				return;
			}
			for (int i = 0; i < this.activeConstraints.Count; i++)
			{
				if (this.activeConstraints[i] >= constraintIndex)
				{
					List<int> activeConstraints;
					int index4;
					(activeConstraints = this.activeConstraints)[index4 = i] = activeConstraints[index4] + 1;
				}
			}
			this.activeConstraints.Add(constraintIndex);
			this.bendingIndices.Insert(constraintIndex * 3, index1);
			this.bendingIndices.Insert(constraintIndex * 3 + 1, index2);
			this.bendingIndices.Insert(constraintIndex * 3 + 2, index3);
			this.restBends.Insert(constraintIndex, restBend);
			this.bendingStiffnesses.Insert(constraintIndex, new Vector2(bending, stiffness));
			this.constraintCount++;
		}

		// Token: 0x060017B6 RID: 6070 RVA: 0x000876E8 File Offset: 0x00085AE8
		public void SetParticleIndex(int constraintIndex, int particleIndex, ObiBendConstraintBatch.BendIndexType type, bool wraparound)
		{
			if (!wraparound)
			{
				if (constraintIndex >= 0 && constraintIndex < base.ConstraintCount)
				{
					this.bendingIndices[(int)(constraintIndex * 3 + type)] = particleIndex;
				}
			}
			else
			{
				this.bendingIndices[(int)((int)ObiUtils.Mod((float)constraintIndex, (float)base.ConstraintCount) * 3 + type)] = particleIndex;
			}
		}

		// Token: 0x060017B7 RID: 6071 RVA: 0x00087744 File Offset: 0x00085B44
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
			this.bendingIndices.RemoveRange(index * 3, 3);
			this.restBends.RemoveAt(index);
			this.bendingStiffnesses.RemoveAt(index);
			this.constraintCount--;
		}

		// Token: 0x060017B8 RID: 6072 RVA: 0x000877F0 File Offset: 0x00085BF0
		public override List<int> GetConstraintsInvolvingParticle(int particleIndex)
		{
			List<int> list = new List<int>(5);
			for (int i = 0; i < base.ConstraintCount; i++)
			{
				if (this.bendingIndices[i * 3] == particleIndex || this.bendingIndices[i * 3 + 1] == particleIndex || this.bendingIndices[i * 3 + 2] == particleIndex)
				{
					list.Add(i);
				}
			}
			return list;
		}

		// Token: 0x060017B9 RID: 6073 RVA: 0x00087864 File Offset: 0x00085C64
		public override void Cook()
		{
			this.batch = Oni.CreateBatch(3, true);
			Oni.SetBendingConstraints(this.batch, this.bendingIndices.ToArray(), this.restBends.ToArray(), this.bendingStiffnesses.ToArray(), base.ConstraintCount);
			if (Oni.CookBatch(this.batch))
			{
				this.constraintCount = Oni.GetBatchConstraintCount(this.batch);
				this.activeConstraints = Enumerable.Range(0, this.constraintCount).ToList<int>();
				int[] array = new int[this.constraintCount * 3];
				float[] collection = new float[this.constraintCount];
				Vector2[] collection2 = new Vector2[this.constraintCount];
				Oni.GetBendingConstraints(this.batch, array, collection, collection2);
				this.bendingIndices = new List<int>(array);
				this.restBends = new List<float>(collection);
				this.bendingStiffnesses = new List<Vector2>(collection2);
				int batchPhaseCount = Oni.GetBatchPhaseCount(this.batch);
				int[] array2 = new int[batchPhaseCount];
				Oni.GetBatchPhaseSizes(this.batch, array2);
				this.phaseSizes = new List<int>(array2);
			}
			Oni.DestroyBatch(this.batch);
			this.batch = IntPtr.Zero;
		}

		// Token: 0x060017BA RID: 6074 RVA: 0x00087984 File Offset: 0x00085D84
		protected override void OnAddToSolver(ObiBatchedConstraints constraints)
		{
			this.solverIndices = new int[this.bendingIndices.Count];
			for (int i = 0; i < this.restBends.Count; i++)
			{
				this.solverIndices[i * 3] = constraints.Actor.particleIndices[this.bendingIndices[i * 3]];
				this.solverIndices[i * 3 + 1] = constraints.Actor.particleIndices[this.bendingIndices[i * 3 + 1]];
				this.solverIndices[i * 3 + 2] = constraints.Actor.particleIndices[this.bendingIndices[i * 3 + 2]];
			}
		}

		// Token: 0x060017BB RID: 6075 RVA: 0x00087A37 File Offset: 0x00085E37
		protected override void OnRemoveFromSolver(ObiBatchedConstraints constraints)
		{
		}

		// Token: 0x060017BC RID: 6076 RVA: 0x00087A3C File Offset: 0x00085E3C
		public override void PushDataToSolver(ObiBatchedConstraints constraints)
		{
			if (constraints == null || constraints.Actor == null || !constraints.Actor.InSolver)
			{
				return;
			}
			ObiBendingConstraints obiBendingConstraints = (ObiBendingConstraints)constraints;
			for (int i = 0; i < this.bendingStiffnesses.Count; i++)
			{
				this.bendingStiffnesses[i] = new Vector2(obiBendingConstraints.maxBending, base.StiffnessToCompliance(obiBendingConstraints.stiffness));
			}
			Oni.SetBendingConstraints(this.batch, this.solverIndices, this.restBends.ToArray(), this.bendingStiffnesses.ToArray(), base.ConstraintCount);
			Oni.SetBatchPhaseSizes(this.batch, this.phaseSizes.ToArray(), this.phaseSizes.Count);
		}

		// Token: 0x060017BD RID: 6077 RVA: 0x00087B0B File Offset: 0x00085F0B
		public override void PullDataFromSolver(ObiBatchedConstraints constraints)
		{
		}

		// Token: 0x0400137C RID: 4988
		[HideInInspector]
		public List<int> bendingIndices = new List<int>();

		// Token: 0x0400137D RID: 4989
		[HideInInspector]
		public List<float> restBends = new List<float>();

		// Token: 0x0400137E RID: 4990
		[HideInInspector]
		public List<Vector2> bendingStiffnesses = new List<Vector2>();

		// Token: 0x0400137F RID: 4991
		private int[] solverIndices = new int[0];

		// Token: 0x020003AA RID: 938
		public enum BendIndexType
		{
			// Token: 0x04001381 RID: 4993
			First,
			// Token: 0x04001382 RID: 4994
			Second,
			// Token: 0x04001383 RID: 4995
			Pivot
		}
	}
}
