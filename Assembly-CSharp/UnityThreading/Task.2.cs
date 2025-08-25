using System;
using System.Collections;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace UnityThreading
{
	// Token: 0x02000350 RID: 848
	public class Task<T> : Task
	{
		// Token: 0x0600149D RID: 5277 RVA: 0x000760F4 File Offset: 0x000744F4
		public Task(Func<Task, T> function)
		{
			this.function = function;
		}

		// Token: 0x0600149E RID: 5278 RVA: 0x00076104 File Offset: 0x00074504
		public Task(Func<T> function)
		{
			Task<T>.<Task>c__AnonStorey0 <Task>c__AnonStorey = new Task<T>.<Task>c__AnonStorey0();
			<Task>c__AnonStorey.function = function;
			base..ctor();
			this.function = new Func<Task, T>(<Task>c__AnonStorey.<>m__0);
		}

		// Token: 0x0600149F RID: 5279 RVA: 0x00076138 File Offset: 0x00074538
		public Task(Action<Task> action)
		{
			Task<T>.<Task>c__AnonStorey1 <Task>c__AnonStorey = new Task<T>.<Task>c__AnonStorey1();
			<Task>c__AnonStorey.action = action;
			base..ctor();
			this.function = new Func<Task, T>(<Task>c__AnonStorey.<>m__0);
		}

		// Token: 0x060014A0 RID: 5280 RVA: 0x0007616C File Offset: 0x0007456C
		public Task(Action action)
		{
			Task<T>.<Task>c__AnonStorey2 <Task>c__AnonStorey = new Task<T>.<Task>c__AnonStorey2();
			<Task>c__AnonStorey.action = action;
			base..ctor();
			this.function = new Func<Task, T>(<Task>c__AnonStorey.<>m__0);
		}

		// Token: 0x060014A1 RID: 5281 RVA: 0x000761A0 File Offset: 0x000745A0
		public Task(IEnumerator enumerator)
		{
			Task<T>.<Task>c__AnonStorey3 <Task>c__AnonStorey = new Task<T>.<Task>c__AnonStorey3();
			<Task>c__AnonStorey.enumerator = enumerator;
			base..ctor();
			this.function = new Func<Task, T>(<Task>c__AnonStorey.<>m__0);
		}

		// Token: 0x060014A2 RID: 5282 RVA: 0x000761D4 File Offset: 0x000745D4
		public Task(Type type, string methodName, params object[] args)
		{
			Task<T>.<Task>c__AnonStorey4 <Task>c__AnonStorey = new Task<T>.<Task>c__AnonStorey4();
			<Task>c__AnonStorey.args = args;
			base..ctor();
			<Task>c__AnonStorey.methodInfo = type.GetMethod(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
			if (<Task>c__AnonStorey.methodInfo == null)
			{
				throw new ArgumentException("methodName", "Fitting method with the given name was not found.");
			}
			this.function = new Func<Task, T>(<Task>c__AnonStorey.<>m__0);
		}

		// Token: 0x060014A3 RID: 5283 RVA: 0x00076234 File Offset: 0x00074634
		public Task(object that, string methodName, params object[] args)
		{
			Task<T>.<Task>c__AnonStorey5 <Task>c__AnonStorey = new Task<T>.<Task>c__AnonStorey5();
			<Task>c__AnonStorey.that = that;
			<Task>c__AnonStorey.args = args;
			base..ctor();
			<Task>c__AnonStorey.methodInfo = <Task>c__AnonStorey.that.GetType().GetMethod(methodName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.InvokeMethod);
			if (<Task>c__AnonStorey.methodInfo == null)
			{
				throw new ArgumentException("methodName", "Fitting method with the given name was not found.");
			}
			this.function = new Func<Task, T>(<Task>c__AnonStorey.<>m__0);
		}

		// Token: 0x060014A4 RID: 5284 RVA: 0x000762A4 File Offset: 0x000746A4
		protected override IEnumerator Do()
		{
			this.result = this.function(this);
			if (this.result is IEnumerator)
			{
				return (IEnumerator)((object)this.result);
			}
			return null;
		}

		// Token: 0x060014A5 RID: 5285 RVA: 0x000762DF File Offset: 0x000746DF
		public override TResult Wait<TResult>()
		{
			this.Priority--;
			return (TResult)((object)this.Result);
		}

		// Token: 0x060014A6 RID: 5286 RVA: 0x00076304 File Offset: 0x00074704
		public override TResult WaitForSeconds<TResult>(float seconds)
		{
			this.Priority--;
			return this.WaitForSeconds<TResult>(seconds, default(TResult));
		}

		// Token: 0x060014A7 RID: 5287 RVA: 0x00076333 File Offset: 0x00074733
		public override TResult WaitForSeconds<TResult>(float seconds, TResult defaultReturnValue)
		{
			if (!base.HasEnded)
			{
				base.WaitForSeconds(seconds);
			}
			if (base.IsSucceeded)
			{
				return (TResult)((object)this.result);
			}
			return defaultReturnValue;
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x060014A8 RID: 5288 RVA: 0x00076364 File Offset: 0x00074764
		public override object RawResult
		{
			get
			{
				if (!base.IsEnding)
				{
					base.Wait();
				}
				return this.result;
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x060014A9 RID: 5289 RVA: 0x00076382 File Offset: 0x00074782
		public T Result
		{
			get
			{
				if (!base.IsEnding)
				{
					base.Wait();
				}
				return this.result;
			}
		}

		// Token: 0x060014AA RID: 5290 RVA: 0x0007639B File Offset: 0x0007479B
		public new Task<T> Run(DispatcherBase target)
		{
			base.Run(target);
			return this;
		}

		// Token: 0x060014AB RID: 5291 RVA: 0x000763A6 File Offset: 0x000747A6
		public new Task<T> Run()
		{
			base.Run();
			return this;
		}

		// Token: 0x040011A2 RID: 4514
		private Func<Task, T> function;

		// Token: 0x040011A3 RID: 4515
		private T result;

		// Token: 0x02000F06 RID: 3846
		[CompilerGenerated]
		private sealed class <Task>c__AnonStorey0
		{
			// Token: 0x0600729F RID: 29343 RVA: 0x000763B0 File Offset: 0x000747B0
			public <Task>c__AnonStorey0()
			{
			}

			// Token: 0x060072A0 RID: 29344 RVA: 0x000763B8 File Offset: 0x000747B8
			internal T <>m__0(Task t)
			{
				return this.function();
			}

			// Token: 0x04006687 RID: 26247
			internal Func<T> function;
		}

		// Token: 0x02000F07 RID: 3847
		[CompilerGenerated]
		private sealed class <Task>c__AnonStorey1
		{
			// Token: 0x060072A1 RID: 29345 RVA: 0x000763C5 File Offset: 0x000747C5
			public <Task>c__AnonStorey1()
			{
			}

			// Token: 0x060072A2 RID: 29346 RVA: 0x000763D0 File Offset: 0x000747D0
			internal T <>m__0(Task t)
			{
				this.action(t);
				return default(T);
			}

			// Token: 0x04006688 RID: 26248
			internal Action<Task> action;
		}

		// Token: 0x02000F08 RID: 3848
		[CompilerGenerated]
		private sealed class <Task>c__AnonStorey2
		{
			// Token: 0x060072A3 RID: 29347 RVA: 0x000763F2 File Offset: 0x000747F2
			public <Task>c__AnonStorey2()
			{
			}

			// Token: 0x060072A4 RID: 29348 RVA: 0x000763FC File Offset: 0x000747FC
			internal T <>m__0(Task t)
			{
				this.action();
				return default(T);
			}

			// Token: 0x04006689 RID: 26249
			internal Action action;
		}

		// Token: 0x02000F09 RID: 3849
		[CompilerGenerated]
		private sealed class <Task>c__AnonStorey3
		{
			// Token: 0x060072A5 RID: 29349 RVA: 0x0007641D File Offset: 0x0007481D
			public <Task>c__AnonStorey3()
			{
			}

			// Token: 0x060072A6 RID: 29350 RVA: 0x00076425 File Offset: 0x00074825
			internal T <>m__0(Task t)
			{
				return (T)((object)this.enumerator);
			}

			// Token: 0x0400668A RID: 26250
			internal IEnumerator enumerator;
		}

		// Token: 0x02000F0A RID: 3850
		[CompilerGenerated]
		private sealed class <Task>c__AnonStorey4
		{
			// Token: 0x060072A7 RID: 29351 RVA: 0x00076432 File Offset: 0x00074832
			public <Task>c__AnonStorey4()
			{
			}

			// Token: 0x060072A8 RID: 29352 RVA: 0x0007643A File Offset: 0x0007483A
			internal T <>m__0(Task t)
			{
				return (T)((object)this.methodInfo.Invoke(null, this.args));
			}

			// Token: 0x0400668B RID: 26251
			internal MethodInfo methodInfo;

			// Token: 0x0400668C RID: 26252
			internal object[] args;
		}

		// Token: 0x02000F0B RID: 3851
		[CompilerGenerated]
		private sealed class <Task>c__AnonStorey5
		{
			// Token: 0x060072A9 RID: 29353 RVA: 0x00076453 File Offset: 0x00074853
			public <Task>c__AnonStorey5()
			{
			}

			// Token: 0x060072AA RID: 29354 RVA: 0x0007645B File Offset: 0x0007485B
			internal T <>m__0(Task t)
			{
				return (T)((object)this.methodInfo.Invoke(this.that, this.args));
			}

			// Token: 0x0400668D RID: 26253
			internal MethodInfo methodInfo;

			// Token: 0x0400668E RID: 26254
			internal object that;

			// Token: 0x0400668F RID: 26255
			internal object[] args;
		}
	}
}
