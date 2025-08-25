using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

// Token: 0x02000DAB RID: 3499
public class TriggerActionDiscreteUI : TriggerActionUI
{
	// Token: 0x06006C82 RID: 27778 RVA: 0x0028EBA6 File Offset: 0x0028CFA6
	public TriggerActionDiscreteUI()
	{
	}

	// Token: 0x04005DE9 RID: 24041
	public Button copyButton;

	// Token: 0x04005DEA RID: 24042
	public Button pasteButton;

	// Token: 0x04005DEB RID: 24043
	public Button testButton;

	// Token: 0x04005DEC RID: 24044
	public Button resetTestButton;

	// Token: 0x04005DED RID: 24045
	public RectTransform audioClipPopupsContainer;

	// Token: 0x04005DEE RID: 24046
	public UIPopup audioClipTypePopup;

	// Token: 0x04005DEF RID: 24047
	public UIPopup audioClipCategoryPopup;

	// Token: 0x04005DF0 RID: 24048
	public UIPopup audioClipPopup;

	// Token: 0x04005DF1 RID: 24049
	public UIPopup stringChooserActionPopup;

	// Token: 0x04005DF2 RID: 24050
	public Button chooseSceneFilePathButton;

	// Token: 0x04005DF3 RID: 24051
	public Button choosePresetFilePathButton;

	// Token: 0x04005DF4 RID: 24052
	public Text sceneFilePathText;

	// Token: 0x04005DF5 RID: 24053
	public Text presetFilePathText;

	// Token: 0x04005DF6 RID: 24054
	public Toggle boolValueToggle;

	// Token: 0x04005DF7 RID: 24055
	public UIDynamicSlider floatValueDynamicSlider;

	// Token: 0x04005DF8 RID: 24056
	public Slider floatValueSlider;

	// Token: 0x04005DF9 RID: 24057
	public Toggle useTimerToggle;

	// Token: 0x04005DFA RID: 24058
	public RectTransform curveContainer;

	// Token: 0x04005DFB RID: 24059
	public UILineRenderer curveLineRenderer;

	// Token: 0x04005DFC RID: 24060
	public Slider timerLengthSlider;

	// Token: 0x04005DFD RID: 24061
	public UIPopup timerTypePopup;

	// Token: 0x04005DFE RID: 24062
	public Toggle useSecondTimerPointToggle;

	// Token: 0x04005DFF RID: 24063
	public UIDynamicSlider secondTimerPointValueDynamicSlider;

	// Token: 0x04005E00 RID: 24064
	public Slider secondTimerPointValueSlider;

	// Token: 0x04005E01 RID: 24065
	public Slider secondTimerPointCurveLocationSlider;

	// Token: 0x04005E02 RID: 24066
	public InputField stringValueField;

	// Token: 0x04005E03 RID: 24067
	public InputFieldAction stringValueFieldAction;

	// Token: 0x04005E04 RID: 24068
	public RectTransform colorPickerContainer;

	// Token: 0x04005E05 RID: 24069
	public HSVColorPicker colorPicker;

	// Token: 0x04005E06 RID: 24070
	public UIPopup stringChooserValuePopup;
}
