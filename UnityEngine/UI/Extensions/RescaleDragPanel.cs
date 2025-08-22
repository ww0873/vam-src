using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004D6 RID: 1238
	[AddComponentMenu("UI/Extensions/RescalePanels/RescaleDragPanel")]
	public class RescaleDragPanel : MonoBehaviour, IPointerDownHandler, IDragHandler, IEventSystemHandler
	{
		// Token: 0x06001F38 RID: 7992 RVA: 0x000B1B5E File Offset: 0x000AFF5E
		public RescaleDragPanel()
		{
		}

		// Token: 0x06001F39 RID: 7993 RVA: 0x000B1B68 File Offset: 0x000AFF68
		private void Awake()
		{
			Canvas componentInParent = base.GetComponentInParent<Canvas>();
			if (componentInParent != null)
			{
				this.canvasRectTransform = (componentInParent.transform as RectTransform);
				this.panelRectTransform = (base.transform.parent as RectTransform);
				this.goTransform = base.transform.parent;
			}
		}

		// Token: 0x06001F3A RID: 7994 RVA: 0x000B1BC0 File Offset: 0x000AFFC0
		public void OnPointerDown(PointerEventData data)
		{
			this.panelRectTransform.SetAsLastSibling();
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.panelRectTransform, data.position, data.pressEventCamera, out this.pointerOffset);
		}

		// Token: 0x06001F3B RID: 7995 RVA: 0x000B1BEC File Offset: 0x000AFFEC
		public void OnDrag(PointerEventData data)
		{
			if (this.panelRectTransform == null)
			{
				return;
			}
			Vector2 screenPoint = this.ClampToWindow(data);
			Vector2 a;
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this.canvasRectTransform, screenPoint, data.pressEventCamera, out a))
			{
				this.panelRectTransform.localPosition = a - new Vector2(this.pointerOffset.x * this.goTransform.localScale.x, this.pointerOffset.y * this.goTransform.localScale.y);
			}
		}

		// Token: 0x06001F3C RID: 7996 RVA: 0x000B1C88 File Offset: 0x000B0088
		private Vector2 ClampToWindow(PointerEventData data)
		{
			Vector2 position = data.position;
			Vector3[] array = new Vector3[4];
			this.canvasRectTransform.GetWorldCorners(array);
			float x = Mathf.Clamp(position.x, array[0].x, array[2].x);
			float y = Mathf.Clamp(position.y, array[0].y, array[2].y);
			Vector2 result = new Vector2(x, y);
			return result;
		}

		// Token: 0x04001A67 RID: 6759
		private Vector2 pointerOffset;

		// Token: 0x04001A68 RID: 6760
		private RectTransform canvasRectTransform;

		// Token: 0x04001A69 RID: 6761
		private RectTransform panelRectTransform;

		// Token: 0x04001A6A RID: 6762
		private Transform goTransform;
	}
}
