using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

// Token: 0x0200076C RID: 1900
public class FastList<T>
{
	// Token: 0x06003103 RID: 12547 RVA: 0x000FEE14 File Offset: 0x000FD214
	public FastList()
	{
	}

	// Token: 0x06003104 RID: 12548 RVA: 0x000FEE1C File Offset: 0x000FD21C
	public FastList(int size)
	{
		if (size > 0)
		{
			this.size = 0;
			this.array = new T[size];
		}
		else
		{
			this.size = 0;
		}
	}

	// Token: 0x170005E9 RID: 1513
	// (get) Token: 0x06003105 RID: 12549 RVA: 0x000FEE4A File Offset: 0x000FD24A
	// (set) Token: 0x06003106 RID: 12550 RVA: 0x000FEE52 File Offset: 0x000FD252
	public int Count
	{
		get
		{
			return this.size;
		}
		set
		{
		}
	}

	// Token: 0x170005EA RID: 1514
	public T this[int i]
	{
		get
		{
			return this.array[i];
		}
		set
		{
			this.array[i] = value;
		}
	}

	// Token: 0x06003109 RID: 12553 RVA: 0x000FEE74 File Offset: 0x000FD274
	public void Add(T item)
	{
		if (this.array == null || this.size == this.array.Length)
		{
			this.Allocate();
		}
		this.array[this.size] = item;
		this.size++;
	}

	// Token: 0x0600310A RID: 12554 RVA: 0x000FEEC8 File Offset: 0x000FD2C8
	public void AddUnique(T item)
	{
		if (this.array == null || this.size == this.array.Length)
		{
			this.Allocate();
		}
		if (!this.Contains(item))
		{
			this.array[this.size] = item;
			this.size++;
		}
	}

	// Token: 0x0600310B RID: 12555 RVA: 0x000FEF28 File Offset: 0x000FD328
	public void AddRange(IEnumerable<T> items)
	{
		foreach (T item in items)
		{
			this.Add(item);
		}
	}

	// Token: 0x0600310C RID: 12556 RVA: 0x000FEF7C File Offset: 0x000FD37C
	public void Insert(int index, T item)
	{
		if (this.array == null || this.size == this.array.Length)
		{
			this.Allocate();
		}
		if (index < this.size)
		{
			for (int i = this.size; i > index; i--)
			{
				this.array[i] = this.array[i - 1];
			}
			this.array[index] = item;
			this.size++;
		}
		else
		{
			this.Add(item);
		}
	}

	// Token: 0x0600310D RID: 12557 RVA: 0x000FF014 File Offset: 0x000FD414
	public bool Remove(T item)
	{
		if (this.array != null)
		{
			for (int i = 0; i < this.size; i++)
			{
				if (item.Equals(this.array[i]))
				{
					this.size--;
					for (int j = i; j < this.size; j++)
					{
						this.array[j] = this.array[j + 1];
					}
					this.array[this.size] = default(T);
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x0600310E RID: 12558 RVA: 0x000FF0C4 File Offset: 0x000FD4C4
	public void RemoveAt(int index)
	{
		if (this.array != null && this.size > 0 && index < this.size)
		{
			this.size--;
			for (int i = index; i < this.size; i++)
			{
				this.array[i] = this.array[i + 1];
			}
			this.array[this.size] = default(T);
		}
	}

	// Token: 0x0600310F RID: 12559 RVA: 0x000FF150 File Offset: 0x000FD550
	public bool RemoveFast(T item)
	{
		if (this.array != null)
		{
			for (int i = 0; i < this.size; i++)
			{
				if (item.Equals(this.array[i]))
				{
					if (i < this.size - 1)
					{
						T t = this.array[this.size - 1];
						this.array[this.size - 1] = default(T);
						this.array[i] = t;
					}
					else
					{
						this.array[i] = default(T);
					}
					this.size--;
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06003110 RID: 12560 RVA: 0x000FF218 File Offset: 0x000FD618
	public void RemoveAtFast(int index)
	{
		if (this.array != null && index < this.size && index >= 0)
		{
			if (index == this.size - 1)
			{
				this.array[index] = default(T);
			}
			else
			{
				T t = this.array[this.size - 1];
				this.array[index] = t;
				this.array[this.size - 1] = default(T);
			}
			this.size--;
		}
	}

	// Token: 0x06003111 RID: 12561 RVA: 0x000FF2B8 File Offset: 0x000FD6B8
	public bool Contains(T item)
	{
		if (this.array == null || this.size <= 0)
		{
			return false;
		}
		for (int i = 0; i < this.size; i++)
		{
			if (this.array[i].Equals(item))
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06003112 RID: 12562 RVA: 0x000FF31C File Offset: 0x000FD71C
	public int IndexOf(T item)
	{
		if (this.size <= 0 || this.array == null)
		{
			return -1;
		}
		for (int i = 0; i < this.size; i++)
		{
			if (item.Equals(this.array[i]))
			{
				return i;
			}
		}
		return -1;
	}

	// Token: 0x06003113 RID: 12563 RVA: 0x000FF380 File Offset: 0x000FD780
	public T Pop()
	{
		if (this.array != null && this.size > 0)
		{
			T result = this.array[this.size - 1];
			this.array[this.size - 1] = default(T);
			this.size--;
			return result;
		}
		return default(T);
	}

	// Token: 0x06003114 RID: 12564 RVA: 0x000FF3ED File Offset: 0x000FD7ED
	public T[] ToArray()
	{
		this.Trim();
		return this.array;
	}

	// Token: 0x06003115 RID: 12565 RVA: 0x000FF3FC File Offset: 0x000FD7FC
	public void Sort(FastList<T>.CompareFunc comparer)
	{
		int num = 0;
		int num2 = this.size - 1;
		bool flag = true;
		while (flag)
		{
			flag = false;
			for (int i = num; i < num2; i++)
			{
				if (comparer(this.array[i], this.array[i + 1]) > 0)
				{
					T t = this.array[i];
					this.array[i] = this.array[i + 1];
					this.array[i + 1] = t;
					flag = true;
				}
				else if (!flag)
				{
					num = ((i != 0) ? (i - 1) : 0);
				}
			}
		}
	}

	// Token: 0x06003116 RID: 12566 RVA: 0x000FF4B0 File Offset: 0x000FD8B0
	public void InsertionSort(FastList<T>.CompareFunc comparer)
	{
		for (int i = 1; i < this.size; i++)
		{
			T t = this.array[i];
			int num = i;
			while (num > 0 && comparer(this.array[num - 1], t) > 0)
			{
				this.array[num] = this.array[num - 1];
				num--;
			}
			this.array[num] = t;
		}
	}

	// Token: 0x06003117 RID: 12567 RVA: 0x000FF538 File Offset: 0x000FD938
	public IEnumerator<T> GetEnumerator()
	{
		if (this.array != null)
		{
			for (int i = 0; i < this.size; i++)
			{
				yield return this.array[i];
			}
		}
		yield break;
	}

	// Token: 0x06003118 RID: 12568 RVA: 0x000FF554 File Offset: 0x000FD954
	public T Find(Predicate<T> match)
	{
		if (match != null && this.array != null)
		{
			for (int i = 0; i < this.size; i++)
			{
				if (match(this.array[i]))
				{
					return this.array[i];
				}
			}
		}
		return default(T);
	}

	// Token: 0x06003119 RID: 12569 RVA: 0x000FF5B8 File Offset: 0x000FD9B8
	private void Allocate()
	{
		T[] array;
		if (this.array == null)
		{
			array = new T[32];
		}
		else
		{
			array = new T[Mathf.Max(this.array.Length << 1, 32)];
		}
		if (this.array != null && this.size > 0)
		{
			this.array.CopyTo(array, 0);
		}
		this.array = array;
	}

	// Token: 0x0600311A RID: 12570 RVA: 0x000FF620 File Offset: 0x000FDA20
	private void Trim()
	{
		if (this.size > 0)
		{
			T[] array = new T[this.size];
			for (int i = 0; i < this.size; i++)
			{
				array[i] = this.array[i];
			}
			this.array = array;
		}
		else
		{
			this.array = null;
		}
	}

	// Token: 0x0600311B RID: 12571 RVA: 0x000FF682 File Offset: 0x000FDA82
	public void Clear()
	{
		this.size = 0;
	}

	// Token: 0x0600311C RID: 12572 RVA: 0x000FF68B File Offset: 0x000FDA8B
	public void Release()
	{
		this.Clear();
		this.array = null;
	}

	// Token: 0x040024DF RID: 9439
	public T[] array;

	// Token: 0x040024E0 RID: 9440
	public int size;

	// Token: 0x0200076D RID: 1901
	// (Invoke) Token: 0x0600311E RID: 12574
	public delegate int CompareFunc(T left, T right);

	// Token: 0x02000FB9 RID: 4025
	[CompilerGenerated]
	private sealed class <GetEnumerator>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<!0>
	{
		// Token: 0x060074EC RID: 29932 RVA: 0x000FF69A File Offset: 0x000FDA9A
		[DebuggerHidden]
		public <GetEnumerator>c__Iterator0()
		{
		}

		// Token: 0x060074ED RID: 29933 RVA: 0x000FF6A4 File Offset: 0x000FDAA4
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				if (this.array == null)
				{
					goto IL_91;
				}
				i = 0;
				break;
			case 1U:
				i++;
				break;
			default:
				return false;
			}
			if (i < this.size)
			{
				this.$current = this.array[i];
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			}
			IL_91:
			this.$PC = -1;
			return false;
		}

		// Token: 0x1700113F RID: 4415
		// (get) Token: 0x060074EE RID: 29934 RVA: 0x000FF74C File Offset: 0x000FDB4C
		T IEnumerator<!0>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x17001140 RID: 4416
		// (get) Token: 0x060074EF RID: 29935 RVA: 0x000FF754 File Offset: 0x000FDB54
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x060074F0 RID: 29936 RVA: 0x000FF761 File Offset: 0x000FDB61
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x060074F1 RID: 29937 RVA: 0x000FF771 File Offset: 0x000FDB71
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04006903 RID: 26883
		internal int <i>__1;

		// Token: 0x04006904 RID: 26884
		internal FastList<T> $this;

		// Token: 0x04006905 RID: 26885
		internal T $current;

		// Token: 0x04006906 RID: 26886
		internal bool $disposing;

		// Token: 0x04006907 RID: 26887
		internal int $PC;
	}
}
