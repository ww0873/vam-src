using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004D8 RID: 1240
	[AddComponentMenu("UI/Extensions/RescalePanels/ResizePanel")]
	public class ResizePanel : MonoBehaviour, IPointerDownHandler, IDragHandler, IEventSystemHandler
	{
		// Token: 0x06001F41 RID: 8001 RVA: 0x000B1EAE File Offset: 0x000B02AE
		public ResizePanel()
		{
		}

		// Token: 0x06001F42 RID: 8002 RVA: 0x000B1EB8 File Offset: 0x000B02B8
		private void Awake()
		{
			this.rectTransform = base.transform.parent.GetComponent<RectTransform>();
			float width = this.rectTransform.rect.width;
			float height = this.rectTransform.rect.height;
			this.ratio = height / width;
			this.minSize = new Vector2(0.1f * width, 0.1f * height);
			this.maxSize = new Vector2(10f * width, 10f * height);
		}

		// Token: 0x06001F43 RID: 8003 RVA: 0x000B1F3E File Offset: 0x000B033E
		public void OnPointerDown(PointerEventData data)
		{
			this.rectTransform.SetAsLastSibling();
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform, data.position, data.pressEventCamera, out this.previousPointerPosition);
		}

		// Token: 0x06001F44 RID: 8004 RVA: 0x000B1F6C File Offset: 0x000B036C
		public void OnDrag(PointerEventData data)
		{
			if (this.rectTransform == null)
			{
				return;
			}
			Vector2 vector = this.rectTransform.sizeDelta;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform, data.position, data.pressEventCamera, out this.currentPointerPosition);
			Vector2 vector2 = this.currentPointerPosition - this.previousPointerPosition;
			vector += new Vector2(vector2.x, this.ratio * vector2.x);
			vector = new Vector2(Mathf.Clamp(vector.x, this.minSize.x, this.maxSize.x), Mathf.Clamp(vector.y, this.minSize.y, this.maxSize.y));
			this.rectTransform.sizeDelta = vector;
			this.previousPointerPosition = this.currentPointerPosition;
		}

		// Token: 0x04001A73 RID: 6771
		public Vector2 minSize;

		// Token: 0x04001A74 RID: 6772
		public Vector2 maxSize;

		// Token: 0x04001A75 RID: 6773
		private RectTransform rectTransform;

		// Token: 0x04001A76 RID: 6774
		private Vector2 currentPointerPosition;

		// Token: 0x04001A77 RID: 6775
		private Vector2 previousPointerPosition;

		// Token: 0x04001A78 RID: 6776
		private float ratio;
	}
}
