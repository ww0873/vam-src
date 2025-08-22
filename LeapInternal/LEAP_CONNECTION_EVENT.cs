using System;
using System.Runtime.InteropServices;

namespace LeapInternal
{
	// Token: 0x020005F8 RID: 1528
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LEAP_CONNECTION_EVENT
	{
		// Token: 0x04002024 RID: 8228
		public eLeapServiceDisposition flags;
	}
}
