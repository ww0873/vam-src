using System;
using UnityEngine;

namespace AssetBundles
{
	// Token: 0x020000A1 RID: 161
	public class Utility
	{
		// Token: 0x0600025B RID: 603 RVA: 0x00011414 File Offset: 0x0000F814
		public Utility()
		{
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0001141C File Offset: 0x0000F81C
		public static string GetPlatformName()
		{
			return Utility.GetPlatformForAssetBundles(Application.platform);
		}

		// Token: 0x0600025D RID: 605 RVA: 0x00011428 File Offset: 0x0000F828
		private static string GetPlatformForAssetBundles(RuntimePlatform platform)
		{
			if (platform == RuntimePlatform.OSXPlayer)
			{
				return "OSX";
			}
			if (platform == RuntimePlatform.WindowsPlayer)
			{
				return "StandaloneWindows64";
			}
			switch (platform)
			{
			case RuntimePlatform.IPhonePlayer:
				return "iOS";
			default:
				if (platform != RuntimePlatform.WebGLPlayer)
				{
					return null;
				}
				return "WebGL";
			case RuntimePlatform.Android:
				return "Android";
			}
		}

		// Token: 0x04000338 RID: 824
		public const string AssetBundlesOutputPath = "AssetBundles";
	}
}
