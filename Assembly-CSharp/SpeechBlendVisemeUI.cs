using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B87 RID: 2951
public class SpeechBlendVisemeUI : MonoBehaviour
{
	// Token: 0x06005320 RID: 21280 RVA: 0x001E0DF5 File Offset: 0x001DF1F5
	public SpeechBlendVisemeUI()
	{
	}

	// Token: 0x04004300 RID: 17152
	public Text visemeNameText;

	// Token: 0x04004301 RID: 17153
	public GameObject visemeFoundIndicator;

	// Token: 0x04004302 RID: 17154
	public GameObject visemeFoundNegativeIndicator;

	// Token: 0x04004303 RID: 17155
	public InputField visemeMorphUidInputField;

	// Token: 0x04004304 RID: 17156
	public Button pasteMorphUidButton;

	// Token: 0x04004305 RID: 17157
	public Slider visemeWeightSlider;

	// Token: 0x04004306 RID: 17158
	public Slider visemeRawValueSlider;

	// Token: 0x04004307 RID: 17159
	public Slider visemeValueSlider;
}
