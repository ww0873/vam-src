using System;
using System.Collections.Generic;

namespace Leap.Unity
{
	// Token: 0x0200073B RID: 1851
	public static class ListAndArrayExtensions
	{
		// Token: 0x06002D2F RID: 11567 RVA: 0x000F10F8 File Offset: 0x000EF4F8
		public static T[] Fill<T>(this T[] array, T value)
		{
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = value;
			}
			return array;
		}

		// Token: 0x06002D30 RID: 11568 RVA: 0x000F1124 File Offset: 0x000EF524
		public static T[] Fill<T>(this T[] array, Func<T> constructor)
		{
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = constructor();
			}
			return array;
		}

		// Token: 0x06002D31 RID: 11569 RVA: 0x000F1154 File Offset: 0x000EF554
		public static T[,] Fill<T>(this T[,] array, T value)
		{
			for (int i = 0; i < array.GetLength(0); i++)
			{
				for (int j = 0; j < array.GetLength(1); j++)
				{
					array[i, j] = value;
				}
			}
			return array;
		}

		// Token: 0x06002D32 RID: 11570 RVA: 0x000F119C File Offset: 0x000EF59C
		public static List<T> Fill<T>(this List<T> list, T value)
		{
			for (int i = 0; i < list.Count; i++)
			{
				list[i] = value;
			}
			return list;
		}

		// Token: 0x06002D33 RID: 11571 RVA: 0x000F11CC File Offset: 0x000EF5CC
		public static List<T> Fill<T>(this List<T> list, int count, T value)
		{
			list.Clear();
			for (int i = 0; i < count; i++)
			{
				list.Add(value);
			}
			return list;
		}

		// Token: 0x06002D34 RID: 11572 RVA: 0x000F11FC File Offset: 0x000EF5FC
		public static List<T> FillEach<T>(this List<T> list, Func<T> generator)
		{
			for (int i = 0; i < list.Count; i++)
			{
				list[i] = generator();
			}
			return list;
		}

		// Token: 0x06002D35 RID: 11573 RVA: 0x000F1230 File Offset: 0x000EF630
		public static List<T> FillEach<T>(this List<T> list, Func<int, T> generator)
		{
			for (int i = 0; i < list.Count; i++)
			{
				list[i] = generator(i);
			}
			return list;
		}

		// Token: 0x06002D36 RID: 11574 RVA: 0x000F1264 File Offset: 0x000EF664
		public static List<T> FillEach<T>(this List<T> list, int count, Func<T> generator)
		{
			list.Clear();
			for (int i = 0; i < count; i++)
			{
				list.Add(generator());
			}
			return list;
		}

		// Token: 0x06002D37 RID: 11575 RVA: 0x000F1298 File Offset: 0x000EF698
		public static List<T> FillEach<T>(this List<T> list, int count, Func<int, T> generator)
		{
			list.Clear();
			for (int i = 0; i < count; i++)
			{
				list.Add(generator(i));
			}
			return list;
		}

		// Token: 0x06002D38 RID: 11576 RVA: 0x000F12CC File Offset: 0x000EF6CC
		public static List<T> Append<T>(this List<T> list, int count, T value)
		{
			for (int i = 0; i < count; i++)
			{
				list.Add(value);
			}
			return list;
		}

		// Token: 0x06002D39 RID: 11577 RVA: 0x000F12F4 File Offset: 0x000EF6F4
		public static T RemoveLast<T>(this List<T> list)
		{
			T result = list[list.Count - 1];
			list.RemoveAt(list.Count - 1);
			return result;
		}

		// Token: 0x06002D3A RID: 11578 RVA: 0x000F1320 File Offset: 0x000EF720
		public static bool RemoveUnordered<T>(this List<T> list, T element)
		{
			for (int i = 0; i < list.Count; i++)
			{
				T t = list[i];
				if (t.Equals(element))
				{
					list[i] = list.RemoveLast<T>();
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002D3B RID: 11579 RVA: 0x000F1374 File Offset: 0x000EF774
		public static void RemoveAtUnordered<T>(this List<T> list, int index)
		{
			if (list.Count - 1 == index)
			{
				list.RemoveLast<T>();
			}
			else
			{
				list[index] = list.RemoveLast<T>();
			}
		}

		// Token: 0x06002D3C RID: 11580 RVA: 0x000F139D File Offset: 0x000EF79D
		public static void InsertUnordered<T>(this List<T> list, int index, T element)
		{
			list.Add(list[index]);
			list[index] = element;
		}

		// Token: 0x06002D3D RID: 11581 RVA: 0x000F13B4 File Offset: 0x000EF7B4
		public static void RemoveAtMany<T>(this List<T> list, List<int> sortedIndexes)
		{
			if (sortedIndexes.Count == 0)
			{
				return;
			}
			if (sortedIndexes.Count == 1)
			{
				list.RemoveAt(sortedIndexes[0]);
				return;
			}
			int num = sortedIndexes[0];
			int i = num;
			int num2 = 0;
			for (;;)
			{
				while (i == sortedIndexes[num2])
				{
					i++;
					num2++;
					if (num2 == sortedIndexes.Count)
					{
						goto Block_3;
					}
				}
				list[num++] = list[i++];
			}
			Block_3:
			while (i < list.Count)
			{
				list[num++] = list[i++];
			}
			list.RemoveRange(list.Count - num2, num2);
		}

		// Token: 0x06002D3E RID: 11582 RVA: 0x000F146C File Offset: 0x000EF86C
		public static void InsertMany<T>(this List<T> list, List<int> sortedIndexes, List<T> elements)
		{
			if (sortedIndexes.Count == 0)
			{
				return;
			}
			if (sortedIndexes.Count == 1)
			{
				list.Insert(sortedIndexes[0], elements[0]);
				return;
			}
			int num = list.Count - 1;
			for (int i = 0; i < sortedIndexes.Count; i++)
			{
				list.Add(default(T));
			}
			int num2 = list.Count - 1;
			int num3 = sortedIndexes.Count - 1;
			for (;;)
			{
				while (num2 == sortedIndexes[num3])
				{
					list[num2--] = elements[num3--];
					if (num3 == -1)
					{
						return;
					}
				}
				list[num2--] = list[num--];
			}
		}
	}
}
