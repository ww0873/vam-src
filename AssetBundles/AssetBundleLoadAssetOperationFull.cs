using System;
using UnityEngine;

namespace AssetBundles
{
	// Token: 0x0200009B RID: 155
	public class AssetBundleLoadAssetOperationFull : AssetBundleLoadAssetOperation
	{
		// Token: 0x06000236 RID: 566 RVA: 0x0001055E File Offset: 0x0000E95E
		public AssetBundleLoadAssetOperationFull(string bundleName, string assetName, Type type)
		{
			this.m_AssetBundleName = bundleName;
			this.m_AssetName = assetName;
			this.m_Type = type;
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0001057B File Offset: 0x0000E97B
		public override T GetAsset<T>()
		{
			if (this.m_Request != null && this.m_Request.isDone)
			{
				return this.m_Request.asset as T;
			}
			return (T)((object)null);
		}

		// Token: 0x06000238 RID: 568 RVA: 0x000105B4 File Offset: 0x0000E9B4
		public override bool Update()
		{
			if (this.m_Request != null)
			{
				return false;
			}
			LoadedAssetBundle loadedAssetBundle = AssetBundleManager.GetLoadedAssetBundle(this.m_AssetBundleName, out this.m_DownloadingError);
			if (loadedAssetBundle != null)
			{
				this.m_Request = loadedAssetBundle.m_AssetBundle.LoadAssetAsync(this.m_AssetName, this.m_Type);
				return false;
			}
			return true;
		}

		// Token: 0x06000239 RID: 569 RVA: 0x00010606 File Offset: 0x0000EA06
		public override bool IsDone()
		{
			if (this.m_Request == null && this.m_DownloadingError != null)
			{
				Debug.LogError(this.m_DownloadingError);
				return true;
			}
			return this.m_Request != null && this.m_Request.isDone;
		}

		// Token: 0x0400031E RID: 798
		protected string m_AssetBundleName;

		// Token: 0x0400031F RID: 799
		protected string m_AssetName;

		// Token: 0x04000320 RID: 800
		protected string m_DownloadingError;

		// Token: 0x04000321 RID: 801
		protected Type m_Type;

		// Token: 0x04000322 RID: 802
		protected AssetBundleRequest m_Request;
	}
}
