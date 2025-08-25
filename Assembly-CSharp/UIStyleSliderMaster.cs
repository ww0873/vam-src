using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000E08 RID: 3592
public class UIStyleSliderMaster : UIStyleSlider
{
	// Token: 0x06006EE0 RID: 28384 RVA: 0x002997A5 File Offset: 0x00297BA5
	public UIStyleSliderMaster()
	{
	}

	// Token: 0x06006EE1 RID: 28385 RVA: 0x002997B0 File Offset: 0x00297BB0
	public override List<UnityEngine.Object> GetControlledObjects()
	{
		List<UnityEngine.Object> list = new List<UnityEngine.Object>();
		foreach (UIStyleSlider uistyleSlider in base.GetComponentsInChildren<UIStyleSlider>(true))
		{
			if (uistyleSlider == this)
			{
				list.Add(this);
			}
			else if (uistyleSlider.styleName == this.styleName && uistyleSlider.gameObject != base.gameObject)
			{
				List<UnityEngine.Object> controlledObjects = uistyleSlider.GetControlledObjects();
				foreach (UnityEngine.Object item in controlledObjects)
				{
					list.Add(item);
				}
			}
		}
		return list;
	}

	// Token: 0x06006EE2 RID: 28386 RVA: 0x00299880 File Offset: 0x00297C80
	public override List<UnityEngine.Object> UpdateStyle()
	{
		List<UnityEngine.Object> list = new List<UnityEngine.Object>();
		foreach (UIStyleSlider uistyleSlider in base.GetComponentsInChildren<UIStyleSlider>(true))
		{
			if (uistyleSlider == this)
			{
				list.Add(this);
			}
			else if (uistyleSlider.styleName == this.styleName && uistyleSlider.gameObject != base.gameObject)
			{
				uistyleSlider.normalColor = this.normalColor;
				uistyleSlider.highlightedColor = this.highlightedColor;
				uistyleSlider.pressedColor = this.pressedColor;
				uistyleSlider.disabledColor = this.disabledColor;
				uistyleSlider.colorMultiplier = this.colorMultiplier;
				List<UnityEngine.Object> list2 = uistyleSlider.UpdateStyle();
				foreach (UnityEngine.Object item in list2)
				{
					list.Add(item);
				}
			}
		}
		return list;
	}
}
