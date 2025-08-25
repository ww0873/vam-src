using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000E04 RID: 3588
public class UIStyleButtonMaster : UIStyleButton
{
	// Token: 0x06006ED4 RID: 28372 RVA: 0x00299241 File Offset: 0x00297641
	public UIStyleButtonMaster()
	{
	}

	// Token: 0x06006ED5 RID: 28373 RVA: 0x0029924C File Offset: 0x0029764C
	public override List<UnityEngine.Object> GetControlledObjects()
	{
		List<UnityEngine.Object> list = new List<UnityEngine.Object>();
		foreach (UIStyleButton uistyleButton in base.GetComponentsInChildren<UIStyleButton>(true))
		{
			if (uistyleButton == this)
			{
				list.Add(this);
			}
			else if (uistyleButton.styleName == this.styleName && uistyleButton.gameObject != base.gameObject)
			{
				List<UnityEngine.Object> controlledObjects = uistyleButton.GetControlledObjects();
				foreach (UnityEngine.Object item in controlledObjects)
				{
					list.Add(item);
				}
			}
		}
		return list;
	}

	// Token: 0x06006ED6 RID: 28374 RVA: 0x0029931C File Offset: 0x0029771C
	public override List<UnityEngine.Object> UpdateStyle()
	{
		List<UnityEngine.Object> list = new List<UnityEngine.Object>();
		foreach (UIStyleButton uistyleButton in base.GetComponentsInChildren<UIStyleButton>(true))
		{
			if (uistyleButton == this)
			{
				list.Add(this);
			}
			else if (uistyleButton.styleName == this.styleName && uistyleButton.gameObject != base.gameObject)
			{
				uistyleButton.normalColor = this.normalColor;
				uistyleButton.highlightedColor = this.highlightedColor;
				uistyleButton.pressedColor = this.pressedColor;
				uistyleButton.disabledColor = this.disabledColor;
				uistyleButton.colorMultiplier = this.colorMultiplier;
				List<UnityEngine.Object> list2 = uistyleButton.UpdateStyle();
				foreach (UnityEngine.Object item in list2)
				{
					list.Add(item);
				}
			}
		}
		return list;
	}
}
