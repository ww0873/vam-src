using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D2B RID: 3371
public class UIImageControl : JSONStorable
{
	// Token: 0x0600674E RID: 26446 RVA: 0x0026DC9B File Offset: 0x0026C09B
	public UIImageControl()
	{
	}

	// Token: 0x0600674F RID: 26447 RVA: 0x0026DCA4 File Offset: 0x0026C0A4
	public void SyncAlpha(float a)
	{
		if (this._image != null)
		{
			Color color = this._image.color;
			color.a = a;
			this._image.color = color;
		}
		else if (this._rawImage != null)
		{
			Color color2 = this._rawImage.color;
			color2.a = a;
			this._rawImage.color = color2;
		}
	}

	// Token: 0x06006750 RID: 26448 RVA: 0x0026DD18 File Offset: 0x0026C118
	public void SyncColor(float h, float s, float v)
	{
		Color color = HSVColorPicker.HSVToRGB(h, s, v);
		if (this._image != null)
		{
			color.a = this._image.color.a;
			this._image.color = color;
		}
		else if (this._rawImage != null)
		{
			color.a = this._rawImage.color.a;
			this._rawImage.color = color;
		}
	}

	// Token: 0x06006751 RID: 26449 RVA: 0x0026DDA1 File Offset: 0x0026C1A1
	protected void SyncEnableImageForBackground(bool b)
	{
		if (this.imageForBackground != null)
		{
			this.imageForBackground.enabled = b;
		}
	}

	// Token: 0x06006752 RID: 26450 RVA: 0x0026DDC0 File Offset: 0x0026C1C0
	protected void Init()
	{
		this._image = base.GetComponent<Image>();
		this._rawImage = base.GetComponent<RawImage>();
		if (this._image != null || this._rawImage != null)
		{
			Color color;
			if (this._image != null)
			{
				color = this._image.color;
			}
			else
			{
				color = this._rawImage.color;
			}
			HSVColor startingColor = HSVColorPicker.RGBToHSV(color.r, color.g, color.b);
			this.colorJSON = new JSONStorableColor("color", startingColor, new JSONStorableColor.SetHSVColorCallback(this.SyncColor));
			base.RegisterColor(this.colorJSON);
			this.alphaJSON = new JSONStorableFloat("alpha", color.a, new JSONStorableFloat.SetFloatCallback(this.SyncAlpha), 0f, 1f, true, true);
			base.RegisterFloat(this.alphaJSON);
			if (this.imageForBackground != null)
			{
				this.enableImageForBackgroundJSON = new JSONStorableBool("enableImageForBackground", this.imageForBackground.enabled, new JSONStorableBool.SetBoolCallback(this.SyncEnableImageForBackground));
				base.RegisterBool(this.enableImageForBackgroundJSON);
			}
		}
	}

	// Token: 0x06006753 RID: 26451 RVA: 0x0026DEF8 File Offset: 0x0026C2F8
	protected override void InitUI(Transform t, bool isAlt)
	{
		if (t != null && this.colorJSON != null)
		{
			UIImageControlUI componentInChildren = this.UITransform.GetComponentInChildren<UIImageControlUI>(true);
			if (componentInChildren != null)
			{
				this.colorJSON.RegisterColorPicker(componentInChildren.colorPicker, isAlt);
				this.alphaJSON.RegisterSlider(componentInChildren.alphaSlider, isAlt);
				if (this.enableImageForBackgroundJSON != null)
				{
					this.enableImageForBackgroundJSON.RegisterToggle(componentInChildren.enableImageForBackgroundToggle, isAlt);
				}
			}
		}
	}

	// Token: 0x06006754 RID: 26452 RVA: 0x0026DF76 File Offset: 0x0026C376
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			this.Init();
			this.InitUI();
			this.InitUIAlt();
		}
	}

	// Token: 0x0400584B RID: 22603
	public Image imageForBackground;

	// Token: 0x0400584C RID: 22604
	protected Image _image;

	// Token: 0x0400584D RID: 22605
	protected RawImage _rawImage;

	// Token: 0x0400584E RID: 22606
	protected JSONStorableFloat alphaJSON;

	// Token: 0x0400584F RID: 22607
	protected JSONStorableColor colorJSON;

	// Token: 0x04005850 RID: 22608
	protected JSONStorableBool enableImageForBackgroundJSON;
}
