using System;
using UnityEngine;

// Token: 0x02000DA2 RID: 3490
public interface TriggerHandler
{
	// Token: 0x06006BA0 RID: 27552
	void RemoveTrigger(Trigger t);

	// Token: 0x06006BA1 RID: 27553
	void DuplicateTrigger(Trigger t);

	// Token: 0x06006BA2 RID: 27554
	RectTransform CreateTriggerActionsUI();

	// Token: 0x06006BA3 RID: 27555
	RectTransform CreateTriggerActionMiniUI();

	// Token: 0x06006BA4 RID: 27556
	RectTransform CreateTriggerActionDiscreteUI();

	// Token: 0x06006BA5 RID: 27557
	RectTransform CreateTriggerActionTransitionUI();

	// Token: 0x06006BA6 RID: 27558
	void RemoveTriggerActionUI(RectTransform rt);
}
