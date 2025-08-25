using System;
using System.Collections;
using System.Threading;

namespace UnityThreading
{
	// Token: 0x02000357 RID: 855
	public sealed class TickThread : ThreadBase
	{
		// Token: 0x0600150A RID: 5386 RVA: 0x00077DF1 File Offset: 0x000761F1
		public TickThread(Action action, int tickLengthInMilliseconds) : this(action, tickLengthInMilliseconds, true)
		{
		}

		// Token: 0x0600150B RID: 5387 RVA: 0x00077DFC File Offset: 0x000761FC
		public TickThread(Action action, int tickLengthInMilliseconds, bool autoStartThread) : base("TickThread", Dispatcher.CurrentNoThrow, false)
		{
			this.tickLengthInMilliseconds = tickLengthInMilliseconds;
			this.action = action;
			if (autoStartThread)
			{
				base.Start();
			}
		}

		// Token: 0x0600150C RID: 5388 RVA: 0x00077E38 File Offset: 0x00076238
		protected override IEnumerator Do()
		{
			while (!this.exitEvent.InterWaitOne(0))
			{
				this.action();
				if (WaitHandle.WaitAny(new WaitHandle[]
				{
					this.exitEvent,
					this.tickEvent
				}, this.tickLengthInMilliseconds) == 0)
				{
					return null;
				}
			}
			return null;
		}

		// Token: 0x040011B5 RID: 4533
		private Action action;

		// Token: 0x040011B6 RID: 4534
		private int tickLengthInMilliseconds;

		// Token: 0x040011B7 RID: 4535
		private ManualResetEvent tickEvent = new ManualResetEvent(false);
	}
}
