using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003AE RID: 942
	[Serializable]
	public class ObiPinConstraintBatch : ObiConstraintBatch
	{
		// Token: 0x060017E3 RID: 6115 RVA: 0x000880F0 File Offset: 0x000864F0
		public ObiPinConstraintBatch(bool cooked, bool sharesParticles) : base(cooked, sharesParticles)
		{
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x00088154 File Offset: 0x00086554
		public ObiPinConstraintBatch(bool cooked, bool sharesParticles, float minYoungModulus, float maxYoungModulus) : base(cooked, sharesParticles, minYoungModulus, maxYoungModulus)
		{
		}

		// Token: 0x060017E5 RID: 6117 RVA: 0x000881BB File Offset: 0x000865BB
		public override Oni.ConstraintType GetConstraintType()
		{
			return Oni.ConstraintType.Pin;
		}

		// Token: 0x060017E6 RID: 6118 RVA: 0x000881C0 File Offset: 0x000865C0
		public override void Clear()
		{
			this.activeConstraints.Clear();
			this.pinIndices.Clear();
			this.pinBodies.Clear();
			this.pinOffsets.Clear();
			this.stiffnesses.Clear();
			this.pinBreakResistance.Clear();
			this.constraintCount = 0;
		}

		// Token: 0x060017E7 RID: 6119 RVA: 0x00088218 File Offset: 0x00086618
		public void AddConstraint(int index1, ObiCollider body, Vector3 offset, float stiffness)
		{
			this.activeConstraints.Add(this.constraintCount);
			this.pinIndices.Add(index1);
			this.pinBodies.Add(body);
			this.pinOffsets.Add(offset);
			this.stiffnesses.Add(stiffness);
			this.pinBreakResistance.Add(float.MaxValue);
			this.constraintCount++;
		}

		// Token: 0x060017E8 RID: 6120 RVA: 0x0008828C File Offset: 0x0008668C
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
			this.pinIndices.RemoveAt(index);
			this.pinBodies.RemoveAt(index);
			this.pinOffsets.RemoveAt(index);
			this.stiffnesses.RemoveAt(index);
			this.pinBreakResistance.RemoveAt(index);
			this.constraintCount--;
		}

		// Token: 0x060017E9 RID: 6121 RVA: 0x0008834C File Offset: 0x0008674C
		public override List<int> GetConstraintsInvolvingParticle(int particleIndex)
		{
			List<int> list = new List<int>(5);
			for (int i = 0; i < base.ConstraintCount; i++)
			{
				if (this.pinIndices[i] == particleIndex)
				{
					list.Add(i);
				}
			}
			return list;
		}

		// Token: 0x060017EA RID: 6122 RVA: 0x00088394 File Offset: 0x00086794
		protected override void OnAddToSolver(ObiBatchedConstraints constraints)
		{
			this.solverIndices = new int[this.pinIndices.Count];
			this.solverColliders = new IntPtr[this.pinIndices.Count];
			for (int i = 0; i < this.pinOffsets.Count; i++)
			{
				this.solverIndices[i] = constraints.Actor.particleIndices[this.pinIndices[i]];
				this.solverColliders[i] = ((!(this.pinBodies[i] != null)) ? IntPtr.Zero : this.pinBodies[i].OniCollider);
			}
		}

		// Token: 0x060017EB RID: 6123 RVA: 0x00088448 File Offset: 0x00086848
		protected override void OnRemoveFromSolver(ObiBatchedConstraints constraints)
		{
		}

		// Token: 0x060017EC RID: 6124 RVA: 0x0008844C File Offset: 0x0008684C
		public override void PushDataToSolver(ObiBatchedConstraints constraints)
		{
			if (constraints == null || constraints.Actor == null || !constraints.Actor.InSolver)
			{
				return;
			}
			ObiPinConstraints obiPinConstraints = (ObiPinConstraints)constraints;
			for (int i = 0; i < this.stiffnesses.Count; i++)
			{
				this.stiffnesses[i] = base.StiffnessToCompliance(obiPinConstraints.stiffness);
			}
			Oni.SetPinConstraints(this.batch, this.solverIndices, this.pinOffsets.ToArray(), this.solverColliders, this.stiffnesses.ToArray(), base.ConstraintCount);
		}

		// Token: 0x060017ED RID: 6125 RVA: 0x000884F5 File Offset: 0x000868F5
		public override void PullDataFromSolver(ObiBatchedConstraints constraints)
		{
		}

		// Token: 0x060017EE RID: 6126 RVA: 0x000884F8 File Offset: 0x000868F8
		public void BreakConstraints()
		{
			float[] array = new float[base.ConstraintCount];
			Oni.GetBatchConstraintForces(this.batch, array, base.ConstraintCount, 0);
			for (int i = 0; i < array.Length; i++)
			{
				if (-array[i] * 1000f > this.pinBreakResistance[i])
				{
					this.activeConstraints.Remove(i);
				}
			}
			base.SetActiveConstraints();
		}

		// Token: 0x04001393 RID: 5011
		[HideInInspector]
		public List<int> pinIndices = new List<int>();

		// Token: 0x04001394 RID: 5012
		[HideInInspector]
		public List<ObiCollider> pinBodies = new List<ObiCollider>();

		// Token: 0x04001395 RID: 5013
		[HideInInspector]
		public List<Vector4> pinOffsets = new List<Vector4>();

		// Token: 0x04001396 RID: 5014
		[HideInInspector]
		public List<float> stiffnesses = new List<float>();

		// Token: 0x04001397 RID: 5015
		[HideInInspector]
		public List<float> pinBreakResistance = new List<float>();

		// Token: 0x04001398 RID: 5016
		private int[] solverIndices = new int[0];

		// Token: 0x04001399 RID: 5017
		private IntPtr[] solverColliders = new IntPtr[0];
	}
}
