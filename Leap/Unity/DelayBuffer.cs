using System;

namespace Leap.Unity
{
	// Token: 0x0200068E RID: 1678
	public class DelayBuffer<T>
	{
		// Token: 0x0600286C RID: 10348 RVA: 0x000DEBB6 File Offset: 0x000DCFB6
		public DelayBuffer(int bufferSize)
		{
			this._buffer = new RingBuffer<T>(bufferSize);
		}

		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x0600286D RID: 10349 RVA: 0x000DEBCA File Offset: 0x000DCFCA
		public RingBuffer<T> Buffer
		{
			get
			{
				return this._buffer;
			}
		}

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x0600286E RID: 10350 RVA: 0x000DEBD2 File Offset: 0x000DCFD2
		public int Count
		{
			get
			{
				return this._buffer.Count;
			}
		}

		// Token: 0x17000503 RID: 1283
		// (get) Token: 0x0600286F RID: 10351 RVA: 0x000DEBDF File Offset: 0x000DCFDF
		public bool IsFull
		{
			get
			{
				return this._buffer.IsFull;
			}
		}

		// Token: 0x17000504 RID: 1284
		// (get) Token: 0x06002870 RID: 10352 RVA: 0x000DEBEC File Offset: 0x000DCFEC
		public bool IsEmpty
		{
			get
			{
				return this._buffer.IsEmpty;
			}
		}

		// Token: 0x17000505 RID: 1285
		// (get) Token: 0x06002871 RID: 10353 RVA: 0x000DEBF9 File Offset: 0x000DCFF9
		public int Capacity
		{
			get
			{
				return this._buffer.Capacity;
			}
		}

		// Token: 0x06002872 RID: 10354 RVA: 0x000DEC06 File Offset: 0x000DD006
		public void Clear()
		{
			this._buffer.Clear();
		}

		// Token: 0x06002873 RID: 10355 RVA: 0x000DEC14 File Offset: 0x000DD014
		public bool Add(T t, out T delayedT)
		{
			bool result;
			if (this._buffer.IsFull)
			{
				result = true;
				delayedT = this._buffer.GetOldest();
			}
			else
			{
				result = false;
				delayedT = default(T);
			}
			this._buffer.Add(t);
			return result;
		}

		// Token: 0x040021BF RID: 8639
		private RingBuffer<T> _buffer;
	}
}
