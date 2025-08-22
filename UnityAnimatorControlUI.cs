using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B77 RID: 2935
public class UnityAnimatorControlUI : UIProvider
{
	// Token: 0x06005260 RID: 21088 RVA: 0x001DC032 File Offset: 0x001DA432
	public UnityAnimatorControlUI()
	{
	}

	// Token: 0x0400422A RID: 16938
	public Button animatorResetButton;

	// Token: 0x0400422B RID: 16939
	public Toggle animatorEnabledToggle;

	// Token: 0x0400422C RID: 16940
	public GameObject animatorIsPlayingIndicator;

	// Token: 0x0400422D RID: 16941
	public Button animatorPlayButton;

	// Token: 0x0400422E RID: 16942
	public Button animatorPauseButton;

	// Token: 0x0400422F RID: 16943
	public Slider animatorSpeedSlider;

	// Token: 0x04004230 RID: 16944
	public UIPopup animationSelectionPopup;

	// Token: 0x04004231 RID: 16945
	public Toggle useCrossFadeToggle;

	// Token: 0x04004232 RID: 16946
	public Slider crossFadeTimeSlider;

	// Token: 0x04004233 RID: 16947
	public Text currentAnimationNameText;

	// Token: 0x04004234 RID: 16948
	public RectTransform sequenceContainer;

	// Token: 0x04004235 RID: 16949
	public Toggle loopSequenceToggle;

	// Token: 0x04004236 RID: 16950
	public Button restartAnimationSequenceButton;

	// Token: 0x04004237 RID: 16951
	public Button addAnimationToSequenceButton;

	// Token: 0x04004238 RID: 16952
	public Button clearAndAddAnimationToSequenceButton;

	// Token: 0x04004239 RID: 16953
	public Button clearSequenceButton;

	// Token: 0x0400423A RID: 16954
	public Button nextClipInSequenceButton;

	// Token: 0x0400423B RID: 16955
	public Button previousClipInSequenceButton;

	// Token: 0x0400423C RID: 16956
	public Slider animationRotationSpeedSlider;

	// Token: 0x0400423D RID: 16957
	public Slider animationRotationDegressForActionSlider;

	// Token: 0x0400423E RID: 16958
	public Button animationRotateButton;
}
