using System;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000237 RID: 567
	public interface IAssetBundleLoader
	{
		// Token: 0x06000BD3 RID: 3027
		void Load(string name, AssetBundleEventHandler callback);
	}
}
