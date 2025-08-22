using System;
using UnityEngine;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000244 RID: 580
	public interface IProjectManager : ISceneManager
	{
		// Token: 0x1400003C RID: 60
		// (add) Token: 0x06000C0C RID: 3084
		// (remove) Token: 0x06000C0D RID: 3085
		event EventHandler ProjectLoading;

		// Token: 0x1400003D RID: 61
		// (add) Token: 0x06000C0E RID: 3086
		// (remove) Token: 0x06000C0F RID: 3087
		event EventHandler<ProjectManagerEventArgs> ProjectLoaded;

		// Token: 0x1400003E RID: 62
		// (add) Token: 0x06000C10 RID: 3088
		// (remove) Token: 0x06000C11 RID: 3089
		event EventHandler<ProjectManagerEventArgs> BundledResourcesAdded;

		// Token: 0x1400003F RID: 63
		// (add) Token: 0x06000C12 RID: 3090
		// (remove) Token: 0x06000C13 RID: 3091
		event EventHandler<ProjectManagerEventArgs> DynamicResourcesAdded;

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x06000C14 RID: 3092
		ProjectItem Project { get; }

		// Token: 0x06000C15 RID: 3093
		bool IsResource(UnityEngine.Object obj);

		// Token: 0x06000C16 RID: 3094
		ID GetID(UnityEngine.Object obj);

		// Token: 0x06000C17 RID: 3095
		void LoadProject(string name, ProjectManagerCallback<ProjectItem> callback);

		// Token: 0x06000C18 RID: 3096
		void AddBundledResources(ProjectItem folder, string bundleName, Func<UnityEngine.Object, string, bool> filter, ProjectManagerCallback<ProjectItem[]> callback);

		// Token: 0x06000C19 RID: 3097
		void AddBundledResource(ProjectItem folder, string bundleName, string assetName, ProjectManagerCallback<ProjectItem[]> callback);

		// Token: 0x06000C1A RID: 3098
		void AddBundledResource<T>(ProjectItem folder, string bundleName, string assetName, ProjectManagerCallback<ProjectItem[]> callback);

		// Token: 0x06000C1B RID: 3099
		void AddBundledResource(ProjectItem folder, string bundleName, string assetName, Type assetType, ProjectManagerCallback<ProjectItem[]> callback);

		// Token: 0x06000C1C RID: 3100
		void AddBundledResources(ProjectItem folder, string bundleName, string[] assetNames, ProjectManagerCallback<ProjectItem[]> callback);

		// Token: 0x06000C1D RID: 3101
		void AddBundledResources(ProjectItem folder, string bundleName, string[] assetNames, Type[] assetTypes, Func<UnityEngine.Object, string, bool> filter, ProjectManagerCallback<ProjectItem[]> callback);

		// Token: 0x06000C1E RID: 3102
		void AddDynamicResource(ProjectItem folder, UnityEngine.Object obj, ProjectManagerCallback<ProjectItem[]> callback);

		// Token: 0x06000C1F RID: 3103
		void AddDynamicResources(ProjectItem folder, UnityEngine.Object[] objects, ProjectManagerCallback<ProjectItem[]> callback);

		// Token: 0x06000C20 RID: 3104
		void AddDynamicResource(ProjectItem folder, UnityEngine.Object obj, bool includingDependencies, Func<UnityEngine.Object, bool> filter, ProjectManagerCallback<ProjectItem[]> callback);

		// Token: 0x06000C21 RID: 3105
		void AddDynamicResources(ProjectItem folder, UnityEngine.Object[] objects, bool includingDependencies, Func<UnityEngine.Object, bool> filter, ProjectManagerCallback<ProjectItem[]> callback);

		// Token: 0x06000C22 RID: 3106
		void CreateFolder(string name, ProjectItem parent, ProjectManagerCallback<ProjectItem> callback);

		// Token: 0x06000C23 RID: 3107
		void SaveObjects(ProjectItemObjectPair[] itemObjectPairs, ProjectManagerCallback callback);

		// Token: 0x06000C24 RID: 3108
		void GetOrCreateObjects(ProjectItem folder, ProjectManagerCallback<ProjectItemObjectPair[]> callback);

		// Token: 0x06000C25 RID: 3109
		void GetOrCreateObjects(ProjectItem[] projectItems, ProjectManagerCallback<ProjectItemObjectPair[]> callback);

		// Token: 0x06000C26 RID: 3110
		void Duplicate(ProjectItem[] projectItems, ProjectManagerCallback<ProjectItem[]> callback);

		// Token: 0x06000C27 RID: 3111
		void Rename(ProjectItem projectItem, string newName, ProjectManagerCallback callback);

		// Token: 0x06000C28 RID: 3112
		void Move(ProjectItem[] projectItems, ProjectItem folder, ProjectManagerCallback callback);

		// Token: 0x06000C29 RID: 3113
		void Delete(ProjectItem[] projectItems, ProjectManagerCallback callback);

		// Token: 0x06000C2A RID: 3114
		void IgnoreTypes(params Type[] type);
	}
}
