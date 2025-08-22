using System;
using UnityEngine.UI;

// Token: 0x02000B7C RID: 2940
public class AudioSourceControlUI : UIProvider
{
	// Token: 0x060052BD RID: 21181 RVA: 0x001DEBE4 File Offset: 0x001DCFE4
	public AudioSourceControlUI()
	{
	}

	// Token: 0x04004285 RID: 17029
	public Toggle loopToggle;

	// Token: 0x04004286 RID: 17030
	public Slider volumeSlider;

	// Token: 0x04004287 RID: 17031
	public Slider pitchSlider;

	// Token: 0x04004288 RID: 17032
	public Slider stereoPanSlider;

	// Token: 0x04004289 RID: 17033
	public Slider minDistanceSlider;

	// Token: 0x0400428A RID: 17034
	public Slider maxDistanceSlider;

	// Token: 0x0400428B RID: 17035
	public Slider spatialBlendSlider;

	// Token: 0x0400428C RID: 17036
	public Slider stereoSpreadSlider;

	// Token: 0x0400428D RID: 17037
	public Toggle spatializeToggle;

	// Token: 0x0400428E RID: 17038
	public UIPopup audioRolloffModePopup;

	// Token: 0x0400428F RID: 17039
	public Slider delayBetweenQueuedClipsSlider;

	// Token: 0x04004290 RID: 17040
	public Slider volumeTriggerQuicknessSlider;

	// Token: 0x04004291 RID: 17041
	public Slider volumeTriggerMultiplierSlider;

	// Token: 0x04004292 RID: 17042
	public Slider recentMaxVolumeSlider;

	// Token: 0x04004293 RID: 17043
	public Toggle equalizeVolumeSlider;

	// Token: 0x04004294 RID: 17044
	public Text playingClipNameText;

	// Token: 0x04004295 RID: 17045
	public Button startMicrophoneInputButton;

	// Token: 0x04004296 RID: 17046
	public Button endMicrophoneInputButton;
}
