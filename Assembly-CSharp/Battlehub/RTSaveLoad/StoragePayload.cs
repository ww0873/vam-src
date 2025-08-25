using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x0200025E RID: 606
	public class StoragePayload<T>
	{
		// Token: 0x06000CC3 RID: 3267 RVA: 0x0004D685 File Offset: 0x0004BA85
		public StoragePayload(T path)
		{
			this.Path = path;
		}

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000CC4 RID: 3268 RVA: 0x0004D694 File Offset: 0x0004BA94
		// (set) Token: 0x06000CC5 RID: 3269 RVA: 0x0004D69C File Offset: 0x0004BA9C
		public T Path
		{
			[CompilerGenerated]
			get
			{
				return this.<Path>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Path>k__BackingField = value;
			}
		}

		// Token: 0x04000D18 RID: 3352
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private T <Path>k__BackingField;
	}
}
