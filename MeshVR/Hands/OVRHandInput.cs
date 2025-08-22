using System;

namespace MeshVR.Hands
{
	// Token: 0x02000E28 RID: 3624
	public class OVRHandInput : HandInput
	{
		// Token: 0x06006F8E RID: 28558 RVA: 0x002A06CA File Offset: 0x0029EACA
		public OVRHandInput()
		{
		}

		// Token: 0x06006F8F RID: 28559 RVA: 0x002A06D4 File Offset: 0x0029EAD4
		protected override void Update()
		{
			if (this.hand == HandInput.Hand.Left)
			{
				float num = SuperController.singleton.GetLeftGrabVal() * this.thumbInputFactor;
				this.currentThumbValue = num;
				HandInput.leftThumbProximalBend = this.thumbProximalBend * num + this.thumbBendOffset;
				HandInput.leftThumbProximalSpread = this.thumbProximalSpread * num + this.thumbSpreadOffset;
				HandInput.leftThumbProximalTwist = this.thumbProximalTwist * num;
				HandInput.leftThumbMiddleBend = this.thumbMiddleBend * num + this.thumbBendOffset;
				HandInput.leftThumbDistalBend = this.thumbDistalBend * num + this.thumbBendOffset;
				num = SuperController.singleton.GetLeftGrabVal() * this.fingerInputFactor;
				this.currentFingerValue = num;
				HandInput.leftIndexProximalBend = this.indexProximalBend * num + this.fingerBendOffset;
				HandInput.leftIndexProximalSpread = this.indexProximalSpread * num + this.fingerSpreadOffset;
				HandInput.leftIndexProximalTwist = this.indexProximalTwist * num;
				HandInput.leftIndexMiddleBend = this.indexMiddleBend * num + this.fingerBendOffset;
				HandInput.leftIndexDistalBend = this.indexDistalBend * num + this.fingerBendOffset;
				HandInput.leftMiddleProximalBend = this.middleProximalBend * num + this.fingerBendOffset;
				HandInput.leftMiddleProximalSpread = this.middleProximalSpread * num + this.fingerSpreadOffset;
				HandInput.leftMiddleProximalTwist = this.middleProximalTwist * num;
				HandInput.leftMiddleMiddleBend = this.middleMiddleBend * num + this.fingerBendOffset;
				HandInput.leftMiddleDistalBend = this.middleDistalBend * num + this.fingerBendOffset;
				HandInput.leftRingProximalBend = this.ringProximalBend * num + this.fingerBendOffset;
				HandInput.leftRingProximalSpread = this.ringProximalSpread * num + this.fingerSpreadOffset;
				HandInput.leftRingProximalTwist = this.ringProximalTwist * num;
				HandInput.leftRingMiddleBend = this.ringMiddleBend * num + this.fingerBendOffset;
				HandInput.leftRingDistalBend = this.ringDistalBend * num + this.fingerBendOffset;
				HandInput.leftPinkyProximalBend = this.pinkyProximalBend * num + this.fingerBendOffset;
				HandInput.leftPinkyProximalSpread = this.pinkyProximalSpread * num + this.fingerSpreadOffset;
				HandInput.leftPinkyProximalTwist = this.pinkyProximalTwist * num;
				HandInput.leftPinkyMiddleBend = this.pinkyMiddleBend * num + this.fingerBendOffset;
				HandInput.leftPinkyDistalBend = this.pinkyDistalBend * num + this.fingerBendOffset;
			}
			else
			{
				float num2 = SuperController.singleton.GetRightGrabVal() * this.thumbInputFactor;
				this.currentThumbValue = num2;
				HandInput.rightThumbProximalBend = this.thumbProximalBend * num2 + this.thumbBendOffset;
				HandInput.rightThumbProximalSpread = this.thumbProximalSpread * num2 + this.thumbSpreadOffset;
				HandInput.rightThumbProximalTwist = this.thumbProximalTwist * num2;
				HandInput.rightThumbMiddleBend = this.thumbMiddleBend * num2 + this.thumbBendOffset;
				HandInput.rightThumbDistalBend = this.thumbDistalBend * num2 + this.thumbBendOffset;
				num2 = SuperController.singleton.GetRightGrabVal() * this.fingerInputFactor;
				this.currentFingerValue = num2;
				HandInput.rightIndexProximalBend = this.indexProximalBend * num2 + this.fingerBendOffset;
				HandInput.rightIndexProximalSpread = this.indexProximalSpread * num2 + this.fingerSpreadOffset;
				HandInput.rightIndexProximalTwist = this.indexProximalTwist * num2;
				HandInput.rightIndexMiddleBend = this.indexMiddleBend * num2 + this.fingerBendOffset;
				HandInput.rightIndexDistalBend = this.indexDistalBend * num2 + this.fingerBendOffset;
				HandInput.rightMiddleProximalBend = this.middleProximalBend * num2 + this.fingerBendOffset;
				HandInput.rightMiddleProximalSpread = this.middleProximalSpread * num2 + this.fingerSpreadOffset;
				HandInput.rightMiddleProximalTwist = this.middleProximalTwist * num2;
				HandInput.rightMiddleMiddleBend = this.middleMiddleBend * num2 + this.fingerBendOffset;
				HandInput.rightMiddleDistalBend = this.middleDistalBend * num2 + this.fingerBendOffset;
				HandInput.rightRingProximalBend = this.ringProximalBend * num2 + this.fingerBendOffset;
				HandInput.rightRingProximalSpread = this.ringProximalSpread * num2 + this.fingerSpreadOffset;
				HandInput.rightRingProximalTwist = this.ringProximalTwist * num2;
				HandInput.rightRingMiddleBend = this.ringMiddleBend * num2 + this.fingerBendOffset;
				HandInput.rightRingDistalBend = this.ringDistalBend * num2 + this.fingerBendOffset;
				HandInput.rightPinkyProximalBend = this.pinkyProximalBend * num2 + this.fingerBendOffset;
				HandInput.rightPinkyProximalSpread = this.pinkyProximalSpread * num2 + this.fingerSpreadOffset;
				HandInput.rightPinkyProximalTwist = this.pinkyProximalTwist * num2;
				HandInput.rightPinkyMiddleBend = this.pinkyMiddleBend * num2 + this.fingerBendOffset;
				HandInput.rightPinkyDistalBend = this.pinkyDistalBend * num2 + this.fingerBendOffset;
			}
		}

		// Token: 0x04006138 RID: 24888
		public float thumbProximalBend;

		// Token: 0x04006139 RID: 24889
		public float thumbProximalSpread;

		// Token: 0x0400613A RID: 24890
		public float thumbProximalTwist;

		// Token: 0x0400613B RID: 24891
		public float thumbMiddleBend;

		// Token: 0x0400613C RID: 24892
		public float thumbDistalBend;

		// Token: 0x0400613D RID: 24893
		public float indexProximalBend;

		// Token: 0x0400613E RID: 24894
		public float indexProximalSpread;

		// Token: 0x0400613F RID: 24895
		public float indexProximalTwist;

		// Token: 0x04006140 RID: 24896
		public float indexMiddleBend;

		// Token: 0x04006141 RID: 24897
		public float indexDistalBend;

		// Token: 0x04006142 RID: 24898
		public float middleProximalBend;

		// Token: 0x04006143 RID: 24899
		public float middleProximalSpread;

		// Token: 0x04006144 RID: 24900
		public float middleProximalTwist;

		// Token: 0x04006145 RID: 24901
		public float middleMiddleBend;

		// Token: 0x04006146 RID: 24902
		public float middleDistalBend;

		// Token: 0x04006147 RID: 24903
		public float ringProximalBend;

		// Token: 0x04006148 RID: 24904
		public float ringProximalSpread;

		// Token: 0x04006149 RID: 24905
		public float ringProximalTwist;

		// Token: 0x0400614A RID: 24906
		public float ringMiddleBend;

		// Token: 0x0400614B RID: 24907
		public float ringDistalBend;

		// Token: 0x0400614C RID: 24908
		public float pinkyProximalBend;

		// Token: 0x0400614D RID: 24909
		public float pinkyProximalSpread;

		// Token: 0x0400614E RID: 24910
		public float pinkyProximalTwist;

		// Token: 0x0400614F RID: 24911
		public float pinkyMiddleBend;

		// Token: 0x04006150 RID: 24912
		public float pinkyDistalBend;

		// Token: 0x04006151 RID: 24913
		public float currentThumbValue;

		// Token: 0x04006152 RID: 24914
		public float currentFingerValue;
	}
}
