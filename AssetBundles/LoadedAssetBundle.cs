using System;
using UnityEngine;

namespace AssetBundles
{
	// Token: 0x0200009D RID: 157
	public class LoadedAssetBundle
	{
		// Token: 0x0600023C RID: 572 RVA: 0x00010681 File Offset: 0x0000EA81
		public LoadedAssetBundle(AssetBundle assetBundle)
		{
			this.m_AssetBundle = assetBundle;
			this.m_ReferencedCount = 1;
		}

		// Token: 0x04000323 RID: 803
		public AssetBundle m_AssetBundle;

		// Token: 0x04000324 RID: 804
		public int m_ReferencedCount;
	}
}
