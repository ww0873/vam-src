using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace MVR.FileManagement
{
	// Token: 0x02000BED RID: 3053
	public class SystemDirectoryEntry : DirectoryEntry
	{
		// Token: 0x060057C6 RID: 22470 RVA: 0x002041B4 File Offset: 0x002025B4
		public SystemDirectoryEntry(string path) : base(path)
		{
			if (this.Path.EndsWith(":"))
			{
				this.FixedPath = this.Path + "\\";
			}
			else
			{
				this.FixedPath = this.Path;
				this.hidePath = this.Path + ".hide";
			}
			if (!Directory.Exists(this.FixedPath))
			{
				throw new Exception("Directory " + this.Path + " does not exist");
			}
			this.FullPath = System.IO.Path.GetFullPath(this.FixedPath);
			this.FullSlashPath = this.FullPath.Replace('\\', '/');
			this.LastWriteTime = Directory.GetLastWriteTime(this.FixedPath);
		}

		// Token: 0x17000CC1 RID: 3265
		// (get) Token: 0x060057C7 RID: 22471 RVA: 0x0020427C File Offset: 0x0020267C
		// (set) Token: 0x060057C8 RID: 22472 RVA: 0x00204284 File Offset: 0x00202684
		public string FixedPath
		{
			[CompilerGenerated]
			get
			{
				return this.<FixedPath>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<FixedPath>k__BackingField = value;
			}
		}

		// Token: 0x17000CC2 RID: 3266
		// (get) Token: 0x060057C9 RID: 22473 RVA: 0x00204290 File Offset: 0x00202690
		// (set) Token: 0x060057CA RID: 22474 RVA: 0x002042B6 File Offset: 0x002026B6
		public override DirectoryEntry Parent
		{
			get
			{
				DirectoryInfo parent = Directory.GetParent(this.FixedPath);
				return new SystemDirectoryEntry(parent.FullName);
			}
			protected set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000CC3 RID: 3267
		// (get) Token: 0x060057CB RID: 22475 RVA: 0x002042C0 File Offset: 0x002026C0
		// (set) Token: 0x060057CC RID: 22476 RVA: 0x0020435C File Offset: 0x0020275C
		public override List<FileEntry> Files
		{
			get
			{
				List<FileEntry> list = new List<FileEntry>();
				string[] directories = Directory.GetDirectories(this.FixedPath);
				foreach (string text in directories)
				{
					if (FileManager.IsPackage(text))
					{
						SystemFileEntry item = new SystemFileEntry(text);
						list.Add(item);
					}
				}
				string[] files = Directory.GetFiles(this.FixedPath);
				foreach (string path in files)
				{
					SystemFileEntry item2 = new SystemFileEntry(path);
					list.Add(item2);
				}
				return list;
			}
			protected set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000CC4 RID: 3268
		// (get) Token: 0x060057CD RID: 22477 RVA: 0x00204364 File Offset: 0x00202764
		// (set) Token: 0x060057CE RID: 22478 RVA: 0x0020441D File Offset: 0x0020281D
		public override List<DirectoryEntry> SubDirectories
		{
			get
			{
				List<DirectoryEntry> list = new List<DirectoryEntry>();
				string[] directories = Directory.GetDirectories(this.FixedPath);
				foreach (string text in directories)
				{
					VarPackage package = FileManager.GetPackage(text);
					if (package != null)
					{
						list.Add(package.RootDirectory);
					}
					else
					{
						SystemDirectoryEntry item = new SystemDirectoryEntry(text);
						list.Add(item);
					}
				}
				string[] files = Directory.GetFiles(this.FixedPath);
				foreach (string path in files)
				{
					VarDirectoryEntry varRootDirectoryEntryFromPath = FileManager.GetVarRootDirectoryEntryFromPath(path);
					if (varRootDirectoryEntryFromPath != null)
					{
						list.Add(varRootDirectoryEntryFromPath);
					}
				}
				return list;
			}
			protected set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x04004890 RID: 18576
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <FixedPath>k__BackingField;
	}
}
