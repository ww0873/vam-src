using System;
using System.Collections.Generic;

namespace GPUTools.Common.Scripts.Utils
{
	// Token: 0x020009DA RID: 2522
	public static class CollectionUtils
	{
		// Token: 0x06003F98 RID: 16280 RVA: 0x0012F914 File Offset: 0x0012DD14
		public static bool NullOrEmpty<T>(this List<T> list)
		{
			return list == null || list.Count == 0;
		}

		// Token: 0x06003F99 RID: 16281 RVA: 0x0012F928 File Offset: 0x0012DD28
		public static bool NullOrEmpty<T>(this T[] array)
		{
			return array == null || array.Length == 0;
		}

		// Token: 0x06003F9A RID: 16282 RVA: 0x0012F93C File Offset: 0x0012DD3C
		public static void SetValueForAll<T>(this T[] array, T value)
		{
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = value;
			}
		}
	}
}
