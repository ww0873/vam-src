using System;
using System.Runtime.InteropServices;

namespace LeapInternal
{
	// Token: 0x020005FE RID: 1534
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LEAP_TRACKING_EVENT
	{
		// Token: 0x04002030 RID: 8240
		public LEAP_FRAME_HEADER info;

		// Token: 0x04002031 RID: 8241
		public long tracking_id;

		// Token: 0x04002032 RID: 8242
		public uint nHands;

		// Token: 0x04002033 RID: 8243
		public IntPtr pHands;

		// Token: 0x04002034 RID: 8244
		public float framerate;
	}
}
