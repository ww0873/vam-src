using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Battlehub.UIControls
{
	// Token: 0x02000297 RID: 663
	public class VirtualizingParentChangedEventArgs : EventArgs
	{
		// Token: 0x06000F86 RID: 3974 RVA: 0x00059483 File Offset: 0x00057883
		public VirtualizingParentChangedEventArgs(TreeViewItemContainerData oldParent, TreeViewItemContainerData newParent)
		{
			this.OldParent = oldParent;
			this.NewParent = newParent;
		}

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000F87 RID: 3975 RVA: 0x00059499 File Offset: 0x00057899
		// (set) Token: 0x06000F88 RID: 3976 RVA: 0x000594A1 File Offset: 0x000578A1
		public TreeViewItemContainerData OldParent
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

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000F89 RID: 3977 RVA: 0x000594AA File Offset: 0x000578AA
		// (set) Token: 0x06000F8A RID: 3978 RVA: 0x000594B2 File Offset: 0x000578B2
		public TreeViewItemContainerData NewParent
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

		// Token: 0x04000E39 RID: 3641
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private TreeViewItemContainerData <OldParent>k__BackingField;

		// Token: 0x04000E3A RID: 3642
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private TreeViewItemContainerData <NewParent>k__BackingField;
	}
}
