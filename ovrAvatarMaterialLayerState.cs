using System;
using UnityEngine;

// Token: 0x020007AC RID: 1964
public struct ovrAvatarMaterialLayerState
{
	// Token: 0x060031E0 RID: 12768 RVA: 0x001058DC File Offset: 0x00103CDC
	private static bool VectorEquals(Vector4 a, Vector4 b)
	{
		return a.x == b.x && a.y == b.y && a.z == b.z && a.w == b.w;
	}

	// Token: 0x060031E1 RID: 12769 RVA: 0x00105938 File Offset: 0x00103D38
	public override bool Equals(object obj)
	{
		if (!(obj is ovrAvatarMaterialLayerState))
		{
			return false;
		}
		ovrAvatarMaterialLayerState ovrAvatarMaterialLayerState = (ovrAvatarMaterialLayerState)obj;
		return this.blendMode == ovrAvatarMaterialLayerState.blendMode && this.sampleMode == ovrAvatarMaterialLayerState.sampleMode && this.maskType == ovrAvatarMaterialLayerState.maskType && ovrAvatarMaterialLayerState.VectorEquals(this.layerColor, ovrAvatarMaterialLayerState.layerColor) && ovrAvatarMaterialLayerState.VectorEquals(this.sampleParameters, ovrAvatarMaterialLayerState.sampleParameters) && this.sampleTexture == ovrAvatarMaterialLayerState.sampleTexture && ovrAvatarMaterialLayerState.VectorEquals(this.sampleScaleOffset, ovrAvatarMaterialLayerState.sampleScaleOffset) && ovrAvatarMaterialLayerState.VectorEquals(this.maskParameters, ovrAvatarMaterialLayerState.maskParameters) && ovrAvatarMaterialLayerState.VectorEquals(this.maskAxis, ovrAvatarMaterialLayerState.maskAxis);
	}

	// Token: 0x060031E2 RID: 12770 RVA: 0x00105A28 File Offset: 0x00103E28
	public override int GetHashCode()
	{
		return this.blendMode.GetHashCode() ^ this.sampleMode.GetHashCode() ^ this.maskType.GetHashCode() ^ this.layerColor.GetHashCode() ^ this.sampleParameters.GetHashCode() ^ this.sampleTexture.GetHashCode() ^ this.sampleScaleOffset.GetHashCode() ^ this.maskParameters.GetHashCode() ^ this.maskAxis.GetHashCode();
	}

	// Token: 0x04002632 RID: 9778
	public ovrAvatarMaterialLayerBlendMode blendMode;

	// Token: 0x04002633 RID: 9779
	public ovrAvatarMaterialLayerSampleMode sampleMode;

	// Token: 0x04002634 RID: 9780
	public ovrAvatarMaterialMaskType maskType;

	// Token: 0x04002635 RID: 9781
	public Vector4 layerColor;

	// Token: 0x04002636 RID: 9782
	public Vector4 sampleParameters;

	// Token: 0x04002637 RID: 9783
	public ulong sampleTexture;

	// Token: 0x04002638 RID: 9784
	public Vector4 sampleScaleOffset;

	// Token: 0x04002639 RID: 9785
	public Vector4 maskParameters;

	// Token: 0x0400263A RID: 9786
	public Vector4 maskAxis;
}
