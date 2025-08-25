using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Battlehub.UIControls
{
	// Token: 0x02000270 RID: 624
	public class SelectionChangedArgs : EventArgs
	{
		// Token: 0x06000D86 RID: 3462 RVA: 0x00052926 File Offset: 0x00050D26
		public SelectionChangedArgs(object[] oldItems, object[] newItems)
		{
			this.OldItems = oldItems;
			this.NewItems = newItems;
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x0005293C File Offset: 0x00050D3C
		public SelectionChangedArgs(object oldItem, object newItem)
		{
			this.OldItems = new object[]
			{
				oldItem
			};
			this.NewItems = new object[]
			{
				newItem
			};
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x06000D88 RID: 3464 RVA: 0x00052964 File Offset: 0x00050D64
		// (set) Token: 0x06000D89 RID: 3465 RVA: 0x0005296C File Offset: 0x00050D6C
		public object[] OldItems
		{
			[CompilerGenerated]
			get
			{
				return this.<OldItems>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<OldItems>k__BackingField = value;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x06000D8A RID: 3466 RVA: 0x00052975 File Offset: 0x00050D75
		// (set) Token: 0x06000D8B RID: 3467 RVA: 0x0005297D File Offset: 0x00050D7D
		public object[] NewItems
		{
			[CompilerGenerated]
			get
			{
				return this.<NewItems>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<NewItems>k__BackingField = value;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06000D8C RID: 3468 RVA: 0x00052986 File Offset: 0x00050D86
		public object OldItem
		{
			get
			{
				if (this.OldItems == null)
				{
					return null;
				}
				if (this.OldItems.Length == 0)
				{
					return null;
				}
				return this.OldItems[0];
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06000D8D RID: 3469 RVA: 0x000529AC File Offset: 0x00050DAC
		public object NewItem
		{
			get
			{
				if (this.NewItems == null)
				{
					return null;
				}
				if (this.NewItems.Length == 0)
				{
					return null;
				}
				return this.NewItems[0];
			}
		}

		// Token: 0x04000D51 RID: 3409
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private object[] <OldItems>k__BackingField;

		// Token: 0x04000D52 RID: 3410
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private object[] <NewItems>k__BackingField;
	}
}
