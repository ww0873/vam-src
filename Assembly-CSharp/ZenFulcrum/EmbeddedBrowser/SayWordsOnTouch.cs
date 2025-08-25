using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	// Token: 0x020005A6 RID: 1446
	public class SayWordsOnTouch : MonoBehaviour
	{
		// Token: 0x0600244C RID: 9292 RVA: 0x000D1AF9 File Offset: 0x000CFEF9
		public SayWordsOnTouch()
		{
		}

		// Token: 0x1700044B RID: 1099
		// (get) Token: 0x0600244D RID: 9293 RVA: 0x000D1B01 File Offset: 0x000CFF01
		// (set) Token: 0x0600244E RID: 9294 RVA: 0x000D1B08 File Offset: 0x000CFF08
		public static int ActiveSpeakers
		{
			[CompilerGenerated]
			get
			{
				return SayWordsOnTouch.<ActiveSpeakers>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				SayWordsOnTouch.<ActiveSpeakers>k__BackingField = value;
			}
		}

		// Token: 0x0600244F RID: 9295 RVA: 0x000D1B10 File Offset: 0x000CFF10
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
			this.stillTriggered = true;
			SayWordsOnTouch.ActiveSpeakers++;
			base.StartCoroutine(this.SayStuff());
			BoxCollider component2 = base.GetComponent<BoxCollider>();
			if (component2)
			{
				Vector3 size = component2.size;
				size.x += this.extraLeaveRange * 2f;
				size.y += this.extraLeaveRange * 2f;
				size.z += this.extraLeaveRange * 2f;
				component2.size = size;
			}
		}

		// Token: 0x06002450 RID: 9296 RVA: 0x000D1BD4 File Offset: 0x000CFFD4
		private IEnumerator SayStuff()
		{
			int idx = 0;
			while (idx < this.thingsToSay.Length && this.stillTriggered)
			{
				yield return new WaitForSeconds(this.thingsToSay[idx].delay);
				if (!this.stillTriggered)
				{
					break;
				}
				HUDManager.Instance.Say(this.thingsToSay[idx].textHTML, this.thingsToSay[idx].dwellTime);
				idx++;
			}
			SayWordsOnTouch.ActiveSpeakers--;
			UnityEngine.Object.Destroy(base.gameObject);
			yield break;
		}

		// Token: 0x06002451 RID: 9297 RVA: 0x000D1BF0 File Offset: 0x000CFFF0
		public void OnTriggerExit(Collider other)
		{
			PlayerInventory component = other.GetComponent<PlayerInventory>();
			if (!component)
			{
				return;
			}
			this.stillTriggered = false;
		}

		// Token: 0x04001E84 RID: 7812
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private static int <ActiveSpeakers>k__BackingField;

		// Token: 0x04001E85 RID: 7813
		public SayWordsOnTouch.Verse[] thingsToSay;

		// Token: 0x04001E86 RID: 7814
		private bool triggered;

		// Token: 0x04001E87 RID: 7815
		private bool stillTriggered;

		// Token: 0x04001E88 RID: 7816
		public float extraLeaveRange;

		// Token: 0x020005A7 RID: 1447
		[Serializable]
		public class Verse
		{
			// Token: 0x06002452 RID: 9298 RVA: 0x000D1C17 File Offset: 0x000D0017
			public Verse()
			{
			}

			// Token: 0x04001E89 RID: 7817
			public float delay;

			// Token: 0x04001E8A RID: 7818
			[Multiline]
			public string textHTML;

			// Token: 0x04001E8B RID: 7819
			public float dwellTime = 5f;
		}

		// Token: 0x02000F87 RID: 3975
		[CompilerGenerated]
		private sealed class <SayStuff>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007439 RID: 29753 RVA: 0x000D1C2A File Offset: 0x000D002A
			[DebuggerHidden]
			public <SayStuff>c__Iterator0()
			{
			}

			// Token: 0x0600743A RID: 29754 RVA: 0x000D1C34 File Offset: 0x000D0034
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
					if (!this.stillTriggered)
					{
						goto IL_E6;
					}
					HUDManager.Instance.Say(this.thingsToSay[idx].textHTML, this.thingsToSay[idx].dwellTime);
					idx++;
					break;
				default:
					return false;
				}
				if (idx < this.thingsToSay.Length && this.stillTriggered)
				{
					this.$current = new WaitForSeconds(this.thingsToSay[idx].delay);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				IL_E6:
				SayWordsOnTouch.ActiveSpeakers--;
				UnityEngine.Object.Destroy(base.gameObject);
				this.$PC = -1;
				return false;
			}

			// Token: 0x17001119 RID: 4377
			// (get) Token: 0x0600743B RID: 29755 RVA: 0x000D1D4D File Offset: 0x000D014D
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x1700111A RID: 4378
			// (get) Token: 0x0600743C RID: 29756 RVA: 0x000D1D55 File Offset: 0x000D0155
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x0600743D RID: 29757 RVA: 0x000D1D5D File Offset: 0x000D015D
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x0600743E RID: 29758 RVA: 0x000D1D6D File Offset: 0x000D016D
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x0400684B RID: 26699
			internal int <idx>__1;

			// Token: 0x0400684C RID: 26700
			internal SayWordsOnTouch $this;

			// Token: 0x0400684D RID: 26701
			internal object $current;

			// Token: 0x0400684E RID: 26702
			internal bool $disposing;

			// Token: 0x0400684F RID: 26703
			internal int $PC;
		}
	}
}
