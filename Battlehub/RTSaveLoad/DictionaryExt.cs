using System;
using System.Collections.Generic;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000205 RID: 517
	public static class DictionaryExt
	{
		// Token: 0x06000A5A RID: 2650 RVA: 0x0003EF44 File Offset: 0x0003D344
		public static U Get<T, U>(this Dictionary<T, U> dict, T key)
		{
			U result;
			if (dict.TryGetValue(key, out result))
			{
				return result;
			}
			return default(U);
		}
	}
}
