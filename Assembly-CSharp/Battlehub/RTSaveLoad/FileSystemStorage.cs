using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000263 RID: 611
	public class FileSystemStorage : IStorage
	{
		// Token: 0x06000CE7 RID: 3303 RVA: 0x0004D6C6 File Offset: 0x0004BAC6
		public FileSystemStorage(string basePath)
		{
			Debug.Log("FileSystemStorage root: " + basePath);
			this.m_basePath = basePath;
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x0004D6E8 File Offset: 0x0004BAE8
		private void CombineWithBasePath(ref string[] path)
		{
			for (int i = 0; i < path.Length; i++)
			{
				path[i] = Path.Combine(this.m_basePath, path[i].TrimStart(new char[]
				{
					'/'
				}));
			}
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x0004D72D File Offset: 0x0004BB2D
		private void CombineWithBasePath(ref string path)
		{
			path = Path.Combine(this.m_basePath, path.TrimStart(new char[]
			{
				'/'
			}));
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x0004D750 File Offset: 0x0004BB50
		public void CheckFolderExists(string path, StorageEventHandler<string, bool> callback)
		{
			this.CombineWithBasePath(ref path);
			bool data = Directory.Exists(path);
			if (callback != null)
			{
				callback(new StoragePayload<string, bool>(path, data));
			}
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x0004D780 File Offset: 0x0004BB80
		public void CheckFileExists(string path, StorageEventHandler<string, bool> callback)
		{
			this.CombineWithBasePath(ref path);
			bool data = File.Exists(path);
			if (callback != null)
			{
				callback(new StoragePayload<string, bool>(path, data));
			}
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x0004D7AF File Offset: 0x0004BBAF
		public void CopyFile(string sourcePath, string destinationPath, StorageEventHandler<string, string> callback)
		{
			this.CombineWithBasePath(ref sourcePath);
			this.CombineWithBasePath(ref destinationPath);
			File.Copy(sourcePath, destinationPath);
			if (callback != null)
			{
				callback(new StoragePayload<string, string>(sourcePath, destinationPath));
			}
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x0004D7DC File Offset: 0x0004BBDC
		public void CopyFiles(string[] sourcePath, string[] destinationPath, StorageEventHandler<string[], string[]> callback)
		{
			this.CombineWithBasePath(ref sourcePath);
			this.CombineWithBasePath(ref destinationPath);
			for (int i = 0; i < sourcePath.Length; i++)
			{
				File.Copy(sourcePath[i], destinationPath[i]);
			}
			if (callback != null)
			{
				callback(new StoragePayload<string[], string[]>(sourcePath, destinationPath));
			}
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x0004D82C File Offset: 0x0004BC2C
		public void CopyFolder(string sourcePath, string destinationPath, StorageEventHandler<string, string> callback)
		{
			this.CombineWithBasePath(ref sourcePath);
			this.CombineWithBasePath(ref destinationPath);
			DirectoryInfo source = new DirectoryInfo(sourcePath);
			DirectoryInfo destination = new DirectoryInfo(destinationPath);
			FileSystemStorage.CopyAll(source, destination);
			if (callback != null)
			{
				callback(new StoragePayload<string, string>(sourcePath, destinationPath));
			}
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x0004D874 File Offset: 0x0004BC74
		public void CopyFolders(string[] sourcePath, string[] destinationPath, StorageEventHandler<string[], string[]> callback)
		{
			this.CombineWithBasePath(ref sourcePath);
			this.CombineWithBasePath(ref destinationPath);
			for (int i = 0; i < sourcePath.Length; i++)
			{
				DirectoryInfo source = new DirectoryInfo(sourcePath[i]);
				DirectoryInfo destination = new DirectoryInfo(destinationPath[i]);
				FileSystemStorage.CopyAll(source, destination);
			}
			if (callback != null)
			{
				callback(new StoragePayload<string[], string[]>(sourcePath, destinationPath));
			}
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x0004D8D4 File Offset: 0x0004BCD4
		private static void CopyAll(DirectoryInfo source, DirectoryInfo destination)
		{
			Directory.CreateDirectory(destination.FullName);
			foreach (FileInfo fileInfo in source.GetFiles())
			{
				fileInfo.CopyTo(Path.Combine(destination.FullName, fileInfo.Name), true);
			}
			foreach (DirectoryInfo directoryInfo in source.GetDirectories())
			{
				DirectoryInfo destination2 = destination.CreateSubdirectory(directoryInfo.Name);
				FileSystemStorage.CopyAll(directoryInfo, destination2);
			}
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x0004D962 File Offset: 0x0004BD62
		public void CreateFolder(string path, StorageEventHandler<string> callback)
		{
			this.CombineWithBasePath(ref path);
			if (!Directory.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
			if (callback != null)
			{
				callback(new StoragePayload<string>(path));
			}
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x0004D990 File Offset: 0x0004BD90
		public void CreateFolders(string[] path, StorageEventHandler<string[]> callback)
		{
			this.CombineWithBasePath(ref path);
			for (int i = 0; i < path.Length; i++)
			{
				Directory.CreateDirectory(path[i]);
			}
			if (callback != null)
			{
				callback(new StoragePayload<string[]>(path));
			}
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x0004D9D4 File Offset: 0x0004BDD4
		public void DeleteFile(string path, StorageEventHandler<string> callback)
		{
			this.CombineWithBasePath(ref path);
			File.Delete(path);
			if (callback != null)
			{
				callback(new StoragePayload<string>(path));
			}
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x0004D9F8 File Offset: 0x0004BDF8
		public void DeleteFiles(string[] path, StorageEventHandler<string[]> callback)
		{
			this.CombineWithBasePath(ref path);
			for (int i = 0; i < path.Length; i++)
			{
				File.Delete(path[i]);
			}
			if (callback != null)
			{
				callback(new StoragePayload<string[]>(path));
			}
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x0004DA3B File Offset: 0x0004BE3B
		public void DeleteFolder(string path, StorageEventHandler<string> callback)
		{
			this.CombineWithBasePath(ref path);
			Directory.Delete(path, true);
			if (callback != null)
			{
				callback(new StoragePayload<string>(path));
			}
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x0004DA60 File Offset: 0x0004BE60
		public void DeleteFolders(string[] path, StorageEventHandler<string[]> callback)
		{
			this.CombineWithBasePath(ref path);
			for (int i = 0; i < path.Length; i++)
			{
				Directory.Delete(path[i], true);
			}
			if (callback != null)
			{
				callback(new StoragePayload<string[]>(path));
			}
		}

		// Token: 0x06000CF7 RID: 3319 RVA: 0x0004DAA4 File Offset: 0x0004BEA4
		public void GetFiles(string path, StorageEventHandler<string, string[]> callback, bool fullPath = true)
		{
			this.CombineWithBasePath(ref path);
			string[] data;
			if (fullPath)
			{
				data = Directory.GetFiles(path).Select(new Func<string, string>(this.<GetFiles>m__0)).ToArray<string>();
			}
			else
			{
				IEnumerable<string> files = Directory.GetFiles(path);
				if (FileSystemStorage.<>f__mg$cache0 == null)
				{
					FileSystemStorage.<>f__mg$cache0 = new Func<string, string>(Path.GetFileName);
				}
				data = files.Select(FileSystemStorage.<>f__mg$cache0).ToArray<string>();
			}
			if (callback != null)
			{
				callback(new StoragePayload<string, string[]>(path, data));
			}
		}

		// Token: 0x06000CF8 RID: 3320 RVA: 0x0004DB24 File Offset: 0x0004BF24
		public void GetFolders(string path, StorageEventHandler<string, string[]> callback, bool fullPath = true)
		{
			this.CombineWithBasePath(ref path);
			string[] data;
			if (fullPath)
			{
				data = Directory.GetDirectories(path).Select(new Func<string, string>(this.<GetFolders>m__1)).ToArray<string>();
			}
			else
			{
				IEnumerable<string> directories = Directory.GetDirectories(path);
				if (FileSystemStorage.<>f__am$cache0 == null)
				{
					FileSystemStorage.<>f__am$cache0 = new Func<string, string>(FileSystemStorage.<GetFolders>m__2);
				}
				data = directories.Select(FileSystemStorage.<>f__am$cache0).ToArray<string>();
			}
			if (callback != null)
			{
				callback(new StoragePayload<string, string[]>(path, data));
			}
		}

		// Token: 0x06000CF9 RID: 3321 RVA: 0x0004DBA4 File Offset: 0x0004BFA4
		public void LoadFile(string path, StorageEventHandler<string, byte[]> callback)
		{
			this.CombineWithBasePath(ref path);
			byte[] data = null;
			if (File.Exists(path))
			{
				data = File.ReadAllBytes(path);
			}
			if (callback != null)
			{
				callback(new StoragePayload<string, byte[]>(path, data));
			}
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x0004DBE0 File Offset: 0x0004BFE0
		public void LoadFiles(string[] path, StorageEventHandler<string[], byte[][]> callback)
		{
			this.CombineWithBasePath(ref path);
			byte[][] array = new byte[path.Length][];
			for (int i = 0; i < path.Length; i++)
			{
				if (File.Exists(path[i]))
				{
					array[i] = File.ReadAllBytes(path[i]);
				}
			}
			if (callback != null)
			{
				callback(new StoragePayload<string[], byte[][]>(path, array));
			}
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x0004DC3D File Offset: 0x0004C03D
		public void MoveFile(string sourcePath, string destinationPath, StorageEventHandler<string, string> callback)
		{
			this.CombineWithBasePath(ref sourcePath);
			this.CombineWithBasePath(ref destinationPath);
			File.Move(sourcePath, destinationPath);
			if (callback != null)
			{
				callback(new StoragePayload<string, string>(sourcePath, destinationPath));
			}
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x0004DC6C File Offset: 0x0004C06C
		public void MoveFiles(string[] sourcePath, string[] destinationPath, StorageEventHandler<string[], string[]> callback)
		{
			this.CombineWithBasePath(ref sourcePath);
			this.CombineWithBasePath(ref destinationPath);
			for (int i = 0; i < sourcePath.Length; i++)
			{
				File.Move(sourcePath[i], destinationPath[i]);
			}
			if (callback != null)
			{
				callback(new StoragePayload<string[], string[]>(sourcePath, destinationPath));
			}
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x0004DCBB File Offset: 0x0004C0BB
		public void MoveFolder(string sourcePath, string destinationPath, StorageEventHandler<string, string> callback)
		{
			this.CombineWithBasePath(ref sourcePath);
			this.CombineWithBasePath(ref destinationPath);
			if (sourcePath != destinationPath)
			{
				Directory.Move(sourcePath, destinationPath);
			}
			if (callback != null)
			{
				callback(new StoragePayload<string, string>(sourcePath, destinationPath));
			}
		}

		// Token: 0x06000CFE RID: 3326 RVA: 0x0004DCF4 File Offset: 0x0004C0F4
		public void MoveFolders(string[] sourcePath, string[] destinationPath, StorageEventHandler<string[], string[]> callback)
		{
			this.CombineWithBasePath(ref sourcePath);
			this.CombineWithBasePath(ref destinationPath);
			for (int i = 0; i < sourcePath.Length; i++)
			{
				Directory.Move(sourcePath[i], destinationPath[i]);
			}
			if (callback != null)
			{
				callback(new StoragePayload<string[], string[]>(sourcePath, destinationPath));
			}
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x0004DD43 File Offset: 0x0004C143
		public void SaveFile(string path, byte[] data, StorageEventHandler<string> callback)
		{
			this.CombineWithBasePath(ref path);
			File.WriteAllBytes(path, data);
			if (callback != null)
			{
				callback(new StoragePayload<string>(path));
			}
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x0004DD68 File Offset: 0x0004C168
		public void SaveFiles(string[] path, byte[][] data, StorageEventHandler<string[]> callback)
		{
			this.CombineWithBasePath(ref path);
			for (int i = 0; i < path.Length; i++)
			{
				File.WriteAllBytes(path[i], data[i]);
			}
			if (callback != null)
			{
				callback(new StoragePayload<string[]>(path));
			}
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x0004DDAE File Offset: 0x0004C1AE
		[CompilerGenerated]
		private string <GetFiles>m__0(string p)
		{
			return PathHelper.GetRelativePath(p, this.m_basePath);
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x0004DDBC File Offset: 0x0004C1BC
		[CompilerGenerated]
		private string <GetFolders>m__1(string p)
		{
			return PathHelper.GetRelativePath(p, this.m_basePath);
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x0004DDCA File Offset: 0x0004C1CA
		[CompilerGenerated]
		private static string <GetFolders>m__2(string d)
		{
			return new DirectoryInfo(d).Name;
		}

		// Token: 0x04000D1A RID: 3354
		private string m_basePath;

		// Token: 0x04000D1B RID: 3355
		[CompilerGenerated]
		private static Func<string, string> <>f__mg$cache0;

		// Token: 0x04000D1C RID: 3356
		[CompilerGenerated]
		private static Func<string, string> <>f__am$cache0;
	}
}
