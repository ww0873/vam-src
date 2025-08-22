using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000DD8 RID: 3544
public class PointerEnterExitAction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
{
	// Token: 0x06006DC1 RID: 28097 RVA: 0x0029410C File Offset: 0x0029250C
	public PointerEnterExitAction()
	{
	}

	// Token: 0x06006DC2 RID: 28098 RVA: 0x00294114 File Offset: 0x00292514
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (this.onEnterActions != null)
		{
			this.onEnterActions();
		}
	}

	// Token: 0x06006DC3 RID: 28099 RVA: 0x0029412C File Offset: 0x0029252C
	public void OnPointerExit(PointerEventData eventData)
	{
		if (this.onExitActions != null)
		{
			this.onExitActions();
		}
	}

	// Token: 0x04005F0D RID: 24333
	public PointerEnterExitAction.OnEnterAction onEnterActions;

	// Token: 0x04005F0E RID: 24334
	public PointerEnterExitAction.OnExitAction onExitActions;

	// Token: 0x02000DD9 RID: 3545
	// (Invoke) Token: 0x06006DC5 RID: 28101
	public delegate void OnEnterAction();

	// Token: 0x02000DDA RID: 3546
	// (Invoke) Token: 0x06006DC9 RID: 28105
	public delegate void OnExitAction();
}
