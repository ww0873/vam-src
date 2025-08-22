using System;

namespace Leap.Unity
{
	// Token: 0x0200062D RID: 1581
	public class SlidingMax
	{
		// Token: 0x060026D9 RID: 9945 RVA: 0x000D9C58 File Offset: 0x000D8058
		public SlidingMax(int history)
		{
			this._history = history;
			this._count = 0;
		}

		// Token: 0x060026DA RID: 9946 RVA: 0x000D9C7C File Offset: 0x000D807C
		public void AddValue(float value)
		{
			while (this._buffer.Count != 0 && this._buffer.Front.value <= value)
			{
				this._buffer.PopFront();
			}
			this._buffer.PushFront(new SlidingMax.IndexValuePair(this._count, value));
			this._count++;
			while (this._buffer.Back.index < this._count - this._history)
			{
				this._buffer.PopBack();
			}
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x060026DB RID: 9947 RVA: 0x000D9D1C File Offset: 0x000D811C
		public float Max
		{
			get
			{
				return this._buffer.Back.value;
			}
		}

		// Token: 0x040020EA RID: 8426
		private int _history;

		// Token: 0x040020EB RID: 8427
		private int _count;

		// Token: 0x040020EC RID: 8428
		private Deque<SlidingMax.IndexValuePair> _buffer = new Deque<SlidingMax.IndexValuePair>(8);

		// Token: 0x0200062E RID: 1582
		private struct IndexValuePair
		{
			// Token: 0x060026DC RID: 9948 RVA: 0x000D9D3C File Offset: 0x000D813C
			public IndexValuePair(int index, float value)
			{
				this.index = index;
				this.value = value;
			}

			// Token: 0x040020ED RID: 8429
			public int index;

			// Token: 0x040020EE RID: 8430
			public float value;
		}
	}
}
