using System;
using System.ComponentModel;

namespace Oculus.Platform
{
	// Token: 0x020007F5 RID: 2037
	public enum LivestreamingAudience
	{
		// Token: 0x04002748 RID: 10056
		[Description("UNKNOWN")]
		Unknown,
		// Token: 0x04002749 RID: 10057
		[Description("PUBLIC")]
		Public,
		// Token: 0x0400274A RID: 10058
		[Description("FRIENDS")]
		Friends,
		// Token: 0x0400274B RID: 10059
		[Description("ONLY_ME")]
		OnlyMe
	}
}
