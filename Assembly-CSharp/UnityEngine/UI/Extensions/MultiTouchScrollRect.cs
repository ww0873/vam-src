using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004CC RID: 1228
	[AddComponentMenu("UI/Extensions/MultiTouchScrollRect")]
	public class MultiTouchScrollRect : ScrollRect
	{
		// Token: 0x06001F00 RID: 7936 RVA: 0x000B06E9 File Offset: 0x000AEAE9
		public MultiTouchScrollRect()
		{
		}

		// Token: 0x06001F01 RID: 7937 RVA: 0x000B06F9 File Offset: 0x000AEAF9
		public override void OnBeginDrag(PointerEventData eventData)
		{
			this.pid = eventData.pointerId;
			base.OnBeginDrag(eventData);
		}

		// Token: 0x06001F02 RID: 7938 RVA: 0x000B070E File Offset: 0x000AEB0E
		public override void OnDrag(PointerEventData eventData)
		{
			if (this.pid == eventData.pointerId)
			{
				base.OnDrag(eventData);
			}
		}

		// Token: 0x06001F03 RID: 7939 RVA: 0x000B0728 File Offset: 0x000AEB28
		public override void OnEndDrag(PointerEventData eventData)
		{
			this.pid = -100;
			base.OnEndDrag(eventData);
		}

		// Token: 0x04001A2E RID: 6702
		private int pid = -100;
	}
}
