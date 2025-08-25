using System;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000DA0 RID: 3488
public class LookAtTrigger : JSONStorableTriggerHandler
{
	// Token: 0x06006B8E RID: 27534 RVA: 0x002894EA File Offset: 0x002878EA
	public LookAtTrigger()
	{
	}

	// Token: 0x06006B8F RID: 27535 RVA: 0x00289508 File Offset: 0x00287908
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

	// Token: 0x06006B90 RID: 27536 RVA: 0x00289570 File Offset: 0x00287970
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

	// Token: 0x06006B91 RID: 27537 RVA: 0x0028961B File Offset: 0x00287A1B
	public void StartLookAt()
	{
		this.lookAtTime = this.lookPercent * this.activationTimeJSON.val;
		this.isLookingAt = true;
	}

	// Token: 0x06006B92 RID: 27538 RVA: 0x0028963C File Offset: 0x00287A3C
	public void EndLookAt()
	{
		this.lookAwayTime = (1f - this.lookPercent) * this.deactivationTimeJSON.val;
		this.isLookingAt = false;
	}

	// Token: 0x06006B93 RID: 27539 RVA: 0x00289663 File Offset: 0x00287A63
	protected void SyncActivationTime(float f)
	{
	}

	// Token: 0x06006B94 RID: 27540 RVA: 0x00289665 File Offset: 0x00287A65
	protected void SyncDeactivationTime(float f)
	{
	}

	// Token: 0x06006B95 RID: 27541 RVA: 0x00289667 File Offset: 0x00287A67
	protected void SyncAllowLookAwayDeactivation(bool b)
	{
	}

	// Token: 0x06006B96 RID: 27542 RVA: 0x00289669 File Offset: 0x00287A69
	public void ResetTrigger()
	{
		this.lookPercent = 0f;
		this.lookAtTime = 0f;
		this.lookAwayTime = 0f;
		this.trigger.active = false;
		this.trigger.transitionInterpValue = 0f;
	}

	// Token: 0x06006B97 RID: 27543 RVA: 0x002896A8 File Offset: 0x00287AA8
	protected void OnAtomRename(string oldid, string newid)
	{
		if (this.trigger != null)
		{
			this.trigger.SyncAtomNames();
		}
	}

	// Token: 0x06006B98 RID: 27544 RVA: 0x002896C0 File Offset: 0x00287AC0
	public bool hasBuiltInSoundAction()
	{
		return this.builtInTriggerSoundCategory != null && this.builtInTriggerSoundCategory != string.Empty && this.builtInTriggerSoundClip != null && this.builtInTriggerSoundClip != string.Empty;
	}

	// Token: 0x06006B99 RID: 27545 RVA: 0x00289710 File Offset: 0x00287B10
	protected void SetupBuiltInTrigger()
	{
		if (this.hasBuiltInSoundAction())
		{
			TriggerActionDiscrete triggerActionDiscrete = this.trigger.CreateDiscreteActionStartInternal(-1);
			triggerActionDiscrete.receiverAtom = this.containingAtom;
			triggerActionDiscrete.SetReceiver("AudioSource");
			triggerActionDiscrete.SetReceiverTargetName("PlayNow");
			triggerActionDiscrete.SetAudioClipType("Embedded");
			triggerActionDiscrete.SetAudioClipCategory(this.builtInTriggerSoundCategory);
			triggerActionDiscrete.SetAudioClip(this.builtInTriggerSoundClip);
		}
	}

	// Token: 0x06006B9A RID: 27546 RVA: 0x0028977C File Offset: 0x00287B7C
	protected void Init()
	{
		this.trigger = new Trigger();
		this.trigger.handler = this;
		this.activationTimeJSON = new JSONStorableFloat("activationTime", 0f, new JSONStorableFloat.SetFloatCallback(this.SyncActivationTime), 0f, 10f, false, true);
		this.activationTimeJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.activationTimeJSON);
		this.deactivationTimeJSON = new JSONStorableFloat("deactivationTime", 0f, new JSONStorableFloat.SetFloatCallback(this.SyncDeactivationTime), 0f, 10f, false, true);
		this.deactivationTimeJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.deactivationTimeJSON);
		this.allowLookAwayDeactivationJSON = new JSONStorableBool("allowLookAwayDeactivation", true, new JSONStorableBool.SetBoolCallback(this.SyncAllowLookAwayDeactivation));
		this.allowLookAwayDeactivationJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterBool(this.allowLookAwayDeactivationJSON);
		this.resetTriggerJSON = new JSONStorableAction("resetTrigger", new JSONStorableAction.ActionCallback(this.ResetTrigger));
		base.RegisterAction(this.resetTriggerJSON);
		if (SuperController.singleton)
		{
			SuperController singleton = SuperController.singleton;
			singleton.onAtomUIDRenameHandlers = (SuperController.OnAtomUIDRename)Delegate.Combine(singleton.onAtomUIDRenameHandlers, new SuperController.OnAtomUIDRename(this.OnAtomRename));
		}
	}

	// Token: 0x06006B9B RID: 27547 RVA: 0x002898BC File Offset: 0x00287CBC
	public override void InitUI()
	{
		if (this.UITransform != null && this.trigger != null)
		{
			LookAtTriggerUI componentInChildren = this.UITransform.GetComponentInChildren<LookAtTriggerUI>();
			if (componentInChildren != null)
			{
				if (componentInChildren.createDefaultSoundActionButton != null)
				{
					if (this.hasBuiltInSoundAction())
					{
						componentInChildren.createDefaultSoundActionButton.gameObject.SetActive(true);
						componentInChildren.createDefaultSoundActionButton.onClick.AddListener(new UnityAction(this.SetupBuiltInTrigger));
					}
					else
					{
						componentInChildren.createDefaultSoundActionButton.gameObject.SetActive(false);
					}
				}
				if (this.activationTimeJSON != null)
				{
					this.activationTimeJSON.slider = componentInChildren.activationTimeSlider;
				}
				if (this.deactivationTimeJSON != null)
				{
					this.deactivationTimeJSON.slider = componentInChildren.deactivationTimeSlider;
				}
				if (this.allowLookAwayDeactivationJSON != null)
				{
					this.allowLookAwayDeactivationJSON.toggle = componentInChildren.allowLookAwayDeactivationToggle;
				}
				this.trigger.triggerActionsParent = componentInChildren.transform;
				this.trigger.triggerPanel = componentInChildren.transform;
				this.trigger.triggerActionsPanel = componentInChildren.transform;
				this.trigger.InitTriggerUI();
				this.trigger.InitTriggerActionsUI();
			}
		}
	}

	// Token: 0x06006B9C RID: 27548 RVA: 0x002899F9 File Offset: 0x00287DF9
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			this.Init();
			this.InitUI();
		}
	}

	// Token: 0x06006B9D RID: 27549 RVA: 0x00289A18 File Offset: 0x00287E18
	protected void Update()
	{
		if (this.isLookingAt)
		{
			if (this.activationTimeJSON.val > 0f)
			{
				this.lookAtTime += Time.unscaledDeltaTime;
				this.lookPercent = Mathf.Clamp01(this.lookAtTime / this.activationTimeJSON.val);
			}
			else
			{
				this.lookPercent = 1f;
			}
			this.trigger.ForceTransitionsActive();
			this.trigger.transitionInterpValue = this.lookPercent;
			if (this.lookPercent == 1f)
			{
				this.trigger.active = true;
			}
		}
		else if (this.allowLookAwayDeactivationJSON.val)
		{
			if (this.deactivationTimeJSON.val > 0f)
			{
				if (this.lookAwayTime <= this.deactivationTimeJSON.val)
				{
					this.lookAwayTime += Time.unscaledDeltaTime;
					this.lookPercent = Mathf.Clamp01(1f - this.lookAwayTime / this.deactivationTimeJSON.val);
				}
				else
				{
					this.lookPercent = 0f;
				}
			}
			else
			{
				this.lookPercent = 0f;
			}
			this.trigger.transitionInterpValue = this.lookPercent;
			if (this.lookPercent == 0f)
			{
				this.trigger.active = false;
			}
		}
		this.trigger.Update();
	}

	// Token: 0x06006B9E RID: 27550 RVA: 0x00289B8C File Offset: 0x00287F8C
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

	// Token: 0x04005D4F RID: 23887
	public Trigger trigger;

	// Token: 0x04005D50 RID: 23888
	public float lookAtTime;

	// Token: 0x04005D51 RID: 23889
	public float lookAwayTime;

	// Token: 0x04005D52 RID: 23890
	public float lookPercent;

	// Token: 0x04005D53 RID: 23891
	protected bool isLookingAt;

	// Token: 0x04005D54 RID: 23892
	protected JSONStorableFloat activationTimeJSON;

	// Token: 0x04005D55 RID: 23893
	protected JSONStorableFloat deactivationTimeJSON;

	// Token: 0x04005D56 RID: 23894
	protected JSONStorableBool allowLookAwayDeactivationJSON;

	// Token: 0x04005D57 RID: 23895
	protected JSONStorableAction resetTriggerJSON;

	// Token: 0x04005D58 RID: 23896
	public string builtInTriggerSoundCategory = string.Empty;

	// Token: 0x04005D59 RID: 23897
	public string builtInTriggerSoundClip = string.Empty;
}
