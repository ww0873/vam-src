using System;
using System.Runtime.InteropServices;

namespace LeapInternal
{
	// Token: 0x02000613 RID: 1555
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct LEAP_POLICY_EVENT
	{
		// Token: 0x04002095 RID: 8341
		public uint reserved;

		// Token: 0x04002096 RID: 8342
		public uint current_policy;
	}
}
