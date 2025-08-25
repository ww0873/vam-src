using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E09 RID: 3593
public class UIStyleText : UIStyle
{
	// Token: 0x06006EE3 RID: 28387 RVA: 0x0029998C File Offset: 0x00297D8C
	public UIStyleText()
	{
	}

	// Token: 0x06006EE4 RID: 28388 RVA: 0x002999BC File Offset: 0x00297DBC
	public override List<UnityEngine.Object> GetControlledObjects()
	{
		List<UnityEngine.Object> list = new List<UnityEngine.Object>();
		list.Add(this);
		Text component = base.GetComponent<Text>();
		if (component != null)
		{
			list.Add(component);
		}
		return list;
	}

	// Token: 0x06006EE5 RID: 28389 RVA: 0x002999F4 File Offset: 0x00297DF4
	public override List<UnityEngine.Object> UpdateStyle()
	{
		Text component = base.GetComponent<Text>();
		if (component != null)
		{
			component.color = this.color;
			component.fontSize = this.fontSize;
			component.fontStyle = this.fontStyle;
		}
		return this.GetControlledObjects();
	}

	// Token: 0x04005FF2 RID: 24562
	public Color color = new Color(1f, 1f, 1f, 1f);

	// Token: 0x04005FF3 RID: 24563
	public FontStyle fontStyle;

	// Token: 0x04005FF4 RID: 24564
	public int fontSize = 28;
}
