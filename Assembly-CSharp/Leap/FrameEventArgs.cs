using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Leap
{
	// Token: 0x020005C4 RID: 1476
	public class FrameEventArgs : LeapEventArgs
	{
		// Token: 0x06002571 RID: 9585 RVA: 0x000D6AE6 File Offset: 0x000D4EE6
		public FrameEventArgs(Frame frame) : base(LeapEvent.EVENT_FRAME)
		{
			this.frame = frame;
		}

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x06002572 RID: 9586 RVA: 0x000D6AF6 File Offset: 0x000D4EF6
		// (set) Token: 0x06002573 RID: 9587 RVA: 0x000D6AFE File Offset: 0x000D4EFE
		public Frame frame
		{
			[CompilerGenerated]
			get
			{
				return this.<frame>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<frame>k__BackingField = value;
			}
		}

		// Token: 0x04001F44 RID: 8004
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Frame <frame>k__BackingField;
	}
}
