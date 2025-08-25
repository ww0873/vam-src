using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace UnityEngine.UI.Extensions.Tweens
{
	// Token: 0x020004AA RID: 1194
	internal class TweenRunner<T> where T : struct, ITweenValue
	{
		// Token: 0x06001E1A RID: 7706 RVA: 0x000AC146 File Offset: 0x000AA546
		public TweenRunner()
		{
		}

		// Token: 0x06001E1B RID: 7707 RVA: 0x000AC150 File Offset: 0x000AA550
		private static IEnumerator Start(T tweenInfo)
		{
			if (!tweenInfo.ValidTarget())
			{
				yield break;
			}
			float elapsedTime = 0f;
			while (elapsedTime < tweenInfo.duration)
			{
				elapsedTime += ((!tweenInfo.ignoreTimeScale) ? Time.deltaTime : Time.unscaledDeltaTime);
				float percentage = Mathf.Clamp01(elapsedTime / tweenInfo.duration);
				tweenInfo.TweenValue(percentage);
				yield return null;
			}
			tweenInfo.TweenValue(1f);
			tweenInfo.Finished();
			yield break;
		}

		// Token: 0x06001E1C RID: 7708 RVA: 0x000AC16B File Offset: 0x000AA56B
		public void Init(MonoBehaviour coroutineContainer)
		{
			this.m_CoroutineContainer = coroutineContainer;
		}

		// Token: 0x06001E1D RID: 7709 RVA: 0x000AC174 File Offset: 0x000AA574
		public void StartTween(T info)
		{
			if (this.m_CoroutineContainer == null)
			{
				Debug.LogWarning("Coroutine container not configured... did you forget to call Init?");
				return;
			}
			if (this.m_Tween != null)
			{
				this.m_CoroutineContainer.StopCoroutine(this.m_Tween);
				this.m_Tween = null;
			}
			if (!this.m_CoroutineContainer.gameObject.activeInHierarchy)
			{
				info.TweenValue(1f);
				return;
			}
			this.m_Tween = TweenRunner<T>.Start(info);
			this.m_CoroutineContainer.StartCoroutine(this.m_Tween);
		}

		// Token: 0x04001976 RID: 6518
		protected MonoBehaviour m_CoroutineContainer;

		// Token: 0x04001977 RID: 6519
		protected IEnumerator m_Tween;

		// Token: 0x02000F6E RID: 3950
		[CompilerGenerated]
		private sealed class <Start>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x060073D4 RID: 29652 RVA: 0x000AC206 File Offset: 0x000AA606
			[DebuggerHidden]
			public <Start>c__Iterator0()
			{
			}

			// Token: 0x060073D5 RID: 29653 RVA: 0x000AC210 File Offset: 0x000AA610
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					if (!tweenInfo.ValidTarget())
					{
						return false;
					}
					elapsedTime = 0f;
					break;
				case 1U:
					break;
				default:
					return false;
				}
				if (elapsedTime < tweenInfo.duration)
				{
					elapsedTime += ((!tweenInfo.ignoreTimeScale) ? Time.deltaTime : Time.unscaledDeltaTime);
					percentage = Mathf.Clamp01(elapsedTime / tweenInfo.duration);
					tweenInfo.TweenValue(percentage);
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				tweenInfo.TweenValue(1f);
				tweenInfo.Finished();
				this.$PC = -1;
				return false;
			}

			// Token: 0x17001101 RID: 4353
			// (get) Token: 0x060073D6 RID: 29654 RVA: 0x000AC33D File Offset: 0x000AA73D
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x17001102 RID: 4354
			// (get) Token: 0x060073D7 RID: 29655 RVA: 0x000AC345 File Offset: 0x000AA745
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x060073D8 RID: 29656 RVA: 0x000AC34D File Offset: 0x000AA74D
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x060073D9 RID: 29657 RVA: 0x000AC35D File Offset: 0x000AA75D
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x040067F1 RID: 26609
			internal T tweenInfo;

			// Token: 0x040067F2 RID: 26610
			internal float <elapsedTime>__0;

			// Token: 0x040067F3 RID: 26611
			internal float <percentage>__1;

			// Token: 0x040067F4 RID: 26612
			internal object $current;

			// Token: 0x040067F5 RID: 26613
			internal bool $disposing;

			// Token: 0x040067F6 RID: 26614
			internal int $PC;
		}
	}
}
