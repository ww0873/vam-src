using System;
using UnityEngine;
using UnityEngine.EventSystems;

// Token: 0x02000DF2 RID: 3570
public class UIHoverPopup : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IEventSystemHandler
{
	// Token: 0x06006E69 RID: 28265 RVA: 0x0029708D File Offset: 0x0029548D
	public UIHoverPopup()
	{
	}

	// Token: 0x06006E6A RID: 28266 RVA: 0x002970A3 File Offset: 0x002954A3
	public void TogglePopup()
	{
		if (this.popup != null)
		{
			if (this.popup.activeSelf)
			{
				this.HidePopup();
			}
			else
			{
				this.ShowPopup();
			}
		}
	}

	// Token: 0x06006E6B RID: 28267 RVA: 0x002970D8 File Offset: 0x002954D8
	public void ShowPopup()
	{
		if (this.popup != null)
		{
			this.isInteracting = true;
			UIHoverPopup.MoveTo moveTo = this.moveTo;
			if (moveTo != UIHoverPopup.MoveTo.Parent)
			{
				if (moveTo == UIHoverPopup.MoveTo.Grandparent)
				{
					if (base.transform.parent != null && base.transform.parent.parent != null)
					{
						this.originalParent = this.popup.transform.parent;
						this.popup.transform.SetParent(base.transform.parent.parent, true);
						this.wasMoved = true;
					}
				}
			}
			else if (base.transform.parent != null)
			{
				this.originalParent = this.popup.transform.parent;
				this.popup.transform.SetParent(base.transform.parent, true);
				this.wasMoved = true;
			}
			this.popup.SetActive(true);
			this.isInteracting = false;
		}
	}

	// Token: 0x06006E6C RID: 28268 RVA: 0x002971F2 File Offset: 0x002955F2
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (this.enableShowOnHover)
		{
			this.ShowPopup();
		}
	}

	// Token: 0x06006E6D RID: 28269 RVA: 0x00297208 File Offset: 0x00295608
	public void HidePopup()
	{
		if (this.popup != null)
		{
			if (this.wasMoved)
			{
				this.popup.transform.SetParent(this.originalParent, true);
				this.wasMoved = false;
			}
			this.popup.SetActive(false);
		}
	}

	// Token: 0x06006E6E RID: 28270 RVA: 0x0029725B File Offset: 0x0029565B
	public void OnPointerExit(PointerEventData eventData)
	{
		if (this.enableHideOnExit)
		{
			this.HidePopup();
		}
	}

	// Token: 0x06006E6F RID: 28271 RVA: 0x0029726E File Offset: 0x0029566E
	private void OnEnable()
	{
		if (!this.isInteracting)
		{
			this.HidePopup();
		}
	}

	// Token: 0x06006E70 RID: 28272 RVA: 0x00297281 File Offset: 0x00295681
	private void OnDisable()
	{
		this.HidePopup();
	}

	// Token: 0x04005F85 RID: 24453
	public GameObject popup;

	// Token: 0x04005F86 RID: 24454
	public bool enableShowOnHover = true;

	// Token: 0x04005F87 RID: 24455
	public bool enableHideOnExit = true;

	// Token: 0x04005F88 RID: 24456
	[Tooltip("To allow it to pop over other objects in hierarchy")]
	public UIHoverPopup.MoveTo moveTo;

	// Token: 0x04005F89 RID: 24457
	protected Transform originalParent;

	// Token: 0x04005F8A RID: 24458
	protected bool wasMoved;

	// Token: 0x04005F8B RID: 24459
	protected bool isInteracting;

	// Token: 0x02000DF3 RID: 3571
	public enum MoveTo
	{
		// Token: 0x04005F8D RID: 24461
		None,
		// Token: 0x04005F8E RID: 24462
		Parent,
		// Token: 0x04005F8F RID: 24463
		Grandparent
	}
}
