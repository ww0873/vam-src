using System;
using System.Collections;
using System.Collections.Generic;
using MVR.FileManagementSecure;
using SimpleJSON;
using UnityEngine;

// Token: 0x02000C38 RID: 3128
public class MVRScript : JSONStorable
{
	// Token: 0x06005AFB RID: 23291 RVA: 0x00216E03 File Offset: 0x00215203
	public MVRScript()
	{
	}

	// Token: 0x06005AFC RID: 23292 RVA: 0x00216E0B File Offset: 0x0021520B
	public virtual bool ShouldIgnore()
	{
		return false;
	}

	// Token: 0x06005AFD RID: 23293 RVA: 0x00216E0E File Offset: 0x0021520E
	public Atom GetContainingAtom()
	{
		return this.containingAtom;
	}

	// Token: 0x06005AFE RID: 23294 RVA: 0x00216E18 File Offset: 0x00215218
	public List<Atom> GetSceneAtoms()
	{
		List<Atom> result = null;
		if (SuperController.singleton != null)
		{
			result = SuperController.singleton.GetAtoms();
		}
		return result;
	}

	// Token: 0x06005AFF RID: 23295 RVA: 0x00216E44 File Offset: 0x00215244
	public List<string> GetAtomUIDs()
	{
		List<string> result = null;
		if (SuperController.singleton != null)
		{
			result = SuperController.singleton.GetAtomUIDs();
		}
		return result;
	}

	// Token: 0x06005B00 RID: 23296 RVA: 0x00216E70 File Offset: 0x00215270
	public List<string> GetVisibleAtomUIDs()
	{
		List<string> result = null;
		if (SuperController.singleton != null)
		{
			result = SuperController.singleton.GetVisibleAtomUIDs();
		}
		return result;
	}

	// Token: 0x06005B01 RID: 23297 RVA: 0x00216E9B File Offset: 0x0021529B
	public Atom GetAtomById(string uid)
	{
		if (SuperController.singleton != null)
		{
			return SuperController.singleton.GetAtomByUid(uid);
		}
		return null;
	}

	// Token: 0x06005B02 RID: 23298 RVA: 0x00216EBA File Offset: 0x002152BA
	public FreeControllerV3 GetMainController()
	{
		return this.containingAtom.mainController;
	}

	// Token: 0x06005B03 RID: 23299 RVA: 0x00216EC7 File Offset: 0x002152C7
	public FreeControllerV3[] GetAllControllers()
	{
		return this.containingAtom.freeControllers;
	}

	// Token: 0x06005B04 RID: 23300 RVA: 0x00216ED4 File Offset: 0x002152D4
	public void SelectController(FreeControllerV3 fc, bool alignView = false)
	{
		if (SuperController.singleton != null)
		{
			SuperController.singleton.SelectController(fc, alignView, true, true, true);
		}
	}

	// Token: 0x06005B05 RID: 23301 RVA: 0x00216EF8 File Offset: 0x002152F8
	public bool GetLeftPointerShow()
	{
		bool result = false;
		if (SuperController.singleton != null)
		{
			result = SuperController.singleton.GetLeftUIPointerShow();
		}
		return result;
	}

	// Token: 0x06005B06 RID: 23302 RVA: 0x00216F24 File Offset: 0x00215324
	public bool GetRightPointerShow()
	{
		bool result = false;
		if (SuperController.singleton != null)
		{
			result = SuperController.singleton.GetRightUIPointerShow();
		}
		return result;
	}

	// Token: 0x06005B07 RID: 23303 RVA: 0x00216F50 File Offset: 0x00215350
	public bool GetLeftSelect()
	{
		bool result = false;
		if (SuperController.singleton != null)
		{
			result = SuperController.singleton.GetLeftSelect();
		}
		return result;
	}

	// Token: 0x06005B08 RID: 23304 RVA: 0x00216F7C File Offset: 0x0021537C
	public bool GetRightSelect()
	{
		bool result = false;
		if (SuperController.singleton != null)
		{
			result = SuperController.singleton.GetRightSelect();
		}
		return result;
	}

	// Token: 0x06005B09 RID: 23305 RVA: 0x00216FA8 File Offset: 0x002153A8
	public float GetLeftTriggerVal()
	{
		float result = 0f;
		if (SuperController.singleton != null)
		{
			result = SuperController.singleton.GetLeftGrabVal();
		}
		return result;
	}

	// Token: 0x06005B0A RID: 23306 RVA: 0x00216FD8 File Offset: 0x002153D8
	public float GetRightTriggerVal()
	{
		float result = 0f;
		if (SuperController.singleton != null)
		{
			result = SuperController.singleton.GetRightGrabVal();
		}
		return result;
	}

	// Token: 0x06005B0B RID: 23307 RVA: 0x00217008 File Offset: 0x00215408
	public bool GetLeftQuickGrab()
	{
		bool result = false;
		if (SuperController.singleton != null)
		{
			result = SuperController.singleton.GetLeftGrab();
		}
		return result;
	}

	// Token: 0x06005B0C RID: 23308 RVA: 0x00217034 File Offset: 0x00215434
	public bool GetRightQuickGrab()
	{
		bool result = false;
		if (SuperController.singleton != null)
		{
			result = SuperController.singleton.GetRightGrab();
		}
		return result;
	}

	// Token: 0x06005B0D RID: 23309 RVA: 0x00217060 File Offset: 0x00215460
	public bool GetLeftQuickRelease()
	{
		bool result = false;
		if (SuperController.singleton != null)
		{
			result = SuperController.singleton.GetLeftGrabRelease();
		}
		return result;
	}

	// Token: 0x06005B0E RID: 23310 RVA: 0x0021708C File Offset: 0x0021548C
	public bool GetRightQuickRelease()
	{
		bool result = false;
		if (SuperController.singleton != null)
		{
			result = SuperController.singleton.GetRightGrabRelease();
		}
		return result;
	}

	// Token: 0x06005B0F RID: 23311 RVA: 0x002170B8 File Offset: 0x002154B8
	public bool GetLeftGrabToggle()
	{
		bool result = false;
		if (SuperController.singleton != null)
		{
			result = SuperController.singleton.GetLeftHoldGrab();
		}
		return result;
	}

	// Token: 0x06005B10 RID: 23312 RVA: 0x002170E4 File Offset: 0x002154E4
	public bool GetRightGrabToggle()
	{
		bool result = false;
		if (SuperController.singleton != null)
		{
			result = SuperController.singleton.GetRightHoldGrab();
		}
		return result;
	}

	// Token: 0x06005B11 RID: 23313 RVA: 0x0021710F File Offset: 0x0021550F
	public void SaveJSON(JSONClass jc, string saveName)
	{
		if (SuperController.singleton != null)
		{
			SuperController.singleton.SaveJSON(jc, saveName);
		}
	}

	// Token: 0x06005B12 RID: 23314 RVA: 0x0021712D File Offset: 0x0021552D
	public void SaveJSON(JSONClass jc, string saveName, UserActionCallback confirmCallback, UserActionCallback denyCallback, ExceptionCallback exceptionCallback)
	{
		if (SuperController.singleton != null)
		{
			SuperController.singleton.SaveJSON(jc, saveName, confirmCallback, denyCallback, exceptionCallback);
		}
	}

	// Token: 0x06005B13 RID: 23315 RVA: 0x00217150 File Offset: 0x00215550
	public JSONNode LoadJSON(string saveName)
	{
		if (SuperController.singleton != null)
		{
			return SuperController.singleton.LoadJSON(saveName);
		}
		return null;
	}

	// Token: 0x06005B14 RID: 23316 RVA: 0x00217170 File Offset: 0x00215570
	public UIDynamicSlider CreateSlider(JSONStorableFloat jsf, bool rightSide = false)
	{
		UIDynamicSlider uidynamicSlider = null;
		if (this.manager != null && this.manager.configurableSliderPrefab != null && jsf.slider == null)
		{
			Transform transform = this.CreateUIElement(this.manager.configurableSliderPrefab.transform, rightSide);
			if (transform != null)
			{
				uidynamicSlider = transform.GetComponent<UIDynamicSlider>();
				if (uidynamicSlider != null)
				{
					uidynamicSlider.Configure(jsf.name, jsf.min, jsf.max, jsf.val, jsf.constrained, "F2", true, !jsf.constrained);
					jsf.slider = uidynamicSlider.slider;
					this.sliderToJSONStorableFloat.Add(uidynamicSlider, jsf);
				}
			}
		}
		return uidynamicSlider;
	}

	// Token: 0x06005B15 RID: 23317 RVA: 0x0021723C File Offset: 0x0021563C
	public void RemoveSlider(JSONStorableFloat jsf)
	{
		if (jsf != null && jsf.slider != null)
		{
			UIDynamicSlider componentInParent = jsf.slider.GetComponentInParent<UIDynamicSlider>();
			if (componentInParent != null)
			{
				this.sliderToJSONStorableFloat.Remove(componentInParent);
				Transform transform = componentInParent.transform;
				this.rightUIElements.Remove(transform);
				this.leftUIElements.Remove(transform);
				jsf.slider = null;
				UnityEngine.Object.Destroy(transform.gameObject);
			}
		}
	}

	// Token: 0x06005B16 RID: 23318 RVA: 0x002172B8 File Offset: 0x002156B8
	public void RemoveSlider(UIDynamicSlider dslider)
	{
		JSONStorableFloat jsonstorableFloat;
		if (this.sliderToJSONStorableFloat.TryGetValue(dslider, out jsonstorableFloat))
		{
			this.sliderToJSONStorableFloat.Remove(dslider);
			Transform transform = dslider.transform;
			this.rightUIElements.Remove(transform);
			this.leftUIElements.Remove(transform);
			jsonstorableFloat.slider = null;
			UnityEngine.Object.Destroy(transform.gameObject);
		}
	}

	// Token: 0x06005B17 RID: 23319 RVA: 0x00217318 File Offset: 0x00215718
	public UIDynamicToggle CreateToggle(JSONStorableBool jsb, bool rightSide = false)
	{
		UIDynamicToggle uidynamicToggle = null;
		if (this.manager != null && this.manager.configurableTogglePrefab != null && jsb.toggle == null)
		{
			Transform transform = this.CreateUIElement(this.manager.configurableTogglePrefab.transform, rightSide);
			if (transform != null)
			{
				uidynamicToggle = transform.GetComponent<UIDynamicToggle>();
				if (uidynamicToggle != null)
				{
					this.toggleToJSONStorableBool.Add(uidynamicToggle, jsb);
					uidynamicToggle.label = jsb.name;
					jsb.toggle = uidynamicToggle.toggle;
				}
			}
		}
		return uidynamicToggle;
	}

	// Token: 0x06005B18 RID: 23320 RVA: 0x002173BC File Offset: 0x002157BC
	public void RemoveToggle(JSONStorableBool jsb)
	{
		if (jsb != null && jsb.toggle != null)
		{
			UIDynamicToggle component = jsb.toggle.GetComponent<UIDynamicToggle>();
			if (component != null)
			{
				this.toggleToJSONStorableBool.Remove(component);
			}
			Transform transform = jsb.toggle.transform;
			this.rightUIElements.Remove(transform);
			this.leftUIElements.Remove(transform);
			jsb.toggle = null;
			UnityEngine.Object.Destroy(transform.gameObject);
		}
	}

	// Token: 0x06005B19 RID: 23321 RVA: 0x00217440 File Offset: 0x00215840
	public void RemoveToggle(UIDynamicToggle dtoggle)
	{
		JSONStorableBool jsonstorableBool;
		if (this.toggleToJSONStorableBool.TryGetValue(dtoggle, out jsonstorableBool))
		{
			this.toggleToJSONStorableBool.Remove(dtoggle);
			Transform transform = jsonstorableBool.toggle.transform;
			this.rightUIElements.Remove(transform);
			this.leftUIElements.Remove(transform);
			jsonstorableBool.toggle = null;
			UnityEngine.Object.Destroy(transform.gameObject);
		}
	}

	// Token: 0x06005B1A RID: 23322 RVA: 0x002174A8 File Offset: 0x002158A8
	public UIDynamicColorPicker CreateColorPicker(JSONStorableColor jsc, bool rightSide = false)
	{
		UIDynamicColorPicker uidynamicColorPicker = null;
		if (this.manager != null && this.manager.configurableColorPickerPrefab != null && jsc.colorPicker == null)
		{
			Transform transform = this.CreateUIElement(this.manager.configurableColorPickerPrefab.transform, rightSide);
			if (transform != null)
			{
				uidynamicColorPicker = transform.GetComponent<UIDynamicColorPicker>();
				if (uidynamicColorPicker != null)
				{
					this.colorPickerToJSONStorableColor.Add(uidynamicColorPicker, jsc);
					uidynamicColorPicker.label = jsc.name;
					jsc.colorPicker = uidynamicColorPicker.colorPicker;
				}
			}
		}
		return uidynamicColorPicker;
	}

	// Token: 0x06005B1B RID: 23323 RVA: 0x0021754C File Offset: 0x0021594C
	public void RemoveColorPicker(JSONStorableColor jsc)
	{
		if (jsc != null && jsc.colorPicker != null)
		{
			UIDynamicColorPicker componentInParent = jsc.colorPicker.GetComponentInParent<UIDynamicColorPicker>();
			if (componentInParent != null)
			{
				this.colorPickerToJSONStorableColor.Remove(componentInParent);
				Transform transform = componentInParent.transform;
				this.rightUIElements.Remove(transform);
				this.leftUIElements.Remove(transform);
				jsc.colorPicker = null;
				UnityEngine.Object.Destroy(transform.gameObject);
			}
		}
	}

	// Token: 0x06005B1C RID: 23324 RVA: 0x002175C8 File Offset: 0x002159C8
	public void RemoveColorPicker(UIDynamicColorPicker dcolor)
	{
		JSONStorableColor jsonstorableColor;
		if (this.colorPickerToJSONStorableColor.TryGetValue(dcolor, out jsonstorableColor))
		{
			this.colorPickerToJSONStorableColor.Remove(dcolor);
			Transform transform = dcolor.transform;
			this.rightUIElements.Remove(transform);
			this.leftUIElements.Remove(transform);
			jsonstorableColor.colorPicker = null;
			UnityEngine.Object.Destroy(transform.gameObject);
		}
	}

	// Token: 0x06005B1D RID: 23325 RVA: 0x00217628 File Offset: 0x00215A28
	public UIDynamicTextField CreateTextField(JSONStorableString jss, bool rightSide = false)
	{
		UIDynamicTextField uidynamicTextField = null;
		if (this.manager != null && this.manager.configurableTextFieldPrefab != null && jss.text == null)
		{
			Transform transform = this.CreateUIElement(this.manager.configurableTextFieldPrefab.transform, rightSide);
			if (transform != null)
			{
				uidynamicTextField = transform.GetComponent<UIDynamicTextField>();
				if (uidynamicTextField != null)
				{
					this.textFieldToJSONStorableString.Add(uidynamicTextField, jss);
					jss.dynamicText = uidynamicTextField;
				}
			}
		}
		return uidynamicTextField;
	}

	// Token: 0x06005B1E RID: 23326 RVA: 0x002176BC File Offset: 0x00215ABC
	public void RemoveTextField(JSONStorableString jss)
	{
		if (jss != null && jss.text != null)
		{
			UIDynamicTextField dynamicText = jss.dynamicText;
			if (dynamicText != null)
			{
				this.textFieldToJSONStorableString.Remove(dynamicText);
				Transform transform = dynamicText.transform;
				this.rightUIElements.Remove(transform);
				this.leftUIElements.Remove(transform);
				jss.text = null;
				UnityEngine.Object.Destroy(transform.gameObject);
			}
		}
	}

	// Token: 0x06005B1F RID: 23327 RVA: 0x00217734 File Offset: 0x00215B34
	public void RemoveTextField(UIDynamicTextField dtext)
	{
		JSONStorableString jsonstorableString;
		if (this.textFieldToJSONStorableString.TryGetValue(dtext, out jsonstorableString))
		{
			this.textFieldToJSONStorableString.Remove(dtext);
			Transform transform = dtext.transform;
			this.rightUIElements.Remove(transform);
			this.leftUIElements.Remove(transform);
			jsonstorableString.text = null;
			UnityEngine.Object.Destroy(transform.gameObject);
		}
	}

	// Token: 0x06005B20 RID: 23328 RVA: 0x00217794 File Offset: 0x00215B94
	public UIDynamicPopup CreatePopup(JSONStorableStringChooser jsc, bool rightSide = false)
	{
		UIDynamicPopup uidynamicPopup = null;
		if (this.manager != null && this.manager.configurablePopupPrefab != null && jsc.popup == null)
		{
			Transform transform = this.CreateUIElement(this.manager.configurablePopupPrefab.transform, rightSide);
			if (transform != null)
			{
				uidynamicPopup = transform.GetComponent<UIDynamicPopup>();
				if (uidynamicPopup != null)
				{
					this.popupToJSONStorableStringChooser.Add(uidynamicPopup, jsc);
					uidynamicPopup.label = jsc.name;
					jsc.popup = uidynamicPopup.popup;
				}
			}
		}
		return uidynamicPopup;
	}

	// Token: 0x06005B21 RID: 23329 RVA: 0x00217838 File Offset: 0x00215C38
	public UIDynamicPopup CreateScrollablePopup(JSONStorableStringChooser jsc, bool rightSide = false)
	{
		UIDynamicPopup uidynamicPopup = null;
		if (this.manager != null && this.manager.configurableScrollablePopupPrefab != null && jsc.popup == null)
		{
			Transform transform = this.CreateUIElement(this.manager.configurableScrollablePopupPrefab.transform, rightSide);
			if (transform != null)
			{
				uidynamicPopup = transform.GetComponent<UIDynamicPopup>();
				if (uidynamicPopup != null)
				{
					this.popupToJSONStorableStringChooser.Add(uidynamicPopup, jsc);
					uidynamicPopup.label = jsc.name;
					jsc.popup = uidynamicPopup.popup;
				}
			}
		}
		return uidynamicPopup;
	}

	// Token: 0x06005B22 RID: 23330 RVA: 0x002178DC File Offset: 0x00215CDC
	public UIDynamicPopup CreateFilterablePopup(JSONStorableStringChooser jsc, bool rightSide = false)
	{
		UIDynamicPopup uidynamicPopup = null;
		if (this.manager != null && this.manager.configurableFilterablePopupPrefab != null && jsc.popup == null)
		{
			Transform transform = this.CreateUIElement(this.manager.configurableFilterablePopupPrefab.transform, rightSide);
			if (transform != null)
			{
				uidynamicPopup = transform.GetComponent<UIDynamicPopup>();
				if (uidynamicPopup != null)
				{
					this.popupToJSONStorableStringChooser.Add(uidynamicPopup, jsc);
					uidynamicPopup.label = jsc.name;
					jsc.popup = uidynamicPopup.popup;
				}
			}
		}
		return uidynamicPopup;
	}

	// Token: 0x06005B23 RID: 23331 RVA: 0x00217980 File Offset: 0x00215D80
	public void RemovePopup(JSONStorableStringChooser jsc)
	{
		if (jsc != null && jsc.popup != null)
		{
			UIDynamicPopup component = jsc.popup.GetComponent<UIDynamicPopup>();
			if (component != null)
			{
				this.popupToJSONStorableStringChooser.Remove(component);
			}
			Transform transform = jsc.popup.transform;
			this.rightUIElements.Remove(transform);
			this.leftUIElements.Remove(transform);
			jsc.popup = null;
			UnityEngine.Object.Destroy(transform.gameObject);
		}
	}

	// Token: 0x06005B24 RID: 23332 RVA: 0x00217A04 File Offset: 0x00215E04
	public void RemovePopup(UIDynamicPopup dpopup)
	{
		JSONStorableStringChooser jsonstorableStringChooser;
		if (this.popupToJSONStorableStringChooser.TryGetValue(dpopup, out jsonstorableStringChooser))
		{
			this.popupToJSONStorableStringChooser.Remove(dpopup);
			Transform transform = jsonstorableStringChooser.popup.transform;
			this.rightUIElements.Remove(transform);
			this.leftUIElements.Remove(transform);
			jsonstorableStringChooser.popup = null;
			UnityEngine.Object.Destroy(transform.gameObject);
		}
	}

	// Token: 0x06005B25 RID: 23333 RVA: 0x00217A6C File Offset: 0x00215E6C
	public UIDynamicButton CreateButton(string label, bool rightSide = false)
	{
		UIDynamicButton uidynamicButton = null;
		if (this.manager != null && this.manager.configurableButtonPrefab != null)
		{
			Transform transform = this.CreateUIElement(this.manager.configurableButtonPrefab.transform, rightSide);
			if (transform != null)
			{
				uidynamicButton = transform.GetComponent<UIDynamicButton>();
				if (uidynamicButton != null)
				{
					uidynamicButton.label = label;
				}
			}
		}
		return uidynamicButton;
	}

	// Token: 0x06005B26 RID: 23334 RVA: 0x00217AE1 File Offset: 0x00215EE1
	public void RemoveButton(UIDynamicButton button)
	{
		if (button != null)
		{
			this.rightUIElements.Remove(button.transform);
			this.leftUIElements.Remove(button.transform);
			UnityEngine.Object.Destroy(button.gameObject);
		}
	}

	// Token: 0x06005B27 RID: 23335 RVA: 0x00217B20 File Offset: 0x00215F20
	public UIDynamic CreateSpacer(bool rightSide = false)
	{
		UIDynamic result = null;
		if (this.manager != null && this.manager.configurableSpacerPrefab != null)
		{
			Transform transform = this.CreateUIElement(this.manager.configurableSpacerPrefab.transform, rightSide);
			if (transform != null)
			{
				result = transform.GetComponent<UIDynamic>();
			}
		}
		return result;
	}

	// Token: 0x06005B28 RID: 23336 RVA: 0x00217B82 File Offset: 0x00215F82
	public void RemoveSpacer(UIDynamic spacer)
	{
		if (spacer != null)
		{
			this.rightUIElements.Remove(spacer.transform);
			this.leftUIElements.Remove(spacer.transform);
			UnityEngine.Object.Destroy(spacer.gameObject);
		}
	}

	// Token: 0x06005B29 RID: 23337 RVA: 0x00217BBF File Offset: 0x00215FBF
	public virtual void Init()
	{
	}

	// Token: 0x06005B2A RID: 23338 RVA: 0x00217BC4 File Offset: 0x00215FC4
	protected Transform CreateUIElement(Transform prefab, bool rightSide = false)
	{
		Transform transform = null;
		if (prefab != null)
		{
			transform = UnityEngine.Object.Instantiate<Transform>(prefab);
			bool flag = false;
			if (rightSide)
			{
				if (this.rightUIContent != null)
				{
					flag = true;
					transform.SetParent(this.rightUIContent, false);
				}
			}
			else if (this.leftUIContent != null)
			{
				flag = true;
				transform.SetParent(this.leftUIContent, false);
			}
			if (flag)
			{
				transform.gameObject.SetActive(true);
			}
			else
			{
				transform.gameObject.SetActive(false);
			}
			if (rightSide)
			{
				this.rightUIElements.Add(transform);
			}
			else
			{
				this.leftUIElements.Add(transform);
			}
		}
		return transform;
	}

	// Token: 0x06005B2B RID: 23339 RVA: 0x00217C7A File Offset: 0x0021607A
	protected void SyncEnabled(bool b)
	{
		base.enabled = b;
	}

	// Token: 0x06005B2C RID: 23340 RVA: 0x00217C83 File Offset: 0x00216083
	protected void SyncLabel(string s)
	{
	}

	// Token: 0x06005B2D RID: 23341 RVA: 0x00217C88 File Offset: 0x00216088
	protected void InitInternal()
	{
		this.rightUIElements = new List<Transform>();
		this.leftUIElements = new List<Transform>();
		this.enabledJSON = new JSONStorableBool("enabled", true, new JSONStorableBool.SetBoolCallback(this.SyncEnabled));
		base.RegisterBool(this.enabledJSON);
		this.pluginLabelJSON = new JSONStorableString("pluginLabel", string.Empty, new JSONStorableString.SetStringCallback(this.SyncLabel));
		base.RegisterString(this.pluginLabelJSON);
		this.sliderToJSONStorableFloat = new Dictionary<UIDynamicSlider, JSONStorableFloat>();
		this.toggleToJSONStorableBool = new Dictionary<UIDynamicToggle, JSONStorableBool>();
		this.colorPickerToJSONStorableColor = new Dictionary<UIDynamicColorPicker, JSONStorableColor>();
		this.textFieldToJSONStorableString = new Dictionary<UIDynamicTextField, JSONStorableString>();
		this.popupToJSONStorableStringChooser = new Dictionary<UIDynamicPopup, JSONStorableStringChooser>();
	}

	// Token: 0x06005B2E RID: 23342 RVA: 0x00217D38 File Offset: 0x00216138
	public override void InitUI()
	{
		if (this.UITransform != null)
		{
			MVRScriptUI componentInChildren = this.UITransform.GetComponentInChildren<MVRScriptUI>();
			if (componentInChildren != null)
			{
				if (componentInChildren.rightUIContent != null)
				{
					this.rightUIContent = componentInChildren.rightUIContent;
					IEnumerator enumerator = componentInChildren.rightUIContent.transform.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							Transform transform = (Transform)obj;
							transform.SetParent(null);
						}
					}
					finally
					{
						IDisposable disposable;
						if ((disposable = (enumerator as IDisposable)) != null)
						{
							disposable.Dispose();
						}
					}
					foreach (Transform transform2 in this.rightUIElements)
					{
						transform2.gameObject.SetActive(true);
						transform2.SetParent(componentInChildren.rightUIContent, false);
					}
				}
				if (componentInChildren.leftUIContent != null)
				{
					this.leftUIContent = componentInChildren.leftUIContent;
					foreach (Transform transform3 in this.leftUIElements)
					{
						transform3.gameObject.SetActive(true);
						transform3.SetParent(componentInChildren.leftUIContent, false);
					}
				}
			}
		}
	}

	// Token: 0x06005B2F RID: 23343 RVA: 0x00217EC8 File Offset: 0x002162C8
	public override void InitUIAlt()
	{
	}

	// Token: 0x06005B30 RID: 23344 RVA: 0x00217ECA File Offset: 0x002162CA
	public void ForceAwake()
	{
		this.Awake();
	}

	// Token: 0x06005B31 RID: 23345 RVA: 0x00217ED2 File Offset: 0x002162D2
	protected override void Awake()
	{
		if (!this.awakecalled)
		{
			base.Awake();
			this.InitInternal();
			this.InitUI();
			this.InitUIAlt();
		}
	}

	// Token: 0x04004B1F RID: 19231
	public MVRPluginManager manager;

	// Token: 0x04004B20 RID: 19232
	protected Dictionary<UIDynamicSlider, JSONStorableFloat> sliderToJSONStorableFloat;

	// Token: 0x04004B21 RID: 19233
	protected Dictionary<UIDynamicToggle, JSONStorableBool> toggleToJSONStorableBool;

	// Token: 0x04004B22 RID: 19234
	protected Dictionary<UIDynamicColorPicker, JSONStorableColor> colorPickerToJSONStorableColor;

	// Token: 0x04004B23 RID: 19235
	protected Dictionary<UIDynamicTextField, JSONStorableString> textFieldToJSONStorableString;

	// Token: 0x04004B24 RID: 19236
	protected Dictionary<UIDynamicPopup, JSONStorableStringChooser> popupToJSONStorableStringChooser;

	// Token: 0x04004B25 RID: 19237
	protected List<Transform> rightUIElements;

	// Token: 0x04004B26 RID: 19238
	protected List<Transform> leftUIElements;

	// Token: 0x04004B27 RID: 19239
	protected RectTransform rightUIContent;

	// Token: 0x04004B28 RID: 19240
	protected RectTransform leftUIContent;

	// Token: 0x04004B29 RID: 19241
	public JSONStorableBool enabledJSON;

	// Token: 0x04004B2A RID: 19242
	public JSONStorableString pluginLabelJSON;
}
