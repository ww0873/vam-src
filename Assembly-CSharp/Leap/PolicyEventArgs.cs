using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Leap
{
	// Token: 0x020005C7 RID: 1479
	public class PolicyEventArgs : LeapEventArgs
	{
		// Token: 0x0600257E RID: 9598 RVA: 0x000D6B80 File Offset: 0x000D4F80
		public PolicyEventArgs(ulong currentPolicies, ulong oldPolicies) : base(LeapEvent.EVENT_POLICY_CHANGE)
		{
			this.currentPolicies = currentPolicies;
			this.oldPolicies = oldPolicies;
		}

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x0600257F RID: 9599 RVA: 0x000D6B97 File Offset: 0x000D4F97
		// (set) Token: 0x06002580 RID: 9600 RVA: 0x000D6B9F File Offset: 0x000D4F9F
		public ulong currentPolicies
		{
			[CompilerGenerated]
			get
			{
				return this.<currentPolicies>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<currentPolicies>k__BackingField = value;
			}
		}

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x06002581 RID: 9601 RVA: 0x000D6BA8 File Offset: 0x000D4FA8
		// (set) Token: 0x06002582 RID: 9602 RVA: 0x000D6BB0 File Offset: 0x000D4FB0
		public ulong oldPolicies
		{
			[CompilerGenerated]
			get
			{
				return this.<oldPolicies>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<oldPolicies>k__BackingField = value;
			}
		}

		// Token: 0x04001F49 RID: 8009
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private ulong <currentPolicies>k__BackingField;

		// Token: 0x04001F4A RID: 8010
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private ulong <oldPolicies>k__BackingField;
	}
}
