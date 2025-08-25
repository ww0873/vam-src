using System;
using System.Collections.Generic;
using MeshVR.Hands;
using UnityEngine;

// Token: 0x02000D78 RID: 3448
public class HandControl : JSONStorable
{
	// Token: 0x06006A3F RID: 27199 RVA: 0x00280903 File Offset: 0x0027ED03
	public HandControl()
	{
	}

	// Token: 0x06006A40 RID: 27200 RVA: 0x00280912 File Offset: 0x0027ED12
	public void SyncHandGraspStrength(float f)
	{
		if (this.handJointSprings != null)
		{
			this.handJointSprings.percent = f;
		}
	}

	// Token: 0x06006A41 RID: 27201 RVA: 0x00280934 File Offset: 0x0027ED34
	protected void SyncFingerControl()
	{
		if ((this._possessed && this._allowPossessFingerControl) || this._fingerControlMode != HandControl.FingerControlMode.Morphs)
		{
			this.thumbProximalBone.rotationMorphsEnabled = false;
			this.thumbMiddlaBone.rotationMorphsEnabled = false;
			this.thumbDistalBone.rotationMorphsEnabled = false;
			this.indexProximalBone.rotationMorphsEnabled = false;
			this.indexMiddlaBone.rotationMorphsEnabled = false;
			this.indexDistalBone.rotationMorphsEnabled = false;
			this.middleProximalBone.rotationMorphsEnabled = false;
			this.middleMiddlaBone.rotationMorphsEnabled = false;
			this.middleDistalBone.rotationMorphsEnabled = false;
			this.ringProximalBone.rotationMorphsEnabled = false;
			this.ringMiddlaBone.rotationMorphsEnabled = false;
			this.ringDistalBone.rotationMorphsEnabled = false;
			this.pinkyProximalBone.rotationMorphsEnabled = false;
			this.pinkyMiddlaBone.rotationMorphsEnabled = false;
			this.pinkyDistalBone.rotationMorphsEnabled = false;
			if (this.handOutput != null)
			{
				this.handOutput.outputConnected = true;
				this.handOutput.inputConnected = ((this._possessed && this._allowPossessFingerControl) || this._fingerControlMode == HandControl.FingerControlMode.Remote);
			}
		}
		else
		{
			if (this.handOutput != null)
			{
				this.handOutput.outputConnected = false;
				this.handOutput.inputConnected = false;
			}
			this.thumbProximalBone.rotationMorphsEnabled = true;
			this.thumbMiddlaBone.rotationMorphsEnabled = true;
			this.thumbDistalBone.rotationMorphsEnabled = true;
			this.indexProximalBone.rotationMorphsEnabled = true;
			this.indexMiddlaBone.rotationMorphsEnabled = true;
			this.indexDistalBone.rotationMorphsEnabled = true;
			this.middleProximalBone.rotationMorphsEnabled = true;
			this.middleMiddlaBone.rotationMorphsEnabled = true;
			this.middleDistalBone.rotationMorphsEnabled = true;
			this.ringProximalBone.rotationMorphsEnabled = true;
			this.ringMiddlaBone.rotationMorphsEnabled = true;
			this.ringDistalBone.rotationMorphsEnabled = true;
			this.pinkyProximalBone.rotationMorphsEnabled = true;
			this.pinkyMiddlaBone.rotationMorphsEnabled = true;
			this.pinkyDistalBone.rotationMorphsEnabled = true;
		}
	}

	// Token: 0x06006A42 RID: 27202 RVA: 0x00280B44 File Offset: 0x0027EF44
	protected void SyncFingerControlMode(string choice)
	{
		try
		{
			this._fingerControlMode = (HandControl.FingerControlMode)Enum.Parse(typeof(HandControl.FingerControlMode), choice);
			this.SyncFingerControl();
		}
		catch (ArgumentException)
		{
			Debug.LogError("Attempted to set finger control mode to " + choice + " which is not a valid finger control mode");
		}
	}

	// Token: 0x17000F9C RID: 3996
	// (get) Token: 0x06006A43 RID: 27203 RVA: 0x00280BA4 File Offset: 0x0027EFA4
	// (set) Token: 0x06006A44 RID: 27204 RVA: 0x00280BAC File Offset: 0x0027EFAC
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
				this.SyncFingerControl();
			}
		}
	}

	// Token: 0x06006A45 RID: 27205 RVA: 0x00280BC7 File Offset: 0x0027EFC7
	protected void SyncAllowPossessFingerControl(bool b)
	{
		this._allowPossessFingerControl = b;
		this.SyncFingerControl();
	}

	// Token: 0x06006A46 RID: 27206 RVA: 0x00280BD8 File Offset: 0x0027EFD8
	protected override void InitUI(Transform t, bool isAlt)
	{
		if (t != null)
		{
			HandControlUI componentInChildren = t.GetComponentInChildren<HandControlUI>(true);
			if (componentInChildren != null)
			{
				this.handGraspStrengthJSON.RegisterSlider(componentInChildren.handGraspStrengthSlider, isAlt);
				this.fingerControlModeJSON.RegisterPopup(componentInChildren.fingerControlModePopup, isAlt);
				this.allowPossessFingerControlJSON.RegisterToggle(componentInChildren.allowPossessFingerControlToggle, isAlt);
			}
		}
	}

	// Token: 0x06006A47 RID: 27207 RVA: 0x00280C3C File Offset: 0x0027F03C
	protected void Init()
	{
		this.handOutput = base.GetComponent<HandOutput>();
		this.handGraspStrengthJSON = new JSONStorableFloat("handGraspStrength", 0.2f, new JSONStorableFloat.SetFloatCallback(this.SyncHandGraspStrength), 0f, 1f, true, true);
		this.handGraspStrengthJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterFloat(this.handGraspStrengthJSON);
		List<string> choicesList = new List<string>(Enum.GetNames(typeof(HandControl.FingerControlMode)));
		this.fingerControlModeJSON = new JSONStorableStringChooser("fingerControlMode", choicesList, this._fingerControlMode.ToString(), "Finger Control Mode", new JSONStorableStringChooser.SetStringCallback(this.SyncFingerControlMode));
		this.fingerControlModeJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterStringChooser(this.fingerControlModeJSON);
		this.allowPossessFingerControlJSON = new JSONStorableBool("allowPossessFingerControl", this._allowPossessFingerControl, new JSONStorableBool.SetBoolCallback(this.SyncAllowPossessFingerControl));
		this.allowPossessFingerControlJSON.storeType = JSONStorableParam.StoreType.Physical;
		base.RegisterBool(this.allowPossessFingerControlJSON);
		this.SyncFingerControl();
	}

	// Token: 0x06006A48 RID: 27208 RVA: 0x00280D3A File Offset: 0x0027F13A
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

	// Token: 0x04005C38 RID: 23608
	public AdjustJointSprings handJointSprings;

	// Token: 0x04005C39 RID: 23609
	public JSONStorableFloat handGraspStrengthJSON;

	// Token: 0x04005C3A RID: 23610
	protected HandOutput handOutput;

	// Token: 0x04005C3B RID: 23611
	[SerializeField]
	protected HandControl.FingerControlMode _fingerControlMode;

	// Token: 0x04005C3C RID: 23612
	public JSONStorableStringChooser fingerControlModeJSON;

	// Token: 0x04005C3D RID: 23613
	protected bool _possessed;

	// Token: 0x04005C3E RID: 23614
	[SerializeField]
	protected bool _allowPossessFingerControl = true;

	// Token: 0x04005C3F RID: 23615
	public JSONStorableBool allowPossessFingerControlJSON;

	// Token: 0x04005C40 RID: 23616
	public DAZBone thumbProximalBone;

	// Token: 0x04005C41 RID: 23617
	public DAZBone thumbMiddlaBone;

	// Token: 0x04005C42 RID: 23618
	public DAZBone thumbDistalBone;

	// Token: 0x04005C43 RID: 23619
	public DAZBone indexProximalBone;

	// Token: 0x04005C44 RID: 23620
	public DAZBone indexMiddlaBone;

	// Token: 0x04005C45 RID: 23621
	public DAZBone indexDistalBone;

	// Token: 0x04005C46 RID: 23622
	public DAZBone middleProximalBone;

	// Token: 0x04005C47 RID: 23623
	public DAZBone middleMiddlaBone;

	// Token: 0x04005C48 RID: 23624
	public DAZBone middleDistalBone;

	// Token: 0x04005C49 RID: 23625
	public DAZBone ringProximalBone;

	// Token: 0x04005C4A RID: 23626
	public DAZBone ringMiddlaBone;

	// Token: 0x04005C4B RID: 23627
	public DAZBone ringDistalBone;

	// Token: 0x04005C4C RID: 23628
	public DAZBone pinkyProximalBone;

	// Token: 0x04005C4D RID: 23629
	public DAZBone pinkyMiddlaBone;

	// Token: 0x04005C4E RID: 23630
	public DAZBone pinkyDistalBone;

	// Token: 0x02000D79 RID: 3449
	public enum FingerControlMode
	{
		// Token: 0x04005C50 RID: 23632
		Morphs,
		// Token: 0x04005C51 RID: 23633
		JSONParams,
		// Token: 0x04005C52 RID: 23634
		Remote
	}
}
