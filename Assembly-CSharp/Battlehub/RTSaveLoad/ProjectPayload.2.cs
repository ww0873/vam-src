using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x02000250 RID: 592
	public class ProjectPayload<T> : ProjectPayload
	{
		// Token: 0x06000C46 RID: 3142 RVA: 0x0004AF84 File Offset: 0x00049384
		public ProjectPayload(T data)
		{
			this.Data = data;
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000C47 RID: 3143 RVA: 0x0004AF93 File Offset: 0x00049393
		// (set) Token: 0x06000C48 RID: 3144 RVA: 0x0004AF9B File Offset: 0x0004939B
		public T Data
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

		// Token: 0x04000CDF RID: 3295
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private T <Data>k__BackingField;
	}
}
