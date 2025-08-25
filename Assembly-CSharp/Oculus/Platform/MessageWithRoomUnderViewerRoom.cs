using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000823 RID: 2083
	public class MessageWithRoomUnderViewerRoom : Message<Room>
	{
		// Token: 0x06003681 RID: 13953 RVA: 0x0010BA45 File Offset: 0x00109E45
		public MessageWithRoomUnderViewerRoom(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x06003682 RID: 13954 RVA: 0x0010BA4E File Offset: 0x00109E4E
		public override Room GetRoom()
		{
			return base.Data;
		}

		// Token: 0x06003683 RID: 13955 RVA: 0x0010BA58 File Offset: 0x00109E58
		protected override Room GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr o = CAPI.ovr_Message_GetRoom(obj);
			return new Room(o);
		}
	}
}
