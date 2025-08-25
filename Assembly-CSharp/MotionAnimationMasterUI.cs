using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B6A RID: 2922
public class MotionAnimationMasterUI : UIProvider
{
	// Token: 0x06005204 RID: 20996 RVA: 0x001DA6F4 File Offset: 0x001D8AF4
	public MotionAnimationMasterUI()
	{
	}

	// Token: 0x0400419C RID: 16796
	public Toggle linkToAudioSourceControlToggle;

	// Token: 0x0400419D RID: 16797
	public Slider audioSourceTimeOffsetSlider;

	// Token: 0x0400419E RID: 16798
	public UIPopup audioSourceControlAtomSelectionPopup;

	// Token: 0x0400419F RID: 16799
	public UIPopup audioSourceControlSelectionPopup;

	// Token: 0x040041A0 RID: 16800
	public Slider playbackCounterSlider;

	// Token: 0x040041A1 RID: 16801
	public Slider startTimestepSlider;

	// Token: 0x040041A2 RID: 16802
	public Slider stopTimestepSlider;

	// Token: 0x040041A3 RID: 16803
	public Slider loopbackTimeSlider;

	// Token: 0x040041A4 RID: 16804
	public Slider playbackSpeedSlider;

	// Token: 0x040041A5 RID: 16805
	public Toggle autoPlayToggle;

	// Token: 0x040041A6 RID: 16806
	public Toggle loopToggle;

	// Token: 0x040041A7 RID: 16807
	public Toggle autoRecordStopToggle;

	// Token: 0x040041A8 RID: 16808
	public Toggle showRecordPathsToggle;

	// Token: 0x040041A9 RID: 16809
	public Toggle showStartMarkersToggle;

	// Token: 0x040041AA RID: 16810
	public Button clearAllAnimationButton;

	// Token: 0x040041AB RID: 16811
	public Button selectControllersArmedForRecordButton;

	// Token: 0x040041AC RID: 16812
	public Button armAllControlledControllersForRecordButton;

	// Token: 0x040041AD RID: 16813
	public Button startRecordModeButton;

	// Token: 0x040041AE RID: 16814
	public Button startRecordButton;

	// Token: 0x040041AF RID: 16815
	public Button stopRecordButton;

	// Token: 0x040041B0 RID: 16816
	public Button stopRecordModeButton;

	// Token: 0x040041B1 RID: 16817
	public Button startPlaybackButton;

	// Token: 0x040041B2 RID: 16818
	public Button stopPlaybackButton;

	// Token: 0x040041B3 RID: 16819
	public Button trimAnimationButton;

	// Token: 0x040041B4 RID: 16820
	public Slider desiredLengthSlider;

	// Token: 0x040041B5 RID: 16821
	public Button setToDesiredLengthButton;

	// Token: 0x040041B6 RID: 16822
	public Button seekToBeginningButton;

	// Token: 0x040041B7 RID: 16823
	public Button resetAnimationButton;

	// Token: 0x040041B8 RID: 16824
	public GameObject advancedPanel;

	// Token: 0x040041B9 RID: 16825
	public Button openAdvancedPanelButton;

	// Token: 0x040041BA RID: 16826
	public Button closeAdvancedPanelButton;

	// Token: 0x040041BB RID: 16827
	public Button selectTriggersInTimeRangeButton;

	// Token: 0x040041BC RID: 16828
	public Button clearSelectedTriggersButton;

	// Token: 0x040041BD RID: 16829
	public Button adjustTimeOfSelectedTriggersButton;

	// Token: 0x040041BE RID: 16830
	public Button sortTriggersByStartTimeButton;

	// Token: 0x040041BF RID: 16831
	public Button copySelectedTriggersAndPasteToTimeButton;

	// Token: 0x040041C0 RID: 16832
	public Button copyFromSceneMasterButton;

	// Token: 0x040041C1 RID: 16833
	public Button copyToSceneMasterButton;

	// Token: 0x040041C2 RID: 16834
	public Slider triggerSelectFromTimeSlider;

	// Token: 0x040041C3 RID: 16835
	public Slider triggerSelectToTimeSlider;

	// Token: 0x040041C4 RID: 16836
	public Slider triggerTimeAdjustmentSlider;

	// Token: 0x040041C5 RID: 16837
	public Slider triggerPasteToTimeSlider;

	// Token: 0x040041C6 RID: 16838
	public RectTransform triggerActionsParent;

	// Token: 0x040041C7 RID: 16839
	public ScrollRectContentManager triggerContentManager;

	// Token: 0x040041C8 RID: 16840
	public Button addTriggerButton;

	// Token: 0x040041C9 RID: 16841
	public Button clearAllTriggersButton;
}
