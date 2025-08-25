using System;
using System.IO;

namespace MVR.FileManagement
{
	// Token: 0x02000BF6 RID: 3062
	public class VarFileEntryStreamReader : FileEntryStreamReader
	{
		// Token: 0x06005806 RID: 22534 RVA: 0x00204F48 File Offset: 0x00203348
		public VarFileEntryStreamReader(VarFileEntry entry) : base(entry)
		{
			this.VarStream = new VarFileEntryStream(entry);
			this.StreamReader = new StreamReader(this.VarStream.Stream);
		}

		// Token: 0x06005807 RID: 22535 RVA: 0x00204F73 File Offset: 0x00203373
		public override void Dispose()
		{
			base.Dispose();
			if (this.VarStream != null)
			{
				this.VarStream.Dispose();
				this.VarStream = null;
			}
		}

		// Token: 0x040048A8 RID: 18600
		protected VarFileEntryStream VarStream;
	}
}
