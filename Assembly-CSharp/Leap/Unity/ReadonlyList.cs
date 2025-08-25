using System;
using System.Collections.Generic;

namespace Leap.Unity
{
	// Token: 0x020006BA RID: 1722
	public struct ReadonlyList<T>
	{
		// Token: 0x06002988 RID: 10632 RVA: 0x000E1A2C File Offset: 0x000DFE2C
		public ReadonlyList(List<T> list)
		{
			this._list = list;
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x06002989 RID: 10633 RVA: 0x000E1A35 File Offset: 0x000DFE35
		public bool isValid
		{
			get
			{
				return this._list != null;
			}
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x0600298A RID: 10634 RVA: 0x000E1A43 File Offset: 0x000DFE43
		public int Count
		{
			get
			{
				return this._list.Count;
			}
		}

		// Token: 0x1700052C RID: 1324
		public T this[int index]
		{
			get
			{
				return this._list[index];
			}
		}

		// Token: 0x0600298C RID: 10636 RVA: 0x000E1A5E File Offset: 0x000DFE5E
		public List<T>.Enumerator GetEnumerator()
		{
			return this._list.GetEnumerator();
		}

		// Token: 0x0600298D RID: 10637 RVA: 0x000E1A6B File Offset: 0x000DFE6B
		public static implicit operator ReadonlyList<T>(List<T> list)
		{
			return new ReadonlyList<T>(list);
		}

		// Token: 0x0600298E RID: 10638 RVA: 0x000E1A73 File Offset: 0x000DFE73
		public int IndexOf(T item)
		{
			return this._list.IndexOf(item);
		}

		// Token: 0x040021F5 RID: 8693
		private readonly List<T> _list;
	}
}
