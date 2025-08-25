using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Battlehub.RTCommon;
using UnityEngine;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000201 RID: 513
	public class AddResourcesTest : MonoBehaviour
	{
		// Token: 0x06000A47 RID: 2631 RVA: 0x0003E818 File Offset: 0x0003CC18
		public AddResourcesTest()
		{
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x0003E878 File Offset: 0x0003CC78
		private void Start()
		{
			this.m_projectManager = Dependencies.ProjectManager;
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x0003E888 File Offset: 0x0003CC88
		private void Update()
		{
			if (InputController.GetKeyDown(this.AddAssetBundleKey2))
			{
				ProjectItem project = this.m_projectManager.Project;
				IProjectManager projectManager = this.m_projectManager;
				ProjectItem folder = project;
				string bundleName = "bundledemo";
				if (AddResourcesTest.<>f__am$cache0 == null)
				{
					AddResourcesTest.<>f__am$cache0 = new Func<UnityEngine.Object, string, bool>(AddResourcesTest.<Update>m__0);
				}
				Func<UnityEngine.Object, string, bool> filter = AddResourcesTest.<>f__am$cache0;
				if (AddResourcesTest.<>f__am$cache1 == null)
				{
					AddResourcesTest.<>f__am$cache1 = new ProjectManagerCallback<ProjectItem[]>(AddResourcesTest.<Update>m__1);
				}
				projectManager.AddBundledResources(folder, bundleName, filter, AddResourcesTest.<>f__am$cache1);
			}
			if (InputController.GetKeyDown(this.AddAssetBundleKey))
			{
				ProjectItem project2 = this.m_projectManager.Project;
				IProjectManager projectManager2 = this.m_projectManager;
				ProjectItem folder2 = project2;
				string bundleName2 = "bundledemo";
				string assetName = "monkey";
				if (AddResourcesTest.<>f__am$cache2 == null)
				{
					AddResourcesTest.<>f__am$cache2 = new ProjectManagerCallback<ProjectItem[]>(AddResourcesTest.<Update>m__2);
				}
				projectManager2.AddBundledResource(folder2, bundleName2, assetName, AddResourcesTest.<>f__am$cache2);
			}
			if (InputController.GetKeyDown(this.AddWithDependenciesKey))
			{
				AddResourcesTest.<Update>c__AnonStorey0 <Update>c__AnonStorey = new AddResourcesTest.<Update>c__AnonStorey0();
				ProjectItem project3 = this.m_projectManager.Project;
				<Update>c__AnonStorey.objects = new List<UnityEngine.Object>();
				Material material = new Material(Shader.Find("Standard"));
				material.color = Color.yellow;
				Mesh mesh = RuntimeGraphics.CreateCubeMesh(Color.white, Vector3.zero, 1f, 1f, 1f, 1f);
				mesh.name = "TestMesh";
				GameObject gameObject = new GameObject();
				MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();
				MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
				gameObject.name = "TestGO";
				meshRenderer.sharedMaterial = material;
				meshFilter.sharedMesh = mesh;
				<Update>c__AnonStorey.objects.Add(gameObject);
				bool includingDependencies = true;
				if (AddResourcesTest.<>f__am$cache3 == null)
				{
					AddResourcesTest.<>f__am$cache3 = new Func<UnityEngine.Object, bool>(AddResourcesTest.<Update>m__3);
				}
				Func<UnityEngine.Object, bool> filter2 = AddResourcesTest.<>f__am$cache3;
				this.m_projectManager.AddDynamicResources(project3, <Update>c__AnonStorey.objects.ToArray(), includingDependencies, filter2, new ProjectManagerCallback<ProjectItem[]>(<Update>c__AnonStorey.<>m__0));
			}
			if (InputController.GetKeyDown(this.AddInstantiatedObjectKey))
			{
				AddResourcesTest.<Update>c__AnonStorey1 <Update>c__AnonStorey2 = new AddResourcesTest.<Update>c__AnonStorey1();
				ProjectItem project4 = this.m_projectManager.Project;
				<Update>c__AnonStorey2.objects = new List<UnityEngine.Object>();
				Material material2 = new Material(Shader.Find("Standard"));
				material2.color = Color.yellow;
				Mesh mesh2 = RuntimeGraphics.CreateCubeMesh(Color.white, Vector3.zero, 1f, 1f, 1f, 1f);
				mesh2.name = "TestMesh";
				GameObject gameObject2 = new GameObject();
				MeshRenderer meshRenderer2 = gameObject2.AddComponent<MeshRenderer>();
				MeshFilter meshFilter2 = gameObject2.AddComponent<MeshFilter>();
				gameObject2.name = "TestGO";
				meshRenderer2.sharedMaterial = material2;
				meshFilter2.sharedMesh = mesh2;
				<Update>c__AnonStorey2.objects.Add(material2);
				<Update>c__AnonStorey2.objects.Add(mesh2);
				<Update>c__AnonStorey2.objects.Add(gameObject2);
				this.m_projectManager.AddDynamicResources(project4, <Update>c__AnonStorey2.objects.ToArray(), new ProjectManagerCallback<ProjectItem[]>(<Update>c__AnonStorey2.<>m__0));
			}
			if (InputController.GetKeyDown(this.AddPrefabKey))
			{
				ProjectItem project5 = this.m_projectManager.Project;
				List<UnityEngine.Object> list = new List<UnityEngine.Object>();
				if (this.Prefab != null)
				{
					list.Add(this.Prefab);
				}
				bool flag = true;
				if (AddResourcesTest.<>f__am$cache4 == null)
				{
					AddResourcesTest.<>f__am$cache4 = new Func<UnityEngine.Object, bool>(AddResourcesTest.<Update>m__4);
				}
				Func<UnityEngine.Object, bool> func = AddResourcesTest.<>f__am$cache4;
				IProjectManager projectManager3 = this.m_projectManager;
				ProjectItem folder3 = project5;
				UnityEngine.Object[] objects = list.ToArray();
				bool includingDependencies2 = flag;
				Func<UnityEngine.Object, bool> filter3 = func;
				if (AddResourcesTest.<>f__am$cache5 == null)
				{
					AddResourcesTest.<>f__am$cache5 = new ProjectManagerCallback<ProjectItem[]>(AddResourcesTest.<Update>m__5);
				}
				projectManager3.AddDynamicResources(folder3, objects, includingDependencies2, filter3, AddResourcesTest.<>f__am$cache5);
			}
			if (InputController.GetKeyDown(this.AddTextureKey))
			{
				ProjectItem project6 = this.m_projectManager.Project;
				List<UnityEngine.Object> list2 = new List<UnityEngine.Object>();
				string text = Application.streamingAssetsPath + "/" + this.ImagePath;
				Texture2D texture2D = AddResourcesTest.LoadPNG(text);
				if (texture2D == null)
				{
					Debug.LogErrorFormat("File {0} not found", new object[]
					{
						text
					});
					return;
				}
				texture2D.name = "TestTexture";
				list2.Add(texture2D);
				IProjectManager projectManager4 = this.m_projectManager;
				ProjectItem folder4 = project6;
				UnityEngine.Object[] objects2 = list2.ToArray();
				if (AddResourcesTest.<>f__am$cache6 == null)
				{
					AddResourcesTest.<>f__am$cache6 = new ProjectManagerCallback<ProjectItem[]>(AddResourcesTest.<Update>m__6);
				}
				projectManager4.AddDynamicResources(folder4, objects2, AddResourcesTest.<>f__am$cache6);
			}
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x0003ECA0 File Offset: 0x0003D0A0
		public static Texture2D LoadPNG(string filePath)
		{
			Texture2D texture2D = null;
			if (File.Exists(filePath))
			{
				byte[] data = File.ReadAllBytes(filePath);
				texture2D = new Texture2D(1, 1, TextureFormat.ARGB32, true);
				texture2D.LoadImage(data);
			}
			return texture2D;
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x0003ECD4 File Offset: 0x0003D0D4
		[CompilerGenerated]
		private static bool <Update>m__0(UnityEngine.Object obj, string assetName)
		{
			return true;
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x0003ECD8 File Offset: 0x0003D0D8
		[CompilerGenerated]
		private static void <Update>m__1(ProjectItem[] addedItems)
		{
			for (int i = 0; i < addedItems.Length; i++)
			{
				Debug.Log(addedItems[i].ToString() + " added");
			}
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x0003ED10 File Offset: 0x0003D110
		[CompilerGenerated]
		private static void <Update>m__2(ProjectItem[] addedItems)
		{
			for (int i = 0; i < addedItems.Length; i++)
			{
				Debug.Log(addedItems[i].ToString() + " added");
			}
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x0003ED48 File Offset: 0x0003D148
		[CompilerGenerated]
		private static bool <Update>m__3(UnityEngine.Object o)
		{
			return !(o is Shader);
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x0003ED58 File Offset: 0x0003D158
		[CompilerGenerated]
		private static bool <Update>m__4(UnityEngine.Object o)
		{
			return !(o is Shader);
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x0003ED68 File Offset: 0x0003D168
		[CompilerGenerated]
		private static void <Update>m__5(ProjectItem[] addedItems)
		{
			for (int i = 0; i < addedItems.Length; i++)
			{
				Debug.Log(addedItems[i].ToString() + " added");
			}
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x0003EDA0 File Offset: 0x0003D1A0
		[CompilerGenerated]
		private static void <Update>m__6(ProjectItem[] addedItems)
		{
			for (int i = 0; i < addedItems.Length; i++)
			{
				Debug.Log(addedItems[i].ToString() + " added");
			}
		}

		// Token: 0x04000B55 RID: 2901
		public KeyCode AddInstantiatedObjectKey = KeyCode.Keypad1;

		// Token: 0x04000B56 RID: 2902
		public KeyCode AddPrefabKey = KeyCode.Keypad2;

		// Token: 0x04000B57 RID: 2903
		public KeyCode AddTextureKey = KeyCode.Keypad3;

		// Token: 0x04000B58 RID: 2904
		public KeyCode AddWithDependenciesKey = KeyCode.Keypad4;

		// Token: 0x04000B59 RID: 2905
		public KeyCode AddAssetBundleKey = KeyCode.Keypad5;

		// Token: 0x04000B5A RID: 2906
		public KeyCode AddAssetBundleKey2 = KeyCode.Keypad6;

		// Token: 0x04000B5B RID: 2907
		public GameObject Prefab;

		// Token: 0x04000B5C RID: 2908
		public string ImagePath = "test.png";

		// Token: 0x04000B5D RID: 2909
		private IProjectManager m_projectManager;

		// Token: 0x04000B5E RID: 2910
		[CompilerGenerated]
		private static Func<UnityEngine.Object, string, bool> <>f__am$cache0;

		// Token: 0x04000B5F RID: 2911
		[CompilerGenerated]
		private static ProjectManagerCallback<ProjectItem[]> <>f__am$cache1;

		// Token: 0x04000B60 RID: 2912
		[CompilerGenerated]
		private static ProjectManagerCallback<ProjectItem[]> <>f__am$cache2;

		// Token: 0x04000B61 RID: 2913
		[CompilerGenerated]
		private static Func<UnityEngine.Object, bool> <>f__am$cache3;

		// Token: 0x04000B62 RID: 2914
		[CompilerGenerated]
		private static Func<UnityEngine.Object, bool> <>f__am$cache4;

		// Token: 0x04000B63 RID: 2915
		[CompilerGenerated]
		private static ProjectManagerCallback<ProjectItem[]> <>f__am$cache5;

		// Token: 0x04000B64 RID: 2916
		[CompilerGenerated]
		private static ProjectManagerCallback<ProjectItem[]> <>f__am$cache6;

		// Token: 0x02000EAA RID: 3754
		[CompilerGenerated]
		private sealed class <Update>c__AnonStorey0
		{
			// Token: 0x06007175 RID: 29045 RVA: 0x0003EDD8 File Offset: 0x0003D1D8
			public <Update>c__AnonStorey0()
			{
			}

			// Token: 0x06007176 RID: 29046 RVA: 0x0003EDE0 File Offset: 0x0003D1E0
			internal void <>m__0(ProjectItem[] addedItems)
			{
				for (int i = 0; i < addedItems.Length; i++)
				{
					Debug.Log(addedItems[i].ToString() + " added");
				}
				for (int j = 0; j < this.objects.Count; j++)
				{
					UnityEngine.Object.Destroy(this.objects[j]);
				}
			}

			// Token: 0x04006546 RID: 25926
			internal List<UnityEngine.Object> objects;
		}

		// Token: 0x02000EAB RID: 3755
		[CompilerGenerated]
		private sealed class <Update>c__AnonStorey1
		{
			// Token: 0x06007177 RID: 29047 RVA: 0x0003EE45 File Offset: 0x0003D245
			public <Update>c__AnonStorey1()
			{
			}

			// Token: 0x06007178 RID: 29048 RVA: 0x0003EE50 File Offset: 0x0003D250
			internal void <>m__0(ProjectItem[] addedItems)
			{
				for (int i = 0; i < addedItems.Length; i++)
				{
					Debug.Log(addedItems[i].ToString() + " added");
				}
				for (int j = 0; j < this.objects.Count; j++)
				{
					UnityEngine.Object.Destroy(this.objects[j]);
				}
			}

			// Token: 0x04006547 RID: 25927
			internal List<UnityEngine.Object> objects;
		}
	}
}
