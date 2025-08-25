using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02000DF6 RID: 3574
public class UIPopup : MonoBehaviour, ISelectHandler, IDeselectHandler, IEventSystemHandler
{
	// Token: 0x06006E79 RID: 28281 RVA: 0x00297304 File Offset: 0x00295704
	public UIPopup()
	{
	}

	// Token: 0x1700102D RID: 4141
	// (get) Token: 0x06006E7A RID: 28282 RVA: 0x0029739A File Offset: 0x0029579A
	// (set) Token: 0x06006E7B RID: 28283 RVA: 0x002973A4 File Offset: 0x002957A4
	public float popupPanelHeight
	{
		get
		{
			return this._popupPanelHeight;
		}
		set
		{
			if (this._popupPanelHeight != value)
			{
				this._popupPanelHeight = value;
				if (this.popupPanel != null)
				{
					bool visible = this._visible;
					if (visible)
					{
						this.MakeInvisible();
					}
					Vector2 sizeDelta = this.popupPanel.sizeDelta;
					sizeDelta.y = this._popupPanelHeight;
					this.popupPanel.sizeDelta = sizeDelta;
					if (visible)
					{
						this.MakeVisible();
					}
				}
			}
		}
	}

	// Token: 0x1700102E RID: 4142
	// (get) Token: 0x06006E7C RID: 28284 RVA: 0x00297419 File Offset: 0x00295819
	// (set) Token: 0x06006E7D RID: 28285 RVA: 0x00297424 File Offset: 0x00295824
	public bool showSlider
	{
		get
		{
			return this._showSlider;
		}
		set
		{
			if (this._showSlider != value)
			{
				this._showSlider = value;
				if (this.topButton != null)
				{
					RectTransform component = this.topButton.GetComponent<RectTransform>();
					if (component != null)
					{
						Vector2 offsetMax = component.offsetMax;
						if (this._showSlider && this.sliderControl != null)
						{
							RectTransform component2 = this.sliderControl.GetComponent<RectTransform>();
							if (component2 != null)
							{
								offsetMax.y = -5f - component2.sizeDelta.y;
							}
							else
							{
								offsetMax.y = -5f;
							}
						}
						else
						{
							offsetMax.y = -5f;
						}
						component.offsetMax = offsetMax;
					}
				}
				if (this.sliderControl != null)
				{
					if (this._showSlider)
					{
						this.sliderControl.gameObject.SetActive(true);
					}
					else
					{
						this.sliderControl.gameObject.SetActive(false);
					}
				}
			}
		}
	}

	// Token: 0x06006E7E RID: 28286 RVA: 0x00297530 File Offset: 0x00295930
	protected void SyncFilter()
	{
		if (this.useFiltering)
		{
			if (this.valueIndexToButtonIndex == null)
			{
				this.valueIndexToButtonIndex = new Dictionary<int, int>();
			}
			else
			{
				this.valueIndexToButtonIndex.Clear();
			}
			if (this.buttonIndexToValueIndex == null)
			{
				this.buttonIndexToValueIndex = new Dictionary<int, int>();
			}
			else
			{
				this.buttonIndexToValueIndex.Clear();
			}
			int num = 0;
			bool flag = false;
			for (int i = 0; i < this._numPopupValues; i++)
			{
				if (this._filter == null || this._filter == string.Empty || this._filterCompareValues[i].Contains(this._filter))
				{
					this.valueIndexToButtonIndex.Add(i, num);
					this.buttonIndexToValueIndex.Add(num, i);
					num++;
					if (num == this.maxNumber)
					{
						flag = true;
						break;
					}
				}
			}
			if (this.filterCountText != null)
			{
				this.filterCountText.text = num.ToString() + " / " + this._numPopupValues;
			}
			if (this.maxIndicator != null)
			{
				this.maxIndicator.gameObject.SetActive(flag);
			}
			if (this.filterFieldPlaceholderText != null)
			{
				if (flag)
				{
					this.filterFieldPlaceholderText.text = "Filter (max limited)";
				}
				else
				{
					this.filterFieldPlaceholderText.text = "Filter...";
				}
			}
			this._filteredNumPopupValues = num;
		}
		else
		{
			this._filteredNumPopupValues = this._numPopupValues;
		}
	}

	// Token: 0x06006E7F RID: 28287 RVA: 0x002976CC File Offset: 0x00295ACC
	protected void SelectFilter()
	{
		this.visible = true;
	}

	// Token: 0x06006E80 RID: 28288 RVA: 0x002976D8 File Offset: 0x00295AD8
	public void SubmitFilter()
	{
		if (this.useFiltering)
		{
			if (this._filteredNumPopupValues > 0)
			{
				int num2;
				if (this._currentHighlightIndex != -1)
				{
					int num;
					if (this.buttonIndexToValueIndex.TryGetValue(this._currentHighlightIndex, out num))
					{
						this.currentValue = this._popupValues[num];
					}
				}
				else if (this._currentButtonIndex == -1 && this.buttonIndexToValueIndex.TryGetValue(0, out num2))
				{
					this.currentValue = this._popupValues[num2];
				}
			}
			this.visible = false;
		}
	}

	// Token: 0x06006E81 RID: 28289 RVA: 0x00297767 File Offset: 0x00295B67
	protected void SyncFilter(string f)
	{
		this.filter = f;
	}

	// Token: 0x1700102F RID: 4143
	// (get) Token: 0x06006E82 RID: 28290 RVA: 0x00297770 File Offset: 0x00295B70
	// (set) Token: 0x06006E83 RID: 28291 RVA: 0x00297778 File Offset: 0x00295B78
	public string filter
	{
		get
		{
			return this._filter;
		}
		set
		{
			string text = value.ToLower();
			if (this._filter != text)
			{
				this._filter = text;
				if (this.filterField != null)
				{
					this.filterField.text = this._filter;
				}
				this.visible = true;
				this.SyncFilter();
				this.ClearPanel();
				this.CreatePanelButtons();
			}
		}
	}

	// Token: 0x17001030 RID: 4144
	// (get) Token: 0x06006E84 RID: 28292 RVA: 0x002977DF File Offset: 0x00295BDF
	// (set) Token: 0x06006E85 RID: 28293 RVA: 0x002977E8 File Offset: 0x00295BE8
	public int numPopupValues
	{
		get
		{
			return this._numPopupValues;
		}
		set
		{
			if (this._numPopupValues != value && base.gameObject != null)
			{
				this.ClearPanel();
				int numPopupValues = this._numPopupValues;
				this._numPopupValues = value;
				string[] array = new string[this._numPopupValues];
				string[] array2 = new string[this._numPopupValues];
				string[] array3 = null;
				if (this.useFiltering)
				{
					array3 = new string[this._numPopupValues];
				}
				for (int i = 0; i < this._numPopupValues; i++)
				{
					if (i < numPopupValues)
					{
						if (this._displayPopupValues != null && i < this._displayPopupValues.Length)
						{
							if (this._displayPopupValues[i] == null)
							{
								array2[i] = string.Empty;
							}
							else
							{
								array2[i] = this._displayPopupValues[i];
							}
							if (this.useFiltering && this.useDifferentDisplayValues)
							{
								array3[i] = array2[i].ToLower();
							}
						}
						if (this._popupValues[i] == null)
						{
							array[i] = string.Empty;
						}
						else
						{
							array[i] = this._popupValues[i];
						}
						if (this.useFiltering && !this.useDifferentDisplayValues)
						{
							array3[i] = array[i].ToLower();
						}
					}
					else
					{
						array[i] = string.Empty;
						array2[i] = string.Empty;
						if (this.useFiltering)
						{
							array3[i] = string.Empty;
						}
					}
				}
				this._popupValues = array;
				this._displayPopupValues = array2;
				this._filterCompareValues = array3;
				this.SyncFilter();
				this.CreatePanelButtons();
				this.SyncSlider();
			}
		}
	}

	// Token: 0x17001031 RID: 4145
	// (get) Token: 0x06006E86 RID: 28294 RVA: 0x0029797E File Offset: 0x00295D7E
	public string[] popupValues
	{
		get
		{
			return this._popupValues;
		}
	}

	// Token: 0x17001032 RID: 4146
	// (get) Token: 0x06006E87 RID: 28295 RVA: 0x00297986 File Offset: 0x00295D86
	public string[] displayPopupValues
	{
		get
		{
			return this._displayPopupValues;
		}
	}

	// Token: 0x06006E88 RID: 28296 RVA: 0x00297990 File Offset: 0x00295D90
	public void setPopupValue(int index, string text)
	{
		if (index >= 0 && index < this._numPopupValues)
		{
			this._popupValues[index] = text;
			if (this.useFiltering && !this.useDifferentDisplayValues)
			{
				this._filterCompareValues[index] = text.ToLower();
			}
			this.SetPanelButtonText(index);
			this._currentValueChanged = true;
		}
	}

	// Token: 0x06006E89 RID: 28297 RVA: 0x002979EC File Offset: 0x00295DEC
	public void setDisplayPopupValue(int index, string text)
	{
		if (index >= 0 && index < this._numPopupValues)
		{
			this._displayPopupValues[index] = text;
			if (this.useFiltering && this.useDifferentDisplayValues)
			{
				this._filterCompareValues[index] = text.ToLower();
			}
			this.SetPanelButtonText(index);
			this._currentValueChanged = true;
		}
	}

	// Token: 0x06006E8A RID: 28298 RVA: 0x00297A48 File Offset: 0x00295E48
	protected void HighlightCurrentValue()
	{
		if (this._popupButtons != null)
		{
			int num = -1;
			for (int i = 0; i < this._popupButtons.Length; i++)
			{
				if (this._popupButtons[i] != null)
				{
					int num2 = i;
					if (this.useFiltering)
					{
						this.buttonIndexToValueIndex.TryGetValue(i, out num2);
					}
					ColorBlock colors = this._popupButtons[i].colors;
					if (this._popupValues[num2] == this.currentValue)
					{
						num = i;
						colors.normalColor = this.selectColor;
					}
					else
					{
						colors.normalColor = this.normalColor;
					}
					this._popupButtons[i].colors = colors;
				}
			}
			if (num != -1 && this.scrollRect != null && this.scrollRect.vertical && !this.scrollRect.horizontal)
			{
				RectTransform component = this._popupButtons[num].GetComponent<RectTransform>();
				float height = component.rect.height;
				float num3 = (this.scrollRect.viewport.rect.height - height) / 2f;
				float num4 = (float)num * height - num3;
				float num5 = Mathf.Clamp01(num4 / (this.scrollRect.content.rect.height - this.scrollRect.viewport.rect.height));
				this.scrollRect.verticalNormalizedPosition = 1f - num5;
			}
		}
	}

	// Token: 0x06006E8B RID: 28299 RVA: 0x00297BD8 File Offset: 0x00295FD8
	private void MakeInvisible()
	{
		this.ClearHighlightButton();
		if (this.popupPanel != null)
		{
			this.popupPanel.gameObject.SetActive(false);
			if (base.transform.parent != null)
			{
				this.popupPanel.transform.SetParent(base.transform);
			}
		}
	}

	// Token: 0x06006E8C RID: 28300 RVA: 0x00297C3C File Offset: 0x0029603C
	private void MakeVisible()
	{
		if (this.popupPanel != null)
		{
			this.popupPanel.gameObject.SetActive(true);
			if (base.transform.parent != null)
			{
				this.popupPanel.transform.SetParent(base.transform.parent);
			}
		}
	}

	// Token: 0x17001033 RID: 4147
	// (get) Token: 0x06006E8D RID: 28301 RVA: 0x00297C9C File Offset: 0x0029609C
	// (set) Token: 0x06006E8E RID: 28302 RVA: 0x00297CA4 File Offset: 0x002960A4
	public bool visible
	{
		get
		{
			return this._visible;
		}
		set
		{
			if (this._visible != value)
			{
				this._visible = value;
				if (this.alwaysOpen)
				{
					this._visible = true;
				}
				if (this._visible)
				{
					if (this.onOpenPopupHandlers != null)
					{
						this.onOpenPopupHandlers();
						if (this.useFiltering)
						{
							this.SyncFilter();
							this.ClearPanel();
							this.CreatePanelButtons();
						}
					}
					else
					{
						this.HighlightCurrentValue();
					}
					this.MakeVisible();
				}
				else
				{
					this.MakeInvisible();
				}
			}
		}
	}

	// Token: 0x06006E8F RID: 28303 RVA: 0x00297D30 File Offset: 0x00296130
	protected void SetCurrentValue(string value, bool callBack = true, bool forceSet = false, bool leaveOpen = false)
	{
		if (this._currentValue != value || forceSet)
		{
			this._currentValue = value;
			if (this.popupPanel != null && !this.alwaysOpen && !leaveOpen)
			{
				this.visible = false;
			}
			if (callBack && this.onValueChangeHandlers != null)
			{
				this.onValueChangeHandlers(this._currentValue);
			}
			if (callBack && this.onValueChangeUnityEvent != null)
			{
				this.onValueChangeUnityEvent.Invoke(this._currentValue);
			}
		}
		this._currentValueChanged = true;
		this._currentValueIndex = -1;
		this._currentButtonIndex = -1;
		for (int i = 0; i < this._popupValues.Length; i++)
		{
			if (this._currentValue == this._popupValues[i])
			{
				this._currentValueIndex = i;
				if (this.useFiltering)
				{
					int num;
					if (this.valueIndexToButtonIndex.TryGetValue(i, out num))
					{
						this._currentButtonIndex = num;
						if (this.sliderControl != null)
						{
							this.sliderControl.value = (float)num;
						}
					}
				}
				else
				{
					this._currentButtonIndex = i;
					if (this.sliderControl != null)
					{
						this.sliderControl.value = (float)this._currentValueIndex;
					}
				}
				break;
			}
		}
		if (this.topButton != null)
		{
			Text[] componentsInChildren = this.topButton.GetComponentsInChildren<Text>(true);
			if (componentsInChildren != null)
			{
				if (this.dynamicTextSize)
				{
					if (!componentsInChildren[0].resizeTextForBestFit)
					{
						componentsInChildren[0].resizeTextMaxSize = componentsInChildren[0].fontSize;
						componentsInChildren[0].resizeTextMinSize = this.minTextSize;
						componentsInChildren[0].resizeTextForBestFit = this.dynamicTextSize;
					}
				}
				else
				{
					componentsInChildren[0].resizeTextForBestFit = this.dynamicTextSize;
				}
				if (this.useDifferentDisplayValues && this._currentValueIndex != -1)
				{
					componentsInChildren[0].text = this._displayPopupValues[this._currentValueIndex];
				}
				else
				{
					componentsInChildren[0].text = this._currentValue;
				}
			}
		}
	}

	// Token: 0x17001034 RID: 4148
	// (get) Token: 0x06006E90 RID: 28304 RVA: 0x00297F47 File Offset: 0x00296347
	// (set) Token: 0x06006E91 RID: 28305 RVA: 0x00297F4F File Offset: 0x0029634F
	public string currentValue
	{
		get
		{
			return this._currentValue;
		}
		set
		{
			if (this._currentValue != value || this._currentValueIndex == -1)
			{
				this.SetCurrentValue(value, true, false, false);
			}
		}
	}

	// Token: 0x17001035 RID: 4149
	// (get) Token: 0x06006E92 RID: 28306 RVA: 0x00297F78 File Offset: 0x00296378
	// (set) Token: 0x06006E93 RID: 28307 RVA: 0x00297F80 File Offset: 0x00296380
	public string currentValueNoCallback
	{
		get
		{
			return this._currentValue;
		}
		set
		{
			if (this._currentValue != value || this._currentValueIndex == -1)
			{
				this.SetCurrentValue(value, false, false, false);
			}
		}
	}

	// Token: 0x17001036 RID: 4150
	// (get) Token: 0x06006E94 RID: 28308 RVA: 0x00297FA9 File Offset: 0x002963A9
	// (set) Token: 0x06006E95 RID: 28309 RVA: 0x00297FC9 File Offset: 0x002963C9
	public string label
	{
		get
		{
			if (this.labelText != null)
			{
				return this.labelText.text;
			}
			return null;
		}
		set
		{
			if (this.labelText != null)
			{
				this.labelText.text = value;
			}
		}
	}

	// Token: 0x17001037 RID: 4151
	// (get) Token: 0x06006E96 RID: 28310 RVA: 0x00297FE8 File Offset: 0x002963E8
	// (set) Token: 0x06006E97 RID: 28311 RVA: 0x0029800C File Offset: 0x0029640C
	public Color labelTextColor
	{
		get
		{
			if (this.labelText != null)
			{
				return this.labelText.color;
			}
			return Color.black;
		}
		set
		{
			if (this.labelText != null)
			{
				this.labelText.color = value;
			}
		}
	}

	// Token: 0x06006E98 RID: 28312 RVA: 0x0029802C File Offset: 0x0029642C
	public void OnSelect(BaseEventData eventData)
	{
		if (this.backgroundImage != null)
		{
			this.backgroundImage.color = this.selectColor;
		}
		else if (this.topButton != null)
		{
			ColorBlock colors = this.topButton.colors;
			colors.normalColor = this.selectColor;
			this.topButton.colors = colors;
		}
	}

	// Token: 0x06006E99 RID: 28313 RVA: 0x00298098 File Offset: 0x00296498
	public void OnDeselect(BaseEventData eventData)
	{
		if (this.backgroundImage != null)
		{
			this.backgroundImage.color = this.normalBackgroundColor;
		}
		else if (this.topButton != null)
		{
			ColorBlock colors = this.topButton.colors;
			colors.normalColor = this.normalColor;
			this.topButton.colors = colors;
		}
	}

	// Token: 0x06006E9A RID: 28314 RVA: 0x00298104 File Offset: 0x00296504
	public void SetPreviousValue()
	{
		if (this._popupValues != null && this._currentButtonIndex > 0)
		{
			if (this.useFiltering)
			{
				int num;
				if (this.buttonIndexToValueIndex.TryGetValue(this._currentButtonIndex - 1, out num))
				{
					this.SetCurrentValue(this._popupValues[num], true, false, true);
				}
			}
			else
			{
				this.SetCurrentValue(this._popupValues[this._currentButtonIndex - 1], true, false, true);
			}
		}
	}

	// Token: 0x06006E9B RID: 28315 RVA: 0x0029817C File Offset: 0x0029657C
	public void SetNextValue()
	{
		if (this._popupValues != null && this._currentButtonIndex < this._filteredNumPopupValues - 1)
		{
			if (this.useFiltering)
			{
				int num;
				if (this.buttonIndexToValueIndex.TryGetValue(this._currentButtonIndex + 1, out num))
				{
					this.SetCurrentValue(this._popupValues[num], true, false, true);
				}
			}
			else
			{
				this.SetCurrentValue(this._popupValues[this._currentButtonIndex + 1], true, false, true);
			}
		}
	}

	// Token: 0x06006E9C RID: 28316 RVA: 0x002981FC File Offset: 0x002965FC
	protected void ClearHighlightButton()
	{
		if (this._popupButtons != null && this._currentHighlightIndex != -1 && this._currentHighlightIndex < this._popupButtons.Length)
		{
			this._popupButtons[this._currentHighlightIndex].OnPointerExit(null);
		}
		this._currentHighlightIndex = -1;
	}

	// Token: 0x06006E9D RID: 28317 RVA: 0x0029824D File Offset: 0x0029664D
	protected void HighlightCurrentButton()
	{
		if (this._popupButtons != null && this._currentHighlightIndex != -1 && this._currentHighlightIndex < this._popupButtons.Length)
		{
			this._popupButtons[this._currentHighlightIndex].OnPointerEnter(null);
		}
	}

	// Token: 0x06006E9E RID: 28318 RVA: 0x0029828C File Offset: 0x0029668C
	protected void HighlightPreviousButton()
	{
		if (this._currentHighlightIndex >= 0)
		{
			int currentHighlightIndex = this._currentHighlightIndex;
			this.ClearHighlightButton();
			this._currentHighlightIndex = currentHighlightIndex - 1;
			this.HighlightCurrentButton();
		}
		this.filterField.caretPosition = this.filterField.text.Length;
	}

	// Token: 0x06006E9F RID: 28319 RVA: 0x002982DC File Offset: 0x002966DC
	protected void HighlightNextButton()
	{
		if (this._popupButtons != null && this._currentHighlightIndex < this._popupButtons.Length - 1)
		{
			int currentHighlightIndex = this._currentHighlightIndex;
			this.ClearHighlightButton();
			this._currentHighlightIndex = currentHighlightIndex + 1;
			this.HighlightCurrentButton();
		}
	}

	// Token: 0x06006EA0 RID: 28320 RVA: 0x00298325 File Offset: 0x00296725
	public void Toggle()
	{
		this.visible = !this.visible;
	}

	// Token: 0x06006EA1 RID: 28321 RVA: 0x00298338 File Offset: 0x00296738
	private void ClearPanel()
	{
		this.ClearHighlightButton();
		RectTransform rectTransform;
		if (this.buttonParent != null)
		{
			rectTransform = this.buttonParent;
		}
		else
		{
			rectTransform = this.popupPanel;
		}
		if (rectTransform != null)
		{
			List<GameObject> list = new List<GameObject>();
			IEnumerator enumerator = rectTransform.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					RectTransform rectTransform2 = (RectTransform)obj;
					if (!this.popupButtonPrefab || !(this.popupButtonPrefab.gameObject == rectTransform2.gameObject))
					{
						list.Add(rectTransform2.gameObject);
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
			foreach (GameObject obj2 in list)
			{
				if (Application.isPlaying)
				{
					UnityEngine.Object.Destroy(obj2);
				}
				else
				{
					UnityEngine.Object.DestroyImmediate(obj2);
				}
			}
		}
		this._popupTransforms = null;
		this._popupButtons = null;
	}

	// Token: 0x06006EA2 RID: 28322 RVA: 0x00298478 File Offset: 0x00296878
	private void SetPanelButtonText(int index)
	{
		string text = this._popupValues[index];
		int num = index;
		bool flag = true;
		if (this.useFiltering && !this.valueIndexToButtonIndex.TryGetValue(index, out num))
		{
			flag = false;
		}
		if (flag)
		{
			RectTransform rectTransform = this._popupTransforms[num];
			if (rectTransform != null)
			{
				Button component = rectTransform.GetComponent<Button>();
				if (component != null)
				{
					UIPopup.<SetPanelButtonText>c__AnonStorey0 <SetPanelButtonText>c__AnonStorey = new UIPopup.<SetPanelButtonText>c__AnonStorey0();
					<SetPanelButtonText>c__AnonStorey.$this = this;
					this._popupButtons[num] = component;
					<SetPanelButtonText>c__AnonStorey.popupValueCopy = string.Copy(text);
					component.onClick.RemoveAllListeners();
					component.onClick.AddListener(new UnityAction(<SetPanelButtonText>c__AnonStorey.<>m__0));
				}
				Text[] componentsInChildren = component.GetComponentsInChildren<Text>(true);
				if (componentsInChildren != null)
				{
					if (this.dynamicTextSize)
					{
						if (!componentsInChildren[0].resizeTextForBestFit)
						{
							componentsInChildren[0].resizeTextMaxSize = componentsInChildren[0].fontSize;
							componentsInChildren[0].resizeTextMinSize = this.minTextSize;
							componentsInChildren[0].resizeTextForBestFit = this.dynamicTextSize;
						}
					}
					else
					{
						componentsInChildren[0].resizeTextForBestFit = this.dynamicTextSize;
					}
					if (this.useDifferentDisplayValues)
					{
						componentsInChildren[0].text = this._displayPopupValues[index];
					}
					else
					{
						componentsInChildren[0].text = text;
					}
				}
			}
		}
	}

	// Token: 0x06006EA3 RID: 28323 RVA: 0x002985C8 File Offset: 0x002969C8
	private void CreatePanelButtons()
	{
		if (this._popupValues != null && this.popupButtonPrefab != null && base.gameObject != null)
		{
			this._popupTransforms = new RectTransform[this._filteredNumPopupValues];
			this._popupButtons = new Button[this._filteredNumPopupValues];
			int num = 1;
			float num2 = 2f * this.topBottomBuffer;
			float num3 = num2;
			this._currentButtonIndex = -1;
			for (int i = 0; i < this._filteredNumPopupValues; i++)
			{
				int num4 = i;
				if (!this.useFiltering || this.buttonIndexToValueIndex.TryGetValue(i, out num4))
				{
					if (this._currentValue == this._popupValues[num4])
					{
						this._currentButtonIndex = i;
					}
					RectTransform rectTransform = UnityEngine.Object.Instantiate<RectTransform>(this.popupButtonPrefab);
					this._popupTransforms[i] = rectTransform;
					rectTransform.gameObject.SetActive(true);
					this.SetPanelButtonText(num4);
					float height = rectTransform.rect.height;
					num2 += height;
					num3 += height;
					if (this.buttonParent != null)
					{
						rectTransform.SetParent(this.buttonParent, false);
					}
					else
					{
						rectTransform.SetParent(this.popupPanel, false);
					}
					Vector2 zero = Vector2.zero;
					zero.y = (float)num * -height + height * 0.5f - this.topBottomBuffer;
					rectTransform.anchoredPosition = zero;
					num++;
				}
			}
			if (this.topButton != null)
			{
				RectTransform component = this.topButton.GetComponent<RectTransform>();
				if (component != null)
				{
					num2 += component.rect.height;
				}
			}
			if (this.buttonParent == null)
			{
				RectTransform component2 = base.GetComponent<RectTransform>();
				if (component2 != null)
				{
					Vector2 sizeDelta = component2.sizeDelta;
					sizeDelta.y = num2;
					component2.sizeDelta = sizeDelta;
				}
			}
			else
			{
				Vector2 sizeDelta2 = this.buttonParent.sizeDelta;
				sizeDelta2.y = num3;
				this.buttonParent.sizeDelta = sizeDelta2;
			}
			this.HighlightCurrentValue();
		}
	}

	// Token: 0x06006EA4 RID: 28324 RVA: 0x002987F0 File Offset: 0x00296BF0
	private void SyncSlider()
	{
		if (this.sliderControl != null)
		{
			this.sliderControl.minValue = 0f;
			this.sliderControl.maxValue = Mathf.Max((float)(this._filteredNumPopupValues - 1), 0f);
		}
	}

	// Token: 0x06006EA5 RID: 28325 RVA: 0x0029883C File Offset: 0x00296C3C
	private void InitSlider()
	{
		if (this.sliderControl != null)
		{
			this.sliderControl.minValue = 0f;
			this.sliderControl.maxValue = Mathf.Max((float)(this._filteredNumPopupValues - 1), 0f);
			this.sliderControl.onValueChanged.AddListener(new UnityAction<float>(this.<InitSlider>m__0));
		}
	}

	// Token: 0x06006EA6 RID: 28326 RVA: 0x002988A4 File Offset: 0x00296CA4
	private void TestDelegate(string test)
	{
		Debug.Log("TestDelegate on " + base.name + " called with " + test);
	}

	// Token: 0x06006EA7 RID: 28327 RVA: 0x002988C4 File Offset: 0x00296CC4
	private void Start()
	{
		this.scrollRect = base.GetComponentInChildren<ScrollRect>(true);
		if (this.popupPanel != null)
		{
			this.popupPanelRelativePosition = this.popupPanel.localPosition;
		}
		if (this.filterFieldAction != null)
		{
			InputFieldAction inputFieldAction = this.filterFieldAction;
			inputFieldAction.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Combine(inputFieldAction.onSubmitHandlers, new InputFieldAction.OnSubmit(this.SubmitFilter));
			InputFieldAction inputFieldAction2 = this.filterFieldAction;
			inputFieldAction2.onSelectedHandlers = (InputFieldAction.OnSelected)Delegate.Combine(inputFieldAction2.onSelectedHandlers, new InputFieldAction.OnSelected(this.SelectFilter));
			InputFieldAction inputFieldAction3 = this.filterFieldAction;
			inputFieldAction3.onUpHandlers = (InputFieldAction.OnUp)Delegate.Combine(inputFieldAction3.onUpHandlers, new InputFieldAction.OnUp(this.HighlightPreviousButton));
			InputFieldAction inputFieldAction4 = this.filterFieldAction;
			inputFieldAction4.onDownHandlers = (InputFieldAction.OnDown)Delegate.Combine(inputFieldAction4.onDownHandlers, new InputFieldAction.OnDown(this.HighlightNextButton));
		}
		if (this.alwaysOpen)
		{
			this._visible = true;
			this.MakeVisible();
		}
		else
		{
			this._visible = false;
			if (this.popupPanel != null)
			{
				this.popupPanel.gameObject.SetActive(false);
			}
		}
		this.SyncFilter();
		this.ClearPanel();
		this.CreatePanelButtons();
		this.InitSlider();
	}

	// Token: 0x06006EA8 RID: 28328 RVA: 0x00298A0C File Offset: 0x00296E0C
	private void Update()
	{
		if (this._currentValueChanged)
		{
			this.HighlightCurrentValue();
		}
		if (this.popupPanel != null)
		{
			this.popupPanel.position = base.transform.localToWorldMatrix.MultiplyPoint3x4(this.popupPanelRelativePosition);
		}
		this._currentValueChanged = false;
	}

	// Token: 0x06006EA9 RID: 28329 RVA: 0x00298A68 File Offset: 0x00296E68
	[CompilerGenerated]
	private void <InitSlider>m__0(float A_1)
	{
		int num = Mathf.FloorToInt(this.sliderControl.value);
		if (num >= 0 && num < this._filteredNumPopupValues)
		{
			int num2 = num;
			if (this.useFiltering)
			{
				this.buttonIndexToValueIndex.TryGetValue(num, out num2);
			}
			this.currentValue = this._popupValues[num2];
		}
	}

	// Token: 0x04005F93 RID: 24467
	public bool alwaysOpen;

	// Token: 0x04005F94 RID: 24468
	public bool dynamicTextSize = true;

	// Token: 0x04005F95 RID: 24469
	public int minTextSize = 14;

	// Token: 0x04005F96 RID: 24470
	public Color normalColor = Color.white;

	// Token: 0x04005F97 RID: 24471
	public Color normalBackgroundColor = Color.white;

	// Token: 0x04005F98 RID: 24472
	public Color selectColor = Color.blue;

	// Token: 0x04005F99 RID: 24473
	public Button topButton;

	// Token: 0x04005F9A RID: 24474
	public RectTransform popupPanel;

	// Token: 0x04005F9B RID: 24475
	[SerializeField]
	protected float _popupPanelHeight = 350f;

	// Token: 0x04005F9C RID: 24476
	public RectTransform buttonParent;

	// Token: 0x04005F9D RID: 24477
	public Image backgroundImage;

	// Token: 0x04005F9E RID: 24478
	public Text labelText;

	// Token: 0x04005F9F RID: 24479
	public Slider sliderControl;

	// Token: 0x04005FA0 RID: 24480
	[SerializeField]
	protected bool _showSlider = true;

	// Token: 0x04005FA1 RID: 24481
	public bool useFiltering;

	// Token: 0x04005FA2 RID: 24482
	public InputField filterField;

	// Token: 0x04005FA3 RID: 24483
	public InputFieldAction filterFieldAction;

	// Token: 0x04005FA4 RID: 24484
	public Text filterFieldPlaceholderText;

	// Token: 0x04005FA5 RID: 24485
	public Text filterCountText;

	// Token: 0x04005FA6 RID: 24486
	public RectTransform maxIndicator;

	// Token: 0x04005FA7 RID: 24487
	private int maxNumber = 400;

	// Token: 0x04005FA8 RID: 24488
	protected Dictionary<int, int> valueIndexToButtonIndex;

	// Token: 0x04005FA9 RID: 24489
	protected Dictionary<int, int> buttonIndexToValueIndex;

	// Token: 0x04005FAA RID: 24490
	protected string _filter = string.Empty;

	// Token: 0x04005FAB RID: 24491
	public RectTransform popupButtonPrefab;

	// Token: 0x04005FAC RID: 24492
	[SerializeField]
	private RectTransform[] _popupTransforms;

	// Token: 0x04005FAD RID: 24493
	[SerializeField]
	private Button[] _popupButtons;

	// Token: 0x04005FAE RID: 24494
	private int _filteredNumPopupValues;

	// Token: 0x04005FAF RID: 24495
	[SerializeField]
	private int _numPopupValues;

	// Token: 0x04005FB0 RID: 24496
	private string[] _filterCompareValues;

	// Token: 0x04005FB1 RID: 24497
	[SerializeField]
	private string[] _popupValues;

	// Token: 0x04005FB2 RID: 24498
	public bool useDifferentDisplayValues;

	// Token: 0x04005FB3 RID: 24499
	[SerializeField]
	private string[] _displayPopupValues;

	// Token: 0x04005FB4 RID: 24500
	public float topBottomBuffer = 5f;

	// Token: 0x04005FB5 RID: 24501
	protected ScrollRect scrollRect;

	// Token: 0x04005FB6 RID: 24502
	private bool _visible;

	// Token: 0x04005FB7 RID: 24503
	public UIPopup.OnOpenPopup onOpenPopupHandlers;

	// Token: 0x04005FB8 RID: 24504
	public UIPopup.OnValueChange onValueChangeHandlers;

	// Token: 0x04005FB9 RID: 24505
	public UIPopup.PopupChangeEvent onValueChangeUnityEvent;

	// Token: 0x04005FBA RID: 24506
	[SerializeField]
	private string _currentValue = string.Empty;

	// Token: 0x04005FBB RID: 24507
	protected int _currentValueIndex = -1;

	// Token: 0x04005FBC RID: 24508
	protected int _currentButtonIndex = -1;

	// Token: 0x04005FBD RID: 24509
	protected bool _currentValueChanged;

	// Token: 0x04005FBE RID: 24510
	protected int _currentHighlightIndex = -1;

	// Token: 0x04005FBF RID: 24511
	private Vector3 popupPanelRelativePosition;

	// Token: 0x02000DF7 RID: 3575
	// (Invoke) Token: 0x06006EAB RID: 28331
	public delegate void OnOpenPopup();

	// Token: 0x02000DF8 RID: 3576
	// (Invoke) Token: 0x06006EAF RID: 28335
	public delegate void OnValueChange(string value);

	// Token: 0x02000DF9 RID: 3577
	[Serializable]
	public class PopupChangeEvent : UnityEvent<string>
	{
		// Token: 0x06006EB2 RID: 28338 RVA: 0x00298AC3 File Offset: 0x00296EC3
		public PopupChangeEvent()
		{
		}
	}

	// Token: 0x0200103F RID: 4159
	[CompilerGenerated]
	private sealed class <SetPanelButtonText>c__AnonStorey0
	{
		// Token: 0x06007797 RID: 30615 RVA: 0x00298ACB File Offset: 0x00296ECB
		public <SetPanelButtonText>c__AnonStorey0()
		{
		}

		// Token: 0x06007798 RID: 30616 RVA: 0x00298AD3 File Offset: 0x00296ED3
		internal void <>m__0()
		{
			this.$this.SetCurrentValue(this.popupValueCopy, true, true, false);
		}

		// Token: 0x04006B99 RID: 27545
		internal string popupValueCopy;

		// Token: 0x04006B9A RID: 27546
		internal UIPopup $this;
	}
}
