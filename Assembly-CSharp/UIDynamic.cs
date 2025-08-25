using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DEB RID: 3563
public class UIDynamic : MonoBehaviour
{
	// Token: 0x06006E26 RID: 28198 RVA: 0x00295CC1 File Offset: 0x002940C1
	public UIDynamic()
	{
	}

	// Token: 0x17001016 RID: 4118
	// (get) Token: 0x06006E27 RID: 28199 RVA: 0x00295CCC File Offset: 0x002940CC
	// (set) Token: 0x06006E28 RID: 28200 RVA: 0x00295D00 File Offset: 0x00294100
	public float height
	{
		get
		{
			RectTransform component = base.GetComponent<RectTransform>();
			if (component != null)
			{
				return component.sizeDelta.y;
			}
			return 0f;
		}
		set
		{
			LayoutElement component = base.GetComponent<LayoutElement>();
			if (component != null)
			{
				if (value > component.minHeight)
				{
					component.preferredHeight = value;
				}
				else
				{
					component.preferredHeight = component.minHeight;
				}
			}
			RectTransform component2 = base.GetComponent<RectTransform>();
			if (component2 != null)
			{
				Vector2 sizeDelta = component2.sizeDelta;
				sizeDelta.y = value;
				component2.sizeDelta = sizeDelta;
			}
		}
	}
}
