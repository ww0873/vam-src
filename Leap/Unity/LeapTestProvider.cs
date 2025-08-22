using System;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006FF RID: 1791
	public class LeapTestProvider : LeapProvider
	{
		// Token: 0x06002B6C RID: 11116 RVA: 0x000E9FDE File Offset: 0x000E83DE
		public LeapTestProvider()
		{
		}

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x06002B6D RID: 11117 RVA: 0x000E9FE6 File Offset: 0x000E83E6
		public override Frame CurrentFrame
		{
			get
			{
				return this.frame;
			}
		}

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x06002B6E RID: 11118 RVA: 0x000E9FEE File Offset: 0x000E83EE
		public override Frame CurrentFixedFrame
		{
			get
			{
				return this.frame;
			}
		}

		// Token: 0x06002B6F RID: 11119 RVA: 0x000E9FF6 File Offset: 0x000E83F6
		private void Awake()
		{
			this._cachedLeftHand = TestHandFactory.MakeTestHand(true, 0, 0, TestHandFactory.UnitType.UnityUnits);
			this._cachedRightHand = TestHandFactory.MakeTestHand(false, 0, 0, TestHandFactory.UnitType.UnityUnits);
		}

		// Token: 0x06002B70 RID: 11120 RVA: 0x000EA018 File Offset: 0x000E8418
		private void Update()
		{
			if (this._leftHand == null && this.leftHandBasis != null)
			{
				this._leftHand = this._cachedLeftHand;
				this.frame.Hands.Add(this._leftHand);
			}
			if (this._leftHand != null && this.leftHandBasis == null)
			{
				this.frame.Hands.Remove(this._leftHand);
				this._leftHand = null;
			}
			if (this._leftHand != null)
			{
				this._leftHand.SetTransform(this.leftHandBasis.position, this.leftHandBasis.rotation);
			}
			if (this._rightHand == null && this.rightHandBasis != null)
			{
				this._rightHand = this._cachedRightHand;
				this.frame.Hands.Add(this._rightHand);
			}
			if (this._rightHand != null && this.rightHandBasis == null)
			{
				this.frame.Hands.Remove(this._rightHand);
				this._rightHand = null;
			}
			if (this._rightHand != null)
			{
				this._rightHand.SetTransform(this.rightHandBasis.position, this.rightHandBasis.rotation);
			}
			base.DispatchUpdateFrameEvent(this.frame);
		}

		// Token: 0x06002B71 RID: 11121 RVA: 0x000EA179 File Offset: 0x000E8579
		private void FixedUpdate()
		{
			base.DispatchFixedFrameEvent(this.frame);
		}

		// Token: 0x04002322 RID: 8994
		public Frame frame;

		// Token: 0x04002323 RID: 8995
		[Header("Runtime Basis Transforms")]
		[Tooltip("At runtime, if this Transform is non-null, the LeapTestProvider will create a test-pose left hand at this transform's position and rotation.Setting this binding to null at runtime will cause the hand to disappear from Frame data, as if it stopped tracking.")]
		public Transform leftHandBasis;

		// Token: 0x04002324 RID: 8996
		private Hand _leftHand;

		// Token: 0x04002325 RID: 8997
		private Hand _cachedLeftHand;

		// Token: 0x04002326 RID: 8998
		[Tooltip("At runtime, if this Transform is non-null, the LeapTestProvider will create a test-pose right hand at this transform's position and rotation.Setting this binding to null at runtime will cause the hand to disappear from Frame data, as if it stopped tracking.")]
		public Transform rightHandBasis;

		// Token: 0x04002327 RID: 8999
		private Hand _rightHand;

		// Token: 0x04002328 RID: 9000
		private Hand _cachedRightHand;
	}
}
