using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000827 RID: 2087
	public class MessageWithSdkAccountList : Message<SdkAccountList>
	{
		// Token: 0x0600368D RID: 13965 RVA: 0x0010BB15 File Offset: 0x00109F15
		public MessageWithSdkAccountList(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x0600368E RID: 13966 RVA: 0x0010BB1E File Offset: 0x00109F1E
		public override SdkAccountList GetSdkAccountList()
		{
			return base.Data;
		}

		// Token: 0x0600368F RID: 13967 RVA: 0x0010BB28 File Offset: 0x00109F28
		protected override SdkAccountList GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr a = CAPI.ovr_Message_GetSdkAccountArray(obj);
			return new SdkAccountList(a);
		}
	}
}
