using System;
using System.Collections;

namespace UnityThreading
{
	// Token: 0x02000355 RID: 853
	public sealed class ActionThread : ThreadBase
	{
		// Token: 0x06001504 RID: 5380 RVA: 0x00077D74 File Offset: 0x00076174
		public ActionThread(Action<ActionThread> action) : this(action, true)
		{
		}

		// Token: 0x06001505 RID: 5381 RVA: 0x00077D7E File Offset: 0x0007617E
		public ActionThread(Action<ActionThread> action, bool autoStartThread) : base("ActionThread", Dispatcher.Current, false)
		{
			this.action = action;
			if (autoStartThread)
			{
				base.Start();
			}
		}

		// Token: 0x06001506 RID: 5382 RVA: 0x00077DA4 File Offset: 0x000761A4
		protected override IEnumerator Do()
		{
			this.action(this);
			return null;
		}

		// Token: 0x040011B3 RID: 4531
		private Action<ActionThread> action;
	}
}
