using System;

namespace LeapInternal
{
	// Token: 0x020005E9 RID: 1513
	public enum eLeapPolicyFlag : uint
	{
		// Token: 0x04001FBA RID: 8122
		eLeapPolicyFlag_BackgroundFrames = 1U,
		// Token: 0x04001FBB RID: 8123
		eLeapPolicyFlag_Images,
		// Token: 0x04001FBC RID: 8124
		eLeapPolicyFlag_OptimizeHMD = 4U,
		// Token: 0x04001FBD RID: 8125
		eLeapPolicyFlag_AllowPauseResume = 8U,
		// Token: 0x04001FBE RID: 8126
		eLeapPolicyFlag_MapPoints = 128U
	}
}
