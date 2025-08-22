using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Leap
{
	// Token: 0x020005CA RID: 1482
	public class SetConfigResponseEventArgs : LeapEventArgs
	{
		// Token: 0x0600258F RID: 9615 RVA: 0x000D6C44 File Offset: 0x000D5044
		public SetConfigResponseEventArgs(string config_key, Config.ValueType dataType, object value, uint requestId) : base(LeapEvent.EVENT_CONFIG_RESPONSE)
		{
			this.ConfigKey = config_key;
			this.DataType = dataType;
			this.Value = value;
			this.RequestId = requestId;
		}

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06002590 RID: 9616 RVA: 0x000D6C6A File Offset: 0x000D506A
		// (set) Token: 0x06002591 RID: 9617 RVA: 0x000D6C72 File Offset: 0x000D5072
		public string ConfigKey
		{
			[CompilerGenerated]
			get
			{
				return this.<ConfigKey>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<ConfigKey>k__BackingField = value;
			}
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06002592 RID: 9618 RVA: 0x000D6C7B File Offset: 0x000D507B
		// (set) Token: 0x06002593 RID: 9619 RVA: 0x000D6C83 File Offset: 0x000D5083
		public Config.ValueType DataType
		{
			[CompilerGenerated]
			get
			{
				return this.<DataType>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<DataType>k__BackingField = value;
			}
		}

		// Token: 0x1700047E RID: 1150
		// (get) Token: 0x06002594 RID: 9620 RVA: 0x000D6C8C File Offset: 0x000D508C
		// (set) Token: 0x06002595 RID: 9621 RVA: 0x000D6C94 File Offset: 0x000D5094
		public object Value
		{
			[CompilerGenerated]
			get
			{
				return this.<Value>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Value>k__BackingField = value;
			}
		}

		// Token: 0x1700047F RID: 1151
		// (get) Token: 0x06002596 RID: 9622 RVA: 0x000D6C9D File Offset: 0x000D509D
		// (set) Token: 0x06002597 RID: 9623 RVA: 0x000D6CA5 File Offset: 0x000D50A5
		public uint RequestId
		{
			[CompilerGenerated]
			get
			{
				return this.<RequestId>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<RequestId>k__BackingField = value;
			}
		}

		// Token: 0x04001F50 RID: 8016
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <ConfigKey>k__BackingField;

		// Token: 0x04001F51 RID: 8017
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private Config.ValueType <DataType>k__BackingField;

		// Token: 0x04001F52 RID: 8018
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private object <Value>k__BackingField;

		// Token: 0x04001F53 RID: 8019
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private uint <RequestId>k__BackingField;
	}
}
