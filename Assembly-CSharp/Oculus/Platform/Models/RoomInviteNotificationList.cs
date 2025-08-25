using System;
using System.Collections.Generic;

namespace Oculus.Platform.Models
{
	// Token: 0x0200086D RID: 2157
	public class RoomInviteNotificationList : DeserializableList<RoomInviteNotification>
	{
		// Token: 0x06003718 RID: 14104 RVA: 0x0010D038 File Offset: 0x0010B438
		public RoomInviteNotificationList(IntPtr a)
		{
			int num = (int)((uint)CAPI.ovr_RoomInviteNotificationArray_GetSize(a));
			this._Data = new List<RoomInviteNotification>(num);
			for (int i = 0; i < num; i++)
			{
				this._Data.Add(new RoomInviteNotification(CAPI.ovr_RoomInviteNotificationArray_GetElement(a, (UIntPtr)((ulong)((long)i)))));
			}
			this._NextUrl = CAPI.ovr_RoomInviteNotificationArray_GetNextUrl(a);
		}
	}
}
