using System;
using UnityEngine.UI;

// Token: 0x02000D9E RID: 3486
public class CollisionTriggerUI : UIProvider
{
	// Token: 0x06006B85 RID: 27525 RVA: 0x002894E2 File Offset: 0x002878E2
	public CollisionTriggerUI()
	{
	}

	// Token: 0x04005D43 RID: 23875
	public Toggle triggerEnabledToggle;

	// Token: 0x04005D44 RID: 23876
	public UIPopup atomFilterPopup;

	// Token: 0x04005D45 RID: 23877
	public Toggle invertAtomFilterToggle;

	// Token: 0x04005D46 RID: 23878
	public Toggle useRelativeVelocityFilterToggle;

	// Token: 0x04005D47 RID: 23879
	public Toggle invertRelativeVelocityFilterToggle;

	// Token: 0x04005D48 RID: 23880
	public Slider relativeVelocityFilterSlider;

	// Token: 0x04005D49 RID: 23881
	public Slider lastRelativeVelocitySlider;

	// Token: 0x04005D4A RID: 23882
	public Button createDefaultSoundActionButton;
}
