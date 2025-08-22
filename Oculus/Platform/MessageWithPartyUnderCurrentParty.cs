using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x0200081B RID: 2075
	public class MessageWithPartyUnderCurrentParty : Message<Party>
	{
		// Token: 0x06003669 RID: 13929 RVA: 0x0010B8A5 File Offset: 0x00109CA5
		public MessageWithPartyUnderCurrentParty(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x0600366A RID: 13930 RVA: 0x0010B8AE File Offset: 0x00109CAE
		public override Party GetParty()
		{
			return base.Data;
		}

		// Token: 0x0600366B RID: 13931 RVA: 0x0010B8B8 File Offset: 0x00109CB8
		protected override Party GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr o = CAPI.ovr_Message_GetParty(obj);
			return new Party(o);
		}
	}
}
