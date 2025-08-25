using System;
using System.Runtime.InteropServices;

namespace LeapInternal
{
	// Token: 0x02000606 RID: 1542
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LEAP_FRAME_HEADER
	{
		// Token: 0x04002050 RID: 8272
		public IntPtr reserved;

		// Token: 0x04002051 RID: 8273
		public long frame_id;

		// Token: 0x04002052 RID: 8274
		public long timestamp;
	}
}
