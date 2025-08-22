using System;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000D8F RID: 3471
public class SyncForceProducerV2 : ForceProducerV2
{
	// Token: 0x06006B07 RID: 27399 RVA: 0x00284866 File Offset: 0x00282C66
	public SyncForceProducerV2()
	{
	}

	// Token: 0x06006B08 RID: 27400 RVA: 0x0028488A File Offset: 0x00282C8A
	public override string[] GetCustomParamNames()
	{
		return this.customParamNamesOverride;
	}

	// Token: 0x06006B09 RID: 27401 RVA: 0x00284894 File Offset: 0x00282C94
	public override JSONClass GetJSON(bool includePhysical = true, bool includeAppearance = true, bool forceStore = false)
	{
		JSONClass json = base.GetJSON(includePhysical, includeAppearance, forceStore);
		if ((includePhysical || forceStore) && this.syncProducer != null && this.syncProducer.containingAtom != null)
		{
			this.needsStore = true;
			json["syncTo"] = this.syncProducer.containingAtom.uid + ":" + this.syncProducer.name;
		}
		return json;
	}

	// Token: 0x06006B0A RID: 27402 RVA: 0x0028491C File Offset: 0x00282D1C
	public override void RestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, JSONArray presetAtoms = null, bool setMissingToDefault = true)
	{
		base.RestoreFromJSON(jc, restorePhysical, restoreAppearance, presetAtoms, setMissingToDefault);
		if (!base.physicalLocked && restorePhysical && !base.IsCustomPhysicalParamLocked("syncTo"))
		{
			if (jc["syncTo"] != null)
			{
				this.SetSyncProducer(jc["syncTo"]);
			}
			else if (setMissingToDefault)
			{
				this.SetSyncProducer(string.Empty);
			}
		}
	}

	// Token: 0x06006B0B RID: 27403 RVA: 0x0028499A File Offset: 0x00282D9A
	protected void SyncAutoSync(bool b)
	{
		this._autoSync = b;
	}

	// Token: 0x17000FC3 RID: 4035
	// (get) Token: 0x06006B0C RID: 27404 RVA: 0x002849A3 File Offset: 0x00282DA3
	// (set) Token: 0x06006B0D RID: 27405 RVA: 0x002849AB File Offset: 0x00282DAB
	public bool autoSync
	{
		get
		{
			return this._autoSync;
		}
		set
		{
			if (this.autoSyncJSON != null)
			{
				this.autoSyncJSON.val = value;
			}
			else if (this._autoSync != value)
			{
				this.SyncAutoSync(value);
			}
		}
	}

	// Token: 0x06006B0E RID: 27406 RVA: 0x002849DC File Offset: 0x00282DDC
	protected override void OnAtomUIDRename(string fromid, string toid)
	{
		base.OnAtomUIDRename(fromid, toid);
		if (this.syncProducer != null && this.syncProducerSelectionPopup != null)
		{
			this.syncProducerSelectionPopup.currentValueNoCallback = this.syncProducer.containingAtom.uid + ":" + this.syncProducer.name;
		}
	}

	// Token: 0x06006B0F RID: 27407 RVA: 0x00284A44 File Offset: 0x00282E44
	protected virtual void SetProducerNames()
	{
		if (this.syncProducerSelectionPopup != null && SuperController.singleton != null)
		{
			string[] forceProducerNames = SuperController.singleton.forceProducerNames;
			if (forceProducerNames == null)
			{
				this.syncProducerSelectionPopup.numPopupValues = 1;
				this.syncProducerSelectionPopup.setPopupValue(0, "None");
			}
			else
			{
				this.syncProducerSelectionPopup.numPopupValues = forceProducerNames.Length + 1;
				this.syncProducerSelectionPopup.setPopupValue(0, "None");
				for (int i = 0; i < forceProducerNames.Length; i++)
				{
					this.syncProducerSelectionPopup.setPopupValue(i + 1, forceProducerNames[i]);
				}
			}
		}
	}

	// Token: 0x06006B10 RID: 27408 RVA: 0x00284AEC File Offset: 0x00282EEC
	public void SetSyncProducer(string producerName)
	{
		if (SuperController.singleton != null)
		{
			ForceProducerV2 forceProducerV = SuperController.singleton.ProducerNameToForceProducer(producerName);
			this.syncProducer = forceProducerV;
			if (this.syncProducerSelectionPopup != null)
			{
				this.syncProducerSelectionPopup.currentValue = producerName;
			}
		}
	}

	// Token: 0x06006B11 RID: 27409 RVA: 0x00284B3C File Offset: 0x00282F3C
	public void SyncAllParameters()
	{
		if (this.syncProducer != null)
		{
			this.forceFactor = this.syncProducer.forceFactor;
			this.forceQuickness = this.syncProducer.forceQuickness;
			this.maxForce = this.syncProducer.maxForce;
			this.torqueFactor = this.syncProducer.torqueFactor;
			this.torqueQuickness = this.syncProducer.torqueQuickness;
			this.maxTorque = this.syncProducer.maxTorque;
		}
	}

	// Token: 0x06006B12 RID: 27410 RVA: 0x00284BC0 File Offset: 0x00282FC0
	protected override void Init()
	{
		base.Init();
		this.autoSyncJSON = new JSONStorableBool("autoSync", this._autoSync, new JSONStorableBool.SetBoolCallback(this.SyncAutoSync));
		this.autoSyncJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterBool(this.autoSyncJSON);
	}

	// Token: 0x06006B13 RID: 27411 RVA: 0x00284C10 File Offset: 0x00283010
	public override void InitUI()
	{
		base.InitUI();
		if (this.UITransform != null)
		{
			SyncForceProducerV2UI componentInChildren = this.UITransform.GetComponentInChildren<SyncForceProducerV2UI>(true);
			if (componentInChildren != null)
			{
				this.autoSyncJSON.toggle = componentInChildren.autoSyncToggle;
				if (componentInChildren.manualSyncButton != null)
				{
					componentInChildren.manualSyncButton.onClick.AddListener(new UnityAction(this.SyncAllParameters));
				}
				this.syncProducerSelectionPopup = componentInChildren.syncProducerSelectionPopup;
				if (this.syncProducerSelectionPopup != null)
				{
					if (this.syncProducer != null && this.syncProducer.containingAtom != null)
					{
						this.syncProducerSelectionPopup.currentValue = this.syncProducer.containingAtom.uid + ":" + this.syncProducer.name;
					}
					else
					{
						this.syncProducerSelectionPopup.currentValue = "None";
					}
					UIPopup uipopup = this.syncProducerSelectionPopup;
					uipopup.onOpenPopupHandlers = (UIPopup.OnOpenPopup)Delegate.Combine(uipopup.onOpenPopupHandlers, new UIPopup.OnOpenPopup(this.SetProducerNames));
					UIPopup uipopup2 = this.syncProducerSelectionPopup;
					uipopup2.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup2.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetSyncProducer));
				}
			}
		}
	}

	// Token: 0x06006B14 RID: 27412 RVA: 0x00284D64 File Offset: 0x00283164
	public override void InitUIAlt()
	{
		base.InitUIAlt();
		if (this.UITransformAlt != null)
		{
			SyncForceProducerV2UI componentInChildren = this.UITransformAlt.GetComponentInChildren<SyncForceProducerV2UI>(true);
			if (componentInChildren != null)
			{
				this.autoSyncJSON.toggleAlt = componentInChildren.autoSyncToggle;
				if (componentInChildren.manualSyncButton != null)
				{
					componentInChildren.manualSyncButton.onClick.AddListener(new UnityAction(this.SyncAllParameters));
				}
			}
		}
	}

	// Token: 0x06006B15 RID: 27413 RVA: 0x00284DDF File Offset: 0x002831DF
	protected override void Update()
	{
		if (this.syncProducer != null)
		{
			this.SetTargetForcePercent(this.syncProducer.targetForcePercent);
		}
		if (this.autoSync)
		{
			this.SyncAllParameters();
		}
		base.Update();
	}

	// Token: 0x06006B16 RID: 27414 RVA: 0x00284E1A File Offset: 0x0028321A
	protected override void Start()
	{
		base.Start();
	}

	// Token: 0x04005CE5 RID: 23781
	protected string[] customParamNamesOverride = new string[]
	{
		"receiver",
		"syncTo"
	};

	// Token: 0x04005CE6 RID: 23782
	public ForceProducerV2 syncProducer;

	// Token: 0x04005CE7 RID: 23783
	public UIPopup syncProducerSelectionPopup;

	// Token: 0x04005CE8 RID: 23784
	protected JSONStorableBool autoSyncJSON;

	// Token: 0x04005CE9 RID: 23785
	[SerializeField]
	protected bool _autoSync;
}
