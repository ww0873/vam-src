using System;
using Oculus.Platform.Models;
using UnityEngine;

namespace Oculus.Platform
{
	// Token: 0x0200088F RID: 2191
	public static class IAP
	{
		// Token: 0x0600379A RID: 14234 RVA: 0x0010E409 File Offset: 0x0010C809
		public static Request ConsumePurchase(string sku)
		{
			if (Core.IsInitialized())
			{
				return new Request(CAPI.ovr_IAP_ConsumePurchase(sku));
			}
			return null;
		}

		// Token: 0x0600379B RID: 14235 RVA: 0x0010E422 File Offset: 0x0010C822
		public static Request<ProductList> GetProductsBySKU(string[] skus)
		{
			if (Core.IsInitialized())
			{
				return new Request<ProductList>(CAPI.ovr_IAP_GetProductsBySKU(skus, (skus == null) ? 0 : skus.Length));
			}
			return null;
		}

		// Token: 0x0600379C RID: 14236 RVA: 0x0010E44A File Offset: 0x0010C84A
		public static Request<PurchaseList> GetViewerPurchases()
		{
			if (Core.IsInitialized())
			{
				return new Request<PurchaseList>(CAPI.ovr_IAP_GetViewerPurchases());
			}
			return null;
		}

		// Token: 0x0600379D RID: 14237 RVA: 0x0010E462 File Offset: 0x0010C862
		public static Request<Purchase> LaunchCheckoutFlow(string sku)
		{
			if (!Core.IsInitialized())
			{
				return null;
			}
			if (Application.isEditor)
			{
				throw new NotImplementedException("LaunchCheckoutFlow() is not implemented in the editor yet.");
			}
			return new Request<Purchase>(CAPI.ovr_IAP_LaunchCheckoutFlow(sku));
		}

		// Token: 0x0600379E RID: 14238 RVA: 0x0010E490 File Offset: 0x0010C890
		public static Request<ProductList> GetNextProductListPage(ProductList list)
		{
			if (!list.HasNextPage)
			{
				Debug.LogWarning("Oculus.Platform.GetNextProductListPage: List has no next page");
				return null;
			}
			if (Core.IsInitialized())
			{
				return new Request<ProductList>(CAPI.ovr_HTTP_GetWithMessageType(list.NextUrl, 467225263));
			}
			return null;
		}

		// Token: 0x0600379F RID: 14239 RVA: 0x0010E4CA File Offset: 0x0010C8CA
		public static Request<PurchaseList> GetNextPurchaseListPage(PurchaseList list)
		{
			if (!list.HasNextPage)
			{
				Debug.LogWarning("Oculus.Platform.GetNextPurchaseListPage: List has no next page");
				return null;
			}
			if (Core.IsInitialized())
			{
				return new Request<PurchaseList>(CAPI.ovr_HTTP_GetWithMessageType(list.NextUrl, 1196886677));
			}
			return null;
		}
	}
}
