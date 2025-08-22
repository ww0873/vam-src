using System;

namespace Leap.Unity
{
	// Token: 0x0200068F RID: 1679
	public abstract class DeltaBuffer<SampleType, DerivativeType> : IIndexable<SampleType>
	{
		// Token: 0x06002874 RID: 10356 RVA: 0x000DEC67 File Offset: 0x000DD067
		public DeltaBuffer(int bufferSize)
		{
			this._buffer = new RingBuffer<DeltaBuffer<SampleType, DerivativeType>.ValueTimePair>(bufferSize);
		}

		// Token: 0x17000506 RID: 1286
		// (get) Token: 0x06002875 RID: 10357 RVA: 0x000DEC7B File Offset: 0x000DD07B
		public int Count
		{
			get
			{
				return this._buffer.Count;
			}
		}

		// Token: 0x17000507 RID: 1287
		// (get) Token: 0x06002876 RID: 10358 RVA: 0x000DEC88 File Offset: 0x000DD088
		public bool IsFull
		{
			get
			{
				return this._buffer.IsFull;
			}
		}

		// Token: 0x17000508 RID: 1288
		// (get) Token: 0x06002877 RID: 10359 RVA: 0x000DEC95 File Offset: 0x000DD095
		public bool IsEmpty
		{
			get
			{
				return this._buffer.IsEmpty;
			}
		}

		// Token: 0x17000509 RID: 1289
		// (get) Token: 0x06002878 RID: 10360 RVA: 0x000DECA2 File Offset: 0x000DD0A2
		public int Capacity
		{
			get
			{
				return this._buffer.Capacity;
			}
		}

		// Token: 0x1700050A RID: 1290
		public SampleType this[int idx]
		{
			get
			{
				return this._buffer[idx].value;
			}
		}

		// Token: 0x0600287A RID: 10362 RVA: 0x000DECD1 File Offset: 0x000DD0D1
		public void Clear()
		{
			this._buffer.Clear();
		}

		// Token: 0x0600287B RID: 10363 RVA: 0x000DECE0 File Offset: 0x000DD0E0
		public void Add(SampleType sample, float sampleTime)
		{
			if (!this.IsEmpty && sampleTime == this.GetLatestTime())
			{
				this.SetLatest(sample, sampleTime);
				return;
			}
			this._buffer.Add(new DeltaBuffer<SampleType, DerivativeType>.ValueTimePair
			{
				value = sample,
				time = sampleTime
			});
		}

		// Token: 0x0600287C RID: 10364 RVA: 0x000DED34 File Offset: 0x000DD134
		public SampleType Get(int idx)
		{
			return this._buffer.Get(idx).value;
		}

		// Token: 0x0600287D RID: 10365 RVA: 0x000DED55 File Offset: 0x000DD155
		public SampleType GetLatest()
		{
			return this.Get(this.Count - 1);
		}

		// Token: 0x0600287E RID: 10366 RVA: 0x000DED68 File Offset: 0x000DD168
		public void Set(int idx, SampleType sample, float sampleTime)
		{
			this._buffer.Set(idx, new DeltaBuffer<SampleType, DerivativeType>.ValueTimePair
			{
				value = sample,
				time = sampleTime
			});
		}

		// Token: 0x0600287F RID: 10367 RVA: 0x000DED9A File Offset: 0x000DD19A
		public void SetLatest(SampleType sample, float sampleTime)
		{
			if (this.Count == 0)
			{
				this.Set(0, sample, sampleTime);
			}
			else
			{
				this.Set(this.Count - 1, sample, sampleTime);
			}
		}

		// Token: 0x06002880 RID: 10368 RVA: 0x000DEDC8 File Offset: 0x000DD1C8
		public float GetTime(int idx)
		{
			return this._buffer.Get(idx).time;
		}

		// Token: 0x06002881 RID: 10369 RVA: 0x000DEDEC File Offset: 0x000DD1EC
		public float GetLatestTime()
		{
			return this._buffer.Get(this.Count - 1).time;
		}

		// Token: 0x06002882 RID: 10370
		public abstract DerivativeType Delta();

		// Token: 0x06002883 RID: 10371 RVA: 0x000DEE14 File Offset: 0x000DD214
		public IndexableEnumerator<SampleType> GetEnumerator()
		{
			return new IndexableEnumerator<SampleType>(this);
		}

		// Token: 0x040021C0 RID: 8640
		protected RingBuffer<DeltaBuffer<SampleType, DerivativeType>.ValueTimePair> _buffer;

		// Token: 0x02000690 RID: 1680
		protected struct ValueTimePair
		{
			// Token: 0x040021C1 RID: 8641
			public SampleType value;

			// Token: 0x040021C2 RID: 8642
			public float time;
		}
	}
}
