using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000519 RID: 1305
	[ExecuteInEditMode]
	[RequireComponent(typeof(ScrollRect))]
	[AddComponentMenu("UI/Extensions/Scroll Snap")]
	public class ScrollSnap : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler, IScrollSnap, IEventSystemHandler
	{
		// Token: 0x060020DE RID: 8414 RVA: 0x000BCD78 File Offset: 0x000BB178
		public ScrollSnap()
		{
		}

		// Token: 0x1400008D RID: 141
		// (add) Token: 0x060020DF RID: 8415 RVA: 0x000BCDC8 File Offset: 0x000BB1C8
		// (remove) Token: 0x060020E0 RID: 8416 RVA: 0x000BCE00 File Offset: 0x000BB200
		public event ScrollSnap.PageSnapChange onPageChange
		{
			add
			{
				ScrollSnap.PageSnapChange pageSnapChange = this.onPageChange;
				ScrollSnap.PageSnapChange pageSnapChange2;
				do
				{
					pageSnapChange2 = pageSnapChange;
					pageSnapChange = Interlocked.CompareExchange<ScrollSnap.PageSnapChange>(ref this.onPageChange, (ScrollSnap.PageSnapChange)Delegate.Combine(pageSnapChange2, value), pageSnapChange);
				}
				while (pageSnapChange != pageSnapChange2);
			}
			remove
			{
				ScrollSnap.PageSnapChange pageSnapChange = this.onPageChange;
				ScrollSnap.PageSnapChange pageSnapChange2;
				do
				{
					pageSnapChange2 = pageSnapChange;
					pageSnapChange = Interlocked.CompareExchange<ScrollSnap.PageSnapChange>(ref this.onPageChange, (ScrollSnap.PageSnapChange)Delegate.Remove(pageSnapChange2, value), pageSnapChange);
				}
				while (pageSnapChange != pageSnapChange2);
			}
		}

		// Token: 0x060020E1 RID: 8417 RVA: 0x000BCE38 File Offset: 0x000BB238
		private void Start()
		{
			this._lerp = false;
			this._scroll_rect = base.gameObject.GetComponent<ScrollRect>();
			this._scrollRectTransform = base.gameObject.GetComponent<RectTransform>();
			this._listContainerTransform = this._scroll_rect.content;
			this._listContainerRectTransform = this._listContainerTransform.GetComponent<RectTransform>();
			this.UpdateListItemsSize();
			this.UpdateListItemPositions();
			this.PageChanged(this.CurrentPage());
			if (this.NextButton)
			{
				this.NextButton.GetComponent<Button>().onClick.AddListener(new UnityAction(this.<Start>m__0));
			}
			if (this.PrevButton)
			{
				this.PrevButton.GetComponent<Button>().onClick.AddListener(new UnityAction(this.<Start>m__1));
			}
			if (this._scroll_rect.horizontalScrollbar != null && this._scroll_rect.horizontal)
			{
				ScrollSnapScrollbarHelper scrollSnapScrollbarHelper = this._scroll_rect.horizontalScrollbar.gameObject.AddComponent<ScrollSnapScrollbarHelper>();
				scrollSnapScrollbarHelper.ss = this;
			}
			if (this._scroll_rect.verticalScrollbar != null && this._scroll_rect.vertical)
			{
				ScrollSnapScrollbarHelper scrollSnapScrollbarHelper2 = this._scroll_rect.verticalScrollbar.gameObject.AddComponent<ScrollSnapScrollbarHelper>();
				scrollSnapScrollbarHelper2.ss = this;
			}
		}

		// Token: 0x060020E2 RID: 8418 RVA: 0x000BCF90 File Offset: 0x000BB390
		public void UpdateListItemsSize()
		{
			float num;
			float num2;
			if (this.direction == ScrollSnap.ScrollDirection.Horizontal)
			{
				num = this._scrollRectTransform.rect.width / (float)this.ItemsVisibleAtOnce;
				num2 = this._listContainerRectTransform.rect.width / (float)this._itemsCount;
			}
			else
			{
				num = this._scrollRectTransform.rect.height / (float)this.ItemsVisibleAtOnce;
				num2 = this._listContainerRectTransform.rect.height / (float)this._itemsCount;
			}
			this._itemSize = num;
			if (this.LinkScrolrectScrollSensitivity)
			{
				this._scroll_rect.scrollSensitivity = this._itemSize;
			}
			if (this.AutoLayoutItems && num2 != num && this._itemsCount > 0)
			{
				if (this.direction == ScrollSnap.ScrollDirection.Horizontal)
				{
					IEnumerator enumerator = this._listContainerTransform.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							GameObject gameObject = ((Transform)obj).gameObject;
							if (gameObject.activeInHierarchy)
							{
								LayoutElement layoutElement = gameObject.GetComponent<LayoutElement>();
								if (layoutElement == null)
								{
									layoutElement = gameObject.AddComponent<LayoutElement>();
								}
								layoutElement.minWidth = this._itemSize;
							}
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
				}
				else
				{
					IEnumerator enumerator2 = this._listContainerTransform.GetEnumerator();
					try
					{
						while (enumerator2.MoveNext())
						{
							object obj2 = enumerator2.Current;
							GameObject gameObject2 = ((Transform)obj2).gameObject;
							if (gameObject2.activeInHierarchy)
							{
								LayoutElement layoutElement2 = gameObject2.GetComponent<LayoutElement>();
								if (layoutElement2 == null)
								{
									layoutElement2 = gameObject2.AddComponent<LayoutElement>();
								}
								layoutElement2.minHeight = this._itemSize;
							}
						}
					}
					finally
					{
						IDisposable disposable2;
						if ((disposable2 = (enumerator2 as IDisposable)) != null)
						{
							disposable2.Dispose();
						}
					}
				}
			}
		}

		// Token: 0x060020E3 RID: 8419 RVA: 0x000BD1AC File Offset: 0x000BB5AC
		public void UpdateListItemPositions()
		{
			if (!this._listContainerRectTransform.rect.size.Equals(this._listContainerCachedSize))
			{
				int num = 0;
				IEnumerator enumerator = this._listContainerTransform.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						if (((Transform)obj).gameObject.activeInHierarchy)
						{
							num++;
						}
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
				this._itemsCount = 0;
				Array.Resize<Vector3>(ref this._pageAnchorPositions, num);
				if (num > 0)
				{
					this._pages = Mathf.Max(num - this.ItemsVisibleAtOnce + 1, 1);
					if (this.direction == ScrollSnap.ScrollDirection.Horizontal)
					{
						this._scroll_rect.horizontalNormalizedPosition = 0f;
						this._listContainerMaxPosition = this._listContainerTransform.localPosition.x;
						this._scroll_rect.horizontalNormalizedPosition = 1f;
						this._listContainerMinPosition = this._listContainerTransform.localPosition.x;
						this._listContainerSize = this._listContainerMaxPosition - this._listContainerMinPosition;
						for (int i = 0; i < this._pages; i++)
						{
							this._pageAnchorPositions[i] = new Vector3(this._listContainerMaxPosition - this._itemSize * (float)i, this._listContainerTransform.localPosition.y, this._listContainerTransform.localPosition.z);
						}
					}
					else
					{
						this._scroll_rect.verticalNormalizedPosition = 1f;
						this._listContainerMinPosition = this._listContainerTransform.localPosition.y;
						this._scroll_rect.verticalNormalizedPosition = 0f;
						this._listContainerMaxPosition = this._listContainerTransform.localPosition.y;
						this._listContainerSize = this._listContainerMaxPosition - this._listContainerMinPosition;
						for (int j = 0; j < this._pages; j++)
						{
							this._pageAnchorPositions[j] = new Vector3(this._listContainerTransform.localPosition.x, this._listContainerMinPosition + this._itemSize * (float)j, this._listContainerTransform.localPosition.z);
						}
					}
					this.UpdateScrollbar(this.LinkScrolbarSteps);
					this._startingPage = Mathf.Min(this._startingPage, this._pages);
					this.ResetPage();
				}
				if (this._itemsCount != num)
				{
					this.PageChanged(this.CurrentPage());
				}
				this._itemsCount = num;
				this._listContainerCachedSize.Set(this._listContainerRectTransform.rect.size.x, this._listContainerRectTransform.rect.size.y);
			}
		}

		// Token: 0x060020E4 RID: 8420 RVA: 0x000BD4C8 File Offset: 0x000BB8C8
		public void ResetPage()
		{
			if (this.direction == ScrollSnap.ScrollDirection.Horizontal)
			{
				this._scroll_rect.horizontalNormalizedPosition = ((this._pages <= 1) ? 0f : ((float)this._startingPage / (float)(this._pages - 1)));
			}
			else
			{
				this._scroll_rect.verticalNormalizedPosition = ((this._pages <= 1) ? 0f : ((float)(this._pages - this._startingPage - 1) / (float)(this._pages - 1)));
			}
		}

		// Token: 0x060020E5 RID: 8421 RVA: 0x000BD554 File Offset: 0x000BB954
		private void UpdateScrollbar(bool linkSteps)
		{
			if (linkSteps)
			{
				if (this.direction == ScrollSnap.ScrollDirection.Horizontal)
				{
					if (this._scroll_rect.horizontalScrollbar != null)
					{
						this._scroll_rect.horizontalScrollbar.numberOfSteps = this._pages;
					}
				}
				else if (this._scroll_rect.verticalScrollbar != null)
				{
					this._scroll_rect.verticalScrollbar.numberOfSteps = this._pages;
				}
			}
			else if (this.direction == ScrollSnap.ScrollDirection.Horizontal)
			{
				if (this._scroll_rect.horizontalScrollbar != null)
				{
					this._scroll_rect.horizontalScrollbar.numberOfSteps = 0;
				}
			}
			else if (this._scroll_rect.verticalScrollbar != null)
			{
				this._scroll_rect.verticalScrollbar.numberOfSteps = 0;
			}
		}

		// Token: 0x060020E6 RID: 8422 RVA: 0x000BD634 File Offset: 0x000BBA34
		private void LateUpdate()
		{
			this.UpdateListItemsSize();
			this.UpdateListItemPositions();
			if (this._lerp)
			{
				this.UpdateScrollbar(false);
				this._listContainerTransform.localPosition = Vector3.Lerp(this._listContainerTransform.localPosition, this._lerpTarget, 7.5f * Time.deltaTime);
				if (Vector3.Distance(this._listContainerTransform.localPosition, this._lerpTarget) < 0.001f)
				{
					this._listContainerTransform.localPosition = this._lerpTarget;
					this._lerp = false;
					this.UpdateScrollbar(this.LinkScrolbarSteps);
				}
				if (Vector3.Distance(this._listContainerTransform.localPosition, this._lerpTarget) < 10f)
				{
					this.PageChanged(this.CurrentPage());
				}
			}
			if (this._fastSwipeTimer)
			{
				this._fastSwipeCounter++;
			}
		}

		// Token: 0x060020E7 RID: 8423 RVA: 0x000BD714 File Offset: 0x000BBB14
		public void NextScreen()
		{
			this.UpdateListItemPositions();
			if (this.CurrentPage() < this._pages - 1)
			{
				this._lerp = true;
				this._lerpTarget = this._pageAnchorPositions[this.CurrentPage() + 1];
				this.PageChanged(this.CurrentPage() + 1);
			}
		}

		// Token: 0x060020E8 RID: 8424 RVA: 0x000BD770 File Offset: 0x000BBB70
		public void PreviousScreen()
		{
			this.UpdateListItemPositions();
			if (this.CurrentPage() > 0)
			{
				this._lerp = true;
				this._lerpTarget = this._pageAnchorPositions[this.CurrentPage() - 1];
				this.PageChanged(this.CurrentPage() - 1);
			}
		}

		// Token: 0x060020E9 RID: 8425 RVA: 0x000BD7C4 File Offset: 0x000BBBC4
		private void NextScreenCommand()
		{
			if (this._pageOnDragStart < this._pages - 1)
			{
				int num = Mathf.Min(this._pages - 1, this._pageOnDragStart + this.ItemsVisibleAtOnce);
				this._lerp = true;
				this._lerpTarget = this._pageAnchorPositions[num];
				this.PageChanged(num);
			}
		}

		// Token: 0x060020EA RID: 8426 RVA: 0x000BD824 File Offset: 0x000BBC24
		private void PrevScreenCommand()
		{
			if (this._pageOnDragStart > 0)
			{
				int num = Mathf.Max(0, this._pageOnDragStart - this.ItemsVisibleAtOnce);
				this._lerp = true;
				this._lerpTarget = this._pageAnchorPositions[num];
				this.PageChanged(num);
			}
		}

		// Token: 0x060020EB RID: 8427 RVA: 0x000BD878 File Offset: 0x000BBC78
		public int CurrentPage()
		{
			float num;
			if (this.direction == ScrollSnap.ScrollDirection.Horizontal)
			{
				num = this._listContainerMaxPosition - this._listContainerTransform.localPosition.x;
				num = Mathf.Clamp(num, 0f, this._listContainerSize);
			}
			else
			{
				num = this._listContainerTransform.localPosition.y - this._listContainerMinPosition;
				num = Mathf.Clamp(num, 0f, this._listContainerSize);
			}
			float f = num / this._itemSize;
			return Mathf.Clamp(Mathf.RoundToInt(f), 0, this._pages);
		}

		// Token: 0x060020EC RID: 8428 RVA: 0x000BD90A File Offset: 0x000BBD0A
		public void SetLerp(bool value)
		{
			this._lerp = value;
		}

		// Token: 0x060020ED RID: 8429 RVA: 0x000BD913 File Offset: 0x000BBD13
		public void ChangePage(int page)
		{
			if (0 <= page && page < this._pages)
			{
				this._lerp = true;
				this._lerpTarget = this._pageAnchorPositions[page];
				this.PageChanged(page);
			}
		}

		// Token: 0x060020EE RID: 8430 RVA: 0x000BD950 File Offset: 0x000BBD50
		private void PageChanged(int currentPage)
		{
			this._startingPage = currentPage;
			if (this.NextButton)
			{
				this.NextButton.interactable = (currentPage < this._pages - 1);
			}
			if (this.PrevButton)
			{
				this.PrevButton.interactable = (currentPage > 0);
			}
			if (this.onPageChange != null)
			{
				this.onPageChange(currentPage);
			}
		}

		// Token: 0x060020EF RID: 8431 RVA: 0x000BD9C0 File Offset: 0x000BBDC0
		public void OnBeginDrag(PointerEventData eventData)
		{
			this.UpdateScrollbar(false);
			this._fastSwipeCounter = 0;
			this._fastSwipeTimer = true;
			this._positionOnDragStart = eventData.position;
			this._pageOnDragStart = this.CurrentPage();
		}

		// Token: 0x060020F0 RID: 8432 RVA: 0x000BD9F4 File Offset: 0x000BBDF4
		public void OnEndDrag(PointerEventData eventData)
		{
			this._startDrag = true;
			float num;
			if (this.direction == ScrollSnap.ScrollDirection.Horizontal)
			{
				num = this._positionOnDragStart.x - eventData.position.x;
			}
			else
			{
				num = -this._positionOnDragStart.y + eventData.position.y;
			}
			if (this.UseFastSwipe)
			{
				this.fastSwipe = false;
				this._fastSwipeTimer = false;
				if (this._fastSwipeCounter <= this._fastSwipeTarget && Math.Abs(num) > (float)this.FastSwipeThreshold)
				{
					this.fastSwipe = true;
				}
				if (this.fastSwipe)
				{
					if (num > 0f)
					{
						this.NextScreenCommand();
					}
					else
					{
						this.PrevScreenCommand();
					}
				}
				else
				{
					this._lerp = true;
					this._lerpTarget = this._pageAnchorPositions[this.CurrentPage()];
				}
			}
			else
			{
				this._lerp = true;
				this._lerpTarget = this._pageAnchorPositions[this.CurrentPage()];
			}
		}

		// Token: 0x060020F1 RID: 8433 RVA: 0x000BDB0F File Offset: 0x000BBF0F
		public void OnDrag(PointerEventData eventData)
		{
			this._lerp = false;
			if (this._startDrag)
			{
				this.OnBeginDrag(eventData);
				this._startDrag = false;
			}
		}

		// Token: 0x060020F2 RID: 8434 RVA: 0x000BDB31 File Offset: 0x000BBF31
		public void StartScreenChange()
		{
		}

		// Token: 0x060020F3 RID: 8435 RVA: 0x000BDB33 File Offset: 0x000BBF33
		[CompilerGenerated]
		private void <Start>m__0()
		{
			this.NextScreen();
		}

		// Token: 0x060020F4 RID: 8436 RVA: 0x000BDB3B File Offset: 0x000BBF3B
		[CompilerGenerated]
		private void <Start>m__1()
		{
			this.PreviousScreen();
		}

		// Token: 0x04001B7B RID: 7035
		private ScrollRect _scroll_rect;

		// Token: 0x04001B7C RID: 7036
		private RectTransform _scrollRectTransform;

		// Token: 0x04001B7D RID: 7037
		private Transform _listContainerTransform;

		// Token: 0x04001B7E RID: 7038
		private int _pages;

		// Token: 0x04001B7F RID: 7039
		private int _startingPage;

		// Token: 0x04001B80 RID: 7040
		private Vector3[] _pageAnchorPositions;

		// Token: 0x04001B81 RID: 7041
		private Vector3 _lerpTarget;

		// Token: 0x04001B82 RID: 7042
		private bool _lerp;

		// Token: 0x04001B83 RID: 7043
		private float _listContainerMinPosition;

		// Token: 0x04001B84 RID: 7044
		private float _listContainerMaxPosition;

		// Token: 0x04001B85 RID: 7045
		private float _listContainerSize;

		// Token: 0x04001B86 RID: 7046
		private RectTransform _listContainerRectTransform;

		// Token: 0x04001B87 RID: 7047
		private Vector2 _listContainerCachedSize;

		// Token: 0x04001B88 RID: 7048
		private float _itemSize;

		// Token: 0x04001B89 RID: 7049
		private int _itemsCount;

		// Token: 0x04001B8A RID: 7050
		private bool _startDrag = true;

		// Token: 0x04001B8B RID: 7051
		private Vector3 _positionOnDragStart = default(Vector3);

		// Token: 0x04001B8C RID: 7052
		private int _pageOnDragStart;

		// Token: 0x04001B8D RID: 7053
		private bool _fastSwipeTimer;

		// Token: 0x04001B8E RID: 7054
		private int _fastSwipeCounter;

		// Token: 0x04001B8F RID: 7055
		private int _fastSwipeTarget = 10;

		// Token: 0x04001B90 RID: 7056
		[Tooltip("Button to go to the next page. (optional)")]
		public Button NextButton;

		// Token: 0x04001B91 RID: 7057
		[Tooltip("Button to go to the previous page. (optional)")]
		public Button PrevButton;

		// Token: 0x04001B92 RID: 7058
		[Tooltip("Number of items visible in one page of scroll frame.")]
		[Range(1f, 100f)]
		public int ItemsVisibleAtOnce = 1;

		// Token: 0x04001B93 RID: 7059
		[Tooltip("Sets minimum width of list items to 1/itemsVisibleAtOnce.")]
		public bool AutoLayoutItems = true;

		// Token: 0x04001B94 RID: 7060
		[Tooltip("If you wish to update scrollbar numberOfSteps to number of active children on list.")]
		public bool LinkScrolbarSteps;

		// Token: 0x04001B95 RID: 7061
		[Tooltip("If you wish to update scrollrect sensitivity to size of list element.")]
		public bool LinkScrolrectScrollSensitivity;

		// Token: 0x04001B96 RID: 7062
		public bool UseFastSwipe = true;

		// Token: 0x04001B97 RID: 7063
		public int FastSwipeThreshold = 100;

		// Token: 0x04001B98 RID: 7064
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private ScrollSnap.PageSnapChange onPageChange;

		// Token: 0x04001B99 RID: 7065
		public ScrollSnap.ScrollDirection direction;

		// Token: 0x04001B9A RID: 7066
		private bool fastSwipe;

		// Token: 0x0200051A RID: 1306
		public enum ScrollDirection
		{
			// Token: 0x04001B9C RID: 7068
			Horizontal,
			// Token: 0x04001B9D RID: 7069
			Vertical
		}

		// Token: 0x0200051B RID: 1307
		// (Invoke) Token: 0x060020F6 RID: 8438
		public delegate void PageSnapChange(int page);
	}
}
