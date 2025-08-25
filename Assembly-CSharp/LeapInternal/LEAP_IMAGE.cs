using System;
using System.Runtime.InteropServices;

namespace LeapInternal
{
	// Token: 0x02000608 RID: 1544
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LEAP_IMAGE
	{
		// Token: 0x0400205C RID: 8284
		public LEAP_IMAGE_PROPERTIES properties;

		// Token: 0x0400205D RID: 8285
		public ulong matrix_version;

		// Token: 0x0400205E RID: 8286
		public IntPtr distortionMatrix;

		// Token: 0x0400205F RID: 8287
		public IntPtr data;

		// Token: 0x04002060 RID: 8288
		public uint offset;
	}
}
