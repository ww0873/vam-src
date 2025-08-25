using System;

// Token: 0x020007B3 RID: 1971
public struct ovrAvatarRenderPart_SkinnedMeshRenderPBS
{
	// Token: 0x04002665 RID: 9829
	public ovrAvatarTransform localTransform;

	// Token: 0x04002666 RID: 9830
	public ovrAvatarVisibilityFlags visibilityMask;

	// Token: 0x04002667 RID: 9831
	public ulong meshAssetID;

	// Token: 0x04002668 RID: 9832
	public ulong albedoTextureAssetID;

	// Token: 0x04002669 RID: 9833
	public ulong surfaceTextureAssetID;

	// Token: 0x0400266A RID: 9834
	public ovrAvatarSkinnedMeshPose skinnedPose;
}
