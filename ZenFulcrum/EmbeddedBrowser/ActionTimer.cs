using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace ZenFulcrum.EmbeddedBrowser
{
	// Token: 0x0200059B RID: 1435
	public class ActionTimer : MonoBehaviour
	{
		// Token: 0x0600240F RID: 9231 RVA: 0x000D0B4D File Offset: 0x000CEF4D
		public ActionTimer()
		{
		}

		// Token: 0x06002410 RID: 9232 RVA: 0x000D0B58 File Offset: 0x000CEF58
		public void OnTriggerEnter(Collider other)
		{
			if (this.triggered)
			{
				return;
			}
			PlayerInventory component = other.GetComponent<PlayerInventory>();
			if (!component)
			{
				return;
			}
			this.triggered = true;
			base.StartCoroutine(this.DoThings());
		}

		// Token: 0x06002411 RID: 9233 RVA: 0x000D0B98 File Offset: 0x000CEF98
		private IEnumerator DoThings()
		{
			for (int idx = 0; idx < this.thingsToDo.Length; idx++)
			{
				yield return new WaitForSeconds(this.thingsToDo[idx].delay);
				this.thingsToDo[idx].action.Invoke();
			}
			yield break;
		}

		// Token: 0x04001E5E RID: 7774
		public ActionTimer.TimedAction[] thingsToDo;

		// Token: 0x04001E5F RID: 7775
		private bool triggered;

		// Token: 0x0200059C RID: 1436
		[Serializable]
		public class TimedAction
		{
			// Token: 0x06002412 RID: 9234 RVA: 0x000D0BB3 File Offset: 0x000CEFB3
			public TimedAction()
			{
			}

			// Token: 0x04001E60 RID: 7776
			public float delay;

			// Token: 0x04001E61 RID: 7777
			public UnityEvent action;
		}

		// Token: 0x02000F81 RID: 3969
		[CompilerGenerated]
		private sealed class <DoThings>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007419 RID: 29721 RVA: 0x000D0BBB File Offset: 0x000CEFBB
			[DebuggerHidden]
			public <DoThings>c__Iterator0()
			{
			}

			// Token: 0x0600741A RID: 29722 RVA: 0x000D0BC4 File Offset: 0x000CEFC4
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					idx = 0;
					break;
				case 1U:
					this.thingsToDo[idx].action.Invoke();
					idx++;
					break;
				default:
					return false;
				}
				if (idx < this.thingsToDo.Length)
				{
					this.$current = new WaitForSeconds(this.thingsToDo[idx].delay);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				this.$PC = -1;
				return false;
			}

			// Token: 0x17001111 RID: 4369
			// (get) Token: 0x0600741B RID: 29723 RVA: 0x000D0C80 File Offset: 0x000CF080
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x17001112 RID: 4370
			// (get) Token: 0x0600741C RID: 29724 RVA: 0x000D0C88 File Offset: 0x000CF088
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x0600741D RID: 29725 RVA: 0x000D0C90 File Offset: 0x000CF090
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x0600741E RID: 29726 RVA: 0x000D0CA0 File Offset: 0x000CF0A0
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x04006833 RID: 26675
			internal int <idx>__1;

			// Token: 0x04006834 RID: 26676
			internal ActionTimer $this;

			// Token: 0x04006835 RID: 26677
			internal object $current;

			// Token: 0x04006836 RID: 26678
			internal bool $disposing;

			// Token: 0x04006837 RID: 26679
			internal int $PC;
		}
	}
}
