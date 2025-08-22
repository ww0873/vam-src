using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003AF RID: 943
	[Serializable]
	public class ObiSkinConstraintBatch : ObiConstraintBatch
	{
		// Token: 0x060017EF RID: 6127 RVA: 0x00088568 File Offset: 0x00086968
		public ObiSkinConstraintBatch(bool cooked, bool sharesParticles) : base(cooked, sharesParticles)
		{
		}

		// Token: 0x060017F0 RID: 6128 RVA: 0x000885C0 File Offset: 0x000869C0
		public ObiSkinConstraintBatch(bool cooked, bool sharesParticles, float minYoungModulus, float maxYoungModulus) : base(cooked, sharesParticles, minYoungModulus, maxYoungModulus)
		{
		}

		// Token: 0x060017F1 RID: 6129 RVA: 0x0008861B File Offset: 0x00086A1B
		public override Oni.ConstraintType GetConstraintType()
		{
			return Oni.ConstraintType.Skin;
		}

		// Token: 0x060017F2 RID: 6130 RVA: 0x00088620 File Offset: 0x00086A20
		public override void Clear()
		{
			this.activeConstraints.Clear();
			this.skinIndices.Clear();
			this.skinPoints.Clear();
			this.skinNormals.Clear();
			this.skinRadiiBackstop.Clear();
			this.skinStiffnesses.Clear();
			this.constraintCount = 0;
		}

		// Token: 0x060017F3 RID: 6131 RVA: 0x00088678 File Offset: 0x00086A78
		public void AddConstraint(int index, Vector4 point, Vector4 normal, float radius, float collisionRadius, float backstop, float stiffness)
		{
			this.activeConstraints.Add(this.constraintCount);
			this.skinIndices.Add(index);
			this.skinPoints.Add(point);
			this.skinNormals.Add(normal);
			this.skinRadiiBackstop.Add(radius);
			this.skinRadiiBackstop.Add(collisionRadius);
			this.skinRadiiBackstop.Add(backstop);
			this.skinStiffnesses.Add(stiffness);
			this.constraintCount++;
		}

		// Token: 0x060017F4 RID: 6132 RVA: 0x000886FC File Offset: 0x00086AFC
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
			this.skinIndices.RemoveAt(index);
			this.skinPoints.RemoveAt(index);
			this.skinNormals.RemoveAt(index);
			this.skinStiffnesses.RemoveAt(index);
			this.skinRadiiBackstop.RemoveRange(index * 3, 3);
			this.constraintCount--;
		}

		// Token: 0x060017F5 RID: 6133 RVA: 0x000887C0 File Offset: 0x00086BC0
		public override List<int> GetConstraintsInvolvingParticle(int particleIndex)
		{
			List<int> list = new List<int>(1);
			for (int i = 0; i < base.ConstraintCount; i++)
			{
				if (this.skinIndices[i] == particleIndex)
				{
					list.Add(i);
				}
			}
			return list;
		}

		// Token: 0x060017F6 RID: 6134 RVA: 0x00088808 File Offset: 0x00086C08
		public override void Cook()
		{
			this.batch = Oni.CreateBatch(8, true);
			Oni.SetSkinConstraints(this.batch, this.skinIndices.ToArray(), this.skinPoints.ToArray(), this.skinNormals.ToArray(), this.skinRadiiBackstop.ToArray(), this.skinStiffnesses.ToArray(), base.ConstraintCount);
			if (Oni.CookBatch(this.batch))
			{
				this.constraintCount = Oni.GetBatchConstraintCount(this.batch);
				this.activeConstraints = Enumerable.Range(0, this.constraintCount).ToList<int>();
				int[] array = new int[this.constraintCount];
				Vector4[] array2 = new Vector4[this.constraintCount];
				Vector4[] array3 = new Vector4[this.constraintCount];
				float[] array4 = new float[this.constraintCount * 3];
				float[] array5 = new float[this.constraintCount];
				Oni.GetSkinConstraints(this.batch, array, array2, array3, array4, array5);
				this.skinIndices = new List<int>(array);
				this.skinPoints = new List<Vector4>(array2);
				this.skinNormals = new List<Vector4>(array3);
				this.skinRadiiBackstop = new List<float>(array4);
				this.skinStiffnesses = new List<float>(array5);
				int batchPhaseCount = Oni.GetBatchPhaseCount(this.batch);
				int[] array6 = new int[batchPhaseCount];
				Oni.GetBatchPhaseSizes(this.batch, array6);
				this.phaseSizes = new List<int>(array6);
			}
			Oni.DestroyBatch(this.batch);
			this.batch = IntPtr.Zero;
		}

		// Token: 0x060017F7 RID: 6135 RVA: 0x00088978 File Offset: 0x00086D78
		protected override void OnAddToSolver(ObiBatchedConstraints constraints)
		{
			this.solverIndices = new int[this.skinIndices.Count];
			for (int i = 0; i < this.skinIndices.Count; i++)
			{
				this.solverIndices[i] = constraints.Actor.particleIndices[this.skinIndices[i]];
				this.solverIndices[i] = constraints.Actor.particleIndices[this.skinIndices[i]];
			}
		}

		// Token: 0x060017F8 RID: 6136 RVA: 0x000889F7 File Offset: 0x00086DF7
		protected override void OnRemoveFromSolver(ObiBatchedConstraints constraints)
		{
		}

		// Token: 0x060017F9 RID: 6137 RVA: 0x000889FC File Offset: 0x00086DFC
		public override void PushDataToSolver(ObiBatchedConstraints constraints)
		{
			if (constraints == null || constraints.Actor == null || !constraints.Actor.InSolver)
			{
				return;
			}
			ObiSkinConstraints obiSkinConstraints = (ObiSkinConstraints)constraints;
			float[] array = new float[this.skinStiffnesses.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = base.StiffnessToCompliance(this.skinStiffnesses[i] * obiSkinConstraints.stiffness);
			}
			Oni.SetSkinConstraints(this.batch, this.solverIndices, this.skinPoints.ToArray(), this.skinNormals.ToArray(), this.skinRadiiBackstop.ToArray(), array, base.ConstraintCount);
			Oni.SetBatchPhaseSizes(this.batch, this.phaseSizes.ToArray(), this.phaseSizes.Count);
		}

		// Token: 0x060017FA RID: 6138 RVA: 0x00088ADC File Offset: 0x00086EDC
		public override void PullDataFromSolver(ObiBatchedConstraints constraints)
		{
			if (constraints == null || constraints.Actor == null || !constraints.Actor.InSolver)
			{
				return;
			}
			int[] indices = new int[this.constraintCount];
			Vector4[] array = new Vector4[this.constraintCount];
			Vector4[] array2 = new Vector4[this.constraintCount];
			float[] radiiBackstops = new float[this.constraintCount * 3];
			float[] stiffnesses = new float[this.constraintCount];
			Oni.GetSkinConstraints(this.batch, indices, array, array2, radiiBackstops, stiffnesses);
			this.skinPoints = new List<Vector4>(array);
			this.skinNormals = new List<Vector4>(array2);
		}

		// Token: 0x060017FB RID: 6139 RVA: 0x00088B7F File Offset: 0x00086F7F
		public Vector3 GetSkinPosition(int index)
		{
			return this.skinPoints[index];
		}

		// Token: 0x060017FC RID: 6140 RVA: 0x00088B92 File Offset: 0x00086F92
		public Vector3 GetSkinNormal(int index)
		{
			return this.skinNormals[index];
		}

		// Token: 0x0400139A RID: 5018
		[HideInInspector]
		public List<int> skinIndices = new List<int>();

		// Token: 0x0400139B RID: 5019
		[HideInInspector]
		public List<Vector4> skinPoints = new List<Vector4>();

		// Token: 0x0400139C RID: 5020
		[HideInInspector]
		public List<Vector4> skinNormals = new List<Vector4>();

		// Token: 0x0400139D RID: 5021
		[HideInInspector]
		public List<float> skinRadiiBackstop = new List<float>();

		// Token: 0x0400139E RID: 5022
		[HideInInspector]
		public List<float> skinStiffnesses = new List<float>();

		// Token: 0x0400139F RID: 5023
		private int[] solverIndices = new int[0];
	}
}
