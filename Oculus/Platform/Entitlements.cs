using System;

namespace Oculus.Platform
{
	// Token: 0x0200088C RID: 2188
	public static class Entitlements
	{
		// Token: 0x06003799 RID: 14233 RVA: 0x0010E3F1 File Offset: 0x0010C7F1
		public static Request IsUserEntitledToApplication()
		{
			if (Core.IsInitialized())
			{
				return new Request(CAPI.ovr_Entitlement_GetIsViewerEntitled());
			}
			return null;
		}
	}
}
