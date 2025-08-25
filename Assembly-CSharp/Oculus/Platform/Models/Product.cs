using System;

namespace Oculus.Platform.Models
{
	// Token: 0x02000866 RID: 2150
	public class Product
	{
		// Token: 0x06003711 RID: 14097 RVA: 0x0010CCD9 File Offset: 0x0010B0D9
		public Product(IntPtr o)
		{
			this.Description = CAPI.ovr_Product_GetDescription(o);
			this.FormattedPrice = CAPI.ovr_Product_GetFormattedPrice(o);
			this.Name = CAPI.ovr_Product_GetName(o);
			this.Sku = CAPI.ovr_Product_GetSKU(o);
		}

		// Token: 0x0400285D RID: 10333
		public readonly string Description;

		// Token: 0x0400285E RID: 10334
		public readonly string FormattedPrice;

		// Token: 0x0400285F RID: 10335
		public readonly string Name;

		// Token: 0x04002860 RID: 10336
		public readonly string Sku;
	}
}
