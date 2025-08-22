using System;
using System.ComponentModel;

namespace Oculus.Platform
{
	// Token: 0x020007F2 RID: 2034
	public enum LaunchType
	{
		// Token: 0x04002739 RID: 10041
		[Description("UNKNOWN")]
		Unknown,
		// Token: 0x0400273A RID: 10042
		[Description("NORMAL")]
		Normal,
		// Token: 0x0400273B RID: 10043
		[Description("INVITE")]
		Invite,
		// Token: 0x0400273C RID: 10044
		[Description("COORDINATED")]
		Coordinated,
		// Token: 0x0400273D RID: 10045
		[Description("DEEPLINK")]
		Deeplink
	}
}
