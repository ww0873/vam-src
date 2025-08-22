using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Battlehub.UIControls
{
	// Token: 0x02000284 RID: 644
	public class ItemExpandingArgs : EventArgs
	{
		// Token: 0x06000E4F RID: 3663 RVA: 0x00053862 File Offset: 0x00051C62
		public ItemExpandingArgs(object item)
		{
			this.Item = item;
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000E50 RID: 3664 RVA: 0x00053871 File Offset: 0x00051C71
		// (set) Token: 0x06000E51 RID: 3665 RVA: 0x00053879 File Offset: 0x00051C79
		public object Item
		{
			[CompilerGenerated]
			get
			{
				return this.<Item>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Item>k__BackingField = value;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000E52 RID: 3666 RVA: 0x00053882 File Offset: 0x00051C82
		// (set) Token: 0x06000E53 RID: 3667 RVA: 0x0005388A File Offset: 0x00051C8A
		public IEnumerable Children
		{
			[CompilerGenerated]
			get
			{
				return this.<Children>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Children>k__BackingField = value;
			}
		}

		// Token: 0x04000DBD RID: 3517
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private object <Item>k__BackingField;

		// Token: 0x04000DBE RID: 3518
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private IEnumerable <Children>k__BackingField;
	}
}
