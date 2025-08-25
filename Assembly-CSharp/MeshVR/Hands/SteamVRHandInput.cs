using System;
using Valve.VR;

namespace MeshVR.Hands
{
	// Token: 0x02000E2B RID: 3627
	public class SteamVRHandInput : HandInput
	{
		// Token: 0x06006F9C RID: 28572 RVA: 0x002A0F2E File Offset: 0x0029F32E
		public SteamVRHandInput()
		{
		}

		// Token: 0x06006F9D RID: 28573 RVA: 0x002A0F38 File Offset: 0x0029F338
		protected override void Update()
		{
			if (this.skeleton != null && this.skeleton.skeletonAvailable)
			{
				base.Update();
			}
			else if (this.hand == HandInput.Hand.Left)
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

		// Token: 0x06006F9E RID: 28574 RVA: 0x002A1387 File Offset: 0x0029F787
		protected override void Awake()
		{
			base.Awake();
			this.skeleton = base.GetComponent<SteamVR_Behaviour_Skeleton>();
		}

		// Token: 0x04006158 RID: 24920
		protected SteamVR_Behaviour_Skeleton skeleton;

		// Token: 0x04006159 RID: 24921
		public float thumbProximalBend;

		// Token: 0x0400615A RID: 24922
		public float thumbProximalSpread;

		// Token: 0x0400615B RID: 24923
		public float thumbProximalTwist;

		// Token: 0x0400615C RID: 24924
		public float thumbMiddleBend;

		// Token: 0x0400615D RID: 24925
		public float thumbDistalBend;

		// Token: 0x0400615E RID: 24926
		public float indexProximalBend;

		// Token: 0x0400615F RID: 24927
		public float indexProximalSpread;

		// Token: 0x04006160 RID: 24928
		public float indexProximalTwist;

		// Token: 0x04006161 RID: 24929
		public float indexMiddleBend;

		// Token: 0x04006162 RID: 24930
		public float indexDistalBend;

		// Token: 0x04006163 RID: 24931
		public float middleProximalBend;

		// Token: 0x04006164 RID: 24932
		public float middleProximalSpread;

		// Token: 0x04006165 RID: 24933
		public float middleProximalTwist;

		// Token: 0x04006166 RID: 24934
		public float middleMiddleBend;

		// Token: 0x04006167 RID: 24935
		public float middleDistalBend;

		// Token: 0x04006168 RID: 24936
		public float ringProximalBend;

		// Token: 0x04006169 RID: 24937
		public float ringProximalSpread;

		// Token: 0x0400616A RID: 24938
		public float ringProximalTwist;

		// Token: 0x0400616B RID: 24939
		public float ringMiddleBend;

		// Token: 0x0400616C RID: 24940
		public float ringDistalBend;

		// Token: 0x0400616D RID: 24941
		public float pinkyProximalBend;

		// Token: 0x0400616E RID: 24942
		public float pinkyProximalSpread;

		// Token: 0x0400616F RID: 24943
		public float pinkyProximalTwist;

		// Token: 0x04006170 RID: 24944
		public float pinkyMiddleBend;

		// Token: 0x04006171 RID: 24945
		public float pinkyDistalBend;

		// Token: 0x04006172 RID: 24946
		public float currentThumbValue;

		// Token: 0x04006173 RID: 24947
		public float currentFingerValue;
	}
}
