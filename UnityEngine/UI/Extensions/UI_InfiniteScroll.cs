using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000558 RID: 1368
	[AddComponentMenu("UI/Extensions/UI Infinite Scroll")]
	public class UI_InfiniteScroll : MonoBehaviour
	{
		// Token: 0x060022C8 RID: 8904 RVA: 0x000C64D8 File Offset: 0x000C48D8
		public UI_InfiniteScroll()
		{
		}

		// Token: 0x060022C9 RID: 8905 RVA: 0x000C6501 File Offset: 0x000C4901
		private void Awake()
		{
			if (!this.InitByUser)
			{
				this.Init();
			}
		}

		// Token: 0x060022CA RID: 8906 RVA: 0x000C6514 File Offset: 0x000C4914
		public void Init()
		{
			if (base.GetComponent<ScrollRect>() != null)
			{
				this._scrollRect = base.GetComponent<ScrollRect>();
				this._scrollRect.onValueChanged.AddListener(new UnityAction<Vector2>(this.OnScroll));
				this._scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
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
				this._isHorizontal = this._scrollRect.horizontal;
				this._isVertical = this._scrollRect.vertical;
				if (this._isHorizontal && this._isVertical)
				{
					Debug.LogError("UI_InfiniteScroll doesn't support scrolling in both directions, plase choose one direction (horizontal or vertical)");
				}
				this._itemCount = this._scrollRect.content.childCount;
			}
			else
			{
				Debug.LogError("UI_InfiniteScroll => No ScrollRect component found");
			}
		}

		// Token: 0x060022CB RID: 8907 RVA: 0x000C66D4 File Offset: 0x000C4AD4
		private void DisableGridComponents()
		{
			if (this._isVertical)
			{
				this._recordOffsetY = this.items[0].GetComponent<RectTransform>().anchoredPosition.y - this.items[1].GetComponent<RectTransform>().anchoredPosition.y;
				this._disableMarginY = this._recordOffsetY * (float)this._itemCount / 2f;
			}
			if (this._isHorizontal)
			{
				this._recordOffsetX = this.items[1].GetComponent<RectTransform>().anchoredPosition.x - this.items[0].GetComponent<RectTransform>().anchoredPosition.x;
				this._disableMarginX = this._recordOffsetX * (float)this._itemCount / 2f;
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
			this._hasDisabledGridComponents = true;
		}

		// Token: 0x060022CC RID: 8908 RVA: 0x000C6828 File Offset: 0x000C4C28
		public void OnScroll(Vector2 pos)
		{
			if (!this._hasDisabledGridComponents)
			{
				this.DisableGridComponents();
			}
			for (int i = 0; i < this.items.Count; i++)
			{
				if (this._isHorizontal)
				{
					if (this._scrollRect.transform.InverseTransformPoint(this.items[i].gameObject.transform.position).x > this._disableMarginX + this._treshold)
					{
						this._newAnchoredPosition = this.items[i].anchoredPosition;
						this._newAnchoredPosition.x = this._newAnchoredPosition.x - (float)this._itemCount * this._recordOffsetX;
						this.items[i].anchoredPosition = this._newAnchoredPosition;
						this._scrollRect.content.GetChild(this._itemCount - 1).transform.SetAsFirstSibling();
					}
					else if (this._scrollRect.transform.InverseTransformPoint(this.items[i].gameObject.transform.position).x < -this._disableMarginX)
					{
						this._newAnchoredPosition = this.items[i].anchoredPosition;
						this._newAnchoredPosition.x = this._newAnchoredPosition.x + (float)this._itemCount * this._recordOffsetX;
						this.items[i].anchoredPosition = this._newAnchoredPosition;
						this._scrollRect.content.GetChild(0).transform.SetAsLastSibling();
					}
				}
				if (this._isVertical)
				{
					if (this._scrollRect.transform.InverseTransformPoint(this.items[i].gameObject.transform.position).y > this._disableMarginY + this._treshold)
					{
						this._newAnchoredPosition = this.items[i].anchoredPosition;
						this._newAnchoredPosition.y = this._newAnchoredPosition.y - (float)this._itemCount * this._recordOffsetY;
						this.items[i].anchoredPosition = this._newAnchoredPosition;
						this._scrollRect.content.GetChild(this._itemCount - 1).transform.SetAsFirstSibling();
					}
					else if (this._scrollRect.transform.InverseTransformPoint(this.items[i].gameObject.transform.position).y < -this._disableMarginY)
					{
						this._newAnchoredPosition = this.items[i].anchoredPosition;
						this._newAnchoredPosition.y = this._newAnchoredPosition.y + (float)this._itemCount * this._recordOffsetY;
						this.items[i].anchoredPosition = this._newAnchoredPosition;
						this._scrollRect.content.GetChild(0).transform.SetAsLastSibling();
					}
				}
			}
		}

		// Token: 0x04001CB3 RID: 7347
		[Tooltip("If false, will Init automatically, otherwise you need to call Init() method")]
		public bool InitByUser;

		// Token: 0x04001CB4 RID: 7348
		private ScrollRect _scrollRect;

		// Token: 0x04001CB5 RID: 7349
		private ContentSizeFitter _contentSizeFitter;

		// Token: 0x04001CB6 RID: 7350
		private VerticalLayoutGroup _verticalLayoutGroup;

		// Token: 0x04001CB7 RID: 7351
		private HorizontalLayoutGroup _horizontalLayoutGroup;

		// Token: 0x04001CB8 RID: 7352
		private GridLayoutGroup _gridLayoutGroup;

		// Token: 0x04001CB9 RID: 7353
		private bool _isVertical;

		// Token: 0x04001CBA RID: 7354
		private bool _isHorizontal;

		// Token: 0x04001CBB RID: 7355
		private float _disableMarginX;

		// Token: 0x04001CBC RID: 7356
		private float _disableMarginY;

		// Token: 0x04001CBD RID: 7357
		private bool _hasDisabledGridComponents;

		// Token: 0x04001CBE RID: 7358
		private List<RectTransform> items = new List<RectTransform>();

		// Token: 0x04001CBF RID: 7359
		private Vector2 _newAnchoredPosition = Vector2.zero;

		// Token: 0x04001CC0 RID: 7360
		private float _treshold = 100f;

		// Token: 0x04001CC1 RID: 7361
		private int _itemCount;

		// Token: 0x04001CC2 RID: 7362
		private float _recordOffsetX;

		// Token: 0x04001CC3 RID: 7363
		private float _recordOffsetY;
	}
}
