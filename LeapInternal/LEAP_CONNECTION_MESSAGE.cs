using System;
using System.Runtime.InteropServices;

namespace LeapInternal
{
	// Token: 0x02000603 RID: 1539
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LEAP_CONNECTION_MESSAGE
	{
		// Token: 0x04002042 RID: 8258
		public uint size;

		// Token: 0x04002043 RID: 8259
		public eLeapEventType type;

		// Token: 0x04002044 RID: 8260
		public IntPtr eventStructPtr;
	}
}
