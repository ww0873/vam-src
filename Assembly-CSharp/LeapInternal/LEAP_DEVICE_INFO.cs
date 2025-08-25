using System;
using System.Runtime.InteropServices;

namespace LeapInternal
{
	// Token: 0x02000605 RID: 1541
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LEAP_DEVICE_INFO
	{
		// Token: 0x04002046 RID: 8262
		public uint size;

		// Token: 0x04002047 RID: 8263
		public eLeapDeviceStatus status;

		// Token: 0x04002048 RID: 8264
		public eLeapDeviceCaps caps;

		// Token: 0x04002049 RID: 8265
		public eLeapDeviceType type;

		// Token: 0x0400204A RID: 8266
		public uint baseline;

		// Token: 0x0400204B RID: 8267
		public uint serial_length;

		// Token: 0x0400204C RID: 8268
		public IntPtr serial;

		// Token: 0x0400204D RID: 8269
		public float h_fov;

		// Token: 0x0400204E RID: 8270
		public float v_fov;

		// Token: 0x0400204F RID: 8271
		public uint range;
	}
}
