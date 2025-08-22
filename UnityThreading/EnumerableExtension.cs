using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace UnityThreading
{
	// Token: 0x02000347 RID: 839
	public static class EnumerableExtension
	{
		// Token: 0x06001464 RID: 5220 RVA: 0x000757E4 File Offset: 0x00073BE4
		public static IEnumerable<Task> ParallelForEach<T>(this IEnumerable<T> that, Action<T> action)
		{
			return that.ParallelForEach(action, null);
		}

		// Token: 0x06001465 RID: 5221 RVA: 0x000757F0 File Offset: 0x00073BF0
		public static IEnumerable<Task> ParallelForEach<T>(this IEnumerable<T> that, Action<T> action, TaskDistributor target)
		{
			EnumerableExtension.<ParallelForEach>c__AnonStorey0<T> <ParallelForEach>c__AnonStorey = new EnumerableExtension.<ParallelForEach>c__AnonStorey0<T>();
			<ParallelForEach>c__AnonStorey.action = action;
			return (IEnumerable<Task>)that.ParallelForEach(new Func<T, Task.Unit>(<ParallelForEach>c__AnonStorey.<>m__0), target);
		}

		// Token: 0x06001466 RID: 5222 RVA: 0x00075822 File Offset: 0x00073C22
		public static IEnumerable<Task<TResult>> ParallelForEach<TResult, T>(this IEnumerable<T> that, Func<T, TResult> action)
		{
			return that.ParallelForEach(action);
		}

		// Token: 0x06001467 RID: 5223 RVA: 0x0007582C File Offset: 0x00073C2C
		public static IEnumerable<Task<TResult>> ParallelForEach<TResult, T>(this IEnumerable<T> that, Func<T, TResult> action, TaskDistributor target)
		{
			EnumerableExtension.<ParallelForEach>c__AnonStorey1<TResult, T> <ParallelForEach>c__AnonStorey = new EnumerableExtension.<ParallelForEach>c__AnonStorey1<TResult, T>();
			<ParallelForEach>c__AnonStorey.action = action;
			List<Task<TResult>> list = new List<Task<TResult>>();
			foreach (T tmp in that)
			{
				EnumerableExtension.<ParallelForEach>c__AnonStorey2<TResult, T> <ParallelForEach>c__AnonStorey2 = new EnumerableExtension.<ParallelForEach>c__AnonStorey2<TResult, T>();
				<ParallelForEach>c__AnonStorey2.<>f__ref$1 = <ParallelForEach>c__AnonStorey;
				<ParallelForEach>c__AnonStorey2.tmp = tmp;
				Task<TResult> item = Task.Create<TResult>(new Func<TResult>(<ParallelForEach>c__AnonStorey2.<>m__0)).Run(target);
				list.Add(item);
			}
			return list;
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x000758C8 File Offset: 0x00073CC8
		public static IEnumerable<Task> SequentialForEach<T>(this IEnumerable<T> that, Action<T> action)
		{
			return that.SequentialForEach(action, null);
		}

		// Token: 0x06001469 RID: 5225 RVA: 0x000758D4 File Offset: 0x00073CD4
		public static IEnumerable<Task> SequentialForEach<T>(this IEnumerable<T> that, Action<T> action, TaskDistributor target)
		{
			EnumerableExtension.<SequentialForEach>c__AnonStorey3<T> <SequentialForEach>c__AnonStorey = new EnumerableExtension.<SequentialForEach>c__AnonStorey3<T>();
			<SequentialForEach>c__AnonStorey.action = action;
			return (IEnumerable<Task>)that.SequentialForEach(new Func<T, Task.Unit>(<SequentialForEach>c__AnonStorey.<>m__0), target);
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x00075906 File Offset: 0x00073D06
		public static IEnumerable<Task<TResult>> SequentialForEach<TResult, T>(this IEnumerable<T> that, Func<T, TResult> action)
		{
			return that.SequentialForEach(action);
		}

		// Token: 0x0600146B RID: 5227 RVA: 0x00075910 File Offset: 0x00073D10
		public static IEnumerable<Task<TResult>> SequentialForEach<TResult, T>(this IEnumerable<T> that, Func<T, TResult> action, TaskDistributor target)
		{
			EnumerableExtension.<SequentialForEach>c__AnonStorey4<TResult, T> <SequentialForEach>c__AnonStorey = new EnumerableExtension.<SequentialForEach>c__AnonStorey4<TResult, T>();
			<SequentialForEach>c__AnonStorey.action = action;
			<SequentialForEach>c__AnonStorey.target = target;
			List<Task<TResult>> list = new List<Task<TResult>>();
			Task task = null;
			foreach (T tmp in that)
			{
				EnumerableExtension.<SequentialForEach>c__AnonStorey5<TResult, T> <SequentialForEach>c__AnonStorey2 = new EnumerableExtension.<SequentialForEach>c__AnonStorey5<TResult, T>();
				<SequentialForEach>c__AnonStorey2.<>f__ref$4 = <SequentialForEach>c__AnonStorey;
				<SequentialForEach>c__AnonStorey2.tmp = tmp;
				<SequentialForEach>c__AnonStorey2.task = Task.Create<TResult>(new Func<TResult>(<SequentialForEach>c__AnonStorey2.<>m__0));
				if (task == null)
				{
					<SequentialForEach>c__AnonStorey2.task.Run(<SequentialForEach>c__AnonStorey.target);
				}
				else
				{
					task.WhenEnded(new Action(<SequentialForEach>c__AnonStorey2.<>m__1));
				}
				task = <SequentialForEach>c__AnonStorey2.task;
				list.Add(<SequentialForEach>c__AnonStorey2.task);
			}
			return list;
		}

		// Token: 0x02000EFF RID: 3839
		[CompilerGenerated]
		private sealed class <ParallelForEach>c__AnonStorey0<T>
		{
			// Token: 0x06007292 RID: 29330 RVA: 0x000759F8 File Offset: 0x00073DF8
			public <ParallelForEach>c__AnonStorey0()
			{
			}

			// Token: 0x06007293 RID: 29331 RVA: 0x00075A00 File Offset: 0x00073E00
			internal Task.Unit <>m__0(T element)
			{
				this.action(element);
				return default(Task.Unit);
			}

			// Token: 0x0400667B RID: 26235
			internal Action<T> action;
		}

		// Token: 0x02000F00 RID: 3840
		[CompilerGenerated]
		private sealed class <ParallelForEach>c__AnonStorey1<TResult, T>
		{
			// Token: 0x06007294 RID: 29332 RVA: 0x00075A22 File Offset: 0x00073E22
			public <ParallelForEach>c__AnonStorey1()
			{
			}

			// Token: 0x0400667C RID: 26236
			internal Func<T, TResult> action;
		}

		// Token: 0x02000F01 RID: 3841
		[CompilerGenerated]
		private sealed class <ParallelForEach>c__AnonStorey2<TResult, T>
		{
			// Token: 0x06007295 RID: 29333 RVA: 0x00075A2A File Offset: 0x00073E2A
			public <ParallelForEach>c__AnonStorey2()
			{
			}

			// Token: 0x06007296 RID: 29334 RVA: 0x00075A32 File Offset: 0x00073E32
			internal TResult <>m__0()
			{
				return this.<>f__ref$1.action(this.tmp);
			}

			// Token: 0x0400667D RID: 26237
			internal T tmp;

			// Token: 0x0400667E RID: 26238
			internal EnumerableExtension.<ParallelForEach>c__AnonStorey1<TResult, T> <>f__ref$1;
		}

		// Token: 0x02000F02 RID: 3842
		[CompilerGenerated]
		private sealed class <SequentialForEach>c__AnonStorey3<T>
		{
			// Token: 0x06007297 RID: 29335 RVA: 0x00075A4A File Offset: 0x00073E4A
			public <SequentialForEach>c__AnonStorey3()
			{
			}

			// Token: 0x06007298 RID: 29336 RVA: 0x00075A54 File Offset: 0x00073E54
			internal Task.Unit <>m__0(T element)
			{
				this.action(element);
				return default(Task.Unit);
			}

			// Token: 0x0400667F RID: 26239
			internal Action<T> action;
		}

		// Token: 0x02000F03 RID: 3843
		[CompilerGenerated]
		private sealed class <SequentialForEach>c__AnonStorey4<TResult, T>
		{
			// Token: 0x06007299 RID: 29337 RVA: 0x00075A76 File Offset: 0x00073E76
			public <SequentialForEach>c__AnonStorey4()
			{
			}

			// Token: 0x04006680 RID: 26240
			internal Func<T, TResult> action;

			// Token: 0x04006681 RID: 26241
			internal TaskDistributor target;
		}

		// Token: 0x02000F04 RID: 3844
		[CompilerGenerated]
		private sealed class <SequentialForEach>c__AnonStorey5<TResult, T>
		{
			// Token: 0x0600729A RID: 29338 RVA: 0x00075A7E File Offset: 0x00073E7E
			public <SequentialForEach>c__AnonStorey5()
			{
			}

			// Token: 0x0600729B RID: 29339 RVA: 0x00075A86 File Offset: 0x00073E86
			internal TResult <>m__0()
			{
				return this.<>f__ref$4.action(this.tmp);
			}

			// Token: 0x0600729C RID: 29340 RVA: 0x00075A9E File Offset: 0x00073E9E
			internal void <>m__1()
			{
				this.task.Run(this.<>f__ref$4.target);
			}

			// Token: 0x04006682 RID: 26242
			internal T tmp;

			// Token: 0x04006683 RID: 26243
			internal Task<TResult> task;

			// Token: 0x04006684 RID: 26244
			internal EnumerableExtension.<SequentialForEach>c__AnonStorey4<TResult, T> <>f__ref$4;
		}
	}
}
