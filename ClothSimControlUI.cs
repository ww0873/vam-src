using System;
using UnityEngine.UI;

// Token: 0x02000D54 RID: 3412
public class ClothSimControlUI : UIProvider
{
	// Token: 0x060068EF RID: 26863 RVA: 0x0027422D File Offset: 0x0027262D
	public ClothSimControlUI()
	{
	}

	// Token: 0x040059AA RID: 22954
	public Toggle simEnabledToggle;

	// Token: 0x040059AB RID: 22955
	public Toggle integrateEnabledToggle;

	// Token: 0x040059AC RID: 22956
	public Toggle collisionEnabledToggle;

	// Token: 0x040059AD RID: 22957
	public Slider collisionRadiusSlider;

	// Token: 0x040059AE RID: 22958
	public Slider iterationsSlider;

	// Token: 0x040059AF RID: 22959
	public Slider dragSlider;

	// Token: 0x040059B0 RID: 22960
	public Slider weightSlider;

	// Token: 0x040059B1 RID: 22961
	public Slider distanceScaleSlider;

	// Token: 0x040059B2 RID: 22962
	public Slider stiffnessSlider;

	// Token: 0x040059B3 RID: 22963
	public Slider compressionResistanceSlider;

	// Token: 0x040059B4 RID: 22964
	public Slider frictionSlider;

	// Token: 0x040059B5 RID: 22965
	public Slider staticMultiplierSlider;

	// Token: 0x040059B6 RID: 22966
	public Slider collisionPowerSlider;

	// Token: 0x040059B7 RID: 22967
	public Slider gravityMultiplierSlider;

	// Token: 0x040059B8 RID: 22968
	public Toggle allowDetachToggle;

	// Token: 0x040059B9 RID: 22969
	public Slider detachThresholdSlider;

	// Token: 0x040059BA RID: 22970
	public Slider jointStrengthSlider;

	// Token: 0x040059BB RID: 22971
	public Slider forceXSlider;

	// Token: 0x040059BC RID: 22972
	public Slider forceYSlider;

	// Token: 0x040059BD RID: 22973
	public Slider forceZSlider;

	// Token: 0x040059BE RID: 22974
	public Button resetButton;
}
