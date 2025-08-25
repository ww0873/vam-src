using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace UnityThreading
{
	// Token: 0x02000352 RID: 850
	internal sealed class TaskWorker : ThreadBase
	{
		// Token: 0x060014BC RID: 5308 RVA: 0x00076CFB File Offset: 0x000750FB
		public TaskWorker(string name, TaskDistributor taskDistributor) : base(name, false)
		{
			this.TaskDistributor = taskDistributor;
			this.Dispatcher = new Dispatcher(false);
		}

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x060014BD RID: 5309 RVA: 0x00076D18 File Offset: 0x00075118
		// (set) Token: 0x060014BE RID: 5310 RVA: 0x00076D20 File Offset: 0x00075120
		public TaskDistributor TaskDistributor
		{
			[CompilerGenerated]
			get
			{
				return this.<TaskDistributor>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<TaskDistributor>k__BackingField = value;
			}
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x060014BF RID: 5311 RVA: 0x00076D29 File Offset: 0x00075129
		public bool IsWorking
		{
			get
			{
				return this.Dispatcher.IsWorking;
			}
		}

		// Token: 0x060014C0 RID: 5312 RVA: 0x00076D38 File Offset: 0x00075138
		protected override IEnumerator Do()
		{
			while (!this.exitEvent.InterWaitOne(0))
			{
				if (!this.Dispatcher.ProcessNextTask())
				{
					this.TaskDistributor.FillTasks(this.Dispatcher);
					if (this.Dispatcher.TaskCount == 0)
					{
						if (WaitHandle.WaitAny(new WaitHandle[]
						{
							this.exitEvent,
							this.TaskDistributor.NewDataWaitHandle
						}) == 0)
						{
							return null;
						}
						this.TaskDistributor.FillTasks(this.Dispatcher);
					}
				}
			}
			return null;
		}

		// Token: 0x060014C1 RID: 5313 RVA: 0x00076DC9 File Offset: 0x000751C9
		public override void Dispose()
		{
			base.Dispose();
			if (this.Dispatcher != null)
			{
				this.Dispatcher.Dispose();
			}
			this.Dispatcher = null;
		}

		// Token: 0x040011AB RID: 4523
		public Dispatcher Dispatcher;

		// Token: 0x040011AC RID: 4524
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private TaskDistributor <TaskDistributor>k__BackingField;
	}
}
