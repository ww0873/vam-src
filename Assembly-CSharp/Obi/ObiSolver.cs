using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003E7 RID: 999
	[ExecuteInEditMode]
	[AddComponentMenu("Physics/Obi/Obi Solver")]
	[DisallowMultipleComponent]
	public sealed class ObiSolver : MonoBehaviour
	{
		// Token: 0x06001946 RID: 6470 RVA: 0x0008BC4C File Offset: 0x0008A04C
		public ObiSolver()
		{
		}

		// Token: 0x14000083 RID: 131
		// (add) Token: 0x06001947 RID: 6471 RVA: 0x0008BD6C File Offset: 0x0008A16C
		// (remove) Token: 0x06001948 RID: 6472 RVA: 0x0008BDA4 File Offset: 0x0008A1A4
		public event EventHandler OnFrameBegin
		{
			add
			{
				EventHandler eventHandler = this.OnFrameBegin;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.OnFrameBegin, (EventHandler)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler eventHandler = this.OnFrameBegin;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.OnFrameBegin, (EventHandler)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x14000084 RID: 132
		// (add) Token: 0x06001949 RID: 6473 RVA: 0x0008BDDC File Offset: 0x0008A1DC
		// (remove) Token: 0x0600194A RID: 6474 RVA: 0x0008BE14 File Offset: 0x0008A214
		public event EventHandler OnStepBegin
		{
			add
			{
				EventHandler eventHandler = this.OnStepBegin;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.OnStepBegin, (EventHandler)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler eventHandler = this.OnStepBegin;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.OnStepBegin, (EventHandler)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x14000085 RID: 133
		// (add) Token: 0x0600194B RID: 6475 RVA: 0x0008BE4C File Offset: 0x0008A24C
		// (remove) Token: 0x0600194C RID: 6476 RVA: 0x0008BE84 File Offset: 0x0008A284
		public event EventHandler OnFixedParticlesUpdated
		{
			add
			{
				EventHandler eventHandler = this.OnFixedParticlesUpdated;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.OnFixedParticlesUpdated, (EventHandler)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler eventHandler = this.OnFixedParticlesUpdated;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.OnFixedParticlesUpdated, (EventHandler)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x14000086 RID: 134
		// (add) Token: 0x0600194D RID: 6477 RVA: 0x0008BEBC File Offset: 0x0008A2BC
		// (remove) Token: 0x0600194E RID: 6478 RVA: 0x0008BEF4 File Offset: 0x0008A2F4
		public event EventHandler OnStepEnd
		{
			add
			{
				EventHandler eventHandler = this.OnStepEnd;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.OnStepEnd, (EventHandler)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler eventHandler = this.OnStepEnd;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.OnStepEnd, (EventHandler)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x14000087 RID: 135
		// (add) Token: 0x0600194F RID: 6479 RVA: 0x0008BF2C File Offset: 0x0008A32C
		// (remove) Token: 0x06001950 RID: 6480 RVA: 0x0008BF64 File Offset: 0x0008A364
		public event EventHandler OnBeforePositionInterpolation
		{
			add
			{
				EventHandler eventHandler = this.OnBeforePositionInterpolation;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.OnBeforePositionInterpolation, (EventHandler)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler eventHandler = this.OnBeforePositionInterpolation;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.OnBeforePositionInterpolation, (EventHandler)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x14000088 RID: 136
		// (add) Token: 0x06001951 RID: 6481 RVA: 0x0008BF9C File Offset: 0x0008A39C
		// (remove) Token: 0x06001952 RID: 6482 RVA: 0x0008BFD4 File Offset: 0x0008A3D4
		public event EventHandler OnBeforeActorsFrameEnd
		{
			add
			{
				EventHandler eventHandler = this.OnBeforeActorsFrameEnd;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.OnBeforeActorsFrameEnd, (EventHandler)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler eventHandler = this.OnBeforeActorsFrameEnd;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.OnBeforeActorsFrameEnd, (EventHandler)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x14000089 RID: 137
		// (add) Token: 0x06001953 RID: 6483 RVA: 0x0008C00C File Offset: 0x0008A40C
		// (remove) Token: 0x06001954 RID: 6484 RVA: 0x0008C044 File Offset: 0x0008A444
		public event EventHandler OnFrameEnd
		{
			add
			{
				EventHandler eventHandler = this.OnFrameEnd;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.OnFrameEnd, (EventHandler)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler eventHandler = this.OnFrameEnd;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.OnFrameEnd, (EventHandler)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x1400008A RID: 138
		// (add) Token: 0x06001955 RID: 6485 RVA: 0x0008C07C File Offset: 0x0008A47C
		// (remove) Token: 0x06001956 RID: 6486 RVA: 0x0008C0B4 File Offset: 0x0008A4B4
		public event EventHandler<ObiSolver.ObiCollisionEventArgs> OnCollision
		{
			add
			{
				EventHandler<ObiSolver.ObiCollisionEventArgs> eventHandler = this.OnCollision;
				EventHandler<ObiSolver.ObiCollisionEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ObiSolver.ObiCollisionEventArgs>>(ref this.OnCollision, (EventHandler<ObiSolver.ObiCollisionEventArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<ObiSolver.ObiCollisionEventArgs> eventHandler = this.OnCollision;
				EventHandler<ObiSolver.ObiCollisionEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ObiSolver.ObiCollisionEventArgs>>(ref this.OnCollision, (EventHandler<ObiSolver.ObiCollisionEventArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x1400008B RID: 139
		// (add) Token: 0x06001957 RID: 6487 RVA: 0x0008C0EC File Offset: 0x0008A4EC
		// (remove) Token: 0x06001958 RID: 6488 RVA: 0x0008C124 File Offset: 0x0008A524
		public event EventHandler<ObiSolver.ObiFluidEventArgs> OnFluidUpdated
		{
			add
			{
				EventHandler<ObiSolver.ObiFluidEventArgs> eventHandler = this.OnFluidUpdated;
				EventHandler<ObiSolver.ObiFluidEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ObiSolver.ObiFluidEventArgs>>(ref this.OnFluidUpdated, (EventHandler<ObiSolver.ObiFluidEventArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<ObiSolver.ObiFluidEventArgs> eventHandler = this.OnFluidUpdated;
				EventHandler<ObiSolver.ObiFluidEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ObiSolver.ObiFluidEventArgs>>(ref this.OnFluidUpdated, (EventHandler<ObiSolver.ObiFluidEventArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06001959 RID: 6489 RVA: 0x0008C15A File Offset: 0x0008A55A
		public IntPtr OniSolver
		{
			get
			{
				return this.oniSolver;
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x0600195A RID: 6490 RVA: 0x0008C162 File Offset: 0x0008A562
		public Bounds Bounds
		{
			get
			{
				return this.bounds;
			}
		}

		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x0600195B RID: 6491 RVA: 0x0008C16A File Offset: 0x0008A56A
		public Matrix4x4 LastTransform
		{
			get
			{
				return this.lastTransform;
			}
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x0600195C RID: 6492 RVA: 0x0008C172 File Offset: 0x0008A572
		public bool IsVisible
		{
			get
			{
				return this.isVisible;
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x0600195D RID: 6493 RVA: 0x0008C17A File Offset: 0x0008A57A
		public int AllocParticleCount
		{
			get
			{
				return this.allocatedParticleCount;
			}
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x0600195E RID: 6494 RVA: 0x0008C182 File Offset: 0x0008A582
		public bool IsUpdating
		{
			get
			{
				return this.initialized && this.simulate && (this.simulateWhenInvisible || this.IsVisible);
			}
		}

		// Token: 0x0600195F RID: 6495 RVA: 0x0008C1B1 File Offset: 0x0008A5B1
		public void RequireRenderablePositions()
		{
			this.renderablePositionsClients++;
		}

		// Token: 0x06001960 RID: 6496 RVA: 0x0008C1C1 File Offset: 0x0008A5C1
		public void RelinquishRenderablePositions()
		{
			if (this.renderablePositionsClients > 0)
			{
				this.renderablePositionsClients--;
			}
		}

		// Token: 0x06001961 RID: 6497 RVA: 0x0008C1DD File Offset: 0x0008A5DD
		private void Awake()
		{
			this.lastTransform = base.transform.localToWorldMatrix;
			if (Application.isPlaying)
			{
				this.Initialize();
			}
		}

		// Token: 0x06001962 RID: 6498 RVA: 0x0008C200 File Offset: 0x0008A600
		private void Start()
		{
			if (Application.isPlaying)
			{
				ObiColliderBase[] array = UnityEngine.Object.FindObjectsOfType<ObiColliderBase>();
				foreach (ObiColliderBase obiColliderBase in array)
				{
					obiColliderBase.RegisterInSolver(this, true);
				}
			}
		}

		// Token: 0x06001963 RID: 6499 RVA: 0x0008C240 File Offset: 0x0008A640
		private void OnDestroy()
		{
			if (Application.isPlaying)
			{
				this.Teardown();
				ObiColliderBase[] array = UnityEngine.Object.FindObjectsOfType<ObiColliderBase>();
				foreach (ObiColliderBase obiColliderBase in array)
				{
					obiColliderBase.RemoveFromSolver(this);
				}
			}
		}

		// Token: 0x06001964 RID: 6500 RVA: 0x0008C284 File Offset: 0x0008A684
		private void OnEnable()
		{
			if (!Application.isPlaying)
			{
				this.Initialize();
			}
			base.StartCoroutine("RunLateFixedUpdate");
			ObiArbiter.RegisterSolver(this);
		}

		// Token: 0x06001965 RID: 6501 RVA: 0x0008C2A8 File Offset: 0x0008A6A8
		private void OnDisable()
		{
			if (!Application.isPlaying)
			{
				this.Teardown();
			}
			base.StopCoroutine("RunLateFixedUpdate");
			ObiArbiter.UnregisterSolver(this);
		}

		// Token: 0x06001966 RID: 6502 RVA: 0x0008C2CC File Offset: 0x0008A6CC
		public void Initialize()
		{
			this.Teardown();
			try
			{
				this.defaultFluidMaterial = ScriptableObject.CreateInstance<ObiEmitterMaterialFluid>();
				this.defaultFluidMaterial.hideFlags = HideFlags.HideAndDontSave;
				this.oniSolver = Oni.CreateSolver(this.maxParticles, 92);
				this.actors = new List<ObiActor>();
				this.activeParticles = new int[this.maxParticles];
				this.particleToActor = new ObiSolver.ParticleInActor[this.maxParticles];
				this.materialIndices = new int[this.maxParticles];
				this.fluidMaterialIndices = new int[this.maxParticles];
				this.renderablePositions = new Vector4[this.maxParticles];
				this.UpdateEmitterMaterials();
				this.UpdateParameters();
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
			finally
			{
				this.initialized = true;
			}
		}

		// Token: 0x06001967 RID: 6503 RVA: 0x0008C3AC File Offset: 0x0008A7AC
		private void Teardown()
		{
			if (!this.initialized)
			{
				return;
			}
			try
			{
				while (this.actors.Count > 0)
				{
					this.actors[this.actors.Count - 1].RemoveFromSolver(null);
				}
				Oni.DestroySolver(this.oniSolver);
				this.oniSolver = IntPtr.Zero;
				UnityEngine.Object.DestroyImmediate(this.defaultFluidMaterial);
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
			finally
			{
				this.initialized = false;
			}
		}

		// Token: 0x06001968 RID: 6504 RVA: 0x0008C450 File Offset: 0x0008A850
		public bool AddActor(ObiActor actor, int numParticles)
		{
			if (this.particleToActor == null || actor == null)
			{
				return false;
			}
			int[] array = new int[numParticles];
			int num = 0;
			int num2 = 0;
			while (num2 < this.maxParticles && num < numParticles)
			{
				if (this.particleToActor[num2] == null)
				{
					array[num] = num2;
					num++;
				}
				num2++;
			}
			if (num < numParticles)
			{
				return false;
			}
			this.allocatedParticleCount += numParticles;
			for (int i = 0; i < numParticles; i++)
			{
				this.particleToActor[array[i]] = new ObiSolver.ParticleInActor(actor, i);
			}
			actor.particleIndices = array;
			this.actors.Add(actor);
			this.UpdateActiveParticles();
			this.UpdateEmitterMaterials();
			return true;
		}

		// Token: 0x06001969 RID: 6505 RVA: 0x0008C50C File Offset: 0x0008A90C
		public int RemoveActor(ObiActor actor)
		{
			if (this.particleToActor == null || actor == null)
			{
				return -1;
			}
			int num = this.actors.IndexOf(actor);
			if (num > -1)
			{
				this.allocatedParticleCount -= actor.particleIndices.Length;
				for (int i = 0; i < actor.particleIndices.Length; i++)
				{
					this.particleToActor[actor.particleIndices[i]] = null;
				}
				this.actors.RemoveAt(num);
				this.UpdateActiveParticles();
				this.UpdateEmitterMaterials();
			}
			return num;
		}

		// Token: 0x0600196A RID: 6506 RVA: 0x0008C5A0 File Offset: 0x0008A9A0
		public void UpdateParameters()
		{
			Oni.SetSolverParameters(this.oniSolver, ref this.parameters);
			Oni.SetConstraintGroupParameters(this.oniSolver, 4, ref this.distanceConstraintParameters);
			Oni.SetConstraintGroupParameters(this.oniSolver, 3, ref this.bendingConstraintParameters);
			Oni.SetConstraintGroupParameters(this.oniSolver, 5, ref this.particleCollisionConstraintParameters);
			Oni.SetConstraintGroupParameters(this.oniSolver, 7, ref this.collisionConstraintParameters);
			Oni.SetConstraintGroupParameters(this.oniSolver, 6, ref this.densityConstraintParameters);
			Oni.SetConstraintGroupParameters(this.oniSolver, 8, ref this.skinConstraintParameters);
			Oni.SetConstraintGroupParameters(this.oniSolver, 2, ref this.volumeConstraintParameters);
			Oni.SetConstraintGroupParameters(this.oniSolver, 0, ref this.tetherConstraintParameters);
			Oni.SetConstraintGroupParameters(this.oniSolver, 1, ref this.pinConstraintParameters);
			Oni.SetConstraintGroupParameters(this.oniSolver, 10, ref this.stitchConstraintParameters);
			if (this.constraintsOrder == null || this.constraintsOrder.Length != 12)
			{
				this.constraintsOrder = Enumerable.Range(0, 12).ToArray<int>();
			}
			Oni.SetConstraintsOrder(this.oniSolver, this.constraintsOrder);
		}

		// Token: 0x0600196B RID: 6507 RVA: 0x0008C6B4 File Offset: 0x0008AAB4
		public void UpdateActiveParticles()
		{
			int num = 0;
			for (int i = 0; i < this.actors.Count; i++)
			{
				ObiActor obiActor = this.actors[i];
				if (obiActor.isActiveAndEnabled)
				{
					for (int j = 0; j < obiActor.particleIndices.Length; j++)
					{
						if (obiActor.active[j])
						{
							this.activeParticles[num] = obiActor.particleIndices[j];
							num++;
						}
					}
				}
			}
			Oni.SetActiveParticles(this.oniSolver, this.activeParticles, num);
		}

		// Token: 0x0600196C RID: 6508 RVA: 0x0008C744 File Offset: 0x0008AB44
		public void UpdateEmitterMaterials()
		{
			this.emitterMaterials = new List<ObiEmitterMaterial>
			{
				this.defaultFluidMaterial
			};
			foreach (ObiActor obiActor in this.actors)
			{
				ObiEmitter obiEmitter = obiActor as ObiEmitter;
				if (!(obiEmitter == null))
				{
					int num = 0;
					if (obiEmitter.EmitterMaterial != null)
					{
						num = this.emitterMaterials.IndexOf(obiEmitter.EmitterMaterial);
						if (num < 0)
						{
							num = this.emitterMaterials.Count;
							this.emitterMaterials.Add(obiEmitter.EmitterMaterial);
							obiEmitter.EmitterMaterial.OnChangesMade += this.emitterMaterial_OnChangesMade;
						}
					}
					for (int i = 0; i < obiActor.particleIndices.Length; i++)
					{
						this.fluidMaterialIndices[obiActor.particleIndices[i]] = num;
					}
				}
			}
			Oni.SetFluidMaterialIndices(this.oniSolver, this.fluidMaterialIndices, this.fluidMaterialIndices.Length, 0);
			Oni.FluidMaterial[] array = this.emitterMaterials.ConvertAll<Oni.FluidMaterial>(new Converter<ObiEmitterMaterial, Oni.FluidMaterial>(this.<UpdateEmitterMaterials>m__0)).ToArray();
			Oni.SetFluidMaterials(this.oniSolver, array, array.Length, 0);
		}

		// Token: 0x0600196D RID: 6509 RVA: 0x0008C8A8 File Offset: 0x0008ACA8
		private void emitterMaterial_OnChangesMade(object sender, ObiEmitterMaterial.MaterialChangeEventArgs e)
		{
			ObiEmitterMaterial obiEmitterMaterial = sender as ObiEmitterMaterial;
			int num = this.emitterMaterials.IndexOf(obiEmitterMaterial);
			if (num >= 0)
			{
				Oni.SetFluidMaterials(this.oniSolver, new Oni.FluidMaterial[]
				{
					obiEmitterMaterial.GetEquivalentOniMaterial(this.parameters.mode)
				}, 1, num);
			}
		}

		// Token: 0x0600196E RID: 6510 RVA: 0x0008C900 File Offset: 0x0008AD00
		public void AccumulateSimulationTime(float dt)
		{
			Oni.AddSimulationTime(this.oniSolver, dt);
		}

		// Token: 0x0600196F RID: 6511 RVA: 0x0008C90E File Offset: 0x0008AD0E
		public void ResetSimulationTime()
		{
			Oni.ResetSimulationTime(this.oniSolver);
		}

		// Token: 0x06001970 RID: 6512 RVA: 0x0008C91C File Offset: 0x0008AD1C
		public void SimulateStep(float stepTime)
		{
			Oni.ClearDiffuseParticles(this.oniSolver);
			if (this.OnStepBegin != null)
			{
				this.OnStepBegin(this, null);
			}
			foreach (ObiActor obiActor in this.actors)
			{
				obiActor.OnSolverStepBegin();
			}
			Oni.UpdateSkeletalAnimation(this.oniSolver);
			if (this.OnFixedParticlesUpdated != null)
			{
				this.OnFixedParticlesUpdated(this, null);
			}
			ObiArbiter.FrameStart();
			Oni.UpdateSolver(this.oniSolver, stepTime);
			ObiArbiter.WaitForAllSolvers();
		}

		// Token: 0x06001971 RID: 6513 RVA: 0x0008C9D4 File Offset: 0x0008ADD4
		public void EndFrame(float frameDelta)
		{
			foreach (ObiActor obiActor in this.actors)
			{
				obiActor.OnSolverPreInterpolation();
			}
			if (this.OnBeforePositionInterpolation != null)
			{
				this.OnBeforePositionInterpolation(this, null);
			}
			Oni.ApplyPositionInterpolation(this.oniSolver, frameDelta);
			if (this.renderablePositionsClients > 0)
			{
				Oni.GetRenderableParticlePositions(this.oniSolver, this.renderablePositions, this.renderablePositions.Length, 0);
				if (this.simulateInLocalSpace)
				{
					Matrix4x4 localToWorldMatrix = base.transform.localToWorldMatrix;
					for (int i = 0; i < this.renderablePositions.Length; i++)
					{
						this.renderablePositions[i] = localToWorldMatrix.MultiplyPoint3x4(this.renderablePositions[i]);
					}
				}
			}
			this.TriggerFluidUpdateEvents();
			if (this.OnBeforeActorsFrameEnd != null)
			{
				this.OnBeforeActorsFrameEnd(this, null);
			}
			this.CheckVisibility();
			foreach (ObiActor obiActor2 in this.actors)
			{
				obiActor2.OnSolverFrameEnd();
			}
		}

		// Token: 0x06001972 RID: 6514 RVA: 0x0008CB50 File Offset: 0x0008AF50
		private void TriggerFluidUpdateEvents()
		{
			int constraintCount = Oni.GetConstraintCount(this.oniSolver, 6);
			if (constraintCount > 0 && this.OnFluidUpdated != null)
			{
				int[] indices = new int[constraintCount];
				Vector4[] vorticities = new Vector4[this.maxParticles];
				float[] densities = new float[this.maxParticles];
				Oni.GetActiveConstraintIndices(this.oniSolver, indices, constraintCount, 6);
				Oni.GetParticleVorticities(this.oniSolver, vorticities, this.maxParticles, 0);
				Oni.GetParticleDensities(this.oniSolver, densities, this.maxParticles, 0);
				this.OnFluidUpdated(this, new ObiSolver.ObiFluidEventArgs(indices, vorticities, densities));
			}
		}

		// Token: 0x06001973 RID: 6515 RVA: 0x0008CBE8 File Offset: 0x0008AFE8
		private void TriggerCollisionEvents()
		{
			int constraintCount = Oni.GetConstraintCount(this.oniSolver, 7);
			if (this.OnCollision != null)
			{
				Oni.Contact[] contacts = new Oni.Contact[constraintCount];
				if (constraintCount > 0)
				{
					Oni.GetCollisionContacts(this.oniSolver, contacts, constraintCount);
				}
				this.OnCollision(this, new ObiSolver.ObiCollisionEventArgs(contacts));
			}
		}

		// Token: 0x06001974 RID: 6516 RVA: 0x0008CC3C File Offset: 0x0008B03C
		private bool AreBoundsValid(Bounds bounds)
		{
			return !float.IsNaN(bounds.center.x) && !float.IsInfinity(bounds.center.x) && !float.IsNaN(bounds.center.y) && !float.IsInfinity(bounds.center.y) && !float.IsNaN(bounds.center.z) && !float.IsInfinity(bounds.center.z);
		}

		// Token: 0x06001975 RID: 6517 RVA: 0x0008CCE4 File Offset: 0x0008B0E4
		private void CheckVisibility()
		{
			Vector3 zero = Vector3.zero;
			Vector3 zero2 = Vector3.zero;
			Oni.GetBounds(this.oniSolver, ref zero, ref zero2);
			this.bounds.SetMinMax(zero, zero2);
			this.isVisible = false;
			if (this.AreBoundsValid(this.bounds))
			{
				Bounds bounds = (!this.simulateInLocalSpace) ? this.bounds : this.bounds.Transform(base.transform.localToWorldMatrix);
				foreach (Camera camera in Camera.allCameras)
				{
					Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
					if (GeometryUtility.TestPlanesAABB(planes, bounds))
					{
						this.isVisible = true;
						return;
					}
				}
			}
		}

		// Token: 0x06001976 RID: 6518 RVA: 0x0008CDA4 File Offset: 0x0008B1A4
		private void Update()
		{
			if (!Application.isPlaying)
			{
				return;
			}
			if (this.OnFrameBegin != null)
			{
				this.OnFrameBegin(this, null);
			}
			foreach (ObiActor obiActor in this.actors)
			{
				obiActor.OnSolverFrameBegin();
			}
			if (this.IsUpdating && this.simulationOrder != ObiSolver.SimulationOrder.LateUpdate)
			{
				this.AccumulateSimulationTime(Time.deltaTime);
			}
		}

		// Token: 0x06001977 RID: 6519 RVA: 0x0008CE44 File Offset: 0x0008B244
		private IEnumerator RunLateFixedUpdate()
		{
			for (;;)
			{
				yield return new WaitForFixedUpdate();
				if (Application.isPlaying && this.IsUpdating && this.simulationOrder == ObiSolver.SimulationOrder.AfterFixedUpdate)
				{
					this.SimulateStep(Time.fixedDeltaTime);
				}
			}
			yield break;
		}

		// Token: 0x06001978 RID: 6520 RVA: 0x0008CE5F File Offset: 0x0008B25F
		private void FixedUpdate()
		{
			if (Application.isPlaying && this.IsUpdating && this.simulationOrder == ObiSolver.SimulationOrder.FixedUpdate)
			{
				this.SimulateStep(Time.fixedDeltaTime);
			}
		}

		// Token: 0x06001979 RID: 6521 RVA: 0x0008CE8C File Offset: 0x0008B28C
		public void AllSolversStepEnd()
		{
			this.TriggerCollisionEvents();
			foreach (ObiActor obiActor in this.actors)
			{
				obiActor.OnSolverStepEnd();
			}
			if (this.OnStepEnd != null)
			{
				this.OnStepEnd(this, null);
			}
			this.lastTransform = base.transform.localToWorldMatrix;
		}

		// Token: 0x0600197A RID: 6522 RVA: 0x0008CF18 File Offset: 0x0008B318
		private void LateUpdate()
		{
			if (Application.isPlaying && this.IsUpdating && this.simulationOrder == ObiSolver.SimulationOrder.LateUpdate)
			{
				this.smoothDelta = Mathf.Lerp(Time.deltaTime, this.smoothDelta, 0.95f);
				this.AccumulateSimulationTime(this.smoothDelta);
				this.SimulateStep(this.smoothDelta);
			}
			if (!Application.isPlaying)
			{
				return;
			}
			this.EndFrame((this.simulationOrder != ObiSolver.SimulationOrder.LateUpdate) ? Time.fixedDeltaTime : this.smoothDelta);
			if (this.OnFrameEnd != null)
			{
				this.OnFrameEnd(this, null);
			}
		}

		// Token: 0x0600197B RID: 6523 RVA: 0x0008CFBE File Offset: 0x0008B3BE
		[CompilerGenerated]
		private Oni.FluidMaterial <UpdateEmitterMaterials>m__0(ObiEmitterMaterial a)
		{
			return a.GetEquivalentOniMaterial(this.parameters.mode);
		}

		// Token: 0x0400149D RID: 5277
		public const int MAX_NEIGHBOURS = 92;

		// Token: 0x0400149E RID: 5278
		public const int CONSTRAINT_GROUPS = 12;

		// Token: 0x0400149F RID: 5279
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler OnFrameBegin;

		// Token: 0x040014A0 RID: 5280
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler OnStepBegin;

		// Token: 0x040014A1 RID: 5281
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler OnFixedParticlesUpdated;

		// Token: 0x040014A2 RID: 5282
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler OnStepEnd;

		// Token: 0x040014A3 RID: 5283
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler OnBeforePositionInterpolation;

		// Token: 0x040014A4 RID: 5284
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler OnBeforeActorsFrameEnd;

		// Token: 0x040014A5 RID: 5285
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler OnFrameEnd;

		// Token: 0x040014A6 RID: 5286
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler<ObiSolver.ObiCollisionEventArgs> OnCollision;

		// Token: 0x040014A7 RID: 5287
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler<ObiSolver.ObiFluidEventArgs> OnFluidUpdated;

		// Token: 0x040014A8 RID: 5288
		public int maxParticles = 5000;

		// Token: 0x040014A9 RID: 5289
		[HideInInspector]
		[NonSerialized]
		public bool simulate = true;

		// Token: 0x040014AA RID: 5290
		[Tooltip("If enabled, will force the solver to keep simulating even when not visible from any camera.")]
		public bool simulateWhenInvisible = true;

		// Token: 0x040014AB RID: 5291
		[Tooltip("If enabled, the solver object transform will be used as the frame of reference for all actors using this solver, instead of the world's frame.")]
		public bool simulateInLocalSpace;

		// Token: 0x040014AC RID: 5292
		[Tooltip("Determines when will the solver update particles.")]
		public ObiSolver.SimulationOrder simulationOrder;

		// Token: 0x040014AD RID: 5293
		public LayerMask collisionLayers = 1;

		// Token: 0x040014AE RID: 5294
		public Oni.SolverParameters parameters = new Oni.SolverParameters(Oni.SolverParameters.Interpolation.None, new Vector4(0f, -9.81f, 0f, 0f));

		// Token: 0x040014AF RID: 5295
		[HideInInspector]
		[NonSerialized]
		public List<ObiActor> actors = new List<ObiActor>();

		// Token: 0x040014B0 RID: 5296
		private int allocatedParticleCount;

		// Token: 0x040014B1 RID: 5297
		[HideInInspector]
		[NonSerialized]
		public ObiSolver.ParticleInActor[] particleToActor;

		// Token: 0x040014B2 RID: 5298
		[HideInInspector]
		[NonSerialized]
		public int[] materialIndices;

		// Token: 0x040014B3 RID: 5299
		[HideInInspector]
		[NonSerialized]
		public int[] fluidMaterialIndices;

		// Token: 0x040014B4 RID: 5300
		private int[] activeParticles;

		// Token: 0x040014B5 RID: 5301
		private List<ObiEmitterMaterial> emitterMaterials = new List<ObiEmitterMaterial>();

		// Token: 0x040014B6 RID: 5302
		[HideInInspector]
		[NonSerialized]
		public Vector4[] renderablePositions;

		// Token: 0x040014B7 RID: 5303
		[HideInInspector]
		public int[] constraintsOrder;

		// Token: 0x040014B8 RID: 5304
		public Oni.ConstraintParameters distanceConstraintParameters = new Oni.ConstraintParameters(true, Oni.ConstraintParameters.EvaluationOrder.Sequential, 3);

		// Token: 0x040014B9 RID: 5305
		public Oni.ConstraintParameters bendingConstraintParameters = new Oni.ConstraintParameters(true, Oni.ConstraintParameters.EvaluationOrder.Parallel, 3);

		// Token: 0x040014BA RID: 5306
		public Oni.ConstraintParameters particleCollisionConstraintParameters = new Oni.ConstraintParameters(true, Oni.ConstraintParameters.EvaluationOrder.Parallel, 3);

		// Token: 0x040014BB RID: 5307
		public Oni.ConstraintParameters collisionConstraintParameters = new Oni.ConstraintParameters(true, Oni.ConstraintParameters.EvaluationOrder.Parallel, 3);

		// Token: 0x040014BC RID: 5308
		public Oni.ConstraintParameters skinConstraintParameters = new Oni.ConstraintParameters(true, Oni.ConstraintParameters.EvaluationOrder.Sequential, 3);

		// Token: 0x040014BD RID: 5309
		public Oni.ConstraintParameters volumeConstraintParameters = new Oni.ConstraintParameters(true, Oni.ConstraintParameters.EvaluationOrder.Parallel, 3);

		// Token: 0x040014BE RID: 5310
		public Oni.ConstraintParameters tetherConstraintParameters = new Oni.ConstraintParameters(true, Oni.ConstraintParameters.EvaluationOrder.Parallel, 3);

		// Token: 0x040014BF RID: 5311
		public Oni.ConstraintParameters pinConstraintParameters = new Oni.ConstraintParameters(true, Oni.ConstraintParameters.EvaluationOrder.Parallel, 3);

		// Token: 0x040014C0 RID: 5312
		public Oni.ConstraintParameters stitchConstraintParameters = new Oni.ConstraintParameters(true, Oni.ConstraintParameters.EvaluationOrder.Parallel, 2);

		// Token: 0x040014C1 RID: 5313
		public Oni.ConstraintParameters densityConstraintParameters = new Oni.ConstraintParameters(true, Oni.ConstraintParameters.EvaluationOrder.Parallel, 2);

		// Token: 0x040014C2 RID: 5314
		private IntPtr oniSolver;

		// Token: 0x040014C3 RID: 5315
		private ObiEmitterMaterial defaultFluidMaterial;

		// Token: 0x040014C4 RID: 5316
		private Bounds bounds = default(Bounds);

		// Token: 0x040014C5 RID: 5317
		private Matrix4x4 lastTransform;

		// Token: 0x040014C6 RID: 5318
		private bool initialized;

		// Token: 0x040014C7 RID: 5319
		private bool isVisible = true;

		// Token: 0x040014C8 RID: 5320
		private float smoothDelta = 0.02f;

		// Token: 0x040014C9 RID: 5321
		private int renderablePositionsClients;

		// Token: 0x020003E8 RID: 1000
		public enum SimulationOrder
		{
			// Token: 0x040014CB RID: 5323
			FixedUpdate,
			// Token: 0x040014CC RID: 5324
			AfterFixedUpdate,
			// Token: 0x040014CD RID: 5325
			LateUpdate
		}

		// Token: 0x020003E9 RID: 1001
		public class ObiCollisionEventArgs : EventArgs
		{
			// Token: 0x0600197C RID: 6524 RVA: 0x0008CFD1 File Offset: 0x0008B3D1
			public ObiCollisionEventArgs(Oni.Contact[] contacts)
			{
				this.contacts = contacts;
			}

			// Token: 0x040014CE RID: 5326
			public Oni.Contact[] contacts;
		}

		// Token: 0x020003EA RID: 1002
		public class ObiFluidEventArgs : EventArgs
		{
			// Token: 0x0600197D RID: 6525 RVA: 0x0008CFE0 File Offset: 0x0008B3E0
			public ObiFluidEventArgs(int[] indices, Vector4[] vorticities, float[] densities)
			{
				this.indices = indices;
				this.vorticities = vorticities;
				this.densities = densities;
			}

			// Token: 0x040014CF RID: 5327
			public int[] indices;

			// Token: 0x040014D0 RID: 5328
			public Vector4[] vorticities;

			// Token: 0x040014D1 RID: 5329
			public float[] densities;
		}

		// Token: 0x020003EB RID: 1003
		public class ParticleInActor
		{
			// Token: 0x0600197E RID: 6526 RVA: 0x0008CFFD File Offset: 0x0008B3FD
			public ParticleInActor(ObiActor actor, int indexInActor)
			{
				this.actor = actor;
				this.indexInActor = indexInActor;
			}

			// Token: 0x040014D2 RID: 5330
			public ObiActor actor;

			// Token: 0x040014D3 RID: 5331
			public int indexInActor;
		}

		// Token: 0x02000F46 RID: 3910
		[CompilerGenerated]
		private sealed class <RunLateFixedUpdate>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007364 RID: 29540 RVA: 0x0008D013 File Offset: 0x0008B413
			[DebuggerHidden]
			public <RunLateFixedUpdate>c__Iterator0()
			{
			}

			// Token: 0x06007365 RID: 29541 RVA: 0x0008D01C File Offset: 0x0008B41C
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					break;
				case 1U:
					if (Application.isPlaying && base.IsUpdating && this.simulationOrder == ObiSolver.SimulationOrder.AfterFixedUpdate)
					{
						base.SimulateStep(Time.fixedDeltaTime);
					}
					break;
				default:
					return false;
				}
				this.$current = new WaitForFixedUpdate();
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			}

			// Token: 0x170010E7 RID: 4327
			// (get) Token: 0x06007366 RID: 29542 RVA: 0x0008D0B3 File Offset: 0x0008B4B3
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010E8 RID: 4328
			// (get) Token: 0x06007367 RID: 29543 RVA: 0x0008D0BB File Offset: 0x0008B4BB
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007368 RID: 29544 RVA: 0x0008D0C3 File Offset: 0x0008B4C3
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007369 RID: 29545 RVA: 0x0008D0D3 File Offset: 0x0008B4D3
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x0400675B RID: 26459
			internal ObiSolver $this;

			// Token: 0x0400675C RID: 26460
			internal object $current;

			// Token: 0x0400675D RID: 26461
			internal bool $disposing;

			// Token: 0x0400675E RID: 26462
			internal int $PC;
		}
	}
}
