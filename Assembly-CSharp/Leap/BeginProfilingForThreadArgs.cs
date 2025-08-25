using System;

namespace Leap
{
	// Token: 0x020005D3 RID: 1491
	public struct BeginProfilingForThreadArgs
	{
		// Token: 0x060025B8 RID: 9656 RVA: 0x000D6E1A File Offset: 0x000D521A
		public BeginProfilingForThreadArgs(string threadName, params string[] blockNames)
		{
			this.threadName = threadName;
			this.blockNames = blockNames;
		}

		// Token: 0x04001F60 RID: 8032
		public string threadName;

		// Token: 0x04001F61 RID: 8033
		public string[] blockNames;
	}
}
