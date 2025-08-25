using System;
using UnityEngine;

namespace AssetBundles
{
	// Token: 0x0200009C RID: 156
	public class AssetBundleLoadManifestOperation : AssetBundleLoadAssetOperationFull
	{
		// Token: 0x0600023A RID: 570 RVA: 0x00010644 File Offset: 0x0000EA44
		public AssetBundleLoadManifestOperation(string bundleName, string assetName, Type type) : base(bundleName, assetName, type)
		{
		}

		// Token: 0x0600023B RID: 571 RVA: 0x0001064F File Offset: 0x0000EA4F
		public override bool Update()
		{
			base.Update();
			if (this.m_Request != null && this.m_Request.isDone)
			{
				AssetBundleManager.AssetBundleManifestObject = this.GetAsset<AssetBundleManifest>();
				return false;
			}
			return true;
		}
	}
}
