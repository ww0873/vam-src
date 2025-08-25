using System;
using GPUTools.Common.Scripts.Tools.Commands;

namespace GPUTools.Cloth.Scripts.Runtime.Commands
{
	// Token: 0x02000999 RID: 2457
	public class BuildLineSpheres : BuildChainCommand
	{
		// Token: 0x06003D67 RID: 15719 RVA: 0x0012A0F3 File Offset: 0x001284F3
		public BuildLineSpheres(ClothSettings settings)
		{
			this.settings = settings;
		}

		// Token: 0x06003D68 RID: 15720 RVA: 0x0012A102 File Offset: 0x00128502
		protected override void OnBuild()
		{
			this.settings.Runtime.ProcessedLineSpheres = GPUCollidersManager.processedLineSpheresBuffer;
		}

		// Token: 0x06003D69 RID: 15721 RVA: 0x0012A119 File Offset: 0x00128519
		protected override void OnFixedDispatch()
		{
			this.settings.Runtime.ProcessedLineSpheres = GPUCollidersManager.processedLineSpheresBuffer;
		}

		// Token: 0x04002F2D RID: 12077
		private readonly ClothSettings settings;
	}
}
