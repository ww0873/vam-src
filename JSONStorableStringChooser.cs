using System;
using System.Collections.Generic;
using SimpleJSON;

// Token: 0x02000CE8 RID: 3304
public class JSONStorableStringChooser : JSONStorableParam
{
	// Token: 0x06006413 RID: 25619 RVA: 0x0025FF38 File Offset: 0x0025E338
	public JSONStorableStringChooser(string paramName, List<string> choicesList, string startingValue, string displayName)
	{
		this.type = JSONStorable.Type.StringChooser;
		this.name = paramName;
		this._choices = choicesList;
		this.useDifferentDisplayChoices = false;
		this._displayChoices = choicesList;
		this.defaultVal = startingValue;
		this.val = startingValue;
		this.label = displayName;
		this.setCallbackFunction = null;
		this.setJSONCallbackFunction = null;
	}

	// Token: 0x06006414 RID: 25620 RVA: 0x0025FF94 File Offset: 0x0025E394
	public JSONStorableStringChooser(string paramName, List<string> choicesList, string startingValue, string displayName, JSONStorableStringChooser.SetStringCallback callback)
	{
		this.type = JSONStorable.Type.StringChooser;
		this.name = paramName;
		this._choices = choicesList;
		this.useDifferentDisplayChoices = false;
		this._displayChoices = choicesList;
		this.defaultVal = startingValue;
		this.val = startingValue;
		this.label = displayName;
		this.setCallbackFunction = callback;
		this.setJSONCallbackFunction = null;
	}

	// Token: 0x06006415 RID: 25621 RVA: 0x0025FFF0 File Offset: 0x0025E3F0
	public JSONStorableStringChooser(string paramName, List<string> choicesList, string startingValue, string displayName, JSONStorableStringChooser.SetJSONStringCallback callback)
	{
		this.type = JSONStorable.Type.StringChooser;
		this.name = paramName;
		this._choices = choicesList;
		this.useDifferentDisplayChoices = false;
		this._displayChoices = choicesList;
		this.defaultVal = startingValue;
		this.val = startingValue;
		this.label = displayName;
		this.setCallbackFunction = null;
		this.setJSONCallbackFunction = callback;
	}

	// Token: 0x06006416 RID: 25622 RVA: 0x0026004C File Offset: 0x0025E44C
	public JSONStorableStringChooser(string paramName, List<string> choicesList, List<string> displayChoicesList, string startingValue, string displayName)
	{
		this.type = JSONStorable.Type.StringChooser;
		this.name = paramName;
		this._choices = choicesList;
		this.useDifferentDisplayChoices = true;
		this._displayChoices = displayChoicesList;
		this.defaultVal = startingValue;
		this.val = startingValue;
		this.label = displayName;
		this.setCallbackFunction = null;
		this.setJSONCallbackFunction = null;
	}

	// Token: 0x06006417 RID: 25623 RVA: 0x002600A8 File Offset: 0x0025E4A8
	public JSONStorableStringChooser(string paramName, List<string> choicesList, List<string> displayChoicesList, string startingValue, string displayName, JSONStorableStringChooser.SetStringCallback callback)
	{
		this.type = JSONStorable.Type.StringChooser;
		this.name = paramName;
		this._choices = choicesList;
		this.useDifferentDisplayChoices = true;
		this._displayChoices = displayChoicesList;
		this.defaultVal = startingValue;
		this.val = startingValue;
		this.label = displayName;
		this.setCallbackFunction = callback;
		this.setJSONCallbackFunction = null;
	}

	// Token: 0x06006418 RID: 25624 RVA: 0x00260108 File Offset: 0x0025E508
	public JSONStorableStringChooser(string paramName, List<string> choicesList, List<string> displayChoicesList, string startingValue, string displayName, JSONStorableStringChooser.SetJSONStringCallback callback)
	{
		this.type = JSONStorable.Type.StringChooser;
		this.name = paramName;
		this._choices = choicesList;
		this.useDifferentDisplayChoices = true;
		this._displayChoices = displayChoicesList;
		this.defaultVal = startingValue;
		this.val = startingValue;
		this.label = displayName;
		this.setCallbackFunction = null;
		this.setJSONCallbackFunction = callback;
	}

	// Token: 0x06006419 RID: 25625 RVA: 0x00260168 File Offset: 0x0025E568
	public override bool StoreJSON(JSONClass jc, bool includePhysical = true, bool includeAppearance = true, bool forceStore = false)
	{
		bool flag = this.NeedsStore(jc, includePhysical, includeAppearance) && (forceStore || this._val != this.defaultVal);
		if (flag)
		{
			if (this.representsAtomUid && this.storable != null)
			{
				string text = this.storable.AtomUidToStoreAtomUid(this.val);
				if (text == null)
				{
					return false;
				}
				jc[this.name] = text;
			}
			else
			{
				jc[this.name] = this.val;
			}
		}
		return flag;
	}

	// Token: 0x0600641A RID: 25626 RVA: 0x00260214 File Offset: 0x0025E614
	public override void RestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, bool setMissingToDefault = true)
	{
		bool flag = this.NeedsRestore(jc, restorePhysical, restoreAppearance);
		if (flag)
		{
			if (jc[this.name] != null)
			{
				if (this.representsAtomUid && this.storable != null)
				{
					this.val = this.storable.StoredAtomUidToAtomUid(jc[this.name]);
				}
				else
				{
					this.val = jc[this.name];
				}
			}
			else if (setMissingToDefault)
			{
				this.val = this.defaultVal;
			}
		}
	}

	// Token: 0x0600641B RID: 25627 RVA: 0x002602BC File Offset: 0x0025E6BC
	public override void LateRestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, bool setMissingToDefault = true)
	{
		bool flag = this.NeedsLateRestore(jc, restorePhysical, restoreAppearance);
		if (flag)
		{
			if (jc[this.name] != null)
			{
				this.val = jc[this.name];
			}
			else if (setMissingToDefault)
			{
				this.val = this.defaultVal;
			}
		}
	}

	// Token: 0x0600641C RID: 25628 RVA: 0x00260320 File Offset: 0x0025E720
	protected void SyncPopup()
	{
		if (this._popup != null)
		{
			this._popup.useDifferentDisplayValues = true;
			if (this._choices != null)
			{
				this._popup.numPopupValues = this._choices.Count;
				for (int i = 0; i < this._choices.Count; i++)
				{
					this._popup.setPopupValue(i, this._choices[i]);
				}
			}
			else
			{
				this._popup.numPopupValues = 0;
			}
			if (this._displayChoices != null)
			{
				for (int j = 0; j < this._displayChoices.Count; j++)
				{
					this._popup.setDisplayPopupValue(j, this._displayChoices[j]);
				}
			}
		}
	}

	// Token: 0x0600641D RID: 25629 RVA: 0x002603F0 File Offset: 0x0025E7F0
	protected void SyncPopupAlt()
	{
		if (this._popupAlt != null)
		{
			this._popupAlt.useDifferentDisplayValues = true;
			if (this._choices != null)
			{
				this._popupAlt.numPopupValues = this._choices.Count;
				for (int i = 0; i < this._choices.Count; i++)
				{
					this._popupAlt.setPopupValue(i, this._choices[i]);
				}
			}
			else
			{
				this._popupAlt.numPopupValues = 0;
			}
			if (this._displayChoices != null)
			{
				for (int j = 0; j < this._displayChoices.Count; j++)
				{
					this._popupAlt.setDisplayPopupValue(j, this._displayChoices[j]);
				}
			}
		}
	}

	// Token: 0x17000EBB RID: 3771
	// (get) Token: 0x0600641E RID: 25630 RVA: 0x002604BF File Offset: 0x0025E8BF
	// (set) Token: 0x0600641F RID: 25631 RVA: 0x002604C7 File Offset: 0x0025E8C7
	public List<string> choices
	{
		get
		{
			return this._choices;
		}
		set
		{
			if (this._choices != value)
			{
				this._choices = value;
				if (!this.useDifferentDisplayChoices)
				{
					this._displayChoices = this._choices;
				}
				this.SyncPopup();
				this.SyncPopupAlt();
			}
		}
	}

	// Token: 0x17000EBC RID: 3772
	// (get) Token: 0x06006420 RID: 25632 RVA: 0x002604FF File Offset: 0x0025E8FF
	// (set) Token: 0x06006421 RID: 25633 RVA: 0x00260507 File Offset: 0x0025E907
	public List<string> displayChoices
	{
		get
		{
			return this._displayChoices;
		}
		set
		{
			if (this._displayChoices != value)
			{
				this._displayChoices = value;
				this.useDifferentDisplayChoices = true;
				this.SyncPopup();
				this.SyncPopupAlt();
			}
		}
	}

	// Token: 0x06006422 RID: 25634 RVA: 0x0026052F File Offset: 0x0025E92F
	public override void SetDefaultFromCurrent()
	{
		this.defaultVal = this.val;
	}

	// Token: 0x06006423 RID: 25635 RVA: 0x0026053D File Offset: 0x0025E93D
	public override void SetValToDefault()
	{
		this.val = this.defaultVal;
	}

	// Token: 0x06006424 RID: 25636 RVA: 0x0026054C File Offset: 0x0025E94C
	protected void InternalSetVal(string s, bool doCallback = true)
	{
		if (this._val != s)
		{
			this._val = s;
			if (this._popup != null)
			{
				this._popup.currentValueNoCallback = this._val;
			}
			if (this._popupAlt != null)
			{
				this._popupAlt.currentValueNoCallback = this._val;
			}
			if (this._toggleGroupValue != null)
			{
				this._toggleGroupValue.activeToggleNameNoCallback = this._val;
			}
			if (this._toggleGroupValueAlt != null)
			{
				this._toggleGroupValueAlt.activeToggleNameNoCallback = this._val;
			}
			if (doCallback)
			{
				if (this.setCallbackFunction != null)
				{
					this.setCallbackFunction(this._val);
				}
				if (this.setJSONCallbackFunction != null)
				{
					this.setJSONCallbackFunction(this);
				}
			}
		}
	}

	// Token: 0x17000EBD RID: 3773
	// (get) Token: 0x06006425 RID: 25637 RVA: 0x00260632 File Offset: 0x0025EA32
	public bool valueSetFromUI
	{
		get
		{
			return this._valueSetFromUI;
		}
	}

	// Token: 0x06006426 RID: 25638 RVA: 0x0026063A File Offset: 0x0025EA3A
	public void SetVal(string value)
	{
		this.val = value;
	}

	// Token: 0x06006427 RID: 25639 RVA: 0x00260643 File Offset: 0x0025EA43
	protected void SetValFromUI(string value)
	{
		this._valueSetFromUI = true;
		this.val = value;
		this._valueSetFromUI = false;
	}

	// Token: 0x17000EBE RID: 3774
	// (get) Token: 0x06006428 RID: 25640 RVA: 0x0026065A File Offset: 0x0025EA5A
	// (set) Token: 0x06006429 RID: 25641 RVA: 0x00260662 File Offset: 0x0025EA62
	public virtual string val
	{
		get
		{
			return this._val;
		}
		set
		{
			this.InternalSetVal(value, true);
		}
	}

	// Token: 0x17000EBF RID: 3775
	// (get) Token: 0x0600642A RID: 25642 RVA: 0x0026066C File Offset: 0x0025EA6C
	// (set) Token: 0x0600642B RID: 25643 RVA: 0x00260674 File Offset: 0x0025EA74
	public string valNoCallback
	{
		get
		{
			return this._val;
		}
		set
		{
			this.InternalSetVal(value, false);
		}
	}

	// Token: 0x0600642C RID: 25644 RVA: 0x0026067E File Offset: 0x0025EA7E
	public void PopupOpen()
	{
		if (this.popupOpenCallback != null)
		{
			this.popupOpenCallback();
		}
	}

	// Token: 0x0600642D RID: 25645 RVA: 0x00260696 File Offset: 0x0025EA96
	public void RegisterPopup(UIPopup p, bool isAlt = false)
	{
		if (isAlt)
		{
			this.popupAlt = p;
		}
		else
		{
			this.popup = p;
		}
	}

	// Token: 0x17000EC0 RID: 3776
	// (get) Token: 0x0600642E RID: 25646 RVA: 0x002606B1 File Offset: 0x0025EAB1
	// (set) Token: 0x0600642F RID: 25647 RVA: 0x002606BC File Offset: 0x0025EABC
	public UIPopup popup
	{
		get
		{
			return this._popup;
		}
		set
		{
			if (this._popup != value)
			{
				if (this._popup != null)
				{
					UIPopup popup = this._popup;
					popup.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Remove(popup.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetValFromUI));
					UIPopup popup2 = this._popup;
					popup2.onOpenPopupHandlers = (UIPopup.OnOpenPopup)Delegate.Remove(popup2.onOpenPopupHandlers, new UIPopup.OnOpenPopup(this.PopupOpen));
				}
				this._popup = value;
				if (this._popup != null)
				{
					if (this._label != null)
					{
						this._popup.label = this._label;
					}
					this.SyncPopup();
					this._popup.currentValue = this._val;
					UIPopup popup3 = this._popup;
					popup3.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(popup3.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetValFromUI));
					UIPopup popup4 = this._popup;
					popup4.onOpenPopupHandlers = (UIPopup.OnOpenPopup)Delegate.Combine(popup4.onOpenPopupHandlers, new UIPopup.OnOpenPopup(this.PopupOpen));
				}
			}
		}
	}

	// Token: 0x17000EC1 RID: 3777
	// (get) Token: 0x06006430 RID: 25648 RVA: 0x002607D2 File Offset: 0x0025EBD2
	// (set) Token: 0x06006431 RID: 25649 RVA: 0x002607DC File Offset: 0x0025EBDC
	public UIPopup popupAlt
	{
		get
		{
			return this._popupAlt;
		}
		set
		{
			if (this._popupAlt != value)
			{
				if (this._popupAlt != null)
				{
					UIPopup popupAlt = this._popupAlt;
					popupAlt.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Remove(popupAlt.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetValFromUI));
					UIPopup popupAlt2 = this._popupAlt;
					popupAlt2.onOpenPopupHandlers = (UIPopup.OnOpenPopup)Delegate.Remove(popupAlt2.onOpenPopupHandlers, new UIPopup.OnOpenPopup(this.PopupOpen));
				}
				this._popupAlt = value;
				if (this._popupAlt != null)
				{
					if (this._label != null)
					{
						this._popupAlt.label = this._label;
					}
					this.SyncPopupAlt();
					this._popupAlt.currentValue = this._val;
					UIPopup popupAlt3 = this._popupAlt;
					popupAlt3.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(popupAlt3.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetValFromUI));
					UIPopup popupAlt4 = this._popupAlt;
					popupAlt4.onOpenPopupHandlers = (UIPopup.OnOpenPopup)Delegate.Combine(popupAlt4.onOpenPopupHandlers, new UIPopup.OnOpenPopup(this.PopupOpen));
				}
			}
		}
	}

	// Token: 0x06006432 RID: 25650 RVA: 0x002608F2 File Offset: 0x0025ECF2
	public void RegisterToggleGroupValue(ToggleGroupValue tgv, bool isAlt = false)
	{
		if (isAlt)
		{
			this.toggleGroupValueAlt = tgv;
		}
		else
		{
			this.toggleGroupValue = tgv;
		}
	}

	// Token: 0x17000EC2 RID: 3778
	// (get) Token: 0x06006433 RID: 25651 RVA: 0x0026090D File Offset: 0x0025ED0D
	// (set) Token: 0x06006434 RID: 25652 RVA: 0x00260918 File Offset: 0x0025ED18
	public ToggleGroupValue toggleGroupValue
	{
		get
		{
			return this._toggleGroupValue;
		}
		set
		{
			if (this._toggleGroupValue != value)
			{
				if (this._toggleGroupValue != null)
				{
					ToggleGroupValue toggleGroupValue = this._toggleGroupValue;
					toggleGroupValue.onToggleChangedHandlers = (ToggleGroupValue.OnToggleChanged)Delegate.Remove(toggleGroupValue.onToggleChangedHandlers, new ToggleGroupValue.OnToggleChanged(this.SetValFromUI));
				}
				this._toggleGroupValue = value;
				if (this._toggleGroupValue != null)
				{
					this._toggleGroupValue.activeToggleNameNoCallback = this._val;
					ToggleGroupValue toggleGroupValue2 = this._toggleGroupValue;
					toggleGroupValue2.onToggleChangedHandlers = (ToggleGroupValue.OnToggleChanged)Delegate.Combine(toggleGroupValue2.onToggleChangedHandlers, new ToggleGroupValue.OnToggleChanged(this.SetValFromUI));
				}
			}
		}
	}

	// Token: 0x17000EC3 RID: 3779
	// (get) Token: 0x06006435 RID: 25653 RVA: 0x002609BE File Offset: 0x0025EDBE
	// (set) Token: 0x06006436 RID: 25654 RVA: 0x002609C8 File Offset: 0x0025EDC8
	public ToggleGroupValue toggleGroupValueAlt
	{
		get
		{
			return this._toggleGroupValueAlt;
		}
		set
		{
			if (this._toggleGroupValueAlt != value)
			{
				if (this._toggleGroupValueAlt != null)
				{
					ToggleGroupValue toggleGroupValueAlt = this._toggleGroupValueAlt;
					toggleGroupValueAlt.onToggleChangedHandlers = (ToggleGroupValue.OnToggleChanged)Delegate.Remove(toggleGroupValueAlt.onToggleChangedHandlers, new ToggleGroupValue.OnToggleChanged(this.SetValFromUI));
				}
				this._toggleGroupValueAlt = value;
				if (this._toggleGroupValueAlt != null)
				{
					this._toggleGroupValueAlt.activeToggleNameNoCallback = this._val;
					ToggleGroupValue toggleGroupValueAlt2 = this._toggleGroupValueAlt;
					toggleGroupValueAlt2.onToggleChangedHandlers = (ToggleGroupValue.OnToggleChanged)Delegate.Combine(toggleGroupValueAlt2.onToggleChangedHandlers, new ToggleGroupValue.OnToggleChanged(this.SetValFromUI));
				}
			}
		}
	}

	// Token: 0x17000EC4 RID: 3780
	// (get) Token: 0x06006437 RID: 25655 RVA: 0x00260A6E File Offset: 0x0025EE6E
	// (set) Token: 0x06006438 RID: 25656 RVA: 0x00260A78 File Offset: 0x0025EE78
	public string label
	{
		get
		{
			return this._label;
		}
		set
		{
			if (this._label != value)
			{
				this._label = value;
				if (this._label != null)
				{
					if (this._popup != null)
					{
						this._popup.label = this._label;
					}
					if (this._popupAlt != null)
					{
						this._popupAlt.label = this._label;
					}
				}
			}
		}
	}

	// Token: 0x0400541E RID: 21534
	public bool representsAtomUid;

	// Token: 0x0400541F RID: 21535
	protected List<string> _choices;

	// Token: 0x04005420 RID: 21536
	protected bool useDifferentDisplayChoices;

	// Token: 0x04005421 RID: 21537
	protected List<string> _displayChoices;

	// Token: 0x04005422 RID: 21538
	public string defaultVal;

	// Token: 0x04005423 RID: 21539
	protected bool _valueSetFromUI;

	// Token: 0x04005424 RID: 21540
	protected string _val;

	// Token: 0x04005425 RID: 21541
	public JSONStorableStringChooser.PopupOpenCallback popupOpenCallback;

	// Token: 0x04005426 RID: 21542
	public JSONStorableStringChooser.SetStringCallback setCallbackFunction;

	// Token: 0x04005427 RID: 21543
	public JSONStorableStringChooser.SetJSONStringCallback setJSONCallbackFunction;

	// Token: 0x04005428 RID: 21544
	protected UIPopup _popup;

	// Token: 0x04005429 RID: 21545
	protected UIPopup _popupAlt;

	// Token: 0x0400542A RID: 21546
	protected ToggleGroupValue _toggleGroupValue;

	// Token: 0x0400542B RID: 21547
	protected ToggleGroupValue _toggleGroupValueAlt;

	// Token: 0x0400542C RID: 21548
	protected string _label;

	// Token: 0x02000CE9 RID: 3305
	// (Invoke) Token: 0x0600643A RID: 25658
	public delegate void PopupOpenCallback();

	// Token: 0x02000CEA RID: 3306
	// (Invoke) Token: 0x0600643E RID: 25662
	public delegate void SetStringCallback(string val);

	// Token: 0x02000CEB RID: 3307
	// (Invoke) Token: 0x06006442 RID: 25666
	public delegate void SetJSONStringCallback(JSONStorableStringChooser js);
}
