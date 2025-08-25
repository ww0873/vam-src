using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;

namespace MVR.FileManagement
{
	// Token: 0x02000BF5 RID: 3061
	public class VarFileEntryStream : FileEntryStream
	{
		// Token: 0x06005804 RID: 22532 RVA: 0x00204E78 File Offset: 0x00203278
		public VarFileEntryStream(VarFileEntry entry) : base(entry)
		{
			if (entry.Simulated)
			{
				string path = entry.Package.Path + "\\" + entry.InternalPath;
				base.Stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
			}
			else
			{
				ZipFile zipFile = entry.Package.ZipFile;
				if (zipFile == null)
				{
					throw new Exception("Could not get ZipFile for package " + entry.Package.Uid);
				}
				ZipEntry entry2 = zipFile.GetEntry(entry.InternalSlashPath);
				if (entry2 == null)
				{
					this.Dispose();
					throw new Exception("Could not find entry " + entry.InternalSlashPath + " in zip file " + entry.Package.Path);
				}
				base.Stream = zipFile.GetInputStream(entry2);
			}
		}

		// Token: 0x06005805 RID: 22533 RVA: 0x00204F40 File Offset: 0x00203340
		public override void Dispose()
		{
			base.Dispose();
		}
	}
}
