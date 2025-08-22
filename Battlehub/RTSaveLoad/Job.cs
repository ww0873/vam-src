using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;

namespace Battlehub.RTSaveLoad
{
	// Token: 0x0200024B RID: 587
	public class Job : MonoBehaviour, IJob
	{
		// Token: 0x06000C32 RID: 3122 RVA: 0x0004AB84 File Offset: 0x00048F84
		public Job()
		{
		}

		// Token: 0x06000C33 RID: 3123 RVA: 0x0004AB98 File Offset: 0x00048F98
		public void Submit(Action<Action> job, Action completed)
		{
			Job.JobContainer jobContainer = new Job.JobContainer(job, completed);
			this.m_jobs.Add(jobContainer);
			jobContainer.Run();
		}

		// Token: 0x06000C34 RID: 3124 RVA: 0x0004ABC0 File Offset: 0x00048FC0
		private void Update()
		{
			for (int i = this.m_jobs.Count - 1; i >= 0; i--)
			{
				Job.JobContainer jobContainer = this.m_jobs[i];
				object @lock = jobContainer.Lock;
				lock (@lock)
				{
					if (jobContainer.IsCompleted)
					{
						try
						{
							jobContainer.RaiseCompleted();
						}
						finally
						{
							this.m_jobs.RemoveAt(i);
						}
					}
				}
			}
		}

		// Token: 0x04000CD5 RID: 3285
		private List<Job.JobContainer> m_jobs = new List<Job.JobContainer>();

		// Token: 0x0200024C RID: 588
		public class JobContainer
		{
			// Token: 0x06000C35 RID: 3125 RVA: 0x0004AC50 File Offset: 0x00049050
			public JobContainer(Action<Action> job, Action completed)
			{
				this.m_job = job;
				this.m_completed = completed;
			}

			// Token: 0x06000C36 RID: 3126 RVA: 0x0004AC71 File Offset: 0x00049071
			private void ThreadFunc(object arg)
			{
				this.m_job(new Action(this.<ThreadFunc>m__0));
			}

			// Token: 0x06000C37 RID: 3127 RVA: 0x0004AC8A File Offset: 0x0004908A
			public void Run()
			{
				ThreadPool.QueueUserWorkItem(new WaitCallback(this.ThreadFunc));
			}

			// Token: 0x06000C38 RID: 3128 RVA: 0x0004AC9E File Offset: 0x0004909E
			public void RaiseCompleted()
			{
				this.m_completed();
			}

			// Token: 0x06000C39 RID: 3129 RVA: 0x0004ACAC File Offset: 0x000490AC
			[CompilerGenerated]
			private void <ThreadFunc>m__0()
			{
				object @lock = this.Lock;
				lock (@lock)
				{
					this.IsCompleted = true;
				}
			}

			// Token: 0x04000CD6 RID: 3286
			public object Lock = new object();

			// Token: 0x04000CD7 RID: 3287
			public bool IsCompleted;

			// Token: 0x04000CD8 RID: 3288
			private Action<Action> m_job;

			// Token: 0x04000CD9 RID: 3289
			private Action m_completed;
		}
	}
}
