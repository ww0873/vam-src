using System;
using UnityEngine.UI;

// Token: 0x02000AC0 RID: 2752
public class DAZClothingItemControlUI : UIProvider
{
	// Token: 0x06004922 RID: 18722 RVA: 0x00173686 File Offset: 0x00171A86
	public DAZClothingItemControlUI()
	{
	}

	// Token: 0x0400379B RID: 14235
	public Toggle lockedToggle;

	// Token: 0x0400379C RID: 14236
	public Toggle enableJointSpringAndDamperAdjustToggle;

	// Token: 0x0400379D RID: 14237
	public Toggle enableBreastJointAdjustToggle;

	// Token: 0x0400379E RID: 14238
	public Toggle enableGluteJointAdjustToggle;

	// Token: 0x0400379F RID: 14239
	public Slider breastJointSpringAndDamperMultiplierSlider;

	// Token: 0x040037A0 RID: 14240
	public Slider gluteJointSpringAndDamperMultiplierSlider;

	// Token: 0x040037A1 RID: 14241
	public Toggle disableAnatomyToggle;

	// Token: 0x040037A2 RID: 14242
	public Toggle isRealClothingItemToggle;
}
