using System;
using System.Collections;
using System.Collections.Generic;

namespace Leap.Unity
{
	// Token: 0x020006A5 RID: 1701
	public abstract class MultiTypedList<BaseType> : MultiTypedList, IList<BaseType>, ICollection<BaseType>, IEnumerable<BaseType>, IEnumerable
	{
		// Token: 0x06002907 RID: 10503 RVA: 0x000E0817 File Offset: 0x000DEC17
		protected MultiTypedList()
		{
		}

		// Token: 0x1700051D RID: 1309
		// (get) Token: 0x06002908 RID: 10504
		public abstract int Count { get; }

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x06002909 RID: 10505 RVA: 0x000E081F File Offset: 0x000DEC1F
		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700051F RID: 1311
		public abstract BaseType this[int index]
		{
			get;
			set;
		}

		// Token: 0x0600290C RID: 10508
		public abstract void Add(BaseType obj);

		// Token: 0x0600290D RID: 10509
		public abstract void Clear();

		// Token: 0x0600290E RID: 10510 RVA: 0x000E0824 File Offset: 0x000DEC24
		public bool Contains(BaseType item)
		{
			for (int i = 0; i < this.Count; i++)
			{
				BaseType baseType = this[i];
				if (baseType.Equals(item))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600290F RID: 10511 RVA: 0x000E086C File Offset: 0x000DEC6C
		public void CopyTo(BaseType[] array, int arrayIndex)
		{
			for (int i = 0; i < this.Count; i++)
			{
				array[i + arrayIndex] = this[i];
			}
		}

		// Token: 0x06002910 RID: 10512 RVA: 0x000E08A0 File Offset: 0x000DECA0
		public MultiTypedList<BaseType>.Enumerator GetEnumerator()
		{
			return new MultiTypedList<BaseType>.Enumerator(this);
		}

		// Token: 0x06002911 RID: 10513 RVA: 0x000E08A8 File Offset: 0x000DECA8
		public int IndexOf(BaseType item)
		{
			for (int i = 0; i < this.Count; i++)
			{
				BaseType baseType = this[i];
				if (baseType.Equals(item))
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06002912 RID: 10514
		public abstract void Insert(int index, BaseType item);

		// Token: 0x06002913 RID: 10515 RVA: 0x000E08F0 File Offset: 0x000DECF0
		public bool Remove(BaseType item)
		{
			int num = this.IndexOf(item);
			if (num >= 0)
			{
				this.RemoveAt(num);
				return true;
			}
			return false;
		}

		// Token: 0x06002914 RID: 10516
		public abstract void RemoveAt(int index);

		// Token: 0x06002915 RID: 10517 RVA: 0x000E0916 File Offset: 0x000DED16
		IEnumerator IEnumerable.GetEnumerator()
		{
			return new MultiTypedList<BaseType>.Enumerator(this);
		}

		// Token: 0x06002916 RID: 10518 RVA: 0x000E0923 File Offset: 0x000DED23
		IEnumerator<BaseType> IEnumerable<!0>.GetEnumerator()
		{
			return new MultiTypedList<BaseType>.Enumerator(this);
		}

		// Token: 0x020006A6 RID: 1702
		public struct Enumerator : IEnumerator<BaseType>, IEnumerator, IDisposable
		{
			// Token: 0x06002917 RID: 10519 RVA: 0x000E0930 File Offset: 0x000DED30
			public Enumerator(MultiTypedList<BaseType> list)
			{
				this._list = list;
				this._index = 0;
				this._current = default(BaseType);
			}

			// Token: 0x17000521 RID: 1313
			// (get) Token: 0x06002918 RID: 10520 RVA: 0x000E095A File Offset: 0x000DED5A
			public BaseType Current
			{
				get
				{
					return this._current;
				}
			}

			// Token: 0x17000520 RID: 1312
			// (get) Token: 0x06002919 RID: 10521 RVA: 0x000E0962 File Offset: 0x000DED62
			object IEnumerator.Current
			{
				get
				{
					throw new NotImplementedException();
				}
			}

			// Token: 0x0600291A RID: 10522 RVA: 0x000E096C File Offset: 0x000DED6C
			public void Dispose()
			{
				this._list = null;
				this._current = default(BaseType);
			}

			// Token: 0x0600291B RID: 10523 RVA: 0x000E0990 File Offset: 0x000DED90
			public bool MoveNext()
			{
				if (this._index >= this._list.Count)
				{
					return false;
				}
				this._current = this._list[this._index++];
				return true;
			}

			// Token: 0x0600291C RID: 10524 RVA: 0x000E09D8 File Offset: 0x000DEDD8
			public void Reset()
			{
				this._index = 0;
				this._current = default(BaseType);
			}

			// Token: 0x040021D9 RID: 8665
			private MultiTypedList<BaseType> _list;

			// Token: 0x040021DA RID: 8666
			private int _index;

			// Token: 0x040021DB RID: 8667
			private BaseType _current;
		}
	}
}
