using System;
using System.Collections.Generic;
using Oculus.Avatar;
using UnityEngine;

// Token: 0x020007BB RID: 1979
public class OvrAvatarSDKManager : MonoBehaviour
{
	// Token: 0x0600324D RID: 12877 RVA: 0x001064E5 File Offset: 0x001048E5
	public OvrAvatarSDKManager()
	{
	}

	// Token: 0x170005F0 RID: 1520
	// (get) Token: 0x0600324E RID: 12878 RVA: 0x001064F0 File Offset: 0x001048F0
	public static OvrAvatarSDKManager Instance
	{
		get
		{
			if (OvrAvatarSDKManager._instance == null)
			{
				OvrAvatarSDKManager._instance = UnityEngine.Object.FindObjectOfType<OvrAvatarSDKManager>();
				if (OvrAvatarSDKManager._instance == null)
				{
					GameObject gameObject = new GameObject("OvrAvatarSDKManager");
					OvrAvatarSDKManager._instance = gameObject.AddComponent<OvrAvatarSDKManager>();
					OvrAvatarSDKManager._instance.Initialize();
				}
			}
			return OvrAvatarSDKManager._instance;
		}
	}

	// Token: 0x0600324F RID: 12879 RVA: 0x0010654C File Offset: 0x0010494C
	private void Initialize()
	{
		string text = OvrAvatarSettings.AppID;
		if (text == string.Empty)
		{
			Debug.Log("No Oculus Rift App ID has been provided. Go to Oculus Avatar > Edit Configuration to supply one", OvrAvatarSettings.Instance);
			text = "0";
		}
		CAPI.ovrAvatar_Initialize(text);
		this.specificationCallbacks = new Dictionary<ulong, HashSet<specificationCallback>>();
		this.assetLoadedCallbacks = new Dictionary<ulong, HashSet<assetLoadedCallback>>();
		this.assetCache = new Dictionary<ulong, OvrAvatarAsset>();
	}

	// Token: 0x06003250 RID: 12880 RVA: 0x001065AB File Offset: 0x001049AB
	private void OnDestroy()
	{
		CAPI.ovrAvatar_Shutdown();
	}

	// Token: 0x06003251 RID: 12881 RVA: 0x001065B4 File Offset: 0x001049B4
	private void Update()
	{
		IntPtr intPtr = CAPI.ovrAvatarMessage_Pop();
		if (intPtr == IntPtr.Zero)
		{
			return;
		}
		ovrAvatarMessageType ovrAvatarMessageType = CAPI.ovrAvatarMessage_GetType(intPtr);
		if (ovrAvatarMessageType != ovrAvatarMessageType.AssetLoaded)
		{
			if (ovrAvatarMessageType != ovrAvatarMessageType.AvatarSpecification)
			{
				throw new NotImplementedException("Unhandled ovrAvatarMessageType: " + ovrAvatarMessageType);
			}
			ovrAvatarMessage_AvatarSpecification ovrAvatarMessage_AvatarSpecification = CAPI.ovrAvatarMessage_GetAvatarSpecification(intPtr);
			HashSet<specificationCallback> hashSet;
			if (this.specificationCallbacks.TryGetValue(ovrAvatarMessage_AvatarSpecification.oculusUserID, out hashSet))
			{
				foreach (specificationCallback specificationCallback in hashSet)
				{
					specificationCallback(ovrAvatarMessage_AvatarSpecification.avatarSpec);
				}
				this.specificationCallbacks.Remove(ovrAvatarMessage_AvatarSpecification.oculusUserID);
			}
			else
			{
				Debug.LogWarning("Error, got an avatar specification callback from a user id we don't have a record for: " + ovrAvatarMessage_AvatarSpecification.oculusUserID);
			}
		}
		else
		{
			ovrAvatarMessage_AssetLoaded ovrAvatarMessage_AssetLoaded = CAPI.ovrAvatarMessage_GetAssetLoaded(intPtr);
			IntPtr asset = ovrAvatarMessage_AssetLoaded.asset;
			ulong assetID = ovrAvatarMessage_AssetLoaded.assetID;
			ovrAvatarAssetType ovrAvatarAssetType = CAPI.ovrAvatarAsset_GetType(asset);
			switch (ovrAvatarAssetType)
			{
			case ovrAvatarAssetType.Mesh:
			{
				OvrAvatarAsset ovrAvatarAsset = new OvrAvatarAssetMesh(assetID, asset);
				goto IL_B6;
			}
			case ovrAvatarAssetType.Texture:
			{
				OvrAvatarAsset ovrAvatarAsset = new OvrAvatarAssetTexture(assetID, asset);
				goto IL_B6;
			}
			case ovrAvatarAssetType.Material:
			{
				OvrAvatarAsset ovrAvatarAsset = new OvrAvatarAssetMaterial(assetID, asset);
				goto IL_B6;
			}
			}
			throw new NotImplementedException(string.Format("Unsupported asset type format {0}", ovrAvatarAssetType.ToString()));
			IL_B6:
			HashSet<assetLoadedCallback> hashSet2;
			if (this.assetLoadedCallbacks.TryGetValue(ovrAvatarMessage_AssetLoaded.assetID, out hashSet2))
			{
				OvrAvatarAsset ovrAvatarAsset;
				this.assetCache.Add(assetID, ovrAvatarAsset);
				foreach (assetLoadedCallback assetLoadedCallback in hashSet2)
				{
					assetLoadedCallback(ovrAvatarAsset);
				}
				this.assetLoadedCallbacks.Remove(ovrAvatarMessage_AssetLoaded.assetID);
			}
			else
			{
				Debug.LogWarning("Loaded an asset with no owner: " + ovrAvatarMessage_AssetLoaded.assetID);
			}
		}
		CAPI.ovrAvatarMessage_Free(intPtr);
	}

	// Token: 0x06003252 RID: 12882 RVA: 0x001067EC File Offset: 0x00104BEC
	public void RequestAvatarSpecification(ulong userId, specificationCallback callback)
	{
		HashSet<specificationCallback> hashSet;
		if (!this.specificationCallbacks.TryGetValue(userId, out hashSet))
		{
			hashSet = new HashSet<specificationCallback>();
			this.specificationCallbacks.Add(userId, hashSet);
			IntPtr specificationRequest = CAPI.ovrAvatarSpecificationRequest_Create(userId);
			CAPI.ovrAvatar_RequestAvatarSpecificationFromSpecRequest(specificationRequest);
			CAPI.ovrAvatarSpecificationRequest_Destroy(specificationRequest);
		}
		hashSet.Add(callback);
	}

	// Token: 0x06003253 RID: 12883 RVA: 0x0010683C File Offset: 0x00104C3C
	public void BeginLoadingAsset(ulong assetId, assetLoadedCallback callback)
	{
		HashSet<assetLoadedCallback> hashSet;
		if (!this.assetLoadedCallbacks.TryGetValue(assetId, out hashSet))
		{
			hashSet = new HashSet<assetLoadedCallback>();
			this.assetLoadedCallbacks.Add(assetId, hashSet);
			CAPI.ovrAvatarAsset_BeginLoading(assetId);
		}
		if (hashSet.Add(callback))
		{
			hashSet.Add(callback);
		}
	}

	// Token: 0x06003254 RID: 12884 RVA: 0x0010688C File Offset: 0x00104C8C
	public OvrAvatarAsset GetAsset(ulong assetId)
	{
		OvrAvatarAsset result;
		if (this.assetCache.TryGetValue(assetId, out result))
		{
			return result;
		}
		return null;
	}

	// Token: 0x04002680 RID: 9856
	private static OvrAvatarSDKManager _instance;

	// Token: 0x04002681 RID: 9857
	private Dictionary<ulong, HashSet<specificationCallback>> specificationCallbacks;

	// Token: 0x04002682 RID: 9858
	private Dictionary<ulong, HashSet<assetLoadedCallback>> assetLoadedCallbacks;

	// Token: 0x04002683 RID: 9859
	private Dictionary<ulong, OvrAvatarAsset> assetCache;
}
