using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000DA7 RID: 3495
public abstract class TriggerAction
{
	// Token: 0x06006BEF RID: 27631 RVA: 0x00289BEC File Offset: 0x00287FEC
	public TriggerAction()
	{
		this.previewTextJSON = new JSONStorableString("previewText", string.Empty, new JSONStorableString.SetStringCallback(this.SyncPreviewText));
		this.nameJSON = new JSONStorableString("name", string.Empty, new JSONStorableString.SetStringCallback(this.SyncName));
		this.enabledJSON = new JSONStorableBool("enabled", true, new JSONStorableBool.SetBoolCallback(this.SyncEnabled));
	}

	// Token: 0x06006BF0 RID: 27632 RVA: 0x00289C75 File Offset: 0x00288075
	public virtual JSONClass GetJSON()
	{
		return this.GetJSON(null);
	}

	// Token: 0x06006BF1 RID: 27633 RVA: 0x00289C80 File Offset: 0x00288080
	public virtual JSONClass GetJSON(string subScenePrefix)
	{
		JSONClass jsonclass = new JSONClass();
		this.nameJSON.StoreJSON(jsonclass, true, true, false);
		this.enabledJSON.StoreJSON(jsonclass, true, true, false);
		if (this._receiverAtom != null)
		{
			if (subScenePrefix != null)
			{
				string text = "^" + subScenePrefix;
				if (Regex.IsMatch(this._receiverAtom.uid, text + "[^/]+$"))
				{
					jsonclass["receiverAtom"] = Regex.Replace(this._receiverAtom.uid, text, string.Empty);
				}
				else
				{
					SuperController.LogError("Warning: trigger referencing atom " + this._receiverAtom.uid + " is outside of subscene. This trigger will be stored as external_ref which will only work if you load into a compatible scene.");
					jsonclass["receiverAtom"] = "external_ref:" + this._receiverAtom.uid;
				}
			}
			else
			{
				jsonclass["receiverAtom"] = this._receiverAtom.uid;
			}
		}
		if (this._receiver != null)
		{
			jsonclass["receiver"] = this._receiver.storeId;
		}
		if (this._receiverTargetName != null)
		{
			jsonclass["receiverTargetName"] = this._receiverTargetName;
		}
		return jsonclass;
	}

	// Token: 0x06006BF2 RID: 27634 RVA: 0x00289DD3 File Offset: 0x002881D3
	public virtual void RestoreFromJSON(JSONClass jc)
	{
		this.RestoreFromJSON(jc, null);
	}

	// Token: 0x06006BF3 RID: 27635 RVA: 0x00289DE0 File Offset: 0x002881E0
	public virtual void RestoreFromJSON(JSONClass jc, string subScenePrefix)
	{
		this.nameJSON.RestoreFromJSON(jc, true, true, true);
		this.enabledJSON.RestoreFromJSON(jc, true, true, true);
		if (jc["receiverAtom"] != null)
		{
			string text = jc["receiverAtom"];
			if (subScenePrefix != null)
			{
				if (Regex.IsMatch(text, "^external_ref:"))
				{
					this.SetReceiverAtom(Regex.Replace(text, "^external_ref:", string.Empty));
				}
				else
				{
					this.SetReceiverAtom(subScenePrefix + text);
				}
			}
			else
			{
				this.SetReceiverAtom(text);
			}
		}
		if (jc["receiver"] != null)
		{
			this.SetReceiver(jc["receiver"]);
		}
		if (jc["receiverTargetName"] != null)
		{
			this.SetReceiverTargetName(jc["receiverTargetName"]);
		}
	}

	// Token: 0x06006BF4 RID: 27636 RVA: 0x00289ED4 File Offset: 0x002882D4
	protected void SyncPreviewText(string n)
	{
		if (this.handler != null)
		{
			this.handler.TriggerActionNameChange(this);
		}
	}

	// Token: 0x17000FD6 RID: 4054
	// (get) Token: 0x06006BF5 RID: 27637 RVA: 0x00289EED File Offset: 0x002882ED
	// (set) Token: 0x06006BF6 RID: 27638 RVA: 0x00289EFA File Offset: 0x002882FA
	public string previewText
	{
		get
		{
			return this.previewTextJSON.val;
		}
		set
		{
			this.previewTextJSON.val = value;
		}
	}

	// Token: 0x06006BF7 RID: 27639 RVA: 0x00289F08 File Offset: 0x00288308
	protected virtual void AutoSetPreviewText()
	{
		if (this._receiverAtom != null && !string.IsNullOrEmpty(this._receiverStoreId) && !string.IsNullOrEmpty(this._receiverTargetName))
		{
			this.previewText = string.Concat(new string[]
			{
				this._receiverAtom.uid,
				":",
				this._receiverStoreId,
				":",
				this._receiverTargetName
			});
		}
	}

	// Token: 0x06006BF8 RID: 27640 RVA: 0x00289F87 File Offset: 0x00288387
	protected void SyncName(string n)
	{
		if (this.handler != null)
		{
			this.handler.TriggerActionNameChange(this);
		}
	}

	// Token: 0x17000FD7 RID: 4055
	// (get) Token: 0x06006BF9 RID: 27641 RVA: 0x00289FA0 File Offset: 0x002883A0
	// (set) Token: 0x06006BFA RID: 27642 RVA: 0x00289FAD File Offset: 0x002883AD
	public string name
	{
		get
		{
			return this.nameJSON.val;
		}
		set
		{
			this.nameJSON.val = value;
		}
	}

	// Token: 0x06006BFB RID: 27643 RVA: 0x00289FBB File Offset: 0x002883BB
	protected virtual void AutoSetName()
	{
		if (this.receiver != null && !string.IsNullOrEmpty(this._receiverTargetName))
		{
			this.name = "A_" + this._receiverTargetName;
		}
	}

	// Token: 0x06006BFC RID: 27644 RVA: 0x00289FF4 File Offset: 0x002883F4
	protected virtual void AutoSetPreviewTextAndName()
	{
		this.AutoSetPreviewText();
		if (this.nameJSON.val != null && (this.nameJSON.val == string.Empty || this.nameJSON.val.StartsWith("A_")))
		{
			this.AutoSetName();
		}
	}

	// Token: 0x06006BFD RID: 27645 RVA: 0x0028A051 File Offset: 0x00288451
	protected virtual void SyncEnabled(bool b)
	{
	}

	// Token: 0x17000FD8 RID: 4056
	// (get) Token: 0x06006BFE RID: 27646 RVA: 0x0028A053 File Offset: 0x00288453
	// (set) Token: 0x06006BFF RID: 27647 RVA: 0x0028A060 File Offset: 0x00288460
	public bool enabled
	{
		get
		{
			return this.enabledJSON.val;
		}
		set
		{
			this.enabledJSON.val = value;
		}
	}

	// Token: 0x06006C00 RID: 27648
	protected abstract void CreateTriggerActionPanel();

	// Token: 0x17000FD9 RID: 4057
	// (get) Token: 0x06006C01 RID: 27649 RVA: 0x0028A06E File Offset: 0x0028846E
	// (set) Token: 0x06006C02 RID: 27650 RVA: 0x0028A076 File Offset: 0x00288476
	public bool detailPanelOpen
	{
		[CompilerGenerated]
		get
		{
			return this.<detailPanelOpen>k__BackingField;
		}
		[CompilerGenerated]
		protected set
		{
			this.<detailPanelOpen>k__BackingField = value;
		}
	}

	// Token: 0x06006C03 RID: 27651 RVA: 0x0028A080 File Offset: 0x00288480
	public virtual void OpenDetailPanel()
	{
		this.detailPanelOpen = true;
		if (this.triggerActionPanel == null)
		{
			this.CreateTriggerActionPanel();
		}
		if (this.triggerActionPanel != null)
		{
			this.triggerActionPanel.gameObject.SetActive(true);
		}
	}

	// Token: 0x06006C04 RID: 27652 RVA: 0x0028A0CD File Offset: 0x002884CD
	public virtual void CloseDetailPanel()
	{
		if (this.triggerActionPanel != null)
		{
			this.triggerActionPanel.gameObject.SetActive(false);
		}
		this.detailPanelOpen = false;
	}

	// Token: 0x06006C05 RID: 27653 RVA: 0x0028A0F8 File Offset: 0x002884F8
	public virtual void Remove()
	{
		if (this.handler != null)
		{
			this.DeregisterUI();
			this.handler.RemoveTriggerAction(this);
		}
		else
		{
			UnityEngine.Debug.LogError("Attempt to Remove() when handler is null");
		}
	}

	// Token: 0x06006C06 RID: 27654 RVA: 0x0028A126 File Offset: 0x00288526
	public virtual void Duplicate()
	{
		if (this.handler != null)
		{
			this.handler.DuplicateTriggerAction(this);
		}
		else
		{
			UnityEngine.Debug.LogError("Attempt to Duplicate() when handler is null");
		}
	}

	// Token: 0x06006C07 RID: 27655 RVA: 0x0028A14E File Offset: 0x0028854E
	public virtual void Validate()
	{
		if (this.receiverAtom != null && this.receiverAtom.destroyed)
		{
			this.Remove();
		}
	}

	// Token: 0x06006C08 RID: 27656 RVA: 0x0028A178 File Offset: 0x00288578
	protected virtual void SetReceiverAtomPopupValues()
	{
		if (SuperController.singleton != null)
		{
			List<string> visibleAtomUIDs = SuperController.singleton.GetVisibleAtomUIDs();
			int num = 0;
			this.receiverAtomPopup.numPopupValues = visibleAtomUIDs.Count + 1;
			this.receiverAtomPopup.setPopupValue(num, "None");
			num++;
			foreach (string text in visibleAtomUIDs)
			{
				this.receiverAtomPopup.setPopupValue(num, text);
				num++;
			}
		}
	}

	// Token: 0x06006C09 RID: 27657 RVA: 0x0028A220 File Offset: 0x00288620
	public virtual void SetReceiverAtom(string uid)
	{
		if (SuperController.singleton != null)
		{
			if (uid == "None")
			{
				this.receiverAtom = null;
			}
			else
			{
				this.receiverAtom = SuperController.singleton.GetAtomByUid(uid);
			}
		}
	}

	// Token: 0x06006C0A RID: 27658 RVA: 0x0028A25F File Offset: 0x0028865F
	public virtual void SyncAtomName()
	{
		if (this._receiverAtom != null && this.receiverAtomPopup != null)
		{
			this.receiverAtomPopup.currentValueNoCallback = this._receiverAtom.uid;
		}
	}

	// Token: 0x17000FDA RID: 4058
	// (get) Token: 0x06006C0B RID: 27659 RVA: 0x0028A299 File Offset: 0x00288699
	// (set) Token: 0x06006C0C RID: 27660 RVA: 0x0028A2A4 File Offset: 0x002886A4
	public Atom receiverAtom
	{
		get
		{
			return this._receiverAtom;
		}
		set
		{
			if (this._receiverAtom != value)
			{
				this._receiverAtom = value;
				if (this.receiverAtomPopup != null)
				{
					if (this._receiverAtom == null)
					{
						this.receiverAtomPopup.currentValue = "None";
					}
					else
					{
						this.receiverAtomPopup.currentValue = this._receiverAtom.uid;
					}
				}
				this.receiver = null;
				this.SetReceiverPopupValues();
				if (this.receiverPopup != null && this.receiverPopup.numPopupValues > 1)
				{
					this.SetReceiver(this.receiverPopup.popupValues[1]);
				}
			}
		}
	}

	// Token: 0x06006C0D RID: 27661 RVA: 0x0028A358 File Offset: 0x00288758
	protected virtual void SetReceiverPopupValues()
	{
		if (this.receiverPopup != null)
		{
			if (this._receiverAtom != null)
			{
				List<string> storableIDs = this._receiverAtom.GetStorableIDs();
				List<string> list = new List<string>();
				foreach (string text in storableIDs)
				{
					JSONStorable storableByID = this._receiverAtom.GetStorableByID(text);
					if (storableByID != null && storableByID.HasParamsOrActions())
					{
						list.Add(text);
					}
				}
				this.receiverPopup.numPopupValues = list.Count + 1;
				int num = 0;
				this.receiverPopup.setPopupValue(num, "None");
				num++;
				foreach (string text2 in list)
				{
					this.receiverPopup.setPopupValue(num, text2);
					num++;
				}
			}
			else
			{
				this.receiverPopup.numPopupValues = 1;
				int index = 0;
				this.receiverPopup.setPopupValue(index, "None");
			}
		}
	}

	// Token: 0x06006C0E RID: 27662 RVA: 0x0028A4B4 File Offset: 0x002888B4
	protected virtual void CheckMissingReceiver()
	{
		if (this._missingReceiverStoreid != string.Empty)
		{
			JSONStorable storableByID = this._receiverAtom.GetStorableByID(this._missingReceiverStoreid);
			if (storableByID != null)
			{
				this._missingReceiverStoreid = string.Empty;
				string receiverTargetName = this._receiverTargetName;
				this.receiver = storableByID;
				this.receiverTargetName = receiverTargetName;
			}
		}
		else if (this._receiverAtom != null && this._receiverStoreId != null)
		{
			JSONStorable storableByID2 = this._receiverAtom.GetStorableByID(this._receiverStoreId);
			if (storableByID2 != null && storableByID2 != this._receiver)
			{
				this._receiver = storableByID2;
			}
		}
	}

	// Token: 0x06006C0F RID: 27663 RVA: 0x0028A56C File Offset: 0x0028896C
	public virtual void SetReceiver(string storeid)
	{
		if (storeid == "None")
		{
			this.receiver = null;
		}
		else if (this._receiverAtom != null)
		{
			this._missingReceiverStoreid = string.Empty;
			this.receiver = this._receiverAtom.GetStorableByID(storeid);
			if (this.receiver == null)
			{
				this._missingReceiverStoreid = storeid;
			}
		}
	}

	// Token: 0x17000FDB RID: 4059
	// (get) Token: 0x06006C10 RID: 27664 RVA: 0x0028A5DB File Offset: 0x002889DB
	// (set) Token: 0x06006C11 RID: 27665 RVA: 0x0028A5E4 File Offset: 0x002889E4
	public JSONStorable receiver
	{
		get
		{
			return this._receiver;
		}
		set
		{
			if (this._receiver != value)
			{
				this._receiver = value;
				if (this._receiver != null)
				{
					this._receiverStoreId = this._receiver.storeId;
				}
				else
				{
					this._receiverStoreId = null;
				}
				if (this.receiverPopup != null)
				{
					if (this._receiver == null)
					{
						this.receiverPopup.currentValue = "None";
					}
					else
					{
						this.receiverPopup.currentValue = this.receiver.storeId;
					}
				}
				this.receiverTargetName = null;
				this.SetReceiverTargetPopupNames();
			}
		}
	}

	// Token: 0x06006C12 RID: 27666 RVA: 0x0028A694 File Offset: 0x00288A94
	protected virtual void SyncTargetPopupNames()
	{
		if (this.receiverTargetNamePopup != null)
		{
			if (this.receiverTargetNames != null)
			{
				this.receiverTargetNamePopup.numPopupValues = this.receiverTargetNames.Count + 1;
				int num = 0;
				this.receiverTargetNamePopup.setPopupValue(num, "None");
				num++;
				foreach (string text in this.receiverTargetNames)
				{
					this.receiverTargetNamePopup.setPopupValue(num, text);
					num++;
				}
			}
			else
			{
				this.receiverTargetNamePopup.numPopupValues = 1;
				int index = 0;
				this.receiverTargetNamePopup.setPopupValue(index, "None");
			}
		}
	}

	// Token: 0x06006C13 RID: 27667 RVA: 0x0028A76C File Offset: 0x00288B6C
	protected virtual void SetReceiverTargetPopupNames()
	{
		this.receiverTargetNames = null;
		if (this._receiver != null)
		{
			this.receiverTargetNames = this._receiver.GetFloatParamNames();
		}
		this.SyncTargetPopupNames();
	}

	// Token: 0x06006C14 RID: 27668 RVA: 0x0028A79D File Offset: 0x00288B9D
	public virtual void SetReceiverTargetName(string targetName)
	{
		this.receiverSetFromPopup = false;
		if (targetName == "None")
		{
			this.receiverTargetName = null;
		}
		else
		{
			this.receiverTargetName = targetName;
		}
	}

	// Token: 0x06006C15 RID: 27669 RVA: 0x0028A7CC File Offset: 0x00288BCC
	public virtual void SetReceiverTargetNameAndSetInitialParams(string targetName)
	{
		this.receiverSetFromPopup = true;
		if (targetName == "None")
		{
			this.receiverTargetName = null;
		}
		else if (this._receiverTargetName != targetName)
		{
			this.receiverTargetName = targetName;
			this.SetInitialParamsFromReceiverTarget();
		}
		this.receiverSetFromPopup = false;
	}

	// Token: 0x06006C16 RID: 27670 RVA: 0x0028A821 File Offset: 0x00288C21
	protected virtual void SetInitialParamsFromReceiverTarget()
	{
	}

	// Token: 0x06006C17 RID: 27671 RVA: 0x0028A824 File Offset: 0x00288C24
	protected virtual void SyncFromReceiverTarget()
	{
		if (this._receiver != null && this._receiverTargetName != null)
		{
			this.receiverTargetFloat = this._receiver.GetFloatJSONParam(this._receiverTargetName);
			if (this.receiverTargetFloat != null)
			{
				this.paramContrained = this.receiverTargetFloat.constrained;
				this.paramDefault = this.receiverTargetFloat.defaultVal;
				this.paramMin = this.receiverTargetFloat.min;
				this.paramMax = this.receiverTargetFloat.max;
			}
			else
			{
				this.paramContrained = false;
				this.paramDefault = 0f;
				this.paramMin = 0f;
				this.paramMax = 1f;
			}
		}
	}

	// Token: 0x17000FDC RID: 4060
	// (get) Token: 0x06006C18 RID: 27672 RVA: 0x0028A8E0 File Offset: 0x00288CE0
	// (set) Token: 0x06006C19 RID: 27673 RVA: 0x0028A8E8 File Offset: 0x00288CE8
	public string receiverTargetName
	{
		get
		{
			return this._receiverTargetName;
		}
		set
		{
			if (this._receiverTargetName != value)
			{
				this._receiverTargetName = value;
				if (this.receiverTargetNamePopup != null)
				{
					if (this._receiverTargetName == null)
					{
						this.receiverTargetNamePopup.currentValue = "None";
					}
					else
					{
						this.receiverTargetNamePopup.currentValue = this._receiverTargetName;
					}
				}
				this.SyncFromReceiverTarget();
			}
		}
	}

	// Token: 0x06006C1A RID: 27674 RVA: 0x0028A958 File Offset: 0x00288D58
	public virtual void InitTriggerActionMiniPanelUI()
	{
		if (this.triggerActionMiniPanel != null)
		{
			TriggerActionMiniUI component = this.triggerActionMiniPanel.GetComponent<TriggerActionMiniUI>();
			if (component != null)
			{
				this.openDetailPanelButton = component.openDetailPanelButton;
				this.removeButton = component.removeButton;
				this.duplicateButton = component.duplicateButton;
				this.previewTextJSON.text = component.previewTextPopupText;
				this.nameJSON.inputField = component.nameField;
				this.nameJSON.inputFieldAction = component.nameFieldAction;
				this.enabledJSON.toggle = component.enabledToggle;
			}
			if (this.openDetailPanelButton != null)
			{
				this.openDetailPanelButton.onClick.AddListener(new UnityAction(this.OpenDetailPanel));
			}
			if (this.removeButton != null)
			{
				this.removeButton.onClick.AddListener(new UnityAction(this.Remove));
			}
			if (this.duplicateButton != null)
			{
				this.duplicateButton.onClick.AddListener(new UnityAction(this.Duplicate));
			}
		}
	}

	// Token: 0x06006C1B RID: 27675 RVA: 0x0028AA80 File Offset: 0x00288E80
	public virtual void InitTriggerActionPanelUI()
	{
		if (this.triggerActionPanel != null)
		{
			TriggerActionUI component = this.triggerActionPanel.GetComponent<TriggerActionUI>();
			if (component != null)
			{
				this.closeDetailPanelButton = component.closeDetailPanelButton;
				this.receiverAtomPopup = component.receiverAtomPopup;
				this.receiverPopup = component.receiverPopup;
				this.receiverTargetNamePopup = component.receiverTargetNamePopup;
				this.nameJSON.inputFieldAlt = component.nameField;
				this.nameJSON.inputFieldActionAlt = component.nameFieldAction;
				this.enabledJSON.toggleAlt = component.enabledToggle;
			}
		}
		if (this.closeDetailPanelButton != null)
		{
			this.closeDetailPanelButton.onClick.AddListener(new UnityAction(this.CloseDetailPanel));
		}
		if (this.receiverAtomPopup != null)
		{
			if (this._receiverAtom == null)
			{
				this.receiverAtomPopup.currentValue = "None";
			}
			else
			{
				this.receiverAtomPopup.currentValue = this._receiverAtom.uid;
			}
			UIPopup uipopup = this.receiverAtomPopup;
			uipopup.onOpenPopupHandlers = (UIPopup.OnOpenPopup)Delegate.Combine(uipopup.onOpenPopupHandlers, new UIPopup.OnOpenPopup(this.SetReceiverAtomPopupValues));
			UIPopup uipopup2 = this.receiverAtomPopup;
			uipopup2.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup2.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetReceiverAtom));
		}
		if (this.receiverPopup != null)
		{
			this.SetReceiverPopupValues();
			if (this._receiver == null)
			{
				if (this._receiverStoreId == null)
				{
					this.receiverPopup.currentValue = "None";
				}
				else
				{
					this.receiverPopup.currentValue = this._receiverStoreId;
				}
			}
			else
			{
				this.receiverPopup.currentValue = this._receiver.storeId;
			}
			UIPopup uipopup3 = this.receiverPopup;
			uipopup3.onOpenPopupHandlers = (UIPopup.OnOpenPopup)Delegate.Combine(uipopup3.onOpenPopupHandlers, new UIPopup.OnOpenPopup(this.SetReceiverPopupValues));
			UIPopup uipopup4 = this.receiverPopup;
			uipopup4.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup4.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetReceiver));
		}
		if (this.receiverTargetNamePopup != null)
		{
			this.SetReceiverTargetPopupNames();
			if (this._receiverTargetName == null)
			{
				this.receiverTargetNamePopup.currentValue = "None";
			}
			else
			{
				this.receiverTargetNamePopup.currentValue = this._receiverTargetName;
			}
			UIPopup uipopup5 = this.receiverTargetNamePopup;
			uipopup5.onOpenPopupHandlers = (UIPopup.OnOpenPopup)Delegate.Combine(uipopup5.onOpenPopupHandlers, new UIPopup.OnOpenPopup(this.SetReceiverTargetPopupNames));
			UIPopup uipopup6 = this.receiverTargetNamePopup;
			uipopup6.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup6.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetReceiverTargetNameAndSetInitialParams));
		}
		this.SyncFromReceiverTarget();
	}

	// Token: 0x06006C1C RID: 27676 RVA: 0x0028AD48 File Offset: 0x00289148
	public virtual void DeregisterUI()
	{
		this.previewTextJSON.text = null;
		this.nameJSON.inputField = null;
		this.nameJSON.inputFieldAction = null;
		this.nameJSON.inputFieldAlt = null;
		this.nameJSON.inputFieldActionAlt = null;
		this.enabledJSON.toggle = null;
		this.enabledJSON.toggleAlt = null;
		if (this.openDetailPanelButton != null)
		{
			this.openDetailPanelButton.onClick.RemoveListener(new UnityAction(this.OpenDetailPanel));
		}
		if (this.closeDetailPanelButton != null)
		{
			this.closeDetailPanelButton.onClick.RemoveListener(new UnityAction(this.CloseDetailPanel));
		}
		if (this.removeButton != null)
		{
			this.removeButton.onClick.RemoveListener(new UnityAction(this.Remove));
		}
		if (this.duplicateButton != null)
		{
			this.duplicateButton.onClick.RemoveListener(new UnityAction(this.Duplicate));
		}
		if (this.receiverAtomPopup != null)
		{
			UIPopup uipopup = this.receiverAtomPopup;
			uipopup.onOpenPopupHandlers = (UIPopup.OnOpenPopup)Delegate.Remove(uipopup.onOpenPopupHandlers, new UIPopup.OnOpenPopup(this.SetReceiverAtomPopupValues));
			UIPopup uipopup2 = this.receiverAtomPopup;
			uipopup2.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Remove(uipopup2.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetReceiverAtom));
		}
		if (this.receiverPopup != null)
		{
			UIPopup uipopup3 = this.receiverPopup;
			uipopup3.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Remove(uipopup3.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetReceiver));
		}
		if (this.receiverTargetNamePopup != null)
		{
			UIPopup uipopup4 = this.receiverTargetNamePopup;
			uipopup4.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Remove(uipopup4.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetReceiverTargetNameAndSetInitialParams));
		}
	}

	// Token: 0x04005D83 RID: 23939
	public TriggerActionHandler handler;

	// Token: 0x04005D84 RID: 23940
	public RectTransform triggerActionMiniPanel;

	// Token: 0x04005D85 RID: 23941
	public RectTransform triggerActionPanel;

	// Token: 0x04005D86 RID: 23942
	protected JSONStorableString previewTextJSON;

	// Token: 0x04005D87 RID: 23943
	protected JSONStorableString nameJSON;

	// Token: 0x04005D88 RID: 23944
	protected JSONStorableBool enabledJSON;

	// Token: 0x04005D89 RID: 23945
	public Button openDetailPanelButton;

	// Token: 0x04005D8A RID: 23946
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private bool <detailPanelOpen>k__BackingField;

	// Token: 0x04005D8B RID: 23947
	public Button closeDetailPanelButton;

	// Token: 0x04005D8C RID: 23948
	public Button removeButton;

	// Token: 0x04005D8D RID: 23949
	public Button duplicateButton;

	// Token: 0x04005D8E RID: 23950
	public UIPopup receiverAtomPopup;

	// Token: 0x04005D8F RID: 23951
	protected Atom _receiverAtom;

	// Token: 0x04005D90 RID: 23952
	protected string _missingReceiverStoreid = string.Empty;

	// Token: 0x04005D91 RID: 23953
	public UIPopup receiverPopup;

	// Token: 0x04005D92 RID: 23954
	protected JSONStorable _receiver;

	// Token: 0x04005D93 RID: 23955
	protected string _receiverStoreId;

	// Token: 0x04005D94 RID: 23956
	protected List<string> receiverTargetNames;

	// Token: 0x04005D95 RID: 23957
	protected bool receiverSetFromPopup;

	// Token: 0x04005D96 RID: 23958
	public UIPopup receiverTargetNamePopup;

	// Token: 0x04005D97 RID: 23959
	protected string _receiverTargetName;

	// Token: 0x04005D98 RID: 23960
	protected JSONStorableFloat receiverTargetFloat;

	// Token: 0x04005D99 RID: 23961
	protected bool paramContrained;

	// Token: 0x04005D9A RID: 23962
	protected float paramDefault;

	// Token: 0x04005D9B RID: 23963
	protected float paramMin;

	// Token: 0x04005D9C RID: 23964
	protected float paramMax = 1f;
}
