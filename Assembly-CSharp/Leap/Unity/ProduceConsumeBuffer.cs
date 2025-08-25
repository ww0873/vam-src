using System;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006B8 RID: 1720
	public class ProduceConsumeBuffer<T>
	{
		// Token: 0x06002978 RID: 10616 RVA: 0x000E17D4 File Offset: 0x000DFBD4
		public ProduceConsumeBuffer(int minCapacity)
		{
			if (minCapacity <= 0)
			{
				throw new ArgumentOutOfRangeException("The capacity of the ProduceConsumeBuffer must be positive and non-zero.");
			}
			int num = Mathf.ClosestPowerOfTwo(minCapacity);
			int num2;
			if (num == minCapacity)
			{
				num2 = minCapacity;
			}
			else if (num < minCapacity)
			{
				num2 = num * 2;
			}
			else
			{
				num2 = num;
			}
			this._buffer = new T[num2];
			this._bufferMask = (uint)(num2 - 1);
			this._head = 0U;
			this._tail = 0U;
		}

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x06002979 RID: 10617 RVA: 0x000E1843 File Offset: 0x000DFC43
		public int Capacity
		{
			get
			{
				return this._buffer.Length;
			}
		}

		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x0600297A RID: 10618 RVA: 0x000E1850 File Offset: 0x000DFC50
		public int Count
		{
			get
			{
				int num = (int)this._tail;
				int head = (int)this._head;
				if (num < head)
				{
					num += this.Capacity;
				}
				return num - head;
			}
		}

		// Token: 0x0600297B RID: 10619 RVA: 0x000E1880 File Offset: 0x000DFC80
		public bool TryEnqueue(ref T t)
		{
			uint num = this._tail + 1U & this._bufferMask;
			if (num == this._head)
			{
				return false;
			}
			this._buffer[(int)((UIntPtr)this._tail)] = t;
			this._tail = num;
			return true;
		}

		// Token: 0x0600297C RID: 10620 RVA: 0x000E18CB File Offset: 0x000DFCCB
		public bool TryEnqueue(T t)
		{
			return this.TryEnqueue(ref t);
		}

		// Token: 0x0600297D RID: 10621 RVA: 0x000E18D8 File Offset: 0x000DFCD8
		public bool TryPeek(out T t)
		{
			if (this.Count == 0)
			{
				t = default(T);
				return false;
			}
			t = this._buffer[(int)((UIntPtr)this._head)];
			return true;
		}

		// Token: 0x0600297E RID: 10622 RVA: 0x000E191C File Offset: 0x000DFD1C
		public bool TryDequeue(out T t)
		{
			if (this._tail == this._head)
			{
				t = default(T);
				return false;
			}
			t = this._buffer[(int)((UIntPtr)this._head)];
			this._head = (this._head + 1U & this._bufferMask);
			return true;
		}

		// Token: 0x0600297F RID: 10623 RVA: 0x000E1979 File Offset: 0x000DFD79
		public bool TryDequeue()
		{
			if (this._tail == this._head)
			{
				return false;
			}
			this._head = (this._head + 1U & this._bufferMask);
			return true;
		}

		// Token: 0x06002980 RID: 10624 RVA: 0x000E19A4 File Offset: 0x000DFDA4
		public bool TryDequeueAll(out T mostRecent)
		{
			if (!this.TryDequeue(out mostRecent))
			{
				return false;
			}
			T t;
			while (this.TryDequeue(out t))
			{
				mostRecent = t;
			}
			return true;
		}

		// Token: 0x040021F0 RID: 8688
		private T[] _buffer;

		// Token: 0x040021F1 RID: 8689
		private uint _bufferMask;

		// Token: 0x040021F2 RID: 8690
		private uint _head;

		// Token: 0x040021F3 RID: 8691
		private uint _tail;
	}
}
