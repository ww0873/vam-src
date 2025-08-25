using System;
using System.IO;

namespace MVR.FileManagement
{
	// Token: 0x02000BF0 RID: 3056
	public class SystemFileEntryStreamReader : FileEntryStreamReader
	{
		// Token: 0x060057D3 RID: 22483 RVA: 0x0020452A File Offset: 0x0020292A
		public SystemFileEntryStreamReader(SystemFileEntry entry) : base(entry)
		{
			this.StreamReader = new StreamReader(entry.Path);
		}
	}
}
