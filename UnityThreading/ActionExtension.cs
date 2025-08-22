using System;

namespace UnityThreading
{
	// Token: 0x02000341 RID: 833
	public static class ActionExtension
	{
		// Token: 0x06001429 RID: 5161 RVA: 0x00074964 File Offset: 0x00072D64
		public static Task RunAsync(this Action that)
		{
			return that.RunAsync(UnityThreadHelper.TaskDistributor);
		}

		// Token: 0x0600142A RID: 5162 RVA: 0x00074971 File Offset: 0x00072D71
		public static Task RunAsync(this Action that, TaskDistributor target)
		{
			return target.Dispatch(that);
		}

		// Token: 0x0600142B RID: 5163 RVA: 0x0007497A File Offset: 0x00072D7A
		public static Task AsTask(this Action that)
		{
			return Task.Create(that);
		}

		// Token: 0x0600142C RID: 5164 RVA: 0x00074982 File Offset: 0x00072D82
		public static Task<T> RunAsync<T>(this Func<T> that)
		{
			return that.RunAsync(UnityThreadHelper.TaskDistributor);
		}

		// Token: 0x0600142D RID: 5165 RVA: 0x0007498F File Offset: 0x00072D8F
		public static Task<T> RunAsync<T>(this Func<T> that, TaskDistributor target)
		{
			return target.Dispatch<T>(that);
		}

		// Token: 0x0600142E RID: 5166 RVA: 0x00074998 File Offset: 0x00072D98
		public static Task<T> AsTask<T>(this Func<T> that)
		{
			return new Task<T>(that);
		}
	}
}
