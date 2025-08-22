using System;
using GPUTools.Common.Scripts.Tools.Commands;

namespace GPUTools.Hair.Scripts.Runtime.Commands
{
	// Token: 0x02000A12 RID: 2578
	public class BuildPlanes : BuildChainCommand
	{
		// Token: 0x0600415E RID: 16734 RVA: 0x00136F2D File Offset: 0x0013532D
		public BuildPlanes(HairSettings settings)
		{
			this.settings = settings;
		}

		// Token: 0x0600415F RID: 16735 RVA: 0x00136F3C File Offset: 0x0013533C
		protected override void OnBuild()
		{
			this.settings.RuntimeData.Planes = GPUCollidersManager.planesBuffer;
		}

		// Token: 0x06004160 RID: 16736 RVA: 0x00136F53 File Offset: 0x00135353
		protected override void OnFixedDispatch()
		{
			this.settings.RuntimeData.Planes = GPUCollidersManager.planesBuffer;
		}

		// Token: 0x0400310C RID: 12556
		private readonly HairSettings settings;
	}
}
