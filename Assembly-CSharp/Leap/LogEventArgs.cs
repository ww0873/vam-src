using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Leap
{
	// Token: 0x020005C6 RID: 1478
	public class LogEventArgs : LeapEventArgs
	{
		// Token: 0x06002577 RID: 9591 RVA: 0x000D6B2E File Offset: 0x000D4F2E
		public LogEventArgs(MessageSeverity severity, long timestamp, string message) : base(LeapEvent.EVENT_LOG_EVENT)
		{
			this.severity = severity;
			this.message = message;
			this.timestamp = timestamp;
		}

		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x06002578 RID: 9592 RVA: 0x000D6B4D File Offset: 0x000D4F4D
		// (set) Token: 0x06002579 RID: 9593 RVA: 0x000D6B55 File Offset: 0x000D4F55
		public MessageSeverity severity
		{
			[CompilerGenerated]
			get
			{
				return this.<severity>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<severity>k__BackingField = value;
			}
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x0600257A RID: 9594 RVA: 0x000D6B5E File Offset: 0x000D4F5E
		// (set) Token: 0x0600257B RID: 9595 RVA: 0x000D6B66 File Offset: 0x000D4F66
		public long timestamp
		{
			[CompilerGenerated]
			get
			{
				return this.<timestamp>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<timestamp>k__BackingField = value;
			}
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x0600257C RID: 9596 RVA: 0x000D6B6F File Offset: 0x000D4F6F
		// (set) Token: 0x0600257D RID: 9597 RVA: 0x000D6B77 File Offset: 0x000D4F77
		public string message
		{
			[CompilerGenerated]
			get
			{
				return this.<message>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<message>k__BackingField = value;
			}
		}

		// Token: 0x04001F46 RID: 8006
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private MessageSeverity <severity>k__BackingField;

		// Token: 0x04001F47 RID: 8007
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private long <timestamp>k__BackingField;

		// Token: 0x04001F48 RID: 8008
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <message>k__BackingField;
	}
}
