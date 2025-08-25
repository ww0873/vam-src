using System;

namespace UnityThreading
{
	// Token: 0x02000345 RID: 837
	public class NullDispatcher : DispatcherBase
	{
		// Token: 0x0600144D RID: 5197 RVA: 0x000752E2 File Offset: 0x000736E2
		public NullDispatcher()
		{
		}

		// Token: 0x0600144E RID: 5198 RVA: 0x000752EA File Offset: 0x000736EA
		protected override void CheckAccessLimitation()
		{
		}

		// Token: 0x0600144F RID: 5199 RVA: 0x000752EC File Offset: 0x000736EC
		internal override void AddTask(Task task)
		{
			task.DoInternal();
		}

		// Token: 0x06001450 RID: 5200 RVA: 0x000752F4 File Offset: 0x000736F4
		// Note: this type is marked as 'beforefieldinit'.
		static NullDispatcher()
		{
		}

		// Token: 0x04001189 RID: 4489
		public static NullDispatcher Null = new NullDispatcher();
	}
}
