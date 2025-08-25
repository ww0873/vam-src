using System;
using System.Runtime.InteropServices;
using Oculus.Platform.Models;
using UnityEngine;

namespace Oculus.Platform
{
	// Token: 0x020008A3 RID: 2211
	public sealed class StandalonePlatform
	{
		// Token: 0x060037CF RID: 14287 RVA: 0x0010EB2C File Offset: 0x0010CF2C
		public StandalonePlatform()
		{
		}

		// Token: 0x060037D0 RID: 14288 RVA: 0x0010EB34 File Offset: 0x0010CF34
		public Request<PlatformInitialize> InitializeInEditor()
		{
			if (string.IsNullOrEmpty(PlatformSettings.AppID))
			{
				throw new UnityException("Update your App ID by selecting 'Oculus Platform' -> 'Edit Settings'");
			}
			string appID = PlatformSettings.AppID;
			if (string.IsNullOrEmpty(StandalonePlatformSettings.OculusPlatformTestUserEmail))
			{
				throw new UnityException("Update your standalone email address by selecting 'Oculus Platform' -> 'Edit Settings'");
			}
			if (string.IsNullOrEmpty(StandalonePlatformSettings.OculusPlatformTestUserPassword))
			{
				throw new UnityException("Update your standalone user password by selecting 'Oculus Platform' -> 'Edit Settings'");
			}
			CAPI.ovr_UnityResetTestPlatform();
			CAPI.ovr_UnityInitGlobals(IntPtr.Zero);
			CAPI.OculusInitParams oculusInitParams = default(CAPI.OculusInitParams);
			oculusInitParams.sType = 1;
			oculusInitParams.appId = ulong.Parse(appID);
			oculusInitParams.email = StandalonePlatformSettings.OculusPlatformTestUserEmail;
			oculusInitParams.password = StandalonePlatformSettings.OculusPlatformTestUserPassword;
			return new Request<PlatformInitialize>(CAPI.ovr_Platform_InitializeStandaloneOculus(ref oculusInitParams));
		}

		// Token: 0x020008A4 RID: 2212
		// (Invoke) Token: 0x060037D2 RID: 14290
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		public delegate void UnityLogDelegate(IntPtr tag, IntPtr msg);
	}
}
