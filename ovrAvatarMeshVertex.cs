using System;
using System.Runtime.InteropServices;

// Token: 0x0200079A RID: 1946
public struct ovrAvatarMeshVertex
{
	// Token: 0x040025D0 RID: 9680
	public float x;

	// Token: 0x040025D1 RID: 9681
	public float y;

	// Token: 0x040025D2 RID: 9682
	public float z;

	// Token: 0x040025D3 RID: 9683
	public float nx;

	// Token: 0x040025D4 RID: 9684
	public float ny;

	// Token: 0x040025D5 RID: 9685
	public float nz;

	// Token: 0x040025D6 RID: 9686
	public float tx;

	// Token: 0x040025D7 RID: 9687
	public float ty;

	// Token: 0x040025D8 RID: 9688
	public float tz;

	// Token: 0x040025D9 RID: 9689
	public float tw;

	// Token: 0x040025DA RID: 9690
	public float u;

	// Token: 0x040025DB RID: 9691
	public float v;

	// Token: 0x040025DC RID: 9692
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
	public byte[] blendIndices;

	// Token: 0x040025DD RID: 9693
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
	public float[] blendWeights;
}
