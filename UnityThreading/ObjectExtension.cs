using System;

namespace UnityThreading
{
	// Token: 0x02000349 RID: 841
	public static class ObjectExtension
	{
		// Token: 0x0600146E RID: 5230 RVA: 0x00075AD2 File Offset: 0x00073ED2
		public static Task RunAsync(this object that, string methodName, params object[] args)
		{
			return that.RunAsync(methodName, null, args);
		}

		// Token: 0x0600146F RID: 5231 RVA: 0x00075ADD File Offset: 0x00073EDD
		public static Task RunAsync(this object that, string methodName, TaskDistributor target, params object[] args)
		{
			return that.RunAsync(methodName, target, args);
		}

		// Token: 0x06001470 RID: 5232 RVA: 0x00075AE8 File Offset: 0x00073EE8
		public static Task<T> RunAsync<T>(this object that, string methodName, params object[] args)
		{
			return that.RunAsync(methodName, null, args);
		}

		// Token: 0x06001471 RID: 5233 RVA: 0x00075AF3 File Offset: 0x00073EF3
		public static Task<T> RunAsync<T>(this object that, string methodName, TaskDistributor target, params object[] args)
		{
			return Task.Create<T>(that, methodName, args).Run(target);
		}
	}
}
