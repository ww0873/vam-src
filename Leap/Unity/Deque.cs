using System;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x02000694 RID: 1684
	public class Deque<T>
	{
		// Token: 0x0600288A RID: 10378 RVA: 0x000DEFE0 File Offset: 0x000DD3E0
		public Deque(int minCapacity = 8)
		{
			if (minCapacity <= 0)
			{
				throw new ArgumentException("Capacity must be positive and nonzero.");
			}
			int num = Mathf.ClosestPowerOfTwo(minCapacity);
			if (num < minCapacity)
			{
				num *= 2;
			}
			this._array = new T[num];
			this.recalculateIndexMask();
			this._front = 0U;
			this._count = 0U;
		}

		// Token: 0x1700050B RID: 1291
		// (get) Token: 0x0600288B RID: 10379 RVA: 0x000DF037 File Offset: 0x000DD437
		public int Count
		{
			get
			{
				return (int)this._count;
			}
		}

		// Token: 0x0600288C RID: 10380 RVA: 0x000DF03F File Offset: 0x000DD43F
		public void Clear()
		{
			if (this._count != 0U)
			{
				Array.Clear(this._array, 0, this._array.Length);
				this._front = 0U;
				this._count = 0U;
			}
		}

		// Token: 0x0600288D RID: 10381 RVA: 0x000DF06E File Offset: 0x000DD46E
		public void PushBack(T t)
		{
			this.doubleCapacityIfFull();
			this._count += 1U;
			this._array[(int)((UIntPtr)this.getBackIndex())] = t;
		}

		// Token: 0x0600288E RID: 10382 RVA: 0x000DF097 File Offset: 0x000DD497
		public void PushFront(T t)
		{
			this.doubleCapacityIfFull();
			this._count += 1U;
			this._front = (this._front - 1U & this._indexMask);
			this._array[(int)((UIntPtr)this._front)] = t;
		}

		// Token: 0x0600288F RID: 10383 RVA: 0x000DF0D8 File Offset: 0x000DD4D8
		public void PopBack()
		{
			this.checkForEmpty("pop back");
			this._array[(int)((UIntPtr)this.getBackIndex())] = default(T);
			this._count -= 1U;
		}

		// Token: 0x06002890 RID: 10384 RVA: 0x000DF11C File Offset: 0x000DD51C
		public void PopFront()
		{
			this.checkForEmpty("pop front");
			this._array[(int)((UIntPtr)this._front)] = default(T);
			this._count -= 1U;
			this._front = (this._front + 1U & this._indexMask);
		}

		// Token: 0x06002891 RID: 10385 RVA: 0x000DF174 File Offset: 0x000DD574
		public void PopBack(out T back)
		{
			this.checkForEmpty("pop back");
			uint backIndex = this.getBackIndex();
			back = this._array[(int)((UIntPtr)backIndex)];
			this._array[(int)((UIntPtr)backIndex)] = default(T);
			this._count -= 1U;
		}

		// Token: 0x06002892 RID: 10386 RVA: 0x000DF1CC File Offset: 0x000DD5CC
		public void PopFront(out T front)
		{
			this.checkForEmpty("pop front");
			front = this._array[(int)((UIntPtr)this._front)];
			this._array[(int)((UIntPtr)this._front)] = default(T);
			this._front = (this._front + 1U & this._indexMask);
			this._count -= 1U;
		}

		// Token: 0x1700050C RID: 1292
		// (get) Token: 0x06002893 RID: 10387 RVA: 0x000DF23A File Offset: 0x000DD63A
		// (set) Token: 0x06002894 RID: 10388 RVA: 0x000DF259 File Offset: 0x000DD659
		public T Front
		{
			get
			{
				this.checkForEmpty("get front");
				return this._array[(int)((UIntPtr)this._front)];
			}
			set
			{
				this.checkForEmpty("set front");
				this._array[(int)((UIntPtr)this._front)] = value;
			}
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06002895 RID: 10389 RVA: 0x000DF279 File Offset: 0x000DD679
		// (set) Token: 0x06002896 RID: 10390 RVA: 0x000DF298 File Offset: 0x000DD698
		public T Back
		{
			get
			{
				this.checkForEmpty("get back");
				return this._array[(int)((UIntPtr)this.getBackIndex())];
			}
			set
			{
				this.checkForEmpty("set back");
				this._array[(int)((UIntPtr)this.getBackIndex())] = value;
			}
		}

		// Token: 0x1700050E RID: 1294
		public T this[int index]
		{
			get
			{
				this.checkForValidIndex((uint)index);
				return this._array[(int)((UIntPtr)this.getIndex((uint)index))];
			}
			set
			{
				this.checkForValidIndex((uint)index);
				this._array[(int)((UIntPtr)this.getIndex((uint)index))] = value;
			}
		}

		// Token: 0x06002899 RID: 10393 RVA: 0x000DF310 File Offset: 0x000DD710
		public string ToDebugString()
		{
			string str = "[";
			uint backIndex = this.getBackIndex();
			uint num = 0U;
			while ((ulong)num < (ulong)((long)this._array.Length))
			{
				bool flag;
				if (this._count == 0U)
				{
					flag = true;
				}
				else if (this._count == 1U)
				{
					flag = (num != this._front);
				}
				else if (this._front < backIndex)
				{
					flag = (num < this._front || num > backIndex);
				}
				else
				{
					flag = (num < this._front && num > backIndex);
				}
				string text = string.Empty;
				if (num == this._front)
				{
					text = "{";
				}
				else
				{
					text = " ";
				}
				if (flag)
				{
					text += ".";
				}
				else
				{
					text += this._array[(int)((UIntPtr)num)].ToString();
				}
				if (num == backIndex)
				{
					text += "}";
				}
				else
				{
					text += " ";
				}
				str += text;
				num += 1U;
			}
			return str + "]";
		}

		// Token: 0x0600289A RID: 10394 RVA: 0x000DF44E File Offset: 0x000DD84E
		private uint getBackIndex()
		{
			return this._front + this._count - 1U & this._indexMask;
		}

		// Token: 0x0600289B RID: 10395 RVA: 0x000DF466 File Offset: 0x000DD866
		private uint getIndex(uint index)
		{
			return this._front + index & this._indexMask;
		}

		// Token: 0x0600289C RID: 10396 RVA: 0x000DF478 File Offset: 0x000DD878
		private void doubleCapacityIfFull()
		{
			if ((ulong)this._count >= (ulong)((long)this._array.Length))
			{
				T[] array = new T[this._array.Length * 2];
				uint backIndex = this.getBackIndex();
				if (this._front <= backIndex)
				{
					Array.Copy(this._array, (long)((ulong)this._front), array, 0L, (long)((ulong)this._count));
				}
				else
				{
					uint num = (uint)(this._array.Length - (int)this._front);
					Array.Copy(this._array, (long)((ulong)this._front), array, 0L, (long)((ulong)num));
					Array.Copy(this._array, 0L, array, (long)((ulong)num), (long)((ulong)(this._count - num)));
				}
				this._front = 0U;
				this._array = array;
				this.recalculateIndexMask();
			}
		}

		// Token: 0x0600289D RID: 10397 RVA: 0x000DF532 File Offset: 0x000DD932
		private void recalculateIndexMask()
		{
			this._indexMask = (uint)(this._array.Length - 1);
		}

		// Token: 0x0600289E RID: 10398 RVA: 0x000DF544 File Offset: 0x000DD944
		private void checkForValidIndex(uint index)
		{
			if (index >= this._count)
			{
				throw new IndexOutOfRangeException(string.Concat(new object[]
				{
					"The index ",
					index,
					" was out of range for the RingBuffer with size ",
					this._count,
					"."
				}));
			}
		}

		// Token: 0x0600289F RID: 10399 RVA: 0x000DF59D File Offset: 0x000DD99D
		private void checkForEmpty(string actionName)
		{
			if (this._count == 0U)
			{
				throw new InvalidOperationException("Cannot " + actionName + " because the RingBuffer is empty.");
			}
		}

		// Token: 0x040021C3 RID: 8643
		private T[] _array;

		// Token: 0x040021C4 RID: 8644
		private uint _front;

		// Token: 0x040021C5 RID: 8645
		private uint _count;

		// Token: 0x040021C6 RID: 8646
		private uint _indexMask;
	}
}
