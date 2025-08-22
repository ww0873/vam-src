using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Oculus.Avatar;
using UnityEngine;

// Token: 0x02000786 RID: 1926
public class OvrAvatarComponent : MonoBehaviour
{
	// Token: 0x0600319D RID: 12701 RVA: 0x00102ED8 File Offset: 0x001012D8
	public OvrAvatarComponent()
	{
	}

	// Token: 0x0600319E RID: 12702 RVA: 0x00102F00 File Offset: 0x00101300
	public void StartMeshCombining(ovrAvatarComponent component)
	{
		this.IsCombiningMeshes = true;
		base.gameObject.SetActive(false);
		this.ThreadData = new OvrAvatarComponent.MeshThreadData[this.RenderParts.Count];
		uint num = 0U;
		while ((ulong)num < (ulong)((long)this.RenderParts.Count))
		{
			OvrAvatarRenderComponent ovrAvatarRenderComponent = this.RenderParts[(int)num];
			IntPtr renderPart = OvrAvatar.GetRenderPart(component, num);
			ovrAvatarMaterialState ovrAvatarMaterialState = CAPI.ovrAvatarSkinnedMeshRender_GetMaterialState(renderPart);
			this.ThreadData[(int)((UIntPtr)num)].VertexCount = ovrAvatarRenderComponent.mesh.sharedMesh.vertexCount;
			this.ThreadData[(int)((UIntPtr)num)].IsDarkMaterial = (num != 0U);
			if (ovrAvatarMaterialState.alphaMaskTextureID != 0UL)
			{
				if (num != 0U)
				{
					this.ClothingAlphaOffset = ovrAvatarMaterialState.alphaMaskScaleOffset;
					this.ClothingAlphaTexture = ovrAvatarMaterialState.alphaMaskTextureID;
				}
				this.ThreadData[(int)((UIntPtr)num)].UsesAlpha = true;
			}
			this.ThreadData[(int)((UIntPtr)num)].MeshColors = ovrAvatarRenderComponent.mesh.sharedMesh.colors;
			num += 1U;
		}
		this.VertexThread = new Thread(new ThreadStart(this.<StartMeshCombining>m__0));
		this.VertexThread.Start();
	}

	// Token: 0x0600319F RID: 12703 RVA: 0x00103030 File Offset: 0x00101430
	private void UpdateVertices(ref OvrAvatarComponent.MeshThreadData[] threadData)
	{
		this.IsVertexDataUpdating = true;
		foreach (OvrAvatarComponent.MeshThreadData meshThreadData in threadData)
		{
			for (int j = 0; j < meshThreadData.VertexCount; j++)
			{
				meshThreadData.MeshColors[j].a = 0f;
				if (meshThreadData.UsesAlpha)
				{
					meshThreadData.MeshColors[j].a = ((!meshThreadData.IsDarkMaterial) ? 0.5f : 1f);
				}
				meshThreadData.MeshColors[j].r = ((!meshThreadData.IsDarkMaterial) ? 0f : 1f);
			}
		}
		this.IsVertexDataUpdating = false;
	}

	// Token: 0x060031A0 RID: 12704 RVA: 0x00103104 File Offset: 0x00101504
	public void CombineMeshes(ovrAvatarComponent component)
	{
		List<Transform> list = new List<Transform>();
		List<BoneWeight> list2 = new List<BoneWeight>();
		List<CombineInstance> list3 = new List<CombineInstance>();
		List<Matrix4x4> list4 = new List<Matrix4x4>();
		List<Color> list5 = new List<Color>();
		this.RootMeshComponent = base.gameObject.AddComponent<SkinnedMeshRenderer>();
		this.RootMeshComponent.quality = SkinQuality.Bone4;
		this.RootMeshComponent.updateWhenOffscreen = true;
		int num = 0;
		uint num2 = 0U;
		while ((ulong)num2 < (ulong)((long)this.RenderParts.Count))
		{
			OvrAvatarRenderComponent ovrAvatarRenderComponent = this.RenderParts[(int)num2];
			if (this.RootMeshComponent.sharedMaterial == null)
			{
				this.RootMeshComponent.sharedMaterial = ovrAvatarRenderComponent.mesh.sharedMaterial;
				this.materialStates.Add(this.RootMeshComponent.sharedMaterial, default(ovrAvatarMaterialState));
				this.RootMeshComponent.sortingLayerID = ovrAvatarRenderComponent.mesh.sortingLayerID;
				this.RootMeshComponent.gameObject.layer = ovrAvatarRenderComponent.gameObject.layer;
			}
			list5.AddRange(this.ThreadData[(int)((UIntPtr)num2)].MeshColors);
			foreach (BoneWeight boneWeight in ovrAvatarRenderComponent.mesh.sharedMesh.boneWeights)
			{
				BoneWeight item = boneWeight;
				item.boneIndex0 += num;
				item.boneIndex1 += num;
				item.boneIndex2 += num;
				item.boneIndex3 += num;
				list2.Add(item);
			}
			num += ovrAvatarRenderComponent.mesh.bones.Length;
			foreach (Transform item2 in ovrAvatarRenderComponent.mesh.bones)
			{
				list.Add(item2);
			}
			list3.Add(new CombineInstance
			{
				mesh = ovrAvatarRenderComponent.mesh.sharedMesh,
				transform = ovrAvatarRenderComponent.mesh.transform.localToWorldMatrix
			});
			for (int k = 0; k < ovrAvatarRenderComponent.mesh.bones.Length; k++)
			{
				list4.Add(ovrAvatarRenderComponent.mesh.sharedMesh.bindposes[k]);
			}
			UnityEngine.Object.Destroy(ovrAvatarRenderComponent.mesh);
			ovrAvatarRenderComponent.mesh = null;
			num2 += 1U;
		}
		this.RootMeshComponent.sharedMesh = new Mesh();
		this.RootMeshComponent.sharedMesh.name = base.transform.name + "_combined_mesh";
		this.RootMeshComponent.sharedMesh.CombineMeshes(list3.ToArray(), true, true);
		this.RootMeshComponent.bones = list.ToArray();
		this.RootMeshComponent.sharedMesh.boneWeights = list2.ToArray();
		this.RootMeshComponent.sharedMesh.bindposes = list4.ToArray();
		this.RootMeshComponent.sharedMesh.colors = list5.ToArray();
		this.RootMeshComponent.rootBone = list[0];
		this.RootMeshComponent.sharedMesh.RecalculateBounds();
	}

	// Token: 0x060031A1 RID: 12705 RVA: 0x00103450 File Offset: 0x00101850
	public void UpdateAvatar(ovrAvatarComponent component, OvrAvatar avatar)
	{
		if (this.IsCombiningMeshes)
		{
			if (!this.IsVertexDataUpdating)
			{
				this.CombineMeshes(component);
				this.IsCombiningMeshes = false;
				this.ThreadData = null;
			}
			return;
		}
		OvrAvatar.ConvertTransform(component.transform, base.transform);
		for (uint num = 0U; num < component.renderPartCount; num += 1U)
		{
			if ((long)this.RenderParts.Count <= (long)((ulong)num))
			{
				break;
			}
			OvrAvatarRenderComponent ovrAvatarRenderComponent = this.RenderParts[(int)num];
			IntPtr renderPart = OvrAvatar.GetRenderPart(component, num);
			switch (CAPI.ovrAvatarRenderPart_GetType(renderPart))
			{
			case ovrAvatarRenderPartType.SkinnedMeshRender:
				((OvrAvatarSkinnedMeshRenderComponent)ovrAvatarRenderComponent).UpdateSkinnedMeshRender(this, avatar, renderPart);
				break;
			case ovrAvatarRenderPartType.SkinnedMeshRenderPBS:
			{
				Material mat = (!(this.RootMeshComponent != null)) ? ovrAvatarRenderComponent.mesh.sharedMaterial : this.RootMeshComponent.sharedMaterial;
				((OvrAvatarSkinnedMeshRenderPBSComponent)ovrAvatarRenderComponent).UpdateSkinnedMeshRenderPBS(avatar, renderPart, mat);
				break;
			}
			case ovrAvatarRenderPartType.ProjectorRender:
				((OvrAvatarProjectorRenderComponent)ovrAvatarRenderComponent).UpdateProjectorRender(this, CAPI.ovrAvatarRenderPart_GetProjectorRender(renderPart));
				break;
			case ovrAvatarRenderPartType.SkinnedMeshRenderPBS_V2:
				((OvrAvatarSkinnedMeshPBSV2RenderComponent)ovrAvatarRenderComponent).UpdateSkinnedMeshRender(this, avatar, renderPart);
				break;
			}
		}
		if (this.RootMeshComponent != null)
		{
			IntPtr renderPart2 = OvrAvatar.GetRenderPart(component, 0U);
			switch (CAPI.ovrAvatarRenderPart_GetType(renderPart2))
			{
			case ovrAvatarRenderPartType.SkinnedMeshRender:
			{
				this.UpdateActive(avatar, CAPI.ovrAvatarSkinnedMeshRender_GetVisibilityMask(renderPart2));
				bool flag = (this.FirstMaterialUpdate && base.gameObject.activeSelf) || CAPI.ovrAvatarSkinnedMeshRender_MaterialStateChanged(renderPart2);
				if (flag)
				{
					this.FirstMaterialUpdate = false;
					ovrAvatarMaterialState matState = CAPI.ovrAvatarSkinnedMeshRender_GetMaterialState(renderPart2);
					this.UpdateAvatarMaterial(this.RootMeshComponent.sharedMaterial, matState);
				}
				break;
			}
			case ovrAvatarRenderPartType.SkinnedMeshRenderPBS:
				this.UpdateActive(avatar, CAPI.ovrAvatarSkinnedMeshRenderPBS_GetVisibilityMask(renderPart2));
				break;
			}
		}
		this.DebugDrawTransforms();
	}

	// Token: 0x060031A2 RID: 12706 RVA: 0x00103644 File Offset: 0x00101A44
	protected void UpdateActive(OvrAvatar avatar, ovrAvatarVisibilityFlags mask)
	{
		bool flag = avatar.ShowFirstPerson && (mask & ovrAvatarVisibilityFlags.FirstPerson) != (ovrAvatarVisibilityFlags)0;
		flag |= (avatar.ShowThirdPerson && (mask & ovrAvatarVisibilityFlags.ThirdPerson) != (ovrAvatarVisibilityFlags)0);
		base.gameObject.SetActive(flag);
	}

	// Token: 0x060031A3 RID: 12707 RVA: 0x00103690 File Offset: 0x00101A90
	private void DebugDrawTransforms()
	{
		Color[] array = new Color[]
		{
			Color.red,
			Color.white,
			Color.blue
		};
		int num = 0;
		if (this.DrawSkeleton && this.RootMeshComponent != null && this.RootMeshComponent.bones != null)
		{
			foreach (Transform transform in this.RootMeshComponent.bones)
			{
				if (transform.parent)
				{
					Debug.DrawLine(transform.position, transform.parent.position, array[num++ % array.Length]);
				}
			}
		}
	}

	// Token: 0x060031A4 RID: 12708 RVA: 0x00103768 File Offset: 0x00101B68
	public void UpdateAvatarMaterial(Material mat, ovrAvatarMaterialState matState)
	{
		mat.SetColor("_BaseColor", matState.baseColor);
		mat.SetInt("_BaseMaskType", (int)matState.baseMaskType);
		mat.SetVector("_BaseMaskParameters", matState.baseMaskParameters);
		mat.SetVector("_BaseMaskAxis", matState.baseMaskAxis);
		if (matState.alphaMaskTextureID != 0UL)
		{
			mat.SetTexture("_AlphaMask", OvrAvatarComponent.GetLoadedTexture(matState.alphaMaskTextureID));
			mat.SetTextureScale("_AlphaMask", new Vector2(matState.alphaMaskScaleOffset.x, matState.alphaMaskScaleOffset.y));
			mat.SetTextureOffset("_AlphaMask", new Vector2(matState.alphaMaskScaleOffset.z, matState.alphaMaskScaleOffset.w));
		}
		if (this.ClothingAlphaTexture != 0UL)
		{
			mat.EnableKeyword("VERTALPHA_ON");
			mat.SetTexture("_AlphaMask2", OvrAvatarComponent.GetLoadedTexture(this.ClothingAlphaTexture));
			mat.SetTextureScale("_AlphaMask2", new Vector2(this.ClothingAlphaOffset.x, this.ClothingAlphaOffset.y));
			mat.SetTextureOffset("_AlphaMask2", new Vector2(this.ClothingAlphaOffset.z, this.ClothingAlphaOffset.w));
		}
		if (matState.normalMapTextureID != 0UL)
		{
			mat.EnableKeyword("NORMAL_MAP_ON");
			mat.SetTexture("_NormalMap", OvrAvatarComponent.GetLoadedTexture(matState.normalMapTextureID));
			mat.SetTextureScale("_NormalMap", new Vector2(matState.normalMapScaleOffset.x, matState.normalMapScaleOffset.y));
			mat.SetTextureOffset("_NormalMap", new Vector2(matState.normalMapScaleOffset.z, matState.normalMapScaleOffset.w));
		}
		if (matState.parallaxMapTextureID != 0UL)
		{
			mat.SetTexture("_ParallaxMap", OvrAvatarComponent.GetLoadedTexture(matState.parallaxMapTextureID));
			mat.SetTextureScale("_ParallaxMap", new Vector2(matState.parallaxMapScaleOffset.x, matState.parallaxMapScaleOffset.y));
			mat.SetTextureOffset("_ParallaxMap", new Vector2(matState.parallaxMapScaleOffset.z, matState.parallaxMapScaleOffset.w));
		}
		if (matState.roughnessMapTextureID != 0UL)
		{
			mat.EnableKeyword("ROUGHNESS_ON");
			mat.SetTexture("_RoughnessMap", OvrAvatarComponent.GetLoadedTexture(matState.roughnessMapTextureID));
			mat.SetTextureScale("_RoughnessMap", new Vector2(matState.roughnessMapScaleOffset.x, matState.roughnessMapScaleOffset.y));
			mat.SetTextureOffset("_RoughnessMap", new Vector2(matState.roughnessMapScaleOffset.z, matState.roughnessMapScaleOffset.w));
		}
		mat.EnableKeyword(OvrAvatarComponent.LayerKeywords[(int)((UIntPtr)matState.layerCount)]);
		for (ulong num = 0UL; num < (ulong)matState.layerCount; num += 1UL)
		{
			checked
			{
				ovrAvatarMaterialLayerState ovrAvatarMaterialLayerState = matState.layers[(int)((IntPtr)num)];
				mat.SetInt(OvrAvatarComponent.LayerSampleModeParameters[(int)((IntPtr)num)], (int)ovrAvatarMaterialLayerState.sampleMode);
				mat.SetInt(OvrAvatarComponent.LayerBlendModeParameters[(int)((IntPtr)num)], (int)ovrAvatarMaterialLayerState.blendMode);
				mat.SetInt(OvrAvatarComponent.LayerMaskTypeParameters[(int)((IntPtr)num)], (int)ovrAvatarMaterialLayerState.maskType);
				mat.SetColor(OvrAvatarComponent.LayerColorParameters[(int)((IntPtr)num)], ovrAvatarMaterialLayerState.layerColor);
				if (ovrAvatarMaterialLayerState.sampleMode != ovrAvatarMaterialLayerSampleMode.Color)
				{
					string name = OvrAvatarComponent.LayerSurfaceParameters[(int)((IntPtr)num)];
					mat.SetTexture(name, OvrAvatarComponent.GetLoadedTexture(ovrAvatarMaterialLayerState.sampleTexture));
					mat.SetTextureScale(name, new Vector2(ovrAvatarMaterialLayerState.sampleScaleOffset.x, ovrAvatarMaterialLayerState.sampleScaleOffset.y));
					mat.SetTextureOffset(name, new Vector2(ovrAvatarMaterialLayerState.sampleScaleOffset.z, ovrAvatarMaterialLayerState.sampleScaleOffset.w));
				}
				if (ovrAvatarMaterialLayerState.sampleMode == ovrAvatarMaterialLayerSampleMode.Parallax)
				{
					mat.EnableKeyword("PARALLAX_ON");
				}
				mat.SetColor(OvrAvatarComponent.LayerSampleParametersParameters[(int)((IntPtr)num)], ovrAvatarMaterialLayerState.sampleParameters);
				mat.SetColor(OvrAvatarComponent.LayerMaskParametersParameters[(int)((IntPtr)num)], ovrAvatarMaterialLayerState.maskParameters);
				mat.SetColor(OvrAvatarComponent.LayerMaskAxisParameters[(int)((IntPtr)num)], ovrAvatarMaterialLayerState.maskAxis);
			}
		}
		this.materialStates[mat] = matState;
	}

	// Token: 0x060031A5 RID: 12709 RVA: 0x00103BA8 File Offset: 0x00101FA8
	public static Texture2D GetLoadedTexture(ulong assetId)
	{
		if (assetId == 0UL)
		{
			return null;
		}
		OvrAvatarAssetTexture ovrAvatarAssetTexture = (OvrAvatarAssetTexture)OvrAvatarSDKManager.Instance.GetAsset(assetId);
		if (ovrAvatarAssetTexture == null)
		{
			throw new Exception("Could not find texture for asset " + assetId);
		}
		return ovrAvatarAssetTexture.texture;
	}

	// Token: 0x060031A6 RID: 12710 RVA: 0x00103BF4 File Offset: 0x00101FF4
	// Note: this type is marked as 'beforefieldinit'.
	static OvrAvatarComponent()
	{
	}

	// Token: 0x060031A7 RID: 12711 RVA: 0x00103EAD File Offset: 0x001022AD
	[CompilerGenerated]
	private void <StartMeshCombining>m__0()
	{
		this.UpdateVertices(ref this.ThreadData);
	}

	// Token: 0x04002574 RID: 9588
	public static readonly string[] LayerKeywords = new string[]
	{
		"LAYERS_0",
		"LAYERS_1",
		"LAYERS_2",
		"LAYERS_3",
		"LAYERS_4",
		"LAYERS_5",
		"LAYERS_6",
		"LAYERS_7",
		"LAYERS_8"
	};

	// Token: 0x04002575 RID: 9589
	public static readonly string[] LayerSampleModeParameters = new string[]
	{
		"_LayerSampleMode0",
		"_LayerSampleMode1",
		"_LayerSampleMode2",
		"_LayerSampleMode3",
		"_LayerSampleMode4",
		"_LayerSampleMode5",
		"_LayerSampleMode6",
		"_LayerSampleMode7"
	};

	// Token: 0x04002576 RID: 9590
	public static readonly string[] LayerBlendModeParameters = new string[]
	{
		"_LayerBlendMode0",
		"_LayerBlendMode1",
		"_LayerBlendMode2",
		"_LayerBlendMode3",
		"_LayerBlendMode4",
		"_LayerBlendMode5",
		"_LayerBlendMode6",
		"_LayerBlendMode7"
	};

	// Token: 0x04002577 RID: 9591
	public static readonly string[] LayerMaskTypeParameters = new string[]
	{
		"_LayerMaskType0",
		"_LayerMaskType1",
		"_LayerMaskType2",
		"_LayerMaskType3",
		"_LayerMaskType4",
		"_LayerMaskType5",
		"_LayerMaskType6",
		"_LayerMaskType7"
	};

	// Token: 0x04002578 RID: 9592
	public static readonly string[] LayerColorParameters = new string[]
	{
		"_LayerColor0",
		"_LayerColor1",
		"_LayerColor2",
		"_LayerColor3",
		"_LayerColor4",
		"_LayerColor5",
		"_LayerColor6",
		"_LayerColor7"
	};

	// Token: 0x04002579 RID: 9593
	public static readonly string[] LayerSurfaceParameters = new string[]
	{
		"_LayerSurface0",
		"_LayerSurface1",
		"_LayerSurface2",
		"_LayerSurface3",
		"_LayerSurface4",
		"_LayerSurface5",
		"_LayerSurface6",
		"_LayerSurface7"
	};

	// Token: 0x0400257A RID: 9594
	public static readonly string[] LayerSampleParametersParameters = new string[]
	{
		"_LayerSampleParameters0",
		"_LayerSampleParameters1",
		"_LayerSampleParameters2",
		"_LayerSampleParameters3",
		"_LayerSampleParameters4",
		"_LayerSampleParameters5",
		"_LayerSampleParameters6",
		"_LayerSampleParameters7"
	};

	// Token: 0x0400257B RID: 9595
	public static readonly string[] LayerMaskParametersParameters = new string[]
	{
		"_LayerMaskParameters0",
		"_LayerMaskParameters1",
		"_LayerMaskParameters2",
		"_LayerMaskParameters3",
		"_LayerMaskParameters4",
		"_LayerMaskParameters5",
		"_LayerMaskParameters6",
		"_LayerMaskParameters7"
	};

	// Token: 0x0400257C RID: 9596
	public static readonly string[] LayerMaskAxisParameters = new string[]
	{
		"_LayerMaskAxis0",
		"_LayerMaskAxis1",
		"_LayerMaskAxis2",
		"_LayerMaskAxis3",
		"_LayerMaskAxis4",
		"_LayerMaskAxis5",
		"_LayerMaskAxis6",
		"_LayerMaskAxis7"
	};

	// Token: 0x0400257D RID: 9597
	public SkinnedMeshRenderer RootMeshComponent;

	// Token: 0x0400257E RID: 9598
	private Dictionary<Material, ovrAvatarMaterialState> materialStates = new Dictionary<Material, ovrAvatarMaterialState>();

	// Token: 0x0400257F RID: 9599
	public List<OvrAvatarRenderComponent> RenderParts = new List<OvrAvatarRenderComponent>();

	// Token: 0x04002580 RID: 9600
	private bool DrawSkeleton;

	// Token: 0x04002581 RID: 9601
	private bool IsVertexDataUpdating;

	// Token: 0x04002582 RID: 9602
	private bool IsCombiningMeshes;

	// Token: 0x04002583 RID: 9603
	private bool FirstMaterialUpdate = true;

	// Token: 0x04002584 RID: 9604
	private ulong ClothingAlphaTexture;

	// Token: 0x04002585 RID: 9605
	private Vector4 ClothingAlphaOffset;

	// Token: 0x04002586 RID: 9606
	private Thread VertexThread;

	// Token: 0x04002587 RID: 9607
	private OvrAvatarComponent.MeshThreadData[] ThreadData;

	// Token: 0x02000787 RID: 1927
	private struct MeshThreadData
	{
		// Token: 0x04002588 RID: 9608
		public Color[] MeshColors;

		// Token: 0x04002589 RID: 9609
		public int VertexCount;

		// Token: 0x0400258A RID: 9610
		public bool IsDarkMaterial;

		// Token: 0x0400258B RID: 9611
		public bool UsesAlpha;
	}
}
