using System;
using UnityEngine;

namespace Obi
{
	// Token: 0x02000392 RID: 914
	[ExecuteInEditMode]
	[AddComponentMenu("Physics/Obi/Obi Emitter")]
	public class ObiEmitter : ObiActor
	{
		// Token: 0x060016DC RID: 5852 RVA: 0x000811BB File Offset: 0x0007F5BB
		public ObiEmitter()
		{
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x060016DE RID: 5854 RVA: 0x00081206 File Offset: 0x0007F606
		// (set) Token: 0x060016DD RID: 5853 RVA: 0x000811EB File Offset: 0x0007F5EB
		public int NumParticles
		{
			get
			{
				return this.numParticles;
			}
			set
			{
				if (this.numParticles != value)
				{
					this.numParticles = value;
					this.GeneratePhysicRepresentation();
				}
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x060016DF RID: 5855 RVA: 0x0008120E File Offset: 0x0007F60E
		public int ActiveParticles
		{
			get
			{
				return this.activeParticleCount;
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x060016E0 RID: 5856 RVA: 0x00081216 File Offset: 0x0007F616
		public override bool SelfCollisions
		{
			get
			{
				return this.selfCollisions;
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x060016E1 RID: 5857 RVA: 0x0008121E File Offset: 0x0007F61E
		// (set) Token: 0x060016E2 RID: 5858 RVA: 0x00081226 File Offset: 0x0007F626
		public ObiEmitterShape EmitterShape
		{
			get
			{
				return this.emitterShape;
			}
			set
			{
				if (this.emitterShape != value)
				{
					this.emitterShape = value;
					this.UpdateEmitterDistribution();
				}
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x060016E4 RID: 5860 RVA: 0x000812CF File Offset: 0x0007F6CF
		// (set) Token: 0x060016E3 RID: 5859 RVA: 0x00081248 File Offset: 0x0007F648
		public ObiEmitterMaterial EmitterMaterial
		{
			get
			{
				return this.emitterMaterial;
			}
			set
			{
				if (this.emitterMaterial != value)
				{
					if (this.emitterMaterial != null)
					{
						this.emitterMaterial.OnChangesMade -= this.EmitterMaterial_OnChangesMade;
					}
					this.emitterMaterial = value;
					if (this.emitterMaterial != null)
					{
						this.emitterMaterial.OnChangesMade += this.EmitterMaterial_OnChangesMade;
						this.EmitterMaterial_OnChangesMade(this.emitterMaterial, new ObiEmitterMaterial.MaterialChangeEventArgs(ObiEmitterMaterial.MaterialChanges.PER_PARTICLE_DATA));
					}
				}
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x060016E5 RID: 5861 RVA: 0x000812D7 File Offset: 0x0007F6D7
		public override bool UsesCustomExternalForces
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060016E6 RID: 5862 RVA: 0x000812DA File Offset: 0x0007F6DA
		public override void Awake()
		{
			base.Awake();
			this.selfCollisions = true;
			this.GeneratePhysicRepresentation();
		}

		// Token: 0x060016E7 RID: 5863 RVA: 0x000812EF File Offset: 0x0007F6EF
		public override void OnEnable()
		{
			if (this.emitterMaterial != null)
			{
				this.emitterMaterial.OnChangesMade += this.EmitterMaterial_OnChangesMade;
			}
			base.OnEnable();
		}

		// Token: 0x060016E8 RID: 5864 RVA: 0x0008131F File Offset: 0x0007F71F
		public override void OnDisable()
		{
			if (this.emitterMaterial != null)
			{
				this.emitterMaterial.OnChangesMade -= this.EmitterMaterial_OnChangesMade;
			}
			base.OnDisable();
		}

		// Token: 0x060016E9 RID: 5865 RVA: 0x0008134F File Offset: 0x0007F74F
		public override void DestroyRequiredComponents()
		{
		}

		// Token: 0x060016EA RID: 5866 RVA: 0x00081351 File Offset: 0x0007F751
		public override bool AddToSolver(object info)
		{
			if (base.Initialized && base.AddToSolver(info))
			{
				this.solver.RequireRenderablePositions();
				this.CalculateParticleMass();
				return true;
			}
			return false;
		}

		// Token: 0x060016EB RID: 5867 RVA: 0x0008137E File Offset: 0x0007F77E
		public override bool RemoveFromSolver(object info)
		{
			if (this.solver != null)
			{
				this.solver.RelinquishRenderablePositions();
			}
			return base.RemoveFromSolver(info);
		}

		// Token: 0x060016EC RID: 5868 RVA: 0x000813A4 File Offset: 0x0007F7A4
		public void CalculateParticleMass()
		{
			float num = (!(this.emitterMaterial != null)) ? 0.1f : this.emitterMaterial.GetParticleMass(this.solver.parameters.mode);
			for (int i = 0; i < this.invMasses.Length; i++)
			{
				this.invMasses[i] = 1f / num;
			}
			this.PushDataToSolver(ParticleData.INV_MASSES);
		}

		// Token: 0x060016ED RID: 5869 RVA: 0x00081418 File Offset: 0x0007F818
		public void SetParticleRestRadius()
		{
			if (!base.InSolver)
			{
				return;
			}
			float num = (!(this.emitterMaterial != null)) ? 0.1f : this.emitterMaterial.GetParticleSize(this.solver.parameters.mode);
			for (int i = 0; i < this.particleIndices.Length; i++)
			{
				this.solidRadii[i] = num * 0.5f;
			}
			this.PushDataToSolver(ParticleData.SOLID_RADII);
		}

		// Token: 0x060016EE RID: 5870 RVA: 0x00081498 File Offset: 0x0007F898
		public void GeneratePhysicRepresentation()
		{
			this.initialized = false;
			this.initializing = true;
			this.RemoveFromSolver(null);
			this.active = new bool[this.numParticles];
			this.life = new float[this.numParticles];
			this.positions = new Vector3[this.numParticles];
			this.velocities = new Vector3[this.numParticles];
			this.invMasses = new float[this.numParticles];
			this.solidRadii = new float[this.numParticles];
			this.phases = new int[this.numParticles];
			this.colors = new Color[this.numParticles];
			float num = (!(this.emitterMaterial != null)) ? 0.1f : this.emitterMaterial.GetParticleSize(this.solver.parameters.mode);
			float num2 = (!(this.emitterMaterial != null)) ? 0.1f : this.emitterMaterial.GetParticleMass(this.solver.parameters.mode);
			for (int i = 0; i < this.numParticles; i++)
			{
				this.active[i] = false;
				this.life[i] = 0f;
				this.invMasses[i] = 1f / num2;
				this.positions[i] = Vector3.zero;
				if (this.emitterMaterial != null && !(this.emitterMaterial is ObiEmitterMaterialFluid))
				{
					float num3 = UnityEngine.Random.Range(0f, num / 100f * (this.emitterMaterial as ObiEmitterMaterialGranular).randomness);
					this.solidRadii[i] = Mathf.Max(new float[]
					{
						0.001f + num * 0.5f - num3
					});
				}
				else
				{
					this.solidRadii[i] = num * 0.5f;
				}
				this.colors[i] = Color.white;
				this.phases[i] = Oni.MakePhase(this.fluidPhase, ((!this.selfCollisions) ? ((Oni.ParticlePhase)0) : Oni.ParticlePhase.SelfCollide) | ((!(this.emitterMaterial != null) || !(this.emitterMaterial is ObiEmitterMaterialFluid)) ? ((Oni.ParticlePhase)0) : Oni.ParticlePhase.Fluid));
			}
			this.initializing = false;
			this.initialized = true;
		}

		// Token: 0x060016EF RID: 5871 RVA: 0x000816FC File Offset: 0x0007FAFC
		public override void UpdateParticlePhases()
		{
			if (!base.InSolver)
			{
				return;
			}
			Oni.ParticlePhase particlePhase = Oni.ParticlePhase.Fluid;
			if (this.emitterMaterial != null && !(this.emitterMaterial is ObiEmitterMaterialFluid))
			{
				particlePhase = (Oni.ParticlePhase)0;
			}
			for (int i = 0; i < this.particleIndices.Length; i++)
			{
				this.phases[i] = Oni.MakePhase(this.fluidPhase, ((!this.selfCollisions) ? ((Oni.ParticlePhase)0) : Oni.ParticlePhase.SelfCollide) | particlePhase);
			}
			this.PushDataToSolver(ParticleData.PHASES);
		}

		// Token: 0x060016F0 RID: 5872 RVA: 0x00081790 File Offset: 0x0007FB90
		private void UpdateEmitterDistribution()
		{
			if (this.emitterShape != null)
			{
				this.emitterShape.particleSize = ((!(this.emitterMaterial != null)) ? 0.1f : this.emitterMaterial.GetParticleSize(this.solver.parameters.mode));
				this.emitterShape.GenerateDistribution();
			}
		}

		// Token: 0x060016F1 RID: 5873 RVA: 0x000817FA File Offset: 0x0007FBFA
		private void EmitterMaterial_OnChangesMade(object sender, ObiEmitterMaterial.MaterialChangeEventArgs e)
		{
			if ((e.changes & ObiEmitterMaterial.MaterialChanges.PER_PARTICLE_DATA) != ObiEmitterMaterial.MaterialChanges.PER_MATERIAL_DATA)
			{
				this.CalculateParticleMass();
				this.SetParticleRestRadius();
				this.UpdateParticlePhases();
			}
			this.UpdateEmitterDistribution();
		}

		// Token: 0x060016F2 RID: 5874 RVA: 0x00081824 File Offset: 0x0007FC24
		public void ResetParticlePosition(int index, float offset)
		{
			if (this.emitterShape == null)
			{
				Vector3 a = Vector3.Lerp(base.transform.forward, UnityEngine.Random.onUnitSphere, this.randomVelocity);
				Vector3 b = a * (this.speed * Time.fixedDeltaTime) * offset;
				Vector4[] positions = new Vector4[]
				{
					base.transform.position + b
				};
				Vector4[] velocities = new Vector4[]
				{
					a * this.speed
				};
				Oni.SetParticlePositions(this.solver.OniSolver, positions, 1, this.particleIndices[index]);
				Oni.SetParticleVelocities(this.solver.OniSolver, velocities, 1, this.particleIndices[index]);
				this.colors[index] = Color.white;
			}
			else
			{
				ObiEmitterShape.DistributionPoint distributionPoint = this.emitterShape.GetDistributionPoint();
				Vector3 a2 = Vector3.Lerp(base.transform.TransformVector(distributionPoint.velocity), UnityEngine.Random.onUnitSphere, this.randomVelocity);
				Vector3 b2 = a2 * (this.speed * Time.fixedDeltaTime) * offset;
				Vector4[] positions2 = new Vector4[]
				{
					base.transform.TransformPoint(distributionPoint.position) + b2
				};
				Vector4[] velocities2 = new Vector4[]
				{
					a2 * this.speed
				};
				Oni.SetParticlePositions(this.solver.OniSolver, positions2, 1, this.particleIndices[index]);
				Oni.SetParticleVelocities(this.solver.OniSolver, velocities2, 1, this.particleIndices[index]);
				this.colors[index] = distributionPoint.color;
			}
		}

		// Token: 0x060016F3 RID: 5875 RVA: 0x00081A0C File Offset: 0x0007FE0C
		public bool EmitParticle(float offset)
		{
			if (this.activeParticleCount == this.numParticles)
			{
				return false;
			}
			this.life[this.activeParticleCount] = this.lifespan;
			this.ResetParticlePosition(this.activeParticleCount, offset);
			this.active[this.activeParticleCount] = true;
			this.activeParticleCount++;
			return true;
		}

		// Token: 0x060016F4 RID: 5876 RVA: 0x00081A6C File Offset: 0x0007FE6C
		public bool KillParticle(int index)
		{
			if (this.activeParticleCount == 0 || index >= this.activeParticleCount)
			{
				return false;
			}
			this.activeParticleCount--;
			this.active[this.activeParticleCount] = false;
			int num = this.particleIndices[this.activeParticleCount];
			this.particleIndices[this.activeParticleCount] = this.particleIndices[index];
			this.particleIndices[index] = num;
			float num2 = this.life[this.activeParticleCount];
			this.life[this.activeParticleCount] = this.life[index];
			this.life[index] = num2;
			Color color = this.colors[this.activeParticleCount];
			this.colors[this.activeParticleCount] = this.colors[index];
			this.colors[index] = color;
			return true;
		}

		// Token: 0x060016F5 RID: 5877 RVA: 0x00081B58 File Offset: 0x0007FF58
		public void KillAll()
		{
			for (int i = this.activeParticleCount - 1; i >= 0; i--)
			{
				this.KillParticle(i);
			}
			this.PushDataToSolver(ParticleData.ACTIVE_STATUS);
		}

		// Token: 0x060016F6 RID: 5878 RVA: 0x00081B90 File Offset: 0x0007FF90
		public override void OnSolverStepBegin()
		{
			base.OnSolverStepBegin();
			bool flag = false;
			bool flag2 = false;
			for (int i = this.activeParticleCount - 1; i >= 0; i--)
			{
				this.life[i] -= Time.deltaTime;
				if (this.life[i] <= 0f)
				{
					flag2 |= this.KillParticle(i);
				}
			}
			int num = (!(this.emitterShape != null)) ? 1 : this.emitterShape.DistributionPointsCount;
			if (this.emitterShape == null || this.emitterShape.samplingMethod == ObiEmitterShape.SamplingMethod.SURFACE)
			{
				float num2 = this.speed * Time.fixedDeltaTime / ((!(this.emitterMaterial != null)) ? 0.1f : this.emitterMaterial.GetParticleSize(this.solver.parameters.mode));
				this.unemittedBursts += num2;
				int num3 = 0;
				while (this.unemittedBursts > 0f)
				{
					for (int j = 0; j < num; j++)
					{
						flag |= this.EmitParticle((float)num3 / num2);
					}
					this.unemittedBursts -= 1f;
					num3++;
				}
			}
			else if (this.activeParticleCount == 0)
			{
				for (int k = 0; k < num; k++)
				{
					flag |= this.EmitParticle(0f);
				}
			}
			if (flag || flag2)
			{
				this.PushDataToSolver(ParticleData.ACTIVE_STATUS);
			}
		}

		// Token: 0x040012F5 RID: 4853
		public int fluidPhase = 1;

		// Token: 0x040012F6 RID: 4854
		[SerializeField]
		[HideInInspector]
		private ObiEmitterMaterial emitterMaterial;

		// Token: 0x040012F7 RID: 4855
		[Tooltip("Amount of solver particles used by this emitter.")]
		[SerializeField]
		[HideInInspector]
		private int numParticles = 1000;

		// Token: 0x040012F8 RID: 4856
		[Tooltip("Speed (in units/second) of emitted particles. Setting it to zero will stop emission. Large values will cause more particles to be emitted.")]
		public float speed = 0.25f;

		// Token: 0x040012F9 RID: 4857
		[Tooltip("Lifespan of each particle.")]
		public float lifespan = 4f;

		// Token: 0x040012FA RID: 4858
		[Range(0f, 1f)]
		[Tooltip("Amount of randomization applied to particles.")]
		public float randomVelocity;

		// Token: 0x040012FB RID: 4859
		private ObiEmitterShape emitterShape;

		// Token: 0x040012FC RID: 4860
		private int activeParticleCount;

		// Token: 0x040012FD RID: 4861
		[HideInInspector]
		public float[] life;

		// Token: 0x040012FE RID: 4862
		private float unemittedBursts;
	}
}
