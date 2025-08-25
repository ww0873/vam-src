using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using MVR.FileManagement;
using SimpleJSON;
using uFileBrowser;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

// Token: 0x02000DA8 RID: 3496
public class TriggerActionDiscrete : TriggerAction
{
	// Token: 0x06006C1D RID: 27677 RVA: 0x0028AF34 File Offset: 0x00289334
	public TriggerActionDiscrete()
	{
	}

	// Token: 0x06006C1E RID: 27678 RVA: 0x0028AF64 File Offset: 0x00289364
	public void CopyData()
	{
		TriggerActionDiscrete.copyOfData = this.GetJSON();
	}

	// Token: 0x06006C1F RID: 27679 RVA: 0x0028AF71 File Offset: 0x00289371
	public void PasteData()
	{
		if (TriggerActionDiscrete.copyOfData != null)
		{
			this.RestoreFromJSON(TriggerActionDiscrete.copyOfData);
		}
	}

	// Token: 0x06006C20 RID: 27680 RVA: 0x0028AF90 File Offset: 0x00289390
	public override JSONClass GetJSON(string subScenePrefix = null)
	{
		this.CheckMissingReceiver();
		JSONClass json = base.GetJSON(subScenePrefix);
		if (json != null)
		{
			switch (this.actionType)
			{
			case JSONStorable.Type.Bool:
				json["boolValue"].AsBool = this._boolValue;
				break;
			case JSONStorable.Type.Float:
				json["floatValue"].AsFloat = this._floatValue;
				if (this._useTimer)
				{
					json["useTimer"].AsBool = this._useTimer;
					json["timerLength"].AsFloat = this._timerLength;
					json["timerType"] = this._timerType.ToString();
					if (this._useSecondTimerPoint)
					{
						json["useSecondTimerPoint"].AsBool = this._useSecondTimerPoint;
						json["secondTimerPointValue"].AsFloat = this._secondTimerPointValue;
						json["secondTimerPointCurveLocation"].AsFloat = this._secondTimerPointCurveLocation;
					}
				}
				break;
			case JSONStorable.Type.String:
				if (this._stringValue != null)
				{
					json["stringValue"] = this._stringValue;
				}
				break;
			case JSONStorable.Type.Url:
				if (this._stringValue != null)
				{
					json["urlValue"] = this._stringValue;
				}
				break;
			case JSONStorable.Type.StringChooser:
				if (this._stringChooserValue != null)
				{
					json["stringChooserValue"] = this._stringChooserValue;
				}
				break;
			case JSONStorable.Type.Color:
				json["color"]["h"].AsFloat = this._HSVColorH;
				json["color"]["s"].AsFloat = this._HSVColorS;
				json["color"]["v"].AsFloat = this._HSVColorV;
				break;
			case JSONStorable.Type.AudioClipAction:
				json["audioClipType"] = this._audioClipType.ToString();
				if (this._audioClipCategory != null)
				{
					json["audioClipCategory"] = this._audioClipCategory;
				}
				if (this._audioClip != null)
				{
					json["audioClip"] = this._audioClip.uid;
				}
				break;
			case JSONStorable.Type.SceneFilePathAction:
				if (this._sceneFilePath != null && this._sceneFilePath != string.Empty)
				{
					string text = this._sceneFilePath;
					if (SuperController.singleton != null)
					{
						text = SuperController.singleton.NormalizeSavePath(text);
					}
					json["sceneFilePath"] = text;
				}
				break;
			case JSONStorable.Type.PresetFilePathAction:
				if (this._presetFilePath != null && this._presetFilePath != string.Empty)
				{
					string text2 = this._presetFilePath;
					if (SuperController.singleton != null)
					{
						text2 = SuperController.singleton.NormalizeSavePath(text2);
					}
					json["presetFilePath"] = text2;
				}
				break;
			case JSONStorable.Type.StringChooserAction:
				if (this._stringChooserActionChoice != null)
				{
					json["stringChooserActionChoice"] = this._stringChooserActionChoice;
				}
				break;
			}
		}
		return json;
	}

	// Token: 0x06006C21 RID: 27681 RVA: 0x0028B2F8 File Offset: 0x002896F8
	public override void RestoreFromJSON(JSONClass jc, string subScenePrefix = null)
	{
		base.RestoreFromJSON(jc, subScenePrefix);
		if (jc["boolValue"] != null)
		{
			this.boolValue = jc["boolValue"].AsBool;
		}
		if (jc["floatValue"] != null)
		{
			this.floatValue = jc["floatValue"].AsFloat;
		}
		if (jc["useTimer"] != null)
		{
			this.useTimer = jc["useTimer"].AsBool;
		}
		else
		{
			this.useTimer = false;
		}
		if (jc["timerLength"] != null)
		{
			this.timerLength = jc["timerLength"].AsFloat;
		}
		else
		{
			this.timerLength = 0.5f;
		}
		if (jc["timerType"] != null)
		{
			this.SetTimerType(jc["timerType"]);
			this.SyncTimerTypePopup();
		}
		else
		{
			this.timerType = TriggerActionDiscrete.TimerType.EaseInOut;
		}
		if (jc["useSecondTimerPoint"] != null)
		{
			this.useSecondTimerPoint = jc["useSecondTimerPoint"].AsBool;
		}
		else
		{
			this.useSecondTimerPoint = false;
		}
		if (jc["secondTimerPointValue"] != null)
		{
			this.secondTimerPointValue = jc["secondTimerPointValue"].AsFloat;
		}
		else
		{
			this.secondTimerPointValue = this._floatValue;
		}
		if (jc["secondTimerPointCurveLocation"] != null)
		{
			this.secondTimerPointCurveLocation = jc["secondTimerPointCurveLocation"].AsFloat;
		}
		else
		{
			this.secondTimerPointCurveLocation = 0.5f;
		}
		if (jc["stringValue"] != null)
		{
			this.stringValue = jc["stringValue"];
		}
		if (jc["urlValue"] != null)
		{
			this.stringValue = FileManager.NormalizeLoadPath(jc["urlValue"]);
		}
		if (jc["stringChooserValue"] != null)
		{
			this.stringChooserValue = jc["stringChooserValue"];
		}
		if (jc["stringChooserActionChoice"] != null)
		{
			this.stringChooserActionChoice = jc["stringChooserActionChoice"];
		}
		if (jc["color"] != null)
		{
			float h = this._HSVColorH;
			float s = this._HSVColorS;
			float v = this._HSVColorV;
			if (jc["color"]["h"] != null)
			{
				h = jc["color"]["h"].AsFloat;
			}
			if (jc["color"]["s"] != null)
			{
				s = jc["color"]["s"].AsFloat;
			}
			if (jc["color"]["v"] != null)
			{
				v = jc["color"]["v"].AsFloat;
			}
			this.SetColorFromHSV(h, s, v);
		}
		if (jc["audioClipType"] != null)
		{
			this.SetAudioClipType(jc["audioClipType"]);
		}
		if (jc["audioClipCategory"] != null)
		{
			this.SetAudioClipCategory(jc["audioClipCategory"]);
		}
		if (jc["audioClip"] != null)
		{
			this.SetAudioClip(jc["audioClip"]);
		}
		if (jc["sceneFilePath"] != null)
		{
			string text = jc["sceneFilePath"];
			if (SuperController.singleton != null)
			{
				text = SuperController.singleton.NormalizeLoadPath(text);
			}
			this.SetSceneFilePath(text);
		}
		if (jc["presetFilePath"] != null)
		{
			string text2 = jc["presetFilePath"];
			if (SuperController.singleton != null)
			{
				text2 = SuperController.singleton.NormalizeLoadPath(text2);
			}
			this.SetPresetFilePath(text2);
		}
		this.AutoSetPreviewTextAndName();
	}

	// Token: 0x06006C22 RID: 27682 RVA: 0x0028B784 File Offset: 0x00289B84
	protected override void CreateTriggerActionPanel()
	{
		if (this.handler != null)
		{
			this.triggerActionPanel = this.handler.CreateTriggerActionDiscreteUI();
			this.InitTriggerActionPanelUI();
		}
		else
		{
			UnityEngine.Debug.LogError("Attempt to CreateTriggerActionPanel when handler is null");
		}
	}

	// Token: 0x06006C23 RID: 27683 RVA: 0x0028B7B7 File Offset: 0x00289BB7
	public override void OpenDetailPanel()
	{
		base.OpenDetailPanel();
		this.SyncFloatCurveRender();
	}

	// Token: 0x06006C24 RID: 27684 RVA: 0x0028B7C8 File Offset: 0x00289BC8
	public void Test()
	{
		this.ResetTest();
		if (this.resetTestButton != null)
		{
			this.resetTestButton.interactable = true;
		}
		if (!this.beforeTestStateCaptured && base.receiver != null && base.receiverTargetName != null)
		{
			switch (this._actionType)
			{
			case JSONStorable.Type.Bool:
				this.beforeTestBoolValue = base.receiver.GetBoolParamValue(base.receiverTargetName);
				break;
			case JSONStorable.Type.Float:
				this.beforeTestFloatValue = base.receiver.GetFloatParamValue(base.receiverTargetName);
				break;
			case JSONStorable.Type.String:
				this.beforeTestStringValue = base.receiver.GetStringParamValue(base.receiverTargetName);
				break;
			case JSONStorable.Type.Url:
				this.beforeTestStringValue = base.receiver.GetUrlParamValue(base.receiverTargetName);
				break;
			case JSONStorable.Type.StringChooser:
				this.beforeTestStringChooserValue = base.receiver.GetStringChooserParamValue(base.receiverTargetName);
				break;
			case JSONStorable.Type.Color:
				this.beforeTestTriggerColor = base.receiver.GetColorParamValue(base.receiverTargetName);
				break;
			}
			this.beforeTestStateCaptured = true;
		}
		this.Trigger(false, true);
	}

	// Token: 0x06006C25 RID: 27685 RVA: 0x0028B908 File Offset: 0x00289D08
	public void ResetTest()
	{
		if (this.resetTestButton != null)
		{
			this.resetTestButton.interactable = false;
		}
		if (this.beforeTestStateCaptured && base.receiver != null && base.receiverTargetName != null)
		{
			switch (this._actionType)
			{
			case JSONStorable.Type.Bool:
				base.receiver.SetBoolParamValue(base.receiverTargetName, this.beforeTestBoolValue);
				break;
			case JSONStorable.Type.Float:
				base.receiver.SetFloatParamValue(base.receiverTargetName, this.beforeTestFloatValue);
				break;
			case JSONStorable.Type.String:
				base.receiver.SetStringParamValue(base.receiverTargetName, this.beforeTestStringValue);
				break;
			case JSONStorable.Type.Url:
				base.receiver.SetUrlParamValue(base.receiverTargetName, this.beforeTestStringValue);
				break;
			case JSONStorable.Type.StringChooser:
				base.receiver.SetStringChooserParamValue(base.receiverTargetName, this.beforeTestStringChooserValue);
				break;
			case JSONStorable.Type.Color:
				base.receiver.SetColorParamValue(base.receiverTargetName, this.beforeTestTriggerColor);
				break;
			case JSONStorable.Type.AudioClipAction:
				base.receiver.CallAction("Stop");
				break;
			}
			this.beforeTestStateCaptured = false;
			this.timerActive = false;
		}
	}

	// Token: 0x06006C26 RID: 27686 RVA: 0x0028BA5C File Offset: 0x00289E5C
	protected override void SetReceiverTargetPopupNames()
	{
		this.receiverTargetNames = null;
		if (this._receiver != null)
		{
			this.receiverTargetNames = this._receiver.GetAllParamAndActionNames();
		}
		this.SyncTargetPopupNames();
	}

	// Token: 0x06006C27 RID: 27687 RVA: 0x0028BA90 File Offset: 0x00289E90
	protected override void AutoSetPreviewText()
	{
		if (this._receiverAtom != null && !string.IsNullOrEmpty(this._receiverStoreId) && !string.IsNullOrEmpty(this._receiverTargetName))
		{
			switch (this._actionType)
			{
			case JSONStorable.Type.Bool:
				this.AutoSetPreviewTextFromBoolParam();
				return;
			case JSONStorable.Type.Float:
				this.AutoSetPreviewTextFromFloatParam();
				return;
			case JSONStorable.Type.String:
			case JSONStorable.Type.Url:
				this.AutoSetPreviewTextFromStringOrUrlParam();
				return;
			case JSONStorable.Type.StringChooser:
				this.AutoSetPreviewTextFromStringChooserParam();
				return;
			case JSONStorable.Type.AudioClipAction:
				this.AutoSetPreviewTextFromAudioClipParam();
				return;
			}
			base.AutoSetPreviewText();
		}
	}

	// Token: 0x06006C28 RID: 27688 RVA: 0x0028BB48 File Offset: 0x00289F48
	protected void AutoSetPreviewTextFromBoolParam()
	{
		base.previewText = string.Concat(new string[]
		{
			this._receiverAtom.uid,
			":",
			this._receiverStoreId,
			":",
			this._receiverTargetName,
			":",
			this.boolValue.ToString()
		});
	}

	// Token: 0x06006C29 RID: 27689 RVA: 0x0028BBB8 File Offset: 0x00289FB8
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
			this.floatValue.ToString("F2")
		});
	}

	// Token: 0x06006C2A RID: 27690 RVA: 0x0028BC24 File Offset: 0x0028A024
	protected void AutoSetPreviewTextFromStringOrUrlParam()
	{
		base.previewText = string.Concat(new string[]
		{
			this._receiverAtom.uid,
			":",
			this._receiverStoreId,
			":",
			this._receiverTargetName,
			":",
			this.stringValue
		});
	}

	// Token: 0x06006C2B RID: 27691 RVA: 0x0028BC84 File Offset: 0x0028A084
	protected void AutoSetPreviewTextFromStringChooserParam()
	{
		base.previewText = string.Concat(new string[]
		{
			this._receiverAtom.uid,
			":",
			this._receiverStoreId,
			":",
			this._receiverTargetName,
			":",
			this.stringChooserValue
		});
	}

	// Token: 0x06006C2C RID: 27692 RVA: 0x0028BCE4 File Offset: 0x0028A0E4
	protected void AutoSetPreviewTextFromColorParam()
	{
		if (this.colorPicker != null)
		{
			base.previewText = string.Concat(new string[]
			{
				this._receiverAtom.uid,
				":",
				this._receiverStoreId,
				":",
				this._receiverTargetName,
				":",
				this.colorPicker.red255string,
				"_",
				this.colorPicker.green255string,
				"_",
				this.colorPicker.blue255string
			});
		}
	}

	// Token: 0x06006C2D RID: 27693 RVA: 0x0028BD88 File Offset: 0x0028A188
	protected void AutoSetPreviewTextFromAudioClipParam()
	{
		if (this._audioClip != null)
		{
			base.previewText = string.Concat(new string[]
			{
				this._receiverAtom.uid,
				":",
				this._receiverStoreId,
				":",
				this._receiverTargetName,
				":",
				this._audioClip.uid
			});
		}
	}

	// Token: 0x06006C2E RID: 27694 RVA: 0x0028BDF8 File Offset: 0x0028A1F8
	protected override void AutoSetName()
	{
		if (base.receiver != null && !string.IsNullOrEmpty(this._receiverTargetName))
		{
			switch (this._actionType)
			{
			case JSONStorable.Type.Bool:
				this.AutoSetNameFromBoolParam();
				return;
			case JSONStorable.Type.Float:
				this.AutoSetNameFromFloatParam();
				return;
			case JSONStorable.Type.String:
			case JSONStorable.Type.Url:
				this.AutoSetNameFromStringOrUrlParam();
				return;
			case JSONStorable.Type.StringChooser:
				this.AutoSetNameFromStringChooserParam();
				return;
			case JSONStorable.Type.AudioClipAction:
				this.AutoSetNameFromAudioClipParam();
				return;
			}
			base.AutoSetName();
		}
	}

	// Token: 0x06006C2F RID: 27695 RVA: 0x0028BEA0 File Offset: 0x0028A2A0
	protected void AutoSetNameFromBoolParam()
	{
		base.name = "A_" + this._receiverTargetName + ":" + this.boolValue.ToString();
	}

	// Token: 0x06006C30 RID: 27696 RVA: 0x0028BEDC File Offset: 0x0028A2DC
	protected void AutoSetNameFromFloatParam()
	{
		base.name = "A_" + this._receiverTargetName + ":" + this.floatValue.ToString("F2");
	}

	// Token: 0x06006C31 RID: 27697 RVA: 0x0028BF17 File Offset: 0x0028A317
	protected void AutoSetNameFromStringOrUrlParam()
	{
		base.name = "A_" + this._receiverTargetName + ":" + this.stringValue;
	}

	// Token: 0x06006C32 RID: 27698 RVA: 0x0028BF3A File Offset: 0x0028A33A
	protected void AutoSetNameFromStringChooserParam()
	{
		base.name = "A_" + this._receiverTargetName + ":" + this.stringChooserValue;
	}

	// Token: 0x06006C33 RID: 27699 RVA: 0x0028BF60 File Offset: 0x0028A360
	protected void AutoSetNameFromColorParam()
	{
		if (this.colorPicker != null)
		{
			base.name = string.Concat(new string[]
			{
				"A_",
				this._receiverTargetName,
				":",
				this.colorPicker.red255string,
				"_",
				this.colorPicker.green255string,
				"_",
				this.colorPicker.blue255string
			});
		}
	}

	// Token: 0x06006C34 RID: 27700 RVA: 0x0028BFE2 File Offset: 0x0028A3E2
	protected void AutoSetNameFromAudioClipParam()
	{
		if (this._audioClip != null)
		{
			base.name = "A_" + this._receiverTargetName + ":" + this._audioClip.uid;
		}
	}

	// Token: 0x06006C35 RID: 27701 RVA: 0x0028C018 File Offset: 0x0028A418
	protected override void SetInitialParamsFromReceiverTarget()
	{
		base.SetInitialParamsFromReceiverTarget();
		if (this._receiver != null && base.receiverTargetName != null)
		{
			switch (this.actionType)
			{
			case JSONStorable.Type.Bool:
				this.boolValue = this._receiver.GetBoolParamValue(this._receiverTargetName);
				break;
			case JSONStorable.Type.Float:
				this.floatValue = this._receiver.GetFloatParamValue(this._receiverTargetName);
				this.secondTimerPointValue = this._floatValue;
				break;
			case JSONStorable.Type.String:
				this.stringValue = this._receiver.GetStringParamValue(this._receiverTargetName);
				break;
			case JSONStorable.Type.Url:
				this.stringValue = this._receiver.GetUrlParamValue(this._receiverTargetName);
				break;
			case JSONStorable.Type.StringChooser:
				this.stringChooserValue = this._receiver.GetStringChooserParamValue(this._receiverTargetName);
				break;
			case JSONStorable.Type.Color:
			{
				HSVColor colorParamValue = this._receiver.GetColorParamValue(this._receiverTargetName);
				this.SetColorFromHSV(colorParamValue.H, colorParamValue.S, colorParamValue.V);
				break;
			}
			}
		}
	}

	// Token: 0x06006C36 RID: 27702 RVA: 0x0028C144 File Offset: 0x0028A544
	protected void SyncStringChooserPopup()
	{
		if (this.stringChooserValuePopup != null && this._receiver != null && this._receiverTargetName != null)
		{
			this.stringChooserValuePopup.label = this._receiverTargetName;
			if (this._receiver.IsStringChooserJSONParam(this._receiverTargetName))
			{
				List<string> stringChooserJSONParamChoices = this._receiver.GetStringChooserJSONParamChoices(this._receiverTargetName);
				this.stringChooserValuePopup.numPopupValues = stringChooserJSONParamChoices.Count;
				for (int i = 0; i < stringChooserJSONParamChoices.Count; i++)
				{
					this.stringChooserValuePopup.setPopupValue(i, stringChooserJSONParamChoices[i]);
				}
			}
		}
	}

	// Token: 0x06006C37 RID: 27703 RVA: 0x0028C1F4 File Offset: 0x0028A5F4
	protected override void SyncFromReceiverTarget()
	{
		base.SyncFromReceiverTarget();
		if (this._receiver != null && this._receiverTargetName != null)
		{
			this.actionType = this._receiver.GetParamOrActionType(this._receiverTargetName);
			JSONStorable.Type actionType = this.actionType;
			if (actionType != JSONStorable.Type.Float)
			{
				if (actionType == JSONStorable.Type.StringChooser)
				{
					this.SyncStringChooserPopup();
				}
			}
			else
			{
				if (this.floatValueSlider != null)
				{
					this.floatValueSlider.minValue = this.paramMin;
					this.floatValueSlider.maxValue = this.paramMax;
				}
				if (this.floatValueDynamicSlider != null)
				{
					this.floatValueDynamicSlider.rangeAdjustEnabled = !this.paramContrained;
					this.floatValueDynamicSlider.defaultValue = this.paramDefault;
					if (this.receiverTargetFloat != null)
					{
						this.floatValueDynamicSlider.label = this.receiverTargetFloat.name;
					}
				}
				if (this.secondTimerPointValueSlider != null)
				{
					this.secondTimerPointValueSlider.minValue = this.paramMin;
					this.secondTimerPointValueSlider.maxValue = this.paramMax;
				}
				if (this.secondTimerPointValueDynamicSlider != null)
				{
					this.secondTimerPointValueDynamicSlider.rangeAdjustEnabled = !this.paramContrained;
					this.secondTimerPointValueDynamicSlider.defaultValue = this.paramDefault;
				}
			}
			this.AutoSetPreviewTextAndName();
		}
		else
		{
			this.actionType = JSONStorable.Type.None;
		}
	}

	// Token: 0x06006C38 RID: 27704 RVA: 0x0028C369 File Offset: 0x0028A769
	protected override void CheckMissingReceiver()
	{
		base.CheckMissingReceiver();
		if (this._receiver != null && this._receiverTargetName != null && this._actionType == JSONStorable.Type.None)
		{
			this.SyncFromReceiverTarget();
		}
	}

	// Token: 0x06006C39 RID: 27705 RVA: 0x0028C3A0 File Offset: 0x0028A7A0
	protected void SyncType()
	{
		if (this.boolValueToggle != null)
		{
			if (this._actionType == JSONStorable.Type.Bool)
			{
				this.boolValueToggle.gameObject.SetActive(true);
			}
			else
			{
				this.boolValueToggle.gameObject.SetActive(false);
			}
		}
		if (this.floatValueSlider != null)
		{
			if (this._actionType == JSONStorable.Type.Float)
			{
				this.floatValueSlider.transform.parent.gameObject.SetActive(true);
			}
			else
			{
				this.floatValueSlider.transform.parent.gameObject.SetActive(false);
			}
		}
		if (this.useTimerToggle != null)
		{
			this.useTimerToggle.gameObject.SetActive(this._actionType == JSONStorable.Type.Float);
		}
		if (this.timerLengthSlider != null)
		{
			this.timerLengthSlider.transform.parent.gameObject.SetActive(this._actionType == JSONStorable.Type.Float);
		}
		if (this.timerTypePopup != null)
		{
			this.timerTypePopup.gameObject.SetActive(this._actionType == JSONStorable.Type.Float);
		}
		if (this.curveContainer != null)
		{
			this.curveContainer.gameObject.SetActive(this._actionType == JSONStorable.Type.Float);
		}
		if (this.useSecondTimerPointToggle != null)
		{
			this.useSecondTimerPointToggle.gameObject.SetActive(this._actionType == JSONStorable.Type.Float);
		}
		if (this.secondTimerPointValueSlider != null)
		{
			this.secondTimerPointValueSlider.transform.parent.gameObject.SetActive(this._actionType == JSONStorable.Type.Float);
		}
		if (this.secondTimerPointCurveLocationSlider != null)
		{
			this.secondTimerPointCurveLocationSlider.transform.parent.gameObject.SetActive(this._actionType == JSONStorable.Type.Float);
		}
		if (this.stringValueField != null)
		{
			if (this._actionType == JSONStorable.Type.String || this._actionType == JSONStorable.Type.Url)
			{
				this.stringValueField.gameObject.SetActive(true);
			}
			else
			{
				this.stringValueField.gameObject.SetActive(false);
			}
		}
		if (this.stringChooserValuePopup != null)
		{
			if (this._actionType == JSONStorable.Type.StringChooser)
			{
				this.stringChooserValuePopup.gameObject.SetActive(true);
			}
			else
			{
				this.stringChooserValuePopup.gameObject.SetActive(false);
			}
		}
		if (this.colorPickerContainer != null)
		{
			if (this._actionType == JSONStorable.Type.Color)
			{
				this.colorPickerContainer.gameObject.SetActive(true);
			}
			else
			{
				this.colorPickerContainer.gameObject.SetActive(false);
			}
		}
		if (this.audioClipPopupsContainer != null)
		{
			if (this._actionType == JSONStorable.Type.AudioClipAction)
			{
				this.audioClipPopupsContainer.gameObject.SetActive(true);
			}
			else
			{
				this.audioClipPopupsContainer.gameObject.SetActive(false);
			}
		}
		if (this.stringChooserActionPopup != null)
		{
			this.stringChooserActionPopup.gameObject.SetActive(this._actionType == JSONStorable.Type.StringChooserAction);
		}
		if (this.sceneFilePathText != null)
		{
			if (this._actionType == JSONStorable.Type.SceneFilePathAction)
			{
				this.sceneFilePathText.gameObject.SetActive(true);
			}
			else
			{
				this.sceneFilePathText.gameObject.SetActive(false);
			}
		}
		if (this.presetFilePathText != null)
		{
			if (this._actionType == JSONStorable.Type.PresetFilePathAction)
			{
				this.presetFilePathText.gameObject.SetActive(true);
			}
			else
			{
				this.presetFilePathText.gameObject.SetActive(false);
			}
		}
		if (this.chooseSceneFilePathButton != null)
		{
			if (this._actionType == JSONStorable.Type.SceneFilePathAction)
			{
				this.chooseSceneFilePathButton.gameObject.SetActive(true);
			}
			else
			{
				this.chooseSceneFilePathButton.gameObject.SetActive(false);
			}
		}
		if (this.choosePresetFilePathButton != null)
		{
			if (this._actionType == JSONStorable.Type.PresetFilePathAction)
			{
				this.choosePresetFilePathButton.gameObject.SetActive(true);
			}
			else
			{
				this.choosePresetFilePathButton.gameObject.SetActive(false);
			}
		}
	}

	// Token: 0x17000FDD RID: 4061
	// (get) Token: 0x06006C3A RID: 27706 RVA: 0x0028C7E9 File Offset: 0x0028ABE9
	// (set) Token: 0x06006C3B RID: 27707 RVA: 0x0028C7F1 File Offset: 0x0028ABF1
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

	// Token: 0x06006C3C RID: 27708 RVA: 0x0028C80C File Offset: 0x0028AC0C
	public void SetAudioClipType(string type)
	{
		try
		{
			this.audioClipType = (TriggerActionDiscrete.AudioClipType)Enum.Parse(typeof(TriggerActionDiscrete.AudioClipType), type);
		}
		catch (ArgumentException)
		{
			UnityEngine.Debug.LogError("Attempted to TriggerActionDiscrete audioClipType to " + type + " which is not a valid type");
		}
	}

	// Token: 0x17000FDE RID: 4062
	// (get) Token: 0x06006C3D RID: 27709 RVA: 0x0028C864 File Offset: 0x0028AC64
	// (set) Token: 0x06006C3E RID: 27710 RVA: 0x0028C86C File Offset: 0x0028AC6C
	public TriggerActionDiscrete.AudioClipType audioClipType
	{
		get
		{
			return this._audioClipType;
		}
		set
		{
			if (this._audioClipType != value)
			{
				this._audioClipType = value;
				if (this.audioClipTypePopup != null)
				{
					this.audioClipTypePopup.currentValue = this._audioClipType.ToString();
				}
				this.audioClipCategory = null;
				this.SetClipCategoryPopupValues();
			}
		}
	}

	// Token: 0x06006C3F RID: 27711 RVA: 0x0028C8C8 File Offset: 0x0028ACC8
	protected void SetClipCategoryPopupValues()
	{
		if (this.audioClipCategoryPopup != null)
		{
			List<string> list = null;
			if (this._audioClipType == TriggerActionDiscrete.AudioClipType.Embedded)
			{
				if (EmbeddedAudioClipManager.singleton != null)
				{
					list = EmbeddedAudioClipManager.singleton.GetCategories();
				}
			}
			else if (this._audioClipType == TriggerActionDiscrete.AudioClipType.URL && URLAudioClipManager.singleton != null)
			{
				list = URLAudioClipManager.singleton.GetCategories();
			}
			if (list != null)
			{
				this.audioClipCategoryPopup.numPopupValues = list.Count + 1;
				int num = 0;
				this.audioClipCategoryPopup.setPopupValue(num, "None");
				num++;
				foreach (string text in list)
				{
					this.audioClipCategoryPopup.setPopupValue(num, text);
					num++;
				}
			}
			else
			{
				this.audioClipCategoryPopup.numPopupValues = 1;
				int index = 0;
				this.audioClipCategoryPopup.setPopupValue(index, "None");
			}
		}
	}

	// Token: 0x06006C40 RID: 27712 RVA: 0x0028C9E4 File Offset: 0x0028ADE4
	public void SetAudioClipCategory(string cat)
	{
		this.audioClipCategory = cat;
	}

	// Token: 0x17000FDF RID: 4063
	// (get) Token: 0x06006C41 RID: 27713 RVA: 0x0028C9ED File Offset: 0x0028ADED
	// (set) Token: 0x06006C42 RID: 27714 RVA: 0x0028C9F8 File Offset: 0x0028ADF8
	public string audioClipCategory
	{
		get
		{
			return this._audioClipCategory;
		}
		set
		{
			if (this._audioClipCategory != value)
			{
				this._audioClipCategory = value;
				if (this.audioClipCategoryPopup != null)
				{
					this.audioClipCategoryPopup.currentValue = value;
				}
				this.audioClip = null;
				this.SetClipPopupValues();
			}
		}
	}

	// Token: 0x06006C43 RID: 27715 RVA: 0x0028CA48 File Offset: 0x0028AE48
	protected void SetClipPopupValues()
	{
		if (this.audioClipPopup != null)
		{
			this.audioClipPopup.useDifferentDisplayValues = true;
			List<NamedAudioClip> list = null;
			if (this._audioClipCategory != null)
			{
				if (this._audioClipType == TriggerActionDiscrete.AudioClipType.Embedded)
				{
					if (EmbeddedAudioClipManager.singleton != null)
					{
						list = EmbeddedAudioClipManager.singleton.GetCategoryClips(this._audioClipCategory);
					}
				}
				else if (this._audioClipType == TriggerActionDiscrete.AudioClipType.URL && URLAudioClipManager.singleton != null)
				{
					list = URLAudioClipManager.singleton.GetCategoryClips(this._audioClipCategory);
				}
			}
			if (list != null)
			{
				this.audioClipPopup.numPopupValues = list.Count + 1;
				int num = 0;
				this.audioClipPopup.setPopupValue(num, "None");
				this.audioClipPopup.setDisplayPopupValue(num, "None");
				num++;
				foreach (NamedAudioClip namedAudioClip in list)
				{
					this.audioClipPopup.setPopupValue(num, namedAudioClip.uid);
					this.audioClipPopup.setDisplayPopupValue(num, namedAudioClip.displayName);
					num++;
				}
			}
			else
			{
				this.audioClipPopup.numPopupValues = 1;
				this.audioClipPopup.setPopupValue(0, "None");
			}
		}
	}

	// Token: 0x06006C44 RID: 27716 RVA: 0x0028CBAC File Offset: 0x0028AFAC
	public void SetAudioClip(string clipUID)
	{
		if (clipUID != "None")
		{
			if (this._audioClipType == TriggerActionDiscrete.AudioClipType.Embedded)
			{
				if (EmbeddedAudioClipManager.singleton != null)
				{
					this.audioClip = EmbeddedAudioClipManager.singleton.GetClip(clipUID);
				}
			}
			else if (this._audioClipType == TriggerActionDiscrete.AudioClipType.URL && URLAudioClipManager.singleton != null)
			{
				this.audioClip = URLAudioClipManager.singleton.GetClip(clipUID);
			}
		}
		else
		{
			this.audioClip = null;
		}
	}

	// Token: 0x17000FE0 RID: 4064
	// (get) Token: 0x06006C45 RID: 27717 RVA: 0x0028CC33 File Offset: 0x0028B033
	// (set) Token: 0x06006C46 RID: 27718 RVA: 0x0028CC3C File Offset: 0x0028B03C
	public NamedAudioClip audioClip
	{
		get
		{
			return this._audioClip;
		}
		set
		{
			if (this._audioClip != value)
			{
				this._audioClip = value;
				if (this.audioClipPopup != null)
				{
					if (this._audioClip == null)
					{
						this.audioClipPopup.currentValue = "None";
					}
					else
					{
						this.audioClipPopup.currentValue = this._audioClip.uid;
					}
				}
				this.AutoSetPreviewTextAndName();
			}
		}
	}

	// Token: 0x06006C47 RID: 27719 RVA: 0x0028CCAC File Offset: 0x0028B0AC
	protected void SetStringChooserActionPopupValues()
	{
		if (base.receiver != null && base.receiverTargetName != null)
		{
			JSONStorableActionStringChooser stringChooserAction = base.receiver.GetStringChooserAction(base.receiverTargetName);
			if (stringChooserAction != null && this.stringChooserActionPopup != null)
			{
				this.stringChooserActionPopup.useDifferentDisplayValues = true;
				List<string> choices = stringChooserAction.choices;
				List<string> displayChoices = stringChooserAction.displayChoices;
				if (choices != null)
				{
					this.stringChooserActionPopup.numPopupValues = choices.Count;
					for (int i = 0; i < choices.Count; i++)
					{
						this.stringChooserActionPopup.setPopupValue(i, choices[i]);
					}
				}
				else
				{
					this.stringChooserActionPopup.numPopupValues = 0;
				}
				if (displayChoices != null)
				{
					for (int j = 0; j < displayChoices.Count; j++)
					{
						this.stringChooserActionPopup.setDisplayPopupValue(j, displayChoices[j]);
					}
				}
			}
		}
	}

	// Token: 0x17000FE1 RID: 4065
	// (get) Token: 0x06006C48 RID: 27720 RVA: 0x0028CDA0 File Offset: 0x0028B1A0
	// (set) Token: 0x06006C49 RID: 27721 RVA: 0x0028CDA8 File Offset: 0x0028B1A8
	public string stringChooserActionChoice
	{
		get
		{
			return this._stringChooserActionChoice;
		}
		set
		{
			if (this._stringChooserActionChoice != value)
			{
				this._stringChooserActionChoice = value;
				if (this.stringChooserActionPopup != null)
				{
					this.stringChooserActionPopup.currentValue = this._stringChooserActionChoice;
				}
				this.AutoSetPreviewTextAndName();
			}
		}
	}

	// Token: 0x06006C4A RID: 27722 RVA: 0x0028CDF5 File Offset: 0x0028B1F5
	protected void SetStringChooserActionChoice(string choice)
	{
		this._stringChooserActionChoice = choice;
		this.AutoSetPreviewTextAndName();
	}

	// Token: 0x06006C4B RID: 27723 RVA: 0x0028CE04 File Offset: 0x0028B204
	protected void GetSceneFilePath()
	{
		if (SuperController.singleton != null)
		{
			SuperController.singleton.GetScenePathDialog(new FileBrowserCallback(this.SetSceneFilePath));
		}
	}

	// Token: 0x06006C4C RID: 27724 RVA: 0x0028CE2C File Offset: 0x0028B22C
	public void SetSceneFilePath(string path)
	{
		path = SuperController.singleton.NormalizeScenePath(path);
		this.sceneFilePath = path;
	}

	// Token: 0x17000FE2 RID: 4066
	// (get) Token: 0x06006C4D RID: 27725 RVA: 0x0028CE42 File Offset: 0x0028B242
	// (set) Token: 0x06006C4E RID: 27726 RVA: 0x0028CE4C File Offset: 0x0028B24C
	public string sceneFilePath
	{
		get
		{
			return this._sceneFilePath;
		}
		set
		{
			if (this._sceneFilePath != value)
			{
				this._sceneFilePath = value;
				if (this.sceneFilePathText != null)
				{
					this.sceneFilePathText.text = this._sceneFilePath;
				}
				this.AutoSetPreviewTextAndName();
			}
		}
	}

	// Token: 0x06006C4F RID: 27727 RVA: 0x0028CE9C File Offset: 0x0028B29C
	protected void GetPresetFilePath()
	{
		if (base.receiver != null && base.receiverTargetName != null)
		{
			JSONStorableActionPresetFilePath presetFilePathAction = base.receiver.GetPresetFilePathAction(base.receiverTargetName);
			if (presetFilePathAction != null)
			{
				presetFilePathAction.Browse(new JSONStorableString.SetStringCallback(this.SetPresetFilePath));
			}
		}
	}

	// Token: 0x06006C50 RID: 27728 RVA: 0x0028CEEF File Offset: 0x0028B2EF
	public void SetPresetFilePath(string path)
	{
		path = SuperController.singleton.NormalizePath(path);
		this.presetFilePath = path;
	}

	// Token: 0x17000FE3 RID: 4067
	// (get) Token: 0x06006C51 RID: 27729 RVA: 0x0028CF05 File Offset: 0x0028B305
	// (set) Token: 0x06006C52 RID: 27730 RVA: 0x0028CF10 File Offset: 0x0028B310
	public string presetFilePath
	{
		get
		{
			return this._presetFilePath;
		}
		set
		{
			if (this._presetFilePath != value)
			{
				this._presetFilePath = value;
				if (this.presetFilePathText != null)
				{
					this.presetFilePathText.text = this._presetFilePath;
				}
				this.AutoSetPreviewTextAndName();
			}
		}
	}

	// Token: 0x17000FE4 RID: 4068
	// (get) Token: 0x06006C53 RID: 27731 RVA: 0x0028CF5D File Offset: 0x0028B35D
	// (set) Token: 0x06006C54 RID: 27732 RVA: 0x0028CF65 File Offset: 0x0028B365
	public bool boolValue
	{
		get
		{
			return this._boolValue;
		}
		set
		{
			if (this._boolValue != value)
			{
				this._boolValue = value;
				if (this.boolValueToggle != null)
				{
					this.boolValueToggle.isOn = this._boolValue;
				}
				this.AutoSetPreviewTextAndName();
			}
		}
	}

	// Token: 0x06006C55 RID: 27733 RVA: 0x0028CFA2 File Offset: 0x0028B3A2
	public void SetFloatValue(float f)
	{
		this._floatValue = f;
		this.SyncFloatCurve(true);
		this.AutoSetPreviewTextAndName();
	}

	// Token: 0x17000FE5 RID: 4069
	// (get) Token: 0x06006C56 RID: 27734 RVA: 0x0028CFB8 File Offset: 0x0028B3B8
	// (set) Token: 0x06006C57 RID: 27735 RVA: 0x0028CFC0 File Offset: 0x0028B3C0
	public float floatValue
	{
		get
		{
			return this._floatValue;
		}
		set
		{
			if (this._floatValue != value)
			{
				this._floatValue = value;
				this.SyncFloatCurve(true);
				if (this.floatValueSlider != null)
				{
					this.floatValueSlider.value = this._floatValue;
				}
				this.AutoSetPreviewTextAndName();
			}
		}
	}

	// Token: 0x17000FE6 RID: 4070
	// (get) Token: 0x06006C58 RID: 27736 RVA: 0x0028D00F File Offset: 0x0028B40F
	// (set) Token: 0x06006C59 RID: 27737 RVA: 0x0028D017 File Offset: 0x0028B417
	public bool timerActive
	{
		[CompilerGenerated]
		get
		{
			return this.<timerActive>k__BackingField;
		}
		[CompilerGenerated]
		protected set
		{
			this.<timerActive>k__BackingField = value;
		}
	}

	// Token: 0x17000FE7 RID: 4071
	// (get) Token: 0x06006C5A RID: 27738 RVA: 0x0028D020 File Offset: 0x0028B420
	// (set) Token: 0x06006C5B RID: 27739 RVA: 0x0028D028 File Offset: 0x0028B428
	public float currentTimerTime
	{
		[CompilerGenerated]
		get
		{
			return this.<currentTimerTime>k__BackingField;
		}
		[CompilerGenerated]
		protected set
		{
			this.<currentTimerTime>k__BackingField = value;
		}
	}

	// Token: 0x06006C5C RID: 27740 RVA: 0x0028D034 File Offset: 0x0028B434
	protected void SyncFloatCurve(bool useCurrentCurveStart = false)
	{
		if (this._useTimer && base.receiver != null && base.receiverTargetName != null)
		{
			float startValue;
			if (useCurrentCurveStart && this.floatCurve != null)
			{
				startValue = this._curveValueStart;
			}
			else
			{
				startValue = base.receiver.GetFloatParamValue(base.receiverTargetName);
			}
			this.ConfigureFloatCurve(this._timerType, this._timerLength, startValue, this._floatValue, this._useSecondTimerPoint, this._secondTimerPointValue, this._secondTimerPointCurveLocation);
		}
	}

	// Token: 0x06006C5D RID: 27741 RVA: 0x0028D0C4 File Offset: 0x0028B4C4
	protected void ConfigureFloatCurve(TriggerActionDiscrete.TimerType type, float length, float startValue, float endValue, bool useSecondPoint, float secondPointValue, float secondPointLocation)
	{
		if (this.floatCurve == null || this._curveTimerType != type || this._curveTimerLength != length || this._curveValueStart != startValue || this._curveValueEnd != endValue || this._curveUseSecondPoint != useSecondPoint || this._curveSecondPointValue != secondPointValue || this._curveSecondPointLocation != secondPointLocation)
		{
			if (useSecondPoint && secondPointLocation <= 0.01f)
			{
				startValue = secondPointValue;
			}
			if (type != TriggerActionDiscrete.TimerType.EaseInOut)
			{
				if (type == TriggerActionDiscrete.TimerType.Linear)
				{
					this.floatCurve = AnimationCurve.Linear(0f, startValue, length, endValue);
				}
			}
			else
			{
				this.floatCurve = AnimationCurve.EaseInOut(0f, startValue, length, endValue);
			}
			if (useSecondPoint && secondPointLocation > 0.01f)
			{
				this.floatCurve.AddKey(length * secondPointLocation, secondPointValue);
			}
			this._curveTimerType = type;
			this._curveTimerLength = length;
			this._curveValueStart = startValue;
			this._curveValueEnd = endValue;
			this._curveUseSecondPoint = useSecondPoint;
			this._curveSecondPointValue = secondPointValue;
			this._curveSecondPointLocation = secondPointLocation;
			this.SyncFloatCurveRender();
		}
	}

	// Token: 0x17000FE8 RID: 4072
	// (get) Token: 0x06006C5E RID: 27742 RVA: 0x0028D1EF File Offset: 0x0028B5EF
	// (set) Token: 0x06006C5F RID: 27743 RVA: 0x0028D1F7 File Offset: 0x0028B5F7
	public AnimationCurve floatCurve
	{
		[CompilerGenerated]
		get
		{
			return this.<floatCurve>k__BackingField;
		}
		[CompilerGenerated]
		protected set
		{
			this.<floatCurve>k__BackingField = value;
		}
	}

	// Token: 0x06006C60 RID: 27744 RVA: 0x0028D200 File Offset: 0x0028B600
	protected void SyncFloatCurveRender()
	{
		if (base.detailPanelOpen && this._useTimer && this.floatCurve != null && this.curveLineRenderer != null && this.curveLineRenderer.Points != null)
		{
			Vector2[] points = this.curveLineRenderer.Points;
			int num = points.Length;
			this.curveLineRenderer.RelativeSize = true;
			float num2 = 1f / (float)(num - 1);
			float num3 = this.paramMax - this.paramMin;
			float num4 = 1f;
			if (num3 != 0f)
			{
				num4 = 1f / num3;
			}
			float num5 = this.paramMin * num4;
			for (int i = 0; i < num; i++)
			{
				float num6 = num2 * (float)i;
				points[i].x = num2 * (float)i;
				points[i].y = Mathf.Clamp01(this.floatCurve.Evaluate(num6 * this._curveTimerLength) * num4 - num5);
			}
		}
	}

	// Token: 0x06006C61 RID: 27745 RVA: 0x0028D305 File Offset: 0x0028B705
	protected void SetUseTimer(bool b)
	{
		this._useTimer = b;
	}

	// Token: 0x17000FE9 RID: 4073
	// (get) Token: 0x06006C62 RID: 27746 RVA: 0x0028D30E File Offset: 0x0028B70E
	// (set) Token: 0x06006C63 RID: 27747 RVA: 0x0028D316 File Offset: 0x0028B716
	public bool useTimer
	{
		get
		{
			return this._useTimer;
		}
		set
		{
			if (this._useTimer != value)
			{
				this._useTimer = value;
				if (this.useTimerToggle != null)
				{
					this.useTimerToggle.isOn = this._useTimer;
				}
			}
		}
	}

	// Token: 0x06006C64 RID: 27748 RVA: 0x0028D34D File Offset: 0x0028B74D
	protected void SetTimerLength(float f)
	{
		this._timerLength = f;
		if (this._timerLength > 0f)
		{
			this._oneOverTimerLength = 1f / this._timerLength;
		}
		this.SyncFloatCurve(true);
	}

	// Token: 0x17000FEA RID: 4074
	// (get) Token: 0x06006C65 RID: 27749 RVA: 0x0028D37F File Offset: 0x0028B77F
	// (set) Token: 0x06006C66 RID: 27750 RVA: 0x0028D387 File Offset: 0x0028B787
	public float timerLength
	{
		get
		{
			return this._timerLength;
		}
		set
		{
			if (this._timerLength != value)
			{
				this.SetTimerLength(value);
				if (this.timerLengthSlider != null)
				{
					this.timerLengthSlider.value = this._timerLength;
				}
			}
		}
	}

	// Token: 0x06006C67 RID: 27751 RVA: 0x0028D3BE File Offset: 0x0028B7BE
	protected void SyncTimerTypePopup()
	{
		if (this.timerTypePopup != null)
		{
			this.timerTypePopup.currentValueNoCallback = this._timerType.ToString();
		}
	}

	// Token: 0x06006C68 RID: 27752 RVA: 0x0028D3F0 File Offset: 0x0028B7F0
	protected void SetTimerType(string type)
	{
		try
		{
			TriggerActionDiscrete.TimerType timerType = (TriggerActionDiscrete.TimerType)Enum.Parse(typeof(TriggerActionDiscrete.TimerType), type);
			this._timerType = timerType;
			this.SyncFloatCurve(true);
		}
		catch (ArgumentException)
		{
			UnityEngine.Debug.LogError("Attempt to set timer type to " + type + " which is not a valid timer type");
		}
	}

	// Token: 0x17000FEB RID: 4075
	// (get) Token: 0x06006C69 RID: 27753 RVA: 0x0028D454 File Offset: 0x0028B854
	// (set) Token: 0x06006C6A RID: 27754 RVA: 0x0028D45C File Offset: 0x0028B85C
	public TriggerActionDiscrete.TimerType timerType
	{
		get
		{
			return this._timerType;
		}
		set
		{
			if (this._timerType != value)
			{
				this._timerType = value;
				this.SyncFloatCurve(true);
				this.SyncTimerTypePopup();
			}
		}
	}

	// Token: 0x06006C6B RID: 27755 RVA: 0x0028D47E File Offset: 0x0028B87E
	protected void SetUseSecondTimerPoint(bool b)
	{
		this._useSecondTimerPoint = b;
		this.SyncFloatCurve(true);
	}

	// Token: 0x17000FEC RID: 4076
	// (get) Token: 0x06006C6C RID: 27756 RVA: 0x0028D48E File Offset: 0x0028B88E
	// (set) Token: 0x06006C6D RID: 27757 RVA: 0x0028D496 File Offset: 0x0028B896
	public bool useSecondTimerPoint
	{
		get
		{
			return this._useSecondTimerPoint;
		}
		set
		{
			if (this._useSecondTimerPoint != value)
			{
				this._useSecondTimerPoint = value;
				this.SyncFloatCurve(true);
				if (this.useSecondTimerPointToggle != null)
				{
					this.useSecondTimerPointToggle.isOn = this._useSecondTimerPoint;
				}
			}
		}
	}

	// Token: 0x06006C6E RID: 27758 RVA: 0x0028D4D4 File Offset: 0x0028B8D4
	public void SetSecondTimerFloatValue(float f)
	{
		this._secondTimerPointValue = f;
		this.SyncFloatCurve(true);
	}

	// Token: 0x17000FED RID: 4077
	// (get) Token: 0x06006C6F RID: 27759 RVA: 0x0028D4E4 File Offset: 0x0028B8E4
	// (set) Token: 0x06006C70 RID: 27760 RVA: 0x0028D4EC File Offset: 0x0028B8EC
	public float secondTimerPointValue
	{
		get
		{
			return this._secondTimerPointValue;
		}
		set
		{
			if (this._secondTimerPointValue != value)
			{
				this._secondTimerPointValue = value;
				this.SyncFloatCurve(true);
				if (this.secondTimerPointValueSlider != null)
				{
					this.secondTimerPointValueSlider.value = this._secondTimerPointValue;
				}
			}
		}
	}

	// Token: 0x06006C71 RID: 27761 RVA: 0x0028D52A File Offset: 0x0028B92A
	public void SetSecondTimerPointCurveLocation(float f)
	{
		this._secondTimerPointCurveLocation = f;
		this.SyncFloatCurve(true);
	}

	// Token: 0x17000FEE RID: 4078
	// (get) Token: 0x06006C72 RID: 27762 RVA: 0x0028D53A File Offset: 0x0028B93A
	// (set) Token: 0x06006C73 RID: 27763 RVA: 0x0028D542 File Offset: 0x0028B942
	public float secondTimerPointCurveLocation
	{
		get
		{
			return this._secondTimerPointCurveLocation;
		}
		set
		{
			if (this._secondTimerPointCurveLocation != value)
			{
				this._secondTimerPointCurveLocation = value;
				this.SyncFloatCurve(true);
				if (this.secondTimerPointCurveLocationSlider != null)
				{
					this.secondTimerPointCurveLocationSlider.value = this._secondTimerPointCurveLocation;
				}
			}
		}
	}

	// Token: 0x06006C74 RID: 27764 RVA: 0x0028D580 File Offset: 0x0028B980
	protected void SetStringValue(string v)
	{
		this.stringValue = v;
	}

	// Token: 0x06006C75 RID: 27765 RVA: 0x0028D589 File Offset: 0x0028B989
	protected void SetStringValueToField()
	{
		if (this.stringValueField != null)
		{
			this.stringValue = this.stringValueField.text;
		}
	}

	// Token: 0x17000FEF RID: 4079
	// (get) Token: 0x06006C76 RID: 27766 RVA: 0x0028D5AD File Offset: 0x0028B9AD
	// (set) Token: 0x06006C77 RID: 27767 RVA: 0x0028D5B8 File Offset: 0x0028B9B8
	public string stringValue
	{
		get
		{
			return this._stringValue;
		}
		set
		{
			if (this._stringValue != value)
			{
				this._stringValue = value;
				if (this.stringValueField != null)
				{
					this.stringValueField.text = this._stringValue;
				}
				this.AutoSetPreviewTextAndName();
			}
		}
	}

	// Token: 0x06006C78 RID: 27768 RVA: 0x0028D608 File Offset: 0x0028BA08
	protected virtual void SetColorFromHSV(float h, float s, float v)
	{
		if (this._HSVColorH != h || this._HSVColorS != s || this._HSVColorV != v)
		{
			this._HSVColorH = h;
			this._HSVColorS = s;
			this._HSVColorV = v;
			if (this.colorPicker != null)
			{
				this.colorPicker.SetHSV(h, s, v, true);
			}
			this.AutoSetPreviewTextAndName();
		}
	}

	// Token: 0x06006C79 RID: 27769 RVA: 0x0028D674 File Offset: 0x0028BA74
	protected void SetStringChooserValue(string v)
	{
		this.stringChooserValue = v;
	}

	// Token: 0x17000FF0 RID: 4080
	// (get) Token: 0x06006C7A RID: 27770 RVA: 0x0028D67D File Offset: 0x0028BA7D
	// (set) Token: 0x06006C7B RID: 27771 RVA: 0x0028D688 File Offset: 0x0028BA88
	public string stringChooserValue
	{
		get
		{
			return this._stringChooserValue;
		}
		set
		{
			if (this._stringChooserValue != value)
			{
				this._stringChooserValue = value;
				if (this.stringChooserValuePopup != null)
				{
					this.stringChooserValuePopup.currentValueNoCallback = this._stringChooserValue;
				}
				this.AutoSetPreviewTextAndName();
			}
		}
	}

	// Token: 0x06006C7C RID: 27772 RVA: 0x0028D6D8 File Offset: 0x0028BAD8
	public override void Validate()
	{
		if (base.receiver != null && base.receiverTargetName != null)
		{
			switch (this._actionType)
			{
			case JSONStorable.Type.Bool:
				if (!base.receiver.IsBoolJSONParam(base.receiverTargetName))
				{
					base.receiverTargetName = null;
				}
				break;
			case JSONStorable.Type.Float:
				if (!base.receiver.IsFloatJSONParam(base.receiverTargetName))
				{
					base.receiverTargetName = null;
				}
				break;
			case JSONStorable.Type.String:
				if (!base.receiver.IsStringJSONParam(base.receiverTargetName))
				{
					base.receiverTargetName = null;
				}
				break;
			case JSONStorable.Type.Url:
				if (!base.receiver.IsUrlJSONParam(base.receiverTargetName))
				{
					base.receiverTargetName = null;
				}
				break;
			case JSONStorable.Type.StringChooser:
				if (!base.receiver.IsStringChooserJSONParam(base.receiverTargetName))
				{
					base.receiverTargetName = null;
				}
				break;
			case JSONStorable.Type.Color:
				if (!base.receiver.IsColorJSONParam(base.receiverTargetName))
				{
					base.receiverTargetName = null;
				}
				break;
			case JSONStorable.Type.Action:
				if (!base.receiver.IsAction(base.receiverTargetName))
				{
					base.receiverTargetName = null;
				}
				break;
			case JSONStorable.Type.AudioClipAction:
				if (!base.receiver.IsAudioClipAction(base.receiverTargetName))
				{
					base.receiverTargetName = null;
				}
				if (this._audioClip != null && this._audioClip.destroyed)
				{
					this.audioClip = null;
				}
				break;
			case JSONStorable.Type.SceneFilePathAction:
				if (!base.receiver.IsSceneFilePathAction(base.receiverTargetName))
				{
					base.receiverTargetName = null;
				}
				break;
			case JSONStorable.Type.PresetFilePathAction:
				if (!base.receiver.IsPresetFilePathAction(base.receiverTargetName))
				{
					base.receiverTargetName = null;
				}
				break;
			case JSONStorable.Type.StringChooserAction:
				if (!base.receiver.IsStringChooserAction(base.receiverTargetName))
				{
					base.receiverTargetName = null;
				}
				break;
			}
		}
		base.Validate();
	}

	// Token: 0x06006C7D RID: 27773 RVA: 0x0028D8E4 File Offset: 0x0028BCE4
	public void Trigger(bool reverse = false, bool force = false)
	{
		this.CheckMissingReceiver();
		if ((base.enabled || force) && base.receiver != null && base.receiverTargetName != null)
		{
			switch (this._actionType)
			{
			case JSONStorable.Type.Bool:
				if (reverse)
				{
					base.receiver.SetBoolParamValue(base.receiverTargetName, !this.boolValue);
				}
				else
				{
					base.receiver.SetBoolParamValue(base.receiverTargetName, this.boolValue);
				}
				break;
			case JSONStorable.Type.Float:
				if (this._useTimer && this._timerLength > 0f)
				{
					this.SyncFloatCurve(false);
					if (reverse)
					{
						if (this._useSecondTimerPoint)
						{
							base.receiver.SetFloatParamValue(base.receiverTargetName, this._secondTimerPointValue);
						}
						else
						{
							base.receiver.SetFloatParamValue(base.receiverTargetName, this.floatValue);
						}
					}
					else
					{
						this.currentTimerTime = 0f;
						this.timerActive = true;
						this.handler.SetHasActiveTimer(this, true);
					}
				}
				else
				{
					base.receiver.SetFloatParamValue(base.receiverTargetName, this.floatValue);
				}
				break;
			case JSONStorable.Type.String:
				base.receiver.SetStringParamValue(base.receiverTargetName, this.stringValue);
				break;
			case JSONStorable.Type.Url:
				base.receiver.SetUrlParamValue(base.receiverTargetName, this.stringValue);
				break;
			case JSONStorable.Type.StringChooser:
				base.receiver.SetStringChooserParamValue(base.receiverTargetName, this.stringChooserValue);
				break;
			case JSONStorable.Type.Color:
				this.triggerColor.H = this._HSVColorH;
				this.triggerColor.S = this._HSVColorS;
				this.triggerColor.V = this._HSVColorV;
				base.receiver.SetColorParamValue(base.receiverTargetName, this.triggerColor);
				break;
			case JSONStorable.Type.Action:
				if (!reverse || this.doActionsInReverse)
				{
					base.receiver.CallAction(base.receiverTargetName);
				}
				break;
			case JSONStorable.Type.AudioClipAction:
				if (this.audioClip != null)
				{
					if (reverse)
					{
						base.receiver.CallAction("Stop");
					}
					else
					{
						base.receiver.CallAction(base.receiverTargetName, this.audioClip);
					}
				}
				break;
			case JSONStorable.Type.SceneFilePathAction:
				if (this.sceneFilePath != string.Empty && !reverse)
				{
					base.receiver.CallAction(base.receiverTargetName, this.sceneFilePath);
				}
				break;
			case JSONStorable.Type.PresetFilePathAction:
				if (this.presetFilePath != string.Empty && !reverse)
				{
					base.receiver.CallPresetFileAction(base.receiverTargetName, this.presetFilePath);
				}
				break;
			case JSONStorable.Type.StringChooserAction:
				if (!reverse || this.doActionsInReverse)
				{
					base.receiver.CallStringChooserAction(base.receiverTargetName, this._stringChooserActionChoice);
				}
				break;
			}
		}
	}

	// Token: 0x06006C7E RID: 27774 RVA: 0x0028DBFC File Offset: 0x0028BFFC
	public void Update()
	{
		if (this.timerActive)
		{
			if (this._receiver != null && this._receiverTargetName != null)
			{
				this.currentTimerTime += Time.deltaTime;
				if (this.currentTimerTime < this._timerLength)
				{
					base.receiver.SetFloatParamValue(base.receiverTargetName, Mathf.Clamp(this.floatCurve.Evaluate(this.currentTimerTime), this.paramMin, this.paramMax));
				}
				else
				{
					base.receiver.SetFloatParamValue(base.receiverTargetName, this.floatCurve.Evaluate(this._timerLength));
					this.timerActive = false;
					this.handler.SetHasActiveTimer(this, false);
				}
			}
			else
			{
				this.timerActive = false;
				this.handler.SetHasActiveTimer(this, false);
			}
		}
	}

	// Token: 0x06006C7F RID: 27775 RVA: 0x0028DCDC File Offset: 0x0028C0DC
	public override void InitTriggerActionPanelUI()
	{
		this.CheckMissingReceiver();
		base.InitTriggerActionPanelUI();
		if (this.triggerActionPanel != null)
		{
			TriggerActionDiscreteUI component = this.triggerActionPanel.GetComponent<TriggerActionDiscreteUI>();
			if (component != null)
			{
				this.copyButton = component.copyButton;
				this.pasteButton = component.pasteButton;
				this.testButton = component.testButton;
				this.resetTestButton = component.resetTestButton;
				this.audioClipPopupsContainer = component.audioClipPopupsContainer;
				this.audioClipTypePopup = component.audioClipTypePopup;
				this.audioClipCategoryPopup = component.audioClipCategoryPopup;
				this.audioClipPopup = component.audioClipPopup;
				this.stringChooserActionPopup = component.stringChooserActionPopup;
				this.sceneFilePathText = component.sceneFilePathText;
				this.presetFilePathText = component.presetFilePathText;
				this.chooseSceneFilePathButton = component.chooseSceneFilePathButton;
				this.choosePresetFilePathButton = component.choosePresetFilePathButton;
				this.boolValueToggle = component.boolValueToggle;
				this.floatValueSlider = component.floatValueSlider;
				this.floatValueDynamicSlider = component.floatValueDynamicSlider;
				this.useTimerToggle = component.useTimerToggle;
				this.curveContainer = component.curveContainer;
				this.curveLineRenderer = component.curveLineRenderer;
				this.timerLengthSlider = component.timerLengthSlider;
				this.timerTypePopup = component.timerTypePopup;
				this.useSecondTimerPointToggle = component.useSecondTimerPointToggle;
				this.secondTimerPointValueSlider = component.secondTimerPointValueSlider;
				this.secondTimerPointValueDynamicSlider = component.secondTimerPointValueDynamicSlider;
				this.secondTimerPointCurveLocationSlider = component.secondTimerPointCurveLocationSlider;
				this.colorPickerContainer = component.colorPickerContainer;
				this.colorPicker = component.colorPicker;
				this.stringValueField = component.stringValueField;
				this.stringValueFieldAction = component.stringValueFieldAction;
				this.stringChooserValuePopup = component.stringChooserValuePopup;
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
		if (this.testButton != null)
		{
			this.testButton.onClick.AddListener(new UnityAction(this.Test));
		}
		if (this.resetTestButton != null)
		{
			this.resetTestButton.interactable = false;
			this.resetTestButton.onClick.AddListener(new UnityAction(this.ResetTest));
		}
		if (this.audioClipTypePopup != null)
		{
			this.audioClipTypePopup.numPopupValues = 2;
			this.audioClipTypePopup.setPopupValue(0, "Embedded");
			this.audioClipTypePopup.setPopupValue(1, "URL");
			this.audioClipTypePopup.currentValue = this._audioClipType.ToString();
			UIPopup uipopup = this.audioClipTypePopup;
			uipopup.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetAudioClipType));
		}
		if (this.audioClipCategoryPopup != null)
		{
			this.SetClipCategoryPopupValues();
			if (this._audioClipCategory == null)
			{
				this.audioClipCategoryPopup.currentValue = "None";
			}
			else
			{
				this.audioClipCategoryPopup.currentValue = this._audioClipCategory;
			}
			UIPopup uipopup2 = this.audioClipCategoryPopup;
			uipopup2.onOpenPopupHandlers = (UIPopup.OnOpenPopup)Delegate.Combine(uipopup2.onOpenPopupHandlers, new UIPopup.OnOpenPopup(this.SetClipCategoryPopupValues));
			UIPopup uipopup3 = this.audioClipCategoryPopup;
			uipopup3.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup3.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetAudioClipCategory));
		}
		if (this.audioClipPopup != null)
		{
			this.SetClipPopupValues();
			if (this._audioClip == null)
			{
				this.audioClipPopup.currentValue = "None";
			}
			else
			{
				this.audioClipPopup.currentValue = this._audioClip.uid;
			}
			UIPopup uipopup4 = this.audioClipPopup;
			uipopup4.onOpenPopupHandlers = (UIPopup.OnOpenPopup)Delegate.Combine(uipopup4.onOpenPopupHandlers, new UIPopup.OnOpenPopup(this.SetClipPopupValues));
			UIPopup uipopup5 = this.audioClipPopup;
			uipopup5.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup5.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetAudioClip));
		}
		if (this.stringChooserActionPopup != null)
		{
			this.SetStringChooserActionPopupValues();
			this.stringChooserActionPopup.currentValue = this._stringChooserActionChoice;
			UIPopup uipopup6 = this.stringChooserActionPopup;
			uipopup6.onOpenPopupHandlers = (UIPopup.OnOpenPopup)Delegate.Combine(uipopup6.onOpenPopupHandlers, new UIPopup.OnOpenPopup(this.SetStringChooserActionPopupValues));
			UIPopup uipopup7 = this.stringChooserActionPopup;
			uipopup7.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup7.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetStringChooserActionChoice));
		}
		if (this.chooseSceneFilePathButton != null)
		{
			this.chooseSceneFilePathButton.onClick.AddListener(new UnityAction(this.GetSceneFilePath));
		}
		if (this.choosePresetFilePathButton != null)
		{
			this.choosePresetFilePathButton.onClick.AddListener(new UnityAction(this.GetPresetFilePath));
		}
		if (this.sceneFilePathText != null)
		{
			this.sceneFilePathText.text = this._sceneFilePath;
		}
		if (this.presetFilePathText != null)
		{
			this.presetFilePathText.text = this._presetFilePath;
		}
		if (this.boolValueToggle != null)
		{
			this.boolValueToggle.isOn = this._boolValue;
			this.boolValueToggle.onValueChanged.AddListener(new UnityAction<bool>(this.<InitTriggerActionPanelUI>m__0));
		}
		if (this.floatValueSlider != null)
		{
			this.floatValueSlider.minValue = this.paramMin;
			if (this.floatValueSlider.minValue > this._floatValue)
			{
				this.floatValueSlider.minValue = this._floatValue;
			}
			this.floatValueSlider.maxValue = this.paramMax;
			if (this.floatValueSlider.maxValue < this._floatValue)
			{
				this.floatValueSlider.maxValue = this._floatValue;
			}
			this.floatValueSlider.value = this._floatValue;
			this.floatValueSlider.onValueChanged.AddListener(new UnityAction<float>(this.SetFloatValue));
		}
		if (this.floatValueDynamicSlider != null)
		{
			this.floatValueDynamicSlider.rangeAdjustEnabled = !this.paramContrained;
			this.floatValueDynamicSlider.defaultValue = this.paramDefault;
			if (this.receiverTargetFloat != null)
			{
				this.floatValueDynamicSlider.label = this.receiverTargetFloat.name;
			}
		}
		if (this.useTimerToggle != null)
		{
			this.useTimerToggle.isOn = this._useTimer;
			this.useTimerToggle.onValueChanged.AddListener(new UnityAction<bool>(this.SetUseTimer));
		}
		if (this.timerLengthSlider != null)
		{
			if (this._timerLength > this.timerLengthSlider.maxValue)
			{
				this.timerLengthSlider.maxValue = this._timerLength;
			}
			this.timerLengthSlider.value = this._timerLength;
			this.timerLengthSlider.onValueChanged.AddListener(new UnityAction<float>(this.SetTimerLength));
		}
		if (this.timerTypePopup != null)
		{
			this.timerTypePopup.currentValueNoCallback = this._timerType.ToString();
			UIPopup uipopup8 = this.timerTypePopup;
			uipopup8.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup8.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetTimerType));
		}
		if (this.useSecondTimerPointToggle != null)
		{
			this.useSecondTimerPointToggle.isOn = this._useSecondTimerPoint;
			this.useSecondTimerPointToggle.onValueChanged.AddListener(new UnityAction<bool>(this.SetUseSecondTimerPoint));
		}
		if (this.secondTimerPointValueSlider != null)
		{
			this.secondTimerPointValueSlider.minValue = this.paramMin;
			if (this.secondTimerPointValueSlider.minValue > this._secondTimerPointValue)
			{
				this.secondTimerPointValueSlider.minValue = this._secondTimerPointValue;
			}
			this.secondTimerPointValueSlider.maxValue = this.paramMax;
			if (this.secondTimerPointValueSlider.maxValue < this._secondTimerPointValue)
			{
				this.secondTimerPointValueSlider.maxValue = this._secondTimerPointValue;
			}
			this.secondTimerPointValueSlider.value = this._secondTimerPointValue;
			this.secondTimerPointValueSlider.onValueChanged.AddListener(new UnityAction<float>(this.SetSecondTimerFloatValue));
		}
		if (this.secondTimerPointValueDynamicSlider != null)
		{
			this.secondTimerPointValueDynamicSlider.rangeAdjustEnabled = !this.paramContrained;
			this.secondTimerPointValueDynamicSlider.defaultValue = this.paramDefault;
		}
		if (this.secondTimerPointCurveLocationSlider != null)
		{
			this.secondTimerPointCurveLocationSlider.value = this._secondTimerPointCurveLocation;
			this.secondTimerPointCurveLocationSlider.onValueChanged.AddListener(new UnityAction<float>(this.SetSecondTimerPointCurveLocation));
		}
		this.SyncFloatCurve(false);
		this.SyncFloatCurveRender();
		if (this.colorPicker != null)
		{
			this.colorPicker.SetHSV(this._HSVColorH, this._HSVColorS, this._HSVColorV, false);
			HSVColorPicker hsvcolorPicker = this.colorPicker;
			hsvcolorPicker.onHSVColorChangedHandlers = (HSVColorPicker.OnHSVColorChanged)Delegate.Combine(hsvcolorPicker.onHSVColorChangedHandlers, new HSVColorPicker.OnHSVColorChanged(this.SetColorFromHSV));
		}
		if (this.stringValueField != null)
		{
			this.stringValueField.text = this._stringValue;
			this.stringValueField.onEndEdit.AddListener(new UnityAction<string>(this.SetStringValue));
		}
		if (this.stringValueFieldAction != null)
		{
			InputFieldAction inputFieldAction = this.stringValueFieldAction;
			inputFieldAction.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Combine(inputFieldAction.onSubmitHandlers, new InputFieldAction.OnSubmit(this.SetStringValueToField));
		}
		if (this.stringChooserValuePopup != null)
		{
			this.SyncStringChooserPopup();
			this.stringChooserValuePopup.currentValue = this._stringChooserValue;
			UIPopup uipopup9 = this.stringChooserValuePopup;
			uipopup9.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Combine(uipopup9.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetStringChooserValue));
		}
		this.SyncType();
	}

	// Token: 0x06006C80 RID: 27776 RVA: 0x0028E6D8 File Offset: 0x0028CAD8
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
		if (this.testButton != null)
		{
			this.testButton.onClick.RemoveListener(new UnityAction(this.Test));
		}
		if (this.resetTestButton != null)
		{
			this.resetTestButton.onClick.RemoveListener(new UnityAction(this.ResetTest));
		}
		if (this.audioClipTypePopup != null)
		{
			UIPopup uipopup = this.audioClipTypePopup;
			uipopup.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Remove(uipopup.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetAudioClipType));
		}
		if (this.audioClipCategoryPopup != null)
		{
			UIPopup uipopup2 = this.audioClipCategoryPopup;
			uipopup2.onOpenPopupHandlers = (UIPopup.OnOpenPopup)Delegate.Remove(uipopup2.onOpenPopupHandlers, new UIPopup.OnOpenPopup(this.SetClipCategoryPopupValues));
			UIPopup uipopup3 = this.audioClipCategoryPopup;
			uipopup3.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Remove(uipopup3.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetAudioClipCategory));
		}
		if (this.audioClipPopup != null)
		{
			UIPopup uipopup4 = this.audioClipPopup;
			uipopup4.onOpenPopupHandlers = (UIPopup.OnOpenPopup)Delegate.Remove(uipopup4.onOpenPopupHandlers, new UIPopup.OnOpenPopup(this.SetClipPopupValues));
			UIPopup uipopup5 = this.audioClipPopup;
			uipopup5.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Remove(uipopup5.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetAudioClip));
		}
		if (this.stringChooserActionPopup != null)
		{
			UIPopup uipopup6 = this.stringChooserActionPopup;
			uipopup6.onOpenPopupHandlers = (UIPopup.OnOpenPopup)Delegate.Remove(uipopup6.onOpenPopupHandlers, new UIPopup.OnOpenPopup(this.SetStringChooserActionPopupValues));
			UIPopup uipopup7 = this.stringChooserActionPopup;
			uipopup7.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Remove(uipopup7.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetStringChooserActionChoice));
		}
		if (this.chooseSceneFilePathButton != null)
		{
			this.chooseSceneFilePathButton.onClick.RemoveListener(new UnityAction(this.GetSceneFilePath));
		}
		if (this.choosePresetFilePathButton != null)
		{
			this.choosePresetFilePathButton.onClick.RemoveListener(new UnityAction(this.GetPresetFilePath));
		}
		if (this.boolValueToggle != null)
		{
			this.boolValueToggle.onValueChanged.RemoveAllListeners();
		}
		if (this.floatValueSlider != null)
		{
			this.floatValueSlider.onValueChanged.RemoveListener(new UnityAction<float>(this.SetFloatValue));
		}
		if (this.useTimerToggle != null)
		{
			this.useTimerToggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.SetUseTimer));
		}
		if (this.timerLengthSlider != null)
		{
			this.timerLengthSlider.onValueChanged.RemoveListener(new UnityAction<float>(this.SetTimerLength));
		}
		if (this.timerTypePopup != null)
		{
			UIPopup uipopup8 = this.timerTypePopup;
			uipopup8.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Remove(uipopup8.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetTimerType));
		}
		if (this.useSecondTimerPointToggle != null)
		{
			this.useSecondTimerPointToggle.onValueChanged.RemoveListener(new UnityAction<bool>(this.SetUseSecondTimerPoint));
		}
		if (this.secondTimerPointValueSlider != null)
		{
			this.secondTimerPointValueSlider.onValueChanged.RemoveListener(new UnityAction<float>(this.SetSecondTimerFloatValue));
		}
		if (this.secondTimerPointCurveLocationSlider != null)
		{
			this.secondTimerPointCurveLocationSlider.onValueChanged.RemoveListener(new UnityAction<float>(this.SetSecondTimerPointCurveLocation));
		}
		if (this.colorPicker != null)
		{
			HSVColorPicker hsvcolorPicker = this.colorPicker;
			hsvcolorPicker.onHSVColorChangedHandlers = (HSVColorPicker.OnHSVColorChanged)Delegate.Remove(hsvcolorPicker.onHSVColorChangedHandlers, new HSVColorPicker.OnHSVColorChanged(this.SetColorFromHSV));
		}
		if (this.stringValueField != null)
		{
			this.stringValueField.onEndEdit.RemoveListener(new UnityAction<string>(this.SetStringValue));
		}
		if (this.stringValueFieldAction != null)
		{
			InputFieldAction inputFieldAction = this.stringValueFieldAction;
			inputFieldAction.onSubmitHandlers = (InputFieldAction.OnSubmit)Delegate.Remove(inputFieldAction.onSubmitHandlers, new InputFieldAction.OnSubmit(this.SetStringValueToField));
		}
		if (this.stringChooserValuePopup != null)
		{
			UIPopup uipopup9 = this.stringChooserValuePopup;
			uipopup9.onValueChangeHandlers = (UIPopup.OnValueChange)Delegate.Remove(uipopup9.onValueChangeHandlers, new UIPopup.OnValueChange(this.SetStringChooserValue));
		}
	}

	// Token: 0x06006C81 RID: 27777 RVA: 0x0028EB8B File Offset: 0x0028CF8B
	[CompilerGenerated]
	private void <InitTriggerActionPanelUI>m__0(bool A_1)
	{
		this.boolValue = this.boolValueToggle.isOn;
	}

	// Token: 0x04005D9D RID: 23965
	public static JSONClass copyOfData;

	// Token: 0x04005D9E RID: 23966
	protected Button copyButton;

	// Token: 0x04005D9F RID: 23967
	protected Button pasteButton;

	// Token: 0x04005DA0 RID: 23968
	protected Button testButton;

	// Token: 0x04005DA1 RID: 23969
	protected Button resetTestButton;

	// Token: 0x04005DA2 RID: 23970
	protected bool beforeTestStateCaptured;

	// Token: 0x04005DA3 RID: 23971
	protected JSONStorable.Type _actionType;

	// Token: 0x04005DA4 RID: 23972
	protected RectTransform audioClipPopupsContainer;

	// Token: 0x04005DA5 RID: 23973
	protected UIPopup audioClipTypePopup;

	// Token: 0x04005DA6 RID: 23974
	protected TriggerActionDiscrete.AudioClipType _audioClipType;

	// Token: 0x04005DA7 RID: 23975
	protected UIPopup audioClipCategoryPopup;

	// Token: 0x04005DA8 RID: 23976
	protected string _audioClipCategory;

	// Token: 0x04005DA9 RID: 23977
	protected UIPopup audioClipPopup;

	// Token: 0x04005DAA RID: 23978
	protected NamedAudioClip _audioClip;

	// Token: 0x04005DAB RID: 23979
	protected string _stringChooserActionChoice;

	// Token: 0x04005DAC RID: 23980
	protected UIPopup stringChooserActionPopup;

	// Token: 0x04005DAD RID: 23981
	protected Button chooseSceneFilePathButton;

	// Token: 0x04005DAE RID: 23982
	protected Text sceneFilePathText;

	// Token: 0x04005DAF RID: 23983
	protected string _sceneFilePath;

	// Token: 0x04005DB0 RID: 23984
	protected Button choosePresetFilePathButton;

	// Token: 0x04005DB1 RID: 23985
	protected Text presetFilePathText;

	// Token: 0x04005DB2 RID: 23986
	protected string _presetFilePath;

	// Token: 0x04005DB3 RID: 23987
	protected Toggle boolValueToggle;

	// Token: 0x04005DB4 RID: 23988
	protected bool beforeTestBoolValue;

	// Token: 0x04005DB5 RID: 23989
	protected bool _boolValue;

	// Token: 0x04005DB6 RID: 23990
	protected UIDynamicSlider floatValueDynamicSlider;

	// Token: 0x04005DB7 RID: 23991
	protected Slider floatValueSlider;

	// Token: 0x04005DB8 RID: 23992
	protected float beforeTestFloatValue;

	// Token: 0x04005DB9 RID: 23993
	protected float _floatValue;

	// Token: 0x04005DBA RID: 23994
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private bool <timerActive>k__BackingField;

	// Token: 0x04005DBB RID: 23995
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private float <currentTimerTime>k__BackingField;

	// Token: 0x04005DBC RID: 23996
	protected TriggerActionDiscrete.TimerType _curveTimerType;

	// Token: 0x04005DBD RID: 23997
	protected float _curveTimerLength;

	// Token: 0x04005DBE RID: 23998
	protected float _curveValueStart;

	// Token: 0x04005DBF RID: 23999
	protected float _curveValueEnd;

	// Token: 0x04005DC0 RID: 24000
	protected bool _curveUseSecondPoint;

	// Token: 0x04005DC1 RID: 24001
	protected float _curveSecondPointValue;

	// Token: 0x04005DC2 RID: 24002
	protected float _curveSecondPointLocation;

	// Token: 0x04005DC3 RID: 24003
	[CompilerGenerated]
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private AnimationCurve <floatCurve>k__BackingField;

	// Token: 0x04005DC4 RID: 24004
	protected RectTransform curveContainer;

	// Token: 0x04005DC5 RID: 24005
	protected UILineRenderer curveLineRenderer;

	// Token: 0x04005DC6 RID: 24006
	protected Toggle useTimerToggle;

	// Token: 0x04005DC7 RID: 24007
	protected bool _useTimer;

	// Token: 0x04005DC8 RID: 24008
	protected Slider timerLengthSlider;

	// Token: 0x04005DC9 RID: 24009
	protected float _timerLength = 0.5f;

	// Token: 0x04005DCA RID: 24010
	protected float _oneOverTimerLength = 2f;

	// Token: 0x04005DCB RID: 24011
	protected UIPopup timerTypePopup;

	// Token: 0x04005DCC RID: 24012
	protected TriggerActionDiscrete.TimerType _timerType;

	// Token: 0x04005DCD RID: 24013
	protected Toggle useSecondTimerPointToggle;

	// Token: 0x04005DCE RID: 24014
	protected bool _useSecondTimerPoint;

	// Token: 0x04005DCF RID: 24015
	protected UIDynamicSlider secondTimerPointValueDynamicSlider;

	// Token: 0x04005DD0 RID: 24016
	protected Slider secondTimerPointValueSlider;

	// Token: 0x04005DD1 RID: 24017
	protected float _secondTimerPointValue;

	// Token: 0x04005DD2 RID: 24018
	protected Slider secondTimerPointCurveLocationSlider;

	// Token: 0x04005DD3 RID: 24019
	protected float _secondTimerPointCurveLocation = 0.5f;

	// Token: 0x04005DD4 RID: 24020
	protected InputField stringValueField;

	// Token: 0x04005DD5 RID: 24021
	protected InputFieldAction stringValueFieldAction;

	// Token: 0x04005DD6 RID: 24022
	protected string beforeTestStringValue;

	// Token: 0x04005DD7 RID: 24023
	protected string _stringValue;

	// Token: 0x04005DD8 RID: 24024
	protected RectTransform colorPickerContainer;

	// Token: 0x04005DD9 RID: 24025
	protected HSVColorPicker colorPicker;

	// Token: 0x04005DDA RID: 24026
	protected float _HSVColorH;

	// Token: 0x04005DDB RID: 24027
	protected float _HSVColorS;

	// Token: 0x04005DDC RID: 24028
	protected float _HSVColorV;

	// Token: 0x04005DDD RID: 24029
	protected UIPopup stringChooserValuePopup;

	// Token: 0x04005DDE RID: 24030
	protected string beforeTestStringChooserValue;

	// Token: 0x04005DDF RID: 24031
	protected string _stringChooserValue;

	// Token: 0x04005DE0 RID: 24032
	protected HSVColor beforeTestTriggerColor;

	// Token: 0x04005DE1 RID: 24033
	protected HSVColor triggerColor;

	// Token: 0x04005DE2 RID: 24034
	public bool doActionsInReverse = true;

	// Token: 0x02000DA9 RID: 3497
	public enum AudioClipType
	{
		// Token: 0x04005DE4 RID: 24036
		Embedded,
		// Token: 0x04005DE5 RID: 24037
		URL
	}

	// Token: 0x02000DAA RID: 3498
	public enum TimerType
	{
		// Token: 0x04005DE7 RID: 24039
		EaseInOut,
		// Token: 0x04005DE8 RID: 24040
		Linear
	}
}
