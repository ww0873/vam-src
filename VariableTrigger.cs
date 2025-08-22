using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;

// Token: 0x02000DB8 RID: 3512
public class VariableTrigger : JSONStorableTriggerHandler
{
	// Token: 0x06006CDB RID: 27867 RVA: 0x001DC4E2 File Offset: 0x001DA8E2
	public VariableTrigger()
	{
	}

	// Token: 0x06006CDC RID: 27868 RVA: 0x001DC4F8 File Offset: 0x001DA8F8
	public override JSONClass GetJSON(bool includePhysical = true, bool includeAppearance = true, bool forceStore = false)
	{
		JSONClass json = base.GetJSON(includePhysical, includeAppearance, forceStore);
		if ((includePhysical || forceStore) && this.trigger != null && (this.trigger.HasActions() || forceStore))
		{
			this.needsStore = true;
			json["trigger"] = this.trigger.GetJSON(base.subScenePrefix);
		}
		return json;
	}

	// Token: 0x06006CDD RID: 27869 RVA: 0x001DC560 File Offset: 0x001DA960
	public override void LateRestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, bool setMissingToDefault = true)
	{
		base.LateRestoreFromJSON(jc, restorePhysical, restoreAppearance, setMissingToDefault);
		if (!base.physicalLocked && restorePhysical && !base.IsCustomPhysicalParamLocked("trigger"))
		{
			if (jc["trigger"] != null)
			{
				JSONClass asObject = jc["trigger"].AsObject;
				if (asObject != null)
				{
					this.trigger.RestoreFromJSON(asObject, base.subScenePrefix, base.mergeRestore);
				}
			}
			else if (setMissingToDefault && !base.mergeRestore)
			{
				this.trigger.RestoreFromJSON(new JSONClass());
			}
		}
	}

	// Token: 0x06006CDE RID: 27870 RVA: 0x001DC60B File Offset: 0x001DAA0B
	public override void Validate()
	{
		base.Validate();
		if (this.trigger != null)
		{
			this.trigger.Validate();
		}
	}

	// Token: 0x06006CDF RID: 27871 RVA: 0x001DC62C File Offset: 0x001DAA2C
	protected void SyncAtomChocies()
	{
		List<string> list = new List<string>();
		list.Add("None");
		foreach (string item in SuperController.singleton.GetVisibleAtomUIDs())
		{
			list.Add(item);
		}
		this.driverAtomJSON.choices = list;
	}

	// Token: 0x06006CE0 RID: 27872 RVA: 0x001DC6AC File Offset: 0x001DAAAC
	protected void SyncDriverAtom(string atomUID)
	{
		List<string> list = new List<string>();
		list.Add("None");
		if (atomUID != null)
		{
			this.driverAtom = SuperController.singleton.GetAtomByUid(atomUID);
			if (this.driverAtom != null)
			{
				foreach (string item in this.driverAtom.GetStorableIDs())
				{
					list.Add(item);
				}
			}
		}
		else
		{
			this.driverAtom = null;
		}
		this.driverJSON.choices = list;
		this.driverJSON.val = "None";
	}

	// Token: 0x06006CE1 RID: 27873 RVA: 0x001DC770 File Offset: 0x001DAB70
	protected void CheckMissingDriver()
	{
		if (this._missingDriverStoreId != string.Empty && this.driverAtom != null)
		{
			JSONStorable storableByID = this.driverAtom.GetStorableByID(this._missingDriverStoreId);
			if (storableByID != null)
			{
				string driverTargetName = this._driverTargetName;
				this.SyncDriver(this._missingDriverStoreId);
				this._missingDriverStoreId = string.Empty;
				this.insideRestore = true;
				this.driverTargetJSON.val = driverTargetName;
				this.insideRestore = false;
			}
		}
	}

	// Token: 0x06006CE2 RID: 27874 RVA: 0x001DC7FC File Offset: 0x001DABFC
	protected void SyncDriver(string driverID)
	{
		List<string> list = new List<string>();
		list.Add("None");
		if (this.driverAtom != null && driverID != null)
		{
			this.driver = this.driverAtom.GetStorableByID(driverID);
			if (this.driver != null)
			{
				foreach (string item in this.driver.GetFloatParamNames())
				{
					list.Add(item);
				}
			}
			else if (driverID != "None")
			{
				this._missingDriverStoreId = driverID;
			}
		}
		else
		{
			this.driver = null;
		}
		this.driverTargetJSON.choices = list;
		this.driverTargetJSON.val = "None";
	}

	// Token: 0x06006CE3 RID: 27875 RVA: 0x001DC8EC File Offset: 0x001DACEC
	protected void SyncDriverTarget(string driverTargetName)
	{
		this._driverTargetName = driverTargetName;
		this.driverTarget = null;
		if (this.driver != null && driverTargetName != null)
		{
			this.driverTarget = this.driver.GetFloatJSONParam(driverTargetName);
			if (this.driverTarget != null)
			{
				float val = this.driverStartValJSON.val;
				float val2 = this.driverEndValJSON.val;
				this.driverStartValJSON.constrained = this.driverTarget.constrained;
				this.driverEndValJSON.constrained = this.driverTarget.constrained;
				this.driverStartValJSON.min = this.driverTarget.min;
				this.driverStartValJSON.max = this.driverTarget.max;
				this.driverEndValJSON.min = this.driverTarget.min;
				this.driverEndValJSON.max = this.driverTarget.max;
				if (this.insideRestore)
				{
					this.driverStartValJSON.val = val;
					this.driverEndValJSON.val = val2;
				}
				else
				{
					this.driverStartValJSON.val = this.driverTarget.val;
					this.driverEndValJSON.val = this.driverTarget.val;
				}
				this.SyncDriverStartValSlider();
				this.SyncDriverEndValSlider();
			}
		}
	}

	// Token: 0x06006CE4 RID: 27876 RVA: 0x001DCA38 File Offset: 0x001DAE38
	protected void SyncDriverStartValSlider()
	{
		if (this.driverStartValDynamicSlider != null)
		{
			this.driverStartValDynamicSlider.rangeAdjustEnabled = this.driverStartValJSON.constrained;
			float max = this.driverStartValJSON.max;
			if (max <= 2f)
			{
				this.driverStartValDynamicSlider.valueFormat = "F3";
			}
			else if (max <= 20f)
			{
				this.driverStartValDynamicSlider.valueFormat = "F2";
			}
			else if (max <= 200f)
			{
				this.driverStartValDynamicSlider.valueFormat = "F1";
			}
			else
			{
				this.driverStartValDynamicSlider.valueFormat = "F0";
			}
			if (this.driverTarget != null)
			{
				this.driverStartValDynamicSlider.label = "(Start) " + this.driverTarget.name;
			}
			else
			{
				this.driverEndValDynamicSlider.label = "Driver Start Value";
			}
		}
	}

	// Token: 0x06006CE5 RID: 27877 RVA: 0x001DCB28 File Offset: 0x001DAF28
	protected void SyncDriverEndValSlider()
	{
		if (this.driverEndValDynamicSlider != null)
		{
			this.driverEndValDynamicSlider.rangeAdjustEnabled = this.driverEndValJSON.constrained;
			float max = this.driverEndValJSON.max;
			if (max <= 2f)
			{
				this.driverEndValDynamicSlider.valueFormat = "F3";
			}
			else if (max <= 20f)
			{
				this.driverEndValDynamicSlider.valueFormat = "F2";
			}
			else if (max <= 200f)
			{
				this.driverEndValDynamicSlider.valueFormat = "F1";
			}
			else
			{
				this.driverEndValDynamicSlider.valueFormat = "F0";
			}
			if (this.driverTarget != null)
			{
				this.driverEndValDynamicSlider.label = "(End) " + this.driverTarget.name;
			}
			else
			{
				this.driverEndValDynamicSlider.label = "Driver End Value";
			}
		}
	}

	// Token: 0x06006CE6 RID: 27878 RVA: 0x001DCC18 File Offset: 0x001DB018
	protected void SyncFloat(float f)
	{
		if (this.trigger != null)
		{
			this.trigger.transitionInterpValue = this.floatJSON.val;
		}
	}

	// Token: 0x06006CE7 RID: 27879 RVA: 0x001DCC3C File Offset: 0x001DB03C
	protected void OnAtomRename(string oldid, string newid)
	{
		if (this.trigger != null)
		{
			this.trigger.SyncAtomNames();
		}
		this.SyncAtomChocies();
		if (this.driverAtomJSON != null && this.driverAtom != null)
		{
			this.driverAtomJSON.valNoCallback = this.driverAtom.uid;
		}
	}

	// Token: 0x06006CE8 RID: 27880 RVA: 0x001DCC98 File Offset: 0x001DB098
	protected virtual void CreateFloatJSON()
	{
		this.floatJSON = new JSONStorableFloat("value", 0f, new JSONStorableFloat.SetFloatCallback(this.SyncFloat), 0f, 1f, true, true);
		this.floatJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.floatJSON);
	}

	// Token: 0x06006CE9 RID: 27881 RVA: 0x001DCCEC File Offset: 0x001DB0EC
	protected virtual void Init()
	{
		this.trigger = new Trigger();
		this.trigger.handler = this;
		this.trigger.active = true;
		this.CreateFloatJSON();
		this.driverAtomJSON = new JSONStorableStringChooser("driverAtom", SuperController.singleton.GetAtomUIDs(), null, "Driver Atom", new JSONStorableStringChooser.SetStringCallback(this.SyncDriverAtom));
		this.driverAtomJSON.representsAtomUid = true;
		this.driverAtomJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterStringChooser(this.driverAtomJSON);
		this.driverAtomJSON.popupOpenCallback = new JSONStorableStringChooser.PopupOpenCallback(this.SyncAtomChocies);
		this.driverJSON = new JSONStorableStringChooser("driver", null, null, "Driver", new JSONStorableStringChooser.SetStringCallback(this.SyncDriver));
		this.driverJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterStringChooser(this.driverJSON);
		this.driverTargetJSON = new JSONStorableStringChooser("driverTarget", null, null, "Driver Target", new JSONStorableStringChooser.SetStringCallback(this.SyncDriverTarget));
		this.driverTargetJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterStringChooser(this.driverTargetJSON);
		this.driverStartValJSON = new JSONStorableFloat("driverStartVal", 0f, 0f, 1f, false, true);
		this.driverStartValJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.driverStartValJSON);
		this.driverEndValJSON = new JSONStorableFloat("driverEndVal", 0f, 0f, 1f, false, true);
		this.driverEndValJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.driverEndValJSON);
		if (SuperController.singleton)
		{
			SuperController singleton = SuperController.singleton;
			singleton.onAtomUIDRenameHandlers = (SuperController.OnAtomUIDRename)Delegate.Combine(singleton.onAtomUIDRenameHandlers, new SuperController.OnAtomUIDRename(this.OnAtomRename));
		}
	}

	// Token: 0x06006CEA RID: 27882 RVA: 0x001DCEA8 File Offset: 0x001DB2A8
	public override void InitUI()
	{
		if (this.UITransform != null && this.trigger != null)
		{
			VariableTriggerUI componentInChildren = this.UITransform.GetComponentInChildren<VariableTriggerUI>(true);
			if (componentInChildren != null)
			{
				this.floatJSON.slider = componentInChildren.variableSlider;
				this.trigger.triggerActionsParent = componentInChildren.transform;
				this.trigger.triggerPanel = componentInChildren.transform;
				this.trigger.triggerActionsPanel = componentInChildren.transform;
				this.trigger.InitTriggerUI();
				this.trigger.InitTriggerActionsUI();
				this.driverAtomJSON.RegisterPopup(componentInChildren.driverAtomPopup, false);
				this.driverJSON.RegisterPopup(componentInChildren.driverPopup, false);
				this.driverTargetJSON.RegisterPopup(componentInChildren.driverTargetPopup, false);
				this.driverStartValJSON.RegisterSlider(componentInChildren.driverStartValSlider, false);
				this.driverEndValJSON.RegisterSlider(componentInChildren.driverEndValSlider, false);
				this.driverStartValDynamicSlider = componentInChildren.driverStartValDynamicSlider;
				this.driverEndValDynamicSlider = componentInChildren.driverEndValDynamicSlider;
				this.SyncDriverStartValSlider();
				this.SyncDriverEndValSlider();
			}
		}
	}

	// Token: 0x06006CEB RID: 27883 RVA: 0x001DCFC4 File Offset: 0x001DB3C4
	public override void InitUIAlt()
	{
		if (this.UITransformAlt != null)
		{
			VariableTriggerUI componentInChildren = this.UITransformAlt.GetComponentInChildren<VariableTriggerUI>(true);
			if (componentInChildren != null)
			{
				this.floatJSON.sliderAlt = componentInChildren.variableSlider;
			}
		}
	}

	// Token: 0x06006CEC RID: 27884 RVA: 0x001DD00C File Offset: 0x001DB40C
	protected virtual void OnEnable()
	{
		if (this.trigger != null)
		{
			this.trigger.active = true;
		}
	}

	// Token: 0x06006CED RID: 27885 RVA: 0x001DD025 File Offset: 0x001DB425
	protected virtual void OnDisable()
	{
		this.trigger.active = false;
	}

	// Token: 0x06006CEE RID: 27886 RVA: 0x001DD033 File Offset: 0x001DB433
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			this.Init();
			this.InitUI();
			this.InitUIAlt();
		}
	}

	// Token: 0x06006CEF RID: 27887 RVA: 0x001DD058 File Offset: 0x001DB458
	protected void LateUpdate()
	{
		this.CheckMissingDriver();
		if (this.driverTarget != null)
		{
			float num = this.driverEndValJSON.val - this.driverStartValJSON.val;
			if (num != 0f)
			{
				this.floatJSON.val = Mathf.Clamp01((this.driverTarget.val - this.driverStartValJSON.val) / num);
			}
			else
			{
				this.floatJSON.val = 0f;
			}
		}
	}

	// Token: 0x06006CF0 RID: 27888 RVA: 0x001DD0D8 File Offset: 0x001DB4D8
	protected void OnDestroy()
	{
		if (this.trigger != null)
		{
			this.trigger.Remove();
		}
		if (SuperController.singleton)
		{
			SuperController singleton = SuperController.singleton;
			singleton.onAtomUIDRenameHandlers = (SuperController.OnAtomUIDRename)Delegate.Remove(singleton.onAtomUIDRenameHandlers, new SuperController.OnAtomUIDRename(this.OnAtomRename));
		}
	}

	// Token: 0x04005E5A RID: 24154
	protected Atom driverAtom;

	// Token: 0x04005E5B RID: 24155
	protected JSONStorableStringChooser driverAtomJSON;

	// Token: 0x04005E5C RID: 24156
	protected string _missingDriverStoreId = string.Empty;

	// Token: 0x04005E5D RID: 24157
	protected JSONStorable driver;

	// Token: 0x04005E5E RID: 24158
	protected JSONStorableStringChooser driverJSON;

	// Token: 0x04005E5F RID: 24159
	protected string _driverTargetName;

	// Token: 0x04005E60 RID: 24160
	protected JSONStorableFloat driverTarget;

	// Token: 0x04005E61 RID: 24161
	protected JSONStorableStringChooser driverTargetJSON;

	// Token: 0x04005E62 RID: 24162
	protected UIDynamicSlider driverStartValDynamicSlider;

	// Token: 0x04005E63 RID: 24163
	protected JSONStorableFloat driverStartValJSON;

	// Token: 0x04005E64 RID: 24164
	protected UIDynamicSlider driverEndValDynamicSlider;

	// Token: 0x04005E65 RID: 24165
	protected JSONStorableFloat driverEndValJSON;

	// Token: 0x04005E66 RID: 24166
	public Trigger trigger;

	// Token: 0x04005E67 RID: 24167
	protected JSONStorableFloat floatJSON;
}
