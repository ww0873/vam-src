using System;
using Battlehub.Utils;
using UnityEngine;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000203 RID: 515
	[ExecuteInEditMode]
	public class BundleResourceMap : MonoBehaviour
	{
		// Token: 0x06000A54 RID: 2644 RVA: 0x0003EEBF File Offset: 0x0003D2BF
		public BundleResourceMap()
		{
		}

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x06000A55 RID: 2645 RVA: 0x0003EEC7 File Offset: 0x0003D2C7
		public string Guid
		{
			get
			{
				return this.m_guid;
			}
		}

		// Token: 0x06000A56 RID: 2646 RVA: 0x0003EED0 File Offset: 0x0003D2D0
		private void Awake()
		{
			if (!Application.isPlaying && string.IsNullOrEmpty(this.m_guid))
			{
				this.m_guid = System.Guid.NewGuid().ToString();
			}
		}

		// Token: 0x04000B65 RID: 2917
		[ReadOnly]
		public string BundleName;

		// Token: 0x04000B66 RID: 2918
		[ReadOnly]
		public string VariantName;

		// Token: 0x04000B67 RID: 2919
		[SerializeField]
		[ReadOnly]
		private string m_guid;
	}
}
