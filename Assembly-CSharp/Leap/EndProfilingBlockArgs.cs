using System;

namespace Leap
{
	// Token: 0x020005D6 RID: 1494
	public struct EndProfilingBlockArgs
	{
		// Token: 0x060025BA RID: 9658 RVA: 0x000D6E33 File Offset: 0x000D5233
		public EndProfilingBlockArgs(string blockName)
		{
			this.blockName = blockName;
		}

		// Token: 0x04001F63 RID: 8035
		public string blockName;
	}
}
