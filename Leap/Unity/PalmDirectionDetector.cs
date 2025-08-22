using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Leap.Unity.Attributes;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006D3 RID: 1747
	public class PalmDirectionDetector : Detector
	{
		// Token: 0x06002A12 RID: 10770 RVA: 0x000E333C File Offset: 0x000E173C
		public PalmDirectionDetector()
		{
		}

		// Token: 0x06002A13 RID: 10771 RVA: 0x000E3389 File Offset: 0x000E1789
		private void OnValidate()
		{
			if (this.OffAngle < this.OnAngle)
			{
				this.OffAngle = this.OnAngle;
			}
		}

		// Token: 0x06002A14 RID: 10772 RVA: 0x000E33A8 File Offset: 0x000E17A8
		private void Awake()
		{
			this.watcherCoroutine = this.palmWatcher();
		}

		// Token: 0x06002A15 RID: 10773 RVA: 0x000E33B6 File Offset: 0x000E17B6
		private void OnEnable()
		{
			base.StartCoroutine(this.watcherCoroutine);
		}

		// Token: 0x06002A16 RID: 10774 RVA: 0x000E33C5 File Offset: 0x000E17C5
		private void OnDisable()
		{
			base.StopCoroutine(this.watcherCoroutine);
			this.Deactivate();
		}

		// Token: 0x06002A17 RID: 10775 RVA: 0x000E33DC File Offset: 0x000E17DC
		private IEnumerator palmWatcher()
		{
			for (;;)
			{
				if (this.HandModel != null)
				{
					Hand hand = this.HandModel.GetLeapHand();
					if (hand != null)
					{
						Vector3 normal = hand.PalmNormal.ToVector3();
						float num = Vector3.Angle(normal, this.selectedDirection(hand.PalmPosition.ToVector3()));
						if (num <= this.OnAngle)
						{
							this.Activate();
						}
						else if (num > this.OffAngle)
						{
							this.Deactivate();
						}
					}
				}
				yield return new WaitForSeconds(this.Period);
			}
			yield break;
		}

		// Token: 0x06002A18 RID: 10776 RVA: 0x000E33F8 File Offset: 0x000E17F8
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
				if (this.TargetObject != null)
				{
					return this.TargetObject.position - tipPosition;
				}
				return Vector3.zero;
			default:
				return this.PointingDirection;
			}
		}

		// Token: 0x04002244 RID: 8772
		[Units("seconds")]
		[Tooltip("The interval in seconds at which to check this detector's conditions.")]
		[MinValue(0f)]
		public float Period = 0.1f;

		// Token: 0x04002245 RID: 8773
		[Tooltip("The hand model to watch. Set automatically if detector is on a hand.")]
		public HandModelBase HandModel;

		// Token: 0x04002246 RID: 8774
		[Header("Direction Settings")]
		[Tooltip("How to treat the target direction.")]
		public PointingType PointingType = PointingType.RelativeToHorizon;

		// Token: 0x04002247 RID: 8775
		[Tooltip("The target direction.")]
		[DisableIf("PointingType", PointingType.AtTarget, null)]
		public Vector3 PointingDirection = Vector3.forward;

		// Token: 0x04002248 RID: 8776
		[Tooltip("A target object(optional). Use PointingType.AtTarget")]
		[DisableIf("PointingType", null, PointingType.AtTarget)]
		public Transform TargetObject;

		// Token: 0x04002249 RID: 8777
		[Tooltip("The angle in degrees from the target direction at which to turn on.")]
		[Range(0f, 180f)]
		public float OnAngle = 45f;

		// Token: 0x0400224A RID: 8778
		[Tooltip("The angle in degrees from the target direction at which to turn off.")]
		[Range(0f, 180f)]
		public float OffAngle = 65f;

		// Token: 0x0400224B RID: 8779
		[Header("")]
		[Tooltip("Draw this detector's Gizmos, if any. (Gizmos must be on in Unity edtor, too.)")]
		public bool ShowGizmos = true;

		// Token: 0x0400224C RID: 8780
		private IEnumerator watcherCoroutine;

		// Token: 0x02000F9D RID: 3997
		[CompilerGenerated]
		private sealed class <palmWatcher>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
		{
			// Token: 0x0600747D RID: 29821 RVA: 0x000E34AE File Offset: 0x000E18AE
			[DebuggerHidden]
			public <palmWatcher>c__Iterator0()
			{
			}

			// Token: 0x0600747E RID: 29822 RVA: 0x000E34B8 File Offset: 0x000E18B8
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
				if (this.HandModel != null)
				{
					hand = this.HandModel.GetLeapHand();
					if (hand != null)
					{
						normal = hand.PalmNormal.ToVector3();
						float num2 = Vector3.Angle(normal, base.selectedDirection(hand.PalmPosition.ToVector3()));
						if (num2 <= this.OnAngle)
						{
							this.Activate();
						}
						else if (num2 > this.OffAngle)
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

			// Token: 0x17001125 RID: 4389
			// (get) Token: 0x0600747F RID: 29823 RVA: 0x000E35D0 File Offset: 0x000E19D0
			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x17001126 RID: 4390
			// (get) Token: 0x06007480 RID: 29824 RVA: 0x000E35D8 File Offset: 0x000E19D8
			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			// Token: 0x06007481 RID: 29825 RVA: 0x000E35E0 File Offset: 0x000E19E0
			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			// Token: 0x06007482 RID: 29826 RVA: 0x000E35F0 File Offset: 0x000E19F0
			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x04006895 RID: 26773
			internal Hand <hand>__1;

			// Token: 0x04006896 RID: 26774
			internal Vector3 <normal>__2;

			// Token: 0x04006897 RID: 26775
			internal PalmDirectionDetector $this;

			// Token: 0x04006898 RID: 26776
			internal object $current;

			// Token: 0x04006899 RID: 26777
			internal bool $disposing;

			// Token: 0x0400689A RID: 26778
			internal int $PC;
		}
	}
}
