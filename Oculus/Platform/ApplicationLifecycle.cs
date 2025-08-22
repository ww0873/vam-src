using System;
using Oculus.Platform.Models;

namespace Oculus.Platform
{
	// Token: 0x0200087E RID: 2174
	public static class ApplicationLifecycle
	{
		// Token: 0x06003733 RID: 14131 RVA: 0x0010D650 File Offset: 0x0010BA50
		public static LaunchDetails GetLaunchDetails()
		{
			return new LaunchDetails(CAPI.ovr_ApplicationLifecycle_GetLaunchDetails());
		}
	}
}
