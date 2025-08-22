using System;
using System.Collections.Generic;

// Token: 0x02000340 RID: 832
public static class IEnumerableExtension
{
	// Token: 0x06001428 RID: 5160 RVA: 0x00074908 File Offset: 0x00072D08
	public static List<T> ToList<T>(this IEnumerable<T> collection)
	{
		List<T> list = new List<T>();
		foreach (T item in collection)
		{
			list.Add(item);
		}
		return list;
	}
}
