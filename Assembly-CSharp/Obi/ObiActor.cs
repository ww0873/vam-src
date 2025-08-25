using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Obi
{
	// Token: 0x0200038E RID: 910
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	public abstract class ObiActor : MonoBehaviour, IObiSolverClient
	{
		// Token: 0x060016A3 RID: 5795 RVA: 0x0007F640 File Offset: 0x0007DA40
		protected ObiActor()
		{
		}

		// Token: 0x1400007E RID: 126
		// (add) Token: 0x060016A4 RID: 5796 RVA: 0x0007F660 File Offset: 0x0007DA60
		// (remove) Token: 0x060016A5 RID: 5797 RVA: 0x0007F698 File Offset: 0x0007DA98
		public event EventHandler<ObiActor.ObiActorSolverArgs> OnAddedToSolver
		{
			add
			{
				EventHandler<ObiActor.ObiActorSolverArgs> eventHandler = this.OnAddedToSolver;
				EventHandler<ObiActor.ObiActorSolverArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ObiActor.ObiActorSolverArgs>>(ref this.OnAddedToSolver, (EventHandler<ObiActor.ObiActorSolverArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<ObiActor.ObiActorSolverArgs> eventHandler = this.OnAddedToSolver;
				EventHandler<ObiActor.ObiActorSolverArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ObiActor.ObiActorSolverArgs>>(ref this.OnAddedToSolver, (EventHandler<ObiActor.ObiActorSolverArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x1400007F RID: 127
		// (add) Token: 0x060016A6 RID: 5798 RVA: 0x0007F6D0 File Offset: 0x0007DAD0
		// (remove) Token: 0x060016A7 RID: 5799 RVA: 0x0007F708 File Offset: 0x0007DB08
		public event EventHandler<ObiActor.ObiActorSolverArgs> OnRemovedFromSolver
		{
			add
			{
				EventHandler<ObiActor.ObiActorSolverArgs> eventHandler = this.OnRemovedFromSolver;
				EventHandler<ObiActor.ObiActorSolverArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ObiActor.ObiActorSolverArgs>>(ref this.OnRemovedFromSolver, (EventHandler<ObiActor.ObiActorSolverArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<ObiActor.ObiActorSolverArgs> eventHandler = this.OnRemovedFromSolver;
				EventHandler<ObiActor.ObiActorSolverArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ObiActor.ObiActorSolverArgs>>(ref this.OnRemovedFromSolver, (EventHandler<ObiActor.ObiActorSolverArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x060016A8 RID: 5800 RVA: 0x0007F73E File Offset: 0x0007DB3E
		// (set) Token: 0x060016A9 RID: 5801 RVA: 0x0007F746 File Offset: 0x0007DB46
		public ObiSolver Solver
		{
			get
			{
				return this.solver;
			}
			set
			{
				if (this.solver != value)
				{
					this.RemoveFromSolver(null);
					this.solver = value;
				}
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x060016AA RID: 5802 RVA: 0x0007F768 File Offset: 0x0007DB68
		// (set) Token: 0x060016AB RID: 5803 RVA: 0x0007F770 File Offset: 0x0007DB70
		public ObiCollisionMaterial CollisionMaterial
		{
			get
			{
				return this.collisionMaterial;
			}
			set
			{
				if (this.collisionMaterial != value)
				{
					this.collisionMaterial = value;
					this.PushDataToSolver(ParticleData.COLLISION_MATERIAL);
				}
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x060016AC RID: 5804 RVA: 0x0007F795 File Offset: 0x0007DB95
		public bool Initializing
		{
			get
			{
				return this.initializing;
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x060016AD RID: 5805 RVA: 0x0007F79D File Offset: 0x0007DB9D
		public bool Initialized
		{
			get
			{
				return this.initialized;
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x060016AE RID: 5806 RVA: 0x0007F7A5 File Offset: 0x0007DBA5
		public bool InSolver
		{
			get
			{
				return this.inSolver;
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x060016AF RID: 5807 RVA: 0x0007F7AD File Offset: 0x0007DBAD
		// (set) Token: 0x060016B0 RID: 5808 RVA: 0x0007F7B5 File Offset: 0x0007DBB5
		public virtual bool SelfCollisions
		{
			get
			{
				return this.selfCollisions;
			}
			set
			{
				if (value != this.selfCollisions)
				{
					this.selfCollisions = value;
					this.UpdateParticlePhases();
				}
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x060016B1 RID: 5809 RVA: 0x0007F7D0 File Offset: 0x0007DBD0
		public virtual Matrix4x4 ActorLocalToWorldMatrix
		{
			get
			{
				return base.transform.localToWorldMatrix;
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x060016B2 RID: 5810 RVA: 0x0007F7DD File Offset: 0x0007DBDD
		public virtual Matrix4x4 ActorWorldToLocalMatrix
		{
			get
			{
				return base.transform.worldToLocalMatrix;
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x060016B3 RID: 5811 RVA: 0x0007F7EA File Offset: 0x0007DBEA
		public virtual bool UsesCustomExternalForces
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060016B4 RID: 5812 RVA: 0x0007F7ED File Offset: 0x0007DBED
		public virtual void Awake()
		{
		}

		// Token: 0x060016B5 RID: 5813 RVA: 0x0007F7EF File Offset: 0x0007DBEF
		public virtual void Start()
		{
			if (Application.isPlaying)
			{
				this.AddToSolver(null);
			}
		}

		// Token: 0x060016B6 RID: 5814 RVA: 0x0007F803 File Offset: 0x0007DC03
		public virtual void OnDestroy()
		{
			this.RemoveFromSolver(null);
		}

		// Token: 0x060016B7 RID: 5815 RVA: 0x0007F80D File Offset: 0x0007DC0D
		public virtual void DestroyRequiredComponents()
		{
		}

		// Token: 0x060016B8 RID: 5816 RVA: 0x0007F810 File Offset: 0x0007DC10
		public virtual void OnEnable()
		{
			this.constraints.Clear();
			ObiBatchedConstraints[] components = base.GetComponents<ObiBatchedConstraints>();
			foreach (ObiBatchedConstraints obiBatchedConstraints in components)
			{
				this.constraints[obiBatchedConstraints.GetConstraintType()] = obiBatchedConstraints;
				obiBatchedConstraints.GrabActor();
				if (obiBatchedConstraints.isActiveAndEnabled)
				{
					obiBatchedConstraints.OnEnable();
				}
			}
			if (!this.InSolver)
			{
				return;
			}
			this.solver.UpdateActiveParticles();
		}

		// Token: 0x060016B9 RID: 5817 RVA: 0x0007F88C File Offset: 0x0007DC8C
		public virtual void OnDisable()
		{
			if (!this.InSolver)
			{
				return;
			}
			this.solver.UpdateActiveParticles();
			this.PullDataFromSolver(ParticleData.POSITIONS | ParticleData.VELOCITIES);
			foreach (ObiBatchedConstraints obiBatchedConstraints in this.constraints.Values)
			{
				obiBatchedConstraints.OnDisable();
			}
		}

		// Token: 0x060016BA RID: 5818 RVA: 0x0007F90C File Offset: 0x0007DD0C
		public virtual void ResetActor()
		{
		}

		// Token: 0x060016BB RID: 5819 RVA: 0x0007F910 File Offset: 0x0007DD10
		public virtual void UpdateParticlePhases()
		{
			if (!this.InSolver)
			{
				return;
			}
			for (int i = 0; i < this.phases.Length; i++)
			{
				this.phases[i] = Oni.MakePhase(Oni.GetGroupFromPhase(this.phases[i]), (!this.selfCollisions) ? ((Oni.ParticlePhase)0) : Oni.ParticlePhase.SelfCollide);
			}
			this.PushDataToSolver(ParticleData.PHASES);
		}

		// Token: 0x060016BC RID: 5820 RVA: 0x0007F980 File Offset: 0x0007DD80
		public virtual bool AddToSolver(object info)
		{
			if (!(this.solver != null) || this.InSolver)
			{
				return false;
			}
			if (!this.solver.AddActor(this, this.positions.Length))
			{
				UnityEngine.Debug.LogWarning("Obi: Solver could not allocate enough particles for this actor. Please increase max particles.");
				return false;
			}
			this.inSolver = true;
			this.UpdateParticlePhases();
			this.trianglesOffset = Oni.GetDeformableTriangleCount(this.solver.OniSolver);
			this.UpdateDeformableTriangles();
			this.PushDataToSolver(ParticleData.ALL);
			foreach (ObiBatchedConstraints obiBatchedConstraints in this.constraints.Values)
			{
				obiBatchedConstraints.AddToSolver(null);
			}
			if (this.OnAddedToSolver != null)
			{
				this.OnAddedToSolver(this, new ObiActor.ObiActorSolverArgs(this.solver));
			}
			return true;
		}

		// Token: 0x060016BD RID: 5821 RVA: 0x0007FA78 File Offset: 0x0007DE78
		public void UpdateDeformableTriangles()
		{
			int[] array = new int[this.deformableTriangles.Length];
			for (int i = 0; i < this.deformableTriangles.Length; i++)
			{
				array[i] = this.particleIndices[this.deformableTriangles[i]];
			}
			Oni.SetDeformableTriangles(this.solver.OniSolver, array, array.Length / 3, this.trianglesOffset);
		}

		// Token: 0x060016BE RID: 5822 RVA: 0x0007FADC File Offset: 0x0007DEDC
		public virtual bool RemoveFromSolver(object info)
		{
			if (this.solver != null && this.InSolver)
			{
				foreach (ObiBatchedConstraints obiBatchedConstraints in this.constraints.Values)
				{
					obiBatchedConstraints.RemoveFromSolver(null);
				}
				Vector4[] array = new Vector4[]
				{
					new Vector4(0f, 0f, 0f, 1f)
				};
				for (int i = 0; i < this.particleIndices.Length; i++)
				{
					Oni.SetRestPositions(this.solver.OniSolver, array, 1, this.particleIndices[i]);
				}
				int num = this.solver.RemoveActor(this);
				this.particleIndices = null;
				for (int j = num; j < this.solver.actors.Count; j++)
				{
					this.solver.actors[j].trianglesOffset -= this.deformableTriangles.Length / 3;
				}
				Oni.RemoveDeformableTriangles(this.solver.OniSolver, this.deformableTriangles.Length / 3, this.trianglesOffset);
				this.inSolver = false;
				if (this.OnRemovedFromSolver != null)
				{
					this.OnRemovedFromSolver(this, new ObiActor.ObiActorSolverArgs(this.solver));
				}
				return true;
			}
			return false;
		}

		// Token: 0x060016BF RID: 5823 RVA: 0x0007FC6C File Offset: 0x0007E06C
		public virtual void PushDataToSolver(ParticleData data = ParticleData.NONE)
		{
			if (!this.InSolver)
			{
				return;
			}
			Matrix4x4 matrix4x;
			if (this.Solver.simulateInLocalSpace)
			{
				matrix4x = this.Solver.transform.worldToLocalMatrix * this.ActorLocalToWorldMatrix;
			}
			else
			{
				matrix4x = this.ActorLocalToWorldMatrix;
			}
			for (int i = 0; i < this.particleIndices.Length; i++)
			{
				int destOffset = this.particleIndices[i];
				if ((data & ParticleData.POSITIONS) != ParticleData.NONE && i < this.positions.Length)
				{
					Oni.SetParticlePositions(this.solver.OniSolver, new Vector4[]
					{
						matrix4x.MultiplyPoint3x4(this.positions[i])
					}, 1, destOffset);
				}
				if ((data & ParticleData.VELOCITIES) != ParticleData.NONE && i < this.velocities.Length)
				{
					Oni.SetParticleVelocities(this.solver.OniSolver, new Vector4[]
					{
						matrix4x.MultiplyVector(this.velocities[i])
					}, 1, destOffset);
				}
				if ((data & ParticleData.INV_MASSES) != ParticleData.NONE && i < this.invMasses.Length)
				{
					Oni.SetParticleInverseMasses(this.solver.OniSolver, new float[]
					{
						this.invMasses[i]
					}, 1, destOffset);
				}
				if ((data & ParticleData.SOLID_RADII) != ParticleData.NONE && i < this.solidRadii.Length)
				{
					Oni.SetParticleSolidRadii(this.solver.OniSolver, new float[]
					{
						this.solidRadii[i]
					}, 1, destOffset);
				}
				if ((data & ParticleData.PHASES) != ParticleData.NONE && i < this.phases.Length)
				{
					Oni.SetParticlePhases(this.solver.OniSolver, new int[]
					{
						this.phases[i]
					}, 1, destOffset);
				}
				if ((data & ParticleData.REST_POSITIONS) != ParticleData.NONE && i < this.restPositions.Length)
				{
					Oni.SetRestPositions(this.solver.OniSolver, new Vector4[]
					{
						this.restPositions[i]
					}, 1, destOffset);
				}
			}
			if ((data & ParticleData.COLLISION_MATERIAL) != ParticleData.NONE)
			{
				IntPtr[] array = new IntPtr[this.particleIndices.Length];
				for (int j = 0; j < this.particleIndices.Length; j++)
				{
					array[j] = ((!(this.collisionMaterial != null)) ? IntPtr.Zero : this.collisionMaterial.OniCollisionMaterial);
				}
				Oni.SetCollisionMaterials(this.solver.OniSolver, array, this.particleIndices, this.particleIndices.Length);
			}
			if ((data & ParticleData.ACTIVE_STATUS) != ParticleData.NONE)
			{
				this.solver.UpdateActiveParticles();
			}
		}

		// Token: 0x060016C0 RID: 5824 RVA: 0x0007FF2C File Offset: 0x0007E32C
		public virtual void PullDataFromSolver(ParticleData data = ParticleData.NONE)
		{
			if (!this.InSolver)
			{
				return;
			}
			for (int i = 0; i < this.particleIndices.Length; i++)
			{
				int sourceOffset = this.particleIndices[i];
				if ((data & ParticleData.POSITIONS) != ParticleData.NONE)
				{
					Vector4[] array = new Vector4[]
					{
						this.positions[i]
					};
					Oni.GetParticlePositions(this.solver.OniSolver, array, 1, sourceOffset);
					this.positions[i] = base.transform.InverseTransformPoint(array[0]);
				}
				if ((data & ParticleData.VELOCITIES) != ParticleData.NONE)
				{
					Vector4[] array2 = new Vector4[]
					{
						this.velocities[i]
					};
					Oni.GetParticleVelocities(this.solver.OniSolver, array2, 1, sourceOffset);
					this.velocities[i] = base.transform.InverseTransformVector(array2[0]);
				}
			}
		}

		// Token: 0x060016C1 RID: 5825 RVA: 0x0008004C File Offset: 0x0007E44C
		public Vector3 GetParticlePosition(int index)
		{
			if (this.InSolver)
			{
				return this.solver.renderablePositions[this.particleIndices[index]];
			}
			return this.ActorLocalToWorldMatrix.MultiplyPoint3x4(this.positions[index]);
		}

		// Token: 0x060016C2 RID: 5826 RVA: 0x000800A6 File Offset: 0x0007E4A6
		public virtual bool GenerateTethers(ObiActor.TetherType type)
		{
			return true;
		}

		// Token: 0x060016C3 RID: 5827 RVA: 0x000800AC File Offset: 0x0007E4AC
		public void ClearTethers()
		{
			ObiBatchedConstraints obiBatchedConstraints;
			if (this.constraints.TryGetValue(Oni.ConstraintType.Tether, out obiBatchedConstraints))
			{
				((ObiTetherConstraints)obiBatchedConstraints).Clear();
			}
		}

		// Token: 0x060016C4 RID: 5828 RVA: 0x000800D7 File Offset: 0x0007E4D7
		public virtual void OnSolverPreInterpolation()
		{
		}

		// Token: 0x060016C5 RID: 5829 RVA: 0x000800DC File Offset: 0x0007E4DC
		public virtual void OnSolverStepBegin()
		{
			if (!base.transform.hasChanged && !this.Solver.transform.hasChanged)
			{
				return;
			}
			base.transform.hasChanged = false;
			this.Solver.transform.hasChanged = false;
			bool enabled = base.enabled;
			int num = this.particleIndices.Length;
			Vector4[] array = new Vector4[]
			{
				Vector4.zero
			};
			Matrix4x4 matrix4x;
			if (this.Solver.simulateInLocalSpace)
			{
				matrix4x = this.Solver.transform.worldToLocalMatrix * this.ActorLocalToWorldMatrix;
			}
			else
			{
				matrix4x = this.ActorLocalToWorldMatrix;
			}
			Matrix4x4 matrix4x2 = this.Solver.transform.worldToLocalMatrix * this.Solver.LastTransform;
			for (int i = 0; i < num; i++)
			{
				if (!enabled || this.invMasses[i] == 0f)
				{
					array[0] = matrix4x.MultiplyPoint3x4(this.positions[i]);
					Oni.SetParticlePositions(this.solver.OniSolver, array, 1, this.particleIndices[i]);
				}
				else if (this.Solver.simulateInLocalSpace)
				{
					Oni.GetParticlePositions(this.solver.OniSolver, array, 1, this.particleIndices[i]);
					array[0] = Vector3.Lerp(array[0], matrix4x2.MultiplyPoint3x4(array[0]), this.worldVelocityScale);
					Oni.SetParticlePositions(this.solver.OniSolver, array, 1, this.particleIndices[i]);
				}
			}
		}

		// Token: 0x060016C6 RID: 5830 RVA: 0x000802B5 File Offset: 0x0007E6B5
		public virtual void OnSolverStepEnd()
		{
		}

		// Token: 0x060016C7 RID: 5831 RVA: 0x000802B7 File Offset: 0x0007E6B7
		public virtual void OnSolverFrameBegin()
		{
		}

		// Token: 0x060016C8 RID: 5832 RVA: 0x000802B9 File Offset: 0x0007E6B9
		public virtual void OnSolverFrameEnd()
		{
		}

		// Token: 0x060016C9 RID: 5833 RVA: 0x000802BB File Offset: 0x0007E6BB
		public virtual bool ReadParticlePropertyFromTexture(Texture2D source, Action<int, Color> onReadProperty)
		{
			return false;
		}

		// Token: 0x040012D4 RID: 4820
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler<ObiActor.ObiActorSolverArgs> OnAddedToSolver;

		// Token: 0x040012D5 RID: 4821
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler<ObiActor.ObiActorSolverArgs> OnRemovedFromSolver;

		// Token: 0x040012D6 RID: 4822
		[Range(0f, 1f)]
		public float worldVelocityScale;

		// Token: 0x040012D7 RID: 4823
		[HideInInspector]
		public ObiCollisionMaterial collisionMaterial;

		// Token: 0x040012D8 RID: 4824
		[HideInInspector]
		[NonSerialized]
		public int[] particleIndices;

		// Token: 0x040012D9 RID: 4825
		protected Dictionary<Oni.ConstraintType, ObiBatchedConstraints> constraints = new Dictionary<Oni.ConstraintType, ObiBatchedConstraints>();

		// Token: 0x040012DA RID: 4826
		[HideInInspector]
		public bool[] active;

		// Token: 0x040012DB RID: 4827
		[HideInInspector]
		public Vector3[] positions;

		// Token: 0x040012DC RID: 4828
		[HideInInspector]
		public Vector4[] restPositions;

		// Token: 0x040012DD RID: 4829
		[HideInInspector]
		public Vector3[] velocities;

		// Token: 0x040012DE RID: 4830
		[HideInInspector]
		public float[] invMasses;

		// Token: 0x040012DF RID: 4831
		[HideInInspector]
		public float[] solidRadii;

		// Token: 0x040012E0 RID: 4832
		[HideInInspector]
		public int[] phases;

		// Token: 0x040012E1 RID: 4833
		[HideInInspector]
		public Color[] colors;

		// Token: 0x040012E2 RID: 4834
		[HideInInspector]
		public int[] deformableTriangles = new int[0];

		// Token: 0x040012E3 RID: 4835
		[NonSerialized]
		protected int trianglesOffset;

		// Token: 0x040012E4 RID: 4836
		private bool inSolver;

		// Token: 0x040012E5 RID: 4837
		protected bool initializing;

		// Token: 0x040012E6 RID: 4838
		[HideInInspector]
		[SerializeField]
		protected ObiSolver solver;

		// Token: 0x040012E7 RID: 4839
		[HideInInspector]
		[SerializeField]
		protected bool selfCollisions;

		// Token: 0x040012E8 RID: 4840
		[HideInInspector]
		[SerializeField]
		protected bool initialized;

		// Token: 0x0200038F RID: 911
		public class ObiActorSolverArgs : EventArgs
		{
			// Token: 0x060016CA RID: 5834 RVA: 0x000802BE File Offset: 0x0007E6BE
			public ObiActorSolverArgs(ObiSolver solver)
			{
				this.solver = solver;
			}

			// Token: 0x17000293 RID: 659
			// (get) Token: 0x060016CB RID: 5835 RVA: 0x000802CD File Offset: 0x0007E6CD
			public ObiSolver Solver
			{
				get
				{
					return this.solver;
				}
			}

			// Token: 0x040012E9 RID: 4841
			private ObiSolver solver;
		}

		// Token: 0x02000390 RID: 912
		public enum TetherType
		{
			// Token: 0x040012EB RID: 4843
			AnchorToFixed,
			// Token: 0x040012EC RID: 4844
			Hierarchical
		}
	}
}
