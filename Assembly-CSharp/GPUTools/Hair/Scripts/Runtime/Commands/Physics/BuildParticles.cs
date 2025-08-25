using System;
using System.Collections.Generic;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Hair.Scripts.Geometry.Abstract;
using GPUTools.Physics.Scripts.Types.Dynamic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Commands.Physics
{
	// Token: 0x02000A11 RID: 2577
	public class BuildParticles : BuildChainCommand
	{
		// Token: 0x06004156 RID: 16726 RVA: 0x0013689E File Offset: 0x00134C9E
		public BuildParticles(HairSettings settings)
		{
			this.settings = settings;
			this.provider = settings.StandsSettings.Provider;
		}

		// Token: 0x06004157 RID: 16727 RVA: 0x001368C0 File Offset: 0x00134CC0
		protected override void OnBuild()
		{
			GPParticle[] array = new GPParticle[this.provider.GetVertices().Count];
			float[] particleRootToTipRatios = new float[this.provider.GetVertices().Count];
			this.ComputeParticles(array, particleRootToTipRatios);
			if (this.settings.RuntimeData.Particles != null)
			{
				this.settings.RuntimeData.Particles.Dispose();
			}
			if (array.Length > 0)
			{
				this.settings.RuntimeData.Particles = new GpuBuffer<GPParticle>(array, GPParticle.Size());
			}
			else
			{
				this.settings.RuntimeData.Particles = null;
			}
			this.settings.RuntimeData.ParticleRootToTipRatios = particleRootToTipRatios;
		}

		// Token: 0x06004158 RID: 16728 RVA: 0x00136978 File Offset: 0x00134D78
		public void UpdateParticleRadius()
		{
			if (this.settings.RuntimeData.Particles != null)
			{
				GPParticle[] array = (GPParticle[])this.settings.RuntimeData.Particles.Data.Clone();
				this.settings.RuntimeData.Particles.PullData();
				float num;
				float radius;
				if (this.settings.PhysicsSettings.StyleMode)
				{
					num = this.settings.PhysicsSettings.StyleModeStrandRadius * this.provider.transform.lossyScale.x;
					if (this.settings.PhysicsSettings.UseSeparateRootRadius)
					{
						radius = this.settings.PhysicsSettings.StyleModeStrandRootRadius * this.provider.transform.lossyScale.x;
					}
					else
					{
						radius = num;
					}
				}
				else
				{
					num = this.settings.PhysicsSettings.StandRadius * this.provider.transform.lossyScale.x;
					if (this.settings.PhysicsSettings.UseSeparateRootRadius)
					{
						radius = this.settings.PhysicsSettings.StandRootRadius * this.provider.transform.lossyScale.x;
					}
					else
					{
						radius = num;
					}
				}
				int segments = this.settings.StandsSettings.Segments;
				GPParticle[] data = this.settings.RuntimeData.Particles.Data;
				for (int i = 0; i < data.Length; i++)
				{
					int num2 = i % segments;
					if (num2 == 1)
					{
						data[i].Radius = radius;
						array[i].Radius = radius;
					}
					else
					{
						data[i].Radius = num;
						array[i].Radius = num;
					}
				}
				this.settings.RuntimeData.Particles.PushData();
				this.settings.RuntimeData.Particles.Data = array;
			}
		}

		// Token: 0x06004159 RID: 16729 RVA: 0x00136B88 File Offset: 0x00134F88
		public void SaveGPUState()
		{
			if (this.settings.RuntimeData.Particles != null)
			{
				GPParticle[] data = (GPParticle[])this.settings.RuntimeData.Particles.Data.Clone();
				this.settings.RuntimeData.Particles.PullData();
				this.gpuState = this.settings.RuntimeData.Particles.Data;
				this.settings.RuntimeData.Particles.Data = data;
			}
		}

		// Token: 0x0600415A RID: 16730 RVA: 0x00136C10 File Offset: 0x00135010
		public void RestoreGPUState()
		{
			if (this.settings.RuntimeData.Particles != null && this.gpuState != null)
			{
				this.settings.RuntimeData.Particles.Data = this.gpuState;
				this.settings.RuntimeData.Particles.PushData();
			}
		}

		// Token: 0x0600415B RID: 16731 RVA: 0x00136C70 File Offset: 0x00135070
		protected override void OnUpdateSettings()
		{
			float[] particleRootToTipRatios = new float[this.provider.GetVertices().Count];
			if (this.settings.RuntimeData.Particles != null)
			{
				this.ComputeParticles(this.settings.RuntimeData.Particles.Data, particleRootToTipRatios);
				this.settings.RuntimeData.Particles.PushData();
				this.settings.RuntimeData.ParticleRootToTipRatios = particleRootToTipRatios;
			}
		}

		// Token: 0x0600415C RID: 16732 RVA: 0x00136CEC File Offset: 0x001350EC
		private void ComputeParticles(GPParticle[] particles, float[] particleRootToTipRatios)
		{
			Matrix4x4 toWorldMatrix = this.provider.GetToWorldMatrix();
			float num = this.settings.PhysicsSettings.StandRadius * this.provider.transform.lossyScale.x;
			float radius = (!this.settings.PhysicsSettings.UseSeparateRootRadius) ? num : (this.settings.PhysicsSettings.StandRootRadius * this.provider.transform.lossyScale.x);
			List<Vector3> vertices = this.provider.GetVertices();
			int segments = this.settings.StandsSettings.Segments;
			float num2 = 0f;
			float num3 = 0f;
			Vector3 b = Vector3.zero;
			for (int i = 0; i < vertices.Count; i++)
			{
				if (i % segments == 0)
				{
					if (num2 > num3)
					{
						num3 = num2;
					}
					num2 = 0f;
				}
				else
				{
					float magnitude = (vertices[i] - b).magnitude;
					num2 += magnitude;
				}
				b = vertices[i];
			}
			for (int j = 0; j < vertices.Count; j++)
			{
				int num4 = j % segments;
				if (num4 == 0)
				{
					num2 = 0f;
				}
				else
				{
					float magnitude2 = (vertices[j] - b).magnitude;
					num2 += magnitude2;
				}
				b = vertices[j];
				float num5 = num2 / num3;
				Vector3 position = toWorldMatrix.MultiplyPoint3x4(vertices[j]);
				if (num4 == 0)
				{
					particles[j] = new GPParticle(position, num);
					particles[j].CollisionEnabled = 0;
				}
				else if (num4 == 1)
				{
					particles[j] = new GPParticle(position, radius);
				}
				else
				{
					particles[j] = new GPParticle(position, num);
				}
				particleRootToTipRatios[j] = num5;
			}
		}

		// Token: 0x0600415D RID: 16733 RVA: 0x00136F01 File Offset: 0x00135301
		protected override void OnDispose()
		{
			if (this.settings.RuntimeData.Particles != null)
			{
				this.settings.RuntimeData.Particles.Dispose();
			}
		}

		// Token: 0x04003109 RID: 12553
		private readonly HairSettings settings;

		// Token: 0x0400310A RID: 12554
		private readonly GeometryProviderBase provider;

		// Token: 0x0400310B RID: 12555
		protected GPParticle[] gpuState;
	}
}
