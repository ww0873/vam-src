using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000E0A RID: 3594
public class UIStyleTextMaster : UIStyleText
{
	// Token: 0x06006EE6 RID: 28390 RVA: 0x00299A3E File Offset: 0x00297E3E
	public UIStyleTextMaster()
	{
	}

	// Token: 0x06006EE7 RID: 28391 RVA: 0x00299A48 File Offset: 0x00297E48
	public override List<UnityEngine.Object> GetControlledObjects()
	{
		List<UnityEngine.Object> list = new List<UnityEngine.Object>();
		foreach (UIStyleText uistyleText in base.GetComponentsInChildren<UIStyleText>(true))
		{
			if (uistyleText == this)
			{
				list.Add(this);
			}
			else if (uistyleText.styleName == this.styleName && uistyleText.gameObject != base.gameObject)
			{
				List<UnityEngine.Object> controlledObjects = uistyleText.GetControlledObjects();
				foreach (UnityEngine.Object item in controlledObjects)
				{
					list.Add(item);
				}
			}
		}
		return list;
	}

	// Token: 0x06006EE8 RID: 28392 RVA: 0x00299B18 File Offset: 0x00297F18
	public override List<UnityEngine.Object> UpdateStyle()
	{
		List<UnityEngine.Object> list = new List<UnityEngine.Object>();
		foreach (UIStyleText uistyleText in base.GetComponentsInChildren<UIStyleText>(true))
		{
			if (!(uistyleText == this))
			{
				if (uistyleText.styleName == this.styleName && uistyleText.gameObject != base.gameObject)
				{
					uistyleText.color = this.color;
					uistyleText.fontSize = this.fontSize;
					uistyleText.fontStyle = this.fontStyle;
					List<UnityEngine.Object> list2 = uistyleText.UpdateStyle();
					foreach (UnityEngine.Object item in list2)
					{
						list.Add(item);
					}
				}
			}
		}
		list.Add(this);
		return list;
	}
}
