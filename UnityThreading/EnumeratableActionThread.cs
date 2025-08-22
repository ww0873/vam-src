using System;
using System.Collections;

namespace UnityThreading
{
	// Token: 0x02000356 RID: 854
	public sealed class EnumeratableActionThread : ThreadBase
	{
		// Token: 0x06001507 RID: 5383 RVA: 0x00077DB3 File Offset: 0x000761B3
		public EnumeratableActionThread(Func<ThreadBase, IEnumerator> enumeratableAction) : this(enumeratableAction, true)
		{
		}

		// Token: 0x06001508 RID: 5384 RVA: 0x00077DBD File Offset: 0x000761BD
		public EnumeratableActionThread(Func<ThreadBase, IEnumerator> enumeratableAction, bool autoStartThread) : base("EnumeratableActionThread", Dispatcher.Current, false)
		{
			this.enumeratableAction = enumeratableAction;
			if (autoStartThread)
			{
				base.Start();
			}
		}

		// Token: 0x06001509 RID: 5385 RVA: 0x00077DE3 File Offset: 0x000761E3
		protected override IEnumerator Do()
		{
			return this.enumeratableAction(this);
		}

		// Token: 0x040011B4 RID: 4532
		private Func<ThreadBase, IEnumerator> enumeratableAction;
	}
}
