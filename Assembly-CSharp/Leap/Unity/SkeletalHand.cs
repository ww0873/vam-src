using System;
using UnityEngine;

namespace Leap.Unity
{
	// Token: 0x020006F0 RID: 1776
	public class SkeletalHand : HandModel
	{
		// Token: 0x06002B09 RID: 11017 RVA: 0x000E8274 File Offset: 0x000E6674
		public SkeletalHand()
		{
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06002B0A RID: 11018 RVA: 0x000E827C File Offset: 0x000E667C
		public override ModelType HandModelType
		{
			get
			{
				return ModelType.Graphics;
			}
		}

		// Token: 0x06002B0B RID: 11019 RVA: 0x000E8280 File Offset: 0x000E6680
		private void Start()
		{
			Utils.IgnoreCollisions(base.gameObject, base.gameObject, true);
			for (int i = 0; i < this.fingers.Length; i++)
			{
				if (this.fingers[i] != null)
				{
					this.fingers[i].fingerType = (Finger.FingerType)i;
				}
			}
		}

		// Token: 0x06002B0C RID: 11020 RVA: 0x000E82D9 File Offset: 0x000E66D9
		public override void UpdateHand()
		{
			this.SetPositions();
		}

		// Token: 0x06002B0D RID: 11021 RVA: 0x000E82E4 File Offset: 0x000E66E4
		protected Vector3 GetPalmCenter()
		{
			Vector3 b = 0.015f * Vector3.Scale(base.GetPalmDirection(), base.transform.lossyScale);
			return base.GetPalmPosition() - b;
		}

		// Token: 0x06002B0E RID: 11022 RVA: 0x000E8320 File Offset: 0x000E6720
		protected void SetPositions()
		{
			for (int i = 0; i < this.fingers.Length; i++)
			{
				if (this.fingers[i] != null)
				{
					this.fingers[i].UpdateFinger();
				}
			}
			if (this.palm != null)
			{
				this.palm.position = this.GetPalmCenter();
				this.palm.rotation = base.GetPalmRotation();
			}
			if (this.wristJoint != null)
			{
				this.wristJoint.position = base.GetWristPosition();
				this.wristJoint.rotation = base.GetPalmRotation();
			}
			if (this.forearm != null)
			{
				this.forearm.position = base.GetArmCenter();
				this.forearm.rotation = base.GetArmRotation();
			}
		}

		// Token: 0x040022D9 RID: 8921
		protected const float PALM_CENTER_OFFSET = 0.015f;
	}
}
