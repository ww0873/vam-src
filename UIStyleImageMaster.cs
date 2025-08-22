using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000E06 RID: 3590
public class UIStyleImageMaster : UIStyleImage
{
	// Token: 0x06006EDA RID: 28378 RVA: 0x002994BA File Offset: 0x002978BA
	public UIStyleImageMaster()
	{
	}

	// Token: 0x06006EDB RID: 28379 RVA: 0x002994C4 File Offset: 0x002978C4
	public override List<UnityEngine.Object> GetControlledObjects()
	{
		List<UnityEngine.Object> list = new List<UnityEngine.Object>();
		foreach (UIStyleImage uistyleImage in base.GetComponentsInChildren<UIStyleImage>(true))
		{
			if (uistyleImage == this)
			{
				list.Add(this);
			}
			else if (uistyleImage.styleName == this.styleName && uistyleImage.gameObject != base.gameObject)
			{
				List<UnityEngine.Object> controlledObjects = uistyleImage.GetControlledObjects();
				foreach (UnityEngine.Object item in controlledObjects)
				{
					list.Add(item);
				}
			}
		}
		return list;
	}

	// Token: 0x06006EDC RID: 28380 RVA: 0x00299594 File Offset: 0x00297994
	public override List<UnityEngine.Object> UpdateStyle()
	{
		List<UnityEngine.Object> list = new List<UnityEngine.Object>();
		foreach (UIStyleImage uistyleImage in base.GetComponentsInChildren<UIStyleImage>(true))
		{
			if (uistyleImage == this)
			{
				list.Add(this);
			}
			else if (uistyleImage.styleName == this.styleName && uistyleImage.gameObject != base.gameObject)
			{
				uistyleImage.color = this.color;
				List<UnityEngine.Object> list2 = uistyleImage.UpdateStyle();
				foreach (UnityEngine.Object item in list2)
				{
					list.Add(item);
				}
			}
		}
		return list;
	}
}
