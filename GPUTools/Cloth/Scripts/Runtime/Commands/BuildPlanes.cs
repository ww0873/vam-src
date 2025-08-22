using System;
using GPUTools.Common.Scripts.Tools.Commands;

namespace GPUTools.Cloth.Scripts.Runtime.Commands
{
	// Token: 0x0200099C RID: 2460
	public class BuildPlanes : BuildChainCommand
	{
		// Token: 0x06003D74 RID: 15732 RVA: 0x0012A54C File Offset: 0x0012894C
		public BuildPlanes(ClothSettings settings)
		{
			this.settings = settings;
		}

		// Token: 0x06003D75 RID: 15733 RVA: 0x0012A55B File Offset: 0x0012895B
		protected override void OnBuild()
		{
			this.settings.Runtime.Planes = GPUCollidersManager.planesBuffer;
		}

		// Token: 0x06003D76 RID: 15734 RVA: 0x0012A572 File Offset: 0x00128972
		protected override void OnFixedDispatch()
		{
			this.settings.Runtime.Planes = GPUCollidersManager.planesBuffer;
		}

		// Token: 0x04002F32 RID: 12082
		private readonly ClothSettings settings;
	}
}
