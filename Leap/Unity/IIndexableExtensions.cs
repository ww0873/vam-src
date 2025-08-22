using System;
using Leap.Unity.Query;

namespace Leap.Unity
{
	// Token: 0x02000698 RID: 1688
	public static class IIndexableExtensions
	{
		// Token: 0x060028C3 RID: 10435 RVA: 0x000DFB1A File Offset: 0x000DDF1A
		public static IndexableEnumerator<T> GetEnumerator<T>(this IIndexable<T> indexable)
		{
			return new IndexableEnumerator<T>(indexable);
		}

		// Token: 0x060028C4 RID: 10436 RVA: 0x000DFB24 File Offset: 0x000DDF24
		public static Query<T> Query<T>(this IIndexable<T> indexable)
		{
			T[] array = ArrayPool<T>.Spawn(indexable.Count);
			for (int i = 0; i < indexable.Count; i++)
			{
				array[i] = indexable[i];
			}
			return new Query<T>(array, indexable.Count);
		}
	}
}
