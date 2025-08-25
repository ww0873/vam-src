using System;
using GPUTools.Common.Scripts.PL.Abstract;

namespace GPUTools.Common.Scripts.PL.Tools
{
	// Token: 0x020009B7 RID: 2487
	public class ExecuteAsPass : IPass
	{
		// Token: 0x06003EEF RID: 16111 RVA: 0x0012E3A9 File Offset: 0x0012C7A9
		public ExecuteAsPass(Action action)
		{
			this.action = action;
		}

		// Token: 0x06003EF0 RID: 16112 RVA: 0x0012E3B8 File Offset: 0x0012C7B8
		public void Dispatch()
		{
			this.action();
		}

		// Token: 0x06003EF1 RID: 16113 RVA: 0x0012E3C5 File Offset: 0x0012C7C5
		public void Dispose()
		{
		}

		// Token: 0x04002FE4 RID: 12260
		private readonly Action action;
	}
}
