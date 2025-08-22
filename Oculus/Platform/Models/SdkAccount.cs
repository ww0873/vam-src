using System;

namespace Oculus.Platform.Models
{
	// Token: 0x0200086E RID: 2158
	public class SdkAccount
	{
		// Token: 0x06003719 RID: 14105 RVA: 0x0010D09E File Offset: 0x0010B49E
		public SdkAccount(IntPtr o)
		{
			this.AccountType = CAPI.ovr_SdkAccount_GetAccountType(o);
			this.UserId = CAPI.ovr_SdkAccount_GetUserId(o);
		}

		// Token: 0x0400287B RID: 10363
		public readonly SdkAccountType AccountType;

		// Token: 0x0400287C RID: 10364
		public readonly ulong UserId;
	}
}
