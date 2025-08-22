using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E03 RID: 3587
public class UIStyleButton : UIStyle
{
	// Token: 0x06006ED1 RID: 28369 RVA: 0x0029910C File Offset: 0x0029750C
	public UIStyleButton()
	{
	}

	// Token: 0x06006ED2 RID: 28370 RVA: 0x00299194 File Offset: 0x00297594
	public override List<UnityEngine.Object> GetControlledObjects()
	{
		List<UnityEngine.Object> list = new List<UnityEngine.Object>();
		list.Add(this);
		Button component = base.GetComponent<Button>();
		if (component != null)
		{
			list.Add(component);
		}
		return list;
	}

	// Token: 0x06006ED3 RID: 28371 RVA: 0x002991CC File Offset: 0x002975CC
	public override List<UnityEngine.Object> UpdateStyle()
	{
		Button component = base.GetComponent<Button>();
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

	// Token: 0x04005FE7 RID: 24551
	public Color normalColor = Color.white;

	// Token: 0x04005FE8 RID: 24552
	public Color highlightedColor = new Color(0.7f, 0.7f, 0.7f, 1f);

	// Token: 0x04005FE9 RID: 24553
	public Color pressedColor = new Color(0.5f, 0.5f, 0.5f, 1f);

	// Token: 0x04005FEA RID: 24554
	public Color disabledColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

	// Token: 0x04005FEB RID: 24555
	public float colorMultiplier = 1f;
}
