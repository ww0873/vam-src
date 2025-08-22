using System;
using System.Runtime.InteropServices;

namespace LeapInternal
{
	// Token: 0x02000622 RID: 1570
	// (Invoke) Token: 0x060026A3 RID: 9891
	[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
	public delegate void Deallocate(IntPtr buffer, IntPtr state);
}
