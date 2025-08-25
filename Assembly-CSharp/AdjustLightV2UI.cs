using System;
using UnityEngine.UI;

// Token: 0x02000D0B RID: 3339
public class AdjustLightV2UI : UIProvider
{
	// Token: 0x060065E1 RID: 26081 RVA: 0x00267231 File Offset: 0x00265631
	public AdjustLightV2UI()
	{
	}

	// Token: 0x0400554E RID: 21838
	public UIPopup typeSelector;

	// Token: 0x0400554F RID: 21839
	public UIPopup renderModeSelector;

	// Token: 0x04005550 RID: 21840
	public UIPopup shadowResolutionSelector;

	// Token: 0x04005551 RID: 21841
	public Toggle onToggle;

	// Token: 0x04005552 RID: 21842
	public Toggle shadowsToggle;

	// Token: 0x04005553 RID: 21843
	public Toggle showHaloToggle;

	// Token: 0x04005554 RID: 21844
	public Toggle showDustToggle;

	// Token: 0x04005555 RID: 21845
	public Slider intensitySlider;

	// Token: 0x04005556 RID: 21846
	public Slider rangeSlider;

	// Token: 0x04005557 RID: 21847
	public Slider shadowStrengthSlider;

	// Token: 0x04005558 RID: 21848
	public Slider spotAngleSlider;

	// Token: 0x04005559 RID: 21849
	public HSVColorPicker colorPicker;

	// Token: 0x0400555A RID: 21850
	public Slider pointBiasSlider;
}
