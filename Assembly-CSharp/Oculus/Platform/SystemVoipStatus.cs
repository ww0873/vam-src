using System;
using System.ComponentModel;

namespace Oculus.Platform
{
	// Token: 0x020008A6 RID: 2214
	public enum SystemVoipStatus
	{
		// Token: 0x040028FE RID: 10494
		[Description("UNKNOWN")]
		Unknown,
		// Token: 0x040028FF RID: 10495
		[Description("UNAVAILABLE")]
		Unavailable,
		// Token: 0x04002900 RID: 10496
		[Description("SUPPRESSED")]
		Suppressed,
		// Token: 0x04002901 RID: 10497
		[Description("ACTIVE")]
		Active
	}
}
