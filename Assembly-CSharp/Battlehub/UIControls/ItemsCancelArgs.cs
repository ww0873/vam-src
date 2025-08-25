using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Battlehub.UIControls
{
	// Token: 0x02000271 RID: 625
	public class ItemsCancelArgs : EventArgs
	{
		// Token: 0x06000D8E RID: 3470 RVA: 0x000529D2 File Offset: 0x00050DD2
		public ItemsCancelArgs(List<object> items)
		{
			this.Items = items;
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06000D8F RID: 3471 RVA: 0x000529E1 File Offset: 0x00050DE1
		// (set) Token: 0x06000D90 RID: 3472 RVA: 0x000529E9 File Offset: 0x00050DE9
		public List<object> Items
		{
			[CompilerGenerated]
			get
			{
				return this.<Items>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Items>k__BackingField = value;
			}
		}

		// Token: 0x04000D53 RID: 3411
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private List<object> <Items>k__BackingField;
	}
}
