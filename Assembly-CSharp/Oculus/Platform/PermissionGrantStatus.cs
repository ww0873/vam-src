using System;
using System.ComponentModel;

namespace Oculus.Platform
{
	// Token: 0x0200087B RID: 2171
	public enum PermissionGrantStatus
	{
		// Token: 0x04002896 RID: 10390
		[Description("UNKNOWN")]
		Unknown,
		// Token: 0x04002897 RID: 10391
		[Description("GRANTED")]
		Granted,
		// Token: 0x04002898 RID: 10392
		[Description("DENIED")]
		Denied,
		// Token: 0x04002899 RID: 10393
		[Description("BLOCKED")]
		Blocked
	}
}
