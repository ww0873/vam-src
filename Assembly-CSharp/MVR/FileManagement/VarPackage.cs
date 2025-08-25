using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using MVR.Hub;
using SimpleJSON;
using UnityEngine;

namespace MVR.FileManagement
{
	// Token: 0x02000BF7 RID: 3063
	public class VarPackage
	{
		// Token: 0x06005808 RID: 22536 RVA: 0x00204F98 File Offset: 0x00203398
		public VarPackage(string uid, string path, VarPackageGroup group, string creator, string name, int version)
		{
			this.Uid = uid;
			this.UidLowerInvariant = uid.ToLowerInvariant();
			this.Path = path.Replace('/', '\\');
			this.SlashPath = path.Replace('\\', '/');
			this.FullPath = System.IO.Path.GetFullPath(this.Path);
			this.FullSlashPath = this.FullPath.Replace('\\', '/');
			this.Name = name;
			this.Group = group;
			this.GroupName = group.Name;
			this.Creator = creator;
			this.Version = version;
			this.HadReferenceIssues = false;
			this.PackageDependencies = new List<string>();
			this.PackageDependenciesMissing = new HashSet<string>();
			this.PackageDependenciesResolved = new List<VarPackage>();
			if (FileManager.debug)
			{
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"New package\n Uid: ",
					this.Uid,
					"\n Path: ",
					this.Path,
					"\n FullPath: ",
					this.FullPath,
					"\n SlashPath: ",
					this.SlashPath,
					"\n Name: ",
					this.Name,
					"\n GroupName: ",
					this.GroupName,
					"\n Creator: ",
					this.Creator,
					"\n Version: ",
					this.Version
				}));
			}
			this.Scan();
		}

		// Token: 0x17000CCF RID: 3279
		// (get) Token: 0x06005809 RID: 22537 RVA: 0x00205128 File Offset: 0x00203528
		// (set) Token: 0x0600580A RID: 22538 RVA: 0x00205130 File Offset: 0x00203530
		public bool invalid
		{
			[CompilerGenerated]
			get
			{
				return this.<invalid>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<invalid>k__BackingField = value;
			}
		}

		// Token: 0x17000CD0 RID: 3280
		// (get) Token: 0x0600580B RID: 22539 RVA: 0x00205139 File Offset: 0x00203539
		// (set) Token: 0x0600580C RID: 22540 RVA: 0x00205141 File Offset: 0x00203541
		public bool forceRefresh
		{
			[CompilerGenerated]
			get
			{
				return this.<forceRefresh>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<forceRefresh>k__BackingField = value;
			}
		}

		// Token: 0x0600580D RID: 22541 RVA: 0x0020514A File Offset: 0x0020354A
		protected void SyncEnabled()
		{
			this._enabled = !FileManager.FileExists(this.Path + ".disabled", false, false);
		}

		// Token: 0x17000CD1 RID: 3281
		// (get) Token: 0x0600580E RID: 22542 RVA: 0x0020516C File Offset: 0x0020356C
		// (set) Token: 0x0600580F RID: 22543 RVA: 0x00205174 File Offset: 0x00203574
		public bool Enabled
		{
			get
			{
				return this._enabled;
			}
			set
			{
				if (this._enabled != value)
				{
					this._enabled = value;
					string path = this.Path + ".disabled";
					if (FileManager.FileExists(path, false, false))
					{
						if (this._enabled)
						{
							FileManager.DeleteFile(path);
							FileManager.Refresh();
						}
					}
					else if (!this._enabled)
					{
						FileManager.WriteAllText(path, string.Empty);
						FileManager.Refresh();
					}
				}
			}
		}

		// Token: 0x06005810 RID: 22544 RVA: 0x002051E8 File Offset: 0x002035E8
		public void Delete()
		{
			if (this.ZipFile != null)
			{
				this.ZipFile.Close();
				this.ZipFile = null;
			}
			if (File.Exists(this.Path))
			{
				FileManager.DeleteFile(this.Path);
			}
			else if (Directory.Exists(this.Path))
			{
				FileManager.DeleteDirectory(this.Path, true);
			}
			string path = this.Path + ".disabled";
			if (File.Exists(path))
			{
				FileManager.DeleteFile(path);
			}
			FileManager.Refresh();
		}

		// Token: 0x17000CD2 RID: 3282
		// (get) Token: 0x06005811 RID: 22545 RVA: 0x00205275 File Offset: 0x00203675
		// (set) Token: 0x06005812 RID: 22546 RVA: 0x0020527D File Offset: 0x0020367D
		public bool PluginsAlwaysEnabled
		{
			get
			{
				return this._pluginsAlwaysEnabled;
			}
			set
			{
				if (this._pluginsAlwaysEnabled != value)
				{
					this._pluginsAlwaysEnabled = value;
					this.SaveUserPrefs();
				}
			}
		}

		// Token: 0x17000CD3 RID: 3283
		// (get) Token: 0x06005813 RID: 22547 RVA: 0x00205298 File Offset: 0x00203698
		// (set) Token: 0x06005814 RID: 22548 RVA: 0x002052A0 File Offset: 0x002036A0
		public bool PluginsAlwaysDisabled
		{
			get
			{
				return this._pluginsAlwaysDisabled;
			}
			set
			{
				if (this._pluginsAlwaysDisabled != value)
				{
					this._pluginsAlwaysDisabled = value;
					this.SaveUserPrefs();
				}
			}
		}

		// Token: 0x17000CD4 RID: 3284
		// (get) Token: 0x06005815 RID: 22549 RVA: 0x002052BB File Offset: 0x002036BB
		// (set) Token: 0x06005816 RID: 22550 RVA: 0x002052C3 File Offset: 0x002036C3
		public bool IgnoreMissingDependencyErrors
		{
			get
			{
				return this._ignoreMissingDependencyErrors;
			}
			set
			{
				if (this._ignoreMissingDependencyErrors != value)
				{
					this._ignoreMissingDependencyErrors = value;
					this.SaveUserPrefs();
				}
			}
		}

		// Token: 0x06005817 RID: 22551 RVA: 0x002052E0 File Offset: 0x002036E0
		protected void LoadUserPrefs()
		{
			string path = FileManager.UserPrefsFolder + "/" + this.Uid + ".prefs";
			if (FileManager.FileExists(path, false, false))
			{
				using (FileEntryStreamReader fileEntryStreamReader = FileManager.OpenStreamReader(path, false))
				{
					string aJSON = fileEntryStreamReader.ReadToEnd();
					JSONClass asObject = JSON.Parse(aJSON).AsObject;
					if (asObject != null)
					{
						this._pluginsAlwaysEnabled = asObject["pluginsAlwaysEnabled"].AsBool;
						this._pluginsAlwaysDisabled = asObject["pluginsAlwaysDisabled"].AsBool;
						this._ignoreMissingDependencyErrors = asObject["ignoreMissingDependencyErrors"].AsBool;
					}
				}
			}
			else
			{
				this._pluginsAlwaysEnabled = false;
				this._pluginsAlwaysDisabled = false;
				this._ignoreMissingDependencyErrors = false;
			}
		}

		// Token: 0x06005818 RID: 22552 RVA: 0x002053BC File Offset: 0x002037BC
		protected void SaveUserPrefs()
		{
			string text = FileManager.UserPrefsFolder + "/" + this.Uid + ".prefs";
			JSONClass jsonclass = new JSONClass();
			jsonclass["pluginsAlwaysEnabled"].AsBool = this._pluginsAlwaysEnabled;
			jsonclass["pluginsAlwaysDisabled"].AsBool = this._pluginsAlwaysDisabled;
			jsonclass["ignoreMissingDependencyErrors"].AsBool = this._ignoreMissingDependencyErrors;
			string text2 = jsonclass.ToString(string.Empty);
			try
			{
				FileManager.WriteAllText(text, text2);
			}
			catch (Exception ex)
			{
				SuperController.LogError("Error during save of prefs file " + text + ": " + ex.Message);
			}
		}

		// Token: 0x17000CD5 RID: 3285
		// (get) Token: 0x06005819 RID: 22553 RVA: 0x00205478 File Offset: 0x00203878
		// (set) Token: 0x0600581A RID: 22554 RVA: 0x00205480 File Offset: 0x00203880
		public float packProgress
		{
			[CompilerGenerated]
			get
			{
				return this.<packProgress>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<packProgress>k__BackingField = value;
			}
		}

		// Token: 0x0600581B RID: 22555 RVA: 0x0020548C File Offset: 0x0020388C
		protected void ProcessFileMethod(object sender, ScanEventArgs args)
		{
			this.packFileProgressCount++;
			if (this.packFileTotalCount != 0)
			{
				this.packProgress = (float)this.packFileProgressCount / (float)this.packFileTotalCount;
			}
			if (this.packThreadAbort)
			{
				args.ContinueRunning = false;
			}
		}

		// Token: 0x0600581C RID: 22556 RVA: 0x002054DC File Offset: 0x002038DC
		protected void UnpackThreaded()
		{
			try
			{
				string text = this.Path + ".orig";
				if (!FileManager.FileExists(text, false, false))
				{
					FileManager.CopyFile(this.Path, text, false);
				}
				FastZip fastZip = new FastZip(new FastZipEvents
				{
					ProcessFile = new ProcessFileHandler(this.ProcessFileMethod)
				});
				string text2 = this.Path + ".extracted";
				if (FileManager.DirectoryExists(text2, false, false))
				{
					FileManager.DeleteDirectory(text2, true);
				}
				fastZip.ExtractZip(this.Path, text2, FastZip.Overwrite.Always, null, string.Empty, string.Empty, true);
				if (this.ZipFile != null)
				{
					this.ZipFile.Close();
					this.ZipFile = null;
				}
				FileManager.DeleteFile(this.Path);
				FileManager.MoveDirectory(text2, this.Path);
			}
			catch (Exception ex)
			{
				this.packThreadError = ex.Message;
			}
		}

		// Token: 0x17000CD6 RID: 3286
		// (get) Token: 0x0600581D RID: 22557 RVA: 0x002055D0 File Offset: 0x002039D0
		public bool IsUnpacking
		{
			get
			{
				return this.unpackThread != null && this.unpackThread.IsAlive;
			}
		}

		// Token: 0x0600581E RID: 22558 RVA: 0x002055F0 File Offset: 0x002039F0
		public void Unpack()
		{
			if (!this.IsSimulated && (this.unpackThread == null || !this.unpackThread.IsAlive))
			{
				this.packThreadError = null;
				this.packThreadAbort = false;
				this.packProgress = 0f;
				this.packFileProgressCount = 0;
				this.packFileTotalCount = this.FileEntries.Count;
				this.unpackThread = new Thread(new ThreadStart(this.UnpackThreaded));
				this.unpackThread.Start();
			}
		}

		// Token: 0x0600581F RID: 22559 RVA: 0x00205678 File Offset: 0x00203A78
		protected void RepackThreaded()
		{
			try
			{
				string text = this.SlashPath + ".zip";
				if (FileManager.FileExists(text, false, false))
				{
					FileManager.DeleteFile(text);
				}
				using (ZipOutputStream zipOutputStream = new ZipOutputStream(File.Create(text)))
				{
					string[] files = Directory.GetFiles(this.SlashPath, "*", SearchOption.AllDirectories);
					byte[] buffer = new byte[32768];
					foreach (string text2 in files)
					{
						string text3 = Regex.Replace(text2, "\\\\", "/");
						text3 = Regex.Replace(text3, "^" + Regex.Escape(this.SlashPath) + "/", string.Empty);
						ZipEntry zipEntry = new ZipEntry(text3);
						zipEntry.IsUnicodeText = true;
						string extension = System.IO.Path.GetExtension(text2);
						if (Regex.IsMatch(extension, "(mp3|ogg|wav|jpg|jpeg|png|gif|tif|tiff|assetbundle|scene|vac|zip)", RegexOptions.IgnoreCase))
						{
							zipOutputStream.SetLevel(0);
						}
						else
						{
							zipOutputStream.SetLevel(9);
						}
						zipOutputStream.PutNextEntry(zipEntry);
						using (FileEntryStream fileEntryStream = FileManager.OpenStream(text2, false))
						{
							StreamUtils.Copy(fileEntryStream.Stream, zipOutputStream, buffer);
						}
						zipEntry.DateTime = File.GetLastWriteTime(text2);
						zipOutputStream.CloseEntry();
						this.packFileProgressCount++;
						if (this.packFileTotalCount != 0)
						{
							this.packProgress = (float)this.packFileProgressCount / (float)this.packFileTotalCount;
						}
						if (this.packThreadAbort)
						{
							return;
						}
					}
				}
				string text4 = this.Path + ".todelete";
				try
				{
					FileManager.MoveDirectory(this.Path, text4);
				}
				catch (Exception)
				{
					this.packThreadError = "Error during attempt of move and delete of " + this.Path + ". Do you have this folder open in explorer or files in this folder open in another tool?";
					return;
				}
				FileManager.MoveFile(text, this.Path, true);
				FileStream file = File.Open(this.Path, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Write | FileShare.Delete);
				this.ZipFile = new ZipFile(file)
				{
					IsStreamOwner = true
				};
				FileManager.DeleteDirectory(text4, true);
			}
			catch (Exception ex)
			{
				this.packThreadError = ex.Message;
			}
		}

		// Token: 0x17000CD7 RID: 3287
		// (get) Token: 0x06005820 RID: 22560 RVA: 0x00205908 File Offset: 0x00203D08
		public bool IsRepacking
		{
			get
			{
				return this.repackThread != null && this.repackThread.IsAlive;
			}
		}

		// Token: 0x06005821 RID: 22561 RVA: 0x00205928 File Offset: 0x00203D28
		public void Repack()
		{
			if (this.IsSimulated && (this.repackThread == null || !this.repackThread.IsAlive))
			{
				this.packThreadError = null;
				this.packThreadAbort = false;
				this.packProgress = 0f;
				this.packFileProgressCount = 0;
				this.packFileTotalCount = FileManager.FolderContentsCount(this.Path);
				this.repackThread = new Thread(new ThreadStart(this.RepackThreaded));
				this.repackThread.Start();
			}
		}

		// Token: 0x17000CD8 RID: 3288
		// (get) Token: 0x06005822 RID: 22562 RVA: 0x002059B0 File Offset: 0x00203DB0
		public bool HasOriginalCopy
		{
			get
			{
				string path = this.Path + ".orig";
				return FileManager.FileExists(path, false, false);
			}
		}

		// Token: 0x06005823 RID: 22563 RVA: 0x002059D8 File Offset: 0x00203DD8
		public void RestoreFromOriginal()
		{
			string text = this.Path + ".orig";
			if (FileManager.FileExists(text, false, false))
			{
				if (FileManager.DirectoryExists(this.Path, false, false))
				{
					FileManager.DeleteDirectory(this.Path, true);
				}
				else if (FileManager.FileExists(this.Path, false, false))
				{
					if (this.ZipFile != null)
					{
						this.ZipFile.Close();
						this.ZipFile = null;
					}
					FileManager.DeleteFile(this.Path);
				}
				FileManager.MoveFile(text, this.Path, true);
			}
		}

		// Token: 0x17000CD9 RID: 3289
		// (get) Token: 0x06005824 RID: 22564 RVA: 0x00205A6D File Offset: 0x00203E6D
		// (set) Token: 0x06005825 RID: 22565 RVA: 0x00205A75 File Offset: 0x00203E75
		public bool IsSimulated
		{
			[CompilerGenerated]
			get
			{
				return this.<IsSimulated>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<IsSimulated>k__BackingField = value;
			}
		}

		// Token: 0x17000CDA RID: 3290
		// (get) Token: 0x06005826 RID: 22566 RVA: 0x00205A7E File Offset: 0x00203E7E
		// (set) Token: 0x06005827 RID: 22567 RVA: 0x00205A86 File Offset: 0x00203E86
		public ZipFile ZipFile
		{
			[CompilerGenerated]
			get
			{
				return this.<ZipFile>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<ZipFile>k__BackingField = value;
			}
		}

		// Token: 0x17000CDB RID: 3291
		// (get) Token: 0x06005828 RID: 22568 RVA: 0x00205A8F File Offset: 0x00203E8F
		// (set) Token: 0x06005829 RID: 22569 RVA: 0x00205A97 File Offset: 0x00203E97
		public string Uid
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

		// Token: 0x17000CDC RID: 3292
		// (get) Token: 0x0600582A RID: 22570 RVA: 0x00205AA0 File Offset: 0x00203EA0
		// (set) Token: 0x0600582B RID: 22571 RVA: 0x00205AA8 File Offset: 0x00203EA8
		public string UidLowerInvariant
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

		// Token: 0x17000CDD RID: 3293
		// (get) Token: 0x0600582C RID: 22572 RVA: 0x00205AB1 File Offset: 0x00203EB1
		// (set) Token: 0x0600582D RID: 22573 RVA: 0x00205AB9 File Offset: 0x00203EB9
		public string Path
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

		// Token: 0x17000CDE RID: 3294
		// (get) Token: 0x0600582E RID: 22574 RVA: 0x00205AC2 File Offset: 0x00203EC2
		// (set) Token: 0x0600582F RID: 22575 RVA: 0x00205ACA File Offset: 0x00203ECA
		public string SlashPath
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

		// Token: 0x17000CDF RID: 3295
		// (get) Token: 0x06005830 RID: 22576 RVA: 0x00205AD3 File Offset: 0x00203ED3
		// (set) Token: 0x06005831 RID: 22577 RVA: 0x00205ADB File Offset: 0x00203EDB
		public string FullPath
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

		// Token: 0x17000CE0 RID: 3296
		// (get) Token: 0x06005832 RID: 22578 RVA: 0x00205AE4 File Offset: 0x00203EE4
		// (set) Token: 0x06005833 RID: 22579 RVA: 0x00205AEC File Offset: 0x00203EEC
		public string FullSlashPath
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

		// Token: 0x17000CE1 RID: 3297
		// (get) Token: 0x06005834 RID: 22580 RVA: 0x00205AF5 File Offset: 0x00203EF5
		// (set) Token: 0x06005835 RID: 22581 RVA: 0x00205AFD File Offset: 0x00203EFD
		public VarPackageGroup Group
		{
			[CompilerGenerated]
			get
			{
				return this.<Group>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<Group>k__BackingField = value;
			}
		}

		// Token: 0x17000CE2 RID: 3298
		// (get) Token: 0x06005836 RID: 22582 RVA: 0x00205B06 File Offset: 0x00203F06
		// (set) Token: 0x06005837 RID: 22583 RVA: 0x00205B0E File Offset: 0x00203F0E
		public string GroupName
		{
			[CompilerGenerated]
			get
			{
				return this.<GroupName>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<GroupName>k__BackingField = value;
			}
		}

		// Token: 0x17000CE3 RID: 3299
		// (get) Token: 0x06005838 RID: 22584 RVA: 0x00205B17 File Offset: 0x00203F17
		// (set) Token: 0x06005839 RID: 22585 RVA: 0x00205B1F File Offset: 0x00203F1F
		public string Creator
		{
			[CompilerGenerated]
			get
			{
				return this.<Creator>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<Creator>k__BackingField = value;
			}
		}

		// Token: 0x17000CE4 RID: 3300
		// (get) Token: 0x0600583A RID: 22586 RVA: 0x00205B28 File Offset: 0x00203F28
		// (set) Token: 0x0600583B RID: 22587 RVA: 0x00205B30 File Offset: 0x00203F30
		public string Name
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

		// Token: 0x17000CE5 RID: 3301
		// (get) Token: 0x0600583C RID: 22588 RVA: 0x00205B39 File Offset: 0x00203F39
		// (set) Token: 0x0600583D RID: 22589 RVA: 0x00205B41 File Offset: 0x00203F41
		public int Version
		{
			[CompilerGenerated]
			get
			{
				return this.<Version>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<Version>k__BackingField = value;
			}
		}

		// Token: 0x17000CE6 RID: 3302
		// (get) Token: 0x0600583E RID: 22590 RVA: 0x00205B4A File Offset: 0x00203F4A
		// (set) Token: 0x0600583F RID: 22591 RVA: 0x00205B52 File Offset: 0x00203F52
		public VarPackage.ReferenceVersionOption StandardReferenceVersionOption
		{
			[CompilerGenerated]
			get
			{
				return this.<StandardReferenceVersionOption>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<StandardReferenceVersionOption>k__BackingField = value;
			}
		}

		// Token: 0x17000CE7 RID: 3303
		// (get) Token: 0x06005840 RID: 22592 RVA: 0x00205B5B File Offset: 0x00203F5B
		// (set) Token: 0x06005841 RID: 22593 RVA: 0x00205B63 File Offset: 0x00203F63
		public VarPackage.ReferenceVersionOption ScriptReferenceVersionOption
		{
			[CompilerGenerated]
			get
			{
				return this.<ScriptReferenceVersionOption>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<ScriptReferenceVersionOption>k__BackingField = value;
			}
		}

		// Token: 0x17000CE8 RID: 3304
		// (get) Token: 0x06005842 RID: 22594 RVA: 0x00205B6C File Offset: 0x00203F6C
		// (set) Token: 0x06005843 RID: 22595 RVA: 0x00205B74 File Offset: 0x00203F74
		public DateTime LastWriteTime
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

		// Token: 0x17000CE9 RID: 3305
		// (get) Token: 0x06005844 RID: 22596 RVA: 0x00205B7D File Offset: 0x00203F7D
		// (set) Token: 0x06005845 RID: 22597 RVA: 0x00205B85 File Offset: 0x00203F85
		public long Size
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

		// Token: 0x17000CEA RID: 3306
		// (get) Token: 0x06005846 RID: 22598 RVA: 0x00205B8E File Offset: 0x00203F8E
		// (set) Token: 0x06005847 RID: 22599 RVA: 0x00205B96 File Offset: 0x00203F96
		public List<VarFileEntry> FileEntries
		{
			[CompilerGenerated]
			get
			{
				return this.<FileEntries>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<FileEntries>k__BackingField = value;
			}
		}

		// Token: 0x17000CEB RID: 3307
		// (get) Token: 0x06005848 RID: 22600 RVA: 0x00205B9F File Offset: 0x00203F9F
		// (set) Token: 0x06005849 RID: 22601 RVA: 0x00205BA7 File Offset: 0x00203FA7
		public List<VarDirectoryEntry> DirectoryEntries
		{
			[CompilerGenerated]
			get
			{
				return this.<DirectoryEntries>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<DirectoryEntries>k__BackingField = value;
			}
		}

		// Token: 0x17000CEC RID: 3308
		// (get) Token: 0x0600584A RID: 22602 RVA: 0x00205BB0 File Offset: 0x00203FB0
		// (set) Token: 0x0600584B RID: 22603 RVA: 0x00205BB8 File Offset: 0x00203FB8
		public string LicenseType
		{
			[CompilerGenerated]
			get
			{
				return this.<LicenseType>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<LicenseType>k__BackingField = value;
			}
		}

		// Token: 0x17000CED RID: 3309
		// (get) Token: 0x0600584C RID: 22604 RVA: 0x00205BC1 File Offset: 0x00203FC1
		// (set) Token: 0x0600584D RID: 22605 RVA: 0x00205BC9 File Offset: 0x00203FC9
		public string SecondaryLicenseType
		{
			[CompilerGenerated]
			get
			{
				return this.<SecondaryLicenseType>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<SecondaryLicenseType>k__BackingField = value;
			}
		}

		// Token: 0x17000CEE RID: 3310
		// (get) Token: 0x0600584E RID: 22606 RVA: 0x00205BD2 File Offset: 0x00203FD2
		// (set) Token: 0x0600584F RID: 22607 RVA: 0x00205BDA File Offset: 0x00203FDA
		public string EAEndYear
		{
			[CompilerGenerated]
			get
			{
				return this.<EAEndYear>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<EAEndYear>k__BackingField = value;
			}
		}

		// Token: 0x17000CEF RID: 3311
		// (get) Token: 0x06005850 RID: 22608 RVA: 0x00205BE3 File Offset: 0x00203FE3
		// (set) Token: 0x06005851 RID: 22609 RVA: 0x00205BEB File Offset: 0x00203FEB
		public string EAEndMonth
		{
			[CompilerGenerated]
			get
			{
				return this.<EAEndMonth>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<EAEndMonth>k__BackingField = value;
			}
		}

		// Token: 0x17000CF0 RID: 3312
		// (get) Token: 0x06005852 RID: 22610 RVA: 0x00205BF4 File Offset: 0x00203FF4
		// (set) Token: 0x06005853 RID: 22611 RVA: 0x00205BFC File Offset: 0x00203FFC
		public string EAEndDay
		{
			[CompilerGenerated]
			get
			{
				return this.<EAEndDay>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<EAEndDay>k__BackingField = value;
			}
		}

		// Token: 0x17000CF1 RID: 3313
		// (get) Token: 0x06005854 RID: 22612 RVA: 0x00205C05 File Offset: 0x00204005
		// (set) Token: 0x06005855 RID: 22613 RVA: 0x00205C0D File Offset: 0x0020400D
		public string Description
		{
			[CompilerGenerated]
			get
			{
				return this.<Description>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<Description>k__BackingField = value;
			}
		}

		// Token: 0x17000CF2 RID: 3314
		// (get) Token: 0x06005856 RID: 22614 RVA: 0x00205C16 File Offset: 0x00204016
		// (set) Token: 0x06005857 RID: 22615 RVA: 0x00205C1E File Offset: 0x0020401E
		public string Credits
		{
			[CompilerGenerated]
			get
			{
				return this.<Credits>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<Credits>k__BackingField = value;
			}
		}

		// Token: 0x17000CF3 RID: 3315
		// (get) Token: 0x06005858 RID: 22616 RVA: 0x00205C27 File Offset: 0x00204027
		// (set) Token: 0x06005859 RID: 22617 RVA: 0x00205C2F File Offset: 0x0020402F
		public string Instructions
		{
			[CompilerGenerated]
			get
			{
				return this.<Instructions>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<Instructions>k__BackingField = value;
			}
		}

		// Token: 0x17000CF4 RID: 3316
		// (get) Token: 0x0600585A RID: 22618 RVA: 0x00205C38 File Offset: 0x00204038
		// (set) Token: 0x0600585B RID: 22619 RVA: 0x00205C40 File Offset: 0x00204040
		public string PromotionalLink
		{
			[CompilerGenerated]
			get
			{
				return this.<PromotionalLink>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<PromotionalLink>k__BackingField = value;
			}
		}

		// Token: 0x17000CF5 RID: 3317
		// (get) Token: 0x0600585C RID: 22620 RVA: 0x00205C49 File Offset: 0x00204049
		// (set) Token: 0x0600585D RID: 22621 RVA: 0x00205C51 File Offset: 0x00204051
		public string ProgramVersion
		{
			[CompilerGenerated]
			get
			{
				return this.<ProgramVersion>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<ProgramVersion>k__BackingField = value;
			}
		}

		// Token: 0x17000CF6 RID: 3318
		// (get) Token: 0x0600585E RID: 22622 RVA: 0x00205C5A File Offset: 0x0020405A
		// (set) Token: 0x0600585F RID: 22623 RVA: 0x00205C62 File Offset: 0x00204062
		public List<string> Contents
		{
			[CompilerGenerated]
			get
			{
				return this.<Contents>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<Contents>k__BackingField = value;
			}
		}

		// Token: 0x17000CF7 RID: 3319
		// (get) Token: 0x06005860 RID: 22624 RVA: 0x00205C6B File Offset: 0x0020406B
		// (set) Token: 0x06005861 RID: 22625 RVA: 0x00205C73 File Offset: 0x00204073
		public List<string> PackageDependencies
		{
			[CompilerGenerated]
			get
			{
				return this.<PackageDependencies>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<PackageDependencies>k__BackingField = value;
			}
		}

		// Token: 0x17000CF8 RID: 3320
		// (get) Token: 0x06005862 RID: 22626 RVA: 0x00205C7C File Offset: 0x0020407C
		// (set) Token: 0x06005863 RID: 22627 RVA: 0x00205C84 File Offset: 0x00204084
		public bool HasMissingDependencies
		{
			[CompilerGenerated]
			get
			{
				return this.<HasMissingDependencies>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<HasMissingDependencies>k__BackingField = value;
			}
		}

		// Token: 0x17000CF9 RID: 3321
		// (get) Token: 0x06005864 RID: 22628 RVA: 0x00205C8D File Offset: 0x0020408D
		// (set) Token: 0x06005865 RID: 22629 RVA: 0x00205C95 File Offset: 0x00204095
		public HashSet<string> PackageDependenciesMissing
		{
			[CompilerGenerated]
			get
			{
				return this.<PackageDependenciesMissing>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<PackageDependenciesMissing>k__BackingField = value;
			}
		}

		// Token: 0x17000CFA RID: 3322
		// (get) Token: 0x06005866 RID: 22630 RVA: 0x00205C9E File Offset: 0x0020409E
		// (set) Token: 0x06005867 RID: 22631 RVA: 0x00205CA6 File Offset: 0x002040A6
		public List<VarPackage> PackageDependenciesResolved
		{
			[CompilerGenerated]
			get
			{
				return this.<PackageDependenciesResolved>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<PackageDependenciesResolved>k__BackingField = value;
			}
		}

		// Token: 0x17000CFB RID: 3323
		// (get) Token: 0x06005868 RID: 22632 RVA: 0x00205CAF File Offset: 0x002040AF
		// (set) Token: 0x06005869 RID: 22633 RVA: 0x00205CB7 File Offset: 0x002040B7
		public bool HadReferenceIssues
		{
			[CompilerGenerated]
			get
			{
				return this.<HadReferenceIssues>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<HadReferenceIssues>k__BackingField = value;
			}
		}

		// Token: 0x0600586A RID: 22634 RVA: 0x00205CC0 File Offset: 0x002040C0
		public List<string> GetCustomOptionNames()
		{
			List<string> result;
			if (this.customOptions != null)
			{
				result = this.customOptions.Keys.ToList<string>();
			}
			else
			{
				result = new List<string>();
			}
			return result;
		}

		// Token: 0x0600586B RID: 22635 RVA: 0x00205CF8 File Offset: 0x002040F8
		public bool GetCustomOption(string optionName)
		{
			bool result = false;
			if (this.customOptions != null)
			{
				this.customOptions.TryGetValue(optionName, out result);
			}
			return result;
		}

		// Token: 0x0600586C RID: 22636 RVA: 0x00205D24 File Offset: 0x00204124
		public VarDirectoryEntry GetDirectoryEntry(string path)
		{
			VarDirectoryEntry result = null;
			if (this._DirectoryEntryLookup != null)
			{
				this._DirectoryEntryLookup.TryGetValue(path, out result);
			}
			return result;
		}

		// Token: 0x17000CFC RID: 3324
		// (get) Token: 0x0600586D RID: 22637 RVA: 0x00205D4E File Offset: 0x0020414E
		// (set) Token: 0x0600586E RID: 22638 RVA: 0x00205D56 File Offset: 0x00204156
		public VarDirectoryEntry RootDirectory
		{
			[CompilerGenerated]
			get
			{
				return this.<RootDirectory>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<RootDirectory>k__BackingField = value;
			}
		}

		// Token: 0x0600586F RID: 22639 RVA: 0x00205D60 File Offset: 0x00204160
		public bool HasMatchingDirectories(string dir)
		{
			string text = dir.Replace('\\', '/');
			text = Regex.Replace(text, "/$", string.Empty);
			foreach (VarDirectoryEntry varDirectoryEntry in this.DirectoryEntries)
			{
				if (varDirectoryEntry.InternalSlashPath == text)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005870 RID: 22640 RVA: 0x00205DEC File Offset: 0x002041EC
		public List<VarDirectoryEntry> FindVarDirectories(string dir, bool exactMatch = true)
		{
			string text = dir.Replace('\\', '/');
			text = Regex.Replace(text, "/$", string.Empty);
			List<VarDirectoryEntry> list = new List<VarDirectoryEntry>();
			foreach (VarDirectoryEntry varDirectoryEntry in this.DirectoryEntries)
			{
				if (exactMatch)
				{
					if (varDirectoryEntry.InternalSlashPath == text)
					{
						list.Add(varDirectoryEntry);
					}
				}
				else if (varDirectoryEntry.InternalSlashPath.StartsWith(dir))
				{
					list.Add(varDirectoryEntry);
				}
			}
			return list;
		}

		// Token: 0x06005871 RID: 22641 RVA: 0x00205EA0 File Offset: 0x002042A0
		public bool HasMatchingFiles(string dir, string pattern)
		{
			if (this.HasMatchingDirectories(dir))
			{
				string pattern2 = "^" + Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", ".") + "$";
				foreach (VarFileEntry varFileEntry in this.FileEntries)
				{
					if (varFileEntry.InternalSlashPath.StartsWith(dir))
					{
						if (Regex.IsMatch(varFileEntry.Name, pattern2))
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		// Token: 0x06005872 RID: 22642 RVA: 0x00205F68 File Offset: 0x00204368
		public void FindFiles(string dir, string pattern, List<FileEntry> foundFiles)
		{
			string pattern2 = "^" + Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", ".") + "$";
			foreach (VarFileEntry varFileEntry in this.FileEntries)
			{
				if (varFileEntry.InternalSlashPath.StartsWith(dir))
				{
					if (Regex.IsMatch(varFileEntry.Name, pattern2))
					{
						foundFiles.Add(varFileEntry);
					}
				}
			}
		}

		// Token: 0x17000CFD RID: 3325
		// (get) Token: 0x06005873 RID: 22643 RVA: 0x00206020 File Offset: 0x00204420
		public bool IsOnHub
		{
			get
			{
				if (HubBrowse.singleton != null && !SuperController.singleton.hubDisabled)
				{
					string packageHubResourceId = HubBrowse.singleton.GetPackageHubResourceId(this.Uid);
					if (packageHubResourceId != null)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x06005874 RID: 22644 RVA: 0x00206068 File Offset: 0x00204468
		public void OpenOnHub()
		{
			if (HubBrowse.singleton != null && !SuperController.singleton.hubDisabled)
			{
				string packageHubResourceId = HubBrowse.singleton.GetPackageHubResourceId(this.Uid);
				if (packageHubResourceId != null)
				{
					HubBrowse.singleton.OpenDetail(packageHubResourceId, false);
				}
			}
		}

		// Token: 0x06005875 RID: 22645 RVA: 0x002060B7 File Offset: 0x002044B7
		public void Dispose()
		{
			if (this.ZipFile != null)
			{
				this.ZipFile.Close();
				this.ZipFile = null;
			}
		}

		// Token: 0x06005876 RID: 22646 RVA: 0x002060D8 File Offset: 0x002044D8
		public JSONNode GetJSONCache(string filePath)
		{
			JSONNode result = null;
			if (this.jsonCache != null)
			{
				result = this.jsonCache[filePath];
			}
			return result;
		}

		// Token: 0x06005877 RID: 22647 RVA: 0x00206108 File Offset: 0x00204508
		protected void AddDirToCache(VarDirectoryEntry de, string pattern, JSONClass cache)
		{
			List<VarDirectoryEntry> varSubDirectories = de.VarSubDirectories;
			foreach (VarDirectoryEntry de2 in varSubDirectories)
			{
				this.AddDirToCache(de2, pattern, cache);
			}
			List<FileEntry> files = de.GetFiles(pattern);
			foreach (FileEntry fileEntry in files)
			{
				VarFileEntry varFileEntry = fileEntry as VarFileEntry;
				if (varFileEntry != null)
				{
					string aJSON = FileManager.ReadAllText(fileEntry);
					JSONNode jsonnode = JSON.Parse(aJSON);
					if (jsonnode != null)
					{
						cache[varFileEntry.InternalSlashPath] = jsonnode;
					}
				}
			}
		}

		// Token: 0x06005878 RID: 22648 RVA: 0x002061F0 File Offset: 0x002045F0
		public void SyncJSONCache()
		{
			if (CacheManager.CachingEnabled)
			{
				if (!this.IsSimulated)
				{
					try
					{
						bool flag = false;
						string str = CacheManager.GetPackageJSONCacheDir() + "/";
						string text = str + this.Uid + ".vamcachemeta";
						string path = str + this.Uid + ".vamcachejson";
						if (File.Exists(text))
						{
							string text2 = FileManager.ReadAllText(text, false);
							if (text2 != null)
							{
								try
								{
									JSONClass asObject = JSON.Parse(text2).AsObject;
									if (asObject != null)
									{
										string text3 = asObject["size"];
										long num;
										if (text3 != null && text3 != string.Empty && long.TryParse(text3, out num) && num == this.Size)
										{
											flag = true;
										}
									}
									else
									{
										UnityEngine.Debug.LogError("Invalid cache meta file " + text);
									}
								}
								catch (Exception arg)
								{
									SuperController.LogError("Exception during parse of package json cache (file will be regenerated to try to fix): " + arg);
								}
							}
						}
						if (!flag || !File.Exists(path))
						{
							JSONClass jsonclass = new JSONClass();
							jsonclass["type"] = "json";
							jsonclass["size"] = this.Size.ToString();
							jsonclass["date"] = this.LastWriteTime.ToString();
							File.WriteAllText(text, jsonclass.ToString(string.Empty));
							VarDirectoryEntry rootDirectory = this.RootDirectory;
							JSONClass jsonclass2 = new JSONClass();
							if (rootDirectory != null)
							{
								foreach (string pattern in this.cacheFilePatterns)
								{
									this.AddDirToCache(rootDirectory, pattern, jsonclass2);
								}
							}
							File.WriteAllText(path, jsonclass2.ToString(string.Empty));
							this.jsonCache = jsonclass2;
						}
						else if (this.jsonCache == null)
						{
							string aJSON = FileManager.ReadAllText(path, false);
							JSONClass asObject2 = JSON.Parse(aJSON).AsObject;
							this.jsonCache = asObject2;
						}
					}
					catch (Exception arg2)
					{
						SuperController.LogError("Exception during sync of package json cache: " + arg2);
						this.jsonCache = null;
					}
				}
			}
			else
			{
				this.jsonCache = null;
			}
		}

		// Token: 0x06005879 RID: 22649 RVA: 0x00206484 File Offset: 0x00204884
		protected void CreateDirectoryEntries(VarFileEntry varFileEntry)
		{
			string internalSlashPath = varFileEntry.InternalSlashPath;
			string[] array = internalSlashPath.Split(new char[]
			{
				'/'
			});
			VarDirectoryEntry varDirectoryEntry = this.RootDirectory;
			string text = string.Empty;
			for (int i = 0; i < array.Length; i++)
			{
				if (i == array.Length - 1)
				{
					varDirectoryEntry.AddFileEntry(varFileEntry);
				}
				else
				{
					if (text == string.Empty)
					{
						text += array[i];
					}
					else
					{
						text = text + "/" + array[i];
					}
					VarDirectoryEntry varDirectoryEntry2;
					if (!this._DirectoryEntryLookup.TryGetValue(text, out varDirectoryEntry2))
					{
						varDirectoryEntry2 = new VarDirectoryEntry(this, text, varDirectoryEntry);
						varDirectoryEntry.AddSubDirectory(varDirectoryEntry2);
						this.DirectoryEntries.Add(varDirectoryEntry2);
						this._DirectoryEntryLookup.Add(text, varDirectoryEntry2);
					}
					varDirectoryEntry = varDirectoryEntry2;
				}
			}
		}

		// Token: 0x0600587A RID: 22650 RVA: 0x0020655C File Offset: 0x0020495C
		protected void Scan()
		{
			this.FileEntries = new List<VarFileEntry>();
			this.DirectoryEntries = new List<VarDirectoryEntry>();
			this._DirectoryEntryLookup = new Dictionary<string, VarDirectoryEntry>();
			this._DirectoryEntryLookup.Add(string.Empty, this.RootDirectory);
			this.SyncEnabled();
			bool flag = false;
			if (File.Exists(this.Path))
			{
				this.IsSimulated = false;
				float elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
				ZipFile zipFile = null;
				try
				{
					FileInfo fileInfo = new FileInfo(this.Path);
					this.LastWriteTime = fileInfo.LastWriteTime;
					this.Size = fileInfo.Length;
					this.RootDirectory = new VarDirectoryEntry(this, string.Empty, null);
					this.DirectoryEntries.Add(this.RootDirectory);
					FileStream file = File.Open(this.Path, FileMode.Open, FileAccess.Read, FileShare.Read | FileShare.Write | FileShare.Delete);
					zipFile = new ZipFile(file);
					zipFile.IsStreamOwner = true;
					this.metaEntry = null;
					IEnumerator enumerator = zipFile.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							ZipEntry zipEntry = (ZipEntry)obj;
							if (zipEntry.IsFile)
							{
								VarFileEntry varFileEntry = new VarFileEntry(this, zipEntry.Name, zipEntry.DateTime, zipEntry.Size, false);
								this.FileEntries.Add(varFileEntry);
								this.CreateDirectoryEntries(varFileEntry);
								if (zipEntry.Name == "meta.json")
								{
									this.metaEntry = varFileEntry;
								}
							}
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
					if (this.metaEntry != null)
					{
						flag = true;
					}
					this.ZipFile = zipFile;
				}
				catch (Exception ex)
				{
					if (zipFile != null)
					{
						zipFile.Close();
					}
					SuperController.LogError(string.Concat(new object[]
					{
						"Exception during zip file scan of ",
						this.Path,
						": ",
						ex
					}));
				}
				float elapsedMilliseconds2 = GlobalStopwatch.GetElapsedMilliseconds();
				float num = elapsedMilliseconds2 - elapsedMilliseconds;
				if (FileManager.debug)
				{
					UnityEngine.Debug.Log(string.Concat(new string[]
					{
						"Scanned var package ",
						this.Path,
						" in ",
						num.ToString("F1"),
						" ms"
					}));
				}
			}
			else if (Directory.Exists(this.Path))
			{
				this.IsSimulated = true;
				float elapsedMilliseconds3 = GlobalStopwatch.GetElapsedMilliseconds();
				try
				{
					this.LastWriteTime = Directory.GetLastWriteTime(this.Path);
					this.Size = 0L;
					this.RootDirectory = new VarDirectoryEntry(this, string.Empty, null);
					this.DirectoryEntries.Add(this.RootDirectory);
					this.metaEntry = null;
					string[] files = Directory.GetFiles(this.Path, "*", SearchOption.AllDirectories);
					foreach (string text in files)
					{
						string text2 = text.Replace(this.Path + "\\", string.Empty);
						text2 = text2.Replace('\\', '/');
						FileInfo fileInfo2 = new FileInfo(text);
						VarFileEntry varFileEntry2 = new VarFileEntry(this, text2, fileInfo2.LastWriteTime, fileInfo2.Length, true);
						this.FileEntries.Add(varFileEntry2);
						this.CreateDirectoryEntries(varFileEntry2);
						if (text2 == "meta.json")
						{
							this.metaEntry = varFileEntry2;
						}
					}
					if (this.metaEntry != null)
					{
						flag = true;
					}
				}
				catch (Exception ex2)
				{
					UnityEngine.Debug.LogError(string.Concat(new object[]
					{
						"Exception during var directory scan of ",
						this.Path,
						": ",
						ex2
					}));
				}
				float elapsedMilliseconds4 = GlobalStopwatch.GetElapsedMilliseconds();
				float num2 = elapsedMilliseconds4 - elapsedMilliseconds3;
				if (FileManager.debug)
				{
					UnityEngine.Debug.Log(string.Concat(new string[]
					{
						"Scanned var package ",
						this.Path,
						" in ",
						num2.ToString("F1"),
						" ms"
					}));
				}
			}
			if (!flag)
			{
				this.invalid = true;
			}
			else
			{
				this.SyncJSONCache();
			}
		}

		// Token: 0x0600587B RID: 22651 RVA: 0x002069A0 File Offset: 0x00204DA0
		protected void FindMissingDependenciesRecursive(JSONClass jc)
		{
			JSONClass asObject = jc["dependencies"].AsObject;
			if (asObject != null)
			{
				foreach (string text in asObject.Keys)
				{
					if (FileManager.GetPackage(text) == null)
					{
						this.HasMissingDependencies = true;
						this.PackageDependenciesMissing.Add(text);
					}
					JSONClass asObject2 = asObject[text].AsObject;
					if (asObject2 != null)
					{
						this.FindMissingDependenciesRecursive(asObject2);
					}
				}
			}
		}

		// Token: 0x0600587C RID: 22652 RVA: 0x00206A54 File Offset: 0x00204E54
		public void LoadMetaData()
		{
			bool flag = false;
			List<string> list = new List<string>();
			if (this.metaEntry != null)
			{
				try
				{
					using (VarFileEntryStreamReader varFileEntryStreamReader = new VarFileEntryStreamReader(this.metaEntry))
					{
						string aJSON = varFileEntryStreamReader.ReadToEnd();
						JSONClass asObject = JSON.Parse(aJSON).AsObject;
						if (asObject != null)
						{
							if (asObject["licenseType"] != null)
							{
								this.LicenseType = asObject["licenseType"];
							}
							else
							{
								this.LicenseType = "MISSING";
							}
							this.SecondaryLicenseType = asObject["secondaryLicenseType"];
							this.EAEndYear = asObject["EAEndYear"];
							this.EAEndMonth = asObject["EAEndMonth"];
							this.EAEndDay = asObject["EAEndDay"];
							if (asObject["standardReferenceVersionOption"] != null)
							{
								try
								{
									string value = asObject["standardReferenceVersionOption"];
									VarPackage.ReferenceVersionOption standardReferenceVersionOption = (VarPackage.ReferenceVersionOption)Enum.Parse(typeof(VarPackage.ReferenceVersionOption), value);
									this.StandardReferenceVersionOption = standardReferenceVersionOption;
								}
								catch (ArgumentException)
								{
									this.StandardReferenceVersionOption = VarPackage.ReferenceVersionOption.Latest;
								}
							}
							else
							{
								this.StandardReferenceVersionOption = VarPackage.ReferenceVersionOption.Latest;
							}
							if (asObject["scriptReferenceVersionOption"] != null)
							{
								try
								{
									string value2 = asObject["scriptReferenceVersionOption"];
									VarPackage.ReferenceVersionOption scriptReferenceVersionOption = (VarPackage.ReferenceVersionOption)Enum.Parse(typeof(VarPackage.ReferenceVersionOption), value2);
									this.ScriptReferenceVersionOption = scriptReferenceVersionOption;
								}
								catch (ArgumentException)
								{
									this.ScriptReferenceVersionOption = VarPackage.ReferenceVersionOption.Exact;
								}
							}
							else
							{
								this.ScriptReferenceVersionOption = VarPackage.ReferenceVersionOption.Exact;
							}
							this.Description = asObject["description"];
							this.Credits = asObject["credits"];
							this.Instructions = asObject["instructions"];
							if (asObject["promotionalLink"] != null)
							{
								this.PromotionalLink = asObject["promotionalLink"];
							}
							else
							{
								this.PromotionalLink = asObject["patreonLink"];
							}
							List<string> list2 = new List<string>();
							JSONArray asArray = asObject["contentList"].AsArray;
							if (asArray != null)
							{
								IEnumerator enumerator = asArray.GetEnumerator();
								try
								{
									while (enumerator.MoveNext())
									{
										object obj = enumerator.Current;
										JSONNode d = (JSONNode)obj;
										string text = d;
										if (text != null)
										{
											list2.Add(text);
										}
									}
								}
								finally
								{
									IDisposable disposable;
									if ((disposable = (enumerator as IDisposable)) != null)
									{
										disposable.Dispose();
									}
								}
							}
							this.Contents = list2;
							this.PackageDependencies = new List<string>();
							this.PackageDependenciesMissing = new HashSet<string>();
							this.HasMissingDependencies = false;
							this.PackageDependenciesResolved = new List<VarPackage>();
							JSONClass asObject2 = asObject["dependencies"].AsObject;
							if (asObject2 != null)
							{
								foreach (string text2 in asObject2.Keys)
								{
									VarPackage package = FileManager.GetPackage(text2);
									if (package == null)
									{
										string packageGroupUid = Regex.Replace(text2, "\\.[0-9]+$", string.Empty);
										VarPackageGroup packageGroup = FileManager.GetPackageGroup(packageGroupUid);
										if (packageGroup != null)
										{
											VarPackage newestPackage = packageGroup.NewestPackage;
											this.PackageDependenciesResolved.Add(newestPackage);
										}
										else if (this.Enabled)
										{
											list.Add(string.Concat(new string[]
											{
												"Missing addon package ",
												text2,
												" that package",
												this.Uid,
												" depends on"
											}));
										}
									}
									else
									{
										this.PackageDependenciesResolved.Add(package);
									}
									this.PackageDependencies.Add(text2);
								}
							}
							if (this.Enabled)
							{
								this.FindMissingDependenciesRecursive(asObject);
							}
							JSONClass asObject3 = asObject["customOptions"].AsObject;
							this.customOptions = new Dictionary<string, bool>();
							if (asObject3 != null)
							{
								foreach (string text3 in asObject3.Keys)
								{
									if (!this.customOptions.ContainsKey(text3))
									{
										this.customOptions.Add(text3, asObject3[text3].AsBool);
									}
								}
							}
							if (asObject["hadReferenceIssues"] != null)
							{
								this.HadReferenceIssues = asObject["hadReferenceIssues"].AsBool;
							}
							else
							{
								this.HadReferenceIssues = false;
							}
							flag = true;
						}
					}
				}
				catch (Exception ex)
				{
					SuperController.LogError(string.Concat(new object[]
					{
						"Exception during process of meta.json from package ",
						this.Uid,
						": ",
						ex
					}));
				}
			}
			if (flag)
			{
				this.LoadUserPrefs();
				if (!this.IgnoreMissingDependencyErrors)
				{
					foreach (string err in list)
					{
						SuperController.LogError(err);
					}
				}
			}
			else
			{
				this.invalid = true;
			}
		}

		// Token: 0x040048A9 RID: 18601
		protected bool _enabled;

		// Token: 0x040048AA RID: 18602
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool <invalid>k__BackingField;

		// Token: 0x040048AB RID: 18603
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool <forceRefresh>k__BackingField;

		// Token: 0x040048AC RID: 18604
		protected bool _pluginsAlwaysEnabled;

		// Token: 0x040048AD RID: 18605
		protected bool _pluginsAlwaysDisabled;

		// Token: 0x040048AE RID: 18606
		protected bool _ignoreMissingDependencyErrors;

		// Token: 0x040048AF RID: 18607
		protected Thread unpackThread;

		// Token: 0x040048B0 RID: 18608
		protected Thread repackThread;

		// Token: 0x040048B1 RID: 18609
		protected int packFileProgressCount;

		// Token: 0x040048B2 RID: 18610
		protected int packFileTotalCount;

		// Token: 0x040048B3 RID: 18611
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private float <packProgress>k__BackingField;

		// Token: 0x040048B4 RID: 18612
		public bool packThreadAbort;

		// Token: 0x040048B5 RID: 18613
		public string packThreadError;

		// Token: 0x040048B6 RID: 18614
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool <IsSimulated>k__BackingField;

		// Token: 0x040048B7 RID: 18615
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private ZipFile <ZipFile>k__BackingField;

		// Token: 0x040048B8 RID: 18616
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <Uid>k__BackingField;

		// Token: 0x040048B9 RID: 18617
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <UidLowerInvariant>k__BackingField;

		// Token: 0x040048BA RID: 18618
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <Path>k__BackingField;

		// Token: 0x040048BB RID: 18619
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <SlashPath>k__BackingField;

		// Token: 0x040048BC RID: 18620
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <FullPath>k__BackingField;

		// Token: 0x040048BD RID: 18621
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <FullSlashPath>k__BackingField;

		// Token: 0x040048BE RID: 18622
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private VarPackageGroup <Group>k__BackingField;

		// Token: 0x040048BF RID: 18623
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <GroupName>k__BackingField;

		// Token: 0x040048C0 RID: 18624
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <Creator>k__BackingField;

		// Token: 0x040048C1 RID: 18625
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <Name>k__BackingField;

		// Token: 0x040048C2 RID: 18626
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private int <Version>k__BackingField;

		// Token: 0x040048C3 RID: 18627
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private VarPackage.ReferenceVersionOption <StandardReferenceVersionOption>k__BackingField;

		// Token: 0x040048C4 RID: 18628
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private VarPackage.ReferenceVersionOption <ScriptReferenceVersionOption>k__BackingField;

		// Token: 0x040048C5 RID: 18629
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private DateTime <LastWriteTime>k__BackingField;

		// Token: 0x040048C6 RID: 18630
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private long <Size>k__BackingField;

		// Token: 0x040048C7 RID: 18631
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private List<VarFileEntry> <FileEntries>k__BackingField;

		// Token: 0x040048C8 RID: 18632
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private List<VarDirectoryEntry> <DirectoryEntries>k__BackingField;

		// Token: 0x040048C9 RID: 18633
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <LicenseType>k__BackingField;

		// Token: 0x040048CA RID: 18634
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <SecondaryLicenseType>k__BackingField;

		// Token: 0x040048CB RID: 18635
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <EAEndYear>k__BackingField;

		// Token: 0x040048CC RID: 18636
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <EAEndMonth>k__BackingField;

		// Token: 0x040048CD RID: 18637
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <EAEndDay>k__BackingField;

		// Token: 0x040048CE RID: 18638
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <Description>k__BackingField;

		// Token: 0x040048CF RID: 18639
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <Credits>k__BackingField;

		// Token: 0x040048D0 RID: 18640
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <Instructions>k__BackingField;

		// Token: 0x040048D1 RID: 18641
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <PromotionalLink>k__BackingField;

		// Token: 0x040048D2 RID: 18642
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <ProgramVersion>k__BackingField;

		// Token: 0x040048D3 RID: 18643
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private List<string> <Contents>k__BackingField;

		// Token: 0x040048D4 RID: 18644
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private List<string> <PackageDependencies>k__BackingField;

		// Token: 0x040048D5 RID: 18645
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool <HasMissingDependencies>k__BackingField;

		// Token: 0x040048D6 RID: 18646
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private HashSet<string> <PackageDependenciesMissing>k__BackingField;

		// Token: 0x040048D7 RID: 18647
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private List<VarPackage> <PackageDependenciesResolved>k__BackingField;

		// Token: 0x040048D8 RID: 18648
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool <HadReferenceIssues>k__BackingField;

		// Token: 0x040048D9 RID: 18649
		protected Dictionary<string, bool> customOptions;

		// Token: 0x040048DA RID: 18650
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private VarDirectoryEntry <RootDirectory>k__BackingField;

		// Token: 0x040048DB RID: 18651
		protected Dictionary<string, VarDirectoryEntry> _DirectoryEntryLookup;

		// Token: 0x040048DC RID: 18652
		public bool isNewestVersion;

		// Token: 0x040048DD RID: 18653
		public bool isNewestEnabledVersion;

		// Token: 0x040048DE RID: 18654
		protected string[] cacheFilePatterns = new string[]
		{
			"*.vmi",
			"*.vam"
		};

		// Token: 0x040048DF RID: 18655
		protected JSONClass jsonCache;

		// Token: 0x040048E0 RID: 18656
		protected VarFileEntry metaEntry;

		// Token: 0x02000BF8 RID: 3064
		public enum ReferenceVersionOption
		{
			// Token: 0x040048E2 RID: 18658
			Latest,
			// Token: 0x040048E3 RID: 18659
			Minimum,
			// Token: 0x040048E4 RID: 18660
			Exact
		}
	}
}
