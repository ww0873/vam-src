using System;

namespace GPUTools.Common.Scripts.Tools.Commands
{
	// Token: 0x020009C1 RID: 2497
	public interface IBuildCommand
	{
		// Token: 0x06003F1F RID: 16159
		void Build();

		// Token: 0x06003F20 RID: 16160
		void Dispatch();

		// Token: 0x06003F21 RID: 16161
		void FixedDispatch();

		// Token: 0x06003F22 RID: 16162
		void UpdateSettings();

		// Token: 0x06003F23 RID: 16163
		void Dispose();
	}
}
