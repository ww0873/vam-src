using System;
using System.ComponentModel;

namespace Oculus.Platform
{
	// Token: 0x020007F4 RID: 2036
	public enum LeaderboardStartAt
	{
		// Token: 0x04002743 RID: 10051
		[Description("TOP")]
		Top,
		// Token: 0x04002744 RID: 10052
		[Description("CENTERED_ON_VIEWER")]
		CenteredOnViewer,
		// Token: 0x04002745 RID: 10053
		[Description("CENTERED_ON_VIEWER_OR_TOP")]
		CenteredOnViewerOrTop,
		// Token: 0x04002746 RID: 10054
		[Description("UNKNOWN")]
		Unknown
	}
}
