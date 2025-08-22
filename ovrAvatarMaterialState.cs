using System;
using System.Runtime.InteropServices;
using UnityEngine;

// Token: 0x020007AD RID: 1965
public struct ovrAvatarMaterialState
{
	// Token: 0x060031E3 RID: 12771 RVA: 0x00105AD8 File Offset: 0x00103ED8
	private static bool VectorEquals(Vector4 a, Vector4 b)
	{
		return a.x == b.x && a.y == b.y && a.z == b.z && a.w == b.w;
	}

	// Token: 0x060031E4 RID: 12772 RVA: 0x00105B34 File Offset: 0x00103F34
	public override bool Equals(object obj)
	{
		if (!(obj is ovrAvatarMaterialState))
		{
			return false;
		}
		ovrAvatarMaterialState ovrAvatarMaterialState = (ovrAvatarMaterialState)obj;
		if (!ovrAvatarMaterialState.VectorEquals(this.baseColor, ovrAvatarMaterialState.baseColor))
		{
			return false;
		}
		if (this.baseMaskType != ovrAvatarMaterialState.baseMaskType)
		{
			return false;
		}
		if (!ovrAvatarMaterialState.VectorEquals(this.baseMaskParameters, ovrAvatarMaterialState.baseMaskParameters))
		{
			return false;
		}
		if (!ovrAvatarMaterialState.VectorEquals(this.baseMaskAxis, ovrAvatarMaterialState.baseMaskAxis))
		{
			return false;
		}
		if (this.sampleMode != ovrAvatarMaterialState.sampleMode)
		{
			return false;
		}
		if (this.alphaMaskTextureID != ovrAvatarMaterialState.alphaMaskTextureID)
		{
			return false;
		}
		if (!ovrAvatarMaterialState.VectorEquals(this.alphaMaskScaleOffset, ovrAvatarMaterialState.alphaMaskScaleOffset))
		{
			return false;
		}
		if (this.normalMapTextureID != ovrAvatarMaterialState.normalMapTextureID)
		{
			return false;
		}
		if (!ovrAvatarMaterialState.VectorEquals(this.normalMapScaleOffset, ovrAvatarMaterialState.normalMapScaleOffset))
		{
			return false;
		}
		if (this.parallaxMapTextureID != ovrAvatarMaterialState.parallaxMapTextureID)
		{
			return false;
		}
		if (!ovrAvatarMaterialState.VectorEquals(this.parallaxMapScaleOffset, ovrAvatarMaterialState.parallaxMapScaleOffset))
		{
			return false;
		}
		if (this.roughnessMapTextureID != ovrAvatarMaterialState.roughnessMapTextureID)
		{
			return false;
		}
		if (!ovrAvatarMaterialState.VectorEquals(this.roughnessMapScaleOffset, ovrAvatarMaterialState.roughnessMapScaleOffset))
		{
			return false;
		}
		if (this.layerCount != ovrAvatarMaterialState.layerCount)
		{
			return false;
		}
		int num = 0;
		while ((long)num < (long)((ulong)this.layerCount))
		{
			if (!this.layers[num].Equals(ovrAvatarMaterialState.layers[num]))
			{
				return false;
			}
			num++;
		}
		return true;
	}

	// Token: 0x060031E5 RID: 12773 RVA: 0x00105CE0 File Offset: 0x001040E0
	public override int GetHashCode()
	{
		int num = 0;
		num ^= this.baseColor.GetHashCode();
		num ^= this.baseMaskType.GetHashCode();
		num ^= this.baseMaskParameters.GetHashCode();
		num ^= this.baseMaskAxis.GetHashCode();
		num ^= this.sampleMode.GetHashCode();
		num ^= this.alphaMaskTextureID.GetHashCode();
		num ^= this.alphaMaskScaleOffset.GetHashCode();
		num ^= this.normalMapTextureID.GetHashCode();
		num ^= this.normalMapScaleOffset.GetHashCode();
		num ^= this.parallaxMapTextureID.GetHashCode();
		num ^= this.parallaxMapScaleOffset.GetHashCode();
		num ^= this.roughnessMapTextureID.GetHashCode();
		num ^= this.roughnessMapScaleOffset.GetHashCode();
		num ^= this.layerCount.GetHashCode();
		int num2 = 0;
		while ((long)num2 < (long)((ulong)this.layerCount))
		{
			num ^= this.layers[num2].GetHashCode();
			num2++;
		}
		return num;
	}

	// Token: 0x0400263B RID: 9787
	public Vector4 baseColor;

	// Token: 0x0400263C RID: 9788
	public ovrAvatarMaterialMaskType baseMaskType;

	// Token: 0x0400263D RID: 9789
	public Vector4 baseMaskParameters;

	// Token: 0x0400263E RID: 9790
	public Vector4 baseMaskAxis;

	// Token: 0x0400263F RID: 9791
	public ovrAvatarMaterialLayerSampleMode sampleMode;

	// Token: 0x04002640 RID: 9792
	public ulong alphaMaskTextureID;

	// Token: 0x04002641 RID: 9793
	public Vector4 alphaMaskScaleOffset;

	// Token: 0x04002642 RID: 9794
	public ulong normalMapTextureID;

	// Token: 0x04002643 RID: 9795
	public Vector4 normalMapScaleOffset;

	// Token: 0x04002644 RID: 9796
	public ulong parallaxMapTextureID;

	// Token: 0x04002645 RID: 9797
	public Vector4 parallaxMapScaleOffset;

	// Token: 0x04002646 RID: 9798
	public ulong roughnessMapTextureID;

	// Token: 0x04002647 RID: 9799
	public Vector4 roughnessMapScaleOffset;

	// Token: 0x04002648 RID: 9800
	public uint layerCount;

	// Token: 0x04002649 RID: 9801
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
	public ovrAvatarMaterialLayerState[] layers;
}
