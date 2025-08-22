using System;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Physics.Scripts.Types.Dynamic;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Runtime.Commands
{
	// Token: 0x02000996 RID: 2454
	public class BuildClothAccessories : BuildChainCommand
	{
		// Token: 0x06003D5A RID: 15706 RVA: 0x00129BDA File Offset: 0x00127FDA
		public BuildClothAccessories(ClothSettings settings)
		{
			this.settings = settings;
			this.sphereCollidersCache = new CacheProvider<SphereCollider>(settings.AccessoriesProviders);
		}

		// Token: 0x06003D5B RID: 15707 RVA: 0x00129BFC File Offset: 0x00127FFC
		protected override void OnBuild()
		{
			if (this.sphereCollidersCache.Items.Count == 0)
			{
				return;
			}
			GPParticle[] data = new GPParticle[this.sphereCollidersCache.Items.Count];
			this.settings.Runtime.OutParticles = new GpuBuffer<GPParticle>(data, GPParticle.Size());
			float[] array = new float[this.sphereCollidersCache.Items.Count];
			this.CalculateOutParticlesMap(array);
			this.settings.Runtime.OutParticlesMap = new GpuBuffer<float>(array, 4);
		}

		// Token: 0x06003D5C RID: 15708 RVA: 0x00129C84 File Offset: 0x00128084
		protected override void OnDispatch()
		{
			if (this.settings.Runtime.OutParticles == null)
			{
				return;
			}
			this.settings.Runtime.OutParticles.PullData();
			for (int i = 0; i < this.settings.Runtime.OutParticles.Data.Length; i++)
			{
				GPParticle gpparticle = this.settings.Runtime.OutParticles.Data[i];
				this.sphereCollidersCache.Items[i].transform.position = gpparticle.Position;
			}
		}

		// Token: 0x06003D5D RID: 15709 RVA: 0x00129D28 File Offset: 0x00128128
		private void CalculateOutParticlesMap(float[] outParticlesMap)
		{
			float[] array = new float[this.sphereCollidersCache.Items.Count];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = float.PositiveInfinity;
			}
			GPParticle[] data = this.settings.Runtime.Particles.Data;
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

		// Token: 0x06003D5E RID: 15710 RVA: 0x00129E18 File Offset: 0x00128218
		protected override void OnDispose()
		{
			if (this.settings.Runtime.OutParticles != null)
			{
				this.settings.Runtime.OutParticles.Dispose();
			}
			if (this.settings.Runtime.OutParticlesMap != null)
			{
				this.settings.Runtime.OutParticlesMap.Dispose();
			}
		}

		// Token: 0x04002F27 RID: 12071
		private readonly ClothSettings settings;

		// Token: 0x04002F28 RID: 12072
		private CacheProvider<SphereCollider> sphereCollidersCache;
	}
}
