using System;

namespace Oculus.Platform.Models
{
	// Token: 0x02000868 RID: 2152
	public class Purchase
	{
		// Token: 0x06003713 RID: 14099 RVA: 0x0010CD7A File Offset: 0x0010B17A
		public Purchase(IntPtr o)
		{
			this.ExpirationTime = CAPI.ovr_Purchase_GetExpirationTime(o);
			this.GrantTime = CAPI.ovr_Purchase_GetGrantTime(o);
			this.ID = CAPI.ovr_Purchase_GetPurchaseID(o);
			this.Sku = CAPI.ovr_Purchase_GetSKU(o);
		}

		// Token: 0x04002861 RID: 10337
		public readonly DateTime ExpirationTime;

		// Token: 0x04002862 RID: 10338
		public readonly DateTime GrantTime;

		// Token: 0x04002863 RID: 10339
		public readonly ulong ID;

		// Token: 0x04002864 RID: 10340
		public readonly string Sku;
	}
}
