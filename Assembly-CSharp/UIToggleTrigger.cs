using System;
using SimpleJSON;
using UnityEngine.UI;

// Token: 0x02000DB6 RID: 3510
public class UIToggleTrigger : JSONStorableTriggerHandler
{
	// Token: 0x06006CCD RID: 27853 RVA: 0x00290C00 File Offset: 0x0028F000
	public UIToggleTrigger()
	{
	}

	// Token: 0x06006CCE RID: 27854 RVA: 0x00290C08 File Offset: 0x0028F008
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

	// Token: 0x06006CCF RID: 27855 RVA: 0x00290C70 File Offset: 0x0028F070
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

	// Token: 0x06006CD0 RID: 27856 RVA: 0x00290D1B File Offset: 0x0028F11B
	public override void Validate()
	{
		base.Validate();
		if (this.trigger != null)
		{
			this.trigger.Validate();
		}
	}

	// Token: 0x06006CD1 RID: 27857 RVA: 0x00290D3C File Offset: 0x0028F13C
	protected void SyncBool(bool b)
	{
		if (this.trigger != null)
		{
			if (b)
			{
				this.trigger.reverse = false;
				this.trigger.active = true;
			}
			else
			{
				this.trigger.reverse = true;
				this.trigger.active = false;
			}
		}
	}

	// Token: 0x06006CD2 RID: 27858 RVA: 0x00290D8F File Offset: 0x0028F18F
	protected void OnAtomRename(string oldid, string newid)
	{
		if (this.trigger != null)
		{
			this.trigger.SyncAtomNames();
		}
	}

	// Token: 0x06006CD3 RID: 27859 RVA: 0x00290DA8 File Offset: 0x0028F1A8
	protected virtual void CreateBoolJSON()
	{
		this.boolJSON = new JSONStorableBool("value", false, new JSONStorableBool.SetBoolCallback(this.SyncBool));
		this.boolJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterBool(this.boolJSON);
		if (this.toggle != null)
		{
			this.boolJSON.RegisterToggle(this.toggle, false);
		}
	}

	// Token: 0x06006CD4 RID: 27860 RVA: 0x00290E10 File Offset: 0x0028F210
	protected void Init()
	{
		this.trigger = new Trigger();
		this.trigger.handler = this;
		this.CreateBoolJSON();
		if (SuperController.singleton)
		{
			SuperController singleton = SuperController.singleton;
			singleton.onAtomUIDRenameHandlers = (SuperController.OnAtomUIDRename)Delegate.Combine(singleton.onAtomUIDRenameHandlers, new SuperController.OnAtomUIDRename(this.OnAtomRename));
		}
	}

	// Token: 0x06006CD5 RID: 27861 RVA: 0x00290E70 File Offset: 0x0028F270
	public override void InitUI()
	{
		if (this.UITransform != null && this.trigger != null)
		{
			UIToggleTriggerUI componentInChildren = this.UITransform.GetComponentInChildren<UIToggleTriggerUI>();
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

	// Token: 0x06006CD6 RID: 27862 RVA: 0x00290EFA File Offset: 0x0028F2FA
	protected virtual void OnEnable()
	{
		if (this.boolJSON != null)
		{
			this.SyncBool(this.boolJSON.val);
		}
	}

	// Token: 0x06006CD7 RID: 27863 RVA: 0x00290F18 File Offset: 0x0028F318
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			this.Init();
			this.InitUI();
		}
	}

	// Token: 0x06006CD8 RID: 27864 RVA: 0x00290F37 File Offset: 0x0028F337
	protected void Update()
	{
		if (this.trigger != null)
		{
			this.trigger.Update();
		}
	}

	// Token: 0x06006CD9 RID: 27865 RVA: 0x00290F50 File Offset: 0x0028F350
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

	// Token: 0x04005E57 RID: 24151
	public Trigger trigger;

	// Token: 0x04005E58 RID: 24152
	public Toggle toggle;

	// Token: 0x04005E59 RID: 24153
	protected JSONStorableBool boolJSON;
}
