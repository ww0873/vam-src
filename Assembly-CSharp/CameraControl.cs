using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B8B RID: 2955
public class CameraControl : JSONStorable
{
	// Token: 0x0600533D RID: 21309 RVA: 0x001E26C2 File Offset: 0x001E0AC2
	public CameraControl()
	{
	}

	// Token: 0x0600533E RID: 21310 RVA: 0x001E26D8 File Offset: 0x001E0AD8
	protected void SyncCamera()
	{
		if (this.cameraToControl != null)
		{
			switch (this._maskSelection)
			{
			case CameraControl.MaskToShow.mask1:
				this.cameraToControl.cullingMask = this.mask1;
				if (this.cameraToControl2 != null)
				{
					this.cameraToControl2.cullingMask = this.mask1;
				}
				break;
			case CameraControl.MaskToShow.mask2:
				this.cameraToControl.cullingMask = this.mask2;
				if (this.cameraToControl2 != null)
				{
					this.cameraToControl2.cullingMask = this.mask2;
				}
				break;
			case CameraControl.MaskToShow.mask3:
				this.cameraToControl.cullingMask = this.mask3;
				if (this.cameraToControl2 != null)
				{
					this.cameraToControl2.cullingMask = this.mask3;
				}
				break;
			case CameraControl.MaskToShow.mask4:
				this.cameraToControl.cullingMask = this.mask4;
				if (this.cameraToControl2 != null)
				{
					this.cameraToControl2.cullingMask = this.mask4;
				}
				break;
			}
			this.cameraToControl.fieldOfView = this._cameraFOV;
			if (this.cameraToControl2 != null)
			{
				this.cameraToControl2.fieldOfView = this._cameraFOV;
			}
		}
	}

	// Token: 0x0600533F RID: 21311 RVA: 0x001E2854 File Offset: 0x001E0C54
	protected void SyncCameraOnAndUseAudioListener()
	{
		if (this.cameraGroup != null)
		{
			this.cameraGroup.gameObject.SetActive(this.cameraOn);
		}
		if (this.otherObjectToControl != null)
		{
			this.otherObjectToControl.SetActive(this.cameraOn);
		}
		if (this.audioListener != null && SuperController.singleton != null)
		{
			if (this.useAudioListener && this.cameraOn)
			{
				SuperController.singleton.PushAudioListener(this.audioListener);
			}
			else
			{
				SuperController.singleton.RemoveAudioListener(this.audioListener);
			}
		}
	}

	// Token: 0x06005340 RID: 21312 RVA: 0x001E2906 File Offset: 0x001E0D06
	protected void SyncCameraOn(bool b)
	{
		this.cameraOn = b;
		this.SyncCameraOnAndUseAudioListener();
	}

	// Token: 0x06005341 RID: 21313 RVA: 0x001E2915 File Offset: 0x001E0D15
	protected void SyncUseAudioListener(bool b)
	{
		this.useAudioListener = b;
		this.SyncCameraOnAndUseAudioListener();
	}

	// Token: 0x06005342 RID: 21314 RVA: 0x001E2924 File Offset: 0x001E0D24
	protected void SyncUseAsMainCamera(bool b)
	{
		this.useAsMainCamera = b;
		if (this.cameraToControl != null)
		{
			if (this.useAsMainCamera)
			{
				this.cameraToControl.tag = "MainCamera";
			}
			else
			{
				this.cameraToControl.tag = "Untagged";
			}
		}
	}

	// Token: 0x06005343 RID: 21315 RVA: 0x001E297C File Offset: 0x001E0D7C
	protected void SetMaskSelection(string maskSel)
	{
		try
		{
			CameraControl.MaskToShow maskSelection = (CameraControl.MaskToShow)Enum.Parse(typeof(CameraControl.MaskToShow), maskSel);
			this._maskSelection = maskSelection;
			this.SyncCamera();
		}
		catch (ArgumentException)
		{
			Debug.LogError("Attempted to set mask type to " + maskSel + " which is not a valid mask type");
		}
	}

	// Token: 0x17000C1F RID: 3103
	// (get) Token: 0x06005344 RID: 21316 RVA: 0x001E29DC File Offset: 0x001E0DDC
	// (set) Token: 0x06005345 RID: 21317 RVA: 0x001E29E4 File Offset: 0x001E0DE4
	public CameraControl.MaskToShow maskSelection
	{
		get
		{
			return this._maskSelection;
		}
		set
		{
			if (this.maskSelectionJSON != null)
			{
				this.maskSelectionJSON.val = value.ToString();
			}
			else if (this._maskSelection != value)
			{
				this.SetMaskSelection(value.ToString());
			}
		}
	}

	// Token: 0x06005346 RID: 21318 RVA: 0x001E2A38 File Offset: 0x001E0E38
	protected void SyncCameraFOV(float f)
	{
		this._cameraFOV = f;
		this.SyncCamera();
	}

	// Token: 0x17000C20 RID: 3104
	// (get) Token: 0x06005347 RID: 21319 RVA: 0x001E2A47 File Offset: 0x001E0E47
	// (set) Token: 0x06005348 RID: 21320 RVA: 0x001E2A4F File Offset: 0x001E0E4F
	public float cameraFOV
	{
		get
		{
			return this._cameraFOV;
		}
		set
		{
			if (this.cameraFOVJSON != null)
			{
				this.cameraFOVJSON.val = value;
			}
			else if (this._cameraFOV != value)
			{
				this.SyncCameraFOV(value);
			}
		}
	}

	// Token: 0x06005349 RID: 21321 RVA: 0x001E2A80 File Offset: 0x001E0E80
	protected void SyncShowHUDView(bool b)
	{
		this._showHUDView = b;
		if (this.HUDView != null)
		{
			this.HUDView.SetActive(this._showHUDView);
		}
	}

	// Token: 0x17000C21 RID: 3105
	// (get) Token: 0x0600534A RID: 21322 RVA: 0x001E2AAB File Offset: 0x001E0EAB
	// (set) Token: 0x0600534B RID: 21323 RVA: 0x001E2AB3 File Offset: 0x001E0EB3
	public bool showHUDView
	{
		get
		{
			return this._showHUDView;
		}
		set
		{
			if (this.showHUDViewJSON != null)
			{
				this.showHUDViewJSON.val = value;
			}
			else if (this._showHUDView != value)
			{
				this.SyncShowHUDView(value);
			}
		}
	}

	// Token: 0x0600534C RID: 21324 RVA: 0x001E2AE4 File Offset: 0x001E0EE4
	protected void Init()
	{
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		if (this.mask1Name != null && this.mask1Name != string.Empty)
		{
			list.Add("mask1");
			list2.Add(this.mask1Name);
		}
		if (this.mask2Name != null && this.mask2Name != string.Empty)
		{
			list.Add("mask2");
			list2.Add(this.mask2Name);
		}
		if (this.mask3Name != null && this.mask3Name != string.Empty)
		{
			list.Add("mask3");
			list2.Add(this.mask3Name);
		}
		if (this.mask4Name != null && this.mask4Name != string.Empty)
		{
			list.Add("mask4");
			list2.Add(this.mask4Name);
		}
		this.cameraOnJSON = new JSONStorableBool("cameraOn", this.cameraOn, new JSONStorableBool.SetBoolCallback(this.SyncCameraOn));
		this.cameraOnJSON.storeType = JSONStorableParam.StoreType.Any;
		base.RegisterBool(this.cameraOnJSON);
		this.useAudioListenerJSON = new JSONStorableBool("useAudioListener", this.useAudioListener, new JSONStorableBool.SetBoolCallback(this.SyncUseAudioListener));
		this.useAudioListenerJSON.storeType = JSONStorableParam.StoreType.Any;
		base.RegisterBool(this.useAudioListenerJSON);
		this.useAsMainCameraJSON = new JSONStorableBool("useAsMainCamera", this.useAsMainCamera, new JSONStorableBool.SetBoolCallback(this.SyncUseAsMainCamera));
		this.useAsMainCameraJSON.storeType = JSONStorableParam.StoreType.Any;
		base.RegisterBool(this.useAsMainCameraJSON);
		this.SyncCameraOnAndUseAudioListener();
		this.maskSelectionJSON = new JSONStorableStringChooser("maskSelection", list, list2, this._maskSelection.ToString(), null, new JSONStorableStringChooser.SetStringCallback(this.SetMaskSelection));
		this.maskSelectionJSON.storeType = JSONStorableParam.StoreType.Any;
		base.RegisterStringChooser(this.maskSelectionJSON);
		this.cameraFOVJSON = new JSONStorableFloat("FOV", this._cameraFOV, new JSONStorableFloat.SetFloatCallback(this.SyncCameraFOV), 10f, 100f, true, true);
		this.cameraFOVJSON.storeType = JSONStorableParam.StoreType.Any;
		base.RegisterFloat(this.cameraFOVJSON);
		this.SyncCamera();
		this.showHUDViewJSON = new JSONStorableBool("showHUDView", this._showHUDView, new JSONStorableBool.SetBoolCallback(this.SyncShowHUDView));
		this.showHUDViewJSON.storeType = JSONStorableParam.StoreType.Any;
		base.RegisterBool(this.showHUDViewJSON);
		this.SyncShowHUDView(this._showHUDView);
	}

	// Token: 0x0600534D RID: 21325 RVA: 0x001E2D68 File Offset: 0x001E1168
	public override void InitUI()
	{
		base.InitUI();
		if (this.UITransform != null)
		{
			CameraControlUI componentInChildren = this.UITransform.GetComponentInChildren<CameraControlUI>(true);
			if (componentInChildren != null)
			{
				this.cameraOnJSON.toggle = componentInChildren.cameraOnToggle;
				this.useAudioListenerJSON.toggle = componentInChildren.useAudioListenerToggle;
				this.useAsMainCameraJSON.toggle = componentInChildren.useAsMainCameraToggle;
				this.maskSelectionJSON.popup = componentInChildren.maskPopup;
				this.cameraFOVJSON.slider = componentInChildren.FOVSlider;
				this.showHUDViewJSON.toggle = componentInChildren.showHUDViewToggle;
			}
		}
	}

	// Token: 0x0600534E RID: 21326 RVA: 0x001E2E0C File Offset: 0x001E120C
	public override void InitUIAlt()
	{
		base.InitUIAlt();
		if (this.UITransformAlt != null)
		{
			CameraControlUI componentInChildren = this.UITransformAlt.GetComponentInChildren<CameraControlUI>(true);
			if (componentInChildren != null)
			{
				this.cameraOnJSON.toggleAlt = componentInChildren.cameraOnToggle;
				this.useAudioListenerJSON.toggleAlt = componentInChildren.useAudioListenerToggle;
				this.useAsMainCameraJSON.toggleAlt = componentInChildren.useAsMainCameraToggle;
				this.maskSelectionJSON.popupAlt = componentInChildren.maskPopup;
				this.cameraFOVJSON.sliderAlt = componentInChildren.FOVSlider;
				this.showHUDViewJSON.toggleAlt = componentInChildren.showHUDViewToggle;
			}
		}
	}

	// Token: 0x0600534F RID: 21327 RVA: 0x001E2EAF File Offset: 0x001E12AF
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

	// Token: 0x06005350 RID: 21328 RVA: 0x001E2ED4 File Offset: 0x001E12D4
	protected void OnDestroy()
	{
		if (this.useAudioListener && this.audioListener != null && SuperController.singleton != null)
		{
			SuperController.singleton.RemoveAudioListener(this.audioListener);
		}
	}

	// Token: 0x04004326 RID: 17190
	public Camera cameraToControl;

	// Token: 0x04004327 RID: 17191
	public Camera cameraToControl2;

	// Token: 0x04004328 RID: 17192
	public LayerMask mask1;

	// Token: 0x04004329 RID: 17193
	public LayerMask mask2;

	// Token: 0x0400432A RID: 17194
	public LayerMask mask3;

	// Token: 0x0400432B RID: 17195
	public LayerMask mask4;

	// Token: 0x0400432C RID: 17196
	public string mask1Name;

	// Token: 0x0400432D RID: 17197
	public string mask2Name;

	// Token: 0x0400432E RID: 17198
	public string mask3Name;

	// Token: 0x0400432F RID: 17199
	public string mask4Name;

	// Token: 0x04004330 RID: 17200
	public Transform cameraGroup;

	// Token: 0x04004331 RID: 17201
	public GameObject otherObjectToControl;

	// Token: 0x04004332 RID: 17202
	protected bool cameraOn;

	// Token: 0x04004333 RID: 17203
	protected JSONStorableBool cameraOnJSON;

	// Token: 0x04004334 RID: 17204
	public AudioListener audioListener;

	// Token: 0x04004335 RID: 17205
	public bool useAudioListener;

	// Token: 0x04004336 RID: 17206
	protected JSONStorableBool useAudioListenerJSON;

	// Token: 0x04004337 RID: 17207
	public bool useAsMainCamera;

	// Token: 0x04004338 RID: 17208
	protected JSONStorableBool useAsMainCameraJSON;

	// Token: 0x04004339 RID: 17209
	protected JSONStorableStringChooser maskSelectionJSON;

	// Token: 0x0400433A RID: 17210
	[SerializeField]
	protected CameraControl.MaskToShow _maskSelection;

	// Token: 0x0400433B RID: 17211
	protected JSONStorableFloat cameraFOVJSON;

	// Token: 0x0400433C RID: 17212
	[SerializeField]
	private float _cameraFOV = 40f;

	// Token: 0x0400433D RID: 17213
	public GameObject HUDView;

	// Token: 0x0400433E RID: 17214
	protected JSONStorableBool showHUDViewJSON;

	// Token: 0x0400433F RID: 17215
	[SerializeField]
	protected bool _showHUDView;

	// Token: 0x02000B8C RID: 2956
	public enum MaskToShow
	{
		// Token: 0x04004341 RID: 17217
		mask1,
		// Token: 0x04004342 RID: 17218
		mask2,
		// Token: 0x04004343 RID: 17219
		mask3,
		// Token: 0x04004344 RID: 17220
		mask4
	}
}
