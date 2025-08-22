using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000525 RID: 1317
	[RequireComponent(typeof(ScrollRect))]
	[AddComponentMenu("Layout/Extensions/Vertical Scroll Snap")]
	public class VerticalScrollSnap : ScrollSnapBase, IEndDragHandler, IEventSystemHandler
	{
		// Token: 0x06002156 RID: 8534 RVA: 0x000BE969 File Offset: 0x000BCD69
		public VerticalScrollSnap()
		{
		}

		// Token: 0x06002157 RID: 8535 RVA: 0x000BE974 File Offset: 0x000BCD74
		private void Start()
		{
			this._isVertical = true;
			this._childAnchorPoint = new Vector2(0.5f, 0f);
			this._currentPage = this.StartingScreen;
			this.panelDimensions = base.gameObject.GetComponent<RectTransform>().rect;
			this.UpdateLayout();
		}

		// Token: 0x06002158 RID: 8536 RVA: 0x000BE9C8 File Offset: 0x000BCDC8
		private void Update()
		{
			if (!this._lerp && this._scroll_rect.velocity == Vector2.zero)
			{
				if (!this._settled && !this._pointerDown && !base.IsRectSettledOnaPage(this._screensContainer.localPosition))
				{
					base.ScrollToClosestElement();
				}
				return;
			}
			if (this._lerp)
			{
				this._screensContainer.localPosition = Vector3.Lerp(this._screensContainer.localPosition, this._lerp_target, this.transitionSpeed * Time.deltaTime);
				if (Vector3.Distance(this._screensContainer.localPosition, this._lerp_target) < 0.1f)
				{
					this._screensContainer.localPosition = this._lerp_target;
					this._lerp = false;
					base.EndScreenChange();
				}
			}
			base.CurrentPage = base.GetPageforPosition(this._screensContainer.localPosition);
			if (!this._pointerDown && ((double)this._scroll_rect.velocity.y > 0.01 || (double)this._scroll_rect.velocity.y < -0.01) && this.IsRectMovingSlowerThanThreshold(0f))
			{
				base.ScrollToClosestElement();
			}
		}

		// Token: 0x06002159 RID: 8537 RVA: 0x000BEB20 File Offset: 0x000BCF20
		private bool IsRectMovingSlowerThanThreshold(float startingSpeed)
		{
			return (this._scroll_rect.velocity.y > startingSpeed && this._scroll_rect.velocity.y < (float)this.SwipeVelocityThreshold) || (this._scroll_rect.velocity.y < startingSpeed && this._scroll_rect.velocity.y > (float)(-(float)this.SwipeVelocityThreshold));
		}

		// Token: 0x0600215A RID: 8538 RVA: 0x000BEBA4 File Offset: 0x000BCFA4
		public void DistributePages()
		{
			this._screens = this._screensContainer.childCount;
			this._scroll_rect.verticalNormalizedPosition = 0f;
			float num = 0f;
			Rect rect = base.gameObject.GetComponent<RectTransform>().rect;
			float num2 = 0f;
			float num3 = this._childSize = (float)((int)rect.height) * ((this.PageStep != 0f) ? this.PageStep : 3f);
			for (int i = 0; i < this._screensContainer.transform.childCount; i++)
			{
				RectTransform component = this._screensContainer.transform.GetChild(i).gameObject.GetComponent<RectTransform>();
				num2 = num + (float)i * num3;
				component.sizeDelta = new Vector2(rect.width, rect.height);
				component.anchoredPosition = new Vector2(0f, num2);
				RectTransform rectTransform = component;
				Vector2 vector = this._childAnchorPoint;
				component.pivot = vector;
				vector = vector;
				component.anchorMax = vector;
				rectTransform.anchorMin = vector;
			}
			float y = num2 + num * -1f;
			this._screensContainer.GetComponent<RectTransform>().offsetMax = new Vector2(0f, y);
		}

		// Token: 0x0600215B RID: 8539 RVA: 0x000BECF0 File Offset: 0x000BD0F0
		public void AddChild(GameObject GO)
		{
			this.AddChild(GO, false);
		}

		// Token: 0x0600215C RID: 8540 RVA: 0x000BECFC File Offset: 0x000BD0FC
		public void AddChild(GameObject GO, bool WorldPositionStays)
		{
			this._scroll_rect.verticalNormalizedPosition = 0f;
			GO.transform.SetParent(this._screensContainer, WorldPositionStays);
			base.InitialiseChildObjectsFromScene();
			this.DistributePages();
			if (this.MaskArea)
			{
				base.UpdateVisible();
			}
			this.SetScrollContainerPosition();
		}

		// Token: 0x0600215D RID: 8541 RVA: 0x000BED53 File Offset: 0x000BD153
		public void RemoveChild(int index, out GameObject ChildRemoved)
		{
			this.RemoveChild(index, false, out ChildRemoved);
		}

		// Token: 0x0600215E RID: 8542 RVA: 0x000BED60 File Offset: 0x000BD160
		public void RemoveChild(int index, bool WorldPositionStays, out GameObject ChildRemoved)
		{
			ChildRemoved = null;
			if (index < 0 || index > this._screensContainer.childCount)
			{
				return;
			}
			this._scroll_rect.verticalNormalizedPosition = 0f;
			Transform child = this._screensContainer.transform.GetChild(index);
			child.SetParent(null, WorldPositionStays);
			ChildRemoved = child.gameObject;
			base.InitialiseChildObjectsFromScene();
			this.DistributePages();
			if (this.MaskArea)
			{
				base.UpdateVisible();
			}
			if (this._currentPage > this._screens - 1)
			{
				base.CurrentPage = this._screens - 1;
			}
			this.SetScrollContainerPosition();
		}

		// Token: 0x0600215F RID: 8543 RVA: 0x000BEE04 File Offset: 0x000BD204
		public void RemoveAllChildren(out GameObject[] ChildrenRemoved)
		{
			this.RemoveAllChildren(false, out ChildrenRemoved);
		}

		// Token: 0x06002160 RID: 8544 RVA: 0x000BEE10 File Offset: 0x000BD210
		public void RemoveAllChildren(bool WorldPositionStays, out GameObject[] ChildrenRemoved)
		{
			int childCount = this._screensContainer.childCount;
			ChildrenRemoved = new GameObject[childCount];
			for (int i = childCount - 1; i >= 0; i--)
			{
				ChildrenRemoved[i] = this._screensContainer.GetChild(i).gameObject;
				ChildrenRemoved[i].transform.SetParent(null, WorldPositionStays);
			}
			this._scroll_rect.verticalNormalizedPosition = 0f;
			base.CurrentPage = 0;
			base.InitialiseChildObjectsFromScene();
			this.DistributePages();
			if (this.MaskArea)
			{
				base.UpdateVisible();
			}
		}

		// Token: 0x06002161 RID: 8545 RVA: 0x000BEEA4 File Offset: 0x000BD2A4
		private void SetScrollContainerPosition()
		{
			this._scrollStartPosition = this._screensContainer.localPosition.y;
			this._scroll_rect.verticalNormalizedPosition = (float)this._currentPage / (float)(this._screens - 1);
			base.OnCurrentScreenChange(this._currentPage);
		}

		// Token: 0x06002162 RID: 8546 RVA: 0x000BEEF2 File Offset: 0x000BD2F2
		public void UpdateLayout()
		{
			this._lerp = false;
			this.DistributePages();
			if (this.MaskArea)
			{
				base.UpdateVisible();
			}
			this.SetScrollContainerPosition();
			base.OnCurrentScreenChange(this._currentPage);
		}

		// Token: 0x06002163 RID: 8547 RVA: 0x000BEF29 File Offset: 0x000BD329
		private void OnRectTransformDimensionsChange()
		{
			if (this._childAnchorPoint != Vector2.zero)
			{
				this.UpdateLayout();
			}
		}

		// Token: 0x06002164 RID: 8548 RVA: 0x000BEF48 File Offset: 0x000BD348
		private void OnEnable()
		{
			base.InitialiseChildObjectsFromScene();
			this.DistributePages();
			if (this.MaskArea)
			{
				base.UpdateVisible();
			}
			if (this.JumpOnEnable || !this.RestartOnEnable)
			{
				this.SetScrollContainerPosition();
			}
			if (this.RestartOnEnable)
			{
				base.GoToScreen(this.StartingScreen);
			}
		}

		// Token: 0x06002165 RID: 8549 RVA: 0x000BEFAC File Offset: 0x000BD3AC
		public void OnEndDrag(PointerEventData eventData)
		{
			this._pointerDown = false;
			if (this._scroll_rect.vertical)
			{
				float num = Vector3.Distance(this._startPosition, this._screensContainer.localPosition);
				if (this.UseFastSwipe && num < this.panelDimensions.height + (float)this.FastSwipeThreshold && num >= 1f)
				{
					this._scroll_rect.velocity = Vector3.zero;
					if (this._startPosition.y - this._screensContainer.localPosition.y > 0f)
					{
						base.NextScreen();
					}
					else
					{
						base.PreviousScreen();
					}
				}
			}
		}
	}
}
