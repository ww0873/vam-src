using System;
using UnityEngine;

namespace MeshVR.Hands
{
	// Token: 0x02000E1E RID: 3614
	public class FingerOutput : Finger
	{
		// Token: 0x06006F39 RID: 28473 RVA: 0x0029C2EC File Offset: 0x0029A6EC
		public FingerOutput()
		{
		}

		// Token: 0x06006F3A RID: 28474 RVA: 0x0029C2F4 File Offset: 0x0029A6F4
		protected void Awake()
		{
			this.joint = base.GetComponent<ConfigurableJoint>();
		}

		// Token: 0x06006F3B RID: 28475 RVA: 0x0029C304 File Offset: 0x0029A704
		public void UpdateOutput()
		{
			if (this.joint != null)
			{
				Vector3 euler;
				euler.x = 0f;
				euler.y = 0f;
				euler.z = 0f;
				if (this.bendEnabled)
				{
					float num = this.currentBend + this.bendOffset;
					switch (this.bendAxis)
					{
					case Finger.Axis.X:
						euler.x = num;
						break;
					case Finger.Axis.NegX:
						euler.x = -num;
						break;
					case Finger.Axis.Y:
						euler.y = num;
						break;
					case Finger.Axis.NegY:
						euler.y = -num;
						break;
					case Finger.Axis.Z:
						euler.z = num;
						break;
					case Finger.Axis.NegZ:
						euler.z = -num;
						break;
					}
				}
				if (this.spreadEnabled)
				{
					float num2 = this.currentSpread + this.spreadOffset;
					switch (this.spreadAxis)
					{
					case Finger.Axis.X:
						euler.x = num2;
						break;
					case Finger.Axis.NegX:
						euler.x = -num2;
						break;
					case Finger.Axis.Y:
						euler.y = num2;
						break;
					case Finger.Axis.NegY:
						euler.y = -num2;
						break;
					case Finger.Axis.Z:
						euler.z = num2;
						break;
					case Finger.Axis.NegZ:
						euler.z = -num2;
						break;
					}
				}
				if (this.twistEnabled)
				{
					float num3 = this.currentTwist + this.twistOffset;
					switch (this.twistAxis)
					{
					case Finger.Axis.X:
						euler.x = num3;
						break;
					case Finger.Axis.NegX:
						euler.x = -num3;
						break;
					case Finger.Axis.Y:
						euler.y = num3;
						break;
					case Finger.Axis.NegY:
						euler.y = -num3;
						break;
					case Finger.Axis.Z:
						euler.z = num3;
						break;
					case Finger.Axis.NegZ:
						euler.z = -num3;
						break;
					}
				}
				this.joint.targetRotation = Quaternion.Euler(euler);
			}
		}

		// Token: 0x0400607A RID: 24698
		protected ConfigurableJoint joint;
	}
}
