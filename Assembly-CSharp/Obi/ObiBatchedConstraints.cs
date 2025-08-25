using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003B3 RID: 947
	[ExecuteInEditMode]
	public abstract class ObiBatchedConstraints : MonoBehaviour, IObiSolverClient
	{
		// Token: 0x0600181C RID: 6172 RVA: 0x000893F2 File Offset: 0x000877F2
		protected ObiBatchedConstraints()
		{
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x0600181D RID: 6173 RVA: 0x000893FA File Offset: 0x000877FA
		public ObiActor Actor
		{
			get
			{
				return this.actor;
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x0600181E RID: 6174 RVA: 0x00089402 File Offset: 0x00087802
		public bool InSolver
		{
			get
			{
				return this.inSolver;
			}
		}

		// Token: 0x0600181F RID: 6175
		public abstract Oni.ConstraintType GetConstraintType();

		// Token: 0x06001820 RID: 6176
		public abstract List<ObiConstraintBatch> GetBatches();

		// Token: 0x06001821 RID: 6177
		public abstract void Clear();

		// Token: 0x06001822 RID: 6178 RVA: 0x0008940C File Offset: 0x0008780C
		protected void OnAddToSolver(object info)
		{
			foreach (ObiConstraintBatch obiConstraintBatch in this.GetBatches())
			{
				obiConstraintBatch.AddToSolver(this);
			}
		}

		// Token: 0x06001823 RID: 6179 RVA: 0x00089468 File Offset: 0x00087868
		protected void OnRemoveFromSolver(object info)
		{
			foreach (ObiConstraintBatch obiConstraintBatch in this.GetBatches())
			{
				obiConstraintBatch.RemoveFromSolver(this);
			}
		}

		// Token: 0x06001824 RID: 6180 RVA: 0x000894C4 File Offset: 0x000878C4
		public void PushDataToSolver(ParticleData data = ParticleData.NONE)
		{
			foreach (ObiConstraintBatch obiConstraintBatch in this.GetBatches())
			{
				obiConstraintBatch.PushDataToSolver(this);
			}
		}

		// Token: 0x06001825 RID: 6181 RVA: 0x00089520 File Offset: 0x00087920
		public void PullDataFromSolver(ParticleData data = ParticleData.NONE)
		{
			foreach (ObiConstraintBatch obiConstraintBatch in this.GetBatches())
			{
				obiConstraintBatch.PullDataFromSolver(this);
			}
		}

		// Token: 0x06001826 RID: 6182 RVA: 0x0008957C File Offset: 0x0008797C
		public void SetActiveConstraints()
		{
			foreach (ObiConstraintBatch obiConstraintBatch in this.GetBatches())
			{
				obiConstraintBatch.SetActiveConstraints();
			}
		}

		// Token: 0x06001827 RID: 6183 RVA: 0x000895D8 File Offset: 0x000879D8
		public void Enable()
		{
			foreach (ObiConstraintBatch obiConstraintBatch in this.GetBatches())
			{
				obiConstraintBatch.Enable();
			}
		}

		// Token: 0x06001828 RID: 6184 RVA: 0x00089634 File Offset: 0x00087A34
		public void Disable()
		{
			foreach (ObiConstraintBatch obiConstraintBatch in this.GetBatches())
			{
				obiConstraintBatch.Disable();
			}
		}

		// Token: 0x06001829 RID: 6185 RVA: 0x00089690 File Offset: 0x00087A90
		public bool AddToSolver(object info)
		{
			if (this.inSolver || this.actor == null || !this.actor.InSolver)
			{
				return false;
			}
			this.OnAddToSolver(info);
			this.inSolver = true;
			this.PushDataToSolver(ParticleData.NONE);
			this.SetActiveConstraints();
			if (base.isActiveAndEnabled)
			{
				this.Enable();
			}
			else
			{
				this.Disable();
			}
			return true;
		}

		// Token: 0x0600182A RID: 6186 RVA: 0x00089703 File Offset: 0x00087B03
		public bool RemoveFromSolver(object info)
		{
			if (!this.inSolver || this.actor == null || !this.actor.InSolver)
			{
				return false;
			}
			this.OnRemoveFromSolver(null);
			this.inSolver = false;
			return true;
		}

		// Token: 0x0600182B RID: 6187 RVA: 0x00089742 File Offset: 0x00087B42
		public void GrabActor()
		{
			this.actor = base.GetComponent<ObiActor>();
		}

		// Token: 0x0600182C RID: 6188 RVA: 0x00089750 File Offset: 0x00087B50
		public void OnEnable()
		{
			this.Enable();
		}

		// Token: 0x0600182D RID: 6189 RVA: 0x00089758 File Offset: 0x00087B58
		public void OnDisable()
		{
			if (this.actor == null || !this.actor.InSolver)
			{
				return;
			}
			this.Disable();
		}

		// Token: 0x0600182E RID: 6190 RVA: 0x00089782 File Offset: 0x00087B82
		public void OnDestroy()
		{
			this.RemoveFromSolver(null);
		}

		// Token: 0x040013AF RID: 5039
		public bool visualize;

		// Token: 0x040013B0 RID: 5040
		[NonSerialized]
		protected ObiActor actor;

		// Token: 0x040013B1 RID: 5041
		[NonSerialized]
		protected bool inSolver;
	}
}
