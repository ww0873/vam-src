using System;
using GPUTools.Common.Scripts.Tools.Commands;

namespace GPUTools.Hair.Scripts.Runtime.Commands.Physics
{
	// Token: 0x02000A14 RID: 2580
	public class BuildSpheres : BuildChainCommand
	{
		// Token: 0x06004169 RID: 16745 RVA: 0x00137480 File Offset: 0x00135880
		public BuildSpheres(HairSettings settings)
		{
			this.settings = settings;
		}

		// Token: 0x0600416A RID: 16746 RVA: 0x0013748F File Offset: 0x0013588F
		protected override void OnBuild()
		{
			this.settings.RuntimeData.ProcessedSpheres = GPUCollidersManager.processedSpheresBuffer;
		}

		// Token: 0x0600416B RID: 16747 RVA: 0x001374A6 File Offset: 0x001358A6
		protected override void OnFixedDispatch()
		{
			this.settings.RuntimeData.ProcessedSpheres = GPUCollidersManager.processedSpheresBuffer;
		}

		// Token: 0x0400310E RID: 12558
		private readonly HairSettings settings;
	}
}
