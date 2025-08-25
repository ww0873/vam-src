using System;
using System.IO;

namespace MVR.FileManagement
{
	// Token: 0x02000BEF RID: 3055
	public class SystemFileEntryStream : FileEntryStream
	{
		// Token: 0x060057D2 RID: 22482 RVA: 0x0020450D File Offset: 0x0020290D
		public SystemFileEntryStream(SystemFileEntry entry) : base(entry)
		{
			base.Stream = File.Open(entry.Path, FileMode.Open, FileAccess.Read, FileShare.Read);
		}
	}
}
