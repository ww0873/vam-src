using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x0200081A RID: 2074
	public class MessageWithParty : Message<Party>
	{
		// Token: 0x06003666 RID: 13926 RVA: 0x0010B871 File Offset: 0x00109C71
		public MessageWithParty(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x06003667 RID: 13927 RVA: 0x0010B87A File Offset: 0x00109C7A
		public override Party GetParty()
		{
			return base.Data;
		}

		// Token: 0x06003668 RID: 13928 RVA: 0x0010B884 File Offset: 0x00109C84
		protected override Party GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr o = CAPI.ovr_Message_GetParty(obj);
			return new Party(o);
		}
	}
}
