using System;
using UnityEngine.UI;

// Token: 0x02000DB9 RID: 3513
public class VariableTriggerUI : UIProvider
{
	// Token: 0x06006CF1 RID: 27889 RVA: 0x00290FB0 File Offset: 0x0028F3B0
	public VariableTriggerUI()
	{
	}

	// Token: 0x04005E68 RID: 24168
	public Slider variableSlider;

	// Token: 0x04005E69 RID: 24169
	public UIPopup driverAtomPopup;

	// Token: 0x04005E6A RID: 24170
	public UIPopup driverPopup;

	// Token: 0x04005E6B RID: 24171
	public UIPopup driverTargetPopup;

	// Token: 0x04005E6C RID: 24172
	public Slider driverStartValSlider;

	// Token: 0x04005E6D RID: 24173
	public UIDynamicSlider driverStartValDynamicSlider;

	// Token: 0x04005E6E RID: 24174
	public Slider driverEndValSlider;

	// Token: 0x04005E6F RID: 24175
	public UIDynamicSlider driverEndValDynamicSlider;
}
