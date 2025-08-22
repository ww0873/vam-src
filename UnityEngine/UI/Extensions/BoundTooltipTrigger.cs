using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000539 RID: 1337
	[AddComponentMenu("UI/Extensions/Bound Tooltip/Tooltip Trigger")]
	public class BoundTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, IEventSystemHandler
	{
		// Token: 0x06002203 RID: 8707 RVA: 0x000C28C5 File Offset: 0x000C0CC5
		public BoundTooltipTrigger()
		{
		}

		// Token: 0x06002204 RID: 8708 RVA: 0x000C28D0 File Offset: 0x000C0CD0
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.useMousePosition)
			{
				this.StartHover(new Vector3(eventData.position.x, eventData.position.y, 0f));
			}
			else
			{
				this.StartHover(base.transform.position + this.offset);
			}
		}

		// Token: 0x06002205 RID: 8709 RVA: 0x000C2935 File Offset: 0x000C0D35
		public void OnSelect(BaseEventData eventData)
		{
			this.StartHover(base.transform.position);
		}

		// Token: 0x06002206 RID: 8710 RVA: 0x000C2948 File Offset: 0x000C0D48
		public void OnPointerExit(PointerEventData eventData)
		{
			this.StopHover();
		}

		// Token: 0x06002207 RID: 8711 RVA: 0x000C2950 File Offset: 0x000C0D50
		public void OnDeselect(BaseEventData eventData)
		{
			this.StopHover();
		}

		// Token: 0x06002208 RID: 8712 RVA: 0x000C2958 File Offset: 0x000C0D58
		private void StartHover(Vector3 position)
		{
			BoundTooltipItem.Instance.ShowTooltip(this.text, position);
		}

		// Token: 0x06002209 RID: 8713 RVA: 0x000C296B File Offset: 0x000C0D6B
		private void StopHover()
		{
			BoundTooltipItem.Instance.HideTooltip();
		}

		// Token: 0x04001C47 RID: 7239
		[TextArea]
		public string text;

		// Token: 0x04001C48 RID: 7240
		public bool useMousePosition;

		// Token: 0x04001C49 RID: 7241
		public Vector3 offset;
	}
}
