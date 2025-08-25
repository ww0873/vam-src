using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D11 RID: 3345
public class SkyshopLightControllerUI : MonoBehaviour
{
	// Token: 0x06006635 RID: 26165 RVA: 0x0026A12A File Offset: 0x0026852A
	public SkyshopLightControllerUI()
	{
	}

	// Token: 0x040055BE RID: 21950
	public Slider masterIntensitySlider;

	// Token: 0x040055BF RID: 21951
	public Slider diffuseIntensitySlider;

	// Token: 0x040055C0 RID: 21952
	public Slider specularIntensitySlider;

	// Token: 0x040055C1 RID: 21953
	public Slider unityAmbientIntensitySlider;

	// Token: 0x040055C2 RID: 21954
	public HSVColorPicker unityAmbientColorPicker;

	// Token: 0x040055C3 RID: 21955
	public Slider camExposureSlider;

	// Token: 0x040055C4 RID: 21956
	public Toggle showSkyboxToggle;

	// Token: 0x040055C5 RID: 21957
	public Slider skyboxIntensitySlider;

	// Token: 0x040055C6 RID: 21958
	public Slider skyboxYAngleSlider;

	// Token: 0x040055C7 RID: 21959
	public UIPopup skyPopup;
}
