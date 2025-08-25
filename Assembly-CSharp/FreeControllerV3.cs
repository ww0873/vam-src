using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using MVR;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000B98 RID: 2968
public class FreeControllerV3 : PhysicsSimulatorJSONStorable
{
	// Token: 0x060053A0 RID: 21408 RVA: 0x001E521C File Offset: 0x001E361C
	public FreeControllerV3()
	{
	}

	// Token: 0x060053A1 RID: 21409 RVA: 0x001E5608 File Offset: 0x001E3A08
	protected void RegisterAllocatedObject(UnityEngine.Object o)
	{
		if (Application.isPlaying)
		{
			if (this.allocatedObjects == null)
			{
				this.allocatedObjects = new List<UnityEngine.Object>();
			}
			this.allocatedObjects.Add(o);
		}
	}

	// Token: 0x060053A2 RID: 21410 RVA: 0x001E5638 File Offset: 0x001E3A38
	protected void DestroyAllocatedObjects()
	{
		if (Application.isPlaying && this.allocatedObjects != null)
		{
			foreach (UnityEngine.Object obj in this.allocatedObjects)
			{
				UnityEngine.Object.Destroy(obj);
			}
		}
	}

	// Token: 0x17000C29 RID: 3113
	// (get) Token: 0x060053A3 RID: 21411 RVA: 0x001E56A8 File Offset: 0x001E3AA8
	// (set) Token: 0x060053A4 RID: 21412 RVA: 0x001E56B0 File Offset: 0x001E3AB0
	public bool forceStorePositionRotationAsLocal
	{
		get
		{
			return this._forceStorePositionRotationAsLocal;
		}
		set
		{
			if (this._forceStorePositionRotationAsLocal != value)
			{
				this._forceStorePositionRotationAsLocal = value;
			}
		}
	}

	// Token: 0x060053A5 RID: 21413 RVA: 0x001E56C5 File Offset: 0x001E3AC5
	public override string[] GetCustomParamNames()
	{
		return this.customParamNames;
	}

	// Token: 0x060053A6 RID: 21414 RVA: 0x001E56D0 File Offset: 0x001E3AD0
	public override JSONClass GetJSON(bool includePhysical = true, bool includeAppearance = true, bool forceStore = true)
	{
		JSONClass json = base.GetJSON(includePhysical, includeAppearance, forceStore);
		if (includePhysical || forceStore)
		{
			this.needsStore = true;
			if (this.storePositionRotationAsLocal || this._forceStorePositionRotationAsLocal)
			{
				Vector3 localPosition = base.transform.localPosition;
				json["localPosition"]["x"].AsFloat = localPosition.x;
				json["localPosition"]["y"].AsFloat = localPosition.y;
				json["localPosition"]["z"].AsFloat = localPosition.z;
				Vector3 localEulerAngles = base.transform.localEulerAngles;
				json["localRotation"]["x"].AsFloat = localEulerAngles.x;
				json["localRotation"]["y"].AsFloat = localEulerAngles.y;
				json["localRotation"]["z"].AsFloat = localEulerAngles.z;
			}
			else
			{
				Vector3 position = base.transform.position;
				json["position"]["x"].AsFloat = position.x;
				json["position"]["y"].AsFloat = position.y;
				json["position"]["z"].AsFloat = position.z;
				Vector3 eulerAngles = base.transform.eulerAngles;
				json["rotation"]["x"].AsFloat = eulerAngles.x;
				json["rotation"]["y"].AsFloat = eulerAngles.y;
				json["rotation"]["z"].AsFloat = eulerAngles.z;
			}
			if (this._linkToRB != null && this.linkToAtomUID != null && this.linkToAtomUID != "[CameraRig]")
			{
				string text = base.AtomUidToStoreAtomUid(this.linkToAtomUID);
				if (text != null)
				{
					json["linkTo"] = text + ":" + this._linkToRB.name;
				}
				else
				{
					SuperController.LogError(string.Concat(new object[]
					{
						"Warning: FreeController ",
						this.containingAtom,
						":",
						base.name,
						" is linked to object in atom ",
						this.linkToAtomUID,
						" that is not in subscene and cannot be saved"
					}));
				}
			}
		}
		return json;
	}

	// Token: 0x060053A7 RID: 21415 RVA: 0x001E5990 File Offset: 0x001E3D90
	public override void RestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, JSONArray presetAtoms = null, bool setMissingToDefault = true)
	{
		if (!base.physicalLocked && restorePhysical && !base.IsCustomPhysicalParamLocked("linkTo"))
		{
			this.SelectLinkToRigidbody(null);
		}
		base.RestoreFromJSON(jc, restorePhysical, restoreAppearance, presetAtoms, setMissingToDefault);
		if (!base.physicalLocked && restorePhysical)
		{
			this.PauseComply(100);
			bool flag = false;
			if (jc["position"] != null)
			{
				if (!base.IsCustomPhysicalParamLocked("position"))
				{
					Vector3 position = base.transform.position;
					if (jc["position"]["x"] != null)
					{
						position.x = jc["position"]["x"].AsFloat;
					}
					if (jc["position"]["y"] != null)
					{
						position.y = jc["position"]["y"].AsFloat;
					}
					if (jc["position"]["z"] != null)
					{
						position.z = jc["position"]["z"].AsFloat;
					}
					base.transform.position = position;
					if (this.control != null)
					{
						this.control.position = position;
						flag = true;
						if (this.onPositionChangeHandlers != null)
						{
							this.onPositionChangeHandlers(this);
						}
					}
					if (this.followWhenOff != null && !this.followWhenOff.GetComponent<JSONStorable>() && !this._detachControl)
					{
						this.followWhenOff.position = position;
					}
				}
			}
			else if (jc["localPosition"] != null)
			{
				if (!base.IsCustomPhysicalParamLocked("localPosition"))
				{
					Vector3 localPosition = base.transform.localPosition;
					if (jc["localPosition"]["x"] != null)
					{
						localPosition.x = jc["localPosition"]["x"].AsFloat;
					}
					if (jc["localPosition"]["y"] != null)
					{
						localPosition.y = jc["localPosition"]["y"].AsFloat;
					}
					if (jc["localPosition"]["z"] != null)
					{
						localPosition.z = jc["localPosition"]["z"].AsFloat;
					}
					base.transform.localPosition = localPosition;
					if (this.control != null)
					{
						this.control.position = base.transform.position;
						flag = true;
						if (this.onPositionChangeHandlers != null)
						{
							this.onPositionChangeHandlers(this);
						}
					}
					if (this.followWhenOff != null && !this.followWhenOff.GetComponent<JSONStorable>() && !this._detachControl)
					{
						this.followWhenOff.position = base.transform.position;
					}
				}
			}
			else if (setMissingToDefault && !base.IsCustomPhysicalParamLocked("position") && !base.IsCustomPhysicalParamLocked("localPosition"))
			{
				if (this.storePositionRotationAsLocal || this._forceStorePositionRotationAsLocal)
				{
					base.transform.localPosition = this.startingLocalPosition;
				}
				else
				{
					base.transform.position = this.startingPosition;
				}
				if (this.control != null)
				{
					this.control.position = base.transform.position;
					flag = true;
					if (this.onPositionChangeHandlers != null)
					{
						this.onPositionChangeHandlers(this);
					}
				}
				if (this.followWhenOff != null && !this.followWhenOff.GetComponent<JSONStorable>() && !this._detachControl)
				{
					this.followWhenOff.position = base.transform.position;
				}
			}
			if (jc["rotation"] != null)
			{
				if (!base.IsCustomPhysicalParamLocked("rotation"))
				{
					Vector3 eulerAngles = base.transform.eulerAngles;
					if (jc["rotation"]["x"] != null)
					{
						eulerAngles.x = jc["rotation"]["x"].AsFloat;
					}
					if (jc["rotation"]["y"] != null)
					{
						eulerAngles.y = jc["rotation"]["y"].AsFloat;
					}
					if (jc["rotation"]["z"] != null)
					{
						eulerAngles.z = jc["rotation"]["z"].AsFloat;
					}
					base.transform.eulerAngles = eulerAngles;
					if (this.control != null)
					{
						this.control.rotation = base.transform.rotation;
						flag = true;
						if (this.onRotationChangeHandlers != null)
						{
							this.onRotationChangeHandlers(this);
						}
					}
					if (this.followWhenOff != null && !this.followWhenOff.GetComponent<JSONStorable>() && !this._detachControl)
					{
						this.followWhenOff.rotation = base.transform.rotation;
					}
				}
			}
			else if (jc["localRotation"] != null)
			{
				if (!base.IsCustomPhysicalParamLocked("localRotation"))
				{
					Vector3 localEulerAngles = base.transform.localEulerAngles;
					if (jc["localRotation"]["x"] != null)
					{
						localEulerAngles.x = jc["localRotation"]["x"].AsFloat;
					}
					if (jc["localRotation"]["y"] != null)
					{
						localEulerAngles.y = jc["localRotation"]["y"].AsFloat;
					}
					if (jc["localRotation"]["z"] != null)
					{
						localEulerAngles.z = jc["localRotation"]["z"].AsFloat;
					}
					base.transform.localEulerAngles = localEulerAngles;
					if (this.control != null)
					{
						this.control.rotation = base.transform.rotation;
						flag = true;
						if (this.onRotationChangeHandlers != null)
						{
							this.onRotationChangeHandlers(this);
						}
					}
					if (this.followWhenOff != null && !this.followWhenOff.GetComponent<JSONStorable>() && !this._detachControl)
					{
						this.followWhenOff.rotation = base.transform.rotation;
					}
				}
			}
			else if (setMissingToDefault && !base.IsCustomPhysicalParamLocked("rotation") && !base.IsCustomPhysicalParamLocked("localRotation"))
			{
				if (this.storePositionRotationAsLocal || this._forceStorePositionRotationAsLocal)
				{
					base.transform.localRotation = this.startingLocalRotation;
				}
				else
				{
					base.transform.rotation = this.startingRotation;
				}
				if (this.control != null)
				{
					this.control.rotation = base.transform.rotation;
					flag = true;
					if (this.onRotationChangeHandlers != null)
					{
						this.onRotationChangeHandlers(this);
					}
				}
				if (this.followWhenOff != null && !this.followWhenOff.GetComponent<JSONStorable>() && !this._detachControl)
				{
					this.followWhenOff.rotation = base.transform.rotation;
				}
			}
			if (flag)
			{
				this.SyncMoveWhenInactive();
				if (this.onMovementHandlers != null)
				{
					this.onMovementHandlers(this);
				}
			}
		}
	}

	// Token: 0x060053A8 RID: 21416 RVA: 0x001E6208 File Offset: 0x001E4608
	public override void LateRestoreFromJSON(JSONClass jc, bool restorePhysical = true, bool restoreAppearance = true, bool setMissingToDefault = true)
	{
		base.LateRestoreFromJSON(jc, restorePhysical, restoreAppearance, setMissingToDefault);
		if (!base.physicalLocked && restorePhysical && !base.IsCustomPhysicalParamLocked("linkTo"))
		{
			if (jc["linkTo"] != null)
			{
				if (SuperController.singleton != null)
				{
					string rigidbodyName = base.StoredAtomUidToAtomUid(jc["linkTo"]);
					Rigidbody rb = SuperController.singleton.RigidbodyNameToRigidbody(rigidbodyName);
					this.SelectLinkToRigidbody(rb, FreeControllerV3.SelectLinkState.PositionAndRotation, false, false);
				}
			}
			else if (setMissingToDefault)
			{
				if (this.startingLinkToRigidbody != null)
				{
					this.SelectLinkToRigidbody(this.startingLinkToRigidbody, FreeControllerV3.SelectLinkState.PositionAndRotation, false, false);
				}
				else
				{
					this.SelectLinkToRigidbody(null);
				}
			}
			this.jointRotationDriveXTargetJSON.RestoreFromJSON(jc, true, true, true);
		}
	}

	// Token: 0x060053A9 RID: 21417 RVA: 0x001E62DC File Offset: 0x001E46DC
	public void Reset()
	{
		JSONStorable[] components = base.GetComponents<JSONStorable>();
		foreach (JSONStorable jsonstorable in components)
		{
			if (!jsonstorable.exclude)
			{
				jsonstorable.RestoreAllFromDefaults();
			}
		}
		if (this.followWhenOff != null)
		{
			components = this.followWhenOff.GetComponents<JSONStorable>();
			foreach (JSONStorable jsonstorable2 in components)
			{
				if (!jsonstorable2.exclude)
				{
					jsonstorable2.RestoreAllFromDefaults();
				}
			}
		}
	}

	// Token: 0x060053AA RID: 21418 RVA: 0x001E6370 File Offset: 0x001E4770
	public void SelectRoot()
	{
		if (this.enableSelectRoot && this.containingAtom != null && this.containingAtom.mainController != null)
		{
			SuperController.singleton.SelectController(this.containingAtom.mainController, false, true, true, true);
		}
	}

	// Token: 0x060053AB RID: 21419 RVA: 0x001E63C8 File Offset: 0x001E47C8
	public void MovePlayerTo()
	{
	}

	// Token: 0x060053AC RID: 21420 RVA: 0x001E63CA File Offset: 0x001E47CA
	public void MovePlayerToAndControl()
	{
	}

	// Token: 0x17000C2A RID: 3114
	// (get) Token: 0x060053AD RID: 21421 RVA: 0x001E63CC File Offset: 0x001E47CC
	// (set) Token: 0x060053AE RID: 21422 RVA: 0x001E63D4 File Offset: 0x001E47D4
	public Rigidbody linkToRB
	{
		get
		{
			return this._linkToRB;
		}
		set
		{
			if (this._linkToRB != value)
			{
				if (this._linkToConnector != null)
				{
					UnityEngine.Object.DestroyImmediate(this._linkToConnector.gameObject);
					this._linkToConnector = null;
				}
				if (this._linkToJoint != null)
				{
					UnityEngine.Object.DestroyImmediate(this._linkToJoint);
					this._linkToJoint = null;
					if (this.kinematicRB != null)
					{
						this.kinematicRB.isKinematic = false;
						this.kinematicRB.isKinematic = true;
					}
				}
				this._linkToRB = value;
				if (this._linkToRB != null)
				{
					if (this._followWhenOffRB != null && this._linkToRB != null)
					{
						GameObject gameObject = this._followWhenOffRB.gameObject;
						this._linkToJoint = gameObject.AddComponent<ConfigurableJoint>();
						this._linkToJoint.connectedBody = this._linkToRB;
						this._linkToJoint.xMotion = ConfigurableJointMotion.Free;
						this._linkToJoint.yMotion = ConfigurableJointMotion.Free;
						this._linkToJoint.zMotion = ConfigurableJointMotion.Free;
						this._linkToJoint.angularXMotion = ConfigurableJointMotion.Free;
						this._linkToJoint.angularYMotion = ConfigurableJointMotion.Free;
						this._linkToJoint.angularZMotion = ConfigurableJointMotion.Free;
						this._linkToJoint.rotationDriveMode = RotationDriveMode.Slerp;
						this.SetLinkedJointSprings();
					}
					GameObject gameObject2 = new GameObject();
					this._linkToConnector = gameObject2.transform;
					this._linkToConnector.position = base.transform.position;
					this._linkToConnector.rotation = base.transform.rotation;
					this._linkToConnector.SetParent(this._linkToRB.transform);
				}
			}
		}
	}

	// Token: 0x060053AF RID: 21423 RVA: 0x001E6578 File Offset: 0x001E4978
	protected virtual void SetLinkToAtomNames()
	{
		if (this.linkToAtomSelectionPopup != null && SuperController.singleton != null)
		{
			List<string> atomUIDsWithRigidbodies = SuperController.singleton.GetAtomUIDsWithRigidbodies();
			if (atomUIDsWithRigidbodies == null)
			{
				this.linkToAtomSelectionPopup.numPopupValues = 1;
				this.linkToAtomSelectionPopup.setPopupValue(0, "None");
			}
			else
			{
				this.linkToAtomSelectionPopup.numPopupValues = atomUIDsWithRigidbodies.Count + 1;
				this.linkToAtomSelectionPopup.setPopupValue(0, "None");
				for (int i = 0; i < atomUIDsWithRigidbodies.Count; i++)
				{
					this.linkToAtomSelectionPopup.setPopupValue(i + 1, atomUIDsWithRigidbodies[i]);
				}
			}
		}
	}

	// Token: 0x060053B0 RID: 21424 RVA: 0x001E6629 File Offset: 0x001E4A29
	protected void OnAtomUIDRename(string fromid, string toid)
	{
		this.SyncAtomUID();
		if (this.linkToAtomUID == fromid)
		{
			this.linkToAtomUID = toid;
			if (this.linkToAtomSelectionPopup != null)
			{
				this.linkToAtomSelectionPopup.currentValueNoCallback = toid;
			}
		}
	}

	// Token: 0x060053B1 RID: 21425 RVA: 0x001E6668 File Offset: 0x001E4A68
	protected void onLinkToRigidbodyNamesChanged(List<string> rbNames)
	{
		if (this.linkToSelectionPopup != null)
		{
			if (rbNames == null)
			{
				this.linkToSelectionPopup.numPopupValues = 1;
				this.linkToSelectionPopup.setPopupValue(0, "None");
			}
			else
			{
				this.linkToSelectionPopup.numPopupValues = rbNames.Count + 1;
				this.linkToSelectionPopup.setPopupValue(0, "None");
				for (int i = 0; i < rbNames.Count; i++)
				{
					this.linkToSelectionPopup.setPopupValue(i + 1, rbNames[i]);
				}
			}
		}
	}

	// Token: 0x060053B2 RID: 21426 RVA: 0x001E6700 File Offset: 0x001E4B00
	public virtual void SetLinkToAtom(string atomUID)
	{
		if (SuperController.singleton != null)
		{
			Atom atomByUid = SuperController.singleton.GetAtomByUid(atomUID);
			if (atomByUid != null)
			{
				this.linkToAtomUID = atomUID;
				List<string> rigidbodyNamesInAtom = SuperController.singleton.GetRigidbodyNamesInAtom(this.linkToAtomUID);
				this.onLinkToRigidbodyNamesChanged(rigidbodyNamesInAtom);
				if (this.linkToSelectionPopup != null)
				{
					this.linkToSelectionPopup.currentValue = "None";
				}
			}
		}
	}

	// Token: 0x060053B3 RID: 21427 RVA: 0x001E6778 File Offset: 0x001E4B78
	public void SetLinkToRigidbody(string rigidbodyName)
	{
		if (SuperController.singleton != null)
		{
			Rigidbody linkToRB = SuperController.singleton.RigidbodyNameToRigidbody(rigidbodyName);
			this.linkToRB = linkToRB;
		}
	}

	// Token: 0x060053B4 RID: 21428 RVA: 0x001E67A8 File Offset: 0x001E4BA8
	public virtual void SetLinkToRigidbodyObject(string objectName)
	{
		if (this.linkToAtomUID != null)
		{
			this.SetLinkToRigidbody(this.linkToAtomUID + ":" + objectName);
		}
	}

	// Token: 0x060053B5 RID: 21429 RVA: 0x001E67CC File Offset: 0x001E4BCC
	private void GetLinkToAtomUIDFromLinkToRB(Rigidbody rb)
	{
		this.linkToAtomUID = "None";
		Atom atom = null;
		FreeControllerV3 component = rb.GetComponent<FreeControllerV3>();
		if (component != null)
		{
			atom = component.containingAtom;
		}
		else
		{
			ForceReceiver component2 = rb.GetComponent<ForceReceiver>();
			if (component2 != null)
			{
				atom = component2.containingAtom;
			}
		}
		if (atom != null)
		{
			this.linkToAtomUID = atom.uid;
		}
	}

	// Token: 0x060053B6 RID: 21430 RVA: 0x001E6837 File Offset: 0x001E4C37
	public void SelectLinkToRigidbody(Rigidbody rb)
	{
		this.SelectLinkToRigidbody(rb, FreeControllerV3.SelectLinkState.PositionAndRotation, false, true);
	}

	// Token: 0x060053B7 RID: 21431 RVA: 0x001E6844 File Offset: 0x001E4C44
	public void SelectLinkToRigidbody(Rigidbody rb, FreeControllerV3.SelectLinkState linkState, bool usePhysicalLink = false, bool modifyState = true)
	{
		if (rb != null)
		{
			this.preLinkRB = this.linkToRB;
			this.preLinkPositionState = this.currentPositionState;
			this.preLinkRotationState = this.currentRotationState;
			this.GetLinkToAtomUIDFromLinkToRB(rb);
			if (this.linkToAtomSelectionPopup != null)
			{
				this.linkToAtomSelectionPopup.currentValue = this.linkToAtomUID;
			}
		}
		else if (this.linkToAtomSelectionPopup != null)
		{
			this.linkToAtomSelectionPopup.currentValue = "None";
		}
		if (this.linkToSelectionPopup != null)
		{
			if (rb != null)
			{
				this.linkToSelectionPopup.currentValueNoCallback = rb.name;
			}
			else
			{
				this.linkToSelectionPopup.currentValueNoCallback = "None";
			}
		}
		if (rb != null)
		{
			this.linkToRB = rb;
			if (modifyState)
			{
				if (linkState == FreeControllerV3.SelectLinkState.Position || linkState == FreeControllerV3.SelectLinkState.PositionAndRotation)
				{
					if (usePhysicalLink && this._currentPositionState != FreeControllerV3.PositionState.PhysicsLink)
					{
						this.currentPositionState = FreeControllerV3.PositionState.PhysicsLink;
					}
					else if (this._currentPositionState != FreeControllerV3.PositionState.ParentLink && this._currentPositionState != FreeControllerV3.PositionState.PhysicsLink)
					{
						this.currentPositionState = FreeControllerV3.PositionState.ParentLink;
					}
				}
				if (linkState == FreeControllerV3.SelectLinkState.Rotation || linkState == FreeControllerV3.SelectLinkState.PositionAndRotation)
				{
					if (usePhysicalLink && this._currentRotationState != FreeControllerV3.RotationState.PhysicsLink)
					{
						this.currentRotationState = FreeControllerV3.RotationState.PhysicsLink;
					}
					else if (this._currentRotationState != FreeControllerV3.RotationState.ParentLink && this._currentRotationState != FreeControllerV3.RotationState.PhysicsLink)
					{
						this.currentRotationState = FreeControllerV3.RotationState.ParentLink;
					}
				}
			}
		}
		else
		{
			this.linkToRB = null;
			if (modifyState)
			{
				if (this._currentPositionState == FreeControllerV3.PositionState.ParentLink || this._currentPositionState == FreeControllerV3.PositionState.PhysicsLink)
				{
					this.currentPositionState = this.preLinkPositionState;
				}
				if (this._currentRotationState == FreeControllerV3.RotationState.ParentLink || this._currentRotationState == FreeControllerV3.RotationState.PhysicsLink)
				{
					this.currentRotationState = this.preLinkRotationState;
				}
			}
		}
	}

	// Token: 0x060053B8 RID: 21432 RVA: 0x001E6A1C File Offset: 0x001E4E1C
	public void RestorePreLinkState()
	{
		if (this.preLinkRB != null)
		{
			this.GetLinkToAtomUIDFromLinkToRB(this.preLinkRB);
			if (this.linkToAtomUID == "None")
			{
				if (this.linkToAtomSelectionPopup != null)
				{
					this.linkToAtomSelectionPopup.currentValue = "None";
				}
				if (this.linkToSelectionPopup != null)
				{
					this.linkToSelectionPopup.currentValueNoCallback = "None";
				}
			}
			else
			{
				if (this.linkToAtomSelectionPopup != null)
				{
					this.linkToAtomSelectionPopup.currentValue = this.linkToAtomUID;
				}
				if (this.linkToSelectionPopup != null)
				{
					this.linkToSelectionPopup.currentValueNoCallback = this.preLinkRB.name;
				}
			}
		}
		else
		{
			if (this.linkToAtomSelectionPopup != null)
			{
				this.linkToAtomSelectionPopup.currentValue = "None";
			}
			if (this.linkToSelectionPopup != null)
			{
				this.linkToSelectionPopup.currentValueNoCallback = "None";
			}
		}
		if (this.preLinkRB != null && this.linkToAtomUID != "None")
		{
			this.linkToRB = this.preLinkRB;
			if (this.currentPositionState != this.preLinkPositionState)
			{
				this.currentPositionState = this.preLinkPositionState;
			}
			if (this.currentRotationState != this.preLinkRotationState)
			{
				this.currentRotationState = this.preLinkRotationState;
			}
		}
		else
		{
			this.linkToRB = null;
			if (this._currentPositionState == FreeControllerV3.PositionState.ParentLink || this._currentPositionState == FreeControllerV3.PositionState.PhysicsLink)
			{
				this.currentPositionState = this.preLinkPositionState;
			}
			if (this._currentRotationState == FreeControllerV3.RotationState.ParentLink || this._currentRotationState == FreeControllerV3.RotationState.PhysicsLink)
			{
				this.currentRotationState = this.preLinkRotationState;
			}
		}
	}

	// Token: 0x060053B9 RID: 21433 RVA: 0x001E6BF2 File Offset: 0x001E4FF2
	protected void SyncGrabFreezePhysics()
	{
		if (this.freezeAtomPhysicsWhenGrabbed)
		{
			if (this._isGrabbing && !this._detachControl)
			{
				this.containingAtom.grabFreezePhysics = true;
			}
			else
			{
				this.containingAtom.grabFreezePhysics = false;
			}
		}
	}

	// Token: 0x17000C2B RID: 3115
	// (get) Token: 0x060053BA RID: 21434 RVA: 0x001E6C32 File Offset: 0x001E5032
	// (set) Token: 0x060053BB RID: 21435 RVA: 0x001E6C3C File Offset: 0x001E503C
	public bool isGrabbing
	{
		get
		{
			return this._isGrabbing;
		}
		set
		{
			if (this._isGrabbing != value)
			{
				this._isGrabbing = value;
				if (this._isGrabbing)
				{
					if (this.onGrabStartHandlers != null)
					{
						this.onGrabStartHandlers(this);
					}
				}
				else if (this.onGrabEndHandlers != null)
				{
					this.onGrabEndHandlers(this);
				}
				this.SyncGrabFreezePhysics();
			}
		}
	}

	// Token: 0x060053BC RID: 21436 RVA: 0x001E6CA0 File Offset: 0x001E50A0
	public void SelectLinkToRigidbodyFromScene()
	{
		this.SetLinkToAtomNames();
		SuperController.singleton.SelectModeRigidbody(new SuperController.SelectRigidbodyCallback(this.SelectLinkToRigidbody));
	}

	// Token: 0x060053BD RID: 21437 RVA: 0x001E6CC0 File Offset: 0x001E50C0
	public void SelectAlignToRigidbody(Rigidbody rb)
	{
		this.control.position = rb.transform.position;
		this.control.rotation = rb.transform.rotation;
		if (this.onPositionChangeHandlers != null)
		{
			this.onPositionChangeHandlers(this);
		}
		if (this.onRotationChangeHandlers != null)
		{
			this.onRotationChangeHandlers(this);
		}
		if (this.onMovementHandlers != null)
		{
			this.onMovementHandlers(this);
		}
	}

	// Token: 0x060053BE RID: 21438 RVA: 0x001E6D3E File Offset: 0x001E513E
	public void SelectAlignToRigidbodyFromScene()
	{
		SuperController.singleton.SelectModeRigidbody(new SuperController.SelectRigidbodyCallback(this.SelectAlignToRigidbody));
	}

	// Token: 0x060053BF RID: 21439 RVA: 0x001E6D58 File Offset: 0x001E5158
	public void SetPositionStateFromString(string state)
	{
		try
		{
			FreeControllerV3.PositionState currentPositionState = (FreeControllerV3.PositionState)Enum.Parse(typeof(FreeControllerV3.PositionState), state);
			this._currentPositionState = currentPositionState;
			this.SyncPositionState();
		}
		catch (ArgumentException)
		{
			Debug.LogError("State " + state + " is not a valid position state");
		}
	}

	// Token: 0x060053C0 RID: 21440 RVA: 0x001E6DB8 File Offset: 0x001E51B8
	private void SyncPositionState()
	{
		switch (this._currentPositionState)
		{
		case FreeControllerV3.PositionState.On:
		case FreeControllerV3.PositionState.Following:
		case FreeControllerV3.PositionState.Hold:
		case FreeControllerV3.PositionState.Lock:
		case FreeControllerV3.PositionState.ParentLink:
		case FreeControllerV3.PositionState.PhysicsLink:
		case FreeControllerV3.PositionState.Comply:
			if (this._followWhenOffRB != null)
			{
				this._followWhenOffRB.useGravity = this._useGravityOnRBWhenOff;
			}
			break;
		case FreeControllerV3.PositionState.Off:
			if (this._followWhenOffRB != null)
			{
				this._followWhenOffRB.useGravity = this._useGravityOnRBWhenOff;
			}
			break;
		}
		switch (this._currentPositionState)
		{
		case FreeControllerV3.PositionState.On:
		case FreeControllerV3.PositionState.Comply:
			this._moveEnabled = true;
			this._moveForceEnabled = false;
			break;
		case FreeControllerV3.PositionState.Off:
		case FreeControllerV3.PositionState.Following:
		case FreeControllerV3.PositionState.Hold:
			this._moveEnabled = this.useForceWhenOff;
			this._moveForceEnabled = this.useForceWhenOff;
			break;
		case FreeControllerV3.PositionState.Lock:
			this._moveEnabled = false;
			this._moveForceEnabled = false;
			break;
		case FreeControllerV3.PositionState.ParentLink:
		case FreeControllerV3.PositionState.PhysicsLink:
			this._moveEnabled = this.useForceWhenOff;
			this._moveForceEnabled = this.useForceWhenOff;
			if (this._linkToConnector != null)
			{
				this._linkToConnector.position = base.transform.position;
			}
			if (this._linkToJoint != null)
			{
				this._linkToJoint.connectedBody = null;
				this._linkToJoint.connectedBody = this._linkToRB;
			}
			break;
		}
		if (this.kinematicRB != null)
		{
			this.kinematicRB.isKinematic = false;
			this.kinematicRB.isKinematic = true;
		}
		this.SetLinkedJointSprings();
		this.SetJointSprings();
		this.StateChanged();
	}

	// Token: 0x17000C2C RID: 3116
	// (get) Token: 0x060053C1 RID: 21441 RVA: 0x001E6F67 File Offset: 0x001E5367
	// (set) Token: 0x060053C2 RID: 21442 RVA: 0x001E6F70 File Offset: 0x001E5370
	public FreeControllerV3.PositionState currentPositionState
	{
		get
		{
			return this._currentPositionState;
		}
		set
		{
			if (this.stateCanBeModified)
			{
				if (this.currentPositionStateJSON != null)
				{
					this.currentPositionStateJSON.val = value.ToString();
				}
				else if (this._currentPositionState != value)
				{
					this._currentPositionState = value;
					this.SyncPositionState();
				}
			}
		}
	}

	// Token: 0x17000C2D RID: 3117
	// (get) Token: 0x060053C3 RID: 21443 RVA: 0x001E6FCC File Offset: 0x001E53CC
	public bool isPositionOn
	{
		get
		{
			return this._currentPositionState == FreeControllerV3.PositionState.On || this._currentPositionState == FreeControllerV3.PositionState.Comply || this._currentPositionState == FreeControllerV3.PositionState.Following || this._currentPositionState == FreeControllerV3.PositionState.Hold || this._currentPositionState == FreeControllerV3.PositionState.ParentLink || this._currentPositionState == FreeControllerV3.PositionState.PhysicsLink;
		}
	}

	// Token: 0x060053C4 RID: 21444 RVA: 0x001E7020 File Offset: 0x001E5420
	public void SetRotationStateFromString(string state)
	{
		try
		{
			FreeControllerV3.RotationState currentRotationState = (FreeControllerV3.RotationState)Enum.Parse(typeof(FreeControllerV3.RotationState), state);
			this._currentRotationState = currentRotationState;
			this.SyncRotationState();
		}
		catch (ArgumentException)
		{
			Debug.LogError("State " + state + " is not a valid rotation state");
		}
	}

	// Token: 0x060053C5 RID: 21445 RVA: 0x001E7080 File Offset: 0x001E5480
	private void SyncRotationState()
	{
		switch (this._currentRotationState)
		{
		case FreeControllerV3.RotationState.On:
		case FreeControllerV3.RotationState.Comply:
			this._rotationEnabled = true;
			this._rotationForceEnabled = false;
			break;
		case FreeControllerV3.RotationState.Off:
		case FreeControllerV3.RotationState.Following:
		case FreeControllerV3.RotationState.Hold:
		case FreeControllerV3.RotationState.LookAt:
			this._rotationEnabled = this.useForceWhenOff;
			this._rotationForceEnabled = this.useForceWhenOff;
			break;
		case FreeControllerV3.RotationState.Lock:
			this._rotationEnabled = false;
			this._rotationForceEnabled = false;
			break;
		case FreeControllerV3.RotationState.ParentLink:
		case FreeControllerV3.RotationState.PhysicsLink:
			this._rotationEnabled = this.useForceWhenOff;
			this._rotationForceEnabled = this.useForceWhenOff;
			if (this._linkToConnector != null)
			{
				this._linkToConnector.rotation = base.transform.rotation;
			}
			if (this._linkToJoint != null)
			{
				this._linkToJoint.connectedBody = null;
				this._linkToJoint.connectedBody = this._linkToRB;
			}
			break;
		}
		if (this.kinematicRB != null)
		{
			this.kinematicRB.isKinematic = false;
			this.kinematicRB.isKinematic = true;
		}
		this.SetLinkedJointSprings();
		this.SetJointSprings();
		this.SetNaturalJointDrive();
		this.StateChanged();
	}

	// Token: 0x17000C2E RID: 3118
	// (get) Token: 0x060053C6 RID: 21446 RVA: 0x001E71B9 File Offset: 0x001E55B9
	// (set) Token: 0x060053C7 RID: 21447 RVA: 0x001E71C4 File Offset: 0x001E55C4
	public FreeControllerV3.RotationState currentRotationState
	{
		get
		{
			return this._currentRotationState;
		}
		set
		{
			if (this.stateCanBeModified)
			{
				if (this.currentRotationStateJSON != null)
				{
					this.currentRotationStateJSON.val = value.ToString();
				}
				else if (this._currentRotationState != value)
				{
					this._currentRotationState = value;
					this.SyncRotationState();
				}
			}
		}
	}

	// Token: 0x17000C2F RID: 3119
	// (get) Token: 0x060053C8 RID: 21448 RVA: 0x001E7220 File Offset: 0x001E5620
	public bool isRotationOn
	{
		get
		{
			return this._currentRotationState == FreeControllerV3.RotationState.On || this._currentRotationState == FreeControllerV3.RotationState.Comply || this._currentRotationState == FreeControllerV3.RotationState.Following || this._currentRotationState == FreeControllerV3.RotationState.Hold || this._currentRotationState == FreeControllerV3.RotationState.LookAt || this._currentRotationState == FreeControllerV3.RotationState.ParentLink || this._currentPositionState == FreeControllerV3.PositionState.PhysicsLink;
		}
	}

	// Token: 0x060053C9 RID: 21449 RVA: 0x001E7280 File Offset: 0x001E5680
	public override void ScaleChanged(float scale)
	{
		base.ScaleChanged(scale);
		this.PauseComply(10);
		this.scalePow = Mathf.Pow(1.7f, scale - 1f);
		this.SetJointSprings();
		this.SetNaturalJointDrive();
	}

	// Token: 0x060053CA RID: 21450 RVA: 0x001E72B4 File Offset: 0x001E56B4
	private void SetLinkedJointSprings()
	{
		if (this._linkToJoint != null)
		{
			JointDrive jointDrive = this._linkToJoint.xDrive;
			if (this._currentPositionState == FreeControllerV3.PositionState.PhysicsLink)
			{
				jointDrive.positionSpring = this._RBLinkPositionSpring;
				jointDrive.positionDamper = this._RBLinkPositionDamper;
				jointDrive.maximumForce = this._RBLinkPositionMaxForce;
			}
			else
			{
				jointDrive.positionSpring = 0f;
				jointDrive.positionDamper = 0f;
				jointDrive.maximumForce = 0f;
			}
			this._linkToJoint.xDrive = jointDrive;
			this._linkToJoint.yDrive = jointDrive;
			this._linkToJoint.zDrive = jointDrive;
			jointDrive = this._linkToJoint.slerpDrive;
			if (this._currentRotationState == FreeControllerV3.RotationState.PhysicsLink)
			{
				jointDrive.positionSpring = this._RBLinkRotationSpring;
				jointDrive.positionDamper = this._RBLinkRotationDamper;
				jointDrive.maximumForce = this._RBLinkRotationMaxForce;
			}
			else
			{
				jointDrive.positionSpring = 0f;
				jointDrive.positionDamper = 0f;
				jointDrive.maximumForce = 0f;
			}
			this._linkToJoint.slerpDrive = jointDrive;
			this._linkToJoint.angularXDrive = jointDrive;
			this._linkToJoint.angularYZDrive = jointDrive;
		}
	}

	// Token: 0x060053CB RID: 21451 RVA: 0x001E73EC File Offset: 0x001E57EC
	private void SetJointSprings()
	{
		if (this.connectedJoint != null)
		{
			float num = this.scalePow;
			float num2 = this.scalePow;
			float num3 = this.scalePow;
			JointDrive jointDrive = this.connectedJoint.xDrive;
			switch (this._currentPositionState)
			{
			case FreeControllerV3.PositionState.On:
			case FreeControllerV3.PositionState.Following:
			case FreeControllerV3.PositionState.Hold:
			case FreeControllerV3.PositionState.ParentLink:
			case FreeControllerV3.PositionState.PhysicsLink:
				if (this._isGrabbing && this._currentPositionState == FreeControllerV3.PositionState.ParentLink && this.preLinkPositionState == FreeControllerV3.PositionState.Lock)
				{
					jointDrive.positionSpring = this._RBLockPositionSpring;
					jointDrive.positionDamper = this._RBLockPositionDamper;
					jointDrive.maximumForce = this._RBLockPositionMaxForce;
				}
				else
				{
					jointDrive.positionSpring = this._RBHoldPositionSpring;
					jointDrive.positionDamper = this._RBHoldPositionDamper;
					jointDrive.maximumForce = this._RBHoldPositionMaxForce;
				}
				break;
			case FreeControllerV3.PositionState.Off:
				jointDrive.positionSpring = 0f;
				jointDrive.positionDamper = 0f;
				jointDrive.maximumForce = 0f;
				break;
			case FreeControllerV3.PositionState.Lock:
				jointDrive.positionSpring = this._RBLockPositionSpring;
				jointDrive.positionDamper = this._RBLockPositionDamper;
				jointDrive.maximumForce = this._RBLockPositionMaxForce;
				break;
			case FreeControllerV3.PositionState.Comply:
				jointDrive.positionSpring = this._RBComplyPositionSpring * this.RBMass;
				jointDrive.positionDamper = this._RBComplyPositionDamper * this.RBMass;
				jointDrive.maximumForce = this._RBComplyPositionMaxForce;
				break;
			}
			this.connectedJoint.xDrive = jointDrive;
			this.connectedJoint.yDrive = jointDrive;
			this.connectedJoint.zDrive = jointDrive;
			jointDrive = this.connectedJoint.slerpDrive;
			switch (this._currentRotationState)
			{
			case FreeControllerV3.RotationState.On:
			case FreeControllerV3.RotationState.Following:
			case FreeControllerV3.RotationState.Hold:
			case FreeControllerV3.RotationState.LookAt:
			case FreeControllerV3.RotationState.ParentLink:
			case FreeControllerV3.RotationState.PhysicsLink:
				if (this._isGrabbing && this._currentRotationState == FreeControllerV3.RotationState.ParentLink && this.preLinkRotationState == FreeControllerV3.RotationState.Lock)
				{
					jointDrive.positionSpring = this._RBLockRotationSpring * num;
					jointDrive.positionDamper = this._RBLockRotationDamper * num2;
					jointDrive.maximumForce = this._RBLockRotationMaxForce * num3;
				}
				else
				{
					jointDrive.positionSpring = this._RBHoldRotationSpring * num;
					jointDrive.positionDamper = this._RBHoldRotationDamper * num2;
					jointDrive.maximumForce = this._RBHoldRotationMaxForce * num3;
				}
				break;
			case FreeControllerV3.RotationState.Off:
				jointDrive.positionSpring = 0f;
				jointDrive.positionDamper = 0f;
				jointDrive.maximumForce = 0f;
				break;
			case FreeControllerV3.RotationState.Lock:
				jointDrive.positionSpring = this._RBLockRotationSpring * num;
				jointDrive.positionDamper = this._RBLockRotationDamper * num2;
				jointDrive.maximumForce = this._RBLockRotationMaxForce * num3;
				break;
			case FreeControllerV3.RotationState.Comply:
				jointDrive.positionSpring = this._RBComplyRotationSpring * num * this.RBMass;
				jointDrive.positionDamper = this._RBComplyRotationDamper * num2 * this.RBMass;
				jointDrive.maximumForce = this._RBComplyRotationMaxForce * num3;
				break;
			}
			this.connectedJoint.slerpDrive = jointDrive;
			this.connectedJoint.angularXDrive = jointDrive;
			this.connectedJoint.angularYZDrive = jointDrive;
			this._followWhenOffRB.WakeUp();
		}
	}

	// Token: 0x060053CC RID: 21452 RVA: 0x001E7724 File Offset: 0x001E5B24
	private void SetNaturalJointDrive()
	{
		if (this.naturalJoint != null)
		{
			float num = this.scalePow;
			float num2 = this.scalePow;
			float num3 = this.scalePow;
			JointDrive slerpDrive = this.naturalJoint.slerpDrive;
			if (this._currentRotationState == FreeControllerV3.RotationState.Comply)
			{
				slerpDrive.positionSpring = this._RBComplyJointRotationDriveSpring * num;
			}
			else
			{
				slerpDrive.positionSpring = this._jointRotationDriveSpring * num;
			}
			slerpDrive.positionDamper = this._jointRotationDriveDamper * num2;
			slerpDrive.maximumForce = this._jointRotationDriveMaxForce * num3;
			this.naturalJoint.slerpDrive = slerpDrive;
		}
	}

	// Token: 0x060053CD RID: 21453 RVA: 0x001E77BC File Offset: 0x001E5BBC
	private void SetNaturalJointDriveTarget()
	{
		if (this.naturalJoint != null)
		{
			Quaternion quaternion = Quaternion.Euler(this._jointRotationDriveXTarget + this._jointRotationDriveXTargetAdditional, 0f, 0f);
			Quaternion quaternion2 = Quaternion.Euler(0f, this._jointRotationDriveYTarget + this._jointRotationDriveYTargetAdditional, 0f);
			Quaternion quaternion3 = Quaternion.Euler(0f, 0f, this._jointRotationDriveZTarget + this._jointRotationDriveZTargetAdditional);
			Quaternion targetRotation = quaternion;
			switch (this.naturalJointDriveRotationOrder)
			{
			case Quaternion2Angles.RotationOrder.XYZ:
				targetRotation = quaternion * quaternion2 * quaternion3;
				break;
			case Quaternion2Angles.RotationOrder.XZY:
				targetRotation = quaternion * quaternion3 * quaternion2;
				break;
			case Quaternion2Angles.RotationOrder.YXZ:
				targetRotation = quaternion2 * quaternion * quaternion3;
				break;
			case Quaternion2Angles.RotationOrder.YZX:
				targetRotation = quaternion2 * quaternion3 * quaternion3;
				break;
			case Quaternion2Angles.RotationOrder.ZXY:
				targetRotation = quaternion3 * quaternion * quaternion2;
				break;
			case Quaternion2Angles.RotationOrder.ZYX:
				targetRotation = quaternion3 * quaternion2 * quaternion;
				break;
			}
			this.naturalJoint.targetRotation = targetRotation;
		}
	}

	// Token: 0x060053CE RID: 21454 RVA: 0x001E78DD File Offset: 0x001E5CDD
	private void SyncXLock(bool b)
	{
		this._xLock = b;
	}

	// Token: 0x17000C30 RID: 3120
	// (get) Token: 0x060053CF RID: 21455 RVA: 0x001E78E6 File Offset: 0x001E5CE6
	// (set) Token: 0x060053D0 RID: 21456 RVA: 0x001E78EE File Offset: 0x001E5CEE
	public bool xLock
	{
		get
		{
			return this._xLock;
		}
		set
		{
			if (this.xLockJSON != null)
			{
				this.xLockJSON.val = value;
			}
			else if (this._xLock != value)
			{
				this.SyncXLock(value);
			}
		}
	}

	// Token: 0x060053D1 RID: 21457 RVA: 0x001E791F File Offset: 0x001E5D1F
	private void SyncYLock(bool b)
	{
		this._yLock = b;
	}

	// Token: 0x17000C31 RID: 3121
	// (get) Token: 0x060053D2 RID: 21458 RVA: 0x001E7928 File Offset: 0x001E5D28
	// (set) Token: 0x060053D3 RID: 21459 RVA: 0x001E7930 File Offset: 0x001E5D30
	public bool yLock
	{
		get
		{
			return this._yLock;
		}
		set
		{
			if (this.yLockJSON != null)
			{
				this.yLockJSON.val = value;
			}
			else if (this._yLock != value)
			{
				this.SyncYLock(value);
			}
		}
	}

	// Token: 0x060053D4 RID: 21460 RVA: 0x001E7961 File Offset: 0x001E5D61
	private void SyncZLock(bool b)
	{
		this._zLock = b;
	}

	// Token: 0x17000C32 RID: 3122
	// (get) Token: 0x060053D5 RID: 21461 RVA: 0x001E796A File Offset: 0x001E5D6A
	// (set) Token: 0x060053D6 RID: 21462 RVA: 0x001E7972 File Offset: 0x001E5D72
	public bool zLock
	{
		get
		{
			return this._zLock;
		}
		set
		{
			if (this.zLockJSON != null)
			{
				this.zLockJSON.val = value;
			}
			else if (this._zLock != value)
			{
				this.SyncZLock(value);
			}
		}
	}

	// Token: 0x060053D7 RID: 21463 RVA: 0x001E79A3 File Offset: 0x001E5DA3
	private void SyncXLocalLock(bool b)
	{
		this._xLocalLock = b;
	}

	// Token: 0x17000C33 RID: 3123
	// (get) Token: 0x060053D8 RID: 21464 RVA: 0x001E79AC File Offset: 0x001E5DAC
	// (set) Token: 0x060053D9 RID: 21465 RVA: 0x001E79B4 File Offset: 0x001E5DB4
	public bool xLocalLock
	{
		get
		{
			return this._xLocalLock;
		}
		set
		{
			if (this.xLocalLockJSON != null)
			{
				this.xLocalLockJSON.val = value;
			}
			else if (this._xLocalLock != value)
			{
				this.SyncXLocalLock(value);
			}
		}
	}

	// Token: 0x060053DA RID: 21466 RVA: 0x001E79E5 File Offset: 0x001E5DE5
	private void SyncYLocalLock(bool b)
	{
		this._yLocalLock = b;
	}

	// Token: 0x17000C34 RID: 3124
	// (get) Token: 0x060053DB RID: 21467 RVA: 0x001E79EE File Offset: 0x001E5DEE
	// (set) Token: 0x060053DC RID: 21468 RVA: 0x001E79F6 File Offset: 0x001E5DF6
	public bool yLocalLock
	{
		get
		{
			return this._yLocalLock;
		}
		set
		{
			if (this.yLocalLockJSON != null)
			{
				this.yLocalLockJSON.val = value;
			}
			else if (this._yLocalLock != value)
			{
				this.SyncYLocalLock(value);
			}
		}
	}

	// Token: 0x060053DD RID: 21469 RVA: 0x001E7A27 File Offset: 0x001E5E27
	private void SyncZLocalLock(bool b)
	{
		this._zLocalLock = b;
	}

	// Token: 0x17000C35 RID: 3125
	// (get) Token: 0x060053DE RID: 21470 RVA: 0x001E7A30 File Offset: 0x001E5E30
	// (set) Token: 0x060053DF RID: 21471 RVA: 0x001E7A38 File Offset: 0x001E5E38
	public bool zLocalLock
	{
		get
		{
			return this._zLocalLock;
		}
		set
		{
			if (this.zLocalLockJSON != null)
			{
				this.zLocalLockJSON.val = value;
			}
			else if (this._zLocalLock != value)
			{
				this.SyncZLocalLock(value);
			}
		}
	}

	// Token: 0x060053E0 RID: 21472 RVA: 0x001E7A69 File Offset: 0x001E5E69
	private void SyncXRotLock(bool b)
	{
		this._xRotLock = b;
	}

	// Token: 0x17000C36 RID: 3126
	// (get) Token: 0x060053E1 RID: 21473 RVA: 0x001E7A72 File Offset: 0x001E5E72
	// (set) Token: 0x060053E2 RID: 21474 RVA: 0x001E7A7A File Offset: 0x001E5E7A
	public bool xRotLock
	{
		get
		{
			return this._xRotLock;
		}
		set
		{
			if (this.xRotLockJSON != null)
			{
				this.xRotLockJSON.val = value;
			}
			else if (this._xRotLock != value)
			{
				this.SyncXRotLock(value);
			}
		}
	}

	// Token: 0x060053E3 RID: 21475 RVA: 0x001E7AAB File Offset: 0x001E5EAB
	private void SyncYRotLock(bool b)
	{
		this._yRotLock = b;
	}

	// Token: 0x17000C37 RID: 3127
	// (get) Token: 0x060053E4 RID: 21476 RVA: 0x001E7AB4 File Offset: 0x001E5EB4
	// (set) Token: 0x060053E5 RID: 21477 RVA: 0x001E7ABC File Offset: 0x001E5EBC
	public bool yRotLock
	{
		get
		{
			return this._yRotLock;
		}
		set
		{
			if (this.yRotLockJSON != null)
			{
				this.yRotLockJSON.val = value;
			}
			else if (this._yRotLock != value)
			{
				this.SyncYRotLock(value);
			}
		}
	}

	// Token: 0x060053E6 RID: 21478 RVA: 0x001E7AED File Offset: 0x001E5EED
	private void SyncZRotLock(bool b)
	{
		this._zRotLock = b;
	}

	// Token: 0x17000C38 RID: 3128
	// (get) Token: 0x060053E7 RID: 21479 RVA: 0x001E7AF6 File Offset: 0x001E5EF6
	// (set) Token: 0x060053E8 RID: 21480 RVA: 0x001E7AFE File Offset: 0x001E5EFE
	public bool zRotLock
	{
		get
		{
			return this._zRotLock;
		}
		set
		{
			if (this.zRotLockJSON != null)
			{
				this.zRotLockJSON.val = value;
			}
			else if (this._zRotLock != value)
			{
				this.SyncZRotLock(value);
			}
		}
	}

	// Token: 0x060053E9 RID: 21481 RVA: 0x001E7B30 File Offset: 0x001E5F30
	public void SetXPositionNoForce(float f)
	{
		Vector3 position = this.control.position;
		position.x = f;
		this.SetPositionNoForce(position);
	}

	// Token: 0x060053EA RID: 21482 RVA: 0x001E7B58 File Offset: 0x001E5F58
	private void SetXPositionNoForce(string val)
	{
		float xpositionNoForce;
		if (float.TryParse(val, out xpositionNoForce))
		{
			this.SetXPositionNoForce(xpositionNoForce);
		}
	}

	// Token: 0x060053EB RID: 21483 RVA: 0x001E7B79 File Offset: 0x001E5F79
	private void SetXPositionToInputField()
	{
		if (this.xPositionInputField != null)
		{
			this.SetXPositionNoForce(this.xPositionInputField.text);
			this.xPositionInputField.text = string.Empty;
		}
	}

	// Token: 0x060053EC RID: 21484 RVA: 0x001E7BB0 File Offset: 0x001E5FB0
	public void SetYPositionNoForce(float f)
	{
		Vector3 position = this.control.position;
		position.y = f;
		this.SetPositionNoForce(position);
	}

	// Token: 0x060053ED RID: 21485 RVA: 0x001E7BD8 File Offset: 0x001E5FD8
	private void SetYPositionNoForce(string val)
	{
		float ypositionNoForce;
		if (float.TryParse(val, out ypositionNoForce))
		{
			this.SetYPositionNoForce(ypositionNoForce);
		}
	}

	// Token: 0x060053EE RID: 21486 RVA: 0x001E7BF9 File Offset: 0x001E5FF9
	private void SetYPositionToInputField()
	{
		if (this.yPositionInputField != null)
		{
			this.SetYPositionNoForce(this.yPositionInputField.text);
			this.yPositionInputField.text = string.Empty;
		}
	}

	// Token: 0x060053EF RID: 21487 RVA: 0x001E7C30 File Offset: 0x001E6030
	public void SetZPositionNoForce(float f)
	{
		Vector3 position = this.control.position;
		position.z = f;
		this.SetPositionNoForce(position);
	}

	// Token: 0x060053F0 RID: 21488 RVA: 0x001E7C58 File Offset: 0x001E6058
	private void SetZPositionNoForce(string val)
	{
		float zpositionNoForce;
		if (float.TryParse(val, out zpositionNoForce))
		{
			this.SetZPositionNoForce(zpositionNoForce);
		}
	}

	// Token: 0x060053F1 RID: 21489 RVA: 0x001E7C79 File Offset: 0x001E6079
	private void SetZPositionToInputField()
	{
		if (this.zPositionInputField != null)
		{
			this.SetZPositionNoForce(this.zPositionInputField.text);
			this.zPositionInputField.text = string.Empty;
		}
	}

	// Token: 0x060053F2 RID: 21490 RVA: 0x001E7CB0 File Offset: 0x001E60B0
	public void SetXRotationNoForce(float f)
	{
		Vector3 eulerAngles = this.control.eulerAngles;
		eulerAngles.x = f;
		this.SetRotationNoForce(eulerAngles);
	}

	// Token: 0x060053F3 RID: 21491 RVA: 0x001E7CD8 File Offset: 0x001E60D8
	private void SetXRotationNoForce(string val)
	{
		float xrotationNoForce;
		if (float.TryParse(val, out xrotationNoForce))
		{
			this.SetXRotationNoForce(xrotationNoForce);
		}
	}

	// Token: 0x060053F4 RID: 21492 RVA: 0x001E7CF9 File Offset: 0x001E60F9
	private void SetXRotationToInputField()
	{
		if (this.xRotationInputField != null)
		{
			this.SetXRotationNoForce(this.xRotationInputField.text);
			this.xRotationInputField.text = string.Empty;
		}
	}

	// Token: 0x060053F5 RID: 21493 RVA: 0x001E7D30 File Offset: 0x001E6130
	public void SetYRotationNoForce(float f)
	{
		Vector3 eulerAngles = this.control.eulerAngles;
		eulerAngles.y = f;
		this.SetRotationNoForce(eulerAngles);
	}

	// Token: 0x060053F6 RID: 21494 RVA: 0x001E7D58 File Offset: 0x001E6158
	private void SetYRotationNoForce(string val)
	{
		float yrotationNoForce;
		if (float.TryParse(val, out yrotationNoForce))
		{
			this.SetYRotationNoForce(yrotationNoForce);
		}
	}

	// Token: 0x060053F7 RID: 21495 RVA: 0x001E7D79 File Offset: 0x001E6179
	private void SetYRotationToInputField()
	{
		if (this.yRotationInputField != null)
		{
			this.SetYRotationNoForce(this.yRotationInputField.text);
			this.yRotationInputField.text = string.Empty;
		}
	}

	// Token: 0x060053F8 RID: 21496 RVA: 0x001E7DB0 File Offset: 0x001E61B0
	public void SetZRotationNoForce(float f)
	{
		Vector3 eulerAngles = this.control.eulerAngles;
		eulerAngles.z = f;
		this.SetRotationNoForce(eulerAngles);
	}

	// Token: 0x060053F9 RID: 21497 RVA: 0x001E7DD8 File Offset: 0x001E61D8
	private void SetZRotationNoForce(string val)
	{
		float zrotationNoForce;
		if (float.TryParse(val, out zrotationNoForce))
		{
			this.SetZRotationNoForce(zrotationNoForce);
		}
	}

	// Token: 0x060053FA RID: 21498 RVA: 0x001E7DF9 File Offset: 0x001E61F9
	private void SetZRotationToInputField()
	{
		if (this.zRotationInputField != null)
		{
			this.SetZRotationNoForce(this.zRotationInputField.text);
			this.zRotationInputField.text = string.Empty;
		}
	}

	// Token: 0x17000C39 RID: 3129
	// (get) Token: 0x060053FB RID: 21499 RVA: 0x001E7E30 File Offset: 0x001E6230
	public Transform ControlParentToUse
	{
		get
		{
			if (this.containingAtom != null && this.containingAtom.reParentObject != null && this.containingAtom.reParentObject == this.control.parent)
			{
				return this.containingAtom.reParentObject.parent;
			}
			return this.control.parent;
		}
	}

	// Token: 0x060053FC RID: 21500 RVA: 0x001E7EA0 File Offset: 0x001E62A0
	public Vector3 GetLocalPosition()
	{
		Transform controlParentToUse = this.ControlParentToUse;
		if (controlParentToUse != null)
		{
			return controlParentToUse.InverseTransformPoint(this.control.position);
		}
		return this.control.localPosition;
	}

	// Token: 0x060053FD RID: 21501 RVA: 0x001E7EE0 File Offset: 0x001E62E0
	public void SetLocalPosition(Vector3 localPosition)
	{
		Transform controlParentToUse = this.ControlParentToUse;
		if (controlParentToUse != null)
		{
			this.control.position = controlParentToUse.TransformPoint(localPosition);
		}
		else
		{
			this.control.localPosition = localPosition;
		}
	}

	// Token: 0x060053FE RID: 21502 RVA: 0x001E7F24 File Offset: 0x001E6324
	public Vector3 GetLocalEulerAngles()
	{
		Transform controlParentToUse = this.ControlParentToUse;
		if (controlParentToUse != null && controlParentToUse != this.control.parent)
		{
			return (Quaternion.Inverse(controlParentToUse.rotation) * this.control.rotation).eulerAngles;
		}
		return this.control.localEulerAngles;
	}

	// Token: 0x060053FF RID: 21503 RVA: 0x001E7F8C File Offset: 0x001E638C
	public void SetLocalEulerAngles(Vector3 eulerAngles)
	{
		Transform controlParentToUse = this.ControlParentToUse;
		if (controlParentToUse != null && controlParentToUse != this.control.parent)
		{
			Quaternion rhs = Quaternion.Euler(eulerAngles);
			this.control.rotation = controlParentToUse.rotation * rhs;
		}
		else
		{
			this.control.localEulerAngles = eulerAngles;
		}
	}

	// Token: 0x06005400 RID: 21504 RVA: 0x001E7FF4 File Offset: 0x001E63F4
	public void SetXLocalPositionNoForce(float f)
	{
		Vector3 localPosition = this.GetLocalPosition();
		localPosition.x = f;
		this.SetLocalPositionNoForce(localPosition);
	}

	// Token: 0x06005401 RID: 21505 RVA: 0x001E8018 File Offset: 0x001E6418
	private void SetXLocalPositionNoForce(string val)
	{
		float xlocalPositionNoForce;
		if (float.TryParse(val, out xlocalPositionNoForce))
		{
			this.SetXLocalPositionNoForce(xlocalPositionNoForce);
		}
		if (this.xLocalPositionInputField != null)
		{
			this.xLocalPositionInputField.text = string.Empty;
		}
	}

	// Token: 0x06005402 RID: 21506 RVA: 0x001E805A File Offset: 0x001E645A
	private void SetXLocalPositionToInputField()
	{
		if (this.xLocalPositionInputField != null)
		{
			this.SetXLocalPositionNoForce(this.xLocalPositionInputField.text);
			this.xLocalPositionInputField.text = string.Empty;
		}
	}

	// Token: 0x06005403 RID: 21507 RVA: 0x001E8090 File Offset: 0x001E6490
	public void SetYLocalPositionNoForce(float f)
	{
		Vector3 localPosition = this.GetLocalPosition();
		localPosition.y = f;
		this.SetLocalPositionNoForce(localPosition);
	}

	// Token: 0x06005404 RID: 21508 RVA: 0x001E80B4 File Offset: 0x001E64B4
	private void SetYLocalPositionNoForce(string val)
	{
		float ylocalPositionNoForce;
		if (float.TryParse(val, out ylocalPositionNoForce))
		{
			this.SetYLocalPositionNoForce(ylocalPositionNoForce);
		}
		if (this.yLocalPositionInputField != null)
		{
			this.yLocalPositionInputField.text = string.Empty;
		}
	}

	// Token: 0x06005405 RID: 21509 RVA: 0x001E80F6 File Offset: 0x001E64F6
	private void SetYLocalPositionToInputField()
	{
		if (this.yLocalPositionInputField != null)
		{
			this.SetYLocalPositionNoForce(this.yLocalPositionInputField.text);
			this.yLocalPositionInputField.text = string.Empty;
		}
	}

	// Token: 0x06005406 RID: 21510 RVA: 0x001E812C File Offset: 0x001E652C
	public void SetZLocalPositionNoForce(float f)
	{
		Vector3 localPosition = this.GetLocalPosition();
		localPosition.z = f;
		this.SetLocalPositionNoForce(localPosition);
	}

	// Token: 0x06005407 RID: 21511 RVA: 0x001E8150 File Offset: 0x001E6550
	private void SetZLocalPositionNoForce(string val)
	{
		float zlocalPositionNoForce;
		if (float.TryParse(val, out zlocalPositionNoForce))
		{
			this.SetZLocalPositionNoForce(zlocalPositionNoForce);
		}
		if (this.zLocalPositionInputField != null)
		{
			this.zLocalPositionInputField.text = string.Empty;
		}
	}

	// Token: 0x06005408 RID: 21512 RVA: 0x001E8192 File Offset: 0x001E6592
	private void SetZLocalPositionToInputField()
	{
		if (this.zLocalPositionInputField != null)
		{
			this.SetZLocalPositionNoForce(this.zLocalPositionInputField.text);
			this.zLocalPositionInputField.text = string.Empty;
		}
	}

	// Token: 0x06005409 RID: 21513 RVA: 0x001E81C8 File Offset: 0x001E65C8
	public void SetXLocalRotationNoForce(float f)
	{
		Vector3 localEulerAngles = this.GetLocalEulerAngles();
		localEulerAngles.x = f;
		this.SetLocalRotationNoForce(localEulerAngles);
	}

	// Token: 0x0600540A RID: 21514 RVA: 0x001E81EC File Offset: 0x001E65EC
	private void SetXLocalRotationNoForce(string val)
	{
		float xlocalRotationNoForce;
		if (float.TryParse(val, out xlocalRotationNoForce))
		{
			this.SetXLocalRotationNoForce(xlocalRotationNoForce);
		}
		if (this.xLocalRotationInputField != null)
		{
			this.xLocalRotationInputField.text = string.Empty;
		}
	}

	// Token: 0x0600540B RID: 21515 RVA: 0x001E822E File Offset: 0x001E662E
	private void SetXLocalRotationToInputField()
	{
		if (this.xLocalRotationInputField != null)
		{
			this.SetXLocalRotationNoForce(this.xLocalRotationInputField.text);
			this.xLocalRotationInputField.text = string.Empty;
		}
	}

	// Token: 0x0600540C RID: 21516 RVA: 0x001E8264 File Offset: 0x001E6664
	public void SetYLocalRotationNoForce(float f)
	{
		Vector3 localEulerAngles = this.GetLocalEulerAngles();
		localEulerAngles.y = f;
		this.SetLocalRotationNoForce(localEulerAngles);
	}

	// Token: 0x0600540D RID: 21517 RVA: 0x001E8288 File Offset: 0x001E6688
	private void SetYLocalRotationNoForce(string val)
	{
		float ylocalRotationNoForce;
		if (float.TryParse(val, out ylocalRotationNoForce))
		{
			this.SetYLocalRotationNoForce(ylocalRotationNoForce);
		}
		if (this.yLocalRotationInputField != null)
		{
			this.yLocalRotationInputField.text = string.Empty;
		}
	}

	// Token: 0x0600540E RID: 21518 RVA: 0x001E82CA File Offset: 0x001E66CA
	private void SetYLocalRotationToInputField()
	{
		if (this.yLocalRotationInputField != null)
		{
			this.SetYLocalRotationNoForce(this.yLocalRotationInputField.text);
			this.yLocalRotationInputField.text = string.Empty;
		}
	}

	// Token: 0x0600540F RID: 21519 RVA: 0x001E8300 File Offset: 0x001E6700
	public void SetZLocalRotationNoForce(float f)
	{
		Vector3 localEulerAngles = this.GetLocalEulerAngles();
		localEulerAngles.z = f;
		this.SetLocalRotationNoForce(localEulerAngles);
	}

	// Token: 0x06005410 RID: 21520 RVA: 0x001E8324 File Offset: 0x001E6724
	private void SetZLocalRotationNoForce(string val)
	{
		float zlocalRotationNoForce;
		if (float.TryParse(val, out zlocalRotationNoForce))
		{
			this.SetZLocalRotationNoForce(zlocalRotationNoForce);
		}
		if (this.zLocalRotationInputField != null)
		{
			this.zLocalRotationInputField.text = string.Empty;
		}
	}

	// Token: 0x06005411 RID: 21521 RVA: 0x001E8366 File Offset: 0x001E6766
	private void SetZLocalRotationToInputField()
	{
		if (this.zLocalRotationInputField != null)
		{
			this.SetZLocalRotationNoForce(this.zLocalRotationInputField.text);
			this.zLocalRotationInputField.text = string.Empty;
		}
	}

	// Token: 0x06005412 RID: 21522 RVA: 0x001E839C File Offset: 0x001E679C
	public void XPositionSnapPoint1()
	{
		Vector3 position = this.control.position;
		position.x *= 10f;
		position.x = Mathf.Round(position.x);
		position.x /= 10f;
		this.control.position = position;
		if (this.onPositionChangeHandlers != null)
		{
			this.onPositionChangeHandlers(this);
		}
		if (this.onMovementHandlers != null)
		{
			this.onMovementHandlers(this);
		}
	}

	// Token: 0x06005413 RID: 21523 RVA: 0x001E8428 File Offset: 0x001E6828
	public void YPositionSnapPoint1()
	{
		Vector3 position = this.control.position;
		position.y *= 10f;
		position.y = Mathf.Round(position.y);
		position.y /= 10f;
		this.control.position = position;
		if (this.onPositionChangeHandlers != null)
		{
			this.onPositionChangeHandlers(this);
		}
		if (this.onMovementHandlers != null)
		{
			this.onMovementHandlers(this);
		}
	}

	// Token: 0x06005414 RID: 21524 RVA: 0x001E84B4 File Offset: 0x001E68B4
	public void ZPositionSnapPoint1()
	{
		Vector3 position = this.control.position;
		position.z *= 10f;
		position.z = Mathf.Round(position.z);
		position.z /= 10f;
		this.control.position = position;
		if (this.onPositionChangeHandlers != null)
		{
			this.onPositionChangeHandlers(this);
		}
		if (this.onMovementHandlers != null)
		{
			this.onMovementHandlers(this);
		}
	}

	// Token: 0x06005415 RID: 21525 RVA: 0x001E8540 File Offset: 0x001E6940
	public void XLocalPositionSnapPoint1()
	{
		Vector3 localPosition = this.GetLocalPosition();
		localPosition.x *= 10f;
		localPosition.x = Mathf.Round(localPosition.x);
		localPosition.x /= 10f;
		this.SetLocalPosition(localPosition);
		if (this.onPositionChangeHandlers != null)
		{
			this.onPositionChangeHandlers(this);
		}
		if (this.onMovementHandlers != null)
		{
			this.onMovementHandlers(this);
		}
	}

	// Token: 0x06005416 RID: 21526 RVA: 0x001E85C4 File Offset: 0x001E69C4
	public void YLocalPositionSnapPoint1()
	{
		Vector3 localPosition = this.GetLocalPosition();
		localPosition.y *= 10f;
		localPosition.y = Mathf.Round(localPosition.y);
		localPosition.y /= 10f;
		this.SetLocalPosition(localPosition);
		if (this.onPositionChangeHandlers != null)
		{
			this.onPositionChangeHandlers(this);
		}
		if (this.onMovementHandlers != null)
		{
			this.onMovementHandlers(this);
		}
	}

	// Token: 0x06005417 RID: 21527 RVA: 0x001E8648 File Offset: 0x001E6A48
	public void ZLocalPositionSnapPoint1()
	{
		Vector3 localPosition = this.GetLocalPosition();
		localPosition.z *= 10f;
		localPosition.z = Mathf.Round(localPosition.x);
		localPosition.z /= 10f;
		this.SetLocalPosition(localPosition);
		if (this.onPositionChangeHandlers != null)
		{
			this.onPositionChangeHandlers(this);
		}
		if (this.onMovementHandlers != null)
		{
			this.onMovementHandlers(this);
		}
	}

	// Token: 0x06005418 RID: 21528 RVA: 0x001E86CC File Offset: 0x001E6ACC
	public void XRotationSnap1()
	{
		Vector3 eulerAngles = this.control.eulerAngles;
		eulerAngles.x = Mathf.Round(eulerAngles.x);
		this.control.eulerAngles = eulerAngles;
		if (this.onRotationChangeHandlers != null)
		{
			this.onRotationChangeHandlers(this);
		}
		if (this.onMovementHandlers != null)
		{
			this.onMovementHandlers(this);
		}
	}

	// Token: 0x06005419 RID: 21529 RVA: 0x001E8734 File Offset: 0x001E6B34
	public void YRotationSnap1()
	{
		Vector3 eulerAngles = this.control.eulerAngles;
		eulerAngles.y = Mathf.Round(eulerAngles.y);
		this.control.eulerAngles = eulerAngles;
		if (this.onRotationChangeHandlers != null)
		{
			this.onRotationChangeHandlers(this);
		}
		if (this.onMovementHandlers != null)
		{
			this.onMovementHandlers(this);
		}
	}

	// Token: 0x0600541A RID: 21530 RVA: 0x001E879C File Offset: 0x001E6B9C
	public void ZRotationSnap1()
	{
		Vector3 eulerAngles = this.control.eulerAngles;
		eulerAngles.z = Mathf.Round(eulerAngles.z);
		this.control.eulerAngles = eulerAngles;
		if (this.onRotationChangeHandlers != null)
		{
			this.onRotationChangeHandlers(this);
		}
		if (this.onMovementHandlers != null)
		{
			this.onMovementHandlers(this);
		}
	}

	// Token: 0x0600541B RID: 21531 RVA: 0x001E8804 File Offset: 0x001E6C04
	public void XLocalRotationSnap1()
	{
		Vector3 localEulerAngles = this.GetLocalEulerAngles();
		localEulerAngles.x = Mathf.Round(localEulerAngles.x);
		this.SetLocalEulerAngles(localEulerAngles);
		if (this.onRotationChangeHandlers != null)
		{
			this.onRotationChangeHandlers(this);
		}
		if (this.onMovementHandlers != null)
		{
			this.onMovementHandlers(this);
		}
	}

	// Token: 0x0600541C RID: 21532 RVA: 0x001E8860 File Offset: 0x001E6C60
	public void YLocalRotationSnap1()
	{
		Vector3 localEulerAngles = this.GetLocalEulerAngles();
		localEulerAngles.y = Mathf.Round(localEulerAngles.y);
		this.SetLocalEulerAngles(localEulerAngles);
		if (this.onRotationChangeHandlers != null)
		{
			this.onRotationChangeHandlers(this);
		}
		if (this.onMovementHandlers != null)
		{
			this.onMovementHandlers(this);
		}
	}

	// Token: 0x0600541D RID: 21533 RVA: 0x001E88BC File Offset: 0x001E6CBC
	public void ZLocalRotationSnap1()
	{
		Vector3 localEulerAngles = this.GetLocalEulerAngles();
		localEulerAngles.z = Mathf.Round(localEulerAngles.z);
		this.SetLocalEulerAngles(localEulerAngles);
		if (this.onRotationChangeHandlers != null)
		{
			this.onRotationChangeHandlers(this);
		}
		if (this.onMovementHandlers != null)
		{
			this.onMovementHandlers(this);
		}
	}

	// Token: 0x0600541E RID: 21534 RVA: 0x001E8918 File Offset: 0x001E6D18
	public void XRotation0()
	{
		Vector3 eulerAngles = this.control.eulerAngles;
		eulerAngles.x = 0f;
		this.control.eulerAngles = eulerAngles;
		if (this.onRotationChangeHandlers != null)
		{
			this.onRotationChangeHandlers(this);
		}
		if (this.onMovementHandlers != null)
		{
			this.onMovementHandlers(this);
		}
	}

	// Token: 0x0600541F RID: 21535 RVA: 0x001E8978 File Offset: 0x001E6D78
	public void YRotation0()
	{
		Vector3 eulerAngles = this.control.eulerAngles;
		eulerAngles.y = 0f;
		this.control.eulerAngles = eulerAngles;
		if (this.onRotationChangeHandlers != null)
		{
			this.onRotationChangeHandlers(this);
		}
		if (this.onMovementHandlers != null)
		{
			this.onMovementHandlers(this);
		}
	}

	// Token: 0x06005420 RID: 21536 RVA: 0x001E89D8 File Offset: 0x001E6DD8
	public void ZRotation0()
	{
		Vector3 eulerAngles = this.control.eulerAngles;
		eulerAngles.z = 0f;
		this.control.eulerAngles = eulerAngles;
		if (this.onRotationChangeHandlers != null)
		{
			this.onRotationChangeHandlers(this);
		}
		if (this.onMovementHandlers != null)
		{
			this.onMovementHandlers(this);
		}
	}

	// Token: 0x06005421 RID: 21537 RVA: 0x001E8A38 File Offset: 0x001E6E38
	public void XLocalRotation0()
	{
		Vector3 localEulerAngles = this.GetLocalEulerAngles();
		localEulerAngles.x = 0f;
		this.SetLocalEulerAngles(localEulerAngles);
		if (this.onRotationChangeHandlers != null)
		{
			this.onRotationChangeHandlers(this);
		}
		if (this.onMovementHandlers != null)
		{
			this.onMovementHandlers(this);
		}
	}

	// Token: 0x06005422 RID: 21538 RVA: 0x001E8A90 File Offset: 0x001E6E90
	public void YLocalRotation0()
	{
		Vector3 localEulerAngles = this.GetLocalEulerAngles();
		localEulerAngles.y = 0f;
		this.SetLocalEulerAngles(localEulerAngles);
		if (this.onRotationChangeHandlers != null)
		{
			this.onRotationChangeHandlers(this);
		}
		if (this.onMovementHandlers != null)
		{
			this.onMovementHandlers(this);
		}
	}

	// Token: 0x06005423 RID: 21539 RVA: 0x001E8AE8 File Offset: 0x001E6EE8
	public void ZLocalRotation0()
	{
		Vector3 localEulerAngles = this.GetLocalEulerAngles();
		localEulerAngles.z = 0f;
		this.SetLocalEulerAngles(localEulerAngles);
		if (this.onRotationChangeHandlers != null)
		{
			this.onRotationChangeHandlers(this);
		}
		if (this.onMovementHandlers != null)
		{
			this.onMovementHandlers(this);
		}
	}

	// Token: 0x06005424 RID: 21540 RVA: 0x001E8B40 File Offset: 0x001E6F40
	public void XRotationAdd(float a)
	{
		Vector3 eulerAngles = this.control.eulerAngles;
		eulerAngles.x += a;
		this.control.eulerAngles = eulerAngles;
		if (this.onRotationChangeHandlers != null)
		{
			this.onRotationChangeHandlers(this);
		}
		if (this.onMovementHandlers != null)
		{
			this.onMovementHandlers(this);
		}
	}

	// Token: 0x06005425 RID: 21541 RVA: 0x001E8BA4 File Offset: 0x001E6FA4
	public void YRotationAdd(float a)
	{
		Vector3 eulerAngles = this.control.eulerAngles;
		eulerAngles.y += a;
		this.control.eulerAngles = eulerAngles;
		if (this.onRotationChangeHandlers != null)
		{
			this.onRotationChangeHandlers(this);
		}
		if (this.onMovementHandlers != null)
		{
			this.onMovementHandlers(this);
		}
	}

	// Token: 0x06005426 RID: 21542 RVA: 0x001E8C08 File Offset: 0x001E7008
	public void ZRotationAdd(float a)
	{
		Vector3 eulerAngles = this.control.eulerAngles;
		eulerAngles.z += a;
		this.control.eulerAngles = eulerAngles;
		if (this.onRotationChangeHandlers != null)
		{
			this.onRotationChangeHandlers(this);
		}
		if (this.onMovementHandlers != null)
		{
			this.onMovementHandlers(this);
		}
	}

	// Token: 0x06005427 RID: 21543 RVA: 0x001E8C6C File Offset: 0x001E706C
	public void XLocalRotationAdd(float a)
	{
		Vector3 localEulerAngles = this.GetLocalEulerAngles();
		localEulerAngles.x += a;
		this.SetLocalEulerAngles(localEulerAngles);
		if (this.onRotationChangeHandlers != null)
		{
			this.onRotationChangeHandlers(this);
		}
		if (this.onMovementHandlers != null)
		{
			this.onMovementHandlers(this);
		}
	}

	// Token: 0x06005428 RID: 21544 RVA: 0x001E8CC4 File Offset: 0x001E70C4
	public void YLocalRotationAdd(float a)
	{
		Vector3 localEulerAngles = this.GetLocalEulerAngles();
		localEulerAngles.y += a;
		this.SetLocalEulerAngles(localEulerAngles);
		if (this.onRotationChangeHandlers != null)
		{
			this.onRotationChangeHandlers(this);
		}
		if (this.onMovementHandlers != null)
		{
			this.onMovementHandlers(this);
		}
	}

	// Token: 0x06005429 RID: 21545 RVA: 0x001E8D1C File Offset: 0x001E711C
	public void ZLocalRotationAdd(float a)
	{
		Vector3 localEulerAngles = this.GetLocalEulerAngles();
		localEulerAngles.z += a;
		this.SetLocalEulerAngles(localEulerAngles);
		if (this.onRotationChangeHandlers != null)
		{
			this.onRotationChangeHandlers(this);
		}
		if (this.onMovementHandlers != null)
		{
			this.onMovementHandlers(this);
		}
	}

	// Token: 0x0600542A RID: 21546 RVA: 0x001E8D74 File Offset: 0x001E7174
	protected void SyncOn(bool b)
	{
		this._on = b;
		if (this.controlsOn && this.followWhenOff != null)
		{
			this.followWhenOff.gameObject.SetActive(this._on);
		}
	}

	// Token: 0x17000C3A RID: 3130
	// (get) Token: 0x0600542B RID: 21547 RVA: 0x001E8DAF File Offset: 0x001E71AF
	// (set) Token: 0x0600542C RID: 21548 RVA: 0x001E8DB7 File Offset: 0x001E71B7
	public bool on
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

	// Token: 0x0600542D RID: 21549 RVA: 0x001E8DE8 File Offset: 0x001E71E8
	protected void SyncInteractableInPlayMode(bool b)
	{
		this._interactableInPlayMode = b;
	}

	// Token: 0x17000C3B RID: 3131
	// (get) Token: 0x0600542E RID: 21550 RVA: 0x001E8DF1 File Offset: 0x001E71F1
	// (set) Token: 0x0600542F RID: 21551 RVA: 0x001E8DF9 File Offset: 0x001E71F9
	public bool interactableInPlayMode
	{
		get
		{
			return this._interactableInPlayMode;
		}
		set
		{
			if (this.interactableInPlayModeJSON != null)
			{
				this.interactableInPlayModeJSON.val = value;
			}
			else if (this._interactableInPlayMode != value)
			{
				this.SyncInteractableInPlayMode(value);
			}
		}
	}

	// Token: 0x17000C3C RID: 3132
	// (get) Token: 0x06005430 RID: 21552 RVA: 0x001E8E2A File Offset: 0x001E722A
	// (set) Token: 0x06005431 RID: 21553 RVA: 0x001E8E34 File Offset: 0x001E7234
	public bool possessed
	{
		get
		{
			return this._possessed;
		}
		set
		{
			if (this._possessed != value)
			{
				this._possessed = value;
				if (this._possessed)
				{
					if (this.onPossessDeactiveList != null && this._deactivateOtherControlsOnPossess)
					{
						foreach (FreeControllerV3 freeControllerV in this.onPossessDeactiveList)
						{
							freeControllerV.currentPositionState = FreeControllerV3.PositionState.Off;
							freeControllerV.currentRotationState = FreeControllerV3.RotationState.Off;
						}
					}
					if (this.onPossessStartHandlers != null)
					{
						this.onPossessStartHandlers(this);
					}
				}
				else if (this.onPossessEndHandlers != null)
				{
					this.onPossessEndHandlers(this);
				}
			}
		}
	}

	// Token: 0x06005432 RID: 21554 RVA: 0x001E8ED5 File Offset: 0x001E72D5
	protected void SyncDeactivateOtherControlsOnPossess(bool b)
	{
		this._deactivateOtherControlsOnPossess = b;
	}

	// Token: 0x17000C3D RID: 3133
	// (get) Token: 0x06005433 RID: 21555 RVA: 0x001E8EDE File Offset: 0x001E72DE
	// (set) Token: 0x06005434 RID: 21556 RVA: 0x001E8EE6 File Offset: 0x001E72E6
	public bool deactivateOtherControlsOnPossess
	{
		get
		{
			return this._deactivateOtherControlsOnPossess;
		}
		set
		{
			if (this.deactivateOtherControlsOnPossessJSON != null)
			{
				this.deactivateOtherControlsOnPossessJSON.val = value;
			}
			else
			{
				this.SyncDeactivateOtherControlsOnPossess(value);
			}
		}
	}

	// Token: 0x06005435 RID: 21557 RVA: 0x001E8F0B File Offset: 0x001E730B
	protected void SyncPossessable(bool b)
	{
		this._possessable = b;
	}

	// Token: 0x17000C3E RID: 3134
	// (get) Token: 0x06005436 RID: 21558 RVA: 0x001E8F14 File Offset: 0x001E7314
	// (set) Token: 0x06005437 RID: 21559 RVA: 0x001E8F1C File Offset: 0x001E731C
	public bool possessable
	{
		get
		{
			return this._possessable;
		}
		set
		{
			if (this.possessableJSON != null)
			{
				this.possessableJSON.val = value;
			}
			else if (this._possessable != value)
			{
				this.SyncPossessable(value);
			}
		}
	}

	// Token: 0x06005438 RID: 21560 RVA: 0x001E8F4D File Offset: 0x001E734D
	protected void SyncCanGrabPosition(bool b)
	{
		this._canGrabPosition = b;
	}

	// Token: 0x17000C3F RID: 3135
	// (get) Token: 0x06005439 RID: 21561 RVA: 0x001E8F56 File Offset: 0x001E7356
	// (set) Token: 0x0600543A RID: 21562 RVA: 0x001E8F5E File Offset: 0x001E735E
	public bool canGrabPosition
	{
		get
		{
			return this._canGrabPosition;
		}
		set
		{
			if (this.canGrabPositionJSON != null)
			{
				this.canGrabPositionJSON.val = value;
			}
			else if (this._canGrabPosition != value)
			{
				this.SyncCanGrabPosition(value);
			}
		}
	}

	// Token: 0x0600543B RID: 21563 RVA: 0x001E8F8F File Offset: 0x001E738F
	protected void SyncCanGrabRotation(bool b)
	{
		this._canGrabRotation = b;
	}

	// Token: 0x17000C40 RID: 3136
	// (get) Token: 0x0600543C RID: 21564 RVA: 0x001E8F98 File Offset: 0x001E7398
	// (set) Token: 0x0600543D RID: 21565 RVA: 0x001E8FA0 File Offset: 0x001E73A0
	public bool canGrabRotation
	{
		get
		{
			return this._canGrabRotation;
		}
		set
		{
			if (this.canGrabRotationJSON != null)
			{
				this.canGrabRotationJSON.val = value;
			}
			else if (this._canGrabRotation != value)
			{
				this.SyncCanGrabRotation(value);
			}
		}
	}

	// Token: 0x0600543E RID: 21566 RVA: 0x001E8FD1 File Offset: 0x001E73D1
	protected void SyncFreezeAtomPhysicsWhenGrabbed(bool b)
	{
		this.freezeAtomPhysicsWhenGrabbed = b;
	}

	// Token: 0x0600543F RID: 21567 RVA: 0x001E8FDC File Offset: 0x001E73DC
	public void SetPositionGridModeFromString(string gridModeString)
	{
		try
		{
			FreeControllerV3.GridMode positionGridMode = (FreeControllerV3.GridMode)Enum.Parse(typeof(FreeControllerV3.GridMode), gridModeString);
			this._positionGridMode = positionGridMode;
		}
		catch (ArgumentException)
		{
			Debug.LogError("Grid Mode " + gridModeString + " is not a valid grid mode");
		}
	}

	// Token: 0x17000C41 RID: 3137
	// (get) Token: 0x06005440 RID: 21568 RVA: 0x001E9038 File Offset: 0x001E7438
	// (set) Token: 0x06005441 RID: 21569 RVA: 0x001E9040 File Offset: 0x001E7440
	public FreeControllerV3.GridMode positionGridMode
	{
		get
		{
			return this._positionGridMode;
		}
		set
		{
			if (this.positionGridModeJSON != null)
			{
				this.positionGridModeJSON.val = value.ToString();
			}
			else if (this._positionGridMode != value)
			{
				this._positionGridMode = value;
			}
		}
	}

	// Token: 0x06005442 RID: 21570 RVA: 0x001E9080 File Offset: 0x001E7480
	private void SyncPositionGrid(float f)
	{
		float num = f * 1000f;
		num = Mathf.Round(num);
		num /= 1000f;
		if (this.positionGridJSON != null && num != f)
		{
			this.positionGridJSON.valNoCallback = num;
		}
		this._positionGrid = num;
	}

	// Token: 0x17000C42 RID: 3138
	// (get) Token: 0x06005443 RID: 21571 RVA: 0x001E90CB File Offset: 0x001E74CB
	// (set) Token: 0x06005444 RID: 21572 RVA: 0x001E90D3 File Offset: 0x001E74D3
	public float positionGrid
	{
		get
		{
			return this._positionGrid;
		}
		set
		{
			if (this.positionGridJSON != null)
			{
				this.positionGridJSON.val = value;
			}
			else
			{
				this.SyncPositionGrid(value);
			}
		}
	}

	// Token: 0x06005445 RID: 21573 RVA: 0x001E90F8 File Offset: 0x001E74F8
	public void SetRotationGridModeFromString(string gridModeString)
	{
		try
		{
			FreeControllerV3.GridMode rotationGridMode = (FreeControllerV3.GridMode)Enum.Parse(typeof(FreeControllerV3.GridMode), gridModeString);
			this._rotationGridMode = rotationGridMode;
		}
		catch (ArgumentException)
		{
			Debug.LogError("Grid Mode " + gridModeString + " is not a valid grid mode");
		}
	}

	// Token: 0x17000C43 RID: 3139
	// (get) Token: 0x06005446 RID: 21574 RVA: 0x001E9154 File Offset: 0x001E7554
	// (set) Token: 0x06005447 RID: 21575 RVA: 0x001E915C File Offset: 0x001E755C
	public FreeControllerV3.GridMode rotationGridMode
	{
		get
		{
			return this._rotationGridMode;
		}
		set
		{
			if (this.rotationGridModeJSON != null)
			{
				this.rotationGridModeJSON.val = value.ToString();
			}
			else if (this._rotationGridMode != value)
			{
				this._rotationGridMode = value;
			}
		}
	}

	// Token: 0x06005448 RID: 21576 RVA: 0x001E919C File Offset: 0x001E759C
	private void SyncRotationGrid(float f)
	{
		float num = f * 100f;
		num = Mathf.Round(num);
		num /= 100f;
		if (this.rotationGridJSON != null && num != f)
		{
			this.rotationGridJSON.valNoCallback = num;
		}
		this._rotationGrid = num;
	}

	// Token: 0x17000C44 RID: 3140
	// (get) Token: 0x06005449 RID: 21577 RVA: 0x001E91E7 File Offset: 0x001E75E7
	// (set) Token: 0x0600544A RID: 21578 RVA: 0x001E91EF File Offset: 0x001E75EF
	public float rotationGrid
	{
		get
		{
			return this._rotationGrid;
		}
		set
		{
			if (this.rotationGridJSON != null)
			{
				this.rotationGridJSON.val = value;
			}
			else
			{
				this.SyncRotationGrid(value);
			}
		}
	}

	// Token: 0x0600544B RID: 21579 RVA: 0x001E9214 File Offset: 0x001E7614
	private void SyncUseGravityOnRBWhenOff(bool b)
	{
		this._useGravityOnRBWhenOff = b;
		if (this._followWhenOffRB != null)
		{
			this._followWhenOffRB.useGravity = this._useGravityOnRBWhenOff;
		}
	}

	// Token: 0x17000C45 RID: 3141
	// (get) Token: 0x0600544C RID: 21580 RVA: 0x001E923F File Offset: 0x001E763F
	// (set) Token: 0x0600544D RID: 21581 RVA: 0x001E9247 File Offset: 0x001E7647
	public bool useGravityOnRBWhenOff
	{
		get
		{
			return this._useGravityOnRBWhenOff;
		}
		set
		{
			if (this.useGravityJSON != null)
			{
				this.useGravityJSON.val = value;
			}
			else if (this._useGravityOnRBWhenOff != value)
			{
				this.SyncUseGravityOnRBWhenOff(value);
			}
		}
	}

	// Token: 0x0600544E RID: 21582 RVA: 0x001E9278 File Offset: 0x001E7678
	private void SyncPhysicsEnabled(bool b)
	{
		if (this._followWhenOffRB != null)
		{
			this._followWhenOffRB.isKinematic = !b;
			MeshCollider component = this._followWhenOffRB.GetComponent<MeshCollider>();
			if (component != null)
			{
				component.convex = b;
			}
		}
	}

	// Token: 0x17000C46 RID: 3142
	// (get) Token: 0x0600544F RID: 21583 RVA: 0x001E92C4 File Offset: 0x001E76C4
	// (set) Token: 0x06005450 RID: 21584 RVA: 0x001E92E8 File Offset: 0x001E76E8
	public bool physicsEnabled
	{
		get
		{
			return this._followWhenOffRB != null && !this._followWhenOffRB.isKinematic;
		}
		set
		{
			if (this.physicsEnabledJSON != null)
			{
				this.physicsEnabledJSON.val = value;
			}
			else if (this._followWhenOffRB != null && this._followWhenOffRB.isKinematic == value)
			{
				this.SyncPhysicsEnabled(value);
			}
		}
	}

	// Token: 0x06005451 RID: 21585 RVA: 0x001E933C File Offset: 0x001E773C
	private void SyncCollisionEnabled(bool b)
	{
		this._collisionEnabled = b;
		if (this.controlsCollisionEnabled && this._followWhenOffRB != null)
		{
			this._followWhenOffRB.detectCollisions = (this._collisionEnabled && this._globalCollisionEnabled);
		}
	}

	// Token: 0x17000C47 RID: 3143
	// (get) Token: 0x06005452 RID: 21586 RVA: 0x001E938B File Offset: 0x001E778B
	// (set) Token: 0x06005453 RID: 21587 RVA: 0x001E9393 File Offset: 0x001E7793
	public bool globalCollisionEnabled
	{
		get
		{
			return this._globalCollisionEnabled;
		}
		set
		{
			if (this._globalCollisionEnabled != value)
			{
				this._globalCollisionEnabled = value;
				this.SyncCollisionEnabled(this._collisionEnabled);
			}
		}
	}

	// Token: 0x17000C48 RID: 3144
	// (get) Token: 0x06005454 RID: 21588 RVA: 0x001E93B4 File Offset: 0x001E77B4
	// (set) Token: 0x06005455 RID: 21589 RVA: 0x001E93BC File Offset: 0x001E77BC
	public override bool collisionEnabled
	{
		get
		{
			return this._collisionEnabled;
		}
		set
		{
			if (this.collisionEnabledJSON != null)
			{
				this.collisionEnabledJSON.val = value;
			}
			else if (this._collisionEnabled != value)
			{
				this.SyncCollisionEnabled(value);
			}
		}
	}

	// Token: 0x17000C49 RID: 3145
	// (get) Token: 0x06005456 RID: 21590 RVA: 0x001E93ED File Offset: 0x001E77ED
	// (set) Token: 0x06005457 RID: 21591 RVA: 0x001E93F8 File Offset: 0x001E77F8
	public Rigidbody followWhenOffRB
	{
		get
		{
			return this._followWhenOffRB;
		}
		set
		{
			if (this._followWhenOffRB != value)
			{
				this._followWhenOffRB = value;
				this.followWhenOff = this._followWhenOffRB.transform;
				ConfigurableJoint[] components = this.followWhenOff.GetComponents<ConfigurableJoint>();
				foreach (ConfigurableJoint configurableJoint in components)
				{
					if (configurableJoint.connectedBody == this.kinematicRB)
					{
						this.connectedJoint = configurableJoint;
						this.SetJointSprings();
					}
				}
			}
		}
	}

	// Token: 0x06005458 RID: 21592 RVA: 0x001E9478 File Offset: 0x001E7878
	private void SyncRBMass(float f)
	{
		if (this._followWhenOffRB != null)
		{
			this._followWhenOffRB.mass = f;
			this._followWhenOffRB.WakeUp();
			this.SetJointSprings();
		}
		if (this.rigidbodySlavesForMass != null)
		{
			foreach (Rigidbody rigidbody in this.rigidbodySlavesForMass)
			{
				rigidbody.mass = f;
			}
		}
	}

	// Token: 0x17000C4A RID: 3146
	// (get) Token: 0x06005459 RID: 21593 RVA: 0x001E94E4 File Offset: 0x001E78E4
	// (set) Token: 0x0600545A RID: 21594 RVA: 0x001E9508 File Offset: 0x001E7908
	public float RBMass
	{
		get
		{
			if (this._followWhenOffRB != null)
			{
				return this._followWhenOffRB.mass;
			}
			return 0f;
		}
		set
		{
			if (this.RBMassJSON != null)
			{
				this.RBMassJSON.val = value;
			}
			else if (this._followWhenOffRB != null && this._followWhenOffRB.mass != value)
			{
				this.SyncRBMass(value);
			}
		}
	}

	// Token: 0x0600545B RID: 21595 RVA: 0x001E955A File Offset: 0x001E795A
	private void SyncRBDrag(float f)
	{
		if (this._followWhenOffRB != null)
		{
			this._followWhenOffRB.drag = f;
			this._followWhenOffRB.WakeUp();
		}
	}

	// Token: 0x17000C4B RID: 3147
	// (get) Token: 0x0600545C RID: 21596 RVA: 0x001E9584 File Offset: 0x001E7984
	// (set) Token: 0x0600545D RID: 21597 RVA: 0x001E95A8 File Offset: 0x001E79A8
	public float RBDrag
	{
		get
		{
			if (this._followWhenOffRB != null)
			{
				return this._followWhenOffRB.drag;
			}
			return 0f;
		}
		set
		{
			if (this.RBDragJSON != null)
			{
				this.RBDragJSON.val = value;
			}
			else if (this._followWhenOffRB != null && this._followWhenOffRB.drag != value)
			{
				this.SyncRBDrag(value);
			}
		}
	}

	// Token: 0x0600545E RID: 21598 RVA: 0x001E95FA File Offset: 0x001E79FA
	private void SyncRBMaxVelocityEnable(bool b)
	{
		this._RBMaxVelocityEnable = b;
	}

	// Token: 0x17000C4C RID: 3148
	// (get) Token: 0x0600545F RID: 21599 RVA: 0x001E9603 File Offset: 0x001E7A03
	// (set) Token: 0x06005460 RID: 21600 RVA: 0x001E960B File Offset: 0x001E7A0B
	public bool RBMaxVelocityEnable
	{
		get
		{
			return this._RBMaxVelocityEnable;
		}
		set
		{
			if (this.RBMaxVelocityEnableJSON != null)
			{
				this.RBMaxVelocityEnableJSON.val = value;
			}
			else
			{
				this._RBMaxVelocityEnable = value;
			}
		}
	}

	// Token: 0x06005461 RID: 21601 RVA: 0x001E9630 File Offset: 0x001E7A30
	private void SyncRBMaxVelocity(float f)
	{
		this._RBMaxVelocity = f;
	}

	// Token: 0x17000C4D RID: 3149
	// (get) Token: 0x06005462 RID: 21602 RVA: 0x001E9639 File Offset: 0x001E7A39
	// (set) Token: 0x06005463 RID: 21603 RVA: 0x001E9641 File Offset: 0x001E7A41
	public float RBMaxVelocity
	{
		get
		{
			return this._RBMaxVelocity;
		}
		set
		{
			if (this.RBMaxVelocityJSON != null)
			{
				this.RBMaxVelocityJSON.val = value;
			}
			else
			{
				this._RBMaxVelocity = value;
			}
		}
	}

	// Token: 0x06005464 RID: 21604 RVA: 0x001E9666 File Offset: 0x001E7A66
	private void SyncRBAngularDrag(float f)
	{
		if (this._followWhenOffRB != null)
		{
			this._followWhenOffRB.angularDrag = f;
			this._followWhenOffRB.WakeUp();
		}
	}

	// Token: 0x17000C4E RID: 3150
	// (get) Token: 0x06005465 RID: 21605 RVA: 0x001E9690 File Offset: 0x001E7A90
	// (set) Token: 0x06005466 RID: 21606 RVA: 0x001E96B4 File Offset: 0x001E7AB4
	public float RBAngularDrag
	{
		get
		{
			if (this._followWhenOffRB != null)
			{
				return this._followWhenOffRB.angularDrag;
			}
			return 0f;
		}
		set
		{
			if (this.RBAngularDragJSON != null)
			{
				this.RBAngularDragJSON.val = value;
			}
			else if (this._followWhenOffRB != null && this._followWhenOffRB.angularDrag != value)
			{
				this.SyncRBAngularDrag(value);
			}
		}
	}

	// Token: 0x17000C4F RID: 3151
	// (get) Token: 0x06005467 RID: 21607 RVA: 0x001E9706 File Offset: 0x001E7B06
	// (set) Token: 0x06005468 RID: 21608 RVA: 0x001E970E File Offset: 0x001E7B0E
	public float RBLockPositionSpring
	{
		get
		{
			return this._RBLockPositionSpring;
		}
		set
		{
			if (this._RBLockPositionSpring != value)
			{
				this._RBLockPositionSpring = value;
				this.SetJointSprings();
			}
		}
	}

	// Token: 0x17000C50 RID: 3152
	// (get) Token: 0x06005469 RID: 21609 RVA: 0x001E9729 File Offset: 0x001E7B29
	// (set) Token: 0x0600546A RID: 21610 RVA: 0x001E9731 File Offset: 0x001E7B31
	public float RBLockPositionDamper
	{
		get
		{
			return this._RBLockPositionDamper;
		}
		set
		{
			if (this._RBLockPositionDamper != value)
			{
				this._RBLockPositionDamper = value;
				this.SetJointSprings();
			}
		}
	}

	// Token: 0x17000C51 RID: 3153
	// (get) Token: 0x0600546B RID: 21611 RVA: 0x001E974C File Offset: 0x001E7B4C
	// (set) Token: 0x0600546C RID: 21612 RVA: 0x001E9754 File Offset: 0x001E7B54
	public float RBLockPositionMaxForce
	{
		get
		{
			return this._RBLockPositionMaxForce;
		}
		set
		{
			if (this._RBLockPositionMaxForce != value)
			{
				this._RBLockPositionMaxForce = value;
				this.SetJointSprings();
			}
		}
	}

	// Token: 0x0600546D RID: 21613 RVA: 0x001E976F File Offset: 0x001E7B6F
	private void SyncRBHoldPositionSpring(float f)
	{
		this._RBHoldPositionSpring = f;
		this.SetJointSprings();
	}

	// Token: 0x0600546E RID: 21614 RVA: 0x001E977E File Offset: 0x001E7B7E
	public void SetHoldPositionSpringMin()
	{
		if (this.RBHoldPositionSpringJSON != null)
		{
			this.RBHoldPositionSpringJSON.val = this.RBHoldPositionSpringJSON.min;
		}
	}

	// Token: 0x0600546F RID: 21615 RVA: 0x001E97A1 File Offset: 0x001E7BA1
	public void SetHoldPositionSpringMax()
	{
		if (this.RBHoldPositionSpringJSON != null)
		{
			this.RBHoldPositionSpringJSON.val = this.RBHoldPositionSpringJSON.max;
		}
	}

	// Token: 0x06005470 RID: 21616 RVA: 0x001E97C4 File Offset: 0x001E7BC4
	public void SetHoldPositionSpringPercent(float percent)
	{
		if (this.RBHoldPositionSpringJSON != null)
		{
			this.RBHoldPositionSpringJSON.val = (this.RBHoldPositionSpringJSON.max - this.RBHoldPositionSpringJSON.min) * percent + this.RBHoldPositionSpringJSON.min;
		}
	}

	// Token: 0x17000C52 RID: 3154
	// (get) Token: 0x06005471 RID: 21617 RVA: 0x001E9801 File Offset: 0x001E7C01
	// (set) Token: 0x06005472 RID: 21618 RVA: 0x001E9809 File Offset: 0x001E7C09
	public float RBHoldPositionSpring
	{
		get
		{
			return this._RBHoldPositionSpring;
		}
		set
		{
			if (this.RBHoldPositionSpringJSON != null)
			{
				this.RBHoldPositionSpringJSON.val = value;
			}
			else if (this._RBHoldPositionSpring != value)
			{
				this.SyncRBHoldPositionSpring(value);
			}
		}
	}

	// Token: 0x06005473 RID: 21619 RVA: 0x001E983A File Offset: 0x001E7C3A
	private void SyncRBHoldPositionDamper(float f)
	{
		this._RBHoldPositionDamper = f;
		this.SetJointSprings();
	}

	// Token: 0x06005474 RID: 21620 RVA: 0x001E9849 File Offset: 0x001E7C49
	public void SetHoldPositionDamperMin()
	{
		if (this.RBHoldPositionDamperJSON != null)
		{
			this.RBHoldPositionDamperJSON.val = this.RBHoldPositionDamperJSON.min;
		}
	}

	// Token: 0x06005475 RID: 21621 RVA: 0x001E986C File Offset: 0x001E7C6C
	public void SetHoldPositionDamperMax()
	{
		if (this.RBHoldPositionDamperJSON != null)
		{
			this.RBHoldPositionDamperJSON.val = this.RBHoldPositionDamperJSON.max;
		}
	}

	// Token: 0x06005476 RID: 21622 RVA: 0x001E988F File Offset: 0x001E7C8F
	public void SetHoldPositionDamperPercent(float percent)
	{
		if (this.RBHoldPositionDamperJSON != null)
		{
			this.RBHoldPositionDamperJSON.val = (this.RBHoldPositionDamperJSON.max - this.RBHoldPositionDamperJSON.min) * percent + this.RBHoldPositionDamperJSON.min;
		}
	}

	// Token: 0x17000C53 RID: 3155
	// (get) Token: 0x06005477 RID: 21623 RVA: 0x001E98CC File Offset: 0x001E7CCC
	// (set) Token: 0x06005478 RID: 21624 RVA: 0x001E98D4 File Offset: 0x001E7CD4
	public float RBHoldPositionDamper
	{
		get
		{
			return this._RBHoldPositionDamper;
		}
		set
		{
			if (this.RBHoldPositionDamperJSON != null)
			{
				this.RBHoldPositionDamperJSON.val = value;
			}
			else if (this._RBHoldPositionDamper != value)
			{
				this.SyncRBHoldPositionDamper(value);
			}
		}
	}

	// Token: 0x06005479 RID: 21625 RVA: 0x001E9905 File Offset: 0x001E7D05
	private void SyncRBHoldPositionMaxForce(float f)
	{
		this._RBHoldPositionMaxForce = f;
		this.SetJointSprings();
	}

	// Token: 0x17000C54 RID: 3156
	// (get) Token: 0x0600547A RID: 21626 RVA: 0x001E9914 File Offset: 0x001E7D14
	// (set) Token: 0x0600547B RID: 21627 RVA: 0x001E991C File Offset: 0x001E7D1C
	public float RBHoldPositionMaxForce
	{
		get
		{
			return this._RBHoldPositionMaxForce;
		}
		set
		{
			if (this.RBHoldPositionMaxForceJSON != null)
			{
				this.RBHoldPositionMaxForceJSON.val = value;
			}
			else if (this._RBHoldPositionMaxForce != value)
			{
				this.SyncRBHoldPositionMaxForce(value);
				this.SetJointSprings();
			}
		}
	}

	// Token: 0x0600547C RID: 21628 RVA: 0x001E9953 File Offset: 0x001E7D53
	private void SyncRBComplyPositionSpring(float f)
	{
		this._RBComplyPositionSpring = f;
		this.SetJointSprings();
	}

	// Token: 0x17000C55 RID: 3157
	// (get) Token: 0x0600547D RID: 21629 RVA: 0x001E9962 File Offset: 0x001E7D62
	// (set) Token: 0x0600547E RID: 21630 RVA: 0x001E996A File Offset: 0x001E7D6A
	public float RBComplyPositionSpring
	{
		get
		{
			return this._RBComplyPositionSpring;
		}
		set
		{
			if (this.RBComplyPositionSpringJSON != null)
			{
				this.RBComplyPositionSpringJSON.val = value;
			}
			else if (this._RBComplyPositionSpring != value)
			{
				this.SyncRBComplyPositionSpring(value);
			}
		}
	}

	// Token: 0x0600547F RID: 21631 RVA: 0x001E999B File Offset: 0x001E7D9B
	private void SyncRBComplyPositionDamper(float f)
	{
		this._RBComplyPositionDamper = f;
		this.SetJointSprings();
	}

	// Token: 0x17000C56 RID: 3158
	// (get) Token: 0x06005480 RID: 21632 RVA: 0x001E99AA File Offset: 0x001E7DAA
	// (set) Token: 0x06005481 RID: 21633 RVA: 0x001E99B2 File Offset: 0x001E7DB2
	public float RBComplyPositionDamper
	{
		get
		{
			return this._RBComplyPositionDamper;
		}
		set
		{
			if (this.RBComplyPositionDamperJSON != null)
			{
				this.RBComplyPositionDamperJSON.val = value;
			}
			else if (this._RBComplyPositionDamper != value)
			{
				this.SyncRBComplyPositionDamper(value);
			}
		}
	}

	// Token: 0x06005482 RID: 21634 RVA: 0x001E99E3 File Offset: 0x001E7DE3
	private void SyncRBLinkPositionSpring(float f)
	{
		this._RBLinkPositionSpring = f;
		this.SetLinkedJointSprings();
	}

	// Token: 0x17000C57 RID: 3159
	// (get) Token: 0x06005483 RID: 21635 RVA: 0x001E99F2 File Offset: 0x001E7DF2
	// (set) Token: 0x06005484 RID: 21636 RVA: 0x001E99FA File Offset: 0x001E7DFA
	public float RBLinkPositionSpring
	{
		get
		{
			return this._RBLinkPositionSpring;
		}
		set
		{
			if (this.RBLinkPositionSpringJSON != null)
			{
				this.RBLinkPositionSpringJSON.val = value;
			}
			else if (this._RBLinkPositionSpring != value)
			{
				this.SyncRBLinkPositionSpring(value);
			}
		}
	}

	// Token: 0x06005485 RID: 21637 RVA: 0x001E9A2B File Offset: 0x001E7E2B
	private void SyncRBLinkPositionDamper(float f)
	{
		this._RBLinkPositionDamper = f;
		this.SetLinkedJointSprings();
	}

	// Token: 0x17000C58 RID: 3160
	// (get) Token: 0x06005486 RID: 21638 RVA: 0x001E9A3A File Offset: 0x001E7E3A
	// (set) Token: 0x06005487 RID: 21639 RVA: 0x001E9A42 File Offset: 0x001E7E42
	public float RBLinkPositionDamper
	{
		get
		{
			return this._RBLinkPositionDamper;
		}
		set
		{
			if (this.RBLinkPositionDamperJSON != null)
			{
				this.RBLinkPositionDamperJSON.val = value;
			}
			else if (this._RBLinkPositionDamper != value)
			{
				this.SyncRBLinkPositionDamper(value);
			}
		}
	}

	// Token: 0x06005488 RID: 21640 RVA: 0x001E9A73 File Offset: 0x001E7E73
	private void SyncRBLinkPositionMaxForce(float f)
	{
		this._RBLinkPositionMaxForce = f;
		this.SetLinkedJointSprings();
	}

	// Token: 0x17000C59 RID: 3161
	// (get) Token: 0x06005489 RID: 21641 RVA: 0x001E9A82 File Offset: 0x001E7E82
	// (set) Token: 0x0600548A RID: 21642 RVA: 0x001E9A8A File Offset: 0x001E7E8A
	public float RBLinkPositionMaxForce
	{
		get
		{
			return this._RBLinkPositionMaxForce;
		}
		set
		{
			if (this.RBLinkPositionMaxForceJSON != null)
			{
				this.RBLinkPositionMaxForceJSON.val = value;
			}
			else if (this._RBLinkPositionMaxForce != value)
			{
				this.SyncRBLinkPositionMaxForce(value);
			}
		}
	}

	// Token: 0x17000C5A RID: 3162
	// (get) Token: 0x0600548B RID: 21643 RVA: 0x001E9ABB File Offset: 0x001E7EBB
	// (set) Token: 0x0600548C RID: 21644 RVA: 0x001E9AC3 File Offset: 0x001E7EC3
	public float RBLockRotationSpring
	{
		get
		{
			return this._RBLockRotationSpring;
		}
		set
		{
			if (this._RBLockRotationSpring != value)
			{
				this._RBLockRotationSpring = value;
				this.SetJointSprings();
			}
		}
	}

	// Token: 0x17000C5B RID: 3163
	// (get) Token: 0x0600548D RID: 21645 RVA: 0x001E9ADE File Offset: 0x001E7EDE
	// (set) Token: 0x0600548E RID: 21646 RVA: 0x001E9AE6 File Offset: 0x001E7EE6
	public float RBLockRotationDamper
	{
		get
		{
			return this._RBLockRotationDamper;
		}
		set
		{
			if (this._RBLockRotationDamper != value)
			{
				this._RBLockRotationDamper = value;
				this.SetJointSprings();
			}
		}
	}

	// Token: 0x17000C5C RID: 3164
	// (get) Token: 0x0600548F RID: 21647 RVA: 0x001E9B01 File Offset: 0x001E7F01
	// (set) Token: 0x06005490 RID: 21648 RVA: 0x001E9B09 File Offset: 0x001E7F09
	public float RBLockRotationMaxForce
	{
		get
		{
			return this._RBLockRotationMaxForce;
		}
		set
		{
			if (this._RBLockRotationMaxForce != value)
			{
				this._RBLockRotationMaxForce = value;
				this.SetJointSprings();
			}
		}
	}

	// Token: 0x06005491 RID: 21649 RVA: 0x001E9B24 File Offset: 0x001E7F24
	private void SyncRBHoldRotationSpring(float f)
	{
		this._RBHoldRotationSpring = f;
		this.SetJointSprings();
	}

	// Token: 0x06005492 RID: 21650 RVA: 0x001E9B33 File Offset: 0x001E7F33
	public void SetHoldRotationSpringMin()
	{
		if (this.RBHoldRotationSpringJSON != null)
		{
			this.RBHoldRotationSpringJSON.val = this.RBHoldRotationSpringJSON.min;
		}
	}

	// Token: 0x06005493 RID: 21651 RVA: 0x001E9B56 File Offset: 0x001E7F56
	public void SetHoldRotationSpringMax()
	{
		if (this.RBHoldRotationSpringJSON != null)
		{
			this.RBHoldRotationSpringJSON.val = this.RBHoldRotationSpringJSON.max;
		}
	}

	// Token: 0x06005494 RID: 21652 RVA: 0x001E9B79 File Offset: 0x001E7F79
	public void SetHoldRotationSpringPercent(float percent)
	{
		if (this.RBHoldRotationSpringJSON != null)
		{
			this.RBHoldRotationSpringJSON.val = (this.RBHoldRotationSpringJSON.max - this.RBHoldRotationSpringJSON.min) * percent + this.RBHoldRotationSpringJSON.min;
		}
	}

	// Token: 0x17000C5D RID: 3165
	// (get) Token: 0x06005495 RID: 21653 RVA: 0x001E9BB6 File Offset: 0x001E7FB6
	// (set) Token: 0x06005496 RID: 21654 RVA: 0x001E9BBE File Offset: 0x001E7FBE
	public float RBHoldRotationSpring
	{
		get
		{
			return this._RBHoldRotationSpring;
		}
		set
		{
			if (this.RBHoldRotationSpringJSON != null)
			{
				this.RBHoldRotationSpringJSON.val = value;
			}
			else if (this._RBHoldRotationSpring != value)
			{
				this.SyncRBHoldRotationSpring(value);
			}
		}
	}

	// Token: 0x06005497 RID: 21655 RVA: 0x001E9BEF File Offset: 0x001E7FEF
	private void SyncRBHoldRotationDamper(float f)
	{
		this._RBHoldRotationDamper = f;
		this.SetJointSprings();
	}

	// Token: 0x06005498 RID: 21656 RVA: 0x001E9BFE File Offset: 0x001E7FFE
	public void SetHoldRotationDamperMin()
	{
		if (this.RBHoldRotationDamperJSON != null)
		{
			this.RBHoldRotationDamperJSON.val = this.RBHoldRotationDamperJSON.min;
		}
	}

	// Token: 0x06005499 RID: 21657 RVA: 0x001E9C21 File Offset: 0x001E8021
	public void SetHoldRotationDamperMax()
	{
		if (this.RBHoldRotationDamperJSON != null)
		{
			this.RBHoldRotationDamperJSON.val = this.RBHoldRotationDamperJSON.max;
		}
	}

	// Token: 0x0600549A RID: 21658 RVA: 0x001E9C44 File Offset: 0x001E8044
	public void SetHoldRotationDamperPercent(float percent)
	{
		if (this.RBHoldRotationDamperJSON != null)
		{
			this.RBHoldRotationDamperJSON.val = (this.RBHoldRotationDamperJSON.max - this.RBHoldRotationDamperJSON.min) * percent + this.RBHoldRotationDamperJSON.min;
		}
	}

	// Token: 0x17000C5E RID: 3166
	// (get) Token: 0x0600549B RID: 21659 RVA: 0x001E9C81 File Offset: 0x001E8081
	// (set) Token: 0x0600549C RID: 21660 RVA: 0x001E9C89 File Offset: 0x001E8089
	public float RBHoldRotationDamper
	{
		get
		{
			return this._RBHoldRotationDamper;
		}
		set
		{
			if (this.RBHoldRotationDamperJSON != null)
			{
				this.RBHoldRotationDamperJSON.val = value;
			}
			else if (this._RBHoldRotationDamper != value)
			{
				this.SyncRBHoldRotationSpring(value);
			}
		}
	}

	// Token: 0x0600549D RID: 21661 RVA: 0x001E9CBA File Offset: 0x001E80BA
	private void SyncRBHoldRotationMaxForce(float f)
	{
		this._RBHoldRotationMaxForce = f;
		this.SetJointSprings();
	}

	// Token: 0x17000C5F RID: 3167
	// (get) Token: 0x0600549E RID: 21662 RVA: 0x001E9CC9 File Offset: 0x001E80C9
	// (set) Token: 0x0600549F RID: 21663 RVA: 0x001E9CD1 File Offset: 0x001E80D1
	public float RBHoldRotationMaxForce
	{
		get
		{
			return this._RBHoldRotationMaxForce;
		}
		set
		{
			if (this.RBHoldRotationMaxForceJSON != null)
			{
				this.RBHoldRotationMaxForceJSON.val = value;
			}
			else if (this._RBHoldRotationMaxForce != value)
			{
				this.SyncRBHoldRotationMaxForce(value);
			}
		}
	}

	// Token: 0x060054A0 RID: 21664 RVA: 0x001E9D02 File Offset: 0x001E8102
	private void SyncRBComplyRotationSpring(float f)
	{
		this._RBComplyRotationSpring = f;
		this.SetJointSprings();
	}

	// Token: 0x17000C60 RID: 3168
	// (get) Token: 0x060054A1 RID: 21665 RVA: 0x001E9D11 File Offset: 0x001E8111
	// (set) Token: 0x060054A2 RID: 21666 RVA: 0x001E9D19 File Offset: 0x001E8119
	public float RBComplyRotationSpring
	{
		get
		{
			return this._RBComplyRotationSpring;
		}
		set
		{
			if (this.RBComplyRotationSpringJSON != null)
			{
				this.RBComplyRotationSpringJSON.val = value;
			}
			else
			{
				this.SyncRBComplyRotationSpring(value);
			}
		}
	}

	// Token: 0x060054A3 RID: 21667 RVA: 0x001E9D3E File Offset: 0x001E813E
	private void SyncRBComplyRotationDamper(float f)
	{
		this._RBComplyRotationDamper = f;
		this.SetJointSprings();
	}

	// Token: 0x17000C61 RID: 3169
	// (get) Token: 0x060054A4 RID: 21668 RVA: 0x001E9D4D File Offset: 0x001E814D
	// (set) Token: 0x060054A5 RID: 21669 RVA: 0x001E9D55 File Offset: 0x001E8155
	public float RBComplyRotationDamper
	{
		get
		{
			return this._RBComplyRotationDamper;
		}
		set
		{
			if (this.RBComplyRotationDamperJSON != null)
			{
				this.RBComplyRotationDamperJSON.val = value;
			}
			else if (this._RBComplyRotationDamper != value)
			{
				this.SyncRBComplyRotationDamper(value);
			}
		}
	}

	// Token: 0x060054A6 RID: 21670 RVA: 0x001E9D86 File Offset: 0x001E8186
	private void SyncRBLinkRotationSpring(float f)
	{
		this._RBLinkRotationSpring = f;
		this.SetLinkedJointSprings();
	}

	// Token: 0x17000C62 RID: 3170
	// (get) Token: 0x060054A7 RID: 21671 RVA: 0x001E9D95 File Offset: 0x001E8195
	// (set) Token: 0x060054A8 RID: 21672 RVA: 0x001E9D9D File Offset: 0x001E819D
	public float RBLinkRotationSpring
	{
		get
		{
			return this._RBLinkRotationSpring;
		}
		set
		{
			if (this.RBLinkRotationSpringJSON != null)
			{
				this.RBLinkRotationSpringJSON.val = value;
			}
			else
			{
				this.SyncRBLinkRotationSpring(value);
			}
		}
	}

	// Token: 0x060054A9 RID: 21673 RVA: 0x001E9DC2 File Offset: 0x001E81C2
	private void SyncRBLinkRotationDamper(float f)
	{
		this._RBLinkRotationDamper = f;
		this.SetLinkedJointSprings();
	}

	// Token: 0x17000C63 RID: 3171
	// (get) Token: 0x060054AA RID: 21674 RVA: 0x001E9DD1 File Offset: 0x001E81D1
	// (set) Token: 0x060054AB RID: 21675 RVA: 0x001E9DD9 File Offset: 0x001E81D9
	public float RBLinkRotationDamper
	{
		get
		{
			return this._RBLinkRotationDamper;
		}
		set
		{
			if (this.RBLinkRotationDamperJSON != null)
			{
				this.RBLinkRotationDamperJSON.val = value;
			}
			else if (this._RBLinkRotationDamper != value)
			{
				this.SyncRBLinkRotationDamper(value);
			}
		}
	}

	// Token: 0x060054AC RID: 21676 RVA: 0x001E9E0A File Offset: 0x001E820A
	private void SyncRBLinkRotationMaxForce(float f)
	{
		this._RBLinkRotationMaxForce = f;
		this.SetLinkedJointSprings();
	}

	// Token: 0x17000C64 RID: 3172
	// (get) Token: 0x060054AD RID: 21677 RVA: 0x001E9E19 File Offset: 0x001E8219
	// (set) Token: 0x060054AE RID: 21678 RVA: 0x001E9E21 File Offset: 0x001E8221
	public float RBLinkRotationMaxForce
	{
		get
		{
			return this._RBLinkRotationMaxForce;
		}
		set
		{
			if (this.RBLinkRotationMaxForceJSON != null)
			{
				this.RBLinkRotationMaxForceJSON.val = value;
			}
			else if (this._RBLinkRotationMaxForce != value)
			{
				this.SyncRBLinkRotationMaxForce(value);
			}
		}
	}

	// Token: 0x060054AF RID: 21679 RVA: 0x001E9E52 File Offset: 0x001E8252
	private void SyncRBComplyJointRotationDriveSpring(float f)
	{
		this._RBComplyJointRotationDriveSpring = f;
		this.SetNaturalJointDrive();
	}

	// Token: 0x17000C65 RID: 3173
	// (get) Token: 0x060054B0 RID: 21680 RVA: 0x001E9E61 File Offset: 0x001E8261
	// (set) Token: 0x060054B1 RID: 21681 RVA: 0x001E9E69 File Offset: 0x001E8269
	public float RBComplyJointRotationDriveSpring
	{
		get
		{
			return this._RBComplyJointRotationDriveSpring;
		}
		set
		{
			if (this.RBComplyJointRotationDriveSpringJSON != null)
			{
				this.RBComplyJointRotationDriveSpringJSON.val = value;
			}
			else
			{
				this.SyncRBComplyJointRotationDriveSpring(value);
			}
		}
	}

	// Token: 0x060054B2 RID: 21682 RVA: 0x001E9E8E File Offset: 0x001E828E
	private void SyncJointRotationDriveSpring(float f)
	{
		this._jointRotationDriveSpring = f;
		this.SetNaturalJointDrive();
	}

	// Token: 0x17000C66 RID: 3174
	// (get) Token: 0x060054B3 RID: 21683 RVA: 0x001E9E9D File Offset: 0x001E829D
	// (set) Token: 0x060054B4 RID: 21684 RVA: 0x001E9EA5 File Offset: 0x001E82A5
	public float jointRotationDriveSpring
	{
		get
		{
			return this._jointRotationDriveSpring;
		}
		set
		{
			if (this.jointRotationDriveSpringJSON != null)
			{
				this.jointRotationDriveSpringJSON.val = value;
			}
			else if (this._jointRotationDriveSpring != value)
			{
				this.SyncJointRotationDriveSpring(value);
				this.SetNaturalJointDrive();
			}
		}
	}

	// Token: 0x060054B5 RID: 21685 RVA: 0x001E9EDC File Offset: 0x001E82DC
	private void SyncJointRotationDriveDamper(float f)
	{
		this._jointRotationDriveDamper = f;
		this.SetNaturalJointDrive();
	}

	// Token: 0x17000C67 RID: 3175
	// (get) Token: 0x060054B6 RID: 21686 RVA: 0x001E9EEB File Offset: 0x001E82EB
	// (set) Token: 0x060054B7 RID: 21687 RVA: 0x001E9EF3 File Offset: 0x001E82F3
	public float jointRotationDriveDamper
	{
		get
		{
			return this._jointRotationDriveDamper;
		}
		set
		{
			if (this.jointRotationDriveDamperJSON != null)
			{
				this.jointRotationDriveDamperJSON.val = value;
			}
			else if (this._jointRotationDriveDamper != value)
			{
				this.SyncJointRotationDriveDamper(value);
			}
		}
	}

	// Token: 0x060054B8 RID: 21688 RVA: 0x001E9F24 File Offset: 0x001E8324
	private void SyncJointRotationDriveMaxForce(float f)
	{
		this._jointRotationDriveMaxForce = f;
		this.SetNaturalJointDrive();
	}

	// Token: 0x17000C68 RID: 3176
	// (get) Token: 0x060054B9 RID: 21689 RVA: 0x001E9F33 File Offset: 0x001E8333
	// (set) Token: 0x060054BA RID: 21690 RVA: 0x001E9F3B File Offset: 0x001E833B
	public float jointRotationDriveMaxForce
	{
		get
		{
			return this._jointRotationDriveMaxForce;
		}
		set
		{
			if (this.jointRotationDriveMaxForceJSON != null)
			{
				this.jointRotationDriveMaxForceJSON.val = value;
			}
			else if (this._jointRotationDriveMaxForce != value)
			{
				this.SyncJointRotationDriveMaxForce(value);
			}
		}
	}

	// Token: 0x060054BB RID: 21691 RVA: 0x001E9F6C File Offset: 0x001E836C
	private void SyncJointRotationDriveXTarget(float f)
	{
		this._jointRotationDriveXTarget = f;
		this.SetNaturalJointDriveTarget();
	}

	// Token: 0x17000C69 RID: 3177
	// (get) Token: 0x060054BC RID: 21692 RVA: 0x001E9F7B File Offset: 0x001E837B
	// (set) Token: 0x060054BD RID: 21693 RVA: 0x001E9F83 File Offset: 0x001E8383
	public float jointRotationDriveXTarget
	{
		get
		{
			return this._jointRotationDriveXTarget;
		}
		set
		{
			if (this.jointRotationDriveXTargetJSON != null)
			{
				this.jointRotationDriveXTargetJSON.val = value;
			}
			else if (this._jointRotationDriveXTarget != value)
			{
				this.SyncJointRotationDriveXTarget(value);
			}
		}
	}

	// Token: 0x17000C6A RID: 3178
	// (get) Token: 0x060054BE RID: 21694 RVA: 0x001E9FB4 File Offset: 0x001E83B4
	// (set) Token: 0x060054BF RID: 21695 RVA: 0x001E9FBC File Offset: 0x001E83BC
	public float jointRotationDriveXTargetAdditional
	{
		get
		{
			return this._jointRotationDriveXTargetAdditional;
		}
		set
		{
			if (this._jointRotationDriveXTargetAdditional != value)
			{
				this._jointRotationDriveXTargetAdditional = value;
				this.SetNaturalJointDriveTarget();
			}
		}
	}

	// Token: 0x060054C0 RID: 21696 RVA: 0x001E9FD7 File Offset: 0x001E83D7
	private void SyncJointRotationDriveYTarget(float f)
	{
		this._jointRotationDriveYTarget = f;
		this.SetNaturalJointDriveTarget();
	}

	// Token: 0x17000C6B RID: 3179
	// (get) Token: 0x060054C1 RID: 21697 RVA: 0x001E9FE6 File Offset: 0x001E83E6
	// (set) Token: 0x060054C2 RID: 21698 RVA: 0x001E9FEE File Offset: 0x001E83EE
	public float jointRotationDriveYTarget
	{
		get
		{
			return this._jointRotationDriveYTarget;
		}
		set
		{
			if (this.jointRotationDriveYTargetJSON != null)
			{
				this.jointRotationDriveYTargetJSON.val = value;
			}
			else if (this._jointRotationDriveYTarget != value)
			{
				this.SyncJointRotationDriveYTarget(value);
			}
		}
	}

	// Token: 0x17000C6C RID: 3180
	// (get) Token: 0x060054C3 RID: 21699 RVA: 0x001EA01F File Offset: 0x001E841F
	// (set) Token: 0x060054C4 RID: 21700 RVA: 0x001EA027 File Offset: 0x001E8427
	public float jointRotationDriveYTargetAdditional
	{
		get
		{
			return this._jointRotationDriveYTargetAdditional;
		}
		set
		{
			if (this._jointRotationDriveYTargetAdditional != value)
			{
				this._jointRotationDriveYTargetAdditional = value;
				this.SetNaturalJointDriveTarget();
			}
		}
	}

	// Token: 0x060054C5 RID: 21701 RVA: 0x001EA042 File Offset: 0x001E8442
	private void SyncJointRotationDriveZTarget(float f)
	{
		this._jointRotationDriveZTarget = f;
		this.SetNaturalJointDriveTarget();
	}

	// Token: 0x17000C6D RID: 3181
	// (get) Token: 0x060054C6 RID: 21702 RVA: 0x001EA051 File Offset: 0x001E8451
	// (set) Token: 0x060054C7 RID: 21703 RVA: 0x001EA059 File Offset: 0x001E8459
	public float jointRotationDriveZTarget
	{
		get
		{
			return this._jointRotationDriveZTarget;
		}
		set
		{
			if (this.jointRotationDriveZTargetJSON != null)
			{
				this.jointRotationDriveZTargetJSON.val = value;
			}
			else if (this._jointRotationDriveZTarget != value)
			{
				this.SyncJointRotationDriveZTarget(value);
				this.SetNaturalJointDriveTarget();
			}
		}
	}

	// Token: 0x17000C6E RID: 3182
	// (get) Token: 0x060054C8 RID: 21704 RVA: 0x001EA090 File Offset: 0x001E8490
	// (set) Token: 0x060054C9 RID: 21705 RVA: 0x001EA098 File Offset: 0x001E8498
	public float jointRotationDriveZTargetAdditional
	{
		get
		{
			return this._jointRotationDriveZTargetAdditional;
		}
		set
		{
			if (this._jointRotationDriveZTargetAdditional != value)
			{
				this._jointRotationDriveZTargetAdditional = value;
				this.SetNaturalJointDriveTarget();
			}
		}
	}

	// Token: 0x060054CA RID: 21706 RVA: 0x001EA0B4 File Offset: 0x001E84B4
	private void SyncDetachControl(bool b)
	{
		this._detachControl = b;
		if (!this._detachControl && this.followWhenOff != null && this._followWhenOffRB.isKinematic)
		{
			Dictionary<FreeControllerV3, Transform> dictionary = new Dictionary<FreeControllerV3, Transform>();
			FreeControllerV3[] componentsInChildren = this.followWhenOff.GetComponentsInChildren<FreeControllerV3>(true);
			foreach (FreeControllerV3 freeControllerV in componentsInChildren)
			{
				if (freeControllerV.transform.parent != null)
				{
					dictionary.Add(freeControllerV, freeControllerV.transform.parent);
					freeControllerV.transform.parent = null;
				}
			}
			this.followWhenOff.position = this.control.position;
			this.followWhenOff.rotation = this.control.rotation;
			foreach (FreeControllerV3 freeControllerV2 in componentsInChildren)
			{
				Transform parent;
				if (dictionary.TryGetValue(freeControllerV2, out parent))
				{
					freeControllerV2.transform.parent = parent;
				}
			}
		}
		this.SyncGrabFreezePhysics();
	}

	// Token: 0x17000C6F RID: 3183
	// (get) Token: 0x060054CB RID: 21707 RVA: 0x001EA1CC File Offset: 0x001E85CC
	// (set) Token: 0x060054CC RID: 21708 RVA: 0x001EA1D4 File Offset: 0x001E85D4
	public bool hidden
	{
		get
		{
			return this._hidden;
		}
		set
		{
			this._hidden = value;
			if (this._hidden)
			{
				if (this.mrs != null)
				{
					foreach (MeshRenderer meshRenderer in this.mrs)
					{
						meshRenderer.enabled = false;
					}
				}
			}
			else if (this.mrs != null)
			{
				foreach (MeshRenderer meshRenderer2 in this.mrs)
				{
					meshRenderer2.enabled = true;
				}
			}
		}
	}

	// Token: 0x17000C70 RID: 3184
	// (get) Token: 0x060054CD RID: 21709 RVA: 0x001EA262 File Offset: 0x001E8662
	// (set) Token: 0x060054CE RID: 21710 RVA: 0x001EA26C File Offset: 0x001E866C
	public bool guihidden
	{
		get
		{
			return this._guihidden;
		}
		set
		{
			this._guihidden = value;
			if (this._guihidden)
			{
				if (!this.GUIalwaysVisibleWhenSelected || !this._selected)
				{
					this.HideGUI();
				}
			}
			else if (this._selected)
			{
				this.ShowGUI();
			}
		}
	}

	// Token: 0x17000C71 RID: 3185
	// (get) Token: 0x060054CF RID: 21711 RVA: 0x001EA2BD File Offset: 0x001E86BD
	// (set) Token: 0x060054D0 RID: 21712 RVA: 0x001EA2C5 File Offset: 0x001E86C5
	public bool highlighted
	{
		get
		{
			return this._highlighted;
		}
		set
		{
			this._highlighted = value;
			this.SetColor();
		}
	}

	// Token: 0x17000C72 RID: 3186
	// (get) Token: 0x060054D1 RID: 21713 RVA: 0x001EA2D4 File Offset: 0x001E86D4
	public Vector3 selectedPosition
	{
		get
		{
			return this._selectedPosition;
		}
	}

	// Token: 0x17000C73 RID: 3187
	// (get) Token: 0x060054D2 RID: 21714 RVA: 0x001EA2DC File Offset: 0x001E86DC
	// (set) Token: 0x060054D3 RID: 21715 RVA: 0x001EA2E4 File Offset: 0x001E86E4
	public bool selected
	{
		get
		{
			return this._selected;
		}
		set
		{
			this._selected = value;
			if (this._selected)
			{
				if (!this._guihidden || this.GUIalwaysVisibleWhenSelected)
				{
					this.ShowGUI();
				}
				this._selectedPosition = this.control.position;
			}
			else
			{
				this.HideGUI();
			}
			this.SetColor();
		}
	}

	// Token: 0x17000C74 RID: 3188
	// (get) Token: 0x060054D4 RID: 21716 RVA: 0x001EA341 File Offset: 0x001E8741
	public Color currentPositionColor
	{
		get
		{
			return this._currentPositionColor;
		}
	}

	// Token: 0x17000C75 RID: 3189
	// (get) Token: 0x060054D5 RID: 21717 RVA: 0x001EA349 File Offset: 0x001E8749
	public Color currentRotationColor
	{
		get
		{
			return this._currentRotationColor;
		}
	}

	// Token: 0x17000C76 RID: 3190
	// (get) Token: 0x060054D6 RID: 21718 RVA: 0x001EA351 File Offset: 0x001E8751
	// (set) Token: 0x060054D7 RID: 21719 RVA: 0x001EA35C File Offset: 0x001E875C
	public FreeControllerV3.ControlMode controlMode
	{
		get
		{
			return this._controlMode;
		}
		set
		{
			if (value == FreeControllerV3.ControlMode.Position)
			{
				if (this._moveEnabled || this._moveForceEnabled)
				{
					this._controlMode = value;
				}
			}
			else if (value == FreeControllerV3.ControlMode.Rotation)
			{
				if (this._rotationEnabled || this._rotationForceEnabled)
				{
					this._controlMode = value;
				}
			}
			else
			{
				this._controlMode = FreeControllerV3.ControlMode.Off;
			}
		}
	}

	// Token: 0x060054D8 RID: 21720 RVA: 0x001EA3C2 File Offset: 0x001E87C2
	public void NextControlMode()
	{
		if (this._controlMode == FreeControllerV3.ControlMode.Off)
		{
			this.controlMode = FreeControllerV3.ControlMode.Position;
		}
		else if (this._controlMode == FreeControllerV3.ControlMode.Position)
		{
			this.controlMode = FreeControllerV3.ControlMode.Rotation;
		}
		else
		{
			this.controlMode = FreeControllerV3.ControlMode.Position;
		}
	}

	// Token: 0x060054D9 RID: 21721 RVA: 0x001EA3FA File Offset: 0x001E87FA
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			this.Init();
			this.HideGUI();
		}
	}

	// Token: 0x060054DA RID: 21722 RVA: 0x001EA41C File Offset: 0x001E881C
	protected void SyncAtomUID()
	{
		if (this.containingAtom != null)
		{
			if (this.UIDText != null)
			{
				this.UIDText.text = this.containingAtom.uid + ":" + base.name;
			}
			if (this.UIDTextAlt != null)
			{
				this.UIDTextAlt.text = this.containingAtom.uid + ":" + base.name;
			}
		}
	}

	// Token: 0x060054DB RID: 21723 RVA: 0x001EA4A8 File Offset: 0x001E88A8
	protected void RegisterFCUI(FreeControllerV3UI fcui, bool isAlt)
	{
		FreeControllerV3.<RegisterFCUI>c__AnonStorey0 <RegisterFCUI>c__AnonStorey = new FreeControllerV3.<RegisterFCUI>c__AnonStorey0();
		<RegisterFCUI>c__AnonStorey.fcui = fcui;
		<RegisterFCUI>c__AnonStorey.$this = this;
		this.resetAction.RegisterButton(<RegisterFCUI>c__AnonStorey.fcui.resetButton, isAlt);
		if (this.currentPositionStateJSON != null)
		{
			this.currentPositionStateJSON.RegisterToggleGroupValue(<RegisterFCUI>c__AnonStorey.fcui.positionToggleGroup, isAlt);
		}
		if (this.currentRotationStateJSON != null)
		{
			this.currentRotationStateJSON.RegisterToggleGroupValue(<RegisterFCUI>c__AnonStorey.fcui.rotationToggleGroup, isAlt);
		}
		this.RBHoldPositionSpringJSON.RegisterSlider(<RegisterFCUI>c__AnonStorey.fcui.holdPositionSpringSlider, isAlt);
		this.RBHoldPositionDamperJSON.RegisterSlider(<RegisterFCUI>c__AnonStorey.fcui.holdPositionDamperSlider, isAlt);
		this.RBHoldPositionMaxForceJSON.RegisterSlider(<RegisterFCUI>c__AnonStorey.fcui.holdPositionMaxForceSlider, isAlt);
		this.RBHoldRotationSpringJSON.RegisterSlider(<RegisterFCUI>c__AnonStorey.fcui.holdRotationSpringSlider, isAlt);
		this.RBHoldRotationDamperJSON.RegisterSlider(<RegisterFCUI>c__AnonStorey.fcui.holdRotationDamperSlider, isAlt);
		this.RBHoldRotationMaxForceJSON.RegisterSlider(<RegisterFCUI>c__AnonStorey.fcui.holdRotationMaxForceSlider, isAlt);
		this.RBComplyPositionSpringJSON.RegisterSlider(<RegisterFCUI>c__AnonStorey.fcui.complyPositionSpringSlider, isAlt);
		this.RBComplyPositionDamperJSON.RegisterSlider(<RegisterFCUI>c__AnonStorey.fcui.complyPositionDamperSlider, isAlt);
		this.RBComplyRotationSpringJSON.RegisterSlider(<RegisterFCUI>c__AnonStorey.fcui.complyRotationSpringSlider, isAlt);
		this.RBComplyRotationDamperJSON.RegisterSlider(<RegisterFCUI>c__AnonStorey.fcui.complyRotationDamperSlider, isAlt);
		this.RBComplyJointRotationDriveSpringJSON.RegisterSlider(<RegisterFCUI>c__AnonStorey.fcui.complyJointRotationDriveSpringSlider, isAlt);
		if (this.complyPositionThresholdJSON != null)
		{
			this.complyPositionThresholdJSON.RegisterSlider(<RegisterFCUI>c__AnonStorey.fcui.complyPositionThresholdSlider, isAlt);
		}
		if (this.complyRotationThresholdJSON != null)
		{
			this.complyRotationThresholdJSON.RegisterSlider(<RegisterFCUI>c__AnonStorey.fcui.complyRotationThresholdSlider, isAlt);
		}
		if (this.complySpeedJSON != null)
		{
			this.complySpeedJSON.RegisterSlider(<RegisterFCUI>c__AnonStorey.fcui.complySpeedSlider, isAlt);
		}
		this.RBLinkPositionSpringJSON.RegisterSlider(<RegisterFCUI>c__AnonStorey.fcui.linkPositionSpringSlider, isAlt);
		this.RBLinkPositionDamperJSON.RegisterSlider(<RegisterFCUI>c__AnonStorey.fcui.linkPositionDamperSlider, isAlt);
		this.RBLinkPositionMaxForceJSON.RegisterSlider(<RegisterFCUI>c__AnonStorey.fcui.linkPositionMaxForceSlider, isAlt);
		this.RBLinkRotationSpringJSON.RegisterSlider(<RegisterFCUI>c__AnonStorey.fcui.linkRotationSpringSlider, isAlt);
		this.RBLinkRotationDamperJSON.RegisterSlider(<RegisterFCUI>c__AnonStorey.fcui.linkRotationDamperSlider, isAlt);
		this.RBLinkRotationMaxForceJSON.RegisterSlider(<RegisterFCUI>c__AnonStorey.fcui.linkRotationMaxForceSlider, isAlt);
		this.jointRotationDriveSpringJSON.RegisterSlider(<RegisterFCUI>c__AnonStorey.fcui.jointRotationDriveSpringSlider, isAlt);
		this.jointRotationDriveDamperJSON.RegisterSlider(<RegisterFCUI>c__AnonStorey.fcui.jointRotationDriveDamperSlider, isAlt);
		this.jointRotationDriveMaxForceJSON.RegisterSlider(<RegisterFCUI>c__AnonStorey.fcui.jointRotationDriveMaxForceSlider, isAlt);
		this.jointRotationDriveXTargetJSON.RegisterSlider(<RegisterFCUI>c__AnonStorey.fcui.jointRotationDriveXTargetSlider, isAlt);
		this.jointRotationDriveYTargetJSON.RegisterSlider(<RegisterFCUI>c__AnonStorey.fcui.jointRotationDriveYTargetSlider, isAlt);
		this.jointRotationDriveZTargetJSON.RegisterSlider(<RegisterFCUI>c__AnonStorey.fcui.jointRotationDriveZTargetSlider, isAlt);
		if (this.onJSON != null)
		{
			this.onJSON.RegisterToggle(<RegisterFCUI>c__AnonStorey.fcui.onToggle, isAlt);
		}
		if (this.detachControlJSON != null)
		{
			this.detachControlJSON.RegisterToggle(<RegisterFCUI>c__AnonStorey.fcui.detachControlToggle, isAlt);
		}
		this.RBMassJSON.RegisterSlider(<RegisterFCUI>c__AnonStorey.fcui.massSlider, isAlt);
		this.RBDragJSON.RegisterSlider(<RegisterFCUI>c__AnonStorey.fcui.dragSlider, isAlt);
		this.RBMaxVelocityEnableJSON.RegisterToggle(<RegisterFCUI>c__AnonStorey.fcui.maxVelocityEnableToggle, isAlt);
		this.RBMaxVelocityJSON.RegisterSlider(<RegisterFCUI>c__AnonStorey.fcui.maxVelocitySlider, isAlt);
		this.RBAngularDragJSON.RegisterSlider(<RegisterFCUI>c__AnonStorey.fcui.angularDragSlider, isAlt);
		this.physicsEnabledJSON.RegisterToggle(<RegisterFCUI>c__AnonStorey.fcui.physicsEnabledToggle, isAlt);
		if (this.collisionEnabledJSON != null)
		{
			this.collisionEnabledJSON.RegisterToggle(<RegisterFCUI>c__AnonStorey.fcui.collisionEnabledToggle, isAlt);
		}
		this.useGravityJSON.RegisterToggle(<RegisterFCUI>c__AnonStorey.fcui.useGravityWhenOffToggle, isAlt);
		this.interactableInPlayModeJSON.RegisterToggle(<RegisterFCUI>c__AnonStorey.fcui.interactableInPlayModeToggle, isAlt);
		this.deactivateOtherControlsOnPossessJSON.RegisterToggle(<RegisterFCUI>c__AnonStorey.fcui.deactivateOtherControlsOnPossessToggle, isAlt);
		if (<RegisterFCUI>c__AnonStorey.fcui.deactivateOtherControlsListText != null)
		{
			string text = string.Empty;
			if (this.onPossessDeactiveList != null)
			{
				foreach (FreeControllerV3 freeControllerV in this.onPossessDeactiveList)
				{
					text = text + freeControllerV.name + " ";
				}
			}
			<RegisterFCUI>c__AnonStorey.fcui.deactivateOtherControlsListText.text = text;
		}
		this.possessableJSON.RegisterToggle(<RegisterFCUI>c__AnonStorey.fcui.possessableToggle, isAlt);
		this.canGrabPositionJSON.RegisterToggle(<RegisterFCUI>c__AnonStorey.fcui.canGrabPositionToggle, isAlt);
		this.canGrabRotationJSON.RegisterToggle(<RegisterFCUI>c__AnonStorey.fcui.canGrabRotationToggle, isAlt);
		this.freezeAtomPhysicsWhenGrabbedJSON.RegisterToggle(<RegisterFCUI>c__AnonStorey.fcui.freezeAtomPhysicsWhenGrabbedToggle, isAlt);
		this.positionGridModeJSON.RegisterPopup(<RegisterFCUI>c__AnonStorey.fcui.positionGridModePopup, isAlt);
		this.rotationGridModeJSON.RegisterPopup(<RegisterFCUI>c__AnonStorey.fcui.rotationGridModePopup, isAlt);
		this.positionGridJSON.RegisterSlider(<RegisterFCUI>c__AnonStorey.fcui.positionGridSlider, isAlt);
		this.rotationGridJSON.RegisterSlider(<RegisterFCUI>c__AnonStorey.fcui.rotationGridSlider, isAlt);
		this.xLockJSON.RegisterToggle(<RegisterFCUI>c__AnonStorey.fcui.xPositionLockToggle, isAlt);
		this.yLockJSON.RegisterToggle(<RegisterFCUI>c__AnonStorey.fcui.yPositionLockToggle, isAlt);
		this.zLockJSON.RegisterToggle(<RegisterFCUI>c__AnonStorey.fcui.zPositionLockToggle, isAlt);
		this.xLocalLockJSON.RegisterToggle(<RegisterFCUI>c__AnonStorey.fcui.xPositionLocalLockToggle, isAlt);
		this.yLocalLockJSON.RegisterToggle(<RegisterFCUI>c__AnonStorey.fcui.yPositionLocalLockToggle, isAlt);
		this.zLocalLockJSON.RegisterToggle(<RegisterFCUI>c__AnonStorey.fcui.zPositionLocalLockToggle, isAlt);
		this.xRotLockJSON.RegisterToggle(<RegisterFCUI>c__AnonStorey.fcui.xRotationLockToggle, isAlt);
		this.yRotLockJSON.RegisterToggle(<RegisterFCUI>c__AnonStorey.fcui.yRotationLockToggle, isAlt);
		this.zRotLockJSON.RegisterToggle(<RegisterFCUI>c__AnonStorey.fcui.zRotationLockToggle, isAlt);
		if (<RegisterFCUI>c__AnonStorey.fcui.linkToAtomSelectionPopup != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.linkToAtomSelectionPopup.numPopupValues = 1;
			<RegisterFCUI>c__AnonStorey.fcui.linkToAtomSelectionPopup.setPopupValue(0, "None");
			if (this.linkToRB != null)
			{
				this.GetLinkToAtomUIDFromLinkToRB(this.linkToRB);
				this.SetLinkToAtom(this.linkToAtomUID);
				<RegisterFCUI>c__AnonStorey.fcui.linkToAtomSelectionPopup.currentValue = this.linkToAtomUID;
			}
			else
			{
				<RegisterFCUI>c__AnonStorey.fcui.linkToAtomSelectionPopup.currentValue = "None";
			}
			UIPopup uipopup = <RegisterFCUI>c__AnonStorey.fcui.linkToAtomSelectionPopup;
			uipopup.onOpenPopupHandlers = (UIPopup.OnOpenPopup)Delegate.Combine(uipopup.onOpenPopupHandlers, new UIPopup.OnOpenPopup(this.SetLinkToAtomNames));
			UIPopup uipopup2 = <RegisterFCUI>c__AnonStorey.fcui.linkToAtomSelectionPopup;
			uipopup2.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup2.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetLinkToAtom));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.linkToSelectionPopup != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.linkToSelectionPopup.numPopupValues = 1;
			<RegisterFCUI>c__AnonStorey.fcui.linkToSelectionPopup.setPopupValue(0, "None");
			if (this.linkToRB != null)
			{
				<RegisterFCUI>c__AnonStorey.fcui.linkToSelectionPopup.currentValue = this.linkToRB.name;
			}
			else
			{
				this.onLinkToRigidbodyNamesChanged(null);
				<RegisterFCUI>c__AnonStorey.fcui.linkToSelectionPopup.currentValue = "None";
			}
			UIPopup uipopup3 = <RegisterFCUI>c__AnonStorey.fcui.linkToSelectionPopup;
			uipopup3.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup3.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetLinkToRigidbodyObject));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.selectLinkToFromSceneButton != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.selectLinkToFromSceneButton.onClick.AddListener(new UnityAction(this.SelectLinkToRigidbodyFromScene));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.selectAlignToFromSceneButton != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.selectAlignToFromSceneButton.onClick.AddListener(new UnityAction(this.SelectAlignToRigidbodyFromScene));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xPositionMinus1Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.xPositionMinus1Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__0));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xPositionMinusPoint1Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.xPositionMinusPoint1Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__1));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xPositionMinusPoint01Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.xPositionMinusPoint01Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__2));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xPositionPlusPoint01Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.xPositionPlusPoint01Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__3));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xPositionPlusPoint1Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.xPositionPlusPoint1Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__4));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xPositionPlus1Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.xPositionPlus1Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__5));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.yPositionMinus1Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.yPositionMinus1Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__6));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.yPositionMinusPoint1Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.yPositionMinusPoint1Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__7));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.yPositionMinusPoint01Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.yPositionMinusPoint01Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__8));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.yPositionPlusPoint01Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.yPositionPlusPoint01Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__9));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.yPositionPlusPoint1Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.yPositionPlusPoint1Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__A));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.yPositionPlus1Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.yPositionPlus1Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__B));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zPositionMinus1Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zPositionMinus1Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__C));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zPositionMinusPoint1Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zPositionMinusPoint1Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__D));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zPositionMinusPoint01Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zPositionMinusPoint01Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__E));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zPositionPlusPoint01Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zPositionPlusPoint01Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__F));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zPositionPlusPoint1Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zPositionPlusPoint1Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__10));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zPositionPlus1Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zPositionPlus1Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__11));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xRotationMinus45Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.xRotationMinus45Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__12));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xRotationMinus5Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.xRotationMinus5Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__13));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xRotationMinusPoint5Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.xRotationMinusPoint5Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__14));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xRotationPlusPoint5Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.xRotationPlusPoint5Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__15));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xRotationPlus5Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.xRotationPlus5Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__16));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xRotationPlus45Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.xRotationPlus45Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__17));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.yRotationMinus45Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.yRotationMinus45Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__18));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.yRotationMinus5Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.yRotationMinus5Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__19));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.yRotationMinusPoint5Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.yRotationMinusPoint5Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__1A));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.yRotationPlusPoint5Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.yRotationPlusPoint5Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__1B));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.yRotationPlus5Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.yRotationPlus5Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__1C));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.yRotationPlus45Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.yRotationPlus45Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__1D));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zRotationMinus45Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zRotationMinus45Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__1E));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zRotationMinus5Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zRotationMinus5Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__1F));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zRotationMinusPoint5Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zRotationMinusPoint5Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__20));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zRotationPlusPoint5Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zRotationPlusPoint5Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__21));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zRotationPlus5Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zRotationPlus5Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__22));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zRotationPlus45Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zRotationPlus45Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__23));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xPosition0Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.xPosition0Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__24));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.yPosition0Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.yPosition0Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__25));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zPosition0Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zPosition0Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__26));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xRotation0Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.xRotation0Button.onClick.AddListener(new UnityAction(this.XRotation0));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.yRotation0Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.yRotation0Button.onClick.AddListener(new UnityAction(this.YRotation0));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zRotation0Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zRotation0Button.onClick.AddListener(new UnityAction(this.ZRotation0));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xPositionSnapPoint1Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.xPositionSnapPoint1Button.onClick.AddListener(new UnityAction(this.XPositionSnapPoint1));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.yPositionSnapPoint1Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.yPositionSnapPoint1Button.onClick.AddListener(new UnityAction(this.YPositionSnapPoint1));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zPositionSnapPoint1Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zPositionSnapPoint1Button.onClick.AddListener(new UnityAction(this.ZPositionSnapPoint1));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xRotationSnap1Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.xRotationSnap1Button.onClick.AddListener(new UnityAction(this.XRotationSnap1));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.yRotationSnap1Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.yRotationSnap1Button.onClick.AddListener(new UnityAction(this.YRotationSnap1));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zRotationSnap1Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zRotationSnap1Button.onClick.AddListener(new UnityAction(this.ZRotationSnap1));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xSelfRelativePositionAdjustInputField != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.xSelfRelativePositionAdjustInputField.onEndEdit.AddListener(new UnityAction<string>(<RegisterFCUI>c__AnonStorey.<>m__27));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xSelfRelativePositionAdjustInputFieldAction != null)
		{
			InputFieldAction xSelfRelativePositionAdjustInputFieldAction = <RegisterFCUI>c__AnonStorey.fcui.xSelfRelativePositionAdjustInputFieldAction;
			xSelfRelativePositionAdjustInputFieldAction.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Combine(xSelfRelativePositionAdjustInputFieldAction.onSubmitHandlers, new InputFieldAction.OnSubmit(<RegisterFCUI>c__AnonStorey.<>m__28));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xSelfRelativePositionMinus1Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.xSelfRelativePositionMinus1Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__29));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xSelfRelativePositionMinusPoint1Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.xSelfRelativePositionMinusPoint1Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__2A));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xSelfRelativePositionMinusPoint01Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.xSelfRelativePositionMinusPoint01Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__2B));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xSelfRelativePositionPlusPoint01Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.xSelfRelativePositionPlusPoint01Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__2C));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xSelfRelativePositionPlusPoint1Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.xSelfRelativePositionPlusPoint1Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__2D));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xSelfRelativePositionPlus1Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.xSelfRelativePositionPlus1Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__2E));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.ySelfRelativePositionAdjustInputField != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.ySelfRelativePositionAdjustInputField.onEndEdit.AddListener(new UnityAction<string>(<RegisterFCUI>c__AnonStorey.<>m__2F));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.ySelfRelativePositionAdjustInputFieldAction != null)
		{
			InputFieldAction ySelfRelativePositionAdjustInputFieldAction = <RegisterFCUI>c__AnonStorey.fcui.ySelfRelativePositionAdjustInputFieldAction;
			ySelfRelativePositionAdjustInputFieldAction.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Combine(ySelfRelativePositionAdjustInputFieldAction.onSubmitHandlers, new InputFieldAction.OnSubmit(<RegisterFCUI>c__AnonStorey.<>m__30));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.ySelfRelativePositionMinus1Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.ySelfRelativePositionMinus1Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__31));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.ySelfRelativePositionMinusPoint1Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.ySelfRelativePositionMinusPoint1Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__32));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.ySelfRelativePositionMinusPoint01Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.ySelfRelativePositionMinusPoint01Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__33));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.ySelfRelativePositionPlusPoint01Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.ySelfRelativePositionPlusPoint01Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__34));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.ySelfRelativePositionPlusPoint1Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.ySelfRelativePositionPlusPoint1Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__35));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.ySelfRelativePositionPlus1Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.ySelfRelativePositionPlus1Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__36));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zSelfRelativePositionAdjustInputField != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zSelfRelativePositionAdjustInputField.onEndEdit.AddListener(new UnityAction<string>(<RegisterFCUI>c__AnonStorey.<>m__37));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zSelfRelativePositionAdjustInputFieldAction != null)
		{
			InputFieldAction zSelfRelativePositionAdjustInputFieldAction = <RegisterFCUI>c__AnonStorey.fcui.zSelfRelativePositionAdjustInputFieldAction;
			zSelfRelativePositionAdjustInputFieldAction.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Combine(zSelfRelativePositionAdjustInputFieldAction.onSubmitHandlers, new InputFieldAction.OnSubmit(<RegisterFCUI>c__AnonStorey.<>m__38));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zSelfRelativePositionMinus1Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zSelfRelativePositionMinus1Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__39));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zSelfRelativePositionMinusPoint1Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zSelfRelativePositionMinusPoint1Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__3A));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zSelfRelativePositionMinusPoint01Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zSelfRelativePositionMinusPoint01Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__3B));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zSelfRelativePositionPlusPoint01Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zSelfRelativePositionPlusPoint01Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__3C));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zSelfRelativePositionPlusPoint1Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zSelfRelativePositionPlusPoint1Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__3D));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zSelfRelativePositionPlus1Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zSelfRelativePositionPlus1Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__3E));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xSelfRelativeRotationAdjustInputField != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.xSelfRelativeRotationAdjustInputField.onEndEdit.AddListener(new UnityAction<string>(<RegisterFCUI>c__AnonStorey.<>m__3F));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xSelfRelativePositionAdjustInputFieldAction != null)
		{
			InputFieldAction xSelfRelativePositionAdjustInputFieldAction2 = <RegisterFCUI>c__AnonStorey.fcui.xSelfRelativePositionAdjustInputFieldAction;
			xSelfRelativePositionAdjustInputFieldAction2.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Combine(xSelfRelativePositionAdjustInputFieldAction2.onSubmitHandlers, new InputFieldAction.OnSubmit(<RegisterFCUI>c__AnonStorey.<>m__40));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xSelfRelativeRotationMinus45Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.xSelfRelativeRotationMinus45Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__41));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xSelfRelativeRotationMinus5Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.xSelfRelativeRotationMinus5Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__42));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xSelfRelativeRotationMinusPoint5Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.xSelfRelativeRotationMinusPoint5Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__43));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xSelfRelativeRotationPlusPoint5Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.xSelfRelativeRotationPlusPoint5Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__44));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xSelfRelativeRotationPlus5Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.xSelfRelativeRotationPlus5Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__45));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xSelfRelativeRotationPlus45Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.xSelfRelativeRotationPlus45Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__46));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.ySelfRelativeRotationAdjustInputField != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.ySelfRelativeRotationAdjustInputField.onEndEdit.AddListener(new UnityAction<string>(<RegisterFCUI>c__AnonStorey.<>m__47));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.ySelfRelativePositionAdjustInputFieldAction != null)
		{
			InputFieldAction ySelfRelativePositionAdjustInputFieldAction2 = <RegisterFCUI>c__AnonStorey.fcui.ySelfRelativePositionAdjustInputFieldAction;
			ySelfRelativePositionAdjustInputFieldAction2.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Combine(ySelfRelativePositionAdjustInputFieldAction2.onSubmitHandlers, new InputFieldAction.OnSubmit(<RegisterFCUI>c__AnonStorey.<>m__48));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.ySelfRelativeRotationMinus45Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.ySelfRelativeRotationMinus45Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__49));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.ySelfRelativeRotationMinus5Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.ySelfRelativeRotationMinus5Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__4A));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.ySelfRelativeRotationMinusPoint5Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.ySelfRelativeRotationMinusPoint5Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__4B));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.ySelfRelativeRotationPlusPoint5Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.ySelfRelativeRotationPlusPoint5Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__4C));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.ySelfRelativeRotationPlus5Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.ySelfRelativeRotationPlus5Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__4D));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.ySelfRelativeRotationPlus45Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.ySelfRelativeRotationPlus45Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__4E));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zSelfRelativeRotationAdjustInputField != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zSelfRelativeRotationAdjustInputField.onEndEdit.AddListener(new UnityAction<string>(<RegisterFCUI>c__AnonStorey.<>m__4F));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zSelfRelativePositionAdjustInputFieldAction != null)
		{
			InputFieldAction zSelfRelativePositionAdjustInputFieldAction2 = <RegisterFCUI>c__AnonStorey.fcui.zSelfRelativePositionAdjustInputFieldAction;
			zSelfRelativePositionAdjustInputFieldAction2.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Combine(zSelfRelativePositionAdjustInputFieldAction2.onSubmitHandlers, new InputFieldAction.OnSubmit(<RegisterFCUI>c__AnonStorey.<>m__50));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zSelfRelativeRotationMinus45Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zSelfRelativeRotationMinus45Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__51));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zSelfRelativeRotationMinus5Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zSelfRelativeRotationMinus5Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__52));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zSelfRelativeRotationMinusPoint5Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zSelfRelativeRotationMinusPoint5Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__53));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zSelfRelativeRotationPlusPoint5Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zSelfRelativeRotationPlusPoint5Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__54));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zSelfRelativeRotationPlus5Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zSelfRelativeRotationPlus5Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__55));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zSelfRelativeRotationPlus45Button != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zSelfRelativeRotationPlus45Button.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__56));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.selectRootButton != null)
		{
			if (this.enableSelectRoot)
			{
				<RegisterFCUI>c__AnonStorey.fcui.selectRootButton.gameObject.SetActive(true);
				<RegisterFCUI>c__AnonStorey.fcui.selectRootButton.onClick.AddListener(new UnityAction(this.SelectRoot));
			}
			else
			{
				<RegisterFCUI>c__AnonStorey.fcui.selectRootButton.gameObject.SetActive(false);
			}
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xPositionInputField != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.xPositionInputField.text = string.Empty;
			<RegisterFCUI>c__AnonStorey.fcui.xPositionInputField.onEndEdit.AddListener(new UnityAction<string>(this.SetXPositionNoForce));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xPositionInputFieldAction != null)
		{
			InputFieldAction xPositionInputFieldAction = <RegisterFCUI>c__AnonStorey.fcui.xPositionInputFieldAction;
			xPositionInputFieldAction.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Combine(xPositionInputFieldAction.onSubmitHandlers, new InputFieldAction.OnSubmit(this.SetXPositionToInputField));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.yPositionInputField != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.yPositionInputField.text = string.Empty;
			<RegisterFCUI>c__AnonStorey.fcui.yPositionInputField.onEndEdit.AddListener(new UnityAction<string>(this.SetYPositionNoForce));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.yPositionInputFieldAction != null)
		{
			InputFieldAction yPositionInputFieldAction = <RegisterFCUI>c__AnonStorey.fcui.yPositionInputFieldAction;
			yPositionInputFieldAction.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Combine(yPositionInputFieldAction.onSubmitHandlers, new InputFieldAction.OnSubmit(this.SetYPositionToInputField));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zPositionInputField != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zPositionInputField.text = string.Empty;
			<RegisterFCUI>c__AnonStorey.fcui.zPositionInputField.onEndEdit.AddListener(new UnityAction<string>(this.SetZPositionNoForce));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zPositionInputFieldAction != null)
		{
			InputFieldAction zPositionInputFieldAction = <RegisterFCUI>c__AnonStorey.fcui.zPositionInputFieldAction;
			zPositionInputFieldAction.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Combine(zPositionInputFieldAction.onSubmitHandlers, new InputFieldAction.OnSubmit(this.SetZPositionToInputField));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xRotationInputField != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.xRotationInputField.text = string.Empty;
			<RegisterFCUI>c__AnonStorey.fcui.xRotationInputField.onEndEdit.AddListener(new UnityAction<string>(this.SetXRotationNoForce));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xRotationInputFieldAction != null)
		{
			InputFieldAction xRotationInputFieldAction = <RegisterFCUI>c__AnonStorey.fcui.xRotationInputFieldAction;
			xRotationInputFieldAction.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Combine(xRotationInputFieldAction.onSubmitHandlers, new InputFieldAction.OnSubmit(this.SetXRotationToInputField));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.yRotationInputField != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.yRotationInputField.text = string.Empty;
			<RegisterFCUI>c__AnonStorey.fcui.yRotationInputField.onEndEdit.AddListener(new UnityAction<string>(this.SetYRotationNoForce));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.yRotationInputFieldAction != null)
		{
			InputFieldAction yRotationInputFieldAction = <RegisterFCUI>c__AnonStorey.fcui.yRotationInputFieldAction;
			yRotationInputFieldAction.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Combine(yRotationInputFieldAction.onSubmitHandlers, new InputFieldAction.OnSubmit(this.SetYRotationToInputField));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zRotationInputField != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zRotationInputField.text = string.Empty;
			<RegisterFCUI>c__AnonStorey.fcui.zRotationInputField.onEndEdit.AddListener(new UnityAction<string>(this.SetZRotationNoForce));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zRotationInputFieldAction != null)
		{
			InputFieldAction zRotationInputFieldAction = <RegisterFCUI>c__AnonStorey.fcui.zRotationInputFieldAction;
			zRotationInputFieldAction.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Combine(zRotationInputFieldAction.onSubmitHandlers, new InputFieldAction.OnSubmit(this.SetZRotationToInputField));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xLocalPositionInputField != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.xLocalPositionInputField.text = string.Empty;
			<RegisterFCUI>c__AnonStorey.fcui.xLocalPositionInputField.onEndEdit.AddListener(new UnityAction<string>(this.SetXLocalPositionNoForce));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xLocalPositionInputFieldAction != null)
		{
			InputFieldAction xLocalPositionInputFieldAction = <RegisterFCUI>c__AnonStorey.fcui.xLocalPositionInputFieldAction;
			xLocalPositionInputFieldAction.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Combine(xLocalPositionInputFieldAction.onSubmitHandlers, new InputFieldAction.OnSubmit(this.SetXLocalPositionToInputField));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.yLocalPositionInputField != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.yLocalPositionInputField.text = string.Empty;
			<RegisterFCUI>c__AnonStorey.fcui.yLocalPositionInputField.onEndEdit.AddListener(new UnityAction<string>(this.SetYLocalPositionNoForce));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.yLocalPositionInputFieldAction != null)
		{
			InputFieldAction yLocalPositionInputFieldAction = <RegisterFCUI>c__AnonStorey.fcui.yLocalPositionInputFieldAction;
			yLocalPositionInputFieldAction.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Combine(yLocalPositionInputFieldAction.onSubmitHandlers, new InputFieldAction.OnSubmit(this.SetYLocalPositionToInputField));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zLocalPositionInputField != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zLocalPositionInputField.text = string.Empty;
			<RegisterFCUI>c__AnonStorey.fcui.zLocalPositionInputField.onEndEdit.AddListener(new UnityAction<string>(this.SetZLocalPositionNoForce));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zLocalPositionInputFieldAction != null)
		{
			InputFieldAction zLocalPositionInputFieldAction = <RegisterFCUI>c__AnonStorey.fcui.zLocalPositionInputFieldAction;
			zLocalPositionInputFieldAction.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Combine(zLocalPositionInputFieldAction.onSubmitHandlers, new InputFieldAction.OnSubmit(this.SetZLocalPositionToInputField));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xLocalRotationInputField != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.xLocalRotationInputField.text = string.Empty;
			<RegisterFCUI>c__AnonStorey.fcui.xLocalRotationInputField.onEndEdit.AddListener(new UnityAction<string>(this.SetXLocalRotationNoForce));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.xLocalRotationInputFieldAction != null)
		{
			InputFieldAction xLocalRotationInputFieldAction = <RegisterFCUI>c__AnonStorey.fcui.xLocalRotationInputFieldAction;
			xLocalRotationInputFieldAction.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Combine(xLocalRotationInputFieldAction.onSubmitHandlers, new InputFieldAction.OnSubmit(this.SetXLocalRotationToInputField));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.yLocalRotationInputField != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.yLocalRotationInputField.text = string.Empty;
			<RegisterFCUI>c__AnonStorey.fcui.yLocalRotationInputField.onEndEdit.AddListener(new UnityAction<string>(this.SetYLocalRotationNoForce));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.yLocalRotationInputFieldAction != null)
		{
			InputFieldAction yLocalRotationInputFieldAction = <RegisterFCUI>c__AnonStorey.fcui.yLocalRotationInputFieldAction;
			yLocalRotationInputFieldAction.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Combine(yLocalRotationInputFieldAction.onSubmitHandlers, new InputFieldAction.OnSubmit(this.SetYLocalRotationToInputField));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zLocalRotationInputField != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zLocalRotationInputField.text = string.Empty;
			<RegisterFCUI>c__AnonStorey.fcui.zLocalRotationInputField.onEndEdit.AddListener(new UnityAction<string>(this.SetZLocalRotationNoForce));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zLocalRotationInputFieldAction != null)
		{
			InputFieldAction zLocalRotationInputFieldAction = <RegisterFCUI>c__AnonStorey.fcui.zLocalRotationInputFieldAction;
			zLocalRotationInputFieldAction.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Combine(zLocalRotationInputFieldAction.onSubmitHandlers, new InputFieldAction.OnSubmit(this.SetZLocalRotationToInputField));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zeroXLocalPositionButton != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zeroXLocalPositionButton.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__57));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zeroYLocalPositionButton != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zeroYLocalPositionButton.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__58));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zeroZLocalPositionButton != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zeroZLocalPositionButton.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__59));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zeroXLocalRotationButton != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zeroXLocalRotationButton.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__5A));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zeroYLocalRotationButton != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zeroYLocalRotationButton.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__5B));
		}
		if (<RegisterFCUI>c__AnonStorey.fcui.zeroZLocalRotationButton != null)
		{
			<RegisterFCUI>c__AnonStorey.fcui.zeroZLocalRotationButton.onClick.AddListener(new UnityAction(<RegisterFCUI>c__AnonStorey.<>m__5C));
		}
		if (isAlt)
		{
			this.UIDTextAlt = <RegisterFCUI>c__AnonStorey.fcui.UIDText;
			if (this.UIDTextAlt != null)
			{
				if (this.containingAtom != null)
				{
					this.UIDTextAlt.text = this.containingAtom.uid + ":" + base.name;
				}
				else
				{
					this.UIDTextAlt.text = base.name;
				}
			}
		}
		else
		{
			this.UIDText = <RegisterFCUI>c__AnonStorey.fcui.UIDText;
			if (this.UIDText != null)
			{
				if (this.containingAtom != null)
				{
					this.UIDText.text = this.containingAtom.uid + ":" + base.name;
				}
				else
				{
					this.UIDText.text = base.name;
				}
			}
			this.xPositionInputField = <RegisterFCUI>c__AnonStorey.fcui.xPositionInputField;
			this.yPositionInputField = <RegisterFCUI>c__AnonStorey.fcui.yPositionInputField;
			this.zPositionInputField = <RegisterFCUI>c__AnonStorey.fcui.zPositionInputField;
			this.xRotationInputField = <RegisterFCUI>c__AnonStorey.fcui.xRotationInputField;
			this.yRotationInputField = <RegisterFCUI>c__AnonStorey.fcui.yRotationInputField;
			this.zRotationInputField = <RegisterFCUI>c__AnonStorey.fcui.zRotationInputField;
			this.xLocalPositionInputField = <RegisterFCUI>c__AnonStorey.fcui.xLocalPositionInputField;
			this.yLocalPositionInputField = <RegisterFCUI>c__AnonStorey.fcui.yLocalPositionInputField;
			this.zLocalPositionInputField = <RegisterFCUI>c__AnonStorey.fcui.zLocalPositionInputField;
			this.xLocalRotationInputField = <RegisterFCUI>c__AnonStorey.fcui.xLocalRotationInputField;
			this.yLocalRotationInputField = <RegisterFCUI>c__AnonStorey.fcui.yLocalRotationInputField;
			this.zLocalRotationInputField = <RegisterFCUI>c__AnonStorey.fcui.zLocalRotationInputField;
			this.xPositionText = <RegisterFCUI>c__AnonStorey.fcui.xPositionText;
			this.yPositionText = <RegisterFCUI>c__AnonStorey.fcui.yPositionText;
			this.zPositionText = <RegisterFCUI>c__AnonStorey.fcui.zPositionText;
			this.xLocalPositionText = <RegisterFCUI>c__AnonStorey.fcui.xLocalPositionText;
			this.yLocalPositionText = <RegisterFCUI>c__AnonStorey.fcui.yLocalPositionText;
			this.zLocalPositionText = <RegisterFCUI>c__AnonStorey.fcui.zLocalPositionText;
			this.xRotationText = <RegisterFCUI>c__AnonStorey.fcui.xRotationText;
			this.yRotationText = <RegisterFCUI>c__AnonStorey.fcui.yRotationText;
			this.zRotationText = <RegisterFCUI>c__AnonStorey.fcui.zRotationText;
			this.xLocalRotationText = <RegisterFCUI>c__AnonStorey.fcui.xLocalRotationText;
			this.yLocalRotationText = <RegisterFCUI>c__AnonStorey.fcui.yLocalRotationText;
			this.zLocalRotationText = <RegisterFCUI>c__AnonStorey.fcui.zLocalRotationText;
			this.linkToAtomSelectionPopup = <RegisterFCUI>c__AnonStorey.fcui.linkToAtomSelectionPopup;
			this.linkToSelectionPopup = <RegisterFCUI>c__AnonStorey.fcui.linkToSelectionPopup;
		}
	}

	// Token: 0x060054DC RID: 21724 RVA: 0x001ECCC4 File Offset: 0x001EB0C4
	protected void DeregisterFCUI(FreeControllerV3UI fcui)
	{
		if (fcui.linkToAtomSelectionPopup != null)
		{
			UIPopup uipopup = this.linkToAtomSelectionPopup;
			uipopup.onOpenPopupHandlers = (UIPopup.OnOpenPopup)Delegate.Remove(uipopup.onOpenPopupHandlers, new UIPopup.OnOpenPopup(this.SetLinkToAtomNames));
			UIPopup uipopup2 = this.linkToAtomSelectionPopup;
			uipopup2.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Remove(uipopup2.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetLinkToAtom));
		}
		if (fcui.linkToSelectionPopup != null)
		{
			UIPopup uipopup3 = this.linkToSelectionPopup;
			uipopup3.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup3.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetLinkToRigidbodyObject));
		}
		if (fcui.selectLinkToFromSceneButton != null)
		{
			fcui.selectLinkToFromSceneButton.onClick.RemoveListener(new UnityAction(this.SelectLinkToRigidbodyFromScene));
		}
		if (fcui.selectAlignToFromSceneButton != null)
		{
			fcui.selectAlignToFromSceneButton.onClick.RemoveListener(new UnityAction(this.SelectAlignToRigidbodyFromScene));
		}
		if (fcui.xPositionMinus1Button != null)
		{
			fcui.xPositionMinus1Button.onClick.RemoveAllListeners();
		}
		if (fcui.xPositionMinusPoint1Button != null)
		{
			fcui.xPositionMinusPoint1Button.onClick.RemoveAllListeners();
		}
		if (fcui.xPositionMinusPoint01Button != null)
		{
			fcui.xPositionMinusPoint01Button.onClick.RemoveAllListeners();
		}
		if (fcui.xPositionPlusPoint01Button != null)
		{
			fcui.xPositionPlusPoint01Button.onClick.RemoveAllListeners();
		}
		if (fcui.xPositionPlusPoint1Button != null)
		{
			fcui.xPositionPlusPoint1Button.onClick.RemoveAllListeners();
		}
		if (fcui.xPositionPlus1Button != null)
		{
			fcui.xPositionPlus1Button.onClick.RemoveAllListeners();
		}
		if (fcui.yPositionMinus1Button != null)
		{
			fcui.yPositionMinus1Button.onClick.RemoveAllListeners();
		}
		if (fcui.yPositionMinusPoint1Button != null)
		{
			fcui.yPositionMinusPoint1Button.onClick.RemoveAllListeners();
		}
		if (fcui.yPositionMinusPoint01Button != null)
		{
			fcui.yPositionMinusPoint01Button.onClick.RemoveAllListeners();
		}
		if (fcui.yPositionPlusPoint01Button != null)
		{
			fcui.yPositionPlusPoint01Button.onClick.RemoveAllListeners();
		}
		if (fcui.yPositionPlusPoint1Button != null)
		{
			fcui.yPositionPlusPoint1Button.onClick.RemoveAllListeners();
		}
		if (fcui.yPositionPlus1Button != null)
		{
			fcui.yPositionPlus1Button.onClick.RemoveAllListeners();
		}
		if (fcui.zPositionMinus1Button != null)
		{
			fcui.zPositionMinus1Button.onClick.RemoveAllListeners();
		}
		if (fcui.zPositionMinusPoint1Button != null)
		{
			fcui.zPositionMinusPoint1Button.onClick.RemoveAllListeners();
		}
		if (fcui.zPositionMinusPoint01Button != null)
		{
			fcui.zPositionMinusPoint01Button.onClick.RemoveAllListeners();
		}
		if (fcui.zPositionPlusPoint01Button != null)
		{
			fcui.zPositionPlusPoint01Button.onClick.RemoveAllListeners();
		}
		if (fcui.zPositionPlusPoint1Button != null)
		{
			fcui.zPositionPlusPoint1Button.onClick.RemoveAllListeners();
		}
		if (fcui.zPositionPlus1Button != null)
		{
			fcui.zPositionPlus1Button.onClick.RemoveAllListeners();
		}
		if (fcui.xRotationMinus45Button != null)
		{
			fcui.xRotationMinus45Button.onClick.RemoveAllListeners();
		}
		if (fcui.xRotationMinus5Button != null)
		{
			fcui.xRotationMinus5Button.onClick.RemoveAllListeners();
		}
		if (fcui.xRotationMinusPoint5Button != null)
		{
			fcui.xRotationMinusPoint5Button.onClick.RemoveAllListeners();
		}
		if (fcui.xRotationPlusPoint5Button != null)
		{
			fcui.xRotationPlusPoint5Button.onClick.RemoveAllListeners();
		}
		if (fcui.xRotationPlus5Button != null)
		{
			fcui.xRotationPlus5Button.onClick.RemoveAllListeners();
		}
		if (fcui.xRotationPlus45Button != null)
		{
			fcui.xRotationPlus45Button.onClick.RemoveAllListeners();
		}
		if (fcui.yRotationMinus45Button != null)
		{
			fcui.yRotationMinus45Button.onClick.RemoveAllListeners();
		}
		if (fcui.yRotationMinus5Button != null)
		{
			fcui.yRotationMinus5Button.onClick.RemoveAllListeners();
		}
		if (fcui.yRotationMinusPoint5Button != null)
		{
			fcui.yRotationMinusPoint5Button.onClick.RemoveAllListeners();
		}
		if (fcui.yRotationPlusPoint5Button != null)
		{
			fcui.yRotationPlusPoint5Button.onClick.RemoveAllListeners();
		}
		if (fcui.yRotationPlus5Button != null)
		{
			fcui.yRotationPlus5Button.onClick.RemoveAllListeners();
		}
		if (fcui.yRotationPlus45Button != null)
		{
			fcui.yRotationPlus45Button.onClick.RemoveAllListeners();
		}
		if (fcui.zRotationMinus45Button != null)
		{
			fcui.zRotationMinus45Button.onClick.RemoveAllListeners();
		}
		if (fcui.zRotationMinus5Button != null)
		{
			fcui.zRotationMinus5Button.onClick.RemoveAllListeners();
		}
		if (fcui.zRotationMinusPoint5Button != null)
		{
			fcui.zRotationMinusPoint5Button.onClick.RemoveAllListeners();
		}
		if (fcui.zRotationPlusPoint5Button != null)
		{
			fcui.zRotationPlusPoint5Button.onClick.RemoveAllListeners();
		}
		if (fcui.zRotationPlus5Button != null)
		{
			fcui.zRotationPlus5Button.onClick.RemoveAllListeners();
		}
		if (fcui.zRotationPlus45Button != null)
		{
			fcui.zRotationPlus45Button.onClick.RemoveAllListeners();
		}
		if (fcui.xPosition0Button != null)
		{
			fcui.xPosition0Button.onClick.RemoveAllListeners();
		}
		if (fcui.yPosition0Button != null)
		{
			fcui.yPosition0Button.onClick.RemoveAllListeners();
		}
		if (fcui.zPosition0Button != null)
		{
			fcui.zPosition0Button.onClick.RemoveAllListeners();
		}
		if (fcui.xRotation0Button != null)
		{
			fcui.xRotation0Button.onClick.RemoveListener(new UnityAction(this.XRotation0));
		}
		if (fcui.yRotation0Button != null)
		{
			fcui.yRotation0Button.onClick.RemoveListener(new UnityAction(this.YRotation0));
		}
		if (fcui.zRotation0Button != null)
		{
			fcui.zRotation0Button.onClick.RemoveListener(new UnityAction(this.ZRotation0));
		}
		if (fcui.xPositionSnapPoint1Button != null)
		{
			fcui.xPositionSnapPoint1Button.onClick.RemoveListener(new UnityAction(this.XPositionSnapPoint1));
		}
		if (fcui.yPositionSnapPoint1Button != null)
		{
			fcui.yPositionSnapPoint1Button.onClick.RemoveListener(new UnityAction(this.YPositionSnapPoint1));
		}
		if (fcui.zPositionSnapPoint1Button != null)
		{
			fcui.zPositionSnapPoint1Button.onClick.RemoveListener(new UnityAction(this.ZPositionSnapPoint1));
		}
		if (fcui.xRotationSnap1Button != null)
		{
			fcui.xRotationSnap1Button.onClick.RemoveListener(new UnityAction(this.XRotationSnap1));
		}
		if (fcui.yRotationSnap1Button != null)
		{
			fcui.yRotationSnap1Button.onClick.RemoveListener(new UnityAction(this.YRotationSnap1));
		}
		if (fcui.zRotationSnap1Button != null)
		{
			fcui.zRotationSnap1Button.onClick.RemoveListener(new UnityAction(this.ZRotationSnap1));
		}
		if (fcui.xSelfRelativePositionAdjustInputField != null)
		{
			fcui.xSelfRelativePositionAdjustInputField.onEndEdit.RemoveAllListeners();
		}
		if (fcui.xSelfRelativePositionAdjustInputFieldAction != null)
		{
			fcui.xSelfRelativePositionAdjustInputFieldAction.onSubmitHandlers = null;
		}
		if (fcui.xSelfRelativePositionMinus1Button != null)
		{
			fcui.xSelfRelativePositionMinus1Button.onClick.RemoveAllListeners();
		}
		if (fcui.xSelfRelativePositionMinusPoint1Button != null)
		{
			fcui.xSelfRelativePositionMinusPoint1Button.onClick.RemoveAllListeners();
		}
		if (fcui.xSelfRelativePositionMinusPoint01Button != null)
		{
			fcui.xSelfRelativePositionMinusPoint01Button.onClick.RemoveAllListeners();
		}
		if (fcui.xSelfRelativePositionPlusPoint01Button != null)
		{
			fcui.xSelfRelativePositionPlusPoint01Button.onClick.RemoveAllListeners();
		}
		if (fcui.xSelfRelativePositionPlusPoint1Button != null)
		{
			fcui.xSelfRelativePositionPlusPoint1Button.onClick.RemoveAllListeners();
		}
		if (fcui.xSelfRelativePositionPlus1Button != null)
		{
			fcui.xSelfRelativePositionPlus1Button.onClick.RemoveAllListeners();
		}
		if (fcui.ySelfRelativePositionAdjustInputField != null)
		{
			fcui.ySelfRelativePositionAdjustInputField.onEndEdit.RemoveAllListeners();
		}
		if (fcui.ySelfRelativePositionAdjustInputFieldAction != null)
		{
			fcui.ySelfRelativePositionAdjustInputFieldAction.onSubmitHandlers = null;
		}
		if (fcui.ySelfRelativePositionMinus1Button != null)
		{
			fcui.ySelfRelativePositionMinus1Button.onClick.RemoveAllListeners();
		}
		if (fcui.ySelfRelativePositionMinusPoint1Button != null)
		{
			fcui.ySelfRelativePositionMinusPoint1Button.onClick.RemoveAllListeners();
		}
		if (fcui.ySelfRelativePositionMinusPoint01Button != null)
		{
			fcui.ySelfRelativePositionMinusPoint01Button.onClick.RemoveAllListeners();
		}
		if (fcui.ySelfRelativePositionPlusPoint01Button != null)
		{
			fcui.ySelfRelativePositionPlusPoint01Button.onClick.RemoveAllListeners();
		}
		if (fcui.ySelfRelativePositionPlusPoint1Button != null)
		{
			fcui.ySelfRelativePositionPlusPoint1Button.onClick.RemoveAllListeners();
		}
		if (fcui.ySelfRelativePositionPlus1Button != null)
		{
			fcui.ySelfRelativePositionPlus1Button.onClick.RemoveAllListeners();
		}
		if (fcui.zSelfRelativePositionAdjustInputField != null)
		{
			fcui.zSelfRelativePositionAdjustInputField.onEndEdit.RemoveAllListeners();
		}
		if (fcui.zSelfRelativePositionAdjustInputFieldAction != null)
		{
			fcui.zSelfRelativePositionAdjustInputFieldAction.onSubmitHandlers = null;
		}
		if (fcui.zSelfRelativePositionMinus1Button != null)
		{
			fcui.zSelfRelativePositionMinus1Button.onClick.RemoveAllListeners();
		}
		if (fcui.zSelfRelativePositionMinusPoint1Button != null)
		{
			fcui.zSelfRelativePositionMinusPoint1Button.onClick.RemoveAllListeners();
		}
		if (fcui.zSelfRelativePositionMinusPoint01Button != null)
		{
			fcui.zSelfRelativePositionMinusPoint01Button.onClick.RemoveAllListeners();
		}
		if (fcui.zSelfRelativePositionPlusPoint01Button != null)
		{
			fcui.zSelfRelativePositionPlusPoint01Button.onClick.RemoveAllListeners();
		}
		if (fcui.zSelfRelativePositionPlusPoint1Button != null)
		{
			fcui.zSelfRelativePositionPlusPoint1Button.onClick.RemoveAllListeners();
		}
		if (fcui.zSelfRelativePositionPlus1Button != null)
		{
			fcui.zSelfRelativePositionPlus1Button.onClick.RemoveAllListeners();
		}
		if (fcui.xSelfRelativeRotationAdjustInputField != null)
		{
			fcui.xSelfRelativeRotationAdjustInputField.onEndEdit.RemoveAllListeners();
		}
		if (fcui.xSelfRelativeRotationAdjustInputFieldAction != null)
		{
			fcui.xSelfRelativeRotationAdjustInputFieldAction.onSubmitHandlers = null;
		}
		if (fcui.xSelfRelativeRotationMinus45Button != null)
		{
			fcui.xSelfRelativeRotationMinus45Button.onClick.RemoveAllListeners();
		}
		if (fcui.xSelfRelativeRotationMinus5Button != null)
		{
			fcui.xSelfRelativeRotationMinus5Button.onClick.RemoveAllListeners();
		}
		if (fcui.xSelfRelativeRotationMinusPoint5Button != null)
		{
			fcui.xSelfRelativeRotationMinusPoint5Button.onClick.RemoveAllListeners();
		}
		if (fcui.xSelfRelativeRotationPlusPoint5Button != null)
		{
			fcui.xSelfRelativeRotationPlusPoint5Button.onClick.RemoveAllListeners();
		}
		if (fcui.xSelfRelativeRotationPlus5Button != null)
		{
			fcui.xSelfRelativeRotationPlus5Button.onClick.RemoveAllListeners();
		}
		if (fcui.xSelfRelativeRotationPlus45Button != null)
		{
			fcui.xSelfRelativeRotationPlus45Button.onClick.RemoveAllListeners();
		}
		if (fcui.ySelfRelativeRotationAdjustInputField != null)
		{
			fcui.ySelfRelativeRotationAdjustInputField.onEndEdit.RemoveAllListeners();
		}
		if (fcui.ySelfRelativeRotationAdjustInputFieldAction != null)
		{
			fcui.ySelfRelativeRotationAdjustInputFieldAction.onSubmitHandlers = null;
		}
		if (fcui.ySelfRelativeRotationMinus45Button != null)
		{
			fcui.ySelfRelativeRotationMinus45Button.onClick.RemoveAllListeners();
		}
		if (fcui.ySelfRelativeRotationMinus5Button != null)
		{
			fcui.ySelfRelativeRotationMinus5Button.onClick.RemoveAllListeners();
		}
		if (fcui.ySelfRelativeRotationMinusPoint5Button != null)
		{
			fcui.ySelfRelativeRotationMinusPoint5Button.onClick.RemoveAllListeners();
		}
		if (fcui.ySelfRelativeRotationPlusPoint5Button != null)
		{
			fcui.ySelfRelativeRotationPlusPoint5Button.onClick.RemoveAllListeners();
		}
		if (fcui.ySelfRelativeRotationPlus5Button != null)
		{
			fcui.ySelfRelativeRotationPlus5Button.onClick.RemoveAllListeners();
		}
		if (fcui.ySelfRelativeRotationPlus45Button != null)
		{
			fcui.ySelfRelativeRotationPlus45Button.onClick.RemoveAllListeners();
		}
		if (fcui.zSelfRelativeRotationAdjustInputField != null)
		{
			fcui.zSelfRelativeRotationAdjustInputField.onEndEdit.RemoveAllListeners();
		}
		if (fcui.zSelfRelativeRotationAdjustInputFieldAction != null)
		{
			fcui.zSelfRelativeRotationAdjustInputFieldAction.onSubmitHandlers = null;
		}
		if (fcui.zSelfRelativeRotationMinus45Button != null)
		{
			fcui.zSelfRelativeRotationMinus45Button.onClick.RemoveAllListeners();
		}
		if (fcui.zSelfRelativeRotationMinus5Button != null)
		{
			fcui.zSelfRelativeRotationMinus5Button.onClick.RemoveAllListeners();
		}
		if (fcui.zSelfRelativeRotationMinusPoint5Button != null)
		{
			fcui.zSelfRelativeRotationMinusPoint5Button.onClick.RemoveAllListeners();
		}
		if (fcui.zSelfRelativeRotationPlusPoint5Button != null)
		{
			fcui.zSelfRelativeRotationPlusPoint5Button.onClick.RemoveAllListeners();
		}
		if (fcui.zSelfRelativeRotationPlus5Button != null)
		{
			fcui.zSelfRelativeRotationPlus5Button.onClick.RemoveAllListeners();
		}
		if (fcui.zSelfRelativeRotationPlus45Button != null)
		{
			fcui.zSelfRelativeRotationPlus45Button.onClick.RemoveAllListeners();
		}
		if (this.enableSelectRoot && fcui.selectRootButton != null)
		{
			fcui.selectRootButton.onClick.RemoveListener(new UnityAction(this.SelectRoot));
		}
		if (fcui.xPositionInputField != null)
		{
			fcui.xPositionInputField.onEndEdit.RemoveAllListeners();
		}
		if (fcui.xPositionInputFieldAction != null)
		{
			InputFieldAction xPositionInputFieldAction = fcui.xPositionInputFieldAction;
			xPositionInputFieldAction.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Remove(xPositionInputFieldAction.onSubmitHandlers, new InputFieldAction.OnSubmit(this.SetXPositionToInputField));
		}
		if (fcui.yPositionInputField != null)
		{
			fcui.yPositionInputField.onEndEdit.RemoveAllListeners();
		}
		if (fcui.yPositionInputFieldAction != null)
		{
			InputFieldAction yPositionInputFieldAction = fcui.yPositionInputFieldAction;
			yPositionInputFieldAction.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Remove(yPositionInputFieldAction.onSubmitHandlers, new InputFieldAction.OnSubmit(this.SetYPositionToInputField));
		}
		if (fcui.zPositionInputField != null)
		{
			fcui.zPositionInputField.onEndEdit.RemoveAllListeners();
		}
		if (fcui.zPositionInputFieldAction != null)
		{
			InputFieldAction zPositionInputFieldAction = fcui.zPositionInputFieldAction;
			zPositionInputFieldAction.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Remove(zPositionInputFieldAction.onSubmitHandlers, new InputFieldAction.OnSubmit(this.SetZPositionToInputField));
		}
		if (fcui.xRotationInputField != null)
		{
			fcui.xRotationInputField.onEndEdit.RemoveAllListeners();
		}
		if (fcui.xRotationInputFieldAction != null)
		{
			InputFieldAction xRotationInputFieldAction = fcui.xRotationInputFieldAction;
			xRotationInputFieldAction.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Remove(xRotationInputFieldAction.onSubmitHandlers, new InputFieldAction.OnSubmit(this.SetXRotationToInputField));
		}
		if (fcui.yRotationInputField != null)
		{
			fcui.yRotationInputField.onEndEdit.RemoveAllListeners();
		}
		if (fcui.yRotationInputFieldAction != null)
		{
			InputFieldAction yRotationInputFieldAction = fcui.yRotationInputFieldAction;
			yRotationInputFieldAction.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Remove(yRotationInputFieldAction.onSubmitHandlers, new InputFieldAction.OnSubmit(this.SetYRotationToInputField));
		}
		if (fcui.zRotationInputField != null)
		{
			fcui.zRotationInputField.onEndEdit.RemoveAllListeners();
		}
		if (fcui.zRotationInputFieldAction != null)
		{
			InputFieldAction zRotationInputFieldAction = fcui.zRotationInputFieldAction;
			zRotationInputFieldAction.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Remove(zRotationInputFieldAction.onSubmitHandlers, new InputFieldAction.OnSubmit(this.SetZRotationToInputField));
		}
		if (fcui.xLocalPositionInputField != null)
		{
			fcui.xLocalPositionInputField.onEndEdit.RemoveAllListeners();
		}
		if (fcui.xLocalPositionInputFieldAction != null)
		{
			InputFieldAction xLocalPositionInputFieldAction = fcui.xLocalPositionInputFieldAction;
			xLocalPositionInputFieldAction.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Remove(xLocalPositionInputFieldAction.onSubmitHandlers, new InputFieldAction.OnSubmit(this.SetXLocalPositionToInputField));
		}
		if (fcui.yLocalPositionInputField != null)
		{
			fcui.yLocalPositionInputField.onEndEdit.RemoveAllListeners();
		}
		if (fcui.yLocalPositionInputFieldAction != null)
		{
			InputFieldAction yLocalPositionInputFieldAction = fcui.yLocalPositionInputFieldAction;
			yLocalPositionInputFieldAction.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Remove(yLocalPositionInputFieldAction.onSubmitHandlers, new InputFieldAction.OnSubmit(this.SetYLocalPositionToInputField));
		}
		if (fcui.zLocalPositionInputField != null)
		{
			fcui.zLocalPositionInputField.onEndEdit.RemoveAllListeners();
		}
		if (fcui.zLocalPositionInputFieldAction != null)
		{
			InputFieldAction zLocalPositionInputFieldAction = fcui.zLocalPositionInputFieldAction;
			zLocalPositionInputFieldAction.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Remove(zLocalPositionInputFieldAction.onSubmitHandlers, new InputFieldAction.OnSubmit(this.SetZLocalPositionToInputField));
		}
		if (fcui.xLocalRotationInputField != null)
		{
			fcui.xLocalRotationInputField.onEndEdit.RemoveAllListeners();
		}
		if (fcui.xLocalRotationInputFieldAction != null)
		{
			InputFieldAction xLocalRotationInputFieldAction = fcui.xLocalRotationInputFieldAction;
			xLocalRotationInputFieldAction.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Remove(xLocalRotationInputFieldAction.onSubmitHandlers, new InputFieldAction.OnSubmit(this.SetXLocalRotationToInputField));
		}
		if (fcui.yLocalRotationInputField != null)
		{
			fcui.yLocalRotationInputField.onEndEdit.RemoveAllListeners();
		}
		if (fcui.yLocalRotationInputFieldAction != null)
		{
			InputFieldAction yLocalRotationInputFieldAction = fcui.yLocalRotationInputFieldAction;
			yLocalRotationInputFieldAction.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Remove(yLocalRotationInputFieldAction.onSubmitHandlers, new InputFieldAction.OnSubmit(this.SetYLocalRotationToInputField));
		}
		if (fcui.zLocalRotationInputField != null)
		{
			fcui.zLocalRotationInputField.onEndEdit.RemoveAllListeners();
		}
		if (fcui.zLocalRotationInputFieldAction != null)
		{
			InputFieldAction zLocalRotationInputFieldAction = fcui.zLocalRotationInputFieldAction;
			zLocalRotationInputFieldAction.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Remove(zLocalRotationInputFieldAction.onSubmitHandlers, new InputFieldAction.OnSubmit(this.SetZLocalRotationToInputField));
		}
		if (fcui.zeroXLocalPositionButton != null)
		{
			fcui.zeroXLocalPositionButton.onClick.RemoveAllListeners();
		}
		if (fcui.zeroYLocalPositionButton != null)
		{
			fcui.zeroYLocalPositionButton.onClick.RemoveAllListeners();
		}
		if (fcui.zeroZLocalPositionButton != null)
		{
			fcui.zeroZLocalPositionButton.onClick.RemoveAllListeners();
		}
		if (fcui.zeroXLocalRotationButton != null)
		{
			fcui.zeroXLocalRotationButton.onClick.RemoveAllListeners();
		}
		if (fcui.zeroYLocalRotationButton != null)
		{
			fcui.zeroYLocalRotationButton.onClick.RemoveAllListeners();
		}
		if (fcui.zeroZLocalRotationButton != null)
		{
			fcui.zeroZLocalRotationButton.onClick.RemoveAllListeners();
		}
	}

	// Token: 0x060054DD RID: 21725 RVA: 0x001EDFA4 File Offset: 0x001EC3A4
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			FreeControllerV3UI componentInChildren = this.UITransform.GetComponentInChildren<FreeControllerV3UI>(true);
			if (componentInChildren != null)
			{
				if (this.currentFCUI != null)
				{
					this.DeregisterFCUI(this.currentFCUI);
				}
				this.currentFCUI = componentInChildren;
				this.RegisterFCUI(componentInChildren, false);
			}
		}
	}

	// Token: 0x060054DE RID: 21726 RVA: 0x001EE008 File Offset: 0x001EC408
	public override void InitUIAlt()
	{
		if (this.UITransformAlt != null)
		{
			FreeControllerV3UI componentInChildren = this.UITransformAlt.GetComponentInChildren<FreeControllerV3UI>(true);
			if (componentInChildren != null)
			{
				if (this.currentFCUIAlt != null)
				{
					this.DeregisterFCUI(this.currentFCUIAlt);
				}
				this.currentFCUIAlt = componentInChildren;
				this.RegisterFCUI(componentInChildren, true);
			}
		}
	}

	// Token: 0x060054DF RID: 21727 RVA: 0x001EE06B File Offset: 0x001EC46B
	public void TakeSnapshot()
	{
		this.snapshotMatrix = this.control.localToWorldMatrix;
	}

	// Token: 0x060054E0 RID: 21728 RVA: 0x001EE080 File Offset: 0x001EC480
	private Vector3 GetVectorFromAxis(FreeControllerV3.DrawAxisnames axis)
	{
		Vector3 zero = Vector3.zero;
		switch (axis)
		{
		case FreeControllerV3.DrawAxisnames.X:
			zero.x = 1f;
			break;
		case FreeControllerV3.DrawAxisnames.Y:
			zero.y = 1f;
			break;
		case FreeControllerV3.DrawAxisnames.Z:
			zero.z = 1f;
			break;
		case FreeControllerV3.DrawAxisnames.NegX:
			zero.x = -1f;
			break;
		case FreeControllerV3.DrawAxisnames.NegY:
			zero.y = -1f;
			break;
		case FreeControllerV3.DrawAxisnames.NegZ:
			zero.z = -1f;
			break;
		}
		return zero;
	}

	// Token: 0x060054E1 RID: 21729 RVA: 0x001EE120 File Offset: 0x001EC520
	private void ApplyForce()
	{
		if (this._followWhenOffRB)
		{
			if (this._moveForceEnabled)
			{
				this._followWhenOffRB.AddForce(this.appliedForce * Time.fixedDeltaTime, ForceMode.Force);
			}
			if (this._rotationForceEnabled)
			{
				this._followWhenOffRB.AddRelativeTorque(this.appliedTorque * Time.fixedDeltaTime, ForceMode.Force);
			}
		}
	}

	// Token: 0x060054E2 RID: 21730 RVA: 0x001EE18B File Offset: 0x001EC58B
	protected void SyncComplyPositionThreshold(float f)
	{
		this.complyPositionThreshold = f;
	}

	// Token: 0x060054E3 RID: 21731 RVA: 0x001EE194 File Offset: 0x001EC594
	protected void SyncComplyRotationThreshold(float f)
	{
		this.complyRotationThreshold = f;
	}

	// Token: 0x060054E4 RID: 21732 RVA: 0x001EE19D File Offset: 0x001EC59D
	protected void SyncComplySpeed(float f)
	{
		this.complySpeed = f;
	}

	// Token: 0x060054E5 RID: 21733 RVA: 0x001EE1A6 File Offset: 0x001EC5A6
	public void PauseComply(int numFrames = 100)
	{
		if (this.complyPauseFrames < numFrames)
		{
			this.complyPauseFrames = numFrames;
		}
	}

	// Token: 0x060054E6 RID: 21734 RVA: 0x001EE1BC File Offset: 0x001EC5BC
	private void ApplyComply()
	{
		if (!this._resetSimulation && !this._freezeSimulation)
		{
			if (this.complyPauseFrames > 0)
			{
				this.complyPauseFrames--;
			}
			if (this.complyPauseFrames == 0 && this.control != null && this._followWhenOffRB != null && !this._followWhenOffRB.isKinematic)
			{
				bool flag = false;
				if (this._currentPositionState == FreeControllerV3.PositionState.Comply)
				{
					Vector3 a = this._followWhenOffRB.position - Physics.gravity * Time.fixedDeltaTime * Time.fixedDeltaTime;
					Vector3 a2 = (a - this.control.position) / this._scale;
					if (a2.sqrMagnitude > this.complyPositionThreshold)
					{
						this.control.position += a2 * Time.fixedDeltaTime * this.complySpeed;
						if (this.onPositionChangeHandlers != null)
						{
							this.onPositionChangeHandlers(this);
						}
						flag = true;
					}
				}
				if (this._currentRotationState == FreeControllerV3.RotationState.Comply)
				{
					float num = Quaternion.Angle(this.control.rotation, this._followWhenOffRB.rotation);
					if (num > this.complyRotationThreshold)
					{
						this.control.rotation = Quaternion.Lerp(this.control.rotation, this._followWhenOffRB.rotation, Mathf.Clamp01(Time.fixedDeltaTime * this.complySpeed));
						if (this.onRotationChangeHandlers != null)
						{
							this.onRotationChangeHandlers(this);
						}
						flag = true;
					}
				}
				if (flag && this.onMovementHandlers != null)
				{
					this.onMovementHandlers(this);
				}
			}
		}
	}

	// Token: 0x060054E7 RID: 21735 RVA: 0x001EE383 File Offset: 0x001EC783
	public void MoveControlRelatve(Vector3 move)
	{
		this.MoveControl(this.control.position + move);
	}

	// Token: 0x060054E8 RID: 21736 RVA: 0x001EE39C File Offset: 0x001EC79C
	public void MoveControl(Vector3 newPosition)
	{
		this.MoveControl(newPosition, true);
	}

	// Token: 0x060054E9 RID: 21737 RVA: 0x001EE3A8 File Offset: 0x001EC7A8
	public void MoveControl(Vector3 newPosition, bool callHandlers)
	{
		if (this._positionGridMode == FreeControllerV3.GridMode.Global)
		{
			if (GridControl.singleton != null)
			{
				float positionGrid = GridControl.singleton.positionGrid;
				float num = Mathf.Round(newPosition.x / positionGrid);
				newPosition.x = num * positionGrid;
				float num2 = Mathf.Round(newPosition.y / positionGrid);
				newPosition.y = num2 * positionGrid;
				float num3 = Mathf.Round(newPosition.z / positionGrid);
				newPosition.z = num3 * positionGrid;
			}
		}
		else if (this._positionGridMode == FreeControllerV3.GridMode.Local)
		{
			float num4 = Mathf.Round(newPosition.x / this._positionGrid);
			newPosition.x = num4 * this._positionGrid;
			float num5 = Mathf.Round(newPosition.y / this._positionGrid);
			newPosition.y = num5 * this._positionGrid;
			float num6 = Mathf.Round(newPosition.z / this._positionGrid);
			newPosition.z = num6 * this._positionGrid;
		}
		Vector3 position = this.control.position;
		Vector3 vector = Vector3.zero;
		if (this._xLocalLock || this._yLocalLock || this._zLocalLock)
		{
			vector = this.GetLocalPosition();
		}
		if (!this._xLock)
		{
			position.x = newPosition.x;
		}
		if (!this._yLock)
		{
			position.y = newPosition.y;
		}
		if (!this._zLock)
		{
			position.z = newPosition.z;
		}
		this.control.position = position;
		if (this._xLocalLock || this._yLocalLock || this._zLocalLock)
		{
			Vector3 localPosition = this.GetLocalPosition();
			if (this._xLocalLock)
			{
				localPosition.x = vector.x;
			}
			if (this._yLocalLock)
			{
				localPosition.y = vector.y;
			}
			if (this._zLocalLock)
			{
				localPosition.z = vector.z;
			}
			this.SetLocalPosition(localPosition);
		}
		if (callHandlers)
		{
			if (this.onPositionChangeHandlers != null)
			{
				this.onPositionChangeHandlers(this);
			}
			if (this.onMovementHandlers != null)
			{
				this.onMovementHandlers(this);
			}
		}
	}

	// Token: 0x060054EA RID: 21738 RVA: 0x001EE5EC File Offset: 0x001EC9EC
	public void MoveLinkConnectorTowards(Transform t, float moveDistance)
	{
		if (this._linkToConnector != null)
		{
			this._linkToConnector.transform.Translate(0f, 0f, moveDistance, t);
		}
	}

	// Token: 0x060054EB RID: 21739 RVA: 0x001EE61B File Offset: 0x001ECA1B
	public void RotateControl(Vector3 axis, float angle)
	{
		this.RotateControl(axis, angle, true);
	}

	// Token: 0x060054EC RID: 21740 RVA: 0x001EE628 File Offset: 0x001ECA28
	public void RotateControl(Vector3 axis, float angle, bool callHandlers)
	{
		Vector3 eulerAngles = this.control.eulerAngles;
		this.control.transform.Rotate(axis, angle, Space.World);
		Vector3 eulerAngles2 = this.control.eulerAngles;
		this.RotateControlContrained(eulerAngles, eulerAngles2, callHandlers);
	}

	// Token: 0x060054ED RID: 21741 RVA: 0x001EE669 File Offset: 0x001ECA69
	public void RotateControl(Vector3 newWorldRotation)
	{
		this.RotateControl(newWorldRotation, true);
	}

	// Token: 0x060054EE RID: 21742 RVA: 0x001EE674 File Offset: 0x001ECA74
	public void RotateControl(Vector3 newWorldRotation, bool callHandlers)
	{
		Vector3 eulerAngles = this.control.eulerAngles;
		this.RotateControlContrained(eulerAngles, newWorldRotation, callHandlers);
	}

	// Token: 0x060054EF RID: 21743 RVA: 0x001EE698 File Offset: 0x001ECA98
	private void RotateControlContrained(Vector3 oldRotation, Vector3 newRotation, bool callHandlers)
	{
		if (this._rotationGridMode == FreeControllerV3.GridMode.Global)
		{
			if (GridControl.singleton != null)
			{
				float rotationGrid = GridControl.singleton.rotationGrid;
				float num = Mathf.Round(newRotation.x / rotationGrid);
				newRotation.x = num * rotationGrid;
				float num2 = Mathf.Round(newRotation.y / rotationGrid);
				newRotation.y = num2 * rotationGrid;
				float num3 = Mathf.Round(newRotation.z / rotationGrid);
				newRotation.z = num3 * rotationGrid;
			}
		}
		else if (this._rotationGridMode == FreeControllerV3.GridMode.Local)
		{
			float num4 = Mathf.Round(newRotation.x / this._rotationGrid);
			newRotation.x = num4 * this._rotationGrid;
			float num5 = Mathf.Round(newRotation.y / this._rotationGrid);
			newRotation.y = num5 * this._rotationGrid;
			float num6 = Mathf.Round(newRotation.z / this._rotationGrid);
			newRotation.z = num6 * this._rotationGrid;
		}
		if (!this.xRotLock)
		{
			oldRotation.x = newRotation.x;
		}
		if (!this.yRotLock)
		{
			oldRotation.y = newRotation.y;
		}
		if (!this.zRotLock)
		{
			oldRotation.z = newRotation.z;
		}
		this.control.eulerAngles = oldRotation;
		if (callHandlers)
		{
			if (this.onRotationChangeHandlers != null)
			{
				this.onRotationChangeHandlers(this);
			}
			if (this.onMovementHandlers != null)
			{
				this.onMovementHandlers(this);
			}
		}
	}

	// Token: 0x060054F0 RID: 21744 RVA: 0x001EE824 File Offset: 0x001ECC24
	private void SyncMoveWhenInactive()
	{
		if (this.alsoMoveWhenInactive != null && this.alsoMoveWhenInactiveParentWhenActive != null && this.alsoMoveWhenInactiveParentWhenInactive != null)
		{
			if (this.alsoMoveWhenInactive.gameObject.activeInHierarchy && !this._freezeSimulation)
			{
				if (this.alsoMoveWhenInactive.parent != this.alsoMoveWhenInactiveParentWhenActive)
				{
					this.alsoMoveWhenInactive.SetParent(this.alsoMoveWhenInactiveParentWhenActive);
					if (this.alsoMoveWhenInactiveAlternate != null)
					{
						Vector3 position = this.alsoMoveWhenInactiveAlternate.position;
						Quaternion rotation = this.alsoMoveWhenInactiveAlternate.rotation;
						this.alsoMoveWhenInactive.localPosition = Vector3.zero;
						this.alsoMoveWhenInactive.localRotation = Quaternion.identity;
						this.alsoMoveWhenInactive.localScale = Vector3.one;
						this.alsoMoveWhenInactiveAlternate.position = position;
						this.alsoMoveWhenInactiveAlternate.rotation = rotation;
					}
				}
			}
			else
			{
				if (!this._detachControl)
				{
					this.alsoMoveWhenInactiveParentWhenInactive.position = this.control.position;
					this.alsoMoveWhenInactiveParentWhenInactive.rotation = this.control.rotation;
				}
				if (this.alsoMoveWhenInactive.parent != this.alsoMoveWhenInactiveParentWhenInactive)
				{
					this.alsoMoveWhenInactive.SetParent(this.alsoMoveWhenInactiveParentWhenInactive);
					if (this.alsoMoveWhenInactiveAlternate != null)
					{
						Vector3 position2 = this.alsoMoveWhenInactiveAlternate.position;
						Quaternion rotation2 = this.alsoMoveWhenInactiveAlternate.rotation;
						this.alsoMoveWhenInactive.localPosition = Vector3.zero;
						this.alsoMoveWhenInactive.localRotation = Quaternion.identity;
						this.alsoMoveWhenInactive.localScale = Vector3.one;
						this.alsoMoveWhenInactiveAlternate.position = position2;
						this.alsoMoveWhenInactiveAlternate.rotation = rotation2;
					}
				}
			}
		}
	}

	// Token: 0x060054F1 RID: 21745 RVA: 0x001EE9FC File Offset: 0x001ECDFC
	private void UpdateTransform(bool updateGUI)
	{
		if (this._currentPositionState == FreeControllerV3.PositionState.Off)
		{
			if (this.followWhenOff)
			{
				Vector3 position = this.followWhenOff.position;
				if (NaNUtils.IsVector3Valid(position))
				{
					this.control.position = this.followWhenOff.position;
				}
				else if (this.containingAtom != null)
				{
					this.containingAtom.AlertPhysicsCorruption("FreeController position " + base.name);
				}
			}
		}
		else if (this._currentPositionState == FreeControllerV3.PositionState.Following)
		{
			if (this.follow)
			{
				this.MoveControl(this.follow.position, false);
			}
		}
		else if ((this._currentPositionState == FreeControllerV3.PositionState.ParentLink || this._currentPositionState == FreeControllerV3.PositionState.PhysicsLink) && this._linkToConnector)
		{
			this.MoveControl(this._linkToConnector.position, this._isGrabbing);
		}
		if (this._currentRotationState == FreeControllerV3.RotationState.Off)
		{
			if (this.followWhenOff)
			{
				this.control.rotation = this.followWhenOff.rotation;
			}
		}
		else if (this._currentRotationState == FreeControllerV3.RotationState.Following)
		{
			if (this.follow)
			{
				this.RotateControl(this.follow.eulerAngles, false);
			}
		}
		else if (this._currentRotationState == FreeControllerV3.RotationState.LookAt)
		{
			this.control.LookAt(this.lookAt.position);
		}
		else if ((this._currentRotationState == FreeControllerV3.RotationState.ParentLink || this._currentPositionState == FreeControllerV3.PositionState.PhysicsLink) && this._linkToConnector)
		{
			this.RotateControl(this._linkToConnector.eulerAngles, this._isGrabbing);
		}
		if (this.control != base.transform)
		{
			base.transform.position = this.control.position;
			base.transform.rotation = this.control.rotation;
		}
		this.SyncMoveWhenInactive();
		if (this._followWhenOffRB != null)
		{
			if (!this._followWhenOffRB.gameObject.activeInHierarchy)
			{
				if (!this._followWhenOffRB.isKinematic || !this._detachControl)
				{
					this._followWhenOffRB.position = this.control.position;
					this.followWhenOff.position = this.control.position;
					this._followWhenOffRB.rotation = this.control.rotation;
					this.followWhenOff.rotation = this.control.rotation;
				}
			}
			else if (this._followWhenOffRB.isKinematic && !this._detachControl)
			{
				this.followWhenOff.position = this.control.position;
				this.followWhenOff.rotation = this.control.rotation;
			}
		}
		if (SuperController.singleton != null && !SuperController.singleton.autoSimulation && this.kinematicRB != null)
		{
			this.kinematicRB.position = this.control.position;
			this.kinematicRB.rotation = this.control.rotation;
		}
		if (updateGUI && !this._guihidden)
		{
			Vector3 vector = this.control.position;
			if (this.xPositionText != null)
			{
				this.xPositionText.floatVal = vector.x;
			}
			if (this.yPositionText != null)
			{
				this.yPositionText.floatVal = vector.y;
			}
			if (this.zPositionText != null)
			{
				this.zPositionText.floatVal = vector.z;
			}
			Vector3 vector2 = this.control.eulerAngles;
			if (this.xRotationText != null)
			{
				this.xRotationText.floatVal = vector2.x;
			}
			if (this.yRotationText != null)
			{
				this.yRotationText.floatVal = vector2.y;
			}
			if (this.zRotationText != null)
			{
				this.zRotationText.floatVal = vector2.z;
			}
			vector = this.GetLocalPosition();
			if (this.xLocalPositionText != null)
			{
				this.xLocalPositionText.floatVal = vector.x;
			}
			if (this.yLocalPositionText != null)
			{
				this.yLocalPositionText.floatVal = vector.y;
			}
			if (this.zLocalPositionText != null)
			{
				this.zLocalPositionText.floatVal = vector.z;
			}
			vector2 = this.GetLocalEulerAngles();
			if (this.xLocalRotationText != null)
			{
				this.xLocalRotationText.floatVal = vector2.x;
			}
			if (this.yLocalRotationText != null)
			{
				this.yLocalRotationText.floatVal = vector2.y;
			}
			if (this.zLocalRotationText != null)
			{
				this.zLocalRotationText.floatVal = vector2.z;
			}
		}
	}

	// Token: 0x060054F2 RID: 21746 RVA: 0x001EEF30 File Offset: 0x001ED330
	public void ShowGUI()
	{
		this.SyncAtomUID();
		bool flag = false;
		if (SuperController.singleton != null && SuperController.singleton.gameMode == SuperController.GameMode.Play)
		{
			flag = true;
		}
		if (flag)
		{
			foreach (Transform transform in this.UITransforms)
			{
				if (transform != null)
				{
					transform.gameObject.SetActive(false);
				}
			}
			foreach (Transform transform2 in this.UITransformsPlayMode)
			{
				if (transform2 != null)
				{
					transform2.gameObject.SetActive(true);
				}
			}
		}
		else
		{
			foreach (Transform transform3 in this.UITransformsPlayMode)
			{
				if (transform3 != null)
				{
					transform3.gameObject.SetActive(false);
				}
			}
			foreach (Transform transform4 in this.UITransforms)
			{
				if (transform4 != null)
				{
					transform4.gameObject.SetActive(true);
				}
			}
		}
	}

	// Token: 0x060054F3 RID: 21747 RVA: 0x001EF070 File Offset: 0x001ED470
	public void HideGUI()
	{
		bool flag = false;
		if (SuperController.singleton != null && SuperController.singleton.gameMode == SuperController.GameMode.Play)
		{
			flag = true;
		}
		foreach (Transform transform in this.UITransforms)
		{
			if (transform != null)
			{
				if (flag)
				{
					transform.gameObject.SetActive(false);
				}
				else
				{
					UIVisibility component = transform.GetComponent<UIVisibility>();
					if (component != null)
					{
						if (!component.keepVisible)
						{
							transform.gameObject.SetActive(false);
						}
					}
					else
					{
						transform.gameObject.SetActive(false);
					}
				}
			}
		}
		foreach (Transform transform2 in this.UITransformsPlayMode)
		{
			if (transform2 != null)
			{
				if (!flag)
				{
					transform2.gameObject.SetActive(false);
				}
				else
				{
					UIVisibility component2 = transform2.GetComponent<UIVisibility>();
					if (component2 != null)
					{
						if (!component2.keepVisible)
						{
							transform2.gameObject.SetActive(false);
						}
					}
					else
					{
						transform2.gameObject.SetActive(false);
					}
				}
			}
		}
	}

	// Token: 0x060054F4 RID: 21748 RVA: 0x001EF1B0 File Offset: 0x001ED5B0
	private void SetColor()
	{
		if (this._selected)
		{
			this._currentPositionColor = this.selectedColor;
			this._currentRotationColor = this.selectedColor;
		}
		else if (this._highlighted)
		{
			this._currentPositionColor = this.highlightColor;
			this._currentRotationColor = this.highlightColor;
		}
		else
		{
			switch (this._currentPositionState)
			{
			case FreeControllerV3.PositionState.On:
			case FreeControllerV3.PositionState.Comply:
				this._currentPositionColor = this.onColor;
				break;
			case FreeControllerV3.PositionState.Off:
				this._currentPositionColor = this.offColor;
				break;
			case FreeControllerV3.PositionState.Following:
			case FreeControllerV3.PositionState.ParentLink:
			case FreeControllerV3.PositionState.PhysicsLink:
				this._currentPositionColor = this.followingColor;
				break;
			case FreeControllerV3.PositionState.Hold:
				this._currentPositionColor = this.holdColor;
				break;
			case FreeControllerV3.PositionState.Lock:
				this._currentPositionColor = this.lockColor;
				break;
			}
			switch (this._currentRotationState)
			{
			case FreeControllerV3.RotationState.On:
			case FreeControllerV3.RotationState.Comply:
				this._currentRotationColor = this.onColor;
				break;
			case FreeControllerV3.RotationState.Off:
				this._currentRotationColor = this.offColor;
				break;
			case FreeControllerV3.RotationState.Following:
			case FreeControllerV3.RotationState.ParentLink:
			case FreeControllerV3.RotationState.PhysicsLink:
				this._currentRotationColor = this.followingColor;
				break;
			case FreeControllerV3.RotationState.Hold:
				this._currentRotationColor = this.holdColor;
				break;
			case FreeControllerV3.RotationState.Lock:
				this._currentRotationColor = this.lockColor;
				break;
			case FreeControllerV3.RotationState.LookAt:
				this._currentRotationColor = this.lookAtColor;
				break;
			}
		}
		if (this.mrs != null)
		{
			foreach (MeshRenderer meshRenderer in this.mrs)
			{
				meshRenderer.material.color = this._currentPositionColor;
			}
		}
	}

	// Token: 0x060054F5 RID: 21749 RVA: 0x001EF370 File Offset: 0x001ED770
	private void SetMesh()
	{
		switch (this._currentPositionState)
		{
		case FreeControllerV3.PositionState.On:
		case FreeControllerV3.PositionState.Comply:
			this._currentPositionMesh = this.onPositionMesh;
			break;
		case FreeControllerV3.PositionState.Off:
			this._currentPositionMesh = this.offPositionMesh;
			break;
		case FreeControllerV3.PositionState.Following:
		case FreeControllerV3.PositionState.ParentLink:
		case FreeControllerV3.PositionState.PhysicsLink:
			this._currentPositionMesh = this.followingPositionMesh;
			break;
		case FreeControllerV3.PositionState.Hold:
			this._currentPositionMesh = this.holdPositionMesh;
			break;
		case FreeControllerV3.PositionState.Lock:
			this._currentPositionMesh = this.lockPositionMesh;
			break;
		}
		switch (this._currentRotationState)
		{
		case FreeControllerV3.RotationState.On:
		case FreeControllerV3.RotationState.Comply:
			this._currentRotationMesh = this.onRotationMesh;
			break;
		case FreeControllerV3.RotationState.Off:
			this._currentRotationMesh = this.offRotationMesh;
			break;
		case FreeControllerV3.RotationState.Following:
		case FreeControllerV3.RotationState.ParentLink:
		case FreeControllerV3.RotationState.PhysicsLink:
			this._currentRotationMesh = this.followingRotationMesh;
			break;
		case FreeControllerV3.RotationState.Hold:
			this._currentRotationMesh = this.holdRotationMesh;
			break;
		case FreeControllerV3.RotationState.Lock:
			this._currentRotationMesh = this.lockRotationMesh;
			break;
		case FreeControllerV3.RotationState.LookAt:
			this._currentRotationMesh = this.lookAtRotationMesh;
			break;
		}
	}

	// Token: 0x060054F6 RID: 21750 RVA: 0x001EF4A0 File Offset: 0x001ED8A0
	private void StateChanged()
	{
		this.SetMesh();
		this.SetColor();
	}

	// Token: 0x060054F7 RID: 21751 RVA: 0x001EF4B0 File Offset: 0x001ED8B0
	public void ResetControl()
	{
		if (this.wasInit)
		{
			this.control.localPosition = this.initialLocalPosition;
			this.control.localRotation = this.initialLocalRotation;
			if (this.onPositionChangeHandlers != null)
			{
				this.onPositionChangeHandlers(this);
			}
			if (this.onRotationChangeHandlers != null)
			{
				this.onRotationChangeHandlers(this);
			}
			if (this.onMovementHandlers != null)
			{
				this.onMovementHandlers(this);
			}
		}
	}

	// Token: 0x060054F8 RID: 21752 RVA: 0x001EF530 File Offset: 0x001ED930
	private void Move(Vector3 direction)
	{
		if (this._moveForceEnabled && this._followWhenOffRB && this.useForceWhenOff)
		{
			this.appliedForce += direction * this.forceFactor;
		}
		else if (this._moveEnabled)
		{
			Vector3 translation = direction * this.moveFactor * Time.unscaledDeltaTime;
			this.control.Translate(translation, Space.World);
			if (this.connectedJoint != null)
			{
				this._followWhenOffRB.WakeUp();
			}
			if (this.onPositionChangeHandlers != null)
			{
				this.onPositionChangeHandlers(this);
			}
			if (this.onMovementHandlers != null)
			{
				this.onMovementHandlers(this);
			}
		}
	}

	// Token: 0x060054F9 RID: 21753 RVA: 0x001EF600 File Offset: 0x001EDA00
	public void SetPositionNoForce(Vector3 position)
	{
		if (!this._moveForceEnabled && this._moveEnabled)
		{
			this.control.position = position;
			if (this.onPositionChangeHandlers != null)
			{
				this.onPositionChangeHandlers(this);
			}
			if (this.onMovementHandlers != null)
			{
				this.onMovementHandlers(this);
			}
		}
	}

	// Token: 0x060054FA RID: 21754 RVA: 0x001EF660 File Offset: 0x001EDA60
	public void SetLocalPositionNoForce(Vector3 position)
	{
		if (!this._moveForceEnabled && this._moveEnabled)
		{
			this.SetLocalPosition(position);
			if (this.onPositionChangeHandlers != null)
			{
				this.onPositionChangeHandlers(this);
			}
			if (this.onMovementHandlers != null)
			{
				this.onMovementHandlers(this);
			}
		}
	}

	// Token: 0x060054FB RID: 21755 RVA: 0x001EF6B8 File Offset: 0x001EDAB8
	public void MoveAbsoluteNoForce(Vector3 direction)
	{
		if (!this._moveForceEnabled && this._moveEnabled)
		{
			this.control.Translate(direction, Space.World);
			if (this.onPositionChangeHandlers != null)
			{
				this.onPositionChangeHandlers(this);
			}
			if (this.onMovementHandlers != null)
			{
				this.onMovementHandlers(this);
			}
		}
	}

	// Token: 0x060054FC RID: 21756 RVA: 0x001EF718 File Offset: 0x001EDB18
	public void MoveAbsoluteNoForce(float x, float y, float z)
	{
		Vector3 direction;
		direction.x = x;
		direction.y = y;
		direction.z = z;
		this.MoveAbsoluteNoForce(direction);
	}

	// Token: 0x060054FD RID: 21757 RVA: 0x001EF744 File Offset: 0x001EDB44
	public void MoveRelativeNoForce(Vector3 direction)
	{
		if (!this._moveForceEnabled && this._moveEnabled)
		{
			this.control.Translate(direction, Space.Self);
			if (this.onPositionChangeHandlers != null)
			{
				this.onPositionChangeHandlers(this);
			}
			if (this.onMovementHandlers != null)
			{
				this.onMovementHandlers(this);
			}
		}
	}

	// Token: 0x060054FE RID: 21758 RVA: 0x001EF7A4 File Offset: 0x001EDBA4
	public void MoveRelativeNoForce(float x, float y, float z)
	{
		Vector3 direction;
		direction.x = x;
		direction.y = y;
		direction.z = z;
		this.MoveRelativeNoForce(direction);
	}

	// Token: 0x060054FF RID: 21759 RVA: 0x001EF7D0 File Offset: 0x001EDBD0
	public void MoveXPositionRelativeNoForce(float f)
	{
		this.MoveRelativeNoForce(f, 0f, 0f);
	}

	// Token: 0x06005500 RID: 21760 RVA: 0x001EF7E4 File Offset: 0x001EDBE4
	public void MoveXPositionRelativeNoForce(string val)
	{
		float x;
		if (float.TryParse(val, out x))
		{
			this.MoveRelativeNoForce(x, 0f, 0f);
		}
	}

	// Token: 0x06005501 RID: 21761 RVA: 0x001EF80F File Offset: 0x001EDC0F
	public void MoveYPositionRelativeNoForce(float f)
	{
		this.MoveRelativeNoForce(0f, f, 0f);
	}

	// Token: 0x06005502 RID: 21762 RVA: 0x001EF824 File Offset: 0x001EDC24
	public void MoveYPositionRelativeNoForce(string val)
	{
		float y;
		if (float.TryParse(val, out y))
		{
			this.MoveRelativeNoForce(0f, y, 0f);
		}
	}

	// Token: 0x06005503 RID: 21763 RVA: 0x001EF84F File Offset: 0x001EDC4F
	public void MoveZPositionRelativeNoForce(float f)
	{
		this.MoveRelativeNoForce(0f, 0f, f);
	}

	// Token: 0x06005504 RID: 21764 RVA: 0x001EF864 File Offset: 0x001EDC64
	public void MoveZPositionRelativeNoForce(string val)
	{
		float z;
		if (float.TryParse(val, out z))
		{
			this.MoveRelativeNoForce(0f, 0f, z);
		}
	}

	// Token: 0x06005505 RID: 21765 RVA: 0x001EF890 File Offset: 0x001EDC90
	public void MoveTo(Vector3 pos, bool alsoMoveRB = false)
	{
		if (!this._moveForceEnabled && this._moveEnabled)
		{
			this.control.position = pos;
			if (alsoMoveRB && this.followWhenOff != null && !this._detachControl)
			{
				this.followWhenOff.position = pos;
			}
			if (this.onPositionChangeHandlers != null)
			{
				this.onPositionChangeHandlers(this);
			}
			if (this.onMovementHandlers != null)
			{
				this.onMovementHandlers(this);
			}
		}
	}

	// Token: 0x06005506 RID: 21766 RVA: 0x001EF91C File Offset: 0x001EDD1C
	public void MoveTo(float x, float y, float z)
	{
		Vector3 pos;
		pos.x = x;
		pos.y = y;
		pos.z = z;
		this.MoveTo(pos, false);
	}

	// Token: 0x06005507 RID: 21767 RVA: 0x001EF94C File Offset: 0x001EDD4C
	public void PossessMoveAndAlignTo(Transform t)
	{
		if (this._canGrabRotation)
		{
			this.AlignTo(t, true);
		}
		if (this._canGrabPosition)
		{
			if (this.possessPoint != null && this.followWhenOff != null)
			{
				Vector3 position = t.position + (this.followWhenOff.position - this.possessPoint.position);
				this.control.position = position;
				if (!this._detachControl)
				{
					this.followWhenOff.position = position;
				}
			}
			else
			{
				this.control.position = t.position;
				if (this.followWhenOff != null && !this._detachControl)
				{
					this.followWhenOff.position = t.position;
				}
			}
			if (this.onPositionChangeHandlers != null)
			{
				this.onPositionChangeHandlers(this);
			}
			if (this.onMovementHandlers != null)
			{
				this.onMovementHandlers(this);
			}
		}
	}

	// Token: 0x06005508 RID: 21768 RVA: 0x001EFA54 File Offset: 0x001EDE54
	public void SetRotationNoForce(Vector3 rotation)
	{
		if (!this._rotationForceEnabled && this._rotationEnabled)
		{
			this.control.eulerAngles = rotation;
			if (this.onRotationChangeHandlers != null)
			{
				this.onRotationChangeHandlers(this);
			}
			if (this.onMovementHandlers != null)
			{
				this.onMovementHandlers(this);
			}
		}
	}

	// Token: 0x06005509 RID: 21769 RVA: 0x001EFAB4 File Offset: 0x001EDEB4
	public void SetLocalRotationNoForce(Vector3 rotation)
	{
		if (!this._rotationForceEnabled && this._rotationEnabled)
		{
			this.SetLocalEulerAngles(rotation);
			if (this.onRotationChangeHandlers != null)
			{
				this.onRotationChangeHandlers(this);
			}
			if (this.onMovementHandlers != null)
			{
				this.onMovementHandlers(this);
			}
		}
	}

	// Token: 0x0600550A RID: 21770 RVA: 0x001EFB0C File Offset: 0x001EDF0C
	public void RotateSelfRelativeNoForce(Vector3 rotation)
	{
		if (!this._rotationForceEnabled && this._rotationEnabled)
		{
			this.control.Rotate(rotation, Space.Self);
			if (this.onRotationChangeHandlers != null)
			{
				this.onRotationChangeHandlers(this);
			}
			if (this.onMovementHandlers != null)
			{
				this.onMovementHandlers(this);
			}
		}
	}

	// Token: 0x0600550B RID: 21771 RVA: 0x001EFB6A File Offset: 0x001EDF6A
	public void RotateXSelfRelativeNoForce(float f)
	{
		this.RotateSelfRelativeNoForce(new Vector3(f, 0f, 0f));
	}

	// Token: 0x0600550C RID: 21772 RVA: 0x001EFB84 File Offset: 0x001EDF84
	public void RotateXSelfRelativeNoForce(string val)
	{
		float x;
		if (float.TryParse(val, out x))
		{
			this.RotateSelfRelativeNoForce(new Vector3(x, 0f, 0f));
		}
	}

	// Token: 0x0600550D RID: 21773 RVA: 0x001EFBB4 File Offset: 0x001EDFB4
	public void RotateYSelfRelativeNoForce(float f)
	{
		this.RotateSelfRelativeNoForce(new Vector3(0f, f, 0f));
	}

	// Token: 0x0600550E RID: 21774 RVA: 0x001EFBCC File Offset: 0x001EDFCC
	public void RotateYSelfRelativeNoForce(string val)
	{
		float y;
		if (float.TryParse(val, out y))
		{
			this.RotateSelfRelativeNoForce(new Vector3(0f, y, 0f));
		}
	}

	// Token: 0x0600550F RID: 21775 RVA: 0x001EFBFC File Offset: 0x001EDFFC
	public void RotateZSelfRelativeNoForce(float f)
	{
		this.RotateSelfRelativeNoForce(new Vector3(0f, 0f, f));
	}

	// Token: 0x06005510 RID: 21776 RVA: 0x001EFC14 File Offset: 0x001EE014
	public void RotateZSelfRelativeNoForce(string val)
	{
		float z;
		if (float.TryParse(val, out z))
		{
			this.RotateSelfRelativeNoForce(new Vector3(0f, 0f, z));
		}
	}

	// Token: 0x06005511 RID: 21777 RVA: 0x001EFC44 File Offset: 0x001EE044
	public void RotateTo(Quaternion q)
	{
		if (!this._rotationForceEnabled && this._rotationEnabled)
		{
			this.control.rotation = q;
			if (this.onRotationChangeHandlers != null)
			{
				this.onRotationChangeHandlers(this);
			}
			if (this.onMovementHandlers != null)
			{
				this.onMovementHandlers(this);
			}
		}
	}

	// Token: 0x06005512 RID: 21778 RVA: 0x001EFCA4 File Offset: 0x001EE0A4
	public Vector3 GetForwardPossessAxis()
	{
		Vector3 result = Vector3.forward;
		switch (this.PossessForwardAxis)
		{
		case FreeControllerV3.DrawAxisnames.X:
			result = base.transform.right;
			break;
		case FreeControllerV3.DrawAxisnames.Y:
			result = base.transform.up;
			break;
		case FreeControllerV3.DrawAxisnames.Z:
			result = base.transform.forward;
			break;
		case FreeControllerV3.DrawAxisnames.NegX:
			result = -base.transform.right;
			break;
		case FreeControllerV3.DrawAxisnames.NegY:
			result = -base.transform.up;
			break;
		case FreeControllerV3.DrawAxisnames.NegZ:
			result = -base.transform.forward;
			break;
		}
		return result;
	}

	// Token: 0x06005513 RID: 21779 RVA: 0x001EFD58 File Offset: 0x001EE158
	public Vector3 GetUpPossessAxis()
	{
		Vector3 result = Vector3.up;
		switch (this.PossessUpAxis)
		{
		case FreeControllerV3.DrawAxisnames.X:
			result = base.transform.right;
			break;
		case FreeControllerV3.DrawAxisnames.Y:
			result = base.transform.up;
			break;
		case FreeControllerV3.DrawAxisnames.Z:
			result = base.transform.forward;
			break;
		case FreeControllerV3.DrawAxisnames.NegX:
			result = -base.transform.right;
			break;
		case FreeControllerV3.DrawAxisnames.NegY:
			result = -base.transform.up;
			break;
		case FreeControllerV3.DrawAxisnames.NegZ:
			result = -base.transform.forward;
			break;
		}
		return result;
	}

	// Token: 0x06005514 RID: 21780 RVA: 0x001EFE0C File Offset: 0x001EE20C
	public void AlignTo(Transform t, bool alsoRotateRB = false)
	{
		Quaternion rotation = this.control.rotation;
		Vector3 view = Vector3.forward;
		Vector3 up = Vector3.up;
		switch (this.PossessForwardAxis)
		{
		case FreeControllerV3.DrawAxisnames.X:
			view = t.right;
			break;
		case FreeControllerV3.DrawAxisnames.Y:
			view = t.up;
			break;
		case FreeControllerV3.DrawAxisnames.Z:
			view = t.forward;
			break;
		case FreeControllerV3.DrawAxisnames.NegX:
			view = -t.right;
			break;
		case FreeControllerV3.DrawAxisnames.NegY:
			view = -t.up;
			break;
		case FreeControllerV3.DrawAxisnames.NegZ:
			view = -t.forward;
			break;
		}
		switch (this.PossessUpAxis)
		{
		case FreeControllerV3.DrawAxisnames.X:
			up = t.right;
			break;
		case FreeControllerV3.DrawAxisnames.Y:
			up = t.up;
			break;
		case FreeControllerV3.DrawAxisnames.Z:
			up = t.forward;
			break;
		case FreeControllerV3.DrawAxisnames.NegX:
			up = -t.right;
			break;
		case FreeControllerV3.DrawAxisnames.NegY:
			up = -t.up;
			break;
		case FreeControllerV3.DrawAxisnames.NegZ:
			up = -t.forward;
			break;
		}
		rotation.SetLookRotation(view, up);
		this.control.rotation = rotation;
		if (alsoRotateRB && this.followWhenOff != null && !this._detachControl)
		{
			this.followWhenOff.rotation = rotation;
		}
		if (this.onRotationChangeHandlers != null)
		{
			this.onRotationChangeHandlers(this);
		}
		if (this.onMovementHandlers != null)
		{
			this.onMovementHandlers(this);
		}
	}

	// Token: 0x06005515 RID: 21781 RVA: 0x001EFFA8 File Offset: 0x001EE3A8
	public void RotateX(float val)
	{
		if (this._rotationForceEnabled && this._followWhenOffRB && this.useForceWhenOff)
		{
			this.appliedTorque.x = val * this.torqueFactor;
		}
		else if (this._rotationEnabled)
		{
			this.control.Rotate(new Vector3(val * this.rotateFactor * Time.unscaledDeltaTime, 0f, 0f));
			if (this.connectedJoint != null)
			{
				this._followWhenOffRB.WakeUp();
			}
			if (this.onRotationChangeHandlers != null)
			{
				this.onRotationChangeHandlers(this);
			}
			if (this.onMovementHandlers != null)
			{
				this.onMovementHandlers(this);
			}
		}
	}

	// Token: 0x06005516 RID: 21782 RVA: 0x001F0070 File Offset: 0x001EE470
	public void RotateY(float val)
	{
		if (this._rotationForceEnabled && this._followWhenOffRB && this.useForceWhenOff)
		{
			this.appliedTorque.y = val * this.torqueFactor;
		}
		else if (this._rotationEnabled)
		{
			this.control.Rotate(new Vector3(0f, val * this.rotateFactor * Time.unscaledDeltaTime, 0f));
			if (this.connectedJoint != null)
			{
				this._followWhenOffRB.WakeUp();
			}
			if (this.onRotationChangeHandlers != null)
			{
				this.onRotationChangeHandlers(this);
			}
			if (this.onMovementHandlers != null)
			{
				this.onMovementHandlers(this);
			}
		}
	}

	// Token: 0x06005517 RID: 21783 RVA: 0x001F0138 File Offset: 0x001EE538
	public void RotateZ(float val)
	{
		if (this._rotationForceEnabled && this._followWhenOffRB && this.useForceWhenOff)
		{
			this.appliedTorque.z = val * this.torqueFactor;
		}
		else if (this._rotationEnabled)
		{
			this.control.Rotate(new Vector3(0f, 0f, val * this.rotateFactor * Time.unscaledDeltaTime));
			if (this.connectedJoint != null)
			{
				this._followWhenOffRB.WakeUp();
			}
			if (this.onRotationChangeHandlers != null)
			{
				this.onRotationChangeHandlers(this);
			}
			if (this.onMovementHandlers != null)
			{
				this.onMovementHandlers(this);
			}
		}
	}

	// Token: 0x06005518 RID: 21784 RVA: 0x001F0200 File Offset: 0x001EE600
	public void RotateWorldX(float val, bool absolute = false)
	{
		if (!this._rotationForceEnabled && this._rotationEnabled)
		{
			float num = val;
			if (!absolute)
			{
				num *= this.rotateFactor * Time.unscaledDeltaTime;
			}
			this.control.Rotate(num, 0f, 0f, Space.World);
			if (this.onRotationChangeHandlers != null)
			{
				this.onRotationChangeHandlers(this);
			}
			if (this.onMovementHandlers != null)
			{
				this.onMovementHandlers(this);
			}
		}
	}

	// Token: 0x06005519 RID: 21785 RVA: 0x001F0280 File Offset: 0x001EE680
	public void RotateWorldY(float val, bool absolute = false)
	{
		if (!this._rotationForceEnabled && this._rotationEnabled)
		{
			float num = val;
			if (!absolute)
			{
				num *= this.rotateFactor * Time.unscaledDeltaTime;
			}
			this.control.Rotate(0f, num, 0f, Space.World);
			if (this.onRotationChangeHandlers != null)
			{
				this.onRotationChangeHandlers(this);
			}
			if (this.onMovementHandlers != null)
			{
				this.onMovementHandlers(this);
			}
		}
	}

	// Token: 0x0600551A RID: 21786 RVA: 0x001F0300 File Offset: 0x001EE700
	public void RotateWorldZ(float val, bool absolute = false)
	{
		if (!this._rotationForceEnabled && this._rotationEnabled)
		{
			float num = val;
			if (!absolute)
			{
				num *= this.rotateFactor * Time.unscaledDeltaTime;
			}
			this.control.Rotate(0f, 0f, num, Space.World);
			if (this.onRotationChangeHandlers != null)
			{
				this.onRotationChangeHandlers(this);
			}
			if (this.onMovementHandlers != null)
			{
				this.onMovementHandlers(this);
			}
		}
	}

	// Token: 0x0600551B RID: 21787 RVA: 0x001F0380 File Offset: 0x001EE780
	public void ResetAppliedForces()
	{
		this.appliedForce.x = 0f;
		this.appliedForce.y = 0f;
		this.appliedForce.z = 0f;
		this.appliedTorque.x = 0f;
		this.appliedTorque.y = 0f;
		this.appliedTorque.z = 0f;
	}

	// Token: 0x0600551C RID: 21788 RVA: 0x001F03F0 File Offset: 0x001EE7F0
	public void MoveAxis(FreeControllerV3.MoveAxisnames man, float val)
	{
		if (man == FreeControllerV3.MoveAxisnames.X)
		{
			this.Move(new Vector3(val, 0f, 0f));
		}
		else if (man == FreeControllerV3.MoveAxisnames.Y)
		{
			this.Move(new Vector3(0f, val, 0f));
		}
		else if (man == FreeControllerV3.MoveAxisnames.Z)
		{
			this.Move(new Vector3(0f, 0f, val));
		}
		else if (man == FreeControllerV3.MoveAxisnames.CameraRight)
		{
			Vector3 direction = Camera.main.transform.right * val;
			this.Move(direction);
		}
		else if (man == FreeControllerV3.MoveAxisnames.CameraRightNoY)
		{
			Vector3 direction2 = Camera.main.transform.right * val;
			direction2.y = 0f;
			this.Move(direction2);
		}
		else if (man == FreeControllerV3.MoveAxisnames.CameraForward)
		{
			Vector3 direction3 = Camera.main.transform.forward * val;
			this.Move(direction3);
		}
		else if (man == FreeControllerV3.MoveAxisnames.CameraForwardNoY)
		{
			Vector3 direction4 = Camera.main.transform.forward * val;
			direction4.y = 0f;
			this.Move(direction4);
		}
		else if (man == FreeControllerV3.MoveAxisnames.CameraUp)
		{
			Vector3 direction5 = Camera.main.transform.up * val;
			this.Move(direction5);
		}
	}

	// Token: 0x0600551D RID: 21789 RVA: 0x001F0544 File Offset: 0x001EE944
	public void RotateAxis(FreeControllerV3.RotateAxisnames ran, float val)
	{
		if (ran == FreeControllerV3.RotateAxisnames.X)
		{
			this.RotateX(val);
		}
		else if (ran == FreeControllerV3.RotateAxisnames.NegX)
		{
			this.RotateX(-val);
		}
		else if (ran == FreeControllerV3.RotateAxisnames.Y)
		{
			this.RotateY(val);
		}
		else if (ran == FreeControllerV3.RotateAxisnames.NegY)
		{
			this.RotateY(-val);
		}
		else if (ran == FreeControllerV3.RotateAxisnames.Z)
		{
			this.RotateZ(val);
		}
		else if (ran == FreeControllerV3.RotateAxisnames.NegZ)
		{
			this.RotateZ(-val);
		}
		else if (ran == FreeControllerV3.RotateAxisnames.WorldY)
		{
			this.RotateWorldY(val, false);
		}
	}

	// Token: 0x0600551E RID: 21790 RVA: 0x001F05D4 File Offset: 0x001EE9D4
	public void ControlAxis1(float val)
	{
		if (this.controlMode == FreeControllerV3.ControlMode.Rotation)
		{
			this.RotateAxis(this.RotateAxis1, val);
		}
		else if (this.controlMode == FreeControllerV3.ControlMode.Position)
		{
			this.MoveAxis(this.MoveAxis1, val);
		}
	}

	// Token: 0x0600551F RID: 21791 RVA: 0x001F060D File Offset: 0x001EEA0D
	public void ControlAxis2(float val)
	{
		if (this.controlMode == FreeControllerV3.ControlMode.Rotation)
		{
			this.RotateAxis(this.RotateAxis2, val);
		}
		else if (this.controlMode == FreeControllerV3.ControlMode.Position)
		{
			this.MoveAxis(this.MoveAxis2, val);
		}
	}

	// Token: 0x06005520 RID: 21792 RVA: 0x001F0646 File Offset: 0x001EEA46
	public void ControlAxis3(float val)
	{
		if (this.controlMode == FreeControllerV3.ControlMode.Rotation)
		{
			this.RotateAxis(this.RotateAxis3, val);
		}
		else if (this.controlMode == FreeControllerV3.ControlMode.Position)
		{
			this.MoveAxis(this.MoveAxis3, val);
		}
	}

	// Token: 0x06005521 RID: 21793 RVA: 0x001F0680 File Offset: 0x001EEA80
	private void Init()
	{
		if (!this.wasInit)
		{
			this.wasInit = true;
			if (this.control == null)
			{
				this.control = base.transform;
			}
			if (this.linkLineMaterial)
			{
				this.linkLineMaterialLocal = UnityEngine.Object.Instantiate<Material>(this.linkLineMaterial);
				this.linkLineDrawer = new LineDrawer(this.linkLineMaterialLocal);
				this.RegisterAllocatedObject(this.linkLineMaterialLocal);
			}
			if (this.useContainedMeshRenderers)
			{
				this.mrs = base.GetComponentsInChildren<MeshRenderer>();
			}
			if (this.material)
			{
				this.positionMaterialLocal = UnityEngine.Object.Instantiate<Material>(this.material);
				this.RegisterAllocatedObject(this.positionMaterialLocal);
				this.rotationMaterialLocal = UnityEngine.Object.Instantiate<Material>(this.material);
				this.RegisterAllocatedObject(this.rotationMaterialLocal);
				this.snapshotMaterialLocal = UnityEngine.Object.Instantiate<Material>(this.material);
				this.RegisterAllocatedObject(this.snapshotMaterialLocal);
				this.materialOverlay = UnityEngine.Object.Instantiate<Material>(this.material);
				this.RegisterAllocatedObject(this.materialOverlay);
			}
			if (this.followWhenOff)
			{
				this._followWhenOffRB = this.followWhenOff.GetComponent<Rigidbody>();
				this.detachControlJSON = new JSONStorableBool("detachControl", this._detachControl, new JSONStorableBool.SetBoolCallback(this.SyncDetachControl));
				this.detachControlJSON.storeType = JSONStorableParam.StoreType.Physical;
				base.RegisterBool(this.detachControlJSON);
			}
			this.kinematicRB = base.GetComponent<Rigidbody>();
			if (this.kinematicRB != null && this.followWhenOff)
			{
				this.control.position = this.followWhenOff.position;
				this.control.rotation = this.followWhenOff.rotation;
				ConfigurableJoint[] components = this.followWhenOff.GetComponents<ConfigurableJoint>();
				foreach (ConfigurableJoint configurableJoint in components)
				{
					if (configurableJoint.connectedBody == this.kinematicRB)
					{
						this.connectedJoint = configurableJoint;
						configurableJoint.connectedAnchor = Vector3.zero;
						this.SetJointSprings();
					}
					else
					{
						this.naturalJoint = configurableJoint;
						JointDrive slerpDrive = this.naturalJoint.slerpDrive;
						this._jointRotationDriveSpring = slerpDrive.positionSpring;
						this._jointRotationDriveDamper = slerpDrive.positionDamper;
						this._jointRotationDriveMaxForce = slerpDrive.maximumForce;
						Vector3 eulerAngles = this.naturalJoint.targetRotation.eulerAngles;
						if (eulerAngles.x > 180f)
						{
							eulerAngles.x -= 360f;
						}
						else if (eulerAngles.x < -180f)
						{
							eulerAngles.x += 360f;
						}
						if (eulerAngles.y > 180f)
						{
							eulerAngles.y -= 360f;
						}
						else if (eulerAngles.y < -180f)
						{
							eulerAngles.y += 360f;
						}
						if (eulerAngles.z > 180f)
						{
							eulerAngles.z -= 360f;
						}
						else if (eulerAngles.z < -180f)
						{
							eulerAngles.z += 360f;
						}
						this._jointRotationDriveXTarget = eulerAngles.x;
						this._jointRotationDriveYTarget = eulerAngles.y;
						this._jointRotationDriveZTarget = eulerAngles.z;
						if (this.naturalJoint.lowAngularXLimit.limit < this.naturalJoint.highAngularXLimit.limit)
						{
							this._jointRotationDriveXTargetMin = this.naturalJoint.lowAngularXLimit.limit;
							this._jointRotationDriveXTargetMax = this.naturalJoint.highAngularXLimit.limit;
						}
						else
						{
							this._jointRotationDriveXTargetMin = this.naturalJoint.highAngularXLimit.limit;
							this._jointRotationDriveXTargetMax = this.naturalJoint.lowAngularXLimit.limit;
						}
						this._jointRotationDriveYTargetMin = -this.naturalJoint.angularYLimit.limit;
						this._jointRotationDriveYTargetMax = this.naturalJoint.angularYLimit.limit;
						this._jointRotationDriveZTargetMin = -this.naturalJoint.angularZLimit.limit;
						this._jointRotationDriveZTargetMax = this.naturalJoint.angularZLimit.limit;
					}
				}
			}
			this.startingPosition = base.transform.position;
			this.startingRotation = base.transform.rotation;
			this.startingLocalPosition = base.transform.localPosition;
			this.startingLocalRotation = base.transform.localRotation;
			if (this.stateCanBeModified)
			{
				string[] names = Enum.GetNames(typeof(FreeControllerV3.PositionState));
				List<string> choicesList = new List<string>(names);
				this.currentPositionStateJSON = new JSONStorableStringChooser("positionState", choicesList, this.startingPositionState.ToString(), "Position State", new JSONStorableStringChooser.SetStringCallback(this.SetPositionStateFromString));
				this.currentPositionStateJSON.storeType = JSONStorableParam.StoreType.Physical;
				base.RegisterStringChooser(this.currentPositionStateJSON);
				string[] names2 = Enum.GetNames(typeof(FreeControllerV3.RotationState));
				List<string> choicesList2 = new List<string>(names2);
				this.currentRotationStateJSON = new JSONStorableStringChooser("rotationState", choicesList2, this.startingRotationState.ToString(), "Rotation State", new JSONStorableStringChooser.SetStringCallback(this.SetRotationStateFromString));
				this.currentRotationStateJSON.storeType = JSONStorableParam.StoreType.Physical;
				base.RegisterStringChooser(this.currentRotationStateJSON);
				this.complyPositionThresholdJSON = new JSONStorableFloat("complyPositionThreshold", this.complyPositionThreshold, new JSONStorableFloat.SetFloatCallback(this.SyncComplyPositionThreshold), 0.0001f, 0.1f, true, true);
				this.complyPositionThresholdJSON.storeType = JSONStorableParam.StoreType.Physical;
				base.RegisterFloat(this.complyPositionThresholdJSON);
				this.complyRotationThresholdJSON = new JSONStorableFloat("complyRotationThreshold", this.complyRotationThreshold, new JSONStorableFloat.SetFloatCallback(this.SyncComplyRotationThreshold), 0.1f, 30f, true, true);
				this.complyRotationThresholdJSON.storeType = JSONStorableParam.StoreType.Physical;
				base.RegisterFloat(this.complyRotationThresholdJSON);
				this.complySpeedJSON = new JSONStorableFloat("complySpeed", this.complySpeed, new JSONStorableFloat.SetFloatCallback(this.SyncComplySpeed), 0f, 100f, true, true);
				this.complySpeedJSON.storeType = JSONStorableParam.StoreType.Physical;
				base.RegisterFloat(this.complySpeedJSON);
			}
			if (this.controlsOn)
			{
				this.onJSON = new JSONStorableBool("on", this._on, new JSONStorableBool.SetBoolCallback(this.SyncOn));
				this.onJSON.storeType = JSONStorableParam.StoreType.Physical;
				base.RegisterBool(this.onJSON);
				this.SyncOn(this._on);
			}
			this.resetAction = new JSONStorableAction("Reset", new JSONStorableAction.ActionCallback(this.Reset));
			base.RegisterAction(this.resetAction);
			this.interactableInPlayModeJSON = new JSONStorableBool("interactableInPlayMode", this._interactableInPlayMode, new JSONStorableBool.SetBoolCallback(this.SyncInteractableInPlayMode));
			this.interactableInPlayModeJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterBool(this.interactableInPlayModeJSON);
			this.deactivateOtherControlsOnPossessJSON = new JSONStorableBool("deactivateOtherControlsOnPossess", this._deactivateOtherControlsOnPossess, new JSONStorableBool.SetBoolCallback(this.SyncDeactivateOtherControlsOnPossess));
			this.deactivateOtherControlsOnPossessJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterBool(this.deactivateOtherControlsOnPossessJSON);
			this.possessableJSON = new JSONStorableBool("possessable", this._possessable, new JSONStorableBool.SetBoolCallback(this.SyncPossessable));
			this.possessableJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterBool(this.possessableJSON);
			this.canGrabPositionJSON = new JSONStorableBool("canGrabPosition", this._canGrabPosition, new JSONStorableBool.SetBoolCallback(this.SyncCanGrabPosition));
			this.canGrabPositionJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterBool(this.canGrabPositionJSON);
			this.canGrabRotationJSON = new JSONStorableBool("canGrabRotation", this._canGrabRotation, new JSONStorableBool.SetBoolCallback(this.SyncCanGrabRotation));
			this.canGrabRotationJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterBool(this.canGrabRotationJSON);
			this.freezeAtomPhysicsWhenGrabbedJSON = new JSONStorableBool("freezeAtomPhysicsWhenGrabbed", this.freezeAtomPhysicsWhenGrabbed, new JSONStorableBool.SetBoolCallback(this.SyncFreezeAtomPhysicsWhenGrabbed));
			this.freezeAtomPhysicsWhenGrabbedJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterBool(this.freezeAtomPhysicsWhenGrabbedJSON);
			string[] names3 = Enum.GetNames(typeof(FreeControllerV3.GridMode));
			List<string> choicesList3 = new List<string>(names3);
			this.positionGridModeJSON = new JSONStorableStringChooser("positionGridMode", choicesList3, this.positionGridMode.ToString(), "Position Grid Mode", new JSONStorableStringChooser.SetStringCallback(this.SetPositionGridModeFromString));
			this.positionGridModeJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterStringChooser(this.positionGridModeJSON);
			this.rotationGridModeJSON = new JSONStorableStringChooser("rotationGridMode", choicesList3, this.rotationGridMode.ToString(), "Rotation Grid Mode", new JSONStorableStringChooser.SetStringCallback(this.SetRotationGridModeFromString));
			this.rotationGridModeJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterStringChooser(this.rotationGridModeJSON);
			this.positionGridJSON = new JSONStorableFloat("positionGrid", this._positionGrid, new JSONStorableFloat.SetFloatCallback(this.SyncPositionGrid), 0.001f, 1f, true, true);
			this.positionGridJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.positionGridJSON);
			this.rotationGridJSON = new JSONStorableFloat("rotationGrid", this._rotationGrid, new JSONStorableFloat.SetFloatCallback(this.SyncRotationGrid), 0.01f, 90f, true, true);
			this.rotationGridJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.rotationGridJSON);
			this.xLockJSON = new JSONStorableBool("xPositionLock", this._xLock, new JSONStorableBool.SetBoolCallback(this.SyncXLock));
			this.xLockJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterBool(this.xLockJSON);
			this.yLockJSON = new JSONStorableBool("yPositionLock", this._yLock, new JSONStorableBool.SetBoolCallback(this.SyncYLock));
			this.yLockJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterBool(this.yLockJSON);
			this.zLockJSON = new JSONStorableBool("zPositionLock", this._zLock, new JSONStorableBool.SetBoolCallback(this.SyncZLock));
			this.zLockJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterBool(this.zLockJSON);
			this.xLocalLockJSON = new JSONStorableBool("xPositionLocalLock", this._xLocalLock, new JSONStorableBool.SetBoolCallback(this.SyncXLocalLock));
			this.xLocalLockJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterBool(this.xLocalLockJSON);
			this.yLocalLockJSON = new JSONStorableBool("yPositionLocalLock", this._yLocalLock, new JSONStorableBool.SetBoolCallback(this.SyncYLocalLock));
			this.yLocalLockJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterBool(this.yLocalLockJSON);
			this.zLocalLockJSON = new JSONStorableBool("zPositionLocalLock", this._zLocalLock, new JSONStorableBool.SetBoolCallback(this.SyncZLocalLock));
			this.zLocalLockJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterBool(this.zLocalLockJSON);
			this.xRotLockJSON = new JSONStorableBool("xRotationLock", this._xRotLock, new JSONStorableBool.SetBoolCallback(this.SyncXRotLock));
			this.xRotLockJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterBool(this.xRotLockJSON);
			this.yRotLockJSON = new JSONStorableBool("yRotationLock", this._yRotLock, new JSONStorableBool.SetBoolCallback(this.SyncYRotLock));
			this.yRotLockJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterBool(this.yRotLockJSON);
			this.zRotLockJSON = new JSONStorableBool("zRotationLock", this._zRotLock, new JSONStorableBool.SetBoolCallback(this.SyncZRotLock));
			this.zRotLockJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterBool(this.zRotLockJSON);
			if (this.controlsCollisionEnabled)
			{
				if (this._followWhenOffRB != null)
				{
					this._collisionEnabled = this._followWhenOffRB.detectCollisions;
				}
				this.collisionEnabledJSON = new JSONStorableBool("collisionEnabled", this._collisionEnabled, new JSONStorableBool.SetBoolCallback(this.SyncCollisionEnabled));
				this.collisionEnabledJSON.storeType = JSONStorableParam.StoreType.Physical;
				base.RegisterBool(this.collisionEnabledJSON);
			}
			this.physicsEnabledJSON = new JSONStorableBool("physicsEnabled", this.physicsEnabled, new JSONStorableBool.SetBoolCallback(this.SyncPhysicsEnabled));
			this.physicsEnabledJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterBool(this.physicsEnabledJSON);
			this.useGravityJSON = new JSONStorableBool("useGravity", this.useGravityOnRBWhenOff, new JSONStorableBool.SetBoolCallback(this.SyncUseGravityOnRBWhenOff));
			this.useGravityJSON.altName = "useGravityWhenOff";
			this.useGravityJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterBool(this.useGravityJSON);
			this.RBMassJSON = new JSONStorableFloat("mass", this.RBMass, new JSONStorableFloat.SetFloatCallback(this.SyncRBMass), 0.01f, 10f, true, true);
			this.RBMassJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.RBMassJSON);
			this.RBDragJSON = new JSONStorableFloat("drag", this.RBDrag, new JSONStorableFloat.SetFloatCallback(this.SyncRBDrag), 0f, 10f, false, true);
			this.RBDragJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.RBDragJSON);
			this.RBMaxVelocityEnableJSON = new JSONStorableBool("maxVelocityEnable", this._RBMaxVelocityEnable, new JSONStorableBool.SetBoolCallback(this.SyncRBMaxVelocityEnable));
			this.RBMaxVelocityEnableJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterBool(this.RBMaxVelocityEnableJSON);
			this.RBMaxVelocityJSON = new JSONStorableFloat("maxVelocity", this._RBMaxVelocity, new JSONStorableFloat.SetFloatCallback(this.SyncRBMaxVelocity), 0f, 100f, true, true);
			this.RBMaxVelocityJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.RBMaxVelocityJSON);
			this.RBAngularDragJSON = new JSONStorableFloat("angularDrag", this.RBAngularDrag, new JSONStorableFloat.SetFloatCallback(this.SyncRBAngularDrag), 0f, 10f, false, true);
			this.RBAngularDragJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.RBAngularDragJSON);
			this.RBHoldPositionSpringJSON = new JSONStorableFloat("holdPositionSpring", this._RBHoldPositionSpring, new JSONStorableFloat.SetFloatCallback(this.SyncRBHoldPositionSpring), 0f, 10000f, false, true);
			this.RBHoldPositionSpringJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.RBHoldPositionSpringJSON);
			this.RBHoldPositionDamperJSON = new JSONStorableFloat("holdPositionDamper", this._RBHoldPositionDamper, new JSONStorableFloat.SetFloatCallback(this.SyncRBHoldPositionDamper), 0f, 100f, false, true);
			this.RBHoldPositionDamperJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.RBHoldPositionDamperJSON);
			this.RBHoldPositionMaxForceJSON = new JSONStorableFloat("holdPositionMaxForce", this._RBHoldPositionMaxForce, new JSONStorableFloat.SetFloatCallback(this.SyncRBHoldPositionMaxForce), 0f, 10000f, false, true);
			this.RBHoldPositionMaxForceJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.RBHoldPositionMaxForceJSON);
			this.RBHoldRotationSpringJSON = new JSONStorableFloat("holdRotationSpring", this._RBHoldRotationSpring, new JSONStorableFloat.SetFloatCallback(this.SyncRBHoldRotationSpring), 0f, 1000f, false, true);
			this.RBHoldRotationSpringJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.RBHoldRotationSpringJSON);
			this.RBHoldRotationDamperJSON = new JSONStorableFloat("holdRotationDamper", this._RBHoldRotationDamper, new JSONStorableFloat.SetFloatCallback(this.SyncRBHoldRotationDamper), 0f, 10f, false, true);
			this.RBHoldRotationDamperJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.RBHoldRotationDamperJSON);
			this.RBHoldRotationMaxForceJSON = new JSONStorableFloat("holdRotationMaxForce", this._RBHoldRotationMaxForce, new JSONStorableFloat.SetFloatCallback(this.SyncRBHoldRotationMaxForce), 0f, 1000f, false, true);
			this.RBHoldRotationMaxForceJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.RBHoldRotationMaxForceJSON);
			this.RBComplyPositionSpringJSON = new JSONStorableFloat("complyPositionSpring", this._RBComplyPositionSpring, new JSONStorableFloat.SetFloatCallback(this.SyncRBComplyPositionSpring), 0f, 10000f, false, true);
			this.RBComplyPositionSpringJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.RBComplyPositionSpringJSON);
			this.RBComplyPositionDamperJSON = new JSONStorableFloat("complyPositionDamper", this._RBComplyPositionDamper, new JSONStorableFloat.SetFloatCallback(this.SyncRBComplyPositionDamper), 0f, 1000f, false, true);
			this.RBComplyPositionDamperJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.RBComplyPositionDamperJSON);
			this.RBComplyRotationSpringJSON = new JSONStorableFloat("complyRotationSpring", this._RBComplyRotationSpring, new JSONStorableFloat.SetFloatCallback(this.SyncRBComplyRotationSpring), 0f, 1000f, false, true);
			this.RBComplyRotationSpringJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.RBComplyRotationSpringJSON);
			this.RBComplyRotationDamperJSON = new JSONStorableFloat("complyRotationDamper", this._RBComplyRotationDamper, new JSONStorableFloat.SetFloatCallback(this.SyncRBComplyRotationDamper), 0f, 100f, false, true);
			this.RBComplyRotationDamperJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.RBComplyRotationDamperJSON);
			this.RBLinkPositionSpringJSON = new JSONStorableFloat("linkPositionSpring", this._RBLinkPositionSpring, new JSONStorableFloat.SetFloatCallback(this.SyncRBLinkPositionSpring), 0f, 100000f, false, true);
			this.RBLinkPositionSpringJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.RBLinkPositionSpringJSON);
			this.RBLinkPositionDamperJSON = new JSONStorableFloat("linkPositionDamper", this._RBLinkPositionDamper, new JSONStorableFloat.SetFloatCallback(this.SyncRBLinkPositionDamper), 0f, 1000f, false, true);
			this.RBLinkPositionDamperJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.RBLinkPositionDamperJSON);
			this.RBLinkPositionMaxForceJSON = new JSONStorableFloat("linkPositionMaxForce", this._RBLinkPositionMaxForce, new JSONStorableFloat.SetFloatCallback(this.SyncRBLinkPositionMaxForce), 0f, 100000f, false, true);
			this.RBLinkPositionMaxForceJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.RBLinkPositionMaxForceJSON);
			this.RBLinkRotationSpringJSON = new JSONStorableFloat("linkRotationSpring", this._RBLinkRotationSpring, new JSONStorableFloat.SetFloatCallback(this.SyncRBLinkRotationSpring), 0f, 100000f, false, true);
			this.RBLinkRotationSpringJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.RBLinkRotationSpringJSON);
			this.RBLinkRotationDamperJSON = new JSONStorableFloat("linkRotationDamper", this._RBLinkRotationDamper, new JSONStorableFloat.SetFloatCallback(this.SyncRBLinkRotationDamper), 0f, 1000f, false, true);
			this.RBLinkRotationDamperJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.RBLinkRotationDamperJSON);
			this.RBLinkRotationMaxForceJSON = new JSONStorableFloat("linkRotationMaxForce", this._RBLinkRotationMaxForce, new JSONStorableFloat.SetFloatCallback(this.SyncRBLinkRotationMaxForce), 0f, 100000f, false, true);
			this.RBLinkRotationMaxForceJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.RBLinkRotationMaxForceJSON);
			this.RBComplyJointRotationDriveSpringJSON = new JSONStorableFloat("complyJointDriveSpring", this._RBComplyJointRotationDriveSpring, new JSONStorableFloat.SetFloatCallback(this.SyncRBComplyJointRotationDriveSpring), 0f, 100f, false, true);
			this.RBComplyJointRotationDriveSpringJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.RBComplyJointRotationDriveSpringJSON);
			this.jointRotationDriveSpringJSON = new JSONStorableFloat("jointDriveSpring", this._jointRotationDriveSpring, new JSONStorableFloat.SetFloatCallback(this.SyncJointRotationDriveSpring), 0f, 200f, false, true);
			this.jointRotationDriveSpringJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.jointRotationDriveSpringJSON);
			this.jointRotationDriveDamperJSON = new JSONStorableFloat("jointDriveDamper", this._jointRotationDriveDamper, new JSONStorableFloat.SetFloatCallback(this.SyncJointRotationDriveDamper), 0f, 10f, false, true);
			this.jointRotationDriveDamperJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.jointRotationDriveDamperJSON);
			this.jointRotationDriveMaxForceJSON = new JSONStorableFloat("jointDriveMaxForce", this._jointRotationDriveMaxForce, new JSONStorableFloat.SetFloatCallback(this.SyncJointRotationDriveMaxForce), 0f, 100f, false, true);
			this.jointRotationDriveMaxForceJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.jointRotationDriveMaxForceJSON);
			this.jointRotationDriveXTargetJSON = new JSONStorableFloat("jointDriveXTarget", this._jointRotationDriveXTarget, new JSONStorableFloat.SetFloatCallback(this.SyncJointRotationDriveXTarget), this._jointRotationDriveXTargetMin, this._jointRotationDriveXTargetMax, true, true);
			this.jointRotationDriveXTargetJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.jointRotationDriveXTargetJSON);
			this.jointRotationDriveYTargetJSON = new JSONStorableFloat("jointDriveYTarget", this._jointRotationDriveYTarget, new JSONStorableFloat.SetFloatCallback(this.SyncJointRotationDriveYTarget), this._jointRotationDriveYTargetMin, this._jointRotationDriveYTargetMax, true, true);
			this.jointRotationDriveYTargetJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.jointRotationDriveYTargetJSON);
			this.jointRotationDriveZTargetJSON = new JSONStorableFloat("jointDriveZTarget", this._jointRotationDriveZTarget, new JSONStorableFloat.SetFloatCallback(this.SyncJointRotationDriveZTarget), this._jointRotationDriveZTargetMin, this._jointRotationDriveZTargetMax, true, true);
			this.jointRotationDriveZTargetJSON.storeType = JSONStorableParam.StoreType.Physical;
			base.RegisterFloat(this.jointRotationDriveZTargetJSON);
			this.initialLocalPosition = this.control.localPosition;
			this.initialLocalRotation = this.control.localRotation;
			this._currentPositionState = this.startingPositionState;
			this.SyncPositionState();
			this._currentRotationState = this.startingRotationState;
			this.SyncRotationState();
			if (this.startingLinkToRigidbody != null)
			{
				this.SelectLinkToRigidbody(this.startingLinkToRigidbody, FreeControllerV3.SelectLinkState.PositionAndRotation, false, false);
			}
			if (SuperController.singleton != null)
			{
				SuperController singleton = SuperController.singleton;
				singleton.onAtomUIDRenameHandlers = (SuperController.OnAtomUIDRename)Delegate.Combine(singleton.onAtomUIDRenameHandlers, new SuperController.OnAtomUIDRename(this.OnAtomUIDRename));
			}
		}
	}

	// Token: 0x06005522 RID: 21794 RVA: 0x001F1B48 File Offset: 0x001EFF48
	private void OnDestroy()
	{
		if (SuperController.singleton != null && this.wasInit)
		{
			SuperController singleton = SuperController.singleton;
			singleton.onAtomUIDRenameHandlers = (SuperController.OnAtomUIDRename)Delegate.Remove(singleton.onAtomUIDRenameHandlers, new SuperController.OnAtomUIDRename(this.OnAtomUIDRename));
		}
		this.DestroyAllocatedObjects();
		if (this.linkLineDrawer != null)
		{
			this.linkLineDrawer.Destroy();
		}
	}

	// Token: 0x06005523 RID: 21795 RVA: 0x001F1BB4 File Offset: 0x001EFFB4
	private void FixedUpdate()
	{
		if (this._followWhenOffRB != null)
		{
			if (this._followWhenOffRB.isKinematic)
			{
				this.UpdateTransform(false);
			}
			if (this._RBMaxVelocityEnable)
			{
				if (this._RBMaxVelocity == 0f)
				{
					this._followWhenOffRB.velocity = Vector3.zero;
				}
				else
				{
					Vector3 velocity = Vector3.ClampMagnitude(this._followWhenOffRB.velocity, this._RBMaxVelocity);
					if (float.IsNaN(velocity.x) || float.IsNaN(velocity.y) || float.IsNaN(velocity.z))
					{
						this._followWhenOffRB.velocity = Vector3.zero;
						if (this.containingAtom != null && this.containingAtom != null)
						{
							this.containingAtom.AlertPhysicsCorruption("FreeController velocity " + base.name);
						}
					}
					else
					{
						this._followWhenOffRB.velocity = velocity;
					}
				}
			}
		}
		this.ApplyComply();
		this.ApplyForce();
	}

	// Token: 0x06005524 RID: 21796 RVA: 0x001F1CD0 File Offset: 0x001F00D0
	protected override void Update()
	{
		base.Update();
		this.UpdateTransform(true);
		if ((this._currentPositionMesh || this._currentRotationMesh) && !this.hidden)
		{
			if (this.deselectedMesh != null && !this._selected && SuperController.singleton != null && SuperController.singleton.centerCameraTarget != null)
			{
				if (this.drawMeshWhenDeselected)
				{
					Transform transform = SuperController.singleton.centerCameraTarget.transform;
					Vector3 forward;
					if (transform.position == base.transform.position)
					{
						forward = transform.forward;
					}
					else
					{
						forward = transform.position - base.transform.position;
					}
					Vector3 up = transform.up;
					Quaternion q = Quaternion.LookRotation(forward, up);
					Vector3 s = new Vector3(this.deselectedMeshScale, this.deselectedMeshScale, this.deselectedMeshScale);
					Matrix4x4 identity = Matrix4x4.identity;
					identity.SetTRS(base.transform.position, q, s);
					this.positionMaterialLocal.SetFloat("_Alpha", FreeControllerV3.targetAlpha);
					this.positionMaterialLocal.color = this._currentPositionColor;
					Graphics.DrawMesh(this.deselectedMesh, identity, this.positionMaterialLocal, base.gameObject.layer, null, 0, null, false, false);
				}
			}
			else if (this.drawMesh)
			{
				Matrix4x4 localToWorldMatrix = this.control.localToWorldMatrix;
				Vector3 vectorFromAxis = this.GetVectorFromAxis(this.MeshForwardAxis);
				Vector3 vectorFromAxis2 = this.GetVectorFromAxis(this.MeshUpAxis);
				Quaternion rhs = Quaternion.LookRotation(vectorFromAxis, vectorFromAxis2);
				Vector3 vectorFromAxis3 = this.GetVectorFromAxis(this.DrawForwardAxis);
				Vector3 vectorFromAxis4 = this.GetVectorFromAxis(this.DrawUpAxis);
				Quaternion lhs = Quaternion.LookRotation(vectorFromAxis3, vectorFromAxis4);
				Quaternion q2 = lhs * rhs;
				float num = this.meshScale;
				if (this._selected)
				{
					num *= this.selectedScale;
				}
				else if (this._highlighted)
				{
					num *= this.highlightedScale;
				}
				else
				{
					num *= this.unhighlightedScale;
				}
				Vector3 s2 = new Vector3(num, num, num);
				Matrix4x4 identity2 = Matrix4x4.identity;
				identity2.SetTRS(Vector3.zero, q2, s2);
				Matrix4x4 matrix = localToWorldMatrix * identity2;
				if (this._currentPositionMesh)
				{
					this.positionMaterialLocal.SetFloat("_Alpha", FreeControllerV3.targetAlpha);
					this.positionMaterialLocal.color = this._currentPositionColor;
					Graphics.DrawMesh(this._currentPositionMesh, matrix, this.positionMaterialLocal, base.gameObject.layer, null, 0, null, false, false);
				}
				if (this._currentRotationMesh)
				{
					this.rotationMaterialLocal.SetFloat("_Alpha", FreeControllerV3.targetAlpha);
					this.rotationMaterialLocal.color = this._currentRotationColor;
					Graphics.DrawMesh(this._currentRotationMesh, matrix, this.rotationMaterialLocal, base.gameObject.layer, null, 0, null, false, false);
				}
				if (this._selected)
				{
					this.materialOverlay.SetFloat("_Alpha", FreeControllerV3.targetAlpha);
					this.materialOverlay.color = this.overlayColor;
					if (this._controlMode == FreeControllerV3.ControlMode.Position)
					{
						if (this.moveModeOverlayMesh)
						{
							Graphics.DrawMesh(this.moveModeOverlayMesh, matrix, this.materialOverlay, base.gameObject.layer, null, 0, null, false, false);
						}
					}
					else if (this._controlMode == FreeControllerV3.ControlMode.Rotation && this.rotateModeOverlayMesh)
					{
						Graphics.DrawMesh(this.rotateModeOverlayMesh, matrix, this.materialOverlay, base.gameObject.layer, null, 0, null, false, false);
					}
				}
			}
		}
		if (this.drawSnapshot)
		{
			Matrix4x4 lhs2 = this.snapshotMatrix;
			Vector3 vectorFromAxis5 = this.GetVectorFromAxis(this.MeshForwardAxis);
			Vector3 vectorFromAxis6 = this.GetVectorFromAxis(this.MeshUpAxis);
			Quaternion rhs2 = Quaternion.LookRotation(vectorFromAxis5, vectorFromAxis6);
			Vector3 vectorFromAxis7 = this.GetVectorFromAxis(this.DrawForwardAxis);
			Vector3 vectorFromAxis8 = this.GetVectorFromAxis(this.DrawUpAxis);
			Quaternion lhs3 = Quaternion.LookRotation(vectorFromAxis7, vectorFromAxis8);
			Quaternion q3 = lhs3 * rhs2;
			float num2 = this.meshScale * this.unhighlightedScale;
			Vector3 s3 = new Vector3(num2, num2, num2);
			Matrix4x4 identity3 = Matrix4x4.identity;
			identity3.SetTRS(Vector3.zero, q3, s3);
			Matrix4x4 matrix2 = lhs2 * identity3;
			this.snapshotMaterialLocal.SetFloat("_Alpha", FreeControllerV3.targetAlpha);
			this.snapshotMaterialLocal.color = this.lockColor;
			if (this._currentPositionMesh)
			{
				Graphics.DrawMesh(this._currentPositionMesh, matrix2, this.snapshotMaterialLocal, base.gameObject.layer, null, 0, null, false, false);
			}
			if (this._currentRotationMesh)
			{
				Graphics.DrawMesh(this._currentRotationMesh, matrix2, this.snapshotMaterialLocal, base.gameObject.layer, null, 0, null, false, false);
			}
		}
		if (this.linkLineDrawer != null && this._linkToRB != null && !this._hidden)
		{
			ForceReceiver component = this._linkToRB.GetComponent<ForceReceiver>();
			if (component == null || !component.skipUIDrawing)
			{
				this.linkLineMaterialLocal.SetFloat("_Alpha", FreeControllerV3.targetAlpha);
				this.linkLineDrawer.SetLinePoints(base.transform.position, this._linkToRB.transform.position);
				this.linkLineDrawer.Draw(base.gameObject.layer);
			}
		}
	}

	// Token: 0x06005525 RID: 21797 RVA: 0x001F2264 File Offset: 0x001F0664
	// Note: this type is marked as 'beforefieldinit'.
	static FreeControllerV3()
	{
	}

	// Token: 0x040043AE RID: 17326
	public static float targetAlpha = 1f;

	// Token: 0x040043AF RID: 17327
	protected List<UnityEngine.Object> allocatedObjects;

	// Token: 0x040043B0 RID: 17328
	public bool storePositionRotationAsLocal;

	// Token: 0x040043B1 RID: 17329
	protected bool _forceStorePositionRotationAsLocal;

	// Token: 0x040043B2 RID: 17330
	protected string[] customParamNames = new string[]
	{
		"localPosition",
		"localRotation",
		"position",
		"rotation",
		"linkTo"
	};

	// Token: 0x040043B3 RID: 17331
	protected JSONStorableAction resetAction;

	// Token: 0x040043B4 RID: 17332
	public bool enableSelectRoot;

	// Token: 0x040043B5 RID: 17333
	public UIPopup linkToSelectionPopup;

	// Token: 0x040043B6 RID: 17334
	public UIPopup linkToAtomSelectionPopup;

	// Token: 0x040043B7 RID: 17335
	private Rigidbody _linkToRB;

	// Token: 0x040043B8 RID: 17336
	private Transform _linkToConnector;

	// Token: 0x040043B9 RID: 17337
	private ConfigurableJoint _linkToJoint;

	// Token: 0x040043BA RID: 17338
	protected string linkToAtomUID;

	// Token: 0x040043BB RID: 17339
	private Rigidbody preLinkRB;

	// Token: 0x040043BC RID: 17340
	private FreeControllerV3.PositionState preLinkPositionState;

	// Token: 0x040043BD RID: 17341
	private FreeControllerV3.RotationState preLinkRotationState;

	// Token: 0x040043BE RID: 17342
	public Rigidbody startingLinkToRigidbody;

	// Token: 0x040043BF RID: 17343
	protected bool _isGrabbing;

	// Token: 0x040043C0 RID: 17344
	public bool stateCanBeModified = true;

	// Token: 0x040043C1 RID: 17345
	public FreeControllerV3.PositionState startingPositionState;

	// Token: 0x040043C2 RID: 17346
	private JSONStorableStringChooser currentPositionStateJSON;

	// Token: 0x040043C3 RID: 17347
	private FreeControllerV3.PositionState _currentPositionState;

	// Token: 0x040043C4 RID: 17348
	public FreeControllerV3.RotationState startingRotationState;

	// Token: 0x040043C5 RID: 17349
	private JSONStorableStringChooser currentRotationStateJSON;

	// Token: 0x040043C6 RID: 17350
	private FreeControllerV3.RotationState _currentRotationState;

	// Token: 0x040043C7 RID: 17351
	protected float scalePow = 1f;

	// Token: 0x040043C8 RID: 17352
	public Quaternion2Angles.RotationOrder naturalJointDriveRotationOrder;

	// Token: 0x040043C9 RID: 17353
	private JSONStorableBool xLockJSON;

	// Token: 0x040043CA RID: 17354
	[SerializeField]
	private bool _xLock;

	// Token: 0x040043CB RID: 17355
	private JSONStorableBool yLockJSON;

	// Token: 0x040043CC RID: 17356
	[SerializeField]
	private bool _yLock;

	// Token: 0x040043CD RID: 17357
	private JSONStorableBool zLockJSON;

	// Token: 0x040043CE RID: 17358
	[SerializeField]
	private bool _zLock;

	// Token: 0x040043CF RID: 17359
	private JSONStorableBool xLocalLockJSON;

	// Token: 0x040043D0 RID: 17360
	[SerializeField]
	private bool _xLocalLock;

	// Token: 0x040043D1 RID: 17361
	private JSONStorableBool yLocalLockJSON;

	// Token: 0x040043D2 RID: 17362
	[SerializeField]
	private bool _yLocalLock;

	// Token: 0x040043D3 RID: 17363
	private JSONStorableBool zLocalLockJSON;

	// Token: 0x040043D4 RID: 17364
	[SerializeField]
	private bool _zLocalLock;

	// Token: 0x040043D5 RID: 17365
	private JSONStorableBool xRotLockJSON;

	// Token: 0x040043D6 RID: 17366
	[SerializeField]
	private bool _xRotLock;

	// Token: 0x040043D7 RID: 17367
	private JSONStorableBool yRotLockJSON;

	// Token: 0x040043D8 RID: 17368
	[SerializeField]
	private bool _yRotLock;

	// Token: 0x040043D9 RID: 17369
	private JSONStorableBool zRotLockJSON;

	// Token: 0x040043DA RID: 17370
	[SerializeField]
	private bool _zRotLock;

	// Token: 0x040043DB RID: 17371
	public SetTextFromFloat xPositionText;

	// Token: 0x040043DC RID: 17372
	public InputField xPositionInputField;

	// Token: 0x040043DD RID: 17373
	public SetTextFromFloat yPositionText;

	// Token: 0x040043DE RID: 17374
	public InputField yPositionInputField;

	// Token: 0x040043DF RID: 17375
	public SetTextFromFloat zPositionText;

	// Token: 0x040043E0 RID: 17376
	public InputField zPositionInputField;

	// Token: 0x040043E1 RID: 17377
	public SetTextFromFloat xRotationText;

	// Token: 0x040043E2 RID: 17378
	public InputField xRotationInputField;

	// Token: 0x040043E3 RID: 17379
	public SetTextFromFloat yRotationText;

	// Token: 0x040043E4 RID: 17380
	public InputField yRotationInputField;

	// Token: 0x040043E5 RID: 17381
	public SetTextFromFloat zRotationText;

	// Token: 0x040043E6 RID: 17382
	public InputField zRotationInputField;

	// Token: 0x040043E7 RID: 17383
	public SetTextFromFloat xLocalPositionText;

	// Token: 0x040043E8 RID: 17384
	public InputField xLocalPositionInputField;

	// Token: 0x040043E9 RID: 17385
	public SetTextFromFloat yLocalPositionText;

	// Token: 0x040043EA RID: 17386
	public InputField yLocalPositionInputField;

	// Token: 0x040043EB RID: 17387
	public SetTextFromFloat zLocalPositionText;

	// Token: 0x040043EC RID: 17388
	public InputField zLocalPositionInputField;

	// Token: 0x040043ED RID: 17389
	public SetTextFromFloat xLocalRotationText;

	// Token: 0x040043EE RID: 17390
	public InputField xLocalRotationInputField;

	// Token: 0x040043EF RID: 17391
	public SetTextFromFloat yLocalRotationText;

	// Token: 0x040043F0 RID: 17392
	public InputField yLocalRotationInputField;

	// Token: 0x040043F1 RID: 17393
	public SetTextFromFloat zLocalRotationText;

	// Token: 0x040043F2 RID: 17394
	public InputField zLocalRotationInputField;

	// Token: 0x040043F3 RID: 17395
	public bool controlsOn;

	// Token: 0x040043F4 RID: 17396
	protected JSONStorableBool onJSON;

	// Token: 0x040043F5 RID: 17397
	[SerializeField]
	private bool _on = true;

	// Token: 0x040043F6 RID: 17398
	protected JSONStorableBool interactableInPlayModeJSON;

	// Token: 0x040043F7 RID: 17399
	[SerializeField]
	private bool _interactableInPlayMode = true;

	// Token: 0x040043F8 RID: 17400
	public FreeControllerV3[] onPossessDeactiveList;

	// Token: 0x040043F9 RID: 17401
	[SerializeField]
	private bool _possessed;

	// Token: 0x040043FA RID: 17402
	protected JSONStorableBool deactivateOtherControlsOnPossessJSON;

	// Token: 0x040043FB RID: 17403
	private bool _deactivateOtherControlsOnPossess = true;

	// Token: 0x040043FC RID: 17404
	public bool startedPossess;

	// Token: 0x040043FD RID: 17405
	public Transform possessPoint;

	// Token: 0x040043FE RID: 17406
	protected JSONStorableBool possessableJSON;

	// Token: 0x040043FF RID: 17407
	[SerializeField]
	private bool _possessable;

	// Token: 0x04004400 RID: 17408
	protected JSONStorableBool canGrabPositionJSON;

	// Token: 0x04004401 RID: 17409
	[SerializeField]
	private bool _canGrabPosition = true;

	// Token: 0x04004402 RID: 17410
	protected JSONStorableBool canGrabRotationJSON;

	// Token: 0x04004403 RID: 17411
	[SerializeField]
	private bool _canGrabRotation = true;

	// Token: 0x04004404 RID: 17412
	public bool freezeAtomPhysicsWhenGrabbed;

	// Token: 0x04004405 RID: 17413
	protected JSONStorableBool freezeAtomPhysicsWhenGrabbedJSON;

	// Token: 0x04004406 RID: 17414
	private JSONStorableStringChooser positionGridModeJSON;

	// Token: 0x04004407 RID: 17415
	[SerializeField]
	private FreeControllerV3.GridMode _positionGridMode;

	// Token: 0x04004408 RID: 17416
	private JSONStorableFloat positionGridJSON;

	// Token: 0x04004409 RID: 17417
	[SerializeField]
	private float _positionGrid = 0.1f;

	// Token: 0x0400440A RID: 17418
	private JSONStorableStringChooser rotationGridModeJSON;

	// Token: 0x0400440B RID: 17419
	[SerializeField]
	private FreeControllerV3.GridMode _rotationGridMode;

	// Token: 0x0400440C RID: 17420
	private JSONStorableFloat rotationGridJSON;

	// Token: 0x0400440D RID: 17421
	[SerializeField]
	private float _rotationGrid = 15f;

	// Token: 0x0400440E RID: 17422
	private JSONStorableBool useGravityJSON;

	// Token: 0x0400440F RID: 17423
	[SerializeField]
	private bool _useGravityOnRBWhenOff = true;

	// Token: 0x04004410 RID: 17424
	private JSONStorableBool physicsEnabledJSON;

	// Token: 0x04004411 RID: 17425
	public bool controlsCollisionEnabled;

	// Token: 0x04004412 RID: 17426
	private bool _globalCollisionEnabled = true;

	// Token: 0x04004413 RID: 17427
	private JSONStorableBool collisionEnabledJSON;

	// Token: 0x04004414 RID: 17428
	private Rigidbody _followWhenOffRB;

	// Token: 0x04004415 RID: 17429
	private Rigidbody kinematicRB;

	// Token: 0x04004416 RID: 17430
	private ConfigurableJoint connectedJoint;

	// Token: 0x04004417 RID: 17431
	private ConfigurableJoint naturalJoint;

	// Token: 0x04004418 RID: 17432
	public bool useForceWhenOff = true;

	// Token: 0x04004419 RID: 17433
	public float distanceHolder;

	// Token: 0x0400441A RID: 17434
	public float forceFactor = 10000f;

	// Token: 0x0400441B RID: 17435
	public float torqueFactor = 2000f;

	// Token: 0x0400441C RID: 17436
	public Rigidbody[] rigidbodySlavesForMass;

	// Token: 0x0400441D RID: 17437
	private JSONStorableFloat RBMassJSON;

	// Token: 0x0400441E RID: 17438
	private JSONStorableFloat RBDragJSON;

	// Token: 0x0400441F RID: 17439
	private bool _RBMaxVelocityEnable = true;

	// Token: 0x04004420 RID: 17440
	private JSONStorableBool RBMaxVelocityEnableJSON;

	// Token: 0x04004421 RID: 17441
	private float _RBMaxVelocity = 10f;

	// Token: 0x04004422 RID: 17442
	private JSONStorableFloat RBMaxVelocityJSON;

	// Token: 0x04004423 RID: 17443
	private JSONStorableFloat RBAngularDragJSON;

	// Token: 0x04004424 RID: 17444
	[SerializeField]
	private float _RBLockPositionSpring = 250000f;

	// Token: 0x04004425 RID: 17445
	[SerializeField]
	private float _RBLockPositionDamper = 250f;

	// Token: 0x04004426 RID: 17446
	[SerializeField]
	public float _RBLockPositionMaxForce = 100000000f;

	// Token: 0x04004427 RID: 17447
	private JSONStorableFloat RBHoldPositionSpringJSON;

	// Token: 0x04004428 RID: 17448
	[SerializeField]
	private float _RBHoldPositionSpring = 1000f;

	// Token: 0x04004429 RID: 17449
	private JSONStorableFloat RBHoldPositionDamperJSON;

	// Token: 0x0400442A RID: 17450
	[SerializeField]
	private float _RBHoldPositionDamper = 50f;

	// Token: 0x0400442B RID: 17451
	private JSONStorableFloat RBHoldPositionMaxForceJSON;

	// Token: 0x0400442C RID: 17452
	[SerializeField]
	private float _RBHoldPositionMaxForce = 10000f;

	// Token: 0x0400442D RID: 17453
	private JSONStorableFloat RBComplyPositionSpringJSON;

	// Token: 0x0400442E RID: 17454
	[SerializeField]
	private float _RBComplyPositionSpring = 1500f;

	// Token: 0x0400442F RID: 17455
	private JSONStorableFloat RBComplyPositionDamperJSON;

	// Token: 0x04004430 RID: 17456
	[SerializeField]
	private float _RBComplyPositionDamper = 100f;

	// Token: 0x04004431 RID: 17457
	private float _RBComplyPositionMaxForce = 1E+13f;

	// Token: 0x04004432 RID: 17458
	private JSONStorableFloat RBLinkPositionSpringJSON;

	// Token: 0x04004433 RID: 17459
	[SerializeField]
	private float _RBLinkPositionSpring = 250000f;

	// Token: 0x04004434 RID: 17460
	private JSONStorableFloat RBLinkPositionDamperJSON;

	// Token: 0x04004435 RID: 17461
	[SerializeField]
	private float _RBLinkPositionDamper = 250f;

	// Token: 0x04004436 RID: 17462
	private JSONStorableFloat RBLinkPositionMaxForceJSON;

	// Token: 0x04004437 RID: 17463
	[SerializeField]
	private float _RBLinkPositionMaxForce = 100000000f;

	// Token: 0x04004438 RID: 17464
	[SerializeField]
	private float _RBLockRotationSpring = 250000f;

	// Token: 0x04004439 RID: 17465
	[SerializeField]
	private float _RBLockRotationDamper = 250f;

	// Token: 0x0400443A RID: 17466
	[SerializeField]
	public float _RBLockRotationMaxForce = 100000000f;

	// Token: 0x0400443B RID: 17467
	private JSONStorableFloat RBHoldRotationSpringJSON;

	// Token: 0x0400443C RID: 17468
	[SerializeField]
	private float _RBHoldRotationSpring = 1000f;

	// Token: 0x0400443D RID: 17469
	private JSONStorableFloat RBHoldRotationDamperJSON;

	// Token: 0x0400443E RID: 17470
	[SerializeField]
	private float _RBHoldRotationDamper = 50f;

	// Token: 0x0400443F RID: 17471
	private JSONStorableFloat RBHoldRotationMaxForceJSON;

	// Token: 0x04004440 RID: 17472
	[SerializeField]
	private float _RBHoldRotationMaxForce = 10000f;

	// Token: 0x04004441 RID: 17473
	private JSONStorableFloat RBComplyRotationSpringJSON;

	// Token: 0x04004442 RID: 17474
	[SerializeField]
	private float _RBComplyRotationSpring = 150f;

	// Token: 0x04004443 RID: 17475
	private JSONStorableFloat RBComplyRotationDamperJSON;

	// Token: 0x04004444 RID: 17476
	[SerializeField]
	private float _RBComplyRotationDamper = 10f;

	// Token: 0x04004445 RID: 17477
	private float _RBComplyRotationMaxForce = 1E+13f;

	// Token: 0x04004446 RID: 17478
	private JSONStorableFloat RBLinkRotationSpringJSON;

	// Token: 0x04004447 RID: 17479
	[SerializeField]
	private float _RBLinkRotationSpring = 250000f;

	// Token: 0x04004448 RID: 17480
	private JSONStorableFloat RBLinkRotationDamperJSON;

	// Token: 0x04004449 RID: 17481
	[SerializeField]
	private float _RBLinkRotationDamper = 250f;

	// Token: 0x0400444A RID: 17482
	private JSONStorableFloat RBLinkRotationMaxForceJSON;

	// Token: 0x0400444B RID: 17483
	[SerializeField]
	private float _RBLinkRotationMaxForce = 100000000f;

	// Token: 0x0400444C RID: 17484
	private JSONStorableFloat RBComplyJointRotationDriveSpringJSON;

	// Token: 0x0400444D RID: 17485
	[SerializeField]
	private float _RBComplyJointRotationDriveSpring = 20f;

	// Token: 0x0400444E RID: 17486
	private JSONStorableFloat jointRotationDriveSpringJSON;

	// Token: 0x0400444F RID: 17487
	[SerializeField]
	private float _jointRotationDriveSpring;

	// Token: 0x04004450 RID: 17488
	private JSONStorableFloat jointRotationDriveDamperJSON;

	// Token: 0x04004451 RID: 17489
	[SerializeField]
	private float _jointRotationDriveDamper;

	// Token: 0x04004452 RID: 17490
	private JSONStorableFloat jointRotationDriveMaxForceJSON;

	// Token: 0x04004453 RID: 17491
	[SerializeField]
	private float _jointRotationDriveMaxForce;

	// Token: 0x04004454 RID: 17492
	private JSONStorableFloat jointRotationDriveXTargetJSON;

	// Token: 0x04004455 RID: 17493
	private float _jointRotationDriveXTargetMin;

	// Token: 0x04004456 RID: 17494
	private float _jointRotationDriveXTargetMax;

	// Token: 0x04004457 RID: 17495
	[SerializeField]
	private float _jointRotationDriveXTarget;

	// Token: 0x04004458 RID: 17496
	[SerializeField]
	private float _jointRotationDriveXTargetAdditional;

	// Token: 0x04004459 RID: 17497
	private JSONStorableFloat jointRotationDriveYTargetJSON;

	// Token: 0x0400445A RID: 17498
	private float _jointRotationDriveYTargetMin;

	// Token: 0x0400445B RID: 17499
	private float _jointRotationDriveYTargetMax;

	// Token: 0x0400445C RID: 17500
	[SerializeField]
	private float _jointRotationDriveYTarget;

	// Token: 0x0400445D RID: 17501
	[SerializeField]
	private float _jointRotationDriveYTargetAdditional;

	// Token: 0x0400445E RID: 17502
	private JSONStorableFloat jointRotationDriveZTargetJSON;

	// Token: 0x0400445F RID: 17503
	private float _jointRotationDriveZTargetMin;

	// Token: 0x04004460 RID: 17504
	private float _jointRotationDriveZTargetMax;

	// Token: 0x04004461 RID: 17505
	[SerializeField]
	private float _jointRotationDriveZTarget;

	// Token: 0x04004462 RID: 17506
	[SerializeField]
	private float _jointRotationDriveZTargetAdditional;

	// Token: 0x04004463 RID: 17507
	private bool _detachControl;

	// Token: 0x04004464 RID: 17508
	private JSONStorableBool detachControlJSON;

	// Token: 0x04004465 RID: 17509
	public Text UIDText;

	// Token: 0x04004466 RID: 17510
	public Text UIDTextAlt;

	// Token: 0x04004467 RID: 17511
	public Transform[] UITransforms;

	// Token: 0x04004468 RID: 17512
	public Transform[] UITransformsPlayMode;

	// Token: 0x04004469 RID: 17513
	public bool GUIalwaysVisibleWhenSelected;

	// Token: 0x0400446A RID: 17514
	public bool useContainedMeshRenderers = true;

	// Token: 0x0400446B RID: 17515
	private bool _hidden = true;

	// Token: 0x0400446C RID: 17516
	private bool _guihidden = true;

	// Token: 0x0400446D RID: 17517
	public float unhighlightedScale = 0.5f;

	// Token: 0x0400446E RID: 17518
	public float highlightedScale = 0.5f;

	// Token: 0x0400446F RID: 17519
	public float selectedScale = 1f;

	// Token: 0x04004470 RID: 17520
	private bool _highlighted;

	// Token: 0x04004471 RID: 17521
	private Vector3 _selectedPosition;

	// Token: 0x04004472 RID: 17522
	private bool _selected;

	// Token: 0x04004473 RID: 17523
	public Color onColor = new Color(0f, 1f, 0f, 0.5f);

	// Token: 0x04004474 RID: 17524
	public Color offColor = new Color(1f, 0f, 0f, 0.5f);

	// Token: 0x04004475 RID: 17525
	public Color followingColor = new Color(1f, 0f, 1f, 0.5f);

	// Token: 0x04004476 RID: 17526
	public Color holdColor = new Color(1f, 0.5f, 0f, 0.5f);

	// Token: 0x04004477 RID: 17527
	public Color lockColor = new Color(0.5f, 0.25f, 0f, 0.5f);

	// Token: 0x04004478 RID: 17528
	public Color lookAtColor = new Color(0f, 1f, 1f, 0.5f);

	// Token: 0x04004479 RID: 17529
	public Color highlightColor = new Color(1f, 1f, 0f, 0.5f);

	// Token: 0x0400447A RID: 17530
	public Color selectedColor = new Color(0f, 0f, 1f, 0.5f);

	// Token: 0x0400447B RID: 17531
	public Color overlayColor = new Color(1f, 1f, 1f, 0.5f);

	// Token: 0x0400447C RID: 17532
	private Color _currentPositionColor;

	// Token: 0x0400447D RID: 17533
	private Color _currentRotationColor;

	// Token: 0x0400447E RID: 17534
	public Material material;

	// Token: 0x0400447F RID: 17535
	public Material linkLineMaterial;

	// Token: 0x04004480 RID: 17536
	private LineDrawer linkLineDrawer;

	// Token: 0x04004481 RID: 17537
	private Material positionMaterialLocal;

	// Token: 0x04004482 RID: 17538
	private Material rotationMaterialLocal;

	// Token: 0x04004483 RID: 17539
	private Material snapshotMaterialLocal;

	// Token: 0x04004484 RID: 17540
	private Material linkLineMaterialLocal;

	// Token: 0x04004485 RID: 17541
	private Material materialOverlay;

	// Token: 0x04004486 RID: 17542
	public float meshScale = 0.5f;

	// Token: 0x04004487 RID: 17543
	private Mesh _currentPositionMesh;

	// Token: 0x04004488 RID: 17544
	private Mesh _currentRotationMesh;

	// Token: 0x04004489 RID: 17545
	public bool drawSnapshot;

	// Token: 0x0400448A RID: 17546
	private Matrix4x4 snapshotMatrix;

	// Token: 0x0400448B RID: 17547
	public bool drawMesh = true;

	// Token: 0x0400448C RID: 17548
	public bool drawMeshWhenDeselected = true;

	// Token: 0x0400448D RID: 17549
	public Mesh onPositionMesh;

	// Token: 0x0400448E RID: 17550
	public Mesh offPositionMesh;

	// Token: 0x0400448F RID: 17551
	public Mesh followingPositionMesh;

	// Token: 0x04004490 RID: 17552
	public Mesh holdPositionMesh;

	// Token: 0x04004491 RID: 17553
	public Mesh lockPositionMesh;

	// Token: 0x04004492 RID: 17554
	public Mesh onRotationMesh;

	// Token: 0x04004493 RID: 17555
	public Mesh offRotationMesh;

	// Token: 0x04004494 RID: 17556
	public Mesh followingRotationMesh;

	// Token: 0x04004495 RID: 17557
	public Mesh holdRotationMesh;

	// Token: 0x04004496 RID: 17558
	public Mesh lockRotationMesh;

	// Token: 0x04004497 RID: 17559
	public Mesh lookAtRotationMesh;

	// Token: 0x04004498 RID: 17560
	public Mesh moveModeOverlayMesh;

	// Token: 0x04004499 RID: 17561
	public Mesh rotateModeOverlayMesh;

	// Token: 0x0400449A RID: 17562
	public Mesh deselectedMesh;

	// Token: 0x0400449B RID: 17563
	public float deselectedMeshScale = 0.5f;

	// Token: 0x0400449C RID: 17564
	public bool debug;

	// Token: 0x0400449D RID: 17565
	public Transform control;

	// Token: 0x0400449E RID: 17566
	public Transform follow;

	// Token: 0x0400449F RID: 17567
	public Transform followWhenOff;

	// Token: 0x040044A0 RID: 17568
	public Transform lookAt;

	// Token: 0x040044A1 RID: 17569
	public Transform alsoMoveWhenInactive;

	// Token: 0x040044A2 RID: 17570
	public Transform alsoMoveWhenInactiveParentWhenActive;

	// Token: 0x040044A3 RID: 17571
	public Transform alsoMoveWhenInactiveParentWhenInactive;

	// Token: 0x040044A4 RID: 17572
	public Transform alsoMoveWhenInactiveAlternate;

	// Token: 0x040044A5 RID: 17573
	public Transform focusPoint;

	// Token: 0x040044A6 RID: 17574
	public FreeControllerV3.MoveAxisnames MoveAxis1 = FreeControllerV3.MoveAxisnames.CameraRightNoY;

	// Token: 0x040044A7 RID: 17575
	public FreeControllerV3.MoveAxisnames MoveAxis2 = FreeControllerV3.MoveAxisnames.CameraForwardNoY;

	// Token: 0x040044A8 RID: 17576
	public FreeControllerV3.MoveAxisnames MoveAxis3 = FreeControllerV3.MoveAxisnames.Y;

	// Token: 0x040044A9 RID: 17577
	public FreeControllerV3.RotateAxisnames RotateAxis1 = FreeControllerV3.RotateAxisnames.Z;

	// Token: 0x040044AA RID: 17578
	public FreeControllerV3.RotateAxisnames RotateAxis2;

	// Token: 0x040044AB RID: 17579
	public FreeControllerV3.RotateAxisnames RotateAxis3 = FreeControllerV3.RotateAxisnames.Y;

	// Token: 0x040044AC RID: 17580
	public FreeControllerV3.DrawAxisnames MeshForwardAxis = FreeControllerV3.DrawAxisnames.Y;

	// Token: 0x040044AD RID: 17581
	public FreeControllerV3.DrawAxisnames MeshUpAxis = FreeControllerV3.DrawAxisnames.Z;

	// Token: 0x040044AE RID: 17582
	public FreeControllerV3.DrawAxisnames DrawForwardAxis = FreeControllerV3.DrawAxisnames.Z;

	// Token: 0x040044AF RID: 17583
	public FreeControllerV3.DrawAxisnames DrawUpAxis = FreeControllerV3.DrawAxisnames.Y;

	// Token: 0x040044B0 RID: 17584
	public FreeControllerV3.DrawAxisnames PossessForwardAxis = FreeControllerV3.DrawAxisnames.Z;

	// Token: 0x040044B1 RID: 17585
	public FreeControllerV3.DrawAxisnames PossessUpAxis = FreeControllerV3.DrawAxisnames.Y;

	// Token: 0x040044B2 RID: 17586
	public float moveFactor = 1f;

	// Token: 0x040044B3 RID: 17587
	public float rotateFactor = 60f;

	// Token: 0x040044B4 RID: 17588
	private bool _moveEnabled = true;

	// Token: 0x040044B5 RID: 17589
	private bool _moveForceEnabled;

	// Token: 0x040044B6 RID: 17590
	private bool _rotationEnabled = true;

	// Token: 0x040044B7 RID: 17591
	private bool _rotationForceEnabled;

	// Token: 0x040044B8 RID: 17592
	private Vector3 appliedForce;

	// Token: 0x040044B9 RID: 17593
	private Vector3 appliedTorque;

	// Token: 0x040044BA RID: 17594
	private FreeControllerV3.ControlMode _controlMode = FreeControllerV3.ControlMode.Position;

	// Token: 0x040044BB RID: 17595
	public Vector3 startingPosition;

	// Token: 0x040044BC RID: 17596
	public Quaternion startingRotation;

	// Token: 0x040044BD RID: 17597
	public Vector3 startingLocalPosition;

	// Token: 0x040044BE RID: 17598
	public Quaternion startingLocalRotation;

	// Token: 0x040044BF RID: 17599
	private Vector3 initialLocalPosition;

	// Token: 0x040044C0 RID: 17600
	private Quaternion initialLocalRotation;

	// Token: 0x040044C1 RID: 17601
	private MeshRenderer[] mrs;

	// Token: 0x040044C2 RID: 17602
	protected FreeControllerV3UI currentFCUI;

	// Token: 0x040044C3 RID: 17603
	protected FreeControllerV3UI currentFCUIAlt;

	// Token: 0x040044C4 RID: 17604
	protected int complyPauseFrames;

	// Token: 0x040044C5 RID: 17605
	[SerializeField]
	protected float complyPositionThreshold = 0.001f;

	// Token: 0x040044C6 RID: 17606
	protected JSONStorableFloat complyPositionThresholdJSON;

	// Token: 0x040044C7 RID: 17607
	[SerializeField]
	protected float complyRotationThreshold = 5f;

	// Token: 0x040044C8 RID: 17608
	protected JSONStorableFloat complyRotationThresholdJSON;

	// Token: 0x040044C9 RID: 17609
	[SerializeField]
	protected float complySpeed = 10f;

	// Token: 0x040044CA RID: 17610
	protected JSONStorableFloat complySpeedJSON;

	// Token: 0x040044CB RID: 17611
	public FreeControllerV3.OnPositionChange onPositionChangeHandlers;

	// Token: 0x040044CC RID: 17612
	public FreeControllerV3.OnPositionChange onRotationChangeHandlers;

	// Token: 0x040044CD RID: 17613
	public FreeControllerV3.OnMovement onMovementHandlers;

	// Token: 0x040044CE RID: 17614
	public FreeControllerV3.OnGrabStart onGrabStartHandlers;

	// Token: 0x040044CF RID: 17615
	public FreeControllerV3.OnGrabEnd onGrabEndHandlers;

	// Token: 0x040044D0 RID: 17616
	public FreeControllerV3.OnPossessStart onPossessStartHandlers;

	// Token: 0x040044D1 RID: 17617
	public FreeControllerV3.OnPossessEnd onPossessEndHandlers;

	// Token: 0x040044D2 RID: 17618
	private bool wasInit;

	// Token: 0x02000B99 RID: 2969
	public enum SelectLinkState
	{
		// Token: 0x040044D4 RID: 17620
		PositionAndRotation,
		// Token: 0x040044D5 RID: 17621
		Position,
		// Token: 0x040044D6 RID: 17622
		Rotation
	}

	// Token: 0x02000B9A RID: 2970
	public enum PositionState
	{
		// Token: 0x040044D8 RID: 17624
		On,
		// Token: 0x040044D9 RID: 17625
		Off,
		// Token: 0x040044DA RID: 17626
		Following,
		// Token: 0x040044DB RID: 17627
		Hold,
		// Token: 0x040044DC RID: 17628
		Lock,
		// Token: 0x040044DD RID: 17629
		ParentLink,
		// Token: 0x040044DE RID: 17630
		PhysicsLink,
		// Token: 0x040044DF RID: 17631
		Comply
	}

	// Token: 0x02000B9B RID: 2971
	public enum RotationState
	{
		// Token: 0x040044E1 RID: 17633
		On,
		// Token: 0x040044E2 RID: 17634
		Off,
		// Token: 0x040044E3 RID: 17635
		Following,
		// Token: 0x040044E4 RID: 17636
		Hold,
		// Token: 0x040044E5 RID: 17637
		Lock,
		// Token: 0x040044E6 RID: 17638
		LookAt,
		// Token: 0x040044E7 RID: 17639
		ParentLink,
		// Token: 0x040044E8 RID: 17640
		PhysicsLink,
		// Token: 0x040044E9 RID: 17641
		Comply
	}

	// Token: 0x02000B9C RID: 2972
	public enum GridMode
	{
		// Token: 0x040044EB RID: 17643
		None,
		// Token: 0x040044EC RID: 17644
		Local,
		// Token: 0x040044ED RID: 17645
		Global
	}

	// Token: 0x02000B9D RID: 2973
	public enum MoveAxisnames
	{
		// Token: 0x040044EF RID: 17647
		X,
		// Token: 0x040044F0 RID: 17648
		Y,
		// Token: 0x040044F1 RID: 17649
		Z,
		// Token: 0x040044F2 RID: 17650
		CameraRight,
		// Token: 0x040044F3 RID: 17651
		CameraUp,
		// Token: 0x040044F4 RID: 17652
		CameraForward,
		// Token: 0x040044F5 RID: 17653
		CameraRightNoY,
		// Token: 0x040044F6 RID: 17654
		CameraForwardNoY,
		// Token: 0x040044F7 RID: 17655
		None
	}

	// Token: 0x02000B9E RID: 2974
	public enum RotateAxisnames
	{
		// Token: 0x040044F9 RID: 17657
		X,
		// Token: 0x040044FA RID: 17658
		Y,
		// Token: 0x040044FB RID: 17659
		Z,
		// Token: 0x040044FC RID: 17660
		NegX,
		// Token: 0x040044FD RID: 17661
		NegY,
		// Token: 0x040044FE RID: 17662
		NegZ,
		// Token: 0x040044FF RID: 17663
		WorldY,
		// Token: 0x04004500 RID: 17664
		None
	}

	// Token: 0x02000B9F RID: 2975
	public enum DrawAxisnames
	{
		// Token: 0x04004502 RID: 17666
		X,
		// Token: 0x04004503 RID: 17667
		Y,
		// Token: 0x04004504 RID: 17668
		Z,
		// Token: 0x04004505 RID: 17669
		NegX,
		// Token: 0x04004506 RID: 17670
		NegY,
		// Token: 0x04004507 RID: 17671
		NegZ
	}

	// Token: 0x02000BA0 RID: 2976
	public enum ControlMode
	{
		// Token: 0x04004509 RID: 17673
		Off,
		// Token: 0x0400450A RID: 17674
		Position,
		// Token: 0x0400450B RID: 17675
		Rotation
	}

	// Token: 0x02000BA1 RID: 2977
	// (Invoke) Token: 0x06005527 RID: 21799
	public delegate void OnPositionChange(FreeControllerV3 fcv3);

	// Token: 0x02000BA2 RID: 2978
	// (Invoke) Token: 0x0600552B RID: 21803
	public delegate void OnRotationChange(FreeControllerV3 fcv3);

	// Token: 0x02000BA3 RID: 2979
	// (Invoke) Token: 0x0600552F RID: 21807
	public delegate void OnMovement(FreeControllerV3 fcv3);

	// Token: 0x02000BA4 RID: 2980
	// (Invoke) Token: 0x06005533 RID: 21811
	public delegate void OnGrabStart(FreeControllerV3 fcv3);

	// Token: 0x02000BA5 RID: 2981
	// (Invoke) Token: 0x06005537 RID: 21815
	public delegate void OnGrabEnd(FreeControllerV3 fcv3);

	// Token: 0x02000BA6 RID: 2982
	// (Invoke) Token: 0x0600553B RID: 21819
	public delegate void OnPossessStart(FreeControllerV3 fcv3);

	// Token: 0x02000BA7 RID: 2983
	// (Invoke) Token: 0x0600553F RID: 21823
	public delegate void OnPossessEnd(FreeControllerV3 fcv3);

	// Token: 0x02000FE1 RID: 4065
	[CompilerGenerated]
	private sealed class <RegisterFCUI>c__AnonStorey0
	{
		// Token: 0x0600758F RID: 30095 RVA: 0x001F2270 File Offset: 0x001F0670
		public <RegisterFCUI>c__AnonStorey0()
		{
		}

		// Token: 0x06007590 RID: 30096 RVA: 0x001F2278 File Offset: 0x001F0678
		internal void <>m__0()
		{
			this.$this.MoveAbsoluteNoForce(-1f, 0f, 0f);
		}

		// Token: 0x06007591 RID: 30097 RVA: 0x001F2294 File Offset: 0x001F0694
		internal void <>m__1()
		{
			this.$this.MoveAbsoluteNoForce(-0.1f, 0f, 0f);
		}

		// Token: 0x06007592 RID: 30098 RVA: 0x001F22B0 File Offset: 0x001F06B0
		internal void <>m__2()
		{
			this.$this.MoveAbsoluteNoForce(-0.01f, 0f, 0f);
		}

		// Token: 0x06007593 RID: 30099 RVA: 0x001F22CC File Offset: 0x001F06CC
		internal void <>m__3()
		{
			this.$this.MoveAbsoluteNoForce(0.01f, 0f, 0f);
		}

		// Token: 0x06007594 RID: 30100 RVA: 0x001F22E8 File Offset: 0x001F06E8
		internal void <>m__4()
		{
			this.$this.MoveAbsoluteNoForce(0.1f, 0f, 0f);
		}

		// Token: 0x06007595 RID: 30101 RVA: 0x001F2304 File Offset: 0x001F0704
		internal void <>m__5()
		{
			this.$this.MoveAbsoluteNoForce(1f, 0f, 0f);
		}

		// Token: 0x06007596 RID: 30102 RVA: 0x001F2320 File Offset: 0x001F0720
		internal void <>m__6()
		{
			this.$this.MoveAbsoluteNoForce(0f, -1f, 0f);
		}

		// Token: 0x06007597 RID: 30103 RVA: 0x001F233C File Offset: 0x001F073C
		internal void <>m__7()
		{
			this.$this.MoveAbsoluteNoForce(0f, -0.1f, 0f);
		}

		// Token: 0x06007598 RID: 30104 RVA: 0x001F2358 File Offset: 0x001F0758
		internal void <>m__8()
		{
			this.$this.MoveAbsoluteNoForce(0f, -0.01f, 0f);
		}

		// Token: 0x06007599 RID: 30105 RVA: 0x001F2374 File Offset: 0x001F0774
		internal void <>m__9()
		{
			this.$this.MoveAbsoluteNoForce(0f, 0.01f, 0f);
		}

		// Token: 0x0600759A RID: 30106 RVA: 0x001F2390 File Offset: 0x001F0790
		internal void <>m__A()
		{
			this.$this.MoveAbsoluteNoForce(0f, 0.1f, 0f);
		}

		// Token: 0x0600759B RID: 30107 RVA: 0x001F23AC File Offset: 0x001F07AC
		internal void <>m__B()
		{
			this.$this.MoveAbsoluteNoForce(0f, 1f, 0f);
		}

		// Token: 0x0600759C RID: 30108 RVA: 0x001F23C8 File Offset: 0x001F07C8
		internal void <>m__C()
		{
			this.$this.MoveAbsoluteNoForce(0f, 0f, -1f);
		}

		// Token: 0x0600759D RID: 30109 RVA: 0x001F23E4 File Offset: 0x001F07E4
		internal void <>m__D()
		{
			this.$this.MoveAbsoluteNoForce(0f, 0f, -0.1f);
		}

		// Token: 0x0600759E RID: 30110 RVA: 0x001F2400 File Offset: 0x001F0800
		internal void <>m__E()
		{
			this.$this.MoveAbsoluteNoForce(0f, 0f, -0.01f);
		}

		// Token: 0x0600759F RID: 30111 RVA: 0x001F241C File Offset: 0x001F081C
		internal void <>m__F()
		{
			this.$this.MoveAbsoluteNoForce(0f, 0f, 0.01f);
		}

		// Token: 0x060075A0 RID: 30112 RVA: 0x001F2438 File Offset: 0x001F0838
		internal void <>m__10()
		{
			this.$this.MoveAbsoluteNoForce(0f, 0f, 0.1f);
		}

		// Token: 0x060075A1 RID: 30113 RVA: 0x001F2454 File Offset: 0x001F0854
		internal void <>m__11()
		{
			this.$this.MoveAbsoluteNoForce(0f, 0f, 1f);
		}

		// Token: 0x060075A2 RID: 30114 RVA: 0x001F2470 File Offset: 0x001F0870
		internal void <>m__12()
		{
			this.$this.XRotationAdd(-45f);
		}

		// Token: 0x060075A3 RID: 30115 RVA: 0x001F2482 File Offset: 0x001F0882
		internal void <>m__13()
		{
			this.$this.XRotationAdd(-5f);
		}

		// Token: 0x060075A4 RID: 30116 RVA: 0x001F2494 File Offset: 0x001F0894
		internal void <>m__14()
		{
			this.$this.XRotationAdd(-0.5f);
		}

		// Token: 0x060075A5 RID: 30117 RVA: 0x001F24A6 File Offset: 0x001F08A6
		internal void <>m__15()
		{
			this.$this.XRotationAdd(0.5f);
		}

		// Token: 0x060075A6 RID: 30118 RVA: 0x001F24B8 File Offset: 0x001F08B8
		internal void <>m__16()
		{
			this.$this.XRotationAdd(5f);
		}

		// Token: 0x060075A7 RID: 30119 RVA: 0x001F24CA File Offset: 0x001F08CA
		internal void <>m__17()
		{
			this.$this.XRotationAdd(45f);
		}

		// Token: 0x060075A8 RID: 30120 RVA: 0x001F24DC File Offset: 0x001F08DC
		internal void <>m__18()
		{
			this.$this.YRotationAdd(-45f);
		}

		// Token: 0x060075A9 RID: 30121 RVA: 0x001F24EE File Offset: 0x001F08EE
		internal void <>m__19()
		{
			this.$this.YRotationAdd(-5f);
		}

		// Token: 0x060075AA RID: 30122 RVA: 0x001F2500 File Offset: 0x001F0900
		internal void <>m__1A()
		{
			this.$this.YRotationAdd(-0.5f);
		}

		// Token: 0x060075AB RID: 30123 RVA: 0x001F2512 File Offset: 0x001F0912
		internal void <>m__1B()
		{
			this.$this.YRotationAdd(0.5f);
		}

		// Token: 0x060075AC RID: 30124 RVA: 0x001F2524 File Offset: 0x001F0924
		internal void <>m__1C()
		{
			this.$this.YRotationAdd(5f);
		}

		// Token: 0x060075AD RID: 30125 RVA: 0x001F2536 File Offset: 0x001F0936
		internal void <>m__1D()
		{
			this.$this.YRotationAdd(45f);
		}

		// Token: 0x060075AE RID: 30126 RVA: 0x001F2548 File Offset: 0x001F0948
		internal void <>m__1E()
		{
			this.$this.ZRotationAdd(-45f);
		}

		// Token: 0x060075AF RID: 30127 RVA: 0x001F255A File Offset: 0x001F095A
		internal void <>m__1F()
		{
			this.$this.ZRotationAdd(-5f);
		}

		// Token: 0x060075B0 RID: 30128 RVA: 0x001F256C File Offset: 0x001F096C
		internal void <>m__20()
		{
			this.$this.ZRotationAdd(-0.5f);
		}

		// Token: 0x060075B1 RID: 30129 RVA: 0x001F257E File Offset: 0x001F097E
		internal void <>m__21()
		{
			this.$this.ZRotationAdd(0.5f);
		}

		// Token: 0x060075B2 RID: 30130 RVA: 0x001F2590 File Offset: 0x001F0990
		internal void <>m__22()
		{
			this.$this.ZRotationAdd(5f);
		}

		// Token: 0x060075B3 RID: 30131 RVA: 0x001F25A2 File Offset: 0x001F09A2
		internal void <>m__23()
		{
			this.$this.ZRotationAdd(45f);
		}

		// Token: 0x060075B4 RID: 30132 RVA: 0x001F25B4 File Offset: 0x001F09B4
		internal void <>m__24()
		{
			this.$this.SetXPositionNoForce(0f);
		}

		// Token: 0x060075B5 RID: 30133 RVA: 0x001F25C6 File Offset: 0x001F09C6
		internal void <>m__25()
		{
			this.$this.SetYPositionNoForce(0f);
		}

		// Token: 0x060075B6 RID: 30134 RVA: 0x001F25D8 File Offset: 0x001F09D8
		internal void <>m__26()
		{
			this.$this.SetZPositionNoForce(0f);
		}

		// Token: 0x060075B7 RID: 30135 RVA: 0x001F25EA File Offset: 0x001F09EA
		internal void <>m__27(string A_1)
		{
			this.$this.MoveXPositionRelativeNoForce(this.fcui.xSelfRelativePositionAdjustInputField.text);
			this.fcui.xSelfRelativePositionAdjustInputField.text = string.Empty;
		}

		// Token: 0x060075B8 RID: 30136 RVA: 0x001F261C File Offset: 0x001F0A1C
		internal void <>m__28()
		{
			this.$this.MoveXPositionRelativeNoForce(this.fcui.xSelfRelativePositionAdjustInputField.text);
			this.fcui.xSelfRelativePositionAdjustInputField.text = string.Empty;
		}

		// Token: 0x060075B9 RID: 30137 RVA: 0x001F264E File Offset: 0x001F0A4E
		internal void <>m__29()
		{
			this.$this.MoveRelativeNoForce(-1f, 0f, 0f);
		}

		// Token: 0x060075BA RID: 30138 RVA: 0x001F266A File Offset: 0x001F0A6A
		internal void <>m__2A()
		{
			this.$this.MoveRelativeNoForce(-0.1f, 0f, 0f);
		}

		// Token: 0x060075BB RID: 30139 RVA: 0x001F2686 File Offset: 0x001F0A86
		internal void <>m__2B()
		{
			this.$this.MoveRelativeNoForce(-0.01f, 0f, 0f);
		}

		// Token: 0x060075BC RID: 30140 RVA: 0x001F26A2 File Offset: 0x001F0AA2
		internal void <>m__2C()
		{
			this.$this.MoveRelativeNoForce(0.01f, 0f, 0f);
		}

		// Token: 0x060075BD RID: 30141 RVA: 0x001F26BE File Offset: 0x001F0ABE
		internal void <>m__2D()
		{
			this.$this.MoveRelativeNoForce(0.1f, 0f, 0f);
		}

		// Token: 0x060075BE RID: 30142 RVA: 0x001F26DA File Offset: 0x001F0ADA
		internal void <>m__2E()
		{
			this.$this.MoveRelativeNoForce(1f, 0f, 0f);
		}

		// Token: 0x060075BF RID: 30143 RVA: 0x001F26F6 File Offset: 0x001F0AF6
		internal void <>m__2F(string A_1)
		{
			this.$this.MoveYPositionRelativeNoForce(this.fcui.ySelfRelativePositionAdjustInputField.text);
			this.fcui.ySelfRelativePositionAdjustInputField.text = string.Empty;
		}

		// Token: 0x060075C0 RID: 30144 RVA: 0x001F2728 File Offset: 0x001F0B28
		internal void <>m__30()
		{
			this.$this.MoveYPositionRelativeNoForce(this.fcui.ySelfRelativePositionAdjustInputField.text);
			this.fcui.ySelfRelativePositionAdjustInputField.text = string.Empty;
		}

		// Token: 0x060075C1 RID: 30145 RVA: 0x001F275A File Offset: 0x001F0B5A
		internal void <>m__31()
		{
			this.$this.MoveRelativeNoForce(0f, -1f, 0f);
		}

		// Token: 0x060075C2 RID: 30146 RVA: 0x001F2776 File Offset: 0x001F0B76
		internal void <>m__32()
		{
			this.$this.MoveRelativeNoForce(0f, -0.1f, 0f);
		}

		// Token: 0x060075C3 RID: 30147 RVA: 0x001F2792 File Offset: 0x001F0B92
		internal void <>m__33()
		{
			this.$this.MoveRelativeNoForce(0f, -0.01f, 0f);
		}

		// Token: 0x060075C4 RID: 30148 RVA: 0x001F27AE File Offset: 0x001F0BAE
		internal void <>m__34()
		{
			this.$this.MoveRelativeNoForce(0f, 0.01f, 0f);
		}

		// Token: 0x060075C5 RID: 30149 RVA: 0x001F27CA File Offset: 0x001F0BCA
		internal void <>m__35()
		{
			this.$this.MoveRelativeNoForce(0f, 0.1f, 0f);
		}

		// Token: 0x060075C6 RID: 30150 RVA: 0x001F27E6 File Offset: 0x001F0BE6
		internal void <>m__36()
		{
			this.$this.MoveRelativeNoForce(0f, 1f, 0f);
		}

		// Token: 0x060075C7 RID: 30151 RVA: 0x001F2802 File Offset: 0x001F0C02
		internal void <>m__37(string A_1)
		{
			this.$this.MoveZPositionRelativeNoForce(this.fcui.zSelfRelativePositionAdjustInputField.text);
			this.fcui.zSelfRelativePositionAdjustInputField.text = string.Empty;
		}

		// Token: 0x060075C8 RID: 30152 RVA: 0x001F2834 File Offset: 0x001F0C34
		internal void <>m__38()
		{
			this.$this.MoveZPositionRelativeNoForce(this.fcui.zSelfRelativePositionAdjustInputField.text);
			this.fcui.zSelfRelativePositionAdjustInputField.text = string.Empty;
		}

		// Token: 0x060075C9 RID: 30153 RVA: 0x001F2866 File Offset: 0x001F0C66
		internal void <>m__39()
		{
			this.$this.MoveRelativeNoForce(0f, 0f, -1f);
		}

		// Token: 0x060075CA RID: 30154 RVA: 0x001F2882 File Offset: 0x001F0C82
		internal void <>m__3A()
		{
			this.$this.MoveRelativeNoForce(0f, 0f, -0.1f);
		}

		// Token: 0x060075CB RID: 30155 RVA: 0x001F289E File Offset: 0x001F0C9E
		internal void <>m__3B()
		{
			this.$this.MoveRelativeNoForce(0f, 0f, -0.01f);
		}

		// Token: 0x060075CC RID: 30156 RVA: 0x001F28BA File Offset: 0x001F0CBA
		internal void <>m__3C()
		{
			this.$this.MoveRelativeNoForce(0f, 0f, 0.01f);
		}

		// Token: 0x060075CD RID: 30157 RVA: 0x001F28D6 File Offset: 0x001F0CD6
		internal void <>m__3D()
		{
			this.$this.MoveRelativeNoForce(0f, 0f, 0.1f);
		}

		// Token: 0x060075CE RID: 30158 RVA: 0x001F28F2 File Offset: 0x001F0CF2
		internal void <>m__3E()
		{
			this.$this.MoveRelativeNoForce(0f, 0f, 1f);
		}

		// Token: 0x060075CF RID: 30159 RVA: 0x001F290E File Offset: 0x001F0D0E
		internal void <>m__3F(string A_1)
		{
			this.$this.RotateXSelfRelativeNoForce(this.fcui.xSelfRelativePositionAdjustInputField.text);
			this.fcui.xSelfRelativeRotationAdjustInputField.text = string.Empty;
		}

		// Token: 0x060075D0 RID: 30160 RVA: 0x001F2940 File Offset: 0x001F0D40
		internal void <>m__40()
		{
			this.$this.RotateXSelfRelativeNoForce(this.fcui.xSelfRelativePositionAdjustInputField.text);
			this.fcui.xSelfRelativeRotationAdjustInputField.text = string.Empty;
		}

		// Token: 0x060075D1 RID: 30161 RVA: 0x001F2972 File Offset: 0x001F0D72
		internal void <>m__41()
		{
			this.$this.RotateSelfRelativeNoForce(new Vector3(-45f, 0f, 0f));
		}

		// Token: 0x060075D2 RID: 30162 RVA: 0x001F2993 File Offset: 0x001F0D93
		internal void <>m__42()
		{
			this.$this.RotateSelfRelativeNoForce(new Vector3(-5f, 0f, 0f));
		}

		// Token: 0x060075D3 RID: 30163 RVA: 0x001F29B4 File Offset: 0x001F0DB4
		internal void <>m__43()
		{
			this.$this.RotateSelfRelativeNoForce(new Vector3(-0.5f, 0f, 0f));
		}

		// Token: 0x060075D4 RID: 30164 RVA: 0x001F29D5 File Offset: 0x001F0DD5
		internal void <>m__44()
		{
			this.$this.RotateSelfRelativeNoForce(new Vector3(0.5f, 0f, 0f));
		}

		// Token: 0x060075D5 RID: 30165 RVA: 0x001F29F6 File Offset: 0x001F0DF6
		internal void <>m__45()
		{
			this.$this.RotateSelfRelativeNoForce(new Vector3(5f, 0f, 0f));
		}

		// Token: 0x060075D6 RID: 30166 RVA: 0x001F2A17 File Offset: 0x001F0E17
		internal void <>m__46()
		{
			this.$this.RotateSelfRelativeNoForce(new Vector3(45f, 0f, 0f));
		}

		// Token: 0x060075D7 RID: 30167 RVA: 0x001F2A38 File Offset: 0x001F0E38
		internal void <>m__47(string A_1)
		{
			this.$this.RotateYSelfRelativeNoForce(this.fcui.ySelfRelativePositionAdjustInputField.text);
			this.fcui.ySelfRelativeRotationAdjustInputField.text = string.Empty;
		}

		// Token: 0x060075D8 RID: 30168 RVA: 0x001F2A6A File Offset: 0x001F0E6A
		internal void <>m__48()
		{
			this.$this.RotateYSelfRelativeNoForce(this.fcui.ySelfRelativePositionAdjustInputField.text);
			this.fcui.ySelfRelativeRotationAdjustInputField.text = string.Empty;
		}

		// Token: 0x060075D9 RID: 30169 RVA: 0x001F2A9C File Offset: 0x001F0E9C
		internal void <>m__49()
		{
			this.$this.RotateSelfRelativeNoForce(new Vector3(0f, -45f, 0f));
		}

		// Token: 0x060075DA RID: 30170 RVA: 0x001F2ABD File Offset: 0x001F0EBD
		internal void <>m__4A()
		{
			this.$this.RotateSelfRelativeNoForce(new Vector3(0f, -5f, 0f));
		}

		// Token: 0x060075DB RID: 30171 RVA: 0x001F2ADE File Offset: 0x001F0EDE
		internal void <>m__4B()
		{
			this.$this.RotateSelfRelativeNoForce(new Vector3(0f, -0.5f, 0f));
		}

		// Token: 0x060075DC RID: 30172 RVA: 0x001F2AFF File Offset: 0x001F0EFF
		internal void <>m__4C()
		{
			this.$this.RotateSelfRelativeNoForce(new Vector3(0f, 0.5f, 0f));
		}

		// Token: 0x060075DD RID: 30173 RVA: 0x001F2B20 File Offset: 0x001F0F20
		internal void <>m__4D()
		{
			this.$this.RotateSelfRelativeNoForce(new Vector3(0f, 5f, 0f));
		}

		// Token: 0x060075DE RID: 30174 RVA: 0x001F2B41 File Offset: 0x001F0F41
		internal void <>m__4E()
		{
			this.$this.RotateSelfRelativeNoForce(new Vector3(0f, 45f, 0f));
		}

		// Token: 0x060075DF RID: 30175 RVA: 0x001F2B62 File Offset: 0x001F0F62
		internal void <>m__4F(string A_1)
		{
			this.$this.RotateZSelfRelativeNoForce(this.fcui.zSelfRelativePositionAdjustInputField.text);
			this.fcui.zSelfRelativeRotationAdjustInputField.text = string.Empty;
		}

		// Token: 0x060075E0 RID: 30176 RVA: 0x001F2B94 File Offset: 0x001F0F94
		internal void <>m__50()
		{
			this.$this.RotateZSelfRelativeNoForce(this.fcui.zSelfRelativePositionAdjustInputField.text);
			this.fcui.zSelfRelativeRotationAdjustInputField.text = string.Empty;
		}

		// Token: 0x060075E1 RID: 30177 RVA: 0x001F2BC6 File Offset: 0x001F0FC6
		internal void <>m__51()
		{
			this.$this.RotateSelfRelativeNoForce(new Vector3(0f, 0f, -45f));
		}

		// Token: 0x060075E2 RID: 30178 RVA: 0x001F2BE7 File Offset: 0x001F0FE7
		internal void <>m__52()
		{
			this.$this.RotateSelfRelativeNoForce(new Vector3(0f, 0f, -5f));
		}

		// Token: 0x060075E3 RID: 30179 RVA: 0x001F2C08 File Offset: 0x001F1008
		internal void <>m__53()
		{
			this.$this.RotateSelfRelativeNoForce(new Vector3(0f, 0f, -0.5f));
		}

		// Token: 0x060075E4 RID: 30180 RVA: 0x001F2C29 File Offset: 0x001F1029
		internal void <>m__54()
		{
			this.$this.RotateSelfRelativeNoForce(new Vector3(0f, 0f, 0.5f));
		}

		// Token: 0x060075E5 RID: 30181 RVA: 0x001F2C4A File Offset: 0x001F104A
		internal void <>m__55()
		{
			this.$this.RotateSelfRelativeNoForce(new Vector3(0f, 0f, 5f));
		}

		// Token: 0x060075E6 RID: 30182 RVA: 0x001F2C6B File Offset: 0x001F106B
		internal void <>m__56()
		{
			this.$this.RotateSelfRelativeNoForce(new Vector3(0f, 0f, 45f));
		}

		// Token: 0x060075E7 RID: 30183 RVA: 0x001F2C8C File Offset: 0x001F108C
		internal void <>m__57()
		{
			this.$this.SetXLocalPositionNoForce(0f);
		}

		// Token: 0x060075E8 RID: 30184 RVA: 0x001F2C9E File Offset: 0x001F109E
		internal void <>m__58()
		{
			this.$this.SetYLocalPositionNoForce(0f);
		}

		// Token: 0x060075E9 RID: 30185 RVA: 0x001F2CB0 File Offset: 0x001F10B0
		internal void <>m__59()
		{
			this.$this.SetZLocalPositionNoForce(0f);
		}

		// Token: 0x060075EA RID: 30186 RVA: 0x001F2CC2 File Offset: 0x001F10C2
		internal void <>m__5A()
		{
			this.$this.SetXLocalRotationNoForce(0f);
		}

		// Token: 0x060075EB RID: 30187 RVA: 0x001F2CD4 File Offset: 0x001F10D4
		internal void <>m__5B()
		{
			this.$this.SetYLocalRotationNoForce(0f);
		}

		// Token: 0x060075EC RID: 30188 RVA: 0x001F2CE6 File Offset: 0x001F10E6
		internal void <>m__5C()
		{
			this.$this.SetZLocalRotationNoForce(0f);
		}

		// Token: 0x040069B4 RID: 27060
		internal FreeControllerV3UI fcui;

		// Token: 0x040069B5 RID: 27061
		internal FreeControllerV3 $this;
	}
}
