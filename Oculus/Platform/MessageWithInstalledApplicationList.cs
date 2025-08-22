using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x0200080F RID: 2063
	public class MessageWithInstalledApplicationList : Message<InstalledApplicationList>
	{
		// Token: 0x06003645 RID: 13893 RVA: 0x0010B635 File Offset: 0x00109A35
		public MessageWithInstalledApplicationList(IntPtr c_message) : base(c_message)
		{
		}

		// Token: 0x06003646 RID: 13894 RVA: 0x0010B63E File Offset: 0x00109A3E
		public override InstalledApplicationList GetInstalledApplicationList()
		{
			return base.Data;
		}

		// Token: 0x06003647 RID: 13895 RVA: 0x0010B648 File Offset: 0x00109A48
		protected override InstalledApplicationList GetDataFromMessage(IntPtr c_message)
		{
			IntPtr obj = CAPI.ovr_Message_GetNativeMessage(c_message);
			IntPtr a = CAPI.ovr_Message_GetInstalledApplicationArray(obj);
			return new InstalledApplicationList(a);
		}
	}
}
