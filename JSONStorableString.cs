using System;
using SimpleJSON;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000CE5 RID: 3301
public class JSONStorableString : JSONStorableParam
{
	// Token: 0x060063DB RID: 25563 RVA: 0x0025F38D File Offset: 0x0025D78D
	public JSONStorableString(string paramName, string startingValue)
	{
		this.type = JSONStorable.Type.String;
		this.name = paramName;
		this.defaultVal = startingValue;
		this.val = startingValue;
		this.setCallbackFunction = null;
		this.setJSONCallbackFunction = null;
	}

	// Token: 0x060063DC RID: 25564 RVA: 0x0025F3C6 File Offset: 0x0025D7C6
	public JSONStorableString(string paramName, string startingValue, JSONStorableString.SetStringCallback callback)
	{
		this.type = JSONStorable.Type.String;
		this.name = paramName;
		this.defaultVal = startingValue;
		this.val = startingValue;
		this.setCallbackFunction = callback;
		this.setJSONCallbackFunction = null;
	}

	// Token: 0x060063DD RID: 25565 RVA: 0x0025F3FF File Offset: 0x0025D7FF
	public JSONStorableString(string paramName, string startingValue, JSONStorableString.SetJSONStringCallback callback)
	{
		this.type = JSONStorable.Type.String;
		this.name = paramName;
		this.defaultVal = startingValue;
		this.val = startingValue;
		this.setCallbackFunction = null;
		this.setJSONCallbackFunction = callback;
	}

	// Token: 0x060063DE RID: 25566 RVA: 0x0025F438 File Offset: 0x0025D838
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

	// Token: 0x060063DF RID: 25567 RVA: 0x0025F4E4 File Offset: 0x0025D8E4
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

	// Token: 0x060063E0 RID: 25568 RVA: 0x0025F58C File Offset: 0x0025D98C
	public override void LateRestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, bool setMissingToDefault = true)
	{
		bool flag = this.NeedsLateRestore(jc, restorePhysical, restoreAppearance);
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

	// Token: 0x060063E1 RID: 25569 RVA: 0x0025F632 File Offset: 0x0025DA32
	public override void SetDefaultFromCurrent()
	{
		this.defaultVal = this.val;
	}

	// Token: 0x060063E2 RID: 25570 RVA: 0x0025F640 File Offset: 0x0025DA40
	public override void SetValToDefault()
	{
		this.val = this.defaultVal;
	}

	// Token: 0x060063E3 RID: 25571 RVA: 0x0025F650 File Offset: 0x0025DA50
	protected void InternalSetVal(string s, bool doCallback = true)
	{
		if (this._val != s)
		{
			this._val = s;
			if (this._text != null)
			{
				this._text.text = this._val;
			}
			if (this._textAlt != null)
			{
				this._textAlt.text = this._val;
			}
			if (this._inputField != null)
			{
				this._inputField.text = this._val;
			}
			if (this._inputFieldAlt != null)
			{
				this._inputFieldAlt.text = this._val;
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

	// Token: 0x060063E4 RID: 25572 RVA: 0x0025F736 File Offset: 0x0025DB36
	public void SetVal(string value)
	{
		this.val = value;
	}

	// Token: 0x060063E5 RID: 25573 RVA: 0x0025F73F File Offset: 0x0025DB3F
	public void SetValToInputField()
	{
		if (this._inputField != null)
		{
			this.val = this._inputField.text;
		}
	}

	// Token: 0x060063E6 RID: 25574 RVA: 0x0025F763 File Offset: 0x0025DB63
	public void SetValToInputFieldAlt()
	{
		if (this._inputFieldAlt != null)
		{
			this.val = this._inputFieldAlt.text;
		}
	}

	// Token: 0x060063E7 RID: 25575 RVA: 0x0025F787 File Offset: 0x0025DB87
	public void ClearVal()
	{
		this.val = string.Empty;
	}

	// Token: 0x17000EAC RID: 3756
	// (get) Token: 0x060063E8 RID: 25576 RVA: 0x0025F794 File Offset: 0x0025DB94
	// (set) Token: 0x060063E9 RID: 25577 RVA: 0x0025F79C File Offset: 0x0025DB9C
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

	// Token: 0x17000EAD RID: 3757
	// (get) Token: 0x060063EA RID: 25578 RVA: 0x0025F7A6 File Offset: 0x0025DBA6
	// (set) Token: 0x060063EB RID: 25579 RVA: 0x0025F7AE File Offset: 0x0025DBAE
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

	// Token: 0x060063EC RID: 25580 RVA: 0x0025F7B8 File Offset: 0x0025DBB8
	public void RegisterText(Text t, bool isAlt = false)
	{
		if (isAlt)
		{
			this.textAlt = t;
		}
		else
		{
			this.text = t;
		}
	}

	// Token: 0x17000EAE RID: 3758
	// (get) Token: 0x060063ED RID: 25581 RVA: 0x0025F7D3 File Offset: 0x0025DBD3
	// (set) Token: 0x060063EE RID: 25582 RVA: 0x0025F7DB File Offset: 0x0025DBDB
	public Text text
	{
		get
		{
			return this._text;
		}
		set
		{
			if (this._text != value)
			{
				this._text = value;
				if (this._text != null)
				{
					this._text.text = this._val;
				}
			}
		}
	}

	// Token: 0x17000EAF RID: 3759
	// (get) Token: 0x060063EF RID: 25583 RVA: 0x0025F817 File Offset: 0x0025DC17
	// (set) Token: 0x060063F0 RID: 25584 RVA: 0x0025F81F File Offset: 0x0025DC1F
	public Text textAlt
	{
		get
		{
			return this._textAlt;
		}
		set
		{
			if (this._textAlt != value)
			{
				this._textAlt = value;
				if (this._textAlt != null)
				{
					this._textAlt.text = this._val;
				}
			}
		}
	}

	// Token: 0x17000EB0 RID: 3760
	// (get) Token: 0x060063F1 RID: 25585 RVA: 0x0025F85B File Offset: 0x0025DC5B
	// (set) Token: 0x060063F2 RID: 25586 RVA: 0x0025F863 File Offset: 0x0025DC63
	public UIDynamicTextField dynamicText
	{
		get
		{
			return this._dynamicText;
		}
		set
		{
			if (this._dynamicText != value)
			{
				this._dynamicText = value;
				if (this._dynamicText != null)
				{
					this.text = this.dynamicText.UItext;
				}
			}
		}
	}

	// Token: 0x17000EB1 RID: 3761
	// (get) Token: 0x060063F3 RID: 25587 RVA: 0x0025F89F File Offset: 0x0025DC9F
	// (set) Token: 0x060063F4 RID: 25588 RVA: 0x0025F8A7 File Offset: 0x0025DCA7
	public UIDynamicTextField dynamicTextAlt
	{
		get
		{
			return this._dynamicTextAlt;
		}
		set
		{
			if (this._dynamicTextAlt != value)
			{
				this._dynamicTextAlt = value;
				if (this._dynamicTextAlt != null)
				{
					this.textAlt = this.dynamicTextAlt.UItext;
				}
			}
		}
	}

	// Token: 0x17000EB2 RID: 3762
	// (get) Token: 0x060063F5 RID: 25589 RVA: 0x0025F8E3 File Offset: 0x0025DCE3
	// (set) Token: 0x060063F6 RID: 25590 RVA: 0x0025F8EC File Offset: 0x0025DCEC
	public bool interactable
	{
		get
		{
			return this._interactable;
		}
		set
		{
			if (this._interactable != value)
			{
				this._interactable = value;
				if (this._inputField != null)
				{
					this._inputField.interactable = this._interactable;
				}
				if (this._inputFieldAlt != null)
				{
					this._inputFieldAlt.interactable = this._interactable;
				}
			}
		}
	}

	// Token: 0x060063F7 RID: 25591 RVA: 0x0025F950 File Offset: 0x0025DD50
	public void RegisterInputField(InputField inf, bool isAlt = false)
	{
		if (isAlt)
		{
			this.inputFieldAlt = inf;
		}
		else
		{
			this.inputField = inf;
		}
	}

	// Token: 0x17000EB3 RID: 3763
	// (get) Token: 0x060063F8 RID: 25592 RVA: 0x0025F96B File Offset: 0x0025DD6B
	// (set) Token: 0x060063F9 RID: 25593 RVA: 0x0025F974 File Offset: 0x0025DD74
	public InputField inputField
	{
		get
		{
			return this._inputField;
		}
		set
		{
			if (this._inputField != value)
			{
				if (this._inputField != null)
				{
					if (!this.disableOnEndEdit)
					{
						this._inputField.onEndEdit.RemoveListener(new UnityAction<string>(this.SetVal));
					}
					if (this.enableOnChange)
					{
						this._inputField.onValueChanged.RemoveListener(new UnityAction<string>(this.SetVal));
					}
				}
				this._inputField = value;
				if (this._inputField != null)
				{
					this._inputField.text = this._val;
					this._inputField.interactable = this._interactable;
					if (!this.disableOnEndEdit)
					{
						this._inputField.onEndEdit.AddListener(new UnityAction<string>(this.SetVal));
					}
					if (this.enableOnChange)
					{
						this._inputField.onValueChanged.AddListener(new UnityAction<string>(this.SetVal));
					}
				}
			}
		}
	}

	// Token: 0x17000EB4 RID: 3764
	// (get) Token: 0x060063FA RID: 25594 RVA: 0x0025FA79 File Offset: 0x0025DE79
	// (set) Token: 0x060063FB RID: 25595 RVA: 0x0025FA84 File Offset: 0x0025DE84
	public InputField inputFieldAlt
	{
		get
		{
			return this._inputFieldAlt;
		}
		set
		{
			if (this._inputFieldAlt != value)
			{
				if (this._inputFieldAlt != null)
				{
					if (!this.disableOnEndEdit)
					{
						this._inputFieldAlt.onEndEdit.RemoveListener(new UnityAction<string>(this.SetVal));
					}
					if (this.enableOnChange)
					{
						this._inputFieldAlt.onValueChanged.RemoveListener(new UnityAction<string>(this.SetVal));
					}
				}
				this._inputFieldAlt = value;
				if (this._inputFieldAlt != null)
				{
					this._inputFieldAlt.text = this._val;
					this._inputFieldAlt.interactable = this._interactable;
					if (!this.disableOnEndEdit)
					{
						this._inputFieldAlt.onEndEdit.AddListener(new UnityAction<string>(this.SetVal));
					}
					if (this.enableOnChange)
					{
						this._inputFieldAlt.onValueChanged.AddListener(new UnityAction<string>(this.SetVal));
					}
				}
			}
		}
	}

	// Token: 0x060063FC RID: 25596 RVA: 0x0025FB89 File Offset: 0x0025DF89
	public void RegisterInputFieldAction(InputFieldAction infa, bool isAlt = false)
	{
		if (isAlt)
		{
			this.inputFieldActionAlt = infa;
		}
		else
		{
			this.inputFieldAction = infa;
		}
	}

	// Token: 0x17000EB5 RID: 3765
	// (get) Token: 0x060063FD RID: 25597 RVA: 0x0025FBA4 File Offset: 0x0025DFA4
	// (set) Token: 0x060063FE RID: 25598 RVA: 0x0025FBAC File Offset: 0x0025DFAC
	public InputFieldAction inputFieldAction
	{
		get
		{
			return this._inputFieldAction;
		}
		set
		{
			if (this._inputFieldAction != value)
			{
				if (this._inputFieldAction != null)
				{
					InputFieldAction inputFieldAction = this._inputFieldAction;
					inputFieldAction.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Remove(inputFieldAction.onSubmitHandlers, new InputFieldAction.OnSubmit(this.SetValToInputField));
				}
				this._inputFieldAction = value;
				if (this._inputFieldAction != null)
				{
					InputFieldAction inputFieldAction2 = this._inputFieldAction;
					inputFieldAction2.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Combine(inputFieldAction2.onSubmitHandlers, new InputFieldAction.OnSubmit(this.SetValToInputField));
				}
			}
		}
	}

	// Token: 0x17000EB6 RID: 3766
	// (get) Token: 0x060063FF RID: 25599 RVA: 0x0025FC41 File Offset: 0x0025E041
	// (set) Token: 0x06006400 RID: 25600 RVA: 0x0025FC4C File Offset: 0x0025E04C
	public InputFieldAction inputFieldActionAlt
	{
		get
		{
			return this._inputFieldActionAlt;
		}
		set
		{
			if (this._inputFieldActionAlt != value)
			{
				if (this._inputFieldActionAlt != null)
				{
					InputFieldAction inputFieldActionAlt = this._inputFieldActionAlt;
					inputFieldActionAlt.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Remove(inputFieldActionAlt.onSubmitHandlers, new InputFieldAction.OnSubmit(this.SetValToInputFieldAlt));
				}
				this._inputFieldActionAlt = value;
				if (this._inputFieldActionAlt != null)
				{
					InputFieldAction inputFieldActionAlt2 = this._inputFieldActionAlt;
					inputFieldActionAlt2.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Combine(inputFieldActionAlt2.onSubmitHandlers, new InputFieldAction.OnSubmit(this.SetValToInputFieldAlt));
				}
			}
		}
	}

	// Token: 0x06006401 RID: 25601 RVA: 0x0025FCE1 File Offset: 0x0025E0E1
	public void RegisterSetValToInputFieldButton(Button but, bool isAlt = false)
	{
		if (isAlt)
		{
			this.setValToInputFieldButtonAlt = but;
		}
		else
		{
			this.setValToInputFieldButton = but;
		}
	}

	// Token: 0x17000EB7 RID: 3767
	// (get) Token: 0x06006402 RID: 25602 RVA: 0x0025FCFC File Offset: 0x0025E0FC
	// (set) Token: 0x06006403 RID: 25603 RVA: 0x0025FD04 File Offset: 0x0025E104
	public Button setValToInputFieldButton
	{
		get
		{
			return this._setValToInputFieldButton;
		}
		set
		{
			if (this._setValToInputFieldButton != value)
			{
				if (this._setValToInputFieldButton != null)
				{
					this._setValToInputFieldButton.onClick.RemoveListener(new UnityAction(this.SetValToInputField));
				}
				this._setValToInputFieldButton = value;
				if (this._setValToInputFieldButton != null)
				{
					this._setValToInputFieldButton.onClick.AddListener(new UnityAction(this.SetValToInputField));
				}
			}
		}
	}

	// Token: 0x17000EB8 RID: 3768
	// (get) Token: 0x06006404 RID: 25604 RVA: 0x0025FD83 File Offset: 0x0025E183
	// (set) Token: 0x06006405 RID: 25605 RVA: 0x0025FD8C File Offset: 0x0025E18C
	public Button setValToInputFieldButtonAlt
	{
		get
		{
			return this._setValToInputFieldButtonAlt;
		}
		set
		{
			if (this._setValToInputFieldButtonAlt != value)
			{
				if (this._setValToInputFieldButtonAlt != null)
				{
					this._setValToInputFieldButtonAlt.onClick.RemoveListener(new UnityAction(this.SetValToInputFieldAlt));
				}
				this._setValToInputFieldButtonAlt = value;
				if (this._setValToInputFieldButtonAlt != null)
				{
					this._setValToInputFieldButtonAlt.onClick.AddListener(new UnityAction(this.SetValToInputFieldAlt));
				}
			}
		}
	}

	// Token: 0x06006406 RID: 25606 RVA: 0x0025FE0B File Offset: 0x0025E20B
	public void RegisterClearValButton(Button but, bool isAlt = false)
	{
		if (isAlt)
		{
			this.clearValButtonAlt = but;
		}
		else
		{
			this.clearValButton = but;
		}
	}

	// Token: 0x17000EB9 RID: 3769
	// (get) Token: 0x06006407 RID: 25607 RVA: 0x0025FE26 File Offset: 0x0025E226
	// (set) Token: 0x06006408 RID: 25608 RVA: 0x0025FE30 File Offset: 0x0025E230
	public Button clearValButton
	{
		get
		{
			return this._clearValButton;
		}
		set
		{
			if (this._clearValButton != value)
			{
				if (this._clearValButton != null)
				{
					this._clearValButton.onClick.RemoveListener(new UnityAction(this.ClearVal));
				}
				this._clearValButton = value;
				if (this._clearValButton != null)
				{
					this._clearValButton.onClick.AddListener(new UnityAction(this.ClearVal));
				}
			}
		}
	}

	// Token: 0x17000EBA RID: 3770
	// (get) Token: 0x06006409 RID: 25609 RVA: 0x0025FEAF File Offset: 0x0025E2AF
	// (set) Token: 0x0600640A RID: 25610 RVA: 0x0025FEB8 File Offset: 0x0025E2B8
	public Button clearValButtonAlt
	{
		get
		{
			return this._clearValButtonAlt;
		}
		set
		{
			if (this._clearValButtonAlt != value)
			{
				if (this._clearValButtonAlt != null)
				{
					this._clearValButtonAlt.onClick.RemoveListener(new UnityAction(this.ClearVal));
				}
				this._clearValButtonAlt = value;
				if (this._clearValButtonAlt != null)
				{
					this._clearValButtonAlt.onClick.AddListener(new UnityAction(this.ClearVal));
				}
			}
		}
	}

	// Token: 0x0400540A RID: 21514
	public bool representsAtomUid;

	// Token: 0x0400540B RID: 21515
	public string defaultVal;

	// Token: 0x0400540C RID: 21516
	protected string _val;

	// Token: 0x0400540D RID: 21517
	public JSONStorableString.SetStringCallback setCallbackFunction;

	// Token: 0x0400540E RID: 21518
	public JSONStorableString.SetJSONStringCallback setJSONCallbackFunction;

	// Token: 0x0400540F RID: 21519
	protected Text _text;

	// Token: 0x04005410 RID: 21520
	protected Text _textAlt;

	// Token: 0x04005411 RID: 21521
	protected UIDynamicTextField _dynamicText;

	// Token: 0x04005412 RID: 21522
	protected UIDynamicTextField _dynamicTextAlt;

	// Token: 0x04005413 RID: 21523
	public bool disableOnEndEdit;

	// Token: 0x04005414 RID: 21524
	public bool enableOnChange;

	// Token: 0x04005415 RID: 21525
	protected bool _interactable = true;

	// Token: 0x04005416 RID: 21526
	protected InputField _inputField;

	// Token: 0x04005417 RID: 21527
	protected InputField _inputFieldAlt;

	// Token: 0x04005418 RID: 21528
	protected InputFieldAction _inputFieldAction;

	// Token: 0x04005419 RID: 21529
	protected InputFieldAction _inputFieldActionAlt;

	// Token: 0x0400541A RID: 21530
	protected Button _setValToInputFieldButton;

	// Token: 0x0400541B RID: 21531
	protected Button _setValToInputFieldButtonAlt;

	// Token: 0x0400541C RID: 21532
	protected Button _clearValButton;

	// Token: 0x0400541D RID: 21533
	protected Button _clearValButtonAlt;

	// Token: 0x02000CE6 RID: 3302
	// (Invoke) Token: 0x0600640C RID: 25612
	public delegate void SetJSONStringCallback(JSONStorableString js);

	// Token: 0x02000CE7 RID: 3303
	// (Invoke) Token: 0x06006410 RID: 25616
	public delegate void SetStringCallback(string val);
}
