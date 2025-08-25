using System;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x0200068D RID: 1677
	[Serializable]
	public class AssetFolder
	{
		// Token: 0x06002868 RID: 10344 RVA: 0x000DEB87 File Offset: 0x000DCF87
		public AssetFolder()
		{
		}

		// Token: 0x06002869 RID: 10345 RVA: 0x000DEB8F File Offset: 0x000DCF8F
		public AssetFolder(string path)
		{
			this.Path = path;
		}

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x0600286A RID: 10346 RVA: 0x000DEB9E File Offset: 0x000DCF9E
		// (set) Token: 0x0600286B RID: 10347 RVA: 0x000DEBAA File Offset: 0x000DCFAA
		public virtual string Path
		{
			get
			{
				throw new InvalidOperationException("Cannot access the Path of an Asset Folder in a build.");
			}
			set
			{
				throw new InvalidOperationException("Cannot set the Path of an Asset Folder in a build.");
			}
		}

		// Token: 0x040021BE RID: 8638
		[SerializeField]
		protected UnityEngine.Object _assetFolder;
	}
}
