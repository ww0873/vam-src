using System;
using System.Collections.Generic;

namespace Oculus.Platform.Models
{
	// Token: 0x0200086A RID: 2154
	public class Room
	{
		// Token: 0x06003715 RID: 14101 RVA: 0x0010CE1C File Offset: 0x0010B21C
		public Room(IntPtr o)
		{
			this.ApplicationID = CAPI.ovr_Room_GetApplicationID(o);
			this.DataStore = CAPI.DataStoreFromNative(CAPI.ovr_Room_GetDataStore(o));
			this.Description = CAPI.ovr_Room_GetDescription(o);
			this.ID = CAPI.ovr_Room_GetID(o);
			IntPtr intPtr = CAPI.ovr_Room_GetInvitedUsers(o);
			this.InvitedUsers = new UserList(intPtr);
			if (intPtr == IntPtr.Zero)
			{
				this.InvitedUsersOptional = null;
			}
			else
			{
				this.InvitedUsersOptional = this.InvitedUsers;
			}
			this.IsMembershipLocked = CAPI.ovr_Room_GetIsMembershipLocked(o);
			this.JoinPolicy = CAPI.ovr_Room_GetJoinPolicy(o);
			this.Joinability = CAPI.ovr_Room_GetJoinability(o);
			IntPtr intPtr2 = CAPI.ovr_Room_GetMatchedUsers(o);
			this.MatchedUsers = new MatchmakingEnqueuedUserList(intPtr2);
			if (intPtr2 == IntPtr.Zero)
			{
				this.MatchedUsersOptional = null;
			}
			else
			{
				this.MatchedUsersOptional = this.MatchedUsers;
			}
			this.MaxUsers = CAPI.ovr_Room_GetMaxUsers(o);
			this.Name = CAPI.ovr_Room_GetName(o);
			IntPtr intPtr3 = CAPI.ovr_Room_GetOwner(o);
			this.Owner = new User(intPtr3);
			if (intPtr3 == IntPtr.Zero)
			{
				this.OwnerOptional = null;
			}
			else
			{
				this.OwnerOptional = this.Owner;
			}
			this.Type = CAPI.ovr_Room_GetType(o);
			IntPtr intPtr4 = CAPI.ovr_Room_GetUsers(o);
			this.Users = new UserList(intPtr4);
			if (intPtr4 == IntPtr.Zero)
			{
				this.UsersOptional = null;
			}
			else
			{
				this.UsersOptional = this.Users;
			}
			this.Version = CAPI.ovr_Room_GetVersion(o);
		}

		// Token: 0x04002865 RID: 10341
		public readonly ulong ApplicationID;

		// Token: 0x04002866 RID: 10342
		public readonly Dictionary<string, string> DataStore;

		// Token: 0x04002867 RID: 10343
		public readonly string Description;

		// Token: 0x04002868 RID: 10344
		public readonly ulong ID;

		// Token: 0x04002869 RID: 10345
		public readonly UserList InvitedUsersOptional;

		// Token: 0x0400286A RID: 10346
		[Obsolete("Deprecated in favor of InvitedUsersOptional")]
		public readonly UserList InvitedUsers;

		// Token: 0x0400286B RID: 10347
		public readonly bool IsMembershipLocked;

		// Token: 0x0400286C RID: 10348
		public readonly RoomJoinPolicy JoinPolicy;

		// Token: 0x0400286D RID: 10349
		public readonly RoomJoinability Joinability;

		// Token: 0x0400286E RID: 10350
		public readonly MatchmakingEnqueuedUserList MatchedUsersOptional;

		// Token: 0x0400286F RID: 10351
		[Obsolete("Deprecated in favor of MatchedUsersOptional")]
		public readonly MatchmakingEnqueuedUserList MatchedUsers;

		// Token: 0x04002870 RID: 10352
		public readonly uint MaxUsers;

		// Token: 0x04002871 RID: 10353
		public readonly string Name;

		// Token: 0x04002872 RID: 10354
		public readonly User OwnerOptional;

		// Token: 0x04002873 RID: 10355
		[Obsolete("Deprecated in favor of OwnerOptional")]
		public readonly User Owner;

		// Token: 0x04002874 RID: 10356
		public readonly RoomType Type;

		// Token: 0x04002875 RID: 10357
		public readonly UserList UsersOptional;

		// Token: 0x04002876 RID: 10358
		[Obsolete("Deprecated in favor of UsersOptional")]
		public readonly UserList Users;

		// Token: 0x04002877 RID: 10359
		public readonly uint Version;
	}
}
