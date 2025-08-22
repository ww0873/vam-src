using System;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000253 RID: 595
	public interface IProject
	{
		// Token: 0x06000C51 RID: 3153
		void LoadProject(string name, ProjectEventHandler<ProjectRoot> callback, bool metaOnly = false, params int[] exceptTypes);

		// Token: 0x06000C52 RID: 3154
		void SaveProjectMeta(string name, ProjectMeta meta, ProjectEventHandler callback);

		// Token: 0x06000C53 RID: 3155
		void Save(ProjectItem item, bool metaOnly, ProjectEventHandler callback);

		// Token: 0x06000C54 RID: 3156
		void Save(ProjectItem[] items, bool metaOnly, ProjectEventHandler callback);

		// Token: 0x06000C55 RID: 3157
		void Load(string[] path, ProjectEventHandler<ProjectItem[]> callback, params int[] exceptTypes);

		// Token: 0x06000C56 RID: 3158
		void LoadData(ProjectItem[] items, ProjectEventHandler<ProjectItem[]> callback, params int[] exceptTypes);

		// Token: 0x06000C57 RID: 3159
		void UnloadData(ProjectItem item);

		// Token: 0x06000C58 RID: 3160
		void Delete(ProjectItem item, ProjectEventHandler callback);

		// Token: 0x06000C59 RID: 3161
		void Delete(ProjectItem[] items, ProjectEventHandler callback);

		// Token: 0x06000C5A RID: 3162
		void Move(ProjectItem item, ProjectItem parent, ProjectEventHandler callback);

		// Token: 0x06000C5B RID: 3163
		void Move(ProjectItem[] items, ProjectItem parent, ProjectEventHandler callback);

		// Token: 0x06000C5C RID: 3164
		void Rename(ProjectItem item, string name, ProjectEventHandler callback);

		// Token: 0x06000C5D RID: 3165
		void Exists(ProjectItem item, ProjectEventHandler<bool> callback);
	}
}
