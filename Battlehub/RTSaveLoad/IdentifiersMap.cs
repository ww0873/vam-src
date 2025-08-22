using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Battlehub.Utils;
using UnityEngine;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x0200022D RID: 557
	public class IdentifiersMap
	{
		// Token: 0x06000B7B RID: 2939 RVA: 0x000492B4 File Offset: 0x000476B4
		public IdentifiersMap()
		{
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x06000B7C RID: 2940 RVA: 0x000492E8 File Offset: 0x000476E8
		public static bool IsInitialized
		{
			get
			{
				return IdentifiersMap.m_instance != null;
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x06000B7D RID: 2941 RVA: 0x000492F8 File Offset: 0x000476F8
		// (set) Token: 0x06000B7E RID: 2942 RVA: 0x00049320 File Offset: 0x00047720
		public static IdentifiersMap Instance
		{
			get
			{
				if (IdentifiersMap.m_instance == null)
				{
					IdentifiersMap identifiersMap = new IdentifiersMap();
					identifiersMap.Initialize();
				}
				return IdentifiersMap.m_instance;
			}
			set
			{
				IdentifiersMap.m_instance = value;
			}
		}

		// Token: 0x06000B7F RID: 2943 RVA: 0x00049328 File Offset: 0x00047728
		public bool IsResource(UnityEngine.Object obj)
		{
			return this.m_idToId.ContainsKey(obj.GetInstanceID());
		}

		// Token: 0x06000B80 RID: 2944 RVA: 0x0004933B File Offset: 0x0004773B
		public bool IsDynamicResource(UnityEngine.Object obj)
		{
			return this.m_idToDynamicID.ContainsKey(obj.GetInstanceID());
		}

		// Token: 0x06000B81 RID: 2945 RVA: 0x0004934E File Offset: 0x0004774E
		public static long ToDynamicResourceID(int id)
		{
			return (long)(34359738368UL | (ulong)id);
		}

		// Token: 0x06000B82 RID: 2946 RVA: 0x0004935C File Offset: 0x0004775C
		public static bool IsNotMapped(UnityEngine.Object obj)
		{
			long mappedInstanceID = obj.GetMappedInstanceID();
			return (mappedInstanceID & 17179869184L) != 0L || (mappedInstanceID & 4294967296L) != 0L;
		}

		// Token: 0x06000B83 RID: 2947 RVA: 0x00049397 File Offset: 0x00047797
		public static bool IsDynamicResourceID(long mappedInstanceID)
		{
			return (mappedInstanceID & 34359738368L) != 0L;
		}

		// Token: 0x06000B84 RID: 2948 RVA: 0x000493AC File Offset: 0x000477AC
		public long GetMappedInstanceID(UnityEngine.Object obj)
		{
			if (obj == null)
			{
				return 4294967296L;
			}
			int instanceID = obj.GetInstanceID();
			return this.GetMappedInstanceID(instanceID);
		}

		// Token: 0x06000B85 RID: 2949 RVA: 0x000493E0 File Offset: 0x000477E0
		public long GetMappedInstanceID(int instanceId)
		{
			int num;
			long result;
			if (this.m_idToId.TryGetValue(instanceId, out num))
			{
				result = (long)(8589934592UL | (ulong)num);
			}
			else if (this.m_idToDynamicID.TryGetValue(instanceId, out num))
			{
				result = (long)(34359738368UL | (ulong)num);
			}
			else
			{
				result = (long)(17179869184UL | (ulong)instanceId);
			}
			return result;
		}

		// Token: 0x06000B86 RID: 2950 RVA: 0x00049448 File Offset: 0x00047848
		public long[] GetMappedInstanceID(UnityEngine.Object[] obj)
		{
			if (obj == null)
			{
				return null;
			}
			long[] array = new long[obj.Length];
			for (int i = 0; i < obj.Length; i++)
			{
				UnityEngine.Object obj2 = obj[i];
				array[i] = this.GetMappedInstanceID(obj2);
			}
			return array;
		}

		// Token: 0x06000B87 RID: 2951 RVA: 0x0004948C File Offset: 0x0004788C
		public void Register(UnityEngine.Object obj, int id)
		{
			if (!this.m_dynamicIDToID.ContainsKey(id))
			{
				int instanceID = obj.GetInstanceID();
				if (!this.m_idToDynamicID.ContainsKey(instanceID))
				{
					this.m_idToDynamicID.Add(instanceID, id);
					this.m_dynamicIDToID.Add(id, instanceID);
				}
			}
		}

		// Token: 0x06000B88 RID: 2952 RVA: 0x000494DC File Offset: 0x000478DC
		public void Register(UnityEngine.Object obj, long mappedInstanceId)
		{
			if (IdentifiersMap.IsDynamicResourceID(mappedInstanceId))
			{
				int num = (int)mappedInstanceId;
				if (!this.m_dynamicIDToID.ContainsKey(num))
				{
					int instanceID = obj.GetInstanceID();
					if (!this.m_idToDynamicID.ContainsKey(instanceID))
					{
						this.m_idToDynamicID.Add(instanceID, num);
						this.m_dynamicIDToID.Add(num, instanceID);
					}
				}
			}
		}

		// Token: 0x06000B89 RID: 2953 RVA: 0x0004953C File Offset: 0x0004793C
		public void Unregister(long mappedInstanceId)
		{
			if (IdentifiersMap.IsDynamicResourceID(mappedInstanceId))
			{
				int key = (int)mappedInstanceId;
				int key2;
				if (this.m_dynamicIDToID.TryGetValue(key, out key2))
				{
					this.m_idToDynamicID.Remove(key2);
				}
				this.m_dynamicIDToID.Remove(key);
			}
		}

		// Token: 0x06000B8A RID: 2954 RVA: 0x00049584 File Offset: 0x00047984
		public void Register(AssetBundle bundle)
		{
			string[] allAssetNames = bundle.GetAllAssetNames();
			IEnumerable<string> source = allAssetNames;
			if (IdentifiersMap.<>f__am$cache0 == null)
			{
				IdentifiersMap.<>f__am$cache0 = new Func<string, bool>(IdentifiersMap.<Register>m__0);
			}
			string text = source.Where(IdentifiersMap.<>f__am$cache0).FirstOrDefault<string>();
			if (text == null)
			{
				this.GenerateResourceMap(bundle, allAssetNames);
				return;
			}
			GameObject gameObject = bundle.LoadAsset<GameObject>(text);
			if (gameObject == null)
			{
				throw new ArgumentException(string.Format("Unable to register bundle. Bundle {0} does not contain BundleResourceMap", bundle.name), "bundle");
			}
			BundleResourceMap component = gameObject.GetComponent<BundleResourceMap>();
			if (component == null)
			{
				throw new ArgumentException(string.Format("Unable to register bundle. Bundle {0} does not contain BundleResourceMap", bundle.name), "bundle");
			}
			Guid key = new Guid(component.Guid);
			if (this.m_loadedBundles.ContainsKey(key))
			{
				throw new ArgumentException("bundle " + bundle.name + " already loaded", "bundle");
			}
			List<int> list = new List<int>();
			foreach (ResourceGroup group in gameObject.GetComponentsInChildren<ResourceGroup>(true))
			{
				this.LoadMappings(group, false, list);
			}
			this.m_loadedBundles.Add(key, list.ToArray());
		}

		// Token: 0x06000B8B RID: 2955 RVA: 0x000496BC File Offset: 0x00047ABC
		private static uint HashString(string str)
		{
			uint num = 2166136261U;
			for (int i = 0; i < str.Length; i++)
			{
				num ^= (uint)char.ToUpper(str[i]);
				num *= 16777619U;
			}
			return num;
		}

		// Token: 0x06000B8C RID: 2956 RVA: 0x00049700 File Offset: 0x00047B00
		private static Guid BundleToGuid(AssetBundle bundle)
		{
			uint value = IdentifiersMap.HashString(bundle.name);
			byte[] array = new byte[16];
			BitConverter.GetBytes(value).CopyTo(array, 0);
			Guid result = new Guid(array);
			return result;
		}

		// Token: 0x06000B8D RID: 2957 RVA: 0x00049738 File Offset: 0x00047B38
		private static string GenerateUniqueObjectName(AssetBundle bundle, string asset, UnityEngine.Object loadedAsset, UnityEngine.Object obj)
		{
			if (loadedAsset == obj)
			{
				return string.Format("{0}/{1}", bundle.name, asset);
			}
			string text = null;
			GameObject gameObject = null;
			if (obj is GameObject)
			{
				text = obj.name;
				gameObject = (GameObject)obj;
			}
			else if (obj is Component)
			{
				Component[] components = ((Component)obj).GetComponents(obj.GetType());
				for (int i = 0; i < components.Length; i++)
				{
					if (components[i] == obj)
					{
						text = string.Format("{0}.{1}:{2}", obj.name, obj.GetType().Name, i);
						break;
					}
				}
				gameObject = ((Component)obj).gameObject;
			}
			else
			{
				text = string.Format("{0}.{1}", obj.name, obj.GetType().Name);
			}
			if (gameObject != null && gameObject != loadedAsset)
			{
				Transform transform = gameObject.transform;
				while (gameObject != loadedAsset && transform != null)
				{
					text = string.Format("{0}/{1}", gameObject.name, text);
					transform = transform.parent;
					gameObject = transform.gameObject;
				}
			}
			return string.Format("{0}/{1}/{2}", bundle.name, asset, text);
		}

		// Token: 0x06000B8E RID: 2958 RVA: 0x0004988C File Offset: 0x00047C8C
		private void GenerateResourceMap(AssetBundle bundle, string[] assets)
		{
			Guid key = IdentifiersMap.BundleToGuid(bundle);
			if (this.m_loadedBundles.ContainsKey(key))
			{
				throw new ArgumentException("bundle " + bundle.name + " already loaded", "bundle");
			}
			List<int> list = new List<int>(assets.Length);
			foreach (string text in assets)
			{
				UnityEngine.Object @object = bundle.LoadAsset(text);
				Dictionary<long, UnityEngine.Object> dictionary = new Dictionary<long, UnityEngine.Object>();
				this.GetDependencies(@object, dictionary);
				this.GetReferencedObjects(@object, dictionary);
				foreach (UnityEngine.Object object2 in dictionary.Values)
				{
					int instanceID = object2.GetInstanceID();
					if (!this.m_idToId.ContainsKey(instanceID))
					{
						string str = IdentifiersMap.GenerateUniqueObjectName(bundle, text, @object, object2);
						int value = (int)IdentifiersMap.HashString(str);
						this.m_idToId.Add(instanceID, value);
						list.Add(instanceID);
					}
				}
			}
			this.m_loadedBundles.Add(key, list.ToArray());
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x000499C8 File Offset: 0x00047DC8
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

		// Token: 0x06000B90 RID: 2960 RVA: 0x00049AA4 File Offset: 0x00047EA4
		private void GetReferencedObjects(UnityEngine.Object obj, Dictionary<long, UnityEngine.Object> objects)
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
						this.GetReferencedObjects(transform.gameObject, objects);
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

		// Token: 0x06000B91 RID: 2961 RVA: 0x00049B84 File Offset: 0x00047F84
		public void Unregister(AssetBundle bundle)
		{
			string[] allAssetNames = bundle.GetAllAssetNames();
			IEnumerable<string> source = allAssetNames;
			if (IdentifiersMap.<>f__am$cache1 == null)
			{
				IdentifiersMap.<>f__am$cache1 = new Func<string, bool>(IdentifiersMap.<Unregister>m__1);
			}
			string text = source.Where(IdentifiersMap.<>f__am$cache1).FirstOrDefault<string>();
			Guid key;
			if (text == null)
			{
				key = IdentifiersMap.BundleToGuid(bundle);
			}
			else
			{
				GameObject gameObject = bundle.LoadAsset<GameObject>(text);
				if (gameObject == null)
				{
					throw new ArgumentException(string.Format("Unable to unregister bundle. Bundle {0} does not contain BundleResourceMap", bundle.name), "bundle");
				}
				BundleResourceMap component = gameObject.GetComponent<BundleResourceMap>();
				if (component == null)
				{
					throw new ArgumentException(string.Format("Unable to unregister bundle. Bundle {0} does not contain BundleResourceMap", bundle.name), "bundle");
				}
				key = new Guid(component.Guid);
			}
			if (!this.m_loadedBundles.ContainsKey(key))
			{
				return;
			}
			int[] array = this.m_loadedBundles[key];
			for (int i = 0; i < array.Length; i++)
			{
				this.m_idToId.Remove(array[i]);
			}
			this.m_loadedBundles.Remove(key);
		}

		// Token: 0x06000B92 RID: 2962 RVA: 0x00049C98 File Offset: 0x00048098
		private void LoadMappings(ResourceGroup group, bool ignoreConflicts = false, List<int> ids = null)
		{
			if (!group.gameObject.IsPrefab())
			{
				PersistentIgnore component = group.GetComponent<PersistentIgnore>();
				if (component == null || component.IsRuntime)
				{
					return;
				}
			}
			ObjectToID[] mapping = group.Mapping;
			for (int i = 0; i < mapping.Length; i++)
			{
				ObjectToID objectToID = mapping[i];
				if (!(objectToID.Object == null))
				{
					int instanceID = objectToID.Object.GetInstanceID();
					if (this.m_idToId.ContainsKey(instanceID))
					{
						if (ignoreConflicts)
						{
							goto IL_117;
						}
						Debug.LogError(string.Concat(new object[]
						{
							"key ",
							instanceID,
							"already added. Group ",
							group.name,
							" guid ",
							group.Guid,
							" mapping ",
							i,
							" mapped object ",
							objectToID.Object
						}));
					}
					this.m_idToId.Add(instanceID, objectToID.Id);
					if (ids != null)
					{
						ids.Add(instanceID);
					}
				}
				IL_117:;
			}
		}

		// Token: 0x06000B93 RID: 2963 RVA: 0x00049DCC File Offset: 0x000481CC
		private void Initialize()
		{
			BundleResourceMap resourceMap = Resources.Load<ResourceMap>("ResourceMap");
			this.Initialize(resourceMap);
		}

		// Token: 0x06000B94 RID: 2964 RVA: 0x00049DEC File Offset: 0x000481EC
		private void Initialize(BundleResourceMap resourceMap)
		{
			if (resourceMap == null)
			{
				Debug.LogWarning("ResourceMap is null. Create Resource map using Tools->Runtime SaveLoad->Create Resource Map menu item");
				return;
			}
			this.m_idToId = new Dictionary<int, int>();
			IdentifiersMap.m_instance = this;
			ResourceGroup[] array = Resources.FindObjectsOfTypeAll<ResourceGroup>();
			IEnumerable<ResourceGroup> source = array;
			if (IdentifiersMap.<>f__am$cache2 == null)
			{
				IdentifiersMap.<>f__am$cache2 = new Func<ResourceGroup, bool>(IdentifiersMap.<Initialize>m__2);
			}
			ResourceGroup[] array2 = source.Where(IdentifiersMap.<>f__am$cache2).ToArray<ResourceGroup>();
			ResourceGroup[] componentsInChildren = resourceMap.GetComponentsInChildren<ResourceGroup>();
			if (componentsInChildren.Length == 0)
			{
				Debug.LogWarning("No resource groups found. Create Resource map using Tools->Runtime SaveLoad->Create Resource Map menu item");
				return;
			}
			foreach (ResourceGroup group in componentsInChildren)
			{
				bool ignoreConflicts = true;
				this.LoadMappings(group, ignoreConflicts, null);
			}
			foreach (ResourceGroup group2 in array2)
			{
				this.LoadMappings(group2, false, null);
			}
		}

		// Token: 0x06000B95 RID: 2965 RVA: 0x00049EBC File Offset: 0x000482BC
		public static Dictionary<long, UnityEngine.Object> FindResources(bool includeDynamicResources)
		{
			Dictionary<long, UnityEngine.Object> dictionary = new Dictionary<long, UnityEngine.Object>();
			foreach (UnityEngine.Object @object in Resources.FindObjectsOfTypeAll<UnityEngine.Object>())
			{
				if (IdentifiersMap.Instance.IsResource(@object) || (includeDynamicResources && IdentifiersMap.Instance.IsDynamicResource(@object)))
				{
					long mappedInstanceID = IdentifiersMap.Instance.GetMappedInstanceID(@object);
					if (mappedInstanceID != 4294967296L)
					{
						if (dictionary.ContainsKey(mappedInstanceID))
						{
							Debug.LogErrorFormat("Resource {0}  with instance id {1} already exists {2}", new object[]
							{
								@object,
								mappedInstanceID,
								dictionary[mappedInstanceID]
							});
						}
						else
						{
							dictionary.Add(mappedInstanceID, @object);
						}
					}
				}
			}
			return dictionary;
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x00049F7F File Offset: 0x0004837F
		[CompilerGenerated]
		private static bool <Register>m__0(string r)
		{
			return r.Contains("resourcemap");
		}

		// Token: 0x06000B97 RID: 2967 RVA: 0x00049F8C File Offset: 0x0004838C
		[CompilerGenerated]
		private static bool <Unregister>m__1(string r)
		{
			return r.Contains("resourcemap");
		}

		// Token: 0x06000B98 RID: 2968 RVA: 0x00049F99 File Offset: 0x00048399
		[CompilerGenerated]
		private static bool <Initialize>m__2(ResourceGroup rg)
		{
			return !rg.gameObject.IsPrefab();
		}

		// Token: 0x04000C90 RID: 3216
		public const string ResourceMapPrefabName = "ResourceMap";

		// Token: 0x04000C91 RID: 3217
		public const long T_NULL = 4294967296L;

		// Token: 0x04000C92 RID: 3218
		private const long T_RESOURCE = 8589934592L;

		// Token: 0x04000C93 RID: 3219
		private const long T_OBJECT = 17179869184L;

		// Token: 0x04000C94 RID: 3220
		private const long T_DYNAMIC_RESOURCE = 34359738368L;

		// Token: 0x04000C95 RID: 3221
		private Dictionary<int, int> m_idToDynamicID = new Dictionary<int, int>();

		// Token: 0x04000C96 RID: 3222
		private Dictionary<int, int> m_dynamicIDToID = new Dictionary<int, int>();

		// Token: 0x04000C97 RID: 3223
		private Dictionary<int, int> m_idToId = new Dictionary<int, int>();

		// Token: 0x04000C98 RID: 3224
		private Dictionary<Guid, int[]> m_loadedBundles = new Dictionary<Guid, int[]>();

		// Token: 0x04000C99 RID: 3225
		private static IdentifiersMap m_instance;

		// Token: 0x04000C9A RID: 3226
		[CompilerGenerated]
		private static Func<string, bool> <>f__am$cache0;

		// Token: 0x04000C9B RID: 3227
		[CompilerGenerated]
		private static Func<string, bool> <>f__am$cache1;

		// Token: 0x04000C9C RID: 3228
		[CompilerGenerated]
		private static Func<ResourceGroup, bool> <>f__am$cache2;
	}
}
