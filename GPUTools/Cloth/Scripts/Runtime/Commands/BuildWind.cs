using System;
using GPUTools.Common.Scripts.Tools.Commands;
using GPUTools.Physics.Scripts.Wind;
using UnityEngine;

namespace GPUTools.Cloth.Scripts.Runtime.Commands
{
	// Token: 0x020009A1 RID: 2465
	public class BuildWind : BuildChainCommand
	{
		// Token: 0x06003D87 RID: 15751 RVA: 0x0012ABBE File Offset: 0x00128FBE
		public BuildWind(ClothSettings settings)
		{
			this.settings = settings;
			this.receiver = new WindReceiver();
		}

		// Token: 0x06003D88 RID: 15752 RVA: 0x0012ABD8 File Offset: 0x00128FD8
		protected override void OnDispatch()
		{
			Vector3 wind = this.receiver.GetWind(this.settings.transform.position) * this.settings.WindMultiplier;
			this.settings.Runtime.Wind = wind;
		}

		// Token: 0x04002F3D RID: 12093
		private readonly ClothSettings settings;

		// Token: 0x04002F3E RID: 12094
		private readonly WindReceiver receiver;
	}
}
