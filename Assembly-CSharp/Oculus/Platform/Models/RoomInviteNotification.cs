using System;

namespace Oculus.Platform.Models
{
	// Token: 0x0200086C RID: 2156
	public class RoomInviteNotification
	{
		// Token: 0x06003717 RID: 14103 RVA: 0x0010D00A File Offset: 0x0010B40A
		public RoomInviteNotification(IntPtr o)
		{
			this.ID = CAPI.ovr_RoomInviteNotification_GetID(o);
			this.RoomID = CAPI.ovr_RoomInviteNotification_GetRoomID(o);
			this.SentTime = CAPI.ovr_RoomInviteNotification_GetSentTime(o);
		}

		// Token: 0x04002878 RID: 10360
		public readonly ulong ID;

		// Token: 0x04002879 RID: 10361
		public readonly ulong RoomID;

		// Token: 0x0400287A RID: 10362
		public readonly DateTime SentTime;
	}
}
