using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x0200025F RID: 607
	public class StoragePayload<T1, T2> : StoragePayload<T1>
	{
		// Token: 0x06000CC6 RID: 3270 RVA: 0x0004D6A5 File Offset: 0x0004BAA5
		public StoragePayload(T1 path, T2 data) : base(path)
		{
			this.Data = data;
		}

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000CC7 RID: 3271 RVA: 0x0004D6B5 File Offset: 0x0004BAB5
		// (set) Token: 0x06000CC8 RID: 3272 RVA: 0x0004D6BD File Offset: 0x0004BABD
		public T2 Data
		{
			[CompilerGenerated]
			get
			{
				return this.<Data>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Data>k__BackingField = value;
			}
		}

		// Token: 0x04000D19 RID: 3353
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private T2 <Data>k__BackingField;
	}
}
