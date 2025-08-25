using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace UnityThreading
{
	// Token: 0x02000354 RID: 852
	public abstract class ThreadBase : IDisposable
	{
		// Token: 0x060014E9 RID: 5353 RVA: 0x00076912 File Offset: 0x00074D12
		public ThreadBase(string threadName) : this(threadName, true)
		{
		}

		// Token: 0x060014EA RID: 5354 RVA: 0x0007691C File Offset: 0x00074D1C
		public ThreadBase(string threadName, bool autoStartThread) : this(threadName, Dispatcher.CurrentNoThrow, autoStartThread)
		{
		}

		// Token: 0x060014EB RID: 5355 RVA: 0x0007692B File Offset: 0x00074D2B
		public ThreadBase(string threadName, Dispatcher targetDispatcher) : this(threadName, targetDispatcher, true)
		{
		}

		// Token: 0x060014EC RID: 5356 RVA: 0x00076936 File Offset: 0x00074D36
		public ThreadBase(string threadName, Dispatcher targetDispatcher, bool autoStartThread)
		{
			this.threadName = threadName;
			this.targetDispatcher = targetDispatcher;
			if (autoStartThread)
			{
				this.Start();
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x060014ED RID: 5357 RVA: 0x0007696B File Offset: 0x00074D6B
		public static int AvailableProcessors
		{
			get
			{
				return SystemInfo.processorCount;
			}
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x060014EE RID: 5358 RVA: 0x00076972 File Offset: 0x00074D72
		public static ThreadBase CurrentThread
		{
			get
			{
				return ThreadBase.currentThread;
			}
		}

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x060014EF RID: 5359 RVA: 0x00076979 File Offset: 0x00074D79
		public bool IsAlive
		{
			get
			{
				return this.thread != null && this.thread.IsAlive;
			}
		}

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x060014F0 RID: 5360 RVA: 0x00076997 File Offset: 0x00074D97
		public bool ShouldStop
		{
			get
			{
				return this.exitEvent.InterWaitOne(0);
			}
		}

		// Token: 0x060014F1 RID: 5361 RVA: 0x000769A8 File Offset: 0x00074DA8
		public void Start()
		{
			if (this.thread != null)
			{
				this.Abort();
			}
			this.exitEvent.Reset();
			this.thread = new Thread(new ThreadStart(this.DoInternal));
			this.thread.Name = this.threadName;
			this.thread.Priority = this.priority;
			this.thread.Start();
		}

		// Token: 0x060014F2 RID: 5362 RVA: 0x00076A16 File Offset: 0x00074E16
		public void Exit()
		{
			if (this.thread != null)
			{
				this.exitEvent.Set();
			}
		}

		// Token: 0x060014F3 RID: 5363 RVA: 0x00076A2F File Offset: 0x00074E2F
		public void Abort()
		{
			this.Exit();
			if (this.thread != null)
			{
				this.thread.Join();
			}
		}

		// Token: 0x060014F4 RID: 5364 RVA: 0x00076A50 File Offset: 0x00074E50
		public void AbortWaitForSeconds(float seconds)
		{
			this.Exit();
			if (this.thread != null)
			{
				this.thread.Join((int)(seconds * 1000f));
				if (this.thread.IsAlive)
				{
					this.thread.Abort();
				}
			}
		}

		// Token: 0x060014F5 RID: 5365 RVA: 0x00076A9D File Offset: 0x00074E9D
		public Task<T> Dispatch<T>(Func<T> function)
		{
			return this.targetDispatcher.Dispatch<T>(function);
		}

		// Token: 0x060014F6 RID: 5366 RVA: 0x00076AAC File Offset: 0x00074EAC
		public T DispatchAndWait<T>(Func<T> function)
		{
			Task<T> task = this.Dispatch<T>(function);
			task.Wait();
			return task.Result;
		}

		// Token: 0x060014F7 RID: 5367 RVA: 0x00076AD0 File Offset: 0x00074ED0
		public T DispatchAndWait<T>(Func<T> function, float timeOutSeconds)
		{
			Task<T> task = this.Dispatch<T>(function);
			task.WaitForSeconds(timeOutSeconds);
			return task.Result;
		}

		// Token: 0x060014F8 RID: 5368 RVA: 0x00076AF2 File Offset: 0x00074EF2
		public Task Dispatch(Action action)
		{
			return this.targetDispatcher.Dispatch(action);
		}

		// Token: 0x060014F9 RID: 5369 RVA: 0x00076B00 File Offset: 0x00074F00
		public void DispatchAndWait(Action action)
		{
			Task task = this.Dispatch(action);
			task.Wait();
		}

		// Token: 0x060014FA RID: 5370 RVA: 0x00076B1C File Offset: 0x00074F1C
		public void DispatchAndWait(Action action, float timeOutSeconds)
		{
			Task task = this.Dispatch(action);
			task.WaitForSeconds(timeOutSeconds);
		}

		// Token: 0x060014FB RID: 5371 RVA: 0x00076B38 File Offset: 0x00074F38
		public Task Dispatch(Task taskBase)
		{
			return this.targetDispatcher.Dispatch(taskBase);
		}

		// Token: 0x060014FC RID: 5372 RVA: 0x00076B48 File Offset: 0x00074F48
		public void DispatchAndWait(Task taskBase)
		{
			Task task = this.Dispatch(taskBase);
			task.Wait();
		}

		// Token: 0x060014FD RID: 5373 RVA: 0x00076B64 File Offset: 0x00074F64
		public void DispatchAndWait(Task taskBase, float timeOutSeconds)
		{
			Task task = this.Dispatch(taskBase);
			task.WaitForSeconds(timeOutSeconds);
		}

		// Token: 0x060014FE RID: 5374 RVA: 0x00076B80 File Offset: 0x00074F80
		protected void DoInternal()
		{
			ThreadBase.currentThread = this;
			IEnumerator enumerator = this.Do();
			if (enumerator == null)
			{
				return;
			}
			this.RunEnumerator(enumerator);
		}

		// Token: 0x060014FF RID: 5375 RVA: 0x00076BA8 File Offset: 0x00074FA8
		private void RunEnumerator(IEnumerator enumerator)
		{
			ThreadBase.<RunEnumerator>c__AnonStorey0 <RunEnumerator>c__AnonStorey = new ThreadBase.<RunEnumerator>c__AnonStorey0();
			<RunEnumerator>c__AnonStorey.enumerator = enumerator;
			<RunEnumerator>c__AnonStorey.$this = this;
			for (;;)
			{
				if (<RunEnumerator>c__AnonStorey.enumerator.Current is Task)
				{
					Task taskBase = (Task)<RunEnumerator>c__AnonStorey.enumerator.Current;
					this.DispatchAndWait(taskBase);
				}
				else if (<RunEnumerator>c__AnonStorey.enumerator.Current is SwitchTo)
				{
					SwitchTo switchTo = (SwitchTo)<RunEnumerator>c__AnonStorey.enumerator.Current;
					if (switchTo.Target == SwitchTo.TargetType.Main && ThreadBase.CurrentThread != null)
					{
						Task taskBase2 = Task.Create(new Action(<RunEnumerator>c__AnonStorey.<>m__0));
						this.DispatchAndWait(taskBase2);
					}
					else if (switchTo.Target == SwitchTo.TargetType.Thread && ThreadBase.CurrentThread == null)
					{
						break;
					}
				}
				if (!<RunEnumerator>c__AnonStorey.enumerator.MoveNext() || this.ShouldStop)
				{
					return;
				}
			}
		}

		// Token: 0x06001500 RID: 5376
		protected abstract IEnumerator Do();

		// Token: 0x06001501 RID: 5377 RVA: 0x00076C86 File Offset: 0x00075086
		public virtual void Dispose()
		{
			this.AbortWaitForSeconds(1f);
		}

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x06001502 RID: 5378 RVA: 0x00076C93 File Offset: 0x00075093
		// (set) Token: 0x06001503 RID: 5379 RVA: 0x00076C9B File Offset: 0x0007509B
		public System.Threading.ThreadPriority Priority
		{
			get
			{
				return this.priority;
			}
			set
			{
				this.priority = value;
				if (this.thread != null)
				{
					this.thread.Priority = this.priority;
				}
			}
		}

		// Token: 0x040011AD RID: 4525
		protected Dispatcher targetDispatcher;

		// Token: 0x040011AE RID: 4526
		protected Thread thread;

		// Token: 0x040011AF RID: 4527
		protected ManualResetEvent exitEvent = new ManualResetEvent(false);

		// Token: 0x040011B0 RID: 4528
		[ThreadStatic]
		private static ThreadBase currentThread;

		// Token: 0x040011B1 RID: 4529
		private string threadName;

		// Token: 0x040011B2 RID: 4530
		private System.Threading.ThreadPriority priority = System.Threading.ThreadPriority.BelowNormal;

		// Token: 0x02000F29 RID: 3881
		[CompilerGenerated]
		private sealed class <RunEnumerator>c__AnonStorey0
		{
			// Token: 0x060072EC RID: 29420 RVA: 0x00076CC0 File Offset: 0x000750C0
			public <RunEnumerator>c__AnonStorey0()
			{
			}

			// Token: 0x060072ED RID: 29421 RVA: 0x00076CC8 File Offset: 0x000750C8
			internal void <>m__0()
			{
				if (this.enumerator.MoveNext() && !this.$this.ShouldStop)
				{
					this.$this.RunEnumerator(this.enumerator);
				}
			}

			// Token: 0x040066C7 RID: 26311
			internal IEnumerator enumerator;

			// Token: 0x040066C8 RID: 26312
			internal ThreadBase $this;
		}
	}
}
