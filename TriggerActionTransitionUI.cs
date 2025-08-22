using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DAF RID: 3503
public class TriggerActionTransitionUI : TriggerActionUI
{
	// Token: 0x06006CAF RID: 27823 RVA: 0x0029048B File Offset: 0x0028E88B
	public TriggerActionTransitionUI()
	{
	}

	// Token: 0x04005E38 RID: 24120
	public Button copyButton;

	// Token: 0x04005E39 RID: 24121
	public Button pasteButton;

	// Token: 0x04005E3A RID: 24122
	public Slider testTransitionSlider;

	// Token: 0x04005E3B RID: 24123
	public Toggle startWithCurrentValToggle;

	// Token: 0x04005E3C RID: 24124
	public Slider startValSlider;

	// Token: 0x04005E3D RID: 24125
	public UIDynamicSlider startValDynamicSlider;

	// Token: 0x04005E3E RID: 24126
	public Slider endValSlider;

	// Token: 0x04005E3F RID: 24127
	public UIDynamicSlider endValDynamicSlider;

	// Token: 0x04005E40 RID: 24128
	public RectTransform startColorPickerContainer;

	// Token: 0x04005E41 RID: 24129
	public HSVColorPicker startColorPicker;

	// Token: 0x04005E42 RID: 24130
	public RectTransform endColorPickerContainer;

	// Token: 0x04005E43 RID: 24131
	public HSVColorPicker endColorPicker;
}
