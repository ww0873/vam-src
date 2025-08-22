using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using ProtoBuf;
using UnityEngine;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x0200025A RID: 602
	[ProtoContract(AsReferenceDefault = true, ImplicitFields = ImplicitFields.AllFields)]
	[Serializable]
	public class ProjectItem
	{
		// Token: 0x06000C90 RID: 3216 RVA: 0x0004CE06 File Offset: 0x0004B206
		public ProjectItem()
		{
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x0004CE0E File Offset: 0x0004B20E
		public ProjectItem(ProjectItemMeta meta, ProjectItemData data)
		{
			this.Internal_Meta = meta;
			this.Internal_Data = data;
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000C92 RID: 3218 RVA: 0x0004CE24 File Offset: 0x0004B224
		// (set) Token: 0x06000C93 RID: 3219 RVA: 0x0004CE31 File Offset: 0x0004B231
		public bool IsExposedFromEditor
		{
			get
			{
				return this.Internal_Meta.IsExposedFromEditor;
			}
			set
			{
				this.Internal_Meta.IsExposedFromEditor = value;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000C94 RID: 3220 RVA: 0x0004CE3F File Offset: 0x0004B23F
		public string BundleName
		{
			get
			{
				if (this.Internal_Meta == null)
				{
					return null;
				}
				return this.Internal_Meta.BundleName;
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000C95 RID: 3221 RVA: 0x0004CE59 File Offset: 0x0004B259
		// (set) Token: 0x06000C96 RID: 3222 RVA: 0x0004CE66 File Offset: 0x0004B266
		public string Name
		{
			get
			{
				return this.Internal_Meta.Name;
			}
			set
			{
				this.Internal_Meta.Name = value;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000C97 RID: 3223 RVA: 0x0004CE74 File Offset: 0x0004B274
		// (set) Token: 0x06000C98 RID: 3224 RVA: 0x0004CEBC File Offset: 0x0004B2BC
		public string NameExt
		{
			get
			{
				string ext = this.Ext;
				if (string.IsNullOrEmpty(ext))
				{
					return this.Internal_Meta.Name;
				}
				return string.Format("{0}.{1}", this.Internal_Meta.Name, this.Ext);
			}
			set
			{
				if (value == null)
				{
					this.Internal_Meta.Name = null;
				}
				else
				{
					int num = value.LastIndexOf("." + this.Ext);
					if (num >= 0)
					{
						this.Internal_Meta.Name = value.Remove(num);
					}
					else
					{
						this.Internal_Meta.Name = value;
					}
				}
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000C99 RID: 3225 RVA: 0x0004CF21 File Offset: 0x0004B321
		public int TypeCode
		{
			get
			{
				if (this.Internal_Meta == null)
				{
					return 0;
				}
				return this.Internal_Meta.TypeCode;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000C9A RID: 3226 RVA: 0x0004CF3B File Offset: 0x0004B33B
		public string TypeName
		{
			get
			{
				if (this.Internal_Meta == null)
				{
					return null;
				}
				return this.Internal_Meta.TypeName;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000C9B RID: 3227 RVA: 0x0004CF55 File Offset: 0x0004B355
		public bool IsFolder
		{
			get
			{
				return this.Internal_Meta.TypeCode == 1;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000C9C RID: 3228 RVA: 0x0004CF65 File Offset: 0x0004B365
		public bool IsScene
		{
			get
			{
				return this.Internal_Meta.TypeCode == 2;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000C9D RID: 3229 RVA: 0x0004CF75 File Offset: 0x0004B375
		public bool IsResource
		{
			get
			{
				return !this.IsFolder && !this.IsScene;
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000C9E RID: 3230 RVA: 0x0004CF90 File Offset: 0x0004B390
		public bool IsGameObject
		{
			get
			{
				Type type = Type.GetType(this.TypeName);
				return type == typeof(GameObject);
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000C9F RID: 3231 RVA: 0x0004CFB6 File Offset: 0x0004B3B6
		public string Ext
		{
			get
			{
				return ProjectItemTypes.Ext[this.Internal_Meta.TypeCode];
			}
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x0004CFD0 File Offset: 0x0004B3D0
		public void AddChild(ProjectItem item)
		{
			if (this.Children == null)
			{
				this.Children = new List<ProjectItem>();
			}
			if (item.Parent != null)
			{
				item.Parent.RemoveChild(item);
			}
			this.Children.Add(item);
			item.Parent = this;
		}

		// Token: 0x06000CA1 RID: 3233 RVA: 0x0004D01D File Offset: 0x0004B41D
		public void RemoveChild(ProjectItem item)
		{
			if (this.Children == null)
			{
				return;
			}
			this.Children.Remove(item);
			item.Parent = null;
		}

		// Token: 0x06000CA2 RID: 3234 RVA: 0x0004D03F File Offset: 0x0004B43F
		public int GetSiblingIndex()
		{
			return this.Parent.Children.IndexOf(this);
		}

		// Token: 0x06000CA3 RID: 3235 RVA: 0x0004D052 File Offset: 0x0004B452
		public void SetSiblingIndex(int index)
		{
			this.Parent.Children.Remove(this);
			this.Parent.Children.Insert(index, this);
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x0004D078 File Offset: 0x0004B478
		public ProjectItem Get(string path)
		{
			path = path.Trim(new char[]
			{
				'/'
			});
			string[] array = path.Split(new char[]
			{
				'/'
			});
			ProjectItem projectItem = this;
			for (int i = 1; i < array.Length; i++)
			{
				ProjectItem.<Get>c__AnonStorey0 <Get>c__AnonStorey = new ProjectItem.<Get>c__AnonStorey0();
				<Get>c__AnonStorey.pathPart = array[i];
				if (projectItem.Children == null)
				{
					return projectItem;
				}
				if (i == array.Length - 1)
				{
					projectItem = projectItem.Children.Where(new Func<ProjectItem, bool>(<Get>c__AnonStorey.<>m__0)).FirstOrDefault<ProjectItem>();
				}
				else
				{
					projectItem = projectItem.Children.Where(new Func<ProjectItem, bool>(<Get>c__AnonStorey.<>m__1)).FirstOrDefault<ProjectItem>();
				}
				if (projectItem == null)
				{
					break;
				}
			}
			return projectItem;
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x0004D134 File Offset: 0x0004B534
		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (ProjectItem projectItem = this; projectItem != null; projectItem = projectItem.Parent)
			{
				stringBuilder.Insert(0, projectItem.Internal_Meta.Name);
				stringBuilder.Insert(0, "/");
			}
			string ext = this.Ext;
			if (string.IsNullOrEmpty(ext))
			{
				return stringBuilder.ToString();
			}
			return string.Format("{0}.{1}", stringBuilder.ToString(), this.Ext);
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x0004D1AC File Offset: 0x0004B5AC
		public static ProjectItem CreateFolder(string name)
		{
			return new ProjectItem
			{
				Internal_Meta = new ProjectItemMeta
				{
					Name = name,
					TypeCode = 1
				}
			};
		}

		// Token: 0x06000CA7 RID: 3239 RVA: 0x0004D1DC File Offset: 0x0004B5DC
		public static ProjectItem CreateScene(string name)
		{
			return new ProjectItem
			{
				Internal_Meta = new ProjectItemMeta
				{
					Name = name,
					TypeCode = 2
				}
			};
		}

		// Token: 0x06000CA8 RID: 3240 RVA: 0x0004D20C File Offset: 0x0004B60C
		public static string GetUniqueName(string desiredName, UnityEngine.Object obj, ProjectItem parent)
		{
			string text = ProjectItemTypes.Ext[ProjectItemTypes.GetProjectItemType(obj.GetType())];
			IEnumerable<ProjectItem> children = parent.Children;
			if (ProjectItem.<>f__am$cache0 == null)
			{
				ProjectItem.<>f__am$cache0 = new Func<ProjectItem, string>(ProjectItem.<GetUniqueName>m__0);
			}
			string[] existingNames = children.Select(ProjectItem.<>f__am$cache0).ToArray<string>();
			string uniqueName = PathHelper.GetUniqueName(desiredName, text, existingNames);
			if (uniqueName == null)
			{
				return null;
			}
			int num = uniqueName.LastIndexOf("." + text);
			if (num >= 0)
			{
				return uniqueName.Remove(num);
			}
			return uniqueName;
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x0004D290 File Offset: 0x0004B690
		public static string GetUniqueName(string desiredName, ProjectItem item, ProjectItem parent, bool exceptItem = true)
		{
			string[] existingNames;
			if (exceptItem)
			{
				IEnumerable<ProjectItem> source = parent.Children.Except(new ProjectItem[]
				{
					item
				});
				if (ProjectItem.<>f__am$cache1 == null)
				{
					ProjectItem.<>f__am$cache1 = new Func<ProjectItem, string>(ProjectItem.<GetUniqueName>m__1);
				}
				existingNames = source.Select(ProjectItem.<>f__am$cache1).ToArray<string>();
			}
			else
			{
				IEnumerable<ProjectItem> children = parent.Children;
				if (ProjectItem.<>f__am$cache2 == null)
				{
					ProjectItem.<>f__am$cache2 = new Func<ProjectItem, string>(ProjectItem.<GetUniqueName>m__2);
				}
				existingNames = children.Select(ProjectItem.<>f__am$cache2).ToArray<string>();
			}
			return PathHelper.GetUniqueName(desiredName, item.Ext, existingNames);
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x0004D320 File Offset: 0x0004B720
		public static bool IsValidName(string name)
		{
			ProjectItem.<IsValidName>c__AnonStorey1 <IsValidName>c__AnonStorey = new ProjectItem.<IsValidName>c__AnonStorey1();
			<IsValidName>c__AnonStorey.name = name;
			return string.IsNullOrEmpty(<IsValidName>c__AnonStorey.name) || Path.GetInvalidFileNameChars().All(new Func<char, bool>(<IsValidName>c__AnonStorey.<>m__0));
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x0004D364 File Offset: 0x0004B764
		public static ProjectItem[] GetRootItems(ProjectItem[] items)
		{
			HashSet<ProjectItem> hashSet = new HashSet<ProjectItem>();
			for (int i = 0; i < items.Length; i++)
			{
				if (!hashSet.Contains(items[i]))
				{
					hashSet.Add(items[i]);
				}
			}
			foreach (ProjectItem projectItem in items)
			{
				for (ProjectItem parent = projectItem.Parent; parent != null; parent = parent.Parent)
				{
					if (hashSet.Contains(parent))
					{
						hashSet.Remove(projectItem);
						break;
					}
				}
			}
			items = hashSet.ToArray<ProjectItem>();
			return items;
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x0004D3FC File Offset: 0x0004B7FC
		public ProjectItem[] FlattenHierarchy(bool includeSelf = false)
		{
			List<ProjectItem> list = new List<ProjectItem>();
			if (includeSelf)
			{
				list.Add(this);
			}
			this.GetAncestors(this, list);
			return list.ToArray();
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x0004D42C File Offset: 0x0004B82C
		private void GetAncestors(ProjectItem item, List<ProjectItem> list)
		{
			if (item.Children == null)
			{
				return;
			}
			for (int i = 0; i < item.Children.Count; i++)
			{
				ProjectItem item2 = item.Children[i];
				list.Add(item2);
				this.GetAncestors(item2, list);
			}
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x0004D47D File Offset: 0x0004B87D
		public void Rename(string name)
		{
			this.Internal_Data.Rename(this.Internal_Meta, name);
		}

		// Token: 0x06000CAF RID: 3247 RVA: 0x0004D491 File Offset: 0x0004B891
		[CompilerGenerated]
		private static string <GetUniqueName>m__0(ProjectItem child)
		{
			return child.NameExt;
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x0004D499 File Offset: 0x0004B899
		[CompilerGenerated]
		private static string <GetUniqueName>m__1(ProjectItem child)
		{
			return child.NameExt;
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x0004D4A1 File Offset: 0x0004B8A1
		[CompilerGenerated]
		private static string <GetUniqueName>m__2(ProjectItem child)
		{
			return child.NameExt;
		}

		// Token: 0x04000D0E RID: 3342
		public ProjectItemMeta Internal_Meta;

		// Token: 0x04000D0F RID: 3343
		public ProjectItemData Internal_Data;

		// Token: 0x04000D10 RID: 3344
		public ProjectItem Parent;

		// Token: 0x04000D11 RID: 3345
		public List<ProjectItem> Children;

		// Token: 0x04000D12 RID: 3346
		[CompilerGenerated]
		private static Func<ProjectItem, string> <>f__am$cache0;

		// Token: 0x04000D13 RID: 3347
		[CompilerGenerated]
		private static Func<ProjectItem, string> <>f__am$cache1;

		// Token: 0x04000D14 RID: 3348
		[CompilerGenerated]
		private static Func<ProjectItem, string> <>f__am$cache2;

		// Token: 0x02000EDE RID: 3806
		[CompilerGenerated]
		private sealed class <Get>c__AnonStorey0
		{
			// Token: 0x060071FE RID: 29182 RVA: 0x0004D4A9 File Offset: 0x0004B8A9
			public <Get>c__AnonStorey0()
			{
			}

			// Token: 0x060071FF RID: 29183 RVA: 0x0004D4B1 File Offset: 0x0004B8B1
			internal bool <>m__0(ProjectItem child)
			{
				return child.NameExt == this.pathPart;
			}

			// Token: 0x06007200 RID: 29184 RVA: 0x0004D4C4 File Offset: 0x0004B8C4
			internal bool <>m__1(ProjectItem child)
			{
				return child.Name == this.pathPart;
			}

			// Token: 0x040065DE RID: 26078
			internal string pathPart;
		}

		// Token: 0x02000EDF RID: 3807
		[CompilerGenerated]
		private sealed class <IsValidName>c__AnonStorey1
		{
			// Token: 0x06007201 RID: 29185 RVA: 0x0004D4D7 File Offset: 0x0004B8D7
			public <IsValidName>c__AnonStorey1()
			{
			}

			// Token: 0x06007202 RID: 29186 RVA: 0x0004D4DF File Offset: 0x0004B8DF
			internal bool <>m__0(char c)
			{
				return !this.name.Contains(c);
			}

			// Token: 0x040065DF RID: 26079
			internal string name;
		}
	}
}
