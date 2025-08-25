using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x02000496 RID: 1174
	public class PaginationScript : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x06001DBA RID: 7610 RVA: 0x000AAEAF File Offset: 0x000A92AF
		public PaginationScript()
		{
		}

		// Token: 0x06001DBB RID: 7611 RVA: 0x000AAEB7 File Offset: 0x000A92B7
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.hss != null)
			{
				this.hss.GoToScreen(this.Page);
			}
		}

		// Token: 0x04001925 RID: 6437
		public HorizontalScrollSnap hss;

		// Token: 0x04001926 RID: 6438
		public int Page;
	}
}
