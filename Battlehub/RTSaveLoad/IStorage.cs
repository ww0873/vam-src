using System;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000262 RID: 610
	public interface IStorage
	{
		// Token: 0x06000CD1 RID: 3281
		void CheckFolderExists(string path, StorageEventHandler<string, bool> callback);

		// Token: 0x06000CD2 RID: 3282
		void CheckFileExists(string path, StorageEventHandler<string, bool> callback);

		// Token: 0x06000CD3 RID: 3283
		void GetFolders(string path, StorageEventHandler<string, string[]> callback, bool fullPath = true);

		// Token: 0x06000CD4 RID: 3284
		void GetFiles(string path, StorageEventHandler<string, string[]> callback, bool fullPath = true);

		// Token: 0x06000CD5 RID: 3285
		void SaveFile(string path, byte[] data, StorageEventHandler<string> callback);

		// Token: 0x06000CD6 RID: 3286
		void SaveFiles(string[] path, byte[][] data, StorageEventHandler<string[]> callback);

		// Token: 0x06000CD7 RID: 3287
		void LoadFile(string path, StorageEventHandler<string, byte[]> callback);

		// Token: 0x06000CD8 RID: 3288
		void LoadFiles(string[] path, StorageEventHandler<string[], byte[][]> callback);

		// Token: 0x06000CD9 RID: 3289
		void DeleteFile(string path, StorageEventHandler<string> callback);

		// Token: 0x06000CDA RID: 3290
		void DeleteFiles(string[] path, StorageEventHandler<string[]> callback);

		// Token: 0x06000CDB RID: 3291
		void CopyFile(string sourcePath, string destinationPath, StorageEventHandler<string, string> callback);

		// Token: 0x06000CDC RID: 3292
		void CopyFiles(string[] sourcePath, string[] destinationPath, StorageEventHandler<string[], string[]> callback);

		// Token: 0x06000CDD RID: 3293
		void MoveFile(string sourcePath, string destinationPath, StorageEventHandler<string, string> callback);

		// Token: 0x06000CDE RID: 3294
		void MoveFiles(string[] sourcePath, string[] destinationPath, StorageEventHandler<string[], string[]> callback);

		// Token: 0x06000CDF RID: 3295
		void CreateFolder(string path, StorageEventHandler<string> callback);

		// Token: 0x06000CE0 RID: 3296
		void CreateFolders(string[] path, StorageEventHandler<string[]> callback);

		// Token: 0x06000CE1 RID: 3297
		void DeleteFolder(string path, StorageEventHandler<string> callback);

		// Token: 0x06000CE2 RID: 3298
		void DeleteFolders(string[] path, StorageEventHandler<string[]> callback);

		// Token: 0x06000CE3 RID: 3299
		void CopyFolder(string sourcePath, string destinationPath, StorageEventHandler<string, string> callback);

		// Token: 0x06000CE4 RID: 3300
		void CopyFolders(string[] sourcePath, string[] destinationPath, StorageEventHandler<string[], string[]> callback);

		// Token: 0x06000CE5 RID: 3301
		void MoveFolder(string sourcePath, string destinationPath, StorageEventHandler<string, string> callback);

		// Token: 0x06000CE6 RID: 3302
		void MoveFolders(string[] sourcePath, string[] destinationPath, StorageEventHandler<string[], string[]> callback);
	}
}
