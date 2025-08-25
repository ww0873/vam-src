using System;
using System.Runtime.InteropServices;

namespace LeapInternal
{
	// Token: 0x020005F6 RID: 1526
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LEAP_CONNECTION_CONFIG
	{
		// Token: 0x0400201F RID: 8223
		public uint size;

		// Token: 0x04002020 RID: 8224
		public uint flags;

		// Token: 0x04002021 RID: 8225
		public IntPtr server_namespace;
	}
}
