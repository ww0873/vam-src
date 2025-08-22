using System;
using System.Runtime.InteropServices;

namespace LeapInternal
{
	// Token: 0x02000601 RID: 1537
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LEAP_POINT_MAPPING
	{
		// Token: 0x0400203A RID: 8250
		public long frame_id;

		// Token: 0x0400203B RID: 8251
		public long timestamp;

		// Token: 0x0400203C RID: 8252
		public uint nPoints;

		// Token: 0x0400203D RID: 8253
		public IntPtr points;

		// Token: 0x0400203E RID: 8254
		public IntPtr ids;
	}
}
