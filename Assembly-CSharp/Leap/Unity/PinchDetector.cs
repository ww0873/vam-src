using System;
using Leap.Unity.Attributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace Leap.Unity
{
	// Token: 0x020006D5 RID: 1749
	public class PinchDetector : AbstractHoldDetector
	{
		// Token: 0x06002A19 RID: 10777 RVA: 0x000E35F7 File Offset: 0x000E19F7
		public PinchDetector()
		{
		}

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06002A1A RID: 10778 RVA: 0x000E3615 File Offset: 0x000E1A15
		public bool IsPinching
		{
			get
			{
				return this.IsHolding;
			}
		}

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06002A1B RID: 10779 RVA: 0x000E361D File Offset: 0x000E1A1D
		public bool DidStartPinch
		{
			get
			{
				return this.DidStartHold;
			}
		}

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06002A1C RID: 10780 RVA: 0x000E3625 File Offset: 0x000E1A25
		public bool DidEndPinch
		{
			get
			{
				return this.DidRelease;
			}
		}

		// Token: 0x06002A1D RID: 10781 RVA: 0x000E3630 File Offset: 0x000E1A30
		protected virtual void OnValidate()
		{
			this.ActivateDistance = Mathf.Max(0f, this.ActivateDistance);
			this.DeactivateDistance = Mathf.Max(0f, this.DeactivateDistance);
			if (this.DeactivateDistance < this.ActivateDistance)
			{
				this.DeactivateDistance = this.ActivateDistance;
			}
		}

		// Token: 0x06002A1E RID: 10782 RVA: 0x000E3688 File Offset: 0x000E1A88
		private float GetPinchDistance(Hand hand)
		{
			Vector3 a = hand.GetIndex().TipPosition.ToVector3();
			Vector3 b = hand.GetThumb().TipPosition.ToVector3();
			return Vector3.Distance(a, b) / base.transform.lossyScale.x;
		}

		// Token: 0x06002A1F RID: 10783 RVA: 0x000E36D4 File Offset: 0x000E1AD4
		protected override void ensureUpToDate()
		{
			if (Time.frameCount == this._lastUpdateFrame)
			{
				return;
			}
			this._lastUpdateFrame = Time.frameCount;
			this._didChange = false;
			Hand leapHand = this._handModel.GetLeapHand();
			if (leapHand == null || !this._handModel.IsTracked)
			{
				this.changeState(false);
				return;
			}
			this._distance = this.GetPinchDistance(leapHand);
			this._rotation = leapHand.Basis.CalculateRotation();
			this._position = ((leapHand.Fingers[0].TipPosition + leapHand.Fingers[1].TipPosition) * 0.5f).ToVector3();
			if (base.IsActive)
			{
				if (this._distance > this.DeactivateDistance)
				{
					this.changeState(false);
				}
			}
			else if (this._distance < this.ActivateDistance)
			{
				this.changeState(true);
			}
			if (base.IsActive)
			{
				this._lastPosition = this._position;
				this._lastRotation = this._rotation;
				this._lastDistance = this._distance;
				this._lastDirection = this._direction;
				this._lastNormal = this._normal;
			}
			if (this.ControlsTransform)
			{
				base.transform.position = this._position;
				base.transform.rotation = this._rotation;
			}
		}

		// Token: 0x04002252 RID: 8786
		protected const float MM_TO_M = 0.001f;

		// Token: 0x04002253 RID: 8787
		[Tooltip("The distance at which to enter the pinching state.")]
		[Header("Distance Settings")]
		[MinValue(0f)]
		[Units("meters")]
		[FormerlySerializedAs("_activatePinchDist")]
		public float ActivateDistance = 0.03f;

		// Token: 0x04002254 RID: 8788
		[Tooltip("The distance at which to leave the pinching state.")]
		[MinValue(0f)]
		[Units("meters")]
		[FormerlySerializedAs("_deactivatePinchDist")]
		public float DeactivateDistance = 0.04f;

		// Token: 0x04002255 RID: 8789
		protected bool _isPinching;

		// Token: 0x04002256 RID: 8790
		protected float _lastPinchTime;

		// Token: 0x04002257 RID: 8791
		protected float _lastUnpinchTime;

		// Token: 0x04002258 RID: 8792
		protected Vector3 _pinchPos;

		// Token: 0x04002259 RID: 8793
		protected Quaternion _pinchRotation;
	}
}
