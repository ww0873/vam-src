using System;
using UnityEngine.UI;

// Token: 0x02000B62 RID: 2914
public class AnimationStepUI : UIProvider
{
	// Token: 0x0600515F RID: 20831 RVA: 0x001D5B67 File Offset: 0x001D3F67
	public AnimationStepUI()
	{
	}

	// Token: 0x04004116 RID: 16662
	public Slider transitionToTimeSlider;

	// Token: 0x04004117 RID: 16663
	public UIPopup curveTypePopup;

	// Token: 0x04004118 RID: 16664
	public Button createStepBeforeButton;

	// Token: 0x04004119 RID: 16665
	public Button createStepAfterButton;

	// Token: 0x0400411A RID: 16666
	public Button alignPositionToRootButton;

	// Token: 0x0400411B RID: 16667
	public Button alignRotationToRootButton;

	// Token: 0x0400411C RID: 16668
	public Button alignPositionToReceiverButton;

	// Token: 0x0400411D RID: 16669
	public Button alignRotationToReceiverButton;

	// Token: 0x0400411E RID: 16670
	public Button removeStepButton;
}
