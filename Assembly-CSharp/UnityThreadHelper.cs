using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityThreading;

// Token: 0x02000358 RID: 856
[ExecuteInEditMode]
public class UnityThreadHelper : MonoBehaviour
{
	// Token: 0x0600150D RID: 5389 RVA: 0x00077E93 File Offset: 0x00076293
	public UnityThreadHelper()
	{
	}

	// Token: 0x0600150E RID: 5390 RVA: 0x00077EA8 File Offset: 0x000762A8
	public static void EnsureHelper()
	{
		object obj = UnityThreadHelper.syncRoot;
		lock (obj)
		{
			if (UnityThreadHelper.instance == null)
			{
				UnityThreadHelper.instance = (UnityEngine.Object.FindObjectOfType(typeof(UnityThreadHelper)) as UnityThreadHelper);
				if (UnityThreadHelper.instance == null)
				{
					UnityThreadHelper.instance = new GameObject("[UnityThreadHelper]")
					{
						hideFlags = (HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.NotEditable)
					}.AddComponent<UnityThreadHelper>();
					UnityThreadHelper.instance.EnsureHelperInstance();
				}
			}
		}
	}

	// Token: 0x17000250 RID: 592
	// (get) Token: 0x0600150F RID: 5391 RVA: 0x00077F34 File Offset: 0x00076334
	private static UnityThreadHelper Instance
	{
		get
		{
			UnityThreadHelper.EnsureHelper();
			return UnityThreadHelper.instance;
		}
	}

	// Token: 0x17000251 RID: 593
	// (get) Token: 0x06001510 RID: 5392 RVA: 0x00077F40 File Offset: 0x00076340
	public static Dispatcher Dispatcher
	{
		get
		{
			return UnityThreadHelper.Instance.CurrentDispatcher;
		}
	}

	// Token: 0x17000252 RID: 594
	// (get) Token: 0x06001511 RID: 5393 RVA: 0x00077F4C File Offset: 0x0007634C
	public static TaskDistributor TaskDistributor
	{
		get
		{
			return UnityThreadHelper.Instance.CurrentTaskDistributor;
		}
	}

	// Token: 0x17000253 RID: 595
	// (get) Token: 0x06001512 RID: 5394 RVA: 0x00077F58 File Offset: 0x00076358
	public Dispatcher CurrentDispatcher
	{
		get
		{
			return this.dispatcher;
		}
	}

	// Token: 0x17000254 RID: 596
	// (get) Token: 0x06001513 RID: 5395 RVA: 0x00077F60 File Offset: 0x00076360
	public TaskDistributor CurrentTaskDistributor
	{
		get
		{
			return this.taskDistributor;
		}
	}

	// Token: 0x06001514 RID: 5396 RVA: 0x00077F68 File Offset: 0x00076368
	private void EnsureHelperInstance()
	{
		this.dispatcher = (Dispatcher.MainNoThrow ?? new Dispatcher());
		this.taskDistributor = (TaskDistributor.MainNoThrow ?? new TaskDistributor("TaskDistributor"));
	}

	// Token: 0x06001515 RID: 5397 RVA: 0x00077FA0 File Offset: 0x000763A0
	public static ActionThread CreateThread(Action<ActionThread> action, bool autoStartThread)
	{
		UnityThreadHelper.<CreateThread>c__AnonStorey0 <CreateThread>c__AnonStorey = new UnityThreadHelper.<CreateThread>c__AnonStorey0();
		<CreateThread>c__AnonStorey.action = action;
		UnityThreadHelper.Instance.EnsureHelperInstance();
		Action<ActionThread> action2 = new Action<ActionThread>(<CreateThread>c__AnonStorey.<>m__0);
		ActionThread actionThread = new ActionThread(action2, autoStartThread);
		UnityThreadHelper.Instance.RegisterThread(actionThread);
		return actionThread;
	}

	// Token: 0x06001516 RID: 5398 RVA: 0x00077FE5 File Offset: 0x000763E5
	public static ActionThread CreateThread(Action<ActionThread> action)
	{
		return UnityThreadHelper.CreateThread(action, true);
	}

	// Token: 0x06001517 RID: 5399 RVA: 0x00077FF0 File Offset: 0x000763F0
	public static ActionThread CreateThread(Action action, bool autoStartThread)
	{
		UnityThreadHelper.<CreateThread>c__AnonStorey1 <CreateThread>c__AnonStorey = new UnityThreadHelper.<CreateThread>c__AnonStorey1();
		<CreateThread>c__AnonStorey.action = action;
		return UnityThreadHelper.CreateThread(new Action<ActionThread>(<CreateThread>c__AnonStorey.<>m__0), autoStartThread);
	}

	// Token: 0x06001518 RID: 5400 RVA: 0x0007801C File Offset: 0x0007641C
	public static ActionThread CreateThread(Action action)
	{
		UnityThreadHelper.<CreateThread>c__AnonStorey2 <CreateThread>c__AnonStorey = new UnityThreadHelper.<CreateThread>c__AnonStorey2();
		<CreateThread>c__AnonStorey.action = action;
		return UnityThreadHelper.CreateThread(new Action<ActionThread>(<CreateThread>c__AnonStorey.<>m__0), true);
	}

	// Token: 0x06001519 RID: 5401 RVA: 0x00078048 File Offset: 0x00076448
	public static ThreadBase CreateThread(Func<ThreadBase, IEnumerator> action, bool autoStartThread)
	{
		UnityThreadHelper.Instance.EnsureHelperInstance();
		EnumeratableActionThread enumeratableActionThread = new EnumeratableActionThread(action, autoStartThread);
		UnityThreadHelper.Instance.RegisterThread(enumeratableActionThread);
		return enumeratableActionThread;
	}

	// Token: 0x0600151A RID: 5402 RVA: 0x00078073 File Offset: 0x00076473
	public static ThreadBase CreateThread(Func<ThreadBase, IEnumerator> action)
	{
		return UnityThreadHelper.CreateThread(action, true);
	}

	// Token: 0x0600151B RID: 5403 RVA: 0x0007807C File Offset: 0x0007647C
	public static ThreadBase CreateThread(Func<IEnumerator> action, bool autoStartThread)
	{
		UnityThreadHelper.<CreateThread>c__AnonStorey3 <CreateThread>c__AnonStorey = new UnityThreadHelper.<CreateThread>c__AnonStorey3();
		<CreateThread>c__AnonStorey.action = action;
		Func<ThreadBase, IEnumerator> action2 = new Func<ThreadBase, IEnumerator>(<CreateThread>c__AnonStorey.<>m__0);
		return UnityThreadHelper.CreateThread(action2, autoStartThread);
	}

	// Token: 0x0600151C RID: 5404 RVA: 0x000780AC File Offset: 0x000764AC
	public static ThreadBase CreateThread(Func<IEnumerator> action)
	{
		UnityThreadHelper.<CreateThread>c__AnonStorey4 <CreateThread>c__AnonStorey = new UnityThreadHelper.<CreateThread>c__AnonStorey4();
		<CreateThread>c__AnonStorey.action = action;
		Func<ThreadBase, IEnumerator> action2 = new Func<ThreadBase, IEnumerator>(<CreateThread>c__AnonStorey.<>m__0);
		return UnityThreadHelper.CreateThread(action2, true);
	}

	// Token: 0x0600151D RID: 5405 RVA: 0x000780DA File Offset: 0x000764DA
	private void RegisterThread(ThreadBase thread)
	{
		if (this.registeredThreads.Contains(thread))
		{
			return;
		}
		this.registeredThreads.Add(thread);
	}

	// Token: 0x0600151E RID: 5406 RVA: 0x000780FC File Offset: 0x000764FC
	private void OnDestroy()
	{
		foreach (ThreadBase threadBase in this.registeredThreads)
		{
			threadBase.Dispose();
		}
		if (this.dispatcher != null)
		{
			this.dispatcher.Dispose();
		}
		this.dispatcher = null;
		if (this.taskDistributor != null)
		{
			this.taskDistributor.Dispose();
		}
		this.taskDistributor = null;
		if (UnityThreadHelper.instance == this)
		{
			UnityThreadHelper.instance = null;
		}
	}

	// Token: 0x0600151F RID: 5407 RVA: 0x000781A8 File Offset: 0x000765A8
	private void Update()
	{
		if (this.dispatcher != null)
		{
			this.dispatcher.ProcessTasks();
		}
		IEnumerable<ThreadBase> source = this.registeredThreads;
		if (UnityThreadHelper.<>f__am$cache0 == null)
		{
			UnityThreadHelper.<>f__am$cache0 = new Func<ThreadBase, bool>(UnityThreadHelper.<Update>m__0);
		}
		ThreadBase[] array = source.Where(UnityThreadHelper.<>f__am$cache0).ToArray<ThreadBase>();
		foreach (ThreadBase threadBase in array)
		{
			threadBase.Dispose();
			this.registeredThreads.Remove(threadBase);
		}
	}

	// Token: 0x06001520 RID: 5408 RVA: 0x00078226 File Offset: 0x00076626
	// Note: this type is marked as 'beforefieldinit'.
	static UnityThreadHelper()
	{
	}

	// Token: 0x06001521 RID: 5409 RVA: 0x00078238 File Offset: 0x00076638
	[CompilerGenerated]
	private static bool <Update>m__0(ThreadBase thread)
	{
		return !thread.IsAlive;
	}

	// Token: 0x040011B8 RID: 4536
	private static UnityThreadHelper instance = null;

	// Token: 0x040011B9 RID: 4537
	private static object syncRoot = new object();

	// Token: 0x040011BA RID: 4538
	private Dispatcher dispatcher;

	// Token: 0x040011BB RID: 4539
	private TaskDistributor taskDistributor;

	// Token: 0x040011BC RID: 4540
	private List<ThreadBase> registeredThreads = new List<ThreadBase>();

	// Token: 0x040011BD RID: 4541
	[CompilerGenerated]
	private static Func<ThreadBase, bool> <>f__am$cache0;

	// Token: 0x02000F2A RID: 3882
	[CompilerGenerated]
	private sealed class <CreateThread>c__AnonStorey0
	{
		// Token: 0x060072EE RID: 29422 RVA: 0x00078243 File Offset: 0x00076643
		public <CreateThread>c__AnonStorey0()
		{
		}

		// Token: 0x060072EF RID: 29423 RVA: 0x0007824C File Offset: 0x0007664C
		internal void <>m__0(ActionThread currentThread)
		{
			try
			{
				this.action(currentThread);
			}
			catch (Exception message)
			{
				Debug.LogError(message);
			}
		}

		// Token: 0x040066C9 RID: 26313
		internal Action<ActionThread> action;
	}

	// Token: 0x02000F2B RID: 3883
	[CompilerGenerated]
	private sealed class <CreateThread>c__AnonStorey1
	{
		// Token: 0x060072F0 RID: 29424 RVA: 0x00078288 File Offset: 0x00076688
		public <CreateThread>c__AnonStorey1()
		{
		}

		// Token: 0x060072F1 RID: 29425 RVA: 0x00078290 File Offset: 0x00076690
		internal void <>m__0(ActionThread thread)
		{
			this.action();
		}

		// Token: 0x040066CA RID: 26314
		internal Action action;
	}

	// Token: 0x02000F2C RID: 3884
	[CompilerGenerated]
	private sealed class <CreateThread>c__AnonStorey2
	{
		// Token: 0x060072F2 RID: 29426 RVA: 0x0007829D File Offset: 0x0007669D
		public <CreateThread>c__AnonStorey2()
		{
		}

		// Token: 0x060072F3 RID: 29427 RVA: 0x000782A5 File Offset: 0x000766A5
		internal void <>m__0(ActionThread thread)
		{
			this.action();
		}

		// Token: 0x040066CB RID: 26315
		internal Action action;
	}

	// Token: 0x02000F2D RID: 3885
	[CompilerGenerated]
	private sealed class <CreateThread>c__AnonStorey3
	{
		// Token: 0x060072F4 RID: 29428 RVA: 0x000782B2 File Offset: 0x000766B2
		public <CreateThread>c__AnonStorey3()
		{
		}

		// Token: 0x060072F5 RID: 29429 RVA: 0x000782BA File Offset: 0x000766BA
		internal IEnumerator <>m__0(ThreadBase thread)
		{
			return this.action();
		}

		// Token: 0x040066CC RID: 26316
		internal Func<IEnumerator> action;
	}

	// Token: 0x02000F2E RID: 3886
	[CompilerGenerated]
	private sealed class <CreateThread>c__AnonStorey4
	{
		// Token: 0x060072F6 RID: 29430 RVA: 0x000782C7 File Offset: 0x000766C7
		public <CreateThread>c__AnonStorey4()
		{
		}

		// Token: 0x060072F7 RID: 29431 RVA: 0x000782CF File Offset: 0x000766CF
		internal IEnumerator <>m__0(ThreadBase thread)
		{
			return this.action();
		}

		// Token: 0x040066CD RID: 26317
		internal Func<IEnumerator> action;
	}
}
