using System;

namespace Leap.Unity
{
	// Token: 0x02000727 RID: 1831
	public static class Enum<T>
	{
		// Token: 0x06002CA5 RID: 11429 RVA: 0x000EF8E5 File Offset: 0x000EDCE5
		static Enum()
		{
		}

		// Token: 0x0400239E RID: 9118
		public static readonly string[] names = Enum.GetNames(typeof(T));

		// Token: 0x0400239F RID: 9119
		public static readonly T[] values = (T[])Enum.GetValues(typeof(T));
	}
}
