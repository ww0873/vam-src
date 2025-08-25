using System;

namespace Leap
{
	// Token: 0x020005D5 RID: 1493
	public struct BeginProfilingBlockArgs
	{
		// Token: 0x060025B9 RID: 9657 RVA: 0x000D6E2A File Offset: 0x000D522A
		public BeginProfilingBlockArgs(string blockName)
		{
			this.blockName = blockName;
		}

		// Token: 0x04001F62 RID: 8034
		public string blockName;
	}
}
