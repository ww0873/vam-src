using System;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000243 RID: 579
	public interface ISceneManager
	{
		// Token: 0x14000037 RID: 55
		// (add) Token: 0x06000BFD RID: 3069
		// (remove) Token: 0x06000BFE RID: 3070
		event EventHandler<ProjectManagerEventArgs> SceneCreated;

		// Token: 0x14000038 RID: 56
		// (add) Token: 0x06000BFF RID: 3071
		// (remove) Token: 0x06000C00 RID: 3072
		event EventHandler<ProjectManagerEventArgs> SceneSaving;

		// Token: 0x14000039 RID: 57
		// (add) Token: 0x06000C01 RID: 3073
		// (remove) Token: 0x06000C02 RID: 3074
		event EventHandler<ProjectManagerEventArgs> SceneSaved;

		// Token: 0x1400003A RID: 58
		// (add) Token: 0x06000C03 RID: 3075
		// (remove) Token: 0x06000C04 RID: 3076
		event EventHandler<ProjectManagerEventArgs> SceneLoading;

		// Token: 0x1400003B RID: 59
		// (add) Token: 0x06000C05 RID: 3077
		// (remove) Token: 0x06000C06 RID: 3078
		event EventHandler<ProjectManagerEventArgs> SceneLoaded;

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x06000C07 RID: 3079
		ProjectItem ActiveScene { get; }

		// Token: 0x06000C08 RID: 3080
		void Exists(ProjectItem scene, ProjectManagerCallback<bool> callback);

		// Token: 0x06000C09 RID: 3081
		void SaveScene(ProjectItem scene, ProjectManagerCallback callback);

		// Token: 0x06000C0A RID: 3082
		void LoadScene(ProjectItem scene, ProjectManagerCallback callback);

		// Token: 0x06000C0B RID: 3083
		void CreateScene();
	}
}
