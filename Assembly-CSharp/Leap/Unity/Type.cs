using System;

namespace Leap.Unity
{
	// Token: 0x020006CB RID: 1739
	public static class Type<T>
	{
		// Token: 0x060029DE RID: 10718 RVA: 0x000E2687 File Offset: 0x000E0A87
		static Type()
		{
		}

		// Token: 0x0400220A RID: 8714
		public static readonly bool isValueType = typeof(T).IsValueType;
	}
}
