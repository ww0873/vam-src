using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000D99 RID: 3481
public class CollisionTrigger : JSONStorableTriggerHandler
{
	// Token: 0x06006B48 RID: 27464 RVA: 0x00287FC8 File Offset: 0x002863C8
	public CollisionTrigger()
	{
	}

	// Token: 0x06006B49 RID: 27465 RVA: 0x00288004 File Offset: 0x00286404
	public override JSONClass GetJSON(bool includePhysical = true, bool includeAppearance = true, bool forceStore = false)
	{
		JSONClass json = base.GetJSON(includePhysical, includeAppearance, forceStore);
		if (includePhysical || forceStore)
		{
			if (this.trigger != null && (this.trigger.HasActions() || forceStore))
			{
				this.needsStore = true;
				json["trigger"] = this.trigger.GetJSON(base.subScenePrefix);
			}
			if (this.atomFilterUID != null && this.atomFilterUID != "None")
			{
				string text = base.AtomUidToStoreAtomUid(this.atomFilterUID);
				if (text != null)
				{
					this.needsStore = true;
					json["atomFilter"] = text;
				}
				else
				{
					SuperController.LogError(string.Concat(new object[]
					{
						"Warning: CollisionTrigger on atom ",
						this.containingAtom,
						" uses atom filter ",
						this.atomFilterUID,
						" which is outside of subscene and cannot be saved"
					}));
				}
			}
		}
		return json;
	}

	// Token: 0x06006B4A RID: 27466 RVA: 0x002880F8 File Offset: 0x002864F8
	public override void LateRestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, bool setMissingToDefault = true)
	{
		base.LateRestoreFromJSON(jc, restorePhysical, restoreAppearance, setMissingToDefault);
		if (!base.physicalLocked && restorePhysical)
		{
			if (!base.IsCustomPhysicalParamLocked("trigger"))
			{
				if (this.collisionTriggerEventHandler != null)
				{
					this.collisionTriggerEventHandler.Reset();
				}
				this.trigger.Reset();
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
			if (!base.IsCustomPhysicalParamLocked("atomFilter"))
			{
				if (jc["atomFilter"] != null)
				{
					string atomFilter = base.StoredAtomUidToAtomUid(jc["atomFilter"]);
					this.SetAtomFilter(atomFilter);
				}
				else if (setMissingToDefault)
				{
					this.SetAtomFilter("None");
				}
			}
		}
	}

	// Token: 0x06006B4B RID: 27467 RVA: 0x00288225 File Offset: 0x00286625
	public override void Validate()
	{
		base.Validate();
		if (this.trigger != null)
		{
			this.trigger.Validate();
		}
	}

	// Token: 0x06006B4C RID: 27468 RVA: 0x00288244 File Offset: 0x00286644
	public void ForceTrigger()
	{
		if (!this.trigger.active)
		{
			this.trigger.active = true;
			this.trigger.transitionInterpValue = 1f;
			this.trigger.active = false;
			this.trigger.transitionInterpValue = 0f;
		}
	}

	// Token: 0x06006B4D RID: 27469 RVA: 0x0028829C File Offset: 0x0028669C
	protected void SyncTriggerEnabled(bool b)
	{
		this._triggerEnabled = b;
		if (this._triggerEnabled)
		{
			this.collisionTriggerEventHandler = base.gameObject.AddComponent<CollisionTriggerEventHandler>();
			this.collisionTriggerEventHandler.collisionTrigger = this;
			this.collisionTriggerEventHandler.atomFilterUID = this.atomFilterUID;
			this.collisionTriggerEventHandler.invertAtomFilter = this._invertAtomFilter;
			this.collisionTriggerEventHandler.useRelativeVelocityFilter = this._useRelativeVelocityFilter;
			this.collisionTriggerEventHandler.invertRelativeVelocityFilter = this._invertRelativeVelocityFilter;
			this.collisionTriggerEventHandler.relativeVelocityFilter = this._relativeVelocityFilter;
			CollisionTriggerEventHandler collisionTriggerEventHandler = this.collisionTriggerEventHandler;
			collisionTriggerEventHandler.relativeVelocityHandlers = (CollisionTriggerEventHandler.RelativeVelocityCallback)Delegate.Combine(collisionTriggerEventHandler.relativeVelocityHandlers, new CollisionTriggerEventHandler.RelativeVelocityCallback(this.LastRelativeVelocityCallback));
			this.collisionTriggerEventHandler.Reset();
		}
		else if (this.collisionTriggerEventHandler != null)
		{
			CollisionTriggerEventHandler collisionTriggerEventHandler2 = this.collisionTriggerEventHandler;
			collisionTriggerEventHandler2.relativeVelocityHandlers = (CollisionTriggerEventHandler.RelativeVelocityCallback)Delegate.Remove(collisionTriggerEventHandler2.relativeVelocityHandlers, new CollisionTriggerEventHandler.RelativeVelocityCallback(this.LastRelativeVelocityCallback));
			UnityEngine.Object.Destroy(this.collisionTriggerEventHandler);
			this.collisionTriggerEventHandler = null;
		}
		if (this.collisionTriggerRenderer != null)
		{
			this.collisionTriggerRenderer.enabled = this._triggerEnabled;
		}
	}

	// Token: 0x17000FC7 RID: 4039
	// (get) Token: 0x06006B4E RID: 27470 RVA: 0x002883D0 File Offset: 0x002867D0
	// (set) Token: 0x06006B4F RID: 27471 RVA: 0x002883D8 File Offset: 0x002867D8
	public bool triggerEnabled
	{
		get
		{
			return this._triggerEnabled;
		}
		set
		{
			if (this.triggerEnabledJSON != null)
			{
				this.triggerEnabledJSON.val = value;
			}
			else
			{
				this.SyncTriggerEnabled(value);
			}
		}
	}

	// Token: 0x06006B50 RID: 27472 RVA: 0x00288400 File Offset: 0x00286800
	protected void SetAtomNames()
	{
		if (this.atomFilterPopup != null && SuperController.singleton != null)
		{
			List<string> atomUIDsWithRigidbodies = SuperController.singleton.GetAtomUIDsWithRigidbodies();
			if (atomUIDsWithRigidbodies == null)
			{
				this.atomFilterPopup.numPopupValues = 1;
				this.atomFilterPopup.setPopupValue(0, "None");
			}
			else
			{
				this.atomFilterPopup.numPopupValues = atomUIDsWithRigidbodies.Count + 1;
				this.atomFilterPopup.setPopupValue(0, "None");
				for (int i = 0; i < atomUIDsWithRigidbodies.Count; i++)
				{
					this.atomFilterPopup.setPopupValue(i + 1, atomUIDsWithRigidbodies[i]);
				}
			}
		}
	}

	// Token: 0x06006B51 RID: 27473 RVA: 0x002884B1 File Offset: 0x002868B1
	protected void SetAtomFilter(string atomUID)
	{
		this.atomFilterUID = atomUID;
	}

	// Token: 0x17000FC8 RID: 4040
	// (get) Token: 0x06006B52 RID: 27474 RVA: 0x002884BA File Offset: 0x002868BA
	// (set) Token: 0x06006B53 RID: 27475 RVA: 0x002884C4 File Offset: 0x002868C4
	protected string atomFilterUID
	{
		get
		{
			return this._atomFilterUID;
		}
		set
		{
			if (this._atomFilterUID != value)
			{
				this._atomFilterUID = value;
				if (this.atomFilterPopup != null)
				{
					this.atomFilterPopup.currentValueNoCallback = this.atomFilterUID;
				}
				if (this.collisionTriggerEventHandler != null)
				{
					this.collisionTriggerEventHandler.atomFilterUID = this._atomFilterUID;
				}
			}
		}
	}

	// Token: 0x06006B54 RID: 27476 RVA: 0x0028852D File Offset: 0x0028692D
	protected void SyncInvertAtomFilter(bool b)
	{
		this._invertAtomFilter = b;
		if (this.collisionTriggerEventHandler != null)
		{
			this.collisionTriggerEventHandler.invertAtomFilter = this._invertAtomFilter;
		}
	}

	// Token: 0x17000FC9 RID: 4041
	// (get) Token: 0x06006B55 RID: 27477 RVA: 0x00288558 File Offset: 0x00286958
	// (set) Token: 0x06006B56 RID: 27478 RVA: 0x00288560 File Offset: 0x00286960
	public bool invertAtomFilter
	{
		get
		{
			return this._invertAtomFilter;
		}
		set
		{
			if (this.invertAtomFilterJSON != null)
			{
				this.invertAtomFilterJSON.val = value;
			}
			else if (this._invertAtomFilter != value)
			{
				this.SyncInvertAtomFilter(value);
			}
		}
	}

	// Token: 0x06006B57 RID: 27479 RVA: 0x00288591 File Offset: 0x00286991
	protected void SyncUseRelativeVelocityFilter(bool b)
	{
		this._useRelativeVelocityFilter = b;
		if (this.collisionTriggerEventHandler != null)
		{
			this.collisionTriggerEventHandler.useRelativeVelocityFilter = this._useRelativeVelocityFilter;
		}
	}

	// Token: 0x17000FCA RID: 4042
	// (get) Token: 0x06006B58 RID: 27480 RVA: 0x002885BC File Offset: 0x002869BC
	// (set) Token: 0x06006B59 RID: 27481 RVA: 0x002885C4 File Offset: 0x002869C4
	public bool useRelativeVelocityFilter
	{
		get
		{
			return this._useRelativeVelocityFilter;
		}
		set
		{
			if (this.useRelativeVelocityFilterJSON != null)
			{
				this.useRelativeVelocityFilterJSON.val = value;
			}
			else if (this._useRelativeVelocityFilter != value)
			{
				this.SyncUseRelativeVelocityFilter(value);
			}
		}
	}

	// Token: 0x06006B5A RID: 27482 RVA: 0x002885F5 File Offset: 0x002869F5
	protected void SyncInvertRelativeVelocityFilter(bool b)
	{
		this._invertRelativeVelocityFilter = b;
		if (this.collisionTriggerEventHandler != null)
		{
			this.collisionTriggerEventHandler.invertRelativeVelocityFilter = this._invertRelativeVelocityFilter;
		}
	}

	// Token: 0x17000FCB RID: 4043
	// (get) Token: 0x06006B5B RID: 27483 RVA: 0x00288620 File Offset: 0x00286A20
	// (set) Token: 0x06006B5C RID: 27484 RVA: 0x00288628 File Offset: 0x00286A28
	public bool invertRelativeVelocityFilter
	{
		get
		{
			return this._invertRelativeVelocityFilter;
		}
		set
		{
			if (this.invertRelativeVelocityFilterJSON != null)
			{
				this.invertRelativeVelocityFilterJSON.val = value;
			}
			else if (this._invertRelativeVelocityFilter != value)
			{
				this.SyncInvertRelativeVelocityFilter(value);
			}
		}
	}

	// Token: 0x06006B5D RID: 27485 RVA: 0x00288659 File Offset: 0x00286A59
	protected void SyncRelativeVelocityFilter(float f)
	{
		this._relativeVelocityFilter = f;
		if (this.collisionTriggerEventHandler != null)
		{
			this.collisionTriggerEventHandler.relativeVelocityFilter = this._relativeVelocityFilter;
		}
	}

	// Token: 0x17000FCC RID: 4044
	// (get) Token: 0x06006B5E RID: 27486 RVA: 0x00288684 File Offset: 0x00286A84
	// (set) Token: 0x06006B5F RID: 27487 RVA: 0x0028868C File Offset: 0x00286A8C
	public float relativeVelocityFilter
	{
		get
		{
			return this._relativeVelocityFilter;
		}
		set
		{
			if (this.relativeVelocityFilterJSON != null)
			{
				this.relativeVelocityFilterJSON.val = value;
			}
			else if (this._relativeVelocityFilter != value)
			{
				this.SyncRelativeVelocityFilter(value);
			}
		}
	}

	// Token: 0x06006B60 RID: 27488 RVA: 0x002886BD File Offset: 0x00286ABD
	protected void LastRelativeVelocityCallback(float f)
	{
		this.lastRelativeVelocityJSON.val = f;
	}

	// Token: 0x06006B61 RID: 27489 RVA: 0x002886CB File Offset: 0x00286ACB
	protected void OnAtomRename(string oldid, string newid)
	{
		if (this.trigger != null)
		{
			this.trigger.SyncAtomNames();
		}
		if (this.atomFilterUID == oldid)
		{
			this.atomFilterUID = newid;
		}
	}

	// Token: 0x06006B62 RID: 27490 RVA: 0x002886FC File Offset: 0x00286AFC
	public bool hasBuiltInSoundAction()
	{
		return this.builtInTriggerSoundCategory != null && this.builtInTriggerSoundCategory != string.Empty && this.builtInTriggerSoundClip != null && this.builtInTriggerSoundClip != string.Empty;
	}

	// Token: 0x06006B63 RID: 27491 RVA: 0x0028874C File Offset: 0x00286B4C
	protected void SetupBuiltInTrigger()
	{
		if (this.hasBuiltInSoundAction())
		{
			TriggerActionDiscrete triggerActionDiscrete = this.trigger.CreateDiscreteActionStartInternal(-1);
			triggerActionDiscrete.receiverAtom = this.containingAtom;
			triggerActionDiscrete.name = "Built-In Audio";
			triggerActionDiscrete.SetReceiver("AudioSource");
			triggerActionDiscrete.SetReceiverTargetName("PlayNow");
			triggerActionDiscrete.SetAudioClipType("Embedded");
			triggerActionDiscrete.SetAudioClipCategory(this.builtInTriggerSoundCategory);
			triggerActionDiscrete.SetAudioClip(this.builtInTriggerSoundClip);
		}
	}

	// Token: 0x06006B64 RID: 27492 RVA: 0x002887C4 File Offset: 0x00286BC4
	protected void Init()
	{
		this.trigger = new Trigger();
		this.trigger.handler = this;
		this.triggerEnabledJSON = new JSONStorableBool("triggerEnabled", this._triggerEnabled, new JSONStorableBool.SetBoolCallback(this.SyncTriggerEnabled));
		this.triggerEnabledJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterBool(this.triggerEnabledJSON);
		this.invertAtomFilterJSON = new JSONStorableBool("invertAtomFilter", this._invertAtomFilter, new JSONStorableBool.SetBoolCallback(this.SyncInvertAtomFilter));
		this.invertAtomFilterJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterBool(this.invertAtomFilterJSON);
		this.useRelativeVelocityFilterJSON = new JSONStorableBool("useRelativeVelocityFilter", this._useRelativeVelocityFilter, new JSONStorableBool.SetBoolCallback(this.SyncUseRelativeVelocityFilter));
		this.useRelativeVelocityFilterJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterBool(this.useRelativeVelocityFilterJSON);
		this.invertRelativeVelocityFilterJSON = new JSONStorableBool("invertRelativeVelocityFilter", this._invertRelativeVelocityFilter, new JSONStorableBool.SetBoolCallback(this.SyncInvertRelativeVelocityFilter));
		this.invertRelativeVelocityFilterJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterBool(this.invertRelativeVelocityFilterJSON);
		this.relativeVelocityFilterJSON = new JSONStorableFloat("relativeVelocityFilter", this._relativeVelocityFilter, new JSONStorableFloat.SetFloatCallback(this.SyncRelativeVelocityFilter), 0f, 10f, false, true);
		this.relativeVelocityFilterJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.relativeVelocityFilterJSON);
		this.lastRelativeVelocityJSON = new JSONStorableFloat("lastRelativeVelocity", 0f, 0f, 10f, false, false);
		this.SyncTriggerEnabled(this._triggerEnabled);
		if (SuperController.singleton)
		{
			SuperController singleton = SuperController.singleton;
			singleton.onAtomUIDRenameHandlers = (SuperController.OnAtomUIDRename)Delegate.Combine(singleton.onAtomUIDRenameHandlers, new SuperController.OnAtomUIDRename(this.OnAtomRename));
		}
	}

	// Token: 0x06006B65 RID: 27493 RVA: 0x00288978 File Offset: 0x00286D78
	public override void InitUI()
	{
		if (this.UITransform != null && this.trigger != null)
		{
			CollisionTriggerUI componentInChildren = this.UITransform.GetComponentInChildren<CollisionTriggerUI>();
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
				this.trigger.triggerActionsParent = componentInChildren.transform;
				this.triggerEnabledJSON.toggle = componentInChildren.triggerEnabledToggle;
				this.atomFilterPopup = componentInChildren.atomFilterPopup;
				this.invertAtomFilterJSON.toggle = componentInChildren.invertAtomFilterToggle;
				if (this.trigger.triggerPanel == null)
				{
					this.trigger.triggerPanel = componentInChildren.transform;
				}
				this.trigger.triggerActionsPanel = componentInChildren.transform;
				this.trigger.InitTriggerUI();
				this.trigger.InitTriggerActionsUI();
				this.useRelativeVelocityFilterJSON.toggle = componentInChildren.useRelativeVelocityFilterToggle;
				this.invertRelativeVelocityFilterJSON.toggle = componentInChildren.invertRelativeVelocityFilterToggle;
				this.relativeVelocityFilterJSON.slider = componentInChildren.relativeVelocityFilterSlider;
				this.lastRelativeVelocityJSON.slider = componentInChildren.lastRelativeVelocitySlider;
			}
			if (this.atomFilterPopup != null)
			{
				this.atomFilterPopup.currentValue = this.atomFilterUID;
				UIPopup uipopup = this.atomFilterPopup;
				uipopup.onOpenPopupHandlers = (UIPopup.OnOpenPopup)Delegate.Combine(uipopup.onOpenPopupHandlers, new UIPopup.OnOpenPopup(this.SetAtomNames));
				UIPopup uipopup2 = this.atomFilterPopup;
				uipopup2.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup2.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetAtomFilter));
			}
		}
	}

	// Token: 0x06006B66 RID: 27494 RVA: 0x00288B59 File Offset: 0x00286F59
	protected void Update()
	{
		if (this.trigger != null)
		{
			this.trigger.Update();
		}
	}

	// Token: 0x06006B67 RID: 27495 RVA: 0x00288B71 File Offset: 0x00286F71
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			this.Init();
			this.InitUI();
		}
	}

	// Token: 0x06006B68 RID: 27496 RVA: 0x00288B90 File Offset: 0x00286F90
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

	// Token: 0x04005D1F RID: 23839
	public Trigger trigger;

	// Token: 0x04005D20 RID: 23840
	public MeshRenderer collisionTriggerRenderer;

	// Token: 0x04005D21 RID: 23841
	protected CollisionTriggerEventHandler collisionTriggerEventHandler;

	// Token: 0x04005D22 RID: 23842
	public JSONStorableBool triggerEnabledJSON;

	// Token: 0x04005D23 RID: 23843
	[SerializeField]
	protected bool _triggerEnabled = true;

	// Token: 0x04005D24 RID: 23844
	public UIPopup atomFilterPopup;

	// Token: 0x04005D25 RID: 23845
	protected string _atomFilterUID = "None";

	// Token: 0x04005D26 RID: 23846
	protected JSONStorableBool invertAtomFilterJSON;

	// Token: 0x04005D27 RID: 23847
	[SerializeField]
	protected bool _invertAtomFilter;

	// Token: 0x04005D28 RID: 23848
	protected JSONStorableBool useRelativeVelocityFilterJSON;

	// Token: 0x04005D29 RID: 23849
	[SerializeField]
	protected bool _useRelativeVelocityFilter;

	// Token: 0x04005D2A RID: 23850
	protected JSONStorableBool invertRelativeVelocityFilterJSON;

	// Token: 0x04005D2B RID: 23851
	[SerializeField]
	protected bool _invertRelativeVelocityFilter;

	// Token: 0x04005D2C RID: 23852
	protected JSONStorableFloat relativeVelocityFilterJSON;

	// Token: 0x04005D2D RID: 23853
	[SerializeField]
	protected float _relativeVelocityFilter = 1f;

	// Token: 0x04005D2E RID: 23854
	protected JSONStorableFloat lastRelativeVelocityJSON;

	// Token: 0x04005D2F RID: 23855
	public string builtInTriggerSoundCategory = string.Empty;

	// Token: 0x04005D30 RID: 23856
	public string builtInTriggerSoundClip = string.Empty;
}
