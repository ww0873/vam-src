using System;

namespace Oculus.Platform.Models
{
	// Token: 0x02000860 RID: 2144
	public class Party
	{
		// Token: 0x06003707 RID: 14087 RVA: 0x0010CADC File Offset: 0x0010AEDC
		public Party(IntPtr o)
		{
			this.ID = CAPI.ovr_Party_GetID(o);
			IntPtr intPtr = CAPI.ovr_Party_GetInvitedUsers(o);
			this.InvitedUsers = new UserList(intPtr);
			if (intPtr == IntPtr.Zero)
			{
				this.InvitedUsersOptional = null;
			}
			else
			{
				this.InvitedUsersOptional = this.InvitedUsers;
			}
			IntPtr intPtr2 = CAPI.ovr_Party_GetLeader(o);
			this.Leader = new User(intPtr2);
			if (intPtr2 == IntPtr.Zero)
			{
				this.LeaderOptional = null;
			}
			else
			{
				this.LeaderOptional = this.Leader;
			}
			IntPtr intPtr3 = CAPI.ovr_Party_GetRoom(o);
			this.Room = new Room(intPtr3);
			if (intPtr3 == IntPtr.Zero)
			{
				this.RoomOptional = null;
			}
			else
			{
				this.RoomOptional = this.Room;
			}
			IntPtr intPtr4 = CAPI.ovr_Party_GetUsers(o);
			this.Users = new UserList(intPtr4);
			if (intPtr4 == IntPtr.Zero)
			{
				this.UsersOptional = null;
			}
			else
			{
				this.UsersOptional = this.Users;
			}
		}

		// Token: 0x0400284F RID: 10319
		public readonly ulong ID;

		// Token: 0x04002850 RID: 10320
		public readonly UserList InvitedUsersOptional;

		// Token: 0x04002851 RID: 10321
		[Obsolete("Deprecated in favor of InvitedUsersOptional")]
		public readonly UserList InvitedUsers;

		// Token: 0x04002852 RID: 10322
		public readonly User LeaderOptional;

		// Token: 0x04002853 RID: 10323
		[Obsolete("Deprecated in favor of LeaderOptional")]
		public readonly User Leader;

		// Token: 0x04002854 RID: 10324
		public readonly Room RoomOptional;

		// Token: 0x04002855 RID: 10325
		[Obsolete("Deprecated in favor of RoomOptional")]
		public readonly Room Room;

		// Token: 0x04002856 RID: 10326
		public readonly UserList UsersOptional;

		// Token: 0x04002857 RID: 10327
		[Obsolete("Deprecated in favor of UsersOptional")]
		public readonly UserList Users;
	}
}
