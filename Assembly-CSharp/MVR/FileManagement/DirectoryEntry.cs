using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace MVR.FileManagement
{
	// Token: 0x02000BD8 RID: 3032
	public abstract class DirectoryEntry
	{
		// Token: 0x0600560F RID: 22031 RVA: 0x001F75C4 File Offset: 0x001F59C4
		public DirectoryEntry()
		{
		}

		// Token: 0x06005610 RID: 22032 RVA: 0x001F75CC File Offset: 0x001F59CC
		public DirectoryEntry(string path)
		{
			if (path == null)
			{
				throw new Exception("Null path in DirectoryEntry constructor");
			}
			this.Path = path.Replace('/', '\\');
			this.SlashPath = path.Replace('\\', '/');
			this.FullPath = this.Path;
			this.FullSlashPath = this.SlashPath;
			this.Uid = this.SlashPath;
			this.UidLowerInvariant = this.Uid.ToLowerInvariant();
			this.Name = Regex.Replace(this.SlashPath, ".*/", string.Empty);
		}

		// Token: 0x17000C85 RID: 3205
		// (get) Token: 0x06005611 RID: 22033 RVA: 0x001F7660 File Offset: 0x001F5A60
		// (set) Token: 0x06005612 RID: 22034 RVA: 0x001F7668 File Offset: 0x001F5A68
		public virtual string Uid
		{
			[CompilerGenerated]
			get
			{
				return this.<Uid>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<Uid>k__BackingField = value;
			}
		}

		// Token: 0x17000C86 RID: 3206
		// (get) Token: 0x06005613 RID: 22035 RVA: 0x001F7671 File Offset: 0x001F5A71
		// (set) Token: 0x06005614 RID: 22036 RVA: 0x001F7679 File Offset: 0x001F5A79
		public virtual string UidLowerInvariant
		{
			[CompilerGenerated]
			get
			{
				return this.<UidLowerInvariant>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<UidLowerInvariant>k__BackingField = value;
			}
		}

		// Token: 0x17000C87 RID: 3207
		// (get) Token: 0x06005615 RID: 22037 RVA: 0x001F7682 File Offset: 0x001F5A82
		// (set) Token: 0x06005616 RID: 22038 RVA: 0x001F768A File Offset: 0x001F5A8A
		public virtual string Path
		{
			[CompilerGenerated]
			get
			{
				return this.<Path>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<Path>k__BackingField = value;
			}
		}

		// Token: 0x17000C88 RID: 3208
		// (get) Token: 0x06005617 RID: 22039 RVA: 0x001F7693 File Offset: 0x001F5A93
		// (set) Token: 0x06005618 RID: 22040 RVA: 0x001F769B File Offset: 0x001F5A9B
		public virtual string SlashPath
		{
			[CompilerGenerated]
			get
			{
				return this.<SlashPath>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<SlashPath>k__BackingField = value;
			}
		}

		// Token: 0x17000C89 RID: 3209
		// (get) Token: 0x06005619 RID: 22041 RVA: 0x001F76A4 File Offset: 0x001F5AA4
		// (set) Token: 0x0600561A RID: 22042 RVA: 0x001F76AC File Offset: 0x001F5AAC
		public virtual string FullPath
		{
			[CompilerGenerated]
			get
			{
				return this.<FullPath>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<FullPath>k__BackingField = value;
			}
		}

		// Token: 0x17000C8A RID: 3210
		// (get) Token: 0x0600561B RID: 22043 RVA: 0x001F76B5 File Offset: 0x001F5AB5
		// (set) Token: 0x0600561C RID: 22044 RVA: 0x001F76BD File Offset: 0x001F5ABD
		public virtual string FullSlashPath
		{
			[CompilerGenerated]
			get
			{
				return this.<FullSlashPath>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<FullSlashPath>k__BackingField = value;
			}
		}

		// Token: 0x17000C8B RID: 3211
		// (get) Token: 0x0600561D RID: 22045 RVA: 0x001F76C6 File Offset: 0x001F5AC6
		// (set) Token: 0x0600561E RID: 22046 RVA: 0x001F76CE File Offset: 0x001F5ACE
		public virtual string Name
		{
			[CompilerGenerated]
			get
			{
				return this.<Name>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<Name>k__BackingField = value;
			}
		}

		// Token: 0x17000C8C RID: 3212
		// (get) Token: 0x0600561F RID: 22047 RVA: 0x001F76D7 File Offset: 0x001F5AD7
		// (set) Token: 0x06005620 RID: 22048 RVA: 0x001F76DF File Offset: 0x001F5ADF
		public virtual DateTime LastWriteTime
		{
			[CompilerGenerated]
			get
			{
				return this.<LastWriteTime>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<LastWriteTime>k__BackingField = value;
			}
		}

		// Token: 0x17000C8D RID: 3213
		// (get) Token: 0x06005621 RID: 22049 RVA: 0x001F76E8 File Offset: 0x001F5AE8
		// (set) Token: 0x06005622 RID: 22050 RVA: 0x001F76F0 File Offset: 0x001F5AF0
		public virtual DirectoryEntry Parent
		{
			[CompilerGenerated]
			get
			{
				return this.<Parent>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<Parent>k__BackingField = value;
			}
		}

		// Token: 0x17000C8E RID: 3214
		// (get) Token: 0x06005623 RID: 22051
		// (set) Token: 0x06005624 RID: 22052
		public abstract List<FileEntry> Files { get; protected set; }

		// Token: 0x17000C8F RID: 3215
		// (get) Token: 0x06005625 RID: 22053
		// (set) Token: 0x06005626 RID: 22054
		public abstract List<DirectoryEntry> SubDirectories { get; protected set; }

		// Token: 0x06005627 RID: 22055 RVA: 0x001F76F9 File Offset: 0x001F5AF9
		public override string ToString()
		{
			return this.Path;
		}

		// Token: 0x06005628 RID: 22056 RVA: 0x001F7704 File Offset: 0x001F5B04
		public virtual List<FileEntry> GetFiles(string pattern)
		{
			if (pattern == null)
			{
				return this.Files;
			}
			List<FileEntry> list = new List<FileEntry>();
			string pattern2 = "^" + Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", ".") + "$";
			foreach (FileEntry fileEntry in this.Files)
			{
				if (Regex.IsMatch(fileEntry.Name, pattern2))
				{
					list.Add(fileEntry);
				}
			}
			return list;
		}

		// Token: 0x06005629 RID: 22057 RVA: 0x001F77BC File Offset: 0x001F5BBC
		protected virtual DirectoryEntry FindFirstDirectoryWithFilesRecursive(DirectoryEntry startEntry)
		{
			if (startEntry.Files.Count != 0 || startEntry.SubDirectories.Count > 1)
			{
				return startEntry;
			}
			foreach (DirectoryEntry startEntry2 in startEntry.SubDirectories)
			{
				DirectoryEntry directoryEntry = this.FindFirstDirectoryWithFilesRecursive(startEntry2);
				if (directoryEntry != null)
				{
					return directoryEntry;
				}
			}
			return null;
		}

		// Token: 0x0600562A RID: 22058 RVA: 0x001F784C File Offset: 0x001F5C4C
		public virtual DirectoryEntry FindFirstDirectoryWithFiles()
		{
			return this.FindFirstDirectoryWithFilesRecursive(this);
		}

		// Token: 0x0600562B RID: 22059 RVA: 0x001F7855 File Offset: 0x001F5C55
		public virtual bool IsHidden()
		{
			return this.hidePath != null && File.Exists(this.hidePath);
		}

		// Token: 0x0600562C RID: 22060 RVA: 0x001F7870 File Offset: 0x001F5C70
		public virtual void SetHidden(bool b)
		{
			if (this.hidePath != null)
			{
				if (File.Exists(this.hidePath))
				{
					if (!b)
					{
						FileManager.DeleteFile(this.hidePath);
					}
				}
				else if (b)
				{
					string directoryName = FileManager.GetDirectoryName(this.hidePath, false);
					if (!FileManager.DirectoryExists(directoryName, false, false))
					{
						FileManager.CreateDirectory(directoryName);
					}
					FileManager.WriteAllText(this.hidePath, string.Empty);
				}
			}
		}

		// Token: 0x0400473B RID: 18235
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <Uid>k__BackingField;

		// Token: 0x0400473C RID: 18236
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <UidLowerInvariant>k__BackingField;

		// Token: 0x0400473D RID: 18237
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <Path>k__BackingField;

		// Token: 0x0400473E RID: 18238
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <SlashPath>k__BackingField;

		// Token: 0x0400473F RID: 18239
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <FullPath>k__BackingField;

		// Token: 0x04004740 RID: 18240
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <FullSlashPath>k__BackingField;

		// Token: 0x04004741 RID: 18241
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <Name>k__BackingField;

		// Token: 0x04004742 RID: 18242
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private DateTime <LastWriteTime>k__BackingField;

		// Token: 0x04004743 RID: 18243
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private DirectoryEntry <Parent>k__BackingField;

		// Token: 0x04004744 RID: 18244
		protected string hidePath;
	}
}
