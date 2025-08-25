using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MVR.FileManagement
{
	// Token: 0x02000BF4 RID: 3060
	public class VarFileEntry : FileEntry
	{
		// Token: 0x060057F5 RID: 22517 RVA: 0x00204B18 File Offset: 0x00202F18
		public VarFileEntry(VarPackage vp, string entryName, DateTime lastWriteTime, long size, bool simulated = false)
		{
			this.Package = vp;
			this.InternalSlashPath = entryName;
			this.flagBasePath = string.Concat(new string[]
			{
				"AddonPackagesFilePrefs/",
				vp.Uid,
				"/",
				this.InternalSlashPath,
				"."
			});
			this.favPath = this.flagBasePath + "fav";
			base.hidePath = this.flagBasePath + "hide";
			this.Uid = vp.Uid + ":/" + this.InternalSlashPath;
			this.UidLowerInvariant = this.Uid.ToLowerInvariant();
			this.InternalPath = this.InternalSlashPath.Replace('/', '\\');
			this.Path = vp.Path + ":\\" + this.InternalPath;
			this.packagedHidePath = this.Path + ".hide";
			this.packagedFlagPath = this.Path + ".";
			this.SlashPath = this.Path.Replace('\\', '/');
			this.Name = Regex.Replace(this.SlashPath, ".*/", string.Empty);
			this.Exists = true;
			this.FullPath = vp.FullPath + ":\\" + this.InternalPath;
			this.FullSlashPath = this.FullPath.Replace('\\', '/');
			this.LastWriteTime = lastWriteTime;
			this.Size = size;
			this.Simulated = simulated;
			if (FileManager.debug)
			{
				UnityEngine.Debug.Log(string.Concat(new string[]
				{
					"New var file entry\n Uid: ",
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

		// Token: 0x17000CCB RID: 3275
		// (get) Token: 0x060057F6 RID: 22518 RVA: 0x00204D2D File Offset: 0x0020312D
		// (set) Token: 0x060057F7 RID: 22519 RVA: 0x00204D35 File Offset: 0x00203135
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

		// Token: 0x17000CCC RID: 3276
		// (get) Token: 0x060057F8 RID: 22520 RVA: 0x00204D3E File Offset: 0x0020313E
		// (set) Token: 0x060057F9 RID: 22521 RVA: 0x00204D46 File Offset: 0x00203146
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

		// Token: 0x17000CCD RID: 3277
		// (get) Token: 0x060057FA RID: 22522 RVA: 0x00204D4F File Offset: 0x0020314F
		// (set) Token: 0x060057FB RID: 22523 RVA: 0x00204D57 File Offset: 0x00203157
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

		// Token: 0x17000CCE RID: 3278
		// (get) Token: 0x060057FC RID: 22524 RVA: 0x00204D60 File Offset: 0x00203160
		// (set) Token: 0x060057FD RID: 22525 RVA: 0x00204D68 File Offset: 0x00203168
		public bool Simulated
		{
			[CompilerGenerated]
			get
			{
				return this.<Simulated>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<Simulated>k__BackingField = value;
			}
		}

		// Token: 0x060057FE RID: 22526 RVA: 0x00204D74 File Offset: 0x00203174
		public override FileEntryStream OpenStream()
		{
			return new VarFileEntryStream(this);
		}

		// Token: 0x060057FF RID: 22527 RVA: 0x00204D8C File Offset: 0x0020318C
		public override FileEntryStreamReader OpenStreamReader()
		{
			return new VarFileEntryStreamReader(this);
		}

		// Token: 0x06005800 RID: 22528 RVA: 0x00204DA4 File Offset: 0x002031A4
		public override bool HasFlagFile(string flagName)
		{
			return (this.flagBasePath != null && File.Exists(this.flagBasePath + flagName)) || (this.packagedFlagPath != null && FileManager.FileExists(this.packagedFlagPath + flagName, false, false));
		}

		// Token: 0x06005801 RID: 22529 RVA: 0x00204DF6 File Offset: 0x002031F6
		public bool IsFlagFileModifiable(string flagName)
		{
			return this.packagedFlagPath == null || !FileManager.FileExists(this.packagedFlagPath + flagName, false, false);
		}

		// Token: 0x06005802 RID: 22530 RVA: 0x00204E1C File Offset: 0x0020321C
		public override bool IsHidden()
		{
			return (base.hidePath != null && File.Exists(base.hidePath)) || (this.packagedHidePath != null && FileManager.FileExists(this.packagedHidePath, false, false));
		}

		// Token: 0x06005803 RID: 22531 RVA: 0x00204E57 File Offset: 0x00203257
		public bool IsHiddenModifiable()
		{
			return this.packagedHidePath == null || !FileManager.FileExists(this.packagedHidePath, false, false);
		}

		// Token: 0x040048A2 RID: 18594
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private VarPackage <Package>k__BackingField;

		// Token: 0x040048A3 RID: 18595
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <InternalPath>k__BackingField;

		// Token: 0x040048A4 RID: 18596
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <InternalSlashPath>k__BackingField;

		// Token: 0x040048A5 RID: 18597
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool <Simulated>k__BackingField;

		// Token: 0x040048A6 RID: 18598
		protected string packagedHidePath;

		// Token: 0x040048A7 RID: 18599
		protected string packagedFlagPath;
	}
}
