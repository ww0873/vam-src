using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000824 RID: 2084
	public class MessageWithRoomList : Message<RoomList>
	{
		// Token: 0x06003684 RID: 13956 RVA: 0x0010BA79 File Offset: 0x00109E79
		public MessageWithRoomList(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x06003685 RID: 13957 RVA: 0x0010BA82 File Offset: 0x00109E82
		public override RoomList GetRoomList()
		{
			return base.Data;
		}

		// Token: 0x06003686 RID: 13958 RVA: 0x0010BA8C File Offset: 0x00109E8C
		protected override RoomList GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr a = CAPI.ovr_Message_GetRoomArray(obj);
			return new RoomList(a);
		}
	}
}
