using System;
using System.Runtime.InteropServices;

namespace LeapInternal
{
	// Token: 0x02000611 RID: 1553
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LEAP_TIP
	{
		// Token: 0x04002090 RID: 8336
		public LEAP_VECTOR position;

		// Token: 0x04002091 RID: 8337
		public float radius;
	}
}
