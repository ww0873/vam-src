using System;
using System.Runtime.InteropServices;

namespace LeapInternal
{
	// Token: 0x020005FC RID: 1532
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LEAP_DEVICE_EVENT
	{
		// Token: 0x0400202B RID: 8235
		public uint flags;

		// Token: 0x0400202C RID: 8236
		public LEAP_DEVICE_REF device;

		// Token: 0x0400202D RID: 8237
		public eLeapDeviceStatus status;
	}
}
