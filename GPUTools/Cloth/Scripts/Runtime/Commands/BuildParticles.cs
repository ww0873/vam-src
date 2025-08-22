using System;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Physics.Scripts.Types.Dynamic;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Runtime.Commands
{
	// Token: 0x0200099B RID: 2459
	public class BuildParticles : BuildChainCommand
	{
		// Token: 0x06003D6F RID: 15727 RVA: 0x0012A392 File Offset: 0x00128792
		public BuildParticles(ClothSettings settings)
		{
			this.settings = settings;
		}

		// Token: 0x06003D70 RID: 15728 RVA: 0x0012A3A4 File Offset: 0x001287A4
		protected override void OnBuild()
		{
			GPParticle[] array = new GPParticle[this.settings.GeometryData.Particles.Length];
			this.ComputeParticles(array);
			this.settings.Runtime.Particles = new GpuBuffer<GPParticle>(array, GPParticle.Size());
		}

		// Token: 0x06003D71 RID: 15729 RVA: 0x0012A3EC File Offset: 0x001287EC
		protected override void OnUpdateSettings()
		{
			this.ComputeParticles(this.settings.Runtime.Particles.Data);
			this.settings.Runtime.Particles.PushData();
			this.settings.builder.physics.ResetPhysics();
		}

		// Token: 0x06003D72 RID: 15730 RVA: 0x0012A440 File Offset: 0x00128840
		private void ComputeParticles(GPParticle[] particles)
		{
			Vector3[] particles2 = this.settings.GeometryData.Particles;
			Matrix4x4 toWorldMatrix = this.settings.MeshProvider.ToWorldMatrix;
			int[] physicsToMeshVerticesMap = this.settings.GeometryData.PhysicsToMeshVerticesMap;
			float[] particlesStrength = this.settings.GeometryData.ParticlesStrength;
			float radius = this.settings.ParticleRadius * this.settings.transform.lossyScale.x;
			if (particles == null)
			{
				particles = new GPParticle[particles2.Length];
			}
			for (int i = 0; i < particles2.Length; i++)
			{
				Vector3 position = toWorldMatrix.MultiplyPoint3x4(particles2[i]);
				int num = physicsToMeshVerticesMap[i];
				particles[i] = new GPParticle(position, radius);
				float num2 = particlesStrength[num];
				num2 = Mathf.Max(0.1f, num2);
				particles[i].Strength = num2;
			}
		}

		// Token: 0x06003D73 RID: 15731 RVA: 0x0012A535 File Offset: 0x00128935
		protected override void OnDispose()
		{
			this.settings.Runtime.Particles.Dispose();
		}

		// Token: 0x04002F31 RID: 12081
		private readonly ClothSettings settings;
	}
}
