using System;
using UnityEngine;
using UnityEngine.Rendering;

namespace Obi
{
	// Token: 0x02000368 RID: 872
	public class ShadowmapExposer : MonoBehaviour
	{
		// Token: 0x060015CC RID: 5580 RVA: 0x0007CB7D File Offset: 0x0007AF7D
		public ShadowmapExposer()
		{
		}

		// Token: 0x060015CD RID: 5581 RVA: 0x0007CB85 File Offset: 0x0007AF85
		public void Awake()
		{
			this.light = base.GetComponent<Light>();
		}

		// Token: 0x060015CE RID: 5582 RVA: 0x0007CB93 File Offset: 0x0007AF93
		public void OnEnable()
		{
			this.Cleanup();
			this.afterShadow = new CommandBuffer();
			this.afterShadow.name = "FluidShadows";
			this.light.AddCommandBuffer(LightEvent.AfterShadowMapPass, this.afterShadow);
		}

		// Token: 0x060015CF RID: 5583 RVA: 0x0007CBC8 File Offset: 0x0007AFC8
		public void OnDisable()
		{
			this.Cleanup();
		}

		// Token: 0x060015D0 RID: 5584 RVA: 0x0007CBD0 File Offset: 0x0007AFD0
		private void Cleanup()
		{
			if (this.afterShadow != null)
			{
				this.light.RemoveCommandBuffer(LightEvent.AfterShadowMapPass, this.afterShadow);
				this.afterShadow = null;
			}
		}

		// Token: 0x060015D1 RID: 5585 RVA: 0x0007CBF8 File Offset: 0x0007AFF8
		public void SetupFluidShadowsCommandBuffer()
		{
			this.afterShadow.Clear();
			if (this.particleRenderers == null)
			{
				return;
			}
			foreach (ObiParticleRenderer obiParticleRenderer in this.particleRenderers)
			{
				if (obiParticleRenderer != null)
				{
					foreach (Mesh mesh in obiParticleRenderer.ParticleMeshes)
					{
						this.afterShadow.DrawMesh(mesh, Matrix4x4.identity, obiParticleRenderer.ParticleMaterial, 0, 1);
					}
				}
			}
		}

		// Token: 0x060015D2 RID: 5586 RVA: 0x0007CCAC File Offset: 0x0007B0AC
		private void Update()
		{
			bool flag = base.gameObject.activeInHierarchy && base.enabled;
			if (!flag || this.particleRenderers == null || this.particleRenderers.Length == 0)
			{
				this.Cleanup();
				return;
			}
			if (this.afterShadow != null)
			{
				this.SetupFluidShadowsCommandBuffer();
			}
		}

		// Token: 0x0400123B RID: 4667
		private Light light;

		// Token: 0x0400123C RID: 4668
		private CommandBuffer afterShadow;

		// Token: 0x0400123D RID: 4669
		public ObiParticleRenderer[] particleRenderers;
	}
}
