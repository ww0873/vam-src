using System;
using UnityEngine.UI;

namespace MeshVR
{
	// Token: 0x02000D94 RID: 3476
	public class WindControlUI : UIProvider
	{
		// Token: 0x06006B2E RID: 27438 RVA: 0x002856E3 File Offset: 0x00283AE3
		public WindControlUI()
		{
		}

		// Token: 0x04005D03 RID: 23811
		public Toggle isGlobalToggle;

		// Token: 0x04005D04 RID: 23812
		public UIPopup atomPopup;

		// Token: 0x04005D05 RID: 23813
		public UIPopup receiverPopup;

		// Token: 0x04005D06 RID: 23814
		public UIPopup receiverTargetPopup;

		// Token: 0x04005D07 RID: 23815
		public Slider currentMagnitudeSlider;

		// Token: 0x04005D08 RID: 23816
		public Toggle autoToggle;

		// Token: 0x04005D09 RID: 23817
		public Slider periodSlider;

		// Token: 0x04005D0A RID: 23818
		public Slider quicknessSlider;

		// Token: 0x04005D0B RID: 23819
		public Slider lowerMagnitudeSlider;

		// Token: 0x04005D0C RID: 23820
		public Slider upperMagnitudeSlider;

		// Token: 0x04005D0D RID: 23821
		public Slider targetMagnitudeSlider;
	}
}
