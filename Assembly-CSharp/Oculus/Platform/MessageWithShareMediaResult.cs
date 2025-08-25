using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000828 RID: 2088
	public class MessageWithShareMediaResult : Message<ShareMediaResult>
	{
		// Token: 0x06003690 RID: 13968 RVA: 0x0010BB49 File Offset: 0x00109F49
		public MessageWithShareMediaResult(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x06003691 RID: 13969 RVA: 0x0010BB52 File Offset: 0x00109F52
		public override ShareMediaResult GetShareMediaResult()
		{
			return base.Data;
		}

		// Token: 0x06003692 RID: 13970 RVA: 0x0010BB5C File Offset: 0x00109F5C
		protected override ShareMediaResult GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr o = CAPI.ovr_Message_GetShareMediaResult(obj);
			return new ShareMediaResult(o);
		}
	}
}
