using System;
using System.Runtime.InteropServices;

namespace LeapInternal
{
	// Token: 0x02000616 RID: 1558
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LEAP_CONFIG_RESPONSE_EVENT
	{
		// Token: 0x0400209D RID: 8349
		public uint requestId;

		// Token: 0x0400209E RID: 8350
		public LEAP_VARIANT_VALUE_TYPE value;
	}
}
