using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using AssetBundles;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MeshVR
{
	// Token: 0x02000C12 RID: 3090
	public class GlobalSceneOptions : MonoBehaviour
	{
		// Token: 0x060059D5 RID: 22997 RVA: 0x00210A98 File Offset: 0x0020EE98
		public GlobalSceneOptions()
		{
		}

		// Token: 0x17000D3F RID: 3391
		// (get) Token: 0x060059D6 RID: 22998 RVA: 0x00210AEE File Offset: 0x0020EEEE
		// (set) Token: 0x060059D7 RID: 22999 RVA: 0x00210AF5 File Offset: 0x0020EEF5
		public static GlobalSceneOptions singleton
		{
			[CompilerGenerated]
			get
			{
				return GlobalSceneOptions.<singleton>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				GlobalSceneOptions.<singleton>k__BackingField = value;
			}
		}

		// Token: 0x17000D40 RID: 3392
		// (get) Token: 0x060059D8 RID: 23000 RVA: 0x00210AFD File Offset: 0x0020EEFD
		public static bool IsLoading
		{
			get
			{
				return GlobalSceneOptions.singleton != null && GlobalSceneOptions.singleton.isLoading;
			}
		}

		// Token: 0x060059D9 RID: 23001 RVA: 0x00210B1C File Offset: 0x0020EF1C
		private IEnumerator LoadAssets()
		{
			AssetBundleManager.SetSourceAssetBundleDirectory(Application.streamingAssetsPath + "/");
			AssetBundleLoadManifestOperation request = AssetBundleManager.Initialize();
			if (request != null)
			{
				yield return base.StartCoroutine(request);
			}
			UnityEngine.Debug.Log("Asset Manager Ready");
			foreach (GlobalSceneOptions.AssetBundleAssetNames assetToLoad in this.assetsToLoadOnStart)
			{
				AssetBundleLoadAssetOperation arequest = AssetBundleManager.LoadAssetAsync(assetToLoad.assetBundleName, assetToLoad.assetName, typeof(GameObject));
				if (arequest == null)
				{
					UnityEngine.Debug.LogError("Failed to load asset " + assetToLoad.assetBundleName + " " + assetToLoad.assetName);
				}
				else
				{
					yield return base.StartCoroutine(arequest);
					GameObject go = arequest.GetAsset<GameObject>();
					if (go != null)
					{
						GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(go);
						if (this.atomContainer != null)
						{
							gameObject.transform.SetParent(this.atomContainer, true);
							if (assetToLoad.setPosition)
							{
								gameObject.transform.position = assetToLoad.position;
							}
							if (assetToLoad.setRotation)
							{
								gameObject.transform.eulerAngles = assetToLoad.rotationEuler;
							}
							gameObject.name = assetToLoad.assetName;
						}
					}
				}
			}
			if (this.transformsToActivateAfterAssetLoad != null)
			{
				foreach (Transform transform in this.transformsToActivateAfterAssetLoad)
				{
					transform.gameObject.SetActive(true);
				}
			}
			this.isLoading = false;
			yield break;
		}

		// Token: 0x060059DA RID: 23002 RVA: 0x00210B37 File Offset: 0x0020EF37
		private void Awake()
		{
			GlobalSceneOptions.singleton = this;
		}

		// Token: 0x060059DB RID: 23003 RVA: 0x00210B40 File Offset: 0x0020EF40
		private void Start()
		{
			if (this.processCommandlineForBenchmarks)
			{
				string[] commandLineArgs = Environment.GetCommandLineArgs();
				string text = string.Empty;
				for (int i = 0; i < commandLineArgs.Length; i++)
				{
					if (commandLineArgs[i] == "-benchmark")
					{
						text = commandLineArgs[i + 1];
					}
				}
				if (text != string.Empty)
				{
					SceneManager.LoadScene(text, LoadSceneMode.Single);
				}
			}
			if (this.assetsToLoadOnStart != null && this.assetsToLoadOnStart.Length > 0)
			{
				this.isLoading = true;
				base.StartCoroutine(this.LoadAssets());
			}
		}

		// Token: 0x040049E9 RID: 18921
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static GlobalSceneOptions <singleton>k__BackingField;

		// Token: 0x040049EA RID: 18922
		protected bool isLoading;

		// Token: 0x040049EB RID: 18923
		public Transform atomContainer;

		// Token: 0x040049EC RID: 18924
		public Text versionText;

		// Token: 0x040049ED RID: 18925
		public bool processCommandlineForBenchmarks;

		// Token: 0x040049EE RID: 18926
		public string freeKey;

		// Token: 0x040049EF RID: 18927
		public string teaserKey;

		// Token: 0x040049F0 RID: 18928
		public string entertainerKey;

		// Token: 0x040049F1 RID: 18929
		public string creatorKey;

		// Token: 0x040049F2 RID: 18930
		public string steamKey;

		// Token: 0x040049F3 RID: 18931
		public string nsteamKey;

		// Token: 0x040049F4 RID: 18932
		public string retailKey;

		// Token: 0x040049F5 RID: 18933
		public string keyFilePath;

		// Token: 0x040049F6 RID: 18934
		public string legacySteamKeyFilePath;

		// Token: 0x040049F7 RID: 18935
		public bool disableAdvancedSceneEdit;

		// Token: 0x040049F8 RID: 18936
		public bool disableSaveSceneButton;

		// Token: 0x040049F9 RID: 18937
		public bool disableLoadSceneButton;

		// Token: 0x040049FA RID: 18938
		public bool disableCustomUI;

		// Token: 0x040049FB RID: 18939
		public bool disableBrowse;

		// Token: 0x040049FC RID: 18940
		public bool disablePackages;

		// Token: 0x040049FD RID: 18941
		public bool disablePromotional;

		// Token: 0x040049FE RID: 18942
		public bool disableKeyInformation;

		// Token: 0x040049FF RID: 18943
		public bool disableHub;

		// Token: 0x04004A00 RID: 18944
		public bool disableTermsOfUse;

		// Token: 0x04004A01 RID: 18945
		public bool disableUI;

		// Token: 0x04004A02 RID: 18946
		public bool alwaysEnablePointers;

		// Token: 0x04004A03 RID: 18947
		public bool disableNavigation;

		// Token: 0x04004A04 RID: 18948
		public bool disableVR;

		// Token: 0x04004A05 RID: 18949
		public float startingMonitorCameraFOV = 40f;

		// Token: 0x04004A06 RID: 18950
		public bool enableStartScene;

		// Token: 0x04004A07 RID: 18951
		public JSONEmbed startJSONEmbedScene;

		// Token: 0x04004A08 RID: 18952
		public bool assetManagerSimulateInEditor = true;

		// Token: 0x04004A09 RID: 18953
		public bool disableLeap;

		// Token: 0x04004A0A RID: 18954
		public bool loadPrefsFileOnStart = true;

		// Token: 0x04004A0B RID: 18955
		public bool bypassFirstTimeUser = true;

		// Token: 0x04004A0C RID: 18956
		public bool overridePhysicsRate;

		// Token: 0x04004A0D RID: 18957
		public UserPreferences.PhysicsRate physicsRate = UserPreferences.PhysicsRate._90;

		// Token: 0x04004A0E RID: 18958
		public bool overridePhysicsUpdateCap;

		// Token: 0x04004A0F RID: 18959
		public int physicsUpdateCap = 2;

		// Token: 0x04004A10 RID: 18960
		public bool overridePhysicsHighQuality;

		// Token: 0x04004A11 RID: 18961
		public bool physicsHighQuality;

		// Token: 0x04004A12 RID: 18962
		public bool overrideSoftPhysics;

		// Token: 0x04004A13 RID: 18963
		public bool softPhysics = true;

		// Token: 0x04004A14 RID: 18964
		public bool overrideMsaaLevel;

		// Token: 0x04004A15 RID: 18965
		public int msaaLevel = 4;

		// Token: 0x04004A16 RID: 18966
		public bool overridePixelLightCount;

		// Token: 0x04004A17 RID: 18967
		public int pixelLightCount = 2;

		// Token: 0x04004A18 RID: 18968
		public bool enablePerfMonOnStart;

		// Token: 0x04004A19 RID: 18969
		public GlobalSceneOptions.AssetBundleAssetNames[] assetsToLoadOnStart;

		// Token: 0x04004A1A RID: 18970
		public Transform[] transformsToActivateAfterAssetLoad;

		// Token: 0x02000C13 RID: 3091
		[Serializable]
		public class AssetBundleAssetNames
		{
			// Token: 0x060059DC RID: 23004 RVA: 0x00210BD4 File Offset: 0x0020EFD4
			public AssetBundleAssetNames()
			{
			}

			// Token: 0x04004A1B RID: 18971
			public string assetBundleName;

			// Token: 0x04004A1C RID: 18972
			public string assetName;

			// Token: 0x04004A1D RID: 18973
			public bool setPosition;

			// Token: 0x04004A1E RID: 18974
			public Vector3 position;

			// Token: 0x04004A1F RID: 18975
			public bool setRotation;

			// Token: 0x04004A20 RID: 18976
			public Vector3 rotationEuler;
		}

		// Token: 0x02000FFD RID: 4093
		[CompilerGenerated]
		private sealed class <LoadAssets>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007657 RID: 30295 RVA: 0x00210BDC File Offset: 0x0020EFDC
			[DebuggerHidden]
			public <LoadAssets>c__Iterator0()
			{
			}

			// Token: 0x06007658 RID: 30296 RVA: 0x00210BE4 File Offset: 0x0020EFE4
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					AssetBundleManager.SetSourceAssetBundleDirectory(Application.streamingAssetsPath + "/");
					request = AssetBundleManager.Initialize();
					if (request != null)
					{
						this.$current = base.StartCoroutine(request);
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					break;
				case 1U:
					break;
				case 2U:
				{
					go = arequest.GetAsset<GameObject>();
					if (!(go != null))
					{
						goto IL_1FC;
					}
					GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(go);
					if (this.atomContainer != null)
					{
						gameObject.transform.SetParent(this.atomContainer, true);
						if (assetToLoad.setPosition)
						{
							gameObject.transform.position = assetToLoad.position;
						}
						if (assetToLoad.setRotation)
						{
							gameObject.transform.eulerAngles = assetToLoad.rotationEuler;
						}
						gameObject.name = assetToLoad.assetName;
						goto IL_1FC;
					}
					goto IL_1FC;
				}
				default:
					return false;
				}
				UnityEngine.Debug.Log("Asset Manager Ready");
				array = this.assetsToLoadOnStart;
				i = 0;
				goto IL_20A;
				IL_1FC:
				i++;
				IL_20A:
				if (i >= array.Length)
				{
					if (this.transformsToActivateAfterAssetLoad != null)
					{
						foreach (Transform transform in this.transformsToActivateAfterAssetLoad)
						{
							transform.gameObject.SetActive(true);
						}
					}
					this.isLoading = false;
					this.$PC = -1;
				}
				else
				{
					assetToLoad = array[i];
					arequest = AssetBundleManager.LoadAssetAsync(assetToLoad.assetBundleName, assetToLoad.assetName, typeof(GameObject));
					if (arequest == null)
					{
						UnityEngine.Debug.LogError("Failed to load asset " + assetToLoad.assetBundleName + " " + assetToLoad.assetName);
						goto IL_1FC;
					}
					this.$current = base.StartCoroutine(arequest);
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				return false;
			}

			// Token: 0x1700117B RID: 4475
			// (get) Token: 0x06007659 RID: 30297 RVA: 0x00210E69 File Offset: 0x0020F269
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x1700117C RID: 4476
			// (get) Token: 0x0600765A RID: 30298 RVA: 0x00210E71 File Offset: 0x0020F271
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x0600765B RID: 30299 RVA: 0x00210E79 File Offset: 0x0020F279
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x0600765C RID: 30300 RVA: 0x00210E89 File Offset: 0x0020F289
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x04006A2D RID: 27181
			internal AssetBundleLoadManifestOperation <request>__0;

			// Token: 0x04006A2E RID: 27182
			internal GlobalSceneOptions.AssetBundleAssetNames[] $locvar0;

			// Token: 0x04006A2F RID: 27183
			internal int $locvar1;

			// Token: 0x04006A30 RID: 27184
			internal GlobalSceneOptions.AssetBundleAssetNames <assetToLoad>__1;

			// Token: 0x04006A31 RID: 27185
			internal AssetBundleLoadAssetOperation <arequest>__2;

			// Token: 0x04006A32 RID: 27186
			internal GameObject <go>__2;

			// Token: 0x04006A33 RID: 27187
			internal GlobalSceneOptions $this;

			// Token: 0x04006A34 RID: 27188
			internal object $current;

			// Token: 0x04006A35 RID: 27189
			internal bool $disposing;

			// Token: 0x04006A36 RID: 27190
			internal int $PC;
		}
	}
}
