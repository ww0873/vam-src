using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Oculus.Platform.Models
{
	// Token: 0x02000864 RID: 2148
	public class PingResult
	{
		// Token: 0x0600370B RID: 14091 RVA: 0x0010CC6A File Offset: 0x0010B06A
		public PingResult(ulong id, ulong? pingTimeUsec)
		{
			this.ID = id;
			this.pingTimeUsec = pingTimeUsec;
		}

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x0600370C RID: 14092 RVA: 0x0010CC80 File Offset: 0x0010B080
		// (set) Token: 0x0600370D RID: 14093 RVA: 0x0010CC88 File Offset: 0x0010B088
		public ulong ID
		{
			[CompilerGenerated]
			get
			{
				return this.<ID>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<ID>k__BackingField = value;
			}
		}

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x0600370E RID: 14094 RVA: 0x0010CC91 File Offset: 0x0010B091
		public ulong PingTimeUsec
		{
			get
			{
				return (this.pingTimeUsec == null) ? 0UL : this.pingTimeUsec.Value;
			}
		}

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x0600370F RID: 14095 RVA: 0x0010CCB5 File Offset: 0x0010B0B5
		public bool IsTimeout
		{
			get
			{
				return this.pingTimeUsec == null;
			}
		}

		// Token: 0x0400285A RID: 10330
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private ulong <ID>k__BackingField;

		// Token: 0x0400285B RID: 10331
		private ulong? pingTimeUsec;
	}
}
