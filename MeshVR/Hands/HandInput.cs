using System;
using UnityEngine;

namespace MeshVR.Hands
{
	// Token: 0x02000E1F RID: 3615
	public class HandInput : MonoBehaviour
	{
		// Token: 0x06006F3C RID: 28476 RVA: 0x0029C51E File Offset: 0x0029A91E
		public HandInput()
		{
		}

		// Token: 0x06006F3D RID: 28477 RVA: 0x0029C53C File Offset: 0x0029A93C
		protected virtual void Update()
		{
			float num = this.fingerSpreadOffset * 0.25f;
			if (this.hand == HandInput.Hand.Left)
			{
				if (this.thumbProximal != null)
				{
					this.thumbProximal.bendInputFactor = this.thumbInputFactor;
					this.thumbProximal.UpdateInput();
					HandInput.leftThumbProximalBend = this.thumbProximal.currentBend + this.thumbBendOffset;
					HandInput.leftThumbProximalSpread = this.thumbProximal.currentSpread + this.thumbSpreadOffset;
					HandInput.leftThumbProximalTwist = this.thumbProximal.currentTwist;
				}
				if (this.thumbMiddle != null)
				{
					this.thumbMiddle.bendInputFactor = this.thumbInputFactor;
					this.thumbMiddle.UpdateInput();
					HandInput.leftThumbMiddleBend = this.thumbMiddle.currentBend + this.thumbBendOffset;
				}
				if (this.thumbDistal != null)
				{
					this.thumbDistal.bendInputFactor = this.thumbInputFactor;
					this.thumbDistal.UpdateInput();
					HandInput.leftThumbDistalBend = this.thumbDistal.currentBend + this.thumbBendOffset;
				}
				if (this.indexProximal != null)
				{
					this.indexProximal.bendInputFactor = this.fingerInputFactor;
					this.indexProximal.UpdateInput();
					HandInput.leftIndexProximalBend = this.indexProximal.currentBend + this.fingerBendOffset;
					if (this.ignoreInputFingerSpread)
					{
						HandInput.leftIndexProximalSpread = -this.fingerSpreadOffset;
					}
					else
					{
						HandInput.leftIndexProximalSpread = this.indexProximal.currentSpread - this.fingerSpreadOffset;
					}
					HandInput.leftIndexProximalTwist = this.indexProximal.currentTwist;
				}
				if (this.indexMiddle != null)
				{
					this.indexMiddle.bendInputFactor = this.fingerInputFactor;
					this.indexMiddle.UpdateInput();
					HandInput.leftIndexMiddleBend = this.indexMiddle.currentBend + this.fingerBendOffset;
				}
				if (this.indexDistal != null)
				{
					this.indexDistal.bendInputFactor = this.fingerInputFactor;
					this.indexDistal.UpdateInput();
					HandInput.leftIndexDistalBend = this.indexDistal.currentBend + this.fingerBendOffset;
				}
				if (this.middleProximal != null)
				{
					this.middleProximal.bendInputFactor = this.fingerInputFactor;
					this.middleProximal.UpdateInput();
					HandInput.leftMiddleProximalBend = this.middleProximal.currentBend + this.fingerBendOffset;
					if (this.ignoreInputFingerSpread)
					{
						HandInput.leftMiddleProximalSpread = -num;
					}
					else
					{
						HandInput.leftMiddleProximalSpread = this.middleProximal.currentSpread - num;
					}
					HandInput.leftMiddleProximalTwist = this.middleProximal.currentTwist;
				}
				if (this.middleMiddle != null)
				{
					this.middleMiddle.bendInputFactor = this.fingerInputFactor;
					this.middleMiddle.UpdateInput();
					HandInput.leftMiddleMiddleBend = this.middleMiddle.currentBend + this.fingerBendOffset;
				}
				if (this.middleDistal != null)
				{
					this.middleDistal.bendInputFactor = this.fingerInputFactor;
					this.middleDistal.UpdateInput();
					HandInput.leftMiddleDistalBend = this.middleDistal.currentBend + this.fingerBendOffset;
				}
				if (this.ringProximal != null)
				{
					this.ringProximal.bendInputFactor = this.fingerInputFactor;
					this.ringProximal.UpdateInput();
					HandInput.leftRingProximalBend = this.ringProximal.currentBend + this.fingerBendOffset;
					if (this.ignoreInputFingerSpread)
					{
						HandInput.leftRingProximalSpread = num;
					}
					else
					{
						HandInput.leftRingProximalSpread = this.ringProximal.currentSpread + num;
					}
					HandInput.leftRingProximalTwist = this.ringProximal.currentTwist;
				}
				if (this.ringMiddle != null)
				{
					this.ringMiddle.bendInputFactor = this.fingerInputFactor;
					this.ringMiddle.UpdateInput();
					HandInput.leftRingMiddleBend = this.ringMiddle.currentBend + this.fingerBendOffset;
				}
				if (this.ringDistal != null)
				{
					this.ringDistal.bendInputFactor = this.fingerInputFactor;
					this.ringDistal.UpdateInput();
					HandInput.leftRingDistalBend = this.ringDistal.currentBend + this.fingerBendOffset;
				}
				if (this.pinkyProximal != null)
				{
					this.pinkyProximal.bendInputFactor = this.fingerInputFactor;
					this.pinkyProximal.UpdateInput();
					HandInput.leftPinkyProximalBend = this.pinkyProximal.currentBend + this.fingerBendOffset;
					if (this.ignoreInputFingerSpread)
					{
						HandInput.leftPinkyProximalSpread = this.fingerSpreadOffset;
					}
					else
					{
						HandInput.leftPinkyProximalSpread = this.pinkyProximal.currentSpread + this.fingerSpreadOffset;
					}
					HandInput.leftPinkyProximalTwist = this.pinkyProximal.currentTwist;
				}
				if (this.pinkyMiddle != null)
				{
					this.pinkyMiddle.bendInputFactor = this.fingerInputFactor;
					this.pinkyMiddle.UpdateInput();
					HandInput.leftPinkyMiddleBend = this.pinkyMiddle.currentBend + this.fingerBendOffset;
				}
				if (this.pinkyDistal != null)
				{
					this.pinkyDistal.bendInputFactor = this.fingerInputFactor;
					this.pinkyDistal.UpdateInput();
					HandInput.leftPinkyDistalBend = this.pinkyDistal.currentBend + this.fingerBendOffset;
				}
			}
			else
			{
				if (this.thumbProximal != null)
				{
					this.thumbProximal.bendInputFactor = this.thumbInputFactor;
					this.thumbProximal.UpdateInput();
					HandInput.rightThumbProximalBend = this.thumbProximal.currentBend + this.thumbBendOffset;
					HandInput.rightThumbProximalSpread = this.thumbProximal.currentSpread + this.thumbSpreadOffset;
					HandInput.rightThumbProximalTwist = this.thumbProximal.currentTwist;
				}
				if (this.thumbMiddle != null)
				{
					this.thumbMiddle.bendInputFactor = this.thumbInputFactor;
					this.thumbMiddle.UpdateInput();
					HandInput.rightThumbMiddleBend = this.thumbMiddle.currentBend + this.thumbBendOffset;
				}
				if (this.thumbDistal != null)
				{
					this.thumbDistal.bendInputFactor = this.thumbInputFactor;
					this.thumbDistal.UpdateInput();
					HandInput.rightThumbDistalBend = this.thumbDistal.currentBend + this.thumbBendOffset;
				}
				if (this.indexProximal != null)
				{
					this.indexProximal.bendInputFactor = this.fingerInputFactor;
					this.indexProximal.UpdateInput();
					HandInput.rightIndexProximalBend = this.indexProximal.currentBend + this.fingerBendOffset;
					if (this.ignoreInputFingerSpread)
					{
						HandInput.rightIndexProximalSpread = -this.fingerSpreadOffset;
					}
					else
					{
						HandInput.rightIndexProximalSpread = this.indexProximal.currentSpread - this.fingerSpreadOffset;
					}
					HandInput.rightIndexProximalTwist = this.indexProximal.currentTwist;
				}
				if (this.indexMiddle != null)
				{
					this.indexMiddle.bendInputFactor = this.fingerInputFactor;
					this.indexMiddle.UpdateInput();
					HandInput.rightIndexMiddleBend = this.indexMiddle.currentBend + this.fingerBendOffset;
				}
				if (this.indexDistal != null)
				{
					this.indexDistal.bendInputFactor = this.fingerInputFactor;
					this.indexDistal.UpdateInput();
					HandInput.rightIndexDistalBend = this.indexDistal.currentBend + this.fingerBendOffset;
				}
				if (this.middleProximal != null)
				{
					this.middleProximal.bendInputFactor = this.fingerInputFactor;
					this.middleProximal.UpdateInput();
					HandInput.rightMiddleProximalBend = this.middleProximal.currentBend + this.fingerBendOffset;
					if (this.ignoreInputFingerSpread)
					{
						HandInput.rightMiddleProximalSpread = -num;
					}
					else
					{
						HandInput.rightMiddleProximalSpread = this.middleProximal.currentSpread - num;
					}
					HandInput.rightMiddleProximalTwist = this.middleProximal.currentTwist;
				}
				if (this.middleMiddle != null)
				{
					this.middleMiddle.bendInputFactor = this.fingerInputFactor;
					this.middleMiddle.UpdateInput();
					HandInput.rightMiddleMiddleBend = this.middleMiddle.currentBend + this.fingerBendOffset;
				}
				if (this.middleDistal != null)
				{
					this.middleDistal.bendInputFactor = this.fingerInputFactor;
					this.middleDistal.UpdateInput();
					HandInput.rightMiddleDistalBend = this.middleDistal.currentBend + this.fingerBendOffset;
				}
				if (this.ringProximal != null)
				{
					this.ringProximal.bendInputFactor = this.fingerInputFactor;
					this.ringProximal.UpdateInput();
					HandInput.rightRingProximalBend = this.ringProximal.currentBend + this.fingerBendOffset;
					if (this.ignoreInputFingerSpread)
					{
						HandInput.rightRingProximalSpread = num;
					}
					else
					{
						HandInput.rightRingProximalSpread = this.ringProximal.currentSpread + num;
					}
					HandInput.rightRingProximalTwist = this.ringProximal.currentTwist;
				}
				if (this.ringMiddle != null)
				{
					this.ringMiddle.bendInputFactor = this.fingerInputFactor;
					this.ringMiddle.UpdateInput();
					HandInput.rightRingMiddleBend = this.ringMiddle.currentBend + this.fingerBendOffset;
				}
				if (this.ringDistal != null)
				{
					this.ringDistal.bendInputFactor = this.fingerInputFactor;
					this.ringDistal.UpdateInput();
					HandInput.rightRingDistalBend = this.ringDistal.currentBend + this.fingerBendOffset;
				}
				if (this.pinkyProximal != null)
				{
					this.pinkyProximal.bendInputFactor = this.fingerInputFactor;
					this.pinkyProximal.UpdateInput();
					HandInput.rightPinkyProximalBend = this.pinkyProximal.currentBend + this.fingerBendOffset;
					if (this.ignoreInputFingerSpread)
					{
						HandInput.rightPinkyProximalSpread = this.fingerSpreadOffset;
					}
					else
					{
						HandInput.rightPinkyProximalSpread = this.pinkyProximal.currentSpread + this.fingerSpreadOffset;
					}
					HandInput.rightPinkyProximalTwist = this.pinkyProximal.currentTwist;
				}
				if (this.pinkyMiddle != null)
				{
					this.pinkyMiddle.bendInputFactor = this.fingerInputFactor;
					this.pinkyMiddle.UpdateInput();
					HandInput.rightPinkyMiddleBend = this.pinkyMiddle.currentBend + this.fingerBendOffset;
				}
				if (this.pinkyDistal != null)
				{
					this.pinkyDistal.bendInputFactor = this.fingerInputFactor;
					this.pinkyDistal.UpdateInput();
					HandInput.rightPinkyDistalBend = this.pinkyDistal.currentBend + this.fingerBendOffset;
				}
			}
		}

		// Token: 0x06006F3E RID: 28478 RVA: 0x0029CF98 File Offset: 0x0029B398
		public void Init()
		{
			if (this.thumbProximal != null)
			{
				this.thumbProximal.Init();
			}
			if (this.thumbMiddle != null)
			{
				this.thumbMiddle.Init();
			}
			if (this.thumbDistal != null)
			{
				this.thumbDistal.Init();
			}
			if (this.indexProximal != null)
			{
				this.indexProximal.Init();
			}
			if (this.indexMiddle != null)
			{
				this.indexMiddle.Init();
			}
			if (this.indexDistal != null)
			{
				this.indexDistal.Init();
			}
			if (this.middleProximal != null)
			{
				this.middleProximal.Init();
			}
			if (this.middleMiddle != null)
			{
				this.middleMiddle.Init();
			}
			if (this.middleDistal != null)
			{
				this.middleDistal.Init();
			}
			if (this.ringProximal != null)
			{
				this.ringProximal.Init();
			}
			if (this.ringMiddle != null)
			{
				this.ringMiddle.Init();
			}
			if (this.ringDistal != null)
			{
				this.ringDistal.Init();
			}
			if (this.pinkyProximal != null)
			{
				this.pinkyProximal.Init();
			}
			if (this.pinkyMiddle != null)
			{
				this.pinkyMiddle.Init();
			}
			if (this.pinkyDistal != null)
			{
				this.pinkyDistal.Init();
			}
		}

		// Token: 0x06006F3F RID: 28479 RVA: 0x0029D149 File Offset: 0x0029B549
		protected virtual void Awake()
		{
			this.Init();
		}

		// Token: 0x0400607B RID: 24699
		public static float rightThumbProximalBend;

		// Token: 0x0400607C RID: 24700
		public static float rightThumbProximalSpread;

		// Token: 0x0400607D RID: 24701
		public static float rightThumbProximalTwist;

		// Token: 0x0400607E RID: 24702
		public static float rightThumbMiddleBend;

		// Token: 0x0400607F RID: 24703
		public static float rightThumbDistalBend;

		// Token: 0x04006080 RID: 24704
		public static float rightIndexProximalBend;

		// Token: 0x04006081 RID: 24705
		public static float rightIndexProximalSpread;

		// Token: 0x04006082 RID: 24706
		public static float rightIndexProximalTwist;

		// Token: 0x04006083 RID: 24707
		public static float rightIndexMiddleBend;

		// Token: 0x04006084 RID: 24708
		public static float rightIndexDistalBend;

		// Token: 0x04006085 RID: 24709
		public static float rightMiddleProximalBend;

		// Token: 0x04006086 RID: 24710
		public static float rightMiddleProximalSpread;

		// Token: 0x04006087 RID: 24711
		public static float rightMiddleProximalTwist;

		// Token: 0x04006088 RID: 24712
		public static float rightMiddleMiddleBend;

		// Token: 0x04006089 RID: 24713
		public static float rightMiddleDistalBend;

		// Token: 0x0400608A RID: 24714
		public static float rightRingProximalBend;

		// Token: 0x0400608B RID: 24715
		public static float rightRingProximalSpread;

		// Token: 0x0400608C RID: 24716
		public static float rightRingProximalTwist;

		// Token: 0x0400608D RID: 24717
		public static float rightRingMiddleBend;

		// Token: 0x0400608E RID: 24718
		public static float rightRingDistalBend;

		// Token: 0x0400608F RID: 24719
		public static float rightPinkyProximalBend;

		// Token: 0x04006090 RID: 24720
		public static float rightPinkyProximalSpread;

		// Token: 0x04006091 RID: 24721
		public static float rightPinkyProximalTwist;

		// Token: 0x04006092 RID: 24722
		public static float rightPinkyMiddleBend;

		// Token: 0x04006093 RID: 24723
		public static float rightPinkyDistalBend;

		// Token: 0x04006094 RID: 24724
		public static float leftThumbProximalBend;

		// Token: 0x04006095 RID: 24725
		public static float leftThumbProximalSpread;

		// Token: 0x04006096 RID: 24726
		public static float leftThumbProximalTwist;

		// Token: 0x04006097 RID: 24727
		public static float leftThumbMiddleBend;

		// Token: 0x04006098 RID: 24728
		public static float leftThumbDistalBend;

		// Token: 0x04006099 RID: 24729
		public static float leftIndexProximalBend;

		// Token: 0x0400609A RID: 24730
		public static float leftIndexProximalSpread;

		// Token: 0x0400609B RID: 24731
		public static float leftIndexProximalTwist;

		// Token: 0x0400609C RID: 24732
		public static float leftIndexMiddleBend;

		// Token: 0x0400609D RID: 24733
		public static float leftIndexDistalBend;

		// Token: 0x0400609E RID: 24734
		public static float leftMiddleProximalBend;

		// Token: 0x0400609F RID: 24735
		public static float leftMiddleProximalSpread;

		// Token: 0x040060A0 RID: 24736
		public static float leftMiddleProximalTwist;

		// Token: 0x040060A1 RID: 24737
		public static float leftMiddleMiddleBend;

		// Token: 0x040060A2 RID: 24738
		public static float leftMiddleDistalBend;

		// Token: 0x040060A3 RID: 24739
		public static float leftRingProximalBend;

		// Token: 0x040060A4 RID: 24740
		public static float leftRingProximalSpread;

		// Token: 0x040060A5 RID: 24741
		public static float leftRingProximalTwist;

		// Token: 0x040060A6 RID: 24742
		public static float leftRingMiddleBend;

		// Token: 0x040060A7 RID: 24743
		public static float leftRingDistalBend;

		// Token: 0x040060A8 RID: 24744
		public static float leftPinkyProximalBend;

		// Token: 0x040060A9 RID: 24745
		public static float leftPinkyProximalSpread;

		// Token: 0x040060AA RID: 24746
		public static float leftPinkyProximalTwist;

		// Token: 0x040060AB RID: 24747
		public static float leftPinkyMiddleBend;

		// Token: 0x040060AC RID: 24748
		public static float leftPinkyDistalBend;

		// Token: 0x040060AD RID: 24749
		public HandInput.Hand hand;

		// Token: 0x040060AE RID: 24750
		public bool ignoreInputFingerSpread;

		// Token: 0x040060AF RID: 24751
		public float fingerInputFactor = 1f;

		// Token: 0x040060B0 RID: 24752
		public float thumbInputFactor = 1f;

		// Token: 0x040060B1 RID: 24753
		public float fingerSpreadOffset;

		// Token: 0x040060B2 RID: 24754
		public float fingerBendOffset;

		// Token: 0x040060B3 RID: 24755
		public float thumbSpreadOffset;

		// Token: 0x040060B4 RID: 24756
		public float thumbBendOffset;

		// Token: 0x040060B5 RID: 24757
		public FingerInput thumbProximal;

		// Token: 0x040060B6 RID: 24758
		public FingerInput thumbMiddle;

		// Token: 0x040060B7 RID: 24759
		public FingerInput thumbDistal;

		// Token: 0x040060B8 RID: 24760
		public FingerInput indexProximal;

		// Token: 0x040060B9 RID: 24761
		public FingerInput indexMiddle;

		// Token: 0x040060BA RID: 24762
		public FingerInput indexDistal;

		// Token: 0x040060BB RID: 24763
		public FingerInput middleProximal;

		// Token: 0x040060BC RID: 24764
		public FingerInput middleMiddle;

		// Token: 0x040060BD RID: 24765
		public FingerInput middleDistal;

		// Token: 0x040060BE RID: 24766
		public FingerInput ringProximal;

		// Token: 0x040060BF RID: 24767
		public FingerInput ringMiddle;

		// Token: 0x040060C0 RID: 24768
		public FingerInput ringDistal;

		// Token: 0x040060C1 RID: 24769
		public FingerInput pinkyProximal;

		// Token: 0x040060C2 RID: 24770
		public FingerInput pinkyMiddle;

		// Token: 0x040060C3 RID: 24771
		public FingerInput pinkyDistal;

		// Token: 0x02000E20 RID: 3616
		public enum Hand
		{
			// Token: 0x040060C5 RID: 24773
			Left,
			// Token: 0x040060C6 RID: 24774
			Right
		}
	}
}
