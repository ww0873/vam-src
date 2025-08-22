using System;
using System.Runtime.InteropServices;

namespace LeapInternal
{
	// Token: 0x02000619 RID: 1561
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LEAP_TELEMETRY_DATA
	{
		// Token: 0x040020A3 RID: 8355
		public uint threadId;

		// Token: 0x040020A4 RID: 8356
		public ulong startTime;

		// Token: 0x040020A5 RID: 8357
		public ulong endTime;

		// Token: 0x040020A6 RID: 8358
		public uint zoneDepth;

		// Token: 0x040020A7 RID: 8359
		public string fileName;

		// Token: 0x040020A8 RID: 8360
		public uint lineNumber;

		// Token: 0x040020A9 RID: 8361
		public string zoneName;
	}
}
