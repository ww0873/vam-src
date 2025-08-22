using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MVR.FileManagement
{
	// Token: 0x02000BF3 RID: 3059
	public class VarDirectoryEntry : DirectoryEntry
	{
		// Token: 0x060057E6 RID: 22502 RVA: 0x0020476C File Offset: 0x00202B6C
		public VarDirectoryEntry(VarPackage vp, string entryName, VarDirectoryEntry parent = null)
		{
			this.Package = vp;
			this.InternalSlashPath = entryName;
			this.hidePath = string.Concat(new string[]
			{
				"AddonPackagesFilePrefs/",
				vp.Uid,
				"/",
				this.InternalSlashPath,
				".hide"
			});
			bool flag = false;
			if (entryName == string.Empty)
			{
				flag = true;
				this.Name = vp.Uid + ".var:";
			}
			this.InternalPath = this.InternalSlashPath.Replace("/", "\\");
			if (flag)
			{
				this.Uid = vp.Uid + ":";
				this.Path = vp.Path + ":";
				this.SlashPath = this.Path.Replace('\\', '/');
				this.FullPath = vp.FullPath + ":";
				this.FullSlashPath = this.FullPath.Replace('\\', '/');
			}
			else
			{
				this.Uid = vp.Uid + ":/" + this.InternalSlashPath;
				this.Path = vp.Path + ":\\" + this.InternalPath;
				this.SlashPath = this.Path.Replace('\\', '/');
				this.FullPath = vp.FullPath + ":\\" + this.InternalPath;
				this.FullSlashPath = this.FullPath.Replace('\\', '/');
			}
			this.Name = Regex.Replace(this.SlashPath, ".*/", string.Empty);
			this.UidLowerInvariant = this.Uid.ToLowerInvariant();
			this.LastWriteTime = vp.LastWriteTime;
			this.Parent = parent;
			this.varSubDirectories = new HashSet<VarDirectoryEntry>();
			this.varFileEntries = new List<VarFileEntry>();
			if (FileManager.debug)
			{
				UnityEngine.Debug.Log(string.Concat(new string[]
				{
					"New var directory entry\n Uid: ",
					this.Uid,
					"\n Path: ",
					this.Path,
					"\n FullPath: ",
					this.FullPath,
					"\n SlashPath: ",
					this.SlashPath,
					"\n Name: ",
					this.Name,
					"\n InternalSlashPath: ",
					this.InternalSlashPath
				}));
			}
		}

		// Token: 0x17000CC5 RID: 3269
		// (get) Token: 0x060057E7 RID: 22503 RVA: 0x002049DB File Offset: 0x00202DDB
		// (set) Token: 0x060057E8 RID: 22504 RVA: 0x002049E3 File Offset: 0x00202DE3
		public VarPackage Package
		{
			[CompilerGenerated]
			get
			{
				return this.<Package>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<Package>k__BackingField = value;
			}
		}

		// Token: 0x17000CC6 RID: 3270
		// (get) Token: 0x060057E9 RID: 22505 RVA: 0x002049EC File Offset: 0x00202DEC
		// (set) Token: 0x060057EA RID: 22506 RVA: 0x002049F4 File Offset: 0x00202DF4
		public string InternalPath
		{
			[CompilerGenerated]
			get
			{
				return this.<InternalPath>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<InternalPath>k__BackingField = value;
			}
		}

		// Token: 0x17000CC7 RID: 3271
		// (get) Token: 0x060057EB RID: 22507 RVA: 0x002049FD File Offset: 0x00202DFD
		// (set) Token: 0x060057EC RID: 22508 RVA: 0x00204A05 File Offset: 0x00202E05
		public string InternalSlashPath
		{
			[CompilerGenerated]
			get
			{
				return this.<InternalSlashPath>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<InternalSlashPath>k__BackingField = value;
			}
		}

		// Token: 0x17000CC8 RID: 3272
		// (get) Token: 0x060057ED RID: 22509 RVA: 0x00204A10 File Offset: 0x00202E10
		// (set) Token: 0x060057EE RID: 22510 RVA: 0x00204A74 File Offset: 0x00202E74
		public override List<FileEntry> Files
		{
			get
			{
				List<FileEntry> list = new List<FileEntry>();
				foreach (VarFileEntry item in this.varFileEntries)
				{
					list.Add(item);
				}
				return list;
			}
			protected set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000CC9 RID: 3273
		// (get) Token: 0x060057EF RID: 22511 RVA: 0x00204A7C File Offset: 0x00202E7C
		// (set) Token: 0x060057F0 RID: 22512 RVA: 0x00204AE0 File Offset: 0x00202EE0
		public override List<DirectoryEntry> SubDirectories
		{
			get
			{
				List<DirectoryEntry> list = new List<DirectoryEntry>();
				foreach (VarDirectoryEntry item in this.varSubDirectories)
				{
					list.Add(item);
				}
				return list;
			}
			protected set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000CCA RID: 3274
		// (get) Token: 0x060057F1 RID: 22513 RVA: 0x00204AE7 File Offset: 0x00202EE7
		// (set) Token: 0x060057F2 RID: 22514 RVA: 0x00204AF4 File Offset: 0x00202EF4
		public List<VarDirectoryEntry> VarSubDirectories
		{
			get
			{
				return this.varSubDirectories.ToList<VarDirectoryEntry>();
			}
			protected set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x060057F3 RID: 22515 RVA: 0x00204AFB File Offset: 0x00202EFB
		public void AddSubDirectory(VarDirectoryEntry subDir)
		{
			this.varSubDirectories.Add(subDir);
		}

		// Token: 0x060057F4 RID: 22516 RVA: 0x00204B0A File Offset: 0x00202F0A
		public void AddFileEntry(VarFileEntry varFileEntry)
		{
			this.varFileEntries.Add(varFileEntry);
		}

		// Token: 0x0400489D RID: 18589
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private VarPackage <Package>k__BackingField;

		// Token: 0x0400489E RID: 18590
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <InternalPath>k__BackingField;

		// Token: 0x0400489F RID: 18591
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <InternalSlashPath>k__BackingField;

		// Token: 0x040048A0 RID: 18592
		protected HashSet<VarDirectoryEntry> varSubDirectories;

		// Token: 0x040048A1 RID: 18593
		protected List<VarFileEntry> varFileEntries;
	}
}
