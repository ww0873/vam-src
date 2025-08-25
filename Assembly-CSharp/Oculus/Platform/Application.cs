using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x02000888 RID: 2184
	public static class Application
	{
		// Token: 0x0600378A RID: 14218 RVA: 0x0010E238 File Offset: 0x0010C638
		public static Request<ApplicationVersion> GetVersion()
		{
			if (Core.IsInitialized())
			{
				return new Request<ApplicationVersion>(CAPI.ovr_Application_GetVersion());
			}
			return null;
		}

		// Token: 0x0600378B RID: 14219 RVA: 0x0010E250 File Offset: 0x0010C650
		public static Request<string> LaunchOtherApp(ulong appID, ApplicationOptions deeplink_options = null)
		{
			if (Core.IsInitialized())
			{
				return new Request<string>(CAPI.ovr_Application_LaunchOtherApp(appID, (IntPtr)deeplink_options));
			}
			return null;
		}
	}
}
