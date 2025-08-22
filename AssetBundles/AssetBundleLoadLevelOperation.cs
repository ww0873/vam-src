using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AssetBundles
{
	// Token: 0x02000098 RID: 152
	public class AssetBundleLoadLevelOperation : AssetBundleLoadOperation
	{
		// Token: 0x0600022D RID: 557 RVA: 0x0001046B File Offset: 0x0000E86B
		public AssetBundleLoadLevelOperation(string assetbundleName, string levelName, bool isAdditive)
		{
			this.m_AssetBundleName = assetbundleName;
			this.m_LevelName = levelName;
			this.m_IsAdditive = isAdditive;
		}

		// Token: 0x0600022E RID: 558 RVA: 0x00010488 File Offset: 0x0000E888
		public override bool Update()
		{
			if (this.m_Request != null)
			{
				return false;
			}
			LoadedAssetBundle loadedAssetBundle = AssetBundleManager.GetLoadedAssetBundle(this.m_AssetBundleName, out this.m_DownloadingError);
			if (loadedAssetBundle != null)
			{
				if (this.m_IsAdditive)
				{
					this.m_Request = SceneManager.LoadSceneAsync(this.m_LevelName, LoadSceneMode.Additive);
				}
				else
				{
					this.m_Request = SceneManager.LoadSceneAsync(this.m_LevelName, LoadSceneMode.Single);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600022F RID: 559 RVA: 0x000104F1 File Offset: 0x0000E8F1
		public override bool IsDone()
		{
			if (this.m_Request == null && this.m_DownloadingError != null)
			{
				Debug.LogError(this.m_DownloadingError);
				return true;
			}
			return this.m_Request != null && this.m_Request.isDone;
		}

		// Token: 0x04000318 RID: 792
		protected string m_AssetBundleName;

		// Token: 0x04000319 RID: 793
		protected string m_LevelName;

		// Token: 0x0400031A RID: 794
		protected bool m_IsAdditive;

		// Token: 0x0400031B RID: 795
		protected string m_DownloadingError;

		// Token: 0x0400031C RID: 796
		protected AsyncOperation m_Request;
	}
}
