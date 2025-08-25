using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000826 RID: 2086
	public class MessageWithRoomInviteNotificationList : Message<RoomInviteNotificationList>
	{
		// Token: 0x0600368A RID: 13962 RVA: 0x0010BAE1 File Offset: 0x00109EE1
		public MessageWithRoomInviteNotificationList(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x0600368B RID: 13963 RVA: 0x0010BAEA File Offset: 0x00109EEA
		public override RoomInviteNotificationList GetRoomInviteNotificationList()
		{
			return base.Data;
		}

		// Token: 0x0600368C RID: 13964 RVA: 0x0010BAF4 File Offset: 0x00109EF4
		protected override RoomInviteNotificationList GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr a = CAPI.ovr_Message_GetRoomInviteNotificationArray(obj);
			return new RoomInviteNotificationList(a);
		}
	}
}
