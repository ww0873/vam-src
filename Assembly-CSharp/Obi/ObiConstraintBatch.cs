using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003AB RID: 939
	[Serializable]
	public abstract class ObiConstraintBatch
	{
		// Token: 0x060017BE RID: 6078 RVA: 0x00087024 File Offset: 0x00085424
		public ObiConstraintBatch(bool cooked, bool sharesParticles)
		{
			this.cooked = cooked;
			this.sharesParticles = sharesParticles;
		}

		// Token: 0x060017BF RID: 6079 RVA: 0x00087074 File Offset: 0x00085474
		public ObiConstraintBatch(bool cooked, bool sharesParticles, float minYoungModulus, float maxYoungModulus)
		{
			this.cooked = cooked;
			this.sharesParticles = sharesParticles;
			this.maxYoungModulus = maxYoungModulus;
			this.minYoungModulus = minYoungModulus;
		}

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x060017C0 RID: 6080 RVA: 0x000870D0 File Offset: 0x000854D0
		public IntPtr OniBatch
		{
			get
			{
				return this.batch;
			}
		}

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x060017C1 RID: 6081 RVA: 0x000870D8 File Offset: 0x000854D8
		public int ConstraintCount
		{
			get
			{
				return this.constraintCount;
			}
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x060017C2 RID: 6082 RVA: 0x000870E0 File Offset: 0x000854E0
		public bool IsCooked
		{
			get
			{
				return this.cooked;
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x060017C3 RID: 6083 RVA: 0x000870E8 File Offset: 0x000854E8
		public bool SharesParticles
		{
			get
			{
				return this.sharesParticles;
			}
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x060017C4 RID: 6084 RVA: 0x000870F0 File Offset: 0x000854F0
		public IEnumerable<int> ActiveConstraints
		{
			get
			{
				return this.activeConstraints.AsReadOnly();
			}
		}

		// Token: 0x060017C5 RID: 6085
		public abstract Oni.ConstraintType GetConstraintType();

		// Token: 0x060017C6 RID: 6086
		public abstract void Clear();

		// Token: 0x060017C7 RID: 6087 RVA: 0x000870FD File Offset: 0x000854FD
		public virtual void Cook()
		{
		}

		// Token: 0x060017C8 RID: 6088
		protected abstract void OnAddToSolver(ObiBatchedConstraints constraints);

		// Token: 0x060017C9 RID: 6089
		protected abstract void OnRemoveFromSolver(ObiBatchedConstraints constraints);

		// Token: 0x060017CA RID: 6090
		public abstract void PushDataToSolver(ObiBatchedConstraints constraints);

		// Token: 0x060017CB RID: 6091
		public abstract void PullDataFromSolver(ObiBatchedConstraints constraints);

		// Token: 0x060017CC RID: 6092
		public abstract List<int> GetConstraintsInvolvingParticle(int particleIndex);

		// Token: 0x060017CD RID: 6093 RVA: 0x00087100 File Offset: 0x00085500
		protected float StiffnessToCompliance(float stiffness)
		{
			float a = 1f / Mathf.Max(this.minYoungModulus, 1E-05f);
			float b = 1f / Mathf.Max(this.maxYoungModulus, this.minYoungModulus);
			return Mathf.Lerp(a, b, stiffness);
		}

		// Token: 0x060017CE RID: 6094 RVA: 0x00087144 File Offset: 0x00085544
		public void ActivateConstraint(int index)
		{
			if (!this.activeConstraints.Contains(index))
			{
				this.activeConstraints.Add(index);
			}
		}

		// Token: 0x060017CF RID: 6095 RVA: 0x00087163 File Offset: 0x00085563
		public void DeactivateConstraint(int index)
		{
			this.activeConstraints.Remove(index);
		}

		// Token: 0x060017D0 RID: 6096 RVA: 0x00087174 File Offset: 0x00085574
		public void AddToSolver(ObiBatchedConstraints constraints)
		{
			this.batch = Oni.CreateBatch((int)this.GetConstraintType(), this.cooked);
			Oni.AddBatch(constraints.Actor.Solver.OniSolver, this.batch, this.sharesParticles);
			this.OnAddToSolver(constraints);
		}

		// Token: 0x060017D1 RID: 6097 RVA: 0x000871C1 File Offset: 0x000855C1
		public void RemoveFromSolver(ObiBatchedConstraints constraints)
		{
			this.OnRemoveFromSolver(constraints);
			Oni.RemoveBatch(constraints.Actor.Solver.OniSolver, this.batch);
			this.batch = IntPtr.Zero;
		}

		// Token: 0x060017D2 RID: 6098 RVA: 0x000871F0 File Offset: 0x000855F0
		public void SetActiveConstraints()
		{
			Oni.SetActiveConstraints(this.batch, this.activeConstraints.ToArray(), this.activeConstraints.Count);
		}

		// Token: 0x060017D3 RID: 6099 RVA: 0x00087214 File Offset: 0x00085614
		public void Enable()
		{
			Oni.EnableBatch(this.batch, true);
		}

		// Token: 0x060017D4 RID: 6100 RVA: 0x00087223 File Offset: 0x00085623
		public void Disable()
		{
			Oni.EnableBatch(this.batch, false);
		}

		// Token: 0x04001384 RID: 4996
		protected IntPtr batch;

		// Token: 0x04001385 RID: 4997
		public float maxYoungModulus = 0.02f;

		// Token: 0x04001386 RID: 4998
		public float minYoungModulus = 0.0001f;

		// Token: 0x04001387 RID: 4999
		[SerializeField]
		[HideInInspector]
		protected int constraintCount;

		// Token: 0x04001388 RID: 5000
		[SerializeField]
		[HideInInspector]
		protected bool cooked;

		// Token: 0x04001389 RID: 5001
		[SerializeField]
		[HideInInspector]
		protected bool sharesParticles;

		// Token: 0x0400138A RID: 5002
		[SerializeField]
		[HideInInspector]
		protected List<int> activeConstraints = new List<int>();

		// Token: 0x0400138B RID: 5003
		[SerializeField]
		[HideInInspector]
		protected List<int> phaseSizes = new List<int>();
	}
}
