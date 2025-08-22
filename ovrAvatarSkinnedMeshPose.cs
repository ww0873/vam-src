using System;
using System.Runtime.InteropServices;

// Token: 0x020007B0 RID: 1968
public struct ovrAvatarSkinnedMeshPose
{
	// Token: 0x04002658 RID: 9816
	public uint jointCount;

	// Token: 0x04002659 RID: 9817
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
	public ovrAvatarTransform[] jointTransform;

	// Token: 0x0400265A RID: 9818
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
	public int[] jointParents;

	// Token: 0x0400265B RID: 9819
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
	public IntPtr[] jointNames;
}
