using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000254 RID: 596
	public class Project : IProject
	{
		// Token: 0x06000C5E RID: 3166 RVA: 0x0004AFA4 File Offset: 0x000493A4
		public Project()
		{
		}

		// Token: 0x06000C5F RID: 3167 RVA: 0x0004AFC4 File Offset: 0x000493C4
		public void Parallel(Action<ProjectEventHandler>[] actions, ProjectEventHandler callback)
		{
			Project.<Parallel>c__AnonStorey0 <Parallel>c__AnonStorey = new Project.<Parallel>c__AnonStorey0();
			<Parallel>c__AnonStorey.callback = callback;
			if (actions == null || actions.Length == 0)
			{
				<Parallel>c__AnonStorey.callback(new ProjectPayload());
			}
			<Parallel>c__AnonStorey.counter = actions.Length;
			<Parallel>c__AnonStorey.hasError = false;
			foreach (Action<ProjectEventHandler> action in actions)
			{
				action(new ProjectEventHandler(<Parallel>c__AnonStorey.<>m__0));
			}
		}

		// Token: 0x06000C60 RID: 3168 RVA: 0x0004B038 File Offset: 0x00049438
		public void LoadProject(string name, ProjectEventHandler<ProjectRoot> callback, bool metaOnly = true, params int[] exceptTypes)
		{
			Project.<LoadProject>c__AnonStorey1 <LoadProject>c__AnonStorey = new Project.<LoadProject>c__AnonStorey1();
			<LoadProject>c__AnonStorey.name = name;
			<LoadProject>c__AnonStorey.metaOnly = metaOnly;
			<LoadProject>c__AnonStorey.callback = callback;
			<LoadProject>c__AnonStorey.$this = this;
			<LoadProject>c__AnonStorey.exceptTypesHs = null;
			if (exceptTypes != null && exceptTypes.Length > 0)
			{
				<LoadProject>c__AnonStorey.exceptTypesHs = new HashSet<int>();
				for (int i = 0; i < exceptTypes.Length; i++)
				{
					if (!<LoadProject>c__AnonStorey.exceptTypesHs.Contains(exceptTypes[i]))
					{
						<LoadProject>c__AnonStorey.exceptTypesHs.Add(exceptTypes[i]);
					}
				}
			}
			this.m_storage.CheckFolderExists(<LoadProject>c__AnonStorey.name, new StorageEventHandler<string, bool>(<LoadProject>c__AnonStorey.<>m__0));
		}

		// Token: 0x06000C61 RID: 3169 RVA: 0x0004B0E0 File Offset: 0x000494E0
		private void LoadFiles(ProjectItem item, ProjectEventHandler callback, bool metaOnly = true, HashSet<int> exceptTypesHS = null)
		{
			Project.<LoadFiles>c__AnonStorey4 <LoadFiles>c__AnonStorey = new Project.<LoadFiles>c__AnonStorey4();
			<LoadFiles>c__AnonStorey.metaOnly = metaOnly;
			<LoadFiles>c__AnonStorey.exceptTypesHS = exceptTypesHS;
			<LoadFiles>c__AnonStorey.callback = callback;
			<LoadFiles>c__AnonStorey.item = item;
			<LoadFiles>c__AnonStorey.$this = this;
			ProjectItem[] array;
			if (<LoadFiles>c__AnonStorey.item.Children != null)
			{
				IEnumerable<ProjectItem> children = <LoadFiles>c__AnonStorey.item.Children;
				if (Project.<>f__am$cache0 == null)
				{
					Project.<>f__am$cache0 = new Func<ProjectItem, bool>(Project.<LoadFiles>m__0);
				}
				array = children.Where(Project.<>f__am$cache0).ToArray<ProjectItem>();
			}
			else
			{
				array = new ProjectItem[0];
			}
			ProjectItem[] array2 = array;
			<LoadFiles>c__AnonStorey.loadFilesActions = new Action<ProjectEventHandler>[array2.Length];
			for (int i = 0; i < array2.Length; i++)
			{
				Project.<LoadFiles>c__AnonStorey3 <LoadFiles>c__AnonStorey2 = new Project.<LoadFiles>c__AnonStorey3();
				<LoadFiles>c__AnonStorey2.<>f__ref$4 = <LoadFiles>c__AnonStorey;
				<LoadFiles>c__AnonStorey2.childItem = array2[i];
				<LoadFiles>c__AnonStorey.loadFilesActions[i] = new Action<ProjectEventHandler>(<LoadFiles>c__AnonStorey2.<>m__0);
			}
			this.m_storage.GetFiles(<LoadFiles>c__AnonStorey.item.ToString(), new StorageEventHandler<string, string[]>(<LoadFiles>c__AnonStorey.<>m__0), true);
		}

		// Token: 0x06000C62 RID: 3170 RVA: 0x0004B1D4 File Offset: 0x000495D4
		private void LoadFolders(ProjectItem item, ProjectEventHandler callback)
		{
			Project.<LoadFolders>c__AnonStorey7 <LoadFolders>c__AnonStorey = new Project.<LoadFolders>c__AnonStorey7();
			<LoadFolders>c__AnonStorey.item = item;
			<LoadFolders>c__AnonStorey.callback = callback;
			<LoadFolders>c__AnonStorey.$this = this;
			this.m_storage.GetFolders(<LoadFolders>c__AnonStorey.item.ToString(), new StorageEventHandler<string, string[]>(<LoadFolders>c__AnonStorey.<>m__0), false);
		}

		// Token: 0x06000C63 RID: 3171 RVA: 0x0004B220 File Offset: 0x00049620
		public void SaveProjectMeta(string name, ProjectMeta meta, ProjectEventHandler callback)
		{
			Project.<SaveProjectMeta>c__AnonStorey9 <SaveProjectMeta>c__AnonStorey = new Project.<SaveProjectMeta>c__AnonStorey9();
			<SaveProjectMeta>c__AnonStorey.callback = callback;
			this.m_storage.SaveFile(name + ".rtpmeta", this.m_serializer.Serialize<ProjectMeta>(meta), new StorageEventHandler<string>(<SaveProjectMeta>c__AnonStorey.<>m__0));
		}

		// Token: 0x06000C64 RID: 3172 RVA: 0x0004B268 File Offset: 0x00049668
		public void Load(string[] path, ProjectEventHandler<ProjectItem[]> callback, params int[] exceptTypes)
		{
			Project.<Load>c__AnonStoreyA <Load>c__AnonStoreyA = new Project.<Load>c__AnonStoreyA();
			<Load>c__AnonStoreyA.callback = callback;
			<Load>c__AnonStoreyA.$this = this;
			<Load>c__AnonStoreyA.pathLength = path.Length;
			Array.Resize<string>(ref path, <Load>c__AnonStoreyA.pathLength + <Load>c__AnonStoreyA.pathLength);
			for (int i = 0; i < <Load>c__AnonStoreyA.pathLength; i++)
			{
				path[<Load>c__AnonStoreyA.pathLength + i] = path[i] + ".rtmeta";
			}
			<Load>c__AnonStoreyA.exceptTypesHs = null;
			if (exceptTypes != null && exceptTypes.Length > 0)
			{
				<Load>c__AnonStoreyA.exceptTypesHs = new HashSet<int>();
				for (int j = 0; j < exceptTypes.Length; j++)
				{
					if (!<Load>c__AnonStoreyA.exceptTypesHs.Contains(exceptTypes[j]))
					{
						<Load>c__AnonStoreyA.exceptTypesHs.Add(exceptTypes[j]);
					}
				}
			}
			this.m_storage.LoadFiles(path, new StorageEventHandler<string[], byte[][]>(<Load>c__AnonStoreyA.<>m__0));
		}

		// Token: 0x06000C65 RID: 3173 RVA: 0x0004B344 File Offset: 0x00049744
		public void LoadData(ProjectItem[] items, ProjectEventHandler<ProjectItem[]> callback, params int[] exceptTypes)
		{
			Project.<LoadData>c__AnonStoreyB <LoadData>c__AnonStoreyB = new Project.<LoadData>c__AnonStoreyB();
			<LoadData>c__AnonStoreyB.exceptTypes = exceptTypes;
			<LoadData>c__AnonStoreyB.items = items;
			<LoadData>c__AnonStoreyB.callback = callback;
			<LoadData>c__AnonStoreyB.$this = this;
			if (<LoadData>c__AnonStoreyB.exceptTypes != null && <LoadData>c__AnonStoreyB.exceptTypes.Length > 0)
			{
				HashSet<int> hashSet = new HashSet<int>();
				for (int i = 0; i < <LoadData>c__AnonStoreyB.exceptTypes.Length; i++)
				{
					if (!hashSet.Contains(<LoadData>c__AnonStoreyB.exceptTypes[i]))
					{
						hashSet.Add(<LoadData>c__AnonStoreyB.exceptTypes[i]);
					}
				}
			}
			if (<LoadData>c__AnonStoreyB.exceptTypes != null)
			{
				<LoadData>c__AnonStoreyB.items = <LoadData>c__AnonStoreyB.items.Where(new Func<ProjectItem, bool>(<LoadData>c__AnonStoreyB.<>m__0)).ToArray<ProjectItem>();
			}
			Project.<LoadData>c__AnonStoreyB <LoadData>c__AnonStoreyB2 = <LoadData>c__AnonStoreyB;
			IEnumerable<ProjectItem> items2 = <LoadData>c__AnonStoreyB.items;
			if (Project.<>f__am$cache1 == null)
			{
				Project.<>f__am$cache1 = new Func<ProjectItem, string>(Project.<LoadData>m__1);
			}
			<LoadData>c__AnonStoreyB2.path = items2.Select(Project.<>f__am$cache1).ToArray<string>();
			this.m_storage.LoadFiles(<LoadData>c__AnonStoreyB.path, new StorageEventHandler<string[], byte[][]>(<LoadData>c__AnonStoreyB.<>m__1));
		}

		// Token: 0x06000C66 RID: 3174 RVA: 0x0004B44C File Offset: 0x0004984C
		public void Save(ProjectItem item, bool metaOnly, ProjectEventHandler callback)
		{
			this.Save(item, item.ToString(), metaOnly, callback);
		}

		// Token: 0x06000C67 RID: 3175 RVA: 0x0004B460 File Offset: 0x00049860
		private void Save(ProjectItem item, string path, bool metaOnly, ProjectEventHandler callback)
		{
			Project.<Save>c__AnonStoreyC <Save>c__AnonStoreyC = new Project.<Save>c__AnonStoreyC();
			<Save>c__AnonStoreyC.item = item;
			<Save>c__AnonStoreyC.metaOnly = metaOnly;
			<Save>c__AnonStoreyC.callback = callback;
			<Save>c__AnonStoreyC.path = path;
			<Save>c__AnonStoreyC.$this = this;
			if (<Save>c__AnonStoreyC.item.IsFolder)
			{
				ProjectItem[] items = <Save>c__AnonStoreyC.item.FlattenHierarchy(true);
				this.Save(items, <Save>c__AnonStoreyC.metaOnly, <Save>c__AnonStoreyC.callback);
			}
			else
			{
				this.m_storage.SaveFile(<Save>c__AnonStoreyC.path + ".rtmeta", this.m_serializer.Serialize<ProjectItemMeta>(<Save>c__AnonStoreyC.item.Internal_Meta), new StorageEventHandler<string>(<Save>c__AnonStoreyC.<>m__0));
			}
		}

		// Token: 0x06000C68 RID: 3176 RVA: 0x0004B50C File Offset: 0x0004990C
		public void Save(ProjectItem[] items, bool metaOnly, ProjectEventHandler callback)
		{
			Project.<Save>c__AnonStoreyD <Save>c__AnonStoreyD = new Project.<Save>c__AnonStoreyD();
			<Save>c__AnonStoreyD.callback = callback;
			<Save>c__AnonStoreyD.$this = this;
			if (Project.<>f__am$cache2 == null)
			{
				Project.<>f__am$cache2 = new Func<ProjectItem, bool>(Project.<Save>m__2);
			}
			ProjectItem[] array = items.Where(Project.<>f__am$cache2).ToArray<ProjectItem>();
			IEnumerable<ProjectItem> source = array;
			if (Project.<>f__am$cache3 == null)
			{
				Project.<>f__am$cache3 = new Func<ProjectItem, string>(Project.<Save>m__3);
			}
			string[] path = source.Select(Project.<>f__am$cache3).ToArray<string>();
			Project.<Save>c__AnonStoreyD <Save>c__AnonStoreyD2 = <Save>c__AnonStoreyD;
			byte[][] fileData;
			if (metaOnly)
			{
				fileData = items.Select(new Func<ProjectItem, byte[]>(<Save>c__AnonStoreyD.<>m__0)).ToArray<byte[]>();
			}
			else
			{
				if (Project.<>f__am$cache4 == null)
				{
					Project.<>f__am$cache4 = new Func<ProjectItem, bool>(Project.<Save>m__4);
				}
				fileData = items.Where(Project.<>f__am$cache4).Select(new Func<ProjectItem, byte[]>(<Save>c__AnonStoreyD.<>m__1)).Union(items.Select(new Func<ProjectItem, byte[]>(<Save>c__AnonStoreyD.<>m__2))).ToArray<byte[]>();
			}
			<Save>c__AnonStoreyD2.fileData = fileData;
			Project.<Save>c__AnonStoreyD <Save>c__AnonStoreyD3 = <Save>c__AnonStoreyD;
			string[] filePath;
			if (metaOnly)
			{
				if (Project.<>f__am$cache5 == null)
				{
					Project.<>f__am$cache5 = new Func<ProjectItem, string>(Project.<Save>m__5);
				}
				filePath = items.Select(Project.<>f__am$cache5).ToArray<string>();
			}
			else
			{
				if (Project.<>f__am$cache6 == null)
				{
					Project.<>f__am$cache6 = new Func<ProjectItem, bool>(Project.<Save>m__6);
				}
				IEnumerable<ProjectItem> source2 = items.Where(Project.<>f__am$cache6);
				if (Project.<>f__am$cache7 == null)
				{
					Project.<>f__am$cache7 = new Func<ProjectItem, string>(Project.<Save>m__7);
				}
				IEnumerable<string> first = source2.Select(Project.<>f__am$cache7);
				if (Project.<>f__am$cache8 == null)
				{
					Project.<>f__am$cache8 = new Func<ProjectItem, string>(Project.<Save>m__8);
				}
				filePath = first.Union(items.Select(Project.<>f__am$cache8)).ToArray<string>();
			}
			<Save>c__AnonStoreyD3.filePath = filePath;
			this.m_storage.CreateFolders(path, new StorageEventHandler<string[]>(<Save>c__AnonStoreyD.<>m__3));
		}

		// Token: 0x06000C69 RID: 3177 RVA: 0x0004B6BC File Offset: 0x00049ABC
		public void Delete(ProjectItem item, ProjectEventHandler callback)
		{
			Project.<Delete>c__AnonStoreyE <Delete>c__AnonStoreyE = new Project.<Delete>c__AnonStoreyE();
			<Delete>c__AnonStoreyE.callback = callback;
			if (item == null)
			{
				<Delete>c__AnonStoreyE.callback(new ProjectPayload());
				return;
			}
			string text = item.ToString();
			if (item.IsFolder)
			{
				this.m_storage.DeleteFolder(text, new StorageEventHandler<string>(<Delete>c__AnonStoreyE.<>m__0));
			}
			else
			{
				string[] path = new string[]
				{
					text,
					text + ".rtmeta"
				};
				this.m_storage.DeleteFiles(path, new StorageEventHandler<string[]>(<Delete>c__AnonStoreyE.<>m__1));
			}
		}

		// Token: 0x06000C6A RID: 3178 RVA: 0x0004B750 File Offset: 0x00049B50
		public void Delete(ProjectItem[] items, ProjectEventHandler callback)
		{
			items = ProjectItem.GetRootItems(items);
			IEnumerable<ProjectItem> source = items;
			if (Project.<>f__am$cache9 == null)
			{
				Project.<>f__am$cache9 = new Func<ProjectItem, bool>(Project.<Delete>m__9);
			}
			IEnumerable<ProjectItem> source2 = source.Where(Project.<>f__am$cache9);
			if (Project.<>f__am$cacheA == null)
			{
				Project.<>f__am$cacheA = new Func<ProjectItem, string>(Project.<Delete>m__A);
			}
			string[] folderPath = source2.Select(Project.<>f__am$cacheA).ToArray<string>();
			IEnumerable<ProjectItem> source3 = items;
			if (Project.<>f__am$cacheB == null)
			{
				Project.<>f__am$cacheB = new Func<ProjectItem, bool>(Project.<Delete>m__B);
			}
			ProjectItem[] array = source3.Where(Project.<>f__am$cacheB).ToArray<ProjectItem>();
			IEnumerable<ProjectItem> source4 = array;
			if (Project.<>f__am$cacheC == null)
			{
				Project.<>f__am$cacheC = new Func<ProjectItem, string>(Project.<Delete>m__C);
			}
			IEnumerable<string> first = source4.Select(Project.<>f__am$cacheC);
			IEnumerable<ProjectItem> source5 = array;
			if (Project.<>f__am$cacheD == null)
			{
				Project.<>f__am$cacheD = new Func<ProjectItem, string>(Project.<Delete>m__D);
			}
			string[] filePath = first.Union(source5.Select(Project.<>f__am$cacheD)).ToArray<string>();
			this.GroupOperation(folderPath, filePath, new Action<string[], StorageEventHandler<string[]>>(this.m_storage.DeleteFolders), new Action<string[], StorageEventHandler<string[]>>(this.m_storage.DeleteFiles), callback);
		}

		// Token: 0x06000C6B RID: 3179 RVA: 0x0004B858 File Offset: 0x00049C58
		public void Move(ProjectItem item, ProjectItem parent, ProjectEventHandler callback)
		{
			string srcPath = item.ToString();
			parent.AddChild(item);
			item.Name = ProjectItem.GetUniqueName(item.Name, item, item.Parent, true);
			string dstPath = item.ToString();
			this.Move(item, callback, srcPath, dstPath);
		}

		// Token: 0x06000C6C RID: 3180 RVA: 0x0004B8A0 File Offset: 0x00049CA0
		public void Move(ProjectItem[] items, ProjectItem parent, ProjectEventHandler callback)
		{
			items = ProjectItem.GetRootItems(items);
			IEnumerable<ProjectItem> source = items;
			if (Project.<>f__am$cacheE == null)
			{
				Project.<>f__am$cacheE = new Func<ProjectItem, bool>(Project.<Move>m__E);
			}
			IEnumerable<ProjectItem> source2 = source.Where(Project.<>f__am$cacheE);
			if (Project.<>f__am$cacheF == null)
			{
				Project.<>f__am$cacheF = new Func<ProjectItem, string>(Project.<Move>m__F);
			}
			string[] folderPath = source2.Select(Project.<>f__am$cacheF).ToArray<string>();
			IEnumerable<ProjectItem> source3 = items;
			if (Project.<>f__am$cache10 == null)
			{
				Project.<>f__am$cache10 = new Func<ProjectItem, bool>(Project.<Move>m__10);
			}
			ProjectItem[] array = source3.Where(Project.<>f__am$cache10).ToArray<ProjectItem>();
			IEnumerable<ProjectItem> source4 = array;
			if (Project.<>f__am$cache11 == null)
			{
				Project.<>f__am$cache11 = new Func<ProjectItem, string>(Project.<Move>m__11);
			}
			IEnumerable<string> first = source4.Select(Project.<>f__am$cache11);
			IEnumerable<ProjectItem> source5 = array;
			if (Project.<>f__am$cache12 == null)
			{
				Project.<>f__am$cache12 = new Func<ProjectItem, string>(Project.<Move>m__12);
			}
			string[] filePath = first.Union(source5.Select(Project.<>f__am$cache12)).ToArray<string>();
			foreach (ProjectItem projectItem in items)
			{
				parent.AddChild(projectItem);
				projectItem.Name = ProjectItem.GetUniqueName(projectItem.Name, projectItem, projectItem.Parent, true);
			}
			IEnumerable<ProjectItem> source6 = items;
			if (Project.<>f__am$cache13 == null)
			{
				Project.<>f__am$cache13 = new Func<ProjectItem, bool>(Project.<Move>m__13);
			}
			IEnumerable<ProjectItem> source7 = source6.Where(Project.<>f__am$cache13);
			if (Project.<>f__am$cache14 == null)
			{
				Project.<>f__am$cache14 = new Func<ProjectItem, string>(Project.<Move>m__14);
			}
			string[] folderDstPath = source7.Select(Project.<>f__am$cache14).ToArray<string>();
			IEnumerable<ProjectItem> source8 = array;
			if (Project.<>f__am$cache15 == null)
			{
				Project.<>f__am$cache15 = new Func<ProjectItem, string>(Project.<Move>m__15);
			}
			IEnumerable<string> first2 = source8.Select(Project.<>f__am$cache15);
			IEnumerable<ProjectItem> source9 = array;
			if (Project.<>f__am$cache16 == null)
			{
				Project.<>f__am$cache16 = new Func<ProjectItem, string>(Project.<Move>m__16);
			}
			string[] fileDstPath = first2.Union(source9.Select(Project.<>f__am$cache16)).ToArray<string>();
			this.GroupOperation(folderPath, filePath, folderDstPath, fileDstPath, new Action<string[], string[], StorageEventHandler<string[], string[]>>(this.m_storage.MoveFolders), new Action<string[], string[], StorageEventHandler<string[], string[]>>(this.m_storage.MoveFiles), callback);
		}

		// Token: 0x06000C6D RID: 3181 RVA: 0x0004BA8C File Offset: 0x00049E8C
		public void Rename(ProjectItem item, string name, ProjectEventHandler callback)
		{
			Project.<Rename>c__AnonStoreyF <Rename>c__AnonStoreyF = new Project.<Rename>c__AnonStoreyF();
			<Rename>c__AnonStoreyF.item = item;
			<Rename>c__AnonStoreyF.name = name;
			<Rename>c__AnonStoreyF.callback = callback;
			<Rename>c__AnonStoreyF.$this = this;
			<Rename>c__AnonStoreyF.srcPath = <Rename>c__AnonStoreyF.item.ToString();
			string name2 = <Rename>c__AnonStoreyF.item.Name;
			<Rename>c__AnonStoreyF.item.Name = ProjectItem.GetUniqueName(<Rename>c__AnonStoreyF.name, <Rename>c__AnonStoreyF.item, <Rename>c__AnonStoreyF.item.Parent, true);
			<Rename>c__AnonStoreyF.dstPath = <Rename>c__AnonStoreyF.item.ToString();
			if (!<Rename>c__AnonStoreyF.item.IsFolder && !<Rename>c__AnonStoreyF.item.IsScene)
			{
				this.m_storage.LoadFile(<Rename>c__AnonStoreyF.srcPath, new StorageEventHandler<string, byte[]>(<Rename>c__AnonStoreyF.<>m__0));
			}
			else if (!<Rename>c__AnonStoreyF.item.IsFolder)
			{
				this.Save(<Rename>c__AnonStoreyF.item, <Rename>c__AnonStoreyF.srcPath, true, new ProjectEventHandler(<Rename>c__AnonStoreyF.<>m__1));
			}
			else
			{
				this.Move(<Rename>c__AnonStoreyF.item, <Rename>c__AnonStoreyF.callback, <Rename>c__AnonStoreyF.srcPath, <Rename>c__AnonStoreyF.dstPath);
			}
		}

		// Token: 0x06000C6E RID: 3182 RVA: 0x0004BBA8 File Offset: 0x00049FA8
		private void Move(ProjectItem item, ProjectEventHandler callback, string srcPath, string dstPath)
		{
			Project.<Move>c__AnonStorey10 <Move>c__AnonStorey = new Project.<Move>c__AnonStorey10();
			<Move>c__AnonStorey.callback = callback;
			if (item.IsFolder)
			{
				this.m_storage.MoveFolder(srcPath, dstPath, new StorageEventHandler<string, string>(<Move>c__AnonStorey.<>m__0));
			}
			else
			{
				this.m_storage.MoveFiles(new string[]
				{
					srcPath,
					srcPath + ".rtmeta"
				}, new string[]
				{
					dstPath,
					dstPath + ".rtmeta"
				}, new StorageEventHandler<string[], string[]>(<Move>c__AnonStorey.<>m__1));
			}
		}

		// Token: 0x06000C6F RID: 3183 RVA: 0x0004BC38 File Offset: 0x0004A038
		private void GroupOperation(string[] folderPath, string[] filePath, string[] folderDstPath, string[] fileDstPath, Action<string[], string[], StorageEventHandler<string[], string[]>> folderOperation, Action<string[], string[], StorageEventHandler<string[], string[]>> fileOperation, ProjectEventHandler callback)
		{
			Project.<GroupOperation>c__AnonStorey11 <GroupOperation>c__AnonStorey = new Project.<GroupOperation>c__AnonStorey11();
			<GroupOperation>c__AnonStorey.filePath = filePath;
			<GroupOperation>c__AnonStorey.fileOperation = fileOperation;
			<GroupOperation>c__AnonStorey.fileDstPath = fileDstPath;
			<GroupOperation>c__AnonStorey.callback = callback;
			if (folderPath.Length > 0)
			{
				folderOperation(folderPath, folderDstPath, new StorageEventHandler<string[], string[]>(<GroupOperation>c__AnonStorey.<>m__0));
			}
			else if (<GroupOperation>c__AnonStorey.filePath.Length > 0)
			{
				<GroupOperation>c__AnonStorey.fileOperation(<GroupOperation>c__AnonStorey.filePath, <GroupOperation>c__AnonStorey.fileDstPath, new StorageEventHandler<string[], string[]>(<GroupOperation>c__AnonStorey.<>m__1));
			}
			else if (<GroupOperation>c__AnonStorey.callback != null)
			{
				<GroupOperation>c__AnonStorey.callback(new ProjectPayload());
			}
		}

		// Token: 0x06000C70 RID: 3184 RVA: 0x0004BCE0 File Offset: 0x0004A0E0
		private void GroupOperation(string[] folderPath, string[] filePath, Action<string[], StorageEventHandler<string[]>> folderOperation, Action<string[], StorageEventHandler<string[]>> fileOperation, ProjectEventHandler callback)
		{
			Project.<GroupOperation>c__AnonStorey12 <GroupOperation>c__AnonStorey = new Project.<GroupOperation>c__AnonStorey12();
			<GroupOperation>c__AnonStorey.filePath = filePath;
			<GroupOperation>c__AnonStorey.fileOperation = fileOperation;
			<GroupOperation>c__AnonStorey.callback = callback;
			if (folderPath.Length > 0)
			{
				folderOperation(folderPath, new StorageEventHandler<string[]>(<GroupOperation>c__AnonStorey.<>m__0));
			}
			else if (<GroupOperation>c__AnonStorey.filePath.Length > 0)
			{
				<GroupOperation>c__AnonStorey.fileOperation(<GroupOperation>c__AnonStorey.filePath, new StorageEventHandler<string[]>(<GroupOperation>c__AnonStorey.<>m__1));
			}
			else if (<GroupOperation>c__AnonStorey.callback != null)
			{
				<GroupOperation>c__AnonStorey.callback(new ProjectPayload());
			}
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x0004BD78 File Offset: 0x0004A178
		public void UnloadData(ProjectItem projectItem)
		{
			if (projectItem == null)
			{
				return;
			}
			projectItem.Internal_Data = null;
			if (projectItem.Children != null)
			{
				for (int i = 0; i < projectItem.Children.Count; i++)
				{
					this.UnloadData(projectItem.Children[i]);
				}
			}
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x0004BDCC File Offset: 0x0004A1CC
		public void Exists(ProjectItem item, ProjectEventHandler<bool> callback)
		{
			Project.<Exists>c__AnonStorey13 <Exists>c__AnonStorey = new Project.<Exists>c__AnonStorey13();
			<Exists>c__AnonStorey.callback = callback;
			if (item.IsFolder)
			{
				this.m_storage.CheckFolderExists(item.ToString(), new StorageEventHandler<string, bool>(<Exists>c__AnonStorey.<>m__0));
			}
			else
			{
				this.m_storage.CheckFileExists(item.ToString(), new StorageEventHandler<string, bool>(<Exists>c__AnonStorey.<>m__1));
			}
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x0004BE30 File Offset: 0x0004A230
		[CompilerGenerated]
		private static bool <LoadFiles>m__0(ProjectItem c)
		{
			return c.IsFolder;
		}

		// Token: 0x06000C74 RID: 3188 RVA: 0x0004BE38 File Offset: 0x0004A238
		[CompilerGenerated]
		private static string <LoadData>m__1(ProjectItem item)
		{
			return item.ToString();
		}

		// Token: 0x06000C75 RID: 3189 RVA: 0x0004BE40 File Offset: 0x0004A240
		[CompilerGenerated]
		private static bool <Save>m__2(ProjectItem item)
		{
			return item.IsFolder;
		}

		// Token: 0x06000C76 RID: 3190 RVA: 0x0004BE48 File Offset: 0x0004A248
		[CompilerGenerated]
		private static string <Save>m__3(ProjectItem item)
		{
			return item.ToString();
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x0004BE50 File Offset: 0x0004A250
		[CompilerGenerated]
		private static bool <Save>m__4(ProjectItem item)
		{
			return item.Internal_Data != null;
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x0004BE5E File Offset: 0x0004A25E
		[CompilerGenerated]
		private static string <Save>m__5(ProjectItem item)
		{
			return item.ToString() + ".rtmeta";
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x0004BE70 File Offset: 0x0004A270
		[CompilerGenerated]
		private static bool <Save>m__6(ProjectItem item)
		{
			return item.Internal_Data != null;
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x0004BE7E File Offset: 0x0004A27E
		[CompilerGenerated]
		private static string <Save>m__7(ProjectItem item)
		{
			return item.ToString();
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x0004BE86 File Offset: 0x0004A286
		[CompilerGenerated]
		private static string <Save>m__8(ProjectItem item)
		{
			return item.ToString() + ".rtmeta";
		}

		// Token: 0x06000C7C RID: 3196 RVA: 0x0004BE98 File Offset: 0x0004A298
		[CompilerGenerated]
		private static bool <Delete>m__9(ProjectItem item)
		{
			return item.IsFolder;
		}

		// Token: 0x06000C7D RID: 3197 RVA: 0x0004BEA0 File Offset: 0x0004A2A0
		[CompilerGenerated]
		private static string <Delete>m__A(ProjectItem item)
		{
			return item.ToString();
		}

		// Token: 0x06000C7E RID: 3198 RVA: 0x0004BEA8 File Offset: 0x0004A2A8
		[CompilerGenerated]
		private static bool <Delete>m__B(ProjectItem item)
		{
			return !item.IsFolder;
		}

		// Token: 0x06000C7F RID: 3199 RVA: 0x0004BEB3 File Offset: 0x0004A2B3
		[CompilerGenerated]
		private static string <Delete>m__C(ProjectItem item)
		{
			return item.ToString();
		}

		// Token: 0x06000C80 RID: 3200 RVA: 0x0004BEBB File Offset: 0x0004A2BB
		[CompilerGenerated]
		private static string <Delete>m__D(ProjectItem item)
		{
			return item.ToString() + ".rtmeta";
		}

		// Token: 0x06000C81 RID: 3201 RVA: 0x0004BECD File Offset: 0x0004A2CD
		[CompilerGenerated]
		private static bool <Move>m__E(ProjectItem item)
		{
			return item.IsFolder;
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x0004BED5 File Offset: 0x0004A2D5
		[CompilerGenerated]
		private static string <Move>m__F(ProjectItem item)
		{
			return item.ToString();
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x0004BEDD File Offset: 0x0004A2DD
		[CompilerGenerated]
		private static bool <Move>m__10(ProjectItem item)
		{
			return !item.IsFolder;
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x0004BEE8 File Offset: 0x0004A2E8
		[CompilerGenerated]
		private static string <Move>m__11(ProjectItem item)
		{
			return item.ToString();
		}

		// Token: 0x06000C85 RID: 3205 RVA: 0x0004BEF0 File Offset: 0x0004A2F0
		[CompilerGenerated]
		private static string <Move>m__12(ProjectItem item)
		{
			return item.ToString() + ".rtmeta";
		}

		// Token: 0x06000C86 RID: 3206 RVA: 0x0004BF02 File Offset: 0x0004A302
		[CompilerGenerated]
		private static bool <Move>m__13(ProjectItem item)
		{
			return item.IsFolder;
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x0004BF0A File Offset: 0x0004A30A
		[CompilerGenerated]
		private static string <Move>m__14(ProjectItem item)
		{
			return item.ToString();
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x0004BF12 File Offset: 0x0004A312
		[CompilerGenerated]
		private static string <Move>m__15(ProjectItem item)
		{
			return item.ToString();
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x0004BF1A File Offset: 0x0004A31A
		[CompilerGenerated]
		private static string <Move>m__16(ProjectItem item)
		{
			return item.ToString() + ".rtmeta";
		}

		// Token: 0x04000CE0 RID: 3296
		private IStorage m_storage = Dependencies.Storage;

		// Token: 0x04000CE1 RID: 3297
		private ISerializer m_serializer = Dependencies.Serializer;

		// Token: 0x04000CE2 RID: 3298
		private const string FileMetaExt = "rtmeta";

		// Token: 0x04000CE3 RID: 3299
		private const string ProjectMetaExt = "rtpmeta";

		// Token: 0x04000CE4 RID: 3300
		private const string ProjectDataExt = "rtpdata";

		// Token: 0x04000CE5 RID: 3301
		[CompilerGenerated]
		private static Func<ProjectItem, bool> <>f__am$cache0;

		// Token: 0x04000CE6 RID: 3302
		[CompilerGenerated]
		private static Func<ProjectItem, string> <>f__am$cache1;

		// Token: 0x04000CE7 RID: 3303
		[CompilerGenerated]
		private static Func<ProjectItem, bool> <>f__am$cache2;

		// Token: 0x04000CE8 RID: 3304
		[CompilerGenerated]
		private static Func<ProjectItem, string> <>f__am$cache3;

		// Token: 0x04000CE9 RID: 3305
		[CompilerGenerated]
		private static Func<ProjectItem, bool> <>f__am$cache4;

		// Token: 0x04000CEA RID: 3306
		[CompilerGenerated]
		private static Func<ProjectItem, string> <>f__am$cache5;

		// Token: 0x04000CEB RID: 3307
		[CompilerGenerated]
		private static Func<ProjectItem, bool> <>f__am$cache6;

		// Token: 0x04000CEC RID: 3308
		[CompilerGenerated]
		private static Func<ProjectItem, string> <>f__am$cache7;

		// Token: 0x04000CED RID: 3309
		[CompilerGenerated]
		private static Func<ProjectItem, string> <>f__am$cache8;

		// Token: 0x04000CEE RID: 3310
		[CompilerGenerated]
		private static Func<ProjectItem, bool> <>f__am$cache9;

		// Token: 0x04000CEF RID: 3311
		[CompilerGenerated]
		private static Func<ProjectItem, string> <>f__am$cacheA;

		// Token: 0x04000CF0 RID: 3312
		[CompilerGenerated]
		private static Func<ProjectItem, bool> <>f__am$cacheB;

		// Token: 0x04000CF1 RID: 3313
		[CompilerGenerated]
		private static Func<ProjectItem, string> <>f__am$cacheC;

		// Token: 0x04000CF2 RID: 3314
		[CompilerGenerated]
		private static Func<ProjectItem, string> <>f__am$cacheD;

		// Token: 0x04000CF3 RID: 3315
		[CompilerGenerated]
		private static Func<ProjectItem, bool> <>f__am$cacheE;

		// Token: 0x04000CF4 RID: 3316
		[CompilerGenerated]
		private static Func<ProjectItem, string> <>f__am$cacheF;

		// Token: 0x04000CF5 RID: 3317
		[CompilerGenerated]
		private static Func<ProjectItem, bool> <>f__am$cache10;

		// Token: 0x04000CF6 RID: 3318
		[CompilerGenerated]
		private static Func<ProjectItem, string> <>f__am$cache11;

		// Token: 0x04000CF7 RID: 3319
		[CompilerGenerated]
		private static Func<ProjectItem, string> <>f__am$cache12;

		// Token: 0x04000CF8 RID: 3320
		[CompilerGenerated]
		private static Func<ProjectItem, bool> <>f__am$cache13;

		// Token: 0x04000CF9 RID: 3321
		[CompilerGenerated]
		private static Func<ProjectItem, string> <>f__am$cache14;

		// Token: 0x04000CFA RID: 3322
		[CompilerGenerated]
		private static Func<ProjectItem, string> <>f__am$cache15;

		// Token: 0x04000CFB RID: 3323
		[CompilerGenerated]
		private static Func<ProjectItem, string> <>f__am$cache16;

		// Token: 0x02000ECA RID: 3786
		[CompilerGenerated]
		private sealed class <Parallel>c__AnonStorey0
		{
			// Token: 0x060071BE RID: 29118 RVA: 0x0004BF2C File Offset: 0x0004A32C
			public <Parallel>c__AnonStorey0()
			{
			}

			// Token: 0x060071BF RID: 29119 RVA: 0x0004BF34 File Offset: 0x0004A334
			internal void <>m__0(ProjectPayload actionCallback)
			{
				this.hasError |= actionCallback.HasError;
				this.counter--;
				if (this.counter == 0)
				{
					this.callback(new ProjectPayload
					{
						HasError = this.hasError
					});
				}
			}

			// Token: 0x0400659D RID: 26013
			internal bool hasError;

			// Token: 0x0400659E RID: 26014
			internal int counter;

			// Token: 0x0400659F RID: 26015
			internal ProjectEventHandler callback;
		}

		// Token: 0x02000ECB RID: 3787
		[CompilerGenerated]
		private sealed class <LoadProject>c__AnonStorey1
		{
			// Token: 0x060071C0 RID: 29120 RVA: 0x0004BF8B File Offset: 0x0004A38B
			public <LoadProject>c__AnonStorey1()
			{
			}

			// Token: 0x060071C1 RID: 29121 RVA: 0x0004BF94 File Offset: 0x0004A394
			internal void <>m__0(StoragePayload<string, bool> checkFolderResult)
			{
				bool data = checkFolderResult.Data;
				if (data)
				{
					Project.<LoadProject>c__AnonStorey1.<LoadProject>c__AnonStorey2 <LoadProject>c__AnonStorey = new Project.<LoadProject>c__AnonStorey1.<LoadProject>c__AnonStorey2();
					<LoadProject>c__AnonStorey.<>f__ref$1 = this;
					<LoadProject>c__AnonStorey.root = new ProjectRoot();
					this.$this.m_storage.LoadFile(this.name + ".rtpmeta", new StorageEventHandler<string, byte[]>(<LoadProject>c__AnonStorey.<>m__0));
				}
				else if (this.callback != null)
				{
					this.callback(new ProjectPayload<ProjectRoot>(null));
				}
			}

			// Token: 0x040065A0 RID: 26016
			internal string name;

			// Token: 0x040065A1 RID: 26017
			internal bool metaOnly;

			// Token: 0x040065A2 RID: 26018
			internal HashSet<int> exceptTypesHs;

			// Token: 0x040065A3 RID: 26019
			internal ProjectEventHandler<ProjectRoot> callback;

			// Token: 0x040065A4 RID: 26020
			internal Project $this;

			// Token: 0x02000EDA RID: 3802
			private sealed class <LoadProject>c__AnonStorey2
			{
				// Token: 0x060071F1 RID: 29169 RVA: 0x0004C013 File Offset: 0x0004A413
				public <LoadProject>c__AnonStorey2()
				{
				}

				// Token: 0x060071F2 RID: 29170 RVA: 0x0004C01C File Offset: 0x0004A41C
				internal void <>m__0(StoragePayload<string, byte[]> loadMetaCallback)
				{
					if (loadMetaCallback.Data != null)
					{
						this.root.Meta = this.<>f__ref$1.$this.m_serializer.Deserialize<ProjectMeta>(loadMetaCallback.Data);
					}
					else
					{
						this.root.Meta = new ProjectMeta();
					}
					this.<>f__ref$1.$this.m_storage.LoadFile(this.<>f__ref$1.name + ".rtpdata", new StorageEventHandler<string, byte[]>(this.<>m__1));
				}

				// Token: 0x060071F3 RID: 29171 RVA: 0x0004C0A8 File Offset: 0x0004A4A8
				internal void <>m__1(StoragePayload<string, byte[]> loadDataCallback)
				{
					if (loadDataCallback.Data != null)
					{
						this.root.Data = this.<>f__ref$1.$this.m_serializer.Deserialize<ProjectData>(loadDataCallback.Data);
					}
					else
					{
						this.root.Data = new ProjectData();
					}
					this.root.Item = ProjectItem.CreateFolder(this.<>f__ref$1.name);
					this.<>f__ref$1.$this.LoadFolders(this.root.Item, new ProjectEventHandler(this.<>m__2));
				}

				// Token: 0x060071F4 RID: 29172 RVA: 0x0004C13D File Offset: 0x0004A53D
				internal void <>m__2(ProjectPayload loadFoldersCallback)
				{
					this.<>f__ref$1.$this.LoadFiles(this.root.Item, new ProjectEventHandler(this.<>m__3), this.<>f__ref$1.metaOnly, this.<>f__ref$1.exceptTypesHs);
				}

				// Token: 0x060071F5 RID: 29173 RVA: 0x0004C17C File Offset: 0x0004A57C
				internal void <>m__3(ProjectPayload loadFilesCallback)
				{
					if (this.<>f__ref$1.callback != null)
					{
						this.<>f__ref$1.callback(new ProjectPayload<ProjectRoot>(this.root));
					}
				}

				// Token: 0x040065D5 RID: 26069
				internal ProjectRoot root;

				// Token: 0x040065D6 RID: 26070
				internal Project.<LoadProject>c__AnonStorey1 <>f__ref$1;
			}
		}

		// Token: 0x02000ECC RID: 3788
		[CompilerGenerated]
		private sealed class <LoadFiles>c__AnonStorey4
		{
			// Token: 0x060071C2 RID: 29122 RVA: 0x0004C1A9 File Offset: 0x0004A5A9
			public <LoadFiles>c__AnonStorey4()
			{
			}

			// Token: 0x060071C3 RID: 29123 RVA: 0x0004C1B4 File Offset: 0x0004A5B4
			internal void <>m__0(StoragePayload<string, string[]> getFilesCompleted)
			{
				string[] array = getFilesCompleted.Data;
				if (array == null && array.Length > 0)
				{
					this.$this.Parallel(this.loadFilesActions, new ProjectEventHandler(this.<>m__1));
				}
				else
				{
					Project.<LoadFiles>c__AnonStorey4.<LoadFiles>c__AnonStorey5 <LoadFiles>c__AnonStorey = new Project.<LoadFiles>c__AnonStorey4.<LoadFiles>c__AnonStorey5();
					<LoadFiles>c__AnonStorey.<>f__ref$4 = this;
					if (this.metaOnly)
					{
						IEnumerable<string> source = array;
						if (Project.<LoadFiles>c__AnonStorey4.<>f__am$cache0 == null)
						{
							Project.<LoadFiles>c__AnonStorey4.<>f__am$cache0 = new Func<string, bool>(Project.<LoadFiles>c__AnonStorey4.<>m__2);
						}
						array = source.Where(Project.<LoadFiles>c__AnonStorey4.<>f__am$cache0).ToArray<string>();
						<LoadFiles>c__AnonStorey.metaLength = array.Length;
					}
					else
					{
						IEnumerable<string> source2 = array;
						if (Project.<LoadFiles>c__AnonStorey4.<>f__am$cache1 == null)
						{
							Project.<LoadFiles>c__AnonStorey4.<>f__am$cache1 = new Func<string, bool>(Project.<LoadFiles>c__AnonStorey4.<>m__3);
						}
						array = source2.Where(Project.<LoadFiles>c__AnonStorey4.<>f__am$cache1).ToArray<string>();
						<LoadFiles>c__AnonStorey.metaLength = array.Length;
						Array.Resize<string>(ref array, <LoadFiles>c__AnonStorey.metaLength + <LoadFiles>c__AnonStorey.metaLength);
						for (int i = 0; i < <LoadFiles>c__AnonStorey.metaLength; i++)
						{
							array[<LoadFiles>c__AnonStorey.metaLength + i] = array[i].Remove(array[i].LastIndexOf(".rtmeta"));
						}
					}
					if (<LoadFiles>c__AnonStorey.metaLength > 0)
					{
						this.$this.m_storage.LoadFiles(array, new StorageEventHandler<string[], byte[][]>(<LoadFiles>c__AnonStorey.<>m__0));
					}
					else
					{
						this.$this.Parallel(this.loadFilesActions, new ProjectEventHandler(<LoadFiles>c__AnonStorey.<>m__1));
					}
				}
			}

			// Token: 0x060071C4 RID: 29124 RVA: 0x0004C30F File Offset: 0x0004A70F
			internal void <>m__1(ProjectPayload parallelCallback)
			{
				if (this.callback != null)
				{
					this.callback(new ProjectPayload());
				}
			}

			// Token: 0x060071C5 RID: 29125 RVA: 0x0004C32C File Offset: 0x0004A72C
			private static bool <>m__2(string n)
			{
				return n.EndsWith(".rtmeta");
			}

			// Token: 0x060071C6 RID: 29126 RVA: 0x0004C339 File Offset: 0x0004A739
			private static bool <>m__3(string n)
			{
				return n.EndsWith(".rtmeta");
			}

			// Token: 0x040065A5 RID: 26021
			internal bool metaOnly;

			// Token: 0x040065A6 RID: 26022
			internal HashSet<int> exceptTypesHS;

			// Token: 0x040065A7 RID: 26023
			internal Action<ProjectEventHandler>[] loadFilesActions;

			// Token: 0x040065A8 RID: 26024
			internal ProjectEventHandler callback;

			// Token: 0x040065A9 RID: 26025
			internal ProjectItem item;

			// Token: 0x040065AA RID: 26026
			internal Project $this;

			// Token: 0x040065AB RID: 26027
			private static Func<string, bool> <>f__am$cache0;

			// Token: 0x040065AC RID: 26028
			private static Func<string, bool> <>f__am$cache1;

			// Token: 0x02000EDB RID: 3803
			private sealed class <LoadFiles>c__AnonStorey5
			{
				// Token: 0x060071F6 RID: 29174 RVA: 0x0004C346 File Offset: 0x0004A746
				public <LoadFiles>c__AnonStorey5()
				{
				}

				// Token: 0x060071F7 RID: 29175 RVA: 0x0004C350 File Offset: 0x0004A750
				internal void <>m__0(StoragePayload<string[], byte[][]> loadFilesCompleted)
				{
					if (this.<>f__ref$4.item.Children == null)
					{
						this.<>f__ref$4.item.Children = new List<ProjectItem>();
					}
					for (int i = 0; i < this.metaLength; i++)
					{
						Project.<LoadFiles>c__AnonStorey4.<LoadFiles>c__AnonStorey5.<LoadFiles>c__AnonStorey6 <LoadFiles>c__AnonStorey = new Project.<LoadFiles>c__AnonStorey4.<LoadFiles>c__AnonStorey5.<LoadFiles>c__AnonStorey6();
						<LoadFiles>c__AnonStorey.<>f__ref$4 = this.<>f__ref$4;
						<LoadFiles>c__AnonStorey.<>f__ref$5 = this;
						<LoadFiles>c__AnonStorey.meta = null;
						ProjectItemData data = null;
						byte[] array = loadFilesCompleted.Data[i];
						if (array != null)
						{
							<LoadFiles>c__AnonStorey.meta = this.<>f__ref$4.$this.m_serializer.Deserialize<ProjectItemMeta>(array);
						}
						if (!this.<>f__ref$4.metaOnly)
						{
							bool flag = <LoadFiles>c__AnonStorey.meta != null && (this.<>f__ref$4.exceptTypesHS == null || !this.<>f__ref$4.exceptTypesHS.Contains(<LoadFiles>c__AnonStorey.meta.TypeCode));
							if (flag)
							{
								byte[] array2 = loadFilesCompleted.Data[this.metaLength + i];
								if (array2 != null)
								{
									data = this.<>f__ref$4.$this.m_serializer.Deserialize<ProjectItemData>(array2);
								}
							}
						}
						if (<LoadFiles>c__AnonStorey.meta.TypeCode == 1)
						{
							ProjectItem projectItem = this.<>f__ref$4.item.Children.Where(new Func<ProjectItem, bool>(<LoadFiles>c__AnonStorey.<>m__0)).FirstOrDefault<ProjectItem>();
							if (projectItem != null)
							{
								projectItem.Internal_Meta = <LoadFiles>c__AnonStorey.meta;
							}
						}
						else
						{
							ProjectItem item = new ProjectItem(<LoadFiles>c__AnonStorey.meta, data);
							this.<>f__ref$4.item.AddChild(item);
						}
					}
					this.<>f__ref$4.$this.Parallel(this.<>f__ref$4.loadFilesActions, new ProjectEventHandler(this.<>m__2));
				}

				// Token: 0x060071F8 RID: 29176 RVA: 0x0004C50C File Offset: 0x0004A90C
				internal void <>m__1(ProjectPayload parallelCallback)
				{
					if (this.<>f__ref$4.callback != null)
					{
						this.<>f__ref$4.callback(new ProjectPayload());
					}
				}

				// Token: 0x060071F9 RID: 29177 RVA: 0x0004C533 File Offset: 0x0004A933
				internal void <>m__2(ProjectPayload parallelCallback)
				{
					if (this.<>f__ref$4.callback != null)
					{
						this.<>f__ref$4.callback(new ProjectPayload());
					}
				}

				// Token: 0x040065D7 RID: 26071
				internal int metaLength;

				// Token: 0x040065D8 RID: 26072
				internal Project.<LoadFiles>c__AnonStorey4 <>f__ref$4;

				// Token: 0x02000EDC RID: 3804
				private sealed class <LoadFiles>c__AnonStorey6
				{
					// Token: 0x060071FA RID: 29178 RVA: 0x0004C55A File Offset: 0x0004A95A
					public <LoadFiles>c__AnonStorey6()
					{
					}

					// Token: 0x060071FB RID: 29179 RVA: 0x0004C562 File Offset: 0x0004A962
					internal bool <>m__0(ProjectItem c)
					{
						return c.IsFolder && c.NameExt == this.meta.Name;
					}

					// Token: 0x040065D9 RID: 26073
					internal ProjectItemMeta meta;

					// Token: 0x040065DA RID: 26074
					internal Project.<LoadFiles>c__AnonStorey4 <>f__ref$4;

					// Token: 0x040065DB RID: 26075
					internal Project.<LoadFiles>c__AnonStorey4.<LoadFiles>c__AnonStorey5 <>f__ref$5;
				}
			}
		}

		// Token: 0x02000ECD RID: 3789
		[CompilerGenerated]
		private sealed class <LoadFiles>c__AnonStorey3
		{
			// Token: 0x060071C7 RID: 29127 RVA: 0x0004C588 File Offset: 0x0004A988
			public <LoadFiles>c__AnonStorey3()
			{
			}

			// Token: 0x060071C8 RID: 29128 RVA: 0x0004C590 File Offset: 0x0004A990
			internal void <>m__0(ProjectEventHandler cb)
			{
				this.<>f__ref$4.$this.LoadFiles(this.childItem, cb, this.<>f__ref$4.metaOnly, this.<>f__ref$4.exceptTypesHS);
			}

			// Token: 0x040065AD RID: 26029
			internal ProjectItem childItem;

			// Token: 0x040065AE RID: 26030
			internal Project.<LoadFiles>c__AnonStorey4 <>f__ref$4;
		}

		// Token: 0x02000ECE RID: 3790
		[CompilerGenerated]
		private sealed class <LoadFolders>c__AnonStorey7
		{
			// Token: 0x060071C9 RID: 29129 RVA: 0x0004C5BF File Offset: 0x0004A9BF
			public <LoadFolders>c__AnonStorey7()
			{
			}

			// Token: 0x060071CA RID: 29130 RVA: 0x0004C5C8 File Offset: 0x0004A9C8
			internal void <>m__0(StoragePayload<string, string[]> getFoldersCompleted)
			{
				string[] data = getFoldersCompleted.Data;
				if (data != null && data.Length > 0)
				{
					Action<ProjectEventHandler>[] array = new Action<ProjectEventHandler>[data.Length];
					if (this.item.Children == null)
					{
						this.item.Children = new List<ProjectItem>(data.Length);
					}
					for (int i = 0; i < data.Length; i++)
					{
						Project.<LoadFolders>c__AnonStorey7.<LoadFolders>c__AnonStorey8 <LoadFolders>c__AnonStorey = new Project.<LoadFolders>c__AnonStorey7.<LoadFolders>c__AnonStorey8();
						<LoadFolders>c__AnonStorey.<>f__ref$7 = this;
						string name = data[i];
						<LoadFolders>c__AnonStorey.childItem = ProjectItem.CreateFolder(name);
						this.item.AddChild(<LoadFolders>c__AnonStorey.childItem);
						array[i] = new Action<ProjectEventHandler>(<LoadFolders>c__AnonStorey.<>m__0);
					}
					this.$this.Parallel(array, new ProjectEventHandler(this.<>m__1));
				}
				else if (this.callback != null)
				{
					this.callback(new ProjectPayload());
				}
			}

			// Token: 0x060071CB RID: 29131 RVA: 0x0004C6A2 File Offset: 0x0004AAA2
			internal void <>m__1(ProjectPayload parallelCallback)
			{
				if (this.callback != null)
				{
					this.callback(new ProjectPayload());
				}
			}

			// Token: 0x040065AF RID: 26031
			internal ProjectItem item;

			// Token: 0x040065B0 RID: 26032
			internal ProjectEventHandler callback;

			// Token: 0x040065B1 RID: 26033
			internal Project $this;

			// Token: 0x02000EDD RID: 3805
			private sealed class <LoadFolders>c__AnonStorey8
			{
				// Token: 0x060071FC RID: 29180 RVA: 0x0004C6BF File Offset: 0x0004AABF
				public <LoadFolders>c__AnonStorey8()
				{
				}

				// Token: 0x060071FD RID: 29181 RVA: 0x0004C6C7 File Offset: 0x0004AAC7
				internal void <>m__0(ProjectEventHandler cb)
				{
					this.<>f__ref$7.$this.LoadFolders(this.childItem, cb);
				}

				// Token: 0x040065DC RID: 26076
				internal ProjectItem childItem;

				// Token: 0x040065DD RID: 26077
				internal Project.<LoadFolders>c__AnonStorey7 <>f__ref$7;
			}
		}

		// Token: 0x02000ECF RID: 3791
		[CompilerGenerated]
		private sealed class <SaveProjectMeta>c__AnonStorey9
		{
			// Token: 0x060071CC RID: 29132 RVA: 0x0004C6E0 File Offset: 0x0004AAE0
			public <SaveProjectMeta>c__AnonStorey9()
			{
			}

			// Token: 0x060071CD RID: 29133 RVA: 0x0004C6E8 File Offset: 0x0004AAE8
			internal void <>m__0(StoragePayload<string> saveMetaCompleted)
			{
				if (this.callback != null)
				{
					this.callback(new ProjectPayload());
				}
			}

			// Token: 0x040065B2 RID: 26034
			internal ProjectEventHandler callback;
		}

		// Token: 0x02000ED0 RID: 3792
		[CompilerGenerated]
		private sealed class <Load>c__AnonStoreyA
		{
			// Token: 0x060071CE RID: 29134 RVA: 0x0004C705 File Offset: 0x0004AB05
			public <Load>c__AnonStoreyA()
			{
			}

			// Token: 0x060071CF RID: 29135 RVA: 0x0004C710 File Offset: 0x0004AB10
			internal void <>m__0(StoragePayload<string[], byte[][]> loadFilesResult)
			{
				List<ProjectItem> list = new List<ProjectItem>();
				for (int i = 0; i < this.pathLength; i++)
				{
					byte[] array = loadFilesResult.Data[i];
					byte[] array2 = loadFilesResult.Data[this.pathLength + i];
					if (array != null && array2 != null)
					{
						ProjectItemMeta projectItemMeta = this.$this.m_serializer.Deserialize<ProjectItemMeta>(array2);
						bool flag = this.exceptTypesHs == null || !this.exceptTypesHs.Contains(projectItemMeta.TypeCode);
						ProjectItemData data = (!flag) ? null : this.$this.m_serializer.Deserialize<ProjectItemData>(array);
						list.Add(new ProjectItem(projectItemMeta, data));
					}
				}
				this.callback(new ProjectPayload<ProjectItem[]>(list.ToArray()));
			}

			// Token: 0x040065B3 RID: 26035
			internal int pathLength;

			// Token: 0x040065B4 RID: 26036
			internal HashSet<int> exceptTypesHs;

			// Token: 0x040065B5 RID: 26037
			internal ProjectEventHandler<ProjectItem[]> callback;

			// Token: 0x040065B6 RID: 26038
			internal Project $this;
		}

		// Token: 0x02000ED1 RID: 3793
		[CompilerGenerated]
		private sealed class <LoadData>c__AnonStoreyB
		{
			// Token: 0x060071D0 RID: 29136 RVA: 0x0004C7DC File Offset: 0x0004ABDC
			public <LoadData>c__AnonStoreyB()
			{
			}

			// Token: 0x060071D1 RID: 29137 RVA: 0x0004C7E4 File Offset: 0x0004ABE4
			internal bool <>m__0(ProjectItem item)
			{
				return !this.exceptTypes.Contains(item.TypeCode);
			}

			// Token: 0x060071D2 RID: 29138 RVA: 0x0004C7FC File Offset: 0x0004ABFC
			internal void <>m__1(StoragePayload<string[], byte[][]> loadFilesResult)
			{
				for (int i = 0; i < this.path.Length; i++)
				{
					byte[] array = loadFilesResult.Data[i];
					if (array != null)
					{
						ProjectItemData internal_Data = this.$this.m_serializer.Deserialize<ProjectItemData>(array);
						this.items[i].Internal_Data = internal_Data;
					}
				}
				this.callback(new ProjectPayload<ProjectItem[]>(this.items));
			}

			// Token: 0x040065B7 RID: 26039
			internal int[] exceptTypes;

			// Token: 0x040065B8 RID: 26040
			internal string[] path;

			// Token: 0x040065B9 RID: 26041
			internal ProjectItem[] items;

			// Token: 0x040065BA RID: 26042
			internal ProjectEventHandler<ProjectItem[]> callback;

			// Token: 0x040065BB RID: 26043
			internal Project $this;
		}

		// Token: 0x02000ED2 RID: 3794
		[CompilerGenerated]
		private sealed class <Save>c__AnonStoreyC
		{
			// Token: 0x060071D3 RID: 29139 RVA: 0x0004C867 File Offset: 0x0004AC67
			public <Save>c__AnonStoreyC()
			{
			}

			// Token: 0x060071D4 RID: 29140 RVA: 0x0004C870 File Offset: 0x0004AC70
			internal void <>m__0(StoragePayload<string> saveMetaCompleted)
			{
				if (this.item.Internal_Data != null)
				{
					if (this.metaOnly)
					{
						if (this.callback != null)
						{
							this.callback(new ProjectPayload());
						}
					}
					else
					{
						this.$this.m_storage.SaveFile(this.path, this.$this.m_serializer.Serialize<ProjectItemData>(this.item.Internal_Data), new StorageEventHandler<string>(this.<>m__1));
					}
				}
				else if (this.callback != null)
				{
					this.callback(new ProjectPayload());
				}
			}

			// Token: 0x060071D5 RID: 29141 RVA: 0x0004C915 File Offset: 0x0004AD15
			internal void <>m__1(StoragePayload<string> saveDataCompleted)
			{
				if (this.callback != null)
				{
					this.callback(new ProjectPayload());
				}
			}

			// Token: 0x040065BC RID: 26044
			internal ProjectItem item;

			// Token: 0x040065BD RID: 26045
			internal bool metaOnly;

			// Token: 0x040065BE RID: 26046
			internal ProjectEventHandler callback;

			// Token: 0x040065BF RID: 26047
			internal string path;

			// Token: 0x040065C0 RID: 26048
			internal Project $this;
		}

		// Token: 0x02000ED3 RID: 3795
		[CompilerGenerated]
		private sealed class <Save>c__AnonStoreyD
		{
			// Token: 0x060071D6 RID: 29142 RVA: 0x0004C932 File Offset: 0x0004AD32
			public <Save>c__AnonStoreyD()
			{
			}

			// Token: 0x060071D7 RID: 29143 RVA: 0x0004C93A File Offset: 0x0004AD3A
			internal byte[] <>m__0(ProjectItem item)
			{
				return this.$this.m_serializer.Serialize<ProjectItemMeta>(item.Internal_Meta);
			}

			// Token: 0x060071D8 RID: 29144 RVA: 0x0004C952 File Offset: 0x0004AD52
			internal byte[] <>m__1(ProjectItem item)
			{
				return this.$this.m_serializer.Serialize<ProjectItemData>(item.Internal_Data);
			}

			// Token: 0x060071D9 RID: 29145 RVA: 0x0004C96A File Offset: 0x0004AD6A
			internal byte[] <>m__2(ProjectItem item)
			{
				return this.$this.m_serializer.Serialize<ProjectItemMeta>(item.Internal_Meta);
			}

			// Token: 0x060071DA RID: 29146 RVA: 0x0004C982 File Offset: 0x0004AD82
			internal void <>m__3(StoragePayload<string[]> foldersCreatedCallback)
			{
				this.$this.m_storage.SaveFiles(this.filePath, this.fileData, new StorageEventHandler<string[]>(this.<>m__4));
			}

			// Token: 0x060071DB RID: 29147 RVA: 0x0004C9AC File Offset: 0x0004ADAC
			internal void <>m__4(StoragePayload<string[]> saveFilesCallback)
			{
				if (this.callback != null)
				{
					this.callback(new ProjectPayload());
				}
			}

			// Token: 0x040065C1 RID: 26049
			internal string[] filePath;

			// Token: 0x040065C2 RID: 26050
			internal byte[][] fileData;

			// Token: 0x040065C3 RID: 26051
			internal ProjectEventHandler callback;

			// Token: 0x040065C4 RID: 26052
			internal Project $this;
		}

		// Token: 0x02000ED4 RID: 3796
		[CompilerGenerated]
		private sealed class <Delete>c__AnonStoreyE
		{
			// Token: 0x060071DC RID: 29148 RVA: 0x0004C9C9 File Offset: 0x0004ADC9
			public <Delete>c__AnonStoreyE()
			{
			}

			// Token: 0x060071DD RID: 29149 RVA: 0x0004C9D1 File Offset: 0x0004ADD1
			internal void <>m__0(StoragePayload<string> payload)
			{
				if (this.callback != null)
				{
					this.callback(new ProjectPayload());
				}
			}

			// Token: 0x060071DE RID: 29150 RVA: 0x0004C9EE File Offset: 0x0004ADEE
			internal void <>m__1(StoragePayload<string[]> payload)
			{
				if (this.callback != null)
				{
					this.callback(new ProjectPayload());
				}
			}

			// Token: 0x040065C5 RID: 26053
			internal ProjectEventHandler callback;
		}

		// Token: 0x02000ED5 RID: 3797
		[CompilerGenerated]
		private sealed class <Rename>c__AnonStoreyF
		{
			// Token: 0x060071DF RID: 29151 RVA: 0x0004CA0B File Offset: 0x0004AE0B
			public <Rename>c__AnonStoreyF()
			{
			}

			// Token: 0x060071E0 RID: 29152 RVA: 0x0004CA14 File Offset: 0x0004AE14
			internal void <>m__0(StoragePayload<string, byte[]> loadFilesResult)
			{
				byte[] data = loadFilesResult.Data;
				if (data != null)
				{
					ProjectItemData internal_Data = this.$this.m_serializer.Deserialize<ProjectItemData>(data);
					this.item.Internal_Data = internal_Data;
					this.item.Rename(this.name);
				}
				this.$this.Save(this.item, this.srcPath, false, new ProjectEventHandler(this.<>m__2));
			}

			// Token: 0x060071E1 RID: 29153 RVA: 0x0004CA81 File Offset: 0x0004AE81
			internal void <>m__1(ProjectPayload saveCompleted)
			{
				this.$this.Move(this.item, this.callback, this.srcPath, this.dstPath);
			}

			// Token: 0x060071E2 RID: 29154 RVA: 0x0004CAA6 File Offset: 0x0004AEA6
			internal void <>m__2(ProjectPayload saveCompleted)
			{
				this.$this.UnloadData(this.item);
				this.$this.Move(this.item, this.callback, this.srcPath, this.dstPath);
			}

			// Token: 0x040065C6 RID: 26054
			internal ProjectItem item;

			// Token: 0x040065C7 RID: 26055
			internal string name;

			// Token: 0x040065C8 RID: 26056
			internal string srcPath;

			// Token: 0x040065C9 RID: 26057
			internal ProjectEventHandler callback;

			// Token: 0x040065CA RID: 26058
			internal string dstPath;

			// Token: 0x040065CB RID: 26059
			internal Project $this;
		}

		// Token: 0x02000ED6 RID: 3798
		[CompilerGenerated]
		private sealed class <Move>c__AnonStorey10
		{
			// Token: 0x060071E3 RID: 29155 RVA: 0x0004CADC File Offset: 0x0004AEDC
			public <Move>c__AnonStorey10()
			{
			}

			// Token: 0x060071E4 RID: 29156 RVA: 0x0004CAE4 File Offset: 0x0004AEE4
			internal void <>m__0(StoragePayload<string, string> payload)
			{
				if (this.callback != null)
				{
					this.callback(new ProjectPayload());
				}
			}

			// Token: 0x060071E5 RID: 29157 RVA: 0x0004CB01 File Offset: 0x0004AF01
			internal void <>m__1(StoragePayload<string[], string[]> payload)
			{
				if (this.callback != null)
				{
					this.callback(new ProjectPayload());
				}
			}

			// Token: 0x040065CC RID: 26060
			internal ProjectEventHandler callback;
		}

		// Token: 0x02000ED7 RID: 3799
		[CompilerGenerated]
		private sealed class <GroupOperation>c__AnonStorey11
		{
			// Token: 0x060071E6 RID: 29158 RVA: 0x0004CB1E File Offset: 0x0004AF1E
			public <GroupOperation>c__AnonStorey11()
			{
			}

			// Token: 0x060071E7 RID: 29159 RVA: 0x0004CB28 File Offset: 0x0004AF28
			internal void <>m__0(StoragePayload<string[], string[]> folderOperationCompleted)
			{
				if (this.filePath.Length > 0)
				{
					this.fileOperation(this.filePath, this.fileDstPath, new StorageEventHandler<string[], string[]>(this.<>m__2));
				}
				else if (this.callback != null)
				{
					this.callback(new ProjectPayload());
				}
			}

			// Token: 0x060071E8 RID: 29160 RVA: 0x0004CB86 File Offset: 0x0004AF86
			internal void <>m__1(StoragePayload<string[], string[]> fileOperationCompleted)
			{
				if (this.callback != null)
				{
					this.callback(new ProjectPayload());
				}
			}

			// Token: 0x060071E9 RID: 29161 RVA: 0x0004CBA3 File Offset: 0x0004AFA3
			internal void <>m__2(StoragePayload<string[], string[]> fileOperationCompleted)
			{
				if (this.callback != null)
				{
					this.callback(new ProjectPayload());
				}
			}

			// Token: 0x040065CD RID: 26061
			internal string[] filePath;

			// Token: 0x040065CE RID: 26062
			internal Action<string[], string[], StorageEventHandler<string[], string[]>> fileOperation;

			// Token: 0x040065CF RID: 26063
			internal string[] fileDstPath;

			// Token: 0x040065D0 RID: 26064
			internal ProjectEventHandler callback;
		}

		// Token: 0x02000ED8 RID: 3800
		[CompilerGenerated]
		private sealed class <GroupOperation>c__AnonStorey12
		{
			// Token: 0x060071EA RID: 29162 RVA: 0x0004CBC0 File Offset: 0x0004AFC0
			public <GroupOperation>c__AnonStorey12()
			{
			}

			// Token: 0x060071EB RID: 29163 RVA: 0x0004CBC8 File Offset: 0x0004AFC8
			internal void <>m__0(StoragePayload<string[]> folderOperationCompleted)
			{
				if (this.filePath.Length > 0)
				{
					this.fileOperation(this.filePath, new StorageEventHandler<string[]>(this.<>m__2));
				}
				else if (this.callback != null)
				{
					this.callback(new ProjectPayload());
				}
			}

			// Token: 0x060071EC RID: 29164 RVA: 0x0004CC20 File Offset: 0x0004B020
			internal void <>m__1(StoragePayload<string[]> fileOperationCompleted)
			{
				if (this.callback != null)
				{
					this.callback(new ProjectPayload());
				}
			}

			// Token: 0x060071ED RID: 29165 RVA: 0x0004CC3D File Offset: 0x0004B03D
			internal void <>m__2(StoragePayload<string[]> fileOperationCompleted)
			{
				if (this.callback != null)
				{
					this.callback(new ProjectPayload());
				}
			}

			// Token: 0x040065D1 RID: 26065
			internal string[] filePath;

			// Token: 0x040065D2 RID: 26066
			internal Action<string[], StorageEventHandler<string[]>> fileOperation;

			// Token: 0x040065D3 RID: 26067
			internal ProjectEventHandler callback;
		}

		// Token: 0x02000ED9 RID: 3801
		[CompilerGenerated]
		private sealed class <Exists>c__AnonStorey13
		{
			// Token: 0x060071EE RID: 29166 RVA: 0x0004CC5A File Offset: 0x0004B05A
			public <Exists>c__AnonStorey13()
			{
			}

			// Token: 0x060071EF RID: 29167 RVA: 0x0004CC62 File Offset: 0x0004B062
			internal void <>m__0(StoragePayload<string, bool> result)
			{
				if (this.callback != null)
				{
					this.callback(new ProjectPayload<bool>(result.Data));
				}
			}

			// Token: 0x060071F0 RID: 29168 RVA: 0x0004CC85 File Offset: 0x0004B085
			internal void <>m__1(StoragePayload<string, bool> result)
			{
				if (this.callback != null)
				{
					this.callback(new ProjectPayload<bool>(result.Data));
				}
			}

			// Token: 0x040065D4 RID: 26068
			internal ProjectEventHandler<bool> callback;
		}
	}
}
