using System;
using System.Collections.Generic;

namespace Leap.Unity.Query
{
	// Token: 0x0200070F RID: 1807
	public static class Values
	{
		// Token: 0x06002C07 RID: 11271 RVA: 0x000ECC58 File Offset: 0x000EB058
		public static Query<T> Single<T>(T value)
		{
			T[] array = ArrayPool<T>.Spawn(1);
			array[0] = value;
			return new Query<T>(array, 1);
		}

		// Token: 0x06002C08 RID: 11272 RVA: 0x000ECC7C File Offset: 0x000EB07C
		public static Query<T> Repeat<T>(T value, int times)
		{
			T[] array = ArrayPool<T>.Spawn(times);
			for (int i = 0; i < times; i++)
			{
				array[i] = value;
			}
			return new Query<T>(array, times);
		}

		// Token: 0x06002C09 RID: 11273 RVA: 0x000ECCB4 File Offset: 0x000EB0B4
		public static Query<T> Empty<T>()
		{
			T[] array = ArrayPool<T>.Spawn(0);
			return new Query<T>(array, 0);
		}

		// Token: 0x06002C0A RID: 11274 RVA: 0x000ECCD0 File Offset: 0x000EB0D0
		public static Query<int> Range(int from, int to, int step = 1, bool endIsExclusive = true)
		{
			if (step <= 0)
			{
				throw new ArgumentException("Step must be positive and non-zero.");
			}
			List<int> list = Pool<List<int>>.Spawn();
			Query<int> result;
			try
			{
				int num = from;
				int num2 = Utils.Sign(to - from);
				if (num2 != 0)
				{
					while (Utils.Sign(to - num) == num2)
					{
						list.Add(num);
						num += step * num2;
					}
				}
				if (!endIsExclusive && num == to)
				{
					list.Add(to);
				}
				result = new Query<int>(list);
			}
			finally
			{
				list.Clear();
				Pool<List<int>>.Recycle(list);
			}
			return result;
		}
	}
}
