using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace UnityThreading
{
	// Token: 0x0200034E RID: 846
	public abstract class Task
	{
		// Token: 0x0600147A RID: 5242 RVA: 0x00075B3B File Offset: 0x00073F3B
		public Task()
		{
		}

		// Token: 0x0600147B RID: 5243 RVA: 0x00075B74 File Offset: 0x00073F74
		~Task()
		{
			this.Dispose();
		}

		// Token: 0x1400007C RID: 124
		// (add) Token: 0x0600147C RID: 5244 RVA: 0x00075BA4 File Offset: 0x00073FA4
		// (remove) Token: 0x0600147D RID: 5245 RVA: 0x00075BDC File Offset: 0x00073FDC
		private event TaskEndedEventHandler taskEnded
		{
			add
			{
				TaskEndedEventHandler taskEndedEventHandler = this.taskEnded;
				TaskEndedEventHandler taskEndedEventHandler2;
				do
				{
					taskEndedEventHandler2 = taskEndedEventHandler;
					taskEndedEventHandler = Interlocked.CompareExchange<TaskEndedEventHandler>(ref this.taskEnded, (TaskEndedEventHandler)Delegate.Combine(taskEndedEventHandler2, value), taskEndedEventHandler);
				}
				while (taskEndedEventHandler != taskEndedEventHandler2);
			}
			remove
			{
				TaskEndedEventHandler taskEndedEventHandler = this.taskEnded;
				TaskEndedEventHandler taskEndedEventHandler2;
				do
				{
					taskEndedEventHandler2 = taskEndedEventHandler;
					taskEndedEventHandler = Interlocked.CompareExchange<TaskEndedEventHandler>(ref this.taskEnded, (TaskEndedEventHandler)Delegate.Remove(taskEndedEventHandler2, value), taskEndedEventHandler);
				}
				while (taskEndedEventHandler != taskEndedEventHandler2);
			}
		}

		// Token: 0x1400007D RID: 125
		// (add) Token: 0x0600147E RID: 5246 RVA: 0x00075C14 File Offset: 0x00074014
		// (remove) Token: 0x0600147F RID: 5247 RVA: 0x00075C70 File Offset: 0x00074070
		public event TaskEndedEventHandler TaskEnded
		{
			add
			{
				object obj = this.syncRoot;
				lock (obj)
				{
					if (this.endingEvent.InterWaitOne(0))
					{
						value(this);
					}
					else
					{
						this.taskEnded += value;
					}
				}
			}
			remove
			{
				object obj = this.syncRoot;
				lock (obj)
				{
					this.taskEnded -= value;
				}
			}
		}

		// Token: 0x06001480 RID: 5248 RVA: 0x00075CB0 File Offset: 0x000740B0
		private void End()
		{
			object obj = this.syncRoot;
			lock (obj)
			{
				this.endingEvent.Set();
				if (this.taskEnded != null)
				{
					this.taskEnded(this);
				}
				this.endedEvent.Set();
				if (Task.current == this)
				{
					Task.current = null;
				}
				this.hasEnded = true;
			}
		}

		// Token: 0x06001481 RID: 5249
		protected abstract IEnumerator Do();

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06001482 RID: 5250 RVA: 0x00075D30 File Offset: 0x00074130
		public static Task Current
		{
			get
			{
				return Task.current;
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06001483 RID: 5251 RVA: 0x00075D37 File Offset: 0x00074137
		public bool ShouldAbort
		{
			get
			{
				return this.abortEvent.InterWaitOne(0);
			}
		}

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06001484 RID: 5252 RVA: 0x00075D45 File Offset: 0x00074145
		public bool HasEnded
		{
			get
			{
				return this.hasEnded || this.endedEvent.InterWaitOne(0);
			}
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06001485 RID: 5253 RVA: 0x00075D61 File Offset: 0x00074161
		public bool IsEnding
		{
			get
			{
				return this.endingEvent.InterWaitOne(0);
			}
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06001486 RID: 5254 RVA: 0x00075D6F File Offset: 0x0007416F
		public bool IsSucceeded
		{
			get
			{
				return this.endingEvent.InterWaitOne(0) && !this.abortEvent.InterWaitOne(0);
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06001487 RID: 5255 RVA: 0x00075D94 File Offset: 0x00074194
		public bool IsFailed
		{
			get
			{
				return this.endingEvent.InterWaitOne(0) && this.abortEvent.InterWaitOne(0);
			}
		}

		// Token: 0x06001488 RID: 5256 RVA: 0x00075DB6 File Offset: 0x000741B6
		public void Abort()
		{
			this.abortEvent.Set();
			if (!this.hasStarted)
			{
				this.End();
			}
		}

		// Token: 0x06001489 RID: 5257 RVA: 0x00075DD5 File Offset: 0x000741D5
		public void AbortWait()
		{
			this.Abort();
			if (!this.hasStarted)
			{
				return;
			}
			this.Wait();
		}

		// Token: 0x0600148A RID: 5258 RVA: 0x00075DEF File Offset: 0x000741EF
		public void AbortWaitForSeconds(float seconds)
		{
			this.Abort();
			if (!this.hasStarted)
			{
				return;
			}
			this.WaitForSeconds(seconds);
		}

		// Token: 0x0600148B RID: 5259 RVA: 0x00075E0A File Offset: 0x0007420A
		public void Wait()
		{
			if (this.hasEnded)
			{
				return;
			}
			this.Priority--;
			this.endedEvent.WaitOne();
		}

		// Token: 0x0600148C RID: 5260 RVA: 0x00075E36 File Offset: 0x00074236
		public void WaitForSeconds(float seconds)
		{
			if (this.hasEnded)
			{
				return;
			}
			this.Priority--;
			this.endedEvent.InterWaitOne(TimeSpan.FromSeconds((double)seconds));
		}

		// Token: 0x0600148D RID: 5261
		public abstract TResult Wait<TResult>();

		// Token: 0x0600148E RID: 5262
		public abstract TResult WaitForSeconds<TResult>(float seconds);

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x0600148F RID: 5263
		public abstract object RawResult { get; }

		// Token: 0x06001490 RID: 5264
		public abstract TResult WaitForSeconds<TResult>(float seconds, TResult defaultReturnValue);

		// Token: 0x06001491 RID: 5265 RVA: 0x00075E6C File Offset: 0x0007426C
		internal void DoInternal()
		{
			Task.current = this;
			this.hasStarted = true;
			if (!this.ShouldAbort)
			{
				try
				{
					IEnumerator enumerator = this.Do();
					if (enumerator == null)
					{
						this.End();
						return;
					}
					this.RunEnumerator(enumerator);
				}
				catch (Exception ex)
				{
					this.Abort();
					if (string.IsNullOrEmpty(this.Name))
					{
						UnityEngine.Debug.LogError("Error while processing task:\n" + ex.ToString());
					}
					else
					{
						UnityEngine.Debug.LogError("Error while processing task '" + this.Name + "':\n" + ex.ToString());
					}
				}
			}
			this.End();
		}

		// Token: 0x06001492 RID: 5266 RVA: 0x00075F24 File Offset: 0x00074324
		private void RunEnumerator(IEnumerator enumerator)
		{
			Task.<RunEnumerator>c__AnonStorey0 <RunEnumerator>c__AnonStorey = new Task.<RunEnumerator>c__AnonStorey0();
			<RunEnumerator>c__AnonStorey.enumerator = enumerator;
			<RunEnumerator>c__AnonStorey.$this = this;
			ThreadBase currentThread = ThreadBase.CurrentThread;
			for (;;)
			{
				if (<RunEnumerator>c__AnonStorey.enumerator.Current is Task)
				{
					Task taskBase = (Task)<RunEnumerator>c__AnonStorey.enumerator.Current;
					currentThread.DispatchAndWait(taskBase);
				}
				else if (<RunEnumerator>c__AnonStorey.enumerator.Current is SwitchTo)
				{
					SwitchTo switchTo = (SwitchTo)<RunEnumerator>c__AnonStorey.enumerator.Current;
					if (switchTo.Target == SwitchTo.TargetType.Main && currentThread != null)
					{
						Task taskBase2 = Task.Create(new Action(<RunEnumerator>c__AnonStorey.<>m__0));
						currentThread.DispatchAndWait(taskBase2);
					}
					else if (switchTo.Target == SwitchTo.TargetType.Thread && currentThread == null)
					{
						break;
					}
				}
				if (!<RunEnumerator>c__AnonStorey.enumerator.MoveNext() || this.ShouldAbort)
				{
					return;
				}
			}
		}

		// Token: 0x06001493 RID: 5267 RVA: 0x00076004 File Offset: 0x00074404
		public void Dispose()
		{
			if (this.disposed)
			{
				return;
			}
			this.disposed = true;
			if (this.hasStarted)
			{
				this.Wait();
			}
			this.endingEvent.Close();
			this.endedEvent.Close();
			this.abortEvent.Close();
		}

		// Token: 0x06001494 RID: 5268 RVA: 0x00076056 File Offset: 0x00074456
		public Task Run(DispatcherBase target)
		{
			if (target == null)
			{
				return this.Run();
			}
			target.Dispatch(this);
			return this;
		}

		// Token: 0x06001495 RID: 5269 RVA: 0x0007606E File Offset: 0x0007446E
		public Task Run()
		{
			this.Run(UnityThreadHelper.TaskDistributor);
			return this;
		}

		// Token: 0x06001496 RID: 5270 RVA: 0x0007607D File Offset: 0x0007447D
		public static Task Create(Action<Task> action)
		{
			return new Task<Task.Unit>(action);
		}

		// Token: 0x06001497 RID: 5271 RVA: 0x00076085 File Offset: 0x00074485
		public static Task Create(Action action)
		{
			return new Task<Task.Unit>(action);
		}

		// Token: 0x06001498 RID: 5272 RVA: 0x0007608D File Offset: 0x0007448D
		public static Task<T> Create<T>(Func<Task, T> func)
		{
			return new Task<T>(func);
		}

		// Token: 0x06001499 RID: 5273 RVA: 0x00076095 File Offset: 0x00074495
		public static Task<T> Create<T>(Func<T> func)
		{
			return new Task<T>(func);
		}

		// Token: 0x0600149A RID: 5274 RVA: 0x0007609D File Offset: 0x0007449D
		public static Task Create(IEnumerator enumerator)
		{
			return new Task<IEnumerator>(enumerator);
		}

		// Token: 0x0600149B RID: 5275 RVA: 0x000760A5 File Offset: 0x000744A5
		public static Task<T> Create<T>(Type type, string methodName, params object[] args)
		{
			return new Task<T>(type, methodName, args);
		}

		// Token: 0x0600149C RID: 5276 RVA: 0x000760AF File Offset: 0x000744AF
		public static Task<T> Create<T>(object that, string methodName, params object[] args)
		{
			return new Task<T>(that, methodName, args);
		}

		// Token: 0x04001197 RID: 4503
		private object syncRoot = new object();

		// Token: 0x04001198 RID: 4504
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private TaskEndedEventHandler taskEnded;

		// Token: 0x04001199 RID: 4505
		private bool hasEnded;

		// Token: 0x0400119A RID: 4506
		public string Name;

		// Token: 0x0400119B RID: 4507
		public volatile int Priority;

		// Token: 0x0400119C RID: 4508
		private ManualResetEvent abortEvent = new ManualResetEvent(false);

		// Token: 0x0400119D RID: 4509
		private ManualResetEvent endedEvent = new ManualResetEvent(false);

		// Token: 0x0400119E RID: 4510
		private ManualResetEvent endingEvent = new ManualResetEvent(false);

		// Token: 0x0400119F RID: 4511
		private bool hasStarted;

		// Token: 0x040011A0 RID: 4512
		[ThreadStatic]
		private static Task current;

		// Token: 0x040011A1 RID: 4513
		private bool disposed;

		// Token: 0x0200034F RID: 847
		public struct Unit
		{
		}

		// Token: 0x02000F05 RID: 3845
		[CompilerGenerated]
		private sealed class <RunEnumerator>c__AnonStorey0
		{
			// Token: 0x0600729D RID: 29341 RVA: 0x000760B9 File Offset: 0x000744B9
			public <RunEnumerator>c__AnonStorey0()
			{
			}

			// Token: 0x0600729E RID: 29342 RVA: 0x000760C1 File Offset: 0x000744C1
			internal void <>m__0()
			{
				if (this.enumerator.MoveNext() && !this.$this.ShouldAbort)
				{
					this.$this.RunEnumerator(this.enumerator);
				}
			}

			// Token: 0x04006685 RID: 26245
			internal IEnumerator enumerator;

			// Token: 0x04006686 RID: 26246
			internal Task $this;
		}
	}
}
