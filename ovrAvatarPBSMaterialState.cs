using System;
using UnityEngine;

// Token: 0x020007AE RID: 1966
public struct ovrAvatarPBSMaterialState
{
	// Token: 0x060031E6 RID: 12774 RVA: 0x00105E3C File Offset: 0x0010423C
	private static bool VectorEquals(Vector4 a, Vector4 b)
	{
		return a.x == b.x && a.y == b.y && a.z == b.z && a.w == b.w;
	}

	// Token: 0x060031E7 RID: 12775 RVA: 0x00105E98 File Offset: 0x00104298
	public override bool Equals(object obj)
	{
		if (!(obj is ovrAvatarPBSMaterialState))
		{
			return false;
		}
		ovrAvatarPBSMaterialState ovrAvatarPBSMaterialState = (ovrAvatarPBSMaterialState)obj;
		return ovrAvatarPBSMaterialState.VectorEquals(this.baseColor, ovrAvatarPBSMaterialState.baseColor) && this.albedoTextureID == ovrAvatarPBSMaterialState.albedoTextureID && ovrAvatarPBSMaterialState.VectorEquals(this.albedoMultiplier, ovrAvatarPBSMaterialState.albedoMultiplier) && this.metallicnessTextureID == ovrAvatarPBSMaterialState.metallicnessTextureID && this.glossinessScale == ovrAvatarPBSMaterialState.glossinessScale && this.normalTextureID == ovrAvatarPBSMaterialState.normalTextureID && this.heightTextureID == ovrAvatarPBSMaterialState.heightTextureID && this.occlusionTextureID == ovrAvatarPBSMaterialState.occlusionTextureID && this.emissionTextureID == ovrAvatarPBSMaterialState.emissionTextureID && ovrAvatarPBSMaterialState.VectorEquals(this.emissionMultiplier, ovrAvatarPBSMaterialState.emissionMultiplier) && this.detailMaskTextureID == ovrAvatarPBSMaterialState.detailMaskTextureID && this.detailAlbedoTextureID == ovrAvatarPBSMaterialState.detailAlbedoTextureID && this.detailNormalTextureID == ovrAvatarPBSMaterialState.detailNormalTextureID;
	}

	// Token: 0x060031E8 RID: 12776 RVA: 0x00105FD0 File Offset: 0x001043D0
	public override int GetHashCode()
	{
		return this.baseColor.GetHashCode() ^ this.albedoTextureID.GetHashCode() ^ this.albedoMultiplier.GetHashCode() ^ this.metallicnessTextureID.GetHashCode() ^ this.glossinessScale.GetHashCode() ^ this.normalTextureID.GetHashCode() ^ this.heightTextureID.GetHashCode() ^ this.occlusionTextureID.GetHashCode() ^ this.emissionTextureID.GetHashCode() ^ this.emissionMultiplier.GetHashCode() ^ this.detailMaskTextureID.GetHashCode() ^ this.detailAlbedoTextureID.GetHashCode() ^ this.detailNormalTextureID.GetHashCode();
	}

	// Token: 0x0400264A RID: 9802
	public Vector4 baseColor;

	// Token: 0x0400264B RID: 9803
	public ulong albedoTextureID;

	// Token: 0x0400264C RID: 9804
	public Vector4 albedoMultiplier;

	// Token: 0x0400264D RID: 9805
	public ulong metallicnessTextureID;

	// Token: 0x0400264E RID: 9806
	public float glossinessScale;

	// Token: 0x0400264F RID: 9807
	public ulong normalTextureID;

	// Token: 0x04002650 RID: 9808
	public ulong heightTextureID;

	// Token: 0x04002651 RID: 9809
	public ulong occlusionTextureID;

	// Token: 0x04002652 RID: 9810
	public ulong emissionTextureID;

	// Token: 0x04002653 RID: 9811
	public Vector4 emissionMultiplier;

	// Token: 0x04002654 RID: 9812
	public ulong detailMaskTextureID;

	// Token: 0x04002655 RID: 9813
	public ulong detailAlbedoTextureID;

	// Token: 0x04002656 RID: 9814
	public ulong detailNormalTextureID;
}
