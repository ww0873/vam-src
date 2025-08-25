using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B93 RID: 2963
public class EyesControl : LookAtWithLimitsControl
{
	// Token: 0x06005375 RID: 21365 RVA: 0x001E446B File Offset: 0x001E286B
	public EyesControl()
	{
	}

	// Token: 0x06005376 RID: 21366 RVA: 0x001E4488 File Offset: 0x001E2888
	protected void SyncLookMode()
	{
		switch (this._currentLookMode)
		{
		case EyesControl.LookMode.None:
			if (this.lookAt1 != null)
			{
				this.lookAt1.enabled = false;
			}
			if (this.lookAt2 != null)
			{
				this.lookAt2.enabled = false;
			}
			break;
		case EyesControl.LookMode.Player:
			if (this.lookAt1 != null)
			{
				this.lookAt1.enabled = true;
				this.lookAt1.lookAtCameraLocation = CameraTarget.CameraLocation.Center;
			}
			if (this.lookAt2 != null)
			{
				this.lookAt2.enabled = true;
				this.lookAt2.lookAtCameraLocation = CameraTarget.CameraLocation.Center;
			}
			break;
		case EyesControl.LookMode.Target:
			if (this.lookAt1 != null)
			{
				this.lookAt1.enabled = true;
				this.lookAt1.lookAtCameraLocation = CameraTarget.CameraLocation.None;
				if (this._lookAt != null)
				{
					this.lookAt1.target = this._lookAt;
				}
				else
				{
					this.lookAt1.target = null;
				}
			}
			if (this.lookAt2 != null)
			{
				this.lookAt2.enabled = true;
				this.lookAt2.lookAtCameraLocation = CameraTarget.CameraLocation.None;
				if (this._lookAt != null)
				{
					this.lookAt2.target = this._lookAt;
				}
				else
				{
					this.lookAt2.target = null;
				}
			}
			break;
		case EyesControl.LookMode.Custom:
			if (this.lookAt1 != null)
			{
				this.lookAt1.enabled = true;
				this.lookAt1.lookAtCameraLocation = CameraTarget.CameraLocation.None;
				if (this.targetControl != null)
				{
					this.lookAt1.target = this.targetControl.control;
				}
				else
				{
					this.lookAt1.target = null;
				}
			}
			if (this.lookAt2 != null)
			{
				this.lookAt2.enabled = true;
				this.lookAt2.lookAtCameraLocation = CameraTarget.CameraLocation.None;
				if (this.targetControl != null)
				{
					this.lookAt2.target = this.targetControl.control;
				}
				else
				{
					this.lookAt2.target = null;
				}
			}
			break;
		}
		if (this.targetAtomJSON != null && this.targetAtomJSON.popup != null)
		{
			this.targetAtomJSON.popup.gameObject.SetActive(this._currentLookMode == EyesControl.LookMode.Custom);
		}
		if (this.targetControlJSON != null && this.targetControlJSON.popup != null)
		{
			this.targetControlJSON.popup.gameObject.SetActive(this._currentLookMode == EyesControl.LookMode.Custom);
		}
	}

	// Token: 0x06005377 RID: 21367 RVA: 0x001E4754 File Offset: 0x001E2B54
	public void SetLookMode(string lookModeString)
	{
		try
		{
			EyesControl.LookMode currentLookMode = (EyesControl.LookMode)Enum.Parse(typeof(EyesControl.LookMode), lookModeString);
			this._currentLookMode = currentLookMode;
			this.SyncLookMode();
		}
		catch (ArgumentException)
		{
			Debug.LogError("Attempted to set look mode type to " + lookModeString + " which is not a valid type");
		}
	}

	// Token: 0x17000C22 RID: 3106
	// (get) Token: 0x06005378 RID: 21368 RVA: 0x001E47B4 File Offset: 0x001E2BB4
	// (set) Token: 0x06005379 RID: 21369 RVA: 0x001E47BC File Offset: 0x001E2BBC
	public EyesControl.LookMode currentLookMode
	{
		get
		{
			return this._currentLookMode;
		}
		set
		{
			if (this.currentLookModeJSON != null)
			{
				this.currentLookModeJSON.val = value.ToString();
			}
			else if (this._currentLookMode != value)
			{
				this.SetLookMode(value.ToString());
			}
		}
	}

	// Token: 0x17000C23 RID: 3107
	// (get) Token: 0x0600537A RID: 21370 RVA: 0x001E4810 File Offset: 0x001E2C10
	// (set) Token: 0x0600537B RID: 21371 RVA: 0x001E4818 File Offset: 0x001E2C18
	public Transform lookAt
	{
		get
		{
			return this._lookAt;
		}
		set
		{
			if (this._lookAt != value)
			{
				this._lookAt = value;
				this.SyncLookMode();
			}
		}
	}

	// Token: 0x0600537C RID: 21372 RVA: 0x001E4838 File Offset: 0x001E2C38
	protected void OnAtomUIDRename(string fromid, string toid)
	{
		if (this.targetAtom != null && this.targetAtom.uid == toid)
		{
			this.targetAtomJSON.valNoCallback = toid;
		}
		this.SyncAtomChocies();
	}

	// Token: 0x0600537D RID: 21373 RVA: 0x001E4873 File Offset: 0x001E2C73
	protected void OnAtomRemoved(Atom a)
	{
		if (this.targetAtom == a)
		{
			this.targetAtomJSON.val = "None";
		}
	}

	// Token: 0x0600537E RID: 21374 RVA: 0x001E4898 File Offset: 0x001E2C98
	protected void SyncAtomChocies()
	{
		List<string> list = new List<string>();
		list.Add("None");
		foreach (string item in SuperController.singleton.GetAtomUIDsWithFreeControllers())
		{
			list.Add(item);
		}
		this.targetAtomJSON.choices = list;
	}

	// Token: 0x0600537F RID: 21375 RVA: 0x001E4918 File Offset: 0x001E2D18
	protected void SyncTargetAtom(string atomUID)
	{
		List<string> list = new List<string>();
		list.Add("None");
		if (atomUID != null)
		{
			this.targetAtom = SuperController.singleton.GetAtomByUid(atomUID);
			if (this.targetAtom != null)
			{
				foreach (FreeControllerV3 freeControllerV in this.targetAtom.freeControllers)
				{
					list.Add(freeControllerV.name);
				}
			}
		}
		else
		{
			this.targetAtom = null;
		}
		this.targetControlJSON.choices = list;
		this.targetControlJSON.val = "None";
	}

	// Token: 0x06005380 RID: 21376 RVA: 0x001E49B8 File Offset: 0x001E2DB8
	protected void SyncTargetControl(string targetID)
	{
		if (this.targetAtom != null && targetID != null && targetID != "None")
		{
			this.targetControl = SuperController.singleton.FreeControllerNameToFreeController(this.targetAtom.uid + ":" + targetID);
		}
		else
		{
			this.targetControl = null;
		}
		this.SyncLookMode();
	}

	// Token: 0x06005381 RID: 21377 RVA: 0x001E4A24 File Offset: 0x001E2E24
	protected override void SyncLookAtLeftRightAngleAdjust()
	{
		if (this.lookAt1 != null)
		{
			this.lookAt1.LeftRightAngleAdjust = this._leftRightAngleAdjust - this._divergeConvergeAngleAdjust;
		}
		if (this.lookAt2 != null)
		{
			this.lookAt2.LeftRightAngleAdjust = this._leftRightAngleAdjust + this._divergeConvergeAngleAdjust;
		}
	}

	// Token: 0x06005382 RID: 21378 RVA: 0x001E4A83 File Offset: 0x001E2E83
	protected void SyncDivergeConvergeAngleAdjust(float f)
	{
		this._divergeConvergeAngleAdjust = f;
		this.SyncLookAtLeftRightAngleAdjust();
	}

	// Token: 0x17000C24 RID: 3108
	// (get) Token: 0x06005383 RID: 21379 RVA: 0x001E4A92 File Offset: 0x001E2E92
	// (set) Token: 0x06005384 RID: 21380 RVA: 0x001E4A9C File Offset: 0x001E2E9C
	public float blinkWeight
	{
		get
		{
			return this._blinkWeight;
		}
		set
		{
			if (this._blinkWeight != value)
			{
				this._blinkWeight = value;
				float moveFactor = Mathf.Clamp01(1f - this._blinkWeight * this.blinkAdjustFactor);
				if (this.lookAt1 != null)
				{
					this.lookAt1.MoveFactor = moveFactor;
				}
				if (this.lookAt2 != null)
				{
					this.lookAt2.MoveFactor = moveFactor;
				}
			}
		}
	}

	// Token: 0x06005385 RID: 21381 RVA: 0x001E4B10 File Offset: 0x001E2F10
	protected override void InitUI(Transform t, bool isAlt)
	{
		base.InitUI(t, isAlt);
		if (t != null)
		{
			EyesControlUI componentInChildren = t.GetComponentInChildren<EyesControlUI>(true);
			if (componentInChildren != null)
			{
				this.currentLookModeJSON.RegisterPopup(componentInChildren.lookModePopup, isAlt);
				this.targetAtomJSON.RegisterPopup(componentInChildren.targetAtomPopup, isAlt);
				this.targetControlJSON.RegisterPopup(componentInChildren.targetControlPopup, isAlt);
				this.divergeConvergeAngleAdjustJSON.RegisterSlider(componentInChildren.divergeConvergeAngleAdjustSlider, isAlt);
				this.SyncLookMode();
			}
		}
	}

	// Token: 0x06005386 RID: 21382 RVA: 0x001E4B94 File Offset: 0x001E2F94
	protected override void Init()
	{
		this.lookAt2InvertRightLeft = true;
		base.Init();
		List<string> choicesList = new List<string>(Enum.GetNames(typeof(EyesControl.LookMode)));
		this.currentLookModeJSON = new JSONStorableStringChooser("lookMode", choicesList, this._currentLookMode.ToString(), "Eyes Look At", new JSONStorableStringChooser.SetStringCallback(this.SetLookMode));
		this.currentLookModeJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterStringChooser(this.currentLookModeJSON);
		this.targetAtomJSON = new JSONStorableStringChooser("targetAtom", null, "None", "Target Atom", new JSONStorableStringChooser.SetStringCallback(this.SyncTargetAtom));
		this.targetAtomJSON.storeType = JSONStorableParam.StoreType.Physical;
		this.targetAtomJSON.popupOpenCallback = new JSONStorableStringChooser.PopupOpenCallback(this.SyncAtomChocies);
		base.RegisterStringChooser(this.targetAtomJSON);
		this.targetControlJSON = new JSONStorableStringChooser("targetControl", null, "None", "Target Control", new JSONStorableStringChooser.SetStringCallback(this.SyncTargetControl));
		this.targetControlJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterStringChooser(this.targetControlJSON);
		this.divergeConvergeAngleAdjustJSON = new JSONStorableFloat("divergeConvergeAngleAdjust", this._divergeConvergeAngleAdjust, new JSONStorableFloat.SetFloatCallback(this.SyncDivergeConvergeAngleAdjust), -30f, 30f, true, true);
		this.divergeConvergeAngleAdjustJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.divergeConvergeAngleAdjustJSON);
		this.SyncTargetAtom(null);
		this.SyncLookMode();
		if (SuperController.singleton != null)
		{
			SuperController singleton = SuperController.singleton;
			singleton.onAtomUIDRenameHandlers = (SuperController.OnAtomUIDRename)Delegate.Combine(singleton.onAtomUIDRenameHandlers, new SuperController.OnAtomUIDRename(this.OnAtomUIDRename));
			SuperController singleton2 = SuperController.singleton;
			singleton2.onAtomRemovedHandlers = (SuperController.OnAtomRemoved)Delegate.Combine(singleton2.onAtomRemovedHandlers, new SuperController.OnAtomRemoved(this.OnAtomRemoved));
		}
	}

	// Token: 0x06005387 RID: 21383 RVA: 0x001E4D52 File Offset: 0x001E3152
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

	// Token: 0x06005388 RID: 21384 RVA: 0x001E4D78 File Offset: 0x001E3178
	protected void OnDestroy()
	{
		if (SuperController.singleton != null)
		{
			SuperController singleton = SuperController.singleton;
			singleton.onAtomUIDRenameHandlers = (SuperController.OnAtomUIDRename)Delegate.Remove(singleton.onAtomUIDRenameHandlers, new SuperController.OnAtomUIDRename(this.OnAtomUIDRename));
			SuperController singleton2 = SuperController.singleton;
			singleton2.onAtomRemovedHandlers = (SuperController.OnAtomRemoved)Delegate.Remove(singleton2.onAtomRemovedHandlers, new SuperController.OnAtomRemoved(this.OnAtomRemoved));
		}
	}

	// Token: 0x04004392 RID: 17298
	protected JSONStorableStringChooser currentLookModeJSON;

	// Token: 0x04004393 RID: 17299
	[SerializeField]
	protected EyesControl.LookMode _currentLookMode = EyesControl.LookMode.Player;

	// Token: 0x04004394 RID: 17300
	[SerializeField]
	protected Transform _lookAt;

	// Token: 0x04004395 RID: 17301
	protected Atom targetAtom;

	// Token: 0x04004396 RID: 17302
	protected JSONStorableStringChooser targetAtomJSON;

	// Token: 0x04004397 RID: 17303
	protected FreeControllerV3 targetControl;

	// Token: 0x04004398 RID: 17304
	protected JSONStorableStringChooser targetControlJSON;

	// Token: 0x04004399 RID: 17305
	protected float _divergeConvergeAngleAdjust;

	// Token: 0x0400439A RID: 17306
	protected JSONStorableFloat divergeConvergeAngleAdjustJSON;

	// Token: 0x0400439B RID: 17307
	public float blinkAdjustFactor = 0.1f;

	// Token: 0x0400439C RID: 17308
	protected float _blinkWeight;

	// Token: 0x02000B94 RID: 2964
	public enum LookMode
	{
		// Token: 0x0400439E RID: 17310
		None,
		// Token: 0x0400439F RID: 17311
		Player,
		// Token: 0x040043A0 RID: 17312
		Target,
		// Token: 0x040043A1 RID: 17313
		Custom
	}
}
