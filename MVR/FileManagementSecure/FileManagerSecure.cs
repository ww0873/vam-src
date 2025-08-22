using System;
using System.Collections.Generic;
using System.IO;
using MVR.FileManagement;

namespace MVR.FileManagementSecure
{
	// Token: 0x02000BE3 RID: 3043
	public class FileManagerSecure
	{
		// Token: 0x060056F7 RID: 22263 RVA: 0x001FD151 File Offset: 0x001FB551
		public FileManagerSecure()
		{
		}

		// Token: 0x060056F8 RID: 22264 RVA: 0x001FD159 File Offset: 0x001FB559
		public static string GetFullPath(string path)
		{
			return FileManager.GetFullPath(path);
		}

		// Token: 0x060056F9 RID: 22265 RVA: 0x001FD161 File Offset: 0x001FB561
		public static string NormalizeLoadPath(string path)
		{
			return FileManager.NormalizeLoadPath(path);
		}

		// Token: 0x060056FA RID: 22266 RVA: 0x001FD169 File Offset: 0x001FB569
		public static string NormalizePath(string path)
		{
			return FileManager.NormalizePath(path);
		}

		// Token: 0x060056FB RID: 22267 RVA: 0x001FD171 File Offset: 0x001FB571
		public static string GetDirectoryName(string path, bool returnSlashPath = false)
		{
			return FileManager.GetDirectoryName(path, returnSlashPath);
		}

		// Token: 0x060056FC RID: 22268 RVA: 0x001FD17A File Offset: 0x001FB57A
		public static string GetFileName(string path)
		{
			return Path.GetFileName(path);
		}

		// Token: 0x060056FD RID: 22269 RVA: 0x001FD182 File Offset: 0x001FB582
		public static bool FileExists(string path, bool onlySystemFiles = false)
		{
			return FileManager.FileExists(path, onlySystemFiles, true);
		}

		// Token: 0x060056FE RID: 22270 RVA: 0x001FD18C File Offset: 0x001FB58C
		public static bool IsFileInPackage(string path)
		{
			return FileManager.IsFileInPackage(path);
		}

		// Token: 0x060056FF RID: 22271 RVA: 0x001FD194 File Offset: 0x001FB594
		public static DateTime FileLastWriteTime(string path, bool onlySystemFiles = false)
		{
			return FileManager.FileLastWriteTime(path, onlySystemFiles, true);
		}

		// Token: 0x06005700 RID: 22272 RVA: 0x001FD19E File Offset: 0x001FB59E
		public static DateTime FileCreationTime(string path, bool onlySystemFiles = false)
		{
			return FileManager.FileCreationTime(path, onlySystemFiles, true);
		}

		// Token: 0x06005701 RID: 22273 RVA: 0x001FD1A8 File Offset: 0x001FB5A8
		public static void RegisterRefreshHandler(OnRefresh refreshHandler)
		{
			FileManager.RegisterRefreshHandler(refreshHandler);
		}

		// Token: 0x06005702 RID: 22274 RVA: 0x001FD1B0 File Offset: 0x001FB5B0
		public static void UnregisterRefreshHandler(OnRefresh refreshHandler)
		{
			FileManager.UnregisterRefreshHandler(refreshHandler);
		}

		// Token: 0x06005703 RID: 22275 RVA: 0x001FD1B8 File Offset: 0x001FB5B8
		public static bool PackageExists(string packageUid)
		{
			VarPackage package = FileManager.GetPackage(packageUid);
			return package != null;
		}

		// Token: 0x06005704 RID: 22276 RVA: 0x001FD1D4 File Offset: 0x001FB5D4
		public static int GetPackageVersion(string packageUid)
		{
			VarPackage package = FileManager.GetPackage(packageUid);
			if (package != null)
			{
				return package.Version;
			}
			return -1;
		}

		// Token: 0x06005705 RID: 22277 RVA: 0x001FD1F6 File Offset: 0x001FB5F6
		public static List<ShortCut> GetShortCutsForDirectory(string dir, bool allowNavigationAboveRegularDirectories = false, bool useFullPaths = false, bool generateAllFlattenedShortcut = false, bool includeRegularDirsInFlattenedShortcut = false)
		{
			return FileManager.GetShortCutsForDirectory(dir, allowNavigationAboveRegularDirectories, useFullPaths, generateAllFlattenedShortcut, includeRegularDirsInFlattenedShortcut);
		}

		// Token: 0x06005706 RID: 22278 RVA: 0x001FD203 File Offset: 0x001FB603
		public static bool DirectoryExists(string path, bool onlySystemDirectories = false)
		{
			return FileManager.DirectoryExists(path, onlySystemDirectories, true);
		}

		// Token: 0x06005707 RID: 22279 RVA: 0x001FD20D File Offset: 0x001FB60D
		public static bool IsDirectoryInPackage(string path)
		{
			return FileManager.IsDirectoryInPackage(path);
		}

		// Token: 0x06005708 RID: 22280 RVA: 0x001FD215 File Offset: 0x001FB615
		public static DateTime DirectoryLastWriteTime(string path, bool onlySystemDirectories = false)
		{
			return FileManager.DirectoryLastWriteTime(path, onlySystemDirectories, true);
		}

		// Token: 0x06005709 RID: 22281 RVA: 0x001FD21F File Offset: 0x001FB61F
		public static DateTime DirectoryCreationTime(string path, bool onlySystemDirectories = false)
		{
			return FileManager.DirectoryCreationTime(path, onlySystemDirectories, true);
		}

		// Token: 0x0600570A RID: 22282 RVA: 0x001FD229 File Offset: 0x001FB629
		public static string[] GetDirectories(string dir, string pattern = null)
		{
			return FileManager.GetDirectories(dir, pattern, true);
		}

		// Token: 0x0600570B RID: 22283 RVA: 0x001FD233 File Offset: 0x001FB633
		public static string[] GetFiles(string dir, string pattern = null)
		{
			return FileManager.GetFiles(dir, pattern, true);
		}

		// Token: 0x0600570C RID: 22284 RVA: 0x001FD23D File Offset: 0x001FB63D
		public static void CreateDirectory(string path)
		{
			FileManager.CreateDirectoryFromPlugin(path, null, null, null);
		}

		// Token: 0x0600570D RID: 22285 RVA: 0x001FD248 File Offset: 0x001FB648
		public static void CreateDirectory(string path, UserActionCallback confirmCallback, UserActionCallback denyCallback, ExceptionCallback exceptionCallback)
		{
			FileManager.CreateDirectoryFromPlugin(path, confirmCallback, denyCallback, exceptionCallback);
		}

		// Token: 0x0600570E RID: 22286 RVA: 0x001FD253 File Offset: 0x001FB653
		public static byte[] ReadAllBytes(string path)
		{
			return FileManager.ReadAllBytes(path, true);
		}

		// Token: 0x0600570F RID: 22287 RVA: 0x001FD25C File Offset: 0x001FB65C
		public static string ReadAllText(string path)
		{
			return FileManager.ReadAllText(path, true);
		}

		// Token: 0x06005710 RID: 22288 RVA: 0x001FD265 File Offset: 0x001FB665
		public static void WriteAllText(string path, string text)
		{
			FileManager.WriteAllTextFromPlugin(path, text, null, null, null);
		}

		// Token: 0x06005711 RID: 22289 RVA: 0x001FD271 File Offset: 0x001FB671
		public static void WriteAllText(string path, string text, UserActionCallback confirmCallback, UserActionCallback denyCallback, ExceptionCallback exceptionCallback)
		{
			FileManager.WriteAllTextFromPlugin(path, text, confirmCallback, denyCallback, exceptionCallback);
		}

		// Token: 0x06005712 RID: 22290 RVA: 0x001FD27E File Offset: 0x001FB67E
		public static void WriteAllBytes(string path, byte[] bytes)
		{
			FileManager.WriteAllBytesFromPlugin(path, bytes, null, null, null);
		}

		// Token: 0x06005713 RID: 22291 RVA: 0x001FD28A File Offset: 0x001FB68A
		public static void WriteAllBytes(string path, byte[] bytes, UserActionCallback confirmCallback, UserActionCallback denyCallback, ExceptionCallback exceptionCallback)
		{
			FileManager.WriteAllBytesFromPlugin(path, bytes, confirmCallback, denyCallback, exceptionCallback);
		}

		// Token: 0x06005714 RID: 22292 RVA: 0x001FD297 File Offset: 0x001FB697
		public static void DeleteFile(string path)
		{
			FileManager.DeleteFileFromPlugin(path, null, null, null);
		}

		// Token: 0x06005715 RID: 22293 RVA: 0x001FD2A2 File Offset: 0x001FB6A2
		public static void DeleteFile(string path, UserActionCallback confirmCallback, UserActionCallback denyCallback, ExceptionCallback exceptionCallback)
		{
			FileManager.DeleteFileFromPlugin(path, confirmCallback, denyCallback, exceptionCallback);
		}

		// Token: 0x06005716 RID: 22294 RVA: 0x001FD2AD File Offset: 0x001FB6AD
		public static void CopyFile(string oldPath, string newPath)
		{
			FileManager.CopyFileFromPlugin(oldPath, newPath, null, null, null);
		}

		// Token: 0x06005717 RID: 22295 RVA: 0x001FD2B9 File Offset: 0x001FB6B9
		public static void CopyFile(string oldPath, string newPath, UserActionCallback confirmCallback, UserActionCallback denyCallback, ExceptionCallback exceptionCallback)
		{
			FileManager.CopyFileFromPlugin(oldPath, newPath, confirmCallback, denyCallback, exceptionCallback);
		}

		// Token: 0x06005718 RID: 22296 RVA: 0x001FD2C6 File Offset: 0x001FB6C6
		public static void MoveFile(string oldPath, string newPath, bool overwrite = true)
		{
			FileManager.MoveFileFromPlugin(oldPath, newPath, overwrite, null, null, null);
		}

		// Token: 0x06005719 RID: 22297 RVA: 0x001FD2D3 File Offset: 0x001FB6D3
		public static void MoveFile(string oldPath, string newPath, UserActionCallback confirmCallback, UserActionCallback denyCallback, ExceptionCallback exceptionCallback, bool overwrite = true)
		{
			FileManager.MoveFileFromPlugin(oldPath, newPath, overwrite, confirmCallback, denyCallback, exceptionCallback);
		}
	}
}
