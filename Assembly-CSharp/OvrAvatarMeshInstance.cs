using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Token: 0x0200078E RID: 1934
public class OvrAvatarMeshInstance : MonoBehaviour
{
	// Token: 0x060031B6 RID: 12726 RVA: 0x001044C4 File Offset: 0x001028C4
	public OvrAvatarMeshInstance()
	{
	}

	// Token: 0x060031B7 RID: 12727 RVA: 0x001044CC File Offset: 0x001028CC
	public void AssetLoadedCallback(OvrAvatarAsset asset)
	{
		this.AssetsToLoad.Remove(asset.assetID);
		this.HandleAssetAvailable(asset);
		if (this.AssetsToLoad.Count <= 0)
		{
			this.UpdateMaterial();
		}
	}

	// Token: 0x060031B8 RID: 12728 RVA: 0x001044FE File Offset: 0x001028FE
	public void SetMeshAssets(ulong fadeTexture, ulong meshID, ulong materialID, ovrAvatarBodyPartType type)
	{
		this.MaterialID = materialID;
		this.MeshID = meshID;
		this.FadeTextureID = fadeTexture;
		this.MeshType = type;
		this.AssetsToLoad = new HashSet<ulong>();
		this.RequestAsset(meshID);
		this.RequestAsset(materialID);
		this.RequestAsset(fadeTexture);
	}

	// Token: 0x060031B9 RID: 12729 RVA: 0x00104540 File Offset: 0x00102940
	private void HandleAssetAvailable(OvrAvatarAsset asset)
	{
		if (asset.assetID == this.MeshID)
		{
			this.Mesh = base.gameObject.AddComponent<MeshFilter>();
			this.MeshInstance = base.gameObject.AddComponent<MeshRenderer>();
			this.MeshInstance.shadowCastingMode = ShadowCastingMode.Off;
			this.Mesh.sharedMesh = ((OvrAvatarAssetMesh)asset).mesh;
			Material material = new Material(Shader.Find("OvrAvatar/AvatarSurfaceShaderSelfOccluding"));
			this.MeshInstance.material = material;
		}
		if (asset.assetID == this.MaterialID)
		{
			this.MaterialState = ((OvrAvatarAssetMaterial)asset).material;
			this.MaterialState.alphaMaskTextureID = this.FadeTextureID;
			this.RequestMaterialTextures();
		}
	}

	// Token: 0x060031BA RID: 12730 RVA: 0x001045F7 File Offset: 0x001029F7
	public void ChangeMaterial(ulong assetID)
	{
		this.MaterialID = assetID;
		this.RequestAsset(this.MaterialID);
	}

	// Token: 0x060031BB RID: 12731 RVA: 0x0010460C File Offset: 0x00102A0C
	private void RequestAsset(ulong assetID)
	{
		if (assetID == 0UL)
		{
			return;
		}
		OvrAvatarAsset asset = OvrAvatarSDKManager.Instance.GetAsset(assetID);
		if (asset == null)
		{
			OvrAvatarSDKManager.Instance.BeginLoadingAsset(assetID, new assetLoadedCallback(this.AssetLoadedCallback));
			this.AssetsToLoad.Add(assetID);
		}
		else
		{
			this.HandleAssetAvailable(asset);
		}
	}

	// Token: 0x060031BC RID: 12732 RVA: 0x00104664 File Offset: 0x00102A64
	private void RequestMaterialTextures()
	{
		this.RequestAsset(this.MaterialState.normalMapTextureID);
		this.RequestAsset(this.MaterialState.parallaxMapTextureID);
		this.RequestAsset(this.MaterialState.roughnessMapTextureID);
		int num = 0;
		while ((long)num < (long)((ulong)this.MaterialState.layerCount))
		{
			this.RequestAsset(this.MaterialState.layers[num].sampleTexture);
			num++;
		}
	}

	// Token: 0x060031BD RID: 12733 RVA: 0x001046DE File Offset: 0x00102ADE
	public void SetActive(bool active)
	{
		base.gameObject.SetActive(active);
		if (active)
		{
			this.UpdateMaterial();
		}
	}

	// Token: 0x060031BE RID: 12734 RVA: 0x001046F8 File Offset: 0x00102AF8
	private void UpdateMaterial()
	{
		if (this.MeshInstance == null || this.MaterialID == 0UL)
		{
			return;
		}
		Material material = this.MeshInstance.material;
		ovrAvatarMaterialState materialState = this.MaterialState;
		material.SetColor("_BaseColor", materialState.baseColor);
		material.SetInt("_BaseMaskType", (int)materialState.baseMaskType);
		material.SetVector("_BaseMaskParameters", materialState.baseMaskParameters);
		material.SetVector("_BaseMaskAxis", materialState.baseMaskAxis);
		if (materialState.alphaMaskTextureID != 0UL)
		{
			material.SetTexture("_AlphaMask", OvrAvatarComponent.GetLoadedTexture(materialState.alphaMaskTextureID));
			material.SetTextureScale("_AlphaMask", new Vector2(materialState.alphaMaskScaleOffset.x, materialState.alphaMaskScaleOffset.y));
			material.SetTextureOffset("_AlphaMask", new Vector2(materialState.alphaMaskScaleOffset.z, materialState.alphaMaskScaleOffset.w));
		}
		if (materialState.normalMapTextureID != 0UL)
		{
			material.EnableKeyword("NORMAL_MAP_ON");
			material.SetTexture("_NormalMap", OvrAvatarComponent.GetLoadedTexture(materialState.normalMapTextureID));
			material.SetTextureScale("_NormalMap", new Vector2(materialState.normalMapScaleOffset.x, materialState.normalMapScaleOffset.y));
			material.SetTextureOffset("_NormalMap", new Vector2(materialState.normalMapScaleOffset.z, materialState.normalMapScaleOffset.w));
		}
		if (materialState.parallaxMapTextureID != 0UL)
		{
			material.SetTexture("_ParallaxMap", OvrAvatarComponent.GetLoadedTexture(materialState.parallaxMapTextureID));
			material.SetTextureScale("_ParallaxMap", new Vector2(materialState.parallaxMapScaleOffset.x, materialState.parallaxMapScaleOffset.y));
			material.SetTextureOffset("_ParallaxMap", new Vector2(materialState.parallaxMapScaleOffset.z, materialState.parallaxMapScaleOffset.w));
		}
		if (materialState.roughnessMapTextureID != 0UL)
		{
			material.EnableKeyword("ROUGHNESS_ON");
			material.SetTexture("_RoughnessMap", OvrAvatarComponent.GetLoadedTexture(materialState.roughnessMapTextureID));
			material.SetTextureScale("_RoughnessMap", new Vector2(materialState.roughnessMapScaleOffset.x, materialState.roughnessMapScaleOffset.y));
			material.SetTextureOffset("_RoughnessMap", new Vector2(materialState.roughnessMapScaleOffset.z, materialState.roughnessMapScaleOffset.w));
		}
		material.EnableKeyword(OvrAvatarComponent.LayerKeywords[(int)((UIntPtr)materialState.layerCount)]);
		for (ulong num = 0UL; num < (ulong)materialState.layerCount; num += 1UL)
		{
			checked
			{
				ovrAvatarMaterialLayerState ovrAvatarMaterialLayerState = materialState.layers[(int)((IntPtr)num)];
				material.SetInt(OvrAvatarComponent.LayerSampleModeParameters[(int)((IntPtr)num)], (int)ovrAvatarMaterialLayerState.sampleMode);
				material.SetInt(OvrAvatarComponent.LayerBlendModeParameters[(int)((IntPtr)num)], (int)ovrAvatarMaterialLayerState.blendMode);
				material.SetInt(OvrAvatarComponent.LayerMaskTypeParameters[(int)((IntPtr)num)], (int)ovrAvatarMaterialLayerState.maskType);
				material.SetColor(OvrAvatarComponent.LayerColorParameters[(int)((IntPtr)num)], ovrAvatarMaterialLayerState.layerColor);
				if (ovrAvatarMaterialLayerState.sampleMode != ovrAvatarMaterialLayerSampleMode.Color)
				{
					string name = OvrAvatarComponent.LayerSurfaceParameters[(int)((IntPtr)num)];
					material.SetTexture(name, OvrAvatarComponent.GetLoadedTexture(ovrAvatarMaterialLayerState.sampleTexture));
					material.SetTextureScale(name, new Vector2(ovrAvatarMaterialLayerState.sampleScaleOffset.x, ovrAvatarMaterialLayerState.sampleScaleOffset.y));
					material.SetTextureOffset(name, new Vector2(ovrAvatarMaterialLayerState.sampleScaleOffset.z, ovrAvatarMaterialLayerState.sampleScaleOffset.w));
				}
				if (ovrAvatarMaterialLayerState.sampleMode == ovrAvatarMaterialLayerSampleMode.Parallax)
				{
					material.EnableKeyword("PARALLAX_ON");
				}
				material.SetColor(OvrAvatarComponent.LayerSampleParametersParameters[(int)((IntPtr)num)], ovrAvatarMaterialLayerState.sampleParameters);
				material.SetColor(OvrAvatarComponent.LayerMaskParametersParameters[(int)((IntPtr)num)], ovrAvatarMaterialLayerState.maskParameters);
				material.SetColor(OvrAvatarComponent.LayerMaskAxisParameters[(int)((IntPtr)num)], ovrAvatarMaterialLayerState.maskAxis);
			}
		}
	}

	// Token: 0x040025A2 RID: 9634
	private HashSet<ulong> AssetsToLoad;

	// Token: 0x040025A3 RID: 9635
	public ulong MeshID;

	// Token: 0x040025A4 RID: 9636
	private ulong MaterialID;

	// Token: 0x040025A5 RID: 9637
	private ulong FadeTextureID;

	// Token: 0x040025A6 RID: 9638
	public ovrAvatarBodyPartType MeshType;

	// Token: 0x040025A7 RID: 9639
	public ovrAvatarMaterialState MaterialState;

	// Token: 0x040025A8 RID: 9640
	private MeshFilter Mesh;

	// Token: 0x040025A9 RID: 9641
	private MeshRenderer MeshInstance;
}
