using System;
using Oculus.Avatar;
using UnityEngine;

// Token: 0x020007BE RID: 1982
public class OvrAvatarSkinnedMeshRenderPBSComponent : OvrAvatarRenderComponent
{
	// Token: 0x06003260 RID: 12896 RVA: 0x00106B0C File Offset: 0x00104F0C
	public OvrAvatarSkinnedMeshRenderPBSComponent()
	{
	}

	// Token: 0x06003261 RID: 12897 RVA: 0x00106B14 File Offset: 0x00104F14
	internal void Initialize(ovrAvatarRenderPart_SkinnedMeshRenderPBS skinnedMeshRenderPBS, Shader shader, int thirdPersonLayer, int firstPersonLayer, int sortOrder)
	{
		if (shader == null)
		{
			shader = Shader.Find("OvrAvatar/AvatarSurfaceShaderPBS");
		}
		this.mesh = base.CreateSkinnedMesh(skinnedMeshRenderPBS.meshAssetID, skinnedMeshRenderPBS.visibilityMask, thirdPersonLayer, firstPersonLayer, sortOrder);
		this.mesh.sharedMaterial = base.CreateAvatarMaterial(base.gameObject.name + "_material", shader);
		this.bones = this.mesh.bones;
	}

	// Token: 0x06003262 RID: 12898 RVA: 0x00106B90 File Offset: 0x00104F90
	internal void UpdateSkinnedMeshRenderPBS(OvrAvatar avatar, IntPtr renderPart, Material mat)
	{
		ovrAvatarVisibilityFlags visibilityMask = CAPI.ovrAvatarSkinnedMeshRenderPBS_GetVisibilityMask(renderPart);
		ovrAvatarTransform localTransform = CAPI.ovrAvatarSkinnedMeshRenderPBS_GetTransform(renderPart);
		base.UpdateSkinnedMesh(avatar, this.bones, localTransform, visibilityMask, renderPart);
		ulong assetId = CAPI.ovrAvatarSkinnedMeshRenderPBS_GetAlbedoTextureAssetID(renderPart);
		ulong assetId2 = CAPI.ovrAvatarSkinnedMeshRenderPBS_GetSurfaceTextureAssetID(renderPart);
		mat.SetTexture("_Albedo", OvrAvatarComponent.GetLoadedTexture(assetId));
		mat.SetTexture("_Surface", OvrAvatarComponent.GetLoadedTexture(assetId2));
	}
}
