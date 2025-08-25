using System;
using System.Runtime.InteropServices;

namespace LeapInternal
{
	// Token: 0x0200060D RID: 1549
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LEAP_BONE
	{
		// Token: 0x0400206F RID: 8303
		public LEAP_VECTOR prev_joint;

		// Token: 0x04002070 RID: 8304
		public LEAP_VECTOR next_joint;

		// Token: 0x04002071 RID: 8305
		public float width;

		// Token: 0x04002072 RID: 8306
		public LEAP_QUATERNION rotation;
	}
}
