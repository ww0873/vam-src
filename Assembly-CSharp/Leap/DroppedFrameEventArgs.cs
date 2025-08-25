using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using LeapInternal;

namespace Leap
{
	// Token: 0x020005CF RID: 1487
	public class DroppedFrameEventArgs : LeapEventArgs
	{
		// Token: 0x060025A4 RID: 9636 RVA: 0x000D6D32 File Offset: 0x000D5132
		public DroppedFrameEventArgs(long frame_id, eLeapDroppedFrameType type) : base(LeapEvent.EVENT_DROPPED_FRAME)
		{
			this.frameID = frame_id;
			this.reason = type;
		}

		// Token: 0x17000484 RID: 1156
		// (get) Token: 0x060025A5 RID: 9637 RVA: 0x000D6D4A File Offset: 0x000D514A
		// (set) Token: 0x060025A6 RID: 9638 RVA: 0x000D6D52 File Offset: 0x000D5152
		public long frameID
		{
			[CompilerGenerated]
			get
			{
				return this.<frameID>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<frameID>k__BackingField = value;
			}
		}

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x060025A7 RID: 9639 RVA: 0x000D6D5B File Offset: 0x000D515B
		// (set) Token: 0x060025A8 RID: 9640 RVA: 0x000D6D63 File Offset: 0x000D5163
		public eLeapDroppedFrameType reason
		{
			[CompilerGenerated]
			get
			{
				return this.<reason>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<reason>k__BackingField = value;
			}
		}

		// Token: 0x04001F58 RID: 8024
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private long <frameID>k__BackingField;

		// Token: 0x04001F59 RID: 8025
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private eLeapDroppedFrameType <reason>k__BackingField;
	}
}
