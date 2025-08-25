using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using MVR.FileManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MeshVR
{
	// Token: 0x02000BFD RID: 3069
	public class AssetLoader : MonoBehaviour
	{
		// Token: 0x060058A6 RID: 22694 RVA: 0x00207A6B File Offset: 0x00205E6B
		public AssetLoader()
		{
		}

		// Token: 0x060058A7 RID: 22695 RVA: 0x00207A74 File Offset: 0x00205E74
		protected IEnumerator LoadBundleFileAsync(AssetLoader.AssetBundleFromFileRequest abffr)
		{
			if (this.assetBundleReferenceCounts == null)
			{
				this.assetBundleReferenceCounts = new Dictionary<string, int>();
			}
			if (this.pathToAssetBundle == null)
			{
				this.pathToAssetBundle = new Dictionary<string, AssetBundle>();
			}
			string path = abffr.path;
			int cnt;
			if (this.assetBundleReferenceCounts.TryGetValue(path, out cnt))
			{
				cnt++;
				this.assetBundleReferenceCounts.Remove(path);
				this.assetBundleReferenceCounts.Add(path, cnt);
			}
			else
			{
				this.assetBundleReferenceCounts.Add(path, 1);
			}
			AssetBundle ab = null;
			if (!this.pathToAssetBundle.TryGetValue(path, out ab))
			{
				AssetBundleCreateRequest abcr = null;
				if (FileManager.IsFileInPackage(path))
				{
					VarFileEntry vfe = FileManager.GetVarFileEntry(path);
					if (vfe.Simulated)
					{
						string path2 = vfe.Package.Path + "\\" + vfe.InternalPath;
						abcr = AssetBundle.LoadFromFileAsync(path2);
					}
					else
					{
						byte[] assetbundleBytes = new byte[vfe.Size];
						yield return FileManager.ReadAllBytesCoroutine(vfe, assetbundleBytes);
						abcr = AssetBundle.LoadFromMemoryAsync(assetbundleBytes);
					}
				}
				else
				{
					abcr = AssetBundle.LoadFromFileAsync(path);
				}
				if (abcr != null)
				{
					yield return abcr;
					if (!abcr.assetBundle)
					{
						SuperController.LogError("Error during attempt to load assetbundle " + path + ". Not valid");
					}
					else
					{
						ab = abcr.assetBundle;
						this.pathToAssetBundle.Add(path, abcr.assetBundle);
					}
				}
			}
			abffr.assetBundle = ab;
			if (abffr.callback != null)
			{
				abffr.callback(abffr);
			}
			yield break;
		}

		// Token: 0x060058A8 RID: 22696 RVA: 0x00207A96 File Offset: 0x00205E96
		public static void QueueLoadAssetBundleFromFile(AssetLoader.AssetBundleFromFileRequest abffr)
		{
			if (AssetLoader.singleton != null)
			{
				if (AssetLoader.singleton.assetBundleFromFileQueue == null)
				{
					AssetLoader.singleton.assetBundleFromFileQueue = new List<AssetLoader.AssetBundleFromFileRequest>();
				}
				AssetLoader.singleton.assetBundleFromFileQueue.Add(abffr);
			}
		}

		// Token: 0x060058A9 RID: 22697 RVA: 0x00207AD8 File Offset: 0x00205ED8
		protected IEnumerator AssetBundleFromFileQueueProcessor()
		{
			if (this.assetBundleFromFileQueue == null)
			{
				this.assetBundleFromFileQueue = new List<AssetLoader.AssetBundleFromFileRequest>();
			}
			for (;;)
			{
				yield return null;
				if (this.assetBundleFromFileQueue.Count > 0)
				{
					AssetLoader.AssetBundleFromFileRequest abffr = this.assetBundleFromFileQueue[0];
					this.assetBundleFromFileQueue.RemoveAt(0);
					yield return this.LoadBundleFileAsync(abffr);
				}
			}
			yield break;
		}

		// Token: 0x060058AA RID: 22698 RVA: 0x00207AF4 File Offset: 0x00205EF4
		public static void DoneWithAssetBundleFromFile(string path)
		{
			int num;
			if (AssetLoader.singleton != null && AssetLoader.singleton.assetBundleReferenceCounts != null && AssetLoader.singleton.assetBundleReferenceCounts.TryGetValue(path, out num))
			{
				num--;
				if (num <= 0)
				{
					AssetBundle assetBundle;
					if (AssetLoader.singleton.pathToAssetBundle.TryGetValue(path, out assetBundle))
					{
						UnityEngine.Debug.Log("Unloading unused asset bundle " + path);
						if (assetBundle != null)
						{
							assetBundle.Unload(true);
						}
						AssetLoader.singleton.pathToAssetBundle.Remove(path);
					}
					AssetLoader.singleton.assetBundleReferenceCounts.Remove(path);
				}
				else
				{
					AssetLoader.singleton.assetBundleReferenceCounts.Remove(path);
					AssetLoader.singleton.assetBundleReferenceCounts.Add(path, num);
				}
			}
		}

		// Token: 0x060058AB RID: 22699 RVA: 0x00207BC8 File Offset: 0x00205FC8
		protected IEnumerator LoadSceneIntoTransformAsync(AssetLoader.SceneLoadIntoTransformRequest slr)
		{
			AsyncOperation async = null;
			try
			{
				async = SceneManager.LoadSceneAsync(slr.scenePath, LoadSceneMode.Additive);
			}
			catch (Exception arg)
			{
				SuperController.LogError("Error during attempt to load scene: " + arg);
			}
			if (async != null)
			{
				yield return async;
				Scene sc = SceneManager.GetSceneByPath(slr.scenePath);
				if (sc.IsValid())
				{
					if (slr.requestCancelled)
					{
						async = SceneManager.UnloadSceneAsync(sc);
						yield return async;
						yield break;
					}
					LightmapData[] newLightmapData = LightmapSettings.lightmaps;
					LightProbes lightProbes = LightmapSettings.lightProbes;
					if (GlobalLightingManager.singleton != null)
					{
						if (GlobalLightingManager.singleton.PushLightmapData(newLightmapData))
						{
							slr.lightmapData = newLightmapData;
							if (!slr.importLightmaps)
							{
								GlobalLightingManager.singleton.RemoveLightmapData(slr.lightmapData);
							}
						}
						else
						{
							slr.lightmapData = null;
						}
						slr.lightProbesHolder = GlobalLightingManager.singleton.PushLightProbes(lightProbes);
						if (slr.lightProbesHolder != null && !slr.importLightProbes)
						{
							GlobalLightingManager.singleton.RemoveLightProbesHolder(slr.lightProbesHolder);
						}
					}
					if (slr.transform != null)
					{
						GameObject[] rootGameObjects = sc.GetRootGameObjects();
						foreach (GameObject gameObject in rootGameObjects)
						{
							Vector3 localPosition = gameObject.transform.localPosition;
							Quaternion localRotation = gameObject.transform.localRotation;
							Vector3 localScale = gameObject.transform.localScale;
							SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
							gameObject.transform.SetParent(slr.transform);
							gameObject.transform.localPosition = localPosition;
							gameObject.transform.localRotation = localRotation;
							gameObject.transform.localScale = localScale;
						}
					}
					async = SceneManager.UnloadSceneAsync(sc);
					yield return async;
					if (slr.requestCancelled)
					{
						if (slr.lightmapData != null && slr.importLightmaps)
						{
							GlobalLightingManager.singleton.RemoveLightmapData(slr.lightmapData);
							slr.lightmapData = null;
						}
						if (slr.lightProbesHolder != null && slr.importLightProbes)
						{
							GlobalLightingManager.singleton.RemoveLightProbesHolder(slr.lightProbesHolder);
							slr.lightProbesHolder = null;
						}
						yield break;
					}
					if (slr.callback != null)
					{
						slr.callback(slr);
					}
				}
			}
			yield break;
		}

		// Token: 0x060058AC RID: 22700 RVA: 0x00207BE3 File Offset: 0x00205FE3
		public static void QueueLoadSceneIntoTransform(AssetLoader.SceneLoadIntoTransformRequest slr)
		{
			if (AssetLoader.singleton != null)
			{
				if (AssetLoader.singleton.sceneLoadIntoTransformQueue == null)
				{
					AssetLoader.singleton.sceneLoadIntoTransformQueue = new List<AssetLoader.SceneLoadIntoTransformRequest>();
				}
				AssetLoader.singleton.sceneLoadIntoTransformQueue.Add(slr);
			}
		}

		// Token: 0x060058AD RID: 22701 RVA: 0x00207C24 File Offset: 0x00206024
		protected IEnumerator SceneLoadIntoTransfromQueueProcessor()
		{
			if (this.sceneLoadIntoTransformQueue == null)
			{
				this.sceneLoadIntoTransformQueue = new List<AssetLoader.SceneLoadIntoTransformRequest>();
			}
			for (;;)
			{
				yield return null;
				if (this.sceneLoadIntoTransformQueue.Count > 0)
				{
					AssetLoader.SceneLoadIntoTransformRequest slr = this.sceneLoadIntoTransformQueue[0];
					this.sceneLoadIntoTransformQueue.RemoveAt(0);
					yield return this.LoadSceneIntoTransformAsync(slr);
				}
			}
			yield break;
		}

		// Token: 0x060058AE RID: 22702 RVA: 0x00207C3F File Offset: 0x0020603F
		private void Awake()
		{
			AssetLoader.singleton = this;
		}

		// Token: 0x060058AF RID: 22703 RVA: 0x00207C47 File Offset: 0x00206047
		private void Start()
		{
			base.StartCoroutine(this.AssetBundleFromFileQueueProcessor());
			base.StartCoroutine(this.SceneLoadIntoTransfromQueueProcessor());
		}

		// Token: 0x060058B0 RID: 22704 RVA: 0x00207C64 File Offset: 0x00206064
		private void OnDestroy()
		{
			if (this.pathToAssetBundle != null)
			{
				foreach (AssetBundle assetBundle in this.pathToAssetBundle.Values)
				{
					assetBundle.Unload(true);
				}
			}
		}

		// Token: 0x040048FB RID: 18683
		protected static AssetLoader singleton;

		// Token: 0x040048FC RID: 18684
		protected Dictionary<string, int> assetBundleReferenceCounts;

		// Token: 0x040048FD RID: 18685
		protected Dictionary<string, AssetBundle> pathToAssetBundle;

		// Token: 0x040048FE RID: 18686
		protected List<AssetLoader.AssetBundleFromFileRequest> assetBundleFromFileQueue;

		// Token: 0x040048FF RID: 18687
		protected List<AssetLoader.SceneLoadIntoTransformRequest> sceneLoadIntoTransformQueue;

		// Token: 0x02000BFE RID: 3070
		// (Invoke) Token: 0x060058B2 RID: 22706
		public delegate void LoadBundleFromFileCallback(AssetLoader.AssetBundleFromFileRequest abffr);

		// Token: 0x02000BFF RID: 3071
		public class AssetBundleFromFileRequest
		{
			// Token: 0x060058B5 RID: 22709 RVA: 0x00207CD0 File Offset: 0x002060D0
			public AssetBundleFromFileRequest()
			{
			}

			// Token: 0x04004900 RID: 18688
			public string path;

			// Token: 0x04004901 RID: 18689
			public AssetLoader.LoadBundleFromFileCallback callback;

			// Token: 0x04004902 RID: 18690
			public AssetBundle assetBundle;
		}

		// Token: 0x02000C00 RID: 3072
		// (Invoke) Token: 0x060058B7 RID: 22711
		public delegate void LoadSceneIntoTransformCallback(AssetLoader.SceneLoadIntoTransformRequest slr);

		// Token: 0x02000C01 RID: 3073
		public class SceneLoadIntoTransformRequest
		{
			// Token: 0x060058BA RID: 22714 RVA: 0x00207CD8 File Offset: 0x002060D8
			public SceneLoadIntoTransformRequest()
			{
			}

			// Token: 0x04004903 RID: 18691
			public string scenePath;

			// Token: 0x04004904 RID: 18692
			public Transform transform;

			// Token: 0x04004905 RID: 18693
			public AssetLoader.LoadSceneIntoTransformCallback callback;

			// Token: 0x04004906 RID: 18694
			public bool importLightmaps;

			// Token: 0x04004907 RID: 18695
			public bool importLightProbes;

			// Token: 0x04004908 RID: 18696
			public LightmapData[] lightmapData;

			// Token: 0x04004909 RID: 18697
			public GlobalLightingManager.LightProbesHolder lightProbesHolder;

			// Token: 0x0400490A RID: 18698
			public bool requestCancelled;
		}

		// Token: 0x02000FF7 RID: 4087
		[CompilerGenerated]
		private sealed class <LoadBundleFileAsync>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007637 RID: 30263 RVA: 0x00207CE0 File Offset: 0x002060E0
			[DebuggerHidden]
			public <LoadBundleFileAsync>c__Iterator0()
			{
			}

			// Token: 0x06007638 RID: 30264 RVA: 0x00207CE8 File Offset: 0x002060E8
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					if (this.assetBundleReferenceCounts == null)
					{
						this.assetBundleReferenceCounts = new Dictionary<string, int>();
					}
					if (this.pathToAssetBundle == null)
					{
						this.pathToAssetBundle = new Dictionary<string, AssetBundle>();
					}
					path = abffr.path;
					if (this.assetBundleReferenceCounts.TryGetValue(path, out cnt))
					{
						cnt++;
						this.assetBundleReferenceCounts.Remove(path);
						this.assetBundleReferenceCounts.Add(path, cnt);
					}
					else
					{
						this.assetBundleReferenceCounts.Add(path, 1);
					}
					ab = null;
					if (this.pathToAssetBundle.TryGetValue(path, out ab))
					{
						goto IL_285;
					}
					abcr = null;
					if (FileManager.IsFileInPackage(path))
					{
						vfe = FileManager.GetVarFileEntry(path);
						if (!vfe.Simulated)
						{
							assetbundleBytes = new byte[vfe.Size];
							this.$current = FileManager.ReadAllBytesCoroutine(vfe, assetbundleBytes);
							if (!this.$disposing)
							{
								this.$PC = 1;
							}
							return true;
						}
						string path2 = vfe.Package.Path + "\\" + vfe.InternalPath;
						abcr = AssetBundle.LoadFromFileAsync(path2);
					}
					else
					{
						abcr = AssetBundle.LoadFromFileAsync(path);
					}
					break;
				case 1U:
					abcr = AssetBundle.LoadFromMemoryAsync(assetbundleBytes);
					break;
				case 2U:
					if (!abcr.assetBundle)
					{
						SuperController.LogError("Error during attempt to load assetbundle " + path + ". Not valid");
						goto IL_285;
					}
					ab = abcr.assetBundle;
					this.pathToAssetBundle.Add(path, abcr.assetBundle);
					goto IL_285;
				default:
					return false;
				}
				if (abcr != null)
				{
					this.$current = abcr;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_285:
				abffr.assetBundle = ab;
				if (abffr.callback != null)
				{
					abffr.callback(abffr);
				}
				this.$PC = -1;
				return false;
			}

			// Token: 0x17001171 RID: 4465
			// (get) Token: 0x06007639 RID: 30265 RVA: 0x00207FBB File Offset: 0x002063BB
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x17001172 RID: 4466
			// (get) Token: 0x0600763A RID: 30266 RVA: 0x00207FC3 File Offset: 0x002063C3
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x0600763B RID: 30267 RVA: 0x00207FCB File Offset: 0x002063CB
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x0600763C RID: 30268 RVA: 0x00207FDB File Offset: 0x002063DB
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x04006A04 RID: 27140
			internal AssetLoader.AssetBundleFromFileRequest abffr;

			// Token: 0x04006A05 RID: 27141
			internal string <path>__0;

			// Token: 0x04006A06 RID: 27142
			internal int <cnt>__0;

			// Token: 0x04006A07 RID: 27143
			internal AssetBundle <ab>__0;

			// Token: 0x04006A08 RID: 27144
			internal AssetBundleCreateRequest <abcr>__1;

			// Token: 0x04006A09 RID: 27145
			internal VarFileEntry <vfe>__2;

			// Token: 0x04006A0A RID: 27146
			internal byte[] <assetbundleBytes>__3;

			// Token: 0x04006A0B RID: 27147
			internal AssetLoader $this;

			// Token: 0x04006A0C RID: 27148
			internal object $current;

			// Token: 0x04006A0D RID: 27149
			internal bool $disposing;

			// Token: 0x04006A0E RID: 27150
			internal int $PC;
		}

		// Token: 0x02000FF8 RID: 4088
		[CompilerGenerated]
		private sealed class <AssetBundleFromFileQueueProcessor>c__Iterator1 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x0600763D RID: 30269 RVA: 0x00207FE2 File Offset: 0x002063E2
			[DebuggerHidden]
			public <AssetBundleFromFileQueueProcessor>c__Iterator1()
			{
			}

			// Token: 0x0600763E RID: 30270 RVA: 0x00207FEC File Offset: 0x002063EC
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					if (this.assetBundleFromFileQueue == null)
					{
						this.assetBundleFromFileQueue = new List<AssetLoader.AssetBundleFromFileRequest>();
					}
					break;
				case 1U:
					if (this.assetBundleFromFileQueue.Count > 0)
					{
						abffr = this.assetBundleFromFileQueue[0];
						this.assetBundleFromFileQueue.RemoveAt(0);
						this.$current = base.LoadBundleFileAsync(abffr);
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						return true;
					}
					break;
				case 2U:
					break;
				default:
					return false;
				}
				IL_45:
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
				goto IL_45;
			}

			// Token: 0x17001173 RID: 4467
			// (get) Token: 0x0600763F RID: 30271 RVA: 0x002080D1 File Offset: 0x002064D1
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x17001174 RID: 4468
			// (get) Token: 0x06007640 RID: 30272 RVA: 0x002080D9 File Offset: 0x002064D9
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007641 RID: 30273 RVA: 0x002080E1 File Offset: 0x002064E1
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007642 RID: 30274 RVA: 0x002080F1 File Offset: 0x002064F1
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x04006A0F RID: 27151
			internal AssetLoader.AssetBundleFromFileRequest <abffr>__1;

			// Token: 0x04006A10 RID: 27152
			internal AssetLoader $this;

			// Token: 0x04006A11 RID: 27153
			internal object $current;

			// Token: 0x04006A12 RID: 27154
			internal bool $disposing;

			// Token: 0x04006A13 RID: 27155
			internal int $PC;
		}

		// Token: 0x02000FF9 RID: 4089
		[CompilerGenerated]
		private sealed class <LoadSceneIntoTransformAsync>c__Iterator2 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007643 RID: 30275 RVA: 0x002080F8 File Offset: 0x002064F8
			[DebuggerHidden]
			public <LoadSceneIntoTransformAsync>c__Iterator2()
			{
			}

			// Token: 0x06007644 RID: 30276 RVA: 0x00208100 File Offset: 0x00206500
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					async = null;
					try
					{
						async = SceneManager.LoadSceneAsync(slr.scenePath, LoadSceneMode.Additive);
					}
					catch (Exception arg)
					{
						SuperController.LogError("Error during attempt to load scene: " + arg);
					}
					if (async != null)
					{
						this.$current = async;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					break;
				case 1U:
					sc = SceneManager.GetSceneByPath(slr.scenePath);
					if (sc.IsValid())
					{
						if (slr.requestCancelled)
						{
							async = SceneManager.UnloadSceneAsync(sc);
							this.$current = async;
							if (!this.$disposing)
							{
								this.$PC = 2;
							}
							return true;
						}
						newLightmapData = LightmapSettings.lightmaps;
						lightProbes = LightmapSettings.lightProbes;
						if (GlobalLightingManager.singleton != null)
						{
							if (GlobalLightingManager.singleton.PushLightmapData(newLightmapData))
							{
								slr.lightmapData = newLightmapData;
								if (!slr.importLightmaps)
								{
									GlobalLightingManager.singleton.RemoveLightmapData(slr.lightmapData);
								}
							}
							else
							{
								slr.lightmapData = null;
							}
							slr.lightProbesHolder = GlobalLightingManager.singleton.PushLightProbes(lightProbes);
							if (slr.lightProbesHolder != null && !slr.importLightProbes)
							{
								GlobalLightingManager.singleton.RemoveLightProbesHolder(slr.lightProbesHolder);
							}
						}
						if (slr.transform != null)
						{
							GameObject[] rootGameObjects = sc.GetRootGameObjects();
							foreach (GameObject gameObject in rootGameObjects)
							{
								Vector3 localPosition = gameObject.transform.localPosition;
								Quaternion localRotation = gameObject.transform.localRotation;
								Vector3 localScale = gameObject.transform.localScale;
								SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
								gameObject.transform.SetParent(slr.transform);
								gameObject.transform.localPosition = localPosition;
								gameObject.transform.localRotation = localRotation;
								gameObject.transform.localScale = localScale;
							}
						}
						async = SceneManager.UnloadSceneAsync(sc);
						this.$current = async;
						if (!this.$disposing)
						{
							this.$PC = 3;
						}
						return true;
					}
					break;
				case 2U:
					return false;
				case 3U:
					if (slr.requestCancelled)
					{
						if (slr.lightmapData != null && slr.importLightmaps)
						{
							GlobalLightingManager.singleton.RemoveLightmapData(slr.lightmapData);
							slr.lightmapData = null;
						}
						if (slr.lightProbesHolder != null && slr.importLightProbes)
						{
							GlobalLightingManager.singleton.RemoveLightProbesHolder(slr.lightProbesHolder);
							slr.lightProbesHolder = null;
						}
						return false;
					}
					if (slr.callback != null)
					{
						slr.callback(slr);
					}
					break;
				default:
					return false;
				}
				this.$PC = -1;
				return false;
			}

			// Token: 0x17001175 RID: 4469
			// (get) Token: 0x06007645 RID: 30277 RVA: 0x00208494 File Offset: 0x00206894
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x17001176 RID: 4470
			// (get) Token: 0x06007646 RID: 30278 RVA: 0x0020849C File Offset: 0x0020689C
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007647 RID: 30279 RVA: 0x002084A4 File Offset: 0x002068A4
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007648 RID: 30280 RVA: 0x002084B4 File Offset: 0x002068B4
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x04006A14 RID: 27156
			internal AsyncOperation <async>__0;

			// Token: 0x04006A15 RID: 27157
			internal AssetLoader.SceneLoadIntoTransformRequest slr;

			// Token: 0x04006A16 RID: 27158
			internal Scene <sc>__1;

			// Token: 0x04006A17 RID: 27159
			internal LightmapData[] <newLightmapData>__2;

			// Token: 0x04006A18 RID: 27160
			internal LightProbes <lightProbes>__2;

			// Token: 0x04006A19 RID: 27161
			internal object $current;

			// Token: 0x04006A1A RID: 27162
			internal bool $disposing;

			// Token: 0x04006A1B RID: 27163
			internal int $PC;
		}

		// Token: 0x02000FFA RID: 4090
		[CompilerGenerated]
		private sealed class <SceneLoadIntoTransfromQueueProcessor>c__Iterator3 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007649 RID: 30281 RVA: 0x002084BB File Offset: 0x002068BB
			[DebuggerHidden]
			public <SceneLoadIntoTransfromQueueProcessor>c__Iterator3()
			{
			}

			// Token: 0x0600764A RID: 30282 RVA: 0x002084C4 File Offset: 0x002068C4
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					if (this.sceneLoadIntoTransformQueue == null)
					{
						this.sceneLoadIntoTransformQueue = new List<AssetLoader.SceneLoadIntoTransformRequest>();
					}
					break;
				case 1U:
					if (this.sceneLoadIntoTransformQueue.Count > 0)
					{
						slr = this.sceneLoadIntoTransformQueue[0];
						this.sceneLoadIntoTransformQueue.RemoveAt(0);
						this.$current = base.LoadSceneIntoTransformAsync(slr);
						if (!this.$disposing)
						{
							this.$PC = 2;
						}
						return true;
					}
					break;
				case 2U:
					break;
				default:
					return false;
				}
				IL_45:
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
				goto IL_45;
			}

			// Token: 0x17001177 RID: 4471
			// (get) Token: 0x0600764B RID: 30283 RVA: 0x002085A9 File Offset: 0x002069A9
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x17001178 RID: 4472
			// (get) Token: 0x0600764C RID: 30284 RVA: 0x002085B1 File Offset: 0x002069B1
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x0600764D RID: 30285 RVA: 0x002085B9 File Offset: 0x002069B9
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x0600764E RID: 30286 RVA: 0x002085C9 File Offset: 0x002069C9
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x04006A1C RID: 27164
			internal AssetLoader.SceneLoadIntoTransformRequest <slr>__1;

			// Token: 0x04006A1D RID: 27165
			internal AssetLoader $this;

			// Token: 0x04006A1E RID: 27166
			internal object $current;

			// Token: 0x04006A1F RID: 27167
			internal bool $disposing;

			// Token: 0x04006A20 RID: 27168
			internal int $PC;
		}
	}
}
