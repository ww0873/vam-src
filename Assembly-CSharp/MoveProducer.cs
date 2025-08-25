using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000B6D RID: 2925
public class MoveProducer : JSONStorable
{
	// Token: 0x06005214 RID: 21012 RVA: 0x00144D06 File Offset: 0x00143106
	public MoveProducer()
	{
	}

	// Token: 0x06005215 RID: 21013 RVA: 0x00144D29 File Offset: 0x00143129
	public override string[] GetCustomParamNames()
	{
		return this.customParamNames;
	}

	// Token: 0x06005216 RID: 21014 RVA: 0x00144D34 File Offset: 0x00143134
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
					"Warning: MoveProducer in atom ",
					this.containingAtom,
					" uses receiver atom ",
					this._receiver.containingAtom.uid,
					" that is not in subscene and cannot be saved"
				}));
			}
		}
		return json;
	}

	// Token: 0x06005217 RID: 21015 RVA: 0x00144E14 File Offset: 0x00143214
	public override void RestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, JSONArray presetAtoms = null, bool setMissingToDefault = true)
	{
		base.RestoreFromJSON(jc, restorePhysical, restoreAppearance, presetAtoms, setMissingToDefault);
		if (!base.physicalLocked && restorePhysical && !base.IsCustomPhysicalParamLocked("receiver"))
		{
			if (jc["receiver"] != null)
			{
				string receiverByName = base.StoredAtomUidToAtomUid(jc["receiver"]);
				this.SetReceiverByName(receiverByName);
			}
			else if (setMissingToDefault)
			{
				this.SetReceiverByName(string.Empty);
			}
		}
	}

	// Token: 0x06005218 RID: 21016 RVA: 0x00144E9A File Offset: 0x0014329A
	protected void SyncOn(bool b)
	{
		this._on = b;
	}

	// Token: 0x17000BEC RID: 3052
	// (get) Token: 0x06005219 RID: 21017 RVA: 0x00144EA3 File Offset: 0x001432A3
	// (set) Token: 0x0600521A RID: 21018 RVA: 0x00144EAB File Offset: 0x001432AB
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

	// Token: 0x0600521B RID: 21019 RVA: 0x00144EDC File Offset: 0x001432DC
	protected virtual void SetControlOption(string option)
	{
		try
		{
			this._controlOption = (MoveProducer.ControlOption)Enum.Parse(typeof(MoveProducer.ControlOption), option, true);
		}
		catch (ArgumentException)
		{
		}
	}

	// Token: 0x17000BED RID: 3053
	// (get) Token: 0x0600521C RID: 21020 RVA: 0x00144F20 File Offset: 0x00143320
	// (set) Token: 0x0600521D RID: 21021 RVA: 0x00144F28 File Offset: 0x00143328
	public MoveProducer.ControlOption controlOption
	{
		get
		{
			return this._controlOption;
		}
		set
		{
			if (this.controlOptionJSON != null)
			{
				this.controlOptionJSON.val = value.ToString();
			}
			else if (this._controlOption != value)
			{
				this._controlOption = value;
			}
		}
	}

	// Token: 0x17000BEE RID: 3054
	// (get) Token: 0x0600521E RID: 21022 RVA: 0x00144F65 File Offset: 0x00143365
	// (set) Token: 0x0600521F RID: 21023 RVA: 0x00144F6D File Offset: 0x0014336D
	public virtual FreeControllerV3 receiver
	{
		get
		{
			return this._receiver;
		}
		set
		{
			this._receiver = value;
		}
	}

	// Token: 0x06005220 RID: 21024 RVA: 0x00144F76 File Offset: 0x00143376
	protected void OnAtomUIDRename(string fromid, string toid)
	{
		if (this.freeControllerAtomUID == fromid)
		{
			this.freeControllerAtomUID = toid;
			if (this.receiverAtomSelectionPopup != null)
			{
				this.receiverAtomSelectionPopup.currentValueNoCallback = toid;
			}
		}
	}

	// Token: 0x06005221 RID: 21025 RVA: 0x00144FB0 File Offset: 0x001433B0
	public virtual void SetReceiverAtom(string atomUID)
	{
		if (SuperController.singleton != null)
		{
			Atom atomByUid = SuperController.singleton.GetAtomByUid(atomUID);
			if (atomByUid != null)
			{
				this.freeControllerAtomUID = atomUID;
				List<string> freeControllerNamesInAtom = SuperController.singleton.GetFreeControllerNamesInAtom(this.freeControllerAtomUID);
				this.onFreeControllerNamesChanged(freeControllerNamesInAtom);
				if (this.receiverSelectionPopup != null)
				{
					this.receiverSelectionPopup.currentValue = "None";
				}
			}
			else
			{
				this.onFreeControllerNamesChanged(null);
			}
		}
	}

	// Token: 0x06005222 RID: 21026 RVA: 0x00145034 File Offset: 0x00143434
	public virtual void SetReceiverObject(string objectName)
	{
		if (this.freeControllerAtomUID != null && SuperController.singleton != null)
		{
			FreeControllerV3 receiver = SuperController.singleton.FreeControllerNameToFreeController(this.freeControllerAtomUID + ":" + objectName);
			this.receiver = receiver;
		}
	}

	// Token: 0x06005223 RID: 21027 RVA: 0x00145080 File Offset: 0x00143480
	public void SetReceiverByName(string controllerName)
	{
		if (SuperController.singleton != null)
		{
			FreeControllerV3 freeControllerV = SuperController.singleton.FreeControllerNameToFreeController(controllerName);
			if (freeControllerV != null)
			{
				if (this.receiverAtomSelectionPopup != null && freeControllerV.containingAtom != null)
				{
					this.receiverAtomSelectionPopup.currentValue = freeControllerV.containingAtom.uid;
				}
				if (this.receiverSelectionPopup != null)
				{
					this.receiverSelectionPopup.currentValue = freeControllerV.name;
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
			this.receiver = freeControllerV;
		}
	}

	// Token: 0x17000BEF RID: 3055
	// (get) Token: 0x06005224 RID: 21028 RVA: 0x0014515D File Offset: 0x0014355D
	public virtual Vector3 currentPosition
	{
		get
		{
			return this._currentPosition;
		}
	}

	// Token: 0x17000BF0 RID: 3056
	// (get) Token: 0x06005225 RID: 21029 RVA: 0x00145165 File Offset: 0x00143565
	public virtual Quaternion currentRotation
	{
		get
		{
			return this._currentRotation;
		}
	}

	// Token: 0x06005226 RID: 21030 RVA: 0x00145170 File Offset: 0x00143570
	protected virtual void SetReceiverAtomNames()
	{
		if (this.receiverAtomSelectionPopup != null && SuperController.singleton != null)
		{
			List<string> atomUIDsWithFreeControllers = SuperController.singleton.GetAtomUIDsWithFreeControllers();
			if (atomUIDsWithFreeControllers == null)
			{
				this.receiverAtomSelectionPopup.numPopupValues = 1;
				this.receiverAtomSelectionPopup.setPopupValue(0, "None");
			}
			else
			{
				this.receiverAtomSelectionPopup.numPopupValues = atomUIDsWithFreeControllers.Count + 1;
				this.receiverAtomSelectionPopup.setPopupValue(0, "None");
				for (int i = 0; i < atomUIDsWithFreeControllers.Count; i++)
				{
					this.receiverAtomSelectionPopup.setPopupValue(i + 1, atomUIDsWithFreeControllers[i]);
				}
			}
		}
	}

	// Token: 0x06005227 RID: 21031 RVA: 0x00145224 File Offset: 0x00143624
	protected void onFreeControllerNamesChanged(List<string> controllerNames)
	{
		if (this.receiverSelectionPopup != null)
		{
			if (controllerNames == null)
			{
				this.receiverSelectionPopup.numPopupValues = 1;
				this.receiverSelectionPopup.setPopupValue(0, "None");
			}
			else
			{
				this.receiverSelectionPopup.numPopupValues = controllerNames.Count + 1;
				this.receiverSelectionPopup.setPopupValue(0, "None");
				for (int i = 0; i < controllerNames.Count; i++)
				{
					this.receiverSelectionPopup.setPopupValue(i + 1, controllerNames[i]);
				}
			}
		}
	}

	// Token: 0x06005228 RID: 21032 RVA: 0x001452BC File Offset: 0x001436BC
	public void SelectReceiver(FreeControllerV3 rcvr)
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

	// Token: 0x06005229 RID: 21033 RVA: 0x00145342 File Offset: 0x00143742
	public void SelectControllerFromScene()
	{
		SuperController.singleton.SelectModeControllers(new SuperController.SelectControllerCallback(this.SelectReceiver));
	}

	// Token: 0x0600522A RID: 21034 RVA: 0x0014535C File Offset: 0x0014375C
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			MoveProducerUI componentInChildren = this.UITransform.GetComponentInChildren<MoveProducerUI>();
			if (componentInChildren != null)
			{
				this.onJSON.toggle = componentInChildren.onToggle;
				this.controlOptionJSON.popup = componentInChildren.controlOptionPopup;
				this.receiverAtomSelectionPopup = componentInChildren.receiverAtomSelectionPopup;
				this.receiverSelectionPopup = componentInChildren.receiverSelectionPopup;
				if (this.receiverAtomSelectionPopup != null)
				{
					if (this.receiver != null)
					{
						if (this.receiver.containingAtom != null)
						{
							this.SetReceiverAtomNames();
							this.SetReceiverAtom(this.receiver.containingAtom.uid);
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
					uipopup2.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup2.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetReceiverAtom));
				}
				if (this.receiverSelectionPopup != null)
				{
					if (this.receiver != null)
					{
						this.receiverSelectionPopup.currentValueNoCallback = this.receiver.name;
					}
					else
					{
						this.onFreeControllerNamesChanged(null);
						this.receiverSelectionPopup.currentValue = "None";
					}
					UIPopup uipopup3 = this.receiverSelectionPopup;
					uipopup3.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup3.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetReceiverObject));
				}
				if (componentInChildren.selectReceiverFromSceneButton != null)
				{
					componentInChildren.selectReceiverFromSceneButton.onClick.AddListener(new UnityAction(this.SelectControllerFromScene));
				}
			}
		}
	}

	// Token: 0x0600522B RID: 21035 RVA: 0x0014555E File Offset: 0x0014395E
	public override void InitUIAlt()
	{
	}

	// Token: 0x0600522C RID: 21036 RVA: 0x00145560 File Offset: 0x00143960
	protected virtual void Init()
	{
		if (SuperController.singleton != null)
		{
			SuperController singleton = SuperController.singleton;
			singleton.onAtomUIDRenameHandlers = (SuperController.OnAtomUIDRename)Delegate.Combine(singleton.onAtomUIDRenameHandlers, new SuperController.OnAtomUIDRename(this.OnAtomUIDRename));
		}
		this.onJSON = new JSONStorableBool("on", this._on, new JSONStorableBool.SetBoolCallback(this.SyncOn));
		this.onJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterBool(this.onJSON);
		string[] names = Enum.GetNames(typeof(MoveProducer.ControlOption));
		List<string> choicesList = new List<string>(names);
		this.controlOptionJSON = new JSONStorableStringChooser("controlOption", choicesList, this._controlOption.ToString(), "Control Option", new JSONStorableStringChooser.SetStringCallback(this.SetControlOption));
		this.controlOptionJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterStringChooser(this.controlOptionJSON);
	}

	// Token: 0x0600522D RID: 21037 RVA: 0x00145640 File Offset: 0x00143A40
	private void OnDestroy()
	{
		if (SuperController.singleton != null)
		{
			SuperController singleton = SuperController.singleton;
			singleton.onAtomUIDRenameHandlers = (SuperController.OnAtomUIDRename)Delegate.Remove(singleton.onAtomUIDRenameHandlers, new SuperController.OnAtomUIDRename(this.OnAtomUIDRename));
		}
	}

	// Token: 0x0600522E RID: 21038 RVA: 0x00145678 File Offset: 0x00143A78
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

	// Token: 0x0600522F RID: 21039 RVA: 0x0014569D File Offset: 0x00143A9D
	protected virtual void Start()
	{
		this.SetCurrentPositionAndRotation();
	}

	// Token: 0x06005230 RID: 21040 RVA: 0x001456A5 File Offset: 0x00143AA5
	protected virtual void SetCurrentPositionAndRotation()
	{
		this._currentPosition = base.transform.position;
		this._currentRotation = base.transform.rotation;
	}

	// Token: 0x06005231 RID: 21041 RVA: 0x001456CC File Offset: 0x00143ACC
	protected virtual void UpdateTransform()
	{
		if (this._on && (SuperController.singleton == null || !SuperController.singleton.freezeAnimation))
		{
			this.SetCurrentPositionAndRotation();
			if (this._receiver != null)
			{
				if ((this._controlOption == MoveProducer.ControlOption.Both || this._controlOption == MoveProducer.ControlOption.Position) && this._receiver.currentPositionState == FreeControllerV3.PositionState.On)
				{
					this._receiver.control.position = this._currentPosition;
				}
				if ((this._controlOption == MoveProducer.ControlOption.Both || this._controlOption == MoveProducer.ControlOption.Rotation) && this._receiver.currentRotationState == FreeControllerV3.RotationState.On)
				{
					this._receiver.control.rotation = this._currentRotation;
				}
			}
		}
	}

	// Token: 0x06005232 RID: 21042 RVA: 0x00145794 File Offset: 0x00143B94
	protected virtual void FixedUpdate()
	{
		this.UpdateTransform();
	}

	// Token: 0x06005233 RID: 21043 RVA: 0x0014579C File Offset: 0x00143B9C
	protected virtual void Update()
	{
		this.UpdateTransform();
	}

	// Token: 0x040041D1 RID: 16849
	protected string[] customParamNames = new string[]
	{
		"receiver"
	};

	// Token: 0x040041D2 RID: 16850
	protected JSONStorableBool onJSON;

	// Token: 0x040041D3 RID: 16851
	[SerializeField]
	protected bool _on = true;

	// Token: 0x040041D4 RID: 16852
	protected JSONStorableStringChooser controlOptionJSON;

	// Token: 0x040041D5 RID: 16853
	[SerializeField]
	protected MoveProducer.ControlOption _controlOption;

	// Token: 0x040041D6 RID: 16854
	[SerializeField]
	protected FreeControllerV3 _receiver;

	// Token: 0x040041D7 RID: 16855
	protected string freeControllerAtomUID;

	// Token: 0x040041D8 RID: 16856
	protected Vector3 _currentPosition;

	// Token: 0x040041D9 RID: 16857
	protected Quaternion _currentRotation;

	// Token: 0x040041DA RID: 16858
	public UIPopup receiverAtomSelectionPopup;

	// Token: 0x040041DB RID: 16859
	public UIPopup receiverSelectionPopup;

	// Token: 0x040041DC RID: 16860
	public Button selectReceiverFromSceneButton;

	// Token: 0x02000B6E RID: 2926
	public enum ControlOption
	{
		// Token: 0x040041DE RID: 16862
		Both,
		// Token: 0x040041DF RID: 16863
		Position,
		// Token: 0x040041E0 RID: 16864
		Rotation
	}
}
