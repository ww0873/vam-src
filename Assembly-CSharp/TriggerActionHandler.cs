using System;
using UnityEngine;

// Token: 0x02000DA3 RID: 3491
public interface TriggerActionHandler
{
	// Token: 0x06006BA7 RID: 27559
	RectTransform CreateTriggerActionDiscreteUI();

	// Token: 0x06006BA8 RID: 27560
	RectTransform CreateTriggerActionTransitionUI();

	// Token: 0x06006BA9 RID: 27561
	void RemoveTriggerAction(TriggerAction ta);

	// Token: 0x06006BAA RID: 27562
	void DuplicateTriggerAction(TriggerAction ta);

	// Token: 0x06006BAB RID: 27563
	void TriggerActionNameChange(TriggerAction ta);

	// Token: 0x06006BAC RID: 27564
	void SetHasActiveTimer(TriggerActionDiscrete tad, bool hasActiveTimer);
}
