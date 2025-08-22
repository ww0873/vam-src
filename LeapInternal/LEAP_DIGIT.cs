using System;
using System.Runtime.InteropServices;

namespace LeapInternal
{
	// Token: 0x0200060E RID: 1550
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LEAP_DIGIT
	{
		// Token: 0x04002073 RID: 8307
		public int finger_id;

		// Token: 0x04002074 RID: 8308
		public LEAP_BONE metacarpal;

		// Token: 0x04002075 RID: 8309
		public LEAP_BONE proximal;

		// Token: 0x04002076 RID: 8310
		public LEAP_BONE intermediate;

		// Token: 0x04002077 RID: 8311
		public LEAP_BONE distal;

		// Token: 0x04002078 RID: 8312
		public int is_extended;
	}
}
