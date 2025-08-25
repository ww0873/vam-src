using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x0200082C RID: 2092
	public class MessageWithUser : Message<User>
	{
		// Token: 0x0600369C RID: 13980 RVA: 0x0010BBFD File Offset: 0x00109FFD
		public MessageWithUser(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x0600369D RID: 13981 RVA: 0x0010BC06 File Offset: 0x0010A006
		public override User GetUser()
		{
			return base.Data;
		}

		// Token: 0x0600369E RID: 13982 RVA: 0x0010BC10 File Offset: 0x0010A010
		protected override User GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr o = CAPI.ovr_Message_GetUser(obj);
			return new User(o);
		}
	}
}
