using System;
using System.Collections.Generic;
using UnityEngine;

namespace GPUTools.Common.Scripts.Tools
{
	// Token: 0x020009BE RID: 2494
	public class CacheProvider<T> where T : Component
	{
		// Token: 0x06003F0B RID: 16139 RVA: 0x0012E7EE File Offset: 0x0012CBEE
		public CacheProvider(List<GameObject> providers)
		{
			this.providers = providers;
			this.items = this.GetItems();
		}

		// Token: 0x06003F0C RID: 16140 RVA: 0x0012E80C File Offset: 0x0012CC0C
		public List<T> GetItems()
		{
			List<T> list = new List<T>();
			foreach (GameObject gameObject in this.providers)
			{
				if (gameObject != null)
				{
					list.AddRange(gameObject.GetComponentsInChildren<T>().ToList<T>());
				}
			}
			return list;
		}

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x06003F0D RID: 16141 RVA: 0x0012E888 File Offset: 0x0012CC88
		public List<T> Items
		{
			get
			{
				if (this.items == null)
				{
					this.items = this.GetItems();
				}
				return this.items;
			}
		}

		// Token: 0x06003F0E RID: 16142 RVA: 0x0012E8A8 File Offset: 0x0012CCA8
		public static bool Verify(List<GameObject> list)
		{
			if (list.Count == 0)
			{
				return false;
			}
			foreach (GameObject x in list)
			{
				if (x == null)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04002FEE RID: 12270
		private readonly List<GameObject> providers;

		// Token: 0x04002FEF RID: 12271
		private List<T> items;
	}
}
