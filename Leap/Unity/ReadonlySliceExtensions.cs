using System;

namespace Leap.Unity
{
	// Token: 0x020006BB RID: 1723
	public static class ReadonlySliceExtensions
	{
		// Token: 0x0600298F RID: 10639 RVA: 0x000E1A84 File Offset: 0x000DFE84
		public static ReadonlySlice<T> ReadonlySlice<T>(this ReadonlyList<T> list, int beginIdx = -1, int endIdx = -1)
		{
			if (beginIdx == -1 && endIdx == -1)
			{
				return new ReadonlySlice<T>(list, 0, list.Count);
			}
			if (beginIdx == -1 && endIdx != -1)
			{
				return new ReadonlySlice<T>(list, 0, endIdx);
			}
			if (endIdx == -1 && beginIdx != -1)
			{
				return new ReadonlySlice<T>(list, beginIdx, list.Count);
			}
			return new ReadonlySlice<T>(list, beginIdx, endIdx);
		}

		// Token: 0x06002990 RID: 10640 RVA: 0x000E1AEA File Offset: 0x000DFEEA
		public static ReadonlySlice<T> FromIndex<T>(this ReadonlyList<T> list, int fromIdx)
		{
			return list.ReadonlySlice(fromIdx, -1);
		}
	}
}
