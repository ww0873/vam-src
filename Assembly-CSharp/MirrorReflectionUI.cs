using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D0E RID: 3342
public class MirrorReflectionUI : UIProvider
{
	// Token: 0x06006611 RID: 26129 RVA: 0x002690EE File Offset: 0x002674EE
	public MirrorReflectionUI()
	{
	}

	// Token: 0x0400557E RID: 21886
	public Toggle disablePixelLightsToggle;

	// Token: 0x0400557F RID: 21887
	public UIPopup textureSizePopup;

	// Token: 0x04005580 RID: 21888
	public UIPopup antiAliasingPopup;

	// Token: 0x04005581 RID: 21889
	public RectTransform reflectionOpacityContainer;

	// Token: 0x04005582 RID: 21890
	public Slider reflectionOpacitySlider;

	// Token: 0x04005583 RID: 21891
	public RectTransform reflectionBlendContainer;

	// Token: 0x04005584 RID: 21892
	public Slider reflectionBlendSlider;

	// Token: 0x04005585 RID: 21893
	public RectTransform surfaceTexturePowerContainer;

	// Token: 0x04005586 RID: 21894
	public Slider surfaceTexturePowerSlider;

	// Token: 0x04005587 RID: 21895
	public RectTransform specularIntensityContainer;

	// Token: 0x04005588 RID: 21896
	public Slider specularIntensitySlider;

	// Token: 0x04005589 RID: 21897
	public HSVColorPicker reflectionColorPicker;

	// Token: 0x0400558A RID: 21898
	public Slider renderQueueSlider;
}
