using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;

// Token: 0x02000D60 RID: 3424
public class ForceProducerV2 : JSONStorable
{
	// Token: 0x0600693E RID: 26942 RVA: 0x00274AF8 File Offset: 0x00272EF8
	public ForceProducerV2()
	{
	}

	// Token: 0x0600693F RID: 26943 RVA: 0x00274BB0 File Offset: 0x00272FB0
	public override string[] GetCustomParamNames()
	{
		return this.customParamNames;
	}

	// Token: 0x06006940 RID: 26944 RVA: 0x00274BB8 File Offset: 0x00272FB8
	public override JSONClass GetJSON(bool includePhysical = true, bool includeAppearance = true, bool forceStore = false)
	{
		JSONClass json = base.GetJSON(includePhysical, includeAppearance, forceStore);
		if ((includePhysical || forceStore) && this._receiver != null && this._receiver.containingAtom != null)
		{
			string text = base.AtomUidToStoreAtomUid(this._receiver.containingAtom.uid);
			if (text != null)
			{
				this.needsStore = true;
				json["receiver"] = text + ":" + this._receiver.name;
			}
			else
			{
				SuperController.LogError(string.Concat(new object[]
				{
					"Warning: ForceProducer in atom ",
					this.containingAtom,
					" uses receiver atom ",
					this._receiver.containingAtom.uid,
					" that is not in subscene and cannot be saved"
				}));
			}
		}
		return json;
	}

	// Token: 0x06006941 RID: 26945 RVA: 0x00274C98 File Offset: 0x00273098
	public override void RestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, JSONArray presetAtoms = null, bool setMissingToDefault = true)
	{
		base.RestoreFromJSON(jc, restorePhysical, restoreAppearance, presetAtoms, setMissingToDefault);
		if (!base.physicalLocked && restorePhysical && !base.IsCustomPhysicalParamLocked("receiver"))
		{
			if (jc["receiver"] != null)
			{
				string forceReceiver = base.StoredAtomUidToAtomUid(jc["receiver"]);
				this.SetForceReceiver(forceReceiver);
			}
			else if (setMissingToDefault)
			{
				this.SetForceReceiver(string.Empty);
			}
		}
	}

	// Token: 0x06006942 RID: 26946 RVA: 0x00274D1E File Offset: 0x0027311E
	protected void SyncOn(bool b)
	{
		this._on = b;
	}

	// Token: 0x17000F8B RID: 3979
	// (get) Token: 0x06006943 RID: 26947 RVA: 0x00274D27 File Offset: 0x00273127
	// (set) Token: 0x06006944 RID: 26948 RVA: 0x00274D2F File Offset: 0x0027312F
	public virtual bool on
	{
		get
		{
			return this._on;
		}
		set
		{
			if (this.onJSON != null)
			{
				this.onJSON.val = value;
			}
			else if (this._on != value)
			{
				this.SyncOn(value);
			}
		}
	}

	// Token: 0x06006945 RID: 26949 RVA: 0x00274D60 File Offset: 0x00273160
	protected virtual void SetReceiverAtomNames()
	{
		if (this.receiverAtomSelectionPopup != null && SuperController.singleton != null)
		{
			List<string> atomUIDsWithForceReceivers = SuperController.singleton.GetAtomUIDsWithForceReceivers();
			if (atomUIDsWithForceReceivers == null)
			{
				this.receiverAtomSelectionPopup.numPopupValues = 1;
				this.receiverAtomSelectionPopup.setPopupValue(0, "None");
			}
			else
			{
				this.receiverAtomSelectionPopup.numPopupValues = atomUIDsWithForceReceivers.Count + 1;
				this.receiverAtomSelectionPopup.setPopupValue(0, "None");
				for (int i = 0; i < atomUIDsWithForceReceivers.Count; i++)
				{
					this.receiverAtomSelectionPopup.setPopupValue(i + 1, atomUIDsWithForceReceivers[i]);
				}
			}
		}
	}

	// Token: 0x06006946 RID: 26950 RVA: 0x00274E14 File Offset: 0x00273214
	protected virtual void onReceiverNamesChanged(List<string> rcvrNames)
	{
		if (this.receiverSelectionPopup != null)
		{
			if (rcvrNames == null)
			{
				this.receiverSelectionPopup.numPopupValues = 1;
				this.receiverSelectionPopup.setPopupValue(0, "None");
			}
			else
			{
				this.receiverSelectionPopup.numPopupValues = rcvrNames.Count + 1;
				this.receiverSelectionPopup.setPopupValue(0, "None");
				for (int i = 0; i < rcvrNames.Count; i++)
				{
					this.receiverSelectionPopup.setPopupValue(i + 1, rcvrNames[i]);
				}
			}
		}
	}

	// Token: 0x06006947 RID: 26951 RVA: 0x00274EAA File Offset: 0x002732AA
	protected virtual void OnAtomUIDRename(string fromid, string toid)
	{
		if (this.receiverAtomUID == fromid)
		{
			this.receiverAtomUID = toid;
			if (this.receiverAtomSelectionPopup != null)
			{
				this.receiverAtomSelectionPopup.currentValueNoCallback = toid;
			}
		}
	}

	// Token: 0x06006948 RID: 26952 RVA: 0x00274EE4 File Offset: 0x002732E4
	public virtual void SetForceReceiverAtom(string atomUID)
	{
		if (SuperController.singleton != null)
		{
			Atom atomByUid = SuperController.singleton.GetAtomByUid(atomUID);
			if (atomByUid != null)
			{
				this.receiverAtomUID = atomUID;
				List<string> forceReceiverNamesInAtom = SuperController.singleton.GetForceReceiverNamesInAtom(this.receiverAtomUID);
				this.onReceiverNamesChanged(forceReceiverNamesInAtom);
				if (this.receiverSelectionPopup != null)
				{
					this.receiverSelectionPopup.currentValue = "None";
				}
			}
			else
			{
				this.onReceiverNamesChanged(null);
			}
		}
	}

	// Token: 0x06006949 RID: 26953 RVA: 0x00274F65 File Offset: 0x00273365
	public virtual void SetForceReceiverObject(string objectName)
	{
		if (this.receiverAtomUID != null && SuperController.singleton != null)
		{
			this.receiver = SuperController.singleton.ReceiverNameToForceReceiver(this.receiverAtomUID + ":" + objectName);
		}
	}

	// Token: 0x0600694A RID: 26954 RVA: 0x00274FA4 File Offset: 0x002733A4
	public virtual void SetForceReceiver(string receiverName)
	{
		if (SuperController.singleton != null)
		{
			ForceReceiver forceReceiver = SuperController.singleton.ReceiverNameToForceReceiver(receiverName);
			if (forceReceiver != null)
			{
				if (this.receiverAtomSelectionPopup != null && forceReceiver.containingAtom != null)
				{
					this.receiverAtomSelectionPopup.currentValue = forceReceiver.containingAtom.uid;
				}
				if (this.receiverSelectionPopup != null)
				{
					this.receiverSelectionPopup.currentValue = forceReceiver.name;
				}
			}
			else
			{
				if (this.receiverAtomSelectionPopup != null)
				{
					this.receiverAtomSelectionPopup.currentValue = "None";
				}
				if (this.receiverSelectionPopup != null)
				{
					this.receiverSelectionPopup.currentValue = "None";
				}
			}
			this.receiver = forceReceiver;
		}
	}

	// Token: 0x0600694B RID: 26955 RVA: 0x00275084 File Offset: 0x00273484
	public void SelectForceReceiver(ForceReceiver rcvr)
	{
		if (this.receiverAtomSelectionPopup != null && rcvr != null && rcvr.containingAtom != null)
		{
			this.receiverAtomSelectionPopup.currentValue = rcvr.containingAtom.uid;
		}
		if (this.receiverSelectionPopup != null && rcvr != null)
		{
			this.receiverSelectionPopup.currentValueNoCallback = rcvr.name;
		}
		this.receiver = rcvr;
	}

	// Token: 0x0600694C RID: 26956 RVA: 0x0027510A File Offset: 0x0027350A
	public void SelectForceReceiverFromScene()
	{
		this.SetReceiverAtomNames();
		SuperController.singleton.SelectModeForceReceivers(new SuperController.SelectForceReceiverCallback(this.SelectForceReceiver));
	}

	// Token: 0x17000F8C RID: 3980
	// (get) Token: 0x0600694D RID: 26957 RVA: 0x00275128 File Offset: 0x00273528
	// (set) Token: 0x0600694E RID: 26958 RVA: 0x00275130 File Offset: 0x00273530
	public virtual ForceReceiver receiver
	{
		get
		{
			return this._receiver;
		}
		set
		{
			this._receiver = value;
			if (this._receiver != null)
			{
				this.RB = this._receiver.GetComponent<Rigidbody>();
			}
			else
			{
				this.RB = null;
			}
		}
	}

	// Token: 0x0600694F RID: 26959 RVA: 0x00275167 File Offset: 0x00273567
	protected void SyncUseForce(bool b)
	{
		this._useForce = b;
	}

	// Token: 0x17000F8D RID: 3981
	// (get) Token: 0x06006950 RID: 26960 RVA: 0x00275170 File Offset: 0x00273570
	// (set) Token: 0x06006951 RID: 26961 RVA: 0x00275178 File Offset: 0x00273578
	public virtual bool useForce
	{
		get
		{
			return this._useForce;
		}
		set
		{
			if (this.useForceJSON != null)
			{
				this.useForceJSON.val = value;
			}
			else if (this._useForce != value)
			{
				this.SyncUseForce(value);
			}
		}
	}

	// Token: 0x06006952 RID: 26962 RVA: 0x002751A9 File Offset: 0x002735A9
	protected void SyncUseTorque(bool b)
	{
		this._useTorque = b;
	}

	// Token: 0x17000F8E RID: 3982
	// (get) Token: 0x06006953 RID: 26963 RVA: 0x002751B2 File Offset: 0x002735B2
	// (set) Token: 0x06006954 RID: 26964 RVA: 0x002751BA File Offset: 0x002735BA
	public virtual bool useTorque
	{
		get
		{
			return this._useTorque;
		}
		set
		{
			if (this.useTorqueJSON != null)
			{
				this.useTorqueJSON.val = value;
			}
			else if (this._useTorque != value)
			{
				this.SyncUseTorque(value);
			}
		}
	}

	// Token: 0x06006955 RID: 26965 RVA: 0x002751EB File Offset: 0x002735EB
	protected virtual void SyncForceFactor(float f)
	{
		this._forceFactor = f;
	}

	// Token: 0x17000F8F RID: 3983
	// (get) Token: 0x06006956 RID: 26966 RVA: 0x002751F4 File Offset: 0x002735F4
	// (set) Token: 0x06006957 RID: 26967 RVA: 0x002751FC File Offset: 0x002735FC
	public virtual float forceFactor
	{
		get
		{
			return this._forceFactor;
		}
		set
		{
			if (this.forceFactorJSON != null)
			{
				this.forceFactorJSON.val = value;
			}
			else if (this._forceFactor != value)
			{
				this.SyncForceFactor(value);
			}
		}
	}

	// Token: 0x06006958 RID: 26968 RVA: 0x0027522D File Offset: 0x0027362D
	protected void SyncMaxForce(float f)
	{
		this._maxForce = f;
	}

	// Token: 0x17000F90 RID: 3984
	// (get) Token: 0x06006959 RID: 26969 RVA: 0x00275236 File Offset: 0x00273636
	// (set) Token: 0x0600695A RID: 26970 RVA: 0x0027523E File Offset: 0x0027363E
	public virtual float maxForce
	{
		get
		{
			return this._maxForce;
		}
		set
		{
			if (this.maxForceJSON != null)
			{
				this.maxForceJSON.val = value;
			}
			else if (this._maxForce != value)
			{
				this.SyncMaxForce(value);
			}
		}
	}

	// Token: 0x0600695B RID: 26971 RVA: 0x0027526F File Offset: 0x0027366F
	protected virtual void SyncTorqueFactor(float f)
	{
		this._torqueFactor = f;
	}

	// Token: 0x17000F91 RID: 3985
	// (get) Token: 0x0600695C RID: 26972 RVA: 0x00275278 File Offset: 0x00273678
	// (set) Token: 0x0600695D RID: 26973 RVA: 0x00275280 File Offset: 0x00273680
	public virtual float torqueFactor
	{
		get
		{
			return this._torqueFactor;
		}
		set
		{
			if (this.torqueFactorJSON != null)
			{
				this.torqueFactorJSON.val = value;
			}
			else if (this._torqueFactor != value)
			{
				this.SyncTorqueFactor(value);
			}
		}
	}

	// Token: 0x0600695E RID: 26974 RVA: 0x002752B1 File Offset: 0x002736B1
	protected void SyncMaxTorque(float f)
	{
		this._maxTorque = f;
	}

	// Token: 0x17000F92 RID: 3986
	// (get) Token: 0x0600695F RID: 26975 RVA: 0x002752BA File Offset: 0x002736BA
	// (set) Token: 0x06006960 RID: 26976 RVA: 0x002752C2 File Offset: 0x002736C2
	public virtual float maxTorque
	{
		get
		{
			return this._maxTorque;
		}
		set
		{
			if (this.maxTorqueJSON != null)
			{
				this.maxTorqueJSON.val = value;
			}
			else if (this._maxTorque != value)
			{
				this.SyncMaxTorque(value);
			}
		}
	}

	// Token: 0x06006961 RID: 26977 RVA: 0x002752F3 File Offset: 0x002736F3
	protected void SyncForceQuickness(float f)
	{
		this._forceQuickness = f;
	}

	// Token: 0x17000F93 RID: 3987
	// (get) Token: 0x06006962 RID: 26978 RVA: 0x002752FC File Offset: 0x002736FC
	// (set) Token: 0x06006963 RID: 26979 RVA: 0x00275304 File Offset: 0x00273704
	public virtual float forceQuickness
	{
		get
		{
			return this._forceQuickness;
		}
		set
		{
			if (this.forceQuicknessJSON != null)
			{
				this.forceQuicknessJSON.val = value;
			}
			else if (this._forceQuickness != value)
			{
				this.SyncForceQuickness(value);
			}
		}
	}

	// Token: 0x06006964 RID: 26980 RVA: 0x00275335 File Offset: 0x00273735
	protected void SyncTorqueQuickness(float f)
	{
		this._torqueQuickness = f;
	}

	// Token: 0x17000F94 RID: 3988
	// (get) Token: 0x06006965 RID: 26981 RVA: 0x0027533E File Offset: 0x0027373E
	// (set) Token: 0x06006966 RID: 26982 RVA: 0x00275346 File Offset: 0x00273746
	public virtual float torqueQuickness
	{
		get
		{
			return this._torqueQuickness;
		}
		set
		{
			if (this.torqueQuicknessJSON != null)
			{
				this.torqueQuicknessJSON.val = value;
			}
			else if (this._torqueQuickness != value)
			{
				this.SyncTorqueQuickness(value);
			}
		}
	}

	// Token: 0x06006967 RID: 26983 RVA: 0x00275378 File Offset: 0x00273778
	protected virtual void Init()
	{
		this.onJSON = new JSONStorableBool("on", this._on, new JSONStorableBool.SetBoolCallback(this.SyncOn));
		this.onJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterBool(this.onJSON);
		this.useForceJSON = new JSONStorableBool("useForce", this._useForce, new JSONStorableBool.SetBoolCallback(this.SyncUseForce));
		this.useForceJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterBool(this.useForceJSON);
		this.useTorqueJSON = new JSONStorableBool("useTorque", this._useTorque, new JSONStorableBool.SetBoolCallback(this.SyncUseTorque));
		this.useTorqueJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterBool(this.useTorqueJSON);
		this.forceFactorJSON = new JSONStorableFloat("forceFactor", this._forceFactor, new JSONStorableFloat.SetFloatCallback(this.SyncForceFactor), 0f, 1000f, false, true);
		this.forceFactorJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.forceFactorJSON);
		this.maxForceJSON = new JSONStorableFloat("maxForce", this._maxForce, new JSONStorableFloat.SetFloatCallback(this.SyncMaxForce), 0f, 100f, false, true);
		this.maxForceJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.maxForceJSON);
		this.forceQuicknessJSON = new JSONStorableFloat("forceQuickness", this._forceQuickness, new JSONStorableFloat.SetFloatCallback(this.SyncForceQuickness), 0f, 50f, false, true);
		this.forceQuicknessJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.forceQuicknessJSON);
		this.torqueFactorJSON = new JSONStorableFloat("torqueFactor", this._torqueFactor, new JSONStorableFloat.SetFloatCallback(this.SyncTorqueFactor), 0f, 100f, false, true);
		this.torqueFactorJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.torqueFactorJSON);
		this.maxTorqueJSON = new JSONStorableFloat("maxTorque", this._maxTorque, new JSONStorableFloat.SetFloatCallback(this.SyncMaxTorque), 0f, 100f, false, true);
		this.maxTorqueJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.maxTorqueJSON);
		this.torqueQuicknessJSON = new JSONStorableFloat("torqueQuickness", this._torqueQuickness, new JSONStorableFloat.SetFloatCallback(this.SyncTorqueQuickness), 0f, 50f, false, true);
		this.torqueQuicknessJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.torqueQuicknessJSON);
		if (this.linkLineMaterial)
		{
			this.linkLineDrawer = new LineDrawer(this.linkLineMaterial);
		}
		if (this.forceLineMaterial)
		{
			this.forceLineDrawer = new LineDrawer(2, this.forceLineMaterial);
		}
		if (this.targetForceLineMaterial)
		{
			this.targetForceLineDrawer = new LineDrawer(6, this.targetForceLineMaterial);
		}
		if (this.rawForceLineMaterial)
		{
			this.rawForceLineDrawer = new LineDrawer(6, this.rawForceLineMaterial);
		}
		if (SuperController.singleton != null)
		{
			SuperController singleton = SuperController.singleton;
			singleton.onAtomUIDRenameHandlers = (SuperController.OnAtomUIDRename)Delegate.Combine(singleton.onAtomUIDRenameHandlers, new SuperController.OnAtomUIDRename(this.OnAtomUIDRename));
		}
	}

	// Token: 0x06006968 RID: 26984 RVA: 0x00275698 File Offset: 0x00273A98
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			ForceProducerV2UI componentInChildren = this.UITransform.GetComponentInChildren<ForceProducerV2UI>(true);
			if (componentInChildren != null)
			{
				this.onJSON.toggle = componentInChildren.onToggle;
				this.useForceJSON.toggle = componentInChildren.useForceToggle;
				this.useTorqueJSON.toggle = componentInChildren.useTorqueToggle;
				this.forceFactorJSON.slider = componentInChildren.forceFactorSlider;
				this.torqueFactorJSON.slider = componentInChildren.torqueFactorSlider;
				this.maxForceJSON.slider = componentInChildren.maxForceSlider;
				this.maxTorqueJSON.slider = componentInChildren.maxTorqueSlider;
				this.forceQuicknessJSON.slider = componentInChildren.forceQuicknessSlider;
				this.torqueQuicknessJSON.slider = componentInChildren.torqueQuicknessSlider;
				if (componentInChildren.selectReceiverAtomFromSceneButton != null)
				{
					componentInChildren.selectReceiverAtomFromSceneButton.onClick.AddListener(new UnityAction(this.SelectForceReceiverFromScene));
				}
				this.receiverAtomSelectionPopup = componentInChildren.receiverAtomSelectionPopup;
				this.receiverSelectionPopup = componentInChildren.receiverSelectionPopup;
				if (this.receiverAtomSelectionPopup != null)
				{
					if (this.receiver != null)
					{
						if (this.receiver.containingAtom != null)
						{
							this.SetForceReceiverAtom(this.receiver.containingAtom.uid);
							this.receiverAtomSelectionPopup.currentValue = this.receiver.containingAtom.uid;
						}
						else
						{
							this.receiverAtomSelectionPopup.currentValue = "None";
						}
					}
					else
					{
						this.receiverAtomSelectionPopup.currentValue = "None";
					}
					UIPopup uipopup = this.receiverAtomSelectionPopup;
					uipopup.onOpenPopupHandlers = (UIPopup.OnOpenPopup)Delegate.Combine(uipopup.onOpenPopupHandlers, new UIPopup.OnOpenPopup(this.SetReceiverAtomNames));
					UIPopup uipopup2 = this.receiverAtomSelectionPopup;
					uipopup2.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup2.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetForceReceiverAtom));
				}
				if (this.receiverSelectionPopup != null)
				{
					if (this.receiver != null)
					{
						this.receiverSelectionPopup.currentValueNoCallback = this.receiver.name;
					}
					else
					{
						this.onReceiverNamesChanged(null);
						this.receiverSelectionPopup.currentValue = "None";
					}
					UIPopup uipopup3 = this.receiverSelectionPopup;
					uipopup3.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup3.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetForceReceiverObject));
				}
			}
		}
	}

	// Token: 0x06006969 RID: 26985 RVA: 0x0027590C File Offset: 0x00273D0C
	public override void InitUIAlt()
	{
		if (this.UITransformAlt != null)
		{
			ForceProducerV2UI componentInChildren = this.UITransformAlt.GetComponentInChildren<ForceProducerV2UI>(true);
			if (componentInChildren != null)
			{
				this.onJSON.toggleAlt = componentInChildren.onToggle;
				this.useForceJSON.toggleAlt = componentInChildren.useForceToggle;
				this.useTorqueJSON.toggleAlt = componentInChildren.useTorqueToggle;
				this.forceFactorJSON.sliderAlt = componentInChildren.forceFactorSlider;
				this.torqueFactorJSON.sliderAlt = componentInChildren.torqueFactorSlider;
				this.maxForceJSON.sliderAlt = componentInChildren.maxForceSlider;
				this.maxTorqueJSON.sliderAlt = componentInChildren.maxTorqueSlider;
				this.forceQuicknessJSON.sliderAlt = componentInChildren.forceQuicknessSlider;
				this.torqueQuicknessJSON.sliderAlt = componentInChildren.torqueQuicknessSlider;
				if (componentInChildren.selectReceiverAtomFromSceneButton != null)
				{
					componentInChildren.selectReceiverAtomFromSceneButton.onClick.AddListener(new UnityAction(this.SelectForceReceiverFromScene));
				}
			}
		}
	}

	// Token: 0x0600696A RID: 26986 RVA: 0x00275A09 File Offset: 0x00273E09
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

	// Token: 0x0600696B RID: 26987 RVA: 0x00275A2E File Offset: 0x00273E2E
	private void OnDestroy()
	{
		if (SuperController.singleton != null)
		{
			SuperController singleton = SuperController.singleton;
			singleton.onAtomUIDRenameHandlers = (SuperController.OnAtomUIDRename)Delegate.Remove(singleton.onAtomUIDRenameHandlers, new SuperController.OnAtomUIDRename(this.OnAtomUIDRename));
		}
	}

	// Token: 0x0600696C RID: 26988 RVA: 0x00275A67 File Offset: 0x00273E67
	protected virtual void Start()
	{
		if (this._receiver)
		{
			this.RB = this._receiver.GetComponent<Rigidbody>();
		}
	}

	// Token: 0x0600696D RID: 26989 RVA: 0x00275A8C File Offset: 0x00273E8C
	protected virtual void SetTargetForcePercent(float forcePercent)
	{
		this.targetForcePercent = Mathf.Clamp(forcePercent, -1f, 1f);
		if (this.useForce)
		{
			this.rawForce = this.forceDirection * this.targetForcePercent * this._forceFactor;
			if (this.rawForce.magnitude > this._maxForce)
			{
				this.targetForce = Vector3.ClampMagnitude(this.rawForce, this._maxForce);
			}
			else
			{
				this.targetForce = this.rawForce;
			}
		}
		if (this.useTorque)
		{
			this.rawTorque = this.torqueDirection * this.targetForcePercent * this._torqueFactor;
			if (this.rawTorque.magnitude > this._maxTorque)
			{
				this.targetTorque = Vector3.ClampMagnitude(this.rawTorque, this._maxTorque);
			}
			else
			{
				this.targetTorque = this.rawTorque;
			}
		}
	}

	// Token: 0x0600696E RID: 26990 RVA: 0x00275B88 File Offset: 0x00273F88
	protected virtual void ApplyForce()
	{
		float fixedDeltaTime = Time.fixedDeltaTime;
		float d;
		if (TimeControl.singleton != null && TimeControl.singleton.compensateFixedTimestep)
		{
			if (!Mathf.Approximately(Time.timeScale, 0f))
			{
				d = 1f / Time.timeScale;
			}
			else
			{
				d = 1f;
			}
		}
		else
		{
			d = 1f;
		}
		this.currentForce = Vector3.Lerp(this.currentForce, this.targetForce, fixedDeltaTime * this._forceQuickness);
		this.currentTorque = Vector3.Lerp(this.currentTorque, this.targetTorque, fixedDeltaTime * this._torqueQuickness);
		if (this.RB && this.on && (!SuperController.singleton || !SuperController.singleton.freezeAnimation))
		{
			if (this.useForce)
			{
				this.appliedForce = this.currentForce * d;
				this.RB.AddForce(this.appliedForce, ForceMode.Force);
			}
			if (this.useTorque)
			{
				this.appliedTorque = this.currentTorque * d;
				this.RB.AddTorque(this.appliedTorque, ForceMode.Force);
				this.RB.maxAngularVelocity = 20f;
			}
		}
	}

	// Token: 0x0600696F RID: 26991 RVA: 0x00275CD8 File Offset: 0x002740D8
	protected virtual Vector3 AxisToVector(ForceProducerV2.AxisName axis)
	{
		Vector3 result;
		switch (axis)
		{
		case ForceProducerV2.AxisName.X:
			result = base.transform.right;
			break;
		case ForceProducerV2.AxisName.Y:
			result = base.transform.up;
			break;
		case ForceProducerV2.AxisName.Z:
			result = base.transform.forward;
			break;
		case ForceProducerV2.AxisName.NegX:
			result = -base.transform.right;
			break;
		case ForceProducerV2.AxisName.NegY:
			result = -base.transform.up;
			break;
		case ForceProducerV2.AxisName.NegZ:
			result = -base.transform.forward;
			break;
		default:
			result = Vector3.zero;
			break;
		}
		return result;
	}

	// Token: 0x06006970 RID: 26992 RVA: 0x00275D8C File Offset: 0x0027418C
	protected virtual Vector3 AxisToUpVector(ForceProducerV2.AxisName axis)
	{
		Vector3 result;
		switch (axis)
		{
		case ForceProducerV2.AxisName.X:
			result = base.transform.up;
			break;
		case ForceProducerV2.AxisName.Y:
			result = base.transform.forward;
			break;
		case ForceProducerV2.AxisName.Z:
			result = base.transform.up;
			break;
		case ForceProducerV2.AxisName.NegX:
			result = base.transform.up;
			break;
		case ForceProducerV2.AxisName.NegY:
			result = base.transform.forward;
			break;
		case ForceProducerV2.AxisName.NegZ:
			result = base.transform.up;
			break;
		default:
			result = Vector3.zero;
			break;
		}
		return result;
	}

	// Token: 0x06006971 RID: 26993 RVA: 0x00275E30 File Offset: 0x00274230
	protected virtual Vector3 getDrawTorque(Vector3 trq)
	{
		Quaternion rotation;
		switch (this.torqueAxis)
		{
		case ForceProducerV2.AxisName.X:
			rotation = Quaternion.FromToRotation(-base.transform.right, this.AxisToVector(this.forceAxis));
			break;
		case ForceProducerV2.AxisName.Y:
			rotation = Quaternion.FromToRotation(base.transform.up, this.AxisToVector(this.forceAxis));
			break;
		case ForceProducerV2.AxisName.Z:
			rotation = Quaternion.FromToRotation(base.transform.forward, this.AxisToVector(this.forceAxis));
			break;
		case ForceProducerV2.AxisName.NegX:
			rotation = Quaternion.FromToRotation(-base.transform.right, this.AxisToVector(this.forceAxis));
			break;
		case ForceProducerV2.AxisName.NegY:
			rotation = Quaternion.FromToRotation(-base.transform.up, this.AxisToVector(this.forceAxis));
			break;
		case ForceProducerV2.AxisName.NegZ:
			rotation = Quaternion.FromToRotation(-base.transform.forward, this.AxisToVector(this.forceAxis));
			break;
		default:
			rotation = Quaternion.identity;
			break;
		}
		return rotation * trq;
	}

	// Token: 0x06006972 RID: 26994 RVA: 0x00275F59 File Offset: 0x00274359
	protected virtual void FixedUpdate()
	{
		this.ApplyForce();
	}

	// Token: 0x06006973 RID: 26995 RVA: 0x00275F64 File Offset: 0x00274364
	protected virtual void Update()
	{
		if (this.useForce)
		{
			this.forceDirection = this.AxisToVector(this.forceAxis);
		}
		if (this.useTorque)
		{
			this.torqueDirection = this.AxisToVector(this.torqueAxis);
		}
		if (this._on && this._receiver != null && this.drawLines)
		{
			Vector3 a = this.AxisToVector(this.forceAxis);
			Vector3 drawTorque = this.getDrawTorque(this.AxisToVector(this.torqueAxis));
			Vector3 a2 = this.AxisToUpVector(this.forceAxis);
			if (this.linkLineDrawer != null)
			{
				this.linkLineDrawer.SetLinePoints(base.transform.position, this.receiver.transform.position);
				this.linkLineDrawer.Draw(base.gameObject.layer);
			}
			if (this.forceLineDrawer != null)
			{
				Vector3 vector = base.transform.position + a2 * this.lineOffset;
				this.forceLineDrawer.SetLinePoints(0, vector, vector + this.currentForce * this.linesScale);
				vector += a2 * this.lineSpacing * 5f;
				Vector3 drawTorque2 = this.getDrawTorque(this.currentTorque);
				this.forceLineDrawer.SetLinePoints(1, vector, vector + drawTorque2 * this.linesScale * this.torqueLineMult);
				this.targetForceLineDrawer.Draw(base.gameObject.layer);
				this.forceLineDrawer.Draw(base.gameObject.layer);
			}
			if (this.targetForceLineDrawer != null)
			{
				Vector3 vector2 = base.transform.position + a2 * (this.lineOffset + this.lineSpacing);
				this.targetForceLineDrawer.SetLinePoints(0, vector2, vector2 + this.targetForce * this.linesScale);
				Vector3 b = a * this._maxForce * this.linesScale;
				Vector3 a3 = vector2 + b;
				this.targetForceLineDrawer.SetLinePoints(1, a3 - a2 * this.lineSpacing, a3 + a2 * this.lineSpacing);
				a3 = vector2 - b;
				this.targetForceLineDrawer.SetLinePoints(2, a3 - a2 * this.lineSpacing, a3 + a2 * this.lineSpacing);
				vector2 += a2 * this.lineSpacing * 5f;
				Vector3 drawTorque3 = this.getDrawTorque(this.targetTorque);
				this.targetForceLineDrawer.SetLinePoints(3, vector2, vector2 + drawTorque3 * this.linesScale * this.torqueLineMult);
				b = drawTorque * this._maxTorque * this.linesScale * this.torqueLineMult;
				a3 = vector2 + b;
				this.targetForceLineDrawer.SetLinePoints(4, a3 - a2 * this.lineSpacing, a3 + a2 * this.lineSpacing);
				a3 = vector2 - b;
				this.targetForceLineDrawer.SetLinePoints(5, a3 - a2 * this.lineSpacing, a3 + a2 * this.lineSpacing);
				this.targetForceLineDrawer.Draw(base.gameObject.layer);
			}
			if (this.rawForceLineDrawer != null)
			{
				Vector3 vector3 = base.transform.position + a2 * (this.lineOffset + this.lineSpacing * 2f);
				this.rawForceLineDrawer.SetLinePoints(0, vector3, vector3 + this.rawForce * this.linesScale);
				Vector3 b2 = a * this._forceFactor * this.linesScale;
				Vector3 a4 = vector3 + b2;
				this.rawForceLineDrawer.SetLinePoints(1, a4 - a2 * this.lineSpacing, a4 + a2 * this.lineSpacing);
				a4 = vector3 - b2;
				this.rawForceLineDrawer.SetLinePoints(2, a4 - a2 * this.lineSpacing, a4 + a2 * this.lineSpacing);
				vector3 += a2 * this.lineSpacing * 5f;
				Vector3 drawTorque4 = this.getDrawTorque(this.rawTorque);
				this.rawForceLineDrawer.SetLinePoints(3, vector3, vector3 + drawTorque4 * this.linesScale * this.torqueLineMult);
				b2 = drawTorque * this._torqueFactor * this.linesScale * this.torqueLineMult;
				a4 = vector3 + b2;
				this.rawForceLineDrawer.SetLinePoints(4, a4 - a2 * this.lineSpacing, a4 + a2 * this.lineSpacing);
				a4 = vector3 - b2;
				this.rawForceLineDrawer.SetLinePoints(5, a4 - a2 * this.lineSpacing, a4 + a2 * this.lineSpacing);
				this.rawForceLineDrawer.Draw(base.gameObject.layer);
			}
		}
	}

	// Token: 0x04005A37 RID: 23095
	protected string[] customParamNames = new string[]
	{
		"receiver"
	};

	// Token: 0x04005A38 RID: 23096
	protected JSONStorableBool onJSON;

	// Token: 0x04005A39 RID: 23097
	[SerializeField]
	protected bool _on = true;

	// Token: 0x04005A3A RID: 23098
	protected string receiverAtomUID;

	// Token: 0x04005A3B RID: 23099
	[SerializeField]
	protected ForceReceiver _receiver;

	// Token: 0x04005A3C RID: 23100
	public ForceProducerV2.AxisName forceAxis;

	// Token: 0x04005A3D RID: 23101
	public ForceProducerV2.AxisName torqueAxis = ForceProducerV2.AxisName.Z;

	// Token: 0x04005A3E RID: 23102
	protected JSONStorableBool useForceJSON;

	// Token: 0x04005A3F RID: 23103
	[SerializeField]
	protected bool _useForce = true;

	// Token: 0x04005A40 RID: 23104
	protected JSONStorableBool useTorqueJSON;

	// Token: 0x04005A41 RID: 23105
	[SerializeField]
	protected bool _useTorque = true;

	// Token: 0x04005A42 RID: 23106
	protected JSONStorableFloat forceFactorJSON;

	// Token: 0x04005A43 RID: 23107
	[SerializeField]
	protected float _forceFactor = 200f;

	// Token: 0x04005A44 RID: 23108
	protected JSONStorableFloat maxForceJSON;

	// Token: 0x04005A45 RID: 23109
	[SerializeField]
	protected float _maxForce = 5000f;

	// Token: 0x04005A46 RID: 23110
	protected JSONStorableFloat torqueFactorJSON;

	// Token: 0x04005A47 RID: 23111
	[SerializeField]
	protected float _torqueFactor = 100f;

	// Token: 0x04005A48 RID: 23112
	protected JSONStorableFloat maxTorqueJSON;

	// Token: 0x04005A49 RID: 23113
	[SerializeField]
	protected float _maxTorque = 1000f;

	// Token: 0x04005A4A RID: 23114
	protected JSONStorableFloat forceQuicknessJSON;

	// Token: 0x04005A4B RID: 23115
	[SerializeField]
	protected float _forceQuickness = 10f;

	// Token: 0x04005A4C RID: 23116
	protected JSONStorableFloat torqueQuicknessJSON;

	// Token: 0x04005A4D RID: 23117
	[SerializeField]
	protected float _torqueQuickness = 10f;

	// Token: 0x04005A4E RID: 23118
	public Material linkLineMaterial;

	// Token: 0x04005A4F RID: 23119
	public Material forceLineMaterial;

	// Token: 0x04005A50 RID: 23120
	public Material targetForceLineMaterial;

	// Token: 0x04005A51 RID: 23121
	public Material rawForceLineMaterial;

	// Token: 0x04005A52 RID: 23122
	public bool drawLines = true;

	// Token: 0x04005A53 RID: 23123
	public float linesScale = 0.001f;

	// Token: 0x04005A54 RID: 23124
	public float lineOffset = 0.1f;

	// Token: 0x04005A55 RID: 23125
	public float lineSpacing = 0.01f;

	// Token: 0x04005A56 RID: 23126
	public UIPopup receiverAtomSelectionPopup;

	// Token: 0x04005A57 RID: 23127
	public UIPopup receiverSelectionPopup;

	// Token: 0x04005A58 RID: 23128
	public float targetForcePercent;

	// Token: 0x04005A59 RID: 23129
	public Vector3 appliedForce;

	// Token: 0x04005A5A RID: 23130
	public Vector3 appliedTorque;

	// Token: 0x04005A5B RID: 23131
	protected Vector3 forceDirection;

	// Token: 0x04005A5C RID: 23132
	protected Vector3 torqueDirection;

	// Token: 0x04005A5D RID: 23133
	protected Vector3 currentForce;

	// Token: 0x04005A5E RID: 23134
	protected Vector3 rawForce;

	// Token: 0x04005A5F RID: 23135
	protected Vector3 targetForce;

	// Token: 0x04005A60 RID: 23136
	protected Vector3 currentTorque;

	// Token: 0x04005A61 RID: 23137
	public Vector3 rawTorque;

	// Token: 0x04005A62 RID: 23138
	public Vector3 targetTorque;

	// Token: 0x04005A63 RID: 23139
	protected LineDrawer linkLineDrawer;

	// Token: 0x04005A64 RID: 23140
	protected LineDrawer forceLineDrawer;

	// Token: 0x04005A65 RID: 23141
	protected LineDrawer targetForceLineDrawer;

	// Token: 0x04005A66 RID: 23142
	protected LineDrawer rawForceLineDrawer;

	// Token: 0x04005A67 RID: 23143
	protected Rigidbody RB;

	// Token: 0x04005A68 RID: 23144
	protected float torqueLineMult = 10f;

	// Token: 0x02000D61 RID: 3425
	public enum AxisName
	{
		// Token: 0x04005A6A RID: 23146
		X,
		// Token: 0x04005A6B RID: 23147
		Y,
		// Token: 0x04005A6C RID: 23148
		Z,
		// Token: 0x04005A6D RID: 23149
		NegX,
		// Token: 0x04005A6E RID: 23150
		NegY,
		// Token: 0x04005A6F RID: 23151
		NegZ
	}
}
