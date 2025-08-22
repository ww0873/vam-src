using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000559 RID: 1369
	[AddComponentMenu("UI/Extensions/UI Scrollrect Occlusion")]
	public class UI_ScrollRectOcclusion : MonoBehaviour
	{
		// Token: 0x060022CD RID: 8909 RVA: 0x000C6B3D File Offset: 0x000C4F3D
		public UI_ScrollRectOcclusion()
		{
		}

		// Token: 0x060022CE RID: 8910 RVA: 0x000C6B50 File Offset: 0x000C4F50
		private void Awake()
		{
			if (this.InitByUser)
			{
				return;
			}
			this.Init();
		}

		// Token: 0x060022CF RID: 8911 RVA: 0x000C6B64 File Offset: 0x000C4F64
		public void Init()
		{
			if (base.GetComponent<ScrollRect>() != null)
			{
				this._scrollRect = base.GetComponent<ScrollRect>();
				this._scrollRect.onValueChanged.AddListener(new UnityAction<Vector2>(this.OnScroll));
				this._isHorizontal = this._scrollRect.horizontal;
				this._isVertical = this._scrollRect.vertical;
				for (int i = 0; i < this._scrollRect.content.childCount; i++)
				{
					this.items.Add(this._scrollRect.content.GetChild(i).GetComponent<RectTransform>());
				}
				if (this._scrollRect.content.GetComponent<VerticalLayoutGroup>() != null)
				{
					this._verticalLayoutGroup = this._scrollRect.content.GetComponent<VerticalLayoutGroup>();
				}
				if (this._scrollRect.content.GetComponent<HorizontalLayoutGroup>() != null)
				{
					this._horizontalLayoutGroup = this._scrollRect.content.GetComponent<HorizontalLayoutGroup>();
				}
				if (this._scrollRect.content.GetComponent<GridLayoutGroup>() != null)
				{
					this._gridLayoutGroup = this._scrollRect.content.GetComponent<GridLayoutGroup>();
				}
				if (this._scrollRect.content.GetComponent<ContentSizeFitter>() != null)
				{
					this._contentSizeFitter = this._scrollRect.content.GetComponent<ContentSizeFitter>();
				}
			}
			else
			{
				Debug.LogError("UI_ScrollRectOcclusion => No ScrollRect component found");
			}
		}

		// Token: 0x060022D0 RID: 8912 RVA: 0x000C6CE4 File Offset: 0x000C50E4
		private void DisableGridComponents()
		{
			if (this._isVertical)
			{
				this._disableMarginY = this._scrollRect.GetComponent<RectTransform>().rect.height / 2f + this.items[0].sizeDelta.y;
			}
			if (this._isHorizontal)
			{
				this._disableMarginX = this._scrollRect.GetComponent<RectTransform>().rect.width / 2f + this.items[0].sizeDelta.x;
			}
			if (this._verticalLayoutGroup)
			{
				this._verticalLayoutGroup.enabled = false;
			}
			if (this._horizontalLayoutGroup)
			{
				this._horizontalLayoutGroup.enabled = false;
			}
			if (this._contentSizeFitter)
			{
				this._contentSizeFitter.enabled = false;
			}
			if (this._gridLayoutGroup)
			{
				this._gridLayoutGroup.enabled = false;
			}
			this.hasDisabledGridComponents = true;
		}

		// Token: 0x060022D1 RID: 8913 RVA: 0x000C6DFC File Offset: 0x000C51FC
		public void OnScroll(Vector2 pos)
		{
			if (!this.hasDisabledGridComponents)
			{
				this.DisableGridComponents();
			}
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this._isVertical && this._isHorizontal)
				{
					if (this._scrollRect.transform.InverseTransformPoint(this.items[i].position).y < -this._disableMarginY || this._scrollRect.transform.InverseTransformPoint(this.items[i].position).y > this._disableMarginY || this._scrollRect.transform.InverseTransformPoint(this.items[i].position).x < -this._disableMarginX || this._scrollRect.transform.InverseTransformPoint(this.items[i].position).x > this._disableMarginX)
					{
						this.items[i].gameObject.SetActive(false);
					}
					else
					{
						this.items[i].gameObject.SetActive(true);
					}
				}
				else
				{
					if (this._isVertical)
					{
						if (this._scrollRect.transform.InverseTransformPoint(this.items[i].position).y < -this._disableMarginY || this._scrollRect.transform.InverseTransformPoint(this.items[i].position).y > this._disableMarginY)
						{
							this.items[i].gameObject.SetActive(false);
						}
						else
						{
							this.items[i].gameObject.SetActive(true);
						}
					}
					if (this._isHorizontal)
					{
						if (this._scrollRect.transform.InverseTransformPoint(this.items[i].position).x < -this._disableMarginX || this._scrollRect.transform.InverseTransformPoint(this.items[i].position).x > this._disableMarginX)
						{
							this.items[i].gameObject.SetActive(false);
						}
						else
						{
							this.items[i].gameObject.SetActive(true);
						}
					}
				}
			}
		}

		// Token: 0x04001CC4 RID: 7364
		public bool InitByUser;

		// Token: 0x04001CC5 RID: 7365
		private ScrollRect _scrollRect;

		// Token: 0x04001CC6 RID: 7366
		private ContentSizeFitter _contentSizeFitter;

		// Token: 0x04001CC7 RID: 7367
		private VerticalLayoutGroup _verticalLayoutGroup;

		// Token: 0x04001CC8 RID: 7368
		private HorizontalLayoutGroup _horizontalLayoutGroup;

		// Token: 0x04001CC9 RID: 7369
		private GridLayoutGroup _gridLayoutGroup;

		// Token: 0x04001CCA RID: 7370
		private bool _isVertical;

		// Token: 0x04001CCB RID: 7371
		private bool _isHorizontal;

		// Token: 0x04001CCC RID: 7372
		private float _disableMarginX;

		// Token: 0x04001CCD RID: 7373
		private float _disableMarginY;

		// Token: 0x04001CCE RID: 7374
		private bool hasDisabledGridComponents;

		// Token: 0x04001CCF RID: 7375
		private List<RectTransform> items = new List<RectTransform>();
	}
}
