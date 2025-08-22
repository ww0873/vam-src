using System;
using System.Runtime.InteropServices;

namespace LeapInternal
{
	// Token: 0x020005F9 RID: 1529
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LEAP_DEVICE_REF
	{
		// Token: 0x04002025 RID: 8229
		public IntPtr handle;

		// Token: 0x04002026 RID: 8230
		public uint id;
	}
}
