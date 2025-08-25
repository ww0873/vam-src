using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000E0C RID: 3596
public class UIStyleToggleMaster : UIStyleToggle
{
	// Token: 0x06006EEC RID: 28396 RVA: 0x00299D41 File Offset: 0x00298141
	public UIStyleToggleMaster()
	{
	}

	// Token: 0x06006EED RID: 28397 RVA: 0x00299D4C File Offset: 0x0029814C
	public override List<UnityEngine.Object> GetControlledObjects()
	{
		List<UnityEngine.Object> list = new List<UnityEngine.Object>();
		foreach (UIStyleToggle uistyleToggle in base.GetComponentsInChildren<UIStyleToggle>(true))
		{
			if (uistyleToggle == this)
			{
				list.Add(this);
			}
			else if (uistyleToggle.styleName == this.styleName && uistyleToggle.gameObject != base.gameObject)
			{
				List<UnityEngine.Object> controlledObjects = uistyleToggle.GetControlledObjects();
				foreach (UnityEngine.Object item in controlledObjects)
				{
					list.Add(item);
				}
			}
		}
		return list;
	}

	// Token: 0x06006EEE RID: 28398 RVA: 0x00299E1C File Offset: 0x0029821C
	public override List<UnityEngine.Object> UpdateStyle()
	{
		List<UnityEngine.Object> list = new List<UnityEngine.Object>();
		foreach (UIStyleToggle uistyleToggle in base.GetComponentsInChildren<UIStyleToggle>(true))
		{
			if (uistyleToggle == this)
			{
				list.Add(this);
			}
			else if (uistyleToggle.styleName == this.styleName && uistyleToggle.gameObject != base.gameObject)
			{
				uistyleToggle.normalColor = this.normalColor;
				uistyleToggle.highlightedColor = this.highlightedColor;
				uistyleToggle.pressedColor = this.pressedColor;
				uistyleToggle.disabledColor = this.disabledColor;
				uistyleToggle.colorMultiplier = this.colorMultiplier;
				List<UnityEngine.Object> list2 = uistyleToggle.UpdateStyle();
				foreach (UnityEngine.Object item in list2)
				{
					list.Add(item);
				}
			}
		}
		return list;
	}
}
