using System;
using SimpleJSON;
using UnityEngine.UI;

// Token: 0x02000DB4 RID: 3508
public class UISliderTrigger : JSONStorableTriggerHandler
{
	// Token: 0x06006CBF RID: 27839 RVA: 0x00290877 File Offset: 0x0028EC77
	public UISliderTrigger()
	{
	}

	// Token: 0x06006CC0 RID: 27840 RVA: 0x00290880 File Offset: 0x0028EC80
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

	// Token: 0x06006CC1 RID: 27841 RVA: 0x002908E8 File Offset: 0x0028ECE8
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

	// Token: 0x06006CC2 RID: 27842 RVA: 0x00290993 File Offset: 0x0028ED93
	public override void Validate()
	{
		base.Validate();
		if (this.trigger != null)
		{
			this.trigger.Validate();
		}
	}

	// Token: 0x06006CC3 RID: 27843 RVA: 0x002909B1 File Offset: 0x0028EDB1
	protected void SyncFloat(float f)
	{
		if (this.trigger != null)
		{
			this.trigger.transitionInterpValue = this.floatJSON.val;
		}
	}

	// Token: 0x06006CC4 RID: 27844 RVA: 0x002909D4 File Offset: 0x0028EDD4
	protected void OnAtomRename(string oldid, string newid)
	{
		if (this.trigger != null)
		{
			this.trigger.SyncAtomNames();
		}
	}

	// Token: 0x06006CC5 RID: 27845 RVA: 0x002909EC File Offset: 0x0028EDEC
	protected virtual void CreateFloatJSON()
	{
		this.floatJSON = new JSONStorableFloat("value", 0f, new JSONStorableFloat.SetFloatCallback(this.SyncFloat), 0f, 1f, true, true);
		this.floatJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.floatJSON);
		if (this.slider != null)
		{
			this.floatJSON.RegisterSlider(this.slider, false);
		}
	}

	// Token: 0x06006CC6 RID: 27846 RVA: 0x00290A64 File Offset: 0x0028EE64
	protected void Init()
	{
		this.trigger = new Trigger();
		this.trigger.handler = this;
		this.trigger.active = true;
		this.CreateFloatJSON();
		if (SuperController.singleton)
		{
			SuperController singleton = SuperController.singleton;
			singleton.onAtomUIDRenameHandlers = (SuperController.OnAtomUIDRename)Delegate.Combine(singleton.onAtomUIDRenameHandlers, new SuperController.OnAtomUIDRename(this.OnAtomRename));
		}
	}

	// Token: 0x06006CC7 RID: 27847 RVA: 0x00290AD0 File Offset: 0x0028EED0
	public override void InitUI()
	{
		if (this.UITransform != null && this.trigger != null)
		{
			UISliderTriggerUI componentInChildren = this.UITransform.GetComponentInChildren<UISliderTriggerUI>();
			if (componentInChildren != null)
			{
				this.trigger.triggerActionsParent = componentInChildren.transform;
				this.trigger.triggerPanel = componentInChildren.transform;
				this.trigger.triggerActionsPanel = componentInChildren.transform;
				this.trigger.InitTriggerUI();
				this.trigger.InitTriggerActionsUI();
			}
		}
	}

	// Token: 0x06006CC8 RID: 27848 RVA: 0x00290B5A File Offset: 0x0028EF5A
	protected virtual void OnEnable()
	{
		if (this.trigger != null)
		{
			this.trigger.active = true;
		}
	}

	// Token: 0x06006CC9 RID: 27849 RVA: 0x00290B73 File Offset: 0x0028EF73
	protected virtual void OnDisable()
	{
		this.trigger.active = false;
	}

	// Token: 0x06006CCA RID: 27850 RVA: 0x00290B81 File Offset: 0x0028EF81
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			this.Init();
			this.InitUI();
		}
	}

	// Token: 0x06006CCB RID: 27851 RVA: 0x00290BA0 File Offset: 0x0028EFA0
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

	// Token: 0x04005E54 RID: 24148
	public Trigger trigger;

	// Token: 0x04005E55 RID: 24149
	public Slider slider;

	// Token: 0x04005E56 RID: 24150
	protected JSONStorableFloat floatJSON;
}
