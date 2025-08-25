using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading;
using ICSharpCode.SharpZipLib.Core;
using MVR.FileManagementSecure;
using UnityEngine;
using UnityEngine.Events;

namespace MVR.FileManagement
{
	// Token: 0x02000BDD RID: 3037
	public class FileManager : MonoBehaviour
	{
		// Token: 0x06005659 RID: 22105 RVA: 0x001F7D5E File Offset: 0x001F615E
		public FileManager()
		{
		}

		// Token: 0x0600565A RID: 22106 RVA: 0x001F7D68 File Offset: 0x001F6168
		protected static string packagePathToUid(string vpath)
		{
			string input = vpath.Replace('\\', '/');
			input = Regex.Replace(input, "\\.(var|zip)$", string.Empty);
			return Regex.Replace(input, ".*/", string.Empty);
		}

		// Token: 0x0600565B RID: 22107 RVA: 0x001F7DA4 File Offset: 0x001F61A4
		protected static VarPackage RegisterPackage(string vpath)
		{
			if (FileManager.debug)
			{
				UnityEngine.Debug.Log("RegisterPackage " + vpath);
			}
			string text = FileManager.packagePathToUid(vpath);
			string[] array = text.Split(new char[]
			{
				'.'
			});
			if (array.Length == 3)
			{
				string text2 = array[0];
				string text3 = array[1];
				string text4 = text2 + "." + text3;
				string s = array[2];
				try
				{
					int version = int.Parse(s);
					if (!FileManager.packagesByUid.ContainsKey(text))
					{
						VarPackageGroup varPackageGroup;
						if (!FileManager.packageGroups.TryGetValue(text4, out varPackageGroup))
						{
							varPackageGroup = new VarPackageGroup(text4);
							FileManager.packageGroups.Add(text4, varPackageGroup);
						}
						VarPackage varPackage = new VarPackage(text, vpath, varPackageGroup, text2, text3, version);
						FileManager.packagesByUid.Add(text, varPackage);
						FileManager.packagesByPath.Add(varPackage.Path, varPackage);
						FileManager.packagesByPath.Add(varPackage.SlashPath, varPackage);
						FileManager.packagesByPath.Add(varPackage.FullPath, varPackage);
						FileManager.packagesByPath.Add(varPackage.FullSlashPath, varPackage);
						varPackageGroup.AddPackage(varPackage);
						if (varPackage.Enabled)
						{
							FileManager.enabledPackages.Add(varPackage);
							foreach (VarFileEntry varFileEntry in varPackage.FileEntries)
							{
								FileManager.allVarFileEntries.Add(varFileEntry);
								FileManager.uidToVarFileEntry.Add(varFileEntry.Uid, varFileEntry);
								if (FileManager.debug)
								{
									UnityEngine.Debug.Log("Add var file with UID " + varFileEntry.Uid);
								}
								FileManager.pathToVarFileEntry.Add(varFileEntry.Path, varFileEntry);
								FileManager.pathToVarFileEntry.Add(varFileEntry.SlashPath, varFileEntry);
								FileManager.pathToVarFileEntry.Add(varFileEntry.FullPath, varFileEntry);
								FileManager.pathToVarFileEntry.Add(varFileEntry.FullSlashPath, varFileEntry);
							}
							foreach (VarDirectoryEntry varDirectoryEntry in varPackage.DirectoryEntries)
							{
								FileManager.allVarDirectoryEntries.Add(varDirectoryEntry);
								if (FileManager.debug)
								{
									UnityEngine.Debug.Log("Add var directory with UID " + varDirectoryEntry.Uid);
								}
								FileManager.uidToVarDirectoryEntry.Add(varDirectoryEntry.Uid, varDirectoryEntry);
								FileManager.pathToVarDirectoryEntry.Add(varDirectoryEntry.Path, varDirectoryEntry);
								FileManager.pathToVarDirectoryEntry.Add(varDirectoryEntry.SlashPath, varDirectoryEntry);
								FileManager.pathToVarDirectoryEntry.Add(varDirectoryEntry.FullPath, varDirectoryEntry);
								FileManager.pathToVarDirectoryEntry.Add(varDirectoryEntry.FullSlashPath, varDirectoryEntry);
							}
							FileManager.varPackagePathToRootVarDirectory.Add(varPackage.Path, varPackage.RootDirectory);
							FileManager.varPackagePathToRootVarDirectory.Add(varPackage.FullPath, varPackage.RootDirectory);
						}
						return varPackage;
					}
					SuperController.LogError("Duplicate package uid " + text + ". Cannot register");
				}
				catch (FormatException)
				{
					SuperController.LogError("VAR file " + vpath + " does not use integer version field in name <creator>.<name>.<version>");
				}
			}
			else
			{
				SuperController.LogError("VAR file " + vpath + " is not named with convention <creator>.<name>.<version>");
			}
			return null;
		}

		// Token: 0x0600565C RID: 22108 RVA: 0x001F8140 File Offset: 0x001F6540
		public static void UnregisterPackage(VarPackage vp)
		{
			if (vp != null)
			{
				if (vp.Group != null)
				{
					vp.Group.RemovePackage(vp);
				}
				FileManager.packagesByUid.Remove(vp.Uid);
				FileManager.packagesByPath.Remove(vp.Path);
				FileManager.packagesByPath.Remove(vp.SlashPath);
				FileManager.packagesByPath.Remove(vp.FullPath);
				FileManager.packagesByPath.Remove(vp.FullSlashPath);
				FileManager.enabledPackages.Remove(vp);
				foreach (VarFileEntry varFileEntry in vp.FileEntries)
				{
					FileManager.allVarFileEntries.Remove(varFileEntry);
					FileManager.uidToVarFileEntry.Remove(varFileEntry.Uid);
					FileManager.pathToVarFileEntry.Remove(varFileEntry.Path);
					FileManager.pathToVarFileEntry.Remove(varFileEntry.SlashPath);
					FileManager.pathToVarFileEntry.Remove(varFileEntry.FullPath);
					FileManager.pathToVarFileEntry.Remove(varFileEntry.FullSlashPath);
				}
				foreach (VarDirectoryEntry varDirectoryEntry in vp.DirectoryEntries)
				{
					FileManager.allVarDirectoryEntries.Remove(varDirectoryEntry);
					FileManager.uidToVarDirectoryEntry.Remove(varDirectoryEntry.Uid);
					FileManager.pathToVarDirectoryEntry.Remove(varDirectoryEntry.Path);
					FileManager.pathToVarDirectoryEntry.Remove(varDirectoryEntry.SlashPath);
					FileManager.pathToVarDirectoryEntry.Remove(varDirectoryEntry.FullPath);
					FileManager.pathToVarDirectoryEntry.Remove(varDirectoryEntry.FullSlashPath);
				}
				FileManager.varPackagePathToRootVarDirectory.Remove(vp.Path);
				FileManager.varPackagePathToRootVarDirectory.Remove(vp.FullPath);
				vp.Dispose();
			}
		}

		// Token: 0x17000C9D RID: 3229
		// (get) Token: 0x0600565D RID: 22109 RVA: 0x001F8344 File Offset: 0x001F6744
		public static string PackageFolder
		{
			get
			{
				return FileManager.packageFolder;
			}
		}

		// Token: 0x17000C9E RID: 3230
		// (get) Token: 0x0600565E RID: 22110 RVA: 0x001F834B File Offset: 0x001F674B
		public static string UserPrefsFolder
		{
			get
			{
				return FileManager.userPrefsFolder;
			}
		}

		// Token: 0x0600565F RID: 22111 RVA: 0x001F8354 File Offset: 0x001F6754
		public static void SyncJSONCache()
		{
			foreach (VarPackage varPackage in FileManager.GetPackages())
			{
				varPackage.SyncJSONCache();
			}
		}

		// Token: 0x06005660 RID: 22112 RVA: 0x001F83B0 File Offset: 0x001F67B0
		public static void RegisterRefreshHandler(OnRefresh refreshHandler)
		{
			FileManager.onRefreshHandlers = (OnRefresh)Delegate.Combine(FileManager.onRefreshHandlers, refreshHandler);
		}

		// Token: 0x06005661 RID: 22113 RVA: 0x001F83C7 File Offset: 0x001F67C7
		public static void UnregisterRefreshHandler(OnRefresh refreshHandler)
		{
			FileManager.onRefreshHandlers = (OnRefresh)Delegate.Remove(FileManager.onRefreshHandlers, refreshHandler);
		}

		// Token: 0x17000C9F RID: 3231
		// (get) Token: 0x06005662 RID: 22114 RVA: 0x001F83DE File Offset: 0x001F67DE
		// (set) Token: 0x06005663 RID: 22115 RVA: 0x001F83E5 File Offset: 0x001F67E5
		public static DateTime lastPackageRefreshTime
		{
			[CompilerGenerated]
			get
			{
				return FileManager.<lastPackageRefreshTime>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				FileManager.<lastPackageRefreshTime>k__BackingField = value;
			}
		}

		// Token: 0x06005664 RID: 22116 RVA: 0x001F83F0 File Offset: 0x001F67F0
		protected static void ClearAll()
		{
			foreach (VarPackage varPackage in FileManager.packagesByUid.Values)
			{
				varPackage.Dispose();
			}
			if (FileManager.packagesByUid != null)
			{
				FileManager.packagesByUid.Clear();
			}
			if (FileManager.packagesByPath != null)
			{
				FileManager.packagesByPath.Clear();
			}
			if (FileManager.packageGroups != null)
			{
				FileManager.packageGroups.Clear();
			}
			if (FileManager.enabledPackages != null)
			{
				FileManager.enabledPackages.Clear();
			}
			if (FileManager.allVarFileEntries != null)
			{
				FileManager.allVarFileEntries.Clear();
			}
			if (FileManager.allVarDirectoryEntries != null)
			{
				FileManager.allVarDirectoryEntries.Clear();
			}
			if (FileManager.uidToVarFileEntry != null)
			{
				FileManager.uidToVarFileEntry.Clear();
			}
			if (FileManager.pathToVarFileEntry != null)
			{
				FileManager.pathToVarFileEntry.Clear();
			}
			if (FileManager.uidToVarDirectoryEntry != null)
			{
				FileManager.uidToVarDirectoryEntry.Clear();
			}
			if (FileManager.pathToVarDirectoryEntry != null)
			{
				FileManager.pathToVarDirectoryEntry.Clear();
			}
			if (FileManager.varPackagePathToRootVarDirectory != null)
			{
				FileManager.varPackagePathToRootVarDirectory.Clear();
			}
		}

		// Token: 0x06005665 RID: 22117 RVA: 0x001F852C File Offset: 0x001F692C
		public static void Refresh()
		{
			if (FileManager.debug)
			{
				UnityEngine.Debug.Log("FileManager Refresh()");
			}
			if (FileManager.packagesByUid == null)
			{
				FileManager.packagesByUid = new Dictionary<string, VarPackage>();
			}
			if (FileManager.packagesByPath == null)
			{
				FileManager.packagesByPath = new Dictionary<string, VarPackage>();
			}
			if (FileManager.packageGroups == null)
			{
				FileManager.packageGroups = new Dictionary<string, VarPackageGroup>();
			}
			if (FileManager.enabledPackages == null)
			{
				FileManager.enabledPackages = new HashSet<VarPackage>();
			}
			if (FileManager.allVarFileEntries == null)
			{
				FileManager.allVarFileEntries = new HashSet<VarFileEntry>();
			}
			if (FileManager.allVarDirectoryEntries == null)
			{
				FileManager.allVarDirectoryEntries = new HashSet<VarDirectoryEntry>();
			}
			if (FileManager.uidToVarFileEntry == null)
			{
				FileManager.uidToVarFileEntry = new Dictionary<string, VarFileEntry>();
			}
			if (FileManager.pathToVarFileEntry == null)
			{
				FileManager.pathToVarFileEntry = new Dictionary<string, VarFileEntry>();
			}
			if (FileManager.uidToVarDirectoryEntry == null)
			{
				FileManager.uidToVarDirectoryEntry = new Dictionary<string, VarDirectoryEntry>();
			}
			if (FileManager.pathToVarDirectoryEntry == null)
			{
				FileManager.pathToVarDirectoryEntry = new Dictionary<string, VarDirectoryEntry>();
			}
			if (FileManager.varPackagePathToRootVarDirectory == null)
			{
				FileManager.varPackagePathToRootVarDirectory = new Dictionary<string, VarDirectoryEntry>();
			}
			bool flag = false;
			float num = GlobalStopwatch.GetElapsedMilliseconds();
			float elapsedMilliseconds;
			try
			{
				if (!Directory.Exists(FileManager.packageFolder))
				{
					FileManager.CreateDirectory(FileManager.packageFolder);
				}
				if (!Directory.Exists(FileManager.userPrefsFolder))
				{
					FileManager.CreateDirectory(FileManager.userPrefsFolder);
				}
				if (Directory.Exists(FileManager.packageFolder))
				{
					string[] array = null;
					string[] array2 = null;
					if (FileManager.packagesEnabled)
					{
						array = Directory.GetDirectories(FileManager.packageFolder, "*.var", SearchOption.AllDirectories);
						array2 = Directory.GetFiles(FileManager.packageFolder, "*.var", SearchOption.AllDirectories);
					}
					else if (FileManager.demoPackagePrefixes != null)
					{
						List<string> list = new List<string>();
						foreach (string str in FileManager.demoPackagePrefixes)
						{
							foreach (string item in Directory.GetFiles(FileManager.packageFolder, str + "*.var", SearchOption.AllDirectories))
							{
								list.Add(item);
							}
						}
						array2 = list.ToArray();
					}
					HashSet<string> hashSet = new HashSet<string>();
					HashSet<string> hashSet2 = new HashSet<string>();
					if (array != null)
					{
						foreach (string text in array)
						{
							hashSet.Add(text);
							VarPackage varPackage;
							if (FileManager.packagesByPath.TryGetValue(text, out varPackage))
							{
								bool flag2 = FileManager.enabledPackages.Contains(varPackage);
								bool enabled = varPackage.Enabled;
								if ((!flag2 && enabled) || (flag2 && !enabled) || !varPackage.IsSimulated)
								{
									FileManager.UnregisterPackage(varPackage);
									hashSet2.Add(text);
								}
							}
							else
							{
								hashSet2.Add(text);
							}
						}
					}
					if (array2 != null)
					{
						foreach (string text2 in array2)
						{
							hashSet.Add(text2);
							VarPackage varPackage2;
							if (FileManager.packagesByPath.TryGetValue(text2, out varPackage2))
							{
								bool flag3 = FileManager.enabledPackages.Contains(varPackage2);
								bool enabled2 = varPackage2.Enabled;
								if ((!flag3 && enabled2) || (flag3 && !enabled2) || varPackage2.IsSimulated)
								{
									FileManager.UnregisterPackage(varPackage2);
									hashSet2.Add(text2);
								}
							}
							else
							{
								hashSet2.Add(text2);
							}
						}
					}
					HashSet<VarPackage> hashSet3 = new HashSet<VarPackage>();
					foreach (VarPackage varPackage3 in FileManager.packagesByUid.Values)
					{
						if (!hashSet.Contains(varPackage3.Path))
						{
							hashSet3.Add(varPackage3);
						}
					}
					foreach (VarPackage vp in hashSet3)
					{
						FileManager.UnregisterPackage(vp);
						flag = true;
					}
					foreach (string vpath in hashSet2)
					{
						FileManager.RegisterPackage(vpath);
						flag = true;
					}
				}
				if (flag)
				{
					foreach (VarPackage varPackage4 in FileManager.packagesByUid.Values)
					{
						varPackage4.LoadMetaData();
					}
					foreach (VarPackageGroup varPackageGroup in FileManager.packageGroups.Values)
					{
						varPackageGroup.Init();
					}
				}
				elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
				float num2 = elapsedMilliseconds - num;
				UnityEngine.Debug.Log(string.Concat(new object[]
				{
					"Scanned ",
					FileManager.packagesByUid.Count,
					" packages in ",
					num2.ToString("F1"),
					" ms"
				}));
				num = elapsedMilliseconds;
				foreach (VarPackage varPackage5 in FileManager.packagesByUid.Values)
				{
					if (varPackage5.forceRefresh)
					{
						UnityEngine.Debug.Log("Force refresh of package " + varPackage5.Uid);
						flag = true;
						varPackage5.forceRefresh = false;
					}
				}
				if (flag)
				{
					UnityEngine.Debug.Log("Package changes detected");
					if (FileManager.onRefreshHandlers != null)
					{
						FileManager.onRefreshHandlers();
					}
				}
				else
				{
					UnityEngine.Debug.Log("No package changes detected");
				}
				elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
				UnityEngine.Debug.Log("Refresh Handlers took " + (elapsedMilliseconds - num).ToString("F1") + " ms");
				num = elapsedMilliseconds;
			}
			catch (Exception arg)
			{
				SuperController.LogError("Exception during package refresh " + arg);
			}
			elapsedMilliseconds = GlobalStopwatch.GetElapsedMilliseconds();
			UnityEngine.Debug.Log("Refresh package handlers took " + (elapsedMilliseconds - num).ToString("F1") + " ms");
			FileManager.lastPackageRefreshTime = DateTime.Now;
		}

		// Token: 0x06005666 RID: 22118 RVA: 0x001F8C18 File Offset: 0x001F7018
		public static void RegisterRestrictedReadPath(string path)
		{
			if (FileManager.restrictedReadPaths == null)
			{
				FileManager.restrictedReadPaths = new HashSet<string>();
			}
			FileManager.restrictedReadPaths.Add(Path.GetFullPath(path));
		}

		// Token: 0x06005667 RID: 22119 RVA: 0x001F8C3F File Offset: 0x001F703F
		public static void RegisterSecureReadPath(string path)
		{
			if (FileManager.secureReadPaths == null)
			{
				FileManager.secureReadPaths = new HashSet<string>();
			}
			FileManager.secureReadPaths.Add(Path.GetFullPath(path));
		}

		// Token: 0x06005668 RID: 22120 RVA: 0x001F8C66 File Offset: 0x001F7066
		public static void ClearSecureReadPaths()
		{
			if (FileManager.secureReadPaths == null)
			{
				FileManager.secureReadPaths = new HashSet<string>();
			}
			else
			{
				FileManager.secureReadPaths.Clear();
			}
		}

		// Token: 0x06005669 RID: 22121 RVA: 0x001F8C8C File Offset: 0x001F708C
		public static bool IsSecureReadPath(string path)
		{
			if (FileManager.secureReadPaths == null)
			{
				FileManager.secureReadPaths = new HashSet<string>();
			}
			string fullPath = FileManager.GetFullPath(path);
			bool flag = false;
			foreach (string value in FileManager.restrictedReadPaths)
			{
				if (fullPath.StartsWith(value))
				{
					flag = true;
					break;
				}
			}
			bool result = false;
			if (!flag)
			{
				foreach (string value2 in FileManager.secureReadPaths)
				{
					if (fullPath.StartsWith(value2))
					{
						result = true;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x0600566A RID: 22122 RVA: 0x001F8D78 File Offset: 0x001F7178
		public static void ClearSecureWritePaths()
		{
			if (FileManager.secureInternalWritePaths == null)
			{
				FileManager.secureInternalWritePaths = new HashSet<string>();
			}
			else
			{
				FileManager.secureInternalWritePaths.Clear();
			}
			if (FileManager.securePluginWritePaths == null)
			{
				FileManager.securePluginWritePaths = new HashSet<string>();
			}
			else
			{
				FileManager.securePluginWritePaths.Clear();
			}
			if (FileManager.pluginWritePathsThatDoNotNeedConfirm == null)
			{
				FileManager.pluginWritePathsThatDoNotNeedConfirm = new HashSet<string>();
			}
			else
			{
				FileManager.pluginWritePathsThatDoNotNeedConfirm.Clear();
			}
		}

		// Token: 0x0600566B RID: 22123 RVA: 0x001F8DEE File Offset: 0x001F71EE
		public static void RegisterInternalSecureWritePath(string path)
		{
			if (FileManager.secureInternalWritePaths == null)
			{
				FileManager.secureInternalWritePaths = new HashSet<string>();
			}
			FileManager.secureInternalWritePaths.Add(Path.GetFullPath(path));
		}

		// Token: 0x0600566C RID: 22124 RVA: 0x001F8E18 File Offset: 0x001F7218
		public static void RegisterPluginSecureWritePath(string path, bool doesNotNeedConfirm)
		{
			if (FileManager.securePluginWritePaths == null)
			{
				FileManager.securePluginWritePaths = new HashSet<string>();
			}
			if (FileManager.pluginWritePathsThatDoNotNeedConfirm == null)
			{
				FileManager.pluginWritePathsThatDoNotNeedConfirm = new HashSet<string>();
			}
			string fullPath = Path.GetFullPath(path);
			FileManager.securePluginWritePaths.Add(fullPath);
			if (doesNotNeedConfirm)
			{
				FileManager.pluginWritePathsThatDoNotNeedConfirm.Add(fullPath);
			}
		}

		// Token: 0x0600566D RID: 22125 RVA: 0x001F8E74 File Offset: 0x001F7274
		public static bool IsSecureWritePath(string path)
		{
			if (FileManager.secureInternalWritePaths == null)
			{
				FileManager.secureInternalWritePaths = new HashSet<string>();
			}
			string fullPath = FileManager.GetFullPath(path);
			bool result = false;
			foreach (string value in FileManager.secureInternalWritePaths)
			{
				if (fullPath.StartsWith(value))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x0600566E RID: 22126 RVA: 0x001F8EFC File Offset: 0x001F72FC
		public static bool IsSecurePluginWritePath(string path)
		{
			if (FileManager.securePluginWritePaths == null)
			{
				FileManager.securePluginWritePaths = new HashSet<string>();
			}
			string fullPath = FileManager.GetFullPath(path);
			bool result = false;
			foreach (string value in FileManager.securePluginWritePaths)
			{
				if (fullPath.StartsWith(value))
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x0600566F RID: 22127 RVA: 0x001F8F84 File Offset: 0x001F7384
		public static bool IsPluginWritePathThatNeedsConfirm(string path)
		{
			if (FileManager.pluginWritePathsThatDoNotNeedConfirm == null)
			{
				FileManager.pluginWritePathsThatDoNotNeedConfirm = new HashSet<string>();
			}
			string fullPath = FileManager.GetFullPath(path);
			bool result = true;
			foreach (string value in FileManager.pluginWritePathsThatDoNotNeedConfirm)
			{
				if (fullPath.StartsWith(value))
				{
					result = false;
					break;
				}
			}
			return result;
		}

		// Token: 0x06005670 RID: 22128 RVA: 0x001F900C File Offset: 0x001F740C
		public static void RegisterPluginHashToPluginPath(string hash, string path)
		{
			if (FileManager.pluginHashToPluginPath == null)
			{
				FileManager.pluginHashToPluginPath = new Dictionary<string, string>();
			}
			FileManager.pluginHashToPluginPath.Remove(hash);
			FileManager.pluginHashToPluginPath.Add(hash, path);
		}

		// Token: 0x06005671 RID: 22129 RVA: 0x001F903C File Offset: 0x001F743C
		protected static string GetPluginHash()
		{
			StackTrace stackTrace = new StackTrace();
			string result = null;
			for (int i = 0; i < stackTrace.FrameCount; i++)
			{
				StackFrame frame = stackTrace.GetFrame(i);
				MethodBase method = frame.GetMethod();
				AssemblyName name = method.DeclaringType.Assembly.GetName();
				string name2 = name.Name;
				if (name2.StartsWith("MVRPlugin_"))
				{
					result = Regex.Replace(name2, "_[0-9]+$", string.Empty);
					break;
				}
			}
			return result;
		}

		// Token: 0x06005672 RID: 22130 RVA: 0x001F90C0 File Offset: 0x001F74C0
		public static void AssertNotCalledFromPlugin()
		{
			string pluginHash = FileManager.GetPluginHash();
			if (pluginHash != null)
			{
				throw new Exception("Plugin with signature " + pluginHash + " tried to execute forbidden operation");
			}
		}

		// Token: 0x06005673 RID: 22131 RVA: 0x001F90F0 File Offset: 0x001F74F0
		protected void DestroyUserConfirmPanel(UserConfirmPanel ucp)
		{
			UnityEngine.Object.Destroy(ucp.gameObject);
			this.activeUserConfirmPanels.Remove(ucp);
			if (this.activeUserConfirmPanels.Count == 0 && this.userConfirmFlag != null)
			{
				this.userConfirmFlag.Raise();
				this.userConfirmFlag = null;
			}
		}

		// Token: 0x06005674 RID: 22132 RVA: 0x001F9144 File Offset: 0x001F7544
		protected void CreateUserConfirmFlag()
		{
			if (this.userConfirmFlag == null && SuperController.singleton != null)
			{
				this.userConfirmFlag = new AsyncFlag("UserConfirm");
				SuperController.singleton.SetLoadingIconFlag(this.userConfirmFlag);
				SuperController.singleton.PauseAutoSimulation(this.userConfirmFlag);
			}
		}

		// Token: 0x06005675 RID: 22133 RVA: 0x001F919C File Offset: 0x001F759C
		protected void DestroyAllUserConfirmPanels()
		{
			List<UserConfirmPanel> list = new List<UserConfirmPanel>(this.activeUserConfirmPanels);
			foreach (UserConfirmPanel ucp in list)
			{
				this.DestroyUserConfirmPanel(ucp);
			}
		}

		// Token: 0x06005676 RID: 22134 RVA: 0x001F9200 File Offset: 0x001F7600
		protected void UserConfirm(string prompt, UserActionCallback confirmCallback, UserActionCallback autoConfirmCallback, UserActionCallback confirmStickyCallback, UserActionCallback denyCallback, UserActionCallback autoDenyCallback, UserActionCallback denyStickyCallback)
		{
			FileManager.<UserConfirm>c__AnonStorey2 <UserConfirm>c__AnonStorey = new FileManager.<UserConfirm>c__AnonStorey2();
			<UserConfirm>c__AnonStorey.confirmCallback = confirmCallback;
			<UserConfirm>c__AnonStorey.autoConfirmCallback = autoConfirmCallback;
			<UserConfirm>c__AnonStorey.confirmStickyCallback = confirmStickyCallback;
			<UserConfirm>c__AnonStorey.denyCallback = denyCallback;
			<UserConfirm>c__AnonStorey.autoDenyCallback = autoDenyCallback;
			<UserConfirm>c__AnonStorey.denyStickyCallback = denyStickyCallback;
			<UserConfirm>c__AnonStorey.$this = this;
			if (this.userConfirmContainer != null && this.userConfirmPrefab != null)
			{
				FileManager.<UserConfirm>c__AnonStorey1 <UserConfirm>c__AnonStorey2 = new FileManager.<UserConfirm>c__AnonStorey1();
				<UserConfirm>c__AnonStorey2.<>f__ref$2 = <UserConfirm>c__AnonStorey;
				if (this.activeUserConfirmPanels == null)
				{
					this.activeUserConfirmPanels = new HashSet<UserConfirmPanel>();
				}
				this.CreateUserConfirmFlag();
				Transform transform = UnityEngine.Object.Instantiate<Transform>(this.userConfirmPrefab);
				transform.SetParent(this.userConfirmContainer, false);
				transform.SetAsFirstSibling();
				<UserConfirm>c__AnonStorey2.ucp = transform.GetComponent<UserConfirmPanel>();
				if (<UserConfirm>c__AnonStorey2.ucp != null)
				{
					<UserConfirm>c__AnonStorey2.ucp.signature = prompt;
					<UserConfirm>c__AnonStorey2.ucp.SetPrompt(prompt);
					this.activeUserConfirmPanels.Add(<UserConfirm>c__AnonStorey2.ucp);
					<UserConfirm>c__AnonStorey2.ucp.SetConfirmCallback(new UserConfirmPanel.ResponseCallback(<UserConfirm>c__AnonStorey2.<>m__0));
					<UserConfirm>c__AnonStorey2.ucp.SetAutoConfirmCallback(new UserConfirmPanel.ResponseCallback(<UserConfirm>c__AnonStorey2.<>m__1));
					<UserConfirm>c__AnonStorey2.ucp.SetConfirmStickyCallback(new UserConfirmPanel.ResponseCallback(<UserConfirm>c__AnonStorey2.<>m__2));
					<UserConfirm>c__AnonStorey2.ucp.SetDenyCallback(new UserConfirmPanel.ResponseCallback(<UserConfirm>c__AnonStorey2.<>m__3));
					<UserConfirm>c__AnonStorey2.ucp.SetAutoDenyCallback(new UserConfirmPanel.ResponseCallback(<UserConfirm>c__AnonStorey2.<>m__4));
					<UserConfirm>c__AnonStorey2.ucp.SetDenyStickyCallback(new UserConfirmPanel.ResponseCallback(<UserConfirm>c__AnonStorey2.<>m__5));
				}
				else
				{
					UnityEngine.Object.Destroy(transform.gameObject);
					if (<UserConfirm>c__AnonStorey.denyCallback != null)
					{
						<UserConfirm>c__AnonStorey.denyCallback();
					}
				}
			}
			else if (<UserConfirm>c__AnonStorey.denyCallback != null)
			{
				<UserConfirm>c__AnonStorey.denyCallback();
			}
		}

		// Token: 0x06005677 RID: 22135 RVA: 0x001F93C4 File Offset: 0x001F77C4
		public static void ConfirmWithUser(string prompt, UserActionCallback confirmCallback, UserActionCallback autoConfirmCallback, UserActionCallback confirmStickyCallback, UserActionCallback denyCallback, UserActionCallback autoDenyCallback, UserActionCallback denyStickyCallback)
		{
			if (FileManager.singleton != null)
			{
				FileManager.singleton.UserConfirm(prompt, confirmCallback, autoConfirmCallback, confirmStickyCallback, denyCallback, autoDenyCallback, denyStickyCallback);
			}
			else
			{
				denyCallback();
			}
		}

		// Token: 0x06005678 RID: 22136 RVA: 0x001F93F8 File Offset: 0x001F77F8
		protected void AutoConfirmAllPanelsWithSignature(string signature)
		{
			List<UserConfirmPanel> list = new List<UserConfirmPanel>();
			foreach (UserConfirmPanel userConfirmPanel in this.activeUserConfirmPanels)
			{
				if (userConfirmPanel.signature == signature)
				{
					list.Add(userConfirmPanel);
				}
			}
			foreach (UserConfirmPanel userConfirmPanel2 in list)
			{
				userConfirmPanel2.AutoConfirm();
			}
		}

		// Token: 0x06005679 RID: 22137 RVA: 0x001F94B4 File Offset: 0x001F78B4
		protected void ConfirmAllPanelsWithSignature(string signature, bool isPlugin)
		{
			List<UserConfirmPanel> list = new List<UserConfirmPanel>();
			foreach (UserConfirmPanel userConfirmPanel in this.activeUserConfirmPanels)
			{
				if (userConfirmPanel.signature == signature)
				{
					list.Add(userConfirmPanel);
				}
			}
			foreach (UserConfirmPanel userConfirmPanel2 in list)
			{
				userConfirmPanel2.Confirm();
			}
			if (isPlugin)
			{
				FileManager.userConfirmedPlugins.Add(signature);
			}
		}

		// Token: 0x0600567A RID: 22138 RVA: 0x001F9580 File Offset: 0x001F7980
		public static void AutoConfirmAllWithSignature(string signature)
		{
			if (FileManager.singleton != null)
			{
				FileManager.singleton.AutoConfirmAllPanelsWithSignature(signature);
			}
		}

		// Token: 0x0600567B RID: 22139 RVA: 0x001F95A0 File Offset: 0x001F79A0
		protected void AutoDenyAllPanelsWithSignature(string signature)
		{
			List<UserConfirmPanel> list = new List<UserConfirmPanel>();
			foreach (UserConfirmPanel userConfirmPanel in this.activeUserConfirmPanels)
			{
				if (userConfirmPanel.signature == signature)
				{
					list.Add(userConfirmPanel);
				}
			}
			foreach (UserConfirmPanel userConfirmPanel2 in list)
			{
				userConfirmPanel2.AutoDeny();
			}
		}

		// Token: 0x0600567C RID: 22140 RVA: 0x001F965C File Offset: 0x001F7A5C
		protected void DenyAllPanelsWithSignature(string signature, bool isPlugin)
		{
			List<UserConfirmPanel> list = new List<UserConfirmPanel>();
			foreach (UserConfirmPanel userConfirmPanel in this.activeUserConfirmPanels)
			{
				if (userConfirmPanel.signature == signature)
				{
					list.Add(userConfirmPanel);
				}
			}
			foreach (UserConfirmPanel userConfirmPanel2 in list)
			{
				userConfirmPanel2.Deny();
			}
			if (isPlugin)
			{
				FileManager.userDeniedPlugins.Add(signature);
			}
		}

		// Token: 0x0600567D RID: 22141 RVA: 0x001F9728 File Offset: 0x001F7B28
		public static void AutoDenyAllWithSignature(string signature)
		{
			if (FileManager.singleton != null)
			{
				FileManager.singleton.AutoDenyAllPanelsWithSignature(signature);
			}
		}

		// Token: 0x0600567E RID: 22142 RVA: 0x001F9748 File Offset: 0x001F7B48
		protected void UserConfirmPluginAction(string prompt, UserActionCallback confirmCallback, UserActionCallback denyCallback)
		{
			FileManager.<UserConfirmPluginAction>c__AnonStorey4 <UserConfirmPluginAction>c__AnonStorey = new FileManager.<UserConfirmPluginAction>c__AnonStorey4();
			<UserConfirmPluginAction>c__AnonStorey.confirmCallback = confirmCallback;
			<UserConfirmPluginAction>c__AnonStorey.denyCallback = denyCallback;
			<UserConfirmPluginAction>c__AnonStorey.$this = this;
			if (this.userConfirmContainer != null && this.userConfirmPluginActionPrefab != null)
			{
				FileManager.<UserConfirmPluginAction>c__AnonStorey3 <UserConfirmPluginAction>c__AnonStorey2 = new FileManager.<UserConfirmPluginAction>c__AnonStorey3();
				<UserConfirmPluginAction>c__AnonStorey2.<>f__ref$4 = <UserConfirmPluginAction>c__AnonStorey;
				if (FileManager.userConfirmedPlugins == null)
				{
					FileManager.userConfirmedPlugins = new HashSet<string>();
				}
				if (FileManager.userDeniedPlugins == null)
				{
					FileManager.userDeniedPlugins = new HashSet<string>();
				}
				if (this.activeUserConfirmPanels == null)
				{
					this.activeUserConfirmPanels = new HashSet<UserConfirmPanel>();
				}
				<UserConfirmPluginAction>c__AnonStorey2.pluginHash = FileManager.GetPluginHash();
				if (<UserConfirmPluginAction>c__AnonStorey2.pluginHash == null)
				{
					UnityEngine.Debug.LogError("Plugin did not have signature!");
				}
				if (<UserConfirmPluginAction>c__AnonStorey2.pluginHash != null)
				{
					if (FileManager.userDeniedPlugins.Contains(<UserConfirmPluginAction>c__AnonStorey2.pluginHash))
					{
						if (<UserConfirmPluginAction>c__AnonStorey.denyCallback != null)
						{
							<UserConfirmPluginAction>c__AnonStorey.denyCallback();
						}
						return;
					}
					if (FileManager.userConfirmedPlugins.Contains(<UserConfirmPluginAction>c__AnonStorey2.pluginHash))
					{
						if (<UserConfirmPluginAction>c__AnonStorey.confirmCallback != null)
						{
							<UserConfirmPluginAction>c__AnonStorey.confirmCallback();
						}
						return;
					}
				}
				Transform transform = UnityEngine.Object.Instantiate<Transform>(this.userConfirmPluginActionPrefab);
				transform.SetParent(this.userConfirmContainer, false);
				transform.SetAsFirstSibling();
				<UserConfirmPluginAction>c__AnonStorey2.ucp = transform.GetComponent<UserConfirmPanel>();
				if (<UserConfirmPluginAction>c__AnonStorey2.ucp != null && <UserConfirmPluginAction>c__AnonStorey2.pluginHash != null)
				{
					string pluginHash;
					if (FileManager.pluginHashToPluginPath == null || !FileManager.pluginHashToPluginPath.TryGetValue(<UserConfirmPluginAction>c__AnonStorey2.pluginHash, out pluginHash))
					{
						pluginHash = <UserConfirmPluginAction>c__AnonStorey2.pluginHash;
					}
					<UserConfirmPluginAction>c__AnonStorey2.ucp.signature = <UserConfirmPluginAction>c__AnonStorey2.pluginHash;
					<UserConfirmPluginAction>c__AnonStorey2.ucp.SetPrompt("Plugin " + pluginHash + "\nwants to " + prompt);
					this.activeUserConfirmPanels.Add(<UserConfirmPluginAction>c__AnonStorey2.ucp);
					<UserConfirmPluginAction>c__AnonStorey2.ucp.SetConfirmCallback(new UserConfirmPanel.ResponseCallback(<UserConfirmPluginAction>c__AnonStorey2.<>m__0));
					<UserConfirmPluginAction>c__AnonStorey2.ucp.SetConfirmStickyCallback(new UserConfirmPanel.ResponseCallback(<UserConfirmPluginAction>c__AnonStorey2.<>m__1));
					<UserConfirmPluginAction>c__AnonStorey2.ucp.SetDenyCallback(new UserConfirmPanel.ResponseCallback(<UserConfirmPluginAction>c__AnonStorey2.<>m__2));
					<UserConfirmPluginAction>c__AnonStorey2.ucp.SetDenyStickyCallback(new UserConfirmPanel.ResponseCallback(<UserConfirmPluginAction>c__AnonStorey2.<>m__3));
				}
				else
				{
					UnityEngine.Object.Destroy(transform.gameObject);
					if (<UserConfirmPluginAction>c__AnonStorey.denyCallback != null)
					{
						<UserConfirmPluginAction>c__AnonStorey.denyCallback();
					}
				}
			}
			else if (<UserConfirmPluginAction>c__AnonStorey.denyCallback != null)
			{
				<UserConfirmPluginAction>c__AnonStorey.denyCallback();
			}
		}

		// Token: 0x0600567F RID: 22143 RVA: 0x001F99AB File Offset: 0x001F7DAB
		public static void ConfirmPluginActionWithUser(string prompt, UserActionCallback confirmCallback, UserActionCallback denyCallback)
		{
			if (FileManager.singleton != null)
			{
				FileManager.singleton.UserConfirmPluginAction(prompt, confirmCallback, denyCallback);
			}
			else
			{
				denyCallback();
			}
		}

		// Token: 0x06005680 RID: 22144 RVA: 0x001F99D8 File Offset: 0x001F7DD8
		public static string GetFullPath(string path)
		{
			string path2 = Regex.Replace(path, "^file:///", string.Empty);
			return Path.GetFullPath(path2);
		}

		// Token: 0x06005681 RID: 22145 RVA: 0x001F99FC File Offset: 0x001F7DFC
		public static bool IsPackagePath(string path)
		{
			string input = path.Replace('\\', '/');
			string packageUidOrPath = Regex.Replace(input, ":/.*", string.Empty);
			VarPackage package = FileManager.GetPackage(packageUidOrPath);
			return package != null;
		}

		// Token: 0x06005682 RID: 22146 RVA: 0x001F9A34 File Offset: 0x001F7E34
		public static bool IsSimulatedPackagePath(string path)
		{
			string input = path.Replace('\\', '/');
			string packageUidOrPath = Regex.Replace(input, ":/.*", string.Empty);
			VarPackage package = FileManager.GetPackage(packageUidOrPath);
			return package != null && package.IsSimulated;
		}

		// Token: 0x06005683 RID: 22147 RVA: 0x001F9A74 File Offset: 0x001F7E74
		public static string ConvertSimulatedPackagePathToNormalPath(string path)
		{
			string text = path.Replace('\\', '/');
			if (text.Contains(":/"))
			{
				string packageUidOrPath = Regex.Replace(text, ":/.*", string.Empty);
				VarPackage package = FileManager.GetPackage(packageUidOrPath);
				if (package != null && package.IsSimulated)
				{
					string str = Regex.Replace(text, ".*:/", string.Empty);
					path = package.SlashPath + "/" + str;
				}
			}
			return path;
		}

		// Token: 0x06005684 RID: 22148 RVA: 0x001F9AEC File Offset: 0x001F7EEC
		public static string RemovePackageFromPath(string path)
		{
			string input = Regex.Replace(path, ".*:/", string.Empty);
			return Regex.Replace(input, ".*:\\\\", string.Empty);
		}

		// Token: 0x06005685 RID: 22149 RVA: 0x001F9B1C File Offset: 0x001F7F1C
		public static string NormalizePath(string path)
		{
			string text = path;
			VarFileEntry varFileEntry = FileManager.GetVarFileEntry(path);
			if (varFileEntry == null)
			{
				string fullPath = FileManager.GetFullPath(path);
				string oldValue = Path.GetFullPath(".") + "\\";
				string text2 = fullPath.Replace(oldValue, string.Empty);
				if (text2 != fullPath)
				{
					text = text2;
				}
				text = text.Replace('\\', '/');
			}
			else
			{
				text = varFileEntry.Uid;
			}
			return text;
		}

		// Token: 0x06005686 RID: 22150 RVA: 0x001F9B8C File Offset: 0x001F7F8C
		public static string GetDirectoryName(string path, bool returnSlashPath = false)
		{
			VarFileEntry varFileEntry;
			string path2;
			if (FileManager.uidToVarFileEntry != null && FileManager.uidToVarFileEntry.TryGetValue(path, out varFileEntry))
			{
				if (returnSlashPath)
				{
					path2 = varFileEntry.SlashPath;
				}
				else
				{
					path2 = varFileEntry.Path;
				}
			}
			else if (returnSlashPath)
			{
				path2 = path.Replace('\\', '/');
			}
			else
			{
				path2 = path.Replace('/', '\\');
			}
			return Path.GetDirectoryName(path2);
		}

		// Token: 0x06005687 RID: 22151 RVA: 0x001F9BFC File Offset: 0x001F7FFC
		public static string GetSuggestedBrowserDirectoryFromDirectoryPath(string suggestedDir, string currentDir, bool allowPackagePath = true)
		{
			if (currentDir == null || currentDir == string.Empty)
			{
				return suggestedDir;
			}
			string text = suggestedDir.Replace('\\', '/');
			text = Regex.Replace(text, "/$", string.Empty);
			string text2 = currentDir.Replace('\\', '/');
			VarDirectoryEntry varDirectoryEntry = FileManager.GetVarDirectoryEntry(text2);
			if (varDirectoryEntry != null)
			{
				if (!allowPackagePath)
				{
					return null;
				}
				string text3 = varDirectoryEntry.InternalSlashPath.Replace(text, string.Empty);
				if (varDirectoryEntry.InternalSlashPath != text3)
				{
					text3 = text3.Replace('/', '\\');
					return varDirectoryEntry.Package.SlashPath + ":/" + text + text3;
				}
			}
			else
			{
				string text4 = text2.Replace(text, string.Empty);
				if (text2 != text4)
				{
					text4 = text4.Replace('/', '\\');
					return suggestedDir + text4;
				}
			}
			return null;
		}

		// Token: 0x17000CA0 RID: 3232
		// (get) Token: 0x06005688 RID: 22152 RVA: 0x001F9CE2 File Offset: 0x001F80E2
		public static string CurrentLoadDir
		{
			get
			{
				if (FileManager.loadDirStack != null && FileManager.loadDirStack.Count > 0)
				{
					return FileManager.loadDirStack.Last.Value;
				}
				return null;
			}
		}

		// Token: 0x17000CA1 RID: 3233
		// (get) Token: 0x06005689 RID: 22153 RVA: 0x001F9D10 File Offset: 0x001F8110
		public static string CurrentPackageUid
		{
			get
			{
				string currentLoadDir = FileManager.CurrentLoadDir;
				if (currentLoadDir != null)
				{
					VarDirectoryEntry varDirectoryEntry = FileManager.GetVarDirectoryEntry(currentLoadDir);
					if (varDirectoryEntry != null)
					{
						return varDirectoryEntry.Package.Uid;
					}
				}
				return null;
			}
		}

		// Token: 0x17000CA2 RID: 3234
		// (get) Token: 0x0600568A RID: 22154 RVA: 0x001F9D43 File Offset: 0x001F8143
		public static string TopLoadDir
		{
			get
			{
				if (FileManager.loadDirStack != null && FileManager.loadDirStack.Count > 0)
				{
					return FileManager.loadDirStack.First.Value;
				}
				return null;
			}
		}

		// Token: 0x17000CA3 RID: 3235
		// (get) Token: 0x0600568B RID: 22155 RVA: 0x001F9D70 File Offset: 0x001F8170
		public static string TopPackageUid
		{
			get
			{
				string topLoadDir = FileManager.TopLoadDir;
				if (topLoadDir != null)
				{
					VarDirectoryEntry varDirectoryEntry = FileManager.GetVarDirectoryEntry(topLoadDir);
					if (varDirectoryEntry != null)
					{
						return varDirectoryEntry.Package.Uid;
					}
				}
				return null;
			}
		}

		// Token: 0x0600568C RID: 22156 RVA: 0x001F9DA3 File Offset: 0x001F81A3
		public static void SetLoadDir(string dir, bool restrictPath = false)
		{
			if (FileManager.loadDirStack != null)
			{
				FileManager.loadDirStack.Clear();
			}
			FileManager.PushLoadDir(dir, restrictPath);
		}

		// Token: 0x0600568D RID: 22157 RVA: 0x001F9DC0 File Offset: 0x001F81C0
		public static void PushLoadDir(string dir, bool restrictPath = false)
		{
			string text = dir.Replace('\\', '/');
			if (text != "/")
			{
				text = Regex.Replace(text, "/$", string.Empty);
			}
			if (restrictPath && !FileManager.IsSecureReadPath(text))
			{
				throw new Exception("Attempted to push load dir for non-secure dir " + text);
			}
			if (FileManager.loadDirStack == null)
			{
				FileManager.loadDirStack = new LinkedList<string>();
			}
			FileManager.loadDirStack.AddLast(text);
		}

		// Token: 0x0600568E RID: 22158 RVA: 0x001F9E3C File Offset: 0x001F823C
		public static string PopLoadDir()
		{
			string result = null;
			if (FileManager.loadDirStack != null && FileManager.loadDirStack.Count > 0)
			{
				result = FileManager.loadDirStack.Last.Value;
				FileManager.loadDirStack.RemoveLast();
			}
			return result;
		}

		// Token: 0x0600568F RID: 22159 RVA: 0x001F9E80 File Offset: 0x001F8280
		public static void SetLoadDirFromFilePath(string path, bool restrictPath = false)
		{
			if (FileManager.loadDirStack != null)
			{
				FileManager.loadDirStack.Clear();
			}
			FileManager.PushLoadDirFromFilePath(path, restrictPath);
		}

		// Token: 0x06005690 RID: 22160 RVA: 0x001F9EA0 File Offset: 0x001F82A0
		public static void PushLoadDirFromFilePath(string path, bool restrictPath = false)
		{
			if (restrictPath && !FileManager.IsSecureReadPath(path))
			{
				throw new Exception("Attempted to set load dir from non-secure path " + path);
			}
			FileEntry fileEntry = FileManager.GetFileEntry(path, false);
			string text;
			if (fileEntry != null)
			{
				if (fileEntry is VarFileEntry)
				{
					text = Path.GetDirectoryName(fileEntry.Uid);
				}
				else
				{
					text = Path.GetDirectoryName(fileEntry.FullPath);
					string oldValue = Path.GetFullPath(".") + "\\";
					text = text.Replace(oldValue, string.Empty);
				}
			}
			else
			{
				text = Path.GetDirectoryName(FileManager.GetFullPath(path));
				string oldValue2 = Path.GetFullPath(".") + "\\";
				text = text.Replace(oldValue2, string.Empty);
			}
			FileManager.PushLoadDir(text, restrictPath);
		}

		// Token: 0x06005691 RID: 22161 RVA: 0x001F9F64 File Offset: 0x001F8364
		public static string PackageIDToPackageGroupID(string packageId)
		{
			string input = Regex.Replace(packageId, "\\.[0-9]+$", string.Empty);
			input = Regex.Replace(input, "\\.latest$", string.Empty);
			return Regex.Replace(input, "\\.min[0-9]+$", string.Empty);
		}

		// Token: 0x06005692 RID: 22162 RVA: 0x001F9FA8 File Offset: 0x001F83A8
		public static string PackageIDToPackageVersion(string packageId)
		{
			Match match = Regex.Match(packageId, "[0-9]+$");
			if (match.Success)
			{
				return match.Value;
			}
			return null;
		}

		// Token: 0x06005693 RID: 22163 RVA: 0x001F9FD4 File Offset: 0x001F83D4
		public static string NormalizeID(string id)
		{
			string result;
			if (id.StartsWith("SELF:"))
			{
				string currentPackageUid = FileManager.CurrentPackageUid;
				if (currentPackageUid != null)
				{
					result = id.Replace("SELF:", currentPackageUid + ":");
				}
				else
				{
					result = id.Replace("SELF:", string.Empty);
				}
			}
			else
			{
				result = FileManager.NormalizeCommon(id);
			}
			return result;
		}

		// Token: 0x06005694 RID: 22164 RVA: 0x001FA03C File Offset: 0x001F843C
		protected static string NormalizeCommon(string path)
		{
			string text = path;
			Match match;
			if ((match = Regex.Match(text, "^(([^\\.]+\\.[^\\.]+)\\.latest):")).Success)
			{
				string value = match.Groups[1].Value;
				string value2 = match.Groups[2].Value;
				VarPackageGroup packageGroup = FileManager.GetPackageGroup(value2);
				if (packageGroup != null)
				{
					VarPackage newestEnabledPackage = packageGroup.NewestEnabledPackage;
					if (newestEnabledPackage != null)
					{
						text = text.Replace(value, newestEnabledPackage.Uid);
					}
				}
			}
			else if ((match = Regex.Match(text, "^(([^\\.]+\\.[^\\.]+)\\.min([0-9]+)):")).Success)
			{
				string value3 = match.Groups[1].Value;
				string value4 = match.Groups[2].Value;
				int requestVersion = int.Parse(match.Groups[3].Value);
				VarPackageGroup packageGroup2 = FileManager.GetPackageGroup(value4);
				if (packageGroup2 != null)
				{
					VarPackage closestMatchingPackageVersion = packageGroup2.GetClosestMatchingPackageVersion(requestVersion, true, false);
					if (closestMatchingPackageVersion != null)
					{
						text = text.Replace(value3, closestMatchingPackageVersion.Uid);
					}
				}
			}
			else if ((match = Regex.Match(text, "^([^\\.]+\\.[^\\.]+\\.[0-9]+):")).Success)
			{
				string value5 = match.Groups[1].Value;
				VarPackage varPackage = FileManager.GetPackage(value5);
				if (varPackage == null || !varPackage.Enabled)
				{
					string packageGroupUid = FileManager.PackageIDToPackageGroupID(value5);
					VarPackageGroup packageGroup3 = FileManager.GetPackageGroup(packageGroupUid);
					if (packageGroup3 != null)
					{
						varPackage = packageGroup3.NewestEnabledPackage;
						if (varPackage != null)
						{
							text = text.Replace(value5, varPackage.Uid);
						}
					}
				}
			}
			return text;
		}

		// Token: 0x06005695 RID: 22165 RVA: 0x001FA1C8 File Offset: 0x001F85C8
		public static string NormalizeLoadPath(string path)
		{
			string text = path;
			if (path != null && path != string.Empty && path != "/" && path != "NULL")
			{
				text = path.Replace('\\', '/');
				string currentLoadDir = FileManager.CurrentLoadDir;
				if (currentLoadDir != null && currentLoadDir != string.Empty)
				{
					if (!text.Contains("/"))
					{
						text = currentLoadDir + "/" + text;
					}
					else if (Regex.IsMatch(text, "^\\./"))
					{
						text = Regex.Replace(text, "^\\./", currentLoadDir + "/");
					}
				}
				if (text.StartsWith("SELF:/"))
				{
					string currentPackageUid = FileManager.CurrentPackageUid;
					if (currentPackageUid != null)
					{
						text = text.Replace("SELF:/", currentPackageUid + ":/");
					}
					else
					{
						text = text.Replace("SELF:/", string.Empty);
					}
				}
				else
				{
					text = FileManager.NormalizeCommon(text);
				}
			}
			return text;
		}

		// Token: 0x17000CA4 RID: 3236
		// (get) Token: 0x06005696 RID: 22166 RVA: 0x001FA2D3 File Offset: 0x001F86D3
		// (set) Token: 0x06005697 RID: 22167 RVA: 0x001FA2DA File Offset: 0x001F86DA
		public static string CurrentSaveDir
		{
			[CompilerGenerated]
			get
			{
				return FileManager.<CurrentSaveDir>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				FileManager.<CurrentSaveDir>k__BackingField = value;
			}
		}

		// Token: 0x06005698 RID: 22168 RVA: 0x001FA2E4 File Offset: 0x001F86E4
		public static void SetSaveDir(string path, bool restrictPath = true)
		{
			if (path == null || path == string.Empty)
			{
				FileManager.CurrentSaveDir = string.Empty;
			}
			else
			{
				path = FileManager.ConvertSimulatedPackagePathToNormalPath(path);
				if (FileManager.IsPackagePath(path))
				{
					return;
				}
				if (restrictPath && !FileManager.IsSecureWritePath(path))
				{
					throw new Exception("Attempted to set save dir from non-secure path " + path);
				}
				string text = FileManager.GetFullPath(path);
				string oldValue = Path.GetFullPath(".") + "\\";
				text = text.Replace(oldValue, string.Empty);
				FileManager.CurrentSaveDir = text.Replace('\\', '/');
			}
		}

		// Token: 0x06005699 RID: 22169 RVA: 0x001FA384 File Offset: 0x001F8784
		public static void SetSaveDirFromFilePath(string path, bool restrictPath = true)
		{
			path = FileManager.ConvertSimulatedPackagePathToNormalPath(path);
			if (FileManager.IsPackagePath(path))
			{
				return;
			}
			if (restrictPath && !FileManager.IsSecureWritePath(path))
			{
				throw new Exception("Attempted to set save dir from non-secure path " + path);
			}
			string text = Path.GetDirectoryName(FileManager.GetFullPath(path));
			string oldValue = Path.GetFullPath(".") + "\\";
			text = text.Replace(oldValue, string.Empty);
			FileManager.CurrentSaveDir = text.Replace('\\', '/');
		}

		// Token: 0x0600569A RID: 22170 RVA: 0x001FA404 File Offset: 0x001F8804
		public static void SetNullSaveDir()
		{
			FileManager.CurrentSaveDir = null;
		}

		// Token: 0x0600569B RID: 22171 RVA: 0x001FA40C File Offset: 0x001F880C
		public static string NormalizeSavePath(string path)
		{
			string text = path;
			if (path != null && path != string.Empty && path != "/" && path != "NULL")
			{
				string path2 = Regex.Replace(path, "^file:///", string.Empty);
				string fullPath = Path.GetFullPath(path2);
				string oldValue = Path.GetFullPath(".") + "\\";
				string text2 = fullPath.Replace(oldValue, string.Empty);
				if (text2 != fullPath)
				{
					text = text2;
				}
				text = text.Replace('\\', '/');
				string fileName = Path.GetFileName(text2);
				string text3 = Path.GetDirectoryName(text2);
				if (text3 != null)
				{
					text3 = text3.Replace('\\', '/');
				}
				if (FileManager.CurrentSaveDir == text3)
				{
					text = fileName;
				}
				else if (FileManager.CurrentSaveDir != null && FileManager.CurrentSaveDir != string.Empty && Regex.IsMatch(text3, "^" + FileManager.CurrentSaveDir + "/"))
				{
					text = text3.Replace(FileManager.CurrentSaveDir, ".") + "/" + fileName;
				}
			}
			return text;
		}

		// Token: 0x0600569C RID: 22172 RVA: 0x001FA544 File Offset: 0x001F8944
		public static List<VarPackage> GetPackages()
		{
			List<VarPackage> result;
			if (FileManager.packagesByUid != null)
			{
				result = FileManager.packagesByUid.Values.ToList<VarPackage>();
			}
			else
			{
				result = new List<VarPackage>();
			}
			return result;
		}

		// Token: 0x0600569D RID: 22173 RVA: 0x001FA57C File Offset: 0x001F897C
		public static List<string> GetPackageUids()
		{
			List<string> list;
			if (FileManager.packagesByUid != null)
			{
				list = FileManager.packagesByUid.Keys.ToList<string>();
				list.Sort();
			}
			else
			{
				list = new List<string>();
			}
			return list;
		}

		// Token: 0x0600569E RID: 22174 RVA: 0x001FA5B7 File Offset: 0x001F89B7
		public static bool IsPackage(string packageUidOrPath)
		{
			return (FileManager.packagesByUid != null && FileManager.packagesByUid.ContainsKey(packageUidOrPath)) || (FileManager.packagesByPath != null && FileManager.packagesByPath.ContainsKey(packageUidOrPath));
		}

		// Token: 0x0600569F RID: 22175 RVA: 0x001FA5F4 File Offset: 0x001F89F4
		public static VarPackage GetPackage(string packageUidOrPath)
		{
			VarPackage result = null;
			Match match;
			if ((match = Regex.Match(packageUidOrPath, "^([^\\.]+\\.[^\\.]+)\\.latest$")).Success)
			{
				string value = match.Groups[1].Value;
				VarPackageGroup packageGroup = FileManager.GetPackageGroup(value);
				if (packageGroup != null)
				{
					result = packageGroup.NewestPackage;
				}
			}
			else if ((match = Regex.Match(packageUidOrPath, "^([^\\.]+\\.[^\\.]+)\\.min([0-9]+)$")).Success)
			{
				string value2 = match.Groups[1].Value;
				int requestVersion = int.Parse(match.Groups[2].Value);
				VarPackageGroup packageGroup2 = FileManager.GetPackageGroup(value2);
				if (packageGroup2 != null)
				{
					result = packageGroup2.GetClosestMatchingPackageVersion(requestVersion, false, false);
				}
			}
			else if (FileManager.packagesByUid != null && FileManager.packagesByUid.ContainsKey(packageUidOrPath))
			{
				FileManager.packagesByUid.TryGetValue(packageUidOrPath, out result);
			}
			else if (FileManager.packagesByPath != null && FileManager.packagesByPath.ContainsKey(packageUidOrPath))
			{
				FileManager.packagesByPath.TryGetValue(packageUidOrPath, out result);
			}
			return result;
		}

		// Token: 0x060056A0 RID: 22176 RVA: 0x001FA700 File Offset: 0x001F8B00
		public static List<VarPackageGroup> GetPackageGroups()
		{
			List<VarPackageGroup> result;
			if (FileManager.packageGroups != null)
			{
				result = FileManager.packageGroups.Values.ToList<VarPackageGroup>();
			}
			else
			{
				result = new List<VarPackageGroup>();
			}
			return result;
		}

		// Token: 0x060056A1 RID: 22177 RVA: 0x001FA738 File Offset: 0x001F8B38
		public static VarPackageGroup GetPackageGroup(string packageGroupUid)
		{
			VarPackageGroup result = null;
			if (FileManager.packageGroups != null)
			{
				FileManager.packageGroups.TryGetValue(packageGroupUid, out result);
			}
			return result;
		}

		// Token: 0x060056A2 RID: 22178 RVA: 0x001FA760 File Offset: 0x001F8B60
		public static void SyncAutoAlwaysAllowPlugins(HashSet<FileManager.PackageUIDAndPublisher> packageUids)
		{
			foreach (FileManager.PackageUIDAndPublisher packageUIDAndPublisher in packageUids)
			{
				FileManager.<SyncAutoAlwaysAllowPlugins>c__AnonStorey5 <SyncAutoAlwaysAllowPlugins>c__AnonStorey = new FileManager.<SyncAutoAlwaysAllowPlugins>c__AnonStorey5();
				<SyncAutoAlwaysAllowPlugins>c__AnonStorey.vp = FileManager.GetPackage(packageUIDAndPublisher.uid);
				if (<SyncAutoAlwaysAllowPlugins>c__AnonStorey.vp != null && <SyncAutoAlwaysAllowPlugins>c__AnonStorey.vp.HasMatchingDirectories("Custom/Scripts"))
				{
					foreach (VarPackage varPackage in <SyncAutoAlwaysAllowPlugins>c__AnonStorey.vp.Group.Packages)
					{
						if (varPackage != <SyncAutoAlwaysAllowPlugins>c__AnonStorey.vp && varPackage.PluginsAlwaysEnabled)
						{
							<SyncAutoAlwaysAllowPlugins>c__AnonStorey.vp.PluginsAlwaysEnabled = true;
							SuperController.AlertUser(<SyncAutoAlwaysAllowPlugins>c__AnonStorey.vp.Uid + "\nuploaded by Hub user: " + packageUIDAndPublisher.publisher + "\n\nwas just downloaded and contains plugins. This package has been automatically set to always enable plugins due to previous version of this same package having that preference set.\n\nClick OK if you accept.\n\nClick Cancel if you wish to reject this automatic setting.", null, new UnityAction(<SyncAutoAlwaysAllowPlugins>c__AnonStorey.<>m__0), SuperController.DisplayUIChoice.Auto);
							break;
						}
					}
					if (UserPreferences.singleton.alwaysAllowPluginsDownloadedFromHub && !<SyncAutoAlwaysAllowPlugins>c__AnonStorey.vp.PluginsAlwaysEnabled)
					{
						<SyncAutoAlwaysAllowPlugins>c__AnonStorey.vp.PluginsAlwaysEnabled = true;
						SuperController.AlertUser(<SyncAutoAlwaysAllowPlugins>c__AnonStorey.vp.Uid + "\nuploaded by Hub user: " + packageUIDAndPublisher.publisher + "\n\nwas just downloaded and contains plugins. This package has been automatically set to always enable plugins due to your user preference setting.\n\nClick OK if you accept.\n\nClick Cancel if you wish to reject this automatic setting for this package.", null, new UnityAction(<SyncAutoAlwaysAllowPlugins>c__AnonStorey.<>m__1), SuperController.DisplayUIChoice.Auto);
					}
				}
			}
			packageUids.Clear();
		}

		// Token: 0x060056A3 RID: 22179 RVA: 0x001FA910 File Offset: 0x001F8D10
		public static string CleanFilePath(string path)
		{
			if (path != null)
			{
				return path.Replace('\\', '/');
			}
			return null;
		}

		// Token: 0x060056A4 RID: 22180 RVA: 0x001FA931 File Offset: 0x001F8D31
		public static void FindAllFiles(string dir, string pattern, List<FileEntry> foundFiles, bool restrictPath = false)
		{
			FileManager.FindRegularFiles(dir, pattern, foundFiles, restrictPath);
			FileManager.FindVarFiles(dir, pattern, foundFiles);
		}

		// Token: 0x060056A5 RID: 22181 RVA: 0x001FA944 File Offset: 0x001F8D44
		public static void FindAllFilesRegex(string dir, string regex, List<FileEntry> foundFiles, bool restrictPath = false)
		{
			FileManager.FindRegularFilesRegex(dir, regex, foundFiles, restrictPath);
			FileManager.FindVarFilesRegex(dir, regex, foundFiles);
		}

		// Token: 0x060056A6 RID: 22182 RVA: 0x001FA958 File Offset: 0x001F8D58
		public static void FindRegularFiles(string dir, string pattern, List<FileEntry> foundFiles, bool restrictPath = false)
		{
			if (Directory.Exists(dir))
			{
				if (restrictPath && !FileManager.IsSecureReadPath(dir))
				{
					throw new Exception("Attempted to find files for non-secure path " + dir);
				}
				string regex = "^" + Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", ".") + "$";
				FileManager.FindRegularFilesRegex(dir, regex, foundFiles, restrictPath);
			}
		}

		// Token: 0x060056A7 RID: 22183 RVA: 0x001FA9D0 File Offset: 0x001F8DD0
		public static bool CheckIfDirectoryChanged(string dir, DateTime previousCheckTime, bool recurse = true)
		{
			if (Directory.Exists(dir))
			{
				DateTime lastWriteTime = Directory.GetLastWriteTime(dir);
				if (lastWriteTime > previousCheckTime)
				{
					return true;
				}
				if (recurse)
				{
					foreach (string dir2 in Directory.GetDirectories(dir))
					{
						if (FileManager.CheckIfDirectoryChanged(dir2, previousCheckTime, recurse))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x060056A8 RID: 22184 RVA: 0x001FAA34 File Offset: 0x001F8E34
		public static void FindRegularFilesRegex(string dir, string regex, List<FileEntry> foundFiles, bool restrictPath = false)
		{
			dir = FileManager.CleanDirectoryPath(dir);
			if (Directory.Exists(dir))
			{
				if (restrictPath && !FileManager.IsSecureReadPath(dir))
				{
					throw new Exception("Attempted to find files for non-secure path " + dir);
				}
				foreach (string text in Directory.GetFiles(dir))
				{
					if (Regex.IsMatch(text, regex, RegexOptions.IgnoreCase))
					{
						SystemFileEntry systemFileEntry = new SystemFileEntry(text);
						if (systemFileEntry.Exists)
						{
							foundFiles.Add(systemFileEntry);
						}
						else
						{
							UnityEngine.Debug.LogError("Error in lookup SystemFileEntry for " + text);
						}
					}
				}
				foreach (string dir2 in Directory.GetDirectories(dir))
				{
					FileManager.FindRegularFilesRegex(dir2, regex, foundFiles, false);
				}
			}
		}

		// Token: 0x060056A9 RID: 22185 RVA: 0x001FAB04 File Offset: 0x001F8F04
		public static void FindVarFiles(string dir, string pattern, List<FileEntry> foundFiles)
		{
			if (FileManager.allVarFileEntries != null)
			{
				string regex = "^" + Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", ".") + "$";
				FileManager.FindVarFilesRegex(dir, regex, foundFiles);
			}
		}

		// Token: 0x060056AA RID: 22186 RVA: 0x001FAB58 File Offset: 0x001F8F58
		public static void FindVarFilesRegex(string dir, string regex, List<FileEntry> foundFiles)
		{
			dir = FileManager.CleanDirectoryPath(dir);
			if (FileManager.allVarFileEntries != null)
			{
				foreach (VarFileEntry varFileEntry in FileManager.allVarFileEntries)
				{
					if (varFileEntry.InternalSlashPath.StartsWith(dir))
					{
						if (Regex.IsMatch(varFileEntry.Name, regex, RegexOptions.IgnoreCase))
						{
							foundFiles.Add(varFileEntry);
						}
					}
				}
			}
		}

		// Token: 0x060056AB RID: 22187 RVA: 0x001FABF0 File Offset: 0x001F8FF0
		public static bool FileExists(string path, bool onlySystemFiles = false, bool restrictPath = false)
		{
			if (path != null && path != string.Empty)
			{
				if (!onlySystemFiles)
				{
					string key = FileManager.CleanFilePath(path);
					if (FileManager.uidToVarFileEntry != null && FileManager.uidToVarFileEntry.ContainsKey(path))
					{
						return true;
					}
					if (FileManager.pathToVarFileEntry != null && FileManager.pathToVarFileEntry.ContainsKey(key))
					{
						return true;
					}
				}
				if (File.Exists(path))
				{
					if (restrictPath && !FileManager.IsSecureReadPath(path))
					{
						throw new Exception("Attempted to check file existence for non-secure path " + path);
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x060056AC RID: 22188 RVA: 0x001FAC88 File Offset: 0x001F9088
		public static DateTime FileLastWriteTime(string path, bool onlySystemFiles = false, bool restrictPath = false)
		{
			if (path != null && path != string.Empty)
			{
				if (!onlySystemFiles)
				{
					string key = FileManager.CleanFilePath(path);
					VarFileEntry varFileEntry;
					if (FileManager.uidToVarFileEntry != null && FileManager.uidToVarFileEntry.TryGetValue(path, out varFileEntry))
					{
						return varFileEntry.LastWriteTime;
					}
					if (FileManager.pathToVarFileEntry != null && FileManager.pathToVarFileEntry.TryGetValue(key, out varFileEntry))
					{
						return varFileEntry.LastWriteTime;
					}
				}
				if (File.Exists(path))
				{
					if (restrictPath && !FileManager.IsSecureReadPath(path))
					{
						throw new Exception("Attempted to check file existence for non-secure path " + path);
					}
					FileInfo fileInfo = new FileInfo(path);
					return fileInfo.LastWriteTime;
				}
			}
			return DateTime.MinValue;
		}

		// Token: 0x060056AD RID: 22189 RVA: 0x001FAD40 File Offset: 0x001F9140
		public static DateTime FileCreationTime(string path, bool onlySystemFiles = false, bool restrictPath = false)
		{
			if (path != null && path != string.Empty)
			{
				if (!onlySystemFiles)
				{
					string key = FileManager.CleanFilePath(path);
					VarFileEntry varFileEntry;
					if (FileManager.uidToVarFileEntry != null && FileManager.uidToVarFileEntry.TryGetValue(path, out varFileEntry))
					{
						return varFileEntry.LastWriteTime;
					}
					if (FileManager.pathToVarFileEntry != null && FileManager.pathToVarFileEntry.TryGetValue(key, out varFileEntry))
					{
						return varFileEntry.LastWriteTime;
					}
				}
				if (File.Exists(path))
				{
					if (restrictPath && !FileManager.IsSecureReadPath(path))
					{
						throw new Exception("Attempted to check file existence for non-secure path " + path);
					}
					FileInfo fileInfo = new FileInfo(path);
					return fileInfo.CreationTime;
				}
			}
			return DateTime.MinValue;
		}

		// Token: 0x060056AE RID: 22190 RVA: 0x001FADF8 File Offset: 0x001F91F8
		public static bool IsFileInPackage(string path)
		{
			string key = FileManager.CleanFilePath(path);
			return (FileManager.uidToVarFileEntry != null && FileManager.uidToVarFileEntry.ContainsKey(key)) || (FileManager.pathToVarFileEntry != null && FileManager.pathToVarFileEntry.ContainsKey(key));
		}

		// Token: 0x060056AF RID: 22191 RVA: 0x001FAE48 File Offset: 0x001F9248
		public static bool IsFavorite(string path, bool restrictPath = false)
		{
			FileEntry fileEntry = FileManager.GetVarFileEntry(path);
			if (fileEntry == null)
			{
				fileEntry = FileManager.GetSystemFileEntry(path, restrictPath);
			}
			return fileEntry != null && fileEntry.IsFavorite();
		}

		// Token: 0x060056B0 RID: 22192 RVA: 0x001FAE78 File Offset: 0x001F9278
		public static void SetFavorite(string path, bool fav, bool restrictPath = false)
		{
			FileEntry fileEntry = FileManager.GetVarFileEntry(path);
			if (fileEntry == null)
			{
				fileEntry = FileManager.GetSystemFileEntry(path, restrictPath);
			}
			if (fileEntry != null)
			{
				fileEntry.SetFavorite(fav);
			}
		}

		// Token: 0x060056B1 RID: 22193 RVA: 0x001FAEA8 File Offset: 0x001F92A8
		public static bool IsHidden(string path, bool restrictPath = false)
		{
			FileEntry fileEntry = FileManager.GetVarFileEntry(path);
			if (fileEntry == null)
			{
				fileEntry = FileManager.GetSystemFileEntry(path, restrictPath);
			}
			return fileEntry != null && fileEntry.IsHidden();
		}

		// Token: 0x060056B2 RID: 22194 RVA: 0x001FAED8 File Offset: 0x001F92D8
		public static void SetHidden(string path, bool hide, bool restrictPath = false)
		{
			FileEntry fileEntry = FileManager.GetVarFileEntry(path);
			if (fileEntry == null)
			{
				fileEntry = FileManager.GetSystemFileEntry(path, restrictPath);
			}
			if (fileEntry != null)
			{
				fileEntry.SetHidden(hide);
			}
		}

		// Token: 0x060056B3 RID: 22195 RVA: 0x001FAF08 File Offset: 0x001F9308
		public static FileEntry GetFileEntry(string path, bool restrictPath = false)
		{
			FileEntry fileEntry = FileManager.GetVarFileEntry(path);
			if (fileEntry == null)
			{
				fileEntry = FileManager.GetSystemFileEntry(path, restrictPath);
			}
			return fileEntry;
		}

		// Token: 0x060056B4 RID: 22196 RVA: 0x001FAF30 File Offset: 0x001F9330
		public static SystemFileEntry GetSystemFileEntry(string path, bool restrictPath = false)
		{
			SystemFileEntry result = null;
			if (File.Exists(path))
			{
				if (restrictPath && !FileManager.IsSecureReadPath(path))
				{
					throw new Exception("Attempted to get file entry for non-secure path " + path);
				}
				result = new SystemFileEntry(path);
			}
			return result;
		}

		// Token: 0x060056B5 RID: 22197 RVA: 0x001FAF74 File Offset: 0x001F9374
		public static VarFileEntry GetVarFileEntry(string path)
		{
			VarFileEntry result = null;
			string key = FileManager.CleanFilePath(path);
			if (FileManager.uidToVarFileEntry == null || !FileManager.uidToVarFileEntry.TryGetValue(key, out result))
			{
				if (FileManager.pathToVarFileEntry == null || FileManager.pathToVarFileEntry.TryGetValue(key, out result))
				{
				}
			}
			return result;
		}

		// Token: 0x060056B6 RID: 22198 RVA: 0x001FAFC8 File Offset: 0x001F93C8
		public static void SortFileEntriesByLastWriteTime(List<FileEntry> fileEntries)
		{
			if (FileManager.<>f__am$cache0 == null)
			{
				FileManager.<>f__am$cache0 = new Comparison<FileEntry>(FileManager.<SortFileEntriesByLastWriteTime>m__0);
			}
			fileEntries.Sort(FileManager.<>f__am$cache0);
		}

		// Token: 0x060056B7 RID: 22199 RVA: 0x001FAFF0 File Offset: 0x001F93F0
		public static string CleanDirectoryPath(string path)
		{
			if (path != null)
			{
				string input = path.Replace('\\', '/');
				return Regex.Replace(input, "/$", string.Empty);
			}
			return null;
		}

		// Token: 0x060056B8 RID: 22200 RVA: 0x001FB024 File Offset: 0x001F9424
		public static int FolderContentsCount(string path)
		{
			int num = Directory.GetFiles(path).Length;
			string[] directories = Directory.GetDirectories(path);
			foreach (string path2 in directories)
			{
				num += FileManager.FolderContentsCount(path2);
			}
			return num;
		}

		// Token: 0x060056B9 RID: 22201 RVA: 0x001FB06C File Offset: 0x001F946C
		public static List<VarDirectoryEntry> FindVarDirectories(string dir, bool exactMatch = true)
		{
			dir = FileManager.CleanDirectoryPath(dir);
			List<VarDirectoryEntry> list = new List<VarDirectoryEntry>();
			if (FileManager.allVarDirectoryEntries != null)
			{
				foreach (VarDirectoryEntry varDirectoryEntry in FileManager.allVarDirectoryEntries)
				{
					if (exactMatch)
					{
						if (varDirectoryEntry.InternalSlashPath == dir)
						{
							list.Add(varDirectoryEntry);
						}
					}
					else if (varDirectoryEntry.InternalSlashPath.StartsWith(dir))
					{
						list.Add(varDirectoryEntry);
					}
				}
			}
			return list;
		}

		// Token: 0x060056BA RID: 22202 RVA: 0x001FB114 File Offset: 0x001F9514
		public static List<ShortCut> GetShortCutsForDirectory(string dir, bool allowNavigationAboveRegularDirectories = false, bool useFullPaths = false, bool generateAllFlattenedShortcut = false, bool includeRegularDirsInFlattenedShortcut = false)
		{
			dir = Regex.Replace(dir, ".*:\\\\", string.Empty);
			string text = dir.TrimEnd(new char[]
			{
				'/',
				'\\'
			});
			text = text.Replace('\\', '/');
			List<VarDirectoryEntry> list = FileManager.FindVarDirectories(text, true);
			List<ShortCut> list2 = new List<ShortCut>();
			if (FileManager.DirectoryExists(text, false, false))
			{
				ShortCut shortCut = new ShortCut();
				shortCut.package = string.Empty;
				if (allowNavigationAboveRegularDirectories)
				{
					text = text.Replace('/', '\\');
					if (useFullPaths)
					{
						shortCut.path = Path.GetFullPath(text);
					}
					else
					{
						shortCut.path = text;
					}
				}
				else
				{
					shortCut.path = text;
				}
				shortCut.displayName = text;
				list2.Add(shortCut);
			}
			if (list.Count > 0)
			{
				if (generateAllFlattenedShortcut)
				{
					if (includeRegularDirsInFlattenedShortcut)
					{
						list2.Add(new ShortCut
						{
							path = text,
							displayName = "From: " + text,
							flatten = true,
							package = "All Flattened",
							includeRegularDirsInFlatten = true
						});
					}
					list2.Add(new ShortCut
					{
						path = text,
						displayName = "From: " + text,
						flatten = true,
						package = "AddonPackages Flattened"
					});
				}
				list2.Add(new ShortCut
				{
					package = "AddonPackages Filtered",
					path = "AddonPackages",
					displayName = "Filter: " + text,
					packageFilter = text
				});
			}
			foreach (VarDirectoryEntry varDirectoryEntry in list)
			{
				list2.Add(new ShortCut
				{
					directoryEntry = varDirectoryEntry,
					isLatest = varDirectoryEntry.Package.isNewestEnabledVersion,
					package = varDirectoryEntry.Package.Uid,
					creator = varDirectoryEntry.Package.Creator,
					displayName = varDirectoryEntry.InternalSlashPath,
					path = varDirectoryEntry.SlashPath
				});
			}
			return list2;
		}

		// Token: 0x060056BB RID: 22203 RVA: 0x001FB35C File Offset: 0x001F975C
		public static bool DirectoryExists(string path, bool onlySystemDirectories = false, bool restrictPath = false)
		{
			if (path != null && path != string.Empty)
			{
				if (!onlySystemDirectories)
				{
					string key = FileManager.CleanDirectoryPath(path);
					if (FileManager.uidToVarDirectoryEntry != null && FileManager.uidToVarDirectoryEntry.ContainsKey(key))
					{
						return true;
					}
					if (FileManager.pathToVarDirectoryEntry != null && FileManager.pathToVarDirectoryEntry.ContainsKey(key))
					{
						return true;
					}
				}
				if (Directory.Exists(path))
				{
					if (restrictPath && !FileManager.IsSecureReadPath(path))
					{
						throw new Exception("Attempted to check file existence for non-secure path " + path);
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x060056BC RID: 22204 RVA: 0x001FB3F4 File Offset: 0x001F97F4
		public static DateTime DirectoryLastWriteTime(string path, bool onlySystemDirectories = false, bool restrictPath = false)
		{
			if (path != null && path != string.Empty)
			{
				if (!onlySystemDirectories)
				{
					string key = FileManager.CleanFilePath(path);
					VarDirectoryEntry varDirectoryEntry;
					if (FileManager.uidToVarDirectoryEntry != null && FileManager.uidToVarDirectoryEntry.TryGetValue(path, out varDirectoryEntry))
					{
						return varDirectoryEntry.LastWriteTime;
					}
					if (FileManager.pathToVarDirectoryEntry != null && FileManager.pathToVarDirectoryEntry.TryGetValue(key, out varDirectoryEntry))
					{
						return varDirectoryEntry.LastWriteTime;
					}
				}
				if (Directory.Exists(path))
				{
					if (restrictPath && !FileManager.IsSecureReadPath(path))
					{
						throw new Exception("Attempted to check directory last write time for non-secure path " + path);
					}
					DirectoryInfo directoryInfo = new DirectoryInfo(path);
					return directoryInfo.LastWriteTime;
				}
			}
			return DateTime.MinValue;
		}

		// Token: 0x060056BD RID: 22205 RVA: 0x001FB4AC File Offset: 0x001F98AC
		public static DateTime DirectoryCreationTime(string path, bool onlySystemDirectories = false, bool restrictPath = false)
		{
			if (path != null && path != string.Empty)
			{
				if (!onlySystemDirectories)
				{
					string key = FileManager.CleanFilePath(path);
					VarDirectoryEntry varDirectoryEntry;
					if (FileManager.uidToVarDirectoryEntry != null && FileManager.uidToVarDirectoryEntry.TryGetValue(path, out varDirectoryEntry))
					{
						return varDirectoryEntry.LastWriteTime;
					}
					if (FileManager.pathToVarDirectoryEntry != null && FileManager.pathToVarDirectoryEntry.TryGetValue(key, out varDirectoryEntry))
					{
						return varDirectoryEntry.LastWriteTime;
					}
				}
				if (Directory.Exists(path))
				{
					if (restrictPath && !FileManager.IsSecureReadPath(path))
					{
						throw new Exception("Attempted to check directory creation time for non-secure path " + path);
					}
					DirectoryInfo directoryInfo = new DirectoryInfo(path);
					return directoryInfo.CreationTime;
				}
			}
			return DateTime.MinValue;
		}

		// Token: 0x060056BE RID: 22206 RVA: 0x001FB564 File Offset: 0x001F9964
		public static bool IsDirectoryInPackage(string path)
		{
			string key = FileManager.CleanDirectoryPath(path);
			return (FileManager.uidToVarDirectoryEntry != null && FileManager.uidToVarDirectoryEntry.ContainsKey(key)) || (FileManager.pathToVarDirectoryEntry != null && FileManager.pathToVarDirectoryEntry.ContainsKey(key));
		}

		// Token: 0x060056BF RID: 22207 RVA: 0x001FB5B4 File Offset: 0x001F99B4
		public static DirectoryEntry GetDirectoryEntry(string path, bool restrictPath = false)
		{
			string path2 = Regex.Replace(path, "(/|\\\\)$", string.Empty);
			DirectoryEntry directoryEntry = FileManager.GetVarDirectoryEntry(path2);
			if (directoryEntry == null)
			{
				directoryEntry = FileManager.GetSystemDirectoryEntry(path2, restrictPath);
			}
			return directoryEntry;
		}

		// Token: 0x060056C0 RID: 22208 RVA: 0x001FB5EC File Offset: 0x001F99EC
		public static SystemDirectoryEntry GetSystemDirectoryEntry(string path, bool restrictPath = false)
		{
			SystemDirectoryEntry result = null;
			if (Directory.Exists(path))
			{
				if (restrictPath && !FileManager.IsSecureReadPath(path))
				{
					throw new Exception("Attempted to get directory entry for non-secure path " + path);
				}
				result = new SystemDirectoryEntry(path);
			}
			return result;
		}

		// Token: 0x060056C1 RID: 22209 RVA: 0x001FB630 File Offset: 0x001F9A30
		public static VarDirectoryEntry GetVarDirectoryEntry(string path)
		{
			VarDirectoryEntry result = null;
			string key = FileManager.CleanDirectoryPath(path);
			if (FileManager.uidToVarDirectoryEntry == null || !FileManager.uidToVarDirectoryEntry.TryGetValue(key, out result))
			{
				if (FileManager.pathToVarDirectoryEntry == null || FileManager.pathToVarDirectoryEntry.TryGetValue(key, out result))
				{
				}
			}
			return result;
		}

		// Token: 0x060056C2 RID: 22210 RVA: 0x001FB684 File Offset: 0x001F9A84
		public static VarDirectoryEntry GetVarRootDirectoryEntryFromPath(string path)
		{
			VarDirectoryEntry result = null;
			if (FileManager.varPackagePathToRootVarDirectory != null)
			{
				FileManager.varPackagePathToRootVarDirectory.TryGetValue(path, out result);
			}
			return result;
		}

		// Token: 0x060056C3 RID: 22211 RVA: 0x001FB6AC File Offset: 0x001F9AAC
		public static string[] GetDirectories(string dir, string pattern = null, bool restrictPath = false)
		{
			if (restrictPath && !FileManager.IsSecureReadPath(dir))
			{
				throw new Exception("Attempted to get directories at non-secure path " + dir);
			}
			List<string> list = new List<string>();
			DirectoryEntry directoryEntry = FileManager.GetDirectoryEntry(dir, restrictPath);
			if (directoryEntry == null)
			{
				throw new Exception("Attempted to get directories at non-existent path " + dir);
			}
			string text = null;
			if (pattern != null)
			{
				text = "^" + Regex.Escape(pattern).Replace("\\*", ".*").Replace("\\?", ".") + "$";
			}
			foreach (DirectoryEntry directoryEntry2 in directoryEntry.SubDirectories)
			{
				if (text == null || Regex.IsMatch(directoryEntry2.Name, text))
				{
					list.Add(dir + "\\" + directoryEntry2.Name);
				}
			}
			return list.ToArray();
		}

		// Token: 0x060056C4 RID: 22212 RVA: 0x001FB7BC File Offset: 0x001F9BBC
		public static string[] GetFiles(string dir, string pattern = null, bool restrictPath = false)
		{
			if (restrictPath && !FileManager.IsSecureReadPath(dir))
			{
				throw new Exception("Attempted to get files at non-secure path " + dir);
			}
			List<string> list = new List<string>();
			DirectoryEntry directoryEntry = FileManager.GetDirectoryEntry(dir, restrictPath);
			if (directoryEntry == null)
			{
				throw new Exception("Attempted to get files at non-existent path " + dir);
			}
			foreach (FileEntry fileEntry in directoryEntry.GetFiles(pattern))
			{
				list.Add(dir + "\\" + fileEntry.Name);
			}
			return list.ToArray();
		}

		// Token: 0x060056C5 RID: 22213 RVA: 0x001FB878 File Offset: 0x001F9C78
		public static void CreateDirectory(string path)
		{
			path = FileManager.ConvertSimulatedPackagePathToNormalPath(path);
			if (FileManager.DirectoryExists(path, false, false))
			{
				return;
			}
			if (!FileManager.IsSecureWritePath(path))
			{
				throw new Exception("Attempted to create directory at non-secure path " + path);
			}
			Directory.CreateDirectory(path);
		}

		// Token: 0x060056C6 RID: 22214 RVA: 0x001FB8B4 File Offset: 0x001F9CB4
		public static void CreateDirectoryFromPlugin(string path, UserActionCallback confirmCallback, UserActionCallback denyCallback, ExceptionCallback exceptionCallback)
		{
			path = FileManager.ConvertSimulatedPackagePathToNormalPath(path);
			if (FileManager.DirectoryExists(path, false, false))
			{
				return;
			}
			if (FileManager.IsSecurePluginWritePath(path))
			{
				try
				{
					Directory.CreateDirectory(path);
				}
				catch (Exception ex)
				{
					if (exceptionCallback != null)
					{
						exceptionCallback(ex);
						return;
					}
					throw ex;
				}
				if (confirmCallback != null)
				{
					confirmCallback();
				}
				return;
			}
			Exception ex2 = new Exception("Plugin attempted to create directory at non-secure path " + path);
			if (exceptionCallback != null)
			{
				exceptionCallback(ex2);
				return;
			}
			throw ex2;
		}

		// Token: 0x060056C7 RID: 22215 RVA: 0x001FB940 File Offset: 0x001F9D40
		public static void DeleteDirectory(string path, bool recursive = false)
		{
			path = FileManager.ConvertSimulatedPackagePathToNormalPath(path);
			if (!FileManager.DirectoryExists(path, false, false))
			{
				return;
			}
			if (!FileManager.IsSecureWritePath(path))
			{
				throw new Exception("Attempted to delete file at non-secure path " + path);
			}
			Directory.Delete(path, recursive);
		}

		// Token: 0x060056C8 RID: 22216 RVA: 0x001FB97C File Offset: 0x001F9D7C
		public static void DeleteDirectoryFromPlugin(string path, bool recursive, UserActionCallback confirmCallback, UserActionCallback denyCallback, ExceptionCallback exceptionCallback)
		{
			FileManager.<DeleteDirectoryFromPlugin>c__AnonStorey6 <DeleteDirectoryFromPlugin>c__AnonStorey = new FileManager.<DeleteDirectoryFromPlugin>c__AnonStorey6();
			<DeleteDirectoryFromPlugin>c__AnonStorey.path = path;
			<DeleteDirectoryFromPlugin>c__AnonStorey.recursive = recursive;
			<DeleteDirectoryFromPlugin>c__AnonStorey.exceptionCallback = exceptionCallback;
			<DeleteDirectoryFromPlugin>c__AnonStorey.confirmCallback = confirmCallback;
			<DeleteDirectoryFromPlugin>c__AnonStorey.path = FileManager.ConvertSimulatedPackagePathToNormalPath(<DeleteDirectoryFromPlugin>c__AnonStorey.path);
			if (!FileManager.DirectoryExists(<DeleteDirectoryFromPlugin>c__AnonStorey.path, false, false))
			{
				return;
			}
			if (FileManager.IsSecurePluginWritePath(<DeleteDirectoryFromPlugin>c__AnonStorey.path))
			{
				if (!FileManager.IsPluginWritePathThatNeedsConfirm(<DeleteDirectoryFromPlugin>c__AnonStorey.path))
				{
					try
					{
						Directory.Delete(<DeleteDirectoryFromPlugin>c__AnonStorey.path, <DeleteDirectoryFromPlugin>c__AnonStorey.recursive);
					}
					catch (Exception ex)
					{
						if (<DeleteDirectoryFromPlugin>c__AnonStorey.exceptionCallback != null)
						{
							<DeleteDirectoryFromPlugin>c__AnonStorey.exceptionCallback(ex);
							return;
						}
						throw ex;
					}
					if (<DeleteDirectoryFromPlugin>c__AnonStorey.confirmCallback != null)
					{
						<DeleteDirectoryFromPlugin>c__AnonStorey.confirmCallback();
					}
				}
				else
				{
					FileManager.ConfirmPluginActionWithUser("delete directory at " + <DeleteDirectoryFromPlugin>c__AnonStorey.path, new UserActionCallback(<DeleteDirectoryFromPlugin>c__AnonStorey.<>m__0), denyCallback);
				}
				return;
			}
			Exception ex2 = new Exception("Plugin attempted to delete directory at non-secure path " + <DeleteDirectoryFromPlugin>c__AnonStorey.path);
			if (<DeleteDirectoryFromPlugin>c__AnonStorey.exceptionCallback != null)
			{
				<DeleteDirectoryFromPlugin>c__AnonStorey.exceptionCallback(ex2);
				return;
			}
			throw ex2;
		}

		// Token: 0x060056C9 RID: 22217 RVA: 0x001FBAA4 File Offset: 0x001F9EA4
		public static void MoveDirectory(string oldPath, string newPath)
		{
			oldPath = FileManager.ConvertSimulatedPackagePathToNormalPath(oldPath);
			if (!FileManager.IsSecureWritePath(oldPath))
			{
				throw new Exception("Attempted to move directory from non-secure path " + oldPath);
			}
			newPath = FileManager.ConvertSimulatedPackagePathToNormalPath(newPath);
			if (!FileManager.IsSecureWritePath(newPath))
			{
				throw new Exception("Attempted to move directory to non-secure path " + newPath);
			}
			Directory.Move(oldPath, newPath);
		}

		// Token: 0x060056CA RID: 22218 RVA: 0x001FBB00 File Offset: 0x001F9F00
		public static void MoveDirectoryFromPlugin(string oldPath, string newPath, UserActionCallback confirmCallback, UserActionCallback denyCallback, ExceptionCallback exceptionCallback)
		{
			FileManager.<MoveDirectoryFromPlugin>c__AnonStorey7 <MoveDirectoryFromPlugin>c__AnonStorey = new FileManager.<MoveDirectoryFromPlugin>c__AnonStorey7();
			<MoveDirectoryFromPlugin>c__AnonStorey.oldPath = oldPath;
			<MoveDirectoryFromPlugin>c__AnonStorey.newPath = newPath;
			<MoveDirectoryFromPlugin>c__AnonStorey.exceptionCallback = exceptionCallback;
			<MoveDirectoryFromPlugin>c__AnonStorey.confirmCallback = confirmCallback;
			<MoveDirectoryFromPlugin>c__AnonStorey.oldPath = FileManager.ConvertSimulatedPackagePathToNormalPath(<MoveDirectoryFromPlugin>c__AnonStorey.oldPath);
			if (!FileManager.IsSecurePluginWritePath(<MoveDirectoryFromPlugin>c__AnonStorey.oldPath))
			{
				Exception ex = new Exception("Plugin attempted to move directory from non-secure path " + <MoveDirectoryFromPlugin>c__AnonStorey.oldPath);
				if (<MoveDirectoryFromPlugin>c__AnonStorey.exceptionCallback != null)
				{
					<MoveDirectoryFromPlugin>c__AnonStorey.exceptionCallback(ex);
					return;
				}
				throw ex;
			}
			else
			{
				<MoveDirectoryFromPlugin>c__AnonStorey.newPath = FileManager.ConvertSimulatedPackagePathToNormalPath(<MoveDirectoryFromPlugin>c__AnonStorey.newPath);
				if (FileManager.IsSecurePluginWritePath(<MoveDirectoryFromPlugin>c__AnonStorey.newPath))
				{
					if (!FileManager.IsPluginWritePathThatNeedsConfirm(<MoveDirectoryFromPlugin>c__AnonStorey.oldPath) && !FileManager.IsPluginWritePathThatNeedsConfirm(<MoveDirectoryFromPlugin>c__AnonStorey.newPath))
					{
						try
						{
							Directory.Move(<MoveDirectoryFromPlugin>c__AnonStorey.oldPath, <MoveDirectoryFromPlugin>c__AnonStorey.newPath);
						}
						catch (Exception ex2)
						{
							if (<MoveDirectoryFromPlugin>c__AnonStorey.exceptionCallback != null)
							{
								<MoveDirectoryFromPlugin>c__AnonStorey.exceptionCallback(ex2);
								return;
							}
							throw ex2;
						}
						if (<MoveDirectoryFromPlugin>c__AnonStorey.confirmCallback != null)
						{
							<MoveDirectoryFromPlugin>c__AnonStorey.confirmCallback();
						}
					}
					else
					{
						FileManager.ConfirmPluginActionWithUser("move directory from " + <MoveDirectoryFromPlugin>c__AnonStorey.oldPath + " to " + <MoveDirectoryFromPlugin>c__AnonStorey.newPath, new UserActionCallback(<MoveDirectoryFromPlugin>c__AnonStorey.<>m__0), denyCallback);
					}
					return;
				}
				Exception ex3 = new Exception("Plugin attempted to move directory to non-secure path " + <MoveDirectoryFromPlugin>c__AnonStorey.newPath);
				if (<MoveDirectoryFromPlugin>c__AnonStorey.exceptionCallback != null)
				{
					<MoveDirectoryFromPlugin>c__AnonStorey.exceptionCallback(ex3);
					return;
				}
				throw ex3;
			}
		}

		// Token: 0x060056CB RID: 22219 RVA: 0x001FBC80 File Offset: 0x001FA080
		public static FileEntryStream OpenStream(FileEntry fe)
		{
			if (fe == null)
			{
				throw new Exception("Null FileEntry passed to OpenStreamReader");
			}
			if (fe is VarFileEntry)
			{
				return new VarFileEntryStream(fe as VarFileEntry);
			}
			if (fe is SystemFileEntry)
			{
				return new SystemFileEntryStream(fe as SystemFileEntry);
			}
			throw new Exception("Unknown FileEntry class passed to OpenStreamReader");
		}

		// Token: 0x060056CC RID: 22220 RVA: 0x001FBCD8 File Offset: 0x001FA0D8
		public static FileEntryStream OpenStream(string path, bool restrictPath = false)
		{
			FileEntry fileEntry = FileManager.GetFileEntry(path, restrictPath);
			if (fileEntry == null)
			{
				throw new Exception("Path " + path + " not found");
			}
			return FileManager.OpenStream(fileEntry);
		}

		// Token: 0x060056CD RID: 22221 RVA: 0x001FBD10 File Offset: 0x001FA110
		public static FileEntryStreamReader OpenStreamReader(FileEntry fe)
		{
			if (fe == null)
			{
				throw new Exception("Null FileEntry passed to OpenStreamReader");
			}
			if (fe is VarFileEntry)
			{
				return new VarFileEntryStreamReader(fe as VarFileEntry);
			}
			if (fe is SystemFileEntry)
			{
				return new SystemFileEntryStreamReader(fe as SystemFileEntry);
			}
			throw new Exception("Unknown FileEntry class passed to OpenStreamReader");
		}

		// Token: 0x060056CE RID: 22222 RVA: 0x001FBD68 File Offset: 0x001FA168
		public static FileEntryStreamReader OpenStreamReader(string path, bool restrictPath = false)
		{
			FileEntry fileEntry = FileManager.GetFileEntry(path, restrictPath);
			if (fileEntry == null)
			{
				throw new Exception("Path " + path + " not found");
			}
			return FileManager.OpenStreamReader(fileEntry);
		}

		// Token: 0x060056CF RID: 22223 RVA: 0x001FBDA0 File Offset: 0x001FA1A0
		public static IEnumerator ReadAllBytesCoroutine(FileEntry fe, byte[] result)
		{
			FileManager.<ReadAllBytesCoroutine>c__Iterator0.<ReadAllBytesCoroutine>c__AnonStorey8 <ReadAllBytesCoroutine>c__AnonStorey = new FileManager.<ReadAllBytesCoroutine>c__Iterator0.<ReadAllBytesCoroutine>c__AnonStorey8();
			<ReadAllBytesCoroutine>c__AnonStorey.fe = fe;
			<ReadAllBytesCoroutine>c__AnonStorey.result = result;
			Thread loadThread = new Thread(new ThreadStart(<ReadAllBytesCoroutine>c__AnonStorey.<>m__0));
			loadThread.Start();
			while (loadThread.IsAlive)
			{
				yield return null;
			}
			yield break;
		}

		// Token: 0x060056D0 RID: 22224 RVA: 0x001FBDC4 File Offset: 0x001FA1C4
		public static byte[] ReadAllBytes(string path, bool restrictPath = false)
		{
			FileEntry fileEntry = FileManager.GetFileEntry(path, restrictPath);
			if (fileEntry == null)
			{
				throw new Exception("Path " + path + " not found");
			}
			return FileManager.ReadAllBytes(fileEntry);
		}

		// Token: 0x060056D1 RID: 22225 RVA: 0x001FBDFC File Offset: 0x001FA1FC
		public static byte[] ReadAllBytes(FileEntry fe)
		{
			if (fe is VarFileEntry)
			{
				byte[] buffer = new byte[32768];
				using (FileEntryStream fileEntryStream = FileManager.OpenStream(fe))
				{
					byte[] array = new byte[fe.Size];
					using (MemoryStream memoryStream = new MemoryStream(array))
					{
						StreamUtils.Copy(fileEntryStream.Stream, memoryStream, buffer);
					}
					return array;
				}
			}
			return File.ReadAllBytes(fe.FullPath);
		}

		// Token: 0x060056D2 RID: 22226 RVA: 0x001FBE98 File Offset: 0x001FA298
		public static string ReadAllText(string path, bool restrictPath = false)
		{
			FileEntry fileEntry = FileManager.GetFileEntry(path, restrictPath);
			if (fileEntry == null)
			{
				throw new Exception("Path " + path + " not found");
			}
			return FileManager.ReadAllText(fileEntry);
		}

		// Token: 0x060056D3 RID: 22227 RVA: 0x001FBED0 File Offset: 0x001FA2D0
		public static string ReadAllText(FileEntry fe)
		{
			string result;
			using (FileEntryStreamReader fileEntryStreamReader = FileManager.OpenStreamReader(fe))
			{
				result = fileEntryStreamReader.ReadToEnd();
			}
			return result;
		}

		// Token: 0x060056D4 RID: 22228 RVA: 0x001FBF10 File Offset: 0x001FA310
		public static FileStream OpenStreamForCreate(string path)
		{
			path = FileManager.ConvertSimulatedPackagePathToNormalPath(path);
			if (!FileManager.IsSecureWritePath(path))
			{
				throw new Exception("Attempted to open stream for create at non-secure path " + path);
			}
			return File.Open(path, FileMode.Create);
		}

		// Token: 0x060056D5 RID: 22229 RVA: 0x001FBF4C File Offset: 0x001FA34C
		public static StreamWriter OpenStreamWriter(string path)
		{
			path = FileManager.ConvertSimulatedPackagePathToNormalPath(path);
			if (!FileManager.IsSecureWritePath(path))
			{
				throw new Exception("Attempted to open stream writer at non-secure path " + path);
			}
			return new StreamWriter(path);
		}

		// Token: 0x060056D6 RID: 22230 RVA: 0x001FBF85 File Offset: 0x001FA385
		public static void WriteAllText(string path, string text)
		{
			path = FileManager.ConvertSimulatedPackagePathToNormalPath(path);
			if (!FileManager.IsSecureWritePath(path))
			{
				throw new Exception("Attempted to write all text at non-secure path " + path);
			}
			File.WriteAllText(path, text);
		}

		// Token: 0x060056D7 RID: 22231 RVA: 0x001FBFB4 File Offset: 0x001FA3B4
		public static void WriteAllTextFromPlugin(string path, string text, UserActionCallback confirmCallback, UserActionCallback denyCallback, ExceptionCallback exceptionCallback)
		{
			FileManager.<WriteAllTextFromPlugin>c__AnonStorey9 <WriteAllTextFromPlugin>c__AnonStorey = new FileManager.<WriteAllTextFromPlugin>c__AnonStorey9();
			<WriteAllTextFromPlugin>c__AnonStorey.path = path;
			<WriteAllTextFromPlugin>c__AnonStorey.text = text;
			<WriteAllTextFromPlugin>c__AnonStorey.exceptionCallback = exceptionCallback;
			<WriteAllTextFromPlugin>c__AnonStorey.confirmCallback = confirmCallback;
			<WriteAllTextFromPlugin>c__AnonStorey.path = FileManager.ConvertSimulatedPackagePathToNormalPath(<WriteAllTextFromPlugin>c__AnonStorey.path);
			if (FileManager.IsSecurePluginWritePath(<WriteAllTextFromPlugin>c__AnonStorey.path))
			{
				if (File.Exists(<WriteAllTextFromPlugin>c__AnonStorey.path))
				{
					if (!FileManager.IsPluginWritePathThatNeedsConfirm(<WriteAllTextFromPlugin>c__AnonStorey.path))
					{
						try
						{
							File.WriteAllText(<WriteAllTextFromPlugin>c__AnonStorey.path, <WriteAllTextFromPlugin>c__AnonStorey.text);
						}
						catch (Exception ex)
						{
							if (<WriteAllTextFromPlugin>c__AnonStorey.exceptionCallback != null)
							{
								<WriteAllTextFromPlugin>c__AnonStorey.exceptionCallback(ex);
								return;
							}
							throw ex;
						}
						if (<WriteAllTextFromPlugin>c__AnonStorey.confirmCallback != null)
						{
							<WriteAllTextFromPlugin>c__AnonStorey.confirmCallback();
						}
					}
					else
					{
						FileManager.ConfirmPluginActionWithUser("overwrite file " + <WriteAllTextFromPlugin>c__AnonStorey.path, new UserActionCallback(<WriteAllTextFromPlugin>c__AnonStorey.<>m__0), denyCallback);
					}
				}
				else
				{
					try
					{
						File.WriteAllText(<WriteAllTextFromPlugin>c__AnonStorey.path, <WriteAllTextFromPlugin>c__AnonStorey.text);
					}
					catch (Exception ex2)
					{
						if (<WriteAllTextFromPlugin>c__AnonStorey.exceptionCallback != null)
						{
							<WriteAllTextFromPlugin>c__AnonStorey.exceptionCallback(ex2);
							return;
						}
						throw ex2;
					}
					if (<WriteAllTextFromPlugin>c__AnonStorey.confirmCallback != null)
					{
						<WriteAllTextFromPlugin>c__AnonStorey.confirmCallback();
					}
				}
				return;
			}
			Exception ex3 = new Exception("Plugin attempted to write all text at non-secure path " + <WriteAllTextFromPlugin>c__AnonStorey.path);
			if (<WriteAllTextFromPlugin>c__AnonStorey.exceptionCallback != null)
			{
				<WriteAllTextFromPlugin>c__AnonStorey.exceptionCallback(ex3);
				return;
			}
			throw ex3;
		}

		// Token: 0x060056D8 RID: 22232 RVA: 0x001FC134 File Offset: 0x001FA534
		public static void WriteAllBytes(string path, byte[] bytes)
		{
			path = FileManager.ConvertSimulatedPackagePathToNormalPath(path);
			if (!FileManager.IsSecureWritePath(path))
			{
				throw new Exception("Attempted to write all bytes at non-secure path " + path);
			}
			File.WriteAllBytes(path, bytes);
		}

		// Token: 0x060056D9 RID: 22233 RVA: 0x001FC164 File Offset: 0x001FA564
		public static void WriteAllBytesFromPlugin(string path, byte[] bytes, UserActionCallback confirmCallback, UserActionCallback denyCallback, ExceptionCallback exceptionCallback)
		{
			FileManager.<WriteAllBytesFromPlugin>c__AnonStoreyA <WriteAllBytesFromPlugin>c__AnonStoreyA = new FileManager.<WriteAllBytesFromPlugin>c__AnonStoreyA();
			<WriteAllBytesFromPlugin>c__AnonStoreyA.path = path;
			<WriteAllBytesFromPlugin>c__AnonStoreyA.bytes = bytes;
			<WriteAllBytesFromPlugin>c__AnonStoreyA.exceptionCallback = exceptionCallback;
			<WriteAllBytesFromPlugin>c__AnonStoreyA.confirmCallback = confirmCallback;
			<WriteAllBytesFromPlugin>c__AnonStoreyA.path = FileManager.ConvertSimulatedPackagePathToNormalPath(<WriteAllBytesFromPlugin>c__AnonStoreyA.path);
			if (FileManager.IsSecurePluginWritePath(<WriteAllBytesFromPlugin>c__AnonStoreyA.path))
			{
				if (File.Exists(<WriteAllBytesFromPlugin>c__AnonStoreyA.path))
				{
					if (!FileManager.IsPluginWritePathThatNeedsConfirm(<WriteAllBytesFromPlugin>c__AnonStoreyA.path))
					{
						try
						{
							File.WriteAllBytes(<WriteAllBytesFromPlugin>c__AnonStoreyA.path, <WriteAllBytesFromPlugin>c__AnonStoreyA.bytes);
						}
						catch (Exception ex)
						{
							if (<WriteAllBytesFromPlugin>c__AnonStoreyA.exceptionCallback != null)
							{
								<WriteAllBytesFromPlugin>c__AnonStoreyA.exceptionCallback(ex);
								return;
							}
							throw ex;
						}
						if (<WriteAllBytesFromPlugin>c__AnonStoreyA.confirmCallback != null)
						{
							<WriteAllBytesFromPlugin>c__AnonStoreyA.confirmCallback();
						}
					}
					else
					{
						FileManager.ConfirmPluginActionWithUser("overwrite file " + <WriteAllBytesFromPlugin>c__AnonStoreyA.path, new UserActionCallback(<WriteAllBytesFromPlugin>c__AnonStoreyA.<>m__0), denyCallback);
					}
				}
				else
				{
					try
					{
						File.WriteAllBytes(<WriteAllBytesFromPlugin>c__AnonStoreyA.path, <WriteAllBytesFromPlugin>c__AnonStoreyA.bytes);
					}
					catch (Exception ex2)
					{
						if (<WriteAllBytesFromPlugin>c__AnonStoreyA.exceptionCallback != null)
						{
							<WriteAllBytesFromPlugin>c__AnonStoreyA.exceptionCallback(ex2);
							return;
						}
						throw ex2;
					}
					if (<WriteAllBytesFromPlugin>c__AnonStoreyA.confirmCallback != null)
					{
						<WriteAllBytesFromPlugin>c__AnonStoreyA.confirmCallback();
					}
				}
				return;
			}
			Exception ex3 = new Exception("Plugin attempted to write all bytes at non-secure path " + <WriteAllBytesFromPlugin>c__AnonStoreyA.path);
			if (<WriteAllBytesFromPlugin>c__AnonStoreyA.exceptionCallback != null)
			{
				<WriteAllBytesFromPlugin>c__AnonStoreyA.exceptionCallback(ex3);
				return;
			}
			throw ex3;
		}

		// Token: 0x060056DA RID: 22234 RVA: 0x001FC2E4 File Offset: 0x001FA6E4
		public static void SetFileAttributes(string path, FileAttributes attrs)
		{
			path = FileManager.ConvertSimulatedPackagePathToNormalPath(path);
			if (!FileManager.IsSecureWritePath(path))
			{
				throw new Exception("Attempted to set file attributes at non-secure path " + path);
			}
			File.SetAttributes(path, attrs);
		}

		// Token: 0x060056DB RID: 22235 RVA: 0x001FC311 File Offset: 0x001FA711
		public static void DeleteFile(string path)
		{
			path = FileManager.ConvertSimulatedPackagePathToNormalPath(path);
			if (!File.Exists(path))
			{
				return;
			}
			if (!FileManager.IsSecureWritePath(path))
			{
				throw new Exception("Attempted to delete file at non-secure path " + path);
			}
			File.Delete(path);
		}

		// Token: 0x060056DC RID: 22236 RVA: 0x001FC34C File Offset: 0x001FA74C
		public static void DeleteFileFromPlugin(string path, UserActionCallback confirmCallback, UserActionCallback denyCallback, ExceptionCallback exceptionCallback)
		{
			FileManager.<DeleteFileFromPlugin>c__AnonStoreyB <DeleteFileFromPlugin>c__AnonStoreyB = new FileManager.<DeleteFileFromPlugin>c__AnonStoreyB();
			<DeleteFileFromPlugin>c__AnonStoreyB.path = path;
			<DeleteFileFromPlugin>c__AnonStoreyB.exceptionCallback = exceptionCallback;
			<DeleteFileFromPlugin>c__AnonStoreyB.confirmCallback = confirmCallback;
			<DeleteFileFromPlugin>c__AnonStoreyB.path = FileManager.ConvertSimulatedPackagePathToNormalPath(<DeleteFileFromPlugin>c__AnonStoreyB.path);
			if (!File.Exists(<DeleteFileFromPlugin>c__AnonStoreyB.path))
			{
				return;
			}
			if (FileManager.IsSecurePluginWritePath(<DeleteFileFromPlugin>c__AnonStoreyB.path))
			{
				if (!FileManager.IsPluginWritePathThatNeedsConfirm(<DeleteFileFromPlugin>c__AnonStoreyB.path))
				{
					try
					{
						File.Delete(<DeleteFileFromPlugin>c__AnonStoreyB.path);
					}
					catch (Exception ex)
					{
						if (<DeleteFileFromPlugin>c__AnonStoreyB.exceptionCallback != null)
						{
							<DeleteFileFromPlugin>c__AnonStoreyB.exceptionCallback(ex);
							return;
						}
						throw ex;
					}
					if (<DeleteFileFromPlugin>c__AnonStoreyB.confirmCallback != null)
					{
						<DeleteFileFromPlugin>c__AnonStoreyB.confirmCallback();
					}
				}
				else
				{
					FileManager.ConfirmPluginActionWithUser("delete file " + <DeleteFileFromPlugin>c__AnonStoreyB.path, new UserActionCallback(<DeleteFileFromPlugin>c__AnonStoreyB.<>m__0), denyCallback);
				}
				return;
			}
			Exception ex2 = new Exception("Plugin attempted to delete file at non-secure path " + <DeleteFileFromPlugin>c__AnonStoreyB.path);
			if (<DeleteFileFromPlugin>c__AnonStoreyB.exceptionCallback != null)
			{
				<DeleteFileFromPlugin>c__AnonStoreyB.exceptionCallback(ex2);
				return;
			}
			throw ex2;
		}

		// Token: 0x060056DD RID: 22237 RVA: 0x001FC464 File Offset: 0x001FA864
		protected static void DoFileCopy(string oldPath, string newPath)
		{
			FileEntry fileEntry = FileManager.GetFileEntry(oldPath, false);
			if (fileEntry != null && fileEntry is VarFileEntry)
			{
				byte[] buffer = new byte[4096];
				using (FileEntryStream fileEntryStream = FileManager.OpenStream(fileEntry))
				{
					using (FileStream fileStream = FileManager.OpenStreamForCreate(newPath))
					{
						StreamUtils.Copy(fileEntryStream.Stream, fileStream, buffer);
					}
				}
			}
			else
			{
				File.Copy(oldPath, newPath);
			}
		}

		// Token: 0x060056DE RID: 22238 RVA: 0x001FC4FC File Offset: 0x001FA8FC
		public static void CopyFile(string oldPath, string newPath, bool restrictPath = false)
		{
			oldPath = FileManager.ConvertSimulatedPackagePathToNormalPath(oldPath);
			if (restrictPath && !FileManager.IsSecureReadPath(oldPath))
			{
				throw new Exception("Attempted to copy file from non-secure path " + oldPath);
			}
			newPath = FileManager.ConvertSimulatedPackagePathToNormalPath(newPath);
			if (!FileManager.IsSecureWritePath(newPath))
			{
				throw new Exception("Attempted to copy file to non-secure path " + newPath);
			}
			FileManager.DoFileCopy(oldPath, newPath);
		}

		// Token: 0x060056DF RID: 22239 RVA: 0x001FC560 File Offset: 0x001FA960
		public static void CopyFileFromPlugin(string oldPath, string newPath, UserActionCallback confirmCallback, UserActionCallback denyCallback, ExceptionCallback exceptionCallback)
		{
			FileManager.<CopyFileFromPlugin>c__AnonStoreyC <CopyFileFromPlugin>c__AnonStoreyC = new FileManager.<CopyFileFromPlugin>c__AnonStoreyC();
			<CopyFileFromPlugin>c__AnonStoreyC.oldPath = oldPath;
			<CopyFileFromPlugin>c__AnonStoreyC.newPath = newPath;
			<CopyFileFromPlugin>c__AnonStoreyC.exceptionCallback = exceptionCallback;
			<CopyFileFromPlugin>c__AnonStoreyC.confirmCallback = confirmCallback;
			<CopyFileFromPlugin>c__AnonStoreyC.oldPath = FileManager.ConvertSimulatedPackagePathToNormalPath(<CopyFileFromPlugin>c__AnonStoreyC.oldPath);
			if (!FileManager.IsSecureReadPath(<CopyFileFromPlugin>c__AnonStoreyC.oldPath))
			{
				Exception ex = new Exception("Attempted to copy file from non-secure path " + <CopyFileFromPlugin>c__AnonStoreyC.oldPath);
				if (<CopyFileFromPlugin>c__AnonStoreyC.exceptionCallback != null)
				{
					<CopyFileFromPlugin>c__AnonStoreyC.exceptionCallback(ex);
					return;
				}
				throw ex;
			}
			else
			{
				<CopyFileFromPlugin>c__AnonStoreyC.newPath = FileManager.ConvertSimulatedPackagePathToNormalPath(<CopyFileFromPlugin>c__AnonStoreyC.newPath);
				if (FileManager.IsSecurePluginWritePath(<CopyFileFromPlugin>c__AnonStoreyC.newPath))
				{
					if (File.Exists(<CopyFileFromPlugin>c__AnonStoreyC.newPath))
					{
						if (!FileManager.IsPluginWritePathThatNeedsConfirm(<CopyFileFromPlugin>c__AnonStoreyC.newPath))
						{
							try
							{
								FileManager.DoFileCopy(<CopyFileFromPlugin>c__AnonStoreyC.oldPath, <CopyFileFromPlugin>c__AnonStoreyC.newPath);
							}
							catch (Exception ex2)
							{
								if (<CopyFileFromPlugin>c__AnonStoreyC.exceptionCallback != null)
								{
									<CopyFileFromPlugin>c__AnonStoreyC.exceptionCallback(ex2);
									return;
								}
								throw ex2;
							}
							if (<CopyFileFromPlugin>c__AnonStoreyC.confirmCallback != null)
							{
								<CopyFileFromPlugin>c__AnonStoreyC.confirmCallback();
							}
						}
						else
						{
							FileManager.ConfirmPluginActionWithUser("copy file from " + <CopyFileFromPlugin>c__AnonStoreyC.oldPath + " to existing file " + <CopyFileFromPlugin>c__AnonStoreyC.newPath, new UserActionCallback(<CopyFileFromPlugin>c__AnonStoreyC.<>m__0), denyCallback);
						}
					}
					else
					{
						try
						{
							FileManager.DoFileCopy(<CopyFileFromPlugin>c__AnonStoreyC.oldPath, <CopyFileFromPlugin>c__AnonStoreyC.newPath);
						}
						catch (Exception ex3)
						{
							if (<CopyFileFromPlugin>c__AnonStoreyC.exceptionCallback != null)
							{
								<CopyFileFromPlugin>c__AnonStoreyC.exceptionCallback(ex3);
								return;
							}
							throw ex3;
						}
						if (<CopyFileFromPlugin>c__AnonStoreyC.confirmCallback != null)
						{
							<CopyFileFromPlugin>c__AnonStoreyC.confirmCallback();
						}
					}
					return;
				}
				Exception ex4 = new Exception("Plugin attempted to copy file to non-secure path " + <CopyFileFromPlugin>c__AnonStoreyC.newPath);
				if (<CopyFileFromPlugin>c__AnonStoreyC.exceptionCallback != null)
				{
					<CopyFileFromPlugin>c__AnonStoreyC.exceptionCallback(ex4);
					return;
				}
				throw ex4;
			}
		}

		// Token: 0x060056E0 RID: 22240 RVA: 0x001FC740 File Offset: 0x001FAB40
		protected static void DoFileMove(string oldPath, string newPath, bool overwrite = true)
		{
			if (File.Exists(newPath))
			{
				if (!overwrite)
				{
					throw new Exception("File " + newPath + " exists. Cannot move into");
				}
				File.Delete(newPath);
			}
			File.Move(oldPath, newPath);
		}

		// Token: 0x060056E1 RID: 22241 RVA: 0x001FC77C File Offset: 0x001FAB7C
		public static void MoveFile(string oldPath, string newPath, bool overwrite = true)
		{
			oldPath = FileManager.ConvertSimulatedPackagePathToNormalPath(oldPath);
			if (!FileManager.IsSecureWritePath(oldPath))
			{
				throw new Exception("Attempted to move file from non-secure path " + oldPath);
			}
			newPath = FileManager.ConvertSimulatedPackagePathToNormalPath(newPath);
			if (!FileManager.IsSecureWritePath(newPath))
			{
				throw new Exception("Attempted to move file to non-secure path " + newPath);
			}
			FileManager.DoFileMove(oldPath, newPath, overwrite);
		}

		// Token: 0x060056E2 RID: 22242 RVA: 0x001FC7DC File Offset: 0x001FABDC
		public static void MoveFileFromPlugin(string oldPath, string newPath, bool overwrite, UserActionCallback confirmCallback, UserActionCallback denyCallback, ExceptionCallback exceptionCallback)
		{
			FileManager.<MoveFileFromPlugin>c__AnonStoreyD <MoveFileFromPlugin>c__AnonStoreyD = new FileManager.<MoveFileFromPlugin>c__AnonStoreyD();
			<MoveFileFromPlugin>c__AnonStoreyD.oldPath = oldPath;
			<MoveFileFromPlugin>c__AnonStoreyD.newPath = newPath;
			<MoveFileFromPlugin>c__AnonStoreyD.overwrite = overwrite;
			<MoveFileFromPlugin>c__AnonStoreyD.exceptionCallback = exceptionCallback;
			<MoveFileFromPlugin>c__AnonStoreyD.confirmCallback = confirmCallback;
			<MoveFileFromPlugin>c__AnonStoreyD.oldPath = FileManager.ConvertSimulatedPackagePathToNormalPath(<MoveFileFromPlugin>c__AnonStoreyD.oldPath);
			if (!FileManager.IsSecurePluginWritePath(<MoveFileFromPlugin>c__AnonStoreyD.oldPath))
			{
				Exception ex = new Exception("Plugin attempted to move file from non-secure path " + <MoveFileFromPlugin>c__AnonStoreyD.oldPath);
				if (<MoveFileFromPlugin>c__AnonStoreyD.exceptionCallback != null)
				{
					<MoveFileFromPlugin>c__AnonStoreyD.exceptionCallback(ex);
					return;
				}
				throw ex;
			}
			else
			{
				<MoveFileFromPlugin>c__AnonStoreyD.newPath = FileManager.ConvertSimulatedPackagePathToNormalPath(<MoveFileFromPlugin>c__AnonStoreyD.newPath);
				if (FileManager.IsSecurePluginWritePath(<MoveFileFromPlugin>c__AnonStoreyD.newPath))
				{
					if (!FileManager.IsPluginWritePathThatNeedsConfirm(<MoveFileFromPlugin>c__AnonStoreyD.oldPath) && !FileManager.IsPluginWritePathThatNeedsConfirm(<MoveFileFromPlugin>c__AnonStoreyD.newPath))
					{
						try
						{
							FileManager.DoFileMove(<MoveFileFromPlugin>c__AnonStoreyD.oldPath, <MoveFileFromPlugin>c__AnonStoreyD.newPath, <MoveFileFromPlugin>c__AnonStoreyD.overwrite);
						}
						catch (Exception ex2)
						{
							if (<MoveFileFromPlugin>c__AnonStoreyD.exceptionCallback != null)
							{
								<MoveFileFromPlugin>c__AnonStoreyD.exceptionCallback(ex2);
								return;
							}
							throw ex2;
						}
						if (<MoveFileFromPlugin>c__AnonStoreyD.confirmCallback != null)
						{
							<MoveFileFromPlugin>c__AnonStoreyD.confirmCallback();
						}
					}
					else
					{
						FileManager.ConfirmPluginActionWithUser("move file from " + <MoveFileFromPlugin>c__AnonStoreyD.oldPath + " to " + <MoveFileFromPlugin>c__AnonStoreyD.newPath, new UserActionCallback(<MoveFileFromPlugin>c__AnonStoreyD.<>m__0), denyCallback);
					}
					return;
				}
				Exception ex3 = new Exception("Plugin attempted to move file to non-secure path " + <MoveFileFromPlugin>c__AnonStoreyD.newPath);
				if (<MoveFileFromPlugin>c__AnonStoreyD.exceptionCallback != null)
				{
					<MoveFileFromPlugin>c__AnonStoreyD.exceptionCallback(ex3);
					return;
				}
				throw ex3;
			}
		}

		// Token: 0x060056E3 RID: 22243 RVA: 0x001FC96C File Offset: 0x001FAD6C
		private void Awake()
		{
			FileManager.singleton = this;
		}

		// Token: 0x060056E4 RID: 22244 RVA: 0x001FC974 File Offset: 0x001FAD74
		private void OnDestroy()
		{
			FileManager.ClearAll();
		}

		// Token: 0x060056E5 RID: 22245 RVA: 0x001FC97B File Offset: 0x001FAD7B
		// Note: this type is marked as 'beforefieldinit'.
		static FileManager()
		{
		}

		// Token: 0x060056E6 RID: 22246 RVA: 0x001FC998 File Offset: 0x001FAD98
		[CompilerGenerated]
		private static int <SortFileEntriesByLastWriteTime>m__0(FileEntry e1, FileEntry e2)
		{
			return e1.LastWriteTime.CompareTo(e2.LastWriteTime);
		}

		// Token: 0x04004754 RID: 18260
		public static bool debug;

		// Token: 0x04004755 RID: 18261
		public static FileManager singleton;

		// Token: 0x04004756 RID: 18262
		protected static Dictionary<string, VarPackage> packagesByUid;

		// Token: 0x04004757 RID: 18263
		protected static HashSet<VarPackage> enabledPackages;

		// Token: 0x04004758 RID: 18264
		protected static Dictionary<string, VarPackage> packagesByPath;

		// Token: 0x04004759 RID: 18265
		protected static Dictionary<string, VarPackageGroup> packageGroups;

		// Token: 0x0400475A RID: 18266
		protected static HashSet<VarFileEntry> allVarFileEntries;

		// Token: 0x0400475B RID: 18267
		protected static HashSet<VarDirectoryEntry> allVarDirectoryEntries;

		// Token: 0x0400475C RID: 18268
		protected static Dictionary<string, VarFileEntry> uidToVarFileEntry;

		// Token: 0x0400475D RID: 18269
		protected static Dictionary<string, VarFileEntry> pathToVarFileEntry;

		// Token: 0x0400475E RID: 18270
		protected static Dictionary<string, VarDirectoryEntry> uidToVarDirectoryEntry;

		// Token: 0x0400475F RID: 18271
		protected static Dictionary<string, VarDirectoryEntry> pathToVarDirectoryEntry;

		// Token: 0x04004760 RID: 18272
		protected static Dictionary<string, VarDirectoryEntry> varPackagePathToRootVarDirectory;

		// Token: 0x04004761 RID: 18273
		protected static string packageFolder = "AddonPackages";

		// Token: 0x04004762 RID: 18274
		protected static string userPrefsFolder = "AddonPackagesUserPrefs";

		// Token: 0x04004763 RID: 18275
		protected static OnRefresh onRefreshHandlers;

		// Token: 0x04004764 RID: 18276
		public static string[] demoPackagePrefixes;

		// Token: 0x04004765 RID: 18277
		public static bool packagesEnabled = true;

		// Token: 0x04004766 RID: 18278
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static DateTime <lastPackageRefreshTime>k__BackingField;

		// Token: 0x04004767 RID: 18279
		protected static HashSet<string> restrictedReadPaths;

		// Token: 0x04004768 RID: 18280
		protected static HashSet<string> secureReadPaths;

		// Token: 0x04004769 RID: 18281
		protected static HashSet<string> secureInternalWritePaths;

		// Token: 0x0400476A RID: 18282
		protected static HashSet<string> securePluginWritePaths;

		// Token: 0x0400476B RID: 18283
		protected static HashSet<string> pluginWritePathsThatDoNotNeedConfirm;

		// Token: 0x0400476C RID: 18284
		public Transform userConfirmContainer;

		// Token: 0x0400476D RID: 18285
		public Transform userConfirmPrefab;

		// Token: 0x0400476E RID: 18286
		public Transform userConfirmPluginActionPrefab;

		// Token: 0x0400476F RID: 18287
		protected static Dictionary<string, string> pluginHashToPluginPath;

		// Token: 0x04004770 RID: 18288
		protected AsyncFlag userConfirmFlag;

		// Token: 0x04004771 RID: 18289
		protected HashSet<UserConfirmPanel> activeUserConfirmPanels;

		// Token: 0x04004772 RID: 18290
		protected static HashSet<string> userConfirmedPlugins;

		// Token: 0x04004773 RID: 18291
		protected static HashSet<string> userDeniedPlugins;

		// Token: 0x04004774 RID: 18292
		protected static LinkedList<string> loadDirStack;

		// Token: 0x04004775 RID: 18293
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static string <CurrentSaveDir>k__BackingField;

		// Token: 0x04004776 RID: 18294
		[CompilerGenerated]
		private static Comparison<FileEntry> <>f__am$cache0;

		// Token: 0x02000BDE RID: 3038
		public class PackageUIDAndPublisher
		{
			// Token: 0x060056E7 RID: 22247 RVA: 0x001FC9B9 File Offset: 0x001FADB9
			public PackageUIDAndPublisher()
			{
			}

			// Token: 0x04004777 RID: 18295
			public string uid;

			// Token: 0x04004778 RID: 18296
			public string publisher;
		}

		// Token: 0x02000FE4 RID: 4068
		[CompilerGenerated]
		private sealed class <UserConfirm>c__AnonStorey2
		{
			// Token: 0x060075FA RID: 30202 RVA: 0x001FC9C1 File Offset: 0x001FADC1
			public <UserConfirm>c__AnonStorey2()
			{
			}

			// Token: 0x040069BF RID: 27071
			internal UserActionCallback confirmCallback;

			// Token: 0x040069C0 RID: 27072
			internal UserActionCallback autoConfirmCallback;

			// Token: 0x040069C1 RID: 27073
			internal UserActionCallback confirmStickyCallback;

			// Token: 0x040069C2 RID: 27074
			internal UserActionCallback denyCallback;

			// Token: 0x040069C3 RID: 27075
			internal UserActionCallback autoDenyCallback;

			// Token: 0x040069C4 RID: 27076
			internal UserActionCallback denyStickyCallback;

			// Token: 0x040069C5 RID: 27077
			internal FileManager $this;
		}

		// Token: 0x02000FE5 RID: 4069
		[CompilerGenerated]
		private sealed class <UserConfirm>c__AnonStorey1
		{
			// Token: 0x060075FB RID: 30203 RVA: 0x001FC9C9 File Offset: 0x001FADC9
			public <UserConfirm>c__AnonStorey1()
			{
			}

			// Token: 0x060075FC RID: 30204 RVA: 0x001FC9D1 File Offset: 0x001FADD1
			internal void <>m__0()
			{
				this.<>f__ref$2.$this.DestroyUserConfirmPanel(this.ucp);
				if (this.<>f__ref$2.confirmCallback != null)
				{
					this.<>f__ref$2.confirmCallback();
				}
			}

			// Token: 0x060075FD RID: 30205 RVA: 0x001FCA09 File Offset: 0x001FAE09
			internal void <>m__1()
			{
				this.<>f__ref$2.$this.DestroyUserConfirmPanel(this.ucp);
				if (this.<>f__ref$2.autoConfirmCallback != null)
				{
					this.<>f__ref$2.autoConfirmCallback();
				}
			}

			// Token: 0x060075FE RID: 30206 RVA: 0x001FCA41 File Offset: 0x001FAE41
			internal void <>m__2()
			{
				this.<>f__ref$2.$this.DestroyUserConfirmPanel(this.ucp);
				if (this.<>f__ref$2.confirmStickyCallback != null)
				{
					this.<>f__ref$2.confirmStickyCallback();
				}
			}

			// Token: 0x060075FF RID: 30207 RVA: 0x001FCA79 File Offset: 0x001FAE79
			internal void <>m__3()
			{
				this.<>f__ref$2.$this.DestroyUserConfirmPanel(this.ucp);
				if (this.<>f__ref$2.denyCallback != null)
				{
					this.<>f__ref$2.denyCallback();
				}
			}

			// Token: 0x06007600 RID: 30208 RVA: 0x001FCAB1 File Offset: 0x001FAEB1
			internal void <>m__4()
			{
				this.<>f__ref$2.$this.DestroyUserConfirmPanel(this.ucp);
				if (this.<>f__ref$2.autoDenyCallback != null)
				{
					this.<>f__ref$2.autoDenyCallback();
				}
			}

			// Token: 0x06007601 RID: 30209 RVA: 0x001FCAE9 File Offset: 0x001FAEE9
			internal void <>m__5()
			{
				this.<>f__ref$2.$this.DestroyUserConfirmPanel(this.ucp);
				if (this.<>f__ref$2.denyStickyCallback != null)
				{
					this.<>f__ref$2.denyStickyCallback();
				}
			}

			// Token: 0x040069C6 RID: 27078
			internal UserConfirmPanel ucp;

			// Token: 0x040069C7 RID: 27079
			internal FileManager.<UserConfirm>c__AnonStorey2 <>f__ref$2;
		}

		// Token: 0x02000FE6 RID: 4070
		[CompilerGenerated]
		private sealed class <UserConfirmPluginAction>c__AnonStorey4
		{
			// Token: 0x06007602 RID: 30210 RVA: 0x001FCB21 File Offset: 0x001FAF21
			public <UserConfirmPluginAction>c__AnonStorey4()
			{
			}

			// Token: 0x040069C8 RID: 27080
			internal UserActionCallback confirmCallback;

			// Token: 0x040069C9 RID: 27081
			internal UserActionCallback denyCallback;

			// Token: 0x040069CA RID: 27082
			internal FileManager $this;
		}

		// Token: 0x02000FE7 RID: 4071
		[CompilerGenerated]
		private sealed class <UserConfirmPluginAction>c__AnonStorey3
		{
			// Token: 0x06007603 RID: 30211 RVA: 0x001FCB29 File Offset: 0x001FAF29
			public <UserConfirmPluginAction>c__AnonStorey3()
			{
			}

			// Token: 0x06007604 RID: 30212 RVA: 0x001FCB31 File Offset: 0x001FAF31
			internal void <>m__0()
			{
				this.<>f__ref$4.$this.DestroyUserConfirmPanel(this.ucp);
				if (this.<>f__ref$4.confirmCallback != null)
				{
					this.<>f__ref$4.confirmCallback();
				}
			}

			// Token: 0x06007605 RID: 30213 RVA: 0x001FCB69 File Offset: 0x001FAF69
			internal void <>m__1()
			{
				this.<>f__ref$4.$this.ConfirmAllPanelsWithSignature(this.pluginHash, true);
			}

			// Token: 0x06007606 RID: 30214 RVA: 0x001FCB82 File Offset: 0x001FAF82
			internal void <>m__2()
			{
				this.<>f__ref$4.$this.DestroyUserConfirmPanel(this.ucp);
				if (this.<>f__ref$4.denyCallback != null)
				{
					this.<>f__ref$4.denyCallback();
				}
			}

			// Token: 0x06007607 RID: 30215 RVA: 0x001FCBBA File Offset: 0x001FAFBA
			internal void <>m__3()
			{
				this.<>f__ref$4.$this.DenyAllPanelsWithSignature(this.pluginHash, true);
			}

			// Token: 0x040069CB RID: 27083
			internal UserConfirmPanel ucp;

			// Token: 0x040069CC RID: 27084
			internal string pluginHash;

			// Token: 0x040069CD RID: 27085
			internal FileManager.<UserConfirmPluginAction>c__AnonStorey4 <>f__ref$4;
		}

		// Token: 0x02000FE8 RID: 4072
		[CompilerGenerated]
		private sealed class <SyncAutoAlwaysAllowPlugins>c__AnonStorey5
		{
			// Token: 0x06007608 RID: 30216 RVA: 0x001FCBD3 File Offset: 0x001FAFD3
			public <SyncAutoAlwaysAllowPlugins>c__AnonStorey5()
			{
			}

			// Token: 0x06007609 RID: 30217 RVA: 0x001FCBDB File Offset: 0x001FAFDB
			internal void <>m__0()
			{
				this.vp.PluginsAlwaysEnabled = false;
			}

			// Token: 0x0600760A RID: 30218 RVA: 0x001FCBE9 File Offset: 0x001FAFE9
			internal void <>m__1()
			{
				this.vp.PluginsAlwaysEnabled = false;
			}

			// Token: 0x040069CE RID: 27086
			internal VarPackage vp;
		}

		// Token: 0x02000FE9 RID: 4073
		[CompilerGenerated]
		private sealed class <DeleteDirectoryFromPlugin>c__AnonStorey6
		{
			// Token: 0x0600760B RID: 30219 RVA: 0x001FCBF7 File Offset: 0x001FAFF7
			public <DeleteDirectoryFromPlugin>c__AnonStorey6()
			{
			}

			// Token: 0x0600760C RID: 30220 RVA: 0x001FCC00 File Offset: 0x001FB000
			internal void <>m__0()
			{
				try
				{
					Directory.Delete(this.path, this.recursive);
				}
				catch (Exception e)
				{
					if (this.exceptionCallback != null)
					{
						this.exceptionCallback(e);
					}
					return;
				}
				if (this.confirmCallback != null)
				{
					this.confirmCallback();
				}
			}

			// Token: 0x040069CF RID: 27087
			internal string path;

			// Token: 0x040069D0 RID: 27088
			internal bool recursive;

			// Token: 0x040069D1 RID: 27089
			internal ExceptionCallback exceptionCallback;

			// Token: 0x040069D2 RID: 27090
			internal UserActionCallback confirmCallback;
		}

		// Token: 0x02000FEA RID: 4074
		[CompilerGenerated]
		private sealed class <MoveDirectoryFromPlugin>c__AnonStorey7
		{
			// Token: 0x0600760D RID: 30221 RVA: 0x001FCC68 File Offset: 0x001FB068
			public <MoveDirectoryFromPlugin>c__AnonStorey7()
			{
			}

			// Token: 0x0600760E RID: 30222 RVA: 0x001FCC70 File Offset: 0x001FB070
			internal void <>m__0()
			{
				try
				{
					Directory.Move(this.oldPath, this.newPath);
				}
				catch (Exception e)
				{
					if (this.exceptionCallback != null)
					{
						this.exceptionCallback(e);
					}
					return;
				}
				if (this.confirmCallback != null)
				{
					this.confirmCallback();
				}
			}

			// Token: 0x040069D3 RID: 27091
			internal string oldPath;

			// Token: 0x040069D4 RID: 27092
			internal string newPath;

			// Token: 0x040069D5 RID: 27093
			internal ExceptionCallback exceptionCallback;

			// Token: 0x040069D6 RID: 27094
			internal UserActionCallback confirmCallback;
		}

		// Token: 0x02000FEB RID: 4075
		[CompilerGenerated]
		private sealed class <ReadAllBytesCoroutine>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x0600760F RID: 30223 RVA: 0x001FCCD8 File Offset: 0x001FB0D8
			[DebuggerHidden]
			public <ReadAllBytesCoroutine>c__Iterator0()
			{
			}

			// Token: 0x06007610 RID: 30224 RVA: 0x001FCCE0 File Offset: 0x001FB0E0
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					<ReadAllBytesCoroutine>c__AnonStorey = new FileManager.<ReadAllBytesCoroutine>c__Iterator0.<ReadAllBytesCoroutine>c__AnonStorey8();
					<ReadAllBytesCoroutine>c__AnonStorey.fe = fe;
					<ReadAllBytesCoroutine>c__AnonStorey.result = result;
					loadThread = new Thread(new ThreadStart(<ReadAllBytesCoroutine>c__AnonStorey.<>m__0));
					loadThread.Start();
					break;
				case 1U:
					break;
				default:
					return false;
				}
				if (loadThread.IsAlive)
				{
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				this.$PC = -1;
				return false;
			}

			// Token: 0x17001169 RID: 4457
			// (get) Token: 0x06007611 RID: 30225 RVA: 0x001FCD9C File Offset: 0x001FB19C
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x1700116A RID: 4458
			// (get) Token: 0x06007612 RID: 30226 RVA: 0x001FCDA4 File Offset: 0x001FB1A4
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007613 RID: 30227 RVA: 0x001FCDAC File Offset: 0x001FB1AC
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007614 RID: 30228 RVA: 0x001FCDBC File Offset: 0x001FB1BC
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x040069D7 RID: 27095
			internal FileEntry fe;

			// Token: 0x040069D8 RID: 27096
			internal byte[] result;

			// Token: 0x040069D9 RID: 27097
			internal Thread <loadThread>__0;

			// Token: 0x040069DA RID: 27098
			internal object $current;

			// Token: 0x040069DB RID: 27099
			internal bool $disposing;

			// Token: 0x040069DC RID: 27100
			internal int $PC;

			// Token: 0x040069DD RID: 27101
			private FileManager.<ReadAllBytesCoroutine>c__Iterator0.<ReadAllBytesCoroutine>c__AnonStorey8 $locvar0;

			// Token: 0x02000FF1 RID: 4081
			private sealed class <ReadAllBytesCoroutine>c__AnonStorey8
			{
				// Token: 0x0600761F RID: 30239 RVA: 0x001FCDC3 File Offset: 0x001FB1C3
				public <ReadAllBytesCoroutine>c__AnonStorey8()
				{
				}

				// Token: 0x06007620 RID: 30240 RVA: 0x001FCDCC File Offset: 0x001FB1CC
				internal void <>m__0()
				{
					byte[] buffer = new byte[32768];
					using (FileEntryStream fileEntryStream = FileManager.OpenStream(this.fe))
					{
						using (MemoryStream memoryStream = new MemoryStream(this.result))
						{
							StreamUtils.Copy(fileEntryStream.Stream, memoryStream, buffer);
						}
					}
				}

				// Token: 0x040069F2 RID: 27122
				internal FileEntry fe;

				// Token: 0x040069F3 RID: 27123
				internal byte[] result;
			}
		}

		// Token: 0x02000FEC RID: 4076
		[CompilerGenerated]
		private sealed class <WriteAllTextFromPlugin>c__AnonStorey9
		{
			// Token: 0x06007615 RID: 30229 RVA: 0x001FCE4C File Offset: 0x001FB24C
			public <WriteAllTextFromPlugin>c__AnonStorey9()
			{
			}

			// Token: 0x06007616 RID: 30230 RVA: 0x001FCE54 File Offset: 0x001FB254
			internal void <>m__0()
			{
				try
				{
					File.WriteAllText(this.path, this.text);
				}
				catch (Exception e)
				{
					if (this.exceptionCallback != null)
					{
						this.exceptionCallback(e);
					}
					return;
				}
				if (this.confirmCallback != null)
				{
					this.confirmCallback();
				}
			}

			// Token: 0x040069DE RID: 27102
			internal string path;

			// Token: 0x040069DF RID: 27103
			internal string text;

			// Token: 0x040069E0 RID: 27104
			internal ExceptionCallback exceptionCallback;

			// Token: 0x040069E1 RID: 27105
			internal UserActionCallback confirmCallback;
		}

		// Token: 0x02000FED RID: 4077
		[CompilerGenerated]
		private sealed class <WriteAllBytesFromPlugin>c__AnonStoreyA
		{
			// Token: 0x06007617 RID: 30231 RVA: 0x001FCEBC File Offset: 0x001FB2BC
			public <WriteAllBytesFromPlugin>c__AnonStoreyA()
			{
			}

			// Token: 0x06007618 RID: 30232 RVA: 0x001FCEC4 File Offset: 0x001FB2C4
			internal void <>m__0()
			{
				try
				{
					File.WriteAllBytes(this.path, this.bytes);
				}
				catch (Exception e)
				{
					if (this.exceptionCallback != null)
					{
						this.exceptionCallback(e);
					}
					return;
				}
				if (this.confirmCallback != null)
				{
					this.confirmCallback();
				}
			}

			// Token: 0x040069E2 RID: 27106
			internal string path;

			// Token: 0x040069E3 RID: 27107
			internal byte[] bytes;

			// Token: 0x040069E4 RID: 27108
			internal ExceptionCallback exceptionCallback;

			// Token: 0x040069E5 RID: 27109
			internal UserActionCallback confirmCallback;
		}

		// Token: 0x02000FEE RID: 4078
		[CompilerGenerated]
		private sealed class <DeleteFileFromPlugin>c__AnonStoreyB
		{
			// Token: 0x06007619 RID: 30233 RVA: 0x001FCF2C File Offset: 0x001FB32C
			public <DeleteFileFromPlugin>c__AnonStoreyB()
			{
			}

			// Token: 0x0600761A RID: 30234 RVA: 0x001FCF34 File Offset: 0x001FB334
			internal void <>m__0()
			{
				try
				{
					File.Delete(this.path);
				}
				catch (Exception e)
				{
					if (this.exceptionCallback != null)
					{
						this.exceptionCallback(e);
					}
					return;
				}
				if (this.confirmCallback != null)
				{
					this.confirmCallback();
				}
			}

			// Token: 0x040069E6 RID: 27110
			internal string path;

			// Token: 0x040069E7 RID: 27111
			internal ExceptionCallback exceptionCallback;

			// Token: 0x040069E8 RID: 27112
			internal UserActionCallback confirmCallback;
		}

		// Token: 0x02000FEF RID: 4079
		[CompilerGenerated]
		private sealed class <CopyFileFromPlugin>c__AnonStoreyC
		{
			// Token: 0x0600761B RID: 30235 RVA: 0x001FCF94 File Offset: 0x001FB394
			public <CopyFileFromPlugin>c__AnonStoreyC()
			{
			}

			// Token: 0x0600761C RID: 30236 RVA: 0x001FCF9C File Offset: 0x001FB39C
			internal void <>m__0()
			{
				try
				{
					FileManager.DoFileCopy(this.oldPath, this.newPath);
				}
				catch (Exception e)
				{
					if (this.exceptionCallback != null)
					{
						this.exceptionCallback(e);
					}
					return;
				}
				if (this.confirmCallback != null)
				{
					this.confirmCallback();
				}
			}

			// Token: 0x040069E9 RID: 27113
			internal string oldPath;

			// Token: 0x040069EA RID: 27114
			internal string newPath;

			// Token: 0x040069EB RID: 27115
			internal ExceptionCallback exceptionCallback;

			// Token: 0x040069EC RID: 27116
			internal UserActionCallback confirmCallback;
		}

		// Token: 0x02000FF0 RID: 4080
		[CompilerGenerated]
		private sealed class <MoveFileFromPlugin>c__AnonStoreyD
		{
			// Token: 0x0600761D RID: 30237 RVA: 0x001FD004 File Offset: 0x001FB404
			public <MoveFileFromPlugin>c__AnonStoreyD()
			{
			}

			// Token: 0x0600761E RID: 30238 RVA: 0x001FD00C File Offset: 0x001FB40C
			internal void <>m__0()
			{
				try
				{
					FileManager.DoFileMove(this.oldPath, this.newPath, this.overwrite);
				}
				catch (Exception e)
				{
					if (this.exceptionCallback != null)
					{
						this.exceptionCallback(e);
					}
					return;
				}
				if (this.confirmCallback != null)
				{
					this.confirmCallback();
				}
			}

			// Token: 0x040069ED RID: 27117
			internal string oldPath;

			// Token: 0x040069EE RID: 27118
			internal string newPath;

			// Token: 0x040069EF RID: 27119
			internal bool overwrite;

			// Token: 0x040069F0 RID: 27120
			internal ExceptionCallback exceptionCallback;

			// Token: 0x040069F1 RID: 27121
			internal UserActionCallback confirmCallback;
		}
	}
}
