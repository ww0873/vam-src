using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000561 RID: 1377
	[AddComponentMenu("UI/Extensions/UI ScrollTo Selection XY")]
	[RequireComponent(typeof(ScrollRect))]
	public class UIScrollToSelectionXY : MonoBehaviour
	{
		// Token: 0x06002302 RID: 8962 RVA: 0x000C7BED File Offset: 0x000C5FED
		public UIScrollToSelectionXY()
		{
		}

		// Token: 0x06002303 RID: 8963 RVA: 0x000C7C07 File Offset: 0x000C6007
		private void Start()
		{
			this.targetScrollRect = base.GetComponent<ScrollRect>();
			this.scrollWindow = this.targetScrollRect.GetComponent<RectTransform>();
		}

		// Token: 0x06002304 RID: 8964 RVA: 0x000C7C26 File Offset: 0x000C6026
		private void Update()
		{
			this.ScrollRectToLevelSelection();
		}

		// Token: 0x06002305 RID: 8965 RVA: 0x000C7C30 File Offset: 0x000C6030
		private void ScrollRectToLevelSelection()
		{
			EventSystem current = EventSystem.current;
			bool flag = this.targetScrollRect == null || this.layoutListGroup == null || this.scrollWindow == null;
			if (flag)
			{
				return;
			}
			RectTransform rectTransform = (!(current.currentSelectedGameObject != null)) ? null : current.currentSelectedGameObject.GetComponent<RectTransform>();
			if (rectTransform != this.targetScrollObject)
			{
				this.scrollToSelection = true;
			}
			if (rectTransform == null || !this.scrollToSelection || rectTransform.transform.parent != this.layoutListGroup.transform)
			{
				return;
			}
			bool flag2 = false;
			bool flag3 = false;
			if (this.targetScrollRect.vertical)
			{
				float num = -rectTransform.anchoredPosition.y;
				float y = this.layoutListGroup.anchoredPosition.y;
				float num2 = y - num;
				this.targetScrollRect.verticalNormalizedPosition += num2 / this.layoutListGroup.sizeDelta.y * Time.deltaTime * this.scrollSpeed;
				flag3 = (Mathf.Abs(num2) < 2f);
			}
			if (this.targetScrollRect.horizontal)
			{
				float num3 = -rectTransform.anchoredPosition.x;
				float x = this.layoutListGroup.anchoredPosition.x;
				float num4 = x - num3;
				this.targetScrollRect.horizontalNormalizedPosition += num4 / this.layoutListGroup.sizeDelta.x * Time.deltaTime * this.scrollSpeed;
				flag2 = (Mathf.Abs(num4) < 2f);
			}
			if (flag2 && flag3)
			{
				this.scrollToSelection = false;
			}
			this.targetScrollObject = rectTransform;
		}

		// Token: 0x04001CF5 RID: 7413
		public float scrollSpeed = 10f;

		// Token: 0x04001CF6 RID: 7414
		[SerializeField]
		private RectTransform layoutListGroup;

		// Token: 0x04001CF7 RID: 7415
		private RectTransform targetScrollObject;

		// Token: 0x04001CF8 RID: 7416
		private bool scrollToSelection = true;

		// Token: 0x04001CF9 RID: 7417
		private RectTransform scrollWindow;

		// Token: 0x04001CFA RID: 7418
		private RectTransform currentCanvas;

		// Token: 0x04001CFB RID: 7419
		private ScrollRect targetScrollRect;
	}
}
