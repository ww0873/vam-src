using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000520 RID: 1312
	public class ScrollSnapScrollbarHelper : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IEventSystemHandler
	{
		// Token: 0x06002121 RID: 8481 RVA: 0x000BDB43 File Offset: 0x000BBF43
		public ScrollSnapScrollbarHelper()
		{
		}

		// Token: 0x06002122 RID: 8482 RVA: 0x000BDB4B File Offset: 0x000BBF4B
		public void OnBeginDrag(PointerEventData eventData)
		{
			this.OnScrollBarDown();
		}

		// Token: 0x06002123 RID: 8483 RVA: 0x000BDB53 File Offset: 0x000BBF53
		public void OnDrag(PointerEventData eventData)
		{
			this.ss.CurrentPage();
		}

		// Token: 0x06002124 RID: 8484 RVA: 0x000BDB61 File Offset: 0x000BBF61
		public void OnEndDrag(PointerEventData eventData)
		{
			this.OnScrollBarUp();
		}

		// Token: 0x06002125 RID: 8485 RVA: 0x000BDB69 File Offset: 0x000BBF69
		public void OnPointerDown(PointerEventData eventData)
		{
			this.OnScrollBarDown();
		}

		// Token: 0x06002126 RID: 8486 RVA: 0x000BDB71 File Offset: 0x000BBF71
		public void OnPointerUp(PointerEventData eventData)
		{
			this.OnScrollBarUp();
		}

		// Token: 0x06002127 RID: 8487 RVA: 0x000BDB79 File Offset: 0x000BBF79
		private void OnScrollBarDown()
		{
			if (this.ss != null)
			{
				this.ss.SetLerp(false);
				this.ss.StartScreenChange();
			}
		}

		// Token: 0x06002128 RID: 8488 RVA: 0x000BDB9D File Offset: 0x000BBF9D
		private void OnScrollBarUp()
		{
			this.ss.SetLerp(true);
			this.ss.ChangePage(this.ss.CurrentPage());
		}

		// Token: 0x04001BC5 RID: 7109
		internal IScrollSnap ss;
	}
}
