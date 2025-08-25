using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004C3 RID: 1219
	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu("UI/Extensions/ComboBox")]
	public class ComboBox : MonoBehaviour
	{
		// Token: 0x06001EBD RID: 7869 RVA: 0x000AF0D5 File Offset: 0x000AD4D5
		public ComboBox()
		{
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x06001EBE RID: 7870 RVA: 0x000AF0E8 File Offset: 0x000AD4E8
		// (set) Token: 0x06001EBF RID: 7871 RVA: 0x000AF0F0 File Offset: 0x000AD4F0
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

		// Token: 0x1700036C RID: 876
		// (get) Token: 0x06001EC0 RID: 7872 RVA: 0x000AF0F9 File Offset: 0x000AD4F9
		// (set) Token: 0x06001EC1 RID: 7873 RVA: 0x000AF101 File Offset: 0x000AD501
		public string Text
		{
			[CompilerGenerated]
			get
			{
				return this.<Text>k__BackingField;
			}
			[CompilerGenerated]
			private set
			{
				this.<Text>k__BackingField = value;
			}
		}

		// Token: 0x1700036D RID: 877
		// (get) Token: 0x06001EC2 RID: 7874 RVA: 0x000AF10A File Offset: 0x000AD50A
		// (set) Token: 0x06001EC3 RID: 7875 RVA: 0x000AF112 File Offset: 0x000AD512
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

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x06001EC4 RID: 7876 RVA: 0x000AF121 File Offset: 0x000AD521
		// (set) Token: 0x06001EC5 RID: 7877 RVA: 0x000AF129 File Offset: 0x000AD529
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

		// Token: 0x06001EC6 RID: 7878 RVA: 0x000AF138 File Offset: 0x000AD538
		public void Awake()
		{
			this.Initialize();
		}

		// Token: 0x06001EC7 RID: 7879 RVA: 0x000AF144 File Offset: 0x000AD544
		private bool Initialize()
		{
			bool result = true;
			try
			{
				this._rectTransform = base.GetComponent<RectTransform>();
				this._inputRT = this._rectTransform.Find("InputField").GetComponent<RectTransform>();
				this._mainInput = this._inputRT.GetComponent<InputField>();
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
				this.itemTemplate = this._rectTransform.Find("ItemTemplate").gameObject;
				this.itemTemplate.SetActive(false);
			}
			catch (NullReferenceException exception)
			{
				Debug.LogException(exception);
				Debug.LogError("Something is setup incorrectly with the dropdownlist component causing a Null Refernece Exception");
				result = false;
			}
			this.panelObjects = new Dictionary<string, GameObject>();
			this._panelItems = this.AvailableOptions.ToList<string>();
			this.RebuildPanel();
			return result;
		}

		// Token: 0x06001EC8 RID: 7880 RVA: 0x000AF318 File Offset: 0x000AD718
		private void RebuildPanel()
		{
			this._panelItems.Clear();
			foreach (string text in this.AvailableOptions)
			{
				this._panelItems.Add(text.ToLower());
			}
			this._panelItems.Sort();
			List<GameObject> list = new List<GameObject>(this.panelObjects.Values);
			this.panelObjects.Clear();
			int num = 0;
			while (list.Count < this.AvailableOptions.Count)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.itemTemplate);
				gameObject.name = "Item " + num;
				gameObject.transform.SetParent(this._itemsPanelRT, false);
				list.Add(gameObject);
				num++;
			}
			for (int i = 0; i < list.Count; i++)
			{
				list[i].SetActive(i <= this.AvailableOptions.Count);
				if (i < this.AvailableOptions.Count)
				{
					ComboBox.<RebuildPanel>c__AnonStorey0 <RebuildPanel>c__AnonStorey = new ComboBox.<RebuildPanel>c__AnonStorey0();
					<RebuildPanel>c__AnonStorey.$this = this;
					list[i].name = string.Concat(new object[]
					{
						"Item ",
						i,
						" ",
						this._panelItems[i]
					});
					list[i].transform.Find("Text").GetComponent<Text>().text = this._panelItems[i];
					Button component = list[i].GetComponent<Button>();
					component.onClick.RemoveAllListeners();
					<RebuildPanel>c__AnonStorey.textOfItem = this._panelItems[i];
					component.onClick.AddListener(new UnityAction(<RebuildPanel>c__AnonStorey.<>m__0));
					this.panelObjects[this._panelItems[i]] = list[i];
				}
			}
		}

		// Token: 0x06001EC9 RID: 7881 RVA: 0x000AF544 File Offset: 0x000AD944
		private void OnItemClicked(string item)
		{
			this.Text = item;
			this._mainInput.text = this.Text;
			this.ToggleDropdownPanel(true);
		}

		// Token: 0x06001ECA RID: 7882 RVA: 0x000AF568 File Offset: 0x000AD968
		private void RedrawPanel()
		{
			float num = (this._panelItems.Count <= this.ItemsToDisplay) ? 0f : this._scrollBarWidth;
			this._scrollBarRT.gameObject.SetActive(this._panelItems.Count > this.ItemsToDisplay);
			if (!this._hasDrawnOnce || this._rectTransform.sizeDelta != this._inputRT.sizeDelta)
			{
				this._hasDrawnOnce = true;
				this._inputRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this._rectTransform.sizeDelta.x);
				this._inputRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this._rectTransform.sizeDelta.y);
				this._scrollPanelRT.SetParent(base.transform, true);
				this._scrollPanelRT.anchoredPosition = new Vector2(0f, -this._rectTransform.sizeDelta.y);
				this._overlayRT.SetParent(this._canvas.transform, false);
				this._overlayRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this._canvasRT.sizeDelta.x);
				this._overlayRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this._canvasRT.sizeDelta.y);
				this._overlayRT.SetParent(base.transform, true);
				this._scrollPanelRT.SetParent(this._overlayRT, true);
			}
			if (this._panelItems.Count < 1)
			{
				return;
			}
			float num2 = this._rectTransform.sizeDelta.y * (float)Mathf.Min(this._itemsToDisplay, this._panelItems.Count);
			this._scrollPanelRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, num2);
			this._scrollPanelRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this._rectTransform.sizeDelta.x);
			this._itemsPanelRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this._scrollPanelRT.sizeDelta.x - num - 5f);
			this._itemsPanelRT.anchoredPosition = new Vector2(5f, 0f);
			this._scrollBarRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, num);
			this._scrollBarRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, num2);
			this._slidingAreaRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0f);
			this._slidingAreaRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, num2 - this._scrollBarRT.sizeDelta.x);
		}

		// Token: 0x06001ECB RID: 7883 RVA: 0x000AF7E4 File Offset: 0x000ADBE4
		public void OnValueChanged(string currText)
		{
			this.Text = currText;
			this.RedrawPanel();
			if (this._panelItems.Count == 0)
			{
				this._isPanelActive = true;
				this.ToggleDropdownPanel(false);
			}
			else if (!this._isPanelActive)
			{
				this.ToggleDropdownPanel(false);
			}
			this.OnSelectionChanged.Invoke(this.Text);
		}

		// Token: 0x06001ECC RID: 7884 RVA: 0x000AF844 File Offset: 0x000ADC44
		public void ToggleDropdownPanel(bool directClick)
		{
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

		// Token: 0x040019E6 RID: 6630
		public Color disabledTextColor;

		// Token: 0x040019E7 RID: 6631
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private DropDownListItem <SelectedItem>k__BackingField;

		// Token: 0x040019E8 RID: 6632
		public List<string> AvailableOptions;

		// Token: 0x040019E9 RID: 6633
		[SerializeField]
		private float _scrollBarWidth = 20f;

		// Token: 0x040019EA RID: 6634
		[SerializeField]
		private int _itemsToDisplay;

		// Token: 0x040019EB RID: 6635
		public ComboBox.SelectionChangedEvent OnSelectionChanged;

		// Token: 0x040019EC RID: 6636
		private bool _isPanelActive;

		// Token: 0x040019ED RID: 6637
		private bool _hasDrawnOnce;

		// Token: 0x040019EE RID: 6638
		private InputField _mainInput;

		// Token: 0x040019EF RID: 6639
		private RectTransform _inputRT;

		// Token: 0x040019F0 RID: 6640
		private RectTransform _rectTransform;

		// Token: 0x040019F1 RID: 6641
		private RectTransform _overlayRT;

		// Token: 0x040019F2 RID: 6642
		private RectTransform _scrollPanelRT;

		// Token: 0x040019F3 RID: 6643
		private RectTransform _scrollBarRT;

		// Token: 0x040019F4 RID: 6644
		private RectTransform _slidingAreaRT;

		// Token: 0x040019F5 RID: 6645
		private RectTransform _itemsPanelRT;

		// Token: 0x040019F6 RID: 6646
		private Canvas _canvas;

		// Token: 0x040019F7 RID: 6647
		private RectTransform _canvasRT;

		// Token: 0x040019F8 RID: 6648
		private ScrollRect _scrollRect;

		// Token: 0x040019F9 RID: 6649
		private List<string> _panelItems;

		// Token: 0x040019FA RID: 6650
		private Dictionary<string, GameObject> panelObjects;

		// Token: 0x040019FB RID: 6651
		private GameObject itemTemplate;

		// Token: 0x040019FC RID: 6652
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <Text>k__BackingField;

		// Token: 0x020004C4 RID: 1220
		[Serializable]
		public class SelectionChangedEvent : UnityEvent<string>
		{
			// Token: 0x06001ECD RID: 7885 RVA: 0x000AF897 File Offset: 0x000ADC97
			public SelectionChangedEvent()
			{
			}
		}

		// Token: 0x02000F71 RID: 3953
		[CompilerGenerated]
		private sealed class <RebuildPanel>c__AnonStorey0
		{
			// Token: 0x060073DF RID: 29663 RVA: 0x000AF89F File Offset: 0x000ADC9F
			public <RebuildPanel>c__AnonStorey0()
			{
			}

			// Token: 0x060073E0 RID: 29664 RVA: 0x000AF8A7 File Offset: 0x000ADCA7
			internal void <>m__0()
			{
				this.$this.OnItemClicked(this.textOfItem);
			}

			// Token: 0x040067FA RID: 26618
			internal string textOfItem;

			// Token: 0x040067FB RID: 26619
			internal ComboBox $this;
		}
	}
}
