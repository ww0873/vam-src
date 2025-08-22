using System;
using Oculus.Avatar;
using UnityEngine;

// Token: 0x020007BD RID: 1981
public class OvrAvatarSkinnedMeshRenderComponent : OvrAvatarRenderComponent
{
	// Token: 0x0600325C RID: 12892 RVA: 0x00106947 File Offset: 0x00104D47
	public OvrAvatarSkinnedMeshRenderComponent()
	{
	}

	// Token: 0x0600325D RID: 12893 RVA: 0x00106950 File Offset: 0x00104D50
	internal void Initialize(ovrAvatarRenderPart_SkinnedMeshRender skinnedMeshRender, Shader surface, Shader surfaceSelfOccluding, int thirdPersonLayer, int firstPersonLayer, int sortOrder)
	{
		this.surfaceSelfOccluding = ((!(surfaceSelfOccluding != null)) ? Shader.Find("OvrAvatar/AvatarSurfaceShaderSelfOccluding") : surfaceSelfOccluding);
		this.surface = ((!(surface != null)) ? Shader.Find("OvrAvatar/AvatarSurfaceShader") : surface);
		this.mesh = base.CreateSkinnedMesh(skinnedMeshRender.meshAssetID, skinnedMeshRender.visibilityMask, thirdPersonLayer, firstPersonLayer, sortOrder);
		this.bones = this.mesh.bones;
		this.UpdateMeshMaterial(skinnedMeshRender.visibilityMask, this.mesh);
	}

	// Token: 0x0600325E RID: 12894 RVA: 0x001069E8 File Offset: 0x00104DE8
	public void UpdateSkinnedMeshRender(OvrAvatarComponent component, OvrAvatar avatar, IntPtr renderPart)
	{
		ovrAvatarVisibilityFlags visibilityMask = CAPI.ovrAvatarSkinnedMeshRender_GetVisibilityMask(renderPart);
		ovrAvatarTransform localTransform = CAPI.ovrAvatarSkinnedMeshRender_GetTransform(renderPart);
		base.UpdateSkinnedMesh(avatar, this.bones, localTransform, visibilityMask, renderPart);
		this.UpdateMeshMaterial(visibilityMask, (!(this.mesh == null)) ? this.mesh : component.RootMeshComponent);
		bool activeSelf = base.gameObject.activeSelf;
		if (this.mesh != null && (CAPI.ovrAvatarSkinnedMeshRender_MaterialStateChanged(renderPart) || (!this.previouslyActive && activeSelf)))
		{
			ovrAvatarMaterialState matState = CAPI.ovrAvatarSkinnedMeshRender_GetMaterialState(renderPart);
			component.UpdateAvatarMaterial(this.mesh.sharedMaterial, matState);
		}
		this.previouslyActive = activeSelf;
	}

	// Token: 0x0600325F RID: 12895 RVA: 0x00106A9C File Offset: 0x00104E9C
	private void UpdateMeshMaterial(ovrAvatarVisibilityFlags visibilityMask, SkinnedMeshRenderer rootMesh)
	{
		Shader shader = ((visibilityMask & ovrAvatarVisibilityFlags.SelfOccluding) == (ovrAvatarVisibilityFlags)0) ? this.surface : this.surfaceSelfOccluding;
		if (rootMesh.sharedMaterial == null || rootMesh.sharedMaterial.shader != shader)
		{
			rootMesh.sharedMaterial = base.CreateAvatarMaterial(base.gameObject.name + "_material", shader);
		}
	}

	// Token: 0x04002687 RID: 9863
	private Shader surface;

	// Token: 0x04002688 RID: 9864
	private Shader surfaceSelfOccluding;

	// Token: 0x04002689 RID: 9865
	private bool previouslyActive;
}
