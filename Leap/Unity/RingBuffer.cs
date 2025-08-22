using System;

namespace Leap.Unity
{
	// Token: 0x020006BD RID: 1725
	public class RingBuffer<T> : IIndexable<T>
	{
		// Token: 0x06002996 RID: 10646 RVA: 0x000E1BC6 File Offset: 0x000DFFC6
		public RingBuffer(int bufferSize)
		{
			bufferSize = Math.Max(1, bufferSize);
			this.arr = new T[bufferSize];
		}

		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x06002997 RID: 10647 RVA: 0x000E1BEC File Offset: 0x000DFFEC
		public int Count
		{
			get
			{
				if (this.lastIdx == -1)
				{
					return 0;
				}
				int num = (this.lastIdx + 1) % this.arr.Length;
				if (num <= this.firstIdx)
				{
					num += this.arr.Length;
				}
				return num - this.firstIdx;
			}
		}

		// Token: 0x17000530 RID: 1328
		// (get) Token: 0x06002998 RID: 10648 RVA: 0x000E1C38 File Offset: 0x000E0038
		public int Capacity
		{
			get
			{
				return this.arr.Length;
			}
		}

		// Token: 0x17000531 RID: 1329
		// (get) Token: 0x06002999 RID: 10649 RVA: 0x000E1C42 File Offset: 0x000E0042
		public bool IsFull
		{
			get
			{
				return this.lastIdx != -1 && (this.lastIdx + 1 + this.arr.Length) % this.arr.Length == this.firstIdx;
			}
		}

		// Token: 0x17000532 RID: 1330
		// (get) Token: 0x0600299A RID: 10650 RVA: 0x000E1C75 File Offset: 0x000E0075
		public bool IsEmpty
		{
			get
			{
				return this.lastIdx == -1;
			}
		}

		// Token: 0x17000533 RID: 1331
		public T this[int idx]
		{
			get
			{
				return this.Get(idx);
			}
			set
			{
				this.Set(idx, value);
			}
		}

		// Token: 0x0600299D RID: 10653 RVA: 0x000E1C93 File Offset: 0x000E0093
		public void Clear()
		{
			this.firstIdx = 0;
			this.lastIdx = -1;
		}

		// Token: 0x0600299E RID: 10654 RVA: 0x000E1CA4 File Offset: 0x000E00A4
		public void Add(T t)
		{
			if (this.IsFull)
			{
				this.firstIdx++;
				this.firstIdx %= this.arr.Length;
			}
			this.lastIdx++;
			this.lastIdx %= this.arr.Length;
			this.arr[this.lastIdx] = t;
		}

		// Token: 0x0600299F RID: 10655 RVA: 0x000E1D14 File Offset: 0x000E0114
		public T Get(int idx)
		{
			if (idx < 0 || idx > this.Count - 1)
			{
				throw new IndexOutOfRangeException();
			}
			return this.arr[(this.firstIdx + idx) % this.arr.Length];
		}

		// Token: 0x060029A0 RID: 10656 RVA: 0x000E1D4D File Offset: 0x000E014D
		public T GetLatest()
		{
			if (this.Count == 0)
			{
				throw new IndexOutOfRangeException("Can't get latest value in an empty RingBuffer.");
			}
			return this.Get(this.Count - 1);
		}

		// Token: 0x060029A1 RID: 10657 RVA: 0x000E1D73 File Offset: 0x000E0173
		public T GetOldest()
		{
			if (this.Count == 0)
			{
				throw new IndexOutOfRangeException("Can't get oldest value in an empty RingBuffer.");
			}
			return this.Get(0);
		}

		// Token: 0x060029A2 RID: 10658 RVA: 0x000E1D94 File Offset: 0x000E0194
		public void Set(int idx, T t)
		{
			if (idx < 0 || idx > this.Count - 1)
			{
				throw new IndexOutOfRangeException();
			}
			int num = (this.firstIdx + idx) % this.arr.Length;
			this.arr[num] = t;
		}

		// Token: 0x060029A3 RID: 10659 RVA: 0x000E1DDB File Offset: 0x000E01DB
		public void SetLatest(T t)
		{
			if (this.Count == 0)
			{
				throw new IndexOutOfRangeException("Can't set latest value in an empty RingBuffer.");
			}
			this.Set(this.Count - 1, t);
		}

		// Token: 0x040021FA RID: 8698
		private T[] arr;

		// Token: 0x040021FB RID: 8699
		private int firstIdx;

		// Token: 0x040021FC RID: 8700
		private int lastIdx = -1;
	}
}
