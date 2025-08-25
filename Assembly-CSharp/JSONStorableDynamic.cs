using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using AssetBundles;
using UnityEngine;
using UnityEngine.SceneManagement;

// Token: 0x02000C24 RID: 3108
public class JSONStorableDynamic : MonoBehaviour
{
	// Token: 0x06005A57 RID: 23127 RVA: 0x0014F433 File Offset: 0x0014D833
	public JSONStorableDynamic()
	{
	}

	// Token: 0x17000D55 RID: 3413
	// (get) Token: 0x06005A58 RID: 23128 RVA: 0x0014F43B File Offset: 0x0014D83B
	// (set) Token: 0x06005A59 RID: 23129 RVA: 0x0014F443 File Offset: 0x0014D843
	public bool ready
	{
		[CompilerGenerated]
		get
		{
			return this.<ready>k__BackingField;
		}
		[CompilerGenerated]
		protected set
		{
			this.<ready>k__BackingField = value;
		}
	}

	// Token: 0x06005A5A RID: 23130 RVA: 0x0014F44C File Offset: 0x0014D84C
	protected virtual void InitInstance()
	{
		this.instance.parent = base.transform;
		this.instance.localPosition = Vector3.zero;
		this.instance.localRotation = Quaternion.identity;
		this.instance.localScale = Vector3.one;
		UIConnectorMaster[] componentsInChildren = this.instance.GetComponentsInChildren<UIConnectorMaster>(true);
		foreach (UIConnectorMaster uiconnectorMaster in componentsInChildren)
		{
			uiconnectorMaster.containingAtom = this.containingAtom;
		}
	}

	// Token: 0x06005A5B RID: 23131 RVA: 0x0014F4D0 File Offset: 0x0014D8D0
	protected void RegisterDynamicObjs()
	{
		if (this.containingAtom != null && this.instance != null)
		{
			this.isRegistered = true;
			if (this.jss == null)
			{
				this.jss = this.instance.GetComponentsInChildren<JSONStorable>(true);
			}
			foreach (JSONStorable jsonstorable in this.jss)
			{
				if (!jsonstorable.exclude)
				{
					this.containingAtom.RegisterAdditionalStorable(jsonstorable);
				}
			}
			if (this.cvs == null)
			{
				this.cvs = this.instance.GetComponentsInChildren<Canvas>();
			}
			foreach (Canvas c in this.cvs)
			{
				this.containingAtom.AddCanvas(c);
			}
			if (this.pss == null)
			{
				this.pss = this.instance.GetComponentsInChildren<PhysicsSimulator>(true);
			}
			foreach (PhysicsSimulator ps in this.pss)
			{
				this.containingAtom.RegisterDynamicPhysicsSimulator(ps);
			}
			if (this.psjs == null)
			{
				this.psjs = this.instance.GetComponentsInChildren<PhysicsSimulatorJSONStorable>(true);
			}
			foreach (PhysicsSimulatorJSONStorable ps2 in this.psjs)
			{
				this.containingAtom.RegisterDynamicPhysicsSimulatorJSONStorable(ps2);
			}
			if (this.scrs == null)
			{
				this.scrs = this.instance.GetComponentsInChildren<ScaleChangeReceiver>(true);
			}
			foreach (ScaleChangeReceiver scr in this.scrs)
			{
				this.containingAtom.RegisterDynamicScaleChangeReceiver(scr);
			}
			if (this.scrjss == null)
			{
				this.scrjss = this.instance.GetComponentsInChildren<ScaleChangeReceiverJSONStorable>(true);
			}
			foreach (ScaleChangeReceiverJSONStorable scr2 in this.scrjss)
			{
				this.containingAtom.RegisterDynamicScaleChangeReceiverJSONStorable(scr2);
			}
			if (this.rss == null)
			{
				this.rss = this.instance.GetComponentsInChildren<RenderSuspend>(true);
			}
			foreach (RenderSuspend rs in this.rss)
			{
				this.containingAtom.RegisterDynamicRenderSuspend(rs);
			}
		}
	}

	// Token: 0x06005A5C RID: 23132 RVA: 0x0014F744 File Offset: 0x0014DB44
	protected void UnregisterDynamicObjs()
	{
		if (this.containingAtom != null && this.instance != null)
		{
			this.isRegistered = false;
			if (this.jss != null)
			{
				foreach (JSONStorable jsonstorable in this.jss)
				{
					if (!jsonstorable.exclude)
					{
						this.containingAtom.UnregisterAdditionalStorable(jsonstorable);
					}
				}
			}
			if (this.cvs != null)
			{
				foreach (Canvas c in this.cvs)
				{
					this.containingAtom.RemoveCanvas(c);
				}
			}
			if (this.pss != null)
			{
				foreach (PhysicsSimulator ps in this.pss)
				{
					this.containingAtom.DeregisterDynamicPhysicsSimulator(ps);
				}
			}
			if (this.psjs != null)
			{
				foreach (PhysicsSimulatorJSONStorable ps2 in this.psjs)
				{
					this.containingAtom.DeregisterDynamicPhysicsSimulatorJSONStorable(ps2);
				}
			}
			if (this.scrs != null)
			{
				foreach (ScaleChangeReceiver scr in this.scrs)
				{
					this.containingAtom.DeregisterDynamicScaleChangeReceiver(scr);
				}
			}
			if (this.scrjss != null)
			{
				foreach (ScaleChangeReceiverJSONStorable scr2 in this.scrjss)
				{
					this.containingAtom.DeregisterDynamicScaleChangeReceiverJSONStorable(scr2);
				}
			}
			if (this.rss != null)
			{
				foreach (RenderSuspend rs in this.rss)
				{
					this.containingAtom.DeregisterDynamicRenderSuspend(rs);
				}
			}
		}
	}

	// Token: 0x06005A5D RID: 23133 RVA: 0x0014F93C File Offset: 0x0014DD3C
	public void ResetUnregisteredInstance()
	{
		if (this.instance != null && !this.isRegistered)
		{
			JSONStorable[] componentsInChildren = this.instance.GetComponentsInChildren<JSONStorable>(true);
			foreach (JSONStorable jsonstorable in componentsInChildren)
			{
				jsonstorable.RestoreAllFromDefaults();
			}
		}
	}

	// Token: 0x06005A5E RID: 23134 RVA: 0x0014F994 File Offset: 0x0014DD94
	public void PostLoadJSONRestore()
	{
		if (this.containingAtom != null && this.instance != null && this.needsPostLoadJSONRestore)
		{
			JSONStorable[] componentsInChildren = this.instance.GetComponentsInChildren<JSONStorable>(true);
			foreach (JSONStorable js in componentsInChildren)
			{
				this.containingAtom.RestoreFromLast(js);
			}
			this.needsPostLoadJSONRestore = false;
		}
	}

	// Token: 0x06005A5F RID: 23135 RVA: 0x0014FA08 File Offset: 0x0014DE08
	protected void ClearLoadFlag()
	{
		if (this.loadFlag != null)
		{
			this.loadFlag.Raise();
			this.loadFlag = null;
		}
	}

	// Token: 0x06005A60 RID: 23136 RVA: 0x0014FA28 File Offset: 0x0014DE28
	protected IEnumerator LoadFromBundleAsync()
	{
		yield return SuperController.AssetManagerReady();
		this.loadFlag = new AsyncFlag("Load " + this.assetName);
		SuperController.singleton.SetLoadingIconFlag(this.loadFlag);
		yield return null;
		float startTime = Time.realtimeSinceStartup;
		GameObject go = null;
		if (SuperController.singleton != null)
		{
			go = SuperController.singleton.GetCachedPrefab(this.assetBundleName, this.assetName);
			if (go != null)
			{
				this.didRegisterBundle = true;
			}
		}
		if (go == null)
		{
			if (this.loadRequest == null)
			{
				this.loadRequest = AssetBundleManager.LoadAssetAsync(this.assetBundleName, this.assetName, typeof(GameObject));
				if (this.loadRequest == null)
				{
					UnityEngine.Debug.LogError("Failed to load asset " + this.assetName);
					yield break;
				}
			}
			this.didStartLoadFromBundleAsync = true;
			yield return base.StartCoroutine(this.loadRequest);
			go = this.loadRequest.GetAsset<GameObject>();
			this.loadRequest = null;
			this.didStartLoadFromBundleAsync = false;
			if (go != null)
			{
				if (SuperController.singleton != null)
				{
					SuperController.singleton.RegisterPrefab(this.assetBundleName, this.assetName, go);
					this.didRegisterBundle = true;
				}
			}
			else
			{
				UnityEngine.Debug.LogError("Asset " + this.assetName + " is missing game object");
			}
		}
		if (go != null)
		{
			startTime = Time.realtimeSinceStartup;
			this.instance = UnityEngine.Object.Instantiate<Transform>(go.transform);
			if (this.nameInstance)
			{
				this.instance.name = this.instanceName;
			}
			else
			{
				this.instance.name = this.instance.name.Replace("(Clone)", string.Empty);
			}
			this.instance.gameObject.SetActive(true);
			this.InitInstance();
			this.OnPreloadComplete();
			this.RegisterDynamicObjs();
		}
		this.OnLoadComplete(false);
		this.ClearLoadFlag();
		yield break;
	}

	// Token: 0x06005A61 RID: 23137 RVA: 0x0014FA44 File Offset: 0x0014DE44
	protected void LoadFromPrefab()
	{
		this.instance = UnityEngine.Object.Instantiate<Transform>(this.prefab);
		if (this.nameInstance)
		{
			this.instance.name = this.instanceName;
		}
		else
		{
			this.instance.name = this.instance.name.Replace("(Clone)", string.Empty);
		}
		this.instance.gameObject.SetActive(true);
		this.InitInstance();
		this.OnPreloadComplete();
		this.RegisterDynamicObjs();
		this.OnLoadComplete(false);
	}

	// Token: 0x06005A62 RID: 23138 RVA: 0x0014FAD4 File Offset: 0x0014DED4
	protected IEnumerator LoadFromPrefabAsync()
	{
		this.loadFlag = new AsyncFlag("Load " + this.prefab.name);
		SuperController.singleton.SetLoadingIconFlag(this.loadFlag);
		yield return null;
		yield return null;
		this.LoadFromPrefab();
		this.ClearLoadFlag();
		yield break;
	}

	// Token: 0x06005A63 RID: 23139 RVA: 0x0014FAF0 File Offset: 0x0014DEF0
	private IEnumerator LoadAsync()
	{
		yield return null;
		Scene sc = SceneManager.GetSceneByName(this.sceneName);
		if (!sc.IsValid() || !sc.isLoaded)
		{
			this.async = SceneManager.LoadSceneAsync(this.sceneName, LoadSceneMode.Additive);
			yield return this.async;
			sc = SceneManager.GetSceneByName(this.sceneName);
		}
		if (sc.IsValid())
		{
			GameObject[] rootGameObjects = sc.GetRootGameObjects();
			if (rootGameObjects.Length > 0)
			{
				GameObject gameObject = rootGameObjects[0];
				this.instance = UnityEngine.Object.Instantiate<Transform>(gameObject.transform);
				if (this.nameInstance)
				{
					this.instance.name = this.instanceName;
				}
				else
				{
					this.instance.name = this.instance.name.Replace("(Clone)", string.Empty);
				}
				this.instance.gameObject.SetActive(true);
				gameObject.SetActive(false);
				this.InitInstance();
				this.OnPreloadComplete();
				this.RegisterDynamicObjs();
				this.OnLoadComplete(false);
			}
		}
		else
		{
			UnityEngine.Debug.LogError("Could not open scene " + this.sceneName);
		}
		yield break;
	}

	// Token: 0x06005A64 RID: 23140 RVA: 0x0014FB0B File Offset: 0x0014DF0B
	protected virtual void Connect()
	{
	}

	// Token: 0x06005A65 RID: 23141 RVA: 0x0014FB0D File Offset: 0x0014DF0D
	protected virtual void OnPreloadComplete()
	{
		if (this.onPreloadedHandlers != null)
		{
			this.onPreloadedHandlers();
			this.onPreloadedHandlers = null;
		}
	}

	// Token: 0x06005A66 RID: 23142 RVA: 0x0014FB2C File Offset: 0x0014DF2C
	protected virtual void OnLoadComplete(bool skipJSONRestore = false)
	{
		this.Connect();
		this.ready = true;
		if (this.onLoadedHandlers != null)
		{
			this.onLoadedHandlers();
			this.onLoadedHandlers = null;
		}
		this.PostLoadJSONRestore();
	}

	// Token: 0x06005A67 RID: 23143 RVA: 0x0014FB60 File Offset: 0x0014DF60
	protected virtual void OnEnable()
	{
		if (this.instance == null)
		{
			if (this.assetBundleName != null && this.assetBundleName != string.Empty)
			{
				if (this.assetName == null || this.assetName == string.Empty)
				{
					this.assetName = base.transform.name;
				}
				if (Application.isPlaying)
				{
					base.StartCoroutine(this.LoadFromBundleAsync());
				}
			}
			else if (this.sceneName != null && this.sceneName != string.Empty)
			{
				if (Application.isPlaying)
				{
					base.StartCoroutine(this.LoadAsync());
				}
			}
			else if (this.prefab != null)
			{
				if (Application.isPlaying)
				{
					base.StartCoroutine(this.LoadFromPrefabAsync());
				}
				else
				{
					this.LoadFromPrefab();
				}
			}
			else
			{
				this.OnPreloadComplete();
				this.OnLoadComplete(true);
			}
		}
		else
		{
			this.OnPreloadComplete();
			this.RegisterDynamicObjs();
			this.OnLoadComplete(false);
		}
	}

	// Token: 0x06005A68 RID: 23144 RVA: 0x0014FC8C File Offset: 0x0014E08C
	protected void UnloadInstance()
	{
		if (this.instance != null)
		{
			UnityEngine.Debug.Log("Unload " + this.containingAtom.name + " " + this.instance.name);
			UnityEngine.Object.Destroy(this.instance.gameObject);
			this.instance = null;
			this.jss = null;
			this.cvs = null;
			this.pss = null;
			this.psjs = null;
			this.scrs = null;
			this.scrjss = null;
			this.rss = null;
			this.ready = false;
			if (this.didRegisterBundle)
			{
				this.didRegisterBundle = false;
				SuperController.singleton.UnregisterPrefab(this.assetBundleName, this.assetName);
			}
			else if (this.didStartLoadFromBundleAsync)
			{
				this.didStartLoadFromBundleAsync = false;
				this.loadRequest = null;
				AssetBundleManager.UnloadAssetBundle(this.assetBundleName);
			}
		}
	}

	// Token: 0x06005A69 RID: 23145 RVA: 0x0014FD74 File Offset: 0x0014E174
	public void UnloadIfInactive()
	{
		if (!base.gameObject.activeInHierarchy || !base.enabled)
		{
			this.UnloadInstance();
		}
	}

	// Token: 0x06005A6A RID: 23146 RVA: 0x0014FD97 File Offset: 0x0014E197
	public void UnloadIfNotEnabled()
	{
		if (!base.enabled)
		{
			this.UnloadInstance();
		}
	}

	// Token: 0x06005A6B RID: 23147 RVA: 0x0014FDAC File Offset: 0x0014E1AC
	protected virtual void OnDisable()
	{
		this.ClearLoadFlag();
		if (this.instance != null)
		{
			this.UnregisterDynamicObjs();
			if (!Application.isPlaying)
			{
				UnityEngine.Object.DestroyImmediate(this.instance.gameObject);
				this.instance = null;
				this.ready = false;
			}
			else if (!this.neverUnloadOnDisable && this.unloadOnDisable && this.containingAtom.on && this.containingAtom.gameObject.activeSelf)
			{
				this.UnloadInstance();
			}
			if (this.onPreloadedHandlers != null)
			{
				this.onPreloadedHandlers = null;
			}
			if (this.onLoadedHandlers != null)
			{
				this.onLoadedHandlers = null;
			}
		}
	}

	// Token: 0x06005A6C RID: 23148 RVA: 0x0014FE68 File Offset: 0x0014E268
	protected virtual void OnDestroy()
	{
		this.ClearLoadFlag();
		base.StopAllCoroutines();
		if (this.didRegisterBundle)
		{
			SuperController.singleton.UnregisterPrefab(this.assetBundleName, this.assetName);
		}
		else if (this.didStartLoadFromBundleAsync)
		{
			AssetBundleManager.UnloadAssetBundle(this.assetBundleName);
		}
	}

	// Token: 0x04004A9C RID: 19100
	public Atom containingAtom;

	// Token: 0x04004A9D RID: 19101
	public Transform prefab;

	// Token: 0x04004A9E RID: 19102
	public string sceneName;

	// Token: 0x04004A9F RID: 19103
	public string assetBundleName;

	// Token: 0x04004AA0 RID: 19104
	public string assetName;

	// Token: 0x04004AA1 RID: 19105
	[SerializeField]
	protected Transform instance;

	// Token: 0x04004AA2 RID: 19106
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private bool <ready>k__BackingField;

	// Token: 0x04004AA3 RID: 19107
	protected bool isRegistered;

	// Token: 0x04004AA4 RID: 19108
	public bool nameInstance;

	// Token: 0x04004AA5 RID: 19109
	public string instanceName;

	// Token: 0x04004AA6 RID: 19110
	public JSONStorableDynamic.OnLoaded onPreloadedHandlers;

	// Token: 0x04004AA7 RID: 19111
	public JSONStorableDynamic.OnLoaded onLoadedHandlers;

	// Token: 0x04004AA8 RID: 19112
	public bool neverUnloadOnDisable;

	// Token: 0x04004AA9 RID: 19113
	public bool unloadOnDisable;

	// Token: 0x04004AAA RID: 19114
	protected JSONStorable[] jss;

	// Token: 0x04004AAB RID: 19115
	protected Canvas[] cvs;

	// Token: 0x04004AAC RID: 19116
	protected PhysicsSimulator[] pss;

	// Token: 0x04004AAD RID: 19117
	protected PhysicsSimulatorJSONStorable[] psjs;

	// Token: 0x04004AAE RID: 19118
	protected ScaleChangeReceiver[] scrs;

	// Token: 0x04004AAF RID: 19119
	protected ScaleChangeReceiverJSONStorable[] scrjss;

	// Token: 0x04004AB0 RID: 19120
	protected RenderSuspend[] rss;

	// Token: 0x04004AB1 RID: 19121
	public bool needsPostLoadJSONRestore;

	// Token: 0x04004AB2 RID: 19122
	protected AsyncFlag loadFlag;

	// Token: 0x04004AB3 RID: 19123
	protected bool didStartLoadFromBundleAsync;

	// Token: 0x04004AB4 RID: 19124
	protected bool didRegisterBundle;

	// Token: 0x04004AB5 RID: 19125
	protected AssetBundleLoadAssetOperation loadRequest;

	// Token: 0x04004AB6 RID: 19126
	protected AsyncOperation async;

	// Token: 0x02000C25 RID: 3109
	// (Invoke) Token: 0x06005A6E RID: 23150
	public delegate void OnLoaded();

	// Token: 0x02000FFE RID: 4094
	[CompilerGenerated]
	private sealed class <LoadFromBundleAsync>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x0600765D RID: 30301 RVA: 0x0014FEBD File Offset: 0x0014E2BD
		[DebuggerHidden]
		public <LoadFromBundleAsync>c__Iterator0()
		{
		}

		// Token: 0x0600765E RID: 30302 RVA: 0x0014FEC8 File Offset: 0x0014E2C8
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				this.$current = SuperController.AssetManagerReady();
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			case 1U:
				this.loadFlag = new AsyncFlag("Load " + this.assetName);
				SuperController.singleton.SetLoadingIconFlag(this.loadFlag);
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
			case 2U:
				startTime = Time.realtimeSinceStartup;
				go = null;
				if (SuperController.singleton != null)
				{
					go = SuperController.singleton.GetCachedPrefab(this.assetBundleName, this.assetName);
					if (go != null)
					{
						this.didRegisterBundle = true;
					}
				}
				if (go == null)
				{
					if (this.loadRequest == null)
					{
						this.loadRequest = AssetBundleManager.LoadAssetAsync(this.assetBundleName, this.assetName, typeof(GameObject));
						if (this.loadRequest == null)
						{
							UnityEngine.Debug.LogError("Failed to load asset " + this.assetName);
							return false;
						}
					}
					this.didStartLoadFromBundleAsync = true;
					this.$current = base.StartCoroutine(this.loadRequest);
					if (!this.$disposing)
					{
						this.$PC = 3;
					}
					return true;
				}
				break;
			case 3U:
				go = this.loadRequest.GetAsset<GameObject>();
				this.loadRequest = null;
				this.didStartLoadFromBundleAsync = false;
				if (go != null)
				{
					if (SuperController.singleton != null)
					{
						SuperController.singleton.RegisterPrefab(this.assetBundleName, this.assetName, go);
						this.didRegisterBundle = true;
					}
				}
				else
				{
					UnityEngine.Debug.LogError("Asset " + this.assetName + " is missing game object");
				}
				break;
			default:
				return false;
			}
			if (go != null)
			{
				startTime = Time.realtimeSinceStartup;
				this.instance = UnityEngine.Object.Instantiate<Transform>(go.transform);
				if (this.nameInstance)
				{
					this.instance.name = this.instanceName;
				}
				else
				{
					this.instance.name = this.instance.name.Replace("(Clone)", string.Empty);
				}
				this.instance.gameObject.SetActive(true);
				this.InitInstance();
				this.OnPreloadComplete();
				base.RegisterDynamicObjs();
			}
			this.OnLoadComplete(false);
			base.ClearLoadFlag();
			this.$PC = -1;
			return false;
		}

		// Token: 0x1700117D RID: 4477
		// (get) Token: 0x0600765F RID: 30303 RVA: 0x0015022B File Offset: 0x0014E62B
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x1700117E RID: 4478
		// (get) Token: 0x06007660 RID: 30304 RVA: 0x00150233 File Offset: 0x0014E633
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x06007661 RID: 30305 RVA: 0x0015023B File Offset: 0x0014E63B
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x06007662 RID: 30306 RVA: 0x0015024B File Offset: 0x0014E64B
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04006A37 RID: 27191
		internal float <startTime>__0;

		// Token: 0x04006A38 RID: 27192
		internal GameObject <go>__0;

		// Token: 0x04006A39 RID: 27193
		internal JSONStorableDynamic $this;

		// Token: 0x04006A3A RID: 27194
		internal object $current;

		// Token: 0x04006A3B RID: 27195
		internal bool $disposing;

		// Token: 0x04006A3C RID: 27196
		internal int $PC;
	}

	// Token: 0x02000FFF RID: 4095
	[CompilerGenerated]
	private sealed class <LoadFromPrefabAsync>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x06007663 RID: 30307 RVA: 0x00150252 File Offset: 0x0014E652
		[DebuggerHidden]
		public <LoadFromPrefabAsync>c__Iterator1()
		{
		}

		// Token: 0x06007664 RID: 30308 RVA: 0x0015025C File Offset: 0x0014E65C
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				this.loadFlag = new AsyncFlag("Load " + this.prefab.name);
				SuperController.singleton.SetLoadingIconFlag(this.loadFlag);
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			case 1U:
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
			case 2U:
				base.LoadFromPrefab();
				base.ClearLoadFlag();
				this.$PC = -1;
				break;
			}
			return false;
		}

		// Token: 0x1700117F RID: 4479
		// (get) Token: 0x06007665 RID: 30309 RVA: 0x00150323 File Offset: 0x0014E723
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x17001180 RID: 4480
		// (get) Token: 0x06007666 RID: 30310 RVA: 0x0015032B File Offset: 0x0014E72B
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x06007667 RID: 30311 RVA: 0x00150333 File Offset: 0x0014E733
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x06007668 RID: 30312 RVA: 0x00150343 File Offset: 0x0014E743
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04006A3D RID: 27197
		internal JSONStorableDynamic $this;

		// Token: 0x04006A3E RID: 27198
		internal object $current;

		// Token: 0x04006A3F RID: 27199
		internal bool $disposing;

		// Token: 0x04006A40 RID: 27200
		internal int $PC;
	}

	// Token: 0x02001000 RID: 4096
	[CompilerGenerated]
	private sealed class <LoadAsync>c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x06007669 RID: 30313 RVA: 0x0015034A File Offset: 0x0014E74A
		[DebuggerHidden]
		public <LoadAsync>c__Iterator2()
		{
		}

		// Token: 0x0600766A RID: 30314 RVA: 0x00150354 File Offset: 0x0014E754
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			case 1U:
				sc = SceneManager.GetSceneByName(this.sceneName);
				if (!sc.IsValid() || !sc.isLoaded)
				{
					this.async = SceneManager.LoadSceneAsync(this.sceneName, LoadSceneMode.Additive);
					this.$current = this.async;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				break;
			case 2U:
				sc = SceneManager.GetSceneByName(this.sceneName);
				break;
			default:
				return false;
			}
			if (sc.IsValid())
			{
				GameObject[] rootGameObjects = sc.GetRootGameObjects();
				if (rootGameObjects.Length > 0)
				{
					GameObject gameObject = rootGameObjects[0];
					this.instance = UnityEngine.Object.Instantiate<Transform>(gameObject.transform);
					if (this.nameInstance)
					{
						this.instance.name = this.instanceName;
					}
					else
					{
						this.instance.name = this.instance.name.Replace("(Clone)", string.Empty);
					}
					this.instance.gameObject.SetActive(true);
					gameObject.SetActive(false);
					this.InitInstance();
					this.OnPreloadComplete();
					base.RegisterDynamicObjs();
					this.OnLoadComplete(false);
				}
			}
			else
			{
				UnityEngine.Debug.LogError("Could not open scene " + this.sceneName);
			}
			this.$PC = -1;
			return false;
		}

		// Token: 0x17001181 RID: 4481
		// (get) Token: 0x0600766B RID: 30315 RVA: 0x0015053F File Offset: 0x0014E93F
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x17001182 RID: 4482
		// (get) Token: 0x0600766C RID: 30316 RVA: 0x00150547 File Offset: 0x0014E947
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x0600766D RID: 30317 RVA: 0x0015054F File Offset: 0x0014E94F
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x0600766E RID: 30318 RVA: 0x0015055F File Offset: 0x0014E95F
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x04006A41 RID: 27201
		internal Scene <sc>__0;

		// Token: 0x04006A42 RID: 27202
		internal JSONStorableDynamic $this;

		// Token: 0x04006A43 RID: 27203
		internal object $current;

		// Token: 0x04006A44 RID: 27204
		internal bool $disposing;

		// Token: 0x04006A45 RID: 27205
		internal int $PC;
	}
}
