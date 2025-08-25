using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000825 RID: 2085
	public class MessageWithRoomInviteNotification : Message<RoomInviteNotification>
	{
		// Token: 0x06003687 RID: 13959 RVA: 0x0010BAAD File Offset: 0x00109EAD
		public MessageWithRoomInviteNotification(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x06003688 RID: 13960 RVA: 0x0010BAB6 File Offset: 0x00109EB6
		public override RoomInviteNotification GetRoomInviteNotification()
		{
			return base.Data;
		}

		// Token: 0x06003689 RID: 13961 RVA: 0x0010BAC0 File Offset: 0x00109EC0
		protected override RoomInviteNotification GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr o = CAPI.ovr_Message_GetRoomInviteNotification(obj);
			return new RoomInviteNotification(o);
		}
	}
}
