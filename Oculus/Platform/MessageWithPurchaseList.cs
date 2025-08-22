using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000820 RID: 2080
	public class MessageWithPurchaseList : Message<PurchaseList>
	{
		// Token: 0x06003678 RID: 13944 RVA: 0x0010B9A9 File Offset: 0x00109DA9
		public MessageWithPurchaseList(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x06003679 RID: 13945 RVA: 0x0010B9B2 File Offset: 0x00109DB2
		public override PurchaseList GetPurchaseList()
		{
			return base.Data;
		}

		// Token: 0x0600367A RID: 13946 RVA: 0x0010B9BC File Offset: 0x00109DBC
		protected override PurchaseList GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr a = CAPI.ovr_Message_GetPurchaseArray(obj);
			return new PurchaseList(a);
		}
	}
}
