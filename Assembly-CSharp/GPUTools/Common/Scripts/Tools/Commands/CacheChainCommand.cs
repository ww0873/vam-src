using System;
using System.Collections.Generic;

namespace GPUTools.Common.Scripts.Tools.Commands
{
	// Token: 0x020009C0 RID: 2496
	public class CacheChainCommand : ICacheCommand
	{
		// Token: 0x06003F1B RID: 16155 RVA: 0x0012E91C File Offset: 0x0012CD1C
		public CacheChainCommand()
		{
		}

		// Token: 0x06003F1C RID: 16156 RVA: 0x0012E930 File Offset: 0x0012CD30
		public void Cache()
		{
			foreach (ICacheCommand cacheCommand in this.commands)
			{
				cacheCommand.Cache();
			}
			this.OnCache();
		}

		// Token: 0x06003F1D RID: 16157 RVA: 0x0012E994 File Offset: 0x0012CD94
		protected void Add(ICacheCommand command)
		{
			this.commands.Add(command);
		}

		// Token: 0x06003F1E RID: 16158 RVA: 0x0012E9A2 File Offset: 0x0012CDA2
		protected virtual void OnCache()
		{
		}

		// Token: 0x04002FF1 RID: 12273
		private readonly List<ICacheCommand> commands = new List<ICacheCommand>();
	}
}
