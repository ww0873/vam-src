using System;
using System.Runtime.InteropServices;

namespace LeapInternal
{
	// Token: 0x0200060C RID: 1548
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LEAP_MATRIX_3x3
	{
		// Token: 0x0400206C RID: 8300
		public LEAP_VECTOR m1;

		// Token: 0x0400206D RID: 8301
		public LEAP_VECTOR m2;

		// Token: 0x0400206E RID: 8302
		public LEAP_VECTOR m3;
	}
}
