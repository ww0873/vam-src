using System;
using System.Collections.Generic;

namespace GPUTools.Common.Scripts.PL.Tools
{
	// Token: 0x020009BD RID: 2493
	public class GroupedData<T> where T : IGroupItem
	{
		// Token: 0x06003F06 RID: 16134 RVA: 0x0012E691 File Offset: 0x0012CA91
		public GroupedData()
		{
		}

		// Token: 0x06003F07 RID: 16135 RVA: 0x0012E6AF File Offset: 0x0012CAAF
		public void AddGroup(List<T> list)
		{
			this.Groups.Add(list);
		}

		// Token: 0x06003F08 RID: 16136 RVA: 0x0012E6C0 File Offset: 0x0012CAC0
		public void Add(T item)
		{
			for (int i = 0; i < this.Groups.Count; i++)
			{
				List<T> list = this.Groups[i];
				bool flag = false;
				for (int j = 0; j < list.Count; j++)
				{
					T t = list[j];
					if (item.HasConflict(t))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					list.Add(item);
					return;
				}
			}
			List<T> item2 = new List<T>
			{
				item
			};
			this.Groups.Add(item2);
		}

		// Token: 0x17000759 RID: 1881
		// (get) Token: 0x06003F09 RID: 16137 RVA: 0x0012E768 File Offset: 0x0012CB68
		public T[] Data
		{
			get
			{
				List<T> list = new List<T>();
				foreach (List<T> list2 in this.Groups)
				{
					this.GroupsData.Add(new GroupData(list.Count, list2.Count));
					list.AddRange(list2);
				}
				return list.ToArray();
			}
		}

		// Token: 0x06003F0A RID: 16138 RVA: 0x0012E7EC File Offset: 0x0012CBEC
		public void Dispose()
		{
		}

		// Token: 0x04002FEC RID: 12268
		public List<GroupData> GroupsData = new List<GroupData>();

		// Token: 0x04002FED RID: 12269
		public List<List<T>> Groups = new List<List<T>>();
	}
}
