using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003AC RID: 940
	[Serializable]
	public class ObiDistanceConstraintBatch : ObiConstraintBatch
	{
		// Token: 0x060017D5 RID: 6101 RVA: 0x00087B0D File Offset: 0x00085F0D
		public ObiDistanceConstraintBatch(bool cooked, bool sharesParticles) : base(cooked, sharesParticles)
		{
		}

		// Token: 0x060017D6 RID: 6102 RVA: 0x00087B44 File Offset: 0x00085F44
		public ObiDistanceConstraintBatch(bool cooked, bool sharesParticles, float minYoungModulus, float maxYoungModulus) : base(cooked, sharesParticles, minYoungModulus, maxYoungModulus)
		{
		}

		// Token: 0x060017D7 RID: 6103 RVA: 0x00087B7E File Offset: 0x00085F7E
		public override Oni.ConstraintType GetConstraintType()
		{
			return Oni.ConstraintType.Distance;
		}

		// Token: 0x060017D8 RID: 6104 RVA: 0x00087B81 File Offset: 0x00085F81
		public override void Clear()
		{
			this.activeConstraints.Clear();
			this.springIndices.Clear();
			this.restLengths.Clear();
			this.stiffnesses.Clear();
			this.constraintCount = 0;
		}

		// Token: 0x060017D9 RID: 6105 RVA: 0x00087BB8 File Offset: 0x00085FB8
		public void AddConstraint(int index1, int index2, float restLength, float stretchStiffness, float compressionStiffness)
		{
			this.activeConstraints.Add(this.constraintCount);
			this.springIndices.Add(index1);
			this.springIndices.Add(index2);
			this.restLengths.Add(restLength);
			this.stiffnesses.Add(new Vector2(stretchStiffness, compressionStiffness));
			this.constraintCount++;
		}

		// Token: 0x060017DA RID: 6106 RVA: 0x00087C1C File Offset: 0x0008601C
		public void InsertConstraint(int constraintIndex, int index1, int index2, float restLength, float stretchStiffness, float compressionStiffness)
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
					int index3;
					(activeConstraints = this.activeConstraints)[index3 = i] = activeConstraints[index3] + 1;
				}
			}
			this.activeConstraints.Add(constraintIndex);
			this.springIndices.Insert(constraintIndex * 2, index1);
			this.springIndices.Insert(constraintIndex * 2 + 1, index2);
			this.restLengths.Insert(constraintIndex, restLength);
			this.stiffnesses.Insert(constraintIndex, new Vector2(stretchStiffness, compressionStiffness));
			this.constraintCount++;
		}

		// Token: 0x060017DB RID: 6107 RVA: 0x00087CE4 File Offset: 0x000860E4
		public void SetParticleIndex(int constraintIndex, int particleIndex, ObiDistanceConstraintBatch.DistanceIndexType type, bool wraparound)
		{
			if (!wraparound)
			{
				if (constraintIndex >= 0 && constraintIndex < base.ConstraintCount)
				{
					this.springIndices[(int)(constraintIndex * 2 + type)] = particleIndex;
				}
			}
			else
			{
				this.springIndices[(int)((int)ObiUtils.Mod((float)constraintIndex, (float)base.ConstraintCount) * 2 + type)] = particleIndex;
			}
		}

		// Token: 0x060017DC RID: 6108 RVA: 0x00087D40 File Offset: 0x00086140
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
			this.springIndices.RemoveRange(index * 2, 2);
			this.restLengths.RemoveAt(index);
			this.stiffnesses.RemoveAt(index);
			this.constraintCount--;
		}

		// Token: 0x060017DD RID: 6109 RVA: 0x00087DEC File Offset: 0x000861EC
		public override List<int> GetConstraintsInvolvingParticle(int particleIndex)
		{
			List<int> list = new List<int>(10);
			for (int i = 0; i < base.ConstraintCount; i++)
			{
				if (this.springIndices[i * 2] == particleIndex || this.springIndices[i * 2 + 1] == particleIndex)
				{
					list.Add(i);
				}
			}
			return list;
		}

		// Token: 0x060017DE RID: 6110 RVA: 0x00087E4C File Offset: 0x0008624C
		public override void Cook()
		{
			this.batch = Oni.CreateBatch(4, true);
			Oni.SetDistanceConstraints(this.batch, this.springIndices.ToArray(), this.restLengths.ToArray(), this.stiffnesses.ToArray(), base.ConstraintCount);
			if (Oni.CookBatch(this.batch))
			{
				this.constraintCount = Oni.GetBatchConstraintCount(this.batch);
				this.activeConstraints = Enumerable.Range(0, this.constraintCount).ToList<int>();
				int[] array = new int[this.constraintCount * 2];
				float[] collection = new float[this.constraintCount];
				Vector2[] collection2 = new Vector2[this.constraintCount];
				Oni.GetDistanceConstraints(this.batch, array, collection, collection2);
				this.springIndices = new List<int>(array);
				this.restLengths = new List<float>(collection);
				this.stiffnesses = new List<Vector2>(collection2);
				int batchPhaseCount = Oni.GetBatchPhaseCount(this.batch);
				int[] array2 = new int[batchPhaseCount];
				Oni.GetBatchPhaseSizes(this.batch, array2);
				this.phaseSizes = new List<int>(array2);
			}
			Oni.DestroyBatch(this.batch);
			this.batch = IntPtr.Zero;
		}

		// Token: 0x060017DF RID: 6111 RVA: 0x00087F6C File Offset: 0x0008636C
		protected override void OnAddToSolver(ObiBatchedConstraints constraints)
		{
			this.solverIndices = new int[this.springIndices.Count];
			for (int i = 0; i < this.restLengths.Count; i++)
			{
				this.solverIndices[i * 2] = constraints.Actor.particleIndices[this.springIndices[i * 2]];
				this.solverIndices[i * 2 + 1] = constraints.Actor.particleIndices[this.springIndices[i * 2 + 1]];
			}
		}

		// Token: 0x060017E0 RID: 6112 RVA: 0x00087FF7 File Offset: 0x000863F7
		protected override void OnRemoveFromSolver(ObiBatchedConstraints constraints)
		{
		}

		// Token: 0x060017E1 RID: 6113 RVA: 0x00087FFC File Offset: 0x000863FC
		public override void PushDataToSolver(ObiBatchedConstraints constraints)
		{
			if (constraints == null || constraints.Actor == null || !constraints.Actor.InSolver)
			{
				return;
			}
			ObiDistanceConstraints obiDistanceConstraints = (ObiDistanceConstraints)constraints;
			float[] array = new float[this.restLengths.Count];
			for (int i = 0; i < this.restLengths.Count; i++)
			{
				array[i] = this.restLengths[i] * obiDistanceConstraints.stretchingScale;
				this.stiffnesses[i] = new Vector2(base.StiffnessToCompliance(obiDistanceConstraints.stiffness), obiDistanceConstraints.slack * array[i]);
			}
			Oni.SetDistanceConstraints(this.batch, this.solverIndices, array, this.stiffnesses.ToArray(), base.ConstraintCount);
			Oni.SetBatchPhaseSizes(this.batch, this.phaseSizes.ToArray(), this.phaseSizes.Count);
		}

		// Token: 0x060017E2 RID: 6114 RVA: 0x000880EC File Offset: 0x000864EC
		public override void PullDataFromSolver(ObiBatchedConstraints constraints)
		{
		}

		// Token: 0x0400138C RID: 5004
		[HideInInspector]
		public List<int> springIndices = new List<int>();

		// Token: 0x0400138D RID: 5005
		[HideInInspector]
		public List<float> restLengths = new List<float>();

		// Token: 0x0400138E RID: 5006
		[HideInInspector]
		public List<Vector2> stiffnesses = new List<Vector2>();

		// Token: 0x0400138F RID: 5007
		private int[] solverIndices = new int[0];

		// Token: 0x020003AD RID: 941
		public enum DistanceIndexType
		{
			// Token: 0x04001391 RID: 5009
			First,
			// Token: 0x04001392 RID: 5010
			Second
		}
	}
}
