using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Battlehub.UIControls
{
	// Token: 0x02000274 RID: 628
	public class ItemArgs : EventArgs
	{
		// Token: 0x06000DA1 RID: 3489 RVA: 0x00052A95 File Offset: 0x00050E95
		public ItemArgs(object[] item)
		{
			this.Items = item;
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x06000DA2 RID: 3490 RVA: 0x00052AA4 File Offset: 0x00050EA4
		// (set) Token: 0x06000DA3 RID: 3491 RVA: 0x00052AAC File Offset: 0x00050EAC
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

		// Token: 0x04000D5B RID: 3419
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private object[] <Items>k__BackingField;
	}
}
