using System;
using System.Runtime.InteropServices;

namespace LeapInternal
{
	// Token: 0x02000607 RID: 1543
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LEAP_IMAGE_PROPERTIES
	{
		// Token: 0x04002053 RID: 8275
		public eLeapImageType type;

		// Token: 0x04002054 RID: 8276
		public eLeapImageFormat format;

		// Token: 0x04002055 RID: 8277
		public uint bpp;

		// Token: 0x04002056 RID: 8278
		public uint width;

		// Token: 0x04002057 RID: 8279
		public uint height;

		// Token: 0x04002058 RID: 8280
		public float x_scale;

		// Token: 0x04002059 RID: 8281
		public float y_scale;

		// Token: 0x0400205A RID: 8282
		public float x_offset;

		// Token: 0x0400205B RID: 8283
		public float y_offset;
	}
}
