using System;
using System.ComponentModel;

namespace Oculus.Platform
{
	// Token: 0x0200089C RID: 2204
	public enum RoomJoinPolicy
	{
		// Token: 0x040028E0 RID: 10464
		[Description("NONE")]
		None,
		// Token: 0x040028E1 RID: 10465
		[Description("EVERYONE")]
		Everyone,
		// Token: 0x040028E2 RID: 10466
		[Description("FRIENDS_OF_MEMBERS")]
		FriendsOfMembers,
		// Token: 0x040028E3 RID: 10467
		[Description("FRIENDS_OF_OWNER")]
		FriendsOfOwner,
		// Token: 0x040028E4 RID: 10468
		[Description("INVITED_USERS")]
		InvitedUsers,
		// Token: 0x040028E5 RID: 10469
		[Description("UNKNOWN")]
		Unknown
	}
}
