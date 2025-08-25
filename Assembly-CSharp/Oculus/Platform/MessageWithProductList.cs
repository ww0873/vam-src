using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x0200081E RID: 2078
	public class MessageWithProductList : Message<ProductList>
	{
		// Token: 0x06003672 RID: 13938 RVA: 0x0010B941 File Offset: 0x00109D41
		public MessageWithProductList(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x06003673 RID: 13939 RVA: 0x0010B94A File Offset: 0x00109D4A
		public override ProductList GetProductList()
		{
			return base.Data;
		}

		// Token: 0x06003674 RID: 13940 RVA: 0x0010B954 File Offset: 0x00109D54
		protected override ProductList GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr a = CAPI.ovr_Message_GetProductArray(obj);
			return new ProductList(a);
		}
	}
}
