using System;
using System.Runtime.InteropServices;

namespace LeapInternal
{
	// Token: 0x02000609 RID: 1545
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LEAP_IMAGE_EVENT
	{
		// Token: 0x04002061 RID: 8289
		public LEAP_FRAME_HEADER info;

		// Token: 0x04002062 RID: 8290
		public LEAP_IMAGE leftImage;

		// Token: 0x04002063 RID: 8291
		public LEAP_IMAGE rightImage;

		// Token: 0x04002064 RID: 8292
		public IntPtr calib;
	}
}
