using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Battlehub.UIControls
{
	// Token: 0x02000276 RID: 630
	public class ItemDropArgs : EventArgs
	{
		// Token: 0x06000DA7 RID: 3495 RVA: 0x00052AB5 File Offset: 0x00050EB5
		public ItemDropArgs(object[] dragItems, object dropTarget, ItemDropAction action, bool isExternal)
		{
			this.DragItems = dragItems;
			this.DropTarget = dropTarget;
			this.Action = action;
			this.IsExternal = isExternal;
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x06000DA8 RID: 3496 RVA: 0x00052ADA File Offset: 0x00050EDA
		// (set) Token: 0x06000DA9 RID: 3497 RVA: 0x00052AE2 File Offset: 0x00050EE2
		public object[] DragItems
		{
			[CompilerGenerated]
			get
			{
				return this.<DragItems>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<DragItems>k__BackingField = value;
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x06000DAA RID: 3498 RVA: 0x00052AEB File Offset: 0x00050EEB
		// (set) Token: 0x06000DAB RID: 3499 RVA: 0x00052AF3 File Offset: 0x00050EF3
		public object DropTarget
		{
			[CompilerGenerated]
			get
			{
				return this.<DropTarget>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<DropTarget>k__BackingField = value;
			}
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x06000DAC RID: 3500 RVA: 0x00052AFC File Offset: 0x00050EFC
		// (set) Token: 0x06000DAD RID: 3501 RVA: 0x00052B04 File Offset: 0x00050F04
		public ItemDropAction Action
		{
			[CompilerGenerated]
			get
			{
				return this.<Action>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Action>k__BackingField = value;
			}
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x06000DAE RID: 3502 RVA: 0x00052B0D File Offset: 0x00050F0D
		// (set) Token: 0x06000DAF RID: 3503 RVA: 0x00052B15 File Offset: 0x00050F15
		public bool IsExternal
		{
			[CompilerGenerated]
			get
			{
				return this.<IsExternal>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<IsExternal>k__BackingField = value;
			}
		}

		// Token: 0x04000D5D RID: 3421
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private object[] <DragItems>k__BackingField;

		// Token: 0x04000D5E RID: 3422
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private object <DropTarget>k__BackingField;

		// Token: 0x04000D5F RID: 3423
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private ItemDropAction <Action>k__BackingField;

		// Token: 0x04000D60 RID: 3424
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool <IsExternal>k__BackingField;
	}
}
