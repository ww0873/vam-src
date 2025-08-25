using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using LeapInternal;

namespace Leap
{
	// Token: 0x020005C5 RID: 1477
	public class InternalFrameEventArgs : LeapEventArgs
	{
		// Token: 0x06002574 RID: 9588 RVA: 0x000D6B07 File Offset: 0x000D4F07
		public InternalFrameEventArgs(ref LEAP_TRACKING_EVENT frame) : base(LeapEvent.EVENT_INTERNAL_FRAME)
		{
			this.frame = frame;
		}

		// Token: 0x17000471 RID: 1137
		// (get) Token: 0x06002575 RID: 9589 RVA: 0x000D6B1D File Offset: 0x000D4F1D
		// (set) Token: 0x06002576 RID: 9590 RVA: 0x000D6B25 File Offset: 0x000D4F25
		public LEAP_TRACKING_EVENT frame
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

		// Token: 0x04001F45 RID: 8005
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private LEAP_TRACKING_EVENT <frame>k__BackingField;
	}
}
