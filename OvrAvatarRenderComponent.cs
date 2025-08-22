using System;
using Oculus.Avatar;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x02000794 RID: 1940
public class OvrAvatarRenderComponent : MonoBehaviour
{
	// Token: 0x060031DB RID: 12763 RVA: 0x00105140 File Offset: 0x00103540
	public OvrAvatarRenderComponent()
	{
	}

	// Token: 0x060031DC RID: 12764 RVA: 0x00105150 File Offset: 0x00103550
	protected void UpdateActive(OvrAvatar avatar, ovrAvatarVisibilityFlags mask)
	{
		bool flag = avatar.ShowFirstPerson && (mask & ovrAvatarVisibilityFlags.FirstPerson) != (ovrAvatarVisibilityFlags)0;
		flag |= (avatar.ShowThirdPerson && (mask & ovrAvatarVisibilityFlags.ThirdPerson) != (ovrAvatarVisibilityFlags)0);
		base.gameObject.SetActive(flag);
	}

	// Token: 0x060031DD RID: 12765 RVA: 0x0010519C File Offset: 0x0010359C
	protected SkinnedMeshRenderer CreateSkinnedMesh(ulong assetID, ovrAvatarVisibilityFlags visibilityMask, int thirdPersonLayer, int firstPersonLayer, int sortingOrder)
	{
		OvrAvatarAssetMesh ovrAvatarAssetMesh = (OvrAvatarAssetMesh)OvrAvatarSDKManager.Instance.GetAsset(assetID);
		if (ovrAvatarAssetMesh == null)
		{
			throw new Exception("Couldn't find mesh for asset " + assetID);
		}
		if ((visibilityMask & ovrAvatarVisibilityFlags.ThirdPerson) != (ovrAvatarVisibilityFlags)0)
		{
			base.gameObject.layer = thirdPersonLayer;
		}
		else
		{
			base.gameObject.layer = firstPersonLayer;
		}
		SkinnedMeshRenderer skinnedMeshRenderer = ovrAvatarAssetMesh.CreateSkinnedMeshRendererOnObject(base.gameObject);
		skinnedMeshRenderer.quality = SkinQuality.Bone4;
		skinnedMeshRenderer.sortingOrder = sortingOrder;
		skinnedMeshRenderer.updateWhenOffscreen = true;
		if ((visibilityMask & ovrAvatarVisibilityFlags.SelfOccluding) == (ovrAvatarVisibilityFlags)0)
		{
			skinnedMeshRenderer.shadowCastingMode = ShadowCastingMode.Off;
		}
		return skinnedMeshRenderer;
	}

	// Token: 0x060031DE RID: 12766 RVA: 0x00105230 File Offset: 0x00103630
	protected void UpdateSkinnedMesh(OvrAvatar avatar, Transform[] bones, ovrAvatarTransform localTransform, ovrAvatarVisibilityFlags visibilityMask, IntPtr renderPart)
	{
		this.UpdateActive(avatar, visibilityMask);
		OvrAvatar.ConvertTransform(localTransform, base.transform);
		ovrAvatarRenderPartType ovrAvatarRenderPartType = CAPI.ovrAvatarRenderPart_GetType(renderPart);
		switch (ovrAvatarRenderPartType)
		{
		case ovrAvatarRenderPartType.SkinnedMeshRender:
		{
			ulong num = CAPI.ovrAvatarSkinnedMeshRender_GetDirtyJoints(renderPart);
			goto IL_75;
		}
		case ovrAvatarRenderPartType.SkinnedMeshRenderPBS:
		{
			ulong num = CAPI.ovrAvatarSkinnedMeshRenderPBS_GetDirtyJoints(renderPart);
			goto IL_75;
		}
		case ovrAvatarRenderPartType.SkinnedMeshRenderPBS_V2:
		{
			ulong num = CAPI.ovrAvatarSkinnedMeshRenderPBSV2_GetDirtyJoints(renderPart);
			goto IL_75;
		}
		}
		throw new Exception("Unhandled render part type: " + ovrAvatarRenderPartType);
		IL_75:
		for (uint num2 = 0U; num2 < 64U; num2 += 1U)
		{
			ulong num3 = 1UL << (int)num2;
			ulong num;
			if ((this.firstSkinnedUpdate && (ulong)num2 < (ulong)((long)bones.Length)) || (num3 & num) != 0UL)
			{
				Transform target = bones[(int)((UIntPtr)num2)];
				ovrAvatarTransform transform;
				switch (ovrAvatarRenderPartType)
				{
				case ovrAvatarRenderPartType.SkinnedMeshRender:
					transform = CAPI.ovrAvatarSkinnedMeshRender_GetJointTransform(renderPart, num2);
					break;
				case ovrAvatarRenderPartType.SkinnedMeshRenderPBS:
					transform = CAPI.ovrAvatarSkinnedMeshRenderPBS_GetJointTransform(renderPart, num2);
					break;
				case ovrAvatarRenderPartType.ProjectorRender:
					goto IL_F2;
				case ovrAvatarRenderPartType.SkinnedMeshRenderPBS_V2:
					transform = CAPI.ovrAvatarSkinnedMeshRenderPBSV2_GetJointTransform(renderPart, num2);
					break;
				default:
					goto IL_F2;
				}
				OvrAvatar.ConvertTransform(transform, target);
				goto IL_111;
				IL_F2:
				throw new Exception("Unhandled render part type: " + ovrAvatarRenderPartType);
			}
			IL_111:;
		}
		this.firstSkinnedUpdate = false;
	}

	// Token: 0x060031DF RID: 12767 RVA: 0x00105364 File Offset: 0x00103764
	protected Material CreateAvatarMaterial(string name, Shader shader)
	{
		if (shader == null)
		{
			throw new Exception("No shader provided for avatar material.");
		}
		return new Material(shader)
		{
			name = name
		};
	}

	// Token: 0x040025B7 RID: 9655
	private bool firstSkinnedUpdate = true;

	// Token: 0x040025B8 RID: 9656
	public SkinnedMeshRenderer mesh;

	// Token: 0x040025B9 RID: 9657
	public Transform[] bones;
}
