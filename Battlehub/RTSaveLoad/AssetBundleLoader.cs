using System;
using System.IO;
using UnityEngine;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000238 RID: 568
	public class AssetBundleLoader : IAssetBundleLoader
	{
		// Token: 0x06000BD4 RID: 3028 RVA: 0x0004A6E3 File Offset: 0x00048AE3
		public AssetBundleLoader()
		{
		}

		// Token: 0x06000BD5 RID: 3029 RVA: 0x0004A6EC File Offset: 0x00048AEC
		public void Load(string name, AssetBundleEventHandler callback)
		{
			if (!File.Exists(Application.streamingAssetsPath + "/" + name))
			{
				callback(name, null);
				return;
			}
			AssetBundle bundle = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/" + name);
			if (callback != null)
			{
				callback(name, bundle);
			}
		}
	}
}
