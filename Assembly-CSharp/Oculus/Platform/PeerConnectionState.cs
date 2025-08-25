using System;
using System.ComponentModel;

namespace Oculus.Platform
{
	// Token: 0x0200087A RID: 2170
	public enum PeerConnectionState
	{
		// Token: 0x04002891 RID: 10385
		[Description("UNKNOWN")]
		Unknown,
		// Token: 0x04002892 RID: 10386
		[Description("CONNECTED")]
		Connected,
		// Token: 0x04002893 RID: 10387
		[Description("TIMEOUT")]
		Timeout,
		// Token: 0x04002894 RID: 10388
		[Description("CLOSED")]
		Closed
	}
}
