using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000821 RID: 2081
	public class MessageWithRoom : Message<Room>
	{
		// Token: 0x0600367B RID: 13947 RVA: 0x0010B9DD File Offset: 0x00109DDD
		public MessageWithRoom(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x0600367C RID: 13948 RVA: 0x0010B9E6 File Offset: 0x00109DE6
		public override Room GetRoom()
		{
			return base.Data;
		}

		// Token: 0x0600367D RID: 13949 RVA: 0x0010B9F0 File Offset: 0x00109DF0
		protected override Room GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr o = CAPI.ovr_Message_GetRoom(obj);
			return new Room(o);
		}
	}
}
