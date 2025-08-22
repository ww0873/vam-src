using System;
using System.Runtime.InteropServices;

namespace LeapInternal
{
	// Token: 0x020005FF RID: 1535
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LEAP_DROPPED_FRAME_EVENT
	{
		// Token: 0x04002035 RID: 8245
		public long frame_id;

		// Token: 0x04002036 RID: 8246
		public eLeapDroppedFrameType reason;
	}
}
