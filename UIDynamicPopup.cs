using System;
using UnityEngine;

// Token: 0x02000DEE RID: 3566
public class UIDynamicPopup : UIDynamic
{
	// Token: 0x06006E36 RID: 28214 RVA: 0x00295F33 File Offset: 0x00294333
	public UIDynamicPopup()
	{
	}

	// Token: 0x1700101C RID: 4124
	// (get) Token: 0x06006E37 RID: 28215 RVA: 0x00295F46 File Offset: 0x00294346
	// (set) Token: 0x06006E38 RID: 28216 RVA: 0x00295F66 File Offset: 0x00294366
	public string label
	{
		get
		{
			if (this.popup != null)
			{
				return this.popup.label;
			}
			return null;
		}
		set
		{
			if (this.popup != null)
			{
				this.popup.label = value;
			}
		}
	}

	// Token: 0x1700101D RID: 4125
	// (get) Token: 0x06006E39 RID: 28217 RVA: 0x00295F85 File Offset: 0x00294385
	// (set) Token: 0x06006E3A RID: 28218 RVA: 0x00295FA9 File Offset: 0x002943A9
	public Color labelTextColor
	{
		get
		{
			if (this.popup != null)
			{
				return this.popup.labelTextColor;
			}
			return Color.black;
		}
		set
		{
			if (this.popup != null)
			{
				this.popup.labelTextColor = value;
			}
		}
	}

	// Token: 0x1700101E RID: 4126
	// (get) Token: 0x06006E3B RID: 28219 RVA: 0x00295FC8 File Offset: 0x002943C8
	// (set) Token: 0x06006E3C RID: 28220 RVA: 0x00296024 File Offset: 0x00294424
	public float labelWidth
	{
		get
		{
			if (this.popup != null && this.popup.labelText != null)
			{
				RectTransform rectTransform = this.popup.labelText.rectTransform;
				return rectTransform.sizeDelta.x;
			}
			return 0f;
		}
		set
		{
			if (this.popup != null && this.popup.labelText != null)
			{
				float x = value + this.labelSpacingRight;
				RectTransform rectTransform = this.popup.labelText.rectTransform;
				Vector2 sizeDelta = rectTransform.sizeDelta;
				sizeDelta.x = value;
				this.popup.labelText.rectTransform.sizeDelta = sizeDelta;
				if (this.popup.topButton != null)
				{
					RectTransform component = this.popup.topButton.GetComponent<RectTransform>();
					Vector2 offsetMin = component.offsetMin;
					offsetMin.x = x;
					component.offsetMin = offsetMin;
				}
				if (this.popup.popupPanel != null)
				{
					RectTransform popupPanel = this.popup.popupPanel;
					Vector2 offsetMin2 = popupPanel.offsetMin;
					offsetMin2.x = x;
					popupPanel.offsetMin = offsetMin2;
				}
				if (this.popup.sliderControl != null)
				{
					RectTransform component2 = this.popup.sliderControl.GetComponent<RectTransform>();
					Vector2 offsetMin3 = component2.offsetMin;
					offsetMin3.x = x;
					component2.offsetMin = offsetMin3;
				}
				if (this.popup.filterField != null)
				{
					RectTransform component3 = this.popup.filterField.GetComponent<RectTransform>();
					Vector2 offsetMin4 = component3.offsetMin;
					offsetMin4.x = x;
					component3.offsetMin = offsetMin4;
				}
			}
		}
	}

	// Token: 0x1700101F RID: 4127
	// (get) Token: 0x06006E3D RID: 28221 RVA: 0x0029619C File Offset: 0x0029459C
	// (set) Token: 0x06006E3E RID: 28222 RVA: 0x002961C0 File Offset: 0x002945C0
	public float popupPanelHeight
	{
		get
		{
			if (this.popup != null)
			{
				return this.popup.popupPanelHeight;
			}
			return 0f;
		}
		set
		{
			if (this.popup != null)
			{
				this.popup.popupPanelHeight = value;
			}
		}
	}

	// Token: 0x04005F6A RID: 24426
	public UIPopup popup;

	// Token: 0x04005F6B RID: 24427
	public float labelSpacingRight = 10f;
}
