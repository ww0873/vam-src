using System;
using System.ComponentModel;

namespace Oculus.Platform
{
	// Token: 0x0200089B RID: 2203
	public enum RoomJoinability
	{
		// Token: 0x040028D8 RID: 10456
		[Description("UNKNOWN")]
		Unknown,
		// Token: 0x040028D9 RID: 10457
		[Description("ARE_IN")]
		AreIn,
		// Token: 0x040028DA RID: 10458
		[Description("ARE_KICKED")]
		AreKicked,
		// Token: 0x040028DB RID: 10459
		[Description("CAN_JOIN")]
		CanJoin,
		// Token: 0x040028DC RID: 10460
		[Description("IS_FULL")]
		IsFull,
		// Token: 0x040028DD RID: 10461
		[Description("NO_VIEWER")]
		NoViewer,
		// Token: 0x040028DE RID: 10462
		[Description("POLICY_PREVENTS")]
		PolicyPrevents
	}
}
