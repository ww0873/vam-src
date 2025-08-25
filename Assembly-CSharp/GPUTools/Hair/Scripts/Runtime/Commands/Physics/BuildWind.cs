using System;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Physics.Scripts.Wind;

namespace GPUTools.Hair.Scripts.Runtime.Commands.Physics
{
	// Token: 0x02000A15 RID: 2581
	public class BuildWind : BuildChainCommand
	{
		// Token: 0x0600416C RID: 16748 RVA: 0x001374BD File Offset: 0x001358BD
		public BuildWind(HairSettings settings)
		{
			this.settings = settings;
			this.wind = new WindReceiver();
		}

		// Token: 0x0600416D RID: 16749 RVA: 0x001374D8 File Offset: 0x001358D8
		protected override void OnDispatch()
		{
			this.settings.RuntimeData.Wind = this.wind.GetWind(this.settings.StandsSettings.HeadCenterWorld) * this.settings.PhysicsSettings.WindMultiplier;
		}

		// Token: 0x0400310F RID: 12559
		private readonly HairSettings settings;

		// Token: 0x04003110 RID: 12560
		private readonly WindReceiver wind;
	}
}
