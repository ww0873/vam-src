using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B86 RID: 2950
public class SpeechBlendControlUI : UIProvider
{
	// Token: 0x0600531F RID: 21279 RVA: 0x001E0DED File Offset: 0x001DF1ED
	public SpeechBlendControlUI()
	{
	}

	// Token: 0x040042E8 RID: 17128
	public Toggle enabledToggle;

	// Token: 0x040042E9 RID: 17129
	public Slider volumeSlider;

	// Token: 0x040042EA RID: 17130
	public Slider maxVolumeSlider;

	// Token: 0x040042EB RID: 17131
	public Slider volumeMultiplierSlider;

	// Token: 0x040042EC RID: 17132
	public Slider volumeClampSlider;

	// Token: 0x040042ED RID: 17133
	public Slider volumeThresholdSlider;

	// Token: 0x040042EE RID: 17134
	public Slider mouthOpenFactorSlider;

	// Token: 0x040042EF RID: 17135
	public Slider mouthOpenChangeRateSlider;

	// Token: 0x040042F0 RID: 17136
	public Slider visemeDetectionFactorSlider;

	// Token: 0x040042F1 RID: 17137
	public Slider visemeThresholdSlider;

	// Token: 0x040042F2 RID: 17138
	public Slider timeBetweenSamplingSlider;

	// Token: 0x040042F3 RID: 17139
	public Slider sampleTimeAdjustSlider;

	// Token: 0x040042F4 RID: 17140
	public Slider visemeMorphChangeRateSlider;

	// Token: 0x040042F5 RID: 17141
	public Slider visemeMorphClampSlider;

	// Token: 0x040042F6 RID: 17142
	public UIPopup voiceTypePopup;

	// Token: 0x040042F7 RID: 17143
	public UIPopup morphSetPopup;

	// Token: 0x040042F8 RID: 17144
	public GameObject advancedPanel;

	// Token: 0x040042F9 RID: 17145
	public Button openAdvancedPanelButton;

	// Token: 0x040042FA RID: 17146
	public GameObject mouthOpenVisemeFoundIndicator;

	// Token: 0x040042FB RID: 17147
	public GameObject mouthOpenVisemeFoundNegativeIndicator;

	// Token: 0x040042FC RID: 17148
	public InputField mouthOpenMorphUidInputField;

	// Token: 0x040042FD RID: 17149
	public Slider mouthOpenVisemeValueSlider;

	// Token: 0x040042FE RID: 17150
	public Button pasteMouthOpenMorphUidButton;

	// Token: 0x040042FF RID: 17151
	public Button resetAllAdvancedSettingsButton;
}
