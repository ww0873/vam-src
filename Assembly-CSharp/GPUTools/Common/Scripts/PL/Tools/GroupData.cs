using System;

namespace GPUTools.Common.Scripts.PL.Tools
{
	// Token: 0x020009BC RID: 2492
	public struct GroupData
	{
		// Token: 0x06003F05 RID: 16133 RVA: 0x0012E681 File Offset: 0x0012CA81
		public GroupData(int start, int num)
		{
			this.Start = start;
			this.Num = num;
		}

		// Token: 0x04002FEA RID: 12266
		public int Start;

		// Token: 0x04002FEB RID: 12267
		public int Num;
	}
}
