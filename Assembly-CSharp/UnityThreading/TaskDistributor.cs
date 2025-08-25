using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;

namespace UnityThreading
{
	// Token: 0x02000351 RID: 849
	public class TaskDistributor : DispatcherBase
	{
		// Token: 0x060014AC RID: 5292 RVA: 0x00076479 File Offset: 0x00074879
		public TaskDistributor(string name) : this(name, 0)
		{
		}

		// Token: 0x060014AD RID: 5293 RVA: 0x00076483 File Offset: 0x00074883
		public TaskDistributor(string name, int workerThreadCount) : this(name, workerThreadCount, true)
		{
		}

		// Token: 0x060014AE RID: 5294 RVA: 0x00076490 File Offset: 0x00074890
		public TaskDistributor(string name, int workerThreadCount, bool autoStart)
		{
			this.name = name;
			if (workerThreadCount <= 0)
			{
				workerThreadCount = ThreadBase.AvailableProcessors * 2;
			}
			this.workerThreads = new TaskWorker[workerThreadCount];
			object obj = this.workerThreads;
			lock (obj)
			{
				for (int i = 0; i < workerThreadCount; i++)
				{
					this.workerThreads[i] = new TaskWorker(name, this);
				}
			}
			if (TaskDistributor.mainTaskDistributor == null)
			{
				TaskDistributor.mainTaskDistributor = this;
			}
			if (autoStart)
			{
				this.Start();
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x060014AF RID: 5295 RVA: 0x00076534 File Offset: 0x00074934
		internal WaitHandle NewDataWaitHandle
		{
			get
			{
				return this.dataEvent;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x060014B0 RID: 5296 RVA: 0x0007653C File Offset: 0x0007493C
		public static TaskDistributor Main
		{
			get
			{
				if (TaskDistributor.mainTaskDistributor == null)
				{
					throw new InvalidOperationException("No default TaskDistributor found, please create a new TaskDistributor instance before calling this property.");
				}
				return TaskDistributor.mainTaskDistributor;
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x060014B1 RID: 5297 RVA: 0x00076558 File Offset: 0x00074958
		public static TaskDistributor MainNoThrow
		{
			get
			{
				return TaskDistributor.mainTaskDistributor;
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x060014B2 RID: 5298 RVA: 0x00076560 File Offset: 0x00074960
		public override int TaskCount
		{
			get
			{
				int num = base.TaskCount;
				object obj = this.workerThreads;
				lock (obj)
				{
					for (int i = 0; i < this.workerThreads.Length; i++)
					{
						num += this.workerThreads[i].Dispatcher.TaskCount;
					}
				}
				return num;
			}
		}

		// Token: 0x060014B3 RID: 5299 RVA: 0x000765CC File Offset: 0x000749CC
		public void Start()
		{
			object obj = this.workerThreads;
			lock (obj)
			{
				for (int i = 0; i < this.workerThreads.Length; i++)
				{
					if (!this.workerThreads[i].IsAlive)
					{
						this.workerThreads[i].Start();
					}
				}
			}
		}

		// Token: 0x060014B4 RID: 5300 RVA: 0x0007663C File Offset: 0x00074A3C
		public void SpawnAdditionalWorkerThread()
		{
			object obj = this.workerThreads;
			lock (obj)
			{
				Array.Resize<TaskWorker>(ref this.workerThreads, this.workerThreads.Length + 1);
				this.workerThreads[this.workerThreads.Length - 1] = new TaskWorker(this.name, this);
				this.workerThreads[this.workerThreads.Length - 1].Priority = this.priority;
				this.workerThreads[this.workerThreads.Length - 1].Start();
			}
		}

		// Token: 0x060014B5 RID: 5301 RVA: 0x000766D8 File Offset: 0x00074AD8
		internal void FillTasks(Dispatcher target)
		{
			target.AddTasks(base.IsolateTasks(1));
		}

		// Token: 0x060014B6 RID: 5302 RVA: 0x000766E8 File Offset: 0x00074AE8
		protected override void CheckAccessLimitation()
		{
			if (this.MaxAdditionalWorkerThreads > 0 || !this.AllowAccessLimitationChecks)
			{
				return;
			}
			if (ThreadBase.CurrentThread != null && ThreadBase.CurrentThread is TaskWorker && ((TaskWorker)ThreadBase.CurrentThread).TaskDistributor == this)
			{
				throw new InvalidOperationException("Access to TaskDistributor prohibited when called from inside a TaskDistributor thread. Dont dispatch new Tasks through the same TaskDistributor. If you want to distribute new tasks create a new TaskDistributor and use the new created instance. Remember to dispose the new instance to prevent thread spamming.");
			}
		}

		// Token: 0x060014B7 RID: 5303 RVA: 0x00076748 File Offset: 0x00074B48
		internal override void TasksAdded()
		{
			if (this.MaxAdditionalWorkerThreads > 0)
			{
				IEnumerable<TaskWorker> source = this.workerThreads;
				if (TaskDistributor.<>f__am$cache0 == null)
				{
					TaskDistributor.<>f__am$cache0 = new Func<TaskWorker, bool>(TaskDistributor.<TasksAdded>m__0);
				}
				if (source.All(TaskDistributor.<>f__am$cache0) || this.taskList.Count > this.workerThreads.Length)
				{
					Interlocked.Decrement(ref this.MaxAdditionalWorkerThreads);
					this.SpawnAdditionalWorkerThread();
				}
			}
			base.TasksAdded();
		}

		// Token: 0x060014B8 RID: 5304 RVA: 0x000767C0 File Offset: 0x00074BC0
		public override void Dispose()
		{
			if (this.isDisposed)
			{
				return;
			}
			for (;;)
			{
				object taskListSyncRoot = this.taskListSyncRoot;
				Task task;
				lock (taskListSyncRoot)
				{
					if (this.taskList.Count == 0)
					{
						break;
					}
					task = this.taskList.Dequeue();
				}
				task.Dispose();
			}
			object obj = this.workerThreads;
			lock (obj)
			{
				for (int i = 0; i < this.workerThreads.Length; i++)
				{
					this.workerThreads[i].Dispose();
				}
				this.workerThreads = new TaskWorker[0];
			}
			this.dataEvent.Close();
			this.dataEvent = null;
			if (TaskDistributor.mainTaskDistributor == this)
			{
				TaskDistributor.mainTaskDistributor = null;
			}
			this.isDisposed = true;
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x060014B9 RID: 5305 RVA: 0x000768B4 File Offset: 0x00074CB4
		// (set) Token: 0x060014BA RID: 5306 RVA: 0x000768BC File Offset: 0x00074CBC
		public ThreadPriority Priority
		{
			get
			{
				return this.priority;
			}
			set
			{
				this.priority = value;
				foreach (TaskWorker taskWorker in this.workerThreads)
				{
					taskWorker.Priority = value;
				}
			}
		}

		// Token: 0x060014BB RID: 5307 RVA: 0x000768F6 File Offset: 0x00074CF6
		[CompilerGenerated]
		private static bool <TasksAdded>m__0(TaskWorker worker)
		{
			return worker.Dispatcher.TaskCount > 0 || worker.IsWorking;
		}

		// Token: 0x040011A4 RID: 4516
		private TaskWorker[] workerThreads;

		// Token: 0x040011A5 RID: 4517
		private static TaskDistributor mainTaskDistributor;

		// Token: 0x040011A6 RID: 4518
		public int MaxAdditionalWorkerThreads;

		// Token: 0x040011A7 RID: 4519
		private string name;

		// Token: 0x040011A8 RID: 4520
		private bool isDisposed;

		// Token: 0x040011A9 RID: 4521
		private ThreadPriority priority = ThreadPriority.BelowNormal;

		// Token: 0x040011AA RID: 4522
		[CompilerGenerated]
		private static Func<TaskWorker, bool> <>f__am$cache0;
	}
}
