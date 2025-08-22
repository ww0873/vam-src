using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace VikingCrewTools.UI
{
	// Token: 0x02000569 RID: 1385
	public class ScriptedDialogueBehaviour : MonoBehaviour
	{
		// Token: 0x06002328 RID: 9000 RVA: 0x000C858F File Offset: 0x000C698F
		public ScriptedDialogueBehaviour()
		{
		}

		// Token: 0x06002329 RID: 9001 RVA: 0x000C85A9 File Offset: 0x000C69A9
		private void Start()
		{
			base.StartCoroutine(this.FollowScript());
		}

		// Token: 0x0600232A RID: 9002 RVA: 0x000C85B8 File Offset: 0x000C69B8
		private IEnumerator FollowScript()
		{
			int index = 0;
			while (index < this.script.Length)
			{
				yield return new WaitForSeconds(this.script[index].delay);
				SpeechBubbleManager.Instance.AddSpeechBubble(this.script[index].speaker, this.script[index].line, this.script[index].speechBubbleType, this.bubbleTimeToLive, Color.white, Vector3.zero);
				index++;
				if (this.doRestartAtEnd && index == this.script.Length)
				{
					index = 0;
				}
			}
			yield break;
		}

		// Token: 0x04001D1C RID: 7452
		public ScriptedDialogueBehaviour.DialogueLine[] script;

		// Token: 0x04001D1D RID: 7453
		public bool doRestartAtEnd = true;

		// Token: 0x04001D1E RID: 7454
		public float bubbleTimeToLive = 3f;

		// Token: 0x0200056A RID: 1386
		[Serializable]
		public class DialogueLine
		{
			// Token: 0x0600232B RID: 9003 RVA: 0x000C85D3 File Offset: 0x000C69D3
			public DialogueLine()
			{
			}

			// Token: 0x04001D1F RID: 7455
			[Tooltip("The transform doing the speaking. This could be a mouth, head or character transform depending on your scene")]
			public Transform speaker;

			// Token: 0x04001D20 RID: 7456
			[Tooltip("Time to delay from the previous message in the array")]
			public float delay = 2f;

			// Token: 0x04001D21 RID: 7457
			[Multiline]
			[Tooltip("What to say")]
			public string line = "Hello World!";

			// Token: 0x04001D22 RID: 7458
			[Tooltip("How to say it")]
			public SpeechBubbleManager.SpeechbubbleType speechBubbleType;
		}

		// Token: 0x02000F7E RID: 3966
		[CompilerGenerated]
		private sealed class <FollowScript>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x0600740D RID: 29709 RVA: 0x000C85F1 File Offset: 0x000C69F1
			[DebuggerHidden]
			public <FollowScript>c__Iterator0()
			{
			}

			// Token: 0x0600740E RID: 29710 RVA: 0x000C85FC File Offset: 0x000C69FC
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					index = 0;
					break;
				case 1U:
					SpeechBubbleManager.Instance.AddSpeechBubble(this.script[index].speaker, this.script[index].line, this.script[index].speechBubbleType, this.bubbleTimeToLive, Color.white, Vector3.zero);
					index++;
					if (this.doRestartAtEnd && index == this.script.Length)
					{
						index = 0;
					}
					break;
				default:
					return false;
				}
				if (index < this.script.Length)
				{
					this.$current = new WaitForSeconds(this.script[index].delay);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				this.$PC = -1;
				return false;
			}

			// Token: 0x1700110D RID: 4365
			// (get) Token: 0x0600740F RID: 29711 RVA: 0x000C8730 File Offset: 0x000C6B30
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x1700110E RID: 4366
			// (get) Token: 0x06007410 RID: 29712 RVA: 0x000C8738 File Offset: 0x000C6B38
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007411 RID: 29713 RVA: 0x000C8740 File Offset: 0x000C6B40
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007412 RID: 29714 RVA: 0x000C8750 File Offset: 0x000C6B50
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x04006823 RID: 26659
			internal int <index>__0;

			// Token: 0x04006824 RID: 26660
			internal ScriptedDialogueBehaviour $this;

			// Token: 0x04006825 RID: 26661
			internal object $current;

			// Token: 0x04006826 RID: 26662
			internal bool $disposing;

			// Token: 0x04006827 RID: 26663
			internal int $PC;
		}
	}
}
