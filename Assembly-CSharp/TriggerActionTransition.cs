using System;
using SimpleJSON;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000DAE RID: 3502
public class TriggerActionTransition : TriggerAction
{
	// Token: 0x06006C85 RID: 27781 RVA: 0x0028EBBE File Offset: 0x0028CFBE
	public TriggerActionTransition()
	{
	}

	// Token: 0x06006C86 RID: 27782 RVA: 0x0028EBC6 File Offset: 0x0028CFC6
	public void CopyData()
	{
		TriggerActionTransition.copyOfData = this.GetJSON();
	}

	// Token: 0x06006C87 RID: 27783 RVA: 0x0028EBD3 File Offset: 0x0028CFD3
	public void PasteData()
	{
		if (TriggerActionTransition.copyOfData != null)
		{
			this.RestoreFromJSON(TriggerActionTransition.copyOfData);
		}
	}

	// Token: 0x06006C88 RID: 27784 RVA: 0x0028EBF0 File Offset: 0x0028CFF0
	public override JSONClass GetJSON(string subScenePrefix = null)
	{
		this.CheckMissingReceiver();
		JSONClass json = base.GetJSON(subScenePrefix);
		if (json != null)
		{
			JSONStorable.Type actionType = this.actionType;
			if (actionType != JSONStorable.Type.Float)
			{
				if (actionType == JSONStorable.Type.Color)
				{
					json["startColor"]["h"].AsFloat = this._startHSVColorH;
					json["startColor"]["s"].AsFloat = this._startHSVColorS;
					json["startColor"]["v"].AsFloat = this._startHSVColorV;
					json["endColor"]["h"].AsFloat = this._endHSVColorH;
					json["endColor"]["s"].AsFloat = this._endHSVColorS;
					json["endColor"]["v"].AsFloat = this._endHSVColorV;
				}
			}
			else
			{
				json["startValue"].AsFloat = this._startVal;
				json["endValue"].AsFloat = this._endVal;
			}
			json["startWithCurrentVal"].AsBool = this._startWithCurrentVal;
		}
		return json;
	}

	// Token: 0x06006C89 RID: 27785 RVA: 0x0028ED40 File Offset: 0x0028D140
	public override void RestoreFromJSON(JSONClass jc, string subScenePrefix = null)
	{
		base.RestoreFromJSON(jc, subScenePrefix);
		if (jc["startValue"] != null)
		{
			this.startVal = jc["startValue"].AsFloat;
		}
		if (jc["endValue"] != null)
		{
			this.endVal = jc["endValue"].AsFloat;
		}
		if (jc["startColor"] != null)
		{
			float h = this._startHSVColorH;
			float s = this._startHSVColorS;
			float v = this._startHSVColorV;
			if (jc["startColor"]["h"] != null)
			{
				h = jc["startColor"]["h"].AsFloat;
			}
			if (jc["startColor"]["s"] != null)
			{
				s = jc["startColor"]["s"].AsFloat;
			}
			if (jc["startColor"]["v"] != null)
			{
				v = jc["startColor"]["v"].AsFloat;
			}
			this.SetStartColorFromHSV(h, s, v);
		}
		if (jc["endColor"] != null)
		{
			float h2 = this._startHSVColorH;
			float s2 = this._startHSVColorS;
			float v2 = this._startHSVColorV;
			if (jc["endColor"]["h"] != null)
			{
				h2 = jc["endColor"]["h"].AsFloat;
			}
			if (jc["endColor"]["s"] != null)
			{
				s2 = jc["endColor"]["s"].AsFloat;
			}
			if (jc["endColor"]["v"] != null)
			{
				v2 = jc["endColor"]["v"].AsFloat;
			}
			this.SetEndColorFromHSV(h2, s2, v2);
		}
		if (jc["startWithCurrentVal"] != null)
		{
			this.startWithCurrentVal = jc["startWithCurrentVal"].AsBool;
		}
		this.AutoSetPreviewTextAndName();
	}

	// Token: 0x06006C8A RID: 27786 RVA: 0x0028EFAF File Offset: 0x0028D3AF
	protected override void CreateTriggerActionPanel()
	{
		if (this.handler != null)
		{
			this.triggerActionPanel = this.handler.CreateTriggerActionTransitionUI();
			this.InitTriggerActionPanelUI();
		}
		else
		{
			Debug.LogError("Attempted to CreateTriggerActionPanel when handler was null");
		}
	}

	// Token: 0x06006C8B RID: 27787 RVA: 0x0028EFE4 File Offset: 0x0028D3E4
	protected override void AutoSetPreviewText()
	{
		if (this._receiverAtom != null && !string.IsNullOrEmpty(this._receiverStoreId) && !string.IsNullOrEmpty(this._receiverTargetName))
		{
			JSONStorable.Type actionType = this._actionType;
			if (actionType != JSONStorable.Type.Float)
			{
				if (actionType != JSONStorable.Type.Color)
				{
					base.AutoSetPreviewText();
				}
				else
				{
					this.AutoSetPreviewTextFromColorParam();
				}
			}
			else
			{
				this.AutoSetPreviewTextFromFloatParam();
			}
		}
	}

	// Token: 0x06006C8C RID: 27788 RVA: 0x0028F060 File Offset: 0x0028D460
	protected void AutoSetPreviewTextFromFloatParam()
	{
		base.previewText = string.Concat(new string[]
		{
			this._receiverAtom.uid,
			":",
			this._receiverStoreId,
			":",
			this._receiverTargetName,
			":",
			this.startVal.ToString("F2"),
			"_",
			this.endVal.ToString("F2")
		});
	}

	// Token: 0x06006C8D RID: 27789 RVA: 0x0028F0EC File Offset: 0x0028D4EC
	protected void AutoSetPreviewTextFromColorParam()
	{
		if (this.startColorPicker != null && this.endColorPicker != null)
		{
			base.previewText = string.Concat(new string[]
			{
				this._receiverAtom.uid,
				":",
				this._receiverStoreId,
				":",
				this._receiverTargetName,
				":",
				this.startColorPicker.red255string,
				"_",
				this.startColorPicker.green255string,
				"_",
				this.startColorPicker.blue255string,
				"__",
				this.endColorPicker.red255string,
				"_",
				this.endColorPicker.green255string,
				"_",
				this.endColorPicker.blue255string
			});
		}
	}

	// Token: 0x06006C8E RID: 27790 RVA: 0x0028F1EC File Offset: 0x0028D5EC
	protected override void AutoSetName()
	{
		if (base.receiver != null && !string.IsNullOrEmpty(this._receiverTargetName))
		{
			JSONStorable.Type actionType = this._actionType;
			if (actionType != JSONStorable.Type.Float)
			{
				if (actionType != JSONStorable.Type.Color)
				{
					base.AutoSetName();
				}
				else
				{
					this.AutoSetNameFromColorParam();
				}
			}
			else
			{
				this.AutoSetNameFromFloatParam();
			}
		}
	}

	// Token: 0x06006C8F RID: 27791 RVA: 0x0028F258 File Offset: 0x0028D658
	protected void AutoSetNameFromFloatParam()
	{
		base.name = string.Concat(new string[]
		{
			"A_",
			this._receiverTargetName,
			":",
			this.startVal.ToString("F2"),
			"_",
			this.endVal.ToString("F2")
		});
	}

	// Token: 0x06006C90 RID: 27792 RVA: 0x0028F2C4 File Offset: 0x0028D6C4
	protected void AutoSetNameFromColorParam()
	{
		if (this.startColorPicker != null && this.endColorPicker != null)
		{
			base.name = string.Concat(new string[]
			{
				"A_",
				this._receiverTargetName,
				":",
				this.startColorPicker.red255string,
				"_",
				this.startColorPicker.green255string,
				"_",
				this.startColorPicker.blue255string,
				"__",
				this.endColorPicker.red255string,
				"_",
				this.endColorPicker.green255string,
				"_",
				this.endColorPicker.blue255string
			});
		}
	}

	// Token: 0x06006C91 RID: 27793 RVA: 0x0028F39F File Offset: 0x0028D79F
	public void SetStartWithCurrentVal(bool b)
	{
		this._startWithCurrentVal = b;
	}

	// Token: 0x17000FF1 RID: 4081
	// (get) Token: 0x06006C92 RID: 27794 RVA: 0x0028F3A8 File Offset: 0x0028D7A8
	// (set) Token: 0x06006C93 RID: 27795 RVA: 0x0028F3B0 File Offset: 0x0028D7B0
	public bool startWithCurrentVal
	{
		get
		{
			return this._startWithCurrentVal;
		}
		set
		{
			if (this._startWithCurrentVal != value)
			{
				this._startWithCurrentVal = value;
				if (this.startWithCurrentValToggle != null)
				{
					this.startWithCurrentValToggle.isOn = this._startWithCurrentVal;
				}
			}
		}
	}

	// Token: 0x06006C94 RID: 27796 RVA: 0x0028F3E8 File Offset: 0x0028D7E8
	protected void SyncStartValDynamicSlider()
	{
		if (this.startValSlider != null)
		{
			this.startValSlider.minValue = this.paramMin;
			this.startValSlider.maxValue = this.paramMax;
			SliderControl component = this.startValSlider.GetComponent<SliderControl>();
			if (this.receiverTargetFloat != null && component != null)
			{
				component.defaultValue = this.receiverTargetFloat.defaultVal;
				component.clamp = this.paramContrained;
			}
		}
		if (this.startValDynamicSlider != null)
		{
			this.startValDynamicSlider.rangeAdjustEnabled = !this.paramContrained;
			if (this.paramMax <= 2f)
			{
				this.startValDynamicSlider.valueFormat = "F3";
			}
			else if (this.paramMax <= 20f)
			{
				this.startValDynamicSlider.valueFormat = "F2";
			}
			else if (this.paramMax <= 200f)
			{
				this.startValDynamicSlider.valueFormat = "F1";
			}
			else
			{
				this.startValDynamicSlider.valueFormat = "F0";
			}
			if (this.receiverTargetFloat != null)
			{
				this.startValDynamicSlider.label = "(Start) " + this.receiverTargetFloat.name;
			}
		}
	}

	// Token: 0x06006C95 RID: 27797 RVA: 0x0028F537 File Offset: 0x0028D937
	public void SetStartVal(float f)
	{
		this._startVal = f;
		this.CheckStartValConstraint(false);
		this.TriggerInterp(this._currentInterp, false);
		this.AutoSetPreviewTextAndName();
	}

	// Token: 0x06006C96 RID: 27798 RVA: 0x0028F55C File Offset: 0x0028D95C
	protected void CheckStartValConstraint(bool skipSync = false)
	{
		if (!this.paramContrained)
		{
			bool flag = false;
			if (this._startVal < this.paramMin)
			{
				this.paramMin = this._startVal;
				flag = true;
			}
			if (this._startVal > this.paramMax)
			{
				this.paramMax = this._startVal;
				flag = true;
			}
			if (flag && !skipSync)
			{
				this.SyncStartValDynamicSlider();
				this.SyncEndValDynamicSlider();
			}
		}
	}

	// Token: 0x17000FF2 RID: 4082
	// (get) Token: 0x06006C97 RID: 27799 RVA: 0x0028F5CC File Offset: 0x0028D9CC
	// (set) Token: 0x06006C98 RID: 27800 RVA: 0x0028F5D4 File Offset: 0x0028D9D4
	public float startVal
	{
		get
		{
			return this._startVal;
		}
		set
		{
			if (this._startVal != value)
			{
				this._startVal = value;
				this.CheckStartValConstraint(false);
				if (this.startValSlider != null)
				{
					this.startValSlider.value = this._startVal;
				}
				this.AutoSetPreviewTextAndName();
			}
		}
	}

	// Token: 0x06006C99 RID: 27801 RVA: 0x0028F624 File Offset: 0x0028DA24
	protected void SyncEndValDynamicSlider()
	{
		if (this.endValSlider != null)
		{
			this.endValSlider.minValue = this.paramMin;
			this.endValSlider.maxValue = this.paramMax;
			SliderControl component = this.endValSlider.GetComponent<SliderControl>();
			if (this.receiverTargetFloat != null && component != null)
			{
				component.defaultValue = this.receiverTargetFloat.defaultVal;
				component.clamp = this.paramContrained;
			}
		}
		if (this.endValDynamicSlider != null)
		{
			this.endValDynamicSlider.rangeAdjustEnabled = !this.paramContrained;
			if (this.paramMax <= 2f)
			{
				this.endValDynamicSlider.valueFormat = "F3";
			}
			else if (this.paramMax <= 20f)
			{
				this.endValDynamicSlider.valueFormat = "F2";
			}
			else if (this.paramMax <= 200f)
			{
				this.endValDynamicSlider.valueFormat = "F1";
			}
			else
			{
				this.endValDynamicSlider.valueFormat = "F0";
			}
			if (this.receiverTargetFloat != null)
			{
				this.endValDynamicSlider.label = "(End) " + this.receiverTargetFloat.name;
			}
		}
	}

	// Token: 0x06006C9A RID: 27802 RVA: 0x0028F773 File Offset: 0x0028DB73
	public void SetEndVal(float f)
	{
		this._endVal = f;
		this.CheckEndValConstraint(false);
		this.TriggerInterp(this._currentInterp, false);
		this.AutoSetPreviewTextAndName();
	}

	// Token: 0x06006C9B RID: 27803 RVA: 0x0028F798 File Offset: 0x0028DB98
	protected void CheckEndValConstraint(bool skipSync = false)
	{
		if (!this.paramContrained)
		{
			bool flag = false;
			if (this._endVal < this.paramMin)
			{
				this.paramMin = this._endVal;
				flag = true;
			}
			if (this._endVal > this.paramMax)
			{
				this.paramMax = this._endVal;
				flag = true;
			}
			if (flag && !skipSync)
			{
				this.SyncStartValDynamicSlider();
				this.SyncEndValDynamicSlider();
			}
		}
	}

	// Token: 0x17000FF3 RID: 4083
	// (get) Token: 0x06006C9C RID: 27804 RVA: 0x0028F808 File Offset: 0x0028DC08
	// (set) Token: 0x06006C9D RID: 27805 RVA: 0x0028F810 File Offset: 0x0028DC10
	public float endVal
	{
		get
		{
			return this._endVal;
		}
		set
		{
			if (this._endVal != value)
			{
				this._endVal = value;
				this.CheckEndValConstraint(false);
				if (this.endValSlider != null)
				{
					this.endValSlider.value = this._endVal;
				}
				this.AutoSetPreviewTextAndName();
			}
		}
	}

	// Token: 0x06006C9E RID: 27806 RVA: 0x0028F860 File Offset: 0x0028DC60
	protected virtual void SetStartColorFromHSV(float h, float s, float v)
	{
		if (this._startHSVColorH != h || this._startHSVColorS != s || this._startHSVColorV != v)
		{
			this._startHSVColorH = h;
			this._startHSVColorS = s;
			this._startHSVColorV = v;
			if (this.startColorPicker != null)
			{
				this.startColorPicker.SetHSV(h, s, v, true);
			}
			this.TriggerInterp(this._currentInterp, false);
			this.AutoSetPreviewTextAndName();
		}
	}

	// Token: 0x06006C9F RID: 27807 RVA: 0x0028F8D9 File Offset: 0x0028DCD9
	protected virtual void SetEndColorFromHSV(float h, float s, float v)
	{
		this.SetEndColorFromHSV(h, s, v, false);
	}

	// Token: 0x06006CA0 RID: 27808 RVA: 0x0028F8E8 File Offset: 0x0028DCE8
	protected virtual void SetEndColorFromHSV(float h, float s, float v, bool force)
	{
		if (force || this._endHSVColorH != h || this._endHSVColorS != s || this._endHSVColorV != v)
		{
			this._endHSVColorH = h;
			this._endHSVColorS = s;
			this._endHSVColorV = v;
			if (this.endColorPicker != null)
			{
				this.endColorPicker.SetHSV(h, s, v, true);
			}
			this.TriggerInterp(this._currentInterp, false);
			this.AutoSetPreviewTextAndName();
		}
	}

	// Token: 0x06006CA1 RID: 27809 RVA: 0x0028F968 File Offset: 0x0028DD68
	protected override void SetReceiverTargetPopupNames()
	{
		this.receiverTargetNames = null;
		if (this._receiver != null)
		{
			this.receiverTargetNames = this._receiver.GetAllFloatAndColorParamNames();
		}
		this.SyncTargetPopupNames();
	}

	// Token: 0x06006CA2 RID: 27810 RVA: 0x0028F999 File Offset: 0x0028DD99
	public override void SetReceiverTargetNameAndSetInitialParams(string targetName)
	{
		this.disableTriggerInterp = true;
		base.SetReceiverTargetNameAndSetInitialParams(targetName);
		this.disableTriggerInterp = false;
	}

	// Token: 0x06006CA3 RID: 27811 RVA: 0x0028F9B0 File Offset: 0x0028DDB0
	protected override void SetInitialParamsFromReceiverTarget()
	{
		base.SetInitialParamsFromReceiverTarget();
		if (this._receiver != null && base.receiverTargetName != null)
		{
			JSONStorable.Type actionType = this._actionType;
			if (actionType != JSONStorable.Type.Float)
			{
				if (actionType == JSONStorable.Type.Color)
				{
					HSVColor colorParamValue = this._receiver.GetColorParamValue(this._receiverTargetName);
					this.SetStartColorFromHSV(colorParamValue.H, colorParamValue.S, colorParamValue.V);
					this.SetEndColorFromHSV(colorParamValue.H, colorParamValue.S, colorParamValue.V);
				}
			}
			else
			{
				this.startVal = this._receiver.GetFloatParamValue(this._receiverTargetName);
				this.endVal = this._receiver.GetFloatParamValue(this._receiverTargetName);
			}
		}
	}

	// Token: 0x06006CA4 RID: 27812 RVA: 0x0028FA7C File Offset: 0x0028DE7C
	protected override void SyncFromReceiverTarget()
	{
		base.SyncFromReceiverTarget();
		if (!this.receiverSetFromPopup)
		{
			this.CheckStartValConstraint(true);
			this.CheckEndValConstraint(true);
		}
		this.SyncStartValDynamicSlider();
		this.SyncEndValDynamicSlider();
		if (this._receiver != null && this._receiverTargetName != null)
		{
			this.actionType = this._receiver.GetParamOrActionType(this._receiverTargetName);
			this.AutoSetPreviewTextAndName();
		}
		else
		{
			this.actionType = JSONStorable.Type.None;
		}
	}

	// Token: 0x06006CA5 RID: 27813 RVA: 0x0028FAF9 File Offset: 0x0028DEF9
	protected override void CheckMissingReceiver()
	{
		base.CheckMissingReceiver();
		if (this._receiver != null && this._receiverTargetName != null && this._actionType == JSONStorable.Type.None)
		{
			this.SyncFromReceiverTarget();
		}
	}

	// Token: 0x06006CA6 RID: 27814 RVA: 0x0028FB30 File Offset: 0x0028DF30
	protected void SyncType()
	{
		if (this.startValSlider != null)
		{
			if (this._actionType == JSONStorable.Type.Float)
			{
				this.startValSlider.transform.parent.gameObject.SetActive(true);
			}
			else
			{
				this.startValSlider.transform.parent.gameObject.SetActive(false);
			}
		}
		if (this.endValSlider != null)
		{
			if (this._actionType == JSONStorable.Type.Float)
			{
				this.endValSlider.transform.parent.gameObject.SetActive(true);
			}
			else
			{
				this.endValSlider.transform.parent.gameObject.SetActive(false);
			}
		}
		if (this.startColorPickerContainer != null)
		{
			if (this._actionType == JSONStorable.Type.Color)
			{
				this.startColorPickerContainer.gameObject.SetActive(true);
			}
			else
			{
				this.startColorPickerContainer.gameObject.SetActive(false);
			}
		}
		if (this.endColorPickerContainer != null)
		{
			if (this._actionType == JSONStorable.Type.Color)
			{
				this.endColorPickerContainer.gameObject.SetActive(true);
			}
			else
			{
				this.endColorPickerContainer.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x17000FF4 RID: 4084
	// (get) Token: 0x06006CA7 RID: 27815 RVA: 0x0028FC75 File Offset: 0x0028E075
	// (set) Token: 0x06006CA8 RID: 27816 RVA: 0x0028FC7D File Offset: 0x0028E07D
	public JSONStorable.Type actionType
	{
		get
		{
			return this._actionType;
		}
		set
		{
			if (this._actionType != value)
			{
				this._actionType = value;
				this.SyncType();
			}
		}
	}

	// Token: 0x06006CA9 RID: 27817 RVA: 0x0028FC98 File Offset: 0x0028E098
	public void InitTriggerStart()
	{
		if (this._startWithCurrentVal)
		{
			this.CheckMissingReceiver();
			if (base.receiver != null && base.receiverTargetName != null)
			{
				if (this._actionType == JSONStorable.Type.Float)
				{
					this.startVal = base.receiver.GetFloatParamValue(base.receiverTargetName);
				}
				else if (this._actionType == JSONStorable.Type.Color)
				{
					HSVColor colorParamValue = base.receiver.GetColorParamValue(base.receiverTargetName);
					this.SetStartColorFromHSV(colorParamValue.H, colorParamValue.S, colorParamValue.V);
				}
			}
		}
	}

	// Token: 0x06006CAA RID: 27818 RVA: 0x0028FD34 File Offset: 0x0028E134
	public override void Validate()
	{
		this.CheckMissingReceiver();
		if (base.receiver != null && base.receiverTargetName != null)
		{
			JSONStorable.Type actionType = this._actionType;
			if (actionType != JSONStorable.Type.Float)
			{
				if (actionType == JSONStorable.Type.Color)
				{
					if (!base.receiver.IsColorJSONParam(base.receiverTargetName))
					{
						base.receiverTargetName = null;
					}
				}
			}
			else if (!base.receiver.IsFloatJSONParam(base.receiverTargetName))
			{
				base.receiverTargetName = null;
			}
		}
		base.Validate();
	}

	// Token: 0x06006CAB RID: 27819 RVA: 0x0028FDC7 File Offset: 0x0028E1C7
	protected void SetTestTransition(float f)
	{
		if (this._currentInterp != f)
		{
			this.TriggerInterp(f, true);
		}
	}

	// Token: 0x06006CAC RID: 27820 RVA: 0x0028FDE0 File Offset: 0x0028E1E0
	public void TriggerInterp(float interp, bool force = false)
	{
		if (base.enabled || force)
		{
			this._currentInterp = interp;
			if (this.testTransitionSlider != null)
			{
				this.testTransitionSlider.value = this._currentInterp;
			}
			if ((this.active || force) && !this.disableTriggerInterp)
			{
				this.CheckMissingReceiver();
				if (base.receiver != null && base.receiverTargetName != null)
				{
					if (this._actionType == JSONStorable.Type.Float)
					{
						float value = Mathf.Lerp(this.startVal, this.endVal, interp);
						base.receiver.SetFloatParamValue(base.receiverTargetName, value);
					}
					else if (this._actionType == JSONStorable.Type.Color)
					{
						this.interpColor.H = Mathf.Lerp(this._startHSVColorH, this._endHSVColorH, interp);
						this.interpColor.S = Mathf.Lerp(this._startHSVColorS, this._endHSVColorS, interp);
						this.interpColor.V = Mathf.Lerp(this._startHSVColorV, this._endHSVColorV, interp);
						base.receiver.SetColorParamValue(this._receiverTargetName, this.interpColor);
					}
				}
			}
		}
	}

	// Token: 0x06006CAD RID: 27821 RVA: 0x0028FF18 File Offset: 0x0028E318
	public override void InitTriggerActionPanelUI()
	{
		this.CheckMissingReceiver();
		base.InitTriggerActionPanelUI();
		if (this.triggerActionPanel != null)
		{
			TriggerActionTransitionUI component = this.triggerActionPanel.GetComponent<TriggerActionTransitionUI>();
			if (component != null)
			{
				this.copyButton = component.copyButton;
				this.pasteButton = component.pasteButton;
				this.testTransitionSlider = component.testTransitionSlider;
				this.startWithCurrentValToggle = component.startWithCurrentValToggle;
				this.startValSlider = component.startValSlider;
				this.startValDynamicSlider = component.startValDynamicSlider;
				this.endValSlider = component.endValSlider;
				this.endValDynamicSlider = component.endValDynamicSlider;
				this.startColorPickerContainer = component.startColorPickerContainer;
				this.startColorPicker = component.startColorPicker;
				this.endColorPickerContainer = component.endColorPickerContainer;
				this.endColorPicker = component.endColorPicker;
			}
		}
		if (this.copyButton != null)
		{
			this.copyButton.onClick.AddListener(new UnityAction(this.CopyData));
		}
		if (this.pasteButton != null)
		{
			this.pasteButton.onClick.AddListener(new UnityAction(this.PasteData));
		}
		if (this.testTransitionSlider != null)
		{
			this.testTransitionSlider.onValueChanged.AddListener(new UnityAction<float>(this.SetTestTransition));
		}
		if (this.startWithCurrentValToggle != null)
		{
			this.startWithCurrentValToggle.isOn = this._startWithCurrentVal;
			this.startWithCurrentValToggle.onValueChanged.AddListener(new UnityAction<bool>(this.SetStartWithCurrentVal));
		}
		if (this.startValSlider != null)
		{
			this.startValSlider.minValue = this.paramMin;
			if (this.startValSlider.minValue > this._startVal)
			{
				this.startValSlider.minValue = this._startVal;
			}
			this.startValSlider.maxValue = this.paramMax;
			if (this.startValSlider.maxValue < this._startVal)
			{
				this.startValSlider.maxValue = this._startVal;
			}
			this.startValSlider.value = this._startVal;
			this.startValSlider.onValueChanged.AddListener(new UnityAction<float>(this.SetStartVal));
		}
		if (this.endValSlider != null)
		{
			this.endValSlider.minValue = this.paramMin;
			if (this.endValSlider.minValue > this._endVal)
			{
				this.endValSlider.minValue = this._endVal;
			}
			this.endValSlider.maxValue = this.paramMax;
			if (this.endValSlider.maxValue < this._endVal)
			{
				this.endValSlider.maxValue = this._endVal;
			}
			this.endValSlider.value = this._endVal;
			this.endValSlider.onValueChanged.AddListener(new UnityAction<float>(this.SetEndVal));
		}
		if (this.startColorPicker != null)
		{
			this.startColorPicker.hue = this._startHSVColorH;
			this.startColorPicker.saturation = this._startHSVColorS;
			this.startColorPicker.cvalue = this._startHSVColorV;
			HSVColorPicker hsvcolorPicker = this.startColorPicker;
			hsvcolorPicker.onHSVColorChangedHandlers = (HSVColorPicker.OnHSVColorChanged)Delegate.Combine(hsvcolorPicker.onHSVColorChangedHandlers, new HSVColorPicker.OnHSVColorChanged(this.SetStartColorFromHSV));
		}
		if (this.endColorPicker != null)
		{
			this.endColorPicker.hue = this._endHSVColorH;
			this.endColorPicker.saturation = this._endHSVColorS;
			this.endColorPicker.cvalue = this._endHSVColorV;
			HSVColorPicker hsvcolorPicker2 = this.endColorPicker;
			hsvcolorPicker2.onHSVColorChangedHandlers = (HSVColorPicker.OnHSVColorChanged)Delegate.Combine(hsvcolorPicker2.onHSVColorChangedHandlers, new HSVColorPicker.OnHSVColorChanged(this.SetEndColorFromHSV));
		}
		this.SyncType();
		this.SyncStartValDynamicSlider();
		this.SyncEndValDynamicSlider();
	}

	// Token: 0x06006CAE RID: 27822 RVA: 0x002902F8 File Offset: 0x0028E6F8
	public override void DeregisterUI()
	{
		base.DeregisterUI();
		if (this.copyButton != null)
		{
			this.copyButton.onClick.RemoveListener(new UnityAction(this.CopyData));
		}
		if (this.pasteButton != null)
		{
			this.pasteButton.onClick.RemoveListener(new UnityAction(this.PasteData));
		}
		if (this.testTransitionSlider != null)
		{
			this.testTransitionSlider.onValueChanged.RemoveListener(new UnityAction<float>(this.SetTestTransition));
		}
		if (this.startWithCurrentValToggle != null)
		{
			this.startWithCurrentValToggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.SetStartWithCurrentVal));
		}
		if (this.startValSlider != null)
		{
			this.startValSlider.onValueChanged.RemoveListener(new UnityAction<float>(this.SetStartVal));
		}
		if (this.endValSlider != null)
		{
			this.endValSlider.onValueChanged.RemoveListener(new UnityAction<float>(this.SetEndVal));
		}
		if (this.startColorPicker != null)
		{
			HSVColorPicker hsvcolorPicker = this.startColorPicker;
			hsvcolorPicker.onHSVColorChangedHandlers = (HSVColorPicker.OnHSVColorChanged)Delegate.Remove(hsvcolorPicker.onHSVColorChangedHandlers, new HSVColorPicker.OnHSVColorChanged(this.SetStartColorFromHSV));
		}
		if (this.endColorPicker != null)
		{
			HSVColorPicker hsvcolorPicker2 = this.endColorPicker;
			hsvcolorPicker2.onHSVColorChangedHandlers = (HSVColorPicker.OnHSVColorChanged)Delegate.Remove(hsvcolorPicker2.onHSVColorChangedHandlers, new HSVColorPicker.OnHSVColorChanged(this.SetEndColorFromHSV));
		}
	}

	// Token: 0x04005E1D RID: 24093
	public static JSONClass copyOfData;

	// Token: 0x04005E1E RID: 24094
	protected Button copyButton;

	// Token: 0x04005E1F RID: 24095
	protected Button pasteButton;

	// Token: 0x04005E20 RID: 24096
	protected Toggle startWithCurrentValToggle;

	// Token: 0x04005E21 RID: 24097
	protected bool _startWithCurrentVal;

	// Token: 0x04005E22 RID: 24098
	protected UIDynamicSlider startValDynamicSlider;

	// Token: 0x04005E23 RID: 24099
	protected Slider startValSlider;

	// Token: 0x04005E24 RID: 24100
	protected float _startVal;

	// Token: 0x04005E25 RID: 24101
	protected UIDynamicSlider endValDynamicSlider;

	// Token: 0x04005E26 RID: 24102
	protected Slider endValSlider;

	// Token: 0x04005E27 RID: 24103
	protected float _endVal;

	// Token: 0x04005E28 RID: 24104
	protected RectTransform startColorPickerContainer;

	// Token: 0x04005E29 RID: 24105
	protected HSVColorPicker startColorPicker;

	// Token: 0x04005E2A RID: 24106
	protected float _startHSVColorH;

	// Token: 0x04005E2B RID: 24107
	protected float _startHSVColorS;

	// Token: 0x04005E2C RID: 24108
	protected float _startHSVColorV;

	// Token: 0x04005E2D RID: 24109
	protected RectTransform endColorPickerContainer;

	// Token: 0x04005E2E RID: 24110
	protected HSVColorPicker endColorPicker;

	// Token: 0x04005E2F RID: 24111
	protected float _endHSVColorH;

	// Token: 0x04005E30 RID: 24112
	protected float _endHSVColorS;

	// Token: 0x04005E31 RID: 24113
	protected float _endHSVColorV;

	// Token: 0x04005E32 RID: 24114
	protected JSONStorable.Type _actionType;

	// Token: 0x04005E33 RID: 24115
	protected HSVColor interpColor;

	// Token: 0x04005E34 RID: 24116
	protected Slider testTransitionSlider;

	// Token: 0x04005E35 RID: 24117
	public bool active;

	// Token: 0x04005E36 RID: 24118
	protected float _currentInterp;

	// Token: 0x04005E37 RID: 24119
	protected bool disableTriggerInterp;
}
