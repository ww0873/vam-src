using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	// Token: 0x02000396 RID: 918
	[ExecuteInEditMode]
	[AddComponentMenu("Physics/Obi/Obi Stitcher")]
	public class ObiStitcher : MonoBehaviour, IObiSolverClient
	{
		// Token: 0x06001746 RID: 5958 RVA: 0x00085187 File Offset: 0x00083587
		public ObiStitcher()
		{
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06001748 RID: 5960 RVA: 0x00085297 File Offset: 0x00083697
		// (set) Token: 0x06001747 RID: 5959 RVA: 0x0008519C File Offset: 0x0008359C
		public ObiActor Actor1
		{
			get
			{
				return this.actor1;
			}
			set
			{
				if (this.actor1 != value)
				{
					if (this.actor1 != null)
					{
						this.actor1.OnAddedToSolver -= this.Actor_OnAddedToSolver;
						this.actor1.OnRemovedFromSolver -= this.Actor_OnRemovedFromSolver;
						if (this.actor1.InSolver)
						{
							this.Actor_OnRemovedFromSolver(this.actor1, new ObiActor.ObiActorSolverArgs(this.actor1.Solver));
						}
					}
					this.actor1 = value;
					if (this.actor1 != null)
					{
						this.actor1.OnAddedToSolver += this.Actor_OnAddedToSolver;
						this.actor1.OnRemovedFromSolver += this.Actor_OnRemovedFromSolver;
						if (this.actor1.InSolver)
						{
							this.Actor_OnAddedToSolver(this.actor1, new ObiActor.ObiActorSolverArgs(this.actor1.Solver));
						}
					}
				}
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x0600174A RID: 5962 RVA: 0x0008539B File Offset: 0x0008379B
		// (set) Token: 0x06001749 RID: 5961 RVA: 0x000852A0 File Offset: 0x000836A0
		public ObiActor Actor2
		{
			get
			{
				return this.actor2;
			}
			set
			{
				if (this.actor2 != value)
				{
					if (this.actor2 != null)
					{
						this.actor2.OnAddedToSolver -= this.Actor_OnAddedToSolver;
						this.actor2.OnRemovedFromSolver -= this.Actor_OnRemovedFromSolver;
						if (this.actor2.InSolver)
						{
							this.Actor_OnRemovedFromSolver(this.actor2, new ObiActor.ObiActorSolverArgs(this.actor2.Solver));
						}
					}
					this.actor2 = value;
					if (this.actor2 != null)
					{
						this.actor2.OnAddedToSolver += this.Actor_OnAddedToSolver;
						this.actor2.OnRemovedFromSolver += this.Actor_OnRemovedFromSolver;
						if (this.actor2.InSolver)
						{
							this.Actor_OnAddedToSolver(this.actor2, new ObiActor.ObiActorSolverArgs(this.actor2.Solver));
						}
					}
				}
			}
		}

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x0600174B RID: 5963 RVA: 0x000853A3 File Offset: 0x000837A3
		public int StitchCount
		{
			get
			{
				return this.stitches.Count;
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x0600174C RID: 5964 RVA: 0x000853B0 File Offset: 0x000837B0
		public IEnumerable<ObiStitcher.Stitch> Stitches
		{
			get
			{
				return this.stitches.AsReadOnly();
			}
		}

		// Token: 0x0600174D RID: 5965 RVA: 0x000853C0 File Offset: 0x000837C0
		public void OnEnable()
		{
			if (this.actor1 != null)
			{
				this.actor1.OnAddedToSolver += this.Actor_OnAddedToSolver;
				this.actor1.OnRemovedFromSolver += this.Actor_OnRemovedFromSolver;
			}
			if (this.actor2 != null)
			{
				this.actor2.OnAddedToSolver += this.Actor_OnAddedToSolver;
				this.actor2.OnRemovedFromSolver += this.Actor_OnRemovedFromSolver;
			}
			if (this.actor1 != null && this.actor2 != null)
			{
				Oni.EnableBatch(this.batch, true);
			}
		}

		// Token: 0x0600174E RID: 5966 RVA: 0x0008547A File Offset: 0x0008387A
		public void OnDisable()
		{
			Oni.EnableBatch(this.batch, false);
		}

		// Token: 0x0600174F RID: 5967 RVA: 0x00085489 File Offset: 0x00083889
		public int AddStitch(int particle1, int particle2)
		{
			this.stitches.Add(new ObiStitcher.Stitch(particle1, particle2));
			return this.stitches.Count - 1;
		}

		// Token: 0x06001750 RID: 5968 RVA: 0x000854AA File Offset: 0x000838AA
		public void RemoveStitch(int index)
		{
			if (index >= 0 && index < this.stitches.Count)
			{
				this.stitches.RemoveAt(index);
			}
		}

		// Token: 0x06001751 RID: 5969 RVA: 0x000854D0 File Offset: 0x000838D0
		public void Clear()
		{
			this.stitches.Clear();
			this.PushDataToSolver(ParticleData.NONE);
		}

		// Token: 0x06001752 RID: 5970 RVA: 0x000854E4 File Offset: 0x000838E4
		private void Actor_OnRemovedFromSolver(object sender, ObiActor.ObiActorSolverArgs e)
		{
			this.RemoveFromSolver(null);
		}

		// Token: 0x06001753 RID: 5971 RVA: 0x000854F0 File Offset: 0x000838F0
		private void Actor_OnAddedToSolver(object sender, ObiActor.ObiActorSolverArgs e)
		{
			if (this.actor1.InSolver && this.actor2.InSolver)
			{
				if (this.actor1.Solver != this.actor2.Solver)
				{
					Debug.LogError("ObiStitcher cannot handle actors in different solvers.");
					return;
				}
				this.AddToSolver(null);
			}
		}

		// Token: 0x06001754 RID: 5972 RVA: 0x00085550 File Offset: 0x00083950
		public bool AddToSolver(object info)
		{
			this.batch = Oni.CreateBatch(10, false);
			Oni.AddBatch(this.actor1.Solver.OniSolver, this.batch, false);
			this.inSolver = true;
			this.PushDataToSolver(ParticleData.NONE);
			if (base.isActiveAndEnabled)
			{
				this.OnEnable();
			}
			else
			{
				this.OnDisable();
			}
			return true;
		}

		// Token: 0x06001755 RID: 5973 RVA: 0x000855B3 File Offset: 0x000839B3
		public bool RemoveFromSolver(object info)
		{
			Oni.RemoveBatch(this.actor1.Solver.OniSolver, this.batch);
			this.batch = IntPtr.Zero;
			this.inSolver = false;
			return true;
		}

		// Token: 0x06001756 RID: 5974 RVA: 0x000855E4 File Offset: 0x000839E4
		public void PushDataToSolver(ParticleData data = ParticleData.NONE)
		{
			if (!this.inSolver)
			{
				return;
			}
			int[] array = new int[this.stitches.Count * 2];
			float[] array2 = new float[this.stitches.Count];
			for (int i = 0; i < this.stitches.Count; i++)
			{
				array[i * 2] = this.actor1.particleIndices[this.stitches[i].particleIndex1];
				array[i * 2 + 1] = this.actor2.particleIndices[this.stitches[i].particleIndex2];
				array2[i] = 0f;
			}
			Oni.SetStitchConstraints(this.batch, array, array2, this.stitches.Count);
			int[] array3 = new int[this.stitches.Count];
			for (int j = 0; j < this.stitches.Count; j++)
			{
				array3[j] = j;
			}
			Oni.SetActiveConstraints(this.batch, array3, this.stitches.Count);
		}

		// Token: 0x06001757 RID: 5975 RVA: 0x000856F2 File Offset: 0x00083AF2
		public void PullDataFromSolver(ParticleData data = ParticleData.NONE)
		{
		}

		// Token: 0x04001334 RID: 4916
		[SerializeField]
		[HideInInspector]
		private List<ObiStitcher.Stitch> stitches = new List<ObiStitcher.Stitch>();

		// Token: 0x04001335 RID: 4917
		[SerializeField]
		[HideInInspector]
		private ObiActor actor1;

		// Token: 0x04001336 RID: 4918
		[SerializeField]
		[HideInInspector]
		private ObiActor actor2;

		// Token: 0x04001337 RID: 4919
		private IntPtr batch;

		// Token: 0x04001338 RID: 4920
		private bool inSolver;

		// Token: 0x02000397 RID: 919
		[Serializable]
		public class Stitch
		{
			// Token: 0x06001758 RID: 5976 RVA: 0x000856F4 File Offset: 0x00083AF4
			public Stitch(int particleIndex1, int particleIndex2)
			{
				this.particleIndex1 = particleIndex1;
				this.particleIndex2 = particleIndex2;
			}

			// Token: 0x04001339 RID: 4921
			public int particleIndex1;

			// Token: 0x0400133A RID: 4922
			public int particleIndex2;
		}
	}
}
