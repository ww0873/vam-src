using System;
using System.Collections.Generic;
using Oculus.Platform.Models;
using UnityEngine;

namespace Oculus.Platform
{
	// Token: 0x0200087F RID: 2175
	public static class Rooms
	{
		// Token: 0x06003734 RID: 14132 RVA: 0x0010D65C File Offset: 0x0010BA5C
		public static Request<Room> UpdateDataStore(ulong roomID, Dictionary<string, string> data)
		{
			if (Core.IsInitialized())
			{
				CAPI.ovrKeyValuePair[] array = new CAPI.ovrKeyValuePair[data.Count];
				int num = 0;
				foreach (KeyValuePair<string, string> keyValuePair in data)
				{
					array[num++] = new CAPI.ovrKeyValuePair(keyValuePair.Key, keyValuePair.Value);
				}
				return new Request<Room>(CAPI.ovr_Room_UpdateDataStore(roomID, array));
			}
			return null;
		}

		// Token: 0x06003735 RID: 14133 RVA: 0x0010D6F8 File Offset: 0x0010BAF8
		public static void SetUpdateNotificationCallback(Message<Room>.Callback callback)
		{
			Callback.SetNotificationCallback<Room>(Message.MessageType.Notification_Room_RoomUpdate, callback);
		}

		// Token: 0x06003736 RID: 14134 RVA: 0x0010D705 File Offset: 0x0010BB05
		[Obsolete("Deprecated in favor of SetRoomInviteAcceptedNotificationCallback")]
		public static void SetRoomInviteNotificationCallback(Message<string>.Callback callback)
		{
			Callback.SetNotificationCallback<string>(Message.MessageType.Notification_Room_InviteAccepted, callback);
		}

		// Token: 0x06003737 RID: 14135 RVA: 0x0010D712 File Offset: 0x0010BB12
		public static void SetRoomInviteAcceptedNotificationCallback(Message<string>.Callback callback)
		{
			Callback.SetNotificationCallback<string>(Message.MessageType.Notification_Room_InviteAccepted, callback);
		}

		// Token: 0x06003738 RID: 14136 RVA: 0x0010D71F File Offset: 0x0010BB1F
		public static void SetRoomInviteReceivedNotificationCallback(Message<RoomInviteNotification>.Callback callback)
		{
			Callback.SetNotificationCallback<RoomInviteNotification>(Message.MessageType.Notification_Room_InviteReceived, callback);
		}

		// Token: 0x06003739 RID: 14137 RVA: 0x0010D72C File Offset: 0x0010BB2C
		public static Request<Room> CreateAndJoinPrivate(RoomJoinPolicy joinPolicy, uint maxUsers, bool subscribeToUpdates = false)
		{
			if (Core.IsInitialized())
			{
				return new Request<Room>(CAPI.ovr_Room_CreateAndJoinPrivate(joinPolicy, maxUsers, subscribeToUpdates));
			}
			return null;
		}

		// Token: 0x0600373A RID: 14138 RVA: 0x0010D747 File Offset: 0x0010BB47
		public static Request<Room> CreateAndJoinPrivate2(RoomJoinPolicy joinPolicy, uint maxUsers, RoomOptions roomOptions)
		{
			if (Core.IsInitialized())
			{
				return new Request<Room>(CAPI.ovr_Room_CreateAndJoinPrivate2(joinPolicy, maxUsers, (IntPtr)roomOptions));
			}
			return null;
		}

		// Token: 0x0600373B RID: 14139 RVA: 0x0010D767 File Offset: 0x0010BB67
		public static Request<Room> Get(ulong roomID)
		{
			if (Core.IsInitialized())
			{
				return new Request<Room>(CAPI.ovr_Room_Get(roomID));
			}
			return null;
		}

		// Token: 0x0600373C RID: 14140 RVA: 0x0010D780 File Offset: 0x0010BB80
		public static Request<Room> GetCurrent()
		{
			if (Core.IsInitialized())
			{
				return new Request<Room>(CAPI.ovr_Room_GetCurrent());
			}
			return null;
		}

		// Token: 0x0600373D RID: 14141 RVA: 0x0010D798 File Offset: 0x0010BB98
		public static Request<Room> GetCurrentForUser(ulong userID)
		{
			if (Core.IsInitialized())
			{
				return new Request<Room>(CAPI.ovr_Room_GetCurrentForUser(userID));
			}
			return null;
		}

		// Token: 0x0600373E RID: 14142 RVA: 0x0010D7B1 File Offset: 0x0010BBB1
		public static Request<UserList> GetInvitableUsers()
		{
			if (Core.IsInitialized())
			{
				return new Request<UserList>(CAPI.ovr_Room_GetInvitableUsers());
			}
			return null;
		}

		// Token: 0x0600373F RID: 14143 RVA: 0x0010D7C9 File Offset: 0x0010BBC9
		public static Request<UserList> GetInvitableUsers2(RoomOptions roomOptions = null)
		{
			if (Core.IsInitialized())
			{
				return new Request<UserList>(CAPI.ovr_Room_GetInvitableUsers2((IntPtr)roomOptions));
			}
			return null;
		}

		// Token: 0x06003740 RID: 14144 RVA: 0x0010D7E7 File Offset: 0x0010BBE7
		public static Request<RoomList> GetModeratedRooms()
		{
			if (Core.IsInitialized())
			{
				return new Request<RoomList>(CAPI.ovr_Room_GetModeratedRooms());
			}
			return null;
		}

		// Token: 0x06003741 RID: 14145 RVA: 0x0010D7FF File Offset: 0x0010BBFF
		public static Request<Room> InviteUser(ulong roomID, string inviteToken)
		{
			if (Core.IsInitialized())
			{
				return new Request<Room>(CAPI.ovr_Room_InviteUser(roomID, inviteToken));
			}
			return null;
		}

		// Token: 0x06003742 RID: 14146 RVA: 0x0010D819 File Offset: 0x0010BC19
		public static Request<Room> Join(ulong roomID, bool subscribeToUpdates = false)
		{
			if (Core.IsInitialized())
			{
				return new Request<Room>(CAPI.ovr_Room_Join(roomID, subscribeToUpdates));
			}
			return null;
		}

		// Token: 0x06003743 RID: 14147 RVA: 0x0010D833 File Offset: 0x0010BC33
		public static Request<Room> Join2(ulong roomID, RoomOptions roomOptions)
		{
			if (Core.IsInitialized())
			{
				return new Request<Room>(CAPI.ovr_Room_Join2(roomID, (IntPtr)roomOptions));
			}
			return null;
		}

		// Token: 0x06003744 RID: 14148 RVA: 0x0010D852 File Offset: 0x0010BC52
		public static Request<Room> KickUser(ulong roomID, ulong userID, int kickDurationSeconds)
		{
			if (Core.IsInitialized())
			{
				return new Request<Room>(CAPI.ovr_Room_KickUser(roomID, userID, kickDurationSeconds));
			}
			return null;
		}

		// Token: 0x06003745 RID: 14149 RVA: 0x0010D86D File Offset: 0x0010BC6D
		public static Request LaunchInvitableUserFlow(ulong roomID)
		{
			if (Core.IsInitialized())
			{
				return new Request(CAPI.ovr_Room_LaunchInvitableUserFlow(roomID));
			}
			return null;
		}

		// Token: 0x06003746 RID: 14150 RVA: 0x0010D886 File Offset: 0x0010BC86
		public static Request<Room> Leave(ulong roomID)
		{
			if (Core.IsInitialized())
			{
				return new Request<Room>(CAPI.ovr_Room_Leave(roomID));
			}
			return null;
		}

		// Token: 0x06003747 RID: 14151 RVA: 0x0010D89F File Offset: 0x0010BC9F
		public static Request<Room> SetDescription(ulong roomID, string description)
		{
			if (Core.IsInitialized())
			{
				return new Request<Room>(CAPI.ovr_Room_SetDescription(roomID, description));
			}
			return null;
		}

		// Token: 0x06003748 RID: 14152 RVA: 0x0010D8B9 File Offset: 0x0010BCB9
		public static Request<Room> UpdateMembershipLockStatus(ulong roomID, RoomMembershipLockStatus membershipLockStatus)
		{
			if (Core.IsInitialized())
			{
				return new Request<Room>(CAPI.ovr_Room_UpdateMembershipLockStatus(roomID, membershipLockStatus));
			}
			return null;
		}

		// Token: 0x06003749 RID: 14153 RVA: 0x0010D8D3 File Offset: 0x0010BCD3
		public static Request UpdateOwner(ulong roomID, ulong userID)
		{
			if (Core.IsInitialized())
			{
				return new Request(CAPI.ovr_Room_UpdateOwner(roomID, userID));
			}
			return null;
		}

		// Token: 0x0600374A RID: 14154 RVA: 0x0010D8ED File Offset: 0x0010BCED
		public static Request<Room> UpdatePrivateRoomJoinPolicy(ulong roomID, RoomJoinPolicy newJoinPolicy)
		{
			if (Core.IsInitialized())
			{
				return new Request<Room>(CAPI.ovr_Room_UpdatePrivateRoomJoinPolicy(roomID, newJoinPolicy));
			}
			return null;
		}

		// Token: 0x0600374B RID: 14155 RVA: 0x0010D907 File Offset: 0x0010BD07
		public static Request<RoomList> GetNextRoomListPage(RoomList list)
		{
			if (!list.HasNextPage)
			{
				Debug.LogWarning("Oculus.Platform.GetNextRoomListPage: List has no next page");
				return null;
			}
			if (Core.IsInitialized())
			{
				return new Request<RoomList>(CAPI.ovr_HTTP_GetWithMessageType(list.NextUrl, 1317239238));
			}
			return null;
		}
	}
}
