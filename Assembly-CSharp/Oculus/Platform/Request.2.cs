using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Oculus.Platform
{
	// Token: 0x0200089A RID: 2202
	public class Request
	{
		// Token: 0x060037BF RID: 14271 RVA: 0x0010EA07 File Offset: 0x0010CE07
		public Request(ulong requestID)
		{
			this.RequestID = requestID;
		}

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x060037C0 RID: 14272 RVA: 0x0010EA16 File Offset: 0x0010CE16
		// (set) Token: 0x060037C1 RID: 14273 RVA: 0x0010EA1E File Offset: 0x0010CE1E
		public ulong RequestID
		{
			[CompilerGenerated]
			get
			{
				return this.<RequestID>k__BackingField;
			}
			[CompilerGenerated]
			set
			{
				this.<RequestID>k__BackingField = value;
			}
		}

		// Token: 0x060037C2 RID: 14274 RVA: 0x0010EA27 File Offset: 0x0010CE27
		public Request OnComplete(Message.Callback callback)
		{
			Callback.OnComplete(this, callback);
			return this;
		}

		// Token: 0x060037C3 RID: 14275 RVA: 0x0010EA31 File Offset: 0x0010CE31
		public static void RunCallbacks(uint limit = 0U)
		{
			if (limit == 0U)
			{
				Callback.RunCallbacks();
			}
			else
			{
				Callback.RunLimitedCallbacks(limit);
			}
		}

		// Token: 0x040028D6 RID: 10454
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private ulong <RequestID>k__BackingField;
	}
}
