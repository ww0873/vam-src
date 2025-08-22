using System;
using System.Runtime.InteropServices;

namespace LeapInternal
{
	// Token: 0x02000617 RID: 1559
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LEAP_CONFIG_RESPONSE_EVENT_WITH_REF_TYPE
	{
		// Token: 0x0400209F RID: 8351
		public uint requestId;

		// Token: 0x040020A0 RID: 8352
		public LEAP_VARIANT_REF_TYPE value;
	}
}
