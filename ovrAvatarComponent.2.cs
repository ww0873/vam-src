using System;
using System.Runtime.InteropServices;

// Token: 0x020007A3 RID: 1955
public struct ovrAvatarComponent
{
	// Token: 0x0400260E RID: 9742
	public ovrAvatarTransform transform;

	// Token: 0x0400260F RID: 9743
	public uint renderPartCount;

	// Token: 0x04002610 RID: 9744
	public IntPtr renderParts;

	// Token: 0x04002611 RID: 9745
	[MarshalAs(UnmanagedType.LPStr)]
	public string name;
}
