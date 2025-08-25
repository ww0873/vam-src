using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200055A RID: 1370
	[AddComponentMenu("UI/Extensions/UI Tween Scale")]
	public class UI_TweenScale : MonoBehaviour
	{
		// Token: 0x060022D2 RID: 8914 RVA: 0x000C70A9 File Offset: 0x000C54A9
		public UI_TweenScale()
		{
		}

		// Token: 0x060022D3 RID: 8915 RVA: 0x000C70CE File Offset: 0x000C54CE
		private void Awake()
		{
			this.myTransform = base.GetComponent<Transform>();
			this.initScale = this.myTransform.localScale;
			if (this.playAtAwake)
			{
				this.Play();
			}
		}

		// Token: 0x060022D4 RID: 8916 RVA: 0x000C70FE File Offset: 0x000C54FE
		public void Play()
		{
			base.StartCoroutine("Tween");
		}

		// Token: 0x060022D5 RID: 8917 RVA: 0x000C710C File Offset: 0x000C550C
		private IEnumerator Tween()
		{
			this.myTransform.localScale = this.initScale;
			float t = 0f;
			float maxT = this.animCurve.keys[this.animCurve.length - 1].time;
			while (t < maxT || this.isLoop)
			{
				t += this.speed * Time.deltaTime;
				if (!this.isUniform)
				{
					this.newScale.x = 1f * this.animCurve.Evaluate(t);
					this.newScale.y = 1f * this.animCurveY.Evaluate(t);
					this.myTransform.localScale = this.newScale;
				}
				else
				{
					this.myTransform.localScale = Vector3.one * this.animCurve.Evaluate(t);
				}
				yield return null;
			}
			yield break;
		}

		// Token: 0x060022D6 RID: 8918 RVA: 0x000C7127 File Offset: 0x000C5527
		public void ResetTween()
		{
			base.StopCoroutine("Tween");
			this.myTransform.localScale = this.initScale;
		}

		// Token: 0x04001CD0 RID: 7376
		public AnimationCurve animCurve;

		// Token: 0x04001CD1 RID: 7377
		[Tooltip("Animation speed multiplier")]
		public float speed = 1f;

		// Token: 0x04001CD2 RID: 7378
		[Tooltip("If true animation will loop, for best effect set animation curve to loop on start and end point")]
		public bool isLoop;

		// Token: 0x04001CD3 RID: 7379
		[Tooltip("If true animation will start automatically, otherwise you need to call Play() method to start the animation")]
		public bool playAtAwake;

		// Token: 0x04001CD4 RID: 7380
		[Space(10f)]
		[Header("Non uniform scale")]
		[Tooltip("If true component will scale by the same amount in X and Y axis, otherwise use animCurve for X scale and animCurveY for Y scale")]
		public bool isUniform = true;

		// Token: 0x04001CD5 RID: 7381
		public AnimationCurve animCurveY;

		// Token: 0x04001CD6 RID: 7382
		private Vector3 initScale;

		// Token: 0x04001CD7 RID: 7383
		private Transform myTransform;

		// Token: 0x04001CD8 RID: 7384
		private Vector3 newScale = Vector3.one;

		// Token: 0x02000F7D RID: 3965
		[CompilerGenerated]
		private sealed class <Tween>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007407 RID: 29703 RVA: 0x000C7145 File Offset: 0x000C5545
			[DebuggerHidden]
			public <Tween>c__Iterator0()
			{
			}

			// Token: 0x06007408 RID: 29704 RVA: 0x000C7150 File Offset: 0x000C5550
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					this.myTransform.localScale = this.initScale;
					t = 0f;
					maxT = this.animCurve.keys[this.animCurve.length - 1].time;
					break;
				case 1U:
					break;
				default:
					return false;
				}
				if (t < maxT || this.isLoop)
				{
					t += this.speed * Time.deltaTime;
					if (!this.isUniform)
					{
						this.newScale.x = 1f * this.animCurve.Evaluate(t);
						this.newScale.y = 1f * this.animCurveY.Evaluate(t);
						this.myTransform.localScale = this.newScale;
					}
					else
					{
						this.myTransform.localScale = Vector3.one * this.animCurve.Evaluate(t);
					}
					this.$current = null;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				}
				this.$PC = -1;
				return false;
			}

			// Token: 0x1700110B RID: 4363
			// (get) Token: 0x06007409 RID: 29705 RVA: 0x000C72F7 File Offset: 0x000C56F7
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x1700110C RID: 4364
			// (get) Token: 0x0600740A RID: 29706 RVA: 0x000C72FF File Offset: 0x000C56FF
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x0600740B RID: 29707 RVA: 0x000C7307 File Offset: 0x000C5707
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x0600740C RID: 29708 RVA: 0x000C7317 File Offset: 0x000C5717
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x0400681D RID: 26653
			internal float <t>__0;

			// Token: 0x0400681E RID: 26654
			internal float <maxT>__0;

			// Token: 0x0400681F RID: 26655
			internal UI_TweenScale $this;

			// Token: 0x04006820 RID: 26656
			internal object $current;

			// Token: 0x04006821 RID: 26657
			internal bool $disposing;

			// Token: 0x04006822 RID: 26658
			internal int $PC;
		}
	}
}
