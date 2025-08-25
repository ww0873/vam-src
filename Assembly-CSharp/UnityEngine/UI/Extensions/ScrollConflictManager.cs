using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200054F RID: 1359
	[RequireComponent(typeof(ScrollRect))]
	[AddComponentMenu("UI/Extensions/Scrollrect Conflict Manager")]
	public class ScrollConflictManager : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IEventSystemHandler
	{
		// Token: 0x06002297 RID: 8855 RVA: 0x000C5558 File Offset: 0x000C3958
		public ScrollConflictManager()
		{
		}

		// Token: 0x06002298 RID: 8856 RVA: 0x000C5560 File Offset: 0x000C3960
		private void Awake()
		{
			this._myScrollRect = base.GetComponent<ScrollRect>();
			this.scrollOtherHorizontally = this._myScrollRect.vertical;
			if (this.scrollOtherHorizontally)
			{
				if (this._myScrollRect.horizontal)
				{
					Debug.Log("You have added the SecondScrollRect to a scroll view that already has both directions selected");
				}
				if (!this.ParentScrollRect.horizontal)
				{
					Debug.Log("The other scroll rect doesnt support scrolling horizontally");
				}
			}
			else if (!this.ParentScrollRect.vertical)
			{
				Debug.Log("The other scroll rect doesnt support scrolling vertically");
			}
		}

		// Token: 0x06002299 RID: 8857 RVA: 0x000C55E8 File Offset: 0x000C39E8
		public void OnBeginDrag(PointerEventData eventData)
		{
			float num = Mathf.Abs(eventData.position.x - eventData.pressPosition.x);
			float num2 = Mathf.Abs(eventData.position.y - eventData.pressPosition.y);
			if (this.scrollOtherHorizontally)
			{
				if (num > num2)
				{
					this.scrollOther = true;
					this._myScrollRect.enabled = false;
					this.ParentScrollRect.OnBeginDrag(eventData);
				}
			}
			else if (num2 > num)
			{
				this.scrollOther = true;
				this._myScrollRect.enabled = false;
				this.ParentScrollRect.OnBeginDrag(eventData);
			}
		}

		// Token: 0x0600229A RID: 8858 RVA: 0x000C5699 File Offset: 0x000C3A99
		public void OnEndDrag(PointerEventData eventData)
		{
			if (this.scrollOther)
			{
				this.scrollOther = false;
				this._myScrollRect.enabled = true;
				this.ParentScrollRect.OnEndDrag(eventData);
			}
		}

		// Token: 0x0600229B RID: 8859 RVA: 0x000C56C5 File Offset: 0x000C3AC5
		public void OnDrag(PointerEventData eventData)
		{
			if (this.scrollOther)
			{
				this.ParentScrollRect.OnDrag(eventData);
			}
		}

		// Token: 0x04001C9F RID: 7327
		public ScrollRect ParentScrollRect;

		// Token: 0x04001CA0 RID: 7328
		private ScrollRect _myScrollRect;

		// Token: 0x04001CA1 RID: 7329
		private bool scrollOther;

		// Token: 0x04001CA2 RID: 7330
		private bool scrollOtherHorizontally;
	}
}
