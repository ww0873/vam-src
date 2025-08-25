using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E07 RID: 3591
public class UIStyleSlider : UIStyle
{
	// Token: 0x06006EDD RID: 28381 RVA: 0x00299670 File Offset: 0x00297A70
	public UIStyleSlider()
	{
	}

	// Token: 0x06006EDE RID: 28382 RVA: 0x002996F8 File Offset: 0x00297AF8
	public override List<UnityEngine.Object> GetControlledObjects()
	{
		List<UnityEngine.Object> list = new List<UnityEngine.Object>();
		list.Add(this);
		Slider component = base.GetComponent<Slider>();
		if (component != null)
		{
			list.Add(component);
		}
		return list;
	}

	// Token: 0x06006EDF RID: 28383 RVA: 0x00299730 File Offset: 0x00297B30
	public override List<UnityEngine.Object> UpdateStyle()
	{
		Slider component = base.GetComponent<Slider>();
		if (component != null)
		{
			ColorBlock colors = component.colors;
			colors.normalColor = this.normalColor;
			colors.highlightedColor = this.highlightedColor;
			colors.pressedColor = this.pressedColor;
			colors.disabledColor = this.disabledColor;
			colors.colorMultiplier = this.colorMultiplier;
			component.colors = colors;
		}
		return this.GetControlledObjects();
	}

	// Token: 0x04005FED RID: 24557
	public Color normalColor = Color.white;

	// Token: 0x04005FEE RID: 24558
	public Color highlightedColor = new Color(0.7f, 0.7f, 0.7f, 1f);

	// Token: 0x04005FEF RID: 24559
	public Color pressedColor = new Color(0.5f, 0.5f, 0.5f, 1f);

	// Token: 0x04005FF0 RID: 24560
	public Color disabledColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

	// Token: 0x04005FF1 RID: 24561
	public float colorMultiplier = 1f;
}
