using System;
using System.IO;

namespace MVR.FileManagement
{
	// Token: 0x02000BEE RID: 3054
	public class SystemFileEntry : FileEntry
	{
		// Token: 0x060057CF RID: 22479 RVA: 0x00204424 File Offset: 0x00202824
		public SystemFileEntry(string path) : base(path)
		{
			this.flagBasePath = this.Path + ".";
			this.favPath = this.Path + ".fav";
			base.hidePath = this.Path + ".hide";
			this.Exists = File.Exists(this.Path);
			this.FullPath = System.IO.Path.GetFullPath(this.Path);
			this.FullSlashPath = this.FullPath.Replace('\\', '/');
			if (this.Exists)
			{
				FileInfo fileInfo = new FileInfo(this.Path);
				this.LastWriteTime = fileInfo.LastWriteTime;
				this.Size = fileInfo.Length;
			}
		}

		// Token: 0x060057D0 RID: 22480 RVA: 0x002044E0 File Offset: 0x002028E0
		public override FileEntryStream OpenStream()
		{
			return new SystemFileEntryStream(this);
		}

		// Token: 0x060057D1 RID: 22481 RVA: 0x002044F8 File Offset: 0x002028F8
		public override FileEntryStreamReader OpenStreamReader()
		{
			return new SystemFileEntryStreamReader(this);
		}
	}
}
