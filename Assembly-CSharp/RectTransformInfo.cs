using System;
using UnityEngine;

// Token: 0x02000DDB RID: 3547
[ExecuteInEditMode]
public class RectTransformInfo : MonoBehaviour
{
	// Token: 0x06006DCC RID: 28108 RVA: 0x00294144 File Offset: 0x00292544
	public RectTransformInfo()
	{
	}

	// Token: 0x06006DCD RID: 28109 RVA: 0x0029414C File Offset: 0x0029254C
	private void Update()
	{
		RectTransform component = base.GetComponent<RectTransform>();
		if (component != null)
		{
			this.anchoredPosition = component.anchoredPosition;
			this.anchorMin = component.anchorMin;
			this.anchorMax = component.anchorMax;
			this.offsetMin = component.offsetMin;
			this.offsetMax = component.offsetMax;
			this.pivot = component.pivot;
			this.rect = component.rect;
			this.sizeDelta = component.sizeDelta;
		}
	}

	// Token: 0x04005F0F RID: 24335
	public Vector2 anchoredPosition;

	// Token: 0x04005F10 RID: 24336
	public Vector2 anchorMin;

	// Token: 0x04005F11 RID: 24337
	public Vector2 anchorMax;

	// Token: 0x04005F12 RID: 24338
	public Vector2 offsetMin;

	// Token: 0x04005F13 RID: 24339
	public Vector2 offsetMax;

	// Token: 0x04005F14 RID: 24340
	public Vector2 pivot;

	// Token: 0x04005F15 RID: 24341
	public Rect rect;

	// Token: 0x04005F16 RID: 24342
	public Vector2 sizeDelta;
}
