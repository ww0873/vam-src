using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Leap.Unity.Attributes;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006D2 RID: 1746
	public class FingerDirectionDetector : Detector
	{
		// Token: 0x06002A0A RID: 10762 RVA: 0x000E2FD0 File Offset: 0x000E13D0
		public FingerDirectionDetector()
		{
		}

		// Token: 0x06002A0B RID: 10763 RVA: 0x000E3024 File Offset: 0x000E1424
		private void OnValidate()
		{
			if (this.OffAngle < this.OnAngle)
			{
				this.OffAngle = this.OnAngle;
			}
		}

		// Token: 0x06002A0C RID: 10764 RVA: 0x000E3043 File Offset: 0x000E1443
		private void Awake()
		{
			this.watcherCoroutine = this.fingerPointingWatcher();
		}

		// Token: 0x06002A0D RID: 10765 RVA: 0x000E3051 File Offset: 0x000E1451
		private void OnEnable()
		{
			base.StartCoroutine(this.watcherCoroutine);
		}

		// Token: 0x06002A0E RID: 10766 RVA: 0x000E3060 File Offset: 0x000E1460
		private void OnDisable()
		{
			base.StopCoroutine(this.watcherCoroutine);
			this.Deactivate();
		}

		// Token: 0x06002A0F RID: 10767 RVA: 0x000E3074 File Offset: 0x000E1474
		private IEnumerator fingerPointingWatcher()
		{
			int selectedFinger = this.selectedFingerOrdinal();
			for (;;)
			{
				if (this.HandModel != null && this.HandModel.IsTracked)
				{
					Hand hand = this.HandModel.GetLeapHand();
					if (hand != null)
					{
						Vector3 targetDirection = this.selectedDirection(hand.Fingers[selectedFinger].TipPosition.ToVector3());
						Vector3 fingerDirection = hand.Fingers[selectedFinger].Bone(Bone.BoneType.TYPE_DISTAL).Direction.ToVector3();
						float num = Vector3.Angle(fingerDirection, targetDirection);
						if (this.HandModel.IsTracked && num <= this.OnAngle)
						{
							this.Activate();
						}
						else if (!this.HandModel.IsTracked || num >= this.OffAngle)
						{
							this.Deactivate();
						}
					}
				}
				yield return new WaitForSeconds(this.Period);
			}
			yield break;
		}

		// Token: 0x06002A10 RID: 10768 RVA: 0x000E3090 File Offset: 0x000E1490
		private Vector3 selectedDirection(Vector3 tipPosition)
		{
			switch (this.PointingType)
			{
			case PointingType.RelativeToCamera:
				return Camera.main.transform.TransformDirection(this.PointingDirection);
			case PointingType.RelativeToHorizon:
			{
				float y = Camera.main.transform.rotation.eulerAngles.y;
				Quaternion rotation = Quaternion.AngleAxis(y, Vector3.up);
				return rotation * this.PointingDirection;
			}
			case PointingType.RelativeToWorld:
				return this.PointingDirection;
			case PointingType.AtTarget:
				return this.TargetObject.position - tipPosition;
			default:
				return this.PointingDirection;
			}
		}

		// Token: 0x06002A11 RID: 10769 RVA: 0x000E3130 File Offset: 0x000E1530
		private int selectedFingerOrdinal()
		{
			switch (this.FingerName)
			{
			case Finger.FingerType.TYPE_THUMB:
				return 0;
			case Finger.FingerType.TYPE_INDEX:
				return 1;
			case Finger.FingerType.TYPE_MIDDLE:
				return 2;
			case Finger.FingerType.TYPE_RING:
				return 3;
			case Finger.FingerType.TYPE_PINKY:
				return 4;
			default:
				return 1;
			}
		}

		// Token: 0x0400223A RID: 8762
		[Units("seconds")]
		[Tooltip("The interval in seconds at which to check this detector's conditions.")]
		[MinValue(0f)]
		public float Period = 0.1f;

		// Token: 0x0400223B RID: 8763
		[Tooltip("The hand model to watch. Set automatically if detector is on a hand.")]
		public HandModelBase HandModel;

		// Token: 0x0400223C RID: 8764
		[Tooltip("The finger to observe.")]
		public Finger.FingerType FingerName = Finger.FingerType.TYPE_INDEX;

		// Token: 0x0400223D RID: 8765
		[Header("Direction Settings")]
		[Tooltip("How to treat the target direction.")]
		public PointingType PointingType = PointingType.RelativeToHorizon;

		// Token: 0x0400223E RID: 8766
		[Tooltip("The target direction.")]
		[DisableIf("PointingType", PointingType.AtTarget, null)]
		public Vector3 PointingDirection = Vector3.forward;

		// Token: 0x0400223F RID: 8767
		[Tooltip("A target object(optional). Use PointingType.AtTarget")]
		[DisableIf("PointingType", null, PointingType.AtTarget)]
		public Transform TargetObject;

		// Token: 0x04002240 RID: 8768
		[Tooltip("The angle in degrees from the target direction at which to turn on.")]
		[Range(0f, 180f)]
		public float OnAngle = 15f;

		// Token: 0x04002241 RID: 8769
		[Tooltip("The angle in degrees from the target direction at which to turn off.")]
		[Range(0f, 180f)]
		public float OffAngle = 25f;

		// Token: 0x04002242 RID: 8770
		[Header("")]
		[Tooltip("Draw this detector's Gizmos, if any. (Gizmos must be on in Unity edtor, too.)")]
		public bool ShowGizmos = true;

		// Token: 0x04002243 RID: 8771
		private IEnumerator watcherCoroutine;

		// Token: 0x02000F9C RID: 3996
		[CompilerGenerated]
		private sealed class <fingerPointingWatcher>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x06007477 RID: 29815 RVA: 0x000E316E File Offset: 0x000E156E
			[DebuggerHidden]
			public <fingerPointingWatcher>c__Iterator0()
			{
			}

			// Token: 0x06007478 RID: 29816 RVA: 0x000E3178 File Offset: 0x000E1578
			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0U:
					selectedFinger = base.selectedFingerOrdinal();
					break;
				case 1U:
					break;
				default:
					return false;
				}
				if (this.HandModel != null && this.HandModel.IsTracked)
				{
					hand = this.HandModel.GetLeapHand();
					if (hand != null)
					{
						targetDirection = base.selectedDirection(hand.Fingers[selectedFinger].TipPosition.ToVector3());
						fingerDirection = hand.Fingers[selectedFinger].Bone(Bone.BoneType.TYPE_DISTAL).Direction.ToVector3();
						float num2 = Vector3.Angle(fingerDirection, targetDirection);
						if (this.HandModel.IsTracked && num2 <= this.OnAngle)
						{
							this.Activate();
						}
						else if (!this.HandModel.IsTracked || num2 >= this.OffAngle)
						{
							this.Deactivate();
						}
					}
				}
				this.$current = new WaitForSeconds(this.Period);
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			}

			// Token: 0x17001123 RID: 4387
			// (get) Token: 0x06007479 RID: 29817 RVA: 0x000E3312 File Offset: 0x000E1712
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x17001124 RID: 4388
			// (get) Token: 0x0600747A RID: 29818 RVA: 0x000E331A File Offset: 0x000E171A
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x0600747B RID: 29819 RVA: 0x000E3322 File Offset: 0x000E1722
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x0600747C RID: 29820 RVA: 0x000E3332 File Offset: 0x000E1732
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x0400688D RID: 26765
			internal int <selectedFinger>__0;

			// Token: 0x0400688E RID: 26766
			internal Hand <hand>__1;

			// Token: 0x0400688F RID: 26767
			internal Vector3 <targetDirection>__2;

			// Token: 0x04006890 RID: 26768
			internal Vector3 <fingerDirection>__2;

			// Token: 0x04006891 RID: 26769
			internal FingerDirectionDetector $this;

			// Token: 0x04006892 RID: 26770
			internal object $current;

			// Token: 0x04006893 RID: 26771
			internal bool $disposing;

			// Token: 0x04006894 RID: 26772
			internal int $PC;
		}
	}
}
