using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace UnityThreading
{
	// Token: 0x02000353 RID: 851
	public static class TaskExtension
	{
		// Token: 0x060014C2 RID: 5314 RVA: 0x00076DEE File Offset: 0x000751EE
		public static Task WithName(this Task task, string name)
		{
			task.Name = name;
			return task;
		}

		// Token: 0x060014C3 RID: 5315 RVA: 0x00076DF8 File Offset: 0x000751F8
		public static Task<T> WithName<T>(this Task<T> task, string name)
		{
			task.Name = name;
			return task;
		}

		// Token: 0x060014C4 RID: 5316 RVA: 0x00076E04 File Offset: 0x00075204
		public static void WaitAll(this IEnumerable<Task> tasks)
		{
			foreach (Task task in tasks)
			{
				task.Wait();
			}
		}

		// Token: 0x060014C5 RID: 5317 RVA: 0x00076E58 File Offset: 0x00075258
		public static IEnumerable<Task> Then(this IEnumerable<Task> that, Task followingTask, DispatcherBase target)
		{
			TaskExtension.<Then>c__AnonStorey0 <Then>c__AnonStorey = new TaskExtension.<Then>c__AnonStorey0();
			<Then>c__AnonStorey.followingTask = followingTask;
			<Then>c__AnonStorey.target = target;
			<Then>c__AnonStorey.remaining = that.Count<Task>();
			<Then>c__AnonStorey.syncRoot = new object();
			foreach (Task task in that)
			{
				task.WhenFailed(new Action(<Then>c__AnonStorey.<>m__0));
				task.WhenSucceeded(new Action(<Then>c__AnonStorey.<>m__1));
			}
			return that;
		}

		// Token: 0x060014C6 RID: 5318 RVA: 0x00076EF8 File Offset: 0x000752F8
		public static IEnumerable<Task> WhenSucceeded(this IEnumerable<Task> that, Action action, DispatcherBase target)
		{
			TaskExtension.<WhenSucceeded>c__AnonStorey1 <WhenSucceeded>c__AnonStorey = new TaskExtension.<WhenSucceeded>c__AnonStorey1();
			<WhenSucceeded>c__AnonStorey.target = target;
			<WhenSucceeded>c__AnonStorey.action = action;
			<WhenSucceeded>c__AnonStorey.remaining = that.Count<Task>();
			<WhenSucceeded>c__AnonStorey.syncRoot = new object();
			foreach (Task task in that)
			{
				task.WhenSucceeded(new Action(<WhenSucceeded>c__AnonStorey.<>m__0));
			}
			return that;
		}

		// Token: 0x060014C7 RID: 5319 RVA: 0x00076F84 File Offset: 0x00075384
		public static IEnumerable<Task> WhenFailed(this IEnumerable<Task> that, Action action, DispatcherBase target)
		{
			TaskExtension.<WhenFailed>c__AnonStorey2 <WhenFailed>c__AnonStorey = new TaskExtension.<WhenFailed>c__AnonStorey2();
			<WhenFailed>c__AnonStorey.target = target;
			<WhenFailed>c__AnonStorey.action = action;
			<WhenFailed>c__AnonStorey.hasFailed = false;
			<WhenFailed>c__AnonStorey.syncRoot = new object();
			foreach (Task task in that)
			{
				task.WhenFailed(new Action(<WhenFailed>c__AnonStorey.<>m__0));
			}
			return that;
		}

		// Token: 0x060014C8 RID: 5320 RVA: 0x0007700C File Offset: 0x0007540C
		public static Task OnResult(this Task task, Action<object> action)
		{
			return task.OnResult(action, null);
		}

		// Token: 0x060014C9 RID: 5321 RVA: 0x00077018 File Offset: 0x00075418
		public static Task OnResult(this Task task, Action<object> action, DispatcherBase target)
		{
			TaskExtension.<OnResult>c__AnonStorey3 <OnResult>c__AnonStorey = new TaskExtension.<OnResult>c__AnonStorey3();
			<OnResult>c__AnonStorey.action = action;
			return task.WhenSucceeded(new Action<Task>(<OnResult>c__AnonStorey.<>m__0), target);
		}

		// Token: 0x060014CA RID: 5322 RVA: 0x00077045 File Offset: 0x00075445
		public static Task OnResult<T>(this Task task, Action<T> action)
		{
			return task.OnResult(action, null);
		}

		// Token: 0x060014CB RID: 5323 RVA: 0x00077050 File Offset: 0x00075450
		public static Task OnResult<T>(this Task task, Action<T> action, DispatcherBase target)
		{
			TaskExtension.<OnResult>c__AnonStorey4<T> <OnResult>c__AnonStorey = new TaskExtension.<OnResult>c__AnonStorey4<T>();
			<OnResult>c__AnonStorey.action = action;
			return task.WhenSucceeded(new Action<Task>(<OnResult>c__AnonStorey.<>m__0), target);
		}

		// Token: 0x060014CC RID: 5324 RVA: 0x0007707D File Offset: 0x0007547D
		public static Task<T> OnResult<T>(this Task<T> task, Action<T> action)
		{
			return task.OnResult(action, null);
		}

		// Token: 0x060014CD RID: 5325 RVA: 0x00077088 File Offset: 0x00075488
		public static Task<T> OnResult<T>(this Task<T> task, Action<T> action, DispatcherBase actionTarget)
		{
			TaskExtension.<OnResult>c__AnonStorey5<T> <OnResult>c__AnonStorey = new TaskExtension.<OnResult>c__AnonStorey5<T>();
			<OnResult>c__AnonStorey.action = action;
			return task.WhenSucceeded(new Action<Task<T>>(<OnResult>c__AnonStorey.<>m__0), actionTarget);
		}

		// Token: 0x060014CE RID: 5326 RVA: 0x000770B8 File Offset: 0x000754B8
		public static Task<T> WhenSucceeded<T>(this Task<T> task, Action action)
		{
			TaskExtension.<WhenSucceeded>c__AnonStorey6<T> <WhenSucceeded>c__AnonStorey = new TaskExtension.<WhenSucceeded>c__AnonStorey6<T>();
			<WhenSucceeded>c__AnonStorey.action = action;
			return task.WhenSucceeded(new Action<Task<T>>(<WhenSucceeded>c__AnonStorey.<>m__0), null);
		}

		// Token: 0x060014CF RID: 5327 RVA: 0x000770E5 File Offset: 0x000754E5
		public static Task<T> WhenSucceeded<T>(this Task<T> task, Action<Task<T>> action)
		{
			return task.WhenSucceeded(action, null);
		}

		// Token: 0x060014D0 RID: 5328 RVA: 0x000770F0 File Offset: 0x000754F0
		public static Task<T> WhenSucceeded<T>(this Task<T> task, Action<Task<T>> action, DispatcherBase target)
		{
			TaskExtension.<WhenSucceeded>c__AnonStorey7<T> <WhenSucceeded>c__AnonStorey = new TaskExtension.<WhenSucceeded>c__AnonStorey7<T>();
			<WhenSucceeded>c__AnonStorey.target = target;
			<WhenSucceeded>c__AnonStorey.action = action;
			<WhenSucceeded>c__AnonStorey.perform = new Action<Task<T>>(<WhenSucceeded>c__AnonStorey.<>m__0);
			return task.WhenEnded(new Action<Task<T>>(<WhenSucceeded>c__AnonStorey.<>m__1), null);
		}

		// Token: 0x060014D1 RID: 5329 RVA: 0x00077138 File Offset: 0x00075538
		public static Task WhenSucceeded(this Task task, Action action)
		{
			TaskExtension.<WhenSucceeded>c__AnonStorey9 <WhenSucceeded>c__AnonStorey = new TaskExtension.<WhenSucceeded>c__AnonStorey9();
			<WhenSucceeded>c__AnonStorey.action = action;
			return task.WhenEnded(new Action<Task>(<WhenSucceeded>c__AnonStorey.<>m__0));
		}

		// Token: 0x060014D2 RID: 5330 RVA: 0x00077164 File Offset: 0x00075564
		public static Task WhenSucceeded(this Task task, Action<Task> action)
		{
			TaskExtension.<WhenSucceeded>c__AnonStoreyA <WhenSucceeded>c__AnonStoreyA = new TaskExtension.<WhenSucceeded>c__AnonStoreyA();
			<WhenSucceeded>c__AnonStoreyA.action = action;
			return task.WhenEnded(new Action<Task>(<WhenSucceeded>c__AnonStoreyA.<>m__0));
		}

		// Token: 0x060014D3 RID: 5331 RVA: 0x00077190 File Offset: 0x00075590
		public static Task WhenSucceeded(this Task task, Action<Task> action, DispatcherBase actiontargetTarget)
		{
			TaskExtension.<WhenSucceeded>c__AnonStoreyB <WhenSucceeded>c__AnonStoreyB = new TaskExtension.<WhenSucceeded>c__AnonStoreyB();
			<WhenSucceeded>c__AnonStoreyB.actiontargetTarget = actiontargetTarget;
			<WhenSucceeded>c__AnonStoreyB.action = action;
			<WhenSucceeded>c__AnonStoreyB.perform = new Action<Task>(<WhenSucceeded>c__AnonStoreyB.<>m__0);
			return task.WhenEnded(new Action<Task>(<WhenSucceeded>c__AnonStoreyB.<>m__1), null);
		}

		// Token: 0x060014D4 RID: 5332 RVA: 0x000771D8 File Offset: 0x000755D8
		public static Task<T> WhenFailed<T>(this Task<T> task, Action action)
		{
			TaskExtension.<WhenFailed>c__AnonStoreyD<T> <WhenFailed>c__AnonStoreyD = new TaskExtension.<WhenFailed>c__AnonStoreyD<T>();
			<WhenFailed>c__AnonStoreyD.action = action;
			return task.WhenFailed(new Action<Task<T>>(<WhenFailed>c__AnonStoreyD.<>m__0), null);
		}

		// Token: 0x060014D5 RID: 5333 RVA: 0x00077205 File Offset: 0x00075605
		public static Task<T> WhenFailed<T>(this Task<T> task, Action<Task<T>> action)
		{
			return task.WhenFailed(action, null);
		}

		// Token: 0x060014D6 RID: 5334 RVA: 0x00077210 File Offset: 0x00075610
		public static Task<T> WhenFailed<T>(this Task<T> task, Action<Task<T>> action, DispatcherBase target)
		{
			TaskExtension.<WhenFailed>c__AnonStoreyE<T> <WhenFailed>c__AnonStoreyE = new TaskExtension.<WhenFailed>c__AnonStoreyE<T>();
			<WhenFailed>c__AnonStoreyE.action = action;
			return task.WhenEnded(new Action<Task<T>>(<WhenFailed>c__AnonStoreyE.<>m__0), target);
		}

		// Token: 0x060014D7 RID: 5335 RVA: 0x00077240 File Offset: 0x00075640
		public static Task WhenFailed(this Task task, Action action)
		{
			TaskExtension.<WhenFailed>c__AnonStoreyF <WhenFailed>c__AnonStoreyF = new TaskExtension.<WhenFailed>c__AnonStoreyF();
			<WhenFailed>c__AnonStoreyF.action = action;
			return task.WhenEnded(new Action<Task>(<WhenFailed>c__AnonStoreyF.<>m__0));
		}

		// Token: 0x060014D8 RID: 5336 RVA: 0x0007726C File Offset: 0x0007566C
		public static Task WhenFailed(this Task task, Action<Task> action)
		{
			TaskExtension.<WhenFailed>c__AnonStorey10 <WhenFailed>c__AnonStorey = new TaskExtension.<WhenFailed>c__AnonStorey10();
			<WhenFailed>c__AnonStorey.action = action;
			return task.WhenEnded(new Action<Task>(<WhenFailed>c__AnonStorey.<>m__0));
		}

		// Token: 0x060014D9 RID: 5337 RVA: 0x00077298 File Offset: 0x00075698
		public static Task WhenFailed(this Task task, Action<Task> action, DispatcherBase target)
		{
			TaskExtension.<WhenFailed>c__AnonStorey11 <WhenFailed>c__AnonStorey = new TaskExtension.<WhenFailed>c__AnonStorey11();
			<WhenFailed>c__AnonStorey.action = action;
			return task.WhenEnded(new Action<Task>(<WhenFailed>c__AnonStorey.<>m__0), target);
		}

		// Token: 0x060014DA RID: 5338 RVA: 0x000772C8 File Offset: 0x000756C8
		public static Task<T> WhenEnded<T>(this Task<T> task, Action action)
		{
			TaskExtension.<WhenEnded>c__AnonStorey12<T> <WhenEnded>c__AnonStorey = new TaskExtension.<WhenEnded>c__AnonStorey12<T>();
			<WhenEnded>c__AnonStorey.action = action;
			return task.WhenEnded(new Action<Task<T>>(<WhenEnded>c__AnonStorey.<>m__0), null);
		}

		// Token: 0x060014DB RID: 5339 RVA: 0x000772F5 File Offset: 0x000756F5
		public static Task<T> WhenEnded<T>(this Task<T> task, Action<Task<T>> action)
		{
			return task.WhenEnded(action, null);
		}

		// Token: 0x060014DC RID: 5340 RVA: 0x00077300 File Offset: 0x00075700
		public static Task<T> WhenEnded<T>(this Task<T> task, Action<Task<T>> action, DispatcherBase target)
		{
			TaskExtension.<WhenEnded>c__AnonStorey13<T> <WhenEnded>c__AnonStorey = new TaskExtension.<WhenEnded>c__AnonStorey13<T>();
			<WhenEnded>c__AnonStorey.target = target;
			<WhenEnded>c__AnonStorey.action = action;
			<WhenEnded>c__AnonStorey.task = task;
			<WhenEnded>c__AnonStorey.task.TaskEnded += <WhenEnded>c__AnonStorey.<>m__0;
			return <WhenEnded>c__AnonStorey.task;
		}

		// Token: 0x060014DD RID: 5341 RVA: 0x00077348 File Offset: 0x00075748
		public static Task WhenEnded(this Task task, Action action)
		{
			TaskExtension.<WhenEnded>c__AnonStorey14 <WhenEnded>c__AnonStorey = new TaskExtension.<WhenEnded>c__AnonStorey14();
			<WhenEnded>c__AnonStorey.action = action;
			return task.WhenEnded(new Action<Task>(<WhenEnded>c__AnonStorey.<>m__0));
		}

		// Token: 0x060014DE RID: 5342 RVA: 0x00077374 File Offset: 0x00075774
		public static Task WhenEnded(this Task task, Action<Task> action)
		{
			TaskExtension.<WhenEnded>c__AnonStorey15 <WhenEnded>c__AnonStorey = new TaskExtension.<WhenEnded>c__AnonStorey15();
			<WhenEnded>c__AnonStorey.action = action;
			return task.WhenEnded(new Action<Task>(<WhenEnded>c__AnonStorey.<>m__0), null);
		}

		// Token: 0x060014DF RID: 5343 RVA: 0x000773A4 File Offset: 0x000757A4
		public static Task WhenEnded(this Task task, Action<Task> action, DispatcherBase target)
		{
			TaskExtension.<WhenEnded>c__AnonStorey16 <WhenEnded>c__AnonStorey = new TaskExtension.<WhenEnded>c__AnonStorey16();
			<WhenEnded>c__AnonStorey.target = target;
			<WhenEnded>c__AnonStorey.action = action;
			<WhenEnded>c__AnonStorey.task = task;
			<WhenEnded>c__AnonStorey.task.TaskEnded += <WhenEnded>c__AnonStorey.<>m__0;
			return <WhenEnded>c__AnonStorey.task;
		}

		// Token: 0x060014E0 RID: 5344 RVA: 0x000773EC File Offset: 0x000757EC
		public static Task Then(this Task that, Task followingTask)
		{
			TaskDistributor target = null;
			if (ThreadBase.CurrentThread is TaskWorker)
			{
				target = ((TaskWorker)ThreadBase.CurrentThread).TaskDistributor;
			}
			return that.Then(followingTask, target);
		}

		// Token: 0x060014E1 RID: 5345 RVA: 0x00077424 File Offset: 0x00075824
		public static Task Then(this Task that, Task followingTask, DispatcherBase target)
		{
			TaskExtension.<Then>c__AnonStorey17 <Then>c__AnonStorey = new TaskExtension.<Then>c__AnonStorey17();
			<Then>c__AnonStorey.followingTask = followingTask;
			<Then>c__AnonStorey.target = target;
			that.WhenFailed(new Action(<Then>c__AnonStorey.<>m__0));
			that.WhenSucceeded(new Action(<Then>c__AnonStorey.<>m__1));
			return that;
		}

		// Token: 0x060014E2 RID: 5346 RVA: 0x0007746C File Offset: 0x0007586C
		public static Task Await(this Task that, Task taskToWaitFor)
		{
			taskToWaitFor.Then(that);
			return that;
		}

		// Token: 0x060014E3 RID: 5347 RVA: 0x00077477 File Offset: 0x00075877
		public static Task Await(this Task that, Task taskToWaitFor, DispatcherBase target)
		{
			taskToWaitFor.Then(that, target);
			return that;
		}

		// Token: 0x060014E4 RID: 5348 RVA: 0x00077483 File Offset: 0x00075883
		public static Task<T> As<T>(this Task that)
		{
			return (Task<T>)that;
		}

		// Token: 0x060014E5 RID: 5349 RVA: 0x0007748C File Offset: 0x0007588C
		public static IEnumerable<Task> ContinueWhenAnyEnded(this IEnumerable<Task> tasks, Action action)
		{
			TaskExtension.<ContinueWhenAnyEnded>c__AnonStorey18 <ContinueWhenAnyEnded>c__AnonStorey = new TaskExtension.<ContinueWhenAnyEnded>c__AnonStorey18();
			<ContinueWhenAnyEnded>c__AnonStorey.action = action;
			return tasks.ContinueWhenAnyEnded(new Action<Task>(<ContinueWhenAnyEnded>c__AnonStorey.<>m__0));
		}

		// Token: 0x060014E6 RID: 5350 RVA: 0x000774B8 File Offset: 0x000758B8
		public static IEnumerable<Task> ContinueWhenAnyEnded(this IEnumerable<Task> tasks, Action<Task> action)
		{
			TaskExtension.<ContinueWhenAnyEnded>c__AnonStorey19 <ContinueWhenAnyEnded>c__AnonStorey = new TaskExtension.<ContinueWhenAnyEnded>c__AnonStorey19();
			<ContinueWhenAnyEnded>c__AnonStorey.action = action;
			<ContinueWhenAnyEnded>c__AnonStorey.syncRoot = new object();
			<ContinueWhenAnyEnded>c__AnonStorey.done = false;
			foreach (Task task in tasks)
			{
				task.WhenEnded(new Action<Task>(<ContinueWhenAnyEnded>c__AnonStorey.<>m__0));
			}
			return tasks;
		}

		// Token: 0x060014E7 RID: 5351 RVA: 0x00077538 File Offset: 0x00075938
		public static IEnumerable<Task> ContinueWhenAllEnded(this IEnumerable<Task> tasks, Action action)
		{
			TaskExtension.<ContinueWhenAllEnded>c__AnonStorey1A <ContinueWhenAllEnded>c__AnonStorey1A = new TaskExtension.<ContinueWhenAllEnded>c__AnonStorey1A();
			<ContinueWhenAllEnded>c__AnonStorey1A.action = action;
			return tasks.ContinueWhenAllEnded(new Action<IEnumerable<Task>>(<ContinueWhenAllEnded>c__AnonStorey1A.<>m__0));
		}

		// Token: 0x060014E8 RID: 5352 RVA: 0x00077564 File Offset: 0x00075964
		public static IEnumerable<Task> ContinueWhenAllEnded(this IEnumerable<Task> tasks, Action<IEnumerable<Task>> action)
		{
			TaskExtension.<ContinueWhenAllEnded>c__AnonStorey1B <ContinueWhenAllEnded>c__AnonStorey1B = new TaskExtension.<ContinueWhenAllEnded>c__AnonStorey1B();
			<ContinueWhenAllEnded>c__AnonStorey1B.action = action;
			<ContinueWhenAllEnded>c__AnonStorey1B.count = tasks.Count<Task>();
			if (<ContinueWhenAllEnded>c__AnonStorey1B.count == 0)
			{
				<ContinueWhenAllEnded>c__AnonStorey1B.action(new Task[0]);
			}
			<ContinueWhenAllEnded>c__AnonStorey1B.finishedTasks = new List<Task>();
			<ContinueWhenAllEnded>c__AnonStorey1B.syncRoot = new object();
			using (IEnumerator<Task> enumerator = tasks.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TaskExtension.<ContinueWhenAllEnded>c__AnonStorey1C <ContinueWhenAllEnded>c__AnonStorey1C = new TaskExtension.<ContinueWhenAllEnded>c__AnonStorey1C();
					<ContinueWhenAllEnded>c__AnonStorey1C.<>f__ref$27 = <ContinueWhenAllEnded>c__AnonStorey1B;
					<ContinueWhenAllEnded>c__AnonStorey1C.task = enumerator.Current;
					<ContinueWhenAllEnded>c__AnonStorey1C.task.WhenEnded(new Action<Task>(<ContinueWhenAllEnded>c__AnonStorey1C.<>m__0));
				}
			}
			return tasks;
		}

		// Token: 0x02000F0C RID: 3852
		[CompilerGenerated]
		private sealed class <Then>c__AnonStorey0
		{
			// Token: 0x060072AB RID: 29355 RVA: 0x00077628 File Offset: 0x00075A28
			public <Then>c__AnonStorey0()
			{
			}

			// Token: 0x060072AC RID: 29356 RVA: 0x00077630 File Offset: 0x00075A30
			internal void <>m__0()
			{
				if (this.followingTask.ShouldAbort)
				{
					return;
				}
				this.followingTask.Abort();
			}

			// Token: 0x060072AD RID: 29357 RVA: 0x00077650 File Offset: 0x00075A50
			internal void <>m__1()
			{
				if (this.followingTask.ShouldAbort)
				{
					return;
				}
				object obj = this.syncRoot;
				lock (obj)
				{
					this.remaining--;
					if (this.remaining == 0)
					{
						if (this.target != null)
						{
							this.followingTask.Run(this.target);
						}
						else if (ThreadBase.CurrentThread is TaskWorker)
						{
							this.followingTask.Run(((TaskWorker)ThreadBase.CurrentThread).TaskDistributor);
						}
						else
						{
							this.followingTask.Run();
						}
					}
				}
			}

			// Token: 0x04006690 RID: 26256
			internal Task followingTask;

			// Token: 0x04006691 RID: 26257
			internal object syncRoot;

			// Token: 0x04006692 RID: 26258
			internal int remaining;

			// Token: 0x04006693 RID: 26259
			internal DispatcherBase target;
		}

		// Token: 0x02000F0D RID: 3853
		[CompilerGenerated]
		private sealed class <WhenSucceeded>c__AnonStorey1
		{
			// Token: 0x060072AE RID: 29358 RVA: 0x00077710 File Offset: 0x00075B10
			public <WhenSucceeded>c__AnonStorey1()
			{
			}

			// Token: 0x060072AF RID: 29359 RVA: 0x00077718 File Offset: 0x00075B18
			internal void <>m__0()
			{
				object obj = this.syncRoot;
				lock (obj)
				{
					this.remaining--;
					if (this.remaining == 0)
					{
						if (this.target == null)
						{
							this.action();
						}
						else
						{
							this.target.Dispatch(new Action(this.<>m__1));
						}
					}
				}
			}

			// Token: 0x060072B0 RID: 29360 RVA: 0x0007779C File Offset: 0x00075B9C
			internal void <>m__1()
			{
				this.action();
			}

			// Token: 0x04006694 RID: 26260
			internal object syncRoot;

			// Token: 0x04006695 RID: 26261
			internal int remaining;

			// Token: 0x04006696 RID: 26262
			internal DispatcherBase target;

			// Token: 0x04006697 RID: 26263
			internal Action action;
		}

		// Token: 0x02000F0E RID: 3854
		[CompilerGenerated]
		private sealed class <WhenFailed>c__AnonStorey2
		{
			// Token: 0x060072B1 RID: 29361 RVA: 0x000777A9 File Offset: 0x00075BA9
			public <WhenFailed>c__AnonStorey2()
			{
			}

			// Token: 0x060072B2 RID: 29362 RVA: 0x000777B4 File Offset: 0x00075BB4
			internal void <>m__0()
			{
				object obj = this.syncRoot;
				lock (obj)
				{
					if (!this.hasFailed)
					{
						this.hasFailed = true;
						if (this.target == null)
						{
							this.action();
						}
						else
						{
							this.target.Dispatch(new Action(this.<>m__1));
						}
					}
				}
			}

			// Token: 0x060072B3 RID: 29363 RVA: 0x00077834 File Offset: 0x00075C34
			internal void <>m__1()
			{
				this.action();
			}

			// Token: 0x04006698 RID: 26264
			internal object syncRoot;

			// Token: 0x04006699 RID: 26265
			internal bool hasFailed;

			// Token: 0x0400669A RID: 26266
			internal DispatcherBase target;

			// Token: 0x0400669B RID: 26267
			internal Action action;
		}

		// Token: 0x02000F0F RID: 3855
		[CompilerGenerated]
		private sealed class <OnResult>c__AnonStorey3
		{
			// Token: 0x060072B4 RID: 29364 RVA: 0x00077841 File Offset: 0x00075C41
			public <OnResult>c__AnonStorey3()
			{
			}

			// Token: 0x060072B5 RID: 29365 RVA: 0x00077849 File Offset: 0x00075C49
			internal void <>m__0(Task t)
			{
				this.action(t.RawResult);
			}

			// Token: 0x0400669C RID: 26268
			internal Action<object> action;
		}

		// Token: 0x02000F10 RID: 3856
		[CompilerGenerated]
		private sealed class <OnResult>c__AnonStorey4<T>
		{
			// Token: 0x060072B6 RID: 29366 RVA: 0x0007785C File Offset: 0x00075C5C
			public <OnResult>c__AnonStorey4()
			{
			}

			// Token: 0x060072B7 RID: 29367 RVA: 0x00077864 File Offset: 0x00075C64
			internal void <>m__0(Task t)
			{
				this.action((T)((object)t.RawResult));
			}

			// Token: 0x0400669D RID: 26269
			internal Action<T> action;
		}

		// Token: 0x02000F11 RID: 3857
		[CompilerGenerated]
		private sealed class <OnResult>c__AnonStorey5<T>
		{
			// Token: 0x060072B8 RID: 29368 RVA: 0x0007787C File Offset: 0x00075C7C
			public <OnResult>c__AnonStorey5()
			{
			}

			// Token: 0x060072B9 RID: 29369 RVA: 0x00077884 File Offset: 0x00075C84
			internal void <>m__0(Task<T> t)
			{
				this.action(t.Result);
			}

			// Token: 0x0400669E RID: 26270
			internal Action<T> action;
		}

		// Token: 0x02000F12 RID: 3858
		[CompilerGenerated]
		private sealed class <WhenSucceeded>c__AnonStorey6<T>
		{
			// Token: 0x060072BA RID: 29370 RVA: 0x00077897 File Offset: 0x00075C97
			public <WhenSucceeded>c__AnonStorey6()
			{
			}

			// Token: 0x060072BB RID: 29371 RVA: 0x0007789F File Offset: 0x00075C9F
			internal void <>m__0(Task<T> t)
			{
				this.action();
			}

			// Token: 0x0400669F RID: 26271
			internal Action action;
		}

		// Token: 0x02000F13 RID: 3859
		[CompilerGenerated]
		private sealed class <WhenSucceeded>c__AnonStorey7<T>
		{
			// Token: 0x060072BC RID: 29372 RVA: 0x000778AC File Offset: 0x00075CAC
			public <WhenSucceeded>c__AnonStorey7()
			{
			}

			// Token: 0x060072BD RID: 29373 RVA: 0x000778B4 File Offset: 0x00075CB4
			internal void <>m__0(Task<T> t)
			{
				TaskExtension.<WhenSucceeded>c__AnonStorey7<T>.<WhenSucceeded>c__AnonStorey8 <WhenSucceeded>c__AnonStorey = new TaskExtension.<WhenSucceeded>c__AnonStorey7<T>.<WhenSucceeded>c__AnonStorey8();
				<WhenSucceeded>c__AnonStorey.<>f__ref$7 = this;
				<WhenSucceeded>c__AnonStorey.t = t;
				if (this.target == null)
				{
					this.action(<WhenSucceeded>c__AnonStorey.t);
				}
				else
				{
					this.target.Dispatch(new Action(<WhenSucceeded>c__AnonStorey.<>m__0));
				}
			}

			// Token: 0x060072BE RID: 29374 RVA: 0x0007790E File Offset: 0x00075D0E
			internal void <>m__1(Task<T> t)
			{
				if (t.IsSucceeded)
				{
					this.perform(t);
				}
			}

			// Token: 0x040066A0 RID: 26272
			internal DispatcherBase target;

			// Token: 0x040066A1 RID: 26273
			internal Action<Task<T>> action;

			// Token: 0x040066A2 RID: 26274
			internal Action<Task<T>> perform;

			// Token: 0x02000F27 RID: 3879
			private sealed class <WhenSucceeded>c__AnonStorey8
			{
				// Token: 0x060072E8 RID: 29416 RVA: 0x00077927 File Offset: 0x00075D27
				public <WhenSucceeded>c__AnonStorey8()
				{
				}

				// Token: 0x060072E9 RID: 29417 RVA: 0x0007792F File Offset: 0x00075D2F
				internal void <>m__0()
				{
					if (this.t.IsSucceeded)
					{
						this.<>f__ref$7.action(this.t);
					}
				}

				// Token: 0x040066C3 RID: 26307
				internal Task<T> t;

				// Token: 0x040066C4 RID: 26308
				internal TaskExtension.<WhenSucceeded>c__AnonStorey7<T> <>f__ref$7;
			}
		}

		// Token: 0x02000F14 RID: 3860
		[CompilerGenerated]
		private sealed class <WhenSucceeded>c__AnonStorey9
		{
			// Token: 0x060072BF RID: 29375 RVA: 0x00077957 File Offset: 0x00075D57
			public <WhenSucceeded>c__AnonStorey9()
			{
			}

			// Token: 0x060072C0 RID: 29376 RVA: 0x0007795F File Offset: 0x00075D5F
			internal void <>m__0(Task t)
			{
				if (t.IsSucceeded)
				{
					this.action();
				}
			}

			// Token: 0x040066A3 RID: 26275
			internal Action action;
		}

		// Token: 0x02000F15 RID: 3861
		[CompilerGenerated]
		private sealed class <WhenSucceeded>c__AnonStoreyA
		{
			// Token: 0x060072C1 RID: 29377 RVA: 0x00077977 File Offset: 0x00075D77
			public <WhenSucceeded>c__AnonStoreyA()
			{
			}

			// Token: 0x060072C2 RID: 29378 RVA: 0x0007797F File Offset: 0x00075D7F
			internal void <>m__0(Task t)
			{
				if (t.IsSucceeded)
				{
					this.action(t);
				}
			}

			// Token: 0x040066A4 RID: 26276
			internal Action<Task> action;
		}

		// Token: 0x02000F16 RID: 3862
		[CompilerGenerated]
		private sealed class <WhenSucceeded>c__AnonStoreyB
		{
			// Token: 0x060072C3 RID: 29379 RVA: 0x00077998 File Offset: 0x00075D98
			public <WhenSucceeded>c__AnonStoreyB()
			{
			}

			// Token: 0x060072C4 RID: 29380 RVA: 0x000779A0 File Offset: 0x00075DA0
			internal void <>m__0(Task t)
			{
				TaskExtension.<WhenSucceeded>c__AnonStoreyB.<WhenSucceeded>c__AnonStoreyC <WhenSucceeded>c__AnonStoreyC = new TaskExtension.<WhenSucceeded>c__AnonStoreyB.<WhenSucceeded>c__AnonStoreyC();
				<WhenSucceeded>c__AnonStoreyC.<>f__ref$11 = this;
				<WhenSucceeded>c__AnonStoreyC.t = t;
				if (this.actiontargetTarget == null)
				{
					this.action(<WhenSucceeded>c__AnonStoreyC.t);
				}
				else
				{
					this.actiontargetTarget.Dispatch(new Action(<WhenSucceeded>c__AnonStoreyC.<>m__0));
				}
			}

			// Token: 0x060072C5 RID: 29381 RVA: 0x000779FA File Offset: 0x00075DFA
			internal void <>m__1(Task t)
			{
				if (t.IsSucceeded)
				{
					this.perform(t);
				}
			}

			// Token: 0x040066A5 RID: 26277
			internal DispatcherBase actiontargetTarget;

			// Token: 0x040066A6 RID: 26278
			internal Action<Task> action;

			// Token: 0x040066A7 RID: 26279
			internal Action<Task> perform;

			// Token: 0x02000F28 RID: 3880
			private sealed class <WhenSucceeded>c__AnonStoreyC
			{
				// Token: 0x060072EA RID: 29418 RVA: 0x00077A13 File Offset: 0x00075E13
				public <WhenSucceeded>c__AnonStoreyC()
				{
				}

				// Token: 0x060072EB RID: 29419 RVA: 0x00077A1B File Offset: 0x00075E1B
				internal void <>m__0()
				{
					if (this.t.IsSucceeded)
					{
						this.<>f__ref$11.action(this.t);
					}
				}

				// Token: 0x040066C5 RID: 26309
				internal Task t;

				// Token: 0x040066C6 RID: 26310
				internal TaskExtension.<WhenSucceeded>c__AnonStoreyB <>f__ref$11;
			}
		}

		// Token: 0x02000F17 RID: 3863
		[CompilerGenerated]
		private sealed class <WhenFailed>c__AnonStoreyD<T>
		{
			// Token: 0x060072C6 RID: 29382 RVA: 0x00077A43 File Offset: 0x00075E43
			public <WhenFailed>c__AnonStoreyD()
			{
			}

			// Token: 0x060072C7 RID: 29383 RVA: 0x00077A4B File Offset: 0x00075E4B
			internal void <>m__0(Task<T> t)
			{
				this.action();
			}

			// Token: 0x040066A8 RID: 26280
			internal Action action;
		}

		// Token: 0x02000F18 RID: 3864
		[CompilerGenerated]
		private sealed class <WhenFailed>c__AnonStoreyE<T>
		{
			// Token: 0x060072C8 RID: 29384 RVA: 0x00077A58 File Offset: 0x00075E58
			public <WhenFailed>c__AnonStoreyE()
			{
			}

			// Token: 0x060072C9 RID: 29385 RVA: 0x00077A60 File Offset: 0x00075E60
			internal void <>m__0(Task<T> t)
			{
				if (t.IsFailed)
				{
					this.action(t);
				}
			}

			// Token: 0x040066A9 RID: 26281
			internal Action<Task<T>> action;
		}

		// Token: 0x02000F19 RID: 3865
		[CompilerGenerated]
		private sealed class <WhenFailed>c__AnonStoreyF
		{
			// Token: 0x060072CA RID: 29386 RVA: 0x00077A79 File Offset: 0x00075E79
			public <WhenFailed>c__AnonStoreyF()
			{
			}

			// Token: 0x060072CB RID: 29387 RVA: 0x00077A81 File Offset: 0x00075E81
			internal void <>m__0(Task t)
			{
				if (t.IsFailed)
				{
					this.action();
				}
			}

			// Token: 0x040066AA RID: 26282
			internal Action action;
		}

		// Token: 0x02000F1A RID: 3866
		[CompilerGenerated]
		private sealed class <WhenFailed>c__AnonStorey10
		{
			// Token: 0x060072CC RID: 29388 RVA: 0x00077A99 File Offset: 0x00075E99
			public <WhenFailed>c__AnonStorey10()
			{
			}

			// Token: 0x060072CD RID: 29389 RVA: 0x00077AA1 File Offset: 0x00075EA1
			internal void <>m__0(Task t)
			{
				if (t.IsFailed)
				{
					this.action(t);
				}
			}

			// Token: 0x040066AB RID: 26283
			internal Action<Task> action;
		}

		// Token: 0x02000F1B RID: 3867
		[CompilerGenerated]
		private sealed class <WhenFailed>c__AnonStorey11
		{
			// Token: 0x060072CE RID: 29390 RVA: 0x00077ABA File Offset: 0x00075EBA
			public <WhenFailed>c__AnonStorey11()
			{
			}

			// Token: 0x060072CF RID: 29391 RVA: 0x00077AC2 File Offset: 0x00075EC2
			internal void <>m__0(Task t)
			{
				if (t.IsFailed)
				{
					this.action(t);
				}
			}

			// Token: 0x040066AC RID: 26284
			internal Action<Task> action;
		}

		// Token: 0x02000F1C RID: 3868
		[CompilerGenerated]
		private sealed class <WhenEnded>c__AnonStorey12<T>
		{
			// Token: 0x060072D0 RID: 29392 RVA: 0x00077ADB File Offset: 0x00075EDB
			public <WhenEnded>c__AnonStorey12()
			{
			}

			// Token: 0x060072D1 RID: 29393 RVA: 0x00077AE3 File Offset: 0x00075EE3
			internal void <>m__0(Task<T> t)
			{
				this.action();
			}

			// Token: 0x040066AD RID: 26285
			internal Action action;
		}

		// Token: 0x02000F1D RID: 3869
		[CompilerGenerated]
		private sealed class <WhenEnded>c__AnonStorey13<T>
		{
			// Token: 0x060072D2 RID: 29394 RVA: 0x00077AF0 File Offset: 0x00075EF0
			public <WhenEnded>c__AnonStorey13()
			{
			}

			// Token: 0x060072D3 RID: 29395 RVA: 0x00077AF8 File Offset: 0x00075EF8
			internal void <>m__0(Task t)
			{
				if (this.target == null)
				{
					this.action(this.task);
				}
				else
				{
					this.target.Dispatch(new Action(this.<>m__1));
				}
			}

			// Token: 0x060072D4 RID: 29396 RVA: 0x00077B33 File Offset: 0x00075F33
			internal void <>m__1()
			{
				this.action(this.task);
			}

			// Token: 0x040066AE RID: 26286
			internal DispatcherBase target;

			// Token: 0x040066AF RID: 26287
			internal Action<Task<T>> action;

			// Token: 0x040066B0 RID: 26288
			internal Task<T> task;
		}

		// Token: 0x02000F1E RID: 3870
		[CompilerGenerated]
		private sealed class <WhenEnded>c__AnonStorey14
		{
			// Token: 0x060072D5 RID: 29397 RVA: 0x00077B46 File Offset: 0x00075F46
			public <WhenEnded>c__AnonStorey14()
			{
			}

			// Token: 0x060072D6 RID: 29398 RVA: 0x00077B4E File Offset: 0x00075F4E
			internal void <>m__0(Task t)
			{
				this.action();
			}

			// Token: 0x040066B1 RID: 26289
			internal Action action;
		}

		// Token: 0x02000F1F RID: 3871
		[CompilerGenerated]
		private sealed class <WhenEnded>c__AnonStorey15
		{
			// Token: 0x060072D7 RID: 29399 RVA: 0x00077B5B File Offset: 0x00075F5B
			public <WhenEnded>c__AnonStorey15()
			{
			}

			// Token: 0x060072D8 RID: 29400 RVA: 0x00077B63 File Offset: 0x00075F63
			internal void <>m__0(Task t)
			{
				this.action(t);
			}

			// Token: 0x040066B2 RID: 26290
			internal Action<Task> action;
		}

		// Token: 0x02000F20 RID: 3872
		[CompilerGenerated]
		private sealed class <WhenEnded>c__AnonStorey16
		{
			// Token: 0x060072D9 RID: 29401 RVA: 0x00077B71 File Offset: 0x00075F71
			public <WhenEnded>c__AnonStorey16()
			{
			}

			// Token: 0x060072DA RID: 29402 RVA: 0x00077B79 File Offset: 0x00075F79
			internal void <>m__0(Task t)
			{
				if (this.target == null)
				{
					this.action(this.task);
				}
				else
				{
					this.target.Dispatch(new Action(this.<>m__1));
				}
			}

			// Token: 0x060072DB RID: 29403 RVA: 0x00077BB4 File Offset: 0x00075FB4
			internal void <>m__1()
			{
				this.action(this.task);
			}

			// Token: 0x040066B3 RID: 26291
			internal DispatcherBase target;

			// Token: 0x040066B4 RID: 26292
			internal Action<Task> action;

			// Token: 0x040066B5 RID: 26293
			internal Task task;
		}

		// Token: 0x02000F21 RID: 3873
		[CompilerGenerated]
		private sealed class <Then>c__AnonStorey17
		{
			// Token: 0x060072DC RID: 29404 RVA: 0x00077BC7 File Offset: 0x00075FC7
			public <Then>c__AnonStorey17()
			{
			}

			// Token: 0x060072DD RID: 29405 RVA: 0x00077BCF File Offset: 0x00075FCF
			internal void <>m__0()
			{
				this.followingTask.Abort();
			}

			// Token: 0x060072DE RID: 29406 RVA: 0x00077BDC File Offset: 0x00075FDC
			internal void <>m__1()
			{
				if (this.target != null)
				{
					this.followingTask.Run(this.target);
				}
				else if (ThreadBase.CurrentThread is TaskWorker)
				{
					this.followingTask.Run(((TaskWorker)ThreadBase.CurrentThread).TaskDistributor);
				}
				else
				{
					this.followingTask.Run();
				}
			}

			// Token: 0x040066B6 RID: 26294
			internal Task followingTask;

			// Token: 0x040066B7 RID: 26295
			internal DispatcherBase target;
		}

		// Token: 0x02000F22 RID: 3874
		[CompilerGenerated]
		private sealed class <ContinueWhenAnyEnded>c__AnonStorey18
		{
			// Token: 0x060072DF RID: 29407 RVA: 0x00077C46 File Offset: 0x00076046
			public <ContinueWhenAnyEnded>c__AnonStorey18()
			{
			}

			// Token: 0x060072E0 RID: 29408 RVA: 0x00077C4E File Offset: 0x0007604E
			internal void <>m__0(Task t)
			{
				this.action();
			}

			// Token: 0x040066B8 RID: 26296
			internal Action action;
		}

		// Token: 0x02000F23 RID: 3875
		[CompilerGenerated]
		private sealed class <ContinueWhenAnyEnded>c__AnonStorey19
		{
			// Token: 0x060072E1 RID: 29409 RVA: 0x00077C5B File Offset: 0x0007605B
			public <ContinueWhenAnyEnded>c__AnonStorey19()
			{
			}

			// Token: 0x060072E2 RID: 29410 RVA: 0x00077C64 File Offset: 0x00076064
			internal void <>m__0(Task t)
			{
				object obj = this.syncRoot;
				lock (obj)
				{
					if (!this.done)
					{
						this.done = true;
						this.action(t);
					}
				}
			}

			// Token: 0x040066B9 RID: 26297
			internal object syncRoot;

			// Token: 0x040066BA RID: 26298
			internal bool done;

			// Token: 0x040066BB RID: 26299
			internal Action<Task> action;
		}

		// Token: 0x02000F24 RID: 3876
		[CompilerGenerated]
		private sealed class <ContinueWhenAllEnded>c__AnonStorey1A
		{
			// Token: 0x060072E3 RID: 29411 RVA: 0x00077CC0 File Offset: 0x000760C0
			public <ContinueWhenAllEnded>c__AnonStorey1A()
			{
			}

			// Token: 0x060072E4 RID: 29412 RVA: 0x00077CC8 File Offset: 0x000760C8
			internal void <>m__0(IEnumerable<Task> t)
			{
				this.action();
			}

			// Token: 0x040066BC RID: 26300
			internal Action action;
		}

		// Token: 0x02000F25 RID: 3877
		[CompilerGenerated]
		private sealed class <ContinueWhenAllEnded>c__AnonStorey1B
		{
			// Token: 0x060072E5 RID: 29413 RVA: 0x00077CD5 File Offset: 0x000760D5
			public <ContinueWhenAllEnded>c__AnonStorey1B()
			{
			}

			// Token: 0x040066BD RID: 26301
			internal object syncRoot;

			// Token: 0x040066BE RID: 26302
			internal List<Task> finishedTasks;

			// Token: 0x040066BF RID: 26303
			internal int count;

			// Token: 0x040066C0 RID: 26304
			internal Action<IEnumerable<Task>> action;
		}

		// Token: 0x02000F26 RID: 3878
		[CompilerGenerated]
		private sealed class <ContinueWhenAllEnded>c__AnonStorey1C
		{
			// Token: 0x060072E6 RID: 29414 RVA: 0x00077CDD File Offset: 0x000760DD
			public <ContinueWhenAllEnded>c__AnonStorey1C()
			{
			}

			// Token: 0x060072E7 RID: 29415 RVA: 0x00077CE8 File Offset: 0x000760E8
			internal void <>m__0(Task t)
			{
				object syncRoot = this.<>f__ref$27.syncRoot;
				lock (syncRoot)
				{
					this.<>f__ref$27.finishedTasks.Add(this.task);
					if (this.<>f__ref$27.finishedTasks.Count == this.<>f__ref$27.count)
					{
						this.<>f__ref$27.action(this.<>f__ref$27.finishedTasks);
					}
				}
			}

			// Token: 0x040066C1 RID: 26305
			internal Task task;

			// Token: 0x040066C2 RID: 26306
			internal TaskExtension.<ContinueWhenAllEnded>c__AnonStorey1B <>f__ref$27;
		}
	}
}
