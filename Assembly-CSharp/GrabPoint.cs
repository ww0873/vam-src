using System;
using System.Collections.Generic;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000D69 RID: 3433
public class GrabPoint : JSONStorable
{
	// Token: 0x06006984 RID: 27012 RVA: 0x00278005 File Offset: 0x00276405
	public GrabPoint()
	{
	}

	// Token: 0x06006985 RID: 27013 RVA: 0x00278010 File Offset: 0x00276410
	public override JSONClass GetJSON(bool includePhysical = true, bool includeAppearance = true, bool forceStore = false)
	{
		JSONClass json = base.GetJSON(includePhysical, includeAppearance, forceStore);
		if ((includePhysical || forceStore) && this._receiver != null && this._receiver.containingAtom != null)
		{
			string text = base.AtomUidToStoreAtomUid(this._receiver.containingAtom.uid);
			if (text != null)
			{
				this.needsStore = true;
				json["controlled"] = text + ":" + this._receiver.name;
			}
			else
			{
				SuperController.LogError(string.Concat(new object[]
				{
					"Warning: GrabPoint in atom ",
					this.containingAtom,
					" uses receiver atom ",
					this._receiver.containingAtom.uid,
					" that is not in subscene and cannot be saved"
				}));
			}
		}
		return json;
	}

	// Token: 0x06006986 RID: 27014 RVA: 0x002780F0 File Offset: 0x002764F0
	public override void LateRestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, bool setMissingToDefault = true)
	{
		base.LateRestoreFromJSON(jc, restorePhysical, restoreAppearance, setMissingToDefault);
		if (!base.physicalLocked && restorePhysical && !base.IsCustomPhysicalParamLocked("controlled"))
		{
			this.SetReceiverAtomNames();
			if (jc["controlled"] != null)
			{
				string forceReceiver = base.StoredAtomUidToAtomUid(jc["controlled"]);
				this.SetForceReceiver(forceReceiver);
			}
			else if (setMissingToDefault)
			{
				this.SetForceReceiver(string.Empty);
			}
		}
	}

	// Token: 0x06006987 RID: 27015 RVA: 0x0027817C File Offset: 0x0027657C
	protected void SyncGrabIcon()
	{
		if (this.controller != null)
		{
			if (this._grabIconType == GrabPoint.GrabIconType.Hand && this.handIconMesh != null)
			{
				this.controller.deselectedMesh = this.handIconMesh;
			}
			if (this._grabIconType == GrabPoint.GrabIconType.Head && this.headIconMesh != null)
			{
				this.controller.deselectedMesh = this.headIconMesh;
			}
		}
	}

	// Token: 0x06006988 RID: 27016 RVA: 0x002781F8 File Offset: 0x002765F8
	public void SetGrabIconType(string type)
	{
		try
		{
			GrabPoint.GrabIconType grabIconType = (GrabPoint.GrabIconType)Enum.Parse(typeof(GrabPoint.GrabIconType), type);
			this._grabIconType = grabIconType;
			this.SyncGrabIcon();
		}
		catch (ArgumentException)
		{
			Debug.LogError("Attempted to set grab icon type to " + type + " which is not a valid grab icon type");
		}
	}

	// Token: 0x17000F97 RID: 3991
	// (get) Token: 0x06006989 RID: 27017 RVA: 0x00278258 File Offset: 0x00276658
	// (set) Token: 0x0600698A RID: 27018 RVA: 0x00278260 File Offset: 0x00276660
	public GrabPoint.GrabIconType grabIconType
	{
		get
		{
			return this._grabIconType;
		}
		set
		{
			if (this.grabIconTypeJSON != null)
			{
				this.grabIconTypeJSON.val = value.ToString();
			}
			else if (this._grabIconType != value)
			{
				this._grabIconType = value;
				this.SyncGrabIcon();
			}
		}
	}

	// Token: 0x17000F98 RID: 3992
	// (get) Token: 0x0600698B RID: 27019 RVA: 0x002782AE File Offset: 0x002766AE
	// (set) Token: 0x0600698C RID: 27020 RVA: 0x002782B8 File Offset: 0x002766B8
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
				Rigidbody component = this._receiver.GetComponent<Rigidbody>();
				if (this.joint != null)
				{
					UnityEngine.Object.Destroy(this.joint);
					this.joint = null;
				}
				if (this.rigidbodyToConnect != null)
				{
					this.joint = this.rigidbodyToConnect.gameObject.AddComponent<FixedJoint>();
					this.joint.connectedBody = component;
				}
			}
			else if (this.joint != null)
			{
				UnityEngine.Object.Destroy(this.joint);
				this.joint = null;
			}
		}
	}

	// Token: 0x0600698D RID: 27021 RVA: 0x00278368 File Offset: 0x00276768
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

	// Token: 0x0600698E RID: 27022 RVA: 0x0027841C File Offset: 0x0027681C
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

	// Token: 0x0600698F RID: 27023 RVA: 0x002784B2 File Offset: 0x002768B2
	protected void OnAtomUIDRename(string fromid, string toid)
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

	// Token: 0x06006990 RID: 27024 RVA: 0x002784EC File Offset: 0x002768EC
	public virtual void SetForceReceiverAtom(string atomUID)
	{
		if (SuperController.singleton != null)
		{
			Atom atomByUid = SuperController.singleton.GetAtomByUid(atomUID);
			this.receiver = null;
			if (atomByUid != null)
			{
				this.receiverAtomUID = atomUID;
				this.UpdateReceiverNames();
				this.receiverSelectionPopup.currentValue = "None";
			}
			else
			{
				this.receiverAtomUID = string.Empty;
				this.UpdateReceiverNames();
			}
		}
	}

	// Token: 0x06006991 RID: 27025 RVA: 0x0027855C File Offset: 0x0027695C
	protected void UpdateReceiverNames()
	{
		if (this.receiverAtomUID != null && this.receiverAtomUID != string.Empty)
		{
			List<string> forceReceiverNamesInAtom = SuperController.singleton.GetForceReceiverNamesInAtom(this.receiverAtomUID);
			this.onReceiverNamesChanged(forceReceiverNamesInAtom);
		}
		else
		{
			this.onReceiverNamesChanged(null);
		}
	}

	// Token: 0x06006992 RID: 27026 RVA: 0x002785B0 File Offset: 0x002769B0
	public virtual void SetForceReceiverObject(string objectName)
	{
		if (this.receiverAtomUID != null && SuperController.singleton != null)
		{
			this.receiver = SuperController.singleton.ReceiverNameToForceReceiver(this.receiverAtomUID + ":" + objectName);
		}
		else
		{
			this.receiver = null;
		}
	}

	// Token: 0x06006993 RID: 27027 RVA: 0x00278608 File Offset: 0x00276A08
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

	// Token: 0x06006994 RID: 27028 RVA: 0x002786E8 File Offset: 0x00276AE8
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

	// Token: 0x06006995 RID: 27029 RVA: 0x0027876E File Offset: 0x00276B6E
	public void SelectForceReceiverFromScene()
	{
		this.SetReceiverAtomNames();
		SuperController.singleton.SelectModeForceReceivers(new SuperController.SelectForceReceiverCallback(this.SelectForceReceiver));
	}

	// Token: 0x06006996 RID: 27030 RVA: 0x0027878C File Offset: 0x00276B8C
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			GrabPointUI componentInChildren = this.UITransform.GetComponentInChildren<GrabPointUI>();
			if (componentInChildren != null)
			{
				this.receiverAtomSelectionPopup = componentInChildren.receiverAtomSelectionPopup;
				this.receiverSelectionPopup = componentInChildren.receiverSelectionPopup;
				this.grabIconTypeJSON.popup = componentInChildren.grabIconTypePopup;
				this.selectFromSceneButton = componentInChildren.selectFromSceneButton;
			}
			if (this.receiverAtomSelectionPopup != null)
			{
				this.SetReceiverAtomNames();
				if (this._receiver != null && this._receiver.containingAtom != null)
				{
					this.receiverAtomSelectionPopup.currentValue = this._receiver.containingAtom.uid;
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
				this.UpdateReceiverNames();
				if (this._receiver != null)
				{
					this.receiverSelectionPopup.currentValue = this._receiver.name;
				}
				else
				{
					this.receiverSelectionPopup.currentValue = "None";
				}
				UIPopup uipopup3 = this.receiverSelectionPopup;
				uipopup3.onOpenPopupHandlers = (UIPopup.OnOpenPopup)Delegate.Combine(uipopup3.onOpenPopupHandlers, new UIPopup.OnOpenPopup(this.UpdateReceiverNames));
				UIPopup uipopup4 = this.receiverSelectionPopup;
				uipopup4.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup4.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetForceReceiverObject));
			}
			if (this.selectFromSceneButton != null)
			{
				this.selectFromSceneButton.onClick.AddListener(new UnityAction(this.SelectForceReceiverFromScene));
			}
		}
	}

	// Token: 0x06006997 RID: 27031 RVA: 0x00278984 File Offset: 0x00276D84
	protected void Init()
	{
		if (SuperController.singleton != null)
		{
			SuperController singleton = SuperController.singleton;
			singleton.onAtomUIDRenameHandlers = (SuperController.OnAtomUIDRename)Delegate.Combine(singleton.onAtomUIDRenameHandlers, new SuperController.OnAtomUIDRename(this.OnAtomUIDRename));
		}
		if (this.linkLineMaterial)
		{
			this.linkLineDrawer = new LineDrawer(this.linkLineMaterial);
		}
		string[] names = Enum.GetNames(typeof(GrabPoint.GrabIconType));
		List<string> choicesList = new List<string>(names);
		this.grabIconTypeJSON = new JSONStorableStringChooser("grabIconType", choicesList, this._grabIconType.ToString(), "Display Icon", new JSONStorableStringChooser.SetStringCallback(this.SetGrabIconType));
		this.grabIconTypeJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterStringChooser(this.grabIconTypeJSON);
		this.SyncGrabIcon();
	}

	// Token: 0x06006998 RID: 27032 RVA: 0x00278A50 File Offset: 0x00276E50
	private void OnDestroy()
	{
		if (SuperController.singleton != null)
		{
			SuperController singleton = SuperController.singleton;
			singleton.onAtomUIDRenameHandlers = (SuperController.OnAtomUIDRename)Delegate.Remove(singleton.onAtomUIDRenameHandlers, new SuperController.OnAtomUIDRename(this.OnAtomUIDRename));
		}
	}

	// Token: 0x06006999 RID: 27033 RVA: 0x00278A88 File Offset: 0x00276E88
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			this.Init();
			this.InitUI();
		}
	}

	// Token: 0x0600699A RID: 27034 RVA: 0x00278AA8 File Offset: 0x00276EA8
	private void Update()
	{
		if (this.drawLines && this.receiver != null && this.rigidbodyToConnect != null && this.linkLineDrawer != null)
		{
			this.linkLineDrawer.SetLinePoints(this.rigidbodyToConnect.transform.position, this.receiver.transform.position);
			this.linkLineDrawer.Draw(base.gameObject.layer);
		}
	}

	// Token: 0x04005A81 RID: 23169
	public FreeControllerV3 controller;

	// Token: 0x04005A82 RID: 23170
	public Mesh handIconMesh;

	// Token: 0x04005A83 RID: 23171
	public Mesh headIconMesh;

	// Token: 0x04005A84 RID: 23172
	protected JSONStorableStringChooser grabIconTypeJSON;

	// Token: 0x04005A85 RID: 23173
	[SerializeField]
	protected GrabPoint.GrabIconType _grabIconType;

	// Token: 0x04005A86 RID: 23174
	public Rigidbody rigidbodyToConnect;

	// Token: 0x04005A87 RID: 23175
	protected FixedJoint joint;

	// Token: 0x04005A88 RID: 23176
	[SerializeField]
	protected ForceReceiver _receiver;

	// Token: 0x04005A89 RID: 23177
	public UIPopup receiverAtomSelectionPopup;

	// Token: 0x04005A8A RID: 23178
	public UIPopup receiverSelectionPopup;

	// Token: 0x04005A8B RID: 23179
	public Material linkLineMaterial;

	// Token: 0x04005A8C RID: 23180
	protected LineDrawer linkLineDrawer;

	// Token: 0x04005A8D RID: 23181
	public bool drawLines;

	// Token: 0x04005A8E RID: 23182
	protected string receiverAtomUID;

	// Token: 0x04005A8F RID: 23183
	public Button selectFromSceneButton;

	// Token: 0x02000D6A RID: 3434
	public enum GrabIconType
	{
		// Token: 0x04005A91 RID: 23185
		Hand,
		// Token: 0x04005A92 RID: 23186
		Head
	}
}
