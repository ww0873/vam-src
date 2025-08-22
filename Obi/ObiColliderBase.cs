using System;
using System.Collections.Generic;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003A5 RID: 933
	public abstract class ObiColliderBase : MonoBehaviour
	{
		// Token: 0x06001788 RID: 6024 RVA: 0x0008635C File Offset: 0x0008475C
		protected ObiColliderBase()
		{
		}

		// Token: 0x170002BA RID: 698
		// (get) Token: 0x0600178A RID: 6026 RVA: 0x000863F4 File Offset: 0x000847F4
		// (set) Token: 0x06001789 RID: 6025 RVA: 0x000863A4 File Offset: 0x000847A4
		public ObiCollisionMaterial CollisionMaterial
		{
			get
			{
				return this.material;
			}
			set
			{
				this.material = value;
				if (this.material != null)
				{
					Oni.SetColliderMaterial(this.oniCollider, this.material.OniCollisionMaterial);
				}
				else
				{
					Oni.SetColliderMaterial(this.oniCollider, IntPtr.Zero);
				}
			}
		}

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x0600178B RID: 6027 RVA: 0x000863FC File Offset: 0x000847FC
		public IntPtr OniCollider
		{
			get
			{
				return this.oniCollider;
			}
		}

		// Token: 0x0600178C RID: 6028
		protected abstract void CreateTracker();

		// Token: 0x0600178D RID: 6029
		protected abstract bool IsUnityColliderEnabled();

		// Token: 0x0600178E RID: 6030
		protected abstract void UpdateColliderAdaptor();

		// Token: 0x0600178F RID: 6031 RVA: 0x00086404 File Offset: 0x00084804
		protected void CreateRigidbody()
		{
			this.obiRigidbody = null;
			Rigidbody componentInParent = base.GetComponentInParent<Rigidbody>();
			if (componentInParent != null)
			{
				this.obiRigidbody = componentInParent.GetComponent<ObiRigidbody>();
				if (this.obiRigidbody == null)
				{
					this.obiRigidbody = componentInParent.gameObject.AddComponent<ObiRigidbody>();
				}
				Oni.SetColliderRigidbody(this.oniCollider, this.obiRigidbody.OniRigidbody);
			}
			else
			{
				Oni.SetColliderRigidbody(this.oniCollider, IntPtr.Zero);
			}
		}

		// Token: 0x06001790 RID: 6032 RVA: 0x00086484 File Offset: 0x00084884
		public void RegisterInSolver(ObiSolver solver, bool addToSolver)
		{
			if (!this.solvers.Contains(solver) && solver.collisionLayers == (solver.collisionLayers | 1 << base.gameObject.layer))
			{
				this.solvers.Add(solver);
				if (addToSolver)
				{
					Oni.AddCollider(solver.OniSolver, this.oniCollider);
				}
			}
		}

		// Token: 0x06001791 RID: 6033 RVA: 0x000864F2 File Offset: 0x000848F2
		public void RemoveFromSolver(ObiSolver solver)
		{
			this.solvers.Remove(solver);
			Oni.RemoveCollider(solver.OniSolver, this.oniCollider);
		}

		// Token: 0x06001792 RID: 6034 RVA: 0x00086514 File Offset: 0x00084914
		private void FindSolvers(bool addToSolvers)
		{
			if (base.gameObject.layer != this.currentLayer)
			{
				this.currentLayer = base.gameObject.layer;
				foreach (ObiSolver obiSolver in this.solvers)
				{
					Oni.RemoveCollider(obiSolver.OniSolver, this.oniCollider);
				}
				this.solvers.Clear();
				ObiSolver[] array = UnityEngine.Object.FindObjectsOfType<ObiSolver>();
				foreach (ObiSolver solver in array)
				{
					this.RegisterInSolver(solver, addToSolvers);
				}
			}
		}

		// Token: 0x06001793 RID: 6035 RVA: 0x000865DC File Offset: 0x000849DC
		private void Update()
		{
			this.FindSolvers(true);
		}

		// Token: 0x06001794 RID: 6036 RVA: 0x000865E8 File Offset: 0x000849E8
		protected virtual void Awake()
		{
			this.wasUnityColliderEnabled = this.IsUnityColliderEnabled();
			ObiColliderBase.idToCollider.Add(this.unityCollider.GetInstanceID(), this.unityCollider);
			this.CreateTracker();
			this.oniCollider = Oni.CreateCollider();
			this.FindSolvers(false);
			if (this.tracker != null)
			{
				Oni.SetColliderShape(this.oniCollider, this.tracker.OniShape);
			}
			ObiArbiter.OnFrameStart += this.UpdateIfNeeded;
			ObiArbiter.OnFrameEnd += this.UpdateRigidbody;
			this.UpdateColliderAdaptor();
			Oni.UpdateCollider(this.oniCollider, ref this.adaptor);
			if (this.material != null)
			{
				Oni.SetColliderMaterial(this.oniCollider, this.material.OniCollisionMaterial);
			}
			else
			{
				Oni.SetColliderMaterial(this.oniCollider, IntPtr.Zero);
			}
			this.CreateRigidbody();
		}

		// Token: 0x06001795 RID: 6037 RVA: 0x000866D0 File Offset: 0x00084AD0
		private void OnDestroy()
		{
			if (this.unityCollider != null)
			{
				ObiColliderBase.idToCollider.Remove(this.unityCollider.GetInstanceID());
			}
			ObiArbiter.OnFrameStart -= this.UpdateIfNeeded;
			ObiArbiter.OnFrameEnd -= this.UpdateRigidbody;
			Oni.DestroyCollider(this.oniCollider);
			this.oniCollider = IntPtr.Zero;
			if (this.tracker != null)
			{
				this.tracker.Destroy();
				this.tracker = null;
			}
		}

		// Token: 0x06001796 RID: 6038 RVA: 0x0008675C File Offset: 0x00084B5C
		public void OnEnable()
		{
			foreach (ObiSolver obiSolver in this.solvers)
			{
				Oni.AddCollider(obiSolver.OniSolver, this.oniCollider);
			}
		}

		// Token: 0x06001797 RID: 6039 RVA: 0x000867C4 File Offset: 0x00084BC4
		public void OnDisable()
		{
			foreach (ObiSolver obiSolver in this.solvers)
			{
				Oni.RemoveCollider(obiSolver.OniSolver, this.oniCollider);
			}
		}

		// Token: 0x06001798 RID: 6040 RVA: 0x0008682C File Offset: 0x00084C2C
		private void UpdateIfNeeded(object sender, EventArgs e)
		{
			if (this.unityCollider != null)
			{
				bool flag = this.IsUnityColliderEnabled();
				if (this.unityCollider.transform.hasChanged || (float)this.phase != this.oldPhase || this.thickness != this.oldThickness || flag != this.wasUnityColliderEnabled)
				{
					this.unityCollider.transform.hasChanged = false;
					this.oldPhase = (float)this.phase;
					this.oldThickness = this.thickness;
					this.wasUnityColliderEnabled = flag;
					foreach (ObiSolver obiSolver in this.solvers)
					{
						Oni.RemoveCollider(obiSolver.OniSolver, this.oniCollider);
					}
					this.UpdateColliderAdaptor();
					Oni.UpdateCollider(this.oniCollider, ref this.adaptor);
					if (flag)
					{
						foreach (ObiSolver obiSolver2 in this.solvers)
						{
							Oni.AddCollider(obiSolver2.OniSolver, this.oniCollider);
						}
					}
				}
			}
			if (this.tracker != null)
			{
				this.tracker.UpdateIfNeeded();
			}
			if (this.obiRigidbody != null)
			{
				this.obiRigidbody.UpdateIfNeeded();
			}
		}

		// Token: 0x06001799 RID: 6041 RVA: 0x000869C4 File Offset: 0x00084DC4
		private void UpdateRigidbody(object sender, EventArgs e)
		{
			if (this.obiRigidbody != null)
			{
				this.obiRigidbody.UpdateVelocities();
			}
		}

		// Token: 0x0600179A RID: 6042 RVA: 0x000869E2 File Offset: 0x00084DE2
		// Note: this type is marked as 'beforefieldinit'.
		static ObiColliderBase()
		{
		}

		// Token: 0x0400135C RID: 4956
		public static Dictionary<int, Component> idToCollider = new Dictionary<int, Component>();

		// Token: 0x0400135D RID: 4957
		[SerializeField]
		[HideInInspector]
		private ObiCollisionMaterial material;

		// Token: 0x0400135E RID: 4958
		public int phase;

		// Token: 0x0400135F RID: 4959
		public float thickness;

		// Token: 0x04001360 RID: 4960
		[HideInInspector]
		[SerializeField]
		protected Component unityCollider;

		// Token: 0x04001361 RID: 4961
		protected IntPtr oniCollider = IntPtr.Zero;

		// Token: 0x04001362 RID: 4962
		protected ObiRigidbody obiRigidbody;

		// Token: 0x04001363 RID: 4963
		protected int currentLayer = -1;

		// Token: 0x04001364 RID: 4964
		protected bool wasUnityColliderEnabled = true;

		// Token: 0x04001365 RID: 4965
		protected float oldPhase;

		// Token: 0x04001366 RID: 4966
		protected float oldThickness;

		// Token: 0x04001367 RID: 4967
		protected HashSet<ObiSolver> solvers = new HashSet<ObiSolver>();

		// Token: 0x04001368 RID: 4968
		protected ObiShapeTracker tracker;

		// Token: 0x04001369 RID: 4969
		protected Oni.Collider adaptor = default(Oni.Collider);
	}
}
