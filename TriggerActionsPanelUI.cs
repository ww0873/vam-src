using System;
using UnityEngine.UI;

// Token: 0x02000DAD RID: 3501
public class TriggerActionsPanelUI : UIProvider
{
	// Token: 0x06006C84 RID: 27780 RVA: 0x0028EBB6 File Offset: 0x0028CFB6
	public TriggerActionsPanelUI()
	{
	}

	// Token: 0x04005E0E RID: 24078
	public Text triggerDisplayNameText;

	// Token: 0x04005E0F RID: 24079
	public Button closeTriggerActionsPanelButton;

	// Token: 0x04005E10 RID: 24080
	public Button clearActionsButtons;

	// Token: 0x04005E11 RID: 24081
	public Button addDiscreteActionStartButton;

	// Token: 0x04005E12 RID: 24082
	public Button addTransitionActionButton;

	// Token: 0x04005E13 RID: 24083
	public Button addDiscreteActionEndButton;

	// Token: 0x04005E14 RID: 24084
	public Button copyDiscreteActionsStartButton;

	// Token: 0x04005E15 RID: 24085
	public Button pasteDiscreteActionsStartButton;

	// Token: 0x04005E16 RID: 24086
	public Button copyTransitionActionsButton;

	// Token: 0x04005E17 RID: 24087
	public Button pasteTransitionActionsButton;

	// Token: 0x04005E18 RID: 24088
	public Button copyDiscreteActionsEndButton;

	// Token: 0x04005E19 RID: 24089
	public Button pasteDiscreteActionsEndButton;

	// Token: 0x04005E1A RID: 24090
	public ScrollRectContentManager discreteActionsStartContentManager;

	// Token: 0x04005E1B RID: 24091
	public ScrollRectContentManager transitionActionsContentManager;

	// Token: 0x04005E1C RID: 24092
	public ScrollRectContentManager discreteActionsEndContentManager;
}
