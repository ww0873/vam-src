using System;
using System.Collections;

namespace UnityThreading
{
	// Token: 0x02000348 RID: 840
	public static class EnumeratorExtension
	{
		// Token: 0x0600146C RID: 5228 RVA: 0x00075AB7 File Offset: 0x00073EB7
		public static Task RunAsync(this IEnumerator that)
		{
			return that.RunAsync(UnityThreadHelper.TaskDistributor);
		}

		// Token: 0x0600146D RID: 5229 RVA: 0x00075AC4 File Offset: 0x00073EC4
		public static Task RunAsync(this IEnumerator that, TaskDistributor target)
		{
			return target.Dispatch(Task.Create(that));
		}
	}
}
