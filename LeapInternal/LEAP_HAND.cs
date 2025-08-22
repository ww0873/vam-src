using System;
using System.Runtime.InteropServices;

namespace LeapInternal
{
	// Token: 0x02000610 RID: 1552
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LEAP_HAND
	{
		// Token: 0x04002080 RID: 8320
		public uint id;

		// Token: 0x04002081 RID: 8321
		public uint flags;

		// Token: 0x04002082 RID: 8322
		public eLeapHandType type;

		// Token: 0x04002083 RID: 8323
		public float confidence;

		// Token: 0x04002084 RID: 8324
		public ulong visible_time;

		// Token: 0x04002085 RID: 8325
		public float pinch_distance;

		// Token: 0x04002086 RID: 8326
		public float grab_angle;

		// Token: 0x04002087 RID: 8327
		public float pinch_strength;

		// Token: 0x04002088 RID: 8328
		public float grab_strength;

		// Token: 0x04002089 RID: 8329
		public LEAP_PALM palm;

		// Token: 0x0400208A RID: 8330
		public LEAP_DIGIT thumb;

		// Token: 0x0400208B RID: 8331
		public LEAP_DIGIT index;

		// Token: 0x0400208C RID: 8332
		public LEAP_DIGIT middle;

		// Token: 0x0400208D RID: 8333
		public LEAP_DIGIT ring;

		// Token: 0x0400208E RID: 8334
		public LEAP_DIGIT pinky;

		// Token: 0x0400208F RID: 8335
		public LEAP_BONE arm;
	}
}
