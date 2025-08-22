using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace MVR.FileManagement
{
	// Token: 0x02000BDB RID: 3035
	public abstract class FileEntryStream : IDisposable
	{
		// Token: 0x06005650 RID: 22096 RVA: 0x001F7CD4 File Offset: 0x001F60D4
		public FileEntryStream(FileEntry fe)
		{
		}

		// Token: 0x17000C9B RID: 3227
		// (get) Token: 0x06005651 RID: 22097 RVA: 0x001F7CDC File Offset: 0x001F60DC
		// (set) Token: 0x06005652 RID: 22098 RVA: 0x001F7CE4 File Offset: 0x001F60E4
		public Stream Stream
		{
			[CompilerGenerated]
			get
			{
				return this.<Stream>k__BackingField;
			}
			[CompilerGenerated]
			protected set
			{
				this.<Stream>k__BackingField = value;
			}
		}

		// Token: 0x06005653 RID: 22099 RVA: 0x001F7CED File Offset: 0x001F60ED
		public virtual void Dispose()
		{
			if (this.Stream != null)
			{
				this.Stream.Dispose();
				this.Stream = null;
			}
		}

		// Token: 0x04004752 RID: 18258
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Stream <Stream>k__BackingField;
	}
}
