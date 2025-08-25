using System;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Physics.Scripts.Types.Dynamic;
using UnityEngine;

namespace GPUTools.Hair.Scripts.Runtime.Commands.Physics
{
	// Token: 0x02000A0B RID: 2571
	public class BuildAccessories : BuildChainCommand
	{
		// Token: 0x0600413B RID: 16699 RVA: 0x00135B95 File Offset: 0x00133F95
		public BuildAccessories(HairSettings settings)
		{
			this.settings = settings;
			this.sphereCollidersCache = new CacheProvider<SphereCollider>(settings.PhysicsSettings.AccessoriesProviders);
		}

		// Token: 0x0600413C RID: 16700 RVA: 0x00135BBC File Offset: 0x00133FBC
		protected override void OnBuild()
		{
			if (this.sphereCollidersCache.Items.Count == 0)
			{
				return;
			}
			GPParticle[] data = new GPParticle[this.sphereCollidersCache.Items.Count];
			this.settings.RuntimeData.OutParticles = new GpuBuffer<GPParticle>(data, GPParticle.Size());
			float[] array = new float[this.sphereCollidersCache.Items.Count];
			this.CalculateOutParticlesMap(array);
			this.settings.RuntimeData.OutParticlesMap = new GpuBuffer<float>(array, 4);
		}

		// Token: 0x0600413D RID: 16701 RVA: 0x00135C44 File Offset: 0x00134044
		protected override void OnDispatch()
		{
			if (this.settings.RuntimeData.OutParticles == null)
			{
				return;
			}
			this.settings.RuntimeData.OutParticles.PullData();
			for (int i = 0; i < this.settings.RuntimeData.OutParticles.Data.Length; i++)
			{
				GPParticle gpparticle = this.settings.RuntimeData.OutParticles.Data[i];
				this.sphereCollidersCache.Items[i].transform.position = gpparticle.Position;
			}
		}

		// Token: 0x0600413E RID: 16702 RVA: 0x00135CE8 File Offset: 0x001340E8
		private void CalculateOutParticlesMap(float[] outParticlesMap)
		{
			float[] array = new float[this.sphereCollidersCache.Items.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = float.PositiveInfinity;
			}
			GPParticle[] data = this.settings.RuntimeData.Particles.Data;
			for (int j = 0; j < data.Length; j++)
			{
				GPParticle gpparticle = data[j];
				for (int k = 0; k < this.sphereCollidersCache.Items.Count; k++)
				{
					SphereCollider sphereCollider = this.sphereCollidersCache.Items[k];
					float num = Vector3.Distance(sphereCollider.transform.position, gpparticle.Position);
					if (num < sphereCollider.radius && num < array[k])
					{
						array[k] = num;
						outParticlesMap[k] = (float)j;
					}
				}
			}
		}

		// Token: 0x0600413F RID: 16703 RVA: 0x00135DD8 File Offset: 0x001341D8
		protected override void OnDispose()
		{
			if (this.settings.RuntimeData.OutParticles != null)
			{
				this.settings.RuntimeData.OutParticles.Dispose();
			}
			if (this.settings.RuntimeData.OutParticlesMap != null)
			{
				this.settings.RuntimeData.OutParticlesMap.Dispose();
			}
		}

		// Token: 0x040030FB RID: 12539
		private readonly HairSettings settings;

		// Token: 0x040030FC RID: 12540
		private CacheProvider<SphereCollider> sphereCollidersCache;
	}
}
