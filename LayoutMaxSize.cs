using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000DD1 RID: 3537
[RequireComponent(typeof(RectTransform))]
[Serializable]
public class LayoutMaxSize : LayoutGroup
{
	// Token: 0x06006D8E RID: 28046 RVA: 0x0029358C File Offset: 0x0029198C
	public LayoutMaxSize()
	{
	}

	// Token: 0x06006D8F RID: 28047 RVA: 0x00293594 File Offset: 0x00291994
	public override void SetLayoutHorizontal()
	{
		this.UpdateMaxSizes();
	}

	// Token: 0x06006D90 RID: 28048 RVA: 0x0029359C File Offset: 0x0029199C
	public override void SetLayoutVertical()
	{
		this.UpdateMaxSizes();
	}

	// Token: 0x06006D91 RID: 28049 RVA: 0x002935A4 File Offset: 0x002919A4
	public override void CalculateLayoutInputVertical()
	{
		this.UpdateMaxSizes();
	}

	// Token: 0x06006D92 RID: 28050 RVA: 0x002935AC File Offset: 0x002919AC
	protected override void OnRectTransformDimensionsChange()
	{
		base.OnRectTransformDimensionsChange();
		this.UpdateMaxSizes();
	}

	// Token: 0x06006D93 RID: 28051 RVA: 0x002935BA File Offset: 0x002919BA
	protected override void OnTransformChildrenChanged()
	{
		base.OnTransformChildrenChanged();
		this.UpdateMaxSizes();
	}

	// Token: 0x06006D94 RID: 28052 RVA: 0x002935C8 File Offset: 0x002919C8
	private void Update()
	{
		this.UpdateMaxSizes();
	}

	// Token: 0x06006D95 RID: 28053 RVA: 0x002935D0 File Offset: 0x002919D0
	private void UpdateMaxSizes()
	{
		if (base.transform.childCount > 0)
		{
			if (this.adjustHeight)
			{
				bool flag = true;
				float num = 0f;
				float num2 = 0f;
				for (int i = 0; i < base.transform.childCount; i++)
				{
					RectTransform component = base.transform.GetChild(i).GetComponent<RectTransform>();
					if (!(component == null))
					{
						Vector3 localPosition = component.localPosition;
						Vector2 sizeDelta = component.sizeDelta;
						Vector2 pivot = component.pivot;
						if (flag)
						{
							num = localPosition.y + sizeDelta.y * (1f - pivot.y);
							num2 = localPosition.y - sizeDelta.y * pivot.y;
						}
						else
						{
							num = Mathf.Max(num, localPosition.y + sizeDelta.y * (1f - pivot.y));
							num2 = Mathf.Min(num2, localPosition.y - sizeDelta.y * pivot.y);
						}
						flag = false;
					}
				}
				float num3 = Mathf.Abs(num - num2);
				base.SetLayoutInputForAxis(num3, num3, 0f, 1);
				if (num3 != this.lastY)
				{
					base.SetDirty();
				}
				this.lastY = num3;
			}
			if (this.adjustWidth)
			{
				bool flag2 = true;
				float num4 = 0f;
				float num5 = 0f;
				for (int j = 0; j < base.transform.childCount; j++)
				{
					RectTransform component2 = base.transform.GetChild(j).GetComponent<RectTransform>();
					if (!(component2 == null))
					{
						Vector3 localPosition2 = component2.localPosition;
						Vector2 sizeDelta2 = component2.sizeDelta;
						Vector2 pivot2 = component2.pivot;
						if (flag2)
						{
							num4 = localPosition2.x + sizeDelta2.x * (1f - pivot2.x);
							num5 = localPosition2.x - sizeDelta2.x * pivot2.x;
						}
						else
						{
							num4 = Mathf.Max(num4, localPosition2.x + sizeDelta2.x * (1f - pivot2.x));
							num5 = Mathf.Min(num5, localPosition2.x - sizeDelta2.x * pivot2.x);
						}
						flag2 = false;
					}
				}
				float num6 = Mathf.Abs(num4 - num5);
				base.SetLayoutInputForAxis(num6, num6, 0f, 0);
				if (num6 != this.lastX)
				{
					base.SetDirty();
				}
				this.lastX = num6;
			}
		}
	}

	// Token: 0x04005EE9 RID: 24297
	public bool adjustHeight;

	// Token: 0x04005EEA RID: 24298
	public bool adjustWidth;

	// Token: 0x04005EEB RID: 24299
	private float lastX;

	// Token: 0x04005EEC RID: 24300
	private float lastY;
}
