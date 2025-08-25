using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Obi
{
	// Token: 0x02000391 RID: 913
	[ExecuteInEditMode]
	[AddComponentMenu("Physics/Obi/Obi Bone")]
	[RequireComponent(typeof(ObiDistanceConstraints))]
	[RequireComponent(typeof(ObiBendingConstraints))]
	[RequireComponent(typeof(ObiSkinConstraints))]
	public class ObiBone : ObiActor
	{
		// Token: 0x060016CC RID: 5836 RVA: 0x000802D5 File Offset: 0x0007E6D5
		public ObiBone()
		{
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x060016CD RID: 5837 RVA: 0x000802E8 File Offset: 0x0007E6E8
		public ObiSkinConstraints SkinConstraints
		{
			get
			{
				return this.constraints[Oni.ConstraintType.Skin] as ObiSkinConstraints;
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x060016CE RID: 5838 RVA: 0x000802FB File Offset: 0x0007E6FB
		public ObiDistanceConstraints DistanceConstraints
		{
			get
			{
				return this.constraints[Oni.ConstraintType.Distance] as ObiDistanceConstraints;
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x060016CF RID: 5839 RVA: 0x0008030E File Offset: 0x0007E70E
		public ObiBendingConstraints BendingConstraints
		{
			get
			{
				return this.constraints[Oni.ConstraintType.Bending] as ObiBendingConstraints;
			}
		}

		// Token: 0x060016D0 RID: 5840 RVA: 0x00080321 File Offset: 0x0007E721
		public override void Awake()
		{
			base.Awake();
			this.SetupAnimatorController();
		}

		// Token: 0x060016D1 RID: 5841 RVA: 0x0008032F File Offset: 0x0007E72F
		public void OnValidate()
		{
			this.particleRadius = Mathf.Max(0f, this.particleRadius);
		}

		// Token: 0x060016D2 RID: 5842 RVA: 0x00080347 File Offset: 0x0007E747
		public override void OnSolverFrameEnd()
		{
			base.OnSolverFrameEnd();
			this.UpdateBones();
		}

		// Token: 0x060016D3 RID: 5843 RVA: 0x00080355 File Offset: 0x0007E755
		public override bool AddToSolver(object info)
		{
			if (base.Initialized && base.AddToSolver(info))
			{
				this.solver.RequireRenderablePositions();
				return true;
			}
			return false;
		}

		// Token: 0x060016D4 RID: 5844 RVA: 0x0008037C File Offset: 0x0007E77C
		public override bool RemoveFromSolver(object info)
		{
			if (this.solver != null)
			{
				this.solver.RelinquishRenderablePositions();
			}
			return base.RemoveFromSolver(info);
		}

		// Token: 0x060016D5 RID: 5845 RVA: 0x000803A4 File Offset: 0x0007E7A4
		private void SetupAnimatorController()
		{
			Animator componentInParent = base.GetComponentInParent<Animator>();
			if (componentInParent != null)
			{
				this.animatorController = componentInParent.GetComponent<ObiAnimatorController>();
				if (this.animatorController == null)
				{
					this.animatorController = componentInParent.gameObject.AddComponent<ObiAnimatorController>();
				}
			}
		}

		// Token: 0x060016D6 RID: 5846 RVA: 0x000803F4 File Offset: 0x0007E7F4
		private IEnumerable EnumerateBonesBreadthFirst()
		{
			int count = 0;
			Queue<Transform> queue = new Queue<Transform>();
			queue.Enqueue(base.transform);
			while (queue.Count > 0)
			{
				Transform current = queue.Dequeue();
				if (current != null)
				{
					count++;
					yield return current;
					IEnumerator enumerator = current.transform.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							Transform item = (Transform)obj;
							queue.Enqueue(item);
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
					if (current == base.transform && count > 1)
					{
						yield return null;
					}
				}
			}
			yield break;
		}

		// Token: 0x060016D7 RID: 5847 RVA: 0x00080418 File Offset: 0x0007E818
		public IEnumerator GeneratePhysicRepresentationForBones()
		{
			this.initialized = false;
			this.initializing = true;
			this.RemoveFromSolver(null);
			this.bones = new List<Transform>();
			IEnumerator enumerator = this.EnumerateBonesBreadthFirst().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform item = (Transform)obj;
					this.bones.Add(item);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			this.parentIndices = new int[this.bones.Count];
			this.active = new bool[this.bones.Count];
			this.positions = new Vector3[this.bones.Count];
			this.velocities = new Vector3[this.bones.Count];
			this.invMasses = new float[this.bones.Count];
			this.solidRadii = new float[this.bones.Count];
			this.phases = new int[this.bones.Count];
			this.restPositions = new Vector4[this.bones.Count];
			this.frozen = new bool[this.bones.Count];
			this.DistanceConstraints.Clear();
			ObiDistanceConstraintBatch distanceBatch = new ObiDistanceConstraintBatch(false, false, 0.0001f, 20f);
			this.DistanceConstraints.AddBatch(distanceBatch);
			this.BendingConstraints.Clear();
			ObiBendConstraintBatch bendingBatch = new ObiBendConstraintBatch(false, false, 0.0001f, 20f);
			this.BendingConstraints.AddBatch(bendingBatch);
			this.SkinConstraints.Clear();
			ObiSkinConstraintBatch skinBatch = new ObiSkinConstraintBatch(true, false, 0.0001f, 20f);
			this.SkinConstraints.AddBatch(skinBatch);
			for (int i = 0; i < this.bones.Count; i++)
			{
				this.active[i] = true;
				this.invMasses[i] = 10f;
				this.positions[i] = base.transform.InverseTransformPoint(this.bones[i].position);
				this.restPositions[i] = this.positions[i];
				this.restPositions[i][3] = 0f;
				this.solidRadii[i] = this.particleRadius;
				this.frozen[i] = false;
				this.phases[i] = Oni.MakePhase(1, (!this.selfCollisions) ? ((Oni.ParticlePhase)0) : Oni.ParticlePhase.SelfCollide);
				this.parentIndices[i] = -1;
				if (this.bones[i].parent != null)
				{
					this.parentIndices[i] = this.bones.IndexOf(this.bones[i].parent);
				}
				skinBatch.AddConstraint(i, this.positions[i], Vector3.up, 0.05f, 0f, 0f, 1f);
				IEnumerator enumerator2 = this.bones[i].GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						object obj2 = enumerator2.Current;
						Transform transform = (Transform)obj2;
						int num = this.bones.IndexOf(transform);
						if (num >= 0)
						{
							distanceBatch.AddConstraint(i, num, Vector3.Distance(this.bones[i].position, transform.position), 1f, 1f);
							if (this.parentIndices[i] >= 0)
							{
								Transform transform2 = this.bones[this.parentIndices[i]];
								float[] constraintCoordinates = new float[]
								{
									transform2.position[0],
									transform2.position[1],
									transform2.position[2],
									transform.position[0],
									transform.position[1],
									transform.position[2],
									this.bones[i].position[0],
									this.bones[i].position[1],
									this.bones[i].position[2]
								};
								float restBend = Oni.BendingConstraintRest(constraintCoordinates);
								bendingBatch.AddConstraint(this.parentIndices[i], num, i, restBend, 0f, 0f);
							}
						}
					}
				}
				finally
				{
					IDisposable disposable2;
					if ((disposable2 = (enumerator2 as IDisposable)) != null)
					{
						disposable2.Dispose();
					}
				}
				if (i % 10 == 0)
				{
					yield return new CoroutineJob.ProgressInfo("ObiBone: generating particles...", (float)i / (float)this.bones.Count);
				}
			}
			skinBatch.Cook();
			this.initializing = false;
			this.initialized = true;
			yield break;
		}

		// Token: 0x060016D8 RID: 5848 RVA: 0x00080434 File Offset: 0x0007E834
		public override void OnSolverStepBegin()
		{
			bool enabled = base.enabled;
			if (this.animatorController != null)
			{
				this.animatorController.UpdateAnimation();
			}
			Vector4[] array = new Vector4[]
			{
				Vector4.zero
			};
			Matrix4x4 matrix4x;
			if (base.Solver.simulateInLocalSpace)
			{
				matrix4x = base.Solver.transform.worldToLocalMatrix * this.ActorLocalToWorldMatrix;
			}
			else
			{
				matrix4x = this.ActorLocalToWorldMatrix;
			}
			Matrix4x4 matrix4x2 = base.Solver.transform.worldToLocalMatrix * base.Solver.LastTransform;
			ObiSkinConstraintBatch obiSkinConstraintBatch = (ObiSkinConstraintBatch)this.SkinConstraints.GetBatches()[0];
			for (int i = 0; i < this.particleIndices.Length; i++)
			{
				Vector3 v = matrix4x.MultiplyPoint3x4(base.transform.InverseTransformPoint(this.bones[i].position));
				if (!enabled || this.invMasses[i] == 0f)
				{
					array[0] = v;
					Oni.SetParticlePositions(this.solver.OniSolver, array, 1, this.particleIndices[i]);
				}
				else if (base.Solver.simulateInLocalSpace)
				{
					Oni.GetParticlePositions(this.solver.OniSolver, array, 1, this.particleIndices[i]);
					array[0] = Vector3.Lerp(array[0], matrix4x2.MultiplyPoint3x4(array[0]), this.worldVelocityScale);
					Oni.SetParticlePositions(this.solver.OniSolver, array, 1, this.particleIndices[i]);
				}
				obiSkinConstraintBatch.skinPoints[i] = v;
			}
			obiSkinConstraintBatch.PushDataToSolver(this.SkinConstraints);
		}

		// Token: 0x060016D9 RID: 5849 RVA: 0x0008062C File Offset: 0x0007EA2C
		public override void OnSolverStepEnd()
		{
			base.OnSolverStepEnd();
			if (this.animatorController != null)
			{
				this.animatorController.ResetUpdateFlag();
			}
		}

		// Token: 0x060016DA RID: 5850 RVA: 0x00080650 File Offset: 0x0007EA50
		public void UpdateBones()
		{
			for (int i = 0; i < this.bones.Count; i++)
			{
				if (!this.frozen[i])
				{
					Vector3 particlePosition = base.GetParticlePosition(i);
					if (this.parentIndices[i] >= 0 && !this.frozen[this.parentIndices[i]])
					{
						Transform transform = this.bones[this.parentIndices[i]];
						if (transform.childCount <= 1)
						{
							Vector3 fromDirection = transform.TransformDirection(this.bones[i].localPosition);
							Vector3 toDirection = particlePosition - base.GetParticlePosition(this.parentIndices[i]);
							transform.rotation = Quaternion.FromToRotation(fromDirection, toDirection) * transform.rotation;
						}
					}
					this.bones[i].position = particlePosition;
				}
			}
		}

		// Token: 0x060016DB RID: 5851 RVA: 0x00080730 File Offset: 0x0007EB30
		public override void ResetActor()
		{
			this.PushDataToSolver(ParticleData.POSITIONS | ParticleData.VELOCITIES);
			if (this.particleIndices != null)
			{
				for (int i = 0; i < this.particleIndices.Length; i++)
				{
					this.solver.renderablePositions[this.particleIndices[i]] = this.positions[i];
					this.bones[i].position = base.transform.TransformPoint(this.positions[i]);
				}
			}
		}

		// Token: 0x040012ED RID: 4845
		public const float DEFAULT_PARTICLE_MASS = 0.1f;

		// Token: 0x040012EE RID: 4846
		public const float MAX_YOUNG_MODULUS = 20f;

		// Token: 0x040012EF RID: 4847
		public const float MIN_YOUNG_MODULUS = 0.0001f;

		// Token: 0x040012F0 RID: 4848
		[Tooltip("Initial particle radius.")]
		public float particleRadius = 0.05f;

		// Token: 0x040012F1 RID: 4849
		[HideInInspector]
		[SerializeField]
		private List<Transform> bones;

		// Token: 0x040012F2 RID: 4850
		[HideInInspector]
		[SerializeField]
		private int[] parentIndices;

		// Token: 0x040012F3 RID: 4851
		[HideInInspector]
		public bool[] frozen;

		// Token: 0x040012F4 RID: 4852
		protected ObiAnimatorController animatorController;

		// Token: 0x02000F42 RID: 3906
		[CompilerGenerated]
		private sealed class <EnumerateBonesBreadthFirst>c__Iterator0 : IEnumerable, IEnumerable<object>, IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x0600734A RID: 29514 RVA: 0x000807C9 File Offset: 0x0007EBC9
			[DebuggerHidden]
			public <EnumerateBonesBreadthFirst>c__Iterator0()
			{
			}

			// Token: 0x0600734B RID: 29515 RVA: 0x000807D4 File Offset: 0x0007EBD4
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					count = 0;
					queue = new Queue<Transform>();
					queue.Enqueue(base.transform);
					break;
				case 1U:
					enumerator = current.transform.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							Transform item = (Transform)enumerator.Current;
							queue.Enqueue(item);
						}
					}
					finally
					{
						if ((disposable = (enumerator as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
					if (current == base.transform && count > 1)
					{
						this.$current = null;
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						return true;
					}
					break;
				case 2U:
					break;
				default:
					return false;
				}
				while (queue.Count > 0)
				{
					current = queue.Dequeue();
					if (current != null)
					{
						count++;
						this.$current = current;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
				}
				this.$PC = -1;
				return false;
			}

			// Token: 0x170010DF RID: 4319
			// (get) Token: 0x0600734C RID: 29516 RVA: 0x00080964 File Offset: 0x0007ED64
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010E0 RID: 4320
			// (get) Token: 0x0600734D RID: 29517 RVA: 0x0008096C File Offset: 0x0007ED6C
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x0600734E RID: 29518 RVA: 0x00080974 File Offset: 0x0007ED74
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x0600734F RID: 29519 RVA: 0x00080984 File Offset: 0x0007ED84
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x06007350 RID: 29520 RVA: 0x0008098B File Offset: 0x0007ED8B
			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<object>.GetEnumerator();
			}

			// Token: 0x06007351 RID: 29521 RVA: 0x00080994 File Offset: 0x0007ED94
			[DebuggerHidden]
			IEnumerator<object> IEnumerable<object>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				ObiBone.<EnumerateBonesBreadthFirst>c__Iterator0 <EnumerateBonesBreadthFirst>c__Iterator = new ObiBone.<EnumerateBonesBreadthFirst>c__Iterator0();
				<EnumerateBonesBreadthFirst>c__Iterator.$this = this;
				return <EnumerateBonesBreadthFirst>c__Iterator;
			}

			// Token: 0x04006732 RID: 26418
			internal int <count>__0;

			// Token: 0x04006733 RID: 26419
			internal Queue<Transform> <queue>__0;

			// Token: 0x04006734 RID: 26420
			internal Transform <current>__1;

			// Token: 0x04006735 RID: 26421
			internal IEnumerator $locvar0;

			// Token: 0x04006736 RID: 26422
			internal IDisposable $locvar1;

			// Token: 0x04006737 RID: 26423
			internal ObiBone $this;

			// Token: 0x04006738 RID: 26424
			internal object $current;

			// Token: 0x04006739 RID: 26425
			internal bool $disposing;

			// Token: 0x0400673A RID: 26426
			internal int $PC;
		}

		// Token: 0x02000F43 RID: 3907
		[CompilerGenerated]
		private sealed class <GeneratePhysicRepresentationForBones>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007352 RID: 29522 RVA: 0x000809C8 File Offset: 0x0007EDC8
			[DebuggerHidden]
			public <GeneratePhysicRepresentationForBones>c__Iterator1()
			{
			}

			// Token: 0x06007353 RID: 29523 RVA: 0x000809D0 File Offset: 0x0007EDD0
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					this.initialized = false;
					this.initializing = true;
					this.RemoveFromSolver(null);
					this.bones = new List<Transform>();
					enumerator = base.EnumerateBonesBreadthFirst().GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							Transform item = (Transform)enumerator.Current;
							this.bones.Add(item);
						}
					}
					finally
					{
						if ((disposable = (enumerator as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
					this.parentIndices = new int[this.bones.Count];
					this.active = new bool[this.bones.Count];
					this.positions = new Vector3[this.bones.Count];
					this.velocities = new Vector3[this.bones.Count];
					this.invMasses = new float[this.bones.Count];
					this.solidRadii = new float[this.bones.Count];
					this.phases = new int[this.bones.Count];
					this.restPositions = new Vector4[this.bones.Count];
					this.frozen = new bool[this.bones.Count];
					base.DistanceConstraints.Clear();
					distanceBatch = new ObiDistanceConstraintBatch(false, false, 0.0001f, 20f);
					base.DistanceConstraints.AddBatch(distanceBatch);
					base.BendingConstraints.Clear();
					bendingBatch = new ObiBendConstraintBatch(false, false, 0.0001f, 20f);
					base.BendingConstraints.AddBatch(bendingBatch);
					base.SkinConstraints.Clear();
					skinBatch = new ObiSkinConstraintBatch(true, false, 0.0001f, 20f);
					base.SkinConstraints.AddBatch(skinBatch);
					i = 0;
					break;
				case 1U:
					IL_72A:
					i++;
					break;
				default:
					return false;
				}
				if (i >= this.bones.Count)
				{
					skinBatch.Cook();
					this.initializing = false;
					this.initialized = true;
					this.$PC = -1;
				}
				else
				{
					this.active[i] = true;
					this.invMasses[i] = 10f;
					this.positions[i] = base.transform.InverseTransformPoint(this.bones[i].position);
					this.restPositions[i] = this.positions[i];
					this.restPositions[i][3] = 0f;
					this.solidRadii[i] = this.particleRadius;
					this.frozen[i] = false;
					this.phases[i] = Oni.MakePhase(1, (!this.selfCollisions) ? ((Oni.ParticlePhase)0) : Oni.ParticlePhase.SelfCollide);
					this.parentIndices[i] = -1;
					if (this.bones[i].parent != null)
					{
						this.parentIndices[i] = this.bones.IndexOf(this.bones[i].parent);
					}
					skinBatch.AddConstraint(i, this.positions[i], Vector3.up, 0.05f, 0f, 0f, 1f);
					enumerator2 = this.bones[i].GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							Transform transform = (Transform)enumerator2.Current;
							int num2 = this.bones.IndexOf(transform);
							if (num2 >= 0)
							{
								distanceBatch.AddConstraint(i, num2, Vector3.Distance(this.bones[i].position, transform.position), 1f, 1f);
								if (this.parentIndices[i] >= 0)
								{
									Transform transform2 = this.bones[this.parentIndices[i]];
									float[] constraintCoordinates = new float[]
									{
										transform2.position[0],
										transform2.position[1],
										transform2.position[2],
										transform.position[0],
										transform.position[1],
										transform.position[2],
										this.bones[i].position[0],
										this.bones[i].position[1],
										this.bones[i].position[2]
									};
									float restBend = Oni.BendingConstraintRest(constraintCoordinates);
									bendingBatch.AddConstraint(this.parentIndices[i], num2, i, restBend, 0f, 0f);
								}
							}
						}
					}
					finally
					{
						if ((disposable2 = (enumerator2 as IDisposable)) != null)
						{
							disposable2.Dispose();
						}
					}
					if (i % 10 == 0)
					{
						this.$current = new CoroutineJob.ProgressInfo("ObiBone: generating particles...", (float)i / (float)this.bones.Count);
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					goto IL_72A;
				}
				return false;
			}

			// Token: 0x170010E1 RID: 4321
			// (get) Token: 0x06007354 RID: 29524 RVA: 0x00081194 File Offset: 0x0007F594
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010E2 RID: 4322
			// (get) Token: 0x06007355 RID: 29525 RVA: 0x0008119C File Offset: 0x0007F59C
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007356 RID: 29526 RVA: 0x000811A4 File Offset: 0x0007F5A4
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007357 RID: 29527 RVA: 0x000811B4 File Offset: 0x0007F5B4
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x0400673B RID: 26427
			internal IEnumerator $locvar0;

			// Token: 0x0400673C RID: 26428
			internal IDisposable $locvar1;

			// Token: 0x0400673D RID: 26429
			internal ObiDistanceConstraintBatch <distanceBatch>__0;

			// Token: 0x0400673E RID: 26430
			internal ObiBendConstraintBatch <bendingBatch>__0;

			// Token: 0x0400673F RID: 26431
			internal ObiSkinConstraintBatch <skinBatch>__0;

			// Token: 0x04006740 RID: 26432
			internal int <i>__1;

			// Token: 0x04006741 RID: 26433
			internal IEnumerator $locvar2;

			// Token: 0x04006742 RID: 26434
			internal IDisposable $locvar3;

			// Token: 0x04006743 RID: 26435
			internal ObiBone $this;

			// Token: 0x04006744 RID: 26436
			internal object $current;

			// Token: 0x04006745 RID: 26437
			internal bool $disposing;

			// Token: 0x04006746 RID: 26438
			internal int $PC;
		}
	}
}
