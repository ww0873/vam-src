using System;
using System.Runtime.InteropServices;

namespace LeapInternal
{
	// Token: 0x020005FD RID: 1533
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LEAP_DEVICE_FAILURE_EVENT
	{
		// Token: 0x0400202E RID: 8238
		public eLeapDeviceStatus status;

		// Token: 0x0400202F RID: 8239
		public IntPtr hDevice;
	}
}
