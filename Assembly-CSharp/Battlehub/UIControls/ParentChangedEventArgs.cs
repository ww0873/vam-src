using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Battlehub.UIControls
{
	// Token: 0x02000289 RID: 649
	public class ParentChangedEventArgs : EventArgs
	{
		// Token: 0x06000E82 RID: 3714 RVA: 0x000547FB File Offset: 0x00052BFB
		public ParentChangedEventArgs(TreeViewItem oldParent, TreeViewItem newParent)
		{
			this.OldParent = oldParent;
			this.NewParent = newParent;
		}

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000E83 RID: 3715 RVA: 0x00054811 File Offset: 0x00052C11
		// (set) Token: 0x06000E84 RID: 3716 RVA: 0x00054819 File Offset: 0x00052C19
		public TreeViewItem OldParent
		{
			[CompilerGenerated]
			get
			{
				return this.<OldParent>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<OldParent>k__BackingField = value;
			}
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x06000E85 RID: 3717 RVA: 0x00054822 File Offset: 0x00052C22
		// (set) Token: 0x06000E86 RID: 3718 RVA: 0x0005482A File Offset: 0x00052C2A
		public TreeViewItem NewParent
		{
			[CompilerGenerated]
			get
			{
				return this.<NewParent>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<NewParent>k__BackingField = value;
			}
		}

		// Token: 0x04000DCB RID: 3531
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private TreeViewItem <OldParent>k__BackingField;

		// Token: 0x04000DCC RID: 3532
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private TreeViewItem <NewParent>k__BackingField;
	}
}
