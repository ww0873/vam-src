using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000B53 RID: 2899
public class AnimationPatternUI : UIProvider
{
	// Token: 0x06005102 RID: 20738 RVA: 0x001D4731 File Offset: 0x001D2B31
	public AnimationPatternUI()
	{
	}

	// Token: 0x040040CE RID: 16590
	public Toggle onToggle;

	// Token: 0x040040CF RID: 16591
	public Toggle autoPlayToggle;

	// Token: 0x040040D0 RID: 16592
	public Toggle pauseToggle;

	// Token: 0x040040D1 RID: 16593
	public Toggle loopToggle;

	// Token: 0x040040D2 RID: 16594
	public Toggle loopOnceToggle;

	// Token: 0x040040D3 RID: 16595
	public UIPopup curveTypeSelector;

	// Token: 0x040040D4 RID: 16596
	public Slider speedSlider;

	// Token: 0x040040D5 RID: 16597
	public Slider currentTimeSlider;

	// Token: 0x040040D6 RID: 16598
	public Toggle autoSyncStepNamesToggle;

	// Token: 0x040040D7 RID: 16599
	public Toggle hideCurveUnlessSelectedToggle;

	// Token: 0x040040D8 RID: 16600
	public RectTransform triggerActionsParent;

	// Token: 0x040040D9 RID: 16601
	public ScrollRectContentManager triggerContentManager;

	// Token: 0x040040DA RID: 16602
	public Button addTriggerButton;

	// Token: 0x040040DB RID: 16603
	public Button clearAllTriggersButton;

	// Token: 0x040040DC RID: 16604
	public Button createStepAtEndButton;

	// Token: 0x040040DD RID: 16605
	public Button resetAnimationButton;

	// Token: 0x040040DE RID: 16606
	public Button playButton;

	// Token: 0x040040DF RID: 16607
	public Button hideAllStepsButton;

	// Token: 0x040040E0 RID: 16608
	public Button unhideAllStepsButton;

	// Token: 0x040040E1 RID: 16609
	public Button parentAllStepsButton;

	// Token: 0x040040E2 RID: 16610
	public Button unparentAllStepsButton;
}
