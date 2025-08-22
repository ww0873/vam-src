using System;
using Oculus.Avatar;
using UnityEngine;

// Token: 0x02000783 RID: 1923
public class OvrAvatarAssetTexture : OvrAvatarAsset
{
	// Token: 0x06003199 RID: 12697 RVA: 0x00102DCC File Offset: 0x001011CC
	public OvrAvatarAssetTexture(ulong _assetId, IntPtr asset)
	{
		this.assetID = _assetId;
		ovrAvatarTextureAssetData ovrAvatarTextureAssetData = CAPI.ovrAvatarAsset_GetTextureData(asset);
		IntPtr textureData = ovrAvatarTextureAssetData.textureData;
		int num = (int)ovrAvatarTextureAssetData.textureDataSize;
		TextureFormat format;
		switch (ovrAvatarTextureAssetData.format)
		{
		case ovrAvatarTextureFormat.RGB24:
			format = TextureFormat.RGB24;
			break;
		case ovrAvatarTextureFormat.DXT1:
			format = TextureFormat.DXT1;
			break;
		case ovrAvatarTextureFormat.DXT5:
			format = TextureFormat.DXT5;
			break;
		case ovrAvatarTextureFormat.ASTC_RGB_6x6:
			format = TextureFormat.ASTC_RGB_6x6;
			textureData = new IntPtr(textureData.ToInt64() + 16L);
			num -= 16;
			break;
		case ovrAvatarTextureFormat.ASTC_RGB_6x6_MIPMAPS:
			format = TextureFormat.ASTC_RGB_6x6;
			break;
		default:
			throw new NotImplementedException(string.Format("Unsupported texture format {0}", ovrAvatarTextureAssetData.format.ToString()));
		}
		this.texture = new Texture2D((int)ovrAvatarTextureAssetData.sizeX, (int)ovrAvatarTextureAssetData.sizeY, format, ovrAvatarTextureAssetData.mipCount > 1U, false);
		this.texture.LoadRawTextureData(textureData, num);
		this.texture.Apply(true, false);
	}

	// Token: 0x04002572 RID: 9586
	public Texture2D texture;

	// Token: 0x04002573 RID: 9587
	private const int ASTCHeaderSize = 16;
}
