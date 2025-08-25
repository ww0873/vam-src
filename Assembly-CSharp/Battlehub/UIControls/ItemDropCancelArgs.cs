using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Battlehub.UIControls
{
	// Token: 0x02000275 RID: 629
	public class ItemDropCancelArgs : ItemDropArgs
	{
		// Token: 0x06000DA4 RID: 3492 RVA: 0x00052B1E File Offset: 0x00050F1E
		public ItemDropCancelArgs(object[] dragItems, object dropTarget, ItemDropAction action, bool isExternal) : base(dragItems, dropTarget, action, isExternal)
		{
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000DA5 RID: 3493 RVA: 0x00052B2B File Offset: 0x00050F2B
		// (set) Token: 0x06000DA6 RID: 3494 RVA: 0x00052B33 File Offset: 0x00050F33
		public bool Cancel
		{
			[CompilerGenerated]
			get
			{
				return this.<Cancel>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Cancel>k__BackingField = value;
			}
		}

		// Token: 0x04000D5C RID: 3420
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool <Cancel>k__BackingField;
	}
}
