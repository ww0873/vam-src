using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Battlehub.RTCommon;
using Battlehub.RTSaveLoad.PersistentObjects;
using Battlehub.Utils;
using UnityEngine;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000227 RID: 551
	[ExecuteInEditMode]
	public class ProjectManager : RuntimeSceneManager, IProjectManager, ISceneManager
	{
		// Token: 0x06000B0B RID: 2827 RVA: 0x00044C0A File Offset: 0x0004300A
		public ProjectManager()
		{
		}

		// Token: 0x1400002E RID: 46
		// (add) Token: 0x06000B0C RID: 2828 RVA: 0x00044C28 File Offset: 0x00043028
		// (remove) Token: 0x06000B0D RID: 2829 RVA: 0x00044C60 File Offset: 0x00043060
		public event EventHandler ProjectLoading
		{
			add
			{
				EventHandler eventHandler = this.ProjectLoading;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.ProjectLoading, (EventHandler)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler eventHandler = this.ProjectLoading;
				EventHandler eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler>(ref this.ProjectLoading, (EventHandler)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x1400002F RID: 47
		// (add) Token: 0x06000B0E RID: 2830 RVA: 0x00044C98 File Offset: 0x00043098
		// (remove) Token: 0x06000B0F RID: 2831 RVA: 0x00044CD0 File Offset: 0x000430D0
		public event EventHandler<ProjectManagerEventArgs> ProjectLoaded
		{
			add
			{
				EventHandler<ProjectManagerEventArgs> eventHandler = this.ProjectLoaded;
				EventHandler<ProjectManagerEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ProjectManagerEventArgs>>(ref this.ProjectLoaded, (EventHandler<ProjectManagerEventArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<ProjectManagerEventArgs> eventHandler = this.ProjectLoaded;
				EventHandler<ProjectManagerEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ProjectManagerEventArgs>>(ref this.ProjectLoaded, (EventHandler<ProjectManagerEventArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x14000030 RID: 48
		// (add) Token: 0x06000B10 RID: 2832 RVA: 0x00044D08 File Offset: 0x00043108
		// (remove) Token: 0x06000B11 RID: 2833 RVA: 0x00044D40 File Offset: 0x00043140
		public event EventHandler<ProjectManagerEventArgs> BundledResourcesAdded
		{
			add
			{
				EventHandler<ProjectManagerEventArgs> eventHandler = this.BundledResourcesAdded;
				EventHandler<ProjectManagerEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ProjectManagerEventArgs>>(ref this.BundledResourcesAdded, (EventHandler<ProjectManagerEventArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<ProjectManagerEventArgs> eventHandler = this.BundledResourcesAdded;
				EventHandler<ProjectManagerEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ProjectManagerEventArgs>>(ref this.BundledResourcesAdded, (EventHandler<ProjectManagerEventArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x14000031 RID: 49
		// (add) Token: 0x06000B12 RID: 2834 RVA: 0x00044D78 File Offset: 0x00043178
		// (remove) Token: 0x06000B13 RID: 2835 RVA: 0x00044DB0 File Offset: 0x000431B0
		public event EventHandler<ProjectManagerEventArgs> DynamicResourcesAdded
		{
			add
			{
				EventHandler<ProjectManagerEventArgs> eventHandler = this.DynamicResourcesAdded;
				EventHandler<ProjectManagerEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ProjectManagerEventArgs>>(ref this.DynamicResourcesAdded, (EventHandler<ProjectManagerEventArgs>)Delegate.Combine(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
			remove
			{
				EventHandler<ProjectManagerEventArgs> eventHandler = this.DynamicResourcesAdded;
				EventHandler<ProjectManagerEventArgs> eventHandler2;
				do
				{
					eventHandler2 = eventHandler;
					eventHandler = Interlocked.CompareExchange<EventHandler<ProjectManagerEventArgs>>(ref this.DynamicResourcesAdded, (EventHandler<ProjectManagerEventArgs>)Delegate.Remove(eventHandler2, value), eventHandler);
				}
				while (eventHandler != eventHandler2);
			}
		}

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x06000B14 RID: 2836 RVA: 0x00044DE6 File Offset: 0x000431E6
		public ProjectItem Project
		{
			get
			{
				if (this.m_root == null)
				{
					return null;
				}
				return this.m_root.Item;
			}
		}

		// Token: 0x06000B15 RID: 2837 RVA: 0x00044E00 File Offset: 0x00043200
		protected override void AwakeOverride()
		{
			base.AwakeOverride();
			this.m_bundleLoader = Dependencies.BundleLoader;
			if (this.m_dynamicResourcesRoot == null)
			{
				this.m_dynamicResourcesRoot = base.transform;
			}
		}

		// Token: 0x06000B16 RID: 2838 RVA: 0x00044E30 File Offset: 0x00043230
		protected override void OnDestroyOverride()
		{
			base.OnDestroyOverride();
			if (this.m_dynamicResources != null)
			{
				this.DestroyDynamicResources();
			}
			if (this.m_loadedBundles != null)
			{
				this.UnloadAssetBundles();
			}
			this.m_loadedResources = null;
		}

		// Token: 0x06000B17 RID: 2839 RVA: 0x00044E61 File Offset: 0x00043261
		private void OnApplicationQuit()
		{
			this.m_loadedResources = null;
			this.m_dynamicResources = null;
			this.m_loadedBundles = null;
		}

		// Token: 0x06000B18 RID: 2840 RVA: 0x00044E78 File Offset: 0x00043278
		private void UnloadAssetBundles()
		{
			foreach (KeyValuePair<string, ProjectManager.LoadedAssetBundle> keyValuePair in this.m_loadedBundles)
			{
				if (keyValuePair.Value.Bundle != null)
				{
					RuntimeShaderUtil.RemoveExtra(keyValuePair.Key);
					IdentifiersMap.Instance.Unregister(keyValuePair.Value.Bundle);
					keyValuePair.Value.Bundle.Unload(true);
				}
			}
		}

		// Token: 0x06000B19 RID: 2841 RVA: 0x00044F18 File Offset: 0x00043318
		private void DestroyDynamicResources()
		{
			foreach (KeyValuePair<long, UnityEngine.Object> keyValuePair in this.m_dynamicResources)
			{
				long key = keyValuePair.Key;
				IdentifiersMap.Instance.Unregister(key);
				this.m_loadedResources.Remove(key);
				UnityEngine.Object value = keyValuePair.Value;
				if (value)
				{
					UnityEngine.Object.Destroy(value);
				}
			}
			this.m_dynamicResources.Clear();
		}

		// Token: 0x06000B1A RID: 2842 RVA: 0x00044FB4 File Offset: 0x000433B4
		private void EnumerateBundles(ProjectItem item)
		{
			if (!string.IsNullOrEmpty(item.BundleName))
			{
				ProjectManager.LoadedAssetBundle loadedAssetBundle;
				if (!this.m_loadedBundles.TryGetValue(item.BundleName, out loadedAssetBundle))
				{
					loadedAssetBundle = new ProjectManager.LoadedAssetBundle
					{
						Usages = 1
					};
					this.m_loadedBundles.Add(item.BundleName, loadedAssetBundle);
				}
				else
				{
					loadedAssetBundle.Usages++;
				}
			}
			if (item.Children != null)
			{
				for (int i = 0; i < item.Children.Count; i++)
				{
					this.EnumerateBundles(item.Children[i]);
				}
			}
		}

		// Token: 0x06000B1B RID: 2843 RVA: 0x00045058 File Offset: 0x00043458
		private void LoadBundles(Action callback)
		{
			ProjectManager.<LoadBundles>c__AnonStorey1 <LoadBundles>c__AnonStorey = new ProjectManager.<LoadBundles>c__AnonStorey1();
			<LoadBundles>c__AnonStorey.callback = callback;
			<LoadBundles>c__AnonStorey.$this = this;
			<LoadBundles>c__AnonStorey.loading = this.m_loadedBundles.Count;
			if (<LoadBundles>c__AnonStorey.loading == 0 && <LoadBundles>c__AnonStorey.callback != null)
			{
				<LoadBundles>c__AnonStorey.callback();
			}
			using (Dictionary<string, ProjectManager.LoadedAssetBundle>.Enumerator enumerator = this.m_loadedBundles.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ProjectManager.<LoadBundles>c__AnonStorey0 <LoadBundles>c__AnonStorey2 = new ProjectManager.<LoadBundles>c__AnonStorey0();
					<LoadBundles>c__AnonStorey2.<>f__ref$1 = <LoadBundles>c__AnonStorey;
					<LoadBundles>c__AnonStorey2.kvp = enumerator.Current;
					this.m_bundleLoader.Load(<LoadBundles>c__AnonStorey2.kvp.Key, new AssetBundleEventHandler(<LoadBundles>c__AnonStorey2.<>m__0));
				}
			}
		}

		// Token: 0x06000B1C RID: 2844 RVA: 0x00045128 File Offset: 0x00043528
		private static TextAsset[] LoadAllTextAssets(AssetBundle bundle)
		{
			string[] allAssetNames = bundle.GetAllAssetNames();
			List<TextAsset> list = new List<TextAsset>();
			foreach (string text in allAssetNames)
			{
				if (text.EndsWith(".txt"))
				{
					list.Add(bundle.LoadAsset<TextAsset>(text));
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000B1D RID: 2845 RVA: 0x00045181 File Offset: 0x00043581
		public bool IsResource(UnityEngine.Object obj)
		{
			return !IdentifiersMap.IsNotMapped(obj);
		}

		// Token: 0x06000B1E RID: 2846 RVA: 0x0004518C File Offset: 0x0004358C
		public ID GetID(UnityEngine.Object obj)
		{
			return new ID(obj.GetMappedInstanceID());
		}

		// Token: 0x06000B1F RID: 2847 RVA: 0x0004519C File Offset: 0x0004359C
		public void LoadProject(string projectName, ProjectManagerCallback<ProjectItem> callback)
		{
			ProjectManager.<LoadProject>c__AnonStorey2 <LoadProject>c__AnonStorey = new ProjectManager.<LoadProject>c__AnonStorey2();
			<LoadProject>c__AnonStorey.projectName = projectName;
			<LoadProject>c__AnonStorey.callback = callback;
			<LoadProject>c__AnonStorey.$this = this;
			this.m_isProjectLoaded = false;
			if (this.ProjectLoading != null)
			{
				this.ProjectLoading(this, EventArgs.Empty);
			}
			this.UnloadAssetBundles();
			this.DestroyDynamicResources();
			IJob job = Dependencies.Job;
			job.Submit(new Action<Action>(<LoadProject>c__AnonStorey.<>m__0), new Action(<LoadProject>c__AnonStorey.<>m__1));
		}

		// Token: 0x06000B20 RID: 2848 RVA: 0x00045218 File Offset: 0x00043618
		private void ContinueLoadingProject(ProjectManagerCallback callback, ProjectItem newTemplateFolder, ProjectItem existingTemplateFolder)
		{
			ProjectManager.<ContinueLoadingProject>c__AnonStorey4 <ContinueLoadingProject>c__AnonStorey = new ProjectManager.<ContinueLoadingProject>c__AnonStorey4();
			<ContinueLoadingProject>c__AnonStorey.callback = callback;
			<ContinueLoadingProject>c__AnonStorey.$this = this;
			List<ProjectItem> list = new List<ProjectItem>();
			if (existingTemplateFolder != null)
			{
				ProjectManager.MergeData(newTemplateFolder, existingTemplateFolder);
				ProjectManager.Diff(newTemplateFolder, existingTemplateFolder, list);
				this.m_root.Item = newTemplateFolder;
			}
			else
			{
				this.m_root.Item = newTemplateFolder;
			}
			this.m_project.Delete(list.ToArray(), new ProjectEventHandler(<ContinueLoadingProject>c__AnonStorey.<>m__0));
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x00045290 File Offset: 0x00043690
		private void CompleteProjectLoading(ProjectManagerCallback callback)
		{
			bool includeDynamicResources = false;
			Dictionary<long, UnityEngine.Object> allResources = IdentifiersMap.FindResources(includeDynamicResources);
			bool allowNulls = false;
			this.m_loadedResources = new Dictionary<long, UnityEngine.Object>();
			ProjectManager.FindDependencies(this.m_root.Item, this.m_loadedResources, allResources, allowNulls);
			ProjectManager.FindReferencedObjects(this.m_root.Item, this.m_loadedResources, allResources, allowNulls);
			this.m_project.UnloadData(this.m_root.Item);
			this.m_isProjectLoaded = true;
			callback();
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x00045308 File Offset: 0x00043708
		public void IgnoreTypes(params Type[] types)
		{
			for (int i = 0; i < types.Length; i++)
			{
				PersistentDescriptor.IgnoreTypes.Add(types[i]);
			}
		}

		// Token: 0x06000B23 RID: 2851 RVA: 0x00045338 File Offset: 0x00043738
		public override void SaveScene(ProjectItem scene, ProjectManagerCallback callback)
		{
			ProjectManager.<SaveScene>c__AnonStorey5 <SaveScene>c__AnonStorey = new ProjectManager.<SaveScene>c__AnonStorey5();
			<SaveScene>c__AnonStorey.scene = scene;
			if (!this.m_isProjectLoaded)
			{
				throw new InvalidOperationException("project is not loaded");
			}
			if (!<SaveScene>c__AnonStorey.scene.IsScene)
			{
				throw new ArgumentException("is not a scene", "scene");
			}
			if (<SaveScene>c__AnonStorey.scene.Parent == null)
			{
				throw new ArgumentException("Scene does not have parent", "scene");
			}
			if (<SaveScene>c__AnonStorey.scene.Parent.Children.Where(new Func<ProjectItem, bool>(<SaveScene>c__AnonStorey.<>m__0)).Count<ProjectItem>() > 1)
			{
				throw new ArgumentException("Scene with same name exists", "scene");
			}
			base.SaveScene(<SaveScene>c__AnonStorey.scene, callback);
		}

		// Token: 0x06000B24 RID: 2852 RVA: 0x000453F4 File Offset: 0x000437F4
		public override void LoadScene(ProjectItem scene, ProjectManagerCallback callback)
		{
			ProjectManager.<LoadScene>c__AnonStorey6 <LoadScene>c__AnonStorey = new ProjectManager.<LoadScene>c__AnonStorey6();
			<LoadScene>c__AnonStorey.scene = scene;
			<LoadScene>c__AnonStorey.callback = callback;
			<LoadScene>c__AnonStorey.$this = this;
			if (!this.m_isProjectLoaded)
			{
				throw new InvalidOperationException("project is not loaded");
			}
			if (!<LoadScene>c__AnonStorey.scene.IsScene)
			{
				throw new ArgumentException("is not a scene", "scene");
			}
			this.DestroyDynamicResources();
			base.RaiseSceneLoading(<LoadScene>c__AnonStorey.scene);
			<LoadScene>c__AnonStorey.isEnabled = RuntimeUndo.Enabled;
			RuntimeUndo.Enabled = false;
			RuntimeSelection.objects = null;
			RuntimeUndo.Enabled = <LoadScene>c__AnonStorey.isEnabled;
			bool includeDynamicResources = false;
			<LoadScene>c__AnonStorey.allObjects = IdentifiersMap.FindResources(includeDynamicResources);
			<LoadScene>c__AnonStorey.sceneDependencies = new Dictionary<long, UnityEngine.Object>();
			bool dynamicOnly = true;
			<LoadScene>c__AnonStorey.idToProjectItem = ProjectManager.GetIdToProjectItemMapping(this.m_root.Item, dynamicOnly);
			this.m_project.LoadData(new ProjectItem[]
			{
				<LoadScene>c__AnonStorey.scene
			}, new ProjectEventHandler<ProjectItem[]>(<LoadScene>c__AnonStorey.<>m__0), new int[0]);
		}

		// Token: 0x06000B25 RID: 2853 RVA: 0x000454E4 File Offset: 0x000438E4
		public override void CreateScene()
		{
			this.DestroyDynamicResources();
			base.CreateScene();
		}

		// Token: 0x06000B26 RID: 2854 RVA: 0x000454F2 File Offset: 0x000438F2
		public void AddBundledResources(ProjectItem folder, string bundleName, Func<UnityEngine.Object, string, bool> filter, ProjectManagerCallback<ProjectItem[]> callback)
		{
			this.AddBundledResources(folder, bundleName, null, null, filter, callback);
		}

		// Token: 0x06000B27 RID: 2855 RVA: 0x00045501 File Offset: 0x00043901
		public void AddBundledResource(ProjectItem folder, string bundleName, string assetName, ProjectManagerCallback<ProjectItem[]> callback)
		{
			string[] assetNames = new string[]
			{
				assetName
			};
			Type[] assetTypes = null;
			if (ProjectManager.<>f__am$cache0 == null)
			{
				ProjectManager.<>f__am$cache0 = new Func<UnityEngine.Object, string, bool>(ProjectManager.<AddBundledResource>m__0);
			}
			this.AddBundledResources(folder, bundleName, assetNames, assetTypes, ProjectManager.<>f__am$cache0, callback);
		}

		// Token: 0x06000B28 RID: 2856 RVA: 0x00045535 File Offset: 0x00043935
		public void AddBundledResource<T>(ProjectItem folder, string bundleName, string assetName, ProjectManagerCallback<ProjectItem[]> callback)
		{
			this.AddBundledResources(folder, bundleName, new string[]
			{
				assetName
			}, new Type[]
			{
				typeof(T)
			}, new Func<UnityEngine.Object, string, bool>(ProjectManager.<AddBundledResource`1>m__1<T>), callback);
		}

		// Token: 0x06000B29 RID: 2857 RVA: 0x0004556A File Offset: 0x0004396A
		public void AddBundledResource(ProjectItem folder, string bundleName, string assetName, Type assetType, ProjectManagerCallback<ProjectItem[]> callback)
		{
			string[] assetNames = new string[]
			{
				assetName
			};
			Type[] assetTypes = new Type[]
			{
				assetType
			};
			if (ProjectManager.<>f__am$cache1 == null)
			{
				ProjectManager.<>f__am$cache1 = new Func<UnityEngine.Object, string, bool>(ProjectManager.<AddBundledResource>m__2);
			}
			this.AddBundledResources(folder, bundleName, assetNames, assetTypes, ProjectManager.<>f__am$cache1, callback);
		}

		// Token: 0x06000B2A RID: 2858 RVA: 0x000455A8 File Offset: 0x000439A8
		public void AddBundledResources(ProjectItem folder, string bundleName, string[] assetNames, ProjectManagerCallback<ProjectItem[]> callback)
		{
			Type[] assetTypes = null;
			if (ProjectManager.<>f__am$cache2 == null)
			{
				ProjectManager.<>f__am$cache2 = new Func<UnityEngine.Object, string, bool>(ProjectManager.<AddBundledResources>m__3);
			}
			this.AddBundledResources(folder, bundleName, assetNames, assetTypes, ProjectManager.<>f__am$cache2, callback);
		}

		// Token: 0x06000B2B RID: 2859 RVA: 0x000455D3 File Offset: 0x000439D3
		private void LoadAssetBundle(string bundleName, AssetBundleEventHandler callback)
		{
			if (this.m_loadedBundles.ContainsKey(bundleName))
			{
				callback(bundleName, this.m_loadedBundles[bundleName].Bundle);
			}
			else
			{
				this.m_bundleLoader.Load(bundleName, callback);
			}
		}

		// Token: 0x06000B2C RID: 2860 RVA: 0x00045610 File Offset: 0x00043A10
		public void AddBundledResources(ProjectItem folder, string bundleName, string[] assetNames, Type[] assetTypes, Func<UnityEngine.Object, string, bool> filter, ProjectManagerCallback<ProjectItem[]> callback)
		{
			ProjectManager.<AddBundledResources>c__AnonStorey8 <AddBundledResources>c__AnonStorey = new ProjectManager.<AddBundledResources>c__AnonStorey8();
			<AddBundledResources>c__AnonStorey.assetNames = assetNames;
			<AddBundledResources>c__AnonStorey.assetTypes = assetTypes;
			<AddBundledResources>c__AnonStorey.filter = filter;
			<AddBundledResources>c__AnonStorey.bundleName = bundleName;
			<AddBundledResources>c__AnonStorey.folder = folder;
			<AddBundledResources>c__AnonStorey.callback = callback;
			<AddBundledResources>c__AnonStorey.$this = this;
			if (!this.m_isProjectLoaded)
			{
				throw new InvalidOperationException("project is not loaded");
			}
			if (string.IsNullOrEmpty(<AddBundledResources>c__AnonStorey.bundleName))
			{
				throw new ArgumentException("bandle name is not specified", "bundleName");
			}
			if (<AddBundledResources>c__AnonStorey.assetNames != null && <AddBundledResources>c__AnonStorey.assetNames.Length > 0)
			{
				if (<AddBundledResources>c__AnonStorey.assetNames.Length != <AddBundledResources>c__AnonStorey.assetNames.Distinct<string>().Count<string>())
				{
					throw new ArgumentException("assetNames array contains duplicates", "assetNames");
				}
				if (<AddBundledResources>c__AnonStorey.assetTypes == null)
				{
					<AddBundledResources>c__AnonStorey.assetTypes = new Type[<AddBundledResources>c__AnonStorey.assetNames.Length];
				}
				if (<AddBundledResources>c__AnonStorey.assetNames.Length != <AddBundledResources>c__AnonStorey.assetTypes.Length)
				{
					throw new ArgumentException("asset types array should be of same size as the asset names array", "assetTypes");
				}
			}
			this.LoadAssetBundle(<AddBundledResources>c__AnonStorey.bundleName, new AssetBundleEventHandler(<AddBundledResources>c__AnonStorey.<>m__0));
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x00045730 File Offset: 0x00043B30
		private bool AddBundledResourceInternalFilter(UnityEngine.Object obj, string assetName)
		{
			if (obj == null)
			{
				return false;
			}
			if (assetName.Contains("resourcemap"))
			{
				return false;
			}
			if (assetName.StartsWith("rt_shader"))
			{
				return false;
			}
			if (obj is TextAsset)
			{
				return false;
			}
			if (obj is Shader)
			{
				if (!obj.HasMappedInstanceID())
				{
					UnityEngine.Debug.LogWarningFormat("Shader {0} can't be added as bundled resource. Please consider adding it to main ResourceMap or bundle ResourceMap", new object[]
					{
						obj.name
					});
				}
				return false;
			}
			return (obj is GameObject || obj is Mesh || obj is Material || obj is Texture) && PersistentData.CanCreate(obj);
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x000457E1 File Offset: 0x00043BE1
		public void AddDynamicResource(ProjectItem folder, UnityEngine.Object obj, ProjectManagerCallback<ProjectItem[]> callback)
		{
			UnityEngine.Object[] objects = new UnityEngine.Object[]
			{
				obj
			};
			bool includingDependencies = false;
			if (ProjectManager.<>f__am$cache3 == null)
			{
				ProjectManager.<>f__am$cache3 = new Func<UnityEngine.Object, bool>(ProjectManager.<AddDynamicResource>m__4);
			}
			this.AddDynamicResources(folder, objects, includingDependencies, ProjectManager.<>f__am$cache3, callback);
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x00045813 File Offset: 0x00043C13
		public void AddDynamicResources(ProjectItem folder, UnityEngine.Object[] objects, ProjectManagerCallback<ProjectItem[]> callback)
		{
			bool includingDependencies = false;
			if (ProjectManager.<>f__am$cache4 == null)
			{
				ProjectManager.<>f__am$cache4 = new Func<UnityEngine.Object, bool>(ProjectManager.<AddDynamicResources>m__5);
			}
			this.AddDynamicResources(folder, objects, includingDependencies, ProjectManager.<>f__am$cache4, callback);
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x0004583C File Offset: 0x00043C3C
		public void AddDynamicResource(ProjectItem folder, UnityEngine.Object obj, bool includingDependencies, Func<UnityEngine.Object, bool> filter, ProjectManagerCallback<ProjectItem[]> callback)
		{
			this.AddDynamicResources(folder, new UnityEngine.Object[]
			{
				obj
			}, includingDependencies, filter, callback);
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x00045854 File Offset: 0x00043C54
		public void AddDynamicResources(ProjectItem folder, UnityEngine.Object[] objects, bool includingDependencies, Func<UnityEngine.Object, bool> filter, ProjectManagerCallback<ProjectItem[]> callback)
		{
			ProjectManager.<AddDynamicResources>c__AnonStoreyB <AddDynamicResources>c__AnonStoreyB = new ProjectManager.<AddDynamicResources>c__AnonStoreyB();
			<AddDynamicResources>c__AnonStoreyB.callback = callback;
			<AddDynamicResources>c__AnonStoreyB.$this = this;
			if (!this.m_isProjectLoaded)
			{
				throw new InvalidOperationException("project is not loaded");
			}
			if (objects == null || objects.Length == 0)
			{
				throw new ArgumentNullException("objects array is null or empty", "objects");
			}
			if (objects.Distinct<UnityEngine.Object>().Count<UnityEngine.Object>() != objects.Length)
			{
				throw new ArgumentException("same object included to objects array multiple times", "objects");
			}
			Dictionary<long, UnityEngine.Object> dictionary = new Dictionary<long, UnityEngine.Object>();
			if (includingDependencies)
			{
				HashSet<UnityEngine.Object> hashSet = new HashSet<UnityEngine.Object>();
				while (objects.Length > 0)
				{
					for (int i = 0; i < objects.Length; i++)
					{
						UnityEngine.Object @object = objects[i];
						if (this.AddDynamicResourceInternalFilter(@object) && filter(@object))
						{
							if (!hashSet.Contains(@object))
							{
								hashSet.Add(@object);
							}
						}
						else
						{
							objects[i] = null;
						}
					}
					Dictionary<long, UnityEngine.Object> dictionary2 = new Dictionary<long, UnityEngine.Object>();
					foreach (UnityEngine.Object object2 in objects)
					{
						if (object2 != null)
						{
							this.GetDependencies(object2, dictionary2);
						}
					}
					objects = dictionary2.Values.ToArray<UnityEngine.Object>();
					foreach (KeyValuePair<long, UnityEngine.Object> keyValuePair in dictionary2)
					{
						if (!dictionary.ContainsKey(keyValuePair.Key))
						{
							dictionary.Add(keyValuePair.Key, keyValuePair.Value);
						}
					}
				}
				objects = hashSet.ToArray<UnityEngine.Object>();
			}
			else
			{
				for (int k = 0; k < objects.Length; k++)
				{
					UnityEngine.Object object3 = objects[k];
					if (this.AddDynamicResourceInternalFilter(object3) && filter(object3))
					{
						this.GetDependencies(object3, dictionary);
					}
					else
					{
						objects[k] = null;
					}
				}
				IEnumerable<UnityEngine.Object> source = objects;
				if (ProjectManager.<>f__am$cache5 == null)
				{
					ProjectManager.<>f__am$cache5 = new Func<UnityEngine.Object, bool>(ProjectManager.<AddDynamicResources>m__6);
				}
				objects = source.Where(ProjectManager.<>f__am$cache5).ToArray<UnityEngine.Object>();
			}
			foreach (UnityEngine.Object object4 in objects)
			{
				if (object4 is Component)
				{
					Component component = (Component)object4;
					long mappedInstanceID = component.gameObject.GetMappedInstanceID();
					if (!dictionary.ContainsKey(mappedInstanceID))
					{
						dictionary.Add(mappedInstanceID, component.gameObject);
					}
				}
			}
			Dictionary<long, UnityEngine.Object> dictionary3 = new Dictionary<long, UnityEngine.Object>();
			this.DuplicateAndRegister(objects, dictionary3);
			for (int m = 0; m < objects.Length; m++)
			{
				UnityEngine.Object object5 = objects[m];
				if (object5 is Component)
				{
					Component component2 = (Component)object5;
					objects[m] = component2.gameObject;
				}
			}
			foreach (UnityEngine.Object object6 in objects)
			{
				object6.name = ProjectItem.GetUniqueName(object6.name, object6, folder);
			}
			<AddDynamicResources>c__AnonStoreyB.projectItems = ProjectManager.ConvertObjectsToProjectItems(objects, false, null, null, null);
			foreach (UnityEngine.Object obj in objects)
			{
				this.GetReferredObjects(obj, dictionary);
			}
			foreach (KeyValuePair<long, UnityEngine.Object> keyValuePair2 in dictionary3)
			{
				if (dictionary.ContainsKey(keyValuePair2.Key))
				{
					dictionary[keyValuePair2.Key] = keyValuePair2.Value;
				}
			}
			for (int num2 = 0; num2 < <AddDynamicResources>c__AnonStoreyB.projectItems.Length; num2++)
			{
				ProjectItem projectItem = <AddDynamicResources>c__AnonStoreyB.projectItems[num2];
				PersistentData.RestoreDataAndResolveDependencies(projectItem.Internal_Data.PersistentData, dictionary);
			}
			ProjectItem[] array = ProjectManager.ConvertObjectsToProjectItems(objects, false, null, null, null);
			for (int num3 = 0; num3 < array.Length; num3++)
			{
				array[num3].Internal_Data.RawData = <AddDynamicResources>c__AnonStoreyB.projectItems[num3].Internal_Data.RawData;
			}
			<AddDynamicResources>c__AnonStoreyB.projectItems = array;
			ProjectManager.<AddDynamicResources>c__AnonStoreyB <AddDynamicResources>c__AnonStoreyB2 = <AddDynamicResources>c__AnonStoreyB;
			IEnumerable<ProjectItem> projectItems = <AddDynamicResources>c__AnonStoreyB.projectItems;
			if (ProjectManager.<>f__am$cache6 == null)
			{
				ProjectManager.<>f__am$cache6 = new Func<ProjectItem, string>(ProjectManager.<AddDynamicResources>m__7);
			}
			<AddDynamicResources>c__AnonStoreyB2.projectItems = projectItems.OrderBy(ProjectManager.<>f__am$cache6).ToArray<ProjectItem>();
			for (int num4 = 0; num4 < <AddDynamicResources>c__AnonStoreyB.projectItems.Length; num4++)
			{
				ProjectItem item = <AddDynamicResources>c__AnonStoreyB.projectItems[num4];
				folder.AddChild(item);
			}
			this.m_project.Save(<AddDynamicResources>c__AnonStoreyB.projectItems, false, new ProjectEventHandler(<AddDynamicResources>c__AnonStoreyB.<>m__0));
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x00045D04 File Offset: 0x00044104
		private bool AddDynamicResourceInternalFilter(UnityEngine.Object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (obj is Texture2D)
			{
				Texture2D texture2D = (Texture2D)obj;
				bool flag = texture2D.IsReadable();
				if (!flag)
				{
					UnityEngine.Debug.LogWarningFormat("Texture {0} can't be added as dynamic resource. Please consider adding it to main ResourceMap or bundle ResourceMap", new object[]
					{
						texture2D.name
					});
				}
				return flag;
			}
			if (obj is Shader)
			{
				if (!obj.HasMappedInstanceID())
				{
					UnityEngine.Debug.LogWarningFormat("Shader {0} can't be added as dynamic resource. Please consider adding it to main ResourceMap or bundle ResourceMap", new object[]
					{
						obj.name
					});
				}
				return false;
			}
			return PersistentData.CanCreate(obj);
		}

		// Token: 0x06000B33 RID: 2867 RVA: 0x00045D90 File Offset: 0x00044190
		private void GetDependencies(UnityEngine.Object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			PersistentData persistentData = PersistentData.Create(obj);
			if (persistentData == null)
			{
				return;
			}
			persistentData.GetDependencies(obj, objects);
			if (obj is GameObject)
			{
				GameObject gameObject = (GameObject)obj;
				foreach (Component component in gameObject.GetComponents<Component>())
				{
					if (component != null)
					{
						persistentData = PersistentData.Create(component);
						persistentData.GetDependencies(component, objects);
					}
				}
				IEnumerator enumerator = gameObject.transform.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj2 = enumerator.Current;
						Transform transform = (Transform)obj2;
						this.GetDependencies(transform.gameObject, objects);
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
			}
		}

		// Token: 0x06000B34 RID: 2868 RVA: 0x00045E6C File Offset: 0x0004426C
		private void GetReferredObjects(UnityEngine.Object obj, Dictionary<long, UnityEngine.Object> objects)
		{
			long mappedInstanceID = obj.GetMappedInstanceID();
			if (!objects.ContainsKey(mappedInstanceID))
			{
				objects.Add(mappedInstanceID, obj);
			}
			if (obj is GameObject)
			{
				GameObject gameObject = (GameObject)obj;
				foreach (Component component in gameObject.GetComponents<Component>())
				{
					mappedInstanceID = component.GetMappedInstanceID();
					if (!objects.ContainsKey(mappedInstanceID))
					{
						objects.Add(mappedInstanceID, component);
					}
				}
				IEnumerator enumerator = gameObject.transform.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj2 = enumerator.Current;
						Transform transform = (Transform)obj2;
						this.GetReferredObjects(transform.gameObject, objects);
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
			}
		}

		// Token: 0x06000B35 RID: 2869 RVA: 0x00045F4C File Offset: 0x0004434C
		private void DuplicateAndRegister(UnityEngine.Object[] objects, Dictionary<long, UnityEngine.Object> objIdToDuplicate)
		{
			for (int i = 0; i < objects.Length; i++)
			{
				UnityEngine.Object @object = objects[i];
				bool active = false;
				bool flag = @object is Component;
				bool flag2 = @object is GameObject;
				GameObject gameObject = null;
				if (flag)
				{
					Component component = (Component)@object;
					gameObject = component.gameObject;
				}
				else if (flag2)
				{
					gameObject = (GameObject)@object;
				}
				if (gameObject != null)
				{
					active = gameObject.activeSelf;
					gameObject.SetActive(false);
				}
				UnityEngine.Object object2 = UnityEngine.Object.Instantiate(objects[i]);
				object2.name = @object.name;
				objects[i] = object2;
				GameObject gameObject2 = null;
				if (flag)
				{
					Component component2 = (Component)object2;
					gameObject2 = component2.gameObject;
					if (flag)
					{
						objIdToDuplicate.Add(gameObject.GetMappedInstanceID(), gameObject2);
					}
				}
				else if (flag2)
				{
					gameObject2 = (GameObject)object2;
				}
				objIdToDuplicate.Add(@object.GetMappedInstanceID(), object2);
				if (gameObject != null)
				{
					gameObject.SetActive(active);
					this.RegisterDynamicResource(gameObject2, new Func<int>(this.<DuplicateAndRegister>m__8));
					gameObject2.transform.SetParent(this.m_dynamicResourcesRoot, true);
					gameObject2.hideFlags = HideFlags.HideAndDontSave;
					gameObject2.SetActive(active);
					Component[] componentsInChildren = gameObject2.GetComponentsInChildren<Component>();
					for (int j = 0; j < componentsInChildren.Length; j++)
					{
						if (componentsInChildren[j] != null)
						{
							componentsInChildren[j].gameObject.hideFlags = HideFlags.HideAndDontSave;
							componentsInChildren[j].hideFlags = HideFlags.HideAndDontSave;
						}
					}
				}
				else if (IdentifiersMap.IsNotMapped(object2))
				{
					if (this.m_root.Meta.Counter < 0)
					{
						throw new InvalidOperationException("identifiers exhausted");
					}
					this.m_root.Meta.Counter++;
					IdentifiersMap.Instance.Register(object2, this.m_root.Meta.Counter);
					long mappedInstanceID = object2.GetMappedInstanceID();
					if (!this.m_loadedResources.ContainsKey(mappedInstanceID))
					{
						this.m_loadedResources.Add(mappedInstanceID, object2);
					}
				}
			}
		}

		// Token: 0x06000B36 RID: 2870 RVA: 0x00046170 File Offset: 0x00044570
		public void CreateFolder(string name, ProjectItem parent, ProjectManagerCallback<ProjectItem> callback)
		{
			ProjectManager.<CreateFolder>c__AnonStoreyC <CreateFolder>c__AnonStoreyC = new ProjectManager.<CreateFolder>c__AnonStoreyC();
			<CreateFolder>c__AnonStoreyC.callback = callback;
			<CreateFolder>c__AnonStoreyC.folder = ProjectItem.CreateFolder(name);
			parent.AddChild(<CreateFolder>c__AnonStoreyC.folder);
			<CreateFolder>c__AnonStoreyC.folder.Name = ProjectItem.GetUniqueName(name, <CreateFolder>c__AnonStoreyC.folder, parent, true);
			this.m_project.Save(<CreateFolder>c__AnonStoreyC.folder, true, new ProjectEventHandler(<CreateFolder>c__AnonStoreyC.<>m__0));
		}

		// Token: 0x06000B37 RID: 2871 RVA: 0x000461DC File Offset: 0x000445DC
		public void SaveObjects(ProjectItemObjectPair[] itemObjectPairs, ProjectManagerCallback callback)
		{
			ProjectManager.<SaveObjects>c__AnonStoreyD <SaveObjects>c__AnonStoreyD = new ProjectManager.<SaveObjects>c__AnonStoreyD();
			<SaveObjects>c__AnonStoreyD.callback = callback;
			<SaveObjects>c__AnonStoreyD.$this = this;
			if (itemObjectPairs == null || itemObjectPairs.Length == 0)
			{
				<SaveObjects>c__AnonStoreyD.callback();
				return;
			}
			ProjectManager.SaveObjectsToProjectItems(itemObjectPairs);
			ProjectManager.<SaveObjects>c__AnonStoreyD <SaveObjects>c__AnonStoreyD2 = <SaveObjects>c__AnonStoreyD;
			if (ProjectManager.<>f__am$cache7 == null)
			{
				ProjectManager.<>f__am$cache7 = new Func<ProjectItemObjectPair, ProjectItem>(ProjectManager.<SaveObjects>m__9);
			}
			<SaveObjects>c__AnonStoreyD2.projectItems = itemObjectPairs.Select(ProjectManager.<>f__am$cache7).ToArray<ProjectItem>();
			this.m_project.Save(<SaveObjects>c__AnonStoreyD.projectItems, false, new ProjectEventHandler(<SaveObjects>c__AnonStoreyD.<>m__0));
		}

		// Token: 0x06000B38 RID: 2872 RVA: 0x0004626C File Offset: 0x0004466C
		public void GetOrCreateObjects(ProjectItem folder, ProjectManagerCallback<ProjectItemObjectPair[]> callback)
		{
			if (folder == null || folder.Children == null || folder.Children.Count == 0)
			{
				callback(new ProjectItemObjectPair[0]);
				return;
			}
			ProjectItem[] projectItems = folder.Children.ToArray();
			this.GetOrCreateObjectsFromProjectItems(projectItems, callback);
		}

		// Token: 0x06000B39 RID: 2873 RVA: 0x000462BC File Offset: 0x000446BC
		public void GetOrCreateObjects(ProjectItem[] projectItems, ProjectManagerCallback<ProjectItemObjectPair[]> callback)
		{
			if (projectItems == null || projectItems.Length == 0)
			{
				callback(new ProjectItemObjectPair[0]);
				return;
			}
			if (ProjectManager.<>f__am$cache8 == null)
			{
				ProjectManager.<>f__am$cache8 = new Func<ProjectItem, bool>(ProjectManager.<GetOrCreateObjects>m__A);
			}
			IEnumerable<ProjectItem> source = projectItems.Where(ProjectManager.<>f__am$cache8);
			if (ProjectManager.<>f__am$cache9 == null)
			{
				ProjectManager.<>f__am$cache9 = new Func<ProjectItem, IEnumerable<ProjectItem>>(ProjectManager.<GetOrCreateObjects>m__B);
			}
			IEnumerable<ProjectItem> first = source.SelectMany(ProjectManager.<>f__am$cache9);
			if (ProjectManager.<>f__am$cacheA == null)
			{
				ProjectManager.<>f__am$cacheA = new Func<ProjectItem, bool>(ProjectManager.<GetOrCreateObjects>m__C);
			}
			ProjectItem[] projectItems2 = first.Union(projectItems.Where(ProjectManager.<>f__am$cacheA)).ToArray<ProjectItem>();
			this.GetOrCreateObjectsFromProjectItems(projectItems2, callback);
		}

		// Token: 0x06000B3A RID: 2874 RVA: 0x00046360 File Offset: 0x00044760
		private void GetOrCreateObjectsFromProjectItems(ProjectItem[] projectItems, ProjectManagerCallback<ProjectItemObjectPair[]> callback)
		{
			ProjectManager.<GetOrCreateObjectsFromProjectItems>c__AnonStoreyE <GetOrCreateObjectsFromProjectItems>c__AnonStoreyE = new ProjectManager.<GetOrCreateObjectsFromProjectItems>c__AnonStoreyE();
			<GetOrCreateObjectsFromProjectItems>c__AnonStoreyE.projectItems = projectItems;
			<GetOrCreateObjectsFromProjectItems>c__AnonStoreyE.callback = callback;
			<GetOrCreateObjectsFromProjectItems>c__AnonStoreyE.$this = this;
			IEnumerable<ProjectItem> projectItems2 = <GetOrCreateObjectsFromProjectItems>c__AnonStoreyE.projectItems;
			if (ProjectManager.<>f__am$cacheB == null)
			{
				ProjectManager.<>f__am$cacheB = new Func<ProjectItem, bool>(ProjectManager.<GetOrCreateObjectsFromProjectItems>m__D);
			}
			IEnumerable<ProjectItem> source = projectItems2.Where(ProjectManager.<>f__am$cacheB);
			if (ProjectManager.<>f__am$cacheC == null)
			{
				ProjectManager.<>f__am$cacheC = new Func<ProjectItem, string>(ProjectManager.<GetOrCreateObjectsFromProjectItems>m__E);
			}
			ProjectItem[] array = source.OrderBy(ProjectManager.<>f__am$cacheC).ToArray<ProjectItem>();
			IEnumerable<ProjectItem> projectItems3 = <GetOrCreateObjectsFromProjectItems>c__AnonStoreyE.projectItems;
			if (ProjectManager.<>f__am$cacheD == null)
			{
				ProjectManager.<>f__am$cacheD = new Func<ProjectItem, bool>(ProjectManager.<GetOrCreateObjectsFromProjectItems>m__F);
			}
			IEnumerable<ProjectItem> source2 = projectItems3.Where(ProjectManager.<>f__am$cacheD);
			if (ProjectManager.<>f__am$cacheE == null)
			{
				ProjectManager.<>f__am$cacheE = new Func<ProjectItem, string>(ProjectManager.<GetOrCreateObjectsFromProjectItems>m__10);
			}
			ProjectItem[] array2 = source2.OrderBy(ProjectManager.<>f__am$cacheE).ToArray<ProjectItem>();
			<GetOrCreateObjectsFromProjectItems>c__AnonStoreyE.result = new List<ProjectItemObjectPair>();
			for (int i = 0; i < array2.Length; i++)
			{
				ProjectItemWrapper projectItemWrapper = ScriptableObject.CreateInstance<ProjectItemWrapper>();
				projectItemWrapper.ProjectItem = array2[i];
				<GetOrCreateObjectsFromProjectItems>c__AnonStoreyE.result.Add(new ProjectItemObjectPair(projectItemWrapper.ProjectItem, projectItemWrapper));
			}
			for (int j = 0; j < array.Length; j++)
			{
				ProjectItemWrapper projectItemWrapper2 = ScriptableObject.CreateInstance<ProjectItemWrapper>();
				projectItemWrapper2.ProjectItem = array[j];
				<GetOrCreateObjectsFromProjectItems>c__AnonStoreyE.result.Add(new ProjectItemObjectPair(projectItemWrapper2.ProjectItem, projectItemWrapper2));
			}
			ProjectManager.<GetOrCreateObjectsFromProjectItems>c__AnonStoreyE <GetOrCreateObjectsFromProjectItems>c__AnonStoreyE2 = <GetOrCreateObjectsFromProjectItems>c__AnonStoreyE;
			IEnumerable<ProjectItem> projectItems4 = <GetOrCreateObjectsFromProjectItems>c__AnonStoreyE.projectItems;
			if (ProjectManager.<>f__am$cacheF == null)
			{
				ProjectManager.<>f__am$cacheF = new Func<ProjectItem, bool>(ProjectManager.<GetOrCreateObjectsFromProjectItems>m__11);
			}
			<GetOrCreateObjectsFromProjectItems>c__AnonStoreyE2.projectItems = projectItems4.Where(ProjectManager.<>f__am$cacheF).ToArray<ProjectItem>();
			bool dynamicOnly = true;
			Dictionary<long, ProjectItem> idToProjectItemMapping = ProjectManager.GetIdToProjectItemMapping(this.m_root.Item, dynamicOnly);
			this.GetOrCreateObjects(<GetOrCreateObjectsFromProjectItems>c__AnonStoreyE.projectItems, idToProjectItemMapping, new ProjectManagerCallback(<GetOrCreateObjectsFromProjectItems>c__AnonStoreyE.<>m__0));
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x00046518 File Offset: 0x00044918
		private void GetOrCreateObjects(ProjectItem[] projectItems, Dictionary<long, ProjectItem> idToProjectItem, ProjectManagerCallback callback)
		{
			ProjectManager.<GetOrCreateObjects>c__AnonStorey10 <GetOrCreateObjects>c__AnonStorey = new ProjectManager.<GetOrCreateObjects>c__AnonStorey10();
			<GetOrCreateObjects>c__AnonStorey.callback = callback;
			<GetOrCreateObjects>c__AnonStorey.$this = this;
			if (projectItems == null || projectItems.Length == 0)
			{
				if (<GetOrCreateObjects>c__AnonStorey.callback != null)
				{
					<GetOrCreateObjects>c__AnonStorey.callback();
				}
			}
			else
			{
				ProjectManager.<GetOrCreateObjects>c__AnonStoreyF <GetOrCreateObjects>c__AnonStoreyF = new ProjectManager.<GetOrCreateObjects>c__AnonStoreyF();
				<GetOrCreateObjects>c__AnonStoreyF.<>f__ref$16 = <GetOrCreateObjects>c__AnonStorey;
				<GetOrCreateObjects>c__AnonStoreyF.loadedProjectItemsDictionary = new Dictionary<long, ProjectItem>();
				this.LoadProjectItemsAndDependencies(projectItems, idToProjectItem, <GetOrCreateObjects>c__AnonStoreyF.loadedProjectItemsDictionary, new ProjectManagerCallback(<GetOrCreateObjects>c__AnonStoreyF.<>m__0));
			}
		}

		// Token: 0x06000B3C RID: 2876 RVA: 0x00046594 File Offset: 0x00044994
		public void Duplicate(ProjectItem[] projectItems, ProjectManagerCallback<ProjectItem[]> callback)
		{
			ProjectManager.<Duplicate>c__AnonStorey11 <Duplicate>c__AnonStorey = new ProjectManager.<Duplicate>c__AnonStorey11();
			<Duplicate>c__AnonStorey.projectItems = projectItems;
			<Duplicate>c__AnonStorey.callback = callback;
			<Duplicate>c__AnonStorey.$this = this;
			if (!this.m_isProjectLoaded)
			{
				throw new InvalidOperationException("project is not loaded");
			}
			<Duplicate>c__AnonStorey.projectItems = ProjectItem.GetRootItems(<Duplicate>c__AnonStorey.projectItems);
			<Duplicate>c__AnonStorey.allOriginalProjectItems = new List<ProjectItem>();
			for (int i = 0; i < <Duplicate>c__AnonStorey.projectItems.Length; i++)
			{
				<Duplicate>c__AnonStorey.allOriginalProjectItems.AddRange(<Duplicate>c__AnonStorey.projectItems[i].FlattenHierarchy(true));
			}
			this.m_project.LoadData(<Duplicate>c__AnonStorey.allOriginalProjectItems.ToArray(), new ProjectEventHandler<ProjectItem[]>(<Duplicate>c__AnonStorey.<>m__0), new int[0]);
		}

		// Token: 0x06000B3D RID: 2877 RVA: 0x00046648 File Offset: 0x00044A48
		public void Rename(ProjectItem projectItem, string newName, ProjectManagerCallback callback)
		{
			ProjectManager.<Rename>c__AnonStorey12 <Rename>c__AnonStorey = new ProjectManager.<Rename>c__AnonStorey12();
			<Rename>c__AnonStorey.callback = callback;
			this.m_project.Rename(projectItem, newName, new ProjectEventHandler(<Rename>c__AnonStorey.<>m__0));
		}

		// Token: 0x06000B3E RID: 2878 RVA: 0x0004667C File Offset: 0x00044A7C
		public void Move(ProjectItem[] projectItems, ProjectItem folder, ProjectManagerCallback callback)
		{
			ProjectManager.<Move>c__AnonStorey13 <Move>c__AnonStorey = new ProjectManager.<Move>c__AnonStorey13();
			<Move>c__AnonStorey.folder = folder;
			<Move>c__AnonStorey.callback = callback;
			if (!this.m_isProjectLoaded)
			{
				throw new InvalidOperationException("project is not loaded");
			}
			projectItems = ProjectItem.GetRootItems(projectItems);
			projectItems = projectItems.Where(new Func<ProjectItem, bool>(<Move>c__AnonStorey.<>m__0)).ToArray<ProjectItem>();
			if (projectItems.Length == 0)
			{
				throw new InvalidOperationException("Can't move items");
			}
			this.m_project.Move(projectItems, <Move>c__AnonStorey.folder, new ProjectEventHandler(<Move>c__AnonStorey.<>m__1));
		}

		// Token: 0x06000B3F RID: 2879 RVA: 0x00046708 File Offset: 0x00044B08
		public void Delete(ProjectItem[] projectItems, ProjectManagerCallback callback)
		{
			ProjectManager.<Delete>c__AnonStorey15 <Delete>c__AnonStorey = new ProjectManager.<Delete>c__AnonStorey15();
			<Delete>c__AnonStorey.projectItems = projectItems;
			<Delete>c__AnonStorey.callback = callback;
			<Delete>c__AnonStorey.$this = this;
			if (!this.m_isProjectLoaded)
			{
				throw new InvalidOperationException("project is not loaded");
			}
			IEnumerable<ProjectItem> projectItems2 = <Delete>c__AnonStorey.projectItems;
			if (ProjectManager.<>f__am$cache10 == null)
			{
				ProjectManager.<>f__am$cache10 = new Func<ProjectItem, bool>(ProjectManager.<Delete>m__12);
			}
			if (projectItems2.Any(ProjectManager.<>f__am$cache10))
			{
				throw new ArgumentException("Unable to remove non-dynamic and non-bundled projectItems", "projectItems");
			}
			Dictionary<long, ProjectItem> dictionary = new Dictionary<long, ProjectItem>();
			bool flag = false;
			for (int i = 0; i < <Delete>c__AnonStorey.projectItems.Length; i++)
			{
				ProjectItem projectItem = <Delete>c__AnonStorey.projectItems[i];
				bool dynamicOnly = false;
				ProjectManager.GetIdToProjectItemMapping(projectItem, dictionary, dynamicOnly);
				if (!string.IsNullOrEmpty(projectItem.BundleName))
				{
					ProjectManager.LoadedAssetBundle loadedAssetBundle = this.m_loadedBundles[projectItem.BundleName];
					loadedAssetBundle.Usages--;
					if (loadedAssetBundle.Usages <= 0)
					{
						this.m_loadedBundles.Remove(projectItem.BundleName);
						if (loadedAssetBundle.Bundle != null)
						{
							bool unloadAllLoadedObjects = true;
							RuntimeShaderUtil.RemoveExtra(projectItem.BundleName);
							IdentifiersMap.Instance.Unregister(loadedAssetBundle.Bundle);
							loadedAssetBundle.Bundle.Unload(unloadAllLoadedObjects);
							flag = true;
						}
					}
				}
			}
			if (flag)
			{
				List<long> list = new List<long>();
				foreach (KeyValuePair<long, UnityEngine.Object> keyValuePair in this.m_loadedResources)
				{
					if (keyValuePair.Value == null)
					{
						list.Add(keyValuePair.Key);
					}
				}
				for (int j = 0; j < list.Count; j++)
				{
					this.m_loadedResources.Remove(list[j]);
				}
			}
			foreach (long num in dictionary.Keys)
			{
				if (IdentifiersMap.IsDynamicResourceID(num))
				{
					UnityEngine.Object @object;
					if (this.m_loadedResources.TryGetValue(num, out @object))
					{
						if (!(@object is Component))
						{
							UnityEngine.Object.Destroy(@object);
						}
						this.m_loadedResources.Remove(num);
					}
					this.m_dynamicResources.Remove(num);
				}
			}
			this.m_project.Delete(<Delete>c__AnonStorey.projectItems, new ProjectEventHandler(<Delete>c__AnonStorey.<>m__0));
		}

		// Token: 0x06000B40 RID: 2880 RVA: 0x000469B0 File Offset: 0x00044DB0
		private void LoadProjectItemsAndDependencies(ProjectItem[] projectItems, Dictionary<long, ProjectItem> idToProjectItem, Dictionary<long, ProjectItem> processedProjectItems, ProjectManagerCallback callback)
		{
			ProjectManager.<LoadProjectItemsAndDependencies>c__AnonStorey16 <LoadProjectItemsAndDependencies>c__AnonStorey = new ProjectManager.<LoadProjectItemsAndDependencies>c__AnonStorey16();
			<LoadProjectItemsAndDependencies>c__AnonStorey.processedProjectItems = processedProjectItems;
			<LoadProjectItemsAndDependencies>c__AnonStorey.idToProjectItem = idToProjectItem;
			<LoadProjectItemsAndDependencies>c__AnonStorey.callback = callback;
			<LoadProjectItemsAndDependencies>c__AnonStorey.$this = this;
			<LoadProjectItemsAndDependencies>c__AnonStorey.exceptTypes = new int[]
			{
				2
			};
			<LoadProjectItemsAndDependencies>c__AnonStorey.loadProjectItems = new List<ProjectItem>();
			foreach (ProjectItem projectItem in projectItems)
			{
				if (!<LoadProjectItemsAndDependencies>c__AnonStorey.processedProjectItems.ContainsKey(ProjectManager.InstanceID(projectItem)))
				{
					<LoadProjectItemsAndDependencies>c__AnonStorey.loadProjectItems.Add(projectItem);
				}
			}
			if (<LoadProjectItemsAndDependencies>c__AnonStorey.loadProjectItems.Count == 0)
			{
				if (<LoadProjectItemsAndDependencies>c__AnonStorey.callback != null)
				{
					<LoadProjectItemsAndDependencies>c__AnonStorey.callback();
				}
			}
			else
			{
				ProjectManager.<LoadProjectItemsAndDependencies>c__AnonStorey17 <LoadProjectItemsAndDependencies>c__AnonStorey2 = new ProjectManager.<LoadProjectItemsAndDependencies>c__AnonStorey17();
				<LoadProjectItemsAndDependencies>c__AnonStorey2.<>f__ref$22 = <LoadProjectItemsAndDependencies>c__AnonStorey;
				IJob job = Dependencies.Job;
				<LoadProjectItemsAndDependencies>c__AnonStorey2.loadedProjectItems = new ProjectItem[0];
				job.Submit(new Action<Action>(<LoadProjectItemsAndDependencies>c__AnonStorey2.<>m__0), new Action(<LoadProjectItemsAndDependencies>c__AnonStorey2.<>m__1));
			}
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x00046AA0 File Offset: 0x00044EA0
		private void RegisterDynamicResource(long mappedInstanceId, UnityEngine.Object dynamicResource, Dictionary<long, UnityEngine.Object> decomposition)
		{
			if (!IdentifiersMap.IsDynamicResourceID(mappedInstanceId))
			{
				return;
			}
			IdentifiersMap.Instance.Register(dynamicResource, mappedInstanceId);
			if (!this.m_dynamicResources.ContainsKey(mappedInstanceId))
			{
				this.m_dynamicResources.Add(mappedInstanceId, dynamicResource);
				if (dynamicResource is GameObject)
				{
					GameObject gameObject = (GameObject)dynamicResource;
					gameObject.transform.SetParent(this.m_dynamicResourcesRoot, true);
					gameObject.hideFlags = HideFlags.HideAndDontSave;
				}
			}
			if (decomposition != null)
			{
				foreach (KeyValuePair<long, UnityEngine.Object> keyValuePair in decomposition)
				{
					long key = keyValuePair.Key;
					UnityEngine.Object value = keyValuePair.Value;
					IdentifiersMap.Instance.Register(value, key);
					if (!this.m_dynamicResources.ContainsKey(key))
					{
						this.m_dynamicResources.Add(key, value);
					}
				}
			}
		}

		// Token: 0x06000B42 RID: 2882 RVA: 0x00046B98 File Offset: 0x00044F98
		private void RegisterDynamicResource(GameObject obj, Func<int> id)
		{
			IdentifiersMap.Instance.Register(obj, id());
			long mappedInstanceID = obj.GetMappedInstanceID();
			if (!this.m_loadedResources.ContainsKey(mappedInstanceID))
			{
				this.m_loadedResources.Add(mappedInstanceID, obj);
			}
			foreach (Component component in obj.GetComponents<Component>())
			{
				if (component != null)
				{
					IdentifiersMap.Instance.Register(component, id());
					mappedInstanceID = component.GetMappedInstanceID();
					if (!this.m_loadedResources.ContainsKey(mappedInstanceID))
					{
						this.m_loadedResources.Add(mappedInstanceID, component);
					}
				}
			}
			IEnumerator enumerator = obj.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj2 = enumerator.Current;
					Transform transform = (Transform)obj2;
					this.RegisterDynamicResource(transform.gameObject, id);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}

		// Token: 0x06000B43 RID: 2883 RVA: 0x00046CA4 File Offset: 0x000450A4
		private ProjectItem ProjectTemplateToProjectItem(string projectName, FolderTemplate projectTemplate)
		{
			ProjectItem projectItem = ProjectItem.CreateFolder(projectName);
			ProjectManager.ProjectTemplateToProjectItem(projectTemplate, projectItem);
			projectItem.Name = projectName;
			return projectItem;
		}

		// Token: 0x06000B44 RID: 2884 RVA: 0x00046CC8 File Offset: 0x000450C8
		private static void ProjectTemplateToProjectItem(FolderTemplate template, ProjectItem folder)
		{
			folder.IsExposedFromEditor = true;
			folder.Name = template.name;
			IEnumerator enumerator = template.transform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					FolderTemplate component = transform.GetComponent<FolderTemplate>();
					if (component != null)
					{
						ProjectItem projectItem = ProjectItem.CreateFolder(component.name);
						folder.AddChild(projectItem);
						ProjectManager.ProjectTemplateToProjectItem(component, projectItem);
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			IEnumerable<UnityEngine.Object> objects = template.Objects;
			if (ProjectManager.<>f__am$cache11 == null)
			{
				ProjectManager.<>f__am$cache11 = new Func<UnityEngine.Object, bool>(ProjectManager.<ProjectTemplateToProjectItem>m__13);
			}
			UnityEngine.Object[] objects2 = objects.Where(ProjectManager.<>f__am$cache11).ToArray<UnityEngine.Object>();
			ProjectItem[] array = ProjectManager.ConvertObjectsToProjectItems(objects2, true, null, null, null);
			for (int i = 0; i < array.Length; i++)
			{
				folder.AddChild(array[i]);
			}
		}

		// Token: 0x06000B45 RID: 2885 RVA: 0x00046DC8 File Offset: 0x000451C8
		private static ProjectItem[] ConvertObjectsToProjectItems(UnityEngine.Object[] objects, bool isExposedFromEditor, string bundleName = null, string[] assetNames = null, Type[] assetTypes = null)
		{
			if (objects == null)
			{
				return null;
			}
			if (objects.Length == 0)
			{
				return new ProjectItem[0];
			}
			List<ProjectItem> list = new List<ProjectItem>();
			List<ProjectItem> list2 = new List<ProjectItem>();
			List<ProjectItem> list3 = new List<ProjectItem>();
			List<GameObject> list4 = new List<GameObject>();
			List<UnityEngine.Object> list5 = new List<UnityEngine.Object>();
			for (int i = 0; i < objects.Length; i++)
			{
				UnityEngine.Object @object = objects[i];
				ProjectItem projectItem = new ProjectItem();
				if (@object is GameObject)
				{
					list4.Add((GameObject)@object);
					list2.Add(projectItem);
				}
				else
				{
					list5.Add(@object);
					list3.Add(projectItem);
				}
				projectItem.Internal_Meta = new ProjectItemMeta();
				if (!string.IsNullOrEmpty(bundleName))
				{
					projectItem.Internal_Meta.BundleDescriptor = new AssetBundleDescriptor
					{
						AssetName = ((assetNames.Length <= i) ? null : assetNames[i]),
						TypeName = ((assetNames.Length <= i || assetTypes[i] == null) ? null : assetTypes[i].AssemblyQualifiedName),
						BundleName = bundleName
					};
				}
				list.Add(projectItem);
			}
			if (list4.Count > 0)
			{
				PersistentDescriptor[] array;
				PersistentData[][] array2;
				PersistentData.CreatePersistentDescriptorsAndData(list4.ToArray(), out array, out array2);
				for (int j = 0; j < array.Length; j++)
				{
					ProjectItem projectItem2 = list2[j];
					projectItem2.Internal_Meta.Descriptor = array[j];
					projectItem2.Internal_Meta.Name = list4[j].name;
					projectItem2.Internal_Meta.TypeCode = ProjectItemTypes.GetProjectItemType(typeof(GameObject));
					projectItem2.Internal_Meta.IsExposedFromEditor = isExposedFromEditor;
					ProjectItemData internal_Data = new ProjectItemData
					{
						PersistentData = array2[j]
					};
					projectItem2.Internal_Data = internal_Data;
				}
			}
			if (list5.Count > 0)
			{
				UnityEngine.Object[] array3 = list5.ToArray();
				PersistentDescriptor[] array4 = PersistentDescriptor.CreatePersistentDescriptors(array3);
				PersistentData[] array5 = PersistentData.CreatePersistentData(array3);
				for (int k = 0; k < array4.Length; k++)
				{
					UnityEngine.Object object2 = array3[k];
					ProjectItem projectItem3 = list3[k];
					projectItem3.Internal_Meta.Descriptor = array4[k];
					projectItem3.Internal_Meta.Name = object2.name;
					projectItem3.Internal_Meta.TypeCode = ProjectItemTypes.GetProjectItemType(object2.GetType());
					projectItem3.Internal_Meta.IsExposedFromEditor = isExposedFromEditor;
					ProjectItemData internal_Data2 = ProjectManager.CreateProjectItemData(array5[k], object2);
					projectItem3.Internal_Data = internal_Data2;
					ProjectManager.TryReadRawData(projectItem3, object2);
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000B46 RID: 2886 RVA: 0x0004705C File Offset: 0x0004545C
		private static ProjectItemData CreateProjectItemData(PersistentData oData, UnityEngine.Object obj)
		{
			return new ProjectItemData
			{
				PersistentData = new PersistentData[]
				{
					oData
				}
			};
		}

		// Token: 0x06000B47 RID: 2887 RVA: 0x00047084 File Offset: 0x00045484
		private static void SaveObjectsToProjectItems(ProjectItemObjectPair[] itemToObjectPairs)
		{
			if (itemToObjectPairs == null)
			{
				return;
			}
			if (itemToObjectPairs.Length == 0)
			{
				return;
			}
			List<ProjectItemObjectPair> list = new List<ProjectItemObjectPair>();
			List<ProjectItemObjectPair> list2 = new List<ProjectItemObjectPair>();
			foreach (ProjectItemObjectPair projectItemObjectPair in itemToObjectPairs)
			{
				if (projectItemObjectPair.Object is GameObject)
				{
					list.Add(projectItemObjectPair);
				}
				else
				{
					list2.Add(projectItemObjectPair);
				}
			}
			if (list.Count > 0)
			{
				IEnumerable<ProjectItemObjectPair> source = list;
				if (ProjectManager.<>f__am$cache12 == null)
				{
					ProjectManager.<>f__am$cache12 = new Func<ProjectItemObjectPair, GameObject>(ProjectManager.<SaveObjectsToProjectItems>m__14);
				}
				PersistentDescriptor[] array;
				PersistentData[][] array2;
				PersistentData.CreatePersistentDescriptorsAndData(source.Select(ProjectManager.<>f__am$cache12).ToArray<GameObject>(), out array, out array2);
				for (int j = 0; j < array.Length; j++)
				{
					ProjectItemMeta internal_Meta = new ProjectItemMeta
					{
						Descriptor = array[j],
						Name = list[j].Object.name,
						TypeCode = ProjectItemTypes.GetProjectItemType(typeof(GameObject)),
						IsExposedFromEditor = list[j].ProjectItem.IsExposedFromEditor,
						BundleDescriptor = list[j].ProjectItem.Internal_Meta.BundleDescriptor
					};
					ProjectItemData internal_Data = new ProjectItemData
					{
						PersistentData = array2[j]
					};
					ProjectItem projectItem = list[j].ProjectItem;
					projectItem.Internal_Meta = internal_Meta;
					projectItem.Internal_Data = internal_Data;
				}
			}
			if (list2.Count > 0)
			{
				IEnumerable<ProjectItemObjectPair> source2 = list2;
				if (ProjectManager.<>f__am$cache13 == null)
				{
					ProjectManager.<>f__am$cache13 = new Func<ProjectItemObjectPair, UnityEngine.Object>(ProjectManager.<SaveObjectsToProjectItems>m__15);
				}
				UnityEngine.Object[] array3 = source2.Select(ProjectManager.<>f__am$cache13).ToArray<UnityEngine.Object>();
				PersistentDescriptor[] array4 = PersistentDescriptor.CreatePersistentDescriptors(array3);
				PersistentData[] array5 = PersistentData.CreatePersistentData(array3);
				for (int k = 0; k < array4.Length; k++)
				{
					ProjectItemMeta internal_Meta2 = new ProjectItemMeta
					{
						Descriptor = array4[k],
						Name = array3[k].name,
						TypeCode = ProjectItemTypes.GetProjectItemType(array3[k].GetType()),
						IsExposedFromEditor = list2[k].ProjectItem.IsExposedFromEditor,
						BundleDescriptor = list2[k].ProjectItem.Internal_Meta.BundleDescriptor
					};
					ProjectItemData internal_Data2 = new ProjectItemData
					{
						PersistentData = new PersistentData[]
						{
							array5[k]
						}
					};
					ProjectItem projectItem2 = list2[k].ProjectItem;
					projectItem2.Internal_Meta = internal_Meta2;
					projectItem2.Internal_Data = internal_Data2;
					ProjectManager.TryReadRawData(projectItem2, array3[k]);
				}
			}
		}

		// Token: 0x06000B48 RID: 2888 RVA: 0x00047316 File Offset: 0x00045716
		private static long InstanceID(ProjectItem projectItem)
		{
			return projectItem.Internal_Meta.Descriptor.InstanceId;
		}

		// Token: 0x06000B49 RID: 2889 RVA: 0x00047328 File Offset: 0x00045728
		private static bool IsDynamicResource(ProjectItem projectItem)
		{
			return projectItem.Internal_Meta.Descriptor != null && IdentifiersMap.IsDynamicResourceID(projectItem.Internal_Meta.Descriptor.InstanceId);
		}

		// Token: 0x06000B4A RID: 2890 RVA: 0x00047354 File Offset: 0x00045754
		private static UnityEngine.Object GetOrCreateObject(ProjectItem projectItem, Dictionary<long, UnityEngine.Object> allResources, Dictionary<long, UnityEngine.Object> decomposition = null)
		{
			if (projectItem.IsFolder)
			{
				throw new InvalidOperationException("Operation is invalid for Folder");
			}
			return PersistentDescriptor.GetOrCreateObject(projectItem.Internal_Meta.Descriptor, allResources, decomposition);
		}

		// Token: 0x06000B4B RID: 2891 RVA: 0x0004738B File Offset: 0x0004578B
		private static void RestoreDataAndResolveDependencies(ProjectItem projectItem, Dictionary<long, UnityEngine.Object> objects)
		{
			if (projectItem.Internal_Data != null && projectItem.Internal_Data.PersistentData != null)
			{
				PersistentData.RestoreDataAndResolveDependencies(projectItem.Internal_Data.PersistentData, objects);
			}
			ProjectManager.TryLoadRawData(projectItem, objects);
		}

		// Token: 0x06000B4C RID: 2892 RVA: 0x000473C0 File Offset: 0x000457C0
		private static void FindDependencies(ProjectItem item, Dictionary<long, ProjectItem> dependencies, Dictionary<long, ProjectItem> identifiersMapping)
		{
			bool allowNulls = false;
			if (item.Internal_Data != null)
			{
				PersistentData[] persistentData = item.Internal_Data.PersistentData;
				for (int i = 0; i < persistentData.Length; i++)
				{
					persistentData[i].FindDependencies<ProjectItem>(dependencies, identifiersMapping, allowNulls);
				}
			}
			if (item.Children != null)
			{
				for (int j = 0; j < item.Children.Count; j++)
				{
					ProjectItem item2 = item.Children[j];
					ProjectManager.FindDependencies(item2, dependencies, identifiersMapping);
				}
			}
		}

		// Token: 0x06000B4D RID: 2893 RVA: 0x00047444 File Offset: 0x00045844
		private static void FindDependencies(ProjectItem item, Dictionary<long, UnityEngine.Object> dependencies, Dictionary<long, UnityEngine.Object> allResources, bool allowNulls)
		{
			if (item.Internal_Data != null)
			{
				PersistentData[] persistentData = item.Internal_Data.PersistentData;
				for (int i = 0; i < persistentData.Length; i++)
				{
					persistentData[i].FindDependencies<UnityEngine.Object>(dependencies, allResources, allowNulls);
				}
			}
			if (item.Children != null)
			{
				for (int j = 0; j < item.Children.Count; j++)
				{
					ProjectItem item2 = item.Children[j];
					ProjectManager.FindDependencies(item2, dependencies, allResources, allowNulls);
				}
			}
		}

		// Token: 0x06000B4E RID: 2894 RVA: 0x000474C4 File Offset: 0x000458C4
		private static void FindReferencedObjects(ProjectItem item, Dictionary<long, UnityEngine.Object> referencedObjects, Dictionary<long, UnityEngine.Object> allResources, bool allowNulls)
		{
			if (item.Internal_Meta.Descriptor != null)
			{
				item.Internal_Meta.Descriptor.FindReferencedObjects(referencedObjects, allResources, allowNulls);
			}
			if (item.Children != null)
			{
				for (int i = 0; i < item.Children.Count; i++)
				{
					ProjectItem item2 = item.Children[i];
					ProjectManager.FindReferencedObjects(item2, referencedObjects, allResources, allowNulls);
				}
			}
		}

		// Token: 0x06000B4F RID: 2895 RVA: 0x00047534 File Offset: 0x00045934
		private static Dictionary<long, ProjectItem> GetIdToProjectItemMapping(ProjectItem projectItem, bool dynamicOnly)
		{
			Dictionary<long, ProjectItem> dictionary = new Dictionary<long, ProjectItem>();
			ProjectManager.GetIdToProjectItemMapping(projectItem, dictionary, dynamicOnly);
			return dictionary;
		}

		// Token: 0x06000B50 RID: 2896 RVA: 0x00047550 File Offset: 0x00045950
		private static void GetIdToProjectItemMapping(ProjectItem item, Dictionary<long, ProjectItem> mapping, bool dynamicOnly)
		{
			if (item.Internal_Meta.Descriptor != null)
			{
				if ((item.Internal_Meta.Descriptor.Children == null || item.Internal_Meta.Descriptor.Children.Length == 0) && (item.Internal_Meta.Descriptor.Components == null || item.Internal_Meta.Descriptor.Components.Length == 0))
				{
					if ((!dynamicOnly || ProjectManager.IsDynamicResource(item)) && !mapping.ContainsKey(ProjectManager.InstanceID(item)))
					{
						mapping.Add(ProjectManager.InstanceID(item), item);
					}
				}
				else
				{
					foreach (long num in item.Internal_Meta.Descriptor.GetInstanceIds())
					{
						if ((!dynamicOnly || IdentifiersMap.IsDynamicResourceID(num)) && !mapping.ContainsKey(num))
						{
							mapping.Add(num, item);
						}
					}
				}
			}
			if (item.Children != null)
			{
				for (int j = 0; j < item.Children.Count; j++)
				{
					ProjectItem item2 = item.Children[j];
					ProjectManager.GetIdToProjectItemMapping(item2, mapping, dynamicOnly);
				}
			}
		}

		// Token: 0x06000B51 RID: 2897 RVA: 0x00047688 File Offset: 0x00045A88
		private static void Diff(ProjectItem dst, ProjectItem src, List<ProjectItem> diff)
		{
			if (!dst.IsFolder)
			{
				return;
			}
			Dictionary<string, ProjectItem> dictionary;
			if (dst.Children != null)
			{
				IEnumerable<ProjectItem> children = dst.Children;
				if (ProjectManager.<>f__am$cache14 == null)
				{
					ProjectManager.<>f__am$cache14 = new Func<ProjectItem, string>(ProjectManager.<Diff>m__16);
				}
				dictionary = children.ToDictionary(ProjectManager.<>f__am$cache14);
			}
			else
			{
				dictionary = new Dictionary<string, ProjectItem>();
			}
			Dictionary<string, ProjectItem> dictionary2 = dictionary;
			if (src.Children != null)
			{
				for (int i = 0; i < src.Children.Count; i++)
				{
					ProjectItem projectItem = src.Children[i];
					ProjectItem src2;
					if (dictionary2.TryGetValue(projectItem.ToString(), out src2))
					{
						ProjectManager.Diff(projectItem, src2, diff);
					}
					else if (projectItem.IsExposedFromEditor)
					{
						diff.Add(projectItem);
					}
				}
			}
		}

		// Token: 0x06000B52 RID: 2898 RVA: 0x00047748 File Offset: 0x00045B48
		private static void MergeData(ProjectItem dst, ProjectItem src)
		{
			if (!dst.IsFolder)
			{
				if (src.IsExposedFromEditor)
				{
					Dictionary<long, PersistentData> dictionary;
					if (dst.Internal_Data.PersistentData != null)
					{
						IEnumerable<PersistentData> persistentData = dst.Internal_Data.PersistentData;
						if (ProjectManager.<>f__am$cache15 == null)
						{
							ProjectManager.<>f__am$cache15 = new Func<PersistentData, long>(ProjectManager.<MergeData>m__17);
						}
						dictionary = persistentData.ToDictionary(ProjectManager.<>f__am$cache15);
					}
					else
					{
						dictionary = new Dictionary<long, PersistentData>();
					}
					Dictionary<long, PersistentData> dictionary2 = null;
					if (src.Internal_Data.PersistentData != null)
					{
						IEnumerable<PersistentData> persistentData2 = src.Internal_Data.PersistentData;
						if (ProjectManager.<>f__am$cache16 == null)
						{
							ProjectManager.<>f__am$cache16 = new Func<PersistentData, long>(ProjectManager.<MergeData>m__18);
						}
						dictionary2 = persistentData2.ToDictionary(ProjectManager.<>f__am$cache16);
					}
					PersistentDescriptor descriptor = src.Internal_Meta.Descriptor;
					PersistentDescriptor descriptor2 = dst.Internal_Meta.Descriptor;
					if (descriptor2.InstanceId == descriptor.InstanceId)
					{
						if (dictionary2 != null)
						{
							ProjectManager.MergeRecursive(descriptor2, descriptor, dictionary, dictionary2);
							dst.Internal_Data.PersistentData = dictionary.Values.ToArray<PersistentData>();
							if (src.Internal_Data.RawData != null)
							{
								dst.Internal_Data.RawData = src.Internal_Data.RawData;
							}
						}
						else
						{
							dst.Internal_Data.PersistentData = null;
							dst.Internal_Data.RawData = src.Internal_Data.RawData;
						}
					}
				}
			}
			else
			{
				Dictionary<string, ProjectItem> dictionary3;
				if (dst.Children != null)
				{
					IEnumerable<ProjectItem> children = dst.Children;
					if (ProjectManager.<>f__am$cache17 == null)
					{
						ProjectManager.<>f__am$cache17 = new Func<ProjectItem, string>(ProjectManager.<MergeData>m__19);
					}
					dictionary3 = children.ToDictionary(ProjectManager.<>f__am$cache17);
				}
				else
				{
					dictionary3 = new Dictionary<string, ProjectItem>();
				}
				Dictionary<string, ProjectItem> dictionary4 = dictionary3;
				if (src.Children != null)
				{
					for (int i = 0; i < src.Children.Count; i++)
					{
						ProjectItem projectItem = src.Children[i];
						ProjectItem dst2;
						if (dictionary4.TryGetValue(projectItem.ToString(), out dst2))
						{
							ProjectManager.MergeData(dst2, projectItem);
						}
						else if (!projectItem.IsExposedFromEditor)
						{
							dictionary4.Add(projectItem.ToString(), projectItem);
							dst.AddChild(projectItem);
							i--;
						}
					}
				}
			}
		}

		// Token: 0x06000B53 RID: 2899 RVA: 0x0004795C File Offset: 0x00045D5C
		private static void MergeRecursive(PersistentDescriptor descriptor, PersistentDescriptor otherDescriptor, Dictionary<long, PersistentData> data, Dictionary<long, PersistentData> otherData)
		{
			if (descriptor.InstanceId == otherDescriptor.InstanceId)
			{
				data[descriptor.InstanceId] = otherData[descriptor.InstanceId];
				if (descriptor.Components != null && otherDescriptor.Components != null)
				{
					IEnumerable<PersistentDescriptor> components = otherDescriptor.Components;
					if (ProjectManager.<>f__am$cache18 == null)
					{
						ProjectManager.<>f__am$cache18 = new Func<PersistentDescriptor, long>(ProjectManager.<MergeRecursive>m__1A);
					}
					Dictionary<long, PersistentDescriptor> dictionary = components.ToDictionary(ProjectManager.<>f__am$cache18);
					for (int i = 0; i < descriptor.Components.Length; i++)
					{
						PersistentDescriptor persistentDescriptor = descriptor.Components[i];
						PersistentDescriptor persistentDescriptor2;
						if (dictionary.TryGetValue(persistentDescriptor.InstanceId, out persistentDescriptor2))
						{
							data[persistentDescriptor.InstanceId] = otherData[persistentDescriptor2.InstanceId];
						}
					}
				}
				if (descriptor.Children != null && otherDescriptor.Children != null)
				{
					IEnumerable<PersistentDescriptor> children = otherDescriptor.Children;
					if (ProjectManager.<>f__am$cache19 == null)
					{
						ProjectManager.<>f__am$cache19 = new Func<PersistentDescriptor, long>(ProjectManager.<MergeRecursive>m__1B);
					}
					Dictionary<long, PersistentDescriptor> dictionary2 = children.ToDictionary(ProjectManager.<>f__am$cache19);
					for (int j = 0; j < descriptor.Children.Length; j++)
					{
						PersistentDescriptor persistentDescriptor3 = descriptor.Children[j];
						PersistentDescriptor otherDescriptor2;
						if (dictionary2.TryGetValue(persistentDescriptor3.InstanceId, out otherDescriptor2))
						{
							ProjectManager.MergeRecursive(persistentDescriptor3, otherDescriptor2, data, otherData);
						}
					}
				}
			}
		}

		// Token: 0x06000B54 RID: 2900 RVA: 0x00047AA8 File Offset: 0x00045EA8
		private static void TryLoadRawData(ProjectItem projectItem, Dictionary<long, UnityEngine.Object> objects)
		{
			if (projectItem.IsExposedFromEditor)
			{
				return;
			}
			if (projectItem.Internal_Data != null && projectItem.Internal_Data.RawData != null)
			{
				PersistentData persistentData = projectItem.Internal_Data.PersistentData[0];
				UnityEngine.Object @object = objects.Get(persistentData.InstanceId);
				if (@object is Texture2D)
				{
					if (!string.IsNullOrEmpty(projectItem.BundleName))
					{
						return;
					}
					Texture2D tex = (Texture2D)@object;
					tex.LoadImage(projectItem.Internal_Data.RawData);
				}
			}
		}

		// Token: 0x06000B55 RID: 2901 RVA: 0x00047B2C File Offset: 0x00045F2C
		private static void TryReadRawData(ProjectItem projectItem, UnityEngine.Object obj)
		{
			if (projectItem.IsExposedFromEditor)
			{
				return;
			}
			if (obj is Texture2D)
			{
				if (!string.IsNullOrEmpty(projectItem.BundleName))
				{
					return;
				}
				Texture2D tex = (Texture2D)obj;
				projectItem.Internal_Data.RawData = tex.EncodeToPNG();
			}
		}

		// Token: 0x06000B56 RID: 2902 RVA: 0x00047B79 File Offset: 0x00045F79
		[CompilerGenerated]
		private static bool <AddBundledResource>m__0(UnityEngine.Object o, string n)
		{
			return true;
		}

		// Token: 0x06000B57 RID: 2903 RVA: 0x00047B7C File Offset: 0x00045F7C
		[CompilerGenerated]
		private static bool <AddBundledResource<T>(UnityEngine.Object o, string n)
		{
			return true;
		}

		// Token: 0x06000B58 RID: 2904 RVA: 0x00047B7F File Offset: 0x00045F7F
		[CompilerGenerated]
		private static bool <AddBundledResource>m__2(UnityEngine.Object o, string n)
		{
			return true;
		}

		// Token: 0x06000B59 RID: 2905 RVA: 0x00047B82 File Offset: 0x00045F82
		[CompilerGenerated]
		private static bool <AddBundledResources>m__3(UnityEngine.Object o, string n)
		{
			return true;
		}

		// Token: 0x06000B5A RID: 2906 RVA: 0x00047B85 File Offset: 0x00045F85
		[CompilerGenerated]
		private static bool <AddDynamicResource>m__4(UnityEngine.Object o)
		{
			return true;
		}

		// Token: 0x06000B5B RID: 2907 RVA: 0x00047B88 File Offset: 0x00045F88
		[CompilerGenerated]
		private static bool <AddDynamicResources>m__5(UnityEngine.Object o)
		{
			return true;
		}

		// Token: 0x06000B5C RID: 2908 RVA: 0x00047B8B File Offset: 0x00045F8B
		[CompilerGenerated]
		private static bool <AddDynamicResources>m__6(UnityEngine.Object o)
		{
			return o != null;
		}

		// Token: 0x06000B5D RID: 2909 RVA: 0x00047B94 File Offset: 0x00045F94
		[CompilerGenerated]
		private static string <AddDynamicResources>m__7(ProjectItem p)
		{
			return p.NameExt;
		}

		// Token: 0x06000B5E RID: 2910 RVA: 0x00047B9C File Offset: 0x00045F9C
		[CompilerGenerated]
		private int <DuplicateAndRegister>m__8()
		{
			if (this.m_root.Meta.Counter < 0)
			{
				throw new InvalidOperationException("identifiers exhausted");
			}
			this.m_root.Meta.Counter++;
			return this.m_root.Meta.Counter;
		}

		// Token: 0x06000B5F RID: 2911 RVA: 0x00047BF2 File Offset: 0x00045FF2
		[CompilerGenerated]
		private static ProjectItem <SaveObjects>m__9(ProjectItemObjectPair i)
		{
			return i.ProjectItem;
		}

		// Token: 0x06000B60 RID: 2912 RVA: 0x00047BFA File Offset: 0x00045FFA
		[CompilerGenerated]
		private static bool <GetOrCreateObjects>m__A(ProjectItem item)
		{
			return item.Children != null && item.Children.Count > 0;
		}

		// Token: 0x06000B61 RID: 2913 RVA: 0x00047C18 File Offset: 0x00046018
		[CompilerGenerated]
		private static IEnumerable<ProjectItem> <GetOrCreateObjects>m__B(ProjectItem item)
		{
			return item.Children;
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x00047C20 File Offset: 0x00046020
		[CompilerGenerated]
		private static bool <GetOrCreateObjects>m__C(ProjectItem item)
		{
			return !item.IsFolder;
		}

		// Token: 0x06000B63 RID: 2915 RVA: 0x00047C2B File Offset: 0x0004602B
		[CompilerGenerated]
		private static bool <GetOrCreateObjectsFromProjectItems>m__D(ProjectItem p)
		{
			return p.IsScene;
		}

		// Token: 0x06000B64 RID: 2916 RVA: 0x00047C33 File Offset: 0x00046033
		[CompilerGenerated]
		private static string <GetOrCreateObjectsFromProjectItems>m__E(ProjectItem p)
		{
			return p.Name;
		}

		// Token: 0x06000B65 RID: 2917 RVA: 0x00047C3B File Offset: 0x0004603B
		[CompilerGenerated]
		private static bool <GetOrCreateObjectsFromProjectItems>m__F(ProjectItem p)
		{
			return p.IsFolder;
		}

		// Token: 0x06000B66 RID: 2918 RVA: 0x00047C43 File Offset: 0x00046043
		[CompilerGenerated]
		private static string <GetOrCreateObjectsFromProjectItems>m__10(ProjectItem p)
		{
			return p.Name;
		}

		// Token: 0x06000B67 RID: 2919 RVA: 0x00047C4B File Offset: 0x0004604B
		[CompilerGenerated]
		private static bool <GetOrCreateObjectsFromProjectItems>m__11(ProjectItem p)
		{
			return !p.IsFolder && !p.IsScene;
		}

		// Token: 0x06000B68 RID: 2920 RVA: 0x00047C64 File Offset: 0x00046064
		[CompilerGenerated]
		private static bool <Delete>m__12(ProjectItem pi)
		{
			return pi.IsResource && !ProjectManager.IsDynamicResource(pi) && string.IsNullOrEmpty(pi.BundleName);
		}

		// Token: 0x06000B69 RID: 2921 RVA: 0x00047C8A File Offset: 0x0004608A
		[CompilerGenerated]
		private static bool <ProjectTemplateToProjectItem>m__13(UnityEngine.Object obj)
		{
			return obj != null;
		}

		// Token: 0x06000B6A RID: 2922 RVA: 0x00047C93 File Offset: 0x00046093
		[CompilerGenerated]
		private static GameObject <SaveObjectsToProjectItems>m__14(ProjectItemObjectPair o)
		{
			return (GameObject)o.Object;
		}

		// Token: 0x06000B6B RID: 2923 RVA: 0x00047CA0 File Offset: 0x000460A0
		[CompilerGenerated]
		private static UnityEngine.Object <SaveObjectsToProjectItems>m__15(ProjectItemObjectPair o)
		{
			return o.Object;
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x00047CA8 File Offset: 0x000460A8
		[CompilerGenerated]
		private static string <Diff>m__16(ProjectItem child)
		{
			return child.ToString();
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x00047CB0 File Offset: 0x000460B0
		[CompilerGenerated]
		private static long <MergeData>m__17(PersistentData k)
		{
			return k.InstanceId;
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x00047CB8 File Offset: 0x000460B8
		[CompilerGenerated]
		private static long <MergeData>m__18(PersistentData k)
		{
			return k.InstanceId;
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x00047CC0 File Offset: 0x000460C0
		[CompilerGenerated]
		private static string <MergeData>m__19(ProjectItem child)
		{
			return child.ToString();
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x00047CC8 File Offset: 0x000460C8
		[CompilerGenerated]
		private static long <MergeRecursive>m__1A(PersistentDescriptor k)
		{
			return k.InstanceId;
		}

		// Token: 0x06000B71 RID: 2929 RVA: 0x00047CD0 File Offset: 0x000460D0
		[CompilerGenerated]
		private static long <MergeRecursive>m__1B(PersistentDescriptor k)
		{
			return k.InstanceId;
		}

		// Token: 0x04000C62 RID: 3170
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler ProjectLoading;

		// Token: 0x04000C63 RID: 3171
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler<ProjectManagerEventArgs> ProjectLoaded;

		// Token: 0x04000C64 RID: 3172
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler<ProjectManagerEventArgs> BundledResourcesAdded;

		// Token: 0x04000C65 RID: 3173
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private EventHandler<ProjectManagerEventArgs> DynamicResourcesAdded;

		// Token: 0x04000C66 RID: 3174
		private IAssetBundleLoader m_bundleLoader;

		// Token: 0x04000C67 RID: 3175
		[SerializeField]
		private FolderTemplate m_projectTemplate;

		// Token: 0x04000C68 RID: 3176
		[NonSerialized]
		private ProjectRoot m_root;

		// Token: 0x04000C69 RID: 3177
		private bool m_isProjectLoaded;

		// Token: 0x04000C6A RID: 3178
		private Dictionary<long, UnityEngine.Object> m_loadedResources;

		// Token: 0x04000C6B RID: 3179
		private Dictionary<string, ProjectManager.LoadedAssetBundle> m_loadedBundles = new Dictionary<string, ProjectManager.LoadedAssetBundle>();

		// Token: 0x04000C6C RID: 3180
		private Dictionary<long, UnityEngine.Object> m_dynamicResources = new Dictionary<long, UnityEngine.Object>();

		// Token: 0x04000C6D RID: 3181
		[SerializeField]
		private Transform m_dynamicResourcesRoot;

		// Token: 0x04000C6E RID: 3182
		[CompilerGenerated]
		private static Func<UnityEngine.Object, string, bool> <>f__am$cache0;

		// Token: 0x04000C6F RID: 3183
		[CompilerGenerated]
		private static Func<UnityEngine.Object, string, bool> <>f__am$cache1;

		// Token: 0x04000C70 RID: 3184
		[CompilerGenerated]
		private static Func<UnityEngine.Object, string, bool> <>f__am$cache2;

		// Token: 0x04000C71 RID: 3185
		[CompilerGenerated]
		private static Func<UnityEngine.Object, bool> <>f__am$cache3;

		// Token: 0x04000C72 RID: 3186
		[CompilerGenerated]
		private static Func<UnityEngine.Object, bool> <>f__am$cache4;

		// Token: 0x04000C73 RID: 3187
		[CompilerGenerated]
		private static Func<UnityEngine.Object, bool> <>f__am$cache5;

		// Token: 0x04000C74 RID: 3188
		[CompilerGenerated]
		private static Func<ProjectItem, string> <>f__am$cache6;

		// Token: 0x04000C75 RID: 3189
		[CompilerGenerated]
		private static Func<ProjectItemObjectPair, ProjectItem> <>f__am$cache7;

		// Token: 0x04000C76 RID: 3190
		[CompilerGenerated]
		private static Func<ProjectItem, bool> <>f__am$cache8;

		// Token: 0x04000C77 RID: 3191
		[CompilerGenerated]
		private static Func<ProjectItem, IEnumerable<ProjectItem>> <>f__am$cache9;

		// Token: 0x04000C78 RID: 3192
		[CompilerGenerated]
		private static Func<ProjectItem, bool> <>f__am$cacheA;

		// Token: 0x04000C79 RID: 3193
		[CompilerGenerated]
		private static Func<ProjectItem, bool> <>f__am$cacheB;

		// Token: 0x04000C7A RID: 3194
		[CompilerGenerated]
		private static Func<ProjectItem, string> <>f__am$cacheC;

		// Token: 0x04000C7B RID: 3195
		[CompilerGenerated]
		private static Func<ProjectItem, bool> <>f__am$cacheD;

		// Token: 0x04000C7C RID: 3196
		[CompilerGenerated]
		private static Func<ProjectItem, string> <>f__am$cacheE;

		// Token: 0x04000C7D RID: 3197
		[CompilerGenerated]
		private static Func<ProjectItem, bool> <>f__am$cacheF;

		// Token: 0x04000C7E RID: 3198
		[CompilerGenerated]
		private static Func<ProjectItem, bool> <>f__am$cache10;

		// Token: 0x04000C7F RID: 3199
		[CompilerGenerated]
		private static Func<UnityEngine.Object, bool> <>f__am$cache11;

		// Token: 0x04000C80 RID: 3200
		[CompilerGenerated]
		private static Func<ProjectItemObjectPair, GameObject> <>f__am$cache12;

		// Token: 0x04000C81 RID: 3201
		[CompilerGenerated]
		private static Func<ProjectItemObjectPair, UnityEngine.Object> <>f__am$cache13;

		// Token: 0x04000C82 RID: 3202
		[CompilerGenerated]
		private static Func<ProjectItem, string> <>f__am$cache14;

		// Token: 0x04000C83 RID: 3203
		[CompilerGenerated]
		private static Func<PersistentData, long> <>f__am$cache15;

		// Token: 0x04000C84 RID: 3204
		[CompilerGenerated]
		private static Func<PersistentData, long> <>f__am$cache16;

		// Token: 0x04000C85 RID: 3205
		[CompilerGenerated]
		private static Func<ProjectItem, string> <>f__am$cache17;

		// Token: 0x04000C86 RID: 3206
		[CompilerGenerated]
		private static Func<PersistentDescriptor, long> <>f__am$cache18;

		// Token: 0x04000C87 RID: 3207
		[CompilerGenerated]
		private static Func<PersistentDescriptor, long> <>f__am$cache19;

		// Token: 0x02000228 RID: 552
		private class LoadedAssetBundle
		{
			// Token: 0x06000B72 RID: 2930 RVA: 0x00047CD8 File Offset: 0x000460D8
			public LoadedAssetBundle()
			{
			}

			// Token: 0x04000C88 RID: 3208
			public AssetBundle Bundle;

			// Token: 0x04000C89 RID: 3209
			public int Usages;
		}

		// Token: 0x02000EAD RID: 3757
		[CompilerGenerated]
		private sealed class <LoadBundles>c__AnonStorey1
		{
			// Token: 0x0600717B RID: 29051 RVA: 0x00047CE0 File Offset: 0x000460E0
			public <LoadBundles>c__AnonStorey1()
			{
			}

			// Token: 0x04006549 RID: 25929
			internal int loading;

			// Token: 0x0400654A RID: 25930
			internal Action callback;

			// Token: 0x0400654B RID: 25931
			internal ProjectManager $this;
		}

		// Token: 0x02000EAE RID: 3758
		[CompilerGenerated]
		private sealed class <LoadBundles>c__AnonStorey0
		{
			// Token: 0x0600717C RID: 29052 RVA: 0x00047CE8 File Offset: 0x000460E8
			public <LoadBundles>c__AnonStorey0()
			{
			}

			// Token: 0x0600717D RID: 29053 RVA: 0x00047CF0 File Offset: 0x000460F0
			internal void <>m__0(string bundleName, AssetBundle bundle)
			{
				if (bundle != null)
				{
					TextAsset[] textAssets = ProjectManager.LoadAllTextAssets(bundle);
					RuntimeShaderUtil.AddExtra(this.<>f__ref$1.$this.name, textAssets);
					IdentifiersMap.Instance.Register(bundle);
					this.kvp.Value.Bundle = bundle;
				}
				this.<>f__ref$1.loading = this.<>f__ref$1.loading - 1;
				if (this.<>f__ref$1.loading == 0)
				{
					this.<>f__ref$1.callback();
				}
			}

			// Token: 0x0400654C RID: 25932
			internal KeyValuePair<string, ProjectManager.LoadedAssetBundle> kvp;

			// Token: 0x0400654D RID: 25933
			internal ProjectManager.<LoadBundles>c__AnonStorey1 <>f__ref$1;
		}

		// Token: 0x02000EAF RID: 3759
		[CompilerGenerated]
		private sealed class <LoadProject>c__AnonStorey2
		{
			// Token: 0x0600717E RID: 29054 RVA: 0x00047D7A File Offset: 0x0004617A
			public <LoadProject>c__AnonStorey2()
			{
			}

			// Token: 0x0600717F RID: 29055 RVA: 0x00047D84 File Offset: 0x00046184
			internal void <>m__0(Action doneCallback)
			{
				ProjectManager.<LoadProject>c__AnonStorey2.<LoadProject>c__AnonStorey3 <LoadProject>c__AnonStorey = new ProjectManager.<LoadProject>c__AnonStorey2.<LoadProject>c__AnonStorey3();
				<LoadProject>c__AnonStorey.<>f__ref$2 = this;
				<LoadProject>c__AnonStorey.doneCallback = doneCallback;
				bool metaOnly = false;
				int[] exceptTypes = new int[]
				{
					2
				};
				this.$this.m_project.LoadProject(this.projectName, new ProjectEventHandler<ProjectRoot>(<LoadProject>c__AnonStorey.<>m__0), metaOnly, exceptTypes);
			}

			// Token: 0x06007180 RID: 29056 RVA: 0x00047DD8 File Offset: 0x000461D8
			internal void <>m__1()
			{
				if (this.$this.m_root == null)
				{
					this.$this.m_root = new ProjectRoot();
					this.$this.m_root.Meta = new ProjectMeta();
					this.$this.m_root.Data = new ProjectData();
					this.$this.m_root.Item = ProjectItem.CreateFolder(this.projectName);
				}
				else if (this.$this.m_root.Item == null)
				{
					this.$this.m_root.Item = ProjectItem.CreateFolder(this.projectName);
				}
				else
				{
					this.$this.m_root.Item.Name = this.projectName;
				}
				if (this.$this.m_projectTemplate != null)
				{
					ProjectItem newTemplateFolder = this.$this.ProjectTemplateToProjectItem(this.projectName, this.$this.m_projectTemplate);
					if (this.$this.m_root.Item.Children == null)
					{
						this.$this.m_root.Item.Children = new List<ProjectItem>();
					}
					ProjectItem item = this.$this.m_root.Item;
					this.$this.ContinueLoadingProject(new ProjectManagerCallback(this.<>m__2), newTemplateFolder, item);
				}
			}

			// Token: 0x06007181 RID: 29057 RVA: 0x00047F30 File Offset: 0x00046330
			internal void <>m__2()
			{
				if (this.callback != null)
				{
					this.callback(this.$this.m_root.Item);
				}
				if (this.$this.ProjectLoaded != null)
				{
					this.$this.ProjectLoaded(this.$this, new ProjectManagerEventArgs(this.$this.m_root.Item));
				}
			}

			// Token: 0x0400654E RID: 25934
			internal string projectName;

			// Token: 0x0400654F RID: 25935
			internal ProjectManagerCallback<ProjectItem> callback;

			// Token: 0x04006550 RID: 25936
			internal ProjectManager $this;

			// Token: 0x02000EC0 RID: 3776
			private sealed class <LoadProject>c__AnonStorey3
			{
				// Token: 0x060071AA RID: 29098 RVA: 0x00047F9E File Offset: 0x0004639E
				public <LoadProject>c__AnonStorey3()
				{
				}

				// Token: 0x060071AB RID: 29099 RVA: 0x00047FA6 File Offset: 0x000463A6
				internal void <>m__0(ProjectPayload<ProjectRoot> loadProjectCompleted)
				{
					this.<>f__ref$2.$this.m_root = loadProjectCompleted.Data;
					this.doneCallback();
				}

				// Token: 0x04006587 RID: 25991
				internal Action doneCallback;

				// Token: 0x04006588 RID: 25992
				internal ProjectManager.<LoadProject>c__AnonStorey2 <>f__ref$2;
			}
		}

		// Token: 0x02000EB0 RID: 3760
		[CompilerGenerated]
		private sealed class <ContinueLoadingProject>c__AnonStorey4
		{
			// Token: 0x06007182 RID: 29058 RVA: 0x00047FC9 File Offset: 0x000463C9
			public <ContinueLoadingProject>c__AnonStorey4()
			{
			}

			// Token: 0x06007183 RID: 29059 RVA: 0x00047FD1 File Offset: 0x000463D1
			internal void <>m__0(ProjectPayload deleteCompleted)
			{
				this.$this.EnumerateBundles(this.$this.m_root.Item);
				this.$this.LoadBundles(new Action(this.<>m__1));
			}

			// Token: 0x06007184 RID: 29060 RVA: 0x00048005 File Offset: 0x00046405
			internal void <>m__1()
			{
				this.$this.m_project.Save(this.$this.m_root.Item, false, new ProjectEventHandler(this.<>m__2));
			}

			// Token: 0x06007185 RID: 29061 RVA: 0x00048034 File Offset: 0x00046434
			internal void <>m__2(ProjectPayload saveRootCompleted)
			{
				this.$this.CompleteProjectLoading(this.callback);
			}

			// Token: 0x04006551 RID: 25937
			internal ProjectManagerCallback callback;

			// Token: 0x04006552 RID: 25938
			internal ProjectManager $this;
		}

		// Token: 0x02000EB1 RID: 3761
		[CompilerGenerated]
		private sealed class <SaveScene>c__AnonStorey5
		{
			// Token: 0x06007186 RID: 29062 RVA: 0x00048047 File Offset: 0x00046447
			public <SaveScene>c__AnonStorey5()
			{
			}

			// Token: 0x06007187 RID: 29063 RVA: 0x0004804F File Offset: 0x0004644F
			internal bool <>m__0(ProjectItem c)
			{
				return c.NameExt.ToLower() == this.scene.NameExt;
			}

			// Token: 0x04006553 RID: 25939
			internal ProjectItem scene;
		}

		// Token: 0x02000EB2 RID: 3762
		[CompilerGenerated]
		private sealed class <LoadScene>c__AnonStorey6
		{
			// Token: 0x06007188 RID: 29064 RVA: 0x0004806C File Offset: 0x0004646C
			public <LoadScene>c__AnonStorey6()
			{
			}

			// Token: 0x06007189 RID: 29065 RVA: 0x00048074 File Offset: 0x00046474
			internal void <>m__0(ProjectPayload<ProjectItem[]> loadDataCompleted)
			{
				ProjectManager.<LoadScene>c__AnonStorey6.<LoadScene>c__AnonStorey7 <LoadScene>c__AnonStorey = new ProjectManager.<LoadScene>c__AnonStorey6.<LoadScene>c__AnonStorey7();
				<LoadScene>c__AnonStorey.<>f__ref$6 = this;
				this.scene = loadDataCompleted.Data[0];
				<LoadScene>c__AnonStorey.persistentScene = this.$this.m_serializer.Deserialize<PersistentScene>(this.scene.Internal_Data.RawData);
				if (<LoadScene>c__AnonStorey.persistentScene.Data != null)
				{
					for (int i = 0; i < <LoadScene>c__AnonStorey.persistentScene.Data.Length; i++)
					{
						PersistentData persistentData = <LoadScene>c__AnonStorey.persistentScene.Data[i];
						bool allowNulls = true;
						persistentData.FindDependencies<UnityEngine.Object>(this.sceneDependencies, this.allObjects, allowNulls);
					}
				}
				List<ProjectItem> list = new List<ProjectItem>();
				foreach (KeyValuePair<long, UnityEngine.Object> keyValuePair in this.sceneDependencies)
				{
					long key = keyValuePair.Key;
					UnityEngine.Object value = keyValuePair.Value;
					ProjectItem item;
					if (IdentifiersMap.IsDynamicResourceID(key) && this.idToProjectItem.TryGetValue(key, out item))
					{
						list.Add(item);
					}
				}
				this.$this.GetOrCreateObjects(list.ToArray(), this.idToProjectItem, new ProjectManagerCallback(<LoadScene>c__AnonStorey.<>m__0));
			}

			// Token: 0x04006554 RID: 25940
			internal ProjectItem scene;

			// Token: 0x04006555 RID: 25941
			internal Dictionary<long, UnityEngine.Object> sceneDependencies;

			// Token: 0x04006556 RID: 25942
			internal Dictionary<long, UnityEngine.Object> allObjects;

			// Token: 0x04006557 RID: 25943
			internal Dictionary<long, ProjectItem> idToProjectItem;

			// Token: 0x04006558 RID: 25944
			internal ProjectManagerCallback callback;

			// Token: 0x04006559 RID: 25945
			internal bool isEnabled;

			// Token: 0x0400655A RID: 25946
			internal ProjectManager $this;

			// Token: 0x02000EC1 RID: 3777
			private sealed class <LoadScene>c__AnonStorey7
			{
				// Token: 0x060071AC RID: 29100 RVA: 0x000481C4 File Offset: 0x000465C4
				public <LoadScene>c__AnonStorey7()
				{
				}

				// Token: 0x060071AD RID: 29101 RVA: 0x000481CC File Offset: 0x000465CC
				internal void <>m__0()
				{
					this.<>f__ref$6.$this.CompleteSceneLoading(this.<>f__ref$6.scene, this.<>f__ref$6.callback, this.<>f__ref$6.isEnabled, this.persistentScene);
				}

				// Token: 0x04006589 RID: 25993
				internal PersistentScene persistentScene;

				// Token: 0x0400658A RID: 25994
				internal ProjectManager.<LoadScene>c__AnonStorey6 <>f__ref$6;
			}
		}

		// Token: 0x02000EB3 RID: 3763
		[CompilerGenerated]
		private sealed class <AddBundledResources>c__AnonStorey8
		{
			// Token: 0x0600718A RID: 29066 RVA: 0x00048205 File Offset: 0x00046605
			public <AddBundledResources>c__AnonStorey8()
			{
			}

			// Token: 0x0600718B RID: 29067 RVA: 0x00048210 File Offset: 0x00046610
			internal void <>m__0(string name, AssetBundle bundle)
			{
				ProjectManager.<AddBundledResources>c__AnonStorey8.<AddBundledResources>c__AnonStoreyA <AddBundledResources>c__AnonStoreyA = new ProjectManager.<AddBundledResources>c__AnonStorey8.<AddBundledResources>c__AnonStoreyA();
				<AddBundledResources>c__AnonStoreyA.<>f__ref$8 = this;
				if (bundle == null)
				{
					throw new ArgumentException("unable to load bundle" + name, "bundleName");
				}
				ProjectManager.LoadedAssetBundle loadedAssetBundle;
				if (!this.$this.m_loadedBundles.TryGetValue(name, out loadedAssetBundle))
				{
					loadedAssetBundle = new ProjectManager.LoadedAssetBundle
					{
						Bundle = bundle,
						Usages = 0
					};
					this.$this.m_loadedBundles.Add(name, loadedAssetBundle);
					TextAsset[] textAssets = ProjectManager.LoadAllTextAssets(bundle);
					RuntimeShaderUtil.AddExtra(name, textAssets);
					IdentifiersMap.Instance.Register(bundle);
				}
				bool flag = this.assetNames == null;
				if (flag)
				{
					this.assetNames = bundle.GetAllAssetNames();
					this.assetTypes = new Type[this.assetNames.Length];
				}
				List<UnityEngine.Object> list = new List<UnityEngine.Object>();
				for (int i = 0; i < this.assetNames.Length; i++)
				{
					string text = this.assetNames[i];
					Type type = this.assetTypes[i];
					UnityEngine.Object @object = (type == null) ? bundle.LoadAsset(text) : bundle.LoadAsset(text, type);
					if (@object == null)
					{
						throw new ArgumentException(string.Concat(new object[]
						{
							"unable to load asset ",
							text,
							" ",
							type
						}));
					}
					if (this.filter(@object, text) && this.$this.AddBundledResourceInternalFilter(@object, text))
					{
						if (@object is Material)
						{
							Material material = (Material)@object;
							if (!(material.shader != null) || IdentifiersMap.IsNotMapped(material.shader))
							{
							}
						}
						list.Add(@object);
					}
				}
				<AddBundledResources>c__AnonStoreyA.projectItems = ProjectManager.ConvertObjectsToProjectItems(list.ToArray(), false, this.bundleName, this.assetNames, this.assetTypes);
				ProjectManager.<AddBundledResources>c__AnonStorey8.<AddBundledResources>c__AnonStoreyA <AddBundledResources>c__AnonStoreyA2 = <AddBundledResources>c__AnonStoreyA;
				IEnumerable<ProjectItem> projectItems = <AddBundledResources>c__AnonStoreyA.projectItems;
				if (ProjectManager.<AddBundledResources>c__AnonStorey8.<>f__am$cache0 == null)
				{
					ProjectManager.<AddBundledResources>c__AnonStorey8.<>f__am$cache0 = new Func<ProjectItem, string>(ProjectManager.<AddBundledResources>c__AnonStorey8.<>m__1);
				}
				<AddBundledResources>c__AnonStoreyA2.projectItems = projectItems.OrderBy(ProjectManager.<AddBundledResources>c__AnonStorey8.<>f__am$cache0).ToArray<ProjectItem>();
				for (int j = 0; j < <AddBundledResources>c__AnonStoreyA.projectItems.Length; j++)
				{
					ProjectManager.<AddBundledResources>c__AnonStorey8.<AddBundledResources>c__AnonStorey9 <AddBundledResources>c__AnonStorey = new ProjectManager.<AddBundledResources>c__AnonStorey8.<AddBundledResources>c__AnonStorey9();
					<AddBundledResources>c__AnonStorey.<>f__ref$8 = this;
					<AddBundledResources>c__AnonStorey.projectItem = <AddBundledResources>c__AnonStoreyA.projectItems[j];
					if (this.folder.Children != null)
					{
						ProjectItem projectItem = this.folder.Children.Where(new Func<ProjectItem, bool>(<AddBundledResources>c__AnonStorey.<>m__0)).FirstOrDefault<ProjectItem>();
						if (projectItem != null)
						{
							this.folder.RemoveChild(projectItem);
						}
						else
						{
							loadedAssetBundle.Usages++;
						}
					}
					else
					{
						loadedAssetBundle.Usages++;
					}
					this.folder.AddChild(<AddBundledResources>c__AnonStorey.projectItem);
				}
				this.$this.m_project.Save(<AddBundledResources>c__AnonStoreyA.projectItems, false, new ProjectEventHandler(<AddBundledResources>c__AnonStoreyA.<>m__0));
			}

			// Token: 0x0600718C RID: 29068 RVA: 0x00048508 File Offset: 0x00046908
			private static string <>m__1(ProjectItem p)
			{
				return p.NameExt;
			}

			// Token: 0x0400655B RID: 25947
			internal string[] assetNames;

			// Token: 0x0400655C RID: 25948
			internal Type[] assetTypes;

			// Token: 0x0400655D RID: 25949
			internal Func<UnityEngine.Object, string, bool> filter;

			// Token: 0x0400655E RID: 25950
			internal string bundleName;

			// Token: 0x0400655F RID: 25951
			internal ProjectItem folder;

			// Token: 0x04006560 RID: 25952
			internal ProjectManagerCallback<ProjectItem[]> callback;

			// Token: 0x04006561 RID: 25953
			internal ProjectManager $this;

			// Token: 0x04006562 RID: 25954
			private static Func<ProjectItem, string> <>f__am$cache0;

			// Token: 0x02000EC2 RID: 3778
			private sealed class <AddBundledResources>c__AnonStoreyA
			{
				// Token: 0x060071AE RID: 29102 RVA: 0x00048510 File Offset: 0x00046910
				public <AddBundledResources>c__AnonStoreyA()
				{
				}

				// Token: 0x060071AF RID: 29103 RVA: 0x00048518 File Offset: 0x00046918
				internal void <>m__0(ProjectPayload saveItemsCompleted)
				{
					bool includeDynamicResources = false;
					Dictionary<long, UnityEngine.Object> allResources = IdentifiersMap.FindResources(includeDynamicResources);
					bool allowNulls = false;
					for (int i = 0; i < this.projectItems.Length; i++)
					{
						ProjectItem item = this.projectItems[i];
						ProjectManager.FindDependencies(item, this.<>f__ref$8.$this.m_loadedResources, allResources, allowNulls);
						ProjectManager.FindReferencedObjects(item, this.<>f__ref$8.$this.m_loadedResources, allResources, allowNulls);
					}
					for (int j = 0; j < this.projectItems.Length; j++)
					{
						ProjectItem item2 = this.projectItems[j];
						this.<>f__ref$8.$this.m_project.UnloadData(item2);
					}
					if (this.<>f__ref$8.callback != null)
					{
						this.<>f__ref$8.callback(this.projectItems);
					}
					if (this.<>f__ref$8.$this.BundledResourcesAdded != null)
					{
						this.<>f__ref$8.$this.BundledResourcesAdded(this.<>f__ref$8.$this, new ProjectManagerEventArgs(this.projectItems));
					}
				}

				// Token: 0x0400658B RID: 25995
				internal ProjectItem[] projectItems;

				// Token: 0x0400658C RID: 25996
				internal ProjectManager.<AddBundledResources>c__AnonStorey8 <>f__ref$8;
			}

			// Token: 0x02000EC3 RID: 3779
			private sealed class <AddBundledResources>c__AnonStorey9
			{
				// Token: 0x060071B0 RID: 29104 RVA: 0x0004862A File Offset: 0x00046A2A
				public <AddBundledResources>c__AnonStorey9()
				{
				}

				// Token: 0x060071B1 RID: 29105 RVA: 0x00048632 File Offset: 0x00046A32
				internal bool <>m__0(ProjectItem p)
				{
					return p.NameExt == this.projectItem.NameExt;
				}

				// Token: 0x0400658D RID: 25997
				internal ProjectItem projectItem;

				// Token: 0x0400658E RID: 25998
				internal ProjectManager.<AddBundledResources>c__AnonStorey8 <>f__ref$8;
			}
		}

		// Token: 0x02000EB4 RID: 3764
		[CompilerGenerated]
		private sealed class <AddDynamicResources>c__AnonStoreyB
		{
			// Token: 0x0600718D RID: 29069 RVA: 0x0004864A File Offset: 0x00046A4A
			public <AddDynamicResources>c__AnonStoreyB()
			{
			}

			// Token: 0x0600718E RID: 29070 RVA: 0x00048654 File Offset: 0x00046A54
			internal void <>m__0(ProjectPayload saveItemsCompleted)
			{
				bool includeDynamicResources = false;
				Dictionary<long, UnityEngine.Object> allResources = IdentifiersMap.FindResources(includeDynamicResources);
				bool allowNulls = false;
				for (int i = 0; i < this.projectItems.Length; i++)
				{
					ProjectItem item = this.projectItems[i];
					ProjectManager.FindDependencies(item, this.$this.m_loadedResources, allResources, allowNulls);
					ProjectManager.FindReferencedObjects(item, this.$this.m_loadedResources, allResources, allowNulls);
				}
				for (int j = 0; j < this.projectItems.Length; j++)
				{
					ProjectItem item2 = this.projectItems[j];
					this.$this.m_project.UnloadData(item2);
				}
				this.$this.m_project.SaveProjectMeta(this.$this.Project.Name, this.$this.m_root.Meta, new ProjectEventHandler(this.<>m__1));
			}

			// Token: 0x0600718F RID: 29071 RVA: 0x00048730 File Offset: 0x00046B30
			internal void <>m__1(ProjectPayload saveProjectMetaCompleted)
			{
				if (this.callback != null)
				{
					this.callback(this.projectItems);
				}
				if (this.$this.DynamicResourcesAdded != null)
				{
					this.$this.DynamicResourcesAdded(this.$this, new ProjectManagerEventArgs(this.projectItems));
				}
			}

			// Token: 0x04006563 RID: 25955
			internal ProjectItem[] projectItems;

			// Token: 0x04006564 RID: 25956
			internal ProjectManagerCallback<ProjectItem[]> callback;

			// Token: 0x04006565 RID: 25957
			internal ProjectManager $this;
		}

		// Token: 0x02000EB5 RID: 3765
		[CompilerGenerated]
		private sealed class <CreateFolder>c__AnonStoreyC
		{
			// Token: 0x06007190 RID: 29072 RVA: 0x0004878A File Offset: 0x00046B8A
			public <CreateFolder>c__AnonStoreyC()
			{
			}

			// Token: 0x06007191 RID: 29073 RVA: 0x00048792 File Offset: 0x00046B92
			internal void <>m__0(ProjectPayload saveCompleted)
			{
				if (this.callback != null)
				{
					this.callback(this.folder);
				}
			}

			// Token: 0x04006566 RID: 25958
			internal ProjectManagerCallback<ProjectItem> callback;

			// Token: 0x04006567 RID: 25959
			internal ProjectItem folder;
		}

		// Token: 0x02000EB6 RID: 3766
		[CompilerGenerated]
		private sealed class <SaveObjects>c__AnonStoreyD
		{
			// Token: 0x06007192 RID: 29074 RVA: 0x000487B0 File Offset: 0x00046BB0
			public <SaveObjects>c__AnonStoreyD()
			{
			}

			// Token: 0x06007193 RID: 29075 RVA: 0x000487B8 File Offset: 0x00046BB8
			internal void <>m__0(ProjectPayload saveCompleted)
			{
				for (int i = 0; i < this.projectItems.Length; i++)
				{
					ProjectItem item = this.projectItems[i];
					this.$this.m_project.UnloadData(item);
				}
				if (this.callback != null)
				{
					this.callback();
				}
			}

			// Token: 0x04006568 RID: 25960
			internal ProjectItem[] projectItems;

			// Token: 0x04006569 RID: 25961
			internal ProjectManagerCallback callback;

			// Token: 0x0400656A RID: 25962
			internal ProjectManager $this;
		}

		// Token: 0x02000EB7 RID: 3767
		[CompilerGenerated]
		private sealed class <GetOrCreateObjectsFromProjectItems>c__AnonStoreyE
		{
			// Token: 0x06007194 RID: 29076 RVA: 0x0004880E File Offset: 0x00046C0E
			public <GetOrCreateObjectsFromProjectItems>c__AnonStoreyE()
			{
			}

			// Token: 0x06007195 RID: 29077 RVA: 0x00048818 File Offset: 0x00046C18
			internal void <>m__0()
			{
				IEnumerable<ProjectItem> source = this.projectItems;
				if (ProjectManager.<GetOrCreateObjectsFromProjectItems>c__AnonStoreyE.<>f__am$cache0 == null)
				{
					ProjectManager.<GetOrCreateObjectsFromProjectItems>c__AnonStoreyE.<>f__am$cache0 = new Func<ProjectItem, string>(ProjectManager.<GetOrCreateObjectsFromProjectItems>c__AnonStoreyE.<>m__1);
				}
				this.projectItems = source.OrderBy(ProjectManager.<GetOrCreateObjectsFromProjectItems>c__AnonStoreyE.<>f__am$cache0).ToArray<ProjectItem>();
				for (int i = 0; i < this.projectItems.Length; i++)
				{
					ProjectItem projectItem = this.projectItems[i];
					UnityEngine.Object obj;
					if (this.$this.m_loadedResources.TryGetValue(projectItem.Internal_Meta.Descriptor.InstanceId, out obj))
					{
						this.result.Add(new ProjectItemObjectPair(projectItem, obj));
					}
				}
				if (this.callback != null)
				{
					this.callback(this.result.ToArray());
				}
			}

			// Token: 0x06007196 RID: 29078 RVA: 0x000488D4 File Offset: 0x00046CD4
			private static string <>m__1(ProjectItem item)
			{
				return item.Name;
			}

			// Token: 0x0400656B RID: 25963
			internal ProjectItem[] projectItems;

			// Token: 0x0400656C RID: 25964
			internal List<ProjectItemObjectPair> result;

			// Token: 0x0400656D RID: 25965
			internal ProjectManagerCallback<ProjectItemObjectPair[]> callback;

			// Token: 0x0400656E RID: 25966
			internal ProjectManager $this;

			// Token: 0x0400656F RID: 25967
			private static Func<ProjectItem, string> <>f__am$cache0;
		}

		// Token: 0x02000EB8 RID: 3768
		[CompilerGenerated]
		private sealed class <GetOrCreateObjects>c__AnonStorey10
		{
			// Token: 0x06007197 RID: 29079 RVA: 0x000488DC File Offset: 0x00046CDC
			public <GetOrCreateObjects>c__AnonStorey10()
			{
			}

			// Token: 0x04006570 RID: 25968
			internal ProjectManagerCallback callback;

			// Token: 0x04006571 RID: 25969
			internal ProjectManager $this;
		}

		// Token: 0x02000EB9 RID: 3769
		[CompilerGenerated]
		private sealed class <GetOrCreateObjects>c__AnonStoreyF
		{
			// Token: 0x06007198 RID: 29080 RVA: 0x000488E4 File Offset: 0x00046CE4
			public <GetOrCreateObjects>c__AnonStoreyF()
			{
			}

			// Token: 0x06007199 RID: 29081 RVA: 0x000488EC File Offset: 0x00046CEC
			internal void <>m__0()
			{
				ProjectItem[] array = this.loadedProjectItemsDictionary.Values.ToArray<ProjectItem>();
				List<GameObject> list = new List<GameObject>();
				foreach (ProjectItem projectItem in array)
				{
					Dictionary<long, UnityEngine.Object> decomposition = null;
					if (projectItem.IsGameObject)
					{
						decomposition = new Dictionary<long, UnityEngine.Object>();
					}
					UnityEngine.Object orCreateObject = ProjectManager.GetOrCreateObject(projectItem, this.<>f__ref$16.$this.m_loadedResources, decomposition);
					this.<>f__ref$16.$this.RegisterDynamicResource(ProjectManager.InstanceID(projectItem), orCreateObject, decomposition);
					if (orCreateObject is GameObject && ProjectManager.IsDynamicResource(projectItem))
					{
						list.Add((GameObject)orCreateObject);
					}
				}
				foreach (ProjectItem projectItem2 in array)
				{
					Dictionary<long, UnityEngine.Object> dictionary = new Dictionary<long, UnityEngine.Object>();
					ProjectManager.FindDependencies(projectItem2, dictionary, this.<>f__ref$16.$this.m_loadedResources, false);
					ProjectManager.FindReferencedObjects(projectItem2, dictionary, this.<>f__ref$16.$this.m_loadedResources, false);
					ProjectManager.RestoreDataAndResolveDependencies(projectItem2, dictionary);
				}
				for (int k = 0; k < list.Count; k++)
				{
					GameObject gameObject = list[k];
					gameObject.transform.SetParent(this.<>f__ref$16.$this.m_dynamicResourcesRoot, true);
					gameObject.hideFlags = HideFlags.HideAndDontSave;
				}
				foreach (ProjectItem item in array)
				{
					this.<>f__ref$16.$this.m_project.UnloadData(item);
				}
				if (this.<>f__ref$16.callback != null)
				{
					this.<>f__ref$16.callback();
				}
			}

			// Token: 0x04006572 RID: 25970
			internal Dictionary<long, ProjectItem> loadedProjectItemsDictionary;

			// Token: 0x04006573 RID: 25971
			internal ProjectManager.<GetOrCreateObjects>c__AnonStorey10 <>f__ref$16;
		}

		// Token: 0x02000EBA RID: 3770
		[CompilerGenerated]
		private sealed class <Duplicate>c__AnonStorey11
		{
			// Token: 0x0600719A RID: 29082 RVA: 0x00048A97 File Offset: 0x00046E97
			public <Duplicate>c__AnonStorey11()
			{
			}

			// Token: 0x0600719B RID: 29083 RVA: 0x00048AA0 File Offset: 0x00046EA0
			internal void <>m__0(ProjectPayload<ProjectItem[]> loadDataCompleted)
			{
				ProjectItem[] array = new ProjectItem[loadDataCompleted.Data.Length];
				for (int i = 0; i < loadDataCompleted.Data.Length; i++)
				{
					array[i] = loadDataCompleted.Data[i].Parent;
					loadDataCompleted.Data[i].Parent = null;
				}
				this.projectItems = this.$this.m_serializer.DeepClone<ProjectItem[]>(loadDataCompleted.Data);
				for (int j = 0; j < this.projectItems.Length; j++)
				{
					loadDataCompleted.Data[j].Parent = array[j];
					this.projectItems[j].Parent = array[j];
				}
				for (int k = 0; k < this.allOriginalProjectItems.Count; k++)
				{
					this.$this.m_project.UnloadData(this.allOriginalProjectItems[k]);
				}
				ProjectItem[] rootItems = ProjectItem.GetRootItems(this.projectItems);
				for (int l = 0; l < rootItems.Length; l++)
				{
					ProjectItem projectItem = rootItems[l];
					projectItem.IsExposedFromEditor = false;
					string uniqueName = ProjectItem.GetUniqueName(projectItem.Name, rootItems[l], rootItems[l].Parent, false);
					if (uniqueName != projectItem.NameExt)
					{
						projectItem.NameExt = uniqueName;
						if (!projectItem.IsFolder && !projectItem.IsScene)
						{
							projectItem.Rename(projectItem.Name);
						}
					}
				}
				for (int m = 0; m < this.projectItems.Length; m++)
				{
					ProjectItem projectItem2 = this.projectItems[m];
					if (!projectItem2.IsFolder && !projectItem2.IsScene)
					{
						ProjectManager.LoadedAssetBundle loadedAssetBundle;
						if (!string.IsNullOrEmpty(projectItem2.BundleName) && this.$this.m_loadedBundles.TryGetValue(projectItem2.BundleName, out loadedAssetBundle))
						{
							loadedAssetBundle.Usages++;
						}
						PersistentDescriptor[] array2 = projectItem2.Internal_Meta.Descriptor.FlattenHierarchy();
						if (projectItem2.Internal_Data.PersistentData != null)
						{
							IEnumerable<PersistentData> persistentData = projectItem2.Internal_Data.PersistentData;
							if (ProjectManager.<Duplicate>c__AnonStorey11.<>f__am$cache0 == null)
							{
								ProjectManager.<Duplicate>c__AnonStorey11.<>f__am$cache0 = new Func<PersistentData, long>(ProjectManager.<Duplicate>c__AnonStorey11.<>m__1);
							}
							Dictionary<long, PersistentData> dictionary = persistentData.ToDictionary(ProjectManager.<Duplicate>c__AnonStorey11.<>f__am$cache0);
							HashSet<PersistentTransform> hashSet = new HashSet<PersistentTransform>();
							for (int n = 0; n < array2.Length; n++)
							{
								this.$this.m_root.Meta.Counter++;
								PersistentDescriptor persistentDescriptor = array2[n];
								PersistentData persistentData2;
								if (dictionary.TryGetValue(persistentDescriptor.InstanceId, out persistentData2))
								{
									persistentData2.InstanceId = IdentifiersMap.ToDynamicResourceID(this.$this.m_root.Meta.Counter);
									if (persistentData2 is PersistentTransform)
									{
										PersistentTransform item = (PersistentTransform)persistentData2;
										if (!hashSet.Contains(item))
										{
											hashSet.Add(item);
										}
									}
								}
								persistentDescriptor.InstanceId = IdentifiersMap.ToDynamicResourceID(this.$this.m_root.Meta.Counter);
							}
							foreach (PersistentTransform persistentTransform in hashSet)
							{
								PersistentData persistentData3;
								if (dictionary.TryGetValue(persistentTransform.parent, out persistentData3))
								{
									persistentTransform.parent = persistentData3.InstanceId;
								}
							}
						}
						else
						{
							this.$this.m_root.Meta.Counter++;
							array2[0].InstanceId = IdentifiersMap.ToDynamicResourceID(this.$this.m_root.Meta.Counter);
						}
					}
				}
				this.$this.m_project.Save(this.projectItems.ToArray<ProjectItem>(), false, new ProjectEventHandler(this.<>m__2));
			}

			// Token: 0x0600719C RID: 29084 RVA: 0x00048E8C File Offset: 0x0004728C
			private static long <>m__1(PersistentData item)
			{
				return item.InstanceId;
			}

			// Token: 0x0600719D RID: 29085 RVA: 0x00048E94 File Offset: 0x00047294
			internal void <>m__2(ProjectPayload saveItemsCompleted)
			{
				for (int i = 0; i < this.projectItems.Length; i++)
				{
					ProjectItem item = this.projectItems[i];
					this.$this.m_project.UnloadData(item);
				}
				this.$this.m_project.SaveProjectMeta(this.$this.Project.Name, this.$this.m_root.Meta, new ProjectEventHandler(this.<>m__3));
			}

			// Token: 0x0600719E RID: 29086 RVA: 0x00048F10 File Offset: 0x00047310
			internal void <>m__3(ProjectPayload saveProjectMetaCompleted)
			{
				if (this.callback != null)
				{
					this.callback(this.projectItems);
				}
			}

			// Token: 0x04006574 RID: 25972
			internal ProjectItem[] projectItems;

			// Token: 0x04006575 RID: 25973
			internal List<ProjectItem> allOriginalProjectItems;

			// Token: 0x04006576 RID: 25974
			internal ProjectManagerCallback<ProjectItem[]> callback;

			// Token: 0x04006577 RID: 25975
			internal ProjectManager $this;

			// Token: 0x04006578 RID: 25976
			private static Func<PersistentData, long> <>f__am$cache0;
		}

		// Token: 0x02000EBB RID: 3771
		[CompilerGenerated]
		private sealed class <Rename>c__AnonStorey12
		{
			// Token: 0x0600719F RID: 29087 RVA: 0x00048F2E File Offset: 0x0004732E
			public <Rename>c__AnonStorey12()
			{
			}

			// Token: 0x060071A0 RID: 29088 RVA: 0x00048F36 File Offset: 0x00047336
			internal void <>m__0(ProjectPayload renameCompleted)
			{
				if (this.callback != null)
				{
					this.callback();
				}
			}

			// Token: 0x04006579 RID: 25977
			internal ProjectManagerCallback callback;
		}

		// Token: 0x02000EBC RID: 3772
		[CompilerGenerated]
		private sealed class <Move>c__AnonStorey13
		{
			// Token: 0x060071A1 RID: 29089 RVA: 0x00048F4E File Offset: 0x0004734E
			public <Move>c__AnonStorey13()
			{
			}

			// Token: 0x060071A2 RID: 29090 RVA: 0x00048F58 File Offset: 0x00047358
			internal bool <>m__0(ProjectItem item)
			{
				ProjectManager.<Move>c__AnonStorey13.<Move>c__AnonStorey14 <Move>c__AnonStorey = new ProjectManager.<Move>c__AnonStorey13.<Move>c__AnonStorey14();
				<Move>c__AnonStorey.<>f__ref$19 = this;
				<Move>c__AnonStorey.item = item;
				return this.folder.Children == null || this.folder.Children.Contains(<Move>c__AnonStorey.item) || !this.folder.Children.Any(new Func<ProjectItem, bool>(<Move>c__AnonStorey.<>m__0));
			}

			// Token: 0x060071A3 RID: 29091 RVA: 0x00048FC6 File Offset: 0x000473C6
			internal void <>m__1(ProjectPayload moveCompleted)
			{
				if (this.callback != null)
				{
					this.callback();
				}
			}

			// Token: 0x0400657A RID: 25978
			internal ProjectItem folder;

			// Token: 0x0400657B RID: 25979
			internal ProjectManagerCallback callback;

			// Token: 0x02000EC4 RID: 3780
			private sealed class <Move>c__AnonStorey14
			{
				// Token: 0x060071B2 RID: 29106 RVA: 0x00048FDE File Offset: 0x000473DE
				public <Move>c__AnonStorey14()
				{
				}

				// Token: 0x060071B3 RID: 29107 RVA: 0x00048FE6 File Offset: 0x000473E6
				internal bool <>m__0(ProjectItem c)
				{
					return c.NameExt == this.item.NameExt;
				}

				// Token: 0x0400658F RID: 25999
				internal ProjectItem item;

				// Token: 0x04006590 RID: 26000
				internal ProjectManager.<Move>c__AnonStorey13 <>f__ref$19;
			}
		}

		// Token: 0x02000EBD RID: 3773
		[CompilerGenerated]
		private sealed class <Delete>c__AnonStorey15
		{
			// Token: 0x060071A4 RID: 29092 RVA: 0x00048FFE File Offset: 0x000473FE
			public <Delete>c__AnonStorey15()
			{
			}

			// Token: 0x060071A5 RID: 29093 RVA: 0x00049008 File Offset: 0x00047408
			internal void <>m__0(ProjectPayload deleteCompleted)
			{
				for (int i = 0; i < this.projectItems.Length; i++)
				{
					ProjectItem projectItem = this.projectItems[i];
					this.$this.m_project.UnloadData(projectItem);
					if (projectItem.Parent != null)
					{
						projectItem.Parent.RemoveChild(projectItem);
					}
				}
				if (this.callback != null)
				{
					this.callback();
				}
			}

			// Token: 0x0400657C RID: 25980
			internal ProjectItem[] projectItems;

			// Token: 0x0400657D RID: 25981
			internal ProjectManagerCallback callback;

			// Token: 0x0400657E RID: 25982
			internal ProjectManager $this;
		}

		// Token: 0x02000EBE RID: 3774
		[CompilerGenerated]
		private sealed class <LoadProjectItemsAndDependencies>c__AnonStorey16
		{
			// Token: 0x060071A6 RID: 29094 RVA: 0x00049075 File Offset: 0x00047475
			public <LoadProjectItemsAndDependencies>c__AnonStorey16()
			{
			}

			// Token: 0x0400657F RID: 25983
			internal List<ProjectItem> loadProjectItems;

			// Token: 0x04006580 RID: 25984
			internal int[] exceptTypes;

			// Token: 0x04006581 RID: 25985
			internal Dictionary<long, ProjectItem> processedProjectItems;

			// Token: 0x04006582 RID: 25986
			internal Dictionary<long, ProjectItem> idToProjectItem;

			// Token: 0x04006583 RID: 25987
			internal ProjectManagerCallback callback;

			// Token: 0x04006584 RID: 25988
			internal ProjectManager $this;
		}

		// Token: 0x02000EBF RID: 3775
		[CompilerGenerated]
		private sealed class <LoadProjectItemsAndDependencies>c__AnonStorey17
		{
			// Token: 0x060071A7 RID: 29095 RVA: 0x0004907D File Offset: 0x0004747D
			public <LoadProjectItemsAndDependencies>c__AnonStorey17()
			{
			}

			// Token: 0x060071A8 RID: 29096 RVA: 0x00049088 File Offset: 0x00047488
			internal void <>m__0(Action doneCallback)
			{
				ProjectManager.<LoadProjectItemsAndDependencies>c__AnonStorey17.<LoadProjectItemsAndDependencies>c__AnonStorey18 <LoadProjectItemsAndDependencies>c__AnonStorey = new ProjectManager.<LoadProjectItemsAndDependencies>c__AnonStorey17.<LoadProjectItemsAndDependencies>c__AnonStorey18();
				<LoadProjectItemsAndDependencies>c__AnonStorey.<>f__ref$22 = this.<>f__ref$22;
				<LoadProjectItemsAndDependencies>c__AnonStorey.<>f__ref$23 = this;
				<LoadProjectItemsAndDependencies>c__AnonStorey.doneCallback = doneCallback;
				this.<>f__ref$22.$this.m_project.LoadData(this.<>f__ref$22.loadProjectItems.ToArray(), new ProjectEventHandler<ProjectItem[]>(<LoadProjectItemsAndDependencies>c__AnonStorey.<>m__0), this.<>f__ref$22.exceptTypes);
			}

			// Token: 0x060071A9 RID: 29097 RVA: 0x000490F4 File Offset: 0x000474F4
			internal void <>m__1()
			{
				Dictionary<long, ProjectItem> dictionary = new Dictionary<long, ProjectItem>();
				for (int i = 0; i < this.loadedProjectItems.Length; i++)
				{
					ProjectItem projectItem = this.loadedProjectItems[i];
					if (!this.<>f__ref$22.processedProjectItems.ContainsKey(ProjectManager.InstanceID(projectItem)))
					{
						this.<>f__ref$22.processedProjectItems.Add(ProjectManager.InstanceID(projectItem), projectItem);
						ProjectManager.FindDependencies(projectItem, dictionary, this.<>f__ref$22.idToProjectItem);
					}
				}
				if (dictionary.Count > 0)
				{
					this.<>f__ref$22.$this.LoadProjectItemsAndDependencies(dictionary.Values.ToArray<ProjectItem>(), this.<>f__ref$22.idToProjectItem, this.<>f__ref$22.processedProjectItems, this.<>f__ref$22.callback);
				}
				else if (this.<>f__ref$22.callback != null)
				{
					this.<>f__ref$22.callback();
				}
			}

			// Token: 0x04006585 RID: 25989
			internal ProjectItem[] loadedProjectItems;

			// Token: 0x04006586 RID: 25990
			internal ProjectManager.<LoadProjectItemsAndDependencies>c__AnonStorey16 <>f__ref$22;

			// Token: 0x02000EC5 RID: 3781
			private sealed class <LoadProjectItemsAndDependencies>c__AnonStorey18
			{
				// Token: 0x060071B4 RID: 29108 RVA: 0x000491DA File Offset: 0x000475DA
				public <LoadProjectItemsAndDependencies>c__AnonStorey18()
				{
				}

				// Token: 0x060071B5 RID: 29109 RVA: 0x000491E2 File Offset: 0x000475E2
				internal void <>m__0(ProjectPayload<ProjectItem[]> loadProjectItemsCompleted)
				{
					this.<>f__ref$23.loadedProjectItems = loadProjectItemsCompleted.Data;
					this.doneCallback();
				}

				// Token: 0x04006591 RID: 26001
				internal Action doneCallback;

				// Token: 0x04006592 RID: 26002
				internal ProjectManager.<LoadProjectItemsAndDependencies>c__AnonStorey16 <>f__ref$22;

				// Token: 0x04006593 RID: 26003
				internal ProjectManager.<LoadProjectItemsAndDependencies>c__AnonStorey17 <>f__ref$23;
			}
		}
	}
}
