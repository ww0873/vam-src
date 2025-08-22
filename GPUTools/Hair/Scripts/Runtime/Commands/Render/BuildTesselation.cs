using System;
using GPUTools.Common.Scripts.PL.Tools;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Hair.Scripts.Runtime.Render;

namespace GPUTools.Hair.Scripts.Runtime.Commands.Render
{
	// Token: 0x02000A18 RID: 2584
	public class BuildTesselation : IBuildCommand
	{
		// Token: 0x06004182 RID: 16770 RVA: 0x00137F05 File Offset: 0x00136305
		public BuildTesselation(HairSettings settings)
		{
			this.settings = settings;
		}

		// Token: 0x06004183 RID: 16771 RVA: 0x00137F14 File Offset: 0x00136314
		public void Build()
		{
			int num = this.settings.StandsSettings.Provider.GetStandsNum() * 64;
			if (this.settings.RuntimeData.TessRenderParticles != null)
			{
				this.settings.RuntimeData.TessRenderParticles.Dispose();
			}
			if (num > 0)
			{
				this.settings.RuntimeData.TessRenderParticles = new GpuBuffer<TessRenderParticle>(num, TessRenderParticle.Size());
			}
			else
			{
				this.settings.RuntimeData.TessRenderParticles = null;
			}
		}

		// Token: 0x06004184 RID: 16772 RVA: 0x00137F9C File Offset: 0x0013639C
		public void Dispatch()
		{
		}

		// Token: 0x06004185 RID: 16773 RVA: 0x00137F9E File Offset: 0x0013639E
		public void FixedDispatch()
		{
		}

		// Token: 0x06004186 RID: 16774 RVA: 0x00137FA0 File Offset: 0x001363A0
		public void UpdateSettings()
		{
		}

		// Token: 0x06004187 RID: 16775 RVA: 0x00137FA2 File Offset: 0x001363A2
		public void Dispose()
		{
			if (this.settings.RuntimeData.TessRenderParticles != null)
			{
				this.settings.RuntimeData.TessRenderParticles.Dispose();
			}
		}

		// Token: 0x04003116 RID: 12566
		private readonly HairSettings settings;
	}
}
