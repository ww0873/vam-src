using System;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006A2 RID: 1698
	public class MinHeap<T> where T : IMinHeapNode, IComparable<T>
	{
		// Token: 0x060028F6 RID: 10486 RVA: 0x000E0387 File Offset: 0x000DE787
		public MinHeap()
		{
		}

		// Token: 0x1700051C RID: 1308
		// (get) Token: 0x060028F7 RID: 10487 RVA: 0x000E039B File Offset: 0x000DE79B
		public int Count
		{
			get
			{
				return this._count;
			}
		}

		// Token: 0x060028F8 RID: 10488 RVA: 0x000E03A3 File Offset: 0x000DE7A3
		public void Clear()
		{
			Array.Clear(this._array, 0, this._count);
			this._count = 0;
		}

		// Token: 0x060028F9 RID: 10489 RVA: 0x000E03C0 File Offset: 0x000DE7C0
		public void Insert(T element)
		{
			if (this._array.Length == this._count)
			{
				T[] array = new T[this._array.Length * 2];
				Array.Copy(this._array, array, this._array.Length);
				this._array = array;
			}
			element.heapIndex = this._count;
			this._count++;
			this.bubbleUp(element);
		}

		// Token: 0x060028FA RID: 10490 RVA: 0x000E0433 File Offset: 0x000DE833
		public void Remove(T element)
		{
			this.removeAt(element.heapIndex);
		}

		// Token: 0x060028FB RID: 10491 RVA: 0x000E0449 File Offset: 0x000DE849
		public T PeekMin()
		{
			if (this._count == 0)
			{
				throw new Exception("Cannot peek when there are zero elements!");
			}
			return this._array[0];
		}

		// Token: 0x060028FC RID: 10492 RVA: 0x000E046D File Offset: 0x000DE86D
		public T RemoveMin()
		{
			if (this._count == 0)
			{
				throw new Exception("Cannot Remove Min when there are zero elements!");
			}
			return this.removeAt(0);
		}

		// Token: 0x060028FD RID: 10493 RVA: 0x000E048C File Offset: 0x000DE88C
		private T removeAt(int index)
		{
			T result = this._array[index];
			this._count--;
			if (this._count == 0)
			{
				return result;
			}
			T t = this._array[this._count];
			t.heapIndex = index;
			int parentIndex = MinHeap<T>.getParentIndex(index);
			if (this.isValidIndex(parentIndex) && this._array[parentIndex].CompareTo(t) > 0)
			{
				this.bubbleUp(t);
			}
			else
			{
				this.bubbleDown(t);
			}
			return result;
		}

		// Token: 0x060028FE RID: 10494 RVA: 0x000E0528 File Offset: 0x000DE928
		private void bubbleUp(T element)
		{
			while (element.heapIndex != 0)
			{
				int parentIndex = MinHeap<T>.getParentIndex(element.heapIndex);
				T t = this._array[parentIndex];
				if (t.CompareTo(element) <= 0)
				{
					IL_96:
					this._array[element.heapIndex] = element;
					return;
				}
				t.heapIndex = element.heapIndex;
				this._array[element.heapIndex] = t;
				element.heapIndex = parentIndex;
			}
			goto IL_96;
		}

		// Token: 0x060028FF RID: 10495 RVA: 0x000E05E4 File Offset: 0x000DE9E4
		public bool Validate()
		{
			return this.validateHeapInternal("Validation ");
		}

		// Token: 0x06002900 RID: 10496 RVA: 0x000E05F4 File Offset: 0x000DE9F4
		private void bubbleDown(T element)
		{
			int num = element.heapIndex;
			for (;;)
			{
				int childLeftIndex = MinHeap<T>.getChildLeftIndex(num);
				int childRightIndex = MinHeap<T>.getChildRightIndex(num);
				T t = element;
				int num2 = num;
				if (!this.isValidIndex(childLeftIndex))
				{
					break;
				}
				T t2 = this._array[childLeftIndex];
				if (t2.CompareTo(t) < 0)
				{
					t = t2;
					num2 = childLeftIndex;
				}
				if (this.isValidIndex(childRightIndex))
				{
					T t3 = this._array[childRightIndex];
					if (t3.CompareTo(t) < 0)
					{
						t = t3;
						num2 = childRightIndex;
					}
				}
				if (num2 == num)
				{
					break;
				}
				t.heapIndex = num;
				this._array[num] = t;
				num = num2;
			}
			element.heapIndex = num;
			this._array[num] = element;
		}

		// Token: 0x06002901 RID: 10497 RVA: 0x000E06E0 File Offset: 0x000DEAE0
		private bool validateHeapInternal(string operation)
		{
			for (int i = 0; i < this._count; i++)
			{
				if (this._array[i].heapIndex != i)
				{
					Debug.LogError(string.Concat(new object[]
					{
						"Element ",
						i,
						" had an index of ",
						this._array[i].heapIndex,
						" instead, after ",
						operation
					}));
					return false;
				}
				if (i != 0)
				{
					T t = this._array[MinHeap<T>.getParentIndex(i)];
					if (t.CompareTo(this._array[i]) > 0)
					{
						Debug.LogError(string.Concat(new object[]
						{
							"Element ",
							i,
							" had an incorrect order after ",
							operation
						}));
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06002902 RID: 10498 RVA: 0x000E07E2 File Offset: 0x000DEBE2
		private static int getChildLeftIndex(int index)
		{
			return index * 2 + 1;
		}

		// Token: 0x06002903 RID: 10499 RVA: 0x000E07E9 File Offset: 0x000DEBE9
		private static int getChildRightIndex(int index)
		{
			return index * 2 + 2;
		}

		// Token: 0x06002904 RID: 10500 RVA: 0x000E07F0 File Offset: 0x000DEBF0
		private static int getParentIndex(int index)
		{
			return (index - 1) / 2;
		}

		// Token: 0x06002905 RID: 10501 RVA: 0x000E07F7 File Offset: 0x000DEBF7
		private bool isValidIndex(int index)
		{
			return index < this._count && index >= 0;
		}

		// Token: 0x040021D5 RID: 8661
		private T[] _array = new T[4];

		// Token: 0x040021D6 RID: 8662
		private int _count;
	}
}
