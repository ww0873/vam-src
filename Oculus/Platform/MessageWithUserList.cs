using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x0200082E RID: 2094
	public class MessageWithUserList : Message<UserList>
	{
		// Token: 0x060036A2 RID: 13986 RVA: 0x0010BC65 File Offset: 0x0010A065
		public MessageWithUserList(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x060036A3 RID: 13987 RVA: 0x0010BC6E File Offset: 0x0010A06E
		public override UserList GetUserList()
		{
			return base.Data;
		}

		// Token: 0x060036A4 RID: 13988 RVA: 0x0010BC78 File Offset: 0x0010A078
		protected override UserList GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr a = CAPI.ovr_Message_GetUserArray(obj);
			return new UserList(a);
		}
	}
}
