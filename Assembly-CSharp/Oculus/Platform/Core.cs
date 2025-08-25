using System;
using Oculus.Platform.Models;
using UnityEngine;

namespace Oculus.Platform
{
	// Token: 0x0200087D RID: 2173
	public sealed class Core
	{
		// Token: 0x0600372B RID: 14123 RVA: 0x0010D40F File Offset: 0x0010B80F
		public Core()
		{
		}

		// Token: 0x0600372C RID: 14124 RVA: 0x0010D417 File Offset: 0x0010B817
		public static bool IsInitialized()
		{
			return Core.IsPlatformInitialized;
		}

		// Token: 0x0600372D RID: 14125 RVA: 0x0010D41E File Offset: 0x0010B81E
		internal static void ForceInitialized()
		{
			Core.IsPlatformInitialized = true;
		}

		// Token: 0x0600372E RID: 14126 RVA: 0x0010D428 File Offset: 0x0010B828
		private static string getAppID(string appId = null)
		{
			string appIDFromConfig = Core.GetAppIDFromConfig();
			if (string.IsNullOrEmpty(appId))
			{
				if (string.IsNullOrEmpty(appIDFromConfig))
				{
					throw new UnityException("Update your app id by selecting 'Oculus Platform' -> 'Edit Settings'");
				}
				appId = appIDFromConfig;
			}
			else if (!string.IsNullOrEmpty(appIDFromConfig))
			{
				Debug.LogWarningFormat("The 'Oculus App Id ({0})' field in 'Oculus Platform/Edit Settings' is clobbering appId ({1}) that you passed in to Platform.Core.Init.  You should only specify this in one place.  We recommend the menu location.", new object[]
				{
					appIDFromConfig,
					appId
				});
			}
			return appId;
		}

		// Token: 0x0600372F RID: 14127 RVA: 0x0010D488 File Offset: 0x0010B888
		public static Request<PlatformInitialize> AsyncInitialize(string appId = null)
		{
			appId = Core.getAppID(appId);
			Request<PlatformInitialize> request;
			if (Application.isEditor && PlatformSettings.UseStandalonePlatform)
			{
				StandalonePlatform standalonePlatform = new StandalonePlatform();
				request = standalonePlatform.InitializeInEditor();
			}
			else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
			{
				WindowsPlatform windowsPlatform = new WindowsPlatform();
				request = windowsPlatform.AsyncInitialize(appId);
			}
			else
			{
				if (Application.platform != RuntimePlatform.Android)
				{
					throw new NotImplementedException("Oculus platform is not implemented on this platform yet.");
				}
				AndroidPlatform androidPlatform = new AndroidPlatform();
				request = androidPlatform.AsyncInitialize(appId);
			}
			Core.IsPlatformInitialized = (request != null);
			if (!Core.IsPlatformInitialized)
			{
				throw new UnityException("Oculus Platform failed to initialize.");
			}
			if (Core.LogMessages)
			{
				Debug.LogWarning("Oculus.Platform.Core.LogMessages is set to true. This will cause extra heap allocations, and should not be used outside of testing and debugging.");
			}
			new GameObject("Oculus.Platform.CallbackRunner").AddComponent<CallbackRunner>();
			return request;
		}

		// Token: 0x06003730 RID: 14128 RVA: 0x0010D55C File Offset: 0x0010B95C
		public static void Initialize(string appId = null)
		{
			appId = Core.getAppID(appId);
			if (Application.isEditor && PlatformSettings.UseStandalonePlatform)
			{
				StandalonePlatform standalonePlatform = new StandalonePlatform();
				Core.IsPlatformInitialized = (standalonePlatform.InitializeInEditor() != null);
			}
			else if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
			{
				WindowsPlatform windowsPlatform = new WindowsPlatform();
				Core.IsPlatformInitialized = windowsPlatform.Initialize(appId);
			}
			else
			{
				if (Application.platform != RuntimePlatform.Android)
				{
					throw new NotImplementedException("Oculus platform is not implemented on this platform yet.");
				}
				AndroidPlatform androidPlatform = new AndroidPlatform();
				Core.IsPlatformInitialized = androidPlatform.Initialize(appId);
			}
			if (!Core.IsPlatformInitialized)
			{
				throw new UnityException("Oculus Platform failed to initialize.");
			}
			if (Core.LogMessages)
			{
				Debug.LogWarning("Oculus.Platform.Core.LogMessages is set to true. This will cause extra heap allocations, and should not be used outside of testing and debugging.");
			}
			new GameObject("Oculus.Platform.CallbackRunner").AddComponent<CallbackRunner>();
		}

		// Token: 0x06003731 RID: 14129 RVA: 0x0010D635 File Offset: 0x0010BA35
		private static string GetAppIDFromConfig()
		{
			if (Application.platform == RuntimePlatform.Android)
			{
				return PlatformSettings.MobileAppID;
			}
			return PlatformSettings.AppID;
		}

		// Token: 0x06003732 RID: 14130 RVA: 0x0010D64E File Offset: 0x0010BA4E
		// Note: this type is marked as 'beforefieldinit'.
		static Core()
		{
		}

		// Token: 0x0400289E RID: 10398
		private static bool IsPlatformInitialized;

		// Token: 0x0400289F RID: 10399
		public static bool LogMessages;
	}
}
