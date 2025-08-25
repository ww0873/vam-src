using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using DynamicCSharp;
using MeshVR;
using MVR.FileManagement;
using MVR.FileManagementSecure;
using UnityEngine;

// Token: 0x02000C0A RID: 3082
public class CustomUnityAssetLoader : JSONStorable
{
	// Token: 0x0600599C RID: 22940 RVA: 0x0020F4EE File Offset: 0x0020D8EE
	public CustomUnityAssetLoader()
	{
	}

	// Token: 0x17000D3B RID: 3387
	// (get) Token: 0x0600599D RID: 22941 RVA: 0x0020F4F6 File Offset: 0x0020D8F6
	// (set) Token: 0x0600599E RID: 22942 RVA: 0x0020F500 File Offset: 0x0020D900
	protected bool isLoading
	{
		get
		{
			return this._isLoading;
		}
		set
		{
			if (this._isLoading != value)
			{
				this._isLoading = value;
				if (this._isLoading)
				{
					if (this.isLoadingFlag != null)
					{
						this.isLoadingFlag.Raise();
					}
					this.isLoadingFlag = new AsyncFlag("CustomUnityAssetLoader");
					if (SuperController.singleton != null)
					{
						SuperController.singleton.HoldLoadComplete(this.isLoadingFlag);
					}
				}
				else if (this.isLoadingFlag != null)
				{
					this.isLoadingFlag.Raise();
					this.isLoadingFlag = null;
				}
				if (this.loadingIndicator != null)
				{
					this.loadingIndicator.gameObject.SetActive(this._isLoading);
				}
				if (this.loadingIndicatorAlt != null)
				{
					this.loadingIndicatorAlt.gameObject.SetActive(this._isLoading);
				}
			}
		}
	}

	// Token: 0x17000D3C RID: 3388
	// (get) Token: 0x0600599F RID: 22943 RVA: 0x0020F5E1 File Offset: 0x0020D9E1
	// (set) Token: 0x060059A0 RID: 22944 RVA: 0x0020F5E9 File Offset: 0x0020D9E9
	public bool isAssetLoaded
	{
		[CompilerGenerated]
		get
		{
			return this.<isAssetLoaded>k__BackingField;
		}
		[CompilerGenerated]
		protected set
		{
			this.<isAssetLoaded>k__BackingField = value;
		}
	}

	// Token: 0x060059A1 RID: 22945 RVA: 0x0020F5F4 File Offset: 0x0020D9F4
	protected void SceneRequestCallback(AssetLoader.SceneLoadIntoTransformRequest slr)
	{
		this.lightmapData = slr.lightmapData;
		this.lightProbesHolder = slr.lightProbesHolder;
		this.canvases = slr.transform.GetComponentsInChildren<Canvas>();
		if (this.registerCanvasesJSON.val)
		{
			foreach (Canvas canvas in this.canvases)
			{
				SuperController.singleton.AddCanvas(canvas);
				canvas.gameObject.SetActive(this.showCanvasesJSON.val);
			}
		}
		this.meshRenderers = new List<MeshRenderer>();
		MeshRenderer[] componentsInChildren = slr.transform.GetComponentsInChildren<MeshRenderer>();
		foreach (MeshRenderer meshRenderer in componentsInChildren)
		{
			if (meshRenderer.enabled)
			{
				this.meshRenderers.Add(meshRenderer);
				this.containingAtom.RegisterDynamicMeshRenderer(meshRenderer);
			}
		}
		this.isLoading = false;
		this.isAssetLoaded = true;
		this.sceneRequest = null;
		this.DoLoadedCallbacks();
	}

	// Token: 0x060059A2 RID: 22946 RVA: 0x0020F6F5 File Offset: 0x0020DAF5
	protected void SyncAssetSelectionChoices()
	{
		if (this.assetNameJSON != null && this.combinedAssetNames != null)
		{
			this.assetNameJSON.choices = this.combinedAssetNames;
		}
	}

	// Token: 0x060059A3 RID: 22947 RVA: 0x0020F720 File Offset: 0x0020DB20
	protected void BundleRequestCallback(AssetLoader.AssetBundleFromFileRequest abffr)
	{
		this.assetBundle = abffr.assetBundle;
		this.assetBundleUrl = abffr.path;
		this.isLoading = false;
		if (this.assetBundle != null)
		{
			this.combinedAssetNames = new List<string>();
			this.combinedAssetNames.Add("None");
			string[] allScenePaths = this.assetBundle.GetAllScenePaths();
			foreach (string text in allScenePaths)
			{
				if (!this.assetBundleScenePaths.ContainsKey(text))
				{
					this.combinedAssetNames.Add(text);
					this.assetBundleScenePaths.Add(text, true);
				}
			}
			string[] allAssetNames = this.assetBundle.GetAllAssetNames();
			foreach (string text2 in allAssetNames)
			{
				if (!this.assetBundlePrefabNames.ContainsKey(text2) && text2.EndsWith(".prefab"))
				{
					this.combinedAssetNames.Add(text2);
					this.assetBundlePrefabNames.Add(text2, true);
				}
			}
			this.SyncAssetSelectionChoices();
			if (this.assetNameJSON != null)
			{
				this.SyncAssetName(this.assetNameJSON.val);
			}
		}
	}

	// Token: 0x060059A4 RID: 22948 RVA: 0x0020F85C File Offset: 0x0020DC5C
	protected void SyncAssetUrl(string url)
	{
		if (SuperController.singleton != null)
		{
			if (this.isLoading)
			{
				SuperController.LogError("Attempt to load new assetbundle " + url + " while another is already being loaded");
				this.assetUrlJSON.valNoCallback = this.bundleRequest.path;
			}
			else
			{
				this.ClearAssetBundle();
				if (!this.insideRestore)
				{
					this.assetNameJSON.val = "None";
				}
				if (url != null && url != string.Empty)
				{
					this.isLoading = true;
					this.isLoadingFlag.Name = "CustomUnityAssetLoader: " + url;
					if (this.loadDllJSON.val)
					{
						string text = url.Replace(".scene", string.Empty);
						text = text.Replace(".assetbundle", string.Empty);
						string text2 = text + ".dll";
						if (FileManager.FileExists(text2, false, false))
						{
							if (UserPreferences.singleton == null || UserPreferences.singleton.enablePlugins)
							{
								this.assetDllUrlJSON.valNoCallback = text2;
								if (this.domain == null)
								{
									this.domain = ScriptDomain.CreateDomain("MVRAssets", true);
								}
								if (FileManager.IsFileInPackage(text2))
								{
									this.domain.LoadAssembly(FileManager.ReadAllBytes(text2, false), null);
								}
								else
								{
									this.domain.LoadAssembly(text2);
								}
							}
							else
							{
								SuperController.LogError("Attempted to load dll when plugins option is disabled. To enable, see User Preferences -> Security tab");
								SuperController.singleton.ShowMainHUDAuto();
								SuperController.singleton.SetActiveUI("MainMenu");
								SuperController.singleton.SetMainMenuTab("TabUserPrefs");
								SuperController.singleton.SetUserPrefsTab("TabSecurity");
							}
						}
						else
						{
							this.assetDllUrlJSON.valNoCallback = string.Empty;
						}
					}
					this.bundleRequest = new AssetLoader.AssetBundleFromFileRequest
					{
						path = url,
						callback = new AssetLoader.LoadBundleFromFileCallback(this.BundleRequestCallback)
					};
					AssetLoader.QueueLoadAssetBundleFromFile(this.bundleRequest);
				}
			}
		}
	}

	// Token: 0x060059A5 RID: 22949 RVA: 0x0020FA57 File Offset: 0x0020DE57
	public void RegisterAssetClearedCallback(CustomUnityAssetLoader.AssetClearedCallback callback)
	{
		this.assetClearedCallbacks.Add(callback);
	}

	// Token: 0x060059A6 RID: 22950 RVA: 0x0020FA66 File Offset: 0x0020DE66
	public void DeregisterAssetClearedCallback(CustomUnityAssetLoader.AssetClearedCallback callback)
	{
		this.assetClearedCallbacks.Remove(callback);
	}

	// Token: 0x060059A7 RID: 22951 RVA: 0x0020FA78 File Offset: 0x0020DE78
	protected void DoClearedCallbacks()
	{
		if (this.assetClearedCallbacks != null)
		{
			foreach (CustomUnityAssetLoader.AssetClearedCallback assetClearedCallback in this.assetClearedCallbacks)
			{
				if (assetClearedCallback != null)
				{
					assetClearedCallback();
				}
			}
		}
	}

	// Token: 0x060059A8 RID: 22952 RVA: 0x0020FAE4 File Offset: 0x0020DEE4
	public void RegisterAssetLoadedCallback(CustomUnityAssetLoader.AssetLoadedCallback callback)
	{
		this.assetLoadedCallbacks.Add(callback);
	}

	// Token: 0x060059A9 RID: 22953 RVA: 0x0020FAF3 File Offset: 0x0020DEF3
	public void DeregisterAssetLoadedCallback(CustomUnityAssetLoader.AssetLoadedCallback callback)
	{
		this.assetLoadedCallbacks.Remove(callback);
	}

	// Token: 0x060059AA RID: 22954 RVA: 0x0020FB04 File Offset: 0x0020DF04
	protected void DoLoadedCallbacks()
	{
		if (this.assetLoadedCallbacks != null)
		{
			foreach (CustomUnityAssetLoader.AssetLoadedCallback assetLoadedCallback in this.assetLoadedCallbacks)
			{
				if (assetLoadedCallback != null)
				{
					assetLoadedCallback();
				}
			}
		}
	}

	// Token: 0x060059AB RID: 22955 RVA: 0x0020FB70 File Offset: 0x0020DF70
	protected void SyncAssetName(string assetName)
	{
		this.RemoveData();
		if (this.assetBundle != null)
		{
			if (this.sceneRequest != null)
			{
				this.sceneRequest.requestCancelled = true;
				this.sceneRequest = null;
				this.isLoading = false;
			}
			bool flag;
			if (this.assetBundleScenePaths.TryGetValue(assetName, out flag))
			{
				this.isLoading = true;
				this.isLoadingFlag.Name = "CustomUnityAssetLoader: " + assetName;
				this.sceneRequest = new AssetLoader.SceneLoadIntoTransformRequest
				{
					scenePath = assetName,
					transform = base.transform,
					importLightmaps = this.importLightmapsJSON.val,
					importLightProbes = this.importLightProbesJSON.val,
					callback = new AssetLoader.LoadSceneIntoTransformCallback(this.SceneRequestCallback)
				};
				AssetLoader.QueueLoadSceneIntoTransform(this.sceneRequest);
			}
			else if (this.assetBundlePrefabNames.TryGetValue(assetName, out flag))
			{
				this.isLoading = true;
				this.isLoadingFlag.Name = "CustomUnityAssetLoader: " + assetName;
				GameObject gameObject = this.assetBundle.LoadAsset<GameObject>(assetName);
				if (gameObject != null)
				{
					Transform transform = UnityEngine.Object.Instantiate<Transform>(gameObject.transform);
					Vector3 localPosition = transform.localPosition;
					Quaternion localRotation = transform.localRotation;
					Vector3 localScale = transform.localScale;
					transform.SetParent(base.transform);
					transform.localPosition = localPosition;
					transform.localRotation = localRotation;
					transform.localScale = localScale;
					this.canvases = transform.GetComponentsInChildren<Canvas>();
					if (this.registerCanvasesJSON.val)
					{
						foreach (Canvas canvas in this.canvases)
						{
							SuperController.singleton.AddCanvas(canvas);
							canvas.gameObject.SetActive(this.showCanvasesJSON.val);
						}
					}
					this.meshRenderers = new List<MeshRenderer>();
					MeshRenderer[] componentsInChildren = transform.GetComponentsInChildren<MeshRenderer>();
					foreach (MeshRenderer meshRenderer in componentsInChildren)
					{
						if (meshRenderer.enabled)
						{
							this.meshRenderers.Add(meshRenderer);
							this.containingAtom.RegisterDynamicMeshRenderer(meshRenderer);
						}
					}
					this.isAssetLoaded = true;
					this.DoLoadedCallbacks();
				}
				this.isLoading = false;
			}
		}
	}

	// Token: 0x060059AC RID: 22956 RVA: 0x0020FDB8 File Offset: 0x0020E1B8
	protected void RemoveData()
	{
		if (this.lightmapData != null)
		{
			if (GlobalLightingManager.singleton != null)
			{
				GlobalLightingManager.singleton.RemoveLightmapData(this.lightmapData);
			}
			this.lightmapData = null;
		}
		if (this.lightProbesHolder != null)
		{
			if (GlobalLightingManager.singleton != null)
			{
				GlobalLightingManager.singleton.RemoveLightProbesHolder(this.lightProbesHolder);
			}
			this.lightProbesHolder = null;
		}
		if (this.canvases != null && this.registerCanvasesJSON.val)
		{
			foreach (Canvas c in this.canvases)
			{
				SuperController.singleton.RemoveCanvas(c);
			}
		}
		if (this.meshRenderers != null)
		{
			foreach (MeshRenderer mr in this.meshRenderers)
			{
				this.containingAtom.DeregisterDynamicMeshRenderer(mr);
			}
		}
		List<GameObject> list = new List<GameObject>();
		IEnumerator enumerator2 = base.transform.GetEnumerator();
		try
		{
			while (enumerator2.MoveNext())
			{
				object obj = enumerator2.Current;
				Transform transform = (Transform)obj;
				list.Add(transform.gameObject);
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator2 as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
		foreach (GameObject obj2 in list)
		{
			UnityEngine.Object.Destroy(obj2);
		}
		if (this.isAssetLoaded)
		{
			this.isAssetLoaded = false;
			this.DoClearedCallbacks();
		}
	}

	// Token: 0x060059AD RID: 22957 RVA: 0x0020FF98 File Offset: 0x0020E398
	protected void ClearAssetBundle()
	{
		if (this.assetBundleScenePaths != null)
		{
			this.assetBundleScenePaths.Clear();
		}
		if (this.assetBundlePrefabNames != null)
		{
			this.assetBundlePrefabNames.Clear();
		}
		if (this.assetBundle != null)
		{
			AssetLoader.DoneWithAssetBundleFromFile(this.assetBundleUrl);
			this.assetBundle = null;
			this.assetBundleUrl = null;
		}
		this.combinedAssetNames = new List<string>();
		this.combinedAssetNames.Add("None");
		this.SyncAssetSelectionChoices();
	}

	// Token: 0x060059AE RID: 22958 RVA: 0x0021001C File Offset: 0x0020E41C
	protected void SyncImportLightmaps(bool b)
	{
		if (this.lightmapData != null && GlobalLightingManager.singleton != null)
		{
			if (b)
			{
				GlobalLightingManager.singleton.PushLightmapData(this.lightmapData);
			}
			else
			{
				GlobalLightingManager.singleton.RemoveLightmapData(this.lightmapData);
			}
		}
	}

	// Token: 0x060059AF RID: 22959 RVA: 0x00210070 File Offset: 0x0020E470
	protected void SyncImportLightProbes(bool b)
	{
		if (this.lightProbesHolder != null && GlobalLightingManager.singleton != null)
		{
			if (b)
			{
				GlobalLightingManager.singleton.PushLightProbesHolder(this.lightProbesHolder);
			}
			else
			{
				GlobalLightingManager.singleton.RemoveLightProbesHolder(this.lightProbesHolder);
			}
		}
	}

	// Token: 0x060059B0 RID: 22960 RVA: 0x002100C4 File Offset: 0x0020E4C4
	protected void BeginBrowse(JSONStorableUrl jsurl)
	{
		List<ShortCut> shortCutsForDirectory = FileManager.GetShortCutsForDirectory("Custom/Assets", true, true, true, true);
		shortCutsForDirectory.Insert(0, new ShortCut
		{
			displayName = "Root",
			path = Path.GetFullPath(".")
		});
		jsurl.shortCuts = shortCutsForDirectory;
	}

	// Token: 0x060059B1 RID: 22961 RVA: 0x00210110 File Offset: 0x0020E510
	protected void Init()
	{
		this.assetClearedCallbacks = new HashSet<CustomUnityAssetLoader.AssetClearedCallback>();
		this.assetLoadedCallbacks = new HashSet<CustomUnityAssetLoader.AssetLoadedCallback>();
		this.assetUrlJSON = new JSONStorableUrl("assetUrl", string.Empty, new JSONStorableString.SetStringCallback(this.SyncAssetUrl), "assetbundle|scene", "Custom\\Assets");
		this.assetUrlJSON.allowFullComputerBrowse = true;
		this.assetUrlJSON.allowBrowseAboveSuggestedPath = true;
		this.assetUrlJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
		this.assetUrlJSON.suggestedPathGroup = "UnityAsset";
		base.RegisterUrl(this.assetUrlJSON);
		this.assetDllUrlJSON = new JSONStorableUrl("assetDllUrl", string.Empty, "dll", "Custom\\Assets");
		this.assetDllUrlJSON.allowFullComputerBrowse = true;
		this.assetDllUrlJSON.allowBrowseAboveSuggestedPath = true;
		this.assetDllUrlJSON.beginBrowseWithObjectCallback = new JSONStorableUrl.BeginBrowseWithObjectCallback(this.BeginBrowse);
		this.assetDllUrlJSON.suggestedPathGroup = "UnityAsset";
		base.RegisterUrl(this.assetDllUrlJSON);
		this.assetNameJSON = new JSONStorableStringChooser("assetName", new List<string>
		{
			"None"
		}, "None", "Asset", new JSONStorableStringChooser.SetStringCallback(this.SyncAssetName));
		base.RegisterStringChooser(this.assetNameJSON);
		this.importLightmapsJSON = new JSONStorableBool("importLightmaps", true, new JSONStorableBool.SetBoolCallback(this.SyncImportLightmaps));
		base.RegisterBool(this.importLightmapsJSON);
		this.importLightProbesJSON = new JSONStorableBool("importLightProbes", true, new JSONStorableBool.SetBoolCallback(this.SyncImportLightProbes));
		base.RegisterBool(this.importLightProbesJSON);
		this.registerCanvasesJSON = new JSONStorableBool("registerCanvases", true, new JSONStorableBool.SetBoolCallback(this.SyncRegisterCanvases));
		base.RegisterBool(this.registerCanvasesJSON);
		this.showCanvasesJSON = new JSONStorableBool("showCanvases", true, new JSONStorableBool.SetBoolCallback(this.SyncShowCanvases));
		base.RegisterBool(this.showCanvasesJSON);
		this.loadDllJSON = new JSONStorableBool("loadDll", true, new JSONStorableBool.SetBoolCallback(this.SyncLoadDll));
		base.RegisterBool(this.loadDllJSON);
		this.assetBundleScenePaths = new Dictionary<string, bool>();
		this.assetBundlePrefabNames = new Dictionary<string, bool>();
		this.combinedAssetNames = new List<string>();
		this.combinedAssetNames.Add("None");
	}

	// Token: 0x060059B2 RID: 22962 RVA: 0x00210358 File Offset: 0x0020E758
	protected void SyncRegisterCanvases(bool b)
	{
		if (this.canvases != null)
		{
			foreach (Canvas c in this.canvases)
			{
				if (b)
				{
					SuperController.singleton.AddCanvas(c);
				}
				else
				{
					SuperController.singleton.RemoveCanvas(c);
				}
			}
		}
	}

	// Token: 0x060059B3 RID: 22963 RVA: 0x002103B0 File Offset: 0x0020E7B0
	protected void SyncShowCanvases(bool b)
	{
		if (this.canvases != null)
		{
			foreach (Canvas canvas in this.canvases)
			{
				canvas.gameObject.SetActive(b);
			}
		}
	}

	// Token: 0x060059B4 RID: 22964 RVA: 0x002103F3 File Offset: 0x0020E7F3
	protected void SyncLoadDll(bool b)
	{
	}

	// Token: 0x060059B5 RID: 22965 RVA: 0x002103F8 File Offset: 0x0020E7F8
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			CustomUnityAssetLoaderUI componentInChildren = this.UITransform.GetComponentInChildren<CustomUnityAssetLoaderUI>(true);
			if (componentInChildren != null)
			{
				this.assetUrlJSON.fileBrowseButton = componentInChildren.fileBrowseButton;
				this.assetUrlJSON.clearButton = componentInChildren.clearButton;
				this.assetUrlJSON.text = componentInChildren.urlText;
				this.importLightmapsJSON.toggle = componentInChildren.importLightmapsToggle;
				this.importLightProbesJSON.toggle = componentInChildren.importLightProbesToggle;
				this.registerCanvasesJSON.toggle = componentInChildren.registerCanvasesToggle;
				this.showCanvasesJSON.toggle = componentInChildren.showCanvasesToggle;
				this.loadDllJSON.toggle = componentInChildren.loadDllToggle;
				this.assetNameJSON.popup = componentInChildren.assetSelectionPopup;
				this.loadingIndicator = componentInChildren.loadingIndicator;
			}
		}
	}

	// Token: 0x060059B6 RID: 22966 RVA: 0x002104D4 File Offset: 0x0020E8D4
	public override void InitUIAlt()
	{
		if (this.UITransformAlt != null)
		{
			CustomUnityAssetLoaderUI componentInChildren = this.UITransformAlt.GetComponentInChildren<CustomUnityAssetLoaderUI>(true);
			if (componentInChildren != null)
			{
				this.assetUrlJSON.fileBrowseButtonAlt = componentInChildren.fileBrowseButton;
				this.assetUrlJSON.clearButtonAlt = componentInChildren.clearButton;
				this.assetUrlJSON.textAlt = componentInChildren.urlText;
				this.importLightmapsJSON.toggleAlt = componentInChildren.importLightmapsToggle;
				this.importLightProbesJSON.toggleAlt = componentInChildren.importLightProbesToggle;
				this.registerCanvasesJSON.toggleAlt = componentInChildren.registerCanvasesToggle;
				this.showCanvasesJSON.toggleAlt = componentInChildren.showCanvasesToggle;
				this.loadDllJSON.toggleAlt = componentInChildren.loadDllToggle;
				this.assetNameJSON.popupAlt = componentInChildren.assetSelectionPopup;
				this.loadingIndicatorAlt = componentInChildren.loadingIndicator;
			}
		}
	}

	// Token: 0x060059B7 RID: 22967 RVA: 0x002105B0 File Offset: 0x0020E9B0
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			this.Init();
			this.InitUI();
			this.InitUIAlt();
		}
	}

	// Token: 0x060059B8 RID: 22968 RVA: 0x002105D5 File Offset: 0x0020E9D5
	public override void Remove()
	{
		this.RemoveData();
		this.ClearAssetBundle();
	}

	// Token: 0x040049B9 RID: 18873
	public RectTransform loadingIndicator;

	// Token: 0x040049BA RID: 18874
	public RectTransform loadingIndicatorAlt;

	// Token: 0x040049BB RID: 18875
	protected AsyncFlag isLoadingFlag;

	// Token: 0x040049BC RID: 18876
	protected bool _isLoading;

	// Token: 0x040049BD RID: 18877
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private bool <isAssetLoaded>k__BackingField;

	// Token: 0x040049BE RID: 18878
	protected AsyncOperation async;

	// Token: 0x040049BF RID: 18879
	protected LightmapData[] lightmapData;

	// Token: 0x040049C0 RID: 18880
	protected GlobalLightingManager.LightProbesHolder lightProbesHolder;

	// Token: 0x040049C1 RID: 18881
	protected AssetBundle assetBundle;

	// Token: 0x040049C2 RID: 18882
	protected string assetBundleUrl;

	// Token: 0x040049C3 RID: 18883
	protected AssetLoader.AssetBundleFromFileRequest bundleRequest;

	// Token: 0x040049C4 RID: 18884
	protected AssetLoader.SceneLoadIntoTransformRequest sceneRequest;

	// Token: 0x040049C5 RID: 18885
	protected Dictionary<string, bool> assetBundleScenePaths;

	// Token: 0x040049C6 RID: 18886
	protected Dictionary<string, bool> assetBundlePrefabNames;

	// Token: 0x040049C7 RID: 18887
	protected List<string> combinedAssetNames;

	// Token: 0x040049C8 RID: 18888
	protected Canvas[] canvases;

	// Token: 0x040049C9 RID: 18889
	protected List<MeshRenderer> meshRenderers;

	// Token: 0x040049CA RID: 18890
	protected ScriptDomain domain;

	// Token: 0x040049CB RID: 18891
	protected JSONStorableUrl assetUrlJSON;

	// Token: 0x040049CC RID: 18892
	protected JSONStorableUrl assetDllUrlJSON;

	// Token: 0x040049CD RID: 18893
	protected HashSet<CustomUnityAssetLoader.AssetClearedCallback> assetClearedCallbacks;

	// Token: 0x040049CE RID: 18894
	protected HashSet<CustomUnityAssetLoader.AssetLoadedCallback> assetLoadedCallbacks;

	// Token: 0x040049CF RID: 18895
	protected JSONStorableStringChooser assetNameJSON;

	// Token: 0x040049D0 RID: 18896
	protected JSONStorableBool importLightmapsJSON;

	// Token: 0x040049D1 RID: 18897
	protected JSONStorableBool importLightProbesJSON;

	// Token: 0x040049D2 RID: 18898
	protected JSONStorableBool registerCanvasesJSON;

	// Token: 0x040049D3 RID: 18899
	protected JSONStorableBool showCanvasesJSON;

	// Token: 0x040049D4 RID: 18900
	protected JSONStorableBool loadDllJSON;

	// Token: 0x02000C0B RID: 3083
	// (Invoke) Token: 0x060059BA RID: 22970
	public delegate void AssetClearedCallback();

	// Token: 0x02000C0C RID: 3084
	// (Invoke) Token: 0x060059BE RID: 22974
	public delegate void AssetLoadedCallback();
}
