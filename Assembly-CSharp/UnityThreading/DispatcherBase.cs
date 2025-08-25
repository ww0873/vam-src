using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace UnityThreading
{
	// Token: 0x02000344 RID: 836
	public abstract class DispatcherBase : IDisposable
	{
		// Token: 0x0600143C RID: 5180 RVA: 0x00074DFC File Offset: 0x000731FC
		public DispatcherBase()
		{
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x0600143D RID: 5181 RVA: 0x00074E31 File Offset: 0x00073231
		public bool IsWorking
		{
			get
			{
				return this.dataEvent.InterWaitOne(0);
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x0600143E RID: 5182 RVA: 0x00074E40 File Offset: 0x00073240
		public virtual int TaskCount
		{
			get
			{
				object obj = this.taskListSyncRoot;
				int count;
				lock (obj)
				{
					count = this.taskList.Count;
				}
				return count;
			}
		}

		// Token: 0x0600143F RID: 5183 RVA: 0x00074E84 File Offset: 0x00073284
		public void Lock()
		{
			object obj = this.taskListSyncRoot;
			lock (obj)
			{
				this.lockCount++;
			}
		}

		// Token: 0x06001440 RID: 5184 RVA: 0x00074EC8 File Offset: 0x000732C8
		public void Unlock()
		{
			object obj = this.taskListSyncRoot;
			lock (obj)
			{
				this.lockCount--;
				if (this.lockCount == 0 && this.delayedTaskList.Count > 0)
				{
					while (this.delayedTaskList.Count > 0)
					{
						this.taskList.Enqueue(this.delayedTaskList.Dequeue());
					}
					if (this.TaskSortingSystem == TaskSortingSystem.ReorderWhenAdded || this.TaskSortingSystem == TaskSortingSystem.ReorderWhenExecuted)
					{
						this.ReorderTasks();
					}
					this.TasksAdded();
				}
			}
		}

		// Token: 0x06001441 RID: 5185 RVA: 0x00074F78 File Offset: 0x00073378
		public Task<T> Dispatch<T>(Func<T> function)
		{
			this.CheckAccessLimitation();
			Task<T> task = new Task<T>(function);
			this.AddTask(task);
			return task;
		}

		// Token: 0x06001442 RID: 5186 RVA: 0x00074F9C File Offset: 0x0007339C
		public Task Dispatch(Action action)
		{
			this.CheckAccessLimitation();
			Task task = Task.Create(action);
			this.AddTask(task);
			return task;
		}

		// Token: 0x06001443 RID: 5187 RVA: 0x00074FBE File Offset: 0x000733BE
		public Task Dispatch(Task task)
		{
			this.CheckAccessLimitation();
			this.AddTask(task);
			return task;
		}

		// Token: 0x06001444 RID: 5188 RVA: 0x00074FD0 File Offset: 0x000733D0
		internal virtual void AddTask(Task task)
		{
			object obj = this.taskListSyncRoot;
			lock (obj)
			{
				if (this.lockCount > 0)
				{
					this.delayedTaskList.Enqueue(task);
					return;
				}
				this.taskList.Enqueue(task);
				if (this.TaskSortingSystem == TaskSortingSystem.ReorderWhenAdded || this.TaskSortingSystem == TaskSortingSystem.ReorderWhenExecuted)
				{
					this.ReorderTasks();
				}
			}
			this.TasksAdded();
		}

		// Token: 0x06001445 RID: 5189 RVA: 0x00075054 File Offset: 0x00073454
		internal void AddTasks(IEnumerable<Task> tasks)
		{
			object obj = this.taskListSyncRoot;
			lock (obj)
			{
				if (this.lockCount > 0)
				{
					foreach (Task item in tasks)
					{
						this.delayedTaskList.Enqueue(item);
					}
					return;
				}
				foreach (Task item2 in tasks)
				{
					this.taskList.Enqueue(item2);
				}
				if (this.TaskSortingSystem == TaskSortingSystem.ReorderWhenAdded || this.TaskSortingSystem == TaskSortingSystem.ReorderWhenExecuted)
				{
					this.ReorderTasks();
				}
			}
			this.TasksAdded();
		}

		// Token: 0x06001446 RID: 5190 RVA: 0x00075154 File Offset: 0x00073554
		internal virtual void TasksAdded()
		{
			this.dataEvent.Set();
		}

		// Token: 0x06001447 RID: 5191 RVA: 0x00075162 File Offset: 0x00073562
		protected void ReorderTasks()
		{
			IEnumerable<Task> source = this.taskList;
			if (DispatcherBase.<>f__am$cache0 == null)
			{
				DispatcherBase.<>f__am$cache0 = new Func<Task, int>(DispatcherBase.<ReorderTasks>m__0);
			}
			this.taskList = new Queue<Task>(source.OrderBy(DispatcherBase.<>f__am$cache0));
		}

		// Token: 0x06001448 RID: 5192 RVA: 0x00075198 File Offset: 0x00073598
		internal IEnumerable<Task> SplitTasks(int divisor)
		{
			if (divisor == 0)
			{
				divisor = 2;
			}
			int count = this.TaskCount / divisor;
			return this.IsolateTasks(count);
		}

		// Token: 0x06001449 RID: 5193 RVA: 0x000751C0 File Offset: 0x000735C0
		internal IEnumerable<Task> IsolateTasks(int count)
		{
			Queue<Task> queue = new Queue<Task>();
			if (count == 0)
			{
				count = this.taskList.Count;
			}
			object obj = this.taskListSyncRoot;
			lock (obj)
			{
				int num = 0;
				while (num < count && num < this.taskList.Count)
				{
					queue.Enqueue(this.taskList.Dequeue());
					num++;
				}
				if (this.TaskCount == 0)
				{
					this.dataEvent.Reset();
				}
			}
			return queue;
		}

		// Token: 0x0600144A RID: 5194
		protected abstract void CheckAccessLimitation();

		// Token: 0x0600144B RID: 5195 RVA: 0x0007525C File Offset: 0x0007365C
		public virtual void Dispose()
		{
			for (;;)
			{
				object obj = this.taskListSyncRoot;
				Task task;
				lock (obj)
				{
					if (this.taskList.Count == 0)
					{
						break;
					}
					task = this.taskList.Dequeue();
				}
				task.Dispose();
			}
			this.dataEvent.Close();
			this.dataEvent = null;
		}

		// Token: 0x0600144C RID: 5196 RVA: 0x000752D8 File Offset: 0x000736D8
		[CompilerGenerated]
		private static int <ReorderTasks>m__0(Task t)
		{
			return t.Priority;
		}

		// Token: 0x04001181 RID: 4481
		protected int lockCount;

		// Token: 0x04001182 RID: 4482
		protected object taskListSyncRoot = new object();

		// Token: 0x04001183 RID: 4483
		protected Queue<Task> taskList = new Queue<Task>();

		// Token: 0x04001184 RID: 4484
		protected Queue<Task> delayedTaskList = new Queue<Task>();

		// Token: 0x04001185 RID: 4485
		protected ManualResetEvent dataEvent = new ManualResetEvent(false);

		// Token: 0x04001186 RID: 4486
		public bool AllowAccessLimitationChecks;

		// Token: 0x04001187 RID: 4487
		public TaskSortingSystem TaskSortingSystem;

		// Token: 0x04001188 RID: 4488
		[CompilerGenerated]
		private static Func<Task, int> <>f__am$cache0;
	}
}
