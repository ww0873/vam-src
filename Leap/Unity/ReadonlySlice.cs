using System;
using Leap.Unity.Query;

namespace Leap.Unity
{
	// Token: 0x020006BC RID: 1724
	public struct ReadonlySlice<T> : IIndexableStruct<T, ReadonlySlice<T>>
	{
		// Token: 0x06002991 RID: 10641 RVA: 0x000E1AF4 File Offset: 0x000DFEF4
		public ReadonlySlice(ReadonlyList<T> list, int beginIdx, int endIdx)
		{
			this._list = list;
			this._beginIdx = beginIdx;
			this._endIdx = endIdx;
			this._direction = ((beginIdx > endIdx) ? -1 : 1);
		}

		// Token: 0x1700052D RID: 1325
		public T this[int index]
		{
			get
			{
				if (index < 0 || index > this.Count - 1)
				{
					throw new IndexOutOfRangeException();
				}
				return this._list[this._beginIdx + index * this._direction];
			}
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x06002993 RID: 10643 RVA: 0x000E1B56 File Offset: 0x000DFF56
		public int Count
		{
			get
			{
				return (this._endIdx - this._beginIdx) * this._direction;
			}
		}

		// Token: 0x06002994 RID: 10644 RVA: 0x000E1B6C File Offset: 0x000DFF6C
		public IndexableStructEnumerator<T, ReadonlySlice<T>> GetEnumerator()
		{
			return new IndexableStructEnumerator<T, ReadonlySlice<T>>(this);
		}

		// Token: 0x06002995 RID: 10645 RVA: 0x000E1B7C File Offset: 0x000DFF7C
		public Query<T> Query()
		{
			T[] array = ArrayPool<T>.Spawn(this.Count);
			for (int i = 0; i < this.Count; i++)
			{
				array[i] = this[i];
			}
			return new Query<T>(array, this.Count);
		}

		// Token: 0x040021F6 RID: 8694
		private ReadonlyList<T> _list;

		// Token: 0x040021F7 RID: 8695
		private int _beginIdx;

		// Token: 0x040021F8 RID: 8696
		private int _endIdx;

		// Token: 0x040021F9 RID: 8697
		private int _direction;
	}
}
