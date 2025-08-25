using System;
using System.Threading;

namespace UnityThreading
{
	// Token: 0x02000359 RID: 857
	public static class WaitOneExtension
	{
		// Token: 0x06001522 RID: 5410 RVA: 0x000782DC File Offset: 0x000766DC
		public static bool InterWaitOne(this ManualResetEvent that, int ms)
		{
			return that.WaitOne(ms, false);
		}

		// Token: 0x06001523 RID: 5411 RVA: 0x000782E6 File Offset: 0x000766E6
		public static bool InterWaitOne(this ManualResetEvent that, TimeSpan duration)
		{
			return that.WaitOne(duration, false);
		}
	}
}
