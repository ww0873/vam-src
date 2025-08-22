using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Obi
{
	// Token: 0x020003BA RID: 954
	public class CoroutineJob
	{
		// Token: 0x0600185F RID: 6239 RVA: 0x0008A37F File Offset: 0x0008877F
		public CoroutineJob()
		{
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06001860 RID: 6240 RVA: 0x0008A387 File Offset: 0x00088787
		public object Result
		{
			get
			{
				if (this.e != null)
				{
					throw this.e;
				}
				return this.result;
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06001861 RID: 6241 RVA: 0x0008A3A1 File Offset: 0x000887A1
		public bool IsDone
		{
			get
			{
				return this.isDone;
			}
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06001862 RID: 6242 RVA: 0x0008A3A9 File Offset: 0x000887A9
		public bool RaisedException
		{
			get
			{
				return this.raisedException;
			}
		}

		// Token: 0x06001863 RID: 6243 RVA: 0x0008A3B1 File Offset: 0x000887B1
		private void Init()
		{
			this.isDone = false;
			this.raisedException = false;
			this.stop = false;
			this.result = null;
		}

		// Token: 0x06001864 RID: 6244 RVA: 0x0008A3D0 File Offset: 0x000887D0
		public static object RunSynchronously(IEnumerator coroutine)
		{
			List<object> list = new List<object>();
			if (coroutine == null)
			{
				return list;
			}
			try
			{
				while (coroutine.MoveNext())
				{
					object item = coroutine.Current;
					list.Add(item);
				}
			}
			catch (Exception ex)
			{
				throw ex;
			}
			return list;
		}

		// Token: 0x06001865 RID: 6245 RVA: 0x0008A420 File Offset: 0x00088820
		public IEnumerator Start(IEnumerator coroutine)
		{
			this.Init();
			if (coroutine == null)
			{
				this.isDone = true;
				yield break;
			}
			Stopwatch sw = new Stopwatch();
			sw.Start();
			while (!this.stop)
			{
				try
				{
					if (!coroutine.MoveNext())
					{
						this.isDone = true;
						sw.Stop();
						yield break;
					}
				}
				catch (Exception exception)
				{
					this.e = exception;
					this.raisedException = true;
					UnityEngine.Debug.LogException(exception);
					this.isDone = true;
					sw.Stop();
					yield break;
				}
				this.result = coroutine.Current;
				if (sw.ElapsedMilliseconds > (long)this.asyncThreshold)
				{
					yield return this.result;
				}
			}
			yield break;
		}

		// Token: 0x06001866 RID: 6246 RVA: 0x0008A442 File Offset: 0x00088842
		public void Stop()
		{
			this.stop = true;
		}

		// Token: 0x040013C9 RID: 5065
		private object result;

		// Token: 0x040013CA RID: 5066
		private bool isDone;

		// Token: 0x040013CB RID: 5067
		private bool raisedException;

		// Token: 0x040013CC RID: 5068
		private bool stop;

		// Token: 0x040013CD RID: 5069
		private Exception e;

		// Token: 0x040013CE RID: 5070
		public int asyncThreshold;

		// Token: 0x020003BB RID: 955
		public class ProgressInfo
		{
			// Token: 0x06001867 RID: 6247 RVA: 0x0008A44B File Offset: 0x0008884B
			public ProgressInfo(string userReadableInfo, float progress)
			{
				this.userReadableInfo = userReadableInfo;
				this.progress = progress;
			}

			// Token: 0x040013CF RID: 5071
			public string userReadableInfo;

			// Token: 0x040013D0 RID: 5072
			public float progress;
		}

		// Token: 0x02000F45 RID: 3909
		[CompilerGenerated]
		private sealed class <Start>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x0600735E RID: 29534 RVA: 0x0008A461 File Offset: 0x00088861
			[DebuggerHidden]
			public <Start>c__Iterator0()
			{
			}

			// Token: 0x0600735F RID: 29535 RVA: 0x0008A46C File Offset: 0x0008886C
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					base.Init();
					if (coroutine == null)
					{
						this.isDone = true;
						return false;
					}
					sw = new Stopwatch();
					sw.Start();
					break;
				case 1U:
					break;
				default:
					return false;
				}
				while (!this.stop)
				{
					try
					{
						if (!coroutine.MoveNext())
						{
							this.isDone = true;
							sw.Stop();
							return false;
						}
					}
					catch (Exception ex)
					{
						this.e = ex;
						this.raisedException = true;
						UnityEngine.Debug.LogException(ex);
						this.isDone = true;
						sw.Stop();
						return false;
					}
					this.result = coroutine.Current;
					if (sw.ElapsedMilliseconds > (long)this.asyncThreshold)
					{
						this.$current = this.result;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
				}
				this.$PC = -1;
				return false;
			}

			// Token: 0x170010E5 RID: 4325
			// (get) Token: 0x06007360 RID: 29536 RVA: 0x0008A5CC File Offset: 0x000889CC
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x170010E6 RID: 4326
			// (get) Token: 0x06007361 RID: 29537 RVA: 0x0008A5D4 File Offset: 0x000889D4
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007362 RID: 29538 RVA: 0x0008A5DC File Offset: 0x000889DC
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007363 RID: 29539 RVA: 0x0008A5EC File Offset: 0x000889EC
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x04006755 RID: 26453
			internal IEnumerator coroutine;

			// Token: 0x04006756 RID: 26454
			internal Stopwatch <sw>__0;

			// Token: 0x04006757 RID: 26455
			internal CoroutineJob $this;

			// Token: 0x04006758 RID: 26456
			internal object $current;

			// Token: 0x04006759 RID: 26457
			internal bool $disposing;

			// Token: 0x0400675A RID: 26458
			internal int $PC;
		}
	}
}
