using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000834 RID: 2100
	public class MessageWithMatchmakingNotification : Message<Room>
	{
		// Token: 0x060036B4 RID: 14004 RVA: 0x0010BDC5 File Offset: 0x0010A1C5
		public MessageWithMatchmakingNotification(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x060036B5 RID: 14005 RVA: 0x0010BDCE File Offset: 0x0010A1CE
		public override Room GetRoom()
		{
			return base.Data;
		}

		// Token: 0x060036B6 RID: 14006 RVA: 0x0010BDD8 File Offset: 0x0010A1D8
		protected override Room GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr o = CAPI.ovr_Message_GetRoom(obj);
			return new Room(o);
		}
	}
}
