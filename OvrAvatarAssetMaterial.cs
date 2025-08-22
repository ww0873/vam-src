using System;
using Oculus.Avatar;

// Token: 0x020007AF RID: 1967
public class OvrAvatarAssetMaterial : OvrAvatarAsset
{
	// Token: 0x060031E9 RID: 12777 RVA: 0x001060C6 File Offset: 0x001044C6
	public OvrAvatarAssetMaterial(ulong id, IntPtr mat)
	{
		this.assetID = id;
		this.material = CAPI.ovrAvatarAsset_GetMaterialState(mat);
	}

	// Token: 0x04002657 RID: 9815
	public ovrAvatarMaterialState material;
}
