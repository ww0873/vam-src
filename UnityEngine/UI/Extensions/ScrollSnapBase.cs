using System;
using System.Runtime.CompilerServices;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200051C RID: 1308
	public class ScrollSnapBase : MonoBehaviour, IBeginDragHandler, IDragHandler, IScrollSnap, IEventSystemHandler
	{
		// Token: 0x060020F9 RID: 8441 RVA: 0x000BB2A0 File Offset: 0x000B96A0
		public ScrollSnapBase()
		{
		}

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x060020FA RID: 8442 RVA: 0x000BB329 File Offset: 0x000B9729
		// (set) Token: 0x060020FB RID: 8443 RVA: 0x000BB334 File Offset: 0x000B9734
		public int CurrentPage
		{
			get
			{
				return this._currentPage;
			}
			internal set
			{
				if ((value != this._currentPage && value >= 0 && value < this._screensContainer.childCount) || (value == 0 && this._screensContainer.childCount == 0))
				{
					this._previousPage = this._currentPage;
					this._currentPage = value;
					if (this.MaskArea)
					{
						this.UpdateVisible();
					}
					if (!this._lerp)
					{
						this.ScreenChange();
					}
					this.OnCurrentScreenChange(this._currentPage);
				}
			}
		}

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x060020FC RID: 8444 RVA: 0x000BB3C1 File Offset: 0x000B97C1
		// (set) Token: 0x060020FD RID: 8445 RVA: 0x000BB3C9 File Offset: 0x000B97C9
		public ScrollSnapBase.SelectionChangeStartEvent OnSelectionChangeStartEvent
		{
			get
			{
				return this.m_OnSelectionChangeStartEvent;
			}
			set
			{
				this.m_OnSelectionChangeStartEvent = value;
			}
		}

		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x060020FE RID: 8446 RVA: 0x000BB3D2 File Offset: 0x000B97D2
		// (set) Token: 0x060020FF RID: 8447 RVA: 0x000BB3DA File Offset: 0x000B97DA
		public ScrollSnapBase.SelectionPageChangedEvent OnSelectionPageChangedEvent
		{
			get
			{
				return this.m_OnSelectionPageChangedEvent;
			}
			set
			{
				this.m_OnSelectionPageChangedEvent = value;
			}
		}

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x06002100 RID: 8448 RVA: 0x000BB3E3 File Offset: 0x000B97E3
		// (set) Token: 0x06002101 RID: 8449 RVA: 0x000BB3EB File Offset: 0x000B97EB
		public ScrollSnapBase.SelectionChangeEndEvent OnSelectionChangeEndEvent
		{
			get
			{
				return this.m_OnSelectionChangeEndEvent;
			}
			set
			{
				this.m_OnSelectionChangeEndEvent = value;
			}
		}

		// Token: 0x06002102 RID: 8450 RVA: 0x000BB3F4 File Offset: 0x000B97F4
		private void Awake()
		{
			if (this._scroll_rect == null)
			{
				this._scroll_rect = base.gameObject.GetComponent<ScrollRect>();
			}
			if (this._scroll_rect.horizontalScrollbar && this._scroll_rect.horizontal)
			{
				ScrollSnapScrollbarHelper scrollSnapScrollbarHelper = this._scroll_rect.horizontalScrollbar.gameObject.AddComponent<ScrollSnapScrollbarHelper>();
				scrollSnapScrollbarHelper.ss = this;
			}
			if (this._scroll_rect.verticalScrollbar && this._scroll_rect.vertical)
			{
				ScrollSnapScrollbarHelper scrollSnapScrollbarHelper2 = this._scroll_rect.verticalScrollbar.gameObject.AddComponent<ScrollSnapScrollbarHelper>();
				scrollSnapScrollbarHelper2.ss = this;
			}
			this.panelDimensions = base.gameObject.GetComponent<RectTransform>().rect;
			if (this.StartingScreen < 0)
			{
				this.StartingScreen = 0;
			}
			this._screensContainer = this._scroll_rect.content;
			this.InitialiseChildObjects();
			if (this.NextButton)
			{
				this.NextButton.GetComponent<Button>().onClick.AddListener(new UnityAction(this.<Awake>m__0));
			}
			if (this.PrevButton)
			{
				this.PrevButton.GetComponent<Button>().onClick.AddListener(new UnityAction(this.<Awake>m__1));
			}
		}

		// Token: 0x06002103 RID: 8451 RVA: 0x000BB54C File Offset: 0x000B994C
		internal void InitialiseChildObjects()
		{
			if (this.ChildObjects != null && this.ChildObjects.Length > 0)
			{
				if (this._screensContainer.transform.childCount > 0)
				{
					Debug.LogError("ScrollRect Content has children, this is not supported when using managed Child Objects\n Either remove the ScrollRect Content children or clear the ChildObjects array");
					return;
				}
				this.InitialiseChildObjectsFromArray();
			}
			else
			{
				this.InitialiseChildObjectsFromScene();
			}
		}

		// Token: 0x06002104 RID: 8452 RVA: 0x000BB5A4 File Offset: 0x000B99A4
		internal void InitialiseChildObjectsFromScene()
		{
			int childCount = this._screensContainer.childCount;
			this.ChildObjects = new GameObject[childCount];
			for (int i = 0; i < childCount; i++)
			{
				this.ChildObjects[i] = this._screensContainer.transform.GetChild(i).gameObject;
				if (this.MaskArea && this.ChildObjects[i].activeSelf)
				{
					this.ChildObjects[i].SetActive(false);
				}
			}
		}

		// Token: 0x06002105 RID: 8453 RVA: 0x000BB62C File Offset: 0x000B9A2C
		internal void InitialiseChildObjectsFromArray()
		{
			int num = this.ChildObjects.Length;
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.ChildObjects[i]);
				if (this.UseParentTransform)
				{
					RectTransform component = gameObject.GetComponent<RectTransform>();
					component.rotation = this._screensContainer.rotation;
					component.localScale = this._screensContainer.localScale;
					component.position = this._screensContainer.position;
				}
				gameObject.transform.SetParent(this._screensContainer.transform);
				this.ChildObjects[i] = gameObject;
				if (this.MaskArea && this.ChildObjects[i].activeSelf)
				{
					this.ChildObjects[i].SetActive(false);
				}
			}
		}

		// Token: 0x06002106 RID: 8454 RVA: 0x000BB6F8 File Offset: 0x000B9AF8
		internal void UpdateVisible()
		{
			if (!this.MaskArea || this.ChildObjects == null || this.ChildObjects.Length < 1 || this._screensContainer.childCount < 1)
			{
				return;
			}
			this._maskSize = ((!this._isVertical) ? this.MaskArea.rect.width : this.MaskArea.rect.height);
			this._halfNoVisibleItems = (int)Math.Round((double)(this._maskSize / (this._childSize * this.MaskBuffer)), MidpointRounding.AwayFromZero) / 2;
			this._bottomItem = (this._topItem = 0);
			for (int i = this._halfNoVisibleItems + 1; i > 0; i--)
			{
				this._bottomItem = ((this._currentPage - i >= 0) ? i : 0);
				if (this._bottomItem > 0)
				{
					break;
				}
			}
			for (int j = this._halfNoVisibleItems + 1; j > 0; j--)
			{
				this._topItem = ((this._screensContainer.childCount - this._currentPage - j >= 0) ? j : 0);
				if (this._topItem > 0)
				{
					break;
				}
			}
			for (int k = this.CurrentPage - this._bottomItem; k < this.CurrentPage + this._topItem; k++)
			{
				try
				{
					this.ChildObjects[k].SetActive(true);
				}
				catch
				{
					Debug.Log("Failed to setactive child [" + k + "]");
				}
			}
			if (this._currentPage > this._halfNoVisibleItems)
			{
				this.ChildObjects[this.CurrentPage - this._bottomItem].SetActive(false);
			}
			if (this._screensContainer.childCount - this._currentPage > this._topItem)
			{
				this.ChildObjects[this.CurrentPage + this._topItem].SetActive(false);
			}
		}

		// Token: 0x06002107 RID: 8455 RVA: 0x000BB924 File Offset: 0x000B9D24
		public void NextScreen()
		{
			if (this._currentPage < this._screens - 1)
			{
				if (!this._lerp)
				{
					this.StartScreenChange();
				}
				this._lerp = true;
				this.CurrentPage = this._currentPage + 1;
				this.GetPositionforPage(this._currentPage, ref this._lerp_target);
				this.ScreenChange();
			}
		}

		// Token: 0x06002108 RID: 8456 RVA: 0x000BB984 File Offset: 0x000B9D84
		public void PreviousScreen()
		{
			if (this._currentPage > 0)
			{
				if (!this._lerp)
				{
					this.StartScreenChange();
				}
				this._lerp = true;
				this.CurrentPage = this._currentPage - 1;
				this.GetPositionforPage(this._currentPage, ref this._lerp_target);
				this.ScreenChange();
			}
		}

		// Token: 0x06002109 RID: 8457 RVA: 0x000BB9DC File Offset: 0x000B9DDC
		public void GoToScreen(int screenIndex)
		{
			if (screenIndex <= this._screens - 1 && screenIndex >= 0)
			{
				if (!this._lerp)
				{
					this.StartScreenChange();
				}
				this._lerp = true;
				this.CurrentPage = screenIndex;
				this.GetPositionforPage(this._currentPage, ref this._lerp_target);
				this.ScreenChange();
			}
		}

		// Token: 0x0600210A RID: 8458 RVA: 0x000BBA38 File Offset: 0x000B9E38
		internal int GetPageforPosition(Vector3 pos)
		{
			return (!this._isVertical) ? ((int)Math.Round((double)((this._scrollStartPosition - pos.x) / this._childSize))) : ((int)Math.Round((double)((this._scrollStartPosition - pos.y) / this._childSize)));
		}

		// Token: 0x0600210B RID: 8459 RVA: 0x000BBA90 File Offset: 0x000B9E90
		internal bool IsRectSettledOnaPage(Vector3 pos)
		{
			return (!this._isVertical) ? (-((pos.x - this._scrollStartPosition) / this._childSize) == (float)(-(float)((int)Math.Round((double)((pos.x - this._scrollStartPosition) / this._childSize))))) : (-((pos.y - this._scrollStartPosition) / this._childSize) == (float)(-(float)((int)Math.Round((double)((pos.y - this._scrollStartPosition) / this._childSize)))));
		}

		// Token: 0x0600210C RID: 8460 RVA: 0x000BBB1C File Offset: 0x000B9F1C
		internal void GetPositionforPage(int page, ref Vector3 target)
		{
			this._childPos = -this._childSize * (float)page;
			if (this._isVertical)
			{
				target.y = this._childPos + this._scrollStartPosition;
			}
			else
			{
				target.x = this._childPos + this._scrollStartPosition;
			}
		}

		// Token: 0x0600210D RID: 8461 RVA: 0x000BBB6F File Offset: 0x000B9F6F
		internal void ScrollToClosestElement()
		{
			this._lerp = true;
			this.CurrentPage = this.GetPageforPosition(this._screensContainer.localPosition);
			this.GetPositionforPage(this._currentPage, ref this._lerp_target);
			this.OnCurrentScreenChange(this._currentPage);
		}

		// Token: 0x0600210E RID: 8462 RVA: 0x000BBBAD File Offset: 0x000B9FAD
		internal void OnCurrentScreenChange(int currentScreen)
		{
			this.ChangeBulletsInfo(currentScreen);
			this.ToggleNavigationButtons(currentScreen);
		}

		// Token: 0x0600210F RID: 8463 RVA: 0x000BBBC0 File Offset: 0x000B9FC0
		private void ChangeBulletsInfo(int targetScreen)
		{
			if (this.Pagination)
			{
				for (int i = 0; i < this.Pagination.transform.childCount; i++)
				{
					this.Pagination.transform.GetChild(i).GetComponent<Toggle>().isOn = (targetScreen == i);
				}
			}
		}

		// Token: 0x06002110 RID: 8464 RVA: 0x000BBC28 File Offset: 0x000BA028
		private void ToggleNavigationButtons(int targetScreen)
		{
			if (this.PrevButton)
			{
				this.PrevButton.GetComponent<Button>().interactable = (targetScreen > 0);
			}
			if (this.NextButton)
			{
				this.NextButton.GetComponent<Button>().interactable = (targetScreen < this._screensContainer.transform.childCount - 1);
			}
		}

		// Token: 0x06002111 RID: 8465 RVA: 0x000BBC90 File Offset: 0x000BA090
		private void OnValidate()
		{
			if (this._scroll_rect == null)
			{
				this._scroll_rect = base.GetComponent<ScrollRect>();
			}
			if (!this._scroll_rect.horizontal && !this._scroll_rect.vertical)
			{
				Debug.LogError("ScrollRect has to have a direction, please select either Horizontal OR Vertical with the appropriate control.");
			}
			if (this._scroll_rect.horizontal && this._scroll_rect.vertical)
			{
				Debug.LogError("ScrollRect has to be unidirectional, only use either Horizontal or Vertical on the ScrollRect, NOT both.");
			}
			int childCount = base.gameObject.GetComponent<ScrollRect>().content.childCount;
			if (childCount != 0 || this.ChildObjects != null)
			{
				int num = (this.ChildObjects != null && this.ChildObjects.Length != 0) ? this.ChildObjects.Length : childCount;
				if (this.StartingScreen > num - 1)
				{
					this.StartingScreen = num - 1;
				}
				if (this.StartingScreen < 0)
				{
					this.StartingScreen = 0;
				}
			}
			if (this.MaskBuffer <= 0f)
			{
				this.MaskBuffer = 1f;
			}
			if (this.PageStep < 0f)
			{
				this.PageStep = 0f;
			}
			if (this.PageStep > 8f)
			{
				this.PageStep = 9f;
			}
		}

		// Token: 0x06002112 RID: 8466 RVA: 0x000BBDD7 File Offset: 0x000BA1D7
		public void StartScreenChange()
		{
			if (!this._moveStarted)
			{
				this._moveStarted = true;
				this.OnSelectionChangeStartEvent.Invoke();
			}
		}

		// Token: 0x06002113 RID: 8467 RVA: 0x000BBDF6 File Offset: 0x000BA1F6
		internal void ScreenChange()
		{
			this.OnSelectionPageChangedEvent.Invoke(this._currentPage);
		}

		// Token: 0x06002114 RID: 8468 RVA: 0x000BBE09 File Offset: 0x000BA209
		internal void EndScreenChange()
		{
			this.OnSelectionChangeEndEvent.Invoke(this._currentPage);
			this._settled = true;
			this._moveStarted = false;
		}

		// Token: 0x06002115 RID: 8469 RVA: 0x000BBE2A File Offset: 0x000BA22A
		public Transform CurrentPageObject()
		{
			return this._screensContainer.GetChild(this.CurrentPage);
		}

		// Token: 0x06002116 RID: 8470 RVA: 0x000BBE3D File Offset: 0x000BA23D
		public void CurrentPageObject(out Transform returnObject)
		{
			returnObject = this._screensContainer.GetChild(this.CurrentPage);
		}

		// Token: 0x06002117 RID: 8471 RVA: 0x000BBE52 File Offset: 0x000BA252
		public void OnBeginDrag(PointerEventData eventData)
		{
			this._pointerDown = true;
			this._settled = false;
			this.StartScreenChange();
			this._startPosition = this._screensContainer.localPosition;
		}

		// Token: 0x06002118 RID: 8472 RVA: 0x000BBE79 File Offset: 0x000BA279
		public void OnDrag(PointerEventData eventData)
		{
			this._lerp = false;
		}

		// Token: 0x06002119 RID: 8473 RVA: 0x000BBE84 File Offset: 0x000BA284
		int IScrollSnap.CurrentPage()
		{
			int pageforPosition = this.GetPageforPosition(this._screensContainer.localPosition);
			this.CurrentPage = pageforPosition;
			return pageforPosition;
		}

		// Token: 0x0600211A RID: 8474 RVA: 0x000BBEAB File Offset: 0x000BA2AB
		public void SetLerp(bool value)
		{
			this._lerp = value;
		}

		// Token: 0x0600211B RID: 8475 RVA: 0x000BBEB4 File Offset: 0x000BA2B4
		public void ChangePage(int page)
		{
			this.GoToScreen(page);
		}

		// Token: 0x0600211C RID: 8476 RVA: 0x000BBEBD File Offset: 0x000BA2BD
		[CompilerGenerated]
		private void <Awake>m__0()
		{
			this.NextScreen();
		}

		// Token: 0x0600211D RID: 8477 RVA: 0x000BBEC5 File Offset: 0x000BA2C5
		[CompilerGenerated]
		private void <Awake>m__1()
		{
			this.PreviousScreen();
		}

		// Token: 0x04001B9E RID: 7070
		internal Rect panelDimensions;

		// Token: 0x04001B9F RID: 7071
		internal RectTransform _screensContainer;

		// Token: 0x04001BA0 RID: 7072
		internal bool _isVertical;

		// Token: 0x04001BA1 RID: 7073
		internal int _screens = 1;

		// Token: 0x04001BA2 RID: 7074
		internal float _scrollStartPosition;

		// Token: 0x04001BA3 RID: 7075
		internal float _childSize;

		// Token: 0x04001BA4 RID: 7076
		private float _childPos;

		// Token: 0x04001BA5 RID: 7077
		private float _maskSize;

		// Token: 0x04001BA6 RID: 7078
		internal Vector2 _childAnchorPoint;

		// Token: 0x04001BA7 RID: 7079
		internal ScrollRect _scroll_rect;

		// Token: 0x04001BA8 RID: 7080
		internal Vector3 _lerp_target;

		// Token: 0x04001BA9 RID: 7081
		internal bool _lerp;

		// Token: 0x04001BAA RID: 7082
		internal bool _pointerDown;

		// Token: 0x04001BAB RID: 7083
		internal bool _settled = true;

		// Token: 0x04001BAC RID: 7084
		internal Vector3 _startPosition = default(Vector3);

		// Token: 0x04001BAD RID: 7085
		[Tooltip("The currently active page")]
		internal int _currentPage;

		// Token: 0x04001BAE RID: 7086
		internal int _previousPage;

		// Token: 0x04001BAF RID: 7087
		internal int _halfNoVisibleItems;

		// Token: 0x04001BB0 RID: 7088
		internal bool _moveStarted;

		// Token: 0x04001BB1 RID: 7089
		private int _bottomItem;

		// Token: 0x04001BB2 RID: 7090
		private int _topItem;

		// Token: 0x04001BB3 RID: 7091
		[Tooltip("The screen / page to start the control on\n*Note, this is a 0 indexed array")]
		[SerializeField]
		public int StartingScreen;

		// Token: 0x04001BB4 RID: 7092
		[Tooltip("The distance between two pages based on page height, by default pages are next to each other")]
		[SerializeField]
		[Range(0f, 8f)]
		public float PageStep = 1f;

		// Token: 0x04001BB5 RID: 7093
		[Tooltip("The gameobject that contains toggles which suggest pagination. (optional)")]
		public GameObject Pagination;

		// Token: 0x04001BB6 RID: 7094
		[Tooltip("Button to go to the previous page. (optional)")]
		public GameObject PrevButton;

		// Token: 0x04001BB7 RID: 7095
		[Tooltip("Button to go to the next page. (optional)")]
		public GameObject NextButton;

		// Token: 0x04001BB8 RID: 7096
		[Tooltip("Transition speed between pages. (optional)")]
		public float transitionSpeed = 7.5f;

		// Token: 0x04001BB9 RID: 7097
		[Tooltip("Fast Swipe makes swiping page next / previous (optional)")]
		public bool UseFastSwipe;

		// Token: 0x04001BBA RID: 7098
		[Tooltip("Offset for how far a swipe has to travel to initiate a page change (optional)")]
		public int FastSwipeThreshold = 100;

		// Token: 0x04001BBB RID: 7099
		[Tooltip("Speed at which the ScrollRect will keep scrolling before slowing down and stopping (optional)")]
		public int SwipeVelocityThreshold = 100;

		// Token: 0x04001BBC RID: 7100
		[Tooltip("The visible bounds area, controls which items are visible/enabled. *Note Should use a RectMask. (optional)")]
		public RectTransform MaskArea;

		// Token: 0x04001BBD RID: 7101
		[Tooltip("Pixel size to buffer arround Mask Area. (optional)")]
		public float MaskBuffer = 1f;

		// Token: 0x04001BBE RID: 7102
		[Tooltip("By default the container will lerp to the start when enabled in the scene, this option overrides this and forces it to simply jump without lerping")]
		public bool JumpOnEnable;

		// Token: 0x04001BBF RID: 7103
		[Tooltip("By default the container will return to the original starting page when enabled, this option overrides this behaviour and stays on the current selection")]
		public bool RestartOnEnable;

		// Token: 0x04001BC0 RID: 7104
		[Tooltip("(Experimental)\nBy default, child array objects will use the parent transform\nHowever you can disable this for some interesting effects")]
		public bool UseParentTransform = true;

		// Token: 0x04001BC1 RID: 7105
		[Tooltip("Scroll Snap children. (optional)\nEither place objects in the scene as children OR\nPrefabs in this array, NOT BOTH")]
		public GameObject[] ChildObjects;

		// Token: 0x04001BC2 RID: 7106
		[SerializeField]
		[Tooltip("Event fires when a user starts to change the selection")]
		private ScrollSnapBase.SelectionChangeStartEvent m_OnSelectionChangeStartEvent = new ScrollSnapBase.SelectionChangeStartEvent();

		// Token: 0x04001BC3 RID: 7107
		[SerializeField]
		[Tooltip("Event fires as the page changes, while dragging or jumping")]
		private ScrollSnapBase.SelectionPageChangedEvent m_OnSelectionPageChangedEvent = new ScrollSnapBase.SelectionPageChangedEvent();

		// Token: 0x04001BC4 RID: 7108
		[SerializeField]
		[Tooltip("Event fires when the page settles after a user has dragged")]
		private ScrollSnapBase.SelectionChangeEndEvent m_OnSelectionChangeEndEvent = new ScrollSnapBase.SelectionChangeEndEvent();

		// Token: 0x0200051D RID: 1309
		[Serializable]
		public class SelectionChangeStartEvent : UnityEvent
		{
			// Token: 0x0600211E RID: 8478 RVA: 0x000BBECD File Offset: 0x000BA2CD
			public SelectionChangeStartEvent()
			{
			}
		}

		// Token: 0x0200051E RID: 1310
		[Serializable]
		public class SelectionPageChangedEvent : UnityEvent<int>
		{
			// Token: 0x0600211F RID: 8479 RVA: 0x000BBED5 File Offset: 0x000BA2D5
			public SelectionPageChangedEvent()
			{
			}
		}

		// Token: 0x0200051F RID: 1311
		[Serializable]
		public class SelectionChangeEndEvent : UnityEvent<int>
		{
			// Token: 0x06002120 RID: 8480 RVA: 0x000BBEDD File Offset: 0x000BA2DD
			public SelectionChangeEndEvent()
			{
			}
		}
	}
}
