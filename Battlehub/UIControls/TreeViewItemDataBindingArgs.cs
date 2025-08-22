using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Battlehub.UIControls
{
	// Token: 0x02000285 RID: 645
	public class TreeViewItemDataBindingArgs : ItemDataBindingArgs
	{
		// Token: 0x06000E54 RID: 3668 RVA: 0x00053893 File Offset: 0x00051C93
		public TreeViewItemDataBindingArgs()
		{
		}

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000E55 RID: 3669 RVA: 0x0005389B File Offset: 0x00051C9B
		// (set) Token: 0x06000E56 RID: 3670 RVA: 0x000538A3 File Offset: 0x00051CA3
		public bool HasChildren
		{
			[CompilerGenerated]
			get
			{
				return this.<HasChildren>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<HasChildren>k__BackingField = value;
			}
		}

		// Token: 0x04000DBF RID: 3519
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool <HasChildren>k__BackingField;
	}
}
