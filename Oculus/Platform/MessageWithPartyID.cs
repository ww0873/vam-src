using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x0200081C RID: 2076
	public class MessageWithPartyID : Message<PartyID>
	{
		// Token: 0x0600366C RID: 13932 RVA: 0x0010B8D9 File Offset: 0x00109CD9
		public MessageWithPartyID(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x0600366D RID: 13933 RVA: 0x0010B8E2 File Offset: 0x00109CE2
		public override PartyID GetPartyID()
		{
			return base.Data;
		}

		// Token: 0x0600366E RID: 13934 RVA: 0x0010B8EC File Offset: 0x00109CEC
		protected override PartyID GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr o = CAPI.ovr_Message_GetPartyID(obj);
			return new PartyID(o);
		}
	}
}
