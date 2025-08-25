using System;
using GPUTools.Common.Scripts.Tools.Commands;

namespace GPUTools.Hair.Scripts.Runtime.Commands.Physics
{
	// Token: 0x02000A0F RID: 2575
	public class BuildLineSpheres : BuildChainCommand
	{
		// Token: 0x0600414E RID: 16718 RVA: 0x0013660B File Offset: 0x00134A0B
		public BuildLineSpheres(HairSettings settings)
		{
			this.settings = settings;
		}

		// Token: 0x0600414F RID: 16719 RVA: 0x0013661A File Offset: 0x00134A1A
		protected override void OnBuild()
		{
			this.settings.RuntimeData.ProcessedLineSpheres = GPUCollidersManager.processedLineSpheresBuffer;
		}

		// Token: 0x06004150 RID: 16720 RVA: 0x00136631 File Offset: 0x00134A31
		protected override void OnFixedDispatch()
		{
			this.settings.RuntimeData.ProcessedLineSpheres = GPUCollidersManager.processedLineSpheresBuffer;
		}

		// Token: 0x04003105 RID: 12549
		private readonly HairSettings settings;
	}
}
