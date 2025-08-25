using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D30 RID: 3376
public class UITextControl : JSONStorable
{
	// Token: 0x0600675F RID: 26463 RVA: 0x0026E1C9 File Offset: 0x0026C5C9
	public UITextControl()
	{
	}

	// Token: 0x06006760 RID: 26464 RVA: 0x0026E1D4 File Offset: 0x0026C5D4
	public void SyncAlpha(float a)
	{
		if (this._text != null)
		{
			Color color = this._text.color;
			color.a = a;
			this._text.color = color;
		}
	}

	// Token: 0x06006761 RID: 26465 RVA: 0x0026E214 File Offset: 0x0026C614
	public void SyncColor(float h, float s, float v)
	{
		if (this._text != null)
		{
			Color color = HSVColorPicker.HSVToRGB(h, s, v);
			color.a = this._text.color.a;
			this._text.color = color;
		}
	}

	// Token: 0x06006762 RID: 26466 RVA: 0x0026E264 File Offset: 0x0026C664
	protected void Init()
	{
		this._text = base.GetComponent<Text>();
		if (this._text != null)
		{
			Color color = this._text.color;
			HSVColor startingColor = HSVColorPicker.RGBToHSV(color.r, color.g, color.b);
			this.colorJSON = new JSONStorableColor("color", startingColor, new JSONStorableColor.SetHSVColorCallback(this.SyncColor));
			base.RegisterColor(this.colorJSON);
			this.alphaJSON = new JSONStorableFloat("alpha", color.a, new JSONStorableFloat.SetFloatCallback(this.SyncAlpha), 0f, 1f, true, true);
			base.RegisterFloat(this.alphaJSON);
		}
	}

	// Token: 0x06006763 RID: 26467 RVA: 0x0026E31C File Offset: 0x0026C71C
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			UITextControlUI componentInChildren = this.UITransform.GetComponentInChildren<UITextControlUI>(true);
			if (componentInChildren != null)
			{
				if (this.colorJSON != null)
				{
					this.colorJSON.colorPicker = componentInChildren.colorPicker;
				}
				if (this.alphaJSON != null)
				{
					this.alphaJSON.slider = componentInChildren.alphaSlider;
				}
			}
		}
	}

	// Token: 0x06006764 RID: 26468 RVA: 0x0026E38C File Offset: 0x0026C78C
	public override void InitUIAlt()
	{
		if (this.UITransformAlt != null)
		{
			UITextControlUI componentInChildren = this.UITransformAlt.GetComponentInChildren<UITextControlUI>(true);
			if (componentInChildren != null)
			{
				if (this.colorJSON != null)
				{
					this.colorJSON.colorPickerAlt = componentInChildren.colorPicker;
				}
				if (this.alphaJSON != null)
				{
					this.alphaJSON.sliderAlt = componentInChildren.alphaSlider;
				}
			}
		}
	}

	// Token: 0x06006765 RID: 26469 RVA: 0x0026E3FB File Offset: 0x0026C7FB
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

	// Token: 0x0400585A RID: 22618
	protected Text _text;

	// Token: 0x0400585B RID: 22619
	protected JSONStorableFloat alphaJSON;

	// Token: 0x0400585C RID: 22620
	protected JSONStorableColor colorJSON;
}
