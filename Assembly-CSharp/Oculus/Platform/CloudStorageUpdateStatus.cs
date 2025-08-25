using System;
using System.ComponentModel;

namespace Oculus.Platform
{
	// Token: 0x020007EE RID: 2030
	public enum CloudStorageUpdateStatus
	{
		// Token: 0x0400272F RID: 10031
		[Description("UNKNOWN")]
		Unknown,
		// Token: 0x04002730 RID: 10032
		[Description("OK")]
		Ok,
		// Token: 0x04002731 RID: 10033
		[Description("BETTER_VERSION_STORED")]
		BetterVersionStored,
		// Token: 0x04002732 RID: 10034
		[Description("MANUAL_MERGE_REQUIRED")]
		ManualMergeRequired
	}
}
