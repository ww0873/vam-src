using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Leap.Unity.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace Leap.Unity.Animation
{
	// Token: 0x0200063B RID: 1595
	public class TransformTweenBehaviour : MonoBehaviour
	{
		// Token: 0x06002706 RID: 9990 RVA: 0x000DAB40 File Offset: 0x000D8F40
		public TransformTweenBehaviour()
		{
			if (TransformTweenBehaviour.<>f__am$cache0 == null)
			{
				TransformTweenBehaviour.<>f__am$cache0 = new Action<float>(TransformTweenBehaviour.<OnProgress>m__0);
			}
			this.OnProgress = TransformTweenBehaviour.<>f__am$cache0;
			if (TransformTweenBehaviour.<>f__am$cache1 == null)
			{
				TransformTweenBehaviour.<>f__am$cache1 = new Action(TransformTweenBehaviour.<OnLeaveStart>m__1);
			}
			this.OnLeaveStart = TransformTweenBehaviour.<>f__am$cache1;
			if (TransformTweenBehaviour.<>f__am$cache2 == null)
			{
				TransformTweenBehaviour.<>f__am$cache2 = new Action(TransformTweenBehaviour.<OnReachEnd>m__2);
			}
			this.OnReachEnd = TransformTweenBehaviour.<>f__am$cache2;
			if (TransformTweenBehaviour.<>f__am$cache3 == null)
			{
				TransformTweenBehaviour.<>f__am$cache3 = new Action(TransformTweenBehaviour.<OnLeaveEnd>m__3);
			}
			this.OnLeaveEnd = TransformTweenBehaviour.<>f__am$cache3;
			if (TransformTweenBehaviour.<>f__am$cache4 == null)
			{
				TransformTweenBehaviour.<>f__am$cache4 = new Action(TransformTweenBehaviour.<OnReachStart>m__4);
			}
			this.OnReachStart = TransformTweenBehaviour.<>f__am$cache4;
			this._curDelayedDirection = Direction.Backward;
			base..ctor();
		}

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x06002707 RID: 9991 RVA: 0x000DAC30 File Offset: 0x000D9030
		// (set) Token: 0x06002708 RID: 9992 RVA: 0x000DAC38 File Offset: 0x000D9038
		public Tween tween
		{
			get
			{
				return this._tween;
			}
			set
			{
				this._tween = value;
			}
		}

		// Token: 0x06002709 RID: 9993 RVA: 0x000DAC44 File Offset: 0x000D9044
		private void OnValidate()
		{
			if (this.targetTransform != null)
			{
				if (this.startTransform == this.targetTransform)
				{
					UnityEngine.Debug.LogError("The start transform of the TransformTweenBehaviour should be a different transform than the target transform; the start transform provides starting position/rotation/scale information for the tween.", base.gameObject);
				}
				else if (this.endTransform == this.targetTransform)
				{
					UnityEngine.Debug.LogError("The end transform of the TransformTweenBehaviour should be a different transform than the target transform; the end transform provides ending position/rotation/scale information for the tween.", base.gameObject);
				}
			}
		}

		// Token: 0x0600270A RID: 9994 RVA: 0x000DACB4 File Offset: 0x000D90B4
		private void Awake()
		{
			this.initUnityEvents();
			this._tween = Tween.Persistent().OverTime(this.tweenDuration).Smooth(this.tweenSmoothType);
			if (this.tweenLocalPosition)
			{
				this._tween = this._tween.Target(this.targetTransform).LocalPosition(this.startTransform, this.endTransform);
			}
			if (this.tweenLocalRotation)
			{
				this._tween = this._tween.Target(this.targetTransform).LocalRotation(this.startTransform, this.endTransform);
			}
			if (this.tweenLocalScale)
			{
				this._tween = this._tween.Target(this.targetTransform).LocalScale(this.startTransform, this.endTransform);
			}
			this._tween.OnProgress(this.OnProgress);
			this._tween.OnLeaveStart(this.OnLeaveStart);
			this._tween.OnReachEnd(this.OnReachEnd);
			this._tween.OnLeaveEnd(this.OnLeaveEnd);
			this._tween.OnReachStart(this.OnReachStart);
			if (this.startAtEnd)
			{
				this._tween.progress = 0.9999999f;
				this._tween.Play(Direction.Forward);
			}
			else
			{
				this._tween.progress = 1E-07f;
				this._tween.Play(Direction.Backward);
			}
		}

		// Token: 0x0600270B RID: 9995 RVA: 0x000DAE35 File Offset: 0x000D9235
		private void OnDestroy()
		{
			if (this._tween.isValid)
			{
				this._tween.Release();
			}
		}

		// Token: 0x0600270C RID: 9996 RVA: 0x000DAE52 File Offset: 0x000D9252
		public void PlayTween()
		{
			this.PlayTween(Direction.Forward, 0f);
		}

		// Token: 0x0600270D RID: 9997 RVA: 0x000DAE60 File Offset: 0x000D9260
		public void PlayTween(Direction tweenDirection = Direction.Forward, float afterDelay = 0f)
		{
			if (this._playTweenAfterDelayCoroutine != null && tweenDirection != this._curDelayedDirection)
			{
				base.StopCoroutine(this._playTweenAfterDelayCoroutine);
				this._curDelayedDirection = tweenDirection;
			}
			this._playTweenAfterDelayCoroutine = base.StartCoroutine(this.playAfterDelay(tweenDirection, afterDelay));
		}

		// Token: 0x0600270E RID: 9998 RVA: 0x000DAEA0 File Offset: 0x000D92A0
		private IEnumerator playAfterDelay(Direction tweenDirection, float delay)
		{
			yield return new WaitForSeconds(delay);
			this.tween.Play(tweenDirection);
			yield break;
		}

		// Token: 0x0600270F RID: 9999 RVA: 0x000DAEC9 File Offset: 0x000D92C9
		public void PlayForward()
		{
			this.PlayTween(Direction.Forward, 0f);
		}

		// Token: 0x06002710 RID: 10000 RVA: 0x000DAED7 File Offset: 0x000D92D7
		public void PlayBackward()
		{
			this.PlayTween(Direction.Backward, 0f);
		}

		// Token: 0x06002711 RID: 10001 RVA: 0x000DAEE5 File Offset: 0x000D92E5
		public void PlayForwardAfterDelay(float delay = 0f)
		{
			this.PlayTween(Direction.Forward, delay);
		}

		// Token: 0x06002712 RID: 10002 RVA: 0x000DAEEF File Offset: 0x000D92EF
		public void PlayBackwardAfterDelay(float delay = 0f)
		{
			this.PlayTween(Direction.Backward, delay);
		}

		// Token: 0x06002713 RID: 10003 RVA: 0x000DAEFC File Offset: 0x000D92FC
		public void StopTween()
		{
			this.tween.Stop();
		}

		// Token: 0x06002714 RID: 10004 RVA: 0x000DAF17 File Offset: 0x000D9317
		public void SetTargetToStart()
		{
			this.setTargetTo(this.startTransform);
		}

		// Token: 0x06002715 RID: 10005 RVA: 0x000DAF25 File Offset: 0x000D9325
		public void SetTargetToEnd()
		{
			this.setTargetTo(this.endTransform);
		}

		// Token: 0x06002716 RID: 10006 RVA: 0x000DAF34 File Offset: 0x000D9334
		private void setTargetTo(Transform t)
		{
			if (this.targetTransform != null && t != null)
			{
				if (this.tweenLocalPosition)
				{
					this.targetTransform.localPosition = t.localPosition;
				}
				if (this.tweenLocalRotation)
				{
					this.targetTransform.localRotation = t.localRotation;
				}
				if (this.tweenLocalScale)
				{
					this.targetTransform.localScale = t.localScale;
				}
			}
		}

		// Token: 0x06002717 RID: 10007 RVA: 0x000DAFB2 File Offset: 0x000D93B2
		private void initUnityEvents()
		{
			this.setupCallback(ref this.OnLeaveStart, TransformTweenBehaviour.EventType.OnLeaveStart);
			this.setupCallback(ref this.OnReachEnd, TransformTweenBehaviour.EventType.OnReachEnd);
			this.setupCallback(ref this.OnLeaveEnd, TransformTweenBehaviour.EventType.OnLeaveEnd);
			this.setupCallback(ref this.OnReachStart, TransformTweenBehaviour.EventType.OnReachStart);
		}

		// Token: 0x06002718 RID: 10008 RVA: 0x000DAFF4 File Offset: 0x000D93F4
		private void setupCallback(ref Action action, TransformTweenBehaviour.EventType type)
		{
			TransformTweenBehaviour.<setupCallback>c__AnonStorey1 <setupCallback>c__AnonStorey = new TransformTweenBehaviour.<setupCallback>c__AnonStorey1();
			<setupCallback>c__AnonStorey.type = type;
			<setupCallback>c__AnonStorey.$this = this;
			action = (Action)Delegate.Combine(action, new Action(<setupCallback>c__AnonStorey.<>m__0));
		}

		// Token: 0x06002719 RID: 10009 RVA: 0x000DB030 File Offset: 0x000D9430
		private void setupCallback<T>(ref Action<T> action, TransformTweenBehaviour.EventType type)
		{
			TransformTweenBehaviour.<setupCallback>c__AnonStorey2<T> <setupCallback>c__AnonStorey = new TransformTweenBehaviour.<setupCallback>c__AnonStorey2<T>();
			<setupCallback>c__AnonStorey.type = type;
			<setupCallback>c__AnonStorey.$this = this;
			action = (Action<T>)Delegate.Combine(action, new Action<T>(<setupCallback>c__AnonStorey.<>m__0));
		}

		// Token: 0x0600271A RID: 10010 RVA: 0x000DB06B File Offset: 0x000D946B
		[CompilerGenerated]
		private static void <OnProgress>m__0(float progress)
		{
		}

		// Token: 0x0600271B RID: 10011 RVA: 0x000DB06D File Offset: 0x000D946D
		[CompilerGenerated]
		private static void <OnLeaveStart>m__1()
		{
		}

		// Token: 0x0600271C RID: 10012 RVA: 0x000DB06F File Offset: 0x000D946F
		[CompilerGenerated]
		private static void <OnReachEnd>m__2()
		{
		}

		// Token: 0x0600271D RID: 10013 RVA: 0x000DB071 File Offset: 0x000D9471
		[CompilerGenerated]
		private static void <OnLeaveEnd>m__3()
		{
		}

		// Token: 0x0600271E RID: 10014 RVA: 0x000DB073 File Offset: 0x000D9473
		[CompilerGenerated]
		private static void <OnReachStart>m__4()
		{
		}

		// Token: 0x0400210B RID: 8459
		[Tooltip("The transform to which to apply the tweened properties.")]
		public Transform targetTransform;

		// Token: 0x0400210C RID: 8460
		[Tooltip("The transform whose position/rotation/localScale provide the start state of the tween.")]
		public Transform startTransform;

		// Token: 0x0400210D RID: 8461
		[Tooltip("The transform whose position/rotation/localScale provide the end state of the tween.")]
		public Transform endTransform;

		// Token: 0x0400210E RID: 8462
		public bool startAtEnd;

		// Token: 0x0400210F RID: 8463
		[Header("Tween Settings")]
		public bool tweenLocalPosition = true;

		// Token: 0x04002110 RID: 8464
		public bool tweenLocalRotation = true;

		// Token: 0x04002111 RID: 8465
		public bool tweenLocalScale = true;

		// Token: 0x04002112 RID: 8466
		[MinValue(0.001f)]
		public float tweenDuration = 0.25f;

		// Token: 0x04002113 RID: 8467
		public SmoothType tweenSmoothType = SmoothType.Smooth;

		// Token: 0x04002114 RID: 8468
		public Action<float> OnProgress;

		// Token: 0x04002115 RID: 8469
		public Action OnLeaveStart;

		// Token: 0x04002116 RID: 8470
		public Action OnReachEnd;

		// Token: 0x04002117 RID: 8471
		public Action OnLeaveEnd;

		// Token: 0x04002118 RID: 8472
		public Action OnReachStart;

		// Token: 0x04002119 RID: 8473
		private Tween _tween;

		// Token: 0x0400211A RID: 8474
		private Coroutine _playTweenAfterDelayCoroutine;

		// Token: 0x0400211B RID: 8475
		private Direction _curDelayedDirection;

		// Token: 0x0400211C RID: 8476
		[SerializeField]
		private EnumEventTable _eventTable;

		// Token: 0x0400211D RID: 8477
		[CompilerGenerated]
		private static Action<float> <>f__am$cache0;

		// Token: 0x0400211E RID: 8478
		[CompilerGenerated]
		private static Action <>f__am$cache1;

		// Token: 0x0400211F RID: 8479
		[CompilerGenerated]
		private static Action <>f__am$cache2;

		// Token: 0x04002120 RID: 8480
		[CompilerGenerated]
		private static Action <>f__am$cache3;

		// Token: 0x04002121 RID: 8481
		[CompilerGenerated]
		private static Action <>f__am$cache4;

		// Token: 0x0200063C RID: 1596
		[Serializable]
		public class FloatEvent : UnityEvent<float>
		{
			// Token: 0x0600271F RID: 10015 RVA: 0x000DB075 File Offset: 0x000D9475
			public FloatEvent()
			{
			}
		}

		// Token: 0x0200063D RID: 1597
		public enum EventType
		{
			// Token: 0x04002123 RID: 8483
			OnLeaveStart = 110,
			// Token: 0x04002124 RID: 8484
			OnReachEnd = 120,
			// Token: 0x04002125 RID: 8485
			OnLeaveEnd = 130,
			// Token: 0x04002126 RID: 8486
			OnReachStart = 140
		}

		// Token: 0x02000F8E RID: 3982
		[CompilerGenerated]
		private sealed class <playAfterDelay>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007453 RID: 29779 RVA: 0x000DB07D File Offset: 0x000D947D
			[DebuggerHidden]
			public <playAfterDelay>c__Iterator0()
			{
			}

			// Token: 0x06007454 RID: 29780 RVA: 0x000DB088 File Offset: 0x000D9488
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					this.$current = new WaitForSeconds(delay);
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1U:
					base.tween.Play(tweenDirection);
					this.$PC = -1;
					break;
				}
				return false;
			}

			// Token: 0x1700111F RID: 4383
			// (get) Token: 0x06007455 RID: 29781 RVA: 0x000DB0FF File Offset: 0x000D94FF
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x17001120 RID: 4384
			// (get) Token: 0x06007456 RID: 29782 RVA: 0x000DB107 File Offset: 0x000D9507
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007457 RID: 29783 RVA: 0x000DB10F File Offset: 0x000D950F
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007458 RID: 29784 RVA: 0x000DB11F File Offset: 0x000D951F
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x04006863 RID: 26723
			internal float delay;

			// Token: 0x04006864 RID: 26724
			internal Direction tweenDirection;

			// Token: 0x04006865 RID: 26725
			internal TransformTweenBehaviour $this;

			// Token: 0x04006866 RID: 26726
			internal object $current;

			// Token: 0x04006867 RID: 26727
			internal bool $disposing;

			// Token: 0x04006868 RID: 26728
			internal int $PC;
		}

		// Token: 0x02000F8F RID: 3983
		[CompilerGenerated]
		private sealed class <setupCallback>c__AnonStorey1
		{
			// Token: 0x06007459 RID: 29785 RVA: 0x000DB126 File Offset: 0x000D9526
			public <setupCallback>c__AnonStorey1()
			{
			}

			// Token: 0x0600745A RID: 29786 RVA: 0x000DB12E File Offset: 0x000D952E
			internal void <>m__0()
			{
				this.$this._eventTable.Invoke((int)this.type);
			}

			// Token: 0x04006869 RID: 26729
			internal TransformTweenBehaviour.EventType type;

			// Token: 0x0400686A RID: 26730
			internal TransformTweenBehaviour $this;
		}

		// Token: 0x02000F90 RID: 3984
		[CompilerGenerated]
		private sealed class <setupCallback>c__AnonStorey2<T>
		{
			// Token: 0x0600745B RID: 29787 RVA: 0x000DB146 File Offset: 0x000D9546
			public <setupCallback>c__AnonStorey2()
			{
			}

			// Token: 0x0600745C RID: 29788 RVA: 0x000DB14E File Offset: 0x000D954E
			internal void <>m__0(T anchObj)
			{
				this.$this._eventTable.Invoke((int)this.type);
			}

			// Token: 0x0400686B RID: 26731
			internal TransformTweenBehaviour.EventType type;

			// Token: 0x0400686C RID: 26732
			internal TransformTweenBehaviour $this;
		}
	}
}
