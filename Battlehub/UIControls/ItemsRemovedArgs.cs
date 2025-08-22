using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Battlehub.UIControls
{
	// Token: 0x02000272 RID: 626
	public class ItemsRemovedArgs : EventArgs
	{
		// Token: 0x06000D91 RID: 3473 RVA: 0x000529F2 File Offset: 0x00050DF2
		public ItemsRemovedArgs(object[] items)
		{
			this.Items = items;
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000D92 RID: 3474 RVA: 0x00052A01 File Offset: 0x00050E01
		// (set) Token: 0x06000D93 RID: 3475 RVA: 0x00052A09 File Offset: 0x00050E09
		public object[] Items
		{
			[CompilerGenerated]
			get
			{
				return this.<Items>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Items>k__BackingField = value;
			}
		}

		// Token: 0x04000D54 RID: 3412
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private object[] <Items>k__BackingField;
	}
}
