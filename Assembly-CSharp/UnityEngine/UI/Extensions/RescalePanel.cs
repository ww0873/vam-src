using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004D7 RID: 1239
	[AddComponentMenu("UI/Extensions/RescalePanels/RescalePanel")]
	public class RescalePanel : MonoBehaviour, IPointerDownHandler, IDragHandler, IEventSystemHandler
	{
		// Token: 0x06001F3D RID: 7997 RVA: 0x000B1D04 File Offset: 0x000B0104
		public RescalePanel()
		{
		}

		// Token: 0x06001F3E RID: 7998 RVA: 0x000B1D0C File Offset: 0x000B010C
		private void Awake()
		{
			this.rectTransform = base.transform.parent.GetComponent<RectTransform>();
			this.goTransform = base.transform.parent;
			this.thisRectTransform = base.GetComponent<RectTransform>();
			this.sizeDelta = this.thisRectTransform.sizeDelta;
		}

		// Token: 0x06001F3F RID: 7999 RVA: 0x000B1D5D File Offset: 0x000B015D
		public void OnPointerDown(PointerEventData data)
		{
			this.rectTransform.SetAsLastSibling();
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform, data.position, data.pressEventCamera, out this.previousPointerPosition);
		}

		// Token: 0x06001F40 RID: 8000 RVA: 0x000B1D88 File Offset: 0x000B0188
		public void OnDrag(PointerEventData data)
		{
			if (this.rectTransform == null)
			{
				return;
			}
			Vector3 vector = this.goTransform.localScale;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform, data.position, data.pressEventCamera, out this.currentPointerPosition);
			Vector2 vector2 = this.currentPointerPosition - this.previousPointerPosition;
			vector += new Vector3(-vector2.y * 0.001f, -vector2.y * 0.001f, 0f);
			vector = new Vector3(Mathf.Clamp(vector.x, this.minSize.x, this.maxSize.x), Mathf.Clamp(vector.y, this.minSize.y, this.maxSize.y), 1f);
			this.goTransform.localScale = vector;
			this.previousPointerPosition = this.currentPointerPosition;
			float num = this.sizeDelta.x / this.goTransform.localScale.x;
			Vector2 vector3 = new Vector2(num, num);
			this.thisRectTransform.sizeDelta = vector3;
		}

		// Token: 0x04001A6B RID: 6763
		public Vector2 minSize;

		// Token: 0x04001A6C RID: 6764
		public Vector2 maxSize;

		// Token: 0x04001A6D RID: 6765
		private RectTransform rectTransform;

		// Token: 0x04001A6E RID: 6766
		private Transform goTransform;

		// Token: 0x04001A6F RID: 6767
		private Vector2 currentPointerPosition;

		// Token: 0x04001A70 RID: 6768
		private Vector2 previousPointerPosition;

		// Token: 0x04001A71 RID: 6769
		private RectTransform thisRectTransform;

		// Token: 0x04001A72 RID: 6770
		private Vector2 sizeDelta;
	}
}
