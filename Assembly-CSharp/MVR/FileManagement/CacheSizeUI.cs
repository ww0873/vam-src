using System;
using UnityEngine;
using UnityEngine.UI;

namespace MVR.FileManagement
{
	// Token: 0x02000BD7 RID: 3031
	public class CacheSizeUI : MonoBehaviour
	{
		// Token: 0x0600560B RID: 22027 RVA: 0x001F7538 File Offset: 0x001F5938
		public CacheSizeUI()
		{
		}

		// Token: 0x0600560C RID: 22028 RVA: 0x001F7540 File Offset: 0x001F5940
		protected void CacheSizeCallback(float f)
		{
			if (this.text != null)
			{
				string text = f.ToString("F2") + "GB";
				this.text.text = text;
			}
		}

		// Token: 0x0600560D RID: 22029 RVA: 0x001F7581 File Offset: 0x001F5981
		public void UpdateUsed()
		{
			if (this.cacheManager != null && this.text != null)
			{
				this.cacheManager.GetCacheSize(new CacheManager.CacheSizeCallback(this.CacheSizeCallback));
			}
		}

		// Token: 0x0600560E RID: 22030 RVA: 0x001F75BC File Offset: 0x001F59BC
		protected void OnEnable()
		{
			this.UpdateUsed();
		}

		// Token: 0x04004739 RID: 18233
		public CacheManager cacheManager;

		// Token: 0x0400473A RID: 18234
		public Text text;
	}
}
