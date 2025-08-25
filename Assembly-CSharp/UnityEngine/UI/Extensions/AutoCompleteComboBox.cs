using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020004BF RID: 1215
	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu("UI/Extensions/AutoComplete ComboBox")]
	public class AutoCompleteComboBox : MonoBehaviour
	{
		// Token: 0x06001EA3 RID: 7843 RVA: 0x000AE52B File Offset: 0x000AC92B
		public AutoCompleteComboBox()
		{
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06001EA4 RID: 7844 RVA: 0x000AE566 File Offset: 0x000AC966
		// (set) Token: 0x06001EA5 RID: 7845 RVA: 0x000AE56E File Offset: 0x000AC96E
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

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06001EA6 RID: 7846 RVA: 0x000AE577 File Offset: 0x000AC977
		// (set) Token: 0x06001EA7 RID: 7847 RVA: 0x000AE57F File Offset: 0x000AC97F
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

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x06001EA8 RID: 7848 RVA: 0x000AE588 File Offset: 0x000AC988
		// (set) Token: 0x06001EA9 RID: 7849 RVA: 0x000AE590 File Offset: 0x000AC990
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

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x06001EAA RID: 7850 RVA: 0x000AE59F File Offset: 0x000AC99F
		// (set) Token: 0x06001EAB RID: 7851 RVA: 0x000AE5A7 File Offset: 0x000AC9A7
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

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x06001EAC RID: 7852 RVA: 0x000AE5B6 File Offset: 0x000AC9B6
		// (set) Token: 0x06001EAD RID: 7853 RVA: 0x000AE5BE File Offset: 0x000AC9BE
		public bool InputColorMatching
		{
			get
			{
				return this._ChangeInputTextColorBasedOnMatchingItems;
			}
			set
			{
				this._ChangeInputTextColorBasedOnMatchingItems = value;
				if (this._ChangeInputTextColorBasedOnMatchingItems)
				{
					this.SetInputTextColor();
				}
			}
		}

		// Token: 0x06001EAE RID: 7854 RVA: 0x000AE5D8 File Offset: 0x000AC9D8
		public void Awake()
		{
			this.Initialize();
		}

		// Token: 0x06001EAF RID: 7855 RVA: 0x000AE5E1 File Offset: 0x000AC9E1
		public void Start()
		{
			if (this.SelectFirstItemOnStart && this.AvailableOptions.Count > 0)
			{
				this.ToggleDropdownPanel(false);
				this.OnItemClicked(this.AvailableOptions[0]);
			}
		}

		// Token: 0x06001EB0 RID: 7856 RVA: 0x000AE618 File Offset: 0x000ACA18
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
			this._prunedPanelItems = new List<string>();
			this._panelItems = new List<string>();
			this.RebuildPanel();
			return result;
		}

		// Token: 0x06001EB1 RID: 7857 RVA: 0x000AE7F0 File Offset: 0x000ACBF0
		private void RebuildPanel()
		{
			this._panelItems.Clear();
			this._prunedPanelItems.Clear();
			this.panelObjects.Clear();
			foreach (string text in this.AvailableOptions)
			{
				this._panelItems.Add(text.ToLower());
			}
			List<GameObject> list = new List<GameObject>(this.panelObjects.Values);
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
					AutoCompleteComboBox.<RebuildPanel>c__AnonStorey0 <RebuildPanel>c__AnonStorey = new AutoCompleteComboBox.<RebuildPanel>c__AnonStorey0();
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
			this.SetInputTextColor();
		}

		// Token: 0x06001EB2 RID: 7858 RVA: 0x000AEA24 File Offset: 0x000ACE24
		private void OnItemClicked(string item)
		{
			this.Text = item;
			this._mainInput.text = this.Text;
			this.ToggleDropdownPanel(true);
		}

		// Token: 0x06001EB3 RID: 7859 RVA: 0x000AEA48 File Offset: 0x000ACE48
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

		// Token: 0x06001EB4 RID: 7860 RVA: 0x000AECC4 File Offset: 0x000AD0C4
		public void OnValueChanged(string currText)
		{
			this.Text = currText;
			this.PruneItems(currText);
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
			bool flag = this._panelItems.Contains(this.Text) != this._selectionIsValid;
			this._selectionIsValid = this._panelItems.Contains(this.Text);
			this.OnSelectionChanged.Invoke(this.Text, this._selectionIsValid);
			this.OnSelectionTextChanged.Invoke(this.Text);
			if (flag)
			{
				this.OnSelectionValidityChanged.Invoke(this._selectionIsValid);
			}
			this.SetInputTextColor();
		}

		// Token: 0x06001EB5 RID: 7861 RVA: 0x000AED94 File Offset: 0x000AD194
		private void SetInputTextColor()
		{
			if (this.InputColorMatching)
			{
				if (this._selectionIsValid)
				{
					this._mainInput.textComponent.color = this.ValidSelectionTextColor;
				}
				else if (this._panelItems.Count > 0)
				{
					this._mainInput.textComponent.color = this.MatchingItemsRemainingTextColor;
				}
				else
				{
					this._mainInput.textComponent.color = this.NoItemsRemainingTextColor;
				}
			}
		}

		// Token: 0x06001EB6 RID: 7862 RVA: 0x000AEE14 File Offset: 0x000AD214
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

		// Token: 0x06001EB7 RID: 7863 RVA: 0x000AEE67 File Offset: 0x000AD267
		private void PruneItems(string currText)
		{
			if (this.autocompleteSearchType == AutoCompleteSearchType.Linq)
			{
				this.PruneItemsLinq(currText);
			}
			else
			{
				this.PruneItemsArray(currText);
			}
		}

		// Token: 0x06001EB8 RID: 7864 RVA: 0x000AEE88 File Offset: 0x000AD288
		private void PruneItemsLinq(string currText)
		{
			AutoCompleteComboBox.<PruneItemsLinq>c__AnonStorey1 <PruneItemsLinq>c__AnonStorey = new AutoCompleteComboBox.<PruneItemsLinq>c__AnonStorey1();
			<PruneItemsLinq>c__AnonStorey.currText = currText;
			<PruneItemsLinq>c__AnonStorey.currText = <PruneItemsLinq>c__AnonStorey.currText.ToLower();
			string[] array = this._panelItems.Where(new Func<string, bool>(<PruneItemsLinq>c__AnonStorey.<>m__0)).ToArray<string>();
			foreach (string text in array)
			{
				this.panelObjects[text].SetActive(false);
				this._panelItems.Remove(text);
				this._prunedPanelItems.Add(text);
			}
			string[] array3 = this._prunedPanelItems.Where(new Func<string, bool>(<PruneItemsLinq>c__AnonStorey.<>m__1)).ToArray<string>();
			foreach (string text2 in array3)
			{
				this.panelObjects[text2].SetActive(true);
				this._panelItems.Add(text2);
				this._prunedPanelItems.Remove(text2);
			}
		}

		// Token: 0x06001EB9 RID: 7865 RVA: 0x000AEF8C File Offset: 0x000AD38C
		private void PruneItemsArray(string currText)
		{
			string value = currText.ToLower();
			for (int i = this._panelItems.Count - 1; i >= 0; i--)
			{
				string text = this._panelItems[i];
				if (!text.Contains(value))
				{
					this.panelObjects[this._panelItems[i]].SetActive(false);
					this._panelItems.RemoveAt(i);
					this._prunedPanelItems.Add(text);
				}
			}
			for (int j = this._prunedPanelItems.Count - 1; j >= 0; j--)
			{
				string text2 = this._prunedPanelItems[j];
				if (text2.Contains(value))
				{
					this.panelObjects[this._prunedPanelItems[j]].SetActive(true);
					this._prunedPanelItems.RemoveAt(j);
					this._panelItems.Add(text2);
				}
			}
		}

		// Token: 0x040019C5 RID: 6597
		public Color disabledTextColor;

		// Token: 0x040019C6 RID: 6598
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private DropDownListItem <SelectedItem>k__BackingField;

		// Token: 0x040019C7 RID: 6599
		public List<string> AvailableOptions;

		// Token: 0x040019C8 RID: 6600
		private bool _isPanelActive;

		// Token: 0x040019C9 RID: 6601
		private bool _hasDrawnOnce;

		// Token: 0x040019CA RID: 6602
		private InputField _mainInput;

		// Token: 0x040019CB RID: 6603
		private RectTransform _inputRT;

		// Token: 0x040019CC RID: 6604
		private RectTransform _rectTransform;

		// Token: 0x040019CD RID: 6605
		private RectTransform _overlayRT;

		// Token: 0x040019CE RID: 6606
		private RectTransform _scrollPanelRT;

		// Token: 0x040019CF RID: 6607
		private RectTransform _scrollBarRT;

		// Token: 0x040019D0 RID: 6608
		private RectTransform _slidingAreaRT;

		// Token: 0x040019D1 RID: 6609
		private RectTransform _itemsPanelRT;

		// Token: 0x040019D2 RID: 6610
		private Canvas _canvas;

		// Token: 0x040019D3 RID: 6611
		private RectTransform _canvasRT;

		// Token: 0x040019D4 RID: 6612
		private ScrollRect _scrollRect;

		// Token: 0x040019D5 RID: 6613
		private List<string> _panelItems;

		// Token: 0x040019D6 RID: 6614
		private List<string> _prunedPanelItems;

		// Token: 0x040019D7 RID: 6615
		private Dictionary<string, GameObject> panelObjects;

		// Token: 0x040019D8 RID: 6616
		private GameObject itemTemplate;

		// Token: 0x040019D9 RID: 6617
		[CompilerGenerated]
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		private string <Text>k__BackingField;

		// Token: 0x040019DA RID: 6618
		[SerializeField]
		private float _scrollBarWidth = 20f;

		// Token: 0x040019DB RID: 6619
		[SerializeField]
		private int _itemsToDisplay;

		// Token: 0x040019DC RID: 6620
		public bool SelectFirstItemOnStart;

		// Token: 0x040019DD RID: 6621
		[SerializeField]
		[Tooltip("Change input text color based on matching items")]
		private bool _ChangeInputTextColorBasedOnMatchingItems;

		// Token: 0x040019DE RID: 6622
		public Color ValidSelectionTextColor = Color.green;

		// Token: 0x040019DF RID: 6623
		public Color MatchingItemsRemainingTextColor = Color.black;

		// Token: 0x040019E0 RID: 6624
		public Color NoItemsRemainingTextColor = Color.red;

		// Token: 0x040019E1 RID: 6625
		public AutoCompleteSearchType autocompleteSearchType = AutoCompleteSearchType.Linq;

		// Token: 0x040019E2 RID: 6626
		private bool _selectionIsValid;

		// Token: 0x040019E3 RID: 6627
		public AutoCompleteComboBox.SelectionTextChangedEvent OnSelectionTextChanged;

		// Token: 0x040019E4 RID: 6628
		public AutoCompleteComboBox.SelectionValidityChangedEvent OnSelectionValidityChanged;

		// Token: 0x040019E5 RID: 6629
		public AutoCompleteComboBox.SelectionChangedEvent OnSelectionChanged;

		// Token: 0x020004C0 RID: 1216
		[Serializable]
		public class SelectionChangedEvent : UnityEvent<string, bool>
		{
			// Token: 0x06001EBA RID: 7866 RVA: 0x000AF07B File Offset: 0x000AD47B
			public SelectionChangedEvent()
			{
			}
		}

		// Token: 0x020004C1 RID: 1217
		[Serializable]
		public class SelectionTextChangedEvent : UnityEvent<string>
		{
			// Token: 0x06001EBB RID: 7867 RVA: 0x000AF083 File Offset: 0x000AD483
			public SelectionTextChangedEvent()
			{
			}
		}

		// Token: 0x020004C2 RID: 1218
		[Serializable]
		public class SelectionValidityChangedEvent : UnityEvent<bool>
		{
			// Token: 0x06001EBC RID: 7868 RVA: 0x000AF08B File Offset: 0x000AD48B
			public SelectionValidityChangedEvent()
			{
			}
		}

		// Token: 0x02000F6F RID: 3951
		[CompilerGenerated]
		private sealed class <RebuildPanel>c__AnonStorey0
		{
			// Token: 0x060073DA RID: 29658 RVA: 0x000AF093 File Offset: 0x000AD493
			public <RebuildPanel>c__AnonStorey0()
			{
			}

			// Token: 0x060073DB RID: 29659 RVA: 0x000AF09B File Offset: 0x000AD49B
			internal void <>m__0()
			{
				this.$this.OnItemClicked(this.textOfItem);
			}

			// Token: 0x040067F7 RID: 26615
			internal string textOfItem;

			// Token: 0x040067F8 RID: 26616
			internal AutoCompleteComboBox $this;
		}

		// Token: 0x02000F70 RID: 3952
		[CompilerGenerated]
		private sealed class <PruneItemsLinq>c__AnonStorey1
		{
			// Token: 0x060073DC RID: 29660 RVA: 0x000AF0AE File Offset: 0x000AD4AE
			public <PruneItemsLinq>c__AnonStorey1()
			{
			}

			// Token: 0x060073DD RID: 29661 RVA: 0x000AF0B6 File Offset: 0x000AD4B6
			internal bool <>m__0(string x)
			{
				return !x.Contains(this.currText);
			}

			// Token: 0x060073DE RID: 29662 RVA: 0x000AF0C7 File Offset: 0x000AD4C7
			internal bool <>m__1(string x)
			{
				return x.Contains(this.currText);
			}

			// Token: 0x040067F9 RID: 26617
			internal string currText;
		}
	}
}
