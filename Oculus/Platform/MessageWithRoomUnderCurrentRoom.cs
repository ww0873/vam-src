using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000822 RID: 2082
	public class MessageWithRoomUnderCurrentRoom : Message<Room>
	{
		// Token: 0x0600367E RID: 13950 RVA: 0x0010BA11 File Offset: 0x00109E11
		public MessageWithRoomUnderCurrentRoom(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x0600367F RID: 13951 RVA: 0x0010BA1A File Offset: 0x00109E1A
		public override Room GetRoom()
		{
			return base.Data;
		}

		// Token: 0x06003680 RID: 13952 RVA: 0x0010BA24 File Offset: 0x00109E24
		protected override Room GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr o = CAPI.ovr_Message_GetRoom(obj);
			return new Room(o);
		}
	}
}
