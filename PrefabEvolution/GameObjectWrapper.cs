using System;
using UnityEngine;

namespace PrefabEvolution
{
	// Token: 0x02000408 RID: 1032
	public class GameObjectWrapper
	{
		// Token: 0x06001A3E RID: 6718 RVA: 0x00093211 File Offset: 0x00091611
		public GameObjectWrapper(GameObject target)
		{
			this.target = target;
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06001A3F RID: 6719 RVA: 0x00093220 File Offset: 0x00091620
		// (set) Token: 0x06001A40 RID: 6720 RVA: 0x0009322D File Offset: 0x0009162D
		public bool m_IsActive
		{
			get
			{
				return this.target.activeSelf;
			}
			set
			{
				this.target.SetActive(value);
			}
		}

		// Token: 0x04001536 RID: 5430
		public GameObject target;
	}
}
