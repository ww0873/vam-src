using System;
using System.Collections.Generic;
using Leap.Unity.Query;

namespace Leap.Unity
{
	// Token: 0x020006B9 RID: 1721
	public struct ReadonlyHashSet<T>
	{
		// Token: 0x06002981 RID: 10625 RVA: 0x000E19D9 File Offset: 0x000DFDD9
		public ReadonlyHashSet(HashSet<T> set)
		{
			this._set = set;
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x06002982 RID: 10626 RVA: 0x000E19E2 File Offset: 0x000DFDE2
		public int Count
		{
			get
			{
				return this._set.Count;
			}
		}

		// Token: 0x06002983 RID: 10627 RVA: 0x000E19EF File Offset: 0x000DFDEF
		public HashSet<T>.Enumerator GetEnumerator()
		{
			return this._set.GetEnumerator();
		}

		// Token: 0x06002984 RID: 10628 RVA: 0x000E19FC File Offset: 0x000DFDFC
		public bool Contains(T obj)
		{
			return this._set.Contains(obj);
		}

		// Token: 0x06002985 RID: 10629 RVA: 0x000E1A0A File Offset: 0x000DFE0A
		public Query<T> Query()
		{
			return this._set.Query<T>();
		}

		// Token: 0x06002986 RID: 10630 RVA: 0x000E1A17 File Offset: 0x000DFE17
		public static implicit operator ReadonlyHashSet<T>(HashSet<T> set)
		{
			return new ReadonlyHashSet<T>(set);
		}

		// Token: 0x06002987 RID: 10631 RVA: 0x000E1A1F File Offset: 0x000DFE1F
		public static implicit operator ReadonlyHashSet<T>(SerializableHashSet<T> set)
		{
			return set;
		}

		// Token: 0x040021F4 RID: 8692
		private readonly HashSet<T> _set;
	}
}
