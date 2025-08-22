using System;
using System.Collections.Generic;

namespace Leap.Unity
{
	// Token: 0x020006C7 RID: 1735
	public static class SliceExtensions
	{
		// Token: 0x060029D0 RID: 10704 RVA: 0x000E24D0 File Offset: 0x000E08D0
		public static Slice<T> Slice<T>(this IList<T> list, int beginIdx = -1, int endIdx = -1)
		{
			if (beginIdx == -1 && endIdx == -1)
			{
				return new Slice<T>(list, 0, list.Count);
			}
			if (beginIdx == -1 && endIdx != -1)
			{
				return new Slice<T>(list, 0, endIdx);
			}
			if (endIdx == -1 && beginIdx != -1)
			{
				return new Slice<T>(list, beginIdx, list.Count);
			}
			return new Slice<T>(list, beginIdx, endIdx);
		}

		// Token: 0x060029D1 RID: 10705 RVA: 0x000E2534 File Offset: 0x000E0934
		public static Slice<T> FromIndex<T>(this IList<T> list, int fromIdx)
		{
			return list.Slice(fromIdx, -1);
		}
	}
}
