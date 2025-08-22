using System;
using UnityEngine.UI;

namespace MeshVR
{
	// Token: 0x02000C1D RID: 3101
	public class HandModelControlUI : UIProvider
	{
		// Token: 0x06005A3E RID: 23102 RVA: 0x00212B92 File Offset: 0x00210F92
		public HandModelControlUI()
		{
		}

		// Token: 0x04004A6C RID: 19052
		public UIPopup leftHandChooserPopup;

		// Token: 0x04004A6D RID: 19053
		public UIPopup rightHandChooserPopup;

		// Token: 0x04004A6E RID: 19054
		public Toggle leftHandEnabledToggle;

		// Token: 0x04004A6F RID: 19055
		public Toggle rightHandEnabledToggle;

		// Token: 0x04004A70 RID: 19056
		public Toggle linkHandsToggle;

		// Token: 0x04004A71 RID: 19057
		public Toggle useCollisionToggle;

		// Token: 0x04004A72 RID: 19058
		public Slider xPositionSlider;

		// Token: 0x04004A73 RID: 19059
		public Slider yPositionSlider;

		// Token: 0x04004A74 RID: 19060
		public Slider zPositionSlider;

		// Token: 0x04004A75 RID: 19061
		public Slider xRotationSlider;

		// Token: 0x04004A76 RID: 19062
		public Slider yRotationSlider;

		// Token: 0x04004A77 RID: 19063
		public Slider zRotationSlider;
	}
}
