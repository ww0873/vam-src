using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Battlehub.UIControls
{
	// Token: 0x0200028F RID: 655
	public class ItemContainerData
	{
		// Token: 0x06000EED RID: 3821 RVA: 0x000582C3 File Offset: 0x000566C3
		public ItemContainerData()
		{
		}

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000EEE RID: 3822 RVA: 0x000582CB File Offset: 0x000566CB
		// (set) Token: 0x06000EEF RID: 3823 RVA: 0x000582D3 File Offset: 0x000566D3
		public bool IsSelected
		{
			[CompilerGenerated]
			get
			{
				return this.<IsSelected>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<IsSelected>k__BackingField = value;
			}
		}

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000EF0 RID: 3824 RVA: 0x000582DC File Offset: 0x000566DC
		// (set) Token: 0x06000EF1 RID: 3825 RVA: 0x000582E4 File Offset: 0x000566E4
		public object Item
		{
			[CompilerGenerated]
			get
			{
				return this.<Item>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Item>k__BackingField = value;
			}
		}

		// Token: 0x04000DF9 RID: 3577
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool <IsSelected>k__BackingField;

		// Token: 0x04000DFA RID: 3578
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private object <Item>k__BackingField;
	}
}
