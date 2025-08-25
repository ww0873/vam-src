using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000836 RID: 2102
	public class MessageWithHttpTransferUpdate : Message<HttpTransferUpdate>
	{
		// Token: 0x060036BB RID: 14011 RVA: 0x0010BE3D File Offset: 0x0010A23D
		public MessageWithHttpTransferUpdate(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x060036BC RID: 14012 RVA: 0x0010BE46 File Offset: 0x0010A246
		public override HttpTransferUpdate GetHttpTransferUpdate()
		{
			return base.Data;
		}

		// Token: 0x060036BD RID: 14013 RVA: 0x0010BE50 File Offset: 0x0010A250
		protected override HttpTransferUpdate GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr o = CAPI.ovr_Message_GetHttpTransferUpdate(obj);
			return new HttpTransferUpdate(o);
		}
	}
}
