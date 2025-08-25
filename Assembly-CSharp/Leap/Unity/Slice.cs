using System;
using System.Collections.Generic;
using Leap.Unity.Query;

namespace Leap.Unity
{
	// Token: 0x020006C8 RID: 1736
	public struct Slice<T> : IIndexableStruct<T, Slice<T>>
	{
		// Token: 0x060029D2 RID: 10706 RVA: 0x000E253E File Offset: 0x000E093E
		public Slice(IList<T> list, int beginIdx = 0, int endIdx = -1)
		{
			this._list = list;
			this._beginIdx = beginIdx;
			if (endIdx == -1)
			{
				endIdx = this._list.Count;
			}
			this._endIdx = endIdx;
			this._direction = ((beginIdx > endIdx) ? -1 : 1);
		}

		// Token: 0x1700053A RID: 1338
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
			set
			{
				if (index < 0 || index > this.Count - 1)
				{
					throw new IndexOutOfRangeException();
				}
				this._list[this._beginIdx + index * this._direction] = value;
			}
		}

		// Token: 0x1700053B RID: 1339
		// (get) Token: 0x060029D5 RID: 10709 RVA: 0x000E25EC File Offset: 0x000E09EC
		public int Count
		{
			get
			{
				return (this._endIdx - this._beginIdx) * this._direction;
			}
		}

		// Token: 0x060029D6 RID: 10710 RVA: 0x000E2602 File Offset: 0x000E0A02
		public IndexableStructEnumerator<T, Slice<T>> GetEnumerator()
		{
			return new IndexableStructEnumerator<T, Slice<T>>(this);
		}

		// Token: 0x060029D7 RID: 10711 RVA: 0x000E2610 File Offset: 0x000E0A10
		public Query<T> Query()
		{
			T[] array = ArrayPool<T>.Spawn(this.Count);
			for (int i = 0; i < this.Count; i++)
			{
				array[i] = this[i];
			}
			return new Query<T>(array, this.Count);
		}

		// Token: 0x04002205 RID: 8709
		private IList<T> _list;

		// Token: 0x04002206 RID: 8710
		private int _beginIdx;

		// Token: 0x04002207 RID: 8711
		private int _endIdx;

		// Token: 0x04002208 RID: 8712
		private int _direction;
	}
}
