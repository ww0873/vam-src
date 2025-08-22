using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace MVR.FileManagement
{
	// Token: 0x02000BDC RID: 3036
	public abstract class FileEntryStreamReader : IDisposable
	{
		// Token: 0x06005654 RID: 22100 RVA: 0x001F7D0C File Offset: 0x001F610C
		public FileEntryStreamReader(FileEntry fe)
		{
		}

		// Token: 0x17000C9C RID: 3228
		// (get) Token: 0x06005655 RID: 22101 RVA: 0x001F7D14 File Offset: 0x001F6114
		// (set) Token: 0x06005656 RID: 22102 RVA: 0x001F7D1C File Offset: 0x001F611C
		public virtual StreamReader StreamReader
		{
			[CompilerGenerated]
			get
			{
				return this.<StreamReader>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<StreamReader>k__BackingField = value;
			}
		}

		// Token: 0x06005657 RID: 22103 RVA: 0x001F7D25 File Offset: 0x001F6125
		public virtual string ReadToEnd()
		{
			if (this.StreamReader != null)
			{
				return this.StreamReader.ReadToEnd();
			}
			return null;
		}

		// Token: 0x06005658 RID: 22104 RVA: 0x001F7D3F File Offset: 0x001F613F
		public virtual void Dispose()
		{
			if (this.StreamReader != null)
			{
				this.StreamReader.Dispose();
				this.StreamReader = null;
			}
		}

		// Token: 0x04004753 RID: 18259
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private StreamReader <StreamReader>k__BackingField;
	}
}
