using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000B23 RID: 2851
public class GenerateDAZDynamicSelectorUI : GenerateTabbedUI
{
	// Token: 0x06004DBD RID: 19901 RVA: 0x001B54C0 File Offset: 0x001B38C0
	public GenerateDAZDynamicSelectorUI()
	{
	}

	// Token: 0x06004DBE RID: 19902 RVA: 0x001B5532 File Offset: 0x001B3932
	public override void TabChange(string name, bool on)
	{
		this.dynamicItemToToggle = new Dictionary<DAZDynamicItem, Toggle>();
		base.TabChange(name, on);
	}

	// Token: 0x06004DBF RID: 19903 RVA: 0x001B5548 File Offset: 0x001B3948
	protected void CloseUserPrefsPanel()
	{
		if (this.currentUserPrefItem != null)
		{
			this.currentUserPrefItem.DeactivateUserPrefs();
		}
		this.currentUserPrefItem = null;
		if (this.userPrefsPanel != null)
		{
			this.userPrefsPanel.gameObject.SetActive(false);
		}
	}

	// Token: 0x06004DC0 RID: 19904 RVA: 0x001B559C File Offset: 0x001B399C
	protected void UserPrefSync(DAZDynamicItem dynamicItem)
	{
		if (this.currentUserPrefItem != null)
		{
			if (this.currentUserPrefItem == dynamicItem)
			{
				if (this.userPrefsPanel.gameObject.activeSelf)
				{
					this.CloseUserPrefsPanel();
				}
				else
				{
					this.userPrefsPanel.gameObject.SetActive(true);
					this.currentUserPrefItem.ActivateUserPrefs();
				}
			}
			else
			{
				this.userPrefsPanel.gameObject.SetActive(true);
				this.currentUserPrefItem.DeactivateUserPrefs();
				this.currentUserPrefItem = dynamicItem;
				this.currentUserPrefItem.ActivateUserPrefs();
			}
		}
		else
		{
			this.userPrefsPanel.gameObject.SetActive(true);
			this.currentUserPrefItem = dynamicItem;
			this.currentUserPrefItem.ActivateUserPrefs();
		}
	}

	// Token: 0x06004DC1 RID: 19905 RVA: 0x001B5664 File Offset: 0x001B3A64
	protected override Transform InstantiateControl(Transform parent, int index)
	{
		GenerateDAZDynamicSelectorUI.<InstantiateControl>c__AnonStorey0 <InstantiateControl>c__AnonStorey = new GenerateDAZDynamicSelectorUI.<InstantiateControl>c__AnonStorey0();
		<InstantiateControl>c__AnonStorey.$this = this;
		Transform transform = base.InstantiateControl(parent, index);
		<InstantiateControl>c__AnonStorey.dynamicItem = this.filteredDynamicItems[index];
		Toggle component = transform.GetComponent<Toggle>();
		if (<InstantiateControl>c__AnonStorey.dynamicItem != null && component != null)
		{
			GenerateDAZDynamicSelectorUI.<InstantiateControl>c__AnonStorey1 <InstantiateControl>c__AnonStorey2 = new GenerateDAZDynamicSelectorUI.<InstantiateControl>c__AnonStorey1();
			<InstantiateControl>c__AnonStorey2.<>f__ref$0 = <InstantiateControl>c__AnonStorey;
			this.dynamicItemToToggle.Add(<InstantiateControl>c__AnonStorey.dynamicItem, component);
			component.onValueChanged.AddListener(new UnityAction<bool>(<InstantiateControl>c__AnonStorey2.<>m__0));
			component.isOn = <InstantiateControl>c__AnonStorey.dynamicItem.active;
			bool isInPackage = <InstantiateControl>c__AnonStorey.dynamicItem.IsInPackage;
			<InstantiateControl>c__AnonStorey2.ddiui = transform.GetComponent<DAZDynamicItemUI>();
			if (<InstantiateControl>c__AnonStorey2.ddiui != null)
			{
				if (isInPackage)
				{
					if (<InstantiateControl>c__AnonStorey2.ddiui.backgroundImage != null)
					{
						<InstantiateControl>c__AnonStorey2.ddiui.backgroundImage.color = this.packageColor;
					}
					if (<InstantiateControl>c__AnonStorey2.ddiui.openInPackageButton != null)
					{
						<InstantiateControl>c__AnonStorey2.ddiui.openInPackageButton.gameObject.SetActive(true);
						<InstantiateControl>c__AnonStorey2.ddiui.openInPackageButton.onClick.AddListener(new UnityAction(<InstantiateControl>c__AnonStorey2.<>m__1));
					}
					if (<InstantiateControl>c__AnonStorey2.ddiui.packageVersionText != null)
					{
						<InstantiateControl>c__AnonStorey2.ddiui.packageVersionText.gameObject.SetActive(true);
						<InstantiateControl>c__AnonStorey2.ddiui.packageVersionText.text = <InstantiateControl>c__AnonStorey.dynamicItem.version;
					}
					if (<InstantiateControl>c__AnonStorey2.ddiui.packageUidText != null)
					{
						<InstantiateControl>c__AnonStorey2.ddiui.packageUidText.text = <InstantiateControl>c__AnonStorey.dynamicItem.packageUid;
					}
					if (<InstantiateControl>c__AnonStorey2.ddiui.packageLicenseText != null)
					{
						<InstantiateControl>c__AnonStorey2.ddiui.packageLicenseText.text = <InstantiateControl>c__AnonStorey.dynamicItem.packageLicense;
					}
				}
				else
				{
					if (<InstantiateControl>c__AnonStorey2.ddiui.openInPackageButton != null)
					{
						<InstantiateControl>c__AnonStorey2.ddiui.openInPackageButton.gameObject.SetActive(false);
					}
					if (<InstantiateControl>c__AnonStorey2.ddiui.packageVersionText != null)
					{
						<InstantiateControl>c__AnonStorey2.ddiui.packageVersionText.gameObject.SetActive(false);
					}
				}
				if (<InstantiateControl>c__AnonStorey2.ddiui.displayNameText != null)
				{
					<InstantiateControl>c__AnonStorey2.ddiui.displayNameText.text = <InstantiateControl>c__AnonStorey.dynamicItem.displayName;
				}
				if (<InstantiateControl>c__AnonStorey2.ddiui.creatorNameText != null)
				{
					<InstantiateControl>c__AnonStorey2.ddiui.creatorNameText.text = <InstantiateControl>c__AnonStorey.dynamicItem.creatorName;
				}
				if (<InstantiateControl>c__AnonStorey2.ddiui.customizationButton != null)
				{
					if (<InstantiateControl>c__AnonStorey.dynamicItem.hasCustomizationUI)
					{
						<InstantiateControl>c__AnonStorey2.ddiui.customizationButton.gameObject.SetActive(true);
						<InstantiateControl>c__AnonStorey2.ddiui.customizationButton.onClick.AddListener(new UnityAction(<InstantiateControl>c__AnonStorey2.<>m__2));
					}
					else
					{
						<InstantiateControl>c__AnonStorey2.ddiui.customizationButton.gameObject.SetActive(false);
					}
				}
				if (<InstantiateControl>c__AnonStorey2.ddiui.rawImage != null)
				{
					<InstantiateControl>c__AnonStorey.dynamicItem.GetThumbnail(new DAZDynamicItem.ThumbnailLoadedCallback(<InstantiateControl>c__AnonStorey2.<>m__3));
				}
				if (<InstantiateControl>c__AnonStorey2.ddiui.toggleUserPrefsPanelButton != null)
				{
					if (<InstantiateControl>c__AnonStorey.dynamicItem.hasUserPrefs)
					{
						<InstantiateControl>c__AnonStorey2.ddiui.toggleUserPrefsPanelButton.gameObject.SetActive(true);
						<InstantiateControl>c__AnonStorey2.ddiui.toggleUserPrefsPanelButton.onClick.AddListener(new UnityAction(<InstantiateControl>c__AnonStorey2.<>m__4));
					}
					else
					{
						<InstantiateControl>c__AnonStorey2.ddiui.toggleUserPrefsPanelButton.gameObject.SetActive(false);
					}
				}
				if (<InstantiateControl>c__AnonStorey2.ddiui.hiddenIndicator != null)
				{
					<InstantiateControl>c__AnonStorey2.ddiui.hiddenIndicator.gameObject.SetActive(<InstantiateControl>c__AnonStorey.dynamicItem.isHidden);
				}
				<InstantiateControl>c__AnonStorey.dynamicItem.dynamicSelectorUI = this;
			}
			else
			{
				Debug.LogError("Could not find DAZDynamicUI component");
			}
		}
		return transform;
	}

	// Token: 0x06004DC2 RID: 19906 RVA: 0x001B5A88 File Offset: 0x001B3E88
	public void SetDynamicItemToggle(DAZDynamicItem dynamicItem, bool on)
	{
		Toggle toggle;
		if (this.dynamicItemToToggle != null && this.dynamicItemToToggle.TryGetValue(dynamicItem, out toggle))
		{
			toggle.isOn = on;
		}
	}

	// Token: 0x06004DC3 RID: 19907 RVA: 0x001B5ABC File Offset: 0x001B3EBC
	public void RefreshThumbnails()
	{
		if (this.dynamicItemToToggle != null)
		{
			foreach (DAZDynamicItem dazdynamicItem in this.dynamicItemToToggle.Keys)
			{
				Toggle toggle;
				if (this.dynamicItemToToggle.TryGetValue(dazdynamicItem, out toggle))
				{
					GenerateDAZDynamicSelectorUI.<RefreshThumbnails>c__AnonStorey2 <RefreshThumbnails>c__AnonStorey = new GenerateDAZDynamicSelectorUI.<RefreshThumbnails>c__AnonStorey2();
					<RefreshThumbnails>c__AnonStorey.ddiui = toggle.GetComponent<DAZDynamicItemUI>();
					if (<RefreshThumbnails>c__AnonStorey.ddiui != null && <RefreshThumbnails>c__AnonStorey.ddiui.rawImage != null)
					{
						dazdynamicItem.GetThumbnail(new DAZDynamicItem.ThumbnailLoadedCallback(<RefreshThumbnails>c__AnonStorey.<>m__0));
					}
				}
			}
		}
	}

	// Token: 0x06004DC4 RID: 19908 RVA: 0x001B5B80 File Offset: 0x001B3F80
	public void SetSortBy(string sortByString)
	{
		try
		{
			GenerateDAZDynamicSelectorUI.SortBy sortBy = (GenerateDAZDynamicSelectorUI.SortBy)Enum.Parse(typeof(GenerateDAZDynamicSelectorUI.SortBy), sortByString);
			this.sortBy = sortBy;
		}
		catch (ArgumentException)
		{
			Debug.LogError("Attempted to set sort by to " + sortByString + " which is not a valid type");
		}
	}

	// Token: 0x17000B0F RID: 2831
	// (get) Token: 0x06004DC5 RID: 19909 RVA: 0x001B5BDC File Offset: 0x001B3FDC
	// (set) Token: 0x06004DC6 RID: 19910 RVA: 0x001B5BE4 File Offset: 0x001B3FE4
	public GenerateDAZDynamicSelectorUI.SortBy sortBy
	{
		get
		{
			return this._sortBy;
		}
		set
		{
			if (this._sortBy != value)
			{
				this._sortBy = value;
				this.ResyncUI();
			}
		}
	}

	// Token: 0x06004DC7 RID: 19911 RVA: 0x001B5C00 File Offset: 0x001B4000
	public void SetShowType(string showTypeString)
	{
		try
		{
			GenerateDAZDynamicSelectorUI.ShowType showType = (GenerateDAZDynamicSelectorUI.ShowType)Enum.Parse(typeof(GenerateDAZDynamicSelectorUI.ShowType), showTypeString);
			this.showType = showType;
		}
		catch (ArgumentException)
		{
			Debug.LogError("Attempted to set show type to " + showTypeString + " which is not a valid type");
		}
	}

	// Token: 0x17000B10 RID: 2832
	// (get) Token: 0x06004DC8 RID: 19912 RVA: 0x001B5C5C File Offset: 0x001B405C
	// (set) Token: 0x06004DC9 RID: 19913 RVA: 0x001B5C64 File Offset: 0x001B4064
	public GenerateDAZDynamicSelectorUI.ShowType showType
	{
		get
		{
			return this._showType;
		}
		set
		{
			if (this._showType != value)
			{
				this._showType = value;
				this.ResyncUI();
			}
		}
	}

	// Token: 0x06004DCA RID: 19914 RVA: 0x001B5C7F File Offset: 0x001B407F
	public void SetNameFilter(string f)
	{
		this.nameFilter = f;
	}

	// Token: 0x17000B11 RID: 2833
	// (get) Token: 0x06004DCB RID: 19915 RVA: 0x001B5C88 File Offset: 0x001B4088
	// (set) Token: 0x06004DCC RID: 19916 RVA: 0x001B5C90 File Offset: 0x001B4090
	public string nameFilter
	{
		get
		{
			return this._nameFilter;
		}
		set
		{
			if (this._nameFilter != value)
			{
				this._nameFilter = value;
				this.ResyncUI();
			}
		}
	}

	// Token: 0x06004DCD RID: 19917 RVA: 0x001B5CB0 File Offset: 0x001B40B0
	public void SetLatestOnly(bool b)
	{
		this.latestOnly = b;
	}

	// Token: 0x17000B12 RID: 2834
	// (get) Token: 0x06004DCE RID: 19918 RVA: 0x001B5CB9 File Offset: 0x001B40B9
	// (set) Token: 0x06004DCF RID: 19919 RVA: 0x001B5CC1 File Offset: 0x001B40C1
	public bool latestOnly
	{
		get
		{
			return this._latestOnly;
		}
		set
		{
			if (this._latestOnly != value)
			{
				this._latestOnly = value;
				this.ResyncUI();
			}
		}
	}

	// Token: 0x06004DD0 RID: 19920 RVA: 0x001B5CDC File Offset: 0x001B40DC
	public void SetShowLegacy(bool b)
	{
		this.showLegacy = b;
	}

	// Token: 0x17000B13 RID: 2835
	// (get) Token: 0x06004DD1 RID: 19921 RVA: 0x001B5CE5 File Offset: 0x001B40E5
	// (set) Token: 0x06004DD2 RID: 19922 RVA: 0x001B5CED File Offset: 0x001B40ED
	public bool showLegacy
	{
		get
		{
			return this._showLegacy;
		}
		set
		{
			if (this._showLegacy != value)
			{
				this._showLegacy = value;
				this.ResyncUI();
			}
		}
	}

	// Token: 0x06004DD3 RID: 19923 RVA: 0x001B5D08 File Offset: 0x001B4108
	public void SetCreatorNameFilter(string f)
	{
		this.creatorNameFilter = f;
	}

	// Token: 0x17000B14 RID: 2836
	// (get) Token: 0x06004DD4 RID: 19924 RVA: 0x001B5D11 File Offset: 0x001B4111
	// (set) Token: 0x06004DD5 RID: 19925 RVA: 0x001B5D19 File Offset: 0x001B4119
	public string creatorNameFilter
	{
		get
		{
			return this._creatorNameFilter;
		}
		set
		{
			if (this._creatorNameFilter != value)
			{
				this._creatorNameFilter = value;
				this.ResyncUI();
			}
		}
	}

	// Token: 0x06004DD6 RID: 19926 RVA: 0x001B5D39 File Offset: 0x001B4139
	protected void SetSingleTagFilter(bool b)
	{
		this.singleTagFilter = b;
	}

	// Token: 0x17000B15 RID: 2837
	// (get) Token: 0x06004DD7 RID: 19927 RVA: 0x001B5D42 File Offset: 0x001B4142
	// (set) Token: 0x06004DD8 RID: 19928 RVA: 0x001B5D4C File Offset: 0x001B414C
	protected bool singleTagFilter
	{
		get
		{
			return this._singleTagFilter;
		}
		set
		{
			if (this._singleTagFilter != value)
			{
				this._singleTagFilter = value;
				if (this.singleTagFilterToggle != null)
				{
					this.singleTagFilterToggle.isOn = this._singleTagFilter;
				}
				if (this._singleTagFilter && this.tagsFilterSet.Count > 1)
				{
					List<string> list = new List<string>(this.tagsFilterSet);
					this.SetTagsFilter(list[0]);
				}
			}
		}
	}

	// Token: 0x06004DD9 RID: 19929 RVA: 0x001B5DC3 File Offset: 0x001B41C3
	public void OpenTagsPanel()
	{
		this.CloseUserPrefsPanel();
		if (this.tagsPanel != null)
		{
			this.tagsPanel.gameObject.SetActive(true);
		}
	}

	// Token: 0x06004DDA RID: 19930 RVA: 0x001B5DED File Offset: 0x001B41ED
	public void CloseTagsPanel()
	{
		if (this.tagsPanel != null)
		{
			this.tagsPanel.gameObject.SetActive(false);
		}
	}

	// Token: 0x06004DDB RID: 19931 RVA: 0x001B5E11 File Offset: 0x001B4211
	public HashSet<string> GetOtherTags()
	{
		return this.otherTags;
	}

	// Token: 0x06004DDC RID: 19932 RVA: 0x001B5E1C File Offset: 0x001B421C
	protected void SyncFilterSetToFilter()
	{
		if (this._tagsFilter != null && this._tagsFilter != string.Empty)
		{
			string[] collection = this._tagsFilter.Split(new char[]
			{
				','
			});
			this.tagsFilterSet = new HashSet<string>(collection);
		}
		else
		{
			this.tagsFilterSet = new HashSet<string>();
		}
	}

	// Token: 0x06004DDD RID: 19933 RVA: 0x001B5E7C File Offset: 0x001B427C
	protected void SyncFilterToFilterSet()
	{
		string[] array = new string[this.tagsFilterSet.Count];
		this.tagsFilterSet.CopyTo(array);
		this.tagsFilter = string.Join(",", array);
	}

	// Token: 0x06004DDE RID: 19934 RVA: 0x001B5EB8 File Offset: 0x001B42B8
	protected void SyncFilterTagFromToggle(string tag, bool isEnabled)
	{
		if (isEnabled)
		{
			if (this._singleTagFilter)
			{
				this.tagsFilterSet.Clear();
				this.tagsFilterSet.Add(tag);
			}
			else
			{
				this.tagsFilterSet.Add(tag);
			}
		}
		else
		{
			this.tagsFilterSet.Remove(tag);
		}
		this.SyncFilterToFilterSet();
		if (isEnabled && this._singleTagFilter)
		{
			this.SyncFilterTogglesToFilter();
		}
	}

	// Token: 0x06004DDF RID: 19935 RVA: 0x001B5F30 File Offset: 0x001B4330
	protected void SyncFilterTogglesToFilter()
	{
		foreach (KeyValuePair<string, Toggle> keyValuePair in this.tagToToggle)
		{
			if (this.tagsFilterSet.Contains(keyValuePair.Key))
			{
				if (keyValuePair.Value != null)
				{
					keyValuePair.Value.isOn = true;
				}
			}
			else if (keyValuePair.Value != null)
			{
				keyValuePair.Value.isOn = false;
			}
		}
	}

	// Token: 0x06004DE0 RID: 19936 RVA: 0x001B5FE0 File Offset: 0x001B43E0
	public void SetTagsFilter(string f)
	{
		if (!this.skipFieldSetCallback)
		{
			string text = f.Trim();
			text = Regex.Replace(text, ",\\s+", ",");
			text = Regex.Replace(text, "\\s+,", ",");
			text = text.ToLower();
			if (text != f && this.tagsFilterField != null)
			{
				this.skipFieldSetCallback = true;
				this.tagsFilterField.text = text;
				this.skipFieldSetCallback = false;
			}
			this.tagsFilter = text;
			this.SyncFilterSetToFilter();
			this.SyncFilterTogglesToFilter();
		}
	}

	// Token: 0x17000B16 RID: 2838
	// (get) Token: 0x06004DE1 RID: 19937 RVA: 0x001B6072 File Offset: 0x001B4472
	// (set) Token: 0x06004DE2 RID: 19938 RVA: 0x001B607C File Offset: 0x001B447C
	protected string tagsFilter
	{
		get
		{
			return this._tagsFilter;
		}
		set
		{
			if (this._tagsFilter != value)
			{
				this._tagsFilter = value;
				if (this.tagsFilterField != null)
				{
					this.skipFieldSetCallback = true;
					this.tagsFilterField.text = this._tagsFilter;
					this.skipFieldSetCallback = false;
				}
				this.ResyncUI();
			}
		}
	}

	// Token: 0x06004DE3 RID: 19939 RVA: 0x001B60D8 File Offset: 0x001B44D8
	protected void CreateTagToggle(string tag, Transform parent)
	{
		GenerateDAZDynamicSelectorUI.<CreateTagToggle>c__AnonStorey3 <CreateTagToggle>c__AnonStorey = new GenerateDAZDynamicSelectorUI.<CreateTagToggle>c__AnonStorey3();
		<CreateTagToggle>c__AnonStorey.tag = tag;
		<CreateTagToggle>c__AnonStorey.$this = this;
		Transform transform = UnityEngine.Object.Instantiate<Transform>(this.tagTogglePrefab);
		Text componentInChildren = transform.GetComponentInChildren<Text>();
		componentInChildren.text = <CreateTagToggle>c__AnonStorey.tag;
		ToggleGroup component = parent.GetComponent<ToggleGroup>();
		Toggle componentInChildren2 = transform.GetComponentInChildren<Toggle>();
		if (component != null)
		{
			componentInChildren2.group = component;
		}
		componentInChildren2.onValueChanged.AddListener(new UnityAction<bool>(<CreateTagToggle>c__AnonStorey.<>m__0));
		this.tagToToggle.Remove(<CreateTagToggle>c__AnonStorey.tag);
		this.tagToToggle.Add(<CreateTagToggle>c__AnonStorey.tag, componentInChildren2);
		transform.SetParent(parent, false);
	}

	// Token: 0x06004DE4 RID: 19940 RVA: 0x001B6180 File Offset: 0x001B4580
	protected void SyncOtherTagsUI()
	{
		if (this.tagTogglePrefab != null && this.otherTagsContent != null)
		{
			IEnumerator enumerator = this.otherTagsContent.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					Transform transform = (Transform)obj;
					UnityEngine.Object.Destroy(transform.gameObject);
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
			List<string> list = this.otherTags.ToList<string>();
			list.Sort();
			foreach (string tag in list)
			{
				this.CreateTagToggle(tag, this.otherTagsContent);
			}
		}
	}

	// Token: 0x06004DE5 RID: 19941 RVA: 0x001B626C File Offset: 0x001B466C
	protected void InitTagsUI()
	{
		this.tagToToggle = new Dictionary<string, Toggle>();
		if (this.tagTogglePrefab != null)
		{
			if (this.regionTags != null && this.regionTagsContent != null)
			{
				List<string> list = new List<string>();
				for (int i = 0; i < this.regionTags.Length; i++)
				{
					list.Add(this.regionTags[i].ToLower());
				}
				list.Sort();
				foreach (string tag in list)
				{
					this.CreateTagToggle(tag, this.regionTagsContent);
				}
			}
			if (this.typeTags != null && this.typeTagsContent != null)
			{
				List<string> list2 = new List<string>();
				for (int j = 0; j < this.typeTags.Length; j++)
				{
					list2.Add(this.typeTags[j].ToLower());
				}
				list2.Sort();
				foreach (string tag2 in list2)
				{
					this.CreateTagToggle(tag2, this.typeTagsContent);
				}
			}
		}
		this.SyncOtherTagsUI();
	}

	// Token: 0x06004DE6 RID: 19942 RVA: 0x001B63EC File Offset: 0x001B47EC
	protected virtual DAZDynamicItem[] GetDynamicItems()
	{
		return new DAZDynamicItem[0];
	}

	// Token: 0x06004DE7 RID: 19943 RVA: 0x001B63F4 File Offset: 0x001B47F4
	public void ResyncTags()
	{
		if (this.allTags == null)
		{
			this.allTags = new HashSet<string>();
		}
		else
		{
			this.allTags.Clear();
		}
		foreach (string item in this.regionTags)
		{
			this.allTags.Add(item);
		}
		foreach (string item2 in this.typeTags)
		{
			this.allTags.Add(item2);
		}
		if (this.otherTags == null)
		{
			this.otherTags = new HashSet<string>();
		}
		else
		{
			this.otherTags.Clear();
		}
		if (this.dynamicItems == null)
		{
			this.dynamicItems = new List<DAZDynamicItem>();
		}
		else
		{
			this.dynamicItems.Clear();
		}
		if (this.characterSelector != null)
		{
			foreach (DAZDynamicItem dazdynamicItem in this.GetDynamicItems())
			{
				dazdynamicItem.Init();
				foreach (string item3 in dazdynamicItem.tagsArray)
				{
					if (!this.allTags.Contains(item3))
					{
						this.allTags.Add(item3);
						this.otherTags.Add(item3);
					}
				}
				this.dynamicItems.Add(dazdynamicItem);
			}
		}
		this.SyncOtherTagsUI();
	}

	// Token: 0x06004DE8 RID: 19944 RVA: 0x001B6580 File Offset: 0x001B4980
	public void ResyncItems()
	{
		this.ResyncTags();
		Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
		foreach (DAZDynamicItem dazdynamicItem in this.dynamicItems)
		{
			string text = dazdynamicItem.creatorName;
			if (text == null || text == string.Empty)
			{
				text = "None";
			}
			if (!dictionary.ContainsKey(text))
			{
				dictionary.Add(text, true);
			}
		}
		this.creatorChoices = dictionary.Keys.ToList<string>();
		this.creatorChoices.Sort();
		this.creatorChoices.Insert(0, "All");
		if (this.creatorNameFilterPopup != null)
		{
			this.creatorNameFilterPopup.numPopupValues = this.creatorChoices.Count;
			for (int i = 0; i < this.creatorChoices.Count; i++)
			{
				this.creatorNameFilterPopup.setPopupValue(i, this.creatorChoices[i]);
			}
		}
	}

	// Token: 0x06004DE9 RID: 19945 RVA: 0x001B66A8 File Offset: 0x001B4AA8
	protected bool CaseInsensitiveStringContains(string s1, string s2)
	{
		return s1.IndexOf(s2, StringComparison.CurrentCultureIgnoreCase) >= 0;
	}

	// Token: 0x06004DEA RID: 19946 RVA: 0x001B66B8 File Offset: 0x001B4AB8
	protected void ResyncUIInternal()
	{
		this.dynamicItemToToggle = new Dictionary<DAZDynamicItem, Toggle>();
		if (this.controlUIPrefab != null && this.tabUIPrefab != null && this.tabButtonUIPrefab != null && this.dynamicItems != null)
		{
			string[] array = null;
			if (this._tagsFilter != null && this._tagsFilter != string.Empty)
			{
				array = this._tagsFilter.Split(new char[]
				{
					','
				});
			}
			List<DAZDynamicItem> list = new List<DAZDynamicItem>();
			this.filteredDynamicItems = new List<DAZDynamicItem>();
			foreach (DAZDynamicItem dazdynamicItem in this.dynamicItems)
			{
				if (dazdynamicItem.showFirstInUI)
				{
					list.Add(dazdynamicItem);
				}
				else
				{
					switch (this._showType)
					{
					case GenerateDAZDynamicSelectorUI.ShowType.Active:
						if (!dazdynamicItem.active)
						{
							continue;
						}
						break;
					case GenerateDAZDynamicSelectorUI.ShowType.BuiltInAll:
						if (dazdynamicItem.type == DAZDynamicItem.Type.Custom)
						{
							continue;
						}
						break;
					case GenerateDAZDynamicSelectorUI.ShowType.BuiltInSim:
						if (dazdynamicItem.type != DAZDynamicItem.Type.Sim)
						{
							continue;
						}
						break;
					case GenerateDAZDynamicSelectorUI.ShowType.BuiltInWrap:
						if (dazdynamicItem.type != DAZDynamicItem.Type.Wrap)
						{
							continue;
						}
						break;
					case GenerateDAZDynamicSelectorUI.ShowType.CustomAll:
						if (dazdynamicItem.type != DAZDynamicItem.Type.Custom)
						{
							continue;
						}
						break;
					case GenerateDAZDynamicSelectorUI.ShowType.CustomPackage:
						if (dazdynamicItem.type != DAZDynamicItem.Type.Custom || !dazdynamicItem.IsInPackage)
						{
							continue;
						}
						break;
					case GenerateDAZDynamicSelectorUI.ShowType.CustomLocal:
						if (dazdynamicItem.type != DAZDynamicItem.Type.Custom || dazdynamicItem.IsInPackage)
						{
							continue;
						}
						break;
					case GenerateDAZDynamicSelectorUI.ShowType.Locked:
						if (!dazdynamicItem.locked)
						{
							continue;
						}
						break;
					case GenerateDAZDynamicSelectorUI.ShowType.MissingTags:
						if (dazdynamicItem.tagsArray != null && dazdynamicItem.tagsArray.Length != 0)
						{
							continue;
						}
						break;
					case GenerateDAZDynamicSelectorUI.ShowType.Hidden:
						if (!dazdynamicItem.isHidden)
						{
							continue;
						}
						break;
					case GenerateDAZDynamicSelectorUI.ShowType.NotLatest:
						if (dazdynamicItem.isLatestVersion)
						{
							continue;
						}
						break;
					case GenerateDAZDynamicSelectorUI.ShowType.Real:
						if (!dazdynamicItem.isRealItem)
						{
							continue;
						}
						break;
					case GenerateDAZDynamicSelectorUI.ShowType.NotReal:
						if (dazdynamicItem.isRealItem)
						{
							continue;
						}
						break;
					}
					if (this._showType == GenerateDAZDynamicSelectorUI.ShowType.Hidden || !dazdynamicItem.isHidden || dazdynamicItem.active)
					{
						if (this._showType == GenerateDAZDynamicSelectorUI.ShowType.NotLatest || !this._latestOnly || dazdynamicItem.active || dazdynamicItem.isLatestVersion)
						{
							if (!dazdynamicItem.isLegacy || dazdynamicItem.active || this._showLegacy)
							{
								if (this._nameFilter == null || !(this._nameFilter != string.Empty) || this.CaseInsensitiveStringContains(dazdynamicItem.displayName, this._nameFilter))
								{
									if (this._creatorNameFilter == null || !(this._creatorNameFilter != string.Empty) || !(this._creatorNameFilter != "All") || !(dazdynamicItem.creatorName != this._creatorNameFilter))
									{
										if (array != null)
										{
											bool flag = true;
											foreach (string text in array)
											{
												if (text != string.Empty && !dazdynamicItem.CheckMatchTag(text))
												{
													flag = false;
													break;
												}
											}
											if (!flag)
											{
												continue;
											}
										}
										this.filteredDynamicItems.Add(dazdynamicItem);
									}
								}
							}
						}
					}
				}
			}
			switch (this.sortBy)
			{
			case GenerateDAZDynamicSelectorUI.SortBy.AtoZ:
			{
				List<DAZDynamicItem> list2 = this.filteredDynamicItems;
				if (GenerateDAZDynamicSelectorUI.<>f__am$cache0 == null)
				{
					GenerateDAZDynamicSelectorUI.<>f__am$cache0 = new Comparison<DAZDynamicItem>(GenerateDAZDynamicSelectorUI.<ResyncUIInternal>m__0);
				}
				list2.Sort(GenerateDAZDynamicSelectorUI.<>f__am$cache0);
				break;
			}
			case GenerateDAZDynamicSelectorUI.SortBy.ZtoA:
			{
				List<DAZDynamicItem> list3 = this.filteredDynamicItems;
				if (GenerateDAZDynamicSelectorUI.<>f__am$cache1 == null)
				{
					GenerateDAZDynamicSelectorUI.<>f__am$cache1 = new Comparison<DAZDynamicItem>(GenerateDAZDynamicSelectorUI.<ResyncUIInternal>m__1);
				}
				list3.Sort(GenerateDAZDynamicSelectorUI.<>f__am$cache1);
				break;
			}
			case GenerateDAZDynamicSelectorUI.SortBy.NewToOld:
				this.filteredDynamicItems.Reverse();
				break;
			case GenerateDAZDynamicSelectorUI.SortBy.Creator:
			{
				List<DAZDynamicItem> list4 = this.filteredDynamicItems;
				if (GenerateDAZDynamicSelectorUI.<>f__am$cache2 == null)
				{
					GenerateDAZDynamicSelectorUI.<>f__am$cache2 = new Comparison<DAZDynamicItem>(GenerateDAZDynamicSelectorUI.<ResyncUIInternal>m__2);
				}
				list4.Sort(GenerateDAZDynamicSelectorUI.<>f__am$cache2);
				break;
			}
			}
			this.filteredDynamicItems.InsertRange(0, list);
			for (int j = 0; j < this.filteredDynamicItems.Count; j++)
			{
				base.AllocateControl();
			}
		}
	}

	// Token: 0x06004DEB RID: 19947 RVA: 0x001B6BC0 File Offset: 0x001B4FC0
	public void ResyncUIIfActiveFilterOn()
	{
		if (this._showType == GenerateDAZDynamicSelectorUI.ShowType.Active)
		{
			this.ResyncUI();
		}
	}

	// Token: 0x06004DEC RID: 19948 RVA: 0x001B6BD4 File Offset: 0x001B4FD4
	protected void GotoTabWithItem(DAZDynamicItem di)
	{
		for (int i = 0; i < this.filteredDynamicItems.Count; i++)
		{
			if (this.filteredDynamicItems[i] == di)
			{
				int tabNum = i / this.numElementsPerTab + 1;
				this.GotoTab(tabNum);
				break;
			}
		}
	}

	// Token: 0x06004DED RID: 19949 RVA: 0x001B6C2B File Offset: 0x001B502B
	public void ResyncUI()
	{
		this.GenerateStart();
		this.ResyncUIInternal();
		this.GenerateFinish();
		if (this.currentUserPrefItem != null)
		{
			this.GotoTabWithItem(this.currentUserPrefItem);
		}
	}

	// Token: 0x06004DEE RID: 19950 RVA: 0x001B6C5C File Offset: 0x001B505C
	public void Resync()
	{
		this.ResyncItems();
		this.ResyncUI();
	}

	// Token: 0x06004DEF RID: 19951 RVA: 0x001B6C6A File Offset: 0x001B506A
	protected override void Generate()
	{
		base.Generate();
		this.ResyncItems();
		this.ResyncUIInternal();
	}

	// Token: 0x06004DF0 RID: 19952 RVA: 0x001B6C80 File Offset: 0x001B5080
	protected void Awake()
	{
		if (this.sortByPopup != null)
		{
			UIPopup uipopup = this.sortByPopup;
			uipopup.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetSortBy));
		}
		if (this.showTypePopup != null)
		{
			UIPopup uipopup2 = this.showTypePopup;
			uipopup2.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup2.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetShowType));
		}
		if (this.latestOnlyToggle != null)
		{
			this.latestOnlyToggle.isOn = this._latestOnly;
			this.latestOnlyToggle.onValueChanged.AddListener(new UnityAction<bool>(this.SetLatestOnly));
		}
		if (this.showLegacyToggle != null)
		{
			this.showLegacyToggle.isOn = this._showLegacy;
			this.showLegacyToggle.onValueChanged.AddListener(new UnityAction<bool>(this.SetShowLegacy));
		}
		if (this.nameFilterField != null)
		{
			this.nameFilterField.text = this._nameFilter;
			this.nameFilterField.onValueChanged.AddListener(new UnityAction<string>(this.SetNameFilter));
		}
		if (this.creatorNameFilterPopup != null)
		{
			this.creatorNameFilterPopup.currentValue = this._creatorNameFilter;
			UIPopup uipopup3 = this.creatorNameFilterPopup;
			uipopup3.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup3.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetCreatorNameFilter));
		}
		if (this.tagsFilterField != null)
		{
			this.tagsFilterField.text = this._tagsFilter;
			this.tagsFilterField.onValueChanged.AddListener(new UnityAction<string>(this.SetTagsFilter));
		}
		if (this.singleTagFilterToggle != null)
		{
			this.singleTagFilterToggle.isOn = this._singleTagFilter;
			this.singleTagFilterToggle.onValueChanged.AddListener(new UnityAction<bool>(this.SetSingleTagFilter));
		}
		if (this.closeUserPrefsPanelButton != null)
		{
			this.closeUserPrefsPanelButton.onClick.AddListener(new UnityAction(this.CloseUserPrefsPanel));
		}
		if (this.resyncUIButton != null)
		{
			this.resyncUIButton.onClick.AddListener(new UnityAction(this.ResyncUI));
		}
		this.InitTagsUI();
	}

	// Token: 0x06004DF1 RID: 19953 RVA: 0x001B6EDC File Offset: 0x001B52DC
	[CompilerGenerated]
	private static int <ResyncUIInternal>m__0(DAZDynamicItem i1, DAZDynamicItem i2)
	{
		return i1.displayName.CompareTo(i2.displayName);
	}

	// Token: 0x06004DF2 RID: 19954 RVA: 0x001B6EEF File Offset: 0x001B52EF
	[CompilerGenerated]
	private static int <ResyncUIInternal>m__1(DAZDynamicItem i1, DAZDynamicItem i2)
	{
		return i2.displayName.CompareTo(i1.displayName);
	}

	// Token: 0x06004DF3 RID: 19955 RVA: 0x001B6F02 File Offset: 0x001B5302
	[CompilerGenerated]
	private static int <ResyncUIInternal>m__2(DAZDynamicItem i1, DAZDynamicItem i2)
	{
		if (i1.creatorName == i2.creatorName)
		{
			return i1.displayName.CompareTo(i2.displayName);
		}
		return i1.creatorName.CompareTo(i2.creatorName);
	}

	// Token: 0x04003D72 RID: 15730
	public DAZCharacterSelector characterSelector;

	// Token: 0x04003D73 RID: 15731
	public RectTransform userPrefsPanel;

	// Token: 0x04003D74 RID: 15732
	public Text userPrefsLabel;

	// Token: 0x04003D75 RID: 15733
	public RectTransform userPrefsRegionTagsContent;

	// Token: 0x04003D76 RID: 15734
	public RectTransform userPrefsTypeTagsContent;

	// Token: 0x04003D77 RID: 15735
	public RectTransform userPrefsOtherTagsContent;

	// Token: 0x04003D78 RID: 15736
	public Button closeUserPrefsPanelButton;

	// Token: 0x04003D79 RID: 15737
	public Toggle userPrefsHideToggle;

	// Token: 0x04003D7A RID: 15738
	public Text userPrefsBuiltInTagsText;

	// Token: 0x04003D7B RID: 15739
	public Toggle userPrefsReplaceTagsToggle;

	// Token: 0x04003D7C RID: 15740
	public InputField userPrefsTagsInputField;

	// Token: 0x04003D7D RID: 15741
	public Button resyncUIButton;

	// Token: 0x04003D7E RID: 15742
	protected DAZDynamicItem currentUserPrefItem;

	// Token: 0x04003D7F RID: 15743
	protected Color packageColor = new Color(1f, 0.85f, 0.9f);

	// Token: 0x04003D80 RID: 15744
	public UIPopup sortByPopup;

	// Token: 0x04003D81 RID: 15745
	protected GenerateDAZDynamicSelectorUI.SortBy _sortBy = GenerateDAZDynamicSelectorUI.SortBy.NewToOld;

	// Token: 0x04003D82 RID: 15746
	public UIPopup showTypePopup;

	// Token: 0x04003D83 RID: 15747
	protected GenerateDAZDynamicSelectorUI.ShowType _showType;

	// Token: 0x04003D84 RID: 15748
	public InputField nameFilterField;

	// Token: 0x04003D85 RID: 15749
	protected string _nameFilter;

	// Token: 0x04003D86 RID: 15750
	public Toggle latestOnlyToggle;

	// Token: 0x04003D87 RID: 15751
	protected bool _latestOnly = true;

	// Token: 0x04003D88 RID: 15752
	public Toggle showLegacyToggle;

	// Token: 0x04003D89 RID: 15753
	protected bool _showLegacy;

	// Token: 0x04003D8A RID: 15754
	public UIPopup creatorNameFilterPopup;

	// Token: 0x04003D8B RID: 15755
	protected string _creatorNameFilter = "All";

	// Token: 0x04003D8C RID: 15756
	public RectTransform tagsPanel;

	// Token: 0x04003D8D RID: 15757
	public Transform tagTogglePrefab;

	// Token: 0x04003D8E RID: 15758
	public RectTransform regionTagsContent;

	// Token: 0x04003D8F RID: 15759
	public RectTransform typeTagsContent;

	// Token: 0x04003D90 RID: 15760
	public RectTransform otherTagsContent;

	// Token: 0x04003D91 RID: 15761
	public Toggle singleTagFilterToggle;

	// Token: 0x04003D92 RID: 15762
	protected bool _singleTagFilter;

	// Token: 0x04003D93 RID: 15763
	public string[] regionTags;

	// Token: 0x04003D94 RID: 15764
	public string[] typeTags;

	// Token: 0x04003D95 RID: 15765
	protected HashSet<string> allTags = new HashSet<string>();

	// Token: 0x04003D96 RID: 15766
	protected HashSet<string> otherTags = new HashSet<string>();

	// Token: 0x04003D97 RID: 15767
	protected HashSet<string> tagsFilterSet = new HashSet<string>();

	// Token: 0x04003D98 RID: 15768
	protected bool skipFieldSetCallback;

	// Token: 0x04003D99 RID: 15769
	public InputField tagsFilterField;

	// Token: 0x04003D9A RID: 15770
	protected string _tagsFilter;

	// Token: 0x04003D9B RID: 15771
	protected Dictionary<string, Toggle> tagToToggle = new Dictionary<string, Toggle>();

	// Token: 0x04003D9C RID: 15772
	protected List<DAZDynamicItem> dynamicItems;

	// Token: 0x04003D9D RID: 15773
	protected Dictionary<DAZDynamicItem, Toggle> dynamicItemToToggle;

	// Token: 0x04003D9E RID: 15774
	protected List<DAZDynamicItem> filteredDynamicItems;

	// Token: 0x04003D9F RID: 15775
	protected List<string> creatorChoices;

	// Token: 0x04003DA0 RID: 15776
	[CompilerGenerated]
	private static Comparison<DAZDynamicItem> <>f__am$cache0;

	// Token: 0x04003DA1 RID: 15777
	[CompilerGenerated]
	private static Comparison<DAZDynamicItem> <>f__am$cache1;

	// Token: 0x04003DA2 RID: 15778
	[CompilerGenerated]
	private static Comparison<DAZDynamicItem> <>f__am$cache2;

	// Token: 0x02000B24 RID: 2852
	public enum SortBy
	{
		// Token: 0x04003DA4 RID: 15780
		AtoZ,
		// Token: 0x04003DA5 RID: 15781
		ZtoA,
		// Token: 0x04003DA6 RID: 15782
		NewToOld,
		// Token: 0x04003DA7 RID: 15783
		OldToNew,
		// Token: 0x04003DA8 RID: 15784
		Creator
	}

	// Token: 0x02000B25 RID: 2853
	public enum ShowType
	{
		// Token: 0x04003DAA RID: 15786
		All,
		// Token: 0x04003DAB RID: 15787
		Active,
		// Token: 0x04003DAC RID: 15788
		BuiltInAll,
		// Token: 0x04003DAD RID: 15789
		BuiltInSim,
		// Token: 0x04003DAE RID: 15790
		BuiltInWrap,
		// Token: 0x04003DAF RID: 15791
		CustomAll,
		// Token: 0x04003DB0 RID: 15792
		CustomPackage,
		// Token: 0x04003DB1 RID: 15793
		CustomLocal,
		// Token: 0x04003DB2 RID: 15794
		Locked,
		// Token: 0x04003DB3 RID: 15795
		MissingTags,
		// Token: 0x04003DB4 RID: 15796
		Hidden,
		// Token: 0x04003DB5 RID: 15797
		NotLatest,
		// Token: 0x04003DB6 RID: 15798
		Real,
		// Token: 0x04003DB7 RID: 15799
		NotReal
	}

	// Token: 0x02000FD0 RID: 4048
	[CompilerGenerated]
	private sealed class <InstantiateControl>c__AnonStorey0
	{
		// Token: 0x0600754F RID: 30031 RVA: 0x001B6F3D File Offset: 0x001B533D
		public <InstantiateControl>c__AnonStorey0()
		{
		}

		// Token: 0x04006964 RID: 26980
		internal DAZDynamicItem dynamicItem;

		// Token: 0x04006965 RID: 26981
		internal GenerateDAZDynamicSelectorUI $this;
	}

	// Token: 0x02000FD1 RID: 4049
	[CompilerGenerated]
	private sealed class <InstantiateControl>c__AnonStorey1
	{
		// Token: 0x06007550 RID: 30032 RVA: 0x001B6F45 File Offset: 0x001B5345
		public <InstantiateControl>c__AnonStorey1()
		{
		}

		// Token: 0x06007551 RID: 30033 RVA: 0x001B6F4D File Offset: 0x001B534D
		internal void <>m__0(bool arg0)
		{
			if (this.<>f__ref$0.$this.characterSelector != null)
			{
				this.<>f__ref$0.$this.characterSelector.SetActiveDynamicItem(this.<>f__ref$0.dynamicItem, arg0, false);
			}
		}

		// Token: 0x06007552 RID: 30034 RVA: 0x001B6F8C File Offset: 0x001B538C
		internal void <>m__1()
		{
			SuperController.singleton.OpenPackageInManager(this.<>f__ref$0.dynamicItem.uid);
		}

		// Token: 0x06007553 RID: 30035 RVA: 0x001B6FA8 File Offset: 0x001B53A8
		internal void <>m__2()
		{
			this.<>f__ref$0.dynamicItem.OpenUI();
		}

		// Token: 0x06007554 RID: 30036 RVA: 0x001B6FBA File Offset: 0x001B53BA
		internal void <>m__3(Texture2D tex)
		{
			if (this.ddiui.rawImage != null)
			{
				this.ddiui.rawImage.texture = tex;
			}
		}

		// Token: 0x06007555 RID: 30037 RVA: 0x001B6FE3 File Offset: 0x001B53E3
		internal void <>m__4()
		{
			this.<>f__ref$0.$this.UserPrefSync(this.<>f__ref$0.dynamicItem);
		}

		// Token: 0x04006966 RID: 26982
		internal DAZDynamicItemUI ddiui;

		// Token: 0x04006967 RID: 26983
		internal GenerateDAZDynamicSelectorUI.<InstantiateControl>c__AnonStorey0 <>f__ref$0;
	}

	// Token: 0x02000FD2 RID: 4050
	[CompilerGenerated]
	private sealed class <RefreshThumbnails>c__AnonStorey2
	{
		// Token: 0x06007556 RID: 30038 RVA: 0x001B7000 File Offset: 0x001B5400
		public <RefreshThumbnails>c__AnonStorey2()
		{
		}

		// Token: 0x06007557 RID: 30039 RVA: 0x001B7008 File Offset: 0x001B5408
		internal void <>m__0(Texture2D tex)
		{
			this.ddiui.rawImage.texture = tex;
		}

		// Token: 0x04006968 RID: 26984
		internal DAZDynamicItemUI ddiui;
	}

	// Token: 0x02000FD3 RID: 4051
	[CompilerGenerated]
	private sealed class <CreateTagToggle>c__AnonStorey3
	{
		// Token: 0x06007558 RID: 30040 RVA: 0x001B701B File Offset: 0x001B541B
		public <CreateTagToggle>c__AnonStorey3()
		{
		}

		// Token: 0x06007559 RID: 30041 RVA: 0x001B7023 File Offset: 0x001B5423
		internal void <>m__0(bool b)
		{
			this.$this.SyncFilterTagFromToggle(this.tag, b);
		}

		// Token: 0x04006969 RID: 26985
		internal string tag;

		// Token: 0x0400696A RID: 26986
		internal GenerateDAZDynamicSelectorUI $this;
	}
}
