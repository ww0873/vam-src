using System;
using UnityEngine;

namespace AssetBundles
{
	// Token: 0x02000099 RID: 153
	public abstract class AssetBundleLoadAssetOperation : AssetBundleLoadOperation
	{
		// Token: 0x06000230 RID: 560 RVA: 0x0001052F File Offset: 0x0000E92F
		protected AssetBundleLoadAssetOperation()
		{
		}

		// Token: 0x06000231 RID: 561
		public abstract T GetAsset<T>() where T : UnityEngine.Object;
	}
}
