using System;
using UnityEngine.UI;

// Token: 0x02000D82 RID: 3458
public class PhysicsMaterialControlUI : UIProvider
{
	// Token: 0x06006A8B RID: 27275 RVA: 0x00282F59 File Offset: 0x00281359
	public PhysicsMaterialControlUI()
	{
	}

	// Token: 0x04005C86 RID: 23686
	public Slider dynamicFrictionSlider;

	// Token: 0x04005C87 RID: 23687
	public Slider staticFrictionSlider;

	// Token: 0x04005C88 RID: 23688
	public Slider bouncinessSlider;

	// Token: 0x04005C89 RID: 23689
	public UIPopup frictionCombinePopup;

	// Token: 0x04005C8A RID: 23690
	public UIPopup bounceCombinePopup;
}
