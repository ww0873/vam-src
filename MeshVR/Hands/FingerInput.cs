using System;
using UnityEngine;

namespace MeshVR.Hands
{
	// Token: 0x02000E1D RID: 3613
	public class FingerInput : Finger
	{
		// Token: 0x06006F33 RID: 28467 RVA: 0x0029BEDD File Offset: 0x0029A2DD
		public FingerInput()
		{
		}

		// Token: 0x06006F34 RID: 28468 RVA: 0x0029BEF0 File Offset: 0x0029A2F0
		protected void CorrectBend()
		{
			this.currentBend += this.bendOffset;
			if (this.currentBend > 180f)
			{
				this.currentBend -= 360f;
			}
			else if (this.currentBend <= -180f)
			{
				this.currentBend += 360f;
			}
			this.currentBend *= this.bendInputFactor;
		}

		// Token: 0x06006F35 RID: 28469 RVA: 0x0029BF6C File Offset: 0x0029A36C
		protected void CorrectSpread()
		{
			this.currentSpread += this.spreadOffset;
			if (this.currentSpread > 180f)
			{
				this.currentSpread -= 360f;
			}
			else if (this.currentSpread <= -180f)
			{
				this.currentSpread += 360f;
			}
		}

		// Token: 0x06006F36 RID: 28470 RVA: 0x0029BFD8 File Offset: 0x0029A3D8
		protected void CorrectTwist()
		{
			this.currentTwist += this.twistOffset;
			if (this.currentTwist > 180f)
			{
				this.currentTwist -= 360f;
			}
			else if (this.currentTwist <= -180f)
			{
				this.currentTwist += 360f;
			}
		}

		// Token: 0x06006F37 RID: 28471 RVA: 0x0029C044 File Offset: 0x0029A444
		public void UpdateInput()
		{
			Vector3 eulerAngles = (this.inverseStartingLocalRotation * base.transform.localRotation).eulerAngles;
			if (this.bendEnabled)
			{
				switch (this.bendAxis)
				{
				case Finger.Axis.X:
					this.currentBend = eulerAngles.x;
					break;
				case Finger.Axis.NegX:
					this.currentBend = -eulerAngles.x;
					break;
				case Finger.Axis.Y:
					this.currentBend = eulerAngles.y;
					break;
				case Finger.Axis.NegY:
					this.currentBend = -eulerAngles.y;
					break;
				case Finger.Axis.Z:
					this.currentBend = eulerAngles.z;
					break;
				case Finger.Axis.NegZ:
					this.currentBend = -eulerAngles.z;
					break;
				}
				this.CorrectBend();
			}
			if (this.spreadEnabled)
			{
				switch (this.spreadAxis)
				{
				case Finger.Axis.X:
					this.currentSpread = eulerAngles.x;
					break;
				case Finger.Axis.NegX:
					this.currentSpread = -eulerAngles.x;
					break;
				case Finger.Axis.Y:
					this.currentSpread = eulerAngles.y;
					break;
				case Finger.Axis.NegY:
					this.currentSpread = -eulerAngles.y;
					break;
				case Finger.Axis.Z:
					this.currentSpread = eulerAngles.z;
					break;
				case Finger.Axis.NegZ:
					this.currentSpread = -eulerAngles.z;
					break;
				}
				this.CorrectSpread();
			}
			if (this.twistEnabled)
			{
				switch (this.twistAxis)
				{
				case Finger.Axis.X:
					this.currentTwist = eulerAngles.x;
					break;
				case Finger.Axis.NegX:
					this.currentTwist = -eulerAngles.x;
					break;
				case Finger.Axis.Y:
					this.currentTwist = eulerAngles.y;
					break;
				case Finger.Axis.NegY:
					this.currentTwist = -eulerAngles.y;
					break;
				case Finger.Axis.Z:
					this.currentTwist = eulerAngles.z;
					break;
				case Finger.Axis.NegZ:
					this.currentTwist = -eulerAngles.z;
					break;
				}
				this.CorrectTwist();
			}
		}

		// Token: 0x06006F38 RID: 28472 RVA: 0x0029C270 File Offset: 0x0029A670
		public void Init()
		{
			if (!this._wasInit)
			{
				this._wasInit = true;
				this.inverseStartingLocalRotation = Quaternion.Inverse(base.transform.localRotation);
				if (this.debug)
				{
					Debug.Log(string.Concat(new object[]
					{
						"Starting local rotation for ",
						base.name,
						" is ",
						base.transform.localEulerAngles
					}));
				}
			}
		}

		// Token: 0x04006076 RID: 24694
		public float bendInputFactor = 1f;

		// Token: 0x04006077 RID: 24695
		public bool debug;

		// Token: 0x04006078 RID: 24696
		protected Quaternion inverseStartingLocalRotation;

		// Token: 0x04006079 RID: 24697
		protected bool _wasInit;
	}
}
