using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Oculus.Platform.Models
{
	// Token: 0x0200085E RID: 2142
	public class NetworkingPeer
	{
		// Token: 0x06003701 RID: 14081 RVA: 0x0010CA90 File Offset: 0x0010AE90
		public NetworkingPeer(ulong id, PeerConnectionState state)
		{
			this.ID = id;
			this.State = state;
		}

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06003702 RID: 14082 RVA: 0x0010CAA6 File Offset: 0x0010AEA6
		// (set) Token: 0x06003703 RID: 14083 RVA: 0x0010CAAE File Offset: 0x0010AEAE
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

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x06003704 RID: 14084 RVA: 0x0010CAB7 File Offset: 0x0010AEB7
		// (set) Token: 0x06003705 RID: 14085 RVA: 0x0010CABF File Offset: 0x0010AEBF
		public PeerConnectionState State
		{
			[CompilerGenerated]
			get
			{
				return this.<State>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<State>k__BackingField = value;
			}
		}

		// Token: 0x0400284C RID: 10316
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private ulong <ID>k__BackingField;

		// Token: 0x0400284D RID: 10317
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private PeerConnectionState <State>k__BackingField;
	}
}
