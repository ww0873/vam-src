using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Leap
{
	// Token: 0x020005C9 RID: 1481
	public class ConfigChangeEventArgs : LeapEventArgs
	{
		// Token: 0x06002588 RID: 9608 RVA: 0x000D6BF3 File Offset: 0x000D4FF3
		public ConfigChangeEventArgs(string config_key, bool succeeded, uint requestId) : base(LeapEvent.EVENT_CONFIG_CHANGE)
		{
			this.ConfigKey = config_key;
			this.Succeeded = succeeded;
			this.RequestId = requestId;
		}

		// Token: 0x17000479 RID: 1145
		// (get) Token: 0x06002589 RID: 9609 RVA: 0x000D6C11 File Offset: 0x000D5011
		// (set) Token: 0x0600258A RID: 9610 RVA: 0x000D6C19 File Offset: 0x000D5019
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

		// Token: 0x1700047A RID: 1146
		// (get) Token: 0x0600258B RID: 9611 RVA: 0x000D6C22 File Offset: 0x000D5022
		// (set) Token: 0x0600258C RID: 9612 RVA: 0x000D6C2A File Offset: 0x000D502A
		public bool Succeeded
		{
			[CompilerGenerated]
			get
			{
				return this.<Succeeded>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<Succeeded>k__BackingField = value;
			}
		}

		// Token: 0x1700047B RID: 1147
		// (get) Token: 0x0600258D RID: 9613 RVA: 0x000D6C33 File Offset: 0x000D5033
		// (set) Token: 0x0600258E RID: 9614 RVA: 0x000D6C3B File Offset: 0x000D503B
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

		// Token: 0x04001F4D RID: 8013
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <ConfigKey>k__BackingField;

		// Token: 0x04001F4E RID: 8014
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private bool <Succeeded>k__BackingField;

		// Token: 0x04001F4F RID: 8015
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private uint <RequestId>k__BackingField;
	}
}
