using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace MVR.FileManagement
{
	// Token: 0x02000BDA RID: 3034
	public abstract class FileEntry
	{
		// Token: 0x0600562F RID: 22063 RVA: 0x001F79C6 File Offset: 0x001F5DC6
		public FileEntry()
		{
		}

		// Token: 0x06005630 RID: 22064 RVA: 0x001F79D0 File Offset: 0x001F5DD0
		public FileEntry(string path)
		{
			if (path == null)
			{
				throw new Exception("Null path in FileEntry constructor");
			}
			this.Path = path.Replace('/', '\\');
			this.SlashPath = path.Replace('\\', '/');
			this.FullPath = this.Path;
			this.FullSlashPath = this.SlashPath;
			this.Uid = this.SlashPath;
			this.UidLowerInvariant = this.Uid.ToLowerInvariant();
			this.Name = Regex.Replace(this.SlashPath, ".*/", string.Empty);
		}

		// Token: 0x17000C90 RID: 3216
		// (get) Token: 0x06005631 RID: 22065 RVA: 0x001F7A64 File Offset: 0x001F5E64
		// (set) Token: 0x06005632 RID: 22066 RVA: 0x001F7A6C File Offset: 0x001F5E6C
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

		// Token: 0x17000C91 RID: 3217
		// (get) Token: 0x06005633 RID: 22067 RVA: 0x001F7A75 File Offset: 0x001F5E75
		// (set) Token: 0x06005634 RID: 22068 RVA: 0x001F7A7D File Offset: 0x001F5E7D
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

		// Token: 0x17000C92 RID: 3218
		// (get) Token: 0x06005635 RID: 22069 RVA: 0x001F7A86 File Offset: 0x001F5E86
		// (set) Token: 0x06005636 RID: 22070 RVA: 0x001F7A8E File Offset: 0x001F5E8E
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

		// Token: 0x17000C93 RID: 3219
		// (get) Token: 0x06005637 RID: 22071 RVA: 0x001F7A97 File Offset: 0x001F5E97
		// (set) Token: 0x06005638 RID: 22072 RVA: 0x001F7A9F File Offset: 0x001F5E9F
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

		// Token: 0x17000C94 RID: 3220
		// (get) Token: 0x06005639 RID: 22073 RVA: 0x001F7AA8 File Offset: 0x001F5EA8
		// (set) Token: 0x0600563A RID: 22074 RVA: 0x001F7AB0 File Offset: 0x001F5EB0
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

		// Token: 0x17000C95 RID: 3221
		// (get) Token: 0x0600563B RID: 22075 RVA: 0x001F7AB9 File Offset: 0x001F5EB9
		// (set) Token: 0x0600563C RID: 22076 RVA: 0x001F7AC1 File Offset: 0x001F5EC1
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

		// Token: 0x17000C96 RID: 3222
		// (get) Token: 0x0600563D RID: 22077 RVA: 0x001F7ACA File Offset: 0x001F5ECA
		// (set) Token: 0x0600563E RID: 22078 RVA: 0x001F7AD2 File Offset: 0x001F5ED2
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

		// Token: 0x17000C97 RID: 3223
		// (get) Token: 0x0600563F RID: 22079 RVA: 0x001F7ADB File Offset: 0x001F5EDB
		// (set) Token: 0x06005640 RID: 22080 RVA: 0x001F7AE3 File Offset: 0x001F5EE3
		public virtual bool Exists
		{
			[CompilerGenerated]
			get
			{
				return this.<Exists>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<Exists>k__BackingField = value;
			}
		}

		// Token: 0x17000C98 RID: 3224
		// (get) Token: 0x06005641 RID: 22081 RVA: 0x001F7AEC File Offset: 0x001F5EEC
		// (set) Token: 0x06005642 RID: 22082 RVA: 0x001F7AF4 File Offset: 0x001F5EF4
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

		// Token: 0x17000C99 RID: 3225
		// (get) Token: 0x06005643 RID: 22083 RVA: 0x001F7AFD File Offset: 0x001F5EFD
		// (set) Token: 0x06005644 RID: 22084 RVA: 0x001F7B05 File Offset: 0x001F5F05
		public virtual long Size
		{
			[CompilerGenerated]
			get
			{
				return this.<Size>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<Size>k__BackingField = value;
			}
		}

		// Token: 0x06005645 RID: 22085 RVA: 0x001F7B0E File Offset: 0x001F5F0E
		public override string ToString()
		{
			return this.Path;
		}

		// Token: 0x06005646 RID: 22086
		public abstract FileEntryStream OpenStream();

		// Token: 0x06005647 RID: 22087
		public abstract FileEntryStreamReader OpenStreamReader();

		// Token: 0x17000C9A RID: 3226
		// (get) Token: 0x06005648 RID: 22088 RVA: 0x001F7B16 File Offset: 0x001F5F16
		// (set) Token: 0x06005649 RID: 22089 RVA: 0x001F7B1E File Offset: 0x001F5F1E
		public string hidePath
		{
			[CompilerGenerated]
			get
			{
				return this.<hidePath>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<hidePath>k__BackingField = value;
			}
		}

		// Token: 0x0600564A RID: 22090 RVA: 0x001F7B27 File Offset: 0x001F5F27
		public virtual bool HasFlagFile(string flagName)
		{
			return this.flagBasePath != null && File.Exists(this.flagBasePath + flagName);
		}

		// Token: 0x0600564B RID: 22091 RVA: 0x001F7B48 File Offset: 0x001F5F48
		public virtual void SetFlagFile(string flagName, bool b)
		{
			if (this.flagBasePath != null)
			{
				string path = this.flagBasePath + flagName;
				if (File.Exists(path))
				{
					if (!b)
					{
						FileManager.DeleteFile(path);
					}
				}
				else if (b)
				{
					string directoryName = FileManager.GetDirectoryName(path, false);
					if (!FileManager.DirectoryExists(directoryName, false, false))
					{
						FileManager.CreateDirectory(directoryName);
					}
					FileManager.WriteAllText(path, string.Empty);
				}
			}
		}

		// Token: 0x0600564C RID: 22092 RVA: 0x001F7BB5 File Offset: 0x001F5FB5
		public virtual bool IsFavorite()
		{
			return this.favPath != null && File.Exists(this.favPath);
		}

		// Token: 0x0600564D RID: 22093 RVA: 0x001F7BD0 File Offset: 0x001F5FD0
		public virtual void SetFavorite(bool b)
		{
			if (this.favPath != null)
			{
				if (File.Exists(this.favPath))
				{
					if (!b)
					{
						FileManager.DeleteFile(this.favPath);
					}
				}
				else if (b)
				{
					string directoryName = FileManager.GetDirectoryName(this.favPath, false);
					if (!FileManager.DirectoryExists(directoryName, false, false))
					{
						FileManager.CreateDirectory(directoryName);
					}
					FileManager.WriteAllText(this.favPath, string.Empty);
				}
			}
		}

		// Token: 0x0600564E RID: 22094 RVA: 0x001F7C44 File Offset: 0x001F6044
		public virtual bool IsHidden()
		{
			return this.hidePath != null && File.Exists(this.hidePath);
		}

		// Token: 0x0600564F RID: 22095 RVA: 0x001F7C60 File Offset: 0x001F6060
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

		// Token: 0x04004745 RID: 18245
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <Uid>k__BackingField;

		// Token: 0x04004746 RID: 18246
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <UidLowerInvariant>k__BackingField;

		// Token: 0x04004747 RID: 18247
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <Path>k__BackingField;

		// Token: 0x04004748 RID: 18248
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <SlashPath>k__BackingField;

		// Token: 0x04004749 RID: 18249
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <FullPath>k__BackingField;

		// Token: 0x0400474A RID: 18250
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <FullSlashPath>k__BackingField;

		// Token: 0x0400474B RID: 18251
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <Name>k__BackingField;

		// Token: 0x0400474C RID: 18252
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool <Exists>k__BackingField;

		// Token: 0x0400474D RID: 18253
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private DateTime <LastWriteTime>k__BackingField;

		// Token: 0x0400474E RID: 18254
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private long <Size>k__BackingField;

		// Token: 0x0400474F RID: 18255
		protected string flagBasePath;

		// Token: 0x04004750 RID: 18256
		protected string favPath;

		// Token: 0x04004751 RID: 18257
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <hidePath>k__BackingField;
	}
}
