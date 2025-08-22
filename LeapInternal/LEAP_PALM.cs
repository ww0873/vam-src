using System;
using System.Runtime.InteropServices;

namespace LeapInternal
{
	// Token: 0x0200060F RID: 1551
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LEAP_PALM
	{
		// Token: 0x04002079 RID: 8313
		public LEAP_VECTOR position;

		// Token: 0x0400207A RID: 8314
		public LEAP_VECTOR stabilized_position;

		// Token: 0x0400207B RID: 8315
		public LEAP_VECTOR velocity;

		// Token: 0x0400207C RID: 8316
		public LEAP_VECTOR normal;

		// Token: 0x0400207D RID: 8317
		public float width;

		// Token: 0x0400207E RID: 8318
		public LEAP_VECTOR direction;

		// Token: 0x0400207F RID: 8319
		public LEAP_QUATERNION orientation;
	}
}
