using System;
using System.Runtime.InteropServices;

namespace LeapInternal
{
	// Token: 0x02000615 RID: 1557
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LEAP_VARIANT_REF_TYPE
	{
		// Token: 0x0400209B RID: 8347
		public eLeapValueType type;

		// Token: 0x0400209C RID: 8348
		public string stringValue;
	}
}
