using System;
using System.Runtime.InteropServices;

namespace LeapInternal
{
	// Token: 0x020005FB RID: 1531
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LEAP_ALLOCATOR
	{
		// Token: 0x04002028 RID: 8232
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public Allocate allocate;

		// Token: 0x04002029 RID: 8233
		[MarshalAs(UnmanagedType.FunctionPtr)]
		public Deallocate deallocate;

		// Token: 0x0400202A RID: 8234
		public IntPtr state;
	}
}
