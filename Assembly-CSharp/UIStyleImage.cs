using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000E05 RID: 3589
public class UIStyleImage : UIStyle
{
	// Token: 0x06006ED7 RID: 28375 RVA: 0x00299428 File Offset: 0x00297828
	public UIStyleImage()
	{
	}

	// Token: 0x06006ED8 RID: 28376 RVA: 0x00299450 File Offset: 0x00297850
	public override List<UnityEngine.Object> GetControlledObjects()
	{
		List<UnityEngine.Object> list = new List<UnityEngine.Object>();
		list.Add(this);
		Image component = base.GetComponent<Image>();
		if (component != null)
		{
			list.Add(component);
		}
		return list;
	}

	// Token: 0x06006ED9 RID: 28377 RVA: 0x00299488 File Offset: 0x00297888
	public override List<UnityEngine.Object> UpdateStyle()
	{
		Image component = base.GetComponent<Image>();
		if (component != null)
		{
			component.color = this.color;
		}
		return this.GetControlledObjects();
	}

	// Token: 0x04005FEC RID: 24556
	public Color color = new Color(1f, 1f, 1f, 1f);
}
