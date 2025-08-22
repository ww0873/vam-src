using System;
using UnityEngine;

// Token: 0x02000418 RID: 1048
public class TorchSlider : MonoBehaviour
{
	// Token: 0x06001A70 RID: 6768 RVA: 0x00093D02 File Offset: 0x00092102
	public TorchSlider()
	{
	}

	// Token: 0x06001A71 RID: 6769 RVA: 0x00093D0C File Offset: 0x0009210C
	private void OnGUI()
	{
		GUI.Label(new Rect(25f, 25f, 150f, 30f), "Light Intensity", this.SkinSlider.label);
		this.TorcheObj.GetComponent<Torchelight>().IntensityLight = GUI.HorizontalSlider(new Rect(25f, 50f, 150f, 30f), this.TorcheObj.GetComponent<Torchelight>().IntensityLight, 0f, this.TorcheObj.GetComponent<Torchelight>().MaxLightIntensity, this.SkinSlider.horizontalSlider, this.SkinSlider.horizontalSliderThumb);
	}

	// Token: 0x0400157B RID: 5499
	public GameObject TorcheObj;

	// Token: 0x0400157C RID: 5500
	public GUISkin SkinSlider;
}
