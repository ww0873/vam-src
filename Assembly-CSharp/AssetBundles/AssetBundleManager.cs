using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssetBundles
{
	// Token: 0x0200009E RID: 158
	public class AssetBundleManager : MonoBehaviour
	{
		// Token: 0x0600023D RID: 573 RVA: 0x00010697 File Offset: 0x0000EA97
		public AssetBundleManager()
		{
		}

		// Token: 0x17000071 RID: 113
		// (get) Token: 0x0600023E RID: 574 RVA: 0x0001069F File Offset: 0x0000EA9F
		// (set) Token: 0x0600023F RID: 575 RVA: 0x000106A6 File Offset: 0x0000EAA6
		public static AssetBundleManager.LogMode logMode
		{
			get
			{
				return AssetBundleManager.m_LogMode;
			}
			set
			{
				AssetBundleManager.m_LogMode = value;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000240 RID: 576 RVA: 0x000106AE File Offset: 0x0000EAAE
		// (set) Token: 0x06000241 RID: 577 RVA: 0x000106B5 File Offset: 0x0000EAB5
		public static string BaseDownloadingURL
		{
			get
			{
				return AssetBundleManager.m_BaseDownloadingURL;
			}
			set
			{
				AssetBundleManager.m_BaseDownloadingURL = value;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000242 RID: 578 RVA: 0x000106BD File Offset: 0x0000EABD
		// (set) Token: 0x06000243 RID: 579 RVA: 0x000106C4 File Offset: 0x0000EAC4
		public static string[] ActiveVariants
		{
			get
			{
				return AssetBundleManager.m_ActiveVariants;
			}
			set
			{
				AssetBundleManager.m_ActiveVariants = value;
			}
		}

		// Token: 0x17000074 RID: 116
		// (set) Token: 0x06000244 RID: 580 RVA: 0x000106CC File Offset: 0x0000EACC
		public static AssetBundleManifest AssetBundleManifestObject
		{
			set
			{
				AssetBundleManager.m_AssetBundleManifest = value;
			}
		}

		// Token: 0x06000245 RID: 581 RVA: 0x000106D4 File Offset: 0x0000EAD4
		private static void Log(AssetBundleManager.LogType logType, string text)
		{
			if (logType == AssetBundleManager.LogType.Error)
			{
				Debug.LogError("[AssetBundleManager] " + text);
			}
			else if (AssetBundleManager.m_LogMode == AssetBundleManager.LogMode.All)
			{
				Debug.Log("[AssetBundleManager] " + text);
			}
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0001070C File Offset: 0x0000EB0C
		private static string GetStreamingAssetsPath()
		{
			if (Application.isEditor)
			{
				return "file://" + Environment.CurrentDirectory.Replace("\\", "/");
			}
			if (Application.isMobilePlatform || Application.isConsolePlatform)
			{
				return Application.streamingAssetsPath;
			}
			return "file://" + Application.streamingAssetsPath;
		}

		// Token: 0x06000247 RID: 583 RVA: 0x0001076B File Offset: 0x0000EB6B
		public static void SetSourceAssetBundleDirectory(string relativePath)
		{
			AssetBundleManager.BaseDownloadingURL = "file://" + relativePath;
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0001077D File Offset: 0x0000EB7D
		public static void SetSourceAssetBundleURL(string absolutePath)
		{
			AssetBundleManager.BaseDownloadingURL = absolutePath + Utility.GetPlatformName() + "/";
		}

		// Token: 0x06000249 RID: 585 RVA: 0x00010794 File Offset: 0x0000EB94
		public static void SetDevelopmentAssetBundleServer()
		{
			TextAsset textAsset = Resources.Load("AssetBundleServerURL") as TextAsset;
			string text = (!(textAsset != null)) ? null : textAsset.text.Trim();
			if (text == null || text.Length == 0)
			{
				Debug.LogError("Development Server URL could not be found.");
			}
			else
			{
				AssetBundleManager.SetSourceAssetBundleURL(text);
			}
		}

		// Token: 0x0600024A RID: 586 RVA: 0x000107F8 File Offset: 0x0000EBF8
		public static LoadedAssetBundle GetLoadedAssetBundle(string assetBundleName, out string error)
		{
			if (AssetBundleManager.m_DownloadingErrors.TryGetValue(assetBundleName, out error))
			{
				return null;
			}
			LoadedAssetBundle loadedAssetBundle = null;
			AssetBundleManager.m_LoadedAssetBundles.TryGetValue(assetBundleName, out loadedAssetBundle);
			if (loadedAssetBundle == null)
			{
				return null;
			}
			string[] array = null;
			if (!AssetBundleManager.m_Dependencies.TryGetValue(assetBundleName, out array))
			{
				return loadedAssetBundle;
			}
			foreach (string key in array)
			{
				if (AssetBundleManager.m_DownloadingErrors.TryGetValue(assetBundleName, out error))
				{
					return loadedAssetBundle;
				}
				LoadedAssetBundle loadedAssetBundle2;
				AssetBundleManager.m_LoadedAssetBundles.TryGetValue(key, out loadedAssetBundle2);
				if (loadedAssetBundle2 == null)
				{
					return null;
				}
			}
			return loadedAssetBundle;
		}

		// Token: 0x0600024B RID: 587 RVA: 0x00010890 File Offset: 0x0000EC90
		public static AssetBundleLoadManifestOperation Initialize()
		{
			return AssetBundleManager.Initialize(Utility.GetPlatformName());
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0001089C File Offset: 0x0000EC9C
		public static AssetBundleLoadManifestOperation Initialize(string manifestAssetBundleName)
		{
			AssetBundleManager x = UnityEngine.Object.FindObjectOfType<AssetBundleManager>();
			if (x != null)
			{
				return null;
			}
			new GameObject("AssetBundleManager", new Type[]
			{
				typeof(AssetBundleManager)
			});
			AssetBundleManager.LoadAssetBundle(manifestAssetBundleName, true);
			AssetBundleLoadManifestOperation assetBundleLoadManifestOperation = new AssetBundleLoadManifestOperation(manifestAssetBundleName, "AssetBundleManifest", typeof(AssetBundleManifest));
			AssetBundleManager.m_InProgressOperations.Add(assetBundleLoadManifestOperation);
			return assetBundleLoadManifestOperation;
		}

		// Token: 0x0600024D RID: 589 RVA: 0x00010904 File Offset: 0x0000ED04
		protected static void LoadAssetBundle(string assetBundleName, bool isLoadingAssetBundleManifest = false)
		{
			if (!isLoadingAssetBundleManifest && AssetBundleManager.m_AssetBundleManifest == null)
			{
				Debug.LogError("Please initialize AssetBundleManifest by calling AssetBundleManager.Initialize()");
				return;
			}
			if (!AssetBundleManager.LoadAssetBundleInternal(assetBundleName, isLoadingAssetBundleManifest) && !isLoadingAssetBundleManifest)
			{
				AssetBundleManager.LoadDependencies(assetBundleName);
			}
		}

		// Token: 0x0600024E RID: 590 RVA: 0x0001094C File Offset: 0x0000ED4C
		protected static string RemapVariantName(string assetBundleName)
		{
			string[] allAssetBundlesWithVariant = AssetBundleManager.m_AssetBundleManifest.GetAllAssetBundlesWithVariant();
			string[] array = assetBundleName.Split(new char[]
			{
				'.'
			});
			int num = int.MaxValue;
			int num2 = -1;
			for (int i = 0; i < allAssetBundlesWithVariant.Length; i++)
			{
				string[] array2 = allAssetBundlesWithVariant[i].Split(new char[]
				{
					'.'
				});
				if (!(array2[0] != array[0]))
				{
					int num3 = Array.IndexOf<string>(AssetBundleManager.m_ActiveVariants, array2[1]);
					if (num3 == -1)
					{
						num3 = 2147483646;
					}
					if (num3 < num)
					{
						num = num3;
						num2 = i;
					}
				}
			}
			if (num == 2147483646)
			{
				Debug.LogWarning("Ambigious asset bundle variant chosen because there was no matching active variant: " + allAssetBundlesWithVariant[num2]);
			}
			if (num2 != -1)
			{
				return allAssetBundlesWithVariant[num2];
			}
			return assetBundleName;
		}

		// Token: 0x0600024F RID: 591 RVA: 0x00010A18 File Offset: 0x0000EE18
		protected static bool LoadAssetBundleInternal(string assetBundleName, bool isLoadingAssetBundleManifest)
		{
			LoadedAssetBundle loadedAssetBundle = null;
			AssetBundleManager.m_LoadedAssetBundles.TryGetValue(assetBundleName, out loadedAssetBundle);
			if (loadedAssetBundle != null)
			{
				loadedAssetBundle.m_ReferencedCount++;
				return true;
			}
			LoadedAssetBundle loadedAssetBundle2;
			if (AssetBundleManager.m_TrackedAssetBundles.TryGetValue(assetBundleName, out loadedAssetBundle2))
			{
				loadedAssetBundle2.m_ReferencedCount++;
				return true;
			}
			string text = AssetBundleManager.m_BaseDownloadingURL + assetBundleName;
			if (isLoadingAssetBundleManifest)
			{
				WWW value = new WWW(text);
				AssetBundleManager.m_DownloadingWWWs.Add(assetBundleName, value);
				loadedAssetBundle2 = new LoadedAssetBundle(null);
				AssetBundleManager.m_TrackedAssetBundles.Add(assetBundleName, loadedAssetBundle2);
			}
			else if (text.StartsWith("file:"))
			{
				text = text.Replace("file://", string.Empty);
				AssetBundleCreateRequest value2 = AssetBundle.LoadFromFileAsync(text);
				AssetBundleManager.m_LoadingBundles.Add(assetBundleName, value2);
				loadedAssetBundle2 = new LoadedAssetBundle(null);
				AssetBundleManager.m_TrackedAssetBundles.Add(assetBundleName, loadedAssetBundle2);
			}
			else
			{
				WWW value = WWW.LoadFromCacheOrDownload(text, AssetBundleManager.m_AssetBundleManifest.GetAssetBundleHash(assetBundleName), 0U);
				AssetBundleManager.m_DownloadingWWWs.Add(assetBundleName, value);
				loadedAssetBundle2 = new LoadedAssetBundle(null);
				AssetBundleManager.m_TrackedAssetBundles.Add(assetBundleName, loadedAssetBundle2);
			}
			return false;
		}

		// Token: 0x06000250 RID: 592 RVA: 0x00010B30 File Offset: 0x0000EF30
		protected static void LoadDependencies(string assetBundleName)
		{
			if (AssetBundleManager.m_AssetBundleManifest == null)
			{
				Debug.LogError("Please initialize AssetBundleManifest by calling AssetBundleManager.Initialize()");
				return;
			}
			string[] allDependencies = AssetBundleManager.m_AssetBundleManifest.GetAllDependencies(assetBundleName);
			if (allDependencies.Length == 0)
			{
				return;
			}
			for (int i = 0; i < allDependencies.Length; i++)
			{
				allDependencies[i] = AssetBundleManager.RemapVariantName(allDependencies[i]);
			}
			AssetBundleManager.m_Dependencies.Add(assetBundleName, allDependencies);
			for (int j = 0; j < allDependencies.Length; j++)
			{
				AssetBundleManager.LoadAssetBundleInternal(allDependencies[j], false);
			}
		}

		// Token: 0x06000251 RID: 593 RVA: 0x00010BB6 File Offset: 0x0000EFB6
		public static void UnloadAssetBundle(string assetBundleName)
		{
			if (AssetBundleManager.UnloadAssetBundleInternal(assetBundleName))
			{
				AssetBundleManager.UnloadDependencies(assetBundleName);
			}
		}

		// Token: 0x06000252 RID: 594 RVA: 0x00010BCC File Offset: 0x0000EFCC
		protected static void UnloadDependencies(string assetBundleName)
		{
			string[] array = null;
			if (!AssetBundleManager.m_Dependencies.TryGetValue(assetBundleName, out array))
			{
				return;
			}
			foreach (string assetBundleName2 in array)
			{
				AssetBundleManager.UnloadAssetBundleInternal(assetBundleName2);
			}
			AssetBundleManager.m_Dependencies.Remove(assetBundleName);
		}

		// Token: 0x06000253 RID: 595 RVA: 0x00010C1C File Offset: 0x0000F01C
		protected static bool UnloadAssetBundleInternal(string assetBundleName)
		{
			LoadedAssetBundle loadedAssetBundle;
			if (!AssetBundleManager.m_TrackedAssetBundles.TryGetValue(assetBundleName, out loadedAssetBundle))
			{
				Debug.LogError("Tried to unload assetbundle " + assetBundleName + " which is not loaded");
				return false;
			}
			loadedAssetBundle.m_ReferencedCount--;
			if (loadedAssetBundle.m_ReferencedCount == 0 && loadedAssetBundle.m_AssetBundle != null)
			{
				loadedAssetBundle.m_AssetBundle.Unload(true);
				AssetBundleManager.m_LoadedAssetBundles.Remove(assetBundleName);
				AssetBundleManager.m_TrackedAssetBundles.Remove(assetBundleName);
				return true;
			}
			return false;
		}

		// Token: 0x06000254 RID: 596 RVA: 0x00010CA8 File Offset: 0x0000F0A8
		public static void ReportLoadedAssetBundles()
		{
			int num = 0;
			if (AssetBundleManager.m_TrackedAssetBundles != null)
			{
				foreach (LoadedAssetBundle loadedAssetBundle in AssetBundleManager.m_TrackedAssetBundles.Values)
				{
					SuperController.LogMessage(string.Concat(new object[]
					{
						"Loaded bundle ",
						loadedAssetBundle.m_AssetBundle.name,
						" has ",
						loadedAssetBundle.m_ReferencedCount,
						" references"
					}));
					num++;
				}
			}
			SuperController.LogMessage("Found " + num + " loaded bundles");
		}

		// Token: 0x06000255 RID: 597 RVA: 0x00010D70 File Offset: 0x0000F170
		public static int GetNumberOfLoadedAssetBundles()
		{
			int num = 0;
			if (AssetBundleManager.m_TrackedAssetBundles != null)
			{
				foreach (LoadedAssetBundle loadedAssetBundle in AssetBundleManager.m_TrackedAssetBundles.Values)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06000256 RID: 598 RVA: 0x00010DDC File Offset: 0x0000F1DC
		private void Update()
		{
			if (this.keysToRemove == null)
			{
				this.keysToRemove = new List<string>();
			}
			else
			{
				this.keysToRemove.Clear();
			}
			foreach (KeyValuePair<string, WWW> keyValuePair in AssetBundleManager.m_DownloadingWWWs)
			{
				WWW value = keyValuePair.Value;
				if (value.error != null)
				{
					if (!AssetBundleManager.m_DownloadingErrors.ContainsKey(keyValuePair.Key))
					{
						AssetBundleManager.m_DownloadingErrors.Add(keyValuePair.Key, string.Format("Failed downloading bundle {0} from {1}: {2}", keyValuePair.Key, value.url, value.error));
					}
					this.keysToRemove.Add(keyValuePair.Key);
				}
				else if (value.isDone)
				{
					AssetBundle assetBundle = value.assetBundle;
					if (assetBundle == null)
					{
						if (!AssetBundleManager.m_DownloadingErrors.ContainsKey(keyValuePair.Key))
						{
							AssetBundleManager.m_DownloadingErrors.Add(keyValuePair.Key, string.Format("{0} is not a valid asset bundle.", keyValuePair.Key));
						}
						this.keysToRemove.Add(keyValuePair.Key);
					}
					else
					{
						LoadedAssetBundle loadedAssetBundle;
						if (AssetBundleManager.m_TrackedAssetBundles.TryGetValue(keyValuePair.Key, out loadedAssetBundle))
						{
							if (loadedAssetBundle.m_ReferencedCount == 0)
							{
								Debug.LogError("Unload assetbundle " + keyValuePair.Key + " that was untracked while being loaded");
								assetBundle.Unload(true);
								AssetBundleManager.m_TrackedAssetBundles.Remove(keyValuePair.Key);
								AssetBundleManager.m_LoadedAssetBundles.Remove(keyValuePair.Key);
							}
							else if (!AssetBundleManager.m_LoadedAssetBundles.ContainsKey(keyValuePair.Key))
							{
								loadedAssetBundle.m_AssetBundle = value.assetBundle;
								AssetBundleManager.m_LoadedAssetBundles.Add(keyValuePair.Key, loadedAssetBundle);
							}
							else
							{
								Debug.LogError("Loaded assetbundles already contains " + keyValuePair.Key);
							}
						}
						this.keysToRemove.Add(keyValuePair.Key);
					}
				}
			}
			foreach (string key in this.keysToRemove)
			{
				WWW www = AssetBundleManager.m_DownloadingWWWs[key];
				AssetBundleManager.m_DownloadingWWWs.Remove(key);
				www.Dispose();
			}
			this.keysToRemove.Clear();
			foreach (KeyValuePair<string, AssetBundleCreateRequest> keyValuePair2 in AssetBundleManager.m_LoadingBundles)
			{
				AssetBundleCreateRequest value2 = keyValuePair2.Value;
				if (value2.isDone)
				{
					AssetBundle assetBundle2 = value2.assetBundle;
					if (assetBundle2 == null)
					{
						if (!AssetBundleManager.m_DownloadingErrors.ContainsKey(keyValuePair2.Key))
						{
							AssetBundleManager.m_DownloadingErrors.Add(keyValuePair2.Key, string.Format("{0} is not a valid asset bundle.", keyValuePair2.Key));
						}
						this.keysToRemove.Add(keyValuePair2.Key);
					}
					else
					{
						LoadedAssetBundle loadedAssetBundle2;
						if (AssetBundleManager.m_TrackedAssetBundles.TryGetValue(keyValuePair2.Key, out loadedAssetBundle2))
						{
							if (loadedAssetBundle2.m_ReferencedCount == 0)
							{
								Debug.LogError("Unload assetbundle " + keyValuePair2.Key + " that was untracked while being loaded");
								assetBundle2.Unload(true);
								AssetBundleManager.m_TrackedAssetBundles.Remove(keyValuePair2.Key);
							}
							else if (!AssetBundleManager.m_LoadedAssetBundles.ContainsKey(keyValuePair2.Key))
							{
								loadedAssetBundle2.m_AssetBundle = value2.assetBundle;
								AssetBundleManager.m_LoadedAssetBundles.Add(keyValuePair2.Key, loadedAssetBundle2);
							}
							else
							{
								Debug.LogError("Loaded assetbundles already contains " + keyValuePair2.Key);
							}
						}
						this.keysToRemove.Add(keyValuePair2.Key);
					}
				}
			}
			foreach (string key2 in this.keysToRemove)
			{
				AssetBundleManager.m_LoadingBundles.Remove(key2);
			}
			int i = 0;
			while (i < AssetBundleManager.m_InProgressOperations.Count)
			{
				if (!AssetBundleManager.m_InProgressOperations[i].Update())
				{
					AssetBundleManager.m_InProgressOperations.RemoveAt(i);
				}
				else
				{
					i++;
				}
			}
		}

		// Token: 0x06000257 RID: 599 RVA: 0x000112C0 File Offset: 0x0000F6C0
		public static void RegisterAssetBundleAdditionalUse(string assetBundleName)
		{
			LoadedAssetBundle loadedAssetBundle;
			if (AssetBundleManager.m_TrackedAssetBundles.TryGetValue(assetBundleName, out loadedAssetBundle))
			{
				loadedAssetBundle.m_ReferencedCount++;
			}
			else
			{
				Debug.LogError("Tried to register additional use of asset bundle " + assetBundleName + " which is not loaded");
			}
		}

		// Token: 0x06000258 RID: 600 RVA: 0x00011308 File Offset: 0x0000F708
		public static AssetBundleLoadAssetOperation LoadAssetAsync(string assetBundleName, string assetName, Type type)
		{
			assetBundleName = AssetBundleManager.RemapVariantName(assetBundleName);
			AssetBundleManager.LoadAssetBundle(assetBundleName, false);
			AssetBundleLoadAssetOperation assetBundleLoadAssetOperation = new AssetBundleLoadAssetOperationFull(assetBundleName, assetName, type);
			AssetBundleManager.m_InProgressOperations.Add(assetBundleLoadAssetOperation);
			return assetBundleLoadAssetOperation;
		}

		// Token: 0x06000259 RID: 601 RVA: 0x0001133C File Offset: 0x0000F73C
		public static AssetBundleLoadOperation LoadLevelAsync(string assetBundleName, string levelName, bool isAdditive)
		{
			AssetBundleManager.Log(AssetBundleManager.LogType.Info, string.Concat(new string[]
			{
				"Loading ",
				levelName,
				" from ",
				assetBundleName,
				" bundle"
			}));
			assetBundleName = AssetBundleManager.RemapVariantName(assetBundleName);
			AssetBundleManager.LoadAssetBundle(assetBundleName, false);
			AssetBundleLoadOperation assetBundleLoadOperation = new AssetBundleLoadLevelOperation(assetBundleName, levelName, isAdditive);
			AssetBundleManager.m_InProgressOperations.Add(assetBundleLoadOperation);
			return assetBundleLoadOperation;
		}

		// Token: 0x0600025A RID: 602 RVA: 0x000113A0 File Offset: 0x0000F7A0
		// Note: this type is marked as 'beforefieldinit'.
		static AssetBundleManager()
		{
		}

		// Token: 0x04000325 RID: 805
		private static AssetBundleManager.LogMode m_LogMode = AssetBundleManager.LogMode.All;

		// Token: 0x04000326 RID: 806
		private static string m_BaseDownloadingURL = string.Empty;

		// Token: 0x04000327 RID: 807
		private static string[] m_ActiveVariants = new string[0];

		// Token: 0x04000328 RID: 808
		private static AssetBundleManifest m_AssetBundleManifest = null;

		// Token: 0x04000329 RID: 809
		private static Dictionary<string, LoadedAssetBundle> m_LoadedAssetBundles = new Dictionary<string, LoadedAssetBundle>();

		// Token: 0x0400032A RID: 810
		private static Dictionary<string, WWW> m_DownloadingWWWs = new Dictionary<string, WWW>();

		// Token: 0x0400032B RID: 811
		private static Dictionary<string, LoadedAssetBundle> m_TrackedAssetBundles = new Dictionary<string, LoadedAssetBundle>();

		// Token: 0x0400032C RID: 812
		private static Dictionary<string, AssetBundleCreateRequest> m_LoadingBundles = new Dictionary<string, AssetBundleCreateRequest>();

		// Token: 0x0400032D RID: 813
		private static Dictionary<string, string> m_DownloadingErrors = new Dictionary<string, string>();

		// Token: 0x0400032E RID: 814
		private static List<AssetBundleLoadOperation> m_InProgressOperations = new List<AssetBundleLoadOperation>();

		// Token: 0x0400032F RID: 815
		private static Dictionary<string, string[]> m_Dependencies = new Dictionary<string, string[]>();

		// Token: 0x04000330 RID: 816
		protected List<string> keysToRemove;

		// Token: 0x0200009F RID: 159
		public enum LogMode
		{
			// Token: 0x04000332 RID: 818
			All,
			// Token: 0x04000333 RID: 819
			JustErrors
		}

		// Token: 0x020000A0 RID: 160
		public enum LogType
		{
			// Token: 0x04000335 RID: 821
			Info,
			// Token: 0x04000336 RID: 822
			Warning,
			// Token: 0x04000337 RID: 823
			Error
		}
	}
}
