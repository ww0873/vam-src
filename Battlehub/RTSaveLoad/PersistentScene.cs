using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using ProtoBuf;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000226 RID: 550
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class PersistentScene
	{
		// Token: 0x06000B05 RID: 2821 RVA: 0x00044254 File Offset: 0x00042654
		public PersistentScene()
		{
			this.Descriptors = new PersistentDescriptor[0];
			this.Data = new PersistentData[0];
		}

		// Token: 0x06000B06 RID: 2822 RVA: 0x00044274 File Offset: 0x00042674
		public static void InstantiateGameObjects(PersistentScene scene)
		{
			if (IdentifiersMap.Instance == null)
			{
				Debug.LogError("Create Runtime Resource Map");
				return;
			}
			PersistentScene.DestroyGameObjects();
			if (scene.Data == null && scene.Descriptors == null)
			{
				return;
			}
			if ((scene.Data == null && scene.Descriptors != null) || (scene.Data != null && scene.Descriptors == null))
			{
				throw new ArgumentException("data is corrupted", "scene");
			}
			if (scene.Descriptors.Length == 0)
			{
				return;
			}
			bool includeDynamicResources = true;
			Dictionary<long, UnityEngine.Object> dictionary = IdentifiersMap.FindResources(includeDynamicResources);
			PersistentDescriptor.GetOrCreateGameObjects(scene.Descriptors, dictionary, null);
			PersistentData.RestoreDataAndResolveDependencies(scene.Data, dictionary);
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x00044320 File Offset: 0x00042720
		private static void DestroyGameObjects()
		{
			foreach (GameObject gameObject in SceneManager.GetActiveScene().GetRootGameObjects())
			{
				if (gameObject != null && PersistentScene.DestroyGameObjects(gameObject.transform, false))
				{
					UnityEngine.Object.DestroyImmediate(gameObject);
				}
			}
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x00044378 File Offset: 0x00042778
		private static bool DestroyGameObjects(Transform t, bool forceDestroy)
		{
			bool result = true;
			if (!forceDestroy)
			{
				PersistentIgnore component = t.GetComponent<PersistentIgnore>();
				if (component)
				{
					return component.IsRuntime;
				}
			}
			List<Transform> list = new List<Transform>();
			IEnumerator enumerator = t.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform item = (Transform)obj;
					list.Add(item);
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
			for (int i = 0; i < list.Count; i++)
			{
				Transform transform = list[i];
				if (PersistentScene.DestroyGameObjects(transform, forceDestroy))
				{
					UnityEngine.Object.DestroyImmediate(transform.gameObject);
				}
				else
				{
					transform.SetParent(null, true);
				}
			}
			return result;
		}

		// Token: 0x06000B09 RID: 2825 RVA: 0x0004445C File Offset: 0x0004285C
		public static PersistentScene CreatePersistentScene(params Type[] ignoreTypes)
		{
			if (IdentifiersMap.Instance == null)
			{
				Debug.LogError("Create Runtime Resource Map");
				return null;
			}
			IEnumerable<GameObject> rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();
			if (PersistentScene.<>f__am$cache0 == null)
			{
				PersistentScene.<>f__am$cache0 = new Func<GameObject, int>(PersistentScene.<CreatePersistentScene>m__0);
			}
			GameObject[] gameObjects = rootGameObjects.OrderBy(PersistentScene.<>f__am$cache0).ToArray<GameObject>();
			PersistentScene persistentScene = new PersistentScene();
			PersistentData.CreatePersistentDescriptorsAndData(gameObjects, out persistentScene.Descriptors, out persistentScene.Data);
			return persistentScene;
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x000444CD File Offset: 0x000428CD
		[CompilerGenerated]
		private static int <CreatePersistentScene>m__0(GameObject g)
		{
			return g.transform.GetSiblingIndex();
		}

		// Token: 0x04000C5F RID: 3167
		public PersistentDescriptor[] Descriptors;

		// Token: 0x04000C60 RID: 3168
		public PersistentData[] Data;

		// Token: 0x04000C61 RID: 3169
		[CompilerGenerated]
		private static Func<GameObject, int> <>f__am$cache0;
	}
}
