using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x0200082D RID: 2093
	public class MessageWithUserAndRoomList : Message<UserAndRoomList>
	{
		// Token: 0x0600369F RID: 13983 RVA: 0x0010BC31 File Offset: 0x0010A031
		public MessageWithUserAndRoomList(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x060036A0 RID: 13984 RVA: 0x0010BC3A File Offset: 0x0010A03A
		public override UserAndRoomList GetUserAndRoomList()
		{
			return base.Data;
		}

		// Token: 0x060036A1 RID: 13985 RVA: 0x0010BC44 File Offset: 0x0010A044
		protected override UserAndRoomList GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr a = CAPI.ovr_Message_GetUserAndRoomArray(obj);
			return new UserAndRoomList(a);
		}
	}
}
