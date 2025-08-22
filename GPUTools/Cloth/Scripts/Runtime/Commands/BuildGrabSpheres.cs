using System;
using GPUTools.Common.Scripts.Tools.Commands;

namespace GPUTools.Cloth.Scripts.Runtime.Commands
{
	// Token: 0x02000998 RID: 2456
	public class BuildGrabSpheres : BuildChainCommand
	{
		// Token: 0x06003D64 RID: 15716 RVA: 0x0012A0B6 File Offset: 0x001284B6
		public BuildGrabSpheres(ClothSettings settings)
		{
			this.settings = settings;
		}

		// Token: 0x06003D65 RID: 15717 RVA: 0x0012A0C5 File Offset: 0x001284C5
		protected override void OnBuild()
		{
			this.settings.Runtime.GrabSpheres = GPUCollidersManager.grabSpheresBuffer;
		}

		// Token: 0x06003D66 RID: 15718 RVA: 0x0012A0DC File Offset: 0x001284DC
		protected override void OnFixedDispatch()
		{
			this.settings.Runtime.GrabSpheres = GPUCollidersManager.grabSpheresBuffer;
		}

		// Token: 0x04002F2C RID: 12076
		private readonly ClothSettings settings;
	}
}
