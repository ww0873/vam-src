using System;
using System.Runtime.InteropServices;

namespace LeapInternal
{
	// Token: 0x020005F7 RID: 1527
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LEAP_CONNECTION_INFO
	{
		// Token: 0x04002022 RID: 8226
		public uint size;

		// Token: 0x04002023 RID: 8227
		public eLeapConnectionStatus status;
	}
}
