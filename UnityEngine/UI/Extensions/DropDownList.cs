using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004C5 RID: 1221
	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu("UI/Extensions/Dropdown List")]
	public class DropDownList : MonoBehaviour
	{
		// Token: 0x06001ECE RID: 7886 RVA: 0x000AF8BA File Offset: 0x000ADCBA
		public DropDownList()
		{
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06001ECF RID: 7887 RVA: 0x000AF8DB File Offset: 0x000ADCDB
		// (set) Token: 0x06001ED0 RID: 7888 RVA: 0x000AF8E3 File Offset: 0x000ADCE3
		public DropDownListItem SelectedItem
		{
			[CompilerGenerated]
			get
			{
				return this.<SelectedItem>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<SelectedItem>k__BackingField = value;
			}
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06001ED1 RID: 7889 RVA: 0x000AF8EC File Offset: 0x000ADCEC
		// (set) Token: 0x06001ED2 RID: 7890 RVA: 0x000AF8F4 File Offset: 0x000ADCF4
		public float ScrollBarWidth
		{
			get
			{
				return this._scrollBarWidth;
			}
			set
			{
				this._scrollBarWidth = value;
				this.RedrawPanel();
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06001ED3 RID: 7891 RVA: 0x000AF903 File Offset: 0x000ADD03
		// (set) Token: 0x06001ED4 RID: 7892 RVA: 0x000AF90B File Offset: 0x000ADD0B
		public int ItemsToDisplay
		{
			get
			{
				return this._itemsToDisplay;
			}
			set
			{
				this._itemsToDisplay = value;
				this.RedrawPanel();
			}
		}

		// Token: 0x06001ED5 RID: 7893 RVA: 0x000AF91A File Offset: 0x000ADD1A
		public void Start()
		{
			this.Initialize();
			if (this.SelectFirstItemOnStart && this.Items.Count > 0)
			{
				this.ToggleDropdownPanel(false);
				this.OnItemClicked(0);
			}
		}

		// Token: 0x06001ED6 RID: 7894 RVA: 0x000AF950 File Offset: 0x000ADD50
		private bool Initialize()
		{
			bool result = true;
			try
			{
				this._rectTransform = base.GetComponent<RectTransform>();
				this._mainButton = new DropDownListButton(this._rectTransform.Find("MainButton").gameObject);
				this._overlayRT = this._rectTransform.Find("Overlay").GetComponent<RectTransform>();
				this._overlayRT.gameObject.SetActive(false);
				this._scrollPanelRT = this._overlayRT.Find("ScrollPanel").GetComponent<RectTransform>();
				this._scrollBarRT = this._scrollPanelRT.Find("Scrollbar").GetComponent<RectTransform>();
				this._slidingAreaRT = this._scrollBarRT.Find("SlidingArea").GetComponent<RectTransform>();
				this._itemsPanelRT = this._scrollPanelRT.Find("Items").GetComponent<RectTransform>();
				this._canvas = base.GetComponentInParent<Canvas>();
				this._canvasRT = this._canvas.GetComponent<RectTransform>();
				this._scrollRect = this._scrollPanelRT.GetComponent<ScrollRect>();
				this._scrollRect.scrollSensitivity = this._rectTransform.sizeDelta.y / 2f;
				this._scrollRect.movementType = ScrollRect.MovementType.Clamped;
				this._scrollRect.content = this._itemsPanelRT;
				this._itemTemplate = this._rectTransform.Find("ItemTemplate").gameObject;
				this._itemTemplate.SetActive(false);
			}
			catch (NullReferenceException exception)
			{
				Debug.LogException(exception);
				Debug.LogError("Something is setup incorrectly with the dropdownlist component causing a Null Reference Exception");
				result = false;
			}
			this._panelItems = new List<DropDownListButton>();
			this.RebuildPanel();
			this.RedrawPanel();
			return result;
		}

		// Token: 0x06001ED7 RID: 7895 RVA: 0x000AFB0C File Offset: 0x000ADF0C
		private void RebuildPanel()
		{
			if (this.Items.Count == 0)
			{
				return;
			}
			int num = this._panelItems.Count;
			while (this._panelItems.Count < this.Items.Count)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this._itemTemplate);
				gameObject.name = "Item " + num;
				gameObject.transform.SetParent(this._itemsPanelRT, false);
				this._panelItems.Add(new DropDownListButton(gameObject));
				num++;
			}
			for (int i = 0; i < this._panelItems.Count; i++)
			{
				if (i < this.Items.Count)
				{
					DropDownList.<RebuildPanel>c__AnonStorey0 <RebuildPanel>c__AnonStorey = new DropDownList.<RebuildPanel>c__AnonStorey0();
					<RebuildPanel>c__AnonStorey.$this = this;
					<RebuildPanel>c__AnonStorey.item = this.Items[i];
					this._panelItems[i].txt.text = <RebuildPanel>c__AnonStorey.item.Caption;
					if (<RebuildPanel>c__AnonStorey.item.IsDisabled)
					{
						this._panelItems[i].txt.color = this.disabledTextColor;
					}
					if (this._panelItems[i].btnImg != null)
					{
						this._panelItems[i].btnImg.sprite = null;
					}
					this._panelItems[i].img.sprite = <RebuildPanel>c__AnonStorey.item.Image;
					this._panelItems[i].img.color = ((!(<RebuildPanel>c__AnonStorey.item.Image == null)) ? ((!<RebuildPanel>c__AnonStorey.item.IsDisabled) ? Color.white : new Color(1f, 1f, 1f, 0.5f)) : new Color(1f, 1f, 1f, 0f));
					<RebuildPanel>c__AnonStorey.ii = i;
					this._panelItems[i].btn.onClick.RemoveAllListeners();
					this._panelItems[i].btn.onClick.AddListener(new UnityAction(<RebuildPanel>c__AnonStorey.<>m__0));
				}
				this._panelItems[i].gameobject.SetActive(i < this.Items.Count);
			}
		}

		// Token: 0x06001ED8 RID: 7896 RVA: 0x000AFD76 File Offset: 0x000AE176
		private void OnItemClicked(int indx)
		{
			if (indx != this._selectedIndex && this.OnSelectionChanged != null)
			{
				this.OnSelectionChanged.Invoke(indx);
			}
			this._selectedIndex = indx;
			this.ToggleDropdownPanel(true);
			this.UpdateSelected();
		}

		// Token: 0x06001ED9 RID: 7897 RVA: 0x000AFDB0 File Offset: 0x000AE1B0
		private void UpdateSelected()
		{
			this.SelectedItem = ((this._selectedIndex <= -1 || this._selectedIndex >= this.Items.Count) ? null : this.Items[this._selectedIndex]);
			if (this.SelectedItem == null)
			{
				return;
			}
			bool flag = this.SelectedItem.Image != null;
			if (flag)
			{
				this._mainButton.img.sprite = this.SelectedItem.Image;
				this._mainButton.img.color = Color.white;
			}
			else
			{
				this._mainButton.img.sprite = null;
			}
			this._mainButton.txt.text = this.SelectedItem.Caption;
			if (this.OverrideHighlighted)
			{
				for (int i = 0; i < this._itemsPanelRT.childCount; i++)
				{
					this._panelItems[i].btnImg.color = ((this._selectedIndex != i) ? new Color(0f, 0f, 0f, 0f) : this._mainButton.btn.colors.highlightedColor);
				}
			}
		}

		// Token: 0x06001EDA RID: 7898 RVA: 0x000AFF00 File Offset: 0x000AE300
		private void RedrawPanel()
		{
			float num = (this.Items.Count <= this.ItemsToDisplay) ? 0f : this._scrollBarWidth;
			if (!this._hasDrawnOnce || this._rectTransform.sizeDelta != this._mainButton.rectTransform.sizeDelta)
			{
				this._hasDrawnOnce = true;
				this._mainButton.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this._rectTransform.sizeDelta.x);
				this._mainButton.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this._rectTransform.sizeDelta.y);
				this._mainButton.txt.rectTransform.offsetMax = new Vector2(4f, 0f);
				this._scrollPanelRT.SetParent(base.transform, true);
				this._scrollPanelRT.anchoredPosition = new Vector2(0f, -this._rectTransform.sizeDelta.y);
				this._overlayRT.SetParent(this._canvas.transform, false);
				this._overlayRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this._canvasRT.sizeDelta.x);
				this._overlayRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this._canvasRT.sizeDelta.y);
				this._overlayRT.SetParent(base.transform, true);
				this._scrollPanelRT.SetParent(this._overlayRT, true);
			}
			if (this.Items.Count < 1)
			{
				return;
			}
			float num2 = this._rectTransform.sizeDelta.y * (float)Mathf.Min(this._itemsToDisplay, this.Items.Count);
			this._scrollPanelRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, num2);
			this._scrollPanelRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this._rectTransform.sizeDelta.x);
			this._itemsPanelRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this._scrollPanelRT.sizeDelta.x - num - 5f);
			this._itemsPanelRT.anchoredPosition = new Vector2(5f, 0f);
			this._scrollBarRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, num);
			this._scrollBarRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, num2);
			this._slidingAreaRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0f);
			this._slidingAreaRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, num2 - this._scrollBarRT.sizeDelta.x);
		}

		// Token: 0x06001EDB RID: 7899 RVA: 0x000B018C File Offset: 0x000AE58C
		public void ToggleDropdownPanel(bool directClick)
		{
			this._overlayRT.transform.localScale = new Vector3(1f, 1f, 1f);
			this._scrollBarRT.transform.localScale = new Vector3(1f, 1f, 1f);
			this._isPanelActive = !this._isPanelActive;
			this._overlayRT.gameObject.SetActive(this._isPanelActive);
			if (this._isPanelActive)
			{
				base.transform.SetAsLastSibling();
			}
			else if (directClick)
			{
			}
		}

		// Token: 0x040019FD RID: 6653
		public Color disabledTextColor;

		// Token: 0x040019FE RID: 6654
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private DropDownListItem <SelectedItem>k__BackingField;

		// Token: 0x040019FF RID: 6655
		public List<DropDownListItem> Items;

		// Token: 0x04001A00 RID: 6656
		public bool OverrideHighlighted = true;

		// Token: 0x04001A01 RID: 6657
		private bool _isPanelActive;

		// Token: 0x04001A02 RID: 6658
		private bool _hasDrawnOnce;

		// Token: 0x04001A03 RID: 6659
		private DropDownListButton _mainButton;

		// Token: 0x04001A04 RID: 6660
		private RectTransform _rectTransform;

		// Token: 0x04001A05 RID: 6661
		private RectTransform _overlayRT;

		// Token: 0x04001A06 RID: 6662
		private RectTransform _scrollPanelRT;

		// Token: 0x04001A07 RID: 6663
		private RectTransform _scrollBarRT;

		// Token: 0x04001A08 RID: 6664
		private RectTransform _slidingAreaRT;

		// Token: 0x04001A09 RID: 6665
		private RectTransform _itemsPanelRT;

		// Token: 0x04001A0A RID: 6666
		private Canvas _canvas;

		// Token: 0x04001A0B RID: 6667
		private RectTransform _canvasRT;

		// Token: 0x04001A0C RID: 6668
		private ScrollRect _scrollRect;

		// Token: 0x04001A0D RID: 6669
		private List<DropDownListButton> _panelItems;

		// Token: 0x04001A0E RID: 6670
		private GameObject _itemTemplate;

		// Token: 0x04001A0F RID: 6671
		[SerializeField]
		private float _scrollBarWidth = 20f;

		// Token: 0x04001A10 RID: 6672
		private int _selectedIndex = -1;

		// Token: 0x04001A11 RID: 6673
		[SerializeField]
		private int _itemsToDisplay;

		// Token: 0x04001A12 RID: 6674
		public bool SelectFirstItemOnStart;

		// Token: 0x04001A13 RID: 6675
		public DropDownList.SelectionChangedEvent OnSelectionChanged;

		// Token: 0x020004C6 RID: 1222
		[Serializable]
		public class SelectionChangedEvent : UnityEvent<int>
		{
			// Token: 0x06001EDC RID: 7900 RVA: 0x000B0227 File Offset: 0x000AE627
			public SelectionChangedEvent()
			{
			}
		}

		// Token: 0x02000F72 RID: 3954
		[CompilerGenerated]
		private sealed class <RebuildPanel>c__AnonStorey0
		{
			// Token: 0x060073E1 RID: 29665 RVA: 0x000B022F File Offset: 0x000AE62F
			public <RebuildPanel>c__AnonStorey0()
			{
			}

			// Token: 0x060073E2 RID: 29666 RVA: 0x000B0237 File Offset: 0x000AE637
			internal void <>m__0()
			{
				this.$this.OnItemClicked(this.ii);
				if (this.item.OnSelect != null)
				{
					this.item.OnSelect();
				}
			}

			// Token: 0x040067FC RID: 26620
			internal int ii;

			// Token: 0x040067FD RID: 26621
			internal DropDownListItem item;

			// Token: 0x040067FE RID: 26622
			internal DropDownList $this;
		}
	}
}
