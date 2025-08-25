using System;
using Oculus.Platform.Models;
using UnityEngine;

namespace Oculus.Platform
{
	// Token: 0x02000891 RID: 2193
	public static class Notifications
	{
		// Token: 0x060037A1 RID: 14241 RVA: 0x0010E51F File Offset: 0x0010C91F
		public static Request<RoomInviteNotificationList> GetRoomInviteNotifications()
		{
			if (Core.IsInitialized())
			{
				return new Request<RoomInviteNotificationList>(CAPI.ovr_Notification_GetRoomInvites());
			}
			return null;
		}

		// Token: 0x060037A2 RID: 14242 RVA: 0x0010E537 File Offset: 0x0010C937
		public static Request MarkAsRead(ulong notificationID)
		{
			if (Core.IsInitialized())
			{
				return new Request(CAPI.ovr_Notification_MarkAsRead(notificationID));
			}
			return null;
		}

		// Token: 0x060037A3 RID: 14243 RVA: 0x0010E550 File Offset: 0x0010C950
		public static Request<RoomInviteNotificationList> GetNextRoomInviteNotificationListPage(RoomInviteNotificationList list)
		{
			if (!list.HasNextPage)
			{
				Debug.LogWarning("Oculus.Platform.GetNextRoomInviteNotificationListPage: List has no next page");
				return null;
			}
			if (Core.IsInitialized())
			{
				return new Request<RoomInviteNotificationList>(CAPI.ovr_HTTP_GetWithMessageType(list.NextUrl, 102890359));
			}
			return null;
		}
	}
}
