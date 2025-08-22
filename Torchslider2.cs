using System;
using UnityEngine;

// Token: 0x02000419 RID: 1049
public class Torchslider2 : MonoBehaviour
{
	// Token: 0x06001A72 RID: 6770 RVA: 0x00093DB0 File Offset: 0x000921B0
	public Torchslider2()
	{
	}

	// Token: 0x06001A73 RID: 6771 RVA: 0x00093DB8 File Offset: 0x000921B8
	private void OnGUI()
	{
		GUI.Label(new Rect(25f, 25f, 150f, 30f), "Light Intensity", this.SkinSlider.label);
		this.Intensity_Light = GUI.HorizontalSlider(new Rect(25f, 50f, 150f, 30f), this.Intensity_Light, 0f, this.TorcheObj.GetComponent<Torchelight>().MaxLightIntensity, this.SkinSlider.horizontalSlider, this.SkinSlider.horizontalSliderThumb);
		this.CameraRendering = GUI.Toggle(new Rect(25f, 80f, 150f, 30f), this.CameraRendering, "Deferred lighting");
		if (this.CameraRendering)
		{
			this.MainCamera.renderingPath = RenderingPath.DeferredLighting;
		}
		else
		{
			this.MainCamera.renderingPath = RenderingPath.Forward;
		}
	}

	// Token: 0x06001A74 RID: 6772 RVA: 0x00093EA0 File Offset: 0x000922A0
	private void Update()
	{
		foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("TagLight"))
		{
			gameObject.GetComponent<Torchelight>().IntensityLight = this.Intensity_Light;
		}
	}

	// Token: 0x0400157D RID: 5501
	public GameObject TorcheObj;

	// Token: 0x0400157E RID: 5502
	public Camera MainCamera;

	// Token: 0x0400157F RID: 5503
	public GUISkin SkinSlider;

	// Token: 0x04001580 RID: 5504
	private GameObject Torch;

	// Token: 0x04001581 RID: 5505
	private float Intensity_Light;

	// Token: 0x04001582 RID: 5506
	private bool CameraRendering;
}
