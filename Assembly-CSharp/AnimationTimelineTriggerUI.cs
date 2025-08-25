using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000D98 RID: 3480
public class AnimationTimelineTriggerUI : TriggerUI
{
	// Token: 0x06006B47 RID: 27463 RVA: 0x00287FC0 File Offset: 0x002863C0
	public AnimationTimelineTriggerUI()
	{
	}

	// Token: 0x04005D19 RID: 23833
	public Slider triggerStartTimeSlider;

	// Token: 0x04005D1A RID: 23834
	public Slider triggerEndTimeSlider;

	// Token: 0x04005D1B RID: 23835
	public Button startTimeToCurrentTimeButton;

	// Token: 0x04005D1C RID: 23836
	public Button endTimeToCurrentTimeButton;

	// Token: 0x04005D1D RID: 23837
	public Toggle selectToggle;

	// Token: 0x04005D1E RID: 23838
	public GameObject selectedIndicator;
}
