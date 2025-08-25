using System;
using System.ComponentModel;

namespace Oculus.Platform
{
	// Token: 0x020007F7 RID: 2039
	public enum LivestreamingStartStatus
	{
		// Token: 0x04002751 RID: 10065
		[Description("SUCCESS")]
		Success = 1,
		// Token: 0x04002752 RID: 10066
		[Description("UNKNOWN")]
		Unknown = 0,
		// Token: 0x04002753 RID: 10067
		[Description("NO_PACKAGE_SET")]
		NoPackageSet = -1,
		// Token: 0x04002754 RID: 10068
		[Description("NO_FB_CONNECT")]
		NoFbConnect = -2,
		// Token: 0x04002755 RID: 10069
		[Description("NO_SESSION_ID")]
		NoSessionId = -3,
		// Token: 0x04002756 RID: 10070
		[Description("MISSING_PARAMETERS")]
		MissingParameters = -4
	}
}
