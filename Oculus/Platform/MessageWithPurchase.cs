using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x0200081F RID: 2079
	public class MessageWithPurchase : Message<Purchase>
	{
		// Token: 0x06003675 RID: 13941 RVA: 0x0010B975 File Offset: 0x00109D75
		public MessageWithPurchase(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x06003676 RID: 13942 RVA: 0x0010B97E File Offset: 0x00109D7E
		public override Purchase GetPurchase()
		{
			return base.Data;
		}

		// Token: 0x06003677 RID: 13943 RVA: 0x0010B988 File Offset: 0x00109D88
		protected override Purchase GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr o = CAPI.ovr_Message_GetPurchase(obj);
			return new Purchase(o);
		}
	}
}
