using System;

namespace Leap.Unity.Attachments
{
	// Token: 0x0200066B RID: 1643
	[Flags]
	public enum AttachmentPointFlags
	{
		// Token: 0x04002186 RID: 8582
		None = 0,
		// Token: 0x04002187 RID: 8583
		Wrist = 2,
		// Token: 0x04002188 RID: 8584
		Palm = 4,
		// Token: 0x04002189 RID: 8585
		ThumbProximalJoint = 8,
		// Token: 0x0400218A RID: 8586
		ThumbDistalJoint = 16,
		// Token: 0x0400218B RID: 8587
		ThumbTip = 32,
		// Token: 0x0400218C RID: 8588
		IndexKnuckle = 64,
		// Token: 0x0400218D RID: 8589
		IndexMiddleJoint = 128,
		// Token: 0x0400218E RID: 8590
		IndexDistalJoint = 256,
		// Token: 0x0400218F RID: 8591
		IndexTip = 512,
		// Token: 0x04002190 RID: 8592
		MiddleKnuckle = 1024,
		// Token: 0x04002191 RID: 8593
		MiddleMiddleJoint = 2048,
		// Token: 0x04002192 RID: 8594
		MiddleDistalJoint = 4096,
		// Token: 0x04002193 RID: 8595
		MiddleTip = 8192,
		// Token: 0x04002194 RID: 8596
		RingKnuckle = 16384,
		// Token: 0x04002195 RID: 8597
		RingMiddleJoint = 32768,
		// Token: 0x04002196 RID: 8598
		RingDistalJoint = 65536,
		// Token: 0x04002197 RID: 8599
		RingTip = 131072,
		// Token: 0x04002198 RID: 8600
		PinkyKnuckle = 262144,
		// Token: 0x04002199 RID: 8601
		PinkyMiddleJoint = 524288,
		// Token: 0x0400219A RID: 8602
		PinkyDistalJoint = 1048576,
		// Token: 0x0400219B RID: 8603
		PinkyTip = 2097152
	}
}
