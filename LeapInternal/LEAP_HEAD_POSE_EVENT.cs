using System;
using System.Runtime.InteropServices;

namespace LeapInternal
{
	// Token: 0x02000602 RID: 1538
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LEAP_HEAD_POSE_EVENT
	{
		// Token: 0x0400203F RID: 8255
		public long timestamp;

		// Token: 0x04002040 RID: 8256
		public LEAP_VECTOR head_position;

		// Token: 0x04002041 RID: 8257
		public LEAP_QUATERNION head_orientation;
	}
}
