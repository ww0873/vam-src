using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000553 RID: 1363
	[RequireComponent(typeof(ScrollRect))]
	[AddComponentMenu("UI/Extensions/ScrollRectTweener")]
	public class ScrollRectTweener : MonoBehaviour, IDragHandler, IEventSystemHandler
	{
		// Token: 0x060022A8 RID: 8872 RVA: 0x000C5B2D File Offset: 0x000C3F2D
		public ScrollRectTweener()
		{
		}

		// Token: 0x060022A9 RID: 8873 RVA: 0x000C5B40 File Offset: 0x000C3F40
		private void Awake()
		{
			this.scrollRect = base.GetComponent<ScrollRect>();
			this.wasHorizontal = this.scrollRect.horizontal;
			this.wasVertical = this.scrollRect.vertical;
		}

		// Token: 0x060022AA RID: 8874 RVA: 0x000C5B70 File Offset: 0x000C3F70
		public void ScrollHorizontal(float normalizedX)
		{
			this.Scroll(new Vector2(normalizedX, this.scrollRect.verticalNormalizedPosition));
		}

		// Token: 0x060022AB RID: 8875 RVA: 0x000C5B89 File Offset: 0x000C3F89
		public void ScrollHorizontal(float normalizedX, float duration)
		{
			this.Scroll(new Vector2(normalizedX, this.scrollRect.verticalNormalizedPosition), duration);
		}

		// Token: 0x060022AC RID: 8876 RVA: 0x000C5BA3 File Offset: 0x000C3FA3
		public void ScrollVertical(float normalizedY)
		{
			this.Scroll(new Vector2(this.scrollRect.horizontalNormalizedPosition, normalizedY));
		}

		// Token: 0x060022AD RID: 8877 RVA: 0x000C5BBC File Offset: 0x000C3FBC
		public void ScrollVertical(float normalizedY, float duration)
		{
			this.Scroll(new Vector2(this.scrollRect.horizontalNormalizedPosition, normalizedY), duration);
		}

		// Token: 0x060022AE RID: 8878 RVA: 0x000C5BD6 File Offset: 0x000C3FD6
		public void Scroll(Vector2 normalizedPos)
		{
			this.Scroll(normalizedPos, this.GetScrollDuration(normalizedPos));
		}

		// Token: 0x060022AF RID: 8879 RVA: 0x000C5BE8 File Offset: 0x000C3FE8
		private float GetScrollDuration(Vector2 normalizedPos)
		{
			Vector2 currentPos = this.GetCurrentPos();
			return Vector2.Distance(this.DeNormalize(currentPos), this.DeNormalize(normalizedPos)) / this.moveSpeed;
		}

		// Token: 0x060022B0 RID: 8880 RVA: 0x000C5C18 File Offset: 0x000C4018
		private Vector2 DeNormalize(Vector2 normalizedPos)
		{
			return new Vector2(normalizedPos.x * this.scrollRect.content.rect.width, normalizedPos.y * this.scrollRect.content.rect.height);
		}

		// Token: 0x060022B1 RID: 8881 RVA: 0x000C5C6A File Offset: 0x000C406A
		private Vector2 GetCurrentPos()
		{
			return new Vector2(this.scrollRect.horizontalNormalizedPosition, this.scrollRect.verticalNormalizedPosition);
		}

		// Token: 0x060022B2 RID: 8882 RVA: 0x000C5C87 File Offset: 0x000C4087
		public void Scroll(Vector2 normalizedPos, float duration)
		{
			this.startPos = this.GetCurrentPos();
			this.targetPos = normalizedPos;
			if (this.disableDragWhileTweening)
			{
				this.LockScrollability();
			}
			base.StopAllCoroutines();
			base.StartCoroutine(this.DoMove(duration));
		}

		// Token: 0x060022B3 RID: 8883 RVA: 0x000C5CC4 File Offset: 0x000C40C4
		private IEnumerator DoMove(float duration)
		{
			if (duration < 0.05f)
			{
				yield break;
			}
			Vector2 posOffset = this.targetPos - this.startPos;
			float currentTime = 0f;
			while (currentTime < duration)
			{
				currentTime += Time.deltaTime;
				this.scrollRect.normalizedPosition = this.EaseVector(currentTime, this.startPos, posOffset, duration);
				yield return null;
			}
			this.scrollRect.normalizedPosition = this.targetPos;
			if (this.disableDragWhileTweening)
			{
				this.RestoreScrollability();
			}
			yield break;
		}

		// Token: 0x060022B4 RID: 8884 RVA: 0x000C5CE8 File Offset: 0x000C40E8
		public Vector2 EaseVector(float currentTime, Vector2 startValue, Vector2 changeInValue, float duration)
		{
			return new Vector2(changeInValue.x * Mathf.Sin(currentTime / duration * 1.5707964f) + startValue.x, changeInValue.y * Mathf.Sin(currentTime / duration * 1.5707964f) + startValue.y);
		}

		// Token: 0x060022B5 RID: 8885 RVA: 0x000C5D38 File Offset: 0x000C4138
		public void OnDrag(PointerEventData eventData)
		{
			if (!this.disableDragWhileTweening)
			{
				this.StopScroll();
			}
		}

		// Token: 0x060022B6 RID: 8886 RVA: 0x000C5D4B File Offset: 0x000C414B
		private void StopScroll()
		{
			base.StopAllCoroutines();
			if (this.disableDragWhileTweening)
			{
				this.RestoreScrollability();
			}
		}

		// Token: 0x060022B7 RID: 8887 RVA: 0x000C5D64 File Offset: 0x000C4164
		private void LockScrollability()
		{
			this.scrollRect.horizontal = false;
			this.scrollRect.vertical = false;
		}

		// Token: 0x060022B8 RID: 8888 RVA: 0x000C5D7E File Offset: 0x000C417E
		private void RestoreScrollability()
		{
			this.scrollRect.horizontal = this.wasHorizontal;
			this.scrollRect.vertical = this.wasVertical;
		}

		// Token: 0x04001CA7 RID: 7335
		private ScrollRect scrollRect;

		// Token: 0x04001CA8 RID: 7336
		private Vector2 startPos;

		// Token: 0x04001CA9 RID: 7337
		private Vector2 targetPos;

		// Token: 0x04001CAA RID: 7338
		private bool wasHorizontal;

		// Token: 0x04001CAB RID: 7339
		private bool wasVertical;

		// Token: 0x04001CAC RID: 7340
		public float moveSpeed = 5000f;

		// Token: 0x04001CAD RID: 7341
		public bool disableDragWhileTweening;

		// Token: 0x02000F7A RID: 3962
		[CompilerGenerated]
		private sealed class <DoMove>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x060073F5 RID: 29685 RVA: 0x000C5DA2 File Offset: 0x000C41A2
			[DebuggerHidden]
			public <DoMove>c__Iterator0()
			{
			}

			// Token: 0x060073F6 RID: 29686 RVA: 0x000C5DAC File Offset: 0x000C41AC
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					if (duration < 0.05f)
					{
						return false;
					}
					posOffset = this.targetPos - this.startPos;
					currentTime = 0f;
					break;
				case 1U:
					break;
				default:
					return false;
				}
				if (currentTime < duration)
				{
					currentTime += Time.deltaTime;
					this.scrollRect.normalizedPosition = base.EaseVector(currentTime, this.startPos, posOffset, duration);
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				this.scrollRect.normalizedPosition = this.targetPos;
				if (this.disableDragWhileTweening)
				{
					base.RestoreScrollability();
				}
				this.$PC = -1;
				return false;
			}

			// Token: 0x17001105 RID: 4357
			// (get) Token: 0x060073F7 RID: 29687 RVA: 0x000C5ED6 File Offset: 0x000C42D6
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x17001106 RID: 4358
			// (get) Token: 0x060073F8 RID: 29688 RVA: 0x000C5EDE File Offset: 0x000C42DE
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x060073F9 RID: 29689 RVA: 0x000C5EE6 File Offset: 0x000C42E6
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x060073FA RID: 29690 RVA: 0x000C5EF6 File Offset: 0x000C42F6
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x0400680A RID: 26634
			internal float duration;

			// Token: 0x0400680B RID: 26635
			internal Vector2 <posOffset>__0;

			// Token: 0x0400680C RID: 26636
			internal float <currentTime>__0;

			// Token: 0x0400680D RID: 26637
			internal ScrollRectTweener $this;

			// Token: 0x0400680E RID: 26638
			internal object $current;

			// Token: 0x0400680F RID: 26639
			internal bool $disposing;

			// Token: 0x04006810 RID: 26640
			internal int $PC;
		}
	}
}
