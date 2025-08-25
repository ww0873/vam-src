using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000DDE RID: 3550
public class ScrollRectContentManager : MonoBehaviour
{
	// Token: 0x06006DD5 RID: 28117 RVA: 0x0029423A File Offset: 0x0029263A
	public ScrollRectContentManager()
	{
	}

	// Token: 0x06006DD6 RID: 28118 RVA: 0x00294258 File Offset: 0x00292658
	public void RelayoutPanel()
	{
		if (this.contentPanel != null)
		{
			float num = this.buffer;
			Vector2 anchoredPosition;
			anchoredPosition.x = 0f;
			anchoredPosition.y = -num;
			foreach (RectTransform rectTransform in this.items)
			{
				rectTransform.anchoredPosition = anchoredPosition;
				float num2 = rectTransform.rect.height + 10f;
				num += num2;
				anchoredPosition.y -= num2;
			}
			num += this.bottomExtraSize;
			Vector2 sizeDelta = this.contentPanel.sizeDelta;
			sizeDelta.y = num;
			this.contentPanel.sizeDelta = sizeDelta;
		}
	}

	// Token: 0x06006DD7 RID: 28119 RVA: 0x00294338 File Offset: 0x00292738
	public void AddItem(RectTransform item, int index = -1, bool skipLayout = false)
	{
		if (this.items == null)
		{
			this.items = new List<RectTransform>();
		}
		item.SetParent(base.transform, false);
		if (index == -1)
		{
			this.items.Add(item);
		}
		else if (index < this.items.Count)
		{
			this.items.Insert(index, item);
		}
		else
		{
			this.items.Add(item);
		}
		if (!skipLayout)
		{
			this.RelayoutPanel();
		}
	}

	// Token: 0x06006DD8 RID: 28120 RVA: 0x002943BB File Offset: 0x002927BB
	public void RemoveItem(RectTransform item)
	{
		if (this.items == null)
		{
			this.items = new List<RectTransform>();
		}
		this.items.Remove(item);
		this.RelayoutPanel();
	}

	// Token: 0x06006DD9 RID: 28121 RVA: 0x002943E6 File Offset: 0x002927E6
	public void RemoveAllItems()
	{
		if (this.items != null)
		{
			this.items.Clear();
			this.RelayoutPanel();
		}
	}

	// Token: 0x04005F19 RID: 24345
	public float buffer = 10f;

	// Token: 0x04005F1A RID: 24346
	public float bottomExtraSize = 200f;

	// Token: 0x04005F1B RID: 24347
	public RectTransform contentPanel;

	// Token: 0x04005F1C RID: 24348
	protected List<RectTransform> items;
}
