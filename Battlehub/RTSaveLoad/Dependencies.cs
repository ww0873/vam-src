using System;
using UnityEngine;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000239 RID: 569
	public static class Dependencies
	{
		// Token: 0x17000116 RID: 278
		// (get) Token: 0x06000BD6 RID: 3030 RVA: 0x0004A740 File Offset: 0x00048B40
		public static ISerializer Serializer
		{
			get
			{
				return new Serializer();
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x06000BD7 RID: 3031 RVA: 0x0004A747 File Offset: 0x00048B47
		public static IStorage Storage
		{
			get
			{
				return new FileSystemStorage(Application.persistentDataPath);
			}
		}

		// Token: 0x17000118 RID: 280
		// (get) Token: 0x06000BD8 RID: 3032 RVA: 0x0004A753 File Offset: 0x00048B53
		public static IProject Project
		{
			get
			{
				return new Project();
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x06000BD9 RID: 3033 RVA: 0x0004A75A File Offset: 0x00048B5A
		public static IAssetBundleLoader BundleLoader
		{
			get
			{
				return new AssetBundleLoader();
			}
		}

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x06000BDA RID: 3034 RVA: 0x0004A761 File Offset: 0x00048B61
		public static IProjectManager ProjectManager
		{
			get
			{
				return UnityEngine.Object.FindObjectOfType<ProjectManager>();
			}
		}

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x06000BDB RID: 3035 RVA: 0x0004A768 File Offset: 0x00048B68
		public static ISceneManager SceneManager
		{
			get
			{
				return UnityEngine.Object.FindObjectOfType<RuntimeSceneManager>();
			}
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000BDC RID: 3036 RVA: 0x0004A76F File Offset: 0x00048B6F
		public static IRuntimeShaderUtil ShaderUtil
		{
			get
			{
				return new RuntimeShaderUtil();
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000BDD RID: 3037 RVA: 0x0004A778 File Offset: 0x00048B78
		public static IJob Job
		{
			get
			{
				Job job = UnityEngine.Object.FindObjectOfType<Job>();
				if (job == null)
				{
					GameObject gameObject = new GameObject();
					gameObject.name = "Job";
					job = gameObject.AddComponent<Job>();
					gameObject.AddComponent<PersistentIgnore>();
				}
				return job;
			}
		}
	}
}
