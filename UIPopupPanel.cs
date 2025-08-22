using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000DFB RID: 3579
public class UIPopupPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
{
	// Token: 0x06006EB4 RID: 28340 RVA: 0x00298AF1 File Offset: 0x00296EF1
	public UIPopupPanel()
	{
	}

	// Token: 0x06006EB5 RID: 28341 RVA: 0x00298B01 File Offset: 0x00296F01
	public void OnPointerEnter(PointerEventData ed)
	{
		this.eligibleForClose = true;
		this._timer = 0;
	}

	// Token: 0x06006EB6 RID: 28342 RVA: 0x00298B11 File Offset: 0x00296F11
	public void OnPointerExit(PointerEventData ed)
	{
		if (this.eligibleForClose)
		{
			this._timer = this.framesToClose;
		}
		this.eligibleForClose = false;
	}

	// Token: 0x04005FC1 RID: 24513
	public UIPopup popup;

	// Token: 0x04005FC2 RID: 24514
	public int framesToClose = 100;

	// Token: 0x04005FC3 RID: 24515
	protected int _timer;

	// Token: 0x04005FC4 RID: 24516
	protected bool eligibleForClose;
}
