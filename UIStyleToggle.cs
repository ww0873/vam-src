using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E0B RID: 3595
public class UIStyleToggle : UIStyle
{
	// Token: 0x06006EE9 RID: 28393 RVA: 0x00299C0C File Offset: 0x0029800C
	public UIStyleToggle()
	{
	}

	// Token: 0x06006EEA RID: 28394 RVA: 0x00299C94 File Offset: 0x00298094
	public override List<UnityEngine.Object> GetControlledObjects()
	{
		List<UnityEngine.Object> list = new List<UnityEngine.Object>();
		list.Add(this);
		Toggle component = base.GetComponent<Toggle>();
		if (component != null)
		{
			list.Add(component);
		}
		return list;
	}

	// Token: 0x06006EEB RID: 28395 RVA: 0x00299CCC File Offset: 0x002980CC
	public override List<UnityEngine.Object> UpdateStyle()
	{
		Toggle component = base.GetComponent<Toggle>();
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

	// Token: 0x04005FF5 RID: 24565
	public Color normalColor = Color.white;

	// Token: 0x04005FF6 RID: 24566
	public Color highlightedColor = new Color(0.7f, 0.7f, 0.7f, 1f);

	// Token: 0x04005FF7 RID: 24567
	public Color pressedColor = new Color(0.5f, 0.5f, 0.5f, 1f);

	// Token: 0x04005FF8 RID: 24568
	public Color disabledColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);

	// Token: 0x04005FF9 RID: 24569
	public float colorMultiplier = 1f;
}
