using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Leap.Unity.Attributes;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006D0 RID: 1744
	public class ExtendedFingerDetector : Detector
	{
		// Token: 0x06002A03 RID: 10755 RVA: 0x000E2B90 File Offset: 0x000E0F90
		public ExtendedFingerDetector()
		{
		}

		// Token: 0x06002A04 RID: 10756 RVA: 0x000E2BE0 File Offset: 0x000E0FE0
		private void OnValidate()
		{
			int num = 0;
			int num2 = 0;
			foreach (PointingState pointingState in new PointingState[]
			{
				this.Thumb,
				this.Index,
				this.Middle,
				this.Ring,
				this.Pinky
			})
			{
				if (pointingState != PointingState.Extended)
				{
					if (pointingState == PointingState.NotExtended)
					{
						num2++;
					}
				}
				else
				{
					num++;
				}
				this.MinimumExtendedCount = Math.Max(num, this.MinimumExtendedCount);
				this.MaximumExtendedCount = Math.Min(5 - num2, this.MaximumExtendedCount);
				this.MaximumExtendedCount = Math.Max(num, this.MaximumExtendedCount);
			}
		}

		// Token: 0x06002A05 RID: 10757 RVA: 0x000E2CA1 File Offset: 0x000E10A1
		private void Awake()
		{
			this.watcherCoroutine = this.extendedFingerWatcher();
		}

		// Token: 0x06002A06 RID: 10758 RVA: 0x000E2CAF File Offset: 0x000E10AF
		private void OnEnable()
		{
			base.StartCoroutine(this.watcherCoroutine);
		}

		// Token: 0x06002A07 RID: 10759 RVA: 0x000E2CBE File Offset: 0x000E10BE
		private void OnDisable()
		{
			base.StopCoroutine(this.watcherCoroutine);
			this.Deactivate();
		}

		// Token: 0x06002A08 RID: 10760 RVA: 0x000E2CD4 File Offset: 0x000E10D4
		private IEnumerator extendedFingerWatcher()
		{
			for (;;)
			{
				bool fingerState = false;
				if (this.HandModel != null && this.HandModel.IsTracked)
				{
					Hand hand = this.HandModel.GetLeapHand();
					if (hand != null)
					{
						fingerState = (this.matchFingerState(hand.Fingers[0], this.Thumb) && this.matchFingerState(hand.Fingers[1], this.Index) && this.matchFingerState(hand.Fingers[2], this.Middle) && this.matchFingerState(hand.Fingers[3], this.Ring) && this.matchFingerState(hand.Fingers[4], this.Pinky));
						int num = 0;
						for (int i = 0; i < 5; i++)
						{
							if (hand.Fingers[i].IsExtended)
							{
								num++;
							}
						}
						fingerState = (fingerState && num <= this.MaximumExtendedCount && num >= this.MinimumExtendedCount);
						if (this.HandModel.IsTracked && fingerState)
						{
							this.Activate();
						}
						else if (!this.HandModel.IsTracked || !fingerState)
						{
							this.Deactivate();
						}
					}
				}
				else if (base.IsActive)
				{
					this.Deactivate();
				}
				yield return new WaitForSeconds(this.Period);
			}
			yield break;
		}

		// Token: 0x06002A09 RID: 10761 RVA: 0x000E2CEF File Offset: 0x000E10EF
		private bool matchFingerState(Finger finger, PointingState requiredState)
		{
			return requiredState == PointingState.Either || (requiredState == PointingState.Extended && finger.IsExtended) || (requiredState == PointingState.NotExtended && !finger.IsExtended);
		}

		// Token: 0x0400222B RID: 8747
		[Tooltip("The interval in seconds at which to check this detector's conditions.")]
		[Units("seconds")]
		[MinValue(0f)]
		public float Period = 0.1f;

		// Token: 0x0400222C RID: 8748
		[Tooltip("The hand model to watch. Set automatically if detector is on a hand.")]
		public HandModelBase HandModel;

		// Token: 0x0400222D RID: 8749
		[Header("Finger States")]
		[Tooltip("Required state of the thumb.")]
		public PointingState Thumb = PointingState.Either;

		// Token: 0x0400222E RID: 8750
		[Tooltip("Required state of the index finger.")]
		public PointingState Index = PointingState.Either;

		// Token: 0x0400222F RID: 8751
		[Tooltip("Required state of the middle finger.")]
		public PointingState Middle = PointingState.Either;

		// Token: 0x04002230 RID: 8752
		[Tooltip("Required state of the ring finger.")]
		public PointingState Ring = PointingState.Either;

		// Token: 0x04002231 RID: 8753
		[Tooltip("Required state of the little finger.")]
		public PointingState Pinky = PointingState.Either;

		// Token: 0x04002232 RID: 8754
		[Header("Min and Max Finger Counts")]
		[Range(0f, 5f)]
		[Tooltip("The minimum number of fingers extended.")]
		public int MinimumExtendedCount;

		// Token: 0x04002233 RID: 8755
		[Range(0f, 5f)]
		[Tooltip("The maximum number of fingers extended.")]
		public int MaximumExtendedCount = 5;

		// Token: 0x04002234 RID: 8756
		[Header("")]
		[Tooltip("Draw this detector's Gizmos, if any. (Gizmos must be on in Unity edtor, too.)")]
		public bool ShowGizmos = true;

		// Token: 0x04002235 RID: 8757
		private IEnumerator watcherCoroutine;

		// Token: 0x02000F9B RID: 3995
		[CompilerGenerated]
		private sealed class <extendedFingerWatcher>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007471 RID: 29809 RVA: 0x000E2D1F File Offset: 0x000E111F
			[DebuggerHidden]
			public <extendedFingerWatcher>c__Iterator0()
			{
			}

			// Token: 0x06007472 RID: 29810 RVA: 0x000E2D28 File Offset: 0x000E1128
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					break;
				case 1U:
					break;
				default:
					return false;
				}
				fingerState = false;
				if (this.HandModel != null && this.HandModel.IsTracked)
				{
					hand = this.HandModel.GetLeapHand();
					if (hand != null)
					{
						fingerState = (base.matchFingerState(hand.Fingers[0], this.Thumb) && base.matchFingerState(hand.Fingers[1], this.Index) && base.matchFingerState(hand.Fingers[2], this.Middle) && base.matchFingerState(hand.Fingers[3], this.Ring) && base.matchFingerState(hand.Fingers[4], this.Pinky));
						int num2 = 0;
						for (int i = 0; i < 5; i++)
						{
							if (hand.Fingers[i].IsExtended)
							{
								num2++;
							}
						}
						fingerState = (fingerState && num2 <= this.MaximumExtendedCount && num2 >= this.MinimumExtendedCount);
						if (this.HandModel.IsTracked && fingerState)
						{
							this.Activate();
						}
						else if (!this.HandModel.IsTracked || !fingerState)
						{
							this.Deactivate();
						}
					}
				}
				else if (base.IsActive)
				{
					this.Deactivate();
				}
				this.$current = new WaitForSeconds(this.Period);
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			}

			// Token: 0x17001121 RID: 4385
			// (get) Token: 0x06007473 RID: 29811 RVA: 0x000E2FA6 File Offset: 0x000E13A6
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x17001122 RID: 4386
			// (get) Token: 0x06007474 RID: 29812 RVA: 0x000E2FAE File Offset: 0x000E13AE
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007475 RID: 29813 RVA: 0x000E2FB6 File Offset: 0x000E13B6
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007476 RID: 29814 RVA: 0x000E2FC6 File Offset: 0x000E13C6
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x04006887 RID: 26759
			internal bool <fingerState>__1;

			// Token: 0x04006888 RID: 26760
			internal Hand <hand>__2;

			// Token: 0x04006889 RID: 26761
			internal ExtendedFingerDetector $this;

			// Token: 0x0400688A RID: 26762
			internal object $current;

			// Token: 0x0400688B RID: 26763
			internal bool $disposing;

			// Token: 0x0400688C RID: 26764
			internal int $PC;
		}
	}
}
