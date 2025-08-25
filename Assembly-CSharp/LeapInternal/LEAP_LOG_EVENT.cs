using System;
using System.Runtime.InteropServices;

namespace LeapInternal
{
	// Token: 0x02000612 RID: 1554
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LEAP_LOG_EVENT
	{
		// Token: 0x04002092 RID: 8338
		public eLeapLogSeverity severity;

		// Token: 0x04002093 RID: 8339
		public long timestamp;

		// Token: 0x04002094 RID: 8340
		public IntPtr message;
	}
}
