using System;
using GPUTools.Common.Scripts.Tools.Commands;

namespace GPUTools.Cloth.Scripts.Runtime.Commands
{
	// Token: 0x0200099E RID: 2462
	public class BuildSpheres : BuildChainCommand
	{
		// Token: 0x06003D7C RID: 15740 RVA: 0x0012A7DD File Offset: 0x00128BDD
		public BuildSpheres(ClothSettings settings)
		{
			this.settings = settings;
		}

		// Token: 0x06003D7D RID: 15741 RVA: 0x0012A7EC File Offset: 0x00128BEC
		protected override void OnBuild()
		{
			this.settings.Runtime.ProcessedSpheres = GPUCollidersManager.processedSpheresBuffer;
		}

		// Token: 0x06003D7E RID: 15742 RVA: 0x0012A803 File Offset: 0x00128C03
		protected override void OnFixedDispatch()
		{
			this.settings.Runtime.ProcessedSpheres = GPUCollidersManager.processedSpheresBuffer;
		}

		// Token: 0x04002F38 RID: 12088
		private readonly ClothSettings settings;
	}
}
