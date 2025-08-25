using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;

namespace UnityThreading
{
	// Token: 0x02000346 RID: 838
	public class Dispatcher : DispatcherBase
	{
		// Token: 0x06001451 RID: 5201 RVA: 0x00075300 File Offset: 0x00073700
		public Dispatcher() : this(true)
		{
		}

		// Token: 0x06001452 RID: 5202 RVA: 0x00075309 File Offset: 0x00073709
		public Dispatcher(bool setThreadDefaults)
		{
			if (!setThreadDefaults)
			{
				return;
			}
			if (Dispatcher.currentDispatcher != null)
			{
				throw new InvalidOperationException("Only one Dispatcher instance allowed per thread.");
			}
			Dispatcher.currentDispatcher = this;
			if (Dispatcher.mainDispatcher == null)
			{
				Dispatcher.mainDispatcher = this;
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x06001453 RID: 5203 RVA: 0x00075343 File Offset: 0x00073743
		public static Task CurrentTask
		{
			get
			{
				if (Dispatcher.currentTask == null)
				{
					throw new InvalidOperationException("No task is currently running.");
				}
				return Dispatcher.currentTask;
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06001454 RID: 5204 RVA: 0x0007535F File Offset: 0x0007375F
		// (set) Token: 0x06001455 RID: 5205 RVA: 0x0007537B File Offset: 0x0007377B
		public static Dispatcher Current
		{
			get
			{
				if (Dispatcher.currentDispatcher == null)
				{
					throw new InvalidOperationException("No Dispatcher found for the current thread, please create a new Dispatcher instance before calling this property.");
				}
				return Dispatcher.currentDispatcher;
			}
			set
			{
				if (Dispatcher.currentDispatcher != null)
				{
					Dispatcher.currentDispatcher.Dispose();
				}
				Dispatcher.currentDispatcher = value;
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06001456 RID: 5206 RVA: 0x00075397 File Offset: 0x00073797
		public static Dispatcher CurrentNoThrow
		{
			get
			{
				return Dispatcher.currentDispatcher;
			}
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06001457 RID: 5207 RVA: 0x0007539E File Offset: 0x0007379E
		public static Dispatcher Main
		{
			get
			{
				if (Dispatcher.mainDispatcher == null)
				{
					throw new InvalidOperationException("No Dispatcher found for the main thread, please create a new Dispatcher instance before calling this property.");
				}
				return Dispatcher.mainDispatcher;
			}
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06001458 RID: 5208 RVA: 0x000753BA File Offset: 0x000737BA
		public static Dispatcher MainNoThrow
		{
			get
			{
				return Dispatcher.mainDispatcher;
			}
		}

		// Token: 0x06001459 RID: 5209 RVA: 0x000753C4 File Offset: 0x000737C4
		public static Func<T> CreateSafeFunction<T>(Func<T> function)
		{
			Dispatcher.<CreateSafeFunction>c__AnonStorey0<T> <CreateSafeFunction>c__AnonStorey = new Dispatcher.<CreateSafeFunction>c__AnonStorey0<T>();
			<CreateSafeFunction>c__AnonStorey.function = function;
			return new Func<T>(<CreateSafeFunction>c__AnonStorey.<>m__0);
		}

		// Token: 0x0600145A RID: 5210 RVA: 0x000753EC File Offset: 0x000737EC
		public static Action CreateSafeAction<T>(Action action)
		{
			Dispatcher.<CreateSafeAction>c__AnonStorey1<T> <CreateSafeAction>c__AnonStorey = new Dispatcher.<CreateSafeAction>c__AnonStorey1<T>();
			<CreateSafeAction>c__AnonStorey.action = action;
			return new Action(<CreateSafeAction>c__AnonStorey.<>m__0);
		}

		// Token: 0x0600145B RID: 5211 RVA: 0x00075412 File Offset: 0x00073812
		public void ProcessTasks()
		{
			if (this.dataEvent.InterWaitOne(0))
			{
				this.ProcessTasksInternal();
			}
		}

		// Token: 0x0600145C RID: 5212 RVA: 0x0007542C File Offset: 0x0007382C
		public bool ProcessTasks(WaitHandle exitHandle)
		{
			if (WaitHandle.WaitAny(new WaitHandle[]
			{
				exitHandle,
				this.dataEvent
			}) == 0)
			{
				return false;
			}
			this.ProcessTasksInternal();
			return true;
		}

		// Token: 0x0600145D RID: 5213 RVA: 0x00075464 File Offset: 0x00073864
		public bool ProcessNextTask()
		{
			object taskListSyncRoot = this.taskListSyncRoot;
			Task task;
			lock (taskListSyncRoot)
			{
				if (this.taskList.Count == 0)
				{
					return false;
				}
				task = this.taskList.Dequeue();
			}
			this.ProcessSingleTask(task);
			if (this.TaskCount == 0)
			{
				this.dataEvent.Reset();
			}
			return true;
		}

		// Token: 0x0600145E RID: 5214 RVA: 0x000754E0 File Offset: 0x000738E0
		public bool ProcessNextTask(WaitHandle exitHandle)
		{
			if (WaitHandle.WaitAny(new WaitHandle[]
			{
				exitHandle,
				this.dataEvent
			}) == 0)
			{
				return false;
			}
			object taskListSyncRoot = this.taskListSyncRoot;
			Task task;
			lock (taskListSyncRoot)
			{
				if (this.taskList.Count == 0)
				{
					return false;
				}
				task = this.taskList.Dequeue();
			}
			this.ProcessSingleTask(task);
			if (this.TaskCount == 0)
			{
				this.dataEvent.Reset();
			}
			return true;
		}

		// Token: 0x0600145F RID: 5215 RVA: 0x0007557C File Offset: 0x0007397C
		private void ProcessTasksInternal()
		{
			object taskListSyncRoot = this.taskListSyncRoot;
			List<Task> list;
			lock (taskListSyncRoot)
			{
				list = new List<Task>(this.taskList);
				this.taskList.Clear();
			}
			while (list.Count != 0)
			{
				Task task = list[0];
				list.RemoveAt(0);
				this.ProcessSingleTask(task);
			}
			if (this.TaskCount == 0)
			{
				this.dataEvent.Reset();
			}
		}

		// Token: 0x06001460 RID: 5216 RVA: 0x00075608 File Offset: 0x00073A08
		private void ProcessSingleTask(Task task)
		{
			this.RunTask(task);
			if (this.TaskSortingSystem == TaskSortingSystem.ReorderWhenExecuted)
			{
				object taskListSyncRoot = this.taskListSyncRoot;
				lock (taskListSyncRoot)
				{
					base.ReorderTasks();
				}
			}
		}

		// Token: 0x06001461 RID: 5217 RVA: 0x00075658 File Offset: 0x00073A58
		internal void RunTask(Task task)
		{
			Task task2 = Dispatcher.currentTask;
			Dispatcher.currentTask = task;
			Dispatcher.currentTask.DoInternal();
			Dispatcher.currentTask = task2;
		}

		// Token: 0x06001462 RID: 5218 RVA: 0x00075681 File Offset: 0x00073A81
		protected override void CheckAccessLimitation()
		{
			if (this.AllowAccessLimitationChecks && Dispatcher.currentDispatcher == this)
			{
				throw new InvalidOperationException("Dispatching a Task with the Dispatcher associated to the current thread is prohibited. You can run these Tasks without the need of a Dispatcher.");
			}
		}

		// Token: 0x06001463 RID: 5219 RVA: 0x000756A4 File Offset: 0x00073AA4
		public override void Dispose()
		{
			for (;;)
			{
				object taskListSyncRoot = this.taskListSyncRoot;
				lock (taskListSyncRoot)
				{
					if (this.taskList.Count == 0)
					{
						break;
					}
					Dispatcher.currentTask = this.taskList.Dequeue();
				}
				Dispatcher.currentTask.Dispose();
			}
			this.dataEvent.Close();
			this.dataEvent = null;
			if (Dispatcher.currentDispatcher == this)
			{
				Dispatcher.currentDispatcher = null;
			}
			if (Dispatcher.mainDispatcher == this)
			{
				Dispatcher.mainDispatcher = null;
			}
		}

		// Token: 0x0400118A RID: 4490
		[ThreadStatic]
		private static Task currentTask;

		// Token: 0x0400118B RID: 4491
		[ThreadStatic]
		internal static Dispatcher currentDispatcher;

		// Token: 0x0400118C RID: 4492
		protected static Dispatcher mainDispatcher;

		// Token: 0x02000EFD RID: 3837
		[CompilerGenerated]
		private sealed class <CreateSafeFunction>c__AnonStorey0<T>
		{
			// Token: 0x0600728E RID: 29326 RVA: 0x00075748 File Offset: 0x00073B48
			public <CreateSafeFunction>c__AnonStorey0()
			{
			}

			// Token: 0x0600728F RID: 29327 RVA: 0x00075750 File Offset: 0x00073B50
			internal T <>m__0()
			{
				T result;
				try
				{
					result = this.function();
				}
				catch
				{
					Dispatcher.CurrentTask.Abort();
					result = default(T);
				}
				return result;
			}

			// Token: 0x04006679 RID: 26233
			internal Func<T> function;
		}

		// Token: 0x02000EFE RID: 3838
		[CompilerGenerated]
		private sealed class <CreateSafeAction>c__AnonStorey1<T>
		{
			// Token: 0x06007290 RID: 29328 RVA: 0x0007579C File Offset: 0x00073B9C
			public <CreateSafeAction>c__AnonStorey1()
			{
			}

			// Token: 0x06007291 RID: 29329 RVA: 0x000757A4 File Offset: 0x00073BA4
			internal void <>m__0()
			{
				try
				{
					this.action();
				}
				catch
				{
					Dispatcher.CurrentTask.Abort();
				}
			}

			// Token: 0x0400667A RID: 26234
			internal Action action;
		}
	}
}
