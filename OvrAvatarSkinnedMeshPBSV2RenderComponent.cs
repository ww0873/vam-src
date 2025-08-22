using System;
using Oculus.Avatar;
using UnityEngine;

// Token: 0x020007BF RID: 1983
public class OvrAvatarSkinnedMeshPBSV2RenderComponent : OvrAvatarRenderComponent
{
	// Token: 0x06003263 RID: 12899 RVA: 0x00106BEB File Offset: 0x00104FEB
	public OvrAvatarSkinnedMeshPBSV2RenderComponent()
	{
	}

	// Token: 0x06003264 RID: 12900 RVA: 0x00106BF4 File Offset: 0x00104FF4
	internal void Initialize(ovrAvatarRenderPart_SkinnedMeshRenderPBS_V2 skinnedMeshRender, Shader surface, int thirdPersonLayer, int firstPersonLayer, int sortOrder)
	{
		this.surface = ((!(surface != null)) ? Shader.Find("OvrAvatar/AvatarSurfaceShaderPBSV2") : surface);
		this.mesh = base.CreateSkinnedMesh(skinnedMeshRender.meshAssetID, skinnedMeshRender.visibilityMask, thirdPersonLayer, firstPersonLayer, sortOrder);
		this.bones = this.mesh.bones;
		this.UpdateMeshMaterial(skinnedMeshRender.visibilityMask, this.mesh);
	}

	// Token: 0x06003265 RID: 12901 RVA: 0x00106C68 File Offset: 0x00105068
	public void UpdateSkinnedMeshRender(OvrAvatarComponent component, OvrAvatar avatar, IntPtr renderPart)
	{
		ovrAvatarVisibilityFlags visibilityMask = CAPI.ovrAvatarSkinnedMeshRenderPBSV2_GetVisibilityMask(renderPart);
		ovrAvatarTransform localTransform = CAPI.ovrAvatarSkinnedMeshRenderPBSV2_GetTransform(renderPart);
		base.UpdateSkinnedMesh(avatar, this.bones, localTransform, visibilityMask, renderPart);
		this.UpdateMeshMaterial(visibilityMask, (!(this.mesh == null)) ? this.mesh : component.RootMeshComponent);
		bool activeSelf = base.gameObject.activeSelf;
		if (this.mesh != null && (CAPI.ovrAvatarSkinnedMeshRenderPBSV2_MaterialStateChanged(renderPart) || (!this.previouslyActive && activeSelf)))
		{
			ovrAvatarPBSMaterialState ovrAvatarPBSMaterialState = CAPI.ovrAvatarSkinnedMeshRenderPBSV2_GetPBSMaterialState(renderPart);
			Material sharedMaterial = this.mesh.sharedMaterial;
			sharedMaterial.SetVector("_AlbedoMultiplier", ovrAvatarPBSMaterialState.albedoMultiplier);
			sharedMaterial.SetTexture("_Albedo", OvrAvatarComponent.GetLoadedTexture(ovrAvatarPBSMaterialState.albedoTextureID));
			sharedMaterial.SetTexture("_Metallicness", OvrAvatarComponent.GetLoadedTexture(ovrAvatarPBSMaterialState.metallicnessTextureID));
			sharedMaterial.SetFloat("_GlossinessScale", ovrAvatarPBSMaterialState.glossinessScale);
		}
		this.previouslyActive = activeSelf;
	}

	// Token: 0x06003266 RID: 12902 RVA: 0x00106D6C File Offset: 0x0010516C
	private void UpdateMeshMaterial(ovrAvatarVisibilityFlags visibilityMask, SkinnedMeshRenderer rootMesh)
	{
		if (rootMesh.sharedMaterial == null || rootMesh.sharedMaterial.shader != this.surface)
		{
			rootMesh.sharedMaterial = base.CreateAvatarMaterial(base.gameObject.name + "_material", this.surface);
		}
	}

	// Token: 0x0400268A RID: 9866
	private Shader surface;

	// Token: 0x0400268B RID: 9867
	private bool previouslyActive;
}
