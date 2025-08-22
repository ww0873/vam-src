using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000B28 RID: 2856
public class GenerateDAZMorphsControlUI : GenerateTabbedUI
{
	// Token: 0x06004DFD RID: 19965 RVA: 0x001B73F2 File Offset: 0x001B57F2
	public GenerateDAZMorphsControlUI()
	{
	}

	// Token: 0x06004DFE RID: 19966 RVA: 0x001B7401 File Offset: 0x001B5801
	public override void TabChange(string name, bool on)
	{
		this.UnregisterCurrentDisplayedMorphsUI();
		this.currentDisplayedMorphs = new List<DAZMorph>();
		base.TabChange(name, on);
	}

	// Token: 0x06004DFF RID: 19967 RVA: 0x001B741C File Offset: 0x001B581C
	protected override Transform InstantiateControl(Transform parent, int index)
	{
		Transform transform = base.InstantiateControl(parent, index);
		DAZMorph dazmorph = this.filteredMorphs[index];
		this.currentDisplayedMorphs.Add(dazmorph);
		if (this.isAltUI)
		{
			dazmorph.InitUIAlt(transform);
		}
		else
		{
			dazmorph.InitUI(transform);
		}
		return transform;
	}

	// Token: 0x06004E00 RID: 19968 RVA: 0x001B746A File Offset: 0x001B586A
	protected void SetOnlyShowFavorites(bool b)
	{
		this.onlyShowFavorites = b;
	}

	// Token: 0x17000B17 RID: 2839
	// (get) Token: 0x06004E01 RID: 19969 RVA: 0x001B7473 File Offset: 0x001B5873
	// (set) Token: 0x06004E02 RID: 19970 RVA: 0x001B747C File Offset: 0x001B587C
	public bool onlyShowFavorites
	{
		get
		{
			return this._onlyShowFavorites;
		}
		set
		{
			if (this._onlyShowFavorites != value)
			{
				this._onlyShowFavorites = value;
				if (this.onlyShowFavoritesToggle != null)
				{
					this.onlyShowFavoritesToggle.isOn = this._onlyShowFavorites;
				}
				if (this.currentCategory != null)
				{
					this.SetCategoryFromString(this.currentCategory);
				}
			}
		}
	}

	// Token: 0x06004E03 RID: 19971 RVA: 0x001B74D5 File Offset: 0x001B58D5
	protected void SetOnlyShowActive(bool b)
	{
		this.onlyShowActive = b;
	}

	// Token: 0x17000B18 RID: 2840
	// (get) Token: 0x06004E04 RID: 19972 RVA: 0x001B74DE File Offset: 0x001B58DE
	// (set) Token: 0x06004E05 RID: 19973 RVA: 0x001B74E8 File Offset: 0x001B58E8
	public bool onlyShowActive
	{
		get
		{
			return this._onlyShowActive;
		}
		set
		{
			if (this._onlyShowActive != value)
			{
				this._onlyShowActive = value;
				if (this.onlyShowActiveToggle != null)
				{
					this.onlyShowActiveToggle.isOn = this._onlyShowActive;
				}
				if (this.currentCategory != null)
				{
					this.SetCategoryFromString(this.currentCategory);
				}
			}
		}
	}

	// Token: 0x06004E06 RID: 19974 RVA: 0x001B7541 File Offset: 0x001B5941
	protected void SetOnlyShowLatest(bool b)
	{
		this.onlyShowLatest = b;
	}

	// Token: 0x17000B19 RID: 2841
	// (get) Token: 0x06004E07 RID: 19975 RVA: 0x001B754A File Offset: 0x001B594A
	// (set) Token: 0x06004E08 RID: 19976 RVA: 0x001B7554 File Offset: 0x001B5954
	public bool onlyShowLatest
	{
		get
		{
			return this._onlyShowLatest;
		}
		set
		{
			if (this._onlyShowLatest != value)
			{
				this._onlyShowLatest = value;
				if (this.onlyShowLatestToggle != null)
				{
					this.onlyShowLatestToggle.isOn = this._onlyShowLatest;
				}
				if (this.currentCategory != null)
				{
					this.SetCategoryFromString(this.currentCategory);
				}
			}
		}
	}

	// Token: 0x06004E09 RID: 19977 RVA: 0x001B75AD File Offset: 0x001B59AD
	public void SetFilter(string f)
	{
		this.filter = f;
	}

	// Token: 0x17000B1A RID: 2842
	// (get) Token: 0x06004E0A RID: 19978 RVA: 0x001B75B6 File Offset: 0x001B59B6
	// (set) Token: 0x06004E0B RID: 19979 RVA: 0x001B75C0 File Offset: 0x001B59C0
	public string filter
	{
		get
		{
			return this._filter;
		}
		set
		{
			if (this._filter != value)
			{
				this._filter = value;
				if (this.filterField != null)
				{
					this.filterField.text = this._filter;
				}
				this._lowerCaseFilter = this._filter.ToLower();
				if (this.currentCategory != null)
				{
					this.SetCategoryFromString(this.currentCategory);
				}
			}
		}
	}

	// Token: 0x06004E0C RID: 19980 RVA: 0x001B7630 File Offset: 0x001B5A30
	protected void SetShowType(string t)
	{
		try
		{
			GenerateDAZMorphsControlUI.ShowType showType = (GenerateDAZMorphsControlUI.ShowType)Enum.Parse(typeof(GenerateDAZMorphsControlUI.ShowType), t);
			this.showType = showType;
		}
		catch (ArgumentException)
		{
			UnityEngine.Debug.LogError("Tried to set show type to " + t + " which is not a valid show type");
		}
	}

	// Token: 0x17000B1B RID: 2843
	// (get) Token: 0x06004E0D RID: 19981 RVA: 0x001B768C File Offset: 0x001B5A8C
	// (set) Token: 0x06004E0E RID: 19982 RVA: 0x001B7694 File Offset: 0x001B5A94
	public GenerateDAZMorphsControlUI.ShowType showType
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
				if (this.showTypePopup != null)
				{
					this.showTypePopup.currentValueNoCallback = this._showType.ToString();
				}
				if (this.currentCategory != null)
				{
					this.SetCategoryFromString(this.currentCategory);
				}
			}
		}
	}

	// Token: 0x06004E0F RID: 19983 RVA: 0x001B76F8 File Offset: 0x001B5AF8
	protected void SetSecondaryShowType(string t)
	{
		try
		{
			GenerateDAZMorphsControlUI.SecondaryShowType secondaryShowType = (GenerateDAZMorphsControlUI.SecondaryShowType)Enum.Parse(typeof(GenerateDAZMorphsControlUI.SecondaryShowType), t);
			this.secondaryShowType = secondaryShowType;
		}
		catch (ArgumentException)
		{
			UnityEngine.Debug.LogError("Tried to set secondary show type to " + t + " which is not a valid secondary show type");
		}
	}

	// Token: 0x17000B1C RID: 2844
	// (get) Token: 0x06004E10 RID: 19984 RVA: 0x001B7754 File Offset: 0x001B5B54
	// (set) Token: 0x06004E11 RID: 19985 RVA: 0x001B775C File Offset: 0x001B5B5C
	public GenerateDAZMorphsControlUI.SecondaryShowType secondaryShowType
	{
		get
		{
			return this._secondaryShowType;
		}
		set
		{
			if (this._secondaryShowType != value)
			{
				this._secondaryShowType = value;
				if (this.secondaryShowTypePopup != null)
				{
					this.secondaryShowTypePopup.currentValueNoCallback = this._secondaryShowType.ToString();
				}
				if (this.currentCategory != null)
				{
					this.SetCategoryFromString(this.currentCategory);
				}
			}
		}
	}

	// Token: 0x06004E12 RID: 19986 RVA: 0x001B77C0 File Offset: 0x001B5BC0
	public List<DAZMorph> GetMorphs()
	{
		return this.morphs;
	}

	// Token: 0x06004E13 RID: 19987 RVA: 0x001B77C8 File Offset: 0x001B5BC8
	public void SetCategoryFromString(string val)
	{
		this._forceCategoryRefresh = false;
		this.currentCategory = val;
		this.GenerateStart();
		if (this.controlUIPrefab != null && this.tabUIPrefab != null && this.tabButtonUIPrefab != null)
		{
			List<DAZMorph> list = null;
			this.filteredMorphs = new List<DAZMorph>();
			if (this.currentCategory == "All")
			{
				list = this.morphs;
			}
			else
			{
				this.categoryToMorphs.TryGetValue(this.currentCategory, out list);
			}
			if (list != null)
			{
				foreach (DAZMorph dazmorph in list)
				{
					switch (this._secondaryShowType)
					{
					case GenerateDAZMorphsControlUI.SecondaryShowType.BuiltIn:
						if (dazmorph.isRuntime)
						{
							continue;
						}
						break;
					case GenerateDAZMorphsControlUI.SecondaryShowType.CustomAll:
						if (!dazmorph.isRuntime || dazmorph.isTransient)
						{
							continue;
						}
						break;
					case GenerateDAZMorphsControlUI.SecondaryShowType.CustomPackage:
						if (!dazmorph.isRuntime || dazmorph.isTransient || !dazmorph.isInPackage)
						{
							continue;
						}
						break;
					case GenerateDAZMorphsControlUI.SecondaryShowType.CustomLocal:
						if (!dazmorph.isRuntime || dazmorph.isTransient || dazmorph.isInPackage)
						{
							continue;
						}
						break;
					case GenerateDAZMorphsControlUI.SecondaryShowType.Transient:
						if (!dazmorph.isTransient)
						{
							continue;
						}
						break;
					case GenerateDAZMorphsControlUI.SecondaryShowType.Formula:
						if (!dazmorph.hasMorphValueFormulas && !dazmorph.hasMCMFormulas)
						{
							continue;
						}
						break;
					}
					if (!this._onlyShowFavorites || dazmorph.favorite)
					{
						if (!this._onlyShowActive || dazmorph.active)
						{
							if (!this._onlyShowLatest || dazmorph.isLatestVersion || dazmorph.active)
							{
								if (this._lowerCaseFilter == null || dazmorph.lowerCaseResolvedDisplayName.Contains(this._lowerCaseFilter))
								{
									GenerateDAZMorphsControlUI.ShowType showType = this._showType;
									if (showType != GenerateDAZMorphsControlUI.ShowType.Morph)
									{
										if (showType == GenerateDAZMorphsControlUI.ShowType.Pose)
										{
											if (!dazmorph.isPoseControl)
											{
												continue;
											}
										}
									}
									else if (dazmorph.isPoseControl)
									{
										continue;
									}
									this.filteredMorphs.Add(dazmorph);
								}
							}
						}
					}
				}
				for (int i = 0; i < this.filteredMorphs.Count; i++)
				{
					base.AllocateControl();
				}
			}
		}
		this.GenerateFinish();
	}

	// Token: 0x17000B1D RID: 2845
	// (get) Token: 0x06004E14 RID: 19988 RVA: 0x001B7AAC File Offset: 0x001B5EAC
	// (set) Token: 0x06004E15 RID: 19989 RVA: 0x001B7AB4 File Offset: 0x001B5EB4
	public DAZMorphBank morphBank1
	{
		get
		{
			return this._morphBank1;
		}
		set
		{
			if (this._morphBank1 != value)
			{
				this._morphBank1 = value;
				this.ResyncMorphs();
				this.ResyncUI();
			}
		}
	}

	// Token: 0x17000B1E RID: 2846
	// (get) Token: 0x06004E16 RID: 19990 RVA: 0x001B7ADA File Offset: 0x001B5EDA
	// (set) Token: 0x06004E17 RID: 19991 RVA: 0x001B7AE2 File Offset: 0x001B5EE2
	public DAZMorphBank morphBank2
	{
		get
		{
			return this._morphBank2;
		}
		set
		{
			if (this._morphBank2 != value)
			{
				this._morphBank2 = value;
				this.ResyncMorphs();
				this.ResyncUI();
			}
		}
	}

	// Token: 0x17000B1F RID: 2847
	// (get) Token: 0x06004E18 RID: 19992 RVA: 0x001B7B08 File Offset: 0x001B5F08
	// (set) Token: 0x06004E19 RID: 19993 RVA: 0x001B7B10 File Offset: 0x001B5F10
	public DAZMorphBank morphBank3
	{
		get
		{
			return this._morphBank3;
		}
		set
		{
			if (this._morphBank3 != value)
			{
				this._morphBank3 = value;
				this.ResyncMorphs();
				this.ResyncUI();
			}
		}
	}

	// Token: 0x06004E1A RID: 19994 RVA: 0x001B7B36 File Offset: 0x001B5F36
	public static int CustomMorphSort(DAZMorph m1, DAZMorph m2)
	{
		return string.Compare(m1.resolvedRegionName + ":" + m1.resolvedDisplayName, m2.resolvedRegionName + ":" + m2.resolvedDisplayName);
	}

	// Token: 0x06004E1B RID: 19995 RVA: 0x001B7B69 File Offset: 0x001B5F69
	public List<string> GetMorphDisplayNames()
	{
		if (this.morphDisplayNameToMorph == null)
		{
			this.ResyncMorphs();
		}
		return new List<string>(this.morphDisplayNameToMorph.Keys);
	}

	// Token: 0x06004E1C RID: 19996 RVA: 0x001B7B8C File Offset: 0x001B5F8C
	public DAZMorph GetMorphByDisplayName(string morphDisplayName)
	{
		if (this.morphDisplayNameToMorph == null)
		{
			this.ResyncMorphs();
		}
		DAZMorph dazmorph = null;
		if (this.morphDisplayNameToMorph != null && !this.morphDisplayNameToMorph.TryGetValue(morphDisplayName, out dazmorph))
		{
			if (this._morphBank1 != null)
			{
				dazmorph = this._morphBank1.GetMorphByDisplayName(morphDisplayName);
			}
			if (dazmorph == null && this._morphBank2 != null)
			{
				dazmorph = this._morphBank2.GetMorphByDisplayName(morphDisplayName);
			}
			if (dazmorph == null && this._morphBank3 != null)
			{
				dazmorph = this._morphBank3.GetMorphByDisplayName(morphDisplayName);
			}
		}
		return dazmorph;
	}

	// Token: 0x06004E1D RID: 19997 RVA: 0x001B7C34 File Offset: 0x001B6034
	public bool IsBadMorph(string morphDisplayName)
	{
		return (this._morphBank1 != null && this._morphBank1.IsBadMorph(morphDisplayName)) || (this._morphBank2 != null && this._morphBank2.IsBadMorph(morphDisplayName)) || (this._morphBank3 != null && this._morphBank3.IsBadMorph(morphDisplayName));
	}

	// Token: 0x06004E1E RID: 19998 RVA: 0x001B7CAE File Offset: 0x001B60AE
	public List<string> GetMorphUids()
	{
		if (this.morphUidToMorph == null)
		{
			this.ResyncMorphs();
		}
		return new List<string>(this.morphUidToMorph.Keys);
	}

	// Token: 0x06004E1F RID: 19999 RVA: 0x001B7CD4 File Offset: 0x001B60D4
	public DAZMorph GetMorphByUid(string morphUid)
	{
		if (this.morphUidToMorph == null)
		{
			this.ResyncMorphs();
		}
		DAZMorph dazmorph = null;
		if (this.morphUidToMorph != null && !this.morphUidToMorph.TryGetValue(morphUid, out dazmorph))
		{
			if (this._morphBank1 != null)
			{
				dazmorph = this._morphBank1.GetMorphByUid(morphUid);
			}
			if (dazmorph == null && this._morphBank2 != null)
			{
				dazmorph = this._morphBank2.GetMorphByUid(morphUid);
			}
			if (dazmorph == null && this._morphBank3 != null)
			{
				dazmorph = this._morphBank3.GetMorphByUid(morphUid);
			}
		}
		return dazmorph;
	}

	// Token: 0x06004E20 RID: 20000 RVA: 0x001B7D7C File Offset: 0x001B617C
	public bool CleanDemandActivatedMorphs()
	{
		bool flag = false;
		if (this._morphBank1 != null && this._morphBank1.CleanDemandActivatedMorphs())
		{
			flag = true;
		}
		if (this._morphBank2 != null && this._morphBank2.CleanDemandActivatedMorphs())
		{
			flag = true;
		}
		if (this._morphBank3 != null && this._morphBank3.CleanDemandActivatedMorphs())
		{
			flag = true;
		}
		if (flag)
		{
			this.ResyncMorphs();
			if (this.currentCategory != null)
			{
				this.SetCategoryFromString(this.currentCategory);
			}
		}
		return flag;
	}

	// Token: 0x06004E21 RID: 20001 RVA: 0x001B7E18 File Offset: 0x001B6218
	protected void UnregisterCurrentDisplayedMorphsUI()
	{
		if (this.currentDisplayedMorphs != null)
		{
			foreach (DAZMorph dazmorph in this.currentDisplayedMorphs)
			{
				if (this.isAltUI)
				{
					dazmorph.DeregisterUIAlt();
				}
				else
				{
					dazmorph.DeregisterUI();
				}
			}
		}
	}

	// Token: 0x06004E22 RID: 20002 RVA: 0x001B7E94 File Offset: 0x001B6294
	public void ResyncMorphs()
	{
		List<DAZMorph> list = new List<DAZMorph>();
		if (this._morphBank1 != null)
		{
			foreach (DAZMorph dazmorph in this._morphBank1.morphs)
			{
				if (dazmorph.visible)
				{
					list.Add(dazmorph);
				}
			}
		}
		if (this._morphBank2 != null)
		{
			foreach (DAZMorph dazmorph2 in this._morphBank2.morphs)
			{
				if (dazmorph2.visible)
				{
					list.Add(dazmorph2);
				}
			}
		}
		if (this._morphBank3 != null)
		{
			foreach (DAZMorph dazmorph3 in this._morphBank3.morphs)
			{
				if (dazmorph3.visible)
				{
					list.Add(dazmorph3);
				}
			}
		}
		List<DAZMorph> list2 = list;
		if (GenerateDAZMorphsControlUI.<>f__mg$cache0 == null)
		{
			GenerateDAZMorphsControlUI.<>f__mg$cache0 = new Comparison<DAZMorph>(GenerateDAZMorphsControlUI.CustomMorphSort);
		}
		list2.Sort(GenerateDAZMorphsControlUI.<>f__mg$cache0);
		if (this.morphDisplayNameToMorph == null)
		{
			this.morphDisplayNameToMorph = new Dictionary<string, DAZMorph>();
		}
		else
		{
			this.morphDisplayNameToMorph.Clear();
		}
		if (this.morphUidToMorph == null)
		{
			this.morphUidToMorph = new Dictionary<string, DAZMorph>();
		}
		else
		{
			this.morphUidToMorph.Clear();
		}
		if (this.morphs == null)
		{
			this.morphs = new List<DAZMorph>();
		}
		else
		{
			this.morphs.Clear();
		}
		foreach (DAZMorph dazmorph4 in list)
		{
			if (dazmorph4.resolvedDisplayName != null)
			{
				if (!this.morphDisplayNameToMorph.ContainsKey(dazmorph4.resolvedDisplayName))
				{
					this.morphDisplayNameToMorph.Add(dazmorph4.resolvedDisplayName, dazmorph4);
				}
			}
			else
			{
				UnityEngine.Debug.LogError("Morph display name is null for morph with morph name " + dazmorph4.morphName);
			}
			if (!this.morphUidToMorph.ContainsKey(dazmorph4.uid))
			{
				this.morphs.Add(dazmorph4);
				this.morphUidToMorph.Add(dazmorph4.uid, dazmorph4);
			}
			else
			{
				UnityEngine.Debug.LogError("Found duplicate morph uid " + dazmorph4.uid + "for morph " + dazmorph4.morphName);
			}
		}
		if (this.currentDisplayedMorphs == null)
		{
			this.currentDisplayedMorphs = new List<DAZMorph>();
		}
		else
		{
			this.currentDisplayedMorphs.Clear();
		}
		if (this.categoryToMorphs == null)
		{
			this.categoryToMorphs = new Dictionary<string, List<DAZMorph>>();
		}
		else
		{
			this.categoryToMorphs.Clear();
		}
		foreach (DAZMorph dazmorph5 in this.morphs)
		{
			string resolvedRegionName = dazmorph5.resolvedRegionName;
			List<DAZMorph> list3;
			if (this.categoryToMorphs.TryGetValue(resolvedRegionName, out list3))
			{
				list3.Add(dazmorph5);
			}
			else
			{
				list3 = new List<DAZMorph>();
				list3.Add(dazmorph5);
				this.categoryToMorphs.Add(resolvedRegionName, list3);
			}
		}
		this.morphCategories = new string[this.categoryToMorphs.Keys.Count + 1];
		this.morphCategories[0] = "All";
		this.categoryToMorphs.Keys.CopyTo(this.morphCategories, 1);
		Array.Sort<string>(this.morphCategories, 1, this.morphCategories.Length - 1);
		if (this.categoryPopup != null)
		{
			this.categoryPopup.numPopupValues = this.morphCategories.Length;
			for (int i = 0; i < this.morphCategories.Length; i++)
			{
				this.categoryPopup.setPopupValue(i, this.morphCategories[i]);
			}
		}
	}

	// Token: 0x06004E23 RID: 20003 RVA: 0x001B830C File Offset: 0x001B670C
	protected IEnumerator DelayedCategoryRefresh()
	{
		yield return null;
		yield return null;
		yield return null;
		if (this.currentCategory != null && (this._onlyShowActive || this._forceCategoryRefresh))
		{
			this.SetCategoryFromString(this.currentCategory);
		}
		yield break;
	}

	// Token: 0x06004E24 RID: 20004 RVA: 0x001B8328 File Offset: 0x001B6728
	public void DoZeroCommand(string command)
	{
		if (this.ZeroPopup != null)
		{
			this.ZeroPopup.currentValueNoCallback = "Zero...";
		}
		if (command != null)
		{
			if (!(command == "ZeroAll"))
			{
				if (!(command == "ZeroMorph"))
				{
					if (!(command == "ZeroPose"))
					{
						if (!(command == "ZeroShown"))
						{
							if (!(command == "ZeroNearZero"))
							{
								if (command == "ZeroNearZeroMore")
								{
									this.ZeroNearZeroMore();
								}
							}
							else
							{
								this.ZeroNearZero();
							}
						}
						else
						{
							this.ZeroShown();
						}
					}
					else
					{
						this.ZeroPose();
					}
				}
				else
				{
					this.ZeroMorph();
				}
			}
			else
			{
				this.ZeroAll();
			}
		}
	}

	// Token: 0x06004E25 RID: 20005 RVA: 0x001B8404 File Offset: 0x001B6804
	public void ZeroAll()
	{
		foreach (DAZMorph dazmorph in this.morphs)
		{
			dazmorph.morphValue = 0f;
		}
		base.StartCoroutine(this.DelayedCategoryRefresh());
	}

	// Token: 0x06004E26 RID: 20006 RVA: 0x001B8474 File Offset: 0x001B6874
	public void ZeroMorph()
	{
		foreach (DAZMorph dazmorph in this.morphs)
		{
			if (!dazmorph.isPoseControl)
			{
				dazmorph.morphValue = 0f;
			}
		}
		base.StartCoroutine(this.DelayedCategoryRefresh());
	}

	// Token: 0x06004E27 RID: 20007 RVA: 0x001B84EC File Offset: 0x001B68EC
	public void ZeroPose()
	{
		foreach (DAZMorph dazmorph in this.morphs)
		{
			if (dazmorph.isPoseControl)
			{
				dazmorph.morphValue = 0f;
			}
		}
		base.StartCoroutine(this.DelayedCategoryRefresh());
	}

	// Token: 0x06004E28 RID: 20008 RVA: 0x001B8564 File Offset: 0x001B6964
	public void ZeroShown()
	{
		foreach (DAZMorph dazmorph in this.filteredMorphs)
		{
			dazmorph.morphValue = 0f;
		}
		base.StartCoroutine(this.DelayedCategoryRefresh());
	}

	// Token: 0x06004E29 RID: 20009 RVA: 0x001B85D4 File Offset: 0x001B69D4
	public void ZeroNearZero()
	{
		foreach (DAZMorph dazmorph in this.morphs)
		{
			if (Mathf.Abs(dazmorph.morphValue) <= 0.01f)
			{
				dazmorph.morphValue = 0f;
			}
		}
		base.StartCoroutine(this.DelayedCategoryRefresh());
	}

	// Token: 0x06004E2A RID: 20010 RVA: 0x001B8658 File Offset: 0x001B6A58
	public void ZeroNearZeroMore()
	{
		foreach (DAZMorph dazmorph in this.morphs)
		{
			if (Mathf.Abs(dazmorph.morphValue) <= 0.05f)
			{
				dazmorph.morphValue = 0f;
			}
		}
		base.StartCoroutine(this.DelayedCategoryRefresh());
	}

	// Token: 0x06004E2B RID: 20011 RVA: 0x001B86DC File Offset: 0x001B6ADC
	public void DoDefaultCommand(string command)
	{
		if (this.DefaultPopup != null)
		{
			this.DefaultPopup.currentValueNoCallback = "Default...";
		}
		if (command != null)
		{
			if (!(command == "DefaultAll"))
			{
				if (!(command == "DefaultMorph"))
				{
					if (!(command == "DefaultPose"))
					{
						if (command == "DefaultShown")
						{
							this.DefaultShown();
						}
					}
					else
					{
						this.DefaultPose();
					}
				}
				else
				{
					this.DefaultMorph();
				}
			}
			else
			{
				this.DefaultAll();
			}
		}
	}

	// Token: 0x06004E2C RID: 20012 RVA: 0x001B8784 File Offset: 0x001B6B84
	public void DefaultAll()
	{
		foreach (DAZMorph dazmorph in this.morphs)
		{
			dazmorph.morphValue = dazmorph.startValue;
		}
		base.StartCoroutine(this.DelayedCategoryRefresh());
	}

	// Token: 0x06004E2D RID: 20013 RVA: 0x001B87F4 File Offset: 0x001B6BF4
	public void DefaultMorph()
	{
		foreach (DAZMorph dazmorph in this.morphs)
		{
			if (!dazmorph.isPoseControl)
			{
				dazmorph.morphValue = dazmorph.startValue;
			}
		}
		base.StartCoroutine(this.DelayedCategoryRefresh());
	}

	// Token: 0x06004E2E RID: 20014 RVA: 0x001B8870 File Offset: 0x001B6C70
	public void DefaultPose()
	{
		foreach (DAZMorph dazmorph in this.morphs)
		{
			if (dazmorph.isPoseControl)
			{
				dazmorph.morphValue = dazmorph.startValue;
			}
		}
		base.StartCoroutine(this.DelayedCategoryRefresh());
	}

	// Token: 0x06004E2F RID: 20015 RVA: 0x001B88EC File Offset: 0x001B6CEC
	public void DefaultShown()
	{
		foreach (DAZMorph dazmorph in this.filteredMorphs)
		{
			dazmorph.morphValue = dazmorph.startValue;
		}
		base.StartCoroutine(this.DelayedCategoryRefresh());
	}

	// Token: 0x06004E30 RID: 20016 RVA: 0x001B895C File Offset: 0x001B6D5C
	public void ForceCategoryRefresh()
	{
		this._forceCategoryRefresh = true;
		if (base.gameObject.activeInHierarchy)
		{
			base.StartCoroutine(this.DelayedCategoryRefresh());
		}
	}

	// Token: 0x06004E31 RID: 20017 RVA: 0x001B8982 File Offset: 0x001B6D82
	public void ResyncUI()
	{
		this.TabChange(this.currentTab.ToString(), true);
	}

	// Token: 0x06004E32 RID: 20018 RVA: 0x001B899C File Offset: 0x001B6D9C
	protected override void Generate()
	{
		base.Generate();
		this.ResyncMorphs();
		if (this.categoryPopup != null)
		{
			this.categoryPopup.numPopupValues = this.morphCategories.Length;
			for (int i = 0; i < this.morphCategories.Length; i++)
			{
				this.categoryPopup.setPopupValue(i, this.morphCategories[i]);
			}
			UIPopup uipopup = this.categoryPopup;
			uipopup.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetCategoryFromString));
			this.categoryPopup.currentValue = this.morphCategories[0];
		}
		else if (this.controlUIPrefab != null && this.tabUIPrefab != null && this.tabButtonUIPrefab != null)
		{
			this.filteredMorphs = new List<DAZMorph>(this.morphs);
			for (int j = 0; j < this.filteredMorphs.Count; j++)
			{
				base.AllocateControl();
			}
		}
	}

	// Token: 0x06004E33 RID: 20019 RVA: 0x001B8AAC File Offset: 0x001B6EAC
	private void Awake()
	{
		if (this.onlyShowActiveToggle != null)
		{
			this.onlyShowActiveToggle.onValueChanged.AddListener(new UnityAction<bool>(this.SetOnlyShowActive));
		}
		if (this.onlyShowLatestToggle != null)
		{
			this.onlyShowLatestToggle.isOn = this._onlyShowLatest;
			this.onlyShowLatestToggle.onValueChanged.AddListener(new UnityAction<bool>(this.SetOnlyShowLatest));
		}
		if (this.onlyShowFavoritesToggle != null)
		{
			this.onlyShowFavoritesToggle.onValueChanged.AddListener(new UnityAction<bool>(this.SetOnlyShowFavorites));
		}
		if (this.filterField != null)
		{
			this.filterField.onValueChanged.AddListener(new UnityAction<string>(this.SetFilter));
		}
		if (this.showTypePopup != null)
		{
			this.showTypePopup.currentValue = this._showType.ToString();
			UIPopup uipopup = this.showTypePopup;
			uipopup.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetShowType));
		}
		if (this.secondaryShowTypePopup != null)
		{
			this.secondaryShowTypePopup.currentValue = this._secondaryShowType.ToString();
			UIPopup uipopup2 = this.secondaryShowTypePopup;
			uipopup2.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup2.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetSecondaryShowType));
		}
		if (this.ZeroPopup != null)
		{
			UIPopup zeroPopup = this.ZeroPopup;
			zeroPopup.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(zeroPopup.onValueChangeHandlers, new UIPopup.OnValueChange(this.DoZeroCommand));
		}
		if (this.DefaultPopup != null)
		{
			UIPopup defaultPopup = this.DefaultPopup;
			defaultPopup.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(defaultPopup.onValueChangeHandlers, new UIPopup.OnValueChange(this.DoDefaultCommand));
		}
	}

	// Token: 0x06004E34 RID: 20020 RVA: 0x001B8C96 File Offset: 0x001B7096
	private void OnEnable()
	{
		base.StartCoroutine(this.DelayedCategoryRefresh());
	}

	// Token: 0x04003DBC RID: 15804
	public Toggle onlyShowFavoritesToggle;

	// Token: 0x04003DBD RID: 15805
	protected bool _onlyShowFavorites;

	// Token: 0x04003DBE RID: 15806
	public Toggle onlyShowActiveToggle;

	// Token: 0x04003DBF RID: 15807
	protected bool _onlyShowActive;

	// Token: 0x04003DC0 RID: 15808
	public Toggle onlyShowLatestToggle;

	// Token: 0x04003DC1 RID: 15809
	protected bool _onlyShowLatest = true;

	// Token: 0x04003DC2 RID: 15810
	public InputField filterField;

	// Token: 0x04003DC3 RID: 15811
	protected string _lowerCaseFilter;

	// Token: 0x04003DC4 RID: 15812
	protected string _filter;

	// Token: 0x04003DC5 RID: 15813
	public UIPopup showTypePopup;

	// Token: 0x04003DC6 RID: 15814
	protected GenerateDAZMorphsControlUI.ShowType _showType;

	// Token: 0x04003DC7 RID: 15815
	public UIPopup secondaryShowTypePopup;

	// Token: 0x04003DC8 RID: 15816
	protected GenerateDAZMorphsControlUI.SecondaryShowType _secondaryShowType;

	// Token: 0x04003DC9 RID: 15817
	protected List<DAZMorph> morphs;

	// Token: 0x04003DCA RID: 15818
	protected Dictionary<string, DAZMorph> morphDisplayNameToMorph;

	// Token: 0x04003DCB RID: 15819
	protected Dictionary<string, DAZMorph> morphUidToMorph;

	// Token: 0x04003DCC RID: 15820
	protected List<DAZMorph> filteredMorphs;

	// Token: 0x04003DCD RID: 15821
	protected Dictionary<string, List<DAZMorph>> categoryToMorphs;

	// Token: 0x04003DCE RID: 15822
	protected string[] morphCategories;

	// Token: 0x04003DCF RID: 15823
	protected List<DAZMorph> currentDisplayedMorphs;

	// Token: 0x04003DD0 RID: 15824
	public UIPopup categoryPopup;

	// Token: 0x04003DD1 RID: 15825
	public bool isAltUI;

	// Token: 0x04003DD2 RID: 15826
	protected string currentCategory;

	// Token: 0x04003DD3 RID: 15827
	[SerializeField]
	protected DAZMorphBank _morphBank1;

	// Token: 0x04003DD4 RID: 15828
	[SerializeField]
	protected DAZMorphBank _morphBank2;

	// Token: 0x04003DD5 RID: 15829
	[SerializeField]
	protected DAZMorphBank _morphBank3;

	// Token: 0x04003DD6 RID: 15830
	public UIPopup ZeroPopup;

	// Token: 0x04003DD7 RID: 15831
	public UIPopup DefaultPopup;

	// Token: 0x04003DD8 RID: 15832
	protected bool _forceCategoryRefresh;

	// Token: 0x04003DD9 RID: 15833
	[CompilerGenerated]
	private static Comparison<DAZMorph> <>f__mg$cache0;

	// Token: 0x02000B29 RID: 2857
	public enum ShowType
	{
		// Token: 0x04003DDB RID: 15835
		MorphAndPose,
		// Token: 0x04003DDC RID: 15836
		Morph,
		// Token: 0x04003DDD RID: 15837
		Pose
	}

	// Token: 0x02000B2A RID: 2858
	public enum SecondaryShowType
	{
		// Token: 0x04003DDF RID: 15839
		All,
		// Token: 0x04003DE0 RID: 15840
		BuiltIn,
		// Token: 0x04003DE1 RID: 15841
		CustomAll,
		// Token: 0x04003DE2 RID: 15842
		CustomPackage,
		// Token: 0x04003DE3 RID: 15843
		CustomLocal,
		// Token: 0x04003DE4 RID: 15844
		Transient,
		// Token: 0x04003DE5 RID: 15845
		Formula
	}

	// Token: 0x02000FD4 RID: 4052
	[CompilerGenerated]
	private sealed class <DelayedCategoryRefresh>c__Iterator0 : IEnumerator, IDisposable, IEnumerator<object>
	{
		// Token: 0x0600755A RID: 30042 RVA: 0x001B8CA5 File Offset: 0x001B70A5
		[DebuggerHidden]
		public <DelayedCategoryRefresh>c__Iterator0()
		{
		}

		// Token: 0x0600755B RID: 30043 RVA: 0x001B8CB0 File Offset: 0x001B70B0
		public bool MoveNext()
		{
			uint num = (uint)this.$PC;
			this.$PC = -1;
			switch (num)
			{
			case 0U:
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 1;
				}
				return true;
			case 1U:
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 2;
				}
				return true;
			case 2U:
				this.$current = null;
				if (!this.$disposing)
				{
					this.$PC = 3;
				}
				return true;
			case 3U:
				if (this.currentCategory != null && (this._onlyShowActive || this._forceCategoryRefresh))
				{
					base.SetCategoryFromString(this.currentCategory);
				}
				this.$PC = -1;
				break;
			}
			return false;
		}

		// Token: 0x17001159 RID: 4441
		// (get) Token: 0x0600755C RID: 30044 RVA: 0x001B8D87 File Offset: 0x001B7187
		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x1700115A RID: 4442
		// (get) Token: 0x0600755D RID: 30045 RVA: 0x001B8D8F File Offset: 0x001B718F
		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return this.$current;
			}
		}

		// Token: 0x0600755E RID: 30046 RVA: 0x001B8D97 File Offset: 0x001B7197
		[DebuggerHidden]
		public void Dispose()
		{
			this.$disposing = true;
			this.$PC = -1;
		}

		// Token: 0x0600755F RID: 30047 RVA: 0x001B8DA7 File Offset: 0x001B71A7
		[DebuggerHidden]
		public void Reset()
		{
			throw new NotSupportedException();
		}

		// Token: 0x0400696B RID: 26987
		internal GenerateDAZMorphsControlUI $this;

		// Token: 0x0400696C RID: 26988
		internal object $current;

		// Token: 0x0400696D RID: 26989
		internal bool $disposing;

		// Token: 0x0400696E RID: 26990
		internal int $PC;
	}
}
