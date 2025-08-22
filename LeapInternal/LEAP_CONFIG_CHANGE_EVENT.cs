using System;
using System.Runtime.InteropServices;

namespace LeapInternal
{
	// Token: 0x02000618 RID: 1560
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LEAP_CONFIG_CHANGE_EVENT
	{
		// Token: 0x040020A1 RID: 8353
		public uint requestId;

		// Token: 0x040020A2 RID: 8354
		public bool status;
	}
}
