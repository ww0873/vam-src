using System;
using System.Runtime.InteropServices;

namespace LeapInternal
{
	// Token: 0x02000600 RID: 1536
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LEAP_POINT_MAPPING_CHANGE_EVENT
	{
		// Token: 0x04002037 RID: 8247
		public long frame_id;

		// Token: 0x04002038 RID: 8248
		public long timestamp;

		// Token: 0x04002039 RID: 8249
		public uint nPoints;
	}
}
