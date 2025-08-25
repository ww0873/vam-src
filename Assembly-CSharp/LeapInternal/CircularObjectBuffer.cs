using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace LeapInternal
{
	// Token: 0x020005B5 RID: 1461
	public class CircularObjectBuffer<T> where T : new()
	{
		// Token: 0x0600249C RID: 9372 RVA: 0x000D3BA0 File Offset: 0x000D1FA0
		public CircularObjectBuffer(int capacity)
		{
			this.Capacity = capacity;
			this.array = new T[this.Capacity];
			this.emptyT = Activator.CreateInstance<T>();
			this.current = 0;
			this.Count = 0;
			this.IsEmpty = true;
		}

		// Token: 0x17000450 RID: 1104
		// (get) Token: 0x0600249D RID: 9373 RVA: 0x000D3BF6 File Offset: 0x000D1FF6
		// (set) Token: 0x0600249E RID: 9374 RVA: 0x000D3BFE File Offset: 0x000D1FFE
		public int Count
		{
			[CompilerGenerated]
			get
			{
				return this.<Count>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Count>k__BackingField = value;
			}
		}

		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x0600249F RID: 9375 RVA: 0x000D3C07 File Offset: 0x000D2007
		// (set) Token: 0x060024A0 RID: 9376 RVA: 0x000D3C0F File Offset: 0x000D200F
		public int Capacity
		{
			[CompilerGenerated]
			get
			{
				return this.<Capacity>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Capacity>k__BackingField = value;
			}
		}

		// Token: 0x17000452 RID: 1106
		// (get) Token: 0x060024A1 RID: 9377 RVA: 0x000D3C18 File Offset: 0x000D2018
		// (set) Token: 0x060024A2 RID: 9378 RVA: 0x000D3C20 File Offset: 0x000D2020
		public bool IsEmpty
		{
			[CompilerGenerated]
			get
			{
				return this.<IsEmpty>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<IsEmpty>k__BackingField = value;
			}
		}

		// Token: 0x060024A3 RID: 9379 RVA: 0x000D3C2C File Offset: 0x000D202C
		public virtual void Put(ref T item)
		{
			object obj = this.locker;
			lock (obj)
			{
				if (!this.IsEmpty)
				{
					this.current++;
					if (this.current >= this.Capacity)
					{
						this.current = 0;
					}
				}
				if (this.Count < this.Capacity)
				{
					this.Count++;
				}
				object obj2 = this.array;
				lock (obj2)
				{
					this.array[this.current] = item;
				}
				this.IsEmpty = false;
			}
		}

		// Token: 0x060024A4 RID: 9380 RVA: 0x000D3CF8 File Offset: 0x000D20F8
		public void Get(out T t, int index = 0)
		{
			object obj = this.locker;
			lock (obj)
			{
				if (this.IsEmpty || index > this.Count - 1 || index < 0)
				{
					t = this.emptyT;
				}
				else
				{
					int num = this.current - index;
					if (num < 0)
					{
						num += this.Capacity;
					}
					t = this.array[num];
				}
			}
		}

		// Token: 0x060024A5 RID: 9381 RVA: 0x000D3D8C File Offset: 0x000D218C
		public void Resize(int newCapacity)
		{
			object obj = this.locker;
			lock (obj)
			{
				if (newCapacity > this.Capacity)
				{
					T[] array = new T[newCapacity];
					int num = 0;
					for (int i = this.Count - 1; i >= 0; i--)
					{
						T t;
						this.Get(out t, i);
						array[num++] = t;
					}
					this.array = array;
					this.Capacity = newCapacity;
				}
			}
		}

		// Token: 0x04001ED1 RID: 7889
		private T[] array;

		// Token: 0x04001ED2 RID: 7890
		private T emptyT;

		// Token: 0x04001ED3 RID: 7891
		private int current;

		// Token: 0x04001ED4 RID: 7892
		private object locker = new object();

		// Token: 0x04001ED5 RID: 7893
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private int <Count>k__BackingField;

		// Token: 0x04001ED6 RID: 7894
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private int <Capacity>k__BackingField;

		// Token: 0x04001ED7 RID: 7895
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool <IsEmpty>k__BackingField;
	}
}
