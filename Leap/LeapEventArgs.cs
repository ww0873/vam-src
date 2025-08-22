using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Leap
{
	// Token: 0x020005C3 RID: 1475
	public class LeapEventArgs : EventArgs
	{
		// Token: 0x0600256E RID: 9582 RVA: 0x000D6AC6 File Offset: 0x000D4EC6
		public LeapEventArgs(LeapEvent type)
		{
			this.type = type;
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x0600256F RID: 9583 RVA: 0x000D6AD5 File Offset: 0x000D4ED5
		// (set) Token: 0x06002570 RID: 9584 RVA: 0x000D6ADD File Offset: 0x000D4EDD
		public LeapEvent type
		{
			[CompilerGenerated]
			get
			{
				return this.<type>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<type>k__BackingField = value;
			}
		}

		// Token: 0x04001F43 RID: 8003
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private LeapEvent <type>k__BackingField;
	}
}
