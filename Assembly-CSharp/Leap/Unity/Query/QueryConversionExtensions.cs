using System;
using System.Collections.Generic;

namespace Leap.Unity.Query
{
	// Token: 0x02000703 RID: 1795
	public static class QueryConversionExtensions
	{
		// Token: 0x06002B90 RID: 11152 RVA: 0x000EAC47 File Offset: 0x000E9047
		public static Query<T> Query<T>(this ICollection<T> collection)
		{
			return new Query<T>(collection);
		}

		// Token: 0x06002B91 RID: 11153 RVA: 0x000EAC50 File Offset: 0x000E9050
		public static Query<T> Query<T>(this IEnumerable<T> enumerable)
		{
			List<T> list = Pool<List<T>>.Spawn();
			Query<T> result;
			try
			{
				list.AddRange(enumerable);
				result = new Query<T>(list);
			}
			finally
			{
				list.Clear();
				Pool<List<T>>.Recycle(list);
			}
			return result;
		}

		// Token: 0x06002B92 RID: 11154 RVA: 0x000EAC94 File Offset: 0x000E9094
		public static Query<T> Query<T>(this IEnumerator<T> enumerator)
		{
			List<T> list = Pool<List<T>>.Spawn();
			Query<T> result;
			try
			{
				while (enumerator.MoveNext())
				{
					T item = enumerator.Current;
					list.Add(item);
				}
				result = new Query<T>(list);
			}
			finally
			{
				list.Clear();
				Pool<List<T>>.Recycle(list);
			}
			return result;
		}

		// Token: 0x06002B93 RID: 11155 RVA: 0x000EACF0 File Offset: 0x000E90F0
		public static Query<T> Query<T>(this T[,] array)
		{
			T[] array2 = ArrayPool<T>.Spawn(array.GetLength(0) * array.GetLength(1));
			int num = 0;
			for (int i = 0; i < array.GetLength(0); i++)
			{
				for (int j = 0; j < array.GetLength(1); j++)
				{
					array2[num++] = array[i, j];
				}
			}
			return new Query<T>(array2, array.GetLength(0) * array.GetLength(1));
		}
	}
}
