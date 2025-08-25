using System;
using Oculus.Platform.Models;
using UnityEngine;

namespace Oculus.Platform
{
	// Token: 0x02000893 RID: 2195
	public static class Users
	{
		// Token: 0x060037A5 RID: 14245 RVA: 0x0010E5A2 File Offset: 0x0010C9A2
		public static Request<User> Get(ulong userID)
		{
			if (Core.IsInitialized())
			{
				return new Request<User>(CAPI.ovr_User_Get(userID));
			}
			return null;
		}

		// Token: 0x060037A6 RID: 14246 RVA: 0x0010E5BB File Offset: 0x0010C9BB
		public static Request<string> GetAccessToken()
		{
			if (Core.IsInitialized())
			{
				return new Request<string>(CAPI.ovr_User_GetAccessToken());
			}
			return null;
		}

		// Token: 0x060037A7 RID: 14247 RVA: 0x0010E5D3 File Offset: 0x0010C9D3
		public static Request<User> GetLoggedInUser()
		{
			if (Core.IsInitialized())
			{
				return new Request<User>(CAPI.ovr_User_GetLoggedInUser());
			}
			return null;
		}

		// Token: 0x060037A8 RID: 14248 RVA: 0x0010E5EB File Offset: 0x0010C9EB
		public static Request<UserList> GetLoggedInUserFriends()
		{
			if (Core.IsInitialized())
			{
				return new Request<UserList>(CAPI.ovr_User_GetLoggedInUserFriends());
			}
			return null;
		}

		// Token: 0x060037A9 RID: 14249 RVA: 0x0010E603 File Offset: 0x0010CA03
		public static Request<UserAndRoomList> GetLoggedInUserFriendsAndRooms()
		{
			if (Core.IsInitialized())
			{
				return new Request<UserAndRoomList>(CAPI.ovr_User_GetLoggedInUserFriendsAndRooms());
			}
			return null;
		}

		// Token: 0x060037AA RID: 14250 RVA: 0x0010E61B File Offset: 0x0010CA1B
		public static Request<UserAndRoomList> GetLoggedInUserRecentlyMetUsersAndRooms(UserOptions userOptions = null)
		{
			if (Core.IsInitialized())
			{
				return new Request<UserAndRoomList>(CAPI.ovr_User_GetLoggedInUserRecentlyMetUsersAndRooms((IntPtr)userOptions));
			}
			return null;
		}

		// Token: 0x060037AB RID: 14251 RVA: 0x0010E639 File Offset: 0x0010CA39
		public static Request<OrgScopedID> GetOrgScopedID(ulong userID)
		{
			if (Core.IsInitialized())
			{
				return new Request<OrgScopedID>(CAPI.ovr_User_GetOrgScopedID(userID));
			}
			return null;
		}

		// Token: 0x060037AC RID: 14252 RVA: 0x0010E652 File Offset: 0x0010CA52
		public static Request<SdkAccountList> GetSdkAccounts()
		{
			if (Core.IsInitialized())
			{
				return new Request<SdkAccountList>(CAPI.ovr_User_GetSdkAccounts());
			}
			return null;
		}

		// Token: 0x060037AD RID: 14253 RVA: 0x0010E66A File Offset: 0x0010CA6A
		public static Request<UserProof> GetUserProof()
		{
			if (Core.IsInitialized())
			{
				return new Request<UserProof>(CAPI.ovr_User_GetUserProof());
			}
			return null;
		}

		// Token: 0x060037AE RID: 14254 RVA: 0x0010E682 File Offset: 0x0010CA82
		public static Request LaunchProfile(ulong userID)
		{
			if (Core.IsInitialized())
			{
				return new Request(CAPI.ovr_User_LaunchProfile(userID));
			}
			return null;
		}

		// Token: 0x060037AF RID: 14255 RVA: 0x0010E69B File Offset: 0x0010CA9B
		public static Request<UserAndRoomList> GetNextUserAndRoomListPage(UserAndRoomList list)
		{
			if (!list.HasNextPage)
			{
				Debug.LogWarning("Oculus.Platform.GetNextUserAndRoomListPage: List has no next page");
				return null;
			}
			if (Core.IsInitialized())
			{
				return new Request<UserAndRoomList>(CAPI.ovr_HTTP_GetWithMessageType(list.NextUrl, 2143146719));
			}
			return null;
		}

		// Token: 0x060037B0 RID: 14256 RVA: 0x0010E6D5 File Offset: 0x0010CAD5
		public static Request<UserList> GetNextUserListPage(UserList list)
		{
			if (!list.HasNextPage)
			{
				Debug.LogWarning("Oculus.Platform.GetNextUserListPage: List has no next page");
				return null;
			}
			if (Core.IsInitialized())
			{
				return new Request<UserList>(CAPI.ovr_HTTP_GetWithMessageType(list.NextUrl, 645723971));
			}
			return null;
		}
	}
}
