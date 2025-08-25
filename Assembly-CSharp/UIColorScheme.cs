using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DEA RID: 3562
public class UIColorScheme : MonoBehaviour
{
	// Token: 0x06006E24 RID: 28196 RVA: 0x00295858 File Offset: 0x00293C58
	public UIColorScheme()
	{
	}

	// Token: 0x06006E25 RID: 28197 RVA: 0x00295A40 File Offset: 0x00293E40
	public void UpdateColors()
	{
		foreach (Image image in base.GetComponentsInChildren<Image>(true))
		{
			if (!false)
			{
				image.color = this.imageColor;
				image.SetAllDirty();
			}
		}
		foreach (Button button in base.GetComponentsInChildren<Button>(true))
		{
			bool flag = false;
			bool flag2 = false;
			if (!flag)
			{
				ColorBlock colors = button.colors;
				colors.normalColor = this.buttonNormalColor;
				colors.highlightedColor = this.buttonHighlightedColor;
				colors.pressedColor = this.buttonPressedColor;
				colors.disabledColor = this.buttonDisabledColor;
				colors.colorMultiplier = this.buttonColorMultiplier;
				button.colors = colors;
			}
			Image component = button.GetComponent<Image>();
			if (!flag2 && component != null)
			{
				component.color = this.buttonImageColor;
				component.SetAllDirty();
			}
		}
		foreach (Toggle toggle in base.GetComponentsInChildren<Toggle>(true))
		{
			if (!false)
			{
				ColorBlock colors2 = toggle.colors;
				colors2.normalColor = this.toggleNormalColor;
				colors2.highlightedColor = this.toggleHighlightedColor;
				colors2.pressedColor = this.togglePressedColor;
				colors2.disabledColor = this.toggleDisabledColor;
				colors2.colorMultiplier = this.toggleColorMultiplier;
				toggle.colors = colors2;
			}
		}
		foreach (Slider slider in base.GetComponentsInChildren<Slider>(true))
		{
			bool flag3 = false;
			bool flag4 = false;
			if (!flag3)
			{
				ColorBlock colors3 = slider.colors;
				colors3.normalColor = this.toggleNormalColor;
				colors3.highlightedColor = this.toggleHighlightedColor;
				colors3.pressedColor = this.togglePressedColor;
				colors3.disabledColor = this.toggleDisabledColor;
				colors3.colorMultiplier = this.toggleColorMultiplier;
				slider.colors = colors3;
			}
			Image component2 = slider.GetComponent<Image>();
			if (!flag4 && component2 != null)
			{
				component2.color = this.sliderImageColor;
				component2.SetAllDirty();
			}
		}
		foreach (Text text in base.GetComponentsInChildren<Text>(true))
		{
			if (!false)
			{
				text.color = this.textColor;
			}
		}
	}

	// Token: 0x04005F50 RID: 24400
	public Color imageColor = new Color(1f, 1f, 1f, 1f);

	// Token: 0x04005F51 RID: 24401
	public Color buttonNormalColor = Color.white;

	// Token: 0x04005F52 RID: 24402
	public Color buttonHighlightedColor = new Color(0.7f, 0.7f, 0.7f, 1f);

	// Token: 0x04005F53 RID: 24403
	public Color buttonPressedColor = new Color(0.5f, 0.5f, 0.5f, 1f);

	// Token: 0x04005F54 RID: 24404
	public Color buttonDisabledColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

	// Token: 0x04005F55 RID: 24405
	public Color buttonImageColor = new Color(0.7f, 0.7f, 0.7f, 1f);

	// Token: 0x04005F56 RID: 24406
	public float buttonColorMultiplier = 1f;

	// Token: 0x04005F57 RID: 24407
	public Color toggleNormalColor = Color.white;

	// Token: 0x04005F58 RID: 24408
	public Color toggleHighlightedColor = new Color(0.7f, 0.7f, 0.7f, 1f);

	// Token: 0x04005F59 RID: 24409
	public Color togglePressedColor = new Color(0.5f, 0.5f, 0.5f, 1f);

	// Token: 0x04005F5A RID: 24410
	public Color toggleDisabledColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

	// Token: 0x04005F5B RID: 24411
	public float toggleColorMultiplier = 1f;

	// Token: 0x04005F5C RID: 24412
	public Color sliderNormalColor = Color.white;

	// Token: 0x04005F5D RID: 24413
	public Color sliderHighlightedColor = new Color(0.7f, 0.7f, 0.7f, 1f);

	// Token: 0x04005F5E RID: 24414
	public Color sliderPressedColor = new Color(0.5f, 0.5f, 0.5f, 1f);

	// Token: 0x04005F5F RID: 24415
	public Color sliderDisabledColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

	// Token: 0x04005F60 RID: 24416
	public Color sliderImageColor = new Color(0f, 0f, 0f, 0.5f);

	// Token: 0x04005F61 RID: 24417
	public float sliderColorMultiplier = 1f;

	// Token: 0x04005F62 RID: 24418
	public Color textColor = new Color(0.2f, 0.2f, 0.2f, 1f);
}
