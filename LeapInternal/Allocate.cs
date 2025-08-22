using System;
using System.Runtime.InteropServices;

namespace LeapInternal
{
	// Token: 0x02000621 RID: 1569
	// (Invoke) Token: 0x0600269F RID: 9887
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate IntPtr Allocate(uint size, eLeapAllocatorType typeHint, IntPtr state);
}
